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
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.OffMode = OutputOffMode.Normal
        Me.OutputOnState = False
        Me.TerminalMode = OutputTerminalMode.Front
    End Sub

#End Region

#Region " OFF MODE "

    ''' <summary> The output off mode. </summary>
    Private _OffMode As OutputOffMode?

    ''' <summary> Gets or sets the cached output off mode. </summary>
    ''' <value> The output off mode or null if unknown. </value>
    Public Property OffMode As OutputOffMode?
        Get
            Return Me._OffMode
        End Get
        Protected Set(ByVal value As OutputOffMode?)
            If Not Nullable.Equals(Me.OffMode, value) Then
                Me._OffMode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.OffMode))
            End If
        End Set
    End Property

    ''' <summary> Queries the output Off Mode. Also sets the <see cref="OffMode">output
    ''' on</see> sentinel. </summary>
    ''' <returns> The output off mode or null if unknown. </returns>
    Public MustOverride Function QueryOffMode() As OutputOffMode?

    ''' <summary> Writes the output off mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The off mode. </param>
    ''' <returns> The output off mode or null if unknown. </returns>
    Public MustOverride Function WriteOffMode(ByVal value As OutputOffMode) As OutputOffMode?

    ''' <summary> Writes and reads back the output off mode. </summary>
    ''' <param name="value"> The <see cref="OutputOffMode">output off mode</see>. </param>
    ''' <returns> The output off mode or null if unknown. </returns>
    Public Function ApplyOffMode(ByVal value As OutputOffMode) As OutputOffMode?
        Me.WriteOffMode(value)
        Return Me.QueryOffMode()
    End Function

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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.OutputOnState))
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

    ''' <summary> Queries the output on/off state. Also sets the <see cref="OutputOnState">output
    ''' on</see> sentinel. </summary>
    ''' <returns> <c>True</c> if on; otherwise <c>False</c>. </returns>
    Public MustOverride Function QueryOutputOnState() As Boolean?

    ''' <summary> Writes the output on/off state. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> [is on]. </param>
    ''' <returns> <c>True</c> if on; otherwise <c>False</c>. </returns>
    Public MustOverride Function WriteOutputOnState(ByVal value As Boolean) As Boolean?

#End Region

#Region " TERMINAL MODE "

    ''' <summary> The Route Terminal mode. </summary>
    Private _TerminalMode As OutputTerminalMode?

    ''' <summary> Gets or sets the cached Route Terminal mode. </summary>
    ''' <value> The Route Terminal mode or null if unknown. </value>
    Public Property TerminalMode As OutputTerminalMode?
        Get
            Return Me._TerminalMode
        End Get
        Protected Set(ByVal value As OutputTerminalMode?)
            If Not Nullable.Equals(Me.TerminalMode, value) Then
                Me._TerminalMode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.TerminalMode))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Route Terminal mode. </summary>
    ''' <param name="value"> The <see cref="RouteTerminalMode">Route Terminal mode</see>. </param>
    ''' <returns> The Route Terminal mode or null if unknown. </returns>
    Public Function ApplyTerminalMode(ByVal value As OutputTerminalMode) As OutputTerminalMode?
        Me.WriteTerminalMode(value)
        Return Me.QueryTerminalMode()
    End Function

    ''' <summary> Gets the terminal mode query command. </summary>
    ''' <value> The terminal mode query command. </value>
    Protected Overridable ReadOnly Property TerminalModeQueryCommand As String

    ''' <summary> Queries the Route Terminal Mode. Also sets the <see cref="TerminalMode">output
    ''' on</see> sentinel. </summary>
    ''' <returns> The Route Terminal mode or null if unknown. </returns>
    Public Function QueryTerminalMode() As OutputTerminalMode?
        Me.TerminalMode = Me.Query(Of OutputTerminalMode)(Me.TerminalModeQueryCommand, Me.TerminalMode)
        Return Me.TerminalMode
    End Function

    ''' <summary> Gets the terminal mode command format. </summary>
    ''' <value> The terminal mode command format. </value>
    Protected Overridable ReadOnly Property TerminalModeCommandFormat As String

    ''' <summary> Writes the Route Terminal mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Terminal mode. </param>
    ''' <returns> The Route Terminal mode or null if unknown. </returns>
    Public Function WriteTerminalMode(ByVal value As OutputTerminalMode) As OutputTerminalMode?
        Me.TerminalMode = Me.Write(Of OutputTerminalMode)(Me.TerminalModeCommandFormat, value)
        Return Me.TerminalMode
    End Function

#End Region

End Class

''' <summary> Specifies the output terminal mode. </summary>
Public Enum OutputTerminalMode
    <ComponentModel.Description("Not set")> None
    <ComponentModel.Description("Front (FRON)")> Front
    <ComponentModel.Description("Rear (REAR)")> Rear
End Enum

''' <summary> Specifies the output off mode. </summary>
Public Enum OutputOffMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Guard (GUAR)")> Guard
    <ComponentModel.Description("High Impedance (HIMP)")> HighImpedance
    <ComponentModel.Description("Normal (NORM)")> Normal
    <ComponentModel.Description("Zero (ZERO)")> Zero
End Enum

