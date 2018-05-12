Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Namespace K2450.Tests

    ''' <summary> K2450 Resistance Measuremetn unit tests. </summary>
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
    Public Class ResistanceTests

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
            Assert.IsTrue(TestInfo.Exists, $"{GetType(K2450.Tests.ResistanceTestInfo)} settings not found")
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

#Region " MEASURE SUBSYSTEM TEST "

        ''' <summary> Check measure subsystem information. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub ReadMeasureSubsystemInfo(ByVal device As VI.Tsp2.K2450.Device)

            Dim expectedPowerLineCycles As Double = DeviceTestInfo.Get.InitialPowerLineCycles
            Dim actualPowerLineCycles As Double = device.MeasureSubsystem.QueryPowerLineCycles.GetValueOrDefault(0)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, DeviceTestInfo.Get.LineFrequency / TimeSpan.TicksPerSecond,
                            $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

            Dim expectedBoolean As Boolean = DeviceTestInfo.Get.InitialAutoRangeEnabled
            Dim actualBoolean As Boolean = device.MeasureSubsystem.QueryAutoRangeEnabled.GetValueOrDefault(False)
            Assert.IsTrue(actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.AutoRangeEnabled)} is {actualBoolean }; expected {True}")

            expectedBoolean = DeviceTestInfo.Get.InitialAutoZeroEnabled
            actualBoolean = device.MeasureSubsystem.QueryAutoZeroEnabled.GetValueOrDefault(False)
            Assert.IsTrue(actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.AutoZeroEnabled)} is {actualBoolean }; expected {True}")

            expectedBoolean = DeviceTestInfo.Get.InitialFrontTerminalsSelected
            actualBoolean = device.MeasureSubsystem.QueryFrontTerminalsSelected.GetValueOrDefault(False)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.FrontTerminalsSelected)} is {actualBoolean }; expected {expectedBoolean }")

            expectedBoolean = DeviceTestInfo.Get.InitialRemoteSenseSelected
            actualBoolean = device.MeasureSubsystem.QueryRemoteSenseSelected.GetValueOrDefault(False)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.RemoteSenseSelected)} is {actualBoolean }; expected {expectedBoolean }")

            Dim senseFn As Tsp2.MeasureFunctionMode = device.MeasureSubsystem.QueryFunctionMode.GetValueOrDefault(MeasureFunctionMode.Resistance)
            Dim expectedFunctionMode As Tsp2.MeasureFunctionMode = DeviceTestInfo.Get.InitialMeasureFunctionMode
            Assert.AreEqual(expectedFunctionMode, senseFn, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.FunctionMode)} is {senseFn} ; expected {expectedFunctionMode}")

        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub ReadMeasureSubsystemTest()
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K2450.Tests.Manager.OpenSession(device)
                ResistanceTests.ReadMeasureSubsystemInfo(device)
                K2450.Tests.Manager.CloseSession(device)
            End Using
        End Sub

#End Region

#Region " SOURCE SUBSYSTEM TEST "

        ''' <summary> Check Source subsystem information. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub ReadSourceSubsystemInfo(ByVal device As VI.Tsp2.K2450.Device)

            Dim expectedBoolean As Boolean = DeviceTestInfo.Get.InitialAutoRangeEnabled
            Dim actualBoolean As Boolean = device.SourceSubsystem.QueryAutoRangeEnabled.GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.AutoRangeEnabled)} is {actualBoolean }; expected {True}")

            expectedBoolean = DeviceTestInfo.Get.InitialAutoDelayEnabled
            actualBoolean = device.SourceSubsystem.QueryAutoDelayEnabled.GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.AutoDelayEnabled)} is {actualBoolean }; expected {True}")

            expectedBoolean = False
            actualBoolean = device.SourceSubsystem.QueryOutputEnabled.GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.OutputEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            Dim functionMode As Tsp2.SourceFunctionMode = device.SourceSubsystem.QueryFunctionMode.GetValueOrDefault(SourceFunctionMode.None)
            Dim expectedFunctionMode As Tsp2.SourceFunctionMode = DeviceTestInfo.Get.InitialSourceFunctionMode
            Assert.AreEqual(expectedFunctionMode, functionMode, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.FunctionMode)} is {functionMode} ; expected {expectedFunctionMode}")

            expectedBoolean = False
            actualBoolean = device.SourceSubsystem.QueryLimitTripped.GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.OutputEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            Dim expectedDouble As Double = DeviceTestInfo.Get.InitialSourceLevel
            Dim actualDouble As Double = device.SourceSubsystem.QueryLevel.GetValueOrDefault(-1)
            Assert.AreEqual(expectedDouble, actualDouble, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.Level)} is {actualDouble}; expected {expectedDouble}")

            expectedDouble = DeviceTestInfo.Get.InitialSourceLimit
            actualDouble = device.SourceSubsystem.QueryLimit.GetValueOrDefault(-1)
            Assert.AreEqual(expectedDouble, actualDouble, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.Limit)} is {actualDouble}; expected {expectedDouble}")

        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub ReadSourceSubsystemTest()
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K2450.Tests.Manager.OpenSession(device)
                ResistanceTests.ReadSourceSubsystemInfo(device)
                K2450.Tests.Manager.CloseSession(device)
            End Using
        End Sub

#End Region

#Region " MEASURE RESISTANCE "

        ''' <summary> Source current measure resistance. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub SourceCurrentMeasureResistance(ByVal device As VI.Tsp2.K2450.Device)

            Dim expectedPowerLineCycles As Double = ResistanceTestInfo.Get.PowerLineCycles
            Dim actualPowerLineCycles As Double = device.MeasureSubsystem.ApplyPowerLineCycles(expectedPowerLineCycles).GetValueOrDefault(0)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, DeviceTestInfo.Get.LineFrequency / TimeSpan.TicksPerSecond,
                        $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

            Dim expectedBoolean As Boolean = ResistanceTestInfo.Get.AutoRangeEnabled
            Dim actualBoolean As Boolean = device.MeasureSubsystem.ApplyAutoRangeEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.AutoRangeEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            expectedBoolean = ResistanceTestInfo.Get.AutoZeroEnabled
            actualBoolean = device.MeasureSubsystem.ApplyAutoZeroEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.AutoZeroEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            expectedBoolean = ResistanceTestInfo.Get.FrontTerminalsSelected
            actualBoolean = device.MeasureSubsystem.ApplyFrontTerminalsSelected(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.FrontTerminalsSelected)} is {actualBoolean}; expected {expectedBoolean}")

            Dim expectedFunctionMode As Tsp2.SourceFunctionMode = ResistanceTestInfo.Get.SourceFunction
            Dim SourceFunction As SourceFunctionMode = device.SourceSubsystem.ApplyFunctionMode(expectedFunctionMode).GetValueOrDefault(SourceFunctionMode.None)
            Assert.AreEqual(expectedFunctionMode, SourceFunction, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.FunctionMode)} is {SourceFunction} ; expected {expectedFunctionMode}")

#If False Then
        ' USE SIMV(Ohm)
        Dim expectedDouble As Double = TestInfo.SourceLevel
        Dim actualDouble As Double = device.SourceSubsystem.ApplyLevel(expectedDouble).GetValueOrDefault(-expectedDouble)
        Assert.AreEqual(expectedDouble, actualDouble, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.Level)} is {actualDouble}; expected {expectedDouble}")
#End If
            Dim expectedMeasureFunctionMode As Tsp2.MeasureFunctionMode = ResistanceTestInfo.Get.SenseFunction
            Dim measureFunction As MeasureFunctionMode = device.MeasureSubsystem.ApplyFunctionMode(expectedMeasureFunctionMode).GetValueOrDefault(MeasureFunctionMode.Resistance)
            Assert.AreEqual(expectedMeasureFunctionMode, measureFunction, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.FunctionMode)} is {measureFunction} ; expected {expectedMeasureFunctionMode}")

            expectedBoolean = ResistanceTestInfo.Get.RemoteSenseSelected
            actualBoolean = device.MeasureSubsystem.ApplyRemoteSenseSelected(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.RemoteSenseSelected)} is {actualBoolean }; expected {expectedBoolean }")

            expectedBoolean = True
            actualBoolean = device.SourceSubsystem.ApplyOutputEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.OutputEnabled)} is {actualBoolean }; expected {expectedBoolean }")

            Dim resistance As Double = device.MeasureSubsystem.Measure.GetValueOrDefault(-1)
            Assert.AreEqual(ResistanceTestInfo.Get.ExpectedResistance, resistance, ResistanceTestInfo.Get.ExpectedResistanceEpsilon,
                  $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.MeasuredValue)} is {resistance}; expected {ResistanceTestInfo.Get.ExpectedResistance} within {ResistanceTestInfo.Get.ExpectedResistanceEpsilon}")

            expectedBoolean = False
            actualBoolean = device.SourceSubsystem.ApplyOutputEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.OutputEnabled)} is {actualBoolean }; expected {expectedBoolean }")

#If False Then
        SIMV(Ohms)
smu.measure.func = smu.FUNC_DC_VOLTAGE
smu.measure.autorange = smu.ON
smu.measure.unit = smu.UNIT_OHM
smu.measure.count = 5
smu.source.func = smu.FUNC_DC_CURRENT
smu.source.level = 5e-6
smu.source.vlimit.level = 10
smu.source.output = smu.ON
smu.measure.read(defbuffer1)
for i=1, defbuffer1.n do
 print(defbuffer1.relativetimestamps[i], defbuffer1[i])
end
smu.source.output=smu.OFF 


#End If

        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub SourceCurrentMeasureResistanceUnitTest()
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K2450.Tests.Manager.OpenSession(device)
                ResistanceTests.SourceCurrentMeasureResistance(device)
                K2450.Tests.Manager.CloseSession(device)
            End Using
        End Sub

#End Region

    End Class
End Namespace
