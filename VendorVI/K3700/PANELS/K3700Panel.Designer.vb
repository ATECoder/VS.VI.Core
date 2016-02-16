<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class K3700Panel

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
        Me._ReadingComboBox = New System.Windows.Forms.ComboBox()
        Me._ReadButton = New System.Windows.Forms.Button()
        Me._InitiateButton = New System.Windows.Forms.Button()
        Me._SenseTabPage = New System.Windows.Forms.TabPage()
        Me._ApplyFunctionModeButton = New System.Windows.Forms.Button()
        Me._AutoZeroCheckBox = New System.Windows.Forms.CheckBox()
        Me._FilterGroupBox = New System.Windows.Forms.GroupBox()
        Me._FilterWindowNumericLabel = New System.Windows.Forms.Label()
        Me._FilterWindowNumeric = New System.Windows.Forms.NumericUpDown()
        Me._RepeatingAverageRadioButton = New System.Windows.Forms.RadioButton()
        Me._MovingAverageRadioButton = New System.Windows.Forms.RadioButton()
        Me._FilterCountNumeric = New System.Windows.Forms.NumericUpDown()
        Me._FilterCountNumericLabel = New System.Windows.Forms.Label()
        Me._FilterEnabledCheckBox = New System.Windows.Forms.CheckBox()
        Me._SenseRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me._PowerLineCyclesNumeric = New System.Windows.Forms.NumericUpDown()
        Me._SenseRangeNumericLabel = New System.Windows.Forms.Label()
        Me._PowerLineCyclesNumericLabel = New System.Windows.Forms.Label()
        Me._SenseFunctionComboBox = New System.Windows.Forms.ComboBox()
        Me._SenseFunctionComboBoxLabel = New System.Windows.Forms.Label()
        Me._AutoRangeCheckBox = New System.Windows.Forms.CheckBox()
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
        Me._ComplianceToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._ReadingToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._TbdToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._LastReadingTextBox = New System.Windows.Forms.TextBox()
        Me._Panel = New System.Windows.Forms.Panel()
        Me._ChannelListTextBox = New System.Windows.Forms.TextBox()
        Me._TitleLabel = New System.Windows.Forms.Label()
        Me._Layout = New System.Windows.Forms.TableLayoutPanel()
        Me._AutoDelayCheckBox = New System.Windows.Forms.CheckBox()
        Me._OpenDetectorCheckBox = New System.Windows.Forms.CheckBox()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        Me._SenseTabPage.SuspendLayout()
        Me._FilterGroupBox.SuspendLayout()
        CType(Me._FilterWindowNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._FilterCountNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._PowerLineCyclesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TraceMessagesBox.Size = New System.Drawing.Size(356, 254)
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
        Me._Tabs.Location = New System.Drawing.Point(0, 87)
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 284)
        Me._Tabs.TabIndex = 5
        '
        '_ReadingTabPage
        '
        Me._ReadingTabPage.Controls.Add(Me._HandleServiceRequestsCheckBox)
        Me._ReadingTabPage.Controls.Add(Me._ReadingComboBoxLabel)
        Me._ReadingTabPage.Controls.Add(Me._ReadingComboBox)
        Me._ReadingTabPage.Controls.Add(Me._ReadButton)
        Me._ReadingTabPage.Controls.Add(Me._InitiateButton)
        Me._ReadingTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadingTabPage.Name = "_ReadingTabPage"
        Me._ReadingTabPage.Size = New System.Drawing.Size(356, 254)
        Me._ReadingTabPage.TabIndex = 0
        Me._ReadingTabPage.Text = "Reading"
        Me._ReadingTabPage.UseVisualStyleBackColor = True
        '
        '_HandleServiceRequestsCheckBox
        '
        Me._HandleServiceRequestsCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._HandleServiceRequestsCheckBox.AutoSize = True
        Me._HandleServiceRequestsCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._HandleServiceRequestsCheckBox.Location = New System.Drawing.Point(12, 222)
        Me._HandleServiceRequestsCheckBox.Name = "_HandleServiceRequestsCheckBox"
        Me._HandleServiceRequestsCheckBox.Size = New System.Drawing.Size(178, 21)
        Me._HandleServiceRequestsCheckBox.TabIndex = 4
        Me._HandleServiceRequestsCheckBox.Text = "Handle Service Requests"
        Me._HandleServiceRequestsCheckBox.UseVisualStyleBackColor = True
        '
        '_ReadingComboBoxLabel
        '
        Me._ReadingComboBoxLabel.AutoSize = True
        Me._ReadingComboBoxLabel.Location = New System.Drawing.Point(110, 13)
        Me._ReadingComboBoxLabel.Name = "_ReadingComboBoxLabel"
        Me._ReadingComboBoxLabel.Size = New System.Drawing.Size(59, 17)
        Me._ReadingComboBoxLabel.TabIndex = 2
        Me._ReadingComboBoxLabel.Text = "Reading:"
        Me._ReadingComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ReadingComboBox
        '
        Me._ReadingComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me._SenseTabPage.Controls.Add(Me._OpenDetectorCheckBox)
        Me._SenseTabPage.Controls.Add(Me._AutoDelayCheckBox)
        Me._SenseTabPage.Controls.Add(Me._ApplyFunctionModeButton)
        Me._SenseTabPage.Controls.Add(Me._AutoZeroCheckBox)
        Me._SenseTabPage.Controls.Add(Me._FilterGroupBox)
        Me._SenseTabPage.Controls.Add(Me._FilterEnabledCheckBox)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeNumeric)
        Me._SenseTabPage.Controls.Add(Me._PowerLineCyclesNumeric)
        Me._SenseTabPage.Controls.Add(Me._SenseRangeNumericLabel)
        Me._SenseTabPage.Controls.Add(Me._PowerLineCyclesNumericLabel)
        Me._SenseTabPage.Controls.Add(Me._SenseFunctionComboBox)
        Me._SenseTabPage.Controls.Add(Me._SenseFunctionComboBoxLabel)
        Me._SenseTabPage.Controls.Add(Me._AutoRangeCheckBox)
        Me._SenseTabPage.Controls.Add(Me._ApplySenseSettingsButton)
        Me._SenseTabPage.Location = New System.Drawing.Point(4, 26)
        Me._SenseTabPage.Name = "_SenseTabPage"
        Me._SenseTabPage.Size = New System.Drawing.Size(356, 254)
        Me._SenseTabPage.TabIndex = 4
        Me._SenseTabPage.Text = "Sense"
        Me._SenseTabPage.UseVisualStyleBackColor = True
        '
        '_ApplyFunctionModeButton
        '
        Me._ApplyFunctionModeButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplyFunctionModeButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ApplyFunctionModeButton.Location = New System.Drawing.Point(309, 5)
        Me._ApplyFunctionModeButton.Name = "_ApplyFunctionModeButton"
        Me._ApplyFunctionModeButton.Size = New System.Drawing.Size(39, 27)
        Me._ApplyFunctionModeButton.TabIndex = 2
        Me._ApplyFunctionModeButton.Text = "Set"
        Me.TipsTooltip.SetToolTip(Me._ApplyFunctionModeButton, "Apply the selected function mode.")
        Me._ApplyFunctionModeButton.UseVisualStyleBackColor = True
        '
        '_AutoZeroCheckBox
        '
        Me._AutoZeroCheckBox.AutoSize = True
        Me._AutoZeroCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._AutoZeroCheckBox.Location = New System.Drawing.Point(201, 38)
        Me._AutoZeroCheckBox.Name = "_AutoZeroCheckBox"
        Me._AutoZeroCheckBox.Size = New System.Drawing.Size(89, 21)
        Me._AutoZeroCheckBox.TabIndex = 5
        Me._AutoZeroCheckBox.Text = "Auto Zero"
        Me.TipsTooltip.SetToolTip(Me._AutoZeroCheckBox, "Auto zero")
        Me._AutoZeroCheckBox.UseVisualStyleBackColor = True
        '
        '_FilterGroupBox
        '
        Me._FilterGroupBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._FilterGroupBox.Controls.Add(Me._FilterWindowNumericLabel)
        Me._FilterGroupBox.Controls.Add(Me._FilterWindowNumeric)
        Me._FilterGroupBox.Controls.Add(Me._RepeatingAverageRadioButton)
        Me._FilterGroupBox.Controls.Add(Me._MovingAverageRadioButton)
        Me._FilterGroupBox.Controls.Add(Me._FilterCountNumeric)
        Me._FilterGroupBox.Controls.Add(Me._FilterCountNumericLabel)
        Me._FilterGroupBox.Location = New System.Drawing.Point(11, 169)
        Me._FilterGroupBox.Name = "_FilterGroupBox"
        Me._FilterGroupBox.Size = New System.Drawing.Size(258, 75)
        Me._FilterGroupBox.TabIndex = 10
        Me._FilterGroupBox.TabStop = False
        Me._FilterGroupBox.Text = "Filter"
        '
        '_FilterWindowNumericLabel
        '
        Me._FilterWindowNumericLabel.AutoSize = True
        Me._FilterWindowNumericLabel.Location = New System.Drawing.Point(3, 48)
        Me._FilterWindowNumericLabel.Name = "_FilterWindowNumericLabel"
        Me._FilterWindowNumericLabel.Size = New System.Drawing.Size(81, 17)
        Me._FilterWindowNumericLabel.TabIndex = 3
        Me._FilterWindowNumericLabel.Text = "Window [%]:"
        '
        '_FilterWindowNumeric
        '
        Me._FilterWindowNumeric.DecimalPlaces = 2
        Me._FilterWindowNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._FilterWindowNumeric.Location = New System.Drawing.Point(87, 44)
        Me._FilterWindowNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me._FilterWindowNumeric.Name = "_FilterWindowNumeric"
        Me._FilterWindowNumeric.Size = New System.Drawing.Size(54, 25)
        Me._FilterWindowNumeric.TabIndex = 4
        Me.TipsTooltip.SetToolTip(Me._FilterWindowNumeric, "Average filter window")
        Me._FilterWindowNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        '_RepeatingAverageRadioButton
        '
        Me._RepeatingAverageRadioButton.AutoSize = True
        Me._RepeatingAverageRadioButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._RepeatingAverageRadioButton.Location = New System.Drawing.Point(160, 46)
        Me._RepeatingAverageRadioButton.Name = "_RepeatingAverageRadioButton"
        Me._RepeatingAverageRadioButton.Size = New System.Drawing.Size(88, 21)
        Me._RepeatingAverageRadioButton.TabIndex = 5
        Me._RepeatingAverageRadioButton.Text = "Repeating"
        Me.TipsTooltip.SetToolTip(Me._RepeatingAverageRadioButton, "Repeating average")
        Me._RepeatingAverageRadioButton.UseVisualStyleBackColor = True
        '
        '_MovingAverageRadioButton
        '
        Me._MovingAverageRadioButton.AutoSize = True
        Me._MovingAverageRadioButton.Checked = True
        Me._MovingAverageRadioButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._MovingAverageRadioButton.Location = New System.Drawing.Point(160, 17)
        Me._MovingAverageRadioButton.Name = "_MovingAverageRadioButton"
        Me._MovingAverageRadioButton.Size = New System.Drawing.Size(73, 21)
        Me._MovingAverageRadioButton.TabIndex = 2
        Me._MovingAverageRadioButton.TabStop = True
        Me._MovingAverageRadioButton.Text = "Moving"
        Me._MovingAverageRadioButton.UseVisualStyleBackColor = True
        '
        '_FilterCountNumeric
        '
        Me._FilterCountNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._FilterCountNumeric.Location = New System.Drawing.Point(87, 15)
        Me._FilterCountNumeric.Name = "_FilterCountNumeric"
        Me._FilterCountNumeric.Size = New System.Drawing.Size(53, 25)
        Me._FilterCountNumeric.TabIndex = 1
        Me.TipsTooltip.SetToolTip(Me._FilterCountNumeric, "Filter count")
        Me._FilterCountNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        '_FilterCountNumericLabel
        '
        Me._FilterCountNumericLabel.AutoSize = True
        Me._FilterCountNumericLabel.Location = New System.Drawing.Point(39, 18)
        Me._FilterCountNumericLabel.Name = "_FilterCountNumericLabel"
        Me._FilterCountNumericLabel.Size = New System.Drawing.Size(45, 17)
        Me._FilterCountNumericLabel.TabIndex = 0
        Me._FilterCountNumericLabel.Text = "Count:"
        Me._FilterCountNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_FilterEnabledCheckBox
        '
        Me._FilterEnabledCheckBox.AutoSize = True
        Me._FilterEnabledCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._FilterEnabledCheckBox.Location = New System.Drawing.Point(201, 88)
        Me._FilterEnabledCheckBox.Name = "_FilterEnabledCheckBox"
        Me._FilterEnabledCheckBox.Size = New System.Drawing.Size(112, 21)
        Me._FilterEnabledCheckBox.TabIndex = 9
        Me._FilterEnabledCheckBox.Text = "Filter Enabled"
        Me.TipsTooltip.SetToolTip(Me._FilterEnabledCheckBox, "Checked to enable filtering")
        Me._FilterEnabledCheckBox.UseVisualStyleBackColor = True
        '
        '_SenseRangeNumeric
        '
        Me._SenseRangeNumeric.DecimalPlaces = 3
        Me._SenseRangeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SenseRangeNumeric.Location = New System.Drawing.Point(105, 66)
        Me._SenseRangeNumeric.Maximum = New Decimal(New Integer() {1010, 0, 0, 0})
        Me._SenseRangeNumeric.Name = "_SenseRangeNumeric"
        Me._SenseRangeNumeric.Size = New System.Drawing.Size(81, 25)
        Me._SenseRangeNumeric.TabIndex = 7
        Me.TipsTooltip.SetToolTip(Me._SenseRangeNumeric, "Range")
        Me._SenseRangeNumeric.Value = New Decimal(New Integer() {105, 0, 0, 196608})
        '
        '_PowerLineCyclesNumeric
        '
        Me._PowerLineCyclesNumeric.DecimalPlaces = 3
        Me._PowerLineCyclesNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PowerLineCyclesNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._PowerLineCyclesNumeric.Location = New System.Drawing.Point(105, 36)
        Me._PowerLineCyclesNumeric.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
        Me._PowerLineCyclesNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 196608})
        Me._PowerLineCyclesNumeric.Name = "_PowerLineCyclesNumeric"
        Me._PowerLineCyclesNumeric.Size = New System.Drawing.Size(81, 25)
        Me._PowerLineCyclesNumeric.TabIndex = 4
        Me.TipsTooltip.SetToolTip(Me._PowerLineCyclesNumeric, "Integration period in power line cycles.")
        Me._PowerLineCyclesNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        '_SenseRangeNumericLabel
        '
        Me._SenseRangeNumericLabel.AutoSize = True
        Me._SenseRangeNumericLabel.Location = New System.Drawing.Point(35, 70)
        Me._SenseRangeNumericLabel.Name = "_SenseRangeNumericLabel"
        Me._SenseRangeNumericLabel.Size = New System.Drawing.Size(68, 17)
        Me._SenseRangeNumericLabel.TabIndex = 6
        Me._SenseRangeNumericLabel.Text = "Range [V]:"
        Me._SenseRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_PowerLineCyclesNumericLabel
        '
        Me._PowerLineCyclesNumericLabel.AutoSize = True
        Me._PowerLineCyclesNumericLabel.Location = New System.Drawing.Point(5, 40)
        Me._PowerLineCyclesNumericLabel.Name = "_PowerLineCyclesNumericLabel"
        Me._PowerLineCyclesNumericLabel.Size = New System.Drawing.Size(98, 17)
        Me._PowerLineCyclesNumericLabel.TabIndex = 3
        Me._PowerLineCyclesNumericLabel.Text = "Aperture [nplc]:"
        Me._PowerLineCyclesNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseFunctionComboBox
        '
        Me._SenseFunctionComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SenseFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._SenseFunctionComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SenseFunctionComboBox.Items.AddRange(New Object() {"I", "V"})
        Me._SenseFunctionComboBox.Location = New System.Drawing.Point(105, 6)
        Me._SenseFunctionComboBox.Name = "_SenseFunctionComboBox"
        Me._SenseFunctionComboBox.Size = New System.Drawing.Size(199, 25)
        Me._SenseFunctionComboBox.TabIndex = 1
        '
        '_SenseFunctionComboBoxLabel
        '
        Me._SenseFunctionComboBoxLabel.AutoSize = True
        Me._SenseFunctionComboBoxLabel.Location = New System.Drawing.Point(37, 10)
        Me._SenseFunctionComboBoxLabel.Name = "_SenseFunctionComboBoxLabel"
        Me._SenseFunctionComboBoxLabel.Size = New System.Drawing.Size(59, 17)
        Me._SenseFunctionComboBoxLabel.TabIndex = 0
        Me._SenseFunctionComboBoxLabel.Text = "Function:"
        Me._SenseFunctionComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_AutoRangeCheckBox
        '
        Me._AutoRangeCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._AutoRangeCheckBox.Location = New System.Drawing.Point(201, 62)
        Me._AutoRangeCheckBox.Name = "_AutoRangeCheckBox"
        Me._AutoRangeCheckBox.Size = New System.Drawing.Size(103, 21)
        Me._AutoRangeCheckBox.TabIndex = 8
        Me._AutoRangeCheckBox.Text = "Auto Range"
        Me._AutoRangeCheckBox.UseVisualStyleBackColor = True
        '
        '_ApplySenseSettingsButton
        '
        Me._ApplySenseSettingsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplySenseSettingsButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ApplySenseSettingsButton.Location = New System.Drawing.Point(290, 212)
        Me._ApplySenseSettingsButton.Name = "_ApplySenseSettingsButton"
        Me._ApplySenseSettingsButton.Size = New System.Drawing.Size(58, 30)
        Me._ApplySenseSettingsButton.TabIndex = 11
        Me._ApplySenseSettingsButton.Text = "&Apply"
        Me.TipsTooltip.SetToolTip(Me._ApplySenseSettingsButton, "Applies new settings")
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
        Me._ChannelTabPage.Size = New System.Drawing.Size(356, 254)
        Me._ChannelTabPage.TabIndex = 1
        Me._ChannelTabPage.Text = "Channel"
        Me._ChannelTabPage.UseVisualStyleBackColor = True
        '
        '_ClosedChannelsTextBoxLabel
        '
        Me._ClosedChannelsTextBoxLabel.AutoSize = True
        Me._ClosedChannelsTextBoxLabel.Location = New System.Drawing.Point(9, 110)
        Me._ClosedChannelsTextBoxLabel.Name = "_ClosedChannelsTextBoxLabel"
        Me._ClosedChannelsTextBoxLabel.Size = New System.Drawing.Size(51, 17)
        Me._ClosedChannelsTextBoxLabel.TabIndex = 6
        Me._ClosedChannelsTextBoxLabel.Text = "Closed:"
        '
        '_ClosedChannelsTextBox
        '
        Me._ClosedChannelsTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ClosedChannelsTextBox.Location = New System.Drawing.Point(8, 132)
        Me._ClosedChannelsTextBox.Multiline = True
        Me._ClosedChannelsTextBox.Name = "_ClosedChannelsTextBox"
        Me._ClosedChannelsTextBox.ReadOnly = True
        Me._ClosedChannelsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._ClosedChannelsTextBox.Size = New System.Drawing.Size(340, 111)
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
        Me._OpenChannelsButton.TabIndex = 1
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
        Me._CloseChannelsButton.TabIndex = 0
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
        Me._ChannelListComboBox.Text = "1921,1912,1001,1031"
        '
        '_ChannelListComboBoxLabel
        '
        Me._ChannelListComboBoxLabel.AutoSize = True
        Me._ChannelListComboBoxLabel.Location = New System.Drawing.Point(7, 22)
        Me._ChannelListComboBoxLabel.Name = "_ChannelListComboBoxLabel"
        Me._ChannelListComboBoxLabel.Size = New System.Drawing.Size(80, 17)
        Me._ChannelListComboBoxLabel.TabIndex = 2
        Me._ChannelListComboBoxLabel.Text = "Channel List:"
        '
        '_ResetTabPage
        '
        Me._ResetTabPage.Controls.Add(Me._ResetLayout)
        Me._ResetTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ResetTabPage.Name = "_ResetTabPage"
        Me._ResetTabPage.Size = New System.Drawing.Size(356, 254)
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
        Me._ResetLayout.Size = New System.Drawing.Size(356, 254)
        Me._ResetLayout.TabIndex = 5
        '
        '_InitializeKnownStateButton
        '
        Me._InitializeKnownStateButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._InitializeKnownStateButton.Location = New System.Drawing.Point(84, 163)
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
        Me._InterfaceClearButton.Location = New System.Drawing.Point(84, 16)
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
        Me._ResetButton.Location = New System.Drawing.Point(84, 114)
        Me._ResetButton.Name = "_ResetButton"
        Me._ResetButton.Size = New System.Drawing.Size(187, 30)
        Me._ResetButton.TabIndex = 2
        Me._ResetButton.Text = "&Reset to Known State"
        Me._ResetButton.UseVisualStyleBackColor = True
        '
        '_SelectiveDeviceClearButton
        '
        Me._SelectiveDeviceClearButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SelectiveDeviceClearButton.Location = New System.Drawing.Point(84, 65)
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
        Me._SessionTraceEnableCheckBox.Location = New System.Drawing.Point(84, 212)
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
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(356, 254)
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
        Me._SimpleReadWriteControl.Size = New System.Drawing.Size(356, 254)
        Me._SimpleReadWriteControl.TabIndex = 0
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me.TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(356, 254)
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
        Me._LastErrorTextBox.Location = New System.Drawing.Point(0, 69)
        Me._LastErrorTextBox.Name = "_LastErrorTextBox"
        Me._LastErrorTextBox.Size = New System.Drawing.Size(364, 18)
        Me._LastErrorTextBox.TabIndex = 4
        Me._LastErrorTextBox.TabStop = False
        Me._LastErrorTextBox.Text = "000, No Errors"
        '
        '_ReadingStatusStrip
        '
        Me._ReadingStatusStrip.BackColor = System.Drawing.Color.Black
        Me._ReadingStatusStrip.Dock = System.Windows.Forms.DockStyle.Top
        Me._ReadingStatusStrip.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ReadingStatusStrip.GripMargin = New System.Windows.Forms.Padding(0)
        Me._ReadingStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ComplianceToolStripStatusLabel, Me._ReadingToolStripStatusLabel, Me._TbdToolStripStatusLabel})
        Me._ReadingStatusStrip.Location = New System.Drawing.Point(0, 0)
        Me._ReadingStatusStrip.Name = "_ReadingStatusStrip"
        Me._ReadingStatusStrip.Size = New System.Drawing.Size(364, 37)
        Me._ReadingStatusStrip.SizingGrip = False
        Me._ReadingStatusStrip.TabIndex = 1
        '
        '_ComplianceToolStripStatusLabel
        '
        Me._ComplianceToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ComplianceToolStripStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ComplianceToolStripStatusLabel.ForeColor = System.Drawing.Color.Red
        Me._ComplianceToolStripStatusLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._ComplianceToolStripStatusLabel.Name = "_ComplianceToolStripStatusLabel"
        Me._ComplianceToolStripStatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._ComplianceToolStripStatusLabel.Size = New System.Drawing.Size(16, 37)
        Me._ComplianceToolStripStatusLabel.Text = "C"
        Me._ComplianceToolStripStatusLabel.ToolTipText = "Compliance"
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
        Me._TbdToolStripStatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
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
        Me._LastReadingTextBox.Margin = New System.Windows.Forms.Padding(0)
        Me._LastReadingTextBox.Name = "_LastReadingTextBox"
        Me._LastReadingTextBox.Size = New System.Drawing.Size(364, 16)
        Me._LastReadingTextBox.TabIndex = 2
        Me._LastReadingTextBox.TabStop = False
        Me._LastReadingTextBox.Text = "<last reading>"
        Me._LastReadingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_Panel
        '
        Me._Panel.Controls.Add(Me._Tabs)
        Me._Panel.Controls.Add(Me._LastErrorTextBox)
        Me._Panel.Controls.Add(Me._ChannelListTextBox)
        Me._Panel.Controls.Add(Me._LastReadingTextBox)
        Me._Panel.Controls.Add(Me._ReadingStatusStrip)
        Me._Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Panel.Location = New System.Drawing.Point(0, 17)
        Me._Panel.Margin = New System.Windows.Forms.Padding(0)
        Me._Panel.Name = "_Panel"
        Me._Panel.Size = New System.Drawing.Size(364, 371)
        Me._Panel.TabIndex = 16
        '
        '_ChannelListTextBox
        '
        Me._ChannelListTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._ChannelListTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._ChannelListTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._ChannelListTextBox.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ChannelListTextBox.ForeColor = System.Drawing.Color.Snow
        Me._ChannelListTextBox.Location = New System.Drawing.Point(0, 53)
        Me._ChannelListTextBox.Margin = New System.Windows.Forms.Padding(0)
        Me._ChannelListTextBox.Name = "_ChannelListTextBox"
        Me._ChannelListTextBox.Size = New System.Drawing.Size(364, 16)
        Me._ChannelListTextBox.TabIndex = 3
        Me._ChannelListTextBox.TabStop = False
        Me._ChannelListTextBox.Text = "<channels>"
        Me._ChannelListTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
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
        Me._TitleLabel.Text = "K3700"
        Me._TitleLabel.UseMnemonic = False
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
        '_AutoDelayCheckBox
        '
        Me._AutoDelayCheckBox.AutoSize = True
        Me._AutoDelayCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._AutoDelayCheckBox.Location = New System.Drawing.Point(201, 114)
        Me._AutoDelayCheckBox.Name = "_AutoDelayCheckBox"
        Me._AutoDelayCheckBox.Size = New System.Drawing.Size(128, 21)
        Me._AutoDelayCheckBox.TabIndex = 12
        Me._AutoDelayCheckBox.Text = "Auto Delay (off)"
        Me._AutoDelayCheckBox.ThreeState = True
        Me.TipsTooltip.SetToolTip(Me._AutoDelayCheckBox, "Toggles auto delay.")
        Me._AutoDelayCheckBox.UseVisualStyleBackColor = True
        '
        '_OpenDetectorCheckBox
        '
        Me._OpenDetectorCheckBox.AutoSize = True
        Me._OpenDetectorCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._OpenDetectorCheckBox.Location = New System.Drawing.Point(201, 140)
        Me._OpenDetectorCheckBox.Name = "_OpenDetectorCheckBox"
        Me._OpenDetectorCheckBox.Size = New System.Drawing.Size(149, 21)
        Me._OpenDetectorCheckBox.TabIndex = 13
        Me._OpenDetectorCheckBox.Text = "Open Detector (off)"
        Me.TipsTooltip.SetToolTip(Me._OpenDetectorCheckBox, "Toggles detection of open contacts.")
        Me._OpenDetectorCheckBox.UseVisualStyleBackColor = True
        '
        'K3700Panel
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "K3700Panel"
        Me.Size = New System.Drawing.Size(364, 450)
        Me.Controls.SetChildIndex(Me.Connector, 0)
        Me.Controls.SetChildIndex(Me._Layout, 0)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Tabs.ResumeLayout(False)
        Me._ReadingTabPage.ResumeLayout(False)
        Me._ReadingTabPage.PerformLayout()
        Me._SenseTabPage.ResumeLayout(False)
        Me._SenseTabPage.PerformLayout()
        Me._FilterGroupBox.ResumeLayout(False)
        Me._FilterGroupBox.PerformLayout()
        CType(Me._FilterWindowNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._FilterCountNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._PowerLineCyclesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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
    Private WithEvents _ReadingComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _ReadButton As System.Windows.Forms.Button
    Private WithEvents _InitiateButton As System.Windows.Forms.Button
    Private WithEvents _ResetTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ChannelTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ApplySenseSettingsButton As System.Windows.Forms.Button
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _AutoRangeCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents _SenseFunctionComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _SenseFunctionComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _PowerLineCyclesNumericLabel As System.Windows.Forms.Label
    Private WithEvents _SenseRangeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _FilterCountNumericLabel As System.Windows.Forms.Label
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
    Private WithEvents _ComplianceToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ReadingToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _TbdToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _HandleServiceRequestsCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents _LastReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _FilterCountNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _SenseRangeNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _PowerLineCyclesNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
    Private WithEvents _ChannelListTextBox As Windows.Forms.TextBox
    Private WithEvents _TitleLabel As Windows.Forms.Label
    Private WithEvents _FilterGroupBox As Windows.Forms.GroupBox
    Private WithEvents _AutoZeroCheckBox As Windows.Forms.CheckBox
    Private WithEvents _RepeatingAverageRadioButton As Windows.Forms.RadioButton
    Private WithEvents _MovingAverageRadioButton As Windows.Forms.RadioButton
    Private WithEvents _FilterEnabledCheckBox As Windows.Forms.CheckBox
    Private WithEvents _FilterWindowNumericLabel As Windows.Forms.Label
    Private WithEvents _FilterWindowNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _ApplyFunctionModeButton As Windows.Forms.Button
    Friend WithEvents _OpenDetectorCheckBox As Windows.Forms.CheckBox
    Friend WithEvents _AutoDelayCheckBox As Windows.Forms.CheckBox
End Class
