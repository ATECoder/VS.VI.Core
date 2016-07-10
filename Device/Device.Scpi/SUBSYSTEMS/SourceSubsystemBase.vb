''' <summary> Defines the contract that must be implemented by a SCPI Source Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SourceSubsystemBase
    Inherits VI.SourceSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " SYNTAX "

#Region " AUTO CLEAR ENABLED "

    ''' <summary> Gets the automatic Clear enabled query command. </summary>
    ''' <value> The automatic Clear enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR:CLE:AUTO?" </remarks>
    Protected Overrides ReadOnly Property AutoClearEnabledQueryCommand As String = ":SOUR:CLE:AUTO?"

    ''' <summary> Gets or sets the automatic Clear enabled command Format. </summary>
    ''' <value> The automatic Clear enabled query command. </value>
    Protected Overrides ReadOnly Property AutoClearEnabledCommandFormat As String = ":SOUR:CLE:AUTO {0:'ON';'ON';'OFF'}"

#End Region

#Region " AUTO DELAY ENABLED "

    ''' <summary> Gets the automatic Delay enabled query command. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR:CLE:AUTO?" </remarks>
    Protected Overrides ReadOnly Property AutoDelayEnabledQueryCommand As String = ":SOUR:DEL:AUTO?"

    ''' <summary> Gets or sets the automatic Delay enabled command Format. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    Protected Overrides ReadOnly Property AutoDelayEnabledCommandFormat As String = ":SOUR:DEL:AUTO {0:'ON';'ON';'OFF'}"

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command, e.g., :SOUR:FUNC? </value>
    Protected Overrides ReadOnly Property FunctionModeQueryCommand As String = ":SOUR:FUNC?"

    ''' <summary> Gets or sets the function mode command. </summary>
    ''' <value> The function mode command, e.g., :SOUR:FUNC {0}. </value>
    Protected Overrides ReadOnly Property FunctionModeCommandFormat As String = ":SOUR:FUNC {0}"

#End Region

#Region " SWEEP POINTS "

    ''' <summary> Gets or sets Sweep Points query command. </summary>
    ''' <value> The Sweep Points query command. </value>
    Protected Overrides ReadOnly Property SweepPointsQueryCommand As String = ":SOUR:SWE:POIN?"

    ''' <summary> Gets or sets Sweep Points command format. </summary>
    ''' <value> The Sweep Points command format. </value>
    Protected Overrides ReadOnly Property SweepPointsCommandFormat As String = ":SOUR:SWE:POIN {0}"

#End Region

#End Region

End Class
