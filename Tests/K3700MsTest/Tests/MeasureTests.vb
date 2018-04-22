Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports isr.VI.tsp.K3700

''' <summary> K3700 Device measurement unit tests. </summary>
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
Public Class MeasureTests

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
        Dim e As New isr.Core.Pith.ActionEventArgs
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

    <TestMethod()>
    Public Sub OpenCloseSessionTest()
        Using device As Device = Device.Create
            MeasureTests.OpenSession(device)
            MeasureTests.CloseSession(device)
        End Using
    End Sub

#End Region

#Region " CHANNEL SUBSYSTEM TEST "

    Private Shared Sub ReadChannelSubsystemInfo(ByVal device As Device)

        Dim expectedChannelList As String = ""
        Dim actualChannelList As String = device.ChannelSubsystem.QueryClosedChannels()
        Assert.AreEqual(expectedChannelList, actualChannelList,
                        $"Initial {NameOf(ChannelSubsystem)}.{NameOf(ChannelSubsystem.ClosedChannels)} is {actualChannelList}; expected {expectedChannelList}")

        expectedChannelList = TestInfo.OpenChannelList
        actualChannelList = device.ChannelSubsystem.ApplyClosedChannels(expectedChannelList, TimeSpan.FromSeconds(2))
        Assert.AreEqual(expectedChannelList, actualChannelList,
                        $"{NameOf(ChannelSubsystem)}.{NameOf(ChannelSubsystem.ClosedChannels)} is {actualChannelList}; expected {expectedChannelList}")

        expectedChannelList = ""
        actualChannelList = device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
        Assert.AreEqual(expectedChannelList, actualChannelList,
                        $"Open All {NameOf(ChannelSubsystem)}.{NameOf(ChannelSubsystem.ClosedChannels)} is {actualChannelList}; expected {expectedChannelList}")


    End Sub

    <TestMethod()>
    Public Sub ReadChannelSubsystemTest()
        Using device As Device = isr.VI.Tsp.K3700.Device.Create
            MeasureTests.OpenSession(device)
            MeasureTests.ReadChannelSubsystemInfo(device)
            MeasureTests.CloseSession(device)
        End Using
    End Sub




#End Region

#Region " MEASURE SUBSYSTEM TEST "

    ''' <summary> Check measure subsystem information. </summary>
    ''' <param name="device"> The device. </param>
    Private Shared Sub ReadMultimeterSubsystemInfo(ByVal device As Device)

        Dim expectedPowerLineCycles As Double = TestInfo.InitialPowerLineCycles
        Dim actualPowerLineCycles As Double = device.MultimeterSubsystem.QueryPowerLineCycles.GetValueOrDefault(0)
        Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, TestInfo.LineFrequency / TimeSpan.TicksPerSecond,
                        $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

        Dim expectedBoolean As Boolean = TestInfo.InitialAutoRangeEnabled
        Dim actualBoolean As Boolean = device.MultimeterSubsystem.QueryAutoRangeEnabled.GetValueOrDefault(False)
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.AutoRangeEnabled)} is {actualBoolean }; expected {expectedBoolean}")

        expectedBoolean = TestInfo.InitialAutoZeroEnabled
        actualBoolean = device.MultimeterSubsystem.QueryAutoZeroEnabled.GetValueOrDefault(False)
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.AutoZeroEnabled)} is {actualBoolean }; expected {expectedBoolean }")

        Dim senseFn As MultimeterFunctionMode = device.MultimeterSubsystem.QueryFunctionMode.GetValueOrDefault(MultimeterFunctionMode.ResistanceTwoWire)
        Assert.AreEqual(TestInfo.InitialSenseFunction, senseFn,
                        $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.FunctionMode)} is {senseFn} ; expected {TestInfo.InitialSenseFunction}")

    End Sub

    <TestMethod()>
    Public Sub ReadMultimeterSubsystemTest()
        Using device As Device = isr.VI.Tsp.K3700.Device.Create
            MeasureTests.OpenSession(device)
            MeasureTests.ReadMultimeterSubsystemInfo(device)
            MeasureTests.CloseSession(device)
        End Using
    End Sub

#End Region

#Region " MEASURE RESISTANCE "

    Private Shared Sub PrepareMeasureResistance(ByVal device As Device)

        Dim expectedPowerLineCycles As Double = TestInfo.PowerLineCycles
        Dim actualPowerLineCycles As Double = device.MultimeterSubsystem.ApplyPowerLineCycles(expectedPowerLineCycles).GetValueOrDefault(0)
        Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, 60 / TimeSpan.TicksPerSecond,
                        $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

        Dim expectedBoolean As Boolean = TestInfo.AutoRangeEnabled
        Dim actualBoolean As Boolean = device.MultimeterSubsystem.ApplyAutoRangeEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
        Assert.IsTrue(actualBoolean, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.AutoRangeEnabled)} is {actualBoolean}; expected {expectedBoolean}")

        expectedBoolean = TestInfo.AutoZeroEnabled
        actualBoolean = device.MultimeterSubsystem.ApplyAutoZeroEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
        Assert.IsTrue(actualBoolean, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.AutoZeroEnabled)} is {actualBoolean}; expected {expectedBoolean}")

        Dim measureFunction As MultimeterFunctionMode = device.MultimeterSubsystem.ApplyFunctionMode(TestInfo.SenseFunction).GetValueOrDefault(MultimeterFunctionMode.ResistanceFourWire)
        Assert.AreEqual(TestInfo.SenseFunction, measureFunction, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.FunctionMode)} is {measureFunction} ; expected {TestInfo.SenseFunction}")

    End Sub

    Private Shared Sub MeasureResistance(ByVal trialNumber As Integer, ByVal device As Device,
                                         ByVal expectedValue As Double, ByVal epsilon As Double,
                                         ByVal channelList As String)

        Dim expectedChannelList As String = ""
        Dim actualChannelList As String = device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
        Assert.AreEqual(expectedChannelList, actualChannelList,
                        $"Open All {trialNumber} {NameOf(ChannelSubsystem)}.{NameOf(ChannelSubsystem.ClosedChannels)} is {actualChannelList}; expected {expectedChannelList}")

        expectedChannelList = channelList
        actualChannelList = device.ChannelSubsystem.ApplyClosedChannels(expectedChannelList, TimeSpan.FromSeconds(2))
        Assert.AreEqual(expectedChannelList, actualChannelList,
                        $"{NameOf(ChannelSubsystem)}.{NameOf(ChannelSubsystem.ClosedChannels)} is {actualChannelList}; expected {expectedChannelList}")

        Dim expectedResistance As Double = expectedValue
        Dim resistance As Double = device.MultimeterSubsystem.Measure.GetValueOrDefault(-1)
        Assert.AreEqual(expectedValue, resistance, epsilon,
                        $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.Reading)} {channelList} is {resistance}; expected {expectedResistance } within {epsilon}")

    End Sub

    <TestMethod()>
    Public Sub MeasureResistanceUnitTest()
        Using device As Device = Device.Create
            MeasureTests.OpenSession(device)
            MeasureTests.PrepareMeasureResistance(device)
            Dim trialNumber As Integer = 0
            trialNumber += 1 : MeasureResistance(trialNumber, device, 0, TestInfo.ExpectedShort32ResistanceEpsilon, TestInfo.ShortChannelList)
            trialNumber += 1 : MeasureResistance(trialNumber, device, TestInfo.ExpectedResistance, TestInfo.ExpectedResistanceEpsilon, TestInfo.ResistorChannelList)
            trialNumber += 1 : MeasureResistance(trialNumber, device, TestInfo.ExpectedOpen, 1, TestInfo.OpenChannelList)
            MeasureTests.CloseSession(device)
        End Using
    End Sub

#End Region

End Class
