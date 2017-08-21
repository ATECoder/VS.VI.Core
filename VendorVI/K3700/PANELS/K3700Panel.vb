Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.Core.Pith.ExceptionExtensions
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
<System.ComponentModel.DisplayName("K3700 Panel"),
      System.ComponentModel.Description("Keithley 3700 Device Panel"),
      System.Drawing.ToolboxBitmap(GetType(K3700Panel))>
Public Class K3700Panel
    Inherits VI.Instrument.ResourcePanelBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private _InitializingComponents As Boolean
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
        Me._InitializingComponents = True
        Me.InitializeComponent()
        Me._InitializingComponents = False
        Me._AssignDevice(device)
        With Me.TraceMessagesBox
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
            Me.TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
            Me._ReadingComboBox.Visible = False
            Me._InitiateButton.Visible = False
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

    ''' <value> Gets a reference to the Keithley 3700 device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overloads ReadOnly Property Device() As Device

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
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

    ''' <summary> Handles the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnDevicePropertyChanged(ByVal device As DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnDevicePropertyChanged(device, propertyName)
        Select Case propertyName
            Case NameOf(device.SessionServiceRequestEventEnabled)
                Me._SessionServiceRequestHandlerEnabledMenuItem.Checked = device.SessionServiceRequestEventEnabled
            Case NameOf(device.DeviceServiceRequestHandlerAdded)
                Me._DeviceServiceRequestHandlerEnabledMenuItem.Checked = device.DeviceServiceRequestHandlerAdded
            Case NameOf(device.SessionMessagesTraceEnabled)
                Me._SessionTraceEnabledMenuItem.Checked = device.SessionMessagesTraceEnabled
            Case NameOf(device.ServiceRequestEnableBitmask)
                Me._ServiceRequestEnableBitmaskNumeric.Value = device.ServiceRequestEnableBitmask
                Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = $"SRE:0b{Convert.ToString(device.ServiceRequestEnableBitmask, 2),8}".Replace(" ", "0")
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        AddHandler Me.Device.MultimeterSubsystem.PropertyChanged, AddressOf Me.MultimeterSubsystemPropertyChanged
        AddHandler Me.Device.ChannelSubsystem.PropertyChanged, AddressOf Me.ChannelSubsystemPropertyChanged
        AddHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        MyBase.DeviceOpened(sender, e)
    End Sub

    ''' <summary> Device initialized. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceInitialized(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            MyBase.DeviceInitialized(sender, e)
            Me.OnSusystemInitialized(Me.Device.MultimeterSubsystem)
            Me.ReadServiceRequestStatus()
        Catch
            Throw
        Finally
        End Try
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overrides Sub OnTitleChanged(ByVal value As String)
        Me._TitleLabel.Text = value
        Me._TitleLabel.Visible = Not String.IsNullOrWhiteSpace(value)
        If String.IsNullOrWhiteSpace(value) Then
            Stop
        End If
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

    ''' <summary> Displays a reading described by subsystem. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub DisplayReading(ByVal subsystem As MultimeterSubsystem)

        Const clear As String = "    "
        Const compliance As String = "..C.."
        Const rangeCompliance As String = ".RC."
        Const levelCompliance As String = ".LC."

        If Not subsystem.Reading.HasValue Then
            Me._ReadingToolStripStatusLabel.Text = "-.------- V"
            Me._ComplianceToolStripStatusLabel.Text = clear
            Me._TbdToolStripStatusLabel.Text = clear
            Me.LastReading = ""
        Else
            Me._ReadingToolStripStatusLabel.SafeTextSetter($"{subsystem.Amount.ToString} {subsystem.Amount.Unit.ToString}")
            Me.LastReading = subsystem.Amount.ToString
            If subsystem.Amount.MetaStatus.HitCompliance Then
                Me._ComplianceToolStripStatusLabel.Text = compliance
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Real Compliance;. Instrument sensed an output overflow of the measured value.")
            ElseIf subsystem.Amount.MetaStatus.HitRangeCompliance Then
                Me._ComplianceToolStripStatusLabel.Text = rangeCompliance
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Range Compliance;. Instrument sensed an output overflow of the measured value.")
            ElseIf subsystem.Amount.MetaStatus.HitLevelCompliance Then
                Me._ComplianceToolStripStatusLabel.Text = levelCompliance
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Level Compliance;. Instrument sensed an output overflow of the measured value.")
            Else
                Me._ComplianceToolStripStatusLabel.Text = clear
                ' Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Parsed {0}", Me.LastReading)
            End If
        End If

    End Sub

    Private Sub OnSusystemInitialized(ByVal subsystem As MultimeterSubsystem)
        If subsystem Is Nothing Then Return
        With Me._PowerLineCyclesNumeric
            .Maximum = CDec(subsystem.PowerLineCyclesRange.Max)
            .Minimum = 1000 * CDec(subsystem.PowerLineCyclesRange.Min)
        End With
        With Me._FilterCountNumeric
            .Maximum = CDec(subsystem.FilterCountRange.Max)
            .Minimum = CDec(subsystem.FilterCountRange.Min)
        End With
        With Me._FilterWindowNumeric
            .Maximum = 100 * CDec(subsystem.FilterWindowRange.Max)
            .Minimum = 100 * CDec(subsystem.FilterWindowRange.Min)
        End With
        Me.DisplayFunctionModes()
    End Sub

    ''' <summary> Handles the Multimeter subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As MultimeterSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.AutoDelayMode)
                If subsystem.AutoDelayMode.HasValue Then Me.AutoDelayMode = subsystem.AutoDelayMode.Value
            Case NameOf(subsystem.AutoRangeEnabled)
                If subsystem.AutoRangeEnabled.HasValue Then Me._AutoRangeCheckBox.Checked = subsystem.AutoRangeEnabled.Value
            Case NameOf(subsystem.AutoZeroEnabled)
                If subsystem.AutoZeroEnabled.HasValue Then Me._AutoZeroCheckBox.Checked = subsystem.AutoZeroEnabled.Value
            Case NameOf(subsystem.FilterCount)
                If subsystem.FilterCount.HasValue Then Me._FilterCountNumeric.Value = subsystem.FilterCount.Value
            Case NameOf(subsystem.FilterEnabled)
                If subsystem.FilterEnabled.HasValue Then Me._FilterEnabledCheckBox.Checked = subsystem.FilterEnabled.Value
                If Me._FilterEnabledCheckBox.Checked <> Me._FilterGroupBox.Enabled Then Me._FilterGroupBox.Enabled = Me._FilterEnabledCheckBox.Checked
            Case NameOf(subsystem.FilterWindow)
                If subsystem.FilterWindow.HasValue Then Me._FilterWindowNumeric.Value = CDec(100 * subsystem.FilterWindow.Value)
            Case NameOf(subsystem.MovingAverageFilterEnabled)
                If subsystem.MovingAverageFilterEnabled.HasValue Then Me._MovingAverageRadioButton.Checked = subsystem.MovingAverageFilterEnabled.Value
                If subsystem.MovingAverageFilterEnabled.HasValue Then Me._RepeatingAverageRadioButton.Checked = Not subsystem.MovingAverageFilterEnabled.Value
            Case NameOf(subsystem.FunctionMode)
                Me._SenseFunctionComboBox.SelectedItem = subsystem.FunctionMode.GetValueOrDefault(VI.Tsp.MultimeterFunctionMode.VoltageDC).ValueDescriptionPair()
                If Me.selectedFunctionMode <> subsystem.FunctionMode.GetValueOrDefault(Me.selectedFunctionMode) Then
                    Me.OnSelectedFunctionModeChanged(subsystem.FunctionMode.Value)
                End If
            Case NameOf(subsystem.OpenDetectorEnabled)
                If subsystem.OpenDetectorEnabled.HasValue Then Me._OpenDetectorCheckBox.Checked = subsystem.OpenDetectorEnabled.Value
            Case NameOf(subsystem.PowerLineCycles)
                If subsystem.PowerLineCycles.HasValue Then Me._PowerLineCyclesNumeric.Value = CDec(subsystem.PowerLineCycles.Value)
            Case NameOf(subsystem.Range)
                If Me.selectedFunctionMode <> subsystem.FunctionMode.GetValueOrDefault(Me.selectedFunctionMode) Then
                    Me.OnSelectedFunctionModeChanged(subsystem.FunctionMode.Value)
                End If
                If subsystem.Range.HasValue Then Me.SenseRangeSetter(subsystem.Range.Value)
            Case NameOf(subsystem.Reading)
                Me.DisplayReading(subsystem)
        End Select
    End Sub

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MultimeterSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As MultimeterSubsystem = TryCast(sender, MultimeterSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Multimeter subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " CHANNEL "

    ''' <summary> Handles the Channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As ChannelSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.ClosedChannels)
                Me.ClosedChannels = subsystem.ClosedChannels
        End Select
    End Sub

    ''' <summary> Channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As ChannelSubsystem = TryCast(sender, ChannelSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Channel subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " DISPLAY "

    ''' <summary> Handles the Display subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As DisplaySubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.DisplayScreen)
        End Select
    End Sub

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplaySubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As DisplaySubsystem = TryCast(sender, DisplaySubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Display subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Private Sub OnLastError(ByVal lastError As VI.DeviceError)
        If lastError?.IsError Then
            Me._LastErrorTextBox.ForeColor = Drawing.Color.OrangeRed
        Else
            Me._LastErrorTextBox.ForeColor = Drawing.Color.Aquamarine
        End If
        Me._LastErrorTextBox.Text = lastError.CompoundErrorMessage
    End Sub

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnPropertyChanged(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(subsystem.DeviceErrors)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.LastDeviceError)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.ErrorAvailable)
                If Not subsystem.ReadingDeviceErrors Then
                    ' if no errors, this clears the error queue.
                    subsystem.QueryDeviceErrors()
                End If
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As StatusSubsystem = TryCast(sender, StatusSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Status subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

    ''' <summary> Reads a service request status. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ReadServiceRequestStatus()
        Try
            Me.Device.StatusSubsystem.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading service request;. {0}", ex.ToFullBlownString)
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
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As SystemSubsystem = TryCast(sender, SystemSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling System subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#End Region

#Region " LAST READING "

    Private _LastReading As String

    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property LastReading As String
        Get
            Return Me._LastReading
        End Get
        Set(value As String)
            If value Is Nothing Then value = ""
            If String.IsNullOrWhiteSpace(value) Then
                Me._LastReadingTextBox.SafeTextSetter("<last reading>")
            Else
                Me._LastReading = value
                If Me.ElapsedTimeStopwatch.IsRunning Then
                    Me.ElapsedTimeStopwatch.Stop()
                    Me._LastReadingTextBox.SafeTextSetter($"{value} @{Me.ElapsedTimeStopwatch.ElapsedMilliseconds}ms")
                Else
                    Me._LastReadingTextBox.SafeTextSetter(value)
                End If
            End If
            Me.SafePostPropertyChanged()
        End Set
    End Property

#End Region

#Region " DEVICE SETTINGS: FUNCTION MODE "

    ''' <summary> Displays a function modes. </summary>
    Private Sub DisplayFunctionModes()
        With Me._SenseFunctionComboBox
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(VI.Tsp.MultimeterFunctionMode).ValueDescriptionPairs()
            .DisplayMember = "Value"
            .ValueMember = "Key"
            If .Items.Count > 0 Then
                .SelectedItem = VI.Tsp.MultimeterFunctionMode.VoltageDC.ValueDescriptionPair()
            End If
        End With
    End Sub

    ''' <summary> Selects a new sense mode. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Friend Sub ApplyFunctionMode(ByVal value As VI.Tsp.MultimeterFunctionMode)
        Me._Device.MultimeterSubsystem.ApplyFunctionMode(value)
    End Sub

    ''' <summary>
    ''' Gets or sets the selected function mode.
    ''' </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedFunctionMode() As VI.Tsp.MultimeterFunctionMode
        Get
            Return CType(CType(Me._SenseFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                  Of [Enum], String)).Key, VI.Tsp.MultimeterFunctionMode)
        End Get
    End Property

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
            Me._ClosedChannelsTextBox.SafeTextSetter(value)
            If Me.ElapsedTimeStopwatch.IsRunning Then
                Me._ChannelListTextBox.SafeTextSetter($"{value} @{Me.ElapsedTimeStopwatch.ElapsedMilliseconds}ms")
                elapsedTimeCounter -= 1
                If elapsedTimeCounter < 0 Then Me.ElapsedTimeStopwatch.Stop()
            Else
                Me._ChannelListTextBox.SafeTextSetter(value)
            End If
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

    Private elapsedTimeCounter As Integer
    ''' <summary> Event handler. Called by _closeChannelsButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _CloseChannelsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _CloseChannelsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.updateChannelListComboBox()
            ' this also turns off status bit setting that enable notifications of wait completion.
            Me.Device.ClearExecutionState()
            ' must be reabled after clearing execution state.
            Me.Device.StatusSubsystem.EnableWaitComplete()
            elapsedTimeCounter = 2
            Me.ElapsedTimeStopwatch.Restart()
            Me.Device.ChannelSubsystem.ApplyClosedChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(2))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
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
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            updateChannelListComboBox()
            ' this also turns off status bit setting that enable notifications of wait completion.
            Me.Device.ClearExecutionState()
            ' must be reabled after clearing execution state.
            Me.Device.StatusSubsystem.EnableWaitComplete()
            elapsedTimeCounter = 2
            Me.ElapsedTimeStopwatch.Restart()
            Me.Device.ChannelSubsystem.ApplyOpenChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(2))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
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
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            updateChannelListComboBox()
            ' this also turns off status bit setting that enable notifications of wait completion.
            Me.Device.ClearExecutionState()
            ' must be reabled after clearing execution state.
            Me.Device.StatusSubsystem.EnableWaitComplete()
            elapsedTimeCounter = 3
            Me.ElapsedTimeStopwatch.Restart()
            Me.Device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
            Me.Device.ChannelSubsystem.ApplyClosedChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(2))
            ' this works only if a single channel:
            ' VI.ChannelSubsystem.CloseChannels(Me.Device, Me._channelListComboBox.Text)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
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
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            ' this also turns off status bit setting that enable notifications of wait completion.
            Me.Device.ClearExecutionState()
            ' must be reabled after clearing execution state.
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.ElapsedTimeStopwatch.Restart()
            elapsedTimeCounter = 2
            Me.Device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
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
                (value <> VI.ReadingTypes.None) AndAlso (value <> Me.selectedReading) Then
            Me._ReadingComboBox.ComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.selectedReading
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
    Private Sub _InitiateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.ErrorProvider.Annunciate(sender, "Not implemented yet")

            ' clear execution state before enabling events
            Me.Device.ClearExecutionState()

            ' set the service request
            Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.ServiceRequests.All And Not VI.ServiceRequests.MessageAvailable)

            ' trigger the initiation of the measurement letting the service request do the rest.
            Me.Device.ClearExecutionState()
            Me.ElapsedTimeStopwatch.Restart()
            ' Me.Device.TriggerSubsystem.Initiate()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
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
    Private Sub _ReadingComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If _InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            If Me._ReadingComboBox.Enabled AndAlso Me._ReadingComboBox.SelectedIndex >= 0 AndAlso
                    Not String.IsNullOrWhiteSpace(Me._ReadingComboBox.Text) Then
                Me.DisplayReading(Me.Device.MultimeterSubsystem)
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by _ReadButton for click events. Query the Device for a
    ''' reading. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.ElapsedTimeStopwatch.Restart()
            Me.Device.MultimeterSubsystem.Measure()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Toggles the session service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionServiceRequestHandlerEnabledCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me._InitializingComponents Then Return
        Dim checkBox As CheckBox = TryCast(sender, CheckBox)
        Try
            Me.Cursor = Cursors.WaitCursor
            If checkBox IsNot Nothing AndAlso checkBox.Checked <> Me.Device.SessionServiceRequestEventEnabled Then
                If checkBox IsNot Nothing AndAlso checkBox.Checked Then
                    Me.Device.Session.EnableServiceRequest()
                    If Me._ServiceRequestEnableBitmaskNumeric.Value = 0 Then
                        If Me._ServiceRequestEnableBitmaskNumeric.Value = 0 Then
                            Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
                        Else
                            Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableBitmaskNumeric.Value, ServiceRequests))
                        End If
                    Else
                        Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableBitmaskNumeric.Value, ServiceRequests))
                    End If
                Else
                    Me.Device.Session.DisableServiceRequest()
                    Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)
                End If
                Me.Device.StatusSubsystem.ReadRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Failed toggling session service request")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred toggling session service request;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the Device service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DeviceServiceRequestHandlerEnabledCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me._InitializingComponents Then Return
        Dim checkBox As CheckBox = TryCast(sender, CheckBox)
        Try
            Me.Cursor = Cursors.WaitCursor
            If checkBox IsNot Nothing AndAlso checkBox.Checked <> Me.Device.DeviceServiceRequestHandlerAdded Then
                If checkBox IsNot Nothing AndAlso checkBox.Checked Then
                    Me.AddServiceRequestEventHandler()
                Else
                    Me.RemoveServiceRequestEventHandler()
                End If
                Me.Device.StatusSubsystem.ReadRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Failed toggling device service request")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred toggling service request;. {0}", ex.ToFullBlownString)
        Finally
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

    ''' <summary> Executes the selected function mode changed action. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    Private Sub OnSelectedFunctionModeChanged(ByVal functionMode As MultimeterFunctionMode)
        Dim format As String = "Range [{0}]:"
        Me._SenseRangeNumericLabel.Text = MultimeterSubsystemBase.ParseUnits(functionMode).ToString
        Me._SenseRangeNumericLabel.Text = String.Format(format, Me._SenseRangeNumericLabel.Text)
        Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
        With Me._SenseRangeNumeric
            If Me.IsDeviceOpen Then
                .Maximum = CDec(Me.Device.MultimeterSubsystem.FunctionModeRanges(functionMode).Max)
                .Minimum = CDec(Me.Device.MultimeterSubsystem.FunctionModeRanges(functionMode).Min)
                Select Case functionMode
                    Case MultimeterFunctionMode.CurrentAC, MultimeterFunctionMode.CurrentDC
                        .DecimalPlaces = 3
                    Case MultimeterFunctionMode.VoltageAC, MultimeterFunctionMode.VoltageDC
                        .DecimalPlaces = 3
                    Case MultimeterFunctionMode.ResistanceCommonWire, MultimeterFunctionMode.ResistanceFourWire, MultimeterFunctionMode.ResistanceTwoWire
                        .DecimalPlaces = 0
                End Select
            End If
        End With
    End Sub

    ''' <summary> Event handler. Called by _SenseFunctionComboBox for selected index changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="degC")>
    Private Sub _SenseFunctionComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SenseFunctionComboBox.SelectedIndexChanged
        If _InitializingComponents Then Return
        Dim control As Windows.Forms.Control = TryCast(sender, Windows.Forms.Control)
        If control IsNot Nothing AndAlso control.Enabled Then
            Me.OnSelectedFunctionModeChanged(Me.selectedFunctionMode)
        End If
    End Sub

    ''' <summary> Applies the function mode button click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyFunctionModeButton_Click(sender As Object, e As EventArgs) Handles _ApplyFunctionModeButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.ApplyFunctionMode(Me.selectedFunctionMode)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
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
            Me.ErrorProvider.Clear()
            If Me.selectedFunctionMode <> Me.Device.MultimeterSubsystem.FunctionMode.Value Then
                Me.ErrorProvider.Annunciate(sender, "Set function first")
            Else
                Me.applySenseSettings()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
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
            autoDelayMode = MultimeterAutoDelayMode.On
            Me._AutoDelayCheckBox.Text = "Auto Delay ON"
        ElseIf Me._AutoDelayCheckBox.CheckState = CheckState.Indeterminate Then
            autoDelayMode = MultimeterAutoDelayMode.Once
            Me._AutoDelayCheckBox.Text = "Auto Delay ONCE"
        Else
            autoDelayMode = MultimeterAutoDelayMode.Off
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
                Me.ErrorProvider.Clear()
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SystemSubsystem.ClearInterface()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
    Private Sub _ClearDeviceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim activity As String = "clearing selective device"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SystemSubsystem.ClearDevice()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
                Me.ErrorProvider.Clear()
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SystemSubsystem.ClearExecutionState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                If Me.IsDeviceOpen Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.ResetKnownState()
                End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                If Me.IsDeviceOpen Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.ResetKnownState()
                    activity = "initializing known state"
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.InitKnownState()
                End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: SESSION "

    ''' <summary> Toggles session message tracing. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionTraceEnabledMenuItem_CheckedChanged(ByVal sender As Object, e As System.EventArgs) Handles _SessionServiceRequestHandlerEnabledMenuItem.Click
        If Me._InitializingComponents Then Return
        Dim activity As String = "toggling instrument message tracing"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        If menuItem IsNot Nothing Then
        End If
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SessionMessagesTraceEnabled = menuItem.Checked
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the session service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionServiceRequestHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SessionServiceRequestHandlerEnabledMenuItem.CheckStateChanged
        If Me._InitializingComponents Then Return
        Dim activity As String = "Toggle session service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.Session.ServiceRequestEventEnabled Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.Device.Session.EnableServiceRequest()
                    If Me._ServiceRequestEnableBitmaskNumeric.Value = 0 Then
                        If Me._ServiceRequestEnableBitmaskNumeric.Value = 0 Then
                            Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
                        Else
                            Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableBitmaskNumeric.Value, ServiceRequests))
                        End If
                    Else
                        Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableBitmaskNumeric.Value, ServiceRequests))
                    End If
                Else
                    Me.Device.Session.DisableServiceRequest()
                    Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)
                End If
                Me.Device.StatusSubsystem.ReadRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the Device service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DeviceServiceRequestHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _DeviceServiceRequestHandlerEnabledMenuItem.CheckStateChanged
        If Me._InitializingComponents Then Return
        Dim activity As String = "Toggle device service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.DeviceServiceRequestHandlerAdded Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.AddServiceRequestEventHandler()
                Else
                    Me.RemoveServiceRequestEventHandler()
                End If
                Me.Device.StatusSubsystem.ReadRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
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
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling {0} property change;. {1}",
                               e?.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Assigns talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AssignTalker(talker As ITraceMessageTalker)
        MyBase.AssignTalker(talker)
        Me._SimpleReadWriteControl.AssignTalker(talker)
         My.MyLibrary.Identify(talker)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        ' this should apply only to the listeners associated with this form
        MyBase.ApplyListenerTraceLevel(listenerType, value)
        Me._SimpleReadWriteControl?.ApplyListenerTraceLevel(listenerType, value)
    End Sub

#End Region

End Class


#Region " UNUSED "
#If False Then

#Region " TALKER "

    ''' <summary> Adds listeners such as current level trace message box and log. </summary>
    Protected Overrides Sub AddListeners()
        MyBase.AddListeners()
        Me._SimpleReadWriteControl.AddListeners(Me.Talker)
    End Sub

    ''' <summary> Adds listeners such as top level trace message box and log. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of IMessageListener))
        MyBase.AddListeners(listeners)
        Me._SimpleReadWriteControl.AddListeners(listeners)
    End Sub

    ''' <summary> Adds the listeners. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AddListeners(ByVal talker As ITraceMessageTalker)
        MyBase.AddListeners(talker)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the log listener. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="log"> The log. </param>
    Public Overrides Sub AddListeners(ByVal log As MyLog)
        If log Is Nothing Then Throw New ArgumentNullException(NameOf(log))
        MyBase.AddListeners(log)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region
#End If
#End Region
