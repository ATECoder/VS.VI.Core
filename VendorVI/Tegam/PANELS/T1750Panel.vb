Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
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
<System.ComponentModel.DisplayName("T1750 Panel"),
      System.ComponentModel.Description("Tegam 1750 Device Panel"),
      System.Drawing.ToolboxBitmap(GetType(T1750Panel))>
Public Class T1750Panel
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
    ''' <remarks> David, 12/22/2015. </remarks>
    ''' <param name="control"> The control. </param>
    ''' <param name="value">   true to value. </param>
    Private Sub enableControls(ByVal control As Windows.Forms.Control, ByVal value As Boolean)
        If control IsNot Nothing Then
            control.Enabled = value
            If control.Controls IsNot Nothing AndAlso control.Controls.Count > 0 Then
                For Each c As Windows.Forms.Control In control.Controls
                    Me.enableControls(c, value)
                Next
            End If
        End If
    End Sub

#End Region

#Region " FORM EVENTS "

    ''' <summary> Handles the <see cref="E:System.Windows.Forms.UserControl.Load" /> event. </summary>
    ''' <remarks> David, 1/4/2016. </remarks>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Try
            Me.TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
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

    ''' <summary> Gets a reference to the T1750 Device. </summary>
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
                ' Me._HandleServiceRequestsCheckBox.Checked = device.IsServiceRequestEventEnabled
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
            RemoveHandler Me.Device.MeasureSubsystem.PropertyChanged, AddressOf Me.MeasureSubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " MEASURE "

    ''' <summary> handles the last reading available action. </summary>
    Private Sub onLastReadingAvailable(ByVal subsystem As MeasureSubsystem)
        If subsystem Is Nothing Then
            Me.onMeasurementAvailable(subsystem)
        Else
            Me._ReadingToolStripStatusLabel.Text = subsystem.LastReading
            Me._ComplianceToolStripStatusLabel.Text = "  "
            Me._TbdToolStripStatusLabel.Text = "  "
            Windows.Forms.Application.DoEvents()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Measure message: {0}.", subsystem.LastReading.InsertCommonEscapeSequences)
        End If
    End Sub

    ''' <summary> Handles the measurement available action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub onMeasurementAvailable(ByVal subsystem As MeasureSubsystem)
        If subsystem Is Nothing Then
            Me._ReadingToolStripStatusLabel.Text = "-.---- Ohm"
            Me._ComplianceToolStripStatusLabel.Text = "  "
            Me._TbdToolStripStatusLabel.Text = "  "
        ElseIf Me.Device.MeasureSubsystem.MeasurementAvailable AndAlso Me._ReadContinuouslyCheckBox.Checked Then
            Windows.Forms.Application.DoEvents()
            Me.Device.StatusSubsystem.ReadRegisters()
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
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Measurement over range or open wire detected;. ")
            Me._ReadingToolStripStatusLabel.Text = Me.Device.MeasureSubsystem.LastReading
            Me._ComplianceToolStripStatusLabel.Text = "O.R."
            Windows.Forms.Application.DoEvents()
        End If
    End Sub

    ''' <summary> Handles the measurement available action. </summary>
    <Obsolete("replaced with measurementAvailable(MeasureSubsystem)")>
    Private Sub onMeasurementAvailable()
        If Me.Device.MeasureSubsystem.MeasurementAvailable AndAlso Me._ReadContinuouslyCheckBox.Checked Then
            Windows.Forms.Application.DoEvents()
            Me.Device.StatusSubsystem.ReadRegisters()
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
    Private Sub onOverRangeOpenWire()
        If Me.Device.MeasureSubsystem.OverRangeOpenWire.GetValueOrDefault(False) Then
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Measurement over range or open wire detected;. ")
            Me._ReadingToolStripStatusLabel.Text = Me.Device.MeasureSubsystem.LastReading
            Me._ComplianceToolStripStatusLabel.Text = "O.R."
            Windows.Forms.Application.DoEvents()
        End If
    End Sub

    ''' <summary> Handles the supported commands changed action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub onSupportedCommandsChanged(ByVal subsystem As MeasureSubsystem)
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
    Private Sub onMeasureSettingsChanged(ByVal subsystem As MeasureSubsystem)
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
                Me.onMeasureSettingsChanged(subsystem)
            Case NameOf(subsystem.LastReading)
                Me.onLastReadingAvailable(subsystem)
            Case NameOf(subsystem.OverRangeOpenWire)
                ' Me.onOverRangeOpenWire()
                Me.onMeasurementAvailable(subsystem)
            Case NameOf(subsystem.RangeMode)
                If subsystem.RangeMode.HasValue Then
                    Me._RangeComboBox.SafeSilentSelectItem(subsystem.RangeMode.Value.Description)
                    Me._RangeComboBox.Refresh()
                End If
            Case NameOf(subsystem.Resistance)
                If subsystem.Resistance.HasValue AndAlso Not subsystem?.OverRangeOpenWire.GetValueOrDefault(False) Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                       "Parsed resistance value;. Resistance = {0}", subsystem.Resistance.Value)
                End If
            Case NameOf(subsystem.TriggerMode)
                If subsystem.TriggerMode.HasValue Then
                    Me._TriggerCombo.SafeSilentSelectItem(subsystem.TriggerMode.Value.Description)
                    Me._TriggerCombo.Refresh()
                End If
            Case NameOf(subsystem.MeasurementAvailable)
                ' Me.onMeasurementAvailable()
                Me.onMeasurementAvailable(subsystem)
            Case NameOf(subsystem.SupportedCommands)
                Me.onSupportedCommandsChanged(subsystem)
        End Select
        Windows.Forms.Application.DoEvents()
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
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Private Sub onLastError(ByVal lastError As VI.DeviceError)
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
            Case NameOf(subsystem.ErrorAvailable)
                ' if no errors, this clears the error queue.
                subsystem.QueryLastError()
            Case NameOf(subsystem.DeviceErrors)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.LastDeviceError)
                onLastError(subsystem.LastDeviceError)
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

#End Region

#Region " CONTROL EVENT HANDLERS: RESET "

    ''' <summary> Event handler. Called by _SessionTraceEnableCheckBox for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _SessionTraceEnableCheckBox_CheckedChanged(ByVal sender As Object, e As System.EventArgs) Handles _SessionTraceEnableCheckBox.CheckedChanged
        If Me._InitializingComponents Then Return
        If Not Me.DesignMode AndAlso sender IsNot Nothing Then
            Dim checkBox As Windows.Forms.CheckBox = CType(sender, Windows.Forms.CheckBox)
            Me.Device.SessionMessagesTraceEnabled = checkBox.Checked
        End If
    End Sub

    ''' <summary> Event handler. Called by interfaceClearButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InterfaceClearButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InterfaceClearButton.Click
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

    ''' <summary> Event handler. Called by _SelectiveDeviceClearButton for click events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SelectiveDeviceClearButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SelectiveDeviceClearButton.Click
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

    ''' <summary> Issue RST. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ResetButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResetButton.Click
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

    ''' <summary> Event handler. Called by _InitializeKnownStateButton for click events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitializeKnownStateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InitializeKnownStateButton.Click
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

#End Region

#Region " CONTROL EVENT HANDLERS "

    ''' <summary>Initiates a reading for retrieval by way of the service request event.</summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadSRQButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ReadSRQButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            If Me.IsDeviceOpen Then
                Me.Device.StatusSubsystem.ReadRegisters()
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading registers;. Details: {0}.", ex)
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
            Me.ErrorProvider.Clear()
            If Me.IsDeviceOpen Then
                ' update display modalities if changed.
                Me.Device.MeasureSubsystem.Read(False)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading;. Details: {0}.", ex)
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
                Me.ErrorProvider.Clear()
                Me.Cursor = Cursors.WaitCursor
                range = CType([Enum].Parse(GetType(RangeMode), Me._RangeComboBox.SelectedValue.ToString), RangeMode)
                Me.Device.MeasureSubsystem.ApplyRangeMode(range)
                trigger = CType([Enum].Parse(GetType(TriggerMode), Me._TriggerCombo.SelectedValue.ToString), TriggerMode)
                Me.Device.MeasureSubsystem.ApplyTriggerMode(trigger)
                Me.Device.StatusSubsystem.ReadRegisters()
            Catch ex As Exception
                Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   "Exception configuring;. Range {0} Trigger {1}. Details: {2}.", range, trigger, ex)
                Me.ErrorProvider.Annunciate(sender, ex.ToString)
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
                Me.ErrorProvider.Clear()
                Me.Cursor = Cursors.WaitCursor
                If dataToWrite.StartsWith("U", StringComparison.OrdinalIgnoreCase) Then
                    Me.Device.MeasureSubsystem.LastReading = Me.Device.Session.QueryTrimEnd(dataToWrite)
                Else
                    Me.Device.Session.WriteLine(dataToWrite)
                End If
                Me.Device.StatusSubsystem.ReadRegisters()
            Catch ex As Exception
                Me.ErrorProvider.Annunciate(sender, ex.ToString)
                Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   "Exception occurred sending message;. '{0}'. Details: {1}", dataToWrite, ex)
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
                Me.ErrorProvider.Clear()
                Me.Cursor = Cursors.WaitCursor
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Started measuring;. ")
                Me.Device.MeasureSubsystem.Measure()
            Catch ex As Exception
                Me.ErrorProvider.Annunciate(sender, ex.ToString)
                Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred measuring;. ")
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If

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
