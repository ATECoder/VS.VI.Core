Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Namespace K2450.Tests
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
    Public Class K2450Tests

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
            Assert.IsTrue(TestInfo.Exists, $"{GetType(K2450.Tests.K2450TestInfo)} settings not found")
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

#Region " VISA RESOURCE TESTS "

        ''' <summary> (Unit Test Method) tests visa resource. </summary>
        ''' <remarks> Finds the resource using the session factory resources manager. </remarks>
        <TestMethod(), TestCategory("VI")>
        Public Sub VisaResourceTest()
            If Not K2450TestInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K2450TestInfo.Get.ResourceTitle} not found")
            Dim resourcesFilter As String = VI.Pith.ResourceNameInfo.BuildMinimalResourcesFilter
            Dim resources As String()
            Using rm As VI.Pith.ResourcesManagerBase = VI.SessionFactory.Get.Factory.CreateResourcesManager()
                resources = rm.FindResources(resourcesFilter).ToArray
            End Using
            Assert.IsTrue(resources.Any, $"VISA Resources {If(resources.Any, "", "not")} found among {resourcesFilter}")
            Assert.IsTrue(resources.Contains(K2450TestInfo.Get.ResourceName), $"Resource {K2450TestInfo.Get.ResourceName} not found among {resourcesFilter}")
        End Sub

        ''' <summary> (Unit Test Method) tests device resource. </summary>
        ''' <remarks> Finds the resource using the device class. </remarks>
        <TestMethod(), TestCategory("VI")>
        Public Sub DeviceResourceTest()
            If Not K2450TestInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K2450TestInfo.Get.ResourceTitle} not found")
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                Assert.IsTrue(VI.Tsp2.K2450.Device.Find(K2450TestInfo.Get.ResourceName, device.Session.ResourceNameInfo.ResourcesFilter),
                              $"VISA Resource {K2450TestInfo.Get.ResourceName} not found among {device.Session.ResourceNameInfo.ResourcesFilter}")
            End Using
        End Sub

#End Region

#Region " DEVICE TESTS: OPEN, CLOSE, CHECK SUSBSYSTEMS "

        ''' <summary> (Unit Test Method) tests device talker. </summary>
        ''' <remarks> Checks if the device adds a trace message to a listener. </remarks>
        <TestMethod()>
        Public Sub DeviceTalkerTest()
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                Dim payload As String = "Device message"
                Dim traceEventId As Integer = 1
                device.Talker.Publish(TraceEventType.Warning, traceEventId, payload)

                Dim traceMessage As Core.Pith.TraceMessage = Nothing
                traceMessage = TestInfo.TraceMessagesQueueListener.TryDequeue()
                If traceMessage Is Nothing Then Assert.Fail($"{payload} failed to trace")
                Assert.AreEqual(traceEventId, traceMessage.Id, $"{payload} trace event id mismatch")

                traceEventId = 1
                payload = "Status subsystem message"
                device.Talker.Publish(TraceEventType.Warning, traceEventId, payload)
                traceMessage = TestInfo.TraceMessagesQueueListener.TryDequeue()
                If traceMessage Is Nothing Then Assert.Fail($"{payload} failed to trace")
                Assert.AreEqual(traceEventId, traceMessage.Id, $"{payload} trace event id mismatch")
            End Using
        End Sub

        ''' <summary> (Unit Test Method) tests open session. </summary>
        ''' <remarks> Tests opening and closing a VISA session. </remarks>
        <TestMethod(), TestCategory("VI")>
        Public Sub OpenSessionTest()
            If Not K2450TestInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K2450TestInfo.Get.ResourceTitle} not found")
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K2450Manager.OpenSession(device)
                K2450Manager.CloseSession(device)
            End Using
        End Sub

        '''<summary>
        '''A test for Open Session and status
        '''</summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub OpenSessionCheckStatusTest()
            If Not K2450TestInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K2450TestInfo.Get.ResourceTitle} not found")
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K2450Manager.OpenSession(device)
                K2450Manager.CheckModel(device.StatusSubsystemBase)
                K2450Manager.CheckDeviceErrors(device.StatusSubsystemBase)
                K2450Manager.CheckTermination(device.Session)
                K2450Manager.CheckLineFrequency(device.StatusSubsystem)
                K2450Manager.CheckIntegrationPeriod(device.StatusSubsystem)
                K2450Manager.CheckMeasureSubsystemInfo(device.MeasureSubsystem)
                K2450Manager.CheckSourceSubsystemInfo(device.SourceSubsystem)
                K2450Manager.ClearSessionCheckDeviceErrors(device)
                K2450Manager.CloseSession(device)
            End Using
        End Sub

        ''' <summary> (Unit Test Method) tests open session read device errors. </summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub OpenSessionReadDeviceErrorsTest()
            If Not K2450TestInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K2450TestInfo.Get.ResourceTitle} not found")
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K2450Manager.OpenSession(device)
                K2450Manager.CheckModel(device.StatusSubsystemBase)
                K2450Manager.CheckDeviceErrors(device.StatusSubsystemBase)
                K2450Manager.CheckTermination(device.Session)
                K2450Manager.CheckLineFrequency(device.StatusSubsystem)
                K2450Manager.CheckIntegrationPeriod(device.StatusSubsystem)
                K2450Manager.CheckMeasureSubsystemInfo(device.MeasureSubsystem)
                K2450Manager.CheckSourceSubsystemInfo(device.SourceSubsystem)
                K2450Manager.ClearSessionCheckDeviceErrors(device)
                K2450Manager.CheckReadingDeviceErrors(device)
                K2450Manager.CloseSession(device)
            End Using
        End Sub

#End Region

#Region " MEASURE SUBSYSTEM TEST "

        ''' <summary> Check measure subsystem information. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub ReadMeasureSubsystemInfo(ByVal device As VI.Tsp2.K2450.Device)

            Dim expectedPowerLineCycles As Double = K2450TestInfo.Get.InitialPowerLineCycles
            Dim actualPowerLineCycles As Double = device.MeasureSubsystem.QueryPowerLineCycles.GetValueOrDefault(0)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, K2450TestInfo.Get.LineFrequency / TimeSpan.TicksPerSecond,
                            $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

            Dim expectedBoolean As Boolean = K2450TestInfo.Get.InitialAutoRangeEnabled
            Dim actualBoolean As Boolean = device.MeasureSubsystem.QueryAutoRangeEnabled.GetValueOrDefault(False)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.AutoRangeEnabled)} is {actualBoolean }; expected {True}")

            expectedBoolean = K2450TestInfo.Get.InitialAutoZeroEnabled
            actualBoolean = device.MeasureSubsystem.QueryAutoZeroEnabled.GetValueOrDefault(False)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.AutoZeroEnabled)} is {actualBoolean }; expected {True}")

            expectedBoolean = K2450TestInfo.Get.InitialFrontTerminalsSelected
            actualBoolean = device.MeasureSubsystem.QueryFrontTerminalsSelected.GetValueOrDefault(False)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.FrontTerminalsSelected)} is {actualBoolean }; expected {expectedBoolean }")

            expectedBoolean = K2450TestInfo.Get.InitialRemoteSenseSelected
            actualBoolean = device.MeasureSubsystem.QueryRemoteSenseSelected.GetValueOrDefault(False)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.RemoteSenseSelected)} is {actualBoolean }; expected {expectedBoolean }")

            Dim senseFn As Tsp2.MeasureFunctionMode = device.MeasureSubsystem.QueryFunctionMode.GetValueOrDefault(VI.Tsp2.MeasureFunctionMode.Resistance)
            Dim expectedFunctionMode As Tsp2.MeasureFunctionMode = K2450TestInfo.Get.InitialMeasureFunctionMode
            Assert.AreEqual(expectedFunctionMode, senseFn, $"{GetType(VI.Tsp2.MeasureSubsystemBase)}.{NameOf(VI.Tsp2.MeasureSubsystemBase.FunctionMode)} is {senseFn} ; expected {expectedFunctionMode}")

        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub ReadMeasureSubsystemTest()
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K2450.Tests.K2450Manager.OpenSession(device)
                K2450Tests.ReadMeasureSubsystemInfo(device)
                K2450.Tests.K2450Manager.CloseSession(device)
            End Using
        End Sub

#End Region

#Region " SOURCE SUBSYSTEM TEST "

        ''' <summary> Check Source subsystem information. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub ReadSourceSubsystemInfo(ByVal device As VI.Tsp2.K2450.Device)

            Dim expectedBoolean As Boolean = K2450TestInfo.Get.InitialAutoRangeEnabled
            Dim actualBoolean As Boolean = device.SourceSubsystem.QueryAutoRangeEnabled.GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.AutoRangeEnabled)} is {actualBoolean }; expected {True}")

            expectedBoolean = K2450TestInfo.Get.InitialAutoDelayEnabled
            actualBoolean = device.SourceSubsystem.QueryAutoDelayEnabled.GetValueOrDefault(Not expectedBoolean)
            Assert.IsTrue(actualBoolean, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.AutoDelayEnabled)} is {actualBoolean }; expected {True}")

            expectedBoolean = False
            actualBoolean = device.SourceSubsystem.QueryOutputEnabled.GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.OutputEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            Dim functionMode As Tsp2.SourceFunctionMode = device.SourceSubsystem.QueryFunctionMode.GetValueOrDefault(VI.Tsp2.SourceFunctionMode.None)
            Dim expectedFunctionMode As Tsp2.SourceFunctionMode = K2450TestInfo.Get.InitialSourceFunctionMode
            Assert.AreEqual(expectedFunctionMode, functionMode, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.FunctionMode)} is {functionMode} ; expected {expectedFunctionMode}")

            expectedBoolean = False
            actualBoolean = device.SourceSubsystem.QueryLimitTripped.GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.OutputEnabled)} is {actualBoolean}; expected {expectedBoolean}")

            Dim expectedDouble As Double = K2450TestInfo.Get.InitialSourceLevel
            Dim actualDouble As Double = device.SourceSubsystem.QueryLevel.GetValueOrDefault(-1)
            Assert.AreEqual(expectedDouble, actualDouble, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.Level)} is {actualDouble}; expected {expectedDouble}")

            expectedDouble = K2450TestInfo.Get.InitialSourceLimit
            actualDouble = device.SourceSubsystem.QueryLimit.GetValueOrDefault(-1)
            Assert.AreEqual(expectedDouble, actualDouble, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.Limit)} is {actualDouble}; expected {expectedDouble}")

        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub ReadSourceSubsystemTest()
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K2450.Tests.K2450Manager.OpenSession(device)
                K2450Tests.ReadSourceSubsystemInfo(device)
                K2450.Tests.K2450Manager.CloseSession(device)
            End Using
        End Sub

#End Region

    End Class
End Namespace
