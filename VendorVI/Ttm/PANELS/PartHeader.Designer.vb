<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PartHeader

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                Me.releaseResources()
                If components IsNot Nothing Then
                    components.Dispose()
                End If
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
        Me._TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me._PartNumberTextBoxLabel = New System.Windows.Forms.Label()
        Me._PartNumberTextBox = New System.Windows.Forms.TextBox()
        Me._PartSerialNumberTextBoxLabel = New System.Windows.Forms.Label()
        Me._PartSerialNumberTextBox = New System.Windows.Forms.TextBox()
        Me._TableLayoutPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        '_TableLayoutPanel
        '
        Me._TableLayoutPanel.BackColor = System.Drawing.Color.Transparent
        Me._TableLayoutPanel.ColumnCount = 5
        Me._TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._TableLayoutPanel.Controls.Add(Me._PartNumberTextBoxLabel, 0, 1)
        Me._TableLayoutPanel.Controls.Add(Me._PartNumberTextBox, 1, 1)
        Me._TableLayoutPanel.Controls.Add(Me._PartSerialNumberTextBoxLabel, 3, 1)
        Me._TableLayoutPanel.Controls.Add(Me._PartSerialNumberTextBox, 4, 1)
        Me._TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me._TableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me._TableLayoutPanel.Name = "_TableLayoutPanel"
        Me._TableLayoutPanel.RowCount = 3
        Me._TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._TableLayoutPanel.Size = New System.Drawing.Size(670, 21)
        Me._TableLayoutPanel.TabIndex = 0
        '
        '_PartNumberTextBoxLabel
        '
        Me._PartNumberTextBoxLabel.AutoSize = True
        Me._PartNumberTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._PartNumberTextBoxLabel.Location = New System.Drawing.Point(3, 2)
        Me._PartNumberTextBoxLabel.Margin = New System.Windows.Forms.Padding(3)
        Me._PartNumberTextBoxLabel.Name = "_PartNumberTextBoxLabel"
        Me._PartNumberTextBoxLabel.Size = New System.Drawing.Size(98, 17)
        Me._PartNumberTextBoxLabel.TabIndex = 1
        Me._PartNumberTextBoxLabel.Text = "PART NUMBER:"
        '
        '_PartNumberTextBox
        '
        Me._PartNumberTextBox.BackColor = System.Drawing.SystemColors.Desktop
        Me._PartNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._PartNumberTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PartNumberTextBox.ForeColor = System.Drawing.Color.LawnGreen
        Me._PartNumberTextBox.Location = New System.Drawing.Point(107, 2)
        Me._PartNumberTextBox.Name = "_PartNumberTextBox"
        Me._PartNumberTextBox.Size = New System.Drawing.Size(164, 18)
        Me._PartNumberTextBox.TabIndex = 2
        Me._PartNumberTextBox.Text = "10"
        '
        '_PartSerialNumberTextBoxLabel
        '
        Me._PartSerialNumberTextBoxLabel.AutoSize = True
        Me._PartSerialNumberTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._PartSerialNumberTextBoxLabel.Location = New System.Drawing.Point(541, 2)
        Me._PartSerialNumberTextBoxLabel.Margin = New System.Windows.Forms.Padding(3)
        Me._PartSerialNumberTextBoxLabel.Name = "_PartSerialNumberTextBoxLabel"
        Me._PartSerialNumberTextBoxLabel.Size = New System.Drawing.Size(33, 17)
        Me._PartSerialNumberTextBoxLabel.TabIndex = 3
        Me._PartSerialNumberTextBoxLabel.Text = "S/N:"
        '
        '_PartSerialNumberTextBox
        '
        Me._PartSerialNumberTextBox.BackColor = System.Drawing.SystemColors.Desktop
        Me._PartSerialNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._PartSerialNumberTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PartSerialNumberTextBox.ForeColor = System.Drawing.Color.LawnGreen
        Me._PartSerialNumberTextBox.Location = New System.Drawing.Point(580, 2)
        Me._PartSerialNumberTextBox.Name = "_PartSerialNumberTextBox"
        Me._PartSerialNumberTextBox.Size = New System.Drawing.Size(87, 18)
        Me._PartSerialNumberTextBox.TabIndex = 4
        Me._PartSerialNumberTextBox.Text = "10"
        '
        'PartHeader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.Controls.Add(Me._TableLayoutPanel)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "PartHeader"
        Me.Size = New System.Drawing.Size(670, 21)
        Me._TableLayoutPanel.ResumeLayout(False)
        Me._TableLayoutPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _TableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _PartNumberTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _PartNumberTextBox As System.Windows.Forms.TextBox
    Private WithEvents _PartSerialNumberTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _PartSerialNumberTextBox As System.Windows.Forms.TextBox

End Class
