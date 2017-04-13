''' <summary> Defines a Trigger Subsystem for a Keithley 7500 Meter. </summary>
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
        Me.AveragingEnabled = False
        Me.TriggerSource = VI.TriggerSources.Internal
        Me.SupportedTriggerSources = TriggerSources.Bus Or TriggerSources.External Or TriggerSources.Immediate Or TriggerSources.Internal
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

#Region " ABORT / INIT COMMANDS "

    ''' <summary> Gets the Abort command. </summary>
    ''' <value> The Abort command. </value>
    Protected Overrides ReadOnly Property AbortCommand As String = ":ABOR"

    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    Protected Overrides ReadOnly Property InitiateCommand As String = ":TRIG"

    ''' <summary> Gets or sets the Immediate command. </summary>
    ''' <remarks>
    ''' This command generates a trigger immediately and executes a measurement, regardless of the
    ''' setting of the trigger mode. This command Is different from :TRIG as the execution of the
    ''' object finishes when the measurement (all of the sweep) initiated with this object Is
    ''' complete. In other words, you can wait for the end of the measurement using the *OPC object.
    ''' If you execute this Object When the trigger system Is Not In the trigger wait state (trigger
    ''' Event detection state), an Error occurs When executed And the Object Is ignored.
    ''' </remarks>
    ''' <value> The Immediate command. </value>
    Protected Overrides ReadOnly Property ImmediateCommand As String = ":TRIG:SING"

#End Region

#Region " AVERAGING ENABLED "

    ''' <summary> Gets or sets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    Protected Overrides ReadOnly Property AveragingEnabledQueryCommand As String = ":TRIG:AVER?"

    ''' <summary> Gets or sets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    Protected Overrides ReadOnly Property AveragingEnabledCommandFormat As String = ":TRIG:AVER {0:1;1;0}"

#End Region

#Region " DELAY "

    ''' <summary> Gets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    Protected Overrides ReadOnly Property DelayCommandFormat As String = ":TRIG:EXT:DEL {0:s\.FFFFFFF}"

    ''' <summary> Gets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    Protected Overrides ReadOnly Property DelayFormat As String = "s\.FFFFFFF"

    ''' <summary> Gets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    Protected Overrides ReadOnly Property DelayQueryCommand As String = ":TRIG:EXT:DEL?"

#End Region

#Region " SOURCE "

    ''' <summary> Gets or sets the Trigger source query command. </summary>
    ''' <value> The Trigger source query command. </value>
    Protected Overrides ReadOnly Property TriggerSourceQueryCommand As String = ":TRIG:SOUR?"

    ''' <summary> Gets or sets the Trigger source command format. </summary>
    ''' <remarks> SCPI: ":TRIG:SOUR {0}". </remarks>
    ''' <value> The write Trigger source command format. </value>
    Protected Overrides ReadOnly Property TriggerSourceCommandFormat As String = ":TRIG:SOUR {0}"

#End Region

#End Region

End Class
