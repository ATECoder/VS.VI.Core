''' <summary> Implements a reading value. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/1/2013" by="David" revision=""> Created. </history>
Public Class ReadingValue
    Inherits ReadingEntity

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary>
    ''' Constructs a measured value without specifying the value or its validity, which must be
    ''' specified for the value to be made valid.
    ''' </summary>
    ''' <param name="readingType"> Type of the reading. </param>
    Public Sub New(ByVal readingType As ReadingTypes)
        MyBase.New(readingType)
        Me._generator = New RandomNumberGenerator
    End Sub

    ''' <summary> Constructs a copy of an existing value. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As ReadingValue)
        MyBase.New(model)
        If model IsNot Nothing Then
            Me._Value = model.Value
        End If
    End Sub

#End Region

#Region " EQUALS "

    ''' <summary> = casting operator. </summary>
    ''' <param name="left">  The left hand side item to compare for equality. </param>
    ''' <param name="right"> The left hand side item to compare for equality. </param>
    ''' <returns> The result of the operation. </returns>
    Public Overloads Shared Operator =(ByVal left As ReadingValue, ByVal right As ReadingValue) As Boolean
        If left Is Nothing Then
            Return right Is Nothing
        ElseIf right Is Nothing Then
            Return False
        Else
            Return ReadingValue.Equals(left, right)
        End If
    End Operator

    ''' <summary> &lt;&gt; casting operator. </summary>
    ''' <param name="left">  The left hand side item to compare for equality. </param>
    ''' <param name="right"> The left hand side item to compare for equality. </param>
    ''' <returns> The result of the operation. </returns>
    Public Overloads Shared Operator <>(ByVal left As ReadingValue, ByVal right As ReadingValue) As Boolean
        Return Not ReadingValue.Equals(left, right)
    End Operator

    ''' <summary> Returns True if equal. </summary>
    ''' <remarks> Ranges are the same if the have the same
    ''' <see cref="Type">min</see> and <see cref="Type">max</see> values. </remarks>
    ''' <param name="left">  The left hand side item to compare for equality. </param>
    ''' <param name="right"> The left hand side item to compare for equality. </param>
    ''' <returns> <c>True</c> if equals. </returns>
    Public Overloads Shared Function Equals(ByVal left As ReadingValue, ByVal right As ReadingValue) As Boolean
        If left Is Nothing Then
            Return right Is Nothing
        ElseIf right Is Nothing Then
            Return False
        Else
            Return Nullable.Equals(left.Value, right.Value) AndAlso String.Equals(left.ValueReading, right.ValueReading) AndAlso String.Equals(left.UnitsReading, right.UnitsReading)
        End If
    End Function

    ''' <summary> Determines whether the specified <see cref="T:System.Object" /> is equal to the
    ''' current <see cref="T:System.Object" />. </summary>
    ''' <param name="obj"> The <see cref="T:System.Object" /> to compare with the current
    ''' <see cref="T:System.Object" />. </param>
    ''' <returns> <c>True</c> if the specified <see cref="T:System.Object" /> is equal to the current
    ''' <see cref="T:System.Object" />; otherwise, <c>False</c>. </returns>
    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, ReadingValue))
    End Function

    ''' <summary> Returns True if the value of the <paramref name="other"/> equals to the instance
    ''' value. </summary>
    ''' <remarks> Ranges are the same if the have the same
    ''' <see cref="Type">min</see> and <see cref="Type">max</see> values. </remarks>
    ''' <param name="other"> The other <see cref="ReadingValue">Range</see> to compare for equality with this
    ''' instance. </param>
    ''' <returns> A Boolean data type. </returns>
    Public Overloads Function Equals(ByVal other As ReadingValue) As Boolean
        If other Is Nothing Then
            Return False
        Else
            Return ReadingValue.Equals(Me, other)
        End If
    End Function

    ''' <summary> Creates a unique hash code. </summary>
    ''' <returns> An <see cref="System.Int32">Int32</see> value. </returns>
    Public Overloads Overrides Function GetHashCode() As Int32
        Return Me.Value.GetHashCode
    End Function

#End Region

#Region " VALUE "

    ''' <summary> Gets or sets the value. </summary>
    ''' <value> The value. </value>
    Public Property Value As Double?

    ''' <summary> Resets value to nothing. </summary>
    Public Overrides Sub Reset()
        MyBase.Reset()
        Me._Value = New Double?
    End Sub

    ''' <summary> Applies the reading to create the specific reading type in the inherited class. </summary>
    ''' <param name="valueReading"> The value reading. </param>
    ''' <param name="unitsReading"> The units reading. </param>
    ''' <returns> <c>True</c> if parsed. </returns>
    Public Overrides Function TryApplyReading(ByVal valueReading As String, ByVal unitsReading As String) As Boolean
        If MyBase.TryApplyReading(valueReading, unitsReading) Then
            ' convert reading to numeric
            Return Me.TryApplyReading(valueReading)
        Else
            Return False
        End If
    End Function

    ''' <summary> Applies the reading to create the specific reading type in the inherited class. </summary>
    ''' <param name="valueReading"> The value reading. </param>
    ''' <returns> <c>True</c> if parsed. </returns>
    Public Overrides Function TryApplyReading(ByVal valueReading As String) As Boolean
        If MyBase.TryApplyReading(valueReading) Then
            ' convert reading to numeric
            Dim value As Double
            If Double.TryParse(valueReading, Globalization.NumberStyles.Number Or Globalization.NumberStyles.AllowExponent,
                               Globalization.CultureInfo.CurrentCulture, value) Then
                Me._Value = value
                Return True
            Else
                Me._Value = VI.Pith.Scpi.Syntax.NotANumber
                Return False
            End If
        Else
            Me._Value = New Double?
            Return False
        End If
    End Function

#End Region

#Region " TO STRING "

    ''' <summary> Returns a string that represents the current object. </summary>
    ''' <returns> A string that represents the current object. </returns>
    Public Overrides Function ToString() As String
        If Me.Value.HasValue Then
            Return Me.Value.Value.ToString()
        Else
            Return Me.ValueReading
        End If
    End Function

#End Region

#Region " SIMULATION "

    Private _Generator As RandomNumberGenerator

    ''' <summary> Gets the generator. </summary>
    ''' <value> The generator. </value>
    Public ReadOnly Property Generator() As RandomNumberGenerator
        Get
            Return Me._generator
        End Get
    End Property

    ''' <summary> Holds the simulated value. </summary>
    ''' <value> The simulated value. </value>
    Public ReadOnly Property SimulatedValue() As Double
        Get
            Return Me._generator.Value
        End Get
    End Property

#End Region

End Class