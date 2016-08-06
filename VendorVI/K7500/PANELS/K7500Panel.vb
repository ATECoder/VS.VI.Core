Imports System.ComponentModel
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

    Private _InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(New Device)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="device"> The device. </param>
    Protected Sub New(ByVal device As Device)
        MyBase.New(device)
        Me._InitializingComponents = True
        Me.InitializeComponent()
        Me._InitializingComponents = False
        Me._AssignDevice(device)
        With Me._PassBitPatternNumeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 63
        End With
        With Me._FailLimit1BitPatternNumeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 63
        End With
        With Me._LowLimit1Numeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 5000000D
            .DecimalPlaces = 3
            .Value = 0
        End With
        With Me._HighLimit1Numeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 5000000D
            .DecimalPlaces = 3
            .Value = 0
        End With
        With Me._BinningTriggerCountNumeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 5000000D
            .DecimalPlaces = 0
            .Value = 100
        End With
        With Me._SimpleLoopCountNumeric.NumericUpDownControl
            .Minimum = 0
            .Maximum = 1000D
            .DecimalPlaces = 0
            .Value = 10
        End With
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the isr.VI.Instrument.ResourcePanelBase and
    ''' optionally releases the managed resources.
    ''' </summary>
    ''' <remarks> David, 12/22/2015. </remarks>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.DebuggerNonUserCode()>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' the device gets closed and disposed (if panel is device owner) in the base class
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " FORM EVENTS "

    ''' <summary> Handles the <see cref="E:System.Windows.Forms.UserControl.Load" /> event. </summary>
    ''' <remarks> David, 1/4/2016. </remarks>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Try
            Me.TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
        Finally
            MyBase.OnLoad(e)
        End Try
    End Sub

#End Region

#Region " DEVICE "

    ''' <summary> Assigns a device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignDevice(ByVal value As Device)
        Me._Device = value
        Me.AddListeners()
        Me.OnDeviceOpenChanged(value)
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device)
        Me.IsDeviceOwner = False
        MyBase.AssignDevice(value)
        Me._AssignDevice(value)
    End Sub

    ''' <summary> Releases the device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    Protected Overrides Sub ReleaseDevice()
        MyBase.ReleaseDevice()
    End Sub

    ''' <summary> Gets a reference to the Keithley 7500 Device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overloads ReadOnly Property Device() As Device

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
    ''' <remarks> David, 3/3/2016. </remarks>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As DeviceBase)
        Dim isOpen As Boolean = CType(device?.IsDeviceOpen, Boolean?).GetValueOrDefault(False)
        If isOpen Then
            Me._SimpleReadWriteControl.Connect(device?.Session)
        Else
            Me._SimpleReadWriteControl.Disconnect()
        End If
        For Each t As Windows.Forms.TabPage In Me._Tabs.TabPages
            If t IsNot Me._MessagesTabPage Then
                For Each c As Windows.Forms.Control In t.Controls : Me.RecursivelyEnable(c, isOpen) : Next
            End If
        Next
    End Sub

    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnDevicePropertyChanged(ByVal device As DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnDevicePropertyChanged(device, propertyName)
        Select Case propertyName
            Case NameOf(device.IsServiceRequestEventEnabled)
                Me._HandleServiceRequestsMenuItem.Checked = device.IsServiceRequestEventEnabled
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
        'AddHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        MyBase.DeviceOpened(sender, e)
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <remarks> David, 1/14/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overrides Sub OnTitleChanged(ByVal value As String)
        Me._TitleLabel.Text = value
        Me._TitleLabel.Visible = Not String.IsNullOrWhiteSpace(value)
        MyBase.OnTitleChanged(Title)
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
            'RemoveHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
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
                If Me.Device IsNot Nothing AndAlso subsystem.Elements <> ReadingTypes.None Then
                    Dim selectedIndex As Integer = Me._ReadingComboBox.SelectedIndex
                    With Me._ReadingComboBox
                        .DataSource = Nothing
                        .Items.Clear()
                        .DataSource = GetType(VI.ReadingTypes).ValueDescriptionPairs(subsystem.Elements And Not ReadingTypes.Units)
                        .DisplayMember = "Value"
                        .ValueMember = "Key"
                        If .Items.Count > 0 Then
                            .SelectedIndex = Math.Max(selectedIndex, 0)
                        End If
                    End With
                End If
        End Select
    End Sub

    ''' <summary> Format subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub FormatSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, FormatSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Format Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " MEASURE "

    ''' <summary> Displays the active reading caption and status. </summary>
    ''' <remarks> David, 3/18/2016. </remarks>
    Private Sub DisplayActiveReading()
        Const clear As String = "    "
        Dim caption As String = clear
        Dim failureCaption As String = clear
        Dim failureToolTip As String = clear
        Dim tbdCaption As String = clear
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
                    Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Instruments parsed reading elements.")
                Else
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, failureToolTip)
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
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                       "Measure message: {0}.", subsystem.LastReading.InsertCommonEscapeSequences)
                Case NameOf(subsystem.MeasurementAvailable)
                    Me.DisplayActiveReading()
            End Select
        End If
    End Sub

    ''' <summary> Measure subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, MeasureSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Measure Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
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
                If subsystem.TerminalsMode.HasValue Then
                    Me._TerminalStateLabel.SafeTextSetter(subsystem.TerminalsMode.Value.Description)
                Else
                    Me._TerminalStateLabel.SafeTextSetter("Rear/Front?")
                End If
                Windows.Forms.Application.DoEvents()
        End Select
    End Sub

    ''' <summary> Route subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031: DoNotCatchGeneralExceptionTypes")>
    Private Sub RouteSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, RouteSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling ROUTE subsystem property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " SENSE "

    ''' <summary> Handles the supported function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub onSupportedFunctionModesChanged(ByVal subsystem As SenseSubsystem)
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

    Private Sub onFunctionModesChanged(ByVal value As SenseFunctionSubsystemBase)
        With Me._SenseRangeNumeric
            .Minimum = CDec(value.ValueRange1.Min)
            .Maximum = CDec(value.ValueRange1.Max)
            .DecimalPlaces = CInt(Math.Max(0, -Math.Log10(value.ValueRange1.Min)))
        End With
        With Me._PowerLineCyclesNumeric
            .Minimum = CDec(value.PowerLineCyclesRange.Min)
            .Maximum = CDec(value.PowerLineCyclesRange.Max)
            .DecimalPlaces = CInt(Math.Max(0, -Math.Log10(value.PowerLineCyclesRange.Min)))
        End With
        With value
            .QueryRange()
            .QueryAutoRangeEnabled()
            .QueryAutoZeroEnabled()
            .QueryPowerLineCycles()
        End With
    End Sub

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
    Private Sub onFunctionModesChanged(ByVal value As VI.Scpi.SenseFunctionModes)
        Me.onFunctionModesChanged(Me.SelectSenseSubsystem(value))
    End Sub

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub onFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.FunctionMode.HasValue Then
            Dim value As VI.Scpi.SenseFunctionModes = subsystem.FunctionMode.GetValueOrDefault(VI.Scpi.SenseFunctionModes.None)
            If value <> VI.Scpi.SenseFunctionModes.None Then
                If Not VI.Scpi.SenseSubsystemBase.TryParse(value, Me.Device.MeasureSubsystem.Readings.Reading.Unit) Then
                    Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                       "Failed parsing function mode '{0}' to a standard unit.", subsystem.FunctionMode.Value)
                    Me.Device.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Volt
                End If
                subsystem.Readings.Reading.Unit = Me.Device.MeasureSubsystem.Readings.Reading.Unit
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.Readings.Reading.Unit.Symbol}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
                Me._SenseFunctionComboBox.SafeSelectItem(value, value.Description)
                Me.onFunctionModesChanged(value)
            End If
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
                Me.onSupportedFunctionModesChanged(subsystem)
            Case NameOf(subsystem.FunctionMode)
                Me.onFunctionModesChanged(subsystem)
                Me.DisplayActiveReading()
        End Select
    End Sub

    ''' <summary> Sense subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
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
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseVoltageSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseVoltageSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Voltage Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
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
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseCurrentSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseCurrentSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Current Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
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
        End Select
    End Sub

    ''' <summary> Sense Four Wire Resistance subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseFourWireResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseFourWireResistanceSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Resistance Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
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
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, SenseResistanceSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Sense Resistance Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Private Sub onLastError(ByVal lastError As DeviceError)
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
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.LastDeviceError)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.ErrorAvailable)
                If Not subsystem.ReadingDeviceErrors Then
                    ' if no errors, this clears the error queue.
                    subsystem.QueryDeviceErrors()
                End If
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, StatusSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

    ''' <summary> Reads a service request status. </summary>
    ''' <remarks> David, 12/26/2015. </remarks>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ReadServiceRequestStatus()
        Try
            Me.Device.StatusSubsystem.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading service request;. Details: {0}", ex)
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
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, SystemSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling SYSTEM subsystem property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " TRACE "

    ''' <summary> Handle the Trace subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As TraceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.PointsCount)
            Case NameOf(subsystem.ActualPointCount)
                Me._BufferCountLabel.Text = CStr(subsystem.ActualPointCount.GetValueOrDefault(0))
                Me._BufferCountLabel.Invalidate()
            Case NameOf(subsystem.FirstPointNumber)
                Me._FirstPointNumberLabel.Text = CStr(subsystem.FirstPointNumber.GetValueOrDefault(0))
                Me._BufferCountLabel.Invalidate()
            Case NameOf(subsystem.LastPointNumber)
                Me._LastPointNumberLabel.Text = CStr(subsystem.LastPointNumber.GetValueOrDefault(0))
                Me._BufferCountLabel.Invalidate()
        End Select
        Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary> Trace subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031: DoNotCatchGeneralExceptionTypes")>
    Private Sub TraceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, TraceSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Trace subsystem property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region


#End Region

#Region " DISPLAY SETTINGS: READING "

    ''' <summary> Selects a new reading to display.
    ''' </summary>
    Friend Function SelectReading(ByVal value As VI.ReadingTypes) As VI.ReadingTypes
        If Me.IsDeviceOpen AndAlso
                (value <> VI.ReadingTypes.None) AndAlso (value <> Me.SelectedReadingType) Then
            Me._ReadingComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.SelectedReadingType
    End Function

    ''' <summary> Gets the type of the selected reading. </summary>
    ''' <value> The type of the selected reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property SelectedReadingType() As VI.ReadingTypes
        Get
            Return CType(CType(Me._ReadingComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                                            Of [Enum], String)).Key, VI.ReadingTypes)
        End Get
    End Property

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
    Private ReadOnly Property selectedFunctionMode() As VI.Scpi.SenseFunctionModes
        Get
            Return CType(CType(Me._SenseFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                  Of [Enum], String)).Key, VI.Scpi.SenseFunctionModes)
        End Get
    End Property

#End Region

#Region " CONTROL EVENT HANDLERS: RESET "

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceInstrumentMessagesMenuItem_CheckedChanged(ByVal sender As Object, e As System.EventArgs) Handles _TraceInstrumentMessagesMenuItem.CheckedChanged
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Not Me.DesignMode AndAlso sender IsNot Nothing Then
                Dim checkBox As Windows.Forms.CheckBox = CType(sender, Windows.Forms.CheckBox)
                Me.Device.SessionMessagesTraceEnabled = checkBox.Checked
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _ClearInterfaceMenuItem for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearInterfaceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearInterfaceMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "{0} clearing interface;. {1}", Me.ResourceTitle, Me.ResourceName)
            Me.Device.SystemSubsystem.ClearInterface()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred clearing interface;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _ClearDeviceMenuItem for click events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearDeviceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearDeviceMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "{0} clearing selective device;. {1}", Me.ResourceTitle, Me.ResourceName)
            Me.Device.SystemSubsystem.ClearDevice()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred sending SDC;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Issues RST. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ResetKnownStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResetKnownStateMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} resetting known state;. {1}", Me.ResourceTitle, Me.ResourceName)
                Me.Device.ResetKnownState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred resetting known state;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _InitKnownStateMenuItem for click events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitKnownStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InitKnownStateMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} resetting known state;. {1}", Me.ResourceTitle, Me.ResourceName)
                Me.Device.ResetKnownState()
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} initializing known state;. {1}", Me.ResourceTitle, Me.ResourceName)
                Me.Device.InitKnownState()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initializing known state;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Reads terminals state menu item click. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadTerminalsStateMenuItem_Click(sender As Object, e As EventArgs) Handles _ReadTerminalsStateMenuItem.Click
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.RouteSubsystem.QueryTerminalsMode()
            Me.Device.StatusSubsystem.ReadRegisters()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initializing known state;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by _HandleServiceRequestsMenuItem for check state changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _HandleServiceRequestsMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _HandleServiceRequestsMenuItem.CheckStateChanged
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
            If menuItem IsNot Nothing AndAlso
                    Not menuItem.Checked = Me.Device.Session.IsServiceRequestEventEnabled Then
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.EnableServiceRequestEventHandler()
                    Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
                Else
                    Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)
                    Me.DisableServiceRequestEventHandler()
                End If
                Me.Device.StatusSubsystem.ReadRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred toggling service request handling mode;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


#End Region

#Region " CONTROL EVENT HANDLERS: READING "

    ''' <summary> Event handler. Called by InitButton for click events. Initiates a reading for
    ''' retrieval by way of the service request event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub InitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InitiateButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()

            ' clear execution state before enabling events
            Me.Device.ClearExecutionState()

            ' set the service request
            Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.ServiceRequests.All And Not VI.ServiceRequests.MessageAvailable)

            ' trigger the initiation of the measurement letting the service request do the rest.
            Me.Device.ClearExecutionState()
            Me.Device.TriggerSubsystem.Initiate()

        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Reading combo box selected value changed. </summary>
    ''' <remarks> David, 3/18/2016. </remarks>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles _ReadingComboBox.SelectedValueChanged
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.MeasureSubsystem.Readings.ActiveReadingType = Me.SelectedReadingType
            Me.DisplayActiveReading()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred displaying a measurement;. Details: {0}", ex)
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
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.RouteSubsystem.QueryTerminalsMode()
            Me.Device.MeasureSubsystem.Read()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: SENSE "

    ''' <summary> Applies the function mode button click. </summary>
    ''' <remarks> David, 6/27/2016. </remarks>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyFunctionModeButton_Click(sender As Object, e As EventArgs) Handles _ApplyFunctionModeButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.ApplyFunctionMode(Me.selectedFunctionMode)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Sense range setter. </summary>
    ''' <remarks> David, 2/10/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub SenseRangeSetter(ByVal value As Double)
        If value <= Me._SenseRangeNumeric.Maximum AndAlso value >= Me._SenseRangeNumeric.Minimum Then Me._SenseRangeNumeric.Value = CDec(value)
    End Sub

    ''' <summary>
    ''' Applies the selected measurements settings.
    ''' </summary>
    Private Sub applySenseSettings()

        With Me.SelectedSenseSubsystem

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
            Me.applySenseSettings()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: BUFFER "

    Private Sub ReadBuffer()
        If Me.IsDeviceOpen Then
            With Me._BufferDataGridView
                .DataSource = Nothing
                .Columns.Clear()
                .Invalidate()
                .DataSource = Me.Device.TraceSubsystem.QueryBufferReadings()
                For Each col As DataGridViewColumn In .Columns
                    If String.Equals(col.Name, NameOf(BufferReading.ElementCount)) Then
                        col.Visible = False
                    Else
                        col.HeaderText = isr.Core.Pith.SplitExtensions.SplitWords(col.Name)
                    End If
                Next
                .ScrollBars = ScrollBars.Both
            End With
        End If
    End Sub

    ''' <summary> Reads buffer button click. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadBufferButton_Click(sender As Object, e As EventArgs) Handles _ReadBufferButton.Click
        If Me._InitializingComponents Then Return
        Dim activity As String = "reading buffer"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} {1};. {2}", Me.ResourceTitle, activity, Me.ResourceName)
            Me.ReadBuffer()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred {0};. Details: {1}", activity, ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: TRIGGER "

    Private Sub _BitPatternFormatCheckBox_Click(sender As Object, e As EventArgs) Handles _BitPatternFormatCheckBox.Click
        If Me._InitializingComponents Then Return
        Me._PassBitPatternNumeric.NumericUpDownControl.Hexadecimal = _BitPatternFormatCheckBox.Checked
    End Sub

    Private Sub _HexLimit1CheckBox_Click(sender As Object, e As EventArgs) Handles _HexLimit1CheckBox.Click
        If Me._InitializingComponents Then Return
        Me._FailLimit1BitPatternNumeric.NumericUpDownControl.Hexadecimal = _BitPatternFormatCheckBox.Checked
    End Sub

    Private Sub _Limit1DecimalsNumeric_ValueChanged(sender As Object, e As EventArgs) Handles _Limit1DecimalsNumeric.ValueChanged
        If Me._InitializingComponents Then Return
        Me._LowLimit1Numeric.NumericUpDownControl.DecimalPlaces = CInt(Me._Limit1DecimalsNumeric.Value)
        Me._HighLimit1Numeric.NumericUpDownControl.DecimalPlaces = CInt(Me._Limit1DecimalsNumeric.Value)
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyGradeBinTriggerModelButton_Click(sender As Object, e As EventArgs) Handles _ApplyGradeBinTriggerModelButton.Click
        If Me._InitializingComponents Then Return
        Dim activity As String = "applying grade binning trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} {1};. {2}", Me.ResourceTitle, activity, Me.ResourceName)
                Dim count As Integer = CInt(Me._BinningTriggerCountNumeric.Value)
                Dim startDelay As TimeSpan = TimeSpan.FromSeconds(Me._StartTriggerDelayNumeric.Value)
                Dim endDelay As TimeSpan = TimeSpan.FromSeconds(Me._EndTriggerDelayNumeric.Value)
                Me.Device.TriggerSubsystem.LoadGradeBinning(count, startDelay, endDelay,
                                                            Me._HighLimit1Numeric.Value, Me._LowLimit1Numeric.Value,
                                                           CInt(Me._FailLimit1BitPatternNumeric.Value), CInt(Me._PassBitPatternNumeric.Value))
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred {0};. Details: {1}", activity, ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub LoadSimpleModel()
        If Me.IsDeviceOpen Then
            Dim count As Integer = CInt(Me._SimpleLoopCountNumeric.Value)
            Dim startDelay As TimeSpan = TimeSpan.FromSeconds(Me._StartTriggerDelayNumeric.Value)
            Me.Device.TriggerSubsystem.LoadSimpleLoop(count, startDelay)
        End If
    End Sub

    Private Sub RunSimpleModel()
        Me.Device.TraceSubsystem.ClearBuffer()
        Me.Device.TriggerSubsystem.Initiate()
        Me.Device.StatusSubsystem.Wait()
        Me.ReadBuffer()
    End Sub

    ''' <summary> Simple loop load run button click. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <param name="sender"> <see cref="Object"/>
    '''                       instance of this
    '''                       <see cref="Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleLoopLoadRunButton_Click(sender As Object, e As EventArgs) Handles _SimpleLoopLoadRunButton.Click
        If Me._InitializingComponents Then Return
        Dim activity As String = "applying simple loop trigger model"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} {1};. {2}", Me.ResourceTitle, activity, Me.ResourceName)
            Me.LoadSimpleModel()
            activity = "running simple loop trigger model"
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} {1};. {2}", Me.ResourceTitle, activity, Me.ResourceName)
            Me.RunSimpleModel()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred {0};. Details: {1}", activity, ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " READ AND WRITE "

    ''' <summary> Executes the simple read write control property changed action. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSimpleReadWriteControlPropertyChanged(sender As Instrument.SimpleReadWriteControl, ByVal propertyName As String)
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

    ''' <summary> Simple read write control property changed. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleReadWriteControl_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _SimpleReadWriteControl.PropertyChanged
        Try
            Me.OnSimpleReadWriteControlPropertyChanged(TryCast(sender, Instrument.SimpleReadWriteControl), e?.PropertyName)
        Catch ex As Exception
            Me.StatusLabel.Text = "Exception occurred handling change"
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Simple Read and Write property changed Event;. Failed property {0}. Details: {1}",
                               e?.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds listeners such as current level trace message box and log. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    Protected Overrides Sub AddListeners()
        MyBase.AddListeners()
        Me._SimpleReadWriteControl.AddListeners(Me.Talker.Listeners)
    End Sub

    ''' <summary> Adds listeners such as top level trace message box and log. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of ITraceMessageListener))
        MyBase.AddListeners(listeners)
        Me._SimpleReadWriteControl.AddListeners(listeners)
    End Sub

    ''' <summary> Adds the log listener. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="log"> The log. </param>
    Public Overrides Sub AddListeners(ByVal log As MyLog)
        If log Is Nothing Then Throw New ArgumentNullException(NameOf(log))
        MyBase.AddListeners(log)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
    Private _actualFunctionMode As VI.Scpi.SenseFunctionModes
    ''' <summary>
    ''' Return the last Sense function mode.  This is set only after the 
    ''' actual value is applies whereas the <see cref="SelectedFunctionMode">selected mode</see>
    ''' reflects the status of the combo box.
    ''' </summary>
    Friend ReadOnly Property ActualFunctionMode() As VI.Scpi.SenseFunctionModes
        Get
            Return Me._actualFunctionMode
        End Get
    End Property


#End If
#End Region