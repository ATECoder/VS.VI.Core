Imports System.Data.Common
''' <summary> Information about the test. </summary>
''' <license>
''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="10/13/2017" by="David" revision=""> Created. </history>
Partial Public NotInheritable Class TestInfo

#Region " CONSTRUCTORS"

    Private Sub New()
        MyBase.New
    End Sub

#End Region

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

#Region " TRACE "

    ''' <summary> Trace message. </summary>
    ''' <param name="format"> Describes the format to use. </param>
    ''' <param name="args">   A variable-length parameters list containing arguments. </param>
    Public Shared Sub TraceMessage(ByVal format As String, ByVal ParamArray args() As Object)
        TestInfo.TraceMessage(String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
    End Sub

    ''' <summary> Trace message. </summary>
    ''' <param name="message"> The message. </param>
    Public Shared Sub TraceMessage(ByVal message As String)
        System.Diagnostics.Debug.WriteLine(message)
        Console.Out.WriteLine(message)
    End Sub

#End Region

End Class
