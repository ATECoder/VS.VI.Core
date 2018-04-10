Imports isr.Core.Pith.EnumExtensions
''' <summary> Device error. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="10/7/2013" by="David" revision=""> Created. </history>
Public Class DeviceError
    Inherits isr.VI.DeviceError

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New(VI.Pith.Scpi.Syntax.NoErrorCompoundMessage)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="compoundError"> The compound error. </param>
    Public Overrides Sub Parse(ByVal compoundError As String)
        If String.IsNullOrWhiteSpace(compoundError) Then
        ElseIf compoundError = "000" Then
            Me.ErrorNumber = 0
            Me.ErrorMessage = "" 'CType(Me.ErrorNumber, ErrorCodes).Description
            Me.CompoundErrorMessage = String.Format(Globalization.CultureInfo.CurrentCulture, "{0},{1}", Me.ErrorNumber, Me.ErrorMessage)
        ElseIf Integer.TryParse(compoundError.Trim("0"c), Me.ErrorNumber) Then
            If Me.ErrorNumber = 0 Then
                'Me.ErrorMessage = CType(Me.ErrorNumber, ErrorCodes).Description
                Me.CompoundErrorMessage = String.Format(Globalization.CultureInfo.CurrentCulture, "{0},{1}", Me.ErrorNumber, Me.ErrorMessage)
            Else
#If False Then
                If [Enum].IsDefined(GetType(ErrorCodes), Me.ErrorNumber) Then
                    Me.ErrorMessage = CType(Me.ErrorNumber, ErrorCodes).Description
                    Me.CompoundErrorMessage = String.Format(Globalization.CultureInfo.CurrentCulture, "{0},{1}", Me.ErrorNumber, Me.ErrorMessage)
                End If
#End If
            End If
        End If
    End Sub
End Class
