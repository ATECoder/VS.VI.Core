Imports isr.Core.Pith
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Implements a EG2000 Prober device. </summary>
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
''' <history date="10/01/2013" by="David" revision="3.0.5022"> Created. </history>
Public Class Device
    Inherits DeviceBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="EG2000.Device" /> class.
    ''' </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializeTimeout = TimeSpan.FromMilliseconds(5000)
        Me.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter(HardwareInterfaceType.Gpib,
                                                                           HardwareInterfaceType.Tcpip,
                                                                           HardwareInterfaceType.Usb)
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

#Region " IDisposable Support "

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ProberSubsystem", Justification:="Disposed @Subsystems")>
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

        ' now request the subsystem to issue an Asynchronous call to get the Prober to reveal its identity.
        Me.StatusSubsystem.QueryIdentity()

        ' enable the service request if not enabled.
        Me.Session.EnableServiceRequest()

        ' enable service request only after everything is initialized.
        Me.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)

        ' Adds the device service request handler
        Me.AddServiceRequestEventHandler()

        Me.ApplySettings()

    End Sub

    Public Overrides Sub ResetKnownState()

        ' remove the device service request handler
        Me.RemoveServiceRequestEventHandler()

        ' disable the session service request 
        Me.Session.DisableServiceRequest()

        MyBase.ResetKnownState()
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

#Region " SESSION "

    ''' <summary> Allows the derived device to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnClosing(ByVal e As ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If e?.Cancel Then Return
        If Me._ProberSubsystem IsNot Nothing Then
            RemoveHandler Me.ProberSubsystem.PropertyChanged, AddressOf Me.ProberSubsystemPropertyChanged
        End If
        If Me._SystemSubsystem IsNot Nothing Then
            'RemoveHandler Me.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
        If Me._StatusSubsystem IsNot Nothing Then
            RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
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
        AddHandler Me.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged

        Me._SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SystemSubsystem)
        'AddHandler Me.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged

        Me._ProberSubsystem = New ProberSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.ProberSubsystem)
        AddHandler Me.ProberSubsystem.PropertyChanged, AddressOf Me.ProberSubsystemPropertyChanged
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " PROBER "

    ''' <summary> Gets or sets the Prober Subsystem. </summary>
    ''' <value> The Prober Subsystem. </value>
    Public Property ProberSubsystem As ProberSubsystem

    ''' <summary> Handle the Prober subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As ProberSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Dim lastReading As String = subsystem.LastReading
        Select Case propertyName
            Case NameOf(subsystem.IdentityRead)
                If subsystem.IdentityRead AndAlso Not String.IsNullOrWhiteSpace(lastReading) Then
                    Me.StatusSubsystem.Identity = lastReading
                End If
            Case NameOf(subsystem.ErrorRead)
                If subsystem.ErrorRead Then
                    Me.StatusSubsystem.EnqueueLastError(subsystem?.LastReading)
                Else
                End If
        End Select
    End Sub

    ''' <summary> Prober subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ProberSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, ProberSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}",
                             e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Gets or sets the Status Subsystem. </summary>
    ''' <value> The Status Subsystem. </value>
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

#End Region

#Region " SYSTEM "

    ''' <summary> Gets or sets the System Subsystem. </summary>
    ''' <value> The System Subsystem. </value>
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
            Me.ProberSubsystem.FetchAndParse()
        End If
    End Sub

#End Region

#Region " MY SETTINGS "

    ''' <summary> Opens the settings editor. </summary>
    Public Shared Sub OpenSettingsEditor()
        Using f As ConfigurationEditor = ConfigurationEditor.Get
            f.Text = "EG2000 Settings Editor"
            f.ShowDialog(isr.VI.EG2000.Settings.Default)
        End Using
    End Sub

    ''' <summary> Applies the settings. </summary>
    Private Sub ApplySettings()
        Dim settings As isr.VI.EG2000.Settings = isr.VI.EG2000.My.Settings
        Me.OnSettingsPropertyChanged(settings, NameOf(settings.TraceLogLevel))
        Me.OnSettingsPropertyChanged(settings, NameOf(settings.TraceShowLevel))
    End Sub

    ''' <summary> Handle the Platform property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSettingsPropertyChanged(ByVal sender As isr.VI.EG2000.Settings, ByVal propertyName As String)
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
        Dim settings As isr.VI.EG2000.Settings = TryCast(sender, isr.VI.EG2000.Settings)
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
