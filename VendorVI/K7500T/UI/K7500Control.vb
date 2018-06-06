Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.CheckBoxExtensions
Imports isr.Core.Controls.NumericUpDownExtensions
Imports isr.Core.Controls.SafeSetterExtensions
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
<System.ComponentModel.DisplayName("K7500 Control"),
      System.ComponentModel.Description("Keithley 7500 Device Control"),
      System.Drawing.ToolboxBitmap(GetType(K7500Control))>
Public Class K7500Control
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
        With Me._StartTriggerDelayNumeric.NumericUpDownControl
            .DecimalPlaces = 3
            .Minimum = 0
            .Maximum = 10
            .Value = 0.02D
        End With
        With Me._EndTriggerDelayNumeric.NumericUpDownControl
            .DecimalPlaces = 3
            .Minimum = 0
            .Maximum = 10
            .Value = 0.0D
        End With
        With Me._OpenLeadsBitPatternNumeric.NumericUpDownControl
            .Minimum = 1
            .Maximum = 63
            .Value = 16
        End With
        With Me._PassBitPatternNumeric.NumericUpDownControl
            .Minimum = 1
            .Maximum = 63
            .Value = 32
        End With
        With Me._FailLimit1BitPatternNumeric.NumericUpDownControl
            .Minimum = 1
            .Maximum = 63
            .Value = 48
        End With
        With Me._LowerLimit1Numeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 5000000D
            .DecimalPlaces = 3
            .Value = 9
        End With
        With Me._UpperLimit1Numeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 5000000D
            .DecimalPlaces = 3
            .Value = 11
        End With
        With Me._TriggerCountNumeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 268000000D
            .DecimalPlaces = 0
            .Value = 10
        End With
        With Me._BufferSizeNumeric.NumericUpDownControl
            .CausesValidation = True
            .Minimum = 0
            .Maximum = 27500000
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
        AddHandler Me.Device.Buffer1Subsystem.PropertyChanged, AddressOf Me.BufferSubsystemPropertyChanged
        AddHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
        AddHandler Me.Device.LocalNodeSubsystem.PropertyChanged, AddressOf Me.LocalNodeSubsystemPropertyChanged
        AddHandler Me.Device.MultimeterSubsystem.PropertyChanged, AddressOf Me.MultimeterSubsystemPropertyChanged
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
            RemoveHandler Me.Device.Buffer1Subsystem.PropertyChanged, AddressOf Me.BufferSubsystemPropertyChanged
            RemoveHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
            RemoveHandler Me.Device.LocalNodeSubsystem.PropertyChanged, AddressOf Me.LocalNodeSubsystemPropertyChanged
            RemoveHandler Me.Device.MultimeterSubsystem.PropertyChanged, AddressOf Me.MultimeterSubsystemPropertyChanged
            RemoveHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

    ''' <summary> Device initialized. </summary>
    ''' <param name="sender"> <see cref="T:System.Object" /> instance of this
    '''                                          <see cref="T:System.Windows.Forms.Control" /> </param>
    ''' <param name="e">      Cancel event information. </param>
    Protected Overrides Sub DeviceInitialized(ByVal sender As VI.DeviceBase, ByVal e As System.EventArgs)
        MyBase.DeviceInitialized(sender, e)
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " BUFFER "

    ''' <summary> Buffer size text box validating. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031: DoNotCatchGeneralExceptionTypes")>
    Private Sub _BufferSizeNumeric_Validating(sender As Object, e As CancelEventArgs) Handles _BufferSizeNumeric.Validating
        Dim activity As String = $"{Me.Device.ResourceTitle} setting {NameOf(BufferSubsystem)}.{NameOf(BufferSubsystem.Capacity)}"
        Try
            If Me.Device.IsDeviceOpen Then
                Dim value As Integer = CInt(Me._BufferSizeNumeric.Value)
                activity = $"{Me.Device.ResourceTitle} setting {GetType(BufferSubsystem)}.{NameOf(BufferSubsystem.Capacity)} to {value}"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, activity)
                ' overrides and set to the minimum size: Me._BufferSizeNumeric.Value = Me._BufferSizeNumeric.NumericUpDownControl.Minimum
                Me.Device.Buffer1Subsystem.ApplyCapacity(CInt(value))
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Handle the Buffer subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As BufferSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K7500.BufferSubsystem.Capacity)
                Me._BufferSizeNumeric.Value = subsystem.Capacity.GetValueOrDefault(0)
                Me._BufferSizeNumeric.Invalidate()
            Case NameOf(K7500.BufferSubsystem.ActualPointCount)
                Me._BufferCountLabel.Text = CStr(subsystem.ActualPointCount.GetValueOrDefault(0))
                Me._BufferCountLabel.Invalidate()
            Case NameOf(K7500.BufferSubsystem.FirstPointNumber)
                Me._FirstPointNumberLabel.Text = CStr(subsystem.FirstPointNumber.GetValueOrDefault(0))
                Me._BufferCountLabel.Invalidate()
            Case NameOf(K7500.BufferSubsystem.LastPointNumber)
                Me._LastPointNumberLabel.Text = CStr(subsystem.LastPointNumber.GetValueOrDefault(0))
                Me._BufferCountLabel.Invalidate()
            Case NameOf(K7500.BufferSubsystem.BufferReadingsCount)
                If Me.BufferStreamingHandlerEnabled Then
                    ' must reconfigure = true to get the grid updated :(
                    If Me._BufferDataGridView.DataSource Is Nothing Then
                        subsystem.BufferReadings.DisplayReadings(Me._BufferDataGridView, True)
                        Windows.Forms.Application.DoEvents()
                        Me._BufferDataGridView.Invalidate()
                        Windows.Forms.Application.DoEvents()
                    Else
                        subsystem.BufferReadings.DisplayReadings(Me._BufferDataGridView, True)
                        'Me._BufferDataGridView.DataSource = subsystem.BufferReadings
                        'Me._BufferDataGridView.InvalidateRow(subsystem.BufferReadings.Count - 1)
                        Windows.Forms.Application.DoEvents()
                        Me._BufferDataGridView.Refresh()
                        Windows.Forms.Application.DoEvents()
                    End If
                    Me._ReadingToolStripStatusLabel.SafeTextSetter($"{subsystem.LastBufferReading.Reading} {Me.Device.MultimeterSubsystem.Readings.Reading.Unit.Symbol}")
                End If
        End Select
        Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary> Buffer subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031: DoNotCatchGeneralExceptionTypes")>
    Private Sub BufferSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(BufferSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.BufferSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, BufferSubsystem), e.PropertyName)
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

    ''' <summary> Handle the Display subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As DisplaySubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K7500.DisplaySubsystem.Enabled)
        End Select
    End Sub

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inDisplayion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplaySubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(DisplaySubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.DisplaySubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, DisplaySubsystem), e.PropertyName)
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

#Region " LOCAL NODE "

    ''' <summary> Handle the Local Node subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As LocalNodeSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K7500.LocalNodeSubsystem.IsDeviceOpen)
        End Select
    End Sub

    ''' <summary> Local Node subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event inLocalNodeion. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub LocalNodeSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(LocalNodeSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.LocalNodeSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, LocalNodeSubsystem), e.PropertyName)
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

#Region " MULTIMETER "

    ''' <summary> Displays a function modes. </summary>
    Private Sub DisplayFunctionModes(ByVal selectedValue As VI.Tsp2.MultimeterFunctionMode)
        Try
            Me.InitializingComponents = True
            With Me._MeasureFunctionComboBox
                If .DataSource Is Nothing OrElse .Items.Count <= 0 Then
                    .DataSource = Nothing
                    .Items.Clear()
                    .DataSource = GetType(VI.Tsp2.MultimeterFunctionMode).ValueDescriptionPairs()
                    .DisplayMember = "Value"
                    .ValueMember = "Key"
                End If
                If .Items.Count > 0 Then
                    .SelectedItem = selectedValue.ValueDescriptionPair
                End If
            End With
        Catch
            Throw
        Finally
            Me.InitializingComponents = False
        End Try
    End Sub

    ''' <summary>
    ''' Gets or sets the selected function mode.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private Function SelectedFunctionMode() As VI.Tsp2.MultimeterFunctionMode
        Dim activity As String = $"Selecting function mode {Me._MeasureFunctionComboBox.SelectedItem}"
        Dim result As VI.Tsp2.MultimeterFunctionMode = MultimeterFunctionMode.CurrentAC
        Try
            result = CType(CType(Me._MeasureFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                  Of [Enum], String)).Key, VI.Tsp2.MultimeterFunctionMode)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(Me._MeasureFunctionComboBox, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Selects a new function mode.
    ''' </summary>
    Friend Sub ApplyFunctionMode(ByVal value As VI.Tsp2.MultimeterFunctionMode)
        If Me.IsDeviceOpen Then Me._Device.MultimeterSubsystem.ApplyFunctionMode(value)
    End Sub

    ''' <summary> Handle the Multimeter subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As MultimeterSubsystem, ByVal propertyName As String)
        If Me.InitializingComponents OrElse subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._MultimeterRangeTextBox.SafeTextSetter(Me.Device.MultimeterRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.MultimeterIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.AutoDelayEnabled)
                ' If subsystem.AutoDelayEnabled.HasValue Then Me._MeasureAutoRangeToggle.checked = subsystem.AutoDelayEnabeld.Value
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.AutoRangeEnabled)
                If subsystem.AutoRangeEnabled.HasValue Then Me._MeasureAutoRangeToggle.Checked = subsystem.AutoRangeEnabled.Value
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.AutoZeroEnabled)
                ' If subsystem.AutoZeroEnabled.HasValue Then Me._MeasureAutoRangeToggle.Checked = subsystem.AutoZeroEnabled.Value

            Case NameOf(K7500.MultimeterSubsystem.FailureCode)
                Me._FailureToolStripStatusLabel.Text = subsystem.FailureCode
            Case NameOf(K7500.MultimeterSubsystem.FailureLongDescription)
                Me._FailureToolStripStatusLabel.ToolTipText = subsystem.FailureLongDescription

#If False Then
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.FilterCount)
                If subsystem.FilterCount.HasValue Then Me._FilterCountNumeric.Value = subsystem.FilterCount.Value
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.FilterCountRange)
                Me._FilterCountNumeric.RangeSetter(subsystem.FilterCountRange)
                Me._FilterCountNumeric.DecimalPlaces = 0
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.FilterWindowRange)
                Me._FilterWindowNumeric.RangeSetter(subsystem.FilterWindowRange.TransposedRange(0, 100))
                Me._FilterWindowNumeric.DecimalPlaces = 0
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.MovingAverageFilterEnabled)
                If subsystem.MovingAverageFilterEnabled.HasValue Then Me._MovingAverageRadioButton.Checked = subsystem.MovingAverageFilterEnabled.Value
                If subsystem.MovingAverageFilterEnabled.HasValue Then Me._RepeatingAverageRadioButton.Checked = Not subsystem.MovingAverageFilterEnabled.Value
#End If

            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.FrontTerminalsSelected)
                Me._ReadTerminalStateButton.Checked = subsystem.FrontTerminalsSelected.GetValueOrDefault(False)
                Windows.Forms.Application.DoEvents()

            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.FunctionMode)
                Dim value As VI.Tsp2.MultimeterFunctionMode = subsystem.FunctionMode.GetValueOrDefault(VI.Tsp2.MultimeterFunctionMode.VoltageDC)
                If Me._MeasureFunctionComboBox.DataSource Is Nothing Then
                    Me.DisplayFunctionModes(value)
                Else
                    Me._MeasureFunctionComboBox.SelectedItem = subsystem.FunctionMode.GetValueOrDefault(VI.Tsp2.MultimeterFunctionMode.VoltageDC).ValueDescriptionPair()
                End If
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.FunctionRange)
                Me._MeasureRangeNumeric.RangeSetter(subsystem.FunctionRange.Min, subsystem.FunctionRange.Max)
            Case NameOf(K7500.MultimeterSubsystem.FunctionRangeDecimalPlaces)
                Me._MeasureRangeNumeric.DecimalPlaces = subsystem.FunctionRangeDecimalPlaces
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.FunctionUnit)
                subsystem.Readings.Reading.Unit = subsystem.FunctionUnit
                Me._MeasureRangeNumericLabel.Text = $"Range [{subsystem.FunctionUnit}]:"
                Me._MeasureRangeNumericLabel.Left = Me._MeasureRangeNumericLabel.Left - Me._MeasureRangeNumericLabel.Width

            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.LastReading)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = If(String.IsNullOrWhiteSpace(value), "<last reading>", value)
                subsystem.LastActionElapsedTime = Me.ReadElapsedTime(True)
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.LastActionElapsedTime)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = $"{If(String.IsNullOrWhiteSpace(value), "<last reading>", value)} @{subsystem.LastActionElapsedTime.ToExactMilliseconds:0.0}ms"

            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.OpenDetectorEnabled)
                If subsystem.OpenDetectorEnabled.HasValue Then Me._OpenLeadsDetectionCheckBox.Checked = subsystem.OpenDetectorEnabled.Value

            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.PowerLineCycles)
                If subsystem.PowerLineCycles.HasValue Then Me._PowerLineCyclesNumeric.Value = CDec(subsystem.PowerLineCycles.Value)
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.PowerLineCyclesRange)
                Me._PowerLineCyclesNumeric.RangeSetter(subsystem.PowerLineCyclesRange)
                Me._PowerLineCyclesNumeric.DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces
            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.PowerLineCyclesDecimalPlaces)
                Me._PowerLineCyclesNumeric.DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces

            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.Range)
                If subsystem.Range.HasValue Then Me._MeasureRangeNumeric.ValueSetter(subsystem.Range.Value)

            Case NameOf(VI.Tsp2.MultimeterSubsystemBase.ReadingCaption)
                Me._ReadingToolStripStatusLabel.Text = subsystem.ReadingCaption
            Case NameOf(K7500.MultimeterSubsystem.Readings)
                Try
                    Me.InitializingComponents = True
                    If Me._ReadingComboBox.ComboBox.DataSource Is Nothing Then
                        subsystem.Readings.ListElements(Me._ReadingComboBox.ComboBox, ReadingTypes.Units)
                    End If
                Catch
                    Throw
                Finally
                    Me.InitializingComponents = False
                End Try
        End Select
    End Sub

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MultimeterSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(MultimeterSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.MultimeterSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, MultimeterSubsystem), e.PropertyName)
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
            Case NameOf(K7500.TriggerSubsystem.TriggerCount)
                If subsystem.TriggerCount.HasValue Then
                    Me._TriggerCountNumeric.Value = subsystem.TriggerCount.Value
                End If
            Case NameOf(K7500.TriggerSubsystem.ContinuousEnabled)
                Me._ContinuousTriggerEnabledMenuItem.CheckState = subsystem.ContinuousEnabled.ToCheckState
            Case NameOf(K7500.TriggerSubsystem.TriggerSource)
                If subsystem.TriggerSource.HasValue AndAlso Me._TriggerSourceComboBox.ComboBox.Items.Count > 0 Then
                    Me._TriggerSourceComboBox.ComboBox.SelectedItem = subsystem.TriggerSource.Value.ValueNamePair
                End If
            Case NameOf(K7500.TriggerSubsystem.SupportedTriggerSources)
                subsystem.ListSupportedTriggerSources(Me._TriggerSourceComboBox.ComboBox)
                If subsystem.TriggerSource.HasValue AndAlso Me._TriggerSourceComboBox.ComboBox.Items.Count > 0 Then
                    Me._TriggerSourceComboBox.ComboBox.SelectedItem = subsystem.TriggerSource.Value.ValueNamePair
                End If
            Case NameOf(K7500.TriggerSubsystem.TriggerState)
                Me._TriggerStateLabel.Visible = subsystem.TriggerState.HasValue
                If subsystem.TriggerState.HasValue Then
                    Me._TriggerStateLabel.Text = subsystem.TriggerState.Value.ToString
                    If Me.TriggerPlanStateChangeHandlerEnabled Then
                        Me.HandleTriggerPlanStateChange(subsystem.TriggerState.Value)
                    End If
                    If Me.BufferStreamingHandlerEnabled AndAlso Not subsystem.IsTriggerStateActive AndAlso _StreamBufferMenuItem.Checked Then
                        Me._StreamBufferMenuItem.Checked = False
                    End If
                End If
                ' ?? this causes a cross thread exception. 
                ' Me._TriggerStateLabel.Invalidate()
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

#Region " STATUS "

    ''' <summary> Displays the last error. </summary>
    ''' <param name="lastError"> The last error. </param>
    Protected Overrides Sub DisplayLastError(ByVal lastError As VI.DeviceError)
        VI.Instrument.ResourceControlBase.DisplayLastError(Me._LastErrorTextBox, lastError)
    End Sub

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub HandlePropertyChange(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.HandlePropertyChange(subsystem, propertyName)
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
            Case NameOf(K7500.SystemSubsystem.LanguageRevision)
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

#Region " CONTROL EVENT HANDLERS: TERMINALS "

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadTerminalStateButton_Click(sender As Object, e As EventArgs) Handles _ReadTerminalStateButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Reading terminals state"
        Dim button As ToolStripButton = TryCast(sender, ToolStripButton)
        Try
            If button IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me._InfoProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.MultimeterSubsystem.QueryFrontTerminalsSelected()
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

    ''' <summary> Reading combo box selected value changed. </summary>
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
            Me.Device.MultimeterSubsystem.SelectActiveReading(Me.SelectedReadingType)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _ReadButton for click events. Query the Device for a
    ''' reading. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadButton.Click
        Dim activity As String = "querying terminal mode"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.StartElapsedStopwatch()
            Me.Device.MultimeterSubsystem.QueryFrontTerminalsSelected()
            Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            activity = "measuring"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            ' elapsed time is displayed upon completion of reading.
            Me.StartElapsedStopwatch()
            Me.Device.MultimeterSubsystem.Measure()
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
    Private Sub _AutoInitiateMenuItem_Click(sender As Object, e As EventArgs)
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
    Private ReadOnly Property TraceReadings As VI.BufferReadingCollection

#Region " STREAM BUFFER "

    ''' <summary> Gets or sets the buffer streaming handler enabled. </summary>
    ''' <value> The buffer streaming handler enabled. </value>
    Private Property BufferStreamingHandlerEnabled As Boolean

    ''' <summary> Stream buffer menu item check state changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _StreamBufferMenuItem_CheckStateChanged(sender As Object, e As EventArgs) Handles _StreamBufferMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "start buffer streaming"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If _StreamBufferMenuItem.Checked Then
                Me.BufferStreamingHandlerEnabled = False
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.StartElapsedStopwatch()
                Me._BufferDataGridView.DataSource = Nothing
                Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
                Me.Device.TriggerSubsystem.CaptureSyncContext(Me.CapturedSyncContext)
                Me.Device.Buffer1Subsystem.CaptureSyncContext(Me.CapturedSyncContext)
                Me.Device.TriggerSubsystem.Initiate()
                Windows.Forms.Application.DoEvents()
                Me.Device.Buffer1Subsystem.StreamBufferAsync(Me.CapturedSyncContext, Me.Device.TriggerSubsystem, TimeSpan.FromMilliseconds(5))
                Me.BufferStreamingHandlerEnabled = True
            Else
                Me.BufferStreamingHandlerEnabled = False
                activity = "Aborting trigger plan to stop buffer streaming"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.AbortTriggerPlan(sender)
                Me.Device.TriggerSubsystem.QueryTriggerState()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub


#End Region

#Region " INITIATE AND WAIT "

    Private Enum TriggerPlanState
        None
        Started
        Completed
    End Enum

    Private Property TriggerPlanStateChangeHandlerEnabled As Boolean

    Private Property LocalTriggerPlanState As TriggerPlanState

    ''' <summary> Handles the trigger plan state change described by triggerState. </summary>
    ''' <param name="triggerState"> State of the trigger. </param>
    Private Sub HandleTriggerPlanStateChange(ByVal triggerState As VI.TriggerState)
        If triggerState = TriggerState.Running OrElse triggerState = TriggerState.Waiting Then
            LocalTriggerPlanState = TriggerPlanState.Started
        ElseIf triggerState = TriggerState.Idle AndAlso LocalTriggerPlanState = TriggerPlanState.Started Then
            LocalTriggerPlanState = TriggerPlanState.Completed
            Me.TryReadBuffer()
            Me.TryDisplayBuffer()
            If Me._RepeatMenuItem.Checked Then
                Me.InitiateMonitorTriggerPlan(True)
            End If
        Else
            LocalTriggerPlanState = TriggerPlanState.None
        End If
        Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
    End Sub

    ''' <summary> Try read buffer. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TryReadBuffer()
        Dim activity As String = "reading"
        Try
            Me.ReadBuffer()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
        End Try
    End Sub

    ''' <summary> Reads the buffer. </summary>
    Private Sub ReadBuffer()
        Dim activity As String = "fetching buffer count"
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
        ' this assume buffer is cleared upon each new cycle
        Dim newBufferCount As Integer = Me.Device.Buffer1Subsystem.QueryActualPointCount.GetValueOrDefault(0)
        activity = $"buffer count {newBufferCount}"
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")

        If newBufferCount > 0 Then
            activity = "fetching buffered readings"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Dim values As IEnumerable(Of BufferReading) = Me.Device.Buffer1Subsystem.QueryBufferReadings()
            If Me.TraceReadings Is Nothing Then Me._TraceReadings = New VI.BufferReadingCollection
            Me.TraceReadings.Add(values)
        End If
        Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
        Windows.Forms.Application.DoEvents()
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TryDisplayBuffer()
        Dim activity As String = "displaying"
        Try
            Me.DisplayBuffer()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
        End Try
    End Sub

    ''' <summary> Displays a buffer. </summary>
    Private Sub DisplayBuffer(ByVal readings As VI.BufferReadingCollection)
        Dim activity As String = "updating the display"
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
        readings.DisplayReadings(Me._BufferDataGridView, True)
        Windows.Forms.Application.DoEvents()
        Me._BufferDataGridView.Invalidate()
    End Sub

    Private Sub DisplayBuffer()
        Dim activity As String = "updating the display"
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
        Me.TraceReadings.DisplayReadings(Me._BufferDataGridView, True)
        Windows.Forms.Application.DoEvents()
        Me._BufferDataGridView.Invalidate()
    End Sub

    ''' <summary> Initiate monitor trigger plan. </summary>
    ''' <param name="stateChangeHandlingEnabled"> True to enable, false to disable the state change
    '''                                           handling. </param>
    Private Sub InitiateMonitorTriggerPlan(ByVal stateChangeHandlingEnabled As Boolean)
        Dim activity As String = "Initiating trigger plan and monitor"
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
        Me.StartElapsedStopwatch()
        Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
        Me.Device.TriggerSubsystem.Initiate()
        Windows.Forms.Application.DoEvents()
        Me.TriggerPlanStateChangeHandlerEnabled = stateChangeHandlingEnabled
        Me.Device.TriggerSubsystem.CaptureSyncContext(Me.CapturedSyncContext)
        Me.Device.TriggerSubsystem.AsyncMonitorTriggerState(Me.CapturedSyncContext, TimeSpan.FromMilliseconds(5))
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TryRestartTriggerPlan()
        Dim activity As String = "Initiating trigger plan and monitor"
        Try
            Me.InitiateMonitorTriggerPlan(True)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MonitorActiveTriggerPlanMenuItem_Click(sender As Object, e As EventArgs) Handles _MonitorActiveTriggerPlanMenuItem.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "start monitoring trigger plan"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.TriggerPlanStateChangeHandlerEnabled = False
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.StartElapsedStopwatch()
            Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
            Me.Device.TriggerSubsystem.CaptureSyncContext(Me.CapturedSyncContext)
            Me.Device.TriggerSubsystem.AsyncMonitorTriggerState(Me.CapturedSyncContext, TimeSpan.FromMilliseconds(5))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitMonitorReadRepeatMenuItem_Click(sender As Object, e As EventArgs) Handles _InitMonitorReadRepeatMenuItem.Click
        Dim activity As String = "Initiating trigger plan and monitor"
        Try
            Me.InitiateMonitorTriggerPlan(True)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
        End Try
    End Sub

#End Region

#Region " TRIGGER PLAN EVENT HANDLING "

    ''' <summary> Handles the measurement completed request. </summary>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub HandleMeasurementCompletedRequest(sender As Object, e As EventArgs)
        Dim activity As String = "handling service request event"
        Try
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} SRQ: {Me.Device.StatusSubsystem.ServiceRequestStatus:X};. ")
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
#If False Then
            ' Measurement bit does not turn on -- kludging for now.
            If Me.Device.StatusSubsystem.MeasurementAvailable Then
            Else
                activity = "measurement not available"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            End If
#End If
            activity = "kludge: reading buffer count"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")

            ' this assume buffer is cleared upon each new cycle
            Dim newBufferCount As Integer = Me.Device.Buffer1Subsystem.QueryActualPointCount.GetValueOrDefault(0)

            If newBufferCount > 0 Then

                activity = "kludge: buffer has data..."
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")

                activity = "handling measurement available"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")

                If False Then
                    activity = "fetching a single reading"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    Me.Device.MultimeterSubsystem.MeasureValue()
                Else
                    activity = "fetching buffered readings"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    Dim values As IEnumerable(Of BufferReading) = Me.Device.Buffer1Subsystem.QueryBufferReadings()
                    If Me.TraceReadings Is Nothing Then Me._TraceReadings = New VI.BufferReadingCollection
                    For Each v As BufferReading In values : Me.TraceReadings.Add(v) : Next

                    activity = "updating the display"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    If Me.TraceReadings.Count = values.Count Then
                        Me.TraceReadings.DisplayReadings(Me._BufferDataGridView, False)
                    Else
                        ' TO_DO: See if observable collection will work.
                        ' Me._BufferDataGridView.Invalidate()
                        Me.TraceReadings.DisplayReadings(Me._BufferDataGridView, False)
                    End If
                    Me._BufferDataGridView.Invalidate()
                    Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
                End If
                If Me._RepeatMenuItem.Checked Then
                    activity = "initiating next measurement(s)"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    Me.StartElapsedStopwatch()
                    Me.Device.Buffer1Subsystem.ClearBuffer() ' ?@3 removed 7/6/17
                    Me.Device.TriggerSubsystem.Initiate()
                End If
            Else
                activity = "trigger plan started; buffer empty"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
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
            Me.Device.StatusSubsystem.ApplyQuestionableEventEnableBitmask(MeasurementEvents.All)
            ' 
            ' if handling buffer full, use the 4917 event to detect buffer full. 

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
            Me.Device.StatusSubsystem.ApplyQuestionableEventEnableBitmask(MeasurementEvents.None)

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
    Private Sub _HandleMeasurementEventMenuItem_CheckStateChanged(sender As Object, e As EventArgs)

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

#End Region

#Region " BUFFER HANDLER "

    ''' <summary> Handles the buffer full request. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub HandleBufferFullRequest(sender As Object, e As EventArgs)
        Dim activity As String = "handling service request event"
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} SRQ: {Me.Device.StatusSubsystem.ServiceRequestStatus:X2};. ")
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If Me.Device.StatusSubsystem.OperationCompleted Then

                activity = "handling operation completed"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")

                ' TO_DO: See if can only do a set condition and not read this.
                Dim condition As Integer = Me.Device.StatusSubsystem.QueryOperationEventCondition().GetValueOrDefault(0)
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} OPER: {condition:X2};. ")

                ' If Bit 0 Is set then the buffer is full
                If (condition And (1 << Me.BufferFullOperationConditionBitNumber)) <> 0 Then
                    activity = "handling buffer full"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")

                    activity = "fetching buffered readings"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    Dim values As IEnumerable(Of BufferReading) = Me.Device.Buffer1Subsystem.QueryBufferReadings()
                    If Me.TraceReadings Is Nothing Then Me._TraceReadings = New VI.BufferReadingCollection
                    For Each v As BufferReading In values : Me.TraceReadings.Add(v) : Next

                    activity = "updating the display"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    If Me.TraceReadings.Count = values.Count Then
                        Me.TraceReadings.DisplayReadings(Me._BufferDataGridView, True)
                    Else
                        Me._BufferDataGridView.Invalidate()
                    End If
                    Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
                    If Me._RepeatMenuItem.Checked Then
                        activity = "initiating next measurement(s)"
                        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                        Me.StartElapsedStopwatch()
                        Me.Device.Buffer1Subsystem.ClearBuffer() ' ?@# removed 7/6/17
                        Me.Device.TriggerSubsystem.Initiate()
                    End If
                Else
                    activity = "handling buffer clear: NOP"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                End If
            Else
                activity = "operation not completed"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(Me._ReadBufferButton, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Gets the Buffer Full handler added. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <value> The Buffer Full handler added. </value>
    Private Property BufferFullHandlerAdded As Boolean

    ''' <summary> Gets the buffer full operation condition bit number. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <value> The buffer full operation condition bit number. </value>
    Private Property BufferFullOperationConditionBitNumber As Integer

    ''' <summary> Adds Buffer Full event handler. </summary>
    Private Sub AddBufferFullEventHandler()

        Dim activity As String = ""
        If Not Me.BufferFullHandlerAdded Then

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

            activity = "Turning on Buffer events"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.BufferFullOperationConditionBitNumber = 0
            Me.Device.StatusSubsystem.ApplyOperationEventMap(Me.BufferFullOperationConditionBitNumber, Me.Device.StatusSubsystem.BufferFullEventNumber, Me.Device.StatusSubsystem.BufferEmptyEventNumber)
            Me.Device.StatusSubsystem.ApplyOperationEventEnableBitmask(1 << Me.BufferFullOperationConditionBitNumber)

            activity = "Turning on status service request"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            ' Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.BufferEvent)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All And Not VI.Pith.ServiceRequests.MessageAvailable)

            activity = "Adding re-triggering event handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            AddHandler Me.Device.ServiceRequested, AddressOf HandleBufferFullRequest
            Me.BufferFullHandlerAdded = True
        End If
    End Sub

    ''' <summary> Removes the Buffer Full event handler. </summary>
    Private Sub RemoveBufferFullEventHandler()

        Dim activity As String = ""
        If Me.BufferFullHandlerAdded Then

            activity = "Disabling session service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.Session.DisableServiceRequest()

            activity = "Removing device service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.RemoveServiceRequestEventHandler()

            activity = "Turning off Buffer events"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.BufferFullOperationConditionBitNumber = 0
            Me.Device.StatusSubsystem.ApplyOperationEventMap(Me.BufferFullOperationConditionBitNumber, 0, 0)
            Me.Device.StatusSubsystem.ApplyOperationEventEnableBitmask(0)

            activity = "Turning off status service request"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.None)

            activity = "Removing re-triggering event handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            RemoveHandler Me.Device.ServiceRequested, AddressOf HandleBufferFullRequest

            Me.BufferFullHandlerAdded = False

        End If
    End Sub

    ''' <summary>
    ''' Event handler. Called by _HandleBufferEventMenuItem for check state changed events.
    ''' </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _HandleBufferEventMenuItem_CheckStateChanged(sender As Object, e As EventArgs)

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

                activity = "Adding Buffer completion handler"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.AddBufferFullEventHandler()

            Else

                activity = "Removing Buffer completion handler"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.RemoveBufferFullEventHandler()

            End If

        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

#Region " TRIGGER CONTROLS ON THE READING PANEL "

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
            Me._TraceReadings = New VI.BufferReadingCollection
            Me._TraceReadings.DisplayReadings(Me._BufferDataGridView, True)

            Me.Device.Buffer1Subsystem.ClearBuffer()

            activity = "initiating trigger plan"
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
    Private Sub _AbortStartTriggerPlanMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _AbortStartTriggerPlanMenuItem.Click

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

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitiateTriggerPlanMenuItem_Click(sender As Object, e As EventArgs) Handles _InitiateTriggerPlanMenuItem.Click

        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            activity = "initiating trigger plan"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.StartElapsedStopwatch()
            Me.Device.TriggerSubsystem.Initiate()
            Me.Device.TriggerSubsystem.QueryTriggerState()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AbortButton_Click(sender As Object, e As EventArgs) Handles _AbortButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "aborting trigger plan"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.AbortTriggerPlan(sender)
            Me.Device.TriggerSubsystem.QueryTriggerState()
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

#Region " BUFFER "

    ''' <summary>
    ''' Handles the DataError event of the _dataGridView control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="DataGridViewDataErrorEventArgs"/> instance containing the event data.</param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _BufferDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles _BufferDataGridView.DataError
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
    Private Sub _ReadBufferButton_Click(sender As Object, e As EventArgs) Handles _ReadBufferButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "reading buffer"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.StartElapsedStopwatch()
            Dim br As New VI.BufferReadingCollection From {
                Me.Device.Buffer1Subsystem.QueryBufferReadings
            }
            br.DisplayReadings(Me._BufferDataGridView, True)
            Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
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
            Dim br As New VI.BufferReadingCollection
            br.DisplayReadings(Me._BufferDataGridView, True)
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

#Region " CONTROL EVENT HANDLERS: MEASURE "

    ''' <summary> Applies the function mode button click. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyFunctionModeButton_Click(sender As Object, e As EventArgs) Handles _ApplyFunctionModeButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.ApplyFunctionMode(Me.SelectedFunctionMode)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{ex.Message} occurred initiating a measurement;. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Applies the selected measurements settings.
    ''' </summary>
    Private Sub ApplyMeasureSettings()

        With Me.Device.MultimeterSubsystem

            If Me._OpenLeadsDetectionCheckBox.Enabled AndAlso Not Nullable.Equals(.OpenDetectorEnabled, Me._OpenLeadsDetectionCheckBox.Checked) Then
                .ApplyOpenDetectorEnabled(Me._OpenLeadsDetectionCheckBox.Checked)
            End If

            If Not Nullable.Equals(.PowerLineCycles, Me._PowerLineCyclesNumeric.Value) Then
                .ApplyPowerLineCycles(Me._PowerLineCyclesNumeric.Value)
            End If

            If Not Nullable.Equals(.AutoRangeEnabled, Me._MeasureAutoRangeToggle.Checked) Then
                .ApplyAutoRangeEnabled(Me._MeasureAutoRangeToggle.Checked)
            End If

            If .AutoRangeEnabled Then
                .QueryRange()
            ElseIf Not Nullable.Equals(.Range, Me._MeasureRangeNumeric.Value) Then
                .ApplyRange(Me._MeasureRangeNumeric.Value)
            End If

        End With

    End Sub

    ''' <summary> Event handler. Called by ApplyMeasureSettingsButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyMeasureSettingsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ApplyMeasureSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.ApplyMeasureSettings()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{ex.Message} occurred initiating a measurement;. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: TRIGGER "

    Private Sub _FailBitPatternNumericButton_CheckStateChanged(sender As Object, e As EventArgs) Handles _FailBitPatternNumericButton.CheckStateChanged
        Me._FailLimit1BitPatternNumeric.NumericUpDownControl.Hexadecimal = Me._FailBitPatternNumericButton.Checked
        Me._FailBitPatternNumericButton.Text = $"Fail {If(_FailBitPatternNumericButton.Checked, "0x", "0d")}"
    End Sub

    Private Sub _PassBitToolStripLabel_CheckStateChanged(sender As Object, e As EventArgs) Handles _PassBitPatternNumericButton.CheckStateChanged
        Me._PassBitPatternNumeric.NumericUpDownControl.Hexadecimal = Me._PassBitPatternNumericButton.Checked
        Me._PassBitPatternNumericButton.Text = $"Pass {If(_FailBitPatternNumericButton.Checked, "0x", "0d")}"
    End Sub

    Private Sub _OpenLeadsBitToolStripLabel_CheckStateChanged(sender As Object, e As EventArgs) Handles _OpenLeadsBitPatternNumericButton.CheckStateChanged
        Me._OpenLeadsBitPatternNumeric.NumericUpDownControl.Hexadecimal = Me._OpenLeadsBitPatternNumericButton.Checked
        Me._OpenLeadsBitPatternNumeric.Text = $"Open {If(_FailBitPatternNumericButton.Checked, "0x", "0d")}"
    End Sub

    Private Sub _Limit1DecimalsNumeric_ValueChanged(sender As Object, e As EventArgs) Handles _Limit1DecimalsNumeric.ValueChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me._LowerLimit1Numeric.NumericUpDownControl.DecimalPlaces = CInt(Me._Limit1DecimalsNumeric.Value)
        Me._UpperLimit1Numeric.NumericUpDownControl.DecimalPlaces = CInt(Me._Limit1DecimalsNumeric.Value)
    End Sub

    ''' <summary> Gets the selected trigger source. </summary>
    ''' <value> The selected trigger source. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedTriggerSource() As Scpi.TriggerSources
        Get
            Return CType(CType(Me._TriggerSourceComboBox.ComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, Scpi.TriggerSources)
        End Get
    End Property

    ''' <summary> Prepare grade binning model. </summary>
    Private Sub PrepareGradeBinningModel()

        Me.ApplyFunctionMode(VI.Tsp2.MultimeterFunctionMode.ResistanceFourWire)

        If Me._LowerLimit1Numeric.Value <= 100 Then
            With Device.MultimeterSubsystem
                .ApplyFilterEnabled(True)
                .ApplyFilterCount(10)
                ' use repeat filter
                .ApplyMovingAverageFilterEnabled(False)
                .ApplyFilterWindow(0.1)
            End With
        Else
            With Device.MultimeterSubsystem
                .ApplyFilterEnabled(False)
            End With
        End If

        With Device.MultimeterSubsystem
            .ApplyLimit1AutoClear(True)
            .ApplyLimit1Enabled(True)
            .ApplyLimit1LowerLevel(Me._LowerLimit1Numeric.Value)
            .ApplyLimit1UpperLevel(Me._UpperLimit1Numeric.Value)
        End With

        ' set limits for open circuit to 10 times the range limit
        With Device.MultimeterSubsystem
            .ApplyLimit2AutoClear(True)
            .ApplyLimit2Enabled(True)
            .ApplyLimit2LowerLevel(-10 * Me._UpperLimit1Numeric.Value)
            .ApplyLimit2UpperLevel(10 * Me._UpperLimit1Numeric.Value)
        End With

        ' enable open detection
        With Device.MultimeterSubsystem
            .ApplyOpenDetectorEnabled(True)
        End With

        Dim count As Integer = CInt(Me._TriggerCountNumeric.Value)
        ' the buffer must have at least 10 data points
        Me.Device.Buffer1Subsystem.ApplyCapacity(Math.Max(10, count))

        ' clear the buffer 
        Me.Device.Buffer1Subsystem.ClearBuffer()

    End Sub

    ''' <summary> Loads grade bin trigger model button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _LoadGradeBinTriggerModelButton_Click(sender As Object, e As EventArgs) Handles _LoadGradeBinTriggerModelMenuItem.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "loading grade binning trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.PrepareGradeBinningModel()
                Me.Device.TriggerSubsystem.ApplyGradeBinning(CInt(Me._TriggerCountNumeric.Value),
                                                             TimeSpan.FromSeconds(Me._StartTriggerDelayNumeric.Value),
                                                             CInt(Me._FailLimit1BitPatternNumeric.Value),
                                                             CInt(Me._PassBitPatternNumeric.Value), CInt(Me._OpenLeadsBitPatternNumeric.Value),
                                                             Me.SelectedTriggerSource)
                Me.Device.TriggerSubsystem.QueryTriggerState()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Simple loop load run button click. </summary>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _LoadSimpleLoopModelButton_Click(sender As Object, e As EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "loading simple loop trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            If Me.IsDeviceOpen Then
                Dim count As Integer = CInt(Me._TriggerCountNumeric.Value)
                Dim startDelay As TimeSpan = TimeSpan.FromSeconds(Me._StartTriggerDelayNumeric.Value)
                Me.Device.TriggerSubsystem.LoadSimpleLoop(count, startDelay)
                Me.Device.TriggerSubsystem.QueryTriggerState()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _RunSimpleLoopTriggerModelButton_Click(sender As Object, e As EventArgs) Handles _RunSimpleLoopMenuItem.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Initiating simple loop trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Dim br As New VI.BufferReadingCollection : br.DisplayReadings(Me._BufferDataGridView, True)
            Me.Device.Buffer1Subsystem.ClearBuffer()
            Me.StartElapsedStopwatch()
            Me.Device.TriggerSubsystem.Initiate()
            Me.Device.StatusSubsystem.Wait()
            br.Add(Me.Device.Buffer1Subsystem.QueryBufferReadings)
            br.DisplayReadings(Me._BufferDataGridView, False)
            Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearTriggerModelMenuItem_Click(sender As Object, e As EventArgs) Handles _ClearTriggerModelMenuItem.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "clearing the trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.StartElapsedStopwatch()
            Me.Device.TriggerSubsystem.Abort()
            Me.Device.TriggerSubsystem.ClearTriggerModel()
            Me.Device.StatusSubsystem.Wait()
            Me._TimingLabel.Text = Me.ReadElapsedTime.ToString("s\.ffff")
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadTriggerStateMenuItem_Click(sender As Object, e As EventArgs) Handles _ReadTriggerStateMenuItem.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "reading trigger state"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            ' Me.Device.StatusSubsystem.Wait()
            Me.Device.TriggerSubsystem.QueryTriggerState()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MeterCompleterFirstGradingBinningMenuItem_Click(sender As Object, e As EventArgs) Handles _MeterCompleterFirstGradingBinningMenuItem.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "loading meter complete first grade binning trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.PrepareGradeBinningModel()
                Me.Device.TriggerSubsystem.ApplyMeterCompleteFirstGradeBinning(CInt(Me._TriggerCountNumeric.Value),
                                                             TimeSpan.FromSeconds(Me._StartTriggerDelayNumeric.Value),
                                                             CInt(Me._FailLimit1BitPatternNumeric.Value),
                                                             CInt(Me._PassBitPatternNumeric.Value), CInt(Me._OpenLeadsBitPatternNumeric.Value),
                                                             Me.SelectedTriggerSource)
                Me.Device.TriggerSubsystem.QueryTriggerState()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
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
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
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
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
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
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
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
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
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
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
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
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
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
        Dim combo As Core.Controls.ToolStripComboBox = TryCast(sender, Core.Controls.ToolStripComboBox)
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

#Region " READ AND WRITE "

    ''' <summary> Handles the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChanged(ByVal sender As Instrument.SimpleReadWriteControl, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(Instrument.SimpleReadWriteControl.StatusMessage)
                    Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusMessage, Me._StatusLabel)
                    Me._StatusLabel.ToolTipText = sender.StatusMessage
                Case NameOf(Instrument.SimpleReadWriteControl.ServiceRequestValue)
                    Me.DisplayStatusRegisterStatus(sender.ServiceRequestValue)
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
                Me.HandlePropertyChanged(TryCast(sender, Instrument.SimpleReadWriteControl), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling Read/Write '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
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
        Me._SimpleReadWriteControl?.ApplyListenerTraceLevel(listenerType, value)
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

