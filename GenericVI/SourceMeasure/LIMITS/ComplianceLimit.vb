''' <summary> Defines the contract that must be implemented by a SCPI Compliance Limit Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class ComplianceLimit
    Inherits VI.Scpi.ComplianceLimitBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ComplianceLimit" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Public Sub New(ByVal statusSubsystem As isr.VI.StatusSubsystemBase)
        MyBase.New(1, statusSubsystem)
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

#Region " SYNTAX "

#Region " COMPLIANCE FAILURE BITS "

    ''' <summary> Gets Compliance Failure Bits query command. </summary>
    ''' <value> The Compliance Failure Bits query command. </value>
    Protected Overrides ReadOnly Property FailureBitsQueryCommand As String = ":CALC2:LIM{0}:COMP:SOUR2?"

    ''' <summary> Gets Compliance Failure Bits command format. </summary>
    ''' <value> The Compliance Failure Bits command format. </value>
    Protected Overrides ReadOnly Property FailureBitsCommandFormat As String = ":CALC2:LIM{0}:COMP:SOUR2 {{0}}"

#End Region

#Region " IN COMPLAINCE FAIL CONDITION "

    ''' <summary>
    ''' Gets or sets the In-compliance Condition command Format.
    ''' <see cref="P:isr.VI.Calculate2SubsystemBase.IncomplianceCondition">Condition</see> sentinel.
    ''' </summary>
    ''' <value> The in-compliance condition command format. </value>
    Protected Overrides ReadOnly Property IncomplianceConditionCommandFormat As String = ":CALC2:LIM{0}:COMP:FAIL {{0:'IN';'IN';'OUT'}}"

    Protected Overrides ReadOnly Property IncomplianceConditionQueryCommand As String = ":CALC2:LIM{0}:COMP:FAIL?"

#End Region

#Region " LIMIT FAILED "

    ''' <summary> Gets or sets the Limit Failed query command. </summary>
    ''' <value> The Limit Failed query command. </value>
    Protected Overrides ReadOnly Property FailedQueryCommand As String = ":CALC2:LIM{0}:FAIL?"

#End Region

#Region " LIMIT ENABLED "

    Protected Overrides ReadOnly Property EnabledCommandFormat As String = "CALC2:LIM{0}:STAT {{0:'ON';'ON';'OFF'}}"

    Protected Overrides ReadOnly Property EnabledQueryCommand As String = ":CALC2:LIM{0}:STAT?"

#End Region

#End Region

End Class
