Namespace Device.Tests
    ''' <summary> A resource tests. </summary>
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
    Public Class ResourceTests

#Region " CONSTRUCTION + CLEANUP "

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
            Assert.IsTrue(TestInfo.Exists, $"{GetType(TestInfo)} settings not found")
            TestInfo.ClearMessageQueue()
        End Sub

        ''' <summary> Cleans up after each test has run. </summary>
        <TestCleanup()> Public Sub MyTestCleanup()

            TestInfo.AssertMessageQueue()
        End Sub

        '''<summary>
        '''Gets or sets the test context which provides
        '''information about and functionality for the current test run.
        '''</summary>
        Public Property TestContext() As TestContext

#End Region

#Region " PARSE TESTS "

        <TestMethod()>
        Public Sub ParseBooleanTest()
            Dim reading As String = "0"
            Dim expectedResult As Boolean = False
            Dim actualResult As Boolean = True
            Dim successParsing As Boolean = False
            successParsing = VI.Pith.SessionBase.TryParse(reading, actualResult)
            Assert.AreEqual(expectedResult, actualResult, "Value set to {0}", actualResult)
            Assert.AreEqual(True, successParsing, "Success set to {0}", actualResult)
            reading = "1"
            expectedResult = True
            successParsing = VI.Pith.SessionBase.TryParse(reading, actualResult)
            Assert.AreEqual(expectedResult, actualResult, "Value set to {0}", actualResult)
            Assert.AreEqual(True, successParsing, "Success set to {0}", actualResult)
        End Sub

#End Region

#Region " VERSION TESTS "

        ''' <summary> (Unit Test Method) validates the visa version test. </summary>
        <TestMethod()>
        Public Sub ValidateVisaVersionTest()
            Dim e As New Core.Pith.ActionEventArgs
            VI.Pith.ResourcesManagerBase.ValidateFoundationSystemFileVersion(e)
            Assert.IsFalse(e.Cancel, $"Invalid foundation VISA System file version: {e.Details}")
            VI.Pith.ResourcesManagerBase.ValidateFoundationVisaAssemblyVersion(e)
            Assert.IsFalse(e.Cancel, $"Invalid foundation VISA .NET file version: {e.Details}")
            VI.Pith.ResourcesManagerBase.ValidateVendorVersion(e)
            Assert.IsFalse(e.Cancel, $"Invalid national Instrument VISA version: {e.Details}")
        End Sub

#End Region

#Region " VISA RESOURCE TEST "

        ''' <summary> (Unit Test Method) tests locating visa resources. </summary>
        <TestMethod()>
        Public Sub VisaResourcesTest()
            Dim resourcesFilter As String = VI.Pith.ResourceNameInfo.BuildMinimalResourcesFilter
            Dim resources As String()
            Using rm As VI.Pith.ResourcesManagerBase = VI.SessionFactory.Get.Factory.CreateResourcesManager()
                resources = rm.FindResources(resourcesFilter).ToArray
            End Using
            Assert.IsTrue(resources.Any, $"VISA Resources {If(resources.Any, "", "not")} found")
        End Sub

#End Region

    End Class

End Namespace