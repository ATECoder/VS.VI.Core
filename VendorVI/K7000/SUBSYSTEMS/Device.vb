Imports isr.Core.Pith
Imports isr.Core.Pith.ExceptionExtensions
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
    Inherits DeviceBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="K7000.Device" /> class. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializeTimeout = TimeSpan.FromMilliseconds(5000)
        Me.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter()
        AddHandler My.Settings.PropertyChanged, AddressOf Me._Settings_PropertyChanged
    End Sub

    ''' <summary> Creates a new Device. </summary>
    ''' <returns> A Device. </returns>
    Public Shared Function Create() As Device
        Dim device As Device = Nothing
        Try
            device = New Device
        Catch
            device.Dispose()
            Throw
        End Try
        Return device
    End Function

#Region "IDisposable Support"

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ArmLayer1Subsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ArmLayer2Subsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_TriggerSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_RouteSubsystem", Justification:="Disposed @Subsystems")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                'listeners must clear, otherwise closing could raise an exception.
                Me.Talker?.Listeners.Clear()
                If Me.IsDeviceOpen Then Me.OnClosing(New ComponentModel.CancelEventArgs)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception disposing device", $"Exception {ex.ToFullBlownString}")
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Clears the Device. Issues <see cref="StatusSubsystemBase.ClearActiveState">Selective
    ''' Device Clear</see>. </summary>
    Public Overrides Sub ClearActiveState()
        Me.StatusSubsystem.ClearActiveState()
    End Sub

    ''' <summary> Initializes the Device. Used after reset to set a desired initial state. </summary>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
        Me.ApplySettings()
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

    ''' <summary> Allows the derived device to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnClosing(ByVal e As ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If e?.Cancel Then Return
        If Me._SystemSubsystem IsNot Nothing Then
            'RemoveHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged
        End If
        If Me._StatusSubsystem IsNot Nothing Then
            RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
        End If
        Me.Subsystems.DisposeItems()
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnOpening(ByVal e As ComponentModel.CancelEventArgs)
        MyBase.OnOpening(e)
        If e?.Cancel Then Return

        ' STATUS must be the first subsystem.
        Me._StatusSubsystem = New StatusSubsystem(Me.Session)
        Me.AddSubsystem(Me.StatusSubsystem)
        AddHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged

        Me._SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SystemSubsystem)
        'AddHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged

        Me._RouteSubsystem = New RouteSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.RouteSubsystem)

        Me._TriggerSubsystem = New TriggerSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.TriggerSubsystem)

        Me._ArmLayer1Subsystem = New ArmLayer1Subsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.ArmLayer1Subsystem)

        Me._ArmLayer2Subsystem = New ArmLayer2Subsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.ArmLayer2Subsystem)

    End Sub

#End Region

#Region " SUBSYSTEMS "

    ''' <summary> Gets or sets the Arm Subsystem. </summary>
    ''' <value> The Arm Subsystem. </value>
    Public Property ArmLayer1Subsystem As ArmLayer1Subsystem

    ''' <summary> Gets or sets the Arm Layer 2 Subsystem. </summary>
    ''' <value> The Arm Layer 2 Subsystem. </value>
    Public Property ArmLayer2Subsystem As ArmLayer2Subsystem

    ''' <summary>
    ''' Gets or sets the Route Subsystem.
    ''' </summary>
    ''' <value>The System Subsystem.</value>
    Public Property RouteSubsystem As RouteSubsystem

    ''' <summary> Gets or sets the Trigger Subsystem. </summary>
    ''' <value> The Trigger Subsystem. </value>
    Public Property TriggerSubsystem As TriggerSubsystem

#Region " STATUS "

    ''' <summary>
    ''' Gets or sets the Status Subsystem.
    ''' </summary>
    ''' <value>The Status Subsystem.</value>
    Public Property StatusSubsystem As StatusSubsystem

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, StatusSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling property '{e?.PropertyName}' change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#Region " SERVICE REQUEST "

    ''' <summary>
    ''' Enables or disables the operation service status registers requesting
    ''' a service request upon a negative transition.
    ''' </summary>
    ''' <param name="isOn">if set to <c>True</c> [is on].</param>
    Public Sub ToggleEndOfScanService(ByVal isOn As Boolean)
        Me.ToggleEndOfScanService(isOn, ServiceRequests.All And (Not ServiceRequests.MessageAvailable))
    End Sub

    ''' <summary>
    ''' Enables or disables the operation service status registers requesting
    ''' a service request upon a negative transition.
    ''' </summary>
    ''' <param name="turnOn">True to turn on or false to turn off the service request.</param>
    ''' <param name="serviceRequestMask">Specifies the
    ''' <see cref="ServiceRequests">service request flags</see></param>
    Public Sub ToggleEndOfScanService(ByVal turnOn As Boolean, ByVal serviceRequestMask As ServiceRequests)
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
    ''' <see cref="ServiceRequests">service request flags</see></param>
    Public Sub ToggleServiceRequest(ByVal turnOn As Boolean, ByVal serviceRequestMask As ServiceRequests)
        If turnOn Then Me.StatusSubsystem.EnableServiceRequest(serviceRequestMask)
    End Sub

#End Region

#End Region

#Region " SYSTEM "

    ''' <summary>
    ''' Gets or sets the System Subsystem.
    ''' </summary>
    ''' <value>The System Subsystem.</value>
    Public Property SystemSubsystem As SystemSubsystem

#End Region

#End Region

#Region " SERVICE REQUEST "

    ''' <summary> Reads the event registers after receiving a service request. </summary>
    Protected Overrides Sub ProcessServiceRequest()
        Me.StatusSubsystem.ReadRegisters()
        If Me.StatusSubsystem.MessageAvailable Then
            ' if we have a message this needs to be processed by the subsystem requesting the message.
            ' Only thereafter the registers should be read.
        End If
        If Not Me.StatusSubsystem.MessageAvailable AndAlso Me.StatusSubsystem.ErrorAvailable Then
            Me.StatusSubsystem.QueryDeviceErrors()
        End If
        If Me.StatusSubsystem.MeasurementAvailable Then
        End If
    End Sub

#End Region

#Region " MY SETTINGS "

    ''' <summary> Opens the settings editor. </summary>
    Public Shared Sub OpenSettingsEditor()
        Using f As ConfigurationEditor = ConfigurationEditor.Get
            f.Text = "K7000 Settings Editor"
            f.ShowDialog(My.MySettings.Default)
        End Using
    End Sub

    ''' <summary> Applies the settings. </summary>
    Private Sub ApplySettings()
        Dim settings As My.MySettings = My.MySettings.Default
        Me.OnSettingsPropertyChanged(settings, NameOf(settings.TraceLogLevel))
        Me.OnSettingsPropertyChanged(settings, NameOf(settings.TraceShowLevel))
    End Sub

    ''' <summary> Handle the Platform property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSettingsPropertyChanged(ByVal sender As My.MySettings, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.TraceLogLevel)
                Me.ApplyTalkerTraceLevel(ListenerType.Logger, sender.TraceLogLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Trace log level changed to {sender.TraceLogLevel}")
            Case NameOf(sender.TraceShowLevel)
                Me.ApplyTalkerTraceLevel(ListenerType.Display, sender.TraceShowLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Trace show level changed to {sender.TraceShowLevel}")
        End Select
    End Sub

    ''' <summary> My settings property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Settings_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        Dim settings As My.MySettings = TryCast(sender, My.MySettings)
        If settings Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSettingsPropertyChanged(settings, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception handling Settings.{e.PropertyName} property;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds a listener. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overrides Sub AddListener(ByVal listener As isr.Core.Pith.IMessageListener)
        MyBase.AddListener(listener)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of isr.Core.Pith.IMessageListener))
        MyBase.AddListeners(listeners)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AddListeners(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
        MyBase.AddListeners(talker)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region

End Class
