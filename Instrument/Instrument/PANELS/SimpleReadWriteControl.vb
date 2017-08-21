Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Pith
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.Core.Pith.ExceptionExtensions
Imports isr.Core.Pith.StopwatchExtensions
''' <summary> A simple read write control. </summary>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="12/29/2015" by="David" revision=""> Created. </history>
Public Class SimpleReadWriteControl
    Inherits TalkerControlBase

#Region " CONSTRUCTORS "

    ''' <summary>
    ''' Constructor that prevents a default instance of this class from being created.
    ''' </summary>
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        Dim escape As String = isr.Core.Pith.EscapeSequencesExtensions.NewLineEscape
        With Me._WriteComboBox
            .Items.Clear()
            .Items.Add(Ieee488.Syntax.ClearExecutionStateCommand & escape)
            .Items.Add(Ieee488.Syntax.IdentityQueryCommand & escape)
            .Items.Add(Ieee488.Syntax.OperationCompletedCommand & escape)
            .Items.Add(Ieee488.Syntax.OperationCompletedQueryCommand & escape)
            .Items.Add(Ieee488.Syntax.ResetKnownStateCommand & escape)
            .Items.Add(String.Format(Globalization.CultureInfo.CurrentCulture,
                                     Ieee488.Syntax.ServiceRequestEnableCommandFormat, 255) & escape)
            .Items.Add(Ieee488.Syntax.ServiceRequestEnableQueryCommand & escape)
            .Items.Add(String.Format(Globalization.CultureInfo.CurrentCulture,
                                     Ieee488.Syntax.StandardEventEnableCommandFormat, 255) & escape)
            .Items.Add(Ieee488.Syntax.StandardEventEnableQueryCommand & escape)
            .Items.Add(Ieee488.Syntax.WaitCommand & escape)
            .SelectedIndex = 1
        End With

        Me._StopWatch = New Stopwatch

    End Sub


    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Me._Session = Nothing
                If components IsNot Nothing Then components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " SESSION "

    ''' <summary> Gets the session. </summary>
    ''' <value> The session. </value>
    Protected ReadOnly Property Session As SessionBase

    ''' <summary> Connects the given session. </summary>
    ''' <param name="session"> The session to connect. </param>
    Public Sub Connect(ByVal session As SessionBase)
        Me.Erase()
        Me._Session = session
        If Me.IsSessionOpen Then
            Me.StatusMessage = "Session Open."
        Else
            Me.StatusMessage = "Session not open. Check resource."
            Me._ReadTextBox.Text = "Failed opening message based VISA session"
        End If
        Me.ReadServiceRequestStatus()
        Me.OnConnectionChanged()
    End Sub

    ''' <summary> Disconnects this object. </summary>
    Public Sub Disconnect()
        Me._Session = Nothing
        Me.ReadServiceRequestStatus()
        Me.OnConnectionChanged()
    End Sub

    ''' <summary> Gets the sentinel indication having an open session. </summary>
    ''' <value> The is open. </value>
    Private ReadOnly Property IsSessionOpen As Boolean
        Get
            Return Me._Session IsNot Nothing AndAlso Me._Session.IsSessionOpen
        End Get
    End Property

    ''' <summary> Executes the connection changed action. </summary>
    Private Sub OnConnectionChanged()
        Me._ClearSessionButton.Enabled = Me.IsSessionOpen
        Me._QueryButton.Enabled = Me.IsSessionOpen
        Me._ReadButton.Enabled = Me.IsSessionOpen
        Me._WriteButton.Enabled = Me.IsSessionOpen
        Me._EraseButton.Enabled = True
    End Sub

#End Region

#Region " NOTIFICATION PROPERTIES "

    Private _ServiceRequestValue As Integer

    ''' <summary> Gets or sets the service request value. </summary>
    ''' <value> The service request value. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ServiceRequestValue As Integer
        Get
            Return Me._ServiceRequestValue
        End Get
        Protected Set(value As Integer)
            Me._ServiceRequestValue = value
            Me.SafePostPropertyChanged(NameOf(Me.ServiceRequestValue))
        End Set
    End Property

    Private _SentMessage As String

    ''' <summary> Gets or sets a message describing the sent. </summary>
    ''' <value> A message describing the sent. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property SentMessage As String
        Get
            Return Me._SentMessage
        End Get
        Protected Set(value As String)
            Me._SentMessage = value
            Me.SafePostPropertyChanged(NameOf(Me.SentMessage))
        End Set
    End Property

    Private _ReceivedMessage As String

    ''' <summary> Gets or sets a message describing the received. </summary>
    ''' <value> A message describing the received. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ReceivedMessage As String
        Get
            Return Me._ReceivedMessage
        End Get
        Protected Set(value As String)
            Me._ReceivedMessage = value
            Me.SafePostPropertyChanged(NameOf(Me.ReceivedMessage))
        End Set
    End Property

    Private _StatusMessage As String

    ''' <summary> Gets or sets a message describing the Status. </summary>
    ''' <value> A message describing the Status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property StatusMessage As String
        Get
            Return Me._StatusMessage
        End Get
        Protected Set(value As String)
            Me._StatusMessage = value
            Me.SafePostPropertyChanged(NameOf(Me.StatusMessage))
        End Set
    End Property

    Private _ReadEnabled As Boolean

    ''' <summary> Gets or sets the read enabled state. Use to prevent read or query if auto read is enabled. </summary>
    ''' <value> The read enabled. </value>
    <Category("Appearance"), Description("Enables reading"), Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(True)>
    Public Property ReadEnabled As Boolean
        Get
            Return Me._ReadEnabled
        End Get
        Set(value As Boolean)
            If Not Me.IsSessionOpen Then
                Me._ReadEnabled = value
                Me._ReadButton.Enabled = value
                Me._QueryButton.Enabled = value
                Me._ClearSessionButton.Enabled = value
                Me.SafePostPropertyChanged(NameOf(Me.ReadEnabled))
            End If
        End Set
    End Property

    Private _ElapsedTime As TimeSpan

    ''' <summary> Gets or sets the elapsed time. </summary>
    ''' <value> The elapsed time. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ElapsedTime As TimeSpan
        Get
            Return Me._ElapsedTime
        End Get
        Protected Set(value As TimeSpan)
            Me._ElapsedTime = value
            Me._TimingTextBox.Text = value.TotalMilliseconds.ToString("0.0", Globalization.CultureInfo.CurrentCulture)
            Me.SafePostPropertyChanged(NameOf(Me.ReadEnabled))
        End Set
    End Property

#End Region

#Region " READ AND WRITE "

    ''' <summary> Reads a service request status. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ReadServiceRequestStatus()
        Try
            If Me.IsSessionOpen Then
                Windows.Forms.Application.DoEvents()
                Stopwatch.StartNew.Wait(TimeSpan.FromMilliseconds(10))
                If Me.IsSessionOpen Then
                    Me.ServiceRequestValue = CInt(Me._Session.ReadStatusByte)
                Else
                    Me.ServiceRequestValue = 0
                End If
            Else
                Me.ServiceRequestValue = 0
            End If
        Catch ex As Exception
            Me.StatusMessage = "Error reading service request"
            Me._ReadTextBox.Text = ex.ToFullBlownString
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception reading service request;. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Builds write message. Add Return and New Line in this order as required. </summary>
    Private Function BuildWriteMessage() As String
        Dim message As New System.Text.StringBuilder(_WriteComboBox.Text.Trim)
        Return message.ToString
    End Function

    Private ReadOnly Property StopWatch As Stopwatch

    ''' <summary> Queries. </summary>
    ''' <param name="textToWrite"> The text to write. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub Query(ByVal textToWrite As String)
        Cursor.Current = Cursors.WaitCursor
        Try
            Me.StatusMessage = "Querying."
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Querying: '{0}'", textToWrite)
            Me._Session.LastAction = $"Querying: '{textToWrite}'".ToString(Globalization.CultureInfo.InvariantCulture)
            textToWrite = textToWrite.ReplaceCommonEscapeSequences
            Me.SentMessage = textToWrite
            Me.StopWatch.Restart()
            Dim responseString As String = Me._Session.Query(textToWrite)
            Me.ElapsedTime = Me.StopWatch.Elapsed
            Dim message As String = responseString.InsertCommonEscapeSequences
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Received: '{0}'", message)
            Me.updateReadMessage(message)
            Me.StatusMessage = "Done Querying."
        Catch ex As Exception
            Me.StatusMessage = "Error Querying."
            Me._ReadTextBox.Text = ex.ToFullBlownString
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception Querying;. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub

    ''' <summary> Gets or sets the service request registered. </summary>
    ''' <value> The service request registered sentinel. </value>
    Private Property ServiceRequestRegistered As Boolean

    ''' <summary> Writes. </summary>
    ''' <param name="textToWrite"> The text to write. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub Write(ByVal textToWrite As String)
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            If Me._Session.IsServiceRequestEnabled(ServiceRequests.MessageAvailable) And Not Me.ServiceRequestRegistered Then
                Me.StatusMessage = "Establishing service request handling."
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Establishing service request handling")
                ' checks if service requests are enabled for the MAV bit is enabled on the service request register.
                Me.ServiceRequestRegistered = True
                AddHandler Me._Session.ServiceRequested, AddressOf Me.OnServiceRequested
                ' this request some delay, otherwise, 
                ' the message is send before the event handler has been registered and therefor, no event takes place.
                Windows.Forms.Application.DoEvents()
                ' clear the status byte before reading
                Me.Session.ReadStatusByte()
            End If
            Me.StatusMessage = "Writing."
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Writing: '{0}'", textToWrite)
            textToWrite = textToWrite.ReplaceCommonEscapeSequences
            Me.SentMessage = textToWrite
            Me.StopWatch.Restart()
            Me._Session.WriteLine(textToWrite)
            Me.ElapsedTime = StopWatch.Elapsed
            Me.StatusMessage = "Done Writing."
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Writing completed;. ")
        Catch ex As Exception
            Me.StatusMessage = "Error Writing."
            Me._ReadTextBox.Text = ex.ToFullBlownString
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception writing;. {ex.ToFullBlownString}")
        Finally
            If Not Me.ServiceRequestRegistered Then Me.ReadServiceRequestStatus()
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub

    ''' <summary> Reads a message from the session. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub Read()
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            Me.StatusMessage = "Reading"
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Reading;. ")
            If Not Me.StopWatch.IsRunning Then Me.StopWatch.Restart()
            Dim responseString As String = Me._Session.ReadLine()
            Me.ElapsedTime = StopWatch.Elapsed
            Dim message As String = responseString.InsertCommonEscapeSequences
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Reading completed;. ")
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Received: '{0}'", message)
            Me.updateReadMessage(message)
            Me.StatusMessage = "Done Reading."
        Catch ex As Exception
            Me.StatusMessage = "Error Reading."
            Me._ReadTextBox.Text = ex.ToFullBlownString
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception reading;. {ex.ToFullBlownString}")
        Finally
            If Me._Session.ServiceRequestEventEnabled AndAlso Me.ServiceRequestRegistered Then
                Me.ServiceRequestRegistered = False
                RemoveHandler Me._Session.ServiceRequested, AddressOf Me.OnServiceRequested
            End If
            Me.ReadServiceRequestStatus()
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub

    ''' <summary> Erases the text boxes. </summary>
    Public Sub [Erase]()
        Me._ReadTextBox.Text = ""
        Me.ServiceRequestValue = 0
    End Sub

    ''' <summary> Clears the session. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ClearSession()
        Try
            If Me._Session IsNot Nothing Then
                Me.StatusMessage = "Clearing the session..."
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing the session;. ")
                Me._Session.Clear()
                Me.StatusMessage = "Cleared"
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Session cleared;. ")
            End If
        Catch ex As Exception
            Me.StatusMessage = "Error clearing the session."
            Me._ReadTextBox.Text = ex.ToFullBlownString
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception clearing session;. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub

#End Region

#Region " SERVICE REQUESTS "

    ''' <summary> Raises the service requested event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    Private Sub HandleMessageService(ByVal sender As SessionBase, value As ServiceRequests)
        If sender Is Nothing Then Throw New ArgumentNullException(NameOf(sender))
        If (value And CInt(ServiceRequests.MessageAvailable)) <> 0 Then
            Me.Read()
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Nothing to read; SRQ=0x{CInt(value):X2};. ")
        End If
    End Sub

    ''' <summary> Raises the service requested event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OnServiceRequested(ByVal sender As Object, ByVal e As EventArgs)
        Dim requester As SessionBase = TryCast(sender, SessionBase)
        If requester Is Nothing Then Return
        Try
            Dim sb As ServiceRequests = requester.ReadStatusByte()
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Servicing events: 0x{CInt(sb):X2}")
            Me.HandleMessageService(requester, sb)
            sb = requester.ReadStatusByte()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception handling session service request;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " CONTROL EVENTS "

    ''' <summary> Queries (write and then reads) the instrument. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _QueryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _QueryButton.Click
        Me.Query(Me.buildWriteMessage)
    End Sub

    ''' <summary> Writes a message to the instrument. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub Write_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _WriteButton.Click
        Me.Write(Me.buildWriteMessage)
    End Sub

    ''' <summary> Updates the read message described by message. </summary>
    ''' <param name="message"> The message. </param>
    Private Sub UpdateReadMessage(ByVal message As String)
        Me.ReceivedMessage = message
        Dim builder As New System.Text.StringBuilder()
        If Me._ReadTextBox.Text.Length > 0 Then
            builder.AppendLine(Me._ReadTextBox.Text)
        End If
        builder.Append(message)
        Me._ReadTextBox.Text = builder.ToString
    End Sub

    ''' <summary> Reads a message from the instrument. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub Read_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadButton.Click
        Me.read()
    End Sub

    ''' <summary> Event handler. Called by _ClearSessionButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ClearSessionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearSessionButton.Click
        Me.ClearSession()
    End Sub

    ''' <summary> Event handler. Called by clear for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _EraseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _EraseButton.Click
        Me.Erase()
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds a listener. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overrides Sub AddListener(ByVal listener As isr.Core.Pith.IMessageListener)
        MyBase.AddListener(listener)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of isr.Core.Pith.IMessageListener))
        MyBase.AddListeners(listeners)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AddListeners(ByVal talker As ITraceMessageTalker)
        MyBase.AddListeners(talker)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region

	
End Class
