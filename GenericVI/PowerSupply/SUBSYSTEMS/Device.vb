Imports isr.VI.ExceptionExtensions
''' <summary> Implements a Power Supply device. </summary>
''' <remarks> An instrument is defined, for the purpose of this library, as a device with a front
''' panel. </remarks>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class Device
    Inherits DeviceBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="PowerSupply.Device" /> class. </summary>
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

#Region " IDisposable Support "

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceVoltageSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceCurrentSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_OutputSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MeasureVoltageSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MeasureCurrentSubsystem", Justification:="@Closing")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' ?listeners must clear, otherwise closing could raise an exception.
                ' Me.Talker.Listeners.Clear()
                If Me.IsDeviceOpen Then Me.OnClosing(New isr.Core.Pith.CancelDetailsEventArgs)
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
        Me.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
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
    Protected Overrides Sub OnClosing(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        MyBase.OnClosing(e)
        If Not e.Cancel Then
            Me.MeasureCurrentSubsystem = Nothing
            Me.MeasureVoltageSubsystem = Nothing
            Me.OutputSubsystem = Nothing
            Me.SourceCurrentSubsystem = Nothing
            Me.SourceVoltageSubsystem = Nothing
            Me.SystemSubsystem = Nothing
            Me.StatusSubsystem = Nothing
            Me.Subsystems.DisposeItems()
        End If
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnOpening(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnOpening(e)
        If Not e.Cancel Then
            ' STATUS must be the first subsystem.
            Me.StatusSubsystem = New StatusSubsystem(Me.Session)
            Me.SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
            Me.MeasureCurrentSubsystem = New MeasureCurrentSubsystem(Me.StatusSubsystem)
            Me.MeasureVoltageSubsystem = New MeasureVoltageSubsystem(Me.StatusSubsystem)
            Me.OutputSubsystem = New OutputSubsystem(Me.StatusSubsystem)
            Me.SourceCurrentSubsystem = New SourceCurrentSubsystem(Me.StatusSubsystem)
            Me.SourceVoltageSubsystem = New SourceVoltageSubsystem(Me.StatusSubsystem)
        End If
    End Sub

#End Region

#Region " STATUS SESSION "

    ''' <summary> Allows the derived device status subsystem to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnStatusClosing(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnClosing(e)
        If Not e.Cancel Then
            Me.StatusSubsystem = Nothing
            Me.Subsystems.DisposeItems()
        End If
    End Sub

    ''' <summary> Allows the derived device status subsystem to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnStatusOpening(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnOpening(e)
        If Not e.Cancel Then
            ' check the language status. 
            Me.StatusSubsystem = New StatusSubsystem(Me.Session)
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

    Private _MeasureCurrentSubsystem As MeasureCurrentSubsystem
    ''' <summary>
    ''' Gets or sets the Measure Current Subsystem.
    ''' </summary>
    ''' <value>The Measure Current Subsystem.</value>
    Public Property MeasureCurrentSubsystem As MeasureCurrentSubsystem
        Get
            Return Me._MeasureCurrentSubsystem
        End Get
        Set(value As MeasureCurrentSubsystem)
            If Me._MeasureCurrentSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.MeasureCurrentSubsystem)
                Me.MeasureCurrentSubsystem.Dispose()
                Me._MeasureCurrentSubsystem = Nothing
            End If
            Me._MeasureCurrentSubsystem = value
            If Me._MeasureCurrentSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.MeasureCurrentSubsystem)
            End If
        End Set
    End Property

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
                Me.RemoveSubsystem(Me.MeasureVoltageSubsystem)
                Me.MeasureVoltageSubsystem.Dispose()
                Me._MeasureVoltageSubsystem = Nothing
            End If
            Me._MeasureVoltageSubsystem = value
            If Me._MeasureVoltageSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.MeasureVoltageSubsystem)
            End If
        End Set
    End Property

    Private _OutputSubsystem As OutputSubsystem
    ''' <summary>
    ''' Gets or sets the Output Subsystem.
    ''' </summary>
    ''' <value>The Output Subsystem.</value>
    Public Property OutputSubsystem As OutputSubsystem
        Get
            Return Me._OutputSubsystem
        End Get
        Set(value As OutputSubsystem)
            If Me._OutputSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.OutputSubsystem)
                Me.OutputSubsystem.Dispose()
                Me._OutputSubsystem = Nothing
            End If
            Me._OutputSubsystem = value
            If Me._OutputSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.OutputSubsystem)
            End If
        End Set
    End Property

    Private _SourceCurrentSubsystem As SourceCurrentSubsystem
    ''' <summary>
    ''' Gets or sets the Source Current Subsystem.
    ''' </summary>
    ''' <value>The Source Current Subsystem.</value>
    Public Property SourceCurrentSubsystem As SourceCurrentSubsystem
        Get
            Return Me._SourceCurrentSubsystem
        End Get
        Set(value As SourceCurrentSubsystem)
            If Me._SourceCurrentSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SourceCurrentSubsystem)
                Me.SourceCurrentSubsystem.Dispose()
                Me._SourceCurrentSubsystem = Nothing
            End If
            Me._SourceCurrentSubsystem = value
            If Me._SourceCurrentSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SourceCurrentSubsystem)
            End If
        End Set
    End Property

    Private _SourceVoltageSubsystem As SourceVoltageSubsystem
    ''' <summary>
    ''' Gets or sets the Source Voltage Subsystem.
    ''' </summary>
    ''' <value>The Source Voltage Subsystem.</value>
    Public Property SourceVoltageSubsystem As SourceVoltageSubsystem
        Get
            Return Me._SourceVoltageSubsystem
        End Get
        Set(value As SourceVoltageSubsystem)
            If Me._SourceVoltageSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SourceVoltageSubsystem)
                Me.SourceVoltageSubsystem.Dispose()
                Me._SourceVoltageSubsystem = Nothing
            End If
            Me._SourceVoltageSubsystem = value
            If Me._SourceVoltageSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SourceVoltageSubsystem)
            End If
        End Set
    End Property

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
#End Region

#Region " MIXED SUBSYSTEMS: OUTPUT / MEASURE "

    ''' <summary>
    ''' Sets the specified voltage and current limit and turns on the power supply.
    ''' </summary>
    ''' <param name="voltage">The voltage.</param>
    ''' <param name="currentLimit">The current limit.</param>
    ''' <param name="overvoltageLimit">The over voltage limit.</param>
    Public Sub OutputOn(ByVal voltage As Double, ByVal currentLimit As Double, ByVal overvoltageLimit As Double)

        ' Set the voltage
        Me.SourceVoltageSubsystem.ApplyLevel(voltage)

        ' Set the over voltage level
        Me.SourceVoltageSubsystem.ApplyProtectionLevel(overvoltageLimit)

        ' Turn on over current protection
        Me.SourceCurrentSubsystem.ApplyProtectionEnabled(True)

        ' Set the current level
        Me.SourceCurrentSubsystem.ApplyLevel(currentLimit)

        ' turn output on.
        Me.OutputSubsystem.ApplyOutputOnState(True)

        Me.StatusSubsystem.QueryOperationCompleted()

        ' operation completion is not sufficient to ensure that the Device has hit the correct voltage.

    End Sub

    ''' <summary>
    ''' Turns the power supply on waiting for the voltage to reach the desired level.
    ''' </summary>
    ''' <param name="voltage">The voltage.</param>
    ''' <param name="currentLimit">The current limit.</param>
    ''' <param name="delta">The delta.</param>
    ''' <param name="voltageLimit">The over voltage limit.</param>
    ''' <param name="timeout">The timeout for awaiting for the voltage to reach the level.</param>
    ''' <returns><c>True</c> if successful <see cref="OutputOn">Output On</see> and in range;
    '''          <c>False</c> otherwise.</returns>
    ''' <remarks>SCPI Command: OUTP ON</remarks>
    Public Function OutputOn(ByVal voltage As Double, ByVal currentLimit As Double,
                                 ByVal delta As Double, ByVal voltageLimit As Double, ByVal timeout As TimeSpan) As Boolean
        Me.OutputOn(voltage, currentLimit, voltageLimit)
        Return Me.MeasureVoltageSubsystem.AwaitLevel(voltage, delta, timeout)
    End Function

#End Region

#Region " SERVICE REQUEST "

#If False Then
    ''' <summary> Reads the event registers after receiving a service request. </summary>
    ''' <remarks> Handled by the <see cref="DeviceBase"/></remarks>
    Protected Overrides Sub ProcessServiceRequest()
        Me.StatusSubsystem.ReadEventRegisters()
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
#End If

#End Region

#Region " MY SETTINGS "

    ''' <summary> Opens the settings editor. </summary>
    Public Shared Sub OpenSettingsEditor()
        Using f As Core.Pith.ConfigurationEditor = Core.Pith.ConfigurationEditor.Get
            f.Text = "Power Supply Settings Editor"
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
            Case NameOf(sender.InitializeTimeout)
                Me.StatusSubsystemBase.InitializeTimeout = sender.InitializeTimeout
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.InitializeTimeout}")
            Case NameOf(sender.ResetRefractoryPeriod)
                Me.StatusSubsystemBase.ResetRefractoryPeriod = sender.ResetRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.ResetRefractoryPeriod}")
            Case NameOf(sender.DeviceClearRefractoryPeriod)
                Me.StatusSubsystemBase.DeviceClearRefractoryPeriod = sender.DeviceClearRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.DeviceClearRefractoryPeriod}")
            Case NameOf(sender.InitRefractoryPeriod)
                Me.StatusSubsystemBase.InitRefractoryPeriod = sender.InitRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.InitRefractoryPeriod}")
            Case NameOf(sender.ClearRefractoryPeriod)
                Me.StatusSubsystemBase.ClearRefractoryPeriod = sender.ClearRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.ClearRefractoryPeriod}")
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

    ''' <summary> Identifies the talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        Mybase.IdentifyTalkers()
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region


End Class
