Imports System.Windows.Forms
Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
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
    Inherits isr.Core.Pith.ConsoleForm

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializingComponents = True
        Me.InitializeComponent()
        Me.InitializingComponents = False
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

#End Region

#Region " INSTRUMENT PANEL "

    ''' <summary> true to enable, false to disable the instrument panel dispose. </summary>
    Private _InstrumentPanelDisposeEnabled As Boolean

    Private WithEvents _InstrumentPanel As Instrument.ResourcePanelBase

    ''' <summary> Adds an instrument panel. </summary>
    ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    '''                                      illegal values. </exception>
    ''' <param name="title">          The title. </param>
    ''' <param name="panelBase">      The panel control. </param>
    ''' <param name="disposeEnabled"> true to enable, false to disable the dispose. </param>
    Public Sub AddInstrumentPanel(ByVal title As String, ByVal panelBase As Instrument.ResourcePanelBase, ByVal disposeEnabled As Boolean)
        If panelBase Is Nothing Then Throw New ArgumentNullException(NameOf(panelBase))
        Me.ResizeClientArea(panelBase)
        Me._InstrumentPanel = panelBase
        Me.Talker.ApplyTalkerTraceLevels(panelBase.Talker)
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
    Private Overloads Sub HandlePropertyChange(sender As Instrument.ResourcePanelBase, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Instrument.ResourcePanelBase.ResourceTitle)
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
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Instrument.ResourcePanelBase)}.{e?.PropertyName} change event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf _InstrumentPanel_PropertyChanged), New Object() {sender, e})
            ElseIf Not Me.InitializingComponents AndAlso sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.HandlePropertyChange(TryCast(sender, Instrument.ResourcePanelBase), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                ' used for figuring out what events are raised to cause this. 
                ' possibly the device is sending messages after the control is disposed indicating that the device needs to 
                ' be disposed before the control is disposed. 
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
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
    Inherits isr.Core.Pith.ConsoleFormCollection

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

End Class

