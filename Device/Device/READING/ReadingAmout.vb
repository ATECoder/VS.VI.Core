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

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Constructs a measured value without specifying the value or its validity, which must
    ''' be specified for the value to be made valid. </summary>
    Public Sub New()
        MyBase.New()
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

    ''' <summary> Returns True if the value of the <paramref name="obj" /> equals to the instance value. </summary>
    ''' <param name="obj"> The object to compare for equality with this instance. This object should
    ''' be type <see cref="ReadingAmount"/> </param>
    ''' <returns> Returns <c>True</c> if <paramref name="obj" /> is the same value as this instance;
    ''' otherwise, <c>False</c> </returns>
    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        Return ReadingAmount.Equals(Me, TryCast(obj, ReadingAmount))
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
    Public Overrides Function TryParse(ByVal valueReading As String, ByVal unitsReading As String) As Boolean
        If MyBase.TryParse(valueReading, unitsReading) Then
            Return Me.TryParse(valueReading)
        Else
            Me.Amount = New Arebis.TypedUnits.Amount(Scpi.Syntax.NotANumber, Me.Unit)
            Return False
        End If
    End Function

    ''' <summary> Parses the reading to create the specific reading type in the inherited class. </summary>
    ''' <param name="valueReading"> The value reading. </param>
    ''' <returns> <c>True</c> if parsed. </returns>
    Public Overrides Function TryParse(ByVal valueReading As String) As Boolean
        If MyBase.TryParse(valueReading) Then
            If Me.Value.HasValue Then
                Me.Amount = New Arebis.TypedUnits.Amount(Me.Value.Value, Me.Unit)
            End If
            Return True
        Else
            Me.Amount = New Arebis.TypedUnits.Amount(Scpi.Syntax.NotANumber, Me.Unit)
            Return False
        End If
    End Function

#End Region

End Class

