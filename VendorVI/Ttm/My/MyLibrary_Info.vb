Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = isr.VI.Pith.My.ProjectTraceEventId.TtmDriver

        Public Const AssemblyTitle As String = "VI TTM Library"
        Public Const AssemblyDescription As String = "Thermal Transient Meter Virtual Instrument Library"
        Public Const AssemblyProduct As String = "VI.TTM.2018"

    End Class


End Namespace

