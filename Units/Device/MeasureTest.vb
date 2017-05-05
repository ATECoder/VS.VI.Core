Imports isr.Core.Pith
'''<summary>
'''This is a test class for Measure and is intended
'''to contain all Measure Unit Tests
'''</summary>
<TestClass()>
Public Class MeasureTest

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
    <ClassInitialize()>
    Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
        OnInitialize()
    End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    Public Shared Sub MyClassCleanup()
        _MyLog = Nothing
    End Sub
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

    ''' <summary> Gets the log. </summary>
    ''' <value> The log. </value>
    Public Shared ReadOnly Property MyLog As MyLog = New MyLog()

    Protected Shared Sub OnInitialize()

        _MyLog = New MyLog()
        Dim listener As Microsoft.VisualBasic.Logging.FileLogTraceListener
        With _MyLog
            listener = .ReplaceDefaultTraceListener(UserLevel.AllUsers)
        End With

        ' set the log for the application
        With My.Application.Log
            .TraceSource.Listeners.Remove(DefaultFileLogTraceListener.DefaultFileLogWriterName)
            .TraceSource.Listeners.Add(listener)
            .TraceSource.Switch.Level = SourceLevels.Verbose
        End With
        _MyLog.ApplyTraceLevel(TraceEventType.Verbose)
    End Sub


    '''<summary>
    '''A test for Open session.
    '''</summary>
    <TestMethod()>
    Public Sub OpenSessionTest()
        Dim resourceName As String = "TCPIP0::192.168.1.134::inst0::INSTR"
        Dim target As SessionBase = isr.VI.SessionFactory.Get.Factory.CreateSession()
        target.OpenSession(resourceName, Threading.SynchronizationContext.Current)
        Assert.AreEqual(TimeSpan.FromSeconds(58), target.KeepAliveInterval)
        Assert.AreEqual(resourceName, target.ResourceName)
        Assert.AreEqual(True, target.IsDeviceOpen)
    End Sub

    Public Function OpenSession(ByVal resourceName As String) As SessionBase
        Dim target As SessionBase = isr.VI.SessionFactory.Get.Factory.CreateSession()
        target.OpenSession(resourceName, Threading.SynchronizationContext.Current)
        Assert.AreEqual(TimeSpan.FromSeconds(58), target.KeepAliveInterval)
        Assert.AreEqual(resourceName, target.ResourceName)
        Assert.AreEqual(True, target.IsDeviceOpen)
        Return target
    End Function

    '''<summary>
    '''A test for measure current.
    '''</summary>
    Public Function MeasureCurrent(ByVal session As SessionBase) As String
        session.Write("_G.status.reset()")
        Dim i As Integer = session.ReadStatusByte
        session.Write("print(smua.measure.i())")
        Dim value As String = session.ReadLineTrimEnd
        Return value
    End Function

    '''<summary>
    '''A test for measure current.
    '''</summary>
    <TestMethod()>
    Public Sub MeasureCurrentTest()
        Dim resourceName As String = "TCPIP0::192.168.1.134::inst0::INSTR"
        Using target As SessionBase = Me.OpenSession(resourceName)
            Dim value As String = Me.MeasureCurrent(target)
            Assert.AreEqual(False, String.IsNullOrWhiteSpace(value))
            My.Application.Log.WriteEntry(value)
        End Using
    End Sub

    <TestMethod()>
    Public Sub MeasureCurrentManyTest()
        Dim resourceName As String = "TCPIP0::192.168.1.134::inst0::INSTR"
        Using target As SessionBase = Me.OpenSession(resourceName)
            For i As Integer = 1 To 100
                Dim value As String = Me.MeasureCurrent(target)
                My.Application.Log.WriteEntry($"{i} {value}")
            Next
        End Using
    End Sub

    Sub LogIT(ByVal value As String)
        My.Application.Log.WriteEntry(value)
    End Sub

    ''' <summary> (Unit Test Method) tests start commands. </summary>
    <TestMethod()>
    Public Sub StartCommandsTest()
        Dim resourceName As String = "TCPIP0::192.168.1.134::inst0::INSTR"
        Using session As SessionBase = Me.OpenSession(resourceName)
            Dim command As String = ""
            Dim value As String = ""
            Dim i As Integer = 0

            command = "_G.status.request_enable=0" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.reset()" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(localnode.linefreq)" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(_G.ttm.coldResistance.Defaults.smuI)" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%7.4f',_G.ttm.coldResistance.Defaults.aperture))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.level))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.minCurrent))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.maxCurrent))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.limit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.minVoltage))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.maxVoltage))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.lowLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.highLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.minResistance))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.maxResistance))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.ir:currentSourceChannelSetter(_G.ttm.coldResistance.Defaults.smuI)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.ir:apertureSetter(_G.ttm.coldResistance.Defaults.aperture)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.ir:levelSetter(_G.ttm.coldResistance.Defaults.level)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.ir:limitSetter(_G.ttm.coldResistance.Defaults.limit)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.ir:lowLimitSetter(_G.ttm.coldResistance.Defaults.lowLimit)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.ir:highLimitSetter(_G.ttm.coldResistance.Defaults.highLimit)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(_G.ttm.ir.smuI==_G.smua)" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%7.4f',_G.ttm.ir.aperture))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.ir.level))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.ir.highLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.ir.lowLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.ir.limit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(_G.ttm.coldResistance.Defaults.smuI)" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%7.4f',_G.ttm.coldResistance.Defaults.aperture))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.level))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.minCurrent))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.maxCurrent))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.limit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.minVoltage))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.maxVoltage))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.lowLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.highLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.minResistance))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.maxResistance))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.fr:currentSourceChannelSetter(_G.ttm.coldResistance.Defaults.smuI)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.fr:apertureSetter(_G.ttm.coldResistance.Defaults.aperture)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.fr:levelSetter(_G.ttm.coldResistance.Defaults.level)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.fr:limitSetter(_G.ttm.coldResistance.Defaults.limit)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.fr:lowLimitSetter(_G.ttm.coldResistance.Defaults.lowLimit)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.fr:highLimitSetter(_G.ttm.coldResistance.Defaults.highLimit)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(_G.ttm.fr.smuI==_G.smua)" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%7.4f',_G.ttm.fr.aperture))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.fr.level))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.fr.highLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.fr.lowLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.fr.limit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.estimator.Defaults.thermalCoefficient))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.est:thermalCoefficientSetter(_G.ttm.estimator.Defaults.thermalCoefficient)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.estimator.Defaults.thermalCoefficient))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.est:thermalCoefficientSetter(_G.ttm.estimator.Defaults.thermalCoefficient)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")

            command = "_G.print(string.format('%9.6f',_G.ttm.est.thermalCoefficient))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.est.thermalCoefficient))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(_G.ttm.trace.Defaults.smuI)" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%7.4f',_G.ttm.trace.Defaults.aperture))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minAperture))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxAperture))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.level))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minCurrent))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxCurrent))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.limit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minVoltage))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxVoltage))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.highLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.lowLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxVoltageChange))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%d',_G.ttm.trace.Defaults.medianFilterSize))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.period))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minPeriod))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxPeriod))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%d',_G.ttm.trace.Defaults.points))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%d',_G.ttm.trace.Defaults.minPoints))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%1f',_G.ttm.trace.Defaults.maxPoints))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:currentSourceChannelSetter(_G.ttm.trace.Defaults.smuI)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")


            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:apertureSetter(_G.ttm.trace.Defaults.aperture)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:levelSetter(_G.ttm.trace.Defaults.level)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:limitSetter(_G.ttm.trace.Defaults.limit)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:lowLimitSetter(_G.ttm.trace.Defaults.lowLimit)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:highLimitSetter(_G.ttm.trace.Defaults.highLimit)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:maxRateSetter()" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:maxVoltageChangeSetter(_G.ttm.trace.Defaults.level)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:latencySetter(_G.ttm.trace.Defaults.latency)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:medianFilterSizeSetter(_G.ttm.trace.Defaults.medianFilterSize)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.ttm.tr:pointsSetter(_G.ttm.trace.Defaults.points)" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")

            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(_G.ttm.tr.smuI==_G.smua)" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%7.4f',_G.ttm.tr.aperture))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.tr.level))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.tr.highLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.tr.lowLimit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.tr.limit))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%d',_G.ttm.tr.medianFilterSize))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.3f',_G.ttm.postTransientDelayGetter()))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.tr.period))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%d',_G.ttm.tr.points))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%9.6f',_G.ttm.tr.maxVoltageChange))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.status.reset()" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.errorqueue.clear() waitcomplete()" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "_G.status.reset() _G.status.standard.enable=253 _G.status.request_enable=32 _G.opc()" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*IDN? *WAI" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.errorqueue.clear() waitcomplete()"

            i = session.ReadStatusByte : Me.LogIT($"STB: {i}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "*IDN? *WAI" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.reset()" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(_G.ttm.coldResistance.Defaults.smuI)" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.print(string.format('%7.4f',_G.ttm.coldResistance.Defaults.aperture))" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")

            i = session.ReadStatusByte : Me.LogIT($"STB: {i}")
            command = "_G.errorqueue.clear() waitcomplete()" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            i = session.ReadStatusByte : Me.LogIT($"STB: {i}")
            command = "_G.status.reset()" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*IDN? *WAI" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            session.Timeout = TimeSpan.FromMilliseconds(30000)
            command = "_G.errorqueue.clear() waitcomplete()" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            session.Timeout = TimeSpan.FromMilliseconds(2000)
            i = session.ReadStatusByte : Me.LogIT($"STB: {i}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            command = "_G.status.reset()" : session.Write(command) : value = "" : Me.LogIT($"{command} {value}")
            command = "*OPC?" : session.Write(command) : value = session.ReadLineTrimEnd : Me.LogIT($"{command} {value}")
            i = session.ReadStatusByte : Me.LogIT($"STB: {i}")
            command = "_G.print(display)" : session.Write(command) : session.ReadLineTrimEnd() : Me.LogIT($"{command} {value}")
        End Using
    End Sub

End Class
