''' <summary> Defines a Trigger Subsystem for a Keysight 34980 Meter/Scanner. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class TriggerSubsystem
    Inherits VI.Scpi.TriggerSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="TriggerSubsystem" /> class. </summary>
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
        Me.SupportedTriggerSources = TriggerSources.Bus Or TriggerSources.External Or TriggerSources.Immediate
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

#Region " ABORT / INIT COMMANDS "

    ''' <summary> Gets the Abort command. </summary>
    ''' <value> The Abort command. </value>
    Protected Overrides ReadOnly Property AbortCommand As String = ":ABOR"

    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    Protected Overrides ReadOnly Property InitiateCommand As String = ":INIT"

#End Region

#Region " AUTO DELAY "

    ''' <summary> Gets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    Protected Overrides ReadOnly Property AutoDelayEnabledCommandFormat As String = ":TRIG:DEL:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    Protected Overrides ReadOnly Property AutoDelayEnabledQueryCommand As String = ":TRIG:DEL:AUTO?"

#End Region

#Region " TRIGGER COUNT "

    ''' <summary> Gets trigger count query command. </summary>
    ''' <value> The trigger count query command. </value>
    Protected Overrides ReadOnly Property TriggerCountQueryCommand As String = ":TRIG:COUN?"

    ''' <summary> Gets trigger count command format. </summary>
    ''' <value> The trigger count command format. </value>
    Protected Overrides ReadOnly Property TriggerCountCommandFormat As String = ":TRIG:COUN {0}"

#End Region

#Region " DELAY "

    ''' <summary> Gets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    Protected Overrides ReadOnly Property DelayCommandFormat As String = ":TRIG:DEL {0:s\.FFFFFFF}"

    ''' <summary> Gets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    Protected Overrides ReadOnly Property DelayFormat As String = "s\.FFFFFFF"

    ''' <summary> Gets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    Protected Overrides ReadOnly Property DelayQueryCommand As String = ":TRIG:DEL?"

#End Region

#Region " SOURCE "

    ''' <summary> Gets the Trigger Source query command. </summary>
    ''' <value> The Trigger Source query command. </value>
    Protected Overrides ReadOnly Property TriggerSourceQueryCommand As String = ":TRIG:SOUR?"

    ''' <summary> Gets the Trigger Source command format. </summary>
    ''' <value> The Trigger Source command format. </value>
    Protected Overrides ReadOnly Property TriggerSourceCommandFormat As String = ":TRIG:SOUR {0}"

#End Region

#Region " TIMER TIME SPAN "

    ''' <summary> Gets the Timer Interval command format. </summary>
    ''' <value> The query command format. </value>
    Protected Overrides ReadOnly Property TimerIntervalCommandFormat As String = ":TRIG:TIM {0:s\.FFFFFFF}"

    ''' <summary> Gets the Timer Interval format for converting the query to time span. </summary>
    ''' <value> The Timer Interval query command. </value>
    Protected Overrides ReadOnly Property TimerIntervalFormat As String = "s\.FFFFFFF"

    ''' <summary> Gets the Timer Interval query command. </summary>
    ''' <value> The Timer Interval query command. </value>
    Protected Overrides ReadOnly Property TimerIntervalQueryCommand As String = ":TRIG:TIM?"

#End Region

#End Region

End Class
