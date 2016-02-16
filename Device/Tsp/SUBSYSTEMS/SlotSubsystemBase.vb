''' <summary> Defines a System Subsystem for a TSP System. </summary>
''' <license> (c) 2016 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/13/2016" by="David" revision=""> Created. </history>
Public MustInherit Class SlotSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystemBase">status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal slotNumber As Integer, ByVal statusSubsystem As StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._SlotNumber = slotNumber
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

#Region " SLOT OPERATIONS "

    ''' <summary> Gets or sets the slot number. </summary>
    ''' <value> The slot number. </value>
    Public ReadOnly Property SlotNumber As Integer

#Region " EXISTS "

    Private _isSlotExists As Boolean?

    ''' <summary> Gets or sets (Protected) the Slot existence indicator. </summary>
    ''' <value> The Slot existence indicator. </value>
    Public Property IsSlotExists() As Boolean?
        Get
            Return Me._isSlotExists
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(value, Me.IsSlotExists) Then
                Me._isSlotExists = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsSlotExists))
            End If
        End Set
    End Property

    ''' <summary> Queries slot exists. </summary>
    ''' <remarks> David, 2/15/2016. </remarks>
    ''' <returns> The slot exists. </returns>
    Public Function QuerySlotExists() As Boolean?
        Me.IsSlotExists = Not Me.Session.IsNil(String.Format(TspSyntax.Slot.SubsystemNameFormat, Me.SlotNumber))
        If Not Me.IsSlotExists.GetValueOrDefault(False) Then
            Me.SupportsInterlock = False
            Me.InterlockState = 0
        End If
        Return Me.IsSlotExists
    End Function

#End Region

#Region " SUPPORTS INTERLOCK "

    Private _SupportsInterlock As Boolean?

    ''' <summary> Gets or sets the supports interlock. </summary>
    ''' <value> The supports interlock. </value>
    Public Property SupportsInterlock() As Boolean?
        Get
            Return Me._SupportsInterlock
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(value, Me.SupportsInterlock) Then
                Me._SupportsInterlock = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SupportsInterlock))
            End If
        End Set
    End Property

    ''' <summary> Queries supports interlock. </summary>
    ''' <remarks> David, 2/15/2016. </remarks>
    ''' <returns> The supports interlock. </returns>
    Public Function QuerySupportsInterlock() As Boolean?
        If Not Me.IsSlotExists.HasValue Then
            Me.QuerySlotExists()
        End If
        If Me.IsSlotExists.GetValueOrDefault(False) Then
            Me.SupportsInterlock = Not Me.Session.IsNil(TspSyntax.Slot.InterlockStateFormat, Me.SlotNumber)
        End If
        Return Me.SupportsInterlock
    End Function

#End Region

#Region " INTERLOCAK STATE "

    Private _InterlockState As Integer?

    ''' <summary> Gets or sets the state of the interlock. </summary>
    ''' <value> The interlock state. </value>
    Public Property InterlockState() As Integer?
        Get
            Return Me._InterlockState
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Integer?.Equals(value, Me.InterlockState) Then
                Me._InterlockState = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.InterlockState))
            End If
        End Set
    End Property

    ''' <summary> Queries interlock state. </summary>
    ''' <remarks> David, 2/15/2016. </remarks>
    ''' <returns> The interlock state. </returns>
    Public Function QueryInterlockState() As Integer?
        If Not Me.IsSlotExists.HasValue Then
            Me.QuerySlotExists()
        End If
        If Me.IsSlotExists.GetValueOrDefault(False) AndAlso Me.QuerySupportsInterlock.GetValueOrDefault(False) Then
            Me.InterlockState = Me.Query(Me.InterlockState, String.Format(TspSyntax.Slot.PrintInterlockStateFormat, Me.SlotNumber))
            ' Me.InterlockState = Me.Query(Me.InterlockState, $"_G.print(string.format('%d',{String.Format(TspSyntax.Slot.InterlockStateFormat, Me.SlotNumber)}))")
        End If
        Return Me.InterlockState
    End Function

    ''' <summary> Query if 'interlockNumber' is interlock engaged. </summary>
    ''' <remarks> David, 2/15/2016. </remarks>
    ''' <param name="interlockNumber"> The interlock number. </param>
    ''' <returns> <c>true</c> if interlock engaged; otherwise <c>false</c> </returns>
    Public Function IsInterlockEngaged(ByVal interlockNumber As Integer) As Boolean
        If Me.SupportsInterlock.GetValueOrDefault(False) Then
            If Not Me.InterlockState.HasValue Then
                Me.QueryInterlockState()
            End If
            Return (Me.InterlockState.GetValueOrDefault(0) And interlockNumber) = interlockNumber
        Else
            Return True
        End If
    End Function

#End Region

#End Region



End Class
