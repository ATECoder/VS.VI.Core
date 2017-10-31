<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class K34980Panel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(K34980Panel))
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
        Me._ReadingsDataGridView = New System.Windows.Forms.DataGridView()
        Me._ReadingToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ReadButton = New System.Windows.Forms.ToolStripButton()
        Me._InitiateButton = New System.Windows.Forms.ToolStripButton()
        Me._TraceButton = New System.Windows.Forms.ToolStripButton()
        Me._ReadingsCountLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ReadingComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me._AbortButton = New System.Windows.Forms.ToolStripButton()
        Me._ClearBufferDisplayButton = New System.Windows.Forms.ToolStripButton()
        Me._SystemToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ResetSplitButton = New System.Windows.Forms.ToolStripSplitButton()
        Me._ClearInterfaceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearDeviceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ResetKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._InitKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearExecutionStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SessionTraceEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SessionServiceRequestHandlerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._DeviceServiceRequestHandlerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._LogTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._DisplayTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._ReadTerminalStateButton = New System.Windows.Forms.ToolStripButton()
        Me._ServiceRequestEnableBitmaskNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ServiceRequestEnableBitmaskNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._SenseTabPage = New System.Windows.Forms.TabPage()
        Me._ApplyFunctionModeButton = New System.Windows.Forms.Button()
        Me._TriggerDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me._SenseRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me._PowerLineCyclesNumeric = New System.Windows.Forms.NumericUpDown()
        Me._TriggerDelayNumericLabel = New System.Windows.Forms.Label()
        Me._SenseRangeNumericLabel = New System.Windows.Forms.Label()
        Me._IntegrationPeriodNumericLabel = New System.Windows.Forms.Label()
        Me._SenseFunctionComboBox = New System.Windows.Forms.ComboBox()
        Me._SenseFunctionComboBoxLabel = New System.Windows.Forms.Label()
        Me._SenseAutoRangeToggle = New System.Windows.Forms.CheckBox()
        Me._ApplySenseSettingsButton = New System.Windows.Forms.Button()
        Me._ChannelTabPage = New System.Windows.Forms.TabPage()
        Me._ClosedChannelsTextBoxLabel = New System.Windows.Forms.Label()
        Me._ClosedChannelsTextBox = New System.Windows.Forms.TextBox()
        Me._OpenAllButton = New System.Windows.Forms.Button()
        Me._OpenChannelsButton = New System.Windows.Forms.Button()
        Me._CloseOnlyButton = New System.Windows.Forms.Button()
        Me._CloseChannelsButton = New System.Windows.Forms.Button()
        Me._ChannelListComboBox = New System.Windows.Forms.ComboBox()
        Me._ChannelListComboBoxLabel = New System.Windows.Forms.Label()
        Me._ReadWriteTabPage = New System.Windows.Forms.TabPage()
        Me._SimpleReadWriteControl = New isr.VI.Instrument.SimpleReadWriteControl()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._LastErrorTextBox = New System.Windows.Forms.TextBox()
        Me._ReadingStatusStrip = New System.Windows.Forms.StatusStrip()
        Me._FailureCodeToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._ReadingToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._TbdToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._LastReadingTextBox = New System.Windows.Forms.TextBox()
        Me._Panel = New System.Windows.Forms.Panel()
        Me._Layout = New System.Windows.Forms.TableLayoutPanel()
        Me._TitleLabel = New System.Windows.Forms.Label()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        CType(Me._ReadingsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ReadingToolStrip.SuspendLayout()
        Me._SystemToolStrip.SuspendLayout()
        Me._SenseTabPage.SuspendLayout()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._PowerLineCyclesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ChannelTabPage.SuspendLayout()
        Me._ReadWriteTabPage.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
        Me._ReadingStatusStrip.SuspendLayout()
        Me._Panel.SuspendLayout()
        Me._Layout.SuspendLayout()
        Me.SuspendLayout()
        '
        'Connector
        '
        Me.Connector.Location = New System.Drawing.Point(0, 388)
        Me.Connector.Searchable = True
        Me.Connector.TabIndex = 4
        '
        'TraceMessagesBox
        '
        Me.TraceMessagesBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TraceMessagesBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TraceMessagesBox.PresetCount = 50
        Me.TraceMessagesBox.ResetCount = 100
        Me.TraceMessagesBox.Size = New System.Drawing.Size(356, 270)
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ReadingTabPage)
        Me._Tabs.Controls.Add(Me._SenseTabPage)
        Me._Tabs.Controls.Add(Me._ChannelTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(52, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 71)
        Me._Tabs.Multiline = True
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 300)
        Me._Tabs.TabIndex = 3
        '
        '_ReadingTabPage
        '
        Me._ReadingTabPage.Controls.Add(Me._ReadingsDataGridView)
        Me._ReadingTabPage.Controls.Add(Me._ReadingToolStrip)
        Me._ReadingTabPage.Controls.Add(Me._SystemToolStrip)
        Me._ReadingTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadingTabPage.Name = "_ReadingTabPage"
        Me._ReadingTabPage.Size = New System.Drawing.Size(356, 270)
        Me._ReadingTabPage.TabIndex = 0
        Me._ReadingTabPage.Text = "Reading"
        Me._ReadingTabPage.UseVisualStyleBackColor = True
        '
        '_ReadingsDataGridView
        '
        Me._ReadingsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me._ReadingsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ReadingsDataGridView.Location = New System.Drawing.Point(0, 25)
        Me._ReadingsDataGridView.Name = "_ReadingsDataGridView"
        Me._ReadingsDataGridView.Size = New System.Drawing.Size(356, 217)
        Me._ReadingsDataGridView.TabIndex = 23
        Me.TipsTooltip.SetToolTip(Me._ReadingsDataGridView, "Buffer data")
        '
        '_ReadingToolStrip
        '
        Me._ReadingToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ReadButton, Me._InitiateButton, Me._TraceButton, Me._ReadingsCountLabel, Me._ReadingComboBox, Me._AbortButton, Me._ClearBufferDisplayButton})
        Me._ReadingToolStrip.Location = New System.Drawing.Point(0, 0)
        Me._ReadingToolStrip.Name = "_ReadingToolStrip"
        Me._ReadingToolStrip.Size = New System.Drawing.Size(356, 25)
        Me._ReadingToolStrip.TabIndex = 22
        Me._ReadingToolStrip.Text = "ToolStrip1"
        '
        '_ReadButton
        '
        Me._ReadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ReadButton.Image = CType(resources.GetObject("_ReadButton.Image"), System.Drawing.Image)
        Me._ReadButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ReadButton.Name = "_ReadButton"
        Me._ReadButton.Size = New System.Drawing.Size(37, 22)
        Me._ReadButton.Text = "Read"
        Me._ReadButton.ToolTipText = "Read single reading"
        '
        '_InitiateButton
        '
        Me._InitiateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._InitiateButton.Image = CType(resources.GetObject("_InitiateButton.Image"), System.Drawing.Image)
        Me._InitiateButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._InitiateButton.Name = "_InitiateButton"
        Me._InitiateButton.Size = New System.Drawing.Size(47, 22)
        Me._InitiateButton.Text = "Initiate"
        '
        '_TraceButton
        '
        Me._TraceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TraceButton.Image = CType(resources.GetObject("_TraceButton.Image"), System.Drawing.Image)
        Me._TraceButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._TraceButton.Name = "_TraceButton"
        Me._TraceButton.Size = New System.Drawing.Size(39, 22)
        Me._TraceButton.Text = "Trace"
        Me._TraceButton.ToolTipText = "Reads the buffer"
        '
        '_ReadingsCountLabel
        '
        Me._ReadingsCountLabel.Name = "_ReadingsCountLabel"
        Me._ReadingsCountLabel.Size = New System.Drawing.Size(13, 22)
        Me._ReadingsCountLabel.Text = "0"
        Me._ReadingsCountLabel.ToolTipText = "Buffer count"
        '
        '_ReadingComboBox
        '
        Me._ReadingComboBox.Name = "_ReadingComboBox"
        Me._ReadingComboBox.Size = New System.Drawing.Size(121, 25)
        Me._ReadingComboBox.ToolTipText = "Select reading type"
        '
        '_AbortButton
        '
        Me._AbortButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._AbortButton.Image = CType(resources.GetObject("_AbortButton.Image"), System.Drawing.Image)
        Me._AbortButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._AbortButton.Name = "_AbortButton"
        Me._AbortButton.Size = New System.Drawing.Size(41, 22)
        Me._AbortButton.Text = "Abort"
        Me._AbortButton.ToolTipText = "Aborts active trigger"
        '
        '_ClearBufferDisplayButton
        '
        Me._ClearBufferDisplayButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ClearBufferDisplayButton.Font = New System.Drawing.Font("Wingdings", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me._ClearBufferDisplayButton.Image = CType(resources.GetObject("_ClearBufferDisplayButton.Image"), System.Drawing.Image)
        Me._ClearBufferDisplayButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ClearBufferDisplayButton.Name = "_ClearBufferDisplayButton"
        Me._ClearBufferDisplayButton.Size = New System.Drawing.Size(25, 22)
        Me._ClearBufferDisplayButton.Text = """"
        Me._ClearBufferDisplayButton.ToolTipText = "CLear the buffer display"
        '
        '_SystemToolStrip
        '
        Me._SystemToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._SystemToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetSplitButton, Me._ReadTerminalStateButton, Me._ServiceRequestEnableBitmaskNumericLabel, Me._ServiceRequestEnableBitmaskNumeric})
        Me._SystemToolStrip.Location = New System.Drawing.Point(0, 242)
        Me._SystemToolStrip.Name = "_SystemToolStrip"
        Me._SystemToolStrip.Size = New System.Drawing.Size(356, 28)
        Me._SystemToolStrip.TabIndex = 21
        Me._SystemToolStrip.Text = "System Tools"
        Me.TipsTooltip.SetToolTip(Me._SystemToolStrip, "System operations")
        '
        '_ResetSplitButton
        '
        Me._ResetSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ResetSplitButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ClearInterfaceMenuItem, Me._ClearDeviceMenuItem, Me._ResetKnownStateMenuItem, Me._InitKnownStateMenuItem, Me._ClearExecutionStateMenuItem, Me._SessionTraceEnabledMenuItem, Me._SessionServiceRequestHandlerEnabledMenuItem, Me._DeviceServiceRequestHandlerEnabledMenuItem, Me._LogTraceLevelComboBox, Me._DisplayTraceLevelComboBox})
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
        '
        '_ClearExecutionStateMenuItem
        '
        Me._ClearExecutionStateMenuItem.Name = "_ClearExecutionStateMenuItem"
        Me._ClearExecutionStateMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._ClearExecutionStateMenuItem.Text = "Clear Execution State (CLS)"
        Me._ClearExecutionStateMenuItem.ToolTipText = "Clears the execution state"
        '
        '_SessionTraceEnabledMenuItem
        '
        Me._SessionTraceEnabledMenuItem.CheckOnClick = True
        Me._SessionTraceEnabledMenuItem.Name = "_SessionTraceEnabledMenuItem"
        Me._SessionTraceEnabledMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._SessionTraceEnabledMenuItem.Text = "Trace Instrument Messages"
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
        '_ServiceRequestEnableBitmaskNumericLabel
        '
        Me._ServiceRequestEnableBitmaskNumericLabel.Name = "_ServiceRequestEnableBitmaskNumericLabel"
        Me._ServiceRequestEnableBitmaskNumericLabel.Size = New System.Drawing.Size(29, 25)
        Me._ServiceRequestEnableBitmaskNumericLabel.Text = "SRE:"
        '
        '_ServiceRequestEnableBitmaskNumeric
        '
        Me._ServiceRequestEnableBitmaskNumeric.Name = "_ServiceRequestEnableBitmaskNumeric"
        Me._ServiceRequestEnableBitmaskNumeric.Size = New System.Drawing.Size(41, 25)
        Me._ServiceRequestEnableBitmaskNumeric.Text = "0"
        Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = "Service request bitmask"
        Me._ServiceRequestEnableBitmaskNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        '
        '_SenseTabPage
        '
        Me._SenseTabPage.Controls.Add(Me._ApplyFunctionModeButton)
        Me._SenseTabPage.Controls.Add(Me._TriggerDelayNumeric)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeNumeric)
        Me._SenseTabPage.Controls.Add(Me._PowerLineCyclesNumeric)
        Me._SenseTabPage.Controls.Add(Me._TriggerDelayNumericLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeNumericLabel)
        Me._SenseTabPage.Controls.Add(Me._IntegrationPeriodNumericLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseFunctionComboBox)
        Me._SenseTabPage.Controls.Add(Me._SenseFunctionComboBoxLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseAutoRangeToggle)
        Me._SenseTabPage.Controls.Add(Me._ApplySenseSettingsButton)
        Me._SenseTabPage.Location = New System.Drawing.Point(4, 26)
        Me._SenseTabPage.Name = "_SenseTabPage"
        Me._SenseTabPage.Size = New System.Drawing.Size(356, 270)
        Me._SenseTabPage.TabIndex = 4
        Me._SenseTabPage.Text = "Sense"
        Me._SenseTabPage.UseVisualStyleBackColor = True
        '
        '_ApplyFunctionModeButton
        '
        Me._ApplyFunctionModeButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplyFunctionModeButton.Location = New System.Drawing.Point(309, 13)
        Me._ApplyFunctionModeButton.Name = "_ApplyFunctionModeButton"
        Me._ApplyFunctionModeButton.Size = New System.Drawing.Size(38, 25)
        Me._ApplyFunctionModeButton.TabIndex = 10
        Me._ApplyFunctionModeButton.Text = "SET"
        Me._ApplyFunctionModeButton.UseVisualStyleBackColor = True
        '
        '_TriggerDelayNumeric
        '
        Me._TriggerDelayNumeric.DecimalPlaces = 3
        Me._TriggerDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerDelayNumeric.Location = New System.Drawing.Point(116, 114)
        Me._TriggerDelayNumeric.Name = "_TriggerDelayNumeric"
        Me._TriggerDelayNumeric.Size = New System.Drawing.Size(76, 25)
        Me._TriggerDelayNumeric.TabIndex = 8
        '
        '_SenseRangeNumeric
        '
        Me._SenseRangeNumeric.DecimalPlaces = 3
        Me._SenseRangeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SenseRangeNumeric.Location = New System.Drawing.Point(116, 80)
        Me._SenseRangeNumeric.Maximum = New Decimal(New Integer() {1010, 0, 0, 0})
        Me._SenseRangeNumeric.Name = "_SenseRangeNumeric"
        Me._SenseRangeNumeric.Size = New System.Drawing.Size(76, 25)
        Me._SenseRangeNumeric.TabIndex = 5
        Me.TipsTooltip.SetToolTip(Me._SenseRangeNumeric, "Range")
        Me._SenseRangeNumeric.Value = New Decimal(New Integer() {105, 0, 0, 196608})
        '
        '_PowerLineCyclesNumeric
        '
        Me._PowerLineCyclesNumeric.DecimalPlaces = 3
        Me._PowerLineCyclesNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PowerLineCyclesNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._PowerLineCyclesNumeric.Location = New System.Drawing.Point(116, 46)
        Me._PowerLineCyclesNumeric.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me._PowerLineCyclesNumeric.Minimum = New Decimal(New Integer() {2, 0, 0, 131072})
        Me._PowerLineCyclesNumeric.Name = "_PowerLineCyclesNumeric"
        Me._PowerLineCyclesNumeric.Size = New System.Drawing.Size(76, 25)
        Me._PowerLineCyclesNumeric.TabIndex = 3
        Me.TipsTooltip.SetToolTip(Me._PowerLineCyclesNumeric, "Power line cycles")
        Me._PowerLineCyclesNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        '_TriggerDelayNumericLabel
        '
        Me._TriggerDelayNumericLabel.AutoSize = True
        Me._TriggerDelayNumericLabel.Location = New System.Drawing.Point(6, 118)
        Me._TriggerDelayNumericLabel.Name = "_TriggerDelayNumericLabel"
        Me._TriggerDelayNumericLabel.Size = New System.Drawing.Size(107, 17)
        Me._TriggerDelayNumericLabel.TabIndex = 7
        Me._TriggerDelayNumericLabel.Text = "Trigger Delay [s]:"
        Me._TriggerDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseRangeNumericLabel
        '
        Me._SenseRangeNumericLabel.AutoSize = True
        Me._SenseRangeNumericLabel.Location = New System.Drawing.Point(46, 84)
        Me._SenseRangeNumericLabel.Name = "_SenseRangeNumericLabel"
        Me._SenseRangeNumericLabel.Size = New System.Drawing.Size(68, 17)
        Me._SenseRangeNumericLabel.TabIndex = 4
        Me._SenseRangeNumericLabel.Text = "Range [V]:"
        Me._SenseRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_IntegrationPeriodNumericLabel
        '
        Me._IntegrationPeriodNumericLabel.AutoSize = True
        Me._IntegrationPeriodNumericLabel.Location = New System.Drawing.Point(16, 50)
        Me._IntegrationPeriodNumericLabel.Name = "_IntegrationPeriodNumericLabel"
        Me._IntegrationPeriodNumericLabel.Size = New System.Drawing.Size(98, 17)
        Me._IntegrationPeriodNumericLabel.TabIndex = 2
        Me._IntegrationPeriodNumericLabel.Text = "Aperture [nplc]:"
        Me._IntegrationPeriodNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseFunctionComboBox
        '
        Me._SenseFunctionComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SenseFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._SenseFunctionComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseFunctionComboBox.Items.AddRange(New Object() {"I", "V"})
        Me._SenseFunctionComboBox.Location = New System.Drawing.Point(116, 12)
        Me._SenseFunctionComboBox.Name = "_SenseFunctionComboBox"
        Me._SenseFunctionComboBox.Size = New System.Drawing.Size(189, 25)
        Me._SenseFunctionComboBox.TabIndex = 1
        '
        '_SenseFunctionComboBoxLabel
        '
        Me._SenseFunctionComboBoxLabel.AutoSize = True
        Me._SenseFunctionComboBoxLabel.Location = New System.Drawing.Point(55, 16)
        Me._SenseFunctionComboBoxLabel.Name = "_SenseFunctionComboBoxLabel"
        Me._SenseFunctionComboBoxLabel.Size = New System.Drawing.Size(59, 17)
        Me._SenseFunctionComboBoxLabel.TabIndex = 0
        Me._SenseFunctionComboBoxLabel.Text = "Function:"
        Me._SenseFunctionComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseAutoRangeToggle
        '
        Me._SenseAutoRangeToggle.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseAutoRangeToggle.Location = New System.Drawing.Point(198, 82)
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
        Me._ApplySenseSettingsButton.Location = New System.Drawing.Point(283, 225)
        Me._ApplySenseSettingsButton.Name = "_ApplySenseSettingsButton"
        Me._ApplySenseSettingsButton.Size = New System.Drawing.Size(58, 30)
        Me._ApplySenseSettingsButton.TabIndex = 9
        Me._ApplySenseSettingsButton.Text = "&Apply"
        Me._ApplySenseSettingsButton.UseVisualStyleBackColor = True
        '
        '_ChannelTabPage
        '
        Me._ChannelTabPage.Controls.Add(Me._ClosedChannelsTextBoxLabel)
        Me._ChannelTabPage.Controls.Add(Me._ClosedChannelsTextBox)
        Me._ChannelTabPage.Controls.Add(Me._OpenAllButton)
        Me._ChannelTabPage.Controls.Add(Me._OpenChannelsButton)
        Me._ChannelTabPage.Controls.Add(Me._CloseOnlyButton)
        Me._ChannelTabPage.Controls.Add(Me._CloseChannelsButton)
        Me._ChannelTabPage.Controls.Add(Me._ChannelListComboBox)
        Me._ChannelTabPage.Controls.Add(Me._ChannelListComboBoxLabel)
        Me._ChannelTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ChannelTabPage.Name = "_ChannelTabPage"
        Me._ChannelTabPage.Size = New System.Drawing.Size(356, 270)
        Me._ChannelTabPage.TabIndex = 1
        Me._ChannelTabPage.Text = "Channel"
        Me._ChannelTabPage.UseVisualStyleBackColor = True
        '
        '_ClosedChannelsTextBoxLabel
        '
        Me._ClosedChannelsTextBoxLabel.Location = New System.Drawing.Point(7, 110)
        Me._ClosedChannelsTextBoxLabel.Name = "_ClosedChannelsTextBoxLabel"
        Me._ClosedChannelsTextBoxLabel.Size = New System.Drawing.Size(113, 21)
        Me._ClosedChannelsTextBoxLabel.TabIndex = 6
        Me._ClosedChannelsTextBoxLabel.Text = "Closed Channels:"
        '
        '_ClosedChannelsTextBox
        '
        Me._ClosedChannelsTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ClosedChannelsTextBox.Location = New System.Drawing.Point(7, 132)
        Me._ClosedChannelsTextBox.Multiline = True
        Me._ClosedChannelsTextBox.Name = "_ClosedChannelsTextBox"
        Me._ClosedChannelsTextBox.ReadOnly = True
        Me._ClosedChannelsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._ClosedChannelsTextBox.Size = New System.Drawing.Size(340, 126)
        Me._ClosedChannelsTextBox.TabIndex = 7
        '
        '_OpenAllButton
        '
        Me._OpenAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._OpenAllButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._OpenAllButton.Location = New System.Drawing.Point(259, 77)
        Me._OpenAllButton.Name = "_OpenAllButton"
        Me._OpenAllButton.Size = New System.Drawing.Size(90, 30)
        Me._OpenAllButton.TabIndex = 5
        Me._OpenAllButton.Text = "Open &All"
        Me._OpenAllButton.UseVisualStyleBackColor = True
        '
        '_OpenChannelsButton
        '
        Me._OpenChannelsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._OpenChannelsButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._OpenChannelsButton.Location = New System.Drawing.Point(293, 10)
        Me._OpenChannelsButton.Name = "_OpenChannelsButton"
        Me._OpenChannelsButton.Size = New System.Drawing.Size(56, 30)
        Me._OpenChannelsButton.TabIndex = 2
        Me._OpenChannelsButton.Text = "&Open"
        Me._OpenChannelsButton.UseVisualStyleBackColor = True
        '
        '_CloseOnlyButton
        '
        Me._CloseOnlyButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._CloseOnlyButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._CloseOnlyButton.Location = New System.Drawing.Point(110, 77)
        Me._CloseOnlyButton.Name = "_CloseOnlyButton"
        Me._CloseOnlyButton.Size = New System.Drawing.Size(142, 30)
        Me._CloseOnlyButton.TabIndex = 4
        Me._CloseOnlyButton.Text = "Open All and &Close"
        Me.TipsTooltip.SetToolTip(Me._CloseOnlyButton, "Closes the specified channels opening all other channels")
        Me._CloseOnlyButton.UseVisualStyleBackColor = True
        '
        '_CloseChannelsButton
        '
        Me._CloseChannelsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._CloseChannelsButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._CloseChannelsButton.Location = New System.Drawing.Point(231, 10)
        Me._CloseChannelsButton.Name = "_CloseChannelsButton"
        Me._CloseChannelsButton.Size = New System.Drawing.Size(57, 30)
        Me._CloseChannelsButton.TabIndex = 1
        Me._CloseChannelsButton.Text = "&Close"
        Me._CloseChannelsButton.UseVisualStyleBackColor = True
        '
        '_ChannelListComboBox
        '
        Me._ChannelListComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ChannelListComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ChannelListComboBox.Location = New System.Drawing.Point(7, 42)
        Me._ChannelListComboBox.Name = "_ChannelListComboBox"
        Me._ChannelListComboBox.Size = New System.Drawing.Size(341, 25)
        Me._ChannelListComboBox.TabIndex = 3
        Me._ChannelListComboBox.Text = "(@ 201,203:205)"
        '
        '_ChannelListComboBoxLabel
        '
        Me._ChannelListComboBoxLabel.Location = New System.Drawing.Point(7, 20)
        Me._ChannelListComboBoxLabel.Name = "_ChannelListComboBoxLabel"
        Me._ChannelListComboBoxLabel.Size = New System.Drawing.Size(84, 21)
        Me._ChannelListComboBoxLabel.TabIndex = 0
        Me._ChannelListComboBoxLabel.Text = "Channel List:"
        '
        '_ReadWriteTabPage
        '
        Me._ReadWriteTabPage.Controls.Add(Me._SimpleReadWriteControl)
        Me._ReadWriteTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadWriteTabPage.Name = "_ReadWriteTabPage"
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(356, 270)
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
        Me._SimpleReadWriteControl.Size = New System.Drawing.Size(356, 270)
        Me._SimpleReadWriteControl.TabIndex = 0
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me.TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(356, 270)
        Me._MessagesTabPage.TabIndex = 3
        Me._MessagesTabPage.Text = "Log"
        Me._MessagesTabPage.UseVisualStyleBackColor = True
        '
        '_LastErrorTextBox
        '
        Me._LastErrorTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._LastErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._LastErrorTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._LastErrorTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._LastErrorTextBox.ForeColor = System.Drawing.Color.OrangeRed
        Me._LastErrorTextBox.Location = New System.Drawing.Point(0, 53)
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
        Me._ReadingStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._FailureCodeToolStripStatusLabel, Me._ReadingToolStripStatusLabel, Me._TbdToolStripStatusLabel})
        Me._ReadingStatusStrip.Location = New System.Drawing.Point(0, 0)
        Me._ReadingStatusStrip.Name = "_ReadingStatusStrip"
        Me._ReadingStatusStrip.Size = New System.Drawing.Size(364, 37)
        Me._ReadingStatusStrip.SizingGrip = False
        Me._ReadingStatusStrip.TabIndex = 1
        '
        '_FailureCodeToolStripStatusLabel
        '
        Me._FailureCodeToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._FailureCodeToolStripStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._FailureCodeToolStripStatusLabel.ForeColor = System.Drawing.Color.Red
        Me._FailureCodeToolStripStatusLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._FailureCodeToolStripStatusLabel.Name = "_FailureCodeToolStripStatusLabel"
        Me._FailureCodeToolStripStatusLabel.Size = New System.Drawing.Size(16, 37)
        Me._FailureCodeToolStripStatusLabel.Text = "C"
        Me._FailureCodeToolStripStatusLabel.ToolTipText = "Compliance"
        '
        '_ReadingToolStripStatusLabel
        '
        Me._ReadingToolStripStatusLabel.AutoSize = False
        Me._ReadingToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ReadingToolStripStatusLabel.ForeColor = System.Drawing.Color.Aquamarine
        Me._ReadingToolStripStatusLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._ReadingToolStripStatusLabel.Name = "_ReadingToolStripStatusLabel"
        Me._ReadingToolStripStatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._ReadingToolStripStatusLabel.Size = New System.Drawing.Size(313, 37)
        Me._ReadingToolStripStatusLabel.Spring = True
        Me._ReadingToolStripStatusLabel.Text = "0.0000000 mV"
        Me._ReadingToolStripStatusLabel.ToolTipText = "Reading"
        '
        '_TbdToolStripStatusLabel
        '
        Me._TbdToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TbdToolStripStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TbdToolStripStatusLabel.ForeColor = System.Drawing.Color.Aquamarine
        Me._TbdToolStripStatusLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._TbdToolStripStatusLabel.Name = "_TbdToolStripStatusLabel"
        Me._TbdToolStripStatusLabel.Size = New System.Drawing.Size(20, 37)
        Me._TbdToolStripStatusLabel.Text = " T"
        Me._TbdToolStripStatusLabel.ToolTipText = "To be defined"
        '
        '_LastReadingTextBox
        '
        Me._LastReadingTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._LastReadingTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._LastReadingTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._LastReadingTextBox.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._LastReadingTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._LastReadingTextBox.Location = New System.Drawing.Point(0, 37)
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
        Me._Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Panel.Location = New System.Drawing.Point(0, 17)
        Me._Panel.Margin = New System.Windows.Forms.Padding(0)
        Me._Panel.Name = "_Panel"
        Me._Panel.Size = New System.Drawing.Size(364, 371)
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
        Me._Layout.Margin = New System.Windows.Forms.Padding(0)
        Me._Layout.Name = "_Layout"
        Me._Layout.RowCount = 2
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._Layout.Size = New System.Drawing.Size(364, 388)
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
        Me._TitleLabel.Text = "K34980"
        Me._TitleLabel.UseMnemonic = False
        '
        'K34980Panel
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "K34980Panel"
        Me.Size = New System.Drawing.Size(364, 450)
        Me.Controls.SetChildIndex(Me.Connector, 0)
        Me.Controls.SetChildIndex(Me._Layout, 0)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Tabs.ResumeLayout(False)
        Me._ReadingTabPage.ResumeLayout(False)
        Me._ReadingTabPage.PerformLayout()
        CType(Me._ReadingsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ReadingToolStrip.ResumeLayout(False)
        Me._ReadingToolStrip.PerformLayout()
        Me._SystemToolStrip.ResumeLayout(False)
        Me._SystemToolStrip.PerformLayout()
        Me._SenseTabPage.ResumeLayout(False)
        Me._SenseTabPage.PerformLayout()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._PowerLineCyclesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ChannelTabPage.ResumeLayout(False)
        Me._ChannelTabPage.PerformLayout()
        Me._ReadWriteTabPage.ResumeLayout(False)
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._ReadingStatusStrip.ResumeLayout(False)
        Me._ReadingStatusStrip.PerformLayout()
        Me._Panel.ResumeLayout(False)
        Me._Panel.PerformLayout()
        Me._Layout.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _ReadingTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ChannelTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ApplySenseSettingsButton As System.Windows.Forms.Button
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseAutoRangeToggle As System.Windows.Forms.CheckBox
    Private WithEvents _SenseFunctionComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _SenseFunctionComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _IntegrationPeriodNumericLabel As System.Windows.Forms.Label
    Private WithEvents _SenseRangeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _TriggerDelayNumericLabel As System.Windows.Forms.Label
    Private WithEvents _OpenChannelsButton As System.Windows.Forms.Button
    Private WithEvents _CloseChannelsButton As System.Windows.Forms.Button
    Private WithEvents _ChannelListComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _ChannelListComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _OpenAllButton As System.Windows.Forms.Button
    Private WithEvents _CloseOnlyButton As System.Windows.Forms.Button
    Private WithEvents _ClosedChannelsTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _ClosedChannelsTextBox As System.Windows.Forms.TextBox
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _LastErrorTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ReadingStatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _FailureCodeToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ReadingToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _TbdToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _LastReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _TriggerDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _SenseRangeNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _PowerLineCyclesNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TitleLabel As System.Windows.Forms.Label
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
    Private WithEvents _ApplyFunctionModeButton As Windows.Forms.Button
    Private WithEvents _SystemToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ResetSplitButton As Windows.Forms.ToolStripSplitButton
    Private WithEvents _ClearInterfaceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearDeviceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ResetKnownStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _InitKnownStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearExecutionStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _SessionTraceEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _SessionServiceRequestHandlerEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _DeviceServiceRequestHandlerEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ReadTerminalStateButton As Windows.Forms.ToolStripButton
    Private WithEvents _ServiceRequestEnableBitmaskNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ServiceRequestEnableBitmaskNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _ReadingsDataGridView As Windows.Forms.DataGridView
    Private WithEvents _ReadingToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ReadButton As Windows.Forms.ToolStripButton
    Private WithEvents _InitiateButton As Windows.Forms.ToolStripButton
    Private WithEvents _TraceButton As Windows.Forms.ToolStripButton
    Private WithEvents _ReadingsCountLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ReadingComboBox As Windows.Forms.ToolStripComboBox
    Private WithEvents _AbortButton As Windows.Forms.ToolStripButton
    Private WithEvents _ClearBufferDisplayButton As Windows.Forms.ToolStripButton
    Private WithEvents _LogTraceLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _DisplayTraceLevelComboBox As Core.Controls.ToolStripComboBox
End Class
