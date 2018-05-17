Namespace K3700.Tests
    ''' <summary> K3700 Device with K3730 Matrix Resistance Measurements unit tests. </summary>
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
    Public Class K3730Tests

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
            Assert.IsTrue(K3700.Tests.K3730TestInfo.Get.Exists, $"{GetType(K3700.Tests.K3730TestInfo)} settings not found")
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
                        $"Initial {GetType(VI.ChannelSubsystemBase)}.{NameOf(VI.ChannelSubsystemBase.ClosedChannels)} is {actualChannelList}; expected {expectedChannelList}")

            expectedChannelList = K3730TestInfo.Get.ResistorChannelList
            actualChannelList = device.ChannelSubsystem.ApplyClosedChannels(expectedChannelList, TimeSpan.FromSeconds(2))
            Assert.AreEqual(expectedChannelList, actualChannelList,
                            $"{GetType(VI.ChannelSubsystemBase)}.{NameOf(VI.ChannelSubsystemBase.ClosedChannels)} is {actualChannelList}; expected {expectedChannelList}")

            expectedChannelList = ""
            actualChannelList = device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
            Assert.AreEqual(expectedChannelList, actualChannelList,
                            $"Open All {GetType(VI.ChannelSubsystemBase)}.{NameOf(VI.ChannelSubsystemBase.ClosedChannels)} is {actualChannelList}; expected {expectedChannelList}")

        End Sub

        ''' <summary> (Unit Test Method) tests read channel subsystem. </summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub ReadChannelSubsystemTest()
            Using device As VI.Tsp.K3700.Device = VI.Tsp.K3700.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K3700.Tests.K3700Manager.OpenSession(device)
                K3730Tests.ReadChannelSubsystemInfo(device)
                K3700.Tests.K3700Manager.CloseSession(device)
            End Using
        End Sub

#End Region

#Region " MEASURE RESISTANCE "

        ''' <summary> Prepare Resistance measurement. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub PrepareMeasureResistance(ByVal device As VI.Tsp.K3700.Device)

            Dim expectedPowerLineCycles As Double = K3730TestInfo.Get.PowerLineCycles
            Dim actualPowerLineCycles As Double = device.MultimeterSubsystem.ApplyPowerLineCycles(expectedPowerLineCycles).GetValueOrDefault(0)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, 60 / TimeSpan.TicksPerSecond,
                        $"{GetType(VI.MultimeterSubsystemBase)}.{NameOf(VI.MultimeterSubsystemBase.PowerLineCycles)} Is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

            Dim expectedBoolean As Boolean = K3730TestInfo.Get.AutoRangeEnabled
            Dim actualBoolean As Boolean = device.MultimeterSubsystem.ApplyAutoRangeEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.MultimeterSubsystemBase)}.{NameOf(VI.MultimeterSubsystemBase.AutoRangeEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            expectedBoolean = K3730TestInfo.Get.AutoZeroEnabled
            actualBoolean = device.MultimeterSubsystem.ApplyAutoZeroEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.MultimeterSubsystemBase)}.{NameOf(VI.MultimeterSubsystemBase.AutoZeroEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            Dim expectedFunction As VI.Tsp.MultimeterFunctionMode = K3730TestInfo.Get.SenseFunction
            Dim actualFunction As VI.Tsp.MultimeterFunctionMode = device.MultimeterSubsystem.ApplyFunctionMode(expectedFunction).GetValueOrDefault(VI.Tsp.MultimeterFunctionMode.ResistanceFourWire)
            Assert.AreEqual(expectedFunction, actualFunction, $"{GetType(VI.Tsp.MultimeterSubsystemBase)}.{NameOf(VI.Tsp.MultimeterSubsystemBase.FunctionMode)} is {actualFunction} ; expected {expectedFunction}")

        End Sub

        ''' <summary> Measure resistance. </summary>
        ''' <param name="trialNumber">   The trial number. </param>
        ''' <param name="device">        The device. </param>
        ''' <param name="expectedValue"> The expected value. </param>
        ''' <param name="epsilon">       The epsilon. </param>
        ''' <param name="channelList">   List of channels. </param>
        Private Shared Sub MeasureResistance(ByVal trialNumber As Integer, ByVal device As VI.Tsp.K3700.Device,
                                             ByVal expectedValue As Double, ByVal epsilon As Double,
                                             ByVal channelList As String)

            Dim expectedChannelList As String = ""
            Dim actualChannelList As String = device.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
            Assert.AreEqual(expectedChannelList, actualChannelList,
                            $"Open All {trialNumber} {GetType(VI.ChannelSubsystemBase)}.{NameOf(VI.ChannelSubsystemBase.ClosedChannels)} is {actualChannelList}; expected {expectedChannelList}")

            expectedChannelList = channelList
            actualChannelList = device.ChannelSubsystem.ApplyClosedChannels(expectedChannelList, TimeSpan.FromSeconds(2))
            Assert.AreEqual(expectedChannelList, actualChannelList,
                            $"{GetType(VI.ChannelSubsystemBase)}.{NameOf(VI.ChannelSubsystemBase.ClosedChannels)}='{actualChannelList}'; expected {expectedChannelList}")

            Dim expectedResistance As Double = expectedValue
            Dim resistance As Double = device.MultimeterSubsystem.Measure.GetValueOrDefault(-1)
            Assert.AreEqual(expectedValue, resistance, epsilon,
                            $"{GetType(VI.MultimeterSubsystemBase)}.{NameOf(VI.MultimeterSubsystemBase.MeasuredValue)} {channelList} is {resistance}; expected {expectedResistance } within {epsilon}")

        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub MeasureResistanceUnitTest()
            Using device As VI.Tsp.K3700.Device = VI.Tsp.K3700.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K3700.Tests.K3700Manager.OpenSession(device)
                K3730Tests.PrepareMeasureResistance(device)
                Dim trialNumber As Integer = 0
                trialNumber += 1 : K3730Tests.MeasureResistance(trialNumber, device, 0, K3730TestInfo.Get.ExpectedShort32ResistanceEpsilon,
                                                                     K3730TestInfo.Get.ShortChannelList)
                trialNumber += 1 : K3730Tests.MeasureResistance(trialNumber, device, K3730TestInfo.Get.ExpectedResistance,
                                                                     K3730TestInfo.Get.ExpectedResistanceEpsilon, K3730TestInfo.Get.ResistorChannelList)
                trialNumber += 1 : K3730Tests.MeasureResistance(trialNumber, device, K3730TestInfo.Get.ExpectedOpen,
                                                                     1, K3730TestInfo.Get.OpenChannelList)
                K3700.Tests.K3700Manager.CloseSession(device)
            End Using
        End Sub

#End Region

    End Class
End Namespace


