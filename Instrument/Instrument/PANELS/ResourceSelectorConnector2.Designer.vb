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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ResourceSelectorConnector))
        Me._ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._ResourceNamesComboBox = New System.Windows.Forms.ComboBox()
        Me._FindButton = New System.Windows.Forms.Button()
        Me._ImageList = New System.Windows.Forms.ImageList(Me.components)
        Me._ClearButton = New System.Windows.Forms.Button()
        Me._MainTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._ToggleConnectionButton = New System.Windows.Forms.Button()
        Me._MainTableLayoutPanel.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_ResourceNamesComboBox
        '
        Me._ResourceNamesComboBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ResourceNamesComboBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ResourceNamesComboBox.Location = New System.Drawing.Point(41, 9)
        Me._ResourceNamesComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._ResourceNamesComboBox.Name = "_ResourceNamesComboBox"
        Me._ResourceNamesComboBox.Size = New System.Drawing.Size(231, 25)
        Me._ResourceNamesComboBox.TabIndex = 5
        Me._ToolTip.SetToolTip(Me._ResourceNamesComboBox, "Select an item from the list")
        '
        '_FindButton
        '
        Me._FindButton.Dock = System.Windows.Forms.DockStyle.Left
        Me._FindButton.ImageIndex = 3
        Me._FindButton.ImageList = Me._ImageList
        Me._FindButton.Location = New System.Drawing.Point(278, 6)
        Me._FindButton.Margin = New System.Windows.Forms.Padding(3, 1, 3, 1)
        Me._FindButton.Name = "_FindButton"
        Me._FindButton.Size = New System.Drawing.Size(31, 31)
        Me._FindButton.TabIndex = 6
        Me._ToolTip.SetToolTip(Me._FindButton, "Search for items")
        Me._FindButton.UseVisualStyleBackColor = True
        '
        '_imageList
        '
        Me._ImageList.ImageStream = CType(resources.GetObject("_imageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me._ImageList.TransparentColor = System.Drawing.SystemColors.Control
        Me._ImageList.Images.SetKeyName(0, "")
        Me._ImageList.Images.SetKeyName(1, "")
        Me._ImageList.Images.SetKeyName(2, "")
        Me._ImageList.Images.SetKeyName(3, "")
        '
        '_ClearButton
        '
        Me._ClearButton.Dock = System.Windows.Forms.DockStyle.Left
        Me._ClearButton.Enabled = False
        Me._ClearButton.ImageIndex = 0
        Me._ClearButton.ImageList = Me._ImageList
        Me._ClearButton.Location = New System.Drawing.Point(3, 6)
        Me._ClearButton.Margin = New System.Windows.Forms.Padding(3, 1, 3, 1)
        Me._ClearButton.Name = "_ClearButton"
        Me._ClearButton.Size = New System.Drawing.Size(32, 31)
        Me._ClearButton.TabIndex = 8
        Me._ToolTip.SetToolTip(Me._ClearButton, "Clear Selected Item")
        Me._ClearButton.UseVisualStyleBackColor = True
        '
        '_mainTableLayoutPanel
        '
        Me._MainTableLayoutPanel.ColumnCount = 4
        Me._MainTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._MainTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._MainTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._MainTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._MainTableLayoutPanel.Controls.Add(Me._ResourceNamesComboBox, 1, 1)
        Me._MainTableLayoutPanel.Controls.Add(Me._ClearButton, 0, 1)
        Me._MainTableLayoutPanel.Controls.Add(Me._FindButton, 2, 1)
        Me._MainTableLayoutPanel.Controls.Add(Me._ToggleConnectionButton, 3, 1)
        Me._MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._MainTableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me._MainTableLayoutPanel.Margin = New System.Windows.Forms.Padding(0)
        Me._MainTableLayoutPanel.Name = "_mainTableLayoutPanel"
        Me._MainTableLayoutPanel.RowCount = 3
        Me._MainTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._MainTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._MainTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._MainTableLayoutPanel.Size = New System.Drawing.Size(350, 43)
        Me._MainTableLayoutPanel.TabIndex = 10
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        ' _ToggleConnectionButton
        '
        Me._ToggleConnectionButton.Dock = System.Windows.Forms.DockStyle.Left
        Me._ToggleConnectionButton.ImageIndex = 1
        Me._ToggleConnectionButton.ImageList = Me._ImageList
        Me._ToggleConnectionButton.Location = New System.Drawing.Point(315, 6)
        Me._ToggleConnectionButton.Margin = New System.Windows.Forms.Padding(3, 1, 3, 1)
        Me._ToggleConnectionButton.Name = "_ToggleConnectionButton"
        Me._ToggleConnectionButton.Size = New System.Drawing.Size(32, 31)
        Me._ToggleConnectionButton.TabIndex = 9
        Me._ToolTip.SetToolTip(Me._ToggleConnectionButton, "Click to Connect")
        Me._ToggleConnectionButton.UseVisualStyleBackColor = True
        '
        'ResourceSelectorConnector
        '
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me._MainTableLayoutPanel)
        Me.Name = "ResourceSelectorConnector"
        Me.Size = New System.Drawing.Size(350, 43)
        Me._MainTableLayoutPanel.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _ImageList As System.Windows.Forms.ImageList
    Private WithEvents _ResourceNamesComboBox As System.Windows.Forms.ComboBox
    Private WithEvents _FindButton As System.Windows.Forms.Button
    Private WithEvents _ClearButton As System.Windows.Forms.Button
    Private WithEvents _MainTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _ErrorProvider As System.Windows.Forms.ErrorProvider
    Private WithEvents _ToggleConnectionButton As Windows.Forms.Button
End Class
