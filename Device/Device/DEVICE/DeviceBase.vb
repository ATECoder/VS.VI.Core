Imports System.Threading
Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.EventHandlerExtensions
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.Core.Pith.StopwatchExtensions
Imports isr.VI.National.Visa
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
    Inherits PropertyPublisherBase
    Implements IPresettablePropertyPublisher, ITalker

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="DeviceBase" /> class. </summary>
    Protected Sub New()
        Me.New(isr.VI.SessionFactory.Get.Factory.CreateSession())
        Me._IsSessionOwner = True
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="DeviceBase" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.SessionBase">message based
    ''' session</see>. </param>
    Protected Sub New(ByVal visaSession As VI.SessionBase)
        MyBase.New()
        If visaSession Is Nothing Then
            Me._Session = isr.VI.SessionFactory.Get.Factory.CreateSession()
            Me._IsSessionOwner = True
        Else
            Me._Session = visaSession
        End If
        Me._Subsystems = New SubsystemCollection
        Me._ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter
        Me._UsingSyncServiceRequestHandler = True
        Me._Talker = New TraceMessageTalker
        Me._ResourceTitle = DeviceBase.DefaultResourceTitle
        Me._ResourceName = DeviceBase.ResourceNameClosed
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
                Me.Talker.Listeners.Clear()
                Me._Talker = Nothing
                Me.SessionMessagesTraceEnabled = False
                Me.Session?.DisableServiceRequest()
                Me.RemoveServiceRequestEventHandler()
                Me.RemoveEventHandler(Me.ServiceRequestedEvent)
                Me.RemoveOpeningEventHandler(Me.OpeningEvent)
                Me.RemoveOpenedEventHandler(Me.OpenedEvent)
                Me.RemoveClosingEventHandler(Me.ClosingEvent)
                Me.RemoveClosedEventHandler(Me.ClosedEvent)
                Me.RemoveInitializingEventHandler(Me.InitializingEvent)
                Me.RemoveInitializedEventHandler(Me.InitializedEvent)
                If Me._IsSessionOwner Then
                    Try
                        Me.CloseSession()
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
                    End Try
                    Try
                        If Me._Session IsNot Nothing Then
                            Me.Session.Dispose()
                        End If
                        ' release the session
                        ' Trying to null the session raises an ObjectDisposedException 
                        ' if session service request handler was not released. 
                        Me._Session = Nothing
                    Catch ex As ObjectDisposedException
                        Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
                    End Try
                End If
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
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceName} Clearing active state")
        Me.StatusSubsystemBase.ClearActiveState()
        ' 20180105: report any errors
        Me.publishErrorEvent(TraceEventType.Warning)
    End Sub

    ''' <summary> Clears the queues and resets all registers to zero. Sets the subsystem properties to
    ''' the following CLS default values:<para>
    ''' </para> </summary>
    ''' <remarks> *CLS. </remarks>
    Public Overridable Sub ClearExecutionState() Implements IPresettable.ClearExecutionState
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceName} Clearing execution state")
        Me.Subsystems.ClearExecutionState()
        ' 20180105: report any errors
        Me.publishErrorEvent(TraceEventType.Warning)
    End Sub

    ''' <summary> Initializes the Device. Used after reset to set a desired initial state. </summary>
    ''' <remarks> Use this to customize the reset. </remarks>
    Public Overridable Sub InitKnownState() Implements IPresettable.InitKnownState
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceName} Initializing known state")
        Me.Subsystems.InitKnownState()
        Me.IsInitialized = True
        ' 20180105: report any errors
        Me.publishErrorEvent(TraceEventType.Warning)
    End Sub

    ''' <summary> Resets the Device to its known state. </summary>
    ''' <remarks> *RST. </remarks>
    Public Overridable Sub PresetKnownState() Implements IPresettable.PresetKnownState
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceName} Presetting known state")
        Me.Subsystems.PresetKnownState()
        ' 20180105: report any errors
        Me.publishErrorEvent(TraceEventType.Warning)
    End Sub

    ''' <summary> Resets the Device to its known state. </summary>
    ''' <remarks> *RST. </remarks>
    Public Overridable Sub ResetKnownState() Implements IPresettable.ResetKnownState
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceName} Resetting known state")
        Me.Subsystems.ResetKnownState()
        ' 20180105: report any errors
        Me.publishErrorEvent(TraceEventType.Warning)
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

#Region " SESSION "

    ''' <summary> Capture synchronization context. </summary>
    ''' <param name="syncContext"> Context for the synchronization. </param>
    Public Overrides Sub CaptureSyncContext(ByVal syncContext As Threading.SynchronizationContext)
        MyBase.CaptureSyncContext(syncContext)
        Me.Session?.CaptureSyncContext(syncContext)
        Me.Subsystems?.CaptureSyncContext(syncContext)
    End Sub

    ''' <summary> true if this object is session owner. </summary>
    Private _IsSessionOwner As Boolean

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As SessionBase, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.IsDeviceOpen)
                If sender.IsDeviceOpen Then
                    AddHandler Me.Session.PropertyChanged, AddressOf Me.SessionPropertyChanged
                Else
                    RemoveHandler Me.Session.PropertyChanged, AddressOf Me.SessionPropertyChanged
                End If
            Case NameOf(sender.ResourceTitle)
                Me.ResourceTitle = sender.ResourceTitle
            Case NameOf(sender.ResourceName)
                Me.ResourceName = sender.ResourceName
        End Select
    End Sub

    ''' <summary> Session property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Session_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _Session.PropertyChanged
        Try
            Me.OnPropertyChanged(TryCast(sender, SessionBase), e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling property Session.{e.PropertyName} change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> The session. </summary>
    Private WithEvents _Session As SessionBase

    ''' <summary> Gets the session. </summary>
    ''' <value> The session. </value>
    Public ReadOnly Property Session As SessionBase
        Get
            Return Me._Session
        End Get
    End Property

    ''' <summary> Gets or sets the default resource title. </summary>
    Public Const DefaultResourceTitle As String = "Device"

    Private _ResourceTitle As String
    ''' <summary> Gets the resource title. </summary>
    ''' <value> The resource title. </value>
    Public Property ResourceTitle As String
        Get
            Return Me._ResourceTitle
        End Get
        Set(value As String)
            If Not String.Equals(Me.ResourceTitle, value) Then
                Me._ResourceTitle = value
                Me.SafePostPropertyChanged()
            End If
            If Me.Session IsNot Nothing Then Me.Session.ResourceTitle = value
        End Set
    End Property

    ''' <summary> Gets or sets the resource name closed. </summary>
    Public Const ResourceNameClosed As String = "<closed>"

    Private _ResourceName As String
    ''' <summary> Gets the name of the resource. </summary>
    ''' <value> The name of the resource or &lt;closed&gt; if not open. </value>
    Public Property ResourceName As String
        Get
            Return Me._ResourceName
        End Get
        Set(ByVal value As String)
            If Not String.Equals(Me.ResourceName, value) Then
                Me._ResourceName = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#Region " SESSION INITIALIZATION PROPERTIES "

    ''' <summary> Gets the bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Protected Overridable ReadOnly Property ErrorAvailableBits() As ServiceRequests = ServiceRequests.ErrorAvailable

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

#Region " RESOURCE NAME PATTERN "

    ''' <summary> Builds minimal resources filter. </summary>
    ''' <returns> A String. </returns>
    Public Shared Function BuildMinimalResourcesFilter() As String
        Return VI.ResourceNamesManager.BuildInstrumentFilter(HardwareInterfaceType.Gpib,
                                                             HardwareInterfaceType.Tcpip,
                                                             HardwareInterfaceType.Usb)
    End Function

    Private Shared _DefaultResourcesFilter As String

    ''' <summary> Gets or sets the default resource filter. </summary>
    ''' <value> The default resource filter. </value>
    Public Shared Property DefaultResourcesFilter As String
        Get
            If String.IsNullOrWhiteSpace(DeviceBase._DefaultResourcesFilter) Then
                DeviceBase._DefaultResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter

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
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Checks if the specified resource name exists. Use for checking if the instrument is open. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function Find(ByVal resourceName As String) As Boolean
        If String.IsNullOrWhiteSpace(resourceName) Then Throw New ArgumentNullException(NameOf(resourceName))
        Using rm As ResourcesManagerBase = isr.VI.SessionFactory.Get.Factory.CreateResourcesManager()
            If String.IsNullOrWhiteSpace(Me.ResourcesFilter) Then
                Return rm.FindResources().ToArray.Contains(resourceName)
            Else
                Return rm.FindResources(Me.ResourcesFilter).ToArray.Contains(resourceName)
            End If
        End Using
    End Function

#End Region

#Region " OPEN / CLOSE "

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
                                   $"{Me.ResourceTitle} {IIf(Me.IsInitialized, "initialized", "not ready")};. ")
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
                Me.SafePostPropertyChanged()
                Windows.Forms.Application.DoEvents()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   $"{Me.ResourceTitle} {IIf(Me.Enabled, "enabled", "disabled")};. ")
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating whether the device is open. See also
    ''' <see cref="SessionBase.IsDeviceOpen"/>.
    ''' </summary>
    ''' <value> <c>True</c> if the device has an open session; otherwise, <c>False</c>. </value>
    Public ReadOnly Property IsDeviceOpen As Boolean
        Get
            Return Me.Session IsNot Nothing AndAlso Me.Session.IsDeviceOpen
        End Get
    End Property

    ''' <summary> Initializes the session prior to opening access to the instrument. </summary>
    Protected Overridable Sub OnCreated()
        Me.Session.KeepAliveInterval = Me.KeepAliveInterval
        Me.Session.IsAliveCommand = Me.IsAliveCommand
        Me.Session.IsAliveQueryCommand = Me.IsAliveQueryCommand
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    ''' <remarks> This override should occur as the first call of the overriding method.
    '''           After this call, the parent class adds the subsystems. </remarks>
    Protected Overridable Sub OnOpening(ByVal e As ComponentModel.CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Me.Subsystems?.Any Then
            e.Cancel = True
            Dim msg As String = Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                                   $"Aborting attempt to add subsystems on top of existing subsystems for resource '{ResourceName}';. ")
            Debug.Assert(Not Debugger.IsAttached, msg)
        Else
            Me.IsInitialized = False
            Me.SuspendPublishing()
            Me.SyncNotifyOpening(e)
        End If
    End Sub

    ''' <summary> Allows the derived device to take actions after opening. </summary>
    ''' <remarks>
    ''' This override should occur as the last call of the overriding method.
    ''' The subsystems are added as part of the <see cref="OnOpening(CancelEventArgs)"/> method.
    ''' </remarks>
    Protected Overridable Sub OnOpened()

        Me.ApplySettings()
        Me.Session.CaptureSyncContext(Me.CapturedSyncContext)
        Me.Subsystems.CaptureSyncContext(Me.CapturedSyncContext)

        ' reset and clear the device to remove any existing errors.
        Me.StatusSubsystemBase.OnDeviceOpen()

        Me.SafePostPropertyChanged(NameOf(Me.IsDeviceOpen))
        ' 2016/01/18: this was done before adding listeners, which was useful when using the device
        ' as a class in a 'meter'. As a result, the actions taken when handling the Opened event, 
        ' such as Reset and Initialize do not get reported. 
        ' The solution was to add the device Initialize event to process publishing and initialization of device
        ' and subsystems.

        Me.SyncNotifyOpened(System.EventArgs.Empty)
    End Sub

    ''' <summary> Executes the initializing actions. </summary>
    Protected Overridable Sub OnInitializing(ByVal e As ComponentModel.CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Me.SyncNotifyInitializing(e)

        ' add listens and enable publishing while initializing the device.
        Me.AddSubsystemListeners()

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
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="resourceName"> Name of the resource. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Sub OpenSession(ByVal resourceName As String, ByVal resourceTitle As String)
        Dim success As Boolean = False
        Try
            If Me.Enabled Then Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Opening session to {resourceName};. ")
            Me.Session.ResourceTitle = resourceTitle
            Me.OnCreated()
            Me.Session.OpenSession(resourceName, resourceTitle, Me.CapturedSyncContext)
            If Me.Session.IsSessionOpen Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Session open to {resourceName};. ")
            ElseIf Me.Session.Enabled Then
                Throw New OperationFailedException($"Unable to open session to {resourceName};. ")
            ElseIf Not Me.IsDeviceOpen Then
                Throw New OperationFailedException($"Unable to emulate {resourceName};. ")
            End If
            If Me.Session.IsSessionOpen OrElse (Me.IsDeviceOpen AndAlso Not Me.Session.Enabled) Then

                Dim e As New ComponentModel.CancelEventArgs
                Me.OnOpening(e)

                If e.Cancel Then
                    Throw New OperationCanceledException($"Opening {resourceTitle}:{resourceName} canceled;. ")
                End If

                Me.OnOpened()

                If Me.IsDeviceOpen Then

                    Me.OnInitializing(e)
                    If e.Cancel Then Throw New OperationCanceledException($"{resourceTitle}:{resourceName} initialization canceled;. ")

                    Me.OnInitialized()

                Else
                    Throw New OperationFailedException($"Opening {resourceTitle}:{resourceName} device failed;. ")
                End If

            Else
                Throw New OperationFailedException($"Opening {resourceTitle}:{resourceName} session failed;. ")
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
    Public Overridable Function TryOpenSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal e As CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim action As String = $"opening {resourceTitle}:{resourceName}"
        Try
            Me.OpenSession(resourceName, resourceTitle)
        Catch ex As Exception
            e.RegisterCancellation($"Exception {action};. {ex.ToFullBlownString}")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, e.Details)
        End Try
        Return Not e.Cancel AndAlso Me.IsDeviceOpen
    End Function

    ''' <summary> Allows the derived device to take actions before closing. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    ''' <remarks> This override should occur as the first call of the overriding method. </remarks>
    Protected Overridable Sub OnClosing(ByVal e As ComponentModel.CancelEventArgs)
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
        Me.ResumePublishing()
        Me.SafePostPropertyChanged(NameOf(Me.IsDeviceOpen))
        Me.SyncNotifyClosed(System.EventArgs.Empty)
    End Sub

    ''' <summary> Closes the session. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Sub CloseSession()
        Try
            ' check if already closed.
            If Me.IsDisposed OrElse Not Me.IsDeviceOpen Then Return
            Dim e As New ComponentModel.CancelEventArgs
            Me.OnClosing(e)
            If e.Cancel Then Return
            Me.RemoveServiceRequestEventHandler()
            Me.Session.DisableServiceRequest()
            Me.Session.CloseSession()
            Me.OnClosed()
        Catch ex As NativeException
            Throw New OperationFailedException("Failed closing the VISA session.", ex)
        Catch ex As Exception
            Throw New OperationFailedException("Exception occurred closing the session.", ex)
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

#Region " SESSION: PROPERTY CHANGES AND MESSAGES EVENTS  "

    ''' <summary> Gets or sets the session messages trace enabled. </summary>
    ''' <value> The session messages trace enabled. </value>
    Public Property SessionMessagesTraceEnabled As Boolean
        Get
            Return Me.IsDeviceOpen AndAlso Me.Session.SessionMessagesTraceEnabled
        End Get
        Set(value As Boolean)
            If Me.IsDeviceOpen AndAlso value <> Me.SessionMessagesTraceEnabled Then
                Me.Session.SessionMessagesTraceEnabled = value
                Me.SafePostPropertyChanged()
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

    ''' <summary> Session property changed. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub SessionPropertyChanged(ByVal sender As SessionBase, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(sender.SessionMessagesTraceEnabled)
                    Me.SafePostPropertyChanged(NameOf(Me.SessionMessagesTraceEnabled))
                Case NameOf(sender.ServiceRequestEventEnabled)
                    Me.SafePostPropertyChanged(NameOf(Me.SessionMessagesTraceEnabled))
                Case NameOf(sender.ServiceRequestEnableBitmask)
                    Me.SafePostPropertyChanged(NameOf(Me.ServiceRequestEnableBitmask))
                Case NameOf(sender.LastMessageReceived)
                    Dim value As String = sender.LastMessageReceived
                    If Not String.IsNullOrWhiteSpace(value) Then
                        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                               "{0} sent: '{1}'.", Me.ResourceName, value.InsertCommonEscapeSequences)
                    End If
                Case NameOf(sender.LastMessageSent)
                    Dim value As String = sender.LastMessageSent
                    If Not String.IsNullOrWhiteSpace(value) Then
                        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                               "{0} received: '{1}'.", Me.ResourceName, value)
                    End If
            End Select
        End If
    End Sub

    ''' <summary> Session property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SessionPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.SessionPropertyChanged(CType(sender, SessionBase), e.PropertyName)
            End If
        Catch ex As Exception
            If e Is Nothing Then
                Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}",
                         "Null event arguments", ex.ToFullBlownString)
            ElseIf String.IsNullOrEmpty(e.PropertyName) Then
                Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}",
                         "Empty", ex.ToFullBlownString)
            Else

                Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}.", e.PropertyName, ex.ToFullBlownString)
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
    Public ReadOnly Property ServiceRequestEnableBitmask As ServiceRequests
        Get
            Return Me.Session.ServiceRequestEnableBitmask
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
            AddHandler Me.Session.ServiceRequested, AddressOf Me.OnSessionBaseServiceRequested
            Me.DeviceServiceRequestHandlerAdded = True
        End If
    End Sub

    ''' <summary> Removes the device service request event handler. </summary>
    Public Sub RemoveServiceRequestEventHandler()
        If Me.DeviceServiceRequestHandlerAdded Then
            RemoveHandler Me.Session.ServiceRequested, AddressOf Me.OnSessionBaseServiceRequested
            Me.DeviceServiceRequestHandlerAdded = False
        End If
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
        Set(value As StatusSubsystemBase)
            If Me._StatusSubsystemBase IsNot Nothing Then
                RemoveHandler Me._StatusSubsystemBase.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
            End If
            Me._StatusSubsystemBase = value
            If Me._StatusSubsystemBase IsNot Nothing Then
                AddHandler Me._StatusSubsystemBase.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
            End If
        End Set
    End Property

    ''' <summary> Executes the subsystem property changed action. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Protected Overridable Sub OnPropertyChanged(ByVal subsystem As StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.ErrorAvailable)
                If Not subsystem.ReadingDeviceErrors AndAlso subsystem.ErrorAvailable Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Error available;. ")
                    subsystem.QueryDeviceErrors()
                End If
            Case NameOf(subsystem.MessageAvailable)
                If subsystem.MessageAvailable Then
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Message available;. ")
                End If
            Case NameOf(subsystem.MeasurementAvailable)
                If subsystem.MeasurementAvailable Then
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measurement available;. ")
                End If
            Case NameOf(subsystem.ReadingDeviceErrors)
                If subsystem.ReadingDeviceErrors Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Reading device errors;. ")
                End If

            Case NameOf(subsystem.Identity)
                If Not String.IsNullOrWhiteSpace(subsystem.Identity) Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceName} identified;. as {subsystem.Identity}")
                    If Not String.IsNullOrWhiteSpace(subsystem.VersionInfo?.Model) Then Me.ResourceTitle = subsystem.VersionInfo.Model
                End If

            Case NameOf(subsystem.DeviceErrors)
                If Not String.IsNullOrWhiteSpace(subsystem.DeviceErrors) Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"{Me.ResourceName} errors;. {subsystem.DeviceErrors}")
                End If

            Case NameOf(subsystem.LastDeviceError)
                If subsystem.LastDeviceError?.IsError Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                       $"{Me.ResourceName} last error;. {subsystem.LastDeviceError.CompoundErrorMessage}")
                End If

        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If e Is Nothing Then Return
        Dim action As String = $"handling property {NameOf(VI.StatusSubsystemBase)}.{e.PropertyName} changed event"
        Try
            Me.OnPropertyChanged(TryCast(sender, StatusSubsystemBase), e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {action};. { ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#End Region

#Region " EVENTS: READ EVENT REGISTERS "

    ''' <summary> Safe query existing device errors. </summary>
    Public Overridable Function QueryExistingDeviceErrors(ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If Me.StatusSubsystemBase.MessageAvailable Then
            ' if the device has message with an error state, the message must be fetched before the device
            ' errors can be fetched. The system requesting the message must:
            ' (1) fetch the message;
            ' (2) fetch the device errors.
            e.RegisterCancellation($"{Me.ResourceName} as a message available in the presence of a device error; the message needs to be fetched before fetching the device errors")
        Else
            Me.StatusSubsystemBase.QueryDeviceErrors()
            Me.StatusSubsystemBase.QueryLastError()
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> Queries device errors. </summary>
    Protected Sub SafeQueryDeviceErrors()
        If Me.StatusSubsystemBase.ErrorAvailable Then
            Dim e As New CancelDetailsEventArgs
            If Not Me.QueryExistingDeviceErrors(e) Then
                Me.ServiceRequestFailureMessage = Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, e.Details)
            End If
        End If
    End Sub

    ''' <summary> Publish last error. </summary>
    ''' <param name="eventType"> Type of the event. </param>
    Protected Overridable Sub PublishLastError(ByVal eventType As TraceEventType)
        If Me.StatusSubsystemBase.DeviceErrorQueue.Any Then
            Me.Talker.Publish(eventType, My.MyLibrary.TraceEventId,
                              $"{Me.ResourceName} last Error {Me.StatusSubsystemBase.LastDeviceError}")
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
        Me.SafeQueryDeviceErrors()
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
            Me._ServiceRequestFailureMessage = value
            If Not String.IsNullOrWhiteSpace(Me.ServiceRequestFailureMessage) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, Me.ServiceRequestFailureMessage)
            End If
        End Set
    End Property

    ''' <summary> Reads the event registers after receiving a service request. </summary>
    Protected Overridable Sub ProcessServiceRequest()
        Me.ReadEventRegisters()
        Me.SafeQueryDeviceErrors()
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

#Region " EVENT HANDLERS "

    ''' <summary> Synchronously handles the Service Request event of the <see cref="Session">session</see> control. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      The <see cref="EventArgs" /> instance
    ''' containing the event data. </param>
    Private Sub OnSessionBaseServiceRequested(ByVal sender As Object, ByVal e As EventArgs)
        Me.ApplyCapturedSyncContext()
        Me.TryProcessServiceRequest()
        If Me.UsingSyncServiceRequestHandler Then
            Me.SafeSendPropertyChanged()
        Else
            Me.SafePostPropertyChanged()
        End If
    End Sub

#End Region

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

#Region " TALKER "

    ''' <summary> Gets the trace message talker. </summary>
    ''' <value> The trace message talker. </value>
    Public ReadOnly Property Talker As ITraceMessageTalker

    ''' <summary> Adds a listener. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overridable Sub AddListener(ByVal listener As IMessageListener) Implements ITalker.AddListener
        Me.Talker.Listeners.Add(listener)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overridable Sub AddListeners(ByVal listeners As IEnumerable(Of IMessageListener)) Implements ITalker.AddListeners
        Me.Talker.Listeners.Add(listeners)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overridable Sub AddListeners(ByVal talker As ITraceMessageTalker) Implements ITalker.AddListeners
        Me.Talker.AddListeners(talker)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType) Implements ITalker.ApplyListenerTraceLevel
        Me.Talker.ApplyListenerTraceLevel(listenerType, value)
        Me.Subsystems.ApplyListenerTraceLevel(listenerType, value)
    End Sub

    ''' <summary> Applies the trace level type to all talkers. </summary>
    ''' <param name="listenerType"> Type of the trace level. </param>
    ''' <param name="value">        The value. </param>
    Public Overridable Sub ApplyTalkerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType) Implements ITalker.ApplyTalkerTraceLevel
        Me.Talker.ApplyTalkerTraceLevel(listenerType, value)
        Me.Subsystems.ApplyTalkerTraceLevel(listenerType, value)
    End Sub

    ''' <summary> Applies the talker trace levels described by talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overridable Sub ApplyTalkerTraceLevels(ByVal talker As ITraceMessageTalker) Implements ITalker.ApplyTalkerTraceLevels
        If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
        Me.ApplyTalkerTraceLevel(ListenerType.Logger, talker.TraceLogLevel)
        Me.ApplyTalkerTraceLevel(ListenerType.Display, talker.TraceShowLevel)
    End Sub

    ''' <summary> Adds subsystem listeners. </summary>
    Public Overridable Sub AddSubsystemListeners()
        Me.Subsystems.ApplyTalkerTraceLevel(ListenerType.Logger, Me.Talker.TraceLogLevel)
        Me.Subsystems.ApplyTalkerTraceLevel(ListenerType.Display, Me.Talker.TraceShowLevel)
        Me.Subsystems.AddListeners(Me.Talker)
    End Sub

    ''' <summary> Clears the listeners. </summary>
    Public Overridable Sub ClearListeners() Implements ITalker.ClearListeners
        Me.Talker.Listeners?.Clear()
        Me.Subsystems.ClearListeners()
    End Sub

    ''' <summary> Clears the subsystem listeners. </summary>
    Public Overridable Sub ClearSubsystemListeners()
        Me.Subsystems?.ClearListeners()
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
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
#End If
#End Region
