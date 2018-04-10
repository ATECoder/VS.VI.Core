Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Partial Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = VI.Pith.My.ProjectTraceEventId.NationalVisaNonstandard

        Public Const AssemblyTitle As String = "VI National Visa NS Library"
        Public Const AssemblyDescription As String = "National Virtual Instrument VisaNS Library"
        Public Const AssemblyProduct As String = "VI.National.VisaNS.2018"

    End Class

End Namespace

