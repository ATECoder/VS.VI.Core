Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Namespace K7500.Tests
    ''' <summary> K7510 Subsystems unit tests. </summary>
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
    Public Class K7510SubsystemsTests

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
            Assert.IsTrue(K7500.Tests.K7510ResourceInfo.Get.Exists, $"{GetType(K7500.Tests.K7510ResourceInfo)} settings not found")
            Assert.IsTrue(K7500.Tests.K7510SubsystemsInfo.Get.Exists, $"{GetType(K7500.Tests.K7510SubsystemsInfo)} settings not found")
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

#Region " STATUS SUSBSYSTEM "

        ''' <summary> Opens session check status. </summary>
        ''' <param name="readErrorEnabled"> True to enable, false to disable the read error. </param>
        Public Shared Sub OpenSessionCheckStatus(ByVal readErrorEnabled As Boolean)
            If Not K7510ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K7510ResourceInfo.Get.ResourceTitle} not found")
            Using device As VI.Tsp2.K7500.Device = VI.Tsp2.K7500.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K7510Manager.CheckSession(device.Session)
                K7510Manager.OpenSession(device)
                K7510Manager.CheckModel(device.StatusSubsystemBase)
                K7510Manager.CheckDeviceErrors(device.StatusSubsystemBase)
                K7510Manager.CheckTermination(device.Session)
                K7510Manager.CheckLineFrequency(device.StatusSubsystem)
                K7510Manager.CheckIntegrationPeriod(device.StatusSubsystem)
                K7510Manager.CheckMultimeterSubsystemInfo(device.MultimeterSubsystem)
                K7510Manager.CheckBufferSubsystemInfo(device.Buffer1Subsystem)
                K7510Manager.ClearSessionCheckDeviceErrors(device)
                If readErrorEnabled Then K7510Manager.CheckReadingDeviceErrors(device)
                K7510Manager.CloseSession(device)
            End Using
        End Sub

        '''<summary>
        '''A test for Open Session and status
        '''</summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub OpenSessionCheckStatusTest()
            K7510SubsystemsTests.OpenSessionCheckStatus(False)
        End Sub

        ''' <summary> (Unit Test Method) tests open session read device errors. </summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub OpenSessionReadDeviceErrorsTest()
            K7510SubsystemsTests.OpenSessionCheckStatus(True)
        End Sub

#End Region

#Region " SENSE SUBSYSTEM TEST "

        ''' <summary> Check measure subsystem information. </summary>
        ''' <param name="device"> The device. </param>
        Private Shared Sub ReadSenseSubsystemInfo(ByVal device As VI.Tsp2.K7500.Device)

            Dim expectedPowerLineCycles As Double = K7510SubsystemsInfo.Get.InitialPowerLineCycles
            Dim actualPowerLineCycles As Double = device.MultimeterSubsystem.QueryPowerLineCycles.GetValueOrDefault(0)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, K7510SubsystemsInfo.Get.LineFrequency / TimeSpan.TicksPerSecond,
                            $"{GetType(VI.SenseSubsystemBase)}.{NameOf(VI.SenseSubsystemBase.PowerLineCycles)} is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")

            Dim expectedBoolean As Boolean = K7510SubsystemsInfo.Get.InitialAutoRangeEnabled
            Dim actualBoolean As Boolean = device.MultimeterSubsystem.QueryAutoRangeEnabled.GetValueOrDefault(False)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{GetType(VI.SenseSubsystemBase)}.{NameOf(VI.SenseSubsystemBase.AutoRangeEnabled)} is {actualBoolean }; expected {expectedBoolean}")

            Dim senseFn As Tsp2.MultimeterFunctionMode = device.MultimeterSubsystem.QueryFunctionMode.GetValueOrDefault(VI.Tsp2.MultimeterFunctionMode.Capacitance)
            Dim expectedFunctionMode As VI.Tsp2.MultimeterFunctionMode = K7510SubsystemsInfo.Get.InitialSenseFunctionMode
            Assert.AreEqual(expectedFunctionMode, senseFn, $"{GetType(VI.SenseSubsystemBase)}.{NameOf(VI.Scpi.SenseSubsystemBase.FunctionMode)} is {senseFn} ; expected {expectedFunctionMode}")
        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub ReadSenseSubsystemTest()
            Using device As VI.Tsp2.K7500.Device = VI.Tsp2.K7500.Device.Create
                device.AddListener(TestInfo.TraceMessagesQueueListener)
                K7500.Tests.K7510Manager.OpenSession(device)
                K7510SubsystemsTests.ReadSenseSubsystemInfo(device)
                K7500.Tests.K7510Manager.CloseSession(device)
            End Using
        End Sub

#End Region

    End Class
End Namespace
