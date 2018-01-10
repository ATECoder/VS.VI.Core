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
        Me.InitializeTimeout = TimeSpan.FromMilliseconds(5000)
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
                'listeners must clear, otherwise closing could raise an exception.
                Me.Talker.Listeners.Clear()
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
    Protected Overrides Sub OnClosing(ByVal e As ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If e?.Cancel Then Return
        If Me._SystemSubsystem IsNot Nothing Then
            'RemoveHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged
        End If
        ' If Me._StatusSubsystem IsNot Nothing Then RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
        Me.StatusSubsystem = Nothing
        Me.Subsystems.DisposeItems()
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnOpening(ByVal e As ComponentModel.CancelEventArgs)
        MyBase.OnOpening(e)
        If e?.Cancel Then Return

        ' STATUS must be the first subsystem.
        Me.StatusSubsystem = New StatusSubsystem(Me.Session)
        'Me.AddSubsystem(Me.StatusSubsystem)
        'AddHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged

        Me._SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SystemSubsystem)
        'AddHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged

        Me._MeasureCurrentSubsystem = New MeasureCurrentSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.MeasureCurrentSubsystem)

        Me._MeasureVoltageSubsystem = New MeasureVoltageSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.MeasureVoltageSubsystem)

        Me._OutputSubsystem = New OutputSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.OutputSubsystem)

        Me._SourceCurrentSubsystem = New SourceCurrentSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SourceCurrentSubsystem)

        Me._SourceVoltageSubsystem = New SourceVoltageSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SourceVoltageSubsystem)
    End Sub

#End Region

#Region " SUBSYSTEMS "

    ''' <summary>
    ''' Gets or sets the Measure Current subsystem.
    ''' </summary>
    ''' <value>The Measure subsystem.</value>
    Public Property MeasureCurrentSubsystem As MeasureCurrentSubsystem

    ''' <summary>
    ''' Gets or sets the Measure Voltage subsystem.
    ''' </summary>
    ''' <value>The Measure subsystem.</value>
    Public Property MeasureVoltageSubsystem As MeasureVoltageSubsystem

    ''' <summary>
    ''' Gets or sets the Output subsystem.
    ''' </summary>
    ''' <value>The Output subsystem.</value>
    Public Property OutputSubsystem As OutputSubsystem

    ''' <summary>
    ''' Gets or sets the Source Current subsystem.
    ''' </summary>
    ''' <value>The Source subsystem.</value>
    Public Property SourceCurrentSubsystem As SourceCurrentSubsystem

    ''' <summary>
    ''' Gets or sets the Source Voltage subsystem.
    ''' </summary>
    ''' <value>The Source subsystem.</value>
    Public Property SourceVoltageSubsystem As SourceVoltageSubsystem

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

    ''' <summary> Gets or sets the System Subsystem. </summary>
    ''' <value> The System Subsystem. </value>
    Public Property SystemSubsystem As SystemSubsystem

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
