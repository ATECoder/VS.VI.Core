<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MeasurementPanelBase

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MeasurementPanelBase))
        Me._TtmToolStripContainer = New System.Windows.Forms.ToolStripContainer()
        Me._TtmMeasureControlsToolStrip = New System.Windows.Forms.ToolStrip()
        Me._MeasureToolStripDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me._AbortSequenceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._MeasureAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._FinalResistanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ThermalTransientToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._InitialResistanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._TtmToolStripProgressBar = New System.Windows.Forms.ToolStripProgressBar()
        Me._ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me._TriggerToolStripDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me._WaitForTriggerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._AssertTriggerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._AbortToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._TriggerActionToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me._ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me._TraceToolStripDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me._ModelTraceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._SaveTraceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ClearTraceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._ReadTraceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me._Chart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._MeterTimer = New System.Windows.Forms.Timer(Me.components)
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._TraceDataGridView = New System.Windows.Forms.DataGridView()
        Me._SplitContainer = New System.Windows.Forms.SplitContainer()
        Me._TtmToolStripContainer.BottomToolStripPanel.SuspendLayout()
        Me._TtmToolStripContainer.ContentPanel.SuspendLayout()
        Me._TtmToolStripContainer.SuspendLayout()
        Me._TtmMeasureControlsToolStrip.SuspendLayout()
        CType(Me._Chart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._TraceDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._SplitContainer.Panel1.SuspendLayout()
        Me._SplitContainer.Panel2.SuspendLayout()
        Me._SplitContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        '_TtmToolStripContainer
        '
        '
        '_TtmToolStripContainer.BottomToolStripPanel
        '
        Me._TtmToolStripContainer.BottomToolStripPanel.Controls.Add(Me._TtmMeasureControlsToolStrip)
        '
        '_TtmToolStripContainer.ContentPanel
        '
        Me._TtmToolStripContainer.ContentPanel.Controls.Add(Me._SplitContainer)
        Me._TtmToolStripContainer.ContentPanel.Size = New System.Drawing.Size(523, 427)
        Me._TtmToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me._TtmToolStripContainer.Location = New System.Drawing.Point(0, 0)
        Me._TtmToolStripContainer.Name = "_TtmToolStripContainer"
        Me._TtmToolStripContainer.Size = New System.Drawing.Size(523, 477)
        Me._TtmToolStripContainer.TabIndex = 7
        Me._TtmToolStripContainer.Text = "ToolStripContainer1"
        '
        '_TtmMeasureControlsToolStrip
        '
        Me._TtmMeasureControlsToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._TtmMeasureControlsToolStrip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TtmMeasureControlsToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._MeasureToolStripDropDownButton, Me._TtmToolStripProgressBar, Me._ToolStripSeparator1, Me._TriggerToolStripDropDownButton, Me._TriggerActionToolStripLabel, Me._ToolStripSeparator2, Me._TraceToolStripDropDownButton})
        Me._TtmMeasureControlsToolStrip.Location = New System.Drawing.Point(3, 0)
        Me._TtmMeasureControlsToolStrip.Name = "_TtmMeasureControlsToolStrip"
        Me._TtmMeasureControlsToolStrip.Size = New System.Drawing.Size(389, 25)
        Me._TtmMeasureControlsToolStrip.TabIndex = 0
        Me._TtmMeasureControlsToolStrip.Text = "TTM Measure Controls"
        '
        '_MeasureToolStripDropDownButton
        '
        Me._MeasureToolStripDropDownButton.AutoToolTip = False
        Me._MeasureToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._MeasureToolStripDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._AbortSequenceToolStripMenuItem, Me._MeasureAllToolStripMenuItem, Me._FinalResistanceToolStripMenuItem, Me._ThermalTransientToolStripMenuItem, Me._InitialResistanceToolStripMenuItem, Me._ClearToolStripMenuItem})
        Me._MeasureToolStripDropDownButton.Image = CType(resources.GetObject("_MeasureToolStripDropDownButton.Image"), System.Drawing.Image)
        Me._MeasureToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._MeasureToolStripDropDownButton.Name = "_MeasureToolStripDropDownButton"
        Me._MeasureToolStripDropDownButton.Size = New System.Drawing.Size(78, 22)
        Me._MeasureToolStripDropDownButton.Text = "MEASURE:"
        Me._MeasureToolStripDropDownButton.ToolTipText = "Measure thermal transient elements"
        '
        '_AbortSequenceToolStripMenuItem
        '
        Me._AbortSequenceToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.MediaPlaybackStop2
        Me._AbortSequenceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._AbortSequenceToolStripMenuItem.Name = "_AbortSequenceToolStripMenuItem"
        Me._AbortSequenceToolStripMenuItem.Size = New System.Drawing.Size(223, 38)
        Me._AbortSequenceToolStripMenuItem.Text = "ABORT"
        Me._AbortSequenceToolStripMenuItem.ToolTipText = "Abort measurement sequence"
        '
        '_MeasureAllToolStripMenuItem
        '
        Me._MeasureAllToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.ArrowRightDouble
        Me._MeasureAllToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._MeasureAllToolStripMenuItem.Name = "_MeasureAllToolStripMenuItem"
        Me._MeasureAllToolStripMenuItem.Size = New System.Drawing.Size(223, 38)
        Me._MeasureAllToolStripMenuItem.Text = "ALL"
        Me._MeasureAllToolStripMenuItem.ToolTipText = "Measure initial resistance, thermal transient, and final resistance"
        '
        '_FinalResistanceToolStripMenuItem
        '
        Me._FinalResistanceToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.ArrowRight2
        Me._FinalResistanceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._FinalResistanceToolStripMenuItem.Name = "_FinalResistanceToolStripMenuItem"
        Me._FinalResistanceToolStripMenuItem.Size = New System.Drawing.Size(223, 38)
        Me._FinalResistanceToolStripMenuItem.Text = "FINAL RESISTANCE"
        Me._FinalResistanceToolStripMenuItem.ToolTipText = "Measure Final Resistance"
        '
        '_ThermalTransientToolStripMenuItem
        '
        Me._ThermalTransientToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.ArrowRight2
        Me._ThermalTransientToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._ThermalTransientToolStripMenuItem.Name = "_ThermalTransientToolStripMenuItem"
        Me._ThermalTransientToolStripMenuItem.Size = New System.Drawing.Size(223, 38)
        Me._ThermalTransientToolStripMenuItem.Text = "THERMAL TRANSIENT"
        Me._ThermalTransientToolStripMenuItem.ToolTipText = "Measure Thermal Transient"
        '
        '_InitialResistanceToolStripMenuItem
        '
        Me._InitialResistanceToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.ArrowRight2
        Me._InitialResistanceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._InitialResistanceToolStripMenuItem.Name = "_InitialResistanceToolStripMenuItem"
        Me._InitialResistanceToolStripMenuItem.Size = New System.Drawing.Size(223, 38)
        Me._InitialResistanceToolStripMenuItem.Text = "INITIAL RESISTANCE"
        Me._InitialResistanceToolStripMenuItem.ToolTipText = "Measure Initial Resistance"
        '
        '_ClearToolStripMenuItem
        '
        Me._ClearToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.EditClear2
        Me._ClearToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._ClearToolStripMenuItem.Name = "_ClearToolStripMenuItem"
        Me._ClearToolStripMenuItem.Size = New System.Drawing.Size(223, 38)
        Me._ClearToolStripMenuItem.Text = "CLEAR MEASUREMENTS"
        Me._ClearToolStripMenuItem.ToolTipText = "Clear measurements on next Initial Resistance measurement"
        '
        '_TtmToolStripProgressBar
        '
        Me._TtmToolStripProgressBar.AutoSize = False
        Me._TtmToolStripProgressBar.Name = "_TtmToolStripProgressBar"
        Me._TtmToolStripProgressBar.Size = New System.Drawing.Size(100, 15)
        Me._TtmToolStripProgressBar.ToolTipText = "Measurement sequence progress"
        '
        '_ToolStripSeparator1
        '
        Me._ToolStripSeparator1.Name = "_ToolStripSeparator1"
        Me._ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        '_TriggerToolStripDropDownButton
        '
        Me._TriggerToolStripDropDownButton.AutoToolTip = False
        Me._TriggerToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TriggerToolStripDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._WaitForTriggerToolStripMenuItem, Me._AssertTriggerToolStripMenuItem, Me._AbortToolStripMenuItem})
        Me._TriggerToolStripDropDownButton.Image = CType(resources.GetObject("_TriggerToolStripDropDownButton.Image"), System.Drawing.Image)
        Me._TriggerToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._TriggerToolStripDropDownButton.Name = "_TriggerToolStripDropDownButton"
        Me._TriggerToolStripDropDownButton.Size = New System.Drawing.Size(71, 22)
        Me._TriggerToolStripDropDownButton.Text = "TRIGGER"
        Me._TriggerToolStripDropDownButton.ToolTipText = "Trigger Options"
        '
        '_WaitForTriggerToolStripMenuItem
        '
        Me._WaitForTriggerToolStripMenuItem.CheckOnClick = True
        Me._WaitForTriggerToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.ViewRefresh7
        Me._WaitForTriggerToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._WaitForTriggerToolStripMenuItem.Name = "_WaitForTriggerToolStripMenuItem"
        Me._WaitForTriggerToolStripMenuItem.Size = New System.Drawing.Size(179, 38)
        Me._WaitForTriggerToolStripMenuItem.Text = "Wait for Trigger"
        Me._WaitForTriggerToolStripMenuItem.ToolTipText = "Check to enable waiting for trigger"
        '
        '_AssertTriggerToolStripMenuItem
        '
        Me._AssertTriggerToolStripMenuItem.Enabled = False
        Me._AssertTriggerToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.EditAdd
        Me._AssertTriggerToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._AssertTriggerToolStripMenuItem.Name = "_AssertTriggerToolStripMenuItem"
        Me._AssertTriggerToolStripMenuItem.Size = New System.Drawing.Size(179, 38)
        Me._AssertTriggerToolStripMenuItem.Text = "Assert Trigger"
        Me._AssertTriggerToolStripMenuItem.ToolTipText = "Assert instrument trigger"
        '
        '_AbortToolStripMenuItem
        '
        Me._AbortToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.ProcessStop
        Me._AbortToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._AbortToolStripMenuItem.Name = "_AbortToolStripMenuItem"
        Me._AbortToolStripMenuItem.Size = New System.Drawing.Size(179, 38)
        Me._AbortToolStripMenuItem.Text = "ABORT"
        Me._AbortToolStripMenuItem.ToolTipText = "Forces abort"
        '
        '_TriggerActionToolStripLabel
        '
        Me._TriggerActionToolStripLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TriggerActionToolStripLabel.Name = "_TriggerActionToolStripLabel"
        Me._TriggerActionToolStripLabel.Size = New System.Drawing.Size(52, 22)
        Me._TriggerActionToolStripLabel.Text = "Inactive"
        '
        '_ToolStripSeparator2
        '
        Me._ToolStripSeparator2.Name = "_ToolStripSeparator2"
        Me._ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        '_TraceToolStripDropDownButton
        '
        Me._TraceToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._TraceToolStripDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ModelTraceToolStripMenuItem, Me._SaveTraceToolStripMenuItem, Me._ClearTraceToolStripMenuItem, Me._ReadTraceToolStripMenuItem})
        Me._TraceToolStripDropDownButton.Image = CType(resources.GetObject("_TraceToolStripDropDownButton.Image"), System.Drawing.Image)
        Me._TraceToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._TraceToolStripDropDownButton.Name = "_TraceToolStripDropDownButton"
        Me._TraceToolStripDropDownButton.Size = New System.Drawing.Size(62, 22)
        Me._TraceToolStripDropDownButton.Text = "TRACE: "
        '
        '_ModelTraceToolStripMenuItem
        '
        Me._ModelTraceToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.GamesSolve
        Me._ModelTraceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._ModelTraceToolStripMenuItem.Name = "_ModelTraceToolStripMenuItem"
        Me._ModelTraceToolStripMenuItem.Size = New System.Drawing.Size(148, 38)
        Me._ModelTraceToolStripMenuItem.Text = "CURVE FIT"
        Me._ModelTraceToolStripMenuItem.ToolTipText = "Fix a model to the trace"
        '
        '_SaveTraceToolStripMenuItem
        '
        Me._SaveTraceToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.DocumentExport
        Me._SaveTraceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._SaveTraceToolStripMenuItem.Name = "_SaveTraceToolStripMenuItem"
        Me._SaveTraceToolStripMenuItem.Size = New System.Drawing.Size(148, 38)
        Me._SaveTraceToolStripMenuItem.Text = "SAVE"
        Me._SaveTraceToolStripMenuItem.ToolTipText = "Save trace values to file"
        '
        '_ClearTraceToolStripMenuItem
        '
        Me._ClearTraceToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.EditClear2
        Me._ClearTraceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._ClearTraceToolStripMenuItem.Name = "_ClearTraceToolStripMenuItem"
        Me._ClearTraceToolStripMenuItem.Size = New System.Drawing.Size(148, 38)
        Me._ClearTraceToolStripMenuItem.Text = "CLEAR"
        Me._ClearTraceToolStripMenuItem.ToolTipText = "Clear trace chart and list"
        '
        '_ReadTraceToolStripMenuItem
        '
        Me._ReadTraceToolStripMenuItem.Image = Global.isr.VI.Ttm.My.Resources.Resources.ViewObjectHistogramLogarithmic
        Me._ReadTraceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._ReadTraceToolStripMenuItem.Name = "_ReadTraceToolStripMenuItem"
        Me._ReadTraceToolStripMenuItem.Size = New System.Drawing.Size(148, 38)
        Me._ReadTraceToolStripMenuItem.Text = "READ"
        '
        '_Chart
        '
        Me._Chart.BorderlineColor = System.Drawing.Color.Aqua
        Me._Chart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        Me._Chart.Dock = System.Windows.Forms.DockStyle.Fill
        Me._Chart.Location = New System.Drawing.Point(0, 0)
        Me._Chart.Name = "_Chart"
        Me._Chart.Size = New System.Drawing.Size(330, 427)
        Me._Chart.TabIndex = 0
        Me._Chart.Text = "Chart1"
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        '_ToolTip
        '
        Me._ToolTip.IsBalloon = True
        '
        '_TraceDataGridView
        '
        Me._TraceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me._TraceDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me._TraceDataGridView.Location = New System.Drawing.Point(0, 0)
        Me._TraceDataGridView.Name = "_TraceDataGridView"
        Me._TraceDataGridView.Size = New System.Drawing.Size(189, 427)
        Me._TraceDataGridView.TabIndex = 1
        '
        '_SplitContainer
        '
        Me._SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me._SplitContainer.Location = New System.Drawing.Point(0, 0)
        Me._SplitContainer.Name = "_SplitContainer"
        '
        '_SplitContainer.Panel1
        '
        Me._SplitContainer.Panel1.Controls.Add(Me._Chart)
        '
        '_SplitContainer.Panel2
        '
        Me._SplitContainer.Panel2.Controls.Add(Me._TraceDataGridView)
        Me._SplitContainer.Size = New System.Drawing.Size(523, 427)
        Me._SplitContainer.SplitterDistance = 330
        Me._SplitContainer.TabIndex = 2
        '
        'MeasurementPanelBase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me._TtmToolStripContainer)
        Me.Name = "MeasurementPanelBase"
        Me.Size = New System.Drawing.Size(523, 477)
        Me._TtmToolStripContainer.BottomToolStripPanel.ResumeLayout(False)
        Me._TtmToolStripContainer.BottomToolStripPanel.PerformLayout()
        Me._TtmToolStripContainer.ContentPanel.ResumeLayout(False)
        Me._TtmToolStripContainer.ResumeLayout(False)
        Me._TtmToolStripContainer.PerformLayout()
        Me._TtmMeasureControlsToolStrip.ResumeLayout(False)
        Me._TtmMeasureControlsToolStrip.PerformLayout()
        CType(Me._Chart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._TraceDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SplitContainer.Panel1.ResumeLayout(False)
        Me._SplitContainer.Panel2.ResumeLayout(False)
        CType(Me._SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SplitContainer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _TtmToolStripContainer As System.Windows.Forms.ToolStripContainer
    Private WithEvents _TtmMeasureControlsToolStrip As System.Windows.Forms.ToolStrip
    Private WithEvents _TtmToolStripProgressBar As System.Windows.Forms.ToolStripProgressBar
    Private WithEvents _Chart As System.Windows.Forms.DataVisualization.Charting.Chart
    Private WithEvents _ErrorProvider As System.Windows.Forms.ErrorProvider
    Private WithEvents _MeterTimer As System.Windows.Forms.Timer
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _MeasureToolStripDropDownButton As System.Windows.Forms.ToolStripDropDownButton
    Private WithEvents _AbortSequenceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _MeasureAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _FinalResistanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _ThermalTransientToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _InitialResistanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _TriggerToolStripDropDownButton As System.Windows.Forms.ToolStripDropDownButton
    Private WithEvents _WaitForTriggerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _AssertTriggerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _TriggerActionToolStripLabel As System.Windows.Forms.ToolStripLabel
    Private WithEvents _ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _TraceToolStripDropDownButton As System.Windows.Forms.ToolStripDropDownButton
    Private WithEvents _ModelTraceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _SaveTraceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _ClearTraceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _ReadTraceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _AbortToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _TraceDataGridView As System.Windows.Forms.DataGridView
    Private WithEvents _SplitContainer As System.Windows.Forms.SplitContainer

End Class
