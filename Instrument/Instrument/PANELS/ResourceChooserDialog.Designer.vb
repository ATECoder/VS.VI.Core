Imports System.Drawing
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ResourceChooserDialog

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me._ResourceNameSelectorConnector = New isr.VI.Instrument.ResourceSelectorConnector()
        Me._CancelButton = New System.Windows.Forms.Button()
        Me._AcceptButton = New System.Windows.Forms.Button()
        Me._ResourceNameSelectorConnectorLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        '_ResourceNameSelectorConnector
        '
        Me._ResourceNameSelectorConnector.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ResourceNameSelectorConnector.BackColor = System.Drawing.Color.Transparent
        Me._ResourceNameSelectorConnector.Clearable = False
        Me._ResourceNameSelectorConnector.Connectable = False
        Me._ResourceNameSelectorConnector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ResourceNameSelectorConnector.Location = New System.Drawing.Point(0, 35)
        Me._ResourceNameSelectorConnector.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ResourceNameSelectorConnector.Name = "_ResourceNameSelectorConnector"
        Me._ResourceNameSelectorConnector.Searchable = False
        Me._ResourceNameSelectorConnector.Size = New System.Drawing.Size(338, 28)
        Me._ResourceNameSelectorConnector.TabIndex = 28
        '
        '_CancelButton
        '
        Me._CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me._CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me._CancelButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._CancelButton.Location = New System.Drawing.Point(79, 70)
        Me._CancelButton.Name = "_CancelButton"
        Me._CancelButton.Size = New System.Drawing.Size(87, 33)
        Me._CancelButton.TabIndex = 27
        Me._CancelButton.Text = "&Cancel"
        '
        '_AcceptButton
        '
        Me._AcceptButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._AcceptButton.Enabled = False
        Me._AcceptButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me._AcceptButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._AcceptButton.Location = New System.Drawing.Point(173, 70)
        Me._AcceptButton.Name = "_AcceptButton"
        Me._AcceptButton.Size = New System.Drawing.Size(87, 33)
        Me._AcceptButton.TabIndex = 26
        Me._AcceptButton.Text = "&OK"
        '
        '_ResourceNameSelectorConnectorLabel
        '
        Me._ResourceNameSelectorConnectorLabel.BackColor = System.Drawing.SystemColors.Control
        Me._ResourceNameSelectorConnectorLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._ResourceNameSelectorConnectorLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me._ResourceNameSelectorConnectorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me._ResourceNameSelectorConnectorLabel.ForeColor = System.Drawing.SystemColors.WindowText
        Me._ResourceNameSelectorConnectorLabel.Location = New System.Drawing.Point(0, 0)
        Me._ResourceNameSelectorConnectorLabel.Name = "_ResourceNameSelectorConnectorLabel"
        Me._ResourceNameSelectorConnectorLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._ResourceNameSelectorConnectorLabel.Size = New System.Drawing.Size(340, 34)
        Me._ResourceNameSelectorConnectorLabel.TabIndex = 25
        Me._ResourceNameSelectorConnectorLabel.Text = "Click the search button and then select a resource  from the drop down list"
        Me._ResourceNameSelectorConnectorLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'ResourceChooserDialog
        '
        Me.AcceptButton = Me._AcceptButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.CancelButton = Me._CancelButton
        Me.ClientSize = New System.Drawing.Size(340, 108)
        Me.Controls.Add(Me._ResourceNameSelectorConnector)
        Me.Controls.Add(Me._CancelButton)
        Me.Controls.Add(Me._AcceptButton)
        Me.Controls.Add(Me._ResourceNameSelectorConnectorLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "ResourceChooserDialog"
        Me.Text = "Select a board"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _ResourceNameSelectorConnector As ResourceSelectorConnector
    Private WithEvents _CancelButton As System.Windows.Forms.Button
    Private WithEvents _AcceptButton As System.Windows.Forms.Button
    Private WithEvents _ResourceNameSelectorConnectorLabel As System.Windows.Forms.Label
End Class
