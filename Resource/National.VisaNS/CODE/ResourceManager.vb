Imports isr.VI.National.VisaNS.ResourceManagerExtensions
''' <summary> A VISA Resource Manager. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="09/10/2013" by="David" revision="3.0.5001.x"> Created. </history>
Public NotInheritable Class ResourceManager

#Region " CONSTRUCTORS "

    ''' <summary>
    ''' Constructor that prevents a default instance of this class from being created.
    ''' </summary>
    ''' <remarks> David, 11/21/2015. </remarks>
    Private Sub New()
        MyBase.New
    End Sub

#End Region

#Region " PARSE RESOURCES "

    ''' <summary> Parse resource. </summary>
    ''' <remarks> David, 11/23/2015. </remarks>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <returns> A VI.ResourceParseResult. </returns>
    Public Shared Function ParseResource(ByVal resourceName As String) As VI.ResourceParseResult
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.ParseResource(resourceName)
    End Function

#End Region

#Region " FIND RESOURCES "

    ''' <summary> Lists all resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> List of all resources. </returns>
    Public Shared Function FindResources() As IEnumerable(Of String)
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.FindResources()
    End Function

    ''' <summary> Tries to find resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resources"> [in,out] The resources. </param>
    ''' <returns> <c>True</c> if resources were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#",
        Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryFindResources(ByRef resources As IEnumerable(Of String)) As Boolean
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.TryFindResources(resources)
    End Function

    ''' <summary> Tries to find resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="filter"> A pattern specifying the search. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if resources were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
        Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryFindResources(ByVal filter As String, ByRef resources As IEnumerable(Of String)) As Boolean
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.TryFindResources(filter, resources)
    End Function

    ''' <summary> Returns true if the specified resource exists. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> The resource name. </param>
    ''' <returns> <c>True</c> if the resource was located; Otherwise, <c>False</c>. </returns>
    Public Shared Function Exists(ByVal resourceName As String) As Boolean
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.Exists(resourceName)
    End Function

    ''' <summary> Searches for the interface. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> The interface resource name. </param>
    ''' <returns> <c>True</c> if the interface was located; Otherwise, <c>False</c>. </returns>
    Public Shared Function InterfaceExists(ByVal resourceName As String) As Boolean
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.InterfaceExists(resourceName)
    End Function

    ''' <summary> Searches for all interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> The found interface resource names. </returns>
    Public Shared Function FindInterfaces() As IEnumerable(Of String)
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.FindInterfaces()
    End Function

    ''' <summary> Tries to find interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if interfaces were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#",
                                                         Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryFindInterfaces(ByRef resources As IEnumerable(Of String)) As Boolean
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.TryFindInterfaces(resources)
    End Function

    ''' <summary> Searches for the interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The found interface resource names. </returns>
    Public Shared Function FindInterfaces(ByVal interfaceType As HardwareInterfaceType) As IEnumerable(Of String)
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.FindInterfaces(interfaceType)
    End Function

    ''' <summary> Tries to find interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if interfaces were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function TryFindInterfaces(ByVal interfaceType As HardwareInterfaceType, ByRef resources As IEnumerable(Of String)) As Boolean
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.TryFindInterfaces(interfaceType, resources)
    End Function

    ''' <summary> Searches for the instrument. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> The instrument resource name. </param>
    ''' <returns> <c>True</c> if the instrument was located; Otherwise, <c>False</c>. </returns>
    Public Shared Function FindInstrument(ByVal resourceName As String) As Boolean
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.InstrumentExists(resourceName)
    End Function

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> The found instrument resource names. </returns>
    Public Shared Function FindInstruments() As IEnumerable(Of String)
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.FindInstruments()
    End Function

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#",
        Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryFindInstruments(ByRef resources As IEnumerable(Of String)) As Boolean
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.TryFindInstruments(resources)
    End Function

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The found instrument resource names. </returns>
    Public Shared Function FindInstruments(ByVal interfaceType As HardwareInterfaceType) As IEnumerable(Of String)
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.FindInstruments(interfaceType)
    End Function

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
        Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryFindInstruments(ByVal interfaceType As HardwareInterfaceType,
                                                  ByRef resources As IEnumerable(Of String)) As Boolean
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.TryFindInstruments(interfaceType, resources)
    End Function

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="boardNumber">   The board number. </param>
    ''' <returns> The found instrument resource names. </returns>
    Public Shared Function FindInstruments(ByVal interfaceType As HardwareInterfaceType, ByVal boardNumber As Integer) As IEnumerable(Of String)
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.FindInstruments(interfaceType, boardNumber)
    End Function

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
    Public Shared Function TryFindInstruments(ByVal interfaceType As HardwareInterfaceType,
                                                  ByVal interfaceNumber As Integer, ByRef resources As IEnumerable(Of String)) As Boolean
        Return NationalInstruments.VisaNS.ResourceManager.GetLocalManager.TryFindInstruments(interfaceType, interfaceNumber, resources)
    End Function

#End Region

End Class
