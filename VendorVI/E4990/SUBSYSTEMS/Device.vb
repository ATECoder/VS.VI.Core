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

    ''' <summary> Initializes a new instance of the <see cref="E4990.Device" /> class. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializeTimeout = TimeSpan.FromMilliseconds(5000)
        Me.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter()
    End Sub

#Region "IDisposable Support"

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
                If Me.IsDeviceOpen Then Me.OnClosing(New ComponentModel.CancelEventArgs)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception disposing device", "Exception details: {0}", ex)
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " I PRESETTABLE PUBLISHER "

    ''' <summary> Clears the Device. Issues <see cref="StatusSubsystemBase.ClearActiveState">Selective
    ''' Device Clear</see>. </summary>
    Public Overrides Sub ClearActiveState()
        Me.StatusSubsystem.ClearActiveState()
    End Sub

    ''' <summary> Initializes the Device. Used after reset to set a desired initial state. </summary>
    ''' <remarks> David, 1/15/2016. </remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
    End Sub

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        Me.Subsystems.Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.AsyncNotifyPropertyChanged(p.Name)
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

        Me._CalculateChannelSubsystem = New CalculateChannelSubsystem(1, Me.StatusSubsystem)
        Me.AddSubsystem(Me.CalculateChannelSubsystem)

        Me._CompensateOpenSubsystem = New CompensateChannelSubsystem(CompensationTypes.OpenCircuit, 1, Me.StatusSubsystem)
        Me.AddSubsystem(Me.CompensateOpenSubsystem)

        Me._CompensateShortSubsystem = New CompensateChannelSubsystem(CompensationTypes.ShortCircuit, 1, Me.StatusSubsystem)
        Me.AddSubsystem(Me.CompensateShortSubsystem)

        Me._CompensateLoadSubsystem = New CompensateChannelSubsystem(CompensationTypes.Load, 1, Me.StatusSubsystem)
        Me.AddSubsystem(Me.CompensateLoadSubsystem)

        Me._ChannelMarkerSubsystem = New ChannelMarkerSubsystem(1, 1, Me.StatusSubsystem)
        Me.AddSubsystem(Me.ChannelMarkerSubsystem)

        Me._PrimaryChannelTraceSubsystem = New ChannelTraceSubsystem(1, 1, Me.StatusSubsystem)
        Me.AddSubsystem(Me.PrimaryChannelTraceSubsystem)

        Me._SecondaryChannelTraceSubsystem = New ChannelTraceSubsystem(1, 1, Me.StatusSubsystem)
        Me.AddSubsystem(Me.SecondaryChannelTraceSubsystem)

        Me._ChannelTriggerSubsystem = New ChannelTriggerSubsystem(1, Me.StatusSubsystem)
        Me.AddSubsystem(Me.ChannelTriggerSubsystem)

        Me._DisplaySubsystem = New DisplaySubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.DisplaySubsystem)

        Me._SenseChannelSubsystem = New SenseChannelSubsystem(1, Me.StatusSubsystem)
        Me.AddSubsystem(Me.SenseChannelSubsystem)

        Me._SourceChannelSubsystem = New SourceChannelSubsystem(1, Me.StatusSubsystem)
        Me.AddSubsystem(Me.SourceChannelSubsystem)

        Me._TriggerSubsystem = New TriggerSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.TriggerSubsystem)

    End Sub

#End Region

#Region " SUBSYSTEMS "

    ''' <summary> Gets or sets the calculate channel subsystem. </summary>
    ''' <value> The calculate channel subsystem. </value>
    Public Property CalculateChannelSubsystem As CalculateChannelSubsystem

    ''' <summary> Gets or sets the channel marker subsystem. </summary>
    ''' <value> The channel marker subsystem. </value>
    Public Property ChannelMarkerSubsystem As ChannelMarkerSubsystem

    ''' <summary> Gets or sets the primary channel Trace subsystem. </summary>
    ''' <value> The primary channel Trace subsystem. </value>
    Public Property PrimaryChannelTraceSubsystem As ChannelTraceSubsystem

    ''' <summary> Gets or sets the Secondary channel Trace subsystem. </summary>
    ''' <value> The Secondary channel Trace subsystem. </value>
    Public Property SecondaryChannelTraceSubsystem As ChannelTraceSubsystem

    ''' <summary> Gets or sets the channel Trigger subsystem. </summary>
    ''' <value> The channel Trigger subsystem. </value>
    Public Property ChannelTriggerSubsystem As ChannelTriggerSubsystem

    ''' <summary> Gets or sets the Compensate open subsystem. </summary>
    ''' <value> The Compensate open subsystem. </value>
    Public Property CompensateOpenSubsystem As CompensateChannelSubsystem

    ''' <summary> Gets or sets the Compensate Short subsystem. </summary>
    ''' <value> The Compensate Short subsystem. </value>
    Public Property CompensateShortSubsystem As CompensateChannelSubsystem

    ''' <summary> Gets or sets the Compensate Load subsystem. </summary>
    ''' <value> The Compensate Load subsystem. </value>
    Public Property CompensateLoadSubsystem As CompensateChannelSubsystem

    ''' <summary> Gets or sets Display subsystem. </summary>
    ''' <value> The Display subsystem. </value>
    Public Property DisplaySubsystem As DisplaySubsystem

    ''' <summary> Gets or sets the sense channel subsystem. </summary>
    ''' <value> The sense channel subsystem. </value>
    Public Property SenseChannelSubsystem As SenseChannelSubsystem

    ''' <summary> Gets or sets source channel subsystem. </summary>
    ''' <value> The source channel subsystem. </value>
    Public Property SourceChannelSubsystem As SourceChannelSubsystem

    ''' <summary> Gets or sets trigger subsystem. </summary>
    ''' <value> The trigger subsystem. </value>
    Public Property TriggerSubsystem As TriggerSubsystem

#Region " COMPENSATION "

    ''' <summary> Clears the compensations. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
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

    ''' <summary>
    ''' Gets or sets the Status Subsystem.
    ''' </summary>
    ''' <value>The Status Subsystem.</value>
    Public Property StatusSubsystem As StatusSubsystem

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, StatusSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

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

End Class
