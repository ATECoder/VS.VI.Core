Namespace TspSyntax

    ''' <summary> Defines a SCPI Base Subsystem. </summary>
    ''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="01/15/05" by="David" revision="1.0.1841.x"> Created. </history>
    Public Module TestScriptProcessorSyntax

#Region " CONSTANTS "

        ''' <summary> The  unknown values for values that go to the data log
        '''           but not for creating the data file name. </summary>
        Public Const UnknownValue As String = "N/A"

        ''' <summary> The illegal file characters. </summary>
        Public Const IllegalFileCharacters As String = "/\\:*?""<>|"

        ''' <summary> The  set of characters that should not be used in a
        '''           script name. </summary>
        Public Const IllegalScriptNameCharacters As String = "./\\"

        ''' <summary> The  global node reference. </summary>
        Public Const GlobalNode As String = "_G"

        ''' <summary> The  global node reference. </summary>
        Public Const LocalNode As String = "_G.localnode"

        ''' <summary> Represents the LUA nil value. </summary>
        Public Const NilValue As String = "nil"

        ''' <summary> Represents the LUA true value </summary>
        Public Const TrueValue As String = "true"

        ''' <summary> Represents the LUA false value </summary>
        Public Const FalseValue As String = "false"

        ''' <summary> The  continuation prompt. </summary>
        Public Const ContinuationPrompt As String = ">>>>"

        ''' <summary> The  ready prompt. </summary>
        Public Const ReadyPrompt As String = "TSP>"

        ''' <summary> The  error prompt. </summary>
        Public Const ErrorPrompt As String = "TSP?"

        ''' <summary> The termination character. </summary>
        Public Const TerminationChar As Char = Global.Microsoft.VisualBasic.ChrW(10)

#End Region

#Region " DEFAULT ERROR MESSAGES "

        ''' <summary> Gets the error message representing no error. </summary>
        Public Const NoErrorMessage As String = "No Error"

        ''' <summary> Gets the compound error message representing no error. </summary>
        Public Const NoErrorCompoundMessage As String = "0,No Error,0,0"

#End Region

#Region " SYSTEM COMMANDS "

        ''' <summary> The Clear Status command (same as '*CLS'). </summary>
        Public Const ClearExecutionStateCommand As String = "_G.status.reset()"

        ''' <summary> The collect garbage command. </summary>
        Public Const CollectGarbageCommand As String = "_G.collectgarbage()"

        ''' <summary> The  collect garbage command. </summary>
        Public Const CollectGarbageWaitCompleteCommand As String = "_G.collectgarbage() _G.waitcomplete(0)"

        ''' <summary> The show errors property. </summary>
        Public Const ShowErrors As String = "_G.localnode.showerrors"

        ''' <summary> The hide/show errors command. </summary>
        Public Const ShowErrorsSetterCommand As String = "_G.localnode.showerrors={0:'1';'1';'0'}"

        ''' <summary> The operation completed command (Same as '*OPC'). </summary>
        Public Const OperationCompletedCommand As String = "_G.opc()"

        ''' <summary> Gets or set the operation completed query command. </summary>
        ''' <remarks> Same as '*OPC?'. </remarks>
        Public Const OperationCompletedQueryCommand As String = "_G.waitcomplete() print(""1"")"

        ''' <summary> The print command format. </summary>
        Public Const PrintCommandFormat As String = "_G.print({0})"

        ''' <summary> The print command to send to the instrument. The
        ''' format conforms to the 'C' query command and returns the Boolean outcome. </summary>
        ''' <remarks> The format string follows the same rules as the printf family of standard C
        ''' functions. The only differences are that the options or modifiers *, l, L, n, p, and h are
        ''' not supported and that there is an extra option, q. The q option formats a string in a form
        ''' suitable to be safely read back by the Lua interpreter: the string is written between double
        ''' quotes, and all double quotes, newlines, embedded zeros, and backslashes in the string are
        ''' correctly escaped when written. For instance, the call string.format('%q', 'a string with
        ''' ''quotes'' and [BS]n new line') will produce the string: a string with [BS]''quotes[BS]'' and
        ''' [BS]new line The options c, d, E, e, f, g, G, i, o, u, X, and x all expect a number as
        ''' argument, whereas q and s expect a string. This function does not accept string values
        ''' containing embedded zeros. </remarks>
        Public Const PrintCommandStringFormat As String = "_G.print(string.format('{0}',{1}))"

        ''' <summary> The print command string number format. </summary>
        Public Const PrintCommandStringNumberFormat As String = "_G.print(string.format('%{0}f',{1}))"

        ''' <summary> The print command string integer format. </summary>
        Public Const PrintCommandStringIntegerFormat As String = "_G.print(string.format('%d',{0}))"

        ''' <summary> Returns the print command for the specified arguments. </summary>
        Public Function PrintCommand(ByVal args As String) As String
            Return TspSyntax.Build(TspSyntax.PrintCommandFormat, args)
        End Function

        ''' <summary> The show prompts property. </summary>
        Public Const ShowPrompts As String = "_G.localnode.prompts"

        ''' <summary> The hide/show prompts setter command. </summary>
        Public Const ShowPromptsSetterCommand As String = "_G.localnode.prompts={0:'1';'1';'0'}"

        ''' <summary> The reset to known state command (Same as '*RST'). </summary>
        ''' <remarks> </remarks>
        Public Const ResetKnownStateCommand As String = "_G.reset()"

        ''' <summary> The wait command. </summary>
        Public Const WaitGroupCommandFormat As String = "_G.waitcomplete({0})"

        ''' <summary> The wait command. </summary>
        Public Const WaitCommand As String = "_G.waitcomplete()"

        Public Const LineFrequencyQueryCommand As String = "_G.print(localnode.linefreq)"

        Public ReadOnly Property IdentityQueryCommand As String = $"{Ieee488.Syntax.IdentityQueryCommand} {Ieee488.Syntax.WaitCommand}"

#End Region

#Region " ERROR QUEUE "

        ''' <summary> The default error queue clear command. </summary>
        ''' <remarks> Same as ':STAT:QUE:CLEAR'</remarks>
        Public Const ClearErrorQueueCommand As String = "_G.errorqueue.clear() waitcomplete()"

        ''' <summary> The clear error queue command. </summary>
        Public Const ClearLocalNodeErrorQueueCommand As String = "localnode.errorqueue.clear() waitcomplete()"

        ''' <summary> The default error queue query command. </summary>
        ''' <remarks> Same as ':STAT:QUE?'</remarks>
        Public Const ErrorQueueQueryCommand As String = "_G.print(string.format('%d,%s,level=%d',_G.errorqueue.next()))"

        ''' <summary> The  error queue count query command. </summary>
        ''' <remarks> Same as '..'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Const ErrorQueueCountQueryCommand As String = "_G.print(_G.errorqueue.count)"

#End Region

#Region " REGISTERS "

#Region " MEASUREMENT EVENTS "

        ''' <summary> The  measurement event enable command format. </summary>
        Public Const MeasurementEventEnableCommandFormat As String = "_G.status.measurement.enable={0}"

        ''' <summary> The  measurement event enable query command. </summary>
        Public Const MeasurementEventEnableQueryCommand As String = "_G.print(_G.tostring(_G.status.measurement.enable))"

        ''' <summary> The  measurement event status query command. </summary>
        Public Const MeasurementEventQueryCommand As String = "_G.print(_G.tostring(_G.status.measurement.event))"

        ''' <summary> The  measurement event condition query command. </summary>
        Public Const MeasurementEventConditionQueryCommand As String = "_G.print(_G.tostring(_G.status.measurement.condition))"

        ''' <summary> The measurement event condition print command. </summary>
        Public Const MeasurementEventConditionPrintCommand As String = "_G.print(_G.string.format('%d',_G.status.measurement.condition))"

#End Region

#Region " OPERATION EVENTS "

        ''' <summary> The  operation event enable command format. </summary>
        ''' <remarks> Same as ''.</remarks>
        Public Const OperationEventEnableCommandFormat As String = "_G.status.operation.enable={0}"

        ''' <summary> The  operation event enable query command. </summary>
        ''' <remarks> Same as ''. </remarks>
        Public Const OperationEventEnableQueryCommand As String = "_G.print(_G.tostring(_G.status.operation.enable))"

        ''' <summary> The  operation event status query command. </summary>
        ''' <remarks> Same as ''. </remarks>
        Public Const OperationEventQueryCommand As String = "_G.print(_G.tostring(_G.status.operation.event))"

        ''' <summary> The  operation event condition query command. </summary>
        ''' <remarks> Same as ''. </remarks>
        Public Const OperationEventConditionQueryCommand As String = "_G.print(_G.tostring(_G.status.operation.condition))"

#End Region

#Region " QUESTIONABLE EVENTS "

        ''' <summary> The  questionable event status query command. </summary>
        ''' <remarks> Same as ''. </remarks>
        Public Const QuestionableEventQueryCommand As String = "_G.print(_G.tostring(_G.status.questionable.event))"

#End Region

#Region " SERVICE REQUEST "

        ''' <summary> The Service Request Enable *SRE {0:D}' Enable command. </summary>
        ''' <remarks> Same as '*SRE {0:D}'</remarks>
        Public Const ServiceRequestEnableCommandFormat As String = "_G.status.request_enable={0}"

        ''' <summary> The  service request enable query command. </summary>
        ''' <remarks> Same as '*SRE?'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Const ServiceRequestEnableQueryCommand As String = "_G.print(_G.tostring(_G.status.request_enable))"

        ''' <summary> The  service request enable query command builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Const ServiceRequestEventQueryCommand As String = "_G.print(_G.tostring(_G.status.condition))"

#End Region

#Region " STANDARD EVENTS "

        ''' <summary> The Standard Event Enable command. </summary>
        ''' <remarks> Same as '*ESE {0:D}'</remarks>
        Public Const StandardEventEnableCommandFormat As String = "_G.status.standard.enable={0}"

        ''' <summary> The  standard event enable query command. </summary>
        ''' <remarks> Same as ''. </remarks>
        Public Const StandardEventEnableQueryCommand As String = "_G.print(_G.tostring(_G.status.standard.enable))"

        ''' <summary> The Standard Event Enable (*ESR?) command. </summary>
        ''' <remarks> Same as '*ESR?'</remarks>
        Public Const StandardEventQueryCommand As String = "_G.waitcomplete() _G.print(_G.tostring(_G.status.standard.event))"

#End Region

#Region " SERVICE REQUEST + STANDARD EVENTS "

        Private _StandardServiceEnableCommandFormat As String

        ''' <summary> The Standard Event and Service Request Enable command format. </summary>
        ''' <remarks>
        ''' Same as *CLS; *ESE {0:D}; *SRE {1:D}'
        ''' <para>Using termination character as separator failed with the 2600 instruments.</para>
        ''' </remarks>
        ''' <value> The standard service enable command format. </value>
        Public Property StandardServiceEnableCommandFormat As String
            Get
                If String.IsNullOrWhiteSpace(TspSyntax._StandardServiceEnableCommandFormat) Then
                    Dim commandLine As New System.Text.StringBuilder
                    commandLine.AppendFormat("{0} ", TspSyntax.ClearExecutionStateCommand)
                    commandLine.AppendFormat("{0} ", TspSyntax.StandardEventEnableCommandFormat)
                    commandLine.Append(TspSyntax.ServiceRequestEnableCommandFormat.Replace("{0}", "{1}"))
                    TspSyntax.StandardServiceEnableCommandFormat = commandLine.ToString
                End If
                Return TspSyntax._StandardServiceEnableCommandFormat
            End Get
            Set(ByVal value As String)
                TspSyntax._StandardServiceEnableCommandFormat = value
            End Set
        End Property

        Private _StandardServiceEnableCompleteCommandFormat As String

        ''' <summary> The  Standard Event and Service Request Enable command format. </summary>
        ''' <remarks>
        ''' Same as '*CLS; *ESE {0:D}; *SRE {1:D} *OPC'
        ''' <para>Using termination character as separator failed with the 2600 instruments.</para>
        ''' </remarks>
        ''' <value> The standard service enable complete command format. </value>
        Public Property StandardServiceEnableCompleteCommandFormat As String
            Get
                If String.IsNullOrWhiteSpace(TspSyntax._StandardServiceEnableCompleteCommandFormat) Then
                    Dim commandLine As New System.Text.StringBuilder
                    commandLine.AppendFormat("{0} ", TspSyntax.ClearExecutionStateCommand)
                    commandLine.AppendFormat("{0} ", TspSyntax.StandardEventEnableCommandFormat)
                    commandLine.AppendFormat("{0} ", TspSyntax.ServiceRequestEnableCommandFormat.Replace("{0}", "{1}"))
                    commandLine.Append(TspSyntax.OperationCompletedCommand)
                    TspSyntax.StandardServiceEnableCompleteCommandFormat = commandLine.ToString
                End If
                Return TspSyntax._StandardServiceEnableCompleteCommandFormat
            End Get
            Set(ByVal value As String)
                TspSyntax._StandardServiceEnableCompleteCommandFormat = value
            End Set
        End Property

#End Region

#End Region

#Region " ELEMENT BUILDERS "

        Private Const _nodeReferenceFormat As String = "_G.node[{0}]"

        ''' <summary> Returns a TSP reference to the specified node. </summary>
        ''' <param name="node">            Specifies the one-based node number. </param>
        ''' <param name="localNodeNumber"> The local node number. </param>
        ''' <returns> Then node reference. </returns>
        Public Function NodeReference(ByVal node As Integer, ByVal localNodeNumber As Integer) As String

            If node = localNodeNumber Then
                ' if node is number one, use the local node as reference in case
                ' we do not have other nodes.
                Return TspSyntax.LocalNode
            Else
                Return TspSyntax.Build(TspSyntax._nodeReferenceFormat, node)
            End If

        End Function

        ''' <summary> The SMU number 'a'. </summary>
        Public Const SourceMeasureUnitNumberA As String = "a"

        ''' <summary> The SMU number 'b'. </summary>
        Public Const SourceMeasureUnitNumberB As String = "b"

        ''' <summary> The  SMU name format. </summary>
        Private Const _SmuNameFormat As String = "smu{0}"

        ''' <summary> Builds the SMU name, e.g., smua or smub for the specified <paramref name="smuNumber">SMU number</paramref>. </summary>
        ''' <param name="smuNumber">             Specifies the SMU Number (either 'a' or 'b'. </param>
        ''' <returns> rThe node smu reference. </returns>
        Public Function BuildSmuName(ByVal smuNumber As String) As String
            Return TspSyntax.Build(TspSyntax._SmuNameFormat, smuNumber)
        End Function

        ''' <summary> The  global node reference. </summary>
        Private Const _localNodeSmuFormat As String = "_G.localnode.smu{0}"

        Private Const _smuReferenceFormat As String = "_G.node[{0}].smu{1}"

        ''' <summary> Builds a TSP reference to the specified <paramref name="smuNumber">SMU
        ''' number</paramref> on the local node. </summary>
        ''' <param name="smuNumber"> Specifies the SMU (either 'a' or 'b'. </param>
        ''' <returns> rThe node smu reference. </returns>
        Public Function BuildSmuReference(ByVal smuNumber As String) As String

            ' use the local node as reference in case we do not have other nodes.
            Return TspSyntax.Build(TspSyntax._localNodeSmuFormat, smuNumber)

        End Function

        ''' <summary> Builds a TSP reference to the specified <paramref name="smuNumber">SMU
        ''' number</paramref> on the specified node. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        ''' illegal values. </exception>
        ''' <param name="nodeNumber">      Specifies the node number (must be greater than 0). </param>
        ''' <param name="localNodeNumber"> The local node number (must be greater than 0). </param>
        ''' <param name="smuNumber">       Specifies the SMU (either 'a' or 'b'. </param>
        ''' <returns> rThe node smu reference. </returns>
        Public Function BuildSmuReference(ByVal nodeNumber As Integer, ByVal localNodeNumber As Integer, ByVal smuNumber As String) As String

            If nodeNumber <= 0 Then
                Throw New ArgumentException("Node number must be greater than or equal to 1.")
            ElseIf localNodeNumber <= 0 Then
                Throw New ArgumentException("Local node number must be greater than or equal to 1.")
            End If
            If nodeNumber = localNodeNumber Then
                ' if node is number one, use the local node as reference in case
                ' we do not have other nodes.
                Return TspSyntax.BuildSmuReference(smuNumber)
            Else
                Return TspSyntax.Build(TspSyntax._smuReferenceFormat, nodeNumber, smuNumber)
            End If

        End Function

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

#Region " FUNCTION "

        ''' <summary> Returns a string from the parameter array of arguments for use when running the
        ''' function. </summary>
        ''' <param name="args"> Specifies a parameter array of arguments. </param>
        ''' <returns> A comma-separated string. </returns>
        Public Function Parameterize(ByVal ParamArray args() As String) As String

            Dim arguments As New System.Text.StringBuilder
            Dim i As Integer
            If args IsNot Nothing AndAlso args.Length >= 0 Then
                For i = 0 To args.Length - 1
                    If (i > 0) Then
                        arguments.Append(",")
                    End If
                    arguments.Append(args(i))
                Next i

            End If
            Return arguments.ToString

        End Function

        ''' <summary> Calls the function with the given arguments in protected mode. </summary>
        ''' <remarks> Protected mode means that any error inside the function is not propagated; instead,
        ''' the call (Lua pcall) catches the error and returns a status code. Its first result is the
        ''' status code (a Boolean), which is <c>True</c> if the call succeeds without errors. In such case,
        ''' pcall also returns all results from the call, after this first result. In case of any error,
        ''' pcall returns false plus the error message. </remarks>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="functionName"> Specifies the function name. </param>
        ''' <param name="args">         Specifies the function arguments. </param>
        Public Sub CallFunction(ByVal session As SessionBase, ByVal functionName As String, ByVal args As String)
            If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
            If String.IsNullOrWhiteSpace(functionName) Then Throw New ArgumentNullException(NameOf(functionName))
            Dim callStatement As String
            callStatement = TspSyntax.CallFunctionCommand(functionName, args)
            callStatement = TspSyntax.PrintCommand(callStatement)
            session.WriteLine(callStatement)
        End Sub

#End Region

    End Module

    Namespace Slot

        Public Module SlotSyntax

            Public Const SubsystemNameFormat As String = "_G.slot[{0}]"

            Public Const InterlockStateFormat As String = "_G.slot[{0}].interlock.state"

            Public Const PrintInterlockStateFormat As String = "_G.print(_G.string.format('%d',_G.slot[{0}].interlock.state))"

        End Module

    End Namespace

    Namespace Display

        Public Module DisplaySyntax

            ''' <summary> The name of the display subsystem. </summary>
            Public Const SubsystemName As String = "display"

            ''' <summary> The clear display command. </summary>
            Public Const ClearCommand As String = "display.clear()"

            ''' <summary> The set cursor command format. </summary>
            Public Const SetCursorCommandFormat As String = "display.setcursor({0}, {1})"

            ''' <summary> The set cursor line command format. </summary>
            Public Const SetCursorLineCommandFormat As String = "display.setcursor({0}, 1)"

            ''' <summary> The set Character format. </summary>
            Public Const SetCharacterCommandFormat As String = "display.settext(string.char({0}))"

            ''' <summary> The set text format. </summary>
            Public Const SetTextCommandFormat As String = "display.settext('{0}')"

            ''' <summary> The restore main screen and wait complete command. </summary>
            Public Const RestoreMainWaitCompleteCommand As String = "display.screen = display.MAIN or 0 _G.waitcomplete(0)"

            ''' <summary> The length of the first line. </summary>
            Public Const FirstLineLength As Integer = 20

            ''' <summary> The length of the second line. </summary>
            Public Const SecondLineLength As Integer = 33

            ''' <summary> The maximum character number. </summary>
            Public Const MaximumCharacterNumber As Integer = Short.MaxValue - 1

        End Module

    End Namespace

End Namespace

