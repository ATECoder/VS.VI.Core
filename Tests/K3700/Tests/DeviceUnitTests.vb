Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

''' <summary> K3700 Device unit tests. </summary>
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
Public Class DeviceUnitTests

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

#Region " RESOURCE TEST "

    ''' <summary> (Unit Test Method) tests device resource. </summary>
    <TestMethod()>
    Public Sub DeviceResourceTest()
        Using target As Device = Device.Create
            Assert.IsTrue(target.Find(TestInfo.ResourceName), $"VISA Resource {TestInfo.ResourceName} not found")
        End Using
    End Sub

#End Region

#Region " DEVICE TESTS: OPEN, CLOSE, CHECK SUSBSYSTEMS "

    ''' <summary> Opens a session. </summary>
    ''' <param name="device"> The device. </param>
    Private Shared Sub OpenSession(ByVal device As Device)

        Dim actualCommand As String = device.Session.IsAliveCommand
        Dim expectedCommand As String = TestInfo.KeepAliveCommand
        Assert.AreEqual(expectedCommand, actualCommand, $"Keep alive command.")

        actualCommand = device.Session.IsAliveQueryCommand
        expectedCommand = TestInfo.KeepAliveQueryCommand
        Assert.AreEqual(expectedCommand, actualCommand, $"Keep alive query command.")

        Dim expectedErrorAvailableBits As Integer = ServiceRequests.ErrorAvailable
        Dim actualErrorAvailableBits As Integer = device.Session.ErrorAvailableBits
        Assert.AreEqual(expectedErrorAvailableBits, actualErrorAvailableBits, $"Error available bits on creating device.")

        device.Session.ResourceTitle = TestInfo.ResourceTitle
        Dim e As New isr.Core.Pith.CancelDetailsEventArgs
        Dim actualBoolean As Boolean = device.TryOpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, e)
        Assert.IsTrue(actualBoolean, $"Failed to open session: {e.Details}")

    End Sub

    ''' <summary> Closes a session. </summary>
    ''' <param name="device"> The device. </param>
    Private Shared Sub CloseSession(ByVal device As Device)
        device.Session.Clear()
        device.CloseSession()
        Assert.IsFalse(device.IsDeviceOpen, $"Failed to close session")
    End Sub

    ''' <summary> Check session info. </summary>
    ''' <param name="device"> The device. </param>
    Private Shared Sub CheckSessionInfo(ByVal device As Device)

        Dim expectedModel As String = TestInfo.ResourceModel
        Dim actualModel As String = device.StatusSubsystem.VersionInfo.Model
        Assert.AreEqual(expectedModel, actualModel, $"Reading version info")

        Assert.IsFalse(device.Session.ErrorAvailable, $"Error available {device.Session.ServiceRequestStatus:X} device errors: {device.StatusSubsystem.LastDeviceError}")

        Dim actualReadTerminationEnabled As Boolean = device.Session.TerminationCharacterEnabled
        Dim expectedReadTerminationEnabled As Boolean = TestInfo.ReadTerminationEnabled
        Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Initial read termination enabled")

        expectedReadTerminationEnabled = TestInfo.ReadTerminationEnabled
        device.Session.TerminationCharacterEnabled = expectedReadTerminationEnabled
        actualReadTerminationEnabled = device.Session.TerminationCharacterEnabled
        Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Requested read termination")

        Dim actualTermination As Integer = device.Session.TerminationCharacter
        Dim expectedTermination As Integer = TestInfo.TerminationCharacter
        Assert.AreEqual(expectedTermination, actualTermination, $"Termination character value")

    End Sub

    ''' <summary> Check status subsystem information. </summary>
    ''' <param name="device"> The device. </param>
    Private Shared Sub CheckStatusSubsystemInfo(ByVal device As Device)

        Dim actualFrequency As Double = device.StatusSubsystem.LineFrequency.GetValueOrDefault(0)
        Assert.AreEqual(TestInfo.LineFrequency, actualFrequency,
                                $"{NameOf(StatusSubsystem.LineFrequency)} is {actualFrequency}; expected {TestInfo.LineFrequency}")

        Dim expectedPowerLineCycles As Double = TestInfo.InitialPowerLineCycles
        Dim expectedIntegrationPeriod As TimeSpan = VI.StatusSubsystemBase.FromSecondsPrecise(expectedPowerLineCycles / TestInfo.LineFrequency)
        Dim actualIntegrationPeriod As TimeSpan = VI.StatusSubsystemBase.FromPowerLineCycles(expectedPowerLineCycles)
        Assert.AreEqual(expectedIntegrationPeriod, actualIntegrationPeriod,
                                $"Integration period for {expectedPowerLineCycles} power line cycles is {actualIntegrationPeriod}; expected {expectedIntegrationPeriod}")

        Dim actualPowerLineCycles As Double = VI.StatusSubsystemBase.ToPowerLineCycles(actualIntegrationPeriod)
        Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, TestInfo.LineFrequency / TimeSpan.TicksPerSecond,
                                $"Power line cycles is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

    End Sub

    ''' <summary> Check channel subsystem information. </summary>
    ''' <param name="device"> The device. </param>
    Private Shared Sub CheckChannelSubsystemInfo(ByVal device As Device)
        Assert.IsTrue(String.IsNullOrWhiteSpace(device.ChannelSubsystem.ClosedChannels), $"Scan list {device.ChannelSubsystem.ClosedChannels}; expected empty")
    End Sub

    <TestMethod()>
    Public Sub OpenCloseSessionTest()
        Using device As Device = Device.Create
            DeviceUnitTests.OpenSession(device)
            DeviceUnitTests.CloseSession(device)
        End Using
    End Sub

    '''<summary>
    '''A test for Open Session
    '''</summary>
    <TestMethod()>
    Public Sub OpenSessionTest()
        Using device As Device = Device.Create
            DeviceUnitTests.OpenSession(device)
            DeviceUnitTests.CheckSessionInfo(device)
            DeviceUnitTests.CheckStatusSubsystemInfo(device)
            DeviceUnitTests.CheckChannelSubsystemInfo(device)
            DeviceUnitTests.CloseSession(device)
        End Using
    End Sub

#End Region

End Class
