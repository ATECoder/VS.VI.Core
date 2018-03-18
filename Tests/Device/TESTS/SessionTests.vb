''' <summary> A session tests. </summary>
''' <license>
''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="10/11/2017" by="David" revision=""> Created. </history>
<TestClass()>
Public Class SessionTests

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

#Region " TERMINATION "

    <TestMethod()>
    Public Sub InitialTerminationTest()
        Using target As SessionBase = isr.VI.SessionFactory.Get.Factory.CreateSession()
            Assert.AreEqual(CByte(AscW(Environment.NewLine.ToCharArray()(1))), CByte(AscW(target.Termination(0))),
                            "Initial termination character set to line feed")
        End Using
    End Sub

    '''<summary>
    '''A test for New Termination
    '''</summary>
    <TestMethod()>
    Public Sub NewTerminationTest()
        Using target As SessionBase = isr.VI.SessionFactory.Get.Factory.CreateSession()
            Dim values() As Char = Environment.NewLine.ToCharArray
            target.NewTermination(values)
            Assert.AreEqual(values.Length, target.Termination.Count)
            For i As Integer = 0 To values.Length - 1
                Assert.AreEqual(CByte(AscW(values(i))), CByte(AscW(target.Termination(i))))
            Next
        End Using
    End Sub

#End Region

#Region " PARSE "

    '''<summary>
    '''A test for ParseEnumValue
    '''</summary>
    Public Shared Sub ParseEnumValueTestHelper(Of T As Structure)(ByVal value As String, ByVal expected As Nullable(Of T))
        Dim actual As Nullable(Of T)
        actual = SessionBase.ParseEnumValue(Of T)(value)
        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub ParseEnumValueTest()
        SessionTests.ParseEnumValueTestHelper(Of Diagnostics.TraceEventType)("2", TraceEventType.Error)
    End Sub

#End Region

End Class
