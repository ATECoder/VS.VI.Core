Imports System.ComponentModel
Imports System.Threading
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.CheckBoxExtensions
Imports isr.Core.Controls.NumericUpDownExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
''' <summary> Provides a user interface for a Keithley 200X Device. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2008" by="David" revision="2.0.2936.x"> Create based on the 24xx
''' system classes. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
<System.ComponentModel.DisplayName("K7500 Panel"),
      System.ComponentModel.Description("Keithley 7500 Device Panel"),
      System.Drawing.ToolboxBitmap(GetType(K7500Panel))>
Public Class K7500Panel
    Inherits VI.Instrument.ResourcePanelBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(New Device)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Public Sub New(ByVal device As Device)
        MyBase.New(device)
        Me.InitializingComponents = True
        Me.InitializeComponent()
        Me.InitializingComponents = False
        Me._AssignDevice(device)
        ' note that the caption is not set if this is run inside the On Load function.
        With Me.TraceMessagesBox
            ' set defaults for the messages box.
            .ResetCount = 500
            .PresetCount = 250
            .ContainerPanel = Me._MessagesTabPage
        End With
        With Me._StartTriggerDelayNumeric.NumericUpDownControl
            .DecimalPlaces = 3
            .Minimum = 0
            .Maximum = 10
            .Value = 0.02D
        End With
        With Me._EndTriggerDelayNumeric.NumericUpDownControl
            .DecimalPlaces = 3
            .Minimum = 0
            .Maximum = 10
            .Value = 0.0D
        End With
        With Me._OpenLeadsBitPatternNumeric.NumericUpDownControl
            .Minimum = 1
            .Maximum = 63
            .Value = 16
        End With
        With Me._PassBitPatternNumeric.NumericUpDownControl
            .Minimum = 1
            .Maximum = 63
            .Value = 32
        End With
        With Me._FailLimit1BitPatternNumeric.NumericUpDownControl
            .Minimum = 1
            .Maximum = 63
            .Value = 48
        End With
        With Me._LowerLimit1Numeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 5000000D
            .DecimalPlaces = 3
            .Value = 9
        End With
        With Me._UpperLimit1Numeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 5000000D
            .DecimalPlaces = 3
            .Value = 11
        End With
        With Me._TriggerCountNumeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 268000000D
            .DecimalPlaces = 0
            .Value = 10
        End With
        With Me._ServiceRequestEnableBitmaskNumeric.NumericUpDownControl
            .Hexadecimal = True
            .Maximum = 255
            .Minimum = 0
            .Value = 0
        End With
        With Me._BufferSizeNumeric.NumericUpDownControl
            .CausesValidation = True
            .Minimum = 0
            .Maximum = 27500000
        End With
        Me.EnableTraceLevelControls
        Me._InterfaceStopWatch = New Stopwatch
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the isr.VI.Instrument.ResourcePanelBase and
    ''' optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.DebuggerNonUserCode()>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Try
                    If Me.Device IsNot Nothing Then Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", $"Exception {ex.ToFullBlownString}")
                End Try
                ' the device gets closed and disposed (if panel is device owner) in the base class
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " DEVICE "

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignDevice(ByVal value As Device)
        Me._Device = value
        Me._Device.CaptureSyncContext(Threading.SynchronizationContext.Current)
        ' Me.AddListeners()
        Me.OnDeviceOpenChanged(value)
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device)
        Me.IsDeviceOwner = False
        MyBase.AssignDevice(value)
        Me._AssignDevice(value)
    End Sub

    ''' <summary> Releases the device. </summary>
    Protected Overrides Sub ReleaseDevice()
        If Me.IsDeviceOwner Then
            MyBase.ReleaseDevice()
        Else
            Me._Device = Nothing
        End If
    End Sub

    ''' <summary> Gets a reference to the Keithley 7500 Device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overloads ReadOnly Property Device() As Device

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As DeviceBase)
        If Me.IsDeviceOpen Then
            Me._SimpleReadWriteControl.Connect(device?.Session)
        Else
            Me._SimpleReadWriteControl.Disconnect()
        End If
        For Each t As Windows.Forms.TabPage In Me._Tabs.TabPages
            If t IsNot Me._MessagesTabPage Then
                For Each c As Windows.Forms.Control In t.Controls : Me.RecursivelyEnable(c, Me.IsDeviceOpen) : Next
            End If
            ' this is clearly a .Net bug -- these controls are not found. 
        Next
    End Sub

    ''' <summary> Handles the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnDevicePropertyChanged(ByVal device As DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnDevicePropertyChanged(device, propertyName)
        Select Case propertyName
            Case NameOf(device.SessionServiceRequestEventEnabled)
                Me._SessionServiceRequestHandlerEnabledMenuItem.Checked = device.SessionServiceRequestEventEnabled
            Case NameOf(device.DeviceServiceRequestHandlerAdded)
                Me._DeviceServiceRequestHandlerEnabledMenuItem.Checked = device.DeviceServiceRequestHandlerAdded
            Case NameOf(device.SessionMessagesTraceEnabled)
                Me._SessionTraceEnabledMenuItem.Checked = device.SessionMessagesTraceEnabled
            Case NameOf(device.ServiceRequestEnableBitmask)
                Me._ServiceRequestEnableBitmaskNumeric.Value = device.ServiceRequestEnableBitmask
                Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = $"SRE:0b{Convert.ToString(device.ServiceRequestEnableBitmask, 2),8}".Replace(" ", "0")
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        AddHandler Me.Device.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
        AddHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
        AddHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
        AddHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
        AddHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
        AddHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
        AddHandler Me.Device.SenseFourWireResistanceSubsystem.PropertyChanged, AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged
        AddHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
        AddHandler Me.Device.TraceSubsystem.PropertyChanged, AddressOf Me.TraceSubsystemPropertyChanged
        AddHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        MyBase.DeviceOpened(sender, e)
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overrides Sub OnTitleChanged(ByVal value As String)
        Me._TitleLabel.Text = value
        Me._TitleLabel.Visible = Not String.IsNullOrWhiteSpace(value)
        MyBase.OnTitleChanged(value)
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.DeviceClosing(sender, e)
        If e?.Cancel Then Return
        If Me.IsDeviceOpen Then
            RemoveHandler Me.Device.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
            RemoveHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
            RemoveHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseFourWireResistanceSubsystem.PropertyChanged, AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
            RemoveHandler Me.Device.TraceSubsystem.PropertyChanged, AddressOf Me.TraceSubsystemPropertyChanged
            RemoveHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " FORMAT "

    ''' <summary> Handle the format subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As FormatSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.Elements)
                subsystem.ListElements(Me._ReadingComboBox.ComboBox, ReadingTypes.Units)
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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling FORMAT '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " MEASURE "

    ''' <summary> Displays the active reading caption and status. </summary>
    Private Sub DisplayActiveReading()
        Const clear As String = "    "
        Dim caption As String = clear
        Dim failureCaption As String = clear
        Dim failureToolTip As String = clear
        Dim tbdCaption As String = Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff")
        Me.InterfaceStopWatch.Stop()
        If Me.Device.MeasureSubsystem Is Nothing OrElse
            Me.Device.MeasureSubsystem.Readings Is Nothing OrElse
            Me.Device.MeasureSubsystem.Readings.ActiveReadingType = ReadingTypes.None Then
            caption = "-.------- :)"
        ElseIf Me.Device.MeasureSubsystem.Readings.IsEmpty Then
            caption = Me.Device.MeasureSubsystem.Readings.ActiveAmountCaption
        Else
            caption = Me.Device.MeasureSubsystem.Readings.ActiveAmountCaption
            Dim metaStatus As MetaStatus = Me.Device.MeasureSubsystem.Readings.ActiveMetaStatus
            If metaStatus.HasValue Then
                failureCaption = $"{metaStatus.ToShortDescription(""),4}"
                failureToolTip = metaStatus.ToLongDescription("")
                If String.IsNullOrEmpty(failureToolTip) Then
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Instruments parsed reading elements.")
                Else
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, failureToolTip)
                End If
            End If
        End If
        Me._ReadingToolStripStatusLabel.SafeTextSetter(caption)
        Me._FailureCodeToolStripStatusLabel.SafeTextSetter(failureCaption)
        Me._FailureCodeToolStripStatusLabel.SafeToolTipTextSetter(failureToolTip)
        Me._TbdToolStripStatusLabel.SafeTextSetter(tbdCaption)
    End Sub

    ''' <summary> Handles the Measure subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As MeasureSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then
            Return
        Else
            Select Case propertyName
                Case NameOf(subsystem.LastReading)
                    Me._LastReadingTextBox.SafeTextSetter(subsystem.LastReading)
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                       "Measure message: {0}.", subsystem.LastReading.InsertCommonEscapeSequences)
                Case NameOf(subsystem.MeasurementAvailable)
                    Me.DisplayActiveReading()
            End Select
        End If
    End Sub

    ''' <summary> Measure subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, MeasureSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling MEASURE '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " ROUTE "

    ''' <summary> Handle the Route subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As RouteSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.TerminalsMode)
                Me._ReadTerminalStateButton.CheckState = (subsystem.TerminalsMode = RouteTerminalsMode.Front).ToCheckState
                Windows.Forms.Application.DoEvents()
        End Select
    End Sub

    ''' <summary> Route subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031: DoNotCatchGeneralExceptionTypes")>
    Private Sub RouteSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, RouteSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling ROUTE '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " SENSE "

    ''' <summary> Handles the supported function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnSupportedFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.SupportedFunctionModes <> VI.Scpi.SenseFunctionModes.None Then
            With Me._SenseFunctionComboBox
                .DataSource = Nothing
                .Items.Clear()
                .DataSource = GetType(VI.Scpi.SenseFunctionModes).ValueDescriptionPairs(subsystem.SupportedFunctionModes)
                .DisplayMember = "Value"
                .ValueMember = "Key"
                If .Items.Count > 0 Then
                    .SelectedItem = VI.Scpi.SenseFunctionModes.VoltageDC.ValueDescriptionPair()
                End If
            End With
        End If
    End Sub
    Private Sub OnFunctionModesChanged(ByVal value As SenseFunctionSubsystemBase)
        With Me._SenseRangeNumeric
            .Minimum = CDec(value.ValueRange.Min)
            .Maximum = CDec(value.ValueRange.Max)
            .DecimalPlaces = CInt(Math.Max(0, -Math.Log10(value.ValueRange.Min)))
        End With
        With Me._PowerLineCyclesNumeric
            .Minimum = CDec(value.PowerLineCyclesRange.Min)
            .Maximum = CDec(value.PowerLineCyclesRange.Max)
            .DecimalPlaces = CInt(Math.Max(0, -Math.Log10(value.PowerLineCyclesRange.Min)))
        End With
#If False Then
        ' done on the device level.
        With value
            .QueryRange()
            .QueryAutoRangeEnabled()
            .QueryAutoZeroEnabled()
            .QueryPowerLineCycles()
        End With
#End If
    End Sub

#If False Then
    Private ReadOnly Property SelectedSenseSubsystem() As SenseFunctionSubsystemBase
        Get
            Return Me.SelectSenseSubsystem(Me.selectedFunctionMode)
        End Get
    End Property

    Private Function SelectSenseSubsystem(ByVal value As VI.Scpi.SenseFunctionModes) As SenseFunctionSubsystemBase
        Dim result As SenseFunctionSubsystemBase = Me.Device.SenseVoltageSubsystem
        Select Case value
            Case VI.Scpi.SenseFunctionModes.CurrentDC
                result = Me.Device.SenseCurrentSubsystem
            Case VI.Scpi.SenseFunctionModes.VoltageDC
                result = Me.Device.SenseVoltageSubsystem
            Case VI.Scpi.SenseFunctionModes.Resistance
                result = Me.Device.SenseResistanceSubsystem
            Case VI.Scpi.SenseFunctionModes.FourWireResistance
                result = Me.Device.SenseFourWireResistanceSubsystem
            Case Else
        End Select
        Return result
    End Function

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="value"> The <see cref="TraceMessage">message</see> to display and
    ''' log. </param>
    Private Sub OnFunctionModesChanged(ByVal value As VI.Scpi.SenseFunctionModes)
        Me.onFunctionModesChanged(Me.Device.SelectSenseSubsystem(value))
    End Sub
#End If

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.FunctionMode.HasValue Then
            Me.Device.ParseMeasurementUnit(subsystem)
            Dim value As VI.Scpi.SenseFunctionModes = subsystem.FunctionMode.GetValueOrDefault(VI.Scpi.SenseFunctionModes.None)
            If value <> VI.Scpi.SenseFunctionModes.None Then
                Me._OpenLeadsDetectionCheckBox.Enabled = (value = Scpi.SenseFunctionModes.FourWireResistance) OrElse (value = Scpi.SenseFunctionModes.Temperature)
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.Readings.Reading.Unit.Symbol}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
                Me._SenseFunctionComboBox.SafeSelectItem(value, value.Description)
            End If
            Me.OnFunctionModesChanged(Me.Device.SelectSenseSubsystem(subsystem))
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
            Case NameOf(subsystem.MeasurementAvailable)
                Me.DisplayActiveReading()
            Case NameOf(subsystem.SupportedFunctionModes)
                Me.OnSupportedFunctionModesChanged(subsystem)
            Case NameOf(subsystem.FunctionMode)
                Me.OnFunctionModesChanged(subsystem)
                Me.DisplayActiveReading()
        End Select
    End Sub

    ''' <summary> Sense subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              $"{Me.Device.ResourceTitle} exception handling SENSE '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " SENSE VOLTAGE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseVoltageSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(subsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense voltage subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseVoltageSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseVoltageSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling SENSE VOLT '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " SENSE CURRENT "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseCurrentSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(subsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense Current subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseCurrentSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseCurrentSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling SENSE CURR '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " FOUR WIRE SENSE RESISTANCE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseFourWireResistanceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(subsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If

            Case NameOf(subsystem.OpenLeadDetectorEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.OpenLeadDetectorEnabled.HasValue Then
                    Me._OpenLeadsDetectionCheckBox.SafeCheckedSetter(subsystem.OpenLeadDetectorEnabled.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense Four Wire Resistance subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseFourWireResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseFourWireResistanceSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling 4-WIRE RESISTANCE SENSE '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " SENSE RESISTANCE "

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseResistanceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(subsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Sense Four Wire Resistance subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseResistanceSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling SENSE RESISTANCE '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Private Sub OnLastError(ByVal lastError As DeviceError)
        If lastError?.IsError Then
            Me._LastErrorTextBox.ForeColor = Drawing.Color.OrangeRed
        Else
            Me._LastErrorTextBox.ForeColor = Drawing.Color.Aquamarine
        End If
        Me._LastErrorTextBox.Text = lastError.CompoundErrorMessage
    End Sub

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnPropertyChanged(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(subsystem.DeviceErrors)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.LastDeviceError)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.ErrorAvailable)
                If subsystem.ErrorAvailable AndAlso Not subsystem.ReadingDeviceErrors Then
                    ' if no errors, this clears the error queue.
                    subsystem.QueryDeviceErrors()
                End If
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, StatusSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling STATUS '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Reads a service request status. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ReadServiceRequestStatus()
        Try
            Me.Device.StatusSubsystem.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling service request;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " SYSTEM "

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.ScpiRevision)
                Windows.Forms.Application.DoEvents()
        End Select
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, SystemSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling SYSTEM '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " TRACE "

    ''' <summary> Buffer size text box validating. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031: DoNotCatchGeneralExceptionTypes")>
    Private Sub _BufferSizeNumeric_Validating(sender As Object, e As CancelEventArgs) Handles _BufferSizeNumeric.Validating
        Try
            If Me.Device.IsDeviceOpen Then
                Dim value As Integer = CInt(Me._BufferSizeNumeric.Value)
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Setting {Me.Device.ResourceTitle} default buffer 1 to {value}")
                Me._BufferSizeNumeric.Value = Me._BufferSizeNumeric.NumericUpDownControl.Minimum
                Me.Device.TraceSubsystem.ApplyPointsCount(CInt(value))
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception updating {NameOf(TraceSubsystem)}.{NameOf(TraceSubsystem.PointsCount)};. {ex.ToFullBlownString}")

        End Try

    End Sub

    ''' <summary> Handle the Trace subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As TraceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.PointsCount)
                Me._BufferSizeNumeric.Value = subsystem.PointsCount.GetValueOrDefault(0)
                Me._BufferSizeNumeric.Invalidate()
            Case NameOf(subsystem.ActualPointCount)
                Me._BufferCountLabel.Text = CStr(subsystem.ActualPointCount.GetValueOrDefault(0))
                Me._BufferCountLabel.Invalidate()
            Case NameOf(subsystem.FirstPointNumber)
                Me._FirstPointNumberLabel.Text = CStr(subsystem.FirstPointNumber.GetValueOrDefault(0))
                Me._BufferCountLabel.Invalidate()
            Case NameOf(subsystem.LastPointNumber)
                Me._LastPointNumberLabel.Text = CStr(subsystem.LastPointNumber.GetValueOrDefault(0))
                Me._BufferCountLabel.Invalidate()
            Case NameOf(subsystem.BufferReadingsCount)
                If Me.BufferStreamingHandlerEnabled Then
                    ' must reconfigure = true to get the grid updated :(
                    If Me._BufferDataGridView.DataSource Is Nothing Then
                        subsystem.BufferReadings.DisplayReadings(Me._BufferDataGridView, True)
                        Windows.Forms.Application.DoEvents()
                        Me._BufferDataGridView.Invalidate()
                        Windows.Forms.Application.DoEvents()
                    Else
                        subsystem.BufferReadings.DisplayReadings(Me._BufferDataGridView, True)
                        'Me._BufferDataGridView.DataSource = subsystem.BufferReadings
                        'Me._BufferDataGridView.InvalidateRow(subsystem.BufferReadings.Count - 1)
                        Windows.Forms.Application.DoEvents()
                        Me._BufferDataGridView.Refresh()
                        Windows.Forms.Application.DoEvents()
                    End If
                    Me._ReadingToolStripStatusLabel.SafeTextSetter($"{subsystem.LastBufferReading.Reading} {Me.Device.SenseSubsystem.Readings.Reading.Unit.Symbol}")
                End If
        End Select
        Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary> Trace subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031: DoNotCatchGeneralExceptionTypes")>
    Private Sub TraceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                If Me.InvokeRequired Then
                    Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.TraceSubsystemPropertyChanged), New Object() {sender, e})
                Else
                    Me.OnSubsystemPropertyChanged(TryCast(sender, TraceSubsystem), e.PropertyName)
                End If
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling TRACE '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " TRIGGER "

    ''' <summary> Handle the Trigger subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As TriggerSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.TriggerCount)
                If subsystem.TriggerCount.HasValue Then
                    Me._TriggerCountNumeric.Value = subsystem.TriggerCount.Value
                End If
            Case NameOf(subsystem.ContinuousEnabled)
                Me._ContinuousTriggerEnabledMenuItem.CheckState = subsystem.ContinuousEnabled.ToCheckState
            Case NameOf(subsystem.TriggerSource)
                If subsystem.TriggerSource.HasValue AndAlso Me._TriggerSourceComboBox.ComboBox.Items.Count > 0 Then
                    Me._TriggerSourceComboBox.ComboBox.SelectedItem = subsystem.TriggerSource.Value.ValueNamePair
                End If
            Case NameOf(subsystem.SupportedTriggerSources)
                subsystem.ListSupportedTriggerSources(Me._TriggerSourceComboBox.ComboBox)
                If subsystem.TriggerSource.HasValue AndAlso Me._TriggerSourceComboBox.ComboBox.Items.Count > 0 Then
                    Me._TriggerSourceComboBox.ComboBox.SelectedItem = subsystem.TriggerSource.Value.ValueNamePair
                End If
            Case NameOf(subsystem.TriggerState)
                Me._TriggerStateLabel.Visible = subsystem.TriggerState.HasValue
                If subsystem.TriggerState.HasValue Then
                    Me._TriggerStateLabel.Text = subsystem.TriggerState.Value.ToString
                    If Me.TriggerPlanStateChangeHandlerEnabled Then
                        Me.HandleTriggerPlanStateChange(subsystem.TriggerState.Value)
                    End If
                    If Me.BufferStreamingHandlerEnabled AndAlso Not subsystem.IsTriggerStateActive AndAlso _StreamBufferMenuItem.Checked Then
                        Me._StreamBufferMenuItem.Checked = False
                    End If
                End If
                ' ?? this causes a cross thread exception. 
                ' Me._TriggerStateLabel.Invalidate()
        End Select
    End Sub

    ''' <summary> Trigger subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TriggerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                If Me.InvokeRequired Then
                    Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.TriggerSubsystemPropertyChanged), New Object() {sender, e})
                Else
                    Me.OnSubsystemPropertyChanged(TryCast(sender, TriggerSubsystem), e.PropertyName)
                End If
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling TRIGGER '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#End Region

#Region " DEVICE SETTINGS: FUNCTION MODE "

    ''' <summary>
    ''' Selects a new sense mode.
    ''' </summary>
    Friend Sub ApplyFunctionMode(ByVal value As VI.Scpi.SenseFunctionModes)
        If Me.IsDeviceOpen Then
            Me._Device.SenseSubsystem.ApplyFunctionMode(value)
        End If
    End Sub

    ''' <summary>
    ''' Gets or sets the selected function mode.
    ''' </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedFunctionMode() As VI.Scpi.SenseFunctionModes
        Get
            Return CType(CType(Me._SenseFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                  Of [Enum], String)).Key, VI.Scpi.SenseFunctionModes)
        End Get
    End Property

#End Region

#Region " CONTROL EVENT HANDLERS: RESET "

    ''' <summary> Clears interface. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearInterfaceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearInterfaceMenuItem.Click
        Dim activity As String = "clearing interface"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.InterfaceStopWatch.Restart()
                Me.Device.SystemSubsystem.ClearInterface()
                Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
                Me.InterfaceStopWatch.Stop()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears device (SDC). </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearDeviceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearDeviceMenuItem.Click
        Dim activity As String = "clearing selective device"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.InterfaceStopWatch.Restart()
                Me.Device.SystemSubsystem.ClearDevice()
                Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
                Me.InterfaceStopWatch.Stop()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears (CLS) the execution state menu item click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearExecutionStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearExecutionStateMenuItem.Click
        Dim activity As String = "clearing the execution state"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.InterfaceStopWatch.Restart()
                Me.Device.SystemSubsystem.ClearExecutionState()
                Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
                Me.InterfaceStopWatch.Stop()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    ''' <summary> Resets (RST) the known state menu item click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ResetKnownStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResetKnownStateMenuItem.Click
        Dim activity As String = "resetting known state"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                If Me.IsDeviceOpen Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.InterfaceStopWatch.Restart()
                    Me.Device.ResetKnownState()
                    Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
                    Me.InterfaceStopWatch.Stop()
                End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Initializes to known state menu item click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitKnownStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InitKnownStateMenuItem.Click
        Dim activity As String = "resetting known state"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                If Me.IsDeviceOpen Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.InterfaceStopWatch.Restart()
                    Me.Device.ResetKnownState()
                    activity = "initializing known state"
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.InitKnownState()
                    Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
                    Me.InterfaceStopWatch.Stop()
                End If
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadStatusByteMenuItem_Click(sender As Object, e As EventArgs) Handles _ReadStatusByteMenuItem.Click
        Dim activity As String = "reading status byte"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.InterfaceStopWatch.Restart()
                Me.ReadServiceRequestStatus()
                Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
                Me.InterfaceStopWatch.Stop()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#Region " TRACE LEVEL "

    ''' <summary> Enables the trace level controls. </summary>
    Private Sub EnableTraceLevelControls()

        TalkerControlBase.ListTraceEventLevels(Me._LogTraceLevelComboBox.ComboBox)
        AddHandler Me._LogTraceLevelComboBox.ComboBox.SelectedValueChanged, AddressOf Me._LogTraceLevelComboBox_SelectedValueChanged

        TalkerControlBase.ListTraceEventLevels(Me._DisplayTraceLevelComboBox.ComboBox)
        AddHandler Me._DisplayTraceLevelComboBox.ComboBox.SelectedValueChanged, AddressOf Me._DisplayTraceLevelComboBox_SelectedValueChanged

        TalkerControlBase.SelectItem(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel)
        TalkerControlBase.SelectItem(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel)

    End Sub

    ''' <summary>
    ''' Event handler. Called by _LogTraceLevelComboBox for selected value changed events.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _LogTraceLevelComboBox_SelectedValueChanged(sender As Object, e As EventArgs)
        Dim activity As String = "selecting log trace level on this instrument only"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ApplyTalkerTraceLevel(ListenerType.Logger,
                                            TalkerControlBase.SelectedValue(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Event handler. Called by _DisplayTraceLevelComboBox for selected value changed events.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DisplayTraceLevelComboBox_SelectedValueChanged(sender As Object, e As EventArgs)
        Dim activity As String = "selecting Display trace level on this instrument only"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ApplyTalkerTraceLevel(ListenerType.Display,
                                            TalkerControlBase.SelectedValue(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#End Region

#Region " CONTROL EVENT HANDLERS: SESSION "

    ''' <summary> Toggles session message tracing. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionTraceEnabledMenuItem_CheckedChanged(ByVal sender As Object, e As System.EventArgs) Handles _SessionServiceRequestHandlerEnabledMenuItem.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "toggling instrument message tracing"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        If menuItem IsNot Nothing Then
        End If
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SessionMessagesTraceEnabled = menuItem.Checked
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the session service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionServiceRequestHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SessionServiceRequestHandlerEnabledMenuItem.CheckStateChanged
        If Me.InitializingComponents Then Return
        Dim activity As String = "Toggle session service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.Session.ServiceRequestEventEnabled Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.Device.Session.EnableServiceRequest()
                    If Me._ServiceRequestEnableBitmaskNumeric.Value = 0 Then
                        Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
                    Else
                        Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableBitmaskNumeric.Value, ServiceRequests))
                    End If
                Else
                    Me.Device.Session.DisableServiceRequest()
                    Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the Device service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DeviceServiceRequestHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _DeviceServiceRequestHandlerEnabledMenuItem.CheckStateChanged
        If Me.InitializingComponents Then Return
        Dim activity As String = "Toggle device service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.DeviceServiceRequestHandlerAdded Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.AddServiceRequestEventHandler()
                Else
                    Me.RemoveServiceRequestEventHandler()
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: TERMINALS "

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadTerminalStateButton_Click(sender As Object, e As EventArgs) Handles _ReadTerminalStateButton.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "Reading terminals state"
        Dim button As ToolStripButton = CType(sender, ToolStripButton)
        Try
            If button IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.ErrorProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.RouteSubsystem.QueryTerminalsMode()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Displays terminal state. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ReadTerminalStateButton_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ReadTerminalStateButton.CheckStateChanged
        If Me.InitializingComponents Then Return
        Dim button As ToolStripButton = TryCast(sender, ToolStripButton)
        If button IsNot Nothing Then
            If button.CheckState = Windows.Forms.CheckState.Indeterminate Then
                button.Text = "R/F?"
                button.ToolTipText = "Unknown terminal state"
            Else
                button.Text = If(button.Checked, "Front", "Rear")
                button.ToolTipText = If(button.Checked, "Click to switch to Rear", "Click to switch to Front")
            End If
        End If
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: READING "

#Region " READ "

    ''' <summary> Gets the interface stop watch. </summary>
    ''' <value> The interface stop watch. </value>
    Private ReadOnly Property InterfaceStopWatch As Stopwatch

    ''' <summary> Selects a new reading to display.
    ''' </summary>
    Friend Function SelectReading(ByVal value As VI.ReadingTypes) As VI.ReadingTypes
        If Me.IsDeviceOpen AndAlso (value <> VI.ReadingTypes.None) AndAlso (value <> Me.SelectedReadingType) Then
            Me._ReadingComboBox.ComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.SelectedReadingType
    End Function

    ''' <summary> Gets the type of the selected reading. </summary>
    ''' <value> The type of the selected reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedReadingType() As VI.ReadingTypes
        Get
            Return CType(CType(Me._ReadingComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, VI.ReadingTypes)
        End Get
    End Property

    ''' <summary> Reading combo box selected value changed. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _ReadingComboBox.SelectedIndexChanged
        If Me.InitializingComponents Then Return
        Dim activity As String = "selecting a reading to display"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.MeasureSubsystem.Readings.ActiveReadingType = Me.SelectedReadingType
            Me.DisplayActiveReading()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _ReadButton for click events. Query the Device for a
    ''' reading. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadButton.Click
        Dim activity As String = "querying terminal mode"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.RouteSubsystem.QueryTerminalsMode()
            activity = "measuring"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.InterfaceStopWatch.Restart()
            Me.Device.MeasureSubsystem.Read()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " EVENTS "

    ''' <summary> Toggles auto trigger. </summary>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AutoInitiateMenuItem_Click(sender As Object, e As EventArgs)
        If Me.InitializingComponents Then Return
        Dim activity As String = "toggling re-trigger mode"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Gets or sets the trace readings. </summary>
    ''' <value> The trace readings. </value>
    Private ReadOnly Property TraceReadings As VI.BufferReadingCollection

#Region " STREAM BUFFER "

    ''' <summary> Gets or sets the buffer streaming handler enabled. </summary>
    ''' <value> The buffer streaming handler enabled. </value>
    Private Property BufferStreamingHandlerEnabled As Boolean

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _StreamBufferMenuItem_CheckStateChanged(sender As Object, e As EventArgs) Handles _StreamBufferMenuItem.CheckStateChanged
        If Me.InitializingComponents Then Return
        Dim activity As String = "start buffer streaming"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If _StreamBufferMenuItem.Checked Then
                Me.BufferStreamingHandlerEnabled = False
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.InterfaceStopWatch.Restart()
                Me._BufferDataGridView.DataSource = Nothing
                Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
                Me.Device.TriggerSubsystem.CaptureSyncContext(Me.CapturedSyncContext)
                Me.Device.TraceSubsystem.CaptureSyncContext(Me.CapturedSyncContext)
                Me.Device.TriggerSubsystem.Initiate()
                Windows.Forms.Application.DoEvents()
                Me.Device.TraceSubsystem.StreamBufferAsync(Me.CapturedSyncContext, Me.Device.TriggerSubsystem, TimeSpan.FromMilliseconds(5))
                Me.BufferStreamingHandlerEnabled = True
            Else
                Me.BufferStreamingHandlerEnabled = False
                activity = "Aborting trigger plan to stop buffer streaming"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.AbortTriggerPlan(sender)
                Me.Device.TriggerSubsystem.QueryTriggerState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub


#End Region

#Region " INITIATE AND WAIT "

    Private Enum TriggerPlanState
        None
        Started
        Completed
    End Enum

    Private Property TriggerPlanStateChangeHandlerEnabled As Boolean

    Private Property LocalTriggerPlanState As TriggerPlanState

    Private Sub HandleTriggerPlanStateChange(ByVal triggerState As VI.TriggerState)
        If triggerState = TriggerState.Running OrElse triggerState = TriggerState.Waiting Then
            LocalTriggerPlanState = TriggerPlanState.Started
        ElseIf triggerState = TriggerState.Idle AndAlso LocalTriggerPlanState = TriggerPlanState.Started Then
            LocalTriggerPlanState = TriggerPlanState.Completed
            Me.TryReadBuffer()
            Me.TryDisplayBuffer()
            If Me._RepeatMenuItem.Checked Then
                Me.InitiateMonitorTriggerPlan(True)
            End If
        Else
            LocalTriggerPlanState = TriggerPlanState.None
        End If
        Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TryReadBuffer()
        Dim activity As String = "reading"
        Try
            Me.ReadBuffer()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
        End Try
    End Sub

    Private Sub ReadBuffer()
        Dim activity As String = "fetching buffer count"
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
        ' this assume buffer is cleared upon each new cycle
        Dim newBufferCount As Integer = Me.Device.TraceSubsystem.QueryActualPointCount.GetValueOrDefault(0)
        activity = $"buffer count {newBufferCount}"
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")

        If newBufferCount > 0 Then
            activity = "fetching buffered readings"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Dim values As IEnumerable(Of BufferReading) = Me.Device.TraceSubsystem.QueryBufferReadings()
            If Me.TraceReadings Is Nothing Then Me._TraceReadings = New VI.BufferReadingCollection
            Me.TraceReadings.Add(values)
        End If
        Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
        Me.InterfaceStopWatch.Stop()
        Windows.Forms.Application.DoEvents()
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TryDisplayBuffer()
        Dim activity As String = "displaying"
        Try
            Me.DisplayBuffer()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
        End Try
    End Sub

    ''' <summary> Displays a buffer. </summary>
    Private Sub DisplayBuffer(ByVal readings As VI.BufferReadingCollection)
        Dim activity As String = "updating the display"
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
        readings.DisplayReadings(Me._BufferDataGridView, True)
        Windows.Forms.Application.DoEvents()
        Me._BufferDataGridView.Invalidate()
    End Sub

    Private Sub DisplayBuffer()
        Dim activity As String = "updating the display"
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
        Me.TraceReadings.DisplayReadings(Me._BufferDataGridView, True)
        Windows.Forms.Application.DoEvents()
        Me._BufferDataGridView.Invalidate()
    End Sub

    ''' <summary> Initiate monitor trigger plan. </summary>
    ''' <param name="stateChangeHandlingEnabled"> True to enable, false to disable the state change
    '''                                           handling. </param>
    Private Sub InitiateMonitorTriggerPlan(ByVal stateChangeHandlingEnabled As Boolean)
        Dim activity As String = "Initiating trigger plan and monitor"
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
        Me.InterfaceStopWatch.Restart()
        Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
        Me.Device.TriggerSubsystem.Initiate()
        Windows.Forms.Application.DoEvents()
        Me.TriggerPlanStateChangeHandlerEnabled = stateChangeHandlingEnabled
        Me.Device.TriggerSubsystem.CaptureSyncContext(Me.CapturedSyncContext)
        Me.Device.TriggerSubsystem.AsyncMonitorTriggerState(Me.CapturedSyncContext, TimeSpan.FromMilliseconds(5))
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TryRestartTriggerPlan()
        Dim activity As String = "Initiating trigger plan and monitor"
        Try
            Me.InitiateMonitorTriggerPlan(True)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MonitorActiveTriggerPlanMenuItem_Click(sender As Object, e As EventArgs) Handles _MonitorActiveTriggerPlanMenuItem.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "start monitoring trigger plan"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.TriggerPlanStateChangeHandlerEnabled = False
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.InterfaceStopWatch.Restart()
            Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
            Me.Device.TriggerSubsystem.CaptureSyncContext(Me.CapturedSyncContext)
            Me.Device.TriggerSubsystem.AsyncMonitorTriggerState(Me.CapturedSyncContext, TimeSpan.FromMilliseconds(5))
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitMonitorReadRepeatMenuItem_Click(sender As Object, e As EventArgs) Handles _InitMonitorReadRepeatMenuItem.Click
        Dim activity As String = "Initiating trigger plan and monitor"
        Try
            Me.InitiateMonitorTriggerPlan(True)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
        End Try
    End Sub

#End Region

#Region " TRIGGER PLAN EVENT HANDLING "

    ''' <summary> Handles the measurement completed request. </summary>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub HandleMeasurementCompletedRequest(sender As Object, e As EventArgs)
        Dim activity As String = "handling service request event"
        Try
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} SRQ: {Me.Device.StatusSubsystem.ServiceRequestStatus:X};. ")
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
#If False Then
            ' Measurement bit does not turn on -- kludging for now.
            If Me.Device.StatusSubsystem.MeasurementAvailable Then
            Else
                activity = "measurement not available"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            End If
#End If
            activity = "kludge: reading buffer count"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")

            ' this assume buffer is cleared upon each new cycle
            Dim newBufferCount As Integer = Me.Device.TraceSubsystem.QueryActualPointCount.GetValueOrDefault(0)

            If newBufferCount > 0 Then

                activity = "kludge: buffer has data..."
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")

                activity = "handling measurement available"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")

                If False Then
                    activity = "fetching a single readings"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.MeasureSubsystem.Fetch()
                Else
                    activity = "fetching buffered readings"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Dim values As IEnumerable(Of BufferReading) = Me.Device.TraceSubsystem.QueryBufferReadings()
                    If Me.TraceReadings Is Nothing Then Me._TraceReadings = New VI.BufferReadingCollection
                    For Each v As BufferReading In values : Me.TraceReadings.Add(v) : Next

                    activity = "updating the display"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    If Me.TraceReadings.Count = values.Count Then
                        Me.TraceReadings.DisplayReadings(Me._BufferDataGridView, False)
                    Else
                        ' TO_DO: See if observable collection will work.
                        ' Me._BufferDataGridView.Invalidate()
                        Me.TraceReadings.DisplayReadings(Me._BufferDataGridView, False)
                    End If
                    Me._BufferDataGridView.Invalidate()
                    Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
                    Me.InterfaceStopWatch.Stop()
                End If
                If Me._RepeatMenuItem.Checked Then
                    activity = "initiating next measurement(s)"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.InterfaceStopWatch.Restart()
                    Me.Device.TraceSubsystem.ClearBuffer() ' ?@3 removed 7/6/17
                    Me.Device.TriggerSubsystem.Initiate()
                End If
            Else
                activity = "trigger plan started; buffer empty"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(Me._ReadBufferButton, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Gets the measurement complete handler added. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <value> The measurement complete handler added. </value>
    Private Property MeasurementCompleteHandlerAdded As Boolean

    ''' <summary> Adds measurement complete event handler. </summary>
    Private Sub AddMeasurementCompleteEventHandler()

        Dim activity As String = ""
        If Not Me.MeasurementCompleteHandlerAdded Then

            ' clear execution state before enabling events
            activity = "Clearing execution state"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.ClearExecutionState()

            activity = "Enabling session service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.Session.EnableServiceRequest()

            activity = "Adding device service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.AddServiceRequestEventHandler()

            activity = "Turning on measurement events"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.StatusSubsystem.ApplyQuestionableEventEnableBitmask(MeasurementEvents.All)
            ' 
            ' if handling buffer full, use the 4917 event to detect buffer full. 

            activity = "Turning on status service request"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            ' Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.MeasurementEvent)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.ServiceRequests.All And Not VI.ServiceRequests.MessageAvailable)

            activity = "Adding re-triggering event handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            AddHandler Me.Device.ServiceRequested, AddressOf HandleMeasurementCompletedRequest
            Me.MeasurementCompleteHandlerAdded = True
        End If
    End Sub

    ''' <summary> Removes the measurement complete event handler. </summary>
    Private Sub RemoveMeasurementCompleteEventHandler()

        Dim activity As String = ""
        If Me.MeasurementCompleteHandlerAdded Then

            activity = "Disabling session service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.Session.DisableServiceRequest()

            activity = "Removing device service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.RemoveServiceRequestEventHandler()

            activity = "Turning off measurement events"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.StatusSubsystem.ApplyQuestionableEventEnableBitmask(MeasurementEvents.None)

            activity = "Turning off status service request"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)

            activity = "Removing re-triggering event handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            RemoveHandler Me.Device.ServiceRequested, AddressOf HandleMeasurementCompletedRequest

            Me.MeasurementCompleteHandlerAdded = False

        End If
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _HandleMeasurementEventMenuItem_CheckStateChanged(sender As Object, e As EventArgs)

        If Me.InitializingComponents Then Return
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        If menuItem Is Nothing Then Return
        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            activity = "Aborting trigger plan"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.AbortTriggerPlan(sender)

            If menuItem.Checked Then

                activity = "Adding measurement completion handler"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.AddMeasurementCompleteEventHandler()

            Else

                activity = "Removing measurement completion handler"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.RemoveMeasurementCompleteEventHandler()

            End If

        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

#Region " BUFFER HANDLER "

    ''' <summary> Handles the buffer full request. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub HandleBufferFullRequest(sender As Object, e As EventArgs)
        Dim activity As String = "handling service request event"
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} SRQ: {Me.Device.StatusSubsystem.ServiceRequestStatus:X2};. ")
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.Device.StatusSubsystem.OperationCompleted Then

                activity = "handling operation completed"
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")

                ' TO_DO: See if can only do a set condition and not read this.
                Dim condition As Integer = Me.Device.StatusSubsystem.QueryOperationEventCondition().GetValueOrDefault(0)
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} OPER: {condition:X2};. ")

                ' If Bit 0 Is set then the buffer is full
                If (condition And (1 << Me.BufferFullOperationConditionBitNumber)) <> 0 Then
                    activity = "handling buffer full"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")

                    activity = "fetching buffered readings"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Dim values As IEnumerable(Of BufferReading) = Me.Device.TraceSubsystem.QueryBufferReadings()
                    If Me.TraceReadings Is Nothing Then Me._TraceReadings = New VI.BufferReadingCollection
                    For Each v As BufferReading In values : Me.TraceReadings.Add(v) : Next

                    activity = "updating the display"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    If Me.TraceReadings.Count = values.Count Then
                        Me.TraceReadings.DisplayReadings(Me._BufferDataGridView, True)
                    Else
                        Me._BufferDataGridView.Invalidate()
                    End If
                    Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
                    Me.InterfaceStopWatch.Stop()
                    If Me._RepeatMenuItem.Checked Then
                        activity = "initiating next measurement(s)"
                        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                        Me.InterfaceStopWatch.Restart()
                        Me.Device.TraceSubsystem.ClearBuffer() ' ?@# removed 7/6/17
                        Me.Device.TriggerSubsystem.Initiate()
                    End If
                Else
                    activity = "handling buffer clear: NOP"
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                End If
            Else
                activity = "operation not completed"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(Me._ReadBufferButton, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Gets the Buffer Full handler added. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <value> The Buffer Full handler added. </value>
    Private Property BufferFullHandlerAdded As Boolean

    ''' <summary> Gets the buffer full operation condition bit number. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <value> The buffer full operation condition bit number. </value>
    Private Property BufferFullOperationConditionBitNumber As Integer

    ''' <summary> Adds Buffer Full event handler. </summary>
    Private Sub AddBufferFullEventHandler()

        Dim activity As String = ""
        If Not Me.BufferFullHandlerAdded Then

            ' clear execution state before enabling events
            activity = "Clearing execution state"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.ClearExecutionState()

            activity = "Enabling session service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.Session.EnableServiceRequest()

            activity = "Adding device service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.AddServiceRequestEventHandler()

            activity = "Turning on Buffer events"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.BufferFullOperationConditionBitNumber = 0
            Me.Device.StatusSubsystem.ApplyOperationEventMap(Me.BufferFullOperationConditionBitNumber, Me.Device.StatusSubsystem.BufferFullEventNumber, Me.Device.StatusSubsystem.BufferEmptyEventNumber)
            Me.Device.StatusSubsystem.ApplyOperationEventEnableBitmask(1 << Me.BufferFullOperationConditionBitNumber)

            activity = "Turning on status service request"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            ' Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.BufferEvent)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.ServiceRequests.All And Not VI.ServiceRequests.MessageAvailable)

            activity = "Adding re-triggering event handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            AddHandler Me.Device.ServiceRequested, AddressOf HandleBufferFullRequest
            Me.BufferFullHandlerAdded = True
        End If
    End Sub

    ''' <summary> Removes the Buffer Full event handler. </summary>
    Private Sub RemoveBufferFullEventHandler()

        Dim activity As String = ""
        If Me.BufferFullHandlerAdded Then

            activity = "Disabling session service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.Session.DisableServiceRequest()

            activity = "Removing device service request handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.RemoveServiceRequestEventHandler()

            activity = "Turning off Buffer events"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.BufferFullOperationConditionBitNumber = 0
            Me.Device.StatusSubsystem.ApplyOperationEventMap(Me.BufferFullOperationConditionBitNumber, 0, 0)
            Me.Device.StatusSubsystem.ApplyOperationEventEnableBitmask(0)

            activity = "Turning off status service request"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)

            activity = "Removing re-triggering event handler"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            RemoveHandler Me.Device.ServiceRequested, AddressOf HandleBufferFullRequest

            Me.BufferFullHandlerAdded = False

        End If
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _HandleBufferEventMenuItem_CheckStateChanged(sender As Object, e As EventArgs)

        If Me.InitializingComponents Then Return
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        If menuItem Is Nothing Then Return
        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            activity = "Aborting trigger plan"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.AbortTriggerPlan(sender)

            If menuItem.Checked Then

                activity = "Adding Buffer completion handler"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.AddBufferFullEventHandler()

            Else

                activity = "Removing Buffer completion handler"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.RemoveBufferFullEventHandler()

            End If

        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

    ''' <summary> Aborts trigger plan. </summary>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub AbortTriggerPlan(ByVal sender As System.Object)

        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()

            activity = "Aborting trigger plan"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.TriggerSubsystem.Abort()

        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Starts trigger plan. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StartTriggerPlan(ByVal sender As System.Object)

        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()

            activity = "clearing buffer and display"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me._TraceReadings = New VI.BufferReadingCollection
            Me._TraceReadings.DisplayReadings(Me._BufferDataGridView, True)

            Me.Device.TraceSubsystem.ClearBuffer()

            activity = "initiating trigger plan"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me._InterfaceStopWatch.Restart()
            Me.Device.TriggerSubsystem.Initiate()

        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by the Initiate Button for click events. Initiates a reading for
    ''' retrieval by way of the service request event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AbortStartTriggerPlanMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _AbortStartTriggerPlanMenuItem.Click

        If Me.InitializingComponents Then Return
        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            activity = "Aborting trigger plan"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.AbortTriggerPlan(sender)

            activity = "Starting trigger plan"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.StartTriggerPlan(sender)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitiateTriggerPlanMenuItem_Click(sender As Object, e As EventArgs) Handles _InitiateTriggerPlanMenuItem.Click

        If Me.InitializingComponents Then Return
        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            activity = "initiating trigger plan"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me._InterfaceStopWatch.Restart()
            Me.Device.TriggerSubsystem.Initiate()
            Me.Device.TriggerSubsystem.QueryTriggerState()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AbortButton_Click(sender As Object, e As EventArgs) Handles _AbortButton.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "aborting trigger plan"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.AbortTriggerPlan(sender)
            Me.Device.TriggerSubsystem.QueryTriggerState()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " BUFFER "

    ''' <summary>
    ''' Handles the DataError event of the _dataGridView control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="DataGridViewDataErrorEventArgs"/> instance containing the event data.</param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _BufferDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles _BufferDataGridView.DataError
        Try
            ' prevent error reporting when adding a new row or editing a cell
            Dim grid As DataGridView = TryCast(sender, DataGridView)
            If grid IsNot Nothing Then
                If grid.CurrentRow IsNot Nothing AndAlso grid.CurrentRow.IsNewRow Then Return
                If grid.IsCurrentCellInEditMode Then Return
                If grid.IsCurrentRowDirty Then Return
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   $"{e.Exception.Message} occurred editing row {e.RowIndex} column {e.ColumnIndex};. {e.Exception.ToFullBlownString}")
                Me.ErrorProvider.Annunciate(grid, $"{e.Exception.Message} occurred editing table")
            End If
        Catch
        End Try
    End Sub

    ''' <summary> Reads buffer button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadBufferButton_Click(sender As Object, e As EventArgs) Handles _ReadBufferButton.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "reading buffer"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.InterfaceStopWatch.Restart()
            Dim br As New VI.BufferReadingCollection From {
                Me.Device.TraceSubsystem.QueryBufferReadings
            }
            br.DisplayReadings(Me._BufferDataGridView, True)
            Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
            Me.InterfaceStopWatch.Stop()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears the buffer display button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearBufferDisplayButton_Click(sender As Object, e As EventArgs) Handles _ClearBufferDisplayButton.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "clearing buffer display"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Dim br As New VI.BufferReadingCollection
            br.DisplayReadings(Me._BufferDataGridView, True)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#End Region

#Region " CONTROL EVENT HANDLERS: SENSE "

    ''' <summary> Applies the function mode button click. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyFunctionModeButton_Click(sender As Object, e As EventArgs) Handles _ApplyFunctionModeButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.ApplyFunctionMode(Me.SelectedFunctionMode)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{ex.Message} occurred initiating a measurement;. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Sense range setter. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub SenseRangeSetter(ByVal value As Double)
        If value <= Me._SenseRangeNumeric.Maximum AndAlso value >= Me._SenseRangeNumeric.Minimum Then Me._SenseRangeNumeric.Value = CDec(value)
    End Sub

    ''' <summary>
    ''' Applies the selected measurements settings.
    ''' </summary>
    Private Sub ApplySenseSettings()

        With Me.Device.SelectedSenseSubsystem

            If Me._OpenLeadsDetectionCheckBox.Enabled AndAlso Not Nullable.Equals(.OpenLeadDetectorEnabled, Me._OpenLeadsDetectionCheckBox.Checked) Then
                .ApplyOpenLeadDetectorEnabled(Me._OpenLeadsDetectionCheckBox.Checked)
            End If

            If Not Nullable.Equals(.PowerLineCycles, Me._PowerLineCyclesNumeric.Value) Then
                .ApplyPowerLineCycles(Me._PowerLineCyclesNumeric.Value)
            End If

            If Not Nullable.Equals(.AutoRangeEnabled, Me._SenseAutoRangeToggle.Checked) Then
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)
            End If

            If .AutoRangeEnabled Then
                .QueryRange()
            ElseIf Not Nullable.Equals(.Range, Me._SenseRangeNumeric.Value) Then
                .ApplyRange(Me._SenseRangeNumeric.Value)
            End If

        End With

    End Sub

    ''' <summary> Event handler. Called by ApplySenseSettingsButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySenseSettingsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ApplySenseSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.ApplySenseSettings()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{ex.Message} occurred initiating a measurement;. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: TRIGGER "

    Private Sub _FailBitPatternNumericButton_CheckStateChanged(sender As Object, e As EventArgs) Handles _FailBitPatternNumericButton.CheckStateChanged
        Me._FailLimit1BitPatternNumeric.NumericUpDownControl.Hexadecimal = Me._FailBitPatternNumericButton.Checked
        Me._FailBitPatternNumericButton.Text = $"Fail {If(_FailBitPatternNumericButton.Checked, "0x", "0d")}"
    End Sub

    Private Sub _PassBitToolStripLabel_CheckStateChanged(sender As Object, e As EventArgs) Handles _PassBitPatternNumericButton.CheckStateChanged
        Me._PassBitPatternNumeric.NumericUpDownControl.Hexadecimal = Me._PassBitPatternNumericButton.Checked
        Me._PassBitPatternNumericButton.Text = $"Pass {If(_FailBitPatternNumericButton.Checked, "0x", "0d")}"
    End Sub

    Private Sub _OpenLeadsBitToolStripLabel_CheckStateChanged(sender As Object, e As EventArgs) Handles _OpenLeadsBitPatternNumericButton.CheckStateChanged
        Me._OpenLeadsBitPatternNumeric.NumericUpDownControl.Hexadecimal = Me._OpenLeadsBitPatternNumericButton.Checked
        Me._OpenLeadsBitPatternNumeric.Text = $"Open {If(_FailBitPatternNumericButton.Checked, "0x", "0d")}"
    End Sub

    Private Sub _Limit1DecimalsNumeric_ValueChanged(sender As Object, e As EventArgs) Handles _Limit1DecimalsNumeric.ValueChanged
        If Me.InitializingComponents Then Return
        Me._LowerLimit1Numeric.NumericUpDownControl.DecimalPlaces = CInt(Me._Limit1DecimalsNumeric.Value)
        Me._UpperLimit1Numeric.NumericUpDownControl.DecimalPlaces = CInt(Me._Limit1DecimalsNumeric.Value)
    End Sub

    ''' <summary> Gets the selected trigger source. </summary>
    ''' <value> The selected trigger source. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedTriggerSource() As VI.TriggerSources
        Get
            Return CType(CType(Me._TriggerSourceComboBox.ComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, VI.TriggerSources)
        End Get
    End Property

    Private Sub PrepareGradeBinningModel()

        Me.ApplyFunctionMode(Scpi.SenseFunctionModes.FourWireResistance)

        If Me._LowerLimit1Numeric.Value <= 100 Then
            With Device.SenseFourWireResistanceSubsystem
                .ApplyAverageEnabled(True)
                .ApplyAverageCount(10)
                .ApplyAverageFilterType(AverageFilterType.Repeat)
                .ApplyAveragePercentWindow(0.1)
            End With
        Else
            With Device.SenseFourWireResistanceSubsystem
                .ApplyAverageEnabled(False)
            End With
        End If

        With Device.Calculate2FourWireResistanceSubsystem
            .ApplyLimit1AutoClear(True)
            .ApplyLimit1Enabled(True)
            .ApplyLimit1LowerLevel(Me._LowerLimit1Numeric.Value)
            .ApplyLimit1UpperLevel(Me._UpperLimit1Numeric.Value)
        End With

        ' set limits for open circuit to 10 times the range limit
        With Device.Calculate2FourWireResistanceSubsystem
            .ApplyLimit1AutoClear(True)
            .ApplyLimit2Enabled(True)
            .ApplyLimit2LowerLevel(-10 * Me._UpperLimit1Numeric.Value)
            .ApplyLimit2UpperLevel(10 * Me._UpperLimit1Numeric.Value)
        End With

        ' enable open detection
        With Device.SenseFourWireResistanceSubsystem
            .ApplyOpenLeadDetectorEnabled(True)
        End With

        Dim count As Integer = CInt(Me._TriggerCountNumeric.Value)
        ' the buffer must have at least 10 data points
        Me.Device.TraceSubsystem.ApplyPointsCount(Math.Max(10, count))

        ' clear the buffer 
        Me.Device.TraceSubsystem.ClearBuffer()

    End Sub

    ''' <summary> Loads grade bin trigger model button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _LoadGradeBinTriggerModelButton_Click(sender As Object, e As EventArgs) Handles _LoadGradeBinTriggerModelMenuItem.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "loading grade binning trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.PrepareGradeBinningModel()
                Me.Device.TriggerSubsystem.ApplyGradeBinning(CInt(Me._TriggerCountNumeric.Value),
                                                             TimeSpan.FromSeconds(Me._StartTriggerDelayNumeric.Value),
                                                             CInt(Me._FailLimit1BitPatternNumeric.Value),
                                                             CInt(Me._PassBitPatternNumeric.Value), CInt(Me._OpenLeadsBitPatternNumeric.Value),
                                                             Me.SelectedTriggerSource)
                Me.Device.TriggerSubsystem.QueryTriggerState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Simple loop load run button click. </summary>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _LoadSimpleLoopModelButton_Click(sender As Object, e As EventArgs)
        If Me.InitializingComponents Then Return
        Dim activity As String = "loading simple loop trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            If Me.IsDeviceOpen Then
                Dim count As Integer = CInt(Me._TriggerCountNumeric.Value)
                Dim startDelay As TimeSpan = TimeSpan.FromSeconds(Me._StartTriggerDelayNumeric.Value)
                Me.Device.TriggerSubsystem.LoadSimpleLoop(count, startDelay)
                Me.Device.TriggerSubsystem.QueryTriggerState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _RunSimpleLoopTriggerModelButton_Click(sender As Object, e As EventArgs) Handles _RunSimpleLoopMenuItem.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "Initiating simple loop trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Dim br As New VI.BufferReadingCollection : br.DisplayReadings(Me._BufferDataGridView, True)
            Me.Device.TraceSubsystem.ClearBuffer()
            Me.InterfaceStopWatch.Restart()
            Me.Device.TriggerSubsystem.Initiate()
            Me.Device.StatusSubsystem.Wait()
            br.Add(Me.Device.TraceSubsystem.QueryBufferReadings)
            br.DisplayReadings(Me._BufferDataGridView, False)
            Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
            Me.InterfaceStopWatch.Stop()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearTriggerModelMenuItem_Click(sender As Object, e As EventArgs) Handles _ClearTriggerModelMenuItem.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "clearing the trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.InterfaceStopWatch.Restart()
            Me.Device.TriggerSubsystem.Abort()
            Me.Device.TriggerSubsystem.ClearTriggerModel()
            Me.Device.StatusSubsystem.Wait()
            Me._TbdToolStripStatusLabel.SafeTextSetter(Me.InterfaceStopWatch.Elapsed.ToString("s\.ffff"))
            Me.InterfaceStopWatch.Stop()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadTriggerStateMenuItem_Click(sender As Object, e As EventArgs) Handles _ReadTriggerStateMenuItem.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "reading trigger state"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            ' Me.Device.StatusSubsystem.Wait()
            Me.Device.TriggerSubsystem.QueryTriggerState()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MeterCompleterFirstGradingBinningMenuItem_Click(sender As Object, e As EventArgs) Handles _MeterCompleterFirstGradingBinningMenuItem.Click
        If Me.InitializingComponents Then Return
        Dim activity As String = "loading meter complete first grade binning trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.PrepareGradeBinningModel()
                Me.Device.TriggerSubsystem.ApplyMeterCompleteFirstGradeBinning(CInt(Me._TriggerCountNumeric.Value),
                                                             TimeSpan.FromSeconds(Me._StartTriggerDelayNumeric.Value),
                                                             CInt(Me._FailLimit1BitPatternNumeric.Value),
                                                             CInt(Me._PassBitPatternNumeric.Value), CInt(Me._OpenLeadsBitPatternNumeric.Value),
                                                             Me.SelectedTriggerSource)
                Me.Device.TriggerSubsystem.QueryTriggerState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


#End Region

#Region " READ AND WRITE "

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub OnPropertyChanged(ByVal sender As Instrument.SimpleReadWriteControl, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(sender.ReceivedMessage)
                Case NameOf(sender.SentMessage)
                Case NameOf(sender.StatusMessage)
                    Me.StatusLabel.Text = sender.StatusMessage
                Case NameOf(sender.ServiceRequestValue)
                    Me.StatusRegisterLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
                Case NameOf(sender.ElapsedTime)
            End Select
        End If
    End Sub

    ''' <summary> Event handler. Called by <see crefname="_SimpleReadWriteControl"/> for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleReadWriteControl_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Handles _SimpleReadWriteControl.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._SimpleReadWriteControl_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, Instrument.SimpleReadWriteControl), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.Device.ResourceTitle} exception handling Read/Write '{e.PropertyName}' change event;. {ex.ToFullBlownString}")
        End Try
    End Sub
#End Region

#Region " TALKER "

    ''' <summary> Identifies talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

    ''' <summary> Assigns talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AssignTalker(talker As ITraceMessageTalker)
        Me._SimpleReadWriteControl.AssignTalker(talker)
        MyBase.AssignTalker(talker)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        Me._SimpleReadWriteControl.ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        ' MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

    Public Overrides Sub ApplyListenerTraceLevels(ByVal talker as ITraceMessageTalker)
        Me._SimpleReadWriteControl.ApplyListenerTraceLevels(talker)
        ' this should apply only to the listeners associated with this form
        ' MyBase.ApplyListenerTraceLevels(talker)
    End Sub
	
    Public Overrides Sub ApplyTalkerTraceLevels(ByVal talker as ITraceMessageTalker)
        Me._SimpleReadWriteControl.ApplyTalkerTraceLevels(talker)
        MyBase.ApplyTalkerTraceLevels(talker)
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
    ''' <summary> Adds listeners such as current level trace message box and log. </summary>
    Protected Overrides Sub AddListeners()
        MyBase.AddListeners()
        Me._SimpleReadWriteControl.AssignTalker(Me.Talker)
    End Sub

    ''' <summary> Adds listeners such as top level trace message box and log. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of ITraceMessageListener))
        MyBase.AddListeners(listeners)
        Me._SimpleReadWriteControl.AddListeners(listeners)
    End Sub

    ''' <summary> Adds the listeners. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AddListeners(ByVal talker As ITraceMessageTalker)
        MyBase.AddListeners(talker)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the log listener. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="log"> The log. </param>
    Public Overrides Sub AddListeners(ByVal log As MyLog)
        If log Is Nothing Then Throw New ArgumentNullException(NameOf(log))
        MyBase.AddListeners(log)
        My.MyLibrary.Identify(Me.Talker)
    End Sub
#End If
#End Region