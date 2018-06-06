<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MovingWindowControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._InstrumentLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._InstrumentPanel = New isr.VI.Tsp.K3700.K3700Control()
        Me._MovingWindowMeter = New isr.VI.Tsp.K3700.MovingWindowMeter()
        Me._InstrumentLayout.SuspendLayout()
        Me.SuspendLayout()
        '
        '_ToolTip
        '
        Me._ToolTip.IsBalloon = True
        '
        '_InstrumentLayout
        '
        Me._InstrumentLayout.ColumnCount = 3
        Me._InstrumentLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3.0!))
        Me._InstrumentLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._InstrumentLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3.0!))
        Me._InstrumentLayout.Controls.Add(Me._InstrumentPanel, 1, 2)
        Me._InstrumentLayout.Controls.Add(Me._MovingWindowMeter, 1, 1)
        Me._InstrumentLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._InstrumentLayout.Location = New System.Drawing.Point(0, 0)
        Me._InstrumentLayout.Name = "_InstrumentLayout"
        Me._InstrumentLayout.RowCount = 4
        Me._InstrumentLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3.0!))
        Me._InstrumentLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._InstrumentLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._InstrumentLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3.0!))
        Me._InstrumentLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._InstrumentLayout.Size = New System.Drawing.Size(412, 627)
        Me._InstrumentLayout.TabIndex = 1
        '
        '_InstrumentPanel
        '
        Me._InstrumentPanel.BackColor = System.Drawing.Color.Transparent
        Me._InstrumentPanel.ClosedResourceTitleFormat = "{0}:Closed"
        Me._InstrumentPanel.Cursor = System.Windows.Forms.Cursors.Default
        Me._InstrumentPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._InstrumentPanel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._InstrumentPanel.Location = New System.Drawing.Point(6, 135)
        Me._InstrumentPanel.Name = "_InstrumentPanel"
        Me._InstrumentPanel.OpenResourceTitleFormat = "{0}:{1}"
        Me._InstrumentPanel.Size = New System.Drawing.Size(400, 486)
        Me._InstrumentPanel.TabIndex = 0
        '
        '_MovingWindowMeter
        '
        Me._MovingWindowMeter.Dock = System.Windows.Forms.DockStyle.Top
        Me._MovingWindowMeter.ExpectedStopTimeout = System.TimeSpan.Parse("00:00:00.1000000")
        Me._MovingWindowMeter.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._MovingWindowMeter.Length = 40
        Me._MovingWindowMeter.Location = New System.Drawing.Point(6, 6)
        Me._MovingWindowMeter.Name = "_MovingWindowMeter"
        Me._MovingWindowMeter.Size = New System.Drawing.Size(400, 123)
        Me._MovingWindowMeter.TabIndex = 1
        '
        'MovingWindowControl
        '
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me._InstrumentLayout)
        Me.Location = New System.Drawing.Point(297, 150)
        Me.Name = "MovingWindowControl"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Size = New System.Drawing.Size(412, 627)
        Me._InstrumentLayout.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _InstrumentLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _InstrumentPanel As K3700Control
    Private WithEvents _MovingWindowMeter As MovingWindowMeter
End Class

