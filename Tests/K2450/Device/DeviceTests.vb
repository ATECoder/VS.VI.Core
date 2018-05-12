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
    Public Class DeviceTests

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
            Assert.IsTrue(TestInfo.Exists, $"{GetType(K2450.Tests.DeviceTestInfo)} settings not found")
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
        <TestMethod(), TestCategory("VI")>
        Public Sub VisaResourceTest()
            If Not DeviceTestInfo.Get.ResourcePinged Then Assert.Inconclusive($"{DeviceTestInfo.Get.ResourceTitle} not found")
            Dim resourcesFilter As String = VI.Pith.ResourceNameInfo.BuildMinimalResourcesFilter
            Dim resources As String()
            Using rm As VI.Pith.ResourcesManagerBase = VI.SessionFactory.Get.Factory.CreateResourcesManager()
                resources = rm.FindResources(resourcesFilter).ToArray
            End Using
            Assert.IsTrue(resources.Any, $"VISA Resources {If(resources.Any, "", "not")} found among {resourcesFilter}")
            Assert.IsTrue(resources.Contains(DeviceTestInfo.Get.ResourceName), $"Resource {DeviceTestInfo.Get.ResourceName} not found among {resourcesFilter}")
        End Sub

        ''' <summary> (Unit Test Method) tests device resource. </summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub DeviceResourceTest()
            If Not DeviceTestInfo.Get.ResourcePinged Then Assert.Inconclusive($"{DeviceTestInfo.Get.ResourceTitle} not found")
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                Assert.IsTrue(VI.Tsp2.K2450.Device.Find(DeviceTestInfo.Get.ResourceName, device.Session.ResourceNameInfo.ResourcesFilter),
                              $"VISA Resource {DeviceTestInfo.Get.ResourceName} not found among {device.Session.ResourceNameInfo.ResourcesFilter}")
            End Using
        End Sub

#End Region

#Region " DEVICE TESTS: OPEN, CLOSE, CHECK SUSBSYSTEMS "

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
        <TestMethod(), TestCategory("VI")>
        Public Sub OpenSessionTest()
            If Not DeviceTestInfo.Get.ResourcePinged Then Assert.Inconclusive($"{DeviceTestInfo.Get.ResourceTitle} not found")
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                Manager.OpenSession(device)
                Manager.CloseSession(device)
            End Using
        End Sub

        '''<summary>
        '''A test for Open Session and status
        '''</summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub OpenSessionCheckStatusTest()
            If Not DeviceTestInfo.Get.ResourcePinged Then Assert.Inconclusive($"{DeviceTestInfo.Get.ResourceTitle} not found")
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                Manager.OpenSession(device)
                Manager.CheckModel(device.StatusSubsystemBase)
                Manager.CheckDeviceErrors(device.StatusSubsystemBase)
                Manager.CheckTermination(device.Session)
                Manager.CheckLineFrequency(device.StatusSubsystem)
                Manager.CheckIntegrationPeriod(device.StatusSubsystem)
                Manager.CheckMeasureSubsystemInfo(device.MeasureSubsystem)
                Manager.CheckSourceSubsystemInfo(device.SourceSubsystem)
                Manager.ClearSessionCheckDeviceErrors(device)
                Manager.CloseSession(device)
            End Using
        End Sub

        ''' <summary> (Unit Test Method) tests open session read device errors. </summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub OpenSessionReadDeviceErrorsTest()
            If Not DeviceTestInfo.Get.ResourcePinged Then Assert.Inconclusive($"{DeviceTestInfo.Get.ResourceTitle} not found")
            Using device As VI.Tsp2.K2450.Device = VI.Tsp2.K2450.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                Manager.OpenSession(device)
                Manager.CheckModel(device.StatusSubsystemBase)
                Manager.CheckDeviceErrors(device.StatusSubsystemBase)
                Manager.CheckTermination(device.Session)
                Manager.CheckLineFrequency(device.StatusSubsystem)
                Manager.CheckIntegrationPeriod(device.StatusSubsystem)
                Manager.CheckMeasureSubsystemInfo(device.MeasureSubsystem)
                Manager.CheckSourceSubsystemInfo(device.SourceSubsystem)
                Manager.ClearSessionCheckDeviceErrors(device)
                Manager.CheckReadingDeviceErrors(device)
                Manager.CloseSession(device)
            End Using
        End Sub

#End Region

    End Class
End Namespace
