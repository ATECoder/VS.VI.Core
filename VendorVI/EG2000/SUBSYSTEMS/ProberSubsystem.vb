Imports System.ComponentModel
''' <summary> Defines a Prober Subsystem for a EG2000 Prober. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/01/2013" by="David" revision="3.0.5022"> Created. </history>
Public Class ProberSubsystem
    Inherits VI.R2D2.ProberSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.ErrorReplyPattern = "E"
        Me.MessageFailedPattern = "MF"
        Me.MessageCompletedPattern = "MC"
        Me.FirstTestStartPattern = "TF"
        Me.PatternCompleteReplyPattern = "PC"
        Me.RetestStartPattern = "TR"
        Me.SetModeCommandPrefix = "SM"
        Me.TestStartPattern = "TS"
        Me.TestAgainStartPattern = "TA"
        Me.WaferStartPattern = "WB"
        Me.TestCompleteCommand = "TC"
        Me.IdentityReplyPattern = "2001X."
        Me.SetSupportedCommandPrefixes(New String() {Me.SetModeCommandPrefix, "ID", Me.TestCompleteCommand, "?E"})
        Me.SetSupportedCommands(New String() {"SM15M101110110001000001", "ID", Me.TestCompleteCommand, "?E"})
        Me.SetSupportedEmulationCommands(New String() {"E28", "2001X.CD.249799-011", Me.PatternCompleteReplyPattern,
                                                               Me.TestAgainStartPattern, Me.FirstTestStartPattern,
                                                               Me.RetestStartPattern, Me.TestStartPattern, "WBI/-299",
                                                               Me.MessageCompletedPattern, Me.MessageFailedPattern})
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Initialize the response mode.
    '''           </para></remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        ' enable MF/MC handshake
        ' SM15M101110110001000001
        Dim mode As ResponseModes = ResponseModes.None Or
                ResponseModes.HandshakePosition Or
                ResponseModes.HandshakeDeviceCommands Or
                ResponseModes.HandshakeCommands Or
                ResponseModes.TestStartSent Or
                ResponseModes.PatternCompleteResponse Or
                ResponseModes.PauseContinueResponse Or
                ResponseModes.EnhancedTestStart Or
                ResponseModes.WaferBegin Or
                ResponseModes.None
        ' sends the message to the prober and waits for the reply.
        ' if failed, try again or throw and exception.
        If Not Me.TryWriteResponseMode(mode, False) Then Me.TryWriteResponseMode(mode, True)
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.ResponseMode = ResponseModes.EnhancedTestStart Or
                ResponseModes.EnhancedPatternComplete Or
                ResponseModes.HandshakePosition Or
                ResponseModes.HandshakeDeviceCommands Or
                ResponseModes.PatternCompleteResponse Or
                ResponseModes.TestStartSent Or
                ResponseModes.WaferComplete
    End Sub
#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#End Region

#Region " RESPONSE MODE "

    ''' <summary> The Response Mode. </summary>
    Private _ResponseMode As ResponseModes?

    ''' <summary> Gets or sets the cached Response Mode. </summary>
    ''' <value> The Response Mode or null if unknown. </value>
    Public Property ResponseMode As ResponseModes?
        Get
            Return Me._ResponseMode
        End Get
        Protected Set(ByVal value As ResponseModes?)
            If Not Nullable.Equals(Me.ResponseMode, value) Then
                Me._ResponseMode = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries the Response Mode. Also sets the <see cref="ResponseMode"></see> cached value. </summary>
    ''' <returns> The Response Mode or null if unknown. </returns>
    ''' <remarks> The 2001x does not returns the expected replay. It returns: 'SZDW0C1'</remarks>
    Public Function QueryResponseMode() As ResponseModes?
        Dim mode As String = Me.ResponseMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryTrimEnd("?SM15")
        If String.IsNullOrWhiteSpace(mode) Then
            Me.ResponseMode = ResponseModes.None
        Else
            ' reverse the order so as to match the enumeration.
            mode = mode.ToCharArray.Reverse.ToString
            Me.ResponseMode = CType(Convert.ToInt32(mode, 2), ResponseModes)
        End If
        Return Me.ResponseMode
    End Function

    ''' <summary> Writes the Response Mode and reads back a reply from the instrument. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="mode">           The mode. </param>
    ''' <param name="raiseException"> true to raise exception. </param>
    Public Function TryWriteResponseMode(mode As ResponseModes, ByVal raiseException As Boolean) As Boolean
        Dim affirmative As Boolean = False
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Setting response mode to 0x{0:X4};. ", CInt(mode))
        If Me.WriteResponseMode(mode).HasValue Then
            Dim bit As VI.Pith.ServiceRequests = Me.StatusSubsystem.MessageAvailableBits
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Awaiting SQR equal to 0x{0:X2};. ", CInt(bit))
            ' emulate return value is session not open.
            Me.Session.EmulatedStatusByte = bit
            If Me.StatusSubsystem.TryAwaitServiceRequest(bit, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(10)) Then
                Me.Session.MakeEmulatedReplyIfEmpty(Me.MessageCompletedPattern)
                Me.FetchAndParse()
                If Me.MessageCompleted Then
                    affirmative = True
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Response mode set to 0x{0:X4};. ", CInt(Me.ResponseMode))
                ElseIf Me.MessageFailed Then
                    If raiseException Then
                        Throw New VI.Pith.OperationFailedException("Message Failed initializing response mode to 0x{0:X4};.", CInt(mode))
                    Else
                        Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                          "Message Failed initializing response mode to 0x{0:X4};. ", CInt(mode))
                    End If
                Else
                    If raiseException Then
                        Throw New VI.Pith.OperationFailedException("Unexpected reply '{0}' initializing response mode;. ", Me.LastReading)
                    Else
                        Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                          "Unexpected reply '{0}' initializing response mode;. ", Me.LastReading)
                    End If
                End If
            Else
                If raiseException Then
                    Throw New VI.Pith.OperationFailedException("Timeout waiting SRQ 0x{0:X2} after setting response mode;. ", CInt(bit))
                Else
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                      "Timeout waiting SRQ 0x{0:X2} after setting response mode;. ", CInt(bit))
                End If
            End If
        Else
            If raiseException Then
                Throw New VI.Pith.OperationFailedException("Failed initializing response mode.")
            Else
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Failed initializing response mode.")
            End If
        End If
        Return affirmative
    End Function

    ''' <summary> Writes the Response Mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Response Mode. </param>
    ''' <returns> The Response Mode or null if unknown. </returns>
    Public Function WriteResponseMode(ByVal value As ResponseModes) As ResponseModes?
        Dim chars As Char() = Convert.ToString(value, 2).ToCharArray
        Array.Reverse(chars)
        Me.Session.WriteLine("SM15M{0}", New String(chars))
        Me.ResponseMode = value
        Return Me.ResponseMode
    End Function

    ''' <summary> Writes the Response Mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Response Mode. </param>
    ''' <returns> The Response Mode or null if unknown. </returns>
    Public Function WriteAsyncResponseMode(ByVal value As ResponseModes) As ResponseModes?
        Dim chars As Char() = Convert.ToString(value, 2).ToCharArray
        Array.Reverse(chars)
        Dim responseMode As String = New String(chars)
        Dim msg As String = String.Format(Globalization.CultureInfo.InvariantCulture, "SM15M{0}", responseMode)
        If Me.TrySendAsync(msg, 3, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(1000)) Then
            Me.ResponseMode = value
        Else
            Me.ResponseMode = New ResponseModes?
        End If
        Return Me.ResponseMode
    End Function

    ''' <summary> Writes and reads back the Response Mode. </summary>
    ''' <param name="value"> The <see cref="ResponseModes">Response Mode</see>. </param>
    ''' <returns> The Response Mode or null if unknown. </returns>
    Public Function ApplyAsyncResponseMode(ByVal value As ResponseModes) As ResponseModes?
        If Me.WriteAsyncResponseMode(value).HasValue Then
            Return Me.QueryResponseMode()
        Else
            Return New ResponseModes?
        End If
    End Function

#End Region

End Class

''' <summary> Enumerates the responses. </summary>
''' <remarks> See Electroglass Manual page 4-9 (23) for detailed descriptions. </remarks>
<Flags()>
Public Enum ResponseModes
    <Description("Not set")> None
    <Description("1: MC/MF on X/Y")> HandshakePosition = CInt(2 ^ 0)
    <Description("2: MC/MF on Z")> HandshakeChuck = CInt(2 ^ 1)
    <Description("3: MC/MF on device commands")> HandshakeDeviceCommands = CInt(2 ^ 2)
    <Description("4: MC/MF on balance parameter commands")> HandshakeCommands = CInt(2 ^ 3)
    <Description("5: TS sent")> TestStartSent = CInt(2 ^ 4)
    <Description("6: TC response")> TestCompleteResponse = CInt(2 ^ 5)
    <Description("7: PC response")> PatternCompleteResponse = CInt(2 ^ 6)
    <Description("8: PA/CO response")> PauseContinueResponse = CInt(2 ^ 7)
    <Description("9: Alarm response")> AlarmResponse = CInt(2 ^ 8)
    <Description("10: WC (Wafer complete)")> WaferComplete = CInt(2 ^ 9)
    <Description("11: Enhanced PC")> EnhancedPatternComplete = CInt(2 ^ 10)
    <Description("12: Enhanced TS")> EnhancedTestStart = CInt(2 ^ 11)
    <Description("13: Ugly Die Report")> UglyDieReport = CInt(2 ^ 12)
    <Description("14: Map Transfer Retries")> MapTransferRetries = CInt(2 ^ 13)
    <Description("15: Send Coordinates With Test Start")> SendCoordinatesWithTestStart = CInt(2 ^ 14)
    <Description("16: Send EC/BC (End/Begin Cassette) Messages")> SendCassetteMessages = CInt(2 ^ 15)
    <Description("17: Pause Pending")> PausePending = CInt(2 ^ 16)
    <Description("18: Wafer Begin")> WaferBegin = CInt(2 ^ 17)
End Enum
