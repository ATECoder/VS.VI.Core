''' <summary> Encapsulate the script information. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/7/2013" by="David" revision="">                 From TSP Library. </history>
''' <history date="03/02/2009" by="David" revision="3.0.3348.x"> Created. </history>
Public Class ScriptEntity
    Inherits ScriptEntityBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
    Private Sub New()
        MyBase.new()
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="name">      Specifies the script name. </param>
    ''' <param name="modelMask"> Specifies the model families for this script. </param>
    Public Sub New(ByVal name As String, ByVal modelMask As String)
        MyBase.New(name, modelMask)
    End Sub

#End Region

End Class

