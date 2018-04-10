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

#Region " CHANNEL SUBSYSTEM TEST "

    ''' <summary> Reads channel subsystem information. </summary>
    ''' <param name="device"> The device. </param>
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

    ''' <summary> (Unit Test Method) tests read channel subsystem. </summary>
    <TestMethod(), TestCategory("VI")>
    Public Sub ReadChannelSubsystemTest()
        Using device As Device = isr.VI.Tsp.K3700.Device.Create
            device.AddListener(TestInfo.TraceMessagesQueueListener)
            TestInfo.OpenSession(device)
            MeasureTests.ReadChannelSubsystemInfo(device)
            TestInfo.CloseSession(device)
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

    ''' <summary> (Unit Test Method) tests read multimeter subsystem. </summary>
    <TestMethod(), TestCategory("VI")>
    Public Sub ReadMultimeterSubsystemTest()
        Using device As Device = isr.VI.Tsp.K3700.Device.Create
            device.AddListener(TestInfo.TraceMessagesQueueListener)
            TestInfo.OpenSession(device)
            MeasureTests.ReadMultimeterSubsystemInfo(device)
            TestInfo.CloseSession(device)
        End Using
    End Sub

#End Region

#Region " MEASURE RESISTANCE "

    ''' <summary> Prepare measure resistance. </summary>
    ''' <param name="device"> The device. </param>
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

        Dim measureFunction As MultimeterFunctionMode = device.MultimeterSubsystem.ApplyFunctionMode(TestInfo.FourWireResistanceSenseFunction).GetValueOrDefault(MultimeterFunctionMode.ResistanceFourWire)
        Assert.AreEqual(TestInfo.FourWireResistanceSenseFunction, measureFunction, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.FunctionMode)} is {measureFunction} ; expected {TestInfo.FourWireResistanceSenseFunction}")

    End Sub

    ''' <summary> Measure resistance. </summary>
    ''' <param name="trialNumber">   The trial number. </param>
    ''' <param name="device">        The device. </param>
    ''' <param name="expectedValue"> The expected value. </param>
    ''' <param name="epsilon">       The epsilon. </param>
    ''' <param name="channelList">   List of channels. </param>
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
                        $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.MeasuredValue)} {channelList} is {resistance}; expected {expectedResistance } within {epsilon}")

    End Sub

    <TestMethod(), TestCategory("VI")>
    Public Sub MeasureResistanceUnitTest()
        Using device As Device = Device.Create
            device.AddListener(TestInfo.TraceMessagesQueueListener)
            TestInfo.OpenSession(device)
            MeasureTests.PrepareMeasureResistance(device)
            Dim trialNumber As Integer = 0
            trialNumber += 1 : MeasureTests.MeasureResistance(trialNumber, device, 0, TestInfo.ExpectedShort32ResistanceEpsilon, TestInfo.ShortChannelList)
            trialNumber += 1 : MeasureTests.MeasureResistance(trialNumber, device, TestInfo.ExpectedResistance, TestInfo.ExpectedResistanceEpsilon, TestInfo.ResistorChannelList)
            trialNumber += 1 : MeasureTests.MeasureResistance(trialNumber, device, TestInfo.ExpectedOpen, 1, TestInfo.OpenChannelList)
            TestInfo.CloseSession(device)
        End Using
    End Sub

#End Region

End Class
