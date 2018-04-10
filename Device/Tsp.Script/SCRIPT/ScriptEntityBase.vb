''' <summary> Provides the contract for a Script Entity. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
'''  <history date="03/02/2009" by="David" revision="3.0.3348.x"> Created. </history>
Public MustInherit Class ScriptEntityBase
    Implements IDisposable

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    Protected Sub New()
        MyBase.New()
        Me._name = ""
        Me._modelMask = ""
        Me._embeddedFirmwareVersion = ""
        'Me._isBootScript = False
        'Me._isSupportScript = False
        'Me._isPrimaryScript = False
        Me._fileName = ""
        Me._FirmwareVersionQueryCommand = ""
        Me.namespaceListSetter("")
        Me._releasedFirmwareVersion = ""
        'Me._requiresReadParseWrite = False
        'Me._savedToFile = False
        Me._source = ""
        Me._sourceFormat = ScriptFileFormats.None
        Me._timeout = TimeSpan.FromMilliseconds(10000)
    End Sub

    ''' <summary>
    ''' Constructs this class.
    ''' </summary>
    ''' <param name="name">Specifies the script name.</param>
    ''' <param name="modelMask">Specifies the model families for this script.</param>
    Protected Sub New(ByVal name As String, ByVal modelMask As String)
        Me.New()
        Me._name = name
        Me._modelMask = modelMask
    End Sub

    ''' <summary>Calls <see cref="M:Dispose(Boolean Disposing)"/> to cleanup.</summary>
    ''' <remarks>Do not make this method Overridable (virtual) because a derived 
    '''   class should not be able to override this method.</remarks>
    Public Sub Dispose() Implements IDisposable.Dispose

        ' Do not change this code.  Put cleanup code in Dispose(Boolean) below.

        ' this disposes all child classes.
        Dispose(True)

        ' GC.SuppressFinalize is issued to take this object off the 
        ' finalization(Queue) and prevent finalization code for this 
        ' prevent from executing a second time.
        GC.SuppressFinalize(Me)

    End Sub

    ''' <summary>
    ''' Gets or sets the dispose status sentinel of the base class.  This applies to the derived class
    ''' provided proper implementation.
    ''' </summary>
    Protected Property IsDisposed() As Boolean

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' Free managed resources when explicitly called
                Me._source = ""
            End If
        Finally
            Me.IsDisposed = True
        End Try

    End Sub

#End Region

#Region " SHARED "

    ''' <summary>
    ''' Returns the compressed code prefix.
    ''' </summary>
    Public Shared ReadOnly Property CompressedPrefix() As String
        Get
            Return "<COMPRESSED>"
        End Get
    End Property

    ''' <summary>
    ''' Returns the compressed code suffix.
    ''' </summary>
    Public Shared ReadOnly Property CompressedSuffix() As String
        Get
            Return "</COMPRESSED>"
        End Get
    End Property

    ''' <summary> Returns a compressed value. </summary>
    ''' <param name="value"> The string being chopped. </param>
    ''' <returns> Compressed value. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")>
    Public Shared Function Compress(ByVal value As String) As String

        If String.IsNullOrWhiteSpace(value) Then
            Return ""
            Exit Function
        End If

        Dim result As String = ""

        ' Compress the byte array
        Using memoryStream As New System.IO.MemoryStream()

            Using compressedStream As New System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionMode.Compress)

                ' Convert the uncompressed string into a byte array
                Dim values As Byte() = System.Text.Encoding.UTF8.GetBytes(value)
                compressedStream.Write(values, 0, values.Length)

                ' Don't FLUSH here - it possibly leads to data loss!
                compressedStream.Close()

                Dim compressedValues As Byte() = memoryStream.ToArray()

                ' Convert the compressed byte array back to a string
                result = System.Convert.ToBase64String(compressedValues)

                memoryStream.Close()

            End Using

        End Using

        Return result

    End Function

    ''' <summary> Returns the decompressed string of the value. </summary>
    ''' <param name="value"> The string being chopped. </param>
    ''' <returns> Decompressed value. </returns>
    ''' <history date="04/09/2009" by="David" revision="1.1.3516.x"> Bug fix in getting the size.
    ''' Changed  memoryStream.Length - 5 to  memoryStream.Length - 4. </history>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")>
    Public Shared Function Decompress(ByVal value As String) As String

        If String.IsNullOrWhiteSpace(value) Then
            Return ""
            Exit Function
        End If

        Dim result As String = ""

        ' Convert the compressed string into a byte array
        Dim compressedValues As Byte() = System.Convert.FromBase64String(value)

        ' Decompress the byte array
        Using memoryStream As New IO.MemoryStream(compressedValues)

            Using compressedStream As New System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionMode.Decompress)

                ' it looks like we are getting a bogus size.
                Dim sizeBytes(3) As Byte
                memoryStream.Position = memoryStream.Length - 4
                memoryStream.Read(sizeBytes, 0, 4)

                Dim outputSize As Int32 = BitConverter.ToInt32(sizeBytes, 0)

                memoryStream.Position = 0

                Dim values(outputSize - 1) As Byte

                compressedStream.Read(values, 0, outputSize)

                ' Convert the decompressed byte array back to a string
                result = System.Text.Encoding.UTF8.GetString(values)

            End Using

        End Using

        Return result

    End Function

    ''' <summary> Query if 'value' includes any of the characters. </summary>
    ''' <param name="value">      The value. </param>
    ''' <param name="characters"> The characters. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Shared Function IncludesAny(ByVal value As String, ByVal characters As String) As Boolean
        ' 2954: changed to [characters] from ^[characters]+$
        Dim r As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(
                $"[{characters}]", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Return r.IsMatch(value)
    End Function

    ''' <summary> Query if 'value' is valid script name. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> <c>true</c> if valid script name; otherwise <c>false</c> </returns>
    Public Shared Function IsValidScriptName(ByVal value As String) As Boolean
        If String.IsNullOrWhiteSpace(value) Then
            Return False
        Else
            Return Not ScriptEntityBase.IncludesAny(value, TspSyntax.IllegalScriptNameCharacters)
        End If
    End Function

    ''' <summary> Query if 'value' is valid script file name. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> <c>true</c> if valid script name; otherwise <c>false</c> </returns>
    Public Shared Function IsValidScriptFileName(ByVal value As String) As Boolean
        If String.IsNullOrWhiteSpace(value) Then
            Return False
        Else
            Return Not ScriptEntityBase.IncludesAny(value, TspSyntax.IllegalFileCharacters)
        End If
    End Function

    ''' <summary> Returns the file size. </summary>
    ''' <param name="path"> The path. </param>
    ''' <returns> System.Int64. </returns>
    Public Shared Function FileSize(ByVal path As String) As Long
        Dim size As Long = 0
        If Not String.IsNullOrWhiteSpace(path) Then
            Dim info As System.IO.FileInfo = New System.IO.FileInfo(path)
            If info.Exists Then size = info.Length
        End If
        Return size
    End Function

#End Region

#Region " FIRMWARE "

    ''' <summary> Gets or sets the embedded firmware version. </summary>
    ''' <value> The embedded firmware version. </value>
    Public Property EmbeddedFirmwareVersion() As String

    ''' <summary> Gets or sets the released firmware version. </summary>
    ''' <value> The released firmware version. </value>
    Public Property ReleasedFirmwareVersion() As String

    ''' <summary> Gets or sets the firmware version command. </summary>
    ''' <value> The firmware version command. </value>
    Public Property FirmwareVersionQueryCommand() As String

    ''' <summary> Checks if the firmware version command exists. </summary>
    ''' <returns> <c>True</c> if the firmware version command exists; otherwise, <c>False</c>. </returns>
    Public Function FirmwareVersionQueryCommandExists(ByVal session As Vi.Pith.SessionBase) As Boolean
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Return Not session.IsNil(Me.FirmwareVersionQueryCommand.TrimEnd("()".ToCharArray))
    End Function

    ''' <summary> Checks if the firmware version command exists. </summary>
    ''' <param name="session">Specifies reference to the Tsp Session</param>
    ''' <returns> <c>True</c> if the firmware version command exists; otherwise, <c>False</c>. </returns>
    Public Function FirmwareVersionQueryCommandExists(ByVal nodeNumber As Integer, ByVal session As Vi.Pith.SessionBase) As Boolean
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Return Not session.IsNil(nodeNumber, Me.FirmwareVersionQueryCommand.TrimEnd("()".ToCharArray))
    End Function

    ''' <summary> Checks if the firmware version command exists. </summary>
    ''' <param name="node">Specifies the node.</param>
    ''' <param name="session">Specifies reference to the Tsp Session</param>
    ''' <returns> <c>True</c> if the firmware version command exists; otherwise, <c>False</c>. </returns>
    Public Function FirmwareVersionQueryCommandExists(ByVal node As NodeEntityBase, ByVal session As Vi.Pith.SessionBase) As Boolean
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        If node.IsController Then
            Return Me.FirmwareVersionQueryCommandExists(session)
        Else
            Return Me.FirmwareVersionQueryCommandExists(node.Number, session)
        End If
    End Function

    ''' <summary> Queries the embedded firmware version from a remote node and saves it to
    ''' <see cref="EmbeddedFirmwareVersion">the firmware version cache.</see> </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="session"> Specifies reference to the Tsp Session. </param>
    ''' <returns> The firmware version. </returns>
    Public Function QueryFirmwareVersion(ByVal session As Vi.Pith.SessionBase) As String
        If session Is Nothing Then            Throw New ArgumentNullException(NameOf(session))
        If Me.FirmwareVersionQueryCommandExists(session) Then
            Me._EmbeddedFirmwareVersion = session.QueryPrintTrimEnd(Me.FirmwareVersionQueryCommand)
        Else
            Me._EmbeddedFirmwareVersion = TspSyntax.NilValue
        End If
        Return Me.EmbeddedFirmwareVersion
    End Function

    ''' <summary> Queries the embedded firmware version from a remote node and saves it to
    ''' <see cref="EmbeddedFirmwareVersion">the firmware version cache.</see> </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="nodeNumber"> The node number. </param>
    ''' <param name="session">    Specifies reference to the Tsp Session. </param>
    ''' <returns> The firmware version. </returns>
    Public Function QueryFirmwareVersion(ByVal nodeNumber As Integer, ByVal session As Vi.Pith.SessionBase) As String
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        If Me.FirmwareVersionQueryCommandExists(nodeNumber, session) Then
            Me._EmbeddedFirmwareVersion = session.QueryPrintTrimEnd(nodeNumber, Me.FirmwareVersionQueryCommand)
        Else
            Me._EmbeddedFirmwareVersion = TspSyntax.NilValue
        End If
        Return Me.EmbeddedFirmwareVersion
    End Function

    ''' <summary> Queries the embedded firmware version from a remote node and saves it to
    ''' <see cref="EmbeddedFirmwareVersion">the firmware version cache.</see> </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node">    Specifies the node. </param>
    ''' <param name="session"> Specifies reference to the Tsp Session. </param>
    ''' <returns> The firmware version. </returns>
    Public Function QueryFirmwareVersion(ByVal node As NodeEntityBase, ByVal session As Vi.Pith.SessionBase) As String
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        If node.IsController Then
            Return Me.QueryFirmwareVersion(session)
        Else
            Return Me.QueryFirmwareVersion(node.Number, session)
        End If
    End Function

    ''' <summary> Validates the released against the embedded firmware. </summary>
    ''' <returns> The <see cref="FirmwareVersionStatus">version status</see>. </returns>
    Public Function ValidateFirmware() As FirmwareVersionStatus
        If String.IsNullOrWhiteSpace(Me.ReleasedFirmwareVersion) Then
            Return FirmwareVersionStatus.ReferenceUnknown
        ElseIf String.IsNullOrWhiteSpace(Me.EmbeddedFirmwareVersion) Then
            Return FirmwareVersionStatus.Unknown
        ElseIf Me.EmbeddedFirmwareVersion = TspSyntax.NilValue Then
            Return FirmwareVersionStatus.Missing
        Else
            Select Case New System.Version(Me.EmbeddedFirmwareVersion).CompareTo(New System.Version(Me.ReleasedFirmwareVersion))
                Case Is > 0
                    Return FirmwareVersionStatus.Newer
                Case 0
                    Return FirmwareVersionStatus.Current
                Case Else
                    Return FirmwareVersionStatus.Older
            End Select
        End If
    End Function

#End Region

#Region " FILE "

    Private _FileName As String

    ''' <summary> Gets or sets the filename of the file. </summary>
    ''' <value> The name of the file. </value>
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")>
    Public Property FileName() As String
        Get
            Return Me._fileName
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            Me._fileName = value
        End Set
    End Property

    ''' <summary> Gets the filename of the resource file. </summary>
    ''' <value> The filename of the resource file. </value>
    Public Property ResourceFileName As String

    ''' <summary> Gets or sets the file format which to write. Defaults to uncompressed format. </summary>
    ''' <value> The file format. </value>
    Public Property FileFormat() As ScriptFileFormats

    ''' <summary> Gets the condition indicating if the script was already saved to file. This property
    ''' is set <c>True</c> if the script source is already in the correct format so no new file needs
    ''' to be saved. </summary>
    ''' <value> The saved to file. </value>
    Public Property SavedToFile() As Boolean

    Private _SourceFormat As ScriptFileFormats

    ''' <summary> Gets the format of the contents that was used to set the source. </summary>
    ''' <value> The source format. </value>
    ReadOnly Property SourceFormat() As ScriptFileFormats
        Get
            Return Me._sourceFormat
        End Get
    End Property

#End Region

#Region " SCRIPT MANAGEMENT "

    ''' <summary> Gets or sets the condition indicating if this script was deleted. </summary>
    ''' <value> The is deleted. </value>
    Public Property IsDeleted() As Boolean

    ''' <summary> Gets or sets the condition indicating if this scripts needs to be deleted on the
    ''' instrument. At this time this is used in design mode. It might be used later for refreshing
    ''' the stored scripts. </summary>
    ''' <value> The requires deletion. </value>
    Public Property RequiresDeletion() As Boolean

    Private _RequiresReadParseWrite As Boolean
    ''' <summary> Indicates if the script requires update from file. </summary>
    ''' <returns> <c>True</c> if the script requires update from file; otherwise, <c>False</c>. </returns>
    Public Function RequiresReadParseWrite() As Boolean
        Return Me._requiresReadParseWrite
    End Function

#End Region

#Region " MODEL MANAGEMENT "

    Private _ModelMask As String

    ''' <summary> Specifies the family of instrument models for this script. </summary>
    ''' <value> The model mask. </value>
    Public ReadOnly Property ModelMask() As String
        Get
            Return Me._modelMask
        End Get
    End Property

    ''' <summary> Checks if the <paramref name="model">model</paramref> matches the
    ''' <see cref="ModelMask">mask</see>. </summary>
    ''' <param name="model"> Actual mode. </param>
    ''' <param name="mask">  Mode mask using '%' to signify ignored characters and * to specify
    ''' wildcard suffix. </param>
    ''' <returns> <c>True</c> if the <paramref name="model">model</paramref> matches the
    ''' <see cref="ModelMask">mask</see>. </returns>
    Public Shared Function IsModelMatch(ByVal model As String, ByVal mask As String) As Boolean

        Dim wildcard As Char = "*"c
        Dim ignore As Char = "%"c
        If String.IsNullOrWhiteSpace(mask) Then
            Return True
        ElseIf String.IsNullOrWhiteSpace(model) Then
            Return False
        ElseIf mask.Contains(wildcard) Then
            Dim length As Integer = mask.IndexOf(wildcard)
            Dim m As Char() = mask.Substring(0, length).ToCharArray
            Dim candidate As Char() = model.Substring(0, length).ToCharArray
            For i As Integer = 0 To m.Length - 1
                Dim c As Char = m(i)
                If c <> ignore AndAlso c <> candidate(i) Then
                    Return False
                End If
            Next
        ElseIf mask.Length <> model.Length Then
            Return False
        Else
            Dim m As Char() = mask.ToCharArray
            Dim candidate As Char() = model.ToCharArray
            For i As Integer = 0 To m.Length - 1
                Dim c As Char = m(i)
                If c <> ignore AndAlso c <> candidate(i) Then
                    Return False
                End If
            Next
        End If
        Return True
    End Function

    ''' <summary> Checks if the <paramref name="model">model</paramref> matches the
    ''' <see cref="ModelMask">mask</see>. </summary>
    ''' <param name="model"> The model. </param>
    ''' <returns> <c>True</c> if the <paramref name="model">model</paramref> matches the
    ''' <see cref="ModelMask">mask</see>. </returns>
    Public Function IsModelMatch(ByVal model As String) As Boolean
        Return ScriptEntity.IsModelMatch(model, Me.ModelMask)
    End Function

#End Region

#Region " SCRIPT SPECIFICATIONS "

    Private _IsBinaryScript As Boolean

    ''' <summary> Gets the sentinel indicating if this is a binary script. This is determined when
    ''' setting the source. </summary>
    ''' <value> <c>True</c> if this is a binary script; otherwise, <c>False</c>. </value>
    Public ReadOnly Property IsBinaryScript() As Boolean
        Get
            Return Me._isBinaryScript
        End Get
    End Property

    ''' <summary> Gets or sets the sentinel indicating if this is a Boot script. </summary>
    ''' <value> <c>True</c> if this is a Boot script; otherwise, <c>False</c>. </value>
    Public Property IsBootScript() As Boolean

    ''' <summary> Gets or sets the sentinel indicating if this is a Primary script. </summary>
    ''' <value> <c>True</c> if this is a Primary script; otherwise, <c>False</c>. </value>
    Public Property IsPrimaryScript() As Boolean

    ''' <summary> Gets or sets the sentinel indicating if this is a Support script. </summary>
    ''' <value> <c>True</c> if this is a Support script; otherwise, <c>False</c>. </value>
    Public Property IsSupportScript() As Boolean

    Private _Name As String

    ''' <summary> Gets the name of the script. </summary>
    ''' <value> The name. </value>
    Public ReadOnly Property Name() As String
        Get
            Return Me._name
        End Get
    End Property

    Private _Source As String

    ''' <summary> Gets or sets the source for the script. </summary>
    ''' <value> The source. </value>
    Public Property Source() As String
        Get
            Return Me._source
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            Me._source = ""
            Me._isBinaryScript = False
            Me._sourceFormat = ScriptFileFormats.None
            Me._requiresReadParseWrite = Me.ReleasedFirmwareVersion.Trim.StartsWith("+", True, Globalization.CultureInfo.CurrentCulture)
            Me._sourceFormat = ScriptFileFormats.None

            If Not Me.RequiresReadParseWrite Then

                If value.StartsWith(ScriptEntity.CompressedPrefix, False, Globalization.CultureInfo.CurrentCulture) Then
                    Dim fromIndex As Integer = value.IndexOf(ScriptEntity.CompressedPrefix, StringComparison.OrdinalIgnoreCase) +
                                               ScriptEntity.CompressedPrefix.Length
                    Dim toIndex As Integer = value.IndexOf(ScriptEntity.CompressedSuffix, StringComparison.OrdinalIgnoreCase) - 1
                    Me._source = value.Substring(fromIndex, toIndex - fromIndex + 1)
                    Me._source = ScriptEntityBase.Decompress(Me.Source)
                    Me._sourceFormat = Me.SourceFormat Or ScriptFileFormats.Compressed
                Else
                    Me._source = value
                End If
                If Not String.IsNullOrWhiteSpace(Me.Source) Then
                    Dim snippet As String = Me.Source.Substring(0, 50).Trim
                    Me._isBinaryScript = snippet.StartsWith("{", True, Globalization.CultureInfo.CurrentCulture) OrElse
                                         snippet.StartsWith("loadstring", True, Globalization.CultureInfo.CurrentCulture) OrElse
                                         snippet.StartsWith("loadscript", True, Globalization.CultureInfo.CurrentCulture)
                End If
                If Not Me.Source.EndsWith(" ", True, Globalization.CultureInfo.CurrentCulture) Then
                    Me._source = Me.Source.Insert(Me.Source.Length, " ")
                End If

                If Me.IsBinaryScript Then
                    Me._sourceFormat = Me.SourceFormat Or ScriptFileFormats.Binary
                End If

            End If

            ' tag file as saved if source format and file format match.
            Me._SavedToFile = Me.SourceFormat = Me.FileFormat

        End Set

    End Property

    ''' <summary> Gets or sets the timeout. </summary>
    ''' <value> The timeout. </value>
    Public Property Timeout() As TimeSpan

    ''' <summary> Namespace list setter. </summary>
    ''' <param name="value"> The value. </param>
    Private Sub NamespaceListSetter(ByVal value As String)
        If String.IsNullOrWhiteSpace(value) Then value = ""
        Me._namespaceList = value
        If String.IsNullOrWhiteSpace(value) Then
            Me._namespaces = New String() {}
        Else
            Me._namespaces = Me.NamespaceList.Split(","c)
        End If
    End Sub

    Private _NamespaceList As String

    ''' <summary> Gets or sets a list of namespaces. </summary>
    ''' <value> A List of namespaces. </value>
    Public Property NamespaceList() As String
        Get
            Return Me._namespaceList
        End Get
        Set(ByVal value As String)
            Me.namespaceListSetter(value)
        End Set
    End Property

    Private _Namespaces() As String

    ''' <summary> Gets the namespaces. </summary>
    ''' <returns> A list of. </returns>
    Public Function Namespaces() As String()
        Return Me._namespaces
    End Function

#End Region

End Class

''' <summary> A <see cref="Collections.ObjectModel.KeyedCollection">collection</see> of
''' <see cref="ScriptEntityBase">script entity</see>
''' items keyed by the <see cref="ScriptEntityBase.Name">name.</see> </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
'''  <history date="03/02/2009" by="David" revision="3.0.3348.x"> Created. </history>
Public Class ScriptEntityCollection
    Inherits ScriptEntityBaseCollection(Of ScriptEntityBase)

    ''' <summary> Gets key for item. </summary>
    ''' <param name="item"> The item. </param>
    ''' <returns> The key for item. </returns>
    Protected Overloads Overrides Function GetKeyForItem(ByVal item As ScriptEntityBase) As String
        Return MyBase.GetKeyForItem(item)
    End Function

    ''' <summary> Gets the condition indicating if scripts that are newer than the scripts specified by
    ''' the program can be deleted. This is required to allow the program install the scripts it
    ''' considers current. </summary>
    ''' <value> The allow deleting newer scripts. </value>
    Public Property AllowDeletingNewerScripts() As Boolean

End Class

''' <summary> A <see cref="Collections.ObjectModel.KeyedCollection">collection</see> of
''' <see cref="ScriptEntityBase">script entity</see>
''' items keyed by the <see cref="ScriptEntityBase.Name">name.</see> </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
'''  <history date="03/02/2009" by="David" revision="3.0.3348.x"> Created. </history>
Public Class ScriptEntityBaseCollection(Of TItem As ScriptEntityBase)
    Inherits Collections.ObjectModel.KeyedCollection(Of String, TItem)
    Implements IDisposable

#Region " I DISPOSABLE "

    ''' <summary> Performs application-defined tasks associated with freeing, releasing, or resetting
    ''' unmanaged resources. </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Private IsDisposed As Boolean
    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                For Each script As TItem In Me.Items()
                    script.Dispose()
                Next
                Me.Clear()
            End If
        Finally
            Me.IsDisposed = True
        End Try
    End Sub

#End Region

#Region " SELECT SCRIPT "

    ''' <summary> Gets key for item. </summary>
    ''' <param name="item"> The item. </param>
    ''' <returns> The key for item. </returns>
    Protected Overrides Function GetKeyForItem(ByVal item As TItem) As String
        Return item.Name & item.ModelMask
    End Function

    ''' <summary> Returns reference to the boot script for the specified node or nothing if a boot
    ''' script does not exist. </summary>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> Reference to the boot script for the specified node or nothing if a boot script does
    ''' not exist. </returns>
    Public Function SelectBootScript(ByVal node As NodeEntityBase) As TItem
        If node IsNot Nothing Then
            For Each script As TItem In Me.Items
                If script.IsModelMatch(node.ModelNumber) AndAlso script.IsBootScript Then
                    Return script
                End If
            Next
        End If
        Return Nothing
    End Function

    ''' <summary> Returns reference to the Serial Number script for the specified node or nothing if a
    ''' serial number script does not exist. </summary>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> Reference to the Serial Number script for the specified node or nothing if a serial
    ''' number script does not exist. </returns>
    Public Function SelectSerialNumberScript(ByVal node As NodeEntityBase) As TItem
        If node Is Nothing Then
            Return Nothing
        End If
        For Each script As TItem In Me.Items
            If script.IsModelMatch(node.ModelNumber) AndAlso script.IsPrimaryScript Then
                Return script
            End If
        Next
        Return Nothing
    End Function

    ''' <summary> Returns reference to the support script for the specified node or nothing if a
    ''' support script does not exist. </summary>
    ''' <param name="node"> The node. </param>
    ''' <returns> Reference to the support script for the specified node or nothing if a support script
    ''' does not exist. </returns>
    Public Function SelectSupportScript(ByVal node As NodeEntityBase) As TItem
        If node Is Nothing Then
            Return Nothing
        End If
        For Each script As TItem In Me.Items
            If script.IsModelMatch(node.ModelNumber) AndAlso script.IsSupportScript Then
                Return script
            End If
        Next
        Return Nothing
    End Function

#End Region

#Region " FIRMWARE "

    Private _IdentifiedScript As TItem

    ''' <summary> Gets any script identified using the test methods. </summary>
    ''' <value> The identified script. </value>
    Public ReadOnly Property IdentifiedScript() As TItem
        Get
            Return Me._identifiedScript
        End Get
    End Property

    Private _OutcomeDetails As System.Text.StringBuilder

    ''' <summary> Gets or sets the status message. Used with reading to identify any problem. </summary>
    ''' <value> The outcome details. </value>
    Public ReadOnly Property OutcomeDetails() As String
        Get
            Return Me._outcomeDetails.ToString
        End Get
    End Property

    ''' <summary> Reads the firmware version of all scripts. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node">    Specified the node. </param>
    ''' <param name="session"> Specifies the TSP session. </param>
    ''' <returns> <c>True</c> if okay; <c>False</c> if any exception had occurred. </returns>
    Public Function ReadFirmwareVersions(ByVal node As NodeEntityBase, ByVal session As Vi.Pith.SessionBase) As Boolean

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))

        Me._outcomeDetails = New System.Text.StringBuilder
        Me._identifiedScript = Nothing
        For Each script As TItem In Me.Items
            If script.IsModelMatch(node.ModelNumber) Then
                ' clear the embedded version.
                script.EmbeddedFirmwareVersion = ""
                If Not script.IsBootScript AndAlso Not String.IsNullOrWhiteSpace(script.Name) Then
                    If session.IsNil(node.IsController, node.Number, script.Name) Then
                        Me._identifiedScript = script
                        If Me._outcomeDetails.Length > 0 Then
                            Me._outcomeDetails.AppendLine()
                        End If
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "custom firmware '{0}' not found on '{1}' node {2}.",
                                                  script.Name, session.ResourceName, node.Number)
                    ElseIf session.IsNil(node.IsController, node.Number, script.Namespaces) Then
                        Me._identifiedScript = script
                        If Me._outcomeDetails.Length > 0 Then
                            Me._outcomeDetails.AppendLine()
                        End If
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "custom firmware '{0}' not executed on '{1}' node {2}.",
                                                  script.Name, session.ResourceName, node.Number)
                    ElseIf Not script.FirmwareVersionQueryCommandExists(node, session) Then
                        ' existing script must be out of date because it does not support the firmware version command.
                        Me._identifiedScript = script
                        If Me._outcomeDetails.Length > 0 Then
                            Me._outcomeDetails.AppendLine()
                        End If
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "custom firmware '{0}' version function not defined on '{1}' node {2}.",
                                                  script.Name, session.ResourceName, node.Number)
                    Else
                        script.QueryFirmwareVersion(node, session)
                    End If
                End If
            End If
        Next
        Return Me._outcomeDetails.Length = 0

    End Function

    ''' <summary> Returns <c>True</c> if all script versions are current. </summary>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> <c>True</c> if all script versions are current. </returns>
    Public Function AllVersionsCurrent(ByVal node As NodeEntityBase) As Boolean
        If node Is Nothing Then
            Return False
        End If
        Me._outcomeDetails = New System.Text.StringBuilder
        Me._identifiedScript = Nothing
        For Each script As TItem In Me.Items
            If script.IsModelMatch(node.ModelNumber) AndAlso Not script.IsBootScript Then
                Dim outcome As FirmwareVersionStatus = script.ValidateFirmware()
                Select Case outcome
                    Case FirmwareVersionStatus.Current
                    Case FirmwareVersionStatus.Missing
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "custom firmware '{0}' version function not defined.",
                                                     script.Name)
                    Case FirmwareVersionStatus.Newer
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "custom firmware '{0}' embedded version '{1}' is newer than the released version '{2}' indicating that this program is out-dated. You must obtain a newer version of this program.",
                                script.Name, script.EmbeddedFirmwareVersion, script.ReleasedFirmwareVersion)
                    Case FirmwareVersionStatus.Older
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "custom firmware '{0}' embedded version '{1}' is older than the released version '{2}' indicating that this script is out-dated.",
                                script.Name, script.EmbeddedFirmwareVersion, script.ReleasedFirmwareVersion)
                    Case FirmwareVersionStatus.ReferenceUnknown
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "custom firmware {0} released version not specified.",
                                                     script.Name)
                    Case FirmwareVersionStatus.Unknown
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "custom firmware {0} failed reading embedded version.",
                                                     script.Name)
                    Case Else
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "unhandled outcome reading custom firmware '{0}' version.", script.Name)
                End Select
                If outcome <> FirmwareVersionStatus.Current Then
                    Me._identifiedScript = script
                End If
            End If
        Next
        Return Me._outcomeDetails.Length = 0
    End Function

    ''' <summary> Returns <c>True</c> if any script has an unspecified version. </summary>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> <c>True</c> if any script has an unspecified version. </returns>
    Public Function VersionsUnspecified(ByVal node As NodeEntityBase) As Boolean
        If node Is Nothing Then
            Return False
        End If
        Me._outcomeDetails = New System.Text.StringBuilder
        Me._identifiedScript = Nothing
        For Each script As TItem In Me.Items
            If script.IsModelMatch(node.ModelNumber) AndAlso Not script.IsBootScript Then
                Dim outcome As FirmwareVersionStatus = script.ValidateFirmware()
                Select Case outcome
                    Case FirmwareVersionStatus.Missing
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "custom firmware '{0}' version function not defined.",
                                                     script.Name)
                    Case FirmwareVersionStatus.Unknown
                        Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "custom firmware {0} failed reading embedded version.",
                                                     script.Name)
                    Case Else
                End Select
                If outcome <> FirmwareVersionStatus.Current Then
                    Me._identifiedScript = script
                End If
            End If
        Next
        Return Me._outcomeDetails.Length > 0
    End Function

    ''' <summary> Returns <c>True</c> if any script version is newer than its released version. </summary>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> <c>True</c> if any script version is newer than its released version. </returns>
    Public Function IsProgramOutdated(ByVal node As NodeEntityBase) As Boolean
        If node Is Nothing Then
            Return False
        End If
        Me._outcomeDetails = New System.Text.StringBuilder
        Me._identifiedScript = Nothing
        For Each script As TItem In Me.Items
            If script.IsModelMatch(node.ModelNumber) AndAlso Not script.IsBootScript AndAlso script.ValidateFirmware() = FirmwareVersionStatus.Newer Then
                Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                             "custom firmware '{0}' embedded version '{1}' is newer than the released version '{2}' indicating that this program is out-dated. You must obtain a newer version of this program.",
                                             script.Name, script.EmbeddedFirmwareVersion, script.ReleasedFirmwareVersion)
                Me._identifiedScript = script
                Return True
            End If
        Next
        Return False
    End Function

#End Region

#Region " UPDATE "

    ''' <summary> Returns <c>True</c> if any script requires update from file. </summary>
    ''' <returns> <c>True</c> if any script requires update from file. </returns>
    Public Function RequiresReadParseWrite() As Boolean
        For Each script As TItem In Me.Items
            If script.RequiresReadParseWrite Then
                Return True
            End If
        Next
        Return False
    End Function

#End Region

#Region " ACTIONS "

    ''' <summary> Runs existing scripts if they did not ran so their versions can be checked. Running
    ''' exits after the first script that failed running assuming that scripts depend on previous
    ''' scripts. </summary>
    ''' <param name="node">               Specifies the node. </param>
    ''' <param name="tspScriptSubsystem"> The tsp script subsystem. </param>
    ''' <returns> <c>True</c> if okay; <c>False</c> if any exception had occurred. </returns>
    Public Function RunScripts(ByVal node As NodeEntityBase, ByVal tspScriptSubsystem As ScriptManagerBase) As Boolean
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If tspScriptSubsystem Is Nothing Then Throw New ArgumentNullException(NameOf(tspScriptSubsystem))
        Me._outcomeDetails = New System.Text.StringBuilder
        Me._identifiedScript = Nothing
        For Each script As TItem In Me.Items
            If script.IsModelMatch(node.ModelNumber) AndAlso Not script.IsBootScript AndAlso Not String.IsNullOrWhiteSpace(script.Name) Then
                If tspScriptSubsystem.Session.IsNil(node.IsController, node.Number, script.Name) Then
                    Me._identifiedScript = script
                    If Me._outcomeDetails.Length > 0 Then
                        Me._outcomeDetails.AppendLine()
                    End If
                    Me._OutcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                 "custom firmware '{0}' not found on '{1}' on node {2}.",
                                                 script.Name, tspScriptSubsystem.ResourceNameCaption, node.Number)
                    ' if script did not run, do not run subsequent scripts.
                    Return False
                ElseIf tspScriptSubsystem.Session.IsNil(node.IsController, node.Number, script.Namespaces) Then

                    tspScriptSubsystem.Session.LastAction = $"{tspScriptSubsystem.ResourceNameCaption} running {script.Name} On node {node.Number}."
                    tspScriptSubsystem.Session.LastNodeNumber = node.Number
                    ' if script not ran, run it now. Throw exception on failure.
                    tspScriptSubsystem.RunScript(script, node)

                    If tspScriptSubsystem.Session.IsNil(node.IsController, node.Number, script.Name) Then

                        Me._OutcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "Instrument '{0}' script {1} not found after running on node {2}.",
                                                     tspScriptSubsystem.ResourceNameCaption, script.Name, node.Number)

                        ' if script did not run, do not run subsequent scripts.
                        Return False
                    ElseIf tspScriptSubsystem.Session.IsNil(node.IsController, node.Number, script.Namespaces) Then

                        Me._OutcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                     "some of the namespace(s) {0} are nil after running {1} on '{2}' node {3}",
                                                     script.NamespaceList, script.Name, tspScriptSubsystem.ResourceNameCaption, node.Number)

                        ' if script did not run, do not run subsequent scripts.
                        Return False
                    End If
                End If
            End If
        Next
        Return Me._outcomeDetails.Length = 0

    End Function

    ''' <summary> Checks if all scripts exist. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node">    Specifies the node. </param>
    ''' <param name="session"> Specifies the <see cref="VI.Pith.SessionBase">TSP session.</see> </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c> if any script does not exist. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification:="Requires instrument interface method")>
    Public Function FindScripts(ByVal node As NodeEntityBase, ByVal session As Vi.Pith.SessionBase) As Boolean

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))

        Me._outcomeDetails = New System.Text.StringBuilder
        Me._identifiedScript = Nothing
        For Each script As TItem In Me.Items
            If script.IsModelMatch(node.ModelNumber) AndAlso Not String.IsNullOrWhiteSpace(script.Name) Then
                If session.IsNil(node.IsController, node.Number, script.Name) Then
                    Me._identifiedScript = script
                    If Me._outcomeDetails.Length > 0 Then
                        Me._outcomeDetails.AppendLine()
                    End If
                    Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                 "custom firmware '{0}' not found on '{1}' on node {2}.",
                                                 script.Name, session.ResourceName, node.Number)
                    ' if script not found return false
                    Return False
                End If
            End If
        Next
        Return Me._outcomeDetails.Length = 0

    End Function

    ''' <summary> Checks if all scripts were saved. </summary>
    ''' <param name="node">                 Specifies the node. </param>
    ''' <param name="tspScriptSubsystem">   The tsp script subsystem. </param>
    ''' <param name="refreshScriptCatalog"> Specifies the condition for updating the catalog of saved
    ''' scripts before checking the status of these scripts. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c> if any script does not exist. </returns>
    Public Function FindSavedScripts(ByVal node As NodeEntityBase, ByVal tspScriptSubsystem As ScriptManagerBase, ByVal refreshScriptCatalog As Boolean) As Boolean

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If tspScriptSubsystem Is Nothing Then Throw New ArgumentNullException(NameOf(tspScriptSubsystem))

        If refreshScriptCatalog Then
            tspScriptSubsystem.FetchSavedScripts()
        End If
        Me._outcomeDetails = New System.Text.StringBuilder
        Me._identifiedScript = Nothing
        For Each script As TItem In Me.Items
            If script.IsModelMatch(node.ModelNumber) AndAlso Not String.IsNullOrWhiteSpace(script.Name) Then
                If Not tspScriptSubsystem.SavedScriptExists(script.Name, node, False) Then
                    Me._identifiedScript = script
                    If Me._outcomeDetails.Length > 0 Then
                        Me._outcomeDetails.AppendLine()
                    End If
                    Me._OutcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                    "saved custom firmware '{0}' not found on '{1}' on node {2}.",
                                                    script.Name, tspScriptSubsystem.ResourceNameCaption, node.Number)
                    ' if script not found return false
                    Return False
                End If
            End If
        Next
        Return Me._outcomeDetails.Length = 0

    End Function

    ''' <summary> Checks if any script exists on the specified instrument and node. </summary>
    ''' <param name="node">    Specifies the node. </param>
    ''' <param name="session"> Specifies the <see cref="VI.Pith.SessionBase">TSP session.</see> </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c> if any exception had occurred. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
        justification:="Uses the TSP Session Methods. Not sure why this is reported as a violation.")>
    Public Function FindAnyScript(ByVal node As NodeEntityBase, ByVal session As Vi.Pith.SessionBase) As Boolean

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Me._outcomeDetails = New System.Text.StringBuilder
        Me._identifiedScript = Nothing
        For Each script As TItem In Me.Items
            If script.IsModelMatch(node.ModelNumber) AndAlso Not String.IsNullOrWhiteSpace(script.Name) Then
                If Not session.IsNil(node.IsController, node.Number, script.Name) Then
                    Me._identifiedScript = script
                    If Me._outcomeDetails.Length > 0 Then
                        Me._outcomeDetails.AppendLine()
                    End If
                    Me._outcomeDetails.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                                 "custom firmware '{0}' found on '{1}' node {2}.",
                                                 script.Name, session.ResourceName, node.Number)
                    ' if script found return true
                    Return True
                End If
            End If
        Next
        Return False

    End Function

    ''' <summary> Checks if any script exists on all nodes from the specified session. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="session">   Specifies the <see cref="VI.Pith.SessionBase">TSP session.</see> </param>
    ''' <param name="nodeEntities"> The node entities. </param>
    ''' <returns> <c>True</c> if any script exists. </returns>
    Public Function FindAnyScript(ByVal session As Vi.Pith.SessionBase, ByVal nodeEntities As NodeEntityBase()) As Boolean

        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        If nodeEntities Is Nothing Then Throw New ArgumentNullException(NameOf(nodeEntities))

        Me._outcomeDetails = New System.Text.StringBuilder
        Me._identifiedScript = Nothing
        For Each node As NodeEntityBase In nodeEntities
            If Me.FindAnyScript(node, session) Then
                Return True
            End If
        Next
        Return False

    End Function

    ''' <summary> Checks if any script exists on all nodes from the specified session. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="session">   Specifies the <see cref="VI.Pith.SessionBase">TSP session.</see> </param>
    ''' <param name="nodeEntities"> The node entities. </param>
    ''' <returns> <c>True</c> if any script exists. </returns>
    Public Function FindAnyScript(ByVal session As Vi.Pith.SessionBase, ByVal nodeEntities As NodeEntityCollection) As Boolean

        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        If nodeEntities Is Nothing Then Throw New ArgumentNullException(NameOf(nodeEntities))

        Me._outcomeDetails = New System.Text.StringBuilder
        Me._identifiedScript = Nothing
        For Each node As NodeEntityBase In nodeEntities
            If Me.FindAnyScript(node, session) Then
                Return True
            End If
        Next
        Return False

    End Function


#End Region

End Class

#Region " ENUMERATIONS "

''' <summary> Enumerates the script file formats. </summary>
<Flags()> Public Enum ScriptFileFormats
    <System.ComponentModel.Description("Uncompressed Human Readable")> None = 0
    <System.ComponentModel.Description("Binary format")> Binary = 1
    <System.ComponentModel.Description("Compressed")> Compressed = 2
End Enum

''' <summary> Enumerates the validation status.  </summary>
Public Enum FirmwareVersionStatus
    <System.ComponentModel.Description("No Specified")> None = 0
    ''' <summary> Firmware is older than expected (released) indicating that the firmware needs to be updated. </summary>
    <System.ComponentModel.Description("Version as older than expected")> Older = 1
    ''' <summary> Firmware is current.  </summary>
    <System.ComponentModel.Description("Version is current")> Current = 2
    ''' <summary> Embedded firmware is newer than expected indicating that the distributed program is out of date. </summary>
    <System.ComponentModel.Description("Version as new than expected")> Newer = 3
    ''' <summary> Embedded firmware version was not set. </summary>
    <System.ComponentModel.Description("Expected version not known (empty)")> Unknown = 4
    ''' <summary> Released firmware was not set. </summary>
    <System.ComponentModel.Description("Expected version not known (empty)")> ReferenceUnknown = 5
    ''' <summary> Version command function does not exist. </summary>
    <System.ComponentModel.Description("Version command is missing -- version is nil")> Missing = 6
End Enum

#End Region
