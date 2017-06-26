Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Provides a user interface for a
''' <see cref="VisaInterface">Visa interface</see>
''' such as a VISA Interface. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="09/04/2013" by="David" revision="3.0.4955"> created based on the legacy
''' interface panel. </history>
<System.ComponentModel.Description("GPIB Interface Panel")>
<System.Drawing.ToolboxBitmap(GetType(InterfacePanel), "InterfacePanel"), ToolboxItem(True)>
Public Class InterfacePanel
    Inherits TalkerControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializeComponent()

        ' disable until the connectible interface is set
        Me.Enabled = False
        Me._InterfaceChooser.Connectable = True
        Me._InterfaceChooser.Clearable = True
        Me._InterfaceChooser.Searchable = True

        Me._InstrumentChooser.Connectable = True
        Me._InstrumentChooser.Clearable = True
        Me._InstrumentChooser.Searchable = True

        Me._TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
        Me.AddListeners()

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
                ' Traps the VISA error because the interface might be disposed and the dispose sentinel is not exposed by the interface.
                Try
                    Me.VisaInterface?.Dispose() : Me._VisaInterface = Nothing
                Catch ex As ObjectDisposedException
                End Try
                ' unable to use null conditional because it is not seen by code analysis
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " FORM EVENT HANDLERS "

    ''' <summary> Event handler. Called by form for load events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me._StatusLabel.Text = "Find and select an interface."
    End Sub

#End Region

#Region " INTERFACE PROPERTIES AND METHODS "

    ''' <summary> Gets a value indicating whether the interface is open. </summary>
    ''' <value> <c>True</c> if the interface session is defined; otherwise, <c>False</c>. </value>
    Public ReadOnly Property IsInterfaceOpen As Boolean
        Get
            Return Me.VisaInterface IsNot Nothing AndAlso Not Me.VisaInterface.IsDisposed AndAlso Me.VisaInterface.IsOpen
        End Get
    End Property

    ''' <summary> Try open session. </summary>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <returns> <c>True</c> if open; <c>False</c> otherwise. </returns>
    Public Overridable Function TryOpenInterfaceSession(ByVal resourceName As String) As Boolean
        Try
            Me.OpenInterfaceSession(resourceName)
        Catch ex As OperationFailedException
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{ex.Message} occurred opening interface;. {ex.ToFullBlownString}")
            Return False
        End Try
        Return Me.IsInterfaceOpen
    End Function

    ''' <summary> Opens the interface session. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="resourceName"> Name of the resource. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Sub OpenInterfaceSession(ByVal resourceName As String)
        If Me.Enabled Then
            Try
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Opening interface to {0};. ", resourceName)
                Me.VisaInterface = isr.VI.SessionFactory.Get.Factory.CreateGpibInterfaceSession()
                Me.VisaInterface.OpenSession(resourceName)
                If Me.IsInterfaceOpen Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Interface '{0}' opened;. ", resourceName)
                    Me.displayResourceNames()
                    If Me._InterfaceChooser.HasResources Then
                        ' if has resources to display, move on. The chooser traps the errors.
                    Else
                        Me.TryCloseInterfaceSession()
                        Throw New OperationFailedException($"Failed listing resources for interface '{resourceName}'; ")
                    End If
                Else
                    Me.TryCloseInterfaceSession()
                    Throw New OperationFailedException($"Failed opening interface '{resourceName}'.")
                End If
            Catch ex As OperationFailedException
                Throw
            Catch ex As NativeException
                Me.TryCloseInterfaceSession()
                Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture,
                                                                 "VISA exception occurred opening interface '{0}'.", resourceName),
                                                             ex)
            Catch ex As Exception
                Me.TryCloseInterfaceSession()
                Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture,
                                                                 "Exception occurred opening interface '{0}'.", resourceName),
                                                             ex)
            Finally
                With Me._InterfaceChooser
                    .Enabled = Me.IsInterfaceOpen
                    .Visible = True
                    .Invalidate()
                End With
                With Me._InstrumentChooser
                    .Enabled = Me.IsInterfaceOpen
                    .Visible = True
                    .Invalidate()
                End With
                With Me._ClearSelectedResourceButton
                    .Enabled = Me.IsInterfaceOpen AndAlso Me._InstrumentChooser.HasResources AndAlso
                        Not String.IsNullOrWhiteSpace(Me._InstrumentChooser.SelectedResourceName)
                    .Visible = True
                    .Invalidate()
                End With
                With Me._ClearAllResourcesButton
                    .Enabled = Me.IsInterfaceOpen AndAlso Me._InstrumentChooser.HasResources
                    .Visible = True
                    .Invalidate()
                End With
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Me.SafePostPropertyChanged(NameOf(Me.IsInterfaceOpen))
                Windows.Forms.Application.DoEvents()
            End Try
        End If
    End Sub

    ''' <summary> Try close session. </summary>
    ''' <returns> <c>True</c> if session closed; otherwise <c>False</c>. </returns>
    Public Overridable Function TryCloseInterfaceSession() As Boolean
        Try
            Me.CloseInterfaceSession()
        Catch ex As OperationFailedException
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{ex.Message} occurred closing interface;. {ex.ToFullBlownString}")
            Return False
        End Try
        Return Not Me.IsInterfaceOpen
    End Function

    ''' <summary> Closes the session. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Sub CloseInterfaceSession()
        If Me.Enabled Then
            If Me.VisaInterface IsNot Nothing Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Closing interface to {0};. ", Me._InterfaceChooser.SelectedResourceName)
                Try
                    Me.VisaInterface.Dispose()
                    Try
                        ' Trying to null the session raises an ObjectDisposedException 
                        ' if session service request handler was not released. 
                        Me.VisaInterface = Nothing
                    Catch ex As ObjectDisposedException
                        Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
                    End Try
                Catch ex As NativeException
                    Throw New OperationFailedException("Exception occurred closing the interface.", ex)
                Catch ex As Exception
                    Throw New OperationFailedException("Exception occurred closing the interface.", ex)
                Finally
                    With Me._InstrumentChooser
                        .Enabled = False
                        .Visible = True
                        .Invalidate()
                    End With
                    With Me._ClearSelectedResourceButton
                        .Enabled = False
                        .Visible = True
                        .Invalidate()
                    End With
                    With Me._ClearAllResourcesButton
                        .Enabled = False
                        .Visible = True
                        .Invalidate()
                    End With
                    Me.SafePostPropertyChanged(NameOf(Me.IsInterfaceOpen))
                    Windows.Forms.Application.DoEvents()
                End Try
            End If
        End If
    End Sub

    ''' <summary> Gets or sets the visa interface. </summary>
    ''' <value> The visa interface. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property VisaInterface As InterfaceSessionBase

#End Region

#Region " INTERFACE CHOOSER EVENT HANDLERS "

    Private _InterfaceResourceName As String

    ''' <summary> Gets or sets the name of the interface resource. </summary>
    ''' <value> The name of the interface resource. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property InterfaceResourceName As String
        Get
            Return Me._InterfaceResourceName
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.InterfaceResourceName) Then
                Me._InterfaceResourceName = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Display the interface names based on the last interface type. </summary>
    Public Sub DisplayInterfaceNames()
        Try
            ' display the selected resources.
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Me._InterfaceChooser.ResourcesFilter = VI.ResourceNamesManager.BuildInterfaceFilter()
            Me._InterfaceChooser.DisplayResourceNames()
            If Me._InterfaceChooser.HasResources Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Interfaces available--select and connect;. Found interfaces.")
            Else
                Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   "NO INTERFACES;. No interfaces were found. Connect the interface(s) and click Find.")
            End If
            Me.displayResourceNames()
            Me.Enabled = True
        Catch ex As System.ArgumentException
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception finding interfaces;. Failed finding or listing interfaces. Connect the interface(s) and click Find. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears the interface by issuing an interface clear. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub InterfaceChooser_Clear(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InterfaceChooser.Clear
        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            If Me.IsInterfaceOpen Then
                Me.VisaInterface.SendInterfaceClear()
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{ex.Message} occurred clearing;. {ex.ToFullBlownString}")
            Me._TraceMessagesBox.AddMessage(ex)
        Finally
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    ''' <summary> Connects the interface. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub InterfaceChooser_Connect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _InterfaceChooser.Connect
        Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                           "Connecting {0};. ", Me._InterfaceChooser.SelectedResourceName)
        Dim resourcename As String = Me._InterfaceChooser.SelectedResourceName
        Me.OpenInterfaceSession(resourcename)
        ' cancel if failed to connect
        If Not Me.IsInterfaceOpen Then e.Cancel = True
    End Sub

    ''' <summary> Disconnects the interface. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub InterfaceChooser_Disconnect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _InterfaceChooser.Disconnect
        Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                           "Disconnecting {0};. ", Me._InterfaceChooser.SelectedResourceName)
        Me.TryCloseInterfaceSession()
        ' cancel if failed to disconnect
        If Me.IsInterfaceOpen Then e.Cancel = True
    End Sub

    ''' <summary> Displays available interface names. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub InterfaceChooser_FindNames(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InterfaceChooser.FindNames
        Me.DisplayInterfaceNames()
    End Sub

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Specifies the object where the call originated. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.SelectedResourceName)
                If String.IsNullOrWhiteSpace(sender.SelectedResourceName) OrElse
                    String.Equals(sender.SelectedResourceName, VI.DeviceBase.ResourceNameClosed) Then
                    Me.InterfaceResourceName = ""
                Else
                    Me.InterfaceResourceName = sender.SelectedResourceName
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Interface selected--connect;. {Me.InterfaceResourceName} selected;. ")
                End If
            Case NameOf(sender.SelectedResourceExists)
            Case NameOf(sender.ResourcesFilter)
            Case NameOf(sender.IsConnected)
            Case NameOf(sender.Clearable)
            Case NameOf(sender.Connectable)
            Case NameOf(sender.Searchable)
            Case NameOf(sender.HasResources)
        End Select
    End Sub

    ''' <summary> Event handler. Called by _InterfaceChooser for property changed events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InterfaceChooser_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InterfaceChooser.PropertyChanged
        If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._InterfaceChooser_PropertyChanged), New Object() {sender, e})
        End If
        Try
            Me.OnPropertyChanged(TryCast(sender, ResourceSelectorConnector), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling InterfaceChoose.{e?.PropertyName} change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " CONTROL EVENT HANDLERS "

    ''' <summary> Event handler. Called by _clearSelectedResourceButton for click events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearSelectedResourceButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearSelectedResourceButton.Click
        ' Transmit the SDC command to the interface.
        If Not String.IsNullOrWhiteSpace(Me._InstrumentChooser.SelectedResourceName) Then
            Try
                Me.Cursor = Windows.Forms.Cursors.WaitCursor
                If Me.IsInterfaceOpen Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                       "Clearing selected device;. Clearing '{0}'...", Me._InstrumentChooser.SelectedResourceName)
                    Me.VisaInterface.SelectiveDeviceClear(Me._InstrumentChooser.SelectedResourceName)
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                       "Cleared selected device;. Cleared '{0}'.", Me._InstrumentChooser.SelectedResourceName)
                End If
            Catch ex As Exception
                Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   $"Exception clearing '{Me._InstrumentChooser.SelectedResourceName}';. {ex.ToFullBlownString}")
            Finally
                Me.Cursor = Windows.Forms.Cursors.Default
            End Try
        End If
    End Sub

    ''' <summary> Event handler. Called by _clearAllResourcesButton for click events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearAllResourcesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ClearAllResourcesButton.Click
        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            If Me.IsInterfaceOpen Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Clearing devices;. Clearing devices at '{0}'...", Me.InterfaceResourceName)
                Me._VisaInterface.ClearDevices()
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Cleared devices;. Cleared devices at '{0}'.", Me.InterfaceResourceName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception clearing devices at '{Me.InterfaceResourceName}';. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub

#End Region

#Region " INSTRUMENT PROPERTIES AND METHODS "

    ''' <summary> Gets the instrument chooser. </summary>
    ''' <value> The instrument chooser. </value>
    Public ReadOnly Property InstrumentChooser As ResourceSelectorConnector
        Get
            Return Me._InstrumentChooser
        End Get
    End Property

    ''' <summary> Displays instrument resource names. </summary>
    Private Sub DisplayResourceNames()
        If Me.VisaInterface Is Nothing Then
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Finding resources;. Finding resources {0}", VI.ResourceNamesManager.BuildInstrumentFilter())
            Me._InstrumentChooser.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter()
        Else
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Finding resources;. Finding resources for interface {0}", Me._InterfaceChooser.SelectedResourceName)
            Me._InstrumentChooser.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter(Me.VisaInterface.HardwareInterfaceType,
                                                                                                  Me.VisaInterface.HardwareInterfaceNumber)
        End If
        Me._InstrumentChooser.DisplayResourceNames()
        Me._InstrumentChooser.Enabled = Me._InstrumentChooser.HasResources
        If Me._InstrumentChooser.HasResources Then
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Instruments available--select and connect;. Found Instruments.")
        Else
            Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                               "No Instruments;. No Instruments were found. Connect the Instrument(s) and click Find.")
        End If
    End Sub

    ''' <summary> Displays resource names. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _InstrumentChooser_FindNames(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InstrumentChooser.FindNames
        Me.displayResourceNames()
    End Sub

    ''' <summary> Executes the property changed action. </summary>
    ''' <param name="sender">       Specifies the object where the call originated. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnInstrumentChooserPropertyChanged(ByVal sender As ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.SelectedResourceName)
                Me._ClearSelectedResourceButton.Enabled = Not String.IsNullOrWhiteSpace(sender.SelectedResourceName)
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Instrument resource selected--connect;. Selected resource {0}",
                                   sender.SelectedResourceName)
            Case NameOf(sender.SelectedResourceExists)
            Case NameOf(sender.ResourcesFilter)
            Case NameOf(sender.IsConnected)
            Case NameOf(sender.Clearable)
            Case NameOf(sender.Connectable)
            Case NameOf(sender.Searchable)
            Case NameOf(sender.HasResources)
                Me._ClearSelectedResourceButton.Enabled = sender.HasResources AndAlso Not String.IsNullOrWhiteSpace(sender.SelectedResourceName)
                Me._ClearAllResourcesButton.Enabled = sender.HasResources
        End Select
    End Sub

    ''' <summary> Event handler. Called by _ResourceChooser for property changed events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InstrumentChooser_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InstrumentChooser.PropertyChanged
        If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._InterfaceChooser_PropertyChanged), New Object() {sender, e})
        End If
        Try
            Me.OnInstrumentChooserPropertyChanged(TryCast(sender, ResourceSelectorConnector), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling InstrumentChooser.{e?.PropertyName} change event;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds the listeners such as the current trace messages box. </summary>
    Protected Overloads Sub AddListeners()
        Me.Talker.Listeners.Add(Me._TraceMessagesBox)
    End Sub

    ''' <summary> Handles the <see cref="_TraceMessagesBox"/> property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(sender As TraceMessagesBox, propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
            If String.Equals(propertyName, NameOf(sender.StatusPrompt)) Then
                Me._StatusLabel.Text = sender.StatusPrompt
                Me._StatusLabel.ToolTipText = sender.StatusPrompt
            End If
    End Sub

    ''' <summary> Trace messages box property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceMessagesBox_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _TraceMessagesBox.PropertyChanged
        Try
            ' there was a cross thread exception because this event is invoked from the control thread.
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._TraceMessagesBox_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, TraceMessagesBox), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Failed reporting Trace Message Property Change;. {ex.ToFullBlownString}")
        End Try
    End Sub


#End Region

End Class

