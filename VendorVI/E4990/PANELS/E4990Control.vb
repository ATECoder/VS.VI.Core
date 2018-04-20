Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.ControlExtensions
Imports isr.Core.Controls.NumericUpDownExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.Core.Pith.TimeSpanExtensions
Imports isr.VI.ExceptionExtensions
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
      System.Drawing.ToolboxBitmap(GetType(E4990Control))>
Public Class E4990Control
    Inherits VI.Instrument.ResourceControlBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(Device.Create, True)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Public Sub New(ByVal device As Device, ByVal isDeviceOwner As Boolean)
        MyBase.New()
        Me._New(device, isDeviceOwner)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Private Sub _New(ByVal device As Device, ByVal isDeviceOwner As Boolean)

        Me.InitializingComponents = True
        ' This call is required by the designer.
        InitializeComponent()
        Me.InitializingComponents = False

        Me._ToolStripPanel.Renderer = New CustomProfessionalRenderer
        MyBase.AssignConnector(Me._ResourceSelectorConnector, True)

        Me._AssignDevice(device, isDeviceOwner)
        ' note that the caption is not set if this is run inside the On Load function.
        With Me._TraceMessagesBox
            ' set defaults for the messages box.
            .ResetCount = 500
            .PresetCount = 250
            .ContainerPanel = Me._MessagesTabPage
        End With
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
                Me.InitializingComponents = True
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
    Public ReadOnly Property Device As Device
        Get
            Return Me._Device
        End Get
    End Property

    ''' <summary> Assign device. </summary>
    ''' <param name="value"> The value. </param>
    Private Sub _AssignDevice(ByVal value As Device, ByVal isDeviceOwner As Boolean)
        If Me._Device IsNot Nothing OrElse MyBase.DeviceBase IsNot Nothing Then
            ' the device base already clears all or only the private listeners. 
            ' Me._Device.RemovePrivateListeners()
            Me._ReleaseDevice()
        End If
        Me._Device = value
        If value IsNot Nothing Then
            value.CaptureSyncContext(WindowsFormsSynchronizationContext.Current)
            value.AddPrivateListener(Me._TraceMessagesBox)
        End If
        ' the device base class addresses the device open state.
        MyBase.AssignDevice(value, isDeviceOwner)
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device, ByVal isDeviceOwner As Boolean)
        Me._AssignDevice(value, isDeviceOwner)
    End Sub

    Private Sub _ReleaseDevice()
        MyBase.ReleaseDevice()
        Me._Device = Nothing
    End Sub

    ''' <summary> Releases the device. </summary>
    ''' <remarks> Called from the base device to release the reference to the device. </remarks>
    Public Overrides Sub ReleaseDevice()
        Me._ReleaseDevice()
    End Sub

    ''' <summary> Releases the device and reassigns the default device. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Overrides Sub RestoreDevice()
        Me.AssignDevice(Device.Create, True)
    End Sub

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Gets the session. </summary>
    ''' <value> The session. </value>
    Private ReadOnly Property Session As VI.Pith.SessionBase
        Get
            Return Me.Device.Session
        End Get
    End Property

    ''' <summary> Executes the device open changed action. </summary>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As VI.DeviceBase)
        MyBase.OnDeviceOpenChanged(device)
        If Me.IsDeviceOpen Then
            'Me.DisplayFunctionModes(VI.Tsp.MultimeterFunctionMode.VoltageDC)
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
    Protected Overrides Sub HandlePropertyChange(ByVal device As VI.DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.HandlePropertyChange(device, propertyName)
        Select Case propertyName
            Case NameOf(isr.VI.DeviceBase.SessionServiceRequestEventEnabled)
                Me._SessionServiceRequestHandlerEnabledMenuItem.Checked = device.SessionServiceRequestEventEnabled
            Case NameOf(isr.VI.DeviceBase.DeviceServiceRequestHandlerAdded)
                Me._DeviceServiceRequestHandlerEnabledMenuItem.Checked = device.DeviceServiceRequestHandlerAdded
            Case NameOf(isr.VI.DeviceBase.MessageNotificationLevel)
                Me._SessionTraceEnabledMenuItem.Checked = device.MessageNotificationLevel <> NotifySyncLevel.None
            Case NameOf(isr.VI.DeviceBase.ServiceRequestEnableBitmask)
                Dim value As VI.Pith.ServiceRequests = device.ServiceRequestEnableBitmask
                Me._ServiceRequestEnableBitmaskNumeric.Value = value
                Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = $"SRE:0b{Convert.ToString(value, 2),8}".Replace(" ", "0")
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub DeviceOpened(ByVal sender As VI.DeviceBase, ByVal e As System.EventArgs)
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        AddHandler Me.Device.SourceChannelSubsystem.PropertyChanged, AddressOf Me.SourceChannelSubsystemPropertyChanged
        AddHandler Me.Device.SenseChannelSubsystem.PropertyChanged, AddressOf Me.SenseChannelSubsystemPropertyChanged
        AddHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
        AddHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
        AddHandler Me.Device.CalculateChannelSubsystem.PropertyChanged, AddressOf Me.CalculateChannelSubsystemPropertyChanged
        AddHandler Me.Device.CompensateOpenSubsystem.PropertyChanged, AddressOf Me.CompensateChannelSubsystemPropertyChanged
        AddHandler Me.Device.CompensateShortSubsystem.PropertyChanged, AddressOf Me.CompensateChannelSubsystemPropertyChanged
        AddHandler Me.Device.CompensateLoadSubsystem.PropertyChanged, AddressOf Me.CompensateChannelSubsystemPropertyChanged
        AddHandler Me.Device.PrimaryChannelTraceSubsystem.PropertyChanged, AddressOf Me.ChannelTraceSubsystemPropertyChanged
        AddHandler Me.Device.SecondaryChannelTraceSubsystem.PropertyChanged, AddressOf Me.ChannelTraceSubsystemPropertyChanged
        AddHandler Me.Device.ChannelTriggerSubsystem.PropertyChanged, AddressOf Me.ChannelTriggerSubsystemPropertyChanged
        AddHandler Me.Device.ChannelMarkerSubsystem.PropertyChanged, AddressOf Me.ChannelmarkerSubsystemPropertyChanged
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
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub DeviceClosing(ByVal sender As VI.DeviceBase, ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.DeviceClosing(sender, e)
        If e?.Cancel Then Return
        If Me.IsDeviceOpen Then
            RemoveHandler Me.Device.CalculateChannelSubsystem.PropertyChanged, AddressOf Me.CalculateChannelSubsystemPropertyChanged
            RemoveHandler Me.Device.CompensateOpenSubsystem.PropertyChanged, AddressOf Me.CompensateChannelSubsystemPropertyChanged
            RemoveHandler Me.Device.CompensateShortSubsystem.PropertyChanged, AddressOf Me.CompensateChannelSubsystemPropertyChanged
            RemoveHandler Me.Device.CompensateLoadSubsystem.PropertyChanged, AddressOf Me.CompensateChannelSubsystemPropertyChanged
            RemoveHandler Me.Device.ChannelMarkerSubsystem.PropertyChanged, AddressOf Me.ChannelmarkerSubsystemPropertyChanged
            RemoveHandler Me.Device.PrimaryChannelTraceSubsystem.PropertyChanged, AddressOf Me.ChannelTraceSubsystemPropertyChanged
            RemoveHandler Me.Device.SecondaryChannelTraceSubsystem.PropertyChanged, AddressOf Me.ChannelTraceSubsystemPropertyChanged
            RemoveHandler Me.Device.ChannelTriggerSubsystem.PropertyChanged, AddressOf Me.ChannelTriggerSubsystemPropertyChanged
            RemoveHandler Me.Device.TriggerSubsystem.PropertyChanged, AddressOf Me.TriggerSubsystemPropertyChanged
            RemoveHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
            RemoveHandler Me.Device.SenseChannelSubsystem.PropertyChanged, AddressOf Me.SenseChannelSubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " CALCULATE "

    ''' <summary> Handle the Calculate channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As CalculateChannelSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.CalculateChannelSubsystemBase.AveragingEnabled)
                Me._AveragingEnabledCheckBox.Checked = subsystem.AveragingEnabled.GetValueOrDefault(False)
            Case NameOf(VI.CalculateChannelSubsystemBase.AverageCount)
                Me._AveragingCountNumeric.Value = subsystem.AverageCount.GetValueOrDefault(0)
            Case NameOf(VI.CalculateChannelSubsystemBase.TraceCount)
                If subsystem.TraceCount.HasValue Then Me._TraceGroupBox.Text = $"Traces ({subsystem.TraceCount.Value})"
        End Select
    End Sub

    ''' <summary> Calculate channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub CalculateChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(E4990.CalculateChannelSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.CalculateChannelSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, E4990.CalculateChannelSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " COMPENSATE "

    ''' <summary> Handle the Compensate channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As Scpi.CompensateChannelSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        If subsystem.CompensationType = Scpi.CompensationTypes.OpenCircuit Then
            Select Case propertyName
                'Case NameOf(Scpi.CompensateChannelSubsystemBase.ChannelNumber)
            End Select
        ElseIf subsystem.CompensationType = Scpi.CompensationTypes.ShortCircuit Then
            Select Case propertyName
                'Case NameOf(Scpi.CompensateChannelSubsystemBase.ChannelNumber)
            End Select
        ElseIf subsystem.CompensationType = Scpi.CompensationTypes.Load Then
            Select Case propertyName
                'Case NameOf(Scpi.CompensateChannelSubsystemBase.ChannelNumber)
            End Select
        End If
    End Sub

    ''' <summary> Compensate channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub CompensateChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Scpi.CompensateChannelSubsystemBase)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.CompensateChannelSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, Scpi.CompensateChannelSubsystemBase), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " CHANNEL MARKER "

    ''' <summary> Handle the channel marker subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As ChannelMarkerSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(E4990.ChannelMarkerSubsystem.Abscissa)
                Me._MarkerFrequencyComboBox.Text = subsystem.Abscissa.ToString
            Case NameOf(E4990.ChannelMarkerSubsystem.MarkerReadings)
                If subsystem.MarkerReadings Is Nothing Then
                    Me._ReadingComboBox.Items.Clear()
                Else
                    subsystem.MarkerReadings.ListElements(Me._ReadingComboBox.ComboBox, VI.ReadingTypes.Units)
                End If
            Case NameOf(E4990.ChannelMarkerSubsystem.LastReading)
                Dim value As String = subsystem.LastReading
                Me._LastReadingTextBox.Text = If(String.IsNullOrWhiteSpace(value), "<last reading>", "value")
                subsystem.LastActionElapsedTime = Me.ReadElapsedTime(True)
            Case NameOf(E4990.ChannelMarkerSubsystem.LastActionElapsedTime)
                Me._LastReadingTextBox.Text = $"{subsystem.LastReading} @{subsystem.LastActionElapsedTime.ToExactMilliseconds:0.0}ms"
            Case NameOf(E4990.ChannelMarkerSubsystem.FailureCode)
                Me._FailureToolStripStatusLabel.Text = subsystem.FailureCode
            Case NameOf(E4990.ChannelMarkerSubsystem.FailureLongDescription)
                Me._FailureToolStripStatusLabel.ToolTipText = subsystem.FailureLongDescription
            Case NameOf(E4990.ChannelMarkerSubsystem.ReadingCaption)
                Me._ReadingToolStripStatusLabel.Text = subsystem.ReadingCaption
        End Select
    End Sub

    ''' <summary> Channel marker subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelmarkerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(E4990.ChannelMarkerSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.ChannelmarkerSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, E4990.ChannelMarkerSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try

    End Sub

#End Region

#Region " CHANNEL TRACE "

    ''' <summary> Handle the channel Trace subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As Scpi.ChannelTraceSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Dim combo As ComboBox = If(subsystem.TraceNumber = 1, Me._PrimaryTraceParameterComboBox, Me._SecondaryTraceParameterComboBox)
        Select Case propertyName
            Case NameOf(Scpi.ChannelTraceSubsystemBase.SupportedParameters)
                subsystem.ListParameters(combo)
            Case NameOf(Scpi.ChannelTraceSubsystemBase.Parameter)
                subsystem.SafeSelectTraceParameters(combo)
            Case NameOf(Scpi.ChannelTraceSubsystemBase.TraceNumber)
        End Select
    End Sub

    ''' <summary> Channel Trace subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelTraceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(ChannelTraceSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.ChannelTraceSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, ChannelTraceSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " CHANNEL TRIGGER "

    ''' <summary> Handle the channel Trigger subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As ChannelTriggerSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.ChannelTriggerSubsystemBase.ContinuousEnabled)
                If subsystem.ContinuousEnabled.HasValue Then Me._ContinuousEnabledMenuItem.Checked = subsystem.ContinuousEnabled.Value
        End Select
    End Sub

    ''' <summary> Channel Trigger subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelTriggerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(ChannelTriggerSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.ChannelTriggerSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, ChannelTriggerSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " DISPLAY "

    ''' <summary> Handle the Display subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As DisplaySubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            ' Case NameOf(E4990.DisplaySubsystem.DisplayScreen)
        End Select
    End Sub

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplaySubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(DisplaySubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.DisplaySubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, DisplaySubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " SENSE "

    ''' <summary> Handle the sense channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As Scpi.SenseChannelSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Scpi.SenseChannelSubsystemBase.SupportedAdapterTypes)
                subsystem.ListAdapters(Me._AdapterComboBox)
            Case NameOf(Scpi.SenseChannelSubsystemBase.Aperture)
                If subsystem.Aperture.HasValue Then Me._ApertureNumeric.Value = CDec(subsystem.Aperture.Value)
            Case NameOf(Scpi.SenseChannelSubsystemBase.AdapterType)
                If subsystem.AdapterType.HasValue Then Me.SelectAdapter(subsystem.AdapterType.Value)
            Case NameOf(Scpi.SenseChannelSubsystemBase.SweepPoints)
                If subsystem.SweepPoints.HasValue Then Me._SweepGroupBox.Text = $"Sweep points: {subsystem.SweepPoints.Value}"
            Case NameOf(Scpi.SenseChannelSubsystemBase.SweepStart)
                If subsystem.SweepStart.HasValue Then Me._LowFrequencyNumeric.Value = CDec(subsystem.SweepStart.Value)
            Case NameOf(Scpi.SenseChannelSubsystemBase.SweepStart)
                If subsystem.SweepStart.HasValue Then Me._HighFrequencyNumeric.Value = CDec(subsystem.SweepStop.Value)
        End Select
    End Sub

    ''' <summary> Sense channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SenseChannelSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SenseChannelSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SenseChannelSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " SOURCE "

    ''' <summary> Handle the Source channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As Scpi.SourceChannelSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Scpi.SourceChannelSubsystemBase.SupportedFunctionModes)
                subsystem.DisplaySupportedFunctionModes(Me._SourceFunctionComboBox)
            Case NameOf(Scpi.SourceChannelSubsystemBase.FunctionMode)
                Me.SelectSourceFunctionMode()
            Case NameOf(Scpi.SourceChannelSubsystemBase.Level)
                If subsystem.Level.HasValue Then Me.SourceLevel = subsystem.Level.Value
        End Select
    End Sub

    ''' <summary> Source channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SourceChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SourceChannelSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.PropertyChangedEventArgs)(AddressOf Me.SourceChannelSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SourceChannelSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " TRIGGER "

    ''' <summary> Handle the Trigger subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As Scpi.TriggerSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Scpi.TriggerSubsystemBase.SafeSelectTriggerSource)
                subsystem.ListTriggerSources(Me._TriggerSourceComboBox.ComboBox)
            Case NameOf(Scpi.TriggerSubsystemBase.TriggerSource)
                If subsystem.TriggerSource.HasValue Then subsystem.SafeSelectTriggerSource(Me._TriggerSourceComboBox.ComboBox)
            Case NameOf(Scpi.TriggerSubsystemBase.Delay)
        End Select
    End Sub

    ''' <summary> Trigger subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TriggerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(TriggerSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.TriggerSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, TriggerSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
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
    Protected Overrides Sub HandlePropertyChange(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.HandlePropertyChange(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(StatusSubsystemBase.DeviceErrorsReport)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.LastDeviceError)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.ServiceRequestStatus)
                Me._StatusRegisterLabel.Text = $"0x{subsystem.ServiceRequestStatus:X2}"
            Case NameOf(StatusSubsystemBase.StandardEventStatus)
                Me._StandardRegisterLabel.Text = $"0x{subsystem.StandardEventStatus:X2}"
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(StatusSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.StatusSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, StatusSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " SYSTEM "

    ''' <summary> Contact check enabled menu item check state changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId:="menuItem")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ContactCheckEnabledMenuItem_CheckStateChanged(sender As Object, e As EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me._InfoProvider.Clear()
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, "Exception occurred enabling contact check")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred enabling contact check;. {0}", ex.ToFullBlownString)
        End Try
    End Sub

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Application.DoEvents()
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(SystemSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.SystemSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, SystemSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#End Region

#Region " STATUS DISPLAY "

    Private _Status As String
    ''' <summary> Gets or sets the status. </summary>
    ''' <value> The status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property Status As String
        Get
            Return Me._Status
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.Status) Then
                Me._Status = value
                Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(value, Me._StatusLabel)
                Me._StatusLabel.ToolTipText = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _Identity As String
    ''' <summary> Gets or sets the identity. </summary>
    ''' <value> The identity. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property Identity As String
        Get
            Return Me._Identity
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.Identity) Then
                Me._Identity = value
                Me._IdentityLabel.Text = isr.Core.Pith.CompactExtensions.Compact(value, Me._IdentityLabel)
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _StatusRegisterCaption As String
    ''' <summary> Gets or sets the status register caption. </summary>
    ''' <value> The status register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property StatusRegisterCaption As String
        Get
            Return Me._StatusRegisterCaption
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.StatusRegisterCaption) Then
                Me._StatusRegisterCaption = value
                Me._StatusRegisterLabel.Text = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property
    Private _StandardRegisterCaption As String
    ''' <summary> Gets or sets the standard register caption. </summary>
    ''' <value> The status register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property StandardRegisterCaption As String
        Get
            Return Me._StandardRegisterCaption
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.StandardRegisterCaption) Then
                Me._StandardRegisterCaption = value
                Me._StandardRegisterLabel.Text = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the standard register status. </summary>
    ''' <value> The standard register status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property ServiceRequestEnableBitmask As Integer
        Get
            Return CInt(Me._ServiceRequestEnableBitmaskNumeric.Value)
        End Get
        Set(value As Integer)
            Me._ServiceRequestEnableBitmaskNumeric.Value = value
        End Set
    End Property

    Private _ServiceRequestEnableBitmaskCaption As String
    ''' <summary> Gets or sets the standard register caption. </summary>
    ''' <value> The standard register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overrides Property ServiceRequestEnableBitmaskCaption As String
        Get
            Return Me._ServiceRequestEnableBitmaskCaption
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.ServiceRequestEnableBitmaskCaption) Then
                Me._ServiceRequestEnableBitmaskCaption = value
                Me._ServiceRequestEnableBitmaskNumeric.ToolTipText = value
            End If
        End Set
    End Property

#End Region

#Region " CONTROL EVENT HANDLERS: READING "

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

    ''' <summary> Abort button click. </summary>
    ''' <param name="sender"> <see cref="T:System.Object" />
    '''                                             instance of this
    '''                       <see cref="T:System.Windows.Forms.Control" /> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AbortButton_Click(sender As Object, e As EventArgs) Handles _AbortButton.Click

        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "aborting measurements(s)"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If Me.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.TriggerSubsystem.Abort()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by InitButton for click events. Initiates a reading for
    ''' retrieval by way of the service request event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitiateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InitiateButton.Click
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "aborting measurements(s)"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()

            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")

            ' clear execution state before enabling events
            Me.Device.ClearExecutionState()

            ' set the service request
            ' Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All And Not VI.Pith.ServiceRequests.MessageAvailable)

            ' trigger the initiation of the measurement letting the service request do the rest.
            Me.Device.ClearExecutionState()
            ' Me.Device.TriggerSubsystem.Initiate()

        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Reading combo box selected value changed. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadingComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _ReadingComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "selecting a reading to display"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.ChannelMarkerSubsystem.SelectActiveReading(Me.SelectedReadingType)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Public Sub ReadMarker(ByVal e As ActionEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Me.Device.ChannelMarkerSubsystem.Enabled Then
            Dim activity As String = "reading a marker"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
            Me.Device.ClearExecutionState()
            ' clear the device display from warnings
            ' Me.Device.Session.Write(":DISP:CCL")
            Me.Device.DisplaySubsystem.ClearCautionMessages()
            If Me.Device.CalculateChannelSubsystem.AveragingEnabled.GetValueOrDefault(False) Then
                Me.Device.StatusSubsystem.EnableMeasurementAvailable()
                Me.Device.ChannelMarkerSubsystem.InitializeMarkerAverage(Me.Device)
            Else
                ' Me.Device.Session.WriteLine(":TRIG")
                Me.Device.TriggerSubsystem.Initiate()
            End If
            If Me.Device.StatusSubsystem.TryAwaitServiceRequest(VI.Pith.ServiceRequests.RequestingService, TimeSpan.FromSeconds(10),
                                                                TimeSpan.FromMilliseconds(100)) Then
                ' auto scale after measurement completes
                Me.Device.PrimaryChannelTraceSubsystem.AutoScale()
                Me.Device.SecondaryChannelTraceSubsystem.AutoScale()
                Me.Device.PrimaryChannelTraceSubsystem.Select()
                Me.Device.ChannelMarkerSubsystem.FetchLatestData()
            Else
                e.RegisterCancellation("timeout")
            End If
        Else
            e.RegisterCancellation("Marker not defined")
        End If
    End Sub

    ''' <summary> Event handler. Called by _ReadButton for click events. Query the Device for a
    ''' reading. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadButton.Click
        Dim activity As String = "reading a marker"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Dim args As New ActionEventArgs
            Me.ReadMarker(args)
            If args.Cancel Then
                Me._InfoProvider.Annunciate(sender, args.Details)
            End If

            If Me.Device.ChannelMarkerSubsystem.Enabled Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.ClearExecutionState()
                ' clear the device display from warnings
                ' Me.Device.Session.Write(":DISP:CCL")
                Me.Device.DisplaySubsystem.ClearCautionMessages()
                If Me._AveragingEnabledCheckBox.Checked Then
                    Me.Device.StatusSubsystem.EnableMeasurementAvailable()
                    Me.Device.ChannelMarkerSubsystem.InitializeMarkerAverage(Me.Device)
                Else
                    ' Me.Device.Session.WriteLine(":TRIG")
                    Me.Device.TriggerSubsystem.Initiate()
                End If
                If Me.Device.StatusSubsystem.TryAwaitServiceRequest(VI.Pith.ServiceRequests.RequestingService, TimeSpan.FromSeconds(10),
                                                                TimeSpan.FromMilliseconds(100)) Then
                    ' auto scale after measurement completes
                    Me.Device.PrimaryChannelTraceSubsystem.AutoScale()
                    Me.Device.SecondaryChannelTraceSubsystem.AutoScale()
                    Me.Device.PrimaryChannelTraceSubsystem.Select()
                    Me.Device.ChannelMarkerSubsystem.FetchLatestData()
                Else
                    Me._InfoProvider.Annunciate(sender, "timeout")
                End If
            Else
                Me._InfoProvider.Annunciate(sender, "Define a marker first.")
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: SOURCE "

    ''' <summary> Gets source level. </summary>
    ''' <value> The source level. </value>
    Private Property SourceLevel As Double
        Get
            Return If(Me.SelectedSourceFunctionMode = Scpi.SourceFunctionModes.Voltage, Me._LevelNumeric.Value, 0.001 * Me._LevelNumeric.Value)
        End Get
        Set(value As Double)
            If Me.SelectedSourceFunctionMode = Scpi.SourceFunctionModes.Voltage Then
                Me._LevelNumeric.Value = CDec(value)
            Else
                Me._LevelNumeric.Value = CDec(1000 * value)
            End If
        End Set
    End Property

    ''' <summary> Gets the selected source function mode. </summary>
    ''' <value> The selected source function mode. </value>
    Public ReadOnly Property SelectedSourceFunctionMode As Scpi.SourceFunctionModes
        Get
            Return SourceChannelSubsystem.SelectedFunctionMode(Me._SourceFunctionComboBox)
        End Get
    End Property

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySourceSettingButton_Click(sender As Object, e As EventArgs) Handles _ApplySourceSettingButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            ' Set OSC mode
            Me.Device.SourceChannelSubsystem.ApplyFunctionMode(SelectedSourceFunctionMode)
            Me.Device.SourceChannelSubsystem.ApplyLevel(Me.SourceLevel)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying source settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Select source function mode. </summary>
    Private Sub SelectSourceFunctionMode()
        Me.Device.SourceChannelSubsystem.SafeSelectFunctionMode(Me._SourceFunctionComboBox)
        If Me.Device.SourceChannelSubsystem.FunctionMode.HasValue Then
            If Me.Device.SourceChannelSubsystem.FunctionMode.Value = Scpi.SourceFunctionModes.Current Then
                With Me._LevelNumeric
                    .Minimum = CDec(1000 * Me.Device.SourceChannelSubsystem.LevelRange.Min)
                    .Maximum = CDec(1000 * Me.Device.SourceChannelSubsystem.LevelRange.Max)
                    .DecimalPlaces = Math.Max(0, Me.Device.SourceChannelSubsystem.LevelRange.DecimalPlaces - 3)
                    If .Minimum > 0 Then
                        .Increment = .Minimum
                    Else
                        .Increment = 0.1D * (.Maximum - .Minimum)
                    End If
                End With
            Else
                With Me._LevelNumeric
                    .Minimum = CDec(Me.Device.SourceChannelSubsystem.LevelRange.Min)
                    .Maximum = CDec(Me.Device.SourceChannelSubsystem.LevelRange.Max)
                    .DecimalPlaces = Me.Device.SourceChannelSubsystem.LevelRange.DecimalPlaces
                    If .Minimum > 0 Then
                        .Increment = .Minimum
                    Else
                        .Increment = 0.1D * (.Maximum - .Minimum)
                    End If
                End With
            End If
        End If
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SourceFunctionComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles _SourceFunctionComboBox.SelectedValueChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            With Me._LevelNumericLabel
                If SelectedSourceFunctionMode = Scpi.SourceFunctionModes.Voltage Then
                    .Text = "Level [V]:"
                Else
                    .Text = "Level [mA]:"
                End If
                .Left = Me._LevelNumeric.Left - .Width
                .Invalidate()
            End With
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred toggling source settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySourceFunctionButton_Click(sender As Object, e As EventArgs) Handles _ApplySourceFunctionButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            ' Set OSC mode
            Me.Device.SourceChannelSubsystem.ApplyFunctionMode(SelectedSourceFunctionMode)
            Me.Device.SourceChannelSubsystem.QueryLevel()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying source settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: SENSE "

#Region " SENSE / AVERAGING "

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyAveragingButton_Click(sender As Object, e As EventArgs) Handles _ApplyAveragingButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Device.SenseChannelSubsystem.ApplyAperture(Me._ApertureNumeric.Value)
            Me.Device.CalculateChannelSubsystem.ApplyAverageSettings(Me._AveragingEnabledCheckBox.Checked, CInt(Me._AveragingCountNumeric.Value))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying average settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _RestartAveragingButton_Click(sender As Object, e As EventArgs) Handles _RestartAveragingButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Device.CalculateChannelSubsystem.ClearAverage()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying average settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

#Region " SENSE / SWEEP "

    ''' <summary> Configure sweep. </summary>
    ''' <param name="lowFrequency">  The low frequency. </param>
    ''' <param name="highFrequency"> The high frequency. </param>
    Public Sub ConfigureSweep(ByVal lowFrequency As Double, ByVal highFrequency As Double)

        ' Set number of points
        'Me.Device.Session.Write(":SENS1:SWE:POIN 2")
        Me.Device.SenseChannelSubsystem.ApplySweepPoints(2)

        ' Set start frequency
        'Me.Device.Session.WriteLine(":SENS1:FREQ:STAR {0}", lowFrequency)
        Me.Device.SenseChannelSubsystem.ApplySweepStart(lowFrequency)

        ' Set stop frequency
        'Me.Device.Session.WriteLine(":SENS1:FREQ:STOP {0}", highFrequency)
        Me.Device.SenseChannelSubsystem.ApplySweepStop(highFrequency)

    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySweepSettingsButton_Click(sender As Object, e As EventArgs) Handles _ApplySweepSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me._MarkerFrequencyComboBox.Items.Clear()
            Me._MarkerFrequencyComboBox.Items.Add(Me._LowFrequencyNumeric.Value.ToString)
            Me._MarkerFrequencyComboBox.Items.Add(Me._HighFrequencyNumeric.Value.ToString)
            Me._MarkerFrequencyComboBox.SelectedIndex = 1
            ' set a two point sweep.
            Me.ConfigureSweep(Me._LowFrequencyNumeric.Value, Me._HighFrequencyNumeric.Value)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying sweep settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " SENSE / TRACES "

    ''' <summary> Configure sweep. </summary>
    Public Sub ConfigureTrace()

        ' Setup Channel 1
        Me.Device.CalculateChannelSubsystem.ApplyTraceCount(2)

        ' Allocate measurement parameter for trace 1: Rs
        ' Me.Device.Session.Write(":CALC1:PAR1:DEF RS")
        Me.Device.PrimaryChannelTraceSubsystem.ApplyParameter(Scpi.ChannelTraceSubsystemBase.SelectedTraceParameters(Me._PrimaryTraceParameterComboBox))
        Me.Device.PrimaryChannelTraceSubsystem.AutoScale()

        ' Allocate measurement parameter for trace 2: Ls
        'Me.Device.Session.Write(":CALC1:PAR2:DEF LS")
        Me.Device.SecondaryChannelTraceSubsystem.ApplyParameter(Scpi.ChannelTraceSubsystemBase.SelectedTraceParameters(Me._SecondaryTraceParameterComboBox))
        Me.Device.SecondaryChannelTraceSubsystem.AutoScale()

    End Sub

    Private Function SelectPrimaryTraceParameter(ByVal value As Scpi.TraceParameters) As Scpi.TraceParameters
        If Me.InitializingComponents OrElse value = Scpi.TraceParameters.None Then Return Scpi.TraceParameters.None
        If (value <> Scpi.TraceParameters.None) AndAlso (value <> Me.SelectedPrimaryTraceParameter) Then
            Me._PrimaryTraceParameterComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.SelectedPrimaryTraceParameter
    End Function

    Private ReadOnly Property SelectedPrimaryTraceParameter() As Scpi.TraceParameters
        Get
            Return CType(CType(Me._PrimaryTraceParameterComboBox.SelectedItem,
                System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, Scpi.TraceParameters)
        End Get
    End Property

    Private Function SelectSecondaryTraceParameter(ByVal value As Scpi.TraceParameters) As Scpi.TraceParameters
        If Me.InitializingComponents OrElse value = Scpi.TraceParameters.None Then Return Scpi.TraceParameters.None
        If (value <> Scpi.TraceParameters.None) AndAlso (value <> Me.SelectedSecondaryTraceParameter) Then
            Me._SecondaryTraceParameterComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.SelectedSecondaryTraceParameter
    End Function

    Private ReadOnly Property SelectedSecondaryTraceParameter() As Scpi.TraceParameters
        Get
            Return CType(CType(Me._SecondaryTraceParameterComboBox.SelectedItem,
                System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, Scpi.TraceParameters)
        End Get
    End Property

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyTracesButton_Click(sender As Object, e As EventArgs) Handles _ApplyTracesButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            ' set a two point sweep.
            Me.ConfigureTrace()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying trace settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " SENSE / MARKERS "

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyMarkerSettingsButton_Click(sender As Object, e As EventArgs) Handles _ApplyMarkerSettingsButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Dim f As Double = Double.Parse(Me._MarkerFrequencyComboBox.Text)
            ' to_do: use sense trace function to set the reading units.
            ' Turn on marker 1
            ' Me.Device.Session.Write(String.Format(Globalization.CultureInfo.InvariantCulture, ":CALC{0}:MARK{1} ON", channelNumber, markerNumber))
            Me.Device.ChannelMarkerSubsystem.ApplyEnabled(True)
            ' set marker position
            ' Me.Device.Session.Write(String.Format(Globalization.CultureInfo.InvariantCulture, ":CALC{0}:MARK{1}:X {2}", channelNumber, markerNumber, frequency))
            Me.Device.ChannelMarkerSubsystem.ApplyAbscissa(f)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying marker settings;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try


    End Sub

#End Region

#End Region

#Region " CONTROL EVENT HANDLERS: SENSE / CAL "

    ''' <summary> Selects a new Adapter to display. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    ''' <returns> A Scpi.AdapterType. </returns>
    Friend Function SelectAdapter(ByVal value As String) As Scpi.AdapterType
        Dim v As Scpi.AdapterType = Scpi.AdapterType.None
        If Me.InitializingComponents OrElse String.IsNullOrWhiteSpace(value) Then Return v
        If [Enum].GetNames(GetType(Scpi.AdapterType)).Contains(value) Then
            v = CType([Enum].Parse(GetType(Scpi.AdapterType), value), Scpi.AdapterType)
        End If
        Return Me.SelectAdapter(v)
    End Function

    ''' <summary> Selects a new Adapter to display.
    ''' </summary>
    Friend Function SelectAdapter(ByVal value As Scpi.AdapterType) As Scpi.AdapterType
        If Me.InitializingComponents OrElse value = Scpi.AdapterType.None Then Return Scpi.AdapterType.None
        If (value <> Scpi.AdapterType.None) AndAlso (value <> Me.SelectedAdapterType) Then
            Me._AdapterComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.SelectedAdapterType
    End Function

    ''' <summary> Gets the type of the selected Adapter. </summary>
    ''' <value> The type of the selected Adapter. </value>
    Private ReadOnly Property SelectedAdapterType() As Scpi.AdapterType
        Get
            Return CType(CType(Me._AdapterComboBox.SelectedItem,
                               System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, Scpi.AdapterType)
        End Get
    End Property

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AcquireCompensationButton_Click(sender As Object, e As EventArgs) Handles _AcquireCompensationButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Using w As New CompensationWizard
                Dim result As DialogResult = w.ShowDialog(Me)
                If result = DialogResult.OK Then
                    My.Settings.AdapterType = Me.Device.SenseChannelSubsystem.AdapterType.ToString
                    My.Settings.FrequencyArrayReading = Me.Device.CompensateOpenSubsystem.FrequencyArrayReading
                    My.Settings.OpenCompensationReading = Me.Device.CompensateOpenSubsystem.ImpedanceArrayReading
                    My.Settings.ShortCompensationReading = Me.Device.CompensateShortSubsystem.ImpedanceArrayReading
                    My.Settings.LoadCompensationReading = Me.Device.CompensateLoadSubsystem.ImpedanceArrayReading
                End If
            End Using
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred acquiring compensation;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyLoadButton_Click(sender As Object, e As EventArgs) Handles _ApplyLoadButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Device.CompensateLoadSubsystem.ApplyImpedanceArray(Me._LoadCompensationTextBox.Text)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying load compensation;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyShortButton_Click(sender As Object, e As EventArgs) Handles _ApplyShortButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Device.CompensateShortSubsystem.ApplyImpedanceArray(Me._ShortCompensationTextBox.Text)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying short compensation;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyOpenButton_Click(sender As Object, e As EventArgs) Handles _ApplyOpenButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Device.SenseChannelSubsystem.ApplyAdapterType(Me.SelectedAdapterType)
            Me.Device.CompensateOpenSubsystem.ApplyImpedanceArray(Me._OpenCompensationTextBox.Text)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying open compensation;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: SCALE "

    ''' <summary> Automatic scale menu item click. </summary>
    ''' <param name="sender"> <see cref="T:System.Object" />
    '''                                             instance of this
    '''                       <see cref="T:System.Windows.Forms.Control" /> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AutoScaleMenuItem_Click(sender As Object, e As EventArgs) Handles _AutoScaleMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            Me.Device.PrimaryChannelTraceSubsystem.AutoScale()
            Me.Device.SecondaryChannelTraceSubsystem.AutoScale()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying auto scale;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS: TRIGGER "

    ''' <summary> Applies the trigger options menu item click. </summary>
    ''' <param name="sender"> <see cref="T:System.Object" />
    '''                                             instance of this
    '''                       <see cref="T:System.Windows.Forms.Control" /> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyTriggerOptionsMenuItem_Click(sender As Object, e As EventArgs) Handles _ApplyTriggerOptionsMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()

            ' Turn on Continuous Activation mode for channel 1
            ' Me.Device.Session.Write(":INIT1:CONT ON")
            Me.Device.ChannelTriggerSubsystem.ApplyContinuousEnabled(Me._ContinuousEnabledMenuItem.Checked)

            ' Set trigger source, e.g., 
            ' Me.Device.Session.Write(":TRIG:SOUR BUS")
            Me.Device.TriggerSubsystem.ApplyTriggerSource(Scpi.TriggerSubsystemBase.SelectedTriggerSource(Me._TriggerSourceComboBox.ComboBox))
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying trigger source;. {0}", ex.ToFullBlownString)
        Finally
            Me.ReadServiceRequestStatus()
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
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.SystemSubsystem.ClearInterface()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
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
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.SystemSubsystem.ClearDevice()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
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
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                Me.Device.SystemSubsystem.ClearExecutionState()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
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
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    Me.Device.ResetKnownState()
                End If
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
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
    Private Sub _InitKnownStateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InitKnowStateMenuItem.Click
        Dim activity As String = "resetting known state"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing Then
                If Me.IsDeviceOpen Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    Me.Device.ResetKnownState()
                    activity = "initializing known state"
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                    Me.Device.InitKnownState()
                End If
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
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
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "toggling instrument message tracing"
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        If menuItem IsNot Nothing Then
        End If
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                If menuItem.Checked Then
                    ' TODO: Change menu item to a drop down.
                    Me.Device.MessageNotificationLevel = NotifySyncLevel.Async
                Else
                    Me.Device.MessageNotificationLevel = NotifySyncLevel.None
                End If

            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the session service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionServiceRequestHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SessionServiceRequestHandlerEnabledMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Toggle session service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.Session.ServiceRequestEventEnabled Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.Device.Session.EnableServiceRequest()
                    If Me._ServiceRequestEnableBitmaskNumeric.Value = 0 Then
                        If Me._ServiceRequestEnableBitmaskNumeric.Value = 0 Then
                            Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.All)
                        Else
                            Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableBitmaskNumeric.Value, VI.Pith.ServiceRequests))
                        End If
                    Else
                        Me.Device.StatusSubsystem.EnableServiceRequest(CType(Me._ServiceRequestEnableBitmaskNumeric.Value, VI.Pith.ServiceRequests))
                    End If
                Else
                    Me.Device.Session.DisableServiceRequest()
                    Me.Device.StatusSubsystem.EnableServiceRequest(VI.Pith.ServiceRequests.None)
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Toggles the Device service request handler . </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DeviceServiceRequestHandlerEnabledMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _DeviceServiceRequestHandlerEnabledMenuItem.CheckStateChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Toggle device service request handling"
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._InfoProvider.Clear()
            If menuItem IsNot Nothing AndAlso menuItem.Checked <> Me.Device.DeviceServiceRequestHandlerAdded Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Title} {activity};. {Me.Device.ResourceNameCaption}")
                If menuItem IsNot Nothing AndAlso menuItem.Checked Then
                    Me.AddServiceRequestEventHandler()
                Else
                    Me.RemoveServiceRequestEventHandler()
                End If
                Me.Device.StatusSubsystem.ReadEventRegisters()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, ex.ToString)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.Title} exception {activity};. {ex.ToFullBlownString}")
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

