Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.Core.Pith.TimeSpanExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Provides a user interface for a Keithley 37XX Device. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2008" by="David" revision="2.0.2936.x"> Create based on the 24xx
''' system classes. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
<System.ComponentModel.DisplayName("K3700 Control"),
      System.ComponentModel.Description("Keithley 3700 Device Control"),
      System.Drawing.ToolboxBitmap(GetType(K3700Control))>
Public Class K3700Control
    Inherits VI.Instrument.ResourceControlBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(Device.Create, True)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Public Sub New(ByVal device As Device, ByVal isDeviceOwner As Boolean)
        MyBase.New()
        Me._New(device, isDeviceOwner)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Private Sub _New(ByVal device As Device, ByVal isDeviceOwner As Boolean)

        Me.InitializingComponents = True
        ' This call is required by the designer.
        InitializeComponent()
        Me.InitializingComponents = False

        Me._ToolStripPanel.Renderer = New CustomProfessionalRenderer
        MyBase.AssignConnector(Me._ResourceSelectorConnector, True)

        Me._AssignDevice(device, isDeviceOwner)
        ' note that the caption is not set if this is run inside the On Load function.
        With Me._TraceMessagesBox
            ' set defaults for the messages box.
            .ResetCount = 500
            .PresetCount = 250
            .ContainerPanel = Me._MessagesTabPage
        End With
        With Me._ServiceRequestEnableBitmaskNumeric.NumericUpDownControl
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
                Me.InitializingComponents = True
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
            Me._TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
            Me._ReadingComboBox.Visible = False
            Me._InitiateButton.Visible = False
        Finally
            MyBase.OnLoad(e)
        End Try
    End Sub

#End Region

#Region " DEVICE "

    Private _Device As Device
    ''' <summary> Gets or sets the device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property Device As Device
        Get
            Return Me._Device
        End Get
    End Property

    ''' <summary> Assign device. </summary>
    ''' <param name="value"> The value. </param>
    Private Sub _AssignDevice(ByVal value As Device, ByVal isDeviceOwner As Boolean)
        If Me._Device IsNot Nothing OrElse MyBase.DeviceBase IsNot Nothing Then
            ' the device base already clears all or only the private listeners. 
            ' Me._Device.RemovePrivateListeners()
            Me._ReleaseDevice()
        End If
        Me._Device = value
        If value IsNot Nothing Then
            value.CaptureSyncContext(WindowsFormsSynchronizationContext.Current)
            value.AddPrivateListener(Me._TraceMessagesBox)
        End If
        ' the device base class addresses the device open state.
        MyBase.AssignDevice(value, isDeviceOwner)
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device, ByVal isDeviceOwner As Boolean)
        Me._AssignDevice(value, isDeviceOwner)
    End Sub

    Private Sub _ReleaseDevice()
        MyBase.ReleaseDevice()
        Me._Device = Nothing
    End Sub

    ''' <summary> Releases the device. </summary>
    ''' <remarks> Called from the base device to release the reference to the device. </remarks>
    Public Overrides Sub ReleaseDevice()
        Me._ReleaseDevice()
    End Sub

    ''' <summary> Releases the device and reassigns the default device. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Overrides Sub RestoreDevice()
        Me.AssignDevice(Device.Create, True)
    End Sub

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As VI.DeviceBase)
        MyBase.OnDeviceOpenChanged(device)
        If Me.IsDeviceOpen Then
            Me.DisplayFunctionModes(VI.Tsp.MultimeterFunctionMode.VoltageDC)
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
                Dim value As VI.Pith.ServiceRequests = device.ServiceRequestEnableBitmask
                Me._ServiceRequestEnableBitmaskNumeric.Value = value
                Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = $"SRE:0b{Convert.ToString(value, 2),8}".Replace(" ", "0")
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As VI.DeviceBase, ByVal e As System.EventArgs)
        AddHandler Me.Device.MultimeterSubsystem.PropertyChanged, AddressOf Me.MultimeterSubsystemPropertyChanged
        AddHandler Me.Device.ChannelSubsystem.PropertyChanged, AddressOf Me.ChannelSubsystemPropertyChanged
        AddHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
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
    Protected Overrides Sub DeviceClosing(ByVal sender As VI.DeviceBase, ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.DeviceClosing(sender, e)
        If e?.Cancel Then Return
        If Me.IsDeviceOpen Then
            RemoveHandler Me.Device.MultimeterSubsystem.PropertyChanged, AddressOf Me.MultimeterSubsystemPropertyChanged
            RemoveHandler Me.Device.ChannelSubsystem.PropertyChanged, AddressOf Me.ChannelSubsystemPropertyChanged
            RemoveHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " MUTLIMETER "

    ''' <summary> Handles the Multimeter subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As MultimeterSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K3700.MultimeterSubsystem.AutoDelayMode)
                If subsystem.AutoDelayMode.HasValue Then Me.AutoDelayMode = subsystem.AutoDelayMode.Value
            Case NameOf(K3700.MultimeterSubsystem.AutoRangeEnabled)
                If subsystem.AutoRangeEnabled.HasValue Then Me._AutoRangeCheckBox.Checked = subsystem.AutoRangeEnabled.Value
            Case NameOf(K3700.MultimeterSubsystem.AutoZeroEnabled)
                If subsystem.AutoZeroEnabled.HasValue Then Me._AutoZeroCheckBox.Checked = subsystem.AutoZeroEnabled.Value
            Case NameOf(K3700.MultimeterSubsystem.FilterCount)
                If subsystem.FilterCount.HasValue Then Me._FilterCountNumeric.Value = subsystem.FilterCount.Value
            Case NameOf(K3700.MultimeterSubsystem.FilterCountRange)
                With Me._FilterCountNumeric
                    .Maximum = CDec(subsystem.FilterCountRange.Max)
                    .Minimum = CDec(subsystem.FilterCountRange.Min)
                    .DecimalPlaces = 0
                End With
            Case NameOf(K3700.MultimeterSubsystem.FilterEnabled)
                If subsystem.FilterEnabled.HasValue Then Me._FilterEnabledCheckBox.Checked = subsystem.FilterEnabled.Value
                If Me._FilterEnabledCheckBox.Checked <> Me._FilterGroupBox.Enabled Then Me._FilterGroupBox.Enabled = Me._FilterEnabledCheckBox.Checked
            Case NameOf(K3700.MultimeterSubsystem.FilterWindow)
                If subsystem.FilterWindow.HasValue Then Me._FilterWindowNumeric.Value = CDec(100 * subsystem.FilterWindow.Value)
            Case NameOf(K3700.MultimeterSubsystem.FilterWindowRange)
                With Me._FilterWindowNumeric
                    .Maximum = 100 * CDec(subsystem.FilterWindowRange.Max)
                    .Minimum = 100 * CDec(subsystem.FilterWindowRange.Min)
                    .DecimalPlaces = 0
                End With
            Case NameOf(K3700.MultimeterSubsystem.MovingAverageFilterEnabled)
                If subsystem.MovingAverageFilterEnabled.HasValue Then Me._MovingAverageRadioButton.Checked = subsystem.MovingAverageFilterEnabled.Value
                If subsystem.MovingAverageFilterEnabled.HasValue Then Me._RepeatingAverageRadioButton.Checked = Not subsystem.MovingAverageFilterEnabled.Value
            Case NameOf(K3700.MultimeterSubsystem.FunctionMode)
                Dim value As VI.Tsp.MultimeterFunctionMode = subsystem.FunctionMode.GetValueOrDefault(VI.Tsp.MultimeterFunctionMode.VoltageDC)
                If Me._SenseFunctionComboBox.DataSource Is Nothing Then
                    Me.DisplayFunctionModes(value)
                Else
                    Me._SenseFunctionComboBox.SelectedItem = subsystem.FunctionMode.GetValueOrDefault(VI.Tsp.MultimeterFunctionMode.VoltageDC).ValueDescriptionPair()
                End If
            Case NameOf(K3700.MultimeterSubsystem.FunctionRange)
                With Me._SenseRangeNumeric
                    .Maximum = CDec(subsystem.FunctionRange.Max)
                    .Minimum = CDec(subsystem.FunctionRange.Min)
                End With
            Case NameOf(K3700.MultimeterSubsystem.FunctionRangeDecimalPlaces)
                Me._SenseRangeNumeric.DecimalPlaces = subsystem.DefaultFunctionModeDecimalPlaces
            Case NameOf(K3700.MultimeterSubsystem.FunctionUnit)
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.FunctionUnit}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
            Case NameOf(K3700.MultimeterSubsystem.OpenDetectorEnabled)
                If subsystem.OpenDetectorEnabled.HasValue Then Me._OpenDetectorCheckBox.Checked = subsystem.OpenDetectorEnabled.Value
            Case NameOf(K3700.MultimeterSubsystem.PowerLineCycles)
                If subsystem.PowerLineCycles.HasValue Then Me._PowerLineCyclesNumeric.Value = CDec(subsystem.PowerLineCycles.Value)
            Case NameOf(K3700.MultimeterSubsystem.PowerLineCyclesRange)
                With Me._PowerLineCyclesNumeric
                    .Maximum = CDec(subsystem.PowerLineCyclesRange.Max)
                    .Minimum = CDec(subsystem.PowerLineCyclesRange.Min)
                    .DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces
                End With
            Case NameOf(K3700.MultimeterSubsystem.Range)
                If subsystem.Range.HasValue Then Me.SenseRangeSetter(subsystem.Range.Value)
            Case NameOf(K3700.MultimeterSubsystem.LastReading)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = If(String.IsNullOrWhiteSpace(value), "<last reading>", value)
                subsystem.LastActionElapsedTime = Me.ReadElapsedTime(True)
            Case NameOf(K3700.MultimeterSubsystem.LastActionElapsedTime)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = $"{If(String.IsNullOrWhiteSpace(value), "<last reading>", value)} @{subsystem.LastActionElapsedTime.ToExactMilliseconds:0.0}ms"
            Case NameOf(K3700.MultimeterSubsystem.FailureCode)
                Me._FailureToolStripStatusLabel.Text = subsystem.FailureCode
            Case NameOf(K3700.MultimeterSubsystem.FailureLongDescription)
                Me._FailureToolStripStatusLabel.ToolTipText = subsystem.FailureLongDescription
            Case NameOf(K3700.MultimeterSubsystem.ReadingCaption)
                Me._ReadingToolStripStatusLabel.Text = subsystem.ReadingCaption
        End Select
    End Sub

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MultimeterSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.MultimeterSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.MultimeterSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, K3700.MultimeterSubsystem), e.PropertyName)
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

#Region " CHANNEL "

    ''' <summary> Handles the Channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As ChannelSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K3700.ChannelSubsystem.ClosedChannels)
                subsystem.LastActionElapsedTime = Me.ReadElapsedTime(True)
                Me.ClosedChannels = subsystem.ClosedChannels
            Case NameOf(K3700.ChannelSubsystem.LastActionElapsedTime)
                Me._ChannelListTextBox.Text = $"{subsystem.ClosedChannels} @{subsystem.LastActionElapsedTime.ToExactMilliseconds:0.0}ms"
        End Select
    End Sub

    ''' <summary> Channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(ChannelSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.ChannelSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, ChannelSubsystem), e.PropertyName)
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

#Region " DISPLAY "

    ''' <summary> Handles the Display subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As DisplaySubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K3700.DisplaySubsystem.DisplayScreen)
        End Select
    End Sub

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplaySubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.DisplaySubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.DisplaySubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, K3700.DisplaySubsystem), e.PropertyName)
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
    Protected Overrides Sub OnLastError(ByVal lastError As VI.DeviceError)
        If lastError IsNot Nothing Then
            Me._LastErrorTextBox.ForeColor = If(lastError.IsError, Drawing.Color.OrangeRed, Drawing.Color.Aquamarine)
            Me._LastErrorTextBox.Text = lastError.CompoundErrorMessage
        End If
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
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.StatusSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.StatusSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, K3700.StatusSubsystem), e.PropertyName)
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

#Region " STATUS DISPLAY "

    Private _Status As String
    ''' <summary> Gets or sets the status. </summary>
    ''' <value> The status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property Status As String
        Get
            Return Me._Status
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.Status) Then
                Me._Status = value
                Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(value, Me._StatusLabel)
                Me._StatusLabel.ToolTipText = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _Identity As String
    ''' <summary> Gets or sets the identity. </summary>
    ''' <value> The identity. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property Identity As String
        Get
            Return Me._Identity
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.Identity) Then
                Me._Identity = value
                Me._IdentityLabel.Text = isr.Core.Pith.CompactExtensions.Compact(value, Me._IdentityLabel)
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _StatusRegisterCaption As String
    ''' <summary> Gets or sets the status register caption. </summary>
    ''' <value> The status register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property StatusRegisterCaption As String
        Get
            Return Me._StatusRegisterCaption
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.StatusRegisterCaption) Then
                Me._StatusRegisterCaption = value
                Me._StatusRegisterLabel.Text = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _StandardRegisterCaption As String
    ''' <summary> Gets or sets the standard register caption. </summary>
    ''' <value> The status register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property StandardRegisterCaption As String
        Get
            Return Me._StandardRegisterCaption
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.StandardRegisterCaption) Then
                Me._StandardRegisterCaption = value
                Me._StandardRegisterLabel.Text = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the standard register status. </summary>
    ''' <value> The standard register status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property ServiceRequestEnableBitmask As Integer
        Get
            Return CInt(Me._ServiceRequestEnableBitmaskNumeric.Value)
        End Get
        Set(value As Integer)
            Me._ServiceRequestEnableBitmaskNumeric.Value = value
        End Set
    End Property

    Private _ServiceRequestEnableBitmaskCaption As String
    ''' <summary> Gets or sets the standard register caption. </summary>
    ''' <value> The standard register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property ServiceRequestEnableBitmaskCaption As String
        Get
            Return Me._ServiceRequestEnableBitmaskCaption
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.ServiceRequestEnableBitmaskCaption) Then
                Me._ServiceRequestEnableBitmaskCaption = value
                Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = value
            End If
        End Set
    End Property

#End Region

#Region " DEVICE SETTINGS: FUNCTION MODE "

    ''' <summary> Displays a function modes. </summary>
    Private Sub DisplayFunctionModes(ByVal selectedValue As VI.Tsp.MultimeterFunctionMode)
        With Me._SenseFunctionComboBox
            If .DataSource Is Nothing OrElse .Items.Count <= 0 Then
                .DataSource = Nothing
                .Items.Clear()
                .DataSource = GetType(VI.Tsp.MultimeterFunctionMode).ValueDescriptionPairs()
                .DisplayMember = "Value"
                .ValueMember = "Key"
            End If
            If .Items.Count > 0 Then
                .SelectedItem = selectedValue.ValueDescriptionPair
            End If
        End With
    End Sub

    ''' <summary>
    ''' Gets or sets the selected function mode.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private Function SelectedFunctionMode() As VI.Tsp.MultimeterFunctionMode
        Dim activity As String = $"Selecting function mode {Me._SenseFunctionComboBox.SelectedItem}"
        Dim result As VI.Tsp.MultimeterFunctionMode = MultimeterFunctionMode.CurrentAC
        Try
            result = CType(CType(Me._SenseFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                  Of [Enum], String)).Key, VI.Tsp.MultimeterFunctionMode)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(Me._SenseFunctionComboBox, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
        End Try
        Return result
    End Function

#End Region

#Region " CHANNELS "

    ''' <summary> Gets or sets the scan list of closed channels. </summary>
    ''' <value> The closed channels. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ClosedChannels() As String
        Get
            Return Me._ClosedChannelsTextBox.Text
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then
                value = "nil"
            ElseIf String.IsNullOrWhiteSpace(value) Then
                value = "all open"
            End If
            Me._ClosedChannelsTextBox.Text = value
        End Set
    End Property

    ''' <summary> Adds new items to the combo box. </summary>
    Friend Sub UpdateChannelListComboBox()
        If Me.Visible Then
            ' check if we are asking for a new channel list
            If Me._ChannelListComboBox IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(Me._ChannelListComboBox.Text) AndAlso
                    Me._ChannelListComboBox.FindString(Me._ChannelListComboBox.Text) < 0 Then
                ' if we have a new string, add it to the channel list
                Me._ChannelListComboBox.Items.Add(Me._ChannelListComboBox.Text)
            End If
        End If
    End Sub

    ''' <summary> Event handler. Called by _closeChannelsButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _CloseChannelsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _CloseChannelsButton.Click
        Dim activity As String = "closing channels"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.UpdateChannelListComboBox()
            ' this also turns off status bit setting that enable notifications of wait completion.
            Me.Device.ClearExecutionState()
            ' must be reabled after clearing execution state.
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.StartElapsedStopwatch(2)
            Me.Device.ChannelSubsystem.ApplyClosedChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(2))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by channel_OpenButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _OpenChannelsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _OpenChannelsButton.Click
        Dim activity As String = "opening channels"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            UpdateChannelListComboBox()
            ' this also turns off status bit setting that enable notifications of wait completion.
            Me.Device.ClearExecutionState()
            ' must be reabled after clearing execution state.
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.StartElapsedStopwatch(2)
            Me.Device.ChannelSubsystem.ApplyOpenChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(2))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally

            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by CloseOnlyButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _CloseOnlyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _CloseOnlyButton.Click
        Dim activity As String = "closing only channels"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            UpdateChannelListComboBox()
            ' this also turns off status bit setting that enable notifications of wait completion.
            Me.Device.ClearExecutionState()
            ' must be reabled after clearing execution state.
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.StartElapsedStopwatch(3)
            Me.Device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
            Me.Device.ChannelSubsystem.ApplyClosedChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(2))
            ' this works only if a single channel:
            ' VI.ChannelSubsystem.CloseChannels(Me.Device, Me._channelListComboBox.Text)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally

            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by OpenAllButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _OpenAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _OpenAllButton.Click

        Dim activity As String = "opening all channels"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            ' this also turns off status bit setting that enable notifications of wait completion.
            Me.Device.ClearExecutionState()
            ' must be reabled after clearing execution state.
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.StartElapsedStopwatch(2)
            Me.Device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally

            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: READING "

    ''' <summary> Selects a new reading to display. </summary>
    ''' <param name="value"> The <see cref="TraceMessage">message</see> to display and
    ''' log. </param>
    ''' <returns> The VI.ReadingElements. </returns>
    Friend Function SelectReading(ByVal value As VI.ReadingTypes) As VI.ReadingTypes
        If Me.IsDeviceOpen AndAlso
                (value <> VI.ReadingTypes.None) AndAlso (value <> Me.SelectedReading) Then
            Me._ReadingComboBox.ComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.SelectedReading
    End Function

    ''' <summary> Gets the selected reading. </summary>
    ''' <value> The selected reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedReading() As VI.ReadingTypes
        Get
            Return CType(CType(Me._ReadingComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                                            Of [Enum], String)).Key, VI.ReadingTypes)
        End Get
    End Property

    ''' <summary> Event handler. Called by InitButton for click events. Initiates a reading for
    ''' retrieval by way of the service request event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitiateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InitiateButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me._InfoProvider.Annunciate(sender, "Not implemented yet")

            ' clear execution state before enabling events
            Me.Device.ClearExecutionState()

            ' set the service request
            Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All And Not VI.Pith.ServiceRequests.MessageAvailable)

            ' trigger the initiation of the measurement letting the service request do the rest.
            Me.Device.ClearExecutionState()
            Me.StartElapsedStopwatch(0)
            ' Me.Device.TriggerSubsystem.Initiate()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by _ReadingComboBox for selected index changed events. Selects
    ''' a new reading to display. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ReadingComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            If Me._ReadingComboBox.Enabled AndAlso Me._ReadingComboBox.SelectedIndex >= 0 AndAlso
                    Not String.IsNullOrWhiteSpace(Me._ReadingComboBox.Text) Then
                ' Me.DisplayReading(Me.Device.MultimeterSubsystem)
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by _ReadButton for click events. Query the Device for a
    ''' reading. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.StartElapsedStopwatch()
            Me.Device.MultimeterSubsystem.Measure()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: SENSE "

    ''' <summary> Sense range setter. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub SenseRangeSetter(ByVal value As Double)
        If value <= Me._SenseRangeNumeric.Maximum AndAlso value >= Me._SenseRangeNumeric.Minimum Then Me._SenseRangeNumeric.Value = CDec(value)
    End Sub

    ''' <summary> Event handler. Called by _SenseFunctionComboBox for selected index changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="degC")>
    Private Sub _SenseFunctionComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SenseFunctionComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim control As Windows.Forms.Control = TryCast(sender, Windows.Forms.Control)
        If control IsNot Nothing AndAlso control.Enabled Then
            Dim functionMode As MultimeterFunctionMode = Me.SelectedFunctionMode
            Me.Device.MultimeterSubsystem.FunctionUnit = Me.Device.MultimeterSubsystem.ToUnit(functionMode)
            Me.Device.MultimeterSubsystem.FunctionRange = Me.Device.MultimeterSubsystem.ToRange(functionMode)
            Me.Device.MultimeterSubsystem.FunctionRangeDecimalPlaces = Me.Device.MultimeterSubsystem.ToDecimalPlaces(functionMode)
        End If
    End Sub

    ''' <summary> Applies the function mode button click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyFunctionModeButton_Click(sender As Object, e As EventArgs) Handles _ApplyFunctionModeButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me._Device.MultimeterSubsystem.ApplyFunctionMode(Me.SelectedFunctionMode)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Applies the selected measurements settings.
    ''' </summary>
    Private Sub ApplySenseSettings()

        With Me.Device.MultimeterSubsystem
            If Not Nullable.Equals(.PowerLineCycles, Me._PowerLineCyclesNumeric.Value) Then
                .ApplyPowerLineCycles(Me._PowerLineCyclesNumeric.Value)
            End If

            If Not Nullable.Equals(.AutoDelayMode, Me._AutoDelayMode) Then
                .ApplyAutoDelayMode(Me._AutoDelayMode)
            End If

            If Not Nullable.Equals(.AutoRangeEnabled, Me._AutoRangeCheckBox.Checked) Then
                .ApplyAutoRangeEnabled(Me._AutoRangeCheckBox.Checked)
            End If

            If Not Nullable.Equals(.AutoZeroEnabled, Me._AutoZeroCheckBox.Checked) Then
                .ApplyAutoZeroEnabled(Me._AutoZeroCheckBox.Checked)
            End If

            If Not Nullable.Equals(.FilterEnabled, Me._FilterEnabledCheckBox.Checked) Then
                .ApplyFilterEnabled(Me._FilterEnabledCheckBox.Checked)
            End If

            If Not Nullable.Equals(.FilterCount, Me._FilterCountNumeric.Value) Then
                .ApplyFilterCount(CInt(Me._FilterCountNumeric.Value))
            End If

            If Not Nullable.Equals(.MovingAverageFilterEnabled, Me._MovingAverageRadioButton.Checked) Then
                .ApplyMovingAverageFilterEnabled(Me._MovingAverageRadioButton.Checked)
            End If

            If Not Nullable.Equals(.OpenDetectorEnabled, Me._OpenDetectorCheckBox.Checked) Then
                .ApplyOpenDetectorEnabled(Me._OpenDetectorCheckBox.Checked)
            End If

            If .AutoRangeEnabled Then
                .QueryRange()
            ElseIf Not Nullable.Equals(.Range, Me._SenseRangeNumeric.Value) Then
                .ApplyRange(CInt(Me._SenseRangeNumeric.Value))
            End If

            If Not Nullable.Equals(.FilterWindow, 0.01 * Me._FilterWindowNumeric.Value) Then
                .ApplyFilterWindow(0.01 * Me._FilterWindowNumeric.Value)
            End If
        End With


    End Sub

    ''' <summary> Event handler. Called by ApplySenseSettingsButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySenseSettingsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ApplySenseSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If Me.SelectedFunctionMode <> Me.Device.MultimeterSubsystem.FunctionMode.Value Then
                Me._InfoProvider.Annunciate(sender, "Set function first")
            Else
                Me.ApplySenseSettings()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Filter enabled check box checked changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _FilterEnabledCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles _FilterEnabledCheckBox.CheckedChanged
        Me._FilterGroupBox.Enabled = Me._FilterEnabledCheckBox.Checked
    End Sub

    Private Sub _OpenDetectorCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles _OpenDetectorCheckBox.CheckedChanged
        _OpenDetectorCheckBox.Text = $"Open Detector {Me._OpenDetectorCheckBox.Checked.GetHashCode:'ON';'ON';'OFF'}"
    End Sub

    Private _AutoDelayMode As MultimeterAutoDelayMode

    ''' <summary> Gets or sets the automatic delay mode. </summary>
    ''' <value> The automatic delay mode. </value>
    Private Property AutoDelayMode As MultimeterAutoDelayMode
        Get
            If Me._AutoDelayCheckBox.CheckState = CheckState.Checked Then
                Me._AutoDelayMode = MultimeterAutoDelayMode.On
            ElseIf Me._AutoDelayCheckBox.CheckState = CheckState.Indeterminate Then
                Me._AutoDelayMode = MultimeterAutoDelayMode.Once
            Else
                Me._AutoDelayMode = MultimeterAutoDelayMode.Off
            End If
            Return Me._AutoDelayMode
        End Get
        Set(value As MultimeterAutoDelayMode)
            Me._AutoDelayMode = value
            Select Case value
                Case MultimeterAutoDelayMode.Off
                    Me._AutoDelayCheckBox.CheckState = CheckState.Unchecked
                Case MultimeterAutoDelayMode.On
                    Me._AutoDelayCheckBox.CheckState = CheckState.Checked
                Case MultimeterAutoDelayMode.Once
                    Me._AutoDelayCheckBox.CheckState = CheckState.Indeterminate
            End Select
        End Set
    End Property
    ''' <summary> Automatic delay check box checked changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _AutoDelayCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles _AutoDelayCheckBox.CheckedChanged
        If Me._AutoDelayCheckBox.CheckState = CheckState.Checked Then
            Me.AutoDelayMode = MultimeterAutoDelayMode.On
            Me._AutoDelayCheckBox.Text = "Auto Delay ON"
        ElseIf Me._AutoDelayCheckBox.CheckState = CheckState.Indeterminate Then
            Me.AutoDelayMode = MultimeterAutoDelayMode.Once
            Me._AutoDelayCheckBox.Text = "Auto Delay ONCE"
        Else
            Me.AutoDelayMode = MultimeterAutoDelayMode.Off
            Me._AutoDelayCheckBox.Text = "Auto Delay OFF"
        End If
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: RESET "

    ''' <summary> Clears interface. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearInterfaceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearInterfaceMenuItem.Click
        Dim activity As String = "clearing interface"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me._InfoProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ClearInterface()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
        Dim activity As String = "clearing device active state (CDC)"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me._InfoProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ClearActiveState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me._InfoProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ClearExecutionState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me.Device.ResetKnownState()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
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
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
        AddHandler Me._LogTraceLevelComboBox.ComboBox.SelectedIndexChanged, AddressOf Me._LogTraceLevelComboBox_SelectedIndexChanged
        TalkerControlBase.SelectItem(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel)

        TalkerControlBase.ListTraceEventLevels(Me._DisplayTraceLevelComboBox.ComboBox)
        AddHandler Me._DisplayTraceLevelComboBox.ComboBox.SelectedIndexChanged, AddressOf Me._DisplayTraceLevelComboBox_SelectedIndexChanged
        TalkerControlBase.SelectItem(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel)

    End Sub

    ''' <summary>
    ''' Event handler. Called by _LogTraceLevelComboBox for selected value changed events.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _LogTraceLevelComboBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "selecting log trace level on this instrument only"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Device.ApplyTalkerTraceLevel(ListenerType.Logger,
                                            TalkerControlBase.SelectedValue(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
    Private Sub _DisplayTraceLevelComboBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "selecting Display trace level on this instrument only"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Device.ApplyTalkerTraceLevel(ListenerType.Display,
                                            TalkerControlBase.SelectedValue(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.StartElapsedStopwatch()
                Me.ReadServiceRequestStatus()
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Session notification level combo box selected index changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionNotificationLevelComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _SessionNotificationLevelComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "selecting session notification level"
        Dim combo As Core.Controls.ToolStripComboBox = CType(sender, Core.Controls.ToolStripComboBox)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If combo IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.MessageNotificationLevel = VI.Pith.SessionBase.SelectedValue(combo, NotifySyncLevel.None)
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.Session.ServiceRequestEventEnabled Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.Device.Session.EnableServiceRequest()
                    If Me._ServiceRequestEnableBitmaskNumeric.Value = 0 Then
                        If Me._ServiceRequestEnableBitmaskNumeric.Value = 0 Then
                            Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All)
                        Else
                            Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableBitmaskNumeric.Value, VI.Pith.ServiceRequests))
                        End If
                    Else
                        Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableBitmaskNumeric.Value, VI.Pith.ServiceRequests))
                    End If
                Else
                    Me.Device.Session.DisableServiceRequest()
                    Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.None)
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
            Me._InfoProvider.Clear()
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
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " READ AND WRITE "

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal sender As Instrument.SimpleReadWriteControl, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(Instrument.SimpleReadWriteControl.StatusMessage)
                    Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusMessage, Me._StatusLabel)
                    Me._StatusLabel.ToolTipText = sender.StatusMessage
                Case NameOf(Instrument.SimpleReadWriteControl.ServiceRequestValue)
                    Me._StatusRegisterLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
            End Select
        End If
    End Sub

    ''' <summary> Event handler. Called by <see crefname="_SimpleReadWriteControl"/> for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleReadWriteControl_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Handles _SimpleReadWriteControl.PropertyChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Instrument.SimpleReadWriteControl)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._SimpleReadWriteControl_PropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, Instrument.SimpleReadWriteControl), e.PropertyName)
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

#Region " TOOL STRIP RENDERER "

    ''' <summary> A custom professional renderer. </summary>
    ''' <license>
    ''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
    ''' Licensed under The MIT License.</para><para>
    ''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
    ''' </license>
    ''' <history date="10/30/2017" by="David" revision=""> Created. </history>
    Private Class CustomProfessionalRenderer
        Inherits ToolStripProfessionalRenderer

        Protected Overrides Sub OnRenderLabelBackground(ByVal e As ToolStripItemRenderEventArgs)
            If e IsNot Nothing AndAlso (e.Item.BackColor <> System.Drawing.SystemColors.ControlDark) Then
                Using brush As New Drawing.SolidBrush(e.Item.BackColor)
                    e.Graphics.FillRectangle(brush, e.Item.ContentRectangle)
                End Using
            End If
        End Sub
    End Class

#End Region

End Class

