Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the Compensation Channel subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public MustInherit Class CompensateChannelSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class.
    ''' </summary>
    ''' <remarks> David, 7/6/2016. </remarks>
    ''' <param name="compensationType"> The Compensation Type. </param>
    ''' <param name="channelNumber">    The channel number. </param>
    ''' <param name="statusSubsystem">  The status subsystem. </param>
    Protected Sub New(ByVal compensationType As CompensationTypes, ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.ChannelNumber = channelNumber
        Me.ApplyCompensationType(compensationType)
    End Sub

#End Region

#Region " CHANNEL "

    ''' <summary> Gets or sets the channel number. </summary>
    ''' <value> The channel number. </value>
    Public ReadOnly Property ChannelNumber As Integer

#End Region

#Region " ARRAY <> STRING "

    ''' <summary> Parses an impedance array string. </summary>
    ''' <remarks> David, 7/7/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="InvalidCastException">  Thrown when an object cannot be cast to a required
    '''                                          type. </exception>
    ''' <param name="values"> The values. </param>
    ''' <returns>
    ''' An enumerator that allows for each to be used to process parse in this collection.
    ''' </returns>
    Public Shared Function Parse(ByVal values As String) As IEnumerable(Of Double)
        If String.IsNullOrEmpty(values) Then Throw New ArgumentNullException(NameOf(values))
        Dim v As New Queue(Of String)(values.Split(","c))
        Dim data As New List(Of Double)
        Do While v.Any
            Dim value As Double = 0
            Dim pop As String = v.Dequeue
            If Not Double.TryParse(pop, value) Then
                Throw New InvalidCastException($"Parse failed for value {pop}")
            End If
            data.Add(value)
        Loop
        Return data
    End Function

    ''' <summary> Builds an impedance array string. </summary>
    ''' <remarks> David, 7/7/2016. </remarks>
    ''' <param name="values"> The values. </param>
    ''' <returns> A String. </returns>
    Public Shared Function Build(ByVal values As IEnumerable(Of Double)) As String
        Dim builder As New System.Text.StringBuilder
        If values?.Any Then
            For Each v As Double In values
                If builder.Length > 0 Then builder.Append(",")
                builder.Append(v.ToString)
            Next
        End If
        Return builder.ToString
    End Function

    ''' <summary> Builds impedance array string. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    '''                                              null. </exception>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <param name="includesFrequency">      true if impedance array includes. </param>
    ''' <param name="lowFrequencyImpedance">  The low frequency impedance values. </param>
    ''' <param name="highFrequencyImpedance"> The high frequency values. </param>
    ''' <returns> A String. </returns>
    Public Shared Function Build(ByVal includesFrequency As Boolean,
                                 ByVal lowFrequencyImpedance As IEnumerable(Of Double),
                                 ByVal highFrequencyImpedance As IEnumerable(Of Double)) As String
        Return Build(Merge(includesFrequency, lowFrequencyImpedance, highFrequencyImpedance))
    End Function

    ''' <summary> Merges the impedance arrays into a single array. </summary>
    ''' <remarks> David, 7/7/2016. </remarks>
    ''' <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    '''                                              null. </exception>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <param name="includesFrequency">      true if impedance array includes. </param>
    ''' <param name="lowFrequencyImpedance">  The low frequency impedance values. </param>
    ''' <param name="highFrequencyImpedance"> The high frequency values. </param>
    ''' <returns>
    ''' An enumerator that allows for each to be used to process merge in this collection.
    ''' </returns>
    Public Shared Function Merge(ByVal includesFrequency As Boolean,
                                 ByVal lowFrequencyImpedance As IEnumerable(Of Double),
                                 ByVal highFrequencyImpedance As IEnumerable(Of Double)) As IEnumerable(Of Double)
        If lowFrequencyImpedance Is Nothing Then Throw New ArgumentNullException(NameOf(lowFrequencyImpedance))
        If highFrequencyImpedance Is Nothing Then Throw New ArgumentNullException(NameOf(highFrequencyImpedance))
        Dim startIndex As Integer = If(includesFrequency, 1, 0)
        If lowFrequencyImpedance.Count <> startIndex + 2 Then Throw New InvalidOperationException($"Low frequency array has {lowFrequencyImpedance.Count} values instead of {startIndex + 2}")
        If highFrequencyImpedance.Count <> startIndex + 2 Then Throw New InvalidOperationException($"High frequency array has {highFrequencyImpedance.Count} values instead of {startIndex + 2}")
        Dim l As New List(Of Double)
        l.Add(lowFrequencyImpedance(startIndex))
        l.Add(lowFrequencyImpedance(startIndex + 1))
        l.Add(highFrequencyImpedance(startIndex))
        l.Add(highFrequencyImpedance(startIndex + 1))
        Return l.ToArray
    End Function

    ''' <summary> Merge frequency and impedance arrays. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    '''                                              null. </exception>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <param name="frequencies"> The frequencies. </param>
    ''' <param name="impedances">  The impedances. </param>
    ''' <returns> A String. </returns>
    Public Shared Function Merge(ByVal frequencies As String, ByVal impedances As String) As String
        If String.IsNullOrEmpty(frequencies) Then Throw New ArgumentNullException(NameOf(frequencies))
        If String.IsNullOrEmpty(impedances) Then Throw New ArgumentNullException(NameOf(impedances))
        Dim builder As New System.Text.StringBuilder
        Dim f As New Queue(Of String)(frequencies.Split(","c))
        Dim v As New Queue(Of String)(impedances.Split(","c))
        If 2 * f.Count <> v.Count Then
            Throw New InvalidOperationException($"Number of values {v.Count} must be twice the number of frequencies {f.Count}")
        End If
        Do While f.Any
            If builder.Length > 0 Then
                builder.AppendFormat("{0}", f.Dequeue)
            Else
                builder.AppendFormat(",{0}", f.Dequeue)
            End If
            If v.Any Then builder.AppendFormat(",{0}", v.Dequeue)
            If v.Any Then builder.AppendFormat(",{0}", v.Dequeue)
        Loop
        Return builder.ToString
    End Function


#End Region

#Region " COMMANDS "

    ''' <summary> Gets the clear measurements command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:&lt;type&gt;:COLL:CLE" </remarks>
    ''' <value> The clear measurements command. </value>
    Protected Overridable ReadOnly Property ClearMeasurementsCommand As String

    ''' <summary> Clears the measured data. </summary>
    Public Sub ClearMeasurements()
        Me.Write(Me.ClearMeasurementsCommand)
        Me.FrequencyArray = New Double() {}
        Me.FrequencyArrayReading = ""
        Me.ImpedanceArray = New Double() {}
        Me.ImpedanceArrayReading = ""
        Me.Enabled = False
        Me.FrequencyStimulusPoints = New Integer?
    End Sub

    ''' <summary> Gets the Acquire measurements command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:COLL:ACQ:&lt;type&gt;" </remarks>
    ''' <value> The clear measurements command. </value>
    Protected Overridable ReadOnly Property AcquireMeasurementsCommand As String

    ''' <summary> Acquires the measured data. </summary>
    Public Sub AcquireMeasurements()
        Me.Write(Me.ClearMeasurementsCommand)
    End Sub


#End Region

#Region " COMPENSATION TYPE "

    Private _SupportedCompensationTypes As CompensationTypes
    ''' <summary>
    ''' Gets or sets the supported Compensation Type.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedCompensationTypes() As CompensationTypes
        Get
            Return _SupportedCompensationTypes
        End Get
        Set(ByVal value As CompensationTypes)
            If Not Me.SupportedCompensationTypes.Equals(value) Then
                Me._SupportedCompensationTypes = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SupportedCompensationTypes))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the compensation type code. </summary>
    ''' <value> The compensation type code. </value>
    Protected ReadOnly Property CompensationTypeCode As String

    ''' <summary> The Compensation Type. </summary>
    Private _CompensationType As CompensationTypes?

    Private Sub ApplyCompensationType(ByVal value As CompensationTypes)
        Me._CompensationType = value
        Me._CompensationTypeCode = value.ExtractBetween
    End Sub

    ''' <summary> Gets or sets the cached source CompensationType. </summary>
    ''' <value> The <see cref="CompensationTypes">source Compensation Type</see> or none if not set or
    ''' unknown. </value>
    Public Overridable Property CompensationType As CompensationTypes?
        Get
            Return Me._CompensationType
        End Get
        Protected Set(ByVal value As CompensationTypes?)
            If Not Nullable.Equals(Me.CompensationType, value) Then
                Me._CompensationType = value
                If value.HasValue Then
                    Me.ApplyCompensationType(value.Value)
                Else
                    Me._CompensationTypeCode = ""
                End If
                Me.AsyncNotifyPropertyChanged(NameOf(Me.CompensationType))
            End If
        End Set
    End Property

#End Region

#Region " ENABLED "

    Private _Enabled As Boolean?
    ''' <summary> Gets or sets the cached Enabled sentinel. </summary>
    ''' <value> <c>null</c> if  Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Enabled As Boolean?
        Get
            Return Me._Enabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Enabled, value) Then
                Me._Enabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Enabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the  Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteEnabled(value)
        Return Me.QueryEnabled()
    End Function

    ''' <summary> Gets the compensation enabled query command. </summary>
    ''' <value> The compensation enabled query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:&lt;type&gt;:STAT?" </remarks>
    Protected Overridable ReadOnly Property EnabledQueryCommand As String

    ''' <summary> Queries the compensation Enabled sentinel. Also sets the
    ''' <see cref="Enabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryEnabled() As Boolean?
        Me.Enabled = Me.Query(Me.Enabled, Me.EnabledQueryCommand)
        Return Me.Enabled
    End Function

    ''' <summary> Gets the enabled command Format. </summary>
    ''' <value> The compensation enabled query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:&lt;type&gt;:STAT {0:1;1;0}" </remarks>
    Protected Overridable ReadOnly Property EnabledCommandFormat As String

    ''' <summary> Writes the  Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteEnabled(ByVal value As Boolean) As Boolean?
        Me.Enabled = Me.Write(value, Me.EnabledCommandFormat)
        Return Me.Enabled
    End Function

#End Region

#Region " FREQUENCY STIMULUS POINTS "

    ''' <summary> The Frequency Stimulus Points. </summary>
    Private _FrequencyStimulusPoints As Integer?

    ''' <summary> Gets or sets the cached Frequency Stimulus Points. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property FrequencyStimulusPoints As Integer?
        Get
            Return Me._FrequencyStimulusPoints
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.FrequencyStimulusPoints, value) Then
                Me._FrequencyStimulusPoints = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.FrequencyStimulusPoints))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Frequency Stimulus Points query command. </summary>
    ''' <value> The Frequency Stimulus Points query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:ZME:&lt;type&gt;:POIN?" </remarks>
    Protected Overridable ReadOnly Property FrequencyStimulusPointsQueryCommand As String

    ''' <summary> Queries the Frequency Stimulus Points. </summary>
    ''' <returns> The Frequency Stimulus Points or none if unknown. </returns>
    Public Function QueryFrequencyStimulusPoints() As Integer?
        Me.FrequencyStimulusPoints = Me.Query(Me.FrequencyStimulusPoints, Me.FrequencyStimulusPointsQueryCommand)
        Return Me.FrequencyStimulusPoints
    End Function

#End Region

#Region " FREQUENCY ARRAY "

    ''' <summary> Gets the has complete compensation values. </summary>
    ''' <value> The has complete compensation values. </value>
    Public ReadOnly Property HasCompleteCompensationValues As Boolean
        Get
            Return Not (String.IsNullOrWhiteSpace(Me.FrequencyArrayReading) Or String.IsNullOrWhiteSpace(Me.ImpedanceArrayReading))
        End Get
    End Property

    Private _FrequencyArrayReading As String
    ''' <summary> Gets or sets the frequency array reading. </summary>
    ''' <value> The frequency array reading. </value>
    Public Property FrequencyArrayReading As String
        Get
            Return Me._FrequencyArrayReading
        End Get
        Protected Set(value As String)
            If Not String.Equals(value, Me.FrequencyArrayReading) Then
                Me._FrequencyArrayReading = value
                Me.AsyncNotifyPropertyChanged()
                Me.AsyncNotifyPropertyChanged(NameOf(Me.HasCompleteCompensationValues))
            End If
        End Set
    End Property

    ''' <summary> The Frequency Array. </summary>
    Private _FrequencyArray As IEnumerable(Of Double)

    ''' <summary> Gets or sets the cached Frequency Array. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property FrequencyArray As IEnumerable(Of Double)
        Get
            Return Me._FrequencyArray
        End Get
        Protected Set(ByVal value As IEnumerable(Of Double))
            If Not Nullable.Equals(Me.FrequencyArray, value) Then
                Me._FrequencyArray = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Frequency Array. </summary>
    ''' <param name="value"> The Frequency Array. </param>
    ''' <returns> The Frequency Array. </returns>
    Public Function ApplyFrequencyArray(ByVal value As IEnumerable(Of Double)) As IEnumerable(Of Double)
        Me.WriteFrequencyArray(value)
        Return Me.QueryFrequencyArray
    End Function

    ''' <summary> Gets or sets the Frequency Array query command. </summary>
    ''' <value> The Frequency Array query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:ZME:&lt;type&gt;:FREQ?" </remarks>
    Protected Overridable ReadOnly Property FrequencyArrayQueryCommand As String

    ''' <summary> Queries the Frequency Array. </summary>
    ''' <returns> The Frequency Array or none if unknown. </returns>
    Public Function QueryFrequencyArray() As IEnumerable(Of Double)
        Me.FrequencyArrayReading = Me.Query("", Me.FrequencyArrayQueryCommand)
        Me.FrequencyArray = Parse(Me._FrequencyArrayReading)
        Return Me.FrequencyArray
    End Function

    ''' <summary> Gets or sets the Frequency Array command format. </summary>
    ''' <value> The Frequency Array command format. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:ZME:&lt;type&gt;:FREQ {0}" </remarks>
    Protected Overridable ReadOnly Property FrequencyArrayCommandFormat As String

    ''' <summary> Writes the Frequency Array without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Frequency Array. </remarks>
    ''' <param name="values"> The Frequency Array. </param>
    ''' <returns> The Frequency Array. </returns>
    Public Function WriteFrequencyArray(ByVal values As IEnumerable(Of Double)) As IEnumerable(Of Double)
        Me.FrequencyArrayReading = Build(values)
        Me.Write(Me.FrequencyArrayCommandFormat, Me._FrequencyArrayReading)
        Me.FrequencyArray = values.ToArray
        Return Me.FrequencyArray
    End Function

#End Region

#Region " IMPEDANCE ARRAY "

    Private _ImpedanceArrayReading As String
    ''' <summary> Gets or sets the impedance array reading. </summary>
    ''' <value> The impedance array reading. </value>
    Public Property ImpedanceArrayReading As String
        Get
            Return Me._ImpedanceArrayReading
        End Get
        Protected Set(value As String)
            If Not String.Equals(value, Me.ImpedanceArrayReading) Then
                Me._ImpedanceArrayReading = value
                Me.AsyncNotifyPropertyChanged()
                Me.AsyncNotifyPropertyChanged(NameOf(Me.HasCompleteCompensationValues))
            End If
        End Set
    End Property

    ''' <summary> The Impedance Array. </summary>
    Private _ImpedanceArray As IEnumerable(Of Double)

    ''' <summary> Gets or sets the cached Impedance Array. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property ImpedanceArray As IEnumerable(Of Double)
        Get
            Return Me._ImpedanceArray
        End Get
        Protected Set(ByVal value As IEnumerable(Of Double))
            If Not Nullable.Equals(Me.ImpedanceArray, value) Then
                Me._ImpedanceArray = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Impedance Array. </summary>
    ''' <remarks> David, 7/9/2016. </remarks>
    ''' <param name="reading"> The reading. </param>
    ''' <returns> The Impedance Array. </returns>
    Public Function ApplyImpedanceArray(ByVal reading As String) As IEnumerable(Of Double)
        Me.WriteImpedanceArray(reading)
        Return Me.QueryImpedanceArray
    End Function

    ''' <summary> Writes and reads back the Impedance Array. </summary>
    ''' <param name="value"> The Impedance Array. </param>
    ''' <returns> The Impedance Array. </returns>
    Public Function ApplyImpedanceArray(ByVal value As IEnumerable(Of Double)) As IEnumerable(Of Double)
        Me.WriteImpedanceArray(value)
        Return Me.QueryImpedanceArray
    End Function

    ''' <summary> Gets or sets the Impedance Array query command. </summary>
    ''' <value> The Impedance Array query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:ZME:&lt;type&gt;:DATA?" </remarks>
    Protected Overridable ReadOnly Property ImpedanceArrayQueryCommand As String

    ''' <summary> Queries the Impedance Array. </summary>
    ''' <returns> The Impedance Array or none if unknown. </returns>
    Public Function QueryImpedanceArray() As IEnumerable(Of Double)
        Me.ImpedanceArrayReading = Me.Query("", Me.ImpedanceArrayQueryCommand)
        Me.ImpedanceArray = Parse(Me._ImpedanceArrayReading)
        Return Me.ImpedanceArray
    End Function

    ''' <summary> Gets or sets the Impedance Array command format. </summary>
    ''' <value> The Impedance Array command format. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:ZME:&lt;type&gt;:DATA {0}" </remarks>
    Protected Overridable ReadOnly Property ImpedanceArrayCommandFormat As String

    ''' <summary> Writes the Impedance Array without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Impedance Array. </remarks>
    ''' <param name="reading"> The reading. </param>
    ''' <returns> The Impedance Array. </returns>
    Public Function WriteImpedanceArray(ByVal reading As String) As IEnumerable(Of Double)
        Me.ImpedanceArrayReading = reading
        Me.Write(Me.ImpedanceArrayCommandFormat, Me._ImpedanceArrayReading)
        Me.ImpedanceArray = Parse(reading)
        Return Me.ImpedanceArray
    End Function

    ''' <summary> Writes the Impedance Array without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Impedance Array. </remarks>
    ''' <param name="values"> The Impedance Array. </param>
    ''' <returns> The Impedance Array. </returns>
    Public Function WriteImpedanceArray(ByVal values As IEnumerable(Of Double)) As IEnumerable(Of Double)
        Return Me.WriteImpedanceArray(Build(values))
    End Function

    ''' <summary> Writes the Impedance Array without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Impedance Array. </remarks>
    ''' <param name="includesFrequency">      true if impedance array includes. </param>
    ''' <param name="lowFrequencyImpedance">  The low frequency impedance values. </param>
    ''' <param name="highFrequencyImpedance"> The high frequency values. </param>
    ''' <returns> The Impedance Array. </returns>
    Public Function WriteImpedanceArray(ByVal includesFrequency As Boolean,
                                        ByVal lowFrequencyImpedance As IEnumerable(Of Double),
                                        ByVal highFrequencyImpedance As IEnumerable(Of Double)) As IEnumerable(Of Double)
        Return Me.WriteImpedanceArray(Merge(includesFrequency, lowFrequencyImpedance, highFrequencyImpedance))
    End Function

#End Region

#Region " MODEL RESISTANCE "

    ''' <summary> The Model Resistance. </summary>
    Private _ModelResistance As Double?

    ''' <summary> Gets or sets the cached Model Resistance. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property ModelResistance As Double?
        Get
            Return Me._ModelResistance
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.ModelResistance, value) Then
                Me._ModelResistance = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ModelResistance))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Model Resistance. </summary>
    ''' <param name="value"> The Model Resistance. </param>
    ''' <returns> The Model Resistance. </returns>
    Public Function ApplyModelResistance(ByVal value As Double) As Double?
        Me.WriteModelResistance(value)
        Return Me.QueryModelResistance
    End Function

    ''' <summary> Gets or sets the Model Resistance query command. </summary>
    ''' <value> The Model Resistance query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:CKIT:&lt;type&gt;:R?" </remarks>
    Protected Overridable ReadOnly Property ModelResistanceQueryCommand As String

    ''' <summary> Queries the Model Resistance. </summary>
    ''' <returns> The Model Resistance or none if unknown. </returns>
    Public Function QueryModelResistance() As Double?
        Me.ModelResistance = Me.Query(Me.ModelResistance, Me.ModelResistanceQueryCommand)
        Return Me.ModelResistance
    End Function

    ''' <summary> Gets or sets the Model Resistance command format. </summary>
    ''' <value> The Model Resistance command format. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:CKIT:&lt;type&gt;:R {0}" </remarks>
    Protected Overridable ReadOnly Property ModelResistanceCommandFormat As String

    ''' <summary> Writes the Model Resistance without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Model Resistance. </remarks>
    ''' <param name="value"> The Model Resistance. </param>
    ''' <returns> The Model Resistance. </returns>
    Public Function WriteModelResistance(ByVal value As Double) As Double?
        Me.ModelResistance = Me.Write(value, Me.ModelResistanceCommandFormat)
        Return Me.ModelResistance
    End Function

#End Region

#Region " MODEL CAPACITANCE "

    ''' <summary> The Model Capacitance. </summary>
    Private _ModelCapacitance As Double?

    ''' <summary> Gets or sets the cached Model Capacitance. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property ModelCapacitance As Double?
        Get
            Return Me._ModelCapacitance
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.ModelCapacitance, value) Then
                Me._ModelCapacitance = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ModelCapacitance))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Model Capacitance. </summary>
    ''' <param name="value"> The Model Capacitance. </param>
    ''' <returns> The Model Capacitance. </returns>
    Public Function ApplyModelCapacitance(ByVal value As Double) As Double?
        Me.WriteModelCapacitance(value)
        Return Me.QueryModelCapacitance
    End Function

    ''' <summary> Gets or sets the Model Capacitance query command. </summary>
    ''' <value> The Model Capacitance query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:CKIT:&lt;type&gt;:C?" </remarks>
    Protected Overridable ReadOnly Property ModelCapacitanceQueryCommand As String

    ''' <summary> Queries the Model Capacitance. </summary>
    ''' <returns> The Model Capacitance or none if unknown. </returns>
    Public Function QueryModelCapacitance() As Double?
        Me.ModelCapacitance = Me.Query(Me.ModelCapacitance, Me.ModelCapacitanceQueryCommand)
        Return Me.ModelCapacitance
    End Function

    ''' <summary> Gets or sets the Model Capacitance command format. </summary>
    ''' <value> The Model Capacitance command format. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:CKIT:&lt;type&gt;:C {0}" </remarks>
    Protected Overridable ReadOnly Property ModelCapacitanceCommandFormat As String

    ''' <summary> Writes the Model Capacitance without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Model Capacitance. </remarks>
    ''' <param name="value"> The Model Capacitance. </param>
    ''' <returns> The Model Capacitance. </returns>
    Public Function WriteModelCapacitance(ByVal value As Double) As Double?
        Me.ModelCapacitance = Me.Write(value, Me.ModelCapacitanceCommandFormat)
        Return Me.ModelCapacitance
    End Function

#End Region

#Region " MODEL Conductance "

    ''' <summary> The Model Conductance. </summary>
    Private _ModelConductance As Double?

    ''' <summary> Gets or sets the cached Model Conductance. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property ModelConductance As Double?
        Get
            Return Me._ModelConductance
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.ModelConductance, value) Then
                Me._ModelConductance = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ModelConductance))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Model Conductance. </summary>
    ''' <param name="value"> The Model Conductance. </param>
    ''' <returns> The Model Conductance. </returns>
    Public Function ApplyModelConductance(ByVal value As Double) As Double?
        Me.WriteModelConductance(value)
        Return Me.QueryModelConductance
    End Function

    ''' <summary> Gets or sets the Model Conductance query command. </summary>
    ''' <value> The Model Conductance query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:CKIT:&lt;type&gt;:G?" </remarks>
    Protected Overridable ReadOnly Property ModelConductanceQueryCommand As String

    ''' <summary> Queries the Model Conductance. </summary>
    ''' <returns> The Model Conductance or none if unknown. </returns>
    Public Function QueryModelConductance() As Double?
        Me.ModelConductance = Me.Query(Me.ModelConductance, Me.ModelConductanceQueryCommand)
        Return Me.ModelConductance
    End Function

    ''' <summary> Gets or sets the Model Conductance command format. </summary>
    ''' <value> The Model Conductance command format. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:CKIT:&lt;type&gt;:G {0}" </remarks>
    Protected Overridable ReadOnly Property ModelConductanceCommandFormat As String

    ''' <summary> Writes the Model Conductance without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Model Conductance. </remarks>
    ''' <param name="value"> The Model Conductance. </param>
    ''' <returns> The Model Conductance. </returns>
    Public Function WriteModelConductance(ByVal value As Double) As Double?
        Me.ModelConductance = Me.Write(value, Me.ModelConductanceCommandFormat)
        Return Me.ModelConductance
    End Function

#End Region

#Region " MODEL INDUCTANCE "

    ''' <summary> The Model Inductance. </summary>
    Private _ModelInductance As Double?

    ''' <summary> Gets or sets the cached Model Inductance. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property ModelInductance As Double?
        Get
            Return Me._ModelInductance
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.ModelInductance, value) Then
                Me._ModelInductance = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ModelInductance))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Model Inductance. </summary>
    ''' <param name="value"> The Model Inductance. </param>
    ''' <returns> The Model Inductance. </returns>
    Public Function ApplyModelInductance(ByVal value As Double) As Double?
        Me.WriteModelInductance(value)
        Return Me.QueryModelInductance
    End Function

    ''' <summary> Gets or sets the Model Inductance query command. </summary>
    ''' <value> The Model Inductance query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:CKIT:&lt;type&gt;:L?" </remarks>
    Protected Overridable ReadOnly Property ModelInductanceQueryCommand As String

    ''' <summary> Queries the Model Inductance. </summary>
    ''' <returns> The Model Inductance or none if unknown. </returns>
    Public Function QueryModelInductance() As Double?
        Me.ModelInductance = Me.Query(Me.ModelInductance, Me.ModelInductanceQueryCommand)
        Return Me.ModelInductance
    End Function

    ''' <summary> Gets or sets the Model Inductance command format. </summary>
    ''' <value> The Model Inductance command format. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:CKIT:&lt;type&gt;:L {0}" </remarks>
    Protected Overridable ReadOnly Property ModelInductanceCommandFormat As String

    ''' <summary> Writes the Model Inductance without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Model Inductance. </remarks>
    ''' <param name="value"> The Model Inductance. </param>
    ''' <returns> The Model Inductance. </returns>
    Public Function WriteModelInductance(ByVal value As Double) As Double?
        Me.ModelInductance = Me.Write(value, Me.ModelInductanceCommandFormat)
        Return Me.ModelInductance
    End Function

#End Region

End Class

''' <summary> A bit-field of flags for specifying compensation types. </summary>
''' <remarks> David, 7/7/2016. </remarks>
<Flags>
Public Enum CompensationTypes
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Open (OPEN)")> OpenCircuit = 1
    <ComponentModel.Description("Short (SHOR)")> ShortCircuit = OpenCircuit << 1
    <ComponentModel.Description("Load (LOAD)")> Load = ShortCircuit << 1
End Enum

