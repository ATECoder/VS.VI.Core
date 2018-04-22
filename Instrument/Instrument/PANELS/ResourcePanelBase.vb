Imports System.ComponentModel
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
Public Class ResourcePanelBase
    Inherits TalkerControlBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    Public Sub New()
        Me.New(Nothing)
    End Sub

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    ''' <param name="device"> The connectable resource. </param>
    Protected Sub New(ByVal device As VI.DeviceBase)
        MyBase.New()
        Me.InitializingComponents = True
        Me.InitializeComponent()
        Me.InitializingComponents = False
        Me._ElapsedTimeStopwatch = New Stopwatch
        Me._AssignDevice(device)
        Me.Connector.Connectable = True
        Me.Connector.Clearable = True
        Me.StatusRegisterLabel.Visible = False
        Me.StandardRegisterLabel.Visible = False
        Me.IdentityLabel.Visible = False
        Me._StatusRegisterStatus = -1
        Me._StandardRegisterStatus = -1
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
                        Me.Device?.RemovePrivateListener(Me.TraceMessagesBox)
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
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing

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
            Me.StatusLabel.Text = "Find and select a resource."
            Me.IdentityLabel.Text = ""
            Me.StatusRegisterLabel.Visible = True
            Me.StandardRegisterLabel.Visible = False
            Me.IdentityLabel.Visible = False
        Finally
            MyBase.OnLoad(e)
        End Try
    End Sub

#End Region

#Region " SAFE SETTERS "

    ''' <summary> Recursively enable. </summary>
    ''' <param name="control"> The control. </param>
    ''' <param name="value">   True to show or False to hide the control. </param>
    Protected Sub RecursivelyEnable(ByVal control As System.Windows.Forms.Control, ByVal value As Boolean)
        If control IsNot Nothing Then
            control.Enabled = value
            If control.Controls IsNot Nothing AndAlso control.Controls.Count > 0 Then
                For Each c As System.Windows.Forms.Control In control.Controls
                    Me.RecursivelyEnable(c, value)
                Next
            End If
        End If
    End Sub

    ''' <summary> Safe visible setter. </summary>
    ''' <param name="control"> The control. </param>
    ''' <param name="value">   True to show or False to hide the control. </param>
    Private Shared Sub SafeVisibleSetter(ByVal control As System.Windows.Forms.ToolStripItem, value As Boolean)
        If control IsNot Nothing AndAlso control.Owner IsNot Nothing AndAlso control.Visible <> value Then
            If control.Owner.InvokeRequired Then
                control.Owner.Invoke(New Action(Of System.Windows.Forms.ToolStripItem, Boolean)(AddressOf ResourcePanelBase.SafeVisibleSetter), New Object() {control, value})
            Else
                control.Visible = value
            End If
        End If
    End Sub

    ''' <summary> Safe text setter. </summary>
    ''' <param name="control"> The control. </param>
    ''' <param name="value">   True to show or False to hide the control. </param>
    Private Shared Sub SafeTextSetter(ByVal control As System.Windows.Forms.ToolStripItem, value As String)
        If control IsNot Nothing AndAlso control.Owner IsNot Nothing AndAlso
            Not String.Equals(control.Text, value) Then
            If control.Owner.InvokeRequired Then
                control.Owner.Invoke(New Action(Of System.Windows.Forms.ToolStripItem, String)(AddressOf ResourcePanelBase.SafeTextSetter), New Object() {control, value})
            Else
                control.Text = value
            End If
        End If
    End Sub

    ''' <summary> Safe tool tip text setter. </summary>
    ''' <param name="control"> The control. </param>
    ''' <param name="value">   True to show or False to hide the control. </param>
    Private Shared Sub SafeToolTipTextSetter(ByVal control As System.Windows.Forms.ToolStripItem, value As String)
        If control IsNot Nothing AndAlso control.Owner IsNot Nothing AndAlso
            Not String.Equals(control.Text, value) Then
            If control.Owner.InvokeRequired Then
                control.Owner.Invoke(New Action(Of System.Windows.Forms.ToolStripItem, String)(AddressOf ResourcePanelBase.SafeTextSetter), New Object() {control, value})
            Else
                control.ToolTipText = value
            End If
        End If
    End Sub

#End Region

#Region " REGISTER DISPLAY SETTERS "


    ''' <summary> Shows or hides the identity display. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Sub IdentityVisibleSetter(ByVal value As Boolean)
        ResourcePanelBase.SafeVisibleSetter(Me.IdentityLabel, value)
    End Sub

    ''' <summary> Show or hide the status register display. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Sub StatusRegisterVisibleSetter(ByVal value As Boolean)
        ResourcePanelBase.SafeVisibleSetter(Me.StatusRegisterLabel, value)
    End Sub

    ''' <summary> Displays the status register status using hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Public Overridable Sub DisplayStatusRegisterStatus(ByVal value As Integer)
        Me.DisplayStatusRegisterStatus(value, "0x{0:X2}")
    End Sub

    Private _StatusRegisterStatus As Integer
    ''' <summary> Displays the status register status. </summary>
    ''' <param name="value">  The register value. </param>
    ''' <param name="format"> The format. </param>
    Public Sub DisplayStatusRegisterStatus(ByVal value As Integer, ByVal format As String)
        If Not value.Equals(Me._StatusRegisterStatus) Then
            Me._StatusRegisterStatus = value
            ResourcePanelBase.SafeTextSetter(Me.StatusRegisterLabel, String.Format(format, value))
        End If
    End Sub

    ''' <summary> Shows or hides the Standard Register display. </summary>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Sub StandardRegisterVisibleSetter(ByVal value As Boolean)
        If Me.StandardRegisterLabel.Visible <> value Then ResourcePanelBase.SafeVisibleSetter(Me.StandardRegisterLabel, value)
    End Sub

    ''' <summary> Displays the standard register status. Uses Hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Public Sub DisplayStandardRegisterStatus(ByVal value As VI.Pith.StandardEvents?)
        If value.HasValue Then Me.DisplayStatusRegisterStatus(CInt(value.Value))
    End Sub

    ''' <summary> Displays the standard register status. Uses Hex format. </summary>
    ''' <param name="value"> The register value. </param>
    Public Sub DisplayStandardRegisterStatus(ByVal value As Integer)
        Me.DisplayStandardRegisterStatus(value, "0x{0:X2}")
    End Sub

    Private _StandardRegisterStatus As Integer
    ''' <summary> Displays the standard register status. </summary>
    ''' <param name="value">  The register value. </param>
    ''' <param name="format"> The format. </param>
    Public Sub DisplayStandardRegisterStatus(ByVal value As Integer?, ByVal format As String)
        Me.StandardRegisterVisibleSetter(value.HasValue)
        If value.HasValue AndAlso Not value.Value.Equals(Me._StandardRegisterStatus) Then
            Me._StandardRegisterStatus = value.Value
            ResourcePanelBase.SafeTextSetter(Me.StatusRegisterLabel, String.Format(format, value.Value))
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
                ResourcePanelBase.SafeTextSetter(Me.IdentityLabel, Me.ResourceName)
                ResourcePanelBase.SafeToolTipTextSetter(Me.IdentityLabel, Me.ResourceName)
                Me.SafePostPropertyChanged()
            End If
            If Not String.IsNullOrWhiteSpace(value) Then Me.Connector.SessionFactory.TrySelectResource(value, New isr.Core.Pith.ActionEventArgs)
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

    ''' <summary> Gets the number of elapsed times. Some action require two cycles to get the full elapsed time. </summary>
    ''' <value> The number of elapsed times. </value>
    Public ReadOnly Property ElapsedTimeCount As Integer

    ''' <summary> Starts elapsed stopwatch. </summary>
    Protected Sub StartElapsedStopwatch(ByVal count As Integer)
        Me._ElapsedTimeCount = count
        Me.StartElapsedStopwatch(0)
    End Sub

    ''' <summary> Gets or sets the sentinel indicating if this panel owns the device and, therefore, needs to 
    '''           dispose of this device. </summary>
    ''' <value> The is device owner. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property IsDeviceOwner As Boolean

    ''' <summary> Releases the device. </summary>
    Private Sub _ReleaseDevice()
        If Me.Device IsNot Nothing Then
            Me.Device.Talker.Listeners.Clear()
            RemoveHandler Me.Device.Session.ResourceNameInfo.PropertyChanged, AddressOf Me.ResourceNameInfoPropertyChanged
            RemoveHandler Me.Device.PropertyChanged, AddressOf Me.DevicePropertyChanged
            RemoveHandler Me.Device.Opening, AddressOf Me.DeviceOpening
            RemoveHandler Me.Device.Opened, AddressOf Me.DeviceOpened
            RemoveHandler Me.Device.Closing, AddressOf Me.DeviceClosing
            RemoveHandler Me.Device.Closed, AddressOf Me.DeviceClosed
            RemoveHandler Me.Device.Initialized, AddressOf Me.DeviceInitialized
            RemoveHandler Me.Device.Initializing, AddressOf Me.DeviceInitializing
            Me.Connector.SessionFactory.ResourcesFilter = ""
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
        Me._Device = value
        If value IsNot Nothing Then
            Me.Device.CaptureSyncContext(Threading.SynchronizationContext.Current)
            AddHandler Me.Device.Session.ResourceNameInfo.PropertyChanged, AddressOf Me.ResourceNameInfoPropertyChanged
            AddHandler Me.Device.PropertyChanged, AddressOf Me.DevicePropertyChanged
            AddHandler Me.Device.Opening, AddressOf Me.DeviceOpening
            AddHandler Me.Device.Opened, AddressOf Me.DeviceOpened
            AddHandler Me.Device.Closing, AddressOf Me.DeviceClosing
            AddHandler Me.Device.Closed, AddressOf Me.DeviceClosed
            AddHandler Me.Device.Initialized, AddressOf Me.DeviceInitialized
            AddHandler Me.Device.Initializing, AddressOf Me.DeviceInitializing
            If Me.Device.Session.ResourceNameInfo.ResourcesFilter IsNot Nothing Then
                Me.Connector.SessionFactory.ResourcesFilter = Me.Device.Session.ResourceNameInfo.ResourcesFilter
            End If
            Me.ResourceName = Me.Device.Session.ResourceName
            Me._Device.AddPrivateListener(Me.TraceMessagesBox)
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
    Protected Overridable Sub OnDeviceOpenChanged(ByVal device As VI.DeviceBase)
    End Sub

    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="resourceNameInfo"> The resource name info. </param>
    ''' <param name="propertyName">     Name of the property. </param>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal resourceNameInfo As VI.Pith.ResourceNameInfo, ByVal propertyName As String)
        If resourceNameInfo Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.Pith.ResourceNameInfo.ResourcesFilter)
                Me.Connector.SessionFactory.ResourcesFilter = resourceNameInfo.ResourcesFilter
            Case NameOf(VI.Pith.ResourceNameInfo.ResourceTitle)
                Me.ResourceTitle = resourceNameInfo.ResourceTitle
                Me.OnTitleChanged(Me.BuildTitle)
            Case NameOf(VI.Pith.ResourceNameInfo.ResourceName)
                Me.ResourceName = resourceNameInfo.ResourceName
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
                Me.OnDeviceOpenChanged(device)
            Case NameOf(isr.VI.DeviceBase.Enabled)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   $"{device.ResourceTitle} {device.Enabled.GetHashCode:enabled;enabled;disabled};. ")
            Case NameOf(isr.VI.DeviceBase.ServiceRequestFailureMessage)
                If Not String.IsNullOrWhiteSpace(device.ServiceRequestFailureMessage) Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, device.ServiceRequestFailureMessage)
                End If
            Case NameOf(isr.VI.DeviceBase.ResourceTitle)
                Me.ResourceTitle = device.ResourceTitle
                Me.OnTitleChanged(Me.BuildTitle)
            Case NameOf(isr.VI.DeviceBase.ResourceName)
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
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(VI.DeviceBase)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.DevicePropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, VI.DeviceBase), e.PropertyName)
            End If
        Catch ex As Exception
            If Me.Talker Is Nothing Then
                My.MyLibrary.LogUnpublishedException(activity, ex)
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}")
            End If
        End Try
    End Sub

    ''' <summary> Handles the subsystem property change. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Overloads Sub HandlePropertyChange(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(StatusSubsystemBase.ErrorAvailable)
                If Not subsystem.ReadingDeviceErrors AndAlso subsystem.ErrorAvailable Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Error available;. ")
                End If
            Case NameOf(StatusSubsystemBase.MessageAvailable)
                If subsystem.MessageAvailable Then
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Message available;. ")
                End If
            Case NameOf(StatusSubsystemBase.MeasurementAvailable)
                If subsystem.MeasurementAvailable Then
                    Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measurement available;. ")
                End If
            Case NameOf(StatusSubsystemBase.ReadingDeviceErrors)
                If subsystem.ReadingDeviceErrors Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Reading device errors;. ")
                End If
            Case NameOf(StatusSubsystemBase.ServiceRequestStatus)
                Me.DisplayStatusRegisterStatus(subsystem.ServiceRequestStatus)
            Case NameOf(StatusSubsystemBase.StandardEventStatus)
                Me.DisplayStandardRegisterStatus(subsystem.StandardEventStatus)
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
        Catch ex As vi.Pith.OperationFailedException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Failed opening {resourceName};. {ex.ToFullBlownString}")
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception opening {resourceName};. {ex.ToFullBlownString}")
        Finally
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Connects the instrument by calling a propagating connect command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub Connector_Connect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Connector.Connect
        Me.OpenSession(Me.Connector.SessionFactory.SelectedResourceName, Me.ResourceTitle)
        ' cancel if failed to open
        If Not Me.IsDeviceOpen Then e.Cancel = True
    End Sub

    ''' <summary> Event handler. Called upon device opening. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceOpening(ByVal sender As Object, ByVal e As ComponentModel.CancelEventArgs)
        Me.StatusRegisterLabel.Text = "0x.."
        Me.StandardRegisterLabel.Text = "0x.."
    End Sub

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overridable Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ResourceName = Me.Device.ResourceName
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
        Me.ReadServiceRequestStatus()
    End Sub

    ''' <summary> Reads a service request status. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub ReadServiceRequestStatus()
        Try
            Me.Device.StatusSubsystemBase.ReadServiceRequestStatus()
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception reading service request;. {ex.ToFullBlownString}")
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
    Public Property Title As String

#End Region

#Region " CLOSING / CLOSED "

    ''' <summary> Disconnects the instrument by closing the open session. </summary>
    Private Sub CloseSession()
        If Me.Device.IsDeviceOpen Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Releasing {0};. ", Me.Device.ResourceName)
        End If
        If Me.Device.TryCloseSession() Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Released {0};. ", Me.Connector.SessionFactory.SelectedResourceName)
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Failed releasing {0};. ", Me.Connector.SessionFactory.SelectedResourceName)
        End If
    End Sub

    ''' <summary> Disconnects the instrument by calling a propagating disconnect command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub Connector_Disconnect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Connector.Disconnect
        Me.CloseSession()
        ' Cancel if failed to closed
        If Me.IsDeviceOpen Then e.Cancel = True
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
        Me.StatusRegisterLabel.Visible = False
        Me.StandardRegisterLabel.Visible = False
        Me.IdentityLabel.Visible = False
        Me.StandardRegisterVisibleSetter(False)
        Me.StatusRegisterVisibleSetter(False)
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

    ''' <summary> Displays the resource names based on the device resource search pattern. </summary>
    Public Sub DisplayNames()
        ' get the list of available resources
        ' this enumerates the resources and sets the resource exists sentinel to tell the control to display 
        ' the enumerated resources.
        Me.Connector.SessionFactory.ResourcesFilter = Me.Device.Session.ResourceNameInfo.ResourcesFilter
        Me.Connector.SessionFactory.EnumerateResources()
    End Sub

    ''' <summary> Clears the instrument by calling a propagating clear command. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub Connector_Clear(ByVal sender As Object, ByVal e As System.EventArgs) Handles Connector.Clear
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                           "Resetting, clearing and initializing resource to know state;. {0}", Me.Connector.SessionFactory.SelectedResourceName)
        Me.Device.ResetClearInit()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                           "Resource reset, cleared and initialized;. {0}", Me.Connector.SessionFactory.SelectedResourceName)
    End Sub

    ''' <summary> Displays available instrument names. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub Connector_FindNames(ByVal sender As Object, ByVal e As System.EventArgs) Handles Connector.FindNames
        Me.DisplayNames()
    End Sub

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(ByVal sender As ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SessionFactory.SelectedResourceName)
                Me.ResourceName = sender.SessionFactory.SelectedResourceName
                If sender.IsConnected Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Resource connected;. {Me.ResourceName}")
                Else
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Resource selected;. {Me.ResourceName}")
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _ResourceNameSelectorConnector for property changed
    ''' events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Connector_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Connector.PropertyChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Instrument.ResourceSelectorConnector)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me.Connector_PropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, Instrument.ResourceSelectorConnector), e.PropertyName)
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
        talker.Listeners.Add(Me.TraceMessagesBox)
        Me.Connector.AssignTalker(talker)
        MyBase.AssignTalker(talker)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        If listenerType = Me._TraceMessagesBox.ListenerType Then Me._TraceMessagesBox.ApplyTraceLevel(value)
        Me.Connector.ApplyListenerTraceLevel(listenerType, value)
        ' this should apply only to the listeners associated with this form
        ' MyBase.ApplyListenerTraceLevel(listenerType, value)
    End Sub

#End Region

#Region " MESSAGE BOX EVENTS "

    ''' <summary> Handles the <see cref="_TraceMessagesBox"/> property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Overloads Sub HandlePropertyChange(sender As TraceMessagesBox, propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        If String.Equals(propertyName, NameOf(isr.Core.Pith.TraceMessagesBox.StatusPrompt)) Then
            Me._StatusLabel.Text = isr.Core.Pith.CompactExtensions.Compact(sender.StatusPrompt, Me._StatusLabel)
            Me._StatusLabel.ToolTipText = sender.StatusPrompt
        End If
    End Sub

    ''' <summary> Handles Trace messages box property changed event. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceMessagesBox_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles TraceMessagesBox.PropertyChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Core.Pith.TraceMessagesBox)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._TraceMessagesBox_PropertyChanged), New Object() {sender, e})
            Else
                Me.HandlePropertyChange(TryCast(sender, Core.Pith.TraceMessagesBox), e.PropertyName)
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

End Class

