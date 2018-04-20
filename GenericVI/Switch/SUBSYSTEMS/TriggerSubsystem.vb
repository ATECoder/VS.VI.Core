''' <summary> Defines a Trigger Subsystem for a Keithley 2700 instrument. </summary>
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

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="TriggerSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " COMMANDS "

    ''' <summary> Gets the Abort command. </summary>
    ''' <value> The Abort command. </value>
    Protected Overrides ReadOnly Property AbortCommand As String = ":ABOR"

    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    Protected Overrides ReadOnly Property InitiateCommand As String = ":INIT"

    ''' <summary> Gets or sets the Immediate command. </summary>
    ''' <value> The Immediate command. </value>
    Protected Overrides ReadOnly Property ImmediateCommand As String = ":TRIG:IMM"

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

#Region " DIRECTION (BYPASS) "

    ''' <summary> Gets the Trigger Direction query command. </summary>
    ''' <value> The Trigger Direction query command. </value>
    Protected Overrides ReadOnly Property DirectionQueryCommand As String = ":TRIG:DIR?"

    ''' <summary> Gets the Trigger Direction command format. </summary>
    ''' <value> The Trigger Direction command format. </value>
    Protected Overrides ReadOnly Property DirectionCommandFormat As String = ":TRIG:DIR {0}"

#End Region

#Region " INPUT LINE NUMBER "

    ''' <summary> Gets the Input Line Number command format. </summary>
    ''' <value> The Input Line Number command format. </value>
    Protected Overrides ReadOnly Property InputLineNumberCommandFormat As String = ":TRIG:ILIN {0}"

    ''' <summary> Gets the Input Line Number query command. </summary>
    ''' <value> The Input Line Number query command. </value>
    Protected Overrides ReadOnly Property InputLineNumberQueryCommand As String = ":TRIG:ILIN?"

#End Region

#Region " OUTPUT LINE NUMBER "

    ''' <summary> Gets the Output Line Number command format. </summary>
    ''' <value> The Output Line Number command format. </value>
    Protected Overrides ReadOnly Property OutputLineNumberCommandFormat As String = ":TRIG:OLIN {0}"

    ''' <summary> Gets the Output Line Number query command. </summary>
    ''' <value> The Output Line Number query command. </value>
    Protected Overrides ReadOnly Property OutputLineNumberQueryCommand As String = ":TRIG:OLIN?"

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
