Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Pith
Imports isr.Core.Pith.EventHandlerExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> A control for selecting and connecting to a VISA resource. </summary>
''' <license> (c) 2006 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="02/20/2006" by="David" revision="1.0.2242.x"> created. </history>
<Description("Resource Selector and Connector Base Control"), DefaultEvent("Connect")>
<System.Drawing.ToolboxBitmap(GetType(ResourceSelectorConnector), "ResourceSelectorConnector"), ToolboxItem(True)>
Public Class ResourceSelectorConnector
    Inherits TalkerControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializingComponents = True
        ' This call is required by the Windows Form Designer.
        Me.InitializeComponent()
        Me.InitializingComponents = False
        Me._clearable = True
        Me._Connectable = True
        Me._Searchable = True
        Me._ToggleConnectionButton.Enabled = False
        Me._ClearButton.Enabled = False
        Me._FindButton.Enabled = True
        Me._SessionFactory = New VI.SessionFactory
        Me._SessionFactory.CaptureSyncContext(Windows.Forms.WindowsFormsSynchronizationContext.Current)
    End Sub

    ''' <summary> Creates a new ResourceSelectorConnector. </summary>
    ''' <returns> A ResourceSelectorConnector. </returns>
    Public Shared Function Create() As ResourceSelectorConnector
        Dim connector As ResourceSelectorConnector = Nothing
        Try
            connector = New ResourceSelectorConnector
            Return connector
        Catch
            connector.Dispose()
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Me.RemoveClearEventHandler(Me.ClearEvent)
                Me.RemoveConnectEventHandler(Me.ConnectEvent)
                Me.RemoveDisconnectEventHandler(Me.DisconnectEvent)
                Me.RemoveFindNamesEventHandler(Me.FindNamesEvent)
                ' unable to use null conditional because it is not seen by code analysis
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " BROWSABLE PROPERTIES "

    Private _clearable As Boolean
    ''' <summary> Gets or sets the value indicating if the clear button is visible and can be enabled.
    ''' An item can be cleared only if it is connected. </summary>
    ''' <value> The clearable. </value>
    <Category("Appearance"), Description("Shows or hides the Clear button"), Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(True)>
    Public Property Clearable() As Boolean
        Get
            Return Me._clearable
        End Get
        Set(ByVal value As Boolean)
            If Not Me.Clearable.Equals(value) Then
                Me._clearable = value
                Me.SafePostPropertyChanged()
            End If
            Me._ClearButton.Visible = value
        End Set
    End Property

    Private _Connectable As Boolean
    ''' <summary> Gets or sets the value indicating if the connect button is visible and can be
    ''' enabled. An item can be connected only if it is selected. </summary>
    ''' <value> The connectable. </value>
    <Category("Appearance"), Description("Shows or hides the Connect button"), Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
    RefreshProperties(RefreshProperties.All), DefaultValue(True)>
    Public Property Connectable() As Boolean
        Get
            Return Me._Connectable
        End Get
        Set(ByVal value As Boolean)
            If Not Me.Connectable.Equals(value) Then
                Me._Connectable = value
                Me.SafePostPropertyChanged()
            End If
            Me._ToggleConnectionButton.Visible = value
            Me._ToolStrip.Invalidate()
            If Not value Then
                ' cannot clear a device if we do not connect to it.
                Me.Clearable = False
            End If
        End Set
    End Property

    Private _Searchable As Boolean
    ''' <summary> Gets or sets the condition determining if the control can be searchable. The elements
    ''' can be searched only if not connected. </summary>
    ''' <value> The searchable. </value>
    <Category("Appearance"), Description("Shows or hides the Search (Find) button"), Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(True)>
    Public Property Searchable() As Boolean
        Get
            Return Me._Searchable
        End Get
        Set(ByVal value As Boolean)
            If Not Me.Searchable.Equals(value) Then
                Me._Searchable = value
                Me.SafePostPropertyChanged()
            End If
            Me._FindButton.Visible = value
        End Set
    End Property

#End Region

#Region " CONNECT "

    Private _IsConnected As Boolean
    ''' <summary> Gets or sets the connected status and enables the clear button. </summary>
    ''' <value> The is connected. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property IsConnected() As Boolean
        Get
            Return Me._IsConnected
        End Get
        Set(value As Boolean)
            If value <> Me.IsConnected Then
                ' enable or disable based on the connection status.
                Me._ResourceNamesComboBox.Enabled = Not value
                Me._ClearButton.Enabled = Me.Clearable AndAlso value
                Me._FindButton.Enabled = Me.Searchable AndAlso Not value
                Me._ToggleConnectionButton.ToolTipText = $"Click to {If(value, "Disconnect", "Connect")}"
                ' turning off visibility is required -- otherwise, control overflows and both search and toggle disappear 
                Me._ToggleConnectionButton.Visible = False
                Me._ToggleConnectionButton.Image = If(value, My.Resources.Connect_22x22, My.Resources.Disconnect_22x22)
                Me._ToggleConnectionButton.Visible = Me.Connectable
                Me._ToggleConnectionButton.Enabled = Me.Connectable
                Me._ToolStrip.Invalidate()
                Me._IsConnected = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Executes the toggle connection action. </summary>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification:="OK")>
    Private Sub OnToggleConnection(ByVal sender As ToolStripItem)
        If sender Is Nothing Then Return
        Dim activity As String = ""
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            If Me.IsConnected Then
                activity = $"connecting '{Me.SessionFactory.EnteredResourceName}'"
                Me.OnDisconnect(New System.ComponentModel.CancelEventArgs)
            Else
                activity = $"disconnecting '{Me.SessionFactory.EnteredResourceName}'"
                Me.OnConnect(New System.ComponentModel.CancelEventArgs)
            End If
        Catch ex As Exception
            Me._ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Occurs when the connect button is depressed. </summary>
    Public Event Connect As EventHandler(Of System.ComponentModel.CancelEventArgs)

    ''' <summary> Removes the event handler. </summary>
    ''' <param name="value"> The value. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveConnectEventHandler(ByVal value As EventHandler(Of System.ComponentModel.CancelEventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.Connect, CType(d, EventHandler(Of System.ComponentModel.CancelEventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub

    ''' <summary> Raises the Connect event. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Sub OnConnect(ByVal e As System.ComponentModel.CancelEventArgs)
        Me._ConnectionChanging = True
        Dim evt As EventHandler(Of System.ComponentModel.CancelEventArgs) = Me.ConnectEvent
        evt?.Invoke(Me, e)
        Me._ConnectionChanging = False
        If e Is Nothing OrElse Not e.Cancel Then Me.IsConnected = True
    End Sub

    ''' <summary> Occurs when the connect button is release. </summary>
    Public Event Disconnect As EventHandler(Of System.ComponentModel.CancelEventArgs)

    ''' <summary> Removes the event handler. </summary>
    ''' <param name="value"> The value. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveDisconnectEventHandler(ByVal value As EventHandler(Of System.ComponentModel.CancelEventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.Disconnect, CType(d, EventHandler(Of System.ComponentModel.CancelEventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub

    ''' <summary> Raises the Disconnect event. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Sub OnDisconnect(ByVal e As System.ComponentModel.CancelEventArgs)
        Me._ConnectionChanging = True
        Dim evt As EventHandler(Of System.ComponentModel.CancelEventArgs) = Me.DisconnectEvent
        evt?.Invoke(Me, e)
        Me._ConnectionChanging = False
        If e Is Nothing OrElse Not e.Cancel Then Me.IsConnected = False
    End Sub

    Private _ConnectionChanging As Boolean
    Private Sub _ToggleConnectionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ToggleConnectionButton.Click
        If sender IsNot Nothing AndAlso Not Me._ConnectionChanging Then Me.OnToggleConnection(TryCast(sender, ToolStripItem))
    End Sub

#End Region

#Region " SESSION FACTORY "

    Private WithEvents _SessionFactory As VI.SessionFactory

    ''' <summary> Gets the session factory. </summary>
    ''' <value> The session factory. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property SessionFactory As VI.SessionFactory
        Get
            Return Me._SessionFactory
        End Get
    End Property

    ''' <summary> Handles the property changed. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Overloads Sub HandlePropertyChanged(sender As VI.SessionFactory, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(VI.SessionFactory.HasResources)
                    ' if No resources, issues alerts
                    Me.DisplayEnumeratedResourceNames()
                Case NameOf(VI.SessionFactory.ResourcesFilter)
                    Me._FindButton.ToolTipText = $"Search using the search pattern '{sender.ResourcesFilter}'"
                Case NameOf(VI.SessionFactory.SelectedResourceExists)
                    sender.OpenResourceSessionEnabled = Me.Connectable AndAlso sender.SelectedResourceExists
                Case NameOf(VI.SessionFactory.SelectedResourceName)
                    If Not String.Equals(Me._ResourceNamesComboBox.Text, sender.SelectedResourceName) Then
                        Me._ResourceNamesComboBox.Text = sender.SelectedResourceName
                    End If
                Case NameOf(VI.SessionFactory.OpenResourceSessionEnabled)
                    Me._ToggleConnectionButton.Enabled = sender.OpenResourceSessionEnabled
            End Select
            Me._ToolStrip.Invalidate()
            Me.SafePostPropertyChanged(propertyName)
        End If
    End Sub

    ''' <summary> Session factory property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SessionFactory_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _SessionFactory.PropertyChanged
        If sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.SessionFactory)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._SessionFactory_PropertyChanged), New Object() {sender, e})
            Else
                If sender IsNot Nothing AndAlso e IsNot Nothing Then
                    Me.HandlePropertyChanged(TryCast(sender, VI.SessionFactory), e.PropertyName)
                End If
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

#Region " RESOURCE NAMES "

    ''' <summary> Displays an enumerated resource names. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplayEnumeratedResourceNames()
        Me._ToggleConnectionButton.Enabled = False
        Dim activity As String = $"displaying {NameOf(VI.SessionFactory.EnumeratedResources)}"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, activity)
            If Me.SessionFactory.HasResources Then
                VI.SessionFactory.DisplayResourceNames(Me._ResourceNamesComboBox.ComboBox, Me.SessionFactory.EnumeratedResources)
                Me._ResourceNamesComboBox.ToolTipText = VI.Pith.My.Resources.LocalResourceSelectorTip
            Else
                Me._ResourceNamesComboBox.ToolTipText = VI.Pith.My.Resources.LocalResourceNotFoundSynopsis
                activity = $"{VI.Pith.My.Resources.LocalResourceNotFoundSynopsis};. {VI.Pith.My.Resources.LocalResourcesNotFoundHint}"
                Me._ErrorProvider.Annunciate(Me._FindButton, activity)
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, activity)
            End If
        Catch ex As Exception
            Me._ErrorProvider.Annunciate(Me._FindButton, ex.Message)
            If Me.Talker Is Nothing Then
                isr.VI.Instrument.My.MyLibrary.LogUnpublishedMessage(New TraceMessage(TraceEventType.Error, My.MyLibrary.TraceEventId,
$"Exception {activity};. 
logged to: {My.Application.Log.DefaultFileLogWriter.FullLogFileName};
{ex.ToFullBlownString}"))
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{VI.Pith.My.Resources.LocalResourceNotFoundSynopsis};. {VI.Pith.My.Resources.LocalResourcesNotFoundHint}
{ex.ToFullBlownString}")
            End If
        Finally
            Me._ToolStrip.Invalidate()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Displays the resource names based on the <see cref="vi.SessionFactory.ResourcesFilter">search pattern</see>. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DisplayResourceNames()
        Dim resources As IEnumerable(Of String) = New String() {}
        Me._ToggleConnectionButton.Enabled = False
        Dim activity As String = $"displaying {NameOf(VI.SessionFactory.EnumeratedResources)}"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, activity)
            Me.SessionFactory.DisplayResourceNames(Me._ResourceNamesComboBox.ComboBox)
            If Me.SessionFactory.HasResources Then
                Me._ResourceNamesComboBox.ToolTipText = VI.Pith.My.Resources.LocalResourceSelectorTip
            Else
                Me._ResourceNamesComboBox.ToolTipText = VI.Pith.My.Resources.LocalResourceNotFoundSynopsis
                activity = $"{VI.Pith.My.Resources.LocalResourceNotFoundSynopsis};. {VI.Pith.My.Resources.LocalResourcesNotFoundHint}."
                Me._ErrorProvider.Annunciate(Me._FindButton, activity)
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, activity)
            End If
            If resources.Count = 0 Then
            Else
            End If
        Catch ex As Exception
            Me._ErrorProvider.Annunciate(Me._FindButton, ex.Message)
            If Me.Talker Is Nothing Then
                isr.VI.Instrument.My.MyLibrary.LogUnpublishedMessage(New TraceMessage(TraceEventType.Error, My.MyLibrary.TraceEventId,
$"Exception {activity};. 
logged to: {My.Application.Log.DefaultFileLogWriter.FullLogFileName};
{ex.ToFullBlownString}"))
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        Finally
            Me._ToolStrip.Invalidate()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Displays the search patterns. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Sub DisplayResourceNamePatterns()
        ' clear the interface names
        Me._ResourceNamesComboBox.ComboBox.DataSource = Nothing
        Me._ResourceNamesComboBox.Items.Clear()
        Me._ResourceNamesComboBox.ComboBox.DataSource = SessionFactory.EnumerateDefaultResourceNamePatterns
    End Sub

    ''' <summary> Attempts to select resource from the given data. </summary>
    ''' <param name="resourceName"> The value. </param>
    ''' <param name="e">     Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification:="OK")>
    Private Function TrySelectResource(ByVal resourceName As String, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If String.IsNullOrWhiteSpace(resourceName) Then resourceName = ""
        Dim activity As String = ""
        Try
            Me._ErrorProvider.Clear()
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            activity = "selecting resource"
            If Not Me.SessionFactory.TrySelectResource(resourceName, e) Then
                Me._ErrorProvider.Annunciate(Me._ResourceNamesComboBox, e.Details)
                activity = Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Failed {activity};. {e.Details}")
            End If
        Catch ex As Exception
            Me._ErrorProvider.Annunciate(Me._ResourceNamesComboBox, ex.Message)
            Dim message As New TraceMessage(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            activity = Me.Talker?.Publish(message)
            e.RegisterCancellation(activity)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Event handler. Called by _ResourceNamesComboBox for validated events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ResourceNamesComboBox_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResourceNamesComboBox.Validated
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.TrySelectResource(Me._ResourceNamesComboBox.Text, New isr.Core.Pith.ActionEventArgs)
    End Sub

    ''' <summary> Selects a resource. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification:="OK")>
    Private Sub _ResourceNamesComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ResourceNamesComboBox.SelectedIndexChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.TrySelectResource(Me._ResourceNamesComboBox.Text, New isr.Core.Pith.ActionEventArgs)
    End Sub

#End Region

#Region " FIND NAMES HANDLERS "

    ''' <summary> Occurs when the find button is clicked. </summary>
    Public Event FindNames As EventHandler(Of EventArgs)

    ''' <summary> Removes the event handler. </summary>
    ''' <param name="value"> The value. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveFindNamesEventHandler(ByVal value As EventHandler(Of EventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.FindNames, CType(d, EventHandler(Of EventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub

    ''' <summary> Raises the FindNames event. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Sub OnFindNames(ByVal e As EventArgs)
        ' clear the selected resource to make sure a new selection is 
        ' made after find.
        Me._ResourceNamesComboBox.Text = ""
        Dim evt As EventHandler(Of EventArgs) = Me.FindNamesEvent
        evt?.Invoke(Me, e)
    End Sub

    ''' <summary> Get the container to update the resource names. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification:="OK")>
    Private Sub _FindButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _FindButton.Click
        Try
            Me._ErrorProvider.Clear()
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me.OnFindNames(New EventArgs)
        Catch ex As Exception
            Dim c As ToolStripItem = TryCast(sender, ToolStripItem)
            If c IsNot Nothing Then
                Me._ErrorProvider.Annunciate(c, ex.Message)
            End If
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception finding resources;. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

#End Region

#Region " CLEAR "

    ''' <summary> Gets or sets the clear tool tip text. </summary>
    ''' <value> The clear tool tip text. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ClearToolTipText As String
        Get
            Return Me._ClearButton.ToolTipText
        End Get
        Set(value As String)
            Me._ClearButton.ToolTipText = value
            Me._ToolStrip.Invalidate()
        End Set
    End Property

    ''' <summary> Synchronously Invokes <see cref="OnClear">clear</see> to clear resources or whatever else needs
    ''' clearing. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification:="OK")>
    Private Sub _ClearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ClearButton.Click
        Dim activity As String = "clearing"
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            If String.IsNullOrWhiteSpace(Me.SessionFactory.SelectedResourceName) OrElse Not Me.SessionFactory.SelectedResourceExists Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                               $"{activity} ignore; resource not selected or does not exists")
            Else
                activity = $"clearing {Me.SessionFactory.SelectedResourceName}"
                Me.OnClear(New EventArgs)
            End If
        Catch ex As Exception
            Dim c As ToolStripItem = TryCast(sender, ToolStripItem)
            If c IsNot Nothing Then
                Me._ErrorProvider.Annunciate(c, ex.Message)
            End If
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Occurs when the clear button is clicked. </summary>
    Public Event Clear As EventHandler(Of EventArgs)

    ''' <summary> Removes the event handler. </summary>
    ''' <param name="value"> The value. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveClearEventHandler(ByVal value As EventHandler(Of EventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.Clear, CType(d, EventHandler(Of EventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
    End Sub

    ''' <summary> Raises the clear. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Sub OnClear(ByVal e As EventArgs)
        Dim evt As EventHandler(Of EventArgs) = Me.ClearEvent
        evt?.Invoke(Me, e)
    End Sub

    Private Sub _ResourceNamesComboBox_DoubleClick(sender As Object, e As EventArgs) Handles _ResourceNamesComboBox.DoubleClick
        Static connected As Boolean
        Me._ToggleConnectionButton.Visible = False
        Me._ToggleConnectionButton.Image = If(connected, My.Resources.Connect_22x22, My.Resources.Disconnect_22x22)
        Me._ToggleConnectionButton.Visible = True
        connected = False
        Me._ToolStrip.Refresh()
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

#End Region

#Region " UNIT TESTS INTERNALS "

    Friend ReadOnly Property InternalResourceNamesCount As Integer
        Get
            Return Me._ResourceNamesComboBox.Items.Count
        End Get
    End Property

    Friend ReadOnly Property InternalConnected As Boolean
        Get
            Return Me.IsConnected
        End Get
    End Property

    Friend ReadOnly Property InternalSelectedResourceName As String
        Get
            Return Me._ResourceNamesComboBox.Text
        End Get
    End Property

#End Region

End Class
