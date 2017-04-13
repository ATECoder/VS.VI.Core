Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Pith
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Panel for simple read write messages. </summary>
''' <remarks> David, 12/24/2015. </remarks>
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

    Private _InitializingComponents As Boolean
    ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
    Public Sub New()
        MyBase.New()

        Me._InitializingComponents = True
        ' This call is required by the designer.
        InitializeComponent()
        Me._InitializingComponents = False

        ' Add any initialization after the InitializeComponent() call.
        Me._ServiceRequestStatusLabel.Text = "0x.."
        Me._ServiceRequestStatusLabel.ToolTipText = "Status byte. Double click to update"
        Me._TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
        Me.AddListeners()
    End Sub

    ''' <summary>
    ''' Disposes of the resources (other than memory) used by the
    ''' <see cref="T:System.Windows.Forms.Form" />.
    ''' </summary>
    ''' <remarks> David, 12/19/2015. </remarks>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me._session IsNot Nothing Then
                    Me._session.Dispose()
                    Try
                        ' Trying to null the session raises an ObjectDisposedException 
                        ' if session service request handler was not released. 
                        Me._session = Nothing
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
                    End Try
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

    Private _session As VI.SessionBase

    ''' <summary> Selects a resource and open a message based session with this resource. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _OpenSessionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _OpenSessionButton.Click
        Dim resource As String = ""
        Try
            Me._SessionInfoTextBox.Text = "Opening session..."
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Opening;. ")
            Using selector As New Instrument.ResourceSelectorDialog()
                If selector.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    resource = selector.ResourceName
                    Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Opening;. session to {0}", resource)
                    Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    Me._session = isr.VI.SessionFactory.Get.Factory.CreateSession()
                    If Me._session IsNot Nothing Then
                        Me._session.OpenSession(selector.ResourceName, Threading.SynchronizationContext.Current)
                        If Me.IsSessionOpen Then
                            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Session open;. to {0}", Me._session.ResourceName)
                            Me._SessionInfoTextBox.Text = Me._session.ResourceName
                        Else
                            Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed opening;. session to {0}", resource)
                            Me._SessionInfoTextBox.Text = "Session not open. Check resource."
                        End If
                        Me._TimeoutSelector.Value = CDec(Me._session.Timeout.TotalMilliseconds)
                        Me._SimpleReadWriteControl.Connect(Me._session)
                        Me._SimpleReadWriteControl.ReadEnabled = Me._AutoReadCheckBox.Checked
                    Else
                        Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed creating;. session to {0}", resource)
                    End If
                    Me.UpdateControlsState()
                End If
            End Using
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception opening;. session to {resource}. {ex.ToFullBlownString}")
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
            resource = Me._session.ResourceName
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Closing;. session to {0}", resource)
        End If
        If Me._session IsNot Nothing Then
            Me._session.Dispose()
            Try
                ' Trying to null the session raises an ObjectDisposedException 
                ' if session service request handler was not released. 
                Me._session = Nothing
                Me._SimpleReadWriteControl.Disconnect()
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            Finally
            End Try
        End If
        If Not Me.IsSessionOpen Then
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Closed;. session to {0}", resource)
        End If
        Me.UpdateControlsState()
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
                    Me._StatusLabel.Text = sender.StatusMessage
                Case NameOf(sender.ServiceRequestValue)
                    Me._ServiceRequestStatusLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
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
                               $"Exception handling SimpleReadWrite.{e?.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

    Private Function IsSessionOpen() As Boolean
        Return Me._session IsNot Nothing AndAlso Me._session.IsSessionOpen
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
        If Not Me._InitializingComponents Then
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
    Private Function displayStatusByte() As Integer
        Dim statusByte As Integer = -1
        If Me._session Is Nothing Then
            Me._ServiceRequestStatusLabel.Text = "0x.."
            Me._ServiceRequestStatusLabel.ToolTipText = "Status byte. Double click to update"
        Else
            statusByte = Me._session.ReadServiceRequestStatus
            Me._ServiceRequestStatusLabel.Text = $"0x{statusByte:X2}"
            Me._ServiceRequestStatusLabel.ToolTipText = VI.StatusSubsystemBase.BuildReport(CType(statusByte, VI.ServiceRequests), ";")
        End If
        Return statusByte
    End Function

    ''' <summary> Event handler. Called by _PolledStatusToolStripLabel for double click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _PolledStatusToolStripLabel_DoubleClick(sender As System.Object, ByVal e As System.EventArgs) Handles _ServiceRequestStatusLabel.DoubleClick
        If Not Me._PollEnableCheckBox.Checked Then
            Me.displayStatusByte()
        End If
    End Sub

    ''' <summary> Event handler. Called by _PollTimer for tick events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _PollTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _PollTimer.Tick
        Try
            _PollTimer.Enabled = False
            If Me._session Is Nothing Then
                Me._ServiceRequestStatusLabel.Text = "0x.."
            Else
                Dim statusbyte As Integer = displayStatusByte()
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
        If Me._session IsNot Nothing Then
            Me._session.StoreTimeout(TimeSpan.FromMilliseconds(Me._TimeoutSelector.Value))
            Me._TimeoutSelector.Value = CDec(Me._session.Timeout.TotalMilliseconds)
        End If
    End Sub

    Private Sub _TimeoutSelector_ValueSelected(sender As Object, e As EventArgs) Handles _TimeoutSelector.Validated
        Me.ApplySelectedTimeout()
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds the listeners such as the current trace messages box. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    Protected Overloads Sub AddListeners()
        Me.Talker.Listeners.Add(Me._TraceMessagesBox)
        Me._SimpleReadWriteControl.Talker.Listeners.Add(Me._TraceMessagesBox)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of ITraceMessageListener))
        MyBase.AddListeners(listeners)
        Me._SimpleReadWriteControl.AddListeners(listeners)
    End Sub

    ''' <summary> Handles the <see cref="_TraceMessagesBox"/> property changed event. </summary>
    ''' <remarks> David, 9/5/2016. </remarks>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub OnPropertyChanged(sender As TraceMessagesBox, propertyName As String)
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
