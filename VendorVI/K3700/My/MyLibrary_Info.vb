<Assembly: Runtime.CompilerServices.InternalsVisibleTo(My.MyLibrary.K3700Tests)>
Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = VI.Pith.My.ProjectTraceEventId.K3700

        Public Const AssemblyTitle As String = "VI K3700 Meter Scanner Library"
        Public Const AssemblyDescription As String = "K3700 Meter Scanner Virtual Instrument Library"
        Public Const AssemblyProduct As String = "VI.K3700.Meter.Scanner.2018"
        Public Const K3700Tests As String = "isr.VI.K3700.Tests,PublicKey=" & SolutionInfo.PublicKey

    End Class

End Namespace

