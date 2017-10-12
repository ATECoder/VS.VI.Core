''' <summary> A resource tests. </summary>
''' <license>
''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="10/11/2017" by="David" revision=""> Created. </history>
<TestClass()>
Public Class ResourceTests


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
    '<ClassInitialize()>  
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region

    '''<summary>
    '''A test for Nullable equality.
    '''</summary>
    <TestMethod()>
    Public Sub NullableBooleanTest()
        Dim bool As Boolean?
        Dim boolValue As Boolean = True
        Assert.AreEqual(True, bool Is Nothing, "Initialized to nothing")
        Assert.AreEqual(True, bool.Equals(New Boolean?), "Nullable not set equals to a new Boolean")
        Assert.AreEqual(False, bool.Equals(boolValue), "Nullable not set is not equal to {0}", boolValue)
        Assert.AreEqual(Nothing, bool = boolValue, "Nullable '=' operator yields Nothing")
        boolValue = False
        Assert.AreEqual(False, bool.Equals(boolValue), "Nullable not set is not equal to True")
        bool = boolValue
        Assert.AreEqual(True, bool.HasValue, "Has value -- set to {0}", boolValue)

        bool = New Nullable(Of Boolean)()
        Assert.AreEqual(True, bool Is Nothing, "Nullable set to new Boolean is still nothing")
        Assert.AreEqual(True, bool.Equals(New Boolean?), "Nullable set to new Boolean equals to a new Boolean")
        Assert.AreEqual(False, bool.Equals(boolValue), "Nullable set to new Boolean not equal to {0}", boolValue)
        boolValue = False
        Assert.AreEqual(False, bool.Equals(boolValue), "Nullable set to new Boolean not equal to True")
        bool = boolValue
        Assert.AreEqual(True, bool.HasValue, "Has value -- set to {0}", boolValue)
    End Sub

    '''<summary>
    '''A test for Nullable Integer equality.
    '''</summary>
    <TestMethod()>
    Public Sub NullableIntegerTest()
        Dim integerValue As Integer = 1
        Dim nullInt As Integer?
        Assert.AreEqual(True, nullInt Is Nothing, "Initialized to nothing")
        nullInt = New Nullable(Of Integer)()
        Assert.AreEqual(True, nullInt Is Nothing, "Nullable set to new Boolean is nothing")
        Assert.AreEqual(True, nullInt.Equals(New Integer?), "Nullable set to new Integer equals to a new Integer")
        Assert.AreEqual(False, nullInt.Equals(integerValue), "Nullable set to new Integer not equal to {0}", integerValue)
        Assert.AreEqual(False, nullInt.Equals(integerValue), "Nullable set to new Integer not equal to {0}", integerValue)
        nullInt = integerValue
        Assert.AreEqual(True, nullInt.HasValue, "Set to {0}", integerValue)
        Assert.AreEqual(integerValue, nullInt.Value, "Set to  {0}", integerValue)
    End Sub


    <TestMethod()>
    Public Sub ParseBooleanTest()
        Dim reading As String = "0"
        Dim expectedResult As Boolean = False
        Dim actualResult As Boolean = True
        Dim successParsing As Boolean = False
        successParsing = isr.VI.SessionBase.TryParse(reading, actualResult)
        Assert.AreEqual(expectedResult, actualResult, "Value set to {0}", actualResult)
        Assert.AreEqual(True, successParsing, "Success set to {0}", actualResult)
        reading = "1"
        expectedResult = True
        successParsing = isr.VI.SessionBase.TryParse(reading, actualResult)
        Assert.AreEqual(expectedResult, actualResult, "Value set to {0}", actualResult)
        Assert.AreEqual(True, successParsing, "Success set to {0}", actualResult)
    End Sub

End Class
