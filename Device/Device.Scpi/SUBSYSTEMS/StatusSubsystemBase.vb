Imports isr.VI.ExceptionExtensions
''' <summary> Defines the contract that must be implemented by SCPI Status Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class StatusSubsystemBase
    Inherits VI.StatusSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="StatusSubsystemBase" /> class.
    ''' </summary>
    ''' <param name="visaSession">            A reference to a <see cref="VI.Pith.SessionBase">message based
    '''                                       session</see>. </param>
    ''' <param name="noErrorCompoundMessage"> Message describing the no error compound. </param>
    Protected Sub New(ByVal visaSession As VI.Pith.SessionBase, ByVal noErrorCompoundMessage As String)
        MyBase.New(visaSession, noErrorCompoundMessage)
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystemBase" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.Pith.SessionBase">message based
    ''' session</see>. </param>
    Protected Sub New(ByVal visaSession As VI.Pith.SessionBase)
        Me.New(visaSession, VI.Pith.Scpi.Syntax.NoErrorCompoundMessage)
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " OPC "

    ''' <summary> Gets or sets the wait command. </summary>
    ''' <value> The wait command. </value>
    Protected Overrides ReadOnly Property WaitCommand As String = VI.Pith.Ieee488.Syntax.WaitCommand

#End Region

#Region " EXECUTION STATE "

    ''' <summary> Gets the preset command. </summary>
    ''' <value> The preset command. </value>
    ''' <remarks> No preset command for the TSP system. </remarks>
    Protected Overrides ReadOnly Property PresetCommand As String = VI.Pith.Scpi.Syntax.StatusPresetCommand

    ''' <summary> Gets the clear execution state command. </summary>
    ''' <value> The clear execution state command. </value>
    Protected Overrides ReadOnly Property ClearExecutionStateCommand As String = VI.Pith.Ieee488.Syntax.ClearExecutionStateCommand

    ''' <summary> Gets the reset known state command. </summary>
    ''' <value> The reset known state command. </value>
    Protected Overrides ReadOnly Property ResetKnownStateCommand As String = VI.Pith.Ieee488.Syntax.ResetKnownStateCommand

#End Region

#Region " SERVICE REQUEST "

    ''' <summary> Gets the bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Public Overrides ReadOnly Property ErrorAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.ErrorAvailable

    ''' <summary> Gets the bits that would be set for detecting if an Measurement is available. </summary>
    ''' <value> The Measurement available bits. </value>
    Public Overrides ReadOnly Property MeasurementAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.MeasurementEvent

    ''' <summary> Gets the bits that would be set for detecting if an Message is available. </summary>
    ''' <value> The Message available bits. </value>
    Public Overrides ReadOnly Property MessageAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.MessageAvailable

    ''' <summary> Gets the bits that would be set for detecting if a Standard Event is available. </summary>
    ''' <value> The Standard Event available bits. </value>
    Public Overrides ReadOnly Property StandardEventAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.StandardEvent

    ''' <summary> Gets or sets the operation completed query command. </summary>
    ''' <value> The operation completed query command. </value>
    Protected Overrides ReadOnly Property OperationCompletedQueryCommand As String = VI.Pith.Ieee488.Syntax.OperationCompletedQueryCommand

    ''' <summary> Gets the standard service enable command format. </summary>
    ''' <value> The standard service enable command format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCommandFormat As String = VI.Pith.Ieee488.Syntax.StandardServiceEnableCommandFormat

    ''' <summary> Gets or sets the standard service enable and complete command format. </summary>
    ''' <value> The standard service enable command and complete format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCompleteCommandFormat As String = VI.Pith.Ieee488.Syntax.StandardServiceEnableCompleteCommandFormat

    ''' <summary> Gets the service request enable command format. </summary>
    ''' <value> The service request enable command format. </value>
    Protected Overrides ReadOnly Property ServiceRequestEnableCommandFormat As String = VI.Pith.Ieee488.Syntax.ServiceRequestEnableCommandFormat

#End Region

#Region " STANDARD EVENT "

    ''' <summary> Gets the standard event status query command. </summary>
    ''' <value> The standard event status query command. </value>
    Protected Overrides ReadOnly Property StandardEventStatusQueryCommand As String = VI.Pith.Ieee488.Syntax.StandardEventQueryCommand

    ''' <summary> Gets the standard event enable query command. </summary>
    ''' <value> The standard event enable query command. </value>
    Protected Overrides ReadOnly Property StandardEventEnableQueryCommand As String = VI.Pith.Ieee488.Syntax.StandardEventEnableQueryCommand

#End Region

#Region " DEVICE ERRORS "

    ''' <summary> Gets the clear error queue command. </summary>
    ''' <value> The clear error queue command. </value>
    Protected Overrides ReadOnly Property ClearErrorQueueCommand As String = VI.Pith.Scpi.Syntax.ClearSystemErrorQueueCommand

    ''' <summary> Gets or sets the 'Next Error' query command. </summary>
    ''' <value> The error queue query command. </value>
    Protected Overrides ReadOnly Property DequeueErrorQueryCommand As String = "" ' VI.Pith.Scpi.Syntax.LastSystemErrorQueryCommand

    ''' <summary> Gets the last error query command. </summary>
    ''' <value> The last error query command. </value>
    Protected Overrides ReadOnly Property DeviceErrorQueryCommand As String = ""  ' VI.Pith.Scpi.Syntax.LastSystemErrorQueryCommand

    ''' <summary> Gets the error queue query command. </summary>
    ''' <value> The error queue query command. </value>
    Protected Overrides ReadOnly Property NextDeviceErrorQueryCommand As String = "" ' = VI.Pith.Scpi.Syntax.ErrorQueueQueryCommand

#End Region

#Region " LINE FREQUENCY "

    ''' <summary> Gets line frequency query command. </summary>
    ''' <value> The line frequency query command. </value>
    Protected Overrides ReadOnly Property LineFrequencyQueryCommand As String = VI.Pith.Scpi.Syntax.ReadLineFrequencyCommand

#End Region

#Region " IDENTITY "

    ''' <summary> Gets the identity query command. </summary>
    ''' <value> The identity query command. </value>
    Protected Overrides ReadOnly Property IdentityQueryCommand As String = VI.Pith.Ieee488.Syntax.IdentityQueryCommand

#End Region

#End Region

#Region " TALKER "

    ''' <summary> Identifies the talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        Mybase.IdentifyTalkers()
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region

End Class


