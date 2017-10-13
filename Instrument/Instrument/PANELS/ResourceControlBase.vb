Imports isr.VI.Instrument
Imports isr.Core.Controls
Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.ExceptionExtensions
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
    Public Sub New()
        Me.New(New ResourceSelectorConnector)
    End Sub

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    ''' <param name="connector"> The connector. </param>
    Public Sub New(ByVal connector As ResourceSelectorConnector)
        MyBase.New()
        Me.InitializeComponent()
        Me.AssignConnector(connector)
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
                If Me.Device IsNot Nothing Then
                    Try
                        ' this is required to release the device event handlers associated with this panel. 
                        If Me.Device IsNot Nothing Then Me.DeviceClosing(Me, New System.ComponentModel.CancelEventArgs)
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred closing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                    Try
                        ' this also releases the device event handlers associated with this panel. 
                        Me.ReleaseDevice()
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred releasing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                    Try
                        If Me.IsDeviceOwner Then
                            ' this also closes the session. 
                            Me._Device.Dispose()
                        End If
                        Me._Device = Nothing
                    Catch ex As Exception
                        Debug.Assert(Not Debugger.IsAttached, "Exception occurred disposing the device", $"Exception {ex.ToFullBlownString}")
                    End Try
                End If
                Me._ElapsedTimeStopwatch = Nothing
                ' unable to use null conditional because it is not seen by code analysis
                ' If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing

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
            End If
            Me.Connector.SelectedResourceName = value
        End Set
    End Property

    ''' <summary> Gets or sets the title. </summary>
    ''' <value> The title. </value>
    <Category("Appearance"), Description("The format of the open resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}.{1}")>
    Public Property OpenResourceTitleFormat As String

    ''' <summary> Gets or sets the title. </summary>
    ''' <value> The title. </value>
    <Category("Appearance"), Description("The format of the close resource title"),
            Browsable(True),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            DefaultValue("{0}")>
    Public Property ClosedResourceTitleFormat As String

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
        End Set
    End Property

    ''' <summary> Builds the title. </summary>
    ''' <returns> A String. </returns>
    Protected Function BuildTitle() As String
        If Me.IsDeviceOpen Then
            If String.IsNullOrWhiteSpace(Me.OpenResourceTitleFormat) OrElse String.IsNullOrWhiteSpace(Me.ResourceTitle) Then
                Return ""
            Else
                Return String.Format(Me.OpenResourceTitleFormat, Me.ResourceTitle, Me.ResourceName)
            End If
        Else
            If String.IsNullOrWhiteSpace(Me.ClosedResourceTitleFormat) OrElse String.IsNullOrWhiteSpace(Me.ResourceTitle) Then
                Return ""
            Else
                Return String.Format(Me.ClosedResourceTitleFormat, Me.ResourceTitle)
            End If
        End If
    End Function

#End Region

#Region " DEVICE And CONNECTOR "

    ''' <summary> Gets or sets the elapsed time stop watch. </summary>
    ''' <value> The elapsed time stop watch. </value>
    Protected ReadOnly Property ElapsedTimeStopwatch As Stopwatch

    ''' <summary> Gets or sets the sentinel indicating if this panel owns the device and, therefore, needs to 
    '''           dispose of this device. </summary>
    ''' <value> The is device owner. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property IsDeviceOwner As Boolean

    ''' <summary> Releases the device. </summary>
    Private Sub _ReleaseDevice()
        If Me.Device IsNot Nothing Then
            Me.Device.Talker.Listeners.Clear()
            RemoveHandler Me.Device.PropertyChanged, AddressOf Me.DevicePropertyChanged
            RemoveHandler Me.Device.Opening, AddressOf Me.DeviceOpening
            RemoveHandler Me.Device.Opened, AddressOf Me.DeviceOpened
            RemoveHandler Me.Device.Closing, AddressOf Me.DeviceClosing
            RemoveHandler Me.Device.Closed, AddressOf Me.DeviceClosed
            RemoveHandler Me.Device.Initialized, AddressOf Me.DeviceInitialized
            RemoveHandler Me.Device.Initializing, AddressOf Me.DeviceInitializing
            Me.Connector.ResourcesFilter = ""
            ' note that service request is released when device closes.
        End If
    End Sub

    ''' <summary> Releases the device. </summary>
    Protected Overridable Sub ReleaseDevice()
        Me._ReleaseDevice()
    End Sub

    ''' <summary> Assign device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignDevice(ByVal value As VI.DeviceBase)
        Me._ReleaseDevice()
        Me._ElapsedTimeStopwatch = New Stopwatch
        If Me.Connector IsNot Nothing Then
            Me.Connector.Clearable = True
            Me.Connector.Connectable = True
            Me.Connector.Searchable = True
        End If
        Me._StatusRegisterStatus = ResourceControlBase.UnknownRegisterValue
        Me._StandardRegisterStatus = ResourceControlBase.UnknownRegisterValue
        Me._Device = value
        If value IsNot Nothing Then
            Me.Device.CaptureSyncContext(Threading.SynchronizationContext.Current)
            AddHandler Me.Device.PropertyChanged, AddressOf Me.DevicePropertyChanged
            AddHandler Me.Device.Opening, AddressOf Me.DeviceOpening
            AddHandler Me.Device.Opened, AddressOf Me.DeviceOpened
            AddHandler Me.Device.Closing, AddressOf Me.DeviceClosing
            AddHandler Me.Device.Closed, AddressOf Me.DeviceClosed
            AddHandler Me.Device.Initialized, AddressOf Me.DeviceInitialized
            AddHandler Me.Device.Initializing, AddressOf Me.DeviceInitializing
            If Me.Device.ResourcesFilter IsNot Nothing Then
                Me.Connector.ResourcesFilter = Me.Device.ResourcesFilter
            End If
            Me.ResourceName = Me.Device.ResourceName
        End If
    End Sub

    ''' <summary> Assign device. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overridable Sub AssignDevice(ByVal value As VI.DeviceBase)
        Me._AssignDevice(value)
    End Sub

    ''' <summary> Gets the is device assigned. </summary>
    ''' <value> The is device assigned. </value>
    Public Overridable ReadOnly Property IsDeviceAssigned As Boolean
        Get
            Return Me.Device IsNot Nothing AndAlso Not Me.Device.IsDisposed
        End Get
    End Property

    ''' <summary> Gets or sets reference to the VISA <see cref="VI.DeviceBase">device</see>
    ''' interfaces. </summary>
    ''' <value> The connectable resource. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected ReadOnly Property Device() As VI.DeviceBase

    ''' <summary> Gets the sentinel indicating if the device is open. </summary>
    ''' <value> The is device open. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Protected ReadOnly Property IsDeviceOpen As Boolean
        Get
            Return Me.Device IsNot Nothing AndAlso Me.Device.IsDeviceOpen
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
            AddHandler Me.Device.ServiceRequested, AddressOf Me.DeviceServiceRequested
            If Me.IsDeviceOwner Then
                Me.Device.AddServiceRequestEventHandler()
            ElseIf Not Me.Device.DeviceServiceRequestHandlerAdded Then
                Me.Device.AddServiceRequestEventHandler()
                Me._PanelDeviceServiceRequestHandlerAdded = True
            End If
            Me._PanelServiceRequestHandlerAdded = True
        End If
    End Sub

    ''' <summary> Removes the service request event. </summary>
    Public Overridable Sub RemoveServiceRequestEventHandler()
        If Me.PanelServiceRequestHandlerAdded Then
            RemoveHandler Me.Device.ServiceRequested, AddressOf Me.DeviceServiceRequested
            If Me.IsDeviceOwner OrElse Me._PanelDeviceServiceRequestHandlerAdded Then Me.Device.RemoveServiceRequestEventHandler()
            Me._PanelServiceRequestHandlerAdded = False
            Me._PanelDeviceServiceRequestHandlerAdded = False
        End If
    End Sub

#End Region

#Region " PROPERTY CHANGE "

    ''' <summary> Executes the device open changed action. </summary>
    Protected Overridable Sub OnDeviceOpenChanged(ByVal device As DeviceBase)
    End Sub

    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Sub OnDevicePropertyChanged(ByVal device As DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(device.IsDeviceOpen)
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
                Me.OnTitleChanged(Me.BuildTitle)
            Case NameOf(device.ResourceName)
                Me.ResourceName = device.ResourceName
                Me.OnTitleChanged(Me.BuildTitle)
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

    ''' <summary> Executes the subsystem property changed action. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Sub OnPropertyChanged(ByVal subsystem As StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.ErrorAvailable)
                If Not subsystem.ReadingDeviceErrors AndAlso subsystem.ErrorAvailable Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Error available;. ")
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
            Case NameOf(subsystem.ServiceRequestStatus)
                Me.StatusRegisterStatus = subsystem.ServiceRequestStatus
            Case NameOf(subsystem.StandardEventStatus)
                Me.StandardRegisterStatus = subsystem.StandardEventStatus
        End Select
    End Sub

#End Region

#Region " OPENING / OPEN "

    ''' <summary> Connects the instrument by opening a visa session. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:   DoNotCatchGeneralExceptionTypes")>
    Private Sub OpenSession(ByVal resourceName As String, ByVal resourceTitle As String)
        Try
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Opening {0};. ", resourceName)
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            Me.Device.OpenSession(resourceName, resourceTitle)
            If Me.Device.IsDeviceOpen Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Opened {0};. ", resourceName)
            Else
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Open {0} Failed;. ", resourceName)
            End If
        Catch ex As OperationFailedException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Failed opening {resourceName};. {ex.ToFullBlownString}")
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception opening {resourceName};. {ex.ToFullBlownString}")
        Finally
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called upon device opening. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceOpening(ByVal sender As Object, ByVal e As ComponentModel.CancelEventArgs)
        Me._StatusRegisterStatus = ResourceControlBase.UnknownRegisterValue
        Me._StandardRegisterStatus = ResourceControlBase.UnknownRegisterValue
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ResourceName = Me.Device.ResourceName
        If Not String.IsNullOrEmpty(Me.ResourceTitle) Then Me.Device.Session.ResourceTitle = Me.ResourceTitle
        Dim outcome As TraceEventType = TraceEventType.Information
        If Me.Device.Session.Enabled And Not Me.Device.Session.IsSessionOpen Then outcome = TraceEventType.Warning
        Me.Talker.Publish(outcome, My.MyLibrary.TraceEventId,
                          "{0} {1:enabled;enabled;disabled} and {2:open;open;closed}; session {3:open;open;closed};. ",
                          Me.ResourceTitle,
                          Me.Device?.Session.Enabled.GetHashCode,
                          Me.Device?.Session.IsDeviceOpen.GetHashCode,
                          Me.Device?.Session.IsSessionOpen.GetHashCode)
    End Sub

#End Region

#Region " INITALIZING / INITIALIZED  "

    Protected Overridable Sub DeviceInitializing(ByVal sender As Object, ByVal e As ComponentModel.CancelEventArgs)
    End Sub

    ''' <summary> Device initialized. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceInitialized(ByVal sender As Object, ByVal e As System.EventArgs)
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

#End Region

#Region " CLOSING / CLOSED "

    ''' <summary> Disconnects the instrument by closing the open session. </summary>
    Private Sub CloseSession()
        If Me.Device.IsDeviceOpen Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Releasing {0};. ", Me.Device.ResourceName)
        End If
        If Me.Device.TryCloseSession() Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Released {0};. ", Me.Connector.SelectedResourceName)
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Failed releasing {0};. ", Me.Connector.SelectedResourceName)
        End If
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceClosing(ByVal sender As Object, ByVal e As ComponentModel.CancelEventArgs)
        If Me.Device IsNot Nothing Then
            Me.RemoveServiceRequestEventHandler()
            If Me.Device.Session IsNot Nothing Then
                Me.Device.Session.DisableServiceRequest()
            End If
        End If
    End Sub

    ''' <summary> Event handler. Called when device is closed. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceClosed(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.Device IsNot Nothing Then
            If Me.Device.Session.IsSessionOpen Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Device closed but session still open;. ")
            ElseIf Me.Device.Session.IsDeviceOpen Then
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

    Private _Connector As ResourceSelectorConnector

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

    ''' <summary> Connects. </summary>
    ''' <param name="e"> The e to connect. </param>
    Public Sub TryConnect(ByVal e As System.ComponentModel.CancelEventArgs)
        Me.Connect(Me.Connector, e)
    End Sub

    ''' <summary> Connects. </summary>
    ''' <param name="e"> The e to connect. </param>
    Public Sub Connect(ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of Object, System.ComponentModel.CancelEventArgs)(AddressOf Me.Connect), New Object() {e})
        Else
            Me.OnConnect(e)
        End If
    End Sub

    ''' <summary> Connects the instrument by calling a propagating connect command. </summary>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Protected Overridable Sub OnConnect(ByVal e As System.ComponentModel.CancelEventArgs)
        Me.OpenSession(Me.Connector.SelectedResourceName, Me.ResourceTitle)
        ' cancel if failed to open
        If e IsNot Nothing AndAlso Not Me.IsDeviceOpen Then e.Cancel = True
    End Sub

    ''' <summary> Connects. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Cancel event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Connect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Dim action As String = "Connecting"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.CancelEventArgs)(AddressOf Me.Connect), New Object() {sender, e})
            Else
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{action};. {Me.Connector.SelectedResourceName}")
                Me.OnConnect(e)
                If e.Cancel Then
                    action = $"connection failed;. resource {Me.Connector.SelectedResourceName} is not open"
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, action)
                Else
                    action = $"connected;. {Me.Connector.SelectedResourceName}"
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, action)
                End If
            End If
        Catch ex As Exception
            e.Cancel = True
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {action};. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Disconnects the instrument by calling a propagating disconnect command. </summary>
    ''' <param name="e"> The e to disconnect. </param>
    Public Sub TryDisconnect(ByVal e As System.ComponentModel.CancelEventArgs)
        Me.Disconnect(Me.Connector, e)
    End Sub

    ''' <summary> Disconnects the instrument by calling a propagating disconnect command. </summary>
    ''' <param name="e"> The e to disconnect. </param>
    Public Sub Disconnect(ByVal e As System.ComponentModel.CancelEventArgs)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of Object, System.ComponentModel.CancelEventArgs)(AddressOf Me.Disconnect), New Object() {e})
        Else
            Me.OnDisconnect(e)
        End If
    End Sub

    ''' <summary> Raises the system. component model. cancel event. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overridable Sub OnDisconnect(ByVal e As System.ComponentModel.CancelEventArgs)
        Me.CloseSession()
        ' Cancel if failed to close
        If e IsNot Nothing AndAlso Me.IsDeviceOpen Then e.Cancel = True
    End Sub

    ''' <summary> Disconnects the instrument by calling a propagating disconnect command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Disconnect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Dim action As String = "Disconnecting"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, System.ComponentModel.CancelEventArgs)(AddressOf Me.Disconnect), New Object() {sender, e})
            Else
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{action};. {Me.Connector.SelectedResourceName}")
                Me.OnDisconnect(e)
                If e.Cancel Then
                    action = $"disconnection failed;. resource {Me.Connector.SelectedResourceName} still open"
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, action)
                Else
                    action = $"disconnected;. {Me.Connector.SelectedResourceName}"
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, action)
                End If
            End If
        Catch ex As Exception
            e.Cancel = True
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {action};. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    Protected Overridable Sub ClearDevice()
        Me.Device.ResetClearInit()
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ClearDevice(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim action As String = "Resetting, clearing and initializing resource to know state"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.ConnectorPropertyChanged), New Object() {sender, e})
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
    Public Overridable Sub DisplayNames()
        ' get the list of available resources
        If Me.Device IsNot Nothing AndAlso Me.Device.ResourcesFilter IsNot Nothing Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Using resource filter {Me.Device.ResourcesFilter}")
            Me.Connector.ResourcesFilter = Me.Device.ResourcesFilter
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
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.ConnectorPropertyChanged), New Object() {sender, e})
            Else
                Me.DisplayNames()
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
            Case NameOf(sender.SelectedResourceName)
                If String.IsNullOrWhiteSpace(sender.SelectedResourceName) OrElse
                    String.Equals(sender.SelectedResourceName, VI.DeviceBase.ResourceNameClosed) Then
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
        If e Is Nothing Then Return
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

#Region " TALKER "

    ''' <summary> Assigns talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AssignTalker(talker As ITraceMessageTalker)
        If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
        MyBase.AssignTalker(talker)
        Me.Connector.AssignTalker(talker)
        My.MyLibrary.Identify(talker)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        ' this should apply only to the listeners associated with this form
        MyBase.ApplyListenerTraceLevel(listenerType, value)
        Me.Connector.ApplyListenerTraceLevel(listenerType, value)
    End Sub

#End Region

End Class

