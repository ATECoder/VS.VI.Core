Imports System.ComponentModel
''' <summary> Defines a System Subsystem for a Keithley 7500 Meter. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class SystemSubsystem
    Inherits VI.Scpi.SystemSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FanLevel = VI.FanLevel.Normal
    End Sub

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    ''' <remarks> David, 6/25/2016. </remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        ' reduce noise when using IDE.
        If Debugger.IsAttached Then Me.ApplyFanLevel(VI.FanLevel.Quiet)
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

    ''' <summary> Gets the initialize memory command. </summary>
    ''' <value> The initialize memory command. </value>
    Protected Overrides ReadOnly Property InitializeMemoryCommand As String = ""

    ''' <summary> Gets the preset command. </summary>
    ''' <value> The preset command. </value>
    Protected Overrides ReadOnly Property PresetCommand As String = ""

    ''' <summary> Gets the scpi revision query command. </summary>
    ''' <value> The scpi revision query command. </value>
    Protected Overrides ReadOnly Property ScpiRevisionQueryCommand As String = Scpi.Syntax.ScpiRevisionQueryCommand

#End Region

#Region " BEEPER IMMEDIATE "

    ''' <summary> Commands the instrument to issue a Beep on the instrument. </summary>
    ''' <param name="frequency"> Specifies the frequency of the beep. </param>
    ''' <param name="duration">  Specifies the duration of the beep. </param>
    Public Sub BeepImmediately(ByVal frequency As Integer, ByVal duration As Single)
        Me.Session.WriteLine(":SYST:BEEP:IMM {0}, {1}", frequency, duration)
    End Sub

#End Region

#Region " FAN LEVEL "

    ''' <summary> Gets the Fan Level query command. </summary>
    ''' <value> The Fan Level command. </value>
    Protected Overrides ReadOnly Property FanLevelQueryCommand As String = ""

    ''' <summary> Gets the Fan Level command format. </summary>
    ''' <value> The Fan Level command format. </value>
    Protected Overrides ReadOnly Property FanLevelCommandFormat As String = ":SYST:FAN:LEV {0}"

#End Region

End Class

