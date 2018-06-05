Imports isr.VI.ExceptionExtensions
''' <summary> The Meter device. </summary>
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
        MyBase.New(StatusSubsystem.Validated(statusSubsystem))
        If statusSubsystem IsNot Nothing Then
            statusSubsystem.ExpectedLanguage = VI.Pith.Ieee488.Syntax.LanguageTsp
            AddHandler My.Settings.PropertyChanged, AddressOf Me._Settings_PropertyChanged
        End If
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
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_DisplaySubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_LocalNodeSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="Disposed @Subsystems")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' ?listeners must clear, otherwise closing could raise an exception.
                ' Me.Talker.Listeners.Clear()
                If Me.IsDeviceOpen Then Me.OnClosing(New ComponentModel.CancelEventArgs)
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

#Region " SESSION "

    ''' <summary> Allows the derived device to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnClosing(ByVal e As ComponentModel.CancelEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnClosing(e)
        If Not e.Cancel Then
            Me.TriggerSubsystem = Nothing
            Me.Buffer1Subsystem = Nothing
            Me.MultimeterSubsystem = Nothing
            Me.DisplaySubsystem = Nothing
            Me.LocalNodeSubsystem = Nothing
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
            ' check the language status. 
            Me.StatusSubsystem.QueryLanguage()
            If Me.StatusSubsystem.LanguageValidated Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Device language {Me.StatusSubsystem.Language} validated;. ")
            Else
                ' set the device to the correct language.
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Setting device to {Me.StatusSubsystem.ExpectedLanguage} language;. ")
                Me.StatusSubsystem.ApplyLanguage(Me.StatusSubsystem.ExpectedLanguage)
                If Not Me.StatusSubsystem.LanguageValidated Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Incorrect {NameOf(StatusSubsystemBase.Language)} settings {Me.StatusSubsystem.Language}; must be {Me.StatusSubsystem.ExpectedLanguage}")
                End If
            End If
            If Not e.Cancel Then
                Me.SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
                Me.LocalNodeSubsystem = New LocalNodeSubsystem(Me.StatusSubsystem)
                Me.DisplaySubsystem = New DisplaySubsystem(Me.StatusSubsystem)
                Me.MultimeterSubsystem = New MultimeterSubsystem(Me.StatusSubsystem)
                Me.TriggerSubsystem = New TriggerSubsystem(Me.StatusSubsystem)
                Me.Buffer1Subsystem = New BufferSubsystem(VI.Tsp2.BufferSubsystemBase.DefaultBuffer1Name, Me.StatusSubsystem)
            End If
        End If
    End Sub

    ''' <summary> Allows the derived device to take actions after opening. Adds subsystems and event
    ''' handlers. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnOpened()
        Try
            MyBase.OnOpened()
            Me.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.None)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              $"Failed opening {NameOf(Device)}--session will close;. {ex.ToFullBlownString}")
            Me.CloseSession()
        End Try

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
            Me.StatusSubsystem.QueryLanguage()
            ' report but do not cancel allowing the calling program to take action and reset the device.
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              $"Incorrect {NameOf(StatusSubsystemBase.Language)} settings {Me.StatusSubsystem.Language}; must be {Me.StatusSubsystem.ExpectedLanguage}")
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " BUFFER1 "

    Private _Buffer1Subsystem As BufferSubsystem
    ''' <summary>
    ''' Gets or sets the Buffer1 Subsystem.
    ''' </summary>
    ''' <value>The Buffer1 Subsystem.</value>
    Public Property Buffer1Subsystem As BufferSubsystem
        Get
            Return Me._Buffer1Subsystem
        End Get
        Set(value As BufferSubsystem)
            If Me._Buffer1Subsystem IsNot Nothing Then
                RemoveHandler Me.Buffer1Subsystem.PropertyChanged, AddressOf Me.Buffer1SubsystemPropertyChanged
                Me.RemoveSubsystem(Me.Buffer1Subsystem)
                Me.Buffer1Subsystem.Dispose()
                Me._Buffer1Subsystem = Nothing
            End If
            Me._Buffer1Subsystem = value
            If Me._Buffer1Subsystem IsNot Nothing Then
                AddHandler Me.Buffer1Subsystem.PropertyChanged, AddressOf Buffer1SubsystemPropertyChanged
                Me.AddSubsystem(Me.Buffer1Subsystem)
            End If
        End Set
    End Property

    ''' <summary> Buffer1 subsystem property changed. </summary>
    ''' <param name="subsystem"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As VI.BufferSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K7500.BufferSubsystem.ActualPointCount)
        End Select
    End Sub

    ''' <summary> Buffer1 subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Buffer1SubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Device.Buffer1Subsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, VI.BufferSubsystemBase), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
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
    ''' <param name="subsystem"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As DisplaySubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K7500.DisplaySubsystem.DisplayScreen)
        End Select
    End Sub

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplaySubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(DisplaySubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, DisplaySubsystem), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
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
    ''' <param name="subsystem"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As LocalNodeSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K7500.LocalNodeSubsystem.ShowEvents)
        End Select
    End Sub

    ''' <summary> LocalNode subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub LocalNodeSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(LocalNodeSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, LocalNodeSubsystem), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " MULTIMETER "

    Private _MultimeterSubsystem As MultimeterSubsystem
    ''' <summary>
    ''' Gets or sets the Multimeter Subsystem.
    ''' </summary>
    ''' <value>The Multimeter Subsystem.</value>
    Public Property MultimeterSubsystem As MultimeterSubsystem
        Get
            Return Me._MultimeterSubsystem
        End Get
        Set(value As MultimeterSubsystem)
            If Me._MultimeterSubsystem IsNot Nothing Then
                RemoveHandler Me.MultimeterSubsystem.PropertyChanged, AddressOf Me.MultimeterSubsystemPropertyChanged
                Me.RemoveSubsystem(Me.MultimeterSubsystem)
                Me.MultimeterSubsystem.Dispose()
                Me._MultimeterSubsystem = Nothing
            End If
            Me._MultimeterSubsystem = value
            If Me._MultimeterSubsystem IsNot Nothing Then
                AddHandler Me.MultimeterSubsystem.PropertyChanged, AddressOf MultimeterSubsystemPropertyChanged
                Me.AddSubsystem(Me.MultimeterSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="subsystem"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As VI.MultimeterSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.MultimeterSubsystemBase.PowerLineCycles)
        End Select
    End Sub

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MultimeterSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Device.MultimeterSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, VI.MultimeterSubsystemBase), e.PropertyName)
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " TRIGGER "

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
                RemoveHandler Me.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
                Me.RemoveSubsystem(Me.TriggerSubsystem)
                Me.TriggerSubsystem.Dispose()
                Me._TriggerSubsystem = Nothing
            End If
            Me._TriggerSubsystem = value
            If Me._TriggerSubsystem IsNot Nothing Then
                AddHandler Me.TriggerSubsystem.PropertyChanged, AddressOf TriggerSubsystemPropertyChanged
                Me.AddSubsystem(Me.TriggerSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Trigger subsystem property changed. </summary>
    ''' <param name="subsystem"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As VI.TriggerSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.TriggerSubsystemBase.Delay)
        End Select
    End Sub

    ''' <summary> Trigger subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TriggerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Device.TriggerSubsystem)}.{e.PropertyName} change"
        Try
            Me.HandlePropertyChange(TryCast(sender, VI.TriggerSubsystemBase), e.PropertyName)
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

#Region " EVENTS: READ EVENT REGISTERS "

#If False Then
    ''' <summary> Reads the event registers after receiving a service request. </summary>
    Protected Overrides Sub ProcessServiceRequest()
        Me.StatusSubsystem.ReadEventRegisters()
        If Me.StatusSubsystem.ErrorAvailable Then Me.StatusSubsystem.QueryDeviceErrors()
    End Sub
#End If

#End Region

#Region " MY SETTINGS "

    ''' <summary> Opens the settings editor. </summary>
    Public Shared Sub OpenSettingsEditor()
        Using f As Core.Pith.ConfigurationEditor = Core.Pith.ConfigurationEditor.Get
            f.Text = "K7500 Settings Editor"
            f.ShowDialog(isr.VI.Tsp2.K7500.My.MySettings.Default)
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
    End Sub

    ''' <summary> Handle the Platform property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>

    Private Overloads Sub HandlePropertyChange(ByVal sender As My.MySettings, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(My.MySettings.TraceLogLevel)
                Me.ApplyTalkerTraceLevel(Core.Pith.ListenerType.Logger, sender.TraceLogLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.TraceLogLevel}")
            Case NameOf(My.MySettings.TraceShowLevel)
                Me.ApplyTalkerTraceLevel(Core.Pith.ListenerType.Display, sender.TraceShowLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.TraceShowLevel}")
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

