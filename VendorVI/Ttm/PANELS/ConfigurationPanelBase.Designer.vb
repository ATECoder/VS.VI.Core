<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigurationPanelBase

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._GroupBox = New System.Windows.Forms.GroupBox()
        Me._RestoreDefaultsButton = New System.Windows.Forms.Button()
        Me._ApplyNewConfigurationButton = New System.Windows.Forms.Button()
        Me._ApplyConfigurationButton = New System.Windows.Forms.Button()
        Me._ThermalTransientGroupBox = New System.Windows.Forms.GroupBox()
        Me._ThermalVoltageLimitLabel = New System.Windows.Forms.Label()
        Me._ThermalVoltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ThermalVoltageLowLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ThermalVoltageLowLimitNumericLabel = New System.Windows.Forms.Label()
        Me._ThermalVoltageHighLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ThermalVoltageHighLimitNumericLabel = New System.Windows.Forms.Label()
        Me._ThermalVoltageNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ThermalVoltageNumericLabel = New System.Windows.Forms.Label()
        Me._ThermalCurrentNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ThermalCurrentNumericLabel = New System.Windows.Forms.Label()
        Me._ColdResistanceConfigGroupBox = New System.Windows.Forms.GroupBox()
        Me._CheckContactsCheckBox = New System.Windows.Forms.CheckBox()
        Me._PostTransientDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me._PostTransientDelayNumericLabel = New System.Windows.Forms.Label()
        Me._ColdResistanceLowLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ResistanceLowLimitNumericLabel = New System.Windows.Forms.Label()
        Me._ColdResistanceHighLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ResistanceHighLimitNumericLabel = New System.Windows.Forms.Label()
        Me._ColdVoltageNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ColdVoltageNumericLabel = New System.Windows.Forms.Label()
        Me._ColdCurrentNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ColdCurrentNumericLabel = New System.Windows.Forms.Label()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._GroupBox.SuspendLayout()
        Me._ThermalTransientGroupBox.SuspendLayout()
        CType(Me._ThermalVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ThermalVoltageLowLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ThermalVoltageHighLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ThermalVoltageNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ThermalCurrentNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ColdResistanceConfigGroupBox.SuspendLayout()
        CType(Me._PostTransientDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ColdResistanceLowLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ColdResistanceHighLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ColdVoltageNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ColdCurrentNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        '_GroupBox
        '
        Me._GroupBox.Controls.Add(Me._RestoreDefaultsButton)
        Me._GroupBox.Controls.Add(Me._ApplyNewConfigurationButton)
        Me._GroupBox.Controls.Add(Me._ApplyConfigurationButton)
        Me._GroupBox.Controls.Add(Me._ThermalTransientGroupBox)
        Me._GroupBox.Controls.Add(Me._ColdResistanceConfigGroupBox)
        Me._GroupBox.Location = New System.Drawing.Point(0, 0)
        Me._GroupBox.Name = "_GroupBox"
        Me._GroupBox.Size = New System.Drawing.Size(544, 333)
        Me._GroupBox.TabIndex = 2
        Me._GroupBox.TabStop = False
        Me._GroupBox.Text = "TTM CONFIGURATION"
        '
        '_RestoreDefaultsButton
        '
        Me._RestoreDefaultsButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._RestoreDefaultsButton.Location = New System.Drawing.Point(364, 294)
        Me._RestoreDefaultsButton.Name = "_RestoreDefaultsButton"
        Me._RestoreDefaultsButton.Size = New System.Drawing.Size(153, 30)
        Me._RestoreDefaultsButton.TabIndex = 4
        Me._RestoreDefaultsButton.Text = "RESTORE DEFAULTS"
        Me._RestoreDefaultsButton.UseVisualStyleBackColor = True
        '
        '_ApplyNewConfigurationButton
        '
        Me._ApplyNewConfigurationButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ApplyNewConfigurationButton.Location = New System.Drawing.Point(196, 294)
        Me._ApplyNewConfigurationButton.Name = "_ApplyNewConfigurationButton"
        Me._ApplyNewConfigurationButton.Size = New System.Drawing.Size(153, 30)
        Me._ApplyNewConfigurationButton.TabIndex = 3
        Me._ApplyNewConfigurationButton.Text = "APPLY CHANGES"
        Me._ApplyNewConfigurationButton.UseVisualStyleBackColor = True
        '
        '_ApplyConfigurationButton
        '
        Me._ApplyConfigurationButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ApplyConfigurationButton.Location = New System.Drawing.Point(28, 294)
        Me._ApplyConfigurationButton.Name = "_ApplyConfigurationButton"
        Me._ApplyConfigurationButton.Size = New System.Drawing.Size(153, 30)
        Me._ApplyConfigurationButton.TabIndex = 2
        Me._ApplyConfigurationButton.Text = "APPLY ALL"
        Me._ApplyConfigurationButton.UseVisualStyleBackColor = True
        '
        '_ThermalTransientGroupBox
        '
        Me._ThermalTransientGroupBox.Controls.Add(Me._ThermalVoltageLimitLabel)
        Me._ThermalTransientGroupBox.Controls.Add(Me._ThermalVoltageLimitNumeric)
        Me._ThermalTransientGroupBox.Controls.Add(Me._ThermalVoltageLowLimitNumeric)
        Me._ThermalTransientGroupBox.Controls.Add(Me._ThermalVoltageLowLimitNumericLabel)
        Me._ThermalTransientGroupBox.Controls.Add(Me._ThermalVoltageHighLimitNumeric)
        Me._ThermalTransientGroupBox.Controls.Add(Me._ThermalVoltageHighLimitNumericLabel)
        Me._ThermalTransientGroupBox.Controls.Add(Me._ThermalVoltageNumeric)
        Me._ThermalTransientGroupBox.Controls.Add(Me._ThermalVoltageNumericLabel)
        Me._ThermalTransientGroupBox.Controls.Add(Me._ThermalCurrentNumeric)
        Me._ThermalTransientGroupBox.Controls.Add(Me._ThermalCurrentNumericLabel)
        Me._ThermalTransientGroupBox.Location = New System.Drawing.Point(277, 23)
        Me._ThermalTransientGroupBox.Name = "_ThermalTransientGroupBox"
        Me._ThermalTransientGroupBox.Size = New System.Drawing.Size(258, 259)
        Me._ThermalTransientGroupBox.TabIndex = 0
        Me._ThermalTransientGroupBox.TabStop = False
        Me._ThermalTransientGroupBox.Text = "THERMAL TRANSIENT"
        '
        '_ThermalVoltageLimitLabel
        '
        Me._ThermalVoltageLimitLabel.AutoSize = True
        Me._ThermalVoltageLimitLabel.Location = New System.Drawing.Point(45, 113)
        Me._ThermalVoltageLimitLabel.Name = "_ThermalVoltageLimitLabel"
        Me._ThermalVoltageLimitLabel.Size = New System.Drawing.Size(121, 17)
        Me._ThermalVoltageLimitLabel.TabIndex = 4
        Me._ThermalVoltageLimitLabel.Text = "VOLTAGE LIMIT &[V]:"
        Me._ThermalVoltageLimitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ThermalVoltageLimitNumeric
        '
        Me._ThermalVoltageLimitNumeric.DecimalPlaces = 3
        Me._ThermalVoltageLimitNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ThermalVoltageLimitNumeric.Location = New System.Drawing.Point(168, 109)
        Me._ThermalVoltageLimitNumeric.Maximum = New Decimal(New Integer() {9999, 0, 0, 196608})
        Me._ThermalVoltageLimitNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me._ThermalVoltageLimitNumeric.Name = "_ThermalVoltageLimitNumeric"
        Me._ThermalVoltageLimitNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ThermalVoltageLimitNumeric.TabIndex = 5
        Me._ThermalVoltageLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ThermalVoltageLimitNumeric.Value = New Decimal(New Integer() {99, 0, 0, 131072})
        '
        '_ThermalVoltageLowLimitNumeric
        '
        Me._ThermalVoltageLowLimitNumeric.DecimalPlaces = 3
        Me._ThermalVoltageLowLimitNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ThermalVoltageLowLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me._ThermalVoltageLowLimitNumeric.Location = New System.Drawing.Point(168, 189)
        Me._ThermalVoltageLowLimitNumeric.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me._ThermalVoltageLowLimitNumeric.Name = "_ThermalVoltageLowLimitNumeric"
        Me._ThermalVoltageLowLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._ThermalVoltageLowLimitNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ThermalVoltageLowLimitNumeric.TabIndex = 9
        Me._ThermalVoltageLowLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ThermalVoltageLowLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        '_ThermalVoltageLowLimitNumericLabel
        '
        Me._ThermalVoltageLowLimitNumericLabel.AutoSize = True
        Me._ThermalVoltageLowLimitNumericLabel.Location = New System.Drawing.Point(72, 193)
        Me._ThermalVoltageLowLimitNumericLabel.Name = "_ThermalVoltageLowLimitNumericLabel"
        Me._ThermalVoltageLowLimitNumericLabel.Size = New System.Drawing.Size(94, 17)
        Me._ThermalVoltageLowLimitNumericLabel.TabIndex = 8
        Me._ThermalVoltageLowLimitNumericLabel.Text = "LOW LIMI&T [V]:"
        Me._ThermalVoltageLowLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ThermalVoltageHighLimitNumeric
        '
        Me._ThermalVoltageHighLimitNumeric.DecimalPlaces = 3
        Me._ThermalVoltageHighLimitNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ThermalVoltageHighLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me._ThermalVoltageHighLimitNumeric.Location = New System.Drawing.Point(168, 149)
        Me._ThermalVoltageHighLimitNumeric.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me._ThermalVoltageHighLimitNumeric.Name = "_ThermalVoltageHighLimitNumeric"
        Me._ThermalVoltageHighLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._ThermalVoltageHighLimitNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ThermalVoltageHighLimitNumeric.TabIndex = 7
        Me._ThermalVoltageHighLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ThermalVoltageHighLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        '_ThermalVoltageHighLimitNumericLabel
        '
        Me._ThermalVoltageHighLimitNumericLabel.AutoSize = True
        Me._ThermalVoltageHighLimitNumericLabel.Location = New System.Drawing.Point(70, 153)
        Me._ThermalVoltageHighLimitNumericLabel.Name = "_ThermalVoltageHighLimitNumericLabel"
        Me._ThermalVoltageHighLimitNumericLabel.Size = New System.Drawing.Size(96, 17)
        Me._ThermalVoltageHighLimitNumericLabel.TabIndex = 6
        Me._ThermalVoltageHighLimitNumericLabel.Text = "HI&GH LIMIT [V]:"
        Me._ThermalVoltageHighLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ThermalVoltageNumeric
        '
        Me._ThermalVoltageNumeric.DecimalPlaces = 3
        Me._ThermalVoltageNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ThermalVoltageNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me._ThermalVoltageNumeric.Location = New System.Drawing.Point(168, 69)
        Me._ThermalVoltageNumeric.Maximum = New Decimal(New Integer() {99, 0, 0, 131072})
        Me._ThermalVoltageNumeric.Name = "_ThermalVoltageNumeric"
        Me._ThermalVoltageNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ThermalVoltageNumeric.TabIndex = 3
        Me._ThermalVoltageNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ThermalVoltageNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        '_ThermalVoltageNumericLabel
        '
        Me._ThermalVoltageNumericLabel.AutoSize = True
        Me._ThermalVoltageNumericLabel.Location = New System.Drawing.Point(25, 73)
        Me._ThermalVoltageNumericLabel.Name = "_ThermalVoltageNumericLabel"
        Me._ThermalVoltageNumericLabel.Size = New System.Drawing.Size(141, 17)
        Me._ThermalVoltageNumericLabel.TabIndex = 2
        Me._ThermalVoltageNumericLabel.Text = "VOLTAGE C&HANGE [V]:"
        Me._ThermalVoltageNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ThermalCurrentNumeric
        '
        Me._ThermalCurrentNumeric.DecimalPlaces = 3
        Me._ThermalCurrentNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ThermalCurrentNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me._ThermalCurrentNumeric.Location = New System.Drawing.Point(168, 29)
        Me._ThermalCurrentNumeric.Maximum = New Decimal(New Integer() {1514, 0, 0, 196608})
        Me._ThermalCurrentNumeric.Name = "_ThermalCurrentNumeric"
        Me._ThermalCurrentNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ThermalCurrentNumeric.TabIndex = 1
        Me._ThermalCurrentNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ThermalCurrentNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        '_ThermalCurrentNumericLabel
        '
        Me._ThermalCurrentNumericLabel.AutoSize = True
        Me._ThermalCurrentNumericLabel.Location = New System.Drawing.Point(40, 33)
        Me._ThermalCurrentNumericLabel.Name = "_ThermalCurrentNumericLabel"
        Me._ThermalCurrentNumericLabel.Size = New System.Drawing.Size(126, 17)
        Me._ThermalCurrentNumericLabel.TabIndex = 0
        Me._ThermalCurrentNumericLabel.Text = "CURR&ENT LEVEL [A]:"
        Me._ThermalCurrentNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ColdResistanceConfigGroupBox
        '
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._CheckContactsCheckBox)
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._PostTransientDelayNumeric)
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._PostTransientDelayNumericLabel)
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._ColdResistanceLowLimitNumeric)
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._ResistanceLowLimitNumericLabel)
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._ColdResistanceHighLimitNumeric)
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._ResistanceHighLimitNumericLabel)
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._ColdVoltageNumeric)
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._ColdVoltageNumericLabel)
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._ColdCurrentNumeric)
        Me._ColdResistanceConfigGroupBox.Controls.Add(Me._ColdCurrentNumericLabel)
        Me._ColdResistanceConfigGroupBox.Location = New System.Drawing.Point(9, 23)
        Me._ColdResistanceConfigGroupBox.Name = "_ColdResistanceConfigGroupBox"
        Me._ColdResistanceConfigGroupBox.Size = New System.Drawing.Size(258, 259)
        Me._ColdResistanceConfigGroupBox.TabIndex = 0
        Me._ColdResistanceConfigGroupBox.TabStop = False
        Me._ColdResistanceConfigGroupBox.Text = "COLD RESISTANCE"
        '
        '_CheckContactsCheckBox
        '
        Me._CheckContactsCheckBox.AutoSize = True
        Me._CheckContactsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._CheckContactsCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._CheckContactsCheckBox.Location = New System.Drawing.Point(36, 233)
        Me._CheckContactsCheckBox.Name = "_CheckContactsCheckBox"
        Me._CheckContactsCheckBox.Size = New System.Drawing.Size(139, 21)
        Me._CheckContactsCheckBox.TabIndex = 5
        Me._CheckContactsCheckBox.Text = "CHECK CONTACTS:"
        Me._CheckContactsCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._CheckContactsCheckBox.UseVisualStyleBackColor = True
        '
        '_PostTransientDelayNumeric
        '
        Me._PostTransientDelayNumeric.DecimalPlaces = 2
        Me._PostTransientDelayNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._PostTransientDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._PostTransientDelayNumeric.Location = New System.Drawing.Point(162, 189)
        Me._PostTransientDelayNumeric.Maximum = New Decimal(New Integer() {600, 0, 0, 0})
        Me._PostTransientDelayNumeric.Name = "_PostTransientDelayNumeric"
        Me._PostTransientDelayNumeric.Size = New System.Drawing.Size(75, 25)
        Me._PostTransientDelayNumeric.TabIndex = 9
        Me._PostTransientDelayNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._PostTransientDelayNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        '_PostTransientDelayNumericLabel
        '
        Me._PostTransientDelayNumericLabel.AutoSize = True
        Me._PostTransientDelayNumericLabel.Location = New System.Drawing.Point(28, 193)
        Me._PostTransientDelayNumericLabel.Name = "_PostTransientDelayNumericLabel"
        Me._PostTransientDelayNumericLabel.Size = New System.Drawing.Size(132, 17)
        Me._PostTransientDelayNumericLabel.TabIndex = 8
        Me._PostTransientDelayNumericLabel.Text = "POST TTM &DELAY [S]:"
        Me._PostTransientDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ColdResistanceLowLimitNumeric
        '
        Me._ColdResistanceLowLimitNumeric.DecimalPlaces = 3
        Me._ColdResistanceLowLimitNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ColdResistanceLowLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._ColdResistanceLowLimitNumeric.Location = New System.Drawing.Point(162, 149)
        Me._ColdResistanceLowLimitNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me._ColdResistanceLowLimitNumeric.Name = "_ColdResistanceLowLimitNumeric"
        Me._ColdResistanceLowLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._ColdResistanceLowLimitNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ColdResistanceLowLimitNumeric.TabIndex = 7
        Me._ColdResistanceLowLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ColdResistanceLowLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        '_ResistanceLowLimitNumericLabel
        '
        Me._ResistanceLowLimitNumericLabel.AutoSize = True
        Me._ResistanceLowLimitNumericLabel.Location = New System.Drawing.Point(64, 153)
        Me._ResistanceLowLimitNumericLabel.Name = "_ResistanceLowLimitNumericLabel"
        Me._ResistanceLowLimitNumericLabel.Size = New System.Drawing.Size(96, 17)
        Me._ResistanceLowLimitNumericLabel.TabIndex = 6
        Me._ResistanceLowLimitNumericLabel.Text = "LO&W LIMIT [Ω]:"
        Me._ResistanceLowLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ColdResistanceHighLimitNumeric
        '
        Me._ColdResistanceHighLimitNumeric.DecimalPlaces = 3
        Me._ColdResistanceHighLimitNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ColdResistanceHighLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._ColdResistanceHighLimitNumeric.Location = New System.Drawing.Point(162, 109)
        Me._ColdResistanceHighLimitNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me._ColdResistanceHighLimitNumeric.Name = "_ColdResistanceHighLimitNumeric"
        Me._ColdResistanceHighLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._ColdResistanceHighLimitNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ColdResistanceHighLimitNumeric.TabIndex = 5
        Me._ColdResistanceHighLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ColdResistanceHighLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        '_ResistanceHighLimitNumericLabel
        '
        Me._ResistanceHighLimitNumericLabel.AutoSize = True
        Me._ResistanceHighLimitNumericLabel.Location = New System.Drawing.Point(62, 113)
        Me._ResistanceHighLimitNumericLabel.Name = "_ResistanceHighLimitNumericLabel"
        Me._ResistanceHighLimitNumericLabel.Size = New System.Drawing.Size(98, 17)
        Me._ResistanceHighLimitNumericLabel.TabIndex = 4
        Me._ResistanceHighLimitNumericLabel.Text = "&HIGH LIMIT [Ω]:"
        Me._ResistanceHighLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ColdVoltageNumeric
        '
        Me._ColdVoltageNumeric.DecimalPlaces = 2
        Me._ColdVoltageNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ColdVoltageNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._ColdVoltageNumeric.Location = New System.Drawing.Point(162, 69)
        Me._ColdVoltageNumeric.Maximum = New Decimal(New Integer() {2, 0, 0, 0})
        Me._ColdVoltageNumeric.Name = "_ColdVoltageNumeric"
        Me._ColdVoltageNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ColdVoltageNumeric.TabIndex = 3
        Me._ColdVoltageNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ColdVoltageNumeric.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        '_ColdVoltageNumericLabel
        '
        Me._ColdVoltageNumericLabel.AutoSize = True
        Me._ColdVoltageNumericLabel.Location = New System.Drawing.Point(39, 73)
        Me._ColdVoltageNumericLabel.Name = "_ColdVoltageNumericLabel"
        Me._ColdVoltageNumericLabel.Size = New System.Drawing.Size(121, 17)
        Me._ColdVoltageNumericLabel.TabIndex = 2
        Me._ColdVoltageNumericLabel.Text = "&VOLTAGE LIMIT [V]:"
        Me._ColdVoltageNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ColdCurrentNumeric
        '
        Me._ColdCurrentNumeric.DecimalPlaces = 4
        Me._ColdCurrentNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ColdCurrentNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me._ColdCurrentNumeric.Location = New System.Drawing.Point(162, 29)
        Me._ColdCurrentNumeric.Maximum = New Decimal(New Integer() {100, 0, 0, 262144})
        Me._ColdCurrentNumeric.Name = "_ColdCurrentNumeric"
        Me._ColdCurrentNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ColdCurrentNumeric.TabIndex = 1
        Me._ColdCurrentNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ColdCurrentNumeric.Value = New Decimal(New Integer() {1, 0, 0, 262144})
        '
        '_ColdCurrentNumericLabel
        '
        Me._ColdCurrentNumericLabel.AutoSize = True
        Me._ColdCurrentNumericLabel.Location = New System.Drawing.Point(34, 33)
        Me._ColdCurrentNumericLabel.Name = "_ColdCurrentNumericLabel"
        Me._ColdCurrentNumericLabel.Size = New System.Drawing.Size(126, 17)
        Me._ColdCurrentNumericLabel.TabIndex = 0
        Me._ColdCurrentNumericLabel.Text = "C&URRENT LEVEL [A]:"
        Me._ColdCurrentNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ConfigurationPanel
        '
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me._GroupBox)
        Me.Name = "ConfigurationPanel"
        Me.Size = New System.Drawing.Size(544, 333)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._GroupBox.ResumeLayout(False)
        Me._ThermalTransientGroupBox.ResumeLayout(False)
        Me._ThermalTransientGroupBox.PerformLayout()
        CType(Me._ThermalVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ThermalVoltageLowLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ThermalVoltageHighLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ThermalVoltageNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ThermalCurrentNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ColdResistanceConfigGroupBox.ResumeLayout(False)
        Me._ColdResistanceConfigGroupBox.PerformLayout()
        CType(Me._PostTransientDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ColdResistanceLowLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ColdResistanceHighLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ColdVoltageNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ColdCurrentNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _ErrorProvider As System.Windows.Forms.ErrorProvider
    Private WithEvents _GroupBox As System.Windows.Forms.GroupBox
    Private WithEvents _RestoreDefaultsButton As System.Windows.Forms.Button
    Private WithEvents _ApplyNewConfigurationButton As System.Windows.Forms.Button
    Private WithEvents _ApplyConfigurationButton As System.Windows.Forms.Button
    Private WithEvents _ThermalTransientGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents _ThermalVoltageLimitLabel As System.Windows.Forms.Label
    Private WithEvents _ThermalVoltageLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ThermalVoltageLowLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ThermalVoltageLowLimitNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ThermalVoltageHighLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ThermalVoltageHighLimitNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ThermalVoltageNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ThermalVoltageNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ThermalCurrentNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ThermalCurrentNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ColdResistanceConfigGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents _CheckContactsCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents _PostTransientDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _PostTransientDelayNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ColdResistanceLowLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ResistanceLowLimitNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ColdResistanceHighLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ResistanceHighLimitNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ColdVoltageNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ColdVoltageNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ColdCurrentNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ColdCurrentNumericLabel As System.Windows.Forms.Label

End Class
