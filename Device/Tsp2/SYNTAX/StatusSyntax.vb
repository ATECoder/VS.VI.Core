Namespace TspSyntax.Status

    ''' <summary> Defines the TSP Status syntax. Modified for TSP2. </summary>
    ''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="01/15/05" by="David" revision="1.0.1841.x"> Created. </history>
    Public Module StatusSyntax

        ''' <summary> The Clear Status command (same as '*CLS'). </summary>
        Public Const ClearExecutionStateCommand As String = "_G.status.reset()"

#Region " REGISTERS "

#Region " OPERATION EVENTS "

        ''' <summary> The  operation event enable command format. </summary>
        Public Const OperationEventEnableCommandFormat As String = "_G.status.operation.enable={0}"

        ''' <summary> The  operation event enable print query command. </summary>
        Public Const OperationEventEnablePrintCommand As String = "_G.print(_G.tostring(_G.status.operation.enable))"

        ''' <summary> The  operation event status print query command. </summary>
        Public Const OperationEventPrintCommand As String = "_G.print(_G.tostring(_G.status.operation.event))"

        ''' <summary> The  operation event condition print query command. </summary>
        Public Const OperationEventConditionPrintCommand As String = "_G.print(_G.tostring(_G.status.operation.condition))"

#End Region

#Region " QUESTIONABLE EVENTS "

        ''' <summary> The  questionable event enable command format. </summary>
        Public Const QuestionableEventEnableCommandFormat As String = "_G.status.questionable.enable={0}"

        ''' <summary> The  questionable event enable print query command. </summary>
        Public Const QuestionableEventEnablePrintCommand As String = "_G.print(_G.tostring(_G.status.questionable.enable))"

        ''' <summary> The  questionable event status print query command. </summary>
        Public Const QuestionableEventPrintCommand As String = "_G.print(_G.tostring(_G.status.questionable.event))"

        ''' <summary> The  questionable event condition print query command. </summary>
        Public Const QuestionableEventConditionPrintCommand As String = "_G.print(_G.tostring(_G.status.questionable.condition))"

#End Region

#Region " SERVICE REQUEST "

        ''' <summary> The Service Request Enable *SRE {0:D}' Enable command. </summary>
        ''' <remarks> Same as '*SRE {0:D}'</remarks>
        Public Const ServiceRequestEnableCommandFormat As String = "_G.status.request_enable={0}"

        ''' <summary> The  service request enable print query command. </summary>
        ''' <remarks> Same as '*SRE?'.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Const ServiceRequestEnablePrintCommand As String = "_G.print(_G.tostring(_G.status.request_enable))"

        ''' <summary> The  service request enable print query command builder. </summary>
        ''' <remarks> Same as ''.<para>
        '''           Requires setting the subsystem reference.
        '''           </para>
        '''           </remarks>
        Public Const ServiceRequestEventPrintCommand As String = "_G.print(_G.tostring(_G.status.condition))"

#End Region

#Region " STANDARD EVENTS "

        ''' <summary> The Standard Event Enable command. </summary>
        ''' <remarks> Same as '*ESE {0:D}'</remarks>
        Public Const StandardEventEnableCommandFormat As String = "_G.status.standard.enable={0}"

        ''' <summary> The  standard event enable print query command. </summary>
        ''' <remarks> Same as ''. </remarks>
        Public Const StandardEventEnablePrintCommand As String = "_G.print(_G.tostring(_G.status.standard.enable))"

        ''' <summary> The Standard Event Enable (*ESR?) print query command. </summary>
        ''' <remarks> Same as '*ESR?'</remarks>
        Public Const StandardEventPrintCommand As String = "_G.waitcomplete() _G.print(_G.tostring(_G.status.standard.event))"

#End Region

#Region " SERVICE REQUEST + STANDARD EVENTS "

        Private _StandardServiceEnableCommandFormat As String

        ''' <summary> The Standard Event and Service Request Enable command format. </summary>
        ''' <remarks>
        ''' Same as *CLS; *ESE {0:D}; *SRE {1:D}'
        ''' <para>Using line feed delimiter causes the 2600 instrument to fail when initializing.</para>
        ''' <para>The command works fine with line feeds if issued on its own.</para>
        ''' </remarks>
        ''' <value> The standard service enable command format. </value>
        Public Property StandardServiceEnableCommandFormat As String
            Get
                If String.IsNullOrWhiteSpace(StatusSyntax._StandardServiceEnableCommandFormat) Then
                    StatusSyntax.StandardServiceEnableCommandFormat = String.Format("{0} {1} {2}",
                                                                             StatusSyntax.ClearExecutionStateCommand,
                                                                             StatusSyntax.StandardEventEnableCommandFormat,
                                                                             StatusSyntax.ServiceRequestEnableCommandFormat.Replace("{0}", "{1}"))
                End If
                Return StatusSyntax._StandardServiceEnableCommandFormat
            End Get
            Set(ByVal value As String)
                StatusSyntax._StandardServiceEnableCommandFormat = value
            End Set
        End Property

        Private _StandardServiceEnableCompleteCommandFormat As String

        ''' <summary> The  Standard Event and Service Request Enable command format. </summary>
        ''' <remarks>
        ''' Same as '*CLS; *ESE {0:D}; *SRE {1:D} *OPC'
        ''' <para>Using line feed delimiter causes the 2600 instrument to fail when initializing.</para>
        ''' </remarks>
        ''' <value> The standard service enable complete command format. </value>
        Public Property StandardServiceEnableCompleteCommandFormat As String
            Get
                If String.IsNullOrWhiteSpace(StatusSyntax._StandardServiceEnableCompleteCommandFormat) Then
                    StatusSyntax.StandardServiceEnableCompleteCommandFormat = String.Format("{0} {1}",
                                                                                            StatusSyntax.StandardServiceEnableCommandFormat,
                                                                                            LuaSyntax.OperationCompletedCommand)
                End If
                Return StatusSyntax._StandardServiceEnableCompleteCommandFormat
            End Get
            Set(ByVal value As String)
                StatusSyntax._StandardServiceEnableCompleteCommandFormat = value
            End Set
        End Property

#End Region

#End Region

    End Module

End Namespace

