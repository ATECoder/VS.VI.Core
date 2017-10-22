''' <summary> Information about the version of a Keysight 34980 Meter/Scanner. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class VersionInfo
    Inherits isr.VI.VersionInfo

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Overrides Sub Clear()
        MyBase.Clear()
    End Sub

    ''' <summary> Parses the instrument identity string. </summary>
    ''' <param name="value"> Specifies the instrument identity string, which includes at a minimum the
    ''' following information:
    ''' <see cref="ManufacturerName">manufacturer</see>, <see cref="Model">model</see>,
    ''' <see cref="SerialNumber">serial number</see>, e.g., <para>
    ''' <c>H3458A</c>.</para> </param>
    Public Overrides Sub Parse(ByVal value As String)

        If String.IsNullOrEmpty(value) Then
            Me.Clear()
        Else

            ' save the identity.
            Me.Identity = value

            ' Parse the id to get the revision number
            Dim idItems As Queue(Of String) = New Queue(Of String)(value.Split(","c))

            Me.ManufacturerName = "KEYSIGHT"

            Me.Model = idItems.Dequeue

        End If

    End Sub

End Class

