Namespace TspSyntax.EventLog

    ''' <summary> Defines the TSP Event (and error) Log syntax. Modified for TSP2. </summary>
    ''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="01/15/05" by="David" revision="1.0.1841.x"> Created. </history>
    Public Module EventLogSyntax

        ''' <summary> The global event log clear command. </summary>
        ''' <remarks> Same as ':STAT:QUE:CLEAR'</remarks>
        Public Const ClearEventLogCommand As String = "_G.eventlog.clear() waitcomplete()"

        Public Const LocalNodeClearEventLogCommand As String = "localnode.eventlog.clear() waitcomplete()"

        ''' <summary> The node event log clear. </summary>
        Public Const NodeClearEventLogCommand As String = "node[{0}].eventlog.clear() waitcomplete({0})"

        ''' <summary> The next error formatted query command. </summary>
        Public Const NextErrorFormattedQueryCommand As String = "_G.print(string.format('%d,%s,level=%d',_G.eventlog.next(eventlog.SEV_ERROR)))"

        ''' <summary> The next error formatted Print command. </summary>
        Public Const NextErrorFormattedPrintCommand As String = "_G.print(string.format('%d,%s,level=%d',_G.eventlog.next(eventlog.SEV_ERROR)))"

        ''' <summary> The next error query command. </summary>
        Public Const NextErrorQueryCommand As String = "_G.eventlog.next(eventlog.SEV_ERROR)"

        ''' <summary> The next error Print command. </summary>
        Public Const NextErrorPrintCommand As String = "_G.print(_G.eventlog.next(eventlog.SEV_ERROR))"

        ''' <summary> The error count query command. </summary>
        Public Const ErrorCountQueryCommand As String = "_G.eventlog.getcount(eventlog.SEV_ERROR)"

        ''' <summary> The node error count query command. </summary>
        Public Const NodeErrorCountQueryCommand As String = "node[{0}].eventlog.getcount(eventlog.SEV_ERROR)"

        ''' <summary> The error count Print command. </summary>
        Public Const ErrorCountPrintCommand As String = "_G.print(_G.eventlog.getcount(eventlog.SEV_ERROR))"

#Region " DEFAULT ERROR MESSAGES "

        ''' <summary> Gets the error message representing no error. </summary>
        Public Const NoErrorMessage As String = "No Error"

        ''' <summary> Gets the compound error message representing no error. </summary>
        Public Const NoErrorCompoundMessage As String = "0,No Error,0,0"

#End Region

    End Module

End Namespace
