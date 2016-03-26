''' <summary> Implements a TSP based device. Defines the I/O driver for accessing the master node
''' of a TSP Linked system. </summary>
''' <license> (c) 2009 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/6/2013" by="David" revision=""> Based on legacy TSP library. </history>
''' <history date="02/21/2009" by="David" revision="3.0.3339.x"> Created </history>
Public MustInherit Class MasterDeviceBase
    Inherits VI.DeviceBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="MasterDeviceBase" /> class. </summary>
    Protected Sub New()
        MyBase.New()
        Me.InitializeTimeout = TimeSpan.FromMilliseconds(30000)
        Me.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter(HardwareInterfaceType.Gpib,
                                                                           HardwareInterfaceType.Tcpip,
                                                                           HardwareInterfaceType.Usb)
    End Sub

#Region " I Disposable Support"

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_DisplaySubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="Disposed @Subsystems")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="Disposed @Subsystems")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me.IsDeviceOpen Then Me.OnClosing(New System.ComponentModel.CancelEventArgs)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception disposing device", "Exception details: {0}", ex)
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Clears the active state. Issues selective device clear. </summary>
    Public Overrides Sub ClearActiveState()
        Me.StatusSubsystem.ClearActiveState()
    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        ' establish the current node as the controller node. 
        ' moved to tsp subsystem base with setting the conditional to true when creating the subsystem: 
        ' Me.StatusSubsystem.InitiateControllerNode()
    End Sub

#End Region

#Region " SESSION "

    ''' <summary> Allows the derived device to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnClosing(ByVal e As ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If e?.Cancel Then Return
        If Me.InteractiveSubsystem IsNot Nothing Then
            Try
                ' turn off prompts
                Me.InteractiveSubsystem.WriteShowPrompts(False)
                Me.InteractiveSubsystem.WriteShowErrors(False)
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, "Exception turning of prompts", "Details: {0}", ex)
            End Try
            Try
                ' set the state to closed
                Me.InteractiveSubsystem.ExecutionState = TspExecutionState.Closed
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, "Exception disposing", "Details: {0}", ex)
            End Try
        End If

        If Me._StatusSubsystem IsNot Nothing Then
            RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
        End If
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnOpening(ByVal e As ComponentModel.CancelEventArgs)
        MyBase.OnOpening(e)
        If e?.Cancel Then Return

        ' allow connection time to materialize
        Threading.Thread.Sleep(100)

    End Sub

    ''' <summary> Allows the derived device to take actions after opening. Adds subsystems and event
    ''' handlers. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub OnOpened()
        Try
            ' instantiate the Link Subsystem
            Me.LinkSubsystem.IsControllerNode = True

            ' instantiate the interactive subsystem
            Me.InteractiveSubsystem.ProcessExecutionStateEnabled = True

            MyBase.OnOpened()
            Me.StatusSubsystem.EnableServiceRequest(ServiceRequests.None)

        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Failed initiating controller node--closing this session;. Details: {0}.", ex)
            Me.CloseSession()
        End Try

    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " STATUS "

    ''' <summary> Gets or sets the Status Subsystem. </summary>
    ''' <value> The Status Subsystem. </value>
    Public Property StatusSubsystem As StatusSubsystemBase

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

    ''' <summary>
    ''' Gets or sets the System Subsystem.
    ''' </summary>
    ''' <value>The System Subsystem.</value>
    Public Property SystemSubsystem As SystemSubsystemBase

    ''' <summary> Gets or sets the Display Subsystem. </summary>
    ''' <value> Display Subsystem. </value>
    Public Property DisplaySubsystem As DisplaySubsystemBase

    ''' <summary> Gets or sets the Smu Subsystem. </summary>
    ''' <value> Smu Subsystem. </value>
    Public Property SourceMeasureUnit As VI.Tsp.SourceMeasureUnit

    ''' <summary> Gets or sets source measure unit current source. </summary>
    ''' <value> The source measure unit current source. </value>
    Public Property SourceMeasureUnitCurrentSource As SourceMeasureUnitCurrentSource

    ''' <summary> Gets or sets source measure unit measure. </summary>
    ''' <value> The source measure unit measure. </value>
    Public Property SourceMeasureUnitMeasure As SourceMeasureUnitMeasure

    ''' <summary> Gets or sets the link subsystem. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <value> The link subsystem. </value>
    Public Property LinkSubsystem As LinkSubsystem

    ''' <summary> Gets or sets the interactive subsystem. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <value> The interactive subsystem. </value>
    Public Property InteractiveSubsystem As InteractiveSubsystem

#End Region

#Region " CHECK CONTACTS "

    ''' <summary> Checks contact resistance. </summary>
    Public Sub CheckContacts(ByVal threshold As Integer)
        Me.SourceMeasureUnit.CheckContacts(threshold)
        If Not Me.SourceMeasureUnit.ContactCheckOkay.HasValue Then
            Throw New OperationFailedException("Failed Measuring contacts;. ")
        ElseIf Not Me.SourceMeasureUnit.ContactCheckOkay.Value Then
            Throw New OperationFailedException("High contact resistances;. Values: '{0}'", Me.SourceMeasureUnit.ContactResistances)
        End If
    End Sub

    ''' <summary> Checks contact resistance. </summary>
    ''' <param name="threshold"> The threshold. </param>
    ''' <param name="details">   [in,out] The details. </param>
    ''' <returns> <c>True</c> if contacts checked okay. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Function TryCheckContacts(ByVal threshold As Integer, ByRef details As String) As Boolean
        Me.SourceMeasureUnit.CheckContacts(threshold)
        If Not Me.SourceMeasureUnit.ContactCheckOkay.HasValue Then
            details = "Failed Measuring contacts;. "
            Return False
        ElseIf Me.SourceMeasureUnit.ContactCheckOkay.Value Then
            Return True
        Else
            details = String.Format("High contact resistances;. Values: '{0}'",
                                    Me.SourceMeasureUnit.ContactResistances)
            Return False
        End If
    End Function

#End Region

#Region " SERVICE REQUEST "

    ''' <summary> Reads the event registers after receiving a service request. </summary>
    Protected Overrides Sub ProcessServiceRequest()
        Me.StatusSubsystem.ReadRegisters()
        If Me.StatusSubsystem.ErrorAvailable Then
            Me.StatusSubsystem.QueryDeviceErrors()
        End If
    End Sub

#End Region

End Class
