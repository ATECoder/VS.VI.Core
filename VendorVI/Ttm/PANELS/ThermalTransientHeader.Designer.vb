<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ThermalTransientHeader

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                Me.releaseResources()
                If disposing Then
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
        Me._OutcomeLayout = New System.Windows.Forms.TableLayoutPanel()
        Me._AsymptoteTextBox = New System.Windows.Forms.TextBox()
        Me._ThermalTransientVoltageTextBoxLabel = New System.Windows.Forms.Label()
        Me._EstimatedVoltageTextBox = New System.Windows.Forms.TextBox()
        Me._EstimatedVoltageTextBoxLabel = New System.Windows.Forms.Label()
        Me._TimeConstantTextBoxLabel = New System.Windows.Forms.Label()
        Me._TimeConstantTextBox = New System.Windows.Forms.TextBox()
        Me._CorrelationCoefficientTextBoxLabel = New System.Windows.Forms.Label()
        Me._CorrelationCoefficientTextBox = New System.Windows.Forms.TextBox()
        Me._StandardErrorTextBox = New System.Windows.Forms.TextBox()
        Me._IterationsCountTextBox = New System.Windows.Forms.TextBox()
        Me._OutcomeTextBox = New System.Windows.Forms.TextBox()
        Me._StandardErrorTextBoxLabel = New System.Windows.Forms.Label()
        Me._IterationsCountTextBoxLabel = New System.Windows.Forms.Label()
        Me._OutcomeTextBoxLabel = New System.Windows.Forms.Label()
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._OutcomeLayout.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_OutcomeLayout
        '
        Me._OutcomeLayout.BackColor = System.Drawing.Color.Transparent
        Me._OutcomeLayout.ColumnCount = 7
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._OutcomeLayout.Controls.Add(Me._AsymptoteTextBox, 1, 2)
        Me._OutcomeLayout.Controls.Add(Me._ThermalTransientVoltageTextBoxLabel, 1, 1)
        Me._OutcomeLayout.Controls.Add(Me._EstimatedVoltageTextBox, 2, 2)
        Me._OutcomeLayout.Controls.Add(Me._EstimatedVoltageTextBoxLabel, 2, 1)
        Me._OutcomeLayout.Controls.Add(Me._TimeConstantTextBoxLabel, 0, 1)
        Me._OutcomeLayout.Controls.Add(Me._TimeConstantTextBox, 0, 2)
        Me._OutcomeLayout.Controls.Add(Me._CorrelationCoefficientTextBoxLabel, 3, 1)
        Me._OutcomeLayout.Controls.Add(Me._CorrelationCoefficientTextBox, 3, 2)
        Me._OutcomeLayout.Controls.Add(Me._StandardErrorTextBox, 4, 2)
        Me._OutcomeLayout.Controls.Add(Me._IterationsCountTextBox, 5, 2)
        Me._OutcomeLayout.Controls.Add(Me._OutcomeTextBox, 6, 2)
        Me._OutcomeLayout.Controls.Add(Me._StandardErrorTextBoxLabel, 4, 1)
        Me._OutcomeLayout.Controls.Add(Me._IterationsCountTextBoxLabel, 5, 1)
        Me._OutcomeLayout.Controls.Add(Me._OutcomeTextBoxLabel, 6, 1)
        Me._OutcomeLayout.Dock = System.Windows.Forms.DockStyle.Top
        Me._OutcomeLayout.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me._OutcomeLayout.Location = New System.Drawing.Point(0, 0)
        Me._OutcomeLayout.Name = "_OutcomeLayout"
        Me._OutcomeLayout.RowCount = 3
        Me._OutcomeLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._OutcomeLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._OutcomeLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._OutcomeLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._OutcomeLayout.Size = New System.Drawing.Size(968, 65)
        Me._OutcomeLayout.TabIndex = 1
        '
        '_AsymptoteTextBox
        '
        Me._AsymptoteTextBox.BackColor = System.Drawing.Color.Black
        Me._AsymptoteTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._AsymptoteTextBox.CausesValidation = False
        Me._AsymptoteTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._AsymptoteTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._AsymptoteTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._AsymptoteTextBox.Location = New System.Drawing.Point(137, 23)
        Me._AsymptoteTextBox.Name = "_AsymptoteTextBox"
        Me._AsymptoteTextBox.ReadOnly = True
        Me._AsymptoteTextBox.Size = New System.Drawing.Size(128, 32)
        Me._AsymptoteTextBox.TabIndex = 3
        Me._AsymptoteTextBox.Text = "0.0"
        Me._AsymptoteTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._AsymptoteTextBox, "Asymptotic voltage")
        '
        '_ThermalTransientVoltageTextBoxLabel
        '
        Me._ThermalTransientVoltageTextBoxLabel.AutoSize = True
        Me._ThermalTransientVoltageTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._ThermalTransientVoltageTextBoxLabel.CausesValidation = False
        Me._ThermalTransientVoltageTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ThermalTransientVoltageTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._ThermalTransientVoltageTextBoxLabel.Location = New System.Drawing.Point(137, 3)
        Me._ThermalTransientVoltageTextBoxLabel.Name = "_ThermalTransientVoltageTextBoxLabel"
        Me._ThermalTransientVoltageTextBoxLabel.Size = New System.Drawing.Size(128, 17)
        Me._ThermalTransientVoltageTextBoxLabel.TabIndex = 2
        Me._ThermalTransientVoltageTextBoxLabel.Text = "V ∞ [mV]"
        Me._ThermalTransientVoltageTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_EstimatedVoltageTextBox
        '
        Me._EstimatedVoltageTextBox.BackColor = System.Drawing.Color.Black
        Me._EstimatedVoltageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._EstimatedVoltageTextBox.CausesValidation = False
        Me._EstimatedVoltageTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._EstimatedVoltageTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._EstimatedVoltageTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._EstimatedVoltageTextBox.Location = New System.Drawing.Point(271, 23)
        Me._EstimatedVoltageTextBox.Name = "_EstimatedVoltageTextBox"
        Me._EstimatedVoltageTextBox.ReadOnly = True
        Me._EstimatedVoltageTextBox.Size = New System.Drawing.Size(128, 32)
        Me._EstimatedVoltageTextBox.TabIndex = 5
        Me._EstimatedVoltageTextBox.Text = "0.0"
        Me._EstimatedVoltageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._EstimatedVoltageTextBox, "Voltage at the end of the pulse")
        '
        '_EstimatedVoltageTextBoxLabel
        '
        Me._EstimatedVoltageTextBoxLabel.AutoSize = True
        Me._EstimatedVoltageTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._EstimatedVoltageTextBoxLabel.CausesValidation = False
        Me._EstimatedVoltageTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._EstimatedVoltageTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._EstimatedVoltageTextBoxLabel.Location = New System.Drawing.Point(271, 3)
        Me._EstimatedVoltageTextBoxLabel.Name = "_EstimatedVoltageTextBoxLabel"
        Me._EstimatedVoltageTextBoxLabel.Size = New System.Drawing.Size(128, 17)
        Me._EstimatedVoltageTextBoxLabel.TabIndex = 4
        Me._EstimatedVoltageTextBoxLabel.Text = "V [mV]"
        Me._EstimatedVoltageTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_TimeConstantTextBoxLabel
        '
        Me._TimeConstantTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._TimeConstantTextBoxLabel.CausesValidation = False
        Me._TimeConstantTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._TimeConstantTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._TimeConstantTextBoxLabel.Location = New System.Drawing.Point(3, 3)
        Me._TimeConstantTextBoxLabel.Name = "_TimeConstantTextBoxLabel"
        Me._TimeConstantTextBoxLabel.Size = New System.Drawing.Size(128, 17)
        Me._TimeConstantTextBoxLabel.TabIndex = 0
        Me._TimeConstantTextBoxLabel.Text = "TC [ms]"
        Me._TimeConstantTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_TimeConstantTextBox
        '
        Me._TimeConstantTextBox.BackColor = System.Drawing.Color.Black
        Me._TimeConstantTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._TimeConstantTextBox.CausesValidation = False
        Me._TimeConstantTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._TimeConstantTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TimeConstantTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._TimeConstantTextBox.Location = New System.Drawing.Point(3, 23)
        Me._TimeConstantTextBox.Name = "_TimeConstantTextBox"
        Me._TimeConstantTextBox.ReadOnly = True
        Me._TimeConstantTextBox.Size = New System.Drawing.Size(128, 32)
        Me._TimeConstantTextBox.TabIndex = 1
        Me._TimeConstantTextBox.Text = "0.00"
        Me._TimeConstantTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._TimeConstantTextBox, "Time constant")
        '
        '_CorrelationCoefficientTextBoxLabel
        '
        Me._CorrelationCoefficientTextBoxLabel.AutoSize = True
        Me._CorrelationCoefficientTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._CorrelationCoefficientTextBoxLabel.CausesValidation = False
        Me._CorrelationCoefficientTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._CorrelationCoefficientTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._CorrelationCoefficientTextBoxLabel.Location = New System.Drawing.Point(405, 3)
        Me._CorrelationCoefficientTextBoxLabel.Name = "_CorrelationCoefficientTextBoxLabel"
        Me._CorrelationCoefficientTextBoxLabel.Size = New System.Drawing.Size(128, 17)
        Me._CorrelationCoefficientTextBoxLabel.TabIndex = 4
        Me._CorrelationCoefficientTextBoxLabel.Text = "R"
        Me._CorrelationCoefficientTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_CorrelationCoefficientTextBox
        '
        Me._CorrelationCoefficientTextBox.BackColor = System.Drawing.Color.Black
        Me._CorrelationCoefficientTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._CorrelationCoefficientTextBox.CausesValidation = False
        Me._CorrelationCoefficientTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._CorrelationCoefficientTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._CorrelationCoefficientTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._CorrelationCoefficientTextBox.Location = New System.Drawing.Point(405, 23)
        Me._CorrelationCoefficientTextBox.Name = "_CorrelationCoefficientTextBox"
        Me._CorrelationCoefficientTextBox.ReadOnly = True
        Me._CorrelationCoefficientTextBox.Size = New System.Drawing.Size(128, 32)
        Me._CorrelationCoefficientTextBox.TabIndex = 8
        Me._CorrelationCoefficientTextBox.Text = "0.0000"
        Me._CorrelationCoefficientTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._CorrelationCoefficientTextBox, "Correlation coefficient.")
        '
        '_StandardErrorTextBox
        '
        Me._StandardErrorTextBox.BackColor = System.Drawing.Color.Black
        Me._StandardErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._StandardErrorTextBox.CausesValidation = False
        Me._StandardErrorTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._StandardErrorTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._StandardErrorTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._StandardErrorTextBox.Location = New System.Drawing.Point(539, 23)
        Me._StandardErrorTextBox.Name = "_StandardErrorTextBox"
        Me._StandardErrorTextBox.ReadOnly = True
        Me._StandardErrorTextBox.Size = New System.Drawing.Size(128, 32)
        Me._StandardErrorTextBox.TabIndex = 8
        Me._StandardErrorTextBox.Text = "0.00"
        Me._StandardErrorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._StandardErrorTextBox, "Standard Error")
        '
        '_IterationsCountTextBox
        '
        Me._IterationsCountTextBox.BackColor = System.Drawing.Color.Black
        Me._IterationsCountTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._IterationsCountTextBox.CausesValidation = False
        Me._IterationsCountTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._IterationsCountTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._IterationsCountTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._IterationsCountTextBox.Location = New System.Drawing.Point(673, 23)
        Me._IterationsCountTextBox.Name = "_IterationsCountTextBox"
        Me._IterationsCountTextBox.ReadOnly = True
        Me._IterationsCountTextBox.Size = New System.Drawing.Size(128, 32)
        Me._IterationsCountTextBox.TabIndex = 8
        Me._IterationsCountTextBox.Text = "0"
        Me._IterationsCountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me._ToolTip.SetToolTip(Me._IterationsCountTextBox, "Number of iterations required to estimate the best model")
        '
        '_OutcomeTextBox
        '
        Me._OutcomeTextBox.BackColor = System.Drawing.Color.Black
        Me._OutcomeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._OutcomeTextBox.CausesValidation = False
        Me._OutcomeTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._OutcomeTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._OutcomeTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._OutcomeTextBox.Location = New System.Drawing.Point(807, 23)
        Me._OutcomeTextBox.Name = "_OutcomeTextBox"
        Me._OutcomeTextBox.ReadOnly = True
        Me._OutcomeTextBox.Size = New System.Drawing.Size(158, 32)
        Me._OutcomeTextBox.TabIndex = 8
        Me._OutcomeTextBox.Text = "OPTIMIZED"
        Me._OutcomeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_StandardErrorTextBoxLabel
        '
        Me._StandardErrorTextBoxLabel.AutoSize = True
        Me._StandardErrorTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._StandardErrorTextBoxLabel.CausesValidation = False
        Me._StandardErrorTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._StandardErrorTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._StandardErrorTextBoxLabel.Location = New System.Drawing.Point(539, 3)
        Me._StandardErrorTextBoxLabel.Name = "_StandardErrorTextBoxLabel"
        Me._StandardErrorTextBoxLabel.Size = New System.Drawing.Size(128, 17)
        Me._StandardErrorTextBoxLabel.TabIndex = 4
        Me._StandardErrorTextBoxLabel.Text = "σ [mV]"
        Me._StandardErrorTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me._ToolTip.SetToolTip(Me._StandardErrorTextBoxLabel, "Standard Error")
        '
        '_IterationsCountTextBoxLabel
        '
        Me._IterationsCountTextBoxLabel.AutoSize = True
        Me._IterationsCountTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._IterationsCountTextBoxLabel.CausesValidation = False
        Me._IterationsCountTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._IterationsCountTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._IterationsCountTextBoxLabel.Location = New System.Drawing.Point(673, 3)
        Me._IterationsCountTextBoxLabel.Name = "_IterationsCountTextBoxLabel"
        Me._IterationsCountTextBoxLabel.Size = New System.Drawing.Size(128, 17)
        Me._IterationsCountTextBoxLabel.TabIndex = 4
        Me._IterationsCountTextBoxLabel.Text = "N"
        Me._IterationsCountTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_OutcomeTextBoxLabel
        '
        Me._OutcomeTextBoxLabel.AutoSize = True
        Me._OutcomeTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._OutcomeTextBoxLabel.CausesValidation = False
        Me._OutcomeTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._OutcomeTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._OutcomeTextBoxLabel.Location = New System.Drawing.Point(807, 3)
        Me._OutcomeTextBoxLabel.Name = "_OutcomeTextBoxLabel"
        Me._OutcomeTextBoxLabel.Size = New System.Drawing.Size(158, 17)
        Me._OutcomeTextBoxLabel.TabIndex = 4
        Me._OutcomeTextBoxLabel.Text = "Outcome"
        Me._OutcomeTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        'ThermalTransientHeader
        '
        Me.BackColor = System.Drawing.Color.Black
        Me.Controls.Add(Me._OutcomeLayout)
        Me.Name = "ThermalTransientHeader"
        Me.Size = New System.Drawing.Size(968, 59)
        Me._OutcomeLayout.ResumeLayout(False)
        Me._OutcomeLayout.PerformLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _ErrorProvider As System.Windows.Forms.ErrorProvider
    Private WithEvents _OutcomeLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _AsymptoteTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ThermalTransientVoltageTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _EstimatedVoltageTextBox As System.Windows.Forms.TextBox
    Private WithEvents _TimeConstantTextBox As System.Windows.Forms.TextBox
    Private WithEvents _EstimatedVoltageTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _TimeConstantTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _CorrelationCoefficientTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _CorrelationCoefficientTextBox As System.Windows.Forms.TextBox
    Private WithEvents _StandardErrorTextBox As System.Windows.Forms.TextBox
    Private WithEvents _IterationsCountTextBox As System.Windows.Forms.TextBox
    Private WithEvents _OutcomeTextBox As System.Windows.Forms.TextBox
    Private WithEvents _StandardErrorTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _IterationsCountTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _OutcomeTextBoxLabel As System.Windows.Forms.Label

End Class
