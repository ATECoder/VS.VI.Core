Imports System.Windows.Forms
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ResistancesMeterControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me._ResourceSelectorConnector = New isr.VI.Instrument.ResourceSelectorConnector()
        Me._Layout = New System.Windows.Forms.TableLayoutPanel()
        Me._BridgeMeterLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._ConfigureGroupBox = New System.Windows.Forms.GroupBox()
        Me._ConfigureButton = New System.Windows.Forms.Button()
        Me._FilterCountNumericLabel = New System.Windows.Forms.Label()
        Me._PowerLineCyclesNumeric = New System.Windows.Forms.NumericUpDown()
        Me._FilterCountNumeric = New System.Windows.Forms.NumericUpDown()
        Me._PowerLineCyclesNumericLabel = New System.Windows.Forms.Label()
        Me._MeasureGroupBox = New System.Windows.Forms.GroupBox()
        Me._DataGrid = New System.Windows.Forms.DataGridView()
        Me._MeasureButton = New System.Windows.Forms.Button()
        Me._ConnectorGroupBox = New System.Windows.Forms.GroupBox()
        Me._BottomToolStrip = New System.Windows.Forms.ToolStrip()
        Me._GageTitleLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ReadingLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ChannelLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ComplianceLabel = New System.Windows.Forms.ToolStripLabel()
        Me._Layout.SuspendLayout()
        Me._BridgeMeterLayout.SuspendLayout()
        Me._ConfigureGroupBox.SuspendLayout()
        CType(Me._PowerLineCyclesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._FilterCountNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._MeasureGroupBox.SuspendLayout()
        CType(Me._DataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ConnectorGroupBox.SuspendLayout()
        Me._BottomToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        '_ResourceSelectorConnector
        '
        Me._ResourceSelectorConnector.BackColor = System.Drawing.Color.Transparent
        Me._ResourceSelectorConnector.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ResourceSelectorConnector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ResourceSelectorConnector.Location = New System.Drawing.Point(3, 21)
        Me._ResourceSelectorConnector.Margin = New System.Windows.Forms.Padding(0)
        Me._ResourceSelectorConnector.Name = "_ResourceSelectorConnector"
        Me._ResourceSelectorConnector.Size = New System.Drawing.Size(563, 29)
        Me._ResourceSelectorConnector.TabIndex = 0
        '
        '_Layout
        '
        Me._Layout.ColumnCount = 3
        Me._Layout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._Layout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._Layout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._Layout.Controls.Add(Me._BridgeMeterLayout, 1, 2)
        Me._Layout.Controls.Add(Me._ConnectorGroupBox, 1, 1)
        Me._Layout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Layout.Location = New System.Drawing.Point(0, 0)
        Me._Layout.Name = "_Layout"
        Me._Layout.RowCount = 4
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6.0!))
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6.0!))
        Me._Layout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._Layout.Size = New System.Drawing.Size(609, 322)
        Me._Layout.TabIndex = 1
        '
        '_BridgeMeterLayout
        '
        Me._BridgeMeterLayout.ColumnCount = 4
        Me._BridgeMeterLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._BridgeMeterLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._BridgeMeterLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._BridgeMeterLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._BridgeMeterLayout.Controls.Add(Me._ConfigureGroupBox, 1, 1)
        Me._BridgeMeterLayout.Controls.Add(Me._MeasureGroupBox, 2, 1)
        Me._BridgeMeterLayout.Location = New System.Drawing.Point(20, 68)
        Me._BridgeMeterLayout.Name = "_BridgeMeterLayout"
        Me._BridgeMeterLayout.RowCount = 3
        Me._BridgeMeterLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._BridgeMeterLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._BridgeMeterLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._BridgeMeterLayout.Size = New System.Drawing.Size(569, 239)
        Me._BridgeMeterLayout.TabIndex = 3
        '
        '_ConfigureGroupBox
        '
        Me._ConfigureGroupBox.Controls.Add(Me._ConfigureButton)
        Me._ConfigureGroupBox.Controls.Add(Me._FilterCountNumericLabel)
        Me._ConfigureGroupBox.Controls.Add(Me._PowerLineCyclesNumeric)
        Me._ConfigureGroupBox.Controls.Add(Me._FilterCountNumeric)
        Me._ConfigureGroupBox.Controls.Add(Me._PowerLineCyclesNumericLabel)
        Me._ConfigureGroupBox.Dock = System.Windows.Forms.DockStyle.Left
        Me._ConfigureGroupBox.Location = New System.Drawing.Point(33, 24)
        Me._ConfigureGroupBox.Name = "_ConfigureGroupBox"
        Me._ConfigureGroupBox.Size = New System.Drawing.Size(207, 191)
        Me._ConfigureGroupBox.TabIndex = 0
        Me._ConfigureGroupBox.TabStop = False
        Me._ConfigureGroupBox.Text = "Configure"
        '
        '_ConfigureButton
        '
        Me._ConfigureButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ConfigureButton.Location = New System.Drawing.Point(124, 157)
        Me._ConfigureButton.Name = "_ConfigureButton"
        Me._ConfigureButton.Size = New System.Drawing.Size(75, 28)
        Me._ConfigureButton.TabIndex = 2
        Me._ConfigureButton.Text = "&APPLY"
        Me._ConfigureButton.UseVisualStyleBackColor = True
        '
        '_FilterCountNumericLabel
        '
        Me._FilterCountNumericLabel.AutoSize = True
        Me._FilterCountNumericLabel.Location = New System.Drawing.Point(46, 52)
        Me._FilterCountNumericLabel.Name = "_FilterCountNumericLabel"
        Me._FilterCountNumericLabel.Size = New System.Drawing.Size(77, 17)
        Me._FilterCountNumericLabel.TabIndex = 6
        Me._FilterCountNumericLabel.Text = "Filter Count:"
        Me._FilterCountNumericLabel.Visible = False
        '
        '_PowerLineCyclesNumeric
        '
        Me._PowerLineCyclesNumeric.DecimalPlaces = 3
        Me._PowerLineCyclesNumeric.Location = New System.Drawing.Point(125, 19)
        Me._PowerLineCyclesNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 196608})
        Me._PowerLineCyclesNumeric.Name = "_PowerLineCyclesNumeric"
        Me._PowerLineCyclesNumeric.Size = New System.Drawing.Size(74, 25)
        Me._PowerLineCyclesNumeric.TabIndex = 3
        Me._PowerLineCyclesNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        '_FilterCountNumeric
        '
        Me._FilterCountNumeric.DecimalPlaces = 4
        Me._FilterCountNumeric.Location = New System.Drawing.Point(125, 48)
        Me._FilterCountNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me._FilterCountNumeric.Name = "_FilterCountNumeric"
        Me._FilterCountNumeric.Size = New System.Drawing.Size(74, 25)
        Me._FilterCountNumeric.TabIndex = 5
        Me._FilterCountNumeric.Visible = False
        '
        '_PowerLineCyclesNumericLabel
        '
        Me._PowerLineCyclesNumericLabel.AutoSize = True
        Me._PowerLineCyclesNumericLabel.Location = New System.Drawing.Point(9, 23)
        Me._PowerLineCyclesNumericLabel.Name = "_PowerLineCyclesNumericLabel"
        Me._PowerLineCyclesNumericLabel.Size = New System.Drawing.Size(114, 17)
        Me._PowerLineCyclesNumericLabel.TabIndex = 4
        Me._PowerLineCyclesNumericLabel.Text = "Power Line Cycles:"
        '
        '_MeasureGroupBox
        '
        Me._MeasureGroupBox.Controls.Add(Me._DataGrid)
        Me._MeasureGroupBox.Controls.Add(Me._MeasureButton)
        Me._MeasureGroupBox.Location = New System.Drawing.Point(246, 24)
        Me._MeasureGroupBox.Name = "_MeasureGroupBox"
        Me._MeasureGroupBox.Size = New System.Drawing.Size(289, 191)
        Me._MeasureGroupBox.TabIndex = 1
        Me._MeasureGroupBox.TabStop = False
        Me._MeasureGroupBox.Text = "Measure"
        '
        '_DataGrid
        '
        Me._DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me._DataGrid.Location = New System.Drawing.Point(9, 19)
        Me._DataGrid.Name = "_DataGrid"
        Me._DataGrid.Size = New System.Drawing.Size(272, 128)
        Me._DataGrid.TabIndex = 8
        '
        '_MeasureButton
        '
        Me._MeasureButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._MeasureButton.Location = New System.Drawing.Point(203, 155)
        Me._MeasureButton.Name = "_MeasureButton"
        Me._MeasureButton.Size = New System.Drawing.Size(75, 28)
        Me._MeasureButton.TabIndex = 7
        Me._MeasureButton.Text = "&MEASURE"
        Me._MeasureButton.UseVisualStyleBackColor = True
        '
        '_ConnectorGroupBox
        '
        Me._ConnectorGroupBox.Controls.Add(Me._ResourceSelectorConnector)
        Me._ConnectorGroupBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._ConnectorGroupBox.Location = New System.Drawing.Point(20, 9)
        Me._ConnectorGroupBox.Name = "_ConnectorGroupBox"
        Me._ConnectorGroupBox.Size = New System.Drawing.Size(569, 53)
        Me._ConnectorGroupBox.TabIndex = 4
        Me._ConnectorGroupBox.TabStop = False
        Me._ConnectorGroupBox.Text = "Instrument Resource Name Finder and Connector"
        '
        '_BottomToolStrip
        '
        Me._BottomToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._BottomToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._GageTitleLabel, Me._ReadingLabel, Me._ChannelLabel, Me._ComplianceLabel})
        Me._BottomToolStrip.Location = New System.Drawing.Point(0, 322)
        Me._BottomToolStrip.Name = "_BottomToolStrip"
        Me._BottomToolStrip.Size = New System.Drawing.Size(609, 25)
        Me._BottomToolStrip.TabIndex = 2
        Me._BottomToolStrip.Text = "Tool Strip"
        '
        '_GageTitleLabel
        '
        Me._GageTitleLabel.Name = "_GageTitleLabel"
        Me._GageTitleLabel.Size = New System.Drawing.Size(20, 22)
        Me._GageTitleLabel.Text = "R1"
        Me._GageTitleLabel.ToolTipText = "Gage"
        '
        '_ReadingLabel
        '
        Me._ReadingLabel.Name = "_ReadingLabel"
        Me._ReadingLabel.Size = New System.Drawing.Size(70, 22)
        Me._ReadingLabel.Text = "0.0000 Ohm"
        Me._ReadingLabel.ToolTipText = "Measured value"
        '
        '_ChannelLabel
        '
        Me._ChannelLabel.Name = "_ChannelLabel"
        Me._ChannelLabel.Size = New System.Drawing.Size(112, 22)
        Me._ChannelLabel.Text = "1001;1031;1912;1921"
        Me._ChannelLabel.ToolTipText = "Channel List"
        '
        '_ComplianceLabel
        '
        Me._ComplianceLabel.Name = "_ComplianceLabel"
        Me._ComplianceLabel.Size = New System.Drawing.Size(21, 22)
        Me._ComplianceLabel.Text = ".C."
        Me._ComplianceLabel.ToolTipText = "Measurement Compliance"
        '
        'BridgeMeterControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me._Layout)
        Me.Controls.Add(Me._BottomToolStrip)
        Me.Name = "BridgeMeterControl"
        Me.Size = New System.Drawing.Size(609, 347)
        Me._Layout.ResumeLayout(False)
        Me._BridgeMeterLayout.ResumeLayout(False)
        Me._ConfigureGroupBox.ResumeLayout(False)
        Me._ConfigureGroupBox.PerformLayout()
        CType(Me._PowerLineCyclesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._FilterCountNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me._MeasureGroupBox.ResumeLayout(False)
        CType(Me._DataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ConnectorGroupBox.ResumeLayout(False)
        Me._BottomToolStrip.ResumeLayout(False)
        Me._BottomToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents _ResourceSelectorConnector As VI.Instrument.ResourceSelectorConnector
    Private WithEvents _Layout As TableLayoutPanel
    Private WithEvents _FilterCountNumericLabel As Label
    Private WithEvents _FilterCountNumeric As NumericUpDown
    Private WithEvents _PowerLineCyclesNumericLabel As Label
    Private WithEvents _PowerLineCyclesNumeric As NumericUpDown
    Private WithEvents _ConfigureButton As Button
    Private WithEvents _BottomToolStrip As ToolStrip
    Private WithEvents _ReadingLabel As ToolStripLabel
    Private WithEvents _BridgeMeterLayout As TableLayoutPanel
    Private WithEvents _ConfigureGroupBox As GroupBox
    Private WithEvents _MeasureGroupBox As GroupBox
    Friend WithEvents _DataGrid As DataGridView
    Private WithEvents _MeasureButton As Button
    Friend WithEvents _ConnectorGroupBox As GroupBox
    Friend WithEvents _GageTitleLabel As ToolStripLabel
    Friend WithEvents _ChannelLabel As ToolStripLabel
    Friend WithEvents _ComplianceLabel As ToolStripLabel
End Class
