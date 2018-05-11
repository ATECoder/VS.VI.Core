<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class K7500Control

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="nplc")>
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(K7500Control))
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
        Me._BufferDataGridView = New System.Windows.Forms.DataGridView()
        Me._ToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me._SystemToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ResetSplitButton = New System.Windows.Forms.ToolStripSplitButton()
        Me._ClearInterfaceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ReadStatusByteMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearDeviceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ResetKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._InitKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearExecutionStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ResetMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ServiceRequestHandlersEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._TraceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._LogTraceLevelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._DisplayTraceLevelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SessionNotificationLevelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SessionNotificationLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._SessionServiceRequestHandlerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._DeviceServiceRequestHandlerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ReadStatusByteMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._LogTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._DisplayTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._ReadTerminalStateButton = New System.Windows.Forms.ToolStripButton()
        Me._ServiceRequestEnableNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ServiceRequestEnableBitmaskNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._BufferToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ReadBufferButton = New System.Windows.Forms.ToolStripButton()
        Me._BufferCountLabel = New System.Windows.Forms.ToolStripLabel()
        Me._LastPointNumberLabel = New System.Windows.Forms.ToolStripLabel()
        Me._FirstPointNumberLabel = New System.Windows.Forms.ToolStripLabel()
        Me._BufferNameLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ClearBufferDisplayButton = New System.Windows.Forms.ToolStripButton()
        Me._BufferSizeNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._ReadingToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ReadButton = New System.Windows.Forms.ToolStripButton()
        Me._ReadingComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me._AbortButton = New System.Windows.Forms.ToolStripButton()
        Me._GoSplitButton = New System.Windows.Forms.ToolStripSplitButton()
        Me._InitiateTriggerPlanMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._AbortStartTriggerPlanMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._MonitorActiveTriggerPlanMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._InitMonitorReadRepeatMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._RepeatMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._StreamBufferMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SenseTabPage = New System.Windows.Forms.TabPage()
        Me._OpenLeadsDetectionCheckBox = New System.Windows.Forms.CheckBox()
        Me._ApplyFunctionModeButton = New System.Windows.Forms.Button()
        Me._SenseMeasureDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me._SenseRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me._PowerLineCyclesNumeric = New System.Windows.Forms.NumericUpDown()
        Me._TriggerDelayNumericLabel = New System.Windows.Forms.Label()
        Me._SenseRangeNumericLabel = New System.Windows.Forms.Label()
        Me._PowerLineCyclesNumericLabel = New System.Windows.Forms.Label()
        Me._SenseFunctionComboBox = New System.Windows.Forms.ComboBox()
        Me._SenseFunctionComboBoxLabel = New System.Windows.Forms.Label()
        Me._SenseAutoRangeToggle = New System.Windows.Forms.CheckBox()
        Me._ApplySenseSettingsButton = New System.Windows.Forms.Button()
        Me._TriggerTabPage = New System.Windows.Forms.TabPage()
        Me._TriggerToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me._TriggerDelayToolStrip = New System.Windows.Forms.ToolStrip()
        Me._TriggerDelaysToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._StartTriggerDelayNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._StartTriggerDelayNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._EndTriggerDelayNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._EndTriggerDelayNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._Limit1ToolStrip = New System.Windows.Forms.ToolStrip()
        Me._Limits1Label = New System.Windows.Forms.ToolStripLabel()
        Me._Limit1DecimalsNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._LowerLimit1NumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._LowerLimit1Numeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._UpperLimit1NumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._UpperLimit1Numeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._GradeBinningToolStrip = New System.Windows.Forms.ToolStrip()
        Me._BinningToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._PassBitPatternNumericButton = New System.Windows.Forms.ToolStripButton()
        Me._PassBitPatternNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._OpenLeadsBitPatternNumericButton = New System.Windows.Forms.ToolStripButton()
        Me._OpenLeadsBitPatternNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._FailBitPatternNumericButton = New System.Windows.Forms.ToolStripButton()
        Me._FailLimit1BitPatternNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._TriggerLayerToolStrip = New System.Windows.Forms.ToolStrip()
        Me._TriggerLayerToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._TriggerCountNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._TriggerCountNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._TriggerSourceComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me._TriggerSplitButton = New System.Windows.Forms.ToolStripSplitButton()
        Me._ContinuousTriggerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ReadTriggerStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._TriggerModelsToolStrip = New System.Windows.Forms.ToolStrip()
        Me._TriggerModelLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ApplyTriggerModelSplitButton = New System.Windows.Forms.ToolStripSplitButton()
        Me._LoadGradeBinTriggerModelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._LoadSimpleLoopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._RunSimpleLoopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearTriggerModelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._MeterCompleterFirstGradingBinningMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ReadWriteTabPage = New System.Windows.Forms.TabPage()
        Me._SimpleReadWriteControl = New isr.VI.Instrument.SimpleReadWriteControl()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._TraceMessagesBox = New isr.Core.Pith.TraceMessagesBox()
        Me._ResourceSelectorConnector = New isr.VI.Instrument.ResourceSelectorConnector()
        Me._StatusStrip = New System.Windows.Forms.StatusStrip()
        Me._StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._IdentityLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._StatusRegisterLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._StandardRegisterLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._TimingLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._TipsToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._InfoProvider = New isr.Core.Controls.InfoProvider(Me.components)
        Me._LastErrorTextBox = New System.Windows.Forms.TextBox()
        Me._ReadingStatusStrip = New System.Windows.Forms.StatusStrip()
        Me._FailureToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._ReadingToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._TriggerStateLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._LastReadingTextBox = New System.Windows.Forms.TextBox()
        Me._Panel = New System.Windows.Forms.Panel()
        Me._Layout = New System.Windows.Forms.TableLayoutPanel()
        Me._TitleLabel = New System.Windows.Forms.Label()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        CType(Me._BufferDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ToolStripPanel.SuspendLayout()
        Me._SystemToolStrip.SuspendLayout()
        Me._BufferToolStrip.SuspendLayout()
        Me._ReadingToolStrip.SuspendLayout()
        Me._SenseTabPage.SuspendLayout()
        CType(Me._SenseMeasureDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._PowerLineCyclesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._TriggerTabPage.SuspendLayout()
        Me._TriggerToolStripPanel.SuspendLayout()
        Me._TriggerDelayToolStrip.SuspendLayout()
        Me._Limit1ToolStrip.SuspendLayout()
        Me._GradeBinningToolStrip.SuspendLayout()
        Me._TriggerLayerToolStrip.SuspendLayout()
        Me._TriggerModelsToolStrip.SuspendLayout()
        Me._ReadWriteTabPage.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
        Me._StatusStrip.SuspendLayout()
        CType(Me._InfoProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ReadingStatusStrip.SuspendLayout()
        Me._Panel.SuspendLayout()
        Me._Layout.SuspendLayout()
        Me.SuspendLayout()
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ReadingTabPage)
        Me._Tabs.Controls.Add(Me._SenseTabPage)
        Me._Tabs.Controls.Add(Me._TriggerTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(52, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 76)
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 335)
        Me._Tabs.TabIndex = 3
        '
        '_ReadingTabPage
        '
        Me._ReadingTabPage.Controls.Add(Me._BufferDataGridView)
        Me._ReadingTabPage.Controls.Add(Me._ToolStripPanel)
        Me._ReadingTabPage.Controls.Add(Me._BufferToolStrip)
        Me._ReadingTabPage.Controls.Add(Me._ReadingToolStrip)
        Me._ReadingTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadingTabPage.Name = "_ReadingTabPage"
        Me._ReadingTabPage.Size = New System.Drawing.Size(356, 305)
        Me._ReadingTabPage.TabIndex = 0
        Me._ReadingTabPage.Text = "Reading"
        Me._ReadingTabPage.UseVisualStyleBackColor = True
        '
        '_BufferDataGridView
        '
        Me._BufferDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me._BufferDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me._BufferDataGridView.Location = New System.Drawing.Point(0, 53)
        Me._BufferDataGridView.Name = "_BufferDataGridView"
        Me._BufferDataGridView.Size = New System.Drawing.Size(356, 252)
        Me._BufferDataGridView.TabIndex = 9
        Me._TipsToolTip.SetToolTip(Me._BufferDataGridView, "Buffer data")
        '
        '_ToolStripPanel
        '
        Me._ToolStripPanel.Controls.Add(Me._SystemToolStrip)
        Me._ToolStripPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ToolStripPanel.Location = New System.Drawing.Point(0, 305)
        Me._ToolStripPanel.Name = "_ToolStripPanel"
        Me._ToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me._ToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me._ToolStripPanel.Size = New System.Drawing.Size(356, 0)
        '
        '_SystemToolStrip
        '
        Me._SystemToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._SystemToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetSplitButton, Me._ReadTerminalStateButton, Me._ServiceRequestEnableNumericLabel, Me._ServiceRequestEnableBitmaskNumeric})
        Me._SystemToolStrip.Location = New System.Drawing.Point(0, 237)
        Me._SystemToolStrip.Name = "_SystemToolStrip"
        Me._SystemToolStrip.Size = New System.Drawing.Size(356, 28)
        Me._SystemToolStrip.TabIndex = 21
        Me._SystemToolStrip.Text = "System Tools"
        Me._TipsToolTip.SetToolTip(Me._SystemToolStrip, "System operations")
        '
        '_ResetSplitButton
        '
        Me._ResetSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ResetSplitButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetMenuItem, Me._ServiceRequestHandlersEnabledMenuItem, Me._TraceMenuItem})
        Me._ResetSplitButton.Image = CType(resources.GetObject("_ResetSplitButton.Image"), System.Drawing.Image)
        Me._ResetSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ResetSplitButton.Name = "_ResetSplitButton"
        Me._ResetSplitButton.Size = New System.Drawing.Size(51, 25)
        Me._ResetSplitButton.Text = "Reset"

        Me._ResetSplitButton.ToolTipText = "Reset, Clear, etc."
        '
        '_ClearInterfaceMenuItem
        '
        Me._ClearInterfaceMenuItem.Name = "_ClearInterfaceMenuItem"
        Me._ClearInterfaceMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._ClearInterfaceMenuItem.Text = "Clear Interface"
        Me._ClearInterfaceMenuItem.ToolTipText = "Issues an interface clear command"
        '
        '_ClearDeviceMenuItem
        '
        Me._ClearDeviceMenuItem.Name = "_ClearDeviceMenuItem"
        Me._ClearDeviceMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._ClearDeviceMenuItem.Text = "Clear Device (SDC)"
        '
        '_ResetKnownStateMenuItem
        '
        Me._ResetKnownStateMenuItem.Name = "_ResetKnownStateMenuItem"
        Me._ResetKnownStateMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._ResetKnownStateMenuItem.Text = "Reset to Known State (RST)"
        '
        '_InitKnownStateMenuItem
        '
        Me._InitKnownStateMenuItem.Name = "_InitKnownStateMenuItem"
        Me._InitKnownStateMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._InitKnownStateMenuItem.Text = "Init to Known State"
        Me._InitKnownStateMenuItem.ToolTipText = "Initializes to custom known state"
        '
        '_ClearExecutionStateMenuItem
        '
        Me._ClearExecutionStateMenuItem.Name = "_ClearExecutionStateMenuItem"
        Me._ClearExecutionStateMenuItem.Size = New System.Drawing.Size(217, 22)

        Me._ClearExecutionStateMenuItem.Text = "Clear Execution State (CLS)"
        Me._ClearExecutionStateMenuItem.ToolTipText = "Clears the execution state"
        '
        '_ReadStatusByteMenuItem
        '
        Me._ReadStatusByteMenuItem.Name = "_ReadStatusByteMenuItem"
        Me._ReadStatusByteMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._ReadStatusByteMenuItem.Text = "Read Status Byte"
        '
        '_ResetMenuItem
        '
        Me._ResetMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ClearInterfaceMenuItem, Me._ClearDeviceMenuItem, Me._ResetKnownStateMenuItem, Me._InitKnownStateMenuItem, Me._ClearExecutionStateMenuItem, Me._ReadStatusByteMenuItem})
        Me._ResetMenuItem.Name = "_ResetMenuItem"
        Me._ResetMenuItem.Size = New System.Drawing.Size(216, 22)
        Me._ResetMenuItem.Text = "Reset..."
        Me._ResetMenuItem.ToolTipText = "Opens the reset menus"
        '
        '_ServiceRequestHandlersEnabledMenuItem
        '
        Me._ServiceRequestHandlersEnabledMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SessionServiceRequestHandlerEnabledMenuItem, Me._DeviceServiceRequestHandlerEnabledMenuItem})
        Me._ServiceRequestHandlersEnabledMenuItem.Name = "_ServiceRequestHandlersEnabledMenuItem"
        Me._ServiceRequestHandlersEnabledMenuItem.Size = New System.Drawing.Size(216, 22)
        Me._ServiceRequestHandlersEnabledMenuItem.Text = "SRQ Handlers Enable..."
        Me._ServiceRequestHandlersEnabledMenuItem.ToolTipText = "Opens the SQR Handler Enable menu"
        '
        '_TraceMenuItem
        '
        Me._TraceMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SessionNotificationLevelMenuItem, Me._LogTraceLevelMenuItem, Me._DisplayTraceLevelMenuItem})
        Me._TraceMenuItem.Name = "_TraceMenuItem"
        Me._TraceMenuItem.Size = New System.Drawing.Size(216, 22)
        Me._TraceMenuItem.Text = "Trace..."
        Me._TraceMenuItem.ToolTipText = "Opens the trace menus"
        '
        '_SessionNotificationLevelMenuItem
        '
        Me._SessionNotificationLevelMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SessionNotificationLevelComboBox})
        Me._SessionNotificationLevelMenuItem.Name = "_SessionNotificationLevelMenuItem"
        Me._SessionNotificationLevelMenuItem.Size = New System.Drawing.Size(209, 22)
        Me._SessionNotificationLevelMenuItem.Text = "Session Notification Level"
        Me._SessionNotificationLevelMenuItem.ToolTipText = "Shows the session notification level selector"
        '
        '_SessionNotificationLevelComboBox
        '
        Me._SessionNotificationLevelComboBox.Name = "_SessionNotificationLevelComboBox"
        Me._SessionNotificationLevelComboBox.Size = New System.Drawing.Size(100, 22)
        Me._SessionNotificationLevelComboBox.ToolTipText = "Select the session notification level"
        '
        '_LogTraceLevelMenuItem
        '
        Me._LogTraceLevelMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._LogTraceLevelComboBox})
        Me._LogTraceLevelMenuItem.Name = "_LogTraceLevelMenuItem"
        Me._LogTraceLevelMenuItem.Size = New System.Drawing.Size(209, 22)
        Me._LogTraceLevelMenuItem.Text = "Log Trace Level"
        Me._LogTraceLevelMenuItem.ToolTipText = "Shows the log trace levels"
        '
        '_DisplayTraceLevelMenuItem
        '
        Me._DisplayTraceLevelMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._DisplayTraceLevelComboBox})
        Me._DisplayTraceLevelMenuItem.Name = "_DisplayTraceLevelMenuItem"
        Me._DisplayTraceLevelMenuItem.Size = New System.Drawing.Size(209, 22)
        Me._DisplayTraceLevelMenuItem.Text = "Display Trace Level"
        Me._DisplayTraceLevelMenuItem.ToolTipText = "Shows the display trace levels"
        '
        '_SessionServiceRequestHandlerEnabledMenuItem
        '
        Me._SessionServiceRequestHandlerEnabledMenuItem.CheckOnClick = True
        Me._SessionServiceRequestHandlerEnabledMenuItem.Name = "_SessionServiceRequestHandlerEnabledMenuItem"
        Me._SessionServiceRequestHandlerEnabledMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._SessionServiceRequestHandlerEnabledMenuItem.Text = "Session SRQ Handled"
        Me._SessionServiceRequestHandlerEnabledMenuItem.ToolTipText = "Check to handle Device service requests"
        '
        '_DeviceServiceRequestHandlerEnabledMenuItem
        '
        Me._DeviceServiceRequestHandlerEnabledMenuItem.CheckOnClick = True
        Me._DeviceServiceRequestHandlerEnabledMenuItem.Name = "_DeviceServiceRequestHandlerEnabledMenuItem"
        Me._DeviceServiceRequestHandlerEnabledMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._DeviceServiceRequestHandlerEnabledMenuItem.Text = "Device SRQ Handled"
        '
        '_ReadStatusByteMenuItem
        '
        Me._ReadStatusByteMenuItem.Name = "_ReadStatusByteMenuItem"
        Me._ReadStatusByteMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._ReadStatusByteMenuItem.Text = "Read Status Byte"
        Me._ReadStatusByteMenuItem.ToolTipText = "Reads the status byte"
        '
        '_LogTraceLevelComboBox
        '
        Me._LogTraceLevelComboBox.Name = "_LogTraceLevelComboBox"
        Me._LogTraceLevelComboBox.Size = New System.Drawing.Size(100, 22)
        Me._LogTraceLevelComboBox.Text = "Verbose"
        Me._LogTraceLevelComboBox.ToolTipText = "Log Trace Level"
        '
        '_DisplayTraceLevelComboBox
        '
        Me._DisplayTraceLevelComboBox.Name = "_DisplayTraceLevelComboBox"
        Me._DisplayTraceLevelComboBox.Size = New System.Drawing.Size(100, 22)
        Me._DisplayTraceLevelComboBox.Text = "Warning"
        Me._DisplayTraceLevelComboBox.ToolTipText = "Display trace level"
        '
        '_ReadTerminalStateButton
        '
        Me._ReadTerminalStateButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._ReadTerminalStateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ReadTerminalStateButton.Image = CType(resources.GetObject("_ReadTerminalStateButton.Image"), System.Drawing.Image)
        Me._ReadTerminalStateButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ReadTerminalStateButton.Name = "_ReadTerminalStateButton"
        Me._ReadTerminalStateButton.Size = New System.Drawing.Size(39, 25)
        Me._ReadTerminalStateButton.Text = "Front"
        Me._ReadTerminalStateButton.ToolTipText = "Reads terminal state"
        '
        '_ServiceRequestEnableNumericLabel
        '
        Me._ServiceRequestEnableNumericLabel.Name = "_ServiceRequestEnableNumericLabel"
        Me._ServiceRequestEnableNumericLabel.Size = New System.Drawing.Size(29, 25)
        Me._ServiceRequestEnableNumericLabel.Text = "SRE:"
        '
        '_ServiceRequestEnableBitmaskNumeric
        '
        Me._ServiceRequestEnableBitmaskNumeric.Name = "_ServiceRequestEnableBitmaskNumeric"
        Me._ServiceRequestEnableBitmaskNumeric.Size = New System.Drawing.Size(41, 25)
        Me._ServiceRequestEnableBitmaskNumeric.Text = "99"
        Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = "Service request enabled value"
        Me._ServiceRequestEnableBitmaskNumeric.Value = New Decimal(New Integer() {99, 0, 0, 0})
        '
        '_BufferToolStrip
        '
        Me._BufferToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ReadBufferButton, Me._BufferCountLabel, Me._LastPointNumberLabel, Me._FirstPointNumberLabel, Me._BufferNameLabel, Me._ClearBufferDisplayButton, Me._BufferSizeNumeric})
        Me._BufferToolStrip.Location = New System.Drawing.Point(0, 25)
        Me._BufferToolStrip.Name = "_BufferToolStrip"
        Me._BufferToolStrip.Size = New System.Drawing.Size(356, 28)
        Me._BufferToolStrip.TabIndex = 8
        Me._BufferToolStrip.Text = "Buffer"
        '
        '_ReadBufferButton
        '
        Me._ReadBufferButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ReadBufferButton.Image = CType(resources.GetObject("_ReadBufferButton.Image"), System.Drawing.Image)
        Me._ReadBufferButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ReadBufferButton.Name = "_ReadBufferButton"
        Me._ReadBufferButton.Size = New System.Drawing.Size(72, 25)
        Me._ReadBufferButton.Text = "Read Buffer"
        '
        '_BufferCountLabel
        '
        Me._BufferCountLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._BufferCountLabel.Name = "_BufferCountLabel"
        Me._BufferCountLabel.Size = New System.Drawing.Size(13, 25)
        Me._BufferCountLabel.Text = "0"
        Me._BufferCountLabel.ToolTipText = "Buffer count"
        '
        '_LastPointNumberLabel
        '
        Me._LastPointNumberLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._LastPointNumberLabel.Name = "_LastPointNumberLabel"
        Me._LastPointNumberLabel.Size = New System.Drawing.Size(13, 25)
        Me._LastPointNumberLabel.Text = "2"
        Me._LastPointNumberLabel.ToolTipText = "Number of last buffer reading"
        '
        '_FirstPointNumberLabel
        '
        Me._FirstPointNumberLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._FirstPointNumberLabel.Name = "_FirstPointNumberLabel"
        Me._FirstPointNumberLabel.Size = New System.Drawing.Size(13, 25)
        Me._FirstPointNumberLabel.Text = "1"
        Me._FirstPointNumberLabel.ToolTipText = "Number of the first buffer reading"
        '
        '_BufferNameLabel
        '
        Me._BufferNameLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._BufferNameLabel.Name = "_BufferNameLabel"
        Me._BufferNameLabel.Size = New System.Drawing.Size(62, 25)
        Me._BufferNameLabel.Text = "defbuffer1"
        '
        '_ClearBufferDisplayButton
        '
        Me._ClearBufferDisplayButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ClearBufferDisplayButton.Image = CType(resources.GetObject("_ClearBufferDisplayButton.Image"), System.Drawing.Image)
        Me._ClearBufferDisplayButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ClearBufferDisplayButton.Name = "_ClearBufferDisplayButton"
        Me._ClearBufferDisplayButton.Size = New System.Drawing.Size(38, 25)
        Me._ClearBufferDisplayButton.Text = "Clear"
        Me._ClearBufferDisplayButton.ToolTipText = "Clears the display"
        '
        '_BufferSizeNumeric
        '
        Me._BufferSizeNumeric.AutoSize = False
        Me._BufferSizeNumeric.Name = "_BufferSizeNumeric"
        Me._BufferSizeNumeric.Size = New System.Drawing.Size(71, 25)
        Me._BufferSizeNumeric.Text = "0"
        Me._BufferSizeNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        '
        '_ReadingToolStrip
        '
        Me._ReadingToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ReadButton, Me._ReadingComboBox, Me._AbortButton, Me._GoSplitButton})
        Me._ReadingToolStrip.Location = New System.Drawing.Point(0, 0)
        Me._ReadingToolStrip.Name = "_ReadingToolStrip"
        Me._ReadingToolStrip.Size = New System.Drawing.Size(356, 25)
        Me._ReadingToolStrip.TabIndex = 7
        Me._ReadingToolStrip.Text = "Reading Tool Strip"
        Me._TipsToolTip.SetToolTip(Me._ReadingToolStrip, "Readings management")
        '
        '_ReadButton
        '
        Me._ReadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ReadButton.Image = CType(resources.GetObject("_ReadButton.Image"), System.Drawing.Image)
        Me._ReadButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ReadButton.Name = "_ReadButton"
        Me._ReadButton.Size = New System.Drawing.Size(37, 22)
        Me._ReadButton.Text = "Read"
        '
        '_ReadingComboBox
        '
        Me._ReadingComboBox.Name = "_ReadingComboBox"
        Me._ReadingComboBox.Size = New System.Drawing.Size(121, 25)
        Me._ReadingComboBox.ToolTipText = "Select reading type to display"
        '
        '_AbortButton
        '
        Me._AbortButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._AbortButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._AbortButton.Image = CType(resources.GetObject("_AbortButton.Image"), System.Drawing.Image)
        Me._AbortButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._AbortButton.Name = "_AbortButton"
        Me._AbortButton.Size = New System.Drawing.Size(41, 22)
        Me._AbortButton.Text = "Abort"
        Me._AbortButton.ToolTipText = "Aborts triggering"
        '
        '_GoSplitButton
        '
        Me._GoSplitButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._GoSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._GoSplitButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._InitiateTriggerPlanMenuItem, Me._AbortStartTriggerPlanMenuItem, Me._MonitorActiveTriggerPlanMenuItem, Me._InitMonitorReadRepeatMenuItem, Me._RepeatMenuItem, Me._StreamBufferMenuItem})
        Me._GoSplitButton.Image = CType(resources.GetObject("_GoSplitButton.Image"), System.Drawing.Image)
        Me._GoSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._GoSplitButton.Name = "_GoSplitButton"
        Me._GoSplitButton.Size = New System.Drawing.Size(47, 22)
        Me._GoSplitButton.Text = "Go..."
        Me._GoSplitButton.ToolTipText = "Select initiation options"
        '
        '_InitiateTriggerPlanMenuItem
        '
        Me._InitiateTriggerPlanMenuItem.Name = "_InitiateTriggerPlanMenuItem"
        Me._InitiateTriggerPlanMenuItem.Size = New System.Drawing.Size(222, 22)
        Me._InitiateTriggerPlanMenuItem.Text = "Initiate"
        Me._InitiateTriggerPlanMenuItem.ToolTipText = "Sends the Initiate command to the instrument"
        '
        '_AbortStartTriggerPlanMenuItem
        '
        Me._AbortStartTriggerPlanMenuItem.Name = "_AbortStartTriggerPlanMenuItem"
        Me._AbortStartTriggerPlanMenuItem.Size = New System.Drawing.Size(222, 22)
        Me._AbortStartTriggerPlanMenuItem.Text = "Abort, Clear, Initiate"
        Me._AbortStartTriggerPlanMenuItem.ToolTipText = "Aborts, clears buffer and starts the trigger plan"
        '
        '_MonitorActiveTriggerPlanMenuItem
        '
        Me._MonitorActiveTriggerPlanMenuItem.Name = "_MonitorActiveTriggerPlanMenuItem"
        Me._MonitorActiveTriggerPlanMenuItem.Size = New System.Drawing.Size(222, 22)
        Me._MonitorActiveTriggerPlanMenuItem.Text = "Monitor Active Trigger State"
        Me._MonitorActiveTriggerPlanMenuItem.ToolTipText = "Monitors active trigger state. Exits if trigger plan inactive"
        '
        '_InitMonitorReadRepeatMenuItem
        '
        Me._InitMonitorReadRepeatMenuItem.Name = "_InitMonitorReadRepeatMenuItem"
        Me._InitMonitorReadRepeatMenuItem.Size = New System.Drawing.Size(222, 22)
        Me._InitMonitorReadRepeatMenuItem.Text = "Init, Monitor, Read, Repeat"
        Me._InitMonitorReadRepeatMenuItem.ToolTipText = "Initiates a trigger plan, monitors it, reads and displays buffer and repeats if a" &
    "uto repeat is checked"
        '
        '_RepeatMenuItem
        '
        Me._RepeatMenuItem.CheckOnClick = True
        Me._RepeatMenuItem.Name = "_RepeatMenuItem"
        Me._RepeatMenuItem.Size = New System.Drawing.Size(222, 22)
        Me._RepeatMenuItem.Text = "Repeat"
        Me._RepeatMenuItem.ToolTipText = "Repeat initiating the trigger plan"
        '
        '_StreamBufferMenuItem
        '
        Me._StreamBufferMenuItem.CheckOnClick = True
        Me._StreamBufferMenuItem.Name = "_StreamBufferMenuItem"
        Me._StreamBufferMenuItem.Size = New System.Drawing.Size(222, 22)
        Me._StreamBufferMenuItem.Text = "Stream Buffer"
        Me._StreamBufferMenuItem.ToolTipText = "Continuously reads new values while a trigger plan is active"
        '
        '_SenseTabPage
        '
        Me._SenseTabPage.Controls.Add(Me._OpenLeadsDetectionCheckBox)
        Me._SenseTabPage.Controls.Add(Me._ApplyFunctionModeButton)
        Me._SenseTabPage.Controls.Add(Me._SenseMeasureDelayNumeric)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeNumeric)
        Me._SenseTabPage.Controls.Add(Me._PowerLineCyclesNumeric)
        Me._SenseTabPage.Controls.Add(Me._TriggerDelayNumericLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeNumericLabel)
        Me._SenseTabPage.Controls.Add(Me._PowerLineCyclesNumericLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseFunctionComboBox)
        Me._SenseTabPage.Controls.Add(Me._SenseFunctionComboBoxLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseAutoRangeToggle)
        Me._SenseTabPage.Controls.Add(Me._ApplySenseSettingsButton)
        Me._SenseTabPage.Location = New System.Drawing.Point(4, 26)
        Me._SenseTabPage.Name = "_SenseTabPage"
        Me._SenseTabPage.Size = New System.Drawing.Size(356, 305)
        Me._SenseTabPage.TabIndex = 4
        Me._SenseTabPage.Text = "Sense"
        Me._SenseTabPage.UseVisualStyleBackColor = True
        '
        '_OpenLeadsDetectionCheckBox
        '
        Me._OpenLeadsDetectionCheckBox.AutoSize = True
        Me._OpenLeadsDetectionCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._OpenLeadsDetectionCheckBox.Location = New System.Drawing.Point(11, 115)
        Me._OpenLeadsDetectionCheckBox.Name = "_OpenLeadsDetectionCheckBox"
        Me._OpenLeadsDetectionCheckBox.Size = New System.Drawing.Size(109, 21)
        Me._OpenLeadsDetectionCheckBox.TabIndex = 11
        Me._OpenLeadsDetectionCheckBox.Text = "Detect Opens:"
        Me._OpenLeadsDetectionCheckBox.UseVisualStyleBackColor = True
        '
        '_ApplyFunctionModeButton
        '
        Me._ApplyFunctionModeButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplyFunctionModeButton.Location = New System.Drawing.Point(306, 12)
        Me._ApplyFunctionModeButton.Name = "_ApplyFunctionModeButton"
        Me._ApplyFunctionModeButton.Size = New System.Drawing.Size(39, 25)
        Me._ApplyFunctionModeButton.TabIndex = 10
        Me._ApplyFunctionModeButton.Text = "SET"
        Me._TipsToolTip.SetToolTip(Me._ApplyFunctionModeButton, "Selects this function mode")
        Me._ApplyFunctionModeButton.UseVisualStyleBackColor = True
        '
        '_SenseMeasureDelayNumeric
        '
        Me._SenseMeasureDelayNumeric.DecimalPlaces = 3
        Me._SenseMeasureDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SenseMeasureDelayNumeric.Location = New System.Drawing.Point(106, 141)
        Me._SenseMeasureDelayNumeric.Name = "_SenseMeasureDelayNumeric"
        Me._SenseMeasureDelayNumeric.Size = New System.Drawing.Size(76, 25)
        Me._SenseMeasureDelayNumeric.TabIndex = 8
        Me._TipsToolTip.SetToolTip(Me._SenseMeasureDelayNumeric, "Sense measure delay")
        Me._SenseMeasureDelayNumeric.Visible = False
        '
        '_SenseRangeNumeric
        '
        Me._SenseRangeNumeric.DecimalPlaces = 3
        Me._SenseRangeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SenseRangeNumeric.Location = New System.Drawing.Point(106, 80)
        Me._SenseRangeNumeric.Maximum = New Decimal(New Integer() {1010, 0, 0, 0})
        Me._SenseRangeNumeric.Name = "_SenseRangeNumeric"
        Me._SenseRangeNumeric.Size = New System.Drawing.Size(76, 25)
        Me._SenseRangeNumeric.TabIndex = 5
        Me._TipsToolTip.SetToolTip(Me._SenseRangeNumeric, "Range")
        Me._SenseRangeNumeric.Value = New Decimal(New Integer() {105, 0, 0, 196608})
        '
        '_PowerLineCyclesNumeric
        '
        Me._PowerLineCyclesNumeric.DecimalPlaces = 3
        Me._PowerLineCyclesNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PowerLineCyclesNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._PowerLineCyclesNumeric.Location = New System.Drawing.Point(106, 46)
        Me._PowerLineCyclesNumeric.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me._PowerLineCyclesNumeric.Minimum = New Decimal(New Integer() {5, 0, 0, 262144})
        Me._PowerLineCyclesNumeric.Name = "_PowerLineCyclesNumeric"
        Me._PowerLineCyclesNumeric.Size = New System.Drawing.Size(76, 25)
        Me._PowerLineCyclesNumeric.TabIndex = 3
        Me._TipsToolTip.SetToolTip(Me._PowerLineCyclesNumeric, "Integration period")
        Me._PowerLineCyclesNumeric.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        '_TriggerDelayNumericLabel
        '
        Me._TriggerDelayNumericLabel.AutoSize = True
        Me._TriggerDelayNumericLabel.Location = New System.Drawing.Point(43, 145)
        Me._TriggerDelayNumericLabel.Name = "_TriggerDelayNumericLabel"
        Me._TriggerDelayNumericLabel.Size = New System.Drawing.Size(61, 17)
        Me._TriggerDelayNumericLabel.TabIndex = 7
        Me._TriggerDelayNumericLabel.Text = "Delay [s]:"
        Me._TriggerDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._TriggerDelayNumericLabel.Visible = False
        '
        '_SenseRangeNumericLabel
        '
        Me._SenseRangeNumericLabel.AutoSize = True
        Me._SenseRangeNumericLabel.Location = New System.Drawing.Point(36, 84)
        Me._SenseRangeNumericLabel.Name = "_SenseRangeNumericLabel"
        Me._SenseRangeNumericLabel.Size = New System.Drawing.Size(68, 17)
        Me._SenseRangeNumericLabel.TabIndex = 4
        Me._SenseRangeNumericLabel.Text = "Range [V]:"
        Me._SenseRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_PowerLineCyclesNumericLabel
        '
        Me._PowerLineCyclesNumericLabel.AutoSize = True
        Me._PowerLineCyclesNumericLabel.Location = New System.Drawing.Point(6, 50)
        Me._PowerLineCyclesNumericLabel.Name = "_PowerLineCyclesNumericLabel"
        Me._PowerLineCyclesNumericLabel.Size = New System.Drawing.Size(98, 17)
        Me._PowerLineCyclesNumericLabel.TabIndex = 2
        Me._PowerLineCyclesNumericLabel.Text = "Aperture [nplc]:"
        Me._PowerLineCyclesNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_SenseFunctionComboBox
        '
        Me._SenseFunctionComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SenseFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._SenseFunctionComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseFunctionComboBox.Items.AddRange(New Object() {"I", "V"})
        Me._SenseFunctionComboBox.Location = New System.Drawing.Point(106, 12)
        Me._SenseFunctionComboBox.Name = "_SenseFunctionComboBox"
        Me._SenseFunctionComboBox.Size = New System.Drawing.Size(198, 25)
        Me._SenseFunctionComboBox.TabIndex = 1
        '
        '_SenseFunctionComboBoxLabel
        '
        Me._SenseFunctionComboBoxLabel.AutoSize = True
        Me._SenseFunctionComboBoxLabel.Location = New System.Drawing.Point(45, 16)
        Me._SenseFunctionComboBoxLabel.Name = "_SenseFunctionComboBoxLabel"
        Me._SenseFunctionComboBoxLabel.Size = New System.Drawing.Size(59, 17)
        Me._SenseFunctionComboBoxLabel.TabIndex = 0
        Me._SenseFunctionComboBoxLabel.Text = "Function:"
        Me._SenseFunctionComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseAutoRangeToggle
        '
        Me._SenseAutoRangeToggle.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseAutoRangeToggle.Location = New System.Drawing.Point(188, 82)
        Me._SenseAutoRangeToggle.Name = "_SenseAutoRangeToggle"
        Me._SenseAutoRangeToggle.Size = New System.Drawing.Size(103, 21)
        Me._SenseAutoRangeToggle.TabIndex = 6
        Me._SenseAutoRangeToggle.Text = "Auto Range"
        Me._SenseAutoRangeToggle.UseVisualStyleBackColor = True
        '
        '_ApplySenseSettingsButton
        '
        Me._ApplySenseSettingsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplySenseSettingsButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ApplySenseSettingsButton.Location = New System.Drawing.Point(283, 260)
        Me._ApplySenseSettingsButton.Name = "_ApplySenseSettingsButton"
        Me._ApplySenseSettingsButton.Size = New System.Drawing.Size(58, 30)
        Me._ApplySenseSettingsButton.TabIndex = 9
        Me._ApplySenseSettingsButton.Text = "&Apply"
        Me._ApplySenseSettingsButton.UseVisualStyleBackColor = True
        '
        '_TriggerTabPage
        '
        Me._TriggerTabPage.Controls.Add(Me._TriggerToolStripPanel)
        Me._TriggerTabPage.Location = New System.Drawing.Point(4, 26)
        Me._TriggerTabPage.Name = "_TriggerTabPage"
        Me._TriggerTabPage.Size = New System.Drawing.Size(356, 305)
        Me._TriggerTabPage.TabIndex = 6
        Me._TriggerTabPage.Text = "Trigger"
        Me._TriggerTabPage.UseVisualStyleBackColor = True
        '
        '_TriggerToolStripPanel
        '
        Me._TriggerToolStripPanel.Controls.Add(Me._TriggerDelayToolStrip)
        Me._TriggerToolStripPanel.Controls.Add(Me._Limit1ToolStrip)
        Me._TriggerToolStripPanel.Controls.Add(Me._GradeBinningToolStrip)
        Me._TriggerToolStripPanel.Controls.Add(Me._TriggerLayerToolStrip)
        Me._TriggerToolStripPanel.Controls.Add(Me._TriggerModelsToolStrip)
        Me._TriggerToolStripPanel.Location = New System.Drawing.Point(1, 0)
        Me._TriggerToolStripPanel.Name = "_TriggerToolStripPanel"
        Me._TriggerToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me._TriggerToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me._TriggerToolStripPanel.Size = New System.Drawing.Size(356, 198)
        '
        '_TriggerDelayToolStrip
        '
        Me._TriggerDelayToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._TriggerDelayToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._TriggerDelaysToolStripLabel, Me._StartTriggerDelayNumericLabel, Me._StartTriggerDelayNumeric, Me._EndTriggerDelayNumericLabel, Me._EndTriggerDelayNumeric})
        Me._TriggerDelayToolStrip.Location = New System.Drawing.Point(3, 0)
        Me._TriggerDelayToolStrip.Name = "_TriggerDelayToolStrip"
        Me._TriggerDelayToolStrip.Size = New System.Drawing.Size(239, 28)
        Me._TriggerDelayToolStrip.TabIndex = 1
        '
        '_TriggerDelaysToolStripLabel
        '
        Me._TriggerDelaysToolStripLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerDelaysToolStripLabel.Name = "_TriggerDelaysToolStripLabel"
        Me._TriggerDelaysToolStripLabel.Size = New System.Drawing.Size(43, 25)
        Me._TriggerDelaysToolStripLabel.Text = "DELAY"
        '
        '_StartTriggerDelayNumericLabel
        '
        Me._StartTriggerDelayNumericLabel.Name = "_StartTriggerDelayNumericLabel"
        Me._StartTriggerDelayNumericLabel.Size = New System.Drawing.Size(27, 25)
        Me._StartTriggerDelayNumericLabel.Text = "Pre:"
        '
        '_StartTriggerDelayNumeric
        '
        Me._StartTriggerDelayNumeric.AutoSize = False
        Me._StartTriggerDelayNumeric.Name = "_StartTriggerDelayNumeric"
        Me._StartTriggerDelayNumeric.Size = New System.Drawing.Size(62, 25)
        Me._StartTriggerDelayNumeric.Text = "0"
        Me._StartTriggerDelayNumeric.ToolTipText = "Start delay in seconds"
        Me._StartTriggerDelayNumeric.Value = New Decimal(New Integer() {20, 0, 0, 196608})
        '
        '_EndTriggerDelayNumericLabel
        '
        Me._EndTriggerDelayNumericLabel.Name = "_EndTriggerDelayNumericLabel"
        Me._EndTriggerDelayNumericLabel.Size = New System.Drawing.Size(33, 25)
        Me._EndTriggerDelayNumericLabel.Text = "Post:"
        '
        '_EndTriggerDelayNumeric
        '
        Me._EndTriggerDelayNumeric.AutoSize = False
        Me._EndTriggerDelayNumeric.Name = "_EndTriggerDelayNumeric"
        Me._EndTriggerDelayNumeric.Size = New System.Drawing.Size(62, 25)
        Me._EndTriggerDelayNumeric.Text = "0"
        Me._EndTriggerDelayNumeric.ToolTipText = "End delay in seconds"
        Me._EndTriggerDelayNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        '_Limit1ToolStrip
        '
        Me._Limit1ToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._Limit1ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._Limits1Label, Me._Limit1DecimalsNumeric, Me._LowerLimit1NumericLabel, Me._LowerLimit1Numeric, Me._UpperLimit1NumericLabel, Me._UpperLimit1Numeric})
        Me._Limit1ToolStrip.Location = New System.Drawing.Point(3, 28)
        Me._Limit1ToolStrip.Name = "_Limit1ToolStrip"
        Me._Limit1ToolStrip.Size = New System.Drawing.Size(260, 28)
        Me._Limit1ToolStrip.TabIndex = 2
        '
        '_Limits1Label
        '
        Me._Limits1Label.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Limits1Label.Name = "_Limits1Label"
        Me._Limits1Label.Size = New System.Drawing.Size(44, 25)
        Me._Limits1Label.Text = "LIMIT1:"
        '
        '_Limit1DecimalsNumeric
        '
        Me._Limit1DecimalsNumeric.Name = "_Limit1DecimalsNumeric"
        Me._Limit1DecimalsNumeric.Size = New System.Drawing.Size(41, 25)
        Me._Limit1DecimalsNumeric.Text = "3"
        Me._Limit1DecimalsNumeric.ToolTipText = "Number of decimal places for setting the limits"
        Me._Limit1DecimalsNumeric.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        '_LowerLimit1NumericLabel
        '
        Me._LowerLimit1NumericLabel.Name = "_LowerLimit1NumericLabel"
        Me._LowerLimit1NumericLabel.Size = New System.Drawing.Size(42, 25)
        Me._LowerLimit1NumericLabel.Text = "Lower:"
        '
        '_LowerLimit1Numeric
        '
        Me._LowerLimit1Numeric.Name = "_LowerLimit1Numeric"
        Me._LowerLimit1Numeric.Size = New System.Drawing.Size(41, 25)
        Me._LowerLimit1Numeric.Text = "0"
        Me._LowerLimit1Numeric.ToolTipText = "Lower limit"
        Me._LowerLimit1Numeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        '
        '_UpperLimit1NumericLabel
        '
        Me._UpperLimit1NumericLabel.Name = "_UpperLimit1NumericLabel"
        Me._UpperLimit1NumericLabel.Size = New System.Drawing.Size(39, 25)
        Me._UpperLimit1NumericLabel.Text = "Upper"
        '
        '_UpperLimit1Numeric
        '
        Me._UpperLimit1Numeric.Name = "_UpperLimit1Numeric"
        Me._UpperLimit1Numeric.Size = New System.Drawing.Size(41, 25)
        Me._UpperLimit1Numeric.Text = "0"
        Me._UpperLimit1Numeric.ToolTipText = "Upper limit"
        Me._UpperLimit1Numeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        '
        '_GradeBinningToolStrip
        '
        Me._GradeBinningToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._GradeBinningToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._BinningToolStripLabel, Me._PassBitPatternNumericButton, Me._PassBitPatternNumeric, Me._OpenLeadsBitPatternNumericButton, Me._OpenLeadsBitPatternNumeric, Me._FailBitPatternNumericButton, Me._FailLimit1BitPatternNumeric})
        Me._GradeBinningToolStrip.Location = New System.Drawing.Point(3, 56)
        Me._GradeBinningToolStrip.Name = "_GradeBinningToolStrip"
        Me._GradeBinningToolStrip.Size = New System.Drawing.Size(306, 28)
        Me._GradeBinningToolStrip.TabIndex = 0
        '
        '_BinningToolStripLabel
        '
        Me._BinningToolStripLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._BinningToolStripLabel.Name = "_BinningToolStripLabel"
        Me._BinningToolStripLabel.Size = New System.Drawing.Size(26, 25)
        Me._BinningToolStripLabel.Text = "BIN"
        '
        '_PassBitPatternNumericButton
        '
        Me._PassBitPatternNumericButton.CheckOnClick = True
        Me._PassBitPatternNumericButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._PassBitPatternNumericButton.Image = CType(resources.GetObject("_PassBitPatternNumericButton.Image"), System.Drawing.Image)
        Me._PassBitPatternNumericButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._PassBitPatternNumericButton.Name = "_PassBitPatternNumericButton"
        Me._PassBitPatternNumericButton.Size = New System.Drawing.Size(48, 25)
        Me._PassBitPatternNumericButton.Text = "Pass 0x"
        '
        '_PassBitPatternNumeric
        '
        Me._PassBitPatternNumeric.Name = "_PassBitPatternNumeric"
        Me._PassBitPatternNumeric.Size = New System.Drawing.Size(41, 25)
        Me._PassBitPatternNumeric.Text = "2"
        Me._PassBitPatternNumeric.ToolTipText = "Pass bit pattern"
        Me._PassBitPatternNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        '_OpenLeadsBitPatternNumericButton
        '
        Me._OpenLeadsBitPatternNumericButton.CheckOnClick = True
        Me._OpenLeadsBitPatternNumericButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._OpenLeadsBitPatternNumericButton.Image = CType(resources.GetObject("_OpenLeadsBitPatternNumericButton.Image"), System.Drawing.Image)
        Me._OpenLeadsBitPatternNumericButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._OpenLeadsBitPatternNumericButton.Name = "_OpenLeadsBitPatternNumericButton"
        Me._OpenLeadsBitPatternNumericButton.Size = New System.Drawing.Size(54, 25)
        Me._OpenLeadsBitPatternNumericButton.Text = "Open 0x"
        '
        '_OpenLeadsBitPatternNumeric
        '
        Me._OpenLeadsBitPatternNumeric.Name = "_OpenLeadsBitPatternNumeric"
        Me._OpenLeadsBitPatternNumeric.Size = New System.Drawing.Size(41, 25)
        Me._OpenLeadsBitPatternNumeric.Text = "0"
        Me._OpenLeadsBitPatternNumeric.ToolTipText = "Open leads bit pattern"
        Me._OpenLeadsBitPatternNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        '
        '_FailBitPatternNumericButton
        '
        Me._FailBitPatternNumericButton.CheckOnClick = True
        Me._FailBitPatternNumericButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._FailBitPatternNumericButton.Image = CType(resources.GetObject("_FailBitPatternNumericButton.Image"), System.Drawing.Image)
        Me._FailBitPatternNumericButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._FailBitPatternNumericButton.Name = "_FailBitPatternNumericButton"
        Me._FailBitPatternNumericButton.Size = New System.Drawing.Size(43, 25)
        Me._FailBitPatternNumericButton.Text = "Fail 0x"
        '
        '_FailLimit1BitPatternNumeric
        '
        Me._FailLimit1BitPatternNumeric.Name = "_FailLimit1BitPatternNumeric"
        Me._FailLimit1BitPatternNumeric.Size = New System.Drawing.Size(41, 25)
        Me._FailLimit1BitPatternNumeric.Text = "1"
        Me._FailLimit1BitPatternNumeric.ToolTipText = "Failed bit pattern"
        Me._FailLimit1BitPatternNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        '_TriggerLayerToolStrip
        '
        Me._TriggerLayerToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._TriggerLayerToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._TriggerLayerToolStripLabel, Me._TriggerCountNumericLabel, Me._TriggerCountNumeric, Me._TriggerSourceComboBox, Me._TriggerSplitButton})
        Me._TriggerLayerToolStrip.Location = New System.Drawing.Point(3, 84)
        Me._TriggerLayerToolStrip.Name = "_TriggerLayerToolStrip"
        Me._TriggerLayerToolStrip.Size = New System.Drawing.Size(259, 28)
        Me._TriggerLayerToolStrip.TabIndex = 7
        Me._TriggerLayerToolStrip.Text = "ToolStrip1"
        '
        '_TriggerLayerToolStripLabel
        '
        Me._TriggerLayerToolStripLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerLayerToolStripLabel.Name = "_TriggerLayerToolStripLabel"
        Me._TriggerLayerToolStripLabel.Size = New System.Drawing.Size(34, 25)
        Me._TriggerLayerToolStripLabel.Text = "TRIG:"
        '
        '_TriggerCountNumericLabel
        '
        Me._TriggerCountNumericLabel.Name = "_TriggerCountNumericLabel"
        Me._TriggerCountNumericLabel.Size = New System.Drawing.Size(19, 25)
        Me._TriggerCountNumericLabel.Text = "N:"
        '
        '_TriggerCountNumeric
        '
        Me._TriggerCountNumeric.Name = "_TriggerCountNumeric"
        Me._TriggerCountNumeric.Size = New System.Drawing.Size(41, 25)
        Me._TriggerCountNumeric.Text = "0"
        Me._TriggerCountNumeric.ToolTipText = "Trigger Count"
        Me._TriggerCountNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        '
        '_TriggerSourceComboBox
        '
        Me._TriggerSourceComboBox.Name = "_TriggerSourceComboBox"
        Me._TriggerSourceComboBox.Size = New System.Drawing.Size(91, 28)
        Me._TriggerSourceComboBox.Text = "Immediate"
        Me._TriggerSourceComboBox.ToolTipText = "Trigger Source"
        '
        '_TriggerSplitButton
        '
        Me._TriggerSplitButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ContinuousTriggerEnabledMenuItem, Me._ReadTriggerStateMenuItem})
        Me._TriggerSplitButton.Name = "_TriggerSplitButton"
        Me._TriggerSplitButton.Size = New System.Drawing.Size(60, 25)
        Me._TriggerSplitButton.Text = "More..."
        Me._TriggerSplitButton.ToolTipText = "Continuous On/Off"
        '
        '_ContinuousTriggerEnabledMenuItem
        '
        Me._ContinuousTriggerEnabledMenuItem.CheckOnClick = True
        Me._ContinuousTriggerEnabledMenuItem.Name = "_ContinuousTriggerEnabledMenuItem"
        Me._ContinuousTriggerEnabledMenuItem.Size = New System.Drawing.Size(136, 22)
        Me._ContinuousTriggerEnabledMenuItem.Text = "Continuous"
        Me._ContinuousTriggerEnabledMenuItem.ToolTipText = "Check to enable"
        '
        '_ReadTriggerStateMenuItem
        '
        Me._ReadTriggerStateMenuItem.Name = "_ReadTriggerStateMenuItem"
        Me._ReadTriggerStateMenuItem.Size = New System.Drawing.Size(136, 22)
        Me._ReadTriggerStateMenuItem.Text = "Read State"
        Me._ReadTriggerStateMenuItem.ToolTipText = "Reads trigger state"
        '
        '_TriggerModelsToolStrip
        '
        Me._TriggerModelsToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._TriggerModelsToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._TriggerModelLabel, Me._ApplyTriggerModelSplitButton})
        Me._TriggerModelsToolStrip.Location = New System.Drawing.Point(5, 112)
        Me._TriggerModelsToolStrip.Name = "_TriggerModelsToolStrip"
        Me._TriggerModelsToolStrip.Size = New System.Drawing.Size(120, 25)
        Me._TriggerModelsToolStrip.TabIndex = 3
        Me._TipsToolTip.SetToolTip(Me._TriggerModelsToolStrip, "Simple trigger loop")
        '
        '_TriggerModelLabel
        '
        Me._TriggerModelLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerModelLabel.Name = "_TriggerModelLabel"
        Me._TriggerModelLabel.Size = New System.Drawing.Size(45, 22)
        Me._TriggerModelLabel.Text = "APPLY:"
        '
        '_ApplyTriggerModelSplitButton
        '
        Me._ApplyTriggerModelSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ApplyTriggerModelSplitButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._LoadGradeBinTriggerModelMenuItem, Me._LoadSimpleLoopMenuItem, Me._RunSimpleLoopMenuItem, Me._ClearTriggerModelMenuItem, Me._MeterCompleterFirstGradingBinningMenuItem})
        Me._ApplyTriggerModelSplitButton.Image = CType(resources.GetObject("_ApplyTriggerModelSplitButton.Image"), System.Drawing.Image)
        Me._ApplyTriggerModelSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ApplyTriggerModelSplitButton.Name = "_ApplyTriggerModelSplitButton"
        Me._ApplyTriggerModelSplitButton.Size = New System.Drawing.Size(63, 22)
        Me._ApplyTriggerModelSplitButton.Text = "Select..."
        Me._ApplyTriggerModelSplitButton.ToolTipText = "Select option:"
        '
        '_LoadGradeBinTriggerModelMenuItem
        '
        Me._LoadGradeBinTriggerModelMenuItem.Name = "_LoadGradeBinTriggerModelMenuItem"
        Me._LoadGradeBinTriggerModelMenuItem.Size = New System.Drawing.Size(353, 22)
        Me._LoadGradeBinTriggerModelMenuItem.Text = "Grading/Binning Trigger Loop"
        Me._LoadGradeBinTriggerModelMenuItem.ToolTipText = "Loads the grading binning trigger model"
        '
        '_LoadSimpleLoopMenuItem
        '
        Me._LoadSimpleLoopMenuItem.Name = "_LoadSimpleLoopMenuItem"
        Me._LoadSimpleLoopMenuItem.Size = New System.Drawing.Size(353, 22)
        Me._LoadSimpleLoopMenuItem.Text = "Load Simple Loop"
        Me._LoadSimpleLoopMenuItem.ToolTipText = "Loads a simple loop trigger model."
        '
        '_RunSimpleLoopMenuItem
        '
        Me._RunSimpleLoopMenuItem.Name = "_RunSimpleLoopMenuItem"
        Me._RunSimpleLoopMenuItem.Size = New System.Drawing.Size(353, 22)
        Me._RunSimpleLoopMenuItem.Text = "Run Simple Loop"
        Me._RunSimpleLoopMenuItem.ToolTipText = "Initiates a single loop, waits for completion and displays the values"
        '
        '_ClearTriggerModelMenuItem
        '
        Me._ClearTriggerModelMenuItem.Name = "_ClearTriggerModelMenuItem"
        Me._ClearTriggerModelMenuItem.Size = New System.Drawing.Size(353, 22)
        Me._ClearTriggerModelMenuItem.Text = "Clear Trigger Model"
        Me._ClearTriggerModelMenuItem.ToolTipText = "Clears the instrument trigger model"
        '
        '_MeterCompleterFirstGradingBinningMenuItem
        '
        Me._MeterCompleterFirstGradingBinningMenuItem.Name = "_MeterCompleterFirstGradingBinningMenuItem"
        Me._MeterCompleterFirstGradingBinningMenuItem.Size = New System.Drawing.Size(353, 22)
        Me._MeterCompleterFirstGradingBinningMenuItem.Text = "Meter Complete First Grading/Binning Trigger Model"
        Me._MeterCompleterFirstGradingBinningMenuItem.ToolTipText = "Applies a trigger model where meter complete is output first."
        '
        '_ReadWriteTabPage
        '
        Me._ReadWriteTabPage.Controls.Add(Me._SimpleReadWriteControl)
        Me._ReadWriteTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadWriteTabPage.Name = "_ReadWriteTabPage"
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(356, 243)
        Me._ReadWriteTabPage.TabIndex = 5
        Me._ReadWriteTabPage.Text = "R/W"
        Me._ReadWriteTabPage.UseVisualStyleBackColor = True
        '
        '_SimpleReadWriteControl
        '
        Me._SimpleReadWriteControl.AutoAppendTermination = Nothing
        Me._SimpleReadWriteControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me._SimpleReadWriteControl.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SimpleReadWriteControl.Location = New System.Drawing.Point(0, 0)
        Me._SimpleReadWriteControl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SimpleReadWriteControl.Name = "_SimpleReadWriteControl"
        Me._SimpleReadWriteControl.ReadEnabled = False
        Me._SimpleReadWriteControl.Size = New System.Drawing.Size(356, 243)
        Me._SimpleReadWriteControl.TabIndex = 0
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me._TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(356, 243)
        Me._MessagesTabPage.TabIndex = 3
        Me._MessagesTabPage.Text = "Log"
        Me._MessagesTabPage.UseVisualStyleBackColor = True
        '
        '_TraceMessagesBox
        '
        Me._TraceMessagesBox.AlertLevel = System.Diagnostics.TraceEventType.Warning
        Me._TraceMessagesBox.BackColor = System.Drawing.SystemColors.Info
        Me._TraceMessagesBox.CaptionFormat = "{0} ≡"
        Me._TraceMessagesBox.CausesValidation = False
        Me._TraceMessagesBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me._TraceMessagesBox.Font = New System.Drawing.Font("Consolas", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TraceMessagesBox.Location = New System.Drawing.Point(0, 0)
        Me._TraceMessagesBox.Multiline = True
        Me._TraceMessagesBox.Name = "_TraceMessagesBox"
        Me._TraceMessagesBox.PresetCount = 500
        Me._TraceMessagesBox.ReadOnly = True
        Me._TraceMessagesBox.ResetCount = 1000
        Me._TraceMessagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._TraceMessagesBox.Size = New System.Drawing.Size(356, 243)
        Me._TraceMessagesBox.TabIndex = 1
        '
        '_ResourceSelectorConnector
        '
        Me._ResourceSelectorConnector.BackColor = System.Drawing.Color.Transparent
        Me._ResourceSelectorConnector.ClearToolTipText = "Clear device state"
        Me._ResourceSelectorConnector.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ResourceSelectorConnector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ResourceSelectorConnector.Location = New System.Drawing.Point(0, 281)
        Me._ResourceSelectorConnector.Margin = New System.Windows.Forms.Padding(0)
        Me._ResourceSelectorConnector.Name = "_ResourceSelectorConnector"
        Me._ResourceSelectorConnector.Size = New System.Drawing.Size(356, 29)
        Me._ResourceSelectorConnector.TabIndex = 0
        '
        '_StatusStrip
        '
        Me._StatusStrip.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusLabel, Me._IdentityLabel})
        Me._StatusStrip.Location = New System.Drawing.Point(0, 411)
        Me._StatusStrip.Name = "_StatusStrip"
        Me._StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
        Me._StatusStrip.ShowItemToolTips = True
        Me._StatusStrip.Size = New System.Drawing.Size(364, 22)
        Me._StatusStrip.TabIndex = 15
        Me._StatusStrip.Text = "StatusStrip1"
        '
        '_StatusLabel
        '
        Me._StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._StatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._StatusLabel.Name = "_StatusLabel"
        Me._StatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._StatusLabel.Size = New System.Drawing.Size(317, 17)
        Me._StatusLabel.Spring = True
        Me._StatusLabel.Text = "<Status>"
        Me._StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._StatusLabel.ToolTipText = "Status"
        '
        '_IdentityLabel
        '
        Me._IdentityLabel.Name = "_IdentityLabel"
        Me._IdentityLabel.Size = New System.Drawing.Size(30, 17)
        Me._IdentityLabel.Text = "<I>"
        Me._IdentityLabel.ToolTipText = "Identity"
        '
        '_StatusRegisterLabel
        '
        Me._StatusRegisterLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._StatusRegisterLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusRegisterLabel.ForeColor = System.Drawing.Color.LightSkyBlue
        Me._StatusRegisterLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusRegisterLabel.Name = "_StatusRegisterLabel"
        Me._StatusRegisterLabel.Size = New System.Drawing.Size(36, 42)
        Me._StatusRegisterLabel.Text = "0x00"
        Me._StatusRegisterLabel.ToolTipText = "Status Register Value"
        '
        '_StandardRegisterLabel
        '
        Me._StandardRegisterLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._StandardRegisterLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StandardRegisterLabel.ForeColor = System.Drawing.Color.LightSkyBlue
        Me._StandardRegisterLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._StandardRegisterLabel.Name = "_StandardRegisterLabel"
        Me._StandardRegisterLabel.Size = New System.Drawing.Size(36, 42)
        Me._StandardRegisterLabel.Text = "0x00"
        Me._StandardRegisterLabel.ToolTipText = "Standard Register Value"
        '
        '_TimingLabel
        '
        Me._TimingLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TimingLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TimingLabel.ForeColor = System.Drawing.Color.LightSkyBlue
        Me._TimingLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._TimingLabel.Name = "_TimingLabel"
        Me._TimingLabel.Size = New System.Drawing.Size(0, 0)
        Me._TimingLabel.ToolTipText = "Elapsed time, ms"
        '
        '_InfoProvider
        '
        Me._InfoProvider.ContainerControl = Me
        '
        '_LastErrorTextBox
        '
        Me._LastErrorTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._LastErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._LastErrorTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._LastErrorTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._LastErrorTextBox.ForeColor = System.Drawing.Color.OrangeRed
        Me._LastErrorTextBox.Location = New System.Drawing.Point(0, 58)
        Me._LastErrorTextBox.Name = "_LastErrorTextBox"
        Me._LastErrorTextBox.Size = New System.Drawing.Size(364, 18)
        Me._LastErrorTextBox.TabIndex = 2
        Me._LastErrorTextBox.Text = "000, No Errors"
        '
        '_ReadingStatusStrip
        '
        Me._ReadingStatusStrip.BackColor = System.Drawing.Color.Black
        Me._ReadingStatusStrip.Dock = System.Windows.Forms.DockStyle.Top
        Me._ReadingStatusStrip.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ReadingStatusStrip.GripMargin = New System.Windows.Forms.Padding(0)
        Me._ReadingStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._FailureToolStripStatusLabel, Me._ReadingToolStripStatusLabel, Me._StatusRegisterLabel, Me._StandardRegisterLabel, Me._TimingLabel})
        Me._ReadingStatusStrip.Location = New System.Drawing.Point(0, 0)
        Me._ReadingStatusStrip.Name = "_ReadingStatusStrip"
        Me._ReadingStatusStrip.Size = New System.Drawing.Size(364, 42)
        Me._ReadingStatusStrip.SizingGrip = False
        Me._ReadingStatusStrip.TabIndex = 1
        '
        '_FailureToolStripStatusLabel
        '
        Me._FailureToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._FailureToolStripStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._FailureToolStripStatusLabel.ForeColor = System.Drawing.Color.Red
        Me._FailureToolStripStatusLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._FailureToolStripStatusLabel.Name = "_FailureToolStripStatusLabel"
        Me._FailureToolStripStatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._FailureToolStripStatusLabel.Size = New System.Drawing.Size(16, 42)
        Me._FailureToolStripStatusLabel.Text = "C"
        Me._FailureToolStripStatusLabel.ToolTipText = "Compliance"
        '
        '_ReadingToolStripStatusLabel
        '
        Me._ReadingToolStripStatusLabel.AutoSize = False
        Me._ReadingToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ReadingToolStripStatusLabel.ForeColor = System.Drawing.Color.Aquamarine
        Me._ReadingToolStripStatusLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._ReadingToolStripStatusLabel.Name = "_ReadingToolStripStatusLabel"
        Me._ReadingToolStripStatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._ReadingToolStripStatusLabel.Size = New System.Drawing.Size(261, 42)
        Me._ReadingToolStripStatusLabel.Spring = True
        Me._ReadingToolStripStatusLabel.Text = "0.0000000 mV"
        Me._ReadingToolStripStatusLabel.ToolTipText = "Reading"
        '
        '_TriggerStateLabel
        '
        Me._TriggerStateLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TriggerStateLabel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerStateLabel.ForeColor = System.Drawing.SystemColors.Info
        Me._TriggerStateLabel.Name = "_TriggerStateLabel"
        Me._TriggerStateLabel.Size = New System.Drawing.Size(35, 37)
        Me._TriggerStateLabel.Text = "None"
        Me._TriggerStateLabel.ToolTipText = "Trigger state"
        '
        '_LastReadingTextBox
        '
        Me._LastReadingTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._LastReadingTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._LastReadingTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._LastReadingTextBox.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._LastReadingTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._LastReadingTextBox.Location = New System.Drawing.Point(0, 42)
        Me._LastReadingTextBox.Name = "_LastReadingTextBox"
        Me._LastReadingTextBox.Size = New System.Drawing.Size(364, 16)
        Me._LastReadingTextBox.TabIndex = 0
        Me._LastReadingTextBox.Text = "<last reading>"
        Me._LastReadingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_Panel
        '
        Me._Panel.Controls.Add(Me._Tabs)
        Me._Panel.Controls.Add(Me._LastErrorTextBox)
        Me._Panel.Controls.Add(Me._LastReadingTextBox)
        Me._Panel.Controls.Add(Me._ReadingStatusStrip)
        Me._Panel.Controls.Add(Me._StatusStrip)
        Me._Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Panel.Location = New System.Drawing.Point(0, 17)
        Me._Panel.Margin = New System.Windows.Forms.Padding(0)
        Me._Panel.Name = "_Panel"
        Me._Panel.Size = New System.Drawing.Size(364, 433)
        Me._Panel.TabIndex = 16
        '
        '_Layout
        '
        Me._Layout.ColumnCount = 1
        Me._Layout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._Layout.Controls.Add(Me._TitleLabel, 0, 0)
        Me._Layout.Controls.Add(Me._Panel, 0, 1)
        Me._Layout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Layout.Location = New System.Drawing.Point(0, 0)
        Me._Layout.Name = "_Layout"
        Me._Layout.RowCount = 2
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._Layout.Size = New System.Drawing.Size(364, 450)
        Me._Layout.TabIndex = 17
        '
        '_TitleLabel
        '
        Me._TitleLabel.BackColor = System.Drawing.Color.Black
        Me._TitleLabel.CausesValidation = False
        Me._TitleLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me._TitleLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TitleLabel.ForeColor = System.Drawing.SystemColors.Info
        Me._TitleLabel.Location = New System.Drawing.Point(0, 0)
        Me._TitleLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._TitleLabel.Name = "_TitleLabel"
        Me._TitleLabel.Size = New System.Drawing.Size(364, 17)
        Me._TitleLabel.TabIndex = 17
        Me._TitleLabel.Text = "K7500"
        Me._TitleLabel.UseMnemonic = False
        '
        'K7500Control
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "K7500Control"
        Me.Size = New System.Drawing.Size(364, 450)
        Me._Tabs.ResumeLayout(False)
        Me._ReadingTabPage.ResumeLayout(False)
        Me._ReadingTabPage.PerformLayout()
        CType(Me._BufferDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ToolStripPanel.ResumeLayout(False)
        Me._ToolStripPanel.PerformLayout()
        Me._SystemToolStrip.ResumeLayout(False)
        Me._SystemToolStrip.PerformLayout()
        Me._BufferToolStrip.ResumeLayout(False)
        Me._BufferToolStrip.PerformLayout()
        Me._ReadingToolStrip.ResumeLayout(False)
        Me._ReadingToolStrip.PerformLayout()
        Me._SenseTabPage.ResumeLayout(False)
        Me._SenseTabPage.PerformLayout()
        CType(Me._SenseMeasureDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._PowerLineCyclesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._TriggerTabPage.ResumeLayout(False)
        Me._TriggerTabPage.PerformLayout()
        Me._TriggerToolStripPanel.ResumeLayout(False)
        Me._TriggerToolStripPanel.PerformLayout()
        Me._TriggerDelayToolStrip.ResumeLayout(False)
        Me._TriggerDelayToolStrip.PerformLayout()
        Me._Limit1ToolStrip.ResumeLayout(False)
        Me._Limit1ToolStrip.PerformLayout()
        Me._GradeBinningToolStrip.ResumeLayout(False)
        Me._GradeBinningToolStrip.PerformLayout()
        Me._TriggerLayerToolStrip.ResumeLayout(False)
        Me._TriggerLayerToolStrip.PerformLayout()
        Me._TriggerModelsToolStrip.ResumeLayout(False)
        Me._TriggerModelsToolStrip.PerformLayout()
        Me._ReadWriteTabPage.ResumeLayout(False)
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._StatusStrip.ResumeLayout(False)
        Me._StatusStrip.PerformLayout()
        CType(Me._InfoProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ReadingStatusStrip.ResumeLayout(False)
        Me._ReadingStatusStrip.PerformLayout()
        Me._Panel.ResumeLayout(False)
        Me._Panel.PerformLayout()
        Me._Layout.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _ReadingTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ApplySenseSettingsButton As System.Windows.Forms.Button
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseAutoRangeToggle As System.Windows.Forms.CheckBox
    Private WithEvents _SenseFunctionComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _SenseFunctionComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _PowerLineCyclesNumericLabel As System.Windows.Forms.Label
    Private WithEvents _SenseRangeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _TriggerDelayNumericLabel As System.Windows.Forms.Label
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _LastErrorTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ReadingStatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _FailureToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ReadingToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _LastReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _SenseMeasureDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _SenseRangeNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _PowerLineCyclesNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TitleLabel As System.Windows.Forms.Label
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
    Private WithEvents _TriggerTabPage As Windows.Forms.TabPage
    Private WithEvents _TriggerToolStripPanel As Windows.Forms.ToolStripPanel
    Private WithEvents _GradeBinningToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _BinningToolStripLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _TriggerDelayToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _TriggerDelaysToolStripLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _StartTriggerDelayNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _EndTriggerDelayNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _PassBitPatternNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _Limit1ToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _Limits1Label As Windows.Forms.ToolStripLabel
    Private WithEvents _ApplyFunctionModeButton As Windows.Forms.Button
    Private WithEvents _Limit1DecimalsNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _LowerLimit1Numeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _UpperLimit1Numeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _TriggerModelsToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _TriggerModelLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _BufferDataGridView As Windows.Forms.DataGridView
    Private WithEvents _BufferToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ReadBufferButton As Windows.Forms.ToolStripButton
    Private WithEvents _BufferCountLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _LastPointNumberLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _FirstPointNumberLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _BufferNameLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ReadingToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ReadButton As Windows.Forms.ToolStripButton
    Private WithEvents _ReadingComboBox As Windows.Forms.ToolStripComboBox
    Private WithEvents _ClearBufferDisplayButton As Windows.Forms.ToolStripButton
    Private WithEvents _AbortButton As Windows.Forms.ToolStripButton
    Private WithEvents _OpenLeadsDetectionCheckBox As Windows.Forms.CheckBox
    Private WithEvents _StartTriggerDelayNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _EndTriggerDelayNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _OpenLeadsBitPatternNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _SystemToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ResetSplitButton As Windows.Forms.ToolStripSplitButton
    Private WithEvents _ClearInterfaceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearDeviceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ReadStatusByteMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ResetKnownStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _InitKnownStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearExecutionStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ResetMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ServiceRequestHandlersEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _TraceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _DisplayTraceLevelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _LogTraceLevelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _SessionNotificationLevelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _SessionNotificationLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _SessionServiceRequestHandlerEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _DeviceServiceRequestHandlerEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ReadTerminalStateButton As Windows.Forms.ToolStripButton
    Private WithEvents _ServiceRequestEnableNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ServiceRequestEnableBitmaskNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _OpenLeadsBitPatternNumericButton As Windows.Forms.ToolStripButton
    Private WithEvents _PassBitPatternNumericButton As Windows.Forms.ToolStripButton
    Private WithEvents _TriggerLayerToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _TriggerLayerToolStripLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _TriggerCountNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _TriggerCountNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _TriggerSourceComboBox As Windows.Forms.ToolStripComboBox
    Private WithEvents _TriggerSplitButton As Windows.Forms.ToolStripSplitButton
    Private WithEvents _ContinuousTriggerEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _LowerLimit1NumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _UpperLimit1NumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _FailBitPatternNumericButton As Windows.Forms.ToolStripButton
    Private WithEvents _FailLimit1BitPatternNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _ApplyTriggerModelSplitButton As Windows.Forms.ToolStripSplitButton
    Private WithEvents _LoadGradeBinTriggerModelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _LoadSimpleLoopMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _RunSimpleLoopMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearTriggerModelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _GoSplitButton As Windows.Forms.ToolStripSplitButton
    Private WithEvents _AbortStartTriggerPlanMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _InitiateTriggerPlanMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _TriggerStateLabel As Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ReadTriggerStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _MonitorActiveTriggerPlanMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _InitMonitorReadRepeatMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _RepeatMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _MeterCompleterFirstGradingBinningMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _StreamBufferMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _BufferSizeNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _LogTraceLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _DisplayTraceLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _ToolStripPanel As Windows.Forms.ToolStripPanel
    Private WithEvents _ResourceSelectorConnector As VI.Instrument.ResourceSelectorConnector
    Private WithEvents _TraceMessagesBox As isr.Core.Pith.TraceMessagesBox
    Private WithEvents _InfoProvider As isr.Core.Controls.InfoProvider
    Private WithEvents _TipsToolTip As Windows.Forms.ToolTip
    Private WithEvents _StatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _IdentityLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _StatusRegisterLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _StandardRegisterLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _TimingLabel As System.Windows.Forms.ToolStripStatusLabel
End Class
