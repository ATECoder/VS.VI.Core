Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports isr.Core.Pith.StopwatchExtensions

''' <summary> K3700 Resource Control unit tests. </summary>
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
Public Class ResourceControlUnitTests

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

#Region " DEVICE OPEN TEST "

    ''' <summary> (Unit Test Method) tests selected resource name. </summary>
    <TestMethod()>
    Public Sub SelectedResourceNameTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean = True
        Using control As isr.VI.Instrument.ResourceControlBase = New isr.VI.Instrument.ResourceControlBase

            Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #1")

            control.ResourceTitle = TestInfo.ResourceTitle
            control.DisplayResourceNames()
            actualBoolean = control.HasResourceNames
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Has Resources {control.ResourceName}")

            Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #2")

            control.ResourceName = TestInfo.ResourceName
            actualBoolean = control.SelectedResourceExists
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Resource exits {control.ResourceName}")

            Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #3")
        End Using
    End Sub

    ''' <summary> Connects a device. </summary>
    ''' <param name="trialNumber"> The trial number. </param>
    ''' <param name="control">     The control. </param>
    Private Shared Sub ConnectDevice(ByVal trialNumber As Integer, ByVal control As isr.VI.Instrument.ResourceControlBase)
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Using device As Device = New Device()
            device.ResourceTitle = TestInfo.ResourceTitle
            control.DeviceBase = device
            control.ResourceName = TestInfo.ResourceName

            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            control.TryOpenSession(e)
            actualBoolean = e.Cancel
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Connect {trialNumber} cancel; {e.Details}")

            actualBoolean = device.IsDeviceOpen
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Connect {trialNumber} open {control.ResourceName}")

            ' check the MODEL
            Assert.AreEqual(TestInfo.ResourceModel, device.StatusSubsystem.VersionInfo.Model,
                            $"Version Info Model {control.ResourceName}", Globalization.CultureInfo.CurrentCulture)

            control.TryCloseSession(e)
            actualBoolean = e.Cancel
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Disconnect {trialNumber} cancel {e.Details}")

            actualBoolean = device.IsDeviceOpen
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Disconnect {trialNumber} open {control.ResourceName}")

        End Using
        Dim sw As Stopwatch = Stopwatch.StartNew
        sw.Wait(TimeSpan.FromMilliseconds(100))
    End Sub

    '''<summary>
    '''A test for Open connect and disconnect
    '''</summary>
    <TestMethod()>
    Public Sub OpenSessionTest()
        Using control As isr.VI.Instrument.ResourceControlBase = New isr.VI.Instrument.ResourceControlBase
            ResourceControlUnitTests.ConnectDevice(1, control)
        End Using
    End Sub

    '''<summary>
    '''A test for dual Open
    '''</summary>
    <TestMethod()>
    Public Sub OpenSessionTwiceTest()
        Dim sw As Stopwatch = Stopwatch.StartNew
        Using control As isr.VI.Instrument.ResourceControlBase = New isr.VI.Instrument.ResourceControlBase
            ResourceControlUnitTests.ConnectDevice(1, control)
        End Using
        Using control As isr.VI.Instrument.ResourceControlBase = New isr.VI.Instrument.ResourceControlBase
            ResourceControlUnitTests.ConnectDevice(2, control)
        End Using
        Using control As isr.VI.Instrument.ResourceControlBase = New isr.VI.Instrument.ResourceControlBase
            ResourceControlUnitTests.ConnectDevice(3, control)
            sw.Wait(TimeSpan.FromMilliseconds(100))
            ResourceControlUnitTests.ConnectDevice(4, control)
        End Using
    End Sub

#End Region

#Region " ASSIGNED DEVICE TESTS "

    ''' <summary> Assign device. </summary>
    ''' <param name="trialNumber"> The trial number. </param>
    ''' <param name="openFirst">   True to open first. </param>
    ''' <param name="control">     The control. </param>
    ''' <param name="device">      The device. </param>
    Private Shared Sub AssignDevice(ByVal trialNumber As Integer, ByVal openFirst As Boolean, ByVal control As K3700Control, ByVal device As Device)
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean

        device.ResourceTitle = TestInfo.ResourceTitle
        device.ResourceName = TestInfo.ResourceName
        Dim e As New isr.Core.Pith.CancelDetailsEventArgs

        If openFirst Then
            device.TryOpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, e)
            actualBoolean = e.Cancel
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Open Device {trialNumber} cancel; {e.Details}")
        End If

        control.AssignDevice(device)

        If Not device.IsDeviceOpen Then
            control.ResourceName = device.ResourceName
            control.TryOpenSession(e)
            actualBoolean = e.Cancel
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Connect {trialNumber} cancel; {e.Details}")
        End If

        actualBoolean = device.IsDeviceOpen
        expectedBoolean = True
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Connect {trialNumber} open {control.ResourceName}")

        ' check the MODEL
        Assert.AreEqual(TestInfo.ResourceModel, device.StatusSubsystem.VersionInfo.Model,
                            $"Version Info Model {control.ResourceName}", Globalization.CultureInfo.CurrentCulture)

        control.TryCloseSession(e)
        actualBoolean = e.Cancel
        expectedBoolean = False
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Disconnect {trialNumber} cancel {e.Details}")

        actualBoolean = device.IsDeviceOpen
        expectedBoolean = False
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Disconnect {trialNumber} open {control.ResourceName}")
        Dim sw As Stopwatch = Stopwatch.StartNew
        sw.Wait(TimeSpan.FromMilliseconds(100))

    End Sub

    <TestMethod()>
    Public Sub AssignDeviceTest()
        Using control As isr.VI.Tsp.K3700.K3700Control = New isr.VI.Tsp.K3700.K3700Control
            Using Device As Device = Device.Create
                ResourceControlUnitTests.AssignDevice(1, False, control, Device)
            End Using
        End Using
    End Sub

    <TestMethod()>
    Public Sub AssignOpenDeviceTest()
        Using control As isr.VI.Tsp.K3700.K3700Control = New isr.VI.Tsp.K3700.K3700Control
            Using Device As Device = Device.Create
                ResourceControlUnitTests.AssignDevice(1, True, control, Device)
            End Using
        End Using
    End Sub

#End Region

End Class
