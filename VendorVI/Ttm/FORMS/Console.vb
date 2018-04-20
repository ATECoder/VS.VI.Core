Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Pith
Imports isr.Core.Pith.NumericExtensions
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Thermal Transient Meter Tester Console. </summary>
''' <license> (c) 2009 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="09/20/2009" by="David" revision="2.3.3915.x"> Created. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
Public Class Console
    Inherits ListenerFormBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializingComponents = True
        Me.InitializeComponent()
        Me.InitializingComponents = False
        Me._TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
    End Sub

    ''' <summary>
    ''' Disposes of the resources (other than memory) used by the
    ''' <see cref="T:System.Windows.Forms.Form" />.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Me.RemovePrivateListener(Me._TraceMessagesBox)
                If Me._Part IsNot Nothing Then
                    RemoveHandler Me._Part.ShuntResistance.PropertyChanged, AddressOf Me.ShuntResistancePropertyChanged
                    Me._Part.Dispose() : Me._Part = Nothing
                End If
                If Me._Meter IsNot Nothing Then Me._Meter.Dispose() : Me._Meter = Nothing
                If components IsNot Nothing Then components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    ''' <summary> Bind controls. </summary>
    Private Sub BindShuntControls()

        ' set the GUI based on the current defaults.
        Dim instrumentSettings As My.MySettings = My.MySettings.Default

        With Me._ShuntResistanceCurrentRangeNumeric
            .Minimum = instrumentSettings.ShuntResistanceCurrentMinimum
            .Maximum = instrumentSettings.ShuntResistanceCurrentMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ShuntResistance, "CurrentRange", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.MySettings.Default.ShuntResistanceCurrentRange.Clip(.Minimum, .Maximum)
            Me.Part.ShuntResistance.CurrentRange = .Value
        End With

        With Me._ShuntResistanceCurrentLevelNumeric
            .Minimum = instrumentSettings.ShuntResistanceCurrentMinimum
            .Maximum = instrumentSettings.ShuntResistanceCurrentMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ShuntResistance, "CurrentLevel", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.MySettings.Default.ShuntResistanceCurrentLevel.Clip(.Minimum, .Maximum)
            Me.Part.ShuntResistance.CurrentLevel = .Value
        End With

        With Me._ShuntResistanceHighLimitNumeric
            .Minimum = instrumentSettings.ShuntResistanceMinimum
            .Maximum = instrumentSettings.ShuntResistanceMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ShuntResistance, "HighLimit", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.MySettings.Default.ShuntResistanceHighLimit.Clip(.Minimum, .Maximum)
            Me.Part.ShuntResistance.HighLimit = .Value
        End With

        With Me._ShuntResistanceLowLimitNumeric
            .Minimum = instrumentSettings.ShuntResistanceMinimum
            .Maximum = instrumentSettings.ShuntResistanceMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ShuntResistance, "LowLimit", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.MySettings.Default.ShuntResistanceLowLimit.Clip(.Minimum, .Maximum)
            Me.Part.ShuntResistance.LowLimit = .Value
        End With

        With Me._ShuntResistanceVoltageLimitNumeric
            .Minimum = instrumentSettings.ShuntResistanceVoltageMinimum
            .Maximum = instrumentSettings.ShuntResistanceVoltageMaximum
            .DataBindings.Clear()
            .DataBindings.Add(New Binding("Value", Me.Part.ShuntResistance, "VoltageLimit", True, DataSourceUpdateMode.OnPropertyChanged))
            .Value = My.MySettings.Default.ShuntResistanceVoltageLimit.Clip(.Minimum, .Maximum)
            Me.Part.ShuntResistance.VoltageLimit = .Value
        End With

    End Sub

#End Region

#Region " EVENT HANDLERS:  FORM "

    ''' <summary> Sets the caption for this form. </summary>
    Friend Sub RefreshCaption()
        Me.Text = String.Format(Globalization.CultureInfo.CurrentCulture, "{0} {1}.{2}.{3}: CONSOLE",
                                My.Application.Info.Title, My.Application.Info.Version.Major,
                                My.Application.Info.Version.Minor, My.Application.Info.Version.Build)
    End Sub

    ''' <summary> Raises the <see cref="E:System.Windows.Forms.Form.Closing" /> event. </summary>
    ''' <param name="e"> A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the
    ''' event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Try

            If Me._PartsPanel.SaveEnabled Then

                Dim dialogResult As DialogResult = MessageBox.Show("Data not saved. Select Yes to save, no to skip saving or cancel to cancel closing the program.",
                                                                   "SAVE DATA?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                If dialogResult = Windows.Forms.DialogResult.Yes Then
                    Me._PartsPanel.SaveParts()
                ElseIf dialogResult = Windows.Forms.DialogResult.No Then
                ElseIf dialogResult = Windows.Forms.DialogResult.Cancel Then
                    If e IsNot Nothing Then
                        e.Cancel = True
                    End If
                Else
                    If e IsNot Nothing Then
                        e.Cancel = True
                    End If
                End If

            End If

            If e Is Nothing OrElse Not e.Cancel Then

                Me._PartsPanel.CopySettings()
                Me._TTMConfigurationPanel.CopySettings()
                Me.CopyShuntSettings()
                My.MySettings.Default.ResourceName = Me.ResourceName
                My.MySettings.Default.Save()

                ' flush the log.
                My.Application.Log.TraceSource.Flush()

                ' wait for timer to terminate all is actions
                Dim timer As System.Diagnostics.Stopwatch = Diagnostics.Stopwatch.StartNew
                Do Until timer.ElapsedMilliseconds > 400
                    Windows.Forms.Application.DoEvents()
                    Threading.Thread.Sleep(10)
                Loop

                ' allow all events requiring the panel to execute on their thread.
                ' this allows all timer events that where in progress to be consummated before closing the form.
                ' this does not prevent timer exceptions in design mode.
                For i As Integer = 1 To 1000
                    Application.DoEvents()
                Next

            End If

        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Application.DoEvents()
            MyBase.OnClosing(e)
        End Try
    End Sub

    ''' <summary> Called upon receiving the <see cref="E:System.Windows.Forms.Form.Load" /> event. </summary>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Try

            ' Turn on the form hourglass cursor
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            Trace.CorrelationManager.StartLogicalOperation(Reflection.MethodInfo.GetCurrentMethod.Name)

	      Me.AddPrivateListeners()

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "loading...")

            ' hide the alerts
            Me._MeasurementsHeader.ShowAlerts(False, False)
            Me._MeasurementsHeader.ShowOutcome(False, False)


            With Me._TraceMessagesBox
                ' set defaults for the messages box.
                .TabCaption = "Log"
                .ResetCount = 500
                .PresetCount = 250
            End With

            ' build the navigator tree.
            Me.BuildNavigatorTreeView()

            ' set the form caption
            Me.RefreshCaption()

            ' default to center screen.
            Me.CenterToScreen()

        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception loading the driver console form;. {0}", ex.ToFullBlownString)
            If DialogResult.Abort = MessageBox.Show(ex.ToString, "Exception Occurred", MessageBoxButtons.AbortRetryIgnore,
                                                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                                    MessageBoxOptions.DefaultDesktopOnly) Then
                Application.Exit()
            End If
        Finally

            MyBase.OnLoad(e)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Trace.CorrelationManager.StopLogicalOperation()

        End Try

    End Sub

    ''' <summary> Called upon receiving the <see cref="E:System.Windows.Forms.Form.Shown" /> event. </summary>
    ''' <param name="e"> A <see cref="T:System.EventArgs" /> that contains the event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnShown(e As System.EventArgs)

        Try

            Trace.CorrelationManager.StartLogicalOperation(Reflection.MethodInfo.GetCurrentMethod.Name)

            ' Turn on the form hourglass cursor
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            ' allow form rendering time to complete: process all messages currently in the queue.
            Application.DoEvents()

            Me.Cursor = Cursors.WaitCursor
            Application.DoEvents()

            Me.RefreshCaption()
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                              "Logging;. to {0}.", My.Application.Log.DefaultFileLogWriter.FullLogFileName)

            Me.ApplyShuntResistanceButtonCaption = Me._ApplyShuntResistanceConfigurationButton.Text
            Me.ApplyNewShuntResistanceButtonCaption = Me._ApplyNewShuntResistanceConfigurationButton.Text
            Me.Meter = New Meter
            Me.Meter.AddListeners(Me.Talker)

            Me.Part = New DeviceUnderTest
            AddHandler Me._Part.ShuntResistance.PropertyChanged, AddressOf Me.ShuntResistancePropertyChanged

            Me._ResourceSelectorConnector.Enabled = False
            With Me._ResourceSelectorConnector
                .Connectable = True : .Clearable = True : .Searchable = True
                .ClearToolTipText = "Reset, clear, initialize the device"
            End With
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Enabling controls;. ")
            Me.onConnectionChanged("")
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Ready - List resources and select one to connect too;. ")
            Me._ResourceSelectorConnector.Enabled = True
            Application.DoEvents()

        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception showing the driver console form;. {0}", ex.ToFullBlownString)
            If DialogResult.Abort = MessageBox.Show(ex.ToString, "Exception Occurred", MessageBoxButtons.AbortRetryIgnore,
                                                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                                    MessageBoxOptions.DefaultDesktopOnly) Then
                Application.Exit()
            End If

        Finally

            MyBase.OnShown(e)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Trace.CorrelationManager.StopLogicalOperation()

            Application.DoEvents()
            Me.SelectNavigatorTreeViewNode(TreeViewNode.ConnectNode)
            Me._ResourceSelectorConnector.Focus()
            Me._ResourceSelectorConnector.SessionFactory.SelectedResourceName = My.Settings.ResourceName
            Application.DoEvents()

        End Try

    End Sub

#End Region

#Region " CONNECT "

    ''' <summary> Updates the availability of the controls. </summary>
    Private Sub OnMeasurementStatusChanged()

        Dim measurementSequenceState As MeasurementSequenceState = MeasurementSequenceState.Idle
        If Me.MeasureSequencer IsNot Nothing Then
            measurementSequenceState = Me.MeasureSequencer.MeasurementSequenceState
        End If

        Dim triggerSequenceState As TriggerSequenceState = TriggerSequenceState.Idle
        If Me.TriggerSequencer IsNot Nothing Then
            triggerSequenceState = Me.TriggerSequencer.TriggerSequenceState
        End If

        Dim enabled As Boolean = Me.Meter.IsDeviceOpen AndAlso
                                 TriggerSequenceState.Idle = triggerSequenceState AndAlso
                                 MeasurementSequenceState.Idle = measurementSequenceState
        Me._ConnectGroupBox.Enabled = TriggerSequenceState.Idle = triggerSequenceState AndAlso
                                      MeasurementSequenceState.Idle = measurementSequenceState
        Me._TTMConfigurationPanel.Enabled = enabled

        Me._MeasureShuntResistanceButton.Enabled = enabled
        Me._ApplyShuntResistanceConfigurationButton.Enabled = enabled
        Me._ApplyNewShuntResistanceConfigurationButton.Enabled = enabled

    End Sub

    ''' <summary> Updates the connection related controls. </summary>
    ''' <param name="resourceName"> The resource name to connect. </param>
    Private Sub OnConnectionChanged(ByVal resourceName As String)

        If Me.Meter.IsDeviceOpen AndAlso Not String.IsNullOrWhiteSpace(resourceName) Then

            ' set reference tot he device under test.
            Me.DeviceUnderTest = Me.Meter.ConfigInfo
            If Me.SupportsParts Then Me._PartHeader.DeviceUnderTest = Me.Part
            Me._MeasurementsHeader.DeviceUnderTest = Me.Part
            Me._MeasurementsHeader.Clear()
            Me._ThermalTransientHeader.DeviceUnderTest = Me.Part
            Me._ThermalTransientHeader.Clear()
            Me._TTMConfigurationPanel.Meter = Me.Meter
            Me._MeasurementPanel.Meter = Me.Meter
            If Me.SupportsParts Then Me._PartsPanel.Part = Me.Part
            Me.Part.ResumePublishing()
            Me.Part.ClearPartInfo()
            Me.Part.ClearMeasurements()
            If Me.SupportsParts Then Me._PartsPanel.ClearParts()
            If Me.SupportsParts Then Me._PartsPanel.ApplySettings()

            ' resume publishing
            Me.Meter.ResumePublishing()
            Me.Meter.Part = Me.Part
            Me.MeasureSequencer = Me.Meter.MeasureSequencer
            Me.TriggerSequencer = Me.Meter.TriggerSequencer

            ' initialize the device system state.
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Clearing master device active state;. ")
            Me.Meter.MasterDevice.ClearActiveState()
            Application.DoEvents()

            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Reading identity from {0};. ", resourceName)
            Me._IdentityTextBox.Text = Me.Meter.MasterDevice.StatusSubsystem.QueryIdentity
            Application.DoEvents()

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Resetting and Clearing meter;. ")
            Me.Meter.ResetClear()
            Application.DoEvents()

            ' reset part to know state based on the current defaults
            Me.Part.ResetKnownState()
            Application.DoEvents()

            Me._TTMConfigurationPanel.Part = Part
            Application.DoEvents()

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Binding controls;. ")
            Me.BindShuntControls()

            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Connected to {0};. ", resourceName)
            Application.DoEvents()

        ElseIf Not String.IsNullOrWhiteSpace(resourceName) Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Disconnected from {0};. ", resourceName)
            Me._MeterTimer.Enabled = False
        End If
        Me.OnMeasurementStatusChanged()
    End Sub

#End Region

#Region " RESOURCE CONNECTOR "

    ''' <summary> Gets or sets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Property ResourceName As String
        Get
            Return Me._ResourceSelectorConnector.SessionFactory.SelectedResourceName
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.ResourceName) Then
                Me._ResourceSelectorConnector.SessionFactory.SelectedResourceName = value
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Search Pattern of the resource. </summary>
    ''' <value> The Search Pattern of the resource. </value>
    Public Property ResourceFilter As String
        Get
            Return Me._ResourceSelectorConnector.SessionFactory.ResourcesFilter
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.ResourceFilter) Then
                Me._ResourceSelectorConnector.SessionFactory.ResourcesFilter = value
            End If
        End Set
    End Property

    ''' <summary> Displays the resource names based on the device resource search pattern. </summary> 
    Public Sub DisplayNames()
        ' get the list of available resources
        If Me.Meter IsNot Nothing AndAlso Me.Meter.MasterDevice IsNot Nothing AndAlso
            Me.Meter.MasterDevice.Session.ResourceNameInfo.ResourcesFilter IsNot Nothing Then
            Me._ResourceSelectorConnector.SessionFactory.ResourcesFilter = Me.Meter.MasterDevice.Session.ResourceNameInfo.ResourcesFilter
        End If
        Me._ResourceSelectorConnector.SessionFactory.EnumerateResources()
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub Connector_Clear(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResourceSelectorConnector.Clear
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                           "Resetting, clearing and initializing resource;. {0}", Me.ResourceName)
        Me.Meter.MasterDevice.ResetClearInit()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                           "Resource reset, initialized and cleared;. {0}", Me.ResourceName)
    End Sub

    ''' <summary> Connects the instrument by calling a propagating connect command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Connector_Connect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _ResourceSelectorConnector.Connect
        Dim activity As String = $"opening VISA Session to {Me.ResourceName}"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Connecting;. {activity}")
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            ' Me.Meter.MasterDevice.RegisterNotifier(Me._ResourceSelectorConnector.Talker)
            Me.Meter.CaptureSyncContext(Threading.SynchronizationContext.Current)
            Me._IdentityTextBox.Text = $"Connecting {Me.ResourceName}"
            Me.Meter.MasterDevice.OpenSession(Me.ResourceName, "TTM")
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Connected;. Opened VISA Session to {Me.ResourceName}")

            ' allow events to take shape before completion of actions -- there is an issue with the 
            ' meter elements not established 
            Application.DoEvents()
        Catch ex As VI.Pith.OperationFailedException
            Me._ErrorProvider.SetError(Me._IdentityTextBox, "Connection failed")
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Failed connecting;. Failed {activity}.{ex.ToFullBlownString}")
        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._IdentityTextBox, "Connection failed")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Failed connecting;. Exception {activity}.{ex.ToFullBlownString}")
        Finally
            Try
                activity = $"preparing {Me.ResourceName}"
                Me.OnConnectionChanged(Me.ResourceName)
            Catch ex As Exception
                Me._ErrorProvider.SetError(Me._IdentityTextBox, "Connection failed")
                Me._IdentityTextBox.Text = $"Failed {activity}"
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception occurred {activity};. {ex.ToFullBlownString}")
            Finally
                ' cancel if failed to open
                If Not Me.Meter.IsDeviceOpen Then e.Cancel = True
                Me.Cursor = Cursors.Default
            End Try
        End Try

    End Sub

    ''' <summary> Disconnects the instrument by calling a propagating disconnect command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Connector_Disconnect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _ResourceSelectorConnector.Disconnect
        Dim activity As String = $"closing VISA Session to {Me.ResourceName}"
        Try
            If Me.Meter.MasterDevice.IsDeviceOpen Then
                ' stop publishing
                Me.Part.SuspendPublishing()

                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{activity};. ")

                Me._IdentityTextBox.Text = activity

                Me.Meter.CaptureSyncContext(Threading.SynchronizationContext.Current)
                Me.Meter.MasterDevice.CloseSession()

            End If

        Catch ex As VI.Pith.OperationFailedException
            Me._ErrorProvider.SetError(Me._IdentityTextBox, $"{activity} failed")
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Failed {activity};. {ex.ToFullBlownString}")
        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._IdentityTextBox, $"{activity} failed")
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
        Finally
            If Me.Meter.MasterDevice.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Still open--{activity}failed;. ")
                Me._IdentityTextBox.Text = $"Still open; Failed {activity}"
            Else
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Disconnected;. Ended access To {0}", Me.ResourceName)
                Me._IdentityTextBox.Text = $"Disconnected from {Me.ResourceName}"
            End If
            Try
                Me.OnConnectionChanged(Me.ResourceName)
            Catch ex As Exception
                Me._ErrorProvider.SetError(Me._IdentityTextBox, "Disconnection failed")
                Me._IdentityTextBox.Text = "Failed Disconnecting"
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                  "Exception occurred disconnecting from {0};. {1}", ResourceName, ex.ToFullBlownString)
            Finally
                ' cancel if failed to close
                If Me.Meter.IsDeviceOpen Then e.Cancel = True
                Me.Cursor = Cursors.Default
            End Try


        End Try
    End Sub

    ''' <summary> Displays available instrument names. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub Connector_FindNames(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResourceSelectorConnector.FindNames
        Me.DisplayNames()
    End Sub

    ''' <summary> Executes the connector property changed action. </summary>
    ''' <param name="sender">       Specifies the object where the call originated. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As Instrument.ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(SessionFactory.SelectedResourceName)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Selected {sender.SessionFactory.SelectedResourceName};. ")
            Case NameOf(SessionFactory.SelectedResourceExists)
                If sender.SessionFactory.SelectedResourceExists Then
                    If Me.Meter.IsDeviceOpen Then
                        Me._IdentityTextBox.Text = $"Resource {Me.ResourceName} connected"
                        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Resource connected;. {Me.ResourceName}")
                    Else
                        Me._IdentityTextBox.Text = $"Resource {Me.ResourceName} located"
                        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Resource located;. {Me.ResourceName}")
                    End If
                Else
                    Me._IdentityTextBox.Text = $"Resource {Me.ResourceName} not found"
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Resource not found;. {Me.ResourceName}")
                End If

        End Select
    End Sub

    ''' <summary> Event handler. Called by _ResourceNameSelectorConnector for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Connector_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ResourceSelectorConnector.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Connector_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, Instrument.ResourceSelectorConnector), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " DEVICE UNDER TEST "

    ''' <summary> Gets or sets the device under test. </summary>
    ''' <value> The device under test. </value>
    ''' <remroks> This is the element representing the actual meter settings. </remroks>
    Private Property DeviceUnderTest As DeviceUnderTest

#End Region

#Region " SHUNT "

#Region " CONFIGURE SHUNT"

    ''' <summary> Restore defaults. </summary>
    Private Shared Sub RestoreShuntDefaults()
        With My.MySettings.Default
            .ShuntResistanceCurrentRange = .ShuntResistanceCurrentRangeDefault
            .ShuntResistanceCurrentLevel = .ShuntResistanceCurrentLevelDefault
            .ShuntResistanceHighLimit = .ShuntResistanceHighLimitDefault
            .ShuntResistanceLowLimit = .ShuntResistanceLowLimitDefault
            .ShuntResistanceVoltageLimit = .ShuntResistanceVoltageLimitDefault
        End With
    End Sub

    ''' <summary> Copy shunt values to the settings store. </summary>
    Private Sub CopyShuntSettings()
        With My.MySettings.Default
            If Me.Part IsNot Nothing Then
                .ShuntResistanceCurrentRange = CDec(Me.Part.ShuntResistance.CurrentRange)
                .ShuntResistanceCurrentLevel = CDec(Me.Part.ShuntResistance.CurrentLevel)
                .ShuntResistanceHighLimit = CDec(Me.Part.ShuntResistance.HighLimit)
                .ShuntResistanceLowLimit = CDec(Me.Part.ShuntResistance.LowLimit)
                .ShuntResistanceVoltageLimit = CDec(Me.Part.ShuntResistance.VoltageLimit)
            End If
        End With
    End Sub


    ''' <summary> Updates the shunt bound values. </summary>
    Private Sub UpdateShuntBoundValues()
        If Me.Meter IsNot Nothing AndAlso Me.Meter.ShuntResistance IsNot Nothing Then
            My.MySettings.Default.ShuntResistanceCurrentLevel = CDec(Me.Meter.ShuntResistance.CurrentLevel)
            With Me._ShuntResistanceCurrentLevelNumeric
                .Value = My.MySettings.Default.ShuntResistanceCurrentLevel.Clip(.Minimum, .Maximum)
            End With
            My.MySettings.Default.ShuntResistanceCurrentRange = CDec(Me.Meter.ShuntResistance.CurrentRange)
            With Me._ShuntResistanceCurrentRangeNumeric
                .Value = My.MySettings.Default.ShuntResistanceCurrentRange.Clip(.Minimum, .Maximum)
            End With
            My.MySettings.Default.ShuntResistanceVoltageLimit = CDec(Me.Meter.ShuntResistance.VoltageLimit)
            With Me._ShuntResistanceVoltageLimitNumeric
                .Value = My.MySettings.Default.ShuntResistanceVoltageLimit.Clip(.Minimum, .Maximum)
            End With
            My.MySettings.Default.ShuntResistanceHighLimit = CDec(Me.Meter.ShuntResistance.HighLimit)
            With Me._ShuntResistanceHighLimitNumeric
                .Value = My.MySettings.Default.ShuntResistanceHighLimit.Clip(.Minimum, .Maximum)
            End With
            My.MySettings.Default.ShuntResistanceLowLimit = CDec(Me.Meter.ShuntResistance.LowLimit)
            With Me._ShuntResistanceLowLimitNumeric
                .Value = My.MySettings.Default.ShuntResistanceLowLimit.Clip(.Minimum, .Maximum)
            End With
        End If
    End Sub

    ''' <summary> Gets or sets the apply new shunt resistance button caption. </summary>
    ''' <value> The apply shunt resistance button caption. </value>
    Private Property ApplyNewShuntResistanceButtonCaption As String

    ''' <summary> Gets or sets the apply shunt resistance button caption. </summary>
    ''' <value> The apply shunt resistance button caption. </value>
    Private Property ApplyShuntResistanceButtonCaption As String

    ''' <summary> Is new shunt resistance settings. </summary>
    ''' <returns> <c>True</c> if settings where updated so that meter settings needs to be updated. </returns>
    Private Function IsNewShuntResistanceSettings() As Boolean
        Return Me.DeviceUnderTest IsNot Nothing AndAlso
               Not Me._Part.ShuntResistance.ConfigurationEquals(Me.DeviceUnderTest.ShuntResistance)
    End Function

    ''' <summary> Updates the shunt configuration button caption. </summary>
    Private Sub UpdateShuntConfigButtonCaption()
        Dim caption As String = Me.ApplyShuntResistanceButtonCaption
        Dim changedCaption As String = Me.ApplyNewShuntResistanceButtonCaption
        If Me.IsNewShuntResistanceSettings Then
            caption &= " !"
            changedCaption &= " !"
        End If
        If Not caption.Equals(Me._ApplyShuntResistanceConfigurationButton.Text) Then
            Me._ApplyShuntResistanceConfigurationButton.Text = caption
        End If
        If Not changedCaption.Equals(Me._ApplyNewShuntResistanceConfigurationButton.Text) Then
            Me._ApplyNewShuntResistanceConfigurationButton.Text = changedCaption
        End If
    End Sub

    ''' <summary> Handles the Click event of the _ApplyShuntResistanceConfigurationButton control.
    ''' Saves the configuration settings and sends them to the meter. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      The <see cref="System.EventArgs" /> instance containing the event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyShuntResistanceConfigurationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ApplyShuntResistanceConfigurationButton.Click

        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Configuring Shunt Resistance;. ")

            If Me.Meter.IsDeviceOpen Then

                ' not required. 
                ' Me.Meter.ClearExecutionState()
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Sending Shunt resistance configuration settings to the meter;. ")
                Me.Meter.ConfigureShuntResistance(Me.Part.ShuntResistance)
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Shunt resistance measurement configured successfully;. ")

            Else

                Me._ErrorProvider.SetError(Me._ApplyShuntResistanceConfigurationButton, "Meter not connected")
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Meter not connected;. ")

            End If

        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._ApplyShuntResistanceConfigurationButton,
                                       "Failed configuring shunt resistance")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred configuring shunt resistance;. {0}", ex.ToFullBlownString)
        Finally
            Me.UpdateShuntConfigButtonCaption()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Applies new shunt resistance settings.
    '''           Event handler. Called by _ApplyNewShuntResistanceConfigurationButton for click
    ''' events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ApplyNewShuntResistanceConfigurationButton_Click(sender As Object, e As System.EventArgs) Handles _ApplyNewShuntResistanceConfigurationButton.Click

        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Configuring Shunt Resistance;. ")

            If Me.Meter.IsDeviceOpen Then

                ' not required. 
                ' Me.Meter.ClearExecutionState()
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                  "Sending Shunt resistance configuration settings to the meter;. ")
                Me.Meter.ConfigureShuntResistanceChanged(Me.Part.ShuntResistance)
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                  "Shunt resistance measurement configured successfully;. ")
            Else

                Me._ErrorProvider.SetError(Me._ApplyShuntResistanceConfigurationButton, "Meter not connected")
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Meter not connected;. ")

            End If

        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._ApplyShuntResistanceConfigurationButton, "Failed configuring shunt resistance")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred configuring shunt resistance;. {0}", ex.ToFullBlownString)
        Finally
            Me.UpdateShuntConfigButtonCaption()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by _RestoreShuntResistanceDefaultsButton for click events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _RestoreShuntResistanceDefaultsButton_Click(sender As System.Object, e As System.EventArgs) Handles _RestoreShuntResistanceDefaultsButton.Click

        ' read the instrument default settings.
        Console.RestoreShuntDefaults()

        ' reset part to know state based on the current defaults
        Me.Part.ShuntResistance.ResetKnownState()
        Application.DoEvents()

        ' bind.
        Me.BindShuntControls()

    End Sub


#End Region

#Region " MEASURE SHUNT "

    ''' <summary> Measures Shunt resistance. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MeasureShuntResistanceButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _MeasureShuntResistanceButton.Click
        SyncLock Meter
            Try
                Me.Cursor = Cursors.WaitCursor
                Me._ErrorProvider.SetError(Me._ShuntResistanceTextBox, "")
                Me._ErrorProvider.SetError(Me._MeasureShuntResistanceButton, "")
                Me._ErrorProvider.SetIconPadding(Me._MeasureShuntResistanceButton, -15)
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measuring Shunt Resistance...;. ")
                Me.Meter.MeasureShuntResistance(Me.Part.ShuntResistance)
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Shunt Resistance measured;. ")
            Catch ex As Exception
                Me._ShuntResistanceTextBox.Text = ""
                Me._ErrorProvider.SetError(Me._MeasureShuntResistanceButton, "Failed Measuring Shunt Resistance")
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                  "Failed Measuring Shunt Resistance;. {0}", ex.ToFullBlownString)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End SyncLock
    End Sub

#End Region

#End Region

#Region " DISPLAY "

    ''' <summary> Displays the thermal transient. </summary>
    Private Sub SetErrorProvider(ByVal textBox As TextBox, ByVal resistance As ResistanceMeasureBase)
        If (resistance.Outcome And MeasurementOutcomes.PartFailed) <> 0 Then
            Me._ErrorProvider.SetError(textBox, "Value out of range")
        ElseIf (resistance.Outcome And MeasurementOutcomes.MeasurementFailed) <> 0 Then
            Me._ErrorProvider.SetError(textBox, "Measurement failed")
        ElseIf (resistance.Outcome And MeasurementOutcomes.MeasurementNotMade) <> 0 Then
            Me._ErrorProvider.SetError(textBox, "Measurement not made")
        End If
    End Sub

    ''' <summary> Displays the resistance. </summary>
    Private Sub ShowResistance(ByVal textBox As TextBox, ByVal resistance As ResistanceMeasureBase)
        textBox.Text = resistance.ResistanceCaption
        Me.setErrorProvider(textBox, resistance)
    End Sub

    ''' <summary> Displays the thermal transient. </summary>
    Private Sub ShowThermalTransient(ByVal textBox As TextBox, ByVal resistance As ResistanceMeasureBase)
        textBox.Text = resistance.VoltageCaption
        Me.setErrorProvider(textBox, resistance)
    End Sub

#End Region

#Region " PART "

    ''' <summary> Gets or sets the part. </summary>
    ''' <value> The part. </value>
    Private Property Part As DeviceUnderTest

#Region " PART: SHUNT RESISTANCE "

    ''' <summary> Raises the property changed event. </summary>
    ''' <param name="sender"> The source of the event. </param>
    Private Sub OnPropertyChanged(ByVal sender As ShuntResistance, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Me.UpdateShuntConfigButtonCaption()
        Select Case propertyName
            Case NameOf(Ttm.ShuntResistance.MeasurementAvailable)
                If sender.MeasurementAvailable Then
                    Me.showResistance(Me._ShuntResistanceTextBox, sender)
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _ShuntResistance for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ShuntResistancePropertyChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, ShuntResistance), e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception handling property '{0}';. {1}",
                               e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#End Region

#Region " PARTS"

    Private Sub _PartsPanel_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _PartsPanel.PropertyChanged
        If sender IsNot Nothing AndAlso e IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(e.PropertyName) Then
        End If
    End Sub

    ''' <summary> Raises the property changed event. </summary>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(sender As ConfigurationPanel, propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Ttm.ConfigurationPanel.IsNewConfigurationSettingAvailable)
                If Me._NavigatorTreeView.Nodes IsNot Nothing AndAlso Me._NavigatorTreeView.Nodes.Count > 0 Then
                    Dim caption As String = TreeViewNode.ConfigureNode.Description
                    If sender.IsNewConfigurationSettingAvailable Then
                        caption &= " *"
                    End If
                    Me._NavigatorTreeView.Nodes(TreeViewNode.ConfigureNode.ToString).Text = caption
                End If
        End Select
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TTMConfigurationPanel_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _TTMConfigurationPanel.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._TTMConfigurationPanel_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, ConfigurationPanel), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception handling property '{0}';. {1}",
                               e.PropertyName, ex.ToFullBlownString)

        End Try
    End Sub

#End Region

#Region " TTM: METER "

    ''' <summary> Assigns a Meter. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignMeter(ByVal value As Meter)
        Me._Meter = value
        Me._Meter.CaptureSyncContext(Threading.SynchronizationContext.Current)
        Me.AddPrivateListeners()
        Me.OnConnectionChanged(Me.ResourceName)
    End Sub

    Private Property IsMeterOwner As Boolean

    ''' <summary> Assigns a Meter. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignMeter(ByVal value As Meter)
        Me.IsMeterOwner = False
        Me._AssignMeter(value)
    End Sub

    ''' <summary> Releases the Meter. </summary>
    Protected Sub ReleaseMeter()
        If Me.IsMeterOwner Then
            Me._Meter?.Dispose() : Me._Meter = Nothing
        Else
            Me._Meter = Nothing
        End If
    End Sub

    ''' <summary> Gets the is Meter assigned. </summary>
    ''' <value> The is Meter assigned. </value>
    Public ReadOnly Property IsMeterAssigned As Boolean
        Get
            Return Me.Meter IsNot Nothing AndAlso Not Me.Meter.IsDisposed
        End Get
    End Property

    ''' <summary> The with events. </summary>
    Private WithEvents _Meter As Meter

    ''' <summary> Gets or sets reference to the thermal transient meter device. </summary>
    ''' <value> The meter. </value>
    Public Property Meter() As Meter
        Get
            Return Me._meter
        End Get
        Set(ByVal value As Meter)
            Me._meter = value
            If Me._meter IsNot Nothing Then
                Me._meter.MasterDevice.Enabled = True
            End If
        End Set
    End Property

    ''' <summary> Raises the property changed event. </summary>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As Meter, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Ttm.Meter.MeasurementCompleted)
                If sender.MeasurementCompleted Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "{0} measurement completed;. ", sender.ResourceName)
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _meter for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Meter_PropertyChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _meter.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._meter_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, Meter), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception handling property '{0}';. {1}",
                               e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " SEQUENCED MEASUREMENTS "

    Private WithEvents _MeasureSequencer As MeasureSequencer
    ''' <summary> Gets or sets the sequencer. </summary>
    ''' <value> The sequencer. </value>
    Private Property MeasureSequencer As MeasureSequencer
        Get
            Return Me._MeasureSequencer
        End Get
        Set(value As MeasureSequencer)
            Me._MeasureSequencer = value
            Me._MeasureSequencer?.CaptureSyncContext(Threading.SynchronizationContext.Current)
        End Set
    End Property

    ''' <summary> Handles the measure sequencer property changed event. </summary>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As MeasureSequencer, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Ttm.MeasureSequencer.MeasurementSequenceState)
                Me.OnMeasurementSequenceStateChanged(sender.MeasurementSequenceState)
        End Select
    End Sub

    ''' <summary> Event handler. Called by _MeasureSequencer for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MeasureSequencer_PropertyChanged(ByVal sender As System.Object, ByVal e As PropertyChangedEventArgs) Handles _MeasureSequencer.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._MeasureSequencer_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, MeasureSequencer), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub


    ''' <summary> Ends a completed sequence. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OnMeasurementSequenceCompleted()

        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Measurement completed;. ")
        Try
            ' add part if auto add is enabled.
            If Me._PartsPanel.AutoAddEnabled Then
                Me._PartsPanel.AddPart()
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Failed adding part upon completing the sequence;. {0}", ex.ToFullBlownString)
        End Try

    End Sub

    ''' <summary> Handles the change in measurement state. </summary>
    ''' <param name="state"> The state. </param>
    Private Sub OnMeasurementSequenceStateChanged(ByVal state As MeasurementSequenceState)
        Static lastState As MeasurementSequenceState
        If lastState <> state Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Processing the {0} state;. ", state.Description)
            lastState = state
        End If
        Select Case state
            Case MeasurementSequenceState.Aborted
            Case MeasurementSequenceState.Completed
            Case MeasurementSequenceState.Failed
            Case MeasurementSequenceState.MeasureInitialResistance
            Case MeasurementSequenceState.MeasureThermalTransient
            Case MeasurementSequenceState.Idle
                Me.onMeasurementStatusChanged()
            Case MeasurementSequenceState.None
            Case MeasurementSequenceState.PostTransientPause
            Case MeasurementSequenceState.MeasureFinalResistance
            Case MeasurementSequenceState.Starting
                Me.onMeasurementStatusChanged()
            Case Else
                Debug.Assert(Not Debugger.IsAttached, "Unhandled state: " & state.ToString)
        End Select
    End Sub

#End Region

#Region " TRIGGERED MEASUREMENTS "

    Private WithEvents _TriggerSequencer As TriggerSequencer
    ''' <summary> Gets or sets the trigger sequencer. </summary>
    ''' <value> The sequencer. </value>
    Private Property TriggerSequencer As TriggerSequencer
        Get
            Return Me._TriggerSequencer
        End Get
        Set(value As TriggerSequencer)
            Me._TriggerSequencer = value
        End Set
    End Property

    ''' <summary> Handles the trigger sequencer property changed event. </summary>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As TriggerSequencer, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Ttm.TriggerSequencer.TriggerSequenceState)
                Me.OnTriggerSequenceStateChanged(sender.TriggerSequenceState)
        End Select
    End Sub


    ''' <summary> Event handler. Called by _TriggerSequencer for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TriggerSequencer_PropertyChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _TriggerSequencer.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._TriggerSequencer_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, TriggerSequencer), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

    ''' <summary> Handles the change in measurement state. </summary>
    ''' <param name="state"> The state. </param>
    Private Sub OnTriggerSequenceStateChanged(ByVal state As TriggerSequenceState)
        Static lastState As TriggerSequenceState
        If lastState <> state Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Processing the {0} state;. ", state.Description)
            lastState = state
        End If
        Select Case state
            Case TriggerSequenceState.Aborted
                Me.onMeasurementStatusChanged()
            Case TriggerSequenceState.Stopped
            Case TriggerSequenceState.Failed
            Case TriggerSequenceState.WaitingForTrigger
            Case TriggerSequenceState.MeasurementCompleted
            Case TriggerSequenceState.ReadingValues
            Case TriggerSequenceState.Idle
                Me.onMeasurementStatusChanged()
            Case TriggerSequenceState.None
            Case TriggerSequenceState.Starting
                Me.onMeasurementStatusChanged()
            Case Else
                Debug.Assert(Not Debugger.IsAttached, "Unhandled state: " & state.ToString)
        End Select
    End Sub

#End Region

#Region " NAVIGATION "

    ''' <summary> Gets the support parts. </summary>
    ''' <value> The support parts. </value>
    Public Property SupportsParts As Boolean

    ''' <summary> Gets the supports shunt. </summary>
    ''' <value> The supports shunt. </value>
    Public Property SupportsShunt As Boolean

    ''' <summary> Enumerates the nodes. Each item is the same as the node name. </summary>
    Private Enum TreeViewNode
        <Description("CONNECT")> ConnectNode
        <Description("CONFIGURE")> ConfigureNode
        <Description("MEASURE")> MeasureNode
        <Description("SHUNT")> ShuntNode
        <Description("PARTS")> PartsNode
        <Description("MESSAGES")> MessagesNode
    End Enum

    ''' <summary> Builds the navigator tree view. </summary>
    Private Sub BuildNavigatorTreeView()

        Me._PartHeader.Visible = Me.SupportsParts

        Me._SplitContainer.Dock = DockStyle.Fill
        Me._SplitContainer.SplitterDistance = 120
        Me._NavigatorTreeView.Enabled = False
        Me._NavigatorTreeView.Nodes.Clear()

        Dim nodes As New List(Of TreeNode)

        For Each node As TreeViewNode In [Enum].GetValues(GetType(TreeViewNode))
            If node = TreeViewNode.PartsNode AndAlso Not Me.SupportsParts Then Continue For
            If node = TreeViewNode.ShuntNode AndAlso Not Me.SupportsShunt Then Continue For
            nodes.Add(New System.Windows.Forms.TreeNode(node.Description))
            nodes(nodes.Count - 1).Name = node.ToString
            nodes(nodes.Count - 1).Text = node.Description
            If node = TreeViewNode.MessagesNode Then
                Me._TraceMessagesBox.ContainerTreeNode = nodes(nodes.Count - 1)
                Me._TraceMessagesBox.TabCaption = "MESSAGES"
            End If
        Next

        Me._NavigatorTreeView.Nodes.AddRange(nodes.ToArray)
        Me._NavigatorTreeView.Enabled = True

    End Sub

    Dim lastNodeSelected As Windows.Forms.TreeNode

    ''' <summary> Gets the last tree view node selected. </summary>
    ''' <value> The last tree view node selected. </value>
    Private ReadOnly Property LastTreeViewNodeSelected As TreeViewNode
        Get
            If lastNodeSelected Is Nothing Then
                Return 0
            Else
                Return CType([Enum].Parse(GetType(TreeViewNode), lastNodeSelected.Name), TreeViewNode)
            End If
        End Get
    End Property

    Private _NodesVisited As List(Of TreeViewNode)

    ''' <summary>
    ''' Called after a node is selected. Displays to relevant screen.
    ''' </summary>
    ''' <param name="node">The node.</param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OnNodeSelected(ByVal node As TreeViewNode)

        If Me._NodesVisited Is Nothing Then
            Me._NodesVisited = New List(Of TreeViewNode)
        End If
        Dim activeControl As Control = Nothing
        Dim focusControl As Control = Nothing
        Dim activeDisplay As DataGridView = Nothing

        Select Case node

            Case TreeViewNode.ConnectNode

                activeControl = Me._ConnectTabLayout
                focusControl = Nothing
                activeDisplay = Nothing

            Case TreeViewNode.ConfigureNode

                activeControl = Me._MainLayout
                focusControl = Nothing
                activeDisplay = Nothing

            Case TreeViewNode.MeasureNode

                activeControl = Me._TtmLayout
                focusControl = Nothing
                activeDisplay = Nothing

            Case TreeViewNode.ShuntNode

                activeControl = Me._ShuntLayout
                focusControl = Nothing
                activeDisplay = Nothing

            Case TreeViewNode.PartsNode

                activeControl = Me._PartsLayout
                focusControl = Nothing
                activeDisplay = Nothing

            Case TreeViewNode.MessagesNode

                activeControl = Me._TraceMessagesBox
                focusControl = Nothing
                activeDisplay = Nothing

        End Select

        ' turn off the visibility of the current panel, this turns off the visibility of the
        ' contained controls, which will now be removed from the panel.
        With Me._SplitContainer.Panel2
            .Hide()
            .Controls.Clear()
#If False Then
            If .HasChildren Then
                For Each Control As Control In .Controls
                    .Controls.Remove(Control)
                Next
            End If
#End If
        End With

        If activeControl IsNot Nothing Then
            activeControl.Dock = DockStyle.None
            Me._SplitContainer.Panel2.Controls.Add(activeControl)
            activeControl.Dock = DockStyle.Fill
            activeControl = Nothing
        End If

        ' turn on visibility on the panel -- this toggles the visibility of the contained controls,
        ' which is required for the messages boxes.
        Me._SplitContainer.Panel2.Show()

        If Not Me._NodesVisited.Contains(node) Then
            If focusControl IsNot Nothing Then
                focusControl.Focus()
                focusControl = Nothing
            End If
            Me._NodesVisited.Add(node)
        End If

        If activeDisplay IsNot Nothing Then
            '  DataDirector.UpdateColumnDisplayOrder(activeDisplay, columnOrder)
            activeDisplay = Nothing
        End If

    End Sub

    ''' <summary> Gets or sets a value indicating whether this <see cref="Console"/> is navigating. </summary>
    ''' <remarks> Used to ignore changes in grids during the navigation. The grids go through selecting
    ''' their rows when navigating. </remarks>
    ''' <value> <c>True</c> if navigating; otherwise, <c>False</c>. </value>
    Private Property Navigating As Boolean

    ''' <summary> Handles the BeforeSelect event of the _NavigatorTreeView control. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      The <see cref="System.Windows.Forms.TreeViewCancelEventArgs"/> instance
    ''' containing the event data. </param>
    Private Sub _NavigatorTreeView_BeforeSelect(sender As Object, e As System.Windows.Forms.TreeViewCancelEventArgs) Handles _NavigatorTreeView.BeforeSelect
        navigating = True
    End Sub

    ''' <summary> Handles the AfterSelect event of the Me._NavigatorTreeView control. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      The <see cref="System.Windows.Forms.TreeViewEventArgs" /> instance
    ''' containing the event data. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _NavigatorTreeView_AfterSelect(sender As System.Object, e As System.Windows.Forms.TreeViewEventArgs) Handles _NavigatorTreeView.AfterSelect
        Try
            If Me.lastNodeSelected IsNot Nothing Then
                lastNodeSelected.BackColor = Me._NavigatorTreeView.BackColor
            End If
            If sender IsNot Nothing AndAlso CType(sender, Control).Enabled AndAlso
                e IsNot Nothing AndAlso e.Node IsNot Nothing AndAlso e.Node.IsSelected Then
                Me.lastNodeSelected = e.Node
                e.Node.BackColor = System.Drawing.SystemColors.Highlight
                Me.OnNodeSelected(Me.LastTreeViewNodeSelected)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception occurred;. @'{0}'{1}", System.Reflection.MethodInfo.GetCurrentMethod.Name, ex.ToFullBlownString)
        Finally
            navigating = False
        End Try
    End Sub


    ''' <summary> Selects the navigator tree view node. </summary>
    ''' <param name="node"> The node. </param>
    Private Sub SelectNavigatorTreeViewNode(ByVal node As TreeViewNode)
        Me._NavigatorTreeView.SelectedNode = Me._NavigatorTreeView.Nodes(node.ToString)
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

    ''' <summary> Adds the listeners such as the current trace messages box. </summary>
    Protected Overloads Sub AddPrivateListeners()
        Me.AddPrivateListener(Me._TraceMessagesBox)
    End Sub

    ''' <summary> Assign talker. </summary>
    ''' <param name="talker"> The talker. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="talker")>
    Private Sub _AssignTalker(ByVal talker As ITraceMessageTalker)
        Me._PartsPanel.AssignTalker(Me.Talker)
        Me._TTMConfigurationPanel.AssignTalker(Me.Talker)
        ' My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Assign talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AssignTalker(ByVal talker As ITraceMessageTalker)
        Me._AssignTalker(Me.Talker)
        MyBase.AssignTalker(talker)
        Me.AddListener(Me._TraceMessagesBox)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        If listenerType = Me._TraceMessagesBox.ListenerType Then Me._TraceMessagesBox.ApplyTraceLevel(value)
        Me._PartsPanel.ApplyListenerTraceLevel(listenerType, value)
        Me._TTMConfigurationPanel.ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        ' MyBase.ApplyListenerTraceLevel(listenerType, value)

    End Sub

    ''' <summary> Executes the trace messages box property changed action. </summary>
    ''' <param name="sender">       The sender. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(sender As TraceMessagesBox, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        If String.Equals(propertyName, NameOf(isr.Core.Pith.TraceMessagesBox.StatusPrompt)) Then
            Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusPrompt, Me._StatusLabel)
            Me._StatusLabel.ToolTipText = sender.StatusPrompt
        End If
    End Sub

    ''' <summary> Trace messages box property changed. </summary>
    ''' <param name="sender"> The sender. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceMessagesBox_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _TraceMessagesBox.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._TraceMessagesBox_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, TraceMessagesBox), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Failed reporting Trace Message {0} Property Change;. {1}", e?.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

End Class

