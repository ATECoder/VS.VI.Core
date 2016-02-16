<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> 
Partial Class Switchboard

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> 
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Switchboard))
        Me._ApplicationsListBoxLabel = New System.Windows.Forms.Label()
        Me._OpenButton = New System.Windows.Forms.Button()
        Me._ExitButton = New System.Windows.Forms.Button()
        Me._ApplicationsListBox = New System.Windows.Forms.ListBox()
        Me._Tooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        '_ApplicationsListBoxLabel
        '
        Me._ApplicationsListBoxLabel.Location = New System.Drawing.Point(15, 15)
        Me._ApplicationsListBoxLabel.Name = "_ApplicationsListBoxLabel"
        Me._ApplicationsListBoxLabel.Size = New System.Drawing.Size(345, 21)
        Me._ApplicationsListBoxLabel.TabIndex = 15
        Me._ApplicationsListBoxLabel.Text = "Select an item from the list and click Open"
        '
        '_OpenButton
        '
        Me._OpenButton.Font = New System.Drawing.Font(SystemFonts.MessageBoxFont.FontFamily, 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._OpenButton.Location = New System.Drawing.Point(414, 38)
        Me._OpenButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._OpenButton.Name = "_OpenButton"
        Me._OpenButton.Size = New System.Drawing.Size(87, 30)
        Me._OpenButton.TabIndex = 13
        Me._OpenButton.Text = "&Open..."
        '
        '_ExitButton
        '
        Me._ExitButton.Font = New System.Drawing.Font(SystemFonts.MessageBoxFont.FontFamily, 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ExitButton.Location = New System.Drawing.Point(414, 165)
        Me._ExitButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ExitButton.Name = "_ExitButton"
        Me._ExitButton.Size = New System.Drawing.Size(87, 30)
        Me._ExitButton.TabIndex = 12
        Me._ExitButton.Text = "E&xit"
        '
        '_ApplicationsListBox
        '
        Me._ApplicationsListBox.Font = New System.Drawing.Font(SystemFonts.MessageBoxFont.FontFamily, 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ApplicationsListBox.ItemHeight = 17
        Me._ApplicationsListBox.Location = New System.Drawing.Point(13, 38)
        Me._ApplicationsListBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ApplicationsListBox.Name = "_ApplicationsListBox"
        Me._ApplicationsListBox.Size = New System.Drawing.Size(382, 157)
        Me._ApplicationsListBox.TabIndex = 14
        Me._Tooltip.SetToolTip(Me._ApplicationsListBox, "Select an application to test")
        '
        'Switchboard
        '
        Me.ClientSize = New System.Drawing.Size(513, 217)
        Me.Controls.Add(Me._ApplicationsListBoxLabel)
        Me.Controls.Add(Me._OpenButton)
        Me.Controls.Add(Me._ExitButton)
        Me.Controls.Add(Me._ApplicationsListBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Switchboard"
        Me.Text = "Switchboard"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _ApplicationsListBoxLabel As System.Windows.Forms.Label
    Private WithEvents _OpenButton As System.Windows.Forms.Button
    Private WithEvents _ExitButton As System.Windows.Forms.Button
    Private WithEvents _ApplicationsListBox As System.Windows.Forms.ListBox
    Private WithEvents _Tooltip As System.Windows.Forms.ToolTip
End Class
