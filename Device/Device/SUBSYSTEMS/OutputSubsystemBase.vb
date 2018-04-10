''' <summary> Defines the contract that must be implemented by an Output Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class OutputSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="OutputSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.OutputOnState = False
    End Sub

#End Region

#Region " ON/OFF STATE "

    ''' <summary> State of the output on. </summary>
    Private _OutputOnState As Boolean?

    ''' <summary> Gets or sets the cached output on/off state. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property OutputOnState As Boolean?
        Get
            Return Me._OutputOnState
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.OutputOnState, value) Then
                Me._OutputOnState = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the output on/off state. </summary>
    ''' <param name="value"> if set to <c>True</c> if turning on; False if turning output off. </param>
    ''' <returns> <c>True</c> if on; otherwise <c>False</c>. </returns>
    Public Function ApplyOutputOnState(ByVal value As Boolean) As Boolean?
        Me.WriteOutputOnState(value)
        Return Me.QueryOutputOnState()
    End Function

    ''' <summary> Gets or sets the output on state query command. </summary>
    ''' <value> The output on state query command. </value>
    Protected Overridable ReadOnly Property OutputOnStateQueryCommand As String

    ''' <summary> Queries the output on/off state. Also sets the <see cref="OutputOnState">output
    ''' on</see> sentinel. </summary>
    ''' <returns> <c>True</c> if on; otherwise <c>False</c>. </returns>
    Public Overridable Function QueryOutputOnState() As Boolean?
        Me.OutputOnState = Me.StatusSubsystem.Query(Me.OutputOnState.GetValueOrDefault(True), Me.OutputOnStateQueryCommand)
        Return Me.OutputOnState
    End Function

    ''' <summary> Gets or sets the output on state command format. </summary>
    ''' <value> The output on state command format. </value>
    Protected Overridable ReadOnly Property OutputOnStateCommandFormat As String

    ''' <summary> Writes the output on/off state. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> [is on]. </param>
    ''' <returns> <c>True</c> if on; otherwise <c>False</c>. </returns>
    Public Overridable Function WriteOutputOnState(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine(Me.OutputOnStateQueryCommand, CType(value, Integer))
        Me.OutputOnState = value
        Return Me.OutputOnState
    End Function

#End Region

End Class
