Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Pith
Imports isr.Core.Pith.EscapeSequencesExtensions
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Panel for simple service requests. </summary>
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
Public Class ServiceRequesterPanel
    Inherits TalkerControlBase

#Region " CONSTRUCTORS AND DESTRUCTORS "

    Public Sub New()
        MyBase.New()
        Me.InitializeComponent()
        Me._ToolTip.SetToolTip(Me._EnableServiceRequestButton,
                              "Enable the instrument's SRQ event on MAV by sending the following command (varies by instrument)")
        Me._ToolTip.SetToolTip(Me._OpenSessionButton,
                               "The resource name of the device is set and the control attempts to connect to the device")
        Me._ToolTip.SetToolTip(Me._WriteButton,
                               "Send string to device")
        Me._EnableServiceRequestButton.Enabled = False
        Me._WriteButton.Enabled = False
        Me._OpenSessionButton.Enabled = True
        Me._TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
        Me.AddListeners()
        Me.StopWatch = New Stopwatch
    End Sub

    ''' <summary>
    ''' Disposes of the resources (other than memory) used by the
    ''' <see cref="T:System.Windows.Forms.Form" />.
    ''' </summary>
    ''' <remarks> David, 12/19/2015. </remarks>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me._Session IsNot Nothing Then
                    Me._Session.Dispose()
                    Try
                        ' Trying to null the session raises an ObjectDisposedException 
                        ' if session service request handler was not released. 
                        Me._Session = Nothing
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

    Private _Session As SessionBase

    ''' <summary> Queries if a session is open. </summary>
    ''' <remarks> David, 11/27/2015. </remarks>
    ''' <returns> <c>true</c> if a session is open; otherwise <c>false</c> </returns>
    Private Function IsSessionOpen() As Boolean
        Return Me._Session IsNot Nothing AndAlso Me._Session.IsSessionOpen
    End Function

    ' When the Configure button is pressed, the resource name of the
    ' device is set and the control attempts to connect to the device
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    Private Sub _OpenSessionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _OpenSessionButton.Click
        Dim resource As String = ""
        Try
            If Me.IsSessionOpen Then
                resource = Me._Session.ResourceName
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Closing;. session to {0}", resource)
                Me._Session.Dispose()
                Try
                    ' Trying to null the session raises an ObjectDisposedException 
                    ' if session service request handler was not released. 
                    Me._Session = Nothing
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
                Finally
                    If Me.IsSessionOpen Then
                        Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed closing;. session to {0}", resource)
                    Else
                        Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Closed;. session to {0}", resource)
                    End If
                End Try
            Else
                If Me._Session IsNot Nothing Then Me._Session.Dispose()
                resource = Me._ResourceNamesComboBox.Text
                Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Opening session;. to {0}", resource)
                '  gpibSession = CType(ResourceManager.GetLocalManager().Open(resourceNameTextBox.Text), gpibSession)
                Me._Session = isr.VI.SessionFactory.Get.Factory.CreateSession()
                Me._Session.OpenSession(Me._ResourceNamesComboBox.Text, TimeSpan.FromMilliseconds(Me._TimeoutSelector.Value), Threading.SynchronizationContext.Current)
            End If
        Catch ex As InvalidCastException
            MessageBox.Show("Resource selected must be a GPIB session")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Exception Occurred", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
        Finally
            Me.OnOpenChanged()
        End Try
    End Sub

    Private Sub ApplySelectedTimeout()
        If Me._Session IsNot Nothing Then
            Me._Session.StoreTimeout(TimeSpan.FromMilliseconds(Me._TimeoutSelector.Value))
            Me._TimeoutSelector.Value = CDec(Me._Session.Timeout.TotalMilliseconds)
        End If
    End Sub

    Private Sub _TimeoutSelector_ValueSelected(sender As Object, e As EventArgs) Handles _TimeoutSelector.Validated
        Me.ApplySelectedTimeout()
    End Sub

    Private Sub OnOpenChanged()
        Me._OpenSessionButton.Text = CStr(IIf(Me.IsSessionOpen, "CLOSE", "OPEN"))
        If Me.IsSessionOpen Then
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Opened;. session to {0}", Me._Session.ResourceName)
            Me._SyncCallBacksCheckBox.Checked = Me._Session.SynchronizeCallbacks
            Me._TimeoutSelector.Value = CDec(Me._Session.Timeout.TotalMilliseconds)
            Me.ApplySelectedTimeout()
            Me._Session.StoreTimeout(Me._Session.Timeout)
            With Me._EnableServiceRequestButton
                .Text = CType(IIf(Me._Session.ServiceRequestEventEnabled, "DISABLE SRQ", "ENABLE SRQ"), String)
                .Enabled = True
            End With
            Me._WriteButton.Enabled = Me._Session.ServiceRequestEventEnabled
        Else
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Closed;. session to {0}", Me._ResourceNamesComboBox.Text)
            Me._EnableServiceRequestButton.Enabled = False
            Me._WriteButton.Enabled = False
        End If
    End Sub

    ' The Enable SRQ button writes the string that tells the instrument to
    ' enable the SRQ bit
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    Private Sub _EnableServiceRequestButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _EnableServiceRequestButton.Click
        Try
            If Me.IsSessionOpen Then
                If Me._Session.ServiceRequestEventEnabled Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Disabling service request handling")
                    Me._Session.DisableServiceRequest()
                    RemoveHandler _Session.ServiceRequested, AddressOf Me.OnServiceRequested
                Else
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Enabling service request handling")
                    ' you have to register the handler before you enable event  
                    AddHandler _Session.ServiceRequested, AddressOf Me.OnServiceRequested
                    Me._Session.EnableServiceRequest()
                    If Not String.IsNullOrWhiteSpace(Me._CommandTextBox.Text.Trim) Then
                        Me.WriteToSession(Me._CommandTextBox.Text)
                    End If
                End If
            End If
        Catch exp As NativeException
            MessageBox.Show(exp.ToString, "Exception Occurred", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
        Catch exp As Exception
            MessageBox.Show(exp.Message, "Exception Occurred", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
        Finally
            Me.OnOpenChanged()
        End Try
    End Sub

    ''' <summary> Writes a button click. </summary>
    ''' <remarks> David, 11/27/2015. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    Private Sub _WriteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _WriteButton.Click
        Me.WriteToSession(_WriteTextBox.Text)
    End Sub

    Private Sub clearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ClearButton.Click
        Me._ReadTextBox.Clear()
        Me._ServiceRequestStatusLabel.Text = ""
    End Sub

    Private Sub _SyncCallBacksCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles _SyncCallBacksCheckBox.CheckedChanged
        If Me.IsSessionOpen AndAlso (Me._SyncCallBacksCheckBox.Checked <> Me._Session.SynchronizeCallbacks) Then
            Me._Session.SynchronizeCallbacks = Me._SyncCallBacksCheckBox.Checked
        End If
    End Sub

    Private Shared Function ReplaceCommonEscapeSequences(ByVal s As String) As String
        If (s <> Nothing) Then
            Return s.Replace("\n", Convert.ToChar(10)).Replace("\r", Convert.ToChar(13))
        Else
            Return Nothing
        End If
    End Function

    Private Shared Function InsertCommonEscapeSequences(ByVal s As String) As String
        If (s <> Nothing) Then
            Return s.Replace(Convert.ToChar(10), "\n").Replace(Convert.ToChar(13), "\r")
        Else
            Return Nothing
        End If
    End Function

    Private StopWatch As Diagnostics.Stopwatch

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")>
    Private Sub WriteToSession(ByVal value As String)
        Try
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Writing...;. {0}", value)
            value = ReplaceCommonEscapeSequences(value)
            StopWatch.Restart()
            Me._Session.WriteLine(value)
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Done writing;. ")
        Catch exp As Exception
            MessageBox.Show(exp.Message, "Exception Occurred", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
        End Try
    End Sub

    ''' <summary> Raises the service requested event. </summary>
    ''' <remarks> David, 11/27/2015. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub HandleMessageService(ByVal sender As SessionBase, value As ServiceRequests)
        If sender Is Nothing Then Throw New ArgumentNullException(NameOf(sender))
        If (value And CInt(Me._MessageStatusBitValueNumeric.Value)) <> 0 Then
            Dim textRead As String = sender.ReadFiniteLine()
            Me._ElapsedTimeTextBox.Text = Me.StopWatch.Elapsed.TotalMilliseconds.ToString("0.0", Globalization.CultureInfo.CurrentCulture)
            Me._ReadTextBox.Text = InsertCommonEscapeSequences(textRead)
        Else
            ' the 3706A gets two consecutive service requests on each write.
            Me._ReadTextBox.Text = "MAV in status register is not set, which means that message is not available. Is the command to enable SRQ is correct? Is the instrument is 488.2 compatible?"
        End If
    End Sub

    ''' <summary> Raises the service requested event. </summary>
    ''' <remarks> David, 11/27/2015. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub OnServiceRequested(ByVal sender As Object, ByVal e As EventArgs)
        Dim requester As SessionBase = TryCast(sender, SessionBase)
        If requester Is Nothing Then Return
        Try
            Dim sb As ServiceRequests = requester.ReadStatusByte()
            Me._ServiceRequestStatusLabel.Text = $"0x{CInt(sb):X2}"
            Me._ServiceRequestStatusLabel.BackColor = System.Drawing.Color.Aqua
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Servicing events: {0}", Me._ServiceRequestStatusLabel.Text)
            Me.HandleMessageService(requester, sb)
            sb = requester.ReadStatusByte()
            Me._ServiceRequestStatusLabel.Text = $"0x{CInt(sb):X2}"
            Me._ServiceRequestStatusLabel.BackColor = System.Drawing.Color.LightGreen
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception handling session service request;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " RESOURCES "

    Sub ListResources()
        Using rm As ResourcesManagerBase = isr.VI.SessionFactory.Get.Factory.CreateResourcesManager
            _ResourceNamesComboBox.DataSource = Nothing
            _ResourceNamesComboBox.Items.Clear()
            _ResourceNamesComboBox.DataSource = rm.FindInstruments()
        End Using
    End Sub

    Private Sub _FindButton_Click(sender As Object, e As EventArgs) Handles _FindButton.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Listing resources...;. ")
            Me.ListResources()
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds the listeners such as the current trace messages box. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    Protected Overloads Sub AddListeners()
        Me.Talker.Listeners.Add(Me._TraceMessagesBox)
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

