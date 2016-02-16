<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class InterfaceForm

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Private WithEvents _TraceMessagesBox As isr.Core.Pith.TraceMessagesBox
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InterfaceForm))
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._InterfaceTabPage = New System.Windows.Forms.TabPage()
        Me._ConnectTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._InterfaceGroupBox = New System.Windows.Forms.GroupBox()
        Me._InterfacePanel = New isr.VI.Instrument.InterfacePanel()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._TraceMessagesBox = New isr.Core.Pith.TraceMessagesBox
        Me._StatusStrip = New System.Windows.Forms.StatusStrip()
        Me._StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._Tabs.SuspendLayout()
        Me._InterfaceTabPage.SuspendLayout()
        Me._ConnectTabLayout.SuspendLayout()
        Me._InterfaceGroupBox.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
        Me._StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        '_ToolTip
        '
        Me._ToolTip.IsBalloon = True
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._InterfaceTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.ItemSize = New System.Drawing.Size(42, 22)
        Me._Tabs.Location = New System.Drawing.Point(0, 0)
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(466, 541)
        Me._Tabs.TabIndex = 1

        '
        '_InterfaceTabPage
        '
        Me._InterfaceTabPage.Controls.Add(Me._ConnectTabLayout)
        Me._InterfaceTabPage.Location = New System.Drawing.Point(4, 22)
        Me._InterfaceTabPage.Name = "_InterfaceTabPage"
        Me._InterfaceTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me._InterfaceTabPage.Size = New System.Drawing.Size(458, 515)
        Me._InterfaceTabPage.TabIndex = 3
        Me._InterfaceTabPage.Text = "INTERFACE"
        Me._InterfaceTabPage.UseVisualStyleBackColor = True
        '
        '_ConnectTabLayout
        '
        Me._ConnectTabLayout.ColumnCount = 3
        Me._ConnectTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConnectTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ConnectTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConnectTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._ConnectTabLayout.Controls.Add(Me._InterfaceGroupBox, 1, 1)
        Me._ConnectTabLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ConnectTabLayout.Location = New System.Drawing.Point(3, 3)
        Me._ConnectTabLayout.Name = "_ConnectTabLayout"
        Me._ConnectTabLayout.RowCount = 3
        Me._ConnectTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConnectTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ConnectTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConnectTabLayout.Size = New System.Drawing.Size(452, 509)
        Me._ConnectTabLayout.TabIndex = 26
        '
        '_InterfaceGroupBox
        '
        Me._InterfaceGroupBox.Controls.Add(Me._InterfacePanel)
        Me._InterfaceGroupBox.Location = New System.Drawing.Point(22, 76)
        Me._InterfaceGroupBox.Name = "_InterfaceGroupBox"
        Me._InterfaceGroupBox.Size = New System.Drawing.Size(408, 357)
        Me._InterfaceGroupBox.TabIndex = 27
        Me._InterfaceGroupBox.TabStop = False
        Me._InterfaceGroupBox.Text = "INTERFACE:"
        '
        '_InterfacePanel
        '
        Me._InterfacePanel.BackColor = System.Drawing.Color.Transparent
        Me._InterfacePanel.Enabled = False
        Me._InterfacePanel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._InterfacePanel.Location = New System.Drawing.Point(13, 23)
        Me._InterfacePanel.Name = "_InterfacePanel"
        Me._InterfacePanel.Size = New System.Drawing.Size(368, 311)
        Me._InterfacePanel.TabIndex = 0
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me._TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 22)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(458, 515)
        Me._MessagesTabPage.TabIndex = 2
        Me._MessagesTabPage.Text = "Log"
        Me._MessagesTabPage.UseVisualStyleBackColor = True
        '
        '_TraceMessagesBox
        '
        Me._TraceMessagesBox.AcceptsReturn = True
        Me._TraceMessagesBox.BackColor = System.Drawing.SystemColors.Info
        Me._TraceMessagesBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me._TraceMessagesBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me._TraceMessagesBox.Location = New System.Drawing.Point(0, 0)
        Me._TraceMessagesBox.MaxLength = 0
        Me._TraceMessagesBox.Multiline = True
        Me._TraceMessagesBox.Name = "_TraceMessagesBox"
        Me._TraceMessagesBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._TraceMessagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me._TraceMessagesBox.Size = New System.Drawing.Size(458, 515)
        Me._TraceMessagesBox.TabIndex = 15
        '
        '_StatusStrip
        '
        Me._StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusLabel})
        Me._StatusStrip.Location = New System.Drawing.Point(0, 541)
        Me._StatusStrip.Name = "_StatusStrip"
        Me._StatusStrip.Size = New System.Drawing.Size(466, 22)
        Me._StatusStrip.TabIndex = 2
        Me._StatusStrip.Text = "Status Strip"
        '
        '_StatusToolStripLabel
        '
        Me._StatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StatusLabel.Name = "_StatusLabel"
        Me._StatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._StatusLabel.Size = New System.Drawing.Size(451, 17)
        Me._StatusLabel.Spring = True
        Me._StatusLabel.Text = "Ready"
        '
        'TestPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(466, 563)
        Me.Controls.Add(Me._Tabs)
        Me.Controls.Add(Me._StatusStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(297, 150)
        Me.MaximizeBox = False
        Me.Name = "TestPanel"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Mini VISA Tester"
        Me._Tabs.ResumeLayout(False)
        Me._InterfaceTabPage.ResumeLayout(False)
        Me._ConnectTabLayout.ResumeLayout(False)
        Me._InterfaceGroupBox.ResumeLayout(False)
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._StatusStrip.ResumeLayout(False)
        Me._StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _InterfaceTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ConnectTabLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _InterfaceGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents _InterfacePanel As isr.VI.Instrument.InterfacePanel
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _StatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _StatusLabel As System.Windows.Forms.ToolStripStatusLabel

End Class

