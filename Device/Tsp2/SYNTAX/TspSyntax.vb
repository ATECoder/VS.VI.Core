Namespace TspSyntax

    ''' <summary> Defines the TSP syntax. Modified for TSP2. </summary>
    ''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="01/15/05" by="David" revision="1.0.1841.x"> Created. </history>
    Public Module TspSyntaxConstants

#Region " CONSTANTS "

        ''' <summary> The  unknown values for values that go to the data log
        '''           but not for creating the data file name. </summary>
        Public Const UnknownValue As String = "N/A"

        ''' <summary> The illegal file characters. </summary>
        Public Const IllegalFileCharacters As String = "/\\:*?""<>|"

        ''' <summary> The  set of characters that should not be used in a
        '''           script name. </summary>
        Public Const IllegalScriptNameCharacters As String = "./\\"

        ''' <summary> The local node reference. </summary>
        Public Const LocalNode As String = "_G.localnode"

        ''' <summary> The  continuation prompt. </summary>
        Public Const ContinuationPrompt As String = ">>>>"

        ''' <summary> The  ready prompt. </summary>
        Public Const ReadyPrompt As String = "TSP>"

        ''' <summary> The  error prompt. </summary>
        Public Const ErrorPrompt As String = "TSP?"

#End Region

#Region " COMMAND BUILDERS "

        ''' <summary> Builds a command. </summary>
        ''' <param name="format"> Specifies a format string for the command. </param>
        ''' <param name="args">   Specifies the arguments for the command. </param>
        ''' <returns> The command. </returns>
        Public Function Build(ByVal format As String, ByVal ParamArray args() As Object) As String
            Return String.Format(Globalization.CultureInfo.InvariantCulture, format, args)
        End Function

#End Region

    End Module

End Namespace

