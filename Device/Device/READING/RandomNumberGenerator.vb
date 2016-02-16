''' <summary> Provides a simulated value for testing measurements without having the benefit of
''' instruments. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created. </history>
Public Class RandomNumberGenerator

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        generator = New Random(CInt(Date.Now.Ticks Mod Integer.MaxValue))
        Me.Min = 0
        Me.Max = 1
    End Sub

    ''' <summary> Holds a shared reference to the number generator. </summary>
    Private Property generator As Random

    ''' <summary> Gets the range. </summary>
    ''' <value> The range. </value>
    Private ReadOnly Property range As Double
        Get
            Return Me._Max - Me._Min
        End Get
    End Property

    ''' <summary> Gets the minimum. </summary>
    ''' <value> The minimum value. </value>
    Public Property Min() As Double

    ''' <summary> Gets the maximum. </summary>
    ''' <value> The maximum value. </value>
    Public Property Max() As Double

    ''' <summary> Returns a simulated value. </summary>
    ''' <value> The value. </value>
    Public ReadOnly Property [Value]() As Double
        Get
            Return generator.NextDouble * Me.range + Me.Min
        End Get
    End Property

End Class

