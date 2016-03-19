Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.VI.Instrument
''' <summary> Provides a user interface for a generic SCPI Device. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2008" by="David" revision="2.0.2936.x"> Create based on the 24xx
''' system classes. </history>
<System.ComponentModel.DisplayName("SCPI Panel"),
      System.ComponentModel.Description("SCPI Device Panel"),
      System.Drawing.ToolboxBitmap(GetType(ScpiPanel))>
Public Class ScpiPanel
    Inherits ResourcePanelBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private _InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        Me.New(New Device)
        Me.IsDeviceOwner = True
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="device"> The device. </param>
    Protected Sub New(ByVal device As Device)
        MyBase.New(device)
        Me._InitializingComponents = True
        Me.InitializeComponent()
        Me._InitializingComponents = False
        Me._AssignDevice(device)
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the isr.VI.Instrument.ResourcePanelBase and
    ''' optionally releases the managed resources.
    ''' </summary>
    ''' <remarks> David, 12/22/2015. </remarks>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.DebuggerNonUserCode()>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' the device gets closed and disposed (if panel is device owner) in the base class
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " FORM EVENTS "

    ''' <summary> Handles the <see cref="E:System.Windows.Forms.UserControl.Load" /> event. </summary>
    ''' <remarks> David, 1/4/2016. </remarks>
    ''' <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Try
            Me.TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
        Finally
            MyBase.OnLoad(e)
        End Try
    End Sub

#End Region

#Region " DEVICE "

    ''' <summary> Assigns a device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Private Sub _AssignDevice(ByVal value As Device)
        Me._Device = value
        Me.AddListeners()
        Me.OnDeviceOpenChanged(value)
    End Sub

    ''' <summary> Assigns a device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Public Overloads Sub AssignDevice(ByVal value As Device)
        Me.IsDeviceOwner = False
        MyBase.AssignDevice(value)
        Me._AssignDevice(value)
    End Sub

    ''' <summary> Releases the device. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    Protected Overrides Sub ReleaseDevice()
        MyBase.ReleaseDevice()
    End Sub

    ''' <summary> Gets a reference to the generic SCPI Device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overloads ReadOnly Property Device() As Device

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Executes the device open changed action. </summary>
    ''' <remarks> David, 3/3/2016. </remarks>
    Protected Overrides Sub OnDeviceOpenChanged(ByVal device As DeviceBase)
        Dim isOpen As Boolean = CType(device?.IsDeviceOpen, Boolean?).GetValueOrDefault(False)
        If isOpen Then
            Me._SimpleReadWriteControl.Connect(device?.Session)
        Else
            Me._SimpleReadWriteControl.Disconnect()
        End If
        For Each t As Windows.Forms.TabPage In Me._Tabs.TabPages
            If t IsNot Me._MessagesTabPage Then
                For Each c As Windows.Forms.Control In t.Controls : Me.RecursivelyEnable(c, isOpen) : Next
            End If
        Next
    End Sub

    ''' <summary> Handle the device property changed event. </summary>
    ''' <param name="device">    The device. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnDevicePropertyChanged(ByVal device As DeviceBase, ByVal propertyName As String)
        If device Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.OnDevicePropertyChanged(device, propertyName)
        Select Case propertyName
            Case NameOf(device.IsServiceRequestEventEnabled)
                ' Me._HandleServiceRequestsCheckBox.Checked = device.IsServiceRequestEventEnabled
        End Select
    End Sub

#End Region

#Region " DEVICE EVENT HANDLERS "

    ''' <summary> Event handler. Called when device opened. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.EnableServiceRequestEventHandler()
        AddHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
        AddHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        MyBase.DeviceOpened(sender, e)
    End Sub

    ''' <summary> Executes the title changed action. </summary>
    ''' <remarks> David, 1/14/2016. </remarks>
    ''' <param name="value"> True to show or False to hide the control. </param>
    Protected Overrides Sub OnTitleChanged(ByVal value As String)
        Me._TitleLabel.Text = value
        Me._TitleLabel.Visible = Not String.IsNullOrWhiteSpace(value)
        MyBase.OnTitleChanged(Title)
    End Sub

    ''' <summary> Event handler. Called when device is closing. </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    ''' <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    Protected Overrides Sub DeviceClosing(ByVal sender As Object, ByVal e As ComponentModel.CancelEventArgs)
        MyBase.DeviceClosing(sender, e)
        If e?.Cancel Then Return
        If Me.IsDeviceOpen Then
            RemoveHandler Me.Device.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
            RemoveHandler Me.Device.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " STATUS "

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, StatusSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " SYSTEM "

    ''' <summary> Handle the System subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, SystemSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. Details: {1}", e.PropertyName, ex)
        End Try
    End Sub

#End Region

#End Region

#Region " CONTROL EVENT HANDLERS: INTERFACE "

    ''' <summary> Event handler. Called by interfaceClearButton for click events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub interfaceClearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InterfaceClearButton.Click
        ' Me.Device.GpibInterface.SendInterfaceClear()
    End Sub

    ''' <summary> Issue RST. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub ResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ResetButton.Click
        Me.Device.ResetKnownState()
    End Sub

#End Region

#Region " READ AND WRITE "

    ''' <summary> Executes the simple read write control property changed action. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSimpleReadWriteControlPropertyChanged(sender As SimpleReadWriteControl, ByVal propertyName As String)
        If sender IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(propertyName) Then
            Select Case propertyName
                Case NameOf(sender.ReceivedMessage)
                Case NameOf(sender.SentMessage)
                Case NameOf(sender.StatusMessage)
                    Me.StatusLabel.Text = sender.StatusMessage
                Case NameOf(sender.ServiceRequestValue)
                    Me.StatusRegisterLabel.Text = $"0x{sender.ServiceRequestValue:X2}"
                Case NameOf(sender.ElapsedTime)
            End Select
        End If
    End Sub

    ''' <summary> Simple read write control property changed. </summary>
    ''' <remarks> David, 12/29/2015. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SimpleReadWriteControl_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _SimpleReadWriteControl.PropertyChanged
        Try
            Me.OnSimpleReadWriteControlPropertyChanged(TryCast(sender, SimpleReadWriteControl), e?.PropertyName)
        Catch ex As Exception
            Me.StatusLabel.Text = "Exception occurred handling change"
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling Simple Read and Write property changed Event;. Failed property {0}. Details: {1}",
                               e?.PropertyName, ex)
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds listeners such as current level trace message box and log. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    Protected Overrides Sub AddListeners()
        MyBase.AddListeners()
        Me._SimpleReadWriteControl.AddListeners(Me.Talker.Listeners)
    End Sub

    ''' <summary> Adds listeners such as top level trace message box and log. </summary>
    ''' <remarks> David, 12/30/2015. </remarks>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of ITraceMessageListener))
        MyBase.AddListeners(listeners)
        Me._SimpleReadWriteControl.AddListeners(listeners)
    End Sub

    ''' <summary> Adds the log listener. </summary>
    ''' <remarks> David, 1/21/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="log"> The log. </param>
    Public Overrides Sub AddListeners(ByVal log As MyLog)
        If log Is Nothing Then Throw New ArgumentNullException(NameOf(log))
        MyBase.AddListeners(log)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    Private Sub _ChannelListComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _ChannelListComboBox.SelectedIndexChanged
        If Me._InitializingComponents Then Return
    End Sub

#End Region

End Class
