Imports System.ComponentModel
Imports System.Timers
Imports isr.Core.Pith.EventHandlerExtensions
''' <summary> Base class for SessionBase. </summary>
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

#Region " CONSTRUCTORS "

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    Protected Sub New()
        MyBase.New()
        Me._ResourceNameParseInfo = New ResourceNameParseInfo
        Me._Enabled = True
        Me._ResourceNameInfo = New ResourceNameInfo
        Me._ResourceNameInfo.ResumePublishing()
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
                    If Me.ResourceNameInfo IsNot Nothing Then Me._ResourceNameInfo.Dispose() :
                    Me._ResourceNameInfo = Nothing
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

    Private _IsDeviceOpen As Boolean
    ''' <summary>
    ''' Gets or sets the Device Open sentinel. When open, the device is capable of addressing real
    ''' hardware if the session is open. See also <see cref="IsSessionOpen"/>.
    ''' </summary>
    ''' <value> The is device open. </value>
    Public Property IsDeviceOpen As Boolean
        Get
            Return Me._IsDeviceOpen
        End Get
        Protected Set(value As Boolean)
            If value <> Me.IsDeviceOpen Then
                Me._IsDeviceOpen = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets the sentinel indicating weather this is a dummy session. </summary>
    ''' <value> The dummy sentinel. </value>
    Public MustOverride ReadOnly Property IsDummy As Boolean

    ''' <summary> Executes the session open action. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Private Sub OnSessionOpen(ByVal resourceName As String, ByVal resourceTitle As String)
        If Me.ResourceNameParseInfo.InterfaceType = HardwareInterfaceType.Tcpip AndAlso Me.KeepAliveInterval > TimeSpan.Zero Then
            Me._KeepAliveTimer = New Timers.Timer With {
                .Interval = Me.KeepAliveInterval.TotalMilliseconds,
                .Enabled = True
            }
        End If
        Me.IsDeviceOpen = True
        Me.ResourceNameInfo.HandleSessionOpen(resourceName, resourceTitle)
        Me.Timeouts.Push(Me.Timeout)
    End Sub

    ''' <summary> Creates a session. </summary>
    ''' <remarks> Throws an exception if the resource is not accessible. </remarks>
    ''' <param name="resourceName"> The name of the resource. </param>
    ''' <param name="timeout">      The timeout. </param>
    Protected MustOverride Sub CreateSession(ByVal resourceName As String, ByVal timeout As TimeSpan)

    ''' <summary> Opens a <see cref="VI.Pith.SessionBase">Session</see>. </summary>
    ''' <remarks> Call this first. </remarks>
    Public Sub OpenSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal timeout As TimeSpan, ByVal syncContext As Threading.SynchronizationContext)
        Try
            Me.CaptureSyncContext(syncContext)
            Me.ResourceNameParseInfo.Parse(resourceName)
            Me.LastAction = $"Opening {resourceName}"
            Me.Timeouts.Clear()
            Me.CreateSession(resourceName, timeout)
            Me.OnSessionOpen(resourceName, resourceTitle)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary> Opens a <see cref="VI.Pith.SessionBase">Session</see>. </summary>
    ''' <remarks> Call this first. </remarks>
    ''' <param name="resourceName">  The name of the resource. </param>
    ''' <param name="resourceTitle"> The short title of the device. </param>
    Public Sub OpenSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal syncContext As Threading.SynchronizationContext)
        Me.OpenSession(resourceName, resourceTitle, Me.DefaultOpenTimeout, syncContext)
    End Sub

    ''' <summary> Opens a <see cref="VI.Pith.SessionBase">Session</see>. </summary>
    ''' <param name="resourceName"> The name of the resource. </param>
    Public Sub OpenSession(ByVal resourceName As String, ByVal syncContext As Threading.SynchronizationContext)
        Me.OpenSession(resourceName, resourceName, Me.DefaultOpenTimeout, syncContext)
    End Sub

    ''' <summary> Opens a <see cref="VI.Pith.SessionBase">Session</see>. </summary>
    ''' <param name="timeout"> The timeout. </param>
    Public Sub OpenSession(ByVal resourceName As String, ByVal timeout As TimeSpan, ByVal syncContext As Threading.SynchronizationContext)
        Me.OpenSession(resourceName, resourceName, timeout, syncContext)
    End Sub

    ''' <summary> Discards the session events. </summary>
    Protected MustOverride Sub DiscardSessionEvents()

    ''' <summary> Closes the <see cref="VI.Pith.SessionBase">Session</see>. </summary>
    Public Sub CloseSession()
        If Me._KeepAliveTimer IsNot Nothing Then
            Me._KeepAliveTimer.Enabled = False
            Me._KeepAliveTimer.Dispose()
            Me._KeepAliveTimer = Nothing
        End If
        Me._Timeouts = New Collections.Generic.Stack(Of TimeSpan)
        If Me.IsDeviceOpen Then
            Me.DiscardSessionEvents()
            Me.IsDeviceOpen = False
            Me.ResourceNameInfo.IsDeviceOpen = False
            ' must use sync notification to prevent race condition if disposing the session.
            Me.SafeSendPropertyChanged(NameOf(SessionBase.IsSessionOpen))
        End If
    End Sub

    Private _LastAction As String
    ''' <summary> Gets the last action. </summary>
    ''' <value> The last action. </value>
    Public Property LastAction As String
        Get
            Return Me._LastAction
        End Get
        Set(ByVal value As String)
            If Not String.Equals(Me.LastAction, value) Then
                Me._LastAction = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    Private _LastNodeNumber As Integer?
    ''' <summary> Gets or sets the last node. </summary>
    ''' <value> The last node. </value>
    Public Property LastNodeNumber As Integer?
        Get
            Return Me._LastNodeNumber
        End Get
        Set(value As Integer?)
            If Not Nullable.Equals(Me.LastNodeNumber, value) Then
                Me._LastNodeNumber = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the last native error. </summary>
    ''' <value> The last native error. </value>
    Protected MustOverride ReadOnly Property LastNativeError As VI.Pith.NativeErrorBase

    ''' <summary>
    ''' Gets or sets the sentinel indicating if call backs are performed in a specific
    ''' synchronization context.
    ''' </summary>
    ''' <value>
    ''' The sentinel indicating if call backs are performed in a specific synchronization context.
    ''' </value>
    Public Overridable Property SynchronizeCallbacks As Boolean

#Region " RESOURCE NAME INFO "

    ''' <summary> Gets or sets information describing the parsed resource name. </summary>
    ''' <value> Information describing the resource. </value>
    Public ReadOnly Property ResourceNameParseInfo As ResourceNameParseInfo

#Region " RESOURCE NAME INFO EVENT "
#If False Then
    ''' <summary> Handles the property change. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal sender As VI.Pith.ResourceNameInfo, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(VI.Pith.SessionBase.IsDeviceOpen)
                Case NameOf(VI.Pith.SessionBase.ResourceTitle)
                Case NameOf(VI.Pith.SessionBase.ResourceName)
                Case Else
                    Me.SafeSendPropertyChanged($"{NameOf(SessionBase.ResourceNameInfo)}.{propertyName}")
            End Select
        End If
    End Sub

    ''' <summary> Resource name information property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    Private Sub _ResourceNameInfo_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.Pith.ResourceNameInfo)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, VI.Pith.ResourceNameInfo), e.PropertyName)
        Catch ex As Exception
            My.MyLibrary.LogUnpublishedException(activity, ex)
        End Try

    End Sub
#End If
#End Region

    ''' <summary> Gets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public ReadOnly Property ResourceName As String
        Get
            Return Me.ResourceNameInfo.ResourceName
        End Get
    End Property

    ''' <summary> Gets the resource name caption. </summary>
    ''' <value> The resource name caption. </value>
    Public ReadOnly Property ResourceNameCaption As String
        Get
            Return Me.ResourceNameInfo.ResourceNameCaption
        End Get
    End Property

    Private _ResourceNameInfo As VI.Pith.ResourceNameInfo

    ''' <summary> Gets information describing the resource name. </summary>
    ''' <value> Information describing the resource name. </value>
    Public ReadOnly Property ResourceNameInfo As VI.Pith.ResourceNameInfo
        Get
            Return Me._ResourceNameInfo
        End Get
    End Property

    ''' <summary>
    ''' Checks if the resource name exists. Use for checking if the instrument is turned on.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public MustOverride Function FindResource() As Boolean

    ''' <summary>
    ''' Checks if the resource name exists. Use for checking if the instrument is turned on.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="manager"> The manager. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function FindResource(ByVal manager As VI.Pith.ResourcesManagerBase) As Boolean
        If manager Is Nothing Then Throw New ArgumentNullException(NameOf(manager))
        Return Me.ResourceNameInfo.Find(manager)
    End Function

#End Region

#Region " MESSAGE EVENTS "

    Private _MessageNotificationLevel As Core.Pith.NotifySyncLevel

    ''' <summary> Gets or sets the message notification level. </summary>
    ''' <value> The message notification level. </value>
    Public Property MessageNotificationLevel As Core.Pith.NotifySyncLevel
        Get
            Return Me._MessageNotificationLevel
        End Get
        Set(value As Core.Pith.NotifySyncLevel)
            If value <> Me.MessageNotificationLevel Then
                Me._MessageNotificationLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> List notification levels. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="control"> The control. </param>
    Public Shared Sub ListNotificationLevels(ByVal control As System.Windows.Forms.ComboBox)
        If control Is Nothing Then Throw New ArgumentNullException(NameOf(control))
        Dim comboEnabled As Boolean = control.Enabled
        control.Enabled = False
        control.DataSource = Nothing
        control.ValueMember = "Key"
        control.DisplayMember = "Value"
        control.Items.Clear()
        control.Items.Add(New KeyValuePair(Of Core.Pith.NotifySyncLevel, String)(Core.Pith.NotifySyncLevel.None, "None"))
        control.Items.Add(New KeyValuePair(Of Core.Pith.NotifySyncLevel, String)(Core.Pith.NotifySyncLevel.Async, "Asynchronous"))
        control.Items.Add(New KeyValuePair(Of Core.Pith.NotifySyncLevel, String)(Core.Pith.NotifySyncLevel.Sync, "Synchronous"))
        control.Enabled = comboEnabled
        control.Invalidate()
    End Sub

    ''' <summary> Select item. </summary>
    ''' <param name="control"> The control. </param>
    ''' <param name="value">   The value. </param>
    Public Shared Sub SelectItem(ByVal control As Windows.Forms.ToolStripComboBox, ByVal value As Core.Pith.NotifySyncLevel)
        If control IsNot Nothing Then
            control.SelectedItem = New KeyValuePair(Of Core.Pith.NotifySyncLevel, String)(value, value.ToString)
        End If
    End Sub

    ''' <summary> Selected value. </summary>
    ''' <param name="control">      The control. </param>
    ''' <param name="defaultValue"> The default value. </param>
    ''' <returns> A Core.Pith.NotifySyncLevel. </returns>
    Public Shared Function SelectedValue(ByVal control As Windows.Forms.ToolStripComboBox, ByVal defaultValue As Core.Pith.NotifySyncLevel) As Core.Pith.NotifySyncLevel
        If control IsNot Nothing AndAlso control.SelectedItem IsNot Nothing Then
            Dim kvp As KeyValuePair(Of Core.Pith.NotifySyncLevel, String) = CType(control.SelectedItem, KeyValuePair(Of Core.Pith.NotifySyncLevel, String))
            If [Enum].IsDefined(GetType(Core.Pith.NotifySyncLevel), kvp.Key) Then
                defaultValue = kvp.Key
            End If
        End If
        Return defaultValue
    End Function



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
            If Me.MessageNotificationLevel = Core.Pith.NotifySyncLevel.Async Then
                Me.SafePostPropertyChanged()
            ElseIf Me.MessageNotificationLevel = Core.Pith.NotifySyncLevel.Sync Then
                Me.SafeSendPropertyChanged()
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
            If Me.MessageNotificationLevel = Core.Pith.NotifySyncLevel.Async Then
                Me.SafePostPropertyChanged()
            ElseIf Me.MessageNotificationLevel = Core.Pith.NotifySyncLevel.Sync Then
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " KEEP ALIVE "

    Private WithEvents _KeepAliveTimer As Timers.Timer

    ''' <summary> Keep alive timer elapsed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Elapsed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub KeepAliveTimerElapsed(sender As Object, e As ElapsedEventArgs) Handles _KeepAliveTimer.Elapsed
        Try
            Dim tmr As Timers.Timer = TryCast(sender, Timers.Timer)
            If tmr IsNot Nothing AndAlso tmr.Interval < DateTime.UtcNow.Subtract(Me.LastInputOutputTime).TotalMilliseconds Then
                Me.KeepAlive()
                Me.LastInputOutputTime = DateTime.UtcNow
            End If
        Catch
        Finally
        End Try
    End Sub

    ''' <summary> Gets or sets the keep alive interval. Must be smaller or equal to half the communication timeout interval. </summary>
    ''' <value> The keep alive interval. </value>
    Public Property KeepAliveInterval As TimeSpan

    ''' <summary> Query if this object is alive. </summary>
    ''' <returns> <c>true</c> if alive; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function IsAlive() As Boolean
        Dim affirmative As Boolean = True
        If Me.IsSessionOpen AndAlso Me.ResourceNameParseInfo.UsingLanController Then
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

    ''' <summary> Gets or sets the termination character enabled. </summary>
    ''' <value> The termination character enabled. </value>
    Public Overridable Property TerminationCharacterEnabled As Boolean

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
    ''' <returns> The received message. </returns>
    Public MustOverride Function ReadFiniteLine() As String

    ''' <summary>
    ''' Synchronously reads ASCII-encoded string data until reaching the
    ''' <see cref="TerminationCharacter">termination character</see>. Not limited by the
    ''' <see cref="InputBufferSize"/>.
    ''' </summary>
    ''' <returns> The received message. </returns>
    Public MustOverride Function ReadFreeLine() As String

    ''' <summary> Writes. </summary>
    ''' <param name="dataToWrite"> The data to write to write. </param>
    Public MustOverride Sub Write(ByVal dataToWrite As String)

    ''' <summary> Clears the device. </summary>
    Public MustOverride Sub Clear()

    ''' <summary> Reads status byte. </summary>
    ''' <returns> The status byte. </returns>
    Public MustOverride Function ReadStatusByte() As VI.Pith.ServiceRequests

    ''' <summary> Gets or sets the size of the input buffer. </summary>
    ''' <value> The size of the input buffer. </value>
    Public MustOverride ReadOnly Property InputBufferSize() As Integer

#End Region

#Region " EMULATION "

    ''' <summary> Makes emulated reply. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeTrueFalseReply(ByVal value As Boolean)
        Me.EmulatedReply = SessionBase.ToTrueFalse(value)
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeTrueFalseReplyIfEmpty(ByVal value As Boolean)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeTrueFalseReply(value)
        End If
    End Sub

    ''' <summary> Makes emulated reply. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReply(ByVal value As Boolean)
        Me.EmulatedReply = SessionBase.ToOneZero(value)
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As Boolean)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

    ''' <summary> Makes emulated reply. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReply(ByVal value As Double)
        Me.EmulatedReply = CStr(value)
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As Double)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

    ''' <summary> Makes emulated reply. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReply(ByVal value As Integer)
        Me.EmulatedReply = CStr(value)
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As Integer)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

    ''' <summary> Makes emulated reply. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReply(ByVal value As TimeSpan)
        Me.EmulatedReply = value.ToString
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As TimeSpan)
        If String.IsNullOrWhiteSpace(Me.EmulatedReply) Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

    ''' <summary> Makes emulated reply. </summary>
    Public Sub MakeEmulatedReply(ByVal value As String)
        Me.EmulatedReply = value
    End Sub

    ''' <summary> Makes emulated reply if empty. </summary>
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
    Public Property EmulatedStatusByte As VI.Pith.ServiceRequests

    ''' <summary> Makes emulated status byte. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReply(ByVal value As VI.Pith.ServiceRequests)
        Me.EmulatedStatusByte = value
    End Sub

    ''' <summary> Makes emulated status byte if none. </summary>
    ''' <param name="value"> The emulated value. </param>
    Public Sub MakeEmulatedReplyIfEmpty(ByVal value As VI.Pith.ServiceRequests)
        If Me.EmulatedStatusByte = VI.Pith.ServiceRequests.None Then
            Me.MakeEmulatedReply(value)
        End If
    End Sub

    ''' <summary> Queries if a service request is enabled. </summary>
    ''' <param name="bitmask"> The bit mask. </param>
    ''' <returns> <c>true</c> if a service request is enabled; otherwise <c>false</c> </returns>
    Public Function IsServiceRequestEnabled(ByVal bitmask As VI.Pith.ServiceRequests) As Boolean
        Return Me.ServiceRequestEventEnabled AndAlso (Me.ServiceRequestEnableBitmask And bitmask) <> 0
    End Function

    ''' <summary> Gets or sets the service request enable bitmask. </summary>
    ''' <value> The service request enable bitmask. </value>
    Public ReadOnly Property ServiceRequestEnableBitmask As VI.Pith.ServiceRequests

    ''' <summary>
    ''' Applies the service request bitmask. Disables service request if bitmask is 0.
    ''' </summary>
    ''' <param name="commandFormat"> The service request enable command format. </param>
    ''' <param name="bitmask">       The bitmask. </param>
    Public Sub ApplyServiceRequestEnableBitmask(ByVal commandFormat As String, ByVal bitmask As VI.Pith.ServiceRequests)
        Me._ServiceRequestEnableBitmask = bitmask
        Me.WriteLine(commandFormat, CInt(bitmask))
        Me.SafePostPropertyChanged(NameOf(ServiceRequestEnableBitmask))
    End Sub

    ''' <summary> Event queue for all listeners interested in ServiceRequested events. </summary>
    Public Event ServiceRequested As EventHandler(Of EventArgs)

    ''' <summary> Removes event handler. </summary>
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
    ''' <param name="statusByte"> The status byte. </param>
    Public Sub EmulateServiceRequest(ByVal statusByte As VI.Pith.ServiceRequests)
        Me.EmulatedStatusByte = statusByte
        If Me.ServiceRequestEventEnabled Then Me.OnServiceRequested()
    End Sub

    ''' <summary> Executes the service requested action. </summary>
    Protected Sub OnServiceRequested()
        Dim evt As EventHandler(Of EventArgs) = Me.ServiceRequestedEvent
        evt?.Invoke(Me, System.EventArgs.Empty)
    End Sub

    ''' <summary> Gets the sentinel indication if a service request event was enabled and registered. </summary>
    ''' <value> <c>True</c> if service request event is enabled and registered; otherwise, <c>False</c>. </value>
    Public MustOverride ReadOnly Property ServiceRequestEventEnabled As Boolean

    ''' <summary> Enables the service request. </summary>
    Public MustOverride Sub EnableServiceRequest()

    ''' <summary> Disables the service request. </summary>
    Public MustOverride Sub DisableServiceRequest()

#End Region

#Region " TRIGGER "

    ''' <summary> Assert trigger. </summary>
    Public MustOverride Sub AssertTrigger()

#End Region

#Region " INTERFACE "

    ''' <summary> Determines if we can requires keep alive. </summary>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function RequiresKeepAlive() As Boolean
        Return Me.ResourceNameParseInfo.InterfaceType = HardwareInterfaceType.Tcpip
    End Function

    ''' <summary> Supports clear interface. </summary>
    ''' <returns> <c>True</c> if supports clearing the interface. </returns>
    Public Function SupportsClearInterface() As Boolean
        Return Me.ResourceNameParseInfo.InterfaceType = HardwareInterfaceType.Gpib
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
                Me.SafePostPropertyChanged(NameOf(SessionBase.Termination))
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
    ''' <returns> The string. </returns>
    Public Function ReadLine() As String
#If LogIO Then
        Dim value As String = Me.ReadString()
        My.Application.Log.WriteEntry($"r: {value.TrimEnd}")
#ElseIf False Then
        Return Me.ReadFiniteLine()
#Else
        ' this is an attempt to address seeing that a read is somehow delayed after a few writes. 
        ' not sure how the instrument is not issuing query unterminated errors in this case.
        Return Me.ReadFreeLine()
#End If
    End Function

#End Region

#Region " QUERY "

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. </summary>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>. </returns>
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
    ''' <returns> The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>. </returns>
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
    ''' <returns> The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
    ''' <see cref="Termination">termination characters</see>. </returns>
    Public Overloads Function QueryTrimTermination(ByVal dataToWrite As String) As String
        Return Me.Query(dataToWrite).TrimEnd(Me.Termination.ToArray)
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. </summary>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
    ''' <see cref="Termination">termination characters</see>. </returns>
    Public Overloads Function QueryTrimEnd(ByVal dataToWrite As String) As String
        Return Me.Query(dataToWrite).TrimEnd(Me.Termination.ToArray)
    End Function

    ''' <summary> Queries the device and returns a string save the termination character.
    '''           Expects terminated query command. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
    ''' <see cref="Termination">termination characters</see>. </returns>
    Public Overloads Function QueryTrimEnd(ByVal format As String, ByVal ParamArray args() As Object) As String
        Return Me.Query(format, args).TrimEnd(Me.Termination.ToArray)
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
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
    ''' <param name="pollDelay">  Time to wait between service requests. </param>
    ''' <param name="timeout">    Specifies the time to wait for message available. </param>
    ''' <param name="trimSpaces"> Specifies a directive to trim leading and trailing spaces from each
    '''                           line. This also trims the end character. </param>
    ''' <param name="trimEnd">    Specifies a directive to trim the end character from each line. </param>
    ''' <returns> Data. </returns>
    Public Function ReadLines(ByVal pollDelay As TimeSpan, ByVal timeout As TimeSpan,
                              ByVal trimSpaces As Boolean, ByVal trimEnd As Boolean) As String

        Dim listBuilder As New System.Text.StringBuilder

        Dim endTime As Date = DateTime.UtcNow.Add(timeout)
        Dim timedOut As Boolean = False
        Do While Not timedOut

            ' allow message available time to materialize
            Me.ReadServiceRequestStatus()
            Do Until Me.MeasurementAvailable OrElse timedOut
                timedOut = DateTime.UtcNow > endTime
                Dim t1 As DateTime = DateTime.UtcNow.Add(pollDelay)
                Do Until DateTime.UtcNow > t1
                    System.Windows.Forms.Application.DoEvents()
                    Threading.Thread.Sleep(2)
                    System.Windows.Forms.Application.DoEvents()
                Loop
                Me.ReadServiceRequestStatus()
            Loop

            If Me.MeasurementAvailable Then
                timedOut = False
                endTime = DateTime.UtcNow.Add(timeout)
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
    ''' <param name="pollDelay"> Time to wait between service requests. </param>
    ''' <param name="timeout">   Specifies the time to wait for message available. </param>
    Public Sub DiscardUnreadData(ByVal pollDelay As TimeSpan, ByVal timeout As TimeSpan)

        Me._DiscardedData = New System.Text.StringBuilder
        Do

            Dim endTime As Date = DateTime.UtcNow.Add(timeout)

            ' allow message available time to materialize
            Do Until Me.IsMessageAvailable(Me.MessageAvailableBits) OrElse DateTime.UtcNow > endTime
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

