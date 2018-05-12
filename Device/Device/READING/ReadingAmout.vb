''' <summary> Implements a reading <see cref="Arebis.TypedUnits.Amount">amount</see>. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/1/2013" by="David" revision=""> Created. </history>
Public Class ReadingAmount
    Inherits ReadingValue

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Constructs a measured value without specifying the value or its validity, which must
    ''' be specified for the value to be made valid. </summary>
    Public Sub New(ByVal readingType As ReadingTypes)
        MyBase.New(readingType)
    End Sub

    ''' <summary> Constructs a copy of an existing value. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As ReadingAmount)
        MyBase.New(model)
        If model IsNot Nothing Then
            Me._Amount = model.Amount
            Me._Unit = model.Unit
        End If
    End Sub

#End Region

#Region " EQUALS "

    ''' <summary> = casting operator. </summary>
    ''' <param name="left">  The left hand side item to compare for equality. </param>
    ''' <param name="right"> The left hand side item to compare for equality. </param>
    ''' <returns> The result of the operation. </returns>
    Public Overloads Shared Operator =(ByVal left As ReadingAmount, ByVal right As ReadingAmount) As Boolean
        If left Is Nothing Then
            Return right Is Nothing
        ElseIf right Is Nothing Then
            Return False
        Else
            Return ReadingAmount.Equals(left, right)
        End If
    End Operator

    ''' <summary> &lt;&gt; casting operator. </summary>
    ''' <param name="left">  The left hand side item to compare for equality. </param>
    ''' <param name="right"> The left hand side item to compare for equality. </param>
    ''' <returns> The result of the operation. </returns>
    Public Overloads Shared Operator <>(ByVal left As ReadingAmount, ByVal right As ReadingAmount) As Boolean
        Return Not ReadingAmount.Equals(left, right)
    End Operator

    ''' <summary> Returns True if equal. </summary>
    ''' <remarks> Ranges are the same if the have the same
    ''' <see cref="Type">min</see> and <see cref="Type">max</see> values. </remarks>
    ''' <param name="left">  The left hand side item to compare for equality. </param>
    ''' <param name="right"> The left hand side item to compare for equality. </param>
    ''' <returns> <c>True</c> if equals. </returns>
    Public Overloads Shared Function Equals(ByVal left As ReadingAmount, ByVal right As ReadingAmount) As Boolean
        If left Is Nothing Then
            Return right Is Nothing
        ElseIf right Is Nothing Then
            Return False
        Else
            Return Arebis.TypedUnits.Amount.Equals(left.Amount, right.Amount)
        End If
    End Function

    ''' <summary> Determines whether the specified <see cref="T:System.Object" /> is equal to the
    ''' current <see cref="T:System.Object" />. </summary>
    ''' <param name="obj"> The <see cref="T:System.Object" /> to compare with the current
    ''' <see cref="T:System.Object" />. </param>
    ''' <returns> <c>True</c> if the specified <see cref="T:System.Object" /> is equal to the current
    ''' <see cref="T:System.Object" />; otherwise, <c>False</c>. </returns>
    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, ReadingAmount))
    End Function

    ''' <summary> Returns True if the value of the <paramref name="other"/> equals to the instance
    ''' value. </summary>
    ''' <remarks> Ranges are the same if the have the same
    ''' <see cref="Type">min</see> and <see cref="Type">max</see> values. </remarks>
    ''' <param name="other"> The other <see cref="ReadingAmount">Range</see> to compare for equality with this
    ''' instance. </param>
    ''' <returns> A Boolean data type. </returns>
    Public Overloads Function Equals(ByVal other As ReadingAmount) As Boolean
        If other Is Nothing Then
            Return False
        Else
            Return ReadingAmount.Equals(Me, other)
        End If
    End Function

    ''' <summary> Creates a unique hash code. </summary>
    ''' <returns> An <see cref="System.Int32">Int32</see> value. </returns>
    Public Overloads Overrides Function GetHashCode() As Int32
        Return Me.Amount.GetHashCode
    End Function

#End Region

#Region " AMOUNT "

    ''' <summary> The unit. </summary>
    ''' <value> The unit. </value>
    Public Property Unit As Arebis.TypedUnits.Unit

    ''' <summary> The amount. </summary>
    ''' <value> The amount. </value>
    Public Property Amount As Arebis.TypedUnits.Amount

    ''' <summary> Parses the reading to create the specific reading type in the inherited class. </summary>
    ''' <param name="valueReading"> The value reading. </param>
    ''' <param name="unitsReading"> The units reading. </param>
    ''' <returns> <c>True</c> if parsed. </returns>
    Public Overrides Function TryApplyReading(ByVal valueReading As String, ByVal unitsReading As String) As Boolean
        If MyBase.TryApplyReading(valueReading, unitsReading) Then
            Return Me.TryApplyReading(valueReading)
        Else
            Me.Amount = New Arebis.TypedUnits.Amount(VI.Pith.Scpi.Syntax.NotANumber, Me.Unit)
            Return False
        End If
    End Function

    ''' <summary> Parses the reading to create the specific reading type in the inherited class. </summary>
    ''' <param name="valueReading"> The value reading. </param>
    ''' <returns> <c>True</c> if parsed. </returns>
    Public Overrides Function TryApplyReading(ByVal valueReading As String) As Boolean
        If MyBase.TryApplyReading(valueReading) Then
            If Me.Value.HasValue Then
                Me.Amount = New Arebis.TypedUnits.Amount(Me.Value.Value, Me.Unit)
            End If
            Return True
        Else
            Me.Amount = New Arebis.TypedUnits.Amount(VI.Pith.Scpi.Syntax.NotANumber, Me.Unit)
            Return False
        End If
    End Function

#End Region

End Class

