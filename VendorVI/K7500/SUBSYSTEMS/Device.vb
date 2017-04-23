Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Implements a Keithley 7500 Meter device. </summary>
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

    ''' <summary> Initializes a new instance of the <see cref="K7500.Device" /> class. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializeTimeout = TimeSpan.FromMilliseconds(5000)
        Me.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter(HardwareInterfaceType.Gpib,
                                                                           HardwareInterfaceType.Tcpip,
                                                                           HardwareInterfaceType.Usb)
    End Sub

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
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_FormatSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_DisplaySubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_Calculate2Subsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_Calculate2FourWireResistanceSubsystem", Justification:="Disposed @Subsystems")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                'listeners must clear, otherwise closing could raise an exception.
                Me.Talker?.Listeners.Clear()
                If Me.IsDeviceOpen Then Me.OnClosing(New ComponentModel.CancelEventArgs)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, $"Exception disposing device", $"Exception {ex.ToFullBlownString}")
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " I PRESETTABLE "

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

#Region " SESSION OPEN / CLOSE "

    ''' <summary> Allows the derived device to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnClosing(ByVal e As ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If e?.Cancel Then Return
        If Me._FormatSubsystem IsNot Nothing Then RemoveHandler Me.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
        If Me._StatusSubsystem IsNot Nothing Then RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        If Me._SenseSubsystem IsNot Nothing Then AddHandler Me.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
        Me.Subsystems.DisposeItems()
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnOpening(ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.OnOpening(e)
        If e?.Cancel Then Return

        ' STATUS must be the first subsystem.
        Me._StatusSubsystem = New StatusSubsystem(Me.Session)
        Me.AddSubsystem(Me.StatusSubsystem)
        AddHandler Me.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged

        Me._SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SystemSubsystem)
        'AddHandler Me.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged

        ' better add before the format subsystem, which reset initializes the readings.
        Me._MeasureSubsystem = New MeasureSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.MeasureSubsystem)

        ' the measure subsystem readings are set when the format system is reset.
        Me._FormatSubsystem = New FormatSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.FormatSubsystem)
        AddHandler Me.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged

        Me._Calculate2FourWireResistanceSubsystem = New Calculate2FourWireResistanceSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.Calculate2FourWireResistanceSubsystem)

        Me._DisplaySubsystem = New DisplaySubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.DisplaySubsystem)

        Me._RouteSubsystem = New RouteSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.RouteSubsystem)

        Me._SenseSubsystem = New SenseSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SenseSubsystem)
        AddHandler Me.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged

        Me._SenseVoltageSubsystem = New SenseVoltageSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SenseVoltageSubsystem)

        Me._SenseCurrentSubsystem = New SenseCurrentSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SenseCurrentSubsystem)

        Me._SenseResistanceSubsystem = New SenseResistanceSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SenseResistanceSubsystem)

        Me._SenseFourWireResistanceSubsystem = New SenseFourWireResistanceSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.SenseFourWireResistanceSubsystem)

        Me._TraceSubsystem = New TraceSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.TraceSubsystem)

        Me._TriggerSubsystem = New TriggerSubsystem(Me.StatusSubsystem)
        Me.AddSubsystem(Me.TriggerSubsystem)

    End Sub

#End Region

#Region " SUBSYSTEMS "

    ''' <summary> Gets or sets the Calculate2 Four Wire Resistance Subsystem. </summary>
    ''' <value> The Calculate2 Four Wire Resistance Subsystem. </value>
    Public Property Calculate2FourWireResistanceSubsystem As Calculate2FourWireResistanceSubsystem

    ''' <summary> Gets or sets the Display Subsystem. </summary>
    ''' <value> The Display Subsystem. </value>
    Public Property DisplaySubsystem As DisplaySubsystem

    ''' <summary> Gets or sets the measure Subsystem. </summary>
    ''' <value> The measure Subsystem. </value>
    Public Property MeasureSubsystem As MeasureSubsystem

    ''' <summary> Gets or sets the Route Subsystem. </summary>
    ''' <value> The Route Subsystem. </value>
    Public Property RouteSubsystem As RouteSubsystem

    ''' <summary> Gets or sets the Sense Subsystem. </summary>
    ''' <value> The Sense Subsystem. </value>
    Public Property SenseSubsystem As SenseSubsystem

    ''' <summary> Gets or sets the Sense Voltage Subsystem. </summary>
    ''' <value> The Sense Voltage Subsystem. </value>
    Public Property SenseVoltageSubsystem As SenseVoltageSubsystem

    ''' <summary> Gets or sets the Sense Current Subsystem. </summary>
    ''' <value> The Sense Current Subsystem. </value>
    Public Property SenseCurrentSubsystem As SenseCurrentSubsystem

    ''' <summary> Gets or sets the Sense Four Wire Resistance Subsystem. </summary>
    ''' <value> The Sense Four Wire Resistance Subsystem. </value>
    Public Property SenseFourWireResistanceSubsystem As SenseFourWireResistanceSubsystem

    ''' <summary> Gets or sets the Sense Resistance Subsystem. </summary>
    ''' <value> The Sense Resistance Subsystem. </value>
    Public Property SenseResistanceSubsystem As SenseResistanceSubsystem

    ''' <summary> Gets or sets the Trace Subsystem. </summary>
    ''' <value> The Trace Subsystem. </value>
    Public Property TraceSubsystem As TraceSubsystem

    ''' <summary> Gets or sets the Trigger Subsystem. </summary>
    ''' <value> The Trigger Subsystem. </value>
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
        Dim subsystem As FormatSubsystem = TryCast(sender, FormatSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              $"{Me.ResourceTitle} exception handling FORMAT.{e.PropertyName} change event;. {ex.ToFullBlownString}")
        End Try
    End Sub


#End Region

#Region " SENSE "

    ''' <summary> Executes the function modes changed action. </summary>
    ''' <remarks> David, 4/4/2017. </remarks>
    ''' <param name="value"> The value. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnFunctionModesChanged(ByVal value As SenseFunctionSubsystemBase)
        With value
            .QueryRange()
            .QueryAutoRangeEnabled()
            .QueryAutoZeroEnabled()
            .QueryPowerLineCycles()
        End With
    End Sub

    ''' <summary> Gets the selected sense subsystem. </summary>
    ''' <value> The selected sense subsystem. </value>
    Public ReadOnly Property SelectedSenseSubsystem() As SenseFunctionSubsystemBase
        Get
            Return Me.SelectSenseSubsystem(Me.SenseSubsystem.FunctionMode.GetValueOrDefault(VI.Scpi.SenseFunctionModes.VoltageDC))
        End Get
    End Property

    ''' <summary> Select sense subsystem. </summary>
    ''' <remarks> David, 4/4/2017. </remarks>
    ''' <param name="value"> The value. </param>
    ''' <returns> A SenseFunctionSubsystemBase. </returns>
    Public Function SelectSenseSubsystem(ByVal value As VI.Scpi.SenseFunctionModes) As SenseFunctionSubsystemBase
        Dim result As SenseFunctionSubsystemBase = Me.SenseVoltageSubsystem
        Select Case value
            Case VI.Scpi.SenseFunctionModes.CurrentDC
                result = Me.SenseCurrentSubsystem
            Case VI.Scpi.SenseFunctionModes.VoltageDC
                result = Me.SenseVoltageSubsystem
            Case VI.Scpi.SenseFunctionModes.Resistance
                result = Me.SenseResistanceSubsystem
            Case VI.Scpi.SenseFunctionModes.FourWireResistance
                result = Me.SenseFourWireResistanceSubsystem
            Case Else
        End Select
        Return result
    End Function

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")>
    Public Function SelectSenseSubsystem(ByVal subsystem As SenseSubsystem) As SenseFunctionSubsystemBase
        If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
        Dim value As VI.Scpi.SenseFunctionModes = subsystem.FunctionMode.GetValueOrDefault(VI.Scpi.SenseFunctionModes.VoltageDC)
        Return Me.SelectSenseSubsystem(value)
    End Function

    ''' <summary> Parse measurement unit. </summary>
    ''' <remarks> David, 4/4/2017. </remarks>
    ''' <param name="subsystem"> The subsystem. </param>
    Public Sub ParseMeasurementUnit(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.FunctionMode.HasValue Then
            Dim value As VI.Scpi.SenseFunctionModes = subsystem.FunctionMode.GetValueOrDefault(VI.Scpi.SenseFunctionModes.None)
            If value <> VI.Scpi.SenseFunctionModes.None Then
                If Not VI.Scpi.SenseSubsystemBase.TryParse(value, Me.MeasureSubsystem.Readings.Reading.Unit) Then
                    Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                       $"Failed parsing function mode '{value}' to a standard unit.")
                    Me.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Volt
                End If
                subsystem.Readings.Reading.Unit = Me.MeasureSubsystem.Readings.Reading.Unit
            End If
        End If
    End Sub

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.FunctionMode.HasValue Then
            Me.ParseMeasurementUnit(subsystem)
            Me.onFunctionModesChanged(Me.SelectSenseSubsystem(subsystem))
        End If
    End Sub

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.FunctionMode)
                Me.onFunctionModesChanged(subsystem)
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
            Me.OnSubsystemPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              $"{Me.ResourceTitle} exception handling SenseSubsystem.{e.PropertyName} change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary>
    ''' Gets or sets the Status Subsystem.
    ''' </summary>
    ''' <value>The Status Subsystem.</value>
    Public ReadOnly Property StatusSubsystem As StatusSubsystem

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As StatusSubsystem = TryCast(sender, StatusSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              $"{Me.ResourceTitle} exception handling STATUS.{e.PropertyName} change event;. {ex.ToFullBlownString}")
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
