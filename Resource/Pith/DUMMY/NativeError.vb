Imports isr.Core.Pith.SplitExtensions
''' <summary> A dummy native error. </summary>
''' <remarks> David, 11/24/2015. </remarks>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="11/24/2015" by="David" revision=""> Created. </history>
Public Class DummyNativeError
    Inherits isr.VI.NativeErrorBase

    ''' <summary> Constructor. </summary>
    ''' <remarks> David, 11/24/2015. </remarks>
    ''' <param name="errorCode"> The error code. </param>
    Public Sub New(ByVal errorCode As Integer)
        MyBase.New(errorCode)
        Me.InitErrorCode(errorCode)
    End Sub

    ''' <summary> Initializes the error code. </summary>
    ''' <remarks> David, 11/25/2015. </remarks>
    ''' <param name="errorCode"> The error code. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub InitErrorCode(ByVal errorCode As Integer)
        Try
            Me._ErrorCodeName = CStr(errorCode)
        Catch ex As Exception
            Me._ErrorCodeName = "UnknownError"
            Debug.Assert(Not Debugger.IsAttached, "Check the code")
        End Try
        Me._ErrorCodeDescription = Me._ErrorCodeName.SplitWords
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <remarks> David, 11/24/2015. </remarks>
    ''' <param name="errorCode">       The error code. </param>
    ''' <param name="resourceName">    Name of the resource. </param>
    ''' <param name="lastMessageSent"> The last message sent. </param>
    ''' <param name="lastAction">      The last action. </param>
    Public Sub New(ByVal errorCode As Integer, ByVal resourceName As String,
                      ByVal lastMessageSent As String, ByVal lastAction As String)
        MyBase.New(errorCode, resourceName, lastMessageSent, lastAction)
        Me.InitErrorCode(errorCode)
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <remarks> David, 11/24/2015. </remarks>
    ''' <param name="errorCode">       The error code. </param>
    ''' <param name="resourceName">    Name of the resource. </param>
    ''' <param name="nodeNumber">      The node number. </param>
    ''' <param name="lastMessageSent"> The last message sent. </param>
    ''' <param name="lastAction">      The last action. </param>
    Public Sub New(ByVal errorCode As Integer, ByVal resourceName As String,
                      ByVal nodeNumber As Integer, ByVal lastMessageSent As String,
                      ByVal lastAction As String)
        MyBase.New(errorCode, resourceName, nodeNumber, lastMessageSent, lastAction)
        Me.InitErrorCode(errorCode)
    End Sub

    Private Shared _Success As DummyNativeError

    ''' <summary> Gets the success. </summary>
    ''' <value> The success. </value>
    Public Shared ReadOnly Property Success As DummyNativeError
        Get
            If DummyNativeError._Success Is Nothing Then
                DummyNativeError._Success = New DummyNativeError(0)
            End If
            Return DummyNativeError._Success
        End Get
    End Property

    Private _ErrorCodeDescription As String

    ''' <summary> Gets information describing the error code. </summary>
    ''' <value> Information describing the error code. </value>
    Public Overrides ReadOnly Property ErrorCodeDescription As String
        Get
            Return Me._ErrorCodeDescription
        End Get
    End Property

    Private _ErrorCodeName As String

    ''' <summary> Gets the name of the error code. </summary>
    ''' <value> The name of the error code. </value>
    Public Overrides ReadOnly Property ErrorCodeName As String
        Get
            Return Me._ErrorCodeName
        End Get
    End Property

End Class
