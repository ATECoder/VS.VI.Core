Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Source Voltage Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SourceVoltageSubsystemBase
    Inherits VI.SourceVoltageSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceVoltageSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " AUTO RANGE "

    ''' <summary> Gets or sets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = ":SOUR:VOLT:RANG:AUTO?"

    ''' <summary> Gets or sets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = ":SOUR:VOLT:RANG:AUTO {0:'ON';'ON';'OFF'}"

#End Region

#Region " LEVEL "

    ''' <summary> Gets or sets the Level query command. </summary>
    ''' <value> The Level query command. </value>
    Protected Overrides ReadOnly Property LevelQueryCommand As String = ":SOUR:VOLT?"

    ''' <summary> Gets or sets the Level command format. </summary>
    ''' <value> The Level command format. </value>
    Protected Overrides ReadOnly Property LevelCommandFormat As String = ":SOUR:VOLT {0}"

#End Region

#Region " SWEEP START LEVEL "

    ''' <summary> Gets or sets the Sweep Start Level query command. </summary>
    ''' <value> The Sweep Start Level query command. </value>
    Protected Overrides ReadOnly Property SweepStartLevelQueryCommand As String = ":SOUR:VOLT:STAR?"

    ''' <summary> Gets or sets the Sweep Start Level command format. </summary>
    ''' <value> The Sweep Start Level command format. </value>
    Protected Overrides ReadOnly Property SweepStartLevelCommandFormat As String = ":SOUR:VOLT:STAR {0}"

#End Region

#Region " SWEEP STOP LEVEL "

    ''' <summary> Gets or sets the Sweep Stop Level query command. </summary>
    ''' <value> The Sweep Stop Level query command. </value>
    Protected Overrides ReadOnly Property SweepStopLevelQueryCommand As String = ":SOUR:VOLT:STOP?"

    ''' <summary> Gets or sets the Sweep Stop Level command format. </summary>
    ''' <value> The Sweep Stop Level command format. </value>
    Protected Overrides ReadOnly Property SweepStopLevelCommandFormat As String = ":SOUR:VOLT:STOP {0}"

#End Region

#Region " SWEEP MODE "

    ''' <summary> Gets or sets the Sweep Mode  query command. </summary>
    ''' <value> The Sweep Mode  query command. </value>
    Protected Overrides ReadOnly Property SweepModeQueryCommand As String = ":SOUR:VOLT:MODE?"

    ''' <summary> Gets or sets the Sweep Mode  command format. </summary>
    ''' <value> The Sweep Mode  command format. </value>
    Protected Overrides ReadOnly Property SweepModeCommandFormat As String = ":SOUR:VOLT:MODE {0}"

#End Region

End Class
