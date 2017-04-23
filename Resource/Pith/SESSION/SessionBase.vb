Imports System.Timers
Imports isr.Core.Pith.EventHandlerExtensions
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Base class for SessionBase. </summary>
''' <remarks> David, 11/21/2015. </remarks>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
Public MustInherit Class SessionBase
    Inherits isr.Core.Pith.PropertyNotifyBase
    Implements IDisposable

#Region " CONSTRUCTORS "

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <remarks> David, 11/25/2015. </remarks>
    Protected Sub New()
        MyBase.New()
        Me._ResourceInfo = New ResourceParseResult
        Me._Enabled = True
        Me._ResourceName = ""
        Me._ResourceTitle = ""
        Me._LastMessageReceived = ""
        Me._LastMessageSent = ""
        Me._UseDefaultTermination()
        Me._InitializeServiceRequestRegisterBits()
        Me._Timeouts = New Collections.Generic.Stack(Of TimeSpan)
        Me._MinimumTimeout = TimeSpan.FromMilliseconds(3000)
        Me._Timeout = TimeSpan.FromMilliseconds(10000)
        ' must be less than half the communication timeout interval.
        Me._KeepAliveInterval = TimeSpan.FromSeconds(58)
    End Sub

#Region " Disposable Support "

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed Then
                If disposing Then
                    If Me._KeepAliveTimer IsNot Nothing Then
                        Me._KeepAliveTimer.Enabled = False
                        Me._KeepAliveTimer.Dispose()
                        Me._KeepAliveTimer = Nothing
                    End If
                    Me.RemoveEventHandler(Me.ServiceRequestedEvent)
                    Me.Timeouts?.Clear() : Me._Timeouts = Nothing
                Else
                    ' put finalize code here
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " SESSION "

    ''' <summary>
    ''' Gets or sets the Enabled sentinel. When enabled, the session is allowed to engage actual
    ''' hardware; otherwise, opening the session does not attempt to link to the hardware.
    ''' </summary>
    ''' <value> The enabled sentinel. </value>
    Public Property Enabled As Boolean

    ''' <summary> Gets or sets the default open timeout. </summary>
    ''' <value> The default open timeout. </value>
    Public Property DefaultOpenTimeout As TimeSpan = TimeSpan.FromSeconds(3)

    ''' <summary>
    ''' Gets or sets the session open sentinel. When open, the session is capable of addressing the
    ''' hardware. See also <see cref="IsDeviceOpen"/>.
    ''' </summary>
    ''' <value> The is session open. </value>
    Public MustOverride ReadOnly Property IsSessionOpen As Boolean

    ''' <summary>
    ''' Gets or sets the Device Open sentinel. When open, the device is capable of addressing real
    ''' hardware if the session is open. See also <see cref="IsSessionOpen"/>.
    ''' </summary>
    ''' <value> The is device open. </value>
    Public ReadOnly Property IsDeviceOpen As Boolean

    ''' <summary> Gets the sentinel indicating weather this is a dummy session. </summary>
    ''' <value> The dummy sentinel. </value>
    Public MustOverride ReadOnly Property IsDummy As Boolean


    ''' <summary> Executes the opening session action. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    ''' <param name="resourceName">  The name of the resource. </param>
    ''' <param name="resourceTitle"> The short title of the device. </param>
    Private Sub OnOpeningSession(ByVal resourceName As String, ByVal resourceTitle As String)
        Me._ResourceName = resourceName
        Me._ResourceTitle = resourceTitle
        Me.ResourceInfo.Parse(resourceName)
        Me._LastAction = $"Opening {resourceName};. "
        Me._Timeouts.Clear()
    End Sub

    ''' <summary> Executes the session open action. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Private Sub OnSessionOpen()
        If Me.ResourceInfo.InterfaceType = HardwareInterfaceType.Tcpip AndAlso Me.KeepAliveInterval > TimeSpan.Zero Then
            Me._KeepAliveTimer = New Timers.Timer With {
                .Interval = Me.KeepAliveInterval.TotalMilliseconds,
                .Enabled = True
            }
        End If
        Me._IsDeviceOpen = True
        Me.SafePostPropertyChanged(NameOf(Me.IsDeviceOpen))
        Me.SafePostPropertyChanged(NameOf(Me.IsSessionOpen))
        Me.SafePostPropertyChanged(NameOf(Me.ResourceName))
        Me.SafePostPropertyChanged(NameOf(Me.ResourceTitle))
        Me._Timeouts.Push(Me.Timeout)
    End Sub

    ''' <summary> Creates a session. </summary>
    ''' <remarks> Throws an exception if the resource is not accessible. </remarks>
    ''' <param name="resourceName"> The name of the resource. </param>
    ''' <param name="timeout">      The timeout. </param>
    Protected MustOverride Sub CreateSession(ByVal resourceName As String, ByVal timeout As TimeSpan)

    ''' <summary> Opens a <see cref="SessionBase">Session</see>. </summary>
    ''' <remarks> Call this first. </remarks>
    Public Sub OpenSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal timeout As TimeSpan, ByVal syncContext As Threading.SynchronizationContext)
        Try
            Me.CaptureSyncContext(syncContext)
            Me.OnOpeningSession(resourceName, resourceTitle)
            Me.CreateSession(resourceName, timeout)
            Me.OnSessionOpen()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary> Opens a <see cref="SessionBase">Session</see>. </summary>
    ''' <remarks> Call this first. </remarks>
    ''' <param name="resourceName">  The name of the resource. </param>
    ''' <param name="resourceTitle"> The short title of the device. </param>
    Public Sub OpenSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal syncContext As Threading.SynchronizationContext)
        Me.OpenSession(resourceName, resourceTitle, Me.DefaultOpenTimeout, syncContext)
    End Sub

    ''' <summary> Opens a <see cref="SessionBase">Session</see>. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    ''' <param name="resourceName"> The name of the resource. </param>
    Public Sub OpenSession(ByVal resourceName As String, ByVal syncContext As Threading.SynchronizationContext)
        Me.OpenSession(resourceName, resourceName, Me.DefaultOpenTimeout, syncContext)
    End Sub

    ''' <summary> Opens a <see cref="SessionBase">Session</see>. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    ''' <param name="timeout"> The timeout. </param>
    Public Sub OpenSession(ByVal resourceName As String, ByVal timeout As TimeSpan, ByVal syncContext As Threading.SynchronizationContext)
        Me.OpenSession(resourceName, resourceName, timeout, syncContext)
    End Sub

    ''' <summary> Discards the session. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    Protected MustOverride Sub DiscardSession()

    ''' <summary> Closes the <see cref="SessionBase">Session</see>. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    Public Sub CloseSession()
        If Me._KeepAliveTimer IsNot Nothing Then
            Me._KeepAliveTimer.Enabled = False
            Me._KeepAliveTimer.Dispose()
            Me._KeepAliveTimer = Nothing
        End If
        Me._Timeouts = New Collections.Generic.Stack(Of TimeSpan)
        If Me.IsDeviceOpen Then
            Me.DiscardSession()
            Me._IsDeviceOpen = False
            Me.SafePostPropertyChanged(NameOf(Me.IsDeviceOpen))
            Me.SafePostPropertyChanged(NameOf(Me.IsSessionOpen))
        End If
    End Sub

    ''' <summary> Searches for a listeners for the specified <see cref="ResourceName">reasource name</see>. </summary>
    ''' <remarks> David, 11/27/2015. Updates <see cref="ResourceFound">ResourceExists</see></remarks>
    ''' <returns> <c>true</c> if it the resource exists; otherwise <c>false</c> </returns>
    Public MustOverride Function FindResource() As Boolean

    Private _ResourceFound As Boolean
    ''' <summary> Gets the sentinel indicating if the session resource exists. </summary>
    ''' <value> The sentinel indicating if the session resource exists. </value>
    ''' <remarks> Use <see cref="FindResource"/> to update. </remarks>
    Public Property ResourceFound As Boolean
        Get
            Return Me._ResourceFound
        End Get
        Protected Set(value As Boolean)
            If Me.ResourceFound <> value Then
                Me._ResourceFound = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets the last action. </summary>
    ''' <value> The last action. </value>
    Public Property LastAction As String

    ''' <summary> Gets or sets the last node. </summary>
    ''' <value> The last node. </value>
    Public Property LastNodeNumber As Integer?

    ''' <summary> Gets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public ReadOnly Property ResourceName As String

    Private _ResourceTitle As String
    ''' <summary> Gets or sets a short title for the device. </summary>
    ''' <value> The short title of the device. </value>
    Public Property ResourceTitle As String
        Get
            Return Me._ResourceTitle
        End Get
        Set(value As String)
            If Not String.Equals(Me.ResourceTitle, value) Then
                Me._ResourceTitle = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the last native error. </summary>
    ''' <value> The last native error. </value>
    Protected MustOverride ReadOnly Property LastNativeError As NativeErrorBase

    ''' <summary>
    ''' Gets or sets the sentinel indicating if call backs are performed in a specific
    ''' synchronization context.
    ''' </summary>
    ''' <value>
    ''' The sentinel indicating if call backs are performed in a specific synchronization context.
    ''' </value>
    Public Overridable Property SynchronizeCallbacks As Boolean

    ''' <summary> Gets or sets information describing the resource. </summary>
    ''' <value> Information describing the resource. </value>
    Public ReadOnly Property ResourceInfo As ResourceParseResult

#Region " MESSAGE EVENTS "

    Private _SyncNotifyLastMessageReceivedEnabled As Boolean

    ''' <summary> Gets or sets the synchronous last message received enabled sentinel. </summary>
    ''' <remarks> Enable synchronous reporting of last received when messages are needed to be parsed
    ''' to determine the instrument state such as with TSP. This ensures that the parser works on the
    ''' relevant message that was received from the instrument. </remarks>
    ''' <value> The synchronous last message received enabled. </value>
    Public Property SyncNotifyLastMessageReceivedEnabled As Boolean
        Get
            Return Me._SyncNotifyLastMessageReceivedEnabled
        End Get
        Set(value As Boolean)
            If value <> Me.SyncNotifyLastMessageReceivedEnabled Then
                Me._SyncNotifyLastMessageReceivedEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _SessionMessagesTraceEnabled As Boolean

    ''' <summary> Gets or sets the session messages trace enabled. </summary>
    ''' <value> The session messages trace enabled. </value>
    Public Property SessionMessagesTraceEnabled As Boolean
        Get
            Return Me._SessionMessagesTraceEnabled
        End Get
        Set(value As Boolean)
            If Not value.Equals(Me.SessionMessagesTraceEnabled) Then
                Me._SessionMessagesTraceEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _LastMessageReceived As String
    ''' <summary> Gets or sets the last message Received. </summary>
    ''' <remarks> The last message sent is posted asynchronously. This may not be processed fast enough
    ''' with TSP devices to determine the state of the instrument. </remarks>
    ''' <value> The last message Received. </value>
    Public Property LastMessageReceived As String
        Get
            Return Me._LastMessageReceived
        End Get
        Protected Set(ByVal value As String)
            Me._LastMessageReceived = value
            If Me.SessionMessagesTraceEnabled Then
                If Me.SyncNotifyLastMessageReceivedEnabled Then
                    Me.SafeSendPropertyChanged()
                Else
                    Me.SafePostPropertyChanged()
                End If
            End If
        End Set
    End Property

    Private _LastMessageSent As String
    ''' <summary> Gets or sets the last message Sent. </summary>
    ''' <remarks> The last message sent is posted asynchronously. </remarks>
    ''' <value> The last message Sent. </value>
    Public Property LastMessageSent As String
        Get
            Return Me._LastMessageSent
        End Get
        Protected Set(ByVal value As String)
            Me._LastMessageSent = value
            If Me.SessionMessagesTraceEnabled Then
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " KEEP ALIVE "

    Private WithEvents _KeepAliveTimer As Timers.Timer

    ''' <summary> Keep alive timer elapsed. </summary>
    ''' <remarks> David, 3/14/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Elapsed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub KeepAliveTimerElapsed(sender As Object, e As ElapsedEventArgs) Handles _KeepAliveTimer.Elapsed
        Try
            Dim tmr As Timers.Timer = TryCast(sender, Timers.Timer)
            If tmr IsNot Nothing AndAlso tmr.Interval < DateTime.Now.Subtract(Me.LastInputOutputTime).TotalMilliseconds Then
                Me.KeepAlive()
                Me.LastInputOutputTime = DateTime.Now
            End If
        Catch
        Finally
        End Try
    End Sub

    ''' <summary> Gets or sets the keep alive interval. Must be smaller or equal to half the communication timeout interval. </summary>
    ''' <value> The keep alive interval. </value>
    Public Property KeepAliveInterval As TimeSpan

    ''' <summary> Query if this object is alive. </summary>
    ''' <remarks> David, 3/14/2016. </remarks>
    ''' <returns> <c>true</c> if alive; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function IsAlive() As Boolean
        Dim affirmative As Boolean = True
        If Me.IsSessionOpen AndAlso Me.ResourceInfo.UsingLanController Then
            Try
                If Not String.IsNullOrWhiteSpace(Me.IsAliveCommand) Then
                    Me.Write(Me.IsAliveCommand)
                ElseIf Not String.IsNullOrWhiteSpace(Me.IsAliveQueryCommand) Then
                    Me.Query(Me.IsAliveQueryCommand)
                End If
            Catch
                affirmative = False
            End Try
        End If
        Return affirmative
    End Function

    ''' <summary> Gets or sets the is alive command. </summary>
    ''' <value> The is alive command. </value>
    Public Property IsAliveCommand As String = "*OPC"

    ''' <summary> Gets or sets the is alive query command. </summary>
    ''' <value> The is alive query command. </value>
    Public Property IsAliveQueryCommand As String = "*OPC?"

    ''' <summary> Keep alive. </summary>
    ''' <remarks> David, 3/15/2016. </remarks>
    Public MustOverride Function KeepAlive() As Boolean

#End Region

#End Region

#Region " READ / WRITE "

    ''' <summary> Gets or sets the last input output time. </summary>
    ''' <value> The last input output time. </value>
    Public Property LastInputOutputTime As Date

    ''' <summary> Gets or sets the termination character. </summary>
    ''' <value> The termination character. </value>
    Public Overridable Property TerminationCharacter As Byte

    ''' <summary> Gets or sets the timeout. </summary>
    ''' <value> The timeout. </value>
    Public Overridable Property Timeout As TimeSpan

    ''' <summary> Gets or sets the emulated reply. </summary>
    ''' <value> The emulated reply. </value>
    Public Property EmulatedReply As String

    ''' <summary>
    ''' Synchronously reads ASCII-encoded string data. Reads up to the
    ''' <see cref="TerminationCharacter">termination character</see>. Limited by the
    ''' <see cref="InputBufferSize"/>. Will time out if end of line is not read before reading a
    ''' buffer.
    ''' </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <returns> The received message. </returns>
    Public MustOverride Function ReadFiniteLine() As String

    ''' <summary>
    ''' Synchronously reads ASCII-encoded string data until reaching the
    ''' <see cref="TerminationCharacter">termination character</see>. Not limited by the
    ''' <see cref="InputBufferSize"/>.
    ''' </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <returns> The received message. </returns>
    Public MustOverride Function ReadFreeLine() As String

    ''' <summary> Writes. </summary>
    ''' <param name="dataToWrite"> The data to write to write. </param>
    Public MustOverride Sub Write(ByVal dataToWrite As String)

    ''' <summary> Clears the device. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public MustOverride Sub Clear()

    ''' <summary> Reads status byte. </summary>
    ''' <returns> The status byte. </returns>
    Public MustOverride Function ReadStatusByte() As ServiceRequests

    ''' <summary> Gets or sets the size of the input buffer. </summary>
    ''' <value> The size of the input buffer. </value>
    Public MustOverride ReadOnly Property InputBufferSize() As Integer

#End Region

#Region " EMULATION "

    ''' <summary> Makes emulated reply. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeTrueFalseReply(ByVal value As Boolean)
        Me.EmulatedReply = SessionBase.ToTrueFalse(value)
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeTrueFalseReplyIfEmpty(ByVal value As Boolean)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeTrueFalseReply(value)
        End If
    End Sub

    ''' <summary> Makes emulated reply. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReply(ByVal value As Boolean)
        Me.EmulatedReply = SessionBase.ToOneZero(value)
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As Boolean)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

    ''' <summary> Makes emulated reply. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReply(ByVal value As Double)
        Me.EmulatedReply = CStr(value)
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As Double)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

    ''' <summary> Makes emulated reply. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReply(ByVal value As Integer)
        Me.EmulatedReply = CStr(value)
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As Integer)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

    ''' <summary> Makes emulated reply. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReply(ByVal value As TimeSpan)
        Me.EmulatedReply = value.ToString
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As TimeSpan)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

    ''' <summary> Makes emulated reply. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    Public Sub MakeEmulatedReply(ByVal value As String)
        Me.EmulatedReply = value
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As String)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

#End Region

#Region " SERVICE REQUEST EVENTS "

    ''' <summary> Gets or sets the emulated status byte. </summary>
    ''' <value> The emulated status byte. </value>
    Public Property EmulatedStatusByte As ServiceRequests

    ''' <summary> Makes emulated status byte. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReply(ByVal value As ServiceRequests)
        Me.EmulatedStatusByte = value
    End Sub

    ''' <summary> Makes emulated status byte if none. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As ServiceRequests)
        If Me.EmulatedStatusByte = ServiceRequests.None Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

    ''' <summary> Queries if a service request is enabled. </summary>
    ''' <remarks> David, 1/18/2017. </remarks>
    ''' <param name="bitmask"> The bit mask. </param>
    ''' <returns> <c>true</c> if a service request is enabled; otherwise <c>false</c> </returns>
    Public Function IsServiceRequestEnabled(ByVal bitmask As ServiceRequests) As Boolean
        Return Me.ServiceRequestEventEnabled AndAlso (Me.ServiceRequestEnableBitmask And bitmask) <> 0
    End Function

    ''' <summary> Gets or sets the service request enable bitmask. </summary>
    ''' <value> The service request enable bitmask. </value>
    Public ReadOnly Property ServiceRequestEnableBitmask As ServiceRequests

    ''' <summary>
    ''' Applies the service request bitmask. Disables service request if bitmask is 0.
    ''' </summary>
    ''' <remarks> David, 1/18/2017. </remarks>
    ''' <param name="commandFormat"> The service request enable command format. </param>
    ''' <param name="bitmask">       The bitmask. </param>
    Public Sub ApplyServiceRequestEnableBitmask(ByVal commandFormat As String, ByVal bitmask As ServiceRequests)
        Me._ServiceRequestEnableBitmask = bitmask
        Me.WriteLine(commandFormat, CInt(bitmask))
        Me.SafePostPropertyChanged(NameOf(ServiceRequestEnableBitmask))
    End Sub

    ''' <summary> Event queue for all listeners interested in ServiceRequested events. </summary>
    Public Event ServiceRequested As EventHandler(Of EventArgs)

    ''' <summary> Removes event handler. </summary>
    ''' <remarks> David, 12/17/2015. </remarks>
    ''' <param name="value"> The emulated value. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveEventHandler(ByVal value As EventHandler(Of EventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.ServiceRequested, CType(d, EventHandler(Of EventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub

    ''' <summary> Emulate service request. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    ''' <param name="statusByte"> The status byte. </param>
    Public Sub EmulateServiceRequest(ByVal statusByte As ServiceRequests)
        Me.EmulatedStatusByte = statusByte
        If Me.ServiceRequestEventEnabled Then Me.OnServiceRequested()
    End Sub

    ''' <summary> Executes the service requested action. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Protected Sub OnServiceRequested()
        Dim evt As EventHandler(Of EventArgs) = Me.ServiceRequestedEvent
        evt?.Invoke(Me, System.EventArgs.Empty)
    End Sub

    ''' <summary> Gets the sentinel indication if a service request event was enabled and registered. </summary>
    ''' <value> <c>True</c> if service request event is enabled and registered; otherwise, <c>False</c>. </value>
    Public MustOverride ReadOnly Property ServiceRequestEventEnabled As Boolean

    ''' <summary> Enables the service request. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public MustOverride Sub EnableServiceRequest()

    ''' <summary> Disables the service request. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public MustOverride Sub DisableServiceRequest()

#End Region

#Region " TRIGGER "

    ''' <summary> Assert trigger. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public MustOverride Sub AssertTrigger()

#End Region

#Region " INTERFACE "

    ''' <summary> Determines if we can requires keep alive. </summary>
    ''' <remarks> David, 3/15/2016. </remarks>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function RequiresKeepAlive() As Boolean
        Return Me.ResourceInfo.InterfaceType = VI.HardwareInterfaceType.Tcpip
    End Function

    ''' <summary> Supports clear interface. </summary>
    ''' <returns> <c>True</c> if supports clearing the interface. </returns>
    Public Function SupportsClearInterface() As Boolean
        Return Me.ResourceInfo.InterfaceType = VI.HardwareInterfaceType.Gpib
    End Function

    ''' <summary> Clears the interface. </summary>
    Public MustOverride Sub ClearInterface()

    ''' <summary> Clears the device (SDC). </summary>
    Public MustOverride Sub ClearDevice()

#End Region

#Region " OVERRIDE NOT REUIRED "

#Region " TERMINATION "

    Private __TerminationCharacters As Char()

    ''' <summary> Gets or sets the termination characters. </summary>
    ''' <value> The termination characters. </value>
    Private Property _Termination As IEnumerable(Of Char)
        Get
            Return Me.__TerminationCharacters
        End Get
        Set(value As IEnumerable(Of Char))
            If value IsNot Nothing AndAlso value.Any Then
                Me.__TerminationCharacters = New Char(value.Count - 1) {}
                Array.Copy(value.ToArray, Me.__TerminationCharacters, value.Count)
                Me.SafePostPropertyChanged(NameOf(Me.Termination))
            End If
        End Set
    End Property

    ''' <summary> Gets the termination characters. </summary>
    Public ReadOnly Property Termination() As IEnumerable(Of Char)
        Get
            Return _Termination
        End Get
    End Property


    ''' <summary> Use default termination. </summary>
    Private Sub _UseDefaultTermination()
        Me._NewTermination(Environment.NewLine.ToCharArray()(1))
    End Sub

    ''' <summary> Use default termination. </summary>
    Public Sub UseDefaultTermination()
        Me._UseDefaultTermination()
    End Sub

    ''' <summary> Creates a new termination. </summary>
    Private Sub _NewTermination(ByVal value As Char)
        Me._Termination = New Char() {value}
    End Sub

    ''' <summary> Creates a new termination. </summary>
    ''' <param name="values"> The values. </param>
    Private Sub _NewTermination(ByVal values() As Char)
        If values Is Nothing Then Throw New ArgumentNullException(NameOf(values))
        If values.Length = 0 Then Throw New InvalidOperationException("Failed creating new termination; Source array must have at least one character")
        Dim terms As Char() = New Char(values.Length - 1) {}
        Array.Copy(values, terms, values.Length)
        Me._Termination = terms
    End Sub

    ''' <summary> Creates a new termination. </summary>
    ''' <param name="values"> The values. </param>
    Public Sub NewTermination(ByVal values() As Char)
        Me._NewTermination(values)
    End Sub

    ''' <summary> Use new line termination. </summary>
    Public Sub UseNewLineTermination()
        Me._NewTermination(Environment.NewLine.ToCharArray)
    End Sub

    ''' <summary> Use line feed termination. </summary>
    Public Sub UseLinefeedTermination()
        Me._NewTermination(Environment.NewLine.ToCharArray()(1))
    End Sub

    ''' <summary> Use Carriage Return termination. </summary>
    Public Sub UseCarriageReturnTermination()
        Me._NewTermination(Environment.NewLine.ToCharArray()(1))
    End Sub

#End Region

#Region " EXECUTE "

    ''' <summary> Executes the command. </summary>
    ''' <param name="command"> The command. </param>
    Public Sub Execute(ByVal command As String)
        Me.WriteLine(command)
    End Sub

    ''' <summary> Executes the command using the specified timeout. </summary>
    ''' <param name="command"> The command. </param>
    Public Sub Execute(ByVal command As String, ByVal timeout As TimeSpan)
        Try
            Me.StoreTimeout(timeout)
            Me.WriteLine(command)
        Catch
            Throw
        Finally
            Me.RestoreTimeout()
        End Try
    End Sub

    ''' <summary> Executes the <see cref="Action">action</see>/> using the specified timeout. </summary>
    ''' <param name="action"> The action. </param>
    Public Sub Execute(ByVal action As Action, ByVal timeout As TimeSpan)
        If action Is Nothing Then Throw New ArgumentNullException(NameOf(action))
        Try
            Me.StoreTimeout(timeout)
            action()
        Catch
            Throw
        Finally
            Me.RestoreTimeout()
        End Try
    End Sub

    ''' <summary> Executes the command using the specified timeout. </summary>
    ''' <remarks> David, 1/9/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="action">  The action. </param>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A Boolean? </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")>
    Public Function Execute(action As Func(Of Boolean?), ByVal timeout As TimeSpan) As Boolean?
        If action Is Nothing Then Throw New ArgumentNullException(NameOf(action))
        Try
            Me.StoreTimeout(timeout)
            Return action()
        Catch
            Throw
        Finally
            Me.RestoreTimeout()
        End Try
    End Function

#End Region

#Region " READ / WRITE LINE"

    ''' <summary> Synchronously writes and ASCII-encoded string data. Terminates the data with the 
    '''           <see cref="Termination">termination characters</see>. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    Public Sub WriteLine(ByVal format As String, ByVal ParamArray args() As Object)
        Me.WriteLine(String.Format(Globalization.CultureInfo.InvariantCulture, format, args))
    End Sub

    ''' <summary> Synchronously writes and ASCII-encoded string data. Terminates the data with the 
    '''           <see cref="Termination">termination characters</see>. </summary>
    ''' <param name="dataToWrite"> The data to write. </param>
    Public Sub WriteLine(ByVal dataToWrite As String)
        Me.Write(dataToWrite)
#If LogIO Then
        My.Application.Log.WriteEntry($"w: {dataToWrite}")
#End If
    End Sub

    ''' <summary>
    ''' Reads to and including the end <see cref="Termination">termination character</see>.
    ''' </summary>
    ''' <remarks> David, 11/17/2015. </remarks>
    ''' <returns> The string. </returns>
    Public Function ReadLine() As String
#If LogIO Then
        Dim value As String = Me.ReadString()
        My.Application.Log.WriteEntry($"r: {value.TrimEnd}")
#Else
        Return Me.ReadFiniteLine()
#End If
    End Function

#End Region

#Region " QUERY "

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. </summary>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The  <see cref="SessionBase.LastMessageReceived">last received data</see>. </returns>
    Public Overloads Function Query(ByVal dataToWrite As String) As String
        If Not String.IsNullOrWhiteSpace(dataToWrite) Then
            Me.WriteLine(dataToWrite)
            Return Me.ReadLine()
        End If
        Return ""
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The  <see cref="SessionBase.LastMessageReceived">last received data</see>. </returns>
    Public Overloads Function Query(ByVal format As String, ByVal ParamArray args() As Object) As String
        If Not String.IsNullOrWhiteSpace(format) Then
            Me.WriteLine(format, args)
            Return Me.ReadLine()
        End If
        Return ""
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. </summary>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The <see cref="SessionBase.LastMessageReceived">last received data</see> without the
    ''' <see cref="Termination">termination characters</see>. </returns>
    Public Overloads Function QueryTrimTermination(ByVal dataToWrite As String) As String
        Return Me.Query(dataToWrite).TrimEnd(Me.Termination.ToArray)
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. </summary>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The <see cref="SessionBase.LastMessageReceived">last received data</see> without the
    ''' <see cref="Termination">termination characters</see>. </returns>
    Public Overloads Function QueryTrimEnd(ByVal dataToWrite As String) As String
        Return Me.Query(dataToWrite).TrimEnd(Me.Termination.ToArray)
    End Function

    ''' <summary> Queries the device and returns a string save the termination character.
    '''           Expects terminated query command. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The <see cref="SessionBase.LastMessageReceived">last received data</see> without the
    ''' <see cref="Termination">termination characters</see>. </returns>
    Public Overloads Function QueryTrimEnd(ByVal format As String, ByVal ParamArray args() As Object) As String
        Return Me.Query(format, args).TrimEnd(Me.Termination.ToArray)
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The <see cref="SessionBase.LastMessageReceived">last received data</see> without the
    ''' <see cref="Termination">termination characters</see>. </returns>
    Public Overloads Function QueryTrimTermination(ByVal format As String, ByVal ParamArray args() As Object) As String
        Return Me.Query(format, args).TrimEnd(Me.Termination.ToArray)
    End Function

#End Region

#Region " READ "

    ''' <summary> Synchronously reads ASCII-encoded string data. Reads up to the
    ''' <see cref="Termination">termination characters</see>. </summary>
    ''' <returns> The received message without the <see cref="Termination">termination
    ''' characters</see>. </returns>
    Public Overloads Function ReadLineTrimTermination() As String
        Return Me.ReadLine().TrimEnd(Me.Termination.ToArray)
    End Function

    ''' <summary> Synchronously reads ASCII-encoded string data. Reads up to the
    ''' <see cref="Termination">termination characters</see>. </summary>
    ''' <returns> The received message without the <see cref="Termination">termination
    ''' characters</see>. </returns>
    Public Overloads Function ReadLineTrimEnd() As String
        Return Me.ReadLine().TrimEnd(Me.Termination.ToArray)
    End Function

    ''' <summary> Reads free line trim end. </summary>
    ''' <remarks> David, 1/21/2017. </remarks>
    ''' <returns> The free line trim end. </returns>
    Public Overloads Function ReadFreeLineTrimEnd() As String
        Return Me.ReadFreeLine().TrimEnd(Me.Termination.ToArray)
    End Function

    ''' <summary> Synchronously reads ASCII-encoded string data. Reads up to the
    ''' <see cref="Termination">termination character</see>. </summary>
    ''' <returns> The received message without the carriage return (13) and line feed (10) characters. </returns>
    Public Overloads Function ReadLineTrimNewLine() As String
        Return Me.ReadLine().TrimEnd(New Char() {Convert.ToChar(13), Convert.ToChar(10)})
    End Function

    ''' <summary> Synchronously reads ASCII-encoded string data. Reads up to the
    ''' <see cref="Termination">termination characters</see>. </summary>
    ''' <returns> The received message without the line feed (10) characters. </returns>
    Public Overloads Function ReadLineTrimLinefeed() As String
        Return Me.ReadLine().TrimEnd(Convert.ToChar(10))
    End Function

#End Region

#Region " READ LINES "

    ''' <summary>
    ''' Reads multiple lines from the instrument until data is no longer available.
    ''' </summary>
    ''' <remarks> David, 11/18/2015. </remarks>
    ''' <param name="pollDelay"> Time to wait between service requests. </param>
    ''' <param name="timeout">   Specifies the time to wait for message available. </param>
    ''' <param name="trimEnd">   Specifies a directive to trim the end character from each line. </param>
    ''' <returns> Data. </returns>
    Public Function ReadLines(ByVal pollDelay As TimeSpan, ByVal timeout As TimeSpan, ByVal trimEnd As Boolean) As String
        Return ReadLines(pollDelay, timeout, False, trimEnd)
    End Function

    ''' <summary>
    ''' Reads multiple lines from the instrument until data is no longer available.
    ''' </summary>
    ''' <remarks> David, 11/18/2015. </remarks>
    ''' <param name="pollDelay">  Time to wait between service requests. </param>
    ''' <param name="timeout">    Specifies the time to wait for message available. </param>
    ''' <param name="trimSpaces"> Specifies a directive to trim leading and trailing spaces from each
    '''                           line. This also trims the end character. </param>
    ''' <param name="trimEnd">    Specifies a directive to trim the end character from each line. </param>
    ''' <returns> Data. </returns>
    Public Function ReadLines(ByVal pollDelay As TimeSpan, ByVal timeout As TimeSpan,
                              ByVal trimSpaces As Boolean, ByVal trimEnd As Boolean) As String

        Dim listBuilder As New System.Text.StringBuilder

        Dim endTime As Date = DateTime.Now.Add(timeout)
        Dim timedOut As Boolean = False
        Do While Not timedOut

            ' allow message available time to materialize
            Me.ReadServiceRequestStatus()
            Do Until Me.MeasurementAvailable OrElse timedOut
                timedOut = DateTime.Now > endTime
                Dim t1 As DateTime = DateTime.Now.Add(pollDelay)
                Do Until DateTime.Now > t1
                    System.Windows.Forms.Application.DoEvents()
                    Threading.Thread.Sleep(2)
                    System.Windows.Forms.Application.DoEvents()
                Loop
                Me.ReadServiceRequestStatus()
            Loop

            If Me.MeasurementAvailable Then
                timedOut = False
                endTime = DateTime.Now.Add(timeout)
                If trimSpaces Then
                    listBuilder.AppendLine(Me.ReadLine().Trim())
                ElseIf trimEnd Then
                    listBuilder.AppendLine(Me.ReadLineTrimEnd())
                Else
                    listBuilder.AppendLine(Me.ReadLine())
                End If
            End If

        Loop
        Return listBuilder.ToString

    End Function

    ''' <summary> Reads multiple lines from the instrument until timeout. </summary>
    ''' <param name="timeout">        Specifies the time in millisecond to wait for message available. </param>
    ''' <param name="trimSpaces">     Specifies a directive to trim leading and trailing spaces from
    ''' each line. </param>
    ''' <param name="expectedLength"> Specifies the amount of data expected without trimming. </param>
    ''' <returns> Data. </returns>
    Public Function ReadLines(ByVal timeout As TimeSpan, ByVal trimSpaces As Boolean, ByVal expectedLength As Integer) As String

        Try

            Dim listBuilder As New System.Text.StringBuilder
            Me.StoreTimeout(timeout)

            Dim currentLength As Integer = 0
            Do While currentLength < expectedLength
                Dim buffer As String = Me.ReadLine()
                expectedLength += buffer.Length
                If trimSpaces Then
                    listBuilder.AppendLine(buffer.Trim)
                Else
                    listBuilder.AppendLine(buffer)
                End If
            Loop

            Return listBuilder.ToString

        Catch
            Throw
        Finally
            Me.RestoreTimeout()
        End Try

    End Function

#End Region

#Region " DISCARD UNREAD DATA "

    ''' <summary> Reads and discards all data from the VISA session until the END indicator is read. </summary>
    ''' <remarks> Uses 10ms poll delay, 100ms timeout. </remarks>
    Public Sub DiscardUnreadData()
        Me.DiscardUnreadData(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(100))
    End Sub

    Private _DiscardedData As System.Text.StringBuilder

    ''' <summary> Gets the information describing the discarded. </summary>
    ''' <value> Information describing the discarded. </value>
    Public ReadOnly Property DiscardedData As String
        Get
            If Me._DiscardedData Is Nothing Then
                Return ""
            Else
                Return Me._DiscardedData.ToString
            End If
        End Get
    End Property

    ''' <summary>
    ''' Reads and discards all data from the VISA session until the END indicator is read and no data
    ''' are added to the output buffer.
    ''' </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    ''' <param name="pollDelay"> Time to wait between service requests. </param>
    ''' <param name="timeout">   Specifies the time to wait for message available. </param>
    Public Sub DiscardUnreadData(ByVal pollDelay As TimeSpan, ByVal timeout As TimeSpan)

        Me._DiscardedData = New System.Text.StringBuilder
        Do

            Dim endTime As Date = DateTime.Now.Add(timeout)

            ' allow message available time to materialize
            Do Until Me.IsMessageAvailable(Me.MessageAvailableBits) OrElse DateTime.Now > endTime
                Threading.Thread.Sleep(pollDelay)
            Loop

            If Me.MessageAvailable Then
                Me._DiscardedData.AppendLine(Me.ReadLine())
            End If

        Loop While Me.MessageAvailable

    End Sub

#End Region

#Region " TIMEOUT MANAGEMENT "

    ''' <summary> Gets the minimum timeout. </summary>
    ''' <value> The minimum timeout. </value>
    ''' <remarks> Defaults to 3 seconds. </remarks>
    Public Property MinimumTimeout As TimeSpan

    ''' <summary> The timeouts. </summary>
    Private _Timeouts As System.Collections.Generic.Stack(Of TimeSpan)

    ''' <summary> Gets the reference to the stack of timeouts. </summary>
    ''' <value> The timeouts. </value>
    Protected ReadOnly Property Timeouts As System.Collections.Generic.Stack(Of TimeSpan)
        Get
            Return Me._Timeouts
        End Get
    End Property

    ''' <summary> Restores the last timeout from the stack. </summary>
    Public Sub RestoreTimeout()
        Me.Timeout = Me.Timeouts.Pop
    End Sub

    ''' <summary> Saves the current timeout and sets a new setting timeout. </summary>
    ''' <param name="timeout"> Specifies the new timeout. </param>
    Public Sub StoreTimeout(ByVal timeout As TimeSpan)
        Me.Timeouts.Push(Me.Timeout)
        Me.Timeout = timeout
    End Sub

#End Region

#End Region

End Class

