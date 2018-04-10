''' <summary> Encapsulates handling a device reported error. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class DeviceError

#Region " CONSTRUCTOR "

    ''' <summary> Initializes a new instance of the <see cref="DeviceError" /> class 
    '''           specifying no error. </summary>
    Public Sub New(ByVal noErrorCompoundMessage As String)
        MyBase.New()
        Me._NoErrorCompoundMessage = noErrorCompoundMessage
        Me._CompoundErrorMessage = noErrorCompoundMessage
        Me._ErrorNumber = 0
        Me._errorMessage = VI.Pith.Scpi.Syntax.NoErrorMessage
        Me._Severity = TraceEventType.Verbose
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="DeviceError" /> class. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub New(ByVal value As DeviceError)
        MyBase.New()
        If value Is Nothing Then
            Me._NoErrorCompoundMessage = VI.Pith.Scpi.Syntax.NoErrorCompoundMessage
            Me._CompoundErrorMessage = VI.Pith.Scpi.Syntax.NoErrorCompoundMessage
            Me._errorMessage = VI.Pith.Scpi.Syntax.NoErrorMessage
            Me._ErrorNumber = 0
        Else
            Me._NoErrorCompoundMessage = value.NoErrorCompoundMessage
            Me._CompoundErrorMessage = value.CompoundErrorMessage
            Me._errorMessage = value.ErrorMessage
            Me._ErrorNumber = value.ErrorNumber
        End If
    End Sub

    ''' <summary> Gets or sets the no error. </summary>
    ''' <value> The no error. </value>
    Public Shared ReadOnly Property NoError As DeviceError = New DeviceError(VI.Pith.Scpi.Syntax.NoErrorCompoundMessage)


#End Region

#Region " PARSE "

    ''' <summary> Parses the error message </summary>
    ''' <param name="compoundError"> The compound error. </param>
    Public Overridable Sub Parse(ByVal compoundError As String)
        If String.IsNullOrWhiteSpace(compoundError) Then
            Me.CompoundErrorMessage = ""
            Me.ErrorNumber = 0
            Me.ErrorMessage = ""
            Me._Severity = TraceEventType.Verbose
        Else
            Me._CompoundErrorMessage = compoundError
            Dim parts() As String = compoundError.Split(","c)
            If parts.Length > 1 Then
                If Not Integer.TryParse(parts(0), Me._ErrorNumber) Then
                    Me._ErrorNumber = Integer.MinValue
                End If
                Me.ErrorMessage = parts(1).Trim.Trim(""""c).Trim
            ElseIf Integer.TryParse(compoundError, Me.ErrorNumber) Then
                Me.ErrorMessage = compoundError
            Else
                Me.ErrorNumber = 0
                Me.ErrorMessage = compoundError
            End If
            Me._Severity = TraceEventType.Error
        End If
    End Sub

#End Region

#Region " ERROR INFO "

    ''' <summary> Builds error message. </summary>
    ''' <returns> A String. </returns>
    Public Overridable Function BuildErrorMessage() As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, "{0},{1}", Me.ErrorNumber, Me.ErrorMessage)
    End Function

    Private _NoErrorCompoundMessage As String

    ''' <summary> Gets or sets a message describing the no error compound message. </summary>
    ''' <value> A message describing the no error compound. </value>
    Public Property NoErrorCompoundMessage As String
        Get
            Return Me._NoErrorCompoundMessage
        End Get
        Protected Set(value As String)
            Me._NoErrorCompoundMessage = value
        End Set
    End Property

    ''' <summary> Gets a value indicating whether the error number represent and error. </summary>
    ''' <value> The is error. </value>
    Public ReadOnly Property IsError As Boolean
        Get
            Return Me.ErrorNumber <> 0
        End Get
    End Property

    ''' <summary> The error number. </summary>
    Private _ErrorNumber As Integer

    ''' <summary> Gets or sets (protected) the error number. </summary>
    ''' <value> The error number. </value>
    Public Property ErrorNumber As Integer
        Get
            Return Me._ErrorNumber
        End Get
        Protected Set(ByVal value As Integer)
            Me._ErrorNumber = value
        End Set
    End Property

    ''' <summary> Message describing the error. </summary>
    Private _ErrorMessage As String

    ''' <summary> Gets or sets (protected) the error message. </summary>
    ''' <value> A message describing the error. </value>
    Public Property ErrorMessage As String
        Get
            Return Me._errorMessage
        End Get
        Protected Set(ByVal value As String)
            Me._errorMessage = value
        End Set
    End Property

    ''' <summary> Message describing the compound error. </summary>
    Private _CompoundErrorMessage As String

    ''' <summary> Gets or sets (protected) the compound error message. </summary>
    ''' <value> A message describing the compound error. </value>
    Public Property CompoundErrorMessage As String
        Get
            Return Me._CompoundErrorMessage
        End Get
        Protected Set(ByVal value As String)
            Me._CompoundErrorMessage = value
        End Set
    End Property

    Private _Severity As TraceEventType

    ''' <summary> Gets or sets the severity. </summary>
    ''' <value> The severity. </value>
    Public Property Severity As TraceEventType
        Get
            Return Me._Severity
        End Get
        Protected Set(value As TraceEventType)
            Me._Severity = value
        End Set
    End Property

    ''' <summary> Returns a string that represents the current object. </summary>
    ''' <returns> A string that represents the current object. </returns>
    Public Overrides Function ToString() As String
        Return Me.CompoundErrorMessage
    End Function

#End Region

#Region " EQUALS "

    ''' <summary> Indicates whether the current <see cref="T:DeviceError"></see> value is equal to a
    ''' specified object. </summary>
    ''' <param name="obj"> An object. </param>
    ''' <returns> <c>True</c> if <paramref name="obj" /> and this instance are the same type and represent the
    ''' same value; otherwise, <c>False</c>. </returns>
    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, DeviceError))
    End Function

    ''' <summary> Indicates whether the current <see cref="T:DeviceError"></see> value is equal to a
    ''' specified object. </summary>
    ''' <remarks> The two Parameters are the same if they have the same actual and cached values. </remarks>
    ''' <param name="value"> The value to compare. </param>
    ''' <returns> <c>True</c> if the other parameter is equal to the current
    ''' <see cref="T:DeviceError"></see> value;
    ''' otherwise, <c>False</c>. </returns>
    Public Overloads Function Equals(ByVal value As DeviceError) As Boolean
        Return value IsNot Nothing AndAlso Me.CompoundErrorMessage = value.CompoundErrorMessage
    End Function

    ''' <summary> Returns a hash code for this instance. </summary>
    ''' <returns> A hash code for this object. </returns>
    Public Overloads Overrides Function GetHashCode() As Int32
        Return MyBase.GetHashCode
    End Function

    ''' <summary> Implements the operator =. </summary>
    ''' <param name="left">  The left. </param>
    ''' <param name="right"> The right. </param>
    ''' <returns> The result of the operation. </returns>
    Public Shared Operator =(ByVal left As DeviceError, ByVal right As DeviceError) As Boolean
        Return (CObj(left) Is CObj(right)) OrElse (left IsNot Nothing AndAlso left.Equals(right))
    End Operator

    ''' <summary> Implements the operator &lt;&gt;. </summary>
    ''' <param name="left">  The left. </param>
    ''' <param name="right"> The right. </param>
    ''' <returns> The result of the operation. </returns>
    Public Shared Operator <>(ByVal left As DeviceError, ByVal right As DeviceError) As Boolean
        Return ((CObj(left) IsNot CObj(right)) AndAlso (left Is Nothing OrElse Not left.Equals(right)))
    End Operator

#End Region

End Class

''' <summary> Queue of device errors. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/12/2016" by="David" revision=""> Created. </history>
Public Class DeviceErrorQueue
    Inherits Queue(Of DeviceError)

    ''' <summary> Constructor. </summary>
    ''' <param name="noErrorCompoundMessage"> A message describing the empty error message. </param>
    Public Sub New(ByVal noErrorCompoundMessage As String)
        MyBase.New
        Me._NoErrorCompoundMessage = noErrorCompoundMessage
    End Sub

    ''' <summary> Gets or sets a message describing the no error compound message. </summary>
    ''' <value> A message describing the no error compound. </value>
    Public ReadOnly Property NoErrorCompoundMessage As String

    ''' <summary> Gets the last error. </summary>
    ''' <value> The last error. </value>
    Public ReadOnly Property LastError As DeviceError
        Get
            If Me.Count = 0 Then
                Return New DeviceError(Me.NoErrorCompoundMessage)
            Else
                Return Me(Me.Count - 1)
            End If
        End Get
    End Property

End Class
