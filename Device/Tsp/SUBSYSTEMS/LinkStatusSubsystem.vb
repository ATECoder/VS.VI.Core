''' <summary> A link status subsystem. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="3/28/2018" by="David" revision=""> Created. </history>
Public Class LinkStatusSubsystem
    Inherits VI.StatusSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="LinkStatusSubsystem" /> class. </summary>
    ''' <param name="session"> The session. </param>
    Public Sub New(ByVal session As VI.Pith.SessionBase)
        MyBase.New(session)
    End Sub

    ''' <summary> Creates a new StatusSubsystem. </summary>
    ''' <returns> A StatusSubsystem. </returns>
    Public Shared Function Create(ByVal session As Vi.Pith.SessionBase) As LinkStatusSubsystem
        Dim subsystem As LinkStatusSubsystem = Nothing
        Try
            subsystem = New LinkStatusSubsystem(session)
        Catch
            If subsystem IsNot Nothing Then
                subsystem.Dispose()
                subsystem = Nothing
            End If
            Throw
        End Try
        Return subsystem
    End Function

#End Region

#Region " I PRESETTABLE "

#If False Then
    ''' <summary> Clears the active state. Issues selective device clear. </summary>
    Public Overrides Sub ClearActiveState()
        ' A delay is required before issuing a device clear.
        ' First a 1ms delay was added. Without the delay, the instrument had error -286 TSP Runtime Error and User Abort error 
        ' if the clear command was issued after turning off the status _G.status.request_enable=0
        ' Thereafter, the instrument has a resource not found error when trying to connect after failing to connect 
        ' because instruments were off. Stopping here in debug mode seems to have alleviated this issue.  So,
        ' the delay was increased to 10 ms.
        Threading.Thread.Sleep(10)
        MyBase.ClearActiveState()
        Me.QueryOperationCompleted()
    End Sub
#End If

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " ERROR QUEUE: NODE "

    ''' <summary> Gets or sets the node entities. </summary>
    ''' <remarks> Required for reading the system errors. </remarks>
    Public ReadOnly Property NodeEntities As NodeEntityCollection

    Private _ErrorQueueCountQueryCommand As String = "_G.print(_G.string.format('%d',_G.errorqueue.count))"
    ''' <summary> Gets The ErrorQueueCount query command. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <value> The ErrorQueueCount query command. </value>
    Protected Overrides Property ErrorQueueCountQueryCommand As String
        Get
            Return Me._ErrorQueueCountQueryCommand
        End Get
        Set(value As String)
            Me._ErrorQueueCountQueryCommand = value
            MyBase.ErrorQueueCountQueryCommand = value
        End Set
    End Property


    ''' <summary> Queries error count on a remote node. </summary>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <param name="node"> . </param>
    ''' <returns> The error count. </returns>
    Public Overloads Function QueryErrorQueueCount(ByVal node As NodeEntityBase) As Integer
        Dim count As Integer?
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If node.IsController Then
            Me.ErrorQueueCountQueryCommand = "_G.print(_G.string.format('%d',_G.errorqueue.count))"
            'Me.ErrorQueueCountQueryCommand = "_G.print(_G.errorqueue.count)"
            'count = Me.Session.QueryPrint(0I, 1, "_G.errorqueue.count")
        Else
            Me.ErrorQueueCountQueryCommand = $"_G.print(_G.string.format('%d',node[{node.Number}].errorqueue.count))"
            'count = Me.Session.QueryPrint(0I, 1, "node[{0}].errorqueue.count", node.Number)
        End If
        count = Me.QueryErrorQueueCount()
        Return count.GetValueOrDefault(0)
    End Function

    ''' <summary> Clears the error cache. </summary>
    Public Overrides Sub ClearErrorCache()
        MyBase.ClearErrorCache()
        Me._DeviceErrorQueue = New Queue(Of DeviceError)
    End Sub

    Public Function NodeExists(ByVal nodeNumber As Integer) As Boolean
        Dim affirmative As Boolean = True
        If Me.NodeEntities.Count > 0 AndAlso Me.NodeEntities.Contains(NodeEntityBase.BuildKey(nodeNumber)) Then
            Return affirmative
        End If
        Return affirmative
    End Function

    ''' <summary> Clears the error queue for the specified node. </summary>
    ''' <param name="nodeNumber"> The node number. </param>
    Private Overloads Sub ClearErrorQueue(ByVal nodeNumber As Integer)
        If Not Me.NodeExists(nodeNumber) Then
            Me.Session.WriteLine("node[{0}].errorqueue.clear() waitcomplete({0})", nodeNumber)
        End If
    End Sub

    ''' <summary> Clears the error queue. </summary>
    Public Overrides Sub ClearErrorQueue()
        If Me.NodeEntities Is Nothing Then
            MyBase.ClearErrorQueue()
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
    Protected Overloads ReadOnly Property DeviceErrorQueue As Queue(Of DeviceError)
        Get
            Return Me._DeviceErrorQueue
        End Get
    End Property

    ''' <summary> Returns the queued error. </summary>
    ''' <exception cref="VI.Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <remarks> Sends the error print format query and reads back and parses the error. </remarks>
    ''' <returns> The queued error. </returns>
    Private Function QueryQueuedError(ByVal node As NodeEntityBase) As DeviceError
        Dim err As New DeviceError()
        If Me.QueryErrorQueueCount(node) > 0 Then
            If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
            Dim message As String = ""
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Querying queued device errors;. ")
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
    Private Overloads Function QueryDeviceErrors(ByVal node As NodeEntityBase) As String
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        Dim deviceError As DeviceError
        Do
            deviceError = Me.QueryQueuedError(node)
            If deviceError.IsError Then
                Me.DeviceErrorQueue.Enqueue(deviceError)
            End If
        Loop While Me.ErrorAvailable()
        Dim builder As New System.Text.StringBuilder
        If Me.DeviceErrorQueue IsNot Nothing AndAlso Me.DeviceErrorQueue.Count > 0 Then
            builder.AppendLine($"Instrument {Me.ResourceNameCaption} Node {node.Number} Errors:")
            builder.AppendLine()
            For Each e As DeviceError In Me.DeviceErrorQueue
                builder.AppendLine(e.ErrorMessage)
            Next
        End If
        Me.AppendDeviceErrorMessage(builder.ToString)
        Return Me.DeviceErrorsReport
    End Function

    ''' <summary> Reads the device errors. </summary>
    ''' <returns> <c>True</c> if device has errors, <c>False</c> otherwise. </returns>
    Protected Overrides Function QueryDeviceErrors() As String
        Me.ClearErrorCache()
        For Each node As NodeEntityBase In Me.NodeEntities
            Me.QueryDeviceErrors(node)
        Next
        Me.SafePostPropertyChanged(NameOf(VI.StatusSubsystemBase.DeviceErrorsReport))
        Return Me.DeviceErrorsReport
    End Function

#End Region

End Class
