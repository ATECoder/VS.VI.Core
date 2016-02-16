Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = isr.VI.My.ProjectTraceEventId.TtmConsole

        Public Const AssemblyTitle As String = "TTM Driver Console"
        Public Const AssemblyDescription As String = "Thermal Transient Meter Driver Console"
        Public Const AssemblyProduct As String = "TTM.Driver.Console.2016"

        ''' <summary> Identifies this talker. </summary>
        ''' <remarks> David, 1/21/2016. </remarks>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            talker?.Publish(TraceEventType.Information, MyApplication.TraceEventId, $"{MyApplication.AssemblyProduct} ID = {MyApplication.TraceEventId:X}")
            isr.VI.Ttm.My.MyLibrary.Identify(talker)
        End Sub

    End Class


End Namespace

