Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

''' <summary> K3458 Device Session unit tests. </summary>
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

#Region " SESSION TEST "

    '''<summary>
    '''A test for Open Session
    '''</summary>
    <TestMethod()>
    Public Sub OpenSessionTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Using device As Device = New Device()
            device.Session.IsAliveCommand = ""
            device.Session.IsAliveQueryCommand = ""
            ' bit 5 in this instrument
            device.Session.ErrorAvailableBits = ServiceRequests.StandardEvent
            Dim e As New isr.Core.Pith.ActionEventArgs
            device.Session.ResourceTitle = TestInfo.ResourceTitle
            device.Session.OpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, device.CapturedSyncContext)
            Assert.AreEqual(CByte(AscW(device.Session.Termination(0))), device.Session.TerminationCharacter)
            actualBoolean = device.Session.IsSessionOpen
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Failed to open session: {e.Details}")

            Dim expectedServiceRequest As Integer = 152
            Dim actualServiceRequest As Integer = device.Session.ReadServiceRequestStatus
            Assert.AreEqual(expectedServiceRequest, actualServiceRequest, $"Service request status on open")

            expectedBoolean = False
            actualBoolean = device.Session.ErrorAvailable
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Error available {device.Session.ServiceRequestStatus:X}")

            Dim expectedReadTerminationEnabled As Boolean = False
            Dim actualReadTerminationEnabled As Boolean
            actualReadTerminationEnabled = device.Session.TerminationCharacterEnabled
            Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Initial read termination enabled")

            expectedReadTerminationEnabled = True
            device.Session.TerminationCharacterEnabled = expectedReadTerminationEnabled
            actualReadTerminationEnabled = device.Session.TerminationCharacterEnabled
            Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Requested read termination enabled")

            Dim expectedTermination As Integer = 10
            Dim actualTermination As Integer
            actualTermination = device.Session.TerminationCharacter
            Assert.AreEqual(expectedTermination, actualTermination, $"Initial read termination character value")

            expectedServiceRequest = 152
            actualServiceRequest = device.Session.ReadServiceRequestStatus
            Assert.AreEqual(expectedServiceRequest, actualServiceRequest, $"Service request status after native termination configuration")

            Dim expectedIdentity As String = "HP3458A"
            Dim actualIdentity As String = device.Session.Query("ID?").Trim
            Assert.AreEqual(expectedIdentity, actualIdentity, $"Reading identity using {device.Session.LastMessageSent}")

            device.Session.Clear()
            device.CloseSession()
        End Using
    End Sub


    <TestMethod()>
    Public Sub OpenDeviceTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Using target As Device = New Device()
            Dim e As New isr.Core.Pith.ActionEventArgs
            target.Session.ResourceTitle = TestInfo.ResourceTitle
            actualBoolean = target.TryOpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Failed to open device: {e.Details}")
            target.Session.Clear()
            target.CloseSession()
        End Using

    End Sub

#End Region

End Class
