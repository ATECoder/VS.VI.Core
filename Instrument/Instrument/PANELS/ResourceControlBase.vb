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
    Protected Sub New(ByVal device As VI.DeviceBase, ByVal isDeviceOwner As Boolean)
        Me.New(device, isDeviceOwner, ResourceSelectorConnector.Create, True)
    End Sub

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    ''' <param name="connector"> The connector. </param>
    Protected Sub New(ByVal device As VI.DeviceBase, ByVal isDeviceOwner As Boolean, ByVal connector As ResourceSelectorConnector, ByVal isConnectorOwner As Boolean)
        MyBase.New()
        Me.InitializingComponents = True
        Me.InitializeComponent()
        Me.InitializingComponents = False
        Me.AssignConnector(connector, isConnectorOwner)
        Me.AssignDevice(device, isDeviceOwner)
        Me.Initialize()
    End Sub

    Private Sub Initialize()
        Me._ElapsedTimeStopwatch = New Stopwatch
        Me._DefaultOpenResourceTitleFormat = VI.Pith.My.MySettings.Default.DefaultOpenResourceTitleFormat
        Me._OpenResourceTitleFormat = Me._DefaultOpenResourceTitleFormat
        Me._DefaultClosedResourceTitleFormat = VI.Pith.My.MySettings.Default.DefaultClosedResourceTitleFormat
        Me._ClosedResourceTitleFormat = Me.DefaultClosedResourceTitleFormat
        Me._DefaultResourceTitle = "<closed>"
        Me._DefaultResourceTitle = VI.Pith.My.MySettings.Default.DefaultResourceTitle
    End Sub

    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(ResourceSelectorConnector.Create, True)
    End Sub

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    ''' <param name="connector"> The connector. </param>
    Protected Sub New(ByVal connector As ResourceSelectorConnector, ByVal isConnectorOwner As Boolean)
        MyBase.New()
        Me.InitializeComponent()
        Me.AssignConnector(connector, isConnectorOwner)
        Me.Initialize()
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
                Me.InitializingComponents = True
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
                Me.AssignConnector(Nothing, True)
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
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
    Protected Sub DisplayStatusRegisterStatus(ByVal value As Integer, ByVal format As String)
        If Not value.Equals(Me._StatusRegisterStatus) Then
            Me._StatusRegisterStatus = value
            Me.StatusRegisterCaption = String.Format(format, value)
        End If
    End Sub

    ''' <summary> The unknown register value. </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Shared Property UnknownRegisterValue As Integer = -1

    ''' <summary> The unknown register value caption. </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Shared Property UnknownRegisterValueCaption As String = "0x.."

    ''' <summary> The default register value format. </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Shared Property DefaultRegisterValueFormat As String = "0x{0:X2}"

    ''' <summary> Displays the status register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Protected Overridable Sub DisplayStatusRegisterStatus(ByVal value As Integer)
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
    Protected Sub DisplayStandardRegisterStatus(ByVal value As Integer, ByVal format As String)
        If Not value.Equals(Me._StandardRegisterStatus) Then
            Me._StandardRegisterStatus = value
            Me.StandardRegisterCaption = String.Format(format, value)
        End If
    End Sub

    ''' <summary> Displays the status register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Protected Overridable Sub DisplayStandardRegisterStatus(ByVal value As Integer?)
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

    ''' <summary> The unknown register value caption. </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Shared Property UnknownRegisterBitmaskCaption As String = "0b.."

    ''' <summary> Gets or sets the standard register caption. </summary>
    ''' <value> The standard register caption. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overridable Property ServiceRequestEnableBitmaskCaption As String

    Private _ServiceRequestEnableBitmask As Integer
    ''' <summary> Gets or sets the standard register status. </summary>
    ''' <value> The standard register status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overridable Property ServiceRequestEnableBitmask As Integer
        Get
            Return Me._ServiceRequestEnableBitmask
        End Get
        Set(value As Integer)
            If value <> Me.ServiceRequestEnableBitmask Then
                Me._ServiceRequestEnableBitmask = value
                Me.DisplayServiceRequestEnableBitmask(value)
            End If
        End Set
    End Property

    ''' <summary> Displays the status register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Protected Overridable Sub DisplayServiceRequestEnableBitmask(ByVal value As Integer)
        If value = ResourceControlBase.UnknownRegisterValue Then
            Me.ServiceRequestEnableBitmaskCaption = ResourceControlBase.UnknownRegisterBitmaskCaption
        Else
            Me.ServiceRequestEnableBitmaskCaption = $"0b{Convert.ToString(value, 2),8}".Replace(" ", "0")
        End If
    End Sub

#End Region

#Region " ANNUNCIATE - OBJECT "

    ''' <summary> Annunciates error. </summary>
    ''' <param name="sender">  The event sender. </param>
    ''' <param name="details"> The details. </param>
    ''' <returns> A String. </returns>
    Protected Overridable Function Annunciate(ByVal sender As Object, ByVal level As InfoProviderLevel, ByVal details As String) As String
        Return details
    End Function

    ''' <summary> Annunciates error. </summary>
    ''' <param name="sender"> The event sender. </param>
    ''' <param name="format"> Describes the format to use. </param>
    ''' <param name="args">   A variable-length parameters list containing arguments. </param>
    ''' <returns> A String. </returns>
    Protected Function Annunciate(ByVal sender As Object, ByVal level As InfoProviderLevel, ByVal format As String, ByVal ParamArray args() As Object) As String
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

    ''' <summary> Attempts to select resource name from the given data. </summary>
    ''' <param name="resourceName"> The name of the resource. </param>
    ''' <param name="e">            Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Function TrySelectResourceName(ByVal resourceName As String, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"trying to select resource '{resourceName}'"
        Try
            If String.IsNullOrWhiteSpace(resourceName) Then
                e.RegisterCancellation($"{activity} canceled because name is empty")
            ElseIf Me.Connector Is Nothing Then
                e.RegisterCancellation($"{activity} canceled because the connector was disposed")
            Else
                activity = $"displaying resource names"
                Me.SessionFactory.EnumerateResources()
                activity = $"trying to select resource '{resourceName}'"
                If Me.SessionFactory.TrySelectResource(resourceName, e) Then
                End If
            End If
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
        End Try
        Return Not e.Cancel
    End Function

    Private _DefaultOpenResourceTitleFormat As String
    ''' <summary> Gets or sets the default Open resource title format. </summary>
    ''' <value> The default Open resource title format. </value>
    <Category("Appearance"), Description("The default format of the close resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}.{1}")>
    Public Property DefaultOpenResourceTitleFormat As String
        Get
            Return Me._DefaultOpenResourceTitleFormat
        End Get
        Set(value As String)
            Me._DefaultOpenResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property

    Private _OpenResourceTitleFormat As String
    <Category("Appearance"), Description("The format of the open resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}.{1}")>
    Public Property OpenResourceTitleFormat As String
        Get
            Return Me._OpenResourceTitleFormat
        End Get
        Set(value As String)
            Me._OpenResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
            Me.OnTitleChanged(Me.BuildTitle)
        End Set
    End Property

    Private _DefaultClosedResourceTitleFormat As String
    ''' <summary> Gets or sets the default closed resource title format. </summary>
    ''' <value> The default closed resource title format. </value>
    <Category("Appearance"), Description("The default format of the close resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}")>
    Public Property DefaultClosedResourceTitleFormat As String
        Get
            Return Me._DefaultClosedResourceTitleFormat
        End Get
        Set(value As String)
            Me._DefaultClosedResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property

    Private _ClosedResourceTitleFormat As String
    ''' <summary> Gets or sets the closed resource title format. </summary>
    ''' <value> The title. </value>
    <Category("Appearance"), Description("The format of the close resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}")>
    Public Property ClosedResourceTitleFormat As String
        Get
            Return Me._ClosedResourceTitleFormat
        End Get
        Set(value As String)
            Me._ClosedResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
            Me.OnTitleChanged(Me.BuildTitle)
        End Set
    End Property

    Private _DefaultResourceTitle As String
    ''' <summary> Gets or sets the default resource title. </summary>
    ''' <value> The default resource title. </value>
    <Category("Appearance"), Description("The default resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("INSTR")>
    Public Property DefaultResourceTitle As String
        Get
            Return Me._DefaultResourceTitle
        End Get
        Set(value As String)
            Me._DefaultResourceTitle = value
            Me.SafeSendPropertyChanged()
            Me.OnTitleChanged(Me.BuildTitle)
        End Set
    End Property

    ''' <summary> Builds the title. </summary>
    ''' <returns> A String. </returns>
    Protected Function BuildTitle() As String
        Dim result As String = ""
        If Me.IsDeviceOpen Then
            If String.IsNullOrWhiteSpace(Me.OpenResourceTitleFormat) OrElse String.IsNullOrWhiteSpace(Me.DeviceBase.ResourceTitle) Then
                result = ""
            Else
                result = String.Format(Me.OpenResourceTitleFormat, Me.DeviceBase.ResourceTitle, Me.DeviceBase.Session.ResourceName)
            End If
        Else
            If String.IsNullOrWhiteSpace(Me.ClosedResourceTitleFormat) OrElse String.IsNullOrWhiteSpace(Me.DeviceBase.ResourceTitle) Then
                result = ""
            Else
                result = String.Format(Me.ClosedResourceTitleFormat, Me.DeviceBase.ResourceTitle)
            End If
        End If
        Return result
    End Function

#End Region

#Region " DEVICE "

    ''' <summary> Gets or sets the elapsed time stop watch. </summary>
    ''' <value> The elapsed time stop watch. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Private ReadOnly Property ElapsedTimeStopwatch As Stopwatch

    ''' <summary> Reads elapsed time. </summary>
    ''' <param name="stopRequested"> True if stop requested. </param>
    ''' <returns> The elapsed time. </returns>
    Protected Function ReadElapsedTime(ByVal stopRequested As Boolean) As TimeSpan
        If stopRequested AndAlso Me.ElapsedTimeStopwatch.IsRunning Then
            Me._ElapsedTimeCount -= 1
            If Me.ElapsedTimeCount <= 0 Then Me.ElapsedTimeStopwatch.Stop()
        End If
        Return Me.ElapsedTimeStopwatch.Elapsed
    End Function

    Private _ElapsedTimeCount As Integer
    ''' <summary> Gets the number of elapsed times. Some action require two cycles to get the full elapsed time. </summary>
    ''' <value> The number of elapsed times. </value>
    Public Property ElapsedTimeCount As Integer
        Get
            Return Me._ElapsedTimeCount
        End Get
        Protected Set(value As Integer)
            If value <> Me.ElapsedTimeCount Then
                Me._ElapsedTimeCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Starts elapsed stopwatch. </summary>
    Protected Sub StartElapsedStopwatch(ByVal count As Integer)
        Me._ElapsedTimeCount = count
        Me.ElapsedTimeStopwatch.Restart()
    End Sub

    ''' <summary> Gets or sets the sentinel indicating if this panel owns the device and, therefore, needs to 
    '''           dispose of this device. </summary>
    ''' <value> The is device owner. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected ReadOnly Property IsDeviceOwner As Boolean

    ''' <summary> Gets the is device assigned. </summary>
    ''' <value> The is device assigned. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overridable ReadOnly Property IsDeviceAssigned As Boolean
        Get
            Return Me.DeviceBase IsNot Nothing AndAlso Not Me.DeviceBase.IsDisposed
        End Get
    End Property

    Private _DeviceBase As VI.DeviceBase
    ''' <summary> Gets or sets reference to the VISA <see cref="VI.DeviceBase">device</see>
    ''' interfaces. </summary>
    ''' <value> The connectable resource. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected ReadOnly Property DeviceBase() As VI.DeviceBase
        Get
            Return Me._DeviceBase
        End Get
    End Property

    ''' <summary> Assigns a device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Sub AssignDevice(ByVal value As VI.DeviceBase, ByVal isDeviceOwner As Boolean)
        If Me.DeviceBase IsNot Nothing Then
            If Me.DeviceBase.Talker IsNot Nothing Then Me.DeviceBase.Talker.RemoveListeners()
            RemoveHandler Me.DeviceBase.Session.ResourceNameInfo.PropertyChanged, AddressOf Me.ResourceNameInfoPropertyChanged
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
            If Me.IsDeviceOwner Then Me._DeviceBase.Dispose() : Me._DeviceBase = Nothing
        End If
        If Me.Connector IsNot Nothing Then
            Me.SessionFactory.ResourcesFilter = ""
            Me.Connector.Clearable = True
            Me.Connector.Connectable = True
            Me.Connector.Searchable = True
        End If
        Me._StatusRegisterStatus = ResourceControlBase.UnknownRegisterValue
        Me._StandardRegisterStatus = ResourceControlBase.UnknownRegisterValue
        Me._DeviceBase = value
        If value IsNot Nothing Then
            Me.DeviceBase.CaptureSyncContext(Threading.SynchronizationContext.Current)
            AddHandler Me.DeviceBase.Session.ResourceNameInfo.PropertyChanged, AddressOf Me.ResourceNameInfoPropertyChanged
            AddHandler Me.DeviceBase.PropertyChanged, AddressOf Me.DevicePropertyChanged
            AddHandler Me.DeviceBase.Opening, AddressOf Me.DeviceOpening
            AddHandler Me.DeviceBase.Opened, AddressOf Me.DeviceOpened
            AddHandler Me.DeviceBase.Closing, AddressOf Me.DeviceClosing
            AddHandler Me.DeviceBase.Closed, AddressOf Me.DeviceClosed
            AddHandler Me.DeviceBase.Initialized, AddressOf Me.DeviceInitialized
            AddHandler Me.DeviceBase.Initializing, AddressOf Me.DeviceInitializing
            Me.SessionFactory.ResourcesFilter = Me.DeviceBase.Session.ResourceNameInfo.ResourcesFilter
            Me.Connector.IsConnected = value.IsDeviceOpen
            Me.StatusSubsystem = value.StatusSubsystemBase
            ' publish device state.  This should also report the open status and set the 
            ' control open/close button state.
            Me.DeviceBase.Publish()
            Me.AssignTalker(value.Talker)
            Me.ApplyListenerTraceLevel(ListenerType.Display, value.Talker.TraceShowLevel)
        End If
        Me._IsDeviceOwner = isDeviceOwner
    End Sub

    ''' <summary> Gets the sentinel indicating if the device is open. </summary>
    ''' <value> The is device open. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected ReadOnly Property IsDeviceOpen As Boolean
        Get
            Return Me.DeviceBase IsNot Nothing AndAlso Me.DeviceBase.IsDeviceOpen
        End Get
    End Property

    ''' <summary> Releases the device. </summary>
    Public Overridable Sub ReleaseDevice()
        ' this also releases the device event handlers associated with this panel. 
        Me.AssignDevice(Nothing, False)
    End Sub

    ''' <summary> Releases the device and reassigns the default device. </summary>
    Public Overridable Sub RestoreDevice()
        ' release the device
        Me.ReleaseDevice()
        ' the parent will assign the device.
    End Sub

#End Region

#Region " DEVICE: SERVICE REQUESTED "

    ''' <summary> Event handler. Called when the device is requesting service. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Message based session event information. </param>
    Protected Overridable Sub DeviceServiceRequested(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    ''' <summary> Gets the sentinel indicating if service requests handler was added for this panel. </summary>
    ''' <value> The service request event handler add sentinel. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected ReadOnly Property PanelServiceRequestHandlerAdded As Boolean

    ''' <summary> Gets or sets the device service request enabled. </summary>
    ''' <value> The device service request enabled. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected ReadOnly Property PanelDeviceServiceRequestHandlerAdded As Boolean

    ''' <summary> Adds a panel service request handler. </summary>
    ''' <remarks>
    ''' The service request for the session must be enabled for handling service requests on the device. 
    ''' The panel can add the service request handler if: (1) is owner or; (2) if not owner but service request handler was not
    ''' added on the device. In case 2, the panel could also remove the device handler.
    ''' </remarks>
    Protected Overridable Sub AddServiceRequestEventHandler()
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
    Protected Overridable Sub RemoveServiceRequestEventHandler()
        If Me.PanelServiceRequestHandlerAdded Then
            RemoveHandler Me.DeviceBase.ServiceRequested, AddressOf Me.DeviceServiceRequested
            If Me.IsDeviceOwner OrElse Me._PanelDeviceServiceRequestHandlerAdded Then Me.DeviceBase.RemoveServiceRequestEventHandler()
            Me._PanelServiceRequestHandlerAdded = False
            Me._PanelDeviceServiceRequestHandlerAdded = False
        End If
    End Sub

#End Region

#Region " DEVICE: PROPERTY CHANGE "

    ''' <summary> Recursively enable. </summary>
    ''' <param name="controls"> The controls. </param>
    ''' <param name="value">    The value. </param>
    Protected Sub RecursivelyEnable(ByVal controls As System.Windows.Forms.Control.ControlCollection, ByVal value As Boolean)
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
    Protected Overridable Sub OnDeviceOpenChanged(ByVal device As VI.DeviceBase)
        If Me.Connector IsNot Nothing Then Me.Connector.IsConnected = Me.IsDeviceOpen
    End Sub

    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="resourceNameInfo"> The resource name info. </param>
    ''' <param name="propertyName">     Name of the property. </param>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal resourceNameInfo As VI.Pith.ResourceNameInfo, ByVal propertyName As String)
        If resourceNameInfo Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.Pith.ResourceNameInfo.ResourcesFilter)
                If Connector IsNot Nothing Then Me.SessionFactory.ResourcesFilter = resourceNameInfo.ResourcesFilter
            Case NameOf(VI.Pith.ResourceNameInfo.ResourceTitle)
                Me.OnTitleChanged(Me.BuildTitle)
            Case NameOf(VI.Pith.ResourceNameInfo.ResourceName)
                Me.OnTitleChanged(Me.BuildTitle)
        End Select
    End Sub

    ''' <summary> Event handler. Called for property changed events. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ResourceNameInfoPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.Pith.ResourceNameInfo)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.DevicePropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, VI.Pith.ResourceNameInfo), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub


    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal device As VI.DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(isr.VI.DeviceBase.IsDeviceOpen)
                ' the open sentinel is turned on after all subsystems are set
                Me.OnDeviceOpenChanged(device)
            Case NameOf(isr.VI.DeviceBase.ResourceTitle)
                Me.OnTitleChanged(Me.BuildTitle())
            Case NameOf(isr.VI.DeviceBase.ServiceRequestEnableBitmask)
                Me.ServiceRequestEnableBitmask = device.ServiceRequestEnableBitmask
        End Select
        ' propagate the property change messages
        Me.SafeSendPropertyChanged(propertyName)
    End Sub

    ''' <summary> Event handler. Called for property changed events. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DevicePropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.DeviceBase)}.{e?.PropertyName} change event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.DevicePropertyChanged), New Object() {sender, e})
            ElseIf Not Me.InitializingComponents AndAlso sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.HandlePropertyChange(TryCast(sender, VI.DeviceBase), e.PropertyName)
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

#Region " DEVICE: OPENING / OPEN ACTIONS AND EVENTS "

    ''' <summary> Attempts to open a session to the device using the specified resource name. </summary>
    ''' <param name="resourceName">  The name of the resource. </param>
    ''' <param name="resourceTitle"> The title. </param>
    ''' <param name="e">             Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:   DoNotCatchGeneralExceptionTypes")>
    Protected Function TryOpenSession(ByVal resourceName As String, ByVal resourceTitle As String, ByVal e As ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"[{resourceTitle}] trying to open {resourceName} VISA session"
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of String, String, ActionEventArgs)(AddressOf Me.TryOpenSession), New Object() {resourceName, resourceTitle, e})
        Else
            Try
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{activity};. ")
                If Me.DeviceBase.TryOpenSession(resourceName, resourceTitle, e) Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Done {activity};. ")
                Else
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Failed {activity};. ")
                End If
            Catch ex As Exception
                e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
            Finally
                Me.Connector.IsConnected = Me.IsDeviceOpen
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            End Try
        End If
        Return Not e.Cancel
    End Function

    ''' <summary>
    ''' Event handler. Called upon device opening so as to instantiated all subsystems.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Protected Overridable Sub DeviceOpening(ByVal sender As VI.DeviceBase, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me._StatusRegisterStatus = ResourceControlBase.UnknownRegisterValue
        Me._StandardRegisterStatus = ResourceControlBase.UnknownRegisterValue
    End Sub

    ''' <summary> Event handler. Called upon device opening so as to instantiated all subsystems. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceOpening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling  device opening event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, CancelEventArgs)(AddressOf Me.DeviceOpening), New Object() {sender, e})
            Else
                Me.DeviceOpening(TryCast(sender, DeviceBase), e)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Event handler. Called after the device opened and all subsystems were defined.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Protected Overridable Sub DeviceOpened(ByVal sender As VI.DeviceBase, ByVal e As System.EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
    End Sub

    ''' <summary>
    ''' Event handler. Called after the device opened and all subsystems were defined.
    ''' </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling device opened event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, EventArgs)(AddressOf Me.DeviceOpened), New Object() {sender, e})
            Else
                Me.DeviceOpened(TryCast(sender, DeviceBase), e)
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

#Region " DEVICE: INITALIZING / INITIALIZED  "

    ''' <summary> Device initializing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Protected Overridable Sub DeviceInitializing(ByVal sender As VI.DeviceBase, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
    End Sub

    ''' <summary> Device initializing. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceInitializing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling device initializing event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, CancelEventArgs)(AddressOf Me.DeviceInitializing), New Object() {sender, e})
            Else
                Me.DeviceInitializing(TryCast(sender, DeviceBase), e)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Device initialized. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Protected Overridable Sub DeviceInitialized(ByVal sender As VI.DeviceBase, ByVal e As System.EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.ReadServiceRequestStatus()
    End Sub

    ''' <summary> Device initialized. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceInitialized(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling device initialized event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, EventArgs)(AddressOf Me.DeviceInitialized), New Object() {sender, e})
            Else
                Me.DeviceInitialized(TryCast(sender, DeviceBase), e)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try

    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overridable Sub OnTitleChanged(ByVal value As String)
        Me.Title = value
    End Sub

    ''' <summary> Gets or sets the title. </summary>
    ''' <value> The title. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Protected Property Title As String

    ''' <summary> Reads a service request status. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Sub ReadServiceRequestStatus()
        Dim activity As String = $"reading {Me.DeviceBase?.ResourceNameCaption} service request"
        Try
            Me.DeviceBase.StatusSubsystemBase.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception reading service request;. {0}", ex.ToFullBlownString)
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

#End Region

#Region " DEVICE: CLOSING / CLOSED "

    ''' <summary> Attempts to close session from the given data. </summary>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:   DoNotCatchGeneralExceptionTypes")>
    Protected Function TryCloseSession(ByVal e As ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"closing VISA session {Me.DeviceBase?.ResourceNameCaption}"
        Try
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{activity};. ")
            If Me.DeviceBase.IsDeviceOpen Then
                If Me.DeviceBase.TryCloseSession() Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{activity} processed;. ")
                Else
                    e.RegisterCancellation($"Failed {activity};. ")
                End If
            End If
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Connector.IsConnected = Me.IsDeviceOpen
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    Protected Overridable Sub DeviceClosing(ByVal sender As VI.DeviceBase, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.DeviceBase Is Nothing OrElse Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.RemoveServiceRequestEventHandler()
        Me.DeviceBase.Session?.DisableServiceRequest()
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.DeviceBase Is Nothing OrElse Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling device closing event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, CancelEventArgs)(AddressOf Me.DeviceClosing), New Object() {sender, e})
            Else
                Me.DeviceClosing(TryCast(sender, DeviceBase), e)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    Protected Overridable Sub DeviceClosed(ByVal sender As VI.DeviceBase, ByVal e As System.EventArgs)
        If Me.DeviceBase Is Nothing OrElse Me.DeviceBase.Session Is Nothing OrElse Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim message As TraceMessage = Nothing
        If Me.DeviceBase Is Nothing OrElse Me.DeviceBase.Session Is Nothing Then
            message = New TraceMessage(TraceEventType.Information, My.MyLibrary.TraceEventId, "Disconnected; Device disposed;. ")
        ElseIf Me.DeviceBase.Session.IsSessionOpen Then
            message = New TraceMessage(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"{Me.DeviceBase.Session.ResourceName} closed but session still open;. ")
        ElseIf Me.DeviceBase.Session.IsDeviceOpen Then
            message = New TraceMessage(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Device closed but emulated session still open;. ")
        Else
            message = New TraceMessage(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Disconnected; Session closed;. ")
        End If
        If Me.Talker Is Nothing Then
            If message.EventType <= TraceEventType.Warning Then isr.Core.Pith.My.MyLibrary.LogUnpublishedMessage(message)
        Else
            Me.Talker.Publish(message)
        End If
    End Sub


    ''' <summary> Event handler. Called when device is closed. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    ''' <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub DeviceClosed(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim activity As String = "handling device closed event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, EventArgs)(AddressOf Me.DeviceClosed), New Object() {sender, e})
            Else
                Me.DeviceClosed(TryCast(sender, DeviceBase), e)
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

#Region " SESSION FACTORY "

    ''' <summary> Gets the session factory. </summary>
    ''' <value> The session factory. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property SessionFactory As VI.SessionFactory
        Get
            Return Me.Connector.SessionFactory
        End Get
    End Property

#End Region

#Region " CONNECTOR SELECTOR EVENT HANDLERS "

    ''' <summary> Gets or sets the is connector that owns this item. </summary>
    ''' <value> The is connector owner. </value>
    Protected Property IsConnectorOwner As Boolean

    Private _Connector As ResourceSelectorConnector

    ''' <summary> Gets or sets the connector. </summary>
    ''' <value> The connector. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected Overridable ReadOnly Property Connector As ResourceSelectorConnector
        Get
            Return Me._Connector
        End Get
    End Property

    ''' <summary> Assign connector. </summary>
    ''' <param name="value"> The connector value. </param>
    Protected Sub AssignConnector(ByVal value As ResourceSelectorConnector, ByVal isConnectorOwner As Boolean)
        If Me._Connector IsNot Nothing Then
            RemoveHandler Me._Connector.Connect, AddressOf Me.Connect
            RemoveHandler Me._Connector.Disconnect, AddressOf Me.Disconnect
            RemoveHandler Me._Connector.Clear, AddressOf Me.ClearDevice
            RemoveHandler Me._Connector.FindNames, AddressOf Me.FindResourceNames
            RemoveHandler Me._Connector.PropertyChanged, AddressOf Me.ConnectorPropertyChanged
            If Me.IsConnectorOwner Then Me._Connector.Dispose()
        End If
        Me._Connector = value
        If value IsNot Nothing Then
            AddHandler Me._Connector.Connect, AddressOf Me.Connect
            AddHandler Me._Connector.Disconnect, AddressOf Me.Disconnect
            AddHandler Me._Connector.Clear, AddressOf Me.ClearDevice
            AddHandler Me._Connector.FindNames, AddressOf Me.FindResourceNames
            AddHandler Me._Connector.PropertyChanged, AddressOf Me.ConnectorPropertyChanged
        End If
        Me.IsConnectorOwner = isConnectorOwner
    End Sub

    ''' <summary> Attempts to connect from the given data. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Private Function TryConnect(ByVal sender As ResourceSelectorConnector, ByVal e As ActionEventArgs) As Boolean
        Dim activity As String = "handling device opened event"
        If String.IsNullOrWhiteSpace(Me.SessionFactory.SelectedResourceName) Then
            e.RegisterCancellation($"Failed {activity}; selected resource name is empty;. ")
        Else
            activity = $"trying to connect '{sender.SessionFactory.SelectedResourceName}' {If(String.IsNullOrWhiteSpace(Me.DeviceBase.ResourceTitle), "unknown", Me.DeviceBase.ResourceTitle)}"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{activity};. ")
            Me.TryOpenSession(sender.SessionFactory.SelectedResourceName, Me.DeviceBase.ResourceTitle, e)
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> Connects. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Connect(ByVal sender As Object, ByVal e As CancelEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling connector connect event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, CancelEventArgs)(AddressOf Me.Connect), New Object() {sender, e})
            Else
                Dim args As New ActionEventArgs
                If Not Me.TryConnect(TryCast(sender, ResourceSelectorConnector), args) Then
                    Dim message As TraceMessage = New TraceMessage(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Failed {activity};. {args.Details}")
                    If Me.Talker Is Nothing Then
                        If message.EventType <= TraceEventType.Warning Then
                            isr.Core.Pith.My.MyLibrary.LogUnpublishedMessage(message)
                        End If
                    Else
                        Me.Talker.Publish(message)
                    End If
                End If
                e.Cancel = args.Cancel
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Disconnects this object. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Cancel event information. </param>
    Protected Overridable Sub Disconnect(ByVal sender As ResourceSelectorConnector, ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling connector disconnect event"
        Dim args As New ActionEventArgs
        If Not Me.TryCloseSession(args) Then
            Dim message As TraceMessage = New TraceMessage(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Failed {activity};. {args.Details}")
            If Me.Talker Is Nothing Then
                If message.EventType <= TraceEventType.Warning Then
                    isr.Core.Pith.My.MyLibrary.LogUnpublishedMessage(message)
                End If
            Else
                Me.Talker.Publish(message)
            End If
        End If
        e.Cancel = args.Cancel
    End Sub

    ''' <summary> Disconnects this object. </summary>
    ''' <param name="sender"> <see cref="Object"/> instance of this
    '''                       <see cref="Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Disconnect(ByVal sender As Object, ByVal e As CancelEventArgs)
        Dim activity As String = "handling connector disconnect event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, CancelEventArgs)(AddressOf Me.Disconnect), New Object() {sender, e})
            Else
                Me.Disconnect(TryCast(sender, ResourceSelectorConnector), e)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Cancel event information. </param>
    Protected Overridable Sub ClearDevice(ByVal sender As ResourceSelectorConnector, ByVal e As System.EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Me.DeviceBase.ResetClearInit()
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ClearDevice(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "handling connector clear event"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.EventArgs)(AddressOf Me.ClearDevice), New Object() {sender, e})
            Else
                Me.ClearDevice(TryCast(sender, ResourceSelectorConnector), e)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Gets a list of names of the has resources. </summary>
    ''' <value> A list of names of the has resources. </value>
    Protected ReadOnly Property HasResourceNames As Boolean
        Get
            Return Me.SessionFactory.HasResources
        End Get
    End Property

    ''' <summary> Gets the connector is connected. </summary>
    ''' <value> The connector is connected. </value>
    Public ReadOnly Property IsConnected As Boolean
        Get
            Return Me.Connector.IsConnected
        End Get
    End Property

    ''' <summary> Gets the selected resource exists. </summary>
    ''' <value> The selected resource exists. </value>
    Protected ReadOnly Property SelectedResourceExists As Boolean
        Get
            Return Me.SessionFactory.SelectedResourceExists
        End Get
    End Property

    Protected Overridable Sub FindResourceNames(ByVal sender As ResourceSelectorConnector, ByVal e As System.EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse sender.SessionFactory Is Nothing OrElse e Is Nothing Then Return
        sender.SessionFactory.ResourcesFilter = Me.DeviceBase.Session.ResourceNameInfo.ResourcesFilter
        Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Displaying {Me.SessionFactory.ResourcesFilter} resources")
        sender.SessionFactory.EnumerateResources()
    End Sub

    ''' <summary> Displays available instrument names. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub FindResourceNames(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = "Finding resource names"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, EventArgs)(AddressOf Me.FindResourceNames), New Object() {sender, e})
            Else
                Me.FindResourceNames(TryCast(sender, ResourceSelectorConnector), e)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Handles the property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal sender As ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SessionFactory.SelectedResourceName)
                If sender.SessionFactory.SelectedResourceName IsNot Nothing Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                      $"Resource {If(sender.IsConnected, "connected", "selected")};. {sender.SessionFactory.SelectedResourceName}")
                End If
        End Select
        Me.SafePostPropertyChanged(propertyName)
    End Sub

    ''' <summary> Event handler. Called by _ResourceNameSelectorConnector for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ConnectorPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        ' the connector keeps sending events even after it is disposed and set to nothing!
        If Me.IsDisposed OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(ResourceSelectorConnector)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.ConnectorPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, ResourceSelectorConnector), e.PropertyName)
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

#Region " SUBSYSTEMS "

#Region " STATUS "

    Private _StatusSubsystem As StatusSubsystemBase

    ''' <summary> Gets or sets the status subsystem. </summary>
    ''' <value> The status subsystem. </value>
    Protected Property StatusSubsystem As StatusSubsystemBase
        Get
            Return Me._StatusSubsystem
        End Get
        Set(value As VI.StatusSubsystemBase)
            If Me._StatusSubsystem IsNot Nothing Then
                RemoveHandler Me._StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
            End If
            Me._StatusSubsystem = value
            If value IsNot Nothing Then
                AddHandler Me._StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
                Me.Identity = value.Identity
            End If
        End Set
    End Property

    ''' <summary> Reports the last error. </summary>
    Protected Overridable Sub OnLastError(ByVal lastError As DeviceError)
    End Sub

    ''' <summary> Handles the subsystem property change. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If Me.InitializingComponents OrElse subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Dim message As TraceMessage = Nothing
        Select Case propertyName
            Case NameOf(StatusSubsystemBase.ErrorAvailable)
            Case NameOf(StatusSubsystemBase.MessageAvailable)
                If subsystem.MessageAvailable Then
                    message = New TraceMessage(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Message available;. ")
                End If
            Case NameOf(StatusSubsystemBase.MeasurementAvailable)
                If subsystem.MeasurementAvailable Then
                    message = New TraceMessage(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measurement available;. ")
                End If
            Case NameOf(StatusSubsystemBase.ReadingDeviceErrors)
                If subsystem.ReadingDeviceErrors Then
                    message = New TraceMessage(TraceEventType.Information, My.MyLibrary.TraceEventId, "Reading device errors;. ")
                End If
            Case NameOf(StatusSubsystemBase.DeviceErrorsReport)
                Me.OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.LastDeviceError)
                OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.ServiceRequestStatus)
                Me.StatusRegisterStatus = subsystem.ServiceRequestStatus
            Case NameOf(StatusSubsystemBase.StandardEventStatus)
                Me.StandardRegisterStatus = subsystem.StandardEventStatus
        End Select
        If message IsNot Nothing Then Me.Talker?.Publish(message)
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(StatusSubsystemBase)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.StatusSubsystemPropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, StatusSubsystemBase), e.PropertyName)
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

#Region " UNIT TESTS INTERNALS "

    ''' <summary> Gets the internal device. </summary>
    ''' <value> The internal device. </value>
    Friend ReadOnly Property InternalDevice As VI.DeviceBase
        Get
            Return Me.DeviceBase
        End Get
    End Property

    ''' <summary> Gets the internal is device that owns this item. </summary>
    ''' <value> The internal is device owner. </value>
    Friend ReadOnly Property InternalIsDeviceOwner As Boolean
        Get
            Return Me.IsDeviceOwner
        End Get
    End Property

    ''' <summary> Gets the number of internal resource names. </summary>
    ''' <value> The number of internal resource names. </value>
    Friend ReadOnly Property InternalResourceNamesCount As Integer
        Get
            Return Me.Connector.InternalResourceNamesCount
        End Get
    End Property

    ''' <summary> Gets the name of the internal selected resource. </summary>
    ''' <value> The name of the internal selected resource. </value>
    Friend ReadOnly Property InternalSelectedResourceName As String
        Get
            Return Me.Connector.InternalSelectedResourceName
        End Get
    End Property

    ''' <summary> Creates a new ResourceControlBase. </summary>
    ''' <param name="device">        The device. </param>
    ''' <param name="isDeviceOwner"> The is device owner. </param>
    ''' <returns> A ResourceControlBase. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Friend Shared Function Create(ByVal device As VI.DeviceBase, ByVal isDeviceOwner As Boolean) As ResourceControlBase
        Dim result As ResourceControlBase = Nothing
        Try
            result = New ResourceControlBase(device, isDeviceOwner)
        Catch ex As Exception
            If result IsNot Nothing Then
                result.Dispose()
                result = Nothing
            End If
        End Try
        Return result
    End Function

#End Region

End Class

#Region " UNUSED "
#If False Then

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
            Me.SafeSendPropertyChanged()
            Me.OnTitleChanged(Me.BuildTitle)
        End Set
    End Property


    ''' <summary>
    ''' 
    ''' 
    ''' Opens a session to the device using the specified resource name.
    ''' </summary>
    ''' <param name="resourceName">  The name of the resource. </param>
    ''' <param name="resourceTitle"> The title. </param>
    Public Sub OpenSession(ByVal resourceName As String, ByVal resourceTitle As String)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of String, String)(AddressOf Me.OpenSession), New Object() {resourceName, resourceTitle})
        Else
            Dim activity As String = $"opening VISA session @{resourceName}"
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{activity};. ")
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            Me.DeviceBase.OpenSession(resourceName, resourceTitle)
            If Me.DeviceBase.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Done {activity};. ")
            Else
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Failed {activity};. ")
            End If
            Me.Connector.IsConnected = Me.IsDeviceOpen
        End If
    End Sub

    ''' <summary> Gets or sets the name of the entered resource. </summary>
    ''' <value> The name of the entered resource. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected ReadOnly Property EnteredResourceName As String
        Get
            Return Me.SessionFactory.EnteredResourceName
        End Get
    End Property

    ''' <summary> Gets or sets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property ResourceName As String

#Region " RESOURCE NAME "

    ''' <summary> Attempts to select resource name from the given data. </summary>
    ''' <param name="resourceName"> The name of the resource. </param>
    ''' <param name="e">            Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TrySelectResourceName(ByVal resourceName As String, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"trying to select resource '{resourceName}'"
        Try
            If Not String.Equals(resourceName, Me.ResourceName, StringComparison.OrdinalIgnoreCase) Then
                Me._ResourceName = resourceName
                Me.SafeSendPropertyChanged(NameOf(ResourcePanelBase.ResourceName))
                Dim titleCandidate As String = Me.BuildTitle
                If Not String.IsNullOrEmpty(titleCandidate) Then
                    Me.OnTitleChanged(Me.BuildTitle)
                    Me.SafeSendPropertyChanged(NameOf(ResourcePanelBase.ResourceTitle))
                End If
            End If
            If String.IsNullOrWhiteSpace(resourceName) Then
                e.RegisterCancellation($"{activity} canceled because name is empty")
            ElseIf Me.Connector Is Nothing Then
                e.RegisterCancellation($"{activity} canceled because the connector was disposed")
            Else
                activity = $"displaying resource names"
                Me.SessionFactory.EnumerateResources()
                activity = $"trying to select resource '{resourceName}'"
                If Me.SessionFactory.TrySelectResource(resourceName, e) Then
                End If
            End If
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Gets or sets the selected resource name. </summary>
    ''' <value> The name of the selected resource. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property SelectedResourceName As String
        Get
            Return Me.SessionFactory.SelectedResourceName
        End Get
        Set(ByVal value As String)
            If Not String.Equals(value, Me.SelectedResourceName, StringComparison.OrdinalIgnoreCase) Then
                If Me.SessionFactory.TrySelectResource(value, New isr.Core.Pith.ActionEventArgs) Then
                    Me.SafeSendPropertyChanged()
                End If
            End If
        End Set
    End Property

    Private _DefaultOpenResourceTitleFormat As String
    ''' <summary> Gets or sets the default Open resource title format. </summary>
    ''' <value> The default Open resource title format. </value>
    <Category("Appearance"), Description("The default format of the close resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}.{1}")>
    Public Property DefaultOpenResourceTitleFormat As String
        Get
            Return Me._DefaultOpenResourceTitleFormat
        End Get
        Set(value As String)
            Me._DefaultOpenResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property

    Private _OpenResourceTitleFormat As String
    <Category("Appearance"), Description("The format of the open resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}.{1}")>
    Public Property OpenResourceTitleFormat As String
        Get
            Return Me._OpenResourceTitleFormat
        End Get
        Set(value As String)
            Me._OpenResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
            Me.OnTitleChanged(Me.BuildTitle)
        End Set
    End Property

    Private _DefaultClosedResourceTitleFormat As String
    ''' <summary> Gets or sets the default closed resource title format. </summary>
    ''' <value> The default closed resource title format. </value>
    <Category("Appearance"), Description("The default format of the close resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}")>
    Public Property DefaultClosedResourceTitleFormat As String
        Get
            Return Me._DefaultClosedResourceTitleFormat
        End Get
        Set(value As String)
            Me._DefaultClosedResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property

    Private _ClosedResourceTitleFormat As String
    ''' <summary> Gets or sets the closed resource title format. </summary>
    ''' <value> The title. </value>
    <Category("Appearance"), Description("The format of the close resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}")>
    Public Property ClosedResourceTitleFormat As String
        Get
            Return Me._ClosedResourceTitleFormat
        End Get
        Set(value As String)
            Me._ClosedResourceTitleFormat = value
            Me.SafeSendPropertyChanged()
            Me.OnTitleChanged(Me.BuildTitle)
        End Set
    End Property

    Private _DefaultResourceTitle As String
    ''' <summary> Gets or sets the default resource title. </summary>
    ''' <value> The default resource title. </value>
    <Category("Appearance"), Description("The default resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("INSTR")>
    Public Property DefaultResourceTitle As String
        Get
            Return Me._DefaultResourceTitle
        End Get
        Set(value As String)
            Me._DefaultResourceTitle = value
            Me.SafeSendPropertyChanged()
            Me.OnTitleChanged(Me.BuildTitle)
        End Set
    End Property

    ''' <summary> Gets the resource title. </summary>
    ''' <value> The resource title. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property ResourceTitle As String

    ''' <summary> Builds the title. </summary>
    ''' <returns> A String. </returns>
    Protected Function BuildTitle() As String
        Dim result As String = ""
        If Me.IsDeviceOpen Then
            If String.IsNullOrWhiteSpace(Me.OpenResourceTitleFormat) OrElse String.IsNullOrWhiteSpace(Me.DeviceBase.ResourceTitle) Then
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
        Return result
    End Function

    ''' <summary> Gets the resource title. </summary>
    ''' <value> The resource title. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property ResourceTitle As String


#End Region

#End If
#End Region