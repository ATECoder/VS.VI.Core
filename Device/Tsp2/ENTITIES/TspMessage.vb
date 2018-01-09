''' <summary> Manages TSP messages. </summary>
''' <remarks> TSP Messages are designed to augment the service request messaging. This may be
''' required whenever the SRQ messaging is insufficient due to the fact that the SRQ messages are
''' unable to report on individual units or nodes. <p>
'''                   </p><p>
''' With TSP only a single register is available for displaying the status of one or more nodes
''' with one or more units (SMU or switches). This the master is unable to report status on each
''' node or unit. For example, the master can raise only a single OPC flag. Thus there is now way
''' to tell which unit finished.  Only that an operation finished. In that case, the output
''' message must be much simpler more like the SRQ bit but for the entire operation rather than
''' the specific command. We need to know the node, unit, and status values.  That could be
''' followed by contents or data.
'''                   </p><p>
''' Message Structure:
'''                   </p><p>
''' Each TSP message consists of a preamble and potential follow up messages.
'''                   </p><p>
''' Preamble:
'''                   </p><p>
''' The preamble is designed to by succinct providing minimal sufficient information to determine
''' status and what to do next. To accomplish this, without reinventing much, the messaging
''' system reports the following register information for the node or the unit:
'''                   </p><p>
''' Service Request Register byte: SRQ, status byte, STB, or status.condition
'''                   </p><p>
''' Status Register Byte:  ESR, or event status register, or status.standard.event
'''                   </p><p>
''' Finally the information included a message available, data available, and buffer available
''' bits.  These help determine whether additional information is already in the output queue or
''' is in the messaging queue.
'''                   </p><p>
''' Format:  All status information is in Hex as follows:
'''                   </p><p>
''' NODE , UNIT , STB , ESR , INFO [, CONTENTS
'''   40 ,    b ,  BF ,  FF , FFFF [, FF, message]
'''                   </p><p>
''' Contents:
'''                   </p><p>
''' Message contents consists of a message type number following by a message contents.
'''                   </p><p>
''' TYPE, MESSAGE
'''   FF, some text
'''                   </p> </remarks>
''' <license> (c) 2007 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/7/2013" by="David" revision="">                  Created. </history>
''' <history date="01/28/2008" by="David" revision="2.0.2949.x">  Convert to .NET
''' Framework. </history>
''' <history date="03/12/2007" by="David" revision="1.15.2627.x"> Created. </history>
Public Class TspMessage

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Copy constructor. </summary>
    ''' <param name="value"> Specifies message received from the instrument. </param>
    Public Sub New(ByVal value As TspMessage)
        Me.new()
        If value Is Nothing Then
            Me.Clear()
        Else
            Me._NodeNumber = value.NodeNumber
            Me._UnitNumber = value.UnitNumber
            Me._StandardEvents = value.StandardEvents
            Me._InfoStatus = value.InfoStatus
            Me._ServiceRequests = value.ServiceRequests
            Me._MessageType = value.MessageType
            Me._Contents = value.Contents
            Me._LastMessage = value.LastMessage
        End If
    End Sub

#End Region

#Region " FIELDS "

    ''' <summary>Gets or sets the message contents.
    ''' </summary>
    Public Property Contents() As String

    ''' <summary>Gets or sets the failure message parsing the message.
    ''' </summary>
    Private _FailureMessage As String

    ''' <summary>Gets or sets the failure message parsing the message.
    ''' </summary>
    Public ReadOnly Property FailureMessage() As String
        Get
            Return Me._failureMessage
        End Get
    End Property

    ''' <summary>Gets or sets the device information
    ''' <see cref="TspMessageInfoBits">status bits</see>.
    ''' </summary>
    Public Property InfoStatus() As TspMessageInfoBits

    ''' <summary>Gets or sets the last message.
    ''' </summary>
    Public Property LastMessage() As String

    ''' <summary>Gets or sets the
    ''' <see cref="TspMessageTypes">message type</see>.
    ''' </summary>
    Public Property MessageType() As TspMessageTypes

    ''' <summary>Gets or sets the one-based node number.
    ''' </summary>
    Public Property NodeNumber() As Integer

    ''' <summary>Gets or sets the service request
    ''' <see cref="VI.ServiceRequests">status bits</see>.
    ''' </summary>
    Public Property ServiceRequests() As VI.ServiceRequests

    ''' <summary>Gets or sets the standard event
    ''' <see cref="VI.StandardEvents">status bits</see>.
    ''' </summary>
    Public Property StandardEvents() As VI.StandardEvents

    ''' <summary>Gets or sets the unit number (a or b).
    ''' </summary>
    Public Property UnitNumber() As String

#End Region

#Region " PARSE "

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Sub Clear()
        Me._LastMessage = ""
        Me._UnitNumber = ""
        Me._NodeNumber = 0
        Me._MessageType = TspMessageTypes.None
        Me._InfoStatus = TspMessageInfoBits.None
        Me._StandardEvents = VI.StandardEvents.None
        Me._ServiceRequests = VI.ServiceRequests.None
        Me._Contents = ""
        Me._failureMessage = ""
    End Sub

    ''' <summary>
    ''' Parses the message received from the instrument.
    ''' </summary>
    ''' <remarks>
    ''' The TSP Message consists of the following elements: <p>
    ''' NODE , UNIT , STB , ESR , INFO [, CONTENTS          </p><p>
    '''   40 ,    b ,  BF ,  FF , FFFF [, FF, message]      </p><p>
    ''' node - node Number                                  </p><p>
    ''' unit - unit number                                  </p><p>
    ''' STB  - status register bits                         </p><p>
    ''' ESR  - standard register bits                       </p><p>
    ''' info - message information bits                     </p><p>
    ''' contents - message data                             </p><p>
    '''                                                     </p>
    ''' </remarks>
    ''' <param name="value">Specifies message received from the instrument.</param>
    Public Sub Parse(ByVal value As String)
        Me.Clear()
        Me._UnitNumber = ""
        Me._NodeNumber = 0
        Me._MessageType = TspMessageTypes.None
        Me._InfoStatus = TspMessageInfoBits.None
        Me._StandardEvents = VI.StandardEvents.None
        Me._ServiceRequests = VI.ServiceRequests.None
        Me._Contents = ""
        Me._failureMessage = ""

        If Not String.IsNullOrWhiteSpace(value) Then

            Dim values As String() = value.Split(","c)

            If values Is Nothing OrElse values.Length <= 0 Then
                Exit Sub
            End If

            ' trim all values.
            For index As Integer = 0 To values.Length - 1
                values(index) = values(index).Trim

                Select Case index
                    Case 0
                        Me._NodeNumber = CInt("&H" & values(index))
                    Case 1
                        Me._UnitNumber = values(index)
                    Case 2
                        Me._ServiceRequests = CType(CInt("&H" & values(index)), VI.ServiceRequests)
                    Case 3
                        Me._StandardEvents = CType(CInt("&H" & values(index)), VI.StandardEvents)
                    Case 4
                        Me._InfoStatus = CType(CInt("&H" & values(index)), TspMessageInfoBits)
                    Case 5
                        Me._MessageType = CType(CInt("&H" & values(index)), TspMessageTypes)
                    Case 6
                        Me._Contents = values(index)
                End Select
            Next index

        End If

    End Sub

#End Region

End Class

''' <summary>Enumerates the TSP Message Type.  This is set as system flags to
''' allow combining types for detecting a super structure.  For example,
''' data available is the sum of binary and ASCII data available bits.</summary>
<Flags()> Public Enum TspMessageTypes
    ''' <summary>0x0000. not defined.</summary>
    <ComponentModel.Description("Not Defined")> None = 0
    ''' <summary>0x001. Sync Instrument message.</summary>
    <ComponentModel.Description("Sync Status")> SyncStatus = CInt(2 ^ 0)
End Enum

''' <summary>Enumerates the TSP Instrument status.</summary>
<Flags()> Public Enum TspMessageInfoBits
    ''' <summary>0x0000. not defined.</summary>
    <ComponentModel.Description("Not Defined")> None = 0
    ''' <summary>0x0001. New message.  Not yet handled.</summary>
    <ComponentModel.Description("New Message")> NewMessage = CInt(2 ^ 0)
    ''' <summary>0x0002. Debug message.  Debug messages always have
    ''' Contents following the information register.
    ''' .</summary>
    <ComponentModel.Description("Debug Message")> DebugMessage = CInt(2 ^ 1)
    ''' <summary>0x0004. Non zero if unit or node is still active.</summary>
    <ComponentModel.Description("Tsp Active")> TspActive = CInt(2 ^ 2)
    ''' <summary>0x0008. Non zero if unit or node had an error.</summary>
    <ComponentModel.Description("Tsp Error")> TspError = CInt(2 ^ 3)
    ''' <summary>0x0010. Source function in compliance.</summary>
    <ComponentModel.Description("Compliance")> Compliance = CInt(2 ^ 4)
    ''' <summary>0x0020. Non zero if unit has taken new “chunks” of data in its
    ''' buffer.</summary>
    <ComponentModel.Description("Data Available")> DataAvailable = CInt(2 ^ 5)
End Enum

