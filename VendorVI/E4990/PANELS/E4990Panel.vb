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
''' <summary> Provides a user interface for the Keysight E4990 Device. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="03/25/2016" by="David" revision="4.0.5928.x"> Create based on the 24xx and 2700
''' system classes. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
<System.ComponentModel.DisplayName("E4990 Panel"),
      System.ComponentModel.Description("Keysight E4990 Device Panel"),
      System.Drawing.ToolboxBitmap(GetType(E4990Panel))>
Public Class E4990Panel
    Inherits VI.Instrument.ResourcePanelBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private Property InitializingComponents As Boolean
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
                Try
                    If Me.Device IsNot Nothing Then Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", "Exception details: {0}", ex)
                End Try
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

    ''' <summary> Gets a reference to the Keysight E4990 Device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overloads ReadOnly Property Device() As Device

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Gets the session. </summary>
    ''' <value> The session. </value>
    Private ReadOnly Property Session As VI.SessionBase
        Get
            Return Me.Device.Session
        End Get
    End Property

    ''' <summary> Executes the device open changed action. </summary>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As DeviceBase)
        Dim isOpen As Boolean = CType(device?.IsDeviceOpen, Boolean?).GetValueOrDefault(False)
        If isOpen Then
            Me._SimpleReadWriteControl.Connect(device?.Session)
            ' Me._SimpleReadWriteControl.ReadEnabled = True
        Else
            Me._SimpleReadWriteControl.Disconnect()
        End If
        ' enable the tabs even if the device failed to open.
        ' Me._Tabs.Enabled = True
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
                Me._ServiceRequestsHandlerEnabledMenuItem.SafeCheckedSetter(device.IsServiceRequestEventEnabled)
            Case NameOf(device.SessionMessagesTraceEnabled)
                Me._SessionTraceEnabledMenuItem.SafeCheckedSetter(device.SessionMessagesTraceEnabled)
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
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
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

    ''' <summary> Event handler. Called by _HandleServiceRequestsCheckBox for check state changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ServiceRequestsHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ServiceRequestsHandlerEnabledMenuItem.CheckStateChanged
        If Me._InitializingComponents Then Return
        Dim checkBox As CheckBox = TryCast(sender, CheckBox)
        Try
            Me.Cursor = Cursors.WaitCursor
            If checkBox IsNot Nothing AndAlso Not checkBox.Checked = Me.Device.IsServiceRequestEventEnabled Then
                If checkBox IsNot Nothing AndAlso checkBox.Checked Then
                    Me.EnableServiceRequestEventHandler()
                    Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
                Else
                    Me.Device.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)
                    Me.DisableServiceRequestEventHandler()
                End If
                Me.Device.StatusSubsystem.ReadRegisters()
            End If
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Failed toggling service request")
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred toggling service request;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " SUBSYSTEMS "

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

    ''' <summary> Contact check enabled menu item check state changed. </summary>
    ''' <remarks> David, 3/7/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId:="menuItem")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ContactCheckEnabledMenuItem_CheckStateChanged(sender As Object, e As EventArgs) Handles _ContactCheckEnabledMenuItem.CheckStateChanged
        If Me._InitializingComponents Then Return
        Me.ErrorProvider.Clear()
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, "Exception occurred enabling contact check")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred enabling contact check;. Details: {0}", ex)
        End Try
    End Sub

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Application.DoEvents()
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
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionTraceEnabledMenuItem_CheckedChanged(ByVal sender As Object, e As System.EventArgs) Handles _SessionTraceEnabledMenuItem.CheckedChanged
        If Me._InitializingComponents Then Return
        Dim checkBox As Windows.Forms.CheckBox = CType(sender, Windows.Forms.CheckBox)
        Try
            Me.Cursor = Cursors.WaitCursor
            If checkBox.Enabled Then
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

    ''' <summary> Event handler. Called by interfaceClearButton for click events. </summary>
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
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred clearing interface;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _SelectiveDeviceClearButton for click events. </summary>
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
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred sending SDC;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Issue RST. </summary>
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
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred resetting known state;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _InitializeKnownStateButton for click events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitializeKnowStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InitializeKnowStateMenuItem.Click
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
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initializing known state;. Details: {0}", ex)
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
            ' Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.ServiceRequests.All And Not VI.ServiceRequests.MessageAvailable)

            ' trigger the initiation of the measurement letting the service request do the rest.
            Me.Device.ClearExecutionState()
            ' Me.Device.TriggerSubsystem.Initiate()

        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Reading combo box selected value changed. </summary>
    ''' <remarks> David, 3/17/2016. </remarks>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles _ReadingComboBox.SelectedValueChanged
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            'Me.Device.MeasureSubsystem.Readings.ActiveReadingType = Me.SelectedReadingType
            'Me.DisplayActiveReading()
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
            Dim caption As String = ""
            Me.Device.ClearExecutionState()
            Me.EnableMeasurementAvailable()
            Me.Device.Session.WriteLine(":TRIG")
            If Me.Device.StatusSubsystem.TryAwaitServiceRequest(ServiceRequests.RequestingService, TimeSpan.FromSeconds(10),
                                                                TimeSpan.FromMilliseconds(100)) Then
                ' auto scale after measurement completes
                Me.AutoScale(1, 1)
                Me.AutoScale(1, 2)
                Me.SelectActiveTrace(1, 1)
                caption = Me.ReadMarker(1, 1)
                Me._ReadingToolStripStatusLabel.SafeTextSetter(caption)
            Else
                Me.ErrorProvider.Annunciate(sender, "timeout")
            End If
            'Me.Device.MeasureSubsystem.Read()
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred initiating a measurement;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " EXAMPLES "

    Public Sub ConfigureKeysightExample()

        Me.Device.Session.StoreTimeout(TimeSpan.FromSeconds(10))

        ' Preset the equipment to its known state.
        Me.Device.SystemSubsystem.PresetKnownState()

        Me.Device.Session.RestoreTimeout()

        ' DISPLAY: Set two channels
        Me.Device.Session.Write(":DISP:SPL D1_2")

        ' Set trigger source at BUS
        Me.Device.Session.Write(":TRIG:SOUR BUS")

        ' Setup Channel 1

        ' Set measurement parameter for trace 1
        Me.Device.Session.Write(":CALC1:PAR1:DEF Z")

        ' Set measurement parameter for trace 2
        Me.Device.Session.Write(":CALC1:PAR2:DEF TZ")

        ' Set Y-Axis at Log format
        Me.Device.Session.Write(":DISP:WIND1:TRAC1:Y:SPAC LOG")

        ' Stimulus Setup

        ' Turn on Continuous Activation mode for channel 1
        Me.Device.Session.Write(":INIT1:CONT ON")

        ' Set sweep type at LOG
        Me.Device.Session.Write(":SENS1:SWE:TYPE LOG")

        ' Set number of point
        Me.Device.Session.Write(":SENS1:SWE:POIN 201")

        ' Set start frequency
        Me.Device.Session.Write(":SENS1:FREQ:STAR 100E3")

        ' Set stop frequency
        Me.Device.Session.Write(":SENS1:FREQ:STOP 10E6")

        ' Set OSC mode
        Me.Device.Session.Write(":SOUR1:MODE VOLT")

        ' Set OSC level
        Me.Device.Session.Write(":SOUR1:VOLT 300E-3")

        ' Turn on ALC
        Me.Device.Session.Write(":SOUR1:ALC ON")

        ' Setup Channel 2

        ' Set measurement parameter for trace 1
        Me.Device.Session.Write(":CALC2:PAR1:DEF CS")

        ' Set measurement parameter for trace 2
        Me.Device.Session.Write(":CALC2:PAR2:DEF Q")

        ' Split the trace windows
        Me.Device.Session.Write(":DISP:WIND2:SPL D1_2")

        ' Stimulus Setup Channel 2

        ' Turn on Continuous Activation mode for channel 2
        Me.Device.Session.Write(":INIT2:CONT ON")

        ' Set sweep type at segment sweep
        Me.Device.Session.Write(":SENS2:SWE:TYPE SEGM")

        ' Set segment display at freq base
        Me.Device.Session.Write(":DISP:WIND2:X:SPAC LIN")

        Dim SegFmt As String = "7,0,1,0,0,0,0,0,3,"
        Dim SegNo1 As String = "1E4,1E5,50,0,0.3," ' Start Freq, Stop Freq, Nop, Voltage Type, OSC level
        Dim SegNo2 As String = "1E5,1E6,200,0,0.5,"
        Dim segNo3 As String = "1E6,1E7,50,0,0.3"

        ' Set sweep type at LOG"
        Me.Device.Session.Write(String.Format(":SENS2:SEGM:DATA {0}{1}{2}{3}", SegFmt, SegNo1, SegNo2, segNo3))

#If False Then
    ' Save setting into state file
    ' Save settings to file
    Me.Device.Session.Write(":MMEM:STOR ""D:\State\Test.sta""")  
   
   ' Close IO
    Analyzer.IO.Close
#End If

    End Sub

    Public Sub DefineShortTermination(ByVal resistance As Double, ByVal inductance As Double)

        ' Define Short termination by equivalent circuit model

        ' Set equivalent circuit model for short
        Me.Device.Session.Write(":SENS1:CORR2:CKIT:SHOR:MOD EQU")

        ' Set short termination parameter (L)
        Me.Device.Session.Write(String.Format(":SENS1:CORR2:CKIT:SHOR:L {0}", inductance))

        ' Set short termination parameter (R)  
        Me.Device.Session.Write(String.Format(":SENS1:CORR2:CKIT:SHOR:R {0}", resistance))

    End Sub

#End Region

#Region " Z: IMPEDANCE "

    ''' <summary> Enables the marker. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="channelNumber"> The channel number. </param>
    ''' <param name="markerNumber">  The marker number. </param>
    ''' <param name="frequency">     The frequency. </param>
    Public Sub EnableMarker(ByVal channelNumber As Integer, ByVal markerNumber As Integer, ByVal frequency As Double)

        ' Turn on marker 1
        Me.Device.Session.Write(String.Format(Globalization.CultureInfo.InvariantCulture,
                                              ":CALC{0}:MARK{1} ON", channelNumber, markerNumber))

        ' set marker position to 100 KHz
        Me.Device.Session.Write(String.Format(Globalization.CultureInfo.InvariantCulture,
                                              ":CALC{0}:MARK{1}:X {2}", channelNumber, markerNumber, frequency))
    End Sub

    ''' <summary> Select active trace. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="channelNumber"> The channel number. </param>
    ''' <param name="traceNumber">   The trace number. </param>
    Public Sub SelectActiveTrace(ByVal channelNumber As Integer, ByVal traceNumber As Integer)
        Me.Device.Session.Write(String.Format(Globalization.CultureInfo.InvariantCulture,
                                              ":CALC{0}:PAR{1}:SEL", channelNumber, traceNumber))
    End Sub

    ''' <summary> Reads a marker. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="channelNumber"> The channel number. </param>
    ''' <param name="markerNumber">  The marker number. </param>
    ''' <returns> The marker. </returns>
    Public Function ReadMarker(ByVal channelNumber As Integer, ByVal markerNumber As Integer) As String
        Dim result As String = ""
        result = Me.Device.Session.QueryTrimEnd(":CALC{0}:MARK{1}:Y?", channelNumber, markerNumber)
        ' marker returns two values.
        result = result.Split(","c)(0)
        Return result
    End Function

    ''' <summary> Automatic scale. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="channelNumber"> The channel number. </param>
    ''' <param name="traceNumber">   The trace number. </param>
    Public Sub AutoScale(ByVal channelNumber As Integer, ByVal traceNumber As Integer)
        Me.Device.Session.WriteLine(":DISP:WIND{0}:TRAC{1}:Y:SCAL:AUTO", channelNumber, traceNumber)
    End Sub

    ''' <summary> Configure sweep. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="lowFrequency">  The low frequency. </param>
    ''' <param name="highFrequency"> The high frequency. </param>
    Public Sub ConfigureSweep(ByVal lowFrequency As Double, ByVal highFrequency As Double)

        ' Set number of points
        Me.Device.Session.Write(":SENS1:SWE:POIN 2")

        ' Set start frequency
        Me.Device.Session.WriteLine(":SENS1:FREQ:STAR {0}", lowFrequency)

        ' Set stop frequency
        Me.Device.Session.WriteLine(":SENS1:FREQ:STOP {0}", highFrequency)

    End Sub

    ''' <summary> Configure impedance measurement. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="sourceMode">    Source mode. </param>
    ''' <param name="aperture">      The aperture. </param>
    ''' <param name="level">         The level. </param>
    ''' <param name="lowFrequency">  The low frequency. </param>
    ''' <param name="highFrequency"> The high frequency. </param>
    Public Sub ConfigureImpedanceMeasurement(ByVal sourceMode As SourceFunctionModes, aperture As Integer, ByVal level As Double,
                                             ByVal lowFrequency As Double, ByVal highFrequency As Double)

        Me.Device.Session.StoreTimeout(TimeSpan.FromSeconds(10))

        ' Preset the equipment to its known state.
        Me.Device.SystemSubsystem.PresetKnownState()

        ' Clear the error queue
        Me.Device.ClearExecutionState()

        ' clear the device display from warnings
        Me.Device.Session.Write(":DISP:CCL")

        Me.Device.Session.RestoreTimeout()

        ' Set trigger source at BUS
        Me.Device.Session.Write(":TRIG:SOUR BUS")

        ' Setup Channel 1

        ' Allocate measurement parameter for trace 1: Rs
        Me.Device.Session.Write(":CALC1:PAR1:DEF RS")

        ' Allocate measurement parameter for trace 2: Ls
        Me.Device.Session.Write(":CALC1:PAR2:DEF LS")

        ' Stimulus Setup

        ' Turn on Continuous Activation mode for channel 1
        Me.Device.Session.Write(":INIT1:CONT ON")

        ' Set aperture
        Me.Device.Session.WriteLine(":SENS1:APER {0}", aperture)

        ' set a two point sweep.
        Me.ConfigureSweep(lowFrequency, highFrequency)

        ' Set OSC mode
        If sourceMode = SourceFunctionModes.Current Then
            Me.Device.Session.Write(":SOUR1:MODE CURR")

            ' Set OSC level
            Me.Device.Session.WriteLine(":SOUR1:CURR {0}", level)
        Else
            Me.Device.Session.Write(":SOUR1:MODE VOLT")

            ' Set OSC level
            Me.Device.Session.WriteLine(":SOUR1:VOLT {0}", level)
        End If

        ' MARKERS

        ' Turn on marker 1
        Me.Device.Session.Write(":CALC1:MARK1 ON")

        ' set first marker position
        Me.Device.Session.WriteLine(":CALC1:MARK1:X {0}", lowFrequency)

        ' Turn on marker 2
        Me.Device.Session.Write(":CALC1:MARK2 ON")

        ' set marker 2 position 
        Me.Device.Session.WriteLine(":CALC1:MARK2:X {0}", highFrequency)

    End Sub

    ''' <summary> Enables the measurement available. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    Public Sub EnableMeasurementAvailable()

        ' Sets the positive transition filter to 0 and the negative transition filter to 1 
        ' so that the operation status event register at bit 4 is set to 1 only when the
        ' operation status condition register at bit 4 is changed from 1 to 0.
        Me.Device.Session.Write(":STAT:OPER:PTR 0")
        Me.Device.Session.Write(":STAT:OPER:NTR 16")

        ' Enables bit 4 in the operation status event register and bit 8 in the status byte register.
        Me.Device.Session.Write(":STAT:OPER:ENAB 16")
        Me.Device.Session.Write("*SRE 128")

    End Sub

    ''' <summary> Applies the impedance settings button click. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyImpedanceSettingsButton_Click(sender As Object, e As EventArgs) Handles _ApplyImpedanceSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.ErrorProvider.Clear()
            Dim mode As SourceFunctionModes = If(_SourceToggle.Checked, SourceFunctionModes.Voltage, SourceFunctionModes.Current)
            Dim level As Double = Me._LevelNumeric.Value
            If mode = SourceFunctionModes.Current Then level = 0.001 * level
            Me.ConfigureImpedanceMeasurement(mode, CInt(Me._ApertureNumeric.Value), level,
                                             Me._LowFrequencyNumeric.Value, Me._HighFrequencyNumeric.Value)
        Catch ex As Exception
            Me.ErrorProvider.Annunciate(sender, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred configuring a measurement;. Details: {0}", ex)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Source toggle checked changed. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _SourceToggle_CheckedChanged(sender As Object, e As EventArgs) Handles _SourceToggle.CheckedChanged
        If Me.InitializingComponents Then Return
        _SourceToggle.Text = If(_SourceToggle.Checked, "Source: Voltage", "Source: Current")
        With Me._LevelNumericLabel
            If Me._SourceToggle.Checked Then
                .Text = "Level [V]:"
            Else
                .Text = "Level [mA]:"
            End If
            .Left = Me._ApertureNumericLabel.Right - .Width
        End With
        With Me._LevelNumeric
            If Me._SourceToggle.Checked Then
                .Minimum = 0.001D
                .Maximum = 1D
                .Value = 0.5D
            Else
                .Minimum = 0.1D
                .Maximum = 10D
                .Value = 5D
            End If
        End With

    End Sub

#End Region

#Region " COMPENSATION "

    ''' <summary> Adapter setup. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    Sub AdapterSetup()

        ' TimeOut time should be greater than the measurement time.    
        Me.Device.Session.StoreTimeout(TimeSpan.FromSeconds(10))

        ' Select the adapter
        ' Set adapter type to 42942A
        Me.Device.Session.Write(":SENS1:ADAP:TYPE E4PR")

        ' Phase setup
        MessageBox.Show("Connect Open Termination")
        ' Execute open in phase setup
        Me.Device.Session.Write(":SENS1:ADAP:CORR:COLL:ACQ PHAS")
        ' Wait for measurement end
        Me.Device.Session.Query("*OPC?")

        ' Save phase setup data
        Me.Device.Session.Write(":SENS1:ADAP:CORR:COLL:SAVE PHAS")
        MessageBox.Show("Phase Setup Done")

        ' Impedance setup
        MessageBox.Show("Connect Open Termination")

        ' Execute open in impedance setup
        Me.Device.Session.Write(":SENS1:ADAP:CORR:COLL:ACQ OPEN")

        ' Wait for measurement end
        Me.Device.Session.Query("*OPC?")

        MessageBox.Show("Connect Short Termination")

        ' Execute short in impedance setup
        Me.Device.Session.Write(":SENS1:ADAP:CORR:COLL:ACQ SHOR")

        ' Wait for measurement end
        Me.Device.Session.Query("*OPC?")

        MessageBox.Show("Connect LOAD Termination")

        ' Execute load in impedance setup
        Me.Device.Session.Write(":SENS1:ADAP:CORR:COLL:ACQ LOAD")

        ' Wait for measurement end
        Me.Device.Session.Query("*OPC?")

        ' Save impedance setup data
        Me.Device.Session.Write(":SENS1:ADAP:CORR:COLL:SAVE IMP")

    End Sub

    ''' <summary> Builds compensation string. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    '''                                              null. </exception>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <param name="lowFrequencyValues">  The low frequency values. </param>
    ''' <param name="highFrequencyValues"> The high frequency values. </param>
    ''' <returns> A String. </returns>
    Public Shared Function BuildCompensationString(ByVal lowFrequencyValues() As Double,
                                            ByVal highFrequencyValues() As Double) As String
        If lowFrequencyValues Is Nothing Then Throw New ArgumentNullException(NameOf(lowFrequencyValues))
        If highFrequencyValues Is Nothing Then Throw New ArgumentNullException(NameOf(highFrequencyValues))
        If lowFrequencyValues.Count <> 3 Then Throw New InvalidOperationException($"Low frequency array has {lowFrequencyValues.Count} values instead of 3")
        If highFrequencyValues.Count <> 3 Then Throw New InvalidOperationException($"High frequency array has {highFrequencyValues.Count} values instead of 3")
        Dim builder As New System.Text.StringBuilder
        builder.Append($"{lowFrequencyValues(1)}")
        builder.AppendFormat($",{lowFrequencyValues(2)}")
        builder.AppendFormat($",{highFrequencyValues(1)}")
        builder.AppendFormat($",{highFrequencyValues(2)}")
        Return builder.ToString
    End Function

    ''' <summary> Merge compensations. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    '''                                              null. </exception>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <param name="frequencies"> The frequencies. </param>
    ''' <param name="values">      The values. </param>
    ''' <returns> A String. </returns>
    Public Shared Function MergeCompensations(ByVal frequencies As String, ByVal values As String) As String
        If String.IsNullOrEmpty(frequencies) Then Throw New ArgumentNullException(NameOf(frequencies))
        If String.IsNullOrEmpty(values) Then Throw New ArgumentNullException(NameOf(values))
        Dim builder As New System.Text.StringBuilder
        Dim f As New Stack(Of String)(frequencies.Split(","c))
        Dim v As New Stack(Of String)(values.Split(","c))
        If 2 * f.Count <> v.Count Then
            Throw New InvalidOperationException($"Number of values {v.Count} must be twice the number of frequencies {f.Count}")
        End If
        Do While f.Any
            If builder.Length > 0 Then
                builder.AppendFormat("{0}", f.Pop)
            Else
                builder.AppendFormat(",{0}", f.Pop)
            End If
            If v.Any Then builder.AppendFormat(",{0}", v.Pop)
            If v.Any Then builder.AppendFormat(",{0}", v.Pop)
        Loop
        Return builder.ToString
    End Function

    ''' <summary> Builds the compensations. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    '''                                              null. </exception>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <exception cref="InvalidCastException">      Thrown when an object cannot be cast to a
    '''                                              required type. </exception>
    ''' <param name="frequencies"> The frequencies. </param>
    ''' <param name="values">      The values. </param>
    ''' <returns> A Double()() </returns>
    Public Shared Function BuildCompensations(ByVal frequencies As String, ByVal values As String) As Double()()
        If String.IsNullOrEmpty(frequencies) Then Throw New ArgumentNullException(NameOf(frequencies))
        If String.IsNullOrEmpty(values) Then Throw New ArgumentNullException(NameOf(values))
        Dim f As New Stack(Of String)(frequencies.Split(","c))
        Dim v As New Stack(Of String)(values.Split(","c))
        If 2 * f.Count <> v.Count Then
            Throw New InvalidOperationException($"Number of values {v.Count} must be twice the number of frequencies {f.Count}")
        End If
        Dim data()() As Double = New Double(f.Count - 1)() {}
        Dim index As Integer = 0
        Do While f.Any
            Dim av() As Double = New Double(2) {}
            Dim pop As String = f.Pop
            If Not Double.TryParse(pop, av(0)) Then
                Throw New InvalidCastException($"Parse failed for frequency {pop}")
            End If
            pop = v.Pop
            If Not Double.TryParse(pop, av(1)) Then
                Throw New InvalidCastException($"Parse failed for real value {pop}")
            End If
            pop = v.Pop
            If Not Double.TryParse(pop, av(2)) Then
                Throw New InvalidCastException($"Parse failed for imaginary value {pop}")
            End If
            data(index) = av
            index += 1
        Loop
        Return data
    End Function

#Region " OPEN "

    ''' <summary> Toggle open compensation state. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="enabled"> true to enable, false to disable. </param>
    Public Sub ToggleOpenCompensationState(ByVal enabled As Boolean)
        Me.Device.Session.WriteLine(":SENS1:CORR2:OPEN {0}", If(enabled, "1", "0"))
    End Sub

    ''' <summary> Applies the open compensation. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    '''                                              null. </exception>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <param name="lowFrequencyValues">  The low frequency values. </param>
    ''' <param name="highFrequencyValues"> The high frequency values. </param>
    Public Sub ApplyOpenCompensation(ByVal lowFrequencyValues() As Double,
                                     ByVal highFrequencyValues() As Double)
        If lowFrequencyValues Is Nothing Then Throw New ArgumentNullException(NameOf(lowFrequencyValues))
        If highFrequencyValues Is Nothing Then Throw New ArgumentNullException(NameOf(highFrequencyValues))
        If lowFrequencyValues.Count <> 3 Then Throw New InvalidOperationException($"Low frequency array has {lowFrequencyValues.Count} values instead of 3")
        If highFrequencyValues.Count <> 3 Then Throw New InvalidOperationException($"High frequency array has {highFrequencyValues.Count} values instead of 3")
        Me.Device.Session.WriteLine(":SENS1:CORR2:ZME:OPEN:DATA {0}",
                                    E4990Panel.BuildCompensationString(lowFrequencyValues, highFrequencyValues))
    End Sub

    ''' <summary> Reads open compensations. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <returns> The open compensations. </returns>
    Public Function ReadOpenCompensations() As String

        ' read the frequencies
        Dim frequencies As String = Me.Device.Session.Query(":SENS1:CORR2:ZME:OPEN:FREQ?")

        ' read the data
        Dim values As String = Me.Device.Session.Query(":SENS1:CORR2:ZME:OPEN:DATA?")

        Return E4990Panel.MergeCompensations(frequencies, values)

    End Function

    ''' <summary> Acquires the open compensation. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="lowFrequency">  The low frequency. </param>
    ''' <param name="highFrequency"> The high frequency. </param>
    Public Sub AcquireOpenCompensation(ByVal lowFrequency As Double, ByVal highFrequency As Double)

        ' Compensation points will be acquired at the sweep points:
        Me.ConfigureSweep(lowFrequency, highFrequency)

        ' Select arbitrary fixture model           
        Me.Device.Session.Write(":SENS1:FIXT:SEL ARB")

        ' Set user-specified frequencies
        Me.Device.Session.Write(":SENS1:CORR:COLL:FPO USER")

        ' Acquire open fixture compensation
        Me.Device.Session.Write(":SENS1:CORR2:COLL:ACQ:OPEN")

        ' Wait for measurement end
        Me.Device.Session.Query("*OPC?")

    End Sub

#End Region

#Region " SHORT "

    ''' <summary> Toggle short compensation state. </summary>
    ''' <remarks> David, 4/15/2016. </remarks>
    ''' <param name="enabled"> true to enable, false to disable. </param>
    Public Sub ToggleShortCompensationState(ByVal enabled As Boolean)
        Me.Device.Session.WriteLine(":SENS1:CORR2:SHOR {0}", If(enabled, "1", "0"))
    End Sub


#End Region

#Region " EXAMPLE "

    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    Sub CompensateFixture()

        ' TimeOut time should be greater than the measurement time.    
        Me.Device.Session.StoreTimeout(TimeSpan.FromSeconds(10))

        ' Select compensation point
        ' 
        ' Set compensation point at fix
        Me.Device.Session.Write(":SENS1:CORR:COLL:FPO FIX")

        ' Select fixture model           
        Me.Device.Session.Write(":SENS1:FIXT:SEL ARB") '  FIXT16047A")

        Call DefineTermination()

        ' Perform Fixture Compensation
        Dim dlg As DialogResult = MessageBox.Show("Do you perform Open Fixture Compensation?", "Fixture Compensation", MessageBoxButtons.YesNo)

        If dlg = DialogResult.Yes Then
            MessageBox.Show("Connect Open Termination")

            ' Execute open in fixture compensation
            Me.Device.Session.Write(":SENS1:CORR2:COLL:ACQ:OPEN")

            ' Wait for measurement end
            Me.Device.Session.Query("*OPC?")

        End If

        ' Perform Fixture Compensation
        dlg = MessageBox.Show("Do you perform Short Fixture Compensation?", "Fixture Compensation", MessageBoxButtons.YesNo)

        If dlg = DialogResult.Yes Then

            MessageBox.Show("Connect Short Termination")

            ' Execute short in fixture compensation
            Me.Device.Session.Write(":SENS1:CORR2:COLL:ACQ:SHOR")

            ' Wait for measurement end
            Me.Device.Session.Query("*OPC?")

        End If

        ' Perform Fixture Compensation
        dlg = MessageBox.Show("Do you perform Load Fixture Compensation?", "Fixture Compensation", MessageBoxButtons.YesNo)

        If dlg = DialogResult.Yes Then

            MessageBox.Show("Connect LOAD Termination")

            ' Execute load in fixture compensation
            Me.Device.Session.Write(":SENS1:CORR2:COLL:ACQ:LOAD")

            ' Wait for measurement end
            Me.Device.Session.Query("*OPC?")

        End If

    End Sub

    Sub DefineTermination()
        '
        Dim LoadF() As String, n As Integer, i As Integer
        ' Define Short termination by equivalent circuit model
        ' Set equivalent circuit model for short
        Me.Device.Session.Write(":SENS1:CORR2:CKIT:SHOR:MOD EQU")
        ' Set short termination parameter (L)
        Me.Device.Session.Write(":SENS1:CORR2:CKIT:SHOR:L 1E-9")
        ' Set short termination parameter (R)  
        Me.Device.Session.Write(":SENS1:CORR2:CKIT:SHOR:R 1E-4")
        '
        ' Define Load by f-Z table model
        ' Set f-Z table model for short
        Me.Device.Session.Write(":SENS1:CORR2:CKIT:LOAD:MOD TABL")
        n = 4
        ReDim LoadF(n)
        ' Define f-Z table (freq, real, imaginary)
        LoadF(1) = "20, 49.5, 1E-3"
        LoadF(2) = "1E3, 49.9, 1.2E-3"
        LoadF(3) = "1E6, 50, 1.5E-3"
        LoadF(4) = "120E6, 50.9, 2E-3"
        ' :SENS1:CORR2:CKIT:LOAD:TABL {n}, {freq 1}, {real 1}, {imaginary 1}, ... , {freq n}, {real n}, {imaginary n}
        Me.Device.Session.Write(":SENS1:CORR2:CKIT:LOAD:TABL " & Str(n) & ",") ' Set f-Z table
        For i = 1 To n - 1
            Me.Device.Session.Write(LoadF(i) & ",")
        Next i
        Me.Device.Session.Write(LoadF(n))

    End Sub

#End Region

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

