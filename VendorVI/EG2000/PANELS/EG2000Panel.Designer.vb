<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EG2000Panel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ReadingTabPage = New System.Windows.Forms.TabPage()
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
        Me._ReadingTextBox = New System.Windows.Forms.TextBox()
        Me._Panel = New System.Windows.Forms.Panel()
        Me._Layout = New System.Windows.Forms.TableLayoutPanel()
        Me._TitleLabel = New System.Windows.Forms.Label()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Tabs.SuspendLayout()
        Me._ReadingTabPage.SuspendLayout()
        Me._SentGroupBox.SuspendLayout()
        Me._ReceivedGroupBox.SuspendLayout()
        Me._ReceivedMessagesGroupBox.SuspendLayout()
        Me._ResetTabPage.SuspendLayout()
        Me._ResetLayout.SuspendLayout()
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
        Me.TraceMessagesBox.Size = New System.Drawing.Size(356, 295)
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ReadingTabPage)
        Me._Tabs.Controls.Add(Me._ResetTabPage)
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
        '_SentGroupBox
        '
        Me._SentGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SentGroupBox.Controls.Add(Me._SendMessageLabel)
        Me._SentGroupBox.Controls.Add(Me._UnhandledSendLabel)
        Me._SentGroupBox.Location = New System.Drawing.Point(160, 154)
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
        Me._ReceivedGroupBox.Location = New System.Drawing.Point(160, 81)
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
        Me._ReceivedMessagesGroupBox.Location = New System.Drawing.Point(13, 81)
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
        Me._EmulatedReplyComboBox.Location = New System.Drawing.Point(93, 47)
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
        Me._CommandComboBox.Location = New System.Drawing.Point(93, 10)
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
        Me._LastMessageTextBox.Location = New System.Drawing.Point(12, 237)
        Me._LastMessageTextBox.Multiline = True
        Me._LastMessageTextBox.Name = "_LastMessageTextBox"
        Me._LastMessageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._LastMessageTextBox.Size = New System.Drawing.Size(333, 60)
        Me._LastMessageTextBox.TabIndex = 8
        '
        '_LastMessageTextBoxLabel
        '
        Me._LastMessageTextBoxLabel.AutoSize = True
        Me._LastMessageTextBoxLabel.Location = New System.Drawing.Point(9, 218)
        Me._LastMessageTextBoxLabel.Name = "_LastMessageTextBoxLabel"
        Me._LastMessageTextBoxLabel.Size = New System.Drawing.Size(95, 17)
        Me._LastMessageTextBoxLabel.TabIndex = 7
        Me._LastMessageTextBoxLabel.Text = "Last Message: "
        '
        '_EmulateButton
        '
        Me._EmulateButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._EmulateButton.Location = New System.Drawing.Point(13, 44)
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
        Me._WriteButton.Location = New System.Drawing.Point(29, 7)
        Me._WriteButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._WriteButton.Name = "_WriteButton"
        Me._WriteButton.Size = New System.Drawing.Size(58, 30)
        Me._WriteButton.TabIndex = 0
        Me._WriteButton.Text = "&Write"
        Me._WriteButton.UseVisualStyleBackColor = True
        '
        '_ResetTabPage
        '
        Me._ResetTabPage.Controls.Add(Me._ResetLayout)
        Me._ResetTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ResetTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ResetTabPage.Name = "_ResetTabPage"
        Me._ResetTabPage.Size = New System.Drawing.Size(356, 295)
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
        Me._ResetLayout.Size = New System.Drawing.Size(356, 295)
        Me._ResetLayout.TabIndex = 3
        '
        '_InitializeKnownStateButton
        '
        Me._InitializeKnownStateButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._InitializeKnownStateButton.Location = New System.Drawing.Point(84, 191)
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
        Me._InterfaceClearButton.Location = New System.Drawing.Point(84, 23)
        Me._InterfaceClearButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
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
        Me._ResetButton.Location = New System.Drawing.Point(84, 135)
        Me._ResetButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ResetButton.Name = "_ResetButton"
        Me._ResetButton.Size = New System.Drawing.Size(187, 30)
        Me._ResetButton.TabIndex = 2
        Me._ResetButton.Text = "&Reset to Known State"
        Me._ResetButton.UseVisualStyleBackColor = True
        '
        '_SelectiveDeviceClearButton
        '
        Me._SelectiveDeviceClearButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._SelectiveDeviceClearButton.Location = New System.Drawing.Point(84, 79)
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
        Me._SessionTraceEnableCheckBox.Location = New System.Drawing.Point(84, 246)
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
        Me._ReadWriteTabPage.Size = New System.Drawing.Size(356, 295)
        Me._ReadWriteTabPage.TabIndex = 4
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
        Me._SentGroupBox.ResumeLayout(False)
        Me._SentGroupBox.PerformLayout()
        Me._ReceivedGroupBox.ResumeLayout(False)
        Me._ReceivedGroupBox.PerformLayout()
        Me._ReceivedMessagesGroupBox.ResumeLayout(False)
        Me._ReceivedMessagesGroupBox.PerformLayout()
        Me._ResetTabPage.ResumeLayout(False)
        Me._ResetLayout.ResumeLayout(False)
        Me._ResetLayout.PerformLayout()
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
    Private WithEvents _ResetTabPage As System.Windows.Forms.TabPage
    Private WithEvents _InterfaceClearButton As System.Windows.Forms.Button
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents _LastMessageTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _ResetButton As System.Windows.Forms.Button
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
    Private WithEvents _InitializeKnownStateButton As System.Windows.Forms.Button
    Private WithEvents _ResetLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _SelectiveDeviceClearButton As System.Windows.Forms.Button
    Private WithEvents _SessionTraceEnableCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents _Panel As System.Windows.Forms.Panel
    Private WithEvents _Layout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TitleLabel As System.Windows.Forms.Label
    Private WithEvents _ReadWriteTabPage As Windows.Forms.TabPage
    Private WithEvents _SimpleReadWriteControl As Instrument.SimpleReadWriteControl
End Class
