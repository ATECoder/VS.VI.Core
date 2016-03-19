Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = 1 ' top level identifier. isr.VI.My.ProjectTraceEventId.K3700Console

        Public Const AssemblyTitle As String = "K3700 Driver Console"
        Public Const AssemblyDescription As String = "K3700 Switching Mainframe Virtual Instrument Console"
        Public Const AssemblyProduct As String = "VI.K3700.Console.2016"

        ''' <summary> Identifies this talker. </summary>
        ''' <remarks> David, 1/21/2016. </remarks>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            talker?.Publish(TraceEventType.Information, MyApplication.TraceEventId, $"{MyApplication.AssemblyProduct} ID = {MyApplication.TraceEventId:X}")
            isr.VI.Tsp.K3700.My.MyLibrary.Identify(talker)
        End Sub

    End Class

End Namespace

