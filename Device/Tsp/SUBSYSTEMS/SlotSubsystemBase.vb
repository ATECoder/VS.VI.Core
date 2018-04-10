Imports isr.VI.Tsp
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
    Protected Sub New(ByVal slotNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._SlotNumber = slotNumber
        Me._EnumerateInterlocks(2)
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

#Region " SLOT NUMBER "

    ''' <summary> Gets or sets the slot number. </summary>
    ''' <value> The slot number. </value>
    Public ReadOnly Property SlotNumber As Integer

#End Region

#Region " EXISTS "

    Private _IsSlotExists As Boolean?

    ''' <summary> Gets or sets (Protected) the Slot existence indicator. </summary>
    ''' <value> The Slot existence indicator. </value>
    Public Property IsSlotExists() As Boolean?
        Get
            Return Me._isSlotExists
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(value, Me.IsSlotExists) Then
                Me._isSlotExists = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries slot exists. </summary>
    ''' <returns> The slot exists. </returns>
    Public Function QuerySlotExists() As Boolean?
        Me.IsSlotExists = Not Me.Session.IsNil(String.Format(TspSyntax.Slot.SubsystemNameFormat, Me.SlotNumber))
        If Not Me.IsSlotExists.GetValueOrDefault(False) Then
            Me.SupportsInterlock = False
            Me.InterlocksState = 0
        End If
        Return Me.IsSlotExists
    End Function

#End Region

#Region " INTERLOCKS "

    ''' <summary> Gets or sets the interlocks. </summary>
    ''' <value> The interlocks. </value>
    Public ReadOnly Property Interlocks As InterlockCollection

    ''' <summary> Enumerate interlocks. </summary>
    ''' <param name="interlockCount"> Number of interlocks. </param>
    Private Sub _EnumerateInterlocks(ByVal interlockCount As Integer)
        Me._Interlocks = New InterlockCollection
        For i As Integer = 1 To interlockCount
            Me._Interlocks.Add(i)
        Next
    End Sub

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
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries supports interlock. </summary>
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

    Private _InterlocksState As Integer?

    ''' <summary> Gets or sets the state of the interlocks. </summary>
    ''' <value> The interlock state. </value>
    Public Property InterlocksState() As Integer?
        Get
            Return Me._InterlocksState
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Integer?.Equals(value, Me.InterlocksState) Then
                Me._InterlocksState = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries interlocks state. </summary>
    ''' <returns> The interlock state. </returns>
    Public Function QueryInterlocksState() As Integer?
        If Not Me.IsSlotExists.HasValue Then Me.QuerySlotExists()
        If Not Me.SupportsInterlock.HasValue Then Me.QuerySupportsInterlock()
        If Me.IsSlotExists.GetValueOrDefault(False) AndAlso Me.QuerySupportsInterlock.GetValueOrDefault(False) Then
            Me.InterlocksState = Me.Query(Me.InterlocksState, String.Format(TspSyntax.Slot.PrintInterlockStateFormat, Me.SlotNumber))
            Me.Interlocks.UpdateInterlockState(Me.InterlocksState.GetValueOrDefault(0))
        End If
        Return Me.InterlocksState
    End Function

    ''' <summary> Query if 'interlockNumber' is interlock engaged. </summary>
    ''' <param name="interlockNumber"> The interlock number. </param>
    ''' <returns> <c>true</c> if interlock engaged; otherwise <c>false</c> </returns>
    Public Function IsInterlockEngaged(ByVal interlockNumber As Integer) As Boolean
        If Me.SupportsInterlock.GetValueOrDefault(False) Then
            If Not Me.InterlocksState.HasValue Then
                Me.QueryInterlocksState()
            End If
            Return (Me.InterlocksState.GetValueOrDefault(0) And interlockNumber) = interlockNumber
        Else
            Return True
        End If
    End Function

#End Region
#End Region

End Class

