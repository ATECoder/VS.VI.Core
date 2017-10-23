Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

''' <summary> K3458 Resource Control unit tests. </summary>
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
Public Class ResourceControlUnitTests

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
    '
    ' You can use the following additional attributes as you write your tests:
    '
    ' Use ClassInitialize to run code before running the first test in the class
    <ClassInitialize()> Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    End Sub
    '
    ' Use ClassCleanup to run code after all tests in a class have run
    ' <ClassCleanup()> Public Shared Sub MyClassCleanup()
    ' End Sub
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

#Region " CONFIGURATION VERIFICATION "

    ''' <summary> (Unit Test Method) tests domain configuration exists. </summary>
    <TestMethod()>
    Public Sub DomainConfigurationExistsTest()
        Assert.IsTrue(TestInfo.Exists, "App.Config not found")
    End Sub

#End Region

#Region " DEVICE OPEN TEST "

    ''' <summary> (Unit Test Method) tests selected resource name. </summary>
    <TestMethod()>
    Public Sub SelectedResourceNameTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean = True
        Using control As isr.VI.Instrument.ResourceControlBase = New isr.VI.Instrument.ResourceControlBase
            control.ResourceTitle = TestInfo.ResourceTitle
            control.DisplayNames()
            actualBoolean = control.HasResourceNames
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Has Resources {control.ResourceName}")

            control.ResourceName = TestInfo.ResourceName
            actualBoolean = control.SelectedResourceExists
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Resource exits {control.ResourceName}")

        End Using
    End Sub

    '''<summary>
    '''A test for Open connect and disconnect
    '''</summary>
    <TestMethod()>
    Public Sub OpenSessionTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Using control As isr.VI.Instrument.ResourceControlBase = New isr.VI.Instrument.ResourceControlBase
            Using target As Device = New Device()
                target.ResourceTitle = TestInfo.ResourceTitle
                control.DeviceBase = target
                control.ResourceName = TestInfo.ResourceName

                Dim e As New System.ComponentModel.CancelEventArgs
                control.Connect(e)
                actualBoolean = e.Cancel
                expectedBoolean = False
                Assert.AreEqual(expectedBoolean, actualBoolean, $"Connect cancel {control.ResourceName}")

                actualBoolean = target.IsDeviceOpen
                expectedBoolean = True
                Assert.AreEqual(expectedBoolean, actualBoolean, $"Connect open {control.ResourceName}")

                ' check the MODEL
                Assert.AreEqual(TestInfo.ResourceModel, target.StatusSubsystem.VersionInfo.Model,
                                $"Version Info Model {control.ResourceName}", Globalization.CultureInfo.CurrentCulture)

                control.Disconnect(e)
                actualBoolean = e.Cancel
                expectedBoolean = False
                Assert.AreEqual(expectedBoolean, actualBoolean, $"Disconnect cancel {control.ResourceName}")

                actualBoolean = target.IsDeviceOpen
                expectedBoolean = False
                Assert.AreEqual(expectedBoolean, actualBoolean, $"Disconnect open {control.ResourceName}")

            End Using

        End Using

    End Sub

    '''<summary>
    '''A test for dual Open
    '''</summary>
    <TestMethod()>
    Public Sub OpenSessionTwiceTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Using control As isr.VI.Instrument.ResourceControlBase = New isr.VI.Instrument.ResourceControlBase
            Using target As Device = New Device()
                control.DeviceBase = target
                control.ResourceName = TestInfo.ResourceName

                Dim e As New System.ComponentModel.CancelEventArgs
                control.Connect(e)
                actualBoolean = target.IsDeviceOpen
                expectedBoolean = True
                Assert.AreEqual(expectedBoolean, actualBoolean, $"Connect open once {control.ResourceName}")

                ' check the MODEL
                Assert.AreEqual(TestInfo.ResourceModel, target.StatusSubsystem.VersionInfo.Model,
                                $"Version Info Model {control.ResourceName}", Globalization.CultureInfo.CurrentCulture)
            End Using
            Using target As Device = New Device()
                control.DeviceBase = target
                control.ResourceName = TestInfo.ResourceName

                Dim e As New System.ComponentModel.CancelEventArgs
                control.Connect(e)
                actualBoolean = target.IsDeviceOpen
                expectedBoolean = True
                Assert.AreEqual(expectedBoolean, actualBoolean, $"Connect open twice {control.ResourceName}")

                ' check the MODEL
                Assert.AreEqual(TestInfo.ResourceModel, target.StatusSubsystem.VersionInfo.Model,
                                $"Version Info Model {control.ResourceName}", Globalization.CultureInfo.CurrentCulture)
            End Using
        End Using
    End Sub

#End Region

End Class
