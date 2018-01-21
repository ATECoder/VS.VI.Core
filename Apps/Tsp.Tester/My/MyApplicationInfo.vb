Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = ProjectTraceEventId.TspTester

        Public Const AssemblyTitle As String = "VI TSP Tester"
        Public Const AssemblyDescription As String = "Test Script Processor Virtual Instruments Tester"
        Public Const AssemblyProduct As String = "VI.Tcp.Tester.2018"

        ''' <summary> Gets the identify date. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <value> The identify date. </value>
        Public Shared Property IdentifyDate As Date

        ''' <summary> Identifies this talker. </summary>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
            If DateTime.Now.Date > My.MyLibrary.IdentifyDate AndAlso talker.Listeners.Any AndAlso talker.Listeners.ContainsLoggerListener Then
                talker.PublishOverride(TraceEventType.Information, MyApplication.TraceEventId, MyApplication.Identity)
                MyApplication.IdentifyDate = DateTime.Now.Date
            End If
        End Sub

        ''' <summary> Gets the identity. </summary>
        ''' <value> The identity. </value>
        Public Shared ReadOnly Property Identity() As String
            Get
                Return $"{MyApplication.AssemblyProduct} ID = {MyApplication.TraceEventId:X}"
            End Get
        End Property

    End Class

End Namespace

