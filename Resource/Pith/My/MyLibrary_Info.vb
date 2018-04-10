Imports isr.Core.Pith
Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Partial Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = VI.Pith.My.ProjectTraceEventId.Pith

        Public Const AssemblyTitle As String = "Visa Pith Library"
        Public Const AssemblyDescription As String = "Virtual Instrument Pith Library"
        Public Const AssemblyProduct As String = "VI.Pith.2018"

    End Class

End Namespace

