<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class K2700Panel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
        Me._HandleServiceRequestsCheckBox = New System.Windows.Forms.CheckBox()
        Me._ReadingComboBoxLabel = New System.Windows.Forms.Label()
        Me._TerminalsToggle = New isr.Core.Controls.CheckBox()
        Me._ReadingComboBox = New System.Windows.Forms.ComboBox()
        Me._ReadButton = New System.Windows.Forms.Button()
        Me._InitiateButton = New System.Windows.Forms.Button()
        Me._SenseTabPage = New System.Windows.Forms.TabPage()
        Me._TriggerDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me._SenseRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me._IntegrationPeriodNumeric = New System.Windows.Forms.NumericUpDown()
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
        Me._ResetTabPage = New System.Windows.Forms.TabPage()
        Me._ResetLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._InitializeKnownStateButton = New System.Windows.Forms.Button()
        Me._InterfaceClearButton = New System.Windows.Forms.Button()
        Me._ResetButton = New System.Windows.Forms.Button()
        Me._SelectiveDeviceClearButton = New System.Windows.Forms.Button()
        Me._SessionTraceEnableCheckBox = New System.Windows.Forms.CheckBox()
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
        Me._SenseTabPage.SuspendLayout()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._IntegrationPeriodNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ChannelTabPage.SuspendLayout()
        Me._ResetTabPage.SuspendLayout()
        Me._ResetLayout.SuspendLayout()
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
        Me._Tabs.Controls.Add(Me._ResetTabPage)
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
        Me._ReadingTabPage.Controls.Add(Me._HandleServiceRequestsCheckBox)
        Me._ReadingTabPage.Controls.Add(Me._ReadingComboBoxLabel)
        Me._ReadingTabPage.Controls.Add(Me._TerminalsToggle)
        Me._ReadingTabPage.Controls.Add(Me._ReadingComboBox)
        Me._ReadingTabPage.Controls.Add(Me._ReadButton)
        Me._ReadingTabPage.Controls.Add(Me._InitiateButton)
        Me._ReadingTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadingTabPage.Name = "_ReadingTabPage"
        Me._ReadingTabPage.Size = New System.Drawing.Size(356, 270)
        Me._ReadingTabPage.TabIndex = 0
        Me._ReadingTabPage.Text = "Reading"
        Me._ReadingTabPage.UseVisualStyleBackColor = True
        '
        '_HandleServiceRequestsCheckBox
        '
        Me._HandleServiceRequestsCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._HandleServiceRequestsCheckBox.AutoSize = True
        Me._HandleServiceRequestsCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._HandleServiceRequestsCheckBox.Location = New System.Drawing.Point(12, 238)
        Me._HandleServiceRequestsCheckBox.Name = "_HandleServiceRequestsCheckBox"
        Me._HandleServiceRequestsCheckBox.Size = New System.Drawing.Size(178, 21)
        Me._HandleServiceRequestsCheckBox.TabIndex = 4
        Me._HandleServiceRequestsCheckBox.Text = "Handle Service Requests"
        Me._HandleServiceRequestsCheckBox.UseVisualStyleBackColor = True
        '
        '_ReadingComboBoxLabel
        '
        Me._ReadingComboBoxLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ReadingComboBoxLabel.AutoSize = True
        Me._ReadingComboBoxLabel.Location = New System.Drawing.Point(110, 13)
        Me._ReadingComboBoxLabel.Name = "_ReadingComboBoxLabel"
        Me._ReadingComboBoxLabel.Size = New System.Drawing.Size(59, 17)
        Me._ReadingComboBoxLabel.TabIndex = 2
        Me._ReadingComboBoxLabel.Text = "Reading:"
        Me._ReadingComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_TerminalsToggle
        '
        Me._TerminalsToggle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._TerminalsToggle.Appearance = System.Windows.Forms.Appearance.Button
        Me._TerminalsToggle.Enabled = False
        Me._TerminalsToggle.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._TerminalsToggle.Location = New System.Drawing.Point(290, 229)
        Me._TerminalsToggle.Name = "_TerminalsToggle"
        Me._TerminalsToggle.ReadOnly = True
        Me._TerminalsToggle.Size = New System.Drawing.Size(58, 30)
        Me._TerminalsToggle.TabIndex = 5
        Me._TerminalsToggle.Text = "Front"
        Me._TerminalsToggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me._TerminalsToggle.ThreeState = True
        Me._TerminalsToggle.UseVisualStyleBackColor = True
        '
        '_ReadingComboBox
        '
        Me._ReadingComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ReadingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._ReadingComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ReadingComboBox.Location = New System.Drawing.Point(171, 9)
        Me._ReadingComboBox.Name = "_ReadingComboBox"
        Me._ReadingComboBox.Size = New System.Drawing.Size(175, 25)
        Me._ReadingComboBox.TabIndex = 3
        Me.TipsTooltip.SetToolTip(Me._ReadingComboBox, "Select he reading to display")
        '
        '_ReadButton
        '
        Me._ReadButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ReadButton.Location = New System.Drawing.Point(12, 15)
        Me._ReadButton.Name = "_ReadButton"
        Me._ReadButton.Size = New System.Drawing.Size(72, 30)
        Me._ReadButton.TabIndex = 0
        Me._ReadButton.Text = "&Read"
        Me._ReadButton.UseVisualStyleBackColor = True
        '
        '_InitiateButton
        '
        Me._InitiateButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._InitiateButton.Location = New System.Drawing.Point(12, 53)
        Me._InitiateButton.Name = "_InitiateButton"
        Me._InitiateButton.Size = New System.Drawing.Size(72, 30)
        Me._InitiateButton.TabIndex = 1
        Me._InitiateButton.Text = "&Initiate"
        Me._InitiateButton.UseVisualStyleBackColor = True
        '
        '_SenseTabPage
        '
        Me._SenseTabPage.Controls.Add(Me._TriggerDelayNumeric)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeNumeric)
        Me._SenseTabPage.Controls.Add(Me._IntegrationPeriodNumeric)
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
        '_TriggerDelayNumeric
        '
        Me._TriggerDelayNumeric.DecimalPlaces = 3
        Me._TriggerDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerDelayNumeric.Location = New System.Drawing.Point(121, 114)
        Me._TriggerDelayNumeric.Name = "_TriggerDelayNumeric"
        Me._TriggerDelayNumeric.Size = New System.Drawing.Size(76, 25)
        Me._TriggerDelayNumeric.TabIndex = 8
        '
        '_SenseRangeNumeric
        '
        Me._SenseRangeNumeric.DecimalPlaces = 3
        Me._SenseRangeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SenseRangeNumeric.Location = New System.Drawing.Point(121, 80)
        Me._SenseRangeNumeric.Maximum = New Decimal(New Integer() {1010, 0, 0, 0})
        Me._SenseRangeNumeric.Name = "_SenseRangeNumeric"
        Me._SenseRangeNumeric.Size = New System.Drawing.Size(76, 25)
        Me._SenseRangeNumeric.TabIndex = 5
        Me.TipsTooltip.SetToolTip(Me._SenseRangeNumeric, "Range")
        Me._SenseRangeNumeric.Value = New Decimal(New Integer() {105, 0, 0, 196608})
        '
        '_IntegrationPeriodNumeric
        '
        Me._IntegrationPeriodNumeric.DecimalPlaces = 3
        Me._IntegrationPeriodNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._IntegrationPeriodNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._IntegrationPeriodNumeric.Location = New System.Drawing.Point(121, 46)
        Me._IntegrationPeriodNumeric.Maximum = New Decimal(New Integer() {16667, 0, 0, 131072})
        Me._IntegrationPeriodNumeric.Minimum = New Decimal(New Integer() {16, 0, 0, 196608})
        Me._IntegrationPeriodNumeric.Name = "_IntegrationPeriodNumeric"
        Me._IntegrationPeriodNumeric.Size = New System.Drawing.Size(76, 25)
        Me._IntegrationPeriodNumeric.TabIndex = 3
        Me.TipsTooltip.SetToolTip(Me._IntegrationPeriodNumeric, "Integration period")
        Me._IntegrationPeriodNumeric.Value = New Decimal(New Integer() {16667, 0, 0, 196608})
        '
        '_TriggerDelayNumericLabel
        '
        Me._TriggerDelayNumericLabel.AutoSize = True
        Me._TriggerDelayNumericLabel.Location = New System.Drawing.Point(11, 118)
        Me._TriggerDelayNumericLabel.Name = "_TriggerDelayNumericLabel"
        Me._TriggerDelayNumericLabel.Size = New System.Drawing.Size(107, 17)
        Me._TriggerDelayNumericLabel.TabIndex = 7
        Me._TriggerDelayNumericLabel.Text = "Trigger Delay [s]:"
        Me._TriggerDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseRangeNumericLabel
        '
        Me._SenseRangeNumericLabel.AutoSize = True
        Me._SenseRangeNumericLabel.Location = New System.Drawing.Point(51, 84)
        Me._SenseRangeNumericLabel.Name = "_SenseRangeNumericLabel"
        Me._SenseRangeNumericLabel.Size = New System.Drawing.Size(68, 17)
        Me._SenseRangeNumericLabel.TabIndex = 4
        Me._SenseRangeNumericLabel.Text = "Range [V]:"
        Me._SenseRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_IntegrationPeriodNumericLabel
        '
        Me._IntegrationPeriodNumericLabel.AutoSize = True
        Me._IntegrationPeriodNumericLabel.Location = New System.Drawing.Point(28, 50)
        Me._IntegrationPeriodNumericLabel.Name = "_IntegrationPeriodNumericLabel"
        Me._IntegrationPeriodNumericLabel.Size = New System.Drawing.Size(91, 17)
        Me._IntegrationPeriodNumericLabel.TabIndex = 2
        Me._IntegrationPeriodNumericLabel.Text = "Aperture [ms]:"
        Me._IntegrationPeriodNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseFunctionComboBox
        '
        Me._SenseFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._SenseFunctionComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseFunctionComboBox.Items.AddRange(New Object() {"I", "V"})
        Me._SenseFunctionComboBox.Location = New System.Drawing.Point(121, 12)
        Me._SenseFunctionComboBox.Name = "_SenseFunctionComboBox"
        Me._SenseFunctionComboBox.Size = New System.Drawing.Size(220, 25)
        Me._SenseFunctionComboBox.TabIndex = 1
        '
        '_SenseFunctionComboBoxLabel
        '
        Me._SenseFunctionComboBoxLabel.AutoSize = True
        Me._SenseFunctionComboBoxLabel.Location = New System.Drawing.Point(60, 16)
        Me._SenseFunctionComboBoxLabel.Name = "_SenseFunctionComboBoxLabel"
        Me._SenseFunctionComboBoxLabel.Size = New System.Drawing.Size(59, 17)
        Me._SenseFunctionComboBoxLabel.TabIndex = 0
        Me._SenseFunctionComboBoxLabel.Text = "Function:"
        Me._SenseFunctionComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseAutoRangeToggle
        '
        Me._SenseAutoRangeToggle.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseAutoRangeToggle.Location = New System.Drawing.Point(203, 82)
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
        Me._ClosedChannelsTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ClosedChannelsTextBox.Location = New System.Drawing.Point(7, 132)
        Me._ClosedChannelsTextBox.Multiline = True
        Me._ClosedChannelsTextBox.Name = "_ClosedChannelsTextBox"
        Me._ClosedChannelsTextBox.ReadOnly = True
        Me._ClosedChannelsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._ClosedChannelsTextBox.Size = New System.Drawing.Size(338, 67)
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
        '_ResetTabPage
        '
        Me._ResetTabPage.Controls.Add(Me._ResetLayout)
        Me._ResetTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ResetTabPage.Name = "_ResetTabPage"
        Me._ResetTabPage.Size = New System.Drawing.Size(356, 270)
        Me._ResetTabPage.TabIndex = 2
        Me._ResetTabPage.Text = "Reset"
        Me._ResetTabPage.UseVisualStyleBackColor = True
        '
        '_ResetLayout
        '
        Me._ResetLayout.ColumnCount = 3
        Me._ResetLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ResetLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ResetLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ResetLayout.Controls.Add(Me._InitializeKnownStateButton, 1, 7)
        Me._ResetLayout.Controls.Add(Me._InterfaceClearButton, 1, 1)
        Me._ResetLayout.Controls.Add(Me._ResetButton, 1, 5)
        Me._ResetLayout.Controls.Add(Me._SelectiveDeviceClearButton, 1, 3)
        Me._ResetLayout.Controls.Add(Me._SessionTraceEnableCheckBox, 1, 9)
        Me._ResetLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ResetLayout.Location = New System.Drawing.Point(0, 0)
        Me._ResetLayout.Name = "_ResetLayout"
        Me._ResetLayout.RowCount = 11
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ResetLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._ResetLayout.Size = New System.Drawing.Size(356, 270)
        Me._ResetLayout.TabIndex = 5
        '
        '_InitializeKnownStateButton
        '
        Me._InitializeKnownStateButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._InitializeKnownStateButton.Location = New System.Drawing.Point(84, 175)
        Me._InitializeKnownStateButton.Name = "_InitializeKnownStateButton"
        Me._InitializeKnownStateButton.Size = New System.Drawing.Size(182, 30)
        Me._InitializeKnownStateButton.TabIndex = 3
        Me._InitializeKnownStateButton.Text = "Initialize to Known State"
        Me.TipsTooltip.SetToolTip(Me._InitializeKnownStateButton, "Reset and Initialize Response Modes")
        Me._InitializeKnownStateButton.UseVisualStyleBackColor = True
        '
        '_InterfaceClearButton
        '
        Me._InterfaceClearButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._InterfaceClearButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._InterfaceClearButton.Location = New System.Drawing.Point(84, 19)
        Me._InterfaceClearButton.Name = "_InterfaceClearButton"
        Me._InterfaceClearButton.Size = New System.Drawing.Size(187, 30)
        Me._InterfaceClearButton.TabIndex = 0
        Me._InterfaceClearButton.Text = "&Clear Interface"
        Me._InterfaceClearButton.UseVisualStyleBackColor = True
        '
        '_ResetButton
        '
        Me._ResetButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ResetButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ResetButton.Location = New System.Drawing.Point(84, 123)
        Me._ResetButton.Name = "_ResetButton"
        Me._ResetButton.Size = New System.Drawing.Size(187, 30)
        Me._ResetButton.TabIndex = 2
        Me._ResetButton.Text = "&Reset to Known State"
        Me._ResetButton.UseVisualStyleBackColor = True
        '
        '_SelectiveDeviceClearButton
        '
        Me._SelectiveDeviceClearButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SelectiveDeviceClearButton.Location = New System.Drawing.Point(84, 71)
        Me._SelectiveDeviceClearButton.Name = "_SelectiveDeviceClearButton"
        Me._SelectiveDeviceClearButton.Size = New System.Drawing.Size(182, 30)
        Me._SelectiveDeviceClearButton.TabIndex = 1
        Me._SelectiveDeviceClearButton.Text = "Clear Device (SDC)"
        Me.TipsTooltip.SetToolTip(Me._SelectiveDeviceClearButton, "Issues Selective Device Clear.")
        Me._SelectiveDeviceClearButton.UseVisualStyleBackColor = True
        '
        '_SessionTraceEnableCheckBox
        '
        Me._SessionTraceEnableCheckBox.AutoSize = True
        Me._SessionTraceEnableCheckBox.Location = New System.Drawing.Point(84, 227)
        Me._SessionTraceEnableCheckBox.Name = "_SessionTraceEnableCheckBox"
        Me._SessionTraceEnableCheckBox.Size = New System.Drawing.Size(186, 21)
        Me._SessionTraceEnableCheckBox.TabIndex = 4
        Me._SessionTraceEnableCheckBox.Text = "Trace Instrument Messages"
        Me.TipsTooltip.SetToolTip(Me._SessionTraceEnableCheckBox, "Check to trace all instrument messages ")
        Me._SessionTraceEnableCheckBox.UseVisualStyleBackColor = True
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
        Me._SimpleReadWriteControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me._SimpleReadWriteControl.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SimpleReadWriteControl.Location = New System.Drawing.Point(0, 0)
        Me._SimpleReadWriteControl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SimpleReadWriteControl.MultipleSyncContextsExpected = False
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
        '_ComplianceToolStripStatusLabel
        '
        Me._FailureCodeToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._FailureCodeToolStripStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._FailureCodeToolStripStatusLabel.ForeColor = System.Drawing.Color.Red
        Me._FailureCodeToolStripStatusLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._FailureCodeToolStripStatusLabel.Name = "_ComplianceToolStripStatusLabel"
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
        Me._TitleLabel.Text = "K2700"
        Me._TitleLabel.UseMnemonic = False
        '
        'K2700Panel
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "K2700Panel"
        Me.Size = New System.Drawing.Size(364, 450)
        Me.Controls.SetChildIndex(Me.Connector, 0)
        Me.Controls.SetChildIndex(Me._Layout, 0)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Tabs.ResumeLayout(False)
        Me._ReadingTabPage.ResumeLayout(False)
        Me._ReadingTabPage.PerformLayout()
        Me._SenseTabPage.ResumeLayout(False)
        Me._SenseTabPage.PerformLayout()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._IntegrationPeriodNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ChannelTabPage.ResumeLayout(False)
        Me._ChannelTabPage.PerformLayout()
        Me._ResetTabPage.ResumeLayout(False)
        Me._ResetLayout.ResumeLayout(False)
        Me._ResetLayout.PerformLayout()
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
    Private WithEvents _TerminalsToggle As isr.Core.Controls.CheckBox
    Private WithEvents _ReadingComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _ReadButton As System.Windows.Forms.Button
    Private WithEvents _InitiateButton As System.Windows.Forms.Button
    Private WithEvents _ResetTabPage As System.Windows.Forms.TabPage
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
    Private WithEvents _ReadingComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _LastErrorTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ResetLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _InitializeKnownStateButton As System.Windows.Forms.Button
    Private WithEvents _InterfaceClearButton As System.Windows.Forms.Button
    Private WithEvents _ResetButton As System.Windows.Forms.Button
    Private WithEvents _SelectiveDeviceClearButton As System.Windows.Forms.Button
    Private WithEvents _SessionTraceEnableCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents _ReadingStatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _FailureCodeToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ReadingToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _TbdToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _HandleServiceRequestsCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents _LastReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _TriggerDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _SenseRangeNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _IntegrationPeriodNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TitleLabel As System.Windows.Forms.Label
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
End Class
