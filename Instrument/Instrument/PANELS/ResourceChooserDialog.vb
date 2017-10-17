Imports System.ComponentModel
''' <summary> Selects a resource. </summary>
''' <license> (c) 2006 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="02/08/2006" by="David" revision="1.0.2229.x">  Created. </history>
Public Class ResourceChooserDialog
    Inherits isr.Core.Pith.TopDialogBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()

        ' This method is required by the Windows Form Designer.
        InitializeComponent()

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

#Region " RESOURCE NAMES "

    ''' <summary> Gets or sets the resources search pattern. </summary>
    ''' <value> The resources search pattern. </value>
    Public Property ResourcesFilter As String

    ''' <summary> Returns the selected Resource name or gets the last resource from the caller. </summary>
    ''' <value> The name of the selected resource. </value>
    Public Property SelectedResourceName() As String
        Get
            Return Me._ResourceNameSelectorConnector.SelectedResourceName
        End Get
        Set(ByVal Value As String)
            Me._ResourceNameSelectorConnector.SelectedResourceName = Value
        End Set
    End Property

    ''' <summary> Gets or sets the caption just above the name selector. </summary>
    ''' <value> The selector caption. </value>
    Public Property SelectorCaption() As String
        Get
            Return Me._ResourceNameSelectorConnectorLabel.Text
        End Get
        Set(ByVal value As String)
            Me._ResourceNameSelectorConnectorLabel.Text = value
        End Set
    End Property

#End Region

#Region " FORM AND CONTROL EVENT HANDLERS "

    ''' <summary> Event handler. Called by _acceptButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _AcceptButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _AcceptButton.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    ''' <summary> Event handler. Called by _cancelButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _CancelButton.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    ''' <summary> Event handler. Called by form for load events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            ' Turn on the form hourglass cursor
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

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
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub Form_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me._ResourceNameSelectorConnector.ResourcesFilter = Me.ResourcesFilter
        Me._ResourceNameSelectorConnector.DisplayResourceNames()
    End Sub

    ''' <summary> Event handler. Called by _nameSelector for double click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ResourceNameSelectorConnector_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResourceNameSelectorConnector.DoubleClick
        If String.IsNullOrWhiteSpace(Me.SelectedResourceName) Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    ''' <summary> Updates the resource names. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ResourceNameSelectorConnector_FindNames(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResourceNameSelectorConnector.FindNames
        Me._ResourceNameSelectorConnector.ResourcesFilter = Me.ResourcesFilter
        Me._ResourceNameSelectorConnector.DisplayResourceNames()
    End Sub

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.SelectedResourceName)
                Me._AcceptButton.Enabled = sender.SelectedResourceName.Length > 0
        End Select
    End Sub

    ''' <summary> Event handler. Called by _ResourceNameSelectorConnector for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ResourceNameSelectorConnector_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Handles _ResourceNameSelectorConnector.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._ResourceNameSelectorConnector_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, ResourceSelectorConnector), e?.PropertyName)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}.", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

End Class