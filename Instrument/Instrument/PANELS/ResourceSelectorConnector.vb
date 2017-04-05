Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Pith
Imports isr.Core.Pith.EventHandlerExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.Core.Pith.ExceptionExtensions
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

        ' This call is required by the Windows Form Designer.
        Me.InitializeComponent()
        Me._ToggleConnectionButton.Enabled = False
        Me._ClearButton.Enabled = False
        Me._FindButton.Enabled = True
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

    Private _connectible As Boolean
    ''' <summary> Gets or sets the value indicating if the connect button is visible and can be
    ''' enabled. An item can be connected only if it is selected. </summary>
    ''' <value> The connectible. </value>
    <Category("Appearance"), Description("Shows or hides the Connect button"), Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
    RefreshProperties(RefreshProperties.All), DefaultValue(True)>
    Public Property Connectible() As Boolean
        Get
            Return Me._connectible
        End Get
        Set(ByVal value As Boolean)
            If Not Me.Connectible.Equals(value) Then
                Me._connectible = value
                Me.SafePostPropertyChanged()
            End If
            Me._ToggleConnectionButton.Visible = value
            If Not value Then
                ' cannot clear a device if we do not connect to it.
                Me.Clearable = False
            End If
        End Set
    End Property

    Private _searchable As Boolean
    ''' <summary> Gets or sets the condition determining if the control can be searchable. The elements
    ''' can be searched only if not connected. </summary>
    ''' <value> The searchable. </value>
    <Category("Appearance"), Description("Shows or hides the Search (Find) button"), Browsable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(True)>
    Public Property Searchable() As Boolean
        Get
            Return Me._searchable
        End Get
        Set(ByVal value As Boolean)
            If Not Me.Searchable.Equals(value) Then
                Me._searchable = value
                Me.SafePostPropertyChanged()
            End If
            Me._FindButton.Visible = value
        End Set
    End Property

#End Region

#Region " CONNECT "

    Private _isConnected As Boolean
    ''' <summary> Gets or sets the connected status and enables the clear button. </summary>
    ''' <value> The is connected. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property IsConnected() As Boolean
        Get
            Return Me._isConnected
        End Get
    End Property

    ''' <summary> Toggle connection. </summary>
    ''' <remarks> David, 12/23/2015. </remarks>
    ''' <param name="value"> The value. </param>
    Private Sub UpdateConnectionState(ByVal value As Boolean)
        ' enable or disable based on the connection status.
        Me._ResourceNamesComboBox.Enabled = Not value
        Me._ClearButton.Enabled = Me.Clearable AndAlso value
        Me._FindButton.Enabled = Me.Searchable AndAlso Not value
        Me._ToggleConnectionButton.ToolTipText = $"Click to {If(value, "Disconnect", "Connect")}"
        Me._ToggleConnectionButton.Image = If(value, My.Resources.Disconnect_22x22, My.Resources.Connect_22x22)
        Me._ToggleConnectionButton.Enabled = Me.Connectible
        Me._isConnected = value
        Me.SafePostPropertyChanged(NameOf(Me.IsConnected))
    End Sub

    ''' <summary> Executes the toggle connection action. </summary>
    ''' <remarks> David, 12/23/2015. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification:="OK")>
    Private Sub OnToggleConnection(ByVal sender As ToolStripItem)
        If sender Is Nothing Then Return
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            If Me.IsConnected Then
                Me.OnDisconnect(New System.ComponentModel.CancelEventArgs)
            Else
                Me.OnConnect(New System.ComponentModel.CancelEventArgs)
            End If
        Catch ex As Exception
            Me._ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception connecting resource '{Me.SelectedResourceName}';. Details: {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Occurs when the connect button is depressed. </summary>
    Public Event Connect As EventHandler(Of System.ComponentModel.CancelEventArgs)

    ''' <summary> Removes the event handler. </summary>
    ''' <remarks> David, 12/21/2015. </remarks>
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
        If e IsNot Nothing AndAlso Not e.Cancel Then Me.UpdateConnectionState(True)
    End Sub

    ''' <summary> Occurs when the connect button is release. </summary>
    Public Event Disconnect As EventHandler(Of System.ComponentModel.CancelEventArgs)

    ''' <summary> Removes the event handler. </summary>
    ''' <remarks> David, 12/21/2015. </remarks>
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
        If e IsNot Nothing AndAlso Not e.Cancel Then Me.UpdateConnectionState(False)
    End Sub

    Private _ConnectionChanging As Boolean
    Private Sub _ToggleConnectionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ToggleConnectionButton.Click
        If sender IsNot Nothing AndAlso Not Me._ConnectionChanging Then Me.OnToggleConnection(TryCast(sender, ToolStripItem))
    End Sub

#End Region

#Region " RESOURCE NAMES "

    Private _HasResources As Boolean
    ''' <summary> Gets or sets the has resources. </summary>
    ''' <value> The has resources. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property HasResources As Boolean
        Get
            Return _HasResources
        End Get
        Set(ByVal value As Boolean)
            If Not Me.HasResources.Equals(value) Then
                Me._HasResources = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _ResourcesFilter As String
    ''' <summary> Gets or sets the resources search pattern. </summary>
    ''' <value> The resources search pattern. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ResourcesFilter As String
        Get
            Return Me._ResourcesFilter
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.ResourcesFilter) Then
                Me._ResourcesFilter = value
                Me._FindButton.ToolTipText = $"Search using the search pattern '{value}'"
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Displays the resource names based on the <see cref="ResourcesFilter">search pattern</see>. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub DisplayResourceNames()
        Dim resources As IEnumerable(Of String) = New String() {}
        Me._ToggleConnectionButton.Enabled = False
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            Using rm As ResourcesManagerBase = isr.VI.SessionFactory.Get.Factory.CreateResourcesManager()
                If String.IsNullOrWhiteSpace(Me.ResourcesFilter) Then
                    resources = rm.FindResources()
                Else
                    resources = rm.FindResources(Me.ResourcesFilter).ToArray
                End If
            End Using
            If resources.Count = 0 Then
                Me.HasResources = False
                Me._ResourceNamesComboBox.ToolTipText = isr.VI.My.Resources.LocalResourceNotFoundSynopsis
                Dim message As String = $"{isr.VI.My.Resources.LocalResourceNotFoundSynopsis};. {isr.VI.My.Resources.LocalResourcesNotFoundHint}."
                Me._ErrorProvider.Annunciate(Me._FindButton, message)
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, message)
            Else
                Me._ResourceNamesComboBox.ComboBox.DataSource = Nothing
                Me._ResourceNamesComboBox.Items.Clear()
                Me._ResourceNamesComboBox.ComboBox.DataSource = resources
                Me.HasResources = True
                Me._ResourceNamesComboBox.ToolTipText = isr.VI.My.Resources.LocalResourceSelectorTip
            End If
        Catch ex As Exception
            Me.HasResources = False
            Me._ErrorProvider.Annunciate(Me._FindButton, ex.Message)
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{isr.VI.My.Resources.LocalResourceNotFoundSynopsis};. {isr.VI.My.Resources.LocalResourcesNotFoundHint}.{Environment.NewLine}Details: {ex.ToFullBlownString}.")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Displays the search patters. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub DisplayResourceNamePatterns()

        ' clear the interface names
        Me._ResourceNamesComboBox.ComboBox.DataSource = Nothing
        Me._ResourceNamesComboBox.Items.Clear()

        Dim resourceList As New List(Of String)
        resourceList.Add("GPIB[board]::number[::INSTR]")
        resourceList.Add("GPIB[board]::INTFC")
        resourceList.Add("TCPIP[board]::host address[::LAN device name][::INSTR]")
        resourceList.Add("TCPIP[board]::host address::port::SOCKET")

        ' set the list of available names
        Me._ResourceNamesComboBox.ComboBox.DataSource = resourceList.ToArray

    End Sub

    ''' <summary> Gets the name of the entered resource. </summary>
    ''' <value> The name of the entered resource. </value>
    Private ReadOnly Property EnteredResourceName As String
        Get
            Return Me._ResourceNamesComboBox.Text.Trim
        End Get
    End Property

    Private _SelectedResourceName As String
    ''' <summary> Returns the selected resource name. </summary>
    ''' <value> The name of the selected. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SelectedResourceName() As String
        Get
            Return Me._SelectedResourceName
        End Get
        <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification:="OK")>
        Set(ByVal Value As String)
            If String.IsNullOrWhiteSpace(Value) Then Value = ""
            If Not String.Equals(Value, Me.SelectedResourceName, StringComparison.OrdinalIgnoreCase) Then
                If Not String.IsNullOrWhiteSpace(Value) Then
                    Me._SelectedResourceName = Value
                    Me.SafePostPropertyChanged()
                    Try
                        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
                        Me._ErrorProvider.Clear()
                        Using rm As ResourcesManagerBase = isr.VI.SessionFactory.Get.Factory.CreateResourcesManager()
                            Me.SelectedResourceExists = rm.Exists(Value)
                        End Using
                    Catch ex As Exception
                        Me._ErrorProvider.Annunciate(Me._ResourceNamesComboBox, ex.Message)
                        Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception setting selected resource;. Details:{0}", ex.ToFullBlownString)
                    Finally
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                    End Try
                    If Not Value.Equals(Me.EnteredResourceName) Then Me._ResourceNamesComboBox.Text = Value
                End If
            End If
        End Set
    End Property

    Private _SelectedResourceExists As Boolean
    ''' <summary> Gets or sets the has resources. </summary>
    ''' <value> The has resources. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property SelectedResourceExists As Boolean
        Get
            Return _SelectedResourceExists
        End Get
        Private Set(ByVal value As Boolean)
            ' not checking for changed value here because this needs to be refreshed if a new
            ' resource was selected.
            Me._ToggleConnectionButton.Enabled = Me.Connectible AndAlso value
            Me._SelectedResourceExists = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Event handler. Called by _ResourceNamesComboBox for validated events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ResourceNamesComboBox_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResourceNamesComboBox.Validated
        Me._ErrorProvider.Clear()
        Me.SelectedResourceName = Me.EnteredResourceName
    End Sub

    ''' <summary> Selects a resource. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification:="OK")>
    Private Sub _ResourceNamesComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ResourceNamesComboBox.SelectedIndexChanged
        Me._ErrorProvider.Clear()
        Me.SelectedResourceName = Me.EnteredResourceName
    End Sub

#End Region

#Region " FIND NAMES HANDLERS "

    ''' <summary> Occurs when the find button is clicked. </summary>
    Public Event FindNames As EventHandler(Of EventArgs)

    ''' <summary> Removes the event handler. </summary>
    ''' <remarks> David, 12/21/2015. </remarks>
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
        Me._SelectedResourceName = ""
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
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception finding resources;. Details: {ex.ToFullBlownString}")
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

#End Region

#Region " CLEAR "

    ''' <summary> Synchronously Invokes <see cref="OnClear">clear</see> to clear resources or whatever else needs
    ''' clearing. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification:="OK")>
    Private Sub _ClearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ClearButton.Click
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            Me.OnClear(New EventArgs)
        Catch ex As Exception
            Dim c As ToolStripItem = TryCast(sender, ToolStripItem)
            If c IsNot Nothing Then
                Me._ErrorProvider.Annunciate(c, ex.Message)
            End If
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception clearing resource '{Me.SelectedResourceName}';. Details: {ex.ToFullBlownString}")
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Occurs when the clear button is clicked. </summary>
    Public Event Clear As EventHandler(Of EventArgs)

    ''' <summary> Removes the event handler. </summary>
    ''' <remarks> David, 12/21/2015. </remarks>
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

#End Region

End Class
