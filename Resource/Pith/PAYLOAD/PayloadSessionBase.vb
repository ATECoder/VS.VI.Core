Partial Public Class SessionBase

#Region " QUERY PAYLOAD "

    ''' <summary>
    ''' Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    ''' Parses the Boolean return value.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="payload"> The payload. </param>
    ''' <returns> <c>True</c> if <see cref="VI.Pith.PayloadStatus"/> is <see cref="VI.Pith.PayloadStatus.Okay"/>; otherwise <c>False</c>. </returns>
    Public Overloads Function Query(ByVal payload As VI.Pith.PayloadBase) As Boolean
        If payload Is Nothing Then Throw New ArgumentNullException(NameOf(payload))
        Me.EmulatedReply = payload.SimulatedPayload
        payload.QueryStatus = PayloadStatus.Okay Or PayloadStatus.Sent
        payload.QueryMessage = payload.BuildQueryCommand
        payload.ReceivedMessage = Me.QueryTrimEnd(payload.QueryMessage)
        payload.QueryStatus = payload.QueryStatus Or PayloadStatus.QueryReceived
        payload.FromReading(payload.ReceivedMessage)
        Return PayloadStatus.Okay = (payload.QueryStatus And PayloadStatus.Okay)
    End Function

#End Region

#Region " WRITE PAYLOAD "

    ''' <summary> Writes the payload to the device. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="payload"> The payload. </param>
    ''' <returns> <c>True</c> if <see cref="VI.Pith.PayloadStatus"/> is <see cref="VI.Pith.PayloadStatus.Okay"/>; otherwise <c>False</c>. </returns>
    Public Function Write(ByVal payload As VI.Pith.PayloadBase) As Boolean
        If payload Is Nothing Then Throw New ArgumentNullException(NameOf(payload))
        payload.CommandStatus = PayloadStatus.Okay Or PayloadStatus.Sent
        payload.SentMessage = payload.BuildCommand
        Me.WriteLine(payload.SentMessage)
        Return PayloadStatus.Okay = (payload.CommandStatus And PayloadStatus.Okay)
    End Function

#End Region

End Class
