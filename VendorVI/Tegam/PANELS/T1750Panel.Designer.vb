<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class T1750Panel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
        Me._measureSettingsLabel = New System.Windows.Forms.Label()
        Me._MeasureButton = New System.Windows.Forms.Button()
        Me._PostReadingDelayNumericLabel = New System.Windows.Forms.Label()
        Me._PostReadingDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ReadContinuouslyCheckBox = New System.Windows.Forms.CheckBox()
        Me._CommandComboBox = New System.Windows.Forms.ComboBox()
        Me._WriteButton = New System.Windows.Forms.Button()
        Me._ReadButton = New System.Windows.Forms.Button()
        Me._ReadSRQButton = New System.Windows.Forms.Button()
        Me._ConfigureTabPage = New System.Windows.Forms.TabPage()
        Me._ConfigureGroupBox = New System.Windows.Forms.GroupBox()
        Me._RangeComboBox = New System.Windows.Forms.ComboBox()
        Me._TriggerCombo = New System.Windows.Forms.ComboBox()
        Me._TriggerComboBoxLabel = New System.Windows.Forms.Label()
        Me._ConfigureButton = New System.Windows.Forms.Button()
        Me._RangeComboBoxLabel = New System.Windows.Forms.Label()
        Me._ResetTabPage = New System.Windows.Forms.TabPage()
        Me._ResetLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._InitializeKnownStateButton = New System.Windows.Forms.Button()
        Me._InterfaceClearButton = New System.Windows.Forms.Button()
        Me._ResetButton = New System.Windows.Forms.Button()
        Me._SelectiveDeviceClearButton = New System.Windows.Forms.Button()
        Me._SessionTraceEnableCheckBox = New System.Windows.Forms.CheckBox()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._LastErrorTextBox = New System.Windows.Forms.TextBox()
        Me._ReadingStatusStrip = New System.Windows.Forms.StatusStrip()
        Me._ComplianceToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._ReadingToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._TbdToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._Panel = New System.Windows.Forms.Panel()
        Me._Layout = New System.Windows.Forms.TableLayoutPanel()
        Me._TitleLabel = New System.Windows.Forms.Label()
        Me._ReadWriteTabPage = New System.Windows.Forms.TabPage()
        Me._SimpleReadWriteControl = New isr.VI.Instrument.SimpleReadWriteControl()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        CType(Me._PostReadingDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ConfigureTabPage.SuspendLayout()
        Me._ConfigureGroupBox.SuspendLayout()
        Me._ResetTabPage.SuspendLayout()
        Me._ResetLayout.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
        Me._ReadingStatusStrip.SuspendLayout()
        Me._Panel.SuspendLayout()
        Me._Layout.SuspendLayout()
        Me._ReadWriteTabPage.SuspendLayout()
        Me.SuspendLayout()
        '
        'Connector
        '
        Me.Connector.Location = New System.Drawing.Point(0, 388)
        Me.Connector.Searchable = True
        '
        'TraceMessagesBox
        '
        Me.TraceMessagesBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TraceMessagesBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TraceMessagesBox.PresetCount = 50
        Me.TraceMessagesBox.ResetCount = 100
        Me.TraceMessagesBox.Size = New System.Drawing.Size(356, 286)
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ReadingTabPage)
        Me._Tabs.Controls.Add(Me._ConfigureTabPage)
        Me._Tabs.Controls.Add(Me._ResetTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(52, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 55)
        Me._Tabs.Multiline = True
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 316)
        Me._Tabs.TabIndex = 1
        '
        '_ReadingTabPage
        '
        Me._ReadingTabPage.Controls.Add(Me._measureSettingsLabel)
        Me._ReadingTabPage.Controls.Add(Me._MeasureButton)
        Me._ReadingTabPage.Controls.Add(Me._PostReadingDelayNumericLabel)
        Me._ReadingTabPage.Controls.Add(Me._PostReadingDelayNumeric)
        Me._ReadingTabPage.Controls.Add(Me._ReadContinuouslyCheckBox)
        Me._ReadingTabPage.Controls.Add(Me._CommandComboBox)
        Me._ReadingTabPage.Controls.Add(Me._WriteButton)
        Me._ReadingTabPage.Controls.Add(Me._ReadButton)
        Me._ReadingTabPage.Controls.Add(Me._ReadSRQButton)
        Me._ReadingTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadingTabPage.Name = "_ReadingTabPage"
        Me._ReadingTabPage.Size = New System.Drawing.Size(356, 286)
        Me._ReadingTabPage.TabIndex = 0
        Me._ReadingTabPage.Text = " Reading"
        Me._ReadingTabPage.UseVisualStyleBackColor = True
        '
        '_measureSettingsLabel
        '
        Me._measureSettingsLabel.Location = New System.Drawing.Point(105, 110)
        Me._measureSettingsLabel.Name = "_measureSettingsLabel"
        Me._measureSettingsLabel.Size = New System.Drawing.Size(232, 36)
        Me._measureSettingsLabel.TabIndex = 8
        Me._measureSettingsLabel.Text = "5 measurements, 150ms delays, 1% accuracy."
        '
        '_MeasureButton
        '
        Me._MeasureButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._MeasureButton.Location = New System.Drawing.Point(12, 111)
        Me._MeasureButton.Name = "_MeasureButton"
        Me._MeasureButton.Size = New System.Drawing.Size(75, 30)
        Me._MeasureButton.TabIndex = 7
        Me._MeasureButton.Text = "&Measure"
        Me._MeasureButton.UseVisualStyleBackColor = True
        '
        '_PostReadingDelayNumericLabel
        '
        Me._PostReadingDelayNumericLabel.AutoSize = True
        Me._PostReadingDelayNumericLabel.Location = New System.Drawing.Point(180, 22)
        Me._PostReadingDelayNumericLabel.Name = "_PostReadingDelayNumericLabel"
        Me._PostReadingDelayNumericLabel.Size = New System.Drawing.Size(72, 17)
        Me._PostReadingDelayNumericLabel.TabIndex = 2
        Me._PostReadingDelayNumericLabel.Text = "Delay [ms]:"
        '
        '_PostReadingDelayNumeric
        '
        Me._PostReadingDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PostReadingDelayNumeric.Location = New System.Drawing.Point(255, 18)
        Me._PostReadingDelayNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me._PostReadingDelayNumeric.Name = "_PostReadingDelayNumeric"
        Me._PostReadingDelayNumeric.Size = New System.Drawing.Size(49, 25)
        Me._PostReadingDelayNumeric.TabIndex = 3
        Me.TipsTooltip.SetToolTip(Me._PostReadingDelayNumeric, "Milliseconds delay before the next reading.")
        '
        '_ReadContinuouslyCheckBox
        '
        Me._ReadContinuouslyCheckBox.AutoSize = True
        Me._ReadContinuouslyCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ReadContinuouslyCheckBox.Location = New System.Drawing.Point(76, 20)
        Me._ReadContinuouslyCheckBox.Name = "_ReadContinuouslyCheckBox"
        Me._ReadContinuouslyCheckBox.Size = New System.Drawing.Size(98, 21)
        Me._ReadContinuouslyCheckBox.TabIndex = 1
        Me._ReadContinuouslyCheckBox.Text = "Continuous"
        Me.TipsTooltip.SetToolTip(Me._ReadContinuouslyCheckBox, "Gets a new reading immediately after completing the previous reading.")
        Me._ReadContinuouslyCheckBox.UseVisualStyleBackColor = True
        '
        '_CommandComboBox
        '
        Me._CommandComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._CommandComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._CommandComboBox.FormattingEnabled = True
        Me._CommandComboBox.Location = New System.Drawing.Point(76, 62)
        Me._CommandComboBox.Name = "_CommandComboBox"
        Me._CommandComboBox.Size = New System.Drawing.Size(60, 25)
        Me._CommandComboBox.TabIndex = 5
        Me._CommandComboBox.Text = "D111x"
        Me.TipsTooltip.SetToolTip(Me._CommandComboBox, "Command to send to the Meter")
        '
        '_WriteButton
        '
        Me._WriteButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._WriteButton.Location = New System.Drawing.Point(12, 59)
        Me._WriteButton.Name = "_WriteButton"
        Me._WriteButton.Size = New System.Drawing.Size(58, 30)
        Me._WriteButton.TabIndex = 4
        Me._WriteButton.Text = "&Write"
        Me.TipsTooltip.SetToolTip(Me._WriteButton, "Click to send the command to the meter.")
        Me._WriteButton.UseVisualStyleBackColor = True
        '
        '_ReadButton
        '
        Me._ReadButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ReadButton.Location = New System.Drawing.Point(12, 15)
        Me._ReadButton.Name = "_ReadButton"
        Me._ReadButton.Size = New System.Drawing.Size(58, 30)
        Me._ReadButton.TabIndex = 0
        Me._ReadButton.Text = "&Read"
        Me._ReadButton.UseVisualStyleBackColor = True
        '
        '_ReadSRQButton
        '
        Me._ReadSRQButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ReadSRQButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ReadSRQButton.Location = New System.Drawing.Point(178, 60)
        Me._ReadSRQButton.Name = "_ReadSRQButton"
        Me._ReadSRQButton.Size = New System.Drawing.Size(166, 30)
        Me._ReadSRQButton.TabIndex = 6
        Me._ReadSRQButton.Text = "Read &Service Request"
        Me._ReadSRQButton.UseVisualStyleBackColor = True
        '
        '_ConfigureTabPage
        '
        Me._ConfigureTabPage.Controls.Add(Me._ConfigureGroupBox)
        Me._ConfigureTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ConfigureTabPage.Name = "_ConfigureTabPage"
        Me._ConfigureTabPage.Size = New System.Drawing.Size(356, 286)
        Me._ConfigureTabPage.TabIndex = 4
        Me._ConfigureTabPage.Text = "Configure"
        Me._ConfigureTabPage.UseVisualStyleBackColor = True
        '
        '_ConfigureGroupBox
        '
        Me._ConfigureGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ConfigureGroupBox.Controls.Add(Me._RangeComboBox)
        Me._ConfigureGroupBox.Controls.Add(Me._TriggerCombo)
        Me._ConfigureGroupBox.Controls.Add(Me._TriggerComboBoxLabel)
        Me._ConfigureGroupBox.Controls.Add(Me._ConfigureButton)
        Me._ConfigureGroupBox.Controls.Add(Me._RangeComboBoxLabel)
        Me._ConfigureGroupBox.Location = New System.Drawing.Point(12, 58)
        Me._ConfigureGroupBox.Name = "_ConfigureGroupBox"
        Me._ConfigureGroupBox.Size = New System.Drawing.Size(332, 169)
        Me._ConfigureGroupBox.TabIndex = 8
        Me._ConfigureGroupBox.TabStop = False
        Me._ConfigureGroupBox.Text = "Configure Measurement"
        '
        '_RangeComboBox
        '
        Me._RangeComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._RangeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._RangeComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._RangeComboBox.Location = New System.Drawing.Point(11, 41)
        Me._RangeComboBox.Name = "_RangeComboBox"
        Me._RangeComboBox.Size = New System.Drawing.Size(310, 25)
        Me._RangeComboBox.TabIndex = 1
        '
        '_TriggerCombo
        '
        Me._TriggerCombo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._TriggerCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._TriggerCombo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._TriggerCombo.Items.AddRange(New Object() {"I", "V"})
        Me._TriggerCombo.Location = New System.Drawing.Point(11, 92)
        Me._TriggerCombo.Name = "_TriggerCombo"
        Me._TriggerCombo.Size = New System.Drawing.Size(310, 25)
        Me._TriggerCombo.TabIndex = 3
        '
        '_TriggerComboBoxLabel
        '
        Me._TriggerComboBoxLabel.Location = New System.Drawing.Point(9, 69)
        Me._TriggerComboBoxLabel.Name = "_TriggerComboBoxLabel"
        Me._TriggerComboBoxLabel.Size = New System.Drawing.Size(62, 21)
        Me._TriggerComboBoxLabel.TabIndex = 2
        Me._TriggerComboBoxLabel.Text = "Trigger: "
        Me._TriggerComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_ConfigureButton
        '
        Me._ConfigureButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ConfigureButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ConfigureButton.Location = New System.Drawing.Point(263, 132)
        Me._ConfigureButton.Name = "_ConfigureButton"
        Me._ConfigureButton.Size = New System.Drawing.Size(58, 30)
        Me._ConfigureButton.TabIndex = 4
        Me._ConfigureButton.Text = "&Apply"
        Me._ConfigureButton.UseVisualStyleBackColor = True
        '
        '_RangeComboBoxLabel
        '
        Me._RangeComboBoxLabel.AutoSize = True
        Me._RangeComboBoxLabel.Location = New System.Drawing.Point(9, 21)
        Me._RangeComboBoxLabel.Name = "_RangeComboBoxLabel"
        Me._RangeComboBoxLabel.Size = New System.Drawing.Size(52, 17)
        Me._RangeComboBoxLabel.TabIndex = 0
        Me._RangeComboBoxLabel.Text = "Range: "
        Me._RangeComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_ResetTabPage
        '
        Me._ResetTabPage.Controls.Add(Me._ResetLayout)
        Me._ResetTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ResetTabPage.Name = "_ResetTabPage"
        Me._ResetTabPage.Size = New System.Drawing.Size(356, 286)
        Me._ResetTabPage.TabIndex = 2
        Me._ResetTabPage.Text = " Reset"
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
        Me._ResetLayout.Size = New System.Drawing.Size(356, 286)
        Me._ResetLayout.TabIndex = 4
        '
        '_InitializeKnownStateButton
        '
        Me._InitializeKnownStateButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._InitializeKnownStateButton.Location = New System.Drawing.Point(84, 187)
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
        Me._InterfaceClearButton.Location = New System.Drawing.Point(84, 22)
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
        Me._ResetButton.Location = New System.Drawing.Point(84, 132)
        Me._ResetButton.Name = "_ResetButton"
        Me._ResetButton.Size = New System.Drawing.Size(187, 30)
        Me._ResetButton.TabIndex = 2
        Me._ResetButton.Text = "&Reset to Known State"
        Me._ResetButton.UseVisualStyleBackColor = True
        '
        '_SelectiveDeviceClearButton
        '
        Me._SelectiveDeviceClearButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SelectiveDeviceClearButton.Location = New System.Drawing.Point(84, 77)
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
        Me._SessionTraceEnableCheckBox.Location = New System.Drawing.Point(84, 242)
        Me._SessionTraceEnableCheckBox.Name = "_SessionTraceEnableCheckBox"
        Me._SessionTraceEnableCheckBox.Size = New System.Drawing.Size(186, 21)
        Me._SessionTraceEnableCheckBox.TabIndex = 4
        Me._SessionTraceEnableCheckBox.Text = "Trace Instrument Messages"
        Me.TipsTooltip.SetToolTip(Me._SessionTraceEnableCheckBox, "Check to trace all instrument messages ")
        Me._SessionTraceEnableCheckBox.UseVisualStyleBackColor = True
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me.TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(356, 286)
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
        Me._LastErrorTextBox.Location = New System.Drawing.Point(0, 37)
        Me._LastErrorTextBox.Name = "_LastErrorTextBox"
        Me._LastErrorTextBox.Size = New System.Drawing.Size(364, 18)
        Me._LastErrorTextBox.TabIndex = 16
        Me._LastErrorTextBox.Text = "000, No Errors"
        '
        '_ReadingStatusStrip
        '
        Me._ReadingStatusStrip.BackColor = System.Drawing.Color.Black
        Me._ReadingStatusStrip.Dock = System.Windows.Forms.DockStyle.Top
        Me._ReadingStatusStrip.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ReadingStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ComplianceToolStripStatusLabel, Me._ReadingToolStripStatusLabel, Me._TbdToolStripStatusLabel})
        Me._ReadingStatusStrip.Location = New System.Drawing.Point(0, 0)
        Me._ReadingStatusStrip.Name = "_ReadingStatusStrip"
        Me._ReadingStatusStrip.Size = New System.Drawing.Size(364, 37)
        Me._ReadingStatusStrip.SizingGrip = False
        Me._ReadingStatusStrip.TabIndex = 17
        '
        '_ComplianceToolStripStatusLabel
        '
        Me._ComplianceToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ComplianceToolStripStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ComplianceToolStripStatusLabel.ForeColor = System.Drawing.Color.Red
        Me._ComplianceToolStripStatusLabel.Name = "_ComplianceToolStripStatusLabel"
        Me._ComplianceToolStripStatusLabel.Size = New System.Drawing.Size(16, 32)
        Me._ComplianceToolStripStatusLabel.Text = "C"
        Me._ComplianceToolStripStatusLabel.ToolTipText = "Compliance"
        '
        '_ReadingToolStripStatusLabel
        '
        Me._ReadingToolStripStatusLabel.AutoSize = False
        Me._ReadingToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ReadingToolStripStatusLabel.ForeColor = System.Drawing.Color.Aquamarine
        Me._ReadingToolStripStatusLabel.Name = "_ReadingToolStripStatusLabel"
        Me._ReadingToolStripStatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._ReadingToolStripStatusLabel.Size = New System.Drawing.Size(313, 32)
        Me._ReadingToolStripStatusLabel.Spring = True
        Me._ReadingToolStripStatusLabel.Text = "0.0000 Ohm"
        Me._ReadingToolStripStatusLabel.ToolTipText = "Reading"
        '
        '_TbdToolStripStatusLabel
        '
        Me._TbdToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TbdToolStripStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TbdToolStripStatusLabel.ForeColor = System.Drawing.Color.Aquamarine
        Me._TbdToolStripStatusLabel.Name = "_TbdToolStripStatusLabel"
        Me._TbdToolStripStatusLabel.Size = New System.Drawing.Size(20, 32)
        Me._TbdToolStripStatusLabel.Text = " T"
        Me._TbdToolStripStatusLabel.ToolTipText = "To be defined"
        '
        '_Panel
        '
        Me._Panel.Controls.Add(Me._Tabs)
        Me._Panel.Controls.Add(Me._LastErrorTextBox)
        Me._Panel.Controls.Add(Me._ReadingStatusStrip)
        Me._Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Panel.Location = New System.Drawing.Point(0, 17)
        Me._Panel.Margin = New System.Windows.Forms.Padding(0)
        Me._Panel.Name = "_Panel"
        Me._Panel.Size = New System.Drawing.Size(364, 371)
        Me._Panel.TabIndex = 18
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
        Me._Layout.TabIndex = 19
        '
        '_TitleLabel
        '
        Me._TitleLabel.AutoSize = False
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
        Me._TitleLabel.Text = "T1750"
        Me._TitleLabel.UseMnemonic = False
        '
        '_ReadWriteTabPage
        '
        Me._ReadWriteTabPage.Controls.Add(Me._SimpleReadWriteControl)
        Me._ReadWriteTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadWriteTabPage.Name = "_ReadWriteTabPage"
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(356, 286)
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
        Me._SimpleReadWriteControl.Size = New System.Drawing.Size(356, 286)
        Me._SimpleReadWriteControl.TabIndex = 0
        '
        'T1750Panel
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "T1750Panel"
        Me.Size = New System.Drawing.Size(364, 450)
        Me.Controls.SetChildIndex(Me.Connector, 0)
        Me.Controls.SetChildIndex(Me._Layout, 0)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Tabs.ResumeLayout(False)
        Me._ReadingTabPage.ResumeLayout(False)
        Me._ReadingTabPage.PerformLayout()
        CType(Me._PostReadingDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ConfigureTabPage.ResumeLayout(False)
        Me._ConfigureGroupBox.ResumeLayout(False)
        Me._ConfigureGroupBox.PerformLayout()
        Me._ResetTabPage.ResumeLayout(False)
        Me._ResetLayout.ResumeLayout(False)
        Me._ResetLayout.PerformLayout()
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._ReadingStatusStrip.ResumeLayout(False)
        Me._ReadingStatusStrip.PerformLayout()
        Me._Panel.ResumeLayout(False)
        Me._Panel.PerformLayout()
        Me._Layout.ResumeLayout(False)
        Me._Layout.PerformLayout()
        Me._ReadWriteTabPage.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _ReadingTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ReadButton As System.Windows.Forms.Button
    Private WithEvents _ReadSRQButton As System.Windows.Forms.Button
    Private WithEvents _ResetTabPage As System.Windows.Forms.TabPage
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _CommandComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _WriteButton As System.Windows.Forms.Button
    Private WithEvents _LastErrorTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ResetLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _InitializeKnownStateButton As System.Windows.Forms.Button
    Private WithEvents _InterfaceClearButton As System.Windows.Forms.Button
    Private WithEvents _ResetButton As System.Windows.Forms.Button
    Private WithEvents _SelectiveDeviceClearButton As System.Windows.Forms.Button
    Private WithEvents _SessionTraceEnableCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents _PostReadingDelayNumericLabel As System.Windows.Forms.Label
    Private WithEvents _PostReadingDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ReadContinuouslyCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents _ConfigureTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ConfigureGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents _RangeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _TriggerCombo As System.Windows.Forms.ComboBox
    Private WithEvents _TriggerComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _ConfigureButton As System.Windows.Forms.Button
    Private WithEvents _RangeComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _measureSettingsLabel As System.Windows.Forms.Label
    Private WithEvents _MeasureButton As System.Windows.Forms.Button
    Private WithEvents _ReadingStatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _ComplianceToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ReadingToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _TbdToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TitleLabel As System.Windows.Forms.Label
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
End Class
