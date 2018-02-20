Imports System.Windows.Forms
Imports isr.VI.ExceptionExtensions
''' <summary> Selects a VISA resource. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created. </history>
Public Class ResourceSelectorDialog
    Inherits isr.Core.Pith.TopDialogBase

#Region " CONSTRUCTORS AND DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializeComponent()
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' unable to use null conditional because it is not seen by code analysis
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

    ''' <summary> Does all the post processing after all the form controls are rendered as the user
    ''' expects them. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Form_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        ' allow form rendering time to complete: process all messages currently in the queue.
        Application.DoEvents()

        Try

            ' Turn on the form hourglass cursor
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Me.ResourceName = ""
            Me.displayResources()


            ' allow some events to occur for refreshing the display.
            Application.DoEvents()

        Catch ex As Exception

            Me._AvailableResourcesListBox.Items.Add(ex.ToFullBlownString)

        Finally

            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Event handler. Called by availableResources for double click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _AvailableResourcesListBox_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _AvailableResourcesListBox.DoubleClick
        Dim selectedString As String = CType(Me._AvailableResourcesListBox.SelectedItem, String)
        ResourceName = selectedString
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    ''' <summary> Event handler. Called by availableResources for selected index changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _AvailableResourcesListBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _AvailableResourcesListBox.SelectedIndexChanged
        Dim selectedString As String = CType(Me._AvailableResourcesListBox.SelectedItem, String)
        Me.ResourceName = selectedString
    End Sub

    ''' <summary> Gets or sets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Property ResourceName() As String
        Get
            Return Me._ResourceNameTextBox.Text
        End Get
        Set(ByVal Value As String)
            Me._ResourceNameTextBox.Text = Value
        End Set
    End Property

    ''' <summary> Selects first not serial instrument. </summary>
    Private Sub SelectFirstNotSerialInstrument()
        For Each item As String In Me._AvailableResourcesListBox.Items
            If item.Contains("GPIB") AndAlso item.Contains("INSTR") Then
                Me._AvailableResourcesListBox.SelectedItem = item
                Exit For
            End If
        Next
    End Sub

    ''' <summary> Displays the resources. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplayResources()

        Try

            ' Turn on the form hourglass cursor
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Me._AvailableResourcesListBox.Items.Clear()
            Dim resources As IEnumerable(Of String) = New String() {}
            Using rm As ResourcesManagerBase = isr.VI.SessionFactory.Get.Factory.CreateResourcesManager()
                resources = rm.FindResources()
            End Using

            If resources.Any Then
                For Each s As String In resources
                    Me._AvailableResourcesListBox.Items.Add(s)
                Next
                Me.selectFirstNotSerialInstrument()
            Else
                If resources.Count = 1 Then
                    Me._AvailableResourcesListBox.Items.Add(
$"VISA Manager found no local resources. It reported: {resources(0)}. Make sure at least one VISA instrument is connected and try again. If this fails, try entering a resource name such as the sample formats in the Resource Name edit box and select OK.")
                Else
                    Me._AvailableResourcesListBox.Items.Add("VISA Manager found no local resources. Make sure at least one VISA instrument is connected. If this fails, try entering a resource name such as the sample formats in the Resource Name edit box and select OK.")
                End If
                Me._AvailableResourcesListBox.Items.Add("GPIB[board]::number[::INSTR]")
                Me._AvailableResourcesListBox.Items.Add("GPIB[board]::INTFC")
                Me._AvailableResourcesListBox.Items.Add("TCPIP[board]::host address[::LAN device name][::INSTR]")
                Me._AvailableResourcesListBox.Items.Add("TCPIP[board]::host address::port::SOCKET")
            End If

            ' allow some events to occur for refreshing the display.
            Application.DoEvents()

        Catch ex As Exception

            Me._AvailableResourcesListBox.Items.Add(ex.ToFullBlownString)

        Finally

            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Event handler. Called by _RefreshButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _RefreshButton_Click(sender As System.Object, ByVal e As System.EventArgs) Handles _RefreshButton.Click
        Me.displayResources()
    End Sub

End Class