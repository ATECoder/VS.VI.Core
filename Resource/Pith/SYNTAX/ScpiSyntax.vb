Namespace Scpi

    Namespace Syntax

        ''' <summary> includes the SCPI Commands. </summary>
        ''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
        ''' Licensed under The MIT License. </para><para>
        ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
        ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
        ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
        ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
        ''' </para> </license>
        ''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created. </history>
        Public Module ScpiSyntax

#Region " FORMAT CONSTANTS "

            ''' <summary> Gets the SCPI value for infinity. </summary>
            Public Const Infinity As Double = 9.9E+37

            ''' <summary> Gets the SCPI caption for infinity. </summary>
            Public Const InfinityCaption As String = "9.90000E+37"

            ''' <summary> Gets the SCPI value for negative infinity. </summary>
            Public Const NegativeInfinity As Double = -9.91E+37

            ''' <summary> Gets the SCPI caption for negative infinity. </summary>
            Public Const NegativeInfinityCaption As String = "-9.91000E+37"

            ''' <summary> Gets the SCPI value for 'non-a-number' (NAN). </summary>
            Public Const NotANumber As Double = 9.91E+37

            ''' <summary> Gets the SCPI caption for 'not-a-number' (NAN). </summary>
            Public Const NotANumberCaption As String = "9.91000E+37"

#End Region

#Region " DEFAULT ERROR MESSAGES "

            ''' <summary> Gets the error message representing no error. </summary>
            Public Const NoErrorMessage As String = "No Error"

            ''' <summary> Gets the compound error message representing no error. </summary>
            Public Const NoErrorCompoundMessage As String = "0,No Error"

#End Region

#Region " STATUS "

            ''' <summary> Gets the error queue query command. </summary>
            Public Const ErrorQueueQueryCommand As String = ":STAT:QUE?"

            ''' <summary> Gets the error queue clear command. </summary>
            Public Const ClearErrorQueueCommand As String = ":STAT:QUE:CLEAR"

            ''' <summary> Gets the questionable event status query command </summary>
            Public Const QuestionableEventQueryCommand As String = ":STAT:QUES:EVEN?"

            ''' <summary> Gets the preset status command. </summary>
            Public Const StatusPresetCommand As String = ":STAT:PRES"

            ''' <summary> Gets the operation event enable command format. </summary>
            Public Const OperationEventEnableCommandFormat As String = ":STAT:OPER:ENAB {0:D}"

            ''' <summary> Gets the operation event enable Query command. </summary>
            Public Const OperationEventEnableQueryCommand As String = ":STAT:OPER:ENAB?"

            ''' <summary> Gets the measurement event status query command. </summary>
            Public Const MeasurementEventQueryCommand As String = ":STAT:MEAS:EVEN?"

            ''' <summary> Gets the measurement event condition command. </summary>
            Public Const MeasurementEventConditionQueryCommand As String = ":STAT:MEAS:COND?"

            ''' <summary> Gets the Measurement event enable Query command. </summary>
            Public Const MeasurementEventEnableQueryCommand As String = ":STAT:MEAS:ENAB?"

            ''' <summary> Gets the Measurement event enable command format. </summary>
            Public Const MeasurementEventEnableCommandFormat As String = ":STAT:MEAS:ENAB {0:D}"

            ''' <summary> Gets the operation register event status query command. </summary>
            Public Const OperationEventQueryCommand As String = ":STAT:OPER:EVEN?"


#End Region

#Region " SYSTEM "

            ''' <summary> Gets the last error queue query command. </summary>
            Public Const LastErrorQueryCommand As String = ":SYST:ERR?"

            ''' <summary> Gets clear system error queue command. </summary>
            Public Const ClearSystemErrorQueueCommand As String = ":SYST:CLE"

            ''' <summary> The read line frequency command. </summary>
            Public Const ReadLineFrequencyCommand As String = ":SYST:LFR?"

            ''' <summary> The initialize memory command. </summary>
            Public Const InitializeMemoryCommand As String = ":SYST:MEM:INIT"

            ''' <summary> The preset command. </summary>
            Public Const SystemPresetCommand As String = ":SYST:PRES"

            ''' <summary> The scpi revision query command. </summary>
            Public Const ScpiRevisionQueryCommand As String = ":SYST:VERS?"

#End Region

        End Module

    End Namespace

End Namespace

