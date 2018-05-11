<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EG2000Panel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EG2000Panel))
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
        Me._SystemToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ResetSplitButton = New System.Windows.Forms.ToolStripSplitButton()
        Me._ClearInterfaceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ReadStatusByteMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearDeviceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ResetKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._InitKnownStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearExecutionStateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ResetMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ServiceRequestHandlersEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._TraceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._LogTraceLevelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._DisplayTraceLevelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SessionNotificationLevelMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SessionNotificationLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._SessionServiceRequestHandlerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._DeviceServiceRequestHandlerEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._LogTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._DisplayTraceLevelComboBox = New isr.Core.Controls.ToolStripComboBox()
        Me._TimingLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._ReadTerminalStateButton = New System.Windows.Forms.ToolStripButton()
        Me._ServiceRequestEnableBitmaskNumericLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ServiceRequestEnableBitmaskNumeric = New isr.Core.Controls.ToolStripNumericUpDown()
        Me._SentGroupBox = New System.Windows.Forms.GroupBox()
        Me._SendMessageLabel = New System.Windows.Forms.Label()
        Me._UnhandledSendLabel = New System.Windows.Forms.Label()
        Me._ReceivedGroupBox = New System.Windows.Forms.GroupBox()
        Me._ReceivedMessageLabel = New System.Windows.Forms.Label()
        Me._UnhandledMessageLabel = New System.Windows.Forms.Label()
        Me._ReceivedMessagesGroupBox = New System.Windows.Forms.GroupBox()
        Me._WaferStartReceivedLabel = New System.Windows.Forms.Label()
        Me._TestStartAttributeLabel = New System.Windows.Forms.Label()
        Me._TestStartedLabel = New System.Windows.Forms.Label()
        Me._PatternCompleteLabel = New System.Windows.Forms.Label()
        Me._TestCompleteLabel = New System.Windows.Forms.Label()
        Me._EmulatedReplyComboBox = New System.Windows.Forms.ComboBox()
        Me._CommandComboBox = New System.Windows.Forms.ComboBox()
        Me._LastMessageTextBox = New System.Windows.Forms.TextBox()
        Me._LastMessageTextBoxLabel = New System.Windows.Forms.Label()
        Me._EmulateButton = New System.Windows.Forms.Button()
        Me._WriteButton = New System.Windows.Forms.Button()
        Me._ReadWriteTabPage = New System.Windows.Forms.TabPage()
        Me._SimpleReadWriteControl = New isr.VI.Instrument.SimpleReadWriteControl()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._ReadingTextBox = New System.Windows.Forms.TextBox()
        Me._Panel = New System.Windows.Forms.Panel()
        Me._TitleLabel = New System.Windows.Forms.Label()
        Me._Layout = New System.Windows.Forms.TableLayoutPanel()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        Me._SystemToolStrip.SuspendLayout()
        Me._SentGroupBox.SuspendLayout()
        Me._ReceivedGroupBox.SuspendLayout()
        Me._ReceivedMessagesGroupBox.SuspendLayout()
        Me._ReadWriteTabPage.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
        Me._Panel.SuspendLayout()
        Me._Layout.SuspendLayout()
        Me.SuspendLayout()
        '
        'Connector
        '
        Me.Connector.Location = New System.Drawing.Point(0, 388)
        Me.Connector.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.Connector.Searchable = True
        '
        'TraceMessagesBox
        '
        Me.TraceMessagesBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TraceMessagesBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TraceMessagesBox.PresetCount = 50
        Me.TraceMessagesBox.ResetCount = 100
        Me.TraceMessagesBox.Size = New System.Drawing.Size(356, 305)
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ReadingTabPage)
        Me._Tabs.Controls.Add(Me._ReadWriteTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(52, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 53)
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 335)
        Me._Tabs.TabIndex = 15
        '
        '_ReadingTabPage
        '
        Me._ReadingTabPage.Controls.Add(Me._SystemToolStrip)
        Me._ReadingTabPage.Controls.Add(Me._SentGroupBox)
        Me._ReadingTabPage.Controls.Add(Me._ReceivedGroupBox)
        Me._ReadingTabPage.Controls.Add(Me._ReceivedMessagesGroupBox)
        Me._ReadingTabPage.Controls.Add(Me._EmulatedReplyComboBox)
        Me._ReadingTabPage.Controls.Add(Me._CommandComboBox)
        Me._ReadingTabPage.Controls.Add(Me._LastMessageTextBox)
        Me._ReadingTabPage.Controls.Add(Me._LastMessageTextBoxLabel)
        Me._ReadingTabPage.Controls.Add(Me._EmulateButton)
        Me._ReadingTabPage.Controls.Add(Me._WriteButton)
        Me._ReadingTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadingTabPage.Name = "_ReadingTabPage"
        Me._ReadingTabPage.Size = New System.Drawing.Size(356, 305)
        Me._ReadingTabPage.TabIndex = 0
        Me._ReadingTabPage.Text = "Reading"
        Me._ReadingTabPage.UseVisualStyleBackColor = True
        '
        '_SystemToolStrip
        '
        Me._SystemToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._SystemToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetSplitButton, Me._ReadTerminalStateButton, Me._ServiceRequestEnableBitmaskNumericLabel, Me._ServiceRequestEnableBitmaskNumeric})
        Me._SystemToolStrip.Location = New System.Drawing.Point(0, 277)
        Me._SystemToolStrip.Name = "_SystemToolStrip"
        Me._SystemToolStrip.Size = New System.Drawing.Size(356, 28)
        Me._SystemToolStrip.TabIndex = 21
        Me._SystemToolStrip.Text = "System Tools"
        Me.TipsTooltip.SetToolTip(Me._SystemToolStrip, "System operations")
        '
        '_ResetSplitButton
        '
        Me._ResetSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._ResetSplitButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ResetMenuItem, Me._ServiceRequestHandlersEnabledMenuItem, Me._TraceMenuItem})
        Me._ResetSplitButton.Image = CType(resources.GetObject("_ResetSplitButton.Image"), System.Drawing.Image)
        Me._ResetSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ResetSplitButton.Name = "_ResetSplitButton"
        Me._ResetSplitButton.Size = New System.Drawing.Size(57, 23)
        Me._ResetSplitButton.Text = "Device"
        Me._ResetSplitButton.ToolTipText = "Opens the device command and settings menu"
        '
        '_ClearInterfaceMenuItem
        '
        Me._ClearInterfaceMenuItem.Name = "_ClearInterfaceMenuItem"
        Me._ClearInterfaceMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._ClearInterfaceMenuItem.Text = "Clear Interface"
        Me._ClearInterfaceMenuItem.ToolTipText = "Issues an interface clear command"
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
        Me._InitKnownStateMenuItem.ToolTipText = "Initializes to custom known state"
        '
        '_ClearExecutionStateMenuItem
        '
        Me._ClearExecutionStateMenuItem.Name = "_ClearExecutionStateMenuItem"
        Me._ClearExecutionStateMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._ClearExecutionStateMenuItem.Text = "Clear Execution State (CLS)"
        Me._ClearExecutionStateMenuItem.ToolTipText = "Clears the execution state"
        '
        '_ReadStatusByteMenuItem
        '
        Me._ReadStatusByteMenuItem.Name = "_ReadStatusByteMenuItem"
        Me._ReadStatusByteMenuItem.Size = New System.Drawing.Size(217, 22)
        Me._ReadStatusByteMenuItem.Text = "Read Status Byte"
        '
        '_ResetMenuItem
        '
        Me._ResetMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ClearInterfaceMenuItem, Me._ClearDeviceMenuItem, Me._ResetKnownStateMenuItem, Me._InitKnownStateMenuItem, Me._ClearExecutionStateMenuItem, Me._ReadStatusByteMenuItem})
        Me._ResetMenuItem.Name = "_ResetMenuItem"
        Me._ResetMenuItem.Size = New System.Drawing.Size(216, 22)
        Me._ResetMenuItem.Text = "Reset..."
        Me._ResetMenuItem.ToolTipText = "Opens the reset menus"
        '
        '_ServiceRequestHandlersEnabledMenuItem
        '
        Me._ServiceRequestHandlersEnabledMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SessionServiceRequestHandlerEnabledMenuItem, Me._DeviceServiceRequestHandlerEnabledMenuItem})
        Me._ServiceRequestHandlersEnabledMenuItem.Name = "_ServiceRequestHandlersEnabledMenuItem"
        Me._ServiceRequestHandlersEnabledMenuItem.Size = New System.Drawing.Size(216, 22)
        Me._ServiceRequestHandlersEnabledMenuItem.Text = "SRQ Handlers Enable..."
        Me._ServiceRequestHandlersEnabledMenuItem.ToolTipText = "Opens the SQR Handler Enable menu"
        '
        '_TraceMenuItem
        '
        Me._TraceMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SessionNotificationLevelMenuItem, Me._LogTraceLevelMenuItem, Me._DisplayTraceLevelMenuItem})
        Me._TraceMenuItem.Name = "_TraceMenuItem"
        Me._TraceMenuItem.Size = New System.Drawing.Size(216, 22)
        Me._TraceMenuItem.Text = "Trace..."
        Me._TraceMenuItem.ToolTipText = "Opens the trace menus"
        '
        '_SessionNotificationLevelMenuItem
        '
        Me._SessionNotificationLevelMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SessionNotificationLevelComboBox})
        Me._SessionNotificationLevelMenuItem.Name = "_SessionNotificationLevelMenuItem"
        Me._SessionNotificationLevelMenuItem.Size = New System.Drawing.Size(209, 22)
        Me._SessionNotificationLevelMenuItem.Text = "Session Notification Level"
        Me._SessionNotificationLevelMenuItem.ToolTipText = "Shows the session notification level selector"
        '
        '_SessionNotificationLevelComboBox
        '
        Me._SessionNotificationLevelComboBox.Name = "_SessionNotificationLevelComboBox"
        Me._SessionNotificationLevelComboBox.Size = New System.Drawing.Size(100, 22)
        Me._SessionNotificationLevelComboBox.ToolTipText = "Select the session notification level"
        '
        '_LogTraceLevelMenuItem
        '
        Me._LogTraceLevelMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._LogTraceLevelComboBox})
        Me._LogTraceLevelMenuItem.Name = "_LogTraceLevelMenuItem"
        Me._LogTraceLevelMenuItem.Size = New System.Drawing.Size(209, 22)
        Me._LogTraceLevelMenuItem.Text = "Log Trace Level"
        Me._LogTraceLevelMenuItem.ToolTipText = "Shows the log trace levels"
        '
        '_DisplayTraceLevelMenuItem
        '
        Me._DisplayTraceLevelMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._DisplayTraceLevelComboBox})
        Me._DisplayTraceLevelMenuItem.Name = "_DisplayTraceLevelMenuItem"
        Me._DisplayTraceLevelMenuItem.Size = New System.Drawing.Size(209, 22)
        Me._DisplayTraceLevelMenuItem.Text = "Display Trace Level"
        Me._DisplayTraceLevelMenuItem.ToolTipText = "Shows the display trace levels"
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
        Me._ReadTerminalStateButton.Visible = False
        '
        '_ServiceRequestEnableBitmaskNumericLabel
        '
        Me._ServiceRequestEnableBitmaskNumericLabel.Name = "_ServiceRequestEnableBitmaskNumericLabel"
        Me._ServiceRequestEnableBitmaskNumericLabel.Size = New System.Drawing.Size(29, 25)
        Me._ServiceRequestEnableBitmaskNumericLabel.Text = "SRE:"
        '
        '_ServiceRequestEnableBitmaskNumeric
        '
        Me._ServiceRequestEnableBitmaskNumeric.Name = "_ServiceRequestEnableBitmaskNumeric"
        Me._ServiceRequestEnableBitmaskNumeric.Size = New System.Drawing.Size(41, 25)
        Me._ServiceRequestEnableBitmaskNumeric.Text = "0"
        Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = "Service request enable bitmask"
        Me._ServiceRequestEnableBitmaskNumeric.Value = New Decimal(New Integer() {0, 0, 0, 0})
        '
        '_SentGroupBox
        '
        Me._SentGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SentGroupBox.Controls.Add(Me._SendMessageLabel)
        Me._SentGroupBox.Controls.Add(Me._UnhandledSendLabel)
        Me._SentGroupBox.Location = New System.Drawing.Point(160, 139)
        Me._SentGroupBox.Name = "_SentGroupBox"
        Me._SentGroupBox.Size = New System.Drawing.Size(185, 72)
        Me._SentGroupBox.TabIndex = 6
        Me._SentGroupBox.TabStop = False
        Me._SentGroupBox.Text = "Message Sent"
        '
        '_SendMessageLabel
        '
        Me._SendMessageLabel.AutoSize = True
        Me._SendMessageLabel.Location = New System.Drawing.Point(6, 46)
        Me._SendMessageLabel.Name = "_SendMessageLabel"
        Me._SendMessageLabel.Size = New System.Drawing.Size(46, 17)
        Me._SendMessageLabel.TabIndex = 1
        Me._SendMessageLabel.Text = "Sent ..."
        Me.TipsTooltip.SetToolTip(Me._SendMessageLabel, "Message sent to the Prober and processed.")
        '
        '_UnhandledSendLabel
        '
        Me._UnhandledSendLabel.AutoSize = True
        Me._UnhandledSendLabel.Location = New System.Drawing.Point(6, 22)
        Me._UnhandledSendLabel.Name = "_UnhandledSendLabel"
        Me._UnhandledSendLabel.Size = New System.Drawing.Size(56, 17)
        Me._UnhandledSendLabel.TabIndex = 0
        Me._UnhandledSendLabel.Text = "? Sent: .."
        Me.TipsTooltip.SetToolTip(Me._UnhandledSendLabel, "Unknown message was sent to the Prober.")
        '
        '_ReceivedGroupBox
        '
        Me._ReceivedGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ReceivedGroupBox.Controls.Add(Me._ReceivedMessageLabel)
        Me._ReceivedGroupBox.Controls.Add(Me._UnhandledMessageLabel)
        Me._ReceivedGroupBox.Location = New System.Drawing.Point(160, 66)
        Me._ReceivedGroupBox.Name = "_ReceivedGroupBox"
        Me._ReceivedGroupBox.Size = New System.Drawing.Size(185, 67)
        Me._ReceivedGroupBox.TabIndex = 5
        Me._ReceivedGroupBox.TabStop = False
        Me._ReceivedGroupBox.Text = "Received Messages"
        '
        '_ReceivedMessageLabel
        '
        Me._ReceivedMessageLabel.AutoSize = True
        Me._ReceivedMessageLabel.Location = New System.Drawing.Point(6, 41)
        Me._ReceivedMessageLabel.Name = "_ReceivedMessageLabel"
        Me._ReceivedMessageLabel.Size = New System.Drawing.Size(73, 17)
        Me._ReceivedMessageLabel.TabIndex = 1
        Me._ReceivedMessageLabel.Text = "Received: .."
        Me.TipsTooltip.SetToolTip(Me._ReceivedMessageLabel, "Message received from the Prober and processed")
        '
        '_UnhandledMessageLabel
        '
        Me._UnhandledMessageLabel.AutoSize = True
        Me._UnhandledMessageLabel.BackColor = System.Drawing.Color.LightGreen
        Me._UnhandledMessageLabel.Location = New System.Drawing.Point(6, 19)
        Me._UnhandledMessageLabel.Name = "_UnhandledMessageLabel"
        Me._UnhandledMessageLabel.Size = New System.Drawing.Size(83, 17)
        Me._UnhandledMessageLabel.TabIndex = 0
        Me._UnhandledMessageLabel.Text = "? Received: .."
        Me.TipsTooltip.SetToolTip(Me._UnhandledMessageLabel, "Received unhandled message")
        '
        '_ReceivedMessagesGroupBox
        '
        Me._ReceivedMessagesGroupBox.Controls.Add(Me._WaferStartReceivedLabel)
        Me._ReceivedMessagesGroupBox.Controls.Add(Me._TestStartAttributeLabel)
        Me._ReceivedMessagesGroupBox.Controls.Add(Me._TestStartedLabel)
        Me._ReceivedMessagesGroupBox.Controls.Add(Me._PatternCompleteLabel)
        Me._ReceivedMessagesGroupBox.Controls.Add(Me._TestCompleteLabel)
        Me._ReceivedMessagesGroupBox.Location = New System.Drawing.Point(13, 66)
        Me._ReceivedMessagesGroupBox.Name = "_ReceivedMessagesGroupBox"
        Me._ReceivedMessagesGroupBox.Size = New System.Drawing.Size(131, 133)
        Me._ReceivedMessagesGroupBox.TabIndex = 4
        Me._ReceivedMessagesGroupBox.TabStop = False
        Me._ReceivedMessagesGroupBox.Text = "Prober Cycle"
        Me.TipsTooltip.SetToolTip(Me._ReceivedMessagesGroupBox, "Lists messages received.")
        '
        '_WaferStartReceivedLabel
        '
        Me._WaferStartReceivedLabel.AutoSize = True
        Me._WaferStartReceivedLabel.Location = New System.Drawing.Point(6, 19)
        Me._WaferStartReceivedLabel.Name = "_WaferStartReceivedLabel"
        Me._WaferStartReceivedLabel.Size = New System.Drawing.Size(88, 17)
        Me._WaferStartReceivedLabel.TabIndex = 0
        Me._WaferStartReceivedLabel.Text = "Wafer Started"
        '
        '_TestStartAttributeLabel
        '
        Me._TestStartAttributeLabel.AutoSize = True
        Me._TestStartAttributeLabel.Location = New System.Drawing.Point(6, 41)
        Me._TestStartAttributeLabel.Name = "_TestStartAttributeLabel"
        Me._TestStartAttributeLabel.Size = New System.Drawing.Size(98, 17)
        Me._TestStartAttributeLabel.TabIndex = 1
        Me._TestStartAttributeLabel.Text = "1st Test Started"
        '
        '_TestStartedLabel
        '
        Me._TestStartedLabel.AutoSize = True
        Me._TestStartedLabel.Location = New System.Drawing.Point(6, 63)
        Me._TestStartedLabel.Name = "_TestStartedLabel"
        Me._TestStartedLabel.Size = New System.Drawing.Size(77, 17)
        Me._TestStartedLabel.TabIndex = 2
        Me._TestStartedLabel.Text = "Test Started"
        '
        '_PatternCompleteLabel
        '
        Me._PatternCompleteLabel.AutoSize = True
        Me._PatternCompleteLabel.Location = New System.Drawing.Point(6, 107)
        Me._PatternCompleteLabel.Name = "_PatternCompleteLabel"
        Me._PatternCompleteLabel.Size = New System.Drawing.Size(109, 17)
        Me._PatternCompleteLabel.TabIndex = 4
        Me._PatternCompleteLabel.Text = "Pattern Complete"
        '
        '_TestCompleteLabel
        '
        Me._TestCompleteLabel.AutoSize = True
        Me._TestCompleteLabel.Location = New System.Drawing.Point(6, 85)
        Me._TestCompleteLabel.Name = "_TestCompleteLabel"
        Me._TestCompleteLabel.Size = New System.Drawing.Size(91, 17)
        Me._TestCompleteLabel.TabIndex = 3
        Me._TestCompleteLabel.Text = "Test Complete"
        '
        '_EmulatedReplyComboBox
        '
        Me._EmulatedReplyComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._EmulatedReplyComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._EmulatedReplyComboBox.FormattingEnabled = True
        Me._EmulatedReplyComboBox.Location = New System.Drawing.Point(93, 36)
        Me._EmulatedReplyComboBox.Name = "_EmulatedReplyComboBox"
        Me._EmulatedReplyComboBox.Size = New System.Drawing.Size(252, 25)
        Me._EmulatedReplyComboBox.TabIndex = 3
        '
        '_CommandComboBox
        '
        Me._CommandComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._CommandComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._CommandComboBox.FormattingEnabled = True
        Me._CommandComboBox.Location = New System.Drawing.Point(93, 5)
        Me._CommandComboBox.Name = "_CommandComboBox"
        Me._CommandComboBox.Size = New System.Drawing.Size(252, 25)
        Me._CommandComboBox.TabIndex = 1
        Me._CommandComboBox.Text = "SM15M0000000000000"
        Me.TipsTooltip.SetToolTip(Me._CommandComboBox, "Command to send to the Prober")
        '
        '_LastMessageTextBox
        '
        Me._LastMessageTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._LastMessageTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._LastMessageTextBox.Location = New System.Drawing.Point(12, 222)
        Me._LastMessageTextBox.Multiline = True
        Me._LastMessageTextBox.Name = "_LastMessageTextBox"
        Me._LastMessageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._LastMessageTextBox.Size = New System.Drawing.Size(333, 54)
        Me._LastMessageTextBox.TabIndex = 8
        '
        '_LastMessageTextBoxLabel
        '
        Me._LastMessageTextBoxLabel.AutoSize = True
        Me._LastMessageTextBoxLabel.Location = New System.Drawing.Point(9, 203)
        Me._LastMessageTextBoxLabel.Name = "_LastMessageTextBoxLabel"
        Me._LastMessageTextBoxLabel.Size = New System.Drawing.Size(95, 17)
        Me._LastMessageTextBoxLabel.TabIndex = 7
        Me._LastMessageTextBoxLabel.Text = "Last Message: "
        '
        '_EmulateButton
        '
        Me._EmulateButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._EmulateButton.Location = New System.Drawing.Point(13, 33)
        Me._EmulateButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._EmulateButton.Name = "_EmulateButton"
        Me._EmulateButton.Size = New System.Drawing.Size(74, 30)
        Me._EmulateButton.TabIndex = 2
        Me._EmulateButton.Text = "&Emulate"
        Me._EmulateButton.UseVisualStyleBackColor = True
        '
        '_WriteButton
        '
        Me._WriteButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._WriteButton.Location = New System.Drawing.Point(29, 2)
        Me._WriteButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._WriteButton.Name = "_WriteButton"
        Me._WriteButton.Size = New System.Drawing.Size(58, 30)
        Me._WriteButton.TabIndex = 0
        Me._WriteButton.Text = "&Write"
        Me._WriteButton.UseVisualStyleBackColor = True
        '
        '_ReadWriteTabPage
        '
        Me._ReadWriteTabPage.Controls.Add(Me._SimpleReadWriteControl)
        Me._ReadWriteTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ReadWriteTabPage.Name = "_ReadWriteTabPage"
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(356, 305)
        Me._ReadWriteTabPage.TabIndex = 4
        Me._ReadWriteTabPage.Text = "R/W"
        Me._ReadWriteTabPage.UseVisualStyleBackColor = True
        '
        '_SimpleReadWriteControl
        '
        Me._SimpleReadWriteControl.AutoAppendTermination = Nothing
        Me._SimpleReadWriteControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me._SimpleReadWriteControl.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SimpleReadWriteControl.Location = New System.Drawing.Point(0, 0)
        Me._SimpleReadWriteControl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._SimpleReadWriteControl.Name = "_SimpleReadWriteControl"
        Me._SimpleReadWriteControl.ReadEnabled = False
        Me._SimpleReadWriteControl.Size = New System.Drawing.Size(356, 305)
        Me._SimpleReadWriteControl.TabIndex = 0
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me.TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(356, 305)
        Me._MessagesTabPage.TabIndex = 3
        Me._MessagesTabPage.Text = "Log"
        Me._MessagesTabPage.UseVisualStyleBackColor = True
        '
        '_TimingLabel
        '
        Me._TimingLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TimingLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TimingLabel.ForeColor = System.Drawing.Color.LightSkyBlue
        Me._TimingLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._TimingLabel.Name = "_TimingLabel"
        Me._TimingLabel.Size = New System.Drawing.Size(0, 0)
        Me._TimingLabel.ToolTipText = "Elapsed time, ms"
        '
        '_ReadingTextBox
        '
        Me._ReadingTextBox.BackColor = System.Drawing.SystemColors.MenuText
        Me._ReadingTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._ReadingTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._ReadingTextBox.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold)
        Me._ReadingTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._ReadingTextBox.Location = New System.Drawing.Point(0, 17)
        Me._ReadingTextBox.Name = "_ReadingTextBox"
        Me._ReadingTextBox.Size = New System.Drawing.Size(364, 36)
        Me._ReadingTextBox.TabIndex = 16
        Me._ReadingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_Panel
        '
        Me._Panel.Controls.Add(Me._Tabs)
        Me._Panel.Controls.Add(Me._ReadingTextBox)
        Me._Panel.Controls.Add(Me._TitleLabel)
        Me._Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Panel.Location = New System.Drawing.Point(0, 0)
        Me._Panel.Margin = New System.Windows.Forms.Padding(0)
        Me._Panel.Name = "_Panel"
        Me._Panel.Size = New System.Drawing.Size(364, 388)
        Me._Panel.TabIndex = 17
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
        Me._TitleLabel.Text = "EG2000"
        Me._TitleLabel.UseMnemonic = False
        '
        '_Layout
        '
        Me._Layout.ColumnCount = 1
        Me._Layout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._Layout.Controls.Add(Me._Panel, 0, 1)
        Me._Layout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Layout.Location = New System.Drawing.Point(0, 0)
        Me._Layout.Margin = New System.Windows.Forms.Padding(0)
        Me._Layout.Name = "_Layout"
        Me._Layout.RowCount = 2
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._Layout.Size = New System.Drawing.Size(364, 388)
        Me._Layout.TabIndex = 18
        '
        'EG2000Panel
        '
        Me.Controls.Add(Me._Layout)
        Me.Name = "EG2000Panel"
        Me.Size = New System.Drawing.Size(364, 450)
        Me.Controls.SetChildIndex(Me.Connector, 0)
        Me.Controls.SetChildIndex(Me._Layout, 0)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Tabs.ResumeLayout(False)
        Me._ReadingTabPage.ResumeLayout(False)
        Me._ReadingTabPage.PerformLayout()
        Me._SystemToolStrip.ResumeLayout(False)
        Me._SystemToolStrip.PerformLayout()
        Me._SentGroupBox.ResumeLayout(False)
        Me._SentGroupBox.PerformLayout()
        Me._ReceivedGroupBox.ResumeLayout(False)
        Me._ReceivedGroupBox.PerformLayout()
        Me._ReceivedMessagesGroupBox.ResumeLayout(False)
        Me._ReceivedMessagesGroupBox.PerformLayout()
        Me._ReadWriteTabPage.ResumeLayout(False)
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._Panel.ResumeLayout(False)
        Me._Panel.PerformLayout()
        Me._Layout.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _ReadingTabPage As System.Windows.Forms.TabPage
    Private WithEvents _WriteButton As System.Windows.Forms.Button
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _LastMessageTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _LastMessageTextBox As System.Windows.Forms.TextBox
    Private WithEvents _EmulateButton As System.Windows.Forms.Button
    Private WithEvents _UnhandledMessageLabel As System.Windows.Forms.Label
    Private WithEvents _TestCompleteLabel As System.Windows.Forms.Label
    Private WithEvents _PatternCompleteLabel As System.Windows.Forms.Label
    Private WithEvents _TestStartedLabel As System.Windows.Forms.Label
    Private WithEvents _TestStartAttributeLabel As System.Windows.Forms.Label
    Private WithEvents _WaferStartReceivedLabel As System.Windows.Forms.Label
    Private WithEvents _CommandComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _EmulatedReplyComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _ReceivedGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents _SendMessageLabel As System.Windows.Forms.Label
    Private WithEvents _ReceivedMessageLabel As System.Windows.Forms.Label
    Private WithEvents _UnhandledSendLabel As System.Windows.Forms.Label
    Private WithEvents _ReceivedMessagesGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents _SentGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TitleLabel As System.Windows.Forms.Label
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
    Private WithEvents _SystemToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _ResetSplitButton As Windows.Forms.ToolStripSplitButton
    Private WithEvents _ClearInterfaceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearDeviceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ReadStatusByteMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ResetKnownStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _InitKnownStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearExecutionStateMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ResetMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ServiceRequestHandlersEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _TraceMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _DisplayTraceLevelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _LogTraceLevelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _SessionNotificationLevelMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _SessionNotificationLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _SessionServiceRequestHandlerEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _DeviceServiceRequestHandlerEnabledMenuItem As Windows.Forms.ToolStripMenuItem
    Private WithEvents _ReadTerminalStateButton As Windows.Forms.ToolStripButton
    Private WithEvents _ServiceRequestEnableBitmaskNumericLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _ServiceRequestEnableBitmaskNumeric As Core.Controls.ToolStripNumericUpDown
    Private WithEvents _LogTraceLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _DisplayTraceLevelComboBox As Core.Controls.ToolStripComboBox
    Private WithEvents _TimingLabel As System.Windows.Forms.ToolStripStatusLabel
End Class
