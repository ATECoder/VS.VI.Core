Namespace TspSyntax.LocalNode

    ''' <summary> Defines the TSP Local Node syntax. Modified for TSP2. </summary>
    ''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="01/15/05" by="David" revision="1.0.1841.x"> Created. </history>
    Public Module LocalNodeSyntax

        Public Const LineFrequencyQueryCommand1 As String = "localnode.linefreq"

        Public Const LineFrequencyPrintCommand As String = "_G.print(localnode.linefreq)"

        Public ReadOnly Property IdentityQueryCommand As String = $"{Ieee488.Syntax.IdentityQueryCommand} {Ieee488.Syntax.WaitCommand}"

        ''' <summary> The serial number print command. </summary>
        Public Const SerialNumberPrintCommand As String = "_G.print(G.localnode.serialno)"

        ''' <summary> The serial number print number command. </summary>
        Public Const SerialNumberPrintNumberCommand As String = "_G.print(string.format('%d',_G.localnode.serialno))"

        ''' <summary> The firmware version print command. </summary>
        Public Const FirmwareVersionPrintCommand As String = "_G.print(_G.localnode.version)"

        ''' <summary> The model print command. </summary>
        Public Const ModelPrintCommand As String = "_G.print(_G.localnode.model)"

        ''' <summary> Gets or sets the IDN print command. </summary>
        ''' <remarks> Returns a message similar to '*IDN?' with Keithley Instruments as the manufacturer. </remarks>
        ''' <value> The identity print command format. </value>
        Public Property IdentityPrintCommand As String = "_G.print(""Keithley Instruments Inc., Model "".._G.localnode.model.."", "".._G.localnode.serialno.."", "".._G.localnode.version)"


    End Module

End Namespace
