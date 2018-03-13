Partial Public NotInheritable Class TestInfo

#Region " VALIDATIONS "

    ''' <summary> Validated test context. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="testContext"> Gets or sets the test context which provides information about
    '''                            and functionality for the current test run. </param>
    ''' <returns> A TestContext. </returns>
    Public Shared Function ValidatedTestContext(ByVal testContext As TestContext) As TestContext
        If testContext Is Nothing Then Throw New ArgumentNullException(NameOf(testContext))
        Return testContext
    End Function

#End Region

End Class
