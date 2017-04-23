''' <summary> Provides an contract for a Node Entity. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="03/02/2009" by="David" revision="3.0.3348.x"> Created. </history>
Public MustInherit Class NodeEntityBase

#Region " CONSTRUCTORS "

    ''' <summary> Constructs the class. </summary>
    ''' <param name="number">               Specifies the node number. </param>
    ''' <param name="controllerNodeNumber"> The controller node number. </param>
    Protected Sub New(ByVal number As Integer, ByVal controllerNodeNumber As Integer)
        MyBase.New()
        Me._controllerNodeNumber = controllerNodeNumber
        Me._number = number
        Me._uniqueKey = NodeEntityBase.BuildKey(number)
        Me._modelNumber = ""
        Me._instrumentModelFamily = Tsp.InstrumentModelFamily.None
        Me._FirmwareVersion = ""
        Me._SerialNumber = ""
    End Sub

    ''' <summary> Builds a key. </summary>
    ''' <param name="nodeNumber"> The node number. </param>
    ''' <returns> The unique value for the node number. </returns>
    Public Shared Function BuildKey(ByVal nodeNumber As Integer) As String
        Return CStr(nodeNumber)
    End Function

#End Region

#Region " INIT "

    ''' <summary> Initializes the node properties . </summary>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is incorrect. </exception>
    ''' <param name="session"> The session. </param>
    Public Sub InitializeKnownState(ByVal session As SessionBase)
        Me.QueryModelNumber(session)
        If Me.InstrumentModelFamily = InstrumentModelFamily.None Then
            Throw New FormatException(String.Format(Globalization.CultureInfo.CurrentCulture,
                                                    "Node #{0} instrument model {1} has no defined instrument family;. ",
                                                    Me.Number, Me.ModelNumber))
        End If
        Me.QueryFirmwareVersion(session)
        Me.QuerySerialNumber(session)
    End Sub

#End Region

#Region " NODE INFO "

    ''' <summary> Queries if a given node exists. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="session">    The session. </param>
    ''' <param name="nodeNumber"> The node number. </param>
    ''' <returns> <c>True</c> is node exists; otherwise, <c>False</c>. </returns>
    Public Shared Function NodeExists(ByVal session As SessionBase, ByVal nodeNumber As Integer) As Boolean
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Return Not session.IsNil("node[{0}]", nodeNumber)
    End Function

    ''' <summary> <c>True</c> if boot script save required. </summary>
    Private _BootScriptSaveRequired As Boolean

    ''' <summary> Gets or sets the condition to indicate that the boot script must be re-saved because
    ''' a script reference changes as would happened if a new binary script was created with a table
    ''' reference that differs from the table reference of the previous script that was used in the
    ''' previous boot script. </summary>
    ''' <value> The boot script save required. </value>
    Public Property BootScriptSaveRequired() As Boolean
        Get
            Return Me._bootScriptSaveRequired
        End Get
        Set(ByVal value As Boolean)
            Me._bootScriptSaveRequired = value
        End Set
    End Property

    ''' <summary> The controller node number. </summary>
    Private _ControllerNodeNumber As Integer

    ''' <summary> Returns the controller node number. </summary>
    ''' <value> The controller node number. </value>
    Public ReadOnly Property ControllerNodeNumber() As Integer
        Get
            Return Me._controllerNodeNumber
        End Get
    End Property

    ''' <summary> Gets or sets the data queue capacity. </summary>
    ''' <value> The data queue capacity. </value>
    Public Property DataQueueCapacity As Integer?

    ''' <summary> Queries the data queue capacity. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="session"> The session. </param>
    Public Sub QueryDataQueueCapacity(ByVal session As SessionBase)
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Me.DataQueueCapacity = session.QueryPrint(0I, 1, "node[{0}].dataqueue.capacity", Me.Number)
    End Sub

    ''' <summary> Gets or sets the data queue Count. </summary>
    ''' <value> The data queue Count. </value>
    Public Property DataQueueCount As Integer?

    ''' <summary> Queries the data queue Count. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="session"> The session. </param>
    Public Sub QueryDataQueueCount(ByVal session As SessionBase)
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Me.DataQueueCount = session.QueryPrint(0I, 1, "node[{0}].dataqueue.count", Me.Number)
    End Sub

    ''' <summary> Gets or sets the firmware version. </summary>
    ''' <value> The firmware version. </value>
    Public Property FirmwareVersion() As String

    ''' <summary> Queries firmware version. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="session"> The session. </param>
    Public Sub QueryFirmwareVersion(ByVal session As SessionBase)
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Me.FirmwareVersion = session.QueryPrintTrimEnd("node[{0}].revision", Me.Number)
    End Sub

    ''' <summary> The instrument model family. </summary>
    Private _InstrumentModelFamily As InstrumentModelFamily

    ''' <summary> Gets the <see cref="InstrumentModelFamily">instrument model family.</see> </summary>
    ''' <value> The instrument model family. </value>
    ReadOnly Property InstrumentModelFamily() As InstrumentModelFamily
        Get
            Return Me._instrumentModelFamily
        End Get
    End Property

    ''' <summary> Gets the is controller. </summary>
    ''' <value> The is controller. </value>
    Public ReadOnly Property IsController() As Boolean
        Get
            Return Me._number = Me.ControllerNodeNumber
        End Get
    End Property

    ''' <summary> The model number. </summary>
    Private _ModelNumber As String

    ''' <summary> Gets the model number. </summary>
    ''' <value> The model number. </value>
    Public ReadOnly Property ModelNumber() As String
        Get
            Return Me._modelNumber
        End Get
    End Property

    ''' <summary> Queries the serial number. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="session">    The session. </param>
    ''' <param name="nodeNumber"> The node number. </param>
    ''' <returns> The model number. </returns>
    Public Shared Function QueryModelNumber(ByVal session As SessionBase, ByVal nodeNumber As Integer) As String
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Return session.QueryPrintTrimEnd("print(node[{0}].model)", nodeNumber)
    End Function

    ''' <summary> Queries controller node model. </summary>
    ''' <returns> The controller node model. </returns>
    Public Shared Function QueryControllerNodeModel(ByVal session As SessionBase) As String
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Return session.QueryPrintTrimEnd("localnode.model")
    End Function

    ''' <summary> Queries the serial number. </summary>
    ''' <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ''' null. </exception>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="session"> The session. </param>
    Public Sub QueryModelNumber(ByVal session As SessionBase)
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        If Me.Number = Me.ControllerNodeNumber Then
            Me._modelNumber = NodeEntityBase.QueryControllerNodeModel(session)
        Else
            Me._modelNumber = NodeEntityBase.QueryModelNumber(session, Me.Number)
        End If
        If String.IsNullOrWhiteSpace(Me.ModelNumber) Then
            Throw New OperationFailedException("Failed reading node model--empty.")
        End If
        Me._instrumentModelFamily = NodeEntityBase.ParseModelNumber(Me.ModelNumber)
    End Sub

    ''' <summary> Gets the node number. </summary>
    ''' <value> The node number. </value>
    Public ReadOnly Property Number() As Integer

    ''' <summary> Gets or sets the serial number. </summary>
    ''' <value> The serial number. </value>
    Public Property SerialNumber() As String

    ''' <summary> Queries the serial number. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="session"> The session. </param>
    Public Sub QuerySerialNumber(ByVal session As SessionBase)
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Me.SerialNumber = session.QueryPrintTrimEnd("node[{0}].serialno", Me.Number)
    End Sub

    ''' <summary> Gets the unique key. </summary>
    ''' <value> The unique key. </value>
    Public ReadOnly Property UniqueKey() As String

#End Region

#Region " INSTRUMENT MODEL "

    ''' <summary> Returns <c>True</c> if the <paramref name="model">model</paramref> matches the mask. </summary>
    ''' <param name="model"> Actual mode. </param>
    ''' <param name="mask">  Mode mask using '%' to signify ignored characters and * to specify
    ''' wildcard suffix. </param>
    ''' <returns> Returns <c>True</c> if the <paramref name="model">model</paramref> matches the mask. </returns>
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

    ''' <summary> Parses the model number to get the model family. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> A <see cref="Tsp.InstrumentModelFamily">model family</see>. </returns>
    Public Shared Function ParseModelNumber(ByVal value As String) As Tsp.InstrumentModelFamily
        For Each item As Tsp.InstrumentModelFamily In [Enum].GetValues(GetType(Tsp.InstrumentModelFamily))
            If item <> 0 AndAlso NodeEntityBase.IsModelMatch(value, ModelFamilyMask(item)) Then
                Return item
            End If
        Next
        Return Tsp.InstrumentModelFamily.None
    End Function

    ''' <summary> Returns the mask for the model family. </summary>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The value. </param>
    ''' <returns> The mask for the model family. </returns>
    Public Shared Function ModelFamilyMask(ByVal value As Tsp.InstrumentModelFamily) As String
        Select Case value
            Case Tsp.InstrumentModelFamily.K2600
                Return "26%%"
            Case Tsp.InstrumentModelFamily.K2600A
                Return "26%%%"
            Case Tsp.InstrumentModelFamily.K3700
                Return "37*"
            Case Else
                Throw New ArgumentOutOfRangeException("value", value, "Unhandled model family")
        End Select
    End Function

    ''' <summary> Model family resource file suffix. </summary>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The value. </param>
    ''' <returns> The suffix for the model family resource file name. </returns>
    Public Shared Function ModelFamilyResourceFileSuffix(ByVal value As Tsp.InstrumentModelFamily) As String
        Select Case value
            Case Tsp.InstrumentModelFamily.K2600
                Return ".2600"
            Case Tsp.InstrumentModelFamily.K2600A
                Return ".2600A"
            Case Tsp.InstrumentModelFamily.K3700
                Return ".3700"
            Case Else
                Throw New ArgumentOutOfRangeException("value", value, "Unhandled model family")
        End Select
    End Function

#End Region


End Class

''' <summary> A <see cref="Collections.ObjectModel.KeyedCollection">collection</see> of
''' <see cref="NodeEntityBase">Node entity</see>
''' items keyed by the <see cref="NodeEntityBase.UniqueKey">unique key.</see> </summary>
Public Class NodeEntityCollection
    Inherits NodeEntityBaseCollection(Of NodeEntityBase)

    ''' <summary> Gets key for item. </summary>
    ''' <param name="item"> The item. </param>
    ''' <returns> The key for item. </returns>
    Protected Overloads Overrides Function GetKeyForItem(ByVal item As NodeEntityBase) As String
        Return MyBase.GetKeyForItem(item)
    End Function
End Class

''' <summary> A <see cref="Collections.ObjectModel.KeyedCollection">collection</see> of
''' <see cref="NodeEntityBase">Node entity</see>
''' items keyed by the <see cref="NodeEntityBase.UniqueKey">unique key.</see> </summary>
Public Class NodeEntityBaseCollection(Of TItem As NodeEntityBase)
    Inherits Collections.ObjectModel.KeyedCollection(Of String, TItem)

    ''' <summary> Gets key for item. </summary>
    ''' <param name="item"> The item. </param>
    ''' <returns> The key for item. </returns>
    Protected Overrides Function GetKeyForItem(ByVal item As TItem) As String
        Return item.UniqueKey
    End Function

End Class

''' <summary>Enumerates the instrument model families.</summary>
Public Enum InstrumentModelFamily

    ''' <summary>Not defined.</summary>
    <System.ComponentModel.Description("Not defined")> None = 0

    ''' <summary>26xx Source Meters.</summary>
    <System.ComponentModel.Description("26xx Source Meters")> K2600 = 1

    ''' <summary>26xxA Source Meters.</summary>
    <System.ComponentModel.Description("26xxA Source Meters")> K2600A = 2

    ''' <summary>37xx Switch Systems.</summary>
    <System.ComponentModel.Description("37xx Switch Systems")> K3700 = 3

End Enum

