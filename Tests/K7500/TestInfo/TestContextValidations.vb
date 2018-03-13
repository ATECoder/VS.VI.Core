Imports System.Data.Common

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

    ''' <summary> Validated data row. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="testContext"> Gets or sets the test context which provides information about
    '''                            and functionality for the current test run. </param>
    ''' <returns> A DataRow. </returns>
    Public Shared Function ValidatedDataRow(ByVal testContext As TestContext) As DataRow
        If testContext Is Nothing Then Throw New ArgumentNullException(NameOf(testContext))
        Return TestInfo.ValidatedDataRow(testContext.DataRow)
    End Function

    ''' <summary> Validated data row. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="dataRow"> The data row. </param>
    ''' <returns> A DataRow. </returns>
    Public Shared Function ValidatedDataRow(ByVal dataRow As DataRow) As DataRow
        If dataRow Is Nothing Then Throw New ArgumentNullException(NameOf(dataRow))
        Return dataRow
    End Function

    ''' <summary> Validated connection. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="testContext"> Gets or sets the test context which provides information about
    '''                            and functionality for the current test run. </param>
    ''' <returns> A DbConnection. </returns>
    Public Shared Function ValidatedConnection(ByVal testContext As TestContext) As DbConnection
        If testContext Is Nothing Then Throw New ArgumentNullException(NameOf(testContext))
        Return TestInfo.Validatedconnection(testContext.DataConnection)
    End Function

    ''' <summary> Validated connection. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="connection"> The connection. </param>
    ''' <returns> A connection. </returns>
    Public Shared Function Validatedconnection(ByVal connection As DbConnection) As DbConnection
        If connection Is Nothing Then Throw New ArgumentNullException(NameOf(connection))
        Return connection
    End Function

#End Region

End Class
