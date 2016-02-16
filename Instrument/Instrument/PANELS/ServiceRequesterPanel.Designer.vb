<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ServiceRequesterPanel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="statusPanel")> <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ResourceTabPage = New System.Windows.Forms.TabPage()
        Me._ResourceLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._ResourceSelectionInfoLabel = New System.Windows.Forms.Label()
        Me._SelectorPanel = New System.Windows.Forms.Panel()
        Me._FindButton = New System.Windows.Forms.Button()
        Me._ResourceNamesComboBox = New System.Windows.Forms.ComboBox()
        Me._OpenSessionButton = New System.Windows.Forms.Button()
        Me._ResourceNameTextBoxLabel = New System.Windows.Forms.Label()
        Me._ConfigureTabPage = New System.Windows.Forms.TabPage()
        Me._ConfiguringLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._ConfiguringGroupBox = New System.Windows.Forms.GroupBox()
        Me._TimeoutSelector = New System.Windows.Forms.NumericUpDown()
        Me._TimeoutSelectorLabel = New System.Windows.Forms.Label()
        Me._SyncCallBacksCheckBox = New System.Windows.Forms.CheckBox()
        Me._EnableServiceRequestButton = New System.Windows.Forms.Button()
        Me._CommandTextBox = New System.Windows.Forms.TextBox()
        Me._CommandInfoLabel = New System.Windows.Forms.Label()
        Me._ReadWriteTabPage = New System.Windows.Forms.TabPage()
        Me._ReadWriteLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._WritingGroupBox = New System.Windows.Forms.GroupBox()
        Me._WriteButton = New System.Windows.Forms.Button()
        Me._WriteTextBox = New System.Windows.Forms.TextBox()
        Me._ReadingGroupBox = New System.Windows.Forms.GroupBox()
        Me._MessageStatusBitValueNumeric = New System.Windows.Forms.NumericUpDown()
        Me._MessageStatusBitsNumericLabel = New System.Windows.Forms.Label()
        Me._ElapsedTimeTextBoxUnitsLabel = New System.Windows.Forms.Label()
        Me._ElapsedTimeTextBox = New System.Windows.Forms.TextBox()
        Me._ClearButton = New System.Windows.Forms.Button()
        Me._ReadTextBox = New System.Windows.Forms.TextBox()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._TraceMessagesBox = New isr.Core.Pith.TraceMessagesBox()
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._StatusStrip = New System.Windows.Forms.StatusStrip()
        Me._StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._ServiceRequestStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._PollTimer = New System.Windows.Forms.Timer(Me.components)
        Me._Tabs.SuspendLayout()
        Me._ResourceTabPage.SuspendLayout()
        Me._ResourceLayout.SuspendLayout()
        Me._SelectorPanel.SuspendLayout()
        Me._ConfigureTabPage.SuspendLayout()
        Me._ConfiguringLayout.SuspendLayout()
        Me._ConfiguringGroupBox.SuspendLayout()
        CType(Me._TimeoutSelector, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ReadWriteTabPage.SuspendLayout()
        Me._ReadWriteLayout.SuspendLayout()
        Me._WritingGroupBox.SuspendLayout()
        Me._ReadingGroupBox.SuspendLayout()
        CType(Me._MessageStatusBitValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._MessagesTabPage.SuspendLayout()
        Me._StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ResourceTabPage)
        Me._Tabs.Controls.Add(Me._ConfigureTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.Location = New System.Drawing.Point(0, 0)
        Me._Tabs.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(338, 371)
        Me._Tabs.TabIndex = 12
        '
        '_ResourceTabPage
        '
        Me._ResourceTabPage.Controls.Add(Me._ResourceLayout)
        Me._ResourceTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ResourceTabPage.Name = "_ResourceTabPage"
        Me._ResourceTabPage.Size = New System.Drawing.Size(330, 341)
        Me._ResourceTabPage.TabIndex = 3
        Me._ResourceTabPage.Text = "Resource"
        Me._ResourceTabPage.UseVisualStyleBackColor = True
        '
        '_ResourceLayout
        '
        Me._ResourceLayout.ColumnCount = 3
        Me._ResourceLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ResourceLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ResourceLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ResourceLayout.Controls.Add(Me._ResourceSelectionInfoLabel, 1, 1)
        Me._ResourceLayout.Controls.Add(Me._SelectorPanel, 1, 2)
        Me._ResourceLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ResourceLayout.Location = New System.Drawing.Point(0, 0)
        Me._ResourceLayout.Name = "_ResourceLayout"
        Me._ResourceLayout.RowCount = 4
        Me._ResourceLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ResourceLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ResourceLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ResourceLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ResourceLayout.Size = New System.Drawing.Size(330, 341)
        Me._ResourceLayout.TabIndex = 2
        '
        '_ResourceSelectionInfoLabel
        '
        Me._ResourceSelectionInfoLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me._ResourceSelectionInfoLabel.Location = New System.Drawing.Point(15, 76)
        Me._ResourceSelectionInfoLabel.Name = "_ResourceSelectionInfoLabel"
        Me._ResourceSelectionInfoLabel.Size = New System.Drawing.Size(300, 75)
        Me._ResourceSelectionInfoLabel.TabIndex = 1
        Me._ResourceSelectionInfoLabel.Text = "Select the Resource Name associated with your device and press the Configure Devi" &
    "ce button. Then enter the command string that enables SRQ and click the Enable S" &
    "RQ button."
        '
        '_SelectorPanel
        '
        Me._SelectorPanel.Controls.Add(Me._FindButton)
        Me._SelectorPanel.Controls.Add(Me._ResourceNamesComboBox)
        Me._SelectorPanel.Controls.Add(Me._OpenSessionButton)
        Me._SelectorPanel.Controls.Add(Me._ResourceNameTextBoxLabel)
        Me._SelectorPanel.Location = New System.Drawing.Point(15, 154)
        Me._SelectorPanel.Name = "_SelectorPanel"
        Me._SelectorPanel.Size = New System.Drawing.Size(300, 107)
        Me._SelectorPanel.TabIndex = 2
        '
        '_FindButton
        '
        Me._FindButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._FindButton.Location = New System.Drawing.Point(169, 6)
        Me._FindButton.Name = "_FindButton"
        Me._FindButton.Size = New System.Drawing.Size(121, 28)
        Me._FindButton.TabIndex = 5
        Me._FindButton.Text = "Find Resources"
        Me._FindButton.UseVisualStyleBackColor = True
        '
        '_ResourceNamesComboBox
        '
        Me._ResourceNamesComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ResourceNamesComboBox.FormattingEnabled = True
        Me._ResourceNamesComboBox.Location = New System.Drawing.Point(8, 38)
        Me._ResourceNamesComboBox.Name = "_ResourceNamesComboBox"
        Me._ResourceNamesComboBox.Size = New System.Drawing.Size(282, 25)
        Me._ResourceNamesComboBox.TabIndex = 6
        '
        '_OpenSessionButton
        '
        Me._OpenSessionButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._OpenSessionButton.Location = New System.Drawing.Point(210, 68)
        Me._OpenSessionButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._OpenSessionButton.Name = "_OpenSessionButton"
        Me._OpenSessionButton.Size = New System.Drawing.Size(80, 31)
        Me._OpenSessionButton.TabIndex = 7
        Me._OpenSessionButton.Text = "Open"
        '
        '_ResourceNameTextBoxLabel
        '
        Me._ResourceNameTextBoxLabel.AutoSize = True
        Me._ResourceNameTextBoxLabel.Location = New System.Drawing.Point(8, 17)
        Me._ResourceNameTextBoxLabel.Name = "_ResourceNameTextBoxLabel"
        Me._ResourceNameTextBoxLabel.Size = New System.Drawing.Size(104, 17)
        Me._ResourceNameTextBoxLabel.TabIndex = 4
        Me._ResourceNameTextBoxLabel.Text = "Resource Name:"
        '
        '_ConfigureTabPage
        '
        Me._ConfigureTabPage.Controls.Add(Me._ConfiguringLayout)
        Me._ConfigureTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ConfigureTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ConfigureTabPage.Name = "_ConfigureTabPage"
        Me._ConfigureTabPage.Size = New System.Drawing.Size(330, 341)
        Me._ConfigureTabPage.TabIndex = 0
        Me._ConfigureTabPage.Text = "Configure"
        Me._ConfigureTabPage.UseVisualStyleBackColor = True
        '
        '_ConfiguringLayout
        '
        Me._ConfiguringLayout.ColumnCount = 3
        Me._ConfiguringLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConfiguringLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ConfiguringLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConfiguringLayout.Controls.Add(Me._ConfiguringGroupBox, 1, 1)
        Me._ConfiguringLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ConfiguringLayout.Location = New System.Drawing.Point(0, 0)
        Me._ConfiguringLayout.Name = "_ConfiguringLayout"
        Me._ConfiguringLayout.RowCount = 3
        Me._ConfiguringLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConfiguringLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ConfiguringLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConfiguringLayout.Size = New System.Drawing.Size(330, 341)
        Me._ConfiguringLayout.TabIndex = 0
        '
        '_ConfiguringGroupBox
        '
        Me._ConfiguringGroupBox.Controls.Add(Me._TimeoutSelector)
        Me._ConfiguringGroupBox.Controls.Add(Me._TimeoutSelectorLabel)
        Me._ConfiguringGroupBox.Controls.Add(Me._SyncCallBacksCheckBox)
        Me._ConfiguringGroupBox.Controls.Add(Me._EnableServiceRequestButton)
        Me._ConfiguringGroupBox.Controls.Add(Me._CommandTextBox)
        Me._ConfiguringGroupBox.Controls.Add(Me._CommandInfoLabel)
        Me._ConfiguringGroupBox.Location = New System.Drawing.Point(12, 60)
        Me._ConfiguringGroupBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ConfiguringGroupBox.Name = "_ConfiguringGroupBox"
        Me._ConfiguringGroupBox.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ConfiguringGroupBox.Size = New System.Drawing.Size(305, 220)
        Me._ConfiguringGroupBox.TabIndex = 2
        Me._ConfiguringGroupBox.TabStop = False
        Me._ConfiguringGroupBox.Text = "Configuring"
        '
        '_TimeoutSelector
        '
        Me._TimeoutSelector.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._TimeoutSelector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TimeoutSelector.Location = New System.Drawing.Point(220, 21)
        Me._TimeoutSelector.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._TimeoutSelector.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me._TimeoutSelector.Name = "_TimeoutSelector"
        Me._TimeoutSelector.Size = New System.Drawing.Size(75, 25)
        Me._TimeoutSelector.TabIndex = 6
        Me._TimeoutSelector.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        '_TimeoutSelectorLabel
        '
        Me._TimeoutSelectorLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._TimeoutSelectorLabel.AutoSize = True
        Me._TimeoutSelectorLabel.Location = New System.Drawing.Point(131, 25)
        Me._TimeoutSelectorLabel.Name = "_TimeoutSelectorLabel"
        Me._TimeoutSelectorLabel.Size = New System.Drawing.Size(87, 17)
        Me._TimeoutSelectorLabel.TabIndex = 5
        Me._TimeoutSelectorLabel.Text = "Timeout [ms]:"
        '
        '_SyncCallBacksCheckBox
        '
        Me._SyncCallBacksCheckBox.AutoSize = True
        Me._SyncCallBacksCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._SyncCallBacksCheckBox.Location = New System.Drawing.Point(4, 25)
        Me._SyncCallBacksCheckBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SyncCallBacksCheckBox.Name = "_SyncCallBacksCheckBox"
        Me._SyncCallBacksCheckBox.Size = New System.Drawing.Size(114, 21)
        Me._SyncCallBacksCheckBox.TabIndex = 4
        Me._SyncCallBacksCheckBox.Text = "Sync Callbacks:"
        Me._SyncCallBacksCheckBox.UseVisualStyleBackColor = True
        '
        '_EnableServiceRequestButton
        '
        Me._EnableServiceRequestButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._EnableServiceRequestButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._EnableServiceRequestButton.Location = New System.Drawing.Point(194, 174)
        Me._EnableServiceRequestButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._EnableServiceRequestButton.Name = "_EnableServiceRequestButton"
        Me._EnableServiceRequestButton.Size = New System.Drawing.Size(102, 31)
        Me._EnableServiceRequestButton.TabIndex = 9
        Me._EnableServiceRequestButton.Text = "Enable SRQ"
        '
        '_CommandTextBox
        '
        Me._CommandTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._CommandTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._CommandTextBox.Location = New System.Drawing.Point(9, 177)
        Me._CommandTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._CommandTextBox.Name = "_CommandTextBox"
        Me._CommandTextBox.Size = New System.Drawing.Size(181, 25)
        Me._CommandTextBox.TabIndex = 8
        Me._CommandTextBox.Text = "*SRE 16\n"
        '
        '_CommandInfoLabel
        '
        Me._CommandInfoLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._CommandInfoLabel.Location = New System.Drawing.Point(9, 67)
        Me._CommandInfoLabel.Name = "_CommandInfoLabel"
        Me._CommandInfoLabel.Size = New System.Drawing.Size(287, 86)
        Me._CommandInfoLabel.TabIndex = 7
        Me._CommandInfoLabel.Text = "Type the command to enable the instrument's SRQ event on MAV or clear this field " &
    "if the instrument service request is already enabled and no command is required " &
    "or allowed then click Enable SRQ:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        '_ReadWriteTabPage
        '
        Me._ReadWriteTabPage.Controls.Add(Me._ReadWriteLayout)
        Me._ReadWriteTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadWriteTabPage.Name = "_ReadWriteTabPage"
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(330, 341)
        Me._ReadWriteTabPage.TabIndex = 2
        Me._ReadWriteTabPage.Text = "Read/Write"
        Me._ReadWriteTabPage.UseVisualStyleBackColor = True
        '
        '_ReadWriteLayout
        '
        Me._ReadWriteLayout.ColumnCount = 3
        Me._ReadWriteLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6.0!))
        Me._ReadWriteLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._ReadWriteLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6.0!))
        Me._ReadWriteLayout.Controls.Add(Me._WritingGroupBox, 1, 1)
        Me._ReadWriteLayout.Controls.Add(Me._ReadingGroupBox, 1, 2)
        Me._ReadWriteLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ReadWriteLayout.Location = New System.Drawing.Point(0, 0)
        Me._ReadWriteLayout.Name = "_ReadWriteLayout"
        Me._ReadWriteLayout.RowCount = 4
        Me._ReadWriteLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6.0!))
        Me._ReadWriteLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ReadWriteLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._ReadWriteLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6.0!))
        Me._ReadWriteLayout.Size = New System.Drawing.Size(330, 341)
        Me._ReadWriteLayout.TabIndex = 22
        '
        '_WritingGroupBox
        '
        Me._WritingGroupBox.Controls.Add(Me._WriteButton)
        Me._WritingGroupBox.Controls.Add(Me._WriteTextBox)
        Me._WritingGroupBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._WritingGroupBox.Location = New System.Drawing.Point(9, 10)
        Me._WritingGroupBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._WritingGroupBox.Name = "_WritingGroupBox"
        Me._WritingGroupBox.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._WritingGroupBox.Size = New System.Drawing.Size(312, 71)
        Me._WritingGroupBox.TabIndex = 3
        Me._WritingGroupBox.TabStop = False
        Me._WritingGroupBox.Text = "Writing"
        '
        '_WriteButton
        '
        Me._WriteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._WriteButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._WriteButton.Location = New System.Drawing.Point(215, 12)
        Me._WriteButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._WriteButton.Name = "_WriteButton"
        Me._WriteButton.Size = New System.Drawing.Size(93, 30)
        Me._WriteButton.TabIndex = 1
        Me._WriteButton.Text = "Write"
        '
        '_WriteTextBox
        '
        Me._WriteTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._WriteTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._WriteTextBox.Location = New System.Drawing.Point(3, 42)
        Me._WriteTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._WriteTextBox.Name = "_WriteTextBox"
        Me._WriteTextBox.Size = New System.Drawing.Size(306, 25)
        Me._WriteTextBox.TabIndex = 0
        Me._WriteTextBox.Text = "*IDN?\n"
        '
        '_ReadingGroupBox
        '
        Me._ReadingGroupBox.Controls.Add(Me._MessageStatusBitValueNumeric)
        Me._ReadingGroupBox.Controls.Add(Me._MessageStatusBitsNumericLabel)
        Me._ReadingGroupBox.Controls.Add(Me._ElapsedTimeTextBoxUnitsLabel)
        Me._ReadingGroupBox.Controls.Add(Me._ElapsedTimeTextBox)
        Me._ReadingGroupBox.Controls.Add(Me._ClearButton)
        Me._ReadingGroupBox.Controls.Add(Me._ReadTextBox)
        Me._ReadingGroupBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ReadingGroupBox.Location = New System.Drawing.Point(9, 89)
        Me._ReadingGroupBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ReadingGroupBox.Name = "_ReadingGroupBox"
        Me._ReadingGroupBox.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ReadingGroupBox.Size = New System.Drawing.Size(312, 242)
        Me._ReadingGroupBox.TabIndex = 4
        Me._ReadingGroupBox.TabStop = False
        Me._ReadingGroupBox.Text = "Reading"
        '
        '_MessageStatusBitValueNumeric
        '
        Me._MessageStatusBitValueNumeric.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._MessageStatusBitValueNumeric.Hexadecimal = True
        Me._MessageStatusBitValueNumeric.Location = New System.Drawing.Point(248, 19)
        Me._MessageStatusBitValueNumeric.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me._MessageStatusBitValueNumeric.Name = "_MessageStatusBitValueNumeric"
        Me._MessageStatusBitValueNumeric.Size = New System.Drawing.Size(51, 25)
        Me._MessageStatusBitValueNumeric.TabIndex = 1
        Me._MessageStatusBitValueNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._MessageStatusBitValueNumeric.Value = New Decimal(New Integer() {16, 0, 0, 0})
        '
        '_MessageStatusBitsNumericLabel
        '
        Me._MessageStatusBitsNumericLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._MessageStatusBitsNumericLabel.AutoSize = True
        Me._MessageStatusBitsNumericLabel.Location = New System.Drawing.Point(57, 23)
        Me._MessageStatusBitsNumericLabel.Name = "_MessageStatusBitsNumericLabel"
        Me._MessageStatusBitsNumericLabel.Size = New System.Drawing.Size(188, 17)
        Me._MessageStatusBitsNumericLabel.TabIndex = 0
        Me._MessageStatusBitsNumericLabel.Text = "Message Status Bit Value [hex]:"
        '
        '_ElapsedTimeTextBoxUnitsLabel
        '
        Me._ElapsedTimeTextBoxUnitsLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ElapsedTimeTextBoxUnitsLabel.AutoSize = True
        Me._ElapsedTimeTextBoxUnitsLabel.Location = New System.Drawing.Point(270, 206)
        Me._ElapsedTimeTextBoxUnitsLabel.Name = "_ElapsedTimeTextBoxUnitsLabel"
        Me._ElapsedTimeTextBoxUnitsLabel.Size = New System.Drawing.Size(25, 17)
        Me._ElapsedTimeTextBoxUnitsLabel.TabIndex = 7
        Me._ElapsedTimeTextBoxUnitsLabel.Text = "ms"
        '
        '_ElapsedTimeTextBox
        '
        Me._ElapsedTimeTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ElapsedTimeTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ElapsedTimeTextBox.Location = New System.Drawing.Point(195, 202)
        Me._ElapsedTimeTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ElapsedTimeTextBox.Name = "_ElapsedTimeTextBox"
        Me._ElapsedTimeTextBox.Size = New System.Drawing.Size(73, 25)
        Me._ElapsedTimeTextBox.TabIndex = 6
        '
        '_ClearButton
        '
        Me._ClearButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._ClearButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ClearButton.Location = New System.Drawing.Point(9, 200)
        Me._ClearButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ClearButton.Name = "_ClearButton"
        Me._ClearButton.Size = New System.Drawing.Size(121, 31)
        Me._ClearButton.TabIndex = 5
        Me._ClearButton.Text = "Clear"
        '
        '_ReadTextBox
        '
        Me._ReadTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ReadTextBox.Location = New System.Drawing.Point(10, 55)
        Me._ReadTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ReadTextBox.Multiline = True
        Me._ReadTextBox.Name = "_ReadTextBox"
        Me._ReadTextBox.ReadOnly = True
        Me._ReadTextBox.Size = New System.Drawing.Size(291, 135)
        Me._ReadTextBox.TabIndex = 2
        Me._ReadTextBox.Text = "If not working, an SDC or *RST or *CLS are required line in the GPIB Test Panel. " &
    " Run the GPIB Test panel and try again."
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
        Me._TraceMessagesBox.TraceLevel = System.Diagnostics.TraceEventType.Verbose
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
        'ServiceRequesterPanel
        '
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me._Tabs)
        Me.Controls.Add(Me._StatusStrip)
        Me.Name = "ServiceRequesterPanel"
        Me.Size = New System.Drawing.Size(338, 393)
        Me._Tabs.ResumeLayout(False)
        Me._ResourceTabPage.ResumeLayout(False)
        Me._ResourceLayout.ResumeLayout(False)
        Me._SelectorPanel.ResumeLayout(False)
        Me._SelectorPanel.PerformLayout()
        Me._ConfigureTabPage.ResumeLayout(False)
        Me._ConfiguringLayout.ResumeLayout(False)
        Me._ConfiguringGroupBox.ResumeLayout(False)
        Me._ConfiguringGroupBox.PerformLayout()
        CType(Me._TimeoutSelector, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ReadWriteTabPage.ResumeLayout(False)
        Me._ReadWriteLayout.ResumeLayout(False)
        Me._WritingGroupBox.ResumeLayout(False)
        Me._WritingGroupBox.PerformLayout()
        Me._ReadingGroupBox.ResumeLayout(False)
        Me._ReadingGroupBox.PerformLayout()
        CType(Me._MessageStatusBitValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._StatusStrip.ResumeLayout(False)
        Me._StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _TraceMessagesBox As isr.Core.Pith.TraceMessagesBox
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _ConfigureTabPage As System.Windows.Forms.TabPage
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _StatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _ReadWriteLayout As Windows.Forms.TableLayoutPanel
    Private WithEvents _ServiceRequestStatusLabel As Windows.Forms.ToolStripStatusLabel
    Private WithEvents _PollTimer As Windows.Forms.Timer
    Private WithEvents _ConfiguringLayout As Windows.Forms.TableLayoutPanel
    Private WithEvents _ConfiguringGroupBox As Windows.Forms.GroupBox
    Private WithEvents _SyncCallBacksCheckBox As Windows.Forms.CheckBox
    Private WithEvents _EnableServiceRequestButton As Windows.Forms.Button
    Private WithEvents _CommandTextBox As Windows.Forms.TextBox
    Private WithEvents _CommandInfoLabel As Windows.Forms.Label
    Private WithEvents _ResourceTabPage As Windows.Forms.TabPage
    Private WithEvents _TimeoutSelectorLabel As Windows.Forms.Label
    Private WithEvents _TimeoutSelector As Windows.Forms.NumericUpDown
    Private WithEvents _ResourceSelectionInfoLabel As Windows.Forms.Label
    Private WithEvents _WritingGroupBox As Windows.Forms.GroupBox
    Private WithEvents _WriteButton As Windows.Forms.Button
    Private WithEvents _WriteTextBox As Windows.Forms.TextBox
    Private WithEvents _ReadingGroupBox As Windows.Forms.GroupBox
    Private WithEvents _MessageStatusBitValueNumeric As Windows.Forms.NumericUpDown
    Private WithEvents _MessageStatusBitsNumericLabel As Windows.Forms.Label
    Private WithEvents _ElapsedTimeTextBoxUnitsLabel As Windows.Forms.Label
    Private WithEvents _ElapsedTimeTextBox As Windows.Forms.TextBox
    Private WithEvents _ClearButton As Windows.Forms.Button
    Private WithEvents _ReadTextBox As Windows.Forms.TextBox
    Private WithEvents _ResourceLayout As Windows.Forms.TableLayoutPanel
    Private WithEvents _SelectorPanel As Windows.Forms.Panel
    Private WithEvents _ResourceNameTextBoxLabel As Windows.Forms.Label
    Private WithEvents _OpenSessionButton As Windows.Forms.Button
    Private WithEvents _ResourceNamesComboBox As Windows.Forms.ComboBox
    Private WithEvents _FindButton As Windows.Forms.Button
End Class
