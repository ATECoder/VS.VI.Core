''' <summary> Defines a SCPI Sense Voltage Subsystem for a generic Source Measure instrument. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class SenseVoltageSubsystem
    Inherits VI.Scpi.SenseVoltageSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseVoltageSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    ''' <remarks> David, 6/27/2016. </remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.01, 10)
        Me.ValueRange1 = New isr.Core.Pith.RangeR(0.001, 10)
        Dim model As String = Me.StatusSubsystem.VersionInfo.Model
        Select Case True
            Case model.StartsWith("2400", StringComparison.OrdinalIgnoreCase)
                Me.ValueRange1 = New isr.Core.Pith.RangeR(0, 210)
            Case model.StartsWith("2410", StringComparison.OrdinalIgnoreCase)
                Me.ValueRange1 = New isr.Core.Pith.RangeR(0, 1100)
            Case model.StartsWith("242", StringComparison.OrdinalIgnoreCase)
                Me.ValueRange1 = New isr.Core.Pith.RangeR(0, 63)
            Case model.StartsWith("243", StringComparison.OrdinalIgnoreCase)
                Me.ValueRange1 = New isr.Core.Pith.RangeR(0, 63)
            Case model.StartsWith("244", StringComparison.OrdinalIgnoreCase)
                Me.ValueRange1 = New isr.Core.Pith.RangeR(0, 42)
            Case Else
                Me.ValueRange1 = New isr.Core.Pith.RangeR(0, 210)
        End Select
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.AsyncNotifyPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " AUTO RANGE "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = ":SENS:VOLT:RANG:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = ":SENS:VOLT:RANG:AUTO?"

#End Region

#Region " POWER LINE CYCLES "

    ''' <summary> Gets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesCommandFormat As String = ":SENS:VOLT:NPLC {0}"

    ''' <summary> Gets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesQueryCommand As String = ":SENS:VOLT:NPLC?"

#End Region

#Region " PROTECTION "

    ''' <summary> Gets the Protection enabled command Format. </summary>
    ''' <value> The Protection enabled query command. </value>
    Protected Overrides ReadOnly Property ProtectionEnabledCommandFormat As String = ":SENS:VOLT:PROT:STAT {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the Protection enabled query command. </summary>
    ''' <value> The Protection enabled query command. </value>
    Protected Overrides ReadOnly Property ProtectionEnabledQueryCommand As String = ":SENS:VOLT:PROT:STAT?"

#End Region

#Region " PROTECTION LEVEL "

    ''' <summary> Gets the protection level command format. </summary>
    ''' <value> the protection level command format. </value>
    Protected Overrides ReadOnly Property ProtectionLevelCommandFormat As String = ":SENS:VOLT:PROT {0}"

    ''' <summary> Gets the protection level query command. </summary>
    ''' <value> the protection level query command. </value>
    Protected Overrides ReadOnly Property ProtectionLevelQueryCommand As String = ":SENS:VOLT:PROT?"

#End Region

#Region " RANGE "

    ''' <summary> Gets the range command format. </summary>
    ''' <value> The range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = ":SENS:VOLT:RANG {0}"

    ''' <summary> Gets the range query command. </summary>
    ''' <value> The range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = ":SENS:VOLT:RANG?"

#End Region

#End Region

End Class
