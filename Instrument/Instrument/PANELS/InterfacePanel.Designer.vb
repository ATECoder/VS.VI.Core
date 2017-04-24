<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class InterfacePanel

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
        Me._InstrumentChooser = New isr.VI.Instrument.ResourceSelectorConnector()
        Me._InterfaceChooserLabel = New System.Windows.Forms.Label()
        Me._InterfaceChooser = New isr.VI.Instrument.ResourceSelectorConnector()
        Me._InstrumentChooserLabel = New System.Windows.Forms.Label()
        Me._ClearSelectedResourceButton = New System.Windows.Forms.Button()
        Me._ClearAllResourcesButton = New System.Windows.Forms.Button()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._TraceMessagesBox = New isr.Core.Pith.TraceMessagesBox()
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._StatusStrip = New System.Windows.Forms.StatusStrip()
        Me._StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._Tabs.SuspendLayout()
        Me._ResourcesTabPage.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
        Me._StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ResourcesTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Tabs.Location = New System.Drawing.Point(0, 0)
        Me._Tabs.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(364, 303)
        Me._Tabs.TabIndex = 12
        '
        '_ResourcesTabPage
        '
        Me._ResourcesTabPage.Controls.Add(Me._InstrumentChooser)
        Me._ResourcesTabPage.Controls.Add(Me._InterfaceChooserLabel)
        Me._ResourcesTabPage.Controls.Add(Me._InterfaceChooser)
        Me._ResourcesTabPage.Controls.Add(Me._InstrumentChooserLabel)
        Me._ResourcesTabPage.Controls.Add(Me._ClearSelectedResourceButton)
        Me._ResourcesTabPage.Controls.Add(Me._ClearAllResourcesButton)
        Me._ResourcesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ResourcesTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ResourcesTabPage.Name = "_ResourcesTabPage"
        Me._ResourcesTabPage.Size = New System.Drawing.Size(356, 273)
        Me._ResourcesTabPage.TabIndex = 0
        Me._ResourcesTabPage.Text = "Resources"
        Me._ResourcesTabPage.UseVisualStyleBackColor = True
        '
        '_InstrumentChooser
        '
        Me._InstrumentChooser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._InstrumentChooser.BackColor = System.Drawing.Color.Transparent
        Me._InstrumentChooser.Clearable = False
        Me._InstrumentChooser.Connectable = False
        Me._InstrumentChooser.Enabled = False
        Me._InstrumentChooser.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._InstrumentChooser.Location = New System.Drawing.Point(22, 98)
        Me._InstrumentChooser.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._InstrumentChooser.Name = "_InstrumentChooser"
        Me._InstrumentChooser.Searchable = False
        Me._InstrumentChooser.Size = New System.Drawing.Size(313, 33)
        Me._InstrumentChooser.TabIndex = 12
        Me._ToolTip.SetToolTip(Me._InstrumentChooser, "Select an instrument resource and click the connect button.")
        '
        '_InterfaceChooserLabel
        '
        Me._InterfaceChooserLabel.AutoSize = True
        Me._InterfaceChooserLabel.Location = New System.Drawing.Point(22, 15)
        Me._InterfaceChooserLabel.Name = "_InterfaceChooserLabel"
        Me._InterfaceChooserLabel.Size = New System.Drawing.Size(71, 17)
        Me._InterfaceChooserLabel.TabIndex = 11
        Me._InterfaceChooserLabel.Text = "Interfaces: "
        Me._InterfaceChooserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_InterfaceChooser
        '
        Me._InterfaceChooser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._InterfaceChooser.BackColor = System.Drawing.Color.Transparent
        Me._InterfaceChooser.Clearable = False
        Me._InterfaceChooser.Connectable = False
        Me._InterfaceChooser.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._InterfaceChooser.Location = New System.Drawing.Point(22, 31)
        Me._InterfaceChooser.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._InterfaceChooser.Name = "_InterfaceChooser"
        Me._InterfaceChooser.Searchable = False
        Me._InterfaceChooser.Size = New System.Drawing.Size(313, 34)
        Me._InterfaceChooser.TabIndex = 10
        Me._ToolTip.SetToolTip(Me._InterfaceChooser, "Select and interface resource and click the connect button to list the instrument" &
        "s.")
        '
        '_InstrumentChooserLabel
        '
        Me._InstrumentChooserLabel.AutoSize = True
        Me._InstrumentChooserLabel.Location = New System.Drawing.Point(22, 81)
        Me._InstrumentChooserLabel.Name = "_InstrumentChooserLabel"
        Me._InstrumentChooserLabel.Size = New System.Drawing.Size(222, 17)
        Me._InstrumentChooserLabel.TabIndex = 6
        Me._InstrumentChooserLabel.Text = "Resources Attached to the Interface: "
        Me._InstrumentChooserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_ClearSelectedResourceButton
        '
        Me._ClearSelectedResourceButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ClearSelectedResourceButton.Enabled = False
        Me._ClearSelectedResourceButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ClearSelectedResourceButton.Location = New System.Drawing.Point(22, 151)
        Me._ClearSelectedResourceButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ClearSelectedResourceButton.Name = "_ClearSelectedResourceButton"
        Me._ClearSelectedResourceButton.Size = New System.Drawing.Size(313, 32)
        Me._ClearSelectedResourceButton.TabIndex = 8
        Me._ClearSelectedResourceButton.Text = "Clear Selected Resource"
        '
        '_ClearAllResourcesButton
        '
        Me._ClearAllResourcesButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ClearAllResourcesButton.Enabled = False
        Me._ClearAllResourcesButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ClearAllResourcesButton.Location = New System.Drawing.Point(22, 203)
        Me._ClearAllResourcesButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ClearAllResourcesButton.Name = "_ClearAllResourcesButton"
        Me._ClearAllResourcesButton.Size = New System.Drawing.Size(313, 32)
        Me._ClearAllResourcesButton.TabIndex = 9
        Me._ClearAllResourcesButton.Text = "Clear All Resources"
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me._TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(356, 273)
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
        Me._TraceMessagesBox.Size = New System.Drawing.Size(356, 273)
        Me._TraceMessagesBox.TabIndex = 0
        Me._TraceMessagesBox.TraceLevel = System.Diagnostics.TraceEventType.Verbose
        '
        '_StatusStrip
        '
        Me._StatusStrip.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusLabel})
        Me._StatusStrip.Location = New System.Drawing.Point(0, 281)
        Me._StatusStrip.Name = "_StatusStrip"
        Me._StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
        Me._StatusStrip.ShowItemToolTips = True
        Me._StatusStrip.Size = New System.Drawing.Size(364, 22)
        Me._StatusStrip.TabIndex = 14
        Me._StatusStrip.Text = "Status Strip"
        '
        '_StatusLabel
        '
        Me._StatusLabel.AutoToolTip = True
        Me._StatusLabel.Name = "_StatusLabel"
        Me._StatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._StatusLabel.Size = New System.Drawing.Size(347, 17)
        Me._StatusLabel.Spring = True
        Me._StatusLabel.Text = "<status>"
        Me._StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._StatusLabel.ToolTipText = "Status"
        '
        'InterfacePanel
        '
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me._StatusStrip)
        Me.Controls.Add(Me._Tabs)
        Me.Name = "InterfacePanel"
        Me.Size = New System.Drawing.Size(364, 303)
        Me._Tabs.ResumeLayout(False)
        Me._ResourcesTabPage.ResumeLayout(False)
        Me._ResourcesTabPage.PerformLayout()
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._StatusStrip.ResumeLayout(False)
        Me._StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _TraceMessagesBox As isr.Core.Pith.TraceMessagesBox
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _ResourcesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _InstrumentChooser As ResourceSelectorConnector
    Private WithEvents _InterfaceChooserLabel As System.Windows.Forms.Label
    Private WithEvents _InterfaceChooser As ResourceSelectorConnector
    Private WithEvents _InstrumentChooserLabel As System.Windows.Forms.Label
    Private WithEvents _ClearSelectedResourceButton As System.Windows.Forms.Button
    Private WithEvents _ClearAllResourcesButton As System.Windows.Forms.Button
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _StatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _StatusLabel As System.Windows.Forms.ToolStripStatusLabel

End Class
