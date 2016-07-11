Imports System.Windows.Forms
Imports isr.Core.Controls
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions

''' <summary> Wizard for setting the compensation. </summary>
''' <remarks> David, 7/8/2016. </remarks>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="7/8/2016" by="David" revision=""> Created. </history>
<CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
Public Class CompensationWizard
    Inherits isr.Core.Pith.FadeFormBase
    Implements ITalker

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private Property InitializingComponents As Boolean
    ''' <summary>
    ''' Creates a new instance of the <see cref="CompensationWizard"/> class.
    ''' </summary>
    Public Sub New()
        ' required for designer support
        Me.InitializingComponents = True
        Me.InitializeComponent()
        Me.InitializingComponents = False
        Me._Talker = New TraceMessageTalker
        Dim builder As New System.Text.StringBuilder
        builder.AppendLine("This wizard will guide you through the steps of performing a compensation task.")
        builder.AppendLine("The following items are required:")
        builder.AppendLine("-- An Open fixture for open compensation;")
        builder.AppendLine("-- A Short fixture for short compensation;")
        builder.AppendLine("-- A Load Resistance fixture for load compensation; and")
        builder.AppendLine("-- A Yardstick Resistance fixture for validation.")
        Me._WelcomeWizardPage.Description = builder.ToString

        Me._LoadResistanceNumeric.Value = My.Settings.LoadResistance
        Me._YardstickResistanceNumeric.Value = My.Settings.YardstickResistance
        Me._YardstickAcceptanceToleranceNumeric.Value = 100 * My.Settings.YardstickResistanceTolerance
        Me._YardstickInductanceLimitNumeric.Value = CDec(1000000.0 * My.Settings.YardstickInductanceLimit)
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                                                   release only unmanaged resources. </param>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed Then
                If disposing Then
                    Me.Device = Nothing
                    Me.Talker?.Listeners.Clear()
                    Me._Talker = Nothing
                    If components IsNot Nothing Then components.Dispose()
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " DEVICE AND SUBSYSTEMS "

    Private _device As Device

    ''' <summary> Gets or sets the device. </summary>
    ''' <value> The device. </value>
    Public Property Device As Device
        Get
            Return Me._device
        End Get
        Set(value As Device)
            If Me._device IsNot Nothing Then
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
            Me._device = value
            If Me._device IsNot Nothing Then
                AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
                AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
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
                Me.Device.SenseChannelSubsystem.ListAdapters(Me._AdapterComboBox)
                Me.Device.Publish()
            End If
        End Set
    End Property

#Region " SUBSYSTEMS "

#Region " CALCULATE "

    ''' <summary> Handle the Calculate channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As CalculateChannelSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.AveragingEnabled)
                Me._AveragingEnabledCheckBox.Checked = subsystem.AveragingEnabled.GetValueOrDefault(False)
            Case NameOf(subsystem.AverageCount)
                Me._AveragingCountNumeric.Value = subsystem.AverageCount.GetValueOrDefault(0)
        End Select
    End Sub

    ''' <summary> Calculate channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub CalculateChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, CalculateChannelSubsystemBase), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " COMPENSATE "

    ''' <summary> Handle the Compensate channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As CompensateChannelSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.HasCompleteCompensationValues)
                Dim value As String = ""
                If subsystem.HasCompleteCompensationValues Then
                    value = CompensateChannelSubsystem.Merge(subsystem.FrequencyArrayReading, subsystem.ImpedanceArrayReading)
                End If
                If subsystem.CompensationType = VI.CompensationTypes.OpenCircuit Then
                    Me._OpenCompensationValuesTextBox.Text = value
                ElseIf subsystem.CompensationType = VI.CompensationTypes.ShortCircuit Then
                    Me._ShortCompensationValuesTextBox.Text = value
                ElseIf subsystem.CompensationType = VI.CompensationTypes.Load Then
                    Me._LoadCompensationTextBox.Text = value
                End If
        End Select

        If subsystem.CompensationType = VI.CompensationTypes.OpenCircuit Then
            Select Case propertyName
                Case NameOf(subsystem.HasCompleteCompensationValues)
            End Select
        ElseIf subsystem.CompensationType = VI.CompensationTypes.ShortCircuit Then
            Select Case propertyName
                Case NameOf(subsystem.FrequencyArrayReading)
            End Select
        ElseIf subsystem.CompensationType = VI.CompensationTypes.Load Then
            Select Case propertyName
                Case NameOf(subsystem.FrequencyArrayReading)
                Case NameOf(subsystem.ModelResistance)
                    If subsystem.ModelResistance.HasValue Then Me._LoadResistanceNumeric.Value = CDec(subsystem.ModelResistance.Value)
            End Select
        End If
    End Sub

    ''' <summary> Compensate channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub CompensateChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, CompensateChannelSubsystemBase), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " CHANNEL MARKER "

    ''' <summary> Handle the channel marker subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As ChannelMarkerSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.MeasurementAvailable)
                If subsystem.MeasurementAvailable Then
                    Me.UpdateYardstickValues()
                End If
        End Select
    End Sub

    ''' <summary> Channel marker subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelmarkerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, ChannelMarkerSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " CHANNEL TRACE "

    ''' <summary> Handle the channel Trace subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As ChannelTraceSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            ' Case NameOf(subsystem.AdapterType)
        End Select
    End Sub

    ''' <summary> Channel Trace subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelTraceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, ChannelTraceSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " CHANNEL TRIGGER "

    ''' <summary> Handle the channel Trigger subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As ChannelTriggerSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            ' Case NameOf(subsystem.AdapterType)
        End Select
    End Sub

    ''' <summary> Channel Trigger subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ChannelTriggerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, ChannelTriggerSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " DISPLAY "

    ''' <summary> Handle the Display subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As DisplaySubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            ' Case NameOf(subsystem.Delay)
        End Select
    End Sub

    ''' <summary> Display subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplaySubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, DisplaySubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " SENSE "

    ''' <summary> Selects a new Adapter to display.
    ''' </summary>
    Friend Function SelectAdapter(ByVal value As VI.AdapterType) As VI.AdapterType
        If Me.InitializingComponents Then Return AdapterType.None
        If (value <> VI.AdapterType.None) AndAlso (value <> Me.SelectedAdapterType) Then
            Me._AdapterComboBox.SafeSelectItem(value.ValueDescriptionPair)
        End If
        Return Me.SelectedAdapterType
    End Function

    ''' <summary> Gets the type of the selected Adapter. </summary>
    ''' <value> The type of the selected Adapter. </value>
    Private ReadOnly Property SelectedAdapterType() As VI.AdapterType
        Get
            Return CType(CType(Me._AdapterComboBox.SelectedItem,
                               System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, VI.AdapterType)
        End Get
    End Property

    ''' <summary> Handle the sense channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As SenseChannelSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.AdapterType)
                If subsystem.AdapterType.HasValue Then Me.SelectAdapter(subsystem.AdapterType.Value)
        End Select
    End Sub

    ''' <summary> Sense channel subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseChannelSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, SenseChannelSubsystem), e?.PropertyName)
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
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As TriggerSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.Delay)
        End Select
    End Sub

    ''' <summary> Trigger subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TriggerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, TriggerSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Reports the last error. </summary>
    Private Sub onLastError(ByVal lastError As DeviceError)
        Me._StatusLabel.Text = lastError.CompoundErrorMessage
    End Sub

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.DeviceErrors)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.LastDeviceError)
                onLastError(subsystem.LastDeviceError)
            Case NameOf(subsystem.OperationCompleted)
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
    Private Overloads Sub OnPropertyChanged(ByVal subsystem As SystemSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Application.DoEvents()
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, SystemSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#End Region

#End Region

#Region " METHODS "

    ''' <summary>
    ''' Starts a sample task simulation.
    ''' </summary>
    Private Sub StartTask()
        ' setup wizard page
        Me._Wizard.BackEnabled = False
        Me._Wizard.NextEnabled = False
        Me._LongTaskProgressBar.Value = Me._LongTaskProgressBar.Minimum

        ' start timer to simulate a long running task
        Me._LongTaskTimer.Enabled = True
    End Sub

#End Region

#Region " CONTROL EVENTS HANDLERS "

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _RestartAveragingButton_Click(sender As Object, e As EventArgs) Handles _RestartAveragingButton.Click
        Try
            Me._InfoProvider.Clear()
            Me.Device.CalculateChannelSubsystem.ApplyAverageSettings(Me._AveragingEnabledCheckBox.Checked, CInt(Me._AveragingCountNumeric.Value))
            Me.Device.CalculateChannelSubsystem.ClearAverage()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred restarting average;. Details: {0}", ex)
        Finally
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplySettingsButton_Click(sender As Object, e As EventArgs) Handles _ApplySettingsButton.Click
        Try
            Me._InfoProvider.Clear()
            Me.Device.CalculateChannelSubsystem.ApplyAverageSettings(Me._AveragingEnabledCheckBox.Checked, CInt(Me._AveragingCountNumeric.Value))
            Me.Device.CalculateChannelSubsystem.ClearAverage()
            Me.Device.SenseChannelSubsystem.ApplyAdapterType(Me.SelectedAdapterType)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred applying settings;. Details: {0}", ex)
        Finally
        End Try
    End Sub

#Region " OPEN "

    ''' <summary> Acquires the open compensation. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    Private Sub AcquireOpenCompensation()

        ' Set user-specified frequencies
        Me.Device.SenseChannelSubsystem.ApplyFrequencyPointsType(FrequencyPointsType.User)

        ' Acquire open fixture compensation
        Me.Device.CompensateOpenSubsystem.AcquireMeasurements()

        ' Wait for measurement end
        Me.Device.StatusSubsystem.QueryOperationCompleted()

    End Sub

    ''' <summary> Reads open compensation. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    Private Sub ReadOpenCompensation()

        ' read the frequencies
        Me.Device.CompensateOpenSubsystem.QueryFrequencyArray()

        ' read the data
        Me.Device.CompensateOpenSubsystem.QueryImpedanceArray()

    End Sub

    ''' <summary> Acquires the open compensation button context menu strip changed. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AcquireOpenCompensationButton_ContextMenuStripChanged(sender As Object, e As EventArgs) Handles _AcquireOpenCompensationButton.ContextMenuStripChanged
        Try
            Me._InfoProvider.Clear()
            Me.Cursor = Cursors.WaitCursor
            Me.Device.ClearCompensations()
            Me.Device.ChannelMarkerSubsystem.Readings.Reset()
            Me.AcquireOpenCompensation()
            Me.ReadOpenCompensation()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred acquiring open compensation;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub


#End Region

#Region " SHORT "

    ''' <summary> Acquires the Short compensation. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    Private Sub AcquireShortCompensation()

        ' Set user-specified frequencies
        Me.Device.SenseChannelSubsystem.ApplyFrequencyPointsType(FrequencyPointsType.User)

        ' Acquire Short fixture compensation
        Me.Device.CompensateShortSubsystem.AcquireMeasurements()

        ' Wait for measurement end
        Me.Device.StatusSubsystem.QueryOperationCompleted()

    End Sub

    ''' <summary> Reads Short compensation. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    Private Sub ReadShortCompensation()

        ' read the frequencies
        Me.Device.CompensateShortSubsystem.QueryFrequencyArray()

        ' read the data
        Me.Device.CompensateShortSubsystem.QueryImpedanceArray()

    End Sub

    ''' <summary> Acquires the Short compensation button context menu strip changed. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AcquireShortCompensationButton_ContextMenuStripChanged(sender As Object, e As EventArgs) Handles _AcquireShortCompensationButton.ContextMenuStripChanged
        Try
            Me._InfoProvider.Clear()
            Me.Cursor = Cursors.WaitCursor
            Me.AcquireShortCompensation()
            Me.ReadShortCompensation()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred acquiring short compensation;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

#Region " LOAD "

    ''' <summary> Acquires the Load compensation. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    Private Sub AcquireLoadCompensation()

        ' Set user-specified frequencies
        Me.Device.SenseChannelSubsystem.ApplyFrequencyPointsType(FrequencyPointsType.User)

        Me.Device.CompensateLoadSubsystem.ApplyModelResistance(Me._LoadResistanceNumeric.Value)
        Me.Device.CompensateLoadSubsystem.ApplyModelCapacitance(0)
        Me.Device.CompensateLoadSubsystem.ApplyModelInductance(0)
        Me.Device.StatusSubsystem.QueryOperationCompleted()

        ' Acquire Load fixture compensation
        Me.Device.CompensateLoadSubsystem.AcquireMeasurements()

        ' Wait for measurement end
        Me.Device.StatusSubsystem.QueryOperationCompleted()

    End Sub

    ''' <summary> Reads Load compensation. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    Private Sub ReadLoadCompensation()

        ' read the frequencies
        Me.Device.CompensateLoadSubsystem.QueryFrequencyArray()

        ' read the data
        Me.Device.CompensateLoadSubsystem.QueryImpedanceArray()

    End Sub

    ''' <summary> Acquires the Load compensation button context menu strip changed. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AcquireLoadCompensationButton_ContextMenuStripChanged(sender As Object, e As EventArgs) Handles _AcquireLoadCompensationButton.ContextMenuStripChanged
        Try
            Me._InfoProvider.Clear()
            Me.Cursor = Cursors.WaitCursor
            Me.AcquireLoadCompensation()
            Me.ReadLoadCompensation()
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred acquiring Load compensation;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " YARDSTICK "

    Private Sub _YardstickAcceptanceToleranceNumeric_ValueChanged(sender As Object, e As EventArgs) Handles _YardstickAcceptanceToleranceNumeric.ValueChanged
        If Me.InitializingComponents Then Return
        Me.AnnunciateYardstickResult()
    End Sub

    Private Sub _YardstickInductanceLimitNumeric_ValueChanged(sender As Object, e As EventArgs) Handles _YardstickInductanceLimitNumeric.ValueChanged
        If Me.InitializingComponents Then Return
        Me.AnnunciateYardstickResult()
    End Sub

    ''' <summary> Gets the yardstick resistance validated. </summary>
    ''' <value> The yardstick resistance validated. </value>
    Private ReadOnly Property YardstickResistanceValidated As Boolean?
        Get
            If Me.Device.ChannelMarkerSubsystem.Readings.PrimaryReading.Value.HasValue Then
                Dim r As Double = Me.Device.ChannelMarkerSubsystem.Readings.PrimaryReading.Value.Value
                Dim delta As Double = 100 * (r / Me._YardstickResistanceNumeric.Value - 1)
                Return Math.Abs(delta) <= Me._YardstickAcceptanceToleranceNumeric.Value
            Else
                Return New Boolean?
            End If
        End Get
    End Property

    ''' <summary> Gets the yardstick impedance validated. </summary>
    ''' <value> The yardstick impedance validated. </value>
    Private ReadOnly Property YardstickInductanceValidated As Boolean?
        Get
            If Me.Device.ChannelMarkerSubsystem.Readings.SecondaryReading.Value.HasValue Then
                Dim l As Double = 1000000.0 * Me.Device.ChannelMarkerSubsystem.Readings.SecondaryReading.Value.Value
                Me._YardstickMeasuredInductanceTextBox.Text = String.Format("{0:0.###}", l)
                Return Math.Abs(l) <= Me._YardstickInductanceLimitNumeric.Value
            Else
                Return New Boolean?
            End If
        End Get
    End Property

    ''' <summary> Gets or sets the yardstick validated. </summary>
    ''' <value> The yardstick validated. </value>
    Private ReadOnly Property YardstickValidated As Boolean
        Get
            Return Me.YardstickResistanceValidated.GetValueOrDefault(False) AndAlso Me.YardstickInductanceValidated.GetValueOrDefault(False)
        End Get
    End Property

    ''' <summary> Updates the yardstick values. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    Private Sub UpdateYardstickValues()

        Me._InfoProvider.Clear()
        Me._YardstickValuesTextBox.Text = Me.Device.ChannelMarkerSubsystem.Readings.RawReading
        Dim r As Double = Me.Device.ChannelMarkerSubsystem.Readings.PrimaryReading.Value.GetValueOrDefault(0)
        Me._MeasuredYardstickResistanceTextBox.Text = String.Format("{0:0.###}", r)
        Dim delta As Double = 100 * (r / Me._YardstickResistanceNumeric.Value - 1)
        Me._YardstickResistanceDeviationTextBox.Text = String.Format("{0:0.##}", delta)

        Dim l As Double = 1000000.0 * Me.Device.ChannelMarkerSubsystem.Readings.SecondaryReading.Value.GetValueOrDefault(0)
        Me._YardstickMeasuredInductanceTextBox.Text = String.Format("{0:0.###}", l)

        Me.AnnunciateYardstickResult()

    End Sub

    ''' <summary> Annunciate yardstick result. </summary>
    ''' <remarks> David, 7/11/2016. </remarks>
    Private Sub AnnunciateYardstickResult()
        If Not Me.YardstickResistanceValidated Then
            Me._InfoProvider.Annunciate(Me._YardstickAcceptanceToleranceNumeric, InfoProviderLevel.Alert, "Exceeds minimum")
        End If
        If Not Me.YardstickInductanceValidated Then
            Me._InfoProvider.Annunciate(Me._YardstickInductanceLimitNumeric, InfoProviderLevel.Alert, "Exceeds minimum")
        End If
    End Sub

    ''' <summary> Measure yardstick button click. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MeasureYardstickButton_Click(sender As Object, e As EventArgs) Handles _MeasureYardstickButton.Click
        Try
            Me._InfoProvider.Clear()
            Me.Cursor = Cursors.WaitCursor
            Me.Device.ChannelMarkerSubsystem.ReadMarkerAverage(Me.Device)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred reading marker;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#End Region

#Region " WIZARD EVENTS HANDLERS "

    ''' <summary>
    ''' Handles the AfterSwitchPages event of the wizard form.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Wizard_AfterSwitchPages(ByVal sender As Object, ByVal e As PageChangedEventArgs) Handles _Wizard.PageChanged
        If Me._InitializingComponents Then Return
        Try
            Me._InfoProvider.Clear()
            Me.Cursor = Cursors.WaitCursor
            Select Case True
                Case Me._Wizard.NewPage Is Me._SettingsWizardPage
                Case Me._Wizard.NewPage Is Me._OpenWizardPage
                Case Me._Wizard.NewPage Is Me._ShortWizardPage
                Case Me._Wizard.NewPage Is Me._LoadWizardPage
                Case Me._Wizard.NewPage Is Me._YardstickWizardPage
                Case Me._Wizard.NewPage Is Me._FinishWizardPage
                Case Me._Wizard.NewPage Is Me._LicenseWizardPage
                    ' check if license page
                    ' sync next button's state with check box
                    Me._Wizard.NextEnabled = Me._AgreeCheckBox.Checked
                Case Me._Wizard.NewPage Is Me._ProgressWizardPage
                    ' check if progress page
                    ' start the sample task
                    Me.StartTask()
            End Select
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred after moving pages;. Details: {0}", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Handles the BeforeSwitchPages event of the wizard form.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Wizard_BeforeSwitchPages(ByVal sender As Object, ByVal e As PageChangingEventArgs) Handles _Wizard.PageChanging
        If Me._InitializingComponents Then Return
        If e Is Nothing Then Return
        Try
            Me._InfoProvider.Clear()
            Select Case True
                Case Me._Wizard.OldPage Is Me._SettingsWizardPage AndAlso e.Direction = Core.Controls.Direction.Next
                Case Me._Wizard.OldPage Is Me._OpenWizardPage
                Case Me._Wizard.OldPage Is Me._ShortWizardPage
                Case Me._Wizard.OldPage Is Me._LoadWizardPage
                Case Me._Wizard.OldPage Is Me._YardstickWizardPage
                Case Me._Wizard.OldPage Is Me._LicenseWizardPage
                Case Me._Wizard.OldPage Is Me._ProgressWizardPage
            End Select
            Me.Device.StatusSubsystem.QueryOperationCompleted()
            Me.Device.StatusSubsystem.ReadServiceRequestStatus()

            ' check if we're going forward from options page
            If Me._Wizard.OldPage Is Me._OptionsWizardPage AndAlso e.NewIndex > e.OldIndex Then
                ' check if user selected one option
                If Me._CheckOptionRadioButton.Checked = False AndAlso Me._SkipOptionRadioButton.Checked = False Then
                    ' display hint & cancel step
                    MessageBox.Show("Please chose one of the options presented.",
                                    Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                    e.Cancel = True
                    ' check if user chose to skip validation
                ElseIf Me._SkipOptionRadioButton.Checked Then
                    ' skip the validation page
                    e.NewIndex += 1
                End If
            End If
        Catch ex As Exception
            e.Cancel = True
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred before moving pages;. Details: {0}", ex)
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' Handles the Cancel event of the wizard form.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Wizard_Cancel(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _Wizard.Cancel
        If Me._InitializingComponents Then Return
        Try
            Me._InfoProvider.Clear()
            ' check if task is running
            Dim isTaskRunning As Boolean = Me._LongTaskTimer.Enabled
            ' stop the task
            Me._LongTaskTimer.Enabled = False

            ' ask user to confirm
            If MessageBox.Show($"Are you sure you wand to exit {Me.Text}?", Me.Text,
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,
                               MessageBoxOptions.DefaultDesktopOnly) <> System.Windows.Forms.DialogResult.Yes Then
                ' cancel closing
                e.Cancel = True
                ' restart the task
                Me._LongTaskTimer.Enabled = isTaskRunning
            Else
                ' ensure parent form is closed (even when ShowDialog is not used)
                Me.DialogResult = DialogResult.Cancel
                Me.Close()
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred canceling the wizard;. Details: {0}", ex)
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' Handles the Finish event of the wizard form.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Wizard_Finish(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Wizard.Finish
        If Me._InitializingComponents Then Return
        Try
            Me._InfoProvider.Clear()
            If Me.YardstickValidated Then
                MessageBox.Show($"The {Me.Text} finished successfully.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                ' ensure parent form is closed (even when ShowDialog is not used)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                If MessageBox.Show($"The {Me.Text} yardstick was not validated or is out of range. Are you sure?", $"{Me.Text} Save Data; Are you sure?",
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2,
                                   MessageBoxOptions.DefaultDesktopOnly) = DialogResult.Yes Then
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                End If
            End If
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred closing the wizard;. Details: {0}", ex)
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' Handles the Help event of the wizard form.
    ''' </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Wizard_Help(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Wizard.Help
        If Me._InitializingComponents Then Return
        Try
            Me._InfoProvider.Clear()
            Dim text As String = $"This is a really cool wizard control!{Environment.NewLine}:-)"
            Select Case True
                Case Me._Wizard.NewPage Is Me._SettingsWizardPage
                    text = My.Resources.SettingsHelp
                Case Me._Wizard.NewPage Is Me._OpenWizardPage
                    text = My.Resources.OpenHelp
                Case Me._Wizard.NewPage Is Me._ShortWizardPage
                    text = My.Resources.ShortHelp
                Case Me._Wizard.NewPage Is Me._LoadWizardPage
                    text = My.Resources.LoadHelp
                Case Me._Wizard.NewPage Is Me._YardstickWizardPage
                    text = My.Resources.YardstickHelp
                Case Me._Wizard.NewPage Is Me._FinishWizardPage
                    text = My.Resources.FinishHelp
                Case Me._Wizard.NewPage Is Me._LicenseWizardPage
                Case Me._Wizard.NewPage Is Me._ProgressWizardPage
            End Select
            MessageBox.Show(text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
        Catch ex As Exception
            Me._InfoProvider.Annunciate(sender, InfoProviderLevel.Error, ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception occurred showing help;. Details: {0}", ex)
        Finally
        End Try
    End Sub

#End Region

#Region " SIMULATION EVENT HANDLERS "

    ''' <summary>
    ''' Handles the CheckedChanged event of checkIAgree.
    ''' </summary>
    Private Sub _AgreeCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _AgreeCheckBox.CheckedChanged
        If Me.InitializingComponents Then Return
        ' sync next button's state with check box
        Me._Wizard.NextEnabled = Me._AgreeCheckBox.Checked
    End Sub

    ''' <summary>
    ''' Handles the Tick event of timerTask.
    ''' </summary>
    Private Sub _LongTaskTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _LongTaskTimer.Tick
        If Me.InitializingComponents Then Return
        ' check if task completed
        If Me._LongTaskProgressBar.Value = Me._LongTaskProgressBar.Maximum Then
            ' stop the timer & switch to next page
            Me._LongTaskTimer.Enabled = False
            Me._Wizard.Next()
        Else
            ' update progress bar
            Me._LongTaskProgressBar.PerformStep()
        End If
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Gets the trace message talker. </summary>
    ''' <value> The trace message talker. </value>
    Public ReadOnly Property Talker As ITraceMessageTalker

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="listeners"> The listeners. </param>
    Public Overridable Sub AddListeners(ByVal listeners As IEnumerable(Of ITraceMessageListener)) Implements ITalker.AddListeners
        Me.Talker.Listeners.Add(listeners)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="log"> The log. </param>
    Public Overridable Sub AddListeners(ByVal log As MyLog)
        If log Is Nothing Then Throw New ArgumentNullException(NameOf(log))
        Me.Talker.Listeners.Add(log)
    End Sub

    ''' <summary> Clears the listeners. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    Public Sub ClearListeners() Implements ITalker.ClearListeners
        Me.Talker.Listeners.Clear()
    End Sub

#End Region

End Class

#Region " UNUSED "

#If False Then
#Region " METHODS "

    ''' <summary>
    ''' Starts a sample task simulation.
    ''' </summary>
    Private Sub StartTask()
        ' setup wizard page
        Me._Wizard.BackEnabled = False
        Me._Wizard.NextEnabled = False
        Me._LongTaskProgressBar.Value = Me._LongTaskProgressBar.Minimum

        ' start timer to simulate a long running task
        Me._LongTaskTimer.Enabled = True
    End Sub

#End Region

#Region " WIZARD EVENTS HANDLERS "

    ''' <summary>
    ''' Handles the AfterSwitchPages event of the wizard form.
    ''' </summary>
    Private Sub _Wizard_AfterSwitchPages(ByVal sender As Object, ByVal e As PageChangedEventArgs) Handles _Wizard.PageChanged

        ' check if license page
        If Me._Wizard.NewPage Is Me._LicenseWizardPage Then
            ' sync next button's state with check box
            Me._Wizard.NextEnabled = Me._AgreeCheckBox.Checked
            ' check if progress page
        ElseIf Me._Wizard.NewPage Is Me._ProgressWizardPage Then
            ' start the sample task
            Me.StartTask()
        End If
    End Sub

    ''' <summary>
    ''' Handles the BeforeSwitchPages event of the wizard form.
    ''' </summary>
    Private Sub _Wizard_BeforeSwitchPages(ByVal sender As Object, ByVal e As PageChangingEventArgs) Handles _Wizard.PageChanging

        ' check if we're going forward from options page
        If Me._Wizard.OldPage Is Me._OptionsWizardPage AndAlso e.NewIndex > e.OldIndex Then
            ' check if user selected one option
            If Me._CheckOptionRadioButton.Checked = False AndAlso Me._SkipOptionRadioButton.Checked = False Then
                ' display hint & cancel step
                MessageBox.Show("Please chose one of the options presented.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                e.Cancel = True
                ' check if user chose to skip validation
            ElseIf Me._SkipOptionRadioButton.Checked Then
                ' skip the validation page
                e.NewIndex += 1
            End If
        End If
    End Sub

    ''' <summary>
    ''' Handles the Cancel event of the wizard form.
    ''' </summary>
    Private Sub _Wizard_Cancel(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _Wizard.Cancel
        ' check if task is running
        Dim isTaskRunning As Boolean = Me._LongTaskTimer.Enabled
        ' stop the task
        Me._LongTaskTimer.Enabled = False

        ' ask user to confirm
        If MessageBox.Show("Are you sure you wand to exit the Sample Wizard?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> System.Windows.Forms.DialogResult.Yes Then
            ' cancel closing
            e.Cancel = True
            ' restart the task
            Me._LongTaskTimer.Enabled = isTaskRunning
        Else
            ' ensure parent form is closed (even when ShowDialog is not used)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    ''' <summary>
    ''' Handles the Finish event of the wizard form.
    ''' </summary>
    Private Sub _Wizard_Finish(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Wizard.Finish
        MessageBox.Show("The Sample Wizard finished successfully.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        ' ensure parent form is closed (even when ShowDialog is not used)
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    ''' <summary>
    ''' Handles the Help event of the wizard form.
    ''' </summary>
    Private Sub _Wizard_Help(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Wizard.Help
        MessageBox.Show("This is a really cool wizard control!" & Environment.NewLine & ":-)", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#End Region

#Region " SIMULATION EVENT HANDLERS "

    ''' <summary>
    ''' Handles the CheckedChanged event of checkIAgree.
    ''' </summary>
    Private Sub _AgreeCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _AgreeCheckBox.CheckedChanged
        If Me.InitializingComponents Then Return
        ' sync next button's state with check box
        Me._Wizard.NextEnabled = Me._AgreeCheckBox.Checked
    End Sub

    ''' <summary>
    ''' Handles the Tick event of timerTask.
    ''' </summary>
    Private Sub _LongTaskTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _LongTaskTimer.Tick
        If Me.InitializingComponents Then Return
        ' check if task completed
        If Me._LongTaskProgressBar.Value = Me._LongTaskProgressBar.Maximum Then
            ' stop the timer & switch to next page
            Me._LongTaskTimer.Enabled = False
            Me._Wizard.Next()
        Else
            ' update progress bar
            Me._LongTaskProgressBar.PerformStep()
        End If
    End Sub

#End Region

#End If
#End Region