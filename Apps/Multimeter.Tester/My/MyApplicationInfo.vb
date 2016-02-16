Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = ProjectTraceEventId.MultimeterTester

        Public Const AssemblyTitle As String = "VI Multimeter Tester"
        Public Const AssemblyDescription As String = "Virtual Instrument Multimeter Tester"
        Public Const AssemblyProduct As String = "VI.Multimeter.Tester.2016"

        ''' <summary> Identifies this talker. </summary>
        ''' <remarks> David, 1/21/2016. </remarks>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            talker?.Publish(TraceEventType.Information, MyApplication.TraceEventId, $"{MyApplication.AssemblyProduct} ID = {MyApplication.TraceEventId:X}")
            isr.VI.K2000.My.MyLibrary.Identify(talker)
            isr.VI.K2700.My.MyLibrary.Identify(talker)
            isr.VI.Tegam.My.MyLibrary.Identify(talker)
        End Sub


    End Class

End Namespace

