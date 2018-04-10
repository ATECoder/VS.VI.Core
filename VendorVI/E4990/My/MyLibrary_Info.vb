Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = VI.Pith.My.ProjectTraceEventId.E4990

        Public Const AssemblyTitle As String = "VI E4990 Impedance Analyzer Library"
        Public Const AssemblyDescription As String = "E4990 Impedance Analyzer Virtual Instrument Library"
        Public Const AssemblyProduct As String = "VI.E4990.Analyzer.2018"

    End Class

End Namespace

