''' <summary> Defines a measured cold resistance element. </summary>
''' <license> (c) 2009 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="02/02/2009" by="David" revision="2.1.3320.x"> Created. </history>
Public MustInherit Class ColdResistanceBase
    Inherits ResistanceMeasureBase
    Implements System.IEquatable(Of ColdResistanceBase)

#Region " CONSTRUCTORS  AND  CLONES "

    ''' <summary> Default constructor. </summary>
    Protected Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Clones an existing measurement. </summary>
    ''' <param name="value"> The value. </param>
    Protected Sub New(ByVal value As ColdResistanceBase)
        MyBase.New(value)
    End Sub

#End Region

#Region " PRESET "

    ''' <summary> Restores defaults. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        With My.MySettings.Default
            Me.Aperture = .ColdResistanceApertureDefault
            Me.CurrentLevel = .ColdResistanceCurrentLevelDefault
            Me.LowLimit = .ColdResistanceLowLimitDefault
            Me.HighLimit = .ColdResistanceHighLimitDefault
            Me.VoltageLimit = .ColdResistanceVoltageLimitDefault
            Windows.Forms.Application.DoEvents()
        End With
    End Sub

#End Region

#Region " EQUALS "

    ''' <summary> Indicates whether the current <see cref="T:ColdResistanceBase"></see> value is equal to a
    ''' specified object. </summary>
    ''' <param name="obj"> An object. </param>
    ''' <returns> <c>True</c> if <paramref name="obj" /> and this instance are the same type and represent the
    ''' same value; otherwise, <c>False</c>. </returns>
    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        Return obj IsNot Nothing AndAlso (Object.ReferenceEquals(Me, obj) OrElse ColdResistanceBase.Equals(Me, TryCast(obj, ColdResistanceBase)))
    End Function

    ''' <summary> Indicates whether the current <see cref="T:ColdResistanceBase"></see> value is
    ''' equal to a specified object. </summary>
    ''' <param name="other"> The cold resistance to compare to this object. </param>
    ''' <returns> <c>True</c> if the other parameter is equal to the current
    ''' <see cref="T:ColdResistanceBase"></see> value;
    ''' otherwise, <c>False</c>. </returns>
    Public Overloads Function Equals(ByVal other As ColdResistanceBase) As Boolean Implements System.IEquatable(Of ColdResistanceBase).Equals
        Return other IsNot Nothing AndAlso
            Me.Reading.Equals(other.Reading) AndAlso
            Me.ConfigurationEquals(other) AndAlso
            True
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
    Public Shared Operator =(ByVal left As ColdResistanceBase, ByVal right As ColdResistanceBase) As Boolean
        Return ((left Is Nothing) AndAlso (right Is Nothing)) OrElse (left IsNot Nothing) AndAlso left.Equals(right)
    End Operator

    ''' <summary> Implements the operator &lt;&gt;. </summary>
    ''' <param name="left">  The left. </param>
    ''' <param name="right"> The right. </param>
    ''' <returns> The result of the operation. </returns>
    Public Shared Operator <>(ByVal left As ColdResistanceBase, ByVal right As ColdResistanceBase) As Boolean
        Return ((left Is Nothing) AndAlso (right IsNot Nothing)) OrElse Not ((left IsNot Nothing) AndAlso left.Equals(right))
    End Operator

#End Region

End Class
