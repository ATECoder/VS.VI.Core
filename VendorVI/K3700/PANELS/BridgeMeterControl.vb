Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> A meter control. </summary>
''' <license>
''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="10/9/2017" by="David" revision=""> Created. </history>
Public Class BridgeMeterControl
    Inherits isr.VI.Instrument.ResourceControlBase

#Region " CONSTRUCTORS "

    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(BridgeMeterDevice.Create, True)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device">        The device. </param>
    ''' <param name="isDeviceOwner"> True if is device owner, false if not. </param>
    Public Sub New(ByVal device As BridgeMeterDevice, ByVal isDeviceOwner As Boolean)
        MyBase.New(device, isDeviceOwner)
        Me._New(device, isDeviceOwner)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device">        The device. </param>
    ''' <param name="isDeviceOwner"> True if is device owner, false if not. </param>
    Private Sub _New(ByVal device As BridgeMeterDevice, ByVal isDeviceOwner As Boolean)
        Me.InitializingComponents = True
        ' This call is required by the designer.
        InitializeComponent()
        Me.InitializingComponents = False

        Me._BottomToolStrip.Renderer = New CustomProfessionalRenderer
        MyBase.AssignConnector(Me._ResourceSelectorConnector, True)
        Me._AssignDevice(device, isDeviceOwner)

        Me._MeasureGroupBox.Enabled = False
        Me._ConfigureGroupBox.Enabled = False

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
            'Me._TopToolStrip.Renderer = New CustomProfessionalRenderer
            'Me._TopToolStrip.Invalidate()
        Finally
            MyBase.OnLoad(e)
        End Try
    End Sub

#End Region

#Region " DEVICE "

    Private _Device As BridgeMeterDevice
    ''' <summary> Gets or sets the device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property Device As BridgeMeterDevice
        Get
            Return Me._Device
        End Get
    End Property

    ''' <summary> Assign device. </summary>
    ''' <param name="value"> The value. </param>
    Private Sub _AssignDevice(ByVal value As BridgeMeterDevice, ByVal isDeviceOwner As Boolean)
        If Me._Device IsNot Nothing OrElse MyBase.DeviceBase IsNot Nothing Then
            Me._ReleaseDevice()
        End If
        Me._Device = value
        If value IsNot Nothing Then
            value.CaptureSyncContext(WindowsFormsSynchronizationContext.Current)
        End If
        ' the device base class addresses the device open state.
        MyBase.AssignDevice(value, isDeviceOwner)
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As BridgeMeterDevice, ByVal isDeviceOwner As Boolean)
        Me._AssignDevice(value, isDeviceOwner)
    End Sub

    ''' <summary> Releases the device. </summary>
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
        Me.AssignDevice(BridgeMeterDevice.Create, True)
    End Sub

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As VI.DeviceBase, ByVal e As System.EventArgs)
        AddHandler Me.Device.MultimeterSubsystem.PropertyChanged, AddressOf Me.MultimeterSubsystemPropertyChanged
        AddHandler Me.Device.ChannelSubsystem.PropertyChanged, AddressOf Me.ChannelSubsystemPropertyChanged
        AddHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        MyBase.DeviceOpened(sender, e)
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceClosing(ByVal sender As VI.DeviceBase, ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.DeviceClosing(sender, e)
        If e?.Cancel Then Return
        If Me.IsDeviceOpen Then
            RemoveHandler Me.Device.MultimeterSubsystem.PropertyChanged, AddressOf Me.MultimeterSubsystemPropertyChanged
            RemoveHandler Me.Device.ChannelSubsystem.PropertyChanged, AddressOf Me.ChannelSubsystemPropertyChanged
            RemoveHandler Me.Device.DisplaySubsystem.PropertyChanged, AddressOf Me.DisplaySubsystemPropertyChanged
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

    ''' <summary> Handles the bridge Meter device property changed event. </summary>
    ''' <param name="sender">    The sender. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Overloads Sub HandlePropertyChange(ByVal sender As BridgeMeterDevice, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(isr.VI.DeviceBase.ResourceNameCaption)
                Me._ReadingLabel.Text = Me.Device.ResourceNameCaption
            Case NameOf(BridgeMeterDevice.IsDeviceOpen)
                Me._ConfigureGroupBox.Enabled = Me.IsDeviceOpen
                Me._ReadingLabel.Text = Me.Device.ResourceNameCaption
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Device.StatusSubsystem.VersionInfo.Model} is {If(IsDeviceOpen, "open", "close")}")
            Case NameOf(BridgeMeterDevice.PowerLineCycles)
                '  Me.PowerLineCycles = sender.PowerLineCycles
            Case NameOf(BridgeMeterDevice.MeasurementEnabled)
                Me._MeasureGroupBox.Enabled = sender.MeasurementEnabled
            Case NameOf(BridgeMeterDevice.HasBridgeValues)
                sender.Bridge.DisplayValues(Me._DataGrid)
        End Select
    End Sub

    ''' <summary> Multimeter sender property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub BridgeMeterDevicePropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.BridgeMeterDevice)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.BridgeMeterDevicePropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, K3700.BridgeMeterDevice), e.PropertyName)
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

#Region " SUBSYSTEMS "

#Region " MUTLIMETER "

    ''' <summary> Handles the Multimeter subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As MultimeterSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K3700.MultimeterSubsystem.FilterCount)
                If subsystem.FilterCount.HasValue Then Me._FilterCountNumeric.Value = subsystem.FilterCount.Value
            Case NameOf(K3700.MultimeterSubsystem.FilterCountRange)
                With Me._FilterCountNumeric
                    .Maximum = CDec(subsystem.FilterCountRange.Max)
                    .Minimum = CDec(subsystem.FilterCountRange.Min)
                    .DecimalPlaces = 0
                End With
            Case NameOf(K3700.MultimeterSubsystem.FilterWindow)
                'If subsystem.FilterWindow.HasValue Then Me._FilterWindowNumeric.Value = CDec(100 * subsystem.FilterWindow.Value)
            Case NameOf(K3700.MultimeterSubsystem.FunctionMode)
                Me._MeasureGroupBox.Enabled = Me.IsDeviceOpen AndAlso
                                                Me.Device.MultimeterSubsystem.FunctionMode.HasValue AndAlso
                                                Me.Device.MultimeterSubsystem.FunctionMode.Value = MultimeterFunctionMode.ResistanceFourWire
            Case NameOf(K3700.MultimeterSubsystem.PowerLineCycles)
                If subsystem.PowerLineCycles.HasValue Then Me._PowerLineCyclesNumeric.Value = CDec(subsystem.PowerLineCycles.Value)
            Case NameOf(K3700.MultimeterSubsystem.PowerLineCyclesRange)
                With Me._PowerLineCyclesNumeric
                    .Maximum = CDec(subsystem.PowerLineCyclesRange.Max)
                    .Minimum = CDec(subsystem.PowerLineCyclesRange.Min)
                    .DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces
                End With
            Case NameOf(K3700.MultimeterSubsystem.LastReading)
                Me._ReadingLabel.Text = subsystem.LastReading
        End Select
    End Sub

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MultimeterSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.MultimeterSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.MultimeterSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, K3700.MultimeterSubsystem), e.PropertyName)
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

#Region " CHANNEL "

    ''' <summary> Handles the Channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As ChannelSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K3700.ChannelSubsystem.ClosedChannels)
                ' Me.ClosedChannels = subsystem.ClosedChannels
        End Select
    End Sub

    ''' <summary> Channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.ChannelSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.ChannelSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, K3700.ChannelSubsystem), e.PropertyName)
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

    ''' <summary> Handles the Display subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As DisplaySubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(K3700.DisplaySubsystem.DisplayScreen)
        End Select
    End Sub

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplaySubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.DisplaySubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.DisplaySubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, K3700.DisplaySubsystem), e.PropertyName)
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
    Protected Overrides Sub OnLastError(ByVal lastError As VI.DeviceError)
        If lastError IsNot Nothing Then
            '   Me._LastErrorTextBox.ForeColor = If(lastError.IsError, Drawing.Color.OrangeRed, Drawing.Color.Aquamarine)
            '  Me._LastErrorTextBox.Text = lastError.CompoundErrorMessage
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
                Me.OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.LastDeviceError)
                Me.OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.ErrorAvailable)
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.StatusSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.StatusSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, K3700.StatusSubsystem), e.PropertyName)
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

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Overloads Sub HandlePropertyChange(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(K3700.SystemSubsystem)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.SystemSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, K3700.SystemSubsystem), e.PropertyName)
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

#Region " CONTROL EVENTS "

    ''' <summary> Measure button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _MeasureButton_Click(sender As Object, e As EventArgs) Handles _MeasureButton.Click
        Dim activity As String = "attempting to measure bridge"
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim args As New isr.Core.Pith.ActionEventArgs
            If Not Me.Device.TryMeasureBridge(args) Then
                Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Failed {activity};. {args.Details}")
            End If
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Configure button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ConfigureButton_Click(sender As Object, e As EventArgs) Handles _ConfigureButton.Click
        Dim activity As String = "attempting to configure measure"
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim args As New isr.Core.Pith.ActionEventArgs
            If Not Me.Device.TryConfigureMeter(Me._PowerLineCyclesNumeric.Value, args) Then
                Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Failed {activity};. {args.Details}")
            End If
        Finally
            Me.Cursor = Cursors.Default
        End Try
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

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
    ''' <summary> Gets or sets the power line cycles. </summary>
    ''' <value> The power line cycles. </value>
    Public Property PowerLineCycles As Double
        Get
            Return Me._PowerLineCyclesNumeric.Value
        End Get
        Set(value As Double)
            If Me.PowerLineCycles <> value OrElse (Me.Device.IsDeviceOpen AndAlso Not Nullable.Equals(Me.Device.MultimeterSubsystem.PowerLineCycles, value)) Then
                Me._PowerLineCyclesNumeric.Value = CDec(value)
                If Me.Device.IsDeviceOpen AndAlso Not Nullable.Equals(Me.Device.MultimeterSubsystem.PowerLineCycles, value) Then
                    Me.Device.MultimeterSubsystem.ApplyPowerLineCycles(value)
                End If
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the bridges. </summary>
    ''' <value> The bridge. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property Bridge As BridgeMeterResistorCollection

    ''' <summary> Configure meter. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    Public Sub ConfigureMeter()
        Dim activity As String = $"Checking {My.Settings.ResourceName} is open"
        If Me.IsDeviceOpen Then
            activity = $"Configuring function mode {MultimeterFunctionMode.ResistanceFourWire}"
            Dim expectedMeasureFunction As MultimeterFunctionMode = MultimeterFunctionMode.ResistanceFourWire
            Dim measureFunction As MultimeterFunctionMode? = Device.MultimeterSubsystem.ApplyFunctionMode(expectedMeasureFunction)
            If measureFunction.HasValue Then
                If expectedMeasureFunction <> measureFunction.Value Then
                    Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {expectedMeasureFunction } <> Actual {measureFunction.Value}")
                End If
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity}--no value set")
            End If
            Dim expectedPowerLineCycles As Double = Me._PowerLineCyclesNumeric.Value
            activity = $"Configuring power line cycles {expectedPowerLineCycles}"
            Dim actualPowerLineCycles As Double? = Device.MultimeterSubsystem.ApplyPowerLineCycles(expectedPowerLineCycles)
            If actualPowerLineCycles.HasValue Then
                If expectedPowerLineCycles <> actualPowerLineCycles.Value Then
                    Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {expectedPowerLineCycles} <> Actual {actualPowerLineCycles.Value}")
                End If
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity}--no value set")
            End If
        Else
            Throw New VI.Pith.OperationFailedException($"Failed {activity}; VISA session to this device is not open")
        End If
    End Sub


    ''' <summary> Attempts to configure meter from the given data. </summary>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryConfigureMeter(ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"Configuring bridge meter {My.Settings.ResourceName}"
        Try
            Me.ConfigureMeter()
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Measure resistance. </summary>
    ''' <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    '''                                             null. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="resistor"> The resistor. </param>
    Public Sub MeasureResistance(ByVal resistor As BridgeMeterResistor)
        If resistor Is Nothing Then Throw New ArgumentNullException(NameOf(resistor))
        Dim activity As String = $"measuring {resistor.Title}"
        If Me.Device.IsDeviceOpen Then
            activity = $"{resistor.Title} opening channels"
            Dim expectedChannelList As String = ""
            Dim actualChannelList As String = Me.Device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
            If expectedChannelList <> actualChannelList Then
                Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {expectedChannelList } <> Actual {actualChannelList}")
            End If

            activity = $"{resistor.Title} closing {resistor.ChannelList}"
            expectedChannelList = resistor.ChannelList
            actualChannelList = Device.ChannelSubsystem.ApplyClosedChannels(expectedChannelList, TimeSpan.FromSeconds(2))
            If expectedChannelList <> actualChannelList Then
                Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {expectedChannelList } <> Actual {actualChannelList}")
            End If

            activity = $"measuring {resistor.Title}"
            If Device.MultimeterSubsystem.Measure.HasValue Then
                resistor.Resistance = Device.MultimeterSubsystem.Reading.GetValueOrDefault(-1)
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity}; driver returned nothing")
            End If
        Else
            Throw New VI.Pith.OperationFailedException($"Failed {activity}; VISA session to this device is not open")
        End If
    End Sub

    ''' <summary> Attempts to measure resistance from the given data. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resistor"> The resistor. </param>
    ''' <param name="e">        Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryMeasureResistance(ByVal resistor As BridgeMeterResistor, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If resistor Is Nothing Then Throw New ArgumentNullException(NameOf(resistor))
        Dim activity As String = $"measuring {resistor.Title}"
        Try
            Me.MeasureResistance(resistor)
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Measure bridge. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    Public Sub MeasureBridge()
        Dim activity As String = $"Measuring bridge at {My.Settings.ResourceName}"
        If Me.IsDeviceOpen Then
            For Each resistor As BridgeMeterResistor In Me.Bridge
                activity = $"Measuring {resistor.Title} at {My.Settings.ResourceName}"
                Me.MeasureResistance(resistor)
            Next
            Me._DataGrid.Refresh()
        Else
            Throw New VI.Pith.OperationFailedException($"Failed {activity}; VISA session to this device is not open")
        End If
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryMeasureBridge(ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"Measuring bridge at {My.Settings.ResourceName}"
        Try
            Me.MeasureBridge()
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function



#End If
#End Region