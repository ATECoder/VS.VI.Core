Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

''' <summary> K2450 Device unit tests. </summary>
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

#Region " MEASURE SUBSYSTEM TEST "

    ''' <summary> Check measure subsystem information. </summary>
    ''' <param name="device"> The device. </param>
    Private Shared Sub ReadMeasureSubsystemInfo(ByVal device As Device)

        Dim expectedPowerLineCycles As Double = TestInfo.InitialPowerLineCycles
        Dim actualPowerLineCycles As Double = device.MeasureSubsystem.QueryPowerLineCycles.GetValueOrDefault(0)
        Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, TestInfo.LineFrequency / TimeSpan.TicksPerSecond,
                        $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

        Dim expectedBoolean As Boolean = TestInfo.InitialAutoRangeEnabled
        Dim actualBoolean As Boolean = device.MeasureSubsystem.QueryAutoRangeEnabled.GetValueOrDefault(False)
        Assert.IsTrue(actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.AutoRangeEnabled)} is {actualBoolean }; expected {True}")

        expectedBoolean = TestInfo.InitialAutoZeroEnabled
        actualBoolean = device.MeasureSubsystem.QueryAutoZeroEnabled.GetValueOrDefault(False)
        Assert.IsTrue(actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.AutoZeroEnabled)} is {actualBoolean }; expected {True}")

        expectedBoolean = TestInfo.InitialFrontTerminalsSelected
        actualBoolean = device.MeasureSubsystem.QueryFrontTerminalsSelected.GetValueOrDefault(False)
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.FrontTerminalsSelected)} is {actualBoolean }; expected {expectedBoolean }")

        expectedBoolean = TestInfo.InitialRemoteSenseSelected
        actualBoolean = device.MeasureSubsystem.QueryRemoteSenseSelected.GetValueOrDefault(False)
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.RemoteSenseSelected)} is {actualBoolean }; expected {expectedBoolean }")

        Dim senseFn As MeasureFunctionMode = device.MeasureSubsystem.QueryFunctionMode.GetValueOrDefault(MeasureFunctionMode.Resistance)
        Assert.AreEqual(TestInfo.InitialSenseFunction, senseFn, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.FunctionMode)} is {senseFn} ; expected {TestInfo.InitialSenseFunction}")

    End Sub

    <TestMethod()>
    Public Sub ReadMeasureSubsystemTest()
        Using device As Device = Device.Create
            TestInfo.OpenSession(device)
            MeasureTests.ReadMeasureSubsystemInfo(device)
            TestInfo.CloseSession(device)
        End Using
    End Sub

#End Region

#Region " SOURCE SUBSYSTEM TEST "

    ''' <summary> Check Source subsystem information. </summary>
    ''' <param name="device"> The device. </param>
    Private Shared Sub ReadSourceSubsystemInfo(ByVal device As Device)

        Dim expectedBoolean As Boolean = TestInfo.InitialAutoRangeEnabled
        Dim actualBoolean As Boolean = device.SourceSubsystem.QueryAutoRangeEnabled.GetValueOrDefault(Not expectedBoolean)
        Assert.IsTrue(actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.AutoRangeEnabled)} is {actualBoolean }; expected {True}")

        expectedBoolean = TestInfo.InitialAutoDelayEnabled
        actualBoolean = device.SourceSubsystem.QueryAutoDelayEnabled.GetValueOrDefault(Not expectedBoolean)
        Assert.IsTrue(actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.AutoDelayEnabled)} is {actualBoolean }; expected {True}")

        expectedBoolean = False
        actualBoolean = device.SourceSubsystem.QueryOutputEnabled.GetValueOrDefault(Not expectedBoolean)
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.OutputEnabled)} is {actualBoolean}; expected {expectedBoolean}")

        Dim functionMode As SourceFunctionMode = device.SourceSubsystem.QueryFunctionMode.GetValueOrDefault(SourceFunctionMode.None)
        Assert.AreEqual(TestInfo.InitialSourceFunction, functionMode, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.FunctionMode)} is {functionMode} ; expected {TestInfo.InitialSourceFunction}")

        expectedBoolean = False
        actualBoolean = device.SourceSubsystem.QueryLimitTripped.GetValueOrDefault(Not expectedBoolean)
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.OutputEnabled)} is {actualBoolean}; expected {expectedBoolean}")

        Dim expectedDouble As Double = TestInfo.InitialSourceLevel
        Dim actualDouble As Double = device.SourceSubsystem.QueryLevel.GetValueOrDefault(-1)
        Assert.AreEqual(expectedDouble, actualDouble, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.Level)} is {actualDouble}; expected {expectedDouble}")

        expectedDouble = TestInfo.InitialSourceLimit
        actualDouble = device.SourceSubsystem.QueryLimit.GetValueOrDefault(-1)
        Assert.AreEqual(expectedDouble, actualDouble, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.Limit)} is {actualDouble}; expected {expectedDouble}")

    End Sub

    <TestMethod()>
    Public Sub ReadSourceSubsystemTest()
        Using device As Device = Device.Create
            TestInfo.OpenSession(device)
            MeasureTests.ReadSourceSubsystemInfo(device)
            TestInfo.CloseSession(device)
        End Using
    End Sub

#End Region

#Region " MEASURE RESISTANCE "

    ''' <summary> Source current measure resistance. </summary>
    ''' <param name="device"> The device. </param>
    Private Shared Sub SourceCurrentMeasureResistance(ByVal device As Device)

        Dim expectedPowerLineCycles As Double = TestInfo.PowerLineCycles
        Dim actualPowerLineCycles As Double = device.MeasureSubsystem.ApplyPowerLineCycles(expectedPowerLineCycles).GetValueOrDefault(0)
        Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, 60 / TimeSpan.TicksPerSecond,
                        $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

        Dim expectedBoolean As Boolean = TestInfo.AutoRangeEnabled
        Dim actualBoolean As Boolean = device.MeasureSubsystem.ApplyAutoRangeEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
        Assert.IsTrue(actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.AutoRangeEnabled)} is {actualBoolean}; expected {expectedBoolean}")

        expectedBoolean = TestInfo.AutoZeroEnabled
        actualBoolean = device.MeasureSubsystem.ApplyAutoZeroEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
        Assert.IsTrue(actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.AutoZeroEnabled)} is {actualBoolean}; expected {expectedBoolean}")

        expectedBoolean = TestInfo.FrontTerminalsSelected
        actualBoolean = device.MeasureSubsystem.ApplyFrontTerminalsSelected(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.FrontTerminalsSelected)} is {actualBoolean}; expected {expectedBoolean}")

        Dim SourceFunction As SourceFunctionMode = device.SourceSubsystem.ApplyFunctionMode(TestInfo.SourceFunction).GetValueOrDefault(SourceFunctionMode.None)
        Assert.AreEqual(TestInfo.SourceFunction, SourceFunction, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.FunctionMode)} is {SourceFunction} ; expected {TestInfo.SourceFunction}")

#If False Then
        ' USE SIMV(Ohm)
        Dim expectedDouble As Double = TestInfo.SourceLevel
        Dim actualDouble As Double = device.SourceSubsystem.ApplyLevel(expectedDouble).GetValueOrDefault(-expectedDouble)
        Assert.AreEqual(expectedDouble, actualDouble, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.Level)} is {actualDouble}; expected {expectedDouble}")
#End If

        Dim measureFunction As MeasureFunctionMode = device.MeasureSubsystem.ApplyFunctionMode(TestInfo.SenseFunction).GetValueOrDefault(MeasureFunctionMode.Resistance)
        Assert.AreEqual(TestInfo.SenseFunction, measureFunction, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.FunctionMode)} is {measureFunction} ; expected {TestInfo.SenseFunction}")

        expectedBoolean = TestInfo.RemoteSenseSelected
        actualBoolean = device.MeasureSubsystem.ApplyRemoteSenseSelected(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.RemoteSenseSelected)} is {actualBoolean }; expected {expectedBoolean }")

        expectedBoolean = True
        actualBoolean = device.SourceSubsystem.ApplyOutputEnabled(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{NameOf(SourceSubsystem)}.{NameOf(SourceSubsystem.OutputEnabled)} is {actualBoolean }; expected {expectedBoolean }")

        Dim resistance As Double = device.MeasureSubsystem.Measure.GetValueOrDefault(-1)
        Assert.AreEqual(TestInfo.ExpectedResistance, resistance, TestInfo.ExpectedResistanceEpsilon,
                  $"{NameOf(MeasureSubsystem)}.{NameOf(MeasureSubsystem.MeasuredValue)} is {resistance}; expected {TestInfo.ExpectedResistance} within {TestInfo.ExpectedResistanceEpsilon}")

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

    <TestMethod()>
    Public Sub SourceCurrentMeasureResistanceUnitTest()
        Using device As Device = Device.Create
            TestInfo.OpenSession(device)
            MeasureTests.SourceCurrentMeasureResistance(device)
            TestInfo.CloseSession(device)
        End Using
    End Sub

#End Region

End Class
