''' <summary> Defines a Status Subsystem for a Keithley 7500 Meter. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class StatusSubsystem
    Inherits VI.Scpi.StatusSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystem" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.Pith.SessionBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal visaSession As VI.Pith.SessionBase)
        MyBase.New(visaSession)
        Me._VersionInfo = New VersionInfo
    End Sub

    ''' <summary> Creates a new StatusSubsystem. </summary>
    ''' <returns> A StatusSubsystem. </returns>
    Public Shared Function Create() As StatusSubsystem
        Dim subsystem As StatusSubsystem = Nothing
        Try
            subsystem = New StatusSubsystem(isr.VI.SessionFactory.Get.Factory.CreateSession())
        Catch
            If subsystem IsNot Nothing Then
                subsystem.Dispose()
                subsystem = Nothing
            End If
            Throw
        End Try
        Return subsystem
    End Function

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

    ''' <summary> Gets the preset command. </summary>
    ''' <value> The preset command. </value>
    Protected Overrides ReadOnly Property PresetCommand As String = VI.Pith.Scpi.Syntax.StatusPresetCommand

#Region " LANGUAGE "

    Protected Overrides ReadOnly Property LanguageQueryCommand As String = VI.Pith.Ieee488.Syntax.LanguageQueryCommand

    Protected Overrides ReadOnly Property LanguageCommandFormat As String = VI.Pith.Ieee488.Syntax.LanguageCommandFormat

#End Region

#Region " STANDARD STATUS  "

    ''' <summary> Gets the clear execution state command. </summary>
    ''' <value> The clear execution state command. </value>
    Protected Overrides ReadOnly Property ClearExecutionStateCommand As String = VI.Pith.Ieee488.Syntax.ClearExecutionStateCommand

    ''' <summary> Gets the reset known state command. </summary>
    ''' <value> The reset known state command. </value>
    Protected Overrides ReadOnly Property ResetKnownStateCommand As String = VI.Pith.Ieee488.Syntax.ResetKnownStateCommand

    ''' <summary> Gets the clear error queue command. </summary>
    ''' <value> The clear error queue command. </value>
    Protected Overrides ReadOnly Property ClearErrorQueueCommand As String = "SYST:CLE" ' VI.Pith.Scpi.Syntax.ClearErrorQueueCommand

    Protected Overrides ReadOnly Property DequeueErrorQueryCommand As String = VI.Pith.Scpi.Syntax.LastSystemErrorQueryCommand

    Protected Overrides ReadOnly Property DeviceErrorQueryCommand As String = "" '  VI.Pith.Scpi.Syntax.ErrorQueueQueryCommand

    ''' <summary> Gets the error queue query command. </summary>
    ''' <value> The error queue query command. </value>
    Protected Overrides ReadOnly Property NextDeviceErrorQueryCommand As String = "" '  VI.Pith.Scpi.Syntax.ErrorQueueQueryCommand

    ''' <summary> Gets the bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Public Overrides ReadOnly Property ErrorAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.ErrorAvailable

    ''' <summary> Gets the identity query command. </summary>
    ''' <value> The identity query command. </value>
    Protected Overrides ReadOnly Property IdentityQueryCommand As String = VI.Pith.Ieee488.Syntax.IdentityQueryCommand

    ''' <summary> Gets the bits that would be set for detecting if an Measurement is available. </summary>
    ''' <value> The Measurement available bits. </value>
    Public Overrides ReadOnly Property MeasurementAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.MeasurementEvent

    ''' <summary> Gets the bits that would be set for detecting if an Message is available. </summary>
    ''' <value> The Message available bits. </value>
    Public Overrides ReadOnly Property MessageAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.MessageAvailable

    ''' <summary> Gets the operation completed query command. </summary>
    ''' <value> The operation completed query command. </value>
    Protected Overrides ReadOnly Property OperationCompletedQueryCommand As String = VI.Pith.Ieee488.Syntax.OperationCompletedQueryCommand

    ''' <summary> Gets the bits that would be set for detecting if a Standard Event is available. </summary>
    ''' <value> The Standard Event available bits. </value>
    Public Overrides ReadOnly Property StandardEventAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.StandardEvent

    ''' <summary> Gets the standard event status query command. </summary>
    ''' <value> The standard event status query command. </value>
    Protected Overrides ReadOnly Property StandardEventStatusQueryCommand As String = VI.Pith.Ieee488.Syntax.StandardEventQueryCommand

    ''' <summary> Gets the standard event enable query command. </summary>
    ''' <value> The standard event enable query command. </value>
    Protected Overrides ReadOnly Property StandardEventEnableQueryCommand As String = VI.Pith.Ieee488.Syntax.StandardEventEnableQueryCommand

    ''' <summary> Gets the standard service enable command format. </summary>
    ''' <value> The standard service enable command format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCommandFormat As String = VI.Pith.Ieee488.Syntax.StandardServiceEnableCommandFormat

    ''' <summary> Gets the standard service enable and complete command format. </summary>
    ''' <value> The standard service enable command and complete format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCompleteCommandFormat As String = VI.Pith.Ieee488.Syntax.StandardServiceEnableCompleteCommandFormat

    ''' <summary> Gets the service request enable command format. </summary>
    ''' <value> The service request enable command format. </value>
    Protected Overrides ReadOnly Property ServiceRequestEnableCommandFormat As String = VI.Pith.Ieee488.Syntax.ServiceRequestEnableCommandFormat

#End Region

#Region " MEASUREMENT REGISTER EVENTS "

    ''' <summary> Gets the measurement status query command. </summary>
    ''' <value> The measurement status query command. </value>
    Protected Overrides ReadOnly Property MeasurementStatusQueryCommand As String = "" ' VI.Pith.Scpi.Syntax.MeasurementEventQueryCommand

    ''' <summary> Gets the measurement event condition query command. </summary>
    ''' <value> The measurement event condition query command. </value>
    Protected Overrides ReadOnly Property MeasurementEventConditionQueryCommand As String = "" '  VI.Pith.Scpi.Syntax.MeasurementEventConditionQueryCommand

#End Region

#Region " OPERATION REGISTER EVENTS "

    ''' <summary> Gets the operation event enable Query command </summary>
    ''' <value> The operation event enable Query command. </value>
    Protected Overrides ReadOnly Property OperationEventEnableQueryCommand As String = VI.Pith.Scpi.Syntax.OperationEventEnableQueryCommand

    ''' <summary> Gets the operation event enable command format. </summary>
    ''' <value> The operation event enable command format. </value>
    Protected Overrides ReadOnly Property OperationEventEnableCommandFormat As String = VI.Pith.Scpi.Syntax.OperationEventEnableCommandFormat

    ''' <summary> Gets the operation event status query command. </summary>
    ''' <value> The operation event status query command. </value>
    Protected Overrides ReadOnly Property OperationEventStatusQueryCommand As String = VI.Pith.Scpi.Syntax.OperationEventQueryCommand

#End Region

#Region " QUESTIONABLE REGISTER "

    ''' <summary> Gets the questionable status query command. </summary>
    ''' <value> The questionable status query command. </value>
    Protected Overrides ReadOnly Property QuestionableStatusQueryCommand As String = VI.Pith.Scpi.Syntax.QuestionableEventQueryCommand

#End Region

#End Region

#Region " IDENTITY "

    ''' <summary> Parse version information. </summary>
    ''' <param name="value"> The value. </param>
    Protected Overrides Sub ParseVersionInfo(value As String)
        MyBase.ParseVersionInfo(value)
        Me._VersionInfo = New VersionInfo
        Me.VersionInfo.Parse(value)
    End Sub

    ''' <summary> Gets or sets the information describing the version. </summary>
    ''' <value> Information describing the version. </value>
    Public Overloads ReadOnly Property VersionInfo As VersionInfo

#End Region

End Class

''' <summary>Gets or sets the status byte flags of the measurement event register.</summary>
<System.Flags()>
Public Enum MeasurementEvents
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Reading Overflow")> ReadingOverflow = 1
    <ComponentModel.Description("Low Limit 1 Failed")> LowLimit1Failed = 2
    <ComponentModel.Description("High Limit 1 Failed")> HighLimit1Failed = 4
    <ComponentModel.Description("Low Limit 2 Failed")> LowLimit2Failed = 8
    <ComponentModel.Description("High Limit 2 Failed")> HighLimit2Failed = 16
    <ComponentModel.Description("Reading Available")> ReadingAvailable = 32
    <ComponentModel.Description("Not Used 1")> NotUsed1 = 64
    <ComponentModel.Description("Buffer Available")> BufferAvailable = 128
    <ComponentModel.Description("Buffer Half Full")> BufferHalfFull = 256
    <ComponentModel.Description("Buffer Full")> BufferFull = 512
    <ComponentModel.Description("Buffer Overflow")> BufferOverflow = 1024
    <ComponentModel.Description("HardwareLimit Event")> HardwareLimitEvent = 2048
    <ComponentModel.Description("Buffer Quarter Full")> BufferQuarterFull = 4096
    <ComponentModel.Description("Buffer Three-Quarters Full")> BufferThreeQuartersFull = 8192
    <ComponentModel.Description("Master Limit")> MasterLimit = 16384
    <ComponentModel.Description("Not Used 2")> NotUsed2 = 32768
    <ComponentModel.Description("All")> All = 32767
End Enum

