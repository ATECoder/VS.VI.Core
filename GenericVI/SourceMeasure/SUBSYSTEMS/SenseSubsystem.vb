''' <summary> Defines a SCPI Sense Subsystem for a generic Source Measure instrument. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class SenseSubsystem
    Inherits VI.SourceMeasure.SenseSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        MyBase.SupportsMultiFunctions = True
        Me.SupportedFunctionModes = VI.SourceMeasure.SenseFunctionModes.Current Or
                                    VI.SourceMeasure.SenseFunctionModes.Voltage Or
                                    VI.SourceMeasure.SenseFunctionModes.Resistance
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.ConcurrentSenseEnabled = True
        Me.FunctionModes = VI.SourceMeasure.SenseFunctionModes.Current Or VI.SourceMeasure.SenseFunctionModes.Voltage
        Me.SupportedFunctionModes = VI.SourceMeasure.SenseFunctionModes.Current Or
                                    VI.SourceMeasure.SenseFunctionModes.Voltage Or
                                    VI.SourceMeasure.SenseFunctionModes.Resistance
        Me.ConcurrentSenseEnabled = True
        Me.Range = 0.105
        Me.PowerLineCycles = 5
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

#Region " AUTO RANGE "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = ":SENS:RANG:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = ":SENS:RANG:AUTO?"

#End Region

#Region " LATEST DATA "

    ''' <summary> Gets the latest data query command. </summary>
    ''' <value> The latest data query command. </value>
    Protected Overrides ReadOnly Property LatestDataQueryCommand As String = ":SENSE:DATA:LAT?"

#End Region

#Region " FUNCTION MODE"

    Protected Overrides ReadOnly Property FunctionModeQueryCommand As String = ":SENS:FUNC:CONC {0:'ON';'ON';'OFF'}"


    Protected Overrides ReadOnly Property FunctionModeCommandFormat As String = ":SENS:FUNC:CONC {0:'ON';'ON';'OFF'}"

#End Region

#Region " POWER LINE CYCLES "

    ''' <summary> Gets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesCommandFormat As String = ":SENS:NPLC {0}"

    ''' <summary> Gets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesQueryCommand As String = ":SENS:NPLC?"

#End Region

#Region " PROTECTION LEVEL "

    ''' <summary> Gets the protection level command format. </summary>
    ''' <value> the protection level command format. </value>
    Protected Overrides ReadOnly Property ProtectionLevelCommandFormat As String = ":SENS:PROT {0}"

    ''' <summary> Gets the protection level query command. </summary>
    ''' <value> the protection level query command. </value>
    Protected Overrides ReadOnly Property ProtectionLevelQueryCommand As String = ":SENS:PROT?"

#End Region

#End Region

End Class

