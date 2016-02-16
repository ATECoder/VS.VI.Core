Imports System.Drawing
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ResourceSelectorDialog

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me._ResourceNameTextBoxLabel = New System.Windows.Forms.Label()
        Me._AvailableResourcesListBoxLabel = New System.Windows.Forms.Label()
        Me._ResourceNameTextBox = New System.Windows.Forms.TextBox()
        Me._CancelButton = New System.Windows.Forms.Button()
        Me._AcceptButton = New System.Windows.Forms.Button()
        Me._AvailableResourcesListBox = New System.Windows.Forms.ListBox()
        Me._RefreshButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        '_ResourceNameTextBoxLabel
        '
        Me._ResourceNameTextBoxLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._ResourceNameTextBoxLabel.Location = New System.Drawing.Point(6, 216)
        Me._ResourceNameTextBoxLabel.Name = "_ResourceNameTextBoxLabel"
        Me._ResourceNameTextBoxLabel.Size = New System.Drawing.Size(125, 20)
        Me._ResourceNameTextBoxLabel.TabIndex = 12
        Me._ResourceNameTextBoxLabel.Text = "Resource Name:"
        '
        '_AvailableResourcesListBoxLabel
        '
        Me._AvailableResourcesListBoxLabel.AutoSize = True
        Me._AvailableResourcesListBoxLabel.Location = New System.Drawing.Point(6, 8)
        Me._AvailableResourcesListBoxLabel.Name = "_AvailableResourcesListBoxLabel"
        Me._AvailableResourcesListBoxLabel.Size = New System.Drawing.Size(127, 17)
        Me._AvailableResourcesListBoxLabel.TabIndex = 11
        Me._AvailableResourcesListBoxLabel.Text = "Available Resources:"
        '
        '_ResourceNameTextBox
        '
        Me._ResourceNameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ResourceNameTextBox.Font = New System.Drawing.Font(Me.Font, FontStyle.Bold)
        Me._ResourceNameTextBox.Location = New System.Drawing.Point(6, 237)
        Me._ResourceNameTextBox.Name = "_ResourceNameTextBox"
        Me._ResourceNameTextBox.Size = New System.Drawing.Size(319, 25)
        Me._ResourceNameTextBox.TabIndex = 10
        Me._ResourceNameTextBox.Text = "GPIB0::2::INSTR"
        '
        '_CancelButton
        '
        Me._CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me._CancelButton.Font = New System.Drawing.Font(Me.Font, FontStyle.Bold)
        Me._CancelButton.Location = New System.Drawing.Point(96, 277)
        Me._CancelButton.Name = "_CancelButton"
        Me._CancelButton.Size = New System.Drawing.Size(90, 33)
        Me._CancelButton.TabIndex = 9
        Me._CancelButton.Text = "Cancel"
        '
        '_AcceptButton
        '
        Me._AcceptButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._AcceptButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me._AcceptButton.Font = New System.Drawing.Font(Me.Font, FontStyle.Bold)
        Me._AcceptButton.Location = New System.Drawing.Point(6, 277)
        Me._AcceptButton.Name = "_AcceptButton"
        Me._AcceptButton.Size = New System.Drawing.Size(90, 33)
        Me._AcceptButton.TabIndex = 8
        Me._AcceptButton.Text = "OK"
        '
        '_AvailableResourcesListBox
        '
        Me._AvailableResourcesListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._AvailableResourcesListBox.Font = New System.Drawing.Font(Me.Font, FontStyle.Bold)
        Me._AvailableResourcesListBox.ItemHeight = 17
        Me._AvailableResourcesListBox.Location = New System.Drawing.Point(6, 27)
        Me._AvailableResourcesListBox.Name = "_AvailableResourcesListBox"
        Me._AvailableResourcesListBox.Size = New System.Drawing.Size(319, 157)
        Me._AvailableResourcesListBox.TabIndex = 7
        '
        '_RefreshButton
        '
        Me._RefreshButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._RefreshButton.Font = New System.Drawing.Font(Me.Font, FontStyle.Bold)
        Me._RefreshButton.Location = New System.Drawing.Point(236, 186)
        Me._RefreshButton.Name = "_RefreshButton"
        Me._RefreshButton.Size = New System.Drawing.Size(90, 33)
        Me._RefreshButton.TabIndex = 9
        Me._RefreshButton.Text = "Refresh"
        '
        'ResourceSelectorDialog
        '
        Me.AcceptButton = Me._AcceptButton
        Me.CancelButton = Me._CancelButton
        Me.ClientSize = New System.Drawing.Size(332, 317)
        Me.Controls.Add(Me._ResourceNameTextBoxLabel)
        Me.Controls.Add(Me._AvailableResourcesListBoxLabel)
        Me.Controls.Add(Me._ResourceNameTextBox)
        Me.Controls.Add(Me._RefreshButton)
        Me.Controls.Add(Me._CancelButton)
        Me.Controls.Add(Me._AcceptButton)
        Me.Controls.Add(Me._AvailableResourcesListBox)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(204, 311)
        Me.Name = "ResourceSelectorDialog"
        Me.Text = "Select Resource"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _ResourceNameTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _AvailableResourcesListBoxLabel As System.Windows.Forms.Label
    Private WithEvents _ResourceNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents _CancelButton As System.Windows.Forms.Button
    Private WithEvents _AcceptButton As System.Windows.Forms.Button
    Private WithEvents _AvailableResourcesListBox As System.Windows.Forms.ListBox
    Private WithEvents _RefreshButton As System.Windows.Forms.Button
End Class
