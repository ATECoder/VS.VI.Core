Imports System.Windows.Forms
Imports System.ComponentModel
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.CheckBoxExtensions
Imports isr.Core.Controls.ControlExtensions
Imports isr.Core.Controls.NumericUpDownExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Provides a user interface for the Keysight 34980 Device. </summary>
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
<System.ComponentModel.DisplayName("K34980 Control"),
      System.ComponentModel.Description("Keysight 34980 Device Control"),
      System.Drawing.ToolboxBitmap(GetType(K34980Control))>
Public Class K34980Control
    Inherits VI.Instrument.ResourceControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private Property InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(Device.Create)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Private Sub New(ByVal device As Device)
        MyBase.New()
        Me.InitializingComponents = True
        ' This call is required by the designer.
        InitializeComponent()
        Me.InitializingComponents = False

        Me._ToolStripPanel.Renderer = New CustomProfessionalRenderer
        MyBase.Connector = Me._ResourceSelectorConnector

        Me._AssignDevice(device)
        ' note that the caption is not set if this is run inside the On Load function.
        With Me._TraceMessagesBox
            ' set defaults for the messages box.
            .ResetCount = 500
            .PresetCount = 250
            .ContainerPanel = Me._MessagesTabPage
        End With
#If False Then
        With Me._ServiceRequestFlagsComboBox
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = [Enum].GetNames(GetType(isr.VI.ServiceRequests))
        End With
#End If
        With Me._ServiceRequestEnableBitmaskNumeric.NumericUpDownControl
            .Hexadecimal = True
            .Maximum = 255
            .Minimum = 0
            .Value = 0
        End With
        Me._TraceButton.Visible = False
        Me._ReadingsCountLabel.Visible = False
        Me._ClearBufferDisplayButton.Visible = False
        Me.EnableTraceLevelControls()
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the isr.VI.Instrument.ResourcePanelBase and
    ''' optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' the device gets disposed in the base class!
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " FORM EVENTS "

    ''' <summary> Handles the <see cref="E:System.Windows.Forms.UserControl.Load" /> event. </summary>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Try
        Finally
            MyBase.OnLoad(e)
        End Try
    End Sub

#End Region

#Region " DEVICE "

    Private _Device As Device
    ''' <summary> Gets or sets the device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property Device As Device
        Get
            Return Me._Device
        End Get
        Set(value As Device)
            Me._AssignDevice(value)
        End Set
    End Property

    ''' <summary> Assign device. </summary>
    ''' <param name="value"> The value. </param>
    Private Sub _AssignDevice(ByVal value As Device)
        If Me._Device IsNot Nothing Then
            ' the device base already clears all or only the private listeners. 
        End If
        Me._Device = value
        If value IsNot Nothing Then
            value.CaptureSyncContext(WindowsFormsSynchronizationContext.Current)
            value.AddPrivateListener(Me._TraceMessagesBox)
        End If
        MyBase.DeviceBase = value
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device)
        Me.IsDeviceOwner = False
        Me.Device = value
    End Sub

    ''' <summary> Releases the device. </summary>
    Protected Overrides Sub ReleaseDevice()
        MyBase.ReleaseDevice()
        Me._Device = Nothing
    End Sub

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
            If t IsNot Me._MessagesTabPage Then Me.RecursivelyEnable(t.Controls, Me.IsDeviceOpen)
        Next
    End Sub

    ''' <summary> Handles the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnDevicePropertyChanged(ByVal device As DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnDevicePropertyChanged(device, propertyName)
        Select Case propertyName
            Case NameOf(isr.VI.DeviceBase.SessionServiceRequestEventEnabled)
                Me._SessionServiceRequestHandlerEnabledMenuItem.Checked = device.SessionServiceRequestEventEnabled
            Case NameOf(isr.VI.DeviceBase.DeviceServiceRequestHandlerAdded)
                Me._DeviceServiceRequestHandlerEnabledMenuItem.Checked = device.DeviceServiceRequestHandlerAdded
            Case NameOf(isr.VI.DeviceBase.SessionMessagesTraceEnabled)
                Me._SessionTraceEnabledMenuItem.Checked = device.SessionMessagesTraceEnabled
            Case NameOf(isr.VI.DeviceBase.ServiceRequestEnableBitmask)
                Me._ServiceRequestEnableBitmaskNumeric.Value = device.ServiceRequestEnableBitmask
                Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = $"SRE:0b{Convert.ToString(device.ServiceRequestEnableBitmask, 2),8}".Replace(" ", "0")
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.Device.FormatSubsystem IsNot Nothing Then AddHandler Me.Device.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
        If Me.Device.MeasureSubsystem IsNot Nothing Then AddHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
        If Me.Device.RouteSubsystem IsNot Nothing Then AddHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
        If Me.Device.SenseSubsystem IsNot Nothing Then AddHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
        If Me.Device.SenseVoltageSubsystem IsNot Nothing Then AddHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
        If Me.Device.SenseCurrentSubsystem IsNot Nothing Then AddHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
        If Me.Device.SenseFourWireResistanceSubsystem IsNot Nothing Then AddHandler Me.Device.SenseFourWireResistanceSubsystem.PropertyChanged, AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged
        If Me.Device.SenseResistanceSubsystem IsNot Nothing Then AddHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
        'AddHandler Me.Device.TraceSubsystem.PropertyChanged, AddressOf Me.TraceSubsystemPropertyChanged
        'AddHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
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
            If Me.Device.FormatSubsystem IsNot Nothing Then RemoveHandler Me.Device.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
            If Me.Device.MeasureSubsystem IsNot Nothing Then RemoveHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
            If Me.Device.RouteSubsystem IsNot Nothing Then RemoveHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
            If Me.Device.SenseCurrentSubsystem IsNot Nothing Then RemoveHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
            If Me.Device.SenseFourWireResistanceSubsystem IsNot Nothing Then RemoveHandler Me.Device.SenseFourWireResistanceSubsystem.PropertyChanged, AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged
            If Me.Device.SenseResistanceSubsystem IsNot Nothing Then RemoveHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
            If Me.Device.SenseVoltageSubsystem IsNot Nothing Then RemoveHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
            If Me.Device.SenseSubsystem IsNot Nothing Then RemoveHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
            'RemoveHandler Me.Device.TraceSubsystem.PropertyChanged, AddressOf Me.TraceSubsystemPropertyChanged
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
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        Select Case propertyName
            Case NameOf(K34980.FormatSubsystem.Elements)
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
                               "Exception handling Format Subsystem property changed Event;. Failed property {0}. {1}", e.PropertyName, ex.ToFullBlownString)
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
    End Sub

    ''' <summary> Handles the Measure subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As MeasureSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        Select Case propertyName
            Case NameOf(K34980.MeasureSubsystem.LastReading)
                Me._LastReadingTextBox.SafeTextSetter(subsystem.LastReading)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Measure message: {0}.", subsystem.LastReading.InsertCommonEscapeSequences)
            Case NameOf(K34980.MeasureSubsystem.MeasurementAvailable)
                Me.DisplayActiveReading()
        End Select
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
                               "Exception handling Measure Subsystem property changed Event;. Failed property {0}. {1}",
                               e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " ROUTE "

    ''' <summary> Handles the ROUTE subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As RouteSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        Select Case propertyName
            Case NameOf(K34980.RouteSubsystem.ClosedChannel)
                Me.ClosedChannels = subsystem.ClosedChannel
            Case NameOf(K34980.RouteSubsystem.ClosedChannels)
                Me.ClosedChannels = subsystem.ClosedChannels
        End Select
    End Sub

    ''' <summary> ROUTE subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RouteSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, RouteSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling ROUTE Subsystem property changed Event;. Failed property {0}. {1}",
                               e.PropertyName, ex.ToFullBlownString)
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

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="value"> The <see cref="TraceMessage">message</see> to display and log. </param>
    Private Shared Sub OnFunctionModesChanged(ByVal value As SenseFunctionSubsystemBase)
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
    Private Sub OnFunctionModesChanged(ByVal value As VI.Scpi.SenseFunctionModes)
        K34980Control.OnFunctionModesChanged(Me.SelectSenseSubsystem(value))
    End Sub

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.FunctionMode.HasValue Then
            Dim value As VI.Scpi.SenseFunctionModes = subsystem.FunctionMode.GetValueOrDefault(VI.Scpi.SenseFunctionModes.None)
            If value <> VI.Scpi.SenseFunctionModes.None Then
                Me._SenseFunctionComboBox.SafeSelectItem(value, value.Description)
                Me.OnFunctionModesChanged(value)
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
            Case NameOf(K34980.SenseSubsystem.MeasurementAvailable)
                Me.DisplayActiveReading()
            Case NameOf(K34980.SenseSubsystem.SupportedFunctionModes)
                Me.onSupportedFunctionModesChanged(subsystem)
            Case NameOf(K34980.SenseSubsystem.FunctionMode)
                Me.OnFunctionModesChanged(subsystem)
                Me.DisplayActiveReading()
            Case NameOf(K34980.SenseSubsystem.FunctionRange)
                With Me._SenseRangeNumeric
                    .Maximum = CDec(subsystem.FunctionRange.Max)
                    .Minimum = CDec(subsystem.FunctionRange.Min)
                End With
            Case NameOf(K34980.SenseSubsystem.FunctionRangeDecimalPlaces)
                Me._SenseRangeNumeric.DecimalPlaces = subsystem.FunctionRangeDecimalPlaces
            Case NameOf(K34980.SenseSubsystem.FunctionUnit)
                Me.Device.MeasureSubsystem.Readings.Reading.Unit = subsystem.FunctionUnit
                subsystem.Readings.Reading.Unit = subsystem.FunctionUnit
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.FunctionUnit}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
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
                               "Exception handling Sense Subsystem property changed Event;. Failed property {0}. {1}",
                               e.PropertyName, ex.ToFullBlownString)
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
            Case NameOf(K34980.SenseVoltageSubsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(K34980.SenseVoltageSubsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(K34980.SenseVoltageSubsystem.PowerLineCyclesRange)
                With Me._PowerLineCyclesNumeric
                    .Maximum = CDec(subsystem.PowerLineCyclesRange.Max)
                    .Minimum = CDec(subsystem.PowerLineCyclesRange.Min)
                    .DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces
                End With
            Case NameOf(K34980.SenseVoltageSubsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
            Case NameOf(K34980.SenseVoltageSubsystem.FunctionRange)
                With Me._SenseRangeNumeric
                    .Maximum = CDec(subsystem.FunctionRange.Max)
                    .Minimum = CDec(subsystem.FunctionRange.Min)
                End With
            Case NameOf(K34980.SenseVoltageSubsystem.FunctionRangeDecimalPlaces)
                Me._SenseRangeNumeric.DecimalPlaces = subsystem.DefaultFunctionModeDecimalPlaces
            Case NameOf(K34980.SenseVoltageSubsystem.FunctionUnit)
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.FunctionUnit}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
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
                               "Exception handling Sense Voltage Subsystem property changed Event;. Failed property {0}. {1}",
                               e.PropertyName, ex.ToFullBlownString)
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
            Case NameOf(K34980.SenseCurrentSubsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(K34980.SenseCurrentSubsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(K34980.SenseCurrentSubsystem.PowerLineCyclesRange)
                With Me._PowerLineCyclesNumeric
                    .Maximum = CDec(subsystem.PowerLineCyclesRange.Max)
                    .Minimum = CDec(subsystem.PowerLineCyclesRange.Min)
                    .DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces
                End With
            Case NameOf(K34980.SenseCurrentSubsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
            Case NameOf(K34980.SenseCurrentSubsystem.FunctionRange)
                With Me._SenseRangeNumeric
                    .Maximum = CDec(subsystem.FunctionRange.Max)
                    .Minimum = CDec(subsystem.FunctionRange.Min)
                End With
            Case NameOf(K34980.SenseCurrentSubsystem.FunctionRangeDecimalPlaces)
                Me._SenseRangeNumeric.DecimalPlaces = subsystem.DefaultFunctionModeDecimalPlaces
            Case NameOf(K34980.SenseCurrentSubsystem.FunctionUnit)
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.FunctionUnit}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
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
                               "Exception handling Sense Current Subsystem property changed Event;. Failed property {0}. {1}",
                               e.PropertyName, ex.ToFullBlownString)
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
            Case NameOf(K34980.SenseFourWireResistanceSubsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(K34980.SenseFourWireResistanceSubsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(K34980.SenseFourWireResistanceSubsystem.PowerLineCyclesRange)
                With Me._PowerLineCyclesNumeric
                    .Maximum = CDec(subsystem.PowerLineCyclesRange.Max)
                    .Minimum = CDec(subsystem.PowerLineCyclesRange.Min)
                    .DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces
                End With
            Case NameOf(K34980.SenseFourWireResistanceSubsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
            Case NameOf(K34980.SenseFourWireResistanceSubsystem.FunctionRange)
                With Me._SenseRangeNumeric
                    .Maximum = CDec(subsystem.FunctionRange.Max)
                    .Minimum = CDec(subsystem.FunctionRange.Min)
                End With
            Case NameOf(K34980.SenseFourWireResistanceSubsystem.FunctionRangeDecimalPlaces)
                Me._SenseRangeNumeric.DecimalPlaces = subsystem.DefaultFunctionModeDecimalPlaces
            Case NameOf(K34980.SenseFourWireResistanceSubsystem.FunctionUnit)
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.FunctionUnit}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
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
                               "Exception handling Sense Resistance Subsystem property changed Event;. Failed property {0}. {1}",
                               e.PropertyName, ex.ToFullBlownString)
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
            Case NameOf(K34980.SenseResistanceSubsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(K34980.SenseResistanceSubsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Me._PowerLineCyclesNumeric.SafeValueSetter(subsystem.PowerLineCycles.Value)
                End If
            Case NameOf(K34980.SenseResistanceSubsystem.PowerLineCyclesRange)
                With Me._PowerLineCyclesNumeric
                    .Maximum = CDec(subsystem.PowerLineCyclesRange.Max)
                    .Minimum = CDec(subsystem.PowerLineCyclesRange.Min)
                    .DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces
                End With
            Case NameOf(K34980.SenseResistanceSubsystem.Range)
                If Me.Device IsNot Nothing AndAlso subsystem.Range.HasValue Then
                    Me._SenseRangeNumeric.SafeValueSetter(subsystem.Range.Value)
                End If
            Case NameOf(K34980.SenseResistanceSubsystem.FunctionRange)
                With Me._SenseRangeNumeric
                    .Maximum = CDec(subsystem.FunctionRange.Max)
                    .Minimum = CDec(subsystem.FunctionRange.Min)
                End With
            Case NameOf(K34980.SenseResistanceSubsystem.FunctionRangeDecimalPlaces)
                Me._SenseRangeNumeric.DecimalPlaces = subsystem.DefaultFunctionModeDecimalPlaces
            Case NameOf(K34980.SenseResistanceSubsystem.FunctionUnit)
                Me._SenseRangeNumericLabel.Text = $"Range [{subsystem.FunctionUnit}]:"
                Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
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
                               "Exception handling Sense Resistance Subsystem property changed Event;. Failed property {0}. {1}",
                               e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Protected Overrides Sub OnLastError(ByVal lastError As DeviceError)
        If lastError IsNot Nothing Then
            Me._LastErrorTextBox.ForeColor = If(lastError.IsError, Drawing.Color.OrangeRed, Drawing.Color.Aquamarine)
            Me._LastErrorTextBox.Text = lastError.CompoundErrorMessage
        End If
    End Sub

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        MyBase.OnPropertyChanged(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(VI.StatusSubsystemBase.DeviceErrors)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(VI.StatusSubsystemBase.LastDeviceError)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(VI.StatusSubsystemBase.ErrorAvailable)
                If Not subsystem.ReadingDeviceErrors Then
                    ' if no errors, this clears the error queue.
                    subsystem.QueryDeviceErrors()
                End If
            Case NameOf(VI.StatusSubsystemBase.ServiceRequestStatus)
                Me._StatusRegisterLabel.Text = $"0x{subsystem.ServiceRequestStatus:X2}"
            Case NameOf(VI.StatusSubsystemBase.StandardEventStatus)
                Me._StandardRegisterLabel.Text = $"0x{subsystem.StandardEventStatus:X2}"
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
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
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
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub


#End Region

#End Region

#Region " STATUS DISPLAY "

    ''' <summary> Gets or sets the status. </summary>
    ''' <value> The status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property Status As String
        Get
            Return Me._StatusLabel.Text
        End Get
        Set(value As String)
            Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(value, Me._StatusLabel)
            Me._StatusLabel.ToolTipText = value
        End Set
    End Property

    ''' <summary> Gets or sets the identity. </summary>
    ''' <value> The identity. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property Identity As String
        Get
            Return Me._IdentityLabel.Text
        End Get
        Set(value As String)
            Me._IdentityLabel.Text = isr.Core.Pith.CompactExtensions.Compact(value, Me._IdentityLabel)
        End Set
    End Property

    ''' <summary> Gets or sets the status register caption. </summary>
    ''' <value> The status register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property StatusRegisterCaption As String
        Get
            Return Me._StatusRegisterLabel.Text
        End Get
        Set(value As String)
            Me._StatusRegisterLabel.Text = value
        End Set
    End Property

    ''' <summary> Gets or sets the standard register caption. </summary>
    ''' <value> The status register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property StandardRegisterCaption As String
        Get
            Return Me._StandardRegisterLabel.Text
        End Get
        Set(value As String)
            Me._StandardRegisterLabel.Text = value
        End Set
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
    Private ReadOnly Property SelectedFunctionMode() As VI.Scpi.SenseFunctionModes
        Get
            Return CType(CType(Me._SenseFunctionComboBox.SelectedItem, System.Collections.Generic.KeyValuePair(
                  Of [Enum], String)).Key, VI.Scpi.SenseFunctionModes)
        End Get
    End Property

#End Region

#Region " CHANNELS "

    ''' <summary> Gets or sets the scan list of closed channels. </summary>
    ''' <value> The closed channels. </value>
    Public Property ClosedChannels() As String
        Get
            Return Me._ClosedChannelsTextBox.Text
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then
                Me._ClosedChannelsTextBox.SafeTextSetter("?")
            Else
                Me._ClosedChannelsTextBox.SafeTextSetter(value)
            End If
        End Set
    End Property

    ''' <summary> Adds new items to the combo box. </summary>
    Friend Sub UpdateChannelListComboBox()
        If Me.Visible Then
            ' check if we are asking for a new channel list
            If Me._ChannelListComboBox IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(Me._ChannelListComboBox.Text) AndAlso
                    Me._ChannelListComboBox.FindString(Me._ChannelListComboBox.Text) < 0 Then
                ' if we have a new string, add it to the channel list
                Me._ChannelListComboBox.Items.Add(Me._ChannelListComboBox.Text)
            End If
        End If
    End Sub

    ''' <summary> Event handler. Called by _closeChannelsButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _CloseChannelsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _CloseChannelsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            UpdateChannelListComboBox()
            Me.Device.ClearExecutionState()
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.Device.RouteSubsystem.ApplyClosedChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(1))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by channel_OpenButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _OpenChannelsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _OpenChannelsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            UpdateChannelListComboBox()
            Me.Device.ClearExecutionState()
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.Device.RouteSubsystem.ApplyOpenChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(1))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by CloseOnlyButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub CloseOnlyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _CloseOnlyButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            UpdateChannelListComboBox()
            Me.Device.ClearExecutionState()
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.Device.RouteSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(1))
            Me.Device.RouteSubsystem.ApplyClosedChannels(Me._ChannelListComboBox.Text, TimeSpan.FromSeconds(1))
            ' this works only if a single channel:
            ' VI.RouteSubsystem.CloseChannels(Me.Device, Me._channelListComboBox.Text)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by OpenAllButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OpenAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _OpenAllButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Device.ClearExecutionState()
            Me.Device.StatusSubsystem.EnableWaitComplete()
            Me.Device.RouteSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(1))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

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
                Me._InfoProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SystemSubsystem.ClearInterface()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
    Private Sub _ClearDeviceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim activity As String = "clearing selective device"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            If menuItem IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me._InfoProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SystemSubsystem.ClearDevice()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
                Me._InfoProvider.Clear()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SystemSubsystem.ClearExecutionState()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing Then
                If Me.IsDeviceOpen Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.ResetKnownState()
                End If
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing Then
                If Me.IsDeviceOpen Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.ResetKnownState()
                    activity = "initializing known state"
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                    Me.Device.InitKnownState()
                End If
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
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
            Me._InfoProvider.Clear()
            Me.Device.ApplyTalkerTraceLevel(ListenerType.Logger,
                                            TalkerControlBase.SelectedValue(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
            Me._InfoProvider.Clear()
            Me.Device.ApplyTalkerTraceLevel(ListenerType.Display,
                                            TalkerControlBase.SelectedValue(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.Message)
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
        If Me._InitializingComponents Then Return
        Dim activity As String = "toggling instrument message tracing"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        If menuItem IsNot Nothing Then
        End If
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.SessionMessagesTraceEnabled = menuItem.Checked
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
        If Me._InitializingComponents Then Return
        Dim activity As String = "Toggle session service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
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
            Me._InfoProvider.Annunciate(sender, ex.ToString)
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
        If Me._InitializingComponents Then Return
        Dim activity As String = "Toggle device service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
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
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: READING "

    ''' <summary> Selects a new reading to display. </summary>
    ''' <param name="value"> The <see cref="TraceMessage">message</see> to display and
    ''' log. </param>
    ''' <returns> The VI.ReadingElements. </returns>
    Friend Function SelectReading(ByVal value As VI.ReadingTypes) As VI.ReadingTypes
        If Me.IsDeviceOpen AndAlso
                (value <> VI.ReadingTypes.None) AndAlso (value <> Me.SelectedReadingType) Then
            Me._ReadingComboBox.ComboBox.SafeSelectItem(value.ValueDescriptionPair)
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
    ''' <summary> Abort button click. </summary>
    ''' <param name="sender"> <see cref="T:System.Object" />
    '''                                             instance of this
    '''                       <see cref="T:System.Windows.Forms.Control" /> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AbortButton_Click(sender As Object, e As EventArgs) Handles _AbortButton.Click

        If Me._InitializingComponents Then Return
        Dim activity As String = "aborting measurements(s)"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
                Me.Device.TriggerSubsystem.Abort()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Initiate button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/>
    '''                       instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitiateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InitiateButton.Click
        Dim activity As String = "initiating measurements(s)"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()

            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")

            ' clear execution state before enabling events
            Me.Device.ClearExecutionState()

            ' set the service request
            Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.ServiceRequests.All And Not VI.ServiceRequests.MessageAvailable)

            ' trigger the initiation of the measurement letting the service request do the rest.
            Me.Device.ClearExecutionState()
            Me.Device.TriggerSubsystem.Initiate()

        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Reading combo box selected index changed. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _ReadingComboBox.SelectedIndexChanged
        If Me._InitializingComponents Then Return
        Dim activity As String = "selecting a reading"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.MeasureSubsystem.Readings.ActiveReadingType = Me.SelectedReadingType
            Me.DisplayActiveReading()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Read button click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadButton.Click
        Dim activity As String = "reading"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            Me.Device.MeasureSubsystem?.Read()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Handles the DataError event of the _dataGridView control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="DataGridViewDataErrorEventArgs"/> instance containing the event data.</param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingsDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles _ReadingsDataGridView.DataError
        Try
            ' prevent error reporting when adding a new row or editing a cell
            Dim grid As DataGridView = TryCast(sender, DataGridView)
            If grid IsNot Nothing Then
                If grid.CurrentRow IsNot Nothing AndAlso grid.CurrentRow.IsNewRow Then Return
                If grid.IsCurrentCellInEditMode Then Return
                If grid.IsCurrentRowDirty Then Return
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   $"{e.Exception.Message} occurred editing row {e.RowIndex} column {e.ColumnIndex};. {e.Exception.ToFullBlownString}")
                Me._InfoProvider.Annunciate(grid, $"{e.Exception.Message} occurred editing table")
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
    Private Sub _TraceButton_Click(sender As Object, e As EventArgs) Handles _TraceButton.Click
        If Me._InitializingComponents Then Return
        Dim activity As String = "reading readings"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            'Dim values As IEnumerable(Of Readings) = Me.Device.TraceSubsystem.QueryReadings(Me.Device.MeasureSubsystem.Readings)
            'Me._ReadingsCountLabel.Text = values?.Count.ToString
            'Me.Device.TraceSubsystem.DisplayReadings(Me._ReadingsDataGridView, values)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
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
        If Me._InitializingComponents Then Return
        Dim activity As String = "clearing buffer display"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} {activity};. {Me.ResourceName}")
            ' Me.Device.TraceSubsystem.DisplayReadings(Me._ReadingsDataGridView, New List(Of Readings))
            Me._ReadingsCountLabel.Text = "0"
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

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
            Me._InfoProvider.Clear()
            Me.ApplyFunctionMode(Me.SelectedFunctionMode)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
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
            Me._InfoProvider.Clear()
            Me.ApplySenseSettings()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred initiating a measurement;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
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
                Case NameOf(Instrument.SimpleReadWriteControl.StatusMessage)
                    Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusMessage, Me._StatusLabel)
                    Me._StatusLabel.ToolTipText = sender.StatusMessage
                Case NameOf(Instrument.SimpleReadWriteControl.ServiceRequestValue)
                    Me._StatusRegisterLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
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
                               "Exception handling {0} property change;. {1}",
                               e?.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

    ''' <summary> Assigns talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AssignTalker(talker As ITraceMessageTalker)
        Me._SimpleReadWriteControl.AssignTalker(talker)
        MyBase.AssignTalker(talker)
        ' My.MyLibrary.Identify(talker)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        Me._SimpleReadWriteControl.ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        ' MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

#End Region

#Region " TOOL STRIP RENDERER "

    ''' <summary> A custom professional renderer. </summary>
    ''' <license>
    ''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
    ''' Licensed under The MIT License.</para><para>
    ''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
    ''' </license>
    ''' <history date="10/30/2017" by="David" revision=""> Created. </history>
    Private Class CustomProfessionalRenderer
        Inherits ToolStripProfessionalRenderer

        Protected Overrides Sub OnRenderLabelBackground(ByVal e As ToolStripItemRenderEventArgs)
            If e IsNot Nothing AndAlso (e.Item.BackColor <> System.Drawing.SystemColors.ControlDark) Then
                Using brush As New Drawing.SolidBrush(e.Item.BackColor)
                    e.Graphics.FillRectangle(brush, e.Item.ContentRectangle)
                End Using
            End If
        End Sub
    End Class

#End Region

End Class
