Imports isr.VI
Imports isr.VI.Tsp
''' <summary> Status subsystem. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="12/14/2013" by="David" revision=""> Created. </history>
Public Class StatusSubsystem
    Inherits isr.VI.Tsp.StatusSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystem" /> class. </summary>
    ''' <param name="session"> The session. </param>
    Public Sub New(ByVal session As isr.VI.SessionBase)
        MyBase.New(session)
    End Sub

#End Region

#Region " I PRESETTABLE "

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

#Region " EXECUTION STATE "

    ''' <summary> Gets the clear execution state command. </summary>
    ''' <value> The clear execution state command. </value>
    Protected Overrides ReadOnly Property ClearExecutionStateCommand As String = TspSyntax.ClearExecutionStateCommand

    ''' <summary> Gets the reset known state command. </summary>
    ''' <value> The reset known state command. </value>
    ''' <remarks> Use reset() to reset all devices on the TSP link. </remarks>
    Protected Overrides ReadOnly Property ResetKnownStateCommand As String = TspSyntax.ResetKnownStateCommand

#End Region

#Region " STANDARD STATUS "

    ''' <summary> Gets the clear error queue command. </summary>
    ''' <value> The clear error queue command. </value>
    Protected Overrides ReadOnly Property ClearErrorQueueCommand As String = TspSyntax.ClearErrorQueueCommand

    ''' <summary> Gets the error queue query command. </summary>
    ''' <value> The error queue query command. </value>
    Protected Overrides ReadOnly Property NextErrorQueryCommand As String = TspSyntax.ErrorQueueQueryCommand

    ''' <summary> Gets the bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Public Overrides ReadOnly Property ErrorAvailableBits As isr.VI.ServiceRequests    =ServiceRequests.ErrorAvailable

    ''' <summary> Gets the identity query command. </summary>
    ''' <value> The identity query command. </value>
    Protected Overrides ReadOnly Property IdentityQueryCommand As String = Ieee488.Syntax.IdentityQueryCommand & " " & Ieee488.Syntax.WaitCommand

    ''' <summary> Gets the bits that would be set for detecting if an Measurement is available. </summary>
    ''' <value> The Measurement available bits. </value>
    Public Overrides ReadOnly Property MeasurementAvailableBits As ServiceRequests = ServiceRequests.MeasurementEvent

    ''' <summary> Gets the bits that would be set for detecting if an Message is available. </summary>
    ''' <value> The Message available bits. </value>
    Public Overrides ReadOnly Property MessageAvailableBits As ServiceRequests = ServiceRequests.MessageAvailable

    ''' <summary> Gets the operation completed query command. </summary>
    ''' <value> The operation completed query command. </value>
    Protected Overrides ReadOnly Property OperationCompletedQueryCommand As String = Ieee488.Syntax.OperationCompletedQueryCommand

    ''' <summary> Gets the bits that would be set for detecting if a Standard Event is available. </summary>
    ''' <value> The Standard Event available bits. </value>
    Public Overrides ReadOnly Property StandardEventAvailableBits As ServiceRequests = ServiceRequests.StandardEvent

    ''' <summary> Gets the standard event status query command. </summary>
    ''' <value> The standard event status query command. </value>
    Protected Overrides ReadOnly Property StandardEventStatusQueryCommand As String = TspSyntax.StandardEventQueryCommand

    ''' <summary> Gets the standard event enable query command. </summary>
    ''' <value> The standard event enable query command. </value>
    Protected Overrides ReadOnly Property StandardEventEnableQueryCommand As String = TspSyntax.StandardEventEnableQueryCommand

    ''' <summary> Gets the standard service enable command format. </summary>
    ''' <value> The standard service enable command format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCommandFormat As String = TspSyntax.StandardServiceEnableCommandFormat

    ''' <summary> Gets the standard service enable and complete command format. </summary>
    ''' <value> The standard service enable command and complete format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCompleteCommandFormat As String = TspSyntax.StandardServiceEnableCompleteCommandFormat

    ''' <summary> Gets the service request enable command format. </summary>
    ''' <value> The service request enable command format. </value>
    Protected Overrides ReadOnly Property ServiceRequestEnableCommandFormat As String = TspSyntax.ServiceRequestEnableCommandFormat

    ''' <summary> Gets the wait command. </summary>
    ''' <value> The wait command. </value>
    Protected Overrides ReadOnly Property WaitCommand As String = Ieee488.Syntax.WaitCommand

#End Region

#Region " MEASUREMENT REGISTER EVENTS "

    ''' <summary> Gets the measurement status query command. </summary>
    ''' <value> The measurement status query command. </value>
    Protected Overrides ReadOnly Property MeasurementStatusQueryCommand As String = TspSyntax.MeasurementEventQueryCommand

    ''' <summary> Gets the measurement event condition query command. </summary>
    ''' <value> The measurement event condition query command. </value>
    Protected Overrides ReadOnly Property MeasurementEventConditionQueryCommand As String = TspSyntax.MeasurementEventConditionQueryCommand

#End Region

#Region " OPERATION REGISTER EVENTS "

    ''' <summary> Gets the operation event enable Query command </summary>
    ''' <value> The operation event enable Query command. </value>
    Protected Overrides ReadOnly Property OperationEventEnableQueryCommand As String = TspSyntax.OperationEventEnableQueryCommand

    ''' <summary> Gets the operation event enable command format. </summary>
    ''' <value> The operation event enable command format. </value>
    Protected Overrides ReadOnly Property OperationEventEnableCommandFormat As String = TspSyntax.OperationEventEnableCommandFormat

    ''' <summary> Gets the operation event status query command. </summary>
    ''' <value> The operation event status query command. </value>
    Protected Overrides ReadOnly Property OperationEventStatusQueryCommand As String = TspSyntax.OperationEventQueryCommand

#End Region

#Region " QUESTIONABLE REGISTER "

    ''' <summary> Gets the questionable status query command. </summary>
    ''' <value> The questionable status query command. </value>
    Protected Overrides ReadOnly Property QuestionableStatusQueryCommand As String = TspSyntax.QuestionableEventQueryCommand

#End Region

#End Region

End Class
