Imports System.Runtime.CompilerServices
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Resource Manager Extensions. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="09/10/2013" by="David" revision="3.0.5001.x"> Created. </history>
Friend Module ResourceManagerExtensions

#Region " CONVERSIONS "

    ''' <summary> Converts the given value. </summary>
    ''' <returns> A Ivi.Visa.HardwareInterfaceType. </returns>
    Public Function ConvertInterfaceType(ByVal value As Ivi.Visa.HardwareInterfaceType) As VI.HardwareInterfaceType
        If [Enum].IsDefined(GetType(VI.HardwareInterfaceType), CInt(value)) Then
            Return CType(CInt(value), VI.HardwareInterfaceType)
        Else
            Return VI.HardwareInterfaceType.Gpib
        End If
    End Function

    ''' <summary> Converts the given value. </summary>
    ''' <returns> A Ivi.Visa.HardwareInterfaceType. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
    Public Function ConvertInterfaceType(ByVal value As VI.HardwareInterfaceType) As Ivi.Visa.HardwareInterfaceType
        If [Enum].IsDefined(GetType(Ivi.Visa.HardwareInterfaceType), CInt(value)) Then
            Return CType(CInt(value), Ivi.Visa.HardwareInterfaceType)
        Else
            Return Ivi.Visa.HardwareInterfaceType.Gpib
        End If
    End Function

    ''' <summary> Build resource message. </summary>
    ''' <param name="ex"> Details of the exception. </param>
    ''' <returns> A an array with a single resource expressing the error. </returns>
    Private Function BuildResourceMessage(ByVal ex As Exception) As IEnumerable(Of String)
        If TypeOf ex.InnerException Is Ivi.Visa.VisaException Then
            With CType(ex.InnerException, Ivi.Visa.NativeVisaException)
                If .ErrorCode = Ivi.Visa.NativeErrorCode.ResourceNotFound Then
                    ' Insufficient location information or the device or resource is not present in the system.  
                    ' VISA error code -1073807343 (0xBFFF0011), ErrorResourceNotFound
                    Return New String() {"Error: Resource Not Found"}
                Else
                    Return New String() {String.Format("{0} code={1}:{2}", .Message, CInt(.ErrorCode), .ErrorCode.ToString)}
                End If
            End With
        Else
            Return New String() {ex.Message}
        End If
    End Function

    ''' <summary> Convert parse result. </summary>
    ''' <param name="value"> Reference to the
    '''                      <see cref="NationalInstruments.Visa.ResourceManager">resource
    '''                      manager</see>. </param>
    ''' <returns> The parse converted result. </returns>
    Public Function ConvertParseResult(value As Ivi.Visa.ParseResult) As VI.ResourceParseResult
        Return New VI.ResourceParseResult(value.OriginalResourceName, ConvertInterfaceType(value.InterfaceType), value.InterfaceNumber)
    End Function

#End Region

#Region " PARSE RESOURCES "

    ''' <summary> Try parse resource. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">           Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="resourceName">    Name of the resource. </param>
    ''' <returns> <c>True</c> if parsed or false if failed parsing. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="3#",
            Justification:="This is the normative implementation of this method.")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    <Extension()>
    Public Function ParseResource(ByVal value As NationalInstruments.Visa.ResourceManager,
                                      ByVal resourceName As String) As VI.ResourceParseResult

        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Try
            Return ConvertParseResult(value.Parse(resourceName))
        Catch ex As Ivi.Visa.NativeVisaException
            Dim nativeError As New NativeError(ex.ErrorCode, resourceName, "@DM", "Parsing resource")
            Throw New NativeException(nativeError, ex)
        End Try

    End Function

#End Region

#Region " FIND RESOURCES "

    ''' <summary> Lists all resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value"> Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource manager</see>. </param>
    ''' <returns> List of all resources. </returns>
    <Extension()>
    Public Function FindResources(ByVal value As NationalInstruments.Visa.ResourceManager) As IEnumerable(Of String)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Dim resources As String = ""
        Try
            resources = VI.ResourceNamesManager.AllResourcesFilter
            Return value.Find(VI.ResourceNamesManager.AllResourcesFilter)
        Catch ex As Ivi.Visa.NativeVisaException
            Dim nativeError As New NativeError(ex.ErrorCode, resources, "@DM", "Finding resources")
            Throw New NativeException(nativeError, ex)
        End Try
    End Function


    ''' <summary> Tries to find resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">     Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource manager</see>. </param>
    ''' <param name="resources"> [in,out] The resources. </param>
    ''' <returns> <c>True</c> if resources were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
            Justification:="This is the normative implementation of this method.")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    <Extension()>
    Public Function TryFindResources(ByVal value As NationalInstruments.Visa.ResourceManager,
                                         ByRef resources As IEnumerable(Of String)) As Boolean
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Try
            Return value.FindResources().Any
        Catch ex As ArgumentException
            resources = ResourceManagerExtensions.buildResourceMessage(ex)
            Return False
        Catch ex As NativeException
            resources = New String() {ex.Message}
            Return False
        End Try
    End Function

    ''' <summary> Tries to find resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">         Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="filter"> A pattern specifying the search. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if resources were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    <Extension()>
    Public Function TryFindResources(ByVal value As NationalInstruments.Visa.ResourceManager,
                                         ByVal filter As String, ByRef resources As IEnumerable(Of String)) As Boolean
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Try
            resources = value.Find(filter)
            Return resources.Any
        Catch ex As ArgumentException
            resources = ResourceManagerExtensions.buildResourceMessage(ex)
            Return False
        Catch ex As Ivi.Visa.NativeVisaException
            resources = New String() {ex.ToFullBlownString}
            Return False
        End Try
    End Function

    ''' <summary> Returns true if the specified resource exists. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">        Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="resourceName"> The resource name. </param>
    ''' <returns> <c>True</c> if the resource was located; Otherwise, <c>False</c>. </returns>
    <Extension()>
    Public Function Exists(ByVal value As NationalInstruments.Visa.ResourceManager, ByVal resourceName As String) As Boolean
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        If String.IsNullOrWhiteSpace(resourceName) Then Throw New ArgumentNullException(NameOf(resourceName))
        Dim resources As IEnumerable(Of String) = value.FindResources()
        Return resources.Contains(resourceName, StringComparer.CurrentCultureIgnoreCase)
    End Function

    ''' <summary> Searches for the interface. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">        Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="resourceName"> The interface resource name. </param>
    ''' <returns> <c>True</c> if the interface was located; Otherwise, <c>False</c>. </returns>
    <Extension()>
    Public Function InterfaceExists(ByVal value As NationalInstruments.Visa.ResourceManager, ByVal resourceName As String) As Boolean
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Dim parseResult As VI.ResourceParseResult = value.ParseResource(resourceName)
        If parseResult.IsParsed Then
            Dim resources As IEnumerable(Of String) = New String() {}
            If value.TryFindInterfaces(parseResult.InterfaceType, resources) Then
                Return resources.Contains(resourceName, StringComparer.CurrentCultureIgnoreCase)
            End If
        End If
        Return False
    End Function

    ''' <summary> Searches for all interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">         Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <returns> The found interface resource names. </returns>
    <Extension()>
    Public Function FindInterfaces(ByVal value As NationalInstruments.Visa.ResourceManager) As IEnumerable(Of String)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Dim filter As String = ""
        Try
            filter = VI.ResourceNamesManager.BuildInterfaceFilter()
            Return value.Find(filter)
        Catch ex As Ivi.Visa.NativeVisaException
            Dim nativeError As New NativeError(ex.ErrorCode, filter, "@DM", "Finding interfaces")
            Throw New NativeException(nativeError, ex)
        End Try
    End Function

    ''' <summary> Tries to find interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">         Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if interfaces were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <Extension(),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
            Justification:="This is the normative implementation of this method.")>
    Public Function TryFindInterfaces(ByVal value As NationalInstruments.Visa.ResourceManager,
                                          ByRef resources As IEnumerable(Of String)) As Boolean
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Try
            resources = value.FindInterfaces()
            Return resources.Any
        Catch ex As ArgumentException
            resources = ResourceManagerExtensions.buildResourceMessage(ex)
            Return False
        Catch ex As NativeException
            resources = New String() {ex.Message}
            Return False
        End Try
    End Function

    ''' <summary> Searches for the interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">         Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The found interface resource names. </returns>
    <Extension()>
    Public Function FindInterfaces(ByVal value As NationalInstruments.Visa.ResourceManager,
                                       ByVal interfaceType As VI.HardwareInterfaceType) As IEnumerable(Of String)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Dim filter As String = ""
        Try
            filter = VI.ResourceNamesManager.BuildInterfaceFilter(interfaceType)
            Return value.Find(filter)
        Catch ex As Ivi.Visa.NativeVisaException
            Dim nativeError As New NativeError(ex.ErrorCode, filter, "@DM", "Finding interfaces")
            Throw New NativeException(nativeError, ex)
        End Try
    End Function

    ''' <summary> Tries to find interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">         Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if interfaces were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <Extension(),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#")>
    Public Function TryFindInterfaces(ByVal value As NationalInstruments.Visa.ResourceManager,
                                          ByVal interfaceType As VI.HardwareInterfaceType,
                                          ByRef resources As IEnumerable(Of String)) As Boolean
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Try
            resources = value.FindInterfaces(interfaceType)
            Return resources.Any
        Catch ex As ArgumentException
            resources = ResourceManagerExtensions.buildResourceMessage(ex)
            Return False
        Catch ex As NativeException
            resources = New String() {ex.Message}
            Return False
        End Try
    End Function

    ''' <summary> Searches for the instrument. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">        Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="resourceName"> The instrument resource name. </param>
    ''' <returns> <c>True</c> if the instrument was located; Otherwise, <c>False</c>. </returns>
    <Extension()>
    Public Function InstrumentExists(ByVal value As NationalInstruments.Visa.ResourceManager, ByVal resourceName As String) As Boolean
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Dim parseResult As VI.ResourceParseResult = value.ParseResource(resourceName)
        If parseResult.IsParsed Then

        End If
        Dim result As VI.ResourceParseResult = value.ParseResource(resourceName)
        If result.IsParsed Then
            Dim resources As IEnumerable(Of String) = New String() {}
            If value.TryFindInstruments(result.InterfaceType, result.InterfaceNumber, resources) Then
                Return resources.Contains(resourceName, StringComparer.CurrentCultureIgnoreCase)
            End If
        End If
        Return False
    End Function

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">         Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <returns> The found instrument resource names. </returns>
    <Extension()>
    Public Function FindInstruments(ByVal value As NationalInstruments.Visa.ResourceManager) As IEnumerable(Of String)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Dim filter As String = ""
        Try
            filter = isr.VI.ResourceNamesManager.BuildInstrumentFilter()
            Return value.Find(filter)
        Catch ex As Ivi.Visa.NativeVisaException
            Dim nativeError As New NativeError(ex.ErrorCode, filter, "@DM", "Finding instruments")
            Throw New NativeException(nativeError, ex)
        End Try
    End Function

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">         Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <Extension(),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
            Justification:="This is the normative implementation of this method.")>
    Public Function TryFindInstruments(ByVal value As NationalInstruments.Visa.ResourceManager,
                                           ByRef resources As IEnumerable(Of String)) As Boolean
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Try
            resources = value.FindInstruments()
            Return resources.Any
        Catch ex As ArgumentException
            resources = ResourceManagerExtensions.buildResourceMessage(ex)
            Return False
        Catch ex As NativeException
            resources = New String() {ex.Message}
            Return False
        End Try
    End Function

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">         Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The found instrument resource names. </returns>
    <Extension()>
    Public Function FindInstruments(ByVal value As NationalInstruments.Visa.ResourceManager,
                                        ByVal interfaceType As VI.HardwareInterfaceType) As IEnumerable(Of String)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Dim filter As String = ""
        Try
            filter = isr.VI.ResourceNamesManager.BuildInstrumentFilter(interfaceType)
            Return value.Find(filter)
        Catch ex As Ivi.Visa.NativeVisaException
            Dim nativeError As New NativeError(ex.ErrorCode, filter, "@DM", "Finding instruments")
            Throw New NativeException(nativeError, ex)
        End Try
    End Function

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">         Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    <Extension()>
    Public Function TryFindInstruments(ByVal value As NationalInstruments.Visa.ResourceManager,
                                           ByVal interfaceType As VI.HardwareInterfaceType,
                                           ByRef resources As IEnumerable(Of String)) As Boolean
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Try
            resources = value.FindInstruments(interfaceType)
            Return resources.Any
        Catch ex As ArgumentException
            resources = ResourceManagerExtensions.buildResourceMessage(ex)
            Return False
        Catch ex As NativeException
            resources = New String() {ex.Message}
            Return False
        End Try
    End Function

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">         Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="boardNumber">   The board number. </param>
    ''' <returns> The found instrument resource names. </returns>
    <Extension()>
    Public Function FindInstruments(ByVal value As NationalInstruments.Visa.ResourceManager,
                                        ByVal interfaceType As VI.HardwareInterfaceType, ByVal boardNumber As Integer) As IEnumerable(Of String)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Dim filter As String = ""
        Try
            filter = isr.VI.ResourceNamesManager.BuildInstrumentFilter(interfaceType, boardNumber)
            Return value.Find(filter)
        Catch ex As Ivi.Visa.NativeVisaException
            Dim nativeError As New NativeError(ex.ErrorCode, filter, "@DM", "Finding instruments")
            Throw New NativeException(nativeError, ex)
        End Try
    End Function

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">           Reference to the <see cref="NationalInstruments.Visa.ResourceManager">resource
    ''' manager</see>. </param>
    ''' <param name="interfaceType">   Type of the interface. </param>
    ''' <param name="interfaceNumber"> The interface number (e.g., board or port number). </param>
    ''' <param name="resources">       [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="3#",
            Justification:="This is the normative implementation of this method.")>
    <Extension()>
    Public Function TryFindInstruments(ByVal value As NationalInstruments.Visa.ResourceManager,
                                           ByVal interfaceType As VI.HardwareInterfaceType,
                                           ByVal interfaceNumber As Integer, ByRef resources As IEnumerable(Of String)) As Boolean
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Try
            resources = value.FindInstruments(interfaceType, interfaceNumber)
            Return resources.Any
        Catch ex As ArgumentException
            resources = ResourceManagerExtensions.buildResourceMessage(ex)
            Return False
        Catch ex As NativeException
            resources = New String() {ex.Message}
            Return False
        End Try
    End Function

#End Region

End Module

