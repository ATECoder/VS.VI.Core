Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.StopwatchExtensions
Imports isr.VI.Tsp
Imports isr.VI.Tsp.K2600
''' <summary> Defines the thermal transient meter including measurement and
''' configuration. The Thermal Transient meter includes the elements that define a complete
''' set of thermal transient measurements and settings. </summary>
''' <license> (c) 2009 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="09/17/2010" by="David" revision="2.2.3912.x"> Created. </history>
Public Class Meter
    Inherits PropertyPublisherBase
    Implements IPresettablePropertyPublisher, ITalker

#Region " CONSTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me._MasterDevice = New Device
        Me._Talker = New TraceMessageTalker
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
                    If Me._MasterDevice Is Nothing Then
                        Me.OnClosing()
                    Else
                        Me._MasterDevice.Dispose() : Me._MasterDevice = Nothing
                    End If
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

#Region " MASTER DEVICE "

    Private WithEvents _MasterDevice As Device

    ''' <summary> Gets the master device. </summary>
    ''' <value> The master device. </value>
    Public ReadOnly Property MasterDevice As Device
        Get
            Return _MasterDevice
        End Get
    End Property

    ''' <summary> Gets a value indicating whether the subsystem has an open Device open. </summary>
    ''' <value> <c>True</c> if the device has an open Device; otherwise, <c>False</c>. </value>
    Public ReadOnly Property IsDeviceOpen As Boolean
        Get
            Return Me._MasterDevice IsNot Nothing AndAlso Me.MasterDevice.IsDeviceOpen
        End Get
    End Property

    ''' <summary> Gets a value indicating whether the subsystem has an open session open. </summary>
    ''' <value> <c>True</c> if the device has an open session; otherwise, <c>False</c>. </value>
    Public ReadOnly Property IsSessionOpen As Boolean
        Get
            Return Me._MasterDevice IsNot Nothing AndAlso Me.MasterDevice.Session.IsSessionOpen
        End Get
    End Property

    ''' <summary> Gets the name of the resource. </summary>
    ''' <value> The name of the resource or &lt;closed&gt; if not open. </value>
    Public ReadOnly Property ResourceName As String
        Get
            If Me.MasterDevice Is Nothing Then
                Return "<closed>"
            Else
                Return Me.MasterDevice.ResourceName
            End If
        End Get
    End Property

    ''' <summary> Disposes of the local elements. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OnClosing()
        Me.SuspendPublishing()
        Try
            Try
                Me.AbortTriggerSequenceIf()
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToString)
            End Try
            Try
                If Me._MeasureSequencer IsNot Nothing Then Me._MeasureSequencer.Dispose() : Me._MeasureSequencer = Nothing
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToString)
            End Try
            Try
                If Me._TriggerSequencer IsNot Nothing Then Me._TriggerSequencer.Dispose() : Me._TriggerSequencer = Nothing
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToString)
            End Try
            Try
                If Me._ShuntResistance IsNot Nothing Then Me._ShuntResistance.Dispose() : Me._ShuntResistance = Nothing
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToString)
            End Try
            Try
                If Me._ConfigInfo IsNot Nothing Then Me._ConfigInfo.Dispose() : Me._ConfigInfo = Nothing
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToString)
            End Try
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred closing Master Device;. Details: {0}", ex)
        End Try

    End Sub

    ''' <summary>
    ''' Called by the master device when stating the closing sequence before disposing
    ''' of the subsystems.
    ''' </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MasterDevice_Closing(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MasterDevice.Closing
        If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then Me.OnClosing()
    End Sub

    ''' <summary> Event handler. Called by the Master Device when closed after all subsystems were disposed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _MasterDevice_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MasterDevice.Closed
        Me.SuspendPublishing()
    End Sub

    ''' <summary> Event handler. Called by the Master Device before initializing the subsystems. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _MasterDevice_Opening(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MasterDevice.Opening
        Me._MeasureSequencer = New MeasureSequencer
        Me._TriggerSequencer = New TriggerSequencer
        Me.SuspendPublishing()
    End Sub

    ''' <summary> Event handler. Called by MasterDevice when opened after initializing the subsystems. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MasterDevice_Opened(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MasterDevice.Opened
        Try
            Me.ConfigInfo = New DeviceUnderTest

            ' initial resistance must be first.
            Me.InitialResistance = New MeterColdResistance(Me.MasterDevice.StatusSubsystem, Me.ConfigInfo.InitialResistance,
                                                           ThermalTransientMeterEntity.InitialResistance)
            Me.MasterDevice.AddSubsystem(Me.InitialResistance)

            Me.FinalResistance = New MeterColdResistance(Me.MasterDevice.StatusSubsystem, Me.ConfigInfo.FinalResistance,
                                                         ThermalTransientMeterEntity.FinalResistance)
            Me.MasterDevice.AddSubsystem(Me.FinalResistance)

            Me.ShuntResistance = New ShuntResistance()

            Me.ThermalTransientEstimator = New ThermalTransientEstimator(Me.MasterDevice.StatusSubsystem)
            Me.MasterDevice.AddSubsystem(Me.ThermalTransientEstimator)

            Me.ThermalTransient = New MeterThermalTransient(Me.MasterDevice.StatusSubsystem, Me.ConfigInfo.ThermalTransient)
            Me.MasterDevice.AddSubsystem(Me.ThermalTransient)

            Me.ResumePublishing()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred initiating TTM elements;. Details: {0}", ex)
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MasterDevice_Initializing(sender As Object, e As ComponentModel.CancelEventArgs) Handles _MasterDevice.Initializing
        Try

        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred initializing TTM elements;. Details: {0}", ex)
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MasterDevice_Initialized(sender As Object, e As EventArgs) Handles _MasterDevice.Initialized
        Try
            Me.ShuntResistance.AddListeners(Me.Talker.Listeners)
            ' this is already done.  Me.MasterDevice.AddListeners(Me.Talker.Listeners)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred while TTM elements initialized;. Details: {0}", ex)
        End Try
    End Sub

    ''' <summary> Raises the system. component model. property changed event. </summary>
    ''' <remarks> David, 3/25/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As Device, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.IsDeviceOpen)
                If sender.IsDeviceOpen Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "{0} open;. ", sender.ResourceName)
                Else
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "{0} close;. ", sender.ResourceName)
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _MasterDevice for property changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MasterDevice_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _MasterDevice.PropertyChanged
        Try
            Me.OnPropertyChanged(TryCast(sender, Device), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "", "Exception handling property '{0}'. Details: {1}.",
                               e.PropertyName, ex.Message)
        End Try
    End Sub

#End Region

#Region " PRESET "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    ''' <remarks> This erases the last reading. </remarks>
    Public Overridable Sub ClearExecutionState() Implements IPresettable.ClearExecutionState

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing device under test execution state;. ")
        Windows.Forms.Application.DoEvents()
        Me.ConfigInfo.ClearExecutionState()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing shunt execution state;. ")
        Windows.Forms.Application.DoEvents()
        Me.ShuntResistance.ClearExecutionState()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing master device execution state;. ")
        Windows.Forms.Application.DoEvents()
        Me.MasterDevice.ClearExecutionState()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Displaying title;. ")
        Me.MasterDevice.DisplaySubsystem.DisplayTitle("TTM 2016", "Integrated Scientific Resources")

        Windows.Forms.Application.DoEvents()

    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Overridable Sub InitKnownState() Implements IPresettable.InitKnownState
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Initializing part configuration  to known state;. ")
        Windows.Forms.Application.DoEvents()
        Me.ConfigInfo.InitKnownState()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Initializing shunt to known state;. ")
        Windows.Forms.Application.DoEvents()
        Me.ShuntResistance.InitKnownState()

        Windows.Forms.Application.DoEvents()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Initializing master device to known state;. ")
        Me.MasterDevice.InitKnownState()

    End Sub

    ''' <summary> Sets the subsystem to its preset state. </summary>
    Public Overridable Sub PresetKnownState() Implements IPresettable.PresetKnownState
        Me.ConfigInfo.PresetKnownState()
    End Sub

    ''' <summary> Preforms a full reset, initialize and clear. </summary>
    ''' <remarks> David, 1/8/2016. </remarks>
    Public Sub ResetClear()

        Me.ResetKnownState()
        Me.InitKnownState()
        Me.ClearExecutionState()

    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overridable Sub ResetKnownState() Implements IPresettable.ResetKnownState

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Resetting part configuration to known state;. ")
        Windows.Forms.Application.DoEvents()
        Me.ConfigInfo.ResetKnownState()

        Windows.Forms.Application.DoEvents()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Resetting shunt to known state;. ")
        Windows.Forms.Application.DoEvents()
        Me.ShuntResistance.ResetKnownState()

        Windows.Forms.Application.DoEvents()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Resetting master device to known state;. ")
        Me.MasterDevice.ResetKnownState()

        Windows.Forms.Application.DoEvents()
        Me._isStarting = True
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Resumes the property events. </summary>
    Public Overrides Sub ResumePublishing()
        MyBase.ResumePublishing()
        Me.ShuntResistance?.ResumePublishing()
        Me.MasterDevice?.ResumePublishing()
    End Sub

    ''' <summary> Suppresses the property events. </summary>
    Public Overrides Sub SuspendPublishing()
        MyBase.SuspendPublishing()
        Me.ShuntResistance?.SuspendPublishing()
        Me.MasterDevice?.SuspendPublishing()
    End Sub

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        Me.ShuntResistance?.Publish()
        Me.MasterDevice?.Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.AsyncNotifyPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS: MEASUREMENT ELEMENTS "

    ''' <summary> Gets or sets reference to the <see cref="MeterColdResistance">meter initial cold
    ''' resistance</see> </summary>
    ''' <value> The initial resistance. </value>
    Public Property InitialResistance() As MeterColdResistance

    ''' <summary> Gets or sets reference to the <see cref="MeterColdResistance">meter Final cold resistance</see> </summary>
    ''' <value> The final resistance. </value>
    Public Property FinalResistance() As MeterColdResistance

    ''' <summary> Gets or sets reference to the <see cref="ShuntResistance">Shunt resistance</see> </summary>
    ''' <value> The shunt resistance. </value>
    Public Property ShuntResistance() As ShuntResistance

    ''' <summary> Gets or sets reference to the <see cref="MeterThermalTransient">meter thermal transient</see> </summary>
    ''' <value> The thermal transient. </value>
    Public Property ThermalTransient() As MeterThermalTransient

    ''' <summary> Gets or sets reference to the <see cref="ThermalTransientEstimator">thermal transient estiamtor</see> </summary>
    ''' <value> The thermal transient estimator. </value>
    Public Property ThermalTransientEstimator() As ThermalTransientEstimator

#End Region

#Region " CONFIGURE"

    ''' <summary> Gets or sets the <see cref="ConfigInfo">Device under test</see>. </summary>
    Public Property ConfigInfo As DeviceUnderTest

    ''' <summary> Configure part information. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="configurationInformation"> The <see cref="ConfigInfo">Device under test</see>. </param>
    Public Sub ConfigurePartInfo(ByVal configurationInformation As DeviceUnderTest)
        If configurationInformation Is Nothing Then Throw New ArgumentNullException(NameOf(configurationInformation))
        Me.ConfigInfo.PartNumber = configurationInformation.PartNumber
        Me.ConfigInfo.OperatorId = configurationInformation.OperatorId
        Me.ConfigInfo.LotId = configurationInformation.LotId

        Me.ConfigInfo.ContactCheckEnabled = configurationInformation.ContactCheckEnabled
        Me.ConfigInfo.ContactCheckThreshold = configurationInformation.ContactCheckThreshold
    End Sub

    ''' <summary> Configures the given device under test. </summary>
    ''' <param name="configurationInformation"> The <see cref="ConfigInfo">Device under test</see>. </param>
    Public Sub Configure(ByVal configurationInformation As DeviceUnderTest)
        If configurationInformation Is Nothing Then
            Throw New ArgumentNullException(NameOf(configurationInformation))
        ElseIf configurationInformation.InitialResistance Is Nothing Then
            Throw New InvalidOperationException("Initial Resistance null detected in device under test.")
        ElseIf configurationInformation.ThermalTransient Is Nothing Then
            Throw New InvalidOperationException("Thermal Transient null detected in device under test.")
        End If
        If Not Me.ConfigInfo.SourceMeasureUnitEquals(configurationInformation) Then
            If Not Me.InitialResistance.SourceMeasureUnitExists(Me.ConfigInfo.InitialResistance.SourceMeasureUnit) Then
                Throw New InvalidOperationException(String.Format("Source measure unit name {0} is invalid.",
                                                                  Me.ConfigInfo.InitialResistance.SourceMeasureUnit))
            End If
        End If
        Dim details As String = ""
        If Not Ttm.ThermalTransient.ValidateVoltageLimit(configurationInformation.ThermalTransient.VoltageLimit,
                                                         configurationInformation.InitialResistance.HighLimit, configurationInformation.ThermalTransient.CurrentLevel, configurationInformation.ThermalTransient.AllowedVoltageChange, details) Then
            Throw New ArgumentOutOfRangeException(NameOf(configurationInformation), details)
        End If

        Me.ConfigurePartInfo(configurationInformation)
        Me.InitialResistance.Configure(configurationInformation.InitialResistance)
        Me.FinalResistance.Configure(configurationInformation.FinalResistance)
        Me.ThermalTransient.Configure(configurationInformation.ThermalTransient)
    End Sub

    ''' <summary> Configures the given device under test for changed values. </summary>
    ''' <param name="configurationInformation "> The <see cref="ConfigInfo">Device under test</see>. </param>
    Public Sub ConfigureChanged(ByVal configurationInformation As DeviceUnderTest)
        If configurationInformation Is Nothing Then
            Throw New ArgumentNullException(NameOf(configurationInformation))
        ElseIf configurationInformation.InitialResistance Is Nothing Then
            Throw New InvalidOperationException("Initial Resistance null detected in device under test.")
        ElseIf configurationInformation.ThermalTransient Is Nothing Then
            Throw New InvalidOperationException("Thermal Transient null detected in device under test.")
        End If
        If Not Me.ConfigInfo.SourceMeasureUnitEquals(configurationInformation) Then
            If Not Me.InitialResistance.SourceMeasureUnitExists(Me.ConfigInfo.InitialResistance.SourceMeasureUnit) Then
                Throw New InvalidOperationException(String.Format("Source measure unit name {0} is invalid.",
                                                                  Me.ConfigInfo.InitialResistance.SourceMeasureUnit))
            End If
        End If
        Dim details As String = ""
        If Not Ttm.ThermalTransient.ValidateVoltageLimit(configurationInformation.ThermalTransient.VoltageLimit,
                                                            configurationInformation.InitialResistance.HighLimit, configurationInformation.ThermalTransient.CurrentLevel, configurationInformation.ThermalTransient.AllowedVoltageChange, details) Then
            Throw New ArgumentOutOfRangeException(NameOf(configurationInformation), details)
        End If
        Me.ConfigurePartInfo(configurationInformation)
        Me.InitialResistance.ConfigureChanged(configurationInformation.InitialResistance)
        Me.FinalResistance.ConfigureChanged(configurationInformation.FinalResistance)
        Me.ThermalTransient.ConfigureChanged(configurationInformation.ThermalTransient)
    End Sub

#End Region

#Region " TTM FRAMEWORK: SOURCE CHANNEL "

    Private _SourceMeasureUnit As String

    ''' <summary> Gets or sets (protected) the source measure unit "smua" or "smub". </summary>
    ''' <value> The source measure unit. </value>
    Public Property SourceMeasureUnit() As String
        Get
            Return Me._SourceMeasureUnit
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.SourceMeasureUnit) Then
                Me._SourceMeasureUnit = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Programs the Source Measure Unit. Does not read back from the instrument. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value"> the Source Measure Unit, e.g., 'smua' or 'smub'. </param>
    ''' <returns> The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    Public Function WriteSourceMeasureUnit(ByVal value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Throw New ArgumentNullException(value)
        End If
        Dim unitNumber As String = value.Substring(value.Length - 1)
        Me.MasterDevice.SourceMeasureUnit.UnitNumber = unitNumber
        Me.MasterDevice.SourceMeasureUnitCurrentSource.UnitNumber = unitNumber
        Me.InitialResistance.WriteSourceMeasureUnit(value)
        Me.FinalResistance.WriteSourceMeasureUnit(value)
        Me.ThermalTransient.WriteSourceMeasureUnit(value)
        Me.SourceMeasureUnit = value
        Return Me.SourceMeasureUnit
    End Function

#End Region

#Region " TTM FRAMEWORK: METERS FIRMWARE "

    ''' <summary> Checks if the firmware version command exists. </summary>
    ''' <returns> <c>True</c> if the firmware version command exists; otherwise, <c>False</c>. </returns>
    Public Function FirmwareVersionQueryCommandExists() As Boolean
        If Me.IsSessionOpen Then
            Return Not Me.MasterDevice.Session.IsNil(TtmSyntax.Meters.VersionQueryCommand.TrimEnd("()".ToCharArray))
        Else
            Return False
        End If
    End Function

    Private _firmwareReleasedVersion As String
    ''' <summary> Gets or sets the version of the current firmware release. </summary>
    ''' <value> The firmware released version. </value>
    Public Property FirmwareReleasedVersion() As String
        Get
            If String.IsNullOrWhiteSpace(Me._firmwareReleasedVersion) Then
                Me.QueryFirmwareVersion()
            End If
            Return Me._firmwareReleasedVersion
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.FirmwareReleasedVersion) Then
                Me._firmwareReleasedVersion = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries the embedded firmware version from a remote node and saves it to
    ''' <see cref="FirmwareReleasedVersion">the firmware version cache.</see> </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> The firmware version. </returns>
    Public Function QueryFirmwareVersion() As String
        If Me.FirmwareVersionQueryCommandExists() Then
            Me.FirmwareReleasedVersion = Me.MasterDevice.Session.QueryPrintTrimEnd(TtmSyntax.Meters.VersionQueryCommand)
        Else
            Me.FirmwareReleasedVersion = TspSyntax.NilValue
        End If
        Return Me.FirmwareReleasedVersion
    End Function

#End Region

#Region " TTM FRAMEWORK: SHUNT "

    ''' <summary> Configures the meter for making Shunt Resistance measurements. </summary>
    ''' <param name="resistance"> The shunt resistance. </param>
    Public Sub ConfigureShuntResistance(ByVal resistance As ShuntResistanceBase)

        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Configuring shunt resistance measurement;. ")

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to DC current source;. ", Me.MasterDevice.SourceMeasureUnit)
        Me.MasterDevice.SourceMeasureUnit.ApplySourceFunction(Tsp.SourceFunctionMode.CurrentDC)

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to DC current range {1};. ", Me.MasterDevice.SourceMeasureUnit)
        Me.MasterDevice.SourceMeasureUnitCurrentSource.ApplyRange(resistance.CurrentRange)
        Me.ShuntResistance.CurrentRange = Me.MasterDevice.SourceMeasureUnitCurrentSource.Range.GetValueOrDefault(0)

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to DC current Level {1};. ", Me.MasterDevice.SourceMeasureUnit)
        Me.MasterDevice.SourceMeasureUnitCurrentSource.ApplyLevel(resistance.CurrentLevel)
        Me.ShuntResistance.CurrentLevel = Me.MasterDevice.SourceMeasureUnitCurrentSource.Level.GetValueOrDefault(0)

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to DC current Voltage Limit {1};. ", Me.MasterDevice.SourceMeasureUnit)
        Me.MasterDevice.SourceMeasureUnitCurrentSource.ApplyVoltageLimit(resistance.VoltageLimit)
        Me.ShuntResistance.VoltageLimit = Me.MasterDevice.SourceMeasureUnitCurrentSource.VoltageLimit.GetValueOrDefault(0)

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to four wire sense;. ", Me.MasterDevice.SourceMeasureUnit)
        Me.MasterDevice.SourceMeasureUnit.ApplySenseMode(SenseActionMode.Remote)

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to auto voltage range;. ", Me.MasterDevice.SourceMeasureUnit)
        Me.MasterDevice.SourceMeasureUnitMeasure.ApplyAutoRangeVoltageEnabled(True)

        Me.MasterDevice.DisplaySubsystem.ClearDisplay()
        Me.MasterDevice.DisplaySubsystem.DisplayLine(2, String.Format(Globalization.CultureInfo.CurrentCulture,
                                                                      "SrcI:{0}A LimV:{1}V",
                                                                      Me.ShuntResistance.CurrentLevel,
                                                                      Me.ShuntResistance.VoltageLimit))
        Me.MasterDevice.StatusSubsystem.CheckThrowDeviceException(False, "configuring shunt resistance measurement;. ")

        Me.ShuntResistance.CheckThrowUnequalConfiguration(resistance)

    End Sub

    ''' <summary> Apply changed Shunt Resistance configuration. </summary>
    ''' <param name="resistance"> The shunt resistance. </param>
    Public Sub ConfigureShuntResistanceChanged(ByVal resistance As ShuntResistanceBase)

        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to DC current source;. ", Me.MasterDevice.SourceMeasureUnit)
        Me.MasterDevice.SourceMeasureUnit.ApplySourceFunction(Tsp.SourceFunctionMode.CurrentDC)

        If Not Me.ShuntResistance.ConfigurationEquals(resistance) Then

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Configuring shunt resistance measurement;. ")

            If Not Me.ShuntResistance.CurrentRange.Equals(resistance.CurrentRange) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to DC current range {1};. ", Me.MasterDevice.SourceMeasureUnit)
                Me.MasterDevice.SourceMeasureUnitCurrentSource.ApplyRange(resistance.CurrentRange)
                Me.ShuntResistance.CurrentRange = Me.MasterDevice.SourceMeasureUnitCurrentSource.Range.GetValueOrDefault(0)
            End If

            If Not Me.ShuntResistance.CurrentLevel.Equals(resistance.CurrentLevel) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to DC current Level {1};. ", Me.MasterDevice.SourceMeasureUnit)
                Me.MasterDevice.SourceMeasureUnitCurrentSource.ApplyLevel(resistance.CurrentLevel)
                Me.ShuntResistance.CurrentLevel = Me.MasterDevice.SourceMeasureUnitCurrentSource.Level.GetValueOrDefault(0)
            End If

            If Not Me.ShuntResistance.VoltageLimit.Equals(resistance.VoltageLimit) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to DC current Voltage Limit {1};. ", Me.MasterDevice.SourceMeasureUnit)
                Me.MasterDevice.SourceMeasureUnitCurrentSource.ApplyVoltageLimit(resistance.VoltageLimit)
                Me.ShuntResistance.VoltageLimit = Me.MasterDevice.SourceMeasureUnitCurrentSource.VoltageLimit.GetValueOrDefault(0)
            End If

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to four wire sense;. ", Me.MasterDevice.SourceMeasureUnit)
            Me.MasterDevice.SourceMeasureUnit.ApplySenseMode(SenseActionMode.Remote)

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} to auto voltage range;. ", Me.MasterDevice.SourceMeasureUnit)
            Me.MasterDevice.SourceMeasureUnitMeasure.ApplyAutoRangeVoltageEnabled(True)
        End If

        Me.MasterDevice.DisplaySubsystem.ClearDisplay()
        Me.MasterDevice.DisplaySubsystem.DisplayLine(2, String.Format(Globalization.CultureInfo.CurrentCulture,
                                                                      "SrcI:{0}A LimV:{1}V",
                                                                      Me.ShuntResistance.CurrentLevel,
                                                                      Me.ShuntResistance.VoltageLimit))
        Me.MasterDevice.StatusSubsystem.CheckThrowDeviceException(False, "configuring shunt resistance measurement;. ")

        Me.ShuntResistance.CheckThrowUnequalConfiguration(resistance)

    End Sub

    ''' <summary> Measures the Shunt resistance. </summary>
    ''' <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ''' type. </exception>
    ''' <param name="resistance"> The shunt resistance. </param>
    Public Sub MeasureShuntResistance(ByVal resistance As ResistanceMeasureBase)
        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))
        Me.ShuntResistance.HighLimit = resistance.HighLimit
        Me.ShuntResistance.LowLimit = resistance.LowLimit
        Me.MasterDevice.SourceMeasureUnitMeasure.MeasureResistance()
        Dim reading As String = Me.MasterDevice.SourceMeasureUnitMeasure.Reading
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Parsing shunt resistance reading {0};. ", reading)
        Me.MasterDevice.StatusSubsystem.CheckThrowDeviceException(False, "measuring shunt resistance;. ")
        Me.ShuntResistance.ParseReading(reading, MeasurementOutcomes.None)
        resistance.ParseReading(reading, MeasurementOutcomes.None)
        Me.MasterDevice.DisplaySubsystem.DisplayLine(1, reading.Trim)
        Me.MasterDevice.DisplaySubsystem.DisplayCharacter(1, reading.Length, 18)
        Me.ShuntResistance.MeasurementAvailable = True
    End Sub

#End Region

#Region " TTM FRAMEWORK: MEASURE "

    ''' <summary> Makes all measurements. Reading is done after all measurements are done. </summary>
    Public Sub Measure()
        If Me.IsSessionOpen Then
            Me.MasterDevice.Session.WriteLine("_G.ttm.measure()  waitcomplete() ")
        End If
    End Sub

#End Region

#Region " TTM FRAMEWORK: TRIGGER "

    ''' <summary> True if the meter was enabled to respond to trigger events. </summary>
    Private _isAwaitingTrigger As Boolean

    ''' <summary> Abort triggered measurements. </summary>
    Public Sub AbortTriggerSequenceIf()
        If Me.IsSessionOpen AndAlso Me._isAwaitingTrigger AndAlso Not Me.TriggerSequencer Is Nothing AndAlso Not Me.MasterDevice Is Nothing Then
            Me.TriggerSequencer.RestartSignal = TriggerSequenceSignal.Stop
            Me.MasterDevice.Session.AssertTrigger()
            ' allow time for the measurement to terminate.
            Threading.Thread.Sleep(CInt(1000 * Me.ThermalTransient.ThermalTransient.PostTransientDelay) + 400)
            Me._isAwaitingTrigger = False
            Me.MasterDevice.Session.DiscardUnreadData()
        End If
    End Sub

    ''' <summary> true if this object is starting. </summary>
    Private _isStarting As Boolean

    ''' <summary> Primes the instrument to wait for a trigger. This clears the digital outputs and
    ''' loops until trigger or external *TRG command. </summary>
    ''' <param name="isStarting"> When true, displays the title and locks the local key. </param>
    Private Sub _PrepareForTrigger(ByVal isStarting As Boolean)
        Me.MasterDevice.Session.MessageAvailableBits = VI.ServiceRequests.MessageAvailable
        Me.MasterDevice.Session.EnableWaitComplete()
        Me.MasterDevice.Session.WriteLine("prepareForTrigger({0},'{1}') waitcomplete() ", IIf(isStarting, "true", "false"), "OPC")
    End Sub

    ''' <summary> Primes the instrument to wait for a trigger. This clears the digital outputs and loops
    ''' until trigger or external TRG command. </summary>
    Public Sub PrepareForTrigger()
        Me._isStarting = False
        Me._isAwaitingTrigger = True
        Me._PrepareForTrigger(Me._isStarting)
        Me._isStarting = False
    End Sub

    Private _MeasurementCompleted As Boolean

    ''' <summary> Gets or sets the measurement completed. </summary>
    ''' <value> The measurement completed. </value>
    Public Property MeasurementCompleted As Boolean
        Get
            Return Me._MeasurementCompleted
        End Get
        Set(ByVal value As Boolean)
            If Not value.Equals(Me.MeasurementCompleted) Then
                Me._MeasurementCompleted = value
                Me.SyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Monitors the bus to detect completion of measurement. </summary>
    Public Function IsMeasurementCompleted() As Boolean
        Me.MasterDevice.Session.ReadServiceRequestStatus()
        Me.MeasurementCompleted = Me.MasterDevice.Session.MessageAvailable '  Me.MasterDevice.Session.IsMessageAvailable(TimeSpan.FromMilliseconds(50), 1)
        Return Me.MeasurementCompleted
    End Function

    ''' <summary> Reads the measurements from the instrument. </summary>
    ''' <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ''' type. </exception>
    ''' <param name="initialResistance"> The initial resistance. </param>
    ''' <param name="finalResistance">   The final resistance. </param>
    ''' <param name="thermalTransient">  The thermal transient. </param>
    Public Sub ReadMeasurements(ByVal initialResistance As ResistanceMeasureBase, ByVal finalResistance As ResistanceMeasureBase,
                                     ByVal thermalTransient As ResistanceMeasureBase)
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "'{0}' reading measurements;. ", Me.ResourceName)
        Me._isAwaitingTrigger = False
        Me.MasterDevice.Session.DiscardUnreadData()
        Me.InitialResistance.ReadResistance(initialResistance)
        Me.FinalResistance.ReadResistance(finalResistance)
        Me.ThermalTransient.ReadThermalTransient(thermalTransient)
    End Sub

#End Region

#Region " CHECK CONTACTS "

    ''' <summary> Checks contact resistance. </summary>
    Public Sub CheckContacts(ByVal threshold As Integer)
        Me.MasterDevice.SourceMeasureUnit.CheckContacts(threshold)
        If Not Me.MasterDevice.SourceMeasureUnit.ContactCheckOkay.HasValue Then
            Throw New OperationFailedException("Failed Measuring contacts;. ")
        ElseIf Not Me.MasterDevice.SourceMeasureUnit.ContactCheckOkay.Value Then
            Throw New OperationFailedException("High contact resistances;. Values: '{0}'", Me.MasterDevice.SourceMeasureUnit.ContactResistances)
        End If
    End Sub

    ''' <summary> Checks contact resistance. </summary>
    ''' <param name="threshold"> The threshold. </param>
    ''' <param name="details">   [in,out] The details. </param>
    ''' <returns> <c>True</c> if contacts checked okay. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Function TryCheckContacts(ByVal threshold As Integer, ByRef details As String) As Boolean
        Me.MasterDevice.SourceMeasureUnit.CheckContacts(threshold)
        If Not Me.MasterDevice.SourceMeasureUnit.ContactCheckOkay.HasValue Then
            details = "Failed Measuring contacts;. "
            Return False
        ElseIf Me.MasterDevice.SourceMeasureUnit.ContactCheckOkay.Value Then
            Return True
        Else
            details = String.Format("High contact resistances;. Values: '{0}'",
                                    Me.MasterDevice.SourceMeasureUnit.ContactResistances)
            Return False
        End If
    End Function

#End Region

#Region " PART MEASUREMENTS "

    ''' <summary> Gets the part. </summary>
    ''' <remarks> The part represents the actual device under test. These are the values that gets
    ''' displayed on the header and saved. </remarks>
    ''' <value> The part. </value>
    Public Property Part As DeviceUnderTest

    ''' <summary> Measures and fetches the initial resistance. </summary>
    ''' <param name="value"> The cold resistance entity where the results are saved. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")>
    Public Sub MeasureInitialResistance(ByVal value As ColdResistance)
        ' clears display if not in measurement state
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing meter display;. ")
        Me.MasterDevice.DisplaySubsystem.ClearDisplayMeasurement()
        Dim contactsOkay As Boolean
        Dim details As String = ""
        If Me.Part.ContactCheckEnabled Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Checking contacts;. ")
            contactsOkay = Me.TryCheckContacts(Me.Part.ContactCheckThreshold, details)
        Else
            contactsOkay = True
        End If
        If contactsOkay Then
            If Me.MasterDevice.Enabled Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measuring Initial Resistance...;. ")
                Me.InitialResistance.Measure(value)
            Else
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Emulating Initial Resistance...;. ")
                Me.InitialResistance.EmulateResistance(value)
            End If
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Got Initial Resistance;. ")
        Else
            Me.Part.InitialResistance.ApplyContactCheckOutcome(MeasurementOutcomes.FailedContactCheck)
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, details)
        End If
    End Sub

    ''' <summary> Measures and fetches the Final resistance. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")>
    Public Sub MeasureFinalResistance(ByVal value As ColdResistance)
        If Me.MasterDevice.Enabled Then
            ' clears display if not in measurement state
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing meter display;. ")
            Me.MasterDevice.DisplaySubsystem.ClearDisplayMeasurement()
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measuring Final Resistance...;. ")
            Me.FinalResistance.Measure(value)
        Else
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Emulating Final Resistance...;. ")
            Me.FinalResistance.EmulateResistance(value)
        End If
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Got Final Resistance;. ")
    End Sub

    ''' <summary> Measures and fetches the thermal transient. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")>
    Public Sub MeasureThermalTransient(ByVal value As ThermalTransient)
        ' clears display if not in measurement state
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing meter display;. ")
        Me.MasterDevice.DisplaySubsystem.ClearDisplayMeasurement()
        If Me.MasterDevice.Enabled Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measuring Thermal Transient...;. ")
            Me.ThermalTransient.Measure(value)
        Else
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Emulating Thermal Transient...;. ")
            Me.ThermalTransient.EmulateThermalTransient(value)
        End If
        If Me.MeasureSequencer IsNot Nothing Then
            Me.MeasureSequencer.StartFinalResistanceTime = DateTime.Now.AddSeconds(Me.Part.ThermalTransient.PostTransientDelay)
        End If
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Got Thermal Transient;. ")
    End Sub

#End Region

#Region " SEQUENCED MEASUREMENTS "

    Private WithEvents _MeasureSequencer As MeasureSequencer

    ''' <summary> Gets the sequencer. </summary>
    ''' <value> The sequencer. </value>
    Public ReadOnly Property MeasureSequencer As MeasureSequencer
        Get
            Return Me._MeasureSequencer
        End Get
    End Property

    ''' <summary> Handles the measure sequencer property changed event. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As MeasureSequencer, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.MeasurementSequenceState)
                Me.onMeasurementSequenceStateChanged(sender.MeasurementSequenceState)
        End Select
    End Sub

    ''' <summary> Event handler. Called by _MeasureSequencer for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MeasureSequencer_PropertyChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _MeasureSequencer.PropertyChanged
        Try
            Me.OnPropertyChanged(TryCast(sender, MeasureSequencer), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

    ''' <summary> Event handler. Called by  for  events. </summary>
    ''' <param name="currentState"> The current state. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Sub onMeasurementSequenceStateChanged(ByVal currentState As MeasurementSequenceState)

        Select Case currentState

            Case MeasurementSequenceState.Idle

                ' Waiting for the step signal to start.
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                           "Ready to start a measurement sequence;. ")

            Case MeasurementSequenceState.Aborted

                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                           "Measurement sequence aborted;. ")
                ' step to the idle state.
                Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Step)

            Case MeasurementSequenceState.Failed

                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                           "Measurement sequence failed;. ")
                If Me.IsSessionOpen Then
                    Try
                        ' flush the buffer so a time out error that might leave stuff will clear the buffers.
                        Me.MasterDevice.Session.DiscardUnreadData()
                    Catch
                    End Try
                End If
                ' step to the aborted state.
                Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Step)

            Case MeasurementSequenceState.Completed

                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                           "Measurement sequence completed;. ")

                ' step to the idle state.
                Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Step)

            Case MeasurementSequenceState.Starting

                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                           "Measurement sequence starting;. ")

                Me.Part.ClearMeasurements()

                ' step to the Measure Initial Resistance state.
                Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Step)

            Case MeasurementSequenceState.MeasureInitialResistance

                Try
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                               "Measuring initial resistance;. ")
                    Me.MeasureInitialResistance(Me.Part.InitialResistance)
                    If Me.Part.InitialResistance.Outcome = MeasurementOutcomes.PartPassed Then
                        ' step to the Measure Thermal Transient.
                        Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Step)
                    Else
                        ' if failure, do not continue.
                        Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Failure)
                    End If
                Catch ex As Exception
                    Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                               "Exception occurred measuring initial resistance;. Details: {0}", ex)
                    Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Failure)
                End Try

            Case MeasurementSequenceState.MeasureThermalTransient

                Try
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                               "Measuring thermal transient;. ")
                    Me.MeasureThermalTransient(Me.Part.ThermalTransient)
                    ' step to the post transient delay
                    Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Step)

                Catch ex As Exception
                    Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                               "Exception occurred measuring thermal resistance;. Details: {0}", ex)
                    Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Failure)
                End Try

            Case MeasurementSequenceState.PostTransientPause

                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                           "Delaying final resistance measurement;. ")

            Case MeasurementSequenceState.MeasureFinalResistance

                Try
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                               "Measuring final transient;. ")
                    Me.MeasureFinalResistance(Me.Part.FinalResistance)
                    ' step to the completed state
                    Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Step)

                Catch ex As Exception
                    Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                               "Exception occurred measuring final resistance;. Details: {0}", ex)
                    Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Failure)
                End Try

            Case Else

                Debug.Assert(Not Debugger.IsAttached, "Unhandled state: " & currentState.ToString)

        End Select

    End Sub

#End Region

#Region " TRIGGER SEQUENCE "

    Private WithEvents _TriggerSequencer As TriggerSequencer

    ''' <summary> Gets the trigger sequencer. </summary>
    ''' <value> The sequencer. </value>
    Public ReadOnly Property TriggerSequencer As TriggerSequencer
        Get
            Return Me._TriggerSequencer
        End Get
    End Property

    ''' <summary> Handles the trigger sequencer property changed event. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As TriggerSequencer, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.AssertRequested)
                If sender.AssertRequested Then
                    Me.MasterDevice.Session.AssertTrigger()
                End If
            Case NameOf(sender.TriggerSequenceState)
                Me.onTriggerSequenceStateChanged(sender.TriggerSequenceState)
        End Select
    End Sub

    ''' <summary> Event handler. Called by _TriggerSequencer for property changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TriggerSequencer_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _TriggerSequencer.PropertyChanged
        Try
            Me.OnPropertyChanged(TryCast(sender, TriggerSequencer), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

    ''' <summary> Event handler. Called by  for  events. </summary>
    ''' <param name="currentState"> The current state. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Sub onTriggerSequenceStateChanged(ByVal currentState As TriggerSequenceState)

        Select Case currentState

            Case TriggerSequenceState.Idle

                If Me.IsSessionOpen Then
                    Try
                        ' flush the buffer so a time out error that might leave stuff will clear the buffers.
                        Me.MasterDevice.Session.DiscardUnreadData()
                    Catch
                    End Try
                End If

                ' Waiting for the step signal to start.
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                           "Ready to start a trigger sequence;. ")

            Case TriggerSequenceState.Aborted

                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                           "Measurement sequence aborted;. ")

                ' step to the idle state.
                Me.TriggerSequencer.Enqueue(TriggerSequenceSignal.Step)

            Case TriggerSequenceState.Failed

                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                           "Measurement sequence failed;. ")
                ' step to the aborted state.
                Me.TriggerSequencer.Enqueue(TriggerSequenceSignal.Step)

            Case TriggerSequenceState.Stopped

                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                           "Measurement sequence stopped;. ")

                ' step to the idle state.
                Me.TriggerSequencer.Enqueue(TriggerSequenceSignal.Step)

            Case TriggerSequenceState.Starting

                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                           "Measurement sequence starting;. ")
                Try
                    Me.PrepareForTrigger()
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Monitoring instrument for measurements;. ")
                    ' step to the Waiting state.
                    Me.TriggerSequencer.Enqueue(TriggerSequenceSignal.Step)
                Catch ex As Exception
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                               "Exception occurred preparing instrument for waiting for trigger;. Details:{0}", ex)
                    ' step to the failed state.
                    Me.TriggerSequencer.Enqueue(TriggerSequenceSignal.Failure)
                End Try

            Case TriggerSequenceState.WaitingForTrigger

                If Me.IsMeasurementCompleted Then
                    ' step to the reading state.
                    Me.TriggerSequencer.Enqueue(TriggerSequenceSignal.Step)
                ElseIf Me.TriggerSequencer.AssertRequested Then
                    Me.TriggerSequencer.AssertRequested = False
                    Me.MasterDevice.Session.AssertTrigger()
                End If

            Case TriggerSequenceState.MeasurementCompleted

                ' clear part measurements.
                Me.Part.ClearMeasurements()

                ' step to the reading state.
                Me.TriggerSequencer.Enqueue(TriggerSequenceSignal.Step)

            Case TriggerSequenceState.ReadingValues

                Try
                    Me.ReadMeasurements(Me.Part.InitialResistance, Me.Part.FinalResistance, Me.Part.ThermalTransient)
                    ' step to the starting state.
                    Me.TriggerSequencer.Enqueue(TriggerSequenceSignal.Step)
                Catch ex As Exception
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                               "Exception occurred reading measurements;. Details:{0}", ex)
                    ' step to the failed state.
                    Me.TriggerSequencer.Enqueue(TriggerSequenceSignal.Failure)
                End Try

            Case Else

                Debug.Assert(Not Debugger.IsAttached, "Unhandled state: " & currentState.ToString)

        End Select

    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Gets the trace message talker. </summary>
    ''' <value> The trace message talker. </value>
    Public ReadOnly Property Talker As ITraceMessageTalker

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="listeners"> The listeners. </param>
    Public Overridable Sub AddListeners(ByVal listeners As IEnumerable(Of ITraceMessageListener)) Implements ITalker.AddListeners
        Me.Talker.Listeners.Add(listeners)
    End Sub

    ''' <summary> Clears the listeners. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    Public Sub ClearListeners() Implements ITalker.ClearListeners
        Me.Talker.Listeners?.Clear()
        Me.ShuntResistance.ClearListeners()
        Me.MasterDevice.ClearListeners()
    End Sub

    ''' <summary> Adds subsystem listeners. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    Public Overridable Sub AddSubsystemListeners()
        Me.ShuntResistance.AddListeners(Me.Talker.Listeners)
        Me.MasterDevice.AddListeners(Me.Talker.Listeners)
    End Sub

    ''' <summary> Clears the subsystem listeners. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    Public Overridable Sub ClearSubsystemListeners()
        Me.MasterDevice.ClearListeners()
    End Sub

#End Region

End Class
