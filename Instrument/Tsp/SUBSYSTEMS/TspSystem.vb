Imports isr.Core.Pith.EventHandlerExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Defines a Test Script Processor (TSP) System consists of one or more TSP nodes. </summary>
''' <license> (c) 2007 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/9/2013" by="David" revision="">             Uses new VISA library. </history>
''' <history date="03/21/2007" by="David" revision="1.15.2636.x"> Created. </history>
Public Class TspSystem
    Implements IDisposable

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Constructor. </summary>
    ''' <param name="device"> The master device. </param>
    Public Sub New(ByVal device As MasterDevice)
        MyBase.New()
        Me._Device = device
        Me._ScriptManager = New ScriptManager(device)
        ' set the default file path for scripts.
        Me.ScriptManager.FilePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "Scripts"))
        AddHandler Me.ScriptManager.PropertyChanged, AddressOf ScriptManagerPropertyChanged
    End Sub

    ''' <summary>Calls <see cref="M:Dispose(Boolean Disposing)"/> to cleanup.</summary>
    ''' <remarks>Do not make this method Overridable (virtual) because a derived 
    '''   class should not be able to override this method.</remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(True)
        ' GC.SuppressFinalize is issued to take this object off the 
        ' finalization(Queue) and prevent finalization code for this 
        ' prevent from executing a second time.
        GC.SuppressFinalize(Me)
    End Sub

    ''' <summary>
    ''' Gets or sets the dispose status sentinel of the base class.  This applies to the derived class
    ''' provided proper implementation.
    ''' </summary>
    Protected Property IsDisposed() As Boolean

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me.ScriptManager IsNot Nothing Then
                    RemoveHandler Me.ScriptManager.PropertyChanged, AddressOf ScriptManagerPropertyChanged
                    Me._ScriptManager.Dispose()
                    Me._ScriptManager = Nothing
                End If
                Me.RemoveConnectionChangedEventHandler(Me.ConnectionChangedEvent)
                If Me._Device IsNot Nothing Then Me._Device.Dispose() : Me._Device = Nothing
            End If
        Finally
            ' set the sentinel indicating that the class was disposed.
            Me.IsDisposed = True
        End Try
    End Sub

    ''' <summary>This destructor will run only if the Dispose method 
    '''   does not get called. It gives the base class the opportunity to 
    '''   finalize. Do not provide destructors in types derived from this class.</summary>
    Protected Overrides Sub Finalize()
        Try
            ' Do not re-create Dispose clean-up code here.
            ' Calling Dispose(false) is optimal for readability and maintainability.
            Dispose(False)
        Finally
            ' The compiler automatically adds a call to the base class finalizer 
            ' that satisfies the rule: FinalizersShouldCallBaseClassFinalizer.
            MyBase.Finalize()
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Clears all status data structure registers (enable, event, negative and positive transitions)
    ''' to their power up states.  The naked TSP clear does not clear the
    ''' error queue. This command adds clear for error queue.
    '''          </summary>
    Public Sub ClearExecutionState()
        Me.Device.ClearExecutionState()
    End Sub

    ''' <summary>
    ''' Resets, clears and initializes the device. Starts with issuing a selective-device-clear, reset (RST),
    ''' Clear Status (CLS, and clear error queue) and initialize.
    ''' </summary>
    Public Sub ResetClearInit()
        Me.Device.ResetClearInit()
    End Sub

    ''' <summary> Resets the Device to its known state. </summary>
    Public Sub ResetKnownState()
        ' RST issues the reset() message, which returns all the nodes
        ' on the TSP LInk system to the original factory defaults:
        Me.Device.ResetKnownState()
    End Sub

#End Region

#Region " TSP ENTITIES "

    Private WithEvents _Device As MasterDevice

    ''' <summary> Gets or sets reference to the
    ''' <see cref="MasterDeviceBase">master device</see> for accessing this TSP system. </summary>
    ''' <value> The master device. </value>
    Public ReadOnly Property Device() As MasterDevice
        Get
            Return Me._Device
        End Get
    End Property

#Region " SCRIPT MANAGER "

    ''' <summary> Gets or sets the manager for script. </summary>
    ''' <value> The script manager. </value>
    Public ReadOnly Property ScriptManager As ScriptManager

    ''' <summary> Handles the Script Manager property changed event. </summary>
    ''' <param name="sender">    The script manager. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnPropertyChanged(ByVal sender As ScriptManager, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> ScriptManager subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ScriptManagerPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, ScriptManager), e?.PropertyName)
        Catch ex As Exception
            Me.ScriptManager.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                             "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Gets the is device open. </summary>
    ''' <value> The is device open. </value>
    Public ReadOnly Property IsDeviceOpen As Boolean
        Get
            Return Me.Device IsNot Nothing AndAlso Me.Device.IsDeviceOpen
        End Get
    End Property

    ''' <summary> Gets the is session open. </summary>
    ''' <value> The is session open. </value>
    Public ReadOnly Property IsSessionOpen As Boolean
        Get
            Return Me.Device IsNot Nothing AndAlso Me.Device.Session.IsSessionOpen
        End Get
    End Property

    ''' <summary> Device service requested. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Message based session event information. </param>
    Private Sub DeviceServiceRequested(ByVal sender As Object, ByVal e As EventArgs) Handles _Device.ServiceRequested
    End Sub

    ''' <summary> Event handler. Called upon device opening. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub DeviceOpening(ByVal sender As Object, ByVal e As ComponentModel.CancelEventArgs) Handles _Device.Opening
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Device.Opened
        ' moved to the calling application
        ' Me.Device.InitializeKnownState()
        Me.OnConnectionChanged(System.EventArgs.Empty)
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub DeviceClosing(ByVal sender As Object, ByVal e As ComponentModel.CancelEventArgs) Handles _Device.Closing
        If Me.IsDeviceOpen Then
        End If
    End Sub

    ''' <summary> Event handler. Called when device is closed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub DeviceClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Device.Closed
        Me.OnConnectionChanged(System.EventArgs.Empty)
    End Sub

#End Region

#Region " EVENT HANDLERS "

    ''' <summary> Raised to update the instrument connection ExecutionState. </summary>
    ''' <param name="sender">Specifies reference to the <see cref="TspSystem">TSP system.</see>.
    ''' </param>
    ''' <param name="e">Specifies the <see cref="EventArgs">event arguments</see>.
    ''' </param>
    Public Event ConnectionChanged As EventHandler(Of EventArgs)

    ''' <summary> Removes event handler. </summary>
    ''' <param name="value"> The handler. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub RemoveConnectionChangedEventHandler(ByVal value As EventHandler(Of EventArgs))
        For Each d As [Delegate] In value.SafeInvocationList
            Try
                RemoveHandler Me.ConnectionChanged, CType(d, EventHandler(Of EventArgs))
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToString)
            End Try
        Next
    End Sub

    ''' <summary> Raises an event to alert on change of connection. </summary>
    ''' <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
    Public Sub OnConnectionChanged(ByVal e As System.EventArgs)
        Dim evt As EventHandler(Of EventArgs) = Me.ConnectionChangedEvent
        evt?.Invoke(Me, e)
    End Sub

#End Region

End Class
