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
Public Class SessionUnitTests

    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Additional test attributes"
    '
    ' You can use the following additional attributes as you write your tests:
    '
    ' Use ClassInitialize to run code before running the first test in the class
    <ClassInitialize()> Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    End Sub
    '
    ' Use ClassCleanup to run code after all tests in a class have run
    ' <ClassCleanup()> Public Shared Sub MyClassCleanup()
    ' End Sub
    '
    ' Use TestInitialize to run code before running each test
    ' <TestInitialize()> Public Sub MyTestInitialize()
    ' End Sub
    '
    ' Use TestCleanup to run code after each test has run
    ' <TestCleanup()> Public Sub MyTestCleanup()
    ' End Sub
    '
#End Region

#Region " CONFIGURATION VERIFICATION "

    ''' <summary> (Unit Test Method) tests domain configuration exists. </summary>
    <TestMethod()>
    Public Sub DomainConfigurationExistsTest()
        Assert.IsTrue(TestInfo.Exists, "App.Config not found")
    End Sub

#End Region

#Region " SESSION TEST "

    '''<summary>
    '''A test for Open Session
    '''</summary>
    <TestMethod()>
    Public Sub OpenSessionTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Using target As Device = New Device()
            target.Session.IsAliveCommand = ""
            target.Session.IsAliveQueryCommand = ""
            ' bit 5 in this instrument
            target.Session.ErrorAvailableBits = ServiceRequests.StandardEvent
            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            target.Session.ResourceTitle = TestInfo.ResourceTitle
            target.Session.OpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, target.CapturedSyncContext)
            actualBoolean = target.Session.IsSessionOpen
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Failed to open session: {e.Details}")

            Dim expectedServiceRequest As Integer = 152
            Dim actualServiceRequest As Integer = target.Session.ReadServiceRequestStatus
            Assert.AreEqual(expectedServiceRequest, actualServiceRequest, $"Service request status on open")

            expectedBoolean = False
            actualBoolean = target.Session.ErrorAvailable
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Error available {target.Session.ServiceRequestStatus:X}")

            Dim expectedReadTerminationEnabled As Boolean = False
            Dim actualReadTerminationEnabled As Boolean
            actualReadTerminationEnabled = target.Session.TerminationCharacterEnabled
            Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Initial read termination enabled")

            expectedReadTerminationEnabled = True
            target.Session.TerminationCharacterEnabled = expectedReadTerminationEnabled
            actualReadTerminationEnabled = target.Session.TerminationCharacterEnabled
            Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Requested read termination enabled")

            Dim expectedTermination As Integer = 10
            Dim actualTermination As Integer
            actualTermination = target.Session.TerminationCharacter
            Assert.AreEqual(expectedTermination, actualTermination, $"Initial read termination character value")

            expectedServiceRequest = 152
            actualServiceRequest = target.Session.ReadServiceRequestStatus
            Assert.AreEqual(expectedServiceRequest, actualServiceRequest, $"Service request status after native termination configuration")

            Dim expectedIdentity As String = "HP3458A"
            Dim actualIdentity As String = target.Session.Query("ID?").Trim
            Assert.AreEqual(expectedIdentity, actualIdentity, $"Reading identity using {target.Session.LastMessageSent}")

            target.Session.Clear()
            target.CloseSession()
        End Using
    End Sub


    <TestMethod()>
    Public Sub OpenDeviceTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Using target As Device = New Device()
            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            target.Session.ResourceTitle = TestInfo.ResourceTitle
            actualBoolean = target.TryOpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Failed to open device: {e.Details}")
            target.Session.Clear()
            target.CloseSession()
        End Using

    End Sub

#End Region

End Class
