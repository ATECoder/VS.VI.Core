Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports isr.VI.tsp.K3700

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
Public Class BridgeMeterUnitTests

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
    End Sub

    ''' <summary> Cleans up after each test has run. </summary>
    <TestCleanup()> Public Sub MyTestCleanup()
    End Sub

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext

#End Region

#Region " DEVICE OPEN TEST "

    ''' <summary> (Unit Test Method) tests selected resource name. </summary>
    <TestMethod()>
    Public Sub SelectedResourceNameTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean = True
        Using control As isr.VI.tsp.K3700.BridgeMeterControl = New isr.VI.tsp.K3700.BridgeMeterControl

            Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #1")

            control.ResourceTitle = TestInfo.ResourceTitle
            control.DisplayResourceNames()
            actualBoolean = control.HasResourceNames
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Has Resources {control.ResourceName}")

            Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #2")

            control.ResourceName = TestInfo.ResourceName
            actualBoolean = control.SelectedResourceExists
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Resource exits {control.ResourceName}")

            Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #3")
        End Using
    End Sub

    ''' <summary> Opens a session. </summary>
    ''' <param name="trialNumber"> The trial number. </param>
    ''' <param name="control">     The control. </param>
    Private Shared Sub OpenSession(ByVal trialNumber As Integer, ByVal control As isr.VI.tsp.K3700.BridgeMeterControl)

        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        control.ResourceTitle = TestInfo.ResourceTitle
        control.ResourceName = TestInfo.ResourceName

        Dim e As New isr.Core.Pith.CancelDetailsEventArgs
        control.TryOpenSession(e)
        actualBoolean = e.Cancel
        expectedBoolean = False
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Connect {trialNumber} cancel; {e.Details}")

        actualBoolean = control.Device.IsDeviceOpen
        expectedBoolean = True
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Connect {trialNumber} open {control.ResourceName}")

        ' check the MODEL
        Assert.AreEqual(TestInfo.ResourceModel, control.Device.StatusSubsystem.VersionInfo.Model,
                            $"Version Info Model {control.ResourceName}", Globalization.CultureInfo.CurrentCulture)

    End Sub

    ''' <summary> Closes a session. </summary>
    ''' <param name="trialNumber"> The trial number. </param>
    ''' <param name="control">     The control. </param>
    Private Shared Sub CloseSession(ByVal trialNumber As Integer, ByVal control As isr.VI.Tsp.K3700.BridgeMeterControl)
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        control.ResourceTitle = TestInfo.ResourceTitle
        control.ResourceName = TestInfo.ResourceName

        Dim e As New isr.Core.Pith.CancelDetailsEventArgs
        control.TryCloseSession(e)
        actualBoolean = e.Cancel
        expectedBoolean = False
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Disconnect {trialNumber} cancel {e.Details}")

        actualBoolean = control.Device.IsDeviceOpen
        expectedBoolean = False
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Disconnect {trialNumber} open {control.ResourceName}")
    End Sub

    '''<summary>
    '''A test for Open connect and disconnect
    '''</summary>
    <TestMethod()>
    Public Sub OpenCloseSessionTest()
        Using control As isr.VI.tsp.K3700.BridgeMeterControl = New isr.VI.tsp.K3700.BridgeMeterControl
            BridgeMeterUnitTests.OpenSession(1, control)
            BridgeMeterUnitTests.CloseSession(1, control)
        End Using
    End Sub

#End Region

#Region " CONFIGURE TEST "

    ''' <summary> (Unit Test Method) tests bridge meter configure. </summary>
    <TestMethod()>
    Public Sub BridgeMeterConfigureTest()
        Using control As isr.VI.Tsp.K3700.BridgeMeterControl = New isr.VI.Tsp.K3700.BridgeMeterControl
            BridgeMeterUnitTests.OpenSession(1, control)
            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            control.TryConfigureMeter(e)
            Assert.AreEqual(False, e.Cancel, $"Configuring bridge meter failed; {e.Details}")
            BridgeMeterUnitTests.CloseSession(1, control)
        End Using
    End Sub

#End Region

#Region " MEASURE GAGE "

    <TestMethod()>
    Public Sub BridgeMeterMeasureGageTest()
        Using control As isr.VI.tsp.K3700.BridgeMeterControl = New isr.VI.tsp.K3700.BridgeMeterControl
            BridgeMeterUnitTests.OpenSession(1, control)
            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            control.TryConfigureMeter(e)
            Assert.AreEqual(False, e.Cancel, $"Configuring bridge meter failed; {e.Details}")
            Dim gage As Resistor = control.Bridge(0)
            control.TryMeasureResistance(gage, e)
            Assert.AreEqual(False, e.Cancel, $"Measuring gage {gage.Title} failed; {e.Details}")
            Assert.AreEqual(TestInfo.BridgeR1, gage.Resistance, TestInfo.BridgeMeterEpsilon, $"Measuring gage resistance expected {TestInfo.BridgeR1:G5} actual {gage.Resistance:G5}")
            BridgeMeterUnitTests.CloseSession(1, control)
        End Using
    End Sub

    <TestMethod()>
    Public Sub BridgeMeterMeasureTest()
        Using control As isr.VI.Tsp.K3700.BridgeMeterControl = New isr.VI.Tsp.K3700.BridgeMeterControl
            BridgeMeterUnitTests.OpenSession(1, control)
            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            control.TryConfigureMeter(e)
            Assert.AreEqual(False, e.Cancel, $"Configuring bridge meter failed; {e.Details}")
            control.TryMeasureBridge(e)
            Assert.AreEqual(False, e.Cancel, $"Measuring bridge {TestInfo.BridgeNumber} failed; {e.Details}")
            Dim gage As Resistor = control.Bridge(0)
            Dim expectedResistance As Double = TestInfo.BridgeR1
            Assert.AreEqual(expectedResistance, gage.Resistance, TestInfo.BridgeMeterEpsilon, $"Measuring gage {gage.Title} resistance expected {expectedResistance:G5} actual {gage.Resistance:G5}")
            gage = control.Bridge(1)
            expectedResistance = TestInfo.BridgeR2
            Assert.AreEqual(expectedResistance, gage.Resistance, TestInfo.BridgeMeterEpsilon, $"Measuring gage {gage.Title} resistance expected {expectedResistance:G5} actual {gage.Resistance:G5}")
            gage = control.Bridge(2)
            expectedResistance = TestInfo.BridgeR3
            Assert.AreEqual(expectedResistance, gage.Resistance, TestInfo.BridgeMeterEpsilon, $"Measuring gage {gage.Title} resistance expected {expectedResistance:G5} actual {gage.Resistance:G5}")
            gage = control.Bridge(3)
            expectedResistance = TestInfo.BridgeR4
            Assert.AreEqual(expectedResistance, gage.Resistance, TestInfo.BridgeMeterEpsilon, $"Measuring gage {gage.Title} resistance expected {expectedResistance:G5} actual {gage.Resistance:G5}")
        End Using
    End Sub

#End Region

End Class
