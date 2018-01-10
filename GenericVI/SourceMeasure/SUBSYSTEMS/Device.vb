''' <summary> Implements a generic source measure device. </summary>
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

    ''' <summary> Initializes a new instance of the <see cref="SourceMeasure.Device" /> class. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializeTimeout = TimeSpan.FromMilliseconds(5000)
        Me.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter()
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
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ArmLayerSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ContactCheckLimit", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ComplianceLimit", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_CompositeLimit", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_DigitalOutput", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_UpperLowerLimit", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_Calculate2Subsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_TriggerSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceVoltageSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceCurrentSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseVoltageSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseResistanceSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseCurrentSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_RouteSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_OutputSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MeasureVoltageSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MeasureSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MeasureCurrentSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_FormatSubsystem", Justification:="@Closing")>
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
        If Me._FormatSubsystem IsNot Nothing Then
            RemoveHandler Me.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
        End If
        If Me._SystemSubsystem IsNot Nothing Then
            RemoveHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged
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
        AddHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged

        ' better add before the format subsystem, which reset initializes the readings.
        Me._MeasureSubsystem = New MeasureSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.MeasureSubsystem)

        ' the measure subsystem reading are initialized when the format system is reset
        Me._FormatSubsystem = New FormatSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.FormatSubsystem)
        AddHandler Me.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged

        Me._OutputSubsystem = New OutputSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.OutputSubsystem)

        Me._RouteSubsystem = New RouteSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.RouteSubsystem)

        Me._SenseCurrentSubsystem = New SenseCurrentSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SenseCurrentSubsystem)

        Me._SenseVoltageSubsystem = New SenseVoltageSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SenseVoltageSubsystem)

        Me._SenseResistanceSubsystem = New SenseResistanceSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SenseResistanceSubsystem)

        Me._SenseSubsystem = New SenseSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SenseSubsystem)

        Me._SourceCurrentSubsystem = New SourceCurrentSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SourceCurrentSubsystem)

        Me._SourceVoltageSubsystem = New SourceVoltageSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SourceVoltageSubsystem)

        Me._SourceSubsystem = New SourceSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SourceSubsystem)

        Me._ArmLayerSubsystem = New ArmLayerSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.ArmLayerSubsystem)

        Me._TriggerSubsystem = New TriggerSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.TriggerSubsystem)

        Me._Calculate2Subsystem = New Calculate2Subsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.Calculate2Subsystem)

        Me._ContactCheckLimit = New ContactCheckLimit(Me.StatusSubsystem)
        Me.AddSubsystem(Me.ContactCheckLimit)

        Me._ComplianceLimit = New ComplianceLimit(Me.StatusSubsystem)
        Me.AddSubsystem(Me.ComplianceLimit)

        Me._CompositeLimit = New CompositeLimit(Me.StatusSubsystem)
        Me.AddSubsystem(Me.CompositeLimit)

        Me._DigitalOutput = New DigitalOutput(Me.StatusSubsystem)
        Me.AddSubsystem(Me.DigitalOutput)

        Me._UpperLowerLimit = New UpperLowerLimit(Me.StatusSubsystem)
        Me.AddSubsystem(Me.UpperLowerLimit)

    End Sub

#End Region

#Region " SUBSYSTEMS "

    ''' <summary>
    ''' Gets or sets the arm layer subsystem.
    ''' </summary>
    ''' <value>The arm layer subsystem.</value>
    Public Property ArmLayerSubsystem As ArmLayerSubsystem

    ''' <summary> Gets or sets the Composite Limit. </summary>
    ''' <value> The Composite limit. </value>
    Public Property CompositeLimit As CompositeLimit

    ''' <summary> Gets or sets the compliance limit. </summary>
    ''' <value> The compliance limit. </value>
    Public Property ComplianceLimit As ComplianceLimit

    ''' <summary> Gets or sets the contact check limit. </summary>
    ''' <value> The contact check limit. </value>
    Public Property ContactCheckLimit As ContactCheckLimit

    ''' <summary> Gets or sets the upper lower limit. </summary>
    ''' <value> The upper lower limit. </value>
    Public Property UpperLowerLimit As UpperLowerLimit

    ''' <summary> Gets or sets the calculate 2 subsystem. </summary>
    ''' <value> The calculate 2 subsystem. </value>
    Public Property Calculate2Subsystem As Calculate2Subsystem

    ''' <summary> Gets or sets the digital output. </summary>
    ''' <value> The digital output. </value>
    Public Property DigitalOutput As DigitalOutput

    ''' <summary>
    ''' Gets or sets the Measure subsystem.
    ''' </summary>
    ''' <value>The Measure subsystem.</value>
    Public Property MeasureSubsystem As MeasureSubsystem

    ''' <summary>
    ''' Gets or sets the Output subsystem.
    ''' </summary>
    ''' <value>The Output subsystem.</value>
    Public Property OutputSubsystem As OutputSubsystem

    ''' <summary>
    ''' Gets or sets the Route subsystem.
    ''' </summary>
    ''' <value>The Route subsystem.</value>
    Public Property RouteSubsystem As RouteSubsystem

    ''' <summary>
    ''' Gets or sets the Sense Current subsystem.
    ''' </summary>
    ''' <value>The Sense Current subsystem.</value>
    Public Property SenseCurrentSubsystem As SenseCurrentSubsystem

    ''' <summary>
    ''' Gets or sets the Sense Resistance subsystem.
    ''' </summary>
    ''' <value>The Sense Resistance subsystem.</value>
    Public Property SenseResistanceSubsystem As SenseResistanceSubsystem

    ''' <summary>
    ''' Gets or sets the Sense Voltage subsystem.
    ''' </summary>
    ''' <value>The Sense Voltage subsystem.</value>
    Public Property SenseVoltageSubsystem As SenseVoltageSubsystem

    ''' <summary>
    ''' Gets or sets the Sense subsystem.
    ''' </summary>
    ''' <value>The Sense subsystem.</value>
    Public Property SenseSubsystem As SourceMeasure.SenseSubsystem

    ''' <summary>
    ''' Gets or sets the Source Current subsystem.
    ''' </summary>
    ''' <value>The Source Current subsystem.</value>
    Public Property SourceCurrentSubsystem As SourceCurrentSubsystem

    ''' <summary>
    ''' Gets or sets the Source Voltage subsystem.
    ''' </summary>
    ''' <value>The Source Voltage subsystem.</value>
    Public Property SourceVoltageSubsystem As SourceVoltageSubsystem

    ''' <summary>
    ''' Gets or sets the Source subsystem.
    ''' </summary>
    ''' <value>The Source subsystem.</value>
    Public Property SourceSubsystem As SourceSubsystem

    ''' <summary>
    ''' Gets or sets the Trigger Subsystem.
    ''' </summary>
    ''' <value>The Trigger Subsystem.</value>
    Public Property TriggerSubsystem As TriggerSubsystem

#Region " FORMAT "

    ''' <summary> Gets or sets the Format Subsystem. </summary>
    ''' <value> The Format Subsystem. </value>
    Public Property FormatSubsystem As FormatSubsystem

    ''' <summary> Handle the Format subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As FormatSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.Elements)
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

    ''' <summary>
    ''' Gets or sets the System Subsystem.
    ''' </summary>
    ''' <value>The System Subsystem.</value>
    Public Property SystemSubsystem As SystemSubsystem

    ''' <summary> Handles the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Overloads Sub OnPropertyChanged(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.Options)
                ' read the contact check option after reading options.
                subsystem.QuerySupportsContactCheck()
        End Select
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, SystemSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#End Region

#Region " MIXED SUBSYSTEMS: OUTPUT / MEASURE "

    ''' <summary>
    ''' Sets the specified voltage and current limit and turns on the output.
    ''' </summary>
    ''' <param name="voltage">The voltage.</param>
    ''' <param name="currentLimit">The current limit.</param>
    ''' <param name="overvoltageLimit">The over voltage limit.</param>
    ''' <remarks> See 2400 system manual page 102 of 592. </remarks>
    Public Sub OutputOn(ByVal voltage As Double, ByVal currentLimit As Double, ByVal overvoltageLimit As Double)

        ' turn output off.
        Me.OutputSubsystem.ApplyOutputOnState(False)

        Me.SourceSubsystem.WriteFunctionMode(SourceFunctionModes.VoltageDC)

        ' Set the voltage
        Me.SourceVoltageSubsystem.ApplyLevel(voltage)

        ' Set the over voltage level
        Me.SourceVoltageSubsystem.ApplyProtectionLevel(overvoltageLimit)

        ' Turn on over current protection
        Me.SenseCurrentSubsystem.ApplyProtectionLevel(currentLimit)

        Me.SenseSubsystem.ApplyFunctionModes(VI.Scpi.SenseFunctionModes.Current Or VI.Scpi.SenseFunctionModes.Voltage)

        Me.SenseSubsystem.ApplyConcurrentSenseEnabled(True)

        ' turn output on.
        Me.OutputSubsystem.ApplyOutputOnState(True)

        Me.StatusSubsystem.QueryOperationCompleted()

        ' operation completion is not sufficient to ensure that the Device has hit the correct voltage.

    End Sub

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
            f.Text = "Source Meter Settings Editor"
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

