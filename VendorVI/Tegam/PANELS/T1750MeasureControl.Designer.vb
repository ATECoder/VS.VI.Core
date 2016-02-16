<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class T1750MeasureControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me._MeterRangeComboBox = New isr.Core.Controls.ComboBox()
        Me._MeterRangeNumeric = New isr.Core.Controls.NumericUpDown()
        Me._MeterCurrentNumeric = New isr.Core.Controls.NumericUpDown()
        Me._MeterRangeNumericLabel = New System.Windows.Forms.Label()
        Me._MeterCurrentNumericLabel = New System.Windows.Forms.Label()
        Me._MeterRangeComboBoxLabel = New System.Windows.Forms.Label()
        Me._TriggerCombo = New System.Windows.Forms.ComboBox()
        Me._TriggerComboBoxLabel = New System.Windows.Forms.Label()
        Me._TriggerDelayNumericLabel = New System.Windows.Forms.Label()
        Me._InitialDelayNumericLabel = New System.Windows.Forms.Label()
        Me._MeasurementDelayNumericLabel = New System.Windows.Forms.Label()
        Me._MaximumTrialsCountNumericLabel = New System.Windows.Forms.Label()
        Me._MaximumDifferenceNumericLabel = New System.Windows.Forms.Label()
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._TriggerDelayNumeric = New isr.Core.Controls.NumericUpDown()
        Me._InitialDelayNumeric = New isr.Core.Controls.NumericUpDown()
        Me._MaximumTrialsCountNumeric = New isr.Core.Controls.NumericUpDown()
        Me._MaximumDifferenceNumeric = New isr.Core.Controls.NumericUpDown()
        Me._MeasurementDelayNumeric = New isr.Core.Controls.NumericUpDown()
        CType(Me._MeterRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._MeterCurrentNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._InitialDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._MaximumTrialsCountNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._MaximumDifferenceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._MeasurementDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_MeterRangeComboBox
        '
        Me._MeterRangeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._MeterRangeComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._MeterRangeComboBox.FormattingEnabled = True
        Me._MeterRangeComboBox.Location = New System.Drawing.Point(73, 3)
        Me._MeterRangeComboBox.Name = "_MeterRangeComboBox"
        Me._MeterRangeComboBox.Size = New System.Drawing.Size(457, 25)
        Me._MeterRangeComboBox.TabIndex = 1
        '
        '_MeterRangeNumeric
        '
        Me._MeterRangeNumeric.DecimalPlaces = 3
        Me._MeterRangeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._MeterRangeNumeric.Location = New System.Drawing.Point(416, 41)
        Me._MeterRangeNumeric.Maximum = New Decimal(New Integer() {20000000, 0, 0, 0})
        Me._MeterRangeNumeric.Name = "_MeterRangeNumeric"
        Me._MeterRangeNumeric.ReadOnly = True
        Me._MeterRangeNumeric.ReadOnlyBackColor = System.Drawing.Color.Empty
        Me._MeterRangeNumeric.ReadOnlyForeColor = System.Drawing.Color.Empty
        Me._MeterRangeNumeric.ReadWriteBackColor = System.Drawing.Color.Empty
        Me._MeterRangeNumeric.ReadWriteForeColor = System.Drawing.Color.Empty
        Me._MeterRangeNumeric.Size = New System.Drawing.Size(114, 25)
        Me._MeterRangeNumeric.TabIndex = 6
        Me._MeterRangeNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._MeterRangeNumeric.Value = New Decimal(New Integer() {2000000, 0, 0, 0})
        '
        '_MeterCurrentNumeric
        '
        Me._MeterCurrentNumeric.DecimalPlaces = 7
        Me._MeterCurrentNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._MeterCurrentNumeric.Location = New System.Drawing.Point(73, 41)
        Me._MeterCurrentNumeric.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me._MeterCurrentNumeric.Name = "_MeterCurrentNumeric"
        Me._MeterCurrentNumeric.ReadOnly = True
        Me._MeterCurrentNumeric.ReadOnlyBackColor = System.Drawing.Color.Empty
        Me._MeterCurrentNumeric.ReadOnlyForeColor = System.Drawing.Color.Empty
        Me._MeterCurrentNumeric.ReadWriteBackColor = System.Drawing.Color.Empty
        Me._MeterCurrentNumeric.ReadWriteForeColor = System.Drawing.Color.Empty
        Me._MeterCurrentNumeric.Size = New System.Drawing.Size(98, 25)
        Me._MeterCurrentNumeric.TabIndex = 3
        Me._MeterCurrentNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_MeterRangeNumericLabel
        '
        Me._MeterRangeNumericLabel.AutoSize = True
        Me._MeterRangeNumericLabel.Location = New System.Drawing.Point(326, 45)
        Me._MeterRangeNumericLabel.Name = "_MeterRangeNumericLabel"
        Me._MeterRangeNumericLabel.Size = New System.Drawing.Size(88, 17)
        Me._MeterRangeNumericLabel.TabIndex = 5
        Me._MeterRangeNumericLabel.Text = "Range [Ohm]:"
        Me._MeterRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_MeterCurrentNumericLabel
        '
        Me._MeterCurrentNumericLabel.AutoSize = True
        Me._MeterCurrentNumericLabel.Location = New System.Drawing.Point(-3, 45)
        Me._MeterCurrentNumericLabel.Name = "_MeterCurrentNumericLabel"
        Me._MeterCurrentNumericLabel.Size = New System.Drawing.Size(74, 17)
        Me._MeterCurrentNumericLabel.TabIndex = 2
        Me._MeterCurrentNumericLabel.Text = "Current [A]:"
        Me._MeterCurrentNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_MeterRangeComboBoxLabel
        '
        Me._MeterRangeComboBoxLabel.AutoSize = True
        Me._MeterRangeComboBoxLabel.Location = New System.Drawing.Point(23, 7)
        Me._MeterRangeComboBoxLabel.Name = "_MeterRangeComboBoxLabel"
        Me._MeterRangeComboBoxLabel.Size = New System.Drawing.Size(48, 17)
        Me._MeterRangeComboBoxLabel.TabIndex = 0
        Me._MeterRangeComboBoxLabel.Text = "Range:"
        Me._MeterRangeComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_TriggerCombo
        '
        Me._TriggerCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._TriggerCombo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._TriggerCombo.Items.AddRange(New Object() {"Delay Continuous (T2)"})
        Me._TriggerCombo.Location = New System.Drawing.Point(73, 75)
        Me._TriggerCombo.Name = "_TriggerCombo"
        Me._TriggerCombo.Size = New System.Drawing.Size(177, 25)
        Me._TriggerCombo.TabIndex = 12
        '
        '_TriggerComboBoxLabel
        '
        Me._TriggerComboBoxLabel.AutoSize = True
        Me._TriggerComboBoxLabel.Location = New System.Drawing.Point(17, 79)
        Me._TriggerComboBoxLabel.Name = "_TriggerComboBoxLabel"
        Me._TriggerComboBoxLabel.Size = New System.Drawing.Size(54, 17)
        Me._TriggerComboBoxLabel.TabIndex = 11
        Me._TriggerComboBoxLabel.Text = "Trigger:"
        Me._TriggerComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_TriggerDelayNumericLabel
        '
        Me._TriggerDelayNumericLabel.AutoSize = True
        Me._TriggerDelayNumericLabel.Location = New System.Drawing.Point(295, 79)
        Me._TriggerDelayNumericLabel.Name = "_TriggerDelayNumericLabel"
        Me._TriggerDelayNumericLabel.Size = New System.Drawing.Size(119, 17)
        Me._TriggerDelayNumericLabel.TabIndex = 13
        Me._TriggerDelayNumericLabel.Text = "Trigger Delay [ms]:"
        Me._TriggerDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_InitialDelayNumericLabel
        '
        Me._InitialDelayNumericLabel.AutoSize = True
        Me._InitialDelayNumericLabel.Location = New System.Drawing.Point(66, 114)
        Me._InitialDelayNumericLabel.Name = "_InitialDelayNumericLabel"
        Me._InitialDelayNumericLabel.Size = New System.Drawing.Size(106, 17)
        Me._InitialDelayNumericLabel.TabIndex = 13
        Me._InitialDelayNumericLabel.Text = "Initial Delay [ms]:"
        Me._InitialDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_MeasurementDelayNumericLabel
        '
        Me._MeasurementDelayNumericLabel.AutoSize = True
        Me._MeasurementDelayNumericLabel.Location = New System.Drawing.Point(258, 114)
        Me._MeasurementDelayNumericLabel.Name = "_MeasurementDelayNumericLabel"
        Me._MeasurementDelayNumericLabel.Size = New System.Drawing.Size(156, 17)
        Me._MeasurementDelayNumericLabel.TabIndex = 13
        Me._MeasurementDelayNumericLabel.Text = "Measurement Delay [ms]:"
        Me._MeasurementDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_MaximumTrialsCountNumericLabel
        '
        Me._MaximumTrialsCountNumericLabel.AutoSize = True
        Me._MaximumTrialsCountNumericLabel.Location = New System.Drawing.Point(4, 150)
        Me._MaximumTrialsCountNumericLabel.Name = "_MaximumTrialsCountNumericLabel"
        Me._MaximumTrialsCountNumericLabel.Size = New System.Drawing.Size(171, 17)
        Me._MaximumTrialsCountNumericLabel.TabIndex = 13
        Me._MaximumTrialsCountNumericLabel.Text = "Maximum Number of Trials:"
        '
        '_MaximumDifferenceNumericLabel
        '
        Me._MaximumDifferenceNumericLabel.AutoSize = True
        Me._MaximumDifferenceNumericLabel.Location = New System.Drawing.Point(259, 150)
        Me._MaximumDifferenceNumericLabel.Name = "_MaximumDifferenceNumericLabel"
        Me._MaximumDifferenceNumericLabel.Size = New System.Drawing.Size(154, 17)
        Me._MaximumDifferenceNumericLabel.TabIndex = 13
        Me._MaximumDifferenceNumericLabel.Text = "Maximum Difference [%]:"
        '
        '_TriggerDelayNumeric
        '
        Me._TriggerDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerDelayNumeric.Location = New System.Drawing.Point(416, 75)
        Me._TriggerDelayNumeric.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me._TriggerDelayNumeric.Name = "_TriggerDelayNumeric"
        Me._TriggerDelayNumeric.ReadOnlyBackColor = System.Drawing.Color.Empty
        Me._TriggerDelayNumeric.ReadOnlyForeColor = System.Drawing.Color.Empty
        Me._TriggerDelayNumeric.ReadWriteBackColor = System.Drawing.Color.Empty
        Me._TriggerDelayNumeric.ReadWriteForeColor = System.Drawing.Color.Empty
        Me._TriggerDelayNumeric.Size = New System.Drawing.Size(76, 25)
        Me._TriggerDelayNumeric.TabIndex = 14
        Me._TriggerDelayNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._TriggerDelayNumeric, "Instrument trigger delay")
        Me._TriggerDelayNumeric.Value = New Decimal(New Integer() {111, 0, 0, 0})
        '
        '_InitialDelayNumeric
        '
        Me._InitialDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._InitialDelayNumeric.Location = New System.Drawing.Point(174, 110)
        Me._InitialDelayNumeric.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me._InitialDelayNumeric.Name = "_InitialDelayNumeric"
        Me._InitialDelayNumeric.ReadOnlyBackColor = System.Drawing.Color.Empty
        Me._InitialDelayNumeric.ReadOnlyForeColor = System.Drawing.Color.Empty
        Me._InitialDelayNumeric.ReadWriteBackColor = System.Drawing.Color.Empty
        Me._InitialDelayNumeric.ReadWriteForeColor = System.Drawing.Color.Empty
        Me._InitialDelayNumeric.Size = New System.Drawing.Size(76, 25)
        Me._InitialDelayNumeric.TabIndex = 14
        Me._InitialDelayNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._InitialDelayNumeric, "Delay of first measurement")
        Me._InitialDelayNumeric.Value = New Decimal(New Integer() {149, 0, 0, 0})
        '
        '_MaximumTrialsCountNumeric
        '
        Me._MaximumTrialsCountNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._MaximumTrialsCountNumeric.Location = New System.Drawing.Point(174, 146)
        Me._MaximumTrialsCountNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me._MaximumTrialsCountNumeric.Name = "_MaximumTrialsCountNumeric"
        Me._MaximumTrialsCountNumeric.ReadOnlyBackColor = System.Drawing.Color.Empty
        Me._MaximumTrialsCountNumeric.ReadOnlyForeColor = System.Drawing.Color.Empty
        Me._MaximumTrialsCountNumeric.ReadWriteBackColor = System.Drawing.Color.Empty
        Me._MaximumTrialsCountNumeric.ReadWriteForeColor = System.Drawing.Color.Empty
        Me._MaximumTrialsCountNumeric.Size = New System.Drawing.Size(76, 25)
        Me._MaximumTrialsCountNumeric.TabIndex = 14
        Me._MaximumTrialsCountNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._MaximumTrialsCountNumeric, "Maximum number of trials before giving up")
        Me._MaximumTrialsCountNumeric.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        '_MaximumDifferenceNumeric
        '
        Me._MaximumDifferenceNumeric.DecimalPlaces = 1
        Me._MaximumDifferenceNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._MaximumDifferenceNumeric.Location = New System.Drawing.Point(415, 146)
        Me._MaximumDifferenceNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._MaximumDifferenceNumeric.Name = "_MaximumDifferenceNumeric"
        Me._MaximumDifferenceNumeric.ReadOnlyBackColor = System.Drawing.Color.Empty
        Me._MaximumDifferenceNumeric.ReadOnlyForeColor = System.Drawing.Color.Empty
        Me._MaximumDifferenceNumeric.ReadWriteBackColor = System.Drawing.Color.Empty
        Me._MaximumDifferenceNumeric.ReadWriteForeColor = System.Drawing.Color.Empty
        Me._MaximumDifferenceNumeric.Size = New System.Drawing.Size(76, 25)
        Me._MaximumDifferenceNumeric.TabIndex = 14
        Me._MaximumDifferenceNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._MaximumDifferenceNumeric, "Maximum difference between measurements")
        Me._MaximumDifferenceNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        '_MeasurementDelayNumeric
        '
        Me._MeasurementDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._MeasurementDelayNumeric.Location = New System.Drawing.Point(416, 110)
        Me._MeasurementDelayNumeric.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me._MeasurementDelayNumeric.Name = "_MeasurementDelayNumeric"
        Me._MeasurementDelayNumeric.ReadOnlyBackColor = System.Drawing.Color.Empty
        Me._MeasurementDelayNumeric.ReadOnlyForeColor = System.Drawing.Color.Empty
        Me._MeasurementDelayNumeric.ReadWriteBackColor = System.Drawing.Color.Empty
        Me._MeasurementDelayNumeric.ReadWriteForeColor = System.Drawing.Color.Empty
        Me._MeasurementDelayNumeric.Size = New System.Drawing.Size(76, 25)
        Me._MeasurementDelayNumeric.TabIndex = 14
        Me._MeasurementDelayNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._MeasurementDelayNumeric, "Delay of measurement 2 and up")
        Me._MeasurementDelayNumeric.Value = New Decimal(New Integer() {149, 0, 0, 0})
        '
        'MeasureControl
        '
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me._MeterRangeComboBox)
        Me.Controls.Add(Me._MaximumDifferenceNumeric)
        Me.Controls.Add(Me._MaximumTrialsCountNumeric)
        Me.Controls.Add(Me._MeterRangeNumeric)
        Me.Controls.Add(Me._MeasurementDelayNumeric)
        Me.Controls.Add(Me._MeterCurrentNumeric)
        Me.Controls.Add(Me._InitialDelayNumeric)
        Me.Controls.Add(Me._MeterRangeNumericLabel)
        Me.Controls.Add(Me._TriggerDelayNumeric)
        Me.Controls.Add(Me._MeterCurrentNumericLabel)
        Me.Controls.Add(Me._MaximumDifferenceNumericLabel)
        Me.Controls.Add(Me._MeterRangeComboBoxLabel)
        Me.Controls.Add(Me._MaximumTrialsCountNumericLabel)
        Me.Controls.Add(Me._MeasurementDelayNumericLabel)
        Me.Controls.Add(Me._InitialDelayNumericLabel)
        Me.Controls.Add(Me._TriggerDelayNumericLabel)
        Me.Controls.Add(Me._TriggerCombo)
        Me.Controls.Add(Me._TriggerComboBoxLabel)
        Me.Name = "MeasureControl"
        Me.Size = New System.Drawing.Size(534, 182)
        CType(Me._MeterRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._MeterCurrentNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._TriggerDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._InitialDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._MaximumTrialsCountNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._MaximumDifferenceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._MeasurementDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _MeterRangeComboBox As isr.Core.Controls.ComboBox
    Private WithEvents _MeterRangeNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _MeterCurrentNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _MeterRangeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _MeterCurrentNumericLabel As System.Windows.Forms.Label
    Private WithEvents _MeterRangeComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _TriggerCombo As System.Windows.Forms.ComboBox
    Private WithEvents _TriggerComboBoxLabel As System.Windows.Forms.Label
    Private WithEvents _TriggerDelayNumericLabel As System.Windows.Forms.Label
    Private WithEvents _InitialDelayNumericLabel As System.Windows.Forms.Label
    Private WithEvents _MeasurementDelayNumericLabel As System.Windows.Forms.Label
    Private WithEvents _MaximumTrialsCountNumericLabel As System.Windows.Forms.Label
    Private WithEvents _MaximumDifferenceNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _TriggerDelayNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _InitialDelayNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _MaximumTrialsCountNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _MaximumDifferenceNumeric As isr.Core.Controls.NumericUpDown
    Private WithEvents _MeasurementDelayNumeric As isr.Core.Controls.NumericUpDown

End Class
