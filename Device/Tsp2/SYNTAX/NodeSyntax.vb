Namespace TspSyntax.Node

    ''' <summary> Defines the TSP Node syntax. Modified for TSP2. </summary>
    ''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="01/15/05" by="David" revision="1.0.1841.x"> Created. </history>
    Public Module NodeSyntax

#Region " NODE IDENTITY "

        ''' <summary> Gets or sets the IDN print command format. </summary>
        ''' <remarks>
        ''' Same as '*IDN?'.<para>
        ''' Requires setting the subsystem reference.
        ''' </para><code>
        ''' Value = String.Format(IdentityPrintCommandFormat,"node name")
        ''' </code>
        ''' </remarks>
        ''' <value> The identity print command format. </value>
        Public Property IdentityPrintCommandBuilder As String = "_G.print(""Keithley Instruments Inc., Model ""..{0}.model.."", ""..{0}.serialno.."", ""..{0}.version)"

#End Region

#Region " NODE COMMANDS "

        ''' <summary> Gets the status clear (CLS) command message. Requires a node number argument. </summary>
        Public Const CollectNodeGarbageFormat As String = "_G.node[{0}].execute('collectgarbage()') _G.waitcomplete({0})"

        ''' <summary> Gets the execute command.  Requires node number and command arguments. </summary>
        Public Const ExecuteNodeCommandFormat As String = "_G.node[{0}].execute(""{1}"") _G.waitcomplete({0})"

        ''' <summary> Gets the value returned by executing a command on the node.
        ''' Requires node number and value to get arguments. </summary>
        Public Const ValueGetterCommandFormat1 As String = "_G.node[{0}].execute('dataqueue.add({1})') _G.waitcomplete({0}) _G.waitcomplete() _G.print(_G.node[{0}].dataqueue.next())"
        '  3517 "_G.node[{0}].execute('dataqueue.add({1})') _G.waitcomplete(0) _G.print(_G.node[{0}].dataqueue.next())"

        ''' <summary> Gets the value returned by executing a command on the node.
        ''' Requires node number, command, and value to get arguments. </summary>
        Public Const ValueGetterCommandFormat2 As String = "_G.node[{0}].execute(""do {1} dataqueue.add({2}) end"") _G.waitcomplete({0}) _G.waitcomplete() _G.print(_G.node[{0}].dataqueue.next())"
        '  3517 "_G.node[{0}].execute(""do {1} dataqueue.add({2}) end"") _G.waitcomplete(0) _G.print(_G.node[{0}].dataqueue.next())"

        ''' <summary> Gets the connect rule command. Requires node number and value arguments. </summary>
        Public Const ConnectRuleSetterCommandFormat As String = "_G.node[{0}].channel.connectrule = {1}  _G.waitcomplete({0})"

#End Region

#Region " SYSTEM COMMAND BUILDERS "

        ''' <summary> Gets the reset command Builder. </summary>
        ''' <remarks> Same as '*RST'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ResetKnownStateCommandBuilder As String = "{0}.reset()"

        ''' <summary> Gets the status clear (CLS) command Builder. </summary>
        ''' <remarks> Same as '*CLS'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ClearExecutionStateCommandBuilder As String = "{0}.status.reset()"

#End Region

#Region " ERROR QUEUE COMMAND BUILDERS "

        ''' <summary> Gets the error queue clear command builder. </summary>
        ''' <remarks> Same as ':STAT:QUE:CLEAR'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ClearErrorQueueCommandBuilder As String = "{0}.eventlog.clear()"

        ''' <summary> Gets the error queue print query command builder. </summary>
        ''' <remarks> Same as ':STAT:QUE?'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ErrorQueuePrintCommandBuilder As String = "_G.print(string.format('%d,%s,level=%d',{0}.eventlog.next(eventlog.SEV_ERROR)))"

        ''' <summary> Gets the error queue count print query command builder. </summary>
        ''' <remarks>            Requires setting the subsystem reference. </remarks>
        Public Property ErrorQueueCountPrintCommandBuilder As String = "_G.print({0}.eventlog.getcount(eventlog.SEV_ERROR))"

#End Region

#Region " REGISTERS "

#Region " OPERATION EVENTS "

        ''' <summary> Gets the operation event enable command format builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property OperationEventEnableCommandFormatBuilder As String = "{0}.status.operation.enable = {{0}}"

        ''' <summary> Gets the operation event enable print query command builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property OperationEventEnablePrintCommandBuilder As String = "_G.print(_G.tostring({0}.status.operation.enable))"

        ''' <summary> Gets the operation event status print query command builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property OperationEventPrintCommandBuilder As String = "_G.print(_G.tostring({0}.status.operation.event))"

#End Region

#Region " SERVICE REQUEST "

        ''' <summary> Gets the service request enable command format builder. </summary>
        ''' <remarks> Same as *SRE {0:D}'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ServiceRequestEnableCommandFormatBuilder As String = "{0}.status.request_enable = {{0}}"

        ''' <summary> Gets the service request enable print query command builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ServiceRequestEnablePrintCommandBuilder As String = "_G.print(_G.tostring({0}.status.request_enable))"


        ''' <summary> Gets the service request enable print query command builder. </summary>
        ''' <remarks> Same as '*ESR?'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ServiceRequestEventPrintCommandBuilder As String = "_G.print(_G.tostring({0}.status.condition))"

#End Region

#Region " STANDARD EVENTS "

        ''' <summary> Gets the standard event enable command format builder. </summary>
        ''' <remarks> Same as *ESE {0:D}'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property StandardEventEnableCommandFormatBuilder As String = "{0}.status.standard.enable = {{0}}"

        ''' <summary> Gets the standard event enable print query command builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property StandardEventEnablePrintCommandBuilder As String = "_G.print(_G.tostring({0}.status.standard.enable))"

        ''' <summary> Gets the standard event status print query command builder. </summary>
        ''' <remarks> Same as *ESR?'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property StandardEventPrintCommandBuilder As String = "_G.waitcomplete() _G.print(_G.tostring({0}.status.standard.event))"

#End Region

#End Region

    End Module

End Namespace

