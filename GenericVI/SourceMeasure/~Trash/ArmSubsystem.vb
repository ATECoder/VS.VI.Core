''' <summary> Defines a SCPI Arm Subsystem for a generic Source Measure instrument. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class ArmSubsystem
    Inherits VI.Scpi.ArmSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ArmSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Clears Error Queue.
    '''           </para></remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.SupportedArmSources = VI.ArmSources.Bus Or VI.ArmSources.External Or VI.ArmSources.Immediate Or
                                 VI.ArmSources.Manual Or VI.ArmSources.StartTestBoth Or VI.ArmSources.StartTestHigh Or
                                 VI.ArmSources.StartTestLow Or VI.ArmSources.Timer Or VI.ArmSources.TriggerLink
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

#Region " ARM COUNT "

    ''' <summary> Gets Arm count query command. </summary>
    ''' <value> The Arm count query command. </value>
    Protected Overrides ReadOnly Property ArmCountQueryCommand As String = ":ARM:COUN?"

    ''' <summary> Gets Arm count command format. </summary>
    ''' <value> The Arm count command format. </value>
    Protected Overrides ReadOnly Property ArmCountCommandFormat As String = ":ARM:COUN {0}"

#End Region

#Region " DIRECTION (BYPASS) "

    ''' <summary> Gets the Trigger Direction query command. </summary>
    ''' <value> The Trigger Direction query command. </value>
    Protected Overrides ReadOnly Property DirectionQueryCommand As String = ":ARM:DIR?"

    ''' <summary> Gets the Trigger Direction command format. </summary>
    ''' <value> The Trigger Direction command format. </value>
    Protected Overrides ReadOnly Property DirectionCommandFormat As String = ":ARM:DIR {0}"

#End Region

#Region " ARM SOURCE "

    ''' <summary> Gets the Arm source command format. </summary>
    ''' <value> The write Arm source command format. </value>
    Protected Overrides ReadOnly Property ArmSourceCommandFormat As String = ":ARM:SOUR {0}"

    ''' <summary> Gets the Arm source query command. </summary>
    ''' <value> The Arm source query command. </value>
    Protected Overrides ReadOnly Property ArmSourceQueryCommand As String = ":ARM:SOUR?"

#End Region

#Region " AUTO DELAY "

    ''' <summary> Gets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    Protected Overrides ReadOnly Property AutoDelayEnabledCommandFormat As String = ""

    ''' <summary> Gets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    Protected Overrides ReadOnly Property AutoDelayEnabledQueryCommand As String = ""

#End Region

#Region " DELAY "

    ''' <summary> Gets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    Protected Overrides ReadOnly Property DelayCommandFormat As String = ""

    ''' <summary> Gets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    Protected Overrides ReadOnly Property DelayFormat As String = ""

    ''' <summary> Gets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    Protected Overrides ReadOnly Property DelayQueryCommand As String = ""

#End Region

#Region " INPUT LINE NUMBER "

    ''' <summary> Gets the Input Line Number command format. </summary>
    ''' <value> The Input Line Number command format. </value>
    Protected Overrides ReadOnly Property InputLineNumberCommandFormat As String = ":ARM:ILIN {0}"

    ''' <summary> Gets the Input Line Number query command. </summary>
    ''' <value> The Input Line Number query command. </value>
    Protected Overrides ReadOnly Property InputLineNumberQueryCommand As String = ":ARM:ILIN?"

#End Region

#Region " OUTPUT LINE NUMBER "

    ''' <summary> Gets the Output Line Number command format. </summary>
    ''' <value> The Output Line Number command format. </value>
    Protected Overrides ReadOnly Property OutputLineNumberCommandFormat As String = ":ARM:OLIN {0}"

    ''' <summary> Gets the Output Line Number query command. </summary>
    ''' <value> The Output Line Number query command. </value>
    Protected Overrides ReadOnly Property OutputLineNumberQueryCommand As String = ":ARM:OLIN?"

#End Region

#Region " TIMER TIME SPAN "

    ''' <summary> Gets the Timer Interval command format. </summary>
    ''' <value> The query command format. </value>
    Protected Overrides ReadOnly Property TimerIntervalCommandFormat As String = ":ARM:TIM {0:s\.fff}"

    ''' <summary> Gets the Timer Interval format for converting the query to time span. </summary>
    ''' <value> The Timer Interval query command. </value>
    Protected Overrides ReadOnly Property TimerIntervalFormat As String = "s\.fff"

    ''' <summary> Gets the Timer Interval query command. </summary>
    ''' <value> The Timer Interval query command. </value>
    Protected Overrides ReadOnly Property TimerIntervalQueryCommand As String = ":ARM:TIM?"

#End Region

#End Region

End Class
