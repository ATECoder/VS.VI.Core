Namespace TspSyntax

    Public Module TspSyntax1

#Region " CHUNK CONSTANTS "

        ''' <summary> Gets the chunk defining a start of comment block. </summary>
        Public Const StartCommentChunk As String = "--[["

        ''' <summary> Gets the chunk defining an end of comment block. </summary>
        Public Const EndCommentChunk As String = "]]--"

        ''' <summary> Gets the chunk defining a comment. </summary>
        Public Const CommentChunk As String = "--"

        ''' <summary> Gets the signature of a chunk line defining a chunk name. </summary>
        Public Const DeclareChunkNameSignature As String = "local chunkName ="

        ''' <summary> Gets the signature of a chunk line defining a require statement for the chunk name. </summary>
        Public Const RequireChunkNameSignature As String = "require("""

        ''' <summary> Gets the signature of a chunk line defining a loaded statement for the chunk name. </summary>
        Public Const LoadedChunkNameSignature As String = "_G._LOADED["

#End Region

#Region " NODE COMMANDS "

        ''' <summary> Gets the status clear (CLS) command message. Requires a node number argument. </summary>
        Public Const CollectNodeGarbageFormat As String = "_G.node[{0}].execute('collectgarbage()') _G.waitcomplete({0})"

        ''' <summary> Gets the execute command.  Requires node number and command arguments. </summary>
        Public Const ExecuteNodeCommandFormat As String = "_G.node[{0}].execute(""{1}"") _G.waitcomplete({0})"

        ''' <summary> Gets the value returned by executing a command on the node.
        ''' Requires node number and value to get arguments. </summary>
        Public Const NodeValueGetterCommandFormat1 As String = "_G.node[{0}].execute('dataqueue.add({1})') _G.waitcomplete({0}) _G.waitcomplete() _G.print(_G.node[{0}].dataqueue.next())"
        '  3517 "_G.node[{0}].execute('dataqueue.add({1})') _G.waitcomplete(0) _G.print(_G.node[{0}].dataqueue.next())"

        ''' <summary> Gets the value returned by executing a command on the node.
        ''' Requires node number, command, and value to get arguments. </summary>
        Public Const NodeValueGetterCommandFormat2 As String = "_G.node[{0}].execute(""do {1} dataqueue.add({2}) end"") _G.waitcomplete({0}) _G.waitcomplete() _G.print(_G.node[{0}].dataqueue.next())"
        '  3517 "_G.node[{0}].execute(""do {1} dataqueue.add({2}) end"") _G.waitcomplete(0) _G.print(_G.node[{0}].dataqueue.next())"

        ''' <summary> Gets the connect rule command. Requires node number and value arguments. </summary>
        Public Const NodeConnectRuleSetterCommandFormat As String = "_G.node[{0}].channel.connectrule = {1}  _G.waitcomplete({0})"

#End Region

#Region " SCRIPT COMMANDS "

        ''' <summary>
        ''' Gets a command to retrieve a catalog from the local node.
        ''' This command must be enclosed in a 'do end' construct.
        ''' a print(names) or dataqueue.add(names) needs to be added to get the data through.
        ''' </summary>
        Public Const ScriptCatalogGetterCommand As String = "local names='' for name in script.user.catalog() do names = names .. name .. ',' end"

#End Region

#Region " SYSTEM COMMANDS "

        ''' <summary> Gets the IDN query command builder. </summary>
        ''' <remarks> Same as '*IDN?'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property IdentifyQueryCommandBuilder As String = "_G.print(""Keithley Instruments Inc., Model ""..{0}.model.."", ""..{0}.serialno.."", ""..{0}.revision)"

        ''' <summary> Gets the function call command for a function w/o arguments. </summary>
        Private Const _callFunctionCommandFormat As String = "_G.pcall( {0} )"

        ''' <summary>
        ''' Gets the function call command for a function with arguments.
        ''' </summary>
        Private Const _callFunctionArgumentsCommandFormat As String = "_G.pcall( {0} , {1} )"

        ''' <summary> Returns a command to run the specified function with arguments. </summary>
        ''' <param name="functionName">Specifies the function name.</param>
        ''' <param name="args">Specifies the function arguments.</param>
        Public Function CallFunctionCommand(ByVal functionName As String, ByVal args As String) As String

            If String.IsNullOrWhiteSpace(args) Then
                Return Build(TspSyntax1._callFunctionCommandFormat, functionName)
            Else
                Return Build(TspSyntax1._callFunctionArgumentsCommandFormat, functionName, args)
            End If

        End Function

#End Region

#Region " SYSTEM COMMAND BUILDERS "

        ''' <summary> Gets the reset command builder. </summary>
        ''' <remarks> Same as '*RST'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ResetKnownStateCommandBuilder As String = "{0}.reset()"

        ''' <summary> Gets the status clear (CLS) command builder. </summary>
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
        Public Property ClearErrorQueueCommandBuilder As String = "{0}.errorqueue.clear()"

        ''' <summary> Gets the error queue query command builder. </summary>
        ''' <remarks> Same as ':STAT:QUE?'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ErrorQueueQueryCommandBuilder As String = "_G.print(string.format('%d,%s,level=%d',{0}.errorqueue.next()))"

        ''' <summary> Gets the error queue count query command builder. </summary>
        ''' <remarks> Same as ':STAT:QUE?'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ErrorQueueCountQueryCommandFormat As String = "_G.print({0}.errorqueue.count)"

#End Region

#Region " REGISTERS "

#Region " OPERATION EVENTS "

        ''' <summary> Gets the operation event enable command format builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property OperationEventEnableCommandFormatBuilder As String = "{0}.status.operation.enable = {{0}}"

        ''' <summary> Gets the operation event enable query command builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property OperationEventEnableQueryCommandBuilder As String = "_G.print(_G.tostring({0}.status.operation.enable))"

        ''' <summary> Gets the operation event status query command builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property OperationEventQueryCommandBuilder As String = "_G.print(_G.tostring({0}.status.operation.event))"

#End Region

#Region " SERVICE REQUEST "

        ''' <summary> Gets the service request enable command format builder. </summary>
        ''' <remarks> Same as *SRE {0:D}'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ServiceRequestEnableCommandFormatBuilder As String = "{0}.status.request_enable = {{0}}"

        ''' <summary> Gets the service request enable query command builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ServiceRequestEnableQueryCommandBuilder As String = "_G.print(_G.tostring({0}.status.request_enable))"


        ''' <summary> Gets the service request enable query command builder. </summary>
        ''' <remarks> Same as '*ESR?'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property ServiceRequestEventQueryCommandBuilder As String = "_G.print(_G.tostring({0}.status.condition))"

#End Region

#Region " STANDARD EVENTS "

        ''' <summary> Gets the standard event enable command format builder. </summary>
        ''' <remarks> Same as *ESE {0:D}'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property StandardEventEnableCommandFormatBuilder As String = "{0}.status.standard.enable = {{0}}"

        ''' <summary> Gets the standard event enable query command builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property StandardEventEnableQueryCommandBuilder As String = "_G.print(_G.tostring({0}.status.standard.enable))"

        ''' <summary> Gets the standard event status query command builder. </summary>
        ''' <remarks> Same as *ESR?'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Property StandardEventQueryCommandBuilder As String = "_G.waitcomplete() _G.print(_G.tostring({0}.status.standard.event))"

#End Region

#End Region

    End Module

End Namespace

