<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class K7000Panel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._SlotTabPage = New System.Windows.Forms.TabPage()
        Me._UpdateSlotConfigurationButton = New System.Windows.Forms.Button()
        Me._ReadSlotConfigurationButton = New System.Windows.Forms.Button()
        Me._CardTypeTextBox = New System.Windows.Forms.TextBox()
        Me._SlotNumberComboBoxLabel = New System.Windows.Forms.Label()
        Me._SlotNumberComboBox = New System.Windows.Forms.ComboBox()
        Me._CardTypeTextBoxLabel = New System.Windows.Forms.Label()
        Me._SettlingTimeTextBoxLabel = New System.Windows.Forms.Label()
        Me._SettlingTimeTextBoxTextBox = New System.Windows.Forms.TextBox()
        Me._ChannelTabPage = New System.Windows.Forms.TabPage()
        Me._ChannelListBuilderGroupBox = New System.Windows.Forms.GroupBox()
        Me._RelayNumberTextBoxLabel = New System.Windows.Forms.Label()
        Me._RelayNumberTextBox = New System.Windows.Forms.TextBox()
        Me._SlotNumberTextBoxLabel = New System.Windows.Forms.Label()
        Me._SlotNumberTextBox = New System.Windows.Forms.TextBox()
        Me._MemoryLocationChannelItemTextBoxLabel = New System.Windows.Forms.Label()
        Me._MemoryLocationChannelItemTextBox = New System.Windows.Forms.TextBox()
        Me._AddMemoryLocationButton = New System.Windows.Forms.Button()
        Me._ClearChannelListButton = New System.Windows.Forms.Button()
        Me._AddChannelToList = New System.Windows.Forms.Button()
        Me._MemoryLocationTextBox = New System.Windows.Forms.TextBox()
        Me._SaveToMemoryButton = New System.Windows.Forms.Button()
        Me._ChannelOpenButton = New System.Windows.Forms.Button()
        Me._ChannelCloseButton = New System.Windows.Forms.Button()
        Me._ChannelListComboBox = New System.Windows.Forms.ComboBox()
        Me._ChannelListComboBoxLabel = New System.Windows.Forms.Label()
        Me._OpenAllChannelsButton = New System.Windows.Forms.Button()
        Me._ScanTabPage = New System.Windows.Forms.TabPage()
        Me._UpdateScanListButton = New System.Windows.Forms.Button()
        Me._ScanTextBox = New System.Windows.Forms.TextBox()
        Me._StepButton = New System.Windows.Forms.Button()
        Me._ScanListComboBox = New System.Windows.Forms.ComboBox()
        Me._ScanListComboBoxLabel = New System.Windows.Forms.Label()
        Me._ServiceRequestTabPage = New System.Windows.Forms.TabPage()
        Me._EnableEndOfSettlingRequestToggle = New System.Windows.Forms.CheckBox()
        Me._ServiceRequestRegisterGroupBox = New System.Windows.Forms.GroupBox()
        Me._ServiceRequestMaskRemoveButton = New System.Windows.Forms.Button()
        Me._ServiceRequestMaskAddButton = New System.Windows.Forms.Button()
        Me._ServiceRequestFlagsComboBox = New System.Windows.Forms.ComboBox()
        Me._EnableServiceRequestToggle = New System.Windows.Forms.CheckBox()
        Me._ServiceRequestByteTextBoxLabel = New System.Windows.Forms.Label()
        Me._ServiceRequestMaskTextBox = New System.Windows.Forms.TextBox()
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
        Me._Layout = New System.Windows.Forms.TableLayoutPanel()
        Me._TitleLabel = New System.Windows.Forms.Label()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Tabs.SuspendLayout()
        Me._SlotTabPage.SuspendLayout()
        Me._ChannelTabPage.SuspendLayout()
        Me._ChannelListBuilderGroupBox.SuspendLayout()
        Me._ScanTabPage.SuspendLayout()
        Me._ServiceRequestTabPage.SuspendLayout()
        Me._ServiceRequestRegisterGroupBox.SuspendLayout()
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
        Me._Tabs.Controls.Add(Me._SlotTabPage)
        Me._Tabs.Controls.Add(Me._ChannelTabPage)
        Me._Tabs.Controls.Add(Me._ScanTabPage)
        Me._Tabs.Controls.Add(Me._ServiceRequestTabPage)
        Me._Tabs.Controls.Add(Me._ResetTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(40, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 71)
        Me._Tabs.Multiline = True
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.Padding = New System.Drawing.Point(3, 3)
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 300)
        Me._Tabs.TabIndex = 3
        '
        '_SlotTabPage
        '
        Me._SlotTabPage.Controls.Add(Me._UpdateSlotConfigurationButton)
        Me._SlotTabPage.Controls.Add(Me._ReadSlotConfigurationButton)
        Me._SlotTabPage.Controls.Add(Me._CardTypeTextBox)
        Me._SlotTabPage.Controls.Add(Me._SlotNumberComboBoxLabel)
        Me._SlotTabPage.Controls.Add(Me._SlotNumberComboBox)
        Me._SlotTabPage.Controls.Add(Me._CardTypeTextBoxLabel)
        Me._SlotTabPage.Controls.Add(Me._SettlingTimeTextBoxLabel)
        Me._SlotTabPage.Controls.Add(Me._SettlingTimeTextBoxTextBox)
        Me._SlotTabPage.Location = New System.Drawing.Point(4, 26)
        Me._SlotTabPage.Name = "_SlotTabPage"
        Me._SlotTabPage.Size = New System.Drawing.Size(356, 270)
        Me._SlotTabPage.TabIndex = 0
        Me._SlotTabPage.Text = "Slot"
        Me._SlotTabPage.UseVisualStyleBackColor = True
        '
        '_UpdateSlotConfigurationButton
        '
        Me._UpdateSlotConfigurationButton.Location = New System.Drawing.Point(242, 175)
        Me._UpdateSlotConfigurationButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._UpdateSlotConfigurationButton.Name = "_UpdateSlotConfigurationButton"
        Me._UpdateSlotConfigurationButton.Size = New System.Drawing.Size(87, 30)
        Me._UpdateSlotConfigurationButton.TabIndex = 15
        Me._UpdateSlotConfigurationButton.Text = "&Apply"
        Me._UpdateSlotConfigurationButton.UseVisualStyleBackColor = True
        '
        '_ReadSlotConfigurationButton
        '
        Me._ReadSlotConfigurationButton.Location = New System.Drawing.Point(140, 175)
        Me._ReadSlotConfigurationButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ReadSlotConfigurationButton.Name = "_ReadSlotConfigurationButton"
        Me._ReadSlotConfigurationButton.Size = New System.Drawing.Size(93, 30)
        Me._ReadSlotConfigurationButton.TabIndex = 14
        Me._ReadSlotConfigurationButton.Text = "&Read"
        Me._ReadSlotConfigurationButton.UseVisualStyleBackColor = True
        '
        '_CardTypeTextBox
        '
        Me._CardTypeTextBox.Location = New System.Drawing.Point(158, 99)
        Me._CardTypeTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._CardTypeTextBox.Name = "_CardTypeTextBox"
        Me._CardTypeTextBox.Size = New System.Drawing.Size(116, 25)
        Me._CardTypeTextBox.TabIndex = 11
        Me._CardTypeTextBox.Text = "<card type>"
        '
        '_SlotNumberComboBoxLabel
        '
        Me._SlotNumberComboBoxLabel.AutoSize = True
        Me._SlotNumberComboBoxLabel.Location = New System.Drawing.Point(97, 72)
        Me._SlotNumberComboBoxLabel.Name = "_SlotNumberComboBoxLabel"
        Me._SlotNumberComboBoxLabel.Size = New System.Drawing.Size(59, 17)
        Me._SlotNumberComboBoxLabel.TabIndex = 8
        Me._SlotNumberComboBoxLabel.Text = "Number:"
        Me._SlotNumberComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SlotNumberComboBox
        '
        Me._SlotNumberComboBox.DisplayMember = "Select slot number"
        Me._SlotNumberComboBox.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me._SlotNumberComboBox.Location = New System.Drawing.Point(158, 68)
        Me._SlotNumberComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SlotNumberComboBox.Name = "_SlotNumberComboBox"
        Me._SlotNumberComboBox.Size = New System.Drawing.Size(46, 25)
        Me._SlotNumberComboBox.TabIndex = 9
        Me._SlotNumberComboBox.Text = "1"
        Me._SlotNumberComboBox.ValueMember = "Select slot number"
        '
        '_CardTypeTextBoxLabel
        '
        Me._CardTypeTextBoxLabel.AutoSize = True
        Me._CardTypeTextBoxLabel.Location = New System.Drawing.Point(86, 103)
        Me._CardTypeTextBoxLabel.Name = "_CardTypeTextBoxLabel"
        Me._CardTypeTextBoxLabel.Size = New System.Drawing.Size(70, 17)
        Me._CardTypeTextBoxLabel.TabIndex = 10
        Me._CardTypeTextBoxLabel.Text = "Card Type:"
        Me._CardTypeTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SettlingTimeTextBoxLabel
        '
        Me._SettlingTimeTextBoxLabel.AutoSize = True
        Me._SettlingTimeTextBoxLabel.Location = New System.Drawing.Point(51, 134)
        Me._SettlingTimeTextBoxLabel.Name = "_SettlingTimeTextBoxLabel"
        Me._SettlingTimeTextBoxLabel.Size = New System.Drawing.Size(105, 17)
        Me._SettlingTimeTextBoxLabel.TabIndex = 12
        Me._SettlingTimeTextBoxLabel.Text = "Settling Time [S]:"
        Me._SettlingTimeTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SettlingTimeTextBoxTextBox
        '
        Me._SettlingTimeTextBoxTextBox.Location = New System.Drawing.Point(158, 130)
        Me._SettlingTimeTextBoxTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SettlingTimeTextBoxTextBox.Name = "_SettlingTimeTextBoxTextBox"
        Me._SettlingTimeTextBoxTextBox.Size = New System.Drawing.Size(116, 25)
        Me._SettlingTimeTextBoxTextBox.TabIndex = 13
        Me._SettlingTimeTextBoxTextBox.Text = "<Settling Time>"
        '
        '_ChannelTabPage
        '
        Me._ChannelTabPage.Controls.Add(Me._ChannelListBuilderGroupBox)
        Me._ChannelTabPage.Controls.Add(Me._MemoryLocationTextBox)
        Me._ChannelTabPage.Controls.Add(Me._SaveToMemoryButton)
        Me._ChannelTabPage.Controls.Add(Me._ChannelOpenButton)
        Me._ChannelTabPage.Controls.Add(Me._ChannelCloseButton)
        Me._ChannelTabPage.Controls.Add(Me._ChannelListComboBox)
        Me._ChannelTabPage.Controls.Add(Me._ChannelListComboBoxLabel)
        Me._ChannelTabPage.Controls.Add(Me._OpenAllChannelsButton)
        Me._ChannelTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ChannelTabPage.Name = "_ChannelTabPage"
        Me._ChannelTabPage.Size = New System.Drawing.Size(356, 270)
        Me._ChannelTabPage.TabIndex = 1
        Me._ChannelTabPage.Text = "Relays"
        Me._ChannelTabPage.UseVisualStyleBackColor = True
        '
        '_ChannelListBuilderGroupBox
        '
        Me._ChannelListBuilderGroupBox.Controls.Add(Me._RelayNumberTextBoxLabel)
        Me._ChannelListBuilderGroupBox.Controls.Add(Me._RelayNumberTextBox)
        Me._ChannelListBuilderGroupBox.Controls.Add(Me._SlotNumberTextBoxLabel)
        Me._ChannelListBuilderGroupBox.Controls.Add(Me._SlotNumberTextBox)
        Me._ChannelListBuilderGroupBox.Controls.Add(Me._MemoryLocationChannelItemTextBoxLabel)
        Me._ChannelListBuilderGroupBox.Controls.Add(Me._MemoryLocationChannelItemTextBox)
        Me._ChannelListBuilderGroupBox.Controls.Add(Me._AddMemoryLocationButton)
        Me._ChannelListBuilderGroupBox.Controls.Add(Me._ClearChannelListButton)
        Me._ChannelListBuilderGroupBox.Controls.Add(Me._AddChannelToList)
        Me._ChannelListBuilderGroupBox.Location = New System.Drawing.Point(36, 12)
        Me._ChannelListBuilderGroupBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ChannelListBuilderGroupBox.Name = "_ChannelListBuilderGroupBox"
        Me._ChannelListBuilderGroupBox.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ChannelListBuilderGroupBox.Size = New System.Drawing.Size(280, 127)
        Me._ChannelListBuilderGroupBox.TabIndex = 8
        Me._ChannelListBuilderGroupBox.TabStop = False
        Me._ChannelListBuilderGroupBox.Text = "Channel List Builder"
        '
        '_RelayNumberTextBoxLabel
        '
        Me._RelayNumberTextBoxLabel.Location = New System.Drawing.Point(22, 95)
        Me._RelayNumberTextBoxLabel.Name = "_RelayNumberTextBoxLabel"
        Me._RelayNumberTextBoxLabel.Size = New System.Drawing.Size(56, 21)
        Me._RelayNumberTextBoxLabel.TabIndex = 5
        Me._RelayNumberTextBoxLabel.Text = "Relay:"
        Me._RelayNumberTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_RelayNumberTextBox
        '
        Me._RelayNumberTextBox.Location = New System.Drawing.Point(79, 93)
        Me._RelayNumberTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._RelayNumberTextBox.Name = "_RelayNumberTextBox"
        Me._RelayNumberTextBox.Size = New System.Drawing.Size(27, 25)
        Me._RelayNumberTextBox.TabIndex = 6
        Me._RelayNumberTextBox.Text = "1"
        '
        '_SlotNumberTextBoxLabel
        '
        Me._SlotNumberTextBoxLabel.Location = New System.Drawing.Point(22, 63)
        Me._SlotNumberTextBoxLabel.Name = "_SlotNumberTextBoxLabel"
        Me._SlotNumberTextBoxLabel.Size = New System.Drawing.Size(56, 21)
        Me._SlotNumberTextBoxLabel.TabIndex = 3
        Me._SlotNumberTextBoxLabel.Text = "Slot:"
        Me._SlotNumberTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SlotNumberTextBox
        '
        Me._SlotNumberTextBox.Location = New System.Drawing.Point(79, 61)
        Me._SlotNumberTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SlotNumberTextBox.Name = "_SlotNumberTextBox"
        Me._SlotNumberTextBox.Size = New System.Drawing.Size(27, 25)
        Me._SlotNumberTextBox.TabIndex = 4
        Me._SlotNumberTextBox.Text = "1"
        '
        '_MemoryLocationChannelItemTextBoxLabel
        '
        Me._MemoryLocationChannelItemTextBoxLabel.Location = New System.Drawing.Point(9, 29)
        Me._MemoryLocationChannelItemTextBoxLabel.Name = "_MemoryLocationChannelItemTextBoxLabel"
        Me._MemoryLocationChannelItemTextBoxLabel.Size = New System.Drawing.Size(60, 21)
        Me._MemoryLocationChannelItemTextBoxLabel.TabIndex = 0
        Me._MemoryLocationChannelItemTextBoxLabel.Text = "Location:"
        '
        '_MemoryLocationChannelItemTextBox
        '
        Me._MemoryLocationChannelItemTextBox.Location = New System.Drawing.Point(70, 27)
        Me._MemoryLocationChannelItemTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._MemoryLocationChannelItemTextBox.Name = "_MemoryLocationChannelItemTextBox"
        Me._MemoryLocationChannelItemTextBox.Size = New System.Drawing.Size(37, 25)
        Me._MemoryLocationChannelItemTextBox.TabIndex = 1
        Me._MemoryLocationChannelItemTextBox.Text = "1"
        '
        '_AddMemoryLocationButton
        '
        Me._AddMemoryLocationButton.Location = New System.Drawing.Point(118, 24)
        Me._AddMemoryLocationButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._AddMemoryLocationButton.Name = "_AddMemoryLocationButton"
        Me._AddMemoryLocationButton.Size = New System.Drawing.Size(149, 31)
        Me._AddMemoryLocationButton.TabIndex = 2
        Me._AddMemoryLocationButton.Text = "Add Memory Location"
        Me._AddMemoryLocationButton.UseVisualStyleBackColor = True
        '
        '_ClearChannelListButton
        '
        Me._ClearChannelListButton.Location = New System.Drawing.Point(211, 64)
        Me._ClearChannelListButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ClearChannelListButton.Name = "_ClearChannelListButton"
        Me._ClearChannelListButton.Size = New System.Drawing.Size(56, 52)
        Me._ClearChannelListButton.TabIndex = 8
        Me._ClearChannelListButton.Text = "Clear List"
        Me._ClearChannelListButton.UseVisualStyleBackColor = True
        '
        '_AddChannelToList
        '
        Me._AddChannelToList.Location = New System.Drawing.Point(118, 63)
        Me._AddChannelToList.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._AddChannelToList.Name = "_AddChannelToList"
        Me._AddChannelToList.Size = New System.Drawing.Size(75, 52)
        Me._AddChannelToList.TabIndex = 7
        Me._AddChannelToList.Text = "Add Relay to List"
        Me._AddChannelToList.UseVisualStyleBackColor = True
        '
        '_MemoryLocationTextBox
        '
        Me._MemoryLocationTextBox.Location = New System.Drawing.Point(193, 213)
        Me._MemoryLocationTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._MemoryLocationTextBox.MaxLength = 3
        Me._MemoryLocationTextBox.Name = "_MemoryLocationTextBox"
        Me._MemoryLocationTextBox.Size = New System.Drawing.Size(40, 25)
        Me._MemoryLocationTextBox.TabIndex = 14
        Me._MemoryLocationTextBox.Text = "1"
        '
        '_SaveToMemoryButton
        '
        Me._SaveToMemoryButton.Location = New System.Drawing.Point(7, 210)
        Me._SaveToMemoryButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SaveToMemoryButton.Name = "_SaveToMemoryButton"
        Me._SaveToMemoryButton.Size = New System.Drawing.Size(186, 30)
        Me._SaveToMemoryButton.TabIndex = 13
        Me._SaveToMemoryButton.Text = "&Save To Memory Location # "
        Me._SaveToMemoryButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._SaveToMemoryButton.UseVisualStyleBackColor = True
        '
        '_ChannelOpenButton
        '
        Me._ChannelOpenButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ChannelOpenButton.Location = New System.Drawing.Point(210, 147)
        Me._ChannelOpenButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ChannelOpenButton.Name = "_ChannelOpenButton"
        Me._ChannelOpenButton.Size = New System.Drawing.Size(56, 30)
        Me._ChannelOpenButton.TabIndex = 12
        Me._ChannelOpenButton.Text = "&Open"
        Me._ChannelOpenButton.UseVisualStyleBackColor = True
        '
        '_ChannelCloseButton
        '
        Me._ChannelCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ChannelCloseButton.Location = New System.Drawing.Point(148, 147)
        Me._ChannelCloseButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ChannelCloseButton.Name = "_ChannelCloseButton"
        Me._ChannelCloseButton.Size = New System.Drawing.Size(57, 30)
        Me._ChannelCloseButton.TabIndex = 11
        Me._ChannelCloseButton.Text = "&Close"
        Me._ChannelCloseButton.UseVisualStyleBackColor = True
        '
        '_ChannelListComboBox
        '
        Me._ChannelListComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ChannelListComboBox.Location = New System.Drawing.Point(7, 178)
        Me._ChannelListComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ChannelListComboBox.Name = "_ChannelListComboBox"
        Me._ChannelListComboBox.Size = New System.Drawing.Size(341, 25)
        Me._ChannelListComboBox.TabIndex = 10
        Me._ChannelListComboBox.Text = "(@ 4!1,5!1)"
        '
        '_ChannelListComboBoxLabel
        '
        Me._ChannelListComboBoxLabel.AutoSize = True
        Me._ChannelListComboBoxLabel.Location = New System.Drawing.Point(7, 158)
        Me._ChannelListComboBoxLabel.Name = "_ChannelListComboBoxLabel"
        Me._ChannelListComboBoxLabel.Size = New System.Drawing.Size(80, 17)
        Me._ChannelListComboBoxLabel.TabIndex = 9
        Me._ChannelListComboBoxLabel.Text = "Channel List:"
        '
        '_OpenAllChannelsButton
        '
        Me._OpenAllChannelsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._OpenAllChannelsButton.Location = New System.Drawing.Point(273, 148)
        Me._OpenAllChannelsButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._OpenAllChannelsButton.Name = "_OpenAllChannelsButton"
        Me._OpenAllChannelsButton.Size = New System.Drawing.Size(75, 30)
        Me._OpenAllChannelsButton.TabIndex = 15
        Me._OpenAllChannelsButton.Text = "Open &All"
        Me._OpenAllChannelsButton.UseVisualStyleBackColor = True
        '
        '_ScanTabPage
        '
        Me._ScanTabPage.Controls.Add(Me._UpdateScanListButton)
        Me._ScanTabPage.Controls.Add(Me._ScanTextBox)
        Me._ScanTabPage.Controls.Add(Me._StepButton)
        Me._ScanTabPage.Controls.Add(Me._ScanListComboBox)
        Me._ScanTabPage.Controls.Add(Me._ScanListComboBoxLabel)
        Me._ScanTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ScanTabPage.Name = "_ScanTabPage"
        Me._ScanTabPage.Size = New System.Drawing.Size(356, 270)
        Me._ScanTabPage.TabIndex = 4
        Me._ScanTabPage.Text = "Scan"
        Me._ScanTabPage.UseVisualStyleBackColor = True
        '
        '_UpdateScanListButton
        '
        Me._UpdateScanListButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._UpdateScanListButton.Location = New System.Drawing.Point(250, 63)
        Me._UpdateScanListButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._UpdateScanListButton.Name = "_UpdateScanListButton"
        Me._UpdateScanListButton.Size = New System.Drawing.Size(95, 30)
        Me._UpdateScanListButton.TabIndex = 9
        Me._UpdateScanListButton.Text = "&Apply"
        Me._UpdateScanListButton.UseVisualStyleBackColor = True
        '
        '_ScanTextBox
        '
        Me._ScanTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ScanTextBox.Location = New System.Drawing.Point(8, 99)
        Me._ScanTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ScanTextBox.Multiline = True
        Me._ScanTextBox.Name = "_ScanTextBox"
        Me._ScanTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._ScanTextBox.Size = New System.Drawing.Size(337, 160)
        Me._ScanTextBox.TabIndex = 8
        Me._ScanTextBox.Text = "<scan list>"
        '
        '_StepButton
        '
        Me._StepButton.Location = New System.Drawing.Point(8, 63)
        Me._StepButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._StepButton.Name = "_StepButton"
        Me._StepButton.Size = New System.Drawing.Size(95, 30)
        Me._StepButton.TabIndex = 7
        Me._StepButton.Text = "&Step"
        Me._StepButton.UseVisualStyleBackColor = True
        '
        '_ScanListComboBox
        '
        Me._ScanListComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ScanListComboBox.Location = New System.Drawing.Point(8, 32)
        Me._ScanListComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ScanListComboBox.Name = "_ScanListComboBox"
        Me._ScanListComboBox.Size = New System.Drawing.Size(337, 25)
        Me._ScanListComboBox.TabIndex = 6
        Me._ScanListComboBox.Text = "(@ M1,M2)"
        '
        '_ScanListComboBoxLabel
        '
        Me._ScanListComboBoxLabel.AutoSize = True
        Me._ScanListComboBoxLabel.Location = New System.Drawing.Point(11, 13)
        Me._ScanListComboBoxLabel.Name = "_ScanListComboBoxLabel"
        Me._ScanListComboBoxLabel.Size = New System.Drawing.Size(61, 17)
        Me._ScanListComboBoxLabel.TabIndex = 5
        Me._ScanListComboBoxLabel.Text = "Scan List:"
        '
        '_ServiceRequestTabPage
        '
        Me._ServiceRequestTabPage.Controls.Add(Me._EnableEndOfSettlingRequestToggle)
        Me._ServiceRequestTabPage.Controls.Add(Me._ServiceRequestRegisterGroupBox)
        Me._ServiceRequestTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ServiceRequestTabPage.Name = "_ServiceRequestTabPage"
        Me._ServiceRequestTabPage.Size = New System.Drawing.Size(356, 270)
        Me._ServiceRequestTabPage.TabIndex = 5
        Me._ServiceRequestTabPage.Text = "SRQ"
        Me._ServiceRequestTabPage.UseVisualStyleBackColor = True
        '
        '_EnableEndOfSettlingRequestToggle
        '
        Me._EnableEndOfSettlingRequestToggle.AutoSize = True
        Me._EnableEndOfSettlingRequestToggle.Location = New System.Drawing.Point(14, 177)
        Me._EnableEndOfSettlingRequestToggle.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._EnableEndOfSettlingRequestToggle.Name = "_EnableEndOfSettlingRequestToggle"
        Me._EnableEndOfSettlingRequestToggle.Size = New System.Drawing.Size(206, 21)
        Me._EnableEndOfSettlingRequestToggle.TabIndex = 6
        Me._EnableEndOfSettlingRequestToggle.Text = "Enable End of Settling Request"
        Me._EnableEndOfSettlingRequestToggle.UseVisualStyleBackColor = True
        '
        '_ServiceRequestRegisterGroupBox
        '
        Me._ServiceRequestRegisterGroupBox.Controls.Add(Me._ServiceRequestMaskRemoveButton)
        Me._ServiceRequestRegisterGroupBox.Controls.Add(Me._ServiceRequestMaskAddButton)
        Me._ServiceRequestRegisterGroupBox.Controls.Add(Me._ServiceRequestFlagsComboBox)
        Me._ServiceRequestRegisterGroupBox.Controls.Add(Me._EnableServiceRequestToggle)
        Me._ServiceRequestRegisterGroupBox.Controls.Add(Me._ServiceRequestByteTextBoxLabel)
        Me._ServiceRequestRegisterGroupBox.Controls.Add(Me._ServiceRequestMaskTextBox)
        Me._ServiceRequestRegisterGroupBox.Location = New System.Drawing.Point(10, 54)
        Me._ServiceRequestRegisterGroupBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ServiceRequestRegisterGroupBox.Name = "_ServiceRequestRegisterGroupBox"
        Me._ServiceRequestRegisterGroupBox.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ServiceRequestRegisterGroupBox.Size = New System.Drawing.Size(336, 115)
        Me._ServiceRequestRegisterGroupBox.TabIndex = 5
        Me._ServiceRequestRegisterGroupBox.TabStop = False
        Me._ServiceRequestRegisterGroupBox.Text = "Service Request Register"
        '
        '_ServiceRequestMaskRemoveButton
        '
        Me._ServiceRequestMaskRemoveButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ServiceRequestMaskRemoveButton.Location = New System.Drawing.Point(259, 73)
        Me._ServiceRequestMaskRemoveButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ServiceRequestMaskRemoveButton.Name = "_ServiceRequestMaskRemoveButton"
        Me._ServiceRequestMaskRemoveButton.Size = New System.Drawing.Size(28, 25)
        Me._ServiceRequestMaskRemoveButton.TabIndex = 5
        Me._ServiceRequestMaskRemoveButton.Text = "-"
        Me._ServiceRequestMaskRemoveButton.UseVisualStyleBackColor = True
        '
        '_ServiceRequestMaskAddButton
        '
        Me._ServiceRequestMaskAddButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ServiceRequestMaskAddButton.Location = New System.Drawing.Point(230, 73)
        Me._ServiceRequestMaskAddButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ServiceRequestMaskAddButton.Name = "_ServiceRequestMaskAddButton"
        Me._ServiceRequestMaskAddButton.Size = New System.Drawing.Size(28, 25)
        Me._ServiceRequestMaskAddButton.TabIndex = 4
        Me._ServiceRequestMaskAddButton.Text = "+"
        Me._ServiceRequestMaskAddButton.UseVisualStyleBackColor = True
        '
        '_ServiceRequestFlagsComboBox
        '
        Me._ServiceRequestFlagsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._ServiceRequestFlagsComboBox.Location = New System.Drawing.Point(10, 73)
        Me._ServiceRequestFlagsComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ServiceRequestFlagsComboBox.Name = "_ServiceRequestFlagsComboBox"
        Me._ServiceRequestFlagsComboBox.Size = New System.Drawing.Size(217, 25)
        Me._ServiceRequestFlagsComboBox.TabIndex = 3
        '
        '_EnableServiceRequestToggle
        '
        Me._EnableServiceRequestToggle.AutoSize = True
        Me._EnableServiceRequestToggle.Location = New System.Drawing.Point(10, 25)
        Me._EnableServiceRequestToggle.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._EnableServiceRequestToggle.Name = "_EnableServiceRequestToggle"
        Me._EnableServiceRequestToggle.Size = New System.Drawing.Size(162, 21)
        Me._EnableServiceRequestToggle.TabIndex = 0
        Me._EnableServiceRequestToggle.Text = "Enable Service Request"
        Me._EnableServiceRequestToggle.UseVisualStyleBackColor = True
        '
        '_ServiceRequestByteTextBoxLabel
        '
        Me._ServiceRequestByteTextBoxLabel.Location = New System.Drawing.Point(9, 52)
        Me._ServiceRequestByteTextBoxLabel.Name = "_ServiceRequestByteTextBoxLabel"
        Me._ServiceRequestByteTextBoxLabel.Size = New System.Drawing.Size(140, 21)
        Me._ServiceRequestByteTextBoxLabel.TabIndex = 1
        Me._ServiceRequestByteTextBoxLabel.Text = "Service Request Mask: "
        '
        '_ServiceRequestMaskTextBox
        '
        Me._ServiceRequestMaskTextBox.Location = New System.Drawing.Point(288, 73)
        Me._ServiceRequestMaskTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ServiceRequestMaskTextBox.MaxLength = 3
        Me._ServiceRequestMaskTextBox.Name = "_ServiceRequestMaskTextBox"
        Me._ServiceRequestMaskTextBox.Size = New System.Drawing.Size(37, 25)
        Me._ServiceRequestMaskTextBox.TabIndex = 2
        Me._ServiceRequestMaskTextBox.Text = "239"
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
        Me._ReadingToolStripStatusLabel.Text = "1:1001"
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
        Me._TitleLabel.Text = "K7000"
        Me._TitleLabel.UseMnemonic = False
        '
        'K7000Panel
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "K7000Panel"
        Me.Size = New System.Drawing.Size(364, 450)
        Me.Controls.SetChildIndex(Me.Connector, 0)
        Me.Controls.SetChildIndex(Me._Layout, 0)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Tabs.ResumeLayout(False)
        Me._SlotTabPage.ResumeLayout(False)
        Me._SlotTabPage.PerformLayout()
        Me._ChannelTabPage.ResumeLayout(False)
        Me._ChannelTabPage.PerformLayout()
        Me._ChannelListBuilderGroupBox.ResumeLayout(False)
        Me._ChannelListBuilderGroupBox.PerformLayout()
        Me._ScanTabPage.ResumeLayout(False)
        Me._ScanTabPage.PerformLayout()
        Me._ServiceRequestTabPage.ResumeLayout(False)
        Me._ServiceRequestTabPage.PerformLayout()
        Me._ServiceRequestRegisterGroupBox.ResumeLayout(False)
        Me._ServiceRequestRegisterGroupBox.PerformLayout()
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
    Private WithEvents _SlotTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ResetTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ChannelTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ScanTabPage As System.Windows.Forms.TabPage
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
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
    Private WithEvents _LastReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TitleLabel As System.Windows.Forms.Label
    Private WithEvents _ChannelListBuilderGroupBox As Windows.Forms.GroupBox
    Private WithEvents _RelayNumberTextBoxLabel As Windows.Forms.Label
    Private WithEvents _RelayNumberTextBox As Windows.Forms.TextBox
    Private WithEvents _SlotNumberTextBoxLabel As Windows.Forms.Label
    Private WithEvents _SlotNumberTextBox As Windows.Forms.TextBox
    Private WithEvents _MemoryLocationChannelItemTextBoxLabel As Windows.Forms.Label
    Private WithEvents _MemoryLocationChannelItemTextBox As Windows.Forms.TextBox
    Private WithEvents _AddMemoryLocationButton As Windows.Forms.Button
    Private WithEvents _ClearChannelListButton As Windows.Forms.Button
    Private WithEvents _AddChannelToList As Windows.Forms.Button
    Private WithEvents _MemoryLocationTextBox As Windows.Forms.TextBox
    Private WithEvents _SaveToMemoryButton As Windows.Forms.Button
    Private WithEvents _ChannelOpenButton As Windows.Forms.Button
    Private WithEvents _ChannelCloseButton As Windows.Forms.Button
    Private WithEvents _ChannelListComboBox As Windows.Forms.ComboBox
    Private WithEvents _ChannelListComboBoxLabel As Windows.Forms.Label
    Private WithEvents _OpenAllChannelsButton As Windows.Forms.Button
    Private WithEvents _UpdateSlotConfigurationButton As Windows.Forms.Button
    Private WithEvents _ReadSlotConfigurationButton As Windows.Forms.Button
    Private WithEvents _CardTypeTextBox As Windows.Forms.TextBox
    Private WithEvents _SlotNumberComboBoxLabel As Windows.Forms.Label
    Private WithEvents _SlotNumberComboBox As Windows.Forms.ComboBox
    Private WithEvents _CardTypeTextBoxLabel As Windows.Forms.Label
    Private WithEvents _SettlingTimeTextBoxLabel As Windows.Forms.Label
    Private WithEvents _SettlingTimeTextBoxTextBox As Windows.Forms.TextBox
    Private WithEvents _UpdateScanListButton As Windows.Forms.Button
    Private WithEvents _ScanTextBox As Windows.Forms.TextBox
    Private WithEvents _StepButton As Windows.Forms.Button
    Private WithEvents _ScanListComboBox As Windows.Forms.ComboBox
    Private WithEvents _ScanListComboBoxLabel As Windows.Forms.Label
    Private WithEvents _ServiceRequestTabPage As Windows.Forms.TabPage
    Private WithEvents _EnableEndOfSettlingRequestToggle As Windows.Forms.CheckBox
    Private WithEvents _ServiceRequestRegisterGroupBox As Windows.Forms.GroupBox
    Private WithEvents _ServiceRequestMaskRemoveButton As Windows.Forms.Button
    Private WithEvents _ServiceRequestMaskAddButton As Windows.Forms.Button
    Private WithEvents _ServiceRequestFlagsComboBox As Windows.Forms.ComboBox
    Private WithEvents _EnableServiceRequestToggle As Windows.Forms.CheckBox
    Private WithEvents _ServiceRequestByteTextBoxLabel As Windows.Forms.Label
    Private WithEvents _ServiceRequestMaskTextBox As Windows.Forms.TextBox
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
End Class
