''' <summary> Defines a Calculate2 Four Wire Resistance Subsystem for a Keithley 7500 Meter. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class Calculate2FourWireResistanceSubsystem
    Inherits VI.Scpi.Calculate2SubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="Calculate2FourWireResistanceSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
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
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " LIMIT 1 "

#Region " LIMIT1 AUTO CLEAR "

    ''' <summary> Gets the Limit1 Auto Clear command Format. </summary>
    ''' <value> The Limit1 AutoClear query command. </value>
    Protected Overrides ReadOnly Property Limit1AutoClearCommandFormat As String = ":CALC2:FRES:LIM1:CLE:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the Limit1 Auto Clear query command. </summary>
    ''' <value> The Limit1 AutoClear query command. </value>
    Protected Overrides ReadOnly Property Limit1AutoClearQueryCommand As String = ":CALC2:FRES:LIM1:CLE:AUTO?"

#End Region

#Region " LIMIT1 ENABLED "

    ''' <summary> Gets the Limit1 enabled command Format. </summary>
    ''' <value> The Limit1 enabled query command. </value>
    Protected Overrides ReadOnly Property Limit1EnabledCommandFormat As String = ":CALC2:FRES:LIM1:STAT {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the Limit1 enabled query command. </summary>
    ''' <value> The Limit1 enabled query command. </value>
    Protected Overrides ReadOnly Property Limit1EnabledQueryCommand As String = ":CALC2:FRES:LIM1:STAT?"

#End Region

#Region " LIMIT1 LOWER LEVEL "

    ''' <summary> Gets the Limit1 Lower Level command format. </summary>
    ''' <value> The Limit1LowerLevel command format. </value>
    Protected Overrides ReadOnly Property Limit1LowerLevelCommandFormat As String = ":CALC2:FRES:LIM1:LOW {0}"

    ''' <summary> Gets the Limit1 Lower Level query command. </summary>
    ''' <value> The Limit1LowerLevel query command. </value>
    Protected Overrides ReadOnly Property Limit1LowerLevelQueryCommand As String = ":CALC2:FRES:LIM1:LOW?"

#End Region

#Region " Limit1 UPPER LEVEL "

    ''' <summary> Gets the Limit1 Upper Level command format. </summary>
    ''' <value> The Limit1UpperLevel command format. </value>
    Protected Overrides ReadOnly Property Limit1UpperLevelCommandFormat As String = ":CALC2:FRES:LIM1:UPP {0}"

    ''' <summary> Gets the Limit1 Upper Level query command. </summary>
    ''' <value> The Limit1UpperLevel query command. </value>
    Protected Overrides ReadOnly Property Limit1UpperLevelQueryCommand As String = ":CALC2:FRES:LIM1:UPP?"

#End Region
#End Region

#Region " LIMIT 2 "

#Region " LIMIT2 AUTO CLEAR "

    ''' <summary> Gets the Limit2 Auto Clear command Format. </summary>
    ''' <value> The Limit2 AutoClear query command. </value>
    Protected Overrides ReadOnly Property Limit2AutoClearCommandFormat As String = ":CALC2:FRES:LIM2:CLE:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the Limit2 Auto Clear query command. </summary>
    ''' <value> The Limit2 AutoClear query command. </value>
    Protected Overrides ReadOnly Property Limit2AutoClearQueryCommand As String = ":CALC2:FRES:LIM2:CLE:AUTO?"

#End Region

#Region " LIMIT2 ENABLED "

    ''' <summary> Gets the Limit2 enabled command Format. </summary>
    ''' <value> The Limit2 enabled query command. </value>
    Protected Overrides ReadOnly Property Limit2EnabledCommandFormat As String = ":CALC2:FRES:LIM2:STAT {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the Limit2 enabled query command. </summary>
    ''' <value> The Limit2 enabled query command. </value>
    Protected Overrides ReadOnly Property Limit2EnabledQueryCommand As String = ":CALC2:FRES:LIM2:STAT?"

#End Region

#Region " LIMIT2 LOWER LEVEL "

    ''' <summary> Gets the Limit2 Lower Level command format. </summary>
    ''' <value> The Limit2LowerLevel command format. </value>
    Protected Overrides ReadOnly Property Limit2LowerLevelCommandFormat As String = ":CALC2:FRES:LIM2:LOW {0}"

    ''' <summary> Gets the Limit2 Lower Level query command. </summary>
    ''' <value> The Limit2LowerLevel query command. </value>
    Protected Overrides ReadOnly Property Limit2LowerLevelQueryCommand As String = ":CALC2:FRES:LIM2:LOW?"

#End Region

#Region " LIMIT2 UPPER LEVEL "

    ''' <summary> Gets the Limit2 Upper Level command format. </summary>
    ''' <value> The Limit2UpperLevel command format. </value>
    Protected Overrides ReadOnly Property Limit2UpperLevelCommandFormat As String = ":CALC2:FRES:LIM2:UPP {0}"

    ''' <summary> Gets the Limit2 Upper Level query command. </summary>
    ''' <value> The Limit2UpperLevel query command. </value>
    Protected Overrides ReadOnly Property Limit2UpperLevelQueryCommand As String = ":CALC2:FRES:LIM2:UPP?"

#End Region
#End Region

#End Region

End Class
