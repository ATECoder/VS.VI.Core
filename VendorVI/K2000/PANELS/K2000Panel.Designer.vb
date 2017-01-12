<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class K2000Panel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(K2000Panel))
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
        Me._ReadingsDataGridView = New System.Windows.Forms.DataGridView()
        Me._ReadingToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ReadButton = New System.Windows.Forms.ToolStripButton()
        Me._InitiateButton = New System.Windows.Forms.ToolStripButton()
        Me._TraceButton = New System.Windows.Forms.ToolStripButton()
        Me._ReadingsCountLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ReadingComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me._SystemToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ResetSplitButton = New System.Windows.Forms.ToolStripSplitButton()
        Me._ClearInterfaceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearDeviceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ResetKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._InitKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._TraceInstrumentMessagesMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._HandleServiceRequestsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._TerminalStateLabel = New System.Windows.Forms.ToolStripLabel()
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
        Me._ScanTabPage = New System.Windows.Forms.TabPage()
        Me._TriggerToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ApplyTriggerPlanButton = New System.Windows.Forms.ToolStripButton()
        Me._InitiateWaitReadButton = New System.Windows.Forms.ToolStripButton()
        Me._ConfigureExternalScan = New System.Windows.Forms.ToolStripButton()
        Me._ArmLayer1ToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ArmLayer1ToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ArmLayer1CountNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ArmLayer1CountNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._ArmLayer1SourceComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me._TriggerLayerToolStrip = New System.Windows.Forms.ToolStrip()
        Me._TriggerLayerToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ContinuousTriggerEnabledCheckBox = New isr.Core.Controls.ToolStripCheckBox()
        Me._TriggerCountNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._TriggerCountNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
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
        CType(Me._IntegrationPeriodNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ScanTabPage.SuspendLayout()
        Me._TriggerToolStrip.SuspendLayout()
        Me._ArmLayer1ToolStrip.SuspendLayout()
        Me._TriggerLayerToolStrip.SuspendLayout()
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
        Me._Tabs.Controls.Add(Me._ScanTabPage)
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
        Me._ReadingsDataGridView.Size = New System.Drawing.Size(356, 220)
        Me._ReadingsDataGridView.TabIndex = 19
        Me.TipsTooltip.SetToolTip(Me._ReadingsDataGridView, "Buffer data")
        '
        '_ReadingToolStrip
        '
        Me._ReadingToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ReadButton, Me._InitiateButton, Me._TraceButton, Me._ReadingsCountLabel, Me._ReadingComboBox})
        Me._ReadingToolStrip.Location = New System.Drawing.Point(0, 0)
        Me._ReadingToolStrip.Name = "_ReadingToolStrip"
        Me._ReadingToolStrip.Size = New System.Drawing.Size(356, 25)
        Me._ReadingToolStrip.TabIndex = 18
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
        Me._TraceButton.ToolTipText = "Read buffer"
        '
        '_ReadingsCountLabel
        '
        Me._ReadingsCountLabel.Name = "_ReadingsCountLabel"
        Me._ReadingsCountLabel.Size = New System.Drawing.Size(13, 22)
        Me._ReadingsCountLabel.Text = "0"
        '
        '_ReadingComboBox
        '
        Me._ReadingComboBox.Name = "_ReadingComboBox"
        Me._ReadingComboBox.Size = New System.Drawing.Size(121, 25)
        Me._ReadingComboBox.ToolTipText = "Select reading type"
        '
        '_SystemToolStrip
        '
        Me._SystemToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._SystemToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetSplitButton, Me._TerminalStateLabel})
        Me._SystemToolStrip.Location = New System.Drawing.Point(0, 245)
        Me._SystemToolStrip.Name = "_SystemToolStrip"
        Me._SystemToolStrip.Size = New System.Drawing.Size(356, 25)
        Me._SystemToolStrip.TabIndex = 17
        Me._SystemToolStrip.Text = "System Tools"
        Me.TipsTooltip.SetToolTip(Me._SystemToolStrip, "System operations")
        '
        '_ResetSplitButton
        '
        Me._ResetSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ResetSplitButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ClearInterfaceMenuItem, Me._ClearDeviceMenuItem, Me._ResetKnownStateMenuItem, Me._InitKnownStateMenuItem, Me._TraceInstrumentMessagesMenuItem, Me._HandleServiceRequestsMenuItem})
        Me._ResetSplitButton.Image = CType(resources.GetObject("_ResetSplitButton.Image"), System.Drawing.Image)
        Me._ResetSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ResetSplitButton.Name = "_ResetSplitButton"
        Me._ResetSplitButton.Size = New System.Drawing.Size(51, 22)
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
        '_TraceInstrumentMessagesMenuItem
        '
        Me._TraceInstrumentMessagesMenuItem.CheckOnClick = True
        Me._TraceInstrumentMessagesMenuItem.Name = "_TraceInstrumentMessagesMenuItem"
        Me._TraceInstrumentMessagesMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._TraceInstrumentMessagesMenuItem.Text = "Trace Instrument Messages"
        '
        '_HandleServiceRequestsMenuItem
        '
        Me._HandleServiceRequestsMenuItem.CheckOnClick = True
        Me._HandleServiceRequestsMenuItem.Name = "_HandleServiceRequestsMenuItem"
        Me._HandleServiceRequestsMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._HandleServiceRequestsMenuItem.Text = "Handle Service Requests"
        '
        '_TerminalStateLabel
        '
        Me._TerminalStateLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._TerminalStateLabel.Name = "_TerminalStateLabel"
        Me._TerminalStateLabel.Size = New System.Drawing.Size(35, 22)
        Me._TerminalStateLabel.Text = "Front"
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
        '_ScanTabPage
        '
        Me._ScanTabPage.Controls.Add(Me._TriggerToolStrip)
        Me._ScanTabPage.Controls.Add(Me._ArmLayer1ToolStrip)
        Me._ScanTabPage.Controls.Add(Me._TriggerLayerToolStrip)
        Me._ScanTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ScanTabPage.Name = "_ScanTabPage"
        Me._ScanTabPage.Size = New System.Drawing.Size(356, 270)
        Me._ScanTabPage.TabIndex = 2
        Me._ScanTabPage.Text = "Scan"
        Me._ScanTabPage.UseVisualStyleBackColor = True
        '
        '_TriggerToolStrip
        '
        Me._TriggerToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ApplyTriggerPlanButton, Me._InitiateWaitReadButton, Me._ConfigureExternalScan})
        Me._TriggerToolStrip.Location = New System.Drawing.Point(0, 56)
        Me._TriggerToolStrip.Name = "_TriggerToolStrip"
        Me._TriggerToolStrip.Size = New System.Drawing.Size(356, 25)
        Me._TriggerToolStrip.TabIndex = 5
        Me._TriggerToolStrip.Text = "Trigger"
        '
        '_ApplyTriggerPlanButton
        '
        Me._ApplyTriggerPlanButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ApplyTriggerPlanButton.Image = CType(resources.GetObject("_ApplyTriggerPlanButton.Image"), System.Drawing.Image)
        Me._ApplyTriggerPlanButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ApplyTriggerPlanButton.Name = "_ApplyTriggerPlanButton"
        Me._ApplyTriggerPlanButton.Size = New System.Drawing.Size(42, 22)
        Me._ApplyTriggerPlanButton.Text = "Apply"
        '
        '_InitiateWaitReadButton
        '
        Me._InitiateWaitReadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._InitiateWaitReadButton.Image = CType(resources.GetObject("_InitiateWaitReadButton.Image"), System.Drawing.Image)
        Me._InitiateWaitReadButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._InitiateWaitReadButton.Name = "_InitiateWaitReadButton"
        Me._InitiateWaitReadButton.Size = New System.Drawing.Size(109, 22)
        Me._InitiateWaitReadButton.Text = "Initiate, Wait, Read"
        Me._InitiateWaitReadButton.ToolTipText = "Starts the trigger plan, wait and then reads"
        '
        '_ConfigureExternalScan
        '
        Me._ConfigureExternalScan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ConfigureExternalScan.Image = CType(resources.GetObject("_ConfigureExternalScan.Image"), System.Drawing.Image)
        Me._ConfigureExternalScan.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ConfigureExternalScan.Name = "_ConfigureExternalScan"
        Me._ConfigureExternalScan.Size = New System.Drawing.Size(136, 22)
        Me._ConfigureExternalScan.Text = "Configure External Scan"
        '
        '_ArmLayer1ToolStrip
        '
        Me._ArmLayer1ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ArmLayer1ToolStripLabel, Me._ArmLayer1CountNumericLabel, Me._ArmLayer1CountNumeric, Me._ArmLayer1SourceComboBox})
        Me._ArmLayer1ToolStrip.Location = New System.Drawing.Point(0, 28)
        Me._ArmLayer1ToolStrip.Name = "_ArmLayer1ToolStrip"
        Me._ArmLayer1ToolStrip.Size = New System.Drawing.Size(356, 28)
        Me._ArmLayer1ToolStrip.TabIndex = 9
        Me._ArmLayer1ToolStrip.Text = "Arm1"
        '
        '_ArmLayer1ToolStripLabel
        '
        Me._ArmLayer1ToolStripLabel.Name = "_ArmLayer1ToolStripLabel"
        Me._ArmLayer1ToolStripLabel.Size = New System.Drawing.Size(42, 25)
        Me._ArmLayer1ToolStripLabel.Text = "ARM1:"
        Me._ArmLayer1ToolStripLabel.ToolTipText = "Arm layer settings"
        '
        '_ArmLayer1CountNumericLabel
        '
        Me._ArmLayer1CountNumericLabel.Name = "_ArmLayer1CountNumericLabel"
        Me._ArmLayer1CountNumericLabel.Size = New System.Drawing.Size(43, 25)
        Me._ArmLayer1CountNumericLabel.Text = "Count:"
        '
        '_ArmLayer1CountNumeric
        '
        Me._ArmLayer1CountNumeric.Name = "_ArmLayer1CountNumeric"
        Me._ArmLayer1CountNumeric.Size = New System.Drawing.Size(41, 25)
        Me._ArmLayer1CountNumeric.Text = "0"
        Me._ArmLayer1CountNumeric.ToolTipText = "Arm layer count"
        Me._ArmLayer1CountNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        '
        '_ArmLayer1SourceComboBox
        '
        Me._ArmLayer1SourceComboBox.Name = "_ArmLayer1SourceComboBox"
        Me._ArmLayer1SourceComboBox.Size = New System.Drawing.Size(90, 28)
        Me._ArmLayer1SourceComboBox.Text = "Immediate"
        Me._ArmLayer1SourceComboBox.ToolTipText = "Arm spacing"
        '
        '_TriggerLayerToolStrip
        '
        Me._TriggerLayerToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._TriggerLayerToolStripLabel, Me._ContinuousTriggerEnabledCheckBox, Me._TriggerCountNumericLabel, Me._TriggerCountNumeric})
        Me._TriggerLayerToolStrip.Location = New System.Drawing.Point(0, 0)
        Me._TriggerLayerToolStrip.Name = "_TriggerLayerToolStrip"
        Me._TriggerLayerToolStrip.Size = New System.Drawing.Size(356, 28)
        Me._TriggerLayerToolStrip.TabIndex = 6
        Me._TriggerLayerToolStrip.Text = "ToolStrip1"
        '
        '_TriggerLayerToolStripLabel
        '
        Me._TriggerLayerToolStripLabel.Name = "_TriggerLayerToolStripLabel"
        Me._TriggerLayerToolStripLabel.Size = New System.Drawing.Size(35, 25)
        Me._TriggerLayerToolStripLabel.Text = "TRIG:"
        '
        '_ContinuousTriggerEnabledCheckBox
        '
        Me._ContinuousTriggerEnabledCheckBox.Checked = False
        Me._ContinuousTriggerEnabledCheckBox.Name = "_ContinuousTriggerEnabledCheckBox"
        Me._ContinuousTriggerEnabledCheckBox.Size = New System.Drawing.Size(88, 25)
        Me._ContinuousTriggerEnabledCheckBox.Text = "Continuous"
        Me._ContinuousTriggerEnabledCheckBox.ToolTipText = "Continuous On/Off"
        '
        '_TriggerCountNumericLabel
        '
        Me._TriggerCountNumericLabel.Name = "_TriggerCountNumericLabel"
        Me._TriggerCountNumericLabel.Size = New System.Drawing.Size(43, 25)
        Me._TriggerCountNumericLabel.Text = "Count:"
        '
        '_TriggerCountNumeric
        '
        Me._TriggerCountNumeric.Name = "_TriggerCountNumeric"
        Me._TriggerCountNumeric.Size = New System.Drawing.Size(41, 25)
        Me._TriggerCountNumeric.Text = "0"
        Me._TriggerCountNumeric.ToolTipText = "Trigger Count"
        Me._TriggerCountNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
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
        Me._TitleLabel.Text = "K2000"
        Me._TitleLabel.UseMnemonic = False
        '
        'K2000Panel
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "K2000Panel"
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
        CType(Me._IntegrationPeriodNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ScanTabPage.ResumeLayout(False)
        Me._ScanTabPage.PerformLayout()
        Me._TriggerToolStrip.ResumeLayout(False)
        Me._TriggerToolStrip.PerformLayout()
        Me._ArmLayer1ToolStrip.ResumeLayout(False)
        Me._ArmLayer1ToolStrip.PerformLayout()
        Me._TriggerLayerToolStrip.ResumeLayout(False)
        Me._TriggerLayerToolStrip.PerformLayout()
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
    Private WithEvents _ScanTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ApplySenseSettingsButton As System.Windows.Forms.Button
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SenseAutoRangeToggle As System.Windows.Forms.CheckBox
    Private WithEvents _SenseFunctionComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _SenseFunctionComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _IntegrationPeriodNumericLabel As System.Windows.Forms.Label
    Private WithEvents _SenseRangeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _TriggerDelayNumericLabel As System.Windows.Forms.Label
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _LastErrorTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ReadingStatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _FailureCodeToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ReadingToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _TbdToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _LastReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _TriggerDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _SenseRangeNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _IntegrationPeriodNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TitleLabel As System.Windows.Forms.Label
    Friend WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
    Private WithEvents _SystemToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ResetSplitButton As Windows.Forms.ToolStripSplitButton
    Private WithEvents _ClearInterfaceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearDeviceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ResetKnownStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _InitKnownStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _TraceInstrumentMessagesMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _HandleServiceRequestsMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _TerminalStateLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _TriggerToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ApplyTriggerPlanButton As Windows.Forms.ToolStripButton
    Private WithEvents _InitiateWaitReadButton As Windows.Forms.ToolStripButton
    Private WithEvents _ConfigureExternalScan As Windows.Forms.ToolStripButton
    Friend WithEvents _TriggerLayerToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _TriggerLayerToolStripLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ContinuousTriggerEnabledCheckBox As Core.Controls.ToolStripCheckBox
    Friend WithEvents _TriggerCountNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _TriggerCountNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _ReadingsDataGridView As Windows.Forms.DataGridView
    Friend WithEvents _ReadingToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ReadButton As Windows.Forms.ToolStripButton
    Private WithEvents _InitiateButton As Windows.Forms.ToolStripButton
    Private WithEvents _TraceButton As Windows.Forms.ToolStripButton
    Private WithEvents _ReadingsCountLabel As Windows.Forms.ToolStripLabel
    Friend WithEvents _ReadingComboBox As Windows.Forms.ToolStripComboBox
    Private WithEvents _ArmLayer1ToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ArmLayer1ToolStripLabel As Windows.Forms.ToolStripLabel
    Friend WithEvents _ArmLayer1CountNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ArmLayer1CountNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _ArmLayer1SourceComboBox As Windows.Forms.ToolStripComboBox
End Class
