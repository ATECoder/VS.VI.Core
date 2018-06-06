Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Includes code for Switchboard Form, which serves as a switchboard for this program. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/11/95" by="David" revision="1.0.2110.x"> Created. </history>
Public Class Switchboard
    Inherits isr.Core.Pith.UserFormBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializeComponent()
        ' turn off saving to prevent exception reported due to access from two programs
        Me.SaveSettingsOnClosing = False
        Me._OpenForms = New Core.Pith.ConsoleFormCollection
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
                ' this disposes of any form that did not close.
                If Me._OpenForms IsNot Nothing Then Me._OpenForms.ClearDispose()
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " FORM EVENT HANDLERS "

    ''' <summary> Event handler. Called by form for load events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Form"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            ' Turn on the form hourglass cursor
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            ' set the form caption
            Me.Text = Core.Pith.ApplicationInfo.BuildApplicationTitleCaption("SWITCH BOARD")

            ' center the form
            Me.CenterToScreen()

        Catch

            ' Use throw without an argument in order to preserve the stack location 
            ' where the exception was initially raised.
            Throw

        Finally

            Me.Cursor = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    ''' <summary> Event handler. Called by form for shown events. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Form"/> </param>
    ''' <param name="e">      Event information. </param>
    Private Sub Form_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            ' Turn on the form hourglass cursor
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            ' populate the action list
            Me.populateActiveList()

        Catch

            ' Use throw without an argument in order to preserve the stack location 
            ' where the exception was initially raised.
            Throw

        Finally

            Me.Cursor = System.Windows.Forms.Cursors.Default


        End Try
    End Sub


#End Region

#Region " PRIVATE  and  PROTECTED "

    ''' <summary> Populates the list of options in the action combo box. </summary>
    ''' <remarks> It seems that out enumerated list does not work very well with this list. </remarks>
    Private Sub PopulateActiveList()

        ' set the action list
        Me._ApplicationsListBox.DataSource = Nothing
        Me._ApplicationsListBox.Items.Clear()
        Me._ApplicationsListBox.DataSource = GetType(ActionOption).ValueDescriptionPairs()
        Me._ApplicationsListBox.ValueMember = "Key"
        Me._ApplicationsListBox.DisplayMember = "Value"

    End Sub

    ''' <summary> Gets the selected action. </summary>
    ''' <value> The selected action. </value>
    Private ReadOnly Property SelectedAction() As ActionOption
        Get
            Return CType(CType(Me._ApplicationsListBox.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, ActionOption)
        End Get
    End Property

#End Region

#Region " CONTROL EVENT HANDLERS "

    ''' <summary> Closes the form and exits the application. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Form"/> </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ExitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ExitButton.Click
        Me.Close()
    End Sub

    ''' <summary> Enumerates the action options. </summary>
    Private Enum ActionOption
        <System.ComponentModel.Description("Interface")> InterfaceForm
        <System.ComponentModel.Description("Simple Read and Write")> SimpleReadAndWrite
        <System.ComponentModel.Description("Keithley 2002")> Keithley2002
        <System.ComponentModel.Description("Keithley 2700")> Keithley2700
        <System.ComponentModel.Description("Tegam T1750")> Tegam1750
        <System.ComponentModel.Description("Keithley 2400")> Keithley2400
    End Enum

    Private _OpenForms As Core.Pith.ConsoleFormCollection
    ''' <summary> Open selected items. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Form"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
                                  Justification:="Disposed when closed as part for the forms collection")>
    Private Sub _OpenButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _OpenButton.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Select Case Me.selectedAction
            Case ActionOption.InterfaceForm
                Me._OpenForms.ShowNew("INTERFACE", New Core.Pith.ConsoleForm, New Instrument.InterfacePanel, My.Application.MyLog, True)
            Case ActionOption.SimpleReadAndWrite
                Me._OpenForms.ShowNew("R/W", New Core.Pith.ConsoleForm, New Instrument.SimpleReadWritePanel, My.Application.MyLog, True)
            Case ActionOption.Keithley2002
                Me._OpenForms.ShowNew("Meter", New Core.Pith.ConsoleForm, New K2000.K2000Panel, My.Application.MyLog, True)
            Case ActionOption.Tegam1750
                Me._OpenForms.ShowNew("Meter", New Core.Pith.ConsoleForm, New Tegam.T1750Panel, My.Application.MyLog, True)
            Case ActionOption.Keithley2700
                Me._OpenForms.ShowNew("Switch/Meter", New Core.Pith.ConsoleForm, New K2700.K2700Panel, My.Application.MyLog, True)
            Case ActionOption.Keithley2400
                Me._OpenForms.ShowNew("Source Meter", New Core.Pith.ConsoleForm, New K2400.K2400Panel, My.Application.MyLog, True)
        End Select
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub


#End Region

End Class