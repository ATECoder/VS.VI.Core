Namespace LuaSyntax

    ''' <summary> Defines the syntax of the LUA base system. </summary>
    ''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="01/15/05" by="David" revision="1.0.1841.x"> Created. </history>
    Public Module LuaSyntaxConstants

#Region " CONSTANTS "

        ''' <summary> Represents the LUA nil value. </summary>
        Public Const NilValue As String = "nil"

        ''' <summary> Represents the LUA true value </summary>
        Public Const TrueValue As String = "true"

        ''' <summary> Represents the LUA false value </summary>
        Public Const FalseValue As String = "false"

        ''' <summary> The global node reference. </summary>
        Public Const GlobalNode As String = "_G"

#End Region

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

#Region " SCRIPT COMMANDS "

        ''' <summary>
        ''' Gets a command to retrieve a catalog from the local node.
        ''' This command must be enclosed in a 'do end' construct.
        ''' a print(names) or dataqueue.add(names) needs to be added to get the data through.
        ''' </summary>
        Public Const ScriptCatalogGetterCommand As String = "local names='' for name in script.user.catalog() do names = names .. name .. ',' end"

#End Region

#Region " SYSTEM COMMANDS "

        ''' <summary> The collect garbage command. </summary>
        Public Const CollectGarbageCommand As String = "_G.collectgarbage()"

        ''' <summary> The  collect garbage command. </summary>
        Public Const CollectGarbageWaitCompleteCommand As String = "_G.collectgarbage() _G.waitcomplete(0)"

        ''' <summary> The operation completed command (Same as '*OPC'). </summary>
        Public Const OperationCompletedCommand As String = "_G.opc()"

        ''' <summary> Gets or set the operation completed query command. </summary>
        ''' <remarks> Same as '*OPC?'. </remarks>
        Public Const OperationCompletedQueryCommand As String = "_G.waitcomplete() print('1') "

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
            Return TspSyntax.Build(LuaSyntaxConstants.PrintCommandFormat, args)
        End Function

        ''' <summary> The reset to known state command (Same as '*RST'). </summary>
        Public Const ResetKnownStateCommand As String = "_G.reset()"

        ''' <summary> The wait command. </summary>
        Public Const WaitGroupCommandFormat As String = "_G.waitcomplete({0})"

        ''' <summary> The wait command. </summary>
        Public Const WaitCommand As String = "_G.waitcomplete()"

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

#Region " SYSTEM COMMANDS "

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
                Return Build(LuaSyntaxConstants._callFunctionCommandFormat, functionName)
            Else
                Return Build(LuaSyntaxConstants._callFunctionArgumentsCommandFormat, functionName, args)
            End If

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
        Public Sub CallFunction(ByVal session As VI.Pith.SessionBase, ByVal functionName As String, ByVal args As String)
            If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
            If String.IsNullOrWhiteSpace(functionName) Then Throw New ArgumentNullException(NameOf(functionName))
            Dim callStatement As String
            callStatement = LuaSyntaxConstants.CallFunctionCommand(functionName, args)
            callStatement = LuaSyntaxConstants.PrintCommand(callStatement)
            session.WriteLine(callStatement)
        End Sub

#End Region

    End Module

End Namespace

