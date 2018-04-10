<Assembly: Runtime.CompilerServices.InternalsVisibleTo(My.MyLibrary.E4990Tests)>
<Assembly: Runtime.CompilerServices.InternalsVisibleTo(My.MyLibrary.K3700Tests)>
<Assembly: Runtime.CompilerServices.InternalsVisibleTo(My.MyLibrary.K2450Tests)>
<Assembly: Runtime.CompilerServices.InternalsVisibleTo(My.MyLibrary.K3458Tests)>
<Assembly: Runtime.CompilerServices.InternalsVisibleTo(My.MyLibrary.K7500Tests)>
<Assembly: Runtime.CompilerServices.InternalsVisibleTo(My.MyLibrary.K7500TTests)>

Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = VI.Pith.My.ProjectTraceEventId.Instrument

        Public Const AssemblyTitle As String = "VI Instrument Library"
        Public Const AssemblyDescription As String = "Instrument Virtual Instrument Library"
        Public Const AssemblyProduct As String = "VI.Instrument.2018"
        Public Const E4990Tests As String = "isr.VI.E4990.Tests,PublicKey=" & SolutionInfo.PublicKey
        Public Const K3700Tests As String = "isr.VI.K3700.Tests,PublicKey=" & SolutionInfo.PublicKey
        Public Const K2450Tests As String = "isr.VI.K2450.Tests,PublicKey=" & SolutionInfo.PublicKey
        Public Const K3458Tests As String = "isr.VI.K3458.Tests,PublicKey=" & SolutionInfo.PublicKey
        Public Const K7500Tests As String = "isr.VI.K7500.Tests,PublicKey=" & SolutionInfo.PublicKey
        Public Const K7500TTests As String = "isr.VI.Tsp2.K7500.Tests,PublicKey=" & SolutionInfo.PublicKey

    End Class

End Namespace

