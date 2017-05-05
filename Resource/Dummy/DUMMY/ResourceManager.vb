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
Public Class ResourceManager
    Implements IDisposable

#Region " CONSTRUCTORS "

    ''' <summary>
    ''' Constructor that prevents a default instance of this class from being created.
    ''' </summary>
    Public Sub New()
        MyBase.New
    End Sub

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

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
            If Not Me.disposedValue AndAlso disposing Then
            End If
        Finally
            Me.disposedValue = True
        End Try
    End Sub

    ''' <summary> Finalizes this object. </summary>
    ''' <remarks> David, 11/23/2015. 
    '''           Override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    '''           </remarks>
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
        ' uncommented the following line because Finalize() is overridden above.
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#End Region

#Region " Resource Manager "

#End Region

#Region " PARSE RESOURCES "

    ''' <summary> Parse resource. </summary>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <returns> A VI.ResourceParseResult. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public Function ParseResource(ByVal resourceName As String) As VI.ResourceParseResult
        Return New VI.ResourceParseResult(resourceName)
    End Function

#End Region

#Region " FIND RESOURCES "

    ''' <summary> Lists all resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> List of all resources. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public Function FindResources() As IEnumerable(Of String)
        Return New List(Of String)
    End Function

    ''' <summary> Tries to find resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resources"> [in,out] The resources. </param>
    ''' <returns> <c>True</c> if resources were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#",
        Justification:="This is the normative implementation of this method.")>
    Public Function TryFindResources(ByRef resources As IEnumerable(Of String)) As Boolean
        resources = New String() {}
        Return True
    End Function

    ''' <summary> Tries to find resources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="filter"> A pattern specifying the search. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if resources were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
        Justification:="This is the normative implementation of this method.")>
    Public Function TryFindResources(ByVal filter As String, ByRef resources As IEnumerable(Of String)) As Boolean
        resources = New String() {filter}
        Return True
    End Function

    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>
    Private _ResourceName As String
    ''' <summary> Returns true if the specified resource exists. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> The resource name. </param>
    ''' <returns> <c>True</c> if the resource was located; Otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public Function Exists(ByVal resourceName As String) As Boolean
        Me._resourceName = resourceName
        Return True
    End Function

    ''' <summary> Searches for the interface. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> The interface resource name. </param>
    ''' <returns> <c>True</c> if the interface was located; Otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public Function InterfaceExists(ByVal resourceName As String) As Boolean
        Me._resourceName = resourceName
        Return True
    End Function

    ''' <summary> Searches for all interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> The found interface resource names. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public Function FindInterfaces() As IEnumerable(Of String)
        Return New String() {}
    End Function

    ''' <summary> Tries to find interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if interfaces were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#",
                                                         Justification:="This is the normative implementation of this method.")>
    Public Function TryFindInterfaces(ByRef resources As IEnumerable(Of String)) As Boolean
        resources = New String() {}
        Return True
    End Function

    ''' <summary> Searches for the interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The found interface resource names. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public Function FindInterfaces(ByVal interfaceType As HardwareInterfaceType) As IEnumerable(Of String)
        Return New String() {ResourceNamesManager.InterfaceBaseName(interfaceType)}
    End Function

    ''' <summary> Tries to find interfaces. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if interfaces were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Function TryFindInterfaces(ByVal interfaceType As HardwareInterfaceType, ByRef resources As IEnumerable(Of String)) As Boolean
        resources = New String() {ResourceNamesManager.InterfaceBaseName(interfaceType)}
        Return True
    End Function

    ''' <summary> Searches for the instrument. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> The instrument resource name. </param>
    ''' <returns> <c>True</c> if the instrument was located; Otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public Function FindInstrument(ByVal resourceName As String) As Boolean
        Me._resourceName = resourceName
        Return True
    End Function

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> The found instrument resource names. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public Function FindInstruments() As IEnumerable(Of String)
        Return New String() {}
    End Function

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#",
        Justification:="This is the normative implementation of this method.")>
    Public Function TryFindInstruments(ByRef resources As IEnumerable(Of String)) As Boolean
        resources = New String() {}
        Return True
    End Function

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The found instrument resource names. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public Function FindInstruments(ByVal interfaceType As HardwareInterfaceType) As IEnumerable(Of String)
        Return New String() {ResourceNamesManager.InterfaceBaseName(interfaceType)}
    End Function

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="resources">     [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
        Justification:="This is the normative implementation of this method.")>
    Public Function TryFindInstruments(ByVal interfaceType As HardwareInterfaceType,
                                       ByRef resources As IEnumerable(Of String)) As Boolean
        resources = New String() {ResourceNamesManager.InterfaceBaseName(interfaceType)}
        Return True
    End Function

    ''' <summary> Searches for instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="boardNumber">   The board number. </param>
    ''' <returns> The found instrument resource names. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public Function FindInstruments(ByVal interfaceType As HardwareInterfaceType, ByVal boardNumber As Integer) As IEnumerable(Of String)
        Return New String() {$"{ResourceNamesManager.InterfaceBaseName(interfaceType)}{boardNumber}"}
    End Function

    ''' <summary> Tries to find instruments. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="interfaceType">   Type of the interface. </param>
    ''' <param name="interfaceNumber"> The interface number (e.g., board or port number). </param>
    ''' <param name="resources">       [in,out] The resources. </param>
    ''' <returns> <c>True</c> if instruments were located or false if failed or no instrument resources were
    ''' located. If exception occurred, the exception details are returned in the first element of the
    ''' <paramref name="resources"/>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
        Justification:="This is the normative implementation of this method.")>
    Public Function TryFindInstruments(ByVal interfaceType As HardwareInterfaceType,
                                       ByVal interfaceNumber As Integer, ByRef resources As IEnumerable(Of String)) As Boolean
        resources = New String() {$"{ResourceNamesManager.InterfaceBaseName(interfaceType)}{interfaceNumber}"}
        Return True
    End Function

#End Region

End Class
