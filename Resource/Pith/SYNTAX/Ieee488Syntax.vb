Imports System.ComponentModel

Namespace Ieee488

    Namespace Syntax

        ''' <summary> Defines the standard IEEE488 command set. </summary>
        ''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
        ''' Licensed under The MIT License. </para><para>
        ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
        ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
        ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
        ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
        ''' </para> </license>
        ''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created. </history>
        Public Module Ieee488Syntax

#Region " IEEE 488.2 STANDARD COMMANDS "

            ''' <summary> Gets the Clear Status (CLS) command. </summary>
            Public Const ClearExecutionStateCommand As String = "*CLS"

            ''' <summary> Gets the Identity query (*IDN?) command. </summary>
            Public Const IdentityQueryCommand As String = "*IDN?"

            ''' <summary> Gets the operation complete (*OPC) command. </summary>
            Public Const OperationCompletedCommand As String = "*OPC"

            ''' <summary> Gets the operation complete query (*OPC?) command. </summary>
            Public Const OperationCompletedQueryCommand As String = "*OPC?"

            ''' <summary> Gets the options query (*OPT?) command. </summary>
            Public Const OptionsQueryCommand As String = "*OPT?"

            ''' <summary> Gets the Wait (*WAI) command. </summary>
            Public Const WaitCommand As String = "*WAI"

            ''' <summary> Gets the Standard Event Enable (*ESE) command. </summary>
            Public Const StandardEventEnableCommandFormat As String = "*ESE {0:D}"

            ''' <summary> Gets the Standard Event Enable query (*ESE?) command. </summary>
            Public Const StandardEventEnableQueryCommand As String = "*ESE?"

            ''' <summary> Gets the Standard Event Enable (*ESR?) command. </summary>
            Public Const StandardEventQueryCommand As String = "*ESR?"

            ''' <summary> Gets the Service Request Enable (*SRE) command. </summary>
            Public Const ServiceRequestEnableCommandFormat As String = "*SRE {0:D}"

            ''' <summary> Gets the Standard Event and Service Request Enable '*CLS; *ESE {0:D}; *SRE {1:D}' command format. </summary>
            Public Const StandardServiceEnableCommandFormat As String = "*CLS; *ESE {0:D}; *SRE {1:D}"

            ''' <summary> Gets the Standard Event and Service Request Enable '*CLS; *ESE {0:D}; *SRE {1:D} *OPC' command format. </summary>
            Public Const StandardServiceEnableCompleteCommandFormat As String = "*CLS; *ESE {0:D}; *SRE {1:D}; *OPC"

            ''' <summary> Gets the Service Request Enable query (*SRE?) command. </summary>
            Public Const ServiceRequestEnableQueryCommand As String = "*SRE?"

            ''' <summary> Gets the Service Request Status query (*STB?) command. </summary>
            Public Const ServiceRequestQueryCommand As String = "*STB?"

            ''' <summary> Gets the reset to know state (*RST) command. </summary>
            Public Const ResetKnownStateCommand As String = "*RST"

#End Region

#Region " BUILDERS "

            ''' <summary> Builds the device clear (DCL) command. </summary>
            ''' <returns>
            ''' An enumerator that allows for-each to be used to process build device clear command in this
            ''' collection.
            ''' </returns>
            Public Function BuildDeviceClear() As IEnumerable(Of Byte)
                ' Thee DCL command to the interface.
                Dim commands As Byte() = New Byte() {Convert.ToByte(Ieee488.CommandCode.Untalk),
                                             Convert.ToByte(Ieee488.CommandCode.Unlisten),
                                             Convert.ToByte(Ieee488.CommandCode.DeviceClear),
                                             Convert.ToByte(Ieee488.CommandCode.Untalk),
                                             Convert.ToByte(Ieee488.CommandCode.Unlisten)}
                Return commands
            End Function

            ''' <summary> Builds selective device clear (SDC) in this collection. </summary>
            ''' <param name="gpibAddress"> The gpib address. </param>
            ''' <returns>
            ''' An enumerator that allows for-each to be used to process build selective device clear in this
            ''' collection.
            ''' </returns>
            Public Function BuildSelectiveDeviceClear(ByVal gpibAddress As Byte) As IEnumerable(Of Byte)
                Dim commands As Byte() = New Byte() {Convert.ToByte(Ieee488.CommandCode.Untalk),
                                     Convert.ToByte(Ieee488.CommandCode.Unlisten),
                                     Convert.ToByte(Ieee488.CommandCode.ListenAddressGroup) Or Convert.ToByte(gpibAddress),
                                     Convert.ToByte(Ieee488.CommandCode.SelectiveDeviceClear),
                                     Convert.ToByte(Ieee488.CommandCode.Untalk),
                                     Convert.ToByte(Ieee488.CommandCode.Unlisten)}
                Return commands
            End Function

#End Region

        End Module

    End Namespace

    ''' <summary> Values that represent IEEE 488.2 Command Code. </summary>
    Public Enum CommandCode
        None = 0
        <Description("GTL")> GoToLocal = &H1
        <Description("SDC")> SelectiveDeviceClear = &H4
        <Description("GET")> GroupExecuteTrigger = &H8
        <Description("LLO")> LocalLockout = &H11
        <Description("DCL")> DeviceClear = &H14
        <Description("SPE")> SerialPollEnable = &H18
        <Description("SPD")> SerialPollDisable = &H19
        <Description("LAG")> ListenAddressGroup = &H20
        <Description("TAG")> TalkAddressGroup = &H40
        <Description("SCG")> SecondaryCommandGroup = &H60
        <Description("UNL")> Unlisten = &H3F
        <Description("UNT")> Untalk = &H5F
    End Enum

End Namespace
