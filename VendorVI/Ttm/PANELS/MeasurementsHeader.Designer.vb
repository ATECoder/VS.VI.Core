<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MeasurementsHeader

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
        Me._ThermalTransientVoltageTextBox = New System.Windows.Forms.TextBox()
        Me._ThermalTransientVoltageTextBoxLabel = New System.Windows.Forms.Label()
        Me._FinalResistanceTextBox = New System.Windows.Forms.TextBox()
        Me._InitialResistanceTextBox = New System.Windows.Forms.TextBox()
        Me._FinalResistanceTextBoxLabel = New System.Windows.Forms.Label()
        Me._InitialResistanceTextBoxLabel = New System.Windows.Forms.Label()
        Me._OutcomePictureBox = New System.Windows.Forms.PictureBox()
        Me._AlertsPictureBox = New System.Windows.Forms.PictureBox()
        Me._OutcomeTextBox = New System.Windows.Forms.TextBox()
        Me._OutcomeTextBoxLabel = New System.Windows.Forms.Label()
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._OutcomeLayout.SuspendLayout()
        CType(Me._OutcomePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._AlertsPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_OutcomeLayout
        '
        Me._OutcomeLayout.BackColor = System.Drawing.Color.Transparent
        Me._OutcomeLayout.ColumnCount = 6
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._OutcomeLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._OutcomeLayout.Controls.Add(Me._ThermalTransientVoltageTextBox, 2, 2)
        Me._OutcomeLayout.Controls.Add(Me._ThermalTransientVoltageTextBoxLabel, 2, 1)
        Me._OutcomeLayout.Controls.Add(Me._FinalResistanceTextBox, 3, 2)
        Me._OutcomeLayout.Controls.Add(Me._InitialResistanceTextBox, 1, 2)
        Me._OutcomeLayout.Controls.Add(Me._FinalResistanceTextBoxLabel, 3, 1)
        Me._OutcomeLayout.Controls.Add(Me._InitialResistanceTextBoxLabel, 1, 1)
        Me._OutcomeLayout.Controls.Add(Me._OutcomePictureBox, 5, 2)
        Me._OutcomeLayout.Controls.Add(Me._AlertsPictureBox, 0, 2)
        Me._OutcomeLayout.Controls.Add(Me._OutcomeTextBox, 4, 2)
        Me._OutcomeLayout.Controls.Add(Me._OutcomeTextBoxLabel, 4, 1)
        Me._OutcomeLayout.Dock = System.Windows.Forms.DockStyle.Top
        Me._OutcomeLayout.Location = New System.Drawing.Point(0, 0)
        Me._OutcomeLayout.Name = "_OutcomeLayout"
        Me._OutcomeLayout.RowCount = 3
        Me._OutcomeLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._OutcomeLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._OutcomeLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._OutcomeLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._OutcomeLayout.Size = New System.Drawing.Size(968, 59)
        Me._OutcomeLayout.TabIndex = 1
        '
        '_ThermalTransientVoltageTextBox
        '
        Me._ThermalTransientVoltageTextBox.BackColor = System.Drawing.Color.Black
        Me._ThermalTransientVoltageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._ThermalTransientVoltageTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ThermalTransientVoltageTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ThermalTransientVoltageTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._ThermalTransientVoltageTextBox.Location = New System.Drawing.Point(261, 24)
        Me._ThermalTransientVoltageTextBox.Name = "_ThermalTransientVoltageTextBox"
        Me._ThermalTransientVoltageTextBox.ReadOnly = True
        Me._ThermalTransientVoltageTextBox.Size = New System.Drawing.Size(219, 32)
        Me._ThermalTransientVoltageTextBox.TabIndex = 3
        Me._ThermalTransientVoltageTextBox.Text = "0.000"
        Me._ThermalTransientVoltageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_ThermalTransientVoltageTextBoxLabel
        '
        Me._ThermalTransientVoltageTextBoxLabel.AutoSize = True
        Me._ThermalTransientVoltageTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._ThermalTransientVoltageTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._ThermalTransientVoltageTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._ThermalTransientVoltageTextBoxLabel.Location = New System.Drawing.Point(261, 4)
        Me._ThermalTransientVoltageTextBoxLabel.Name = "_ThermalTransientVoltageTextBoxLabel"
        Me._ThermalTransientVoltageTextBoxLabel.Size = New System.Drawing.Size(219, 17)
        Me._ThermalTransientVoltageTextBoxLabel.TabIndex = 2
        Me._ThermalTransientVoltageTextBoxLabel.Text = "THERMAL TRANSIENT [V]"
        Me._ThermalTransientVoltageTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_FinalResistanceTextBox
        '
        Me._FinalResistanceTextBox.BackColor = System.Drawing.Color.Black
        Me._FinalResistanceTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._FinalResistanceTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._FinalResistanceTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._FinalResistanceTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._FinalResistanceTextBox.Location = New System.Drawing.Point(486, 24)
        Me._FinalResistanceTextBox.Name = "_FinalResistanceTextBox"
        Me._FinalResistanceTextBox.ReadOnly = True
        Me._FinalResistanceTextBox.Size = New System.Drawing.Size(219, 32)
        Me._FinalResistanceTextBox.TabIndex = 5
        Me._FinalResistanceTextBox.Text = "0.000"
        Me._FinalResistanceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_InitialResistanceTextBox
        '
        Me._InitialResistanceTextBox.BackColor = System.Drawing.Color.Black
        Me._InitialResistanceTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._InitialResistanceTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._InitialResistanceTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._InitialResistanceTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._InitialResistanceTextBox.Location = New System.Drawing.Point(36, 24)
        Me._InitialResistanceTextBox.Name = "_InitialResistanceTextBox"
        Me._InitialResistanceTextBox.ReadOnly = True
        Me._InitialResistanceTextBox.Size = New System.Drawing.Size(219, 32)
        Me._InitialResistanceTextBox.TabIndex = 1
        Me._InitialResistanceTextBox.Text = "0.000"
        Me._InitialResistanceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_FinalResistanceTextBoxLabel
        '
        Me._FinalResistanceTextBoxLabel.AutoSize = True
        Me._FinalResistanceTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._FinalResistanceTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._FinalResistanceTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._FinalResistanceTextBoxLabel.Location = New System.Drawing.Point(486, 4)
        Me._FinalResistanceTextBoxLabel.Name = "_FinalResistanceTextBoxLabel"
        Me._FinalResistanceTextBoxLabel.Size = New System.Drawing.Size(219, 17)
        Me._FinalResistanceTextBoxLabel.TabIndex = 4
        Me._FinalResistanceTextBoxLabel.Text = "FINAL RESISTANCE [Ω]"
        Me._FinalResistanceTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_InitialResistanceTextBoxLabel
        '
        Me._InitialResistanceTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._InitialResistanceTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._InitialResistanceTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._InitialResistanceTextBoxLabel.Location = New System.Drawing.Point(36, 4)
        Me._InitialResistanceTextBoxLabel.Name = "_InitialResistanceTextBoxLabel"
        Me._InitialResistanceTextBoxLabel.Size = New System.Drawing.Size(219, 17)
        Me._InitialResistanceTextBoxLabel.TabIndex = 0
        Me._InitialResistanceTextBoxLabel.Text = "INITIAL RESISTANCE [Ω]"
        Me._InitialResistanceTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_OutcomePictureBox
        '
        Me._OutcomePictureBox.BackColor = System.Drawing.Color.Black
        Me._OutcomePictureBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._OutcomePictureBox.Image = Global.isr.Vi.Ttm.My.Resources.Resources.Good
        Me._OutcomePictureBox.Location = New System.Drawing.Point(936, 24)
        Me._OutcomePictureBox.Name = "_OutcomePictureBox"
        Me._OutcomePictureBox.Size = New System.Drawing.Size(29, 28)
        Me._OutcomePictureBox.TabIndex = 6
        Me._OutcomePictureBox.TabStop = False
        '
        '_AlertsPictureBox
        '
        Me._AlertsPictureBox.BackColor = System.Drawing.Color.Black
        Me._AlertsPictureBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._AlertsPictureBox.Image = Global.isr.Vi.Ttm.My.Resources.Resources.Bad
        Me._AlertsPictureBox.Location = New System.Drawing.Point(3, 24)
        Me._AlertsPictureBox.Name = "_AlertsPictureBox"
        Me._AlertsPictureBox.Size = New System.Drawing.Size(27, 28)
        Me._AlertsPictureBox.TabIndex = 7
        Me._AlertsPictureBox.TabStop = False
        '
        '_OutcomeTextBox
        '
        Me._OutcomeTextBox.BackColor = System.Drawing.Color.Black
        Me._OutcomeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._OutcomeTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._OutcomeTextBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._OutcomeTextBox.ForeColor = System.Drawing.Color.Aquamarine
        Me._OutcomeTextBox.Location = New System.Drawing.Point(711, 24)
        Me._OutcomeTextBox.Name = "_OutcomeTextBox"
        Me._OutcomeTextBox.ReadOnly = True
        Me._OutcomeTextBox.Size = New System.Drawing.Size(219, 32)
        Me._OutcomeTextBox.TabIndex = 5
        Me._OutcomeTextBox.Text = "COMPLIANCE"
        Me._OutcomeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_OutcomeTextBoxLabel
        '
        Me._OutcomeTextBoxLabel.AutoSize = True
        Me._OutcomeTextBoxLabel.BackColor = System.Drawing.Color.Black
        Me._OutcomeTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._OutcomeTextBoxLabel.ForeColor = System.Drawing.Color.Yellow
        Me._OutcomeTextBoxLabel.Location = New System.Drawing.Point(711, 4)
        Me._OutcomeTextBoxLabel.Name = "_OutcomeTextBoxLabel"
        Me._OutcomeTextBoxLabel.Size = New System.Drawing.Size(219, 17)
        Me._OutcomeTextBoxLabel.TabIndex = 4
        Me._OutcomeTextBoxLabel.Text = "OUTCOME"
        Me._OutcomeTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        'MeasurementsHeader
        '
        Me.BackColor = System.Drawing.Color.Black
        Me.Controls.Add(Me._OutcomeLayout)
        Me.Name = "MeasurementsHeader"
        Me.Size = New System.Drawing.Size(968, 61)
        Me._OutcomeLayout.ResumeLayout(False)
        Me._OutcomeLayout.PerformLayout()
        CType(Me._OutcomePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._AlertsPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _ErrorProvider As System.Windows.Forms.ErrorProvider
    Private WithEvents _OutcomeLayout As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _ThermalTransientVoltageTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ThermalTransientVoltageTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _FinalResistanceTextBox As System.Windows.Forms.TextBox
    Private WithEvents _InitialResistanceTextBox As System.Windows.Forms.TextBox
    Private WithEvents _FinalResistanceTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _InitialResistanceTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _OutcomePictureBox As System.Windows.Forms.PictureBox
    Private WithEvents _AlertsPictureBox As System.Windows.Forms.PictureBox
    Private WithEvents _OutcomeTextBox As System.Windows.Forms.TextBox
    Private WithEvents _OutcomeTextBoxLabel As System.Windows.Forms.Label

End Class
