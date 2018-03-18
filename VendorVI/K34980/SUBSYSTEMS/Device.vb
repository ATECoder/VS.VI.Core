Imports isr.VI.ExceptionExtensions
''' <summary> Implements a Keysight 34980 Meter/Scanner device. </summary>
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
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class Device
    Inherits DeviceBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="K34980.Device" /> class. </summary>
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

#Region "IDisposable Support"

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_TriggerSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_TraceSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseVoltageSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseResistanceSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseFourWireResistanceSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseCurrentSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_RouteSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MeasureSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_InstrumentSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_FormatSubsystem", Justification:="Disposed @Subsystems")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' ?listeners must clear, otherwise closing could raise an exception.
                ' Me.Talker.Listeners.Clear()
                If Me.IsDeviceOpen Then Me.OnClosing(New Core.Pith.CancelDetailsEventArgs)
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
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnClosing(e)
        If Not e.Cancel Then
            Me.StatusSubsystem = Nothing
            Me.SystemSubsystem = Nothing
            ' must be added before the measure system
            Me.RouteSubsystem = Nothing
            Me.TriggerSubsystem = Nothing
            If Me.InstrumentSubsystem?.DmmInstalled Then
                ' must be added before the format system
                Me.MeasureSubsystem = Nothing
                Me.FormatSubsystem = Nothing
                Me.SenseSubsystem = Nothing
                Me.SenseVoltageSubsystem = Nothing
                Me.SenseCurrentSubsystem = Nothing
                Me.SenseFourWireResistanceSubsystem = Nothing
                Me.SenseResistanceSubsystem = Nothing
            End If
            Me.InstrumentSubsystem = Nothing
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
            ' must be added before the measure system
            Me.InstrumentSubsystem = New InstrumentSubsystem(Me.StatusSubsystem)
            Me.InstrumentSubsystem.QueryDmmInstalled()
            Me.StatusSubsystem.DmmInstalled = Me.InstrumentSubsystem.DmmInstalled.GetValueOrDefault(False)
            Me.RouteSubsystem = New RouteSubsystem(Me.StatusSubsystem)
            Me.TriggerSubsystem = New TriggerSubsystem(Me.StatusSubsystem)

            If Me.InstrumentSubsystem.DmmInstalled Then
                ' must be added before the format system
                Me.MeasureSubsystem = New MeasureSubsystem(Me.StatusSubsystem)
                ' the measure subsystem reading are initialized when the format system is reset
                Me.FormatSubsystem = New FormatSubsystem(Me.StatusSubsystem)
                Me.SenseSubsystem = New SenseSubsystem(Me.StatusSubsystem)
                Me.SenseVoltageSubsystem = New SenseVoltageSubsystem(Me.StatusSubsystem)
                Me.SenseCurrentSubsystem = New SenseCurrentSubsystem(Me.StatusSubsystem)
                Me.SenseFourWireResistanceSubsystem = New SenseFourWireResistanceSubsystem(Me.StatusSubsystem)
                Me.SenseResistanceSubsystem = New SenseResistanceSubsystem(Me.StatusSubsystem)
            End If
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

    Private _InstrumentSubsystem As InstrumentSubsystem
    ''' <summary>
    ''' Gets or sets the Instrument Subsystem.
    ''' </summary>
    ''' <value>The Instrument Subsystem.</value>
    Public Property InstrumentSubsystem As InstrumentSubsystem
        Get
            Return Me._InstrumentSubsystem
        End Get
        Set(value As InstrumentSubsystem)
            If Me._InstrumentSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.InstrumentSubsystem)
                Me.InstrumentSubsystem.Dispose()
                Me._InstrumentSubsystem = Nothing
            End If
            Me._InstrumentSubsystem = value
            If Me._InstrumentSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.InstrumentSubsystem)
            End If
        End Set
    End Property

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
                Me.RemoveSubsystem(Me.MeasureSubsystem)
                Me.MeasureSubsystem.Dispose()
                Me._MeasureSubsystem = Nothing
            End If
            Me._MeasureSubsystem = value
            If Me._MeasureSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.MeasureSubsystem)
            End If
        End Set
    End Property

    Private _RouteSubsystem As RouteSubsystem
    ''' <summary>
    ''' Gets or sets the Route Subsystem.
    ''' </summary>
    ''' <value>The Route Subsystem.</value>
    Public Property RouteSubsystem As RouteSubsystem
        Get
            Return Me._RouteSubsystem
        End Get
        Set(value As RouteSubsystem)
            If Me._RouteSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.RouteSubsystem)
                Me.RouteSubsystem.Dispose()
                Me._RouteSubsystem = Nothing
            End If
            Me._RouteSubsystem = value
            If Me._RouteSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.RouteSubsystem)
            End If
        End Set
    End Property


    Private _SenseSubsystem As SenseSubsystem
    ''' <summary>
    ''' Gets or sets the Sense  Subsystem.
    ''' </summary>
    ''' <value>The Sense  Subsystem.</value>
    Public Property SenseSubsystem As SenseSubsystem
        Get
            Return Me._SenseSubsystem
        End Get
        Set(value As SenseSubsystem)
            If Me._SenseSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SenseSubsystem)
                Me.SenseSubsystem.Dispose()
                Me._SenseSubsystem = Nothing
            End If
            Me._SenseSubsystem = value
            If Me._SenseSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SenseSubsystem)
            End If
        End Set
    End Property

    Private _SenseVoltageSubsystem As SenseVoltageSubsystem
    ''' <summary>
    ''' Gets or sets the Sense Voltage Subsystem.
    ''' </summary>
    ''' <value>The Sense Voltage Subsystem.</value>
    Public Property SenseVoltageSubsystem As SenseVoltageSubsystem
        Get
            Return Me._SenseVoltageSubsystem
        End Get
        Set(value As SenseVoltageSubsystem)
            If Me._SenseVoltageSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SenseVoltageSubsystem)
                Me.SenseVoltageSubsystem.Dispose()
                Me._SenseVoltageSubsystem = Nothing
            End If
            Me._SenseVoltageSubsystem = value
            If Me._SenseVoltageSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SenseVoltageSubsystem)
            End If
        End Set
    End Property

    Private _SenseCurrentSubsystem As SenseCurrentSubsystem
    ''' <summary>
    ''' Gets or sets the Sense Current Subsystem.
    ''' </summary>
    ''' <value>The Sense Current Subsystem.</value>
    Public Property SenseCurrentSubsystem As SenseCurrentSubsystem
        Get
            Return Me._SenseCurrentSubsystem
        End Get
        Set(value As SenseCurrentSubsystem)
            If Me._SenseCurrentSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SenseCurrentSubsystem)
                Me.SenseCurrentSubsystem.Dispose()
                Me._SenseCurrentSubsystem = Nothing
            End If
            Me._SenseCurrentSubsystem = value
            If Me._SenseCurrentSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SenseCurrentSubsystem)
            End If
        End Set
    End Property

    Private _SenseFourWireResistanceSubsystem As SenseFourWireResistanceSubsystem
    ''' <summary>
    ''' Gets or sets the Sense Four Wire Resistance Subsystem.
    ''' </summary>
    ''' <value>The Sense Four Wire Resistance Subsystem.</value>
    Public Property SenseFourWireResistanceSubsystem As SenseFourWireResistanceSubsystem
        Get
            Return Me._SenseFourWireResistanceSubsystem
        End Get
        Set(value As SenseFourWireResistanceSubsystem)
            If Me._SenseFourWireResistanceSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SenseFourWireResistanceSubsystem)
                Me.SenseFourWireResistanceSubsystem.Dispose()
                Me._SenseFourWireResistanceSubsystem = Nothing
            End If
            Me._SenseFourWireResistanceSubsystem = value
            If Me._SenseFourWireResistanceSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SenseFourWireResistanceSubsystem)
            End If
        End Set
    End Property

    Private _SenseResistanceSubsystem As SenseResistanceSubsystem
    ''' <summary>
    ''' Gets or sets the Sense Resistance Subsystem.
    ''' </summary>
    ''' <value>The Sense Resistance Subsystem.</value>
    Public Property SenseResistanceSubsystem As SenseResistanceSubsystem
        Get
            Return Me._SenseResistanceSubsystem
        End Get
        Set(value As SenseResistanceSubsystem)
            If Me._SenseResistanceSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SenseResistanceSubsystem)
                Me.SenseResistanceSubsystem.Dispose()
                Me._SenseResistanceSubsystem = Nothing
            End If
            Me._SenseResistanceSubsystem = value
            If Me._SenseResistanceSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SenseResistanceSubsystem)
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

#Region " FORMAT "

    Private _FormatSubsystem As FormatSubsystem
    ''' <summary>
    ''' Gets or sets the Format Subsystem.
    ''' </summary>
    ''' <value>The Format Subsystem.</value>
    Public Property FormatSubsystem As FormatSubsystem
        Get
            Return Me._FormatSubsystem
        End Get
        Set(value As FormatSubsystem)
            If Me._FormatSubsystem IsNot Nothing Then
                RemoveHandler Me.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
                Me.RemoveSubsystem(Me.FormatSubsystem)
                Me.FormatSubsystem.Dispose()
                Me._FormatSubsystem = Nothing
            End If
            Me._FormatSubsystem = value
            If Me._FormatSubsystem IsNot Nothing Then
                AddHandler Me.FormatSubsystem.PropertyChanged, AddressOf FormatSubsystemPropertyChanged
                Me.AddSubsystem(Me.FormatSubsystem)
            End If
        End Set
    End Property

    ''' <summary> Handle the Format subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As FormatSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K34980.FormatSubsystem.Elements)
                Me.MeasureSubsystem.Readings.Initialize(subsystem.Elements)
        End Select
    End Sub

    ''' <summary> Format subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub FormatSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, FormatSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}",
                             e.PropertyName, ex.ToFullBlownString)
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
            f.Text = "K34980 Settings Editor"
            f.ShowDialog(My.MySettings.Default)
        End Using
    End Sub

    ''' <summary> Applies the settings. </summary>
    Protected Overrides Sub ApplySettings()
        Dim settings As My.MySettings = My.MySettings.Default
        Me.OnSettingsPropertyChanged(settings, NameOf(My.MySettings.TraceLogLevel))
        Me.OnSettingsPropertyChanged(settings, NameOf(My.MySettings.TraceShowLevel))
    End Sub

    ''' <summary> Handle the Platform property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSettingsPropertyChanged(ByVal sender As My.MySettings, ByVal propertyName As String)
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
