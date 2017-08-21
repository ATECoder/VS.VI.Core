<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Console

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Console))
        Me._MainLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._ConfigurationLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._TTMConfigurationPanel = New isr.VI.Ttm.ConfigurationPanel()
        Me._ShuntResistanceConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me._RestoreShuntResistanceDefaultsButton = New System.Windows.Forms.Button()
        Me._ApplyNewShuntResistanceConfigurationButton = New System.Windows.Forms.Button()
        Me._ApplyShuntResistanceConfigurationButton = New System.Windows.Forms.Button()
        Me._ShuntResistanceCurrentRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ShuntResistanceCurrentRangeNumericLabel = New System.Windows.Forms.Label()
        Me._ShuntResistanceLowLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ShuntResistanceLowLimitNumericLabel = New System.Windows.Forms.Label()
        Me._ShuntResistanceHighLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ShuntResistanceHighLimitNumericLabel = New System.Windows.Forms.Label()
        Me._ShuntResistanceVoltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ShuntResistanceVoltageLimitNumericLabel = New System.Windows.Forms.Label()
        Me._ShuntResistanceCurrentLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me._ShuntResistanceCurrentLevelNumericLabel = New System.Windows.Forms.Label()
        Me._MeasureShuntResistanceButton = New System.Windows.Forms.Button()
        Me._ShuntResistanceTextBoxLabel = New System.Windows.Forms.Label()
        Me._ShuntResistanceTextBox = New System.Windows.Forms.TextBox()
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._IdentityTextBox = New System.Windows.Forms.TextBox()
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._MeterTimer = New System.Windows.Forms.Timer(Me.components)
        Me._StatusStrip = New System.Windows.Forms.StatusStrip()
        Me._StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._Tabs = New System.Windows.Forms.TabControl()
        Me._ConnectTabPage = New System.Windows.Forms.TabPage()
        Me._ConnectTabLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._ConnectGroupBox = New System.Windows.Forms.GroupBox()
        Me._ResourceInfoLabel = New System.Windows.Forms.Label()
        Me._TtmConfigTabPage = New System.Windows.Forms.TabPage()
        Me._TtmTabPage = New System.Windows.Forms.TabPage()
        Me._TtmLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._MeasurementPanel = New isr.VI.Ttm.MeasurementPanel()
        Me._ShuntTabPage = New System.Windows.Forms.TabPage()
        Me._ShuntLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._ShuntDisplayLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._ShuntGroupBoxLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._ShuntConfigureGroupBoxLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._PartsTabPage = New System.Windows.Forms.TabPage()
        Me._PartsLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._PartsPanel = New isr.VI.Ttm.PartsPanel()
        Me._MessagesTabPage = New System.Windows.Forms.TabPage()
        Me._TraceMessagesBox = New isr.Core.Pith.TraceMessagesBox()
        Me._MeasurementsHeader = New isr.VI.Ttm.MeasurementsHeader()
        Me._ThermalTransientHeader = New isr.VI.Ttm.ThermalTransientHeader()
        Me._PartHeader = New isr.VI.Ttm.PartHeader()
        Me._SplitContainer = New System.Windows.Forms.SplitContainer()
        Me._NavigatorTreeView = New System.Windows.Forms.TreeView()
        Me._ResourceSelectorConnector = New isr.VI.Instrument.ResourceSelectorConnector()
        Me._MainLayout.SuspendLayout()
        Me._ConfigurationLayout.SuspendLayout()
        Me._ShuntResistanceConfigurationGroupBox.SuspendLayout()
        CType(Me._ShuntResistanceCurrentRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ShuntResistanceLowLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ShuntResistanceHighLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ShuntResistanceVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ShuntResistanceCurrentLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._StatusStrip.SuspendLayout()
        Me._Tabs.SuspendLayout()
        Me._ConnectTabPage.SuspendLayout()
        Me._ConnectTabLayout.SuspendLayout()
        Me._ConnectGroupBox.SuspendLayout()
        Me._TtmConfigTabPage.SuspendLayout()
        Me._TtmTabPage.SuspendLayout()
        Me._TtmLayout.SuspendLayout()
        Me._ShuntTabPage.SuspendLayout()
        Me._ShuntLayout.SuspendLayout()
        Me._ShuntDisplayLayout.SuspendLayout()
        Me._ShuntGroupBoxLayout.SuspendLayout()
        Me._ShuntConfigureGroupBoxLayout.SuspendLayout()
        Me._PartsTabPage.SuspendLayout()
        Me._PartsLayout.SuspendLayout()
        Me._MessagesTabPage.SuspendLayout()
        CType(Me._SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._SplitContainer.Panel1.SuspendLayout()
        Me._SplitContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        '_MainLayout
        '
        Me._MainLayout.ColumnCount = 3
        Me._MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._MainLayout.Controls.Add(Me._ConfigurationLayout, 1, 1)
        Me._MainLayout.Location = New System.Drawing.Point(13, 12)
        Me._MainLayout.Name = "_MainLayout"
        Me._MainLayout.RowCount = 3
        Me._MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._MainLayout.Size = New System.Drawing.Size(655, 453)
        Me._MainLayout.TabIndex = 0
        '
        '_ConfigurationLayout
        '
        Me._ConfigurationLayout.ColumnCount = 3
        Me._ConfigurationLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConfigurationLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ConfigurationLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConfigurationLayout.Controls.Add(Me._TTMConfigurationPanel, 1, 0)
        Me._ConfigurationLayout.Location = New System.Drawing.Point(-13, 55)
        Me._ConfigurationLayout.Name = "_ConfigurationLayout"
        Me._ConfigurationLayout.RowCount = 1
        Me._ConfigurationLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._ConfigurationLayout.Size = New System.Drawing.Size(681, 343)
        Me._ConfigurationLayout.TabIndex = 2
        '
        '_TTMConfigurationPanel
        '
        Me._TTMConfigurationPanel.BackColor = System.Drawing.Color.Transparent
        Me._TTMConfigurationPanel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TTMConfigurationPanel.IsNewConfigurationSettingAvailable = False
        Me._TTMConfigurationPanel.Location = New System.Drawing.Point(68, 4)
        Me._TTMConfigurationPanel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._TTMConfigurationPanel.Name = "_TTMConfigurationPanel"
        Me._TTMConfigurationPanel.Size = New System.Drawing.Size(545, 335)
        Me._TTMConfigurationPanel.TabIndex = 0
        '
        '_ShuntResistanceConfigurationGroupBox
        '
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._RestoreShuntResistanceDefaultsButton)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ApplyNewShuntResistanceConfigurationButton)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ApplyShuntResistanceConfigurationButton)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ShuntResistanceCurrentRangeNumeric)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ShuntResistanceCurrentRangeNumericLabel)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ShuntResistanceLowLimitNumeric)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ShuntResistanceLowLimitNumericLabel)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ShuntResistanceHighLimitNumeric)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ShuntResistanceHighLimitNumericLabel)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ShuntResistanceVoltageLimitNumeric)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ShuntResistanceVoltageLimitNumericLabel)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ShuntResistanceCurrentLevelNumeric)
        Me._ShuntResistanceConfigurationGroupBox.Controls.Add(Me._ShuntResistanceCurrentLevelNumericLabel)
        Me._ShuntResistanceConfigurationGroupBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._ShuntResistanceConfigurationGroupBox.Location = New System.Drawing.Point(39, 12)
        Me._ShuntResistanceConfigurationGroupBox.Name = "_ShuntResistanceConfigurationGroupBox"
        Me._ShuntResistanceConfigurationGroupBox.Size = New System.Drawing.Size(258, 294)
        Me._ShuntResistanceConfigurationGroupBox.TabIndex = 2
        Me._ShuntResistanceConfigurationGroupBox.TabStop = False
        Me._ShuntResistanceConfigurationGroupBox.Text = "SHUNT RESISTANCE CONFIG."
        '
        '_RestoreShuntResistanceDefaultsButton
        '
        Me._RestoreShuntResistanceDefaultsButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._RestoreShuntResistanceDefaultsButton.Location = New System.Drawing.Point(14, 252)
        Me._RestoreShuntResistanceDefaultsButton.Name = "_RestoreShuntResistanceDefaultsButton"
        Me._RestoreShuntResistanceDefaultsButton.Size = New System.Drawing.Size(234, 30)
        Me._RestoreShuntResistanceDefaultsButton.TabIndex = 12
        Me._RestoreShuntResistanceDefaultsButton.Text = "RESTORE DEFAULTS"
        Me._RestoreShuntResistanceDefaultsButton.UseVisualStyleBackColor = True
        '
        '_ApplyNewShuntResistanceConfigurationButton
        '
        Me._ApplyNewShuntResistanceConfigurationButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ApplyNewShuntResistanceConfigurationButton.Location = New System.Drawing.Point(133, 210)
        Me._ApplyNewShuntResistanceConfigurationButton.Name = "_ApplyNewShuntResistanceConfigurationButton"
        Me._ApplyNewShuntResistanceConfigurationButton.Size = New System.Drawing.Size(115, 30)
        Me._ApplyNewShuntResistanceConfigurationButton.TabIndex = 11
        Me._ApplyNewShuntResistanceConfigurationButton.Text = "APPLY CHANGES"
        Me._ApplyNewShuntResistanceConfigurationButton.UseVisualStyleBackColor = True
        '
        '_ApplyShuntResistanceConfigurationButton
        '
        Me._ApplyShuntResistanceConfigurationButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ApplyShuntResistanceConfigurationButton.Location = New System.Drawing.Point(11, 209)
        Me._ApplyShuntResistanceConfigurationButton.Name = "_ApplyShuntResistanceConfigurationButton"
        Me._ApplyShuntResistanceConfigurationButton.Size = New System.Drawing.Size(115, 30)
        Me._ApplyShuntResistanceConfigurationButton.TabIndex = 10
        Me._ApplyShuntResistanceConfigurationButton.Text = "APPLY ALL"
        Me._ToolTip.SetToolTip(Me._ApplyShuntResistanceConfigurationButton, "Clears last measurement and applies shunt resistance configuration")
        Me._ApplyShuntResistanceConfigurationButton.UseVisualStyleBackColor = True
        '
        '_ShuntResistanceCurrentRangeNumeric
        '
        Me._ShuntResistanceCurrentRangeNumeric.DecimalPlaces = 3
        Me._ShuntResistanceCurrentRangeNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ShuntResistanceCurrentRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me._ShuntResistanceCurrentRangeNumeric.Location = New System.Drawing.Point(161, 62)
        Me._ShuntResistanceCurrentRangeNumeric.Maximum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._ShuntResistanceCurrentRangeNumeric.Name = "_ShuntResistanceCurrentRangeNumeric"
        Me._ShuntResistanceCurrentRangeNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ShuntResistanceCurrentRangeNumeric.TabIndex = 3
        Me._ShuntResistanceCurrentRangeNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._ShuntResistanceCurrentRangeNumeric, "Current range in Amperes")
        Me._ShuntResistanceCurrentRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        '_ShuntResistanceCurrentRangeNumericLabel
        '
        Me._ShuntResistanceCurrentRangeNumericLabel.AutoSize = True
        Me._ShuntResistanceCurrentRangeNumericLabel.Location = New System.Drawing.Point(25, 66)
        Me._ShuntResistanceCurrentRangeNumericLabel.Name = "_ShuntResistanceCurrentRangeNumericLabel"
        Me._ShuntResistanceCurrentRangeNumericLabel.Size = New System.Drawing.Size(134, 17)
        Me._ShuntResistanceCurrentRangeNumericLabel.TabIndex = 2
        Me._ShuntResistanceCurrentRangeNumericLabel.Text = "CURRENT RANGE [A]:"
        Me._ShuntResistanceCurrentRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ShuntResistanceLowLimitNumeric
        '
        Me._ShuntResistanceLowLimitNumeric.DecimalPlaces = 1
        Me._ShuntResistanceLowLimitNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ShuntResistanceLowLimitNumeric.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me._ShuntResistanceLowLimitNumeric.Location = New System.Drawing.Point(161, 172)
        Me._ShuntResistanceLowLimitNumeric.Maximum = New Decimal(New Integer() {2000, 0, 0, 0})
        Me._ShuntResistanceLowLimitNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
        Me._ShuntResistanceLowLimitNumeric.Name = "_ShuntResistanceLowLimitNumeric"
        Me._ShuntResistanceLowLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._ShuntResistanceLowLimitNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ShuntResistanceLowLimitNumeric.TabIndex = 9
        Me._ShuntResistanceLowLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._ShuntResistanceLowLimitNumeric, "Low limit of passed shunt resistance")
        Me._ShuntResistanceLowLimitNumeric.Value = New Decimal(New Integer() {1150, 0, 0, 0})
        '
        '_ShuntResistanceLowLimitNumericLabel
        '
        Me._ShuntResistanceLowLimitNumericLabel.AutoSize = True
        Me._ShuntResistanceLowLimitNumericLabel.Location = New System.Drawing.Point(63, 176)
        Me._ShuntResistanceLowLimitNumericLabel.Name = "_ShuntResistanceLowLimitNumericLabel"
        Me._ShuntResistanceLowLimitNumericLabel.Size = New System.Drawing.Size(96, 17)
        Me._ShuntResistanceLowLimitNumericLabel.TabIndex = 8
        Me._ShuntResistanceLowLimitNumericLabel.Text = "LO&W LIMIT [Ω]:"
        Me._ShuntResistanceLowLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ShuntResistanceHighLimitNumeric
        '
        Me._ShuntResistanceHighLimitNumeric.DecimalPlaces = 1
        Me._ShuntResistanceHighLimitNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ShuntResistanceHighLimitNumeric.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me._ShuntResistanceHighLimitNumeric.Location = New System.Drawing.Point(161, 135)
        Me._ShuntResistanceHighLimitNumeric.Maximum = New Decimal(New Integer() {3000, 0, 0, 0})
        Me._ShuntResistanceHighLimitNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
        Me._ShuntResistanceHighLimitNumeric.Name = "_ShuntResistanceHighLimitNumeric"
        Me._ShuntResistanceHighLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._ShuntResistanceHighLimitNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ShuntResistanceHighLimitNumeric.TabIndex = 7
        Me._ShuntResistanceHighLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._ShuntResistanceHighLimitNumeric, "High limit of passed shunt resistance.")
        Me._ShuntResistanceHighLimitNumeric.Value = New Decimal(New Integer() {1250, 0, 0, 0})
        '
        '_ShuntResistanceHighLimitNumericLabel
        '
        Me._ShuntResistanceHighLimitNumericLabel.AutoSize = True
        Me._ShuntResistanceHighLimitNumericLabel.Location = New System.Drawing.Point(61, 139)
        Me._ShuntResistanceHighLimitNumericLabel.Name = "_ShuntResistanceHighLimitNumericLabel"
        Me._ShuntResistanceHighLimitNumericLabel.Size = New System.Drawing.Size(98, 17)
        Me._ShuntResistanceHighLimitNumericLabel.TabIndex = 6
        Me._ShuntResistanceHighLimitNumericLabel.Text = "&HIGH LIMIT [Ω]:"
        Me._ShuntResistanceHighLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ShuntResistanceVoltageLimitNumeric
        '
        Me._ShuntResistanceVoltageLimitNumeric.DecimalPlaces = 2
        Me._ShuntResistanceVoltageLimitNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ShuntResistanceVoltageLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me._ShuntResistanceVoltageLimitNumeric.Location = New System.Drawing.Point(161, 98)
        Me._ShuntResistanceVoltageLimitNumeric.Maximum = New Decimal(New Integer() {40, 0, 0, 0})
        Me._ShuntResistanceVoltageLimitNumeric.Name = "_ShuntResistanceVoltageLimitNumeric"
        Me._ShuntResistanceVoltageLimitNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ShuntResistanceVoltageLimitNumeric.TabIndex = 5
        Me._ShuntResistanceVoltageLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._ShuntResistanceVoltageLimitNumeric, "Voltage Limit in Volts")
        Me._ShuntResistanceVoltageLimitNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        '_ShuntResistanceVoltageLimitNumericLabel
        '
        Me._ShuntResistanceVoltageLimitNumericLabel.AutoSize = True
        Me._ShuntResistanceVoltageLimitNumericLabel.Location = New System.Drawing.Point(38, 102)
        Me._ShuntResistanceVoltageLimitNumericLabel.Name = "_ShuntResistanceVoltageLimitNumericLabel"
        Me._ShuntResistanceVoltageLimitNumericLabel.Size = New System.Drawing.Size(119, 17)
        Me._ShuntResistanceVoltageLimitNumericLabel.TabIndex = 4
        Me._ShuntResistanceVoltageLimitNumericLabel.Text = "&VOLTAGE LIMIT [V]:"
        Me._ShuntResistanceVoltageLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ShuntResistanceCurrentLevelNumeric
        '
        Me._ShuntResistanceCurrentLevelNumeric.DecimalPlaces = 4
        Me._ShuntResistanceCurrentLevelNumeric.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ShuntResistanceCurrentLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me._ShuntResistanceCurrentLevelNumeric.Location = New System.Drawing.Point(161, 27)
        Me._ShuntResistanceCurrentLevelNumeric.Maximum = New Decimal(New Integer() {100, 0, 0, 262144})
        Me._ShuntResistanceCurrentLevelNumeric.Name = "_ShuntResistanceCurrentLevelNumeric"
        Me._ShuntResistanceCurrentLevelNumeric.Size = New System.Drawing.Size(75, 25)
        Me._ShuntResistanceCurrentLevelNumeric.TabIndex = 1
        Me._ShuntResistanceCurrentLevelNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._ShuntResistanceCurrentLevelNumeric, "Current Level in Amperes")
        Me._ShuntResistanceCurrentLevelNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        '_ShuntResistanceCurrentLevelNumericLabel
        '
        Me._ShuntResistanceCurrentLevelNumericLabel.AutoSize = True
        Me._ShuntResistanceCurrentLevelNumericLabel.Location = New System.Drawing.Point(33, 31)
        Me._ShuntResistanceCurrentLevelNumericLabel.Name = "_ShuntResistanceCurrentLevelNumericLabel"
        Me._ShuntResistanceCurrentLevelNumericLabel.Size = New System.Drawing.Size(126, 17)
        Me._ShuntResistanceCurrentLevelNumericLabel.TabIndex = 0
        Me._ShuntResistanceCurrentLevelNumericLabel.Text = "C&URRENT LEVEL [A]:"
        Me._ShuntResistanceCurrentLevelNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_MeasureShuntResistanceButton
        '
        Me._MeasureShuntResistanceButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._MeasureShuntResistanceButton.Location = New System.Drawing.Point(53, 13)
        Me._MeasureShuntResistanceButton.Name = "_MeasureShuntResistanceButton"
        Me._MeasureShuntResistanceButton.Size = New System.Drawing.Size(230, 36)
        Me._MeasureShuntResistanceButton.TabIndex = 3
        Me._MeasureShuntResistanceButton.Text = "READ SHUNT RESISTANCE"
        Me._MeasureShuntResistanceButton.UseVisualStyleBackColor = True
        '
        '_ShuntResistanceTextBoxLabel
        '
        Me._ShuntResistanceTextBoxLabel.AutoSize = True
        Me._ShuntResistanceTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._ShuntResistanceTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ShuntResistanceTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._ShuntResistanceTextBoxLabel.Location = New System.Drawing.Point(68, 5)
        Me._ShuntResistanceTextBoxLabel.Name = "_ShuntResistanceTextBoxLabel"
        Me._ShuntResistanceTextBoxLabel.Size = New System.Drawing.Size(201, 17)
        Me._ShuntResistanceTextBoxLabel.TabIndex = 6
        Me._ShuntResistanceTextBoxLabel.Text = "SHUNT RESISTANCE [Ω]"
        Me._ShuntResistanceTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_ShuntResistanceTextBox
        '
        Me._ShuntResistanceTextBox.BackColor = System.Drawing.Color.Black
        Me._ShuntResistanceTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._ShuntResistanceTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ShuntResistanceTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._ShuntResistanceTextBox.Location = New System.Drawing.Point(68, 25)
        Me._ShuntResistanceTextBox.Name = "_ShuntResistanceTextBox"
        Me._ShuntResistanceTextBox.ReadOnly = True
        Me._ShuntResistanceTextBox.Size = New System.Drawing.Size(201, 39)
        Me._ShuntResistanceTextBox.TabIndex = 7
        Me._ShuntResistanceTextBox.Text = "0.000"
        Me._ShuntResistanceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._ShuntResistanceTextBox, "Measured Shunt Resistance")
        '
        '_ToolTip
        '
        Me._ToolTip.IsBalloon = True
        '
        '_IdentityTextBox
        '
        Me._IdentityTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._IdentityTextBox.Location = New System.Drawing.Point(28, 61)
        Me._IdentityTextBox.Name = "_IdentityTextBox"
        Me._IdentityTextBox.ReadOnly = True
        Me._IdentityTextBox.Size = New System.Drawing.Size(578, 25)
        Me._IdentityTextBox.TabIndex = 3
        Me._ToolTip.SetToolTip(Me._IdentityTextBox, "Displays the meter identity information")
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        '_StatusStrip
        '
        Me._StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusLabel})
        Me._StatusStrip.Location = New System.Drawing.Point(0, 751)
        Me._StatusStrip.Name = "_StatusStrip"
        Me._StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
        Me._StatusStrip.Size = New System.Drawing.Size(788, 22)
        Me._StatusStrip.TabIndex = 1
        Me._StatusStrip.Text = "StatusStrip1"
        '
        '_StatusLabel
        '
        Me._StatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._StatusLabel.Name = "_StatusLabel"
        Me._StatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._StatusLabel.Size = New System.Drawing.Size(771, 17)
        Me._StatusLabel.Spring = True
        Me._StatusLabel.Text = "Loading..."
        Me._StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_Tabs
        '
        Me._Tabs.Controls.Add(Me._ConnectTabPage)
        Me._Tabs.Controls.Add(Me._TtmConfigTabPage)
        Me._Tabs.Controls.Add(Me._TtmTabPage)
        Me._Tabs.Controls.Add(Me._ShuntTabPage)
        Me._Tabs.Controls.Add(Me._PartsTabPage)
        Me._Tabs.Controls.Add(Me._MessagesTabPage)
        Me._Tabs.Location = New System.Drawing.Point(56, 205)
        Me._Tabs.Multiline = True
        Me._Tabs.Name = "_Tabs"
        Me._Tabs.Padding = New System.Drawing.Point(12, 3)
        Me._Tabs.SelectedIndex = 0
        Me._Tabs.Size = New System.Drawing.Size(676, 537)
        Me._Tabs.TabIndex = 2
        '
        '_ConnectTabPage
        '
        Me._ConnectTabPage.Controls.Add(Me._ConnectTabLayout)
        Me._ConnectTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ConnectTabPage.Name = "_ConnectTabPage"
        Me._ConnectTabPage.Size = New System.Drawing.Size(668, 507)
        Me._ConnectTabPage.TabIndex = 2
        Me._ConnectTabPage.Text = "CONNECT"
        Me._ConnectTabPage.UseVisualStyleBackColor = True
        '
        '_ConnectTabLayout
        '
        Me._ConnectTabLayout.ColumnCount = 3
        Me._ConnectTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConnectTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ConnectTabLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConnectTabLayout.Controls.Add(Me._ConnectGroupBox, 1, 1)
        Me._ConnectTabLayout.Location = New System.Drawing.Point(12, 12)
        Me._ConnectTabLayout.Name = "_ConnectTabLayout"
        Me._ConnectTabLayout.RowCount = 3
        Me._ConnectTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConnectTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ConnectTabLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ConnectTabLayout.Size = New System.Drawing.Size(644, 403)
        Me._ConnectTabLayout.TabIndex = 0
        '
        '_ConnectGroupBox
        '
        Me._ConnectGroupBox.Controls.Add(Me._ResourceSelectorConnector)
        Me._ConnectGroupBox.Controls.Add(Me._ResourceInfoLabel)
        Me._ConnectGroupBox.Controls.Add(Me._IdentityTextBox)
        Me._ConnectGroupBox.Location = New System.Drawing.Point(5, 100)
        Me._ConnectGroupBox.Name = "_ConnectGroupBox"
        Me._ConnectGroupBox.Size = New System.Drawing.Size(634, 203)
        Me._ConnectGroupBox.TabIndex = 2
        Me._ConnectGroupBox.TabStop = False
        Me._ConnectGroupBox.Text = "CONNECT"
        '
        '_ResourceInfoLabel
        '
        Me._ResourceInfoLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ResourceInfoLabel.Location = New System.Drawing.Point(28, 89)
        Me._ResourceInfoLabel.Name = "_ResourceInfoLabel"
        Me._ResourceInfoLabel.Size = New System.Drawing.Size(578, 102)
        Me._ResourceInfoLabel.TabIndex = 4
        Me._ResourceInfoLabel.Text = resources.GetString("_ResourceInfoLabel.Text")
        '
        '_TtmConfigTabPage
        '
        Me._TtmConfigTabPage.Controls.Add(Me._MainLayout)
        Me._TtmConfigTabPage.Location = New System.Drawing.Point(4, 26)
        Me._TtmConfigTabPage.Name = "_TtmConfigTabPage"
        Me._TtmConfigTabPage.Size = New System.Drawing.Size(668, 507)
        Me._TtmConfigTabPage.TabIndex = 0
        Me._TtmConfigTabPage.Text = "TTM CONFIG."
        Me._TtmConfigTabPage.UseVisualStyleBackColor = True
        '
        '_TtmTabPage
        '
        Me._TtmTabPage.Controls.Add(Me._TtmLayout)
        Me._TtmTabPage.Location = New System.Drawing.Point(4, 26)
        Me._TtmTabPage.Name = "_TtmTabPage"
        Me._TtmTabPage.Size = New System.Drawing.Size(668, 507)
        Me._TtmTabPage.TabIndex = 4
        Me._TtmTabPage.Text = "TTM"
        Me._TtmTabPage.UseVisualStyleBackColor = True
        '
        '_TtmLayout
        '
        Me._TtmLayout.ColumnCount = 3
        Me._TtmLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._TtmLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._TtmLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._TtmLayout.Controls.Add(Me._MeasurementPanel, 1, 1)
        Me._TtmLayout.Location = New System.Drawing.Point(16, 16)
        Me._TtmLayout.Name = "_TtmLayout"
        Me._TtmLayout.RowCount = 3
        Me._TtmLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._TtmLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._TtmLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._TtmLayout.Size = New System.Drawing.Size(589, 413)
        Me._TtmLayout.TabIndex = 0
        '
        '_MeasurementPanel
        '
        Me._MeasurementPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._MeasurementPanel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._MeasurementPanel.Location = New System.Drawing.Point(23, 23)
        Me._MeasurementPanel.Name = "_MeasurementPanel"
        Me._MeasurementPanel.Size = New System.Drawing.Size(543, 367)
        Me._MeasurementPanel.TabIndex = 0
        '
        '_ShuntTabPage
        '
        Me._ShuntTabPage.Controls.Add(Me._ShuntLayout)
        Me._ShuntTabPage.Location = New System.Drawing.Point(4, 26)
        Me._ShuntTabPage.Name = "_ShuntTabPage"
        Me._ShuntTabPage.Size = New System.Drawing.Size(668, 507)
        Me._ShuntTabPage.TabIndex = 3
        Me._ShuntTabPage.Text = "SHUNT"
        Me._ShuntTabPage.UseVisualStyleBackColor = True
        '
        '_ShuntLayout
        '
        Me._ShuntLayout.ColumnCount = 3
        Me._ShuntLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ShuntLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ShuntLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ShuntLayout.Controls.Add(Me._ShuntDisplayLayout, 1, 1)
        Me._ShuntLayout.Controls.Add(Me._ShuntGroupBoxLayout, 1, 3)
        Me._ShuntLayout.Controls.Add(Me._ShuntConfigureGroupBoxLayout, 1, 2)
        Me._ShuntLayout.Location = New System.Drawing.Point(5, 5)
        Me._ShuntLayout.Name = "_ShuntLayout"
        Me._ShuntLayout.RowCount = 5
        Me._ShuntLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ShuntLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ShuntLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ShuntLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ShuntLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ShuntLayout.Size = New System.Drawing.Size(466, 492)
        Me._ShuntLayout.TabIndex = 0
        '
        '_ShuntDisplayLayout
        '
        Me._ShuntDisplayLayout.BackColor = System.Drawing.Color.Black
        Me._ShuntDisplayLayout.ColumnCount = 3
        Me._ShuntDisplayLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65.0!))
        Me._ShuntDisplayLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ShuntDisplayLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65.0!))
        Me._ShuntDisplayLayout.Controls.Add(Me._ShuntResistanceTextBox, 1, 2)
        Me._ShuntDisplayLayout.Controls.Add(Me._ShuntResistanceTextBoxLabel, 1, 1)
        Me._ShuntDisplayLayout.Dock = System.Windows.Forms.DockStyle.Left
        Me._ShuntDisplayLayout.Location = New System.Drawing.Point(64, 11)
        Me._ShuntDisplayLayout.Name = "_ShuntDisplayLayout"
        Me._ShuntDisplayLayout.RowCount = 4
        Me._ShuntDisplayLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me._ShuntDisplayLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ShuntDisplayLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ShuntDisplayLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me._ShuntDisplayLayout.Size = New System.Drawing.Size(337, 75)
        Me._ShuntDisplayLayout.TabIndex = 0
        '
        '_ShuntGroupBoxLayout
        '
        Me._ShuntGroupBoxLayout.ColumnCount = 3
        Me._ShuntGroupBoxLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ShuntGroupBoxLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ShuntGroupBoxLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ShuntGroupBoxLayout.Controls.Add(Me._MeasureShuntResistanceButton, 1, 1)
        Me._ShuntGroupBoxLayout.Location = New System.Drawing.Point(64, 417)
        Me._ShuntGroupBoxLayout.Name = "_ShuntGroupBoxLayout"
        Me._ShuntGroupBoxLayout.RowCount = 3
        Me._ShuntGroupBoxLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me._ShuntGroupBoxLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ShuntGroupBoxLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me._ShuntGroupBoxLayout.Size = New System.Drawing.Size(337, 63)
        Me._ShuntGroupBoxLayout.TabIndex = 2
        '
        '_ShuntConfigureGroupBoxLayout
        '
        Me._ShuntConfigureGroupBoxLayout.ColumnCount = 3
        Me._ShuntConfigureGroupBoxLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ShuntConfigureGroupBoxLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._ShuntConfigureGroupBoxLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ShuntConfigureGroupBoxLayout.Controls.Add(Me._ShuntResistanceConfigurationGroupBox, 1, 1)
        Me._ShuntConfigureGroupBoxLayout.Location = New System.Drawing.Point(64, 92)
        Me._ShuntConfigureGroupBoxLayout.Name = "_ShuntConfigureGroupBoxLayout"
        Me._ShuntConfigureGroupBoxLayout.RowCount = 3
        Me._ShuntConfigureGroupBoxLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ShuntConfigureGroupBoxLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._ShuntConfigureGroupBoxLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._ShuntConfigureGroupBoxLayout.Size = New System.Drawing.Size(337, 319)
        Me._ShuntConfigureGroupBoxLayout.TabIndex = 3
        '
        '_PartsTabPage
        '
        Me._PartsTabPage.Controls.Add(Me._PartsLayout)
        Me._PartsTabPage.Location = New System.Drawing.Point(4, 26)
        Me._PartsTabPage.Name = "_PartsTabPage"
        Me._PartsTabPage.Size = New System.Drawing.Size(668, 507)
        Me._PartsTabPage.TabIndex = 5
        Me._PartsTabPage.Text = "PARTS"
        Me._PartsTabPage.UseVisualStyleBackColor = True
        '
        '_PartsLayout
        '
        Me._PartsLayout.ColumnCount = 3
        Me._PartsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._PartsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._PartsLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._PartsLayout.Controls.Add(Me._PartsPanel, 1, 1)
        Me._PartsLayout.Location = New System.Drawing.Point(14, 7)
        Me._PartsLayout.Name = "_PartsLayout"
        Me._PartsLayout.RowCount = 3
        Me._PartsLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._PartsLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._PartsLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._PartsLayout.Size = New System.Drawing.Size(573, 489)
        Me._PartsLayout.TabIndex = 0
        '
        '_PartsPanel
        '
        Me._PartsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._PartsPanel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PartsPanel.Location = New System.Drawing.Point(23, 24)
        Me._PartsPanel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._PartsPanel.Name = "_PartsPanel"
        Me._PartsPanel.Size = New System.Drawing.Size(527, 441)
        Me._PartsPanel.TabIndex = 0
        '
        '_MessagesTabPage
        '
        Me._MessagesTabPage.Controls.Add(Me._TraceMessagesBox)
        Me._MessagesTabPage.Location = New System.Drawing.Point(4, 26)
        Me._MessagesTabPage.Name = "_MessagesTabPage"
        Me._MessagesTabPage.Size = New System.Drawing.Size(668, 507)
        Me._MessagesTabPage.TabIndex = 1
        Me._MessagesTabPage.Text = "Log"
        Me._MessagesTabPage.UseVisualStyleBackColor = True
        '
        '_TraceMessagesBox
        '
        Me._TraceMessagesBox.AlertLevel = System.Diagnostics.TraceEventType.Warning
        Me._TraceMessagesBox.BackColor = System.Drawing.SystemColors.Info
        Me._TraceMessagesBox.CaptionFormat = "{0} ≡"
        Me._TraceMessagesBox.CausesValidation = False
        Me._TraceMessagesBox.Font = New System.Drawing.Font("Consolas", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TraceMessagesBox.Location = New System.Drawing.Point(0, 0)
        Me._TraceMessagesBox.Multiline = True
        Me._TraceMessagesBox.Name = "_TraceMessagesBox"
        Me._TraceMessagesBox.PresetCount = 100
        Me._TraceMessagesBox.ReadOnly = True
        Me._TraceMessagesBox.ResetCount = 200
        Me._TraceMessagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._TraceMessagesBox.Size = New System.Drawing.Size(601, 471)
        Me._TraceMessagesBox.TabIndex = 0
        '
        '_MeasurementsHeader
        '
        Me._MeasurementsHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._MeasurementsHeader.BackColor = System.Drawing.Color.Black
        Me._MeasurementsHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me._MeasurementsHeader.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._MeasurementsHeader.Location = New System.Drawing.Point(0, 0)
        Me._MeasurementsHeader.Name = "_MeasurementsHeader"
        Me._MeasurementsHeader.Size = New System.Drawing.Size(788, 55)
        Me._MeasurementsHeader.TabIndex = 3
        '
        '_ThermalTransientHeader
        '
        Me._ThermalTransientHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._ThermalTransientHeader.BackColor = System.Drawing.Color.Black
        Me._ThermalTransientHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me._ThermalTransientHeader.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ThermalTransientHeader.Location = New System.Drawing.Point(0, 55)
        Me._ThermalTransientHeader.Name = "_ThermalTransientHeader"
        Me._ThermalTransientHeader.Size = New System.Drawing.Size(788, 64)
        Me._ThermalTransientHeader.TabIndex = 4
        '
        '_PartHeader
        '
        Me._PartHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._PartHeader.BackColor = System.Drawing.SystemColors.Desktop
        Me._PartHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me._PartHeader.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PartHeader.Location = New System.Drawing.Point(0, 119)
        Me._PartHeader.Margin = New System.Windows.Forms.Padding(0)
        Me._PartHeader.Name = "_PartHeader"
        Me._PartHeader.Size = New System.Drawing.Size(788, 26)
        Me._PartHeader.TabIndex = 5
        '
        '_SplitContainer
        '
        Me._SplitContainer.Location = New System.Drawing.Point(12, 413)
        Me._SplitContainer.Name = "_SplitContainer"
        '
        '_SplitContainer.Panel1
        '
        Me._SplitContainer.Panel1.Controls.Add(Me._NavigatorTreeView)
        Me._SplitContainer.Size = New System.Drawing.Size(264, 231)
        Me._SplitContainer.SplitterDistance = 133
        Me._SplitContainer.TabIndex = 6
        '
        '_NavigatorTreeView
        '
        Me._NavigatorTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me._NavigatorTreeView.Enabled = False
        Me._NavigatorTreeView.Location = New System.Drawing.Point(0, 0)
        Me._NavigatorTreeView.Name = "_NavigatorTreeView"
        Me._NavigatorTreeView.Size = New System.Drawing.Size(133, 231)
        Me._NavigatorTreeView.TabIndex = 3
        '
        '_ResourceSelectorConnector
        '
        Me._ResourceSelectorConnector.BackColor = System.Drawing.Color.Transparent
        Me._ResourceSelectorConnector.Clearable = False
        Me._ResourceSelectorConnector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ResourceSelectorConnector.Location = New System.Drawing.Point(28, 21)
        Me._ResourceSelectorConnector.Margin = New System.Windows.Forms.Padding(0)
        Me._ResourceSelectorConnector.Name = "_ResourceSelectorConnector"
        Me._ResourceSelectorConnector.Size = New System.Drawing.Size(578, 29)
        Me._ResourceSelectorConnector.TabIndex = 5
        Me._ToolTip.SetToolTip(Me._ResourceSelectorConnector, "Find resources and connect")
        '
        'Console
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.ClientSize = New System.Drawing.Size(788, 773)
        Me.Controls.Add(Me._SplitContainer)
        Me.Controls.Add(Me._Tabs)
        Me.Controls.Add(Me._PartHeader)
        Me.Controls.Add(Me._ThermalTransientHeader)
        Me.Controls.Add(Me._StatusStrip)
        Me.Controls.Add(Me._MeasurementsHeader)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Console"
        Me.Text = "TTM Console"
        Me._MainLayout.ResumeLayout(False)
        Me._ConfigurationLayout.ResumeLayout(False)
        Me._ShuntResistanceConfigurationGroupBox.ResumeLayout(False)
        Me._ShuntResistanceConfigurationGroupBox.PerformLayout()
        CType(Me._ShuntResistanceCurrentRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ShuntResistanceLowLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ShuntResistanceHighLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ShuntResistanceVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ShuntResistanceCurrentLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._StatusStrip.ResumeLayout(False)
        Me._StatusStrip.PerformLayout()
        Me._Tabs.ResumeLayout(False)
        Me._ConnectTabPage.ResumeLayout(False)
        Me._ConnectTabLayout.ResumeLayout(False)
        Me._ConnectGroupBox.ResumeLayout(False)
        Me._ConnectGroupBox.PerformLayout()
        Me._TtmConfigTabPage.ResumeLayout(False)
        Me._TtmTabPage.ResumeLayout(False)
        Me._TtmLayout.ResumeLayout(False)
        Me._ShuntTabPage.ResumeLayout(False)
        Me._ShuntLayout.ResumeLayout(False)
        Me._ShuntDisplayLayout.ResumeLayout(False)
        Me._ShuntDisplayLayout.PerformLayout()
        Me._ShuntGroupBoxLayout.ResumeLayout(False)
        Me._ShuntConfigureGroupBoxLayout.ResumeLayout(False)
        Me._PartsTabPage.ResumeLayout(False)
        Me._PartsLayout.ResumeLayout(False)
        Me._MessagesTabPage.ResumeLayout(False)
        Me._MessagesTabPage.PerformLayout()
        Me._SplitContainer.Panel1.ResumeLayout(False)
        CType(Me._SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SplitContainer.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _MainLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _ConfigurationLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _ErrorProvider As System.Windows.Forms.ErrorProvider
    Private WithEvents _MeterTimer As System.Windows.Forms.Timer
    Private WithEvents _ShuntResistanceConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents _ApplyShuntResistanceConfigurationButton As System.Windows.Forms.Button
    Private WithEvents _ShuntResistanceCurrentRangeNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ShuntResistanceCurrentRangeNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ShuntResistanceLowLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ShuntResistanceLowLimitNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ShuntResistanceHighLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ShuntResistanceHighLimitNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ShuntResistanceVoltageLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ShuntResistanceVoltageLimitNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ShuntResistanceCurrentLevelNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents _ShuntResistanceCurrentLevelNumericLabel As System.Windows.Forms.Label
    Private WithEvents _ShuntResistanceTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _ShuntResistanceTextBox As System.Windows.Forms.TextBox
    Private WithEvents _MeasureShuntResistanceButton As System.Windows.Forms.Button
    Private WithEvents _StatusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents _StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _Tabs As System.Windows.Forms.TabControl
    Private WithEvents _TtmConfigTabPage As System.Windows.Forms.TabPage
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _TraceMessagesBox As isr.Core.Pith.TraceMessagesBox
    Private WithEvents _ConnectTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ConnectTabLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _ConnectGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents _IdentityTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ApplyNewShuntResistanceConfigurationButton As System.Windows.Forms.Button
    Private WithEvents _ShuntTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ShuntLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _ShuntDisplayLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TtmTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ShuntGroupBoxLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _ShuntConfigureGroupBoxLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _TtmLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _MeasurementsHeader As MeasurementsHeader
    Private WithEvents _TTMConfigurationPanel As ConfigurationPanel
    Private WithEvents _RestoreShuntResistanceDefaultsButton As System.Windows.Forms.Button
    Private WithEvents _MeasurementPanel As MeasurementPanel
    Private WithEvents _ThermalTransientHeader As ThermalTransientHeader
    Private WithEvents _PartHeader As PartHeader
    Private WithEvents _SplitContainer As System.Windows.Forms.SplitContainer
    Private WithEvents _NavigatorTreeView As System.Windows.Forms.TreeView
    Private WithEvents _PartsTabPage As System.Windows.Forms.TabPage
    Private WithEvents _PartsLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _PartsPanel As PartsPanel
    Private WithEvents _ResourceInfoLabel As Windows.Forms.Label
    Private WithEvents _ResourceSelectorConnector As Instrument.ResourceSelectorConnector
End Class
