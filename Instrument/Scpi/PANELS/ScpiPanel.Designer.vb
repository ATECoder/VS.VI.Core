Imports isr.VI.Instrument
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ScpiPanel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
        Me._ModalityComboBoxLabel = New System.Windows.Forms.Label()
        Me._TerminalsToggle = New System.Windows.Forms.CheckBox()
        Me._ModalityComboBox = New System.Windows.Forms.ComboBox()
        Me._ReadButton = New System.Windows.Forms.Button()
        Me._InitiateButton = New System.Windows.Forms.Button()
        Me._interfaceTabPage = New System.Windows.Forms.TabPage()
        Me._ResetButton = New System.Windows.Forms.Button()
        Me._InterfaceClearButton = New System.Windows.Forms.Button()
        Me._channelTabPage = New System.Windows.Forms.TabPage()
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
        Me._SimpleReadWriteControl = New SimpleReadWriteControl()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._ReadingTextBox = New System.Windows.Forms.TextBox()
        Me._TitleLabel = New System.Windows.Forms.Label()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        Me._interfaceTabPage.SuspendLayout()
        Me._channelTabPage.SuspendLayout()
        Me._SenseTabPage.SuspendLayout()
        Me._ReadWriteTabPage.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
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
        Me.TraceMessagesBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TraceMessagesBox.PresetCount = 50
        Me.TraceMessagesBox.ResetCount = 100
        Me.TraceMessagesBox.Size = New System.Drawing.Size(356, 295)
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ReadingTabPage)
        Me._Tabs.Controls.Add(Me._interfaceTabPage)
        Me._Tabs.Controls.Add(Me._channelTabPage)
        Me._Tabs.Controls.Add(Me._SenseTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(52, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 60)
        Me._Tabs.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._Tabs.Multiline = True
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 308)
        Me._Tabs.TabIndex = 15
        '
        '_ReadingTabPage
        '
        Me._ReadingTabPage.Controls.Add(Me._ModalityComboBoxLabel)
        Me._ReadingTabPage.Controls.Add(Me._TerminalsToggle)
        Me._ReadingTabPage.Controls.Add(Me._ModalityComboBox)
        Me._ReadingTabPage.Controls.Add(Me._ReadButton)
        Me._ReadingTabPage.Controls.Add(Me._InitiateButton)
        Me._ReadingTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadingTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ReadingTabPage.Name = "_ReadingTabPage"
        Me._ReadingTabPage.Size = New System.Drawing.Size(356, 278)
        Me._ReadingTabPage.TabIndex = 0
        Me._ReadingTabPage.Text = "Reading"
        Me._ReadingTabPage.UseVisualStyleBackColor = True
        '
        '_ModalityComboBoxLabel
        '
        Me._ModalityComboBoxLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ModalityComboBoxLabel.AutoSize = True
        Me._ModalityComboBoxLabel.Location = New System.Drawing.Point(186, 38)
        Me._ModalityComboBoxLabel.Name = "_ModalityComboBoxLabel"
        Me._ModalityComboBoxLabel.Size = New System.Drawing.Size(66, 17)
        Me._ModalityComboBoxLabel.TabIndex = 4
        Me._ModalityComboBoxLabel.Text = "Modality: "
        '
        '_TerminalsToggle
        '
        Me._TerminalsToggle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._TerminalsToggle.Appearance = System.Windows.Forms.Appearance.Button
        Me._TerminalsToggle.Enabled = False
        Me._TerminalsToggle.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._TerminalsToggle.Location = New System.Drawing.Point(287, 12)
        Me._TerminalsToggle.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._TerminalsToggle.Name = "_TerminalsToggle"
        Me._TerminalsToggle.Size = New System.Drawing.Size(58, 30)
        Me._TerminalsToggle.TabIndex = 3
        Me._TerminalsToggle.Text = "REAR"
        Me._TerminalsToggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me._TerminalsToggle.UseVisualStyleBackColor = True
        '
        '_ModalityComboBox
        '
        Me._ModalityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._ModalityComboBox.Location = New System.Drawing.Point(188, 57)
        Me._ModalityComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ModalityComboBox.Name = "_ModalityComboBox"
        Me._ModalityComboBox.Size = New System.Drawing.Size(157, 25)
        Me._ModalityComboBox.TabIndex = 1
        '
        '_ReadButton
        '
        Me._ReadButton.Location = New System.Drawing.Point(12, 15)
        Me._ReadButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ReadButton.Name = "_ReadButton"
        Me._ReadButton.Size = New System.Drawing.Size(58, 30)
        Me._ReadButton.TabIndex = 0
        Me._ReadButton.Text = "&Read"
        Me._ReadButton.UseVisualStyleBackColor = True
        '
        '_InitiateButton
        '
        Me._InitiateButton.Location = New System.Drawing.Point(12, 53)
        Me._InitiateButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._InitiateButton.Name = "_InitiateButton"
        Me._InitiateButton.Size = New System.Drawing.Size(58, 30)
        Me._InitiateButton.TabIndex = 0
        Me._InitiateButton.Text = "&Initiate"
        Me._InitiateButton.UseVisualStyleBackColor = True
        '
        '_interfaceTabPage
        '
        Me._interfaceTabPage.Controls.Add(Me._ResetButton)
        Me._interfaceTabPage.Controls.Add(Me._InterfaceClearButton)
        Me._interfaceTabPage.Location = New System.Drawing.Point(4, 26)
        Me._interfaceTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._interfaceTabPage.Name = "_interfaceTabPage"
        Me._interfaceTabPage.Size = New System.Drawing.Size(356, 295)
        Me._interfaceTabPage.TabIndex = 2
        Me._interfaceTabPage.Text = "Interface"
        Me._interfaceTabPage.UseVisualStyleBackColor = True
        '
        '_ResetButton
        '
        Me._ResetButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ResetButton.Location = New System.Drawing.Point(78, 120)
        Me._ResetButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
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
        Me._InterfaceClearButton.Location = New System.Drawing.Point(106, 65)
        Me._InterfaceClearButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._InterfaceClearButton.Name = "_InterfaceClearButton"
        Me._InterfaceClearButton.Size = New System.Drawing.Size(135, 30)
        Me._InterfaceClearButton.TabIndex = 0
        Me._InterfaceClearButton.Text = "&Clear Interface"
        Me._InterfaceClearButton.UseVisualStyleBackColor = True
        '
        '_channelTabPage
        '
        Me._channelTabPage.Controls.Add(Me._ClosedChannelsTextBoxLabel)
        Me._channelTabPage.Controls.Add(Me._ClosedChannelsTextBox)
        Me._channelTabPage.Controls.Add(Me._OpenAllButton)
        Me._channelTabPage.Controls.Add(Me._OpenChannelsButton)
        Me._channelTabPage.Controls.Add(Me._CloseOnlyButton)
        Me._channelTabPage.Controls.Add(Me._CloseChannelsButton)
        Me._channelTabPage.Controls.Add(Me._ChannelListComboBox)
        Me._channelTabPage.Controls.Add(Me._ChannelListComboBoxLabel)
        Me._channelTabPage.Location = New System.Drawing.Point(4, 26)
        Me._channelTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._channelTabPage.Name = "_channelTabPage"
        Me._channelTabPage.Size = New System.Drawing.Size(356, 295)
        Me._channelTabPage.TabIndex = 1
        Me._channelTabPage.Text = "Channel"
        Me._channelTabPage.UseVisualStyleBackColor = True
        '
        '_ClosedChannelsTextBoxLabel
        '
        Me._ClosedChannelsTextBoxLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
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
        Me._ClosedChannelsTextBox.Location = New System.Drawing.Point(7, 132)
        Me._ClosedChannelsTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
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
        Me._OpenAllButton.Location = New System.Drawing.Point(259, 77)
        Me._OpenAllButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._OpenAllButton.Name = "_OpenAllButton"
        Me._OpenAllButton.Size = New System.Drawing.Size(90, 30)
        Me._OpenAllButton.TabIndex = 11
        Me._OpenAllButton.Text = "Open &All"
        Me._OpenAllButton.UseVisualStyleBackColor = True
        '
        '_OpenChannelsButton
        '
        Me._OpenChannelsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._OpenChannelsButton.Location = New System.Drawing.Point(293, 10)
        Me._OpenChannelsButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._OpenChannelsButton.Name = "_OpenChannelsButton"
        Me._OpenChannelsButton.Size = New System.Drawing.Size(56, 30)
        Me._OpenChannelsButton.TabIndex = 10
        Me._OpenChannelsButton.Text = "&Open"
        Me._OpenChannelsButton.UseVisualStyleBackColor = True
        '
        '_CloseOnlyButton
        '
        Me._CloseOnlyButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._CloseOnlyButton.Location = New System.Drawing.Point(110, 77)
        Me._CloseOnlyButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
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
        Me._CloseChannelsButton.Location = New System.Drawing.Point(231, 10)
        Me._CloseChannelsButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
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
        Me._ChannelListComboBox.Location = New System.Drawing.Point(7, 42)
        Me._ChannelListComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ChannelListComboBox.Name = "_ChannelListComboBox"
        Me._ChannelListComboBox.Size = New System.Drawing.Size(341, 25)
        Me._ChannelListComboBox.TabIndex = 8
        Me._ChannelListComboBox.Text = "(@ 201,203:205)"
        '
        '_ChannelListComboBoxLabel
        '
        Me._ChannelListComboBoxLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
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
        Me._SenseTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SenseTabPage.Name = "_SenseTabPage"
        Me._SenseTabPage.Size = New System.Drawing.Size(356, 295)
        Me._SenseTabPage.TabIndex = 4
        Me._SenseTabPage.Text = "Sense"
        Me._SenseTabPage.UseVisualStyleBackColor = True
        '
        '_TriggerDelayTextBox
        '
        Me._TriggerDelayTextBox.Location = New System.Drawing.Point(157, 114)
        Me._TriggerDelayTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._TriggerDelayTextBox.Name = "_TriggerDelayTextBox"
        Me._TriggerDelayTextBox.Size = New System.Drawing.Size(75, 25)
        Me._TriggerDelayTextBox.TabIndex = 37
        Me._TriggerDelayTextBox.Text = "0.0"
        '
        '_TriggerDelayTextBoxLabel
        '
        Me._TriggerDelayTextBoxLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me._TriggerDelayTextBoxLabel.Location = New System.Drawing.Point(15, 114)
        Me._TriggerDelayTextBoxLabel.Name = "_TriggerDelayTextBoxLabel"
        Me._TriggerDelayTextBoxLabel.Size = New System.Drawing.Size(141, 24)
        Me._TriggerDelayTextBoxLabel.TabIndex = 36
        Me._TriggerDelayTextBoxLabel.Text = "Trigger Delay [s]:"
        Me._TriggerDelayTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseRangeTextBox
        '
        Me._SenseRangeTextBox.Location = New System.Drawing.Point(157, 81)
        Me._SenseRangeTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SenseRangeTextBox.Name = "_SenseRangeTextBox"
        Me._SenseRangeTextBox.Size = New System.Drawing.Size(75, 25)
        Me._SenseRangeTextBox.TabIndex = 33
        Me._SenseRangeTextBox.Text = "1.00"
        '
        '_SenseRangeTextBoxLabel
        '
        Me._SenseRangeTextBoxLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me._SenseRangeTextBoxLabel.Location = New System.Drawing.Point(15, 81)
        Me._SenseRangeTextBoxLabel.Name = "_SenseRangeTextBoxLabel"
        Me._SenseRangeTextBoxLabel.Size = New System.Drawing.Size(141, 24)
        Me._SenseRangeTextBoxLabel.TabIndex = 32
        Me._SenseRangeTextBoxLabel.Text = "Range:"
        Me._SenseRangeTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_IntegrationPeriodTextBox
        '
        Me._IntegrationPeriodTextBox.Location = New System.Drawing.Point(157, 47)
        Me._IntegrationPeriodTextBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._IntegrationPeriodTextBox.Name = "_IntegrationPeriodTextBox"
        Me._IntegrationPeriodTextBox.Size = New System.Drawing.Size(75, 25)
        Me._IntegrationPeriodTextBox.TabIndex = 31
        Me._IntegrationPeriodTextBox.Text = "16.7"
        '
        '_IntegrationPeriodTextBoxLabel
        '
        Me._IntegrationPeriodTextBoxLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me._IntegrationPeriodTextBoxLabel.Location = New System.Drawing.Point(12, 47)
        Me._IntegrationPeriodTextBoxLabel.Name = "_IntegrationPeriodTextBoxLabel"
        Me._IntegrationPeriodTextBoxLabel.Size = New System.Drawing.Size(144, 24)
        Me._IntegrationPeriodTextBoxLabel.TabIndex = 30
        Me._IntegrationPeriodTextBoxLabel.Text = "Integration Period [ms] :"
        Me._IntegrationPeriodTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_SenseFunctionComboBox
        '
        Me._SenseFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._SenseFunctionComboBox.Items.AddRange(New Object() {"I", "V"})
        Me._SenseFunctionComboBox.Location = New System.Drawing.Point(77, 12)
        Me._SenseFunctionComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SenseFunctionComboBox.Name = "_SenseFunctionComboBox"
        Me._SenseFunctionComboBox.Size = New System.Drawing.Size(175, 25)
        Me._SenseFunctionComboBox.TabIndex = 29
        '
        '_SenseFunctionComboBoxLabel
        '
        Me._SenseFunctionComboBoxLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me._SenseFunctionComboBoxLabel.Location = New System.Drawing.Point(14, 14)
        Me._SenseFunctionComboBoxLabel.Name = "_SenseFunctionComboBoxLabel"
        Me._SenseFunctionComboBoxLabel.Size = New System.Drawing.Size(62, 21)
        Me._SenseFunctionComboBoxLabel.TabIndex = 28
        Me._SenseFunctionComboBoxLabel.Text = "Function: "
        Me._SenseFunctionComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_SenseAutoRangeToggle
        '
        Me._SenseAutoRangeToggle.Location = New System.Drawing.Point(239, 84)
        Me._SenseAutoRangeToggle.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SenseAutoRangeToggle.Name = "_SenseAutoRangeToggle"
        Me._SenseAutoRangeToggle.Size = New System.Drawing.Size(103, 21)
        Me._SenseAutoRangeToggle.TabIndex = 22
        Me._SenseAutoRangeToggle.Text = "Auto Range"
        Me._SenseAutoRangeToggle.UseVisualStyleBackColor = True
        '
        '_ApplySenseSettingsButton
        '
        Me._ApplySenseSettingsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ApplySenseSettingsButton.Location = New System.Drawing.Point(279, 224)
        Me._ApplySenseSettingsButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
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
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(356, 295)
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
        Me._SimpleReadWriteControl.Size = New System.Drawing.Size(356, 295)
        Me._SimpleReadWriteControl.TabIndex = 0
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me.TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(356, 295)
        Me._MessagesTabPage.TabIndex = 3
        Me._MessagesTabPage.Text = "Log"
        Me._MessagesTabPage.UseVisualStyleBackColor = True
        '
        '_ReadingTextBox
        '
        Me._ReadingTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._ReadingTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._ReadingTextBox.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold)
        Me._ReadingTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._ReadingTextBox.Location = New System.Drawing.Point(0, 17)
        Me._ReadingTextBox.Name = "_ReadingTextBox"
        Me._ReadingTextBox.Size = New System.Drawing.Size(364, 43)
        Me._ReadingTextBox.TabIndex = 16
        Me._ReadingTextBox.Text = "0.0000000 mV"
        Me._ReadingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
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
        Me._TitleLabel.Text = "SCPi"
        Me._TitleLabel.UseMnemonic = False
        '
        'ScpiPanel
        '
        Me.Controls.Add(Me._Tabs)
        Me.Controls.Add(Me._ReadingTextBox)
        Me.Controls.Add(Me._TitleLabel)
        Me.Name = "ScpiPanel"
        Me.Size = New System.Drawing.Size(364, 430)
        Me.Controls.SetChildIndex(Me._TitleLabel, 0)
        Me.Controls.SetChildIndex(Me._ReadingTextBox, 0)
        Me.Controls.SetChildIndex(Me.Connector, 0)
        Me.Controls.SetChildIndex(Me._Tabs, 0)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Tabs.ResumeLayout(False)
        Me._ReadingTabPage.ResumeLayout(False)
        Me._ReadingTabPage.PerformLayout()
        Me._interfaceTabPage.ResumeLayout(False)
        Me._channelTabPage.ResumeLayout(False)
        Me._channelTabPage.PerformLayout()
        Me._SenseTabPage.ResumeLayout(False)
        Me._SenseTabPage.PerformLayout()
        Me._ReadWriteTabPage.ResumeLayout(False)
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _ReadingTabPage As System.Windows.Forms.TabPage
    Private WithEvents _TerminalsToggle As System.Windows.Forms.CheckBox
    Private WithEvents _ModalityComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _ReadButton As System.Windows.Forms.Button
    Private WithEvents _InitiateButton As System.Windows.Forms.Button
    Private WithEvents _interfaceTabPage As System.Windows.Forms.TabPage
    Private WithEvents _InterfaceClearButton As System.Windows.Forms.Button
    Private WithEvents _channelTabPage As System.Windows.Forms.TabPage
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
    Private WithEvents _ModalityComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _ResetButton As System.Windows.Forms.Button
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As SimpleReadWriteControl
    Private WithEvents _TitleLabel As Windows.Forms.Label
End Class
