Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = ProjectTraceEventId.MultimeterTester

        Public Const AssemblyTitle As String = "VI Multimeter Tester"
        Public Const AssemblyDescription As String = "Virtual Instrument Multimeter Tester"
        Public Const AssemblyProduct As String = "VI.Multimeter.Tester.2017"

        ''' <summary> Gets or sets the identified sentinel. </summary>
        ''' <value> The identified sentinel. </value>
        Public Shared Property Identified As Boolean

        ''' <summary> Identifies this talker. </summary>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
            If Not My.MyLibrary.Identified AndAlso talker.Listeners.ContainsListener(isr.Core.Pith.ListenerType.Logger) Then
                talker.Publish(TraceEventType.Information, MyApplication.TraceEventId, $"{MyApplication.AssemblyProduct} ID = {MyApplication.TraceEventId:X}")
                My.MyLibrary.Identified = True
            End If

        End Sub




    End Class

End Namespace

