﻿''' <summary> Defines the contract that must be implemented by SCPI Status Subsystem. </summary>
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

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="StatusSubsystemBase" /> class.
    ''' </summary>
    ''' <remarks> David, 1/12/2016. </remarks>
    ''' <param name="visaSession">            A reference to a <see cref="VI.SessionBase">message based
    '''                                       session</see>. </param>
    ''' <param name="noErrorCompoundMessage"> Message describing the no error compound. </param>
    Protected Sub New(ByVal visaSession As VI.SessionBase, ByVal noErrorCompoundMessage As String)
        MyBase.New(visaSession, noErrorCompoundMessage)
        Me._VersionInfo = New VersionInfo
        ' check for query and other errors reported by the standard event register
        Me.StandardDeviceErrorAvailableBits = StandardEvents.CommandError Or StandardEvents.DeviceDependentError Or
                                                  StandardEvents.ExecutionError Or StandardEvents.QueryError
        ' Me.StandardServiceEnableCommandFormat = Ieee488.Syntax.StandardServiceEnableCommandFormat
        ' Me.StandardServiceEnableCompleteCommandFormat = Ieee488.Syntax.StandardServiceEnableCompleteCommandFormat

    End Sub

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystemBase" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.SessionBase">message based
    ''' session</see>. </param>
    Protected Sub New(ByVal visaSession As VI.SessionBase)
        Me.New(visaSession, Scpi.Syntax.NoErrorCompoundMessage)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Clears Error Queue.
    '''           </para></remarks>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Try
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Enabling wait completion;. ")
            Me.EnableWaitComplete()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Exception enabling wait completion;. Details: {0}.", ex)
        End Try
        Try
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Reading identity;. ")
            Me.QueryIdentity()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Exception reading identity;. Details: {0}.", ex)
        End Try
    End Sub

    ''' <summary> Sets the subsystem to its preset state. </summary>
    Public Overrides Sub PresetKnownState()
        MyBase.PresetKnownState()
        Me.OperationEventEnableBitmask = 0
        Me.QuestionableEventEnableBitmask = 0
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.MeasurementEventEnableBitmask = 0
        Me.MeasurementEventStatus = 0
        Me.OperationEventEnableBitmask = 0
        Me.OperationEventStatus = 0
        Me.QuestionableEventEnableBitmask = 0
        Me.QuestionableEventStatus = 0
        Me.StandardEventStatus = 0
        Me.StandardEventEnableBitmask = 0
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " EXECUTION STATE "

    ''' <summary> Gets the preset command. </summary>
    ''' <value> The preset command. </value>
    ''' <remarks> No preset command for the TSP system. </remarks>
    Protected Overrides ReadOnly Property PresetCommand As String = VI.Scpi.Syntax.StatusPresetCommand

    ''' <summary> Gets the clear execution state command. </summary>
    ''' <value> The clear execution state command. </value>
    Protected Overrides ReadOnly Property ClearExecutionStateCommand As String = Ieee488.Syntax.ClearExecutionStateCommand

    ''' <summary> Gets the reset known state command. </summary>
    ''' <value> The reset known state command. </value>
    Protected Overrides ReadOnly Property ResetKnownStateCommand As String = Ieee488.Syntax.ResetKnownStateCommand

#End Region

#Region " SERVICE REQUEST "

    ''' <summary> Gets the bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Public Overrides ReadOnly Property ErrorAvailableBits As ServiceRequests = ServiceRequests.ErrorAvailable

    ''' <summary> Gets the bits that would be set for detecting if an Measurement is available. </summary>
    ''' <value> The Measurement available bits. </value>
    Public Overrides ReadOnly Property MeasurementAvailableBits As ServiceRequests = ServiceRequests.MeasurementEvent

    ''' <summary> Gets the bits that would be set for detecting if an Message is available. </summary>
    ''' <value> The Message available bits. </value>
    Public Overrides ReadOnly Property MessageAvailableBits As ServiceRequests = ServiceRequests.MessageAvailable

    ''' <summary> Gets the bits that would be set for detecting if a Standard Event is available. </summary>
    ''' <value> The Standard Event available bits. </value>
    Public Overrides ReadOnly Property StandardEventAvailableBits As ServiceRequests = ServiceRequests.StandardEvent

    ''' <summary> Gets or sets the operation completed query command. </summary>
    ''' <value> The operation completed query command. </value>
    Protected Overrides ReadOnly Property OperationCompletedQueryCommand As String = Ieee488.Syntax.OperationCompletedQueryCommand

    ''' <summary> Gets the standard service enable command format. </summary>
    ''' <value> The standard service enable command format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCommandFormat As String = Ieee488.Syntax.StandardServiceEnableCommandFormat

    ''' <summary> Gets or sets the standard service enable and complete command format. </summary>
    ''' <value> The standard service enable command and complete format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCompleteCommandFormat As String = Ieee488.Syntax.StandardServiceEnableCompleteCommandFormat

    ''' <summary> Gets the service request enable command format. </summary>
    ''' <value> The service request enable command format. </value>
    Protected Overrides ReadOnly Property ServiceRequestEnableCommandFormat As String = Ieee488.Syntax.ServiceRequestEnableCommandFormat

#End Region

#Region " STANDARD EVENT "

    ''' <summary> Gets the standard event status query command. </summary>
    ''' <value> The standard event status query command. </value>
    Protected Overrides ReadOnly Property StandardEventStatusQueryCommand As String = Ieee488.Syntax.StandardEventQueryCommand

    ''' <summary> Gets the standard event enable query command. </summary>
    ''' <value> The standard event enable query command. </value>
    Protected Overrides ReadOnly Property StandardEventEnableQueryCommand As String = Ieee488.Syntax.StandardEventEnableQueryCommand

#End Region

#Region " DEVICE ERRORS "

    ''' <summary> Gets the last error query command. </summary>
    ''' <value> The last error query command. </value>
    Protected Overrides ReadOnly Property LastErrorQueryCommand As String = VI.Scpi.Syntax.LastErrorQueryCommand

    ''' <summary> Gets the clear error queue command. </summary>
    ''' <value> The clear error queue command. </value>
    Protected Overrides ReadOnly Property ClearErrorQueueCommand As String = "SYST:CLE" ' VI.Scpi.Syntax.ClearErrorQueueCommand

    ''' <summary> Gets the error queue query command. </summary>
    ''' <value> The error queue query command. </value>
    Protected Overrides ReadOnly Property ErrorQueueQueryCommand As String = "" ' = VI.Scpi.Syntax.ErrorQueueQueryCommand

#End Region

#Region " LINE FREQUENCY "

    ''' <summary> Gets line frequency query command. </summary>
    ''' <value> The line frequency query command. </value>
    Protected Overrides ReadOnly Property LineFrequencyQueryCommand As String = VI.Scpi.Syntax.ReadLineFrequencyCommand

#End Region
#End Region

#Region " DEVICE REGISTERS "

    ''' <summary> Reads the standard registers based on the service request register status. </summary>
    Public Overrides Sub ReadRegisters()
        MyBase.ReadRegisters()
        If Me.StandardEventAvailable Then
            Me.QueryStandardEventStatus()
        End If
        If Me.MeasurementAvailable Then
            Me.QueryMeasurementEventStatus()
        End If
        If (Me.ServiceRequestStatus And ServiceRequests.OperationEvent) <> 0 Then
            Me.QueryOperationEventStatus()
        End If
        If (Me.ServiceRequestStatus And ServiceRequests.QuestionableEvent) <> 0 Then
            Me.QueryQuestionableEventStatus()
        End If
    End Sub

#End Region

#Region " MEASUREMENT REGISTER "

#Region " BITMASK"

    ''' <summary> The measurement event enable bitmask. </summary>
    Private _MeasurementEventEnableBitmask As Integer?

    ''' <summary> Gets or sets the cached value of the measurement register event enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the measurement events type that is specific to
    ''' the instrument The enable register gates the corresponding events for registration by the
    ''' Status Byte register. When an event bit is set and the corresponding enable bit is set, the
    ''' output (summary) of the register will set to 1, which in turn sets the summary bit of the
    ''' Status Byte Register. </remarks>
    ''' <value> The mask to use for enabling the events. </value>
    Public Property MeasurementEventEnableBitmask() As Integer?
        Get
            Return Me._MeasurementEventEnableBitmask
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.MeasurementEventEnableBitmask, value) Then
                Me._MeasurementEventEnableBitmask = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.MeasurementEventEnableBitmask))
            End If
        End Set
    End Property

    ''' <summary> Programs and reads back the measurement register events enable bit mask. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The bit mask or nothing if not known. </returns>
    Public Function ApplyMeasurementEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.WriteMeasurementEventEnableBitmask(value)
        Return Me.QueryMeasurementEventEnableBitmask()
    End Function

    ''' <summary> Reads back the measurement register event enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the measurement events type that is specific to
    ''' the instrument The enable register gates the corresponding events for registration by the
    ''' Status Byte register. When an event bit is set and the corresponding enable bit is set, the
    ''' output (summary) of the register will set to 1, which in turn sets the summary bit of the
    ''' Status Byte Register. </remarks>
    ''' <returns>  The mask used for enabling the events. </returns>
    Public Function QueryMeasurementEventEnableBitmask() As Integer?
        Me.MeasurementEventEnableBitmask = Me.Session.Query(Me.MeasurementEventEnableBitmask.GetValueOrDefault(0), ":STAT:MEAS:ENAB?")
        Return Me.MeasurementEventEnableBitmask
    End Function

    ''' <summary> Programs the measurement register events enable bit mask without updating the value from
    ''' the device. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The bit mask or nothing if not known. </returns>
    Public Function WriteMeasurementEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.Session.WriteLine(":STAT:MEAS:ENAB {0:D}", value)
        Me.MeasurementEventEnableBitmask = value
        Return Me.MeasurementEventEnableBitmask
    End Function

#End Region

#Region " CONDITION "

    ''' <summary> The measurement event Condition. </summary>
    Private _MeasurementEventCondition As Integer?

    ''' <summary> Gets or sets the cached Condition of the measurement register events. </summary>
    ''' <value> <c>null</c> if value is not known;. </value>
    Public Property MeasurementEventCondition() As Integer?
        Get
            Return Me._MeasurementEventCondition
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.MeasurementEventCondition, value) Then
                Me._MeasurementEventCondition = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.MeasurementEventCondition))
            End If
        End Set
    End Property

    ''' <summary> Gets the measurement event condition query command. </summary>
    ''' <remarks> SCPI: ":STAT:MEAS:COND?".
    ''' <see cref="Scpi.Syntax.MeasurementEventConditionQueryCommand"> </see> </remarks>
    ''' <value> The measurement event condition query command. </value>
    Protected Overridable ReadOnly Property MeasurementEventConditionQueryCommand As String = VI.Scpi.Syntax.MeasurementEventConditionQueryCommand

    ''' <summary> Reads the condition of the measurement register event. </summary>
    ''' <returns> System.Nullable{System.Int32}. </returns>
    Public Overridable Function QueryMeasurementEventCondition() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.MeasurementEventConditionQueryCommand) Then
            Me.MeasurementEventCondition = Me.Session.Query(0I, Me.MeasurementEventConditionQueryCommand)
        End If
        Return Me.MeasurementEventCondition
    End Function

#End Region

#Region " STATUS "

    ''' <summary> The measurement event status. </summary>
    Private _MeasurementEventStatus As Integer?

    ''' <summary> Gets or sets the cached status of the measurement register events. </summary>
    ''' <value> <c>null</c> if value is not known;. </value>
    Public Property MeasurementEventStatus() As Integer?
        Get
            Return Me._MeasurementEventStatus
        End Get
        Protected Set(ByVal value As Integer?)
            Me._MeasurementEventStatus = value
            Me.AsyncNotifyPropertyChanged(NameOf(Me.MeasurementEventStatus))
        End Set
    End Property

    ''' <summary> Gets the measurement status query command. </summary>
    ''' <remarks> SCPI: ":STAT:MEAS:EVEN?".
    ''' <see cref="Scpi.Syntax.MeasurementEventQueryCommand"> </see> </remarks>
    ''' <value> The measurement status query command. </value>
    Protected Overridable ReadOnly Property MeasurementStatusQueryCommand As String = VI.Scpi.Syntax.MeasurementEventQueryCommand

    ''' <summary> Reads the status of the measurement register events. </summary>
    ''' <returns> System.Nullable{System.Int32}. </returns>
    Public Overridable Function QueryMeasurementEventStatus() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.MeasurementStatusQueryCommand) Then
            Me.MeasurementEventStatus = Me.Session.Query(0I, Me.MeasurementStatusQueryCommand)
        End If
        Return Me.MeasurementEventStatus
    End Function
#End Region

#End Region

#Region " OPERATION REGISTER "

#Region " ENABLE BITMASK "

    ''' <summary> The operation event enable bitmask. </summary>
    Private _OperationEventEnableBitmask As Integer?

    ''' <summary> Gets or sets the cached value of the Operation register event enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the Operation events type that is specific to the
    ''' instrument The enable register gates the corresponding events for registration by the Status
    ''' Byte register. When an event bit is set and the corresponding enable bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <value> The mask to use for enabling the events. </value>
    Public Property OperationEventEnableBitmask() As Integer?
        Get
            Return Me._OperationEventEnableBitmask
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.OperationEventEnableBitmask, value) Then
                Me._OperationEventEnableBitmask = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.OperationEventEnableBitmask))
            End If
        End Set
    End Property

    ''' <summary> Programs and reads back the Operation register events enable bit mask. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The bit mask or nothing if not known. </returns>
    Public Function ApplyOperationEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.WriteOperationEventEnableBitmask(value)
        Return Me.QueryOperationEventEnableBitmask()
    End Function

    ''' <summary> Gets the operation event enable query command. </summary>
    ''' <remarks> SCPI: ":STAT:OPER:ENAB?". </remarks>
    ''' <value> The operation event enable command format. </value>
    Protected Overridable ReadOnly Property OperationEventEnableQueryCommand As String = VI.Scpi.Syntax.OperationEventEnableQueryCommand

    ''' <summary> Queries the Operation register event enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the Operation events type that is specific to the
    ''' instrument The enable register gates the corresponding events for registration by the Status
    ''' Byte register. When an event bit is set and the corresponding enable bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <returns> The mask to use for enabling the events; nothing if unknown </returns>
    Public Function QueryOperationEventEnableBitmask() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.OperationEventEnableQueryCommand) Then
            Me.OperationEventEnableBitmask = Me.Session.Query(0I, Me.OperationEventEnableQueryCommand)
        End If
        Return Me.OperationEventEnableBitmask
    End Function

    ''' <summary> Gets or sets the operation event enable command format. </summary>
    ''' <remarks> SCPI: ":STAT:OPER:ENAB {0:D}".
    ''' <see cref="Scpi.Syntax.OperationEventEnableCommandFormat"> </see> </remarks>
    ''' <value> The operation event enable command format. </value>
    Protected Overridable ReadOnly Property OperationEventEnableCommandFormat As String = VI.Scpi.Syntax.OperationEventEnableCommandFormat

    ''' <summary> Programs the Operation register event enable bit mask. </summary>
    ''' <remarks> When an event bit is set and the corresponding enable bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <returns> The mask to use for enabling the events; nothing if unknown </returns>
    Public Overridable Function WriteOperationEventEnableBitmask(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.OperationEventEnableCommandFormat) Then
            Me.Session.WriteLine(Me.OperationEventEnableCommandFormat, value)
        End If
        Me.OperationEventEnableBitmask = value
        Return Me.OperationEventEnableBitmask
    End Function

#End Region

#Region " NEGATIVE TRANSITION EVENT ENABLE BITMASK "

    ''' <summary> The operation register negative transition events enable bitmask. </summary>
    Private _OperationNegativeTransitionEventEnableBitmask As Integer?

    ''' <summary> Gets or sets the operation negative transition event request. </summary>
    ''' <remarks> At this time this reads back the event status rather than the enable status. The
    ''' returned value could be cast to the Operation Transition events type that is specific to the
    ''' instrument. </remarks>
    ''' <value> The Operation Transition Events mask to use for enabling the events. </value>
    Public Property OperationNegativeTransitionEventEnableBitmask() As Integer?
        Get
            Return Me._OperationNegativeTransitionEventEnableBitmask
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.OperationNegativeTransitionEventEnableBitmask, value) Then
                Me._OperationNegativeTransitionEventEnableBitmask = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.OperationNegativeTransitionEventEnableBitmask))
            End If
        End Set
    End Property

    ''' <summary> Queries the operation register negative transition events enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the Operation events type that is specific to the
    ''' instrument The enable register gates the corresponding events for registration by the Status
    ''' Byte register. When an event bit is set and the corresponding enable bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <returns> The mask to use for enabling the events; nothing if unknown </returns>
    Public Overridable Function QueryOperationNegativeTransitionEventEnableBitmask() As Integer?
        Me.OperationNegativeTransitionEventEnableBitmask = Me.Session.Query(Me.OperationNegativeTransitionEventEnableBitmask.GetValueOrDefault(0), ":STAT:OPER:NTR?")
        Return Me.OperationNegativeTransitionEventEnableBitmask
    End Function

    ''' <summary> Programs and reads back the operation register negative transition events enable bit mask. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The bit mask or nothing if not known. </returns>
    Public Function ApplyOperationNegativeTransitionEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.WriteOperationNegativeTransitionEventEnableBitmask(value)
        Return Me.QueryOperationNegativeTransitionEventEnableBitmask()
    End Function

    ''' <summary> Programs the operation register negative transition events enable bit mask. </summary>
    ''' <remarks> When an event bit is set and the corresponding enable bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <returns> The mask to use for enabling the events; nothing if unknown </returns>
    Public Overridable Function WriteOperationNegativeTransitionEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.Session.WriteLine(":STAT:OPER:NTR {0:D}", value)
        Me.OperationNegativeTransitionEventEnableBitmask = value
        Return Me.OperationNegativeTransitionEventEnableBitmask
    End Function

#End Region

#Region " POSITIVE TRANSITION EVENT ENABLE BITMASK "

    ''' <summary> The operation register Positive transition events enable bitmask. </summary>
    Private _OperationPositiveTransitionEventEnableBitmask As Integer?

    ''' <summary> Gets or sets the cached value of the operation Positive transition event request. </summary>
    ''' <remarks> At this time this reads back the event status rather than the enable status. The
    ''' returned value could be cast to the Operation Transition events type that is specific to the
    ''' instrument. </remarks>
    ''' <value> The Operation Transition Events mask to use for enabling the events. </value>
    Public Property OperationPositiveTransitionEventEnableBitmask() As Integer?
        Get
            Return Me._OperationPositiveTransitionEventEnableBitmask
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.OperationPositiveTransitionEventEnableBitmask, value) Then
                Me._OperationPositiveTransitionEventEnableBitmask = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.OperationPositiveTransitionEventEnableBitmask))
            End If
        End Set
    End Property

    ''' <summary> Queries the operation register Positive transition events enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the Operation events type that is specific to the
    ''' instrument The enable register gates the corresponding events for registration by the Status
    ''' Byte register. When an event bit is set and the corresponding enable bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <returns> The mask to use for enabling the events; nothing if unknown </returns>
    Public Overridable Function QueryOperationPositiveTransitionEventEnableBitmask() As Integer?
        Me.OperationPositiveTransitionEventEnableBitmask = Me.Session.Query(Me.OperationPositiveTransitionEventEnableBitmask.GetValueOrDefault(0), ":STAT:OPER:PTR?")
        Return Me.OperationPositiveTransitionEventEnableBitmask
    End Function

    ''' <summary> Programs and reads back the operation register Positive transition events enable bit mask. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The bit mask or nothing if not known. </returns>
    Public Function ApplyOperationPositiveTransitionEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.WriteOperationPositiveTransitionEventEnableBitmask(value)
        Return Me.QueryOperationPositiveTransitionEventEnableBitmask()
    End Function

    ''' <summary> Programs the operation register Positive transition events enable bit mask. </summary>
    ''' <remarks> When an event bit is set and the corresponding enable bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <returns> The mask to use for enabling the events; nothing if unknown </returns>
    Public Overridable Function WriteOperationPositiveTransitionEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.Session.WriteLine(":STAT:OPER:PTR {0:D}", value)
        Me.OperationPositiveTransitionEventEnableBitmask = value
        Return Me.OperationPositiveTransitionEventEnableBitmask
    End Function

#End Region

#Region " CONDITION "

    ''' <summary> The operation event Condition. </summary>
    Private _OperationEventCondition As Integer?

    ''' <summary> Gets the Operation event register cached condition of the instrument. </summary>
    ''' <remarks> This value reflects the cached condition of the instrument. The returned value
    ''' could be cast to the Operation events type that is specific to the instrument The condition
    ''' register is a real-time, read-only register that constantly updates to reflect the present
    ''' operating conditions of the instrument. </remarks>
    ''' <value> The operation event condition. </value>
    Public Property OperationEventCondition() As Integer?
        Get
            Return Me._OperationEventCondition
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.OperationEventCondition, value) Then
                Me._OperationEventCondition = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.OperationEventCondition))
            End If
        End Set
    End Property

    ''' <summary> Queries the Operation event register condition from the instrument. </summary>
    ''' <remarks> This value reflects the real time condition of the instrument. The returned value
    ''' could be cast to the Operation events type that is specific to the instrument The condition
    ''' register is a real-time, read-only register that constantly updates to reflect the present
    ''' operating conditions of the instrument. </remarks>
    ''' <returns> The operation event condition. </returns>
    Public Overridable Function QueryOperationEventCondition() As Integer?
        Me.OperationEventCondition = Me.Session.Query(Me.OperationEventCondition.GetValueOrDefault(0), ":STAT:OPER:COND?")
        Return Me.OperationEventCondition
    End Function

#End Region

#Region " STATUS "

    ''' <summary> The operation event status. </summary>
    Private _OperationEventStatus As Integer?

    ''' <summary> Gets or sets the cached status of the Operation Register events. </summary>
    ''' <remarks> This query commands Queries the contents of the status event register. This value
    ''' indicates which bits are set. An event register bit is set to 1 when its event occurs. The
    ''' bit remains latched to 1 until the register is reset. Reading an event register clears the
    ''' bits of that register. *CLS resets all four event registers. </remarks>
    ''' <value> <c>null</c> if value is not known;. </value>
    Public Property OperationEventStatus() As Integer?
        Get
            Return Me._OperationEventStatus
        End Get
        Protected Set(ByVal value As Integer?)
            Me._OperationEventStatus = value
            Me.AsyncNotifyPropertyChanged(NameOf(Me.OperationEventStatus))
        End Set
    End Property

    ''' <summary> Gets or sets the operation event status query command. </summary>
    ''' <remarks> SCPI: ":STAT:OPER:EVEN?".
    ''' <see cref="Scpi.Syntax.OperationEventQueryCommand"> </see> </remarks>
    ''' <value> The operation event status query command. </value>
    Protected Overridable ReadOnly Property OperationEventStatusQueryCommand As String = VI.Scpi.Syntax.OperationEventQueryCommand

    ''' <summary> Queries the status of the Operation Register events. </summary>
    ''' <remarks> This query commands Queries the contents of the status event register. This value
    ''' indicates which bits are set. An event register bit is set to 1 when its event occurs. The
    ''' bit remains latched to 1 until the register is reset. Reading an event register clears the
    ''' bits of that register. *CLS resets all four event registers. </remarks>
    ''' <returns> <c>null</c> if value is not known;. </returns>
    Public Overridable Function QueryOperationEventStatus() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.OperationEventStatusQueryCommand) Then
            Me.OperationEventStatus = Me.Session.Query(0I, Me.OperationEventStatusQueryCommand)
        End If
        Return Me.OperationEventStatus
    End Function
#End Region

#End Region

#Region " QUESTIONABLE REGISTER "

#Region " BITMASK "

    ''' <summary> The questionable event enable bitmask. </summary>
    Private _QuestionableEventEnableBitmask As Integer?

    ''' <summary> Gets or sets the cached value of the Questionable register event enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the Questionable events type that is specific to
    ''' the instrument The enable register gates the corresponding events for registration by the
    ''' Status Byte register. When an event bit is set and the corresponding enable bit is set, the
    ''' output (summary) of the register will set to 1, which in turn sets the summary bit of the
    ''' Status Byte Register. </remarks>
    ''' <value> The mask to use for enabling the events. </value>
    Public Property QuestionableEventEnableBitmask() As Integer?
        Get
            Return Me._QuestionableEventEnableBitmask
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.QuestionableEventEnableBitmask, value) Then
                Me._QuestionableEventEnableBitmask = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.QuestionableEventEnableBitmask))
            End If
        End Set
    End Property

    ''' <summary> Queries the status of the Questionable Register events. </summary>
    ''' <remarks> This query commands Queries the contents of the status event register. This value
    ''' indicates which bits are set. An event register bit is set to 1 when its event occurs. The
    ''' bit remains latched to 1 until the register is reset. Reading an event register clears the
    ''' bits of that register. *CLS resets all four event registers. </remarks>
    ''' <returns> <c>null</c> if value is not known;. </returns>
    Public Overridable Function QueryQuestionableEventEnableBitmask() As Integer?
        Me.QuestionableEventEnableBitmask = Me.Session.Query(Me.QuestionableEventEnableBitmask.GetValueOrDefault(0), ":STAT:QUES:ENAB?")
        Return Me.QuestionableEventEnableBitmask
    End Function

    ''' <summary> Programs and reads back the Questionable register events enable bit mask. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The bit mask or nothing if not known. </returns>
    Public Function ApplyQuestionableEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.WriteQuestionableEventEnableBitmask(value)
        Return Me.QueryQuestionableEventEnableBitmask()
    End Function

    ''' <summary> Writes the Questionable register events enable bit mask without updating the value from
    ''' the device. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The bit mask or nothing if not known. </returns>
    Public Overridable Function WriteQuestionableEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.Session.WriteLine(":STAT:QUES:ENAB {0:D}", value)
        Me.QuestionableEventEnableBitmask = value
        Return Me.QuestionableEventEnableBitmask
    End Function

#End Region

#Region " CONDITION "

    ''' <summary> The questionable event Condition. </summary>
    Private _QuestionableEventCondition As Integer?

    ''' <summary> Gets or sets the cached value of the Questionable event register condition of the instrument. </summary>
    ''' <remarks> This value reflects the real time condition of the instrument. The returned value
    ''' could be cast to the Questionable events type that is specific to the instrument The
    ''' condition register is a real-time, read-only register that constantly updates to reflect the
    ''' present operating conditions of the instrument. </remarks>
    ''' <value> The questionable event condition. </value>
    Public Property QuestionableEventCondition() As Integer?
        Get
            Return Me._QuestionableEventCondition
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.QuestionableEventCondition, value) Then
                Me._QuestionableEventCondition = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.QuestionableEventCondition))
            End If
        End Set
    End Property

    ''' <summary> Gets the Questionable event register condition from the instrument. </summary>
    ''' <remarks> This value reflects the real time condition of the instrument. The returned value
    ''' could be cast to the Questionable events type that is specific to the instrument The
    ''' condition register is a real-time, read-only register that constantly updates to reflect the
    ''' present operating conditions of the instrument. </remarks>
    ''' <returns> The questionable event condition. </returns>
    Public Overridable Function QueryQuestionableEventCondition() As Integer?
        Me.QuestionableEventCondition = Me.Session.Query(Me.QuestionableEventCondition.GetValueOrDefault(0), ":STAT:QUES:COND?")
        Return Me.QuestionableEventCondition
    End Function

#End Region

#Region " STATUS "

    ''' <summary> The questionable event status. </summary>
    Private _QuestionableEventStatus As Integer?

    ''' <summary> Gets or sets the status of the Questionable register events. </summary>
    ''' <value> <c>null</c> if value is not known;. </value>
    Public Property QuestionableEventStatus() As Integer?
        Get
            Return Me._QuestionableEventStatus
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.QuestionableEventStatus, value) Then
                Me._QuestionableEventStatus = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.QuestionableEventStatus))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the questionable status query command. </summary>
    ''' <remarks> SCPI: ":STAT:QUES:EVEN?"
    ''' <see cref="Scpi.Syntax.QuestionableEventQueryCommand"> </see> </remarks>
    ''' <value> The questionable status query command. </value>
    Protected Overridable ReadOnly Property QuestionableStatusQueryCommand As String = VI.Scpi.Syntax.QuestionableEventQueryCommand

    ''' <summary> Reads the status of the Questionable register events. </summary>
    ''' <returns> System.Nullable{System.Int32}. </returns>
    Public Overridable Function QueryQuestionableEventStatus() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.QuestionableStatusQueryCommand) Then
            Me.QuestionableEventStatus = Me.Session.Query(0I, Me.QuestionableStatusQueryCommand)
        End If
        Return Me.QuestionableEventStatus
    End Function

#End Region

#End Region

#Region " IDENTITY "

    ''' <summary> Gets the identity query command. </summary>
    ''' <value> The identity query command. </value>
    Protected Overrides ReadOnly Property IdentityQueryCommand As String = Ieee488.Syntax.IdentityQueryCommand

    ''' <summary> Parse version information. </summary>
    ''' <param name="value"> The value. </param>
    Protected Overrides Sub ParseVersionInfo(ByVal value As String)
        MyBase.ParseVersionInfo(value)
        Me._VersionInfo = New VersionInfo
        Me.VersionInfo.Parse(value)
    End Sub

    ''' <summary> Gets or sets the information describing the version. </summary>
    ''' <value> Information describing the version. </value>
    Public Overloads ReadOnly Property VersionInfo As VersionInfo

#End Region

End Class
