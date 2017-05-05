Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = ProjectTraceEventId.OhmniTester

        Public Const AssemblyTitle As String = "VI Ohm Prober Tester"
        Public Const AssemblyDescription As String = "Ohm Prober Virtual Instruments Tester"
        Public Const AssemblyProduct As String = "VI.OhmProber.Tester.2017"

        ''' <summary> Identifies this talker. </summary>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            talker?.Publish(TraceEventType.Information, MyApplication.TraceEventId, $"{MyApplication.AssemblyProduct} ID = {MyApplication.TraceEventId:X}")
            isr.VI.K2000.My.MyLibrary.Identify(talker)
            isr.VI.EG2000.My.MyLibrary.Identify(talker)
            isr.VI.Tegam.My.MyLibrary.Identify(talker)
        End Sub


    End Class

End Namespace

