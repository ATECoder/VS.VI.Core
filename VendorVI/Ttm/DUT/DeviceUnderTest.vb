Imports System.Collections.Specialized
Imports isr.Core.Pith
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Defines the device under test element including measurement and configuration. </summary>
''' <license> (c) 2009 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="02/02/2009" by="David" revision="2.1.3320.x"> Created. </history>
Public Class DeviceUnderTest
    Inherits PropertyPublisherBase
    Implements ICloneable, IPresettablePropertyPublisher

#Region " CONSTRUCTORS  AND  CLONES "

    ''' <summary> Primary Constructor. </summary>
    Public Sub New()
        MyBase.New()
        ' initial resistance must be first.
        Me._Elements = New ResistanceMeasureCollection
        Me.InitialResistance = New ColdResistance()
        Me.Elements.Add(Me.InitialResistance)
        Me.FinalResistance = New ColdResistance()
        Me.Elements.Add(Me.FinalResistance)
        Me.ShuntResistance = New ShuntResistance()
        Me.Elements.Add(Me.ShuntResistance)
        Me.ThermalTransient = New ThermalTransient()
        Me.Elements.Add(Me.ThermalTransient)
    End Sub

    ''' <summary> Clones an existing part. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub New(ByVal value As DeviceUnderTest)
        MyBase.New()
        If value IsNot Nothing Then
            Me.PartNumber = value.PartNumber
            Me.OperatorId = value.OperatorId
            Me.LotId = value.LotId
            Me.SampleNumber = value.SampleNumber
            Me.SerialNumber = value.SerialNumber
            Me.ContactCheckEnabled = value.ContactCheckEnabled
            Me.ContactCheckThreshold = value.ContactCheckThreshold
            ' Measurements
            Me._Elements = New ResistanceMeasureCollection
            ' initial resistance must be first.
            Me.InitialResistance = New ColdResistance(value.InitialResistance)
            Me.Elements.Add(Me.InitialResistance)
            Me.FinalResistance = New ColdResistance(value.FinalResistance)
            Me.Elements.Add(Me.FinalResistance)
            Me.ShuntResistance = New ShuntResistance(value.ShuntResistance)
            Me.Elements.Add(Me.ShuntResistance)
            Me.ThermalTransient = New ThermalTransient(value.ThermalTransient)
            Me.Elements.Add(Me.ThermalTransient)
            Me._outcome = value.Outcome
        End If
    End Sub

    ''' <summary> Clones the part. </summary>
    ''' <returns> A copy of this object. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return New DeviceUnderTest(Me)
    End Function

    ''' <summary> Copies the device under test information except the serial number. </summary>
    ''' <param name="value"> The other device under test. </param>
    Public Overridable Sub CopyInfo(ByVal value As DeviceUnderTest)
        If value IsNot Nothing Then
            Me.OperatorId = value.OperatorId
            Me.LotId = value.LotId
            Me.PartNumber = value.PartNumber
        End If
    End Sub

    ''' <summary> Copies the configuration described by value. </summary>
    ''' <param name="value"> The other device under test. </param>
    Public Overridable Sub CopyConfiguration(ByVal value As DeviceUnderTest)
        If value IsNot Nothing Then
            Me._ContactCheckEnabled = value.ContactCheckEnabled
            Me._ContactCheckThreshold = value.ContactCheckThreshold
            Me.InitialResistance.CopyConfiguration(value.InitialResistance)
            Me.FinalResistance.CopyConfiguration(value.FinalResistance)
            Me.ThermalTransient.CopyConfiguration(value.FinalResistance)
        End If
    End Sub

    ''' <summary> Copies the measurement described by value. </summary>
    ''' <param name="value"> The other device under test. </param>
    Public Overridable Sub CopyMeasurement(ByVal value As DeviceUnderTest)
        If value IsNot Nothing Then
            Me.InitialResistance.CopyMeasurement(value.InitialResistance)
            Me.FinalResistance.CopyMeasurement(value.FinalResistance)
            Me.ThermalTransient.CopyMeasurement(value.FinalResistance)
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
                Me.Elements?.DisposeItems() : Me.Elements = Nothing
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

    ''' <summary> Initializes known state. </summary>
    ''' <remarks> This erases the last reading. </remarks>
    Public Sub ClearMeasurements()
        Me.Elements.ClearExecutionState()
        Me.Outcome = MeasurementOutcomes.None
    End Sub

    ''' <summary> Initializes known state. </summary>
    ''' <remarks> This erases the last reading. </remarks>
    Public Sub ClearPartInfo()
        Me.LotId = ""
        Me.OperatorId = ""
        Me.SampleNumber = 0
        Me.SerialNumber = 0
        Me.PartNumber = ""
    End Sub

    ''' <summary> Sets values to their known execution clear state. </summary>
    ''' <remarks> This erases the last reading. </remarks>
    Public Overridable Sub ClearExecutionState() Implements IPresettable.ClearExecutionState
        Me.ClearPartInfo()
        Me.ClearMeasurements()
    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Overridable Sub InitKnownState() Implements IPresettable.InitKnownState
    End Sub

    ''' <summary> Sets values to their known execution preset state. </summary>
    Public Overridable Sub PresetKnownState() Implements IPresettable.PresetKnownState
    End Sub

    ''' <summary> Sets values to their known execution reset state. </summary>
    Public Overridable Sub ResetKnownState() Implements IPresettable.ResetKnownState
        Me.ContactCheckEnabled = My.Settings.ContactCheckEnabledDefault
        Me.ContactCheckThreshold = CInt(My.Settings.ContactCheckThresholdDefault)
        Me.Elements.ResetKnownState()
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Gets or sets the elements. </summary>
    ''' <value> The elements. </value>
    Private Property Elements As ResistanceMeasureCollection

    ''' <summary> Resumes the property events. </summary>
    Public Overrides Sub ResumePublishing()
        MyBase.ResumePublishing()
        Me.Elements.ResumePublishing()
    End Sub

    ''' <summary> Suppresses the property events. </summary>
    Public Overrides Sub SuspendPublishing()
        MyBase.SuspendPublishing()
        Me.Elements.SuspendPublishing()
    End Sub

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        Me.Elements.Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COLLECTIBLE "

    ''' <summary> Returns a unique key based on the part number and sample number. </summary>
    ''' <param name="partNumber">   The part number. </param>
    ''' <param name="sampleNumber"> The sample number. </param>
    Public Shared Function BuildUniqueKey(ByVal partNumber As String, ByVal sampleNumber As Integer) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, "{0}.{1}", partNumber, sampleNumber)
    End Function

    Private _SampleNumber As Integer
    ''' <summary> Gets or sets the sample number. </summary>
    ''' <value> The sample number. </value>
    Public Property SampleNumber() As Integer
        Get
            Return Me._SampleNumber
        End Get
        Set(ByVal value As Integer)
            If Not value.Equals(Me.SampleNumber) Then
                Me._SampleNumber = value
                Me.SafePostPropertyChanged(NameOf(Me.SampleNumber))
            End If
        End Set
    End Property

    ''' <summary> The unique key. </summary>
    Private _UniqueKey As String

    ''' <summary> Gets or sets (protected) the unique key. </summary>
    ''' <value> The unique key. </value>
    Public Property UniqueKey() As String
        Get
            Dim value As String = DeviceUnderTest.BuildUniqueKey(Me.PartNumber, Me.SampleNumber)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me._uniqueKey) Then
                Me._uniqueKey = value
                Me.SafePostPropertyChanged(NameOf(Me.UniqueKey))
            End If
            Return Me._uniqueKey
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.UniqueKey) Then
                Me._uniqueKey = value
                Me.SafePostPropertyChanged(NameOf(Me.UniqueKey))
            End If
        End Set
    End Property

#End Region

#Region " RECORD "

    ''' <value> The data header. </value>
    Public Shared ReadOnly Property DataHeader() As String
        Get
            Return """Sample Number"",""Time Stamp"",""Serial Number"",""Initial Resistance"",""Final Resistance"",""Thermal Transient Voltage"""
        End Get
    End Property

    Private _DataRecordFormat As String
    ''' <summary> Gets the data record to save to a comma separated file. </summary>
    ''' <value> The data record. </value>
    Public ReadOnly Property DataRecord() As String
        Get
            If String.IsNullOrWhiteSpace(Me._dataRecordFormat) Then
                Me._dataRecordFormat = "{0},#{1}#,{2},{3:G5},{4:G5},{5:G5}"
            End If
            Dim timestamp As String = Me.ThermalTransient.Timestamp.ToShortDateString & " " & Me.ThermalTransient.Timestamp.ToShortTimeString
            Return String.Format(Globalization.CultureInfo.CurrentCulture, Me._dataRecordFormat,
                                 Me.SampleNumber, timestamp, Me.SerialNumber,
                                 Me.InitialResistance.Resistance, Me.FinalResistance.Resistance, Me.ThermalTransient.Voltage)
        End Get
    End Property

#End Region

#Region " DEVICE UNDER TEST INFO "

    ''' <summary> Information equals. </summary>
    ''' <param name="other"> The other. </param>
    ''' <returns> <c>True</c> if basic info is equal excluding serial number. </returns>
    Public Function InfoEquals(ByVal other As DeviceUnderTest) As Boolean
        Return other IsNot Nothing AndAlso
            String.Equals(Me.PartNumber, other.PartNumber) AndAlso
            String.Equals(Me.LotId, other.LotId) AndAlso
            String.Equals(Me.OperatorId, other.OperatorId) AndAlso
            Me.ContactCheckEnabled.Equals(other.ContactCheckEnabled) AndAlso
            Me.ContactCheckThreshold.Equals(other.ContactCheckThreshold) AndAlso
            True
    End Function

    Private _PartNumber As String
    ''' <summary> Gets or sets the part number. </summary>
    ''' <value> The part number. </value>
    Public Property PartNumber() As String
        Get
            Return Me._PartNumber
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.PartNumber) Then
                Me._PartNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _LotId As String
    ''' <summary> Gets or Sets the Lot ID. </summary>
    ''' <value> The identifier of the lot. </value>
    Public Property LotId() As String
        Get
            Return Me._LotId
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.LotId) Then
                Me._LotId = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _OperatorId As String
    ''' <summary> Gets or Sets the Operator ID. </summary>
    ''' <value> The identifier of the operator. </value>
    Public Property OperatorId() As String
        Get
            Return Me._OperatorId
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.OperatorId) Then
                Me._OperatorId = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _SerialNumber As Integer
    ''' <summary> Gets or sets the part serial number. </summary>
    ''' <value> The serial number. </value>
    Public Property SerialNumber() As Integer
        Get
            Return Me._SerialNumber
        End Get
        Set(ByVal value As Integer)
            If Not value.Equals(Me.SerialNumber) Then
                Me._SerialNumber = value
                Me.SafePostPropertyChanged()
            End If

        End Set
    End Property

#End Region

#Region " CONTACT CHECK INFO "

    Private _ContactCheckEnabled As Boolean

    ''' <summary> Gets or sets the contact check enabled. </summary>
    ''' <value> The contact check enabled. </value>
    Property ContactCheckEnabled As Boolean
        Get
            Return Me._ContactCheckEnabled
        End Get
        Set(ByVal value As Boolean)
            If Not value.Equals(Me.ContactCheckEnabled) Then
                Me._ContactCheckEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _ContactCheckThreshold As Integer
    ''' <summary> Gets or sets the contact check threshold. </summary>
    ''' <value> The serial number. </value>
    Public Property ContactCheckThreshold() As Integer
        Get
            Return Me._ContactCheckThreshold
        End Get
        Set(ByVal value As Integer)
            If Not value.Equals(Me.ContactCheckThreshold) Then
                Me._ContactCheckThreshold = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " MEASUREMENT ELEMENTS "

    ''' <summary> Handles the resistance measure property changed event. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OnPropertyChanged(ByVal sender As ResistanceMeasureBase, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.MeasurementAvailable)
                If sender.MeasurementAvailable Then
                    Me.MergeOutcomes()
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _InitialResistance for property changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitialResistance_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _InitialResistance.PropertyChanged
        Try
            Me.OnPropertyChanged(TryCast(sender, ResistanceMeasureBase), e?.PropertyName)
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}.", e.PropertyName, ex.ToFullBlownString)
        Finally
            Me.SafePostPropertyChanged(NameOf(Me.InitialResistance))
        End Try
    End Sub

    Private WithEvents _InitialResistance As ColdResistance
    ''' <summary> Gets or sets reference to the <see cref="ColdResistance">initial cold
    ''' resistance</see> </summary>
    ''' <value> The initial resistance. </value>
    Public Property InitialResistance() As ColdResistance
        Get
            Return Me._InitialResistance
        End Get
        Set(value As ColdResistance)
            Me._InitialResistance = value
        End Set
    End Property

    ''' <summary> Event handler. Called by _FinalResistance for property changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _FinalResistance_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _FinalResistance.PropertyChanged
        Try
            Me.OnPropertyChanged(TryCast(sender, ResistanceMeasureBase), e?.PropertyName)
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}.", e.PropertyName, ex.ToFullBlownString)
        Finally
            Me.SafePostPropertyChanged(NameOf(Me.FinalResistance))
        End Try
    End Sub

    Private WithEvents _FinalResistance As ColdResistance
    ''' <summary> Gets or sets reference to the <see cref="ColdResistance">Final cold
    ''' resistance</see> </summary>
    ''' <value> The Final resistance. </value>
    Public Property FinalResistance() As ColdResistance
        Get
            Return Me._FinalResistance
        End Get
        Set(value As ColdResistance)
            Me._FinalResistance = value
        End Set
    End Property

    ''' <summary> Event handler. Called by _ShuntResistance for property changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    Private Sub _ShuntResistance_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _ShuntResistance.PropertyChanged
        Me.SafePostPropertyChanged(NameOf(Me.ShuntResistance))
    End Sub

    Private WithEvents _ShuntResistance As ShuntResistance
    ''' <summary> Gets or sets reference to the <see cref="ShuntResistance">Shunt resistance</see> </summary>
    ''' <value> The shunt resistance. </value>
    Public Property ShuntResistance() As ShuntResistance
        Get
            Return Me._ShuntResistance
        End Get
        Set(value As ShuntResistance)
            Me._ShuntResistance = value
        End Set
    End Property

    ''' <summary> Event handler. Called by _ThermalTransient for property changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ThermalTransient_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _ThermalTransient.PropertyChanged
        Try
            Me.OnPropertyChanged(TryCast(sender, ResistanceMeasureBase), e?.PropertyName)
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}.", e.PropertyName, ex.ToFullBlownString)
        Finally
            Me.SafePostPropertyChanged(NameOf(Me.ThermalTransient))
        End Try
    End Sub

    Private WithEvents _ThermalTransient As ThermalTransient
    ''' <summary> Gets or sets reference to the <see cref="ThermalTransient">thermal transient</see> </summary>
    ''' <value> The thermal transient. </value>
    Public Property ThermalTransient() As ThermalTransient
        Get
            Return Me._ThermalTransient
        End Get
        Set(value As ThermalTransient)
            Me._ThermalTransient = value
        End Set
    End Property

    ''' <summary> Validates the transient voltage limit described by details. </summary>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if the voltage limit is in rage; <c>False</c> is the limit is too low. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function ValidateTransientVoltageLimit(ByRef details As String) As Boolean
        If Me.InitialResistance Is Nothing Then
            Throw New InvalidOperationException("Initial Resistance object is null.")
        End If
        If Me.ThermalTransient Is Nothing Then
            Throw New InvalidOperationException("Thermal Transient object is null.")
        End If
        Dim value As Double = Me.InitialResistance.HighLimit * Me.ThermalTransient.CurrentLevel + Me.ThermalTransient.AllowedVoltageChange
        If value > Me._ThermalTransient.VoltageLimit Then
            details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                   "A Thermal transient voltage limit of {0} volts is too low;. It must be at least {1} volts, which is determined by the cold resistance high limit and thermal transient current level.",
                                   Me.ThermalTransient.VoltageLimit, value)
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary> Checks if the configuration is equal to the other <see cref="DeviceUnderTest">DUT</see>. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="other"> The other. </param>
    ''' <returns> <c>True</c> if configurations are the same as the other device. </returns>
    Public Function ConfigurationEquals(ByVal other As DeviceUnderTest) As Boolean
        If other Is Nothing Then Throw New ArgumentNullException(NameOf(other))
        Return other IsNot Nothing AndAlso
               Me.ContactCheckEnabled.Equals(other.ContactCheckEnabled) AndAlso
               Me.ContactCheckThreshold.Equals(other.ContactCheckThreshold) AndAlso
               Me.InitialResistance.ConfigurationEquals(other.InitialResistance) AndAlso
               Me.FinalResistance.ConfigurationEquals(other.FinalResistance) AndAlso
               Me.ThermalTransient.ConfigurationEquals(other.ThermalTransient)
    End Function

    ''' <summary> Checks if the thermal transient configuration is equal to the other
    ''' <see cref="DeviceUnderTest">DUT</see> changed. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="other"> The other. </param>
    ''' <returns> <c>True</c> if configurations are the same as the other device. </returns>
    Public Function ThermalTransientConfigurationEquals(ByVal other As DeviceUnderTest) As Boolean
        If other Is Nothing Then Throw New ArgumentNullException(NameOf(other))
        Return other IsNot Nothing AndAlso
               Me.InitialResistance.ConfigurationEquals(other.InitialResistance) AndAlso
               Me.FinalResistance.ConfigurationEquals(other.FinalResistance) AndAlso
               Me.ThermalTransient.ConfigurationEquals(other.ThermalTransient)
    End Function

    ''' <summary> Checks if DUT Information and configuration are equal between the two devices. </summary>
    ''' <param name="other"> The other. </param>
    ''' <returns> <c>True</c> if info and configurations are the same as the other device. </returns>
    Public Function InfoConfigurationEquals(ByVal other As DeviceUnderTest) As Boolean
        If other Is Nothing Then Throw New ArgumentNullException(NameOf(other))
        Return Me.InfoEquals(other) AndAlso Me.ConfigurationEquals(other)
    End Function

    ''' <summary> Checks if the configuration is equal to the other <see cref="DeviceUnderTest">DUT</see> changed. </summary>
    ''' <returns> <c>True</c> if configurations are the same as the other device.</returns>
    Public Function SourceMeasureUnitEquals(ByVal other As DeviceUnderTest) As Boolean
        If other Is Nothing Then
            Throw New ArgumentNullException(NameOf(other))
        ElseIf other.InitialResistance Is Nothing OrElse
            other.FinalResistance Is Nothing OrElse
            other.ThermalTransient Is Nothing Then
            Throw New InvalidOperationException("DUT elements not instantiated.")
        ElseIf other.InitialResistance.SourceMeasureUnit Is Nothing OrElse
            other.FinalResistance.SourceMeasureUnit Is Nothing OrElse
            other.ThermalTransient.SourceMeasureUnit Is Nothing Then
            Throw New InvalidOperationException("DUT Source Measure Elements are null.")
        ElseIf String.IsNullOrWhiteSpace(other.InitialResistance.SourceMeasureUnit) OrElse
            String.IsNullOrWhiteSpace(other.FinalResistance.SourceMeasureUnit) OrElse
            String.IsNullOrWhiteSpace(other.ThermalTransient.SourceMeasureUnit) Then
            Throw New InvalidOperationException(String.Format("A DUT Source Measure Element is empty: Initial='{0}'; Final='{1}'; Transient='{2}'.",
                                                other.InitialResistance.SourceMeasureUnit, other.FinalResistance.SourceMeasureUnit,
                                                other.ThermalTransient.SourceMeasureUnit))
        End If
        Return other IsNot Nothing AndAlso
               String.Equals(Me.InitialResistance.SourceMeasureUnit, other.InitialResistance.SourceMeasureUnit) AndAlso
               String.Equals(Me.FinalResistance.SourceMeasureUnit, other.FinalResistance.SourceMeasureUnit) AndAlso
               String.Equals(Me.ThermalTransient.SourceMeasureUnit, other.ThermalTransient.SourceMeasureUnit)
    End Function

    ''' <summary> Gets the timestamp. </summary>
    ''' <value> The timestamp. </value>
    Public ReadOnly Property Timestamp As Date
        Get
            Return Me.ThermalTransient.Timestamp
        End Get
    End Property

    ''' <summary> Gets the initial resistance caption. </summary>
    ''' <value> The initial resistance caption. </value>
    Public ReadOnly Property InitialResistanceCaption As String
        Get
            Return Me._InitialResistance.ResistanceCaption
        End Get
    End Property

    ''' <summary> Gets the final resistance caption. </summary>
    ''' <value> The final resistance caption. </value>
    Public ReadOnly Property FinalResistanceCaption As String
        Get
            Return Me._FinalResistance.ResistanceCaption
        End Get
    End Property

    ''' <summary> Gets the thermal transient voltage caption. </summary>
    ''' <value> The thermal transient voltage caption. </value>
    Public ReadOnly Property ThermalTransientVoltageCaption As String
        Get
            Return Me._ThermalTransient.VoltageCaption
        End Get
    End Property

#End Region

#Region " OUTCOME "

    ''' <summary> Checks if any part measurement is in. This is determined by examining all outcomes
    ''' as not equal to
    ''' <see cref="MeasurementOutcomes.None">None</see> </summary>
    ''' <returns> <c>True</c> if one or more measurement outcomes were set. </returns>
    Public Function AnyMeasurementMade() As Boolean
        Return Me.InitialResistance.Outcome <> MeasurementOutcomes.None OrElse
               Me.ThermalTransient.Outcome <> MeasurementOutcomes.None OrElse
               Me.FinalResistance.Outcome <> MeasurementOutcomes.None
    End Function

    Private _AnyMeasurementAvailable As Boolean

    ''' <summary> Gets or sets the sentinel indicating if Any measurements is available. </summary>
    ''' <value> The measurement available. </value>
    Public Property AnyMeasurementsAvailable As Boolean
        Get
            Return Me._AnyMeasurementAvailable
        End Get
        Set(value As Boolean)
            Me._AnyMeasurementAvailable = value
            Me.SafePostPropertyChanged(NameOf(Me.AnyMeasurementsAvailable))
        End Set
    End Property

    ''' <summary> Checks if all part measurements are in. This is determined by examining all outcomes
    ''' as not equal to
    ''' <see cref="MeasurementOutcomes.None">None</see> </summary>
    ''' <returns> <c>True</c> if all measurement outcomes were set. </returns>
    Public Function AllMeasurementsMade() As Boolean
        Return Me.InitialResistance.Outcome <> MeasurementOutcomes.None AndAlso
               Me.ThermalTransient.Outcome <> MeasurementOutcomes.None AndAlso
               Me.FinalResistance.Outcome <> MeasurementOutcomes.None
    End Function

    Private _AllMeasurementAvailable As Boolean

    ''' <summary> Gets or sets the sentinel indicating if all measurements are available. </summary>
    ''' <value> The measurement available. </value>
    Public Property AllMeasurementsAvailable As Boolean
        Get
            Return Me._AllMeasurementAvailable
        End Get
        Set(value As Boolean)
            Me._AllMeasurementAvailable = value
            Me.SafePostPropertyChanged(NameOf(Me.AllMeasurementsAvailable))
        End Set
    End Property

    ''' <summary> Merges the measurements outcomes to a Double outcome. </summary>
    Public Sub MergeOutcomes()
        Dim outcome As MeasurementOutcomes = MeasurementOutcomes.None
        outcome = DeviceUnderTest.mergeOutcomes(outcome, Me.InitialResistance.Outcome)
        outcome = DeviceUnderTest.mergeOutcomes(outcome, Me.FinalResistance.Outcome)
        outcome = DeviceUnderTest.mergeOutcomes(outcome, Me.ThermalTransient.Outcome)
        Me.Outcome = outcome
    End Sub

    ''' <summary> Combine the existing outcome with new outcome. If failed, leave failed. If hit
    ''' compliance set to hit compliance. </summary>
    ''' <param name="existingOutcome"> The existing outcome. </param>
    ''' <param name="newOutcome">      The new outcome. </param>
    Private Shared Function MergeOutcomes(ByVal existingOutcome As MeasurementOutcomes,
                                          ByVal newOutcome As MeasurementOutcomes) As MeasurementOutcomes

        If (newOutcome And MeasurementOutcomes.PartPassed) <> 0 Then
            If existingOutcome = MeasurementOutcomes.None Then
                Return MeasurementOutcomes.PartPassed
            Else
                Return existingOutcome
            End If
        Else
            existingOutcome = existingOutcome And Not MeasurementOutcomes.PartPassed
            Return existingOutcome Or newOutcome
        End If

    End Function

    Private _Outcome As MeasurementOutcomes
    ''' <summary> Gets or sets the measurement outcome for the part. </summary>
    ''' <value> The outcome. </value>
    Public Property Outcome() As MeasurementOutcomes
        Get
            Return Me._outcome
        End Get
        Protected Set(ByVal value As MeasurementOutcomes)
            ' None is used to flag the measurements were cleared.
            If MeasurementOutcomes.None = value OrElse Not value.Equals(Me.Outcome) Then
                Me._outcome = value
                Me.SafePostPropertyChanged()
            End If
            Me.AllMeasurementsAvailable = Me.AllMeasurementsMade
            Me.AnyMeasurementsAvailable = Me.AnyMeasurementMade
        End Set
    End Property

#End Region

End Class

''' <summary> Collection of device under tests. </summary>
''' <remarks> David, 1/6/2016. </remarks>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/6/2016" by="David" revision=""> Created. </history>
Public Class DeviceUnderTestCollection
    Inherits Collections.ObjectModel.ObservableCollection(Of DeviceUnderTest)

    Public Sub New()
        MyBase.New
        Me._devices = New DeviceCollection
    End Sub

    Private Class DeviceCollection
        Inherits ObjectModel.KeyedCollection(Of String, DeviceUnderTest)
        Protected Overrides Function GetKeyForItem(item As DeviceUnderTest) As String
            Return item?.UniqueKey
        End Function
    End Class

    Private _Devices As DeviceCollection

    Protected Overrides Sub OnCollectionChanged(e As NotifyCollectionChangedEventArgs)
        Me._devices.Clear()
        For Each d As DeviceUnderTest In Me
            Me._devices.Add(d)
        Next
        MyBase.OnCollectionChanged(e)
    End Sub
End Class

