Imports System.Windows.Forms
Imports System.ComponentModel
Imports isr.Core.Pith
''' <summary> The instrument panel form. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="02/07/2005" by="David" revision="2.0.2597.x"> Created. </history>
Public Class InstrumentPanelForm
    Inherits ListenerFormBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializeComponent()
        Me._TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
        Me._TraceMessagesBox.MaxSynopsisLength = 0
    End Sub

    ''' <summary>
    ''' Disposes of the resources (other than memory) used by the
    ''' <see cref="T:System.Windows.Forms.Form" />.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' removes the private text box listener
                Me.RemovePrivateListener(Me._TraceMessagesBox)
                If Me._InstrumentPanel IsNot Nothing Then
                    Me._InstrumentLayout.Controls.Remove(Me._InstrumentPanel)
                    If Me._InstrumentPanelDisposeEnabled Then
                        Me._InstrumentPanel.Dispose()
                    End If
                    Me._InstrumentPanel = Nothing
                End If
                If Me._PropertyNotifyControl IsNot Nothing Then
                    Me._InstrumentLayout.Controls.Remove(Me._PropertyNotifyControl)
                End If
                If Me._PropertyNotifyControlDisposeEnabled Then
                    Me._PropertyNotifyControl.Dispose()
                End If
                If Me._TalkerControl IsNot Nothing Then
                    ' removes the private listeners of the talker control hosted in this form.
                    Me._TalkerControl.RemovePrivateListeners()
                    Me._InstrumentLayout.Controls.Remove(Me._TalkerControl)
                    If Me._TalkerControlDisposeEnabled Then
                        Me._TalkerControl.Dispose()
                    End If
                End If
                Me._TalkerControl = Nothing
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
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
            If e IsNot Nothing Then
                e.Cancel = False
            End If
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
            Me.Text = $"{My.Application.Info.ProductName} release {My.Application.Info.Version}"

            ' default to center screen.
            Me.CenterToScreen()

        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception loading the form;. {ex.ToFullBlownString}")
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
            End If
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Ready to open Visa Session;. ")

        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception showing the form;. {ex.ToFullBlownString}")
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

    ''' <summary> Resize client area. </summary>
    ''' <param name="clientControl"> The client control. </param>
    Public Sub ResizeClientArea(ByVal clientControl As Control)
        If clientControl Is Nothing Then Throw New ArgumentNullException(NameOf(clientControl))
        Dim tabsMargins As New Drawing.Size(Me._Tabs.Width - Me._TraceMessagesBox.Width, Me._Tabs.TabPages(1).Height - Me._TraceMessagesBox.Height)
        Dim controlMargins As New Drawing.Size(Me.ClientSize.Width - Me._Tabs.Width, Me.ClientSize.Height - Me._Tabs.Height)
        Dim newTabWidth As Drawing.Size = New Drawing.Size(Me._Tabs.Width, Me._Tabs.Height)
        Dim resizeRequired As Boolean = False
        If Me._Tabs.Width - tabsMargins.Width < clientControl.Width Then
            newTabWidth = New Drawing.Size(clientControl.Width + tabsMargins.Width, Me._Tabs.Height)
            resizeRequired = True
        End If
        If Me._Tabs.Height - tabsMargins.Height < clientControl.Height Then
            newTabWidth = New Drawing.Size(newTabWidth.Width, clientControl.Height + tabsMargins.Height)
            resizeRequired = True
        End If
        If resizeRequired Then
            Me._Tabs.Size = newTabWidth
            Me._Tabs.Refresh()
            Me.ClientSize = New Drawing.Size(newTabWidth.Width + controlMargins.Width, newTabWidth.Height + controlMargins.Height)
            Me.Refresh()
        End If
    End Sub

#End Region

#Region " INSTRUMENT PANEL "

    ''' <summary> true to enable, false to disable the instrument panel dispose. </summary>
    Private _InstrumentPanelDisposeEnabled As Boolean

    Private WithEvents _InstrumentPanel As Instrument.ResourcePanelBase

    ''' <summary> Adds an instrument panel. </summary>
    ''' <param name="title">          The title. </param>
    ''' <param name="value">          The value. </param>
    ''' <param name="disposeEnabled"> true to enable, false to disable the dispose. </param>
    Public Sub AddInstrumentPanel(ByVal title As String, ByVal value As Instrument.ResourcePanelBase, ByVal disposeEnabled As Boolean)
        Me.ResizeClientArea(value)
        Me._InstrumentPanel = value
        Me._TalkerControl = value
        Me.Talker.ApplyTalkerTraceLevels(Me._TalkerControl.Talker)
        Me.AddPrivateListeners()
        Me._InstrumentPanelDisposeEnabled = disposeEnabled
        With Me._InstrumentPanel
            Me._InstrumentPanel.Dock = Windows.Forms.DockStyle.Fill
            Me._InstrumentPanel.TabIndex = 0
            Me._InstrumentPanel.BackColor = System.Drawing.Color.Transparent
            Me._InstrumentPanel.ClosedResourceTitleFormat = "{0}:Closed"
            Me._InstrumentPanel.OpenResourceTitleFormat = "{0}:{1}"
            Me._InstrumentPanel.ResourceTitle = title
            Me._InstrumentPanel.Font = New System.Drawing.Font(Me.Font, System.Drawing.FontStyle.Regular)
            Me._InstrumentPanel.Name = "_InstrumentPanel"
            Me._InstrumentTabPage.Text = .ResourceTitle
            Me._InstrumentTabPage.ToolTipText = .ResourceTitle
        End With
        Me._InstrumentLayout.Controls.Add(Me._InstrumentPanel, 1, 1)
    End Sub

    ''' <summary> Executes the property change action. </summary>
    ''' <param name="sender">       The sender. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChange(sender As Instrument.ResourcePanelBase, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.ResourceTitle)
                If Not String.IsNullOrWhiteSpace(sender.ResourceTitle) Then
                    Me._InstrumentTabPage.Text = sender.ResourceTitle
                End If
        End Select
    End Sub

    ''' <summary> Instrument panel property changed. </summary>
    ''' <param name="sender"> The sender. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InstrumentPanel_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _InstrumentPanel.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._InstrumentPanel_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChange(TryCast(sender, Instrument.ResourcePanelBase), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling IntstrumentPanel.{e?.PropertyName} change event;. {ex.ToFullBlownString}")


        End Try
    End Sub

#End Region

#Region " PROPERTY NOTIFY CONTROL "

    ''' <summary> true to enable, false to disable the instrument panel dispose. </summary>
    Private _PropertyNotifyControlDisposeEnabled As Boolean

    Private WithEvents _PropertyNotifyControl As PropertyNotifyControlBase

    ''' <summary> Adds an instrument panel. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub AddPropertyNotifyControl(ByVal value As PropertyNotifyControlBase)
        Me.AddPropertyNotifyControl("Instrument", value, True)
    End Sub

    ''' <summary> Adds an instrument panel. </summary>
    ''' <param name="title"> The title. </param>
    ''' <param name="value"> The value. </param>
    Public Sub AddPropertyNotifyControl(ByVal title As String, ByVal value As PropertyNotifyControlBase)
        Me.AddPropertyNotifyControl(title, value, True)
    End Sub

    ''' <summary> Adds an instrument panel. </summary>
    ''' <param name="title">          The title. </param>
    ''' <param name="value">          The value. </param>
    ''' <param name="disposeEnabled"> true to enable, false to disable the dispose. </param>
    Public Sub AddPropertyNotifyControl(ByVal title As String, ByVal value As PropertyNotifyControlBase, ByVal disposeEnabled As Boolean)
        Me.ResizeClientArea(value)
        Me._PropertyNotifyControl = value
        Me.AddPrivateListeners()
        Me._PropertyNotifyControlDisposeEnabled = disposeEnabled
        With Me._PropertyNotifyControl
            .Dock = Windows.Forms.DockStyle.Fill
            .TabIndex = 0
            .BackColor = System.Drawing.Color.Transparent
            .Font = New System.Drawing.Font(Me.Font, System.Drawing.FontStyle.Regular)
            .Name = "_PropertyNotifyControl"
        End With
        Me._InstrumentTabPage.Text = title
        Me._InstrumentTabPage.ToolTipText = title
        Me._InstrumentLayout.Controls.Add(Me._PropertyNotifyControl, 1, 1)
    End Sub

    ''' <summary> Executes the property change action. </summary>
    ''' <param name="sender">       The sender. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnPropertyChange(sender As PropertyNotifyControlBase, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
    End Sub

    ''' <summary> Instrument panel property changed. </summary>
    ''' <param name="sender"> The sender. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _PropertyNotifyControl_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _PropertyNotifyControl.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._PropertyNotifyControl_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChange(TryCast(sender, PropertyNotifyControlBase), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling PropertyNotifyControl.{e?.PropertyName} change event;. {ex.ToFullBlownString}")

        End Try
    End Sub

#End Region

#Region " TALKER CONTROL "

    ''' <summary> true to enable, false to disable the instrument panel dispose. </summary>
    Private _TalkerControlDisposeEnabled As Boolean

    Private WithEvents _TalkerControl As TalkerControlBase

    ''' <summary> Adds an instrument panel. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub AddTalkerControl(ByVal value As TalkerControlBase)
        Me.AddTalkerControl("Instrument", value, True)
    End Sub

    ''' <summary> Adds an instrument panel. </summary>
    ''' <param name="title"> The title. </param>
    ''' <param name="value"> The value. </param>
    Public Sub AddTalkerControl(ByVal title As String, ByVal value As TalkerControlBase)
        Me.AddTalkerControl(title, value, True)
    End Sub

    ''' <summary> Adds an instrument panel. </summary>
    ''' <param name="title">          The title. </param>
    ''' <param name="value">          The value. </param>
    ''' <param name="disposeEnabled"> true to enable, false to disable the dispose. </param>
    Public Sub AddTalkerControl(ByVal title As String, ByVal value As TalkerControlBase, ByVal disposeEnabled As Boolean)
        Me.ResizeClientArea(value)
        Me._TalkerControl = value
        Me.AddPrivateListeners()
        Me._TalkerControlDisposeEnabled = disposeEnabled
        With Me._TalkerControl
            .Dock = Windows.Forms.DockStyle.Fill
            .TabIndex = 0
            .BackColor = System.Drawing.Color.Transparent
            .Font = New System.Drawing.Font(Me.Font, System.Drawing.FontStyle.Regular)
            .Name = "_TalkerControl"
            .ApplyTalkerTraceLevels(Me.Talker)
            .ApplyListenerTraceLevels(Me.Talker)
        End With
        Me._InstrumentTabPage.Text = title
        Me._InstrumentTabPage.ToolTipText = title
        Me._InstrumentLayout.Controls.Add(Me._TalkerControl, 1, 1)
    End Sub

    ''' <summary> Executes the property change action. </summary>
    ''' <param name="sender">       The sender. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnPropertyChange(sender As TalkerControlBase, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
    End Sub

    ''' <summary> Instrument panel property changed. </summary>
    ''' <param name="sender"> The sender. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TalkerControl_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _TalkerControl.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._TalkerControl_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChange(TryCast(sender, TalkerControlBase), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling TalkerControl.{e?.PropertyName} change event;. {ex.ToFullBlownString}")

        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners such as the current trace messages box. </summary>
    Protected Overloads Sub AddPrivateListeners()
        MyBase.AddPrivateListener(Me._TraceMessagesBox)
        Me._TalkerControl?.AddPrivateListeners(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overrides Sub AddListener(ByVal listener As IMessageListener)
        Me._TalkerControl?.AddListener(listener)
        MyBase.AddListener(listener)
    End Sub

    ''' <summary> Removes the listeners if the talker was not assigned. </summary>
    Public Overrides Sub RemoveListeners()
        MyBase.RemoveListeners()
        Me._TalkerControl?.RemoveListeners()
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        If listenerType = Me._TraceMessagesBox.ListenerType Then Me._TraceMessagesBox.ApplyTraceLevel(value)
        Me._TalkerControl?.ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

    ''' <summary> Applies the trace level type to all talkers. </summary>
    ''' <param name="listenerType"> Type of the trace level. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyTalkerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        Me._TalkerControl?.ApplyTalkerTraceLevel(listenerType, value)
        MyBase.ApplyTalkerTraceLevel(listenerType, value)
    End Sub

#End Region

#Region " MESSAGE BOX EVENTS "

    ''' <summary> Handles the <see cref="_TraceMessagesBox"/> property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(sender As TraceMessagesBox, propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        If String.Equals(propertyName, NameOf(sender.StatusPrompt)) Then
            Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusPrompt, Me._StatusLabel)
            Me._StatusLabel.ToolTipText = sender.StatusPrompt
        End If
    End Sub

    ''' <summary> Trace messages box property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
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
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property {0} change", e?.PropertyName)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Failed reporting Trace Message Property Change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

End Class

''' <summary> Collection of instrument panel forms. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/15/2016" by="David" revision=""> Created. </history>
Public Class InstrumentPanelFormCollection
    Inherits isr.Core.Pith.ListenerFormCollection

    ''' <summary> Adds and shows the form. </summary>
    ''' <param name="title">          The title. </param>
    ''' <param name="form">           The form. </param>
    ''' <param name="panel">          The panel. </param>
    ''' <param name="log">            The log. </param>
    ''' <param name="disposeEnabled"> true to enable, false to disable the dispose. </param>
    Public Overloads Sub ShowNew(ByVal title As String,
                                 ByVal form As Instrument.InstrumentPanelForm,
                                 ByVal panel As Instrument.ResourcePanelBase,
                                 ByVal log As MyLog, ByVal disposeEnabled As Boolean)
        If form Is Nothing Then Throw New ArgumentNullException(NameOf(form))
        form.AddInstrumentPanel(title, panel, disposeEnabled)
        Me.ShowNew(form, log)
    End Sub

    ''' <summary> Adds and shows the form. </summary>
    ''' <param name="form">  The form. </param>
    ''' <param name="panel"> The panel. </param>
    ''' <param name="log">   The log. </param>
    Public Overloads Sub ShowNew(ByVal form As Instrument.InstrumentPanelForm,
                                 ByVal panel As isr.Core.Pith.PropertyNotifyControlBase,
                                 ByVal log As MyLog)
        If form Is Nothing Then Throw New ArgumentNullException(NameOf(form))
        form.AddPropertyNotifyControl(panel)
        Me.ShowNew(form, log)
    End Sub

    ''' <summary> Adds and shows the form. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="form">  The form. </param>
    ''' <param name="panel"> The panel. </param>
    ''' <param name="log">   The log. </param>
    Public Overloads Sub ShowNew(ByVal form As Instrument.InstrumentPanelForm,
                                 ByVal panel As isr.Core.Pith.TalkerControlBase,
                                 ByVal log As MyLog)
        If form Is Nothing Then Throw New ArgumentNullException(NameOf(form))
        form.AddTalkerControl(panel)
        Me.ShowNew(form, log)
    End Sub

    ''' <summary> Adds and shows the form. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="title"> The title. </param>
    ''' <param name="form">  The form. </param>
    ''' <param name="panel"> The panel. </param>
    ''' <param name="log">   The log. </param>
    Public Overloads Sub ShowNew(ByVal title As String,
                                 ByVal form As Instrument.InstrumentPanelForm,
                                 ByVal panel As isr.Core.Pith.TalkerControlBase,
                                 ByVal log As MyLog)
        If form Is Nothing Then Throw New ArgumentNullException(NameOf(form))
        form.AddTalkerControl(title, panel)
        Me.ShowNew(form, log)
    End Sub

End Class

#Region " UNUSED "
#If False Then
    ''' <summary> Updates the trace log level described by traceLevel. </summary>
    ''' <param name="traceLevel"> The trace level. </param>
    Public Overrides Sub UpdateTraceLogLevel(ByVal traceLevel As TraceEventType)
        MyBase.UpdateTraceLogLevel(traceLevel)
        Me._TalkerControl?.UpdateTraceLogLevel(traceLevel)
    End Sub

    ''' <summary> Updates the trace show level described by traceLevel. </summary>
    ''' <param name="traceLevel"> The trace level. </param>
    Public Overrides Sub UpdateTraceShowLevel(ByVal traceLevel As TraceEventType)
        MyBase.UpdateTraceShowLevel(traceLevel)
        Me._TalkerControl?.UpdateTraceShowLevel(traceLevel)
    End Sub
#End If
#End Region