Imports isr.Core.Pith.StackTraceExtensions
''' <summary> Defines a subsystem for handing TSP Link and multiple nodes. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/1/2016" by="David" revision=""> Based on legacy status subsystem. </history>
Public MustInherit Class LinkSubsystem
    Inherits VI.SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystem">TSP status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As Scpi.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.TspLinkResetTimeout = TimeSpan.FromMilliseconds(5000)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Clears Error Queue.
    '''           </para></remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        ' 01/07/16: Moved from the Master Device.
        Me.UsingTspLink = False
        If Me.IsControllerNode Then
            ' establish the current node as the controller node. 
            Me.InitiateControllerNode()
        End If
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()

        MyBase.ResetKnownState()

        Me._NodeEntities = New NodeEntityCollection()
        Me.ControllerNodeNumber = New Integer?
        Me.ControllerNode = Nothing

        ' these values are set upon loading or initializing the framework.
        Me.TspLinkOnlineStateQueryCommand = ""
        Me.TspLinkOfflineStateQueryCommand = ""
        Me.TspLinkResetCommand = ""
        Me.ResetNodesCommand = ""

        Me.IsTspLinkOnline = New Boolean?
        Me.IsTspLinkOffline = New Boolean?

    End Sub

#End Region

#Region " ERROR QUEUE: NODE "

    ''' <summary> Queries error count on a remote node. </summary>
    ''' <exception cref="NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <param name="node"> . </param>
    ''' <returns> The error count. </returns>
    Public Function QueryErrorQueueCount(ByVal node As NodeEntityBase) As Integer
        Dim count As Integer?
        If node Is Nothing Then
            Throw New ArgumentNullException("node")
        End If
        If node.IsController Then
            count = Me.Session.QueryPrint(0I, 1, "_G.errorqueue.count")
        Else
            count = Me.Session.QueryPrint(0I, 1, "node[{0}].errorqueue.count", node.Number)
        End If
        Return count.GetValueOrDefault(0)
    End Function

    ''' <summary> Clears the error cache. </summary>
    Public Sub ClearErrorCache()
        Me.StatusSubsystem.ClearErrorCache()
        Me._DeviceErrorQueue = New Queue(Of DeviceError)
    End Sub

    ''' <summary> Clears the error queue for the specified node. </summary>
    ''' <param name="nodeNumber"> The node number. </param>
    Public Sub ClearErrorQueue(ByVal nodeNumber As Integer)
        If Not Me.NodeExists(nodeNumber) Then
            Me.Session.WriteLine("node[{0}].errorqueue.clear() waitcomplete({0})", nodeNumber)
        End If
    End Sub

    ''' <summary> Clears the error queue. </summary>
    Public Sub ClearErrorQueue()
        If Me.NodeEntities Is Nothing Then
            Me.StatusSubsystem.ClearErrorQueue()
        Else
            Me.ClearErrorCache()
            For Each node As NodeEntityBase In Me.NodeEntities
                Me.ClearErrorQueue(node.Number)
            Next
        End If
    End Sub

    Private _DeviceErrorQueue As Queue(Of DeviceError)
    ''' <summary> Gets or sets the error queue. </summary>
    ''' <value> A Queue of device errors. </value>
    Protected Shadows ReadOnly Property DeviceErrorQueue As Queue(Of DeviceError)
        Get
            Return Me._DeviceErrorQueue
        End Get
    End Property

    ''' <summary> Returns the queued error. </summary>
    ''' <exception cref="VI.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <remarks> Sends the error print format query and reads back and parses the error. </remarks>
    ''' <returns> The queued error. </returns>
    Public Shadows Function QueryQueuedError(ByVal node As NodeEntityBase) As DeviceError
        Dim err As New DeviceError()
        If Me.QueryErrorQueueCount(node) > 0 Then
            If node Is Nothing Then
                Throw New ArgumentNullException("node")
            End If
            Dim message As String = ""
            Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Querying queued device errors;. ")
            If node.IsController Then
                Me.Session.LastNodeNumber = node.ControllerNodeNumber
                message = Me.Session.QueryPrintStringFormatTrimEnd($"%d,%s,%d,node{node.Number}", "_G.errorqueue.next()")
            Else
                Me.Session.LastNodeNumber = node.Number
                message = Me.Session.QueryPrintStringFormatTrimEnd($"%d,%s,%d,node{node.Number}", $"node[{node.Number}].errorqueue.next()")
            End If
            Me.CheckThrowDeviceException(False, "getting queued error;. using {0}.", Me.Session.LastMessageSent)
            err = New DeviceError()
            err.Parse(message)
        End If
        Return err
    End Function

    ''' <summary> Reads the device errors. </summary>
    ''' <returns> <c>True</c> if device has errors, <c>False</c> otherwise. </returns>
    Public Shadows Function QueryDeviceErrors(ByVal node As NodeEntityBase) As String
        If node Is Nothing Then
            Throw New ArgumentNullException("node")
        End If
        Dim deviceError As DeviceError
        Do
            deviceError = Me.QueryQueuedError(node)
            If deviceError.IsError Then
                Me.DeviceErrorQueue.Enqueue(deviceError)
            End If
        Loop While Me.StatusSubsystem.ErrorAvailable()
        Dim message As New System.Text.StringBuilder
        If Me.DeviceErrorQueue IsNot Nothing AndAlso Me.DeviceErrorQueue.Count > 0 Then
            message.AppendFormat("Instrument {0} Node {1} Errors:", Me.ResourceName, node.Number)
            message.AppendLine()
            For Each e As DeviceError In Me.DeviceErrorQueue
                message.AppendLine(e.ErrorMessage)
            Next
        End If
        Me.StatusSubsystem.AppendDeviceErrorMessage(message.ToString)
        Return Me.StatusSubsystem.DeviceErrors
    End Function

    ''' <summary> Reads the device errors. </summary>
    ''' <returns> <c>True</c> if device has errors, <c>False</c> otherwise. </returns>
    Public Shadows Function QueryDeviceErrors() As String
        Me.ClearErrorCache()
        For Each node As NodeEntityBase In Me.NodeEntities
            Me.QueryDeviceErrors(node)
        Next
        Return Me.StatusSubsystem.DeviceErrors
    End Function

#End Region

#Region " OPC "

    ''' <summary> Enables group wait complete. </summary>
    ''' <param name="groupNumber"> Specifies the group number. That would be the same as the TSP
    ''' Link group number for the node. </param>
    Public Overloads Sub EnableWaitComplete(ByVal groupNumber As Integer)
        Me.StatusSubsystem.EnableWaitComplete()
        Me.Session.WriteLine(TspSyntax.WaitGroupCommandFormat, groupNumber)
    End Sub

    ''' <summary> Waits completion after command. </summary>
    ''' <exception cref="VI.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="nodeNumber"> Specifies the node number. </param>
    ''' <param name="timeout">    The timeout. </param>
    ''' <param name="isQuery">    Specifies the condition indicating if the command that preceded the wait is a query, which
    ''' determines how errors are fetched. </param>
    Public Sub WaitComplete(ByVal nodeNumber As Integer, ByVal timeout As TimeSpan, ByVal isQuery As Boolean)

        Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Enabling wait complete;. ")
        Me.Session.LastNodeNumber = nodeNumber
        Me.EnableWaitComplete(0)
        Me.CheckThrowDeviceException(Not isQuery, "enabled wait complete group '{0}';. ", 0)

        Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "waiting completion;. ")
        Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        Me.CheckThrowDeviceException(Not isQuery, "waiting completion;. ")
        Me.Session.LastNodeNumber = New Integer?

    End Sub

#End Region


#Region " COLLECT GARBAGE "

    ''' <summary> Collect garbage wait complete. </summary>
    ''' <remarks> David, 1/11/2016. </remarks>
    ''' <param name="node">    Specifies the remote node number to validate. </param>
    ''' <param name="timeout"> Specifies the time to wait for the instrument to return operation
    '''                        completed. </param>
    ''' <param name="format">  Describes the format to use. </param>
    ''' <param name="args">    A variable-length parameters list containing arguments. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function CollectGarbageWaitComplete(ByVal node As NodeEntityBase, ByVal timeout As TimeSpan,
                                               ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return Me._CollectGarbageWaitComplete(node, timeout, format, args)
    End Function

    ''' <summary> Does garbage collection. Reports operations synopsis. </summary>
    ''' <param name="node">    Specifies the remote node number to validate. </param>
    ''' <param name="timeout"> Specifies the time to wait for the instrument to return operation
    ''' completed. </param>
    ''' <param name="format">  Describes the format to use. </param>
    ''' <param name="args">    A variable-length parameters list containing arguments. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Function _CollectGarbageWaitComplete(ByVal node As NodeEntityBase, ByVal timeout As TimeSpan,
                                               ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        If node Is Nothing Then
            Throw New ArgumentNullException("node")
        End If
        If node.IsController Then
            Return Me.StatusSubsystem.CollectGarbageWaitComplete(timeout, format, args)
        End If

        Dim affirmative As Boolean = True
        ' do a garbage collection
        Try
            Me.Session.WriteLine(TspSyntax.CollectNodeGarbageFormat, node.Number)
            affirmative = Me.TraceVisaDeviceOperationOkay(node.Number, True, "collecting garbage after {0};. ",
                                                          String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
        Catch ex As NativeException
            Me.TraceVisaOperation(ex, node.Number, "collecting garbage after {0};. ",
                                  String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
            affirmative = False
        Catch ex As Exception
            Me.TraceOperation(ex, node.Number, "collecting garbage after {0};. ",
                              String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
            affirmative = False
        End Try


        Try
            If affirmative Then
                Me.EnableWaitComplete(0)
                Me.StatusSubsystem.AwaitOperationCompleted(timeout)
                affirmative = Me.TraceVisaDeviceOperationOkay(True, "awaiting completion after collecting garbage after {0};. ",
                                                              String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
            End If
        Catch ex As NativeException
            Me.TraceVisaOperation(ex, "awaiting completion after collecting garbage after {0};. ",
                                  String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
            affirmative = False
        Catch ex As Exception
            Me.TraceOperation(ex, "awaiting completion after collecting garbage after {0};. ",
                              String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
            affirmative = False
        End Try
        Return affirmative

    End Function

#End Region

#Region " DATA QUEUE "

    ''' <summary> clears the data queue for the specified node. </summary>
    ''' <param name="node">                Specifies the node. </param>
    ''' <param name="reportQueueNotEmpty"> true to report queue not empty. </param>
    Public Sub ClearDataQueue(ByVal node As NodeEntityBase, ByVal reportQueueNotEmpty As Boolean)
        If node Is Nothing Then
            Throw New ArgumentNullException("node")
        End If
        If Me.NodeExists(node.Number) Then
            If reportQueueNotEmpty Then
                If Me.QueryDataQueueCount(node) > 0 Then
                    Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Data queue not empty on node {0};. ", node.Number)
                End If
            End If
            Me.Session.WriteLine("node[{0}].dataqueue.clear() waitcomplete({0})", node.Number)
        End If
    End Sub

    ''' <summary> clears the data queue for the specified node. </summary>
    ''' <param name="nodeNumber"> The node number. </param>
    Public Sub ClearDataQueue(ByVal nodeNumber As Integer)
        If Me.NodeExists(nodeNumber) Then
            Me.Session.WriteLine("node[{0}].dataqueue.clear() waitcomplete({0})", nodeNumber)
        End If
    End Sub

    ''' <summary> Clears data queue on all nodes. </summary>
    ''' <param name="nodeEntities">        The node entities. </param>
    ''' <param name="reportQueueNotEmpty"> true to report queue not empty. </param>
    Public Sub ClearDataQueue(ByVal nodeEntities As NodeEntityCollection, ByVal reportQueueNotEmpty As Boolean)
        If nodeEntities IsNot Nothing Then
            For Each node As NodeEntityBase In nodeEntities
                Me.ClearDataQueue(node, reportQueueNotEmpty)
            Next
        End If
    End Sub

    ''' <summary> Clears data queue on the nodes. </summary>
    ''' <param name="nodes"> The nodes. </param>
    Public Sub ClearDataQueue(ByVal nodes As NodeEntityCollection)
        If nodes IsNot Nothing Then
            For Each node As NodeEntityBase In nodes
                If node IsNot Nothing Then
                    Me.ClearDataQueue(node.Number)
                End If
            Next
        End If
    End Sub

    ''' <summary> Clears data queue on all nodes. </summary>
    Public Sub ClearDataQueue()
        Me.ClearDataQueue(Me.NodeEntities)
    End Sub

#Region " CAPACITY "

    ''' <summary> Queries the capacity of the data queue. </summary>
    ''' <returns> Capacity. </returns>
    Public Function QueryDataQueueCapacity(ByVal node As NodeEntityBase) As Integer
        If node Is Nothing Then
            Throw New ArgumentNullException("node")
        End If
        Return Me.QueryDataQueueCapacity(node.Number)
    End Function

    ''' <summary> Queries the capacity of the data queue. </summary>
    ''' <exception cref="VI.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="nodeNumber"> The node number. </param>
    ''' <returns> Capacity. </returns>
    Public Function QueryDataQueueCapacity(ByVal nodeNumber As Integer) As Integer
        If Me.NodeExists(nodeNumber) Then
            Return Me.Session.QueryPrint(0I, 1, $"node[{nodeNumber}].dataqueue.capacity")
        Else
            Return 0
        End If
    End Function

#End Region

#Region " COUNT "

    ''' <summary> Queries the data queue count. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node"> . </param>
    ''' <returns> Count. </returns>
    Public Function QueryDataQueueCount(ByVal node As NodeEntityBase) As Integer
        If node Is Nothing Then
            Throw New ArgumentNullException("node")
        End If
        Return Me.QueryDataQueueCount(node.Number)
    End Function

    ''' <summary> Queries the data queue count. </summary>
    ''' <exception cref="VI.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="nodeNumber"> The node number. </param>
    ''' <returns> Count. </returns>
    Public Function QueryDataQueueCount(ByVal nodeNumber As Integer) As Integer
        If Me.NodeExists(nodeNumber) Then
            Return Me.Session.QueryPrint(0I, 1, $"node[{nodeNumber}].dataqueue.count")
        End If
    End Function

#End Region

#End Region

#Region " NODE "

    ''' <summary> Resets the local TSP node. </summary>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False></c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function ResetNode() As Boolean
        Dim affirmative As Boolean = True
        Try
            Me.Session.WriteLine("localnode.reset()")
            affirmative = Me.TraceVisaDeviceOperationOkay(False, "resetting local TSP node;. ")
        Catch ex As NativeException
            Me.TraceVisaOperation(ex, "resetting local TSP node;. ")
            affirmative = False
        Catch ex As Exception
            Me.TraceOperation(ex, "resetting local TSP node;. ")
            affirmative = False
        End Try
        Return affirmative
    End Function

    ''' <summary> Resets a node. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node"> The node. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False></c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function ResetNode(ByVal node As NodeEntityBase) As Boolean

        If node Is Nothing Then
            Throw New ArgumentNullException("node")
        End If
        Dim affirmative As Boolean = True
        If node.IsController Then
            affirmative = Me.ResetNode()
        Else
            Try
                Me.Session.WriteLine("node[{0}].reset()", node.Number)
                affirmative = Me.TraceVisaDeviceOperationOkay(node.Number, "resetting TSP node {0};. ", node.Number)
            Catch ex As NativeException
                Me.TraceVisaOperation(ex, node.Number, "resetting TSP node {0};. ", node.Number)
                affirmative = False
            Catch ex As Exception
                Me.TraceOperation(ex, node.Number, "resetting TSP node {0};. ", node.Number)
                affirmative = False
            End Try
        End If
        Return affirmative

    End Function

    ''' <summary> Sets the connect rule on the specified node. </summary>
    ''' <param name="nodeNumber"> Specifies the remote node number. </param>
    ''' <param name="value">      true to value. </param>
    Public Sub ConnectRuleSetter(ByVal nodeNumber As Integer, ByVal value As Integer)
        Me.Session.WriteLine(TspSyntax.NodeConnectRuleSetterCommandFormat, nodeNumber, value)
    End Sub

#Region " RESET NODES "

    ''' <summary> Gets the reset nodes command. </summary>
    ''' <value> The reset nodes command. </value>
    Public Property ResetNodesCommand As String

    ''' <summary> Resets the TSP nodes. </summary>
    ''' <param name="timeout"> Specifies the time to wait for the instrument to return operation
    ''' completed. </param>
    ''' <exception cref="VI.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <exception cref="TimeoutException"> Thrown when a Timeout error condition occurs. </exception>
    Public Sub ResetNodes(ByVal timeout As TimeSpan)
        If Not String.IsNullOrWhiteSpace(Me.ResetNodesCommand) Then
            Me.StatusSubsystem.EnableWaitComplete()
            Me.Session.WriteLine("isr.node.reset() waitcomplete(0)")
            Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        End If
    End Sub

    ''' <summary> Try reset nodes. </summary>
    ''' <param name="timeout"> Specifies the time to wait for the instrument to return operation
    ''' completed. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False&gt;</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryResetNodes(ByVal timeout As TimeSpan) As Boolean
        Try
            Me.ResetNodes(timeout)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                               "Instrument '{0}' timed out resetting nodes;. Ignored.", Me.Session.ResourceName)
            Return False
        End Try
    End Function

#End Region
#End Region

#Region " CONTROLLER NODE "

    ''' <summary> Gets or sets the controller node model. </summary>
    ''' <value> The controller node model. </value>
    Public ReadOnly Property ControllerNodeModel As String
        Get
            If Me.ControllerNode Is Nothing Then
                Return ""
            Else
                Return Me.ControllerNode.ModelNumber
            End If
        End Get
    End Property

    Private _controllerNode As NodeEntityBase

    ''' <summary> Gets reference to the controller node. </summary>
    ''' <value> The controller node. </value>
    Public Property ControllerNode() As NodeEntityBase
        Get
            Return Me._controllerNode
        End Get
        Set(ByVal value As NodeEntityBase)
            If value Is Nothing Then
                If Me.ControllerNode IsNot Nothing Then
                    Me._controllerNode = value
                    Me.AsyncNotifyPropertyChanged(NameOf(Me.ControllerNode))
                End If
            ElseIf Me.ControllerNode Is Nothing OrElse Not value.Equals(Me.ControllerNode) Then
                Me._controllerNode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ControllerNode))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the is controller node. </summary>
    ''' <value> The is controller node. </value>
    ''' <remarks> Required to allow this subsystem to initialize properly. </remarks>
    Public Property IsControllerNode As Boolean

    ''' <summary> Initiates the controller node. </summary>
    ''' <remarks> This also clears the <see cref="NodeEntities">collection of nodes</see>. </remarks>
    ''' <exception cref="FormatException"> Thrown when the format of the model number cannot be
    ''' parse to a valid instrument family. </exception>
    Public Sub InitiateControllerNode()
        If Me.ControllerNode Is Nothing Then
            Me.QueryControllerNodeNumber()
            Me.ControllerNode = New NodeEntity(Me.ControllerNodeNumber.Value, Me.ControllerNodeNumber.Value)
            Me.ControllerNode.InitializeKnownState(Me.Session)
            Me.AsyncNotifyPropertyChanged(NameOf(Me.ControllerNodeModel))
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Initiated controller node #{3};. Instrument model {0} S/N={1} Firmware={2} enumerated on node.",
                               Me.ControllerNode.ModelNumber, Me.ControllerNode.SerialNumber,
                               Me.ControllerNode.FirmwareVersion, Me.ControllerNode.Number)
        End If
        Me._NodeEntities = New NodeEntityCollection()
        Me.NodeEntities.Add(Me.ControllerNode)
    End Sub

#Region " CONTROLLER NODE NUMBER "

    Private _ControllerNodeNumber As Integer?

    ''' <summary> Gets or sets the Controller (local) node number. </summary>
    ''' <value> The Controller (local) node number. </value>
    Public Property ControllerNodeNumber As Integer?
        Get
            Return Me._ControllerNodeNumber
        End Get
        Set(ByVal value As Integer?)
            If Not Nullable.Equals(value, Me.ControllerNodeNumber) Then
                Me._ControllerNodeNumber = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ControllerNodeNumber))
            End If
        End Set
    End Property

    ''' <summary> Reads the Controller node number. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <exception cref="InvalidCastException">     Thrown when an object cannot be cast to a
    ''' required type. </exception>
    ''' <exception cref="VI.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <returns> An Integer or Null of failed. </returns>
    Public Function QueryControllerNodeNumber() As Integer?
        Me.ControllerNodeNumber = Me.Session.QueryPrint(0I, 1, "_G.tsplink.node")
        Return Me.ControllerNodeNumber
    End Function

    ''' <summary> Reads the Controller node number. </summary>
    ''' <returns> An Integer or Null of failed. </returns>
    Public Function TryQueryControllerNodeNumber() As Integer?
        Try
            Me.QueryControllerNodeNumber()
        Catch ex As OperationFailedException
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, ex.ToString)
        Catch ex As InvalidCastException
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, ex.ToString)
        End Try
        Return Me.ControllerNodeNumber
    End Function

#End Region

#End Region

#Region " TSP: NODE ENTITIES "

    ''' <summary> Gets or sets the node entities. </summary>
    ''' <remarks> Required for reading the system errors. </remarks>
    Private _NodeEntities As NodeEntityCollection

    ''' <summary> Returns the enumerated list of node entities. </summary>
    ''' <returns> A list of. </returns>
    Public Function NodeEntities() As NodeEntityCollection
        Return Me._NodeEntities
    End Function

    ''' <summary> Gets the number of nodes detected in the system. </summary>
    ''' <value> The number of nodes. </value>
    Public ReadOnly Property NodeCount() As Integer
        Get
            If Me._NodeEntities Is Nothing Then
                Return 0
            Else
                Return Me._NodeEntities.Count
            End If
        End Get
    End Property

    ''' <summary> Adds a node entity. </summary>
    ''' <param name="nodeNumber"> The node number. </param>
    Private Sub AddNodeEntity(ByVal nodeNumber As Integer)
        If nodeNumber = Me.ControllerNodeNumber Then
            Me._NodeEntities.Add(Me.ControllerNode)
        Else
            Dim node As Tsp.NodeEntity = New NodeEntity(nodeNumber, Me.ControllerNodeNumber.Value)
            node.InitializeKnownState(Me.Session)
            Me._NodeEntities.Add(node)
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Added node #{3};. Instrument model {0} S/N={1} Firmware={2} enumerated on node.",
                               node.ModelNumber, node.SerialNumber, node.FirmwareVersion, node.Number)
        End If
    End Sub

    ''' <summary> Queries if a given node exists. </summary>
    ''' <remarks> Uses the <see cref="NodeEntities">node entities</see> as a cache and add to the cache
    ''' is not is not cached. </remarks>
    ''' <param name="nodeNumber"> Specifies the node number. </param>
    ''' <returns> <c>True</c> if node exists; otherwise, false. </returns>
    Public Function NodeExists(ByVal nodeNumber As Integer) As Boolean
        Dim affirmative As Boolean = True
        If Me._NodeEntities.Count > 0 AndAlso Me._NodeEntities.Contains(NodeEntityBase.BuildKey(nodeNumber)) Then
            Return affirmative
        Else
            If NodeEntity.NodeExists(Me.Session, nodeNumber) Then
                Me.AddNodeEntity(nodeNumber)
                affirmative = Me._NodeEntities.Count > 0 AndAlso Me._NodeEntities.Contains(NodeEntityBase.BuildKey(nodeNumber))
            Else
                affirmative = False
            End If
        End If
        Return affirmative
    End Function

    ''' <summary> Enumerates the collection of nodes on the TSP Link net. </summary>
    ''' <param name="maximumCount"> Specifies the maximum expected node number. There could be up to
    ''' 64 nodes on the TSP link. Specify 0 to use the maximum node count. </param>
    Public Sub EnumerateNodes(ByVal maximumCount As Integer)

        Me.InitiateControllerNode()
        If maximumCount > 1 Then
            For i As Integer = 1 To maximumCount
                If Not NodeEntity.NodeExists(Me.Session, i) Then
                    Me.AddNodeEntity(i)
                End If
            Next
        End If
        Me.AsyncNotifyPropertyChanged(NameOf(Me.NodeCount))
    End Sub

#End Region

#Region " TSP LINK "

    Private _usingTspLink As Boolean

    ''' <summary> Gets or sets the condition for using TSP Link. Must be affirmative otherwise TSP link
    ''' reset commands are ignored. </summary>
    ''' <value> The using tsp link. </value>
    Public Property UsingTspLink() As Boolean
        Get
            Return Me._usingTspLink
        End Get
        Set(ByVal value As Boolean)
            If value <> Me.UsingTspLink Then
                Me._usingTspLink = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.UsingTspLink))
            End If
        End Set
    End Property

#Region " TSP LINK ONLINE STATE "

    Private _isTspLinkOnline As Boolean?

    ''' <summary> gets or sets the sentinel indicating if the TSP Link System is ready. </summary>
    ''' <value> <c>null</c> if Not known; <c>True</c> if the tsp link is on line; otherwise <c>False</c>. </value>
    Public Property IsTspLinkOnline() As Boolean?
        Get
            Return Me._isTspLinkOnline
        End Get
        Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(value, Me.IsTspLinkOnline) Then
                Me._isTspLinkOnline = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsTspLinkOnline))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the tsp link on-line state query command. </summary>
    ''' <value> The tsp link on line state query command. </value>
    Public Property TspLinkOnlineStateQueryCommand As String

    ''' <summary> Reads tsp link on line state. </summary>
    ''' <returns> <c>null</c> if Not known; <c>True</c> if the tsp link is on line; otherwise
    ''' <c>False</c>. </returns>
    Public Function ReadTspLinkOnlineState() As Boolean?
        If Not String.IsNullOrWhiteSpace(Me.TspLinkOnlineStateQueryCommand) Then
            Me.IsTspLinkOnline = Me.Session.IsStatementTrue(Me.TspLinkOnlineStateQueryCommand)
        End If
        Return Me.IsTspLinkOnline
    End Function

#End Region

#Region " TSP LINK OFFLINE STATE "

    Private _isTspLinkOffline As Boolean?

    ''' <summary> gets or sets the sentinel indicating if the TSP Link System is ready. </summary>
    ''' <value> <c>null</c> if Not known; <c>True</c> if the tsp link is Off line; otherwise <c>False</c>. </value>
    Public Property IsTspLinkOffline() As Boolean?
        Get
            Return Me._isTspLinkOffline
        End Get
        Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(value, Me.IsTspLinkOffline) Then
                Me._isTspLinkOffline = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsTspLinkOffline))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the tsp link Off line state query command. </summary>
    ''' <value> The tsp link Off line state query command. </value>
    Public Property TspLinkOfflineStateQueryCommand As String

    ''' <summary> Reads tsp link Off line state. </summary>
    ''' <returns> <c>null</c> if Not known; <c>True</c> if the tsp link is Off line; otherwise
    ''' <c>False</c>. </returns>
    Public Function ReadTspLinkOfflineState() As Boolean?
        If Not String.IsNullOrWhiteSpace(Me.TspLinkOfflineStateQueryCommand) Then
            Me.IsTspLinkOnline = Me.Session.IsStatementTrue(Me.TspLinkOfflineStateQueryCommand)
        End If
        Return Me.IsTspLinkOffline
    End Function

#End Region

#Region " RESET "

    ''' <summary> Gets or sets the tsp link reset command. </summary>
    ''' <value> The tsp link reset command. </value>
    Public Property TspLinkResetCommand As String

    ''' <summary> Resets the TSP link and the ISR support framework. </summary>
    ''' <remarks> Requires loading the 'isr.tsplink' scripts. </remarks>
    ''' <param name="timeout">          The timeout. </param>
    ''' <param name="maximumNodeCount"> Number of maximum nodes. </param>
    Public Sub ResetTspLinkWaitComplete(ByVal timeout As TimeSpan, ByVal maximumNodeCount As Integer)

        Try

            Me.Session.StoreTimeout(timeout)

            If Not String.IsNullOrWhiteSpace(Me.TspLinkResetCommand) Then
                Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "resetting TSP Link;. ")
                Me.Session.LastNodeNumber = New Integer?
                ' do not condition the reset upon a previous reset.
                Me.StatusSubsystem.EnableWaitComplete()
                Me.Session.WriteLine(Me.TspLinkResetCommand)
                Me.StatusSubsystem.AwaitOperationCompleted(timeout)
                Me.CheckThrowDeviceException(False, "resetting TSP Link")
            End If

            ' clear the reset status
            Me.IsTspLinkOffline = New Boolean?
            Me.IsTspLinkOnline = New Boolean?
            Me.ReadTspLinkOnlineState()
            Me.EnumerateNodes(maximumNodeCount)

        Catch

            Throw

        Finally

            Me.Session.RestoreTimeout()

        End Try

    End Sub

    ''' <summary> Reset the TSP Link or just the first node if TSP link not defined. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="timeout">          The timeout. </param>
    ''' <param name="maximumNodeCount"> Number of maximum nodes. </param>
    ''' <param name="displaySubsystem"> The display subsystem. </param>
    ''' <param name="frameworkName">    Name of the framework. </param>
    Public Sub ResetTspLink(ByVal timeout As TimeSpan, ByVal maximumNodeCount As Integer,
                            ByVal displaySubsystem As DisplaySubsystemBase, ByVal frameworkName As String)
        If displaySubsystem Is Nothing Then
            Throw New ArgumentNullException("displaySubsystem")
        End If
        displaySubsystem.DisplayLine(1, "Resetting  {0}", frameworkName)
        displaySubsystem.DisplayLine(2, "Resetting TSP Link")
        Me.ResetTspLinkWaitComplete(timeout, maximumNodeCount)
        If Me.NodeCount <= 0 Then
            If Me.UsingTspLink Then
                Throw New VI.OperationFailedException("Instrument '{0}' failed resetting TSP Link--no nodes;. ",
                                                           Me.Session.ResourceName)
            Else
                Throw New VI.OperationFailedException("Instrument '{0}' failed setting master node;. ",
                                                           Me.Session.ResourceName)
            End If
        ElseIf Me.UsingTspLink AndAlso Not Me.IsTspLinkOnline Then
            Throw New VI.OperationFailedException("Instrument '{0}' failed resetting TSP Link;. TSP Link is not on line.",
                                                       Me.Session.ResourceName)
        End If
    End Sub

#End Region

#Region " TSP LINK STATE "

    Private Const OnlineState As String = "online"
    Private _TspLinkState As String

    ''' <summary> Gets or sets the state of the tsp link. </summary>
    ''' <value> The tsp state. </value>
    Public Property TspLinkState As String
        Get
            Return Me._TspLinkState
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.TspLinkState) Then
                Me._TspLinkState = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.TspLinkState))
                Me.IsTspLinkOnline = Me.TspLinkState.Equals(LinkSubsystem.OnlineState, StringComparison.OrdinalIgnoreCase)
                Me.IsTspLinkOffline = Not Me.IsTspLinkOnline.Value
            End If
        End Set
    End Property

    ''' <summary> Reads tsp link state. </summary>
    ''' <returns> The tsp link state. </returns>
    ''' <exception cref="VI.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.DeviceException"> Thrown when a device error condition occurs. </exception>
    Public Function QueryTspLinkState() As String
        Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Reading TSP Link state;. ")
        Me.Session.LastNodeNumber = New Integer?
        Me.TspLinkState = Me.Session.QueryPrintStringFormatTrimEnd("tsplink.state")
        Me.CheckThrowDeviceException(False, "getting tsp link state;. using {0}.", Me.Session.LastMessageSent)
        Return Me.TspLinkState
    End Function

#End Region

#Region " TSP LINK GROUP NUMBERS "

    ''' <summary> Assigns group numbers to the nodes. A unique group number is required for executing
    ''' concurrent code on all nodes. </summary>
    ''' <returns> <c>True</c> of okay; otherwise, <c>False</c>. </returns>
    ''' <history date="09/08/2009" by="David" revision="3.0.3538.x"> Allows setting groups
    ''' even if TSP Link is not on line. </history>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function AssignNodeGroupNumbers() As Boolean
        Dim affirmative As Boolean = True
        If Me.NodeEntities IsNot Nothing Then
            For Each node As NodeEntityBase In Me._NodeEntities
                Try
                    If Me.IsTspLinkOnline Then
                        Me.Session.WriteLine("node[{0}].tsplink.group = {0}", node.Number)
                    Else
                        Me.Session.WriteLine("localnode.tsplink.group = {0}", node.Number)
                    End If
                    affirmative = Me.TraceVisaDeviceOperationOkay(False, "assigning group to node number {0};. ", node.Number)
                Catch ex As NativeException
                    Me.TraceVisaOperation(ex, "assigning group to node number {0};. ", node.Number)
                    affirmative = False
                Catch ex As Exception
                    Me.TraceOperation(ex, "assigning group to node number {0};. ", node.Number)
                    affirmative = False
                End Try
                If Not affirmative Then Exit For
            Next
            'If Me.IsTspLinkOnline Then
            'End If
        End If
        Return affirmative

    End Function

#End Region

#Region " TSP LINK RESET "

    Public Property TspLinkResetTimeout As TimeSpan

    ''' <summary> Reset TSP Link with error reporting. </summary>
    ''' <history date="09/22/2009" by="David" revision="3.0.3552.x"> The procedure caused
    ''' error 1220 - TSP link failure on the remote instrument. The error occurred only if the
    ''' program was stopped and restarted without toggling power on the instruments. Waiting
    ''' completion of the previous task helped even though that task did not access the remote node! </history>
    Private Sub ResetTspLinkIgnoreError()
        Me.IsTspLinkOnline = New Boolean?
        Me.EnableWaitComplete(0)
        Me.StatusSubsystem.AwaitOperationCompleted(Me.TspLinkResetTimeout)
        Me.StatusSubsystem.EnableWaitComplete()
        Me.Session.WriteLine("tsplink.reset() waitcomplete(0) errorqueue.clear() waitcomplete()")
        Me.StatusSubsystem.AwaitOperationCompleted(Me.TspLinkResetTimeout)
    End Sub

    ''' <summary> Reset TSP Link with error reporting. </summary>
    ''' <returns> <c>True</c> of okay; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
    Private Function TryResetTspLinkReportError() As Boolean
        Me.IsTspLinkOnline = New Boolean?
        Dim affirmative As Boolean = True
        Try
            Me.StatusSubsystem.EnableWaitComplete()
            Me.Session.WriteLine("tsplink.reset() waitcomplete(0)")
            affirmative = Me.TraceVisaDeviceOperationOkay(False, "resetting TSP Link;. ")
        Catch ex As NativeException
            Me.TraceVisaOperation(ex, "resetting TSP Link;. ")
            affirmative = False
        Catch ex As Exception
            Me.TraceOperation(ex, "resetting TSP Link;. ")
            affirmative = False
        End Try
        If affirmative Then
            Try
                Me.StatusSubsystem.AwaitServiceRequest(ServiceRequests.RequestingService, TimeSpan.FromMilliseconds(1000))
                affirmative = Me.TraceVisaDeviceOperationOkay(False, "resetting TSP Link;. ")
            Catch ex As NativeException
                Me.TraceVisaOperation(ex, "awaiting completion after resetting TSP Link;. ")
                affirmative = False
            Catch ex As Exception
                Me.TraceOperation(ex, "awaiting completion after resetting TSP Link;. ")
                affirmative = False
            End Try
        Else
            Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                               "Instrument '{0}' failed resetting TSP Link;. {1}{2}",
                               Me.ResourceName, Environment.NewLine, New StackFrame(True).UserCallStack())
        End If
        Return affirmative

    End Function

    ''' <summary> Resets the TSP link if not on line. </summary>
    ''' <history date="09/08/2009" by="David" revision="3.0.3538.x"> Allows to complete TSP
    ''' Link reset even on failure in case we have a single node. </history>
    Public Sub ResetTspLink(ByVal maximumNodeCount As Integer)

        If Me.UsingTspLink Then
            Me.ResetTspLinkIgnoreError()
        End If

        If Me.IsTspLinkOnline Then
            ' enumerate all nodes.
            Me.EnumerateNodes(maximumNodeCount)
        Else
            ' enumerate the controller node.
            Me.EnumerateNodes(1)
        End If

        ' assign node group numbers.
        Me.AssignNodeGroupNumbers()

        ' clear the error queue on all nodes.
        Me.ClearErrorQueue()

        ' clear data queues.
        Me.ClearDataQueue(Me.NodeEntities)

    End Sub

#End Region

#End Region

#Region " CHECK AND REPORT "

    ''' <summary> Check and reports visa or device error occurred. Can only be used after receiving a
    ''' full reply from the instrument. </summary>
    ''' <param name="nodeNumber"> Specifies the remote node number to validate. </param>
    ''' <param name="format">     Specifies the report format. </param>
    ''' <param name="args">       Specifies the report arguments. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Overrides Function TraceVisaDeviceOperationOkay(ByVal nodeNumber As Integer, ByVal format As String,
                                                           ByVal ParamArray args() As Object) As Boolean
        Dim success As Boolean = Me.TraceVisaDeviceOperationOkay(nodeNumber, False, format, args)
        If success AndAlso (nodeNumber <> Me.ControllerNode.ControllerNodeNumber) Then
            success = Me.QueryErrorQueueCount(Me.ControllerNode) = 0
            Dim details As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
            If success Then
                Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Instrument {0} node {1} done {2}", Me.ResourceName, nodeNumber, details)
            Else
                Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                   "Instrument {0} node {1} encountered errors {2}Details: {3}{4}{5}",
                                   Me.ResourceName, nodeNumber, Me.StatusSubsystem.DeviceErrors, details,
                                   Environment.NewLine, New StackFrame(True).UserCallStack())
            End If
        End If
        Return success
    End Function

#End Region

End Class

