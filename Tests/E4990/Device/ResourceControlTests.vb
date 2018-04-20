Imports System.Data.Common
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports isr.Core.Pith.StopwatchExtensions

''' <summary> K3700 Resource Control unit tests. </summary>
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
Public Class ResourceControlTests

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

#Region " DEVICE OPEN TEST "

    ''' <summary> (Unit Test Method) tests selected resource name. </summary>
    <TestMethod(), TestCategory("VI")>
    Public Sub SelectedResourceNameTest()
        Using device As Device = Device.Create
            device.AddListener(TestInfo.TraceMessagesQueueListener)
            Using control As VI.Instrument.ResourceControlBase = isr.VI.Instrument.ResourceControlBase.Create(device, False)
                TestInfo.CheckSelectedResourceName(control)
            End Using
        End Using
    End Sub

    ''' <summary> (Unit Test Method) tests control talker. </summary>
    <TestMethod()>
    Public Sub ControlTalkerTest()
        Using device As Device = Device.Create
            device.AddListener(TestInfo.TraceMessagesQueueListener)
            Using control As VI.Instrument.ResourceControlBase = isr.VI.Instrument.ResourceControlBase.Create(device, False)
                Dim payload As String = "Control message"
                Dim traceEventId As Integer = 1
                control.Talker.Publish(TraceEventType.Warning, traceEventId, payload)
                Dim traceMessage As isr.Core.Pith.TraceMessage = Nothing
                traceMessage = TestInfo.TraceMessagesQueueListener.TryDequeue()
                If traceMessage Is Nothing Then Assert.Fail($"{payload} failed to trace")
                Assert.AreEqual(traceEventId, traceMessage.Id, $"{payload} trace event id mismatch")
            End Using
        End Using
    End Sub

    '''<summary>
    '''A test for Open connect and disconnect
    '''</summary>
    <TestMethod(), TestCategory("VI")>
    Public Sub OpenSessionTest()
        Using device As Device = Device.Create
            device.AddListener(TestInfo.TraceMessagesQueueListener)
            Using control As VI.Instrument.ResourceControlBase = isr.VI.Instrument.ResourceControlBase.Create(device, False)
                TestInfo.OpenCloseSession(0, control)
            End Using
        End Using
    End Sub

    '''<summary>
    '''A test for dual Open
    '''</summary>
    <TestMethod(), TestCategory("VI")>
    Public Sub OpenSessionTwiceTest()
        Dim sw As Stopwatch = Stopwatch.StartNew
        Using device As Device = Device.Create
            device.AddListener(TestInfo.TraceMessagesQueueListener)
            Using control As VI.Instrument.ResourceControlBase = isr.VI.Instrument.ResourceControlBase.Create(device, False)
                TestInfo.OpenCloseSession(1, control)
            End Using
            Using control As VI.Instrument.ResourceControlBase = isr.VI.Instrument.ResourceControlBase.Create(device, False)
                TestInfo.OpenCloseSession(2, control)
            End Using
            Using control As VI.Instrument.ResourceControlBase = isr.VI.Instrument.ResourceControlBase.Create(device, False)
                TestInfo.OpenCloseSession(3, control)
                sw.Wait(TimeSpan.FromMilliseconds(100))
                TestInfo.OpenCloseSession(4, control)
            End Using
        End Using
    End Sub

#End Region

End Class
