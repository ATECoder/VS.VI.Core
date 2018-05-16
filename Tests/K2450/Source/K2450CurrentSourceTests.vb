Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Namespace K2450.Tests

    ''' <summary> K2450 Current Source unit tests. </summary>
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
    Public Class K2450CurrentSourceTests

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
            Assert.IsTrue(TestInfo.Exists, $"{GetType(K2450.Tests.K2450CurrentSourceTestInfo)} settings not found")
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

#Region " SOURCE CURRENT MEASURE VOLTAGE "

        ''' <summary> Source current measure voltage. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub SourceCurrentMeasureVoltage(ByVal device As VI.Tsp2.K2450.Device)

            Dim expectedPowerLineCycles As Double = K2450CurrentSourceTestInfo.Get.PowerLineCycles
            Dim actualPowerLineCycles As Double = device.MeasureSubsystem.ApplyPowerLineCycles(expectedPowerLineCycles).GetValueOrDefault(0)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, device.StatusSubsystem.LineFrequency.Value / TimeSpan.TicksPerSecond,
                        $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

            Dim expectedBoolean As Boolean = K2450CurrentSourceTestInfo.Get.AutoRangeEnabled
            Dim actualBoolean As Boolean = device.MeasureSubsystem.ApplyAutoRangeEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.AutoRangeEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            expectedBoolean = K2450CurrentSourceTestInfo.Get.AutoZeroEnabled
            actualBoolean = device.MeasureSubsystem.ApplyAutoZeroEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.AutoZeroEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            expectedBoolean = K2450CurrentSourceTestInfo.Get.FrontTerminalsSelected
            actualBoolean = device.MeasureSubsystem.ApplyFrontTerminalsSelected(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.FrontTerminalsSelected)} is {actualBoolean}; expected {expectedBoolean}")

            Dim expectedFunctionMode As VI.Tsp2.SourceFunctionMode = K2450CurrentSourceTestInfo.Get.SourceFunction
            Dim SourceFunction As VI.Tsp2.SourceFunctionMode = device.SourceSubsystem.ApplyFunctionMode(expectedFunctionMode).GetValueOrDefault(VI.Tsp2.SourceFunctionMode.None)
            Assert.AreEqual(expectedFunctionMode, SourceFunction, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.FunctionMode)} is {SourceFunction} ; expected {expectedFunctionMode}")

            Dim expectedMeasureFunctionMode As VI.Tsp2.MeasureFunctionMode = K2450CurrentSourceTestInfo.Get.SenseFunction
            Dim measureFunction As VI.Tsp2.MeasureFunctionMode = device.MeasureSubsystem.ApplyFunctionMode(expectedMeasureFunctionMode).GetValueOrDefault(VI.Tsp2.MeasureFunctionMode.Resistance)
            Assert.AreEqual(expectedMeasureFunctionMode, measureFunction, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.FunctionMode)} is {measureFunction} ; expected {expectedMeasureFunctionMode}")

            expectedBoolean = K2450CurrentSourceTestInfo.Get.RemoteSenseSelected
            actualBoolean = device.MeasureSubsystem.ApplyRemoteSenseSelected(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.RemoteSenseSelected)} is {actualBoolean}; expected {expectedBoolean}")

            Dim expectedSourceLevel As Double = K2450CurrentSourceTestInfo.Get.SourceLevel
            Dim actualSourceLevel As Double = device.SourceSubsystem.ApplyLevel(expectedSourceLevel).GetValueOrDefault(0)
            Assert.AreEqual(expectedSourceLevel, actualSourceLevel, 0.000001,
                        $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.Level)} is {actualSourceLevel:G5}; expected {expectedSourceLevel:G5}")

            Dim expectedSourceLimit As Double = 2 * K2450CurrentSourceTestInfo.Get.SourceLevel * K2450CurrentSourceTestInfo.Get.LoadResistance
            Dim actualSourceLimit As Double = device.SourceSubsystem.ApplyLimit(expectedSourceLimit).GetValueOrDefault(0)
            Assert.AreEqual(expectedSourceLimit, actualSourceLimit, 0.000001,
                            $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.Limit)} is {actualSourceLimit:G5}; expected {expectedSourceLimit:G5}")

            expectedBoolean = K2450CurrentSourceTestInfo.Get.SourceReadBackEnabled
            actualBoolean = device.SourceSubsystem.ApplyReadBackEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.ReadBackEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            ' set the reading to read voltage
            device.MeasureSubsystem.Readings.Initialize(VI.ReadingTypes.Voltage)
            device.MeasureSubsystem.Readings.Reading.Unit = device.MeasureSubsystem.FunctionUnit

            ' turn on the output
            K2450Manager.ToggleOutput(device, True)

            For i As Integer = 1 To 2

                Dim measuredVoltage As Double = device.MeasureSubsystem.MeasureValue.GetValueOrDefault(-1)
                Dim expectedVoltage As Double = K2450CurrentSourceTestInfo.Get.SourceLevel * K2450CurrentSourceTestInfo.Get.LoadResistance
                Dim epsilon As Double = expectedVoltage * K2450CurrentSourceTestInfo.Get.MeasurementTolerance
                Assert.AreEqual(expectedVoltage, measuredVoltage, epsilon,
                            $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.MeasuredValue)} is {measuredVoltage}; expected {expectedVoltage} within {epsilon}")

                If K2450CurrentSourceTestInfo.Get.SourceReadBackEnabled Then
                    ' read back the last source value
                    Dim measuredCurrent As Double = device.SourceSubsystem.ParseReadBackBufferAmount()
                    Dim expectedCurrent As Double = K2450CurrentSourceTestInfo.Get.SourceLevel
                    epsilon = expectedCurrent * K2450CurrentSourceTestInfo.Get.MeasurementTolerance
                    Assert.AreEqual(expectedCurrent, measuredCurrent, epsilon,
                            $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.ReadBackAmount)} is {measuredCurrent}; expected {expectedCurrent} within {epsilon}")
                End If

            Next

            ' turn off the output
            K2450Manager.ToggleOutput(device, False)

        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub SourceCurrentMeasureResistanceUnitTest()
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K2450.Tests.K2450Manager.OpenSession(device)
                Try
                    K2450CurrentSourceTests.SourceCurrentMeasureVoltage(device)
                Catch
                    Throw
                Finally
                    K2450.Tests.K2450Manager.ToggleOutput(device, False)
                End Try
                K2450.Tests.K2450Manager.CloseSession(device)
            End Using
        End Sub

#End Region

    End Class
End Namespace
