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
Public Class DeviceConfigureUnitTests

    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Additional test attributes"


    Public Shared Property Device As Device

    ''' <summary> My class initialize. </summary>
    ''' <param name="testContext"> Gets or sets the test context which provides information about
    '''                            and functionality for the current test run. </param>
    ''' <remarks> Use ClassInitialize to run code before running the first test in the class </remarks>
    <ClassInitialize()> Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
        Dim e As New isr.Core.Pith.CancelDetailsEventArgs
        DeviceConfigureUnitTests.Device = New Device()
        With DeviceConfigureUnitTests.Device
            .ResourceTitle = TestInfo.ResourceTitle
            .TryOpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, e)
        End With
    End Sub

    ''' <summary> My class cleanup. </summary>
    ''' <remarks> Use ClassCleanup to run code after all tests in a class have run</remarks>
    <ClassCleanup()> Public Shared Sub MyClassCleanup()
        If DeviceConfigureUnitTests.Device IsNot Nothing Then
            If DeviceConfigureUnitTests.Device.IsDeviceOpen Then
                DeviceConfigureUnitTests.Device.CloseSession()
            End If
            DeviceConfigureUnitTests.Device.Dispose()
            DeviceConfigureUnitTests.Device = Nothing
        End If
    End Sub
    '
    ' Use TestInitialize to run code before running each test
    ' <TestInitialize()> Public Sub MyTestInitialize()
    ' End Sub
    '
    ' Use TestCleanup to run code after each test has run
    ' <TestCleanup()> Public Sub MyTestCleanup()
    ' End Sub
    '
#End Region

#Region " DEVICE CONFIGURATION TESTS "

    '''<summary>
    '''A test for Open Session
    '''</summary>
    <TestMethod()>
    Public Sub DeviceOpenTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean = DeviceConfigureUnitTests.Device.IsDeviceOpen
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Open {DeviceConfigureUnitTests.Device.ResourceName}")
    End Sub

#End Region

End Class
