Imports isr.VI.ExceptionExtensions
''' <summary> Implements a Keysight K3458 Meter. </summary>
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
''' <history date="10/7/2013" by="David" revision=""> Created. </history>
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

    ''' <summary> Creates a new Device. </summary>
    ''' <returns> A Device. </returns>
    Public Shared Function Create() As Device
        Dim device As Device = Nothing
        Try
            device = New Device
        Catch
            If device IsNot Nothing Then device.Dispose()
            device = Nothing
            Throw
        End Try
        Return device
    End Function

    ''' <summary> Validated the given device. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="device"> The device. </param>
    ''' <returns> A Device. </returns>
    Public Shared Function Validated(ByVal device As Device) As Device
        If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
        Return device
    End Function

#Region " I Disposable Support "

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MeasureSubsystem", Justification:="Disposed @Subsystems")>
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
        Me.StatusSubsystem.EnableServiceRequest(Me.StatusSubsystem.ServiceRequestEnableBitmask.Value)
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

#Region " SESSION INITIALIZATION PROPERTIES  "

    ''' <summary> Gets the bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Protected Overrides ReadOnly Property ErrorAvailableBits() As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.StandardEvent

    ''' <summary> Gets or sets the keep alive interval. </summary>
    ''' <value> The keep alive interval. </value>
    Protected Overrides ReadOnly Property KeepAliveInterval As TimeSpan = TimeSpan.Zero

    ''' <summary> Gets or sets the is alive command. </summary>
    ''' <value> The is alive command. </value>
    Protected Overrides ReadOnly Property IsAliveCommand As String = ""

    ''' <summary> Gets or sets the is alive query command. </summary>
    ''' <value> The is alive query command. </value>
    Protected Overrides ReadOnly Property IsAliveQueryCommand As String = ""

#End Region

    ''' <summary> Allows the derived device to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnClosing(ByVal e As ComponentModel.CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnClosing(e)
        If Not e.Cancel Then
            Me.MeasureSubsystem = Nothing
            Me.SystemSubsystem = Nothing
        End If
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnOpening(ByVal e As ComponentModel.CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnOpening(e)
        If Not e.Cancel Then
            Me.SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
            ' better add before the format subsystem, which reset initializes the readings.
            Me.MeasureSubsystem = New MeasureSubsystem(Me.StatusSubsystem)
        End If
    End Sub

#End Region

#Region " STATUS SESSION "

    ''' <summary> Allows the derived device status subsystem to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
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

#Region " MEASURE "

    Private _MeasureSubsystem As MeasureSubsystem
    ''' <summary>
    ''' Gets or sets the Measure Subsystem.
    ''' </summary>
    ''' <value>The Measure Subsystem.</value>
    Public Property MeasureSubsystem As MeasureSubsystem
        Get
            Return Me._MeasureSubsystem
        End Get
        Set(value As MeasureSubsystem)
            If Me._MeasureSubsystem IsNot Nothing Then
                RemoveHandler Me.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
                Me.RemoveSubsystem(Me.MeasureSubsystem)
                Me.MeasureSubsystem.Dispose()
                Me._MeasureSubsystem = Nothing
            End If
            Me._MeasureSubsystem = value
            If Me._MeasureSubsystem IsNot Nothing Then
                AddHandler Me.MeasureSubsystem.PropertyChanged, AddressOf MeasureSubsystemPropertyChanged
                Me.AddSubsystem(Me.MeasureSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Measure subsystem property changed. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As MeasureSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K3458.MeasureSubsystem.LastReading)
        End Select
    End Sub

    ''' <summary> Measure subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(MeasureSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, MeasureSubsystem), e.PropertyName)
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
        Dim activity As String = $"handling {NameOf(StatusSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, StatusSubsystem), e.PropertyName)
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
            Me.HandlePropertyChange(TryCast(sender, SystemSubsystem), e.PropertyName)
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
        Me.StatusSubsystem.ReadEventRegisters()
        If Me.StatusSubsystem.ErrorAvailable Then
            Me.StatusSubsystem.QueryLastError()
        End If
    End Sub
#End If

#End Region

#Region " MY SETTINGS "

    ''' <summary> Opens the settings editor. </summary>
    Public Shared Sub OpenSettingsEditor()
        Using f As Core.Pith.ConfigurationEditor = Core.Pith.ConfigurationEditor.Get
            f.Text = "K3458 Settings Editor"
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

    ''' <summary> Identifies talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

#End Region

End Class
