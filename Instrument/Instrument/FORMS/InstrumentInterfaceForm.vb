Imports System.Windows.Forms
Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Instrument Interface Panel. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="02/07/2005" by="David" revision="2.0.2597.x"> Created. </history>
<CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
Public Class InstrumentInterfaceForm
    Inherits ListenerFormBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private _InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me._InitializingComponents = True
        Me.InitializeComponent()
        Me._InitializingComponents = False
        Me._TraceMessagesBox.ContainerPanel = Me._messagesTabPage
        Me.AddListeners()
    End Sub

    ''' <summary>
    ''' Disposes of the resources (other than memory) used by the
    ''' <see cref="T:System.Windows.Forms.Form" />.
    ''' </summary>
    ''' <remarks> David, 12/19/2015. </remarks>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' Free managed resources when explicitly called
                If Me.Session IsNot Nothing Then
                    Me.CloseInstrumentSession()
                End If
                If Me._instrumentChooser IsNot Nothing Then Me._instrumentChooser.Dispose() : Me._instrumentChooser = Nothing

                ' unable to use null conditional because it is not seen by code analysis
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If

        Finally

            ' Invoke the base class dispose method
            MyBase.Dispose(disposing)

        End Try
    End Sub

#End Region

#Region " MODULE DATA MEMBERS "

    ''' <summary> Gets reference to the
    ''' <see cref="VI.SessionBase">message based session</see>.
    ''' Was Ivi.VI.Interop.IGpib. </summary>
    ''' <value> The session. </value>
    Private Property Session As VI.SessionBase

    ''' <summary> Gets the last message that was received from the instrument. </summary>
    ''' <value> A Buffer for receive data. </value>
    Private Property receiveBuffer As String

    ''' <summary> Gets the last message that was sent to the instrument. </summary>
    ''' <value> A Buffer for transmit data. </value>
    Private Property transmitBuffer As String

#End Region

#Region " CONNECT / DISCONNECT "

    ''' <summary> Gets the condition for determining if the interface session is open (connected). </summary>
    ''' <value> <c>True</c> if a VISA session exists. </value>
    Private ReadOnly Property IsOpen() As Boolean
        Get
            Return Me.Session IsNot Nothing AndAlso Me.Session.IsSessionOpen
        End Get
    End Property

    ''' <summary> Opens a visa session to the instrument. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OpenInstrumentSession()

        Dim wasOpen As Boolean
        wasOpen = Me.IsOpen

        ' close the device if open
        If wasOpen Then
            Me.CloseInstrumentSession()
        End If

        Dim lastAction As String = "N/A"
        Try

            ' clear values
            Me.receiveBuffer = String.Empty
            Me.transmitBuffer = String.Empty

            lastAction = "Initializing driver"
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               $"{lastAction};. ".ToString(Globalization.CultureInfo.CurrentCulture))
            Dim resourceName As String = ""
            resourceName = Me._InterfacePanel.InstrumentChooser.SelectedResourceName

            lastAction = $"Opening a VISA Session to {resourceName}"
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
            Me.Session = isr.VI.SessionFactory.Get.Factory.CreateSession()
            Me.Session.OpenSession(resourceName, Threading.SynchronizationContext.Current)

            If Me.IsOpen Then
                lastAction = "Clearing the device"
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                Me.Session.Clear()
                lastAction = $"Connected to {Me.Session.ResourceName}"
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{lastAction};. ")
            Else
                Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                   "Failed opening a session to {0};. ", resourceName)
            End If

        Catch ex As Exception
            ex.Data.Add("@isr", lastAction)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{lastAction};. {ex.ToFullBlownString}")
            Try
                Me.CloseInstrumentSession()
            Finally
            End Try

        Finally

            ' check if ExecutionState changed.
            If wasOpen <> Me.IsOpen Then
                ' if so, alert on connection changed.
                Me.OnConnectionChanged()
            End If
            Me.Session.RestoreTimeout()

        End Try

    End Sub

    ''' <summary> Closes the device. Performs the necessary termination functions, which will cleanup
    ''' and disconnect the interface connection. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub CloseInstrumentSession()

        Dim wasOpen As Boolean
        wasOpen = Me.IsOpen
        stopPollTimer()

        Dim lastAction As String = "N/A"
        Try

            If Me.IsOpen Then

                lastAction = "Disconnecting Instrument"
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")

                If _SendDisconnectCommandsCheckBox.Checked Then

                    If Me._DisconnectCommandsTextBox.Lines.Length > 0 Then
                        For Each command As String In Me._DisconnectCommandsTextBox.Lines
                            command = command.Trim
                            If Not String.IsNullOrWhiteSpace(command) Then
                                lastAction = $"Sending '{command}'"
                                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                                Try
                                    Me.Session.WriteLine(command)
                                Catch ex As NativeException
                                    Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                                       $"{lastAction} failed;. {ex.ToFullBlownString}")
                                End Try
                            End If
                        Next
                    End If
                End If

                lastAction = "Clearing the device"
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                Me.Session.Clear()

                lastAction = "Disabling service request events if any"
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                Me.DisableServiceRequestEventHandler()

                lastAction = "Ending the VISA session"
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                Me.Session.Dispose()
                Try
                    ' Trying to null the session raises an ObjectDisposedException 
                    ' if session service request handler was not released. 
                    Me.Session = Nothing
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
                End Try

                lastAction = "Session closed"
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")

            End If

        Catch

            Throw

        Finally

            ' check if ExecutionState changed.
            If wasOpen <> Me.IsOpen Then
                ' if so, alert on connection changed.
                Me.OnConnectionChanged()
            End If

        End Try

    End Sub

    ''' <summary> Updates the connection status. </summary>
    Private Sub OnConnectionChanged()

        If Not Me.IsOpen Then
            Me.stopPollTimer()
        End If

        Me._SendButton.Enabled = Me.IsOpen
        Me._ReadStatusRegisterButton.Enabled = Me.IsOpen
        Me._SendComboCommandButton.Enabled = Me.IsOpen
        Me._sendReceiveControlPanel.Enabled = Me.IsOpen

        ' enable by reading SRQ
        Me._ReceiveButton.Enabled = False

        Me._StatusRegisterLabel.Text = String.Empty

    End Sub

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
            Me._StatusLabel.Text = "CLOSING."
            Me.CloseInstrumentSession()
            Me.stopPollTimer()
            If e IsNot Nothing Then
                e.Cancel = False
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
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
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception loading the instrument interface form;. {ex.ToFullBlownString}")
            If DialogResult.Abort = MessageBox.Show(ex.ToFullBlownString, "Exception Occurred", MessageBoxButtons.AbortRetryIgnore,
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

                ' allow connecting the resource.
                Me._instrumentChooser = _InterfacePanel.InstrumentChooser
                Me._instrumentChooser.AddListeners(Me.Talker.Listeners)
                Me._InterfacePanel.InstrumentChooser.Connectible = True

                ' allow form rendering time to complete: process all messages currently in the queue.
                Application.DoEvents()

                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Displaying interface names;. ")
                Me._InterfacePanel.DisplayInterfaceNames()
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Ready to open Visa Session;. ")
                ' select the first item
                If Me._CommandsComboBox.Items.Count > 0 Then
                    Me._CommandsComboBox.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception showing the instrument interface form;. {ex.ToFullBlownString}")
            If DialogResult.Abort = MessageBox.Show(ex.ToFullBlownString, "Exception Occurred", MessageBoxButtons.AbortRetryIgnore,
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

#Region " INTERFACE EVENT HANDLERS "

    Private WithEvents _pollTimer As Timer

    Private WithEvents _instrumentChooser As isr.VI.Instrument.ResourceSelectorConnector

    Private Sub _instrumentChooser_Clear(ByVal sender As Object, ByVal e As System.EventArgs) Handles _instrumentChooser.Clear
        Dim lastAction As String = "N/A"
        Try
            lastAction = "Clearing the device"
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
            Me.Session.Clear()
        Catch ex As NativeException
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{lastAction} failed;. {ex.ToFullBlownString}")
        End Try
    End Sub

    Private Sub _instrumentChooser_Connect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _instrumentChooser.Connect
        ' exception handling is done in the resource connector.
        Me.OpenInstrumentSession()
        ' cancel if failed to open
        If Not Me.IsOpen Then e.Cancel = True
    End Sub

    Private Sub _instrumentChooser_Disconnect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _instrumentChooser.Disconnect
        ' exception handling is done in the resource connector.
        Me.CloseInstrumentSession()
        ' cancel if failed to close.
        If Me.IsOpen Then e.Cancel = True
    End Sub

    ''' <summary> Executes the property changed action. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As InterfacePanel, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.InterfaceResourceName)
                Me._StatusLabel.Text = $"Selected {sender.InterfaceResourceName}"
            Case NameOf(sender.IsInterfaceOpen)
                If sender.IsInterfaceOpen Then
                    Me._StatusLabel.Text = "Select Instrument Resource"
                Else
                    Me._StatusLabel.Text = "Interface Closed"
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _InterfacePanel for property changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InterfacePanel_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InterfacePanel.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._InterfacePanel_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, InterfacePanel), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling InterfacePanel.{e.PropertyName} change event;. {ex.ToFullBlownString}")

        End Try
    End Sub

#End Region

#Region " SERVICE REQUEST MANAGEMENT "

    Private _ServiceRequestBits As Integer

    ''' <summary> Gets or sets the service request bits. </summary>
    ''' <value> The service request bits. </value>
    Private Property ServiceRequestBits As Integer
        Get
            Return Me._ServiceRequestBits
        End Get
        Set(value As Integer)
            If value <> Me.ServiceRequestBits Then
                Me._ServiceRequestBits = value
                Me._StatusRegisterLabel.Text = $"0x{value And &HFF:X2}"
                Dim MAV As Integer = CInt(Me._MessageAvailableBitsNumeric.Value)
                Me.MessageAvailable = (value And MAV) <> 0
            End If
        End Set
    End Property

    Private _MessageAvailable As Boolean

    ''' <summary> Gets or sets the message available sentinel. </summary>
    ''' <value> The message available. </value>
    Private Property MessageAvailable As Boolean
        Get
            Return Me._MessageAvailable
        End Get
        Set(value As Boolean)
            If value <> Me.MessageAvailable OrElse value <> (Me._ReadManualRadioButton.Checked AndAlso value) Then
                Me._MessageAvailable = value
                Me._ReceiveButton.Enabled = Me._ReadManualRadioButton.Checked AndAlso value
            End If
        End Set
    End Property

#End Region

#Region " TIMER HANDLERS "

    ''' <summary> Stops poll timer. </summary>
    Private Sub stopPollTimer()

        If Me._pollTimer IsNot Nothing Then
            Me._pollTimer.Enabled = False
            RemoveHandler Me._pollTimer.Tick, AddressOf Me.onPollTimerTick
            Me._pollTimer.Dispose()
            Me._pollTimer = Nothing
        End If

        ' wait for timer to terminate all is actions
        Dim timer As System.Diagnostics.Stopwatch = Diagnostics.Stopwatch.StartNew
        Do Until timer.ElapsedMilliseconds > 200
            Application.DoEvents()
            Threading.Thread.Sleep(10)
        Loop

        If Me._PollRadioButton.Checked Then
            Me._ReadManualRadioButton.Checked = True
        End If

    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS "

    ''' <summary> Event handler. Called by _ExitButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub _ExitButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        Me.Close()
    End Sub

    ''' <summary> Reads status register. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub readStatusRegister()

        Dim lastAction As String = "N/A"
        Try

            lastAction = "Reading SRQ"
            Me._StatusRegisterLabel.Text = ""
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
            Me.ServiceRequestBits = Me.Session.ReadServiceRequestStatus
        Catch ex As Exception
            ex.Data.Add("@isr", lastAction)
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. failed: {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Event handler. Called by readStatusRegisterButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub readStatusRegisterButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _ReadStatusRegisterButton.Click
        Me.readStatusRegister()
    End Sub

    ''' <summary> Builds a message for the message log appending a line. </summary>
    ''' <param name="message"> Specifies the message to append. </param>
    ''' <returns> The time stamped message. </returns>
    Friend Shared Function BuildTimeStampLine(ByVal message As String) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, "{0:HH:mm:ss.fff} {1}{2}", DateTime.Now, message, Environment.NewLine)
    End Function

    ''' <summary> Receives this object. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub receive()

        Dim lastAction As String = "N/A"
        Try
            lastAction = "Receiving data"
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")

            receiveBuffer = Me.Session.ReadLine()

            If Not String.IsNullOrWhiteSpace(receiveBuffer) Then
                receiveBuffer = receiveBuffer.InsertCommonEscapeSequences
            End If

            If Not String.IsNullOrWhiteSpace(receiveBuffer) Then
                With Me._OutputTextBox
                    .SelectionStart = .Text.Length
                    .SelectionLength = 0
                    .SelectedText = InstrumentInterfaceForm.BuildTimeStampLine(receiveBuffer)
                    .SelectionStart = .Text.Length
                End With
                Me._ReceiveButton.Enabled = False
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Received '{0}'.", receiveBuffer)
            End If

            ' update the status register information
            lastAction = "Reading status register"
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. ")
            Me.readStatusRegister()

        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. failed: {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Event handler. Called by receiveButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub receiveButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _ReceiveButton.Click
        Me.receive()
    End Sub

    ''' <summary> Send this message. </summary>
    ''' <param name="value"> The value. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub send(ByVal value As String)

        Dim lastAction As String = "N/A"
        Try

            If Not String.IsNullOrWhiteSpace(value) Then
                transmitBuffer = value.ReplaceCommonEscapeSequences
                If Not String.IsNullOrWhiteSpace(transmitBuffer) Then
                    lastAction = $"Sending '{transmitBuffer}'"
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{lastAction };. ")
                    Me.Session.WriteLine(transmitBuffer)
                End If
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. failed: {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Event handler. Called by sendButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    Private Sub sendButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _SendButton.Click
        Me.send(_InputTextBox.Text.Trim)
    End Sub

    ''' <summary> Event handler. Called by sendComboCommandButton for click events. </summary>
    ''' <param name="eventSender"> The event sender. </param>
    ''' <param name="eventArgs">   Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub sendComboCommandButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _SendComboCommandButton.Click

        Dim lastAction As String = "N/A"
        Try

            transmitBuffer = Me._CommandsComboBox.Text.Trim
            If Not String.IsNullOrWhiteSpace(transmitBuffer) Then
                lastAction = $"Sending '{transmitBuffer}'"
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{lastAction};. ")
                Me.Session.WriteLine(transmitBuffer)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{lastAction};. failed: {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Event handler. Called by _readManualRadioButton for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _readManualRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ReadManualRadioButton.CheckedChanged
        If Me._InitializingComponents Then Return
        If Not Me._ReadManualRadioButton.Checked Then
            Exit Sub
        End If
        Try
            Me._ReceiveButton.Enabled = False
            Me._ReadStatusRegisterButton.Enabled = True
            If Me.Session IsNot Nothing Then
                RemoveHandler Me.Session.ServiceRequested, AddressOf Me.OnSessionServiceRequested
            End If
            stopPollTimer()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"exception occurred;. failed: {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Event handler. Called by _pollRadioButton for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _pollRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _PollRadioButton.CheckedChanged
        If Me._InitializingComponents Then Return
        If Not Me._PollRadioButton.Checked Then
            Exit Sub
        End If
        Try
            Me._ReceiveButton.Enabled = False
            Me._ReadStatusRegisterButton.Enabled = False
            If Me.Session IsNot Nothing Then
                Me.DisableServiceRequestEventHandler()
            End If
            Me._pollTimer = New Timer
            Me._pollTimer.Enabled = False
            Me._pollTimer.Interval = CInt(Me._PollIntervalNumericUpDown.Value)
            AddHandler Me._pollTimer.Tick, AddressOf Me.onPollTimerTick
            Me._pollTimer.Enabled = True
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"exception occurred;. {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Raises the system. event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    Private Sub onPollTimerTick(ByVal sender As Object, ByVal e As System.EventArgs)
        Static pollTimerLocker As New Object
        SyncLock pollTimerLocker
            Me.readStatusRegister()
            If Me.MessageAvailable Then
                Me.receive()
            End If
        End SyncLock

    End Sub

    ''' <summary> Raises the message based session event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OnSessionServiceRequested(ByVal sender As SessionBase)
        If sender IsNot Nothing Then
            Try
                Me.readStatusRegister()
                If Me.MessageAvailable Then
                    Me.receive()
                End If
            Catch ex As Exception
                ex.Data.Add("@isr", "failed service request")
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"exception occurred;. {ex.ToFullBlownString}")
            End Try
        Else
            Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Sender is not a message-based session")
        End If
    End Sub

    ''' <summary> Raises the message based session event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    Private Sub OnSessionServiceRequested(ByVal sender As Object, ByVal e As EventArgs)
        Me.OnSessionServiceRequested(TryCast(sender, SessionBase))
    End Sub

    Private _ServiceRequestEventHandlerEnabled As Boolean

    ''' <summary> Enable service request event handler. </summary>
    Public Sub EnableServiceRequestEventHandler()
        If Me.IsOpen AndAlso Not Me._ServiceRequestEventHandlerEnabled Then
            Me._ServiceRequestEventHandlerEnabled = True
            AddHandler Me.Session.ServiceRequested, AddressOf Me.OnSessionServiceRequested
            Me.Session.EnableServiceRequest()
        End If
    End Sub

    ''' <summary> Disable service request event handler. </summary>
    Public Sub DisableServiceRequestEventHandler()
        If Me.IsOpen AndAlso Me._ServiceRequestEventHandlerEnabled Then
            Me._ServiceRequestEventHandlerEnabled = False
            Me.Session.DisableServiceRequest()
            RemoveHandler Me.Session.ServiceRequested, AddressOf Me.OnSessionServiceRequested
        End If
    End Sub

    ''' <summary> Event handler. Called by _serviceRequestReceiveOptionRadioButton for checked changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _serviceRequestReceiveOptionRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ServiceRequestReceiveOptionRadioButton.CheckedChanged
        If Me._InitializingComponents Then Return
        If Not Me._ServiceRequestReceiveOptionRadioButton.Checked Then
            Exit Sub
        End If
        Try
            If Me._pollTimer IsNot Nothing Then
                Me._pollTimer.Enabled = False
                RemoveHandler Me._pollTimer.Tick, AddressOf Me.onPollTimerTick
                Me._pollTimer.Dispose()
                Me._pollTimer = Nothing
            End If
            If Me.Session IsNot Nothing Then
                Me.EnableServiceRequestEventHandler()
                If Not String.IsNullOrWhiteSpace(Me._SreCommandComboBox.Text) Then
                    Me.send(Me._SreCommandComboBox.Text & "\n")
                End If
                Me._ReceiveButton.Enabled = False
                Me._ReadStatusRegisterButton.Enabled = False
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"exception occurred;. {ex.ToFullBlownString}")
        End Try

    End Sub

    ''' <summary> Event handler. Called by _pollIntervalNumericUpDown for value changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _pollIntervalNumericUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _PollIntervalNumericUpDown.ValueChanged

        If Me._pollTimer IsNot Nothing Then
            Me._pollTimer.Interval = CInt(Me._PollIntervalNumericUpDown.Value)
        End If

    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds the listeners such as the current trace messages box. </summary>
    Protected Overloads Sub AddListeners()
        MyBase.AddListener(Me._TraceMessagesBox)
        Me._InterfacePanel.AddListeners(Me.Talker.Listeners)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    Public Overrides Sub AddListener(ByVal item As ITraceMessageListener)
        MyBase.AddListener(item)
        Me._InterfacePanel.AddListener(item)
        If TypeOf (item) Is MyLog Then My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Clears the listeners. </summary>
    Public Overrides Sub ClearListeners()
        MyBase.ClearListeners()
        Me._InterfacePanel.ClearListeners()
    End Sub

    ''' <summary> Updates the trace log level described by traceLevel. </summary>
    ''' <remarks> David, 12/14/2016. </remarks>
    ''' <param name="traceLevel"> The trace level. </param>
    Public Overrides Sub UpdateTraceLogLevel(ByVal traceLevel As TraceEventType)
        MyBase.UpdateTraceLogLevel(traceLevel)
        Me._InterfacePanel.UpdateTraceLogLevel(traceLevel)
    End Sub

    ''' <summary> Updates the trace show level described by traceLevel. </summary>
    ''' <remarks> David, 12/14/2016. </remarks>
    ''' <param name="traceLevel"> The trace level. </param>
    Public Overrides Sub UpdateTraceShowLevel(ByVal traceLevel As TraceEventType)
        MyBase.UpdateTraceShowLevel(traceLevel)
        Me._InterfacePanel.UpdateTraceShowLevel(traceLevel)
    End Sub

    ''' <summary> Handles the <see cref="_TraceMessagesBox"/> property changed event. </summary>
    ''' <remarks> David, 9/5/2016. </remarks>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(sender As TraceMessagesBox, propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
            If String.Equals(propertyName, NameOf(sender.StatusPrompt)) Then
                Me._StatusLabel.Text = sender.StatusPrompt
                Me._StatusLabel.ToolTipText = sender.StatusPrompt
            End If
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
                               $"Failed reporting Trace Message Property Change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

End Class
