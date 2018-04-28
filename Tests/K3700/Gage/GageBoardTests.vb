Imports isr.VI.Tsp
Imports isr.VI.Tsp.K3700
Namespace K3700.Tests
    ''' <summary> K3700 Device Gage Board measurements unit tests. </summary>
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
    Public Class GageBoardTests

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
            Assert.IsTrue(TestInfo.Exists, $"{GetType(GageBoardTestInfo)} settings not found")
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
        Private Shared Sub ReadChannelSubsystemInfo(ByVal device As VI.Tsp.K3700.Device)

            Dim expectedChannelList As String = ""
            Dim actualChannelList As String = device.ChannelSubsystem.QueryClosedChannels()
            Assert.AreEqual(expectedChannelList, actualChannelList,
                        $"Initial {NameOf(ChannelSubsystem)}.{NameOf(ChannelSubsystem.ClosedChannels)} is {actualChannelList}; expected {expectedChannelList}")

            expectedChannelList = GageBoardTestInfo.Get.FirstRtdChannelList
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
            Using device As VI.Tsp.K3700.Device = VI.Tsp.K3700.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K3700.Tests.Manager.OpenSession(device)
                GageBoardTests.ReadChannelSubsystemInfo(device)
                K3700.Tests.Manager.CloseSession(device)
            End Using
        End Sub

#End Region

#Region " GAGE BOARD SUBSYSTEM TEST "

        ''' <summary> Check GageBoard subsystem information. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub ReadMultimeterSubsystemInfo(ByVal device As VI.Tsp.K3700.Device)

            Dim expectedPowerLineCycles As Double = K3700.Tests.DeviceTestInfo.Get.InitialPowerLineCycles
            Dim actualPowerLineCycles As Double = device.MultimeterSubsystem.QueryPowerLineCycles.GetValueOrDefault(0)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, K3700.Tests.DeviceTestInfo.Get.LineFrequency / TimeSpan.TicksPerSecond,
                            $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

            Dim expectedBoolean As Boolean = K3700.Tests.DeviceTestInfo.Get.InitialAutoRangeEnabled
            Dim actualBoolean As Boolean = device.MultimeterSubsystem.QueryAutoRangeEnabled.GetValueOrDefault(False)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.AutoRangeEnabled)} Is {actualBoolean }; expected {expectedBoolean}")

            expectedBoolean = K3700.Tests.DeviceTestInfo.Get.InitialAutoZeroEnabled
            actualBoolean = device.MultimeterSubsystem.QueryAutoZeroEnabled.GetValueOrDefault(False)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.AutoZeroEnabled)} Is {actualBoolean }; expected {expectedBoolean }")

            Dim expectedFunction As MultimeterFunctionMode = K3700.Tests.DeviceTestInfo.Get.InitialSenseFunctionMode
            Dim senseFn As VI.Tsp.MultimeterFunctionMode = device.MultimeterSubsystem.QueryFunctionMode.GetValueOrDefault(MultimeterFunctionMode.ResistanceTwoWire)
            Assert.AreEqual(expectedFunction, senseFn,
                            $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.FunctionMode)} Is {senseFn} ; expected {expectedFunction}")

        End Sub

        ''' <summary> (Unit Test Method) tests read multimeter subsystem. </summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub ReadMultimeterSubsystemTest()
            Using device As VI.Tsp.K3700.Device = VI.Tsp.K3700.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K3700.Tests.Manager.OpenSession(device)
                GageBoardTests.ReadMultimeterSubsystemInfo(device)
                K3700.Tests.Manager.CloseSession(device)
            End Using
        End Sub

#End Region

#Region " GAGE BOARD RESISTANCE "

        ''' <summary> Prepare GageBoard resistance. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub PrepareGageBoardResistance(ByVal device As VI.Tsp.K3700.Device)

            Dim expectedPowerLineCycles As Double = GageBoardTestInfo.Get.PowerLineCycles
            Dim actualPowerLineCycles As Double = device.MultimeterSubsystem.ApplyPowerLineCycles(expectedPowerLineCycles).GetValueOrDefault(0)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, 60 / TimeSpan.TicksPerSecond,
                        $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.PowerLineCycles)} Is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

            Dim expectedBoolean As Boolean = GageBoardTestInfo.Get.AutoRangeEnabled
            Dim actualBoolean As Boolean = device.MultimeterSubsystem.ApplyAutoRangeEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.AutoRangeEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            expectedBoolean = GageBoardTestInfo.Get.AutoZeroEnabled
            actualBoolean = device.MultimeterSubsystem.ApplyAutoZeroEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.AutoZeroEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            Dim expectedFunction As MultimeterFunctionMode = GageBoardTestInfo.Get.SenseFunctionMode
            Dim actualFunction As MultimeterFunctionMode = device.MultimeterSubsystem.ApplyFunctionMode(expectedFunction).GetValueOrDefault(MultimeterFunctionMode.ResistanceFourWire)
            Assert.AreEqual(expectedFunction, actualFunction, $"{NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.FunctionMode)} is {actualFunction} ; expected {expectedFunction}")

            Dim expectedDetectorEnabled As Boolean = False
            device.MultimeterSubsystem.ApplyOpenDetectorEnabled(expectedDetectorEnabled)
            Assert.IsTrue(device.MultimeterSubsystem.OpenDetectorEnabled.HasValue, $"Detector enabled has value: {device.MultimeterSubsystem.OpenDetectorEnabled.HasValue}")
            Dim detectorEnabled As Boolean = device.MultimeterSubsystem.OpenDetectorEnabled.Value
            Assert.AreEqual(expectedDetectorEnabled, detectorEnabled, $"Detector enabled {device.MultimeterSubsystem.OpenDetectorEnabled.Value}")
        End Sub

        ''' <summary> GageBoard resistance. </summary>
        ''' <param name="trialNumber">   The trial number. </param>
        ''' <param name="device">        The device. </param>
        ''' <param name="expectedValue"> The expected value. </param>
        ''' <param name="epsilon">       The epsilon. </param>
        ''' <param name="channelList">   List of channels. </param>
        Private Shared Sub GageBoardResistance(ByVal trialNumber As Integer, ByVal device As VI.Tsp.K3700.Device,
                                             ByVal expectedValue As Double, ByVal epsilon As Double, ByVal channelList As String)

            Dim expectedChannelList As String = ""
            Dim actualChannelList As String = device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
            Assert.AreEqual(expectedChannelList, actualChannelList,
                            $"Open All {trialNumber} {NameOf(ChannelSubsystem)}.{NameOf(ChannelSubsystem.ClosedChannels)}='{actualChannelList}'; expected {expectedChannelList}")

            expectedChannelList = channelList
            actualChannelList = device.ChannelSubsystem.ApplyClosedChannels(expectedChannelList, TimeSpan.FromSeconds(2))
            Assert.AreEqual(expectedChannelList, actualChannelList,
                            $"{trialNumber} {NameOf(ChannelSubsystem)}.{NameOf(ChannelSubsystem.ClosedChannels)}='{actualChannelList}'; expected {expectedChannelList}")

            Dim expectedResistance As Double = expectedValue
            Dim resistance As Double = device.MultimeterSubsystem.Measure.GetValueOrDefault(-1)
            Assert.AreEqual(expectedValue, resistance, epsilon,
                            $"{trialNumber} {NameOf(ChannelSubsystem)}.{NameOf(ChannelSubsystem.ClosedChannels)}='{actualChannelList}' {NameOf(MultimeterSubsystem)}.{NameOf(MultimeterSubsystem.MeasuredValue)} {channelList} is {resistance}; expected {expectedResistance } within {epsilon}")

        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub GageBoardResistanceUnitTest()
            Using device As VI.Tsp.K3700.Device = VI.Tsp.K3700.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K3700.Tests.Manager.OpenSession(device)
                GageBoardTests.PrepareGageBoardResistance(device)
                Dim trialNumber As Integer = 0
                Dim expectedValue As Double = GageBoardTestInfo.Get.RtdAmbientResistance
                Dim epsilon As Double = GageBoardTestInfo.Get.RtdResistanceEpsilon
                Dim channelList As String = GageBoardTestInfo.Get.FirstRtdChannelList
                trialNumber += 1 : GageBoardTests.GageBoardResistance(trialNumber, device, expectedValue, epsilon, channelList)
                expectedValue = GageBoardTestInfo.Get.GaugeAmbientResistance
                epsilon = GageBoardTestInfo.Get.GaugeResistanceEpsilon
                channelList = GageBoardTestInfo.Get.FirstGaugeChannelList
                trialNumber += 1 : GageBoardTests.GageBoardResistance(trialNumber, device, expectedValue, epsilon, channelList)
                K3700.Tests.Manager.CloseSession(device)
            End Using
        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub TwoWireCircuitTest()
            Using device As VI.Tsp.K3700.Device = VI.Tsp.K3700.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K3700.Tests.Manager.OpenSession(device)
                GageBoardTests.PrepareGageBoardResistance(device)
                Dim trialNumber As Integer = 0
                Dim expectedValue As Double = GageBoardTestInfo.Get.RtdAmbientResistance
                Dim epsilon As Double = GageBoardTestInfo.Get.RtdResistanceEpsilon
                Dim channelList As String = GageBoardTestInfo.Get.TwoWireRtdSenseChannelList
                trialNumber += 1 : GageBoardTests.GageBoardResistance(trialNumber, device, expectedValue, epsilon, channelList)

                expectedValue = GageBoardTestInfo.Get.RtdAmbientResistance + GageBoardTestInfo.Get.GaugeAmbientResistance
                epsilon = GageBoardTestInfo.Get.RtdResistanceEpsilon + GageBoardTestInfo.Get.GaugeResistanceEpsilon
                channelList = GageBoardTestInfo.Get.TwoWireRtdSourceChannelList
                trialNumber += 1 : GageBoardTests.GageBoardResistance(trialNumber, device, expectedValue, epsilon, channelList)

                expectedValue = GageBoardTestInfo.Get.GaugeAmbientResistance
                epsilon = GageBoardTestInfo.Get.GaugeResistanceEpsilon
                channelList = GageBoardTestInfo.Get.TwoWireGageSenseChannelList
                trialNumber += 1 : GageBoardTests.GageBoardResistance(trialNumber, device, expectedValue, epsilon, channelList)

                expectedValue = 3 * GageBoardTestInfo.Get.GaugeAmbientResistance
                epsilon = 3 * GageBoardTestInfo.Get.GaugeResistanceEpsilon
                channelList = GageBoardTestInfo.Get.TwoWireGageSourceChannelList
                trialNumber += 1 : GageBoardTests.GageBoardResistance(trialNumber, device, expectedValue, epsilon, channelList)
                K3700.Tests.Manager.CloseSession(device)
            End Using
        End Sub

#End Region

    End Class
End Namespace
