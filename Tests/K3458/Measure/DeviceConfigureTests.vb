Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

''' <summary> K3458 Device Configuration unit tests. </summary>
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
Public Class DeviceConfigureTests

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
            DeviceConfigureTests.InitializeClass()
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
        DeviceConfigureTests.CleanupClass()
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

#Region " ADDITIONAL TEST ATTRIBUTES "

    Public Shared Property Device As Device

    ''' <summary> Initialize class. </summary>
    Private Shared Sub InitializeClass()
        Dim e As New isr.Core.Pith.ActionEventArgs
        DeviceConfigureTests.Device = New Device()
        With DeviceConfigureTests.Device
            .TryOpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, e)
        End With
    End Sub

    ''' <summary> Cleanup class. </summary>
    Private Shared Sub CleanupClass()
        If DeviceConfigureTests.Device IsNot Nothing Then
            If DeviceConfigureTests.Device.IsDeviceOpen Then
                DeviceConfigureTests.Device.CloseSession()
            End If
            DeviceConfigureTests.Device.Dispose()
            DeviceConfigureTests.Device = Nothing
        End If
    End Sub

#End Region

#Region " DEVICE CONFIGURATION TESTS "

    '''<summary>
    '''A test for Open Session
    '''</summary>
    <TestMethod()>
    Public Sub DeviceOpenTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean = DeviceConfigureTests.Device.IsDeviceOpen
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Open {TestInfo.ResourceName}")
    End Sub

    ''' <summary> (Unit Test Method) tests custom device configuration. </summary>
    <TestMethod()>
    Public Sub CustomDeviceConfigurationTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean = DeviceConfigureTests.Device.IsDeviceOpen
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Open {TestInfo.ResourceName}")

        ' ! AUTO ZERO ONCE RETURNS OFF
        Dim expectedAutoZeroMode As AutoZeroMode = CType(TestInfo.AutoZero, AutoZeroMode)
        Dim actualAutoZeroMode As AutoZeroMode? = Device.MeasureSubsystem.ApplyAutoZeroMode(expectedAutoZeroMode)
        Assert.AreEqual(True, actualAutoZeroMode.HasValue, $"Auto zero mode has value")
        Assert.AreEqual(expectedAutoZeroMode, actualAutoZeroMode.Value, $"Auto zero mode applied")

        Dim expectedSenseFunctionMode As SenseFunctionMode = CType(TestInfo.SenseFunction, SenseFunctionMode)
        Dim actualSenseFunctionMode As SenseFunctionMode? = Device.MeasureSubsystem.ApplySenseFunctionMode(expectedSenseFunctionMode)
        Assert.AreEqual(True, actualSenseFunctionMode.HasValue, $"Sense function mode has value")
        Assert.AreEqual(expectedSenseFunctionMode, actualSenseFunctionMode.Value, $"Sense function mode applied")

        Dim expectedDouble As Double = TestInfo.PowerLineCycles
        Dim actualDouble As Double? = Device.MeasureSubsystem.ApplyPowerLineCycles(expectedDouble)
        Assert.AreEqual(True, actualDouble.HasValue, $"Power line cycles has value")
        Assert.AreEqual(expectedDouble, actualDouble.Value, $"Power line cycles applied")

        Dim expectedMathMode As MathMode = CType(TestInfo.MathMode, MathMode)
        Dim actualMathMode As MathMode? = Device.MeasureSubsystem.ApplyMathMode(expectedMathMode)
        Assert.AreEqual(True, actualMathMode.HasValue, $"Math mode has value")
        Assert.AreEqual(expectedMathMode, actualMathMode.Value, $"Math mode applied")

        Dim expectedStoreMathRegister As StoreMathRegister = CType(TestInfo.StoreMathRegister, StoreMathRegister)
        expectedDouble = TestInfo.StoreMathValue
        Dim actualStoreMathRegister As StoreMathRegister? = Device.MeasureSubsystem.WriteStoreMath(expectedStoreMathRegister, expectedDouble)
        actualDouble = Device.MeasureSubsystem.StoreMathValue
        Assert.AreEqual(True, actualStoreMathRegister.HasValue, $"Store math register has value")
        Assert.AreEqual(expectedStoreMathRegister, actualStoreMathRegister.Value, $"Store Math Register applied")

        Assert.AreEqual(True, actualDouble.HasValue, $"Store math value has value")
        Assert.AreEqual(expectedDouble, actualDouble.Value, $"Store Math value applied")

    End Sub

#End Region

End Class
