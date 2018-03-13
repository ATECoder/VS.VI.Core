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

    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region

    '''<summary>
    '''A test for Initial Termination
    '''</summary>
    <TestMethod()>
    Public Sub InitialTerminationTest()
        Dim resourceName As String = "GPIB0::22::INSTR"
        Dim target As SessionBase = isr.VI.SessionFactory.Get.Factory.CreateSession()
        target.OpenSession(resourceName, Threading.SynchronizationContext.Current)
        Assert.AreEqual(CByte(AscW(target.Termination(0))), target.TerminationCharacter)
        Assert.AreEqual(target.Termination(0), Convert.ToChar(target.TerminationCharacter))
    End Sub

    '''<summary>
    '''A test for New Termination
    '''</summary>
    <TestMethod()>
    Public Sub NewTerminationTest()
        Dim target As SessionBase = isr.VI.SessionFactory.Get.Factory.CreateSession()
        Dim values() As Char = Environment.NewLine.ToCharArray
        target.NewTermination(values)
        Assert.AreEqual(values.Length, target.Termination.Count)
        For i As Integer = 0 To values.Length - 1
            Assert.AreEqual(CByte(AscW(values(i))), CByte(AscW(target.Termination(i))))
        Next
    End Sub

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

End Class
