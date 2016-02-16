''' <summary> VISA exception base. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/7/2013" by="David" revision="">            Updated from TSP Library. </history>
''' <history date="12/25/2010" by="David" revision="1.0.4011.x"> Created. </history>
<Serializable()>
Public Class NativeException
    Inherits ExceptionBase

#Region " STANDARD CONSTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="VI.NativeException"/> class. </summary>
    Public Sub New()
        Me.New("Native Exception")
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="VI.NativeException" /> class. </summary>
    ''' <param name="message"> The message. </param>
    Public Sub New(ByVal message As String)
        MyBase.New(message)
        Me._Timestamp = DateTime.Now
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="VI.NativeException" /> class. </summary>
    ''' <param name="message">        The message. </param>
    ''' <param name="innerException"> The inner exception. </param>
    Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
        MyBase.New(message, innerException)
    End Sub

    ''' <summary> Initializes a new instance of the class with serialized data. </summary>
    ''' <param name="info">    The <see cref="T:System.Runtime.Serialization.SerializationInfo" />
    ''' that holds the serialized object data about the exception being thrown. </param>
    ''' <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext" />
    ''' that contains contextual information about the source or destination. </param>
    Protected Sub New(ByVal info As Runtime.Serialization.SerializationInfo, ByVal context As Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
        If info Is Nothing Then
            Return
        End If
        Me._Timestamp = CType(info.GetValue("Timestamp", GetType(DateTime)), DateTime)
        Me._InnerError = CType(info.GetValue("InnerError", GetType(NativeErrorBase)), NativeErrorBase)
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
        info.AddValue("Timestamp", Me.Timestamp, GetType(DateTime))
        info.AddValue("InnerError", Me.InnerError)
        MyBase.GetObjectData(info, context)
    End Sub


#End Region

#Region " CUSTOM CONSTRUCTORS "

    ''' <summary> Constructor. </summary>
    ''' <remarks> David, 11/25/2015. </remarks>
    ''' <param name="innerError">     The inner error. </param>
    Public Sub New(ByVal innerError As NativeErrorBase)
        Me.New("Native exception")
        Me._InnerError = innerError
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <remarks> David, 11/25/2015. </remarks>
    ''' <param name="innerError">     The inner error. </param>
    ''' <param name="innerException"> The inner exception. </param>
    Public Sub New(ByVal innerError As NativeErrorBase, ByVal innerException As System.Exception)
        Me.New("Native exception", innerException)
        Me._InnerError = innerError
    End Sub

#End Region

#Region " CUSTOM PROPERTIES "

    ''' <summary> Gets the timestamp. </summary>
    ''' <value> The timestamp. </value>
    Public Property Timestamp As DateTime

    ''' <summary> Gets or sets the inner error. </summary>
    ''' <value> The inner error. </value>
    Public ReadOnly Property InnerError As NativeErrorBase

    ''' <summary> Convert this object into a string representation. </summary>
    ''' <remarks> David, 11/25/2015. </remarks>
    ''' <returns> A String that represents this object. </returns>
    Public Overrides Function ToString() As String
        Dim builder As New System.Text.StringBuilder(MyBase.ToString)
        builder.AppendLine("Native Error Info:")
        builder.AppendLine($"resource: {Me.InnerError.ResourceName};")
        If Me.InnerError.NodeNumber.HasValue Then builder.AppendLine($"node: {Me.InnerError.NodeNumber.Value};")
        builder.AppendLine($"error: {Me.InnerError.BuildErrorCodeDetails()};")
        builder.AppendLine($"action: {Me.InnerError.LastAction};")
        builder.AppendLine($"sent: {Me.InnerError.LastMessageSent};")
        Return builder.ToString()
    End Function

#End Region

End Class

