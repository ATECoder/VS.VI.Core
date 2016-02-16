'''<summary>
'''This is a test class for Tegam.MeasureSubsystemTest and is intended
'''to contain all MeasureSubsystemTest Unit Tests
'''</summary>
<TestClass()>
Public Class TegamMeasureSubsystemTest

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


    '''<summary>
    '''A test for TryParse
    '''</summary>
    Public Sub TryParseTest(ByVal rangeMode As Tegam.RangeMode, ByVal expectedCurrent As String, ByVal expectedRange As String)
        Dim current As String = ""
        Dim range As String = ""
        Dim expected As Boolean = True
        Dim actual As Boolean
        actual = Tegam.MeasureSubsystem.TryParse(rangeMode, current, range)
        Assert.AreEqual(expected, actual)
        Assert.AreEqual(expectedCurrent, current)
        Assert.AreEqual(expectedRange, range)
    End Sub

    '''<summary>
    '''A test for TryParse
    '''</summary>
    <TestMethod()>
    Public Sub TryParseTest()
        Me.TryParseTest(Tegam.RangeMode.R10, "10 mA", "200 ohm")
    End Sub

    '''<summary>
    '''A test for TryParse
    '''</summary>
    Public Sub TryParseTest(ByVal rangeMode As Tegam.RangeMode, ByVal expectedCurrent As Double, ByVal expectedRange As Double)
        Dim current As Double
        Dim range As Double
        Dim expected As Boolean = True
        Dim actual As Boolean
        actual = Tegam.MeasureSubsystem.TryParse(rangeMode, current, range)
        Assert.AreEqual(expected, actual)
        Assert.AreEqual(expectedCurrent, current, 0.00001 * current)
        Assert.AreEqual(expectedRange, range, 0.00001 * range)
    End Sub

    '''<summary>
    '''A test for TryParse
    '''</summary>
    <TestMethod()>
    Public Sub TryParseTestNumericR10()
        Me.TryParseTest(Tegam.RangeMode.R10, 0.01, 200)
    End Sub

    '''<summary>
    '''A test for TryParse
    '''</summary>
    <TestMethod()>
    Public Sub TryParseTestNumericR1()
        Me.TryParseTest(Tegam.RangeMode.R1, 1, 0.002)
    End Sub

    '''<summary>
    '''A test for TryParse
    '''</summary>
    <TestMethod()>
    Public Sub TryParseTestNumericR5()
        Me.TryParseTest(Tegam.RangeMode.R5, 0.1, 0.2)
    End Sub

    '''<summary>
    '''A test for TryParse
    '''</summary>
    <TestMethod()>
    Public Sub TryParseTestNumericR14()
        Me.TryParseTest(Tegam.RangeMode.R14, 0.0001, 2000)
    End Sub

    '''<summary>
    '''A test for TryParse
    '''</summary>
    <TestMethod()>
    Public Sub TryParseTestNumericR19()
        Me.TryParseTest(Tegam.RangeMode.R19, 0.0000001, 20000000.0)
    End Sub

    '''<summary>
    '''A test for Try Convert
    '''</summary>
    Public Sub TryConvertTest(ByVal current As Double, ByVal range As Double, ByVal expectedRangeMode As Tegam.RangeMode)
        Dim rangeMode As Tegam.RangeMode = Tegam.RangeMode.R0
        Dim expected As Boolean = True
        Dim actual As Boolean
        actual = Tegam.MeasureSubsystem.TryConvert(current, range, rangeMode)
        Assert.AreEqual(expected, actual)
        Assert.AreEqual(expectedRangeMode, rangeMode)
    End Sub

    '''<summary>
    '''A test for Try Convert
    '''</summary>
    <TestMethod()>
    Public Sub TryConvertTestNumericR10()
        Me.TryConvertTest(0.01, 200, Tegam.RangeMode.R10)
    End Sub

End Class
