''' <summary> A payload base class. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="2/28/2018" by="David" revision=""> Created. </history>
Public MustInherit Class PayloadBase

#Region " CONSTRUCTORS "

    ''' <summary> Specialized default constructor for use only by derived class. </summary>
    Protected Sub New()
        MyBase.New
    End Sub

#End Region

#Region " READ "

    ''' <summary> Gets or sets the reading value as read from the VISA session. </summary>
    ''' <value> The reading. </value>
    Public ReadOnly Property Reading As String

    ''' <summary> Converts the reading to the specific payload such as a read number. </summary>
    ''' <param name="reading"> The reading. </param>
    Public Overridable Sub FromReading(ByVal reading As String)
        Me._Reading = reading
    End Sub

    ''' <summary> Gets or sets the query command. </summary>
    ''' <value> The query command. </value>
    ''' <remarks> This <see cref="T:String"/> is used query the device. </remarks>
    Public Property QueryCommand As String

    ''' <summary> Builds query command. </summary>
    ''' <returns> A String. </returns>
    Public Overridable Function BuildQueryCommand() As String
        Return Me.QueryCommand
    End Function

    ''' <summary> Gets or sets the message that was last sent to the session for querying the device. </summary>
    ''' <value> A query message that was last sent to the device. </value>
    Public Property QueryMessage As String

    ''' <summary> Gets or sets the message that was last received from the session. </summary>
    ''' <value> A message that was last received. </value>
    Public Property ReceivedMessage As String

    ''' <summary> Gets or sets the query status. </summary>
    ''' <value> The query status. </value>
    Public Property QueryStatus As PayloadStatus

    ''' <summary>
    ''' Gets or sets the query status details containing any info on the query status, such as if the
    ''' query failed to parse a value from the reading.
    ''' </summary>
    ''' <value> The query details. </value>
    Public Property QueryStatusDetails As String

    ''' <summary> Gets or sets the emulated payload equaling the emulated reply. </summary>
    ''' <value> The emulated payload reply. </value>
    Public MustOverride ReadOnly Property SimulatedPayload As String

#End Region

#Region " WRITE "

    ''' <summary> Gets or sets the writing value corresponding to the . </summary>
    ''' <value> The writing. </value>
    Public Property Writing As String

    ''' <summary> Converts the specific payload value to a <see cref="Writing">value</see> to send to the session. </summary>
    Public MustOverride Function FromValue() As String

    ''' <summary> Gets or sets the command format. </summary>
    ''' <value> The command format. </value>
    ''' <remarks> This <see cref="T:String"/> is used to format the <see cref="Writing"/> message. </remarks>
    Public Property CommandFormat As String

    ''' <summary> Builds query command. </summary>
    ''' <returns> A String. </returns>
    Public Overridable Function BuildCommand() As String
        Return String.Format(Me.CommandFormat, Me.FromValue)
    End Function

    ''' <summary> Gets or sets the message that was last sent. </summary>
    ''' <value> A message that was last sent. </value>
    Public Property SentMessage As String

    ''' <summary> Gets or sets the Command status. </summary>
    ''' <value> The Command status. </value>
    Public Property CommandStatus As PayloadStatus

    ''' <summary>
    ''' Gets or sets the Command status details containing any info on the Command status, such as if the
    ''' Command failed to parse a value from the reading.
    ''' </summary>
    ''' <value> The Command details. </value>
    Public Property CommandStatusDetails As String

#End Region

End Class

''' <summary> A bit-field of flags for specifying payload status. </summary>
<Flags>
Public Enum PayloadStatus
    None = 0
    Okay = 1 << 0
    Sent = 1 << 1
    QueryReceived = 1 << 2
    QueryParsed = 1 << 3
    QueryParseFailed = 1 << 4
    Failed = 1 << 5
End Enum