Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = VI.Pith.My.ProjectTraceEventId.K34980

        Public Const AssemblyTitle As String = "VI K34980 Meter Scanner Library"
        Public Const AssemblyDescription As String = "K34980 Meter Scanner Virtual Instrument Library"
        Public Const AssemblyProduct As String = "VI.K34980.Meter.Scanner.2018"

    End Class

End Namespace

