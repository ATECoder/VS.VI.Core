Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
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
<System.ComponentModel.DisplayName("K2450 Control"),
      System.ComponentModel.Description("Keithley 2450 Device Control"),
      System.Drawing.ToolboxBitmap(GetType(K2450Control))>
Public Class K2450Control
    Inherits VI.Instrument.ResourceControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private Property InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        MyBase.New()
        Me._New(Device.Create)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Public Sub New(ByVal device As Device)
        MyBase.New()
        Me._New(device)
        Me.IsDeviceOwner = False
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Private Sub _New(ByVal device As Device)

        Me.InitializingComponents = True
        ' This call is required by the designer.
        InitializeComponent()
        Me.InitializingComponents = False

        Me._ToolStripPanel.Renderer = New CustomProfessionalRenderer
        MyBase.Connector = Me._ResourceSelectorConnector

        Me._AssignDevice(device)
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
    Public Property Device As Device
        Get
            Return Me._Device
        End Get
        Set(value As Device)
            Me._AssignDevice(value)
        End Set
    End Property

    ''' <summary> Assign device. </summary>
    ''' <param name="value"> The value. </param>
    Private Sub _AssignDevice(ByVal value As Device)
        If Me._Device IsNot Nothing Then
            ' the device base already clears all or only the private listeners. 
        End If
        Me._Device = value
        If value IsNot Nothing Then
            value.CaptureSyncContext(WindowsFormsSynchronizationContext.Current)
            value.AddPrivateListener(Me._TraceMessagesBox)
        End If
        MyBase.DeviceBase = value
    End Sub


    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device)
        Me.IsDeviceOwner = False
        Me.Device = value
    End Sub

    ''' <summary> Releases the device. </summary>
    Protected Overrides Sub ReleaseDevice()
        MyBase.ReleaseDevice()
        Me._Device = Nothing
    End Sub

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As DeviceBase)
        If Me.IsDeviceOpen Then
            Me._SimpleReadWriteControl.Connect(device?.Session)
        Else
            Me._SimpleReadWriteControl.Disconnect()
        End If
        For Each t As Windows.Forms.TabPage In Me._Tabs.TabPages
            If t IsNot Me._MessagesTabPage Then Me.RecursivelyEnable(t.Controls, Me.IsDeviceOpen)
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
        If Me.Device.MeasureSubsystem IsNot Nothing Then AddHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
        If Me.Device.DisplaySubsystem IsNot Nothing Then AddHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
        If Me.Device.StatusSubsystem IsNot Nothing Then AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        If Me.Device.SystemSubsystem IsNot Nothing Then AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        MyBase.DeviceOpened(sender, e)
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overrides Sub OnTitleChanged(ByVal value As String)
        Me._TitleLabel.Text = value
        Me._TitleLabel.Visible = Not String.IsNullOrWhiteSpace(value)
        If String.IsNullOrWhiteSpace(value) Then
            Stop
        End If
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
            If Me.Device.MeasureSubsystem IsNot Nothing Then RemoveHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
            If Me.Device.DisplaySubsystem IsNot Nothing Then RemoveHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
            If Me.Device.StatusSubsystem IsNot Nothing Then RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            If Me.Device.SystemSubsystem IsNot Nothing Then RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " MEASURE "

    ''' <summary> Displays a reading described by subsystem. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub DisplayReading(ByVal subsystem As MeasureSubsystem)

        Const clear As String = "    "
        Const compliance As String = "..C.."
        Const rangeCompliance As String = ".RC."
        Const levelCompliance As String = ".LC."

        If Not subsystem.Reading.HasValue Then
            Me._ReadingToolStripStatusLabel.Text = "-.------- V"
            Me._ComplianceToolStripStatusLabel.Text = clear
            Me.LastReading = ""
        Else
            Me._ReadingToolStripStatusLabel.SafeTextSetter($"{subsystem.Amount.ToString} {subsystem.Amount.Unit.ToString}")
            Me.LastReading = subsystem.Amount.ToString
            If subsystem.Amount.MetaStatus.HitCompliance Then
                Me._ComplianceToolStripStatusLabel.Text = compliance
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Real Compliance;. Instrument sensed an output overflow of the measured value.")
            ElseIf subsystem.Amount.MetaStatus.HitRangeCompliance Then
                Me._ComplianceToolStripStatusLabel.Text = rangeCompliance
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Range Compliance;. Instrument sensed an output overflow of the measured value.")
            ElseIf subsystem.Amount.MetaStatus.HitLevelCompliance Then
                Me._ComplianceToolStripStatusLabel.Text = levelCompliance
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Level Compliance;. Instrument sensed an output overflow of the measured value.")
            Else
                Me._ComplianceToolStripStatusLabel.Text = clear
                ' Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Parsed {0}", Me.LastReading)
            End If
        End If

    End Sub

    ''' <summary> Handles the Multimeter subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As MeasureSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If subsystem.AutoRangeEnabled.HasValue Then Me._AutoRangeCheckBox.Checked = subsystem.AutoRangeEnabled.Value
            Case NameOf(subsystem.AutoZeroEnabled)
                If subsystem.AutoZeroEnabled.HasValue Then Me._AutoZeroCheckBox.Checked = subsystem.AutoZeroEnabled.Value
            Case NameOf(subsystem.FilterCount)
                If subsystem.FilterCount.HasValue Then Me._FilterCountNumeric.Value = subsystem.FilterCount.Value
            Case NameOf(subsystem.FilterCountRange)
                With Me._FilterCountNumeric
                    .Maximum = CDec(subsystem.FilterCountRange.Max)
                    .Minimum = CDec(subsystem.FilterCountRange.Min)
                End With
            Case NameOf(subsystem.FilterEnabled)
                If subsystem.FilterEnabled.HasValue Then Me._FilterEnabledCheckBox.Checked = subsystem.FilterEnabled.Value
                If Me._FilterEnabledCheckBox.Checked <> Me._FilterGroupBox.Enabled Then Me._FilterGroupBox.Enabled = Me._FilterEnabledCheckBox.Checked
            Case NameOf(subsystem.FilterWindow)
                If subsystem.FilterWindow.HasValue Then Me._FilterWindowNumeric.Value = CDec(100 * subsystem.FilterWindow.Value)
            Case NameOf(subsystem.FilterWindowRange)
                With Me._FilterWindowNumeric
                    .Maximum = 100 * CDec(subsystem.FilterWindowRange.Max)
                    .Minimum = 100 * CDec(subsystem.FilterWindowRange.Min)
                End With
            Case NameOf(subsystem.MovingAverageFilterEnabled)
                If subsystem.MovingAverageFilterEnabled.HasValue Then Me._MovingAverageRadioButton.Checked = subsystem.MovingAverageFilterEnabled.Value
                If subsystem.MovingAverageFilterEnabled.HasValue Then Me._RepeatingAverageRadioButton.Checked = Not subsystem.MovingAverageFilterEnabled.Value
            Case NameOf(subsystem.FunctionMode)
                If Me._SenseFunctionComboBox.DataSource Is Nothing Then Me.DisplayFunctionModes()
                Me._SenseFunctionComboBox.SelectedItem = subsystem.FunctionMode.GetValueOrDefault(VI.Tsp2.MeasureFunctionMode.VoltageDC).ValueDescriptionPair()
            Case NameOf(subsystem.FunctionRange)
                With Me._SenseRangeNumeric
                    .Maximum = CDec(subsystem.FunctionRange.Max)
                    .Minimum = CDec(subsystem.FunctionRange.Min)
                End With
            Case NameOf(subsystem.FunctionRangeDecimalPlaces)
                Me._SenseRangeNumeric.DecimalPlaces = subsystem.DefaultFunctionModeDecimalPlaces
            Case NameOf(subsystem.FunctionUnit)
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.FunctionUnit}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
            Case NameOf(subsystem.OpenDetectorEnabled)
                If subsystem.OpenDetectorEnabled.HasValue Then Me._OpenDetectorCheckBox.Checked = subsystem.OpenDetectorEnabled.Value
            Case NameOf(subsystem.PowerLineCycles)
                If subsystem.PowerLineCycles.HasValue Then Me._PowerLineCyclesNumeric.Value = CDec(subsystem.PowerLineCycles.Value)
            Case NameOf(subsystem.PowerLineCyclesRange)
                With Me._PowerLineCyclesNumeric
                    .Maximum = CDec(subsystem.PowerLineCyclesRange.Max)
                    .Minimum = CDec(subsystem.PowerLineCyclesRange.Min)
                    .DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces
                End With
            Case NameOf(subsystem.Range)
                If subsystem.Range.HasValue Then Me.SenseRangeSetter(subsystem.Range.Value)
            Case NameOf(subsystem.Reading)
                Me.DisplayReading(subsystem)
        End Select
    End Sub

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As MeasureSubsystem = TryCast(sender, MeasureSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Multimeter subsystem {1} property change;. {2}",
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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Display subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
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
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnPropertyChanged(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(subsystem.DeviceErrors)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.LastDeviceError)
                OnLastError(subsystem.LastDeviceError)
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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Status subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling System subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#End Region

#Region " STATUS DISPLAY "

    ''' <summary> Gets or sets the status. </summary>
    ''' <value> The status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property Status As String
        Get
            Return Me._StatusLabel.Text
        End Get
        Set(value As String)
            Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(value, Me._StatusLabel)
            Me._StatusLabel.ToolTipText = value
        End Set
    End Property

    ''' <summary> Gets or sets the identity. </summary>
    ''' <value> The identity. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property Identity As String
        Get
            Return Me._IdentityLabel.Text
        End Get
        Set(value As String)
            Me._IdentityLabel.Text = isr.Core.Pith.CompactExtensions.Compact(value, Me._IdentityLabel)
        End Set
    End Property

    ''' <summary> Gets or sets the status register caption. </summary>
    ''' <value> The status register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property StatusRegisterCaption As String
        Get
            Return Me._StatusRegisterLabel.Text
        End Get
        Set(value As String)
            Me._StatusRegisterLabel.Text = value
        End Set
    End Property

    ''' <summary> Gets or sets the standard register caption. </summary>
    ''' <value> The status register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property StandardRegisterCaption As String
        Get
            Return Me._StandardRegisterLabel.Text
        End Get
        Set(value As String)
            Me._StandardRegisterLabel.Text = value
        End Set
    End Property

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
            .DataSource = GetType(VI.Tsp2.MultimeterFunctionMode).ValueDescriptionPairs()
            .DisplayMember = "Value"
            .ValueMember = "Key"
            If .Items.Count > 0 Then
                .SelectedItem = VI.Tsp2.MultimeterFunctionMode.VoltageDC.ValueDescriptionPair()
            End If
        End With
    End Sub

    ''' <summary> Selects a new sense mode. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Friend Sub ApplyFunctionMode(ByVal value As VI.Tsp2.MeasureFunctionMode)
        Me._Device.MeasureSubsystem.ApplyFunctionMode(value)
    End Sub

    ''' <summary>
    ''' Gets or sets the selected function mode.
    ''' </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedFunctionMode() As VI.Tsp2.MeasureFunctionMode
        Get
            Return CType(CType(Me._SenseFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                  Of [Enum], String)).Key, VI.Tsp2.MeasureFunctionMode)
        End Get
    End Property

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
    Private Sub _InitiateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me._InfoProvider.Annunciate(sender, "Not implemented yet")

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
    Private Sub _ReadingComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If _InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            If Me._ReadingComboBox.Enabled AndAlso Me._ReadingComboBox.SelectedIndex >= 0 AndAlso
                    Not String.IsNullOrWhiteSpace(Me._ReadingComboBox.Text) Then
                Me.DisplayReading(Me.Device.MeasureSubsystem)
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
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
            Me._InfoProvider.Clear()
            Me.ElapsedTimeStopwatch.Restart()
            Me.Device.MeasureSubsystem.Measure()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
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
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, "Failed toggling session service request")
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
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, "Failed toggling device service request")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred toggling service request;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


#End Region

#Region " CONTROL EVENT HANDLERS: MEASURE "

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
        If _InitializingComponents Then Return
        Dim control As Windows.Forms.Control = TryCast(sender, Windows.Forms.Control)
        If control IsNot Nothing AndAlso control.Enabled Then
            Dim functionMode As MeasureFunctionMode = Me.SelectedFunctionMode
            With Device.MeasureSubsystem
                .FunctionRange = .ToRange(functionMode)
                .FunctionUnit = .ToUnit(functionMode)
                .FunctionRangeDecimalPlaces = .ToDecimalPlaces(functionMode)
            End With
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
            Me.ApplyFunctionMode(Me.SelectedFunctionMode)
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

        With Me.Device.MeasureSubsystem
            If Not Nullable.Equals(.PowerLineCycles, Me._PowerLineCyclesNumeric.Value) Then
                .ApplyPowerLineCycles(Me._PowerLineCyclesNumeric.Value)
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
            If Me.SelectedFunctionMode <> Me.Device.MeasureSubsystem.FunctionMode.Value Then
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
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SystemSubsystem.ClearInterface()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
                Me._InfoProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SystemSubsystem.ClearDevice()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SystemSubsystem.ClearExecutionState()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
                If Me.IsDeviceOpen Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.ResetKnownState()
                End If
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
                If Me.IsDeviceOpen Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.ResetKnownState()
                    activity = "initializing known state"
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.InitKnownState()
                End If
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

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
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SessionMessagesTraceEnabled = menuItem.Checked
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.Session.ServiceRequestEventEnabled Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
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
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.DeviceServiceRequestHandlerAdded Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.AddServiceRequestEventHandler()
                Else
                    Me.RemoveServiceRequestEventHandler()
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
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
                    Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusMessage, Me._StatusLabel)
                    Me._StatusLabel.ToolTipText = sender.StatusMessage
                Case NameOf(sender.ServiceRequestValue)
                    Me._StatusRegisterLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
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

#Region " UNUSED "
#If False Then
' 01/23/2018
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Try
                    Me.Device?.RemovePrivateListener(Me._TraceMessagesBox)
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
    Private Sub _AssignDevice(ByVal value As Device)
        MyBase.DeviceBase = value
        If Me._Device IsNot Nothing Then
        End If
        Me._Device = value
        If Me._Device IsNot Nothing Then
            value.CaptureSyncContext(WindowsFormsSynchronizationContext.Current)
            MyBase.DeviceBase.CaptureSyncContext(WindowsFormsSynchronizationContext.Current)
            value.NotifyDeviceOpentate()
            Me.AssignTalker(value.Talker)
            Me.ApplyListenerTraceLevel(ListenerType.Display, value.Talker.TraceShowLevel)
            value.AddPrivateListener(Me._TraceMessagesBox)
        End If
    End Sub

#End If
#End Region