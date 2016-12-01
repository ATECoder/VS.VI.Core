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
<System.ComponentModel.DisplayName("K2000 Panel"),
      System.ComponentModel.Description("Keithley 2000 Device Panel"),
      System.Drawing.ToolboxBitmap(GetType(K2000Panel))>
Public Class K2000Panel
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
                Try
                    If Me.Device IsNot Nothing Then Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", "Exception details: {0}", ex)
                End Try
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
        If Me.IsDeviceOwner Then
            MyBase.ReleaseDevice()
        Else
            Me._Device = Nothing
        End If
    End Sub

    ''' <summary> Gets a reference to the Keithley 2000 Device. </summary>
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
        'AddHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
        AddHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
        AddHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
        AddHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
        AddHandler Me.Device.SenseFourWireResistanceSubsystem.PropertyChanged, AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged
        AddHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
        'AddHandler Me.Device.TraceSubsystem.PropertyChanged, AddressOf Me.TraceSubsystemPropertyChanged
        AddHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
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
            'RemoveHandler Me.Device.RouteSubsystem.PropertyChanged, AddressOf Me.RouteSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseCurrentSubsystem.PropertyChanged, AddressOf Me.SenseCurrentSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseResistanceSubsystem.PropertyChanged, AddressOf Me.SenseResistanceSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseFourWireResistanceSubsystem.PropertyChanged, AddressOf Me.SenseFourWireResistanceSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseVoltageSubsystem.PropertyChanged, AddressOf Me.SenseVoltageSubsystemPropertyChanged
            RemoveHandler Me.Device.SenseSubsystem.PropertyChanged, AddressOf Me.SenseSubsystemPropertyChanged
            'RemoveHandler Me.Device.TraceSubsystem.PropertyChanged, AddressOf Me.TraceSubsystemPropertyChanged
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
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
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

#Region " SENSE "

    ''' <summary> Handles the supported function modes changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub onSupportedFunctionModesChanged(ByVal subsystem As SenseSubsystem)
        If subsystem IsNot Nothing AndAlso subsystem.SupportedFunctionModes <> VI.Scpi.SenseFunctionModes.None Then
            subsystem.DisplaySupportedFunctionModes(Me._SenseFunctionComboBox)
        End If
    End Sub

    ''' <summary> Handles the function modes changed action. </summary>
    ''' <param name="value"> The <see cref="TraceMessage">message</see> to display and
    ''' log. </param>
    Private Sub onFunctionModesChanged(ByVal value As VI.Scpi.SenseFunctionModes)
        Dim format As String = "Range [{0}]:"
        Select Case value
            Case VI.Scpi.SenseFunctionModes.CurrentDC, VI.Scpi.SenseFunctionModes.Current, VI.Scpi.SenseFunctionModes.CurrentAC
                Me._SenseRangeNumericLabel.Text = String.Format(format, "A")
                With Me._SenseRangeNumeric
                    .Minimum = 0
                    .Maximum = 10D
                    .DecimalPlaces = 3
                End With
            Case VI.Scpi.SenseFunctionModes.VoltageDC, VI.Scpi.SenseFunctionModes.Voltage, VI.Scpi.SenseFunctionModes.VoltageAC
                Me._SenseRangeNumericLabel.Text = String.Format(format, "V")
                With Me._SenseRangeNumeric
                    .Minimum = 0
                    .Maximum = 1000D
                    .DecimalPlaces = 3
                End With
            Case VI.Scpi.SenseFunctionModes.FourWireResistance
                Me._SenseRangeNumericLabel.Text = String.Format(format, "Ohm")
                With Me._SenseRangeNumeric
                    .Minimum = 0
                    .Maximum = 2000000D
                    .DecimalPlaces = 0
                End With
            Case VI.Scpi.SenseFunctionModes.Resistance
                Me._SenseRangeNumericLabel.Text = String.Format(format, "Ohm")
                With Me._SenseRangeNumeric
                    .Minimum = 0
                    .Maximum = 1000000000D
                    .DecimalPlaces = 0
                End With
        End Select
        Me._SenseRangeNumericLabel.Left = Me._SenseRangeNumeric.Left - Me._SenseRangeNumericLabel.Width
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
                End If
                Me._SenseFunctionComboBox.SafeSelectItem(value, value.Description)
                Me.onFunctionModesChanged(value)
            End If
        End If
    End Sub

    ''' <summary> Handle the Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.MeasurementAvailable)
                Me.DisplayActiveReading()
            Case NameOf(subsystem.SupportedFunctionModes)
                Me.onSupportedFunctionModesChanged(subsystem)
            Case NameOf(subsystem.FunctionMode)
                Me.onFunctionModesChanged(subsystem)
                Me._SenseRangeNumericLabel.Text = String.Format(Globalization.CultureInfo.CurrentCulture,
                                                                    "Range [{0}]:", subsystem.Readings.Reading.Unit.Symbol)
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
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Dim nplc As Double = subsystem.PowerLineCycles.Value
                    Me._IntegrationPeriodNumeric.SafeValueSetter(VI.StatusSubsystemBase.IntegrationPeriod(nplc).TotalMilliseconds)
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
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Dim nplc As Double = subsystem.PowerLineCycles.Value
                    Me._IntegrationPeriodNumeric.SafeValueSetter(VI.StatusSubsystemBase.IntegrationPeriod(nplc).TotalMilliseconds)
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
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Dim nplc As Double = subsystem.PowerLineCycles.Value
                    Me._IntegrationPeriodNumeric.SafeValueSetter(VI.StatusSubsystemBase.IntegrationPeriod(nplc).TotalMilliseconds)
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
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
        ' Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        ' Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        Select Case propertyName
            Case NameOf(subsystem.AutoRangeEnabled)
                If Me.Device IsNot Nothing AndAlso subsystem.AutoRangeEnabled.HasValue Then
                    Me._SenseAutoRangeToggle.SafeCheckedSetter(subsystem.AutoRangeEnabled.Value)
                End If
            Case NameOf(subsystem.PowerLineCycles)
                If Me.Device IsNot Nothing AndAlso subsystem.PowerLineCycles.HasValue Then
                    Dim nplc As Double = subsystem.PowerLineCycles.Value
                    Me._IntegrationPeriodNumeric.SafeValueSetter(VI.StatusSubsystemBase.IntegrationPeriod(nplc).TotalMilliseconds)
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
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName)  Then Return
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
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.FrontSwitched)
                If subsystem.FrontSwitched.HasValue Then
                    Me._TerminalStateLabel.Text = If(subsystem.FrontSwitched, "Front", "Rear")
                End If
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
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
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
            Case NameOf(subsystem.ContinuousEnabled)
                If subsystem.ContinuousEnabled.HasValue Then
                    Me._ContinuousTriggerEnabledCheckBox.Checked = subsystem.ContinuousEnabled.Value
                End If
        End Select
    End Sub

    ''' <summary> Trigger subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TriggerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, TriggerSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Trigger Subsystem property changed Event;. Failed property {0}. Details: {1}",
                               e.PropertyName, ex)
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

    ''' <summary>
    ''' Selects a new sense mode.
    ''' </summary>
    Friend Sub ApplyFunctionMode(ByVal value As VI.Scpi.SenseFunctionModes)
        If Me.IsDeviceOpen AndAlso
                ((Me._Device.SenseSubsystem.SupportedFunctionModes And value) <> 0) AndAlso
                (value <> Me._actualFunctionMode) Then
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
            Me.Device.SystemSubsystem.QueryFrontSwitched()
            Me.Device.MeasureSubsystem.Read()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _terminalsToggle for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _TerminalsToggle_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me._InitializingComponents Then Return
        Dim checkBox As Windows.Forms.CheckBox = TryCast(sender, Windows.Forms.CheckBox)
        If checkBox IsNot Nothing Then
            If checkBox.CheckState = Windows.Forms.CheckState.Indeterminate Then
                checkBox.Text = "R/F?"
            Else
                checkBox.Text = CStr(IIf(checkBox.Checked, "Rear", "Front"))
            End If
        End If
    End Sub

    ''' <summary> Event handler. Called by _HandleServiceRequestsCheckBox for check state changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _HandleServiceRequestsCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me._InitializingComponents Then Return
        Dim checkBox As CheckBox = TryCast(sender, CheckBox)
        If checkBox IsNot Nothing AndAlso
                    Not checkBox.Checked = Me.Device.Session.IsServiceRequestEventEnabled Then
            If checkBox IsNot Nothing AndAlso checkBox.Checked Then
                Me.EnableServiceRequestEventHandler()
                Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
            Else
                Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)
                Me.DisableServiceRequestEventHandler()
            End If
            Me.Device.StatusSubsystem.ReadRegisters()
        End If
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: SENSE "

    ''' <summary> Event handler. Called by _SenseFunctionComboBox for selected index changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="degC")>
    Private Sub _SenseFunctionComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SenseFunctionComboBox.SelectedIndexChanged
        Dim control As Windows.Forms.Control = TryCast(sender, Windows.Forms.Control)
        If Not Me._InitializingComponents AndAlso control IsNot Nothing AndAlso control.Enabled Then
            Me.onFunctionModesChanged(Me.selectedFunctionMode)
        End If
    End Sub

    ''' <summary>
    ''' Applies the selected measurements settings.
    ''' </summary>
    Private Sub applySenseSettings()

        Me.Device.ClearExecutionState()

        Me.Device.SenseSubsystem.ApplyFunctionMode(Me.selectedFunctionMode)

        If Me.Device.SenseSubsystem.FunctionMode = VI.Scpi.SenseFunctionModes.CurrentDC Then

            Me.Device.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Ampere

            With Me.Device.SenseCurrentSubsystem
                ' set the Integration Period
                .ApplyPowerLineCycles(VI.StatusSubsystemBase.PowerLineCycles(TimeSpan.FromMilliseconds(Me._IntegrationPeriodNumeric.Value)))

                ' set the range
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)

                If Not Me._SenseAutoRangeToggle.Checked Then
                    .ApplyRange(Me._SenseRangeNumeric.Value)
                End If
            End With

        ElseIf Me.Device.SenseSubsystem.FunctionMode = VI.Scpi.SenseFunctionModes.FourWireResistance Then

            Me.Device.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Ohm

            With Me.Device.SenseFourWireResistanceSubsystem
                ' set the Integration Period
                .ApplyPowerLineCycles(VI.StatusSubsystemBase.PowerLineCycles(TimeSpan.FromMilliseconds(Me._IntegrationPeriodNumeric.Value)))

                ' set the range
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)

                If Not Me._SenseAutoRangeToggle.Checked Then
                    .ApplyRange(Me._SenseRangeNumeric.Value)
                End If
            End With

        ElseIf Me.Device.SenseSubsystem.FunctionMode = VI.Scpi.SenseFunctionModes.Resistance Then

            Me.Device.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Ohm

            With Me.Device.SenseResistanceSubsystem
                ' set the Integration Period
                .ApplyPowerLineCycles(VI.StatusSubsystemBase.PowerLineCycles(TimeSpan.FromMilliseconds(Me._IntegrationPeriodNumeric.Value)))

                ' set the range
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)

                If Not Me._SenseAutoRangeToggle.Checked Then
                    .ApplyRange(Me._SenseRangeNumeric.Value)
                End If
            End With

        ElseIf Me.Device.SenseSubsystem.FunctionMode = VI.Scpi.SenseFunctionModes.VoltageDC Then

            Me.Device.MeasureSubsystem.Readings.Reading.Unit = Arebis.StandardUnits.ElectricUnits.Volt

            With Me.Device.SenseVoltageSubsystem
                ' set the Integration Period
                .ApplyPowerLineCycles(VI.StatusSubsystemBase.PowerLineCycles(TimeSpan.FromMilliseconds(Me._IntegrationPeriodNumeric.Value)))

                ' set the range
                .ApplyAutoRangeEnabled(Me._SenseAutoRangeToggle.Checked)

                If Not Me._SenseAutoRangeToggle.Checked Then
                    .ApplyRange(Me._SenseRangeNumeric.Value)
                End If
            End With

        End If

        ' get the delay time
        If Me._TriggerDelayNumeric.Value >= 0 Then
            Me.Device.TriggerSubsystem.ApplyDelay(TimeSpan.FromTicks(CLng(TimeSpan.TicksPerSecond * Me._TriggerDelayNumeric.Value)))
        Else
            Me.Device.TriggerSubsystem.ApplyAutoDelayEnabled(True)
        End If

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
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: RESET "

    ''' <summary> Event handler. Called by _TraceInstrumentMessagesMenuItem for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
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
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _ClearInterfaceMenuItem for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
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
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
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
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
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
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
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
            Me.ReadServiceRequestStatus()
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
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleReadWriteControl_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Handles _SimpleReadWriteControl.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._SimpleReadWriteControl_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, Instrument.SimpleReadWriteControl), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling {0} property change;. Details: {1}",
                               e?.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " TRIGGER "

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyTriggerPlanButton_Click(sender As Object, e As EventArgs) Handles _ApplyTriggerPlanButton.Click
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            If Not Me.DesignMode AndAlso sender IsNot Nothing Then
                ' Me.Device.RouteSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(1))
                ' Me.Device.TriggerSubsystem.ApplyTriggerCount(CInt(Me._ChannelLayerCountNumeric.Value))
                Me.Device.TriggerSubsystem.ApplyTriggerSource(TriggerSources.External)
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred setting the trigger plan;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitiateButton_Click(sender As Object, e As EventArgs) Handles _InitiateButton.Click
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ClearExecutionState()

            ' set the service request
            Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.ServiceRequests.All And Not VI.ServiceRequests.MessageAvailable)

            ' trigger the initiation of the measurement letting the service request do the rest.
            Me.Device.TriggerSubsystem.Initiate()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ConfigureExternalScan_Click(sender As Object, e As EventArgs) Handles _ConfigureExternalScan.Click
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Me.Device.ClearExecutionState()
            Me.Device.TriggerSubsystem.ApplyContinuousEnabled(False)
            Me.Device.TriggerSubsystem.Abort()
            Me.Device.SystemSubsystem.ApplyAutoZeroEnabled(True)
            Me.Device.FormatSubsystem.ApplyElements(ReadingTypes.Reading)
            Me.Device.SenseSubsystem.ApplyFunctionMode(Scpi.SenseFunctionModes.FourWireResistance)
            Me.Device.SenseFourWireResistanceSubsystem.ApplyAverageEnabled(False)
            Me.Device.ArmLayerSubsystem.ApplyArmSource(ArmSources.Immediate)
            Me.Device.ArmLayerSubsystem.ApplyArmCount(1)
            Me.Device.ArmLayer2Subsystem.ApplyArmSource(ArmSources.Bus)
            Me.Device.ArmLayer2Subsystem.ApplyArmCount(1)
            Me.Device.ArmLayer2Subsystem.ApplyDelay(TimeSpan.Zero)
            Me.Device.TriggerSubsystem.ApplyTriggerSource(TriggerSources.External)
            Me.Device.TriggerSubsystem.ApplyTriggerCount(10)
            Me.Device.TriggerSubsystem.ApplyDelay(TimeSpan.Zero)
            Me.Device.TriggerSubsystem.ApplyDirection(Direction.Source)
            Me.StatusLabel.Text = "Ready: Initiate scanner and then meter"
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub WaitAndRead()
#If False Then
        cmd$ = ":INIT"
        GOSUB send7001

        cmd$ = ":INIT"
        GOSUB send2001
        '
        ' Arm buffer.
        '
        cmd$ = "*SRE 1"
        GOSUB send2001

        cmd$ = "STAT:MEAS:PTR 32767;NTR 0;ENAB 512"
        GOSUB send2001

        cmd$ = ":TRAC:FEED CALC"
        GOSUB send2001

        cmd$ = ":TRAC:POIN 10"
        GOSUB send2001

        cmd$ = ":TRAC:EGR COMP"
        GOSUB send2001

        cmd$ = ":FORM:DATA SREAL"
        GOSUB send2001

        cmd$ = ":TRAC:FEED:CONT NEXT;*OPC"
        GOSUB send2001

        CALL Delay(1)
        '
        ' Start filling....
        '
        t1 = TIMER
        CALL transmit("UNL LISTEN 16 GET", status%)
        '
        ' Wait for buffer to fill...
        '
        DO
           DO
           LOOP UNTIL srq%
           CALL spoll(DmmAddr%, poll%, status%)
        LOOP UNTIL (poll% AND 64)

        cmd$ = ":STAT:MEAS:EVENT?"
        GOSUB send2001

        CALL enter(Sme$, length%, 16, status%)
        '
        ' Go get buffer contents.
        '
        cmd$ = "TRACe:DATA?"
        GOSUB send2001

        DIM Readings!(1 TO 10), dummy!(1 TO 1)

        '   Setup CEC for LISTEN & 2001 for TALK
        CALL transmit("UNT UNL MLA TALK 16", status%)

        '   Read 2 bytes (#0)
        r$ = SPACE$(2)

        CALL receive(r$, l%, status%)

        NumDataPoints% = 10

        '   Get Data in 4 blocks since CEC can only handle up to 64kB
        '   per data transfer.

        NumBytes% = NumDataPoints% * 4
        CALL rarray(Readings!(1), NumBytes%, l%, status%)

        '   get Terminator Character(s)
        CALL settimeout(100)

        CALL rarray(dummy!(1), 1, l%, status%)
        CALL settimeout(10000)

        CALL transmit("UNT UNL", status%)      ' UNTalk and UNListen all devices

        t2 = TIMER
        t3 = t2 - t1

        FOR i = 1 TO 10
            PRINT "Rdg("; i; ") = "; Readings!(i)
        NEXT i

        PRINT
        PRINT "Total time = "; t3
        PRINT

        t4 = t3 / 10
        PRINT "Seconds per channel = "; t4
#End If
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
