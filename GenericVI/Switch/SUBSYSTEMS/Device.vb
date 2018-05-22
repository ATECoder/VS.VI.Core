Imports isr.VI.ExceptionExtensions
''' <summary> Implements a generic switch device. </summary>
''' <remarks> An instrument is defined, for the purpose of this library, as a device with a front
''' panel. </remarks>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/10/2013" by="David" revision="3.0.5001"> Created. </history>
Public Class Device
    Inherits VI.DeviceBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="Device" /> class. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(StatusSubsystem.Create)
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <param name="statusSubsystem"> The Status Subsystem. </param>
    Protected Sub New(ByVal statusSubsystem As StatusSubsystem)
        MyBase.New(statusSubsystem)
        AddHandler My.Settings.PropertyChanged, AddressOf Me._Settings_PropertyChanged
        Me.StatusSubsystem = statusSubsystem
    End Sub

#Region "IDisposable Support"

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_TriggerSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_RouteSubsystem", Justification:="@Closing")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me.IsDeviceOpen Then
                    Me.OnClosing(New ComponentModel.CancelEventArgs)
                    Me.StatusSubsystem = Nothing
                End If
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception disposing device", "Exception {0}", ex.ToFullBlownString)
        Finally


            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Initializes the Device. Used after reset to set a desired initial state. </summary>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All)
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        Me.Subsystems.Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#End Region

#Region " SESSION "

    ''' <summary>
    ''' Allows the derived device to take actions before closing. Removes subsystems and event
    ''' handlers.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnClosing(ByVal e As ComponentModel.CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnClosing(e)
        If Not e.Cancel Then
            Me.RouteSubsystem = Nothing
            Me.TriggerSubsystem = Nothing
            Me.SystemSubsystem = Nothing
        End If
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnOpening(ByVal e As ComponentModel.CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnOpening(e)
        If Not e.Cancel Then
            Me.SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
            Me.RouteSubsystem = New RouteSubsystem(Me.StatusSubsystem)
            Me.TriggerSubsystem = New TriggerSubsystem(Me.StatusSubsystem)
        End If
    End Sub

#End Region

#Region " STATUS SESSION "

    ''' <summary>
    ''' Allows the derived device status subsystem to take actions before closing. Removes subsystems
    ''' and event handlers.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnStatusClosing(ByVal e As ComponentModel.CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnStatusClosing(e)
        If Not e.Cancel Then
        End If
    End Sub

    ''' <summary> Allows the derived device status subsystem to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnStatusOpening(ByVal e As ComponentModel.CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnStatusOpening(e)
        If Not e.Cancel Then
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

    Private _RouteSubsystem As RouteSubsystem
    ''' <summary>
    ''' Gets or sets the Route Subsystem.
    ''' </summary>
    ''' <value>The Route Subsystem.</value>
    Public Property RouteSubsystem As RouteSubsystem
        Get
            Return Me._RouteSubsystem
        End Get
        Set(value As RouteSubsystem)
            If Me._RouteSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.RouteSubsystem)
                Me.RouteSubsystem.Dispose()
                Me._RouteSubsystem = Nothing
            End If
            Me._RouteSubsystem = value
            If Me._RouteSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.RouteSubsystem)
            End If
        End Set
    End Property

    Private _TriggerSubsystem As TriggerSubsystem
    ''' <summary>
    ''' Gets or sets the Trigger Subsystem.
    ''' </summary>
    ''' <value>The Trigger Subsystem.</value>
    Public Property TriggerSubsystem As TriggerSubsystem
        Get
            Return Me._TriggerSubsystem
        End Get
        Set(value As TriggerSubsystem)
            If Me._TriggerSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.TriggerSubsystem)
                Me.TriggerSubsystem.Dispose()
                Me._TriggerSubsystem = Nothing
            End If
            Me._TriggerSubsystem = value
            If Me._TriggerSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.TriggerSubsystem)
            End If
        End Set
    End Property

#Region " STATUS "

    Private _StatusSubsystem As StatusSubsystem
    ''' <summary>
    ''' Gets or sets the Status Subsystem.
    ''' </summary>
    ''' <value>The Status Subsystem.</value>
    Public Property StatusSubsystem As StatusSubsystem
        Get
            Return Me._StatusSubsystem
        End Get
        Set(value As StatusSubsystem)
            If Me._StatusSubsystem IsNot Nothing Then
                RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
                Me._StatusSubsystem = Nothing
            End If
            Me._StatusSubsystem = value
            If Me._StatusSubsystem IsNot Nothing Then
                AddHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
            End If
        End Set
    End Property

    ''' <summary> Handles the subsystem property change. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub HandlePropertyChange(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        MyBase.HandlePropertyChange(subsystem, propertyName)
        If Me.StatusSubsystem IsNot Nothing And Not String.IsNullOrWhiteSpace(propertyName) Then
            Me.HandlePropertyChange(CType(subsystem, StatusSubsystem), propertyName)
        End If
    End Sub

    ''' <summary> Handles the subsystem property change. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As StatusSubsystem, ByVal propertyName As String)
        MyBase.HandlePropertyChange(subsystem, propertyName)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overloads Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Switch.StatusSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, Switch.StatusSubsystem), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#Region " SERVICE REQUEST "

    ''' <summary>
    ''' Enables or disables the operation service status registers requesting
    ''' a service request upon a negative transition.
    ''' </summary>
    ''' <param name="isOn">if set to <c>True</c> [is on].</param>
    Public Sub ToggleEndOfScanService(ByVal isOn As Boolean)
        Me.ToggleEndOfScanService(isOn, VI.Pith.ServiceRequests.All And (Not VI.Pith.ServiceRequests.MessageAvailable))
    End Sub

    ''' <summary>
    ''' Enables or disables the operation service status registers requesting
    ''' a service request upon a negative transition.
    ''' </summary>
    ''' <param name="turnOn">True to turn on or false to turn off the service request.</param>
    ''' <param name="serviceRequestMask">Specifies the
    ''' <see cref="VI.Pith.ServiceRequests">service request flags</see></param>
    Public Sub ToggleEndOfScanService(ByVal turnOn As Boolean, ByVal serviceRequestMask As VI.Pith.ServiceRequests)
        If turnOn Then
            Me.StatusSubsystem.ApplyOperationNegativeTransitionEventEnableBitmask(OperationTransitionEvents.Settling)
            Me.StatusSubsystem.ApplyOperationPositiveTransitionEventEnableBitmask(OperationTransitionEvents.None)
            Me.StatusSubsystem.ApplyOperationEventEnableBitmask(OperationEvents.Settling)
        Else
            Me.StatusSubsystem.ApplyOperationNegativeTransitionEventEnableBitmask(OperationTransitionEvents.None)
            Me.StatusSubsystem.ApplyOperationPositiveTransitionEventEnableBitmask(OperationTransitionEvents.None)
            Me.StatusSubsystem.ApplyOperationEventEnableBitmask(OperationEvents.None)
        End If
        Me.ToggleServiceRequest(turnOn, serviceRequestMask)
    End Sub

    ''' <summary>
    ''' Enables or disables service requests.
    ''' </summary>
    ''' <param name="turnOn">True to turn on or false to turn off the service request.</param>
    ''' <param name="serviceRequestMask">Specifies the
    ''' <see cref="VI.Pith.ServiceRequests">service request flags</see></param>
    Public Sub ToggleServiceRequest(ByVal turnOn As Boolean, ByVal serviceRequestMask As VI.Pith.ServiceRequests)
        If turnOn Then Me.StatusSubsystem.EnableServiceRequest(serviceRequestMask)
    End Sub

#End Region

#End Region

#Region " SYSTEM "

    Private _SystemSubsystem As SystemSubsystem
    ''' <summary>
    ''' Gets or sets the System Subsystem.
    ''' </summary>
    ''' <value>The System Subsystem.</value>
    Public Property SystemSubsystem As SystemSubsystem
        Get
            Return Me._SystemSubsystem
        End Get
        Set(value As SystemSubsystem)
            If Me._SystemSubsystem IsNot Nothing Then
                RemoveHandler Me.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
                Me.RemoveSubsystem(Me.SystemSubsystem)
                Me.SystemSubsystem.Dispose()
                Me._SystemSubsystem = Nothing
            End If
            Me._SystemSubsystem = value
            If Me._SystemSubsystem IsNot Nothing Then
                AddHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged
                Me.AddSubsystem(Me.SystemSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
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
        Dim activity As String = $"handling {NameOf(SystemSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, Switch.SystemSubsystem), e.PropertyName)
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

#Region " SERVICE REQUEST "

#If False Then
    ''' <summary> Reads the event registers after receiving a service request. </summary>
    ''' <remarks> Handled by the <see cref="DeviceBase"/></remarks>
    Protected Overrides Sub ProcessServiceRequest()
        If Me.IsDisposed OrElse Me.StatusSubsystem Is Nothing Then Return
        Dim activity As String = $"handling service request"
        Try
            Me.StatusSubsystem.ReadEventRegisters()
            If Me.StatusSubsystem.ErrorAvailable Then
                activity = "handling service request: reading device errors"
                Dim e As New isr.Core.Pith.ActionEventArgs
                If Not Me.StatusSubsystem.TrySafeQueryDeviceErrors(e) Then
                    Dim message As New isr.Core.Pith.TraceMessage(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"failed {activity};. details:{e.Details}")
                    If Me.Talker Is Nothing Then
                        My.MyLibrary.LogUnpublishedMessage(message)
                    Else
                        Me.Talker.Publish(message)
                    End If
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
#End If

#End Region

#Region " MY SETTINGS "

    ''' <summary> Opens the settings editor. </summary>
    Public Shared Sub OpenSettingsEditor()
        Using f As Core.Pith.ConfigurationEditor = Core.Pith.ConfigurationEditor.Get
            f.Text = "Switch Settings Editor"
            f.ShowDialog(My.MySettings.Default)
        End Using
    End Sub

    ''' <summary> Applies the settings. </summary>
    Public Overrides Sub ApplySettings()
        Dim settings As My.MySettings = My.MySettings.Default
        Me.HandlePropertyChange(settings, NameOf(My.MySettings.TraceLogLevel))
        Me.HandlePropertyChange(settings, NameOf(My.MySettings.TraceShowLevel))
        Me.HandlePropertyChange(settings, NameOf(My.MySettings.InitializeTimeout))
        Me.HandlePropertyChange(settings, NameOf(My.MySettings.ResetRefractoryPeriod))
        Me.HandlePropertyChange(settings, NameOf(My.MySettings.DeviceClearRefractoryPeriod))
        Me.HandlePropertyChange(settings, NameOf(My.MySettings.InitRefractoryPeriod))
        Me.HandlePropertyChange(settings, NameOf(My.MySettings.ClearRefractoryPeriod))
        Me.HandlePropertyChange(settings, NameOf(My.MySettings.SessionMessageNotificationLevel))
    End Sub

    ''' <summary> Handle the Platform property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal sender As My.MySettings, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(My.MySettings.TraceLogLevel)
                Me.ApplyTalkerTraceLevel(Core.Pith.ListenerType.Logger, sender.TraceLogLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Trace log level changed to {sender.TraceLogLevel}")
            Case NameOf(My.MySettings.TraceShowLevel)
                Me.ApplyTalkerTraceLevel(Core.Pith.ListenerType.Display, sender.TraceShowLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Trace show level changed to {sender.TraceShowLevel}")
            Case NameOf(My.MySettings.InitializeTimeout)
                Me.StatusSubsystemBase.InitializeTimeout = sender.InitializeTimeout
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.InitializeTimeout}")
            Case NameOf(My.MySettings.ResetRefractoryPeriod)
                Me.StatusSubsystemBase.ResetRefractoryPeriod = sender.ResetRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.ResetRefractoryPeriod}")
            Case NameOf(My.MySettings.DeviceClearRefractoryPeriod)
                Me.StatusSubsystemBase.DeviceClearRefractoryPeriod = sender.DeviceClearRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.DeviceClearRefractoryPeriod}")
            Case NameOf(My.MySettings.InitRefractoryPeriod)
                Me.StatusSubsystemBase.InitRefractoryPeriod = sender.InitRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.InitRefractoryPeriod}")
            Case NameOf(My.MySettings.ClearRefractoryPeriod)
                Me.StatusSubsystemBase.ClearRefractoryPeriod = sender.ClearRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.ClearRefractoryPeriod}")
        End Select
    End Sub

    ''' <summary> My settings property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Settings_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(My.MySettings)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, My.MySettings), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identifies the talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        Mybase.IdentifyTalkers()
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region


End Class
