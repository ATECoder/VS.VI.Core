Imports isr.Core.Pith.StackTraceExtensions
''' <summary> Defines an Interactive Subsystem for a TSP System. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/1/2016" by="David" revision=""> Based on legacy status subsystem. </history>
Public MustInherit Class InteractiveSubsystem
    Inherits VI.SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystem">TSP status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._InitializeTimeout = TimeSpan.FromMilliseconds(30000)
        Me.showErrorsStack = New System.Collections.Generic.Stack(Of Boolean?)
        Me.showPromptsStack = New System.Collections.Generic.Stack(Of Boolean?)
        If statusSubsystem IsNot Nothing Then AddHandler statusSubsystem.Session.PropertyChanged, AddressOf Me.SessionPropertyChanged
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me.Session IsNot Nothing Then RemoveHandler Me.Session.PropertyChanged, AddressOf Me.SessionPropertyChanged
                Me.showErrorsStack?.Clear() : Me.showErrorsStack = Nothing
                Me.showPromptsStack?.Clear() : Me.showPromptsStack = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    Private _InitializeTimeout As TimeSpan
    ''' <summary> Gets or sets the time out for doing a reset and clear on the instrument. </summary>
    ''' <value> The connect timeout. </value>
    Public Property InitializeTimeout() As TimeSpan
        Get
            Return Me._InitializeTimeout
        End Get
        Set(ByVal value As TimeSpan)
            If Not value.Equals(Me.InitializeTimeout) Then
                Me._InitializeTimeout = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Clears the active state. Issues selective device clear. </summary>
    Public Sub ClearActiveState()
        Me.ExecutionState = TspExecutionState.IdleReady
    End Sub

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.ReadExecutionState()
        ' Set all cached values that get reset by CLS
        Me.ClearStatus()
        Me.StatusSubsystem.QueryOperationCompleted()
    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()

        Try
            Me.Session.StoreTimeout(Me.InitializeTimeout)
            ' turn prompts off. This may not be necessary.
            Me.TurnPromptsErrorsOff()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Exception ignored turning off prompts;. Details: {0}.", ex)
        Finally
            Me.Session.RestoreTimeout()
        End Try

        Try
            ' flush the input buffer in case the instrument has some leftovers.
            Me.Session.DiscardUnreadData()
            If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Data discarded after turning prompts and errors off;. Data: {0}.", Me.Session.DiscardedData)
            End If
        Catch ex As NativeException
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception ignored clearing read buffer;. Details: {0}.", ex)
        End Try

        Try
            ' flush write may cause the instrument to send off a new data.
            Me.Session.DiscardUnreadData()
            If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Unread data discarded after discarding unset data;. Data: {0}.", Me.Session.DiscardedData)
            End If
        Catch ex As NativeException
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception ignored clearing read buffer;. Details: {0}.", ex)
        End Try

    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()

        ' clear elements.
        Me.ClearStatus()

        MyBase.ResetKnownState()

        ' enable processing of execution state.
        Me.ProcessExecutionStateEnabled = True

        ' read the prompts status
        Me.QueryShowPrompts()

        ' read the errors status
        Me.QueryShowErrors()

        Me.StatusSubsystem.QueryOperationCompleted()

        Me.ExecutionState = New TspExecutionState?

    End Sub

#End Region

#Region " SESSION "

    ''' <summary> Handles the Session property changed event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    Private Sub SessionPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If sender IsNot Nothing AndAlso e IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(e.PropertyName) Then
            Me.OnSessionPropertyChanged(e)
        End If
    End Sub


    ''' <summary> Handles the property changed event. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Sub OnSessionPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.ProcessExecutionStateEnabled AndAlso e IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(e.PropertyName) Then
            Select Case e.PropertyName
                Case NameOf(Me.Session.LastMessageReceived)
                    ' parse the command to get the TSP execution state.
                    Me.ParseExecutionState(Me.Session.LastMessageReceived, TspExecutionState.IdleReady)
                Case NameOf(Me.Session.LastMessageSent)
                    ' set the TSP status
                    Me.ExecutionState = TspExecutionState.Processing
            End Select
        End If
    End Sub

#End Region

#Region " ASSET TRIGGER "

    ''' <summary> Issues a hardware trigger. </summary>
    Public Sub AssertTrigger()
        Me.ExecutionState = TspExecutionState.IdleReady
        Me.Session.AssertTrigger()
    End Sub

#End Region

#Region " EXECUTION STATE "

    Private _ProcessExecutionStateEnabled As Boolean

    ''' <summary> Gets or sets the process execution state enabled. </summary>
    ''' <value> The process execution state enabled. </value>
    Public Property ProcessExecutionStateEnabled As Boolean
        Get
            Return Me._ProcessExecutionStateEnabled
        End Get
        Set(value As Boolean)
            If Not value.Equals(Me._ProcessExecutionStateEnabled) Then
                Me._ProcessExecutionStateEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _ExecutionState As TspExecutionState?

    ''' <summary> Gets or sets the last TSP execution state. Setting the last state is useful when
    ''' closing the Tsp System. </summary>
    ''' <value> The last state. </value>
    Public Property ExecutionState() As TspExecutionState?
        Get
            Return Me._ExecutionState
        End Get
        Set(ByVal Value As TspExecutionState?)
            If (Value.HasValue AndAlso Not Me.ExecutionState.HasValue) OrElse
                (Not Value.HasValue AndAlso Me.ExecutionState.HasValue) OrElse Not Value.Equals(Me.ExecutionState) Then
                Me._ExecutionState = Value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets the instrument Execution State caption. </summary>
    ''' <value> The state caption. </value>
    Public ReadOnly Property ExecutionStateCaption() As String
        Get
            If Me.ExecutionState.HasValue Then
                Return isr.Core.Pith.EnumExtensions.Description(Me.ExecutionState.Value)
            Else
                Return "N/A"
            End If
        End Get
    End Property

    ''' <summary> Parses the state of the TSP prompt and saves it in the state cache value. </summary>
    ''' <param name="value"> Specifies the read buffer. </param>
    ''' <returns> The instrument Execution State. </returns>
    Public Function ParseExecutionState(ByVal value As String, ByVal defaultValue As TspExecutionState) As TspExecutionState
        Dim state As TspExecutionState = defaultValue
        If String.IsNullOrWhiteSpace(value) OrElse value.Length < 4 Then
        Else
            value = value.Substring(0, 4)
            If value.StartsWith(TspSyntax.ReadyPrompt, True, Globalization.CultureInfo.CurrentCulture) Then
                state = TspExecutionState.IdleReady
            ElseIf value.StartsWith(TspSyntax.ContinuationPrompt, True, Globalization.CultureInfo.CurrentCulture) Then
                state = TspExecutionState.IdleContinuation
            ElseIf value.StartsWith(TspSyntax.ErrorPrompt, True, Globalization.CultureInfo.CurrentCulture) Then
                state = TspExecutionState.IdleError
            Else
                ' no prompt -- set to the default state
                state = defaultValue
            End If
        End If
        Me.ExecutionState = state
        Return state
    End Function

    ''' <summary> Reads the state of the TSP prompt and saves it in the state cache value. </summary>
    ''' <returns> The instrument Execution State. </returns>
    Public Function ReadExecutionState() As TspExecutionState?

        ' check status of the prompt flag.
        If Me.ShowPrompts.HasValue Then

            ' if prompts are on, 
            If Me.ShowPrompts.Value Then

                ' do a read. This raises an event that parses the state
                If Me.StatusSubsystem.IsMessageAvailable(TimeSpan.FromMilliseconds(1), 3) Then
                    Me.Session.ReadLine()
                End If

            Else

                Me.ExecutionState = TspExecutionState.Unknown

            End If

        Else

            ' check if we have data in the output buffer.  
            If Me.StatusSubsystem.IsMessageAvailable(TimeSpan.FromMilliseconds(1), 3) Then

                ' if data exists in the buffer, it may indicate that the prompts are already on 
                ' so just go read the output buffer. Once read, the status will be parsed.
                Me.Session.ReadLine()

            Else

                ' if we have no value then we must first read the prompt status
                ' once read, the status will be parsed.
                Me.QueryShowPrompts()

            End If

        End If

        Return Me.ExecutionState

    End Function

#End Region

#Region " SHOW ERRORS "

    Private _showErrors As Nullable(Of Boolean)

    ''' <summary> Gets or sets the Show Errors sentinel. </summary>
    ''' <remarks> When true, the unit will automatically display the errors stored in the error queue,
    ''' and then clear the queue. Errors will be processed at the end of executing a command message
    ''' (just prior to issuing a prompt if prompts are enabled). When false, errors will not display.
    ''' Errors will be left in the error queue and must be explicitly read or cleared. The error
    ''' prompt (TSP?) is enabled. </remarks>
    ''' <value> <c>True</c> to show errors; otherwise <c>False</c>. </value>
    Public Property ShowErrors() As Boolean?
        Get
            Return Me._showErrors
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Nullable.Equals(value, Me.ShowErrors) Then
                Me._showErrors = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the show errors sentinel. </summary>
    ''' <param name="value"> <c>True</c> to show errors; otherwise, <c>False</c>. </param>
    ''' <returns> <c>True</c> if on; otherwise <c>False</c>. </returns>
    Public Function ApplyShowErrors(ByVal value As Boolean) As Boolean?
        Me.WriteShowErrors(value)
        Return Me.QueryShowErrors()
    End Function

    ''' <summary> Reads the condition for showing errors. </summary>
    ''' <returns> <c>True</c> to show errors; otherwise <c>False</c>. </returns>
    Public Function QueryShowErrors() As Boolean?
        Me.ShowErrors = Me.Session.QueryPrint(False, TspSyntax.ShowErrors)
        If Not Me.ProcessExecutionStateEnabled Then
            ' read execution state explicitly, because session events are disabled.
            Me.ReadExecutionState()
        End If
        Return Me.ShowErrors
    End Function

    ''' <summary> Sets the condition for showing errors. </summary>
    ''' <exception cref="NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <param name="value"> true to value. </param>
    ''' <returns> <c>True</c> to show errors; otherwise <c>False</c>. </returns>
    Public Function WriteShowErrors(ByVal value As Boolean) As Boolean?
        Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "showing errors;. ")
        Me.Session.LastNodeNumber = New Integer?
        Me.Session.WriteLine(TspSyntax.ShowErrorsSetterCommand, CType(value, Integer))
        Me.ShowErrors = value
        If Not Me.ProcessExecutionStateEnabled Then
            ' read execution state explicitly, because session events are disabled.
            Me.ReadExecutionState()
        End If
        Return Me.ShowErrors
    End Function

#End Region

#Region " SHOW PROMPTS "

    Private _showPrompts As Boolean?

    ''' <summary> Gets or sets the Show Prompts sentinel. </summary>
    ''' <remarks> When true, prompts are issued after each command message is processed by the
    ''' instrument.<para>
    ''' When false prompts are not issued.</para><para>
    ''' Command messages do not generate prompts. Rather, the TSP instrument generates prompts in
    ''' response to command messages. When prompting is enabled, the instrument generates prompts in
    ''' response to command messages. There are three prompts that might be returned:</para><para>
    ''' “TSP&gt;” is the standard prompt. This prompt indicates that everything is normal and the
    ''' command is done processing.</para><para>
    ''' “TSP?” is issued if there are entries in the error queue when the prompt is issued. Like the
    ''' “TSP&gt;” prompt, it indicates the command is done processing. It does not mean the previous
    ''' command generated an error, only that there are still errors in the queue when the command
    ''' was done processing.</para><para>
    ''' “&gt;&gt;&gt;&gt;” is the continuation prompt. This prompt is used when downloading scripts
    ''' or flash images. When downloading scripts or flash images, many command messages must be sent
    ''' as a unit. The continuation prompt indicates that the instrument is expecting more messages
    ''' as part of the current command.</para> </remarks>
    ''' <value> The show prompts. </value>
    Public Property ShowPrompts() As Boolean?
        Get
            Return Me._showPrompts
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Nullable.Equals(value, Me.ShowErrors) Then
                Me._showPrompts = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the show Prompts sentinel. </summary>
    ''' <param name="value"> <c>True</c> to show Prompts; otherwise, <c>False</c>. </param>
    ''' <returns> <c>True</c> if on; otherwise <c>False</c>. </returns>
    Public Function ApplyShowPrompts(ByVal value As Boolean) As Boolean?
        Me.WriteShowPrompts(value)
        Return Me.QueryShowPrompts()
    End Function

    ''' <summary> Queries the condition for showing prompts. Controls prompting.
    ''' </summary>
    ''' <returns> <c>True</c> to show prompts; otherwise <c>False</c>. </returns>
    Public Function QueryShowPrompts() As Boolean?
        Me.ShowPrompts = Me.Session.QueryPrint(False, TspSyntax.ShowPrompts)
        If Not Me.ProcessExecutionStateEnabled Then
            ' read execution state explicitly, because session events are disabled.
            Me.ReadExecutionState()
        End If
        Return Me.ShowPrompts
    End Function

    ''' <summary> Sets the condition for showing prompts. Controls prompting. </summary>
    ''' <exception cref="NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <param name="value"> true to value. </param>
    ''' <returns> <c>True</c> to show prompts; otherwise <c>False</c>. </returns>
    Public Function WriteShowPrompts(ByVal value As Boolean) As Boolean?
        Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "showing prompts;. ")
        Me.Session.LastNodeNumber = New Integer?
        Me.Session.WriteLine(TspSyntax.ShowPromptsSetterCommand, CType(value, Integer))
        Me.ShowPrompts = value

        If Not Me.ProcessExecutionStateEnabled Then
            ' read execution state explicitly, because session events are disabled.
            Me.ReadExecutionState()
        End If
        Return Me.ShowPrompts
    End Function

    ''' <summary> Turns off prompts and errors. It seems that the new systems come with prompts and
    ''' errors off when the instrument is started or reset so this is not needed. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub TurnPromptsErrorsOff()

        ' flush the input buffer in case the instrument has some leftovers.
        Me.Session.DiscardUnreadData()

        Dim showPromptsCommand As String = "<failed to issue>"
        Try
            ' turn off prompt transmissions
            Me.WriteShowPrompts(False)
            showPromptsCommand = Me.Session.LastMessageSent
        Catch
        End Try

        ' flush again in case turning off prompts added stuff to the buffer.
        Me.Session.DiscardUnreadData()
        If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Unread data discarded after turning prompts off;. Data: {0}.", Me.Session.DiscardedData)
        End If

        Dim showErrorsCommand As String = "<failed to issue>"
        Try
            ' turn off error transmissions
            Me.WriteShowErrors(False)
            showErrorsCommand = Me.Session.LastMessageSent
        Catch
        End Try

        ' flush again in case turning off errors added stuff to the buffer.
        Me.Session.DiscardUnreadData()
        If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Unread data discarded after turning errors off;. Data: {0}.", Me.Session.DiscardedData)
        End If

        ' now validate
        If Me.QueryShowErrors.GetValueOrDefault(True) Then
            Throw New OperationFailedException(Me.ResourceName, showErrorsCommand, "turning off automatic error display--still on.")
        ElseIf Me.QueryShowPrompts.GetValueOrDefault(True) Then
            Throw New OperationFailedException(Me.ResourceName, showPromptsCommand, "turning off test script prompts--still on.")
        End If

    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Gets or sets the stack for storing the show errors states. </summary>
    Private showErrorsStack As System.Collections.Generic.Stack(Of Boolean?)

    ''' <summary> Gets or sets the stack for storing the show prompts states. </summary>
    Private showPromptsStack As System.Collections.Generic.Stack(Of Boolean?)

    ''' <summary> Clears the status. </summary>
    Public Sub ClearStatus()

        ' clear the stacks
        Me.showErrorsStack.Clear()
        Me.showPromptsStack.Clear()

        If Me.Session.IsDeviceOpen Then
            Me.ExecutionState = TspExecutionState.IdleReady
        Else
            Me.ExecutionState = TspExecutionState.Closed
        End If

    End Sub

    ''' <summary> Restores the status of errors and prompts. </summary>
    Public Sub RestoreStatus()

        Dim lastValue As Boolean? = Me.showErrorsStack.Pop
        If lastValue.HasValue Then
            Me.WriteShowErrors(lastValue.Value)
        End If
        lastValue = Me.showPromptsStack.Pop
        If lastValue.HasValue Then
            Me.WriteShowPrompts(lastValue.Value)
        End If

    End Sub

    ''' <summary> Saves the current status of errors and prompts. </summary>
    Public Sub StoreStatus()
        Me.showErrorsStack.Push(Me.QueryShowErrors())
        Me.showPromptsStack.Push(Me.QueryShowPrompts())
    End Sub

#End Region

End Class

''' <summary> Enumerates the TSP Execution State. </summary>
Public Enum TspExecutionState
    ''' <summary> Not defined. </summary>
    <ComponentModel.Description("Not defined")> None
    ''' <summary> Closed. </summary>
    <ComponentModel.Description("Closed")> Closed
    ''' <summary> Received the continuation prompt.
    ''' Send between lines when loading a script indicating that
    ''' TSP received script line successfully and is waiting for next line
    ''' or the end script command. </summary>
    <ComponentModel.Description("Continuation")> IdleContinuation
    ''' <summary> Received the error prompt. Error occurred; 
    '''           handle as desired. Use “errorqueue” commands to read and clear errors. </summary>
    <ComponentModel.Description("Error")> IdleError
    ''' <summary> Received the ready prompt. For example, TSP received script successfully and is ready for next command. </summary>
    <ComponentModel.Description("Ready")> IdleReady
    ''' <summary> A command was sent to the instrument. </summary>
    <ComponentModel.Description("Processing")> Processing
    ''' <summary> Cannot tell because prompt are off. </summary>
    <ComponentModel.Description("Unknown")> Unknown
End Enum

