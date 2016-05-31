Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ControlExtensions
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.CheckBoxExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.StopwatchExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
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

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private _InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(New Device)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="device"> The device. </param>
    Protected Sub New(ByVal device As Device)
        MyBase.New(device)
        Me._InitializingComponents = True
        Me.InitializeComponent()
        Me._InitializingComponents = False
        Me._SetpointWindowNumeric.ReadOnly = True
        Me._SetpointNumeric.ReadOnly = True
        Me._SoakTimeNumeric.ReadOnly = True
        Me._AssignDevice(device)
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the isr.VI.Instrument.ResourcePanelBase and
    ''' optionally releases the managed resources.
    ''' </summary>
    ''' <remarks> David, 12/22/2015. </remarks>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Try
                    If Me.Device IsNot Nothing Then Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", "Exception details: {0}", ex)
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
    ''' <remarks> David, 1/4/2016. </remarks>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Try
            Me.TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
            Me.DisplaySensorTypes()
        Finally
            MyBase.OnLoad(e)
        End Try
    End Sub

#End Region

#Region " DEVICE "

    ''' <summary> Assigns a device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignDevice(ByVal value As Device)
        Me._Device = value
        Me.AddListeners()
        Me.OnDeviceOpenChanged(value)
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device)
        Me.IsDeviceOwner = False
        MyBase.AssignDevice(value)
        Me._AssignDevice(value)
    End Sub

    ''' <summary> Releases the device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    Protected Overrides Sub ReleaseDevice()
        MyBase.ReleaseDevice()
    End Sub

    ''' <summary> Gets a reference to the InTest Thermo Stream Device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overloads ReadOnly Property Device() As Device

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
    ''' <remarks> David, 3/3/2016. </remarks>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As DeviceBase)
        Dim isOpen As Boolean = CType(device?.IsDeviceOpen, Boolean?).GetValueOrDefault(False)
        If isOpen Then
            Me._SimpleReadWriteControl.Connect(device?.Session)
        Else
            Me._SimpleReadWriteControl.Disconnect()
        End If
        For Each t As Windows.Forms.TabPage In Me._Tabs.TabPages
            If t IsNot Me._MessagesTabPage Then
                For Each c As Windows.Forms.Control In t.Controls : Me.RecursivelyEnable(c, isOpen) : Next
            End If
        Next
    End Sub

    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnDevicePropertyChanged(ByVal device As DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnDevicePropertyChanged(device, propertyName)
        Select Case propertyName
            Case NameOf(device.IsServiceRequestEventEnabled)
                ' Me._HandleServiceRequestsCheckBox.Checked = device.IsServiceRequestEventEnabled
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        AddHandler Me.Device.ThermoStreamSubsystem.PropertyChanged, AddressOf Me.ThermoStreamSubsystemPropertyChanged
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        MyBase.DeviceOpened(sender, e)
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <remarks> David, 1/14/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overrides Sub OnTitleChanged(ByVal value As String)
        Me._TitleLabel.Text = value
        Me._TitleLabel.Visible = Not String.IsNullOrWhiteSpace(value)
        MyBase.OnTitleChanged(Title)
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.DeviceClosing(sender, e)
        If e?.Cancel Then Return
        If Me.IsDeviceOpen Then
            RemoveHandler Me.Device.ThermoStreamSubsystem.PropertyChanged, AddressOf Me.ThermoStreamSubsystemPropertyChanged
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
    Private Sub onMeasurementAvailable(ByVal value As Double?)
        If value.HasValue Then
            Me.onMeasurementAvailable(CStr(value.Value))
        Else
            Me.onMeasurementAvailable("")
        End If
    End Sub

    ''' <summary> Executes the measurement available action. </summary>
    Private Sub onMeasurementAvailable(ByVal value As String)
        Const clear As String = "    "
        If String.IsNullOrWhiteSpace(value) Then
            Me._ReadingToolStripStatusLabel.Text = ThermostreamPanel.DegreesCaption("-.-")
            Me._ComplianceToolStripStatusLabel.Text = clear
            Me._TbdToolStripStatusLabel.Text = clear
        Else
            Me._ReadingToolStripStatusLabel.SafeTextSetter(ThermostreamPanel.DegreesCaption(value))
            Me._ComplianceToolStripStatusLabel.Text = "  "
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Instruments parsed reading elements.")
        End If
    End Sub

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As ThermostreamSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        Select Case propertyName
            Case NameOf(subsystem.IsHeadUp)
                Me._HeadDownCheckBox.SafeCheckedSetter(Not subsystem.IsHeadUp)
            Case NameOf(subsystem.IsReady)
                Me._ReadyCheckBox.SafeCheckedSetter(subsystem.IsReady)
            Case NameOf(subsystem.CycleCount)
                Me._CycleCountNumeric.SafeValueSetter(CType(subsystem.CycleCount, Decimal?))
            Case NameOf(subsystem.DeviceSensorType)
                Me._DeviceSensorTypeSelector.ComboBox.SafeSelectValue(subsystem.DeviceSensorType.GetValueOrDefault(0))
            Case NameOf(subsystem.DeviceThermalConstant)
                Me._DeviceThermalConstantSelector.SafeValueSetter(CType(subsystem.DeviceThermalConstant, Decimal?))
            Case NameOf(subsystem.MaximumTestTime)
                Me._MaxTestTimeNumeric.SafeValueSetter(CType(subsystem.MaximumTestTime, Decimal?))
            Case NameOf(subsystem.RampRate)
                Me._RampRateNumeric.SafeValueSetter(CType(subsystem.RampRate, Decimal?))
            Case NameOf(subsystem.SetpointNumber)
                Me._SetpointNumberNumeric.SafeValueSetter(CType(subsystem.SetpointNumber, Decimal?))
            Case NameOf(subsystem.Setpoint)
                Me._SetpointNumeric.SafeValueSetter(CType(subsystem.Setpoint, Decimal?))
            Case NameOf(subsystem.SetpointWindow)
                Me._SetpointWindowNumeric.SafeValueSetter(CType(subsystem.SetpointWindow, Decimal?))
            Case NameOf(subsystem.SoakTime)
                Me._SoakTimeNumeric.SafeValueSetter(CType(subsystem.SoakTime, Decimal?))
            Case NameOf(subsystem.IsAtTemperature)
                Me._AtTempCheckBox.SafeCheckedSetter(subsystem.IsAtTemperature)
            Case NameOf(subsystem.IsCycleCompleted)
                Me._CycleCompletedCheckBox1.CheckBoxControl.SafeCheckedSetter(subsystem.IsCycleCompleted)
            Case NameOf(subsystem.IsCyclesCompleted)
                Me._CyclesCompletedCheckBox1.CheckBoxControl.SafeCheckedSetter(subsystem.IsCyclesCompleted)
            Case NameOf(subsystem.IsCyclingStopped)
                Me._CyclingStoppedCheckBox.CheckBoxControl.SafeCheckedSetter(subsystem.IsCyclingStopped)
            Case NameOf(subsystem.IsTestTimeElapsed)
                Me._TestTimeElapsedCheckBox.SafeCheckedSetter(subsystem.IsTestTimeElapsed)
            Case NameOf(subsystem.IsNotAtTemperature)
                Me._NotAtTempCheckBox.SafeCheckedSetter(subsystem.IsNotAtTemperature)
            Case NameOf(subsystem.Temperature)
                Me.onMeasurementAvailable(subsystem.Temperature)
        End Select
    End Sub

    ''' <summary> Sense Current subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ThermoStreamSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, ThermostreamSubsystem), e.PropertyName)
                Application.DoEvents()
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Current Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Private Sub onLastError(ByVal lastError As DeviceError)
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
        Me._StatusByteLabel.Text = String.Format("{0,8}", Convert.ToString(value, 2)).Replace(" ", "0")
        MyBase.DisplayStatusRegisterStatus(value)
        Application.DoEvents()
    End Sub

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnPropertyChanged(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(subsystem.DeviceErrors)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.LastDeviceError)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.ErrorAvailable)
                ' if no errors, this clears the error queue.
                subsystem.QueryLastError()
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, StatusSubsystem), e?.PropertyName)
            System.Windows.Forms.Application.DoEvents()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

    ''' <summary> Reads a service request status. </summary>
    ''' <remarks> David, 12/26/2015. </remarks>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ReadServiceRequestStatus()
        Try
            Me.Device.StatusSubsystem.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading service request;. Details: {0}", ex)
        End Try
    End Sub

#End Region

#Region " SYSTEM "

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, SystemSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#End Region

#Region " CONTROL EVENT HANDLERS: RESET "

    ''' <summary> Event handler. Called by _SessionTraceEnableCheckBox for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionTraceEnableCheckBox_CheckedChanged(ByVal sender As Object, e As System.EventArgs) Handles _SessionTraceEnableCheckBox.CheckedChanged
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Not Me.DesignMode AndAlso sender IsNot Nothing Then
                Dim checkBox As Windows.Forms.CheckBox = CType(sender, Windows.Forms.CheckBox)
                Me.Device.SessionMessagesTraceEnabled = checkBox.Checked
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by interfaceClearButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InterfaceClearButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InterfaceClearButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "{0} clearing interface;. {1}", Me.ResourceTitle, Me.ResourceName)
            Me.Device.SystemSubsystem.ClearInterface()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred clearing interface;. Details: {0}", ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _SelectiveDeviceClearButton for click events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SelectiveDeviceClearButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SelectiveDeviceClearButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "{0} clearing selective device;. {1}", Me.ResourceTitle, Me.ResourceName)
            Me.Device.SystemSubsystem.ClearDevice()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred sending SDC;. Details: {0}", ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Issue RST. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ResetButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResetButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} resetting known state;. {1}", Me.ResourceTitle, Me.ResourceName)
                Me.Device.ResetKnownState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred resetting known state;. Details: {0}", ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _InitializeKnownStateButton for click events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitializeKnownStateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InitializeKnownStateButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} resetting known state;. {1}", Me.ResourceTitle, Me.ResourceName)
                Me.Device.ResetKnownState()
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} initializing known state;. {1}", Me.ResourceTitle, Me.ResourceName)
                Me.Device.InitKnownState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initializing known state;. Details: {0}", ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
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
                Dim srq As isr.VI.ServiceRequests = Me.readServiceRequest
                If (srq And ServiceRequests.MessageAvailable) = 0 Then
                    Me.ErrorProvider.Annunciate(sender, "Nothing to read")
                Else
                    Me.Device.SystemSubsystem.ReadLastError()
                End If
#End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initializing known state;. Details: {0}", ex)
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
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initializing known state;. Details: {0}", ex)
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
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.ErrorProvider.Clear()

            If Me.Device.IsDeviceOpen Then

                ' clear execution state before enabling events
                Me.Device.ClearExecutionState()

                ' set the service request
                Me.Device.ThermoStreamSubsystem.WriteTemperatureEventEnableBitmask(255)
                Me.Device.StatusSubsystem.EnableServiceRequest(VI.ServiceRequests.All And Not VI.ServiceRequests.MessageAvailable)
                Me.Device.ClearExecutionState()
                Me.readServiceRequest()
            End If

        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. Details: {0}", ex)
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
                Me.Device.ThermoStreamSubsystem.QueryTemperature()
                Me.readServiceRequest()
                Application.DoEvents()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading temperature;. Details: {0}", ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by _HandleServiceRequestsCheckBox for check state changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _HandleServiceRequestsCheckBox_CheckStateChanged(sender As Object, e As System.EventArgs)
        If Me._InitializingComponents Then Return
        Dim checkBox As CheckBox = TryCast(sender, CheckBox)
        If checkBox IsNot Nothing AndAlso Not checkBox.Checked = Me.Device.Session.IsServiceRequestEventEnabled Then
            If checkBox IsNot Nothing AndAlso checkBox.Checked Then
                Me.EnableServiceRequestEventHandler()
                Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
            Else
                Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)
                Me.DisableServiceRequestEventHandler()
            End If
            Me.Device.StatusSubsystem.ReadRegisters()
            Application.DoEvents()
        End If
    End Sub

    ''' <summary> Reads service request. </summary>
    ''' <returns> The service request. </returns>
    Private Function readServiceRequest() As isr.VI.ServiceRequests
        Dim srq As isr.VI.ServiceRequests = ServiceRequests.None
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
            Me.readServiceRequest()
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
                    Me.readServiceRequest()
                End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred moving to the next setpoint;. Details: {0}", ex)
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
                Dim srq As isr.VI.ServiceRequests = Me.Device.StatusSubsystem.ReadServiceRequestStatus()
                If (srq And ServiceRequests.MessageAvailable) = 0 Then
                    Me.ErrorProvider.Annunciate(sender, "Nothing to read")
                Else
                    Me._ReceiveTextBox.Text = Me.Device.Session.ReadLineTrimEnd
                    Me.readServiceRequest()
                End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred moving to the next setpoint;. Details: {0}", ex)
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
                Me._FindLastSetpointButton1.Text = String.Format("Find Last Setpoint: {0}", Me.Device.ThermoStreamSubsystem.FindLastSetpointNumber)
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred finding last set point;. Details: {0}", ex)
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
                Me.Device.ThermoStreamSubsystem.QueryTemperatureEventStatus()
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading temperature events;. Details: {0}", ex)
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
                Me.Device.ThermoStreamSubsystem.QueryAuxiliaryEventStatus()
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading auxiliary event status;. Details: {0}", ex)
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
                With Me.Device.ThermoStreamSubsystem
                    If Me._SetpointNumberNumeric.SelectedValue.HasValue Then
                        .WriteSetpointNumber(CInt(Me._SetpointNumberNumeric.Value))
                        Application.DoEvents()
                    End If
                    .QuerySetpointNumber()
                    Application.DoEvents()
                    .ReadCurrentSetpointValues()
                    Application.DoEvents()
                End With
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred selecting setpoint;. Details: {0}", ex)
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
                With Me.Device.ThermoStreamSubsystem
                    .QueryHeadDown()
                End With
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading head status;. Details: {0}", ex)
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
                With Me.Device.ThermoStreamSubsystem
                    .ReadCurrentSetpointValues()
                End With
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading the setpoint values;. Details: {0}", ex)
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
                With Me.Device.ThermoStreamSubsystem
                    .NextSetpoint()
                End With
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred moving to the next setpoint;. Details: {0}", ex)
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
                With Me.Device.ThermoStreamSubsystem
                    If Me._MaxTestTimeNumeric.SelectedValue.HasValue Then
                        .WriteMaximumTestTime(Me._MaxTestTimeNumeric.SelectedValue.Value)
                    End If
                    .QueryMaximumTestTime()
                End With
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred setting and reading the maximum test time;. Details: {0}", ex)
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
                With Me.Device.ThermoStreamSubsystem
                    .ApplyCycleCount(CInt(Me._CycleCountNumeric.SelectedValue.Value))
                    .QueryCycleCount()
                End With
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred setting and reading the cycle count;. Details: {0}", ex)
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
                With Me.Device.ThermoStreamSubsystem
                    .StartCycling()
                End With
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred starting cycling;. Details: {0}", ex)
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
                With Me.Device.ThermoStreamSubsystem
                    .StopCycling()
                End With
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred Stopping cycling;. Details: {0}", ex)
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
                With Me.Device.ThermoStreamSubsystem
                    Dim refrectoryPeriod As TimeSpan = TimeSpan.Zero
                    If resetOperator Then
                        refrectoryPeriod = .ResetOperatorScreenRefractoryTimeSpan
                        .ResetOperatorScreen()
                    Else
                        refrectoryPeriod = .ResetCycleScreenRefractoryTimeSpan
                        .ResetCycleScreen()
                    End If
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Awaiting reset;. ")
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
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred Stopping cycling;. Details: {0}", ex)
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
                With Me.Device.ThermoStreamSubsystem
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
                Me.readServiceRequest()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred setting and reading the device sensor type;. Details: {0}", ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

#End Region

#Region " READ AND WRITE "

    ''' <summary> Executes the simple read write control property changed action. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSimpleReadWriteControlPropertyChanged(sender As Instrument.SimpleReadWriteControl, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(sender.ReceivedMessage)
                Case NameOf(sender.SentMessage)
                Case NameOf(sender.StatusMessage)
                    Me.StatusLabel.Text = sender.StatusMessage
                Case NameOf(sender.ServiceRequestValue)
                    Me.StatusRegisterLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
                Case NameOf(sender.ElapsedTime)
            End Select
        End If
    End Sub

    ''' <summary> Simple read write control property changed. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleReadWriteControl_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _SimpleReadWriteControl.PropertyChanged
        Try
            Me.OnSimpleReadWriteControlPropertyChanged(TryCast(sender, Instrument.SimpleReadWriteControl), e?.PropertyName)
        Catch ex As Exception
            Me.StatusLabel.Text = "Exception occurred handling change"
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Simple Read and Write property changed Event;. Failed property {0}. Details: {1}",
                               e?.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds listeners such as current level trace message box and log. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    Protected Overrides Sub AddListeners()
        MyBase.AddListeners()
        Me._SimpleReadWriteControl.AddListeners(Me.Talker.Listeners)
    End Sub

    ''' <summary> Adds listeners such as top level trace message box and log. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of ITraceMessageListener))
        MyBase.AddListeners(listeners)
        Me._SimpleReadWriteControl.AddListeners(listeners)
    End Sub

    ''' <summary> Adds the log listener. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="log"> The log. </param>
    Public Overrides Sub AddListeners(ByVal log As MyLog)
        If log Is Nothing Then Throw New ArgumentNullException(NameOf(log))
        MyBase.AddListeners(log)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region

End Class
