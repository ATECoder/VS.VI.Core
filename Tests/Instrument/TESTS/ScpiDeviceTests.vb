Imports isr.VI.Scpi.Instrument
'''<summary>
'''This is a test class for DeviceTest and is intended
'''to contain all DeviceTest Unit Tests
'''</summary>
<TestClass()>
Public Class ScpiDeviceTests


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region

    ''' <summary> Select resource name. </summary>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> . </returns>
    Friend Function SelectResourceName(ByVal interfaceType As HardwareInterfaceType) As String
        Select Case interfaceType
            Case HardwareInterfaceType.Gpib
                Return "GPIB0::24::INSTR"
            Case HardwareInterfaceType.Tcpip
                Return "TCPIP0::A-N5767A-K4381"
            Case HardwareInterfaceType.Usb
                Return "USB0::0x0957::0x0807::N5767A-US11K4381H::0::INSTR"
            Case Else
                Return "GPIB0::24::INSTR"
        End Select
    End Function

    '''<summary>
    '''A test for Open Session
    '''</summary>
    <TestMethod()>
    Public Sub OpenSessionTest()
        Dim expectedHardwareInterfaceType As HardwareInterfaceType = 0
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Dim expectedShort As Short = 0
        Dim usingInterfaceType As HardwareInterfaceType = HardwareInterfaceType.Gpib
        Using target As Device = New Device()
            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            actualBoolean = target.TryOpenSession(SelectResourceName(usingInterfaceType), "GPIB", e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Open Session; details: {e.Details}")
            target.Session.Clear()
            target.CloseSession()
        End Using
    End Sub

End Class
