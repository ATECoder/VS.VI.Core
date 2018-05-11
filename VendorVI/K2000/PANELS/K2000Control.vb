Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.CheckBoxExtensions
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.NumericUpDownExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.Core.Pith.TimeSpanExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Provides a user interface for a Keithley 200X Device. </summary>
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
<System.ComponentModel.DisplayName("K2000 Control"),
      System.ComponentModel.Description("Keithley 2000 Device Control"),
      System.Drawing.ToolboxBitmap(GetType(K2000Control))>
Public Class K2000Control
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
    <System.Diagnostics.DebuggerNonUserCode()>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Me.InitializingComponents = True
                ' the device gets closed and disposed (if panel is device owner) in the base class
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
    ''' <param name="device">    The device. </param>
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
        AddHandler Me.Device.ArmLayer1Subsystem.PropertyChanged, AddressOf Me.ArmLayer1SubsystemPropertyChanged
        AddHandler Me.Device.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
        AddHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
        'AddHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
        AddHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
        AddHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
        AddHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
        AddHandler Me.Device.SenseFourWireResistanceSubsystem.PropertyChanged, AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged
        AddHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
        AddHandler Me.Device.TraceSubsystem.PropertyChanged, AddressOf Me.TraceSubsystemPropertyChanged
        AddHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
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
            RemoveHandler Me.Device.ArmLayer1Subsystem.PropertyChanged, AddressOf Me.ArmLayer1SubsystemPropertyChanged
            RemoveHandler Me.Device.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
            RemoveHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
            'RemoveHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseFourWireResistanceSubsystem.PropertyChanged, AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
            RemoveHandler Me.Device.TraceSubsystem.PropertyChanged, AddressOf Me.TraceSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " ARM LAYER 1 "

    ''' <summary> Handle the ArmLayer1 subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As ArmLayer1Subsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K2000.ArmLayer1Subsystem.ArmCount)
                If subsystem.ArmCount.HasValue Then
                    Me._ArmLayer1CountNumeric.Value = subsystem.ArmCount.Value
                End If
            Case NameOf(K2000.ArmLayer1Subsystem.ArmSource)
                If subsystem.ArmSource.HasValue AndAlso Me._ArmLayer1SourceComboBox.ComboBox.Items.Count > 0 Then
                    Me._ArmLayer1SourceComboBox.ComboBox.SelectedItem = subsystem.ArmSource.Value.ValueNamePair
                End If
            Case NameOf(K2000.ArmLayer1Subsystem.SupportedArmSources)
                subsystem.ListSupportedArmSources(Me._ArmLayer1SourceComboBox.ComboBox)
                If subsystem.ArmSource.HasValue AndAlso Me._ArmLayer1SourceComboBox.ComboBox.Items.Count > 0 Then
                    Me._ArmLayer1SourceComboBox.ComboBox.SelectedItem = subsystem.ArmSource.Value.ValueNamePair
                End If
        End Select
    End Sub

    ''' <summary> ArmLayer1 subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ArmLayer1SubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(ArmLayer1Subsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.ArmLayer1SubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, ArmLayer1Subsystem), e.PropertyName)
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

#Region " FORMAT "

    ''' <summary> Handle the format subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As FormatSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K2000.FormatSubsystem.Elements)
                subsystem.ListElements(Me._ReadingComboBox.ComboBox, ReadingTypes.Units)
        End Select
    End Sub

    ''' <summary> Format subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub FormatSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(FormatSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.FormatSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, FormatSubsystem), e.PropertyName)
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

#Region " MEASURE "

    ''' <summary> Handles the Measure subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As MeasureSubsystem, ByVal propertyName As String)
        If Me.InitializingComponents OrElse subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K2000.MeasureSubsystem.LastReading)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = If(String.IsNullOrWhiteSpace(value), "<last reading>", value)
                subsystem.LastActionElapsedTime = Me.ReadElapsedTime(True)
            Case NameOf(K2000.MeasureSubsystem.LastActionElapsedTime)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = $"{If(String.IsNullOrWhiteSpace(value), "<last reading>", value)} @{subsystem.LastActionElapsedTime.ToExactMilliseconds:0.0}ms"
            Case NameOf(K2000.MeasureSubsystem.FailureCode)
                Me._FailureToolStripStatusLabel.Text = subsystem.FailureCode
            Case NameOf(K2000.MeasureSubsystem.FailureLongDescription)
                Me._FailureToolStripStatusLabel.ToolTipText = subsystem.FailureLongDescription
            Case NameOf(K2000.MeasureSubsystem.ReadingCaption)
                Me._ReadingToolStripStatusLabel.Text = subsystem.ReadingCaption
        End Select
    End Sub

    ''' <summary> Measure subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(MeasureSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.MeasureSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, MeasureSubsystem), e.PropertyName)
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

#Region " SENSE "

    ''' <summary> Handles the supported function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnSupportedFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.SupportedFunctionModes <> VI.Scpi.SenseFunctionModes.None Then
            subsystem.DisplaySupportedFunctionModes(Me._SenseFunctionComboBox)
        End If
    End Sub

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.FunctionMode.HasValue Then
            Dim value As VI.Scpi.SenseFunctionModes = subsystem.FunctionMode.GetValueOrDefault(VI.Scpi.SenseFunctionModes.None)
            If value <> VI.Scpi.SenseFunctionModes.None Then
                Me.Device.MeasureSubsystem.Readings.Reading.Unit = subsystem.ToUnit(value)
                Me._SenseFunctionComboBox.SafeSelectItem(value, value.Description)
            End If
        End If
    End Sub

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SenseSubsystem, ByVal propertyName As String)
        If Me.InitializingComponents OrElse subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(K2000.SenseSubsystem.LastReading)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = If(String.IsNullOrWhiteSpace(value), "<last reading>", value)
                subsystem.LastActionElapsedTime = Me.ReadElapsedTime(True)
            Case NameOf(K2000.SenseSubsystem.LastActionElapsedTime)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = $"{If(String.IsNullOrWhiteSpace(value), "<last reading>", value)} @{subsystem.LastActionElapsedTime.ToExactMilliseconds:0.0}ms"
            Case NameOf(K2000.SenseSubsystem.FailureCode)
                Me._FailureToolStripStatusLabel.Text = subsystem.FailureCode
            Case NameOf(K2000.SenseSubsystem.FailureLongDescription)
                Me._FailureToolStripStatusLabel.ToolTipText = subsystem.FailureLongDescription
            Case NameOf(K2000.SenseSubsystem.ReadingCaption)
                Me._ReadingToolStripStatusLabel.Text = subsystem.ReadingCaption
            Case NameOf(K2000.SenseSubsystem.SupportedFunctionModes)
                Me.OnSupportedFunctionModesChanged(subsystem)
            Case NameOf(K2000.SenseSubsystem.FunctionMode)
                Me.OnFunctionModesChanged(subsystem)
            Case NameOf(K2000.SenseSubsystem.FunctionRange)
                With Me._SenseRangeNumeric
                    .Minimum = CDec(subsystem.FunctionRange.Min)
                    .Maximum = CDec(subsystem.FunctionRange.Max)
                End With
            Case NameOf(K2000.SenseSubsystem.FunctionRangeDecimalPlaces)
                Me._SenseRangeNumeric.DecimalPlaces = subsystem.FunctionRangeDecimalPlaces
            Case NameOf(K2000.SenseSubsystem.FunctionUnit)
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.FunctionUnit}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
        End Select
    End Sub

    ''' <summary> Sense subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SenseSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SenseSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SenseSubsystem), e.PropertyName)
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

#Region " SENSE VOLTAGE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SenseVoltageSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(K2000.SenseVoltageSubsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(K2000.SenseVoltageSubsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Dim nplc As Double = subsystem.PowerLineCycles.Value
                    Me._IntegrationPeriodNumeric.SafeValueSetter(StatusSubsystemBase.FromPowerLineCycles(nplc).TotalMilliseconds)
                End If
            Case NameOf(K2000.SenseVoltageSubsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense voltage subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseVoltageSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SenseVoltageSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SenseVoltageSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SenseVoltageSubsystem), e.PropertyName)
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

#Region " SENSE CURRENT "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SenseCurrentSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(K2000.SenseCurrentSubsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(K2000.SenseCurrentSubsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Dim nplc As Double = subsystem.PowerLineCycles.Value
                    Me._IntegrationPeriodNumeric.SafeValueSetter(StatusSubsystemBase.FromPowerLineCycles(nplc).TotalMilliseconds)
                End If
            Case NameOf(K2000.SenseCurrentSubsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense Current subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseCurrentSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SenseCurrentSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SenseCurrentSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SenseCurrentSubsystem), e.PropertyName)
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

#Region " FOUR WIRE SENSE RESISTANCE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SenseFourWireResistanceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(K2000.SenseFourWireResistanceSubsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(K2000.SenseFourWireResistanceSubsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Dim nplc As Double = subsystem.PowerLineCycles.Value
                    Me._IntegrationPeriodNumeric.SafeValueSetter(StatusSubsystemBase.FromPowerLineCycles(nplc).TotalMilliseconds)
                End If
            Case NameOf(K2000.SenseFourWireResistanceSubsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense Four Wire Resistance subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseFourWireResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SenseFourWireResistanceSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SenseFourWireResistanceSubsystem), e.PropertyName)
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

#Region " SENSE RESISTANCE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SenseResistanceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(K2000.SenseResistanceSubsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(K2000.SenseResistanceSubsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Dim nplc As Double = subsystem.PowerLineCycles.Value
                    Me._IntegrationPeriodNumeric.SafeValueSetter(StatusSubsystemBase.FromPowerLineCycles(nplc).TotalMilliseconds)
                End If
            Case NameOf(K2000.SenseResistanceSubsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense Four Wire Resistance subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SenseResistanceSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SenseResistanceSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SenseResistanceSubsystem), e.PropertyName)
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
    Protected Overrides Sub OnLastError(ByVal lastError As DeviceError)
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
            Case NameOf(StatusSubsystemBase.ServiceRequestStatus)
                Me._StatusRegisterLabel.Text = $"0x{subsystem.ServiceRequestStatus:X2}"
            Case NameOf(StatusSubsystemBase.StandardEventStatus)
                Me._StandardRegisterLabel.Text = $"0x{subsystem.StandardEventStatus:X2}"
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
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K2000.SystemSubsystem.FrontSwitched)
                Me._ReadTerminalStateButton.CheckState = subsystem.FrontSwitched.ToCheckState
                Windows.Forms.Application.DoEvents()
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
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SystemSubsystemPropertyChanged), New Object() {sender, e})
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

#Region " TRACE "

    ''' <summary> Handle the Trace subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As TraceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K2000.TraceSubsystem.PointsCount)
            Case NameOf(K2000.TraceSubsystem.ActualPointCount)
            Case NameOf(K2000.TraceSubsystem.FirstPointNumber)
            Case NameOf(K2000.TraceSubsystem.LastPointNumber)
        End Select
        Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary> Trace subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031: DoNotCatchGeneralExceptionTypes")>
    Private Sub TraceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(TraceSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.TraceSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, TraceSubsystem), e.PropertyName)
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

#Region " TRIGGER "

    ''' <summary> Handle the Trigger subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As TriggerSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K2000.TriggerSubsystem.TriggerCount)
                If subsystem.TriggerCount.HasValue Then
                    Me._TriggerCountNumeric.Value = subsystem.TriggerCount.Value
                End If
            Case NameOf(K2000.TriggerSubsystem.ContinuousEnabled)
                Me._ContinuousTriggerEnabledMenuItem.CheckState = subsystem.ContinuousEnabled.ToCheckState
            Case NameOf(K2000.TriggerSubsystem.TriggerSource)
                If subsystem.TriggerSource.HasValue AndAlso Me._TriggerSourceComboBox.ComboBox.Items.Count > 0 Then
                    Me._TriggerSourceComboBox.ComboBox.SelectedItem = subsystem.TriggerSource.Value.ValueNamePair
                End If
            Case NameOf(K2000.TriggerSubsystem.SupportedTriggerSources)
                subsystem.ListSupportedTriggerSources(Me._TriggerSourceComboBox.ComboBox)
                If subsystem.TriggerSource.HasValue AndAlso Me._TriggerSourceComboBox.ComboBox.Items.Count > 0 Then
                    Me._TriggerSourceComboBox.ComboBox.SelectedItem = subsystem.TriggerSource.Value.ValueNamePair
                End If
        End Select
    End Sub

    ''' <summary> Trigger subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TriggerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(TriggerSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.TriggerSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, TriggerSubsystem), e.PropertyName)
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

    Private _ActualFunctionMode As VI.Scpi.SenseFunctionModes
    ''' <summary>
    ''' Return the last Sense function mode.  This is set only after the 
    ''' actual value is applies whereas the <see cref="SelectedFunctionMode">selected mode</see>
    ''' reflects the status of the combo box.
    ''' </summary>
    Friend ReadOnly Property ActualFunctionMode() As VI.Scpi.SenseFunctionModes
        Get
            Return Me._ActualFunctionMode
        End Get
    End Property

    ''' <summary>
    ''' Selects a new sense mode.
    ''' </summary>
    Friend Sub ApplyFunctionMode(ByVal value As VI.Scpi.SenseFunctionModes)
        If Me.IsDeviceOpen AndAlso
                ((Me._Device.SenseSubsystem.SupportedFunctionModes And value) <> 0) AndAlso
                (value <> Me._ActualFunctionMode) Then
            Me._Device.SenseSubsystem.ApplyFunctionMode(value)
        End If
    End Sub

    ''' <summary>
    ''' Gets or sets the selected function mode.
    ''' </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedFunctionMode() As VI.Scpi.SenseFunctionModes
        Get
            Return CType(CType(Me._SenseFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, VI.Scpi.SenseFunctionModes)
        End Get
    End Property

#End Region

#Region " CONTROL EVENT HANDLERS: READING "

#Region " READ "


    ''' <summary> Selects a new reading to display.
    ''' </summary>
    Friend Function SelectReading(ByVal value As VI.ReadingTypes) As VI.ReadingTypes
        If Me.IsDeviceOpen AndAlso (value <> VI.ReadingTypes.None) AndAlso (value <> Me.SelectedReadingType) Then
            Me._ReadingComboBox.ComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.SelectedReadingType
    End Function

    ''' <summary> Gets the type of the selected reading. </summary>
    ''' <value> The type of the selected reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedReadingType() As VI.ReadingTypes
        Get
            Return CType(CType(Me._ReadingComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, VI.ReadingTypes)
        End Get
    End Property

    ''' <summary> Reading combo box selected index changed. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _ReadingComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "selecting a reading to display"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.MeasureSubsystem.SelectActiveReading(Me.SelectedReadingType)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Read button click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadButton.Click
        Dim activity As String = "reading"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.SystemSubsystem.QueryFrontSwitched()
            Me.StartElapsedStopwatch()
            Me.Device.MeasureSubsystem.Read()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " EVENTS "

    ''' <summary> Toggles auto trigger. </summary>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AutoInitiateMenuItem_Click(sender As Object, e As EventArgs) Handles _AutoInitiateMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "toggling re-trigger mode"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Gets or sets the trace readings. </summary>
    ''' <value> The trace readings. </value>
    Private ReadOnly Property TraceReadings As List(Of Readings)

    ''' <summary> Handles the measurement completed request. </summary>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub HandleMeasurementCompletedRequest(sender As Object, e As EventArgs)
        Dim activity As String = "handling measurement event"
        Try
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} SRQ: {Me.Device.StatusSubsystem.ServiceRequestStatus:X};. ")
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If Me.Device.StatusSubsystem.MeasurementAvailable Then

                If Me.Device.TriggerSubsystem.TriggerCount.Value = 1 Then
                    If Me.Device.TraceSubsystem.FeedSource = VI.Scpi.FeedSource.None Then
                        activity = "fetching a single readings"
                        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                        Me.Device.MeasureSubsystem.Fetch()
                    Else
                        activity = "fetching buffered readings"
                        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                        Dim values As IEnumerable(Of Readings) = Me.Device.TraceSubsystem.QueryReadings(Me.Device.MeasureSubsystem.Readings)
                        If Me.TraceReadings Is Nothing Then Me._TraceReadings = New List(Of Readings)
                        Me.TraceReadings.AddRange(values)

                        activity = "updating the display"
                        If Me.TraceReadings.Count = values.Count Then
                            TraceSubsystem.DisplayReadings(Me._ReadingsDataGridView, values)
                        Else
                            Me._ReadingsDataGridView.Invalidate()
                        End If
                    End If
                Else
                    activity = "fetching buffered readings"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    Dim values As IEnumerable(Of Readings) = Me.Device.TraceSubsystem.QueryReadings(Me.Device.MeasureSubsystem.Readings)
                    If Me.TraceReadings Is Nothing Then Me._TraceReadings = New List(Of Readings)
                    Me.TraceReadings.AddRange(values)

                    activity = "updating the display"
                    If Me.TraceReadings.Count = values.Count Then
                        TraceSubsystem.DisplayReadings(Me._ReadingsDataGridView, values)
                    Else
                        Me._ReadingsDataGridView.Invalidate()
                    End If
                End If
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")

                If Me._AutoInitiateMenuItem.Checked Then
                    activity = "initiating next measurement(s)"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    Me.StartElapsedStopwatch()
                    Me.Device.TraceSubsystem.ClearBuffer() ' ?@#  7/6/17
                    Me.Device.TriggerSubsystem.Initiate()
                End If
            Else
                activity = "measurement not available--is error?"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")

            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(Me._ReadBufferButton, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Gets the measurement complete handler added. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <value> The measurement complete handler added. </value>
    Private Property MeasurementCompleteHandlerAdded As Boolean

    ''' <summary> Adds measurement complete event handler. </summary>
    Private Sub AddMeasurementCompleteEventHandler()

        Dim activity As String = ""
        If Not Me.MeasurementCompleteHandlerAdded Then

            ' clear execution state before enabling events
            activity = "Clearing execution state"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.ClearExecutionState()

            activity = "Enabling session service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.Session.EnableServiceRequest()

            activity = "Adding device service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.AddServiceRequestEventHandler()

            activity = "Turning on measurement events"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)

            activity = "Turning on status service request"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            ' Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.MeasurementEvent)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All And Not VI.Pith.ServiceRequests.MessageAvailable)

            activity = "Adding re-triggering event handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            AddHandler Me.Device.ServiceRequested, AddressOf HandleMeasurementCompletedRequest
            Me.MeasurementCompleteHandlerAdded = True
        End If
    End Sub

    ''' <summary> Removes the measurement complete event handler. </summary>
    Private Sub RemoveMeasurementCompleteEventHandler()

        Dim activity As String = ""
        If Me.MeasurementCompleteHandlerAdded Then

            activity = "Disabling session service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.Session.DisableServiceRequest()

            activity = "Removing device service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.RemoveServiceRequestEventHandler()

            activity = "Turning off measurement events"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.None)

            activity = "Turning off status service request"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.None)

            activity = "Removing re-triggering event handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            RemoveHandler Me.Device.ServiceRequested, AddressOf HandleMeasurementCompletedRequest

            Me.MeasurementCompleteHandlerAdded = False

        End If
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _HandleMeasurementEventMenuItem_CheckStateChanged(sender As Object, e As EventArgs) Handles _HandleMeasurementEventMenuItem.CheckStateChanged

        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        If menuItem Is Nothing Then Return
        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            activity = "Aborting trigger plan"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.AbortTriggerPlan(sender)

            If menuItem.Checked Then

                activity = "Adding measurement completion handler"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.AddMeasurementCompleteEventHandler()

            Else

                activity = "Removing measurement completion handler"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.RemoveMeasurementCompleteEventHandler()

            End If

        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Aborts trigger plan. </summary>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub AbortTriggerPlan(ByVal sender As System.Object)

        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()

            activity = "Aborting trigger plan"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.TriggerSubsystem.Abort()

        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Starts trigger plan. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StartTriggerPlan(ByVal sender As System.Object)

        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()

            activity = "clearing buffer and display"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me._TraceReadings = New List(Of Readings)
            TraceSubsystem.DisplayReadings(Me._ReadingsDataGridView, Me._TraceReadings)

            ' this seems to cause the buffer to not fill. 
            ' Me.Device.TraceSubsystem.ClearBuffer()

            activity = "initiating single trigger measurements(s)"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.StartElapsedStopwatch()
            Me.Device.TriggerSubsystem.Initiate()

        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by the Initiate Button for click events. Initiates a reading for
    ''' retrieval by way of the service request event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitiateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InitiateTriggerButton.Click

        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            activity = "Aborting trigger plan"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.AbortTriggerPlan(sender)

            activity = "Starting trigger plan"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.StartTriggerPlan(sender)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by the Initiate Button for click events. Initiates a reading for
    ''' retrieval by way of the service request event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AbortButton_Click(sender As Object, e As EventArgs) Handles _AbortButton.Click

        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            activity = "Aborting trigger plan"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.AbortTriggerPlan(sender)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " BUFFER "

    ''' <summary>
    ''' Handles the DataError event of the _dataGridView control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="DataGridViewDataErrorEventArgs"/> instance containing the event data.</param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingsDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles _ReadingsDataGridView.DataError
        Try
            ' prevent error reporting when adding a new row or editing a cell
            Dim grid As DataGridView = TryCast(sender, DataGridView)
            If grid IsNot Nothing Then
                If grid.CurrentRow IsNot Nothing AndAlso grid.CurrentRow.IsNewRow Then Return
                If grid.IsCurrentCellInEditMode Then Return
                If grid.IsCurrentRowDirty Then Return
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   $"{e.Exception.Message} occurred editing row {e.RowIndex} column {e.ColumnIndex};. {e.Exception.ToFullBlownString}")
                Me._InfoProvider.Annunciate(grid, $"{e.Exception.Message} occurred editing table")
            End If
        Catch
        End Try
    End Sub

    ''' <summary> Reads buffer button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceButton_Click(sender As Object, e As EventArgs) Handles _ReadBufferButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "reading readings"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.StartElapsedStopwatch()
            Dim values As IEnumerable(Of Readings) = Me.Device.TraceSubsystem.QueryReadings(Me.Device.MeasureSubsystem.Readings)
            Me._ReadingsCountLabel.Text = values?.Count.ToString
            TraceSubsystem.DisplayReadings(Me._ReadingsDataGridView, values)
            Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears the buffer display button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearBufferDisplayButton_Click(sender As Object, e As EventArgs) Handles _ClearBufferDisplayButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "clearing buffer display"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            TraceSubsystem.DisplayReadings(Me._ReadingsDataGridView, New List(Of Readings))
            Me._ReadingsCountLabel.Text = "0"
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#End Region

#Region " CONTROL EVENT HANDLERS: SENSE "

    ''' <summary> Event handler. Called by _SenseFunctionComboBox for selected index changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="degC")>
    Private Sub _SenseFunctionComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SenseFunctionComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim control As Windows.Forms.Control = TryCast(sender, Windows.Forms.Control)
        If control IsNot Nothing Then
            Me.Device.SenseSubsystem.FunctionUnit = Me.Device.SenseSubsystem.ToUnit(Me.SelectedFunctionMode)
            Me.Device.SenseSubsystem.FunctionRange = Me.Device.SenseSubsystem.ToRange(Me.SelectedFunctionMode)
            Me.Device.SenseSubsystem.FunctionRangeDecimalPlaces = Me.Device.SenseSubsystem.ToDecimalPlaces(Me.SelectedFunctionMode)
        End If
    End Sub

    ''' <summary>
    ''' Applies the selected measurements settings.
    ''' </summary>
    Private Sub ApplySenseSettings()

        Me.Device.ClearExecutionState()

        Me.Device.SenseSubsystem.ApplyFunctionMode(Me.SelectedFunctionMode)

        If Me.Device.SenseSubsystem.FunctionMode = VI.Scpi.SenseFunctionModes.CurrentDC Then

            Me.Device.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Ampere

            With Me.Device.SenseCurrentSubsystem
                ' set the Integration Period
                .ApplyPowerLineCycles(StatusSubsystemBase.ToPowerLineCycles(TimeSpan.FromMilliseconds(Me._IntegrationPeriodNumeric.Value)))

                ' set the range
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)

                If Not Me._SenseAutoRangeToggle.Checked Then
                    .ApplyRange(Me._SenseRangeNumeric.Value)
                End If
            End With

        ElseIf Me.Device.SenseSubsystem.FunctionMode = VI.Scpi.SenseFunctionModes.FourWireResistance Then

            Me.Device.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Ohm

            With Me.Device.SenseFourWireResistanceSubsystem
                ' set the Integration Period
                .ApplyPowerLineCycles(StatusSubsystemBase.ToPowerLineCycles(TimeSpan.FromMilliseconds(Me._IntegrationPeriodNumeric.Value)))

                ' set the range
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)

                If Not Me._SenseAutoRangeToggle.Checked Then
                    .ApplyRange(Me._SenseRangeNumeric.Value)
                End If
            End With

        ElseIf Me.Device.SenseSubsystem.FunctionMode = VI.Scpi.SenseFunctionModes.Resistance Then

            Me.Device.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Ohm

            With Me.Device.SenseResistanceSubsystem
                ' set the Integration Period
                .ApplyPowerLineCycles(StatusSubsystemBase.ToPowerLineCycles(TimeSpan.FromMilliseconds(Me._IntegrationPeriodNumeric.Value)))

                ' set the range
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)

                If Not Me._SenseAutoRangeToggle.Checked Then
                    .ApplyRange(Me._SenseRangeNumeric.Value)
                End If
            End With

        ElseIf Me.Device.SenseSubsystem.FunctionMode = VI.Scpi.SenseFunctionModes.VoltageDC Then

            Me.Device.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Volt

            With Me.Device.SenseVoltageSubsystem
                ' set the Integration Period
                .ApplyPowerLineCycles(StatusSubsystemBase.ToPowerLineCycles(TimeSpan.FromMilliseconds(Me._IntegrationPeriodNumeric.Value)))

                ' set the range
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)

                If Not Me._SenseAutoRangeToggle.Checked Then
                    .ApplyRange(Me._SenseRangeNumeric.Value)
                End If
            End With

        End If

        ' get the delay time
        If Me._TriggerDelayNumeric.Value >= 0 Then
            Me.Device.TriggerSubsystem.ApplyDelay(TimeSpan.FromTicks(CLng(TimeSpan.TicksPerSecond * Me._TriggerDelayNumeric.Value)))
        Else
            Me.Device.TriggerSubsystem.ApplyAutoDelayEnabled(True)
        End If

    End Sub

    ''' <summary> Event handler. Called by ApplySenseSettingsButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySenseSettingsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ApplySenseSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.ApplySenseSettings()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{ex.Message} occurred initiating a measurement;. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
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
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
    Private Sub _DisplayTraceLevelComboBox_SelectedValueChanged(sender As Object, e As EventArgs)
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

    ''' <summary> Toggles session message tracing. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
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
                        Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All)
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
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: TERMINALS "

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadTerminalStateButton_Click(sender As Object, e As EventArgs) Handles _ReadTerminalStateButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Reading terminals state"
        Dim button As ToolStripButton = CType(sender, ToolStripButton)
        Try
            If button IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me._InfoProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.SystemSubsystem.QueryFrontSwitched()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Displays terminal state. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ReadTerminalStateButton_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ReadTerminalStateButton.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim button As ToolStripButton = TryCast(sender, ToolStripButton)
        If button IsNot Nothing Then
            If button.CheckState = Windows.Forms.CheckState.Indeterminate Then
                button.Text = "R/F?"
                button.ToolTipText = "Unknown terminal state"
            Else
                button.Text = If(button.Checked, "Front", "Rear")
                button.ToolTipText = If(button.Checked, "Click to switch to Rear", "Click to switch to Front")
            End If
        End If
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: SCAN "

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyTriggerPlanButton_Click(sender As Object, e As EventArgs) Handles _ApplyTriggerPlanButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If Not Me.DesignMode AndAlso sender IsNot Nothing Then
                ' Me.Device.RouteSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(1))
                ' Me.Device.TriggerSubsystem.ApplyTriggerCount(CInt(Me._ChannelLayerCountNumeric.Value))
                Me.Device.TriggerSubsystem.ApplyTriggerSource(Scpi.TriggerSources.External)
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{ex.Message} occurred setting the trigger plan;. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitiateWaitReadButton_Click(sender As Object, e As EventArgs) Handles _InitiateWaitReadButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
#If False Then
            Me.Device.ClearExecutionState()
            ' set the service request
            Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All And Not VI.Pith.ServiceRequests.MessageAvailable)
            Me.Device.Session.Write("*SRE 1") ' Set MSB bit of SRE register
            Me.Device.Session.Write("stat:meas:ptr 32767; ntr 0; enab 512") ' Set all PTR bits and clear all NTR bits for measurement events Set Buffer Full bit of Measurement
            Me.Device.Session.Write(":trac:feed calc") ' Select Calculate as reading source
            Me.Device.Session.Write(":trac:poin 10")   ' Set buffer size to 10 points 
            Me.Device.Session.Write(":trac:egr full") ' Select Full element group
#End If
            ' trigger the initiation of the measurement letting the triggering or service request do the rest.
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Initiating meter;. ")
            Me.Device.TriggerSubsystem.Initiate()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception occurred;. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ConfigureExternalScan_Click(sender As Object, e As EventArgs) Handles _ConfigureExternalScan.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            ' Me.Device.ResetKnownState()
            ' Me.Device.ClearExecutionState()

            Dim count As Integer = CInt(Me._TriggerCountNumeric.Value)
            ' TO_DO: Add service request handler to trace when done. 
            ' enable service requests
            ' Me.Device.Session.EnableServiceRequest()
            ' Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All)
            Me.Device.TriggerSubsystem.ApplyContinuousEnabled(False)
            Me.Device.TriggerSubsystem.Abort()

            Me.Device.SystemSubsystem.ApplyAutoZeroEnabled(True)
            Me.Device.FormatSubsystem.ApplyElements(ReadingTypes.Reading)
            Me.Device.SenseSubsystem.ApplyFunctionMode(Scpi.SenseFunctionModes.FourWireResistance)
            Me.Device.SenseFourWireResistanceSubsystem.ApplyAverageEnabled(False)

            If count <= 1 Then
                ' setting the feed source to none causes -109,"Missing parameter" error
                ' Me.Device.TraceSubsystem.WriteFeedSource(Scpi.FeedSource.None)
                Me.Device.TriggerSubsystem.ApplyTriggerSource(CType(Me._TriggerSourceComboBox.ComboBox.SelectedValue, Scpi.TriggerSources))
                Me.Device.TriggerSubsystem.ApplyTriggerCount(count)

                If Me.Device.TriggerSubsystem.TriggerSource.Value = Scpi.TriggerSources.External Then
                    Me._StatusLabel.Text = "Ready: Initiate meter and then scanner"
                ElseIf Me.Device.TriggerSubsystem.TriggerSource.Value = Scpi.TriggerSources.Immediate Then
                    Me._StatusLabel.Text = "Ready: Initiate meter to take a reading"
                ElseIf Me.Device.TriggerSubsystem.TriggerSource.Value = Scpi.TriggerSources.Bus Then
                    Me._StatusLabel.Text = "Ready: Click the Trigger button to take a reading"
                Else
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Invalid trigger source {Me.Device.TriggerSubsystem.TriggerSource.Value}")
                End If
            Else
                ' use external if the meter waits for the scanner or immediate if the scanner waits for the meter.
                Me.Device.ArmLayer1Subsystem.ApplyArmSource(CType(Me._ArmLayer1SourceComboBox.ComboBox.SelectedValue, Scpi.ArmSources))
                Me.Device.ArmLayer1Subsystem.ApplyArmCount(1)
                Me.Device.ArmLayer2Subsystem.ApplyArmSource(Scpi.ArmSources.Immediate)
                Me.Device.ArmLayer2Subsystem.ApplyArmCount(1)
                Me.Device.ArmLayer2Subsystem.ApplyDelay(TimeSpan.Zero)
                Me.Device.TriggerSubsystem.ApplyTriggerSource(Scpi.TriggerSources.External)
                Me.Device.TriggerSubsystem.ApplyTriggerCount(count)
                Me.Device.TriggerSubsystem.ApplyDelay(TimeSpan.Zero)
                Me.Device.TriggerSubsystem.ApplyDirection(Scpi.Direction.Source)
                If count > 1 Then
                    Me.Device.TraceSubsystem.WriteFeedSource(Scpi.FeedSource.Sense)
                    ' Me.Device.TraceSubsystem.ApplyPointsCount(count)
                    Me.Device.Session.WriteLine(":TRAC:POIN:AUTO 1")
                    Me.Device.TraceSubsystem.WriteFeedControl(Scpi.FeedControl.Next)
                Else
                    ' This does not work. Getting device errors:
                    ' -109,"Missing parameter"
                    ' -221,"Settings conflict", ""
                    ' -222,"Parameter data out of range", ""
                    ' -102,"Syntax error", ""
                    ' -109,"Missing parameter", ""
                    'Me.Device.Session.WriteLine(":TRAC:POIN:AUTO 1")
                    'Me.Device.TraceSubsystem.WriteFeedControl(Scpi.FeedControl.Never)
                    'Me.Device.TraceSubsystem.WriteFeedSource(Scpi.FeedSource.None)
                End If
                If Me.Device.ArmLayer1Subsystem.ArmSource.Value = Scpi.ArmSources.External Then
                    Me._StatusLabel.Text = "Ready: Initiate meter and then scanner"
                ElseIf Me.Device.ArmLayer1Subsystem.ArmSource.Value = Scpi.ArmSources.Immediate Then
                    Me._StatusLabel.Text = "Ready: Initiate scanner and then meter"
                Else
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Invalid arms source {Me.Device.ArmLayer1Subsystem.ArmSource.Value}")
                End If
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception occurred;. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: READ AND WRITE "

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub OnPropertyChanged(ByVal sender As Instrument.SimpleReadWriteControl, ByVal propertyName As String)
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
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._SimpleReadWriteControl_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, Instrument.SimpleReadWriteControl), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling READ/WIRTE '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
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

