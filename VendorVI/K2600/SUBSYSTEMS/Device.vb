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
        AddHandler My.Settings.PropertyChanged, AddressOf Me._Settings_PropertyChanged
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
                ' ?listeners must clear, otherwise closing could raise an exception.
                ' Me.Talker.Listeners.Clear()
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

    ''' <summary> Initializes the Device. Used after reset to set a desired initial state. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Try
            Me.Session.StoreTimeout(Me.StatusSubsystem.InitializeTimeout)
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing error queue;. ")
            ' clear the error queue on the controller node only.
            Me.StatusSubsystem.ClearErrorQueue()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Exception ignored clearing error queue;. {0}", ex.ToFullBlownString)
        Finally
            Me.Session.RestoreTimeout()
        End Try

        Try
            ' flush the input buffer in case the instrument has some leftovers.
            Me.Session.DiscardUnreadData()
            If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Data discarded after clearing read buffer;. Data: {0}.", Me.Session.DiscardedData)
            End If
        Catch ex As NativeException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception ignored clearing read buffer;. {0}", ex.ToFullBlownString)
        End Try

        Try
            ' flush write may cause the instrument to send off a new data.
            Me.Session.DiscardUnreadData()
            If Not String.IsNullOrWhiteSpace(Me.Session.DiscardedData) Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Unread data discarded after discarding unset data;. Data: {0}.", Me.Session.DiscardedData)
            End If
        Catch ex As NativeException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
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

    ''' <summary> Capture synchronization context. </summary>
    ''' <param name="syncContext"> Context for the synchronization. </param>
    Public Overrides Sub CaptureSyncContext(ByVal syncContext As Threading.SynchronizationContext)
        MyBase.CaptureSyncContext(syncContext)
        Me.Session?.CaptureSyncContext(syncContext)
        Me.Subsystems?.CaptureSyncContext(syncContext)
    End Sub

    ''' <summary> Allows the derived device to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If e?.Cancel Then Return
        Me.MeasureResistanceSubsystem = Nothing
        Me.MeasureVoltageSubsystem = Nothing
        Me.SourceSubsystem = Nothing
        Me.SenseSubsystem = Nothing
        Me.CurrentSourceSubsystem = Nothing
        Me.DisplaySubsystem = Nothing
        Me.ContactSubsystem = Nothing
        Me.LocalNodeSubsystem = Nothing
        Me.SourceMeasureUnit = Nothing
        Me.SystemSubsystem = Nothing
        Me.StatusSubsystem = Nothing
        Me.Subsystems.DisposeItems()
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnOpening(e As System.ComponentModel.CancelEventArgs)
        MyBase.OnOpening(e)
        If e?.Cancel Then Return
        ' STATUS must be the first subsystem.
        Me.StatusSubsystem = New StatusSubsystem(Me.Session)
        Me.SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
        Me.SourceMeasureUnit = New SourceMeasureUnit(Me.StatusSubsystem)
        Me.LocalNodeSubsystem = New LocalNodeSubsystem(Me.StatusSubsystem)
        Me.DisplaySubsystem = New DisplaySubsystem(Me.StatusSubsystem)
        Me.ContactSubsystem = New ContactSubsystem(Me.StatusSubsystem)
        Me.CurrentSourceSubsystem = New CurrentSourceSubsystem(Me.StatusSubsystem)
        Me.SenseSubsystem = New SenseSubsystem(Me.StatusSubsystem)
        Me.SourceSubsystem = New SourceSubsystem(Me.StatusSubsystem)
        Me.MeasureVoltageSubsystem = New MeasureVoltageSubsystem(Me.StatusSubsystem)
        Me.MeasureResistanceSubsystem = New MeasureResistanceSubsystem(Me.StatusSubsystem)
    End Sub

    ''' <summary> Allows the derived device to take actions after opening. Adds subsystems and event
    ''' handlers. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnOpened()
        Try
            MyBase.OnOpened()
            Me.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)
            Me.CaptureSyncContext(Me.CapturedSyncContext)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Failed initiating controller node--closing this session;. {0}", ex.ToFullBlownString)
            Me.CloseSession()
        End Try
    End Sub

#End Region

#Region " SUBSYSTEMS "

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
                Me.RemoveSubsystem(Me.StatusSubsystem)
                Me.StatusSubsystem.Dispose()
                Me._StatusSubsystem = Nothing
            End If
            Me._StatusSubsystem = value
            If Me._StatusSubsystem IsNot Nothing Then
                AddHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
                Me.AddSubsystem(Me.StatusSubsystem)
            End If
            Me.StatusSubsystemBase = value
        End Set
    End Property

    ''' <summary> Executes the subsystem property changed action. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        Me.OnPropertyChanged(CType(subsystem, StatusSubsystem), propertyName)
    End Sub

    ''' <summary> Executes the subsystem property changed action. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As StatusSubsystem, ByVal propertyName As String)
        MyBase.OnPropertyChanged(subsystem, propertyName)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(StatusSubsystem)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
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
    Private Overloads Sub OnPropertyChanged(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
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
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(SystemSubsystem)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " CURRENT SOURCE "

    Private _CurrentSourceSubsystem As CurrentSourceSubsystem
    ''' <summary>
    ''' Gets or sets the Current Source Subsystem.
    ''' </summary>
    ''' <value>The CurrentSource Subsystem.</value>
    Public Property CurrentSourceSubsystem As CurrentSourceSubsystem
        Get
            Return Me._CurrentSourceSubsystem
        End Get
        Set(value As CurrentSourceSubsystem)
            If Me._CurrentSourceSubsystem IsNot Nothing Then
                RemoveHandler Me.CurrentSourceSubsystem.PropertyChanged, AddressOf Me.CurrentSourceSubsystemPropertyChanged
                Me.SourceMeasureUnit.Remove(Me.CurrentSourceSubsystem)
                Me.RemoveSubsystem(Me.CurrentSourceSubsystem)
                Me.CurrentSourceSubsystem.Dispose()
                Me._CurrentSourceSubsystem = Nothing
            End If
            Me._CurrentSourceSubsystem = value
            If Me._CurrentSourceSubsystem IsNot Nothing Then
                AddHandler Me.CurrentSourceSubsystem.PropertyChanged, AddressOf CurrentSourceSubsystemPropertyChanged
                Me.AddSubsystem(Me.CurrentSourceSubsystem)
                Me.SourceMeasureUnit.Add(Me.CurrentSourceSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Current Source subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As CurrentSourceSubsystem, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.Level)
        End Select
    End Sub

    ''' <summary> Current Source subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub CurrentSourceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As CurrentSourceSubsystem = TryCast(sender, CurrentSourceSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(CurrentSourceSubsystem)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " CONTACT "

    Private _ContactSubsystem As ContactSubsystem
    ''' <summary>
    ''' Gets or sets the Contact Subsystem.
    ''' </summary>
    ''' <value>The Contact Subsystem.</value>
    Public Property ContactSubsystem As ContactSubsystem
        Get
            Return Me._ContactSubsystem
        End Get
        Set(value As ContactSubsystem)
            If Me._ContactSubsystem IsNot Nothing Then
                RemoveHandler Me.ContactSubsystem.PropertyChanged, AddressOf Me.ContactSubsystemPropertyChanged
                Me.SourceMeasureUnit.Remove(Me.ContactSubsystem)
                Me.RemoveSubsystem(Me.ContactSubsystem)
                Me.ContactSubsystem.Dispose()
                Me._ContactSubsystem = Nothing
            End If
            Me._ContactSubsystem = value
            If Me._ContactSubsystem IsNot Nothing Then
                AddHandler Me.ContactSubsystem.PropertyChanged, AddressOf ContactSubsystemPropertyChanged
                Me.AddSubsystem(Me.ContactSubsystem)
                Me.SourceMeasureUnit.Add(Me.ContactSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Contact subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As ContactSubsystem, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.ContactCheckSpeedMode)
        End Select
    End Sub

    ''' <summary> Contact subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ContactSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As ContactSubsystem = TryCast(sender, ContactSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(ContactSubsystem)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " DISPLAY "

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
                RemoveHandler Me.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
                Me.RemoveSubsystem(Me.DisplaySubsystem)
                Me.DisplaySubsystem.Dispose()
                Me._DisplaySubsystem = Nothing
            End If
            Me._DisplaySubsystem = value
            If Me._DisplaySubsystem IsNot Nothing Then
                AddHandler Me.DisplaySubsystem.PropertyChanged, AddressOf DisplaySubsystemPropertyChanged
                Me.AddSubsystem(Me.DisplaySubsystem)
            End If
        End Set
    End Property

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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(DisplaySubsystem)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " SOURCE MEASURE UNIT "

    Private _SourceMeasureUnit As SourceMeasureUnit
    ''' <summary>
    ''' Gets or sets the Source Measure Unit.
    ''' </summary>
    ''' <value>The Source Measure Unit.</value>
    Public Property SourceMeasureUnit As SourceMeasureUnit
        Get
            Return Me._SourceMeasureUnit
        End Get
        Set(value As SourceMeasureUnit)
            If Me._SourceMeasureUnit IsNot Nothing Then
                RemoveHandler Me.SourceMeasureUnit.PropertyChanged, AddressOf Me.SourceMeasureUnitPropertyChanged
                Me.SourceMeasureUnit.Remove(Me.SourceMeasureUnit)
                Me.RemoveSubsystem(Me.SourceMeasureUnit)
                Me.SourceMeasureUnit.Dispose()
                Me._SourceMeasureUnit = Nothing
            End If
            Me._SourceMeasureUnit = value
            If Me._SourceMeasureUnit IsNot Nothing Then
                AddHandler Me.SourceMeasureUnit.PropertyChanged, AddressOf SourceMeasureUnitPropertyChanged
                Me.AddSubsystem(Me.SourceMeasureUnit)
                Me.SourceMeasureUnit.Add(Me.SourceMeasureUnit)
            End If
        End Set
    End Property

    ''' <summary> Source Measure Unit property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As SourceMeasureUnit, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.ResourceName)
        End Select
    End Sub

    ''' <summary> Source Measure Unit property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SourceMeasureUnitPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As SourceMeasureUnit = TryCast(sender, SourceMeasureUnit)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(SourceMeasureUnit)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " LOCAL NODE "

    Private _LocalNodeSubsystem As LocalNodeSubsystem
    ''' <summary>
    ''' Gets or sets the Local Node Subsystem.
    ''' </summary>
    ''' <value>The Local Node Subsystem.</value>
    Public Property LocalNodeSubsystem As LocalNodeSubsystem
        Get
            Return Me._LocalNodeSubsystem
        End Get
        Set(value As LocalNodeSubsystem)
            If Me._LocalNodeSubsystem IsNot Nothing Then
                RemoveHandler Me.LocalNodeSubsystem.PropertyChanged, AddressOf Me.LocalNodeSubsystemPropertyChanged
                Me.RemoveSubsystem(Me.LocalNodeSubsystem)
                Me.LocalNodeSubsystem.Dispose()
                Me._LocalNodeSubsystem = Nothing
            End If
            Me._LocalNodeSubsystem = value
            If Me._LocalNodeSubsystem IsNot Nothing Then
                AddHandler Me.LocalNodeSubsystem.PropertyChanged, AddressOf LocalNodeSubsystemPropertyChanged
                Me.AddSubsystem(Me.LocalNodeSubsystem)
            End If
        End Set
    End Property

    ''' <summary> LocalNode subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As LocalNodeSubsystem, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.ShowPrompts)
        End Select
    End Sub

    ''' <summary> LocalNode subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub LocalNodeSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As LocalNodeSubsystem = TryCast(sender, LocalNodeSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(LocalNodeSubsystem)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " SENSE "

    Private _SenseSubsystem As SenseSubsystem
    ''' <summary>
    ''' Gets or sets the Sense Subsystem.
    ''' </summary>
    ''' <value>The Sense Subsystem.</value>
    Public Property SenseSubsystem As SenseSubsystem
        Get
            Return Me._SenseSubsystem
        End Get
        Set(value As SenseSubsystem)
            If Me._SenseSubsystem IsNot Nothing Then
                RemoveHandler Me.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
                Me.SourceMeasureUnit.Remove(Me.SenseSubsystem)
                Me.RemoveSubsystem(Me.SenseSubsystem)
                Me.SenseSubsystem.Dispose()
                Me._SenseSubsystem = Nothing
            End If
            Me._SenseSubsystem = value
            If Me._SenseSubsystem IsNot Nothing Then
                AddHandler Me.SenseSubsystem.PropertyChanged, AddressOf SenseSubsystemPropertyChanged
                Me.AddSubsystem(Me.SenseSubsystem)
                Me.SourceMeasureUnit.Add(Me.SenseSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Sense subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As SenseSubsystem, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.SenseMode)
        End Select
    End Sub

    ''' <summary> Sense subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As SenseSubsystem = TryCast(sender, SenseSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(SenseSubsystem)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " SOURCE "

    Private _SourceSubsystem As SourceSubsystem
    ''' <summary>
    ''' Gets or sets the Source Subsystem.
    ''' </summary>
    ''' <value>The Source Subsystem.</value>
    Public Property SourceSubsystem As SourceSubsystem
        Get
            Return Me._SourceSubsystem
        End Get
        Set(value As SourceSubsystem)
            If Me._SourceSubsystem IsNot Nothing Then
                RemoveHandler Me.SourceSubsystem.PropertyChanged, AddressOf Me.SourceSubsystemPropertyChanged
                Me.SourceMeasureUnit.Remove(Me.SourceSubsystem)
                Me.RemoveSubsystem(Me.SourceSubsystem)
                Me.SourceSubsystem.Dispose()
                Me._SourceSubsystem = Nothing
            End If
            Me._SourceSubsystem = value
            If Me._SourceSubsystem IsNot Nothing Then
                AddHandler Me.SourceSubsystem.PropertyChanged, AddressOf SourceSubsystemPropertyChanged
                Me.AddSubsystem(Me.SourceSubsystem)
                Me.SourceMeasureUnit.Add(Me.SourceSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Source subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub OnPropertyChanged(ByVal sender As SourceSubsystem, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.SourceFunction)
        End Select
    End Sub

    ''' <summary> Source subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SourceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As SourceSubsystem = TryCast(sender, SourceSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(SourceSubsystem)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " MEASURE RESISTANCE "

    Private _MeasureResistanceSubsystem As MeasureResistanceSubsystem
    ''' <summary>
    ''' Gets or sets the Measure Resistance Subsystem.
    ''' </summary>
    ''' <value>The Measure Resistance Subsystem.</value>
    Public Property MeasureResistanceSubsystem As MeasureResistanceSubsystem
        Get
            Return Me._MeasureResistanceSubsystem
        End Get
        Set(value As MeasureResistanceSubsystem)
            If Me._MeasureResistanceSubsystem IsNot Nothing Then
                RemoveHandler Me.MeasureResistanceSubsystem.PropertyChanged, AddressOf Me.MeasureResistanceSubsystemPropertyChanged
                Me.SourceMeasureUnit.Remove(Me.MeasureResistanceSubsystem)
                Me.RemoveSubsystem(Me.MeasureResistanceSubsystem)
                Me.MeasureResistanceSubsystem.Dispose()
                Me._MeasureResistanceSubsystem = Nothing
            End If
            Me._MeasureResistanceSubsystem = value
            If Me._MeasureResistanceSubsystem IsNot Nothing Then
                AddHandler Me.MeasureResistanceSubsystem.PropertyChanged, AddressOf MeasureResistanceSubsystemPropertyChanged
                Me.AddSubsystem(Me.MeasureResistanceSubsystem)
                Me.SourceMeasureUnit.Add(Me.MeasureResistanceSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Handles the Source Measure Unit Measure subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As VI.Tsp.MeasureResistanceSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.Resistance)
        End Select
    End Sub

    ''' <summary> Measure resistance subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, VI.Tsp.MeasureResistanceSubsystemBase), e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " MEASURE VOLTAGE "

    Private _MeasureVoltageSubsystem As MeasureVoltageSubsystem
    ''' <summary>
    ''' Gets or sets the Measure Voltage Subsystem.
    ''' </summary>
    ''' <value>The Measure Voltage Subsystem.</value>
    Public Property MeasureVoltageSubsystem As MeasureVoltageSubsystem
        Get
            Return Me._MeasureVoltageSubsystem
        End Get
        Set(value As MeasureVoltageSubsystem)
            If Me._MeasureVoltageSubsystem IsNot Nothing Then
                RemoveHandler Me.MeasureVoltageSubsystem.PropertyChanged, AddressOf Me.MeasureVoltageSubsystemPropertyChanged
                Me.SourceMeasureUnit.Remove(Me.MeasureVoltageSubsystem)
                Me.RemoveSubsystem(Me.MeasureVoltageSubsystem)
                Me.MeasureVoltageSubsystem.Dispose()
                Me._MeasureVoltageSubsystem = Nothing
            End If
            Me._MeasureVoltageSubsystem = value
            If Me._MeasureVoltageSubsystem IsNot Nothing Then
                AddHandler Me.MeasureVoltageSubsystem.PropertyChanged, AddressOf MeasureVoltageSubsystemPropertyChanged
                Me.AddSubsystem(Me.MeasureVoltageSubsystem)
                Me.SourceMeasureUnit.Add(Me.MeasureVoltageSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Handles the Source Measure Unit Measure subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As VI.Tsp.MeasureVoltageSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeVoltageEnabled)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} Measure auto voltage range enabled set to {1};. ",
                                   Me.ResourceName, subsystem.AutoRangeVoltageEnabled)
        End Select
    End Sub

    ''' <summary> Measure Voltage subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureVoltageSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, VI.Tsp.MeasureVoltageSubsystemBase), e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
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
            Me.StatusSubsystem.QueryDeviceErrors()
        End If
    End Sub
#End If

#End Region

#Region " MY SETTINGS "

    ''' <summary> Opens the settings editor. </summary>
    Public Shared Sub OpenSettingsEditor()
        Using f As Core.Pith.ConfigurationEditor = Core.Pith.ConfigurationEditor.Get
            f.Text = "K2600 Settings Editor"
            f.ShowDialog(My.MySettings.Default)
        End Using
    End Sub

    ''' <summary> Applies the settings. </summary>
    Protected Overrides Sub ApplySettings()
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
                Me.ApplyTalkerTraceLevel(Core.Pith.ListenerType.Logger, sender.TraceLogLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Trace log level changed to {sender.TraceLogLevel}")
            Case NameOf(sender.TraceShowLevel)
                Me.ApplyTalkerTraceLevel(Core.Pith.ListenerType.Display, sender.TraceShowLevel)
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

    ''' <summary> Identifies talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

#End Region

End Class

