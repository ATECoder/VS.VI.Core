Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = VI.Pith.My.ProjectTraceEventId.InstrumentScpi

        Public Const AssemblyTitle As String = "VI Instrument Scpi Library"
        Public Const AssemblyDescription As String = "Instrument Virtual Instrument Scpi Library"
        Public Const AssemblyProduct As String = "VI.Instrument.Scpi.2018"

    End Class

End Namespace

