Namespace TspSyntax.Display

    ''' <summary> Defines the TSP Display syntax. Modified for TSP2. </summary>
    ''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="01/15/05" by="David" revision="1.0.1841.x"> Created. </history>
    Public Module DisplaySyntax

        ''' <summary> The name of the display subsystem. </summary>
        Public Const SubsystemName As String = "display"

        ''' <summary> The clear display command. </summary>
        Public Const ClearCommand As String = "display.clear()"

        Public Const DisplayScreenCommandFormat As String = "display.changescreen(_G.display.SCREEN_{0})"

        ''' <summary> The set text format. </summary>
        Public Const SetTextLineCommandFormat As String = "display.settext(display.TEXT{0},'{1}')"

        ''' <summary> The restore main screen and wait complete command. </summary>
        Public Const RestoreMainWaitCompleteCommand As String = "display.screen = display.MAIN or 0 _G.waitcomplete(0)"

        ''' <summary> The length of the first line. </summary>
        Public Const FirstLineLength As Integer = 20

        ''' <summary> The length of the second line. </summary>
        Public Const SecondLineLength As Integer = 32

    End Module

End Namespace

