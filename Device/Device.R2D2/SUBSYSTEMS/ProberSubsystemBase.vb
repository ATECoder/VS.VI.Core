''' <summary> Defines the contract that must be implemented by R2D2 Prober Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class ProberSubsystemBase
    Inherits VI.ProberSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.Worker = New System.ComponentModel.BackgroundWorker() With {
            .WorkerSupportsCancellation = True
        }
        Me._CommandTrialCount = 3
        Me._CommandPollInterval = TimeSpan.FromMilliseconds(30)
        Me._CommandTimeoutInterval = TimeSpan.FromMilliseconds(200)

        Me.ProberTimer = New Timers.Timer(1.5 * Me._CommandTrialCount * Me._CommandTimeoutInterval.TotalMilliseconds)
        Me.ProberTimer.Stop()

    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me.ProberTimer IsNot Nothing Then
                    Me.ProberTimer.Stop()
                    System.Windows.Forms.Application.DoEvents()
                    Me.ProberTimer.Dispose()
                    Me.ProberTimer = Nothing
                End If
                ' Free managed resources when explicitly called
                If Me.Worker IsNot Nothing Then
                    Me.Worker.CancelAsync()
                    Windows.Forms.Application.DoEvents()
                    If Not (Me.Worker.IsBusy OrElse Me.Worker.CancellationPending) Then
                        Me.Worker.Dispose()
                    End If
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " IDENTITY COMPLETE "

    ''' <summary> A pattern specifying the Identity. </summary>
    Protected Property IdentityReplyPattern As String

#End Region

#Region " ERROR PATTERN "

    ''' <summary> A pattern specifying the Error Message coming back from the Prober. </summary>
    Protected Property ErrorReplyPattern As String

#End Region

#Region " MESSAGE FAILED "

    Private _MessageFailedPattern As String

    ''' <summary> Gets or sets the message failed pattern. </summary>
    ''' <value> The message failed pattern. </value>
    Public Property MessageFailedPattern As String
        Get
            Return _MessageFailedPattern
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.MessageFailedPattern) Then
                Me._MessageFailedPattern = value
                Me.SafeSendPropertyChanged()
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

#End Region

#Region " MESSAGE COMPLETED "

    Private _MessageCompletedPattern As String

    ''' <summary> Gets or sets the message Completed pattern. </summary>
    ''' <value> The message Completed pattern. </value>
    Public Property MessageCompletedPattern As String
        Get
            Return _MessageCompletedPattern
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.MessageCompletedPattern) Then
                Me._MessageCompletedPattern = value
                Me.SafeSendPropertyChanged()
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

#End Region

#Region " PATTERN COMPLETE "

    ''' <summary> A pattern specifying the pattern complete. </summary>
    Public Property PatternCompleteReplyPattern As String

#End Region

#Region " SET MODE "

    ''' <summary> A pattern specifying a mode setup command prefix. </summary>
    Protected Property SetModeCommandPrefix As String

#End Region

#Region " TEST COMPLETE "

    ''' <summary> Gets or sets the test complete command. </summary>
    ''' <value> The test complete command. </value>
    Public Property TestCompleteCommand As String

#End Region

#Region " TEST START "

    ''' <summary> Gets or sets the retest start pattern. </summary>
    ''' <value> The test start pattern. </value>
    Public Property RetestStartPattern As String

    ''' <summary> Gets or sets the test again start pattern. </summary>
    ''' <value> The test start pattern. </value>
    Public Property TestAgainStartPattern As String

    ''' <summary> Gets or sets the test start pattern. </summary>
    ''' <value> The test start pattern. </value>
    Public Property TestStartPattern As String

    ''' <summary> Gets or sets the first test start pattern. </summary>
    ''' <value> The first test start pattern. </value>
    Public Property FirstTestStartPattern As String

#End Region

#Region " WAFER START "

    ''' <summary> Gets or sets the wafer start pattern. </summary>
    ''' <value> The wafer start pattern. </value>
    Public Property WaferStartPattern As String

#End Region

#Region " WRITE "

    Private _SupportedCommandPrefixes As String()

    ''' <summary> Gets the supported command prefixes. </summary>
    ''' <returns> A String </returns>
    Public Function SupportedCommandPrefixes() As String()
        Return Me._SupportedCommandPrefixes
    End Function

    ''' <summary> Sets supported command prefixes. </summary>
    ''' <param name="value"> The value. </param>
    Protected Sub SetSupportedCommandPrefixes(ByVal value As String())
        Me._SupportedCommandPrefixes = value
    End Sub

    Private _SupportedCommands As String()

    ''' <summary> Gets the supported commands. </summary>
    ''' <returns> A String </returns>
    Public Function SupportedCommands() As String()
        Return Me._SupportedCommands
    End Function

    ''' <summary> Sets supported commands. </summary>
    ''' <param name="value"> The value. </param>
    Protected Sub SetSupportedCommands(ByVal value As String())
        Me._SupportedCommands = value
    End Sub

    Private _LastMessageSent As String
    ''' <summary> Gets or sets (protected) the last Message Sent. </summary>
    ''' <value> The last MessageSent. </value>
    Public Property LastMessageSent As String
        Get
            Return Me._LastMessageSent
        End Get
        Protected Set(value As String)
            Me._LastMessageSent = value
            Me.SafeSendPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

    ''' <summary> Sends a message to the instrument. </summary>
    ''' <param name="format"> Describes the format to use. </param>
    ''' <param name="args">   A variable-length parameters list containing arguments. </param>
    Public Sub Send(ByVal format As String, ByVal ParamArray args() As Object)
        Me.Send(String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
    End Sub

    ''' <summary> Synchronously writes and ASCII-encoded string data. Terminates the data with the 
    '''           <see cref="SessionBase.Termination">termination character</see>. </summary>
    ''' <param name="dataToWrite"> The data to write. </param>
    Public Sub Send(ByVal dataToWrite As String)
        Me.ProberTimer.Stop()
        Me.ClearSendSentinels()
        Me.ClearFetchSentinels()
        If Not String.IsNullOrWhiteSpace(dataToWrite) Then
            Me.LastMessageSent = dataToWrite
            Me.Session.WriteLine(dataToWrite)
            If dataToWrite.StartsWith(Me.TestCompleteCommand, StringComparison.OrdinalIgnoreCase) Then
                Me.TestCompleteSent = True
            ElseIf dataToWrite.StartsWith(Me.SetModeCommandPrefix, StringComparison.OrdinalIgnoreCase) Then
                Me.SetModeSent = True
            ElseIf Me._SupportedCommandPrefixes.Contains(dataToWrite.Substring(0, 2), StringComparer.OrdinalIgnoreCase) Then
            ElseIf dataToWrite.StartsWith("*SRE", StringComparison.OrdinalIgnoreCase) Then
                Me.LastReading = Me.MessageCompletedPattern
                Me.ParseReading(Me.LastReading)
            ElseIf dataToWrite.StartsWith("*IDN?", StringComparison.OrdinalIgnoreCase) Then
            Else
                Me.UnhandledMessageSent = True
            End If
        End If
    End Sub

#End Region

#Region " BACKGROUND WORKER "

    ''' <summary> Worker payload. </summary>
    ''' <history date="10/29/2013" by="David" revision=""> Created. </history>
    Private Class WorkerPayLoad
        Public Property Message As String
        Property TrialNumber As Integer
        Property ResendOnRetry As Boolean
        Public Property TrialCount As Integer
        Public Property PollInterval As TimeSpan
        Public Property TimeoutInterval As TimeSpan
    End Class

    Private WithEvents Worker As System.ComponentModel.BackgroundWorker

    Private Sub Worker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles Worker.DoWork
        If Not (Me.IsDisposed OrElse e Is Nothing OrElse e.Cancel) Then
            Dim payload As WorkerPayLoad = TryCast(e.Argument, WorkerPayLoad)
            payload.TrialNumber = 0
            Do
                Dim endTime As DateTime = DateTime.UtcNow.Add(payload.TimeoutInterval)
                payload.TrialNumber += 1
                If payload.ResendOnRetry OrElse payload.TrialNumber = 1 Then Me.Send(payload.Message)
                Do Until DateTime.UtcNow > endTime OrElse Me.MessageCompleted OrElse Me.MessageFailed
                    Dim pauseTime As DateTime = DateTime.UtcNow.Add(payload.PollInterval)
                    Do
                        Windows.Forms.Application.DoEvents()
                    Loop While DateTime.UtcNow < pauseTime
                Loop
            Loop Until Me.MessageCompleted OrElse Me.MessageFailed OrElse (payload.TrialNumber >= payload.TrialCount)
        End If
    End Sub

    Private Sub Worker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles Worker.RunWorkerCompleted
        If Not (Me.IsDisposed OrElse e Is Nothing OrElse e.Cancelled OrElse e.Error IsNot Nothing) Then
        End If
    End Sub

    ''' <summary> Gets or sets the number of command trials. </summary>
    ''' <value> The number of command trials. </value>
    Public Property CommandTrialCount As Integer

    ''' <summary> Gets or sets the command poll interval. </summary>
    ''' <value> The command poll interval. </value>
    Public Property CommandPollInterval As TimeSpan

    ''' <summary> Gets or sets the command timeout interval. </summary>
    ''' <value> The command timeout interval. </value>
    Public Property CommandTimeoutInterval As TimeSpan

    ''' <summary> Try sending a message using a background worker waiting for an asynchronous reply. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> <c>True</c> if message was completed before the trial count expired. </returns>
    Public Function TrySendAsync(value As String) As Boolean
        Return Me.TrySendAsync(value, Me.CommandTrialCount, Me.CommandPollInterval, Me.CommandTimeoutInterval)
    End Function

    ''' <summary> Try sending a message. </summary>
    ''' <param name="value">        The value. </param>
    ''' <param name="trialCount">   Number of trials. </param>
    ''' <param name="pollInterval"> The poll interval. </param>
    ''' <param name="timeout">      The timeout. </param>
    ''' <returns> <c>True</c> if message was completed before the trial count expired. </returns>
    Public Function TrySendAsync(ByVal value As String, ByVal trialCount As Integer,
                                 ByVal pollInterval As TimeSpan, ByVal timeout As TimeSpan) As Boolean
        ' wait for previous operation to complete.
        Dim endTime As DateTime = DateTime.UtcNow.Add(timeout)
        Do Until Me.IsDisposed OrElse Not Worker.IsBusy OrElse DateTime.UtcNow > endTime
            Windows.Forms.Application.DoEvents()
        Loop
        If Worker.IsBusy Then
            Worker.CancelAsync()
        End If
        endTime = DateTime.UtcNow.Add(timeout)
        Do Until Me.IsDisposed OrElse Not Worker.IsBusy OrElse DateTime.UtcNow > endTime
            Windows.Forms.Application.DoEvents()
        Loop
        If Worker.IsBusy Then
            Return False
        End If

        Dim payload As New WorkerPayLoad With {
            .Message = value,
            .TrialCount = trialCount,
            .PollInterval = pollInterval,
            .TimeoutInterval = timeout,
            .ResendOnRetry = False}

        If Not (Me.IsDisposed OrElse Me.Worker.IsBusy) Then
            endTime = DateTime.UtcNow.Add(payload.TimeoutInterval)
            Me.Worker.RunWorkerAsync(payload)
            ' wait for worker to get busy.
            Do While Not (Me.IsDisposed OrElse Worker.IsBusy)
                Windows.Forms.Application.DoEvents()
            Loop
            ' wait till worker is done
            Do Until Me.IsDisposed OrElse Not Worker.IsBusy OrElse DateTime.UtcNow > endTime
                Windows.Forms.Application.DoEvents()
            Loop
            Do Until Me.IsDisposed OrElse Not Worker.IsBusy
                Windows.Forms.Application.DoEvents()
            Loop
        End If
        Return Me.MessageCompleted
    End Function

    ''' <summary> Clears the send sentinels. </summary>
    Private Sub ClearSendSentinels()
        Me.TestCompleteSent = False
        Me.UnhandledMessageSent = False
    End Sub

#End Region

#Region " EMULATE "

    ''' <summary> Emulates sending a Prober message. </summary>
    ''' <param name="command">   The command. </param>
    ''' <param name="timeDelay"> The time delay. </param>
    Public Sub Emulate(ByVal command As String, ByVal timeDelay As TimeSpan)
        Me.ProberCommand = ""
        If Me.ProberTimer IsNot Nothing Then
            Me.ProberTimer.Stop()
            Me.ProberCommand = command
            Me.ProberTimer.Interval = timeDelay.TotalMilliseconds
            Me.ProberTimer.Start()
        End If
    End Sub

    ''' <summary> Gets or sets the Prober command. </summary>
    ''' <value> The Prober command. </value>
    Private Property ProberCommand As String

    Dim WithEvents ProberTimer As System.Timers.Timer

    ''' <summary> Event handler. Called by the Prober Timer for elapsed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Elapsed event information. </param>
    Private Sub ProberTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles ProberTimer.Elapsed
        If Me.ProberTimer IsNot Nothing Then
            Me.ProberTimer.Stop()
        End If
        If Not String.IsNullOrWhiteSpace(Me.proberCommand) Then
            Dim value As String = Me.proberCommand
            Me.proberCommand = ""
            Me.LastReading = value
            Me.ParseReading(value)
        End If
    End Sub

#End Region

#Region " FETCH "

    Private _SupportedEmulationCommands As String()

    ''' <summary> Gets the supported emulation commands. </summary>
    ''' <returns> A String. </returns>
    Public Function SupportedEmulationCommands() As String()
        Return Me._supportedEmulationCommands
    End Function

    ''' <summary> Sets supported emulation commands. </summary>
    ''' <param name="value"> The value. </param>
    Protected Sub SetSupportedEmulationCommands(ByVal value As String())
        Me._supportedEmulationCommands = value
    End Sub

    ''' <summary> Clears the fetch sentinels. </summary>
    Private Sub ClearFetchSentinels()
        Me.ErrorRead = False
        Me.IdentityRead = False
        Me.MessageCompleted = False
        Me.MessageFailed = False
        Me.UnhandledMessageReceived = False
    End Sub

    ''' <summary> Parses the message. </summary>
    Public Overrides Sub ParseReading(ByVal reading As String)
        Me.ClearFetchSentinels()
        If Not String.IsNullOrWhiteSpace(reading) Then
            If reading.StartsWith(Me.ErrorReplyPattern, StringComparison.OrdinalIgnoreCase) Then
                Me.ErrorRead = False
                Me.ErrorRead = True
            ElseIf reading.StartsWith(Me.IdentityReplyPattern, StringComparison.OrdinalIgnoreCase) Then
                Me.IdentityRead = False
                Me.IdentityRead = True
                Me.MessageCompleted = True
            ElseIf reading.StartsWith(Me.PatternCompleteReplyPattern, StringComparison.OrdinalIgnoreCase) Then
                'tag message completed as it is assumed that this is the reply to the test complete command.
                Me.MessageCompleted = True
                Me.PatternCompleteReceived = False
                Me.PatternCompleteReceived = True
            ElseIf reading.StartsWith(Me.FirstTestStartPattern, StringComparison.OrdinalIgnoreCase) Then
                'tag message completed as it is assumed that this is the reply to the test complete command.
                Me.MessageCompleted = True
                Me.RetestRequested = False
                Me.TestAgainRequested = False
                Me.IsFirstTestStart = True
                Me.TestStartReceived = True
            ElseIf reading.StartsWith(Me.TestStartPattern, StringComparison.OrdinalIgnoreCase) Then
                'tag message completed as it is assumed that this is the reply to the test complete command.
                Me.MessageCompleted = True
                Me.RetestRequested = False
                Me.TestAgainRequested = False
                Me.IsFirstTestStart = False
                Me.TestStartReceived = True
            ElseIf reading.StartsWith(Me.RetestStartPattern, StringComparison.OrdinalIgnoreCase) Then
                'tag message completed as it is assumed that this is the reply to the test complete command.
                Me.MessageCompleted = True
                Me.RetestRequested = True
                Me.TestAgainRequested = False
                Me.IsFirstTestStart = False
                Me.TestStartReceived = True
            ElseIf reading.StartsWith(Me.TestAgainStartPattern, StringComparison.OrdinalIgnoreCase) Then
                'tag message completed as it is assumed that this is the reply to the test complete command.
                Me.MessageCompleted = True
                Me.RetestRequested = False
                Me.TestAgainRequested = True
                Me.IsFirstTestStart = False
                Me.TestStartReceived = True
            ElseIf reading.StartsWith(Me.WaferStartPattern, StringComparison.OrdinalIgnoreCase) Then
                Me.WaferStartReceived = False
                Me.WaferStartReceived = True
            ElseIf reading.StartsWith(Me.MessageCompletedPattern, StringComparison.OrdinalIgnoreCase) Then
                Me.MessageCompleted = False
                Me.MessageCompleted = True
            ElseIf reading.StartsWith(Me.MessageFailedPattern, StringComparison.OrdinalIgnoreCase) Then
                Me.MessageFailed = True
            ElseIf reading.StartsWith("Keithley", StringComparison.OrdinalIgnoreCase) Then
                Me.MessageCompleted = True
            Else
                Me.UnhandledMessageReceived = True
            End If
        End If
    End Sub

#End Region

#Region " READ "

    ''' <summary> Attempts to read from the given data. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="raiseException"> true to raise exception. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function TryRead(ByVal raiseException As Boolean) As Boolean
        Dim affirmative As Boolean = False
        Dim bits As ServiceRequests = Me.StatusSubsystem.MessageAvailableBits
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Awaiting SRQ bits {0:X2}", CInt(bits))
        If Me.StatusSubsystem.TryAwaitServiceRequest(bits, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(10)) Then
            Me.FetchAndParse()
            If Me.MessageCompleted Then
                affirmative = True
            ElseIf Me.MessageFailed Then
                If raiseException Then
                    Throw New OperationFailedException("Message Failed received attempting to read;. ")
                Else
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Message Failed received attempting to read;. ")
                End If
            Else
                If raiseException Then
                    Throw New OperationFailedException("Unexpected reply '{0}' when reading;. ", Me.LastReading)
                Else
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Unexpected reply '{0}' when reading;. ", Me.LastReading)
                End If
            End If
        Else
            If raiseException Then
                Throw New OperationFailedException("Timeout waiting SRQ bits {0:X2} when reading;. ", CInt(bits))
            Else
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Timeout waiting SRQ bits {0:X2} when reading;. ", CInt(bits))
            End If
        End If
        Return affirmative
    End Function

#End Region

End Class

