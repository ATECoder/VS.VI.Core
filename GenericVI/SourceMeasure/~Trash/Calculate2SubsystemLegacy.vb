''' <summary> Defines the contract that must be implemented by a SCPI Calculate 2 Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class Calculate2Subsystem
    Inherits VI.Scpi.Calculate2SubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="Calculate2Subsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Public Sub New(ByVal statusSubsystem As isr.VI.StatusSubsystemBase)
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


#Region " SYNTAX "

#Region " FEED SOURCE "

    ''' <summary> Gets or sets the feed source query command. </summary>
    ''' <value> The feed source query command. </value>
    Protected Overrides ReadOnly Property FeedSourceQueryCommand As String = ":CALC2:FEED?"

    ''' <summary> Gets or sets the feed source command format. </summary>
    ''' <value> The write feed source command format. </value>
    Protected Overrides ReadOnly Property FeedSourceCommandFormat As String = "CALC2:FEED {0}"

#End Region

#Region " LIMIT1 "

#Region " COMPLIANCE FAILURE BITS "

    ''' <summary> Gets Compliance Failure Bits query command. </summary>
    ''' <value> The Compliance Failure Bits query command. </value>
    Protected Overrides ReadOnly Property ComplianceFailureBitsQueryCommand As String = ":CALC2:LIM:COMP:SOUR2?"

    ''' <summary> Gets Compliance Failure Bits command format. </summary>
    ''' <value> The Compliance Failure Bits command format. </value>
    Protected Overrides ReadOnly Property ComplianceFailureBitsCommandFormat As String = ":CALC2:LIM:COMP:SOUR2 {0}"

#End Region

#Region " IN COMPLAINCE FAIL CONDITION "

    ''' <summary>
    ''' Gets or sets the In-compliance Condition command Format.
    ''' <see cref="P:isr.VI.Calculate2SubsystemBase.IncomplianceCondition">Condition</see> sentinel.
    ''' </summary>
    ''' <value> The in-compliance condition command format. </value>
    Protected Overrides ReadOnly Property IncomplianceConditionCommandFormat As String = ":CALC2:LIM:COMP:FAIL {0:'IN';'IN';'OUT'}"

    Protected Overrides ReadOnly Property IncomplianceConditionQueryCommand As String = ":CALC2:LIM:COMP:FAIL?"

#End Region

#Region " FAIL "

    ''' <summary> Gets or sets the Limit Failed query command. </summary>
    ''' <value> The Limit Failed query command. </value>
    Protected Overrides ReadOnly Property LimitFailedQueryCommand As String = ":CALC2:LIM:FAIL?"

#End Region

#Region " ENABLED "

    Protected Overrides ReadOnly Property LimitEnabledCommandFormat As String = "CALC2:LIM:STAT {0:'ON';'ON';'OFF'}"

    Protected Overrides ReadOnly Property LimitEnabledQueryCommand As String = ":CALC2:LIM:STAT?"

#End Region

#Region " LIMIT 2 "

    Protected Overrides ReadOnly Property PassBitsQueryCommand As String = ":CALC2:LIM2:PASS:SOUR2?"

    Protected Overrides ReadOnly Property PassBitsCommandFormat As String = ":CALC2:LIM2:PASS:SOUR2 {0}"

    Protected Overrides ReadOnly Property LowerLimitQueryCommand As String = ":CALC2:LIM2:LOW?"

    Protected Overrides ReadOnly Property LowerLimitCommandFormat As String = ":CALC2:LIM2:LOW {0}"

    Protected Overrides ReadOnly Property LowerLimitFailureBitsQueryCommand As String = ":CALC2:LIM2:LOW:SOUR2?"

    Protected Overrides ReadOnly Property LowerLimitFailureBitsCommandFormat As String = ":CALC2:LIM2:LOW:SOUR2 {0}"

    Protected Overrides ReadOnly Property UpperLimitQueryCommand As String = ":CALC2:LIM2:UPP?"

    Protected Overrides ReadOnly Property UpperLimitCommandFormat As String = ":CALC2:LIM2:UPP {0}"

    Protected Overrides ReadOnly Property UpperLimitFailureBitsQueryCommand As String = ":CALC2:LIM2:UPP:SOUR2?"

    Protected Overrides ReadOnly Property UpperLimitFailureBitsCommandFormat As String = ":CALC2:LIM2:UPP:SOUR2 {0}"

#End Region
#End Region

#Region " COMPOSITES "

    ''' <summary> Gets or sets the composite limits clear command. </summary>
    ''' <value> The composite limits clear command. </value>
    ''' <remarks> SCPI: ":CLAC2:CLIM:CLE" </remarks>
    Protected Overrides ReadOnly Property CompositeLimitsClearCommand As String = ":CLAC2:CLIM:CLE"

    ''' <summary> Gets the Composite Limits Auto Clear enabled query command. </summary>
    ''' <value> The Composite Limits Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:CLE:AUTO?" </remarks>
    Protected Overrides ReadOnly Property CompositeLimitsAutoClearEnabledQueryCommand As String = ":CALC2:CLIM:CLE:AUTO?"

    ''' <summary> Gets the Composite Limits Auto Clear enabled command Format. </summary>
    ''' <value> The Composite Limits Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:CLE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overrides ReadOnly Property CompositeLimitsAutoClearEnabledCommandFormat As String = ":CALC2:CLIM:CLE:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets or sets the Composite Limits failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property CompositeLimitsFailureBitsQueryCommand As String = ":CALC2:CLIM:FAIL:SOUR2?"

    ''' <summary> Gets or sets the Composite Limits Failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property CompositeLimitsFailureBitsCommandFormat As String = ":CALC2:CLIM:FAIL:SOUR2 {0}"

    ''' <summary> Gets or sets the Composite Limits Pass Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property CompositeLimitsPassBitsQueryCommand As String = ":CALC2:CLIM:PASS:SOUR2?"

    ''' <summary> Gets or sets the Composite Limits Pass Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property CompositeLimitsPassBitsCommandFormat As String = ":CALC2:CLIM:PASS:SOUR2 {0}"

    ''' <summary> Gets the Binning Control query command. </summary>
    ''' <value> The Binning Control query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:BCON" </remarks>
    Protected Overrides ReadOnly Property BinningControlQueryCommand As String = ":CALC2:CLIM:BCON"

    ''' <summary> Gets the Binning Control command format. </summary>
    ''' <value> The Binning Control query command format. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:BCON {0}" </remarks>
    Protected Overrides ReadOnly Property BinningControlCommandFormat As String = ":CALC2:CLIM:BCON {0}"

    ''' <summary> Gets the Limit Mode query command. </summary>
    ''' <value> The Limit Mode query command. </value>
    ''' <remarks> SCPI: "CALC2:CLIM:MODE?" </remarks>
    Protected Overrides ReadOnly Property LimitModeQueryCommand As String = "CALC2:CLIM:MODE?"

    ''' <summary> Gets the Limit Mode command format. </summary>
    ''' <value> The Limit Mode command format. </value>
    ''' <remarks> SCPI: "CALC2:CLIM:MODE {0}" </remarks>
    Protected Overrides ReadOnly Property LimitModeCommandFormat As String = "CALC2:CLIM:MODE {0}"

#End Region

#End Region
End Class
