<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SimpleReadWritePanel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="statusPanel")> <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ResourcesTabPage = New System.Windows.Forms.TabPage()
        Me._SessionInfoTextBox = New System.Windows.Forms.TextBox()
        Me._CloseSessionButton = New System.Windows.Forms.Button()
        Me._OpenSessionButton = New System.Windows.Forms.Button()
        Me._ReadWriteTabPage = New System.Windows.Forms.TabPage()
        Me._SimpleReadWriteControl = New isr.VI.Instrument.SimpleReadWriteControl()
        Me._SettingsTabPage = New System.Windows.Forms.TabPage()
        Me._SettingsLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._SettingsPanel = New System.Windows.Forms.Panel()
        Me._TerminationGroupBox = New System.Windows.Forms.GroupBox()
        Me._AppendNewLineCheckBox = New System.Windows.Forms.CheckBox()
        Me._AppendReturnCheckBox = New System.Windows.Forms.CheckBox()
        Me._PollSettingsGroupBox = New System.Windows.Forms.GroupBox()
        Me._MessageStatusBitValueNumeric = New System.Windows.Forms.NumericUpDown()
        Me._MessageStatusBitsNumericLabel = New System.Windows.Forms.Label()
        Me._PollMillisecondsNumeric = New System.Windows.Forms.NumericUpDown()
        Me._PollMillisecondsNumericLabel = New System.Windows.Forms.Label()
        Me._AutoReadCheckBox = New System.Windows.Forms.CheckBox()
        Me._PollEnableCheckBox = New System.Windows.Forms.CheckBox()
        Me._TimeoutSelectorLabel = New System.Windows.Forms.Label()
        Me._TimeoutSelector = New System.Windows.Forms.NumericUpDown()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._TraceMessagesBox = New isr.Core.Pith.TraceMessagesBox()
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._StatusStrip = New System.Windows.Forms.StatusStrip()
        Me._StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._ServiceRequestStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._PollTimer = New System.Windows.Forms.Timer(Me.components)
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._ReadTerminationComboBox = New System.Windows.Forms.ComboBox()
        Me._ReadTerminationComboBoxLabel = New System.Windows.Forms.Label()
        Me._Tabs.SuspendLayout()
        Me._ResourcesTabPage.SuspendLayout()
        Me._ReadWriteTabPage.SuspendLayout()
        Me._SettingsTabPage.SuspendLayout()
        Me._SettingsLayout.SuspendLayout()
        Me._SettingsPanel.SuspendLayout()
        Me._TerminationGroupBox.SuspendLayout()
        Me._PollSettingsGroupBox.SuspendLayout()
        CType(Me._MessageStatusBitValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._PollMillisecondsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._TimeoutSelector, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._MessagesTabPage.SuspendLayout()
        Me._StatusStrip.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ResourcesTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._SettingsTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.Location = New System.Drawing.Point(0, 0)
        Me._Tabs.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(338, 371)
        Me._Tabs.TabIndex = 12
        '
        '_ResourcesTabPage
        '
        Me._ResourcesTabPage.Controls.Add(Me._SessionInfoTextBox)
        Me._ResourcesTabPage.Controls.Add(Me._CloseSessionButton)
        Me._ResourcesTabPage.Controls.Add(Me._OpenSessionButton)
        Me._ResourcesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ResourcesTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ResourcesTabPage.Name = "_ResourcesTabPage"
        Me._ResourcesTabPage.Size = New System.Drawing.Size(330, 341)
        Me._ResourcesTabPage.TabIndex = 0
        Me._ResourcesTabPage.Text = "Resources"
        Me._ResourcesTabPage.UseVisualStyleBackColor = True
        '
        '_SessionInfoTextBox
        '
        Me._SessionInfoTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SessionInfoTextBox.Location = New System.Drawing.Point(9, 45)
        Me._SessionInfoTextBox.Multiline = True
        Me._SessionInfoTextBox.Name = "_SessionInfoTextBox"
        Me._SessionInfoTextBox.Size = New System.Drawing.Size(312, 289)
        Me._SessionInfoTextBox.TabIndex = 41
        '
        '_CloseSessionButton
        '
        Me._CloseSessionButton.Enabled = False
        Me._CloseSessionButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._CloseSessionButton.Location = New System.Drawing.Point(117, 9)
        Me._CloseSessionButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._CloseSessionButton.Name = "_CloseSessionButton"
        Me._CloseSessionButton.Size = New System.Drawing.Size(107, 29)
        Me._CloseSessionButton.TabIndex = 40
        Me._CloseSessionButton.Text = "Close Session"
        '
        '_OpenSessionButton
        '
        Me._OpenSessionButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._OpenSessionButton.Location = New System.Drawing.Point(10, 9)
        Me._OpenSessionButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._OpenSessionButton.Name = "_OpenSessionButton"
        Me._OpenSessionButton.Size = New System.Drawing.Size(107, 29)
        Me._OpenSessionButton.TabIndex = 39
        Me._OpenSessionButton.Text = "Open Session"
        '
        '_ReadWriteTabPage
        '
        Me._ReadWriteTabPage.Controls.Add(Me._SimpleReadWriteControl)
        Me._ReadWriteTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadWriteTabPage.Name = "_ReadWriteTabPage"
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(330, 341)
        Me._ReadWriteTabPage.TabIndex = 3
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
        Me._SimpleReadWriteControl.Size = New System.Drawing.Size(330, 341)
        Me._SimpleReadWriteControl.TabIndex = 0
        '
        '_SettingsTabPage
        '
        Me._SettingsTabPage.Controls.Add(Me._SettingsLayout)
        Me._SettingsTabPage.Location = New System.Drawing.Point(4, 26)
        Me._SettingsTabPage.Name = "_SettingsTabPage"
        Me._SettingsTabPage.Size = New System.Drawing.Size(330, 341)
        Me._SettingsTabPage.TabIndex = 2
        Me._SettingsTabPage.Text = "Settings"
        Me._SettingsTabPage.UseVisualStyleBackColor = True
        '
        '_SettingsLayout
        '
        Me._SettingsLayout.ColumnCount = 3
        Me._SettingsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._SettingsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._SettingsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._SettingsLayout.Controls.Add(Me._SettingsPanel, 1, 1)
        Me._SettingsLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._SettingsLayout.Location = New System.Drawing.Point(0, 0)
        Me._SettingsLayout.Name = "_SettingsLayout"
        Me._SettingsLayout.RowCount = 3
        Me._SettingsLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._SettingsLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._SettingsLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._SettingsLayout.Size = New System.Drawing.Size(330, 341)
        Me._SettingsLayout.TabIndex = 22
        '
        '_SettingsPanel
        '
        Me._SettingsPanel.Controls.Add(Me._TerminationGroupBox)
        Me._SettingsPanel.Controls.Add(Me._PollSettingsGroupBox)
        Me._SettingsPanel.Controls.Add(Me._TimeoutSelectorLabel)
        Me._SettingsPanel.Controls.Add(Me._TimeoutSelector)
        Me._SettingsPanel.Location = New System.Drawing.Point(17, 17)
        Me._SettingsPanel.Name = "_SettingsPanel"
        Me._SettingsPanel.Size = New System.Drawing.Size(296, 307)
        Me._SettingsPanel.TabIndex = 23
        '
        '_TerminationGroupBox
        '
        Me._TerminationGroupBox.Controls.Add(Me._ReadTerminationComboBoxLabel)
        Me._TerminationGroupBox.Controls.Add(Me._ReadTerminationComboBox)
        Me._TerminationGroupBox.Controls.Add(Me._AppendNewLineCheckBox)
        Me._TerminationGroupBox.Controls.Add(Me._AppendReturnCheckBox)
        Me._TerminationGroupBox.Location = New System.Drawing.Point(3, 41)
        Me._TerminationGroupBox.Name = "_TerminationGroupBox"
        Me._TerminationGroupBox.Size = New System.Drawing.Size(289, 100)
        Me._TerminationGroupBox.TabIndex = 18
        Me._TerminationGroupBox.TabStop = False
        Me._TerminationGroupBox.Text = "Termination"
        '
        '_AppendNewLineCheckBox
        '
        Me._AppendNewLineCheckBox.AutoSize = True
        Me._AppendNewLineCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._AppendNewLineCheckBox.Location = New System.Drawing.Point(8, 64)
        Me._AppendNewLineCheckBox.Name = "_AppendNewLineCheckBox"
        Me._AppendNewLineCheckBox.Size = New System.Drawing.Size(138, 21)
        Me._AppendNewLineCheckBox.TabIndex = 1
        Me._AppendNewLineCheckBox.Text = "Auto New Line [\n]:"
        Me._AppendNewLineCheckBox.UseVisualStyleBackColor = True
        '
        '_AppendReturnCheckBox
        '
        Me._AppendReturnCheckBox.AutoSize = True
        Me._AppendReturnCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._AppendReturnCheckBox.Location = New System.Drawing.Point(25, 32)
        Me._AppendReturnCheckBox.Name = "_AppendReturnCheckBox"
        Me._AppendReturnCheckBox.Size = New System.Drawing.Size(121, 21)
        Me._AppendReturnCheckBox.TabIndex = 0
        Me._AppendReturnCheckBox.Text = "Auto Return [\r]:"
        Me._AppendReturnCheckBox.UseVisualStyleBackColor = True
        '
        '_PollSettingsGroupBox
        '
        Me._PollSettingsGroupBox.Controls.Add(Me._MessageStatusBitValueNumeric)
        Me._PollSettingsGroupBox.Controls.Add(Me._MessageStatusBitsNumericLabel)
        Me._PollSettingsGroupBox.Controls.Add(Me._PollMillisecondsNumeric)
        Me._PollSettingsGroupBox.Controls.Add(Me._PollMillisecondsNumericLabel)
        Me._PollSettingsGroupBox.Controls.Add(Me._AutoReadCheckBox)
        Me._PollSettingsGroupBox.Controls.Add(Me._PollEnableCheckBox)
        Me._PollSettingsGroupBox.Location = New System.Drawing.Point(3, 148)
        Me._PollSettingsGroupBox.Name = "_PollSettingsGroupBox"
        Me._PollSettingsGroupBox.Size = New System.Drawing.Size(289, 156)
        Me._PollSettingsGroupBox.TabIndex = 19
        Me._PollSettingsGroupBox.TabStop = False
        Me._PollSettingsGroupBox.Text = "Poll Service Request Byte Settings"
        '
        '_MessageStatusBitValueNumeric
        '
        Me._MessageStatusBitValueNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._MessageStatusBitValueNumeric.Hexadecimal = True
        Me._MessageStatusBitValueNumeric.Location = New System.Drawing.Point(216, 92)
        Me._MessageStatusBitValueNumeric.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me._MessageStatusBitValueNumeric.Name = "_MessageStatusBitValueNumeric"
        Me._MessageStatusBitValueNumeric.Size = New System.Drawing.Size(51, 25)
        Me._MessageStatusBitValueNumeric.TabIndex = 4
        Me._MessageStatusBitValueNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._MessageStatusBitValueNumeric, "Bit value to detect if a message is available for reading.")
        Me._MessageStatusBitValueNumeric.Value = New Decimal(New Integer() {16, 0, 0, 0})
        '
        '_MessageStatusBitsNumericLabel
        '
        Me._MessageStatusBitsNumericLabel.AutoSize = True
        Me._MessageStatusBitsNumericLabel.Location = New System.Drawing.Point(25, 96)
        Me._MessageStatusBitsNumericLabel.Name = "_MessageStatusBitsNumericLabel"
        Me._MessageStatusBitsNumericLabel.Size = New System.Drawing.Size(188, 17)
        Me._MessageStatusBitsNumericLabel.TabIndex = 3
        Me._MessageStatusBitsNumericLabel.Text = "Message Status Bit Value [hex]:"
        '
        '_PollMillisecondsNumeric
        '
        Me._PollMillisecondsNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._PollMillisecondsNumeric.Location = New System.Drawing.Point(216, 60)
        Me._PollMillisecondsNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me._PollMillisecondsNumeric.Name = "_PollMillisecondsNumeric"
        Me._PollMillisecondsNumeric.Size = New System.Drawing.Size(51, 25)
        Me._PollMillisecondsNumeric.TabIndex = 2
        Me._ToolTip.SetToolTip(Me._PollMillisecondsNumeric, "Time interval of poll timer")
        Me._PollMillisecondsNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        '_PollMillisecondsNumericLabel
        '
        Me._PollMillisecondsNumericLabel.AutoSize = True
        Me._PollMillisecondsNumericLabel.Location = New System.Drawing.Point(70, 64)
        Me._PollMillisecondsNumericLabel.Name = "_PollMillisecondsNumericLabel"
        Me._PollMillisecondsNumericLabel.Size = New System.Drawing.Size(144, 17)
        Me._PollMillisecondsNumericLabel.TabIndex = 1
        Me._PollMillisecondsNumericLabel.Text = "Poll Timer Interval [ms]:"
        '
        '_AutoReadCheckBox
        '
        Me._AutoReadCheckBox.AutoSize = True
        Me._AutoReadCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._AutoReadCheckBox.Location = New System.Drawing.Point(57, 124)
        Me._AutoReadCheckBox.Name = "_AutoReadCheckBox"
        Me._AutoReadCheckBox.Size = New System.Drawing.Size(210, 21)
        Me._AutoReadCheckBox.TabIndex = 5
        Me._AutoReadCheckBox.Text = "Read When Message Detected:"
        Me._ToolTip.SetToolTip(Me._AutoReadCheckBox, "Check to have the program automatically read the message from the instrument when" &
        " message status bit is detected.")
        Me._AutoReadCheckBox.UseVisualStyleBackColor = True
        '
        '_PollEnableCheckBox
        '
        Me._PollEnableCheckBox.AutoSize = True
        Me._PollEnableCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._PollEnableCheckBox.Location = New System.Drawing.Point(64, 32)
        Me._PollEnableCheckBox.Name = "_PollEnableCheckBox"
        Me._PollEnableCheckBox.Size = New System.Drawing.Size(203, 21)
        Me._PollEnableCheckBox.TabIndex = 0
        Me._PollEnableCheckBox.Text = "Poll Service Request on Timer:"
        Me._PollEnableCheckBox.UseVisualStyleBackColor = True
        '
        '_TimeoutSelectorLabel
        '
        Me._TimeoutSelectorLabel.AutoSize = True
        Me._TimeoutSelectorLabel.Location = New System.Drawing.Point(9, 11)
        Me._TimeoutSelectorLabel.Name = "_TimeoutSelectorLabel"
        Me._TimeoutSelectorLabel.Size = New System.Drawing.Size(87, 17)
        Me._TimeoutSelectorLabel.TabIndex = 21
        Me._TimeoutSelectorLabel.Text = "Timeout [ms]:"
        '
        '_TimeoutSelector
        '
        Me._TimeoutSelector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TimeoutSelector.Location = New System.Drawing.Point(98, 7)
        Me._TimeoutSelector.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._TimeoutSelector.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me._TimeoutSelector.Name = "_TimeoutSelector"
        Me._TimeoutSelector.Size = New System.Drawing.Size(103, 25)
        Me._TimeoutSelector.TabIndex = 20
        Me._TimeoutSelector.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me._TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(330, 341)
        Me._MessagesTabPage.TabIndex = 1
        Me._MessagesTabPage.Text = "Log"
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
        Me._TraceMessagesBox.PresetCount = 50
        Me._TraceMessagesBox.ReadOnly = True
        Me._TraceMessagesBox.ResetCount = 100
        Me._TraceMessagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._TraceMessagesBox.Size = New System.Drawing.Size(330, 341)
        Me._TraceMessagesBox.TabIndex = 0
        '
        '_StatusStrip
        '
        Me._StatusStrip.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusLabel, Me._ServiceRequestStatusLabel})
        Me._StatusStrip.Location = New System.Drawing.Point(0, 371)
        Me._StatusStrip.Name = "_StatusStrip"
        Me._StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
        Me._StatusStrip.ShowItemToolTips = True
        Me._StatusStrip.Size = New System.Drawing.Size(338, 22)
        Me._StatusStrip.TabIndex = 14
        Me._StatusStrip.Text = "Status Strip"
        '
        '_StatusLabel
        '
        Me._StatusLabel.AutoToolTip = True
        Me._StatusLabel.Name = "_StatusLabel"
        Me._StatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._StatusLabel.Size = New System.Drawing.Size(285, 17)
        Me._StatusLabel.Spring = True
        Me._StatusLabel.Text = "<status>"
        Me._StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._StatusLabel.ToolTipText = "Status"
        '
        '_ServiceRequestStatusLabel
        '
        Me._ServiceRequestStatusLabel.Name = "_ServiceRequestStatusLabel"
        Me._ServiceRequestStatusLabel.Size = New System.Drawing.Size(36, 17)
        Me._ServiceRequestStatusLabel.Text = "0x00"
        '
        '_PollTimer
        '
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        '_ReadTerminationComboBox
        '
        Me._ReadTerminationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._ReadTerminationComboBox.FormattingEnabled = True
        Me._ReadTerminationComboBox.Items.AddRange(New Object() {"None", "Line Feed", "Return"})
        Me._ReadTerminationComboBox.Location = New System.Drawing.Point(158, 53)
        Me._ReadTerminationComboBox.Name = "_ReadTerminationComboBox"
        Me._ReadTerminationComboBox.Size = New System.Drawing.Size(121, 25)
        Me._ReadTerminationComboBox.TabIndex = 3
        '
        '_ReadTerminationComboBoxLabel
        '
        Me._ReadTerminationComboBoxLabel.AutoSize = True
        Me._ReadTerminationComboBoxLabel.Location = New System.Drawing.Point(161, 33)
        Me._ReadTerminationComboBoxLabel.Name = "_ReadTerminationComboBoxLabel"
        Me._ReadTerminationComboBoxLabel.Size = New System.Drawing.Size(113, 17)
        Me._ReadTerminationComboBoxLabel.TabIndex = 2
        Me._ReadTerminationComboBoxLabel.Text = "Read Termination:"
        '
        'SimpleReadWritePanel
        '
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me._Tabs)
        Me.Controls.Add(Me._StatusStrip)
        Me.Name = "SimpleReadWritePanel"
        Me.Size = New System.Drawing.Size(338, 393)
        Me._Tabs.ResumeLayout(False)
        Me._ResourcesTabPage.ResumeLayout(False)
        Me._ResourcesTabPage.PerformLayout()
        Me._ReadWriteTabPage.ResumeLayout(False)
        Me._SettingsTabPage.ResumeLayout(False)
        Me._SettingsLayout.ResumeLayout(False)
        Me._SettingsPanel.ResumeLayout(False)
        Me._SettingsPanel.PerformLayout()
        Me._TerminationGroupBox.ResumeLayout(False)
        Me._TerminationGroupBox.PerformLayout()
        Me._PollSettingsGroupBox.ResumeLayout(False)
        Me._PollSettingsGroupBox.PerformLayout()
        CType(Me._MessageStatusBitValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._PollMillisecondsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._TimeoutSelector, System.ComponentModel.ISupportInitialize).EndInit()
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._StatusStrip.ResumeLayout(False)
        Me._StatusStrip.PerformLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _TraceMessagesBox As isr.Core.Pith.TraceMessagesBox
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _ResourcesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _StatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _SettingsTabPage As Windows.Forms.TabPage
    Private WithEvents _SettingsLayout As Windows.Forms.TableLayoutPanel
    Private WithEvents _SettingsPanel As Windows.Forms.Panel
    Private WithEvents _TerminationGroupBox As Windows.Forms.GroupBox
    Private WithEvents _AppendNewLineCheckBox As Windows.Forms.CheckBox
    Private WithEvents _AppendReturnCheckBox As Windows.Forms.CheckBox
    Private WithEvents _PollSettingsGroupBox As Windows.Forms.GroupBox
    Private WithEvents _MessageStatusBitValueNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _MessageStatusBitsNumericLabel As Windows.Forms.Label
    Private WithEvents _PollMillisecondsNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _PollMillisecondsNumericLabel As Windows.Forms.Label
    Private WithEvents _AutoReadCheckBox As Windows.Forms.CheckBox
    Private WithEvents _PollEnableCheckBox As Windows.Forms.CheckBox
    Private WithEvents _TimeoutSelectorLabel As Windows.Forms.Label
    Private WithEvents _TimeoutSelector As Windows.Forms.NumericUpDown
    Private WithEvents _OpenSessionButton As Windows.Forms.Button
    Private WithEvents _CloseSessionButton As Windows.Forms.Button
    Private WithEvents _ServiceRequestStatusLabel As Windows.Forms.ToolStripStatusLabel
    Private WithEvents _PollTimer As Windows.Forms.Timer
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As SimpleReadWriteControl
    Private WithEvents _ErrorProvider As Windows.Forms.ErrorProvider
    Private WithEvents _SessionInfoTextBox As Windows.Forms.TextBox
    Private WithEvents _ReadTerminationComboBoxLabel As Windows.Forms.Label
    Private WithEvents _ReadTerminationComboBox As Windows.Forms.ComboBox
End Class
