Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.StackTraceExtensions
Imports isr.Core.Pith.StopwatchExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Defines the contract that must be implemented by Status Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class StatusSubsystemBase
    Inherits SubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystemBase" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.Pith.SessionBase">message based
    ''' session</see>. </param>
    Protected Sub New(ByVal visaSession As VI.Pith.SessionBase)
        Me.New(visaSession, VI.Pith.Scpi.Syntax.NoErrorCompoundMessage)
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="StatusSubsystemBase" /> class.
    ''' </summary>
    ''' <param name="visaSession">            A reference to a <see cref="Session">message based
    '''                                       session</see>. </param>
    ''' <param name="noErrorCompoundMessage"> A message describing the no error. </param>
    Protected Sub New(ByVal visaSession As VI.Pith.SessionBase, ByVal noErrorCompoundMessage As String)
        MyBase.New(visaSession)

        Me._VersionInfo = New isr.VI.VersionInfo()
        Me._OperationCompleted = New Boolean?
        Me._DeviceErrorBuilder = New System.Text.StringBuilder
        Me._NoErrorCompoundMessage = noErrorCompoundMessage
        Me._DeviceErrorQueue = New DeviceErrorQueue(noErrorCompoundMessage)

        ' check for query and other errors reported by the standard event register
        Me.StandardDeviceErrorAvailableBits = VI.Pith.StandardEvents.CommandError Or
            VI.Pith.StandardEvents.DeviceDependentError Or
            VI.Pith.StandardEvents.ExecutionError Or
            VI.Pith.StandardEvents.QueryError

        If visaSession IsNot Nothing Then AddHandler visaSession.PropertyChanged, AddressOf Me.SessionPropertyChanged

        Me._InitializeTimeout = TimeSpan.FromMilliseconds(5000)
        Me._DeviceClearRefractoryPeriod = TimeSpan.FromMilliseconds(1050)
        Me._ResetRefractoryPeriod = TimeSpan.FromMilliseconds(200)
        Me._InitRefractoryPeriod = TimeSpan.FromMilliseconds(100)
        Me._ClearRefractoryPeriod = TimeSpan.FromMilliseconds(100)

        ' check for query and other errors reported by the standard event register
        Me.StandardDeviceErrorAvailableBits = VI.Pith.StandardEvents.CommandError Or
                                              VI.Pith.StandardEvents.DeviceDependentError Or
                                              VI.Pith.StandardEvents.ExecutionError Or VI.Pith.StandardEvents.QueryError
        ' Me.StandardServiceEnableCommandFormat = Vi.Pith.Ieee488.Syntax.StandardServiceEnableCommandFormat
        ' Me.StandardServiceEnableCompleteCommandFormat = Vi.Pith.Ieee488.Syntax.StandardServiceEnableCompleteCommandFormat
        Me._OperationEventMap = New Dictionary(Of Integer, String)
    End Sub

    ''' <summary> Validated the given status subsystem base. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="statusSubsystemBase"> The status subsystem base. </param>
    ''' <returns> A StatusSubsystemBase. </returns>
    Public Shared Function Validated(ByVal statusSubsystemBase As VI.StatusSubsystemBase) As StatusSubsystemBase
        If statusSubsystemBase Is Nothing Then Throw New ArgumentNullException(NameOf(statusSubsystemBase))
        Return statusSubsystemBase
    End Function

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me.Session IsNot Nothing Then RemoveHandler Me.Session.PropertyChanged, AddressOf Me.SessionPropertyChanged
                Me.DeviceErrorBuilder?.Clear() : Me.DeviceErrorBuilder = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Executes the device open actions. </summary>
    ''' <remarks> This was added in order to defer reading error status after clearing the device state when opening the device. </remarks>
    Public Overridable Sub OnDeviceOpen()

        ' clear the device active state
        Me.ClearActiveState()

        ' reset device
        Me.ResetKnownState()

        ' Clear the device Status and set more defaults
        Me.ClearExecutionState()

    End Sub

    ''' <summary> Clears the active state.
    '''           Issues selective device clear. Waits for the <see cref="DeviceClearRefractoryPeriod"/> before releasing control. </summary>
    Public Overridable Sub ClearActiveState()
        Me.ClearService()
        If Me.Session.IsSessionOpen Then Stopwatch.StartNew.Wait(Me.DeviceClearRefractoryPeriod)
    End Sub

    ''' <summary> Gets the clear execution state command. </summary>
    ''' <remarks> SCPI: "*CLS".
    ''' <see cref="VI.Pith.Ieee488.Syntax.ClearExecutionStateCommand"> </see> </remarks>
    ''' <value> The clear execution state command. </value>
    Protected Overridable ReadOnly Property ClearExecutionStateCommand As String

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    ''' <remarks> Sends the '*CLS' message. 
    '''           '*CLS' clears the error queue. </remarks>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        If Me.Session.IsSessionOpen Then Stopwatch.StartNew.Wait(Me.ClearRefractoryPeriod)
        If Not String.IsNullOrWhiteSpace(Me.ClearExecutionStateCommand) Then
            Me.QueryOperationCompleted()
            Me.Session.WriteLine(Me.ClearExecutionStateCommand)
            Me.QueryOperationCompleted()
            Me.ReadServiceRequestStatus()
        End If
    End Sub

    ''' <summary> Returns the instrument registers to there preset power on state. </summary>
    ''' <remarks> SCPI: "*CLS".<para>
    ''' <see cref="VI.Pith.Ieee488.Syntax.ClearExecutionStateCommand"> </see> </para><para>
    ''' When this command is sent, the SCPI event registers are affected as follows:<p>
    '''   1. All bits of the positive transition filter registers are set to one (1).</p><p>
    '''   2. All bits of the negative transition filter registers are cleared to zero (0).</p><p>
    '''   3. All bits of the following registers are cleared to zero (0):</p><p>
    '''      a. Operation Event Enable Register.</p><p>
    '''      b. Questionable Event Enable Register.</p><p>
    '''   4. All bits of the following registers are set to one (1):</p><p>
    '''      a. Trigger Event Enable Register.</p><p>
    '''      b. Arm Event Enable Register.</p><p>
    '''      c. Sequence Event Enable Register.</p><p>
    ''' Note: Registers not included in the above list are not affected by this command.</p> </para></remarks>
    Public Overrides Sub PresetKnownState()
        MyBase.PresetKnownState()
        Me.PresetRegistersKnownState()
    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Clears Error Queue.
    '''           </para></remarks>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        If Me.Session.IsSessionOpen Then Stopwatch.StartNew.Wait(Me.InitRefractoryPeriod)
        Dim activity As String = "clearing error queue"
        Try
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{activity};. ")
            Me.ClearErrorQueue()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               $"Exception ignored clearing error queue;. {ex.ToFullBlownString}")
        End Try
        activity = "enabling wait completion"
        Try
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{activity};. ")
            Me.EnableWaitComplete()
        Catch ex As Exception
            ex.Data.Add($"data{ex.Data.Count}.resource", Me.Session.ResourceName)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
        End Try
        Try
            activity = "querying identity"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{activity};. ")
            Me.QueryIdentity()
        Catch ex As Exception
            ex.Data.Add($"data{ex.Data.Count}.resource", Me.Session.ResourceName)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Gets or sets the reset known state command. </summary>
    ''' <remarks> SCPI: "*RST".
    ''' <see cref="VI.Pith.Ieee488.Syntax.ResetKnownStateCommand"> </see> </remarks>
    ''' <value> The reset known state command. </value>
    Protected Overridable ReadOnly Property ResetKnownStateCommand As String

    ''' <summary> Sets the subsystem to its reset state. </summary>
    ''' <remarks> Sends the '*RST' message. </remarks>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Identity = ""
        Me.ServiceRequestEnableBitmask = 0
        Me.OperationCompleted = New Boolean?
        Me.Session.StandardServiceEnableCommandFormat = Me.StandardServiceEnableCommandFormat
        Me.Session.StandardEventQueryCommand = Me.StandardEventStatusQueryCommand
        Me.Session.ErrorAvailableBits = Me.ErrorAvailableBits
        Me.Session.MeasurementAvailableBits = Me.MeasurementAvailableBits
        Me.Session.MessageAvailableBits = Me.MessageAvailableBits
        Me.Session.StandardEventAvailableBits = Me.StandardEventAvailableBits
        Me.Session.TerminationCharacterEnabled = Me.TerminationCharacterEnabled
        If Me.TerminationCharacterEnabled Then
            Me.Session.TerminationCharacter = Me.TerminationCharacter
            Me.Session.ReadServiceRequestStatus()
        End If
        Me.ResetRegistersKnownState()
        Me.Session.Execute(Me.ResetKnownStateCommand)
        If Me.Session.IsSessionOpen Then Stopwatch.StartNew.Wait(Me.ResetRefractoryPeriod)
        Me.QueryOperationCompleted()
        Me.ReadServiceRequestStatus()
        Me.QueryLineFrequency()
        Me.ReadServiceRequestStatus()
    End Sub

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

    Private _DeviceClearRefractoryPeriod As TimeSpan
    ''' <summary> Gets the device clear refractory period. </summary>
    ''' <value> The device clear refractory period. </value>
    Public Property DeviceClearRefractoryPeriod As TimeSpan
        Get
            Return Me._DeviceClearRefractoryPeriod
        End Get
        Set(value As TimeSpan)
            If Not value.Equals(Me.DeviceClearRefractoryPeriod) Then
                Me._DeviceClearRefractoryPeriod = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _ResetRefractoryPeriod As TimeSpan
    ''' <summary> Gets the reset refractory period. </summary>
    ''' <value> The reset refractory period. </value>
    Public Property ResetRefractoryPeriod As TimeSpan
        Get
            Return Me._ResetRefractoryPeriod
        End Get
        Set(value As TimeSpan)
            If Not value.Equals(Me.ResetRefractoryPeriod) Then
                Me._ResetRefractoryPeriod = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _InitRefractoryPeriod As TimeSpan
    ''' <summary> Gets the initialize refractory period. </summary>
    ''' <value> The initialize refractory period. </value>
    Public Property InitRefractoryPeriod As TimeSpan
        Get
            Return Me._InitRefractoryPeriod
        End Get
        Set(value As TimeSpan)
            If Not value.Equals(Me.DeviceClearRefractoryPeriod) Then
                Me._InitRefractoryPeriod = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _ClearRefractoryPeriod As TimeSpan
    ''' <summary> Gets the clear refractory period. </summary>
    ''' <value> The clear refractory period. </value>
    Public Property ClearRefractoryPeriod As TimeSpan
        Get
            Return Me._ClearRefractoryPeriod
        End Get
        Set(value As TimeSpan)
            If Not value.Equals(Me.ClearRefractoryPeriod) Then
                Me._ClearRefractoryPeriod = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " SESSION "

#Region " ATTRIBUTES "

    ''' <summary> Gets the termination character enabled. </summary>
    ''' <value> The termination character enabled. </value>
    Protected Overridable ReadOnly Property TerminationCharacterEnabled As Boolean = False

    ''' <summary> Gets the termination character. </summary>
    ''' <value> The termination character. </value>
    Protected Overridable ReadOnly Property TerminationCharacter As Byte = isr.Core.Pith.EscapeSequencesExtensions.NewLineValue

#End Region

    ''' <summary> Executes the session property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal sender As VI.Pith.SessionBase, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.Pith.SessionBase.ServiceRequestStatus)
            Case NameOf(VI.Pith.SessionBase.ErrorAvailable)
                Me.SafeQueryDeviceErrors()
        End Select
        Me.SafeSendPropertyChanged(propertyName)
    End Sub


    ''' <summary> Handles the Session property changed event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SessionPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.Pith.SessionBase)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, VI.Pith.SessionBase), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " DEVICE CLEAR "

    ''' <summary> Clears the device. </summary>
    ''' <remarks> When communicating with a message-based device, particularly when you
    ''' are first developing your program, you may need to tell the device to clear its I/O buffers
    ''' so that you can start again. In addition, if a device has more information than you need, you
    ''' may want to read until you have everything you need and then tell the device to throw the
    ''' rest away. The <c>viClear()</c> operation performs these tasks. More specifically, the clear
    ''' operation lets a controller send the device clear command to the device it is associated with,
    ''' as specified by the interface specification and the type of device. The action that the
    ''' device takes depends on the interface to which it is connected. <para>
    ''' For a GPIB device, the controller sends the IEEE 488.1 SDC (04h) command.</para><para>
    ''' For a VXI or MXI device, the controller sends the Word Serial Clear (FFFFh) command.</para>
    ''' <para>
    ''' For the ASRL INSTR or TCPIP SOCKET resource, the controller sends the string "*CLS\n". The
    ''' I/O protocol must be set to VI_PROT_4882_STRS for this service to be available to these
    ''' resources.</para>
    ''' For more details on these clear commands, refer to your device documentation, the IEEE 488.1
    ''' standard, or the VXI bus specification. <para>
    ''' Source: NI-Visa HTML help.</para></remarks>
    Public Sub ClearService()
        Me.Session.Clear()
    End Sub

#End Region

#Region " DEVICE ERRORS: CLEAR "

    ''' <summary> Gets the clear error queue command. </summary>
    ''' <remarks> SCPI: ":STAT:QUE:CLEAR".
    ''' <see cref="VI.Pith.Scpi.Syntax.ClearErrorQueueCommand"> </see> </remarks>
    ''' <value> The clear error queue command. </value>
    Protected Overridable ReadOnly Property ClearErrorQueueCommand As String

    ''' <summary> Clears messages from the error queue. </summary>
    ''' <remarks> Sends the <see cref="ClearErrorQueueCommand">clear error queue</see> message. </remarks>
    Public Overridable Sub ClearErrorQueue()
        Me.ClearErrorCache()
        Me.Session.Execute(Me.ClearErrorQueueCommand)
    End Sub

    ''' <summary> Clears the error cache. </summary>
    Public Overridable Sub ClearErrorCache()
        Me.DeviceErrorQueue.Clear()
        Me.DeviceErrorBuilder = New System.Text.StringBuilder
        Me.SafePostPropertyChanged(NameOf(VI.StatusSubsystemBase.DeviceErrorsReport))
        Me.SafePostPropertyChanged(NameOf(VI.StatusSubsystemBase.LastDeviceError))
    End Sub

    ''' <summary> Gets the last message that was sent before the error. </summary>
    ''' <value> The message sent before error. </value>
    Public ReadOnly Property MessageSentBeforeError As String

    ''' <summary> Gets the last message that was received before the error. </summary>
    ''' <value> The message received before error. </value>
    Public ReadOnly Property MessageReceivedBeforeError As String

#End Region

#Region " DEVICE ERRORS: PUBLIC METHODS AND MEMBERS "

    ''' <summary> Queries device errors. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub SafeQueryDeviceErrors()
        Dim activity As String = "reading device errors"
        Try
            activity = "reading device errors"
            Dim e As New isr.Core.Pith.ActionEventArgs
            If Not Me.TrySafeQueryDeviceErrors(e) Then
                Dim message As New isr.Core.Pith.TraceMessage(TraceEventType.Warning, My.MyLibrary.TraceEventId, e.Details)
                If Me.Talker Is Nothing Then
                    isr.VI.My.MyLibrary.LogUnpublishedMessage(message)
                Else
                    Me.Talker.Publish(message)
                End If
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Queries device errors. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TrySafeQueryDeviceErrors(ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = "reading device errors"
        Try
            activity = "checking the error available bit and reading status"
            If Me.IsErrorBitSet And Not Me.ReadingDeviceErrors Then
                activity = "reading device errors"
                Me.TryQueryExistingDeviceErrors(e)
            End If
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Gets or sets the device errors queue. </summary>
    ''' <value> The device errors. </value>
    Public ReadOnly Property DeviceErrorQueue As DeviceErrorQueue

    ''' <summary> Gets or sets a message describing the no error compound message. </summary>
    ''' <value> A message describing the no error compound. </value>
    Public Property NoErrorCompoundMessage As String

    ''' <summary> Gets a report of the error stored in the cached error queue. </summary>
    ''' <value> The cached device errors. </value>
    Public ReadOnly Property DeviceErrorsReport() As String
        Get
            Dim builder As New System.Text.StringBuilder(Me.DeviceErrorBuilder.ToString)
            If Me.StandardEventStatus.GetValueOrDefault(0) <> 0 Then
                Dim report As String = StatusSubsystemBase.BuildReport(Me.StandardEventStatus.Value, ";")
                If Not String.IsNullOrWhiteSpace(report) Then
                    builder.AppendLine(report)
                End If
                If Not String.IsNullOrWhiteSpace(Me.MessageReceivedBeforeError) Then
                    builder.AppendLine($"Received: {Me.MessageReceivedBeforeError}")
                End If
                If Not String.IsNullOrWhiteSpace(Me.MessageSentBeforeError) Then
                    builder.AppendLine($"Sent: {Me.MessageSentBeforeError}")
                End If
            End If
            Return builder.ToString.TrimEnd(Environment.NewLine.ToCharArray)
        End Get
    End Property

    Private _ReadingDeviceErrors As Boolean
    ''' <summary> Gets or sets the reading device errors. </summary>
    ''' <value> The reading device errors. </value>
    Public Property ReadingDeviceErrors As Boolean
        Get
            Return Me._ReadingDeviceErrors
        End Get
        Protected Set(value As Boolean)
            If value <> Me.ReadingDeviceErrors Then
                Me._ReadingDeviceErrors = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries if error bit is set without affecting the <see cref="ErrorAvailable"/> property. </summary>
    ''' <returns> <c>true</c> if it error bit is on; otherwise <c>false</c> </returns>
    Public Function IsErrorBitSet() As Boolean
        ' read status byte without affecting the error available property.
        Return (Me.ErrorAvailableBits And Me.Session.ReadStatusByte) <> 0
    End Function

    ''' <summary> Gets the last device error. </summary>
    ''' <value> The last device error. </value>
    Public ReadOnly Property LastDeviceError As DeviceError
        Get
            Return Me.DeviceErrorQueue.LastError
        End Get
    End Property

#End Region

#Region " DEVICE ERRORS: PROTECTED QUERY METHODS AND MEMBERS "

    ''' <summary> Safe query existing device errors. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Protected Overridable Function TryQueryExistingDeviceErrors(ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Me.MessageAvailable Then
            ' if the device has message with an error state, the message must be fetched before the device
            ' errors can be fetched. The system requesting the message must:
            ' (1) fetch the message;
            ' (2) fetch the device errors.
            e.RegisterCancellation($"{Me.ResourceNameCaption} as a message available in the presence of a device error; the message needs to be fetched before fetching the device errors")
        Else
            ' There are currently two queue reading commands and two single error reading commands.
            If Not String.IsNullOrWhiteSpace(Me.NextDeviceErrorQueryCommand) Then
                Me.QueryDeviceErrors()
            ElseIf Not String.IsNullOrWhiteSpace(Me.DequeueErrorQueryCommand) Then
                Me.QueryErrorQueue()
            End If
            If Not String.IsNullOrWhiteSpace(Me.DeviceErrorQueryCommand) Then
                If String.IsNullOrWhiteSpace(Me.NextDeviceErrorQueryCommand) AndAlso
                    String.IsNullOrWhiteSpace(Me.DequeueErrorQueryCommand) Then
                    ' If No Queue reading commands, 
                    ' clear the cache as this device reports only the last error 
                    Me.ClearErrorCache()
                End If
                Me.QueryLastError()
            ElseIf Not String.IsNullOrWhiteSpace(Me.LastSystemErrorQueryCommand) Then
                If String.IsNullOrWhiteSpace(Me.NextDeviceErrorQueryCommand) AndAlso
                    String.IsNullOrWhiteSpace(Me.DequeueErrorQueryCommand) Then
                    ' If No Queue reading commands, 
                    ' clear the cache as this device reports only the last error 
                    Me.ClearErrorCache()
                End If
                Me.WriteLastSystemErrorQueryCommand()
                Windows.Forms.Application.DoEvents()
                Me.ReadLastSystemError()
            End If
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> The device errors. </summary>
    Protected Property DeviceErrorBuilder As System.Text.StringBuilder

    ''' <summary> Appends a device error message. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub AppendDeviceErrorMessage(ByVal value As String)
        If Not String.IsNullOrWhiteSpace(value) Then Me.DeviceErrorBuilder.AppendLine(value)
    End Sub

    ''' <summary> Enqueues device error. </summary>
    ''' <param name="compoundErrorMessage"> Message describing the compound error. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Protected Overridable Function EnqueueDeviceError(ByVal compoundErrorMessage As String) As DeviceError
        Dim de As DeviceError = New DeviceError(Me.NoErrorCompoundMessage)
        de.Parse(compoundErrorMessage)
        If de.IsError Then Me.DeviceErrorQueue.Enqueue(de)
        Return de
    End Function


    ''' <summary> Enqueue last error. </summary>
    ''' <param name="compoundErrorMessage"> Message describing the compound error. </param>
    ''' <returns> A DeviceError. </returns>
    Public Function EnqueueLastError(ByVal compoundErrorMessage As String) As DeviceError
        Dim de As DeviceError = New DeviceError(Me.NoErrorCompoundMessage)
        de = Me.EnqueueDeviceError(compoundErrorMessage)
        If de.IsError Then
            Me.AppendDeviceErrorMessage($"{Me.ResourceNameCaption} Last Error:
{de.ErrorMessage}")
            Me.SafePostPropertyChanged(NameOf(VI.StatusSubsystemBase.LastDeviceError))
        End If
        Return de
    End Function

#End Region

#Region " QUERY DEVICE ERROR: USES NEXT ERROR COMMANDS :STAT:QUE? "

    ''' <summary> Gets the 'Next Error' query command. </summary>
    ''' <remarks> SCPI: ":STAT:QUE?".
    ''' <see cref="VI.Pith.Scpi.Syntax.NextErrorQueryCommand"> </see> </remarks>
    ''' <value> The error queue query command. </value>
    Protected Overridable ReadOnly Property NextDeviceErrorQueryCommand As String

    ''' <summary> Reads the device errors. </summary>
    ''' <returns> The device errors. </returns>
    Protected Overridable Function QueryDeviceErrors() As String
        If Me.ReadingDeviceErrors Then Return ""
        Dim notifyLastError As Boolean = False
        Try
            Me._MessageSentBeforeError = Me.Session.LastMessageSent
            Me._MessageReceivedBeforeError = Me.Session.LastMessageReceived
            Me.ReadingDeviceErrors = True
            Me.ClearErrorCache()
            If Not String.IsNullOrWhiteSpace(Me.NextDeviceErrorQueryCommand) AndAlso Me.IsErrorBitSet() Then
                Dim builder As New System.Text.StringBuilder
                Dim de As DeviceError = DeviceError.NoError
                Do
                    de = Me.EnqueueDeviceError(Me.Session.QueryTrimEnd(Me.NextDeviceErrorQueryCommand))
                    If de.IsError Then
                        notifyLastError = True
                        builder.AppendLine(de.CompoundErrorMessage)
                    End If
                Loop While Me.IsErrorBitSet AndAlso de.IsError
                ' this is a kludge because the 7510 does not clear the error queue.
                If Not de.IsError AndAlso Me.IsErrorBitSet Then Me.Session.Execute(Me.ClearErrorQueueCommand)
                If builder.Length > 0 Then
                    Me.QueryStandardEventStatus()
                    Me.AppendDeviceErrorMessage(builder.ToString)
                End If
            End If
            If notifyLastError Then Me.SafePostPropertyChanged(NameOf(VI.StatusSubsystemBase.LastDeviceError))
            Me.SafePostPropertyChanged(NameOf(StatusSubsystemBase.DeviceErrorsReport))
            Return Me.DeviceErrorsReport
        Catch
            Throw
        Finally
            Me.ReadingDeviceErrors = False
        End Try
    End Function

#End Region

#Region " QUERY DEVICE ERROR: USES DEQUEUE COMMANDS ?? "

    ''' <summary> Gets the 'Next Error' query command. </summary>
    ''' <value> The error queue query command. </value>
    Protected Overridable ReadOnly Property DequeueErrorQueryCommand As String

    ''' <summary> Queries the next error from the device error queue. </summary>
    ''' <returns> The <see cref="DeviceError">Device Error structure.</see> </returns>
    Protected Function QueryNextError() As DeviceError
        Dim de As DeviceError = New DeviceError(Me.NoErrorCompoundMessage)
        If Not String.IsNullOrWhiteSpace(Me.DequeueErrorQueryCommand) Then
            de = Me.EnqueueDeviceError(Me.Session.QueryTrimEnd(Me.DequeueErrorQueryCommand))
        End If
        Return de
    End Function

    ''' <summary> Reads the device errors. </summary>
    ''' <returns> The device errors. </returns>
    Protected Overridable Function QueryErrorQueue() As String
        If Me.ReadingDeviceErrors Then Return ""
        Dim notifyLastError As Boolean = False
        Try
            Me._MessageSentBeforeError = Me.Session.LastMessageSent
            Me._MessageReceivedBeforeError = Me.Session.LastMessageReceived
            Me.ReadingDeviceErrors = True
            Me.ClearErrorCache()
            If Not String.IsNullOrWhiteSpace(Me.DequeueErrorQueryCommand) AndAlso Me.IsErrorBitSet() Then
                Dim builder As New System.Text.StringBuilder
                Dim de As DeviceError = DeviceError.NoError
                Do
                    de = Me.QueryNextError
                    If de.IsError Then
                        notifyLastError = True
                        builder.AppendLine(de.CompoundErrorMessage)
                    End If
                Loop While Me.IsErrorBitSet AndAlso de.IsError

                ' this is a kludge because the 7510 does not clear the error queue.
                If Not de.IsError AndAlso Me.IsErrorBitSet Then Me.Session.Execute(Me.ClearErrorQueueCommand)

                If builder.Length > 0 Then
                    Me.QueryStandardEventStatus()
                    Me.AppendDeviceErrorMessage(builder.ToString)
                End If
            End If
            If notifyLastError Then Me.SafePostPropertyChanged(NameOf(VI.StatusSubsystemBase.LastDeviceError))
            Me.SafePostPropertyChanged(NameOf(StatusSubsystemBase.DeviceErrorsReport))
            Return Me.DeviceErrorsReport
        Catch
            Throw
        Finally
            Me.ReadingDeviceErrors = False
        End Try
    End Function

#End Region

#Region " LAST ERROR "

    ''' <summary> Gets the last error query command. </summary>
    ''' <value> The last error query command. </value>
    Protected Overridable ReadOnly Property DeviceErrorQueryCommand As String

    ''' <summary> Queries the last error from the device. </summary>
    ''' <returns> The <see cref="DeviceError">Device Error structure.</see> </returns>
    Protected Overridable Function QueryLastError() As DeviceError
        Dim de As DeviceError = New DeviceError(Me.NoErrorCompoundMessage)
        If Not String.IsNullOrWhiteSpace(Me.DeviceErrorQueryCommand) Then
            de = Me.EnqueueDeviceError(Me.Session.QueryTrimEnd(Me.DeviceErrorQueryCommand))
            If de.IsError Then
                Me.AppendDeviceErrorMessage($"{Me.ResourceNameCaption} Last Error:
{de.ErrorMessage}")
            End If
        End If
        Return de
    End Function

#End Region

#Region " LAST SYSTEM ERROR "

    ''' <summary> Gets the last system error query command. </summary>
    ''' <value> The last error query command. </value>
    Protected Overridable ReadOnly Property LastSystemErrorQueryCommand As String

    ''' <summary> Reads last error. </summary>
    ''' <returns> The last error. </returns>
    Protected Function ReadLastSystemError() As DeviceError
        Dim de As DeviceError = New DeviceError(Me.NoErrorCompoundMessage)
        If Not String.IsNullOrWhiteSpace(Me.LastSystemErrorQueryCommand) Then
            de = Me.EnqueueLastError(Me.Session.ReadLine)
        End If
        Return de
    End Function

    ''' <summary> Writes the last error query command. </summary>
    Protected Sub WriteLastSystemErrorQueryCommand()
        If Not String.IsNullOrWhiteSpace(Me.LastSystemErrorQueryCommand) Then
            Me.Session.WriteLine(Me.LastSystemErrorQueryCommand)
        End If
    End Sub

#End Region

#Region " ERROR QUEUE COUNT "

    ''' <summary> The ErrorQueueCount. </summary>
    Private _ErrorQueueCount As Integer?

    ''' <summary> Gets or sets the cached error queue count. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property ErrorQueueCount As Integer?
        Get
            Return Me._ErrorQueueCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.ErrorQueueCount, value) Then
                Me._ErrorQueueCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property
    ''' <summary> Gets or sets The ErrorQueueCount query command. </summary>
    ''' <value> The ErrorQueueCount query command. </value>
    Protected Overridable Property ErrorQueueCountQueryCommand As String

    ''' <summary> Queries The ErrorQueueCount. </summary>
    ''' <returns> The ErrorQueueCount or none if unknown. </returns>
    Public Function QueryErrorQueueCount() As Integer?
        Me.ErrorQueueCount = Me.Query(Me.ErrorQueueCount, Me.ErrorQueueCountQueryCommand)
        Return Me.ErrorQueueCount
    End Function

#End Region

#Region " DEVICE REGISTERS "

    ''' <summary> Reads event registers. </summary>
    Public Overridable Sub ReadEventRegisters()
        Me.ReadServiceRequestStatus()
        Me._ReadEventRegisters()
    End Sub

#End Region

#Region " IDENTITY "

    ''' <summary> The identity. </summary>
    Private _Identity As String

    ''' <summary> Gets or sets the device identity string (*IDN?). </summary>
    ''' <value> The identity. </value>
    Public Property Identity As String
        Get
            Return Me._Identity
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then value = ""
            If Not String.Equals(value, Me.Identity) Then
                value = value.Trim
                Me._Identity = value
                Me.ParseVersionInfo(value)
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets the identity query command. </summary>
    ''' <remarks> SCPI: "*IDN?".
    ''' <see cref="VI.Pith.Ieee488.Syntax.IdentityQueryCommand"> </see> </remarks>
    ''' <value> The identity query command. </value>
    Protected Overridable ReadOnly Property IdentityQueryCommand As String

    ''' <summary> Writes the identity query command. </summary>
    ''' <remarks> This is required for systems which require asynchronous communication. </remarks>
    Public Sub WriteIdentityQueryCommand()
        Me.Write(Me.IdentityQueryCommand)
    End Sub

    ''' <summary> Queries the Identity. </summary>
    ''' <returns> System.String. </returns>
    ''' <remarks> Sends the '*IDN?' query. </remarks>
    Public Overridable Function QueryIdentity() As String
        If Not String.IsNullOrWhiteSpace(Me.IdentityQueryCommand) Then
            Me.Identity = Me.Session.QueryTrimEnd(Me.IdentityQueryCommand)
        End If
        Return Me.Identity
    End Function

    ''' <summary> Parse version information. </summary>
    ''' <param name="value"> The value. </param>
    Protected Overridable Sub ParseVersionInfo(ByVal value As String)
        Me._VersionInfo = New VersionInfo
        If Not String.IsNullOrWhiteSpace(value) Then
            Me.VersionInfo.Parse(value)
            Me.SerialNumberReading = Me.VersionInfo.SerialNumber
            If Not String.IsNullOrWhiteSpace(Me.VersionInfo.Model) Then Me.Session.ResourceNameInfo.ResourceTitle = Me.VersionInfo.Model
        End If
    End Sub

    ''' <summary> Gets information describing the version. </summary>
    ''' <value> Information describing the version. </value>
    Public ReadOnly Property VersionInfo As VersionInfo

#Region " SERIAL NUMBER "

    Private _SerialNumberReading As String

    ''' <summary> Gets or sets the serial number reading. </summary>
    ''' <value> The serial number reading. </value>
    Public Property SerialNumberReading As String
        Get
            Return Me._SerialNumberReading
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then value = ""
            If Not String.Equals(value, Me.SerialNumberReading) Then
                Me._SerialNumberReading = value
                Me.SafePostPropertyChanged()
                If String.IsNullOrWhiteSpace(value) Then
                    Me.SerialNumber = New Long?
                Else
                    Dim numericValue As Long = 0
                    If Long.TryParse(Me.SerialNumberReading, Globalization.NumberStyles.Number, Globalization.CultureInfo.InvariantCulture, numericValue) Then
                    End If
                    Me.SerialNumber = numericValue
                End If
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the serial number query command. </summary>
    ''' <value> The serial number query command. </value>
    Protected Overridable ReadOnly Property SerialNumberQueryCommand As String

    ''' <summary> Reads and returns the instrument serial number. </summary>
    ''' <returns> The serial number. </returns>
    ''' <exception cref="VI.Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    Public Function QuerySerialNumber() As String
        If Not String.IsNullOrWhiteSpace(Me.SerialNumberQueryCommand) Then
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Reading serial number;. ")
            Me.Session.LastNodeNumber = New Integer?
            Dim value As String = Me.Session.QueryTrimEnd(Me.SerialNumberQueryCommand)
            Me.CheckThrowDeviceException(False, "getting serial number;. using {0}.", Me.Session.LastMessageSent)
            Me.SerialNumberReading = value
        ElseIf String.IsNullOrWhiteSpace(Me.SerialNumberReading) AndAlso Me.SerialNumber.HasValue Then
            Me.SerialNumberReading = CStr(Me.SerialNumber.Value)
        End If
        Return Me.SerialNumberReading
    End Function

    Private _SerialNumber As Long?
    ''' <summary>
    ''' Reads and returns the instrument serial number
    ''' </summary>
    Public Property SerialNumber() As Long?
        Get
            Return Me._SerialNumber
        End Get
        Set(ByVal value As Long?)
            If Not Nullable.Equals(Me.SerialNumber, value) Then
                Me._SerialNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#End Region

#Region " LANGUAGE "

    ''' <summary> The Language. </summary>
    Private _Language As String

    ''' <summary> Gets or sets the device Language string (*IDN?). </summary>
    ''' <value> The Language. </value>
    Public Property Language As String
        Get
            Return Me._Language
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then value = ""
            If Not String.Equals(value, Me.Language) Then
                value = value.Trim
                Me._Language = value
                Me.SafePostPropertyChanged()
            End If
            Me.LanguageValidated = String.Equals(Me.Language, Me.ExpectedLanguage, StringComparison.OrdinalIgnoreCase)
        End Set
    End Property

    ''' <summary> Gets or sets the expected language. </summary>
    ''' <value> The expected language. </value>
    Public Property ExpectedLanguage As String = ""

    Private _LanguageValidated As Boolean

    ''' <summary> Gets or sets the sentinel indicating if the language was validated. </summary>
    ''' <value> The LanguageValidated. </value>
    Public Property LanguageValidated As Boolean
        Get
            Return Me._LanguageValidated
        End Get
        Set(ByVal value As Boolean)
            If Not Boolean.Equals(value, Me.LanguageValidated) Then
                Me._LanguageValidated = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the language. </summary>
    ''' <param name="value"> The required language. </param>
    ''' <returns> The language. </returns>
    Public Function ApplyLanguage(ByVal value As String) As String
        Me.WriteLanguage(value)
        Return Me.QueryLanguage()
    End Function

    ''' <summary> Gets the Language query command. </summary>
    ''' <remarks> SCPI: "*LANG?" </remarks>
    ''' <value> The Language query command. </value>
    Protected Overridable ReadOnly Property LanguageQueryCommand As String

    ''' <summary> Queries the Language. </summary>
    ''' <returns> The Language or Empty if not required or unknown. </returns>
    ''' <remarks> Sends the '*LANG?' query. </remarks>
    Public Overridable Function QueryLanguage() As String
        If String.IsNullOrWhiteSpace(Me.LanguageQueryCommand) Then
            Me.Language = ""
        Else
            Me.Language = Me.Session.QueryTrimEnd(Me.LanguageQueryCommand)
        End If
        Return Me.Language
    End Function

    ''' <summary> Gets the Language command format. </summary>
    ''' <value> The Language command format. </value>
    ''' <remarks> *LANG {0}" </remarks>
    Protected Overridable ReadOnly Property LanguageCommandFormat As String

    ''' <summary> Writes the Language value without reading back the value from the device. </summary>
    ''' <param name="value"> The current Language. </param>
    ''' <returns> The Language or Empty if not required or unknown. </returns>
    ''' <remarks> Changing the language causes the instrument to reboot. </remarks>
    Public Function WriteLanguage(ByVal value As String) As String
        If String.IsNullOrWhiteSpace(Me.LanguageQueryCommand) Then
            Me.Language = value
        Else
            Me.Language = Me.Write(Me.LanguageCommandFormat, value)
        End If
        Return Me.Language
    End Function

#End Region

#Region " OPC "

    ''' <summary> Gets or sets the wait command. </summary>
    ''' <value> The wait command. </value>
    Protected Overridable ReadOnly Property WaitCommand As String = VI.Pith.Ieee488.Syntax.WaitCommand

    ''' <summary> Issues the wait command. </summary>
    Public Sub Wait()
        Me.Write(Me.WaitCommand)
    End Sub

    ''' <summary> The operation completed. </summary>
    Private _OperationCompleted As Boolean?

    ''' <summary> Gets or sets the cached value indicating whether the last operation completed. </summary>
    ''' <value> <c>null</c> if operation completed contains no value, <c>True</c> if operation
    ''' completed; otherwise, <c>False</c>. </value>
    Public Property OperationCompleted As Boolean?
        Get
            Return Me._OperationCompleted
        End Get
        Protected Set(ByVal value As Boolean?)
            Me._OperationCompleted = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Gets or sets the operation completed query command. </summary>
    ''' <remarks> SCPI: "*OPC?".
    ''' <see cref="VI.Pith.Ieee488.Syntax.OperationCompletedQueryCommand"> </see> </remarks>
    ''' <value> The operation completed query command. </value>
    Protected Overridable ReadOnly Property OperationCompletedQueryCommand As String

    ''' <summary> Queries operation completed. </summary>
    ''' <param name="timeout"> Specifies how long to wait for the service request before throwing
    '''                        the timeout exception. Set to zero for an infinite (120 seconds)
    '''                        timeout. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function QueryOperationCompleted(ByVal timeout As TimeSpan) As Boolean?
        Me.OperationCompleted = Me.Session.Execute(AddressOf Me.QueryOperationCompleted, timeout)
    End Function

    ''' <summary> Issues the operation completion query, waits and returns a reply. </summary>
    ''' <returns> <c>True</c> if operation is complete; <c>False</c> otherwise. </returns>
    ''' <remarks> Sends the '*OPC?' query. </remarks>
    Public Function QueryOperationCompleted() As Boolean?
        Me.Session.MakeEmulatedReplyIfEmpty(True)
        If Not String.IsNullOrWhiteSpace(Me.OperationCompletedQueryCommand) Then
            Me.OperationCompleted = Me.Session.Query(True, Me.OperationCompletedQueryCommand)
        Else
            Me.OperationCompleted = True
        End If
        Return Me.OperationCompleted
    End Function

    ''' <summary> Gets the standard event wait complete bitmask. </summary>
    ''' <exception cref="TimeoutException"> Thrown when a Timeout error condition occurs. </exception>
    ''' <value> The standard event wait complete bitmask. </value>
    ''' <remarks> 3475. Add Or VI.Pith.Ieee4882.ServiceRequests.OperationEvent. </remarks>
    Public Property StandardEventWaitCompleteBitmask As VI.Pith.StandardEvents = VI.Pith.StandardEvents.All And Not VI.Pith.StandardEvents.RequestControl

    ''' <summary> Gets the service request wait complete bitmask. </summary>
    ''' <exception cref="TimeoutException"> Thrown when a Timeout error condition occurs. </exception>
    ''' <value> The service request wait complete bitmask. </value>
    Public Property ServiceRequestWaitCompleteBitmask As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.StandardEvent

    ''' <summary> Enabled detection of completion. </summary>
    ''' <param name="standardEventEnableBitmask">  Specifies standard events will issue an SRQ. </param>
    ''' <param name="serviceRequestEnableBitmask"> Specifies which status registers will issue an
    '''                                            SRQ. </param>
    Public Sub EnableWaitComplete(ByVal standardEventEnableBitmask As VI.Pith.StandardEvents,
                                  ByVal serviceRequestEnableBitmask As VI.Pith.ServiceRequests)
        Me.EnableServiceRequestComplete(standardEventEnableBitmask, serviceRequestEnableBitmask)
    End Sub

    ''' <summary> Enabled detection of completion. </summary>
    Public Sub EnableWaitComplete()
        Me.EnableWaitComplete(Me.StandardEventWaitCompleteBitmask, Me.ServiceRequestWaitCompleteBitmask)
    End Sub

    ''' <summary> Awaits for service request. Use this method to wait for an operation to complete on
    ''' this device.  Set the time out value to negative value for infinite time out. 
    ''' Uses minimum poll delay of 10ms or 0.1 timeout.</summary>
    ''' <param name="statusByteBits"> Specifies the status byte which to check. </param>
    ''' <param name="timeout">        Specifies how long to wait for the service request before
    ''' throwing the timeout exception. Set to zero for an infinite (120 seconds) timeout. </param>
    Public Sub AwaitServiceRequest(ByVal statusByteBits As VI.Pith.ServiceRequests, ByVal timeout As TimeSpan)
        Me.AwaitServiceRequest(statusByteBits, timeout, TimeSpan.FromSeconds(Math.Min(0.01, timeout.TotalSeconds / 10)))
    End Sub

    ''' <summary> Awaits for service request. Use this method to wait for an operation to complete on
    ''' this device.  Set the time out value to negative value for infinite time out. </summary>
    ''' <exception cref="TimeoutException"> Thrown when a Timeout error condition occurs. </exception>
    ''' <param name="statusByteBits"> Specifies the status byte which to check. </param>
    ''' <param name="timeout">        Specifies how long to wait for the service request before
    ''' throwing the timeout exception. Set to zero for an infinite (120 seconds) timeout. </param>
    ''' <param name="pollDelay">      Specifies time between serial polls. </param>
    Public Function TryAwaitServiceRequest(ByVal statusByteBits As VI.Pith.ServiceRequests, ByVal timeout As TimeSpan, ByVal pollDelay As TimeSpan) As Boolean

        ' emulate the reply for disconnected operations.
        Me.Session.MakeEmulatedReplyIfEmpty(statusByteBits)

        Dim endTime As Date

        ' check if time out is negative
        If timeout.TotalMilliseconds > 0 Then
            endTime = DateTime.UtcNow.Add(timeout)
        Else
            ' if negative, set to 'infinite' value.
            endTime = DateTime.UtcNow.AddMinutes(2)
        End If

        ' Clear the SRQ flag
        ' this clears the service request item (32) we are looking for!
        ' Me.ReadServiceRequestStatus()

        ' Loop until SRQ or Time out
        Do Until ((Me.ReadServiceRequestStatus And statusByteBits) > 0) OrElse (DateTime.UtcNow.CompareTo(endTime) > 0)
            Dim endPoll As DateTime = DateTime.UtcNow.Add(pollDelay)
            Do Until DateTime.UtcNow.CompareTo(endPoll) > 0
                Windows.Forms.Application.DoEvents()
            Loop
        Loop

        Return (Me.ServiceRequestStatus And statusByteBits) <> 0

    End Function


    ''' <summary> Awaits for service request. Use this method to wait for an operation to complete on
    ''' this device.  Set the time out value to negative value for infinite time out. </summary>
    ''' <exception cref="TimeoutException"> Thrown when a Timeout error condition occurs. </exception>
    ''' <param name="statusByteBits"> Specifies the status byte which to check. </param>
    ''' <param name="timeout">        Specifies how long to wait for the service request before
    ''' throwing the timeout exception. Set to zero for an infinite (120 seconds) timeout. </param>
    ''' <param name="pollDelay">      Specifies time between serial polls. </param>
    Public Sub AwaitServiceRequest(ByVal statusByteBits As VI.Pith.ServiceRequests, ByVal timeout As TimeSpan, ByVal pollDelay As TimeSpan)
        If Not Me.TryAwaitServiceRequest(statusByteBits, timeout, pollDelay) Then Throw New TimeoutException("Timeout awaiting service request.")
    End Sub

    ''' <summary> Waits for completion of last operation. </summary>
    ''' <remarks> Assumes requesting service event is registered with the instrument. </remarks>
    ''' <param name="timeout"> Specifies the time to wait for the instrument to return operation
    ''' completed. </param>
    Public Sub AwaitOperationCompleted(ByVal timeout As TimeSpan)
        Me.AwaitServiceRequest(VI.Pith.ServiceRequests.RequestingService Or VI.Pith.ServiceRequests.StandardEvent, timeout)
    End Sub

    ''' <summary> Waits for completion of last operation. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="timeout"> Specifies the time to wait for the instrument to return operation
    '''                        completed. </param>
    Public Sub AwaitOperationCompleted(ByVal value As VI.Pith.ServiceRequests, ByVal timeout As TimeSpan)
        Me.AwaitServiceRequest(value, timeout)
    End Sub

#End Region

#Region " STATUS REGISTER EVENTS: ERROR "

    ''' <summary> Gets the bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Public Overridable ReadOnly Property ErrorAvailableBits() As VI.Pith.ServiceRequests

    ''' <summary> Gets a value indicating whether [Error available]. </summary>
    ''' <value> <c>True</c> if [Error available]; otherwise, <c>False</c>. </value>
    Public ReadOnly Property ErrorAvailable As Boolean
        Get
            Return Me.Session.ErrorAvailable
        End Get
    End Property

#End Region

#Region " STATUS REGISTER EVENTS: MESSAGE "

    ''' <summary> Gets the bits that would be set for detecting if an Message is available. </summary>
    ''' <value> The Message available bits. </value>
    Public Overridable ReadOnly Property MessageAvailableBits() As VI.Pith.ServiceRequests

    ''' <summary> Gets a value indicating whether [Message available]. </summary>
    ''' <value> <c>True</c> if [Message available]; otherwise, <c>False</c>. </value>
    Public ReadOnly Property MessageAvailable As Boolean
        Get
            Return Me.Session.MessageAvailable
        End Get
    End Property

    ''' <summary> Is message available. </summary>
    ''' <remarks> Delays looking for the message status by the status latency to make sure the
    ''' instrument had enough time to process the previous command. </remarks>
    ''' <param name="latency">     The latency. </param>
    ''' <param name="repeatCount"> Number of repeats. </param>
    ''' <returns> <c>True</c> if message is available. </returns>
    Public Function IsMessageAvailable(ByVal latency As TimeSpan, ByVal repeatCount As Integer) As Boolean
        Return Me.Session.IsMessageAvailable(latency, repeatCount)
    End Function

#End Region

#Region " STATUS REGISTER EVENTS: MEASUREMENT "

    ''' <summary> Gets the bits that would be set for detecting if an Measurement is available. </summary>
    ''' <value> The Measurement available bits. </value>
    Public Overridable ReadOnly Property MeasurementAvailableBits() As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.MeasurementEvent

    ''' <summary> Gets a value indicating whether [Measurement available]. </summary>
    ''' <value> <c>True</c> if [Measurement available]; otherwise, <c>False</c>. </value>
    Public ReadOnly Property MeasurementAvailable As Boolean
        Get
            Return Me.Session.MeasurementAvailable
        End Get
    End Property

#End Region

#Region " STATUS REGISTER EVENTS: OPERATION "

    ''' <summary> Gets the bits that would be set for detecting if an Operation is available. </summary>
    ''' <value> The Operation available bits. </value>
    Public Overridable ReadOnly Property OperationAvailableBits() As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.OperationEvent

    ''' <summary> Gets a value indicating whether [Operation available]. </summary>
    ''' <value> <c>True</c> if [Operation available]; otherwise, <c>False</c>. </value>
    Public ReadOnly Property OperationAvailable As Boolean
        Get
            Return Me.Session.OperationAvailable
        End Get
    End Property

#End Region

#Region " STATUS REGISTER EVENTS: STANDARD EVENT "

    ''' <summary> Gets the bits that would be set for detecting if a Standard Event is available. </summary>
    ''' <value> The Standard Event available bits. </value>
    Public Overridable ReadOnly Property StandardEventAvailableBits() As VI.Pith.ServiceRequests

    ''' <summary> Gets a value indicating whether [StandardEvent available]. </summary>
    ''' <value> <c>True</c> if [StandardEvent available]; otherwise, <c>False</c>. </value>
    Public ReadOnly Property StandardEventAvailable As Boolean
        Get
            Return Me.Session.StandardEventAvailable
        End Get
    End Property

#End Region

#Region " SERVICE REQUEST REGISTER EVENTS: REPORT "

    ''' <summary> Returns a detailed report of the service request register (SRQ) byte. </summary>
    ''' <param name="value">     Specifies the value that was read from the service request register. </param>
    ''' <param name="delimiter"> The delimiter. </param>
    ''' <returns> The structured report. </returns>
    Public Shared Function BuildReport(ByVal value As VI.Pith.ServiceRequests, ByVal delimiter As String) As String

        If String.IsNullOrWhiteSpace(delimiter) Then
            delimiter = "; "
        End If

        Dim builder As New System.Text.StringBuilder

        For Each element As VI.Pith.ServiceRequests In [Enum].GetValues(GetType(VI.Pith.ServiceRequests))
            If element <> VI.Pith.ServiceRequests.None AndAlso element <> VI.Pith.ServiceRequests.All AndAlso (element And value) <> 0 Then
                If builder.Length > 0 Then
                    builder.Append(delimiter)
                End If
                builder.Append(element.Description)
            End If
        Next

        If builder.Length > 0 Then
            builder.Append(".")
            builder.Insert(0, String.Format(Globalization.CultureInfo.CurrentCulture,
                                            "The device service request register reported: 0x{0:X2}{1}", value, Environment.NewLine))
        End If
        Return builder.ToString

    End Function

#End Region

#Region " SERVICE REQUEST REGISTER "

    ''' <summary> Gets the standard service enable command format. </summary>
    ''' <remarks> SCPI: "*CLS; *ESE {0:D}; *SRE {1:D}". 
    ''' <see cref="VI.Pith.Ieee488.Syntax.StandardServiceEnableCommandFormat"> </see> </remarks>
    ''' <value> The standard service enable command format. </value>
    Protected Overridable ReadOnly Property StandardServiceEnableCommandFormat As String

    ''' <summary> Program the device to issue an SRQ upon any of the SCPI events. Uses *ESE to select
    ''' (mask) the events that will issue SRQ and  *SRE to select (mask) the event registers to be
    ''' included in the bits that will issue an SRQ. </summary>
    ''' <param name="standardEventEnableBitmask">  Specifies standard events will issue an SRQ. </param>
    ''' <param name="serviceRequestEnableBitmask"> Specifies which status registers will issue an
    ''' SRQ. </param>
    Public Sub EnableServiceRequest(ByVal standardEventEnableBitmask As VI.Pith.StandardEvents,
                                    ByVal serviceRequestEnableBitmask As VI.Pith.ServiceRequests)
        Me.Session.ReadServiceRequestStatus()
        Me.ServiceRequestEnableBitmask = serviceRequestEnableBitmask
        Me.StandardEventEnableBitmask = standardEventEnableBitmask
        If Not String.IsNullOrWhiteSpace(Me.StandardServiceEnableCommandFormat) Then
            Me.Session.WriteLine(Me.StandardServiceEnableCommandFormat, CInt(standardEventEnableBitmask), CInt(serviceRequestEnableBitmask))
        End If
    End Sub

    ''' <summary> Gets or sets the standard service enable and complete command format. </summary>
    ''' <remarks> SCPI: "*CLS; *ESE {0:D}; *SRE {1:D}; *OPC".
    ''' <see cref="VI.Pith.Ieee488.Syntax.StandardServiceEnableCompleteCommandFormat"> </see> </remarks>
    ''' <value> The standard service enable command and complete format. </value>
    Protected Overridable ReadOnly Property StandardServiceEnableCompleteCommandFormat As String

    ''' <summary> Sets the device to issue an SRQ upon any of the SCPI events. Uses *ESE to select
    ''' (mask) the events that will issue SRQ and  *SRE to select (mask) the event registers to be
    ''' included in the bits that will issue an SRQ Uses a single line command to accomplish the
    ''' entire command so as to save time. Also issues *OPC. </summary>
    ''' <param name="standardEventEnableBitmask">  Specifies standard events will issue an SRQ. </param>
    ''' <param name="serviceRequestEnableBitmask"> Specifies which status registers will issue an SRQ. </param>
    Public Sub EnableServiceRequestComplete(ByVal standardEventEnableBitmask As VI.Pith.StandardEvents,
                                            ByVal serviceRequestEnableBitmask As VI.Pith.ServiceRequests)
        Me.Session.ReadServiceRequestStatus()
        Me.ServiceRequestEnableBitmask = serviceRequestEnableBitmask
        Me.StandardEventEnableBitmask = standardEventEnableBitmask
        If Not String.IsNullOrWhiteSpace(Me.StandardServiceEnableCompleteCommandFormat) Then
            Me.Session.WriteLine(Me.StandardServiceEnableCompleteCommandFormat, CInt(standardEventEnableBitmask), CInt(serviceRequestEnableBitmask))
        End If

    End Sub

    ''' <summary> Queries the service request enable bit mask. </summary>
    ''' <returns> <c>null</c> if value is not known; otherwise <see cref="VI.Pith.ServiceRequests">Service
    ''' Requests</see>. </returns>
    Public Function QueryServiceRequestEnableBitmask() As VI.Pith.ServiceRequests?
        Me.ServiceRequestEnableBitmask = CType(Me.Session.Query(0I, VI.Pith.Ieee488.Syntax.ServiceRequestEnableQueryCommand), VI.Pith.ServiceRequests?)
        Return Me.ServiceRequestEnableBitmask
    End Function

    ''' <summary> The service request enable bitmask. </summary>
    Private _ServiceRequestEnableBitmask As VI.Pith.ServiceRequests?

    ''' <summary> Gets or sets the cached service request enable bit mask. </summary>
    ''' <value> <c>null</c> if value is not known; otherwise <see cref="VI.Pith.ServiceRequests">Service
    ''' Requests</see>. </value>
    Public Property ServiceRequestEnableBitmask() As VI.Pith.ServiceRequests?
        Get
            Return Me._ServiceRequestEnableBitmask
        End Get
        Protected Set(ByVal value As VI.Pith.ServiceRequests?)
            If Not Me.ServiceRequestEnableBitmask.Equals(value) Then
                Me._ServiceRequestEnableBitmask = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the service request enable command format. </summary>
    ''' <remarks> SCPI: "*SRE {0:D}". 
    ''' <see cref="VI.Pith.Ieee488.Syntax.ServiceRequestEnableCommandFormat"> </see> </remarks>
    ''' <value> The service request enable command format. </value>
    Protected Overridable ReadOnly Property ServiceRequestEnableCommandFormat As String

    ''' <summary> Program the device to issue an SRQ upon any of the SCPI events. Uses *SRE to select
    ''' (mask) the event registers to be included in the bits that will issue an SRQ. </summary>
    ''' <param name="serviceRequestEnableBitmask"> Specifies which status registers will issue an
    ''' SRQ. </param>
    Public Sub EnableServiceRequest(ByVal serviceRequestEnableBitmask As VI.Pith.ServiceRequests)
        Me.ServiceRequestEnableBitmask = serviceRequestEnableBitmask
        If Not String.IsNullOrWhiteSpace(Me.ServiceRequestEnableCommandFormat) Then
            Me.Session.ApplyServiceRequestEnableBitmask(Me.ServiceRequestEnableCommandFormat, Me.ServiceRequestEnableBitmask.GetValueOrDefault(0))
        End If
    End Sub

    ''' <summary> Gets or sets the cached service request Status. </summary>
    ''' <value> <c>null</c> if value is not known; otherwise <see cref="VI.Pith.ServiceRequests">Service
    ''' Requests</see>. </value>
    Public ReadOnly Property ServiceRequestStatus() As VI.Pith.ServiceRequests
        Get
            Return Me.Session.ServiceRequestStatus
        End Get
    End Property

    ''' <summary> Reads the service request Status. This method casts the
    ''' <see cref="VI.Pith.SessionBase.ReadStatusByte">Read Status Byte</see> to
    ''' <see cref="VI.Pith.ServiceRequests">Service Requests</see>. </summary>
    ''' <returns> <c>null</c> if value is not known; otherwise <see cref="VI.Pith.ServiceRequests">Service
    ''' Requests</see>. </returns>
    Public Function ReadServiceRequestStatus() As VI.Pith.ServiceRequests
        Me.Session.ReadServiceRequestStatus()
        Return Me.ServiceRequestStatus
    End Function

#End Region

#Region " STANDARD EVENT REGISTER "

    ''' <summary> Gets or sets bits that would be set for detecting if an Standard Device Error is available. </summary>
    ''' <value> The Standard Device Error available bits. </value>
    Public Property StandardDeviceErrorAvailableBits() As VI.Pith.StandardEvents

    ''' <summary> <c>True</c> if StandardDeviceError available. </summary>
    Private _StandardDeviceErrorAvailable As Boolean

    ''' <summary> Gets or sets a value indicating whether a Standard Device Error is available. </summary>
    ''' <value> <c>True</c> if [StandardDeviceError available]; otherwise, <c>False</c>. </value>
    Public Property StandardDeviceErrorAvailable As Boolean
        Get
            Return Me._StandardDeviceErrorAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            Me._StandardDeviceErrorAvailable = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property

    ''' <summary> Returns a detailed report of the event status register (ESR) byte. </summary>
    ''' <param name="value">     Specifies the value that was read from the status register. </param>
    ''' <param name="delimiter"> The delimiter. </param>
    ''' <returns> Returns a detailed report of the event status register (ESR) byte. </returns>
    Public Shared Function BuildReport(ByVal value As VI.Pith.StandardEvents, ByVal delimiter As String) As String
        If String.IsNullOrWhiteSpace(delimiter) Then delimiter = ";"
        Dim builder As New System.Text.StringBuilder
        For Each eventValue As VI.Pith.StandardEvents In [Enum].GetValues(GetType(VI.Pith.StandardEvents))
            If eventValue <> VI.Pith.StandardEvents.None AndAlso eventValue <> VI.Pith.StandardEvents.All AndAlso (eventValue And value) <> 0 Then
                If builder.Length > 0 Then
                    builder.Append(delimiter)
                End If
                builder.Append(eventValue.Description)
            End If
        Next
        Return ($"The device standard status register reported: 0x{CInt(value):X2}{Environment.NewLine}{builder.ToString}.")
    End Function

    ''' <summary> The standard event status. </summary>
    Private _StandardEventStatus As VI.Pith.StandardEvents?

    ''' <summary> Gets or sets the cached Standard Event enable bit mask. </summary>
    ''' <value> <c>null</c> if value is not known; otherwise <see cref="VI.Pith.StandardEvents">Standard
    ''' Events</see>. </value>
    Public Property StandardEventStatus() As VI.Pith.StandardEvents?
        Get
            Return Me._StandardEventStatus
        End Get
        Set(ByVal value As VI.Pith.StandardEvents?)
            Me._StandardEventStatus = value
            If value.HasValue Then
                Me.StandardDeviceErrorAvailable = (value.Value And Me.StandardDeviceErrorAvailableBits) <> 0
            Else
                Me.StandardDeviceErrorAvailable = False
            End If
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Gets the standard event status query command. </summary>
    ''' <remarks> SCPI: "*ESR?".
    ''' <see cref="VI.Pith.Ieee488.Syntax.StandardEventQueryCommand"> </see> </remarks>
    ''' <value> The standard event status query command. </value>
    Protected Overridable ReadOnly Property StandardEventStatusQueryCommand As String

    ''' <summary> Queries the Standard Event enable bit mask. </summary>
    ''' <returns> <c>null</c> if value is not known; otherwise <see cref="VI.Pith.StandardEvents">Standard
    ''' Events</see>. </returns>
    Public Function QueryStandardEventStatus() As VI.Pith.StandardEvents?
        If Not String.IsNullOrWhiteSpace(Me.StandardEventStatusQueryCommand) Then
            Me.StandardEventStatus = CType(Me.Session.Query(0I, Me.StandardEventStatusQueryCommand), VI.Pith.StandardEvents?)
        End If
        Return Me.StandardEventStatus
    End Function

    ''' <summary> The standard event enable bitmask. </summary>
    Private _StandardEventEnableBitmask As VI.Pith.StandardEvents?

    ''' <summary> Gets or sets the cached Standard Event enable bit mask. </summary>
    ''' <value> <c>null</c> if value is not known; otherwise <see cref="VI.Pith.StandardEvents">Standard
    ''' Events</see>. </value>
    Public Property StandardEventEnableBitmask() As VI.Pith.StandardEvents?
        Get
            Return Me._StandardEventEnableBitmask
        End Get
        Set(ByVal value As VI.Pith.StandardEvents?)
            If Not Me.StandardEventEnableBitmask.Equals(value) Then
                Me._StandardEventEnableBitmask = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the standard event enable query command. </summary>
    ''' <remarks> SCPI: "*ESE?".
    ''' <see cref="VI.Pith.Ieee488.Syntax.StandardEventEnableQueryCommand"> </see> </remarks>
    ''' <value> The standard event enable query command. </value>
    Protected Overridable ReadOnly Property StandardEventEnableQueryCommand As String

    ''' <summary> Queries the Standard Event enable bit mask. </summary>
    ''' <returns> <c>null</c> if value is not known; otherwise <see cref="VI.Pith.StandardEvents">Standard
    ''' Events</see>. </returns>
    Public Function QueryStandardEventEnableBitmask() As VI.Pith.StandardEvents?
        If Not String.IsNullOrWhiteSpace(Me.StandardEventEnableQueryCommand) Then
            Me.StandardEventEnableBitmask = CType(Me.Session.Query(0I, Me.StandardEventEnableQueryCommand), VI.Pith.StandardEvents?)
        End If
        Return Me.StandardEventEnableBitmask
    End Function

#End Region

#Region " CHECK AND THROW "

    ''' <summary> Checks and throws an exception if device errors occurred. Can only be used after
    ''' receiving a full reply from the device. </summary>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="flushReadFirst"> Flushes the read buffer before processing the error. </param>
    ''' <param name="format">         Describes the format to use. </param>
    ''' <param name="args">           A variable-length parameters list containing arguments. </param>
    Public Sub CheckThrowDeviceException(ByVal flushReadFirst As Boolean, ByVal format As String, ByVal ParamArray args() As Object)
        If (Me.ReadServiceRequestStatus And Me.ErrorAvailableBits) <> 0 Then
            If flushReadFirst Then
                Me.Session.DiscardUnreadData()
            End If
            Me.QueryStandardEventStatus()
            Me.QueryDeviceErrors()
            Dim details As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
            Throw New isr.VI.Pith.DeviceException(Me.ResourceNameCaption, $"{details}. {Me.DeviceErrorsReport}.")
        End If
    End Sub

#End Region

#Region " CHECK AND TRACE "

    ''' <summary> Check and reports visa or device error occurred. Can only be used after receiving a
    ''' full reply from the device. </summary>
    ''' <param name="nodeNumber"> Specifies the remote node number to validate. </param>
    ''' <param name="format">     Specifies the report format. </param>
    ''' <param name="args">       Specifies the report arguments. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Overridable Function TraceVisaDeviceOperationOkay(ByVal nodeNumber As Integer,
                                                             ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return True
    End Function

    ''' <summary> Checks and reports if a visa or device error occurred. Can only be used after
    ''' receiving a full reply from the device. </summary>
    ''' <param name="nodeNumber">     Specifies the remote node number to validate. </param>
    ''' <param name="flushReadFirst"> Flushes the read buffer before processing the error. </param>
    ''' <param name="format">         Describes the format to use. </param>
    ''' <param name="args">           A variable-length parameters list containing arguments. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function TraceVisaDeviceOperationOkay(ByVal nodeNumber As Integer,
                                                 ByVal flushReadFirst As Boolean, ByVal format As String,
                                                 ByVal ParamArray args() As Object) As Boolean
        Dim success As Boolean
        Me.TraceVisaOperation(nodeNumber, format, args)
        Me.ReadServiceRequestStatus()
        success = Not Me.ErrorAvailable
        Dim details As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
        If success Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                              $"{Me.ResourceNameCaption} node {nodeNumber} done {details}")
        Else
            If flushReadFirst Then Me.Session.DiscardUnreadData()
            Me.QueryStandardEventStatus()
            Me.QueryDeviceErrors()
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              $"{Me.ResourceNameCaption} node {nodeNumber} device errors: {Me.DeviceErrorsReport};. Details: 
{details}
{New StackFrame(True).UserCallStack}")
        End If
        Return success
    End Function

    ''' <summary> Checks and reports if a visa or device error occurred.
    ''' Can only be used after receiving a full reply from the device. </summary>
    ''' <param name="flushReadFirst"> Flushes the read buffer before processing the error. </param>
    ''' <param name="format">         Describes the format to use. </param>
    ''' <param name="args">           A variable-length parameters list containing arguments. </param>
    ''' <returns><c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function TraceVisaDeviceOperationOkay(ByVal flushReadFirst As Boolean, ByVal format As String,
                                                 ByVal ParamArray args() As Object) As Boolean
        Me.TraceVisaOperation(format, args)
        Me.ReadServiceRequestStatus()
        Dim success As Boolean = Not Me.ErrorAvailable
        Dim details As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
        If success Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} done {details}")
        Else
            If flushReadFirst Then Me.Session.DiscardUnreadData()
            Me.QueryStandardEventStatus()
            Me.QueryDeviceErrors()
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              $"{Me.ResourceNameCaption} device errors: {Me.DeviceErrorsReport};. Details: 
{details}
{New StackFrame(True).UserCallStack}")
        End If
        Return success
    End Function

    ''' <summary> Reports operation action. Can be used with queries. </summary>
    ''' <param name="format">   Describes the format to use. </param>
    ''' <param name="args">     A variable-length parameters list containing arguments. </param>
    Public Sub TraceVisaOperation(ByVal format As String, ByVal ParamArray args() As Object)
        Dim details As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} done {details}")
    End Sub

    ''' <summary> Reports if a visa error occurred.
    '''           Can be used with queries. </summary>
    ''' <param name="format">   Describes the format to use. </param>
    ''' <param name="args">     A variable-length parameters list containing arguments. </param>
    Public Sub TraceVisaOperation(ByVal ex As VI.Pith.NativeException, ByVal format As String, ByVal ParamArray args() As Object)
        If ex Is Nothing Then Throw New ArgumentNullException(NameOf(ex))
        Dim details As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
        Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                          $"{Me.ResourceNameCaption} VISA error: {ex.InnerError.BuildErrorCodeDetails()};. Details: 
{details}
{New StackFrame(True).UserCallStack}")
    End Sub

    ''' <summary> Reports if a visa error occurred.
    '''           Can be used with queries. </summary>
    ''' <param name="format">   Describes the format to use. </param>
    ''' <param name="args">     A variable-length parameters list containing arguments. </param>
    Public Sub TraceOperation(ByVal ex As Exception, ByVal format As String, ByVal ParamArray args() As Object)
        If ex Is Nothing Then Throw New ArgumentNullException(NameOf(ex))
        Dim details As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
        Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                          $"{Me.ResourceNameCaption} exception: {ex.Message};. Details: 
{details}
{New StackFrame(True).UserCallStack}")
    End Sub

    ''' <summary> Trace visa node operation. Can be used with queries. </summary>
    ''' <param name="nodeNumber"> Specifies the remote node number to validate. </param>
    ''' <param name="format">     Describes the format to use. </param>
    ''' <param name="args">       A variable-length parameters list containing arguments. </param>
    Public Sub TraceVisaOperation(ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object)
        Dim details As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                              $"{Me.ResourceNameCaption} node {nodeNumber} done {details}")
    End Sub

    ''' <summary> Trace visa failed operation. Can be used with queries. </summary>
    ''' <param name="nodeNumber">   Specifies the remote node number to validate. </param>
    ''' <param name="format"> Describes the format to use. </param>
    ''' <param name="args">   A variable-length parameters list containing arguments. </param>
    Public Sub TraceVisaOperation(ByVal ex As VI.Pith.NativeException, ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object)
        If ex Is Nothing Then Throw New ArgumentNullException(NameOf(ex))
        Dim details As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
        Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                          $"{Me.ResourceNameCaption} node {nodeNumber} VISA error: {ex.InnerError.BuildErrorCodeDetails()};. Details: 
{details}
{New StackFrame(True).UserCallStack}")
    End Sub

    ''' <summary> Trace failed operation. Can be used with queries. </summary>
    ''' <param name="nodeNumber">   Specifies the remote node number to validate. </param>
    ''' <param name="format"> Describes the format to use. </param>
    ''' <param name="args">   A variable-length parameters list containing arguments. </param>
    Public Sub TraceOperation(ByVal ex As Exception, ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object)
        If ex Is Nothing Then Throw New ArgumentNullException(NameOf(ex))
        Dim details As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
        Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                          $"{Me.ResourceNameCaption} node {nodeNumber} exception: {ex.Message};. Details: 
{details}
{New StackFrame(True).UserCallStack}")
    End Sub

#End Region

#Region " COLLECT GARBAGE "

    ''' <summary> Gets or sets the collect garbage wait complete command. </summary>
    ''' <value> The collect garbage wait complete command. </value>
    Protected Overridable ReadOnly Property CollectGarbageWaitCompleteCommand As String = ""

    ''' <summary> Collect garbage wait complete. </summary>
    ''' <param name="timeout"> Specifies how long to wait for the service request before throwing
    '''                        the timeout exception. Set to zero for an infinite (120 seconds)
    '''                        timeout. </param>
    ''' <param name="format">  Specifies the report format. </param>
    ''' <param name="args">    Specifies the report arguments. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function CollectGarbageWaitComplete(ByVal timeout As TimeSpan, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        If Not String.IsNullOrWhiteSpace(Me.CollectGarbageWaitCompleteCommand) Then
            Return Me._CollectGarbageWaitComplete(timeout, format, args)
        Else
            Return True
        End If
    End Function

    ''' <summary> Does garbage collection. Reports operations synopsis. </summary>
    ''' <param name="timeout"> Specifies the time to wait for the instrument to return operation
    ''' completed. </param>
    ''' <param name="format">  Describes the format to use. </param>
    ''' <param name="args">    A variable-length parameters list containing arguments. </param>
    ''' <returns><c>True</c> if okay; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Function _CollectGarbageWaitComplete(ByVal timeout As TimeSpan, ByVal format As String, ByVal ParamArray args() As Object) As Boolean

        Dim affirmative As Boolean = True
        Try
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                       "Collecting garbage after {0};. ",
                                                       String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
            Me.Session.LastNodeNumber = New Integer?
            ' do a garbage collection
            Me.EnableWaitComplete()
            Me.Write(Me.CollectGarbageWaitCompleteCommand)
            affirmative = Me.TraceVisaDeviceOperationOkay(True, "collecting garbage after {0};. ",
                                                          String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
        Catch ex As VI.Pith.NativeException
            Me.TraceVisaOperation(ex, "collecting garbage after {0};. ",
                                  String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
            affirmative = False
        Catch ex As Exception
            Me.TraceOperation(ex, "collecting garbage after {0};. ",
                              String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
            affirmative = False
        End Try
        If affirmative Then
            Try
                Me.AwaitOperationCompleted(timeout)
                affirmative = Me.TraceVisaDeviceOperationOkay(True, "awaiting completion after collecting garbage after {0};. ",
                                                              String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
            Catch ex As VI.Pith.NativeException
                Me.TraceVisaOperation(ex, "awaiting completion after collecting garbage after {0};. ",
                                      String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
                affirmative = False
            Catch ex As Exception
                Me.TraceOperation(ex, "awaiting completion after collecting garbage after {0};. ",
                                  String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
                affirmative = False
            End Try
        End If
        Return affirmative

    End Function

#End Region

#Region " LINE FREQUENCY "

    ''' <summary> Gets or sets the station line frequency. </summary>
    ''' <value> The station line frequency. </value>
    ''' <remarks>This value is shared and used for all systems on this station. </remarks>
    Public Shared Property StationLineFrequency As Double? = New Double?

    ''' <summary> The line frequency. </summary>
    Private _LineFrequency As Double?

    ''' <summary> Gets or sets the line frequency. </summary>
    ''' <value> The line frequency. </value>
    Public Property LineFrequency() As Double?
        Get
            Return Me._lineFrequency
        End Get
        Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.LineFrequency, value) Then
                StatusSubsystemBase.StationLineFrequency = value
                Me._lineFrequency = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the default line frequency. </summary>
    ''' <value> The default line frequency. </value>
    Public Shared Property DefaultLineFrequency As Double = 60

    ''' <summary> Gets line frequency query command. </summary>
    ''' <value> The line frequency query command. </value>
    Protected Overridable ReadOnly Property LineFrequencyQueryCommand As String

    ''' <summary> Reads the line frequency. </summary>
    ''' <returns> System.Nullable{System.Double}. </returns>
    ''' <remarks> Sends the <see cref="LineFrequencyQueryCommand"/> query. </remarks>
    Public Function QueryLineFrequency() As Double?
        If Not Me.LineFrequency.HasValue Then
            If String.IsNullOrWhiteSpace(Me.LineFrequencyQueryCommand) Then
                Me.LineFrequency = StatusSubsystemBase.DefaultLineFrequency
            Else
                Me.LineFrequency = Me.Session.Query(Me.LineFrequency.GetValueOrDefault(StatusSubsystemBase.DefaultLineFrequency), Me.LineFrequencyQueryCommand)
            End If
        End If
        Return Me.LineFrequency
    End Function

    ''' <summary> Converts power line cycles to time span. </summary>
    ''' <param name="powerLineCycles"> The power line cycles. </param>
    ''' <returns> The integration period corresponding to the specified number of power line . </returns>
    Public Shared Function FromPowerLineCycles(ByVal powerLineCycles As Double) As TimeSpan
        Return StatusSubsystemBase.FromPowerLineCycles(powerLineCycles, StatusSubsystemBase.StationLineFrequency.GetValueOrDefault(StatusSubsystemBase.DefaultLineFrequency))
    End Function

    ''' <summary> Converts integration period to Power line cycles. </summary>
    ''' <param name="integrationPeriod"> The integration period. </param>
    ''' <returns> The number of power line cycles corresponding to the integration period. </returns>
    Public Shared Function ToPowerLineCycles(ByVal integrationPeriod As TimeSpan) As Double
        Return StatusSubsystemBase.ToPowerLineCycles(integrationPeriod, StatusSubsystemBase.StationLineFrequency.GetValueOrDefault(StatusSubsystemBase.DefaultLineFrequency))
    End Function

    ''' <summary> Converts power line cycles to time span. </summary>
    ''' <param name="powerLineCycles"> The power line cycles. </param>
    ''' <param name="frequency">       The frequency. </param>
    ''' <returns>
    ''' The integration period corresponding to the specified number of power line .
    ''' </returns>
    Public Shared Function FromPowerLineCycles(ByVal powerLineCycles As Double, ByVal frequency As Double) As TimeSpan
        Return StatusSubsystemBase.FromSecondsPrecise(powerLineCycles / frequency)
    End Function

    ''' <summary> Converts integration period to Power line cycles. </summary>
    ''' <param name="integrationPeriod"> The integration period. </param>
    ''' <param name="frequency">         The frequency. </param>
    ''' <returns> The number of power line cycles corresponding to the integration period. </returns>
    Public Shared Function ToPowerLineCycles(ByVal integrationPeriod As TimeSpan, ByVal frequency As Double) As Double
        Return StatusSubsystemBase.TotalSecondsPrecise(integrationPeriod) * frequency
    End Function

    ''' <summary> Total seconds precise. </summary>
    ''' <param name="timespan"> The timespan. </param>
    ''' <returns> The total number of seconds precise. </returns>
    Public Shared Function TotalSecondsPrecise(ByVal timespan As TimeSpan) As Double
        Return timespan.Ticks / TimeSpan.TicksPerSecond
    End Function

    ''' <summary> Converts seconds to time span with tick timespan accuracy. </summary>
    ''' <param name="seconds"> The seconds. </param>
    ''' <returns> A TimeSpan. </returns>
    Public Shared Function FromSecondsPrecise(ByVal seconds As Double) As TimeSpan
        Return TimeSpan.FromTicks(CLng(TimeSpan.TicksPerSecond * seconds))
    End Function

#End Region

End Class

