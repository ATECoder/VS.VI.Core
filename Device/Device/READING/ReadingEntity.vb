''' <summary> Implements a reading element. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/1/2013" by="David" revision=""> Created. </history>
Public Class ReadingEntity

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary>
    ''' Constructs a measured value without specifying the value or its validity, which must be
    ''' specified for the value to be made valid.
    ''' </summary>
    ''' <param name="readingType"> The type of the reading. </param>
    Public Sub New(ByVal readingType As ReadingTypes)
        MyBase.New()
        Me._ReadingType = readingType
    End Sub

    ''' <summary> Constructs a copy of an existing value. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As ReadingEntity)
        Me.New(ReadingTypes.None)
        If model IsNot Nothing Then
            Me._ReadingType = model.ReadingType
            Me._Heading = model.Heading
            Me._ValueReading = model.ValueReading
            Me._IncludesUnitsSuffix = model.IncludesUnitsSuffix
        End If
    End Sub

#End Region

#Region " SHARED "

    ''' <summary>
    ''' Remove unit characters from SCPI data. Some instruments append units to the end of the
    ''' fetched values. This methods removes alpha characters as well as the number sign which the
    ''' instruments append to the reading number.
    ''' </summary>
    ''' <param name="value"> A delimited string of values. </param>
    ''' <returns> A String. </returns>
    Public Shared Function TrimUnits(ByVal value As String) As String
        Return ReadingEntity.TrimUnits(value, ",")
    End Function

    ''' <summary> Remove unit characters from SCPI data. Some instruments append units to the end of
    ''' the fetched values. This methods removes alpha characters as well as the number sign which
    ''' instruments append to the reading number. </summary>
    ''' <param name="value"> A delimited string of values. </param>
    ''' <param name="delimiter"> The delimiter. </param>
    Public Shared Function TrimUnits(ByVal value As String, ByVal delimiter As String) As String
        Const unitCharacters As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ#"
        If String.IsNullOrWhiteSpace(delimiter) Then Throw New ArgumentNullException(NameOf(delimiter))
        Dim dataBuilder As New System.Text.StringBuilder
        If Not String.IsNullOrWhiteSpace(value) Then
            If value.Contains(delimiter) Then
                For Each dataElement As String In value.Split(delimiter.ToCharArray)
                    If dataBuilder.Length > 0 Then dataBuilder.Append(delimiter)
                    dataBuilder.Append(dataElement.TrimEnd(unitCharacters.ToCharArray))
                Next
            Else
                dataBuilder.Append(value.TrimEnd(unitCharacters.ToCharArray))
            End If
        End If
        Return dataBuilder.ToString
    End Function

#End Region

#Region " EQUALS "

    ''' <summary> = casting operator. </summary>
    ''' <param name="left">  The left hand side item to compare for equality. </param>
    ''' <param name="right"> The left hand side item to compare for equality. </param>
    ''' <returns> The result of the operation. </returns>
    Public Shared Operator =(ByVal left As ReadingEntity, ByVal right As ReadingEntity) As Boolean
        If left Is Nothing Then
            Return right Is Nothing
        ElseIf right Is Nothing Then
            Return False
        Else
            Return ReadingEntity.Equals(left, right)
        End If
    End Operator

    ''' <summary> &lt;&gt; casting operator. </summary>
    ''' <param name="left">  The left hand side item to compare for equality. </param>
    ''' <param name="right"> The left hand side item to compare for equality. </param>
    ''' <returns> The result of the operation. </returns>
    Public Shared Operator <>(ByVal left As ReadingEntity, ByVal right As ReadingEntity) As Boolean
        Return Not ReadingEntity.Equals(left, right)
    End Operator

    ''' <summary> Returns True if equal. </summary>
    ''' <remarks> Ranges are the same if the have the same
    ''' <see cref="Type">min</see> and <see cref="Type">max</see> values. </remarks>
    ''' <param name="left">  The left hand side item to compare for equality. </param>
    ''' <param name="right"> The left hand side item to compare for equality. </param>
    ''' <returns> <c>True</c> if equals. </returns>
    Public Overloads Shared Function Equals(ByVal left As ReadingEntity, ByVal right As ReadingEntity) As Boolean
        If left Is Nothing Then
            Return right Is Nothing
        ElseIf right Is Nothing Then
            Return False
        Else
            Return String.Equals(left.ValueReading, right.ValueReading) AndAlso String.Equals(left.UnitsReading, right.UnitsReading)
        End If
    End Function

    ''' <summary> Determines whether the specified <see cref="T:System.Object" /> is equal to the
    ''' current <see cref="T:System.Object" />. </summary>
    ''' <param name="obj"> The <see cref="T:System.Object" /> to compare with the current
    ''' <see cref="T:System.Object" />. </param>
    ''' <returns> <c>True</c> if the specified <see cref="T:System.Object" /> is equal to the current
    ''' <see cref="T:System.Object" />; otherwise, <c>False</c>. </returns>
    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, ReadingEntity))
    End Function

    ''' <summary> Returns True if the value of the <paramref name="other"/> equals to the instance
    ''' value. </summary>
    ''' <remarks> Ranges are the same if the have the same
    ''' <see cref="Type">min</see> and <see cref="Type">max</see> values. </remarks>
    ''' <param name="other"> The other <see cref="ReadingEntity">Range</see> to compare for equality with this
    ''' instance. </param>
    ''' <returns> A Boolean data type. </returns>
    Public Overloads Function Equals(ByVal other As ReadingEntity) As Boolean
        If other Is Nothing Then
            Return False
        Else
            Return ReadingEntity.Equals(Me, other)
        End If
    End Function

    ''' <summary> Creates a unique hash code. </summary>
    ''' <returns> An <see cref="System.Int32">Int32</see> value. </returns>
    Public Overloads Overrides Function GetHashCode() As Int32
        Return Me.ValueReading.GetHashCode
    End Function

#End Region

#Region " RESET "

    ''' <summary> Resets value to nothing. </summary>
    Public Overridable Sub Reset()
        Me.ValueReading = ""
    End Sub

#End Region

#Region " READING "

    ''' <summary> Gets or sets the type of the reading. </summary>
    ''' <value> The type of the reading. </value>
    Public Property ReadingType As ReadingTypes

    ''' <summary> Applies the reading to create the specific reading type in the inherited class. </summary>
    ''' <param name="valueReading"> The value reading. </param>
    ''' <param name="unitsReading"> The units reading. </param>
    Public Overridable Function TryApplyReading(ByVal valueReading As String, ByVal unitsReading As String) As Boolean
        ' save the readings 
        If String.IsNullOrEmpty(valueReading) Then valueReading = ""
        If String.IsNullOrEmpty(unitsReading) Then valueReading = ""
        Me.ValueReading = valueReading
        Me.UnitsReading = unitsReading
        Return True
    End Function


    ''' <summary> Applies the reading to create the specific reading type in the inherited class. </summary>
    ''' <param name="valueReading"> The value reading. </param>
    Public Overridable Function TryApplyReading(ByVal valueReading As String) As Boolean
        If String.IsNullOrEmpty(valueReading) Then
            valueReading = ""
        End If
        Me.ValueReading = valueReading
        Return True
    End Function

    ''' <summary> Attempts to evaluate using the applied reading and given status. </summary>
    ''' <param name="reading"> The reading. </param>
    ''' <returns> <c>True</c> if evaluated. </returns>
    Public Overridable Function TryEvaluate(ByVal reading As Double) As Boolean
        Return True
    End Function

    ''' <summary> Attempts to evaluate using the applied reading and given status. </summary>
    ''' <returns> <c>True</c> if evaluated. </returns>
    Public Overridable Function TryEvaluate(ByVal status As Long) As Boolean
        Return True
    End Function

    ''' <summary> Returns a string that represents the current object. </summary>
    ''' <returns> A string that represents the current object. </returns>
    Public Overrides Function ToString() As String
        Return Me.ValueReading
    End Function

    ''' <summary> Gets or sets the sentinel indicating if the reading includes a units suffix. </summary>
    ''' <value> <c>True</c> if the reading includes units. </value>
    Public Property IncludesUnitsSuffix As Boolean

    ''' <summary> Gets or sets the value reading text. </summary>
    ''' <value> The value reading. </value>
    Public Property ValueReading() As String

    ''' <summary> Gets the length of the reading. </summary>
    ''' <value> The length of the reading. </value>
    Public Property ReadingLength As Integer

    ''' <summary> Gets or sets the heading. </summary>
    ''' <value> The heading. </value>
    Public Property Heading() As String

    ''' <summary> Gets or sets the units reading. </summary>
    ''' <value> The units reading. </value>
    Public Property UnitsReading() As String

#End Region

End Class