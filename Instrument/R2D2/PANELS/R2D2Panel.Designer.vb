Imports isr.VI.Instrument
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class R2D2Panel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(R2D2Panel))
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
        Me._SystemToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ResetSplitButton = New System.Windows.Forms.ToolStripSplitButton()
        Me._ClearInterfaceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearDeviceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ResetKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._LogTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._DisplayTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._InitKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearExecutionStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SessionTraceEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SessionServiceRequestHandlerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._DeviceServiceRequestHandlerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ReadTerminalStateButton = New System.Windows.Forms.ToolStripButton()
        Me._ServiceRequestEnableNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ServiceRequestEnableNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._InterfaceTabPage = New System.Windows.Forms.TabPage()
        Me._ResetButton = New System.Windows.Forms.Button()
        Me._InterfaceClearButton = New System.Windows.Forms.Button()
        Me._ChannelTabPage = New System.Windows.Forms.TabPage()
        Me._ClosedChannelsTextBoxLabel = New System.Windows.Forms.Label()
        Me._ClosedChannelsTextBox = New System.Windows.Forms.TextBox()
        Me._OpenAllButton = New System.Windows.Forms.Button()
        Me._OpenChannelsButton = New System.Windows.Forms.Button()
        Me._CloseOnlyButton = New System.Windows.Forms.Button()
        Me._CloseChannelsButton = New System.Windows.Forms.Button()
        Me._ChannelListComboBox = New System.Windows.Forms.ComboBox()
        Me._ChannelListComboBoxLabel = New System.Windows.Forms.Label()
        Me._SenseTabPage = New System.Windows.Forms.TabPage()
        Me._TriggerDelayTextBox = New System.Windows.Forms.TextBox()
        Me._TriggerDelayTextBoxLabel = New System.Windows.Forms.Label()
        Me._SenseRangeTextBox = New System.Windows.Forms.TextBox()
        Me._SenseRangeTextBoxLabel = New System.Windows.Forms.Label()
        Me._IntegrationPeriodTextBox = New System.Windows.Forms.TextBox()
        Me._IntegrationPeriodTextBoxLabel = New System.Windows.Forms.Label()
        Me._SenseFunctionComboBox = New System.Windows.Forms.ComboBox()
        Me._SenseFunctionComboBoxLabel = New System.Windows.Forms.Label()
        Me._SenseAutoRangeToggle = New System.Windows.Forms.CheckBox()
        Me._ApplySenseSettingsButton = New System.Windows.Forms.Button()
        Me._ReadWriteTabPage = New System.Windows.Forms.TabPage()
        Me._SimpleReadWriteControl = New isr.VI.Instrument.SimpleReadWriteControl()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._ReadingTextBox = New System.Windows.Forms.TextBox()
        Me._TitleLabel = New System.Windows.Forms.Label()
        Me._ReadingsDataGridView = New System.Windows.Forms.DataGridView()
        Me._ReadingToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ReadButton = New System.Windows.Forms.ToolStripButton()
        Me._InitiateButton = New System.Windows.Forms.ToolStripButton()
        Me._TraceButton = New System.Windows.Forms.ToolStripButton()
        Me._ReadingsCountLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ReadingComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me._AbortButton = New System.Windows.Forms.ToolStripButton()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        Me._SystemToolStrip.SuspendLayout()
        Me._InterfaceTabPage.SuspendLayout()
        Me._ChannelTabPage.SuspendLayout()
        Me._SenseTabPage.SuspendLayout()
        Me._ReadWriteTabPage.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
        CType(Me._ReadingsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ReadingToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'Connector
        '
        Me.Connector.Location = New System.Drawing.Point(0, 368)
        Me.Connector.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.Connector.Searchable = True
        '
        'TraceMessagesBox
        '
        Me.TraceMessagesBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TraceMessagesBox.PresetCount = 50
        Me.TraceMessagesBox.ResetCount = 100
        Me.TraceMessagesBox.Size = New System.Drawing.Size(356, 281)
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ReadingTabPage)
        Me._Tabs.Controls.Add(Me._InterfaceTabPage)
        Me._Tabs.Controls.Add(Me._ChannelTabPage)
        Me._Tabs.Controls.Add(Me._SenseTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(52, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 57)
        Me._Tabs.Multiline = True
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 311)
        Me._Tabs.TabIndex = 15
        '
        '_ReadingTabPage
        '
        Me._ReadingTabPage.Controls.Add(Me._ReadingsDataGridView)
        Me._ReadingTabPage.Controls.Add(Me._ReadingToolStrip)
        Me._ReadingTabPage.Controls.Add(Me._SystemToolStrip)
        Me._ReadingTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadingTabPage.Name = "_ReadingTabPage"
        Me._ReadingTabPage.Size = New System.Drawing.Size(356, 281)
        Me._ReadingTabPage.TabIndex = 0
        Me._ReadingTabPage.Text = "Reading"
        Me._ReadingTabPage.UseVisualStyleBackColor = True
        '
        '_SystemToolStrip
        '
        Me._SystemToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._SystemToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetSplitButton, Me._ReadTerminalStateButton, Me._ServiceRequestEnableNumericLabel, Me._ServiceRequestEnableNumeric})
        Me._SystemToolStrip.Location = New System.Drawing.Point(0, 253)
        Me._SystemToolStrip.Name = "_SystemToolStrip"
        Me._SystemToolStrip.Size = New System.Drawing.Size(356, 28)
        Me._SystemToolStrip.TabIndex = 22
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
        '_ServiceRequestEnableNumeric
        '
        Me._ServiceRequestEnableNumeric.Name = "_ServiceRequestEnableNumeric"
        Me._ServiceRequestEnableNumeric.Size = New System.Drawing.Size(41, 25)
        Me._ServiceRequestEnableNumeric.Text = "99"
        Me._ServiceRequestEnableNumeric.ToolTipText = "Service request enabled value"
        Me._ServiceRequestEnableNumeric.Value = New Decimal(New Integer() {99, 0, 0, 0})
        '
        '_InterfaceTabPage
        '
        Me._InterfaceTabPage.Controls.Add(Me._ResetButton)
        Me._InterfaceTabPage.Controls.Add(Me._InterfaceClearButton)
        Me._InterfaceTabPage.Location = New System.Drawing.Point(4, 26)
        Me._InterfaceTabPage.Name = "_InterfaceTabPage"
        Me._InterfaceTabPage.Size = New System.Drawing.Size(356, 281)
        Me._InterfaceTabPage.TabIndex = 2
        Me._InterfaceTabPage.Text = "Interface"
        Me._InterfaceTabPage.UseVisualStyleBackColor = True
        '
        '_ResetButton
        '
        Me._ResetButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ResetButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ResetButton.Location = New System.Drawing.Point(78, 120)
        Me._ResetButton.Name = "_ResetButton"
        Me._ResetButton.Size = New System.Drawing.Size(182, 30)
        Me._ResetButton.TabIndex = 1
        Me._ResetButton.Text = "&Reset to Known State"
        Me._ResetButton.UseVisualStyleBackColor = True
        '
        '_InterfaceClearButton
        '
        Me._InterfaceClearButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._InterfaceClearButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._InterfaceClearButton.Location = New System.Drawing.Point(106, 65)
        Me._InterfaceClearButton.Name = "_InterfaceClearButton"
        Me._InterfaceClearButton.Size = New System.Drawing.Size(135, 30)
        Me._InterfaceClearButton.TabIndex = 0
        Me._InterfaceClearButton.Text = "&Clear Interface"
        Me._InterfaceClearButton.UseVisualStyleBackColor = True
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
        Me._ChannelTabPage.Size = New System.Drawing.Size(356, 281)
        Me._ChannelTabPage.TabIndex = 1
        Me._ChannelTabPage.Text = "Channel"
        Me._ChannelTabPage.UseVisualStyleBackColor = True
        '
        '_ClosedChannelsTextBoxLabel
        '
        Me._ClosedChannelsTextBoxLabel.Location = New System.Drawing.Point(9, 110)
        Me._ClosedChannelsTextBoxLabel.Name = "_ClosedChannelsTextBoxLabel"
        Me._ClosedChannelsTextBoxLabel.Size = New System.Drawing.Size(113, 21)
        Me._ClosedChannelsTextBoxLabel.TabIndex = 13
        Me._ClosedChannelsTextBoxLabel.Text = "Closed Channels:"
        '
        '_ClosedChannelsTextBox
        '
        Me._ClosedChannelsTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ClosedChannelsTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ClosedChannelsTextBox.Location = New System.Drawing.Point(7, 132)
        Me._ClosedChannelsTextBox.Multiline = True
        Me._ClosedChannelsTextBox.Name = "_ClosedChannelsTextBox"
        Me._ClosedChannelsTextBox.ReadOnly = True
        Me._ClosedChannelsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._ClosedChannelsTextBox.Size = New System.Drawing.Size(338, 67)
        Me._ClosedChannelsTextBox.TabIndex = 12
        '
        '_OpenAllButton
        '
        Me._OpenAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._OpenAllButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._OpenAllButton.Location = New System.Drawing.Point(259, 77)
        Me._OpenAllButton.Name = "_OpenAllButton"
        Me._OpenAllButton.Size = New System.Drawing.Size(90, 30)
        Me._OpenAllButton.TabIndex = 11
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
        Me._OpenChannelsButton.TabIndex = 10
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
        Me._CloseOnlyButton.TabIndex = 9
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
        Me._CloseChannelsButton.TabIndex = 9
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
        Me._ChannelListComboBox.TabIndex = 8
        Me._ChannelListComboBox.Text = "(@ 201,203:205)"
        '
        '_ChannelListComboBoxLabel
        '
        Me._ChannelListComboBoxLabel.Location = New System.Drawing.Point(7, 21)
        Me._ChannelListComboBoxLabel.Name = "_ChannelListComboBoxLabel"
        Me._ChannelListComboBoxLabel.Size = New System.Drawing.Size(84, 21)
        Me._ChannelListComboBoxLabel.TabIndex = 7
        Me._ChannelListComboBoxLabel.Text = "Channel List:"
        '
        '_SenseTabPage
        '
        Me._SenseTabPage.Controls.Add(Me._TriggerDelayTextBox)
        Me._SenseTabPage.Controls.Add(Me._TriggerDelayTextBoxLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeTextBox)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeTextBoxLabel)
        Me._SenseTabPage.Controls.Add(Me._IntegrationPeriodTextBox)
        Me._SenseTabPage.Controls.Add(Me._IntegrationPeriodTextBoxLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseFunctionComboBox)
        Me._SenseTabPage.Controls.Add(Me._SenseFunctionComboBoxLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseAutoRangeToggle)
        Me._SenseTabPage.Controls.Add(Me._ApplySenseSettingsButton)
        Me._SenseTabPage.Location = New System.Drawing.Point(4, 26)
        Me._SenseTabPage.Name = "_SenseTabPage"
        Me._SenseTabPage.Size = New System.Drawing.Size(356, 281)
        Me._SenseTabPage.TabIndex = 4
        Me._SenseTabPage.Text = "Sense"
        Me._SenseTabPage.UseVisualStyleBackColor = True
        '
        '_TriggerDelayTextBox
        '
        Me._TriggerDelayTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._TriggerDelayTextBox.Location = New System.Drawing.Point(157, 114)
        Me._TriggerDelayTextBox.Name = "_TriggerDelayTextBox"
        Me._TriggerDelayTextBox.Size = New System.Drawing.Size(75, 25)
        Me._TriggerDelayTextBox.TabIndex = 37
        Me._TriggerDelayTextBox.Text = "0.0"
        '
        '_TriggerDelayTextBoxLabel
        '
        Me._TriggerDelayTextBoxLabel.Location = New System.Drawing.Point(15, 114)
        Me._TriggerDelayTextBoxLabel.Name = "_TriggerDelayTextBoxLabel"
        Me._TriggerDelayTextBoxLabel.Size = New System.Drawing.Size(141, 24)
        Me._TriggerDelayTextBoxLabel.TabIndex = 36
        Me._TriggerDelayTextBoxLabel.Text = "Trigger Delay [s]:"
        Me._TriggerDelayTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseRangeTextBox
        '
        Me._SenseRangeTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseRangeTextBox.Location = New System.Drawing.Point(157, 81)
        Me._SenseRangeTextBox.Name = "_SenseRangeTextBox"
        Me._SenseRangeTextBox.Size = New System.Drawing.Size(75, 25)
        Me._SenseRangeTextBox.TabIndex = 33
        Me._SenseRangeTextBox.Text = "1.00"
        '
        '_SenseRangeTextBoxLabel
        '
        Me._SenseRangeTextBoxLabel.Location = New System.Drawing.Point(15, 81)
        Me._SenseRangeTextBoxLabel.Name = "_SenseRangeTextBoxLabel"
        Me._SenseRangeTextBoxLabel.Size = New System.Drawing.Size(141, 24)
        Me._SenseRangeTextBoxLabel.TabIndex = 32
        Me._SenseRangeTextBoxLabel.Text = "Range:"
        Me._SenseRangeTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_IntegrationPeriodTextBox
        '
        Me._IntegrationPeriodTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._IntegrationPeriodTextBox.Location = New System.Drawing.Point(157, 47)
        Me._IntegrationPeriodTextBox.Name = "_IntegrationPeriodTextBox"
        Me._IntegrationPeriodTextBox.Size = New System.Drawing.Size(75, 25)
        Me._IntegrationPeriodTextBox.TabIndex = 31
        Me._IntegrationPeriodTextBox.Text = "16.7"
        '
        '_IntegrationPeriodTextBoxLabel
        '
        Me._IntegrationPeriodTextBoxLabel.Location = New System.Drawing.Point(12, 47)
        Me._IntegrationPeriodTextBoxLabel.Name = "_IntegrationPeriodTextBoxLabel"
        Me._IntegrationPeriodTextBoxLabel.Size = New System.Drawing.Size(144, 24)
        Me._IntegrationPeriodTextBoxLabel.TabIndex = 30
        Me._IntegrationPeriodTextBoxLabel.Text = "Integration Period [ms]:"
        Me._IntegrationPeriodTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseFunctionComboBox
        '
        Me._SenseFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._SenseFunctionComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseFunctionComboBox.Items.AddRange(New Object() {"I", "V"})
        Me._SenseFunctionComboBox.Location = New System.Drawing.Point(77, 12)
        Me._SenseFunctionComboBox.Name = "_SenseFunctionComboBox"
        Me._SenseFunctionComboBox.Size = New System.Drawing.Size(175, 25)
        Me._SenseFunctionComboBox.TabIndex = 29
        '
        '_SenseFunctionComboBoxLabel
        '
        Me._SenseFunctionComboBoxLabel.Location = New System.Drawing.Point(14, 14)
        Me._SenseFunctionComboBoxLabel.Name = "_SenseFunctionComboBoxLabel"
        Me._SenseFunctionComboBoxLabel.Size = New System.Drawing.Size(62, 21)
        Me._SenseFunctionComboBoxLabel.TabIndex = 28
        Me._SenseFunctionComboBoxLabel.Text = "Function:"
        Me._SenseFunctionComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_SenseAutoRangeToggle
        '
        Me._SenseAutoRangeToggle.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseAutoRangeToggle.Location = New System.Drawing.Point(239, 84)
        Me._SenseAutoRangeToggle.Name = "_SenseAutoRangeToggle"
        Me._SenseAutoRangeToggle.Size = New System.Drawing.Size(103, 21)
        Me._SenseAutoRangeToggle.TabIndex = 22
        Me._SenseAutoRangeToggle.Text = "Auto Range"
        Me._SenseAutoRangeToggle.UseVisualStyleBackColor = True
        '
        '_ApplySenseSettingsButton
        '
        Me._ApplySenseSettingsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplySenseSettingsButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ApplySenseSettingsButton.Location = New System.Drawing.Point(279, 210)
        Me._ApplySenseSettingsButton.Name = "_ApplySenseSettingsButton"
        Me._ApplySenseSettingsButton.Size = New System.Drawing.Size(58, 30)
        Me._ApplySenseSettingsButton.TabIndex = 13
        Me._ApplySenseSettingsButton.Text = "&Apply"
        Me._ApplySenseSettingsButton.UseVisualStyleBackColor = True
        '
        '_ReadWriteTabPage
        '
        Me._ReadWriteTabPage.Controls.Add(Me._SimpleReadWriteControl)
        Me._ReadWriteTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadWriteTabPage.Name = "_ReadWriteTabPage"
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(356, 281)
        Me._ReadWriteTabPage.TabIndex = 5
        Me._ReadWriteTabPage.Text = "R/W"
        Me._ReadWriteTabPage.UseVisualStyleBackColor = True
        '
        '_SimpleReadWriteControl
        '
        Me._SimpleReadWriteControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me._SimpleReadWriteControl.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SimpleReadWriteControl.Location = New System.Drawing.Point(0, 0)
        Me._SimpleReadWriteControl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SimpleReadWriteControl.Name = "_SimpleReadWriteControl"
        Me._SimpleReadWriteControl.ReadEnabled = False
        Me._SimpleReadWriteControl.Size = New System.Drawing.Size(356, 281)
        Me._SimpleReadWriteControl.TabIndex = 0
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me.TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(356, 281)
        Me._MessagesTabPage.TabIndex = 3
        Me._MessagesTabPage.Text = "Log"
        Me._MessagesTabPage.UseVisualStyleBackColor = True
        '
        '_ReadingTextBox
        '
        Me._ReadingTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._ReadingTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._ReadingTextBox.Font = New System.Drawing.Font("Segoe UI", 18.25!, System.Drawing.FontStyle.Bold)
        Me._ReadingTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._ReadingTextBox.Location = New System.Drawing.Point(0, 17)
        Me._ReadingTextBox.Name = "_ReadingTextBox"
        Me._ReadingTextBox.Size = New System.Drawing.Size(364, 40)
        Me._ReadingTextBox.TabIndex = 16
        Me._ReadingTextBox.Text = "0.0000000 mV"
        Me._ReadingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
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
        Me._TitleLabel.Text = "R2D2"
        Me._TitleLabel.UseMnemonic = False
        '
        '_ReadingsDataGridView
        '
        Me._ReadingsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me._ReadingsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ReadingsDataGridView.Location = New System.Drawing.Point(0, 25)
        Me._ReadingsDataGridView.Name = "_ReadingsDataGridView"
        Me._ReadingsDataGridView.Size = New System.Drawing.Size(356, 228)
        Me._ReadingsDataGridView.TabIndex = 24
        Me.TipsTooltip.SetToolTip(Me._ReadingsDataGridView, "Buffer data")
        '
        '_ReadingToolStrip
        '
        Me._ReadingToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ReadButton, Me._InitiateButton, Me._TraceButton, Me._ReadingsCountLabel, Me._ReadingComboBox, Me._AbortButton})
        Me._ReadingToolStrip.Location = New System.Drawing.Point(0, 0)
        Me._ReadingToolStrip.Name = "_ReadingToolStrip"
        Me._ReadingToolStrip.Size = New System.Drawing.Size(356, 25)
        Me._ReadingToolStrip.TabIndex = 23
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
        'R2D2Panel
        '
        Me.Controls.Add(Me._Tabs)
        Me.Controls.Add(Me._ReadingTextBox)
        Me.Controls.Add(Me._TitleLabel)
        Me.Name = "R2D2Panel"
        Me.Size = New System.Drawing.Size(364, 430)
        Me.Controls.SetChildIndex(Me._TitleLabel, 0)
        Me.Controls.SetChildIndex(Me._ReadingTextBox, 0)
        Me.Controls.SetChildIndex(Me.Connector, 0)
        Me.Controls.SetChildIndex(Me._Tabs, 0)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Tabs.ResumeLayout(False)
        Me._ReadingTabPage.ResumeLayout(False)
        Me._ReadingTabPage.PerformLayout()
        Me._SystemToolStrip.ResumeLayout(False)
        Me._SystemToolStrip.PerformLayout()
        Me._InterfaceTabPage.ResumeLayout(False)
        Me._ChannelTabPage.ResumeLayout(False)
        Me._ChannelTabPage.PerformLayout()
        Me._SenseTabPage.ResumeLayout(False)
        Me._SenseTabPage.PerformLayout()
        Me._ReadWriteTabPage.ResumeLayout(False)
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        CType(Me._ReadingsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ReadingToolStrip.ResumeLayout(False)
        Me._ReadingToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _ReadingTabPage As System.Windows.Forms.TabPage
    Private WithEvents _InterfaceTabPage As System.Windows.Forms.TabPage
    Private WithEvents _InterfaceClearButton As System.Windows.Forms.Button
    Private WithEvents _ChannelTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ApplySenseSettingsButton As System.Windows.Forms.Button
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseAutoRangeToggle As System.Windows.Forms.CheckBox
    Private WithEvents _SenseFunctionComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _SenseFunctionComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _IntegrationPeriodTextBox As System.Windows.Forms.TextBox
    Private WithEvents _IntegrationPeriodTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _SenseRangeTextBox As System.Windows.Forms.TextBox
    Private WithEvents _SenseRangeTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _TriggerDelayTextBox As System.Windows.Forms.TextBox
    Private WithEvents _TriggerDelayTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _OpenChannelsButton As System.Windows.Forms.Button
    Private WithEvents _CloseChannelsButton As System.Windows.Forms.Button
    Private WithEvents _ChannelListComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _ChannelListComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _OpenAllButton As System.Windows.Forms.Button
    Private WithEvents _CloseOnlyButton As System.Windows.Forms.Button
    Private WithEvents _ClosedChannelsTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _ClosedChannelsTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _ResetButton As System.Windows.Forms.Button
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As SimpleReadWriteControl
    Private WithEvents _TitleLabel As Windows.Forms.Label
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
    Private WithEvents _ServiceRequestEnableNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ServiceRequestEnableNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _ReadingsDataGridView As Windows.Forms.DataGridView
    Private WithEvents _ReadingToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ReadButton As Windows.Forms.ToolStripButton
    Private WithEvents _InitiateButton As Windows.Forms.ToolStripButton
    Private WithEvents _TraceButton As Windows.Forms.ToolStripButton
    Private WithEvents _ReadingsCountLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ReadingComboBox As Windows.Forms.ToolStripComboBox
    Private WithEvents _AbortButton As Windows.Forms.ToolStripButton
    Private WithEvents _LogTraceLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _DisplayTraceLevelComboBox As Core.Controls.ToolStripComboBox
End Class
