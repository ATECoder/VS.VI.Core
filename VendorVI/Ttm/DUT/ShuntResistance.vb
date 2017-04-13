''' <summary> Shunt resistance. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="12/23/2013" by="David" revision=""> Created. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance")>
Public Class ShuntResistance
    Inherits ShuntResistanceBase
    Implements System.ICloneable

#Region " CONSTRUCTORS  AND  CLONES "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Clones an existing measurement. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub New(ByVal value As ShuntResistance)
        MyBase.New(value)
        If value IsNot Nothing Then
        End If
    End Sub

    ''' <summary> Creates a new object that is a copy of the current instance. </summary>
    ''' <returns> A new object that is a copy of this instance. </returns>
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return New ShuntResistance(Me)
    End Function

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

End Class
