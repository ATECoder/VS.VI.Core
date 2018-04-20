Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions

''' <summary> A bridge meter. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="3/22/2018" by="David" revision=""> Created. </history>
Public Class BridgeMeter
    Inherits Core.Pith.PropertyPublisherTalkerBase

#Region " CONSTRUCTORS "

    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(BridgeMeterDevice.Create, True)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device">        The device. </param>
    ''' <param name="isDeviceOwner"> True if is device owner, false if not. </param>
    Public Sub New(ByVal device As Device, ByVal isDeviceOwner As Boolean)
        MyBase.New(Device.Validated(device).Talker, True)
        Me._New(device, isDeviceOwner)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device">        The device. </param>
    ''' <param name="isDeviceOwner"> True if is device owner, false if not. </param>
    Private Sub _New(ByVal device As Device, ByVal isDeviceOwner As Boolean)
        Me._AssignDevice(device, isDeviceOwner)
        Me._Bridge = New ChannelResistorCollection
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the isr.VI.Instrument.ResourcePanelBase and
    ''' optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me.IsDeviceOwner AndAlso Me.Device IsNot Nothing Then
                    Me._Device.Dispose() : Me._Device = Nothing
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " DEVICE "

    Private _Device As Device
    ''' <summary> Gets or sets the device. </summary>
    ''' <value> The device. </value>
    Public ReadOnly Property Device As Device
        Get
            Return Me._Device
        End Get
    End Property

    ''' <summary> Gets the is device assigned. </summary>
    ''' <value> The is device assigned. </value>
    Public Overridable ReadOnly Property IsDeviceAssigned As Boolean
        Get
            Return Me.Device IsNot Nothing AndAlso Not Me.Device.IsDisposed
        End Get
    End Property

    ''' <summary> Gets the is device that owns this item. </summary>
    ''' <value> The is device owner. </value>
    Public Property IsDeviceOwner As Boolean

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignDevice(ByVal value As Device, ByVal isDeviceOwner As Boolean)
        If Me.Device IsNot Nothing Then
            If Me.Device.Talker IsNot Nothing Then Me.Device.Talker.RemoveListeners()
            RemoveHandler Me.Device.PropertyChanged, AddressOf Me.DevicePropertyChanged
            RemoveHandler Me.Device.Opening, AddressOf Me.DeviceOpening
            RemoveHandler Me.Device.Opened, AddressOf Me.DeviceOpened
            RemoveHandler Me.Device.Closing, AddressOf Me.DeviceClosing
            RemoveHandler Me.Device.Closed, AddressOf Me.DeviceClosed
            RemoveHandler Me.Device.Initialized, AddressOf Me.DeviceInitialized
            RemoveHandler Me.Device.Initializing, AddressOf Me.DeviceInitializing
            ' this also closes the session. 
            If Me.IsDeviceOwner Then Me._Device.Dispose() : Me._Device = Nothing
        End If
        Me._Device = value
        If value IsNot Nothing Then
            value.CaptureSyncContext(WindowsFormsSynchronizationContext.Current)
            AddHandler Me.Device.PropertyChanged, AddressOf Me.DevicePropertyChanged
            AddHandler Me.Device.Opening, AddressOf Me.DeviceOpening
            AddHandler Me.Device.Opened, AddressOf Me.DeviceOpened
            AddHandler Me.Device.Closing, AddressOf Me.DeviceClosing
            AddHandler Me.Device.Closed, AddressOf Me.DeviceClosed
            AddHandler Me.Device.Initialized, AddressOf Me.DeviceInitialized
            AddHandler Me.Device.Initializing, AddressOf Me.DeviceInitializing
        End If
        Me._IsDeviceOwner = isDeviceOwner
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device, ByVal isDeviceOwner As Boolean)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Me._AssignDevice(value, isDeviceOwner)
        Me.AssignTalker(value.Talker)
        ' publish device state.  This should also report the open status and set the 
        ' control open/close button state.
        Me.Device.Publish()
        Me.ApplyListenerTraceLevel(ListenerType.Display, value.Talker.TraceShowLevel)
    End Sub

    Private Sub _ReleaseDevice()
        Me._Device = Nothing
    End Sub

    ''' <summary> Releases the device. </summary>
    ''' <remarks> Called from the base device to release the reference to the device. </remarks>
    Protected Sub ReleaseDevice()
        Me._ReleaseDevice()
    End Sub

    ''' <summary> Releases the device and reassigns the default device. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub RestoreDevice()
        Me.AssignDevice(Device.Create, True)
    End Sub

    ''' <summary> Gets the sentinel indicating if the device is open. </summary>
    ''' <value> The is device open. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected ReadOnly Property IsDeviceOpen As Boolean
        Get
            Return Me.Device IsNot Nothing AndAlso Me.Device.IsDeviceOpen
        End Get
    End Property

#Region " PROPERTY CHANGE "

    ''' <summary> Executes the device open changed action. </summary>
    Protected Sub OnDeviceOpenChanged()
        If Me.IsDeviceOpen Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Device.StatusSubsystem.VersionInfo.Model} is open")
            Dim e As New isr.Core.Pith.ActionEventArgs
            If Not Me.TryConfigureMeter(My.MySettings.Default.BridgeMeterPowerLineCycles, e) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Files configuring meter;. Details: {e.Details}")
            End If
        End If
    End Sub

    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Sub HandlePropertyChange(ByVal device As VI.DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K3700.Device.IsDeviceOpen)
                ' the open sentinel is turned on after all subsystems are set
                Me.OnDeviceOpenChanged()
        End Select
    End Sub

    ''' <summary> Event handler. Called for property changed events. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DevicePropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.Device)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, K3700.Device), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " OPENING / OPEN "

    ''' <summary> Attempts to open a session to the device using the specified resource name. </summary>
    ''' <param name="resourceName">  The name of the resource. </param>
    ''' <param name="resourceTitle"> The title. </param>
    ''' <param name="e">             Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:   DoNotCatchGeneralExceptionTypes")>
    Public Function TryOpenSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal e As ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"opening {resourceName} VISA session"
        Try
            Me.Device.TryOpenSession(resourceName, resourceTitle, e)
            If Not Me.Device.IsDeviceOpen Then
                e.RegisterCancellation($"failed {activity};. ")
            End If
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
        Finally
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Event handler. Called upon device opening so as to instantiated all subsystems. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Sub DeviceOpening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
    End Sub

    ''' <summary>
    ''' Event handler. Called after the device opened and all subsystems were defined.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim outcome As TraceEventType = TraceEventType.Information
        If Me.Device.Session.Enabled And Not Me.Device.Session.IsSessionOpen Then outcome = TraceEventType.Warning
        Me.Talker.Publish(outcome, My.MyLibrary.TraceEventId,
                          "{0} {1:enabled;enabled;disabled} and {2:open;open;closed}; session {3:open;open;closed};. ",
                          Me.Device.ResourceTitle,
                          Me.Device.Session.Enabled.GetHashCode,
                          Me.Device.Session.IsDeviceOpen.GetHashCode,
                          Me.Device.Session.IsSessionOpen.GetHashCode)
        AddHandler Me.Device.MultimeterSubsystem.PropertyChanged, AddressOf Me.MultimeterSubsystemPropertyChanged
        AddHandler Me.Device.ChannelSubsystem.PropertyChanged, AddressOf Me.ChannelSubsystemPropertyChanged
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
    End Sub

#End Region

#Region " INITALIZING / INITIALIZED  "

    Protected Overridable Sub DeviceInitializing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
    End Sub

    ''' <summary> Device initialized. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceInitialized(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ReadServiceRequestStatus()
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overridable Sub OnTitleChanged(ByVal value As String)
        Me.Title = value
    End Sub

    ''' <summary> Gets or sets the title. </summary>
    ''' <value> The title. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Title As String

    ''' <summary> Reads a service request status. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ReadServiceRequestStatus()
        Try
            Me.Device.StatusSubsystemBase.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading service request;. {0}", ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " CLOSING / CLOSED "

    ''' <summary> Attempts to close session from the given data. </summary>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:   DoNotCatchGeneralExceptionTypes")>
    Public Function TryCloseSession(ByVal e As ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"closing VISA session @{Me.Device.ResourceName}"
        Try
            Me.Device.TryCloseSession()
            If Me.Device.IsDeviceOpen Then
                e.RegisterCancellation($"Failed {activity};. ")
            End If
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
        Finally
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.Device IsNot Nothing Then
            If Me.Device.Session IsNot Nothing Then
                Me.Device.Session.DisableServiceRequest()
            End If
            If Me.IsDeviceOpen Then
                RemoveHandler Me.Device.MultimeterSubsystem.PropertyChanged, AddressOf Me.MultimeterSubsystemPropertyChanged
                RemoveHandler Me.Device.ChannelSubsystem.PropertyChanged, AddressOf Me.ChannelSubsystemPropertyChanged
                RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
                RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
            End If
        End If
    End Sub

    ''' <summary> Event handler. Called when device is closed. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceClosed(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.Device IsNot Nothing Then
            If Me.Device.Session.IsSessionOpen Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Device closed but session still open @{Me.Device.Session.ResourceName};. ")
            ElseIf Me.Device.Session.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Device closed but emulated session still open;. ")
            Else
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Disconnected;. Device access closed.")
            End If
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Disconnected;. Device disposed.")
        End If
    End Sub

#End Region

#End Region

#Region " SUBSYSTEMS "

#Region " MUTLIMETER "

    Private _MeasurementEnabled As Boolean
    Public Property MeasurementEnabled As Boolean
        Get
            Return Me._MeasurementEnabled
        End Get
        Set(value As Boolean)
            If value <> Me.MeasurementEnabled Then
                Me._MeasurementEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Enables the measurements. </summary>
    Public Sub EnableMeasurements()
        Me.MeasurementEnabled = Me.Device IsNot Nothing AndAlso Me.Device.IsDeviceOpen AndAlso
                                MultimeterFunctionMode.ResistanceFourWire = Me.Device.MultimeterSubsystem.FunctionMode.GetValueOrDefault(MultimeterFunctionMode.None) AndAlso
                                Me.Device.MultimeterSubsystem.PowerLineCycles.HasValue
    End Sub

    Private _PowerLineCycles As Double

    ''' <summary> Gets or sets the power line cycles. </summary>
    ''' <value> The power line cycles. </value>
    Public Property PowerLineCycles As Double
        Get
            Return Me._PowerLineCycles
        End Get
        Set(value As Double)
            If value <> Me.PowerLineCycles Then
                Me._PowerLineCycles = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Handles the Multimeter subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Sub HandlePropertyChange(ByVal subsystem As MultimeterSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K3700.MultimeterSubsystem.PowerLineCycles)
                If subsystem.PowerLineCycles.HasValue Then
                    Me.PowerLineCycles = subsystem.PowerLineCycles.Value
                End If
            Case NameOf(K3700.MultimeterSubsystem.FunctionMode)
                Me.EnableMeasurements()
        End Select
    End Sub

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MultimeterSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.MultimeterSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, K3700.MultimeterSubsystem), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " CHANNEL "

    Private _ClosedChannels As String
    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property ClosedChannels As String
        Get
            Return Me._ClosedChannels
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.ClosedChannels, StringComparison.OrdinalIgnoreCase) Then
                Me._ClosedChannels = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Handles the Channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub HandlePropertyChange(ByVal subsystem As ChannelSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K3700.ChannelSubsystem.ClosedChannels)
                Me.ClosedChannels = subsystem.ClosedChannels
        End Select
    End Sub

    ''' <summary> Channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.ChannelSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, K3700.ChannelSubsystem), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " STATUS "

    Private _LastError As String
    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property LastError As String
        Get
            Return Me._LastError
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.LastError, StringComparison.OrdinalIgnoreCase) Then
                Me._LastError = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property


    ''' <summary> Reports the last error. </summary>
    Protected Sub OnLastError(ByVal lastError As VI.DeviceError)
        If lastError IsNot Nothing Then
            Me.LastError = lastError.CompoundErrorMessage
        End If
    End Sub

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Sub HandlePropertyChange(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(StatusSubsystemBase.DeviceErrorsReport)
                Me.OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.LastDeviceError)
                Me.OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.ErrorAvailable)
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.StatusSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, K3700.StatusSubsystem), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " SYSTEM "

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub HandlePropertyChange(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.SystemSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, K3700.SystemSubsystem), e.PropertyName)
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

#Region " MEASURE "

    ''' <summary> Gets or sets the bridges. </summary>
    ''' <value> The bridge. </value>
    Public ReadOnly Property Bridge As ChannelResistorCollection

    ''' <summary> Configure meter. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="powerLineCycles"> The power line cycles. </param>
    Public Sub ConfigureMeter(ByVal powerLineCycles As Double)
        Dim activity As String = $"Checking {My.Settings.ResourceName} is open"
        If Me.IsDeviceOpen Then
            activity = $"Configuring function mode {MultimeterFunctionMode.ResistanceFourWire}"
            Dim expectedMeasureFunction As MultimeterFunctionMode = MultimeterFunctionMode.ResistanceFourWire
            Dim measureFunction As MultimeterFunctionMode? = Device.MultimeterSubsystem.ApplyFunctionMode(expectedMeasureFunction)
            If measureFunction.HasValue Then
                If expectedMeasureFunction <> measureFunction.Value Then
                    Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {expectedMeasureFunction } <> Actual {measureFunction.Value}")
                End If
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity}--no value set")
            End If
            activity = $"Configuring power line cycles {powerLineCycles}"
            Dim actualPowerLineCycles As Double? = Device.MultimeterSubsystem.ApplyPowerLineCycles(powerLineCycles)
            If actualPowerLineCycles.HasValue Then
                If powerLineCycles <> actualPowerLineCycles.Value Then
                    Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {powerLineCycles} <> Actual {actualPowerLineCycles.Value}")
                End If
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity}--no value set")
            End If
        Else
            Throw New VI.Pith.OperationFailedException($"Failed {activity}; VISA session to this device is not open")
        End If
    End Sub

    ''' <summary> Attempts to configure meter from the given data. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="powerLineCycles"> The power line cycles. </param>
    ''' <param name="e">               Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryConfigureMeter(ByVal powerLineCycles As Double, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"Configuring bridge meter {My.Settings.ResourceName}"
        Try
            Me.ConfigureMeter(powerLineCycles)
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Measure resistance. </summary>
    ''' <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    '''                                             null. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="resistor"> The resistor. </param>
    Public Sub MeasureResistance(ByVal resistor As ChannelResistor)
        If resistor Is Nothing Then Throw New ArgumentNullException(NameOf(resistor))
        Dim activity As String = $"measuring {resistor.Title}"
        If Me.Device.IsDeviceOpen Then
            activity = $"{resistor.Title} opening channels"
            Dim expectedChannelList As String = ""
            Dim actualChannelList As String = Me.Device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
            If expectedChannelList <> actualChannelList Then
                Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {expectedChannelList } <> Actual {actualChannelList}")
            End If

            activity = $"{resistor.Title} closing {resistor.ChannelList}"
            expectedChannelList = resistor.ChannelList
            actualChannelList = Device.ChannelSubsystem.ApplyClosedChannels(expectedChannelList, TimeSpan.FromSeconds(2))
            If expectedChannelList <> actualChannelList Then
                Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {expectedChannelList } <> Actual {actualChannelList}")
            End If

            activity = $"measuring {resistor.Title}"
            If Device.MultimeterSubsystem.Measure.HasValue Then
                resistor.Resistance = Device.MultimeterSubsystem.MeasuredValue.GetValueOrDefault(-1)
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity}; driver returned nothing")
            End If
        Else
            Throw New VI.Pith.OperationFailedException($"Failed {activity}; VISA session to this device is not open")
        End If
    End Sub

    ''' <summary> Attempts to measure resistance from the given data. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resistor"> The resistor. </param>
    ''' <param name="e">        Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryMeasureResistance(ByVal resistor As ChannelResistor, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If resistor Is Nothing Then Throw New ArgumentNullException(NameOf(resistor))
        Dim activity As String = $"measuring {resistor.Title}"
        Try
            Me.MeasureResistance(resistor)
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Measure bridge. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    Public Sub MeasureBridge()
        Dim activity As String = $"Measuring bridge at {My.Settings.ResourceName}"
        If Me.IsDeviceOpen Then
            For Each resistor As ChannelResistor In Me.Bridge
                activity = $"Measuring {resistor.Title} at {My.Settings.ResourceName}"
                Me.MeasureResistance(resistor)
            Next
        Else
            Throw New VI.Pith.OperationFailedException($"Failed {activity}; VISA session to this device is not open")
        End If
    End Sub

    ''' <summary> Attempts to measure bridge from the given data. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryMeasureBridge(ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"Measuring bridge at {My.Settings.ResourceName}"
        Try
            Me.MeasureBridge()
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

#End Region

End Class

