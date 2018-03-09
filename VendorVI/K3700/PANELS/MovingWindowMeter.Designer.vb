<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MovingWindowMeter

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MovingWindowMeter))
        Me._TopLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._RightLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._MaximumLabel = New System.Windows.Forms.Label()
        Me._MinimumLabel = New System.Windows.Forms.Label()
        Me._ReadingLabel = New System.Windows.Forms.Label()
        Me._StatusLabel = New System.Windows.Forms.Label()
        Me._MiddleLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._TaskProgressBar = New System.Windows.Forms.ProgressBar()
        Me._AverageLabel = New System.Windows.Forms.Label()
        Me._LeftLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._ElapsedTimeLabel = New System.Windows.Forms.Label()
        Me._CountLabel = New System.Windows.Forms.Label()
        Me._ReadingsCountLabel = New System.Windows.Forms.Label()
        Me._ReadingTimeSpanLabel = New System.Windows.Forms.Label()
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._SettingsToolStrip = New System.Windows.Forms.ToolStrip()
        Me._LengthTextBoxLabel = New System.Windows.Forms.ToolStripLabel()
        Me._LengthTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me._WindowTextBoxLabel = New System.Windows.Forms.ToolStripLabel()
        Me._WindowTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me._TimeoutTextBoxLabel = New System.Windows.Forms.ToolStripLabel()
        Me._TimeoutTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me._StartMovingWindowButton = New System.Windows.Forms.ToolStripButton()
        Me._TopLayout.SuspendLayout()
        Me._RightLayout.SuspendLayout()
        Me._MiddleLayout.SuspendLayout()
        Me._LeftLayout.SuspendLayout()
        Me._SettingsToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        '_TopLayout
        '
        Me._TopLayout.BackColor = System.Drawing.Color.Transparent
        Me._TopLayout.ColumnCount = 3
        Me._TopLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84.0!))
        Me._TopLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._TopLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84.0!))
        Me._TopLayout.Controls.Add(Me._RightLayout, 2, 0)
        Me._TopLayout.Controls.Add(Me._MiddleLayout, 1, 0)
        Me._TopLayout.Controls.Add(Me._LeftLayout, 0, 0)
        Me._TopLayout.Dock = System.Windows.Forms.DockStyle.Top
        Me._TopLayout.Location = New System.Drawing.Point(0, 0)
        Me._TopLayout.Name = "_TopLayout"
        Me._TopLayout.RowCount = 1
        Me._TopLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._TopLayout.Size = New System.Drawing.Size(370, 100)
        Me._TopLayout.TabIndex = 0
        '
        '_RightLayout
        '
        Me._RightLayout.ColumnCount = 1
        Me._RightLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._RightLayout.Controls.Add(Me._MaximumLabel, 0, 0)
        Me._RightLayout.Controls.Add(Me._MinimumLabel, 0, 2)
        Me._RightLayout.Controls.Add(Me._ReadingLabel, 0, 1)
        Me._RightLayout.Controls.Add(Me._StatusLabel, 0, 3)
        Me._RightLayout.Location = New System.Drawing.Point(286, 0)
        Me._RightLayout.Margin = New System.Windows.Forms.Padding(0)
        Me._RightLayout.Name = "_RightLayout"
        Me._RightLayout.RowCount = 4
        Me._RightLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._RightLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._RightLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._RightLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._RightLayout.Size = New System.Drawing.Size(84, 98)
        Me._RightLayout.TabIndex = 2
        '
        '_MaximumLabel
        '
        Me._MaximumLabel.AutoSize = True
        Me._MaximumLabel.BackColor = System.Drawing.Color.Black
        Me._MaximumLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._MaximumLabel.ForeColor = System.Drawing.Color.LightGreen
        Me._MaximumLabel.Location = New System.Drawing.Point(3, 0)
        Me._MaximumLabel.Name = "_MaximumLabel"
        Me._MaximumLabel.Size = New System.Drawing.Size(78, 24)
        Me._MaximumLabel.TabIndex = 0
        Me._MaximumLabel.Text = "maximum"
        Me._MaximumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._ToolTip.SetToolTip(Me._MaximumLabel, "current maximum")
        '
        '_MinimumLabel
        '
        Me._MinimumLabel.AutoSize = True
        Me._MinimumLabel.BackColor = System.Drawing.Color.Black
        Me._MinimumLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._MinimumLabel.ForeColor = System.Drawing.Color.LightGreen
        Me._MinimumLabel.Location = New System.Drawing.Point(3, 48)
        Me._MinimumLabel.Name = "_MinimumLabel"
        Me._MinimumLabel.Size = New System.Drawing.Size(78, 24)
        Me._MinimumLabel.TabIndex = 0
        Me._MinimumLabel.Text = "minimum"
        Me._MinimumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_ReadingLabel
        '
        Me._ReadingLabel.AutoSize = True
        Me._ReadingLabel.BackColor = System.Drawing.Color.Black
        Me._ReadingLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ReadingLabel.ForeColor = System.Drawing.Color.LightGreen
        Me._ReadingLabel.Location = New System.Drawing.Point(3, 24)
        Me._ReadingLabel.Name = "_ReadingLabel"
        Me._ReadingLabel.Size = New System.Drawing.Size(78, 24)
        Me._ReadingLabel.TabIndex = 0
        Me._ReadingLabel.Text = "reading"
        Me._ReadingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._ToolTip.SetToolTip(Me._ReadingLabel, "Last reading")
        '
        '_StatusLabel
        '
        Me._StatusLabel.AutoSize = True
        Me._StatusLabel.BackColor = System.Drawing.Color.Black
        Me._StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._StatusLabel.ForeColor = System.Drawing.Color.LightGreen
        Me._StatusLabel.Location = New System.Drawing.Point(3, 72)
        Me._StatusLabel.Name = "_StatusLabel"
        Me._StatusLabel.Size = New System.Drawing.Size(78, 26)
        Me._StatusLabel.TabIndex = 0
        Me._StatusLabel.Text = "status"
        Me._StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._ToolTip.SetToolTip(Me._StatusLabel, "Status")
        '
        '_MiddleLayout
        '
        Me._MiddleLayout.ColumnCount = 1
        Me._MiddleLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._MiddleLayout.Controls.Add(Me._TaskProgressBar, 0, 1)
        Me._MiddleLayout.Controls.Add(Me._AverageLabel, 0, 0)
        Me._MiddleLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me._MiddleLayout.Location = New System.Drawing.Point(84, 0)
        Me._MiddleLayout.Margin = New System.Windows.Forms.Padding(0)
        Me._MiddleLayout.Name = "_MiddleLayout"
        Me._MiddleLayout.RowCount = 2
        Me._MiddleLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._MiddleLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._MiddleLayout.Size = New System.Drawing.Size(202, 100)
        Me._MiddleLayout.TabIndex = 3
        '
        '_AverageProgressBar
        '
        Me._TaskProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._TaskProgressBar.Location = New System.Drawing.Point(3, 74)
        Me._TaskProgressBar.Name = "_AverageProgressBar"
        Me._TaskProgressBar.Size = New System.Drawing.Size(196, 23)
        Me._TaskProgressBar.Step = 1
        Me._TaskProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me._TaskProgressBar.TabIndex = 0
        Me._ToolTip.SetToolTip(Me._TaskProgressBar, "Progress")
        '
        '_AverageLabel
        '
        Me._AverageLabel.AutoSize = True
        Me._AverageLabel.BackColor = System.Drawing.Color.Black
        Me._AverageLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._AverageLabel.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._AverageLabel.ForeColor = System.Drawing.Color.Aquamarine
        Me._AverageLabel.Location = New System.Drawing.Point(3, 0)
        Me._AverageLabel.Name = "_AverageLabel"
        Me._AverageLabel.Size = New System.Drawing.Size(196, 71)
        Me._AverageLabel.TabIndex = 1
        Me._AverageLabel.Text = "average"
        Me._AverageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me._ToolTip.SetToolTip(Me._AverageLabel, "Current average")
        '
        '_LeftLayout
        '
        Me._LeftLayout.ColumnCount = 1
        Me._LeftLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84.0!))
        Me._LeftLayout.Controls.Add(Me._ElapsedTimeLabel, 0, 0)
        Me._LeftLayout.Controls.Add(Me._CountLabel, 0, 1)
        Me._LeftLayout.Controls.Add(Me._ReadingsCountLabel, 0, 2)
        Me._LeftLayout.Controls.Add(Me._ReadingTimeSpanLabel, 0, 3)
        Me._LeftLayout.Location = New System.Drawing.Point(0, 0)
        Me._LeftLayout.Margin = New System.Windows.Forms.Padding(0)
        Me._LeftLayout.Name = "_LeftLayout"
        Me._LeftLayout.RowCount = 4
        Me._LeftLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._LeftLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._LeftLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._LeftLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._LeftLayout.Size = New System.Drawing.Size(84, 98)
        Me._LeftLayout.TabIndex = 1
        '
        '_ElapsedTimeLabel
        '
        Me._ElapsedTimeLabel.AutoSize = True
        Me._ElapsedTimeLabel.BackColor = System.Drawing.Color.Black
        Me._ElapsedTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ElapsedTimeLabel.ForeColor = System.Drawing.Color.LightGreen
        Me._ElapsedTimeLabel.Location = New System.Drawing.Point(3, 0)
        Me._ElapsedTimeLabel.Name = "_ElapsedTimeLabel"
        Me._ElapsedTimeLabel.Size = New System.Drawing.Size(78, 24)
        Me._ElapsedTimeLabel.TabIndex = 0
        Me._ElapsedTimeLabel.Text = "elapsed"
        Me._ElapsedTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._ToolTip.SetToolTip(Me._ElapsedTimeLabel, "Elapsed time")
        '
        '_CountLabel
        '
        Me._CountLabel.AutoSize = True
        Me._CountLabel.BackColor = System.Drawing.Color.Black
        Me._CountLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._CountLabel.ForeColor = System.Drawing.Color.LightGreen
        Me._CountLabel.Location = New System.Drawing.Point(3, 24)
        Me._CountLabel.Name = "_CountLabel"
        Me._CountLabel.Size = New System.Drawing.Size(78, 24)
        Me._CountLabel.TabIndex = 0
        Me._CountLabel.Text = "count"
        Me._CountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._ToolTip.SetToolTip(Me._CountLabel, "Element count")
        '
        '_ReadingsCountLabel
        '
        Me._ReadingsCountLabel.AutoSize = True
        Me._ReadingsCountLabel.BackColor = System.Drawing.Color.Black
        Me._ReadingsCountLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ReadingsCountLabel.ForeColor = System.Drawing.Color.LightGreen
        Me._ReadingsCountLabel.Location = New System.Drawing.Point(3, 48)
        Me._ReadingsCountLabel.Name = "_ReadingsCountLabel"
        Me._ReadingsCountLabel.Size = New System.Drawing.Size(78, 24)
        Me._ReadingsCountLabel.TabIndex = 0
        Me._ReadingsCountLabel.Text = "readings"
        Me._ReadingsCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._ToolTip.SetToolTip(Me._ReadingsCountLabel, "Number of readings")
        '
        '_ReadingTimeSpanLabel
        '
        Me._ReadingTimeSpanLabel.AutoSize = True
        Me._ReadingTimeSpanLabel.BackColor = System.Drawing.Color.Black
        Me._ReadingTimeSpanLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ReadingTimeSpanLabel.ForeColor = System.Drawing.Color.LightGreen
        Me._ReadingTimeSpanLabel.Location = New System.Drawing.Point(3, 72)
        Me._ReadingTimeSpanLabel.Name = "_ReadingTimeSpanLabel"
        Me._ReadingTimeSpanLabel.Size = New System.Drawing.Size(78, 26)
        Me._ReadingTimeSpanLabel.TabIndex = 0
        Me._ReadingTimeSpanLabel.Text = "read time"
        Me._ReadingTimeSpanLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._ToolTip.SetToolTip(Me._ReadingTimeSpanLabel, "Single measurement time")
        '
        '_SettingsToolStrip
        '
        Me._SettingsToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._LengthTextBoxLabel, Me._LengthTextBox, Me._WindowTextBoxLabel, Me._WindowTextBox, Me._TimeoutTextBoxLabel, Me._TimeoutTextBox, Me._StartMovingWindowButton})
        Me._SettingsToolStrip.Location = New System.Drawing.Point(0, 100)
        Me._SettingsToolStrip.Name = "_SettingsToolStrip"
        Me._SettingsToolStrip.Size = New System.Drawing.Size(370, 25)
        Me._SettingsToolStrip.TabIndex = 1
        Me._SettingsToolStrip.Text = "ToolStrip1"
        '
        '_LengthTextBoxLabel
        '
        Me._LengthTextBoxLabel.Name = "_LengthTextBoxLabel"
        Me._LengthTextBoxLabel.Size = New System.Drawing.Size(47, 22)
        Me._LengthTextBoxLabel.Text = "Length:"
        '
        '_LengthTextBox
        '
        Me._LengthTextBox.Margin = New System.Windows.Forms.Padding(1, 0, 4, 0)
        Me._LengthTextBox.Name = "_LengthTextBox"
        Me._LengthTextBox.Size = New System.Drawing.Size(25, 25)
        Me._LengthTextBox.Text = "40"
        Me._LengthTextBox.ToolTipText = "Moving average filter length"
        '
        '_WindowTextBoxLabel
        '
        Me._WindowTextBoxLabel.Name = "_WindowTextBoxLabel"
        Me._WindowTextBoxLabel.Size = New System.Drawing.Size(75, 22)
        Me._WindowTextBoxLabel.Text = "Window [%]:"
        '
        '_WindowTextBox
        '
        Me._WindowTextBox.Margin = New System.Windows.Forms.Padding(1, 0, 4, 0)
        Me._WindowTextBox.Name = "_WindowTextBox"
        Me._WindowTextBox.Size = New System.Drawing.Size(34, 25)
        Me._WindowTextBox.Text = "0.05"
        Me._WindowTextBox.ToolTipText = "Moving average window in %"
        '
        '_TimeoutTextBoxLabel
        '
        Me._TimeoutTextBoxLabel.Name = "_TimeoutTextBoxLabel"
        Me._TimeoutTextBoxLabel.Size = New System.Drawing.Size(71, 22)
        Me._TimeoutTextBoxLabel.Text = "Timeout [s]:"
        '
        '_TimeoutTextBox
        '
        Me._TimeoutTextBox.Margin = New System.Windows.Forms.Padding(1, 0, 4, 0)
        Me._TimeoutTextBox.Name = "_TimeoutTextBox"
        Me._TimeoutTextBox.Size = New System.Drawing.Size(34, 25)
        Me._TimeoutTextBox.Text = "120"
        Me._TimeoutTextBox.ToolTipText = "Timeout in seconds"
        '
        '_StartMovingAverageButton
        '
        Me._StartMovingWindowButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._StartMovingWindowButton.CheckOnClick = True
        Me._StartMovingWindowButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me._StartMovingWindowButton.Image = CType(resources.GetObject("_StartMovingAverageButton.Image"), System.Drawing.Image)
        Me._StartMovingWindowButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._StartMovingWindowButton.Name = "_StartMovingAverageButton"
        Me._StartMovingWindowButton.Size = New System.Drawing.Size(35, 22)
        Me._StartMovingWindowButton.Text = "Start"
        Me._StartMovingWindowButton.ToolTipText = "Start the moving average"
        '
        'MovingWindowMeter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me._SettingsToolStrip)
        Me.Controls.Add(Me._TopLayout)
        Me.Name = "MovingWindowMeter"
        Me.Size = New System.Drawing.Size(370, 127)
        Me._TopLayout.ResumeLayout(False)
        Me._RightLayout.ResumeLayout(False)
        Me._RightLayout.PerformLayout()
        Me._MiddleLayout.ResumeLayout(False)
        Me._MiddleLayout.PerformLayout()
        Me._LeftLayout.ResumeLayout(False)
        Me._LeftLayout.PerformLayout()
        Me._SettingsToolStrip.ResumeLayout(False)
        Me._SettingsToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents _TopLayout As Windows.Forms.TableLayoutPanel
    Private WithEvents _MiddleLayout As Windows.Forms.TableLayoutPanel
    Private WithEvents _TaskProgressBar As Windows.Forms.ProgressBar
    Private WithEvents _ToolTip As Windows.Forms.ToolTip
    Private WithEvents _AverageLabel As Windows.Forms.Label
    Private WithEvents _LeftLayout As Windows.Forms.TableLayoutPanel
    Private WithEvents _ElapsedTimeLabel As Windows.Forms.Label
    Private WithEvents _CountLabel As Windows.Forms.Label
    Private WithEvents _ReadingsCountLabel As Windows.Forms.Label
    Private WithEvents _ReadingTimeSpanLabel As Windows.Forms.Label
    Private WithEvents _RightLayout As Windows.Forms.TableLayoutPanel
    Private WithEvents _MaximumLabel As Windows.Forms.Label
    Private WithEvents _MinimumLabel As Windows.Forms.Label
    Private WithEvents _ReadingLabel As Windows.Forms.Label
    Private WithEvents _StatusLabel As Windows.Forms.Label
    Private WithEvents _SettingsToolStrip As Windows.Forms.ToolStrip
    Private WithEvents _LengthTextBoxLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _LengthTextBox As Windows.Forms.ToolStripTextBox
    Private WithEvents _WindowTextBoxLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _WindowTextBox As Windows.Forms.ToolStripTextBox
    Private WithEvents _TimeoutTextBoxLabel As Windows.Forms.ToolStripLabel
    Private WithEvents _TimeoutTextBox As Windows.Forms.ToolStripTextBox
    Private WithEvents _StartMovingWindowButton As Windows.Forms.ToolStripButton
End Class
