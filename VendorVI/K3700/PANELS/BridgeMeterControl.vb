Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.ControlExtensions
Imports isr.Core.Controls.DataGridViewExtensions
Imports isr.Core.Controls.SafeSetterExtensions
Imports isr.Core.Controls.ToolStripExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.VI.Tsp.K3700
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

    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
    Private Property InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        MyBase.New()
        Me._New(Device.Create)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Public Sub New(ByVal device As Device)
        MyBase.New()
        Me._New(device)
        Me.IsDeviceOwner = False
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The device. </param>
    Private Sub _New(ByVal device As Device)
        Me.InitializingComponents = True
        ' This call is required by the designer.
        InitializeComponent()
        Me.InitializingComponents = False

        Me._BottomToolStrip.Renderer = New CustomProfessionalRenderer
        MyBase.Connector = Me._ResourceSelectorConnector

        Me._AssignDevice(device)

        Me._Bridge = New ResistorCollection
        Me.Bridge.DisplayValues(Me._DataGrid)

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
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignDevice(ByVal value As Device)
        Me._Device = value
        If value IsNot Nothing Then
            value.CaptureSyncContext(WindowsFormsSynchronizationContext.Current)
        End If
        ' the device base class addresses the device open state.
        MyBase.DeviceBase = value
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device)
        Me.IsDeviceOwner = False
        Me.Device = value
    End Sub

    ''' <summary> Releases the device. </summary>
    ''' <remarks> Called from the base device to release the reference to the device. </remarks>
    Protected Overrides Sub ReleaseDevice()
        MyBase.ReleaseDevice()
        Me._Device = Nothing
    End Sub

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As DeviceBase)
        Me._ConfigureGroupBox.Enabled = Me.IsDeviceOpen 
        Me._MeasureGroupBox.Enabled = Me.IsDeviceOpen
        Me._MeasureGroupBox.Enabled = Me.IsDeviceOpen AndAlso
                                        Me.Device.MultimeterSubsystem.FunctionMode.HasValue AndAlso
                                        Me.Device.MultimeterSubsystem.FunctionMode.Value = MultimeterFunctionMode.ResistanceFourWire
        If Me.IsDeviceOpen Then
            Me.Bridge.DisplayValues(Me._DataGrid)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.Device.StatusSubsystem.VersionInfo.Model} is open")
            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            If Not Me.TryConfigureMeter(e) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Files configuring meter;. Details: {e.Details}")
            End If
        End If
    End Sub

    ''' <summary> Handles the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnDevicePropertyChanged(ByVal device As DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnDevicePropertyChanged(device, propertyName)
        Select Case propertyName
            Case NameOf(device.ResourceName)
                Me._ReadingLabel.Text = Me.ResourceName
        End Select
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
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
    Protected Overrides Sub DeviceClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
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

#End Region

#Region " SUBSYSTEMS "

#Region " MUTLIMETER "

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

    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property LastReading As String

    ''' <summary> Displays a reading described by subsystem. </summary>
    ''' <param name="subsystem"> The subsystem. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub DisplayReading(ByVal subsystem As MultimeterSubsystem)

        Const clear As String = "    "
        Const compliance As String = "..C.."
        Const rangeCompliance As String = ".RC."
        Const levelCompliance As String = ".LC."

        If Not subsystem.Reading.HasValue Then
            Me._ReadingLabel.SafeTextSetter($"-.---- {subsystem.Amount.Unit.ToString}")
            Me._ComplianceLabel.Text = clear
            Me.LastReading = ""
        Else
            Me.LastReading = subsystem.Amount.ToString
            Me._ReadingLabel.SafeTextSetter($"{subsystem.Amount.ToString} {subsystem.Amount.Unit.ToString}")
            If subsystem.Amount.MetaStatus.HitCompliance Then
                Me._ComplianceLabel.Text = compliance
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Real Compliance;. Instrument sensed an output overflow of the measured value.")
            ElseIf subsystem.Amount.MetaStatus.HitRangeCompliance Then
                Me._ComplianceLabel.Text = rangeCompliance
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Range Compliance;. Instrument sensed an output overflow of the measured value.")
            ElseIf subsystem.Amount.MetaStatus.HitLevelCompliance Then
                Me._ComplianceLabel.Text = levelCompliance
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Level Compliance;. Instrument sensed an output overflow of the measured value.")
            Else
                Me._ComplianceLabel.Text = clear
                ' Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Parsed {0}", Me.LastReading)
            End If
        End If

    End Sub

    ''' <summary> Handles the Multimeter subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As MultimeterSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.FilterCount)
                If subsystem.FilterCount.HasValue Then Me._FilterCountNumeric.Value = subsystem.FilterCount.Value
            Case NameOf(subsystem.FilterCountRange)
                With Me._FilterCountNumeric
                    .Maximum = CDec(subsystem.FilterCountRange.Max)
                    .Minimum = CDec(subsystem.FilterCountRange.Min)
                    .DecimalPlaces = 0
                End With
            Case NameOf(subsystem.FilterWindow)
                'If subsystem.FilterWindow.HasValue Then Me._FilterWindowNumeric.Value = CDec(100 * subsystem.FilterWindow.Value)
            Case NameOf(subsystem.FunctionMode)
                Me._MeasureGroupBox.Enabled = Me.IsDeviceOpen AndAlso
                                                Me.Device.MultimeterSubsystem.FunctionMode.HasValue AndAlso
                                                Me.Device.MultimeterSubsystem.FunctionMode.Value = MultimeterFunctionMode.ResistanceFourWire
            Case NameOf(subsystem.PowerLineCycles)
                If subsystem.PowerLineCycles.HasValue Then Me._PowerLineCyclesNumeric.Value = CDec(subsystem.PowerLineCycles.Value)
            Case NameOf(subsystem.PowerLineCyclesRange)
                With Me._PowerLineCyclesNumeric
                    .Maximum = CDec(subsystem.PowerLineCyclesRange.Max)
                    .Minimum = CDec(subsystem.PowerLineCyclesRange.Min)
                    .DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces
                End With
            Case NameOf(subsystem.Reading)
                Me.DisplayReading(subsystem)
        End Select
    End Sub

    ''' <summary> Multimeter subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MultimeterSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As MultimeterSubsystem = TryCast(sender, MultimeterSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Multimeter subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " CHANNEL "

    ''' <summary> Handles the Channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As ChannelSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.ClosedChannels)
                ' Me.ClosedChannels = subsystem.ClosedChannels
        End Select
    End Sub

    ''' <summary> Channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As ChannelSubsystem = TryCast(sender, ChannelSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Channel subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " DISPLAY "

    ''' <summary> Handles the Display subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As DisplaySubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.DisplayScreen)
        End Select
    End Sub

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplaySubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As DisplaySubsystem = TryCast(sender, DisplaySubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Display subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
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
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
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
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As StatusSubsystem = TryCast(sender, StatusSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling Status subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
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
        Dim subsystem As SystemSubsystem = TryCast(sender, SystemSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSubsystemPropertyChanged(subsystem, e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "{0} exception handling System subsystem {1} property change;. {2}",
                               Me.Name, e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#End Region

#Region " MEASURE "

    ''' <summary> Gets or sets the bridges. </summary>
    ''' <value> The bridge. </value>
    Public ReadOnly Property Bridge As ResistorCollection

    ''' <summary> Configure meter. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    Public Sub ConfigureMeter()
        Dim action As String = $"Checking {My.Settings.ResourceName} is open"
        If Me.IsDeviceOpen Then
            action = $"Configuring function mode {MultimeterFunctionMode.ResistanceFourWire}"
            Dim expectedMeasureFunction As MultimeterFunctionMode = MultimeterFunctionMode.ResistanceFourWire
            Dim measureFunction As MultimeterFunctionMode? = Device.MultimeterSubsystem.ApplyFunctionMode(expectedMeasureFunction).GetValueOrDefault(MultimeterFunctionMode.ResistanceTwoWire)
            If measureFunction.HasValue Then
                If expectedMeasureFunction <> measureFunction.Value Then
                    Throw New OperationFailedException($"Failed {action} Expected {expectedMeasureFunction } <> Actual {measureFunction.Value}")
                End If
            Else
                Throw New OperationFailedException($"Failed {action}--no value set")
            End If
            Dim expectedPowerLineCycles As Double = Me._PowerLineCyclesNumeric.Value
            action = $"Configuring power line cycles {expectedPowerLineCycles}"
            Dim actualPowerLineCycles As Double? = Device.MultimeterSubsystem.ApplyPowerLineCycles(expectedPowerLineCycles)
            If actualPowerLineCycles.HasValue Then
                If expectedPowerLineCycles <> actualPowerLineCycles.Value Then
                    Throw New OperationFailedException($"Failed {action} Expected {expectedPowerLineCycles} <> Actual {actualPowerLineCycles.Value}")
                End If
            Else
                Throw New OperationFailedException($"Failed {action}--no value set")
            End If
        Else
            Throw New OperationFailedException($"Failed {action}; VISA session to this device is not open")
        End If
    End Sub


    ''' <summary> Attempts to configure meter from the given data. </summary>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryConfigureMeter(ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim action As String = $"Configuring bridge meter {My.Settings.ResourceName}"
        Try
            Me.ConfigureMeter()
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {action};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Measure resistance. </summary>
    ''' <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    '''                                             null. </exception>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="resistor"> The resistor. </param>
    Public Sub MeasureResistance(ByVal resistor As Resistor)
        If resistor Is Nothing Then Throw New ArgumentNullException(NameOf(resistor))
        Dim action As String = $"measuring {resistor.Title}"
        If Me.Device.IsDeviceOpen Then
            action = $"{resistor.Title} opening channels"
            Dim expectedChannelList As String = ""
            Dim actualChannelList As String = Me.Device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
            If expectedChannelList <> actualChannelList Then
                Throw New OperationFailedException($"Failed {action} Expected {expectedChannelList } <> Actual {actualChannelList}")
            End If

            action = $"{resistor.Title} closing {resistor.ChannelList}"
            expectedChannelList = resistor.ChannelList
            actualChannelList = Device.ChannelSubsystem.ApplyClosedChannels(expectedChannelList, TimeSpan.FromSeconds(2))
            If expectedChannelList <> actualChannelList Then
                Throw New OperationFailedException($"Failed {action} Expected {expectedChannelList } <> Actual {actualChannelList}")
            End If

            action = $"measuring {resistor.Title}"
            If Device.MultimeterSubsystem.Measure.HasValue Then
                resistor.Resistance = Device.MultimeterSubsystem.Reading.GetValueOrDefault(-1)
            Else
                Throw New OperationFailedException($"Failed {action}; driver returned nothing")
            End If
        Else
            Throw New OperationFailedException($"Failed {action}; VISA session to this device is not open")
        End If
    End Sub

    ''' <summary> Attempts to measure resistance from the given data. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resistor"> The resistor. </param>
    ''' <param name="e">        Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryMeasureResistance(ByVal resistor As Resistor, ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If resistor Is Nothing Then Throw New ArgumentNullException(NameOf(resistor))
        Dim action As String = $"measuring {resistor.Title}"
        Try
            Me.MeasureResistance(resistor)
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {action};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Measure bridge. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    Public Sub MeasureBridge()
        Dim action As String = $"Measuring bridge at {My.Settings.ResourceName}"
        If Me.IsDeviceOpen Then
            For Each resistor As Resistor In Me.Bridge
                action = $"Measuring {resistor.Title} at {My.Settings.ResourceName}"
                Me.MeasureResistance(resistor)
            Next
            Me._DataGrid.Refresh()
        Else
            Throw New OperationFailedException($"Failed {action}; VISA session to this device is not open")
        End If
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryMeasureBridge(ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim action As String = $"Measuring bridge at {My.Settings.ResourceName}"
        Try
            Me.MeasureBridge()
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {action};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

#End Region

#Region " CONTROL EVENTS "

    ''' <summary> Measure button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _MeasureButton_Click(sender As Object, e As EventArgs) Handles _MeasureButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.TryMeasureBridge(New isr.Core.Pith.CancelDetailsEventArgs)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Configure button click. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ConfigureButton_Click(sender As Object, e As EventArgs) Handles _ConfigureButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.TryConfigureMeter(New isr.Core.Pith.CancelDetailsEventArgs)
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

''' <summary> A resistor. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/13/2018" by="David" revision=""> Created. </history>
Public Class Resistor
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
    Public Sub New(ByVal title As String, ByVal channelList As String)
        MyBase.New()
        Me.Title = title
        Me.ChannelList = channelList
    End Sub
    Public ReadOnly Property Title As String
    Public ReadOnly Property ChannelList As String
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
    Public Property Resistance As Double
End Class

''' <summary> Bridge: an ordered collection of resistors. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/13/2018" by="David" revision=""> Created. </history>
Public Class ResistorCollection
    Inherits ObjectModel.KeyedCollection(Of String, Resistor)

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New
        Me.AddResistor("R1", My.Settings.R1ChannelList)
        Me.AddResistor("R2", My.Settings.R2ChannelList)
        Me.AddResistor("R3", My.Settings.R3ChannelList)
        Me.AddResistor("R4", My.Settings.R4ChannelList)
    End Sub

    ''' <summary>
    ''' When implemented in a derived class, extracts the key from the specified element.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="item"> The element from which to extract the key. </param>
    ''' <returns> The key for the specified element. </returns>
    Protected Overrides Function GetKeyForItem(item As Resistor) As String
        If item Is Nothing Then Throw New ArgumentNullException(NameOf(item))
        Return item.Title
    End Function

    ''' <summary> Adds a new resistor. </summary>
    ''' <param name="title">       The title. </param>
    ''' <param name="channelList"> List of channels. </param>
    Public Sub AddResistor(ByVal title As String, ByVal channelList As String)
        Me.Add(New Resistor(title, channelList))
    End Sub

    ''' <summary> Configure display values. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    Public Function ConfigureDisplayValues(ByVal grid As DataGridView) As Integer

        If grid Is Nothing Then Throw New ArgumentNullException("grid")

        Dim wasEnabled As Boolean = grid.Enabled
        grid.Enabled = False
        grid.Enabled = wasEnabled

        grid.Enabled = False
        grid.DataSource = Nothing
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSalmon
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        grid.AutoGenerateColumns = False
        grid.RowHeadersVisible = False
        grid.ReadOnly = True
        grid.DataSource = Me.ToArray

        grid.Columns.Clear()
        grid.Refresh()
        Dim displayIndex As Integer = 0
        Dim width As Integer = 30
        Dim column As DataGridViewTextBoxColumn = Nothing
        Try
            column = New DataGridViewTextBoxColumn()
            With column
                .DataPropertyName = "Title"
                .Name = "Title"
                .Visible = True
                .DisplayIndex = displayIndex
            End With
            grid.Columns.Add(column)
            width += column.Width

            displayIndex += 1
            column = New DataGridViewTextBoxColumn()
            With column
                .DataPropertyName = "Resistance"
                .Name = "Resistance"
                .Visible = True
                .DisplayIndex = displayIndex
                .Width = grid.Width - width
                .DefaultCellStyle.Format = "G5"
            End With
        Catch
            If column IsNot Nothing Then column.Dispose()
            Throw
        End Try
        grid.Columns.Add(column)
        grid.ParseHeaderText()
        grid.Enabled = True
        If grid.Columns IsNot Nothing AndAlso grid.Columns.Count > 0 Then
            Return grid.Columns.Count
        Else
            Return 0
        End If

    End Function

    ''' <summary> Displays the values described by grid. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    Public Function DisplayValues(ByVal grid As DataGridView) As Integer

        If grid Is Nothing Then Throw New ArgumentNullException("grid")

        Dim wasEnabled As Boolean = grid.Enabled
        grid.Enabled = False
        grid.Enabled = wasEnabled

        If grid.DataSource Is Nothing Then
            Me.ConfigureDisplayValues(grid)
            Application.DoEvents()
        End If

        grid.DataSource = Me
        Application.DoEvents()
        Return grid.Columns.Count

    End Function

    ''' <summary> Configure display. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    Public Function ConfigureDisplay(ByVal grid As DataGridView) As Integer

        If grid Is Nothing Then Throw New ArgumentNullException("grid")

        Dim wasEnabled As Boolean = grid.Enabled
        grid.Enabled = False
        grid.Enabled = wasEnabled

        grid.Enabled = False
        grid.DataSource = Nothing
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSalmon
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        grid.AutoGenerateColumns = True
        grid.RowHeadersVisible = False
        grid.ReadOnly = True
        grid.DataSource = Me.ToArray
        grid.ParseHeaderText()
        grid.Enabled = True
        If grid.Columns IsNot Nothing AndAlso grid.Columns.Count > 0 Then
            Return grid.Columns.Count
        Else
            Return 0
        End If

    End Function

    ''' <summary> Displays the given grid. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    Public Function Display(ByVal grid As DataGridView) As Integer

        If grid Is Nothing Then Throw New ArgumentNullException("grid")

        Dim wasEnabled As Boolean = grid.Enabled
        grid.Enabled = False
        grid.Enabled = wasEnabled

        If grid.DataSource Is Nothing Then
            Me.ConfigureDisplay(grid)
            Application.DoEvents()
        End If

        grid.DataSource = Me
        Application.DoEvents()
        Return grid.Columns.Count

    End Function
End Class

