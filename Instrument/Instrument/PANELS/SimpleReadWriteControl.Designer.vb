<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SimpleReadWriteControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me._ClearSessionButton = New System.Windows.Forms.Button()
        Me._WriteComboBox = New System.Windows.Forms.ComboBox()
        Me._TimingUnitsLabel = New System.Windows.Forms.Label()
        Me._TimingTextBox = New System.Windows.Forms.TextBox()
        Me._EraseButton = New System.Windows.Forms.Button()
        Me._ReadTextBox = New System.Windows.Forms.TextBox()
        Me._ReadTextBoxLabel = New System.Windows.Forms.Label()
        Me._WriteTextBoxLabel = New System.Windows.Forms.Label()
        Me._ReadButton = New System.Windows.Forms.Button()
        Me._WriteButton = New System.Windows.Forms.Button()
        Me._QueryButton = New System.Windows.Forms.Button()
        Me._WriteComboBoxLabel = New System.Windows.Forms.Label()
        Me._ControlsLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._ControlsLayout.SuspendLayout()
        Me.SuspendLayout()
        '
        '_ClearSessionButton
        '
        Me._ClearSessionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ClearSessionButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ClearSessionButton.Location = New System.Drawing.Point(225, 4)
        Me._ClearSessionButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ClearSessionButton.Name = "_ClearSessionButton"
        Me._ClearSessionButton.Size = New System.Drawing.Size(64, 31)
        Me._ClearSessionButton.TabIndex = 57
        Me._ClearSessionButton.Text = "Clear"
        Me._ClearSessionButton.UseVisualStyleBackColor = True
        '
        '_WriteComboBox
        '
        Me._WriteComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._WriteComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._WriteComboBox.FormattingEnabled = True
        Me._WriteComboBox.Location = New System.Drawing.Point(3, 22)
        Me._WriteComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._WriteComboBox.Name = "_WriteComboBox"
        Me._WriteComboBox.Size = New System.Drawing.Size(292, 25)
        Me._WriteComboBox.TabIndex = 53
        '
        '_TimingUnitsLabel
        '
        Me._TimingUnitsLabel.AutoSize = True
        Me._TimingUnitsLabel.Location = New System.Drawing.Point(198, 102)
        Me._TimingUnitsLabel.Name = "_TimingUnitsLabel"
        Me._TimingUnitsLabel.Size = New System.Drawing.Size(25, 17)
        Me._TimingUnitsLabel.TabIndex = 61
        Me._TimingUnitsLabel.Text = "ms"
        '
        '_TimingTextBox
        '
        Me._TimingTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._TimingTextBox.Location = New System.Drawing.Point(139, 94)
        Me._TimingTextBox.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me._TimingTextBox.Name = "_TimingTextBox"
        Me._TimingTextBox.Size = New System.Drawing.Size(56, 25)
        Me._TimingTextBox.TabIndex = 60
        '
        '_EraseButton
        '
        Me._EraseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._EraseButton.Enabled = False
        Me._EraseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._EraseButton.Location = New System.Drawing.Point(232, 88)
        Me._EraseButton.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me._EraseButton.Name = "_EraseButton"
        Me._EraseButton.Size = New System.Drawing.Size(64, 31)
        Me._EraseButton.TabIndex = 62
        Me._EraseButton.Text = "Erase"
        '
        '_ReadTextBox
        '
        Me._ReadTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ReadTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ReadTextBox.Location = New System.Drawing.Point(3, 121)
        Me._ReadTextBox.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me._ReadTextBox.Multiline = True
        Me._ReadTextBox.Name = "_ReadTextBox"
        Me._ReadTextBox.ReadOnly = True
        Me._ReadTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me._ReadTextBox.Size = New System.Drawing.Size(293, 241)
        Me._ReadTextBox.TabIndex = 59
        Me._ReadTextBox.TabStop = False
        '
        '_ReadTextBoxLabel
        '
        Me._ReadTextBoxLabel.AutoSize = True
        Me._ReadTextBoxLabel.Location = New System.Drawing.Point(3, 102)
        Me._ReadTextBoxLabel.Name = "_ReadTextBoxLabel"
        Me._ReadTextBoxLabel.Size = New System.Drawing.Size(120, 17)
        Me._ReadTextBoxLabel.TabIndex = 58
        Me._ReadTextBoxLabel.Text = "Message Received:"
        '
        '_WriteTextBoxLabel
        '
        Me._WriteTextBoxLabel.Location = New System.Drawing.Point(-13, -17)
        Me._WriteTextBoxLabel.Name = "_WriteTextBoxLabel"
        Me._WriteTextBoxLabel.Size = New System.Drawing.Size(146, 24)
        Me._WriteTextBoxLabel.TabIndex = 52
        Me._WriteTextBoxLabel.Text = "Message to Send:"
        '
        '_ReadButton
        '
        Me._ReadButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ReadButton.Enabled = False
        Me._ReadButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ReadButton.Location = New System.Drawing.Point(151, 5)
        Me._ReadButton.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me._ReadButton.Name = "_ReadButton"
        Me._ReadButton.Size = New System.Drawing.Size(64, 31)
        Me._ReadButton.TabIndex = 56
        Me._ReadButton.Text = "Read"
        '
        '_WriteButton
        '
        Me._WriteButton.Enabled = False
        Me._WriteButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._WriteButton.Location = New System.Drawing.Point(77, 5)
        Me._WriteButton.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me._WriteButton.Name = "_WriteButton"
        Me._WriteButton.Size = New System.Drawing.Size(64, 31)
        Me._WriteButton.TabIndex = 55
        Me._WriteButton.Text = "Write"
        '
        '_QueryButton
        '
        Me._QueryButton.Enabled = False
        Me._QueryButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._QueryButton.Location = New System.Drawing.Point(3, 5)
        Me._QueryButton.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me._QueryButton.Name = "_QueryButton"
        Me._QueryButton.Size = New System.Drawing.Size(64, 31)
        Me._QueryButton.TabIndex = 54
        Me._QueryButton.Text = "Query"
        '
        '_WriteComboBoxLabel
        '
        Me._WriteComboBoxLabel.Location = New System.Drawing.Point(3, 3)
        Me._WriteComboBoxLabel.Name = "_WriteComboBoxLabel"
        Me._WriteComboBoxLabel.Size = New System.Drawing.Size(125, 18)
        Me._WriteComboBoxLabel.TabIndex = 63
        Me._WriteComboBoxLabel.Text = "Message to Send:"
        '
        '_ControlsLayout
        '
        Me._ControlsLayout.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ControlsLayout.ColumnCount = 7
        Me._ControlsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ControlsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me._ControlsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ControlsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me._ControlsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ControlsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me._ControlsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ControlsLayout.Controls.Add(Me._QueryButton, 0, 0)
        Me._ControlsLayout.Controls.Add(Me._WriteButton, 2, 0)
        Me._ControlsLayout.Controls.Add(Me._ClearSessionButton, 6, 0)
        Me._ControlsLayout.Controls.Add(Me._ReadButton, 4, 0)
        Me._ControlsLayout.Location = New System.Drawing.Point(3, 47)
        Me._ControlsLayout.Name = "_ControlsLayout"
        Me._ControlsLayout.RowCount = 1
        Me._ControlsLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ControlsLayout.Size = New System.Drawing.Size(292, 40)
        Me._ControlsLayout.TabIndex = 64
        '
        'SimpleReadWriteControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me._ControlsLayout)
        Me.Controls.Add(Me._WriteComboBoxLabel)
        Me.Controls.Add(Me._WriteComboBox)
        Me.Controls.Add(Me._TimingUnitsLabel)
        Me.Controls.Add(Me._TimingTextBox)
        Me.Controls.Add(Me._EraseButton)
        Me.Controls.Add(Me._ReadTextBox)
        Me.Controls.Add(Me._ReadTextBoxLabel)
        Me.Controls.Add(Me._WriteTextBoxLabel)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "SimpleReadWriteControl"
        Me.Size = New System.Drawing.Size(298, 365)
        Me._ControlsLayout.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents _ClearSessionButton As Windows.Forms.Button
    Private WithEvents _WriteComboBox As Windows.Forms.ComboBox
    Private WithEvents _TimingUnitsLabel As Windows.Forms.Label
    Private WithEvents _TimingTextBox As Windows.Forms.TextBox
    Private WithEvents _EraseButton As Windows.Forms.Button
    Private WithEvents _ReadTextBox As Windows.Forms.TextBox
    Private WithEvents _ReadTextBoxLabel As Windows.Forms.Label
    Private WithEvents _WriteTextBoxLabel As Windows.Forms.Label
    Private WithEvents _ReadButton As Windows.Forms.Button
    Private WithEvents _WriteButton As Windows.Forms.Button
    Private WithEvents _QueryButton As Windows.Forms.Button
    Private WithEvents _WriteComboBoxLabel As Windows.Forms.Label
    Private WithEvents _ControlsLayout As Windows.Forms.TableLayoutPanel
End Class
