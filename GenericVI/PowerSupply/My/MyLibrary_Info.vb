Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = VI.Pith.My.ProjectTraceEventId.PowerSupply

        Public Const AssemblyTitle As String = "VI Power Supply Library"
        Public Const AssemblyDescription As String = "Power Supply Virtual Instrument Library"
        Public Const AssemblyProduct As String = "VI.PowerSupply.2018"

    End Class

End Namespace

