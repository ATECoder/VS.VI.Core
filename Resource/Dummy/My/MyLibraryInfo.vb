Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    ''' <remarks> David, 11/26/2015. </remarks>
    Partial Friend NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        Public Const AssemblyTitle As String = "Visa Dummy Library"
        Public Const AssemblyDescription As String = "Dummy Virtual Instrument Library"
        Public Const AssemblyProduct As String = "VI.Dummy.2017"

    End Class

End Namespace

