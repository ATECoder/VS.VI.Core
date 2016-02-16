''' <summary> Handles operation failed exceptions. </summary>
''' <remarks> Use this class to handle exceptions that might be thrown exercising open, close,
''' hardware access, and other similar operations. </remarks>
''' <license> (c) 2003 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="07/29/2003" by="David" revision="1.0.1305.x"> created. </history>
<Serializable()>
Public Class OperationFailedException
    Inherits ExceptionBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private Const defaultMessage As String = "Operation failed."

    ''' <summary> Initializes a new instance of the <see cref="T:OperationFailedException" /> class. Uses
    ''' the internal default message. </summary>
    Public Sub New()
        MyBase.New(OperationFailedException.defaultMessage)
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="T:OperationFailedException" /> class. </summary>
    ''' <param name="message"> The message. </param>
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="T:OperationFailedException" /> class. </summary>
    ''' <param name="message">        The message. </param>
    ''' <param name="innerException"> Specifies the exception that was trapped for throwing this
    ''' exception. </param>
    Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
        MyBase.New(message, innerException)
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="T:OperationFailedException" /> class. </summary>
    ''' <param name="format"> The format. </param>
    ''' <param name="args">   The arguments. </param>
    Public Sub New(ByVal format As String, ByVal ParamArray args() As Object)
        MyBase.New(format, args)
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="T:OperationFailedException" /> class. </summary>
    ''' <param name="innerException"> Specifies the InnerException. </param>
    ''' <param name="format">         Specifies the exception formatting. </param>
    ''' <param name="args">           Specifies the message arguments. </param>
    Public Sub New(ByVal innerException As System.Exception, ByVal format As String, ByVal ParamArray args() As Object)
        MyBase.New(innerException, format, args)
    End Sub

    ''' <summary> Initializes a new instance of the class with serialized data. </summary>
    ''' <param name="info">    The <see cref="T:System.Runtime.Serialization.SerializationInfo" />
    ''' that holds the serialized object data about the exception being thrown. </param>
    ''' <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext" />
    ''' that contains contextual information about the source or destination. </param>
    Protected Sub New(ByVal info As Runtime.Serialization.SerializationInfo, ByVal context As Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

#End Region

#Region " GET OBJECT DATA "

    ''' <summary> Overrides the GetObjectData method to serialize custom values. </summary>
    ''' <param name="info">    Represents the SerializationInfo of the exception. </param>
    ''' <param name="context"> Represents the context information of the exception. </param>
    <Security.Permissions.SecurityPermission(Security.Permissions.SecurityAction.Demand, SerializationFormatter:=True)>
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.GetObjectData(info, context)
    End Sub

#End Region

End Class

