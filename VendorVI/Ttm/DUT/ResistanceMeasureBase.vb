Imports isr.Core.Pith
Imports isr.Core.Pith.NumericExtensions
''' <summary> Defines a measured cold resistance element. </summary>
''' <license> (c) 2009 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="02/02/2009" by="David" revision="2.1.3320.x"> Created. </history>
Public MustInherit Class ResistanceMeasureBase
    Inherits PropertyPublisherBase
    Implements IPresettablePropertyPublisher, ITalker

#Region " CONSTRUCTORS  AND  CLONES "

    ''' <summary> Default constructor. </summary>
    Protected Sub New()
        MyBase.New()
        randomNumberGenerator = New Random(DateTime.Now.Second)
        Me.ConstructorSafeSetter(New TraceMessageTalker)
    End Sub

    ''' <summary> Clones an existing measurement. </summary>
    ''' <param name="value"> The value. </param>
    Protected Sub New(ByVal value As ResistanceMeasureBase)
        Me.New()
        If value IsNot Nothing Then
            ' measurement
            Me._ResistanceDisplayFormat = value.ResistanceDisplayFormat
            Me._outcome = value.Outcome
            Me._resistance = value.Resistance
            Me._ResistanceCaption = value.ResistanceCaption
            Me._reading = value.Reading
            Me._Voltage = value.Voltage
            Me._VoltageCaption = value.VoltageCaption
            Me._timestamp = value.Timestamp
            ' Configuration
            Me._SourceMeasureUnit = value.SourceMeasureUnit
            Me._Aperture = value.Aperture
            Me._CurrentLevel = value.CurrentLevel
            Me._HighLimit = value.HighLimit
            Me._LowLimit = value.LowLimit
            Me._VoltageLimit = value.VoltageLimit
        End If
    End Sub

    ''' <summary> Copies the configuration described by value. </summary>
    ''' <param name="value"> The value. </param>
    Public Overridable Sub CopyConfiguration(ByVal value As ResistanceMeasureBase)
        If value IsNot Nothing Then
            Me._SourceMeasureUnit = value.SourceMeasureUnit
            Me._Aperture = value.Aperture
            Me._CurrentLevel = value.CurrentLevel
            Me._HighLimit = value.HighLimit
            Me._LowLimit = value.LowLimit
            Me._VoltageLimit = value.VoltageLimit
        End If
    End Sub

    ''' <summary> Copies the measurement described by value. </summary>
    ''' <param name="value"> The value. </param>
    Public Overridable Sub CopyMeasurement(ByVal value As ResistanceMeasureBase)
        If value IsNot Nothing Then
            Me._ResistanceDisplayFormat = value.ResistanceDisplayFormat
            Me._outcome = value.Outcome
            Me._resistance = value.Resistance
            Me._ResistanceCaption = value.ResistanceCaption
            Me._reading = value.Reading
            Me._Voltage = value.Voltage
            Me._VoltageCaption = value.VoltageCaption
            Me._timestamp = value.Timestamp
        End If
    End Sub

#Region " I Disposable Support "

    ''' <summary> Cleans up unmanaged or managed and unmanaged resources. </summary>
    ''' <param name="disposing"> True if this method releases both managed and unmanaged resources;
    ''' False if this method releases only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Try
                    Me.Talker = Nothing
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, ex.ToString)
                End Try
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, ex.ToString)
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " PRESET "

    ''' <summary> Clears execution state. </summary>
    Public Overridable Sub ClearExecutionState() Implements IPresettablePropertyPublisher.ClearExecutionState
        Me.LastReading = ""
        Me.LastOutcome = ""
        Me.Outcome = MeasurementOutcomes.None
        Me.Timestamp = DateTime.MinValue
    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Overridable Sub InitKnownState() Implements IPresettable.InitKnownState
    End Sub

    ''' <summary> Sets values to their known execution preset state. </summary>
    Public Overridable Sub PresetKnownState() Implements IPresettable.PresetKnownState
    End Sub

    ''' <summary> Restores defaults. </summary>
    Public Overridable Sub ResetKnownState() Implements IPresettable.ResetKnownState
        With My.MySettings.Default
            Me.SourceMeasureUnit = .SourceMeasureUnitDefault
        End With
        Me.ResistanceDisplayFormat = "0.000"
        Me.VoltageDisplayFormat = "0.0000"
    End Sub

#End Region

#Region " EQUALS "

    ''' <summary> Indicates whether the current <see cref="T:ColdResistanceBase"></see> value is
    ''' equal to a specified object. </summary>
    ''' <param name="other"> The cold resistance to compare to this object. </param>
    ''' <returns> <c>True</c> if the other parameter is equal to the current
    ''' <see cref="T:ColdResistanceBase"></see> value;
    ''' otherwise, <c>False</c>. </returns>
    Public Overloads Function Equals(ByVal other As ResistanceMeasureBase) As Boolean
        Return other IsNot Nothing AndAlso
            Me.Reading.Equals(other.Reading) AndAlso
            Me.ConfigurationEquals(other) AndAlso
            True
    End Function

    ''' <summary> Indicates whether the current <see cref="T:ResistanceMeasureBase"></see> configuration values are
    ''' equal to a specified object. </summary>
    ''' <param name="other"> The resistance to compare to this object. </param>
    ''' <returns> <c>True</c> if the other parameter is equal to the current
    ''' <see cref="T:ResistanceMeasureBase"></see> value;
    ''' otherwise, <c>False</c>. </returns>
    Public Overridable Function ConfigurationEquals(ByVal other As ResistanceMeasureBase) As Boolean
        Return other IsNot Nothing AndAlso
            String.Equals(Me.SourceMeasureUnit, other.SourceMeasureUnit) AndAlso
            Me.Aperture.Approximates(other.Aperture, 0.00001) AndAlso
            Me.CurrentLevel.Approximates(other.CurrentLevel, 0.000001) AndAlso
            Me.HighLimit.Approximates(other.HighLimit, 0.0001) AndAlso
            Me.LowLimit.Approximates(other.LowLimit, 0.0001) AndAlso
            Me.VoltageLimit.Approximates(other.VoltageLimit, 0.0001) AndAlso
            True
    End Function

    ''' <summary> Check throw if unequal configuration. </summary>
    ''' <param name="other"> The resistance to compare to this object. </param>
    Public Overridable Sub CheckThrowUnequalConfiguration(ByVal other As ResistanceMeasureBase)
        If other IsNot Nothing Then
            If Not Me.ConfigurationEquals(other) Then
                Dim format As String = "Unequal configuring--instrument {0} value of {1} is not {2}"
                If Not String.Equals(Me.SourceMeasureUnit, other.SourceMeasureUnit) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "Source Measure Unit", Me.SourceMeasureUnit, other.SourceMeasureUnit))
                ElseIf Not Me.Aperture.Approximates(other.Aperture, 0.00001) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "Aperture", Me.Aperture, other.Aperture))
                ElseIf Not Me.CurrentLevel.Approximates(other.CurrentLevel, 0.00001) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "Current Level", Me.CurrentLevel, other.CurrentLevel))
                ElseIf Not Me.HighLimit.Approximates(other.HighLimit, 0.00001) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "High Limit", Me.HighLimit, other.HighLimit))
                ElseIf Not Me.LowLimit.Approximates(other.LowLimit, 0.00001) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "Low Limit", Me.LowLimit, other.LowLimit))
                ElseIf Not Me.VoltageLimit.Approximates(other.VoltageLimit, 0.0001) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "Voltage Limit", Me.VoltageLimit, other.VoltageLimit))
                Else
                    Debug.Assert(Not Debugger.IsAttached, "Failed logic")
                End If
            End If
        End If
    End Sub

#End Region

#Region " DISPLAY PROPERTIES "

    Private _ResistanceDisplayFormat As String
    ''' <summary> Gets or sets the display format for resistance. </summary>
    ''' <value> The display format. </value>
    Public Property ResistanceDisplayFormat() As String
        Get
            Return Me._ResistanceDisplayFormat
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.ResistanceDisplayFormat) Then
                Me._ResistanceDisplayFormat = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _VoltageDisplayFormat As String
    ''' <summary> Gets or sets the display format for Voltage. </summary>
    ''' <value> The display format. </value>
    Public Property VoltageDisplayFormat() As String
        Get
            Return Me._VoltageDisplayFormat
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.VoltageDisplayFormat) Then
                Me._VoltageDisplayFormat = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " PARSER "

    Private _LastReading As String
    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property LastReading() As String
        Get
            Return Me._lastReading
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.LastReading) Then
                Me._lastReading = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _LastOutcome As String
    ''' <summary> Gets or sets the last outcome. </summary>
    ''' <value> The last outcome. </value>
    Public Property LastOutcome() As String
        Get
            Return Me._lastOutcome
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.LastOutcome) Then
                Me._lastOutcome = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _LastStatus As String
    ''' <summary> Gets or sets the last status. </summary>
    ''' <value> The last status. </value>
    Public Property LastStatus() As String
        Get
            Return Me._lastStatus
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.LastStatus) Then
                Me._lastStatus = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#Region " RESISTANCE "

    ''' <summary> Sets the reading and outcome. </summary>
    ''' <param name="resistanceReading"> Specifies the reading as received from the instrument. </param>
    ''' <param name="outcome">           . </param>
    Public Sub ParseReading(ByVal resistanceReading As String, ByVal outcome As MeasurementOutcomes)
        Dim value As Double = 0
        If String.IsNullOrWhiteSpace(resistanceReading) Then
            Me.Reading = ""
            If outcome <> MeasurementOutcomes.None Then
                Me.Outcome = MeasurementOutcomes.MeasurementFailed Or outcome
            Else
                Me.Outcome = outcome
            End If
        Else
            Dim numberFormat As Globalization.NumberStyles = Globalization.NumberStyles.Number Or Globalization.NumberStyles.AllowExponent
            Me.Reading = resistanceReading
            If Double.TryParse(Me.Reading, numberFormat, Globalization.CultureInfo.InvariantCulture, value) Then
                If value >= Me.LowLimit AndAlso value <= Me.HighLimit Then
                    Me.Outcome = MeasurementOutcomes.PartPassed
                Else
                    Me.Outcome = outcome Or MeasurementOutcomes.PartFailed
                End If
            Else
                Me.Outcome = MeasurementOutcomes.MeasurementFailed Or MeasurementOutcomes.UnexpectedReadingFormat
            End If
        End If
        Me.Resistance = value
        If Me.CurrentLevel <> 0 Then
            Me.Voltage = value + Me.CurrentLevel
        Else
            Me.Voltage = 0
        End If
        Me.Timestamp = DateTime.Now
        Me.MeasurementAvailable = True
    End Sub

#End Region

#Region " VOLTAGE READING "

    ''' <summary> Applies the contact check outcome described by outcome. </summary>
    ''' <param name="outcome"> . </param>
    Public Sub ApplyContactCheckOutcome(ByVal outcome As MeasurementOutcomes)
        Me.Outcome = Me.Outcome Or outcome
    End Sub

    ''' <summary> Sets the readings. </summary>
    ''' <param name="voltageReading"> Specifies the voltage reading. </param>
    ''' <param name="current">        Specifies the current level. </param>
    ''' <param name="outcome">        Specifies the outcome. </param>
    Public Sub ParseReading(ByVal voltageReading As String, ByVal current As Double, ByVal outcome As MeasurementOutcomes)
        Dim value As Double = 0
        If String.IsNullOrWhiteSpace(voltageReading) Then
            voltageReading = ""
            Me.Voltage = 0
            Me.Resistance = 0
            If outcome <> MeasurementOutcomes.None Then
                Me.Outcome = MeasurementOutcomes.MeasurementFailed Or outcome
            Else
                Me.Outcome = outcome
            End If
        Else
            Dim numberFormat As Globalization.NumberStyles = Globalization.NumberStyles.Number Or Globalization.NumberStyles.AllowExponent
            Me.Reading = voltageReading
            If Double.TryParse(Me.Reading, numberFormat, Globalization.CultureInfo.InvariantCulture, value) Then
                If current <> 0 Then
                    Me.Resistance = value / current
                Else
                    Me.Resistance = 0
                End If
                If value >= Me.LowLimit AndAlso value <= Me.HighLimit Then
                    Me.Outcome = Me.Outcome Or MeasurementOutcomes.PartPassed
                Else
                    Me.Outcome = Me.Outcome Or MeasurementOutcomes.PartFailed
                End If
                Me.Voltage = value
            Else
                Me.Voltage = 0
                Me.Resistance = 0
                Me.Outcome = MeasurementOutcomes.MeasurementFailed Or MeasurementOutcomes.UnexpectedReadingFormat
            End If
        End If
        Me.Timestamp = DateTime.Now
        Me.MeasurementAvailable = True
    End Sub

#End Region

#End Region

#Region " MEASUREMENT PROPERTIES "

    Private _MeasurementAvailable As Boolean

    ''' <summary> Gets or sets (protected) the measurement available. </summary>
    ''' <value> The measurement available. </value>
    Public Property MeasurementAvailable As Boolean
        Get
            Return Me._MeasurementAvailable
        End Get
        Set(ByVal value As Boolean)
            Me._MeasurementAvailable = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property

    Private _Outcome As MeasurementOutcomes
    ''' <summary> Gets or sets the measurement outcome. </summary>
    ''' <value> The outcome. </value>
    Public Property Outcome() As MeasurementOutcomes
        Get
            Return Me._outcome
        End Get
        Set(ByVal value As MeasurementOutcomes)
            If Not value.Equals(Me.Outcome) Then
                Me._outcome = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _Reading As String
    ''' <summary> Gets  or sets (protected) the reading.  When set, the value is converted to resistance. </summary>
    ''' <value> The reading. </value>
    Public Property Reading() As String
        Get
            Return Me._reading
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.Reading) Then
                Me._reading = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _Resistance As Double
    ''' <summary> Gets or sets (protected) the measured resistance. </summary>
    ''' <value> The resistance. </value>
    Public Property Resistance() As Double
        Get
            Return Me._resistance
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.Resistance) Then
                Me._resistance = value
                Me.SafePostPropertyChanged()
                If Me.Outcome = MeasurementOutcomes.None OrElse ((Outcome And MeasurementOutcomes.MeasurementNotMade) <> 0) Then
                    Me.ResistanceCaption = ""
                ElseIf String.IsNullOrWhiteSpace(Me.Reading) OrElse ((Outcome And MeasurementOutcomes.MeasurementFailed) <> 0) Then
                    Me.ResistanceCaption = "#null#"
                Else
                    Me.ResistanceCaption = Me._resistance.ToString(Me.ResistanceDisplayFormat, Globalization.CultureInfo.CurrentCulture)
                End If
            End If
        End Set
    End Property

    Private _ResistanceCaption As String
    ''' <summary> Gets or sets (protected) the measured resistance display caption. </summary>
    ''' <value> The resistance caption. </value>
    Public Property ResistanceCaption() As String
        Get
            Return Me._ResistanceCaption
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.ResistanceCaption) Then
                Me._ResistanceCaption = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Date/Time of the timestamp. </summary>
    Private _Timestamp As DateTime

    ''' <summary> Gets or sets (protected) the measurement time stamp. Gets set when setting the reading or resistance
    ''' value. </summary>
    ''' <value> The timestamp. </value>
    Public Property Timestamp() As DateTime
        Get
            Return Me._timestamp
        End Get
        Protected Set(ByVal value As DateTime)
            If Not value.Equals(Me.Timestamp) Then
                Me._timestamp = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private randomNumberGenerator As Random

    ''' <summary> Generates a random reading. </summary>
    ''' <returns> The random reading. </returns>
    Public Function GenerateRandomReading() As Double
        Dim k As Double = 1000
        Return randomNumberGenerator.Next(CInt(K * Me.LowLimit), CInt(K * Me.HighLimit)) / K
    End Function

    ''' <summary> Emulates a reading. </summary>
    Public Sub EmulateReading()
        Me.LastReading = CStr(Me.GenerateRandomReading())
        Me.LastOutcome = "0"
    End Sub

    Private _Voltage As Double
    ''' <summary> Gets or sets the measured Voltage. </summary>
    ''' <value> The Voltage. </value>
    Public Property Voltage() As Double
        Get
            Return Me._Voltage
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.Voltage) Then
                Me._Voltage = value
                Me.SafePostPropertyChanged()
                If (Me.Outcome And MeasurementOutcomes.MeasurementNotMade) <> 0 Then
                    Me.VoltageCaption = ""
                ElseIf String.IsNullOrWhiteSpace(Me.Reading) OrElse ((Outcome And MeasurementOutcomes.MeasurementFailed) <> 0) Then
                    Me.VoltageCaption = "#null#"
                Else
                    Me.VoltageCaption = Me._Voltage.ToString(Me.VoltageDisplayFormat, Globalization.CultureInfo.CurrentCulture)
                End If
            End If
        End Set
    End Property

    Private _VoltageCaption As String
    ''' <summary> Gets or sets the measured Voltage display caption. </summary>
    ''' <value> The Voltage caption. </value>
    Public Property VoltageCaption() As String
        Get
            Return Me._VoltageCaption
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.VoltageCaption) Then
                Me._VoltageCaption = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " CONFIGURATION PROPERTIES "

    Private _SourceMeasureUnit As String

    ''' <summary> Gets or sets the cached Source Measure Unit. </summary>
    ''' <returns> The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    Public Overridable Property SourceMeasureUnit As String
        Get
            Return Me._SourceMeasureUnit
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.SourceMeasureUnit) Then
                Me._SourceMeasureUnit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _Aperture As Double
    ''' <summary> Gets or sets the integration period in number of power line cycles for measuring the
    ''' cold resistance. </summary>
    ''' <value> The aperture. </value>
    Public Property Aperture() As Double
        Get
            Return Me._Aperture
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.Aperture) Then
                Me._Aperture = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _CurrentLevel As Double
    ''' <summary> Gets or sets the current level. </summary>
    ''' <value> The current level. </value>
    Public Property CurrentLevel() As Double
        Get
            Return Me._CurrentLevel
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.CurrentLevel) Then
                Me._CurrentLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _HighLimit As Double
    ''' <summary> Gets or sets the high limit for determining measurement pass fail condition. </summary>
    ''' <value> The high limit. </value>
    Public Property HighLimit() As Double
        Get
            Return Me._HighLimit
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.HighLimit) Then
                Me._HighLimit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _LowLimit As Double
    ''' <summary> Gets or sets the low limit for determining measurement pass fail condition. </summary>
    ''' <value> The low limit. </value>
    Public Property LowLimit() As Double
        Get
            Return Me._LowLimit
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.LowLimit) Then
                Me._LowLimit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _VoltageLimit As Double
    ''' <summary> Gets or sets the voltage limit. </summary>
    ''' <value> The voltage limit. </value>
    Public Property VoltageLimit() As Double
        Get
            Return Me._VoltageLimit
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.VoltageLimit) Then
                Me._VoltageLimit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

End Class

