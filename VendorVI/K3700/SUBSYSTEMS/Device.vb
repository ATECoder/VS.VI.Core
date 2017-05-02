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

    ''' <summary> Creates a new Device. </summary>
    ''' <remarks> David, 5/1/2017. </remarks>
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


#Region " I Disposable Support"

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MultimeterSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ChannelSubsystem", Justification:="Disposed @Subsystems")>
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

        If Me.MultimeterSubsystem IsNot Nothing Then
            RemoveHandler Me.MultimeterSubsystem.PropertyChanged, AddressOf MultimeterSubsystemPropertyChanged
        End If

        If Me.DisplaySubsystem IsNot Nothing Then
            RemoveHandler Me.DisplaySubsystem.PropertyChanged, AddressOf DisplaySubsystemPropertyChanged
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

        Me.DisplaySubsystem = New DisplaySubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.DisplaySubsystem)
        AddHandler Me.DisplaySubsystem.PropertyChanged, AddressOf DisplaySubsystemPropertyChanged

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
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overloads Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As StatusSubsystem = TryCast(sender, StatusSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Status subsystem {1} property change;. {2}",
                               Me.ResourceName, e.PropertyName, ex.ToFullBlownString)
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
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As SystemSubsystem = TryCast(sender, SystemSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling System subsystem {1} property change;. {2}",
                               Me.ResourceName, e.PropertyName, ex.ToFullBlownString)
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
    Private Overloads Sub OnPropertyChanged(ByVal sender As ChannelSubsystem, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.ClosedChannels)
        End Select
    End Sub

    ''' <summary> Channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As ChannelSubsystem = TryCast(sender, ChannelSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Channel subsystem {1} property change;. {2}",
                               Me.ResourceName, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " DISPLAY "

    ''' <summary> Gets or sets the Display Subsystem. </summary>
    ''' <value> Display Subsystem. </value>
    Public Property DisplaySubsystem As DisplaySubsystem

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As DisplaySubsystem, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.DisplayScreen)
        End Select
    End Sub

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplaySubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As DisplaySubsystem = TryCast(sender, DisplaySubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Display subsystem {1} property change;. {2}",
                               Me.ResourceName, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " MULTIMETER "

    ''' <summary> Gets or sets the Multimeter Subsystem. </summary>
    ''' <value> Multimeter Subsystem. </value>
    Public Property MultimeterSubsystem As MultimeterSubsystem

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As MultimeterSubsystem, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.Reading)
        End Select
    End Sub

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MultimeterSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As MultimeterSubsystem = TryCast(sender, MultimeterSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Multimeter subsystem {1} property change;. {2}",
                               Me.ResourceName, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " SLOTS "

    ''' <summary> Gets or sets the Slots Subsystem. </summary>
    ''' <value> Slots Subsystem. </value>
    Public Property SlotsSubsystem As SlotsSubsystem

    ''' <summary> Slots subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As SlotsSubsystem, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.Slots)
        End Select
    End Sub

    ''' <summary> Slots subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SlotsSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As SlotsSubsystem = TryCast(sender, SlotsSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Slots subsystem {1} property change;. {2}",
                               Me.ResourceName, e.PropertyName, ex.ToFullBlownString)
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

