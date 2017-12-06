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
    Public MustOverride Function ParseResource(ByVal resourceName As String) As VI.ResourceParseResult

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
    Public MustOverride Function FindInterfaces(ByVal interfaceType As HardwareInterfaceType) As IEnumerable(Of String)

    ''' <summary> Tries to find interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if interfaces were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public MustOverride Function TryFindInterfaces(ByVal interfaceType As HardwareInterfaceType, ByRef resources As IEnumerable(Of String)) As Boolean

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
    Public MustOverride Function FindInstruments(ByVal interfaceType As HardwareInterfaceType) As IEnumerable(Of String)

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
        Justification:="This is the normative implementation of this method.")>
    Public MustOverride Function TryFindInstruments(ByVal interfaceType As HardwareInterfaceType,
                                                    ByRef resources As IEnumerable(Of String)) As Boolean

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="boardNumber">   The board number. </param>
    ''' <returns> The found instrument resource names. </returns>
    Public MustOverride Function FindInstruments(ByVal interfaceType As HardwareInterfaceType, ByVal boardNumber As Integer) As IEnumerable(Of String)

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
    Public MustOverride Function TryFindInstruments(ByVal interfaceType As HardwareInterfaceType,
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

    ''' <summary> Gets or sets the full pathname of the vendor version file. </summary>
    ''' <value> The full pathname of the vendor version file. </value>
    Public Shared Property VendorVersionPath As String = "SOFTWARE\National Instruments\NI-VISA\"

    ''' <summary> Gets or sets the name of the vendor version key. </summary>
    ''' <value> The name of the vendor version key. </value>
    Public Shared Property VendorVersionKeyName As String = "CurrentVersion"

    ''' <summary> Reads vendor version. </summary>
    ''' <returns> The vendor version. </returns>
    Public Shared Function ReadVendorVersion() As Version
        Dim version As String = isr.Core.Pith.MachineInfo.ReadRegistry(Microsoft.Win32.RegistryHive.LocalMachine, VendorVersionPath, VendorVersionKeyName, "0.0.0")
        Return New Version(version)
    End Function

    Public Shared Function ValidateVendorVersion(ByVal expectedVersion As String, ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        Dim actualVersion As String = ResourcesManagerBase.ReadVendorVersion.ToString
        If Not String.Equals(actualVersion, expectedVersion) Then
            e.RegisterCancellation($"Vendor version {actualVersion} different from expected {expectedVersion}")
        End If
        Return Not e.Cancel
    End Function

    ''' <summary> Gets or sets the filename of the foundation file. </summary>
    ''' <value> The filename of the foundation file. </value>
    Public Shared ReadOnly Property FoundationFileName As String = "visa32.dll"

    ''' <summary> Gets the name of the foundation file full. </summary>
    ''' <value> The name of the foundation file full. </value>
    Public Shared ReadOnly Property FoundationFileFullName As String
        Get
            Return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), ResourcesManagerBase.FoundationFileName)
        End Get
    End Property

    ''' <summary> Reads foundation file version. </summary>
    ''' <returns> The foundation file version. </returns>
    Public Shared Function ReadFoundationFileVersion() As FileVersionInfo
        Return FileVersionInfo.GetVersionInfo(ResourcesManagerBase.FoundationFileFullName)
    End Function

    ''' <summary> Reads foundation version. </summary>
    ''' <returns> The foundation version. </returns>
    Public Shared Function ReadFoundationVersion() As Version
        With ReadFoundationFileVersion()
            Return New Version(.FileMajorPart, .FileMinorPart, .FileBuildPart)
        End With
    End Function

    ''' <summary> Validates the foundation file version. </summary>
    ''' <param name="expectedVersion"> The expected version. </param>
    ''' <param name="e">               Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Shared Function ValidateFoundationVersion(ByVal expectedVersion As String, ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        Dim actualVersion As String = ResourcesManagerBase.ReadFoundationVersion.ToString
        If Not String.Equals(actualVersion, expectedVersion) Then
            e.RegisterCancellation($"IVI Foundation file version {actualVersion} different from expected {expectedVersion}")
        End If
        Return Not e.Cancel
    End Function

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

