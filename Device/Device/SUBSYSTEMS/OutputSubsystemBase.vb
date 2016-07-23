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
        Me.TerminalsMode = OutputTerminalsMode.Front
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

#Region " TERMINALS MODE "

    ''' <summary> The output Terminals mode. </summary>
    Private _TerminalsMode As OutputTerminalsMode?

    ''' <summary> Gets or sets the cached Output Terminals mode. </summary>
    ''' <value> The Output Terminals mode or null if unknown. </value>
    Public Property TerminalsMode As OutputTerminalsMode?
        Get
            Return Me._TerminalsMode
        End Get
        Protected Set(ByVal value As OutputTerminalsMode?)
            If Not Nullable.Equals(Me.TerminalsMode, value) Then
                Me._TerminalsMode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.TerminalsMode))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Output Terminals mode. </summary>
    ''' <param name="value"> The <see cref="OutputTerminalsMode">Output Terminals mode</see>. </param>
    ''' <returns> The Output Terminals mode or null if unknown. </returns>
    Public Function ApplyTerminalsMode(ByVal value As OutputTerminalsMode) As OutputTerminalsMode?
        Me.WriteTerminalsMode(value)
        Return Me.QueryTerminalsMode()
    End Function

    ''' <summary> Gets the terminals mode query command. </summary>
    ''' <value> The terminals mode query command. </value>
    Protected Overridable ReadOnly Property TerminalsModeQueryCommand As String

    ''' <summary> Queries the Output Terminals Mode. Also sets the <see cref="TerminalsMode">output
    ''' on</see> sentinel. </summary>
    ''' <returns> The Output Terminals mode or null if unknown. </returns>
    Public Function QueryTerminalsMode() As OutputTerminalsMode?
        Me.TerminalsMode = Me.Query(Of OutputTerminalsMode)(Me.TerminalsModeQueryCommand, Me.TerminalsMode)
        Return Me.TerminalsMode
    End Function

    ''' <summary> Gets the terminals mode command format. </summary>
    ''' <value> The terminals mode command format. </value>
    Protected Overridable ReadOnly Property TerminalsModeCommandFormat As String

    ''' <summary> Writes the Output Terminals mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Terminal mode. </param>
    ''' <returns> The Output Terminals mode or null if unknown. </returns>
    Public Function WriteTerminalsMode(ByVal value As OutputTerminalsMode) As OutputTerminalsMode?
        Me.TerminalsMode = Me.Write(Of OutputTerminalsMode)(Me.TerminalsModeCommandFormat, value)
        Return Me.TerminalsMode
    End Function

#End Region

End Class

''' <summary> Specifies the output terminals mode. </summary>
Public Enum OutputTerminalsMode
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

