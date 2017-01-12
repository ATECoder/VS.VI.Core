Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = 1 ' top level identifier. isr.VI.My.ProjectTraceEventId.K3700Console

        Public Const AssemblyTitle As String = "Generic Device Console"
        Public Const AssemblyDescription As String = "Generic Virtual Instrument Console"
        Public Const AssemblyProduct As String = "VI.Device.Console.2017"

        ''' <summary> Identifies this talker. </summary>
        ''' <remarks> David, 1/21/2016. </remarks>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            talker?.Publish(TraceEventType.Information, MyApplication.TraceEventId, $"{MyApplication.AssemblyProduct} ID = {MyApplication.TraceEventId:X}")
            isr.VI.Tsp.K3700.My.MyLibrary.Identify(talker)
            isr.VI.K2400.My.MyLibrary.Identify(talker)
        End Sub

    End Class


End Namespace

