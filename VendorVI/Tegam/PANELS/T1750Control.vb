Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Provides a user interface for the a Tegam 1750 Resistance Measuring System. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/7/2013" by="David" revision=""> Created. </history>
<System.ComponentModel.DisplayName("T1750 Control"),
      System.ComponentModel.Description("Tegam 1750 Device Control"),
      System.Drawing.ToolboxBitmap(GetType(T1750Control))>
Public Class T1750Control
    Inherits VI.Instrument.ResourceControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private Property InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(New Device)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Public Sub New(ByVal device As Device)

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

        ' populate the supported commands. 
        With Me._RangeComboBox
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(RangeMode).ValueDescriptionPairs
            .SelectedIndex = 0
            .ValueMember = "Key"
            .DisplayMember = "Value"
        End With

        ' populate the emulated reply combo.
        With Me._TriggerCombo
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(TriggerMode).ValueDescriptionPairs
            .SelectedIndex = 0
            .ValueMember = "Key"
            .DisplayMember = "Value"
        End With
        With Me._ServiceRequestEnableNumeric.NumericUpDownControl
            .Hexadecimal = True
            .Maximum = 255
            .Minimum = 0
            .Value = 0
        End With
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
                ' the device gets closed and disposed (if panel is device owner) in the base class
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    ''' <summary> Enables the controls. </summary>
    ''' <param name="control"> The control. </param>
    ''' <param name="value">   true to value. </param>
    Private Sub EnableControls(ByVal control As Windows.Forms.Control, ByVal value As Boolean)
        If control IsNot Nothing Then
            control.Enabled = value
            If control.Controls IsNot Nothing AndAlso control.Controls.Count > 0 Then
                For Each c As Windows.Forms.Control In control.Controls
                    Me.EnableControls(c, value)
                Next
            End If
        End If
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
            Case NameOf(device.SessionServiceRequestEventEnabled)
                Me._SessionServiceRequestHandlerEnabledMenuItem.Checked = device.SessionServiceRequestEventEnabled
            Case NameOf(device.DeviceServiceRequestHandlerAdded)
                Me._DeviceServiceRequestHandlerEnabledMenuItem.Checked = device.DeviceServiceRequestHandlerAdded
            Case NameOf(device.SessionMessagesTraceEnabled)
                Me._SessionTraceEnabledMenuItem.Checked = device.SessionMessagesTraceEnabled
            Case NameOf(device.ServiceRequestEnableBitmask)
                Me._ServiceRequestEnableNumeric.Value = device.ServiceRequestEnableBitmask
                Me._ServiceRequestEnableNumeric.ToolTipText = $"SRE:0b{Convert.ToString(device.ServiceRequestEnableBitmask, 2),8}".Replace(" ", "0")
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        AddHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
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
            RemoveHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " MEASURE "

    ''' <summary> handles the last reading available action. </summary>
    Private Sub OnLastReadingAvailable(ByVal subsystem As MeasureSubsystem)
        If subsystem Is Nothing Then
            Me.OnMeasurementAvailable(subsystem)
        Else
            Me._ReadingToolStripStatusLabel.Text = subsystem.LastReading
            Me._ComplianceToolStripStatusLabel.Text = "  "
            Windows.Forms.Application.DoEvents()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Measure message: {0}.", subsystem.LastReading.InsertCommonEscapeSequences)
        End If
    End Sub

    ''' <summary> Handles the measurement available action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnMeasurementAvailable(ByVal subsystem As MeasureSubsystem)
        If subsystem Is Nothing Then
            Me._ReadingToolStripStatusLabel.Text = "-.---- Ohm"
            Me._ComplianceToolStripStatusLabel.Text = "  "
        ElseIf Me.Device.MeasureSubsystem.MeasurementAvailable AndAlso Me._ReadContinuouslyCheckBox.Checked Then
            Windows.Forms.Application.DoEvents()
            Me.Device.StatusSubsystem.ReadEventRegisters()
            Windows.Forms.Application.DoEvents()
            If Not Me.Device.StatusSubsystem.ErrorAvailable Then
                If Me._PostReadingDelayNumeric.Value > 0 Then
                    Dim startTime As DateTime = DateTime.Now
                    Do
                        Windows.Forms.Application.DoEvents()
                    Loop Until DateTime.Now.Subtract(startTime).TotalMilliseconds > Me._PostReadingDelayNumeric.Value
                End If
                Windows.Forms.Application.DoEvents()
                Me.Device.MeasureSubsystem.Read(False)
            End If
        ElseIf Me.Device.MeasureSubsystem.OverRangeOpenWire.GetValueOrDefault(False) Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Measurement over range or open wire detected;. ")
            Me._ReadingToolStripStatusLabel.Text = Me.Device.MeasureSubsystem.LastReading
            Me._ComplianceToolStripStatusLabel.Text = "O.R."
            Windows.Forms.Application.DoEvents()
        End If
    End Sub

    ''' <summary> Handles the measurement available action. </summary>
    <Obsolete("replaced with measurementAvailable(MeasureSubsystem)")>
    Private Sub OnMeasurementAvailable()
        If Me.Device.MeasureSubsystem.MeasurementAvailable AndAlso Me._ReadContinuouslyCheckBox.Checked Then
            Windows.Forms.Application.DoEvents()
            Me.Device.StatusSubsystem.ReadEventRegisters()
            Windows.Forms.Application.DoEvents()
            If Not Me.Device.StatusSubsystem.ErrorAvailable Then
                If Me._PostReadingDelayNumeric.Value > 0 Then
                    Dim startTime As DateTime = DateTime.Now
                    Do
                        Windows.Forms.Application.DoEvents()
                    Loop Until DateTime.Now.Subtract(startTime).TotalMilliseconds > Me._PostReadingDelayNumeric.Value
                End If
                Windows.Forms.Application.DoEvents()
                Me.Device.MeasureSubsystem.Read(False)
            End If
        End If
    End Sub

    ''' <summary> Executes the over range open wire action. </summary>
    <Obsolete("replaced with measurementAvailable(MeasureSubsystem)")>
    Private Sub OnOverRangeOpenWire()
        If Me.Device.MeasureSubsystem.OverRangeOpenWire.GetValueOrDefault(False) Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Measurement over range or open wire detected;. ")
            Me._ReadingToolStripStatusLabel.Text = Me.Device.MeasureSubsystem.LastReading
            Me._ComplianceToolStripStatusLabel.Text = "O.R."
            Windows.Forms.Application.DoEvents()
        End If
    End Sub

    ''' <summary> Handles the supported commands changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnSupportedCommandsChanged(ByVal subsystem As MeasureSubsystem)
        If subsystem IsNot Nothing Then
            With Me._CommandComboBox
                .DataSource = Nothing
                .Items.Clear()
                .DataSource = subsystem.SupportedCommands
                .SelectedIndex = 0
            End With
        End If
    End Sub

    ''' <summary> Updates the display of measurement settings. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub OnMeasureSettingsChanged(ByVal subsystem As MeasureSubsystem)
        If subsystem IsNot Nothing Then
            Me._measureSettingsLabel.Text = String.Format(Globalization.CultureInfo.CurrentCulture,
                                                          "Trials: {0}; Initial Delay: {1} ms; Measurement Delay: {2} ms; Delta: {3:0.0%}",
                                                          subsystem.MaximumTrialsCount, subsystem.InitialDelay.TotalMilliseconds,
                                                          subsystem.MeasurementDelay.TotalMilliseconds, subsystem.MaximumDifference)
        End If
    End Sub

    ''' <summary> Handles the Measure subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As MeasureSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.MaximumTrialsCount), NameOf(subsystem.InitialDelay),
                 NameOf(subsystem.MeasurementDelay), NameOf(subsystem.MaximumDifference)
                Me.OnMeasureSettingsChanged(subsystem)
            Case NameOf(subsystem.LastReading)
                Me.OnLastReadingAvailable(subsystem)
            Case NameOf(subsystem.OverRangeOpenWire)
                ' Me.onOverRangeOpenWire()
                Me.OnMeasurementAvailable(subsystem)
            Case NameOf(subsystem.RangeMode)
                If subsystem.RangeMode.HasValue Then
                    Me._RangeComboBox.SafeSilentSelectItem(subsystem.RangeMode.Value.Description)
                    Me._RangeComboBox.Refresh()
                End If
            Case NameOf(subsystem.Resistance)
                If subsystem.Resistance.HasValue AndAlso Not subsystem?.OverRangeOpenWire.GetValueOrDefault(False) Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                       "Parsed resistance value;. Resistance = {0}", subsystem.Resistance.Value)
                End If
            Case NameOf(subsystem.TriggerMode)
                If subsystem.TriggerMode.HasValue Then
                    Me._TriggerCombo.SafeSilentSelectItem(subsystem.TriggerMode.Value.Description)
                    Me._TriggerCombo.Refresh()
                End If
            Case NameOf(subsystem.MeasurementAvailable)
                ' Me.onMeasurementAvailable()
                Me.OnMeasurementAvailable(subsystem)
            Case NameOf(subsystem.SupportedCommands)
                Me.OnSupportedCommandsChanged(subsystem)
        End Select
        Windows.Forms.Application.DoEvents()
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
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Protected Overrides Sub OnLastError(ByVal lastError As isr.VI.DeviceError)
        If lastError IsNot Nothing Then
            Me._LastErrorTextBox.ForeColor = If(lastError.IsError, Drawing.Color.OrangeRed, Drawing.Color.Aquamarine)
            Me._LastErrorTextBox.Text = lastError.CompoundErrorMessage
        End If
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
                If Not subsystem.ReadingDeviceErrors Then
                    ' if no errors, this clears the error queue.
                    subsystem.QueryDeviceErrors()
                End If
            Case NameOf(subsystem.ServiceRequestStatus)
                Me._StatusRegisterLabel.Text = $"0x{subsystem.ServiceRequestStatus:X2}"
            Case NameOf(subsystem.StandardEventStatus)
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
        Select Case propertyName
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
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

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

#End Region

#Region " CONTROL EVENT HANDLERS: READ "

    ''' <summary>Initiates a reading for retrieval by way of the service request event.</summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadSRQButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ReadSRQButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            If Me.IsDeviceOpen Then
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading registers;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>Selects a new reading to display.</summary>
    Private Sub _RangeComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _RangeComboBox.SelectedIndexChanged
        If Me.IsDeviceOpen Then
            If Me._RangeComboBox.Enabled AndAlso Me._RangeComboBox.SelectedIndex >= 0 AndAlso
                    Not String.IsNullOrWhiteSpace(Me._RangeComboBox.Text) Then
            End If
        End If
    End Sub

    ''' <summary>Query the Device for a reading.</summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ReadButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If Me.IsDeviceOpen Then
                ' update display modalities if changed.
                Me.Device.MeasureSubsystem.Read(False)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _ConfigureButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ConfigureButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ConfigureButton.Click
        If Me.IsDeviceOpen Then
            Dim range As RangeMode = RangeMode.R0
            Dim trigger As TriggerMode = TriggerMode.T0
            Try
                Me._InfoProvider.Clear()
                Me.Cursor = Cursors.WaitCursor
                range = CType([Enum].Parse(GetType(RangeMode), Me._RangeComboBox.SelectedValue.ToString), RangeMode)
                Me.Device.MeasureSubsystem.ApplyRangeMode(range)
                trigger = CType([Enum].Parse(GetType(TriggerMode), Me._TriggerCombo.SelectedValue.ToString), TriggerMode)
                Me.Device.MeasureSubsystem.ApplyTriggerMode(trigger)
                Me.Device.StatusSubsystem.ReadEventRegisters()
            Catch ex As Exception
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   "Exception configuring;. Range {0} Trigger {1}. {2}.", range, trigger, ex.ToFullBlownString)
                Me._InfoProvider.Annunciate(sender, ex.ToString)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    ''' <summary> Event handler. Called by _WriteButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _WriteButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _WriteButton.Click
        If Not String.IsNullOrWhiteSpace(Me._CommandComboBox.Text) Then
            Dim dataToWrite As String = Me._CommandComboBox.Text.Trim
            Try
                Me._InfoProvider.Clear()
                Me.Cursor = Cursors.WaitCursor
                If dataToWrite.StartsWith("U", StringComparison.OrdinalIgnoreCase) Then
                    Me.Device.MeasureSubsystem.LastReading = Me.Device.Session.QueryTrimEnd(dataToWrite)
                Else
                    Me.Device.Session.WriteLine(dataToWrite)
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            Catch ex As Exception
                Me._InfoProvider.Annunciate(sender, ex.ToString)
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   "Exception occurred sending message;. '{0}'. {1}", dataToWrite, ex.ToFullBlownString)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    ''' <summary> Event handler. Called by _MeasureButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MeasureButton_Click(sender As System.Object, e As System.EventArgs) Handles _MeasureButton.Click
        If Not String.IsNullOrWhiteSpace(Me._CommandComboBox.Text) Then
            Try
                Me._InfoProvider.Clear()
                Me.Cursor = Cursors.WaitCursor
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Started measuring;. ")
                Me.Device.MeasureSubsystem.Measure()
            Catch ex As Exception
                Me._InfoProvider.Annunciate(sender, ex.ToString)
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred measuring;. ")
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If

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
    Private Sub _ClearDeviceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearDeviceMenuItem.Click
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
                    If Me._ServiceRequestEnableNumeric.Value = 0 Then
                        Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
                    Else
                        Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableNumeric.Value, ServiceRequests))
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
                    Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusMessage, Me._StatusLabel)
                    Me._StatusLabel.ToolTipText = sender.StatusMessage
                Case NameOf(sender.ServiceRequestValue)
                    Me._StatusRegisterLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
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
        Me._SimpleReadWriteControl?.ApplyListenerTraceLevel(listenerType, value)
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
