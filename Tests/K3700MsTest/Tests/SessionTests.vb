Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

''' <summary> K3700 Device Session unit tests. </summary>
''' <license>
''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="10/10/2017" by="David" revision=""> Created. </history>
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

#Region " VISA RESOURCE TEST "

    ''' <summary> (Unit Test Method) tests visa resource. </summary>
    <TestMethod()>
    Public Sub VisaResourceTest()
        Dim resourcesFilter As String = DeviceBase.BuildMinimalResourcesFilter
        Dim resources As String()
        Using rm As ResourcesManagerBase = isr.VI.SessionFactory.Get.Factory.CreateResourcesManager()
            resources = rm.FindResources(resourcesFilter).ToArray
        End Using
        Assert.IsTrue(resources.Any, $"VISA Resources {If(resources.Any, "", "not")} found")
        Assert.IsTrue(resources.Contains(TestInfo.ResourceName), $"Resource {TestInfo.ResourceName} not found")
    End Sub

#End Region

#Region " SESSION TESTs: OPEN / CLOSE ONLY "

    ''' <summary> Opens close session. </summary>
    Private Shared Sub OpenCloseSession()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Using device As Device = Device.Create
            Dim e As New isr.Core.Pith.ActionEventArgs
            device.Session.ResourceTitle = TestInfo.ResourceTitle
            actualBoolean = device.TryOpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Failed to open session: {e.Details}")
            Assert.AreEqual(CByte(AscW(device.Session.Termination(0))), device.Session.TerminationCharacter)
            device.Session.Clear()
            device.QueryExistingDeviceErrors(e)
            Assert.IsFalse(e.Cancel, $"Device {TestInfo.ResourceName} failed reading existing errors {e.Details}")
            Dim resport As String = device.StatusSubsystem.DeviceErrorsReport
            Assert.IsTrue(String.IsNullOrWhiteSpace(resport), $"Device {TestInfo.ResourceName} has errors: {resport}")

            Assert.IsFalse(device.StatusSubsystem.LastDeviceError.IsError, $"Device {TestInfo.ResourceName} has last error: {device.StatusSubsystem.LastDeviceError?.ToString}")
            device.CloseSession()
            actualBoolean = device.IsDeviceOpen
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Failed to close session")
        End Using
    End Sub

    ''' <summary> (Unit Test Method) tests open close session. </summary>
    <TestMethod()>
    Public Sub OpenCloseSessionTest()
        SessionTests.OpenCloseSession()
    End Sub

    ''' <summary> (Unit Test Method) tests open close session twice. </summary>
    <TestMethod()>
    Public Sub OpenCloseSessionTwiceTest()
        SessionTests.OpenCloseSession()
        SessionTests.OpenCloseSession()
    End Sub

#End Region

End Class
