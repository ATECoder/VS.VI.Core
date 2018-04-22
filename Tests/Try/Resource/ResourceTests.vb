Imports Microsoft.Win32
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
        Assert.IsTrue(TestInfo.Exists, $"{GetType(TestInfo)} settings not found")
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

#Region " NULLABLE TESTS "

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
        successParsing = VI.Pith.SessionBase.TryParse(reading, actualResult)
        Assert.AreEqual(expectedResult, actualResult, "Value set to {0}", actualResult)
        Assert.AreEqual(True, successParsing, "Success set to {0}", actualResult)
        reading = "1"
        expectedResult = True
        successParsing = VI.Pith.SessionBase.TryParse(reading, actualResult)
        Assert.AreEqual(expectedResult, actualResult, "Value set to {0}", actualResult)
        Assert.AreEqual(True, successParsing, "Success set to {0}", actualResult)
    End Sub

#End Region

#Region " VERSION TESTS "

    <TestMethod()>
    Public Sub VisaVersionTest()
        Dim value As String = ""
        Dim rv As RegistryView = RegistryView.Registry32
        If Environment.Is64BitOperatingSystem Then
            rv = RegistryView.Registry64
        Else
            rv = RegistryView.Registry32
        End If
        Dim readValue As Object = Nothing
        Dim defaultValue As String = "0.0.0"
#If True Then
        Using regKeyBase As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, rv)
            Using regKey As RegistryKey = regKeyBase.OpenSubKey("SOFTWARE\National Instruments\NI-VISA\", RegistryKeyPermissionCheck.ReadSubTree)
                readValue = regKey.GetValue("CurrentVersion", CObj(defaultValue))
            End Using
        End Using
#Else
        Using regKeyBase As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, rv)
            Using regKey As RegistryKey = regKeyBase.OpenSubKey("SOFTWARE\Microsoft\Cryptography", RegistryKeyPermissionCheck.ReadSubTree)
                readValue = regKey.GetValue("MachineGuid", CObj(defaultValue))
            End Using
        End Using

#End If
        Dim NationalInstrumentVisaVersion As String = "17.5.0"
        If readValue IsNot Nothing AndAlso readValue.ToString() <> "defaultValue" Then
            value = readValue.ToString()
            Assert.AreEqual(NationalInstrumentVisaVersion, value, $"Value set to {value}")
        End If
#If False Then
        Dim varsionString As Object = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\National Instruments\NI-VISA\", "CurrentVersion", "0.0.0.0")
        Assert.AreEqual("17.0.0", varsionString, $"Value set to {varsionString}")
#End If
        Dim version As Version = VI.Pith.ResourcesManagerBase.ReadVendorVersion
        Dim expectedVersion As Version = New Version(NationalInstrumentVisaVersion)
        Assert.IsTrue(version.Equals(expectedVersion), $"Vendor version {version.ToString} equals to {expectedVersion}")

        Dim FoundationVisaFileVersion32 As String = "5.8.908"
        Dim FoundationVisaFileVersion64 As String = "5.9.2008"
        Dim fileVersionInfo As FileVersionInfo = VI.Pith.ResourcesManagerBase.ReadFoundationSystemFileVersionInfo
        If Environment.Is64BitOperatingSystem Then
            expectedVersion = New Version(FoundationVisaFileVersion64)
        Else
            expectedVersion = New Version(FoundationVisaFileVersion32)
        End If
        Assert.AreEqual(expectedVersion.ToString, $"{fileVersionInfo.FileMajorPart}.{fileVersionInfo.FileMinorPart}.{fileVersionInfo.FileBuildPart}",
                        $"Foundation file {VI.Pith.ResourcesManagerBase.FoundationSystemFileFullName} version set to {fileVersionInfo.FileVersion}")

    End Sub

#End Region

End Class
