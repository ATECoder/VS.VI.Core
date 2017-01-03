Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    ''' <remarks> David, 11/26/2015. </remarks>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets or sets the log. </summary>
        ''' <value> The log. </value>
        Public Shared ReadOnly Property MyLog As isr.Core.Pith.MyLog = New isr.Core.Pith.MyLog

        ''' <summary> Applies the given value. </summary>
        ''' <remarks> David, 3/16/2016. </remarks>
        ''' <param name="value"> The value. </param>
        Public Shared Sub Apply(ByVal value As isr.Core.Pith.MyLog)
            MyLibrary._MyLog = value
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = ProjectTraceEventId.Device

        Public Const AssemblyTitle As String = "VI Device Library"
        Public Const AssemblyDescription As String = "Virtual Instrument Device Library"
        Public Const AssemblyProduct As String = "VI.Device.2017"

        ''' <summary> Identifies this talker. </summary>
        ''' <remarks> David, 1/21/2016. </remarks>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            talker?.Publish(TraceEventType.Information, MyLibrary.TraceEventId, $"{MyLibrary.AssemblyProduct} ID = {MyLibrary.TraceEventId:X}")
        End Sub

    End Class

End Namespace

