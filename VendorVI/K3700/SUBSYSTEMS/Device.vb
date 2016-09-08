Imports isr.VI
Imports isr.VI.Tsp
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
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MultimeterSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ChannelSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="Disposed @Subsystems")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me.IsDeviceOpen Then Me.OnClosing(New System.ComponentModel.CancelEventArgs)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception disposing device", "Exception details: {0}", ex)
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

#End Region

#Region " PUBLISHER "

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
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If e?.Cancel Then Return

        If Me._StatusSubsystem IsNot Nothing Then
            RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
        End If

        If Me.MultimeterSubsystem IsNot Nothing Then
            RemoveHandler Me.MultimeterSubsystem.PropertyChanged, AddressOf MultimeterSubsystemPropertyChanged
        End If

        If Me.ChannelSubsystem IsNot Nothing Then
            RemoveHandler Me.ChannelSubsystem.PropertyChanged, AddressOf ChannelSubsystemPropertyChanged
        End If

        If Me.SystemSubsystem IsNot Nothing Then
            RemoveHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged
        End If

        If Me.SlotsSubsystem IsNot Nothing Then
            RemoveHandler Me.SlotsSubsystem.PropertyChanged, AddressOf SlotsSubsystemPropertyChanged
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

        Me.ChannelSubsystem = New ChannelSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.ChannelSubsystem)
        AddHandler Me.ChannelSubsystem.PropertyChanged, AddressOf ChannelSubsystemPropertyChanged

        Me.SlotsSubsystem = New SlotsSubsystem(6, Me.StatusSubsystem)
        Me.AddSubsystem(Me.SlotsSubsystem)
        AddHandler Me.SlotsSubsystem.PropertyChanged, AddressOf SlotsSubsystemPropertyChanged

        Me.MultimeterSubsystem = New MultimeterSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.MultimeterSubsystem)
        AddHandler Me.MultimeterSubsystem.PropertyChanged, AddressOf MultimeterSubsystemPropertyChanged

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
                               "Failed initiating controller node--closing this session;. Details: {0}.", ex)
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
    Protected Overloads Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, StatusSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0} changed event;. Details: {1}", e?.PropertyName, ex)
        End Try
    End Sub

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
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " CHANNEL "

    ''' <summary> Gets or sets the Channel Subsystem. </summary>
    ''' <value> Channel Subsystem. </value>
    Public Property ChannelSubsystem As ChannelSubsystem

    ''' <summary> Channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As ChannelSubsystem, ByVal propertyName As String)
        Try
            If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
                Select Case propertyName
                    Case NameOf(sender.ClosedChannels)

                End Select
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. Details: {1}.",
                         propertyName, ex.Message)
        End Try
    End Sub

    ''' <summary> Channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Me.OnPropertyChanged(TryCast(sender, ChannelSubsystem), e?.PropertyName)
    End Sub

#End Region

#Region " MULTIMETER "

    ''' <summary> Gets or sets the Multimeter Subsystem. </summary>
    ''' <value> Multimeter Subsystem. </value>
    Public Property MultimeterSubsystem As MultimeterSubsystem

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As MultimeterSubsystem, ByVal propertyName As String)
        Try
            If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
                Select Case propertyName
                    Case NameOf(sender.Reading)

                End Select
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. Details: {1}.",
                         propertyName, ex.Message)
        End Try
    End Sub

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MultimeterSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Me.OnPropertyChanged(TryCast(sender, MultimeterSubsystem), e?.PropertyName)
    End Sub

#End Region

#Region " SLOTS "

    ''' <summary> Gets or sets the Slots Subsystem. </summary>
    ''' <value> Slots Subsystem. </value>
    Public Property SlotsSubsystem As SlotsSubsystem

    ''' <summary> Slots subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As SlotsSubsystem, ByVal propertyName As String)
        Try
            If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
                Select Case propertyName
                End Select
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. Details: {1}.",
                         propertyName, ex.Message)
        End Try
    End Sub

    ''' <summary> Slots subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SlotsSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Me.OnPropertyChanged(TryCast(sender, SlotsSubsystem), e?.PropertyName)
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

