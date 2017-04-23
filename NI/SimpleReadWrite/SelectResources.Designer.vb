<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectResources
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me._ResourceStringsListBoxLabel = New System.Windows.Forms.Label()
        Me._AvailableResourcesListBoxLabel = New System.Windows.Forms.Label()
        Me.closeButton = New System.Windows.Forms.Button()
        Me.okButton = New System.Windows.Forms.Button()
        Me._AvailableResourcesListBox = New System.Windows.Forms.ListBox()
        Me._ResourceStringsListBox = New System.Windows.Forms.ListBox()
        Me._AddButton = New System.Windows.Forms.Button()
        Me._RemoveButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        '_ResourceStringsListBoxLabel
        '
        Me._ResourceStringsListBoxLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._ResourceStringsListBoxLabel.AutoSize = True
        Me._ResourceStringsListBoxLabel.Location = New System.Drawing.Point(1, 163)
        Me._ResourceStringsListBoxLabel.Name = "_ResourceStringsListBoxLabel"
        Me._ResourceStringsListBoxLabel.Size = New System.Drawing.Size(91, 13)
        Me._ResourceStringsListBoxLabel.TabIndex = 12
        Me._ResourceStringsListBoxLabel.Text = "Resource Strings:"
        '
        '_AvailableResourcesListBoxLabel
        '
        Me._AvailableResourcesListBoxLabel.AutoSize = True
        Me._AvailableResourcesListBoxLabel.Location = New System.Drawing.Point(1, 4)
        Me._AvailableResourcesListBoxLabel.Name = "_AvailableResourcesListBoxLabel"
        Me._AvailableResourcesListBoxLabel.Size = New System.Drawing.Size(107, 13)
        Me._AvailableResourcesListBoxLabel.TabIndex = 11
        Me._AvailableResourcesListBoxLabel.Text = "Available Resources:"
        '
        'closeButton
        '
        Me.closeButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.closeButton.Location = New System.Drawing.Point(213, 267)
        Me.closeButton.Name = "closeButton"
        Me.closeButton.Size = New System.Drawing.Size(66, 25)
        Me.closeButton.TabIndex = 9
        Me.closeButton.Text = "Cancel"
        '
        'okButton
        '
        Me.okButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.okButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.okButton.Location = New System.Drawing.Point(143, 267)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(66, 25)
        Me.okButton.TabIndex = 8
        Me.okButton.Text = "OK"
        '
        '_AvailableResourcesListBox
        '
        Me._AvailableResourcesListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._AvailableResourcesListBox.Location = New System.Drawing.Point(4, 17)
        Me._AvailableResourcesListBox.Name = "_AvailableResourcesListBox"
        Me._AvailableResourcesListBox.Size = New System.Drawing.Size(278, 134)
        Me._AvailableResourcesListBox.TabIndex = 7
        '
        '_ResourceStringsListBox
        '
        Me._ResourceStringsListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ResourceStringsListBox.FormattingEnabled = True
        Me._ResourceStringsListBox.Location = New System.Drawing.Point(4, 179)
        Me._ResourceStringsListBox.Name = "_ResourceStringsListBox"
        Me._ResourceStringsListBox.Size = New System.Drawing.Size(278, 82)
        Me._ResourceStringsListBox.TabIndex = 13
        '
        '_AddButton
        '
        Me._AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._AddButton.Location = New System.Drawing.Point(4, 267)
        Me._AddButton.Name = "_AddButton"
        Me._AddButton.Size = New System.Drawing.Size(47, 25)
        Me._AddButton.TabIndex = 14
        Me._AddButton.Text = "Add"
        Me._AddButton.UseVisualStyleBackColor = True
        '
        '_RemoveButton
        '
        Me._RemoveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._RemoveButton.Location = New System.Drawing.Point(55, 267)
        Me._RemoveButton.Name = "_RemoveButton"
        Me._RemoveButton.Size = New System.Drawing.Size(61, 25)
        Me._RemoveButton.TabIndex = 15
        Me._RemoveButton.Text = "Remove"
        Me._RemoveButton.UseVisualStyleBackColor = True
        '
        'SelectResources
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 297)
        Me.Controls.Add(Me._RemoveButton)
        Me.Controls.Add(Me._AddButton)
        Me.Controls.Add(Me._ResourceStringsListBox)
        Me.Controls.Add(Me._ResourceStringsListBoxLabel)
        Me.Controls.Add(Me._AvailableResourcesListBoxLabel)
        Me.Controls.Add(Me.closeButton)
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me._AvailableResourcesListBox)
        Me.Name = "SelectResources"
        Me.Text = "Select Resources"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents _ResourceStringsListBoxLabel As Label
    Private WithEvents _AvailableResourcesListBoxLabel As Label
    Private WithEvents CloseButton As Button
    Private WithEvents OkButton As Button
    Private WithEvents _AvailableResourcesListBox As ListBox
    Private WithEvents _ResourceStringsListBox As ListBox
    Private WithEvents _AddButton As Button
    Private WithEvents _RemoveButton As Button
End Class
