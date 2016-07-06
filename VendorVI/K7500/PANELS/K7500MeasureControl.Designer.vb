Imports isr.Core.Pith
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class K7500MeasureControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me._MeterRangeNumeric = New isr.Core.Controls.NumericUpDown()
        Me._MeterCurrentNumeric = New isr.Core.Controls.NumericUpDown()
        Me._MeterRangeNumericLabel = New System.Windows.Forms.Label()
        Me._MeterCurrentNumericLabel = New System.Windows.Forms.Label()
        Me._TriggerDelayNumericLabel = New System.Windows.Forms.Label()
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._TriggerDelayNumeric = New isr.Core.Controls.NumericUpDown()
        Me._MeterRangeComboBox = New isr.Core.Controls.ComboBox()
        Me._MeterRangeComboBoxLabel = New System.Windows.Forms.Label()
        CType(Me._MeterRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._MeterCurrentNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_MeterRangeNumeric
        '
        Me._MeterRangeNumeric.DecimalPlaces = 3
        Me._MeterRangeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._MeterRangeNumeric.Location = New System.Drawing.Point(126, 43)
        Me._MeterRangeNumeric.Maximum = New Decimal(New Integer() {1000000000, 0, 0, 0})
        Me._MeterRangeNumeric.Name = "_MeterRangeNumeric"
        Me._MeterRangeNumeric.ReadOnly = True
        Me._MeterRangeNumeric.ReadOnlyBackColor = System.Drawing.Color.Empty
        Me._MeterRangeNumeric.ReadOnlyForeColor = System.Drawing.Color.Empty
        Me._MeterRangeNumeric.ReadWriteBackColor = System.Drawing.Color.Empty
        Me._MeterRangeNumeric.ReadWriteForeColor = System.Drawing.Color.Empty
        Me._MeterRangeNumeric.Size = New System.Drawing.Size(114, 25)
        Me._MeterRangeNumeric.TabIndex = 3
        Me._MeterRangeNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._MeterRangeNumeric.Value = New Decimal(New Integer() {2000000, 0, 0, 0})
        '
        '_MeterCurrentNumeric
        '
        Me._MeterCurrentNumeric.DecimalPlaces = 7
        Me._MeterCurrentNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._MeterCurrentNumeric.Location = New System.Drawing.Point(323, 43)
        Me._MeterCurrentNumeric.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me._MeterCurrentNumeric.Name = "_MeterCurrentNumeric"
        Me._MeterCurrentNumeric.ReadOnly = True
        Me._MeterCurrentNumeric.ReadOnlyBackColor = System.Drawing.Color.Empty
        Me._MeterCurrentNumeric.ReadOnlyForeColor = System.Drawing.Color.Empty
        Me._MeterCurrentNumeric.ReadWriteBackColor = System.Drawing.Color.Empty
        Me._MeterCurrentNumeric.ReadWriteForeColor = System.Drawing.Color.Empty
        Me._MeterCurrentNumeric.Size = New System.Drawing.Size(98, 25)
        Me._MeterCurrentNumeric.TabIndex = 5
        Me._MeterCurrentNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_MeterRangeNumericLabel
        '
        Me._MeterRangeNumericLabel.AutoSize = True
        Me._MeterRangeNumericLabel.Location = New System.Drawing.Point(35, 47)
        Me._MeterRangeNumericLabel.Name = "_MeterRangeNumericLabel"
        Me._MeterRangeNumericLabel.Size = New System.Drawing.Size(88, 17)
        Me._MeterRangeNumericLabel.TabIndex = 2
        Me._MeterRangeNumericLabel.Text = "Range [Ohm]:"
        Me._MeterRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_MeterCurrentNumericLabel
        '
        Me._MeterCurrentNumericLabel.AutoSize = True
        Me._MeterCurrentNumericLabel.Location = New System.Drawing.Point(247, 47)
        Me._MeterCurrentNumericLabel.Name = "_MeterCurrentNumericLabel"
        Me._MeterCurrentNumericLabel.Size = New System.Drawing.Size(74, 17)
        Me._MeterCurrentNumericLabel.TabIndex = 4
        Me._MeterCurrentNumericLabel.Text = "Current [A]:"
        Me._MeterCurrentNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_TriggerDelayNumericLabel
        '
        Me._TriggerDelayNumericLabel.AutoSize = True
        Me._TriggerDelayNumericLabel.Location = New System.Drawing.Point(4, 85)
        Me._TriggerDelayNumericLabel.Name = "_TriggerDelayNumericLabel"
        Me._TriggerDelayNumericLabel.Size = New System.Drawing.Size(119, 17)
        Me._TriggerDelayNumericLabel.TabIndex = 6
        Me._TriggerDelayNumericLabel.Text = "Trigger Delay [ms]:"
        Me._TriggerDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_TriggerDelayNumeric
        '
        Me._TriggerDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerDelayNumeric.Location = New System.Drawing.Point(125, 81)
        Me._TriggerDelayNumeric.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me._TriggerDelayNumeric.Name = "_TriggerDelayNumeric"
        Me._TriggerDelayNumeric.ReadOnlyBackColor = System.Drawing.Color.Empty
        Me._TriggerDelayNumeric.ReadOnlyForeColor = System.Drawing.Color.Empty
        Me._TriggerDelayNumeric.ReadWriteBackColor = System.Drawing.Color.Empty
        Me._TriggerDelayNumeric.ReadWriteForeColor = System.Drawing.Color.Empty
        Me._TriggerDelayNumeric.Size = New System.Drawing.Size(76, 25)
        Me._TriggerDelayNumeric.TabIndex = 7
        Me._TriggerDelayNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._TriggerDelayNumeric, "Instrument trigger delay")
        Me._TriggerDelayNumeric.Value = New Decimal(New Integer() {111, 0, 0, 0})
        '
        '_MeterRangeComboBox
        '
        Me._MeterRangeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._MeterRangeComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._MeterRangeComboBox.FormattingEnabled = True
        Me._MeterRangeComboBox.Location = New System.Drawing.Point(51, 4)
        Me._MeterRangeComboBox.Name = "_MeterRangeComboBox"
        Me._MeterRangeComboBox.Size = New System.Drawing.Size(370, 25)
        Me._MeterRangeComboBox.TabIndex = 1
        '
        '_MeterRangeComboBoxLabel
        '
        Me._MeterRangeComboBoxLabel.AutoSize = True
        Me._MeterRangeComboBoxLabel.Location = New System.Drawing.Point(1, 8)
        Me._MeterRangeComboBoxLabel.Name = "_MeterRangeComboBoxLabel"
        Me._MeterRangeComboBoxLabel.Size = New System.Drawing.Size(48, 17)
        Me._MeterRangeComboBoxLabel.TabIndex = 0
        Me._MeterRangeComboBoxLabel.Text = "Range:"
        Me._MeterRangeComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MeasureControl
        '
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me._MeterRangeComboBox)
        Me.Controls.Add(Me._MeterRangeComboBoxLabel)
        Me.Controls.Add(Me._MeterRangeNumeric)
        Me.Controls.Add(Me._MeterCurrentNumeric)
        Me.Controls.Add(Me._MeterRangeNumericLabel)
        Me.Controls.Add(Me._TriggerDelayNumeric)
        Me.Controls.Add(Me._MeterCurrentNumericLabel)
        Me.Controls.Add(Me._TriggerDelayNumericLabel)
        Me.Name = "MeasureControl"
        Me.Size = New System.Drawing.Size(430, 113)
        CType(Me._MeterRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._MeterCurrentNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _MeterRangeNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _MeterCurrentNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _MeterRangeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _MeterCurrentNumericLabel As System.Windows.Forms.Label
    Private WithEvents _TriggerDelayNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _TriggerDelayNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _MeterRangeComboBox As isr.Core.Controls.ComboBox
    Private WithEvents _MeterRangeComboBoxLabel As System.Windows.Forms.Label

End Class
