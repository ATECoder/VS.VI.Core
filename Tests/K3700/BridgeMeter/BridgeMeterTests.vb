Namespace K3700.Tests
    ''' <summary> Bridge meter unit tests. </summary>
    ''' <license>
    ''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
    ''' Licensed under The MIT License.</para><para>
    ''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
    ''' </license>
    ''' <history date="01/15/2018" by="David" revision=""> Created. </history>
    <TestClass()>
    Public Class BridgeMeterTests

#Region " CONSTRUCTION + CLEANUP "

        ''' <summary> My class initialize. </summary>
        ''' <param name="testContext"> Gets or sets the test context which provides information about
        '''                            and functionality for the current test run. </param>
        ''' <remarks>Use ClassInitialize to run code before running the first test in the class</remarks>
        <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
        <ClassInitialize()>
        Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
            Try
                BridgeMeterTests.MeterDevice = VI.Tsp.K3700.BridgeMeterDevice.Create
                BridgeMeterTests.MeterDevice.AddListener(TestInfo.TraceMessagesQueueListener)
                BridgeMeterTests.BridgeMeter = New VI.Tsp.K3700.BridgeMeterControl(BridgeMeterTests.MeterDevice, False)
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
            If BridgeMeterTests.BridgeMeter IsNot Nothing Then BridgeMeterTests.BridgeMeter.Dispose() : BridgeMeterTests.BridgeMeter = Nothing
            If BridgeMeterTests.MeterDevice IsNot Nothing Then BridgeMeterTests.MeterDevice.Dispose() : BridgeMeterTests.MeterDevice = Nothing
        End Sub

        ''' <summary> Initializes before each test runs. </summary>
        <TestInitialize()> Public Sub MyTestInitialize()
            Assert.IsTrue(TestInfo.Exists, $"{GetType(TestInfo)} settings not found")
            Assert.IsTrue(TestInfo.Exists, $"{GetType(K3700ResourceInfo)} settings not found")
            Assert.IsTrue(TestInfo.Exists, $"{GetType(BridgeMeterTestInfo)} settings not found")
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

#Region " SHARED CONTROL AND DEVICE "

        Private Shared BridgeMeter As VI.Tsp.K3700.BridgeMeterControl
        Private Shared MeterDevice As VI.Tsp.K3700.BridgeMeterDevice

        ''' <summary> Opens a session. </summary>
        ''' <param name="trialNumber"> The trial number. </param>
        ''' <param name="control">     The control. </param>
        Private Shared Sub OpenSession(ByVal trialNumber As Integer, ByVal control As VI.Tsp.K3700.BridgeMeterControl)
            If Not K3700ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K3700ResourceInfo.Get.ResourceTitle} not found")
            Dim expectedBoolean As Boolean = True
            Dim actualBoolean As Boolean
            Dim e As New Core.Pith.ActionEventArgs
            control.Device.TryOpenSession(K3700ResourceInfo.Get.ResourceName, K3700ResourceInfo.Get.ResourceTitle, e)
            actualBoolean = e.Cancel
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} Connect canceled; {e.Details}")

            actualBoolean = control.IsConnected
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} Connect not connected {control.Device.ResourceNameCaption}")

            actualBoolean = control.Device.IsDeviceOpen
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} Open not open {control.Device.ResourceNameCaption}")

            ' check the MODEL
            Assert.AreEqual(K3700ResourceInfo.Get.ResourceModel, control.Device.StatusSubsystem.VersionInfo.Model,
                            $"Version Info Model {control.Device.ResourceNameCaption}", Globalization.CultureInfo.CurrentCulture)

        End Sub

        ''' <summary> Closes a session. </summary>
        ''' <param name="trialNumber"> The trial number. </param>
        ''' <param name="control">     The control. </param>
        Private Shared Sub CloseSession(ByVal trialNumber As Integer, ByVal control As VI.Tsp.K3700.BridgeMeterControl)
            If Not K3700ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K3700ResourceInfo.Get.ResourceTitle} not found")
            Dim expectedBoolean As Boolean = True
            Dim actualBoolean As Boolean
            control.Device.TryCloseSession()

            actualBoolean = control.IsConnected
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} Disconnect still connected {control.Device.ResourceNameCaption}")

            actualBoolean = control.Device.IsDeviceOpen
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} Close still open {control.Device.ResourceNameCaption}")
        End Sub

#End Region

#Region " SELECTED RESOURCE TEST "

        ''' <summary> (Unit Test Method) tests selected resource name. </summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub SelectedResourceNameTest()
            If Not K3700ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K3700ResourceInfo.Get.ResourceTitle} not found")
            K3700.Tests.K3700Manager.CheckSelectedResourceName(BridgeMeterTests.BridgeMeter)
        End Sub

#End Region

#Region " DEVICE OPEN TEST "

        '''<summary>
        '''A test for Open connect and disconnect
        '''</summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub OpenCloseBridgeMeterSessionTest()
            BridgeMeterTests.OpenSession(1, BridgeMeterTests.BridgeMeter)
            BridgeMeterTests.CloseSession(1, BridgeMeterTests.BridgeMeter)
        End Sub

#End Region

#Region " CONFIGURE TEST "

        ''' <summary> (Unit Test Method) tests bridge meter configure. </summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub BridgeMeterConfigureTest()
            BridgeMeterTests.OpenSession(1, BridgeMeterTests.BridgeMeter)
            Dim e As New Core.Pith.ActionEventArgs
            BridgeMeterTests.BridgeMeter.Device.TryConfigureMeter(BridgeMeterTestInfo.Get.PowerLineCycles, e)
            TestInfo.AssertMessageQueue()
            Assert.AreEqual(False, e.Cancel, $"Configuring bridge meter failed; {e.Details}")
            BridgeMeterTests.CloseSession(1, BridgeMeterTests.BridgeMeter)
        End Sub

#End Region

#Region " MEASURE RESISTOR "

        ''' <summary> Bridge meter measure resistor. </summary>
        ''' <param name="control"> The control. </param>
        Private Shared Sub BridgeMeterMeasureResistor(control As VI.Tsp.K3700.BridgeMeterControl)
            Dim e As New Core.Pith.ActionEventArgs
            control.Device.TryConfigureMeter(BridgeMeterTestInfo.Get.PowerLineCycles, e)
            Assert.AreEqual(False, e.Cancel, $"Configuring bridge meter failed; {e.Details}")
            TestInfo.AssertMessageQueue()
            Dim resistor As VI.ChannelResistor = control.Device.Bridge(0)
            control.Device.TryMeasureResistance(resistor, e)
            Assert.AreEqual(False, e.Cancel, $"Measuring resistor {resistor.Title} failed; {e.Details}")
            Assert.AreEqual(BridgeMeterTestInfo.Get.BridgeR1, resistor.Resistance, BridgeMeterTestInfo.Get.BridgeMeterEpsilon,
                            $"Measuring resistor resistance expected {BridgeMeterTestInfo.Get.BridgeR1:G5} actual {resistor.Resistance:G5}")
            TestInfo.AssertMessageQueue()
        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub BridgeMeterMeasureResistorTest()
            BridgeMeterTests.OpenSession(1, BridgeMeterTests.BridgeMeter)
            BridgeMeterTests.BridgeMeterMeasureResistor(BridgeMeterTests.BridgeMeter)
            BridgeMeterTests.CloseSession(1, BridgeMeterTests.BridgeMeter)
        End Sub

#End Region

#Region " MEASURE BRIDGE "

        Private Shared Sub BridgeMeterMeasure(control As VI.Tsp.K3700.BridgeMeterControl)
            Dim e As New Core.Pith.ActionEventArgs
            control.Device.TryConfigureMeter(BridgeMeterTestInfo.Get.PowerLineCycles, e)
            Assert.AreEqual(False, e.Cancel, $"Configuring bridge meter failed; {e.Details}")
            TestInfo.AssertMessageQueue()
            control.Device.TryMeasureBridge(e)
            Assert.AreEqual(False, e.Cancel, $"Measuring bridge {BridgeMeterTestInfo.Get.BridgeNumber} failed; {e.Details}")
            TestInfo.AssertMessageQueue()
            Dim resistor As VI.ChannelResistor = control.Device.Bridge(0)
            Dim expectedResistance As Double = BridgeMeterTestInfo.Get.BridgeR1
            Assert.AreEqual(expectedResistance, resistor.Resistance, BridgeMeterTestInfo.Get.BridgeMeterEpsilon,
                            $"Measuring resistor {resistor.Title} resistance expected {expectedResistance:G5} actual {resistor.Resistance:G5}")
            resistor = control.Device.Bridge(1)
            expectedResistance = BridgeMeterTestInfo.Get.BridgeR2
            Assert.AreEqual(expectedResistance, resistor.Resistance, BridgeMeterTestInfo.Get.BridgeMeterEpsilon, $"Measuring resistor {resistor.Title} resistance expected {expectedResistance:G5} actual {resistor.Resistance:G5}")
            resistor = control.Device.Bridge(2)
            expectedResistance = BridgeMeterTestInfo.Get.BridgeR3
            Assert.AreEqual(expectedResistance, resistor.Resistance, BridgeMeterTestInfo.Get.BridgeMeterEpsilon, $"Measuring resistor {resistor.Title} resistance expected {expectedResistance:G5} actual {resistor.Resistance:G5}")
            resistor = control.Device.Bridge(3)
            expectedResistance = BridgeMeterTestInfo.Get.BridgeR4
            Assert.AreEqual(expectedResistance, resistor.Resistance, BridgeMeterTestInfo.Get.BridgeMeterEpsilon, $"Measuring resistor {resistor.Title} resistance expected {expectedResistance:G5} actual {resistor.Resistance:G5}")
        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub BridgeMeterMeasureTest()
            BridgeMeterTests.OpenSession(1, BridgeMeterTests.BridgeMeter)
            BridgeMeterTests.BridgeMeterMeasure(BridgeMeterTests.BridgeMeter)
            BridgeMeterTests.CloseSession(1, BridgeMeterTests.BridgeMeter)
        End Sub

#End Region

#Region " ASSIGNED DEVICE TESTS "

        <TestMethod(), TestCategory("VI")>
        Public Sub AssignedDeviceMeasureResistorTest()
            If Not K3700ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K3700ResourceInfo.Get.ResourceTitle} not found")
            BridgeMeterTests.BridgeMeter.AssignDevice(BridgeMeterTests.MeterDevice, False)
            BridgeMeterTests.OpenSession(1, BridgeMeterTests.BridgeMeter)
            BridgeMeterTests.BridgeMeterMeasureResistor(BridgeMeterTests.BridgeMeter)
            BridgeMeterTests.CloseSession(1, BridgeMeterTests.BridgeMeter)
            BridgeMeterTests.BridgeMeter.RestoreDevice()
            TestInfo.AssertMessageQueue()
        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub AssignedOpenDeviceMeasureResistorTest()
            K3700.Tests.K3700Manager.OpenSession(BridgeMeterTests.MeterDevice)
            BridgeMeterTests.BridgeMeter.AssignDevice(BridgeMeterTests.MeterDevice, False)
            BridgeMeterTests.BridgeMeterMeasureResistor(BridgeMeterTests.BridgeMeter)
            K3700.Tests.K3700Manager.CloseSession(BridgeMeterTests.MeterDevice)
            BridgeMeterTests.BridgeMeter.RestoreDevice()
            TestInfo.AssertMessageQueue()
        End Sub

#End Region

    End Class
End Namespace