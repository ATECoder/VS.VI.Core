<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PartsPanel

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PartsPanel))
        Me._PartsListToolStrip = New System.Windows.Forms.ToolStrip()
        Me._AddPartToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me._ClearPartListToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me._SavePartsToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me._PartNumberToolStrip = New System.Windows.Forms.ToolStrip()
        Me._OperatorLabel = New System.Windows.Forms.ToolStripLabel()
        Me._OperatorToolStripTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me._LotToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._LotToolStripTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me._PartNumberToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._PartNumberToolStripTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me._ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me._PartToolStrip = New System.Windows.Forms.ToolStrip()
        Me._SerialNumberToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._SerialNumberToolStripTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me._ToolStripSplitButton = New System.Windows.Forms.ToolStripSplitButton()
        Me._AutoAddToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearMeasurementsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me._OutcomeToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._PassFailToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._PartsDataGridView = New System.Windows.Forms.DataGridView()
        Me._PartsListToolStrip.SuspendLayout()
        Me._PartNumberToolStrip.SuspendLayout()
        Me._PartToolStrip.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._PartsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_PartsListToolStrip
        '
        Me._PartsListToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._PartsListToolStrip.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PartsListToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._AddPartToolStripButton, Me._ClearPartListToolStripButton, Me._SavePartsToolStripButton})
        Me._PartsListToolStrip.Location = New System.Drawing.Point(0, 404)
        Me._PartsListToolStrip.Name = "_PartsListToolStrip"
        Me._PartsListToolStrip.Size = New System.Drawing.Size(584, 25)
        Me._PartsListToolStrip.TabIndex = 2
        Me._PartsListToolStrip.Text = "ToolStrip1"
        '
        '_AddPartToolStripButton
        '
        Me._AddPartToolStripButton.AutoToolTip = False
        Me._AddPartToolStripButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._AddPartToolStripButton.Image = CType(resources.GetObject("_AddPartToolStripButton.Image"), System.Drawing.Image)
        Me._AddPartToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._AddPartToolStripButton.Name = "_AddPartToolStripButton"
        Me._AddPartToolStripButton.Size = New System.Drawing.Size(93, 22)
        Me._AddPartToolStripButton.Text = "ADD PART"
        Me._AddPartToolStripButton.ToolTipText = "Add part to the list"
        '
        '_ClearPartListToolStripButton
        '
        Me._ClearPartListToolStripButton.AutoToolTip = False
        Me._ClearPartListToolStripButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ClearPartListToolStripButton.Image = CType(resources.GetObject("_ClearPartListToolStripButton.Image"), System.Drawing.Image)
        Me._ClearPartListToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ClearPartListToolStripButton.Name = "_ClearPartListToolStripButton"
        Me._ClearPartListToolStripButton.Size = New System.Drawing.Size(140, 22)
        Me._ClearPartListToolStripButton.Text = "CLEAR PARTS LIST"
        Me._ClearPartListToolStripButton.ToolTipText = "Clear Parts List"
        '
        '_SavePartsToolStripButton
        '
        Me._SavePartsToolStripButton.AutoToolTip = False
        Me._SavePartsToolStripButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SavePartsToolStripButton.Image = CType(resources.GetObject("_SavePartsToolStripButton.Image"), System.Drawing.Image)
        Me._SavePartsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._SavePartsToolStripButton.Name = "_SavePartsToolStripButton"
        Me._SavePartsToolStripButton.Size = New System.Drawing.Size(102, 22)
        Me._SavePartsToolStripButton.Text = "SAVE PARTS"
        Me._SavePartsToolStripButton.ToolTipText = "Save parts to file"
        '
        '_PartNumberToolStrip
        '
        Me._PartNumberToolStrip.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PartNumberToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._OperatorLabel, Me._OperatorToolStripTextBox, Me._LotToolStripLabel, Me._LotToolStripTextBox, Me._PartNumberToolStripLabel, Me._PartNumberToolStripTextBox, Me._ToolStripSeparator1})
        Me._PartNumberToolStrip.Location = New System.Drawing.Point(0, 0)
        Me._PartNumberToolStrip.Name = "_PartNumberToolStrip"
        Me._PartNumberToolStrip.Size = New System.Drawing.Size(584, 25)
        Me._PartNumberToolStrip.TabIndex = 3
        Me._PartNumberToolStrip.Text = "ToolStrip1"
        '
        '_OperatorLabel
        '
        Me._OperatorLabel.Name = "_OperatorLabel"
        Me._OperatorLabel.Size = New System.Drawing.Size(65, 22)
        Me._OperatorLabel.Text = "Operator:"
        '
        '_OperatorToolStripTextBox
        '
        Me._OperatorToolStripTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._OperatorToolStripTextBox.Name = "_OperatorToolStripTextBox"
        Me._OperatorToolStripTextBox.Size = New System.Drawing.Size(132, 25)
        Me._OperatorToolStripTextBox.ToolTipText = "Operator Id"
        '
        '_LotToolStripLabel
        '
        Me._LotToolStripLabel.Name = "_LotToolStripLabel"
        Me._LotToolStripLabel.Size = New System.Drawing.Size(29, 22)
        Me._LotToolStripLabel.Text = "Lot:"
        '
        '_LotToolStripTextBox
        '
        Me._LotToolStripTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._LotToolStripTextBox.Name = "_LotToolStripTextBox"
        Me._LotToolStripTextBox.Size = New System.Drawing.Size(132, 25)
        Me._LotToolStripTextBox.ToolTipText = "Lot number"
        '
        '_PartNumberToolStripLabel
        '
        Me._PartNumberToolStripLabel.Name = "_PartNumberToolStripLabel"
        Me._PartNumberToolStripLabel.Size = New System.Drawing.Size(34, 22)
        Me._PartNumberToolStripLabel.Text = "Part:"
        '
        '_PartNumberToolStripTextBox
        '
        Me._PartNumberToolStripTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PartNumberToolStripTextBox.Name = "_PartNumberToolStripTextBox"
        Me._PartNumberToolStripTextBox.Size = New System.Drawing.Size(132, 25)
        Me._PartNumberToolStripTextBox.ToolTipText = "Part number"
        '
        '_ToolStripSeparator1
        '
        Me._ToolStripSeparator1.Name = "_ToolStripSeparator1"
        Me._ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        '_PartToolStrip
        '
        Me._PartToolStrip.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PartToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SerialNumberToolStripLabel, Me._SerialNumberToolStripTextBox, Me._ToolStripSplitButton, Me._ToolStripSeparator2, Me._OutcomeToolStripLabel, Me._PassFailToolStripButton})
        Me._PartToolStrip.Location = New System.Drawing.Point(0, 25)
        Me._PartToolStrip.Name = "_PartToolStrip"
        Me._PartToolStrip.Size = New System.Drawing.Size(584, 25)
        Me._PartToolStrip.TabIndex = 4
        Me._PartToolStrip.Text = "ToolStrip1"
        '
        '_SerialNumberToolStripLabel
        '
        Me._SerialNumberToolStripLabel.Name = "_SerialNumberToolStripLabel"
        Me._SerialNumberToolStripLabel.Size = New System.Drawing.Size(33, 22)
        Me._SerialNumberToolStripLabel.Text = "S/N:"
        '
        '_SerialNumberToolStripTextBox
        '
        Me._SerialNumberToolStripTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SerialNumberToolStripTextBox.Name = "_SerialNumberToolStripTextBox"
        Me._SerialNumberToolStripTextBox.Size = New System.Drawing.Size(132, 25)
        Me._SerialNumberToolStripTextBox.ToolTipText = "Serial Number"
        '
        '_ToolStripSplitButton
        '
        Me._ToolStripSplitButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._AutoAddToolStripMenuItem, Me._ClearMeasurementsToolStripMenuItem})
        Me._ToolStripSplitButton.Image = CType(resources.GetObject("_ToolStripSplitButton.Image"), System.Drawing.Image)
        Me._ToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ToolStripSplitButton.Name = "_ToolStripSplitButton"
        Me._ToolStripSplitButton.Size = New System.Drawing.Size(86, 22)
        Me._ToolStripSplitButton.Text = "Options"
        '
        '_AutoAddToolStripMenuItem
        '
        Me._AutoAddToolStripMenuItem.CheckOnClick = True
        Me._AutoAddToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._AutoAddToolStripMenuItem.Name = "_AutoAddToolStripMenuItem"
        Me._AutoAddToolStripMenuItem.Size = New System.Drawing.Size(222, 22)
        Me._AutoAddToolStripMenuItem.Text = "AUTO ADD"
        Me._AutoAddToolStripMenuItem.ToolTipText = "Automatically add parts when measurements complete."
        '
        '_ClearMeasurementsToolStripMenuItem
        '
        Me._ClearMeasurementsToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ClearMeasurementsToolStripMenuItem.Name = "_ClearMeasurementsToolStripMenuItem"
        Me._ClearMeasurementsToolStripMenuItem.Size = New System.Drawing.Size(222, 22)
        Me._ClearMeasurementsToolStripMenuItem.Text = "CLEAR MEASUREMENTS"
        Me._ClearMeasurementsToolStripMenuItem.ToolTipText = "Clears all measurements on the current part"
        '
        '_ToolStripSeparator2
        '
        Me._ToolStripSeparator2.Name = "_ToolStripSeparator2"
        Me._ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        '_OutcomeToolStripLabel
        '
        Me._OutcomeToolStripLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._OutcomeToolStripLabel.Name = "_OutcomeToolStripLabel"
        Me._OutcomeToolStripLabel.Size = New System.Drawing.Size(80, 22)
        Me._OutcomeToolStripLabel.Text = "<outcome>"
        '
        '_PassFailToolStripButton
        '
        Me._PassFailToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._PassFailToolStripButton.Image = Global.isr.Vi.Ttm.My.Resources.Resources.Good
        Me._PassFailToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._PassFailToolStripButton.Name = "_PassFailToolStripButton"
        Me._PassFailToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me._PassFailToolStripButton.Visible = False
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        '_PartsDataGridView
        '
        Me._PartsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me._PartsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me._PartsDataGridView.Location = New System.Drawing.Point(0, 50)
        Me._PartsDataGridView.Name = "_PartsDataGridView"
        Me._PartsDataGridView.Size = New System.Drawing.Size(584, 354)
        Me._PartsDataGridView.TabIndex = 5
        '
        'PartsPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me._PartsDataGridView)
        Me.Controls.Add(Me._PartToolStrip)
        Me.Controls.Add(Me._PartNumberToolStrip)
        Me.Controls.Add(Me._PartsListToolStrip)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "PartsPanel"
        Me.Size = New System.Drawing.Size(584, 429)
        Me._PartsListToolStrip.ResumeLayout(False)
        Me._PartsListToolStrip.PerformLayout()
        Me._PartNumberToolStrip.ResumeLayout(False)
        Me._PartNumberToolStrip.PerformLayout()
        Me._PartToolStrip.ResumeLayout(False)
        Me._PartToolStrip.PerformLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._PartsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _PartsListToolStrip As System.Windows.Forms.ToolStrip
    Private WithEvents _AddPartToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents _ClearPartListToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents _SavePartsToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents _PartNumberToolStrip As System.Windows.Forms.ToolStrip
    Private WithEvents _OperatorLabel As System.Windows.Forms.ToolStripLabel
    Private WithEvents _OperatorToolStripTextBox As System.Windows.Forms.ToolStripTextBox
    Private WithEvents _LotToolStripLabel As System.Windows.Forms.ToolStripLabel
    Private WithEvents _LotToolStripTextBox As System.Windows.Forms.ToolStripTextBox
    Private WithEvents _PartNumberToolStripLabel As System.Windows.Forms.ToolStripLabel
    Private WithEvents _PartNumberToolStripTextBox As System.Windows.Forms.ToolStripTextBox
    Private WithEvents _PartToolStrip As System.Windows.Forms.ToolStrip
    Private WithEvents _SerialNumberToolStripLabel As System.Windows.Forms.ToolStripLabel
    Private WithEvents _SerialNumberToolStripTextBox As System.Windows.Forms.ToolStripTextBox
    Private WithEvents _ToolStripSplitButton As System.Windows.Forms.ToolStripSplitButton
    Private WithEvents _AutoAddToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearMeasurementsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _ErrorProvider As System.Windows.Forms.ErrorProvider
    Private WithEvents _ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _OutcomeToolStripLabel As System.Windows.Forms.ToolStripLabel
    Private WithEvents _PassFailToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents _PartsDataGridView As System.Windows.Forms.DataGridView

End Class
