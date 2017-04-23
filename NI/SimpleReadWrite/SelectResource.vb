Imports NationalInstruments.Visa

''' <summary>
''' Summary description for SelectResource.
''' </summary>
Public Class SelectResource
		Inherits System.Windows.Forms.Form

		Private WithEvents AvailableResourcesListBox As System.Windows.Forms.ListBox
		Private okButton As System.Windows.Forms.Button
		Private closeButton As System.Windows.Forms.Button
		Private visaResourceNameTextBox As System.Windows.Forms.TextBox
		Private AvailableResourcesLabel As System.Windows.Forms.Label
		Private ResourceStringLabel As System.Windows.Forms.Label
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.Container = Nothing

        Public Sub New()
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()

        End Sub

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				If components IsNot Nothing Then
					components.Dispose()
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"
		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Dim resources As New System.Resources.ResourceManager(GetType(SelectResource))
			Me.availableResourcesListBox = New System.Windows.Forms.ListBox()
			Me.okButton = New System.Windows.Forms.Button()
			Me.closeButton = New System.Windows.Forms.Button()
			Me.visaResourceNameTextBox = New System.Windows.Forms.TextBox()
			Me.AvailableResourcesLabel = New System.Windows.Forms.Label()
			Me.ResourceStringLabel = New System.Windows.Forms.Label()
			Me.SuspendLayout()
			' 
			' availableResourcesListBox
			' 
			Me.availableResourcesListBox.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.availableResourcesListBox.Location = New System.Drawing.Point(5, 18)
			Me.availableResourcesListBox.Name = "availableResourcesListBox"
			Me.availableResourcesListBox.Size = New System.Drawing.Size(282, 108)
			Me.availableResourcesListBox.TabIndex = 0
'			Me.availableResourcesListBox.DoubleClick += New System.EventHandler(Me.availableResourcesListBox_DoubleClick)
'			Me.availableResourcesListBox.SelectedIndexChanged += New System.EventHandler(Me.availableResourcesListBox_SelectedIndexChanged)
			' 
			' okButton
			' 
			Me.okButton.Anchor = (CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles))
			Me.okButton.DialogResult = System.Windows.Forms.DialogResult.OK
			Me.okButton.Location = New System.Drawing.Point(5, 187)
			Me.okButton.Name = "okButton"
			Me.okButton.Size = New System.Drawing.Size(77, 25)
			Me.okButton.TabIndex = 2
			Me.okButton.Text = "OK"
			' 
			' closeButton
			' 
			Me.closeButton.Anchor = (CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles))
			Me.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
			Me.closeButton.Location = New System.Drawing.Point(82, 187)
			Me.closeButton.Name = "closeButton"
			Me.closeButton.Size = New System.Drawing.Size(77, 25)
			Me.closeButton.TabIndex = 3
			Me.closeButton.Text = "Cancel"
			' 
			' visaResourceNameTextBox
			' 
			Me.visaResourceNameTextBox.Anchor = (CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.visaResourceNameTextBox.Location = New System.Drawing.Point(5, 157)
			Me.visaResourceNameTextBox.Name = "visaResourceNameTextBox"
			Me.visaResourceNameTextBox.Size = New System.Drawing.Size(282, 20)
			Me.visaResourceNameTextBox.TabIndex = 4
			Me.visaResourceNameTextBox.Text = "GPIB0::2::INSTR"
			' 
			' AvailableResourcesLabel
			' 
			Me.AvailableResourcesLabel.Anchor = (CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.AvailableResourcesLabel.Location = New System.Drawing.Point(5, 5)
			Me.AvailableResourcesLabel.Name = "AvailableResourcesLabel"
			Me.AvailableResourcesLabel.Size = New System.Drawing.Size(279, 12)
			Me.AvailableResourcesLabel.TabIndex = 5
			Me.AvailableResourcesLabel.Text = "Available Resources:"
			' 
			' ResourceStringLabel
			' 
			Me.ResourceStringLabel.Anchor = (CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.ResourceStringLabel.Location = New System.Drawing.Point(5, 141)
			Me.ResourceStringLabel.Name = "ResourceStringLabel"
			Me.ResourceStringLabel.Size = New System.Drawing.Size(279, 13)
			Me.ResourceStringLabel.TabIndex = 6
			Me.ResourceStringLabel.Text = "Resource String:"
			' 
			' SelectResource
			' 
			Me.AcceptButton = Me.okButton
			Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
			Me.CancelButton = Me.closeButton
			Me.ClientSize = New System.Drawing.Size(292, 220)
			Me.Controls.Add(Me.ResourceStringLabel)
			Me.Controls.Add(Me.AvailableResourcesLabel)
			Me.Controls.Add(Me.visaResourceNameTextBox)
			Me.Controls.Add(Me.closeButton)
			Me.Controls.Add(Me.okButton)
			Me.Controls.Add(Me.availableResourcesListBox)
			Me.Icon = (CType(resources.GetObject("$this.Icon"), System.Drawing.Icon))
			Me.KeyPreview = True
			Me.MaximizeBox = False
			Me.MinimumSize = New System.Drawing.Size(177, 247)
			Me.Name = "SelectResource"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "Select Resource"
'			Me.Load += New System.EventHandler(Me.OnLoad)
			Me.ResumeLayout(False)

		End Sub
		#End Region


		Private Overloads Sub OnLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
			' This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
			' Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
			' requires additional VISA .NET implementations.
			Using rmSession = New ResourceManager()
				Dim resources = rmSession.Find("(ASRL|GPIB|TCPIP|USB)?*")
				For Each s As String In resources
					availableResourcesListBox.Items.Add(s)
				Next s
			End Using
		End Sub

		Private Sub AvailableResourcesListBox_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles availableResourcesListBox.DoubleClick
			Dim selectedString As String = CStr(availableResourcesListBox.SelectedItem)
			ResourceName = selectedString
			Me.DialogResult = System.Windows.Forms.DialogResult.OK
			Me.Close()
		End Sub

		Private Sub AvailableResourcesListBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles availableResourcesListBox.SelectedIndexChanged
			Dim selectedString As String = CStr(availableResourcesListBox.SelectedItem)
			ResourceName = selectedString
		End Sub

		Public Property ResourceName() As String
			Get
				Return visaResourceNameTextBox.Text
			End Get
			Set(ByVal value As String)
				visaResourceNameTextBox.Text = value
			End Set
		End Property
	End Class
