Imports System.Windows.Forms
Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.Core.Pith.SplitExtensions
''' <summary> Provides a user interface for the EG2000 Prober Device. </summary>
''' <license> (c) 2015 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/01/2013" by="David" revision="3.0.5022"> Created. </history>
<System.ComponentModel.DisplayName("EG2000 Panel"),
      System.ComponentModel.Description("EG2000 Prober Panel"),
      System.Drawing.ToolboxBitmap(GetType(EG2000.EG2000Panel))>
Public Class EG2000Panel
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
        ' note that the caption is not set if this is run inside the On Load function.
        With Me.TraceMessagesBox
            ' set defaults for the messages box.
            .ResetCount = 500
            .PresetCount = 250
            .SupportsOpenLogFolderRequest = False
            .ContainerPanel = Me._MessagesTabPage
        End With
    End Sub

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
            Me.onLastReadingAvailable(Nothing)
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

    ''' <summary> Gets a reference to the Device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overloads ReadOnly Property Device() As Device

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
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

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)

        ' populate the supported commands. 
        With Me._CommandComboBox
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = Me.Device.ProberSubsystem.SupportedCommands
            .SelectedIndex = 0
        End With

        ' populate the emulated reply combo.
        With Me._EmulatedReplyComboBox
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = Me.Device.ProberSubsystem.SupportedEmulationCommands
            .SelectedIndex = 0
        End With

        ' 2016/01/18 moved to device initialize known state: Me.EnableServiceRequestEventHandler()
        AddHandler Me.Device.ProberSubsystem.PropertyChanged, AddressOf Me.ProberSubsystemPropertyChanged
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
            RemoveHandler Me.Device.ProberSubsystem.PropertyChanged, AddressOf Me.ProberSubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

    ''' <summary> Updates the indicator. </summary>
    ''' <param name="label">    The label. </param>
    ''' <param name="sentinel"> The sentinel. </param>
    Private Shared Sub updateIndicator(ByVal label As Windows.Forms.Label, ByVal sentinel As Boolean?)
        If sentinel.GetValueOrDefault(False) Then
            label.BackColor = Drawing.Color.LightGreen
        Else
            label.BackColor = Drawing.Color.White
        End If
    End Sub

    ''' <summary> Updates the indicator. </summary>
    ''' <param name="label">    The label. </param>
    ''' <param name="value"> The value. </param>
    Private Shared Sub updateIndicator(ByVal label As Windows.Forms.Label, ByVal prefix As String, ByVal value As String, ByVal sentinel As Boolean?)
        updateIndicator(label, sentinel)
        If sentinel.GetValueOrDefault(False) Then
            label.Text = prefix & value
        Else
            label.Text = prefix
        End If
    End Sub

#Region " PROBER "

    ''' <summary> Handles the last reading available action. </summary>
    ''' <param name="value"> The reading. </param>
    Private Sub onLastReadingAvailable(ByVal value As String)
        If String.IsNullOrWhiteSpace(value) Then
            value = ""
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Last reading cleared")
        Else
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Received '{0}'", value)
        End If
        Me._ReadingTextBox.Text = value
        Me._LastMessageTextBox.Text = value
    End Sub

    ''' <summary> Handles the test start requested action. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    Private Sub onTestStartRequested(ByVal subsystem As ProberSubsystem)
        If subsystem Is Nothing Then
        ElseIf subsystem.IsFirstTestStart.GetValueOrDefault(False) Then
            EG2000Panel.updateIndicator(Me._TestStartAttributeLabel, "..", "First Test", subsystem.IsFirstTestStart)
        ElseIf subsystem.RetestRequested.GetValueOrDefault(False) Then
            EG2000Panel.updateIndicator(Me._TestStartAttributeLabel, "..", "Retest", subsystem.RetestRequested)
        ElseIf subsystem.TestAgainRequested.GetValueOrDefault(False) Then
            EG2000Panel.updateIndicator(Me._TestStartAttributeLabel, "..", "Test Again", subsystem.TestAgainRequested)
        Else
            EG2000Panel.updateIndicator(Me._TestStartAttributeLabel, "..", "TS", New Boolean?)
        End If
    End Sub

    ''' <summary> Handle the Prober subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As ProberSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
                ' The error is read by the EG2000 device
                ' the id is set by the device using the last reading that was set.
            Case NameOf(subsystem.ErrorRead), NameOf(subsystem.IdentityRead),
                 NameOf(subsystem.MessageCompleted), NameOf(subsystem.MessageFailed)
                If subsystem.ErrorRead OrElse subsystem.IdentityRead OrElse
                    subsystem.MessageCompleted OrElse subsystem.MessageFailed Then
                    Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{propertyName.SplitWords};. ")
                End If
            Case NameOf(subsystem.IsFirstTestStart), NameOf(subsystem.RetestRequested)
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{propertyName.SplitWords};. ")
                Me.onTestStartRequested(subsystem)
            Case NameOf(subsystem.LastMessageSent)
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Sent '{0}';. ", subsystem.LastMessageSent)
            Case NameOf(subsystem.LastReading)
                Me.onLastReadingAvailable(subsystem.LastReading)
            Case NameOf(subsystem.PatternCompleteReceived)
                EG2000Panel.updateIndicator(Me._PatternCompleteLabel, subsystem.PatternCompleteReceived)
            Case NameOf(subsystem.SetModeSent)
                Me._SendMessageLabel.Text = subsystem.LastMessageSent
            Case NameOf(subsystem.TestCompleteSent)
                EG2000Panel.updateIndicator(Me._TestCompleteLabel, subsystem.TestCompleteSent)
            Case NameOf(subsystem.TestStartReceived)
                EG2000Panel.updateIndicator(Me._TestStartedLabel, subsystem.TestStartReceived)
            Case NameOf(subsystem.UnhandledMessageReceived)
                EG2000Panel.updateIndicator(Me._UnhandledMessageLabel, "? ", subsystem.LastReading, subsystem.UnhandledMessageReceived)
            Case NameOf(subsystem.UnhandledMessageSent)
                EG2000Panel.updateIndicator(Me._UnhandledSendLabel, "? ", subsystem.LastMessageSent, subsystem.UnhandledMessageSent)
            Case NameOf(subsystem.WaferStartReceived)
                EG2000Panel.updateIndicator(Me._WaferStartReceivedLabel, subsystem.WaferStartReceived)
        End Select
    End Sub

    ''' <summary> Prober subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ProberSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, ProberSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnPropertyChanged(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(subsystem.DeviceErrors)
                Me._LastMessageTextBox.Text = subsystem.LastDeviceError.CompoundErrorMessage
            Case NameOf(subsystem.LastDeviceError)
                Me._LastMessageTextBox.Text = subsystem.LastDeviceError.CompoundErrorMessage
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

    ''' <summary> Event handler. Called by interfaceClearButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InterfaceClearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InterfaceClearButton.Click
        Try
            ' Turn on the form hourglass
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
    Private Sub _ResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ResetButton.Click
        Try
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
        End Try
    End Sub

    ''' <summary> Event handler. Called by _InitializeKnownStateButton for click events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitializeKnownStateButton_Click(sender As System.Object, ByVal e As System.EventArgs) Handles _InitializeKnownStateButton.Click
        Try
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
                               "Exception occurred resetting and initializing known state;. Details: {0}", ex)
        End Try
    End Sub

#End Region

#Region " CONTROL EVENTS: READ and WRITE "

    ''' <summary> Event handler. Called by _SessionTraceEnableCheckBox for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _SessionTraceEnableCheckBox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles _SessionTraceEnableCheckBox.CheckedChanged
        If Me._InitializingComponents Then Return
        If Not Me.DesignMode AndAlso sender IsNot Nothing Then
            Dim checkBox As Windows.Forms.CheckBox = CType(sender, Windows.Forms.CheckBox)
            Me.Device.SessionMessagesTraceEnabled = checkBox.Checked
        End If
    End Sub

    ''' <summary> Event handler. Called by _EmulateButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _EmulateButton_Click(sender As System.Object, ByVal e As System.EventArgs) Handles _EmulateButton.Click
        Dim message As String = Me._EmulatedReplyComboBox.Text.Trim
        If Not String.IsNullOrWhiteSpace(message) Then
            Try
                Me.ErrorProvider.Clear()
                Me.Device.ProberSubsystem.LastReading = message
                Me.Device.ProberSubsystem.ParseReading(message)
            Catch ex As Exception
                Me.ErrorProvider.Annunciate(sender, ex.ToString)
                Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   "Exception occurred sending emulation message;. '{0}'. Details: {1}", message, ex)
            End Try
        End If
    End Sub

    ''' <summary> Event handler. Called by _WriteButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _WriteButton_Click(sender As System.Object, ByVal e As System.EventArgs) Handles _WriteButton.Click
        Dim c As Windows.Forms.Control = TryCast(sender, Windows.Forms.Control)
        If c IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(Me._CommandComboBox.Text) Then
            Dim message As String = Me._CommandComboBox.Text.Trim
            Try
                Me.ErrorProvider.Clear()
                Me.Cursor = Windows.Forms.Cursors.WaitCursor
                Me.Device.ProberSubsystem.CommandTimeoutInterval = TimeSpan.FromMilliseconds(400)
                If Me._CommandComboBox.Text.StartsWith(Me.Device.ProberSubsystem.TestCompleteCommand,
                                                       StringComparison.OrdinalIgnoreCase) Then
                    Me.Device.ProberSubsystem.CommandTimeoutInterval = TimeSpan.FromMilliseconds(1000)
                End If
                If Me.Device.ProberSubsystem.TrySendAsync(message) Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                       "Message sent;. Sent: '{0}'; Received: '{1}'.",
                                       Me.Device.ProberSubsystem.LastMessageSent, Me.Device.ProberSubsystem.LastReading)
                Else
                    If Me.Device.ProberSubsystem.UnhandledMessageSent Then
                        Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                           "Failed sending message--unknown message sent;. Sent: {0}", message)
                        Me.ErrorProvider.SetError(c, "Failed sending message--unknown message sent.")
                    ElseIf Me.Device.ProberSubsystem.UnhandledMessageReceived Then
                        Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                           "Failed sending message--unknown message received;. Sent: {0}", message)
                        Me.ErrorProvider.SetError(c, "Failed sending message--unknown message received.")
                    Else
                        Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed sending message;. Sent: {0}", message)
                        Me.ErrorProvider.SetError(c, "Failed sending message.")
                    End If
                End If
            Catch ex As Exception
                Me.ErrorProvider.SetError(c, ex.ToString)
                Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   "Exception occurred sending message;. '{0}'. Details: {1}", message, ex)
            Finally
                Me.Cursor = Windows.Forms.Cursors.Default
            End Try
        End If
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
