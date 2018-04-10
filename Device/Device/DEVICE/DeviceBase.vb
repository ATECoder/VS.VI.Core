Imports System.ComponentModel
Imports System.Threading
Imports isr.Core.Pith
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.Core.Pith.EventHandlerExtensions
Imports isr.VI.ExceptionExtensions
Imports isr.VI.Pith
''' <summary> Defines the contract that must be implemented by Devices. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.         
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class DeviceBase
    Inherits PropertyPublisherTalkerBase
    Implements IPresettablePropertyPublisher

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="DeviceBase" /> class. </summary>
    Private Sub New()
        Me.New(isr.VI.SessionFactory.Get.Factory.CreateSession(), New TraceMessageTalker, False)
        Me._IsSessionOwner = True
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        Me.New(StatusSubsystemBase.Validated(statusSubsystem).Session, New TraceMessageTalker, False)
        Me.StatusSubsystemBase = statusSubsystem
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="DeviceBase" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.Pith.SessionBase">message based
    ''' session</see>. </param>
    Private Sub New(ByVal visaSession As VI.Pith.SessionBase, ByVal talker As TraceMessageTalker, ByVal isAssignedTalker As Boolean)
        MyBase.New(talker, isAssignedTalker)
        If visaSession Is Nothing Then
            Me.SafeAssignSession(isr.VI.SessionFactory.Get.Factory.CreateSession(), True)
        Else
            Me.SafeAssignSession(visaSession, False)
        End If
        Me._Subsystems = New SubsystemCollection
        Me._UsingSyncServiceRequestHandler = True
        Me.ConstructorSafeSetter(talker)
    End Sub

#Region " I Disposable Support "

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
            If Not Me.IsDisposed AndAlso disposing Then
                Me.MessageNotificationLevel = NotifySyncLevel.None
                Me.Session?.DisableServiceRequest()
                Me.RemoveServiceRequestEventHandler()
                Me.RemoveEventHandler(Me.ServiceRequestedEvent)
                Me.RemoveOpeningEventHandler(Me.OpeningEvent)

                Me.RemoveOpenedEventHandler(Me.OpenedEvent)
                Me.RemoveClosingEventHandler(Me.ClosingEvent)
                Me.RemoveClosedEventHandler(Me.ClosedEvent)
                Me.RemoveInitializingEventHandler(Me.InitializingEvent)
                Me.RemoveInitializedEventHandler(Me.InitializedEvent)
                ' this also removes the talker that was assigned to the subsystems.
                Me.Talker = Nothing
                If Me._IsSessionOwner Then
                    Try
                        Me.CloseSession()
                        Me.SafeAssignSession(Nothing, True)
                    Catch ex As ObjectDisposedException
                        Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
                    End Try
                End If
                Me.StatusSubsystemBase = Nothing
                Me.Subsystems.DisposeItems()
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Applies default settings and clears the Device. Issues <see cref="StatusSubsystemBase.ClearActiveState">Selective device clear</see>. </summary>
    Public Overridable Sub ClearActiveState()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} Clearing active state")
        Me.StatusSubsystemBase.ClearActiveState()
        ' 20180105: report any errors
        Me.PublishErrorEvent(TraceEventType.Warning)
    End Sub

    ''' <summary> Clears the queues and resets all registers to zero. Sets the subsystem properties to
    ''' the following CLS default values:<para>
    ''' </para> </summary>
    ''' <remarks> *CLS. </remarks>
    Public Overridable Sub ClearExecutionState() Implements IPresettable.ClearExecutionState
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} Clearing execution state")
        Me.Subsystems.ClearExecutionState()
        ' 20180105: report any errors
        Me.PublishErrorEvent(TraceEventType.Warning)
    End Sub

    ''' <summary> Initializes the Device. Used after reset to set a desired initial state. </summary>
    ''' <remarks> Use this to customize the reset. </remarks>
    Public Overridable Sub InitKnownState() Implements IPresettable.InitKnownState
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} Initializing known state")
        Me.Subsystems.InitKnownState()
        Me.IsInitialized = True
        ' 20180105: report any errors
        Me.PublishErrorEvent(TraceEventType.Warning)
    End Sub

    ''' <summary> Resets the Device to its known state. </summary>
    ''' <remarks> *RST. </remarks>
    Public Overridable Sub PresetKnownState() Implements IPresettable.PresetKnownState
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} Presetting known state")
        Me.Subsystems.PresetKnownState()
        ' 20180105: report any errors
        Me.PublishErrorEvent(TraceEventType.Warning)
    End Sub

    ''' <summary> Resets the Device to its known state. </summary>
    ''' <remarks> *RST. </remarks>
    Public Overridable Sub ResetKnownState() Implements IPresettable.ResetKnownState
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} Resetting known state")
        Me.Subsystems.ResetKnownState()
        ' 20180105: report any errors
        Me.PublishErrorEvent(TraceEventType.Warning)
    End Sub

    ''' <summary>
    ''' Resets, clears and initializes the device. Starts with issuing a selective-device-clear, reset (RST),
    ''' Clear Status (CLS, and clear error queue) and initialize.
    ''' </summary>
    Public Overridable Sub ResetClearInit()

        ' issues selective device clear.
        Me.ClearActiveState()

        ' reset device
        Me.ResetKnownState()

        ' Clear the device Status and set more defaults
        Me.ClearExecutionState()

        ' initialize the device must be done after clear (5892)
        Me.InitKnownState()

    End Sub

    ''' <summary>
    ''' Resets, clears and initializes the device. Starts with issuing a selective-device-clear, reset (RST),
    ''' Clear Status (CLS, and clear error queue) and initialize.
    ''' </summary>
    ''' <param name="timeout"> The timeout to use. This allows using a longer timeout than the
    ''' minimal timeout set for the session. Typically, a source meter requires a 5000 milliseconds
    ''' timeout. </param>
    Public Overridable Sub ResetClearInit(ByVal timeout As TimeSpan)
        Try
            Me.Session.StoreTimeout(timeout)
            Me.ResetClearInit()
        Catch
            Throw
        Finally
            Me.Session.RestoreTimeout()
        End Try
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Resumes the property events. </summary>
    Public Overrides Sub ResumePublishing()
        MyBase.ResumePublishing()
        Me.Subsystems.ResumePublishing()
    End Sub

    ''' <summary> Suppresses the property events. </summary>
    Public Overrides Sub SuspendPublishing()
        MyBase.SuspendPublishing()
        Me.Subsystems.SuspendPublishing()
    End Sub

#End Region

#Region " RESOURCE NAME INFO "

    ''' <summary>
    ''' Checks if the specified resource name exists. Use for checking if the instrument is turned on.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName">    Name of the resource. </param>
    ''' <param name="resourcesFilter"> The resources search pattern. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Shared Function Find(ByVal resourceName As String, ByVal resourcesFilter As String) As Boolean
        If String.IsNullOrWhiteSpace(resourceName) Then Throw New ArgumentNullException(NameOf(resourceName))
        Using rm As VI.Pith.ResourcesManagerBase = isr.VI.SessionFactory.Get.Factory.CreateResourcesManager()
            If String.IsNullOrWhiteSpace(resourcesFilter) Then
                Return rm.FindResources().ToArray.Contains(resourceName)
            Else
                Return rm.FindResources(resourcesFilter).ToArray.Contains(resourceName)
            End If
        End Using
    End Function

    ''' <summary>
    ''' Checks if the specified resource name exists. Use for checking if the instrument is turned on.
    ''' </summary>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function Find() As Boolean
        Return Me.Session.FindResource(isr.VI.SessionFactory.Get.Factory.CreateResourcesManager())
    End Function

#End Region

#Region " SESSION "

    ''' <summary> Capture synchronization context. </summary>
    ''' <param name="syncContext"> Context for the synchronization. </param>
    Public Overrides Sub CaptureSyncContext(ByVal syncContext As Threading.SynchronizationContext)
        MyBase.CaptureSyncContext(syncContext)
        Me.Session?.CaptureSyncContext(Me.CapturedSyncContext)
        Me.Subsystems?.CaptureSyncContext(Me.CapturedSyncContext)
    End Sub

    ''' <summary> true if this object is session owner. </summary>
    Private Property IsSessionOwner As Boolean

    ''' <summary> The session. </summary>
    Private WithEvents _Session As Vi.Pith.SessionBase

    ''' <summary> Gets the session. </summary>
    ''' <value> The session. </value>
    Public ReadOnly Property Session As Vi.Pith.SessionBase
        Get
            Return Me._Session
        End Get
    End Property

    ''' <summary> Constructor safe session assignment. If session owner and session exists, must close first. </summary>
    ''' <param name="value">          The value. </param>
    ''' <param name="isSessionOwner"> true if this object is session owner. </param>
    Private Sub SafeAssignSession(ByVal value As Vi.Pith.SessionBase, ByVal isSessionOwner As Boolean)
        If Me._Session IsNot Nothing Then
            Windows.Forms.Application.DoEvents()
            RemoveHandler Me.Session.PropertyChanged, AddressOf Me.SessionPropertyChanged
            If Me.IsSessionOwner Then
                Me._Session.Dispose()
                ' release the session
                ' Trying to null the session raises an ObjectDisposedException 
                ' if session service request handler was not released. 
                Me._Session = Nothing
            End If
        End If
        Me._Session = value
        If value IsNot Nothing Then
            AddHandler Me.Session.PropertyChanged, AddressOf Me.SessionPropertyChanged
            value.ResourceNameInfo.Publish()
        End If
        Me.IsSessionOwner = isSessionOwner
    End Sub

    ''' <summary> Gets the resource Name. </summary>
    ''' <value> The resource Name. </value>
    Public ReadOnly Property ResourceName As String
        Get
            If Me.Session Is Nothing Then
                Return VI.Pith.My.MySettings.Default.DefaultClosedResourceCaption
            Else
                Return Me.Session.ResourceNameInfo.ResourceName
            End If
        End Get
    End Property

    ''' <summary> Gets the resource title. </summary>
    ''' <value> The resource title. </value>
    Public ReadOnly Property ResourceTitle As String
        Get
            If Me.Session Is Nothing Then
                Return VI.Pith.My.MySettings.Default.DefaultResourceTitle
            Else
                Return Me.Session.ResourceNameInfo.ResourceTitle
            End If
        End Get
    End Property

    ''' <summary> Gets the resource name caption. </summary>
    ''' <value>
    ''' The <see cref="VI.Pith.SessionBase.ResourceName"/> resource <see cref="VI.Pith.ResourceNameInfo.ResourceNameCaption">closed
    ''' caption</see> if not open.
    ''' </value>
    Public ReadOnly Property ResourceNameCaption As String
        Get
            If Me.Session Is Nothing Then
                Return VI.Pith.My.MySettings.Default.DefaultClosedResourceCaption
            Else
                Return Me.Session.ResourceNameCaption
            End If
        End Get
    End Property

#Region " SESSION INITIALIZATION PROPERTIES "

    ''' <summary> Gets the bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Protected Overridable ReadOnly Property ErrorAvailableBits() As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.ErrorAvailable

    ''' <summary> Gets or sets the keep alive interval. </summary>
    ''' <value> The keep alive interval. </value>
    Protected Overridable ReadOnly Property KeepAliveInterval As TimeSpan = TimeSpan.FromSeconds(58)

    ''' <summary> Gets the is alive command. </summary>
    ''' <value> The is alive command. </value>
    Protected Overridable ReadOnly Property IsAliveCommand As String = "*OPC"

    ''' <summary> Gets the is alive query command. </summary>
    ''' <value> The is alive query command. </value>
    Protected Overridable ReadOnly Property IsAliveQueryCommand As String = "*OPC?"

#End Region

#End Region

#Region " SESSION: OPEN / CLOSE "

    Private _IsInitialized As Boolean

    ''' <summary> Gets or sets the Initialized sentinel of the device.
    '''           The device is ready after it is initialized. </summary>
    ''' <value> <c>True</c> if hardware device is Initialized; <c>False</c> otherwise. </value>
    Public Overridable Property IsInitialized As Boolean
        Get
            Return Me._IsInitialized
        End Get
        Set(ByVal value As Boolean)
            If Not Me.IsInitialized.Equals(value) Then
                Me._IsInitialized = value
                Me.SafePostPropertyChanged()
                Windows.Forms.Application.DoEvents()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                  $"{Me.ResourceNameCaption} {IIf(Me.IsInitialized, "initialized", "not ready")};. ")
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Enabled sentinel of the device.
    '''           A device is enabled when hardware can be used. </summary>
    ''' <value> <c>True</c> if hardware device is enabled; <c>False</c> otherwise. </value>
    Public Overridable Property Enabled As Boolean
        Get
            Return Me.Session.Enabled
        End Get
        Set(ByVal value As Boolean)
            If Me.Enabled <> value Then
                Me.Session.Enabled = value
                Me.SafeSendPropertyChanged()
                Windows.Forms.Application.DoEvents()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} {Me.Enabled.GetHashCode:enabled;enabled;disabled};. ")
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating whether a session to the device is open. See also
    ''' <see cref="VI.Pith.SessionBase.IsDeviceOpen"/>.
    ''' </summary>
    ''' <value> <c>True</c> if the device has an open session; otherwise, <c>False</c>. </value>
    Public ReadOnly Property IsDeviceOpen As Boolean
        Get
            Return Me.Session IsNot Nothing AndAlso Me.Session.IsDeviceOpen
        End Get
    End Property

    ''' <summary> Initializes the session prior to opening access to the instrument. </summary>
    Protected Overridable Sub BeforeOpening()
        Me.Session.MessageNotificationLevel = Me.MessageNotificationLevel
        Me.Session.KeepAliveInterval = Me.KeepAliveInterval
        Me.Session.IsAliveCommand = Me.IsAliveCommand
        Me.Session.IsAliveQueryCommand = Me.IsAliveQueryCommand
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    ''' <remarks> This override should occur as the first call of the overriding method.
    '''           After this call, the parent class adds the subsystems. </remarks>
    Protected Overridable Sub OnOpening(ByVal e As CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Me.Subsystems.Count > 1 Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} subsystem count {Me.Subsystems.Count} on opening;. ")
            Me.Subsystems.Clear()
            Me.Subsystems.Add(Me.StatusSubsystemBase)
        End If
        Me.IsInitialized = False
        Me.SuspendPublishing()
        Me.SyncNotifyOpening(e)
    End Sub

    ''' <summary> Allows the derived device to take actions after opening. </summary>
    ''' <remarks>
    ''' This override should occur as the last call of the overriding method.
    ''' The subsystems are added as part of the <see cref="OnOpening(cancelEventArgs)"/> method.
    ''' </remarks>
    Protected Overridable Sub OnOpened()

        Dim outcome As TraceEventType = TraceEventType.Information
        If Me.Session.Enabled And Not Me.Session.IsSessionOpen Then outcome = TraceEventType.Warning
        Me.Talker.Publish(outcome, My.MyLibrary.TraceEventId,
                          "{0} {1:enabled;enabled;disabled} and {2:open;open;closed}; session {3:open;open;closed};. ",
                          Me.ResourceNameCaption,
                          Me.Session.Enabled.GetHashCode,
                          Me.Session.IsDeviceOpen.GetHashCode,
                          Me.Session.IsSessionOpen.GetHashCode)

        ' send property change info up the ladder.
        Me.Session.ResourceNameInfo.Publish()

        ' A talker is assigned to the subsystem when the device is constructed. 
        ' This talker is assigned to each subsystem when it is added.
        ' Me.Subsystems.AssignTalker(Me.Talker)
        Me.ApplySettings()
        Me.Session.CaptureSyncContext(Me.CapturedSyncContext)
        Me.Subsystems.CaptureSyncContext(Me.CapturedSyncContext)

        ' reset and clear the device to remove any existing errors.
        Me.StatusSubsystemBase.OnDeviceOpen()

        Me.SafePostPropertyChanged(NameOf(DeviceBase.IsDeviceOpen))
        ' 2016/01/18: this was done before adding listeners, which was useful when using the device
        ' as a class in a 'meter'. As a result, the actions taken when handling the Opened event, 
        ' such as Reset and Initialize do not get reported. 
        ' The solution was to add the device Initialize event to process publishing and initialization of device
        ' and subsystems.
        Me.SyncNotifyOpened(System.EventArgs.Empty)
    End Sub

    ''' <summary> Executes the initializing actions. </summary>
    Protected Overridable Sub OnInitializing(ByVal e As CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Me.SyncNotifyInitializing(e)

        ' 2/18/16: replaces calls to reset and then init known states
        Me.ResetClearInit()
    End Sub

    ''' <summary> Executes the initialized action. </summary>
    Protected Overridable Sub OnInitialized()
        ' publish 
        Me.ResumePublishing()
        Me.Publish()
        Me.SyncNotifyInitialized(System.EventArgs.Empty)
    End Sub

    ''' <summary> Opens the session. </summary>
    ''' <remarks> Register the device trace notifier before opening the session. </remarks>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="resourceName"> Name of the resource. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Sub OpenSession(ByVal resourceName As String, ByVal resourceTitle As String)
        Dim success As Boolean = False
        Try
            If Me.Enabled Then Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Opening session to {resourceName};. ")
            ' initializes the session keep alive commands.
            Me.BeforeOpening()
            Me.Session.OpenSession(resourceName, resourceTitle, Me.CapturedSyncContext)
            If Me.Session.IsSessionOpen Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Session open to {resourceName};. ")
            ElseIf Me.Session.Enabled Then
                Throw New VI.Pith.OperationFailedException($"Unable to open session to {resourceName};. ")
            ElseIf Not Me.IsDeviceOpen Then
                Throw New VI.Pith.OperationFailedException($"Unable to emulate {resourceName};. ")
            End If
            If Me.Session.IsSessionOpen OrElse (Me.IsDeviceOpen AndAlso Not Me.Session.Enabled) Then

                Dim e As New CancelEventArgs
                Me.OnOpening(e)

                If e.Cancel Then Throw New OperationCanceledException($"Opening {resourceTitle}:{resourceName} canceled;. ")

                Me.OnOpened()

                If Me.IsDeviceOpen Then
                    Me.OnInitializing(e)
                    If e.Cancel Then Throw New OperationCanceledException($"{resourceTitle}:{resourceName} initialization canceled;. ")
                    Me.OnInitialized()
                Else
                    Throw New VI.Pith.OperationFailedException($"Opening {resourceTitle}:{resourceName} device failed;. ")
                End If

            Else
                Throw New VI.Pith.OperationFailedException($"Opening {resourceTitle}:{resourceName} session failed;. ")
            End If
            success = True
        Catch
            Throw
        Finally
            If Not success Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"{resourceName} failed connecting. Disconnecting.")
                Me.TryCloseSession()
            End If
        End Try
    End Sub

    ''' <summary> Try open session. </summary>
    ''' <param name="resourceName">  Name of the resource. </param>
    ''' <param name="resourceTitle"> The resource title. </param>
    ''' <param name="e">             Cancel details event information. </param>
    ''' <returns> <c>True</c> if success; <c>False</c> otherwise. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Function TryOpenSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal e As ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"opening {resourceTitle}:{resourceName}"
        Try
            Me.OpenSession(resourceName, resourceTitle)
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, e.Details)
        End Try
        Return Not e.Cancel AndAlso Me.IsDeviceOpen
    End Function

    ''' <summary> Allows the derived device to take actions before closing. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    ''' <remarks> This override should occur as the first call of the overriding method. </remarks>
    Protected Overridable Sub OnClosing(ByVal e As CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Not e.Cancel Then Me.SyncNotifyClosing(e)
        If Not e.Cancel Then
            Me.IsInitialized = False
            Me.SuspendPublishing()
        End If
    End Sub

    ''' <summary> Allows the derived device to take actions after closing. </summary>
    ''' <remarks> This override should occur as the last call of the overriding method. </remarks>
    Protected Overridable Sub OnClosed()
        Me.Session.ResourceNameInfo.Publish()
        Me.ResumePublishing()
        ' required to prevent property change errors when the device closes.
        Me.SafeSendPropertyChanged(NameOf(DeviceBase.IsDeviceOpen))
        Me.SyncNotifyClosed(System.EventArgs.Empty)
    End Sub

    ''' <summary> Closes the session. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Sub CloseSession()
        Try
            ' check if already closed.
            If Me.IsDisposed OrElse Not Me.IsDeviceOpen Then Return
            Dim e As New CancelEventArgs
            Me.OnClosing(e)
            If e.Cancel Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Close session canceled;. ")
            Else
                Me.RemoveServiceRequestEventHandler()
                Me.Session.DisableServiceRequest()
                Me.Session.CloseSession()
                Me.OnClosed()
            End If
        Catch ex As VI.Pith.NativeException
            Throw New VI.Pith.OperationFailedException("Failed closing the VISA session.", ex)
        Catch ex As Exception
            Throw New VI.Pith.OperationFailedException("Exception occurred closing the session.", ex)
        End Try
    End Sub

    ''' <summary> Try close session. </summary>
    ''' <returns> <c>True</c> if session closed; otherwise <c>False</c>. </returns>
    Public Overridable Function TryCloseSession() As Boolean
        Try
            Me.CloseSession()
        Catch ex As OperationFailedException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{ex.Message} occurred closing session;. {ex.ToFullBlownString}")
            Return False
        End Try
        Return Not Me.IsDeviceOpen
    End Function

#End Region

#Region " SESSIOn: OPEN / CLOSE STATUS SUBSYSTEM ONLY"

    ''' <summary> Allows the derived device to take actions before opening the status subsystem. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    ''' <remarks> This override should occur as the first call of the overriding method.
    '''           After this call, the parent class adds the subsystems. </remarks>
    Protected Overridable Sub OnStatusOpening(ByVal e As CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Me.Subsystems.Count > 1 Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} subsystem count {Me.Subsystems.Count} on opening;. ")
            Me.Subsystems.Clear()
            Me.Subsystems.Add(Me.StatusSubsystemBase)
        Else
            Me.IsInitialized = False
        End If
    End Sub

    ''' <summary> Allows the derived device status to take actions after opening. </summary>
    ''' <remarks>
    ''' This override should occur as the last call of the overriding method.
    ''' The subsystems are added as part of the <see cref="OnOpening(CancelEventArgs)"/> method.
    ''' </remarks>
    Protected Overridable Sub OnStatusOpened()

        Me.ApplySettings()
        Me.Session.CaptureSyncContext(Me.CapturedSyncContext)
        Me.Subsystems.CaptureSyncContext(Me.CapturedSyncContext)

        Me.SafePostPropertyChanged(NameOf(DeviceBase.IsDeviceOpen))
        ' 2016/01/18: this was done before adding listeners, which was useful when using the device
        ' as a class in a 'meter'. As a result, the actions taken when handling the Opened event, 
        ' such as Reset and Initialize do not get reported. 
        ' The solution was to add the device Initialize event to process publishing and initialization of device
        ' and subsystems.

        Me.SyncNotifyOpened(System.EventArgs.Empty)
    End Sub

    ''' <summary> Opens the session. </summary>
    ''' <remarks> Register the device trace notifier before opening the session. </remarks>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="resourceName"> Name of the resource. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Sub OpenStatusSession(ByVal resourceName As String, ByVal resourceTitle As String)
        Dim success As Boolean = False
        Try
            If Me.Enabled Then Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Opening session to {resourceName};. ")
            Me.BeforeOpening()
            Me.Session.OpenSession(resourceName, resourceTitle, Me.CapturedSyncContext)
            If Me.Session.IsSessionOpen Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Session open to {resourceName};. ")
            ElseIf Me.Session.Enabled Then
                Throw New VI.Pith.OperationFailedException($"Unable to open session to {resourceName};. ")
            ElseIf Not Me.IsDeviceOpen Then
                Throw New VI.Pith.OperationFailedException($"Unable to emulate {resourceName};. ")
            End If
            If Me.Session.IsSessionOpen OrElse (Me.IsDeviceOpen AndAlso Not Me.Session.Enabled) Then

                Dim e As New CancelEventArgs
                Me.OnStatusOpening(e)

                If e.Cancel Then
                    Throw New OperationCanceledException($"Opening {resourceTitle}:{resourceName} status canceled;. ")
                End If

                Me.OnStatusOpened()

                If Not Me.IsDeviceOpen Then
                    Throw New VI.Pith.OperationFailedException($"Opening {resourceTitle}:{resourceName} device status failed;. ")
                End If

            Else
                Throw New VI.Pith.OperationFailedException($"Opening {resourceTitle}:{resourceName} status session failed;. ")
            End If
            success = True
        Catch
            Throw
        Finally
            If Not success Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"{resourceName} failed connecting. Disconnecting.")
                Me.TryCloseStatusSession()
            End If
        End Try
    End Sub

    ''' <summary> Try open session. </summary>
    ''' <param name="resourceName">  Name of the resource. </param>
    ''' <param name="resourceTitle"> The resource title. </param>
    ''' <param name="e">             Cancel details event information. </param>
    ''' <returns> <c>True</c> if success; <c>False</c> otherwise. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Function TryOpenStatusSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal e As ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"opening {resourceTitle}:{resourceName}"
        Try
            Me.OpenStatusSession(resourceName, resourceTitle)
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, e.Details)
        End Try
        Return Not e.Cancel AndAlso Me.IsDeviceOpen
    End Function

    ''' <summary> Allows the derived device status subsystem to take actions before closing. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    ''' <remarks> This override should occur as the first call of the overriding method. </remarks>
    Protected Overridable Sub OnStatusClosing(ByVal e As CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Not e.Cancel Then Me.SyncNotifyClosing(e)
        If Not e.Cancel Then
            Me.IsInitialized = False
            Me.SuspendPublishing()
        End If
    End Sub

    ''' <summary> Allows the derived device status subsystem to take actions after closing. </summary>
    ''' <remarks> This override should occur as the last call of the overriding method. </remarks>
    Protected Overridable Sub OnStatusClosed()
        Me.ResumePublishing()
        Me.SafePostPropertyChanged(NameOf(DeviceBase.IsDeviceOpen))
    End Sub

    ''' <summary> Closes the session. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Sub CloseStatusSession()
        Try
            ' check if already closed.
            If Me.IsDisposed OrElse Not Me.IsDeviceOpen Then Return
            Dim e As New CancelEventArgs
            Me.OnStatusClosing(e)
            If e.Cancel Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Close status session canceled;. ")
            Else
                Me.Session.CloseSession()
                Me.OnStatusClosed()
            End If
        Catch ex As VI.Pith.NativeException
            Throw New VI.Pith.OperationFailedException("Failed closing the status session.", ex)
        Catch ex As Exception
            Throw New VI.Pith.OperationFailedException("Exception occurred closing the status session.", ex)
        End Try
    End Sub

    ''' <summary> Try close session. </summary>
    ''' <returns> <c>True</c> if session closed; otherwise <c>False</c>. </returns>
    Public Overridable Function TryCloseStatusSession() As Boolean
        Try
            Me.CloseStatusSession()
        Catch ex As OperationFailedException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{ex.Message} occurred closing status session;. {ex.ToFullBlownString}")
            Return False
        End Try
        Return Not Me.IsDeviceOpen
    End Function

#End Region

#Region " SESSION: PROPERTY CHANGES AND MESSAGES EVENTS  "

    Private _MessageNotificationLevel As isr.Core.Pith.NotifySyncLevel

    ''' <summary> Gets or sets the message notification level. </summary>
    ''' <value> The message notification level. </value>
    Public Property MessageNotificationLevel As isr.Core.Pith.NotifySyncLevel
        Get
            Return Me._MessageNotificationLevel
        End Get
        Set(value As isr.Core.Pith.NotifySyncLevel)
            If value <> Me.MessageNotificationLevel Then
                Me._MessageNotificationLevel = value
                If Me.Session IsNot Nothing Then
                    Me.Session.MessageNotificationLevel = value
                Else
                    Me.SafePostPropertyChanged()
                End If
            End If
        End Set
    End Property

    ''' <summary> Gets the session service request event enabled. </summary>
    ''' <value> The session service request event enabled. </value>
    Public ReadOnly Property SessionServiceRequestEventEnabled As Boolean
        Get
            Return Me.IsDeviceOpen AndAlso Me.Session.ServiceRequestEventEnabled
        End Get
    End Property

    ''' <summary> Handles Session property change. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal sender As Vi.Pith.SessionBase, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(VI.Pith.SessionBase.LastMessageReceived)
                    Dim value As String = sender.LastMessageReceived
                    If Not String.IsNullOrWhiteSpace(value) Then
                        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} sent: '{value.InsertCommonEscapeSequences}'")
                        Me.SafeSendPropertyChanged(propertyName)
                    End If
                Case NameOf(VI.Pith.SessionBase.LastMessageSent)
                    Dim value As String = sender.LastMessageSent
                    If Not String.IsNullOrWhiteSpace(value) Then
                        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} received: '{value}'")
                        Me.SafeSendPropertyChanged(propertyName)
                    End If
                Case NameOf(VI.Pith.SessionBase.IsDeviceOpen)
                    Me.SafeSendPropertyChanged(propertyName)
                Case Else
                    Me.SafeSendPropertyChanged(propertyName)
            End Select
        End If
    End Sub

    ''' <summary> Handles Session property change. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
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

#Region " SESSION: SERVICE REQUEST MANAGEMENT "

    Private _UsingSyncServiceRequestHandler As Boolean

    ''' <summary> Gets or sets the sentinel indicating how service request is handled: Synchronously or Asynchronously. </summary>
    ''' <value>  <c>True</c> if service request is handled Synchronously; <c>False</c> if Asynchronously. </value>
    Public Property UsingSyncServiceRequestHandler As Boolean
        Get
            Return Me._UsingSyncServiceRequestHandler
        End Get
        Set(value As Boolean)
            If value <> Me.UsingSyncServiceRequestHandler Then
                Me._UsingSyncServiceRequestHandler = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets the service request enable bitmask. </summary>
    ''' <value> The service request enable bitmask. </value>
    Public ReadOnly Property ServiceRequestEnableBitmask As VI.Pith.ServiceRequests
        Get
            If Me.IsDeviceOpen Then
                Return Me.Session.ServiceRequestEnableBitmask
            Else
                Return VI.Pith.ServiceRequests.None
            End If
        End Get
    End Property


    Dim _DeviceServiceRequestHandlerAdded As Boolean
    ''' <summary> Gets the device service request handler Added. </summary>
    ''' <value> The device service request handler registered. </value>
    Public Property DeviceServiceRequestHandlerAdded As Boolean
        Get
            Return Me._DeviceServiceRequestHandlerAdded
        End Get
        Protected Set(value As Boolean)
            Me._DeviceServiceRequestHandlerAdded = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Registers the device service request handler. </summary>
    ''' <remarks>
    ''' The Session service request handler must be registered and enabled before registering the
    ''' device event handler.
    ''' </remarks>
    Public Sub AddServiceRequestEventHandler()
        If Not Me.DeviceServiceRequestHandlerAdded Then
            AddHandler Me.Session.ServiceRequested, AddressOf Me.SessionBaseServiceRequested
            Me.DeviceServiceRequestHandlerAdded = True
        End If
    End Sub

    ''' <summary> Removes the device service request event handler. </summary>
    Public Sub RemoveServiceRequestEventHandler()
        If Me.DeviceServiceRequestHandlerAdded AndAlso Me.Session IsNot Nothing Then
            RemoveHandler Me.Session.ServiceRequested, AddressOf Me.SessionBaseServiceRequested
        End If
        Me.DeviceServiceRequestHandlerAdded = False
    End Sub

#End Region

#Region " SUBSYSTEMS "

    ''' <summary> Adds a subsystem. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub AddSubsystem(ByVal value As SubsystemBase)
        Me.Subsystems.Add(value)
    End Sub

    Private _Subsystems As SubsystemCollection
    ''' <summary> Enumerates the Presettable subsystems. </summary>
    Protected ReadOnly Property Subsystems As SubsystemCollection
        Get
            Return Me._Subsystems
        End Get
    End Property

    ''' <summary> Removes the subsystem described by value. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub RemoveSubsystem(ByVal value As SubsystemBase)
        Me.Subsystems.Remove(value)
    End Sub

#Region " STATUS "

    Private _StatusSubsystemBase As StatusSubsystemBase

    ''' <summary> Gets or sets the status subsystem. </summary>
    ''' <value> The status subsystem. </value>
    Public Property StatusSubsystemBase As StatusSubsystemBase
        Get
            Return Me._StatusSubsystemBase
        End Get
        Private Set(value As VI.StatusSubsystemBase)
            If Me._StatusSubsystemBase IsNot Nothing Then
                Me._Subsystems.Remove(Me._StatusSubsystemBase)
                RemoveHandler Me._StatusSubsystemBase.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
            End If
            Me._StatusSubsystemBase = value
            If Me._StatusSubsystemBase IsNot Nothing Then
                AddHandler Me._StatusSubsystemBase.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
                Me._Subsystems.Add(Me._StatusSubsystemBase)
            End If
        End Set
    End Property

    ''' <summary> Handles the subsystem property change. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(StatusSubsystemBase.ErrorAvailable)
            Case NameOf(StatusSubsystemBase.MessageAvailable)
                If subsystem.MessageAvailable Then
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Message available;. ")
                End If
            Case NameOf(StatusSubsystemBase.MeasurementAvailable)
                If subsystem.MeasurementAvailable Then
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measurement available;. ")
                End If
            Case NameOf(StatusSubsystemBase.ReadingDeviceErrors)
                If subsystem.ReadingDeviceErrors Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Reading device errors;. ")
                End If

            Case NameOf(StatusSubsystemBase.Identity)
                If Not String.IsNullOrWhiteSpace(subsystem.Identity) Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} identified;. as {subsystem.Identity}")
                    If Not String.IsNullOrWhiteSpace(subsystem.VersionInfo?.Model) Then Me.Session.ResourceNameInfo.ResourceTitle = subsystem.VersionInfo.Model
                End If

            Case NameOf(StatusSubsystemBase.DeviceErrorsReport)
                If Not String.IsNullOrWhiteSpace(subsystem.DeviceErrorsReport) Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"{Me.ResourceNameCaption} errors;. {subsystem.DeviceErrorsReport}")
                End If

            Case NameOf(StatusSubsystemBase.LastDeviceError)
                If subsystem.LastDeviceError?.IsError Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                       $"{Me.ResourceNameCaption} last error;. {subsystem.LastDeviceError.CompoundErrorMessage}")
                End If

        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(StatusSubsystemBase)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, StatusSubsystemBase), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#End Region

#Region " EVENTS: READ EVENT REGISTERS "

    ''' <summary> Publish last error. </summary>
    ''' <param name="eventType"> Type of the event. </param>
    Protected Overridable Sub PublishLastError(ByVal eventType As TraceEventType)
        If Me.StatusSubsystemBase.DeviceErrorQueue.Any Then
            Me.Talker.Publish(eventType, My.MyLibrary.TraceEventId,
                              $"{Me.ResourceNameCaption} last Error {Me.StatusSubsystemBase.LastDeviceError}")
        End If
    End Sub

    ''' <summary> Publish error event. </summary>
    ''' <param name="eventType"> Type of the event. </param>
    Private Sub PublishErrorEvent(ByVal eventType As TraceEventType)
        Me.ProcessErrorEvent()
        Me.PublishLastError(eventType)
    End Sub

    ''' <summary> Read service request and query device errors if errors. </summary>
    Protected Overridable Sub ProcessErrorEvent()
        Me.ReadServiceRequestRegister()
        Me.StatusSubsystemBase.SafeQueryDeviceErrors()
    End Sub

    ''' <summary> Reads service request register. </summary>
    Protected Overridable Sub ReadServiceRequestRegister()
        Me.StatusSubsystemBase.ReadServiceRequestStatus()
    End Sub

    ''' <summary> Reads event registers. </summary>
    Protected Overridable Sub ReadEventRegisters()
        Me.StatusSubsystemBase.ReadEventRegisters()
    End Sub

#End Region

#Region " EVENTS: PROCESS SERVICE REQUEST "

    Private _ServiceRequestFailureMessage As String

    ''' <summary> Gets or sets a message describing the service request failure. </summary>
    ''' <value> A message describing the service request failure. </value>
    Public Property ServiceRequestFailureMessage As String
        Get
            Return Me._ServiceRequestFailureMessage
        End Get
        Set(value As String)
            If String.IsNullOrEmpty(value) Then value = ""
            If Not String.Equals(value, Me.ServiceRequestFailureMessage) Then
                Me._ServiceRequestFailureMessage = value
                If Not String.IsNullOrWhiteSpace(Me.ServiceRequestFailureMessage) Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, Me.ServiceRequestFailureMessage)
                End If
            End If
            ' send every time
            Me.SafeSendPropertyChanged()
        End Set
    End Property

    ''' <summary> Reads the event registers after receiving a service request. </summary>
    Protected Overridable Sub ProcessServiceRequest()
        Me.ReadEventRegisters()
        Me.StatusSubsystemBase.SafeQueryDeviceErrors()
    End Sub

    ''' <summary> Reads the event registers after receiving a service request. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Function TryProcessServiceRequest() As Boolean
        Dim result As Boolean = True
        Try
            Me.ProcessServiceRequest()
        Catch ex As Exception
            Me.ServiceRequestFailureMessage = Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                                                $"{ex.Message} occurred processing service request;. {ex.ToFullBlownString}")
            result = False
        End Try
        Return result
    End Function

#End Region

#Region " EVENTS: SERVICE REQUESTED "

    ''' <summary> Occurs when service is requested. </summary>
    Public Event ServiceRequested As EventHandler(Of EventArgs)

    ''' <summary> Removes event handler. </summary>
    ''' <param name="value"> The handler. </param>
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

    ''' <summary> Session base service requested. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SessionBaseServiceRequested(ByVal sender As Object, ByVal e As EventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.Pith.SessionBase)} service request"
        Try
            Me.ApplyCapturedSyncContext()
            Me.TryProcessServiceRequest()
            Dim evt As EventHandler(Of EventArgs) = Me.ServiceRequestedEvent
            If Me.UsingSyncServiceRequestHandler Then
                evt?.SafeSend(Me)
            Else
                evt?.SafePost(Me)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " DEVICE OPEN, INITIALIZE AND CLOSE EVENTS "

#Region " OPENING "

    ''' <summary> Occurs when Opening. </summary>
    Public Event Opening As EventHandler(Of ComponentModel.CancelEventArgs)

    ''' <summary> Removes event handler. </summary>
    ''' <param name="value"> The handler. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveOpeningEventHandler(ByVal value As EventHandler(Of ComponentModel.CancelEventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.Opening, CType(d, EventHandler(Of ComponentModel.CancelEventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub


    ''' <summary> Safely and synchronously <see cref="SynchronizationContext.Send">sends</see> or
    ''' invokes the <see cref="Opening">Opening Event</see>. </summary>
    ''' <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
    Private Sub SyncNotifyOpening(ByVal e As ComponentModel.CancelEventArgs)
        Me.OpeningEvent.SafeSend(Me, e)
    End Sub

#End Region

#Region " OPENED "

    ''' <summary> Occurs when Opened. </summary>
    Public Event Opened As EventHandler(Of System.EventArgs)

    ''' <summary> Removes event handler. </summary>
    ''' <param name="value"> The handler. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveOpenedEventHandler(ByVal value As EventHandler(Of EventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.Opened, CType(d, EventHandler(Of EventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub

    ''' <summary> Safely and synchronously <see cref="SynchronizationContext.Send">sends</see> or
    ''' invokes the <see cref="Opened">Opened Event</see>. </summary>
    ''' <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
    Protected Overridable Sub SyncNotifyOpened(ByVal e As System.EventArgs)
        Me.OpenedEvent.SafeSend(Me, e)
    End Sub

#End Region

#Region " CLOSING "

    ''' <summary> Occurs when Closing. </summary>
    Public Event Closing As EventHandler(Of System.ComponentModel.CancelEventArgs)

    ''' <summary> Removes event handler. </summary>
    ''' <param name="value"> The handler. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveClosingEventHandler(ByVal value As EventHandler(Of System.ComponentModel.CancelEventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.Closing, CType(d, EventHandler(Of System.ComponentModel.CancelEventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub

    ''' <summary> Safely and synchronously <see cref="SynchronizationContext.Send">sends</see> or
    ''' invokes the <see cref="Closing">Closing Event</see>. </summary>
    ''' <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
    Private Sub SyncNotifyClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        Me.ClosingEvent.SafeSend(Me, e)
    End Sub

#End Region

#Region " CLOSED "

    ''' <summary> Occurs when Closed. </summary>
    Public Event Closed As EventHandler(Of System.EventArgs)

    ''' <summary> Removes event handler. </summary>
    ''' <param name="value"> The handler. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveClosedEventHandler(ByVal value As EventHandler(Of EventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.Closed, CType(d, EventHandler(Of EventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub

    ''' <summary> Safely and synchronously <see cref="SynchronizationContext.Send">sends</see> or
    ''' invokes the <see cref="Closed">Closed Event</see>. </summary>
    ''' <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
    Private Sub SyncNotifyClosed(ByVal e As System.EventArgs)
        Me.ClosedEvent.SafeSend(Me, e)
    End Sub

#End Region

#Region " INITIALIZING "

    ''' <summary> Occurs when Initializing. </summary>
    Public Event Initializing As EventHandler(Of System.ComponentModel.CancelEventArgs)

    ''' <summary> Removes event handler. </summary>
    ''' <param name="value"> The handler. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveInitializingEventHandler(ByVal value As EventHandler(Of System.ComponentModel.CancelEventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.Initializing, CType(d, EventHandler(Of System.ComponentModel.CancelEventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub

    ''' <summary> Safely and synchronously <see cref="SynchronizationContext.Send">sends</see> or
    ''' invokes the <see cref="Initializing">Initializing Event</see>. </summary>
    ''' <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
    Private Sub SyncNotifyInitializing(ByVal e As System.ComponentModel.CancelEventArgs)
        Me.InitializingEvent.SafeSend(Me, e)
    End Sub

#End Region

#Region " INITIALIZED "

    ''' <summary> Occurs when Initialized. </summary>
    Public Event Initialized As EventHandler(Of System.EventArgs)

    ''' <summary> Removes event handler. </summary>
    ''' <param name="value"> The handler. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveInitializedEventHandler(ByVal value As EventHandler(Of EventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.Initialized, CType(d, EventHandler(Of EventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub

    ''' <summary> Safely and synchronously <see cref="SynchronizationContext.Send">sends</see> or
    ''' invokes the <see cref="Initialized">Initialized Event</see>. </summary>
    ''' <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
    Private Sub SyncNotifyInitialized(ByVal e As System.EventArgs)
        Me.InitializedEvent.SafeSend(Me, e)
    End Sub

#End Region

#End Region

#Region " MY SETTINGS "

    ''' <summary> Applies the settings. </summary>
    Protected MustOverride Sub ApplySettings()

#End Region

#Region " I TALKER "

    ''' <summary> Constructor-Safe talker setter. </summary>
    ''' <param name="talker"> The talker. </param>
    Private Sub ConstructorSafeSetter(ByVal talker As ITraceMessageTalker)
        Me.Subsystems.AssignTalker(talker)
    End Sub

    ''' <summary> Gets the trace message talker. </summary>
    ''' <value> The trace message talker. </value>
    Public Overrides Property Talker As ITraceMessageTalker
        Get
            Return MyBase.Talker
        End Get
        Protected Set(value As ITraceMessageTalker)
            MyBase.Talker = value
            Me.ConstructorSafeSetter(value)
        End Set
    End Property

    ''' <summary> Identifies talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Removes the listener described by listener. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overrides Sub RemoveListener(ByVal listener As IMessageListener)
        MyBase.RemoveListener(listener)
        Me.Subsystems?.RemoveListener(listener)
    End Sub

    ''' <summary> Adds a listener. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overrides Sub AddListener(ByVal listener As IMessageListener)
        MyBase.AddListener(listener)
        Me.Subsystems?.AddListener(listener)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        Me.Subsystems?.ApplyListenerTraceLevel(listenerType, value)
        MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

    ''' <summary> Applies the trace level type to all talkers. </summary>
    ''' <param name="listenerType"> Type of the trace level. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyTalkerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        MyBase.ApplyTalkerTraceLevel(listenerType, value)
        Me.Subsystems?.ApplyTalkerTraceLevel(listenerType, value)
    End Sub

    ''' <summary> Applies the talker trace levels described by talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub ApplyTalkerTraceLevels(ByVal talker As ITraceMessageTalker)
        MyBase.ApplyTalkerTraceLevels(talker)
        Me.Subsystems?.ApplyTalkerTraceLevels(talker)
    End Sub

    ''' <summary> Applies the talker listeners trace levels described by talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub ApplyListenerTraceLevels(ByVal talker As ITraceMessageTalker)
        MyBase.ApplyListenerTraceLevels(talker)
        Me.Subsystems?.ApplyListenerTraceLevels(talker)
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
#Region " RESOURCE NAME PATTERN + FIND "

    ''' <summary> Gets information describing the resource name. </summary>
    ''' <value> Information describing the resource name. </value>
    Public ReadOnly Property ResourceNameInfo As VI.Pith.ResourceNameInfo

    ''' <summary> Builds minimal resources filter. </summary>
    ''' <returns> A String. </returns>
    Public Shared Function BuildMinimalResourcesFilter() As String
        Return VI.Pith.ResourceNamesManager.BuildInstrumentFilter(HardwareInterfaceType.Gpib,
                                                             HardwareInterfaceType.Tcpip,
                                                             HardwareInterfaceType.Usb)
    End Function

    Private Shared _DefaultResourcesFilter As String

    ''' <summary> Gets or sets the default resource filter. </summary>
    ''' <value> The default resource filter. </value>
    Public Shared Property DefaultResourcesFilter As String
        Get
            If String.IsNullOrWhiteSpace(DeviceBase._DefaultResourcesFilter) Then
                DeviceBase._DefaultResourcesFilter = VI.Pith.ResourceNamesManager.BuildInstrumentFilter

            End If
            Return DeviceBase._DefaultResourcesFilter
        End Get
        Set(value As String)
            DeviceBase._DefaultResourcesFilter = value
        End Set
    End Property

    Private _ResourcesFilter As String
    ''' <summary> Gets or sets the resources search pattern. </summary>
    ''' <value> The resources search pattern. </value>
    Public Property ResourcesFilter As String
        Get
            Return Me._ResourcesFilter
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.ResourcesFilter) Then
                Me._ResourcesFilter = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    Private _ResourceTitle As String
    ''' <summary> Gets the resource title. </summary>
    ''' <value> The resource title. </value>
    Public Property ResourceTitle As String
        Get
            If Me.Session Is Nothing Then
                If String.IsNullOrWhiteSpace(Me._ResourceTitle) Then
                    Return My.MySettings.Default.DefaultResourceTitle
                Else
                    Return Me._ResourceTitle
                End If
            Else
                Return Me.Session.ResourceTitle
            End If
        End Get
        Protected Set(value As String)
            If Not String.Equals(Me.ResourceTitle, value) Then
                Me._ResourceTitle = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    Private _ResourceName As String
    ''' <summary> Gets the name of the resource. </summary>
    Public Property ResourceName As String
        Get
            If Me.Session Is Nothing Then
                Return Me._ResourceName
            Else
                Return Me.Session.ResourceName
            End If
        End Get
        Protected Set(ByVal value As String)
            If Not String.Equals(Me.ResourceName, value) Then
                Me._ResourceName = value
                Me.SafeSendPropertyChanged()
                Me.SafeSendPropertyChanged(NameOf(DeviceBase.ResourceNameCaption))
            End If
        End Set
    End Property

    ''' <summary> Gets the resource name caption. </summary>
    ''' <value>
    ''' The <see cref="ResourceName"/> resource <see cref="VI.Pith.SessionBase.ResourceNameClosedCaption">closed
    ''' caption</see> if not open.
    ''' </value>
    Public ReadOnly Property ResourceNameCaption As String
        Get
            If Me.Session Is Nothing Then
                Return My.MySettings.Default.DefaultClosedResourceCaption
            Else
                Return Me.Session.ResourceNameCaption
            End If
        End Get
    End Property


#End Region
#End If
#End Region
