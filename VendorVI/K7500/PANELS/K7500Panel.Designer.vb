<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class K7500Panel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="nplc")>
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(K7500Panel))
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
        Me._GradeBinningToolStrip = New System.Windows.Forms.ToolStrip()
        Me._BinningToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._TriggerCountTextBoxLabel = New System.Windows.Forms.ToolStripLabel()
        Me._TriggerCountTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me._PassBitPatternNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._BitPatternFormatCheckBox = New isr.Core.Controls.ToolStripCheckBox()
        Me._PassBitPatternNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._ApplyGradeBinTriggerModelButton = New System.Windows.Forms.ToolStripButton()
        Me._TriggerDelayToolStrip = New System.Windows.Forms.ToolStrip()
        Me._TriggerDelaysToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._StartTriggerDelayNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._EndTriggerDelayNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._Limit1ToolStrip = New System.Windows.Forms.ToolStrip()
        Me._Limits1Label = New System.Windows.Forms.ToolStripLabel()
        Me._LowLimit1TextBox = New System.Windows.Forms.ToolStripTextBox()
        Me._HighLimit1TextBox = New System.Windows.Forms.ToolStripTextBox()
        Me._FailBitToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._HexLimit1CheckBox = New isr.Core.Controls.ToolStripCheckBox()
        Me._FailLimit1BitPatternNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
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
        Me._ApplyFunctionModeButton = New System.Windows.Forms.Button()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        Me._SenseTabPage.SuspendLayout()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SenseRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._PowerLineCyclesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._TriggerTabPage.SuspendLayout()
        Me._TriggerToolStripPanel.SuspendLayout()
        Me._GradeBinningToolStrip.SuspendLayout()
        Me._TriggerDelayToolStrip.SuspendLayout()
        Me._Limit1ToolStrip.SuspendLayout()
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
        Me._Tabs.Controls.Add(Me._TriggerTabPage)
        Me._Tabs.Controls.Add(Me._ResetTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(52, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 71)
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
        Me._TerminalsToggle.Size = New System.Drawing.Size(58, 30)
        Me._TerminalsToggle.TabIndex = 5
        Me._TerminalsToggle.Text = "Front"
        Me._TerminalsToggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.TipsTooltip.SetToolTip(Me._TerminalsToggle, "Reads front/rear terminals state")
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
        Me._SenseTabPage.Controls.Add(Me._ApplyFunctionModeButton)
        Me._SenseTabPage.Controls.Add(Me._TriggerDelayNumeric)
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
        Me._SenseTabPage.Size = New System.Drawing.Size(356, 270)
        Me._SenseTabPage.TabIndex = 4
        Me._SenseTabPage.Text = "Sense"
        Me._SenseTabPage.UseVisualStyleBackColor = True
        '
        '_TriggerDelayNumeric
        '
        Me._TriggerDelayNumeric.DecimalPlaces = 3
        Me._TriggerDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerDelayNumeric.Location = New System.Drawing.Point(106, 114)
        Me._TriggerDelayNumeric.Name = "_TriggerDelayNumeric"
        Me._TriggerDelayNumeric.Size = New System.Drawing.Size(76, 25)
        Me._TriggerDelayNumeric.TabIndex = 8
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
        Me.TipsTooltip.SetToolTip(Me._SenseRangeNumeric, "Range")
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
        Me.TipsTooltip.SetToolTip(Me._PowerLineCyclesNumeric, "Integration period")
        Me._PowerLineCyclesNumeric.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        '_TriggerDelayNumericLabel
        '
        Me._TriggerDelayNumericLabel.AutoSize = True
        Me._TriggerDelayNumericLabel.Location = New System.Drawing.Point(43, 118)
        Me._TriggerDelayNumericLabel.Name = "_TriggerDelayNumericLabel"
        Me._TriggerDelayNumericLabel.Size = New System.Drawing.Size(61, 17)
        Me._TriggerDelayNumericLabel.TabIndex = 7
        Me._TriggerDelayNumericLabel.Text = "Delay [s]:"
        Me._TriggerDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        Me._ApplySenseSettingsButton.Location = New System.Drawing.Point(283, 225)
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
        Me._TriggerTabPage.Size = New System.Drawing.Size(356, 270)
        Me._TriggerTabPage.TabIndex = 6
        Me._TriggerTabPage.Text = "Trig"
        Me._TriggerTabPage.UseVisualStyleBackColor = True
        '
        '_TriggerToolStripPanel
        '
        Me._TriggerToolStripPanel.Controls.Add(Me._GradeBinningToolStrip)
        Me._TriggerToolStripPanel.Controls.Add(Me._TriggerDelayToolStrip)
        Me._TriggerToolStripPanel.Controls.Add(Me._Limit1ToolStrip)
        Me._TriggerToolStripPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._TriggerToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me._TriggerToolStripPanel.Name = "_TriggerToolStripPanel"
        Me._TriggerToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me._TriggerToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me._TriggerToolStripPanel.Size = New System.Drawing.Size(356, 270)
        '
        '_GradeBinningToolStrip
        '
        Me._GradeBinningToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._GradeBinningToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._BinningToolStripLabel, Me._TriggerCountTextBoxLabel, Me._TriggerCountTextBox, Me._PassBitPatternNumericLabel, Me._BitPatternFormatCheckBox, Me._PassBitPatternNumeric, Me._ApplyGradeBinTriggerModelButton})
        Me._GradeBinningToolStrip.Location = New System.Drawing.Point(3, 0)
        Me._GradeBinningToolStrip.Name = "_GradeBinningToolStrip"
        Me._GradeBinningToolStrip.Size = New System.Drawing.Size(336, 28)
        Me._GradeBinningToolStrip.TabIndex = 0
        '
        '_BinningToolStripLabel
        '
        Me._BinningToolStripLabel.Name = "_BinningToolStripLabel"
        Me._BinningToolStripLabel.Size = New System.Drawing.Size(55, 25)
        Me._BinningToolStripLabel.Text = "BINNING"
        '
        '_TriggerCountTextBoxLabel
        '
        Me._TriggerCountTextBoxLabel.Name = "_TriggerCountTextBoxLabel"
        Me._TriggerCountTextBoxLabel.Size = New System.Drawing.Size(43, 25)
        Me._TriggerCountTextBoxLabel.Text = "Count:"
        '
        '_TriggerCountTextBox
        '
        Me._TriggerCountTextBox.AutoSize = False
        Me._TriggerCountTextBox.Name = "_TriggerCountTextBox"
        Me._TriggerCountTextBox.Size = New System.Drawing.Size(50, 25)
        Me._TriggerCountTextBox.Text = "10000"
        '
        '_PassBitPatternNumericLabel
        '
        Me._PassBitPatternNumericLabel.Name = "_PassBitPatternNumericLabel"
        Me._PassBitPatternNumericLabel.Size = New System.Drawing.Size(50, 25)
        Me._PassBitPatternNumericLabel.Text = "Pass Bit:"
        '
        '_BitPatternFormatCheckBox
        '
        Me._BitPatternFormatCheckBox.Checked = False
        Me._BitPatternFormatCheckBox.Name = "_BitPatternFormatCheckBox"
        Me._BitPatternFormatCheckBox.Size = New System.Drawing.Size(46, 25)
        Me._BitPatternFormatCheckBox.Text = "Hex"
        '
        '_PassBitPatternNumeric
        '
        Me._PassBitPatternNumeric.AutoSize = False
        Me._PassBitPatternNumeric.Name = "_PassBitPatternNumeric"
        Me._PassBitPatternNumeric.Size = New System.Drawing.Size(36, 25)
        Me._PassBitPatternNumeric.Text = "32"
        Me._PassBitPatternNumeric.ToolTipText = "Pass bit pattern"
        Me._PassBitPatternNumeric.Value = New Decimal(New Integer() {32, 0, 0, 0})
        '
        '_ApplyGradeBinTriggerModelButton
        '
        Me._ApplyGradeBinTriggerModelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ApplyGradeBinTriggerModelButton.Image = CType(resources.GetObject("_ApplyGradeBinTriggerModelButton.Image"), System.Drawing.Image)
        Me._ApplyGradeBinTriggerModelButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ApplyGradeBinTriggerModelButton.Name = "_ApplyGradeBinTriggerModelButton"
        Me._ApplyGradeBinTriggerModelButton.Size = New System.Drawing.Size(42, 25)
        Me._ApplyGradeBinTriggerModelButton.Text = "Apply"
        Me._ApplyGradeBinTriggerModelButton.ToolTipText = "Apply trigger model"
        '
        '_TriggerDelayToolStrip
        '
        Me._TriggerDelayToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._TriggerDelayToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._TriggerDelaysToolStripLabel, Me._StartTriggerDelayNumeric, Me._EndTriggerDelayNumeric})
        Me._TriggerDelayToolStrip.Location = New System.Drawing.Point(3, 28)
        Me._TriggerDelayToolStrip.Name = "_TriggerDelayToolStrip"
        Me._TriggerDelayToolStrip.Size = New System.Drawing.Size(181, 28)
        Me._TriggerDelayToolStrip.TabIndex = 1
        '
        '_TriggerDelaysToolStripLabel
        '
        Me._TriggerDelaysToolStripLabel.Name = "_TriggerDelaysToolStripLabel"
        Me._TriggerDelaysToolStripLabel.Size = New System.Drawing.Size(66, 25)
        Me._TriggerDelaysToolStripLabel.Text = "DELAYS [s]:"
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
        '_EndTriggerDelayNumeric
        '
        Me._EndTriggerDelayNumeric.Name = "_EndTriggerDelayNumeric"
        Me._EndTriggerDelayNumeric.Size = New System.Drawing.Size(41, 25)
        Me._EndTriggerDelayNumeric.Text = "0"
        Me._EndTriggerDelayNumeric.ToolTipText = "End delay in seconds"
        Me._EndTriggerDelayNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        '_Limit1ToolStrip
        '
        Me._Limit1ToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._Limit1ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._Limits1Label, Me._LowLimit1TextBox, Me._HighLimit1TextBox, Me._FailBitToolStripLabel, Me._HexLimit1CheckBox, Me._FailLimit1BitPatternNumeric})
        Me._Limit1ToolStrip.Location = New System.Drawing.Point(3, 56)
        Me._Limit1ToolStrip.Name = "_Limit1ToolStrip"
        Me._Limit1ToolStrip.Size = New System.Drawing.Size(314, 28)
        Me._Limit1ToolStrip.TabIndex = 2
        '
        '_Limits1Label
        '
        Me._Limits1Label.Name = "_Limits1Label"
        Me._Limits1Label.Size = New System.Drawing.Size(46, 25)
        Me._Limits1Label.Text = "LIMIT1:"
        '
        '_LowLimit1TextBox
        '
        Me._LowLimit1TextBox.AutoSize = False
        Me._LowLimit1TextBox.Name = "_LowLimit1TextBox"
        Me._LowLimit1TextBox.Size = New System.Drawing.Size(60, 25)
        Me._LowLimit1TextBox.Text = "0.000"
        Me._LowLimit1TextBox.ToolTipText = "Low limit"
        '
        '_HighLimit1TextBox
        '
        Me._HighLimit1TextBox.AutoSize = False
        Me._HighLimit1TextBox.Name = "_HighLimit1TextBox"
        Me._HighLimit1TextBox.Size = New System.Drawing.Size(60, 25)
        Me._HighLimit1TextBox.Text = "0.000"
        Me._HighLimit1TextBox.ToolTipText = "High limit"
        '
        '_FailBitToolStripLabel
        '
        Me._FailBitToolStripLabel.Name = "_FailBitToolStripLabel"
        Me._FailBitToolStripLabel.Size = New System.Drawing.Size(45, 25)
        Me._FailBitToolStripLabel.Text = "Fail Bit:"
        '
        '_HexLimit1CheckBox
        '
        Me._HexLimit1CheckBox.Checked = False
        Me._HexLimit1CheckBox.Name = "_HexLimit1CheckBox"
        Me._HexLimit1CheckBox.Size = New System.Drawing.Size(46, 25)
        Me._HexLimit1CheckBox.Text = "Hex"
        '
        '_FailLimit1BitPatternNumeric
        '
        Me._FailLimit1BitPatternNumeric.Name = "_FailLimit1BitPatternNumeric"
        Me._FailLimit1BitPatternNumeric.Size = New System.Drawing.Size(41, 25)
        Me._FailLimit1BitPatternNumeric.Text = "0"
        Me._FailLimit1BitPatternNumeric.ToolTipText = "Failed bit pattern"
        Me._FailLimit1BitPatternNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
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
        Me._FailureCodeToolStripStatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
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
        Me._TitleLabel.Text = "K7500"
        Me._TitleLabel.UseMnemonic = False
        '
        '_ApplyFunctionModeButton
        '
        Me._ApplyFunctionModeButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplyFunctionModeButton.Location = New System.Drawing.Point(306, 12)
        Me._ApplyFunctionModeButton.Name = "_ApplyFunctionModeButton"
        Me._ApplyFunctionModeButton.Size = New System.Drawing.Size(39, 25)
        Me._ApplyFunctionModeButton.TabIndex = 10
        Me._ApplyFunctionModeButton.Text = "SET"
        Me.TipsTooltip.SetToolTip(Me._ApplyFunctionModeButton, "Selects this function mode")
        Me._ApplyFunctionModeButton.UseVisualStyleBackColor = True
        '
        'K7500Panel
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "K7500Panel"
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
        CType(Me._PowerLineCyclesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._TriggerTabPage.ResumeLayout(False)
        Me._TriggerTabPage.PerformLayout()
        Me._TriggerToolStripPanel.ResumeLayout(False)
        Me._TriggerToolStripPanel.PerformLayout()
        Me._GradeBinningToolStrip.ResumeLayout(False)
        Me._GradeBinningToolStrip.PerformLayout()
        Me._TriggerDelayToolStrip.ResumeLayout(False)
        Me._TriggerDelayToolStrip.PerformLayout()
        Me._Limit1ToolStrip.ResumeLayout(False)
        Me._Limit1ToolStrip.PerformLayout()
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
    Private WithEvents _TerminalsToggle As Core.Controls.CheckBox
    Private WithEvents _ReadingComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _ReadButton As System.Windows.Forms.Button
    Private WithEvents _InitiateButton As System.Windows.Forms.Button
    Private WithEvents _ResetTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ApplySenseSettingsButton As System.Windows.Forms.Button
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseAutoRangeToggle As System.Windows.Forms.CheckBox
    Private WithEvents _SenseFunctionComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _SenseFunctionComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _PowerLineCyclesNumericLabel As System.Windows.Forms.Label
    Private WithEvents _SenseRangeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _TriggerDelayNumericLabel As System.Windows.Forms.Label
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
    Private WithEvents _TriggerCountTextBoxLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _TriggerCountTextBox As Windows.Forms.ToolStripTextBox
    Private WithEvents _TriggerDelayToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _TriggerDelaysToolStripLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _StartTriggerDelayNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _EndTriggerDelayNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _PassBitPatternNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _BitPatternFormatCheckBox As Core.Controls.ToolStripCheckBox
    Private WithEvents _PassBitPatternNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _Limit1ToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _Limits1Label As Windows.Forms.ToolStripLabel
    Private WithEvents _LowLimit1TextBox As Windows.Forms.ToolStripTextBox
    Private WithEvents _HighLimit1TextBox As Windows.Forms.ToolStripTextBox
    Private WithEvents _FailBitToolStripLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _HexLimit1CheckBox As Core.Controls.ToolStripCheckBox
    Private WithEvents _FailLimit1BitPatternNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _ApplyGradeBinTriggerModelButton As Windows.Forms.ToolStripButton
    Private WithEvents _ApplyFunctionModeButton As Windows.Forms.Button
End Class
