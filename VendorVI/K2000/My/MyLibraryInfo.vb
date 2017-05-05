Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = VI.My.ProjectTraceEventId.K2000

        Public Const AssemblyTitle As String = "VI K2000 Meter Library"
        Public Const AssemblyDescription As String = "K2000 Meter Virtual Instrument Library"
        Public Const AssemblyProduct As String = "VI.K2000.Meter.2017"

        ''' <summary> Identifies this talker. </summary>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            talker?.Publish(TraceEventType.Information, MyLibrary.TraceEventId, $"{MyLibrary.AssemblyProduct} ID = {MyLibrary.TraceEventId:X}")
            isr.VI.Scpi.My.MyLibrary.Identify(talker)
            isr.VI.Instrument.My.MyLibrary.Identify(talker)
        End Sub

    End Class

End Namespace

