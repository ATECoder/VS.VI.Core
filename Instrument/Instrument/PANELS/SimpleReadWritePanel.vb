Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
''' <summary> Panel for simple read write messages. </summary>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="12/24/2015" by="David" revision=""> Created. </history>
<System.ComponentModel.Description("Simple Read and Write Panel")>
<System.Drawing.ToolboxBitmap(GetType(InterfacePanel), "SimpleReadWritePanel"), ToolboxItem(True)>
Public Class SimpleReadWritePanel
    Inherits TalkerControlBase

#Region " CONSTRUCTORS AND DESTRUCTORS "

    ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
    Public Sub New()
        MyBase.New()

        Me.InitializingComponents = True
        ' This call is required by the designer.
        InitializeComponent()
        Me.InitializingComponents = False

        ' Add any initialization after the InitializeComponent() call.
        Me._ServiceRequestStatusLabel.Text = "0x.."
        Me._ServiceRequestStatusLabel.ToolTipText = "Status byte. Double click to update"
        Me._TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
        Me.AddPrivateListeners()
    End Sub

    ''' <summary>
    ''' Disposes of the resources (other than memory) used by the
    ''' <see cref="T:System.Windows.Forms.Form" />.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Me.RemovePrivateListener(Me._TraceMessagesBox)
                Me._SimpleReadWriteControl.RemovePrivateListeners()
                If Me.Session IsNot Nothing Then
                    Me.Session = Nothing
                End If
                ' unable to use null conditional because it is not seen by code analysis
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " SESSION "

    Private _Session As VI.Pith.SessionBase

    ''' <summary> Gets or sets the session. </summary>
    ''' <value> The session. </value>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Property Session As Vi.Pith.SessionBase
        Get
            Return Me._Session
        End Get
        Set(value As Vi.Pith.SessionBase)
            If Me._Session IsNot Nothing Then
                RemoveHandler Me._Session.PropertyChanged, AddressOf Me._Session_PropertyChanged
                Me._Session.Dispose()
            End If
            Try
                ' Trying to null the session raises an ObjectDisposedException 
                ' if session service request handler was not released. 
                Me._Session = value
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
            If Me._Session IsNot Nothing Then
                AddHandler Me._Session.PropertyChanged, AddressOf Me._Session_PropertyChanged
            End If
        End Set
    End Property

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal sender As VI.Pith.SessionBase, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(VI.Pith.SessionBase.TerminationCharacter)
                    Me.UpdateTermination(sender)
                Case NameOf(VI.Pith.SessionBase.TerminationCharacterEnabled)
                    Me.UpdateTermination(sender)
                Case NameOf(VI.Pith.SessionBase.IsSessionOpen)
                    If sender.IsSessionOpen Then
                        Me.UpdateTermination(sender)
                    End If
            End Select
        End If
    End Sub

    ''' <summary> Event handler. Called by <see crefname="_SimpleReadWriteControl"/> for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Session_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.Pith.SessionBase)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._Session_PropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, VI.Pith.SessionBase), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Gets or sets the supports keep alive. </summary>
    ''' <value> The supports keep alive. </value>
    Public Property SupportsAliveCommands As Boolean

    ''' <summary> Selects a resource and open a message based session with this resource. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _OpenSessionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _OpenSessionButton.Click
        Dim resource As String = ""
        Try
            Me._SessionInfoTextBox.Text = "Opening session..."
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Opening;. ")
            Using selector As New Instrument.ResourceSelectorDialog()
                If selector.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    resource = selector.ResourceName
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Opening;. session to {0}", resource)
                    Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    Me.Session = isr.VI.SessionFactory.Get.Factory.CreateSession()
                    If Me.Session IsNot Nothing Then
                        If Not Me.SupportsAliveCommands Then
                            Me.Session.IsAliveCommand = ""
                            Me.Session.IsAliveQueryCommand = ""
                        End If
                        Me.Session.OpenSession(selector.ResourceName, Threading.SynchronizationContext.Current)
                        If Me.IsSessionOpen Then
                            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Session open;. to {0}", Me.Session.ResourceName)
                            Me._SessionInfoTextBox.Text = Me.Session.ResourceName
                        Else
                            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed opening;. session to {0}", resource)
                            Me._SessionInfoTextBox.Text = "Session not open. Check resource."
                        End If
                        Me._TimeoutSelector.Value = CDec(Me.Session.Timeout.TotalMilliseconds)
                        Me._SimpleReadWriteControl.Connect(Me.Session)
                        Me._SimpleReadWriteControl.ReadEnabled = Me._AutoReadCheckBox.Checked
                    Else
                        Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed creating;. session to {0}", resource)
                    End If
                    Me.UpdateControlsState()
                End If
            End Using
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception opening;. session to {resource}. {ex.ToFullBlownString}")
            Me._SessionInfoTextBox.Text = ex.ToFullBlownString
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try
        Return
    End Sub

    ''' <summary> Closed an open session. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _CloseSessionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _CloseSessionButton.Click
        Dim resource As String = ""
        If Me.IsSessionOpen Then
            resource = Me.Session.ResourceName
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Closing;. session to {0}", resource)
        End If
        If Me.Session IsNot Nothing Then
            Me._Session.Dispose()
            Me.Session = Nothing
        End If
        If Not Me.IsSessionOpen Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Closed;. session to {0}", resource)
        End If
        Me.UpdateControlsState()
    End Sub

#End Region

#Region " READ AND WRITE "

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal sender As Instrument.SimpleReadWriteControl, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(Instrument.SimpleReadWriteControl.StatusMessage)
                    Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusMessage, Me._StatusLabel)
                    Me._StatusLabel.ToolTipText = sender.StatusMessage
                Case NameOf(Instrument.SimpleReadWriteControl.ServiceRequestValue)
                    Me._ServiceRequestStatusLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
            End Select
        End If
    End Sub

    ''' <summary> Event handler. Called by <see crefname="_SimpleReadWriteControl"/> for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleReadWriteControl_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Handles _SimpleReadWriteControl.PropertyChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Instrument.SimpleReadWriteControl)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._SimpleReadWriteControl_PropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, Instrument.SimpleReadWriteControl), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    Private Function IsSessionOpen() As Boolean
        Return Me.Session IsNot Nothing AndAlso Me.Session.IsSessionOpen
    End Function

    ''' <summary> Updates the controls state. </summary>
    Private Sub UpdateControlsState()
        Dim sessionopen As Boolean = Me.IsSessionOpen
        Me._OpenSessionButton.Enabled = Not sessionopen
        Me._CloseSessionButton.Enabled = sessionopen
        Me._ServiceRequestStatusLabel.ToolTipText = "Status byte. Double click to update"
    End Sub

#End Region

#Region " POLL "

    ''' <summary> Event handler. Called by _AutoReadCheckBox for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _AutoReadCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _AutoReadCheckBox.CheckedChanged
        If Not Me.InitializingComponents Then
            Me._SimpleReadWriteControl.ReadEnabled = Me._AutoReadCheckBox.Checked
        End If
    End Sub

    ''' <summary> Event handler. Called by _PollMillisecondsNumeric for value changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _PollMillisecondsNumeric_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _PollMillisecondsNumeric.ValueChanged
        Me._PollEnableCheckBox.Checked = False
    End Sub

    ''' <summary> Event handler. Called by _PollEnableCheckBox for checked changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _PollEnableCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _PollEnableCheckBox.CheckedChanged
        Me._ServiceRequestStatusLabel.Visible = Me._PollEnableCheckBox.Checked
        Me._PollTimer.Enabled = False
        If Me._PollEnableCheckBox.Checked Then
            Me._PollTimer.Interval = CInt(Me._PollMillisecondsNumeric.Value)
        End If
        Me._PollTimer.Enabled = Me._PollEnableCheckBox.Checked
    End Sub

    ''' <summary> Reads and displays the status byte. </summary>
    Private Function DisplayStatusByte() As Integer
        Dim statusByte As Integer = -1
        If Me.Session Is Nothing Then
            Me._ServiceRequestStatusLabel.Text = "0x.."
            Me._ServiceRequestStatusLabel.ToolTipText = "Status byte. Double click to update"
        Else
            statusByte = Me.Session.ReadServiceRequestStatus
            Me._ServiceRequestStatusLabel.Text = $"0x{statusByte:X2}"
            Me._ServiceRequestStatusLabel.ToolTipText = StatusSubsystemBase.BuildReport(CType(statusByte, VI.Pith.ServiceRequests), ";")
        End If
        Return statusByte
    End Function

    ''' <summary> Event handler. Called by _PolledStatusToolStripLabel for double click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _PolledStatusToolStripLabel_DoubleClick(sender As System.Object, ByVal e As System.EventArgs) Handles _ServiceRequestStatusLabel.DoubleClick
        If Not Me._PollEnableCheckBox.Checked Then
            Me.DisplayStatusByte()
        End If
    End Sub

    ''' <summary> Event handler. Called by _PollTimer for tick events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _PollTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _PollTimer.Tick
        Try
            _PollTimer.Enabled = False
            If Me.Session Is Nothing Then
                Me._ServiceRequestStatusLabel.Text = "0x.."
            Else
                Dim statusbyte As Integer = DisplayStatusByte()
                If ((statusbyte And CInt(Me._MessageStatusBitValueNumeric.Value)) = Me._MessageStatusBitValueNumeric.Value) AndAlso
                    Me._AutoReadCheckBox.Checked Then
                    Threading.Thread.Sleep(10)
                    Me._SimpleReadWriteControl.Read()
                End If
            End If
        Catch
        Finally
            _PollTimer.Enabled = True
        End Try
    End Sub

    Private Sub ApplySelectedTimeout()
        If Me.Session IsNot Nothing Then
            Me.Session.StoreTimeout(TimeSpan.FromMilliseconds(Me._TimeoutSelector.Value))
            Me._TimeoutSelector.Value = CDec(Me.Session.Timeout.TotalMilliseconds)
        End If
    End Sub

    Private Sub _TimeoutSelector_ValueSelected(sender As Object, e As EventArgs) Handles _TimeoutSelector.Validated
        Me.ApplySelectedTimeout()
    End Sub

#End Region

#Region " ATRRIBUTE SETTINGS "

    ''' <summary> Zero-based index of the no termination. </summary>
    Const NoTerminationIndex As Integer = 0
    ''' <summary> The new line termination index. </summary>
    Const NewLineTerminationIndex As Integer = 1
    ''' <summary> Zero-based index of the return termination. </summary>
    Const ReturnTerminationIndex As Integer = 2

    ''' <summary> Updates the termination described by session. </summary>
    ''' <param name="session"> The session. </param>
    Private Sub UpdateTermination(ByVal session As VI.Pith.SessionBase)
        If session.TerminationCharacterEnabled Then
            If session.TerminationCharacter = isr.Core.Pith.EscapeSequencesExtensions.NewLineValue Then
                Me._ReadTerminationComboBox.SelectedIndex = NewLineTerminationIndex
            Else
                Me._ReadTerminationComboBox.SelectedIndex = ReturnTerminationIndex
            End If
        Else
            Me._ReadTerminationComboBox.SelectedIndex = NoTerminationIndex
        End If
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadTerminationComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _ReadTerminationComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "applying return termination"
        Try
            Select Case Me._ReadTerminationComboBox.SelectedIndex
                Case NoTerminationIndex
                    Me.Session.TerminationCharacterEnabled = False
                Case NewLineTerminationIndex
                    Me.Session.TerminationCharacterEnabled = True
                    Me.Session.TerminationCharacter = isr.Core.Pith.EscapeSequencesExtensions.NewLineValue
                Case ReturnTerminationIndex
                    Me.Session.TerminationCharacterEnabled = True
                    Me.Session.TerminationCharacter = isr.Core.Pith.EscapeSequencesExtensions.ReturnValue
            End Select
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Failed {activity};. {ex.ToFullBlownString}")
        Finally
            Me.ReadServiceRequestStatus()
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Function BuildAutoTermination() As String
        Dim message As New System.Text.StringBuilder()
        If Me._AppendNewLineCheckBox.Checked Then message.Append(isr.Core.Pith.EscapeSequencesExtensions.NewLineEscape)
        If Me._AppendReturnCheckBox.Checked Then message.Append(isr.Core.Pith.EscapeSequencesExtensions.ReturnEscape)
        Return message.ToString
    End Function

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AppendReturnCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles _AppendReturnCheckBox.CheckedChanged
        Dim activity As String = "applying return termination"
        Try
            Me._SimpleReadWriteControl.AutoAppendTermination = Me.BuildAutoTermination
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Failed {activity};. {ex.ToFullBlownString}")
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _AppendNewLineCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles _AppendNewLineCheckBox.CheckedChanged
        Dim activity As String = "applying new line termination"
        Try
            Me._SimpleReadWriteControl.AutoAppendTermination = Me.BuildAutoTermination
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Failed {activity};. {ex.ToFullBlownString}")
        End Try
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ReadServiceRequestStatus()
        Dim activity As String = "reading service request status"
        Try
            Me._SimpleReadWriteControl.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Failed {activity};. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Service request status label click. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ServiceRequestStatusLabel_Click(sender As Object, e As EventArgs) Handles _ServiceRequestStatusLabel.Click
        Me.ReadServiceRequestStatus()
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

    ''' <summary> Adds listeners. </summary>
    Private Overloads Sub AddPrivateListeners()
        Me._SimpleReadWriteControl.AddPrivateListeners(Me.Talker)
        Me.AddPrivateListener(Me._TraceMessagesBox)
    End Sub

    ''' <summary> Assigns talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AssignTalker(talker As ITraceMessageTalker)
        If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
        talker.AddListener(Me._TraceMessagesBox)
        Me._SimpleReadWriteControl.AssignTalker(talker)
        MyBase.AssignTalker(talker)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        If listenerType = Me._TraceMessagesBox.ListenerType Then Me._TraceMessagesBox.ApplyTraceLevel(value)
        Me._SimpleReadWriteControl?.ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        ' MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

    ''' <summary> Handles the <see cref="_TraceMessagesBox"/> property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(sender As TraceMessagesBox, propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        If String.Equals(propertyName, NameOf(isr.Core.Pith.TraceMessagesBox.StatusPrompt)) Then
            Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusPrompt, Me._StatusLabel)
            Me._StatusLabel.ToolTipText = sender.StatusPrompt
        End If
    End Sub

    ''' <summary> Handles Trace messages box property changed event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceMessagesBox_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _TraceMessagesBox.PropertyChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Core.Pith.TraceMessagesBox)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._TraceMessagesBox_PropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, Core.Pith.TraceMessagesBox), e.PropertyName)
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

End Class
