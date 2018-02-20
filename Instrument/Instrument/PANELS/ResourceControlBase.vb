Imports System.ComponentModel
Imports isr.Core.Controls
Imports isr.Core.Controls.ControlExtensions
Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
''' <summary> Provides a base user interface for a <see cref="isr.VI.DeviceBase">Visa Device</see>. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="09/04/2013" by="David" revision="3.0.4955"> created based on the legacy VISA
''' resource panel. </history>
<System.ComponentModel.Description("Resource Panel Base Control")>
<System.Drawing.ToolboxBitmap(GetType(ResourcePanelBase), "Panels.ResourcePanelBase.bmp"), ToolboxItem(True)>
Public Class ResourceControlBase
    Inherits TalkerControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(ResourceSelectorConnector.Create)
        Me._IsConnectorOwner = True
    End Sub

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    ''' <param name="connector"> The connector. </param>
    Public Sub New(ByVal connector As ResourceSelectorConnector)
        MyBase.New()
        Me.InitializeComponent()
        Me.AssignConnector(connector)
        Me._IsConnectorOwner = False
        Me._OpenResourceTitleFormat = ResourceControlBase.DefaultOpenSourceTitleFormat
        Me._ClosedResourceTitleFormat = ResourceControlBase.DefaultClosedResourceTitleFormat
        Me._ResourceTitle = ResourceControlBase.DefaultResourceTitle
    End Sub

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
                If Me.IsConnectorOwner Then Me.Connector.Dispose()
                Me.Connector = Nothing
                If Me.DeviceBase?.IsDeviceOpen Then
                    Try
                        If Me.IsDeviceOwner Then
                            ' if the device owner, then close the session
                            Me.DeviceBase.CloseSession()
                        Else
                            ' if not device owner, just release the event handlers associated with this panel. 
                            Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                        End If
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                End If
                If Me.DeviceBase IsNot Nothing Then
                    Try
                        Me.ReleaseDevice()
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred releasing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                End If
                Me._ElapsedTimeStopwatch = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    ''' <summary> Releases the device. </summary>
    Protected Overridable Sub ReleaseDevice()
        ' this also releases the device event handlers associated with this panel. 
        Me.DeviceBase = Nothing
    End Sub


#End Region

#Region " FORM EVENT HANDLERS "

    ''' <summary> Handles the <see cref="E:System.Windows.Forms.UserControl.Load" /> event. </summary>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Try
            Me.Status = "Find and select a resource."
            Me.Identity = ""
        Finally
            MyBase.OnLoad(e)
        End Try
    End Sub

#End Region

#Region " STATUS REPORTING "

    ''' <summary> Gets or sets the status. </summary>
    ''' <value> The status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overridable Property Status As String

    ''' <summary> Gets or sets the identity. </summary>
    ''' <value> The identity. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overridable Property Identity As String

    ''' <summary> Gets or sets the status register caption. </summary>
    ''' <value> The status register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overridable Property StatusRegisterCaption As String

    Private _StatusRegisterStatus As Integer

    ''' <summary> Gets or sets the status register status. </summary>
    ''' <value> The status register status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Property StatusRegisterStatus As Integer
        Get
            Return Me._StatusRegisterStatus
        End Get
        Set(value As Integer)
            If value <> Me.StatusRegisterStatus Then
                Me._StatusRegisterStatus = value
                Me.DisplayStatusRegisterStatus(value)
            End If
        End Set
    End Property

    ''' <summary> Displays the status register status. </summary>
    ''' <param name="value">  The register value. </param>
    ''' <param name="format"> The format. </param>
    Public Sub DisplayStatusRegisterStatus(ByVal value As Integer, ByVal format As String)
        If Not value.Equals(Me._StatusRegisterStatus) Then
            Me._StatusRegisterStatus = value
            Me.StatusRegisterCaption = String.Format(format, value)
        End If
    End Sub

    ''' <summary> The unknown register value. </summary>
    Public Shared Property UnknownRegisterValue As Integer = -1

    ''' <summary> The unknown register value caption. </summary>
    Public Shared Property UnknownRegisterValueCaption As String = "0x.."

    ''' <summary> The default register value format. </summary>
    Public Shared Property DefaultRegisterValueFormat As String = "0x{0:X2}"

    ''' <summary> Displays the status register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Public Overridable Sub DisplayStatusRegisterStatus(ByVal value As Integer)
        If value = ResourceControlBase.UnknownRegisterValue Then
            Me.StatusRegisterCaption = ResourceControlBase.UnknownRegisterValueCaption
        Else
            Me.DisplayStatusRegisterStatus(value, ResourceControlBase.DefaultRegisterValueFormat)
        End If
    End Sub

    ''' <summary> Gets or sets the standard register caption. </summary>
    ''' <value> The standard register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overridable Property StandardRegisterCaption As String

    Private _StandardRegisterStatus As Integer?
    ''' <summary> Gets or sets the standard register status. </summary>
    ''' <value> The standard register status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Property StandardRegisterStatus As Integer?
        Get
            Return Me._StandardRegisterStatus
        End Get
        Set(value As Integer?)
            If Not Nullable.Equals(value, Me.StandardRegisterStatus) Then
                Me._StandardRegisterStatus = value
                Me.DisplayStandardRegisterStatus(value)
            End If
        End Set
    End Property

    ''' <summary> Displays the status register status. </summary>
    ''' <param name="value">  The register value. </param>
    ''' <param name="format"> The format. </param>
    Public Sub DisplayStandardRegisterStatus(ByVal value As Integer, ByVal format As String)
        If Not value.Equals(Me._StandardRegisterStatus) Then
            Me._StandardRegisterStatus = value
            Me.StandardRegisterCaption = String.Format(format, value)
        End If
    End Sub

    ''' <summary> Displays the status register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Public Overridable Sub DisplayStandardRegisterStatus(ByVal value As Integer?)
        If value.HasValue Then
            If value = ResourceControlBase.UnknownRegisterValue Then
                Me.StandardRegisterCaption = ResourceControlBase.UnknownRegisterValueCaption
            Else
                Me.DisplayStandardRegisterStatus(value.Value, ResourceControlBase.DefaultRegisterValueFormat)
            End If
        Else
            Me.StandardRegisterCaption = ResourceControlBase.UnknownRegisterValueCaption
        End If
    End Sub

#End Region

#Region " ANNUNCIATE - OBJECT "

    ''' <summary> Annunciates error. </summary>
    ''' <param name="sender">  The event sender. </param>
    ''' <param name="details"> The details. </param>
    ''' <returns> A String. </returns>
    Public Overridable Function Annunciate(ByVal sender As Object, ByVal level As InfoProviderLevel, ByVal details As String) As String
        Return details
    End Function

    ''' <summary> Annunciates error. </summary>
    ''' <param name="sender"> The event sender. </param>
    ''' <param name="format"> Describes the format to use. </param>
    ''' <param name="args">   A variable-length parameters list containing arguments. </param>
    ''' <returns> A String. </returns>
    Public Function Annunciate(ByVal sender As Object, ByVal level As InfoProviderLevel, ByVal format As String, ByVal ParamArray args() As Object) As String
        Return Me.Annunciate(sender, level, String.Format(format, args))
    End Function

#End Region

#Region " SAFE SETTERS "

    ''' <summary> Safe text setter. </summary>
    ''' <param name="control"> The control. </param>
    ''' <param name="value">   True to show or False to hide the control. </param>
    Private Shared Sub SafeTextSetter(ByVal control As System.Windows.Forms.ToolStripItem, value As String)
        If control IsNot Nothing AndAlso control.Owner IsNot Nothing AndAlso
            Not String.Equals(control.Text, value) Then
            If control.Owner.InvokeRequired Then
                control.Owner.Invoke(New Action(Of System.Windows.Forms.ToolStripItem, String)(AddressOf ResourceControlBase.SafeTextSetter), New Object() {control, value})
            Else
                control.Text = value
            End If
        End If
    End Sub

#End Region

#Region " RESOURCE NAME "

    ''' <summary> Gets or sets the name of the entered resource. </summary>
    ''' <value> The name of the entered resource. </value>
    Public Property EnteredResourceName As String
        Get
            Return Me.Connector.EnteredResourceName
        End Get
        Set(value As String)
            Me.Connector.EnteredResourceName = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    Private _ResourceName As String

    ''' <summary> Gets or sets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ResourceName As String
        Get
            Return Me._ResourceName
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.ResourceName, StringComparison.OrdinalIgnoreCase) Then
                Me._ResourceName = value
                Me.Identity = Me.ResourceName
                Me.SafePostPropertyChanged()
                Me.OnTitleChanged(Me.BuildTitle)
            End If
            If Not String.Equals(Me.ResourceName, Me.Connector.SelectedResourceName, StringComparison.OrdinalIgnoreCase) Then
                Me.Connector.SelectedResourceName = value
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the default open source title format. </summary>
    ''' <value> The default open source title format. </value>
    Public Shared Property DefaultOpenSourceTitleFormat As String = "{0}.{1}"

    ''' <summary> Gets or sets the title. </summary>
    ''' <value> The title. </value>
    <Category("Appearance"), Description("The format of the open resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}.{1}")>
    Public Property OpenResourceTitleFormat As String

    ''' <summary> Gets or sets the default closed resource title format. </summary>
    ''' <value> The default closed resource title format. </value>
    Public Shared Property DefaultClosedResourceTitleFormat As String = "{0}"

    ''' <summary> Gets or sets the title. </summary>
    ''' <value> The title. </value>
    <Category("Appearance"), Description("The format of the close resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}")>
    Public Property ClosedResourceTitleFormat As String

    ''' <summary> Gets or sets the default resource title. </summary>
    ''' <value> The default resource title. </value>
    Public Shared Property DefaultResourceTitle As String = "INSTR"

    Private _ResourceTitle As String
    ''' <summary> Gets or sets the title. </summary>
    ''' <value> The title. </value>
    <Category("Appearance"), Description("The resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("INSTR")>
    Public Property ResourceTitle As String
        Get
            Return Me._ResourceTitle
        End Get
        Set(value As String)
            Me._ResourceTitle = value
            Me.SafePostPropertyChanged()
            Me.OnTitleChanged(Me.BuildTitle)
        End Set
    End Property

    ''' <summary> Builds the title. </summary>
    ''' <returns> A String. </returns>
    Protected Function BuildTitle() As String
        Dim result As String = ""
        If Me.IsDeviceOpen Then
            If String.IsNullOrWhiteSpace(Me.OpenResourceTitleFormat) OrElse String.IsNullOrWhiteSpace(Me.ResourceTitle) Then
                result = ""
            Else
                result = String.Format(Me.OpenResourceTitleFormat, Me.ResourceTitle, Me.ResourceName)
            End If
        Else
            If String.IsNullOrWhiteSpace(Me.ClosedResourceTitleFormat) OrElse String.IsNullOrWhiteSpace(Me.ResourceTitle) Then
                result = ""
            Else
                result = String.Format(Me.ClosedResourceTitleFormat, Me.ResourceTitle)
            End If
        End If
        If String.IsNullOrWhiteSpace(result) Then
            Stop
        End If
        Return result
    End Function

#End Region

#Region " DEVICE "

    ''' <summary> Gets or sets the elapsed time stop watch. </summary>
    ''' <value> The elapsed time stop watch. </value>
    Protected ReadOnly Property ElapsedTimeStopwatch As Stopwatch

    ''' <summary> Gets or sets the sentinel indicating if this panel owns the device and, therefore, needs to 
    '''           dispose of this device. </summary>
    ''' <value> The is device owner. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property IsDeviceOwner As Boolean

    ''' <summary> Gets the is device assigned. </summary>
    ''' <value> The is device assigned. </value>
    Public Overridable ReadOnly Property IsDeviceAssigned As Boolean
        Get
            Return Me.DeviceBase IsNot Nothing AndAlso Not Me.DeviceBase.IsDisposed
        End Get
    End Property

    Private _DeviceBase As VI.DeviceBase
    ''' <summary> Gets or sets reference to the VISA <see cref="VI.DeviceBase">device</see>
    ''' interfaces. </summary>
    ''' <value> The connectable resource. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property DeviceBase() As VI.DeviceBase
        Get
            Return Me._DeviceBase
        End Get
        Set(value As VI.DeviceBase)
            Me.AssignDevice(value)
        End Set
    End Property

    ''' <summary> Assign device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub AssignDevice(ByVal value As DeviceBase)
        If Me.DeviceBase IsNot Nothing Then
            If Me.DeviceBase.Talker IsNot Nothing Then Me.DeviceBase.Talker.RemoveListeners()
            RemoveHandler Me.DeviceBase.PropertyChanged, AddressOf Me.DevicePropertyChanged
            RemoveHandler Me.DeviceBase.Opening, AddressOf Me.DeviceOpening
            RemoveHandler Me.DeviceBase.Opened, AddressOf Me.DeviceOpened
            RemoveHandler Me.DeviceBase.Closing, AddressOf Me.DeviceClosing
            RemoveHandler Me.DeviceBase.Closed, AddressOf Me.DeviceClosed
            RemoveHandler Me.DeviceBase.Initialized, AddressOf Me.DeviceInitialized
            RemoveHandler Me.DeviceBase.Initializing, AddressOf Me.DeviceInitializing
            ' note that service request is released when device closes.
            Me.StatusSubsystem = Nothing
            ' this also closes the session. 
            If value Is Nothing AndAlso Me.IsDeviceOwner Then Me._DeviceBase.Dispose()
        End If
        Me._ElapsedTimeStopwatch = New Stopwatch
        If Me.Connector IsNot Nothing Then
            Me.Connector.ResourcesFilter = ""
            Me.Connector.Clearable = True
            Me.Connector.Connectable = True
            Me.Connector.Searchable = True
        End If
        Me._StatusRegisterStatus = ResourceControlBase.UnknownRegisterValue
        Me._StandardRegisterStatus = ResourceControlBase.UnknownRegisterValue
        Me._DeviceBase = value
        If Me.DeviceBase IsNot Nothing Then
            Me.DeviceBase.CaptureSyncContext(Threading.SynchronizationContext.Current)
            AddHandler Me.DeviceBase.PropertyChanged, AddressOf Me.DevicePropertyChanged
            AddHandler Me.DeviceBase.Opening, AddressOf Me.DeviceOpening
            AddHandler Me.DeviceBase.Opened, AddressOf Me.DeviceOpened
            AddHandler Me.DeviceBase.Closing, AddressOf Me.DeviceClosing
            AddHandler Me.DeviceBase.Closed, AddressOf Me.DeviceClosed
            AddHandler Me.DeviceBase.Initialized, AddressOf Me.DeviceInitialized
            AddHandler Me.DeviceBase.Initializing, AddressOf Me.DeviceInitializing
            Me.Connector.ResourcesFilter = If(Me.DeviceBase.ResourcesFilter, Me.DeviceBase.ResourcesFilter)
            Me.Connector.IsConnected = value.IsDeviceOpen
            Me.ResourceName = Me.DeviceBase.ResourceName
            Me.ResourceTitle = Me.DeviceBase.ResourceTitle
            Me.StatusSubsystem = Me.DeviceBase.StatusSubsystemBase

            Me.DeviceBase.NotifyDeviceOpenState()
            Me.AssignTalker(value.Talker)
            Me.ApplyListenerTraceLevel(ListenerType.Display, value.Talker.TraceShowLevel)
        End If
    End Sub

    ''' <summary> Gets the sentinel indicating if the device is open. </summary>
    ''' <value> The is device open. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected ReadOnly Property IsDeviceOpen As Boolean
        Get
            Return Me.DeviceBase IsNot Nothing AndAlso Me.DeviceBase.IsDeviceOpen
        End Get
    End Property

#Region " SERVICE REQUESTED "

    ''' <summary> Event handler. Called when the device is requesting service. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Message based session event information. </param>
    Protected Overridable Sub DeviceServiceRequested(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    ''' <summary> Gets the sentinel indicating if service requests handler was added for this panel. </summary>
    ''' <value> The service request event handler add sentinel. </value>
    Protected ReadOnly Property PanelServiceRequestHandlerAdded As Boolean

    ''' <summary> Gets or sets the device service request enabled. </summary>
    ''' <value> The device service request enabled. </value>
    Protected ReadOnly Property PanelDeviceServiceRequestHandlerAdded As Boolean

    ''' <summary> Adds a panel service request handler. </summary>
    ''' <remarks>
    ''' The service request for the session must be enabled for handling service requests on the device. 
    ''' The panel can add the service request handler if: (1) is owner or; (2) if not owner but service request handler was not
    ''' added on the device. In case 2, the panel could also remove the device handler.
    ''' </remarks>
    Public Overridable Sub AddServiceRequestEventHandler()
        If Not Me.PanelServiceRequestHandlerAdded Then
            AddHandler Me.DeviceBase.ServiceRequested, AddressOf Me.DeviceServiceRequested
            If Me.IsDeviceOwner Then
                Me.DeviceBase.AddServiceRequestEventHandler()
            ElseIf Not Me.DeviceBase.DeviceServiceRequestHandlerAdded Then
                Me.DeviceBase.AddServiceRequestEventHandler()
                Me._PanelDeviceServiceRequestHandlerAdded = True
            End If
            Me._PanelServiceRequestHandlerAdded = True
        End If
    End Sub

    ''' <summary> Removes the service request event. </summary>
    Public Overridable Sub RemoveServiceRequestEventHandler()
        If Me.PanelServiceRequestHandlerAdded Then
            RemoveHandler Me.DeviceBase.ServiceRequested, AddressOf Me.DeviceServiceRequested
            If Me.IsDeviceOwner OrElse Me._PanelDeviceServiceRequestHandlerAdded Then Me.DeviceBase.RemoveServiceRequestEventHandler()
            Me._PanelServiceRequestHandlerAdded = False
            Me._PanelDeviceServiceRequestHandlerAdded = False
        End If
    End Sub

#End Region

#Region " PROPERTY CHANGE "

    ''' <summary> Recursively enable. </summary>
    ''' <param name="controls"> The controls. </param>
    ''' <param name="value">    The value. </param>
    Public Sub RecursivelyEnable(ByVal controls As System.Windows.Forms.Control.ControlCollection, ByVal value As Boolean)
        If controls IsNot Nothing Then
            For Each c As System.Windows.Forms.Control In controls
                If c IsNot Me.Connector Then
                    c.RecursivelyEnable(value)
                End If
            Next
        End If
    End Sub

    ''' <summary> Executes the device open changed action. 
    '''           The open event occurs after all subsystems are created. </summary>
    Protected Overridable Sub OnDeviceOpenChanged(ByVal device As DeviceBase)
        If device IsNot Nothing Then
            Me.Connector.IsConnected = device.IsDeviceOpen
        End If
    End Sub

    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Sub OnDevicePropertyChanged(ByVal device As DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(device.IsDeviceOpen)
                ' the open sentinel is turned on after all subsystems are set
                Me.OnDeviceOpenChanged(device)
            Case NameOf(device.Enabled)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                  $"{device.ResourceTitle} {device.Enabled.GetHashCode:enabled;enabled;disabled};. ")
            Case NameOf(device.ResourcesFilter)
                Me.Connector.ResourcesFilter = device.ResourcesFilter
            Case NameOf(device.ServiceRequestFailureMessage)
                If Not String.IsNullOrWhiteSpace(device.ServiceRequestFailureMessage) Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, device.ServiceRequestFailureMessage)
                End If
            Case NameOf(device.ResourceTitle)
                Me.ResourceTitle = device.ResourceTitle
            Case NameOf(device.ResourceName)
                Me.ResourceName = device.ResourceName
        End Select
    End Sub

    ''' <summary> Event handler. Called for property changed events. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DevicePropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnDevicePropertyChanged(TryCast(sender, DeviceBase), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              $"Exception handling Device.{e?.PropertyName} change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " OPENING / OPEN "

    ''' <summary> Attempts to open a session to the device using the specified resource name. </summary>
    ''' <param name="resourceName">  The name of the resource. </param>
    ''' <param name="resourceTitle"> The title. </param>
    ''' <param name="e">             Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:   DoNotCatchGeneralExceptionTypes")>
    Public Function TryOpenSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal e As CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim action As String = $"opening {resourceName} VISA session"
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of String, String, CancelDetailsEventArgs)(AddressOf Me.TryOpenSession), New Object() {resourceName, resourceTitle, e})
        Else
            Try
                Me.OpenSession(resourceName, resourceTitle)
                If Not Me.DeviceBase.IsDeviceOpen Then
                    e.RegisterCancellation($"failed {action};. ")
                End If
            Catch ex As Exception
                e.RegisterCancellation($"Exception opening {action};. {ex.ToFullBlownString}")
            Finally
                Me.Connector.IsConnected = Me.IsDeviceOpen
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            End Try
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> Attempts to open a session to the device using the connector resource name and title. </summary>
    Public Function TryOpenSession(ByVal e As CancelDetailsEventArgs) As Boolean
        Return Me.TryOpenSession(Me.Connector.SelectedResourceName, Me.ResourceTitle, e)
    End Function

    ''' <summary>
    ''' Opens a session to the device using the specified resource name.
    ''' </summary>
    ''' <param name="resourceName">  The name of the resource. </param>
    ''' <param name="resourceTitle"> The title. </param>
    Public Sub OpenSession(ByVal resourceName As String, ByVal resourceTitle As String)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of String, String)(AddressOf Me.OpenSession), New Object() {resourceName, resourceTitle})
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Opening {resourceName} VISA session;. ")
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            Me.DeviceBase.OpenSession(resourceName, resourceTitle)
            If Me.DeviceBase.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Opened {resourceName} VISA session;. ")
            Else
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Failed opening {resourceName} VISA session;. ")
            End If
            Me.Connector.IsConnected = Me.IsDeviceOpen
        End If
    End Sub

    ''' <summary> Opens a session to the instrument. </summary>
    Public Sub OpenSession()
        Me.OpenSession(Me.Connector.SelectedResourceName, Me.ResourceTitle)
    End Sub

    ''' <summary> Event handler. Called upon device opening so as to instantiated all subsystems. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceOpening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Me._StatusRegisterStatus = ResourceControlBase.UnknownRegisterValue
        Me._StandardRegisterStatus = ResourceControlBase.UnknownRegisterValue
    End Sub

    ''' <summary> Event handler. Called after the device opened and all subsystems were defined. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Connector.IsConnected = Me.IsDeviceOpen
        Me.ResourceName = Me.DeviceBase.ResourceName
        If Not String.IsNullOrEmpty(Me.ResourceTitle) Then Me.DeviceBase.Session.ResourceTitle = Me.ResourceTitle
        Dim outcome As TraceEventType = TraceEventType.Information
        If Me.DeviceBase.Session.Enabled And Not Me.DeviceBase.Session.IsSessionOpen Then outcome = TraceEventType.Warning
        Me.Talker.Publish(outcome, My.MyLibrary.TraceEventId,
                          "{0} {1:enabled;enabled;disabled} and {2:open;open;closed}; session {3:open;open;closed};. ",
                          Me.ResourceTitle,
                          Me.DeviceBase.Session.Enabled.GetHashCode,
                          Me.DeviceBase.Session.IsDeviceOpen.GetHashCode,
                          Me.DeviceBase.Session.IsSessionOpen.GetHashCode)
    End Sub

#End Region

#Region " INITALIZING / INITIALIZED  "

    Protected Overridable Sub DeviceInitializing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
    End Sub

    ''' <summary> Device initialized. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceInitialized(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ReadServiceRequestStatus()
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overridable Sub OnTitleChanged(ByVal value As String)
        Me.Title = value
    End Sub

    ''' <summary> Gets or sets the title. </summary>
    ''' <value> The title. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Title As String

    ''' <summary> Reads a service request status. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ReadServiceRequestStatus()
        Try
            Me.DeviceBase.StatusSubsystemBase.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading service request;. {0}", ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " CLOSING / CLOSED "

    ''' <summary> Attempts to close session from the given data. </summary>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:   DoNotCatchGeneralExceptionTypes")>
    Public Function TryCloseSession(ByVal e As CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of CancelDetailsEventArgs)(AddressOf Me.TryCloseSession), New Object() {e})
        Else
            Try
                Me.CloseSession()
                If Me.DeviceBase.IsDeviceOpen Then
                    e.RegisterCancellation($"Failed closing {ResourceName} VISA session;. ")
                End If
            Catch ex As Exception
                e.RegisterCancellation($"Exception closing {ResourceName} VISA session;. {ex.ToFullBlownString}")
            Finally
                Me.Connector.IsConnected = Me.IsDeviceOpen
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            End Try
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> Closes the instrument VISA session if open. </summary>
    Public Sub CloseSession()
        If Me.InvokeRequired Then
            Me.Invoke(New Action(AddressOf Me.CloseSession))
        Else
            If Me.DeviceBase.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Closing {Me.DeviceBase.ResourceName} VISA session;. ")
            End If
            If Me.DeviceBase.TryCloseSession() Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Closed {Me.DeviceBase.ResourceName} VISA session;. ")
            Else
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Failed closing {Me.Connector.SelectedResourceName} VISA session;. ")
            End If
            Me.Connector.IsConnected = Me.IsDeviceOpen
        End If
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.DeviceBase IsNot Nothing Then
            Me.RemoveServiceRequestEventHandler()
            If Me.DeviceBase.Session IsNot Nothing Then
                Me.DeviceBase.Session.DisableServiceRequest()
            End If
        End If
    End Sub

    ''' <summary> Event handler. Called when device is closed. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceClosed(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.DeviceBase IsNot Nothing Then
            If Me.DeviceBase.Session.IsSessionOpen Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Device closed but session still open;. ")
            ElseIf Me.DeviceBase.Session.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Device closed but emulated session still open;. ")
            Else
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Disconnected;. Device access closed.")
            End If
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Disconnected;. Device disposed.")
        End If
    End Sub

#End Region

#End Region

#Region " CONNECTOR SELECTOR EVENT HANDLERS "

    ''' <summary> Gets or sets the is connector that owns this item. </summary>
    ''' <value> The is connector owner. </value>
    Public Property IsConnectorOwner As Boolean

    Private _Connector As ResourceSelectorConnector

    ''' <summary> Gets or sets the connector. </summary>
    ''' <value> The connector. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overridable Property Connector As ResourceSelectorConnector
        Get
            Return Me._Connector
        End Get
        Set(value As ResourceSelectorConnector)
            Me.AssignConnector(value)
        End Set
    End Property

    ''' <summary> Assign connector. </summary>
    ''' <param name="value"> The connector value. </param>
    Private Sub AssignConnector(ByVal value As ResourceSelectorConnector)
        If Me._Connector IsNot Nothing Then
            RemoveHandler Me._Connector.Connect, AddressOf Me.Connect
            RemoveHandler Me._Connector.Disconnect, AddressOf Me.Disconnect
            RemoveHandler Me._Connector.Clear, AddressOf Me.ClearDevice
            RemoveHandler Me._Connector.FindNames, AddressOf Me.FindResourceNames
            RemoveHandler Me._Connector.PropertyChanged, AddressOf Me.ConnectorPropertyChanged
        End If
        Me._Connector = value
        If Me._Connector IsNot Nothing Then
            AddHandler Me._Connector.Connect, AddressOf Me.Connect
            AddHandler Me._Connector.Disconnect, AddressOf Me.Disconnect
            AddHandler Me._Connector.Clear, AddressOf Me.ClearDevice
            AddHandler Me._Connector.FindNames, AddressOf Me.FindResourceNames
            AddHandler Me._Connector.PropertyChanged, AddressOf Me.ConnectorPropertyChanged
        End If
    End Sub

    ''' <summary> Connects. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Connect(ByVal sender As Object, ByVal e As CancelEventArgs)
        Dim args As New CancelDetailsEventArgs
        Me.TryOpenSession(args)
        e.Cancel = args.Cancel
        If e.Cancel Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              $"Failed opening {Me.Connector.SelectedResourceName};. details: {args.Details}")
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"connected;. {Me.Connector.SelectedResourceName}")
        End If
    End Sub

    ''' <summary> Disconnects this object. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Private Sub Disconnect(ByVal sender As Object, ByVal e As CancelEventArgs)
        Dim args As New CancelDetailsEventArgs
        Me.TryCloseSession(args)
        e.Cancel = args.Cancel
        If e.Cancel Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              $"Failed closing session to resource; {Me.Connector.SelectedResourceName}; details: {args.Details}")
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Disconnected;. {Me.Connector.SelectedResourceName}")
        End If
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    Protected Overridable Sub ClearDevice()
        Me.DeviceBase.ResetClearInit()
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ClearDevice(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim action As String = "Resetting, clearing and initializing resource to know state"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.ClearDevice), New Object() {sender, e})
            Else
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{action};. {Me.Connector.SelectedResourceName}")
                Me.ClearDevice()
                action = "Resource reset, cleared and initialized"
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{action};. {Me.Connector.SelectedResourceName}")
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {action};. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Displays the resource names based on the device resource search pattern. </summary>
    Public Overridable Sub DisplayResourceNames()
        ' get the list of available resources
        If Me.DeviceBase IsNot Nothing AndAlso Me.DeviceBase.ResourcesFilter IsNot Nothing Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Using resource filter {Me.DeviceBase.ResourcesFilter}")
            Me.Connector.ResourcesFilter = Me.DeviceBase.ResourcesFilter
        ElseIf String.IsNullOrWhiteSpace(Me.Connector.ResourcesFilter) Then
            Me.Connector.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter
        End If
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Displaying {Me.Connector.ResourcesFilter} resources")
        Me.Connector.DisplayResourceNames()
    End Sub

    ''' <summary> Gets a list of names of the has resources. </summary>
    ''' <value> A list of names of the has resources. </value>
    Public ReadOnly Property HasResourceNames As Boolean
        Get
            Return Me.Connector.HasResources
        End Get
    End Property

    ''' <summary> Gets the selected resource exists. </summary>
    ''' <value> The selected resource exists. </value>
    Public ReadOnly Property SelectedResourceExists As Boolean
        Get
            Return Me.Connector.SelectedResourceExists
        End Get
    End Property

    ''' <summary> Displays available instrument names. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub FindResourceNames(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim action As String = "Finding resource names"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.FindResourceNames), New Object() {sender, e})
            Else
                Me.DisplayResourceNames()
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {action};. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Sub OnConnectorPropertyChanged(ByVal sender As ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.EnteredResourceName)
                Me.SafePostPropertyChanged(NameOf(Me.EnteredResourceName))
            Case NameOf(sender.SelectedResourceName)
                If String.IsNullOrWhiteSpace(sender.SelectedResourceName) OrElse String.Equals(sender.SelectedResourceName, VI.DeviceBase.ResourceNameClosed) Then
                    Me.ResourceName = ""
                Else
                    Me.ResourceName = sender.SelectedResourceName
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{Me.ResourceName} selected;. ")
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _ResourceNameSelectorConnector for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ConnectorPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        ' the connector keeps sending events even after it is disposed and set to nothing!
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.ConnectorPropertyChanged), New Object() {sender, e})
            Else
                Me.OnConnectorPropertyChanged(TryCast(sender, ResourceSelectorConnector), e.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling Connector.{e.PropertyName} change Event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " STATUS "

    Private _StatusSubsystem As StatusSubsystemBase

    ''' <summary> Gets or sets the status subsystem. </summary>
    ''' <value> The status subsystem. </value>
    Protected Property StatusSubsystem As StatusSubsystemBase
        Get
            Return Me._StatusSubsystem
        End Get
        Set(value As StatusSubsystemBase)
            If Me._StatusSubsystem IsNot Nothing Then
                RemoveHandler Me._StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
            End If
            Me._StatusSubsystem = value
            If Me._StatusSubsystem IsNot Nothing Then
                AddHandler Me._StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
            End If
        End Set
    End Property

    ''' <summary> Reports the last error. </summary>
    Protected Overridable Sub OnLastError(ByVal lastError As DeviceError)
    End Sub

    ''' <summary> Executes the subsystem property changed action. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Protected Overridable Sub OnPropertyChanged(ByVal subsystem As StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.ErrorAvailable)
                If Not subsystem.ReadingDeviceErrors AndAlso subsystem.ErrorAvailable Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Error available;. ")
                    subsystem.QueryDeviceErrors()
                End If
            Case NameOf(subsystem.MessageAvailable)
                If subsystem.MessageAvailable Then
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Message available;. ")
                End If
            Case NameOf(subsystem.MeasurementAvailable)
                If subsystem.MeasurementAvailable Then
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measurement available;. ")
                End If
            Case NameOf(subsystem.ReadingDeviceErrors)
                If subsystem.ReadingDeviceErrors Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Reading device errors;. ")
                End If

            Case NameOf(subsystem.DeviceErrors)
                Me.OnLastError(subsystem.LastDeviceError)

            Case NameOf(subsystem.LastDeviceError)
                OnLastError(subsystem.LastDeviceError)

            Case NameOf(subsystem.ServiceRequestStatus)
                Me.StatusRegisterStatus = subsystem.ServiceRequestStatus

            Case NameOf(subsystem.StandardEventStatus)
                Me.StandardRegisterStatus = subsystem.StandardEventStatus

        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If e Is Nothing Then Return
        Dim action As String = $"handling property {NameOf(StatusSubsystemBase)}.{e.PropertyName} changed event"
        Try
            Me.OnPropertyChanged(TryCast(sender, StatusSubsystemBase), e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {action};. { ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

    ''' <summary> Assigns talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AssignTalker(talker As ITraceMessageTalker)
        If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
        Me.Connector.AssignTalker(talker)
        MyBase.AssignTalker(talker)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        Me.Connector.ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        ' MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
' 1/23/2018
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me.IsConnectorOwner Then Me.Connector.Dispose()
                Me.Connector = Nothing
                If Me.DeviceBase?.IsDeviceOpen Then
                    Try
                        If Me.IsDeviceOwner Then
                            ' if the device owner, then close the session
                            Me.DeviceBase.CloseSession()
                        Else
                            ' if not device owner, just release the event handlers associated with this panel. 
                            Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                        End If
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                End If
                If Me.DeviceBase IsNot Nothing Then
                    Try
                        Me.ReleaseDevice()
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred releasing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                End If
                Me._ElapsedTimeStopwatch = Nothing
#If False Then
                If Me.DeviceBase IsNot Nothing Then
                    Try
                        ' this is required to release the device event handlers associated with this panel. 
                        If Me.DeviceBase IsNot Nothing Then Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                    Try
                        ' this also releases the device event handlers associated with this panel. 
                        Me.DeviceBase = Nothing
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred releasing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                End If
                Me._ElapsedTimeStopwatch = Nothing
                ' unable to use null conditional because it is not seen by code analysis
                ' If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
#End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End If
#End Region