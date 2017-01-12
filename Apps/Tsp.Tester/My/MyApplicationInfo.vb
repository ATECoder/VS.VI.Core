Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = ProjectTraceEventId.TspTester

        Public Const AssemblyTitle As String = "VI TSP Tester"
        Public Const AssemblyDescription As String = "Test Script Processor Virtual Instruments Tester"
        Public Const AssemblyProduct As String = "VI.Tcp.Tester.2017"

        ''' <summary> Identifies this talker. </summary>
        ''' <remarks> David, 1/21/2016. </remarks>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            talker?.Publish(TraceEventType.Information, MyApplication.TraceEventId, $"{MyApplication.AssemblyProduct} ID = {MyApplication.TraceEventId:X}")
            isr.VI.Tsp.Instrument.My.MyLibrary.Identify(talker)
            isr.VI.Tsp.Script.My.MyLibrary.Identify(talker)
        End Sub

    End Class


End Namespace

