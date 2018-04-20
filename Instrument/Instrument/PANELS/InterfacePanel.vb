Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
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

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.InitializingComponents = True
        Me.InitializeComponent()
        Me.InitializingComponents = False


        ' disable until the connectible interface is set
        Me.Enabled = False
        Me._InterfaceChooser.Connectable = True
        Me._InterfaceChooser.Clearable = True
        Me._InterfaceChooser.Searchable = True

        Me._InstrumentChooser.Connectable = True
        Me._InstrumentChooser.Clearable = True
        Me._InstrumentChooser.Searchable = True

        Me._TraceMessagesBox.ContainerPanel = Me._MessagesTabPage
        Me.AddPrivateListeners()

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
                ' removes the private text box listener; ?might as well remove all private listeners
                Me.RemovePrivateListener(Me._TraceMessagesBox)
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
        Catch ex As VI.Pith.OperationFailedException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{ex.Message} occurred opening interface;. {ex.ToFullBlownString}")
            Return False
        End Try
        Return Me.IsInterfaceOpen
    End Function

    ''' <summary> Opens the interface session. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="resourceName"> Name of the resource. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Sub OpenInterfaceSession(ByVal resourceName As String)
        If Me.Enabled Then
            Try
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Opening interface to {0};. ", resourceName)
                Me.VisaInterface = isr.VI.SessionFactory.Get.Factory.CreateGpibInterfaceSession()
                Me.VisaInterface.OpenSession(resourceName)
                If Me.IsInterfaceOpen Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Interface '{0}' opened;. ", resourceName)
                    Me.displayResourceNames()
                    If Me._InterfaceChooser.SessionFactory.HasResources Then
                        ' if has resources to display, move on. The chooser traps the errors.
                    Else
                        Me.TryCloseInterfaceSession()
                        Throw New VI.Pith.OperationFailedException($"Failed listing resources for interface '{resourceName}'; ")
                    End If
                Else
                    Me.TryCloseInterfaceSession()
                    Throw New VI.Pith.OperationFailedException($"Failed opening interface '{resourceName}'.")
                End If
            Catch ex As VI.Pith.OperationFailedException
                Throw
            Catch ex As VI.Pith.NativeException
                Me.TryCloseInterfaceSession()
                Throw New VI.Pith.OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture,
                                                                 "VISA exception occurred opening interface '{0}'.", resourceName),
                                                             ex)
            Catch ex As Exception
                Me.TryCloseInterfaceSession()
                Throw New VI.Pith.OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture,
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
                    .Enabled = Me.IsInterfaceOpen AndAlso Me._InstrumentChooser.SessionFactory.HasResources AndAlso
                        Not String.IsNullOrWhiteSpace(Me._InstrumentChooser.SessionFactory.SelectedResourceName)
                    .Visible = True
                    .Invalidate()
                End With
                With Me._ClearAllResourcesButton
                    .Enabled = Me.IsInterfaceOpen AndAlso Me._InstrumentChooser.SessionFactory.HasResources
                    .Visible = True
                    .Invalidate()
                End With
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Me.SafePostPropertyChanged(NameOf(Instrument.InterfacePanel.IsInterfaceOpen))
                Windows.Forms.Application.DoEvents()
            End Try
        End If
    End Sub

    ''' <summary> Try close session. </summary>
    ''' <returns> <c>True</c> if session closed; otherwise <c>False</c>. </returns>
    Public Overridable Function TryCloseInterfaceSession() As Boolean
        Try
            Me.CloseInterfaceSession()
        Catch ex As VI.Pith.OperationFailedException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{ex.Message} occurred closing interface;. {ex.ToFullBlownString}")
            Return False
        End Try
        Return Not Me.IsInterfaceOpen
    End Function

    ''' <summary> Closes the session. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overridable Sub CloseInterfaceSession()
        If Me.Enabled Then
            If Me.VisaInterface IsNot Nothing Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Closing interface to {0};. ", Me._InterfaceChooser.SessionFactory.SelectedResourceName)
                Try
                    Me.VisaInterface.Dispose()
                    Try
                        ' Trying to null the session raises an ObjectDisposedException 
                        ' if session service request handler was not released. 
                        Me.VisaInterface = Nothing
                    Catch ex As ObjectDisposedException
                        Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
                    End Try
                Catch ex As VI.Pith.NativeException
                    Throw New VI.Pith.OperationFailedException("Exception occurred closing the interface.", ex)
                Catch ex As Exception
                    Throw New VI.Pith.OperationFailedException("Exception occurred closing the interface.", ex)
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
                    Me.SafePostPropertyChanged(NameOf(Instrument.InterfacePanel.IsInterfaceOpen))
                    Windows.Forms.Application.DoEvents()
                End Try
            End If
        End If
    End Sub

    ''' <summary> Gets or sets the visa interface. </summary>
    ''' <value> The visa interface. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property VisaInterface As VI.Pith.InterfaceSessionBase

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
            If Not String.Equals(value, Me.InterfaceResourceName, StringComparison.OrdinalIgnoreCase) Then
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
            Me._InterfaceChooser.SessionFactory.ResourcesFilter = VI.Pith.ResourceNamesManager.BuildInterfaceFilter()
            Me._InterfaceChooser.SessionFactory.EnumerateResources()
            If Me._InterfaceChooser.SessionFactory.HasResources Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Interfaces available--select and connect;. Found interfaces.")
            Else
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   "NO INTERFACES;. No interfaces were found. Connect the interface(s) and click Find.")
            End If
            Me.displayResourceNames()
            Me.Enabled = True
        Catch ex As System.ArgumentException
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
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
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{ex.Message} occurred clearing;. {ex.ToFullBlownString}")
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
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                           "Connecting {0};. ", Me._InterfaceChooser.SessionFactory.SelectedResourceName)
        Dim resourcename As String = Me._InterfaceChooser.SessionFactory.SelectedResourceName
        Me.OpenInterfaceSession(resourcename)
        ' cancel if failed to connect
        If Not Me.IsInterfaceOpen Then e.Cancel = True
    End Sub

    ''' <summary> Disconnects the interface. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Specifies the event arguments provided with the call. </param>
    Private Sub InterfaceChooser_Disconnect(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _InterfaceChooser.Disconnect
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                           "Disconnecting {0};. ", Me._InterfaceChooser.SessionFactory.SelectedResourceName)
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
    Private Overloads Sub HandlePropertyChange(ByVal sender As ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SessionFactory.SelectedResourceName)
                Me.InterfaceResourceName = sender.SessionFactory.SelectedResourceName
                If sender.IsConnected Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Interface connected;. {Me.InterfaceResourceName}")
                Else
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Interface selected;. {Me.InterfaceResourceName}")
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _InterfaceChooser for property changed events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InterfaceChooser_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InterfaceChooser.PropertyChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Instrument.ResourceSelectorConnector)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._InterfaceChooser_PropertyChanged), New Object() {sender, e})
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

#Region " CONTROL EVENT HANDLERS "

    ''' <summary> Event handler. Called by _clearSelectedResourceButton for click events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ClearSelectedResourceButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearSelectedResourceButton.Click
        ' Transmit the SDC command to the interface.
        If Not String.IsNullOrWhiteSpace(Me._InstrumentChooser.SessionFactory.SelectedResourceName) Then
            Try
                Me.Cursor = Windows.Forms.Cursors.WaitCursor
                If Me.IsInterfaceOpen Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                       "Clearing selected device;. Clearing '{0}'...", Me._InstrumentChooser.SessionFactory.SelectedResourceName)
                    Me.VisaInterface.SelectiveDeviceClear(Me._InstrumentChooser.SessionFactory.SelectedResourceName)
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                       "Cleared selected device;. Cleared '{0}'.", Me._InstrumentChooser.SessionFactory.SelectedResourceName)
                End If
            Catch ex As Exception
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                   $"Exception clearing '{Me._InstrumentChooser.SessionFactory.SelectedResourceName}';. {ex.ToFullBlownString}")
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
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Clearing devices;. Clearing devices at '{0}'...", Me.InterfaceResourceName)
                Me._VisaInterface.ClearDevices()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Cleared devices;. Cleared devices at '{0}'.", Me.InterfaceResourceName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
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
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Finding resources;. Finding resources {0}", VI.Pith.ResourceNamesManager.BuildInstrumentFilter())
            Me._InstrumentChooser.SessionFactory.ResourcesFilter = VI.Pith.ResourceNamesManager.BuildInstrumentFilter()
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Finding resources;. Finding resources for interface {0}", Me._InterfaceChooser.SessionFactory.SelectedResourceName)
            Me._InstrumentChooser.SessionFactory.ResourcesFilter = VI.Pith.ResourceNamesManager.BuildInstrumentFilter(Me.VisaInterface.HardwareInterfaceType,
                                                                                                  Me.VisaInterface.HardwareInterfaceNumber)
        End If
        Me._InstrumentChooser.SessionFactory.EnumerateResources()
        Me._InstrumentChooser.Enabled = Me._InstrumentChooser.SessionFactory.HasResources
        If Me._InstrumentChooser.SessionFactory.HasResources Then
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Instruments available--select and connect;. Found Instruments.")
        Else
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
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
    Private Sub HandleInstrumentChooserPropertyChange(ByVal sender As ResourceSelectorConnector, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SessionFactory.SelectedResourceName)
                Me._ClearSelectedResourceButton.Enabled = Not String.IsNullOrWhiteSpace(sender.SessionFactory.SelectedResourceName)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Instrument resource selected--connect;. Selected resource {0}",
                                   sender.SessionFactory.SelectedResourceName)
            Case NameOf(VI.SessionFactory.HasResources)
                Me._ClearSelectedResourceButton.Enabled = sender.SessionFactory.HasResources AndAlso Not String.IsNullOrWhiteSpace(sender.SessionFactory.SelectedResourceName)
                Me._ClearAllResourcesButton.Enabled = sender.SessionFactory.HasResources
        End Select
    End Sub

    ''' <summary> Event handler. Called by _ResourceChooser for property changed events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InstrumentChooser_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InstrumentChooser.PropertyChanged
        If Me.InitializingComponents OrElse sender Is Nothing OrElse e Is Nothing Then Return
        Dim activity As String = $"handling {NameOf(Instrument.ResourceSelectorConnector)}.{e.PropertyName} change"
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf _InstrumentChooser_PropertyChanged), New Object() {sender, e})
            Else
                Me.HandleInstrumentChooserPropertyChange(TryCast(sender, Instrument.ResourceSelectorConnector), e.PropertyName)
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

    ''' <summary> Assign talker. </summary>
    Protected Overloads Sub AddPrivateListeners()
        Me.AddPrivateListener(Me._TraceMessagesBox)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overrides Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType)
        If listenerType = Me._TraceMessagesBox.ListenerType Then Me._TraceMessagesBox.ApplyTraceLevel(value)
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

    ''' <summary> Trace messages box property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TraceMessagesBox_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _TraceMessagesBox.PropertyChanged
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

