''' <summary> Defines a SCPI Source Voltage Subsystem for a generic Source Measure instrument. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class SourceVoltageSubsystem
    Inherits VI.Scpi.SourceVoltageSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceVoltageSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
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

#Region " PROTECTION LEVEL "

    ''' <summary> Gets the protection level command format. </summary>
    ''' <value> the protection level command format. </value>
    Protected Overrides ReadOnly Property ProtectionLevelCommandFormat As String = ":SOUR:VOLT:PROT:LEV {0}"

    ''' <summary> Gets the protection level query command. </summary>
    ''' <value> the protection level query command. </value>
    Protected Overrides ReadOnly Property ProtectionLevelQueryCommand As String = ":SOUR:VOLT:PROT:LEV?"

#End Region

#Region " RANGE "

    ''' <summary> Gets the range command format. </summary>
    ''' <value> The range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = ":SOUR:VOLT:RANG {0}"

    ''' <summary> Gets the range query command. </summary>
    ''' <value> The range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = ":SOUR:VOLT:RANG?"

    ''' <summary> The Range of function values. </summary>
    Public Overrides ReadOnly Property LevelRange As Core.Pith.RangeR
        Get
            Dim model As String = Me.StatusSubsystem.VersionInfo.Model
            Select Case True
                Case model.StartsWith("2400", StringComparison.OrdinalIgnoreCase)
                    Return New isr.Core.Pith.RangeR(-210, +210)
                Case model.StartsWith("2410", StringComparison.OrdinalIgnoreCase)
                    Return New isr.Core.Pith.RangeR(-1100, +1100)
                Case model.StartsWith("2420", StringComparison.OrdinalIgnoreCase)
                    Return New isr.Core.Pith.RangeR(-63, +63)
                Case model.StartsWith("2425", StringComparison.OrdinalIgnoreCase)
                    Return New isr.Core.Pith.RangeR(-105, +105)
                Case model.StartsWith("243", StringComparison.OrdinalIgnoreCase)
                    Return New isr.Core.Pith.RangeR(-105, +105)
                Case model.StartsWith("244", StringComparison.OrdinalIgnoreCase)
                    Return New isr.Core.Pith.RangeR(-42, +42)
                Case Else
                    Return New isr.Core.Pith.RangeR(-210, +210)
            End Select
        End Get
    End Property

#End Region

#End Region

End Class
