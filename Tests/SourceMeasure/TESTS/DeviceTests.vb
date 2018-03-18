'''<summary>
'''This is a test class for DeviceTest and is intended
'''to contain all DeviceTest Unit Tests
'''</summary>
<TestClass()>
Public Class DeviceTests

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
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
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

    ''' <summary> A test for Open Session. </summary>
    <TestMethod()>
    Public Sub OpenSessionTest()
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Dim usingInterfaceType As HardwareInterfaceType = HardwareInterfaceType.Gpib
        Using target As SourceMeasure.Device = New SourceMeasure.Device()
            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            actualBoolean = target.TryOpenSession(SelectResourceName(usingInterfaceType), "Source Measure", e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Open Session; {e.Details}")
            target.Session.Clear()
            target.CloseSession()
        End Using
    End Sub

    ''' <summary> A test for ToggleOutput On/Off. </summary>
    <TestMethod()>
    Public Sub ToggleOutputOnOffTest()
        Dim expectedBoolean As Boolean = True
        Dim expectedString As String = ""
        Dim actualBoolean As Boolean
        Dim actualString As String = ""
        Using target As SourceMeasure.Device = New SourceMeasure.Device()
            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            actualBoolean = target.TryOpenSession(SelectResourceName(HardwareInterfaceType.Gpib), "Source Measure", e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Open Session; {e.Details}")
            ' do a device clear and reset.
            target.ResetClearInit()
            actualBoolean = True

            ' output should be off after a device clear.
            expectedBoolean = False
            actualBoolean = target.OutputSubsystem.QueryOutputOnState.GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean,
                            "Output {0:'ON';'ON';'OFF'};", CInt(expectedBoolean))

            ' turn it on
            expectedBoolean = True
            actualBoolean = target.OutputSubsystem.ApplyOutputOnState(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean,
                            "Output {0:'ON';'ON';'OFF'};", CInt(expectedBoolean))

            ' turn it off
            expectedBoolean = False
            actualBoolean = target.OutputSubsystem.ApplyOutputOnState(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean,
                            "Output {0:'ON';'ON';'OFF'};", CInt(expectedBoolean))

            expectedString = "no error"
            actualString = target.StatusSubsystem.QueryLastError.ErrorMessage
            Assert.AreEqual(expectedString, actualString, True, Globalization.CultureInfo.CurrentCulture)
        End Using

    End Sub

    ''' <summary> A test for OutputOn and Output Off. </summary>
    <TestMethod()>
    Public Sub OutputOnOffTest()
        Dim voltage As Double = 5.0!
        Dim resistance As Double = 9910
        Dim currentLimit As Double = 0.001!
        Dim voltageLimit As Double = voltage + 1.0!
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean
        Dim expectedString As String = ""
        Dim actualString As String = ""
        Dim expectedDouble As Double = 0
        Using target As SourceMeasure.Device = New SourceMeasure.Device()
            Dim e As New isr.Core.Pith.CancelDetailsEventArgs
            actualBoolean = target.TryOpenSession(SelectResourceName(HardwareInterfaceType.Gpib), "Source Measure", e)
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Open Session; {e.Details}")
            ' do a device clear
            target.Session.Clear()

            expectedBoolean = True
            target.ResetClearInit()
            actualBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, "Reset;")

            expectedString = "Keithley"
            actualString = target.StatusSubsystem.QueryIdentity
            Assert.IsFalse(String.IsNullOrWhiteSpace(actualString), "Identity is empty")

            actualString = actualString.Substring(0, Len(expectedString))
            Assert.AreEqual(expectedString, actualString, True, Globalization.CultureInfo.CurrentCulture)

            expectedBoolean = True
            target.OutputOn(voltage, currentLimit, voltageLimit)
            actualBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, "Output On;")

            actualBoolean = target.OutputSubsystem.QueryOutputOnState.GetValueOrDefault(False)
            Assert.AreEqual(expectedBoolean, actualBoolean,
                            "Output {0:'ON';'ON';'OFF'};", CInt(expectedBoolean))

            target.MeasureSubsystem.Read()
            expectedDouble = voltage
            Assert.AreEqual(expectedDouble, target.MeasureSubsystem.Readings.VoltageReading.Value.GetValueOrDefault(0), 0.1)

            expectedDouble = voltage / resistance
            Assert.AreEqual(expectedDouble, target.MeasureSubsystem.Readings.CurrentReading.Value.GetValueOrDefault(0), 0.1 / resistance)

            expectedDouble = resistance
            Assert.AreEqual(expectedDouble, target.MeasureSubsystem.Readings.ResistanceReading.Value.GetValueOrDefault(0), 0.1 / resistance)

            expectedBoolean = False
            actualBoolean = target.OutputSubsystem.ApplyOutputOnState(expectedBoolean).GetValueOrDefault(Not expectedBoolean)
            Assert.AreEqual(expectedBoolean, actualBoolean,
                            "Output {0:'ON';'ON';'OFF'};", CInt(expectedBoolean))

            expectedString = "no error"
            actualString = target.StatusSubsystem.QueryLastError.ErrorMessage
            Assert.AreEqual(expectedString, actualString, True, Globalization.CultureInfo.CurrentCulture)
        End Using
    End Sub

End Class
