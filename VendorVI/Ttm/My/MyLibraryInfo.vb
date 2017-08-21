Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = isr.VI.My.ProjectTraceEventId.TtmDriver

        Public Const AssemblyTitle As String = "VI TTM Library"
        Public Const AssemblyDescription As String = "Thermal Transient Meter Virtual Instrument Library"
        Public Const AssemblyProduct As String = "VI.TTM.2017"

        ''' <summary> Gets or sets the identified sentinel. </summary>
        ''' <value> The identified sentinel. </value>
        Public Shared Property Identified As Boolean

        ''' <summary> Identifies this talker. </summary>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
            If Not My.MyLibrary.Identified AndAlso talker.Listeners.ContainsListener(isr.Core.Pith.ListenerType.Logger) Then
                talker.Publish(TraceEventType.Information, MyLibrary.TraceEventId, $"{MyLibrary.AssemblyProduct} ID = {MyLibrary.TraceEventId:X}")
                My.MyLibrary.Identified = True
            End If
        End Sub


    End Class


End Namespace

