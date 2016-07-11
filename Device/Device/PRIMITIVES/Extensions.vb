Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Namespace ComboBoxExtensions

    ''' <summary> Includes extensions for <see cref="ComboBox">Combo Box</see>. </summary>
    ''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="11/19/2010" by="David" revision="1.2.3975.x"> Created. </history>
    Public Module Methods

        ''' <summary> Selects the <see cref="Control">combo box</see> item by setting the selected item to
        ''' the
        ''' <paramref name="value">value</paramref>.
        ''' This setter is thread safe. </summary>
        ''' <param name="control"> Combo box control. </param>
        ''' <param name="value">   The selected item value. </param>
        ''' <returns> value. </returns>
        <Extension()>
        Public Function SafeSelectItem(ByVal control As System.Windows.Forms.ComboBox, ByVal value As Object) As Object
            If control IsNot Nothing Then
                If control.InvokeRequired Then
                    control.Invoke(New Action(Of ComboBox, Object)(AddressOf ComboBoxExtensions.SafeSelectItem), New Object() {control, value})
                Else
                    control.SelectedItem = value
                End If
            End If
            Return value
        End Function

        ''' <summary> Selects the <see cref="Control">combo box</see> item by setting the selected item to
        ''' the <see cref="T:System.Collections.Generic.KeyValuePair">key value pair</see>.
        ''' This setter is thread safe. </summary>
        ''' <param name="control"> Combo box control. </param>
        ''' <param name="key">     The selected item key. </param>
        ''' <param name="value">   The selected item value. </param>
        ''' <returns> value. </returns>
        <Extension()>
        Public Function SafeSelectItem(ByVal control As ComboBox, ByVal key As System.Enum, ByVal value As String) As Object
            Return ComboBoxExtensions.SafeSelectItem(control, New KeyValuePair(Of [Enum], String)(key, value))
        End Function

    End Module

End Namespace
