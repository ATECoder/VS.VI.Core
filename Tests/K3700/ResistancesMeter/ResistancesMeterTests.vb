Namespace K3700.Tests
    ''' <summary> Resistances meter unit tests. </summary>
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
    Public Class ResistancesMeterTests

#Region " CONSTRUCTION + CLEANUP "

        ''' <summary> My class initialize. </summary>
        ''' <param name="testContext"> Gets or sets the test context which provides information about
        '''                            and functionality for the current test run. </param>
        ''' <remarks>Use ClassInitialize to run code before running the first test in the class</remarks>
        <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
        <ClassInitialize()>
        Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
            Try
                ResistancesMeterTests.ResistancesMeterDevice = VI.Tsp.K3700.ResistancesMeterDevice.Create
                ResistancesMeterTests.ResistancesMeterDevice.AddListener(TestInfo.TraceMessagesQueueListener)
                ResistancesMeterTests.ResistancesMeter = New VI.Tsp.K3700.ResistancesMeterControl(ResistancesMeterTests.ResistancesMeterDevice, False)
                ResistancesMeterTests.ResistancesMeterDevice.Populate(New K3700.Tests.ResistorCollection)
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
            If ResistancesMeterTests.ResistancesMeter IsNot Nothing Then ResistancesMeterTests.ResistancesMeter.Dispose() : ResistancesMeterTests.ResistancesMeter = Nothing
            If ResistancesMeterTests.ResistancesMeterDevice IsNot Nothing Then ResistancesMeterTests.ResistancesMeterDevice.Dispose() : ResistancesMeterTests.ResistancesMeterDevice = Nothing
        End Sub

        ''' <summary> Initializes before each test runs. </summary>
        <TestInitialize()> Public Sub MyTestInitialize()
            Assert.IsTrue(TestInfo.Exists, $"{GetType(TestInfo)} settings not found")
            Assert.IsTrue(K3700ResourceInfo.Get.Exists, $"{GetType(K3700ResourceInfo)} settings not found")
            Assert.IsTrue(ResistancesMeterTestInfo.Get.Exists, $"{GetType(ResistancesMeterTestInfo)} settings not found")
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

        Private Shared ResistancesMeter As VI.Tsp.K3700.ResistancesMeterControl
        Private Shared ResistancesMeterDevice As VI.Tsp.K3700.ResistancesMeterDevice

        ''' <summary> Opens a session. </summary>
        ''' <param name="trialNumber"> The trial number. </param>
        ''' <param name="control">     The control. </param>
        Friend Shared Sub OpenSession(ByVal trialNumber As Integer, ByVal control As VI.Tsp.K3700.ResistancesMeterControl)
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
        Friend Shared Sub CloseSession(ByVal trialNumber As Integer, ByVal control As VI.Tsp.K3700.ResistancesMeterControl)
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
            K3700.Tests.K3700Manager.CheckSelectedResourceName(ResistancesMeterTests.ResistancesMeter)
        End Sub

#End Region

#Region " DEVICE OPEN TEST "

        '''<summary>
        '''A test for Open connect and disconnect
        '''</summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub OpenCloseResistancesMeterSessionTest()
            ResistancesMeterTests.OpenSession(1, ResistancesMeterTests.ResistancesMeter)
            ResistancesMeterTests.CloseSession(1, ResistancesMeterTests.ResistancesMeter)
        End Sub

#End Region

#Region " CONFIGURE TEST "

        ''' <summary> (Unit Test Method) tests Resistances meter configure. </summary>
        <TestMethod(), TestCategory("VI")>
        Public Sub ResistancesMeterConfigureTest()
            ResistancesMeterTests.OpenSession(1, ResistancesMeterTests.ResistancesMeter)
            Dim e As New Core.Pith.ActionEventArgs
            ResistancesMeterTests.ResistancesMeter.Device.TryConfigureMeter(ResistancesMeterTestInfo.Get.PowerLineCycles, e)
            TestInfo.AssertMessageQueue()
            Assert.AreEqual(False, e.Cancel, $"Configuring Resistances meter failed; {e.Details}")
            ResistancesMeterTests.CloseSession(1, ResistancesMeterTests.ResistancesMeter)
        End Sub

#End Region

#Region " MEASURE RESISTOR "

        ''' <summary> Resistances meter measure resistor. </summary>
        ''' <param name="control"> The control. </param>
        Private Shared Sub ResistancesMeterMeasureResistor(control As VI.Tsp.K3700.ResistancesMeterControl)
            Dim e As New Core.Pith.ActionEventArgs
            control.Device.TryConfigureMeter(ResistancesMeterTestInfo.Get.PowerLineCycles, e)
            TestInfo.AssertMessageQueue()
            Assert.AreEqual(False, e.Cancel, $"Configuring Resistances meter failed; {e.Details}")
            Dim resistor As VI.ChannelResistor = control.Device.Resistors(0)
            control.Device.TryMeasureResistance(resistor, e)
            TestInfo.AssertMessageQueue()
            Assert.AreEqual(False, e.Cancel, $"Measuring resistor {resistor.Title} failed; {e.Details}")
            Assert.AreEqual(ResistancesMeterTestInfo.Get.R1, resistor.Resistance, ResistancesMeterTestInfo.Get.ResistanceEpsilon,
                            $"Measuring resistor resistance expected {ResistancesMeterTestInfo.Get.R1:G5} actual {resistor.Resistance:G5}")
        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub ResistancesMeterMeasureResistorTest()
            ResistancesMeterTests.OpenSession(1, ResistancesMeterTests.ResistancesMeter)
            ResistancesMeterTests.ResistancesMeterMeasureResistor(ResistancesMeterTests.ResistancesMeter)
            ResistancesMeterTests.CloseSession(1, ResistancesMeterTests.ResistancesMeter)
        End Sub

#End Region

#Region " MEASURE Resistances "

        Private Shared Sub ResistancesMeterMeasure(control As VI.Tsp.K3700.ResistancesMeterControl)
            Dim e As New Core.Pith.ActionEventArgs
            control.Device.TryConfigureMeter(ResistancesMeterTestInfo.Get.PowerLineCycles, e)
            Assert.AreEqual(False, e.Cancel, $"Configuring resistances meter failed; {e.Details}")
            TestInfo.AssertMessageQueue()
            control.Device.TryMeasureResistors(e)
            Assert.AreEqual(False, e.Cancel, $"Measuring resistances failed; {e.Details}")
            TestInfo.AssertMessageQueue()
            Dim resistor As VI.ChannelResistor = control.Device.Resistors(0)
            Dim expectedResistance As Double = ResistancesMeterTestInfo.Get.R1
            Assert.AreEqual(expectedResistance, resistor.Resistance, ResistancesMeterTestInfo.Get.ResistanceEpsilon,
                            $"Measuring resistor {resistor.Title} resistance expected {expectedResistance:G5} actual {resistor.Resistance:G5}")
            resistor = control.Device.Resistors(1)
            expectedResistance = ResistancesMeterTestInfo.Get.R2
            Assert.AreEqual(expectedResistance, resistor.Resistance, ResistancesMeterTestInfo.Get.ResistanceEpsilon, $"Measuring resistor {resistor.Title} resistance expected {expectedResistance:G5} actual {resistor.Resistance:G5}")
            resistor = control.Device.Resistors(2)
            expectedResistance = ResistancesMeterTestInfo.Get.R3
            Assert.AreEqual(expectedResistance, resistor.Resistance, ResistancesMeterTestInfo.Get.ResistanceEpsilon, $"Measuring resistor {resistor.Title} resistance expected {expectedResistance:G5} actual {resistor.Resistance:G5}")
            resistor = control.Device.Resistors(3)
            expectedResistance = ResistancesMeterTestInfo.Get.R4
            Assert.AreEqual(expectedResistance, resistor.Resistance, ResistancesMeterTestInfo.Get.ResistanceEpsilon, $"Measuring resistor {resistor.Title} resistance expected {expectedResistance:G5} actual {resistor.Resistance:G5}")
        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub ResistancesMeterMeasureTest()
            ResistancesMeterTests.OpenSession(1, ResistancesMeterTests.ResistancesMeter)
            ResistancesMeterTests.ResistancesMeterMeasure(ResistancesMeterTests.ResistancesMeter)
            ResistancesMeterTests.CloseSession(1, ResistancesMeterTests.ResistancesMeter)
        End Sub

#End Region

#Region " ASSIGNED DEVICE TESTS "

        <TestMethod(), TestCategory("VI")>
        Public Sub AssignedDeviceMeasureResistorTest()
            If Not K3700ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K3700ResourceInfo.Get.ResourceTitle} not found")
            ResistancesMeterTests.ResistancesMeter.AssignDevice(ResistancesMeterTests.ResistancesMeterDevice, False)
            ResistancesMeterTests.OpenSession(1, ResistancesMeterTests.ResistancesMeter)
            ResistancesMeterTests.ResistancesMeterMeasureResistor(ResistancesMeterTests.ResistancesMeter)
            ResistancesMeterTests.CloseSession(1, ResistancesMeterTests.ResistancesMeter)
            ResistancesMeterTests.ResistancesMeter.RestoreDevice()
            TestInfo.AssertMessageQueue()
        End Sub

        <TestMethod(), TestCategory("VI")>
        Public Sub AssignedOpenDeviceMeasureResistorTest()
            K3700.Tests.K3700Manager.OpenSession(ResistancesMeterTests.ResistancesMeterDevice)
            ResistancesMeterTests.ResistancesMeter.AssignDevice(ResistancesMeterTests.ResistancesMeterDevice, False)
            ResistancesMeterTests.ResistancesMeterMeasureResistor(ResistancesMeterTests.ResistancesMeter)
            K3700.Tests.K3700Manager.CloseSession(ResistancesMeterTests.ResistancesMeterDevice)
            ResistancesMeterTests.ResistancesMeter.RestoreDevice()
            TestInfo.AssertMessageQueue()
        End Sub

#End Region

    End Class
End Namespace