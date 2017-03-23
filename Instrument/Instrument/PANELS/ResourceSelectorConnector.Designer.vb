Imports System.Drawing
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ResourceSelectorConnector

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._ToolStrip = New System.Windows.Forms.ToolStrip()
        Me._ClearButton = New System.Windows.Forms.ToolStripButton()
        Me._ResourceNamesComboBox = New isr.Core.Pith.ToolStripSpringComboBox
        Me._ToggleConnectionButton = New System.Windows.Forms.ToolStripButton()
        Me._FindButton = New System.Windows.Forms.ToolStripButton()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._ToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        '_ToolStrip
        '
        Me._ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me._ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ClearButton, Me._ResourceNamesComboBox, Me._ToggleConnectionButton, Me._FindButton})
        Me._ToolStrip.Location = New System.Drawing.Point(0, 0)
        Me._ToolStrip.Name = "_ToolStrip"
        Me._ToolStrip.Size = New System.Drawing.Size(474, 29)
        Me._ToolStrip.TabIndex = 11
        Me._ToolStrip.Text = "Resource Selector"
        '
        '_ClearButton
        '
        Me._ClearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._ClearButton.Image = Global.isr.VI.Instrument.My.Resources.Resources.Clear_22x22
        Me._ClearButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._ClearButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ClearButton.Name = "_ClearButton"
        Me._ClearButton.Size = New System.Drawing.Size(26, 26)
        Me._ClearButton.Text = "ToolStripButton1"
        '
        '_ResourceNamesComboBox
        '
        Me._ResourceNamesComboBox.AutoSize = True
        Me._ResourceNamesComboBox.Name = "_ResourceNamesComboBox"
        Me._ResourceNamesComboBox.Size = New System.Drawing.Size(121, 23)
        Me._ResourceNamesComboBox.ToolTipText = "Available resource names"
        '
        '_ToggleConnectionButton
        '
        Me._ToggleConnectionButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._ToggleConnectionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._ToggleConnectionButton.Image = Global.isr.VI.Instrument.My.Resources.Resources.Connect_22x22
        Me._ToggleConnectionButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._ToggleConnectionButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._ToggleConnectionButton.Name = "_ToggleConnectionButton"
        Me._ToggleConnectionButton.Size = New System.Drawing.Size(26, 26)
        Me._ToggleConnectionButton.ToolTipText = "Click to connect"
        '
        '_FindButton
        '
        Me._FindButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me._FindButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._FindButton.Image = Global.isr.VI.Instrument.My.Resources.Resources.Find_22x22
        Me._FindButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._FindButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._FindButton.Name = "_FindButton"
        Me._FindButton.Size = New System.Drawing.Size(26, 26)
        Me._FindButton.ToolTipText = "Find resources"
        '
        'ResourceSelectorConnector1
        '
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me._ToolStrip)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "ResourceSelectorConnector1"
        Me.Size = New System.Drawing.Size(474, 29)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._ToolStrip.ResumeLayout(False)
        Me._ToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _ErrorProvider As System.Windows.Forms.ErrorProvider
    Private WithEvents _ClearButton As Windows.Forms.ToolStripButton
    Private WithEvents _ResourceNamesComboBox As isr.Core.Pith.ToolStripSpringComboBox
    Private WithEvents _FindButton As Windows.Forms.ToolStripButton
    Private WithEvents _ToggleConnectionButton As Windows.Forms.ToolStripButton
    Private WithEvents _ToolStrip As Windows.Forms.ToolStrip
End Class
