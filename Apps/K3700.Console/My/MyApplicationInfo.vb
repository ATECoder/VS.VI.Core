Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = 1 ' top level identifier. isr.VI.My.ProjectTraceEventId.K3700Console

        Public Const AssemblyTitle As String = "K3700 Driver Console"
        Public Const AssemblyDescription As String = "K3700 Switching Mainframe Virtual Instrument Console"
        Public Const AssemblyProduct As String = "VI.K3700.Console.2017"

        ''' <summary> Gets or sets the identified sentinel. </summary>
        ''' <value> The identified sentinel. </value>
        Public Shared Property Identified As Boolean

        ''' <summary> Identifies this talker. </summary>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
            If Not My.MyApplication.Identified AndAlso talker.Listeners.ContainsListener(isr.Core.Pith.ListenerType.Logger) Then
                talker.Publish(TraceEventType.Information, MyApplication.TraceEventId, $"{MyApplication.AssemblyProduct} ID = {MyApplication.TraceEventId:X}")
                My.MyApplication.Identified = True
            End If
        End Sub


    End Class


End Namespace

