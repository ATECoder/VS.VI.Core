Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Defines a local node subsystem. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/1/2016" by="David" revision=""> Based on legacy status subsystem. </history>
Public MustInherit Class LocalNodeSubsystemBase
    Inherits VI.SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystem">TSP status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._InitializeTimeout = TimeSpan.FromMilliseconds(30000)
        Me.showEventsStack = New System.Collections.Generic.Stack(Of EventLogModes?)
        Me.showPromptsStack = New System.Collections.Generic.Stack(Of PromptsState)
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
                Me.showEventsStack?.Clear() : Me.showEventsStack = Nothing
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
            Me.WriteAutoInstrumentMessages(PromptsState.Disable, EventLogModes.None)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                              "Exception ignored turning off prompts;. {0}", ex.ToFullBlownString)
        Finally
            Me.Session.RestoreTimeout()
        End Try

        Try
            ' flush the input buffer in case the instrument has some leftovers.
            Me.Session.DiscardUnreadData()
            If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Data discarded after turning prompts and errors off;. Data: {0}.", Me.Session.DiscardedData)
            End If
        Catch ex As NativeException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception ignored clearing read buffer;. {0}", ex.ToFullBlownString)
        End Try

        Try
            ' flush write may cause the instrument to send off a new data.
            Me.Session.DiscardUnreadData()
            If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Unread data discarded after discarding unset data;. Data: {0}.", Me.Session.DiscardedData)
            End If
        Catch ex As NativeException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception ignored clearing read buffer;. {0}", ex.ToFullBlownString)
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
        Me.QueryPromptsState()

        ' read the errors status
        Me.QueryShowEvents()

        Me.StatusSubsystem.QueryOperationCompleted()

        Me.ExecutionState = New TspExecutionState?

    End Sub

#End Region

#Region " SESSION "

    ''' <summary> Handles the Session property changed event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
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
                Case NameOf(VI.SessionBase.LastMessageReceived)
                    ' parse the command to get the TSP execution state.
                    Me.ParseExecutionState(Me.Session.LastMessageReceived, TspExecutionState.IdleReady)
                Case NameOf(VI.SessionBase.LastMessageSent)
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
        If Me.PromptsState <> PromptsState.None Then

            ' if prompts are on, 
            If Me.PromptsState = PromptsState.Enable Then

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
                Me.QueryPromptsState()

            End If

        End If

        Return Me.ExecutionState

    End Function

#End Region

#Region " SHOWS EVENTS MODE "

    Private _ShowEvents As EventLogModes?

    ''' <summary> Gets or sets the cached Shows Events Mode. </summary>
    Public Overloads Property ShowEvents As EventLogModes?
        Get
            Return Me._ShowEvents
        End Get
        Protected Set(ByVal value As EventLogModes?)
            If Not Nullable.Equals(Me.ShowEvents, value) Then
                Me._ShowEvents = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Shows Events Mode . </summary>
    ''' <param name="value"> the Shows Events Mode . </param>
    ''' <returns> A List of scans. </returns>
    Public Function ApplyShowEvents(ByVal value As EventLogModes) As EventLogModes?
        Me.WriteShowEvents(value)
        Return Me.QueryShowEvents()
    End Function

    ''' <summary> Gets the Shows Events Mode query command. </summary>
    ''' <value> the Shows Events Mode query command. </value>
    Protected Overridable ReadOnly Property ShowEventsQueryCommand As String = "_G.localnode.showevents"

    ''' <summary> Queries the Shows Events Modes. </summary>
    ''' <returns> The <see cref="EventLogModes">Shows Events Modes</see> or none if unknown. </returns>
    Public Function QueryShowEvents() As EventLogModes?
        Dim mode As String = Me.ShowEvents.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.ShowEventsQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = "Failed fetching Shows Events Modes"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.ShowEvents = New EventLogModes?
        Else
            Dim eventMode As Integer = 0
            If Integer.TryParse(mode, eventMode) Then
                Me.ShowEvents = CType(eventMode, EventLogModes)
            Else
                Me.ShowEvents = New EventLogModes?
            End If
        End If
        Return Me.ShowEvents
    End Function

    ''' <summary> Gets the Shows Events Mode command format. </summary>
    ''' <value> the Shows Events Mode command format. </value>
    ''' <remarks> SCPI Base Command: ":FORM:ELEM {0}". </remarks>
    Protected Overridable ReadOnly Property ShowEventsCommandFormat As String = "_G.localnode.showevents={0}"

    ''' <summary> Writes the Shows Events Modes without reading back the value from the device. </summary>
    ''' <param name="value"> the Shows Events Mode. </param>
    ''' <returns> The <see cref="EventLogModes">Shows Events Modes</see> or none if unknown. </returns>
    Public Function WriteShowEvents(ByVal value As EventLogModes) As EventLogModes?
        Me.Session.WriteLine(Me.ShowEventsCommandFormat, CInt(value))
        Me.ShowEvents = value
        Return Me.ShowEvents
    End Function

    ''' <summary> List Shows Events Modes. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    ''' <param name="excluded">    The excluded. </param>
    Public Sub ListEventLogMode(ByVal listControl As System.Windows.Forms.ComboBox, ByVal excluded As EventLogModes)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Dim selectedIndex As Integer = listControl.SelectedIndex
        With listControl
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(EventLogModes).ValueDescriptionPairs(Me.ShowEvents.GetValueOrDefault(EventLogModes.None) And Not excluded)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            If .Items.Count > 0 Then
                .SelectedIndex = Math.Max(selectedIndex, 0)
            End If
        End With
    End Sub

#End Region

#Region " PROMPTS STATE "

    Private _PromptsState As PromptsState

    ''' <summary> Gets or sets the prompts state sentinel. </summary>
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
    ''' <value> The prompts state. </value>
    Public Property PromptsState() As PromptsState
        Get
            Return Me._PromptsState
        End Get
        Protected Set(ByVal value As PromptsState)
            If Not Nullable.Equals(value, Me.PromptsState) Then
                Me._PromptsState = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the prompts state. </summary>
    Public Function ApplyPromptsState(ByVal value As PromptsState) As PromptsState
        Me.WritePromptsState(value)
        Return Me.QueryPromptsState()
    End Function

    ''' <summary> Gets or sets the prompts state query command. </summary>
    ''' <value> The prompts state query command. </value>
    Protected Overridable ReadOnly Property PromptsStateQueryCommand As String = "_G.localnode.prompts"

    ''' <summary> Queries the condition for showing prompts. Controls prompting. </summary>
    ''' <returns> The prompts state. </returns>
    Public Function QueryPromptsState() As PromptsState
        Dim mode As String = Me.PromptsState.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.PromptsStateQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = "Failed fetching prompts state"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.PromptsState = PromptsState.None
        Else
            Dim se As New StringEnumerator(Of PromptsState)
            Me.PromptsState = se.ParseContained(mode.BuildDelimitedValue)
        End If
        If Not Me.ProcessExecutionStateEnabled Then
            ' read execution state explicitly, because session events are disabled.
            Me.ReadExecutionState()
        End If
        Return Me.PromptsState
    End Function

    ''' <summary> The prompts state command format. </summary>
    Protected Overridable ReadOnly Property PromptsStateCommandFormat As String = "_G.localnode.prompts={0}"

    ''' <summary> Sets the condition for showing prompts. Controls prompting. </summary>
    ''' <exception cref="NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <param name="value"> true to value. </param>
    ''' <returns> <c>True</c> to prompts state; otherwise <c>False</c>. </returns>
    Public Function WritePromptsState(ByVal value As PromptsState) As PromptsState
        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{value} prompts;. ")
        Me.Session.LastNodeNumber = New Integer?
        If value <> PromptsState.None Then
            Me.Session.WriteLine(Me.PromptsStateCommandFormat, value.ExtractBetween)
        End If
        Me.PromptsState = value
        If Not Me.ProcessExecutionStateEnabled Then
            ' read execution state explicitly, because session events are disabled.
            Me.ReadExecutionState()
        End If
        Return Me.PromptsState
    End Function

    ''' <summary> Sets the instrument to automatically send or stop sending instrument prompts and events. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub WriteAutoInstrumentMessages(ByVal prompts As PromptsState, ByVal events As EventLogModes)

        ' flush the input buffer in case the instrument has some leftovers.
        Me.Session.DiscardUnreadData()

        Dim showPromptsCommand As String = "<failed to issue>"
        Try
            ' sets prompt transmissions
            Me.WritePromptsState(prompts)
            showPromptsCommand = Me.Session.LastMessageSent
        Catch
        End Try

        ' flush again in case turning off prompts added stuff to the buffer.
        Me.Session.DiscardUnreadData()
        If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Unread data discarded after turning prompts off;. Data: {0}.", Me.Session.DiscardedData)
        End If

        Dim showErrorsCommand As String = "<failed to issue>"
        Try
            ' turn off event log transmissions
            Me.WriteShowEvents(events)
            showErrorsCommand = Me.Session.LastMessageSent
        Catch
        End Try

        ' flush again in case turning off errors added stuff to the buffer.
        Me.Session.DiscardUnreadData()
        If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Unread data discarded after turning errors off;. Data: {0}.", Me.Session.DiscardedData)
        End If

        ' now validate
        Me.QueryShowEvents()
        If Not Me.ShowEvents.HasValue Then
            Throw New OperationFailedException(Me.ResourceName, showErrorsCommand, "turning off automatic event display failed; value not set.")
        ElseIf Me.ShowEvents.Value <> events Then
            Throw New OperationFailedException(Me.ResourceName, showPromptsCommand, $"turning off test script prompts failed; showing {Me.ShowEvents} expected {events}.")
        End If

    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Gets or sets the stack for storing the show events states. </summary>
    Private showEventsStack As System.Collections.Generic.Stack(Of EventLogModes?)

    ''' <summary> Gets or sets the stack for storing the prompts state states. </summary>
    Private showPromptsStack As System.Collections.Generic.Stack(Of PromptsState)

    ''' <summary> Clears the status. </summary>
    Public Sub ClearStatus()

        ' clear the stacks
        Me.showEventsStack.Clear()
        Me.showPromptsStack.Clear()

        If Me.Session.IsDeviceOpen Then
            Me.ExecutionState = TspExecutionState.IdleReady
        Else
            Me.ExecutionState = TspExecutionState.Closed
        End If

    End Sub

    ''' <summary> Restores the status of errors and prompts. </summary>
    Public Sub RestoreStatus()

        If Me.showEventsStack.Count > 0 Then
            Dim lastShowEvent As EventLogModes? = Me.showEventsStack.Pop
            If lastShowEvent.HasValue Then
                Me.WriteShowEvents(lastShowEvent.Value)
            End If
        End If
        If Me.showPromptsStack.Count > 0 Then
            Me.WritePromptsState(Me.showPromptsStack.Pop)
        End If

    End Sub

    ''' <summary> Saves the current status of errors and prompts. </summary>
    Public Sub StoreStatus()
        Me.showEventsStack.Push(Me.QueryShowEvents())
        Me.showPromptsStack.Push(Me.QueryPromptsState())
    End Sub

#End Region

End Class

''' <summary> Values that represent on off states. </summary>
Public Enum OnOffState
    <ComponentModel.Description("Off (smu.OFF)")> [Off] = 0
    <ComponentModel.Description("On (smu.ON)")> [On] = 1
End Enum

''' <summary> Values that represent prompts states. </summary>
Public Enum PromptsState
    ''' <summary> Not defined. </summary>
    <ComponentModel.Description("Not defined")> None
    <ComponentModel.Description("Disable (localnode.DISABLE)")> Disable = 1
    <ComponentModel.Description("Enable (localnode.ENABLE)")> Enable = 2
End Enum

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

''' <summary> A bit-field of flags for specifying event log modes. </summary>
<Flags>
Public Enum EventLogModes
    ''' <summary> No errors. </summary>
    <ComponentModel.Description("No events")> None = 0
    ''' <summary> Errors only. </summary>
    <ComponentModel.Description("Errors only (eventlog.SEV_ERR)")> Errors = 1
    ''' <summary> Warnings only. </summary>
    <ComponentModel.Description("Warnings only (eventlog.SEV_WARN)")> Warnings = 2
    ''' <summary> Information only. </summary>
    <ComponentModel.Description("Information only (eventlog.SEV_INFO)")> Information = 4
End Enum

