''' <summary> The resources manager base. </summary>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="11/21/2015" by="David" revision=""> Created. </history>
Public MustInherit Class ResourcesManagerBase
    Implements IDisposable

#Region " PARSE RESOURCES "

    ''' <summary> Gets the sentinel indicating weather this is a dummy session. </summary>
    ''' <value> The dummy sentinel. </value>
    Public MustOverride ReadOnly Property IsDummy As Boolean

    ''' <summary> Parse resource. </summary>
    ''' <param name="resourceName"> The resource name. </param>
    ''' <returns> A VI.ResourceParseResult. </returns>
    Public MustOverride Function ParseResource(ByVal resourceName As String) As ResourceNameParseInfo

#End Region

#Region " FIND RESOURCES "

    ''' <summary> Lists all resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> List of all resources. </returns>
    Public MustOverride Function FindResources() As IEnumerable(Of String)

    ''' <summary> Lists all resources. </summary>
    ''' <param name="filter"> A pattern specifying the search. </param>
    ''' <returns> List of all resources. </returns>
    Public MustOverride Function FindResources(ByVal filter As String) As IEnumerable(Of String)

    ''' <summary> Tries to find resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resources"> [in,out] The resources. </param>
    ''' <returns> <c>True</c> if resources were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public MustOverride Function TryFindResources(ByRef resources As IEnumerable(Of String)) As Boolean

    ''' <summary> Tries to find resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="filter"> A pattern specifying the search. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if resources were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public MustOverride Function TryFindResources(ByVal filter As String, ByRef resources As IEnumerable(Of String)) As Boolean

    ''' <summary> Returns true if the specified resource exists. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> The resource name. </param>
    ''' <returns> <c>True</c> if the resource was located; Otherwise, <c>False</c>. </returns>
    Public MustOverride Function Exists(ByVal resourceName As String) As Boolean

#End Region

#Region " INTERFACES "

    ''' <summary> Searches for the interface. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> The interface resource name. </param>
    ''' <returns> <c>True</c> if the interface was located; Otherwise, <c>False</c>. </returns>
    Public MustOverride Function InterfaceExists(ByVal resourceName As String) As Boolean

    ''' <summary> Searches for all interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> The found interface resource names. </returns>
    Public MustOverride Function FindInterfaces() As IEnumerable(Of String)

    ''' <summary> Tries to find interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if interfaces were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public MustOverride Function TryFindInterfaces(ByRef resources As IEnumerable(Of String)) As Boolean

    ''' <summary> Searches for the interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The found interface resource names. </returns>
    Public MustOverride Function FindInterfaces(ByVal interfaceType As VI.Pith.HardwareInterfaceType) As IEnumerable(Of String)

    ''' <summary> Tries to find interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if interfaces were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public MustOverride Function TryFindInterfaces(ByVal interfaceType As VI.Pith.HardwareInterfaceType, ByRef resources As IEnumerable(Of String)) As Boolean

#End Region

#Region " INSTRUMENTS  "

    ''' <summary> Searches for the instrument. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> The instrument resource name. </param>
    ''' <returns> <c>True</c> if the instrument was located; Otherwise, <c>False</c>. </returns>
    Public MustOverride Function InstrumentExists(ByVal resourceName As String) As Boolean

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> The found instrument resource names. </returns>
    Public MustOverride Function FindInstruments() As IEnumerable(Of String)

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#",
        Justification:="This is the normative implementation of this method.")>
    Public MustOverride Function TryFindInstruments(ByRef resources As IEnumerable(Of String)) As Boolean

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The found instrument resource names. </returns>
    Public MustOverride Function FindInstruments(ByVal interfaceType As VI.Pith.HardwareInterfaceType) As IEnumerable(Of String)

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
        Justification:="This is the normative implementation of this method.")>
    Public MustOverride Function TryFindInstruments(ByVal interfaceType As VI.Pith.HardwareInterfaceType,
                                                    ByRef resources As IEnumerable(Of String)) As Boolean

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="boardNumber">   The board number. </param>
    ''' <returns> The found instrument resource names. </returns>
    Public MustOverride Function FindInstruments(ByVal interfaceType As VI.Pith.HardwareInterfaceType, ByVal boardNumber As Integer) As IEnumerable(Of String)

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType">   Type of the interface. </param>
    ''' <param name="interfaceNumber"> The interface number (e.g., board or port number). </param>
    ''' <param name="resources">       [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
        Justification:="This is the normative implementation of this method.")>
    Public MustOverride Function TryFindInstruments(ByVal interfaceType As VI.Pith.HardwareInterfaceType,
                                                    ByVal interfaceNumber As Integer,
                                                    ByRef resources As IEnumerable(Of String)) As Boolean


    ''' <summary> Searches for the first resource. </summary>
    ''' <param name="resourceNames"> List of names of the resources. </param>
    ''' <returns> The found resource. </returns>
    Public Function FindResource(ByVal resourceNames As IEnumerable(Of String)) As String
        Dim result As String = ""
        If resourceNames IsNot Nothing AndAlso resourceNames.Any Then
            For Each rn As String In resourceNames
                If Me.InstrumentExists(rn) Then
                    result = rn
                    Exit For
                End If
            Next
        End If
        Return result
    End Function

#End Region

#Region " REVISION MANANGEMENT "

    ''' <summary> Determine if the elements of the two versions are equal. </summary>
    ''' <param name="expectedVersion"> The expected version. </param>
    ''' <param name="actualVersion">   The actual version. </param>
    ''' <returns> <c>true</c> if equal; otherwise <c>false</c> </returns>
    Private Shared Function AreEqual(ByVal expectedVersion As String, actualVersion As String) As Boolean
        Dim delimiter As Char = "."c
        Return ResourcesManagerBase.AreEqual(expectedVersion.Split(delimiter), actualVersion.Split(delimiter))
    End Function

    ''' <summary> Determine if the elements of the two versions are equal. </summary>
    ''' <param name="expectedValues"> The expected values. </param>
    ''' <param name="actualValues">   The actual values. </param>
    ''' <returns> <c>true</c> if equal; otherwise <c>false</c> </returns>
    Private Shared Function AreEqual(ByVal expectedValues As String(), actualValues As String()) As Boolean
        Dim result As Boolean = True
        For i As Integer = 0 To Math.Min(expectedValues.Count, actualValues.Count) - 1
            result = result AndAlso String.Equals(expectedValues(i), actualValues(i), StringComparison.OrdinalIgnoreCase)
        Next
        Return result
    End Function

    ''' <summary> Attempts to validate visa versions using settings versions. </summary>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Shared Function TryValidateVisaVersions(ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Not ResourcesManagerBase.ValidateFoundationSystemFileVersion(e) Then
        ElseIf Not ResourcesManagerBase.ValidateFoundationVisaAssemblyVersion(e) Then
        ElseIf Not ResourcesManagerBase.ValidateVendorVersion(e) Then
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> Validates the visa versions. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    Public Shared Sub ValidateVisaVersions()
        Dim e As New isr.Core.Pith.ActionEventArgs
        If Not ResourcesManagerBase.TryValidateVisaVersions(e) Then
            Throw New VI.Pith.OperationFailedException($"Invalid VISA version: {e.Details}")
        End If
    End Sub

#Region " REGISTERY VISA VERSION "

    ''' <summary> Gets or sets the full pathname of the vendor version file. </summary>
    ''' <value> The full pathname of the vendor version file. </value>
    Public Shared Property VendorVersionPath As String = "SOFTWARE\National Instruments\NI-VISA\"

    ''' <summary> Gets or sets the name of the vendor version key. </summary>
    ''' <value> The name of the vendor version key. </value>
    Public Shared Property VendorVersionKeyName As String = "CurrentVersion"

    ''' <summary> Reads vendor version. </summary>
    ''' <returns> The vendor version. </returns>
    Public Shared Function ReadVendorVersion() As Version
        Dim version As String = isr.Core.Pith.MachineInfo.ReadRegistry(Microsoft.Win32.RegistryHive.LocalMachine, VendorVersionPath, VendorVersionKeyName, "0.0.0.0")
        Return New Version(version)
    End Function

    ''' <summary> Validates the vendor version. </summary>
    ''' <param name="expectedVersion"> The expected version. </param>
    ''' <param name="e">               Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Shared Function ValidateVendorVersion(ByVal expectedVersion As String, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim actualVersion As String = ResourcesManagerBase.ReadVendorVersion.ToString
        If Not ResourcesManagerBase.AreEqual(expectedVersion, actualVersion) Then
            e.RegisterCancellation($"Vendor {ResourcesManagerBase.VendorVersionPath} version {actualVersion} different from expected {expectedVersion}")
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> Validates the National Instruments vendor version. </summary>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Shared Function ValidateVendorVersion(ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        Return ResourcesManagerBase.ValidateVendorVersion(ResourcesManagerBase.ExpectedVendorVersion, e)
    End Function

    ''' <summary> Gets the expected vendor version. </summary>
    ''' <value> The expected vendor version. </value>
    Public Shared ReadOnly Property ExpectedVendorVersion As String
        Get
            Return My.Settings.NationalInsrumentVisaVersion
        End Get
    End Property

#End Region

#Region " FOUNDATION IVI VISA DLL FILE VERSION "

    ''' <summary> Gets or sets the filename of the foundation .NET VISA file. </summary>
    ''' <value> The filename of the foundation .NET VISA file. </value>
    Public Shared ReadOnly Property FoundationVisaAssemblyFileName As String = "ivi.visa.dll"

    ''' <summary> Gets the name of the foundation .NET visa file full. </summary>
    ''' <value> The name of the foundation .NET visa file full. </value>
    Public Shared ReadOnly Property FoundationVisaAssemblyFileFullName As String
        Get
            Return System.IO.Path.Combine(My.Application.Info.DirectoryPath, ResourcesManagerBase.FoundationVisaAssemblyFileName)
        End Get
    End Property

    ''' <summary> Reads foundation .NET visa file version. </summary>
    ''' <returns> The foundation .NET visa file version. </returns>
    Public Shared Function ReadFoundationVisaAssemblyFileVersionInfo() As FileVersionInfo
        Return FileVersionInfo.GetVersionInfo(ResourcesManagerBase.FoundationVisaAssemblyFileFullName)
    End Function

    ''' <summary> Validates the foundation visa Assembly version. </summary>
    ''' <param name="expectedVersion"> The expected version. </param>
    ''' <param name="e">               Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Shared Function ValidateFoundationVisaAssemblyVersion(ByVal expectedVersion As String, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim actualVersion As String = ResourcesManagerBase.ReadFoundationVisaAssemblyFileVersionInfo.FileVersion.ToString
        If Not ResourcesManagerBase.AreEqual(expectedVersion, actualVersion) Then
            e.RegisterCancellation($"IVI Foundation VISA assembly {ResourcesManagerBase.FoundationVisaAssemblyFileFullName} version {actualVersion} different from expected {expectedVersion}")
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> Validates the foundation visa Assembly version. </summary>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Shared Function ValidateFoundationVisaAssemblyVersion(ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        Return ResourcesManagerBase.ValidateFoundationVisaAssemblyVersion(ResourcesManagerBase.ExpectedFoundationVisaAssemblyVersion, e)
    End Function

    Public Shared ReadOnly Property ExpectedFoundationVisaAssemblyVersion As String
        Get
            Return My.Settings.FoundationVisaAssemblyVersion
        End Get
    End Property

#End Region

#Region " FOUNDATION SYSTEM VISA DLL FILE VERSION "

    ''' <summary> Gets or sets the filename of the foundation system DLL file. </summary>
    ''' <value> The filename of the foundation system DLL file. </value>
    Public Shared ReadOnly Property FoundationSystemFileName As String = "visa32.dll"

    ''' <summary> Gets the name of the foundation system DLL file full name. </summary>
    ''' <value> The name of the foundation system DLL file full name. </value>
    Public Shared ReadOnly Property FoundationSystemFileFullName As String
        Get
            Return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), ResourcesManagerBase.FoundationSystemFileName)
        End Get
    End Property

    ''' <summary> Reads foundation file version. </summary>
    ''' <returns> The foundation file version. </returns>
    Public Shared Function ReadFoundationSystemFileVersionInfo() As FileVersionInfo
        Return FileVersionInfo.GetVersionInfo(ResourcesManagerBase.FoundationSystemFileFullName)
    End Function

    ''' <summary> Validates the foundation file version. </summary>
    ''' <param name="expectedVersion"> The expected version. </param>
    ''' <param name="e">               Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Shared Function ValidateFoundationSystemFileVersion(ByVal expectedVersion As String, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim actualVersion As String = ResourcesManagerBase.ReadFoundationSystemFileVersionInfo.FileVersion.ToString
        If Not ResourcesManagerBase.AreEqual(expectedVersion, actualVersion) Then
            e.RegisterCancellation($"IVI Foundation system file {ResourcesManagerBase.FoundationSystemFileFullName} version {actualVersion} different from expected {expectedVersion}")
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> Gets the expected foundation system file version. </summary>
    ''' <value> The expected foundation system file version. </value>
    Public Shared ReadOnly Property ExpectedFoundationSystemFileVersion As String
        Get
            If Environment.Is64BitProcess Then
                Return My.Settings.FoundationSystemFileVersion64
            Else
                ' IDE is running in 32 bit mode.
                Return My.Settings.FoundationSystemFileVersion32
            End If
        End Get
    End Property

    ''' <summary> Validates the foundation system file version described by e. </summary>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Shared Function ValidateFoundationSystemFileVersion(ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        Return ResourcesManagerBase.ValidateFoundationSystemFileVersion(ResourcesManagerBase.ExpectedFoundationSystemFileVersion, e)
    End Function

#End Region

#End Region

#Region " Disposable Support"

    ''' <summary> Gets or sets the disposed sentinel. </summary>
    ''' <value> The disposed. </value>
    Property IsDisposed As Boolean

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overridable Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
            End If
        Finally
            Me.IsDisposed = True
        End Try
    End Sub

    ''' <summary> Finalizes this object. </summary>
    ''' <remarks> David, 11/21/2015: Override because Dispose(disposing As Boolean) above has code to free unmanaged resources.
    ''' </remarks>
    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ''' <summary>
    ''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    ''' resources.
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' uncommented because Finalize() is overridden above.
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class

