''' <summary> Encapsulate the node information. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="03/02/2009" by="David" revision="3.0.3348.x"> Created. </history>
Public Class NodeEntity
    Inherits NodeEntityBase

    ''' <summary> Constructs the class. </summary>
    ''' <param name="number">               Specifies the node number. </param>
    ''' <param name="controllerNodeNumber"> The controller node number. </param>
    Public Sub New(ByVal number As Integer, ByVal controllerNodeNumber As Integer)
        MyBase.New(number, controllerNodeNumber)
    End Sub

End Class
