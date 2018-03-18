''' <summary> A trace event identities. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="3/17/2018" by="David" revision=""> Created. </history>
<TestClass()>
Public Class TraceEventIdentities

#Region " CONSTRUCTION AND CLEANUP "

    ''' <summary> My class initialize. </summary>
    ''' <param name="testContext"> Gets or sets the test context which provides information about
    '''                            and functionality for the current test run. </param>
    ''' <remarks>Use ClassInitialize to run code before running the first test in the class</remarks>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <ClassInitialize()>
    Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
        Try
            TestInfo.InitializeTraceListener()
        Catch
            ' cleanup to meet strong guarantees
            Try
                MyClassCleanup()
            Finally
            End Try
            Throw
        End Try
    End Sub

    ''' <summary> My class cleanup. </summary>
    ''' <remarks> Use ClassCleanup to run code after all tests in a class have run. </remarks>
    <ClassCleanup()>
    Public Shared Sub MyClassCleanup()
    End Sub

    ''' <summary> Initializes before each test runs. </summary>
    <TestInitialize()> Public Sub MyTestInitialize()
        Assert.IsTrue(TestInfo.Exists, "App.Config not found")
    End Sub

    ''' <summary> Cleans up after each test has run. </summary>
    <TestCleanup()> Public Sub MyTestCleanup()
    End Sub

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext

#End Region

#Region " SAVE TRACE EVENT IDENTITIES "

    ''' <summary> Save the trace event identities. </summary>
    Private Shared Sub SaveTraceEventIdentities(ByVal fileName As String, ByVal openMode As OpenMode,
                                                ByVal values As IEnumerable(Of KeyValuePair(Of String, Integer)))
        If values Is Nothing Then Throw New ArgumentNullException(NameOf(values))
        Dim fileNo As Integer = Microsoft.VisualBasic.FreeFile()
        If openMode = OpenMode.Output AndAlso System.IO.File.Exists(fileName) Then
            System.IO.File.Delete(fileName)
        End If
        Microsoft.VisualBasic.FileOpen(fileNo, fileName, openMode)
        Microsoft.VisualBasic.WriteLine(fileNo, "hex", "decimal", "description")
        For Each value As KeyValuePair(Of String, Integer) In values
            Microsoft.VisualBasic.WriteLine(fileNo, CInt(value.Value).ToString("X"), value.Value.ToString, value.Key)
        Next
        Microsoft.VisualBasic.FileClose(fileNo)
    End Sub

    ''' <summary> (Unit Test Method) enumerates test event identities. </summary>
    <TestMethod()>
    Public Sub SaveTestEventIdentities()
        Dim fileName As String = TestInfo.TraceEventProjectIdentitiesFileName
        Dim values As New List(Of KeyValuePair(Of String, Integer))
        For Each value As Global.isr.VI.My.ProjectTraceEventId In [Enum].GetValues(GetType(Global.isr.VI.My.ProjectTraceEventId))
            values.Add(New KeyValuePair(Of String, Integer)(isr.Core.Pith.EnumExtensions.Description(value), value))
        Next
        TraceEventIdentities.SaveTraceEventIdentities(fileName, OpenMode.Output, values)
    End Sub

#End Region

End Class
