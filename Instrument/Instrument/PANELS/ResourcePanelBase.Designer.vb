<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ResourcePanelBase

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="StatusPanel")> <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="IdentityPanel")> <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Connector = New isr.VI.Instrument.ResourceSelectorConnector()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.IdentityLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusRegisterLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StandardRegisterLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TipsTooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.TraceMessagesBox = New isr.Core.Pith.TraceMessagesBox()
        Me.StatusStrip.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Connector
        '
        Me.Connector.BackColor = System.Drawing.Color.Transparent
        Me.Connector.Clearable = False
        Me.Connector.Connectable = False
        Me.Connector.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Connector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Connector.Location = New System.Drawing.Point(0, 10)
        Me.Connector.Name = "Connector"
        Me.Connector.Searchable = False
        Me.Connector.Size = New System.Drawing.Size(364, 40)
        Me.Connector.TabIndex = 14
        '
        'StatusStrip
        '
        Me.StatusStrip.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel, Me.IdentityLabel, Me.StatusRegisterLabel, Me.StandardRegisterLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 50)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
        Me.StatusStrip.ShowItemToolTips = True
        Me.StatusStrip.Size = New System.Drawing.Size(364, 22)
        Me.StatusStrip.TabIndex = 15
        Me.StatusStrip.Text = "StatusStrip1"
        '
        'StatusLabel
        '
        Me.StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.StatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me.StatusLabel.Size = New System.Drawing.Size(214, 17)
        Me.StatusLabel.Spring = True
        Me.StatusLabel.Text = "<Status>"
        Me.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.StatusLabel.ToolTipText = "Status"
        '
        'IdentityLabel
        '
        Me.IdentityLabel.Name = "IdentityLabel"
        Me.IdentityLabel.Size = New System.Drawing.Size(30, 17)
        Me.IdentityLabel.Text = "<I>"
        Me.IdentityLabel.ToolTipText = "Identity"
        '
        'StatusRegisterLabel
        '
        Me.StatusRegisterLabel.Name = "StatusRegisterLabel"
        Me.StatusRegisterLabel.Size = New System.Drawing.Size(36, 17)
        Me.StatusRegisterLabel.Text = "0x00"
        Me.StatusRegisterLabel.ToolTipText = "Status Register Value"
        '
        'StandardRegisterLabel
        '
        Me.StandardRegisterLabel.Name = "StandardRegisterLabel"
        Me.StandardRegisterLabel.Size = New System.Drawing.Size(36, 17)
        Me.StandardRegisterLabel.Text = "0x00"
        Me.StandardRegisterLabel.ToolTipText = "Standard Register Value"
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'TraceMessagesBox
        '
        Me.TraceMessagesBox.BackColor = System.Drawing.SystemColors.Info
        Me.TraceMessagesBox.CausesValidation = False
        Me.TraceMessagesBox.Font = New System.Drawing.Font("Consolas", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TraceMessagesBox.Location = New System.Drawing.Point(0, 0)
        Me.TraceMessagesBox.Multiline = True
        Me.TraceMessagesBox.Name = "TraceMessagesBox"
        Me.TraceMessagesBox.PresetCount = 100
        Me.TraceMessagesBox.ReadOnly = True
        Me.TraceMessagesBox.ResetCount = 200
        Me.TraceMessagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TraceMessagesBox.Size = New System.Drawing.Size(150, 150)
        Me.TraceMessagesBox.TabIndex = 0
        '
        'ResourcePanelBase
        '
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.Connector)
        Me.Controls.Add(Me.StatusStrip)
        Me.Name = "ResourcePanelBase"
        Me.Size = New System.Drawing.Size(364, 72)
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Protected WithEvents Connector As ResourceSelectorConnector
    Protected WithEvents TipsTooltip As System.Windows.Forms.ToolTip
    Protected WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Protected WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Protected WithEvents IdentityLabel As System.Windows.Forms.ToolStripStatusLabel
    Protected WithEvents StatusRegisterLabel As System.Windows.Forms.ToolStripStatusLabel
    Protected WithEvents StandardRegisterLabel As System.Windows.Forms.ToolStripStatusLabel
    Protected WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
    Protected WithEvents TraceMessagesBox As isr.Core.Pith.TraceMessagesBox

End Class
