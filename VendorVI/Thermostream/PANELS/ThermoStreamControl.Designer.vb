<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ThermostreamControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ThermostreamPanel))
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
        Me._SrqRefratoryTimeNumericLabel = New System.Windows.Forms.Label()
        Me._SrqRefratoryTimeNumeric = New isr.Core.Controls.NumericUpDown()
        Me._SendComboBox = New System.Windows.Forms.ComboBox()
        Me._ReceiveTextBox = New System.Windows.Forms.TextBox()
        Me._ReceiveButton = New System.Windows.Forms.Button()
        Me._SendButton = New System.Windows.Forms.Button()
        Me._StatusByteLabel = New System.Windows.Forms.Label()
        Me._ReadStatusByteButton = New System.Windows.Forms.Button()
        Me._ToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me._ClearErrorQueueButton = New System.Windows.Forms.Button()
        Me._ReadLastErrorButton = New System.Windows.Forms.Button()
        Me._ReadButton = New System.Windows.Forms.Button()
        Me._InitializeButton = New System.Windows.Forms.Button()
        Me._FunctionTabPage = New System.Windows.Forms.TabPage()
        Me._QueryStatusButton = New System.Windows.Forms.Button()
        Me._ResourceSelectorConnector = New isr.VI.Instrument.ResourceSelectorConnector()
        Me._OperatorCycleTabs = New System.Windows.Forms.TabControl()
        Me._CycleTabPage1 = New System.Windows.Forms.TabPage()
        Me._CycleToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ResetCycleModeButton = New System.Windows.Forms.ToolStripButton()
        Me._StartCycleButton = New System.Windows.Forms.ToolStripButton()
        Me._NextSetpointButton = New System.Windows.Forms.ToolStripButton()
        Me._StopCycleButton = New System.Windows.Forms.ToolStripButton()
        Me._CyclesCompletedCheckBox1 = New isr.Core.Controls.ToolStripCheckBox()
        Me._CycleCompletedCheckBox1 = New isr.Core.Controls.ToolStripCheckBox()
        Me._CyclingStoppedCheckBox = New isr.Core.Controls.ToolStripCheckBox()
        Me._FindLastSetpointButton1 = New System.Windows.Forms.ToolStripButton()
        Me._OperatorTabPage = New System.Windows.Forms.TabPage()
        Me._ResetDelayNumericLabel = New System.Windows.Forms.Label()
        Me._ResetQueryDelayNumeric = New isr.Core.Controls.NumericUpDown()
        Me._ResetOperatorModeButton = New System.Windows.Forms.Button()
        Me._SetpointNumberNumeric = New isr.Core.Controls.SelectorNumeric()
        Me._TestTimeElapsedCheckBox = New isr.Core.Controls.CheckBox()
        Me._CycleCountNumericLabel = New System.Windows.Forms.Label()
        Me._CycleCountNumeric = New isr.Core.Controls.SelectorNumeric()
        Me._MaxTestTimeNumeric = New isr.Core.Controls.SelectorNumeric()
        Me._MaxTestTimeNumericLabel = New System.Windows.Forms.Label()
        Me._ReadSetpointButton = New System.Windows.Forms.Button()
        Me._ReadyCheckBox = New isr.Core.Controls.CheckBox()
        Me._NotAtTempCheckBox = New isr.Core.Controls.CheckBox()
        Me._AtTempCheckBox = New isr.Core.Controls.CheckBox()
        Me._HeadDownCheckBox = New isr.Core.Controls.CheckBox()
        Me._SetpointWindowNumericLabel = New System.Windows.Forms.Label()
        Me._SetpointWindowNumeric = New isr.Core.Controls.SelectorNumeric()
        Me._SetpointNumberNumericLabel = New System.Windows.Forms.Label()
        Me._RampRateNumeric = New isr.Core.Controls.SelectorNumeric()
        Me._SoakTimeNumeric = New isr.Core.Controls.SelectorNumeric()
        Me._SetpointNumeric = New isr.Core.Controls.SelectorNumeric()
        Me._RampRateNumericLabel = New System.Windows.Forms.Label()
        Me._SoakTimeNumericLabel = New System.Windows.Forms.Label()
        Me._SetpointNumericLabel = New System.Windows.Forms.Label()
        Me._SensorTabPage = New System.Windows.Forms.TabPage()
        Me._DeviceSensorTypeSelectorLabel = New System.Windows.Forms.Label()
        Me._DeviceThermalConstantSelectorLabel = New System.Windows.Forms.Label()
        Me._DeviceSensorTypeSelector = New isr.Core.Controls.SelectorComboBox()
        Me._DeviceThermalConstantSelector = New isr.Core.Controls.SelectorNumeric()
        Me._ReadWriteTabPage = New System.Windows.Forms.TabPage()
        Me._SimpleReadWriteControl = New isr.VI.Instrument.SimpleReadWriteControl()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._TraceMessagesBox = New isr.Core.Pith.TraceMessagesBox()
        Me._LastErrorTextBox = New System.Windows.Forms.TextBox()
        Me._ReadingStatusStrip = New System.Windows.Forms.StatusStrip()
        Me._StatusRegisterLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._StandardRegisterLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._ComplianceToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._ReadingToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._StatusStrip = New System.Windows.Forms.StatusStrip()
        Me._StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._IdentityLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._TipsToolTip = New System.Windows.Forms.ToolTip()
        Me._InfoProvider = New isr.Core.Controls.InfoProvider()
        Me._LastReadingTextBox = New System.Windows.Forms.TextBox()
        Me._Panel = New System.Windows.Forms.Panel()
        Me._Layout = New System.Windows.Forms.TableLayoutPanel()
        Me._TitleLabel = New System.Windows.Forms.Label()
        Me._LogTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._DisplayTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._ServiceRequestValuesTextBox = New System.Windows.Forms.TextBox()
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
        Me._ReadTerminalStateButton = New System.Windows.Forms.ToolStripButton()
        Me._ServiceRequestEnableNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ServiceRequestEnableNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        CType(Me._SrqRefratoryTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._FunctionTabPage.SuspendLayout()
        Me._ToolStripPanel.SuspendLayout()
        Me._OperatorCycleTabs.SuspendLayout()
        Me._CycleTabPage1.SuspendLayout()
        Me._CycleToolStrip.SuspendLayout()
        Me._OperatorTabPage.SuspendLayout()
        CType(Me._ResetQueryDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._SensorTabPage.SuspendLayout()
        Me._ReadWriteTabPage.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
        Me._ReadingStatusStrip.SuspendLayout()
        Me._StatusStrip.SuspendLayout()
        CType(Me._InfoProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Panel.SuspendLayout()
        Me._Layout.SuspendLayout()
        Me._SystemToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'Connector
        '
        Me.Connector.Location = New System.Drawing.Point(0, 388)
        Me.Connector.Searchable = True
        Me.Connector.TabIndex = 4
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ReadingTabPage)
        Me._Tabs.Controls.Add(Me._FunctionTabPage)
        Me._Tabs.Controls.Add(Me._SensorTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(60, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 71)
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 300)
        Me._Tabs.TabIndex = 0
        '
        '_ReadingTabPage
        '
        Me._ReadingTabPage.Controls.Add(Me._SystemToolStrip)
        Me._ReadingTabPage.Controls.Add(Me._ServiceRequestValuesTextBox)
        Me._ReadingTabPage.Controls.Add(Me._SrqRefratoryTimeNumericLabel)
        Me._ReadingTabPage.Controls.Add(Me._SrqRefratoryTimeNumeric)
        Me._ReadingTabPage.Controls.Add(Me._SendComboBox)
        Me._ReadingTabPage.Controls.Add(Me._ReceiveTextBox)
        Me._ReadingTabPage.Controls.Add(Me._ReceiveButton)
        Me._ReadingTabPage.Controls.Add(Me._SendButton)
        Me._ReadingTabPage.Controls.Add(Me._StatusByteLabel)
        Me._ReadingTabPage.Controls.Add(Me._ReadStatusByteButton)
        Me._ReadingTabPage.Controls.Add(Me._ClearErrorQueueButton)
        Me._ReadingTabPage.Controls.Add(Me._ReadLastErrorButton)
        Me._ReadingTabPage.Controls.Add(Me._ReadButton)
        Me._ReadingTabPage.Controls.Add(Me._InitializeButton)
        Me._ReadingTabPage.Controls.Add(Me._ToolStripPanel)
        Me._ReadingTabPage.Controls.Add(Me._ResourceSelectorConnector)
        Me._ReadingTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadingTabPage.Name = "_ReadingTabPage"
        Me._ReadingTabPage.Size = New System.Drawing.Size(356, 270)
        Me._ReadingTabPage.TabIndex = 0
        Me._ReadingTabPage.Text = "Reading"
        Me._ReadingTabPage.UseVisualStyleBackColor = True
        '
        '_ToolStripPanel
        '
        Me._ToolStripPanel.Controls.Add(Me._SystemToolStrip)
        Me._ToolStripPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ToolStripPanel.Location = New System.Drawing.Point(0, 253)
        Me._ToolStripPanel.Name = "_ToolStripPanel"
        Me._ToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me._ToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me._ToolStripPanel.Size = New System.Drawing.Size(356, 28)
        '
        '_SrqRefratoryTimeNumericLabel
        '
        Me._SrqRefratoryTimeNumericLabel.AutoSize = True
        Me._SrqRefratoryTimeNumericLabel.Location = New System.Drawing.Point(227, 82)
        Me._SrqRefratoryTimeNumericLabel.Name = "_SrqRefratoryTimeNumericLabel"
        Me._SrqRefratoryTimeNumericLabel.Size = New System.Drawing.Size(72, 17)
        Me._SrqRefratoryTimeNumericLabel.TabIndex = 12
        Me._SrqRefratoryTimeNumericLabel.Text = "Delay [ms]:"
        '
        '_SrqRefratoryTimeNumeric
        '
        Me._SrqRefratoryTimeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SrqRefratoryTimeNumeric.Location = New System.Drawing.Point(302, 78)
        Me._SrqRefratoryTimeNumeric.Name = "_SrqRefratoryTimeNumeric"
        Me._SrqRefratoryTimeNumeric.NullValue = New Decimal(New Integer() {5, 0, 0, 0})
        Me._SrqRefratoryTimeNumeric.ReadOnlyBackColor = System.Drawing.SystemColors.Control
        Me._SrqRefratoryTimeNumeric.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText
        Me._SrqRefratoryTimeNumeric.ReadWriteBackColor = System.Drawing.SystemColors.Window
        Me._SrqRefratoryTimeNumeric.ReadWriteForeColor = System.Drawing.SystemColors.ControlText
        Me._SrqRefratoryTimeNumeric.Size = New System.Drawing.Size(45, 25)
        Me._SrqRefratoryTimeNumeric.TabIndex = 11
        Me._TipsToolTip.SetToolTip(Me._SrqRefratoryTimeNumeric, "Time it takes Status Register to reflect actual instrument status.")
        Me._SrqRefratoryTimeNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        '_SendComboBox
        '
        Me._SendComboBox.FormattingEnabled = True
        Me._SendComboBox.Items.AddRange(New Object() {"*CLS ", "*ESE 255", "*ESE?", "*ESR?", "*IDN? ", "*RST ", "*SRE 255", "*SRE?", "EROR?", "FLWR?", "FLRL?", "HEAD?", "LLIM?", "NEXT", "RAMP?", "SETD?", "TEMP?"})
        Me._SendComboBox.Location = New System.Drawing.Point(74, 147)
        Me._SendComboBox.Name = "_SendComboBox"
        Me._SendComboBox.Size = New System.Drawing.Size(273, 25)
        Me._SendComboBox.TabIndex = 10
        Me._SendComboBox.Text = "*IDN? "
        '
        '_ReceiveTextBox
        '
        Me._ReceiveTextBox.Location = New System.Drawing.Point(74, 180)
        Me._ReceiveTextBox.Multiline = True
        Me._ReceiveTextBox.Name = "_ReceiveTextBox"
        Me._ReceiveTextBox.ReadOnly = True
        Me._ReceiveTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._ReceiveTextBox.Size = New System.Drawing.Size(273, 50)
        Me._ReceiveTextBox.TabIndex = 9
        '
        '_ReceiveButton
        '
        Me._ReceiveButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ReceiveButton.Location = New System.Drawing.Point(12, 179)
        Me._ReceiveButton.Name = "_ReceiveButton"
        Me._ReceiveButton.Size = New System.Drawing.Size(56, 30)
        Me._ReceiveButton.TabIndex = 7
        Me._ReceiveButton.Text = "&Read:"
        Me._TipsToolTip.SetToolTip(Me._ReceiveButton, "Click to read from the instrument.")
        Me._ReceiveButton.UseVisualStyleBackColor = True
        '
        '_SendButton
        '
        Me._SendButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SendButton.Location = New System.Drawing.Point(12, 144)
        Me._SendButton.Name = "_SendButton"
        Me._SendButton.Size = New System.Drawing.Size(56, 30)
        Me._SendButton.TabIndex = 6
        Me._SendButton.Text = "&Send:"
        Me._TipsToolTip.SetToolTip(Me._SendButton, "Click to send")
        Me._SendButton.UseVisualStyleBackColor = True
        '
        '_StatusByteLabel
        '
        Me._StatusByteLabel.AutoSize = True
        Me._StatusByteLabel.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusByteLabel.Location = New System.Drawing.Point(150, 83)
        Me._StatusByteLabel.Name = "_StatusByteLabel"
        Me._StatusByteLabel.Size = New System.Drawing.Size(63, 15)
        Me._StatusByteLabel.TabIndex = 4
        Me._StatusByteLabel.Text = "00000000"
        '
        '_ReadStatusByteButton
        '
        Me._ReadStatusByteButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ReadStatusByteButton.Location = New System.Drawing.Point(12, 75)
        Me._ReadStatusByteButton.Name = "_ReadStatusByteButton"
        Me._ReadStatusByteButton.Size = New System.Drawing.Size(135, 30)
        Me._ReadStatusByteButton.TabIndex = 3
        Me._ReadStatusByteButton.Text = "Read Status Byte:"
        Me._TipsToolTip.SetToolTip(Me._ReadStatusByteButton, "Reads the status byte. ")
        Me._ReadStatusByteButton.UseVisualStyleBackColor = True
        '
        '_ClearErrorQueueButton
        '
        Me._ClearErrorQueueButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ClearErrorQueueButton.Location = New System.Drawing.Point(180, 110)
        Me._ClearErrorQueueButton.Name = "_ClearErrorQueueButton"
        Me._ClearErrorQueueButton.Size = New System.Drawing.Size(147, 30)
        Me._ClearErrorQueueButton.TabIndex = 2
        Me._ClearErrorQueueButton.Text = "Clear Error Queue"
        Me._ClearErrorQueueButton.UseVisualStyleBackColor = True
        '
        '_ReadLastErrorButton
        '
        Me._ReadLastErrorButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ReadLastErrorButton.Location = New System.Drawing.Point(12, 110)
        Me._ReadLastErrorButton.Name = "_ReadLastErrorButton"
        Me._ReadLastErrorButton.Size = New System.Drawing.Size(162, 30)
        Me._ReadLastErrorButton.TabIndex = 2
        Me._ReadLastErrorButton.Text = "Read Last Device Error"
        Me._ReadLastErrorButton.UseVisualStyleBackColor = True
        '
        '_ReadButton
        '
        Me._ReadButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ReadButton.Location = New System.Drawing.Point(12, 6)
        Me._ReadButton.Name = "_ReadButton"
        Me._ReadButton.Size = New System.Drawing.Size(104, 30)
        Me._ReadButton.TabIndex = 0
        Me._ReadButton.Text = "&Read Temp"
        Me._ReadButton.UseVisualStyleBackColor = True
        '
        '_InitializeButton
        '
        Me._InitializeButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._InitializeButton.Location = New System.Drawing.Point(12, 40)
        Me._InitializeButton.Name = "_InitializeButton"
        Me._InitializeButton.Size = New System.Drawing.Size(104, 30)
        Me._InitializeButton.TabIndex = 1
        Me._InitializeButton.Text = "&Initialize"
        Me._TipsToolTip.SetToolTip(Me._InitializeButton, "+Clears execution state and sets register masks")
        Me._InitializeButton.UseVisualStyleBackColor = True
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
        '_FunctionTabPage
        '
        Me._FunctionTabPage.Controls.Add(Me._QueryStatusButton)
        Me._FunctionTabPage.Controls.Add(Me._OperatorCycleTabs)
        Me._FunctionTabPage.Controls.Add(Me._SetpointNumberNumeric)
        Me._FunctionTabPage.Controls.Add(Me._TestTimeElapsedCheckBox)
        Me._FunctionTabPage.Controls.Add(Me._CycleCountNumericLabel)
        Me._FunctionTabPage.Controls.Add(Me._CycleCountNumeric)
        Me._FunctionTabPage.Controls.Add(Me._MaxTestTimeNumeric)
        Me._FunctionTabPage.Controls.Add(Me._MaxTestTimeNumericLabel)
        Me._FunctionTabPage.Controls.Add(Me._ReadSetpointButton)
        Me._FunctionTabPage.Controls.Add(Me._ReadyCheckBox)
        Me._FunctionTabPage.Controls.Add(Me._NotAtTempCheckBox)
        Me._FunctionTabPage.Controls.Add(Me._AtTempCheckBox)
        Me._FunctionTabPage.Controls.Add(Me._HeadDownCheckBox)
        Me._FunctionTabPage.Controls.Add(Me._SetpointWindowNumericLabel)
        Me._FunctionTabPage.Controls.Add(Me._SetpointWindowNumeric)
        Me._FunctionTabPage.Controls.Add(Me._SetpointNumberNumericLabel)
        Me._FunctionTabPage.Controls.Add(Me._RampRateNumeric)
        Me._FunctionTabPage.Controls.Add(Me._SoakTimeNumeric)
        Me._FunctionTabPage.Controls.Add(Me._SetpointNumeric)
        Me._FunctionTabPage.Controls.Add(Me._RampRateNumericLabel)
        Me._FunctionTabPage.Controls.Add(Me._SoakTimeNumericLabel)
        Me._FunctionTabPage.Controls.Add(Me._SetpointNumericLabel)
        Me._FunctionTabPage.Location = New System.Drawing.Point(4, 26)
        Me._FunctionTabPage.Name = "_FunctionTabPage"
        Me._FunctionTabPage.Size = New System.Drawing.Size(356, 270)
        Me._FunctionTabPage.TabIndex = 4
        Me._FunctionTabPage.Text = "Function"
        Me._FunctionTabPage.UseVisualStyleBackColor = True
        '
        '_QueryStatusButton
        '
        Me._QueryStatusButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._QueryStatusButton.Location = New System.Drawing.Point(243, 3)
        Me._QueryStatusButton.Name = "_QueryStatusButton"
        Me._QueryStatusButton.Size = New System.Drawing.Size(107, 28)
        Me._QueryStatusButton.TabIndex = 1
        Me._QueryStatusButton.Text = "Refresh Status"
        Me._TipsToolTip.SetToolTip(Me._QueryStatusButton, "Check auxiliary status")
        Me._QueryStatusButton.UseVisualStyleBackColor = True
        '
        '_OperatorCycleTabs
        '
        Me._OperatorCycleTabs.Controls.Add(Me._CycleTabPage1)
        Me._OperatorCycleTabs.Controls.Add(Me._OperatorTabPage)
        Me._OperatorCycleTabs.Location = New System.Drawing.Point(213, 58)
        Me._OperatorCycleTabs.Name = "_OperatorCycleTabs"
        Me._OperatorCycleTabs.SelectedIndex = 0
        Me._OperatorCycleTabs.Size = New System.Drawing.Size(138, 208)
        Me._OperatorCycleTabs.TabIndex = 22
        '
        '_CycleTabPage1
        '
        Me._CycleTabPage1.Controls.Add(Me._CycleToolStrip)
        Me._CycleTabPage1.Location = New System.Drawing.Point(4, 26)
        Me._CycleTabPage1.Name = "_CycleTabPage1"
        Me._CycleTabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me._CycleTabPage1.Size = New System.Drawing.Size(130, 178)
        Me._CycleTabPage1.TabIndex = 0
        Me._CycleTabPage1.Text = "Cycle"
        Me._CycleTabPage1.UseVisualStyleBackColor = True
        '
        '_CycleToolStrip
        '
        Me._CycleToolStrip.Dock = System.Windows.Forms.DockStyle.Left
        Me._CycleToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me._CycleToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetCycleModeButton, Me._StartCycleButton, Me._NextSetpointButton, Me._StopCycleButton, Me._CyclesCompletedCheckBox1, Me._CycleCompletedCheckBox1, Me._CyclingStoppedCheckBox, Me._FindLastSetpointButton1})
        Me._CycleToolStrip.Location = New System.Drawing.Point(3, 3)
        Me._CycleToolStrip.Name = "_CycleToolStrip"
        Me._CycleToolStrip.Size = New System.Drawing.Size(123, 172)
        Me._CycleToolStrip.TabIndex = 22
        Me._CycleToolStrip.Text = "ToolStrip1"
        '
        '_ResetCycleModeButton
        '
        Me._ResetCycleModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ResetCycleModeButton.Image = CType(resources.GetObject("_ResetCycleModeButton.Image"), System.Drawing.Image)
        Me._ResetCycleModeButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ResetCycleModeButton.Margin = New System.Windows.Forms.Padding(0)
        Me._ResetCycleModeButton.Name = "_ResetCycleModeButton"
        Me._ResetCycleModeButton.Size = New System.Drawing.Size(120, 19)
        Me._ResetCycleModeButton.Text = "Cycle Mode: On/Off"
        '
        '_StartCycleButton
        '
        Me._StartCycleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._StartCycleButton.Image = CType(resources.GetObject("_StartCycleButton.Image"), System.Drawing.Image)
        Me._StartCycleButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._StartCycleButton.Margin = New System.Windows.Forms.Padding(0)
        Me._StartCycleButton.Name = "_StartCycleButton"
        Me._StartCycleButton.Size = New System.Drawing.Size(120, 19)
        Me._StartCycleButton.Text = "Start"
        '
        '_NextSetpointButton
        '
        Me._NextSetpointButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._NextSetpointButton.Image = CType(resources.GetObject("_NextSetpointButton.Image"), System.Drawing.Image)
        Me._NextSetpointButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._NextSetpointButton.Margin = New System.Windows.Forms.Padding(0)
        Me._NextSetpointButton.Name = "_NextSetpointButton"
        Me._NextSetpointButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._NextSetpointButton.Size = New System.Drawing.Size(120, 19)
        Me._NextSetpointButton.Text = "Next"
        '
        '_StopCycleButton
        '
        Me._StopCycleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._StopCycleButton.Image = CType(resources.GetObject("_StopCycleButton.Image"), System.Drawing.Image)
        Me._StopCycleButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._StopCycleButton.Margin = New System.Windows.Forms.Padding(0)
        Me._StopCycleButton.Name = "_StopCycleButton"
        Me._StopCycleButton.Size = New System.Drawing.Size(120, 19)
        Me._StopCycleButton.Text = "Stop"
        Me._StopCycleButton.ToolTipText = "Stop cycle"
        '
        '_CyclesCompletedCheckBox1
        '
        Me._CyclesCompletedCheckBox1.Checked = False
        Me._CyclesCompletedCheckBox1.Margin = New System.Windows.Forms.Padding(0)
        Me._CyclesCompletedCheckBox1.Name = "_CyclesCompletedCheckBox1"
        Me._CyclesCompletedCheckBox1.Size = New System.Drawing.Size(120, 19)
        Me._CyclesCompletedCheckBox1.Text = "Cycles Completed"
        '
        '_CycleCompletedCheckBox1
        '
        Me._CycleCompletedCheckBox1.Checked = False
        Me._CycleCompletedCheckBox1.Margin = New System.Windows.Forms.Padding(0)
        Me._CycleCompletedCheckBox1.Name = "_CycleCompletedCheckBox1"
        Me._CycleCompletedCheckBox1.Size = New System.Drawing.Size(120, 19)
        Me._CycleCompletedCheckBox1.Text = "Cycle Complete"
        '
        '_CyclingStoppedCheckBox
        '
        Me._CyclingStoppedCheckBox.Checked = False
        Me._CyclingStoppedCheckBox.Margin = New System.Windows.Forms.Padding(0)
        Me._CyclingStoppedCheckBox.Name = "_CyclingStoppedCheckBox"
        Me._CyclingStoppedCheckBox.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._CyclingStoppedCheckBox.Size = New System.Drawing.Size(120, 19)
        Me._CyclingStoppedCheckBox.Text = "Cycling Stopped"
        '
        '_FindLastSetpointButton1
        '
        Me._FindLastSetpointButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._FindLastSetpointButton1.Image = CType(resources.GetObject("_FindLastSetpointButton1.Image"), System.Drawing.Image)
        Me._FindLastSetpointButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._FindLastSetpointButton1.Margin = New System.Windows.Forms.Padding(0)
        Me._FindLastSetpointButton1.Name = "_FindLastSetpointButton1"
        Me._FindLastSetpointButton1.Size = New System.Drawing.Size(120, 19)
        Me._FindLastSetpointButton1.Text = "Find Last Setpoint: -1"
        '
        '_OperatorTabPage
        '
        Me._OperatorTabPage.Controls.Add(Me._ResetDelayNumericLabel)
        Me._OperatorTabPage.Controls.Add(Me._ResetQueryDelayNumeric)
        Me._OperatorTabPage.Controls.Add(Me._ResetOperatorModeButton)
        Me._OperatorTabPage.Location = New System.Drawing.Point(4, 22)
        Me._OperatorTabPage.Name = "_OperatorTabPage"
        Me._OperatorTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me._OperatorTabPage.Size = New System.Drawing.Size(130, 182)
        Me._OperatorTabPage.TabIndex = 1
        Me._OperatorTabPage.Text = "Operator"
        Me._OperatorTabPage.UseVisualStyleBackColor = True
        '
        '_ResetDelayNumericLabel
        '
        Me._ResetDelayNumericLabel.AutoSize = True
        Me._ResetDelayNumericLabel.Location = New System.Drawing.Point(5, 67)
        Me._ResetDelayNumericLabel.Name = "_ResetDelayNumericLabel"
        Me._ResetDelayNumericLabel.Size = New System.Drawing.Size(72, 17)
        Me._ResetDelayNumericLabel.TabIndex = 14
        Me._ResetDelayNumericLabel.Text = "Delay [ms]:"
        '
        '_ResetQueryDelayNumeric
        '
        Me._ResetQueryDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ResetQueryDelayNumeric.Location = New System.Drawing.Point(80, 63)
        Me._ResetQueryDelayNumeric.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me._ResetQueryDelayNumeric.Name = "_ResetQueryDelayNumeric"
        Me._ResetQueryDelayNumeric.NullValue = New Decimal(New Integer() {5, 0, 0, 0})
        Me._ResetQueryDelayNumeric.ReadOnlyBackColor = System.Drawing.SystemColors.Control
        Me._ResetQueryDelayNumeric.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText
        Me._ResetQueryDelayNumeric.ReadWriteBackColor = System.Drawing.SystemColors.Window
        Me._ResetQueryDelayNumeric.ReadWriteForeColor = System.Drawing.SystemColors.ControlText
        Me._ResetQueryDelayNumeric.Size = New System.Drawing.Size(45, 25)
        Me._ResetQueryDelayNumeric.TabIndex = 13
        Me._TipsToolTip.SetToolTip(Me._ResetQueryDelayNumeric, "Time it takes reset to complete")
        Me._ResetQueryDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        '_ResetOperatorModeButton
        '
        Me._ResetOperatorModeButton.Location = New System.Drawing.Point(8, 6)
        Me._ResetOperatorModeButton.Name = "_ResetOperatorModeButton"
        Me._ResetOperatorModeButton.Size = New System.Drawing.Size(114, 49)
        Me._ResetOperatorModeButton.TabIndex = 0
        Me._ResetOperatorModeButton.Text = "Operator Mode: On/Off"
        Me._ResetOperatorModeButton.UseVisualStyleBackColor = True
        '
        '_SetpointNumberNumeric
        '
        Me._SetpointNumberNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._SetpointNumberNumeric.CanSelectEmptyValue = True
        Me._SetpointNumberNumeric.DirtyBackColor = System.Drawing.Color.Orange
        Me._SetpointNumberNumeric.DirtyForeColor = System.Drawing.SystemColors.ActiveCaption
        Me._SetpointNumberNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SetpointNumberNumeric.Location = New System.Drawing.Point(123, 86)
        Me._SetpointNumberNumeric.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SetpointNumberNumeric.Minimum = New Decimal(New Integer() {0, 0, 0, 0})
        Me._SetpointNumberNumeric.Name = "_SetpointNumberNumeric"
        Me._SetpointNumberNumeric.SelectorIcon = CType(resources.GetObject("_SetpointNumberNumeric.SelectorIcon"), System.Drawing.Image)
        Me._SetpointNumberNumeric.Size = New System.Drawing.Size(84, 25)
        Me._SetpointNumberNumeric.TabIndex = 6
        Me._TipsToolTip.SetToolTip(Me._SetpointNumberNumeric, "Select Setpoint")
        Me._SetpointNumberNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        Me._SetpointNumberNumeric.Watermark = Nothing
        '
        '_TestTimeElapsedCheckBox
        '
        Me._TestTimeElapsedCheckBox.AutoSize = True
        Me._TestTimeElapsedCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._TestTimeElapsedCheckBox.Location = New System.Drawing.Point(31, 46)
        Me._TestTimeElapsedCheckBox.Name = "_TestTimeElapsedCheckBox"
        Me._TestTimeElapsedCheckBox.ReadOnly = True
        Me._TestTimeElapsedCheckBox.Size = New System.Drawing.Size(104, 21)
        Me._TestTimeElapsedCheckBox.TabIndex = 4
        Me._TestTimeElapsedCheckBox.Text = "Test Timeout:"
        Me._TestTimeElapsedCheckBox.ThreeState = True
        Me._TipsToolTip.SetToolTip(Me._TestTimeElapsedCheckBox, "Test time elapsed")
        Me._TestTimeElapsedCheckBox.UseVisualStyleBackColor = True
        '
        '_CycleCountNumericLabel
        '
        Me._CycleCountNumericLabel.AutoSize = True
        Me._CycleCountNumericLabel.Location = New System.Drawing.Point(41, 246)
        Me._CycleCountNumericLabel.Name = "_CycleCountNumericLabel"
        Me._CycleCountNumericLabel.Size = New System.Drawing.Size(79, 17)
        Me._CycleCountNumericLabel.TabIndex = 20
        Me._CycleCountNumericLabel.Text = "Cycle Count:"
        '
        '_CycleCountNumeric
        '
        Me._CycleCountNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._CycleCountNumeric.CanSelectEmptyValue = True
        Me._CycleCountNumeric.DirtyBackColor = System.Drawing.Color.Orange
        Me._CycleCountNumeric.DirtyForeColor = System.Drawing.SystemColors.ActiveCaption
        Me._CycleCountNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._CycleCountNumeric.Location = New System.Drawing.Point(123, 242)
        Me._CycleCountNumeric.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._CycleCountNumeric.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me._CycleCountNumeric.Minimum = New Decimal(New Integer() {0, 0, 0, 0})
        Me._CycleCountNumeric.Name = "_CycleCountNumeric"
        Me._CycleCountNumeric.SelectorIcon = CType(resources.GetObject("_CycleCountNumeric.SelectorIcon"), System.Drawing.Image)
        Me._CycleCountNumeric.Size = New System.Drawing.Size(84, 25)
        Me._CycleCountNumeric.TabIndex = 19
        Me._TipsToolTip.SetToolTip(Me._CycleCountNumeric, "Cycle count")
        Me._CycleCountNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        Me._CycleCountNumeric.Watermark = Nothing
        '
        '_MaxTestTimeNumeric
        '
        Me._MaxTestTimeNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._MaxTestTimeNumeric.CanSelectEmptyValue = True
        Me._MaxTestTimeNumeric.DirtyBackColor = System.Drawing.Color.Orange
        Me._MaxTestTimeNumeric.DirtyForeColor = System.Drawing.SystemColors.ActiveCaption
        Me._MaxTestTimeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._MaxTestTimeNumeric.Location = New System.Drawing.Point(123, 216)
        Me._MaxTestTimeNumeric.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._MaxTestTimeNumeric.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me._MaxTestTimeNumeric.Minimum = New Decimal(New Integer() {0, 0, 0, 0})
        Me._MaxTestTimeNumeric.Name = "_MaxTestTimeNumeric"
        Me._MaxTestTimeNumeric.SelectorIcon = CType(resources.GetObject("_MaxTestTimeNumeric.SelectorIcon"), System.Drawing.Image)
        Me._MaxTestTimeNumeric.Size = New System.Drawing.Size(84, 25)
        Me._MaxTestTimeNumeric.TabIndex = 18
        Me._TipsToolTip.SetToolTip(Me._MaxTestTimeNumeric, "Test time")
        Me._MaxTestTimeNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        Me._MaxTestTimeNumeric.Watermark = Nothing
        '
        '_MaxTestTimeNumericLabel
        '
        Me._MaxTestTimeNumericLabel.AutoSize = True
        Me._MaxTestTimeNumericLabel.Location = New System.Drawing.Point(35, 221)
        Me._MaxTestTimeNumericLabel.Name = "_MaxTestTimeNumericLabel"
        Me._MaxTestTimeNumericLabel.Size = New System.Drawing.Size(84, 17)
        Me._MaxTestTimeNumericLabel.TabIndex = 17
        Me._MaxTestTimeNumericLabel.Text = "Test Time [s]:"
        Me._TipsToolTip.SetToolTip(Me._MaxTestTimeNumericLabel, "Maximum test time")
        '
        '_ReadSetpointButton
        '
        Me._ReadSetpointButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ReadSetpointButton.Location = New System.Drawing.Point(243, 31)
        Me._ReadSetpointButton.Name = "_ReadSetpointButton"
        Me._ReadSetpointButton.Size = New System.Drawing.Size(109, 25)
        Me._ReadSetpointButton.TabIndex = 14
        Me._ReadSetpointButton.Text = "Read Setpoint"
        Me._ReadSetpointButton.UseVisualStyleBackColor = True
        '
        '_ReadyCheckBox
        '
        Me._ReadyCheckBox.AutoSize = True
        Me._ReadyCheckBox.Location = New System.Drawing.Point(149, 8)
        Me._ReadyCheckBox.Name = "_ReadyCheckBox"
        Me._ReadyCheckBox.ReadOnly = True
        Me._ReadyCheckBox.Size = New System.Drawing.Size(66, 21)
        Me._ReadyCheckBox.TabIndex = 4
        Me._ReadyCheckBox.Text = ":Ready"
        Me._ReadyCheckBox.ThreeState = True
        Me._ReadyCheckBox.UseVisualStyleBackColor = True
        '
        '_NotAtTempCheckBox
        '
        Me._NotAtTempCheckBox.AutoSize = True
        Me._NotAtTempCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._NotAtTempCheckBox.Location = New System.Drawing.Point(31, 27)
        Me._NotAtTempCheckBox.Name = "_NotAtTempCheckBox"
        Me._NotAtTempCheckBox.ReadOnly = True
        Me._NotAtTempCheckBox.Size = New System.Drawing.Size(104, 21)
        Me._NotAtTempCheckBox.TabIndex = 4
        Me._NotAtTempCheckBox.Text = "Not At Temp:"
        Me._NotAtTempCheckBox.ThreeState = True
        Me._NotAtTempCheckBox.UseVisualStyleBackColor = True
        '
        '_AtTempCheckBox
        '
        Me._AtTempCheckBox.AutoSize = True
        Me._AtTempCheckBox.Location = New System.Drawing.Point(149, 27)
        Me._AtTempCheckBox.Name = "_AtTempCheckBox"
        Me._AtTempCheckBox.ReadOnly = True
        Me._AtTempCheckBox.Size = New System.Drawing.Size(78, 21)
        Me._AtTempCheckBox.TabIndex = 2
        Me._AtTempCheckBox.Text = ":At Temp"
        Me._AtTempCheckBox.ThreeState = True
        Me._AtTempCheckBox.UseVisualStyleBackColor = True
        '
        '_HeadDownCheckBox
        '
        Me._HeadDownCheckBox.AutoSize = True
        Me._HeadDownCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._HeadDownCheckBox.Location = New System.Drawing.Point(38, 8)
        Me._HeadDownCheckBox.Name = "_HeadDownCheckBox"
        Me._HeadDownCheckBox.ReadOnly = True
        Me._HeadDownCheckBox.Size = New System.Drawing.Size(98, 21)
        Me._HeadDownCheckBox.TabIndex = 0
        Me._HeadDownCheckBox.Text = "Head Down:"
        Me._HeadDownCheckBox.ThreeState = True
        Me._HeadDownCheckBox.UseVisualStyleBackColor = True
        '
        '_SetpointWindowNumericLabel
        '
        Me._SetpointWindowNumericLabel.AutoSize = True
        Me._SetpointWindowNumericLabel.Location = New System.Drawing.Point(37, 142)
        Me._SetpointWindowNumericLabel.Name = "_SetpointWindowNumericLabel"
        Me._SetpointWindowNumericLabel.Size = New System.Drawing.Size(83, 17)
        Me._SetpointWindowNumericLabel.TabIndex = 10
        Me._SetpointWindowNumericLabel.Text = "Window [°C]:"
        '
        '_SetpointWindowNumeric
        '
        Me._SetpointWindowNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._SetpointWindowNumeric.DecimalPlaces = 1
        Me._SetpointWindowNumeric.DirtyBackColor = System.Drawing.Color.Orange
        Me._SetpointWindowNumeric.DirtyForeColor = System.Drawing.SystemColors.ActiveCaption
        Me._SetpointWindowNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SetpointWindowNumeric.Location = New System.Drawing.Point(123, 138)
        Me._SetpointWindowNumeric.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SetpointWindowNumeric.Maximum = New Decimal(New Integer() {99, 0, 0, 65536})
        Me._SetpointWindowNumeric.Minimum = New Decimal(New Integer() {0, 0, 0, 0})
        Me._SetpointWindowNumeric.Name = "_SetpointWindowNumeric"
        Me._SetpointWindowNumeric.SelectorIcon = CType(resources.GetObject("_SetpointWindowNumeric.SelectorIcon"), System.Drawing.Image)
        Me._SetpointWindowNumeric.Size = New System.Drawing.Size(84, 25)
        Me._SetpointWindowNumeric.TabIndex = 11
        Me._TipsToolTip.SetToolTip(Me._SetpointWindowNumeric, "Setpoint Window")
        Me._SetpointWindowNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        Me._SetpointWindowNumeric.Watermark = Nothing
        '
        '_SetpointNumberNumericLabel
        '
        Me._SetpointNumberNumericLabel.AutoSize = True
        Me._SetpointNumberNumericLabel.Location = New System.Drawing.Point(12, 88)
        Me._SetpointNumberNumericLabel.Name = "_SetpointNumberNumericLabel"
        Me._SetpointNumberNumericLabel.Size = New System.Drawing.Size(108, 17)
        Me._SetpointNumberNumericLabel.TabIndex = 5
        Me._SetpointNumberNumericLabel.Text = "Setpoint number:"
        '
        '_RampRateNumeric
        '
        Me._RampRateNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._RampRateNumeric.CanSelectEmptyValue = True
        Me._RampRateNumeric.DirtyBackColor = System.Drawing.Color.Orange
        Me._RampRateNumeric.DirtyForeColor = System.Drawing.SystemColors.ActiveCaption
        Me._RampRateNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._RampRateNumeric.Location = New System.Drawing.Point(123, 190)
        Me._RampRateNumeric.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._RampRateNumeric.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me._RampRateNumeric.Minimum = New Decimal(New Integer() {0, 0, 0, 0})
        Me._RampRateNumeric.Name = "_RampRateNumeric"
        Me._RampRateNumeric.SelectorIcon = CType(resources.GetObject("_RampRateNumeric.SelectorIcon"), System.Drawing.Image)
        Me._RampRateNumeric.Size = New System.Drawing.Size(84, 25)
        Me._RampRateNumeric.TabIndex = 13
        Me._TipsToolTip.SetToolTip(Me._RampRateNumeric, "Ramp rate in degrees per minute.")
        Me._RampRateNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        Me._RampRateNumeric.Watermark = Nothing
        '
        '_SoakTimeNumeric
        '
        Me._SoakTimeNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._SoakTimeNumeric.DirtyBackColor = System.Drawing.Color.Orange
        Me._SoakTimeNumeric.DirtyForeColor = System.Drawing.SystemColors.ActiveCaption
        Me._SoakTimeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SoakTimeNumeric.Location = New System.Drawing.Point(123, 164)
        Me._SoakTimeNumeric.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SoakTimeNumeric.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me._SoakTimeNumeric.Minimum = New Decimal(New Integer() {0, 0, 0, 0})
        Me._SoakTimeNumeric.Name = "_SoakTimeNumeric"
        Me._SoakTimeNumeric.SelectorIcon = CType(resources.GetObject("_SoakTimeNumeric.SelectorIcon"), System.Drawing.Image)
        Me._SoakTimeNumeric.Size = New System.Drawing.Size(84, 25)
        Me._SoakTimeNumeric.TabIndex = 13
        Me._TipsToolTip.SetToolTip(Me._SoakTimeNumeric, "Soak time")
        Me._SoakTimeNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        Me._SoakTimeNumeric.Watermark = Nothing
        '
        '_SetpointNumeric
        '
        Me._SetpointNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._SetpointNumeric.DecimalPlaces = 1
        Me._SetpointNumeric.DirtyBackColor = System.Drawing.Color.Orange
        Me._SetpointNumeric.DirtyForeColor = System.Drawing.SystemColors.ActiveCaption
        Me._SetpointNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SetpointNumeric.Location = New System.Drawing.Point(123, 112)
        Me._SetpointNumeric.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SetpointNumeric.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me._SetpointNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me._SetpointNumeric.Name = "_SetpointNumeric"
        Me._SetpointNumeric.SelectorIcon = CType(resources.GetObject("_SetpointNumeric.SelectorIcon"), System.Drawing.Image)
        Me._SetpointNumeric.Size = New System.Drawing.Size(84, 25)
        Me._SetpointNumeric.TabIndex = 9
        Me._SetpointNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        Me._SetpointNumeric.Watermark = Nothing
        '
        '_RampRateNumericLabel
        '
        Me._RampRateNumericLabel.AutoSize = True
        Me._RampRateNumericLabel.Location = New System.Drawing.Point(32, 194)
        Me._RampRateNumericLabel.Name = "_RampRateNumericLabel"
        Me._RampRateNumericLabel.Size = New System.Drawing.Size(88, 17)
        Me._RampRateNumericLabel.TabIndex = 12
        Me._RampRateNumericLabel.Text = "Ramp [°/min]:"
        Me._RampRateNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SoakTimeNumericLabel
        '
        Me._SoakTimeNumericLabel.AutoSize = True
        Me._SoakTimeNumericLabel.Location = New System.Drawing.Point(31, 168)
        Me._SoakTimeNumericLabel.Name = "_SoakTimeNumericLabel"
        Me._SoakTimeNumericLabel.Size = New System.Drawing.Size(89, 17)
        Me._SoakTimeNumericLabel.TabIndex = 12
        Me._SoakTimeNumericLabel.Text = "Soak Time [s]:"
        Me._SoakTimeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SetpointNumericLabel
        '
        Me._SetpointNumericLabel.AutoSize = True
        Me._SetpointNumericLabel.Location = New System.Drawing.Point(36, 116)
        Me._SetpointNumericLabel.Name = "_SetpointNumericLabel"
        Me._SetpointNumericLabel.Size = New System.Drawing.Size(84, 17)
        Me._SetpointNumericLabel.TabIndex = 8
        Me._SetpointNumericLabel.Text = "Setpoint [°C]:"
        Me._SetpointNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SensorTabPage
        '
        Me._SensorTabPage.Controls.Add(Me._DeviceSensorTypeSelectorLabel)
        Me._SensorTabPage.Controls.Add(Me._DeviceThermalConstantSelectorLabel)
        Me._SensorTabPage.Controls.Add(Me._DeviceSensorTypeSelector)
        Me._SensorTabPage.Controls.Add(Me._DeviceThermalConstantSelector)
        Me._SensorTabPage.Location = New System.Drawing.Point(4, 26)
        Me._SensorTabPage.Name = "_SensorTabPage"
        Me._SensorTabPage.Size = New System.Drawing.Size(356, 270)
        Me._SensorTabPage.TabIndex = 5
        Me._SensorTabPage.Text = "Sensor"
        Me._SensorTabPage.UseVisualStyleBackColor = True
        '
        '_DeviceSensorTypeSelectorLabel
        '
        Me._DeviceSensorTypeSelectorLabel.AutoSize = True
        Me._DeviceSensorTypeSelectorLabel.Location = New System.Drawing.Point(51, 57)
        Me._DeviceSensorTypeSelectorLabel.Name = "_DeviceSensorTypeSelectorLabel"
        Me._DeviceSensorTypeSelectorLabel.Size = New System.Drawing.Size(82, 17)
        Me._DeviceSensorTypeSelectorLabel.TabIndex = 3
        Me._DeviceSensorTypeSelectorLabel.Text = "Sensor Type:"
        '
        '_DeviceThermalConstantSelectorLabel
        '
        Me._DeviceThermalConstantSelectorLabel.AutoSize = True
        Me._DeviceThermalConstantSelectorLabel.Location = New System.Drawing.Point(21, 24)
        Me._DeviceThermalConstantSelectorLabel.Name = "_DeviceThermalConstantSelectorLabel"
        Me._DeviceThermalConstantSelectorLabel.Size = New System.Drawing.Size(113, 17)
        Me._DeviceThermalConstantSelectorLabel.TabIndex = 2
        Me._DeviceThermalConstantSelectorLabel.Text = "Thermal Constant:"
        '
        '_DeviceSensorTypeSelector
        '
        Me._DeviceSensorTypeSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._DeviceSensorTypeSelector.DirtyBackColor = System.Drawing.Color.Orange
        Me._DeviceSensorTypeSelector.DirtyForeColor = System.Drawing.SystemColors.ActiveCaption
        Me._DeviceSensorTypeSelector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._DeviceSensorTypeSelector.Location = New System.Drawing.Point(136, 53)
        Me._DeviceSensorTypeSelector.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._DeviceSensorTypeSelector.Name = "_DeviceSensorTypeSelector"
        Me._DeviceSensorTypeSelector.SelectorIcon = CType(resources.GetObject("_DeviceSensorTypeSelector.SelectorIcon"), System.Drawing.Image)
        Me._DeviceSensorTypeSelector.Size = New System.Drawing.Size(182, 25)
        Me._DeviceSensorTypeSelector.TabIndex = 1
        Me._DeviceSensorTypeSelector.Watermark = Nothing
        '
        '_DeviceThermalConstantSelector
        '
        Me._DeviceThermalConstantSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._DeviceThermalConstantSelector.DirtyBackColor = System.Drawing.Color.Orange
        Me._DeviceThermalConstantSelector.DirtyForeColor = System.Drawing.SystemColors.ActiveCaption
        Me._DeviceThermalConstantSelector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._DeviceThermalConstantSelector.Location = New System.Drawing.Point(136, 21)
        Me._DeviceThermalConstantSelector.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._DeviceThermalConstantSelector.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me._DeviceThermalConstantSelector.Minimum = New Decimal(New Integer() {20, 0, 0, 0})
        Me._DeviceThermalConstantSelector.Name = "_DeviceThermalConstantSelector"
        Me._DeviceThermalConstantSelector.SelectorIcon = CType(resources.GetObject("_DeviceThermalConstantSelector.SelectorIcon"), System.Drawing.Image)
        Me._DeviceThermalConstantSelector.Size = New System.Drawing.Size(74, 25)
        Me._DeviceThermalConstantSelector.TabIndex = 0
        Me._DeviceThermalConstantSelector.Value = New Decimal(New Integer() {20, 0, 0, 0})
        Me._DeviceThermalConstantSelector.Watermark = Nothing
        '
        '_ReadWriteTabPage
        '
        Me._ReadWriteTabPage.Controls.Add(Me._SimpleReadWriteControl)
        Me._ReadWriteTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadWriteTabPage.Name = "_ReadWriteTabPage"
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(356, 270)
        Me._ReadWriteTabPage.TabIndex = 6
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
        Me._MessagesTabPage.Controls.Add(Me._TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(356, 270)
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
        Me._TraceMessagesBox.Size = New System.Drawing.Size(356, 310)
        Me._TraceMessagesBox.TabIndex = 1
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
        Me._ReadingStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ComplianceToolStripStatusLabel, Me._ReadingToolStripStatusLabel, Me._StatusRegisterLabel, Me._StandardRegisterLabel})
        Me._ReadingStatusStrip.Location = New System.Drawing.Point(0, 16)
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
        Me._ReadingToolStripStatusLabel.Text = "0.0 °C"
        Me._ReadingToolStripStatusLabel.ToolTipText = "Reading"
        '
        '_StatusRegisterLabel
        '
        Me._StatusRegisterLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._StatusRegisterLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusRegisterLabel.ForeColor = System.Drawing.Color.LightSkyBlue
        Me._StatusRegisterLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusRegisterLabel.Name = "_StatusRegisterLabel"
        Me._StatusRegisterLabel.Size = New System.Drawing.Size(36, 37)
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
        Me._StandardRegisterLabel.Size = New System.Drawing.Size(36, 37)
        Me._StandardRegisterLabel.Text = "0x00"
        Me._StandardRegisterLabel.ToolTipText = "Standard Register Value"
        '
        '_LastReadingTextBox
        '
        Me._LastReadingTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._LastReadingTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._LastReadingTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._LastReadingTextBox.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._LastReadingTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._LastReadingTextBox.Location = New System.Drawing.Point(0, 0)
        Me._LastReadingTextBox.Name = "_LastReadingTextBox"
        Me._LastReadingTextBox.Size = New System.Drawing.Size(364, 16)
        Me._LastReadingTextBox.TabIndex = 0
        Me._LastReadingTextBox.Text = "<last reading>"
        Me._LastReadingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
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
        '_InfoProvider
        '
        Me._InfoProvider.ContainerControl = Me
        '
        '_Panel
        '
        Me._Panel.Controls.Add(Me._Tabs)
        Me._Panel.Controls.Add(Me._LastErrorTextBox)
        Me._Panel.Controls.Add(Me._ReadingStatusStrip)
        Me._Panel.Controls.Add(Me._LastReadingTextBox)
        Me._Panel.Controls.Add(Me._StatusStrip)
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
        Me._TitleLabel.Text = "Thermostream"
        Me._TitleLabel.UseMnemonic = False
        '
        '_ServiceRequestValuesTextBox
        '
        Me._ServiceRequestValuesTextBox.Location = New System.Drawing.Point(131, 6)
        Me._ServiceRequestValuesTextBox.Multiline = True
        Me._ServiceRequestValuesTextBox.Name = "_ServiceRequestValuesTextBox"
        Me._ServiceRequestValuesTextBox.ReadOnly = True
        Me._ServiceRequestValuesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me._ServiceRequestValuesTextBox.Size = New System.Drawing.Size(216, 62)
        Me._ServiceRequestValuesTextBox.TabIndex = 13
        Me._ServiceRequestValuesTextBox.Text = "bit  Value" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  2  Device Specific Error" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  3  Temperature Event" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  4  Message Avai" &
    "lable" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  5  Standard Event Status" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  6  Master Summery Status" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  7  Ready"
        '
        '_SystemToolStrip
        '
        Me._SystemToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._SystemToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetSplitButton, Me._ReadTerminalStateButton, Me._ServiceRequestEnableNumericLabel, Me._ServiceRequestEnableNumeric})
        Me._SystemToolStrip.Location = New System.Drawing.Point(0, 242)
        Me._SystemToolStrip.Name = "_SystemToolStrip"
        Me._SystemToolStrip.Size = New System.Drawing.Size(356, 28)
        Me._SystemToolStrip.TabIndex = 21
        Me._SystemToolStrip.Text = "System Tools"
        Me._TipsToolTip.SetToolTip(Me._SystemToolStrip, "System operations")
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
        'ThermostreamPanel
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "ThermostreamPanel"
        Me.Size = New System.Drawing.Size(364, 450)
        Me.Controls.SetChildIndex(Me.Connector, 0)
        Me.Controls.SetChildIndex(Me._Layout, 0)
        Me._Tabs.ResumeLayout(False)
        Me._ReadingTabPage.ResumeLayout(False)
        Me._ReadingTabPage.PerformLayout()
        CType(Me._SrqRefratoryTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._FunctionTabPage.ResumeLayout(False)
        Me._FunctionTabPage.PerformLayout()
        Me._ToolStripPanel.ResumeLayout(False)
        Me._ToolStripPanel.PerformLayout()
        Me._OperatorCycleTabs.ResumeLayout(False)
        Me._CycleTabPage1.ResumeLayout(False)
        Me._CycleTabPage1.PerformLayout()
        Me._CycleToolStrip.ResumeLayout(False)
        Me._CycleToolStrip.PerformLayout()
        Me._OperatorTabPage.ResumeLayout(False)
        Me._OperatorTabPage.PerformLayout()
        CType(Me._ResetQueryDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SensorTabPage.ResumeLayout(False)
        Me._SensorTabPage.PerformLayout()
        Me._ReadWriteTabPage.ResumeLayout(False)
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._ReadingStatusStrip.ResumeLayout(False)
        Me._ReadingStatusStrip.PerformLayout()
        Me._StatusStrip.ResumeLayout(False)
        Me._StatusStrip.PerformLayout()
        CType(Me._InfoProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Panel.ResumeLayout(False)
        Me._Panel.PerformLayout()
        Me._Layout.ResumeLayout(False)
        Me._SystemToolStrip.ResumeLayout(False)
        Me._SystemToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _ReadingTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ReadButton As System.Windows.Forms.Button
    Private WithEvents _InitializeButton As System.Windows.Forms.Button
    Private WithEvents _FunctionTabPage As System.Windows.Forms.TabPage
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _SetpointNumericLabel As System.Windows.Forms.Label
    Private WithEvents _SoakTimeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _LastErrorTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ReadingStatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _ComplianceToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ReadingToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _LastReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TitleLabel As System.Windows.Forms.Label
    Private WithEvents _QueryStatusButton As System.Windows.Forms.Button
    Private WithEvents _SetpointNumberNumericLabel As System.Windows.Forms.Label
    Private WithEvents _SoakTimeNumeric As isr.Core.Controls.SelectorNumeric
    Private WithEvents _SetpointNumeric As isr.Core.Controls.SelectorNumeric
    Private WithEvents _SetpointNumberNumeric As isr.Core.Controls.SelectorNumeric
    Private WithEvents _SetpointWindowNumericLabel As System.Windows.Forms.Label
    Private WithEvents _SetpointWindowNumeric As isr.Core.Controls.SelectorNumeric
    Private WithEvents _NotAtTempCheckBox As isr.Core.Controls.CheckBox
    Private WithEvents _AtTempCheckBox As isr.Core.Controls.CheckBox
    Private WithEvents _HeadDownCheckBox As isr.Core.Controls.CheckBox
    Private WithEvents _ClearErrorQueueButton As System.Windows.Forms.Button
    Private WithEvents _ReadLastErrorButton As System.Windows.Forms.Button
    Private WithEvents _StatusByteLabel As System.Windows.Forms.Label
    Private WithEvents _ReadStatusByteButton As System.Windows.Forms.Button
    Private WithEvents _SendButton As System.Windows.Forms.Button
    Private WithEvents _ReceiveButton As System.Windows.Forms.Button
    Private WithEvents _SendComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _ReceiveTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ReadSetpointButton As System.Windows.Forms.Button
    Private WithEvents _TestTimeElapsedCheckBox As isr.Core.Controls.CheckBox
    Private WithEvents _SrqRefratoryTimeNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _SrqRefratoryTimeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _RampRateNumeric As isr.Core.Controls.SelectorNumeric
    Private WithEvents _RampRateNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ReadyCheckBox As isr.Core.Controls.CheckBox
    Private WithEvents _MaxTestTimeNumeric As isr.Core.Controls.SelectorNumeric
    Private WithEvents _MaxTestTimeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _CycleCountNumericLabel As System.Windows.Forms.Label
    Private WithEvents _CycleCountNumeric As isr.Core.Controls.SelectorNumeric
    Private WithEvents _OperatorCycleTabs As System.Windows.Forms.TabControl
    Private WithEvents _CycleTabPage1 As System.Windows.Forms.TabPage
    Private WithEvents _OperatorTabPage As System.Windows.Forms.TabPage
    Private WithEvents _CycleToolStrip As System.Windows.Forms.ToolStrip
    Private WithEvents _StartCycleButton As System.Windows.Forms.ToolStripButton
    Private WithEvents _StopCycleButton As System.Windows.Forms.ToolStripButton
    Private WithEvents _NextSetpointButton As System.Windows.Forms.ToolStripButton
    Private WithEvents _CyclesCompletedCheckBox1 As isr.Core.Controls.ToolStripCheckBox
    Private WithEvents _CycleCompletedCheckBox1 As isr.Core.Controls.ToolStripCheckBox
    Private WithEvents _CyclingStoppedCheckBox As isr.Core.Controls.ToolStripCheckBox
    Private WithEvents _FindLastSetpointButton1 As System.Windows.Forms.ToolStripButton
    Private WithEvents _ResetCycleModeButton As System.Windows.Forms.ToolStripButton
    Private WithEvents _ResetOperatorModeButton As System.Windows.Forms.Button
    Private WithEvents _ResetDelayNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ResetQueryDelayNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _SensorTabPage As System.Windows.Forms.TabPage
    Private WithEvents _DeviceSensorTypeSelector As isr.Core.Controls.SelectorComboBox
    Private WithEvents _DeviceThermalConstantSelector As isr.Core.Controls.SelectorNumeric
    Private WithEvents _DeviceThermalConstantSelectorLabel As System.Windows.Forms.Label
    Private WithEvents _DeviceSensorTypeSelectorLabel As System.Windows.Forms.Label
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
    Private WithEvents _ServiceRequestValuesTextBox As Windows.Forms.TextBox
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
    Private WithEvents _LogTraceLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _DisplayTraceLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _ResourceSelectorConnector As VI.Instrument.ResourceSelectorConnector
    Private WithEvents _TraceMessagesBox As isr.Core.Pith.TraceMessagesBox
    Private WithEvents _ToolStripPanel As Windows.Forms.ToolStripPanel
    Private WithEvents _InfoProvider As isr.Core.Controls.InfoProvider
    Private WithEvents _TipsToolTip As Windows.Forms.ToolTip
    Private WithEvents _StatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _IdentityLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _StatusRegisterLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _StandardRegisterLabel As System.Windows.Forms.ToolStripStatusLabel
End Class
