''' <summary> Handles device exception including errors retrieved from the device. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="12/18/2013" by="David" revision="3.0.5100">   Updated from TSP Library. </history>
''' <history date="12/25/2010" by="David" revision="1.0.4011.x"> Created. </history>
<Serializable()> Public Class DeviceException
    Inherits ExceptionBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="DeviceException"/> class. </summary>
    Public Sub New()
        Me.New("Device Exception")
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="DeviceException" /> class. </summary>
    ''' <param name="message"> The message. </param>
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="DeviceException" /> class. </summary>
    ''' <param name="message">        The message. </param>
    ''' <param name="innerException"> The inner exception. </param>
    Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
        MyBase.New(message, innerException)
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="T:System.Exception" /> class with
    ''' serialized data. </summary>
    ''' <param name="info">    The <see cref="T:System.Runtime.Serialization.SerializationInfo" />
    ''' that holds the serialized object data about the exception being thrown. </param>
    ''' <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext" />
    ''' that contains contextual information about the source or destination. </param>
    Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
        ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
        Me._ResourceName = info.GetString("ResourceName")
        Me._LastAction = info.GetString("LastAction")
        Me._DeviceErrors = info.GetString("DeviceErrors")
        Me._NodeNumber = CType(info.GetValue("NodeNumber", GetType(Integer?)), Integer?)
    End Sub

    ''' <summary> Overrides the <see cref="GetObjectData" /> method to serialize custom values. </summary>
    ''' <param name="info">    The <see cref="Runtime.Serialization.SerializationInfo">serialization
    ''' information</see>. </param>
    ''' <param name="context"> The <see cref="Runtime.Serialization.StreamingContext">streaming
    ''' context</see> for the exception. </param>
    <Security.Permissions.SecurityPermission(Security.Permissions.SecurityAction.Demand, SerializationFormatter:=True)>
    Public Overrides Sub GetObjectData(ByVal info As Runtime.Serialization.SerializationInfo, ByVal context As Runtime.Serialization.StreamingContext)

        If info Is Nothing Then
            Return
        End If
        info.AddValue("ResourceName", Me.ResourceName, GetType(String))
        info.AddValue("NodeNumber", Me.NodeNumber, GetType(Integer?))
        info.AddValue("LastAction", Me.LastAction, GetType(String))
        info.AddValue("DeviceErrors", Me.DeviceErrors, GetType(String))
        MyBase.GetObjectData(info, context)
    End Sub

#End Region

#Region " CUSTOM CONSTRUCTORS "

    ''' <summary> Constructor specifying the Message to be set. </summary>
    ''' <param name="resourceName">              Name of the resource. </param>
    ''' <param name="format">                    Specifies the exception message format. </param>
    ''' <param name="args">                      Specifies the report argument. </param>
    Public Sub New(ByVal resourceName As String, ByVal format As String, ByVal ParamArray args() As Object)
        MyBase.New(String.Format(Globalization.CultureInfo.CurrentCulture,
                                 "{0} had errors {1}",
                                 resourceName,
                                 String.Format(Globalization.CultureInfo.CurrentCulture, format, args)))
    End Sub

    ''' <summary> Constructor specifying the Message to be set. </summary>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <param name="nodeNumber">   The node number. </param>
    ''' <param name="format">       Specifies the exception message format. </param>
    ''' <param name="args">         Specifies the report argument. </param>
    Public Sub New(ByVal resourceName As String, ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object)
        MyBase.New(String.Format(Globalization.CultureInfo.CurrentCulture,
                                 "{0} node {1} had errors {2}",
                                 resourceName, nodeNumber,
                                 String.Format(Globalization.CultureInfo.CurrentCulture, format, args)))
    End Sub

#End Region

#Region " CUSTOM PROPERTIES "

    ''' <summary> Gets or sets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Property ResourceName As String

    ''' <summary> Gets or sets the node number. </summary>
    ''' <value> The node number. </value>
    Public Property NodeNumber As Integer?

    ''' <summary> Gets or sets the last action. </summary>
    ''' <value> The last action. </value>
    Public Property LastAction As String

    ''' <summary> Gets or sets the device errors. </summary>
    ''' <value> The device errors. </value>
    Public Property DeviceErrors As String

    Public Overrides Function ToString() As String
        Dim builder As New System.Text.StringBuilder(MyBase.ToString())
        builder.AppendLine("Device error info:")
        builder.AppendLine($"resource: {Me.ResourceName};")
        If Me.NodeNumber.HasValue Then builder.AppendLine($"node: {Me.NodeNumber.Value};")
        builder.AppendLine($"action: {Me.LastAction};")
        builder.AppendLine($"errors: {Me.DeviceErrors}")
        Return builder.ToString
    End Function

    ''' <summary> Adds an exception data. </summary>
    ''' <param name="exception"> The exception receiving the added data.. </param>
    Public Sub AddExceptionData(ByVal exception As Exception)
        If exception Is Nothing Then Return
        Dim count As Integer = exception.Data.Count
        If Not String.IsNullOrWhiteSpace(Me.ResourceName) Then exception.Data.Add($"{count}-ResourceName", Me.ResourceName)
        If Me.NodeNumber.HasValue Then exception.Data.Add($"{count}-Node", Me.NodeNumber)
        If Not String.IsNullOrWhiteSpace(Me.LastAction) Then exception.Data.Add($"{count}-LastAction", Me.LastAction)
        If Me.DeviceErrors.Any Then
            Dim errorIndex As Integer = 0
            For Each s As String In Me.DeviceErrors
                If Not String.IsNullOrWhiteSpace(s) Then exception.Data.Add($"{count}-DeviceError({errorIndex})", s)
                errorIndex += 1
            Next
        End If
    End Sub
#End Region

End Class

