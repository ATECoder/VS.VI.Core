Imports System.ComponentModel
Imports isr.Core.Controls
Imports isr.Core.Controls.CheckBoxExtensions
Imports isr.Core.Pith
Imports isr.VI.Tsp.Script
Imports isr.VI.Tsp.Instrument
''' <summary> TSP Test panel. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="02/07/2007" by="David" revision="2.0.2597.x"> Created. </history>
Public Class TestPanel
    Inherits ListenerFormBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private _InitializingComponents As Boolean
    ''' <summary>
    ''' Initializes a new instance of this class.
    ''' </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        MyBase.New()
        Me._InitializingComponents = True
        Me.InitializeComponent()
        Me._InitializingComponents = False
        Me.queryCommand = ""
        Me.TimingStopwatch = System.Diagnostics.Stopwatch.StartNew
        Me.TspSystem = New TspSystem(New MasterDevice)
        Me._executionTimeTextBox.Visible = False
        Me._executionTimeTextBoxLabel.Visible = False
        With Me._ResourceSelectorConnector
            .Searchable = True
            .Clearable = True
            .Connectible = True
        End With
        Me._TraceMessagesBox.ContainerPanel = Me._messagesTabPage
        Me.AddListeners()
    End Sub

    ''' <summary>
    ''' Disposes of the resources (other than memory) used by the
    ''' <see cref="T:System.Windows.Forms.Form" />.
    ''' </summary>
    ''' <remarks> David, 12/22/2015. </remarks>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Try
                    ' dispose of the timer.
                    Me.DisposeRefreshTimer()
                Finally
                End Try
                Try
                    If Me.TspSystem IsNot Nothing Then
                        Try
                            If Me.TspSystem.IsDeviceOpen Then
                                Me.TspSystem.Device.CloseSession()
                            End If
                        Finally
                        End Try
                        Me.TspSystem.Dispose()
                        Me.TspSystem = Nothing
                    End If
                Finally
                End Try
                Me.TimingStopwatch = Nothing
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " TYPES "

    ''' <summary>Gets or sets Tab index values.</summary>
    Private Enum MainTabsIndex
        ConsoleTabIndex = 0
        TspScripts = 1
        TspFunctions = 2
        TbdTabIndex = 3
        MessagesTabIndex = 4
    End Enum

#End Region

#Region " FORM EVENT HANDLERS "

    ''' <summary> Raises the <see cref="E:System.Windows.Forms.Form.Closing" /> event. Releases all
    ''' publishers. </summary>
    ''' <param name="e"> A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the
    ''' event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor

            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Unloading..;. ")

            ' stop the timer
            Me.StopRefreshTimer()

            ' dispose of the timer.
            Me.DisposeRefreshTimer()

            ' dispose of the instrument
            If Me.TspSystem IsNot Nothing Then
                If Me.TspSystem.IsDeviceOpen Then
                    Me.TspSystem.Device.CloseSession()
                End If
                Me.TspSystem.Dispose()
                Me.TspSystem = Nothing
            End If

            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Saving settings..;. ")
            System.Windows.Forms.Application.DoEvents()

            ' terminate all the singleton objects.
            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Closing. Please wait..;. ")

            ' delay a bit longer
            Dim timer As System.Diagnostics.Stopwatch = Diagnostics.Stopwatch.StartNew
            Do Until timer.ElapsedMilliseconds > 200
                Windows.Forms.Application.DoEvents()
                Threading.Thread.Sleep(10)
            Loop

            If e IsNot Nothing Then e.Cancel = False

        Catch ex As Exception

            Debug.Assert(Not Debugger.IsAttached, ex.ToString)

        Finally
            Application.DoEvents()
            MyBase.OnClosing(e)
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Called upon receiving the <see cref="E:System.Windows.Forms.Form.Load" /> event. </summary>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor
            Trace.CorrelationManager.StartLogicalOperation(Reflection.MethodInfo.GetCurrentMethod.Name)

            ' set the form caption
            Me.Text = My.Application.Info.ProductName & " release " & My.Application.Info.Version.ToString

            ' default to center screen.
            Me.CenterToScreen()

        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception loading the form;. Details: {0}", ex)
            If DialogResult.Abort = MessageBox.Show(ex.ToString, "Exception Occurred", MessageBoxButtons.AbortRetryIgnore,
                                                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                                    MessageBoxOptions.DefaultDesktopOnly) Then
                Application.Exit()
            End If
        Finally
            MyBase.OnLoad(e)
            Trace.CorrelationManager.StopLogicalOperation()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Called upon receiving the <see cref="E:System.Windows.Forms.Form.Shown" /> event. </summary>
    ''' <param name="e"> A <see cref="T:System.EventArgs" /> that contains the event data. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnShown(e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor
            Trace.CorrelationManager.StartLogicalOperation(Reflection.MethodInfo.GetCurrentMethod.Name)

            ' allow form rendering time to complete: process all messages currently in the queue.
            Application.DoEvents()

            If Not Me.DesignMode Then

                ' allow form rendering time to complete: process all messages currently in the queue.
                Application.DoEvents()

                Me._ResourceSelectorConnector.Enabled = False
                Me._clearInterfaceButton.Enabled = False

                ' disable controls
                Me.UpdateConnectionChange(False)

                ' clear the SRQ value.
                Me._srqStatusLabel.Text = "0x.."

                ' allow some events to occur for refreshing the display.
                Application.DoEvents()

                Me._ResourceSelectorConnector.Clearable = True
                Me._ResourceSelectorConnector.Searchable = True
                Me._ResourceSelectorConnector.Connectible = True
                Me._ResourceSelectorConnector.Enabled = True

            End If
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyApplication.TraceEventId, "Ready to open Visa Session;. ")

        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception showing the form;. Details: {0}", ex)
            If Windows.Forms.DialogResult.Abort = MessageBox.Show(ex.ToString, "Exception Occurred", MessageBoxButtons.AbortRetryIgnore,
                                                                  MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                                                  MessageBoxOptions.DefaultDesktopOnly) Then
                Application.Exit()
            End If
        Finally
            MyBase.OnShown(e)
            Trace.CorrelationManager.StopLogicalOperation()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " RESOURCE CONNECTOR "

    Private _ResourceName As String

    ''' <summary> Gets or sets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Property ResourceName As String
        Get
            Return Me._ResourceName
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.ResourceName) Then
                Me._ResourceName = value
            End If
            Me._ResourceSelectorConnector.SelectedResourceName = value
        End Set
    End Property

    ''' <summary> Gets or sets the Search Pattern of the resource. </summary>
    ''' <value> The Search Pattern of the resource. </value>
    Public Property ResourceFilter As String
        Get
            Return Me._ResourceSelectorConnector.ResourcesFilter
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.ResourceFilter) Then
                Me._ResourceSelectorConnector.ResourcesFilter = value
            End If
        End Set
    End Property

    ''' <summary> Displays the resource names based on the device resource search pattern. </summary> 
    Public Sub DisplayNames()
        ' get the list of available resources
        If Me.TspSystem IsNot Nothing AndAlso Me.TspSystem.Device IsNot Nothing AndAlso
            Me.TspSystem.Device.ResourcesFilter IsNot Nothing Then
            Me._ResourceSelectorConnector.ResourcesFilter = Me.TspSystem.Device.ResourcesFilter
        End If
        Me._ResourceSelectorConnector.DisplayResourceNames()
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub connector_Clear(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResourceSelectorConnector.Clear
        Me.Talker?.Publish(TraceEventType.Verbose, My.MyApplication.TraceEventId,
                           "Resetting, clearing and initializing resource;. {0}", Me._ResourceSelectorConnector.SelectedResourceName)
        Me.TspSystem.Device.ResetClearInit()
        Me.Talker?.Publish(TraceEventType.Verbose, My.MyApplication.TraceEventId,
                           "Resource reset, initialized and cleared;. {0}", Me._ResourceSelectorConnector.SelectedResourceName)
    End Sub

    ''' <summary> Connects the instrument by calling a propagating connect command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub connector_Connect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _ResourceSelectorConnector.Connect
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                               "Connecting;. Opening VISA Session to {0}", Me._ResourceSelectorConnector.SelectedResourceName)
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            ' Me.TspSystem.Device.RegisterNotifier(Me._ResourceSelectorConnector.Talker)
            Me.TspSystem.Device.OpenSession(Me._ResourceSelectorConnector.SelectedResourceName, "TSP")
            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                               "Connected;. Opened VISA Session to {0}", Me._ResourceSelectorConnector.SelectedResourceName)
            Me._clearInterfaceButton.Enabled = Me.TspSystem.IsDeviceOpen
        Catch ex As OperationFailedException
            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                               "Failed connecting;. Failed opening VISA Session to {0}. Details: {1}",
                               Me._ResourceSelectorConnector.SelectedResourceName, ex)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId,
                               "Exception occurred connecting;. {0}. Details: {1}",
                               Me._ResourceSelectorConnector.SelectedResourceName, ex)
        Finally
            ' cancel if failed to open
            If Not Me.TspSystem.IsDeviceOpen Then e.Cancel = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary> Disconnects the instrument by calling a propagating disconnect command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub connector_Disconnect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _ResourceSelectorConnector.Disconnect
        Try
            If Me.TspSystem.Device.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                                   "Disconnecting;. Ending access to {0}", Me._ResourceSelectorConnector.SelectedResourceName)
            End If
            Me.TspSystem.Device.CloseSession()
        Catch ex As OperationFailedException
            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                               "Failed disconnecting;. Failed closing VISA Session to {0}. Details: {1}",
                               Me._ResourceSelectorConnector.SelectedResourceName, ex)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId,
                               "Exception occurred disconnecting;. {0}. Details: {1}",
                               Me._ResourceSelectorConnector.SelectedResourceName, ex)
        Finally
            If Me.TspSystem.Device.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                                   "Failed disconnecting;. Failed ending access to {0}",
                                   Me._ResourceSelectorConnector.SelectedResourceName)
            Else
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                                   "Disconnected;. Ended access to {0}", Me._ResourceSelectorConnector.SelectedResourceName)
            End If
            Me._clearInterfaceButton.Enabled = Me.TspSystem.IsDeviceOpen
            Me.Cursor = Cursors.Default

            ' cancel if failed to close
            If Not Me.TspSystem.IsDeviceOpen Then e.Cancel = True

        End Try
    End Sub

    ''' <summary> Displays available instrument names. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub connector_FindNames(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResourceSelectorConnector.FindNames
        Me.DisplayNames()
    End Sub

    ''' <summary> Executes the connector property changed action. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       Specifies the object where the call originated. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As Instrument.ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.SelectedResourceName)
                Me.ResourceName = sender.SelectedResourceName
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Selected {0};. ", sender.SelectedResourceName)
        End Select
    End Sub

    ''' <summary> Event handler. Called by _ResourceNameSelectorConnector for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub connector_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ResourceSelectorConnector.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf connector_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, Instrument.ResourceSelectorConnector), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " TSP SYSTEM "

    ''' <summary> Updates the connection change described by isOpen. </summary>
    ''' <remarks> David, 12/23/2015. </remarks>
    ''' <param name="isOpen"> true if this object is open. </param>
    Private Sub UpdateConnectionChange(ByVal isOpen As Boolean)
        Me.Talker?.Publish(TraceEventType.Verbose, My.MyApplication.TraceEventId, $"{IIf(isOpen, "Connected;. ", "Not connected;. ")}")
        Me._inputTextBox.Enabled = isOpen
        Me._outputTextBox.Enabled = isOpen
        Me._scriptsPanel1.Enabled = isOpen
        Me._scriptsPanel2.Enabled = isOpen
        Me._scriptsPanel3.Enabled = isOpen
        Me._scriptsPanel4.Enabled = isOpen
        Me._functionsPanel1.Enabled = isOpen
        Me._loadFunctionButton.Enabled = isOpen
        Me._instrumentPanel1.Enabled = isOpen
        Me._instrumentPanel2.Enabled = isOpen
        Me._instrumentPanel3.Enabled = isOpen
    End Sub

    ''' <summary> Executes the connection changed action. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges")>
    Private Sub OnConnectionChanged()

        If Me.TspSystem Is Nothing Then
            Return
        End If

        Dim wasEnabled As Boolean = Me._ResourceSelectorConnector.Enabled
        Dim value As Boolean = Me.TspSystem.IsDeviceOpen

        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyApplication.TraceEventId,
                               $"{IIf(value, "Connection changed--connecting;. ", "Connection changed--disconnecting;. ")}")
            If value Then

                ' get the instrument ID
                If Me.TspSystem IsNot Nothing AndAlso Me.TspSystem.IsDeviceOpen Then

                    If Me.TspSystem.IsSessionOpen Then
                        Me._ResourceSelectorConnector.SelectedResourceName = Me.TspSystem.Device.ResourceName
                    End If

                    AddHandler Me.TspSystem.Device.InteractiveSubsystem.PropertyChanged, AddressOf InteractiveSubsystemPropertyChanged
                    AddHandler Me.TspSystem.Device.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
                    AddHandler Me.TspSystem.Device.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged

                    ' flush the input buffer in case the instrument has some leftovers.
                    Me.TspSystem.Device.StatusSubsystem.ReadServiceRequestStatus()
                    If Me.TspSystem.Device.StatusSubsystem.MessageAvailable Then
                        Me.receive(True)
                    End If

                    Me.Talker?.Publish(TraceEventType.Verbose, My.MyApplication.TraceEventId, "Resetting, clearing and initializing the device;. ")
                    Me.TspSystem.Device.ResetClearInit()

                    ' set the error and prompt check boxes.
                    Me.TspSystem.Device.StatusSubsystem.QueryIdentity()
                    If Not Me.TspSystem.Device.InteractiveSubsystem.ProcessExecutionStateEnabled Then
                        ' read execution state explicitly, because session events are disabled.
                        Me.TspSystem.Device.InteractiveSubsystem.ReadExecutionState()
                    End If

                    ' list any user scripts.
                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Listing user scripts;. ")
                    Try
                        Me.listUserScripts()
                    Catch ex As Exception
                        Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId,
                                           "Exception occurred {0};. Details: {1}", Me._statusLabel.Text, ex)
                    End Try

                    Me.StartRefreshTimer(TimeSpan.FromMilliseconds(500))

                End If

            Else
                If Me.TspSystem IsNot Nothing AndAlso Not Me.TspSystem.IsDeviceOpen Then
                    RemoveHandler Me.TspSystem.Device.InteractiveSubsystem.PropertyChanged, AddressOf InteractiveSubsystemPropertyChanged
                    RemoveHandler Me.TspSystem.Device.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged
                    RemoveHandler Me.TspSystem.Device.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
                End If
                Me._srqStatusLabel.Text = "0x.."
                Me.StopRefreshTimer()
            End If


        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId,
                               "Exception occurred {0};. Details: {1}", Me._statusLabel.Text, ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

            Me._ResourceSelectorConnector.Enabled = wasEnabled

            ' update status
            Me.UpdateConnectionChange(Me.TspSystem.IsDeviceOpen)

        End Try

    End Sub

    ''' <summary>Gets or sets the currently built query command string.</summary>
    Dim queryCommand As String

    ''' <summary> Gets or sets reference to the timing stop watch. </summary>
    Dim TimingStopwatch As System.Diagnostics.Stopwatch

    ''' <summary> Gets or sets reference to the TSP system. </summary>
    Private WithEvents TspSystem As TspSystem

    ''' <summary> Event handler. Called by TspSystem for connection changed events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Form"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TspSystem_ConnectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TspSystem.ConnectionChanged
        Me.OnConnectionChanged()
    End Sub

#Region " STATUS "

    ''' <summary> Handles the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal subsystem As StatusSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.Identity)
                If Not String.IsNullOrWhiteSpace(subsystem.Identity) Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "{0} is {1};. ", Me.ResourceName, subsystem.Identity)
                End If
            Case NameOf(subsystem.DeviceErrors)
                If Not String.IsNullOrWhiteSpace(subsystem.DeviceErrors) Then
                    Me.Talker?.Publish(TraceEventType.Warning, My.MyApplication.TraceEventId, "{0} error {1};. ", Me.ResourceName, subsystem.DeviceErrors)
                End If
            Case NameOf(subsystem.LastDeviceError)
                If subsystem.LastDeviceError?.IsError Then
                    Me.Talker?.Publish(TraceEventType.Warning, My.MyApplication.TraceEventId,
                                       "{0} error {1};. ", Me.ResourceName, subsystem.LastDeviceError.CompoundErrorMessage)
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
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " INTERACTIVE "

    ''' <summary> Handles the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As Tsp.InteractiveSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.ExecutionState)
                Me._tspStatusLabel.Text = subsystem.ExecutionStateCaption
            Case NameOf(subsystem.ShowErrors)
                Me._showErrorsCheckBox.SafeSilentCheckStateSetter(subsystem.ShowErrors.ToCheckState)
            Case NameOf(subsystem.ShowPrompts)
                Me._showPromptsCheckBox.SafeSilentCheckStateSetter(subsystem.ShowPrompts.ToCheckState)
        End Select
    End Sub

    ''' <summary> Interactive subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub InteractiveSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, Tsp.InteractiveSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
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
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#End Region

#Region " METHODS "

    ''' <summary>Gets the current line of the text box.
    ''' </summary>
    ''' <param name="control">Specifies reference to a text box.</param>
    Private Shared Function getLineOld(ByVal control As System.Windows.Forms.TextBoxBase) As String
        Dim commandText As String
        Dim commandStart As Integer
        Dim commandEnd As Integer
        Dim commandLength As Integer

        ' save the text
        commandText = control.Text

        ' lookup the command start
        commandStart = control.SelectionStart

        If commandStart > 0 Then
            Do Until (commandStart = 1) Or (Asc(Mid(commandText, commandStart, 1)) = 10)
                commandStart = commandStart - 1
            Loop
            If commandStart > 1 Then
                commandStart = commandStart + 1
            End If
            ' check if operator is entering returns
            If commandStart < commandText.Length Then
                commandLength = commandText.Length
                commandEnd = control.SelectionStart
                Do Until (commandEnd = commandLength) Or (Asc(Mid(commandText, commandEnd, 1)) = 13)
                    commandEnd = commandEnd + 1
                Loop
                If commandEnd < commandLength Then
                    commandEnd = commandEnd - 1
                End If
                commandLength = commandEnd - commandStart + 1
                getLineOld = Mid(commandText, commandStart, commandLength)
            Else
                getLineOld = ""
            End If
        Else
            getLineOld = ""
        End If

    End Function

    ''' <summary>List the user scripts.
    ''' </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Function listUserScripts() As Boolean

        Dim refreshTimeState As TimerStates = TimerStates.None
        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            If Me._refreshTimer IsNot Nothing Then
                refreshTimeState = Me._refreshTimer.TimerState
                Me.StopRefreshTimer()
            End If

            If Me.TspSystem IsNot Nothing AndAlso Me.TspSystem.IsDeviceOpen Then
                If Me.TspSystem.IsSessionOpen Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Flushing the read buffer...;. ")
                    Me.TspSystem.Device.Session.DiscardUnreadData()
                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Fetching script names...;. ")
                    Dim scriptCount As Integer = Me.TspSystem.ScriptManager.FetchUserScriptNames()
                    If scriptCount > 0 Then
                        Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Listing script names...;. ")
                        Me._userScriptsList.DataSource = Nothing
                        Me._userScriptsList.Items.Clear()
                        Me._userScriptsList.DataSource = Me.TspSystem.ScriptManager.UserScriptNames
                        If Me._userScriptsList.Items.Count > 0 Then
                            Me._userScriptsList.SelectedIndex = 0
                        End If
                    Else
                        Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "No Scripts;. ")
                        Me._userScriptsList.DataSource = Nothing
                        Me._userScriptsList.Items.Clear()
                    End If
                End If
            End If

        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred listing scripts;. Details: {0}", ex)

        Finally

            If refreshTimeState <> TimerStates.None Then
                Me.StartRefreshTimer(TimeSpan.FromMilliseconds(500))
            End If

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Function

    Private receiveLock As New Object
    ''' <summary>Receive a message from the instrument.
    ''' </summary>
    ''' <param name="updateConsole">True to update the instrument output text box.</param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub receive(ByVal updateConsole As Boolean)

        SyncLock (receiveLock)

            Try

                ' read a message if we have messages.
                Dim receiveBuffer As String = ""
                If updateConsole AndAlso Me.TspSystem.Device.StatusSubsystem.IsMessageAvailable(TimeSpan.FromMilliseconds(50), 3) Then

                    ' Turn on the form hourglass
                    Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Reading..;. ")

                    If Me.TspSystem IsNot Nothing AndAlso Me.TspSystem.IsSessionOpen Then
                        receiveBuffer = Me.TspSystem.Device.Session.ReadLine()
                    End If

                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Done Reading;. ")

                    Me._outputTextBox.SelectionStart = Me._outputTextBox.Text.Length
                    Me._outputTextBox.SelectionLength = 0
                    Me._outputTextBox.SelectedText = receiveBuffer & vbCrLf
                    Me._outputTextBox.SelectionStart = Me._outputTextBox.Text.Length

                    ' stop the timer and get execution time in ms.
                    Me.TimingStopwatch.Stop()
                    Me._executionTimeTextBox.Text = Me.TimingStopwatch.Elapsed.TotalMilliseconds.ToString("0.0", Globalization.CultureInfo.CurrentCulture)

                End If

            Catch ex As Exception

                Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred receiving;. Details: {0}", ex)

            Finally

                ' Turn off the form hourglass
                Me.Cursor = System.Windows.Forms.Cursors.Default

            End Try

        End SyncLock

    End Sub

    ''' <summary> Sends a message to the instrument. </summary>
    ''' <param name="sendBuffer">         Specifies the message to send. </param>
    ''' <param name="updateInputConsole"> True to update the instrument input text box. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub send(ByVal sendBuffer As String, ByVal updateInputConsole As Boolean)

        ' Turn on the form hourglass
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Try

            If Not String.IsNullOrWhiteSpace(sendBuffer) Then
                Me._refreshTimer.Enabled = False

                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Sending query/command..;. ")

                ' start the timing timer
                Me.TimingStopwatch = System.Diagnostics.Stopwatch.StartNew

                Me.TspSystem.Device.Session.WriteLine(sendBuffer)
                Me.TspSystem.Device.StatusSubsystem.TraceVisaOperation("sending query/command;. '{0}'", sendBuffer)

                If updateInputConsole Then
                    Me._inputTextBox.SelectionStart = Me._inputTextBox.Text.Length
                    Me._inputTextBox.SelectionLength = 0
                    Me._inputTextBox.SelectedText = sendBuffer & vbCrLf
                    Me._inputTextBox.SelectionStart = Me._inputTextBox.Text.Length
                End If

            End If

        Catch ex As NativeException
            Me.TspSystem.Device.StatusSubsystem.TraceVisaOperation(ex, "sending query/command;. '{0}'", sendBuffer)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred sending;. Details: {0}", ex)
        Finally

            Me._refreshTimer.Enabled = True

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    Private _validatedGpibAddress As Nullable(Of Integer)
    ''' <summary>
    ''' Enables connecting to the GPIB board.
    ''' </summary>
    Private Function ValidatedGpibAddress(ByVal address As String) As Nullable(Of Integer)
        Dim value As Integer
        If Integer.TryParse(address, System.Globalization.NumberStyles.Any, Globalization.CultureInfo.CurrentCulture, value) Then
            If Not Me._validatedGpibAddress.HasValue OrElse Me._validatedGpibAddress.Value <> value OrElse Not Me._ResourceSelectorConnector.Enabled Then
                Dim resourceName As String = VI.ResourceNamesManager.BuildGpibInstrumentResource("0", address)
                Using rm As ResourcesManagerBase = isr.VI.SessionFactory.Get.Factory.CreateResourcesManager()
                    Me._ResourceSelectorConnector.Enabled = rm.InstrumentExists(resourceName)
                End Using
            End If
            Me._validatedGpibAddress = value
        Else
            Me._validatedGpibAddress = New Nullable(Of Integer)()
        End If
        Return Me._validatedGpibAddress
    End Function

#End Region

#Region " REFRESH TIMER "

    ''' <summary> Dispose refresh timer. </summary>
    Private Sub DisposeRefreshTimer()
        ' dispose of the timer.
        If Me._refreshTimer IsNot Nothing Then
            Me._refreshTimer.Close()
            Me._refreshTimer.Dispose()
            Me._refreshTimer = Nothing
        End If
    End Sub

    ''' <summary> Starts refresh timer. </summary>
    ''' <param name="interval"> The interval. </param>
    Private Sub StartRefreshTimer(ByVal interval As TimeSpan)
        Me._refreshTimer = New isr.Core.Controls.StateAwareTimer
        Me._refreshTimer.SynchronizingObject = Me
        Me.RestartRefreshTimer(interval)
    End Sub

    ''' <summary> Restarts refresh timer. </summary>
    ''' <param name="interval"> The interval. </param>
    Private Sub RestartRefreshTimer(ByVal interval As TimeSpan)
        If Me._refreshTimer IsNot Nothing Then
            Me._refreshTimer.Interval = interval.TotalMilliseconds
            Me._refreshTimer.AutoReset = True
            Me._refreshTimer.Start()
        End If
    End Sub

    ''' <summary> Stops the refresh timer. </summary>
    Private Sub StopRefreshTimer()
        If Me._refreshTimer IsNot Nothing Then
            Me._refreshTimer.Stop()
            Me._refreshTimer.AutoReset = False
            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Timer stopped;. ")
        End If
    End Sub

    ''' <summary> Status register visible setter. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Sub StatusRegisterVisibleSetter(ByVal value As Boolean)
        isr.Core.Controls.ToolStripExtensions.SafeVisibleSetter(Me._srqStatusLabel, value)
    End Sub

    ''' <summary> Displays the status register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Public Sub DisplayStatusRegisterStatus(ByVal value As Integer?)
        Me.DisplayStatusRegisterStatus(value, "0x{0:X2}")
    End Sub

    ''' <summary> Displays the status register status. </summary>
    ''' <param name="value">  The register value. </param>
    ''' <param name="format"> The format. </param>
    Public Sub DisplayStatusRegisterStatus(ByVal value As Integer?, ByVal format As String)
        isr.Core.Controls.ToolStripExtensions.SafeTextSetter(Me._srqStatusLabel, value, format)
    End Sub

    Private WithEvents _refreshTimer As isr.Core.Controls.StateAwareTimer
    ''' <summary>
    ''' Serial polls and receives.
    ''' </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _refreshTimer_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _refreshTimer.Tick

        Try

            If Me.TspSystem IsNot Nothing AndAlso Me.TspSystem.IsDeviceOpen Then
                If Me.TspSystem.IsSessionOpen Then
                    Me.TspSystem.Device.StatusSubsystem.ReadServiceRequestStatus()
                End If
                Me.DisplayStatusRegisterStatus(Me.TspSystem.Device.StatusSubsystem.ServiceRequestStatus)

                ' check if the console tab is open
                If Me._tabControl.SelectedIndex = MainTabsIndex.ConsoleTabIndex Then

                    Me.receive(True)

                End If

            End If

            Me._tspStatusLabel.Text = Me.TspSystem.Device.InteractiveSubsystem.ExecutionStateCaption

        Catch ex As Exception

            ' stop the time on error
            Me.StopRefreshTimer()

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred receiving;. Details: {0}", ex)


        Finally

        End Try

    End Sub

#End Region

#Region " TESTING TIMER "

    Private WithEvents _testingTimer As isr.Core.Controls.StateAwareTimer

    ''' <summary>
    ''' Serial polls and receives.
    ''' </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")> Private Sub _testingTimer_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _testingTimer.Tick

        Try

            Me._srqStatusLabel.Text = DateTime.Now.ToLongTimeString

        Catch ex As Exception

            ' stop the time on error
            Me._testingTimer.Stop()

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred timing;. Details: {0}", ex)

        Finally

        End Try

    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS "

    ''' <summary>
    ''' Terminates script execution.
    ''' </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _abortButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _abortButton.Click

        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            ' Terminates script execution when called from a script that is being
            ' executed. This command will not wait for overlapped commands to complete
            ' before terminating script execution. If overlapped commands are required
            ' to finish, use the wait complete function prior to calling exit.
            send("exit() ", True)

            If Not Me.TspSystem.Device.InteractiveSubsystem.ProcessExecutionStateEnabled Then
                ' read execution state explicitly, because session events are disabled.
                ' update the instrument status.
                Me.TspSystem.Device.InteractiveSubsystem.ReadExecutionState()
            End If


            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Aborted;. ")

        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred clearing device;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary>
    ''' Displays the application information
    ''' </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _aboutButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _aboutButton.Click

        ' Display the application information
        Using aboutScreen As New isr.Core.WindowsForms.About
            aboutScreen.Image = Me.Icon
            aboutScreen.ShowDialog(System.Reflection.Assembly.GetExecutingAssembly,
              "", "", "", "")
        End Using

        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred displaying about panel;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Toggle the font on the control to highlight selection of this control. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub _button_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _ExitButton.Enter, _abortButton.Enter, _aboutButton.Enter, _callFunctionButton.Enter,
      _clearInterfaceButton.Enter, _deviceClearButton.Enter, _ExitButton.Enter, _groupTriggerButton.Enter,
       _removeScriptButton.Enter, _resetLocalNodeButton.Enter, _resetLocalNodeButton.Enter, _refreshUserScriptsListButton.Enter
        Dim thisButton As Control = CType(eventSender, Control)
        thisButton.Font = New Font(thisButton.Font, FontStyle.Bold)
    End Sub

    ''' <summary> Toggle the font on the control to highlight leaving this control. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub _button_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _ExitButton.Leave, _abortButton.Leave, _aboutButton.Leave, _callFunctionButton.Leave,
      _clearInterfaceButton.Leave, _deviceClearButton.Leave, _ExitButton.Leave, _groupTriggerButton.Leave,
       _removeScriptButton.Leave, _resetLocalNodeButton.Leave, _resetLocalNodeButton.Leave, _refreshUserScriptsListButton.Leave
        Dim thisButton As Control = CType(eventSender, Control)
        thisButton.Font = New Font(thisButton.Font, FontStyle.Regular)
    End Sub

    ''' <summary> Event handler. Called by _clearInterfaceButton for click events. </summary>
    ''' <param name="sender"> The event sender. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _clearInterfaceButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _clearInterfaceButton.Click

        Try
            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me._errorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                               "Clearing interface @'{0}';. ", Me.TspSystem.Device.Session.ResourceName)
            Me.TspSystem.Device.SystemSubsystem.ClearInterface()
        Catch ex As Exception
            Me._errorProvider.SetError(CType(sender, Windows.Forms.Control), ex.ToString)
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId,
                               "Exception occurred clearing interface;. Details: {0}", ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try

    End Sub

    ''' <summary> Event handler. Called by _callFunctionButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _callFunctionButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _callFunctionButton.Click

        Dim functionName As String = ""
        Dim functionArgs As String = ""
        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            functionName = Me._functionNameTextBox.Text

            functionArgs = Me._functionArgsTextBox.Text

            If Not String.IsNullOrWhiteSpace(functionName) Then
                Tsp.TspSyntax.CallFunction(TspSystem.Device.Session, functionName, functionArgs)
                Me.TspSystem.Device.StatusSubsystem.TraceVisaOperation("calling function '{0}({1})';. ", functionName, functionArgs)
            End If

        Catch ex As NativeException
            Me.TspSystem.Device.StatusSubsystem.TraceVisaOperation(ex, "calling function '{0}({1})';. ", functionName, functionArgs)
        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred calling function;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Event handler. Called by _deviceClearButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _deviceClearButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _deviceClearButton.Click

        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            If Me.TspSystem.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Clearing device..;. ")
                Me.TspSystem.Device.StatusSubsystem.ClearActiveState()
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Device cleared.;. ")
            End If

        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred during device clear;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Event handler. Called by _ExitButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub _ExitButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _ExitButton.Click
        Me.Close()
    End Sub

    ''' <summary> Event handler. Called by _groupTriggerButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _groupTriggerButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _groupTriggerButton.Click,
                                                                                                            _groupTriggerButton.Click

        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            If TspSystem.IsSessionOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Asserting trigger..;. ")
                Me.TspSystem.Device.Session.AssertTrigger()
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Trigger asserted.;. ")
            End If

        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred triggering device;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try
    End Sub

    ''' <summary> Event handler. Called by _inputTextBox for key press events. 
    '''           Handles key at the command console.
    '''           </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Key press event information. </param>
    Private Sub _inputTextBox_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles _inputTextBox.KeyPress

        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)

        Select Case KeyAscii

            Case System.Windows.Forms.Keys.Return ' enter key

                ' User pressed enter key.  Send the string to the instrument.

                ' Disable the command console while waiting for command to process.
                Me._inputTextBox.Enabled = False

                ' Enable the abort button.
                Me._abortButton.Enabled = False

                ' dh 6.0.02 use command line at cursor
                If Me._inputTextBox.Lines IsNot Nothing AndAlso Me._inputTextBox.Lines.Length > 0 Then
                    Dim line As Integer = Me._inputTextBox.GetLineFromCharIndex(Me._inputTextBox.SelectionStart)
                    Me.queryCommand = Me._inputTextBox.Lines(line)
                Else
                    Me.queryCommand = ""
                End If
                'queryCommand = getLine(Me._inputTextBox)

                ' Send the string to the instrument. Receive is invoked by the timer.
                Me.send(Me.queryCommand, False)

                ' TO_DO: this is a perfect place for setting a service request before read and
                ' letting the service request invoke the next read.

                ' clear the command
                Me.queryCommand = ""

                ' Turn command console back on and display response.
                Me._abortButton.Enabled = False
                Me._inputTextBox.Enabled = True
                Me._inputTextBox.Focus()

            Case System.Windows.Forms.Keys.Back ' backspace key
                If queryCommand.Length < 2 Then
                    Me.queryCommand = ""
                Else
                    Me.queryCommand = queryCommand.Substring(0, queryCommand.Length - 1)
                End If

            Case Else ' normal key
                ' Stash key in buffer.
                Me.queryCommand &= Chr(KeyAscii)
        End Select

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    ''' <summary>
    ''' _loads the and run script.
    ''' </summary>
    ''' <param name="scriptName">Name of the script.</param>
    ''' <param name="filePath">The file path.</param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _loadAndRunScript(ByVal scriptName As String, ByVal filePath As String)

        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            If Me.TspSystem.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Loading script '{0}';. from '{1}'.", scriptName, filePath)
                Me.TspSystem.ScriptManager.ScriptNameSetter(scriptName)
                Me.TspSystem.ScriptManager.FilePath = filePath
                Me.TspSystem.ScriptManager.LoadScriptFile(True, True, Me._retainCodeOutlineToggle.Checked)
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Script '{0}' loaded;. from '{1}'.", scriptName, filePath)
                listUserScripts()
                Me.TspSystem.ScriptManager.RunScript(TimeSpan.FromMilliseconds(3000))
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Script '{0}' run okay;. ", scriptName, filePath)

            End If

        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred loading or running script;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Event handler. Called by _loadAndRunButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub _loadAndRunButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _loadAndRunButton.Click

        Me._loadAndRunScript(Me._scriptNameTextBox.Text, Me._tspScriptSelector.FilePath)

    End Sub

    ''' <summary> Event handler. Called by _loadFunctionButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _loadFunctionButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _loadFunctionButton.Click

        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            Dim functionCode As String
            functionCode = Me._functionCodeTextBox.Text.Replace(vbCrLf, Space(1))
            If Me.TspSystem.IsSessionOpen AndAlso Not String.IsNullOrWhiteSpace(functionCode) Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Loading function code..;. ")
                Me.TspSystem.Device.Session.WriteLine(functionCode)
                Me.TspSystem.Device.StatusSubsystem.TraceVisaOperation("loading function code;. ")
            End If

        Catch ex As NativeException
            Me.TspSystem.Device.StatusSubsystem.TraceVisaOperation(ex, "loading function code;. ")
        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred loading function;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Event handler. Called by _loadScriptButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _loadScriptButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _loadScriptButton.Click

        If Not Tsp.Script.ScriptEntityBase.IsValidScriptFileName(Me._scriptNameTextBox.Text) Then
            Return
        End If
        ' Turn on the form hourglass
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Try

            Me.TspSystem.ScriptManager.ScriptNameSetter(Me._scriptNameTextBox.Text)
            Me.TspSystem.ScriptManager.FilePath = Me._tspScriptSelector.FilePath
            If Me.TspSystem.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                                   "Loading script '{0}';. from '{1}'.",
                                   Me.TspSystem.ScriptManager.Name, Me.TspSystem.ScriptManager.FilePath)
                Me.TspSystem.ScriptManager.LoadScriptFile(True, True, Me._retainCodeOutlineToggle.Checked)
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                                   "Script '{0}' loaded;. from '{1}'.",
                                   Me.TspSystem.ScriptManager.Name, Me.TspSystem.ScriptManager.FilePath)
                listUserScripts()
            End If

        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred loading script;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try


    End Sub

    ''' <summary> Event handler. Called by _refreshUserScriptsListButton for click events. Updates the
    ''' list of user scripts. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Form"/> </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _refreshUserScriptsListButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _refreshUserScriptsListButton.Click
        Me.listUserScripts()
    End Sub

    ''' <summary> Event handler. Called by _removeScriptButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _removeScriptButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _removeScriptButton.Click

        ' Turn on the form hourglass
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Try

            If Me._userScriptsList.SelectedItems.Count > 0 Then
                For Each scriptName As String In Me._userScriptsList.SelectedItems
                    If Me.TspSystem.IsSessionOpen Then
                        Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Removing script '{0}'..;. ", scriptName)
                        Me.TspSystem.ScriptManager.RemoveScript(scriptName)
                        Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Removed script '{0}';. ", scriptName)
                    End If
                Next
                listUserScripts()
            End If

        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred removing script;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Event handler. Called by _resetLocalNodeButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _resetLocalNodeButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _resetLocalNodeButton.Click

        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            If Me.TspSystem IsNot Nothing AndAlso Me.TspSystem.IsDeviceOpen Then

                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Resetting local node..;. ")
                send("localnode.reset() ", True)
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Local node was reset;. ")

                If Not Me.TspSystem.Device.InteractiveSubsystem.ProcessExecutionStateEnabled Then
                    ' read execution state explicitly, because session events are disabled.
                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Updating instrument state..;. ")
                    Me.TspSystem.Device.InteractiveSubsystem.ReadExecutionState()

                End If

                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Clearing error queue..;. ")
                send("localnode.errorqueue.clear() ", True)
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Error queue cleared;. ")

                ' update the instrument state.
                If Not Me.TspSystem.Device.InteractiveSubsystem.ProcessExecutionStateEnabled Then
                    ' read execution state explicitly, because session events are disabled.
                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Updating instrument state..;. ")
                    Me.TspSystem.Device.InteractiveSubsystem.ReadExecutionState()
                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Instrument state updated;. ")
                End If

            End If

        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred setting prompts;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Event handler. Called by _runScriptButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _runScriptButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _runScriptButton.Click

        Try

            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            Me.TspSystem.ScriptManager.ScriptNameSetter(Me._scriptNameTextBox.Text)
            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Running script '{0}';. ", Me.TspSystem.ScriptManager.Name)
            Me.TspSystem.ScriptManager.RunScript(TimeSpan.FromMilliseconds(3000))
            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Script '{0}' ran okay;. ", Me.TspSystem.ScriptManager.Name)

        Catch ex As Exception

            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred running;. Details: {0}", ex)

        Finally

            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Event handler. Called by _showErrorsCheckBox for check state changed events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _showErrorsCheckBox_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _showErrorsCheckBox.CheckStateChanged
        If Me._InitializingComponents Then Return
        Try
            ' Turn on the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            If Me._showErrorsCheckBox.Enabled AndAlso Me.TspSystem IsNot Nothing AndAlso Me.TspSystem.IsDeviceOpen Then
                If Me.TspSystem.Device.InteractiveSubsystem.QueryShowErrors() <> Me._showErrorsCheckBox.Checked Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Toggling showing errors..;. ")
                    Me.TspSystem.Device.InteractiveSubsystem.WriteShowErrors(Me._showErrorsCheckBox.Checked)
                    If Not Me.TspSystem.Device.InteractiveSubsystem.ShowErrors.HasValue Then
                        Me.Talker?.Publish(TraceEventType.Warning, My.MyApplication.TraceEventId,
                                           "Failed toggling showing errors--value not set;. ")
                    ElseIf Me.TspSystem.Device.InteractiveSubsystem.ShowErrors.Value <> Me._showErrorsCheckBox.Checked Then
                        Me.Talker?.Publish(TraceEventType.Warning, My.MyApplication.TraceEventId,
                                           "Failed toggling showing errors--incorrect value;. ")
                    End If
                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                                       "Showing errors: {0};. ", IIf(Me.TspSystem.Device.InteractiveSubsystem.ShowErrors.Value, "ON", "OFF"))
                End If
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred showing errors;. Details: {0}", ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _showPromptsCheckBox for check state changed events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _showPromptsCheckBox_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _showPromptsCheckBox.CheckStateChanged
        If Me._InitializingComponents Then Return
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            If Me._showPromptsCheckBox.Enabled AndAlso Me.TspSystem IsNot Nothing AndAlso Me.TspSystem.IsDeviceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Toggling showing prompts..;. ")

                If Me.TspSystem.Device.InteractiveSubsystem.QueryShowPrompts() <> Me._showPromptsCheckBox.Checked Then
                    Me.TspSystem.Device.InteractiveSubsystem.WriteShowPrompts(Me._showPromptsCheckBox.Checked)

                    If Not Me.TspSystem.Device.InteractiveSubsystem.ShowPrompts.HasValue Then
                        Me.Talker?.Publish(TraceEventType.Warning, My.MyApplication.TraceEventId,
                                           "Failed toggling showing Prompts--value not set;. ")
                    ElseIf Me.TspSystem.Device.InteractiveSubsystem.ShowPrompts.Value <> Me._showPromptsCheckBox.Checked Then
                        Me.Talker?.Publish(TraceEventType.Warning, My.MyApplication.TraceEventId,
                                           "Failed toggling showing Prompts--incorrect value;. ")
                    End If
                    Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId,
                                       "Showing Prompts: {0};. ", IIf(Me.TspSystem.Device.InteractiveSubsystem.ShowPrompts.Value, "ON", "OFF"))
                End If
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred showing prompts;. Details: {0}", ex)
        Finally
            ' Turn off the form hourglass
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by _tspScriptSelector for selected changed events. </summary>
    ''' <param name="Sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _tspScriptSelector_SelectedChanged(ByVal Sender As System.Object, ByVal e As EventArgs) Handles _tspScriptSelector.SelectedChanged
        If Not String.IsNullOrWhiteSpace(Me._tspScriptSelector.FileTitle) Then
            Me._scriptNameTextBox.Text = Me._tspScriptSelector.FileTitle.Replace(".", "_")
            Me.Talker?.Publish(TraceEventType.Information, My.MyApplication.TraceEventId, "Selected Script;. '{0}'.", Me._scriptNameTextBox.Text)
        End If
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds the listeners such as the current trace messages box. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    Protected Overloads Sub AddListeners()
        Me.Talker.Listeners.Add(Me._TraceMessagesBox)
        'Me._InstrumentPanel.AddListeners(Me.Talker.Listeners)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    Public Overrides Sub AddListeners(ByVal log As MyLog)
        If log Is Nothing Then Throw New ArgumentNullException(NameOf(log))
        MyBase.AddListeners(log)
        'Me._InstrumentPanel.AddListeners(New ITraceMessageListener() {log})
        My.MyApplication.Identify(Me.Talker)
    End Sub

    ''' <summary> Handles the <see cref="_TraceMessagesBox"/> property changed event. </summary>
    ''' <remarks> David, 9/5/2016. </remarks>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(sender As TraceMessagesBox, propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.Caption)
                Me._tabControl.TabPages.Item(MainTabsIndex.MessagesTabIndex).Text = sender.Caption()
            Case NameOf(sender.StatusPrompt)
                Me._statusLabel.Text = sender.StatusPrompt
                Me._statusLabel.ToolTipText = sender.StatusPrompt
        End Select
    End Sub

    ''' <summary> Trace messages box property changed. </summary>
    ''' <remarks> David, 9/5/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceMessagesBox_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _TraceMessagesBox.PropertyChanged
        Try
            ' there was a cross thread exception because this event is invoked from the control thread.
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._TraceMessagesBox_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, TraceMessagesBox), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Failed reporting Trace Message Property Change;. Details: {0}", ex)
        End Try
    End Sub

#End Region

End Class
