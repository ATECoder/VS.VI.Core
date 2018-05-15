Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.CheckBoxExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.StopwatchExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Provides a user interface for the Thermo Stream Device. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2015" by="David" revision="2.0.2936.x"> Create based on the 27xx
''' system classes. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
<System.ComponentModel.DisplayName("Thermo Stream Panel"),
      System.ComponentModel.Description("In Test Thermo Stream Device Panel"),
      System.Drawing.ToolboxBitmap(GetType(Thermostream.ThermostreamPanel))>
Public Class ThermostreamPanel
    Inherits VI.Instrument.ResourcePanelBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(New Device)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Public Sub New(ByVal device As Device)
        MyBase.New(device)
        Me.InitializingComponents = True
        Me.InitializeComponent()
        Me.InitializingComponents = False
        Me._SetpointWindowNumeric.ReadOnly = True
        Me._SetpointNumeric.ReadOnly = True
        Me._SoakTimeNumeric.ReadOnly = True
        Me._AssignDevice(device)
        ' note that the caption is not set if this is run inside the On Load function.
        With Me.TraceMessagesBox
            ' set defaults for the messages box.
            .ResetCount = 500
            .PresetCount = 250
            .ContainerPanel = Me._MessagesTabPage
        End With
        With Me._ServiceRequestEnableNumeric.NumericUpDownControl
            .Hexadecimal = True
            .Maximum = 255
            .Minimum = 0
            .Value = 0
        End With
        Me.EnableTraceLevelControls()
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the isr.VI.Instrument.ResourcePanelBase and
    ''' optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Try
                    Me.Device?.RemovePrivateListener(Me.TraceMessagesBox)
                    If Me.Device IsNot Nothing Then Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", "Exception {0}", ex.ToFullBlownString)
                End Try
                ' the device gets disposed in the base class!
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " FORM EVENTS "

    ''' <summary> Handles the <see cref="E:System.Windows.Forms.UserControl.Load" /> event. </summary>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Try
            Me.DisplaySensorTypes()
        Finally
            MyBase.OnLoad(e)
        End Try
    End Sub

#End Region

#Region " DEVICE "

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignDevice(ByVal value As Device)
        Me._Device = value
        Me._Device.CaptureSyncContext(Threading.SynchronizationContext.Current)
        Me._Device.AddPrivateListener(Me.TraceMessagesBox)
        Me.OnDeviceOpenChanged(value)
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device)
        Me.IsDeviceOwner = False
        MyBase.AssignDevice(value)
        Me._AssignDevice(value)
    End Sub

    ''' <summary> Releases the device. </summary>
    Protected Overrides Sub ReleaseDevice()
        If Me.IsDeviceOwner Then
            MyBase.ReleaseDevice()
        Else
            Me._Device = Nothing
        End If
    End Sub

    ''' <summary> Gets a reference to the InTest Thermo Stream Device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overloads ReadOnly Property Device() As Device

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
    ''' <param name="device"> The device. </param>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As VI.DeviceBase)
        MyBase.OnDeviceOpenChanged(device)
        If Me.IsDeviceOpen Then
            Me._SimpleReadWriteControl.Connect(device?.Session)
        Else
            Me._SimpleReadWriteControl.Disconnect()
        End If
        For Each t As Windows.Forms.TabPage In Me._Tabs.TabPages
            If t IsNot Me._MessagesTabPage Then Me.RecursivelyEnable(t.Controls, Me.IsDeviceOpen)
        Next
        If Me.IsDeviceOpen Then
            Me._ClearInterfaceMenuItem.Visible = Me.Device.StatusSubsystemBase.SupportsClearInterface
            VI.Pith.SessionBase.ListNotificationLevels(Me._SessionNotificationLevelComboBox.ComboBox)
            AddHandler Me._SessionNotificationLevelComboBox.ComboBox.SelectedIndexChanged, AddressOf Me._SessionNotificationLevelComboBox_SelectedIndexChanged
            VI.Pith.SessionBase.SelectItem(Me._SessionNotificationLevelComboBox, NotifySyncLevel.None)
        End If
    End Sub


    ''' <summary> Handles the device property changed event. </summary>
    ''' <param name="device">       The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub HandlePropertyChange(ByVal device As VI.DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.HandlePropertyChange(device, propertyName)
        Select Case propertyName
            Case NameOf(isr.VI.DeviceBase.SessionServiceRequestEventEnabled)
                Me._SessionServiceRequestHandlerEnabledMenuItem.Checked = device.SessionServiceRequestEventEnabled
            Case NameOf(isr.VI.DeviceBase.DeviceServiceRequestHandlerAdded)
                Me._DeviceServiceRequestHandlerEnabledMenuItem.Checked = device.DeviceServiceRequestHandlerAdded
            Case NameOf(isr.VI.DeviceBase.MessageNotificationLevel)
                VI.Pith.SessionBase.SelectItem(Me._SessionNotificationLevelComboBox, device.MessageNotificationLevel)
            Case NameOf(isr.VI.DeviceBase.ServiceRequestEnableBitmask)
                Me._ServiceRequestEnableNumeric.Value = device.ServiceRequestEnableBitmask
                Me._ServiceRequestEnableNumeric.ToolTipText = $"SRE:0b{Convert.ToString(device.ServiceRequestEnableBitmask, 2),8}".Replace(" ", "0")
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        AddHandler Me.Device.ThermostreamSubsystem.PropertyChanged, AddressOf Me.ThermoStreamSubsystemPropertyChanged
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        MyBase.DeviceOpened(sender, e)
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overrides Sub OnTitleChanged(ByVal value As String)
        Me._TitleLabel.Text = value
        Me._TitleLabel.Visible = Not String.IsNullOrWhiteSpace(value)
        MyBase.OnTitleChanged(value)
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.DeviceClosing(sender, e)
        If e?.Cancel Then Return
        If Me.IsDeviceOpen Then
            RemoveHandler Me.Device.ThermostreamSubsystem.PropertyChanged, AddressOf Me.ThermoStreamSubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " THERMO STREAM "

    ''' <summary> Degrees caption. </summary>
    ''' <param name="value"> The <see cref="TraceMessage">message</see> to display and
    ''' log. </param>
    ''' <returns> A String. </returns>
    Private Shared Function DegreesCaption(ByVal value As String) As String
        Return String.Format("{0} {1}C", value, Convert.ToChar(&H2070))
    End Function

    ''' <summary> Executes the measurement available action. </summary>
    Private Sub OnMeasurementAvailable(ByVal value As Double?)
        If value.HasValue Then
            Me.OnMeasurementAvailable(CStr(value.Value))
        Else
            Me.OnMeasurementAvailable("")
        End If
    End Sub

    ''' <summary> Executes the measurement available action. </summary>
    Private Sub OnMeasurementAvailable(ByVal value As String)
        Const clear As String = "    "
        If String.IsNullOrWhiteSpace(value) Then
            Me._ReadingToolStripStatusLabel.Text = ThermostreamPanel.DegreesCaption("-.-")
            Me._FailureToolStripStatusLabel.Text = clear
        Else
            Me._ReadingToolStripStatusLabel.SafeTextSetter(ThermostreamPanel.DegreesCaption(value))
            Me._FailureToolStripStatusLabel.Text = "  "
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Instruments parsed reading elements.")
        End If
    End Sub

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As ThermostreamSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.IsHeadUp)
                Me._HeadDownCheckBox.SafeCheckedSetter(Not subsystem.IsHeadUp)
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.IsReady)
                Me._ReadyCheckBox.SafeCheckedSetter(subsystem.IsReady)
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.CycleCount)
                Me._CycleCountNumeric.SafeValueSetter(CType(subsystem.CycleCount, Decimal?))
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.DeviceSensorType)
                Me._DeviceSensorTypeSelector.ComboBox.SafeSelectValue(subsystem.DeviceSensorType.GetValueOrDefault(0))
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.DeviceThermalConstant)
                Me._DeviceThermalConstantSelector.SafeValueSetter(CType(subsystem.DeviceThermalConstant, Decimal?))
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.MaximumTestTime)
                Me._MaxTestTimeNumeric.SafeValueSetter(CType(subsystem.MaximumTestTime, Decimal?))
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.RampRate)
                Me._RampRateNumeric.SafeValueSetter(CType(subsystem.RampRate, Decimal?))
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.SetpointNumber)
                Me._SetpointNumberNumeric.SafeValueSetter(CType(subsystem.SetpointNumber, Decimal?))
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.Setpoint)
                Me._SetpointNumeric.SafeValueSetter(CType(subsystem.Setpoint, Decimal?))
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.SetpointWindow)
                Me._SetpointWindowNumeric.SafeValueSetter(CType(subsystem.SetpointWindow, Decimal?))
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.SoakTime)
                Me._SoakTimeNumeric.SafeValueSetter(CType(subsystem.SoakTime, Decimal?))
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.IsAtTemperature)
                Me._AtTempCheckBox.SafeCheckedSetter(subsystem.IsAtTemperature)
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.IsCycleCompleted)
                Me._CycleCompletedCheckBox1.CheckBoxControl.SafeCheckedSetter(subsystem.IsCycleCompleted)
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.IsCyclesCompleted)
                Me._CyclesCompletedCheckBox1.CheckBoxControl.SafeCheckedSetter(subsystem.IsCyclesCompleted)
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.IsCyclingStopped)
                Me._CyclingStoppedCheckBox.CheckBoxControl.SafeCheckedSetter(subsystem.IsCyclingStopped)
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.IsTestTimeElapsed)
                Me._TestTimeElapsedCheckBox.SafeCheckedSetter(subsystem.IsTestTimeElapsed)
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.IsNotAtTemperature)
                Me._NotAtTempCheckBox.SafeCheckedSetter(subsystem.IsNotAtTemperature)
            Case NameOf(VI.Thermostream.ThermostreamSubsystem.Temperature)
                Me.OnMeasurementAvailable(subsystem.Temperature)
        End Select
    End Sub

    ''' <summary> Sense Current subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ThermoStreamSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(ThermostreamSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.ThermoStreamSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, ThermostreamSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Private Sub OnLastError(ByVal lastError As DeviceError)
        If lastError?.IsError Then
            Me._LastErrorTextBox.ForeColor = Drawing.Color.OrangeRed
        Else
            Me._LastErrorTextBox.ForeColor = Drawing.Color.Aquamarine
        End If
        Me._LastErrorTextBox.Text = lastError.CompoundErrorMessage
    End Sub

    ''' <summary> Displays the status register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Public Overrides Sub DisplayStatusRegisterStatus(ByVal value As Integer)
        Me._StatusByteLabel.Text = $"{Convert.ToString(value, 2),8}".Replace(" ", "0")
        MyBase.DisplayStatusRegisterStatus(value)
        Application.DoEvents()
    End Sub

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub HandlePropertyChange(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.HandlePropertyChange(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(StatusSubsystemBase.DeviceErrorsReport)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.LastDeviceError)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.ErrorAvailable)
            Case NameOf(StatusSubsystemBase.ServiceRequestStatus)
                'Me._StatusRegisterLabel.Text = $"0x{subsystem.ServiceRequestStatus:X2}"
            Case NameOf(StatusSubsystemBase.StandardEventStatus)
                'Me._StandardRegisterLabel.Text = $"0x{subsystem.StandardEventStatus:X2}"
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(StatusSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.StatusSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, StatusSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " SYSTEM "

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SystemSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.SystemSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SystemSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#End Region

#Region " CONTROL EVENT HANDLERS: RESET "

    ''' <summary> Clears interface. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearInterfaceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearInterfaceMenuItem.Click
        Dim activity As String = "clearing interface"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ClearInterface()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears device (SDC). </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearDeviceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearDeviceMenuItem.Click
        Dim activity As String = "clearing device active state (SDC)"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ClearActiveState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears (CLS) the execution state menu item click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearExecutionStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearExecutionStateMenuItem.Click
        Dim activity As String = "clearing the execution state"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ClearExecutionState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    ''' <summary> Resets (RST) the known state menu item click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ResetKnownStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResetKnownStateMenuItem.Click
        Dim activity As String = "resetting known state"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ResetKnownState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Initializes to known state menu item click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitKnownStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InitKnownStateMenuItem.Click
        Dim activity As String = "resetting known state"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                'Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                'Me.Device.ResetKnownState()
                activity = "initializing known state"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.InitKnownState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#Region " TRACE LEVEL "

    ''' <summary> Enables the trace level controls. </summary>
    Private Sub EnableTraceLevelControls()

        TalkerControlBase.ListTraceEventLevels(Me._LogTraceLevelComboBox.ComboBox)
        AddHandler Me._LogTraceLevelComboBox.ComboBox.SelectedValueChanged, AddressOf Me._LogTraceLevelComboBox_SelectedValueChanged

        TalkerControlBase.ListTraceEventLevels(Me._DisplayTraceLevelComboBox.ComboBox)
        AddHandler Me._DisplayTraceLevelComboBox.ComboBox.SelectedValueChanged, AddressOf Me._DisplayTraceLevelComboBox_SelectedValueChanged

        TalkerControlBase.SelectItem(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel)
        TalkerControlBase.SelectItem(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel)

    End Sub

    ''' <summary>
    ''' Event handler. Called by _LogTraceLevelComboBox for selected value changed events.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _LogTraceLevelComboBox_SelectedValueChanged(sender As Object, e As EventArgs)
        Dim activity As String = "selecting log trace level on this instrument only"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ApplyTalkerTraceLevel(ListenerType.Logger,
                                            TalkerControlBase.SelectedValue(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Event handler. Called by _DisplayTraceLevelComboBox for selected value changed events.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DisplayTraceLevelComboBox_SelectedValueChanged(sender As Object, e As EventArgs)
        Dim activity As String = "selecting Display trace level on this instrument only"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ApplyTalkerTraceLevel(ListenerType.Display,
                                            TalkerControlBase.SelectedValue(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#End Region

#Region " CONTROL EVENT HANDLERS: SESSION "

    ''' <summary> Handles Reads Status Byte Menu Item click event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadStatusByteMenuItem_Click(ByVal sender As Object, e As System.EventArgs) Handles _ReadStatusByteMenuItem.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "reading status byte"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.StartElapsedStopwatch()
                Me.ReadServiceRequestStatus()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles session message tracing. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionNotificationLevelComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _SessionNotificationLevelComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "selecting session notification level"
        Dim combo As Core.Controls.ToolStripComboBox = TryCast(sender, Core.Controls.ToolStripComboBox)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If combo IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.MessageNotificationLevel = VI.Pith.SessionBase.SelectedValue(combo, NotifySyncLevel.None)
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the session service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionServiceRequestHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SessionServiceRequestHandlerEnabledMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Toggle session service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.Session.ServiceRequestEventEnabled Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.Device.Session.EnableServiceRequest()
                    If Me._ServiceRequestEnableNumeric.Value = 0 Then
                        Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All)
                    Else
                        Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableNumeric.Value, VI.Pith.ServiceRequests))
                    End If
                Else
                    Me.Device.Session.DisableServiceRequest()
                    Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.None)
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the Device service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DeviceServiceRequestHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _DeviceServiceRequestHandlerEnabledMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Toggle device service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.DeviceServiceRequestHandlerAdded Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.AddServiceRequestEventHandler()
                Else
                    Me.RemoveServiceRequestEventHandler()
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: MAIN PANEL "

    ''' <summary> Reads last error button click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadLastErrorButton_Click(sender As System.Object, e As System.EventArgs) Handles _ReadLastErrorButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
#If False Then
                Me.Device.SystemSubsystem.WriteLastErrorQuery()
                Dim srq As VI.Pith.ServiceRequests = Me.readServiceRequest
                If (srq And VI.Pith.ServiceRequests.MessageAvailable) = 0 Then
                    Me.ErrorProvider.Annunciate(sender, "Nothing to read")
                Else
                    Me.Device.SystemSubsystem.ReadLastError()
                End If
#End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initializing known state;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears the error queue button click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearErrorQueueButton_Click(sender As System.Object, e As System.EventArgs) Handles _ClearErrorQueueButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Device.StatusSubsystem.ClearErrorQueue()
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initializing known state;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by InitButton for click events. Initiates a reading for
    ''' retrieval by way of the service request event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitializeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InitializeButton.Click
        Dim activity As String = "initializing"
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()

            If Me.Device.IsDeviceOpen Then

                ' clear execution state before enabling events
                Me.Device.ClearExecutionState()

                ' set the service request
                Me.Device.ThermostreamSubsystem.WriteTemperatureEventEnableBitmask(255)
                Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All And Not VI.Pith.ServiceRequests.MessageAvailable)
                Me.Device.ClearExecutionState()
                Me.ReadServiceRequest()
            End If

        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally

            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by _ReadButton for click events. Query the Device for a
    ''' reading. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                ' update display if changed.
                Me.Device.ThermostreamSubsystem.QueryTemperature()
                Me.ReadServiceRequest()
                Application.DoEvents()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading temperature;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try

    End Sub

    ''' <summary> Reads service request. </summary>
    ''' <returns> The service request. </returns>
    Private Function ReadServiceRequest() As VI.Pith.ServiceRequests
        Dim srq As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.None
        If Me.IsDeviceOpen Then
            ' it takes a few ms for the event to register. 
            Diagnostics.Stopwatch.StartNew.Wait(TimeSpan.FromMilliseconds(Me._SrqRefratoryTimeNumeric.Value))
            srq = Me.Device.StatusSubsystem.ReadServiceRequestStatus
            Application.DoEvents()
        End If
        Return srq
    End Function

    ''' <summary> Reads status byte button click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ReadStatusByteButton_Click(sender As System.Object, e As System.EventArgs) Handles _ReadStatusByteButton.Click
        If Me.IsDeviceOpen Then
            Me.ReadServiceRequest()
        End If
    End Sub

    ''' <summary> Sends a button click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SendButton_Click(sender As System.Object, e As System.EventArgs) Handles _SendButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                If Not String.IsNullOrWhiteSpace(Me._SendComboBox.Text) Then
                    Me.Device.Session.WriteLine(Me._SendComboBox.Text)
                    Me.ReadServiceRequest()
                End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred moving to the next setpoint;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReceiveButton_Click(sender As Object, e As System.EventArgs) Handles _ReceiveButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                Dim srq As VI.Pith.ServiceRequests = Me.Device.StatusSubsystem.ReadServiceRequestStatus()
                If (srq And VI.Pith.ServiceRequests.MessageAvailable) = 0 Then
                    Me.ErrorProvider.Annunciate(sender, "Nothing to read")
                Else
                    Me._ReceiveTextBox.Text = Me.Device.Session.ReadLineTrimEnd
                    Me.ReadServiceRequest()
                End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred moving to the next setpoint;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: CYCLE PANEL "

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _FindLastSetpointButton_Click(sender As System.Object, e As System.EventArgs) Handles _FindLastSetpointButton1.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                Me._FindLastSetpointButton1.Text = String.Format("Find Last Setpoint: {0}", Me.Device.ThermostreamSubsystem.FindLastSetpointNumber)
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred finding last set point;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _QueryTempEvents(sender As System.Object)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                Me.Device.ThermostreamSubsystem.QueryTemperatureEventStatus()
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading temperature events;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _QueryAuxiliaryStatus(sender As System.Object)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                Me.Device.ThermostreamSubsystem.QueryAuxiliaryEventStatus()
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading auxiliary event status;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub _QueryTempEventButton_Click(sender As System.Object, e As System.EventArgs)
        Me._QueryTempEvents(sender)
    End Sub

    Private Sub _QueryAuxiliaryStatusButton_Click(sender As System.Object, e As System.EventArgs) Handles _QueryStatusButton.Click
        Me._QueryAuxiliaryStatus(sender)
        Me._QueryTempEvents(sender)
    End Sub


    ''' <summary> Setpoint number numeric value selected. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SetpointNumberNumeric_ValueSelected(sender As System.Object, e As System.EventArgs) Handles _SetpointNumberNumeric.ValueSelected
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                With Me.Device.ThermostreamSubsystem
                    If Me._SetpointNumberNumeric.SelectedValue.HasValue Then
                        .WriteSetpointNumber(CInt(Me._SetpointNumberNumeric.Value))
                        Application.DoEvents()
                    End If
                    .QuerySetpointNumber()
                    Application.DoEvents()
                    .ReadCurrentSetpointValues()
                    Application.DoEvents()
                End With
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred selecting setpoint;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try

    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub QueryHeadStatus(sender As System.Object)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                With Me.Device.ThermostreamSubsystem
                    .QueryHeadDown()
                End With
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading head status;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Reads set point button click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadSetpointButton_Click(sender As System.Object, e As System.EventArgs) Handles _ReadSetpointButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                With Me.Device.ThermostreamSubsystem
                    .ReadCurrentSetpointValues()
                End With
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading the setpoint values;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Next button click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _NextButton_Click(sender As System.Object, e As System.EventArgs) Handles _NextSetpointButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                With Me.Device.ThermostreamSubsystem
                    .NextSetpoint()
                End With
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred moving to the next setpoint;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MaxTestTimeNumeric_ValueSelected(sender As System.Object, e As System.EventArgs) Handles _MaxTestTimeNumeric.ValueSelected
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                With Me.Device.ThermostreamSubsystem
                    If Me._MaxTestTimeNumeric.SelectedValue.HasValue Then
                        .WriteMaximumTestTime(Me._MaxTestTimeNumeric.SelectedValue.Value)
                    End If
                    .QueryMaximumTestTime()
                End With
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred setting and reading the maximum test time;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _CycleCountNumeric_ValueSelected(sender As System.Object, e As System.EventArgs) Handles _CycleCountNumeric.ValueSelected
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen AndAlso Me._CycleCountNumeric.SelectedValue.HasValue Then
                With Me.Device.ThermostreamSubsystem
                    .ApplyCycleCount(CInt(Me._CycleCountNumeric.SelectedValue.Value))
                    .QueryCycleCount()
                End With
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred setting and reading the cycle count;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _StartButton_Click(sender As System.Object, e As System.EventArgs) Handles _StartCycleButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                With Me.Device.ThermostreamSubsystem
                    .StartCycling()
                End With
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred starting cycling;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _StopCycleButton_Click(sender As System.Object, e As System.EventArgs) Handles _StopCycleButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                With Me.Device.ThermostreamSubsystem
                    .StopCycling()
                End With
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred Stopping cycling;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ResetSystemMode(ByVal resetOperator As Boolean, sender As System.Object)
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If sender IsNot Nothing AndAlso Me.IsDeviceOpen Then
                With Me.Device.ThermostreamSubsystem
                    Dim refrectoryPeriod As TimeSpan = TimeSpan.Zero
                    If resetOperator Then
                        refrectoryPeriod = .ResetOperatorScreenRefractoryTimeSpan
                        .ResetOperatorScreen()
                    Else
                        refrectoryPeriod = .ResetCycleScreenRefractoryTimeSpan
                        .ResetCycleScreen()
                    End If
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Awaiting reset;. ")
                    Diagnostics.Stopwatch.StartNew.Wait(refrectoryPeriod)
                    .QuerySystemScreen()
                    If .OperatorScreen.HasValue Then
                        Me._ResetOperatorModeButton.Text = String.Format("Operator Mode: {0}", IIf(.OperatorScreen.Value, "ON", "OFF"))
                    Else
                        Me._ResetOperatorModeButton.Text = String.Format("Operator Mode: {0}", "N/A")
                    End If
                    If .CycleScreen.HasValue Then
                        Me._ResetCycleModeButton.Text = String.Format("Cycle Mode: {0}", IIf(.CycleScreen.Value, "ON", "OFF"))
                    Else
                        Me._ResetCycleModeButton.Text = String.Format("Cycle Mode: {0}", "N/A")
                    End If
                End With
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred Stopping cycling;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub _ResetOperatorModeButton_Click(sender As System.Object, e As System.EventArgs) Handles _ResetOperatorModeButton.Click
        Me.ResetSystemMode(True, sender)
    End Sub

    Private Sub _ResetCycleModeButton_Click(sender As System.Object, e As System.EventArgs) Handles _ResetCycleModeButton.Click
        Me.ResetSystemMode(False, sender)
    End Sub

#End Region

#Region " SENSOR "

    Dim _DeviceSensorType As isr.Core.Pith.EnumExtender(Of DeviceSensorType)
    ''' <summary> Displays a sensor types. </summary>
    Private Sub DisplaySensorTypes()
        Me._DeviceSensorType = New isr.Core.Pith.EnumExtender(Of DeviceSensorType)
        Me._DeviceSensorType.ListValues(Me._DeviceSensorTypeSelector.ComboBox)
    End Sub

    Private Sub _DeviceThermalConstantSelector_ValueSelected(sender As System.Object, e As System.EventArgs) Handles _DeviceThermalConstantSelector.ValueSelected
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DeviceSensorTypeSelector_ValueSelected(sender As System.Object, e As System.EventArgs) Handles _DeviceSensorTypeSelector.ValueSelected
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me._DeviceSensorType IsNot Nothing Then

            End If
            If Me._DeviceSensorType IsNot Nothing AndAlso sender IsNot Nothing AndAlso
                    Me.IsDeviceOpen Then
                Dim v As DeviceSensorType = Me._DeviceSensorType.SelectedValue(Me._DeviceSensorTypeSelector.ComboBox)
                With Me.Device.ThermostreamSubsystem
                    .ApplyDeviceSensorType(v)
                    .QueryDeviceSensorType()
                    If .DeviceSensorType.GetValueOrDefault(0) = DeviceSensorType.None Then
                        .ApplyDeviceControl(False)
                    Else
                        .ApplyDeviceControl(True)
                    End If
                    .QueryDeviceControl()
                    .QueryDeviceThermalConstant()
                End With
                Me.ReadServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred setting and reading the device sensor type;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

#End Region

#Region " READ AND WRITE "

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub OnPropertyChanged(ByVal sender As Instrument.SimpleReadWriteControl, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(Instrument.SimpleReadWriteControl.StatusMessage)
                    Me.StatusLabel.Text = sender.StatusMessage
                Case NameOf(Instrument.SimpleReadWriteControl.ServiceRequestValue)
                    Me.StatusRegisterLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
            End Select
        End If
    End Sub

    ''' <summary> Event handler. Called by <see crefname="_SimpleReadWriteControl"/> for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleReadWriteControl_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Handles _SimpleReadWriteControl.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._SimpleReadWriteControl_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, Instrument.SimpleReadWriteControl), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling {0} property change;. {1}",
                               e?.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

    ''' <summary> Assigns talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AssignTalker(talker As ITraceMessageTalker)
        Me._SimpleReadWriteControl.AssignTalker(talker)
        MyBase.AssignTalker(talker)
        ' My.MyLibrary.Identify(talker)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        Me._SimpleReadWriteControl.ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        ' MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

#End Region

End Class
