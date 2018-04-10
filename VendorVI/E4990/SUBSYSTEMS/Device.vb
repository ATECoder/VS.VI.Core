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

#Region " CONSTRUCTORS  and  DESTRUCTORS "

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
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_CalculateChannelSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_CompensateOpenSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_CompensateLoadSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_CompensateShortSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ChannelMarkerSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_PrimaryChannelTraceSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SecondaryChannelTraceSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ChannelTriggerSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseChannelSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceChannelSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_TriggerSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_DisplaySubsystem", Justification:="Disposed @Subsystems")>
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

#Region " I PRESETTABLE PUBLISHER "

    ''' <summary> Initializes the Device. Used after reset to set a desired initial state. </summary>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All)
    End Sub

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
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnClosing(e)
        If Not e.Cancel Then
            Me.SystemSubsystem = Nothing
            Me.CalculateChannelSubsystem = Nothing
            Me.CompensateOpenSubsystem = Nothing
            Me.CompensateShortSubsystem = Nothing
            Me.CompensateLoadSubsystem = Nothing
            Me.ChannelMarkerSubsystem = Nothing
            Me.PrimaryChannelTraceSubsystem = Nothing
            Me.SecondaryChannelTraceSubsystem = Nothing
            Me.ChannelTriggerSubsystem = Nothing
            Me.DisplaySubsystem = Nothing
            Me.SenseChannelSubsystem = Nothing
            Me.SourceChannelSubsystem = Nothing
            Me.TriggerSubsystem = Nothing
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
            Me.CalculateChannelSubsystem = New CalculateChannelSubsystem(1, Me.StatusSubsystem)
            Me.CompensateOpenSubsystem = New CompensateChannelSubsystem(Scpi.CompensationTypes.OpenCircuit, 1, Me.StatusSubsystem)
            Me.CompensateShortSubsystem = New CompensateChannelSubsystem(Scpi.CompensationTypes.ShortCircuit, 1, Me.StatusSubsystem)
            Me.CompensateLoadSubsystem = New CompensateChannelSubsystem(Scpi.CompensationTypes.Load, 1, Me.StatusSubsystem)
            Me.ChannelMarkerSubsystem = New ChannelMarkerSubsystem(1, 1, Me.StatusSubsystem)
            Me.PrimaryChannelTraceSubsystem = New ChannelTraceSubsystem(1, 1, Me.StatusSubsystem)
            Me.SecondaryChannelTraceSubsystem = New ChannelTraceSubsystem(2, 1, Me.StatusSubsystem)
            Me.ChannelTriggerSubsystem = New ChannelTriggerSubsystem(1, Me.StatusSubsystem)
            Me.DisplaySubsystem = New DisplaySubsystem(Me.StatusSubsystem)
            Me.SenseChannelSubsystem = New SenseChannelSubsystem(1, Me.StatusSubsystem)
            Me.SourceChannelSubsystem = New SourceChannelSubsystem(1, Me.StatusSubsystem)
            Me.TriggerSubsystem = New TriggerSubsystem(Me.StatusSubsystem)
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

    Private _CalculateChannelSubsystem As CalculateChannelSubsystem
    ''' <summary>
    ''' Gets or sets the CalculateChannel Subsystem.
    ''' </summary>
    ''' <value>The CalculateChannel Subsystem.</value>
    Public Property CalculateChannelSubsystem As CalculateChannelSubsystem
        Get
            Return Me._CalculateChannelSubsystem
        End Get
        Set(value As CalculateChannelSubsystem)
            If Me._CalculateChannelSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.CalculateChannelSubsystem)
                Me.CalculateChannelSubsystem.Dispose()
                Me._CalculateChannelSubsystem = Nothing
            End If
            Me._CalculateChannelSubsystem = value
            If Me._CalculateChannelSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.CalculateChannelSubsystem)
            End If
        End Set
    End Property

    Private _ChannelMarkerSubsystem As ChannelMarkerSubsystem
    ''' <summary>
    ''' Gets or sets the ChannelMarker Subsystem.
    ''' </summary>
    ''' <value>The ChannelMarker Subsystem.</value>
    Public Property ChannelMarkerSubsystem As ChannelMarkerSubsystem
        Get
            Return Me._ChannelMarkerSubsystem
        End Get
        Set(value As ChannelMarkerSubsystem)
            If Me._ChannelMarkerSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.ChannelMarkerSubsystem)
                Me.ChannelMarkerSubsystem.Dispose()
                Me._ChannelMarkerSubsystem = Nothing
            End If
            Me._ChannelMarkerSubsystem = value
            If Me._ChannelMarkerSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.ChannelMarkerSubsystem)
            End If
        End Set
    End Property

    Private _PrimaryChannelTraceSubsystem As ChannelTraceSubsystem
    ''' <summary>
    ''' Gets or sets the PrimaryChannelTrace Subsystem.
    ''' </summary>
    ''' <value>The PrimaryChannelTrace Subsystem.</value>
    Public Property PrimaryChannelTraceSubsystem As ChannelTraceSubsystem
        Get
            Return Me._PrimaryChannelTraceSubsystem
        End Get
        Set(value As ChannelTraceSubsystem)
            If Me._PrimaryChannelTraceSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.PrimaryChannelTraceSubsystem)
                Me.PrimaryChannelTraceSubsystem.Dispose()
                Me._PrimaryChannelTraceSubsystem = Nothing
            End If
            Me._PrimaryChannelTraceSubsystem = value
            If Me._PrimaryChannelTraceSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.PrimaryChannelTraceSubsystem)
            End If
        End Set
    End Property

    Private _SecondaryChannelTraceSubsystem As ChannelTraceSubsystem
    ''' <summary>
    ''' Gets or sets the SecondaryChannelTrace Subsystem.
    ''' </summary>
    ''' <value>The SecondaryChannelTrace Subsystem.</value>
    Public Property SecondaryChannelTraceSubsystem As ChannelTraceSubsystem
        Get
            Return Me._SecondaryChannelTraceSubsystem
        End Get
        Set(value As ChannelTraceSubsystem)
            If Me._SecondaryChannelTraceSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SecondaryChannelTraceSubsystem)
                Me.SecondaryChannelTraceSubsystem.Dispose()
                Me._SecondaryChannelTraceSubsystem = Nothing
            End If
            Me._SecondaryChannelTraceSubsystem = value
            If Me._SecondaryChannelTraceSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SecondaryChannelTraceSubsystem)
            End If
        End Set
    End Property

    Private _ChannelTriggerSubsystem As ChannelTriggerSubsystem
    ''' <summary>
    ''' Gets or sets the ChannelTrigger Subsystem.
    ''' </summary>
    ''' <value>The ChannelTrigger Subsystem.</value>
    Public Property ChannelTriggerSubsystem As ChannelTriggerSubsystem
        Get
            Return Me._ChannelTriggerSubsystem
        End Get
        Set(value As ChannelTriggerSubsystem)
            If Me._ChannelTriggerSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.ChannelTriggerSubsystem)
                Me.ChannelTriggerSubsystem.Dispose()
                Me._ChannelTriggerSubsystem = Nothing
            End If
            Me._ChannelTriggerSubsystem = value
            If Me._ChannelTriggerSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.ChannelTriggerSubsystem)
            End If
        End Set
    End Property

    Private _CompensateOpenSubsystem As CompensateChannelSubsystem
    ''' <summary>
    ''' Gets or sets the Compensate Open Subsystem.
    ''' </summary>
    ''' <value>The Compensate Open Subsystem.</value>
    Public Property CompensateOpenSubsystem As CompensateChannelSubsystem
        Get
            Return Me._CompensateOpenSubsystem
        End Get
        Set(value As CompensateChannelSubsystem)
            If Me._CompensateOpenSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me._CompensateOpenSubsystem)
                Me._CompensateOpenSubsystem.Dispose()
                Me._CompensateOpenSubsystem = Nothing
            End If
            Me._CompensateOpenSubsystem = value
            If Me._CompensateOpenSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.CompensateOpenSubsystem)
            End If
        End Set
    End Property

    Private _CompensateShortSubsystem As CompensateChannelSubsystem
    ''' <summary>
    ''' Gets or sets the Compensate Short Subsystem.
    ''' </summary>
    ''' <value>The Compensate Short Subsystem.</value>
    Public Property CompensateShortSubsystem As CompensateChannelSubsystem
        Get
            Return Me._CompensateShortSubsystem
        End Get
        Set(value As CompensateChannelSubsystem)
            If Me._CompensateShortSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.CompensateShortSubsystem)
                Me.CompensateShortSubsystem.Dispose()
                Me._CompensateShortSubsystem = Nothing
            End If
            Me._CompensateShortSubsystem = value
            If Me._CompensateShortSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.CompensateShortSubsystem)
            End If
        End Set
    End Property

    Private _CompensateLoadSubsystem As CompensateChannelSubsystem
    ''' <summary>
    ''' Gets or sets the Compensate Load Subsystem.
    ''' </summary>
    ''' <value>The Compensate Load Subsystem.</value>
    Public Property CompensateLoadSubsystem As CompensateChannelSubsystem
        Get
            Return Me._CompensateLoadSubsystem
        End Get
        Set(value As CompensateChannelSubsystem)
            If Me._CompensateLoadSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.CompensateLoadSubsystem)
                Me.CompensateLoadSubsystem.Dispose()
                Me._CompensateLoadSubsystem = Nothing
            End If
            Me._CompensateLoadSubsystem = value
            If Me._CompensateLoadSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.CompensateLoadSubsystem)
            End If
        End Set
    End Property

    Private _DisplaySubsystem As DisplaySubsystem
    ''' <summary>
    ''' Gets or sets the Display Subsystem.
    ''' </summary>
    ''' <value>The Display Subsystem.</value>
    Public Property DisplaySubsystem As DisplaySubsystem
        Get
            Return Me._DisplaySubsystem
        End Get
        Set(value As DisplaySubsystem)
            If Me._DisplaySubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.DisplaySubsystem)
                Me.DisplaySubsystem.Dispose()
                Me._DisplaySubsystem = Nothing
            End If
            Me._DisplaySubsystem = value
            If Me._DisplaySubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.DisplaySubsystem)
            End If
        End Set
    End Property

    Private _SenseChannelSubsystem As SenseChannelSubsystem
    ''' <summary>
    ''' Gets or sets the SenseChannel Subsystem.
    ''' </summary>
    ''' <value>The SenseChannel Subsystem.</value>
    Public Property SenseChannelSubsystem As SenseChannelSubsystem
        Get
            Return Me._SenseChannelSubsystem
        End Get
        Set(value As SenseChannelSubsystem)
            If Me._SenseChannelSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SenseChannelSubsystem)
                Me.SenseChannelSubsystem.Dispose()
                Me._SenseChannelSubsystem = Nothing
            End If
            Me._SenseChannelSubsystem = value
            If Me._SenseChannelSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SenseChannelSubsystem)
            End If
        End Set
    End Property

    Private _SourceChannelSubsystem As SourceChannelSubsystem
    ''' <summary>
    ''' Gets or sets the SourceChannel Subsystem.
    ''' </summary>
    ''' <value>The SourceChannel Subsystem.</value>
    Public Property SourceChannelSubsystem As SourceChannelSubsystem
        Get
            Return Me._SourceChannelSubsystem
        End Get
        Set(value As SourceChannelSubsystem)
            If Me._SourceChannelSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SourceChannelSubsystem)
                Me.SourceChannelSubsystem.Dispose()
                Me._SourceChannelSubsystem = Nothing
            End If
            Me._SourceChannelSubsystem = value
            If Me._SourceChannelSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SourceChannelSubsystem)
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

#Region " COMPENSATION "

    ''' <summary> Clears the compensations. </summary>
    Public Sub ClearCompensations()

        ' clear the calibration parameters
        Me.SenseChannelSubsystem.ClearCompensations()
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.CompensateOpenSubsystem.ClearMeasurements()
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.CompensateShortSubsystem.ClearMeasurements()
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.CompensateLoadSubsystem.ClearMeasurements()
        Me.StatusSubsystem.QueryOperationCompleted()

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
        Dim activity As String = $"handling {NameOf(E4990.StatusSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, E4990.StatusSubsystem), e.PropertyName)
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
                Me.RemoveSubsystem(Me.SystemSubsystem)
                Me.SystemSubsystem.Dispose()
                Me._SystemSubsystem = Nothing
            End If
            Me._SystemSubsystem = value
            If Me._SystemSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SystemSubsystem)
            End If
        End Set
    End Property

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
            f.Text = "E4990 Settings Editor"
            f.ShowDialog(My.MySettings.Default)
        End Using
    End Sub

    ''' <summary> Applies the settings. </summary>
    Protected Overrides Sub ApplySettings()
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
