Imports isr.Core.Pith.ExceptionExtensions
''' <summary> The Thermal Transient Meter device. </summary>
''' <remarks> An instrument is defined, for the purpose of this library, as a device with a front
''' panel. </remarks>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="12/12/2013" by="David" revision=""> Created. </history>
Public Class Device
    Inherits DeviceBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="DeviceBase" /> class. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializeTimeout = TimeSpan.FromMilliseconds(30000)
        Me.ResourcesFilter = ResourceNamesManager.BuildInstrumentFilter(HardwareInterfaceType.Gpib,
                                                                        HardwareInterfaceType.Tcpip,
                                                                        HardwareInterfaceType.Usb)
    End Sub

#Region " I Disposable Support"

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_DisplaySubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="Disposed @Subsystems")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                'listeners must clear, otherwise closing could raise an exception.
                Me.Talker?.Listeners.Clear()
                If Me.IsDeviceOpen Then Me.OnClosing(New System.ComponentModel.CancelEventArgs)
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

    ''' <summary> Clears the active state. Issues selective device clear. </summary>
    Public Overrides Sub ClearActiveState()
        MyBase.ClearActiveState()
        Me.StatusSubsystem.ClearActiveState()
    End Sub

    ''' <summary> Initializes the Device. Used after reset to set a desired initial state. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Try
            Me.Session.StoreTimeout(Me.InitializeTimeout)
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing error queue;. ")
            ' clear the error queue on the controller node only.
            Me.StatusSubsystem.ClearErrorQueue()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Exception ignored clearing error queue;. {0}", ex.ToFullBlownString)
        Finally
            Me.Session.RestoreTimeout()
        End Try

        Try
            ' flush the input buffer in case the instrument has some leftovers.
            Me.Session.DiscardUnreadData()
            If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Data discarded after clearing read buffer;. Data: {0}.", Me.Session.DiscardedData)
            End If
        Catch ex As NativeException
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception ignored clearing read buffer;. {0}", ex.ToFullBlownString)
        End Try

        Try
            ' flush write may cause the instrument to send off a new data.
            Me.Session.DiscardUnreadData()
            If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Unread data discarded after discarding unset data;. Data: {0}.", Me.Session.DiscardedData)
            End If
        Catch ex As NativeException
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception ignored clearing read buffer;. {0}", ex.ToFullBlownString)
        End Try
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
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If e?.Cancel Then Return

        If Me._StatusSubsystem IsNot Nothing Then
            RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
        End If

        If Me.DisplaySubsystem IsNot Nothing Then
            Me.DisplaySubsystem.RestoreDisplay(Me.InitializeTimeout)
        End If

        If Me.SourceMeasureUnitCurrentSource IsNot Nothing Then
            RemoveHandler Me.SourceMeasureUnitCurrentSource.PropertyChanged, AddressOf SourceMeasureUnitCurrentSourcePropertyChanged
        End If

        If Me.SourceMeasureUnitMeasure IsNot Nothing Then
            RemoveHandler Me.SourceMeasureUnitMeasure.PropertyChanged, AddressOf SourceMeasureUnitMeasurePropertyChanged
        End If

        If Me.SourceMeasureUnit IsNot Nothing Then
            RemoveHandler Me.SourceMeasureUnit.PropertyChanged, AddressOf SourceMeasureSubsystemPropertyChanged
        End If

        If Me.DisplaySubsystem IsNot Nothing Then
            RemoveHandler Me.DisplaySubsystem.PropertyChanged, AddressOf DisplaySubsystemPropertyChanged
        End If

        If Me.SystemSubsystem IsNot Nothing Then
            RemoveHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged
        End If
        Me.Subsystems.DisposeItems()
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnOpening(e As System.ComponentModel.CancelEventArgs)
        MyBase.OnOpening(e)
        If e?.Cancel Then Return
        ' STATUS must be the first subsystem.
        Me.StatusSubsystem = New StatusSubsystem(Me.Session)
        Me.AddSubsystem(Me.StatusSubsystem)
        AddHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged

        Me.SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SystemSubsystem)
        AddHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged

        Me.DisplaySubsystem = New DisplaySubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.DisplaySubsystem)
        AddHandler Me.DisplaySubsystem.PropertyChanged, AddressOf DisplaySubsystemPropertyChanged

        Me.SourceMeasureUnit = New SourceMeasureUnitDevice(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SourceMeasureUnit)
        AddHandler Me.SourceMeasureUnit.PropertyChanged, AddressOf SourceMeasureSubsystemPropertyChanged

        Me.SourceMeasureUnitCurrentSource = New SourceMeasureUnitCurrentSource(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SourceMeasureUnitCurrentSource)
        AddHandler Me.SourceMeasureUnitCurrentSource.PropertyChanged, AddressOf SourceMeasureUnitCurrentSourcePropertyChanged

        Me.SourceMeasureUnitMeasure = New SourceMeasureUnitMeasure(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SourceMeasureUnitMeasure)
        AddHandler Me.SourceMeasureUnitMeasure.PropertyChanged, AddressOf SourceMeasureUnitMeasurePropertyChanged

    End Sub

    ''' <summary> Allows the derived device to take actions after opening. Adds subsystems and event
    ''' handlers. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnOpened()
        Try
            MyBase.OnOpened()
            Me.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Failed initiating controller node--closing this session;. {0}", ex.ToFullBlownString)
            Me.CloseSession()
        End Try
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " STATUS "

    ''' <summary> Gets or sets the Status Subsystem. </summary>
    ''' <value> The Status Subsystem. </value>
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
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " DISPLAY "

    ''' <summary> Gets or sets the Display Subsystem. </summary>
    ''' <value> Display Subsystem. </value>
    Public Property DisplaySubsystem As DisplaySubsystem

    ''' <summary> Handle the display subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As DisplaySubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplaySubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, DisplaySubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " STATUS "

#End Region

#Region " SYSTEM "

    ''' <summary>
    ''' Gets or sets the System Subsystem.
    ''' </summary>
    ''' <value>The System Subsystem.</value>
    Public Property SystemSubsystem As SystemSubsystem

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, SystemSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling System Subsystem property changed Event;. Failed property {0}. {1}",
                               e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " SMU "

    ''' <summary> Gets or sets the Smu Subsystem. </summary>
    ''' <value> Smu Subsystem. </value>
    Public Property SourceMeasureUnit As SourceMeasureUnit

    ''' <summary> Handle the source measure unit subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As VI.Tsp.SourceMeasureUnitBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> Source Measure Unit subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SourceMeasureSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, VI.Tsp.SourceMeasureUnitBase), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " SMU: CURRENT SOURCE "

    ''' <summary> Gets or sets source measure unit current source. </summary>
    ''' <value> The source measure unit current source. </value>
    Public Property SourceMeasureUnitCurrentSource As SourceMeasureUnitCurrentSource

    ''' <summary> Handles the Source Measure Unit Current Source  subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As VI.Tsp.SourceMeasureUnitCurrentSource, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.Level)
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} current source level set at {1};. ", Me.ResourceName, subsystem.Level)
        End Select
    End Sub

    ''' <summary> Source Measure Unit Current Source subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SourceMeasureUnitCurrentSourcePropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, VI.Tsp.SourceMeasureUnitCurrentSource), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " SMU: MEASURE "

    ''' <summary> Gets or sets source measure unit measure. </summary>
    ''' <value> The source measure unit measure. </value>
    Public Property SourceMeasureUnitMeasure As SourceMeasureUnitMeasure

    ''' <summary> Handles the Source Measure Unit Measure subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As VI.Tsp.SourceMeasureUnitMeasure, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeVoltageEnabled)
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} Measure auto voltage range enabled set to {1};. ",
                                   Me.ResourceName, subsystem.AutoRangeVoltageEnabled)
        End Select
    End Sub

    ''' <summary> Source Measure Unit Current Source subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SourceMeasureUnitMeasurePropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, VI.Tsp.SourceMeasureUnitMeasure), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#End Region

#Region " SERVICE REQUEST "

    ''' <summary> Reads the event registers after receiving a service request. </summary>
    Protected Overrides Sub ProcessServiceRequest()
        Me.StatusSubsystem.ReadRegisters()
        If Me.StatusSubsystem.ErrorAvailable Then
            Me.StatusSubsystem.QueryDeviceErrors()
        End If
    End Sub

#End Region

End Class

