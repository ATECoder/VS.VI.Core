Imports NationalInstruments.Visa

''' <summary>
''' Summary description for Form1.
''' </summary>
Public Class MainForm
		Inherits System.Windows.Forms.Form

    Private lastResourceString As String = Nothing
    Private writeTextBox As System.Windows.Forms.TextBox
    Private _ReadTextBox As System.Windows.Forms.TextBox
    Private WithEvents queryButton As System.Windows.Forms.Button
    Private WithEvents writeButton As System.Windows.Forms.Button
    Private WithEvents readButton As System.Windows.Forms.Button
    Private WithEvents _OpenSessionButton As System.Windows.Forms.Button
    Private WithEvents clearButton As System.Windows.Forms.Button
    Private WithEvents _CloseSessionButton As System.Windows.Forms.Button
    Private stringToWriteLabel As System.Windows.Forms.Label
    Private _ReadTextBoxLabel As System.Windows.Forms.Label
    Friend WithEvents _ResourcesComboBox As ComboBox
    Private WithEvents _ResourcesComboBoxLabel As Label
    Private WithEvents _MultipleResourcesCheckBox As CheckBox
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.Container = Nothing

    Public Sub New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()
        SetupControlState(False)
    End Sub

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If _Session IsNot Nothing Then
                _Session.Dispose()
            End If
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.queryButton = New System.Windows.Forms.Button()
        Me.writeButton = New System.Windows.Forms.Button()
        Me.readButton = New System.Windows.Forms.Button()
        Me._OpenSessionButton = New System.Windows.Forms.Button()
        Me.writeTextBox = New System.Windows.Forms.TextBox()
        Me._ReadTextBox = New System.Windows.Forms.TextBox()
        Me.clearButton = New System.Windows.Forms.Button()
        Me._CloseSessionButton = New System.Windows.Forms.Button()
        Me.stringToWriteLabel = New System.Windows.Forms.Label()
        Me._ReadTextBoxLabel = New System.Windows.Forms.Label()
        Me._ResourcesComboBox = New System.Windows.Forms.ComboBox()
        Me._ResourcesComboBoxLabel = New System.Windows.Forms.Label()
        Me._MultipleResourcesCheckBox = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'queryButton
        '
        Me.queryButton.Location = New System.Drawing.Point(71, 129)
        Me.queryButton.Name = "queryButton"
        Me.queryButton.Size = New System.Drawing.Size(67, 23)
        Me.queryButton.TabIndex = 3
        Me.queryButton.Text = "Query"
        '
        'writeButton
        '
        Me.writeButton.Location = New System.Drawing.Point(143, 129)
        Me.writeButton.Name = "writeButton"
        Me.writeButton.Size = New System.Drawing.Size(67, 23)
        Me.writeButton.TabIndex = 4
        Me.writeButton.Text = "Write"
        '
        'readButton
        '
        Me.readButton.Location = New System.Drawing.Point(214, 129)
        Me.readButton.Name = "readButton"
        Me.readButton.Size = New System.Drawing.Size(67, 23)
        Me.readButton.TabIndex = 5
        Me.readButton.Text = "Read"
        '
        '_OpenSessionButton
        '
        Me._OpenSessionButton.Location = New System.Drawing.Point(5, 5)
        Me._OpenSessionButton.Name = "_OpenSessionButton"
        Me._OpenSessionButton.Size = New System.Drawing.Size(92, 22)
        Me._OpenSessionButton.TabIndex = 0
        Me._OpenSessionButton.Text = "Open Session"
        '
        'writeTextBox
        '
        Me.writeTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.writeTextBox.Location = New System.Drawing.Point(5, 101)
        Me.writeTextBox.Name = "writeTextBox"
        Me.writeTextBox.Size = New System.Drawing.Size(277, 20)
        Me.writeTextBox.TabIndex = 2
        Me.writeTextBox.Text = "*IDN?\n"
        '
        '_ReadTextBox
        '
        Me._ReadTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ReadTextBox.Location = New System.Drawing.Point(5, 157)
        Me._ReadTextBox.Multiline = True
        Me._ReadTextBox.Name = "_ReadTextBox"
        Me._ReadTextBox.ReadOnly = True
        Me._ReadTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me._ReadTextBox.Size = New System.Drawing.Size(277, 124)
        Me._ReadTextBox.TabIndex = 6
        Me._ReadTextBox.TabStop = False
        '
        'clearButton
        '
        Me.clearButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clearButton.Location = New System.Drawing.Point(6, 288)
        Me.clearButton.Name = "clearButton"
        Me.clearButton.Size = New System.Drawing.Size(276, 24)
        Me.clearButton.TabIndex = 7
        Me.clearButton.Text = "Clear"
        '
        '_CloseSessionButton
        '
        Me._CloseSessionButton.Location = New System.Drawing.Point(97, 5)
        Me._CloseSessionButton.Name = "_CloseSessionButton"
        Me._CloseSessionButton.Size = New System.Drawing.Size(92, 22)
        Me._CloseSessionButton.TabIndex = 1
        Me._CloseSessionButton.Text = "Close Session"
        '
        'stringToWriteLabel
        '
        Me.stringToWriteLabel.AutoSize = True
        Me.stringToWriteLabel.Location = New System.Drawing.Point(5, 85)
        Me.stringToWriteLabel.Name = "stringToWriteLabel"
        Me.stringToWriteLabel.Size = New System.Drawing.Size(77, 13)
        Me.stringToWriteLabel.TabIndex = 8
        Me.stringToWriteLabel.Text = "String to Write:"
        '
        '_ReadTextBoxLabel
        '
        Me._ReadTextBoxLabel.AutoSize = True
        Me._ReadTextBoxLabel.Location = New System.Drawing.Point(5, 140)
        Me._ReadTextBoxLabel.Name = "_ReadTextBoxLabel"
        Me._ReadTextBoxLabel.Size = New System.Drawing.Size(56, 13)
        Me._ReadTextBoxLabel.TabIndex = 9
        Me._ReadTextBoxLabel.Text = "Received:"
        '
        '_ResourcesComboBox
        '
        Me._ResourcesComboBox.FormattingEnabled = True
        Me._ResourcesComboBox.Location = New System.Drawing.Point(5, 56)
        Me._ResourcesComboBox.Name = "_ResourcesComboBox"
        Me._ResourcesComboBox.Size = New System.Drawing.Size(277, 21)
        Me._ResourcesComboBox.TabIndex = 10
        '
        '_ResourcesComboBoxLabel
        '
        Me._ResourcesComboBoxLabel.AutoSize = True
        Me._ResourcesComboBoxLabel.Location = New System.Drawing.Point(5, 40)
        Me._ResourcesComboBoxLabel.Name = "_ResourcesComboBoxLabel"
        Me._ResourcesComboBoxLabel.Size = New System.Drawing.Size(92, 13)
        Me._ResourcesComboBoxLabel.TabIndex = 11
        Me._ResourcesComboBoxLabel.Text = "Resource Names:"
        '
        '_MultipleResourcesCheckBox
        '
        Me._MultipleResourcesCheckBox.AutoSize = True
        Me._MultipleResourcesCheckBox.Location = New System.Drawing.Point(195, 8)
        Me._MultipleResourcesCheckBox.Name = "_MultipleResourcesCheckBox"
        Me._MultipleResourcesCheckBox.Size = New System.Drawing.Size(90, 17)
        Me._MultipleResourcesCheckBox.TabIndex = 12
        Me._MultipleResourcesCheckBox.Text = "> 1 Resource"
        Me._MultipleResourcesCheckBox.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(287, 317)
        Me.Controls.Add(Me._MultipleResourcesCheckBox)
        Me.Controls.Add(Me._ResourcesComboBoxLabel)
        Me.Controls.Add(Me._ResourcesComboBox)
        Me.Controls.Add(Me._ReadTextBoxLabel)
        Me.Controls.Add(Me.stringToWriteLabel)
        Me.Controls.Add(Me._CloseSessionButton)
        Me.Controls.Add(Me.clearButton)
        Me.Controls.Add(Me._ReadTextBox)
        Me.Controls.Add(Me.writeTextBox)
        Me.Controls.Add(Me._OpenSessionButton)
        Me.Controls.Add(Me.readButton)
        Me.Controls.Add(Me.writeButton)
        Me.Controls.Add(Me.queryButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(295, 316)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Simple Read/Write"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

#If False Then
    ''' <summary>
    ''' The main entry point for the application.
    ''' </summary>
    <STAThread>
    Shared Sub Main()
        Application.Run(New MainForm())
    End Sub
#End If

#Region " OPEN / CLOSE SESSIONS "

    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OpenSession()
        Using sr As New SelectResource()
            If lastResourceString IsNot Nothing Then
                sr.ResourceName = lastResourceString
            End If
            Dim result As DialogResult = sr.ShowDialog(Me)
            If result = System.Windows.Forms.DialogResult.OK Then
                lastResourceString = sr.ResourceName
                Cursor.Current = Cursors.WaitCursor
                Using rmSession = New ResourceManager()
                    Try
                        Me._ResourcesComboBox.DataSource = Nothing
                        Me._ResourcesComboBox.Items.Clear()
                        Me._ResourcesComboBox.Text = sr.ResourceName
                        _Session = CType(rmSession.Open(sr.ResourceName), MessageBasedSession)
                        ' setting to 1000 -- 1000; 1001 -- 1, 4000 -- 10000'
                        _Session.TimeoutMilliseconds = 1001
                        SetupControlState(True)
                    Catch e1 As InvalidCastException
                        MessageBox.Show("Resource selected must be a message-based session")
                    Catch exp As Exception
                        MessageBox.Show(exp.Message)
                    Finally
                        Cursor.Current = Cursors.Default
                    End Try
                End Using
            End If
        End Using
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OpenSessions()
        Using sr As New SelectResources()
            Dim result As DialogResult = sr.ShowDialog(Me)
            If result = System.Windows.Forms.DialogResult.OK Then
                Cursor.Current = Cursors.WaitCursor
                Using rmSession = New ResourceManager()
                    Try
                        Me._Sessions = New List(Of MessageBasedSession)
                        Dim names As IEnumerable(Of String) = sr.ResourceNames
                        Me._ResourcesComboBox.DataSource = Nothing
                        Me._ResourcesComboBox.Items.Clear()
                        Me._ResourcesComboBox.DataSource = names
                        Dim builder As New System.Text.StringBuilder
                        For Each rs As String In names
                            lastResourceString = rs
                            Dim visaSession As MessageBasedSession = CType(rmSession.Open(rs), MessageBasedSession)
                            ' setting to 1000 -- 1000; 1001 -- 1, 4000 -- 10000'
                            visaSession.TimeoutMilliseconds = 1001
                            Me._Sessions.Add(visaSession)
                            ' TSP INITIALIZATION CODE:
                            visaSession.RawIO.Write($"_G.status.request_enable=0{vbCr}")

                            Dim solution As Integer = 3
                            If solution = 0 Then
                                ' Error
                                ' getting error -286 TSP Runtime error 
                                ' instrument reports User Abort Error
                                ' Visa reports USRE ABORT
                            ElseIf solution = 1 Then
                                ' still getting error -286 TSP Runtime error
                                ' but instrument connects and trace shows no error.
                            ElseIf solution = 2 Then
                                ' no longer getting error -286 TSP Runtime error
                                Threading.Thread.Sleep(10)
                            ElseIf solution = 3 Then
                                ' no longer getting error -286 TSP Runtime error
                                Threading.Thread.Sleep(1)
                            End If

                            ' this is the culprit: 
                            visaSession.Clear()

                            If solution = 0 Then
                                ' error 
                            ElseIf solution = 1 Then
                                Threading.Thread.Sleep(10)
                            ElseIf solution = 2 Then
                            ElseIf solution = 3 Then
                            End If

                            visaSession.RawIO.Write($"_G.waitcomplete() _G.print('1'){vbCr}")
                            builder.AppendLine(MainForm.InsertCommonEscapeSequences(visaSession.RawIO.ReadString))
                        Next
                        _ReadTextBox.Text = builder.ToString
                        If Me._Sessions.Count > 0 Then
                            Me._Session = Me._Sessions(0)
                            Me._ResourcesComboBox.Text = Me._Session.ResourceName
                            SetupControlState(Me._Session IsNot Nothing)
                        End If
                    Catch e1 As InvalidCastException
                        MessageBox.Show("Resource selected must be a message-based session")
                    Catch exp As Exception
                        MessageBox.Show(exp.Message)
                    Finally
                        Cursor.Current = Cursors.Default
                    End Try
                End Using
            End If
        End Using
    End Sub

    Private Sub openSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _OpenSessionButton.Click
        If Me._MultipleResourcesCheckBox.Checked Then
            Me.OpenSessions()
        Else
            Me.OpenSession()
        End If
    End Sub

    Private Sub closeSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _CloseSessionButton.Click
        SetupControlState(False)
        If Me._Sessions Is Nothing Then
            If Me._Session IsNot Nothing Then
                Me._Session.Dispose()
            End If
        Else
            Dim sq As New Queue(Of MessageBasedSession)(Me._Sessions)
            Me._Sessions.Clear()
            Do While sq.Count > 0
                Dim s As Session = sq.Dequeue
                s.Dispose()
                s = Nothing
            Loop
        End If
        Me._Session = Nothing
    End Sub

#End Region

#Region " MISC "

    Private Sub SetupControlState(ByVal isSessionOpen As Boolean)
        _OpenSessionButton.Enabled = Not isSessionOpen
        _CloseSessionButton.Enabled = isSessionOpen
        queryButton.Enabled = isSessionOpen
        writeButton.Enabled = isSessionOpen
        readButton.Enabled = isSessionOpen
        writeTextBox.Enabled = isSessionOpen
        clearButton.Enabled = isSessionOpen
        If isSessionOpen Then
            ' _ReadTextBox.Text = String.Empty
            writeTextBox.Focus()
        End If
    End Sub

    Private Shared Function ReplaceCommonEscapeSequences(ByVal s As String) As String
        Return s.Replace("\n", vbLf).Replace("\r", vbCr)
    End Function

    Private Shared Function InsertCommonEscapeSequences(ByVal s As String) As String
        Return s.Replace(vbLf, "\n").Replace(vbCr, "\r")
    End Function

    Private Sub _MultipleResourcesCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles _MultipleResourcesCheckBox.CheckedChanged
        If _MultipleResourcesCheckBox.Checked Then
            Me._OpenSessionButton.Text = "Open Sessions"
            Me._CloseSessionButton.Text = "Close Sessions"
        Else
            Me._OpenSessionButton.Text = "Open Session"
            Me._CloseSessionButton.Text = "Close Session"
        End If

    End Sub

#End Region

#Region " SESSIONS "

    Private _Session As MessageBasedSession

    Private _Sessions As List(Of MessageBasedSession)
    Private Sub _ResourcesComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _ResourcesComboBox.SelectedIndexChanged
        If Me._Sessions IsNot Nothing AndAlso Me._ResourcesComboBox.SelectedIndex >= 0 AndAlso Me._Sessions.Count > Me._ResourcesComboBox.SelectedIndex Then
            Me._Session = Me._Sessions(Me._ResourcesComboBox.SelectedIndex)
        End If
    End Sub

#End Region

#Region " READ / WRITE "


    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub query_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles queryButton.Click
        _ReadTextBox.Text = String.Empty
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim textToWrite As String = ReplaceCommonEscapeSequences(writeTextBox.Text)
            _Session.RawIO.Write(textToWrite)
            _ReadTextBox.Text = InsertCommonEscapeSequences(_Session.RawIO.ReadString())
        Catch exp As Exception
            MessageBox.Show(exp.Message)
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub write_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles writeButton.Click
        Try
            Dim textToWrite As String = ReplaceCommonEscapeSequences(writeTextBox.Text)
            _Session.RawIO.Write(textToWrite)
        Catch exp As Exception
            MessageBox.Show(exp.Message)
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub read_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles readButton.Click
        _ReadTextBox.Text = String.Empty
        Cursor.Current = Cursors.WaitCursor
        Try
            _ReadTextBox.Text = InsertCommonEscapeSequences(_Session.RawIO.ReadString())
        Catch exp As Exception
            MessageBox.Show(exp.Message)
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles clearButton.Click
        _ReadTextBox.Text = String.Empty
    End Sub

#End Region


End Class
