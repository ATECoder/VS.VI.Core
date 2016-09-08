Imports System.Windows.Forms
Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.NumericExtensions
''' <summary> Panel for editing the configuration. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="2/25/2014" by="David" revision=""> Created. </history>
Public Class ConfigurationPanelBase
    Inherits TalkerControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> A private constructor for this class making it not publicly creatable. This ensure
    ''' using the class as a singleton. </summary>
    Protected Sub New()
        MyBase.New()
        Me.InitializeComponent()
        Me.ApplyConfigurationButtonCaption = Me._ApplyConfigurationButton.Text
        Me.ApplyNewConfigurationButtonCaption = Me._ApplyNewConfigurationButton.Text
    End Sub

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                Me.ReleaseResources()
                If components IsNot Nothing Then
                    components.Dispose()
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " ON CONTROL EVENTS "

    ''' <summary> Releases the resources. </summary>
    Protected Overridable Sub ReleaseResources()
        Me._DeviceUnderTest = Nothing
        Me._Part = Nothing
    End Sub

#End Region

#Region " PART "

    ''' <summary> The Part. </summary>
    Private WithEvents _Part As DeviceUnderTest

    ''' <summary> Gets or sets the part. </summary>
    ''' <value> The part. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Part As DeviceUnderTest
        Get
            Return Me._Part
        End Get
        Set(ByVal value As DeviceUnderTest)
            Me._Part = value
            If value IsNot Nothing Then
                Me.BindIntialResitanceControls()
                Me.BindThermalTransientControls()
            End If
        End Set
    End Property

    ''' <summary> Executes the property changed action. </summary>
    ''' <remarks> David, 1/6/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OnPropertyChanged(ByVal sender As DeviceUnderTest, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.InitialResistance)
                Me.onConfigurationChanged()
            Case NameOf(sender.FinalResistance)
                Me.onConfigurationChanged()
            Case NameOf(sender.ShuntResistance)
            Case NameOf(sender.ThermalTransient)
                Me.onConfigurationChanged()
        End Select
    End Sub

    ''' <summary> Event handler. Called by _InitialResistance for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _part_PropertyChanged(ByVal sender As System.Object, ByVal e As PropertyChangedEventArgs) Handles _Part.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._part_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, DeviceUnderTest), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling {0} property change;. Details: {1}",
                               e?.PropertyName, ex)
        End Try
    End Sub

    ''' <summary> Bind controls. </summary>
    Private Sub BindIntialResitanceControls()

        ' set the GUI based on the current defaults.
        Dim instrumentSettings As My.MySettings = My.MySettings.Default

        With Me._CheckContactsCheckBox
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Checked", Me.Part, "ContactCheckEnabled", True, DataSourceUpdateMode.OnPropertyChanged))
            .Checked = My.Settings.ContactCheckEnabled
            Me.Part.ContactCheckEnabled = .Checked
        End With
        Windows.Forms.Application.DoEvents()

        With Me._ColdVoltageNumeric
            .Minimum = instrumentSettings.ColdResistanceVoltageMinimum
            .Maximum = instrumentSettings.ColdResistanceVoltageMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.InitialResistance, "VoltageLimit", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.Settings.ColdResistanceVoltageLimit.Clip(.Minimum, .Maximum)
            Me.Part.InitialResistance.VoltageLimit = .Value
        End With
        Windows.Forms.Application.DoEvents()

        With Me._ColdCurrentNumeric
            .Minimum = instrumentSettings.ColdResistanceCurrentMinimum
            .Maximum = instrumentSettings.ColdResistanceCurrentMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.InitialResistance, "CurrentLevel", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.Settings.ColdResistanceCurrentLevel.Clip(.Minimum, .Maximum)
            Me.Part.InitialResistance.CurrentLevel = .Value
        End With

        With Me._ColdResistanceHighLimitNumeric
            .Minimum = instrumentSettings.ColdResistanceMinimum
            .Maximum = instrumentSettings.ColdResistanceMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.InitialResistance, "HighLimit", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.Settings.ColdResistanceHighLimit.Clip(.Minimum, .Maximum)
            Me.Part.InitialResistance.HighLimit = .Value
        End With

        With Me._ColdResistanceLowLimitNumeric
            .Minimum = instrumentSettings.ColdResistanceMinimum
            .Maximum = instrumentSettings.ColdResistanceMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.InitialResistance, "LowLimit", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.Settings.ColdResistanceLowLimit.Clip(.Minimum, .Maximum)
            Me.Part.InitialResistance.LowLimit = .Value
        End With

    End Sub

    ''' <summary> Bind controls. </summary>
    Private Sub BindThermalTransientControls()

        ' set the GUI based on the current defaults.
        Dim instrumentSettings As My.MySettings = My.MySettings.Default

        With Me._PostTransientDelayNumeric
            .Minimum = instrumentSettings.PostTransientDelayMinimum
            .Maximum = instrumentSettings.PostTransientDelayMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ThermalTransient, "PostTransientDelay", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.Settings.PostTransientDelay.Clip(.Minimum, .Maximum)
            Me.Part.ThermalTransient.PostTransientDelay = .Value
        End With

        With Me._ThermalVoltageNumeric
            .Minimum = instrumentSettings.ThermalTransientVoltageMinimum
            .Maximum = instrumentSettings.ThermalTransientVoltageMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ThermalTransient, "AllowedVoltageChange", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.Settings.ThermalTransientVoltageChange.Clip(.Minimum, .Maximum)
            Me.Part.ThermalTransient.AllowedVoltageChange = .Value
        End With

        With Me._ThermalCurrentNumeric
            .Minimum = instrumentSettings.ThermalTransientCurrentMinimum
            .Maximum = instrumentSettings.ThermalTransientCurrentMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ThermalTransient, "CurrentLevel", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.Settings.ThermalTransientCurrentLevel.Clip(.Minimum, .Maximum)
            Me.Part.ThermalTransient.CurrentLevel = .Value
        End With

        With Me._ThermalVoltageLimitNumeric
            .Minimum = instrumentSettings.ThermalTransientVoltageMinimum
            .Maximum = instrumentSettings.ThermalTransientVoltageMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ThermalTransient, "VoltageLimit", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.Settings.ThermalTransientVoltageLimit.Clip(.Minimum, .Maximum)
            Me.Part.ThermalTransient.VoltageLimit = .Value
        End With

        With Me._ThermalVoltageHighLimitNumeric
            .Minimum = instrumentSettings.ThermalTransientVoltageChangeMinimum
            .Maximum = instrumentSettings.ThermalTransientVoltageChangeMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ThermalTransient, "HighLimit", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.Settings.ThermalTransientHighLimit.Clip(.Minimum, .Maximum)
            Me.Part.ThermalTransient.HighLimit = .Value
        End With

        With Me._ThermalVoltageLowLimitNumeric
            .Minimum = instrumentSettings.ThermalTransientVoltageChangeMinimum
            .Maximum = instrumentSettings.ThermalTransientVoltageChangeMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ThermalTransient, "LowLimit", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.Settings.ThermalTransientLowLimit.Clip(.Minimum, .Maximum)
            Me.Part.ThermalTransient.LowLimit = .Value
        End With

    End Sub

#End Region

#Region " DEVICE UNDER TEST "

    ''' <summary> Gets or sets the device under test. </summary>
    ''' <value> The device under test. </value>
    ''' <remroks> This is the element representing the actual meter settings. </remroks>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property DeviceUnderTest As DeviceUnderTest

#End Region

#Region " CONFIGURE "

    ''' <summary> Restore defaults. </summary>
    Private Shared Sub RestoreDefaults()
        With My.MySettings.Default
            .ContactCheckEnabled = .ContactCheckEnabledDefault
            .ContactCheckThreshold = CInt(.ContactCheckThresholdDefault)

            .ColdResistanceVoltageLimit = .ColdResistanceVoltageLimitDefault
            .ColdResistanceCurrentLevel = .ColdResistanceCurrentLevelDefault
            .ColdResistanceHighLimit = .ColdResistanceHighLimitDefault
            .ColdResistanceLowLimit = .ColdResistanceLowLimitDefault

            .PostTransientDelay = .PostTransientDelayDefault
            .ThermalTransientVoltageChange = .ThermalTransientVoltageChangeDefault
            .ThermalTransientCurrentLevel = .ThermalTransientCurrentLevelDefault
            .ThermalTransientHighLimit = .ThermalTransientHighLimitDefault
            .ThermalTransientLowLimit = .ThermalTransientLowLimitDefault
            .ThermalTransientVoltageLimit = .ThermalTransientVoltageLimitDefault
        End With
    End Sub

    ''' <summary> Copy part values to the settings store. </summary>
    Public Sub CopySettings()
        With My.Settings
            If Me.Part IsNot Nothing Then
                .SourceMeasureUnit = Me.Part.InitialResistance.SourceMeasureUnit

                .ContactCheckEnabled = Me.Part.ContactCheckEnabled
                .ContactCheckThreshold = Me.Part.ContactCheckThreshold

                .ColdResistanceVoltageLimit = CDec(Me.Part.InitialResistance.VoltageLimit)
                .ColdResistanceCurrentLevel = CDec(Me.Part.InitialResistance.CurrentLevel)
                .ColdResistanceHighLimit = CDec(Me.Part.InitialResistance.HighLimit)
                .ColdResistanceLowLimit = CDec(Me.Part.InitialResistance.LowLimit)

                .PostTransientDelay = CDec(Me.Part.ThermalTransient.PostTransientDelay)
                .ThermalTransientVoltageChange = CDec(Me.Part.ThermalTransient.AllowedVoltageChange)
                .ThermalTransientCurrentLevel = CDec(Me.Part.ThermalTransient.CurrentLevel)
                .ThermalTransientHighLimit = CDec(Me.Part.ThermalTransient.HighLimit)
                .ThermalTransientLowLimit = CDec(Me.Part.ThermalTransient.LowLimit)
                .ThermalTransientVoltageLimit = CDec(Me.Part.ThermalTransient.VoltageLimit)
            End If
        End With

    End Sub

    ''' <summary> Gets or sets the apply new configuration button caption. </summary>
    ''' <value> The apply new configuration button caption. </value>
    Private Property ApplyNewConfigurationButtonCaption As String

    ''' <summary> Gets or sets the configuration button caption. </summary>
    ''' <value> The apply configuration button caption. </value>
    Private Property ApplyConfigurationButtonCaption As String

    ''' <summary> Checks if new configuration settings need to be applied. </summary>
    ''' <returns> <c>True</c> if settings where updated so that meter settings needs to be updated.</returns>
    Private Function IsNewConfigurationSettings() As Boolean
        Return Not Me.Part.ThermalTransientConfigurationEquals(Me.DeviceUnderTest)
    End Function

    Private _IsNewConfigurationSettingAvailable As Boolean

    ''' <summary> Gets or sets the is new configuration setting available. </summary>
    ''' <value> The is new configuration setting available. </value>
    Public Property IsNewConfigurationSettingAvailable As Boolean
        Get
            Return Me._IsNewConfigurationSettingAvailable
        End Get
        Set(value As Boolean)
            If Not value.Equals(Me.IsNewConfigurationSettingAvailable) Then
                Me._IsNewConfigurationSettingAvailable = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Updates the configuration status. </summary>
    Private Sub onConfigurationChanged()
        Dim caption As String = Me.ApplyConfigurationButtonCaption
        Dim changedCaption As String = Me.ApplyNewConfigurationButtonCaption
        Me.IsNewConfigurationSettingAvailable = Me.IsNewConfigurationSettings
        If Me.IsNewConfigurationSettingAvailable Then
            caption &= " !"
            changedCaption &= " !"
        End If
        If Not caption.Equals(Me._ApplyConfigurationButton.Text) Then
            Me._ApplyConfigurationButton.Text = caption
        End If
        If Not changedCaption.Equals(Me._ApplyNewConfigurationButton.Text) Then
            Me._ApplyNewConfigurationButton.Text = changedCaption
        End If
    End Sub

    ''' <summary> Configures changed values. </summary>
    ''' <param name="part"> The Part. </param>
    Protected Overridable Sub ConfigureChanged(ByVal part As DeviceUnderTest)
    End Sub

    ''' <summary> Event handler. Called by _ApplyNewThermalTransientConfigurationButton for click
    ''' events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyNewConfigurationButton_Click(sender As Object, e As System.EventArgs) Handles _ApplyNewConfigurationButton.Click

        If Me.Part Is Nothing OrElse Me.DeviceUnderTest Is Nothing Then
            Me._ErrorProvider.SetError(Me._ApplyNewConfigurationButton, "Meter not connected")
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Meter not connected;. ")
            Return
        End If
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            Me.Part.FinalResistance.CopyConfiguration(Me.Part.InitialResistance)
            If Not Me.DeviceUnderTest.InfoConfigurationEquals(Me.Part) Then
                ' clear execution state is not required - done before the measurement.
                ' Me.Meter.ClearExecutionState()
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Configuring Thermal Transient;. ")
                Me.ConfigureChanged(Me.Part)
            End If
        Catch ex As isr.VI.OperationFailedException
            Me._ErrorProvider.SetError(Me._ApplyNewConfigurationButton,
                                       "failed configuring--most one configuration element did not succeed in setting the correct value. See messages for details.")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred when configuring thermal transient;. Details: {0}", ex)
        Catch ex As ArgumentException
            Me._ErrorProvider.SetError(Me._ApplyNewConfigurationButton,
                                       "failed configuring--most likely due to a validation issue. See messages for details.")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred when configuring thermal transient;. Details: {0}", ex)
        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._ApplyNewConfigurationButton,
                                       "failed configuring thermal transient")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred configuring thermal transient;. Details: {0}", ex)
        Finally
            Me.onConfigurationChanged()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Configures all values. </summary>
    ''' <param name="part"> The Part. </param>
    Protected Overridable Sub Configure(ByVal part As DeviceUnderTest)
    End Sub

    ''' <summary> Saves the configuration settings and sends them to the meter. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyConfigurationButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ApplyConfigurationButton.Click

        If Me.Part Is Nothing OrElse Me.DeviceUnderTest Is Nothing Then
            Me._ErrorProvider.SetError(Me._ApplyConfigurationButton, "Meter not connected")
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Meter not connected;. ")
            Return
        End If
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            Me.Part.FinalResistance.CopyConfiguration(Me.Part.InitialResistance)
            ' clear execution state is not required - done before the measurement.
            ' Me.Meter.ClearExecutionState()
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Configuring Thermal Transient;. ")
            Me.Configure(Me.Part)
        Catch ex As isr.VI.OperationFailedException
            Me._ErrorProvider.SetError(Me._ApplyNewConfigurationButton,
                                       "failed configuring--most one configuration element did not succeed in setting the correct value. See messages for details.")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred when configuring thermal transient;. Details: {0}", ex)
        Catch ex As ArgumentException
            Me._ErrorProvider.SetError(Me._ApplyNewConfigurationButton,
                                       "failed configuring--most likely due to a validation issue. See messages for details.")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred when configuring thermal transient;. Details: {0}", ex)
        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._ApplyNewConfigurationButton, "failed configuring thermal transient")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred configuring thermal transient;. Details: {0}", ex)
        Finally
            Me.onConfigurationChanged()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by _RestoreDefaultsButton for click events. 
    '''           Restores the defaults settings.
    '''           </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _RestoreDefaultsButton_Click(sender As System.Object, e As System.EventArgs) Handles _RestoreDefaultsButton.Click

        ' read the instrument default settings.
        ConfigurationPanelBase.RestoreDefaults()

        ' reset part to know state based on the current defaults
        Me.Part.ResetKnownState()
        Windows.Forms.Application.DoEvents()

        ' bind.
        Me.BindIntialResitanceControls()
        Me.BindThermalTransientControls()

    End Sub

#End Region

End Class

