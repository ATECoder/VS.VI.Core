Imports isr.Core.Pith.NumericExtensions
''' <summary> Defines a measured shunt resistance element. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="8/5/2013" by="David" revision="">                  Created. </history>
''' <history date="11/10/2012" by="David" revision="3.1.4697.x"> Created. </history>
Public MustInherit Class ShuntResistanceBase
    Inherits ResistanceMeasureBase
    Implements System.IEquatable(Of ShuntResistanceBase)

#Region " CONSTRUCTORS  AND  CLONES "

    ''' <summary> Default constructor. </summary>
    Protected Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Clones an existing measurement. </summary>
    ''' <param name="value"> The value. </param>
    Protected Sub New(ByVal value As ShuntResistanceBase)
        MyBase.New(value)
        If value IsNot Nothing Then
            Me._CurrentRange = value.CurrentRange
        End If
    End Sub

#End Region

#Region " PRESET "

    ''' <summary> Restores defaults. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        With My.MySettings.Default
            Me.Aperture = .ShuntResistanceApertureDefault
            Me.CurrentRange = .ShuntResistanceCurrentRangeDefault
            Me.CurrentLevel = .ShuntResistanceCurrentLevelDefault
            Me.LowLimit = .ShuntResistanceLowLimitDefault
            Me.HighLimit = .ShuntResistanceHighLimitDefault
            Me.VoltageLimit = .ShuntResistanceVoltageLimitDefault
        End With
    End Sub

#End Region

#Region " EQUALS "

    ''' <summary> Check throw if unequal configuration. </summary>
    ''' <param name="other"> The resistance to compare to this object. </param>
    Public Overloads Sub CheckThrowUnequalConfiguration(ByVal other As ShuntResistanceBase)
        If other IsNot Nothing Then
            MyBase.CheckThrowUnequalConfiguration(other)
            If Not Me.ConfigurationEquals(other) Then
                Dim format As String = "Unequal configuring--instrument {0}={1}.NE.{2}"
                If Not Me.CurrentRange.Approximates(other.CurrentRange, 0.000001) Then
                    Throw New isr.VI.OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format,
                                                                            "Current Range", Me.CurrentRange, other.CurrentRange))
                Else
                    Debug.Assert(Not Debugger.IsAttached, "Failed logic")
                End If
            End If
        End If
    End Sub

    ''' <summary> Indicates whether the current <see cref="T:ShuntResistanceBase"></see> value is equal to a
    ''' specified object. </summary>
    ''' <param name="obj"> An object. </param>
    ''' <returns> <c>True</c> if <paramref name="obj" /> and this instance are the same type and represent the
    ''' same value; otherwise, <c>False</c>. </returns>
    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        Return obj IsNot Nothing AndAlso (Object.ReferenceEquals(Me, obj) OrElse ShuntResistanceBase.Equals(Me, TryCast(obj, ShuntResistanceBase)))
    End Function

    ''' <summary> Indicates whether the current <see cref="T:ShuntResistanceBase"></see> value is
    ''' equal to a specified object. </summary>
    ''' <param name="other"> The cold resistance to compare to this object. </param>
    ''' <returns> <c>True</c> if the other parameter is equal to the current
    ''' <see cref="T:ShuntResistanceBase"></see> value;
    ''' otherwise, <c>False</c>. </returns>
    Public Overloads Function Equals(ByVal other As ShuntResistanceBase) As Boolean Implements System.IEquatable(Of ShuntResistanceBase).Equals
        Return other IsNot Nothing AndAlso MyBase.Equals(other) AndAlso
            Me.CurrentRange.Approximates(other.CurrentRange, 0.000001) AndAlso
            True
    End Function

    ''' <summary> Indicates whether the current <see cref="T:ShuntResistanceBase"></see> configuration values are
    ''' equal to a specified object. </summary>
    ''' <param name="other"> The cold resistance to compare to this object. </param>
    ''' <returns> <c>True</c> if the other parameter is equal to the current
    ''' <see cref="T:ShuntResistanceBase"></see> value;
    ''' otherwise, <c>False</c>. </returns>
    Public Overloads Function ConfigurationEquals(ByVal other As ShuntResistanceBase) As Boolean
        Return other IsNot Nothing AndAlso MyBase.ConfigurationEquals(other) AndAlso
            Me.CurrentRange.Approximates(other.CurrentRange, 0.000001) AndAlso
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
    Public Shared Operator =(ByVal left As ShuntResistanceBase, ByVal right As ShuntResistanceBase) As Boolean
        Return ((left Is Nothing) AndAlso (right Is Nothing)) OrElse (left IsNot Nothing) AndAlso left.Equals(right)
    End Operator

    ''' <summary> Implements the operator &lt;&gt;. </summary>
    ''' <param name="left">  The left. </param>
    ''' <param name="right"> The right. </param>
    ''' <returns> The result of the operation. </returns>
    Public Shared Operator <>(ByVal left As ShuntResistanceBase, ByVal right As ShuntResistanceBase) As Boolean
        Return ((left Is Nothing) AndAlso (right IsNot Nothing)) OrElse Not ((left IsNot Nothing) AndAlso left.Equals(right))
    End Operator

#End Region

#Region " CONFIGURATION PROPERTIES "

    Private _CurrentRange As Double
    ''' <summary> Gets or sets the current Range. </summary>
    ''' <value> The current Range. </value>
    Public Property CurrentRange() As Double
        Get
            Return Me._CurrentRange
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.CurrentRange) Then
                Me._CurrentRange = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

#End Region

End Class
