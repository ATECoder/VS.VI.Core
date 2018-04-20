
''' <summary> Defines the contract that must be implemented by a SCPI Route Subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created. </history>
Public MustInherit Class RouteSubsystemBase
    Inherits VI.RouteSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="RouteSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.TerminalsMode = RouteTerminalsMode.Front
    End Sub

#End Region

#Region " TERMINALS MODE "

    ''' <summary> The Route Terminals mode. </summary>
    Private _TerminalsMode As RouteTerminalsMode?

    ''' <summary> Gets or sets the cached Route Terminals mode. </summary>
    ''' <value> The Route Terminals mode or null if unknown. </value>
    Public Property TerminalsMode As RouteTerminalsMode?
        Get
            Return Me._TerminalsMode
        End Get
        Protected Set(ByVal value As RouteTerminalsMode?)
            If Not Nullable.Equals(Me.TerminalsMode, value) Then
                Me._TerminalsMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Route Terminals mode. </summary>
    ''' <param name="value"> The <see cref="RouteTerminalsMode">Route Terminals mode</see>. </param>
    ''' <returns> The Route Terminals mode or null if unknown. </returns>
    Public Function ApplyTerminalsMode(ByVal value As RouteTerminalsMode) As RouteTerminalsMode?
        Me.WriteTerminalsMode(value)
        Return Me.QueryTerminalsMode()
    End Function

    ''' <summary> Gets the terminals mode query command. </summary>
    ''' <value> The terminals mode command. </value>
    Protected Overridable ReadOnly Property TerminalsModeQueryCommand As String

    ''' <summary> Queries the Route Terminals Mode. Also sets the <see cref="TerminalsMode">output
    ''' on</see> sentinel. </summary>
    ''' <returns> The Route Terminals mode or null if unknown. </returns>
    Public Function QueryTerminalsMode() As RouteTerminalsMode?
        Me.TerminalsMode = Me.Query(Of RouteTerminalsMode)(Me.TerminalsModeQueryCommand, Me.TerminalsMode)
        Return Me.TerminalsMode
    End Function

    ''' <summary> Gets the terminals mode command format. </summary>
    ''' <value> The terminals mode command format. </value>
    Protected Overridable ReadOnly Property TerminalsModeCommandFormat As String

    ''' <summary> Writes the Route Terminals mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Terminal mode. </param>
    ''' <returns> The Route Terminals mode or null if unknown. </returns>
    Public Function WriteTerminalsMode(ByVal value As RouteTerminalsMode) As RouteTerminalsMode?
        Me.TerminalsMode = Me.Write(Of RouteTerminalsMode)(Me.TerminalsModeCommandFormat, value)
        Return Me.TerminalsMode
    End Function

#End Region

End Class

''' <summary> Specifies the route terminals mode. </summary>
Public Enum RouteTerminalsMode
    <ComponentModel.Description("Not set")> None
    <ComponentModel.Description("Front (FRON)")> Front
    <ComponentModel.Description("Rear (REAR)")> Rear
End Enum
