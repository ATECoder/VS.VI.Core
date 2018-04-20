''' <summary> Manages TSP SMU subsystem. </summary>
''' <license> (c) 2007 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/7/2013" by="David" revision="">                  Uses new core. </history>
''' <history date="01/28/2008" by="David" revision="2.0.2949.x">  Use .NET Framework. </history>
''' <history date="03/12/2007" by="David" revision="1.15.2627.x"> Created. </history>
Public MustInherit Class SourceMeasureUnitBase
    Inherits VI.SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SourceMeasureUnitBase" /> class. </summary>
    ''' <remarks> Note that the local node status clear command only clears the SMU status.  So, issue
    ''' a CLS and RST as necessary when adding an SMU. </remarks>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystem">TSP status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        Me.New(statusSubsystem, 0, TspSyntax.SourceMeasureUnitNumberA)
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="SourceMeasureUnitBase" /> class. </summary>
    ''' <remarks> Note that the local node status clear command only clears the SMU status.  So, issue
    ''' a CLS and RST as necessary when adding an SMU. </remarks>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystem">TSP status
    ''' Subsystem</see>. </param>
    ''' <param name="nodeNumber">      Specifies the node number. </param>
    ''' <param name="smuNumber">       Specifies the SMU (either 'a' or 'b'. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase, ByVal nodeNumber As Integer, ByVal smuNumber As String)
        MyBase.New(statusSubsystem)
        Me._NodeNumber = nodeNumber
        Me.UnitNumber = smuNumber
        Me._SourceMeasureBasedSubsystems = New SourceMeasureUnitSubsystemCollection With {
            .NodeNumber = (nodeNumber),
            .UnitNumber = smuNumber
            }
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " SMU NODE "

    ''' <summary> Queries the <see cref="SourceMeasureUnitReference">source measure unit</see> exists. </summary>
    ''' <returns> <c>True</c> if the source measure unit exists; otherwise <c>False</c> </returns>
    Public Function SourceMeasureUnitExists() As Boolean
        Me.Session.MakeTrueFalseReplyIfEmpty(False)
        Return Not Me.Session.IsNil(Me.SourceMeasureUnitReference)
    End Function

    Private _LocalNodeNumber As Integer

    ''' <summary> Gets or sets the local node number. </summary>
    ''' <value> The local node number. </value>
    Public Property LocalNodeNumber As Integer
        Get
            Return Me._localNodeNumber
        End Get
        Set(ByVal value As Integer)
            If Not value.Equals(Me.LocalNodeNumber) Then
                Me._localNodeNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _NodeNumber As Integer

    ''' <summary> Gets or sets the one-based node number. </summary>
    ''' <value> The node number. </value>
    Public Property NodeNumber() As Integer
        Get
            Return Me._nodeNumber
        End Get
        Set(ByVal Value As Integer)
            If Not Value.Equals(Me.NodeNumber) Then
                Me._NodeNumber = Value
                Me.SourceMeasureBasedSubsystems.NodeNumber = Value
                Me.SafePostPropertyChanged()
                If Not String.IsNullOrWhiteSpace(Me.UnitNumber) Then
                    ' update the smu reference
                    Me.UnitNumber = Me.UnitNumber
                End If
            End If
        End Set
    End Property

    Private _SourceMeasureUnitReference As String

    ''' <summary> Gets or sets the full SMU reference string, e.g., '_G.node[ 1 ].smua'. </summary>
    ''' <value> The SMU reference. </value>
    Public Property SourceMeasureUnitReference() As String
        Get
            Return Me._SourceMeasureUnitReference
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.SourceMeasureUnitReference, StringComparison.OrdinalIgnoreCase) OrElse
                Me._SourceMeasureUnitReference = value Then
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _SourceMeasureUnitName As String

    ''' <summary> Gets or sets the SMU name string, e.g., 'smua'. </summary>
    ''' <value> The SMU reference. </value>
    Public Property SourceMeasureUnitName() As String
        Get
            Return Me._SourceMeasureUnitName
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me._SourceMeasureUnitName, StringComparison.OrdinalIgnoreCase) OrElse
                Me._SourceMeasureUnitName = value Then
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the SMU Unit number. </summary>
    Private _UnitNumber As String

    ''' <summary> Gets or sets the SMU unit number (a or b). </summary>
    ''' <value> The unit number. </value>
    Public Property UnitNumber() As String
        Get
            Return Me._unitNumber
        End Get
        Set(ByVal Value As String)
            If String.IsNullOrWhiteSpace(Value) Then Value = ""
            If Not String.Equals(Value, Me.UnitNumber, StringComparison.OrdinalIgnoreCase) OrElse
                Me._UnitNumber = Value Then
                Me.SourceMeasureBasedSubsystems.UnitNumber = Value
                Me.SafePostPropertyChanged()
            End If
            If String.IsNullOrWhiteSpace(Me.UnitNumber) Then
                Me.SourceMeasureUnitName = ""
                Me.SourceMeasureUnitReference = ""
            Else
                Me.SourceMeasureUnitName = TspSyntax.BuildSmuName(Me.UnitNumber)
                If Me.NodeNumber <= 0 OrElse Me.LocalNodeNumber <= 0 Then
                    Me.SourceMeasureUnitReference = TspSyntax.BuildSmuReference(Me.UnitNumber)
                Else
                    Me.SourceMeasureUnitReference = TspSyntax.BuildSmuReference(Me.NodeNumber, Me.LocalNodeNumber, Me.UnitNumber)
                End If
            End If
        End Set
    End Property

    ''' <summary> Gets the unique key. </summary>
    ''' <value> The unique key. </value>
    Public ReadOnly Property UniqueKey As String
        Get
            Return Me.SourceMeasureUnitReference
        End Get
    End Property

    ''' <summary> Gets or sets source measure based subsystems. </summary>
    ''' <value> The source measure based subsystems. </value>
    Public ReadOnly Property SourceMeasureBasedSubsystems As SourceMeasureUnitSubsystemCollection

    ''' <summary> Adds a subsystem. </summary>
    ''' <param name="item"> The item. </param>
    Public Sub Add(ByVal item As SourceMeasureUnitBase)
        Me.SourceMeasureBasedSubsystems.Add(item)
    End Sub

    ''' <summary> Removes the subsystem described by item. </summary>
    ''' <param name="item"> The item. </param>
    Public Sub Remove(ByVal item As SourceMeasureUnitBase)
        Me.SourceMeasureBasedSubsystems.Remove(item)
    End Sub

#End Region

End Class

''' <summary> A <see cref="Collections.ObjectModel.KeyedCollection">collection</see> of
''' <see cref="SourceMeasureUnitBase">Source Measure Unit</see>
''' items keyed by the <see cref="SourceMeasureUnitBase.UniqueKey">unique key.</see> </summary>
Public Class SourceMeasureUnitCollection
    Inherits SourceMeasureUnitBaseCollection(Of SourceMeasureUnitBase)

    ''' <summary> Gets key for item. </summary>
    ''' <param name="item"> The item. </param>
    ''' <returns> The key for item. </returns>
    Protected Overloads Overrides Function GetKeyForItem(ByVal item As SourceMeasureUnitBase) As String
        Return MyBase.GetKeyForItem(item)
    End Function

End Class

''' <summary> A <see cref="Collections.ObjectModel.KeyedCollection">collection</see> of
''' <see cref="SourceMeasureUnitBase">Source Measure Unit (SMU)</see>
''' items keyed by the <see cref="SourceMeasureUnitBase.UniqueKey">unique key.</see> </summary>
Public Class SourceMeasureUnitBaseCollection(Of TItem As SourceMeasureUnitBase)
    Inherits Collections.ObjectModel.KeyedCollection(Of String, TItem)

    ''' <summary> Gets key for item. </summary>
    ''' <param name="item"> The item. </param>
    ''' <returns> The key for item. </returns>
    Protected Overrides Function GetKeyForItem(ByVal item As TItem) As String
        Return item.UniqueKey
    End Function

End Class

''' <summary> Collection of source measure unit subsystems. </summary>
Public Class SourceMeasureUnitSubsystemCollection
    Inherits Collections.ObjectModel.Collection(Of SourceMeasureUnitBase)

    Private _UnitNumber As String

    ''' <summary> Gets or sets the unit number. </summary>
    ''' <value> The unit number. </value>
    Public Property UnitNumber As String
        Get
            Return Me._UnitNumber
        End Get
        Set(value As String)
            Me._UnitNumber = value
            For Each smu As SourceMeasureUnitBase In Me
                smu.UnitNumber = value
            Next
        End Set
    End Property

    Dim _nodeNumber As Integer

    ''' <summary> Gets or sets the node number. </summary>
    ''' <value> The node number. </value>
    Public Property NodeNumber As Integer
        Get
            Return Me._nodeNumber
        End Get
        Set(value As Integer)
            Me._nodeNumber = value
            For Each smu As SourceMeasureUnitBase In Me
                smu.NodeNumber = value
            Next
        End Set
    End Property

    ''' <summary>
    ''' Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
    ''' </summary>
    ''' <exception cref="ArgumentNullException">          Thrown when one or more required arguments
    '''                                                   are null. </exception>
    ''' <exception cref="T:System.NotSupportedException"> The
    '''                                                   <see cref="T:System.Collections.Generic.ICollection`1" />
    '''                                                   is read-only. </exception>
    ''' <param name="item"> The object to add to the
    '''                     <see cref="T:System.Collections.Generic.ICollection`1" />. </param>
    Public Overloads Sub Add(item As SourceMeasureUnitBase)
        If item Is Nothing Then Throw New ArgumentNullException(NameOf(item))
        item.NodeNumber = Me.NodeNumber
        item.UnitNumber = Me.UnitNumber
        MyBase.Add(item)
    End Sub

End Class

