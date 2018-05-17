Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Namespace K2450.Tests

    ''' <summary> K2450 Resistance Measurement unit tests. </summary>
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
    Public Class K2450ResistanceTests

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
            Assert.IsTrue(K2450.Tests.K2450ResistanceTestInfo.Get.Exists, $"{GetType(K2450.Tests.K2450ResistanceTestInfo)} settings not found")
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

#Region " MEASURE RESISTANCE "

        ''' <summary> Source current measure resistance. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub SourceCurrentMeasureResistance(ByVal device As VI.Tsp2.K2450.Device)

            Dim expectedPowerLineCycles As Double = K2450ResistanceTestInfo.Get.PowerLineCycles
            Dim actualPowerLineCycles As Double = device.MeasureSubsystem.ApplyPowerLineCycles(expectedPowerLineCycles).GetValueOrDefault(0)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, K2450SubsystemsInfo.Get.LineFrequency / TimeSpan.TicksPerSecond,
                        $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

            Dim expectedBoolean As Boolean = K2450ResistanceTestInfo.Get.AutoRangeEnabled
            Dim actualBoolean As Boolean = device.MeasureSubsystem.ApplyAutoRangeEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.AutoRangeEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            expectedBoolean = K2450ResistanceTestInfo.Get.AutoZeroEnabled
            actualBoolean = device.MeasureSubsystem.ApplyAutoZeroEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.AutoZeroEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            expectedBoolean = K2450ResistanceTestInfo.Get.FrontTerminalsSelected
            actualBoolean = device.MeasureSubsystem.ApplyFrontTerminalsSelected(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.FrontTerminalsSelected)} is {actualBoolean}; expected {expectedBoolean}")

            Dim expectedFunctionMode As VI.Tsp2.SourceFunctionMode = K2450ResistanceTestInfo.Get.SourceFunction
            Dim SourceFunction As VI.Tsp2.SourceFunctionMode = device.SourceSubsystem.ApplyFunctionMode(expectedFunctionMode).GetValueOrDefault(VI.Tsp2.SourceFunctionMode.None)
            Assert.AreEqual(expectedFunctionMode, SourceFunction, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.FunctionMode)} is {SourceFunction} ; expected {expectedFunctionMode}")

            Dim expectedMeasureFunctionMode As VI.Tsp2.MeasureFunctionMode = K2450ResistanceTestInfo.Get.SenseFunction
            Dim measureFunction As VI.Tsp2.MeasureFunctionMode = device.MeasureSubsystem.ApplyFunctionMode(expectedMeasureFunctionMode).GetValueOrDefault(VI.Tsp2.MeasureFunctionMode.Resistance)
            Assert.AreEqual(expectedMeasureFunctionMode, measureFunction, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.FunctionMode)} is {measureFunction} ; expected {expectedMeasureFunctionMode}")

            expectedBoolean = K2450ResistanceTestInfo.Get.RemoteSenseSelected
            actualBoolean = device.MeasureSubsystem.ApplyRemoteSenseSelected(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.RemoteSenseSelected)} is {actualBoolean}; expected {expectedBoolean}")

            ' set the reading to read resistance
            device.MeasureSubsystem.Readings.Initialize(VI.ReadingTypes.Resistance)
            device.MeasureSubsystem.Readings.Reading.Unit = device.MeasureSubsystem.FunctionUnit

            ' turn on the output
            K2450Manager.ToggleOutput(device, True)

            Dim measuredResistance As Double = device.MeasureSubsystem.MeasureValue.GetValueOrDefault(-1)
            Dim expectedResistance As Double = K2450ResistanceTestInfo.Get.ExpectedResistance
            Dim epsilon As Double = expectedResistance * K2450ResistanceTestInfo.Get.ResistanceTolerance
            Assert.AreEqual(expectedResistance, measuredResistance, epsilon,
                            $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.MeasuredValue)} is {measuredResistance}; expected {expectedResistance} within {epsilon}")

            ' turn off the output
            K2450Manager.ToggleOutput(device, False)

        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub SourceCurrentMeasureResistanceUnitTest()
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K2450.Tests.K2450Manager.OpenSession(device)
                Try
                    K2450ResistanceTests.SourceCurrentMeasureResistance(device)
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
