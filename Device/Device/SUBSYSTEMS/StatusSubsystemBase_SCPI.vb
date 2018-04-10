Imports isr.VI.ExceptionExtensions
Partial Public Class StatusSubsystemBase

#Region " DEVICE REGISTERS "

    ''' <summary> Sets the subsystem to its preset state. </summary>
    Private Sub PresetRegistersKnownState()
        Me.OperationEventEnableBitmask = 0
        Me.QuestionableEventEnableBitmask = 0
        Me._OperationEventMap.Clear()
    End Sub

    ''' <summary> Sets the subsystem registers to their reset state. </summary>
    Private Sub ResetRegistersKnownState()
        Me.MeasurementEventEnableBitmask = 0
        Me.MeasurementEventStatus = 0
        Me.OperationEventEnableBitmask = 0
        Me.OperationEventStatus = 0
        Me.QuestionableEventEnableBitmask = 0
        Me.QuestionableEventStatus = 0
        Me.StandardEventStatus = 0
        Me.StandardEventEnableBitmask = 0
    End Sub

    ''' <summary> Reads the event registers based on the service request register status. </summary>
    Private Sub _ReadEventRegisters()
        If Me.StandardEventAvailable Then Me.QueryStandardEventStatus()
        If Me.MeasurementAvailable Then Me.QueryMeasurementEventStatus()
        If (Me.ServiceRequestStatus And VI.Pith.ServiceRequests.OperationEvent) <> 0 Then Me.QueryOperationEventStatus()
        If (Me.ServiceRequestStatus And VI.Pith.ServiceRequests.QuestionableEvent) <> 0 Then Me.QueryQuestionableEventStatus()
    End Sub

#End Region

#Region " MEASUREMENT REGISTER "

#Region " ENABLE BITMASK "

    ''' <summary> The Measurement event enable bitmask. </summary>
    Private _MeasurementEventEnableBitmask As Integer?

    ''' <summary> Gets or sets the cached value of the Measurement register event enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the Measurement events type that is specific to the
    ''' instrument The enable register gates the corresponding events for registration by the Status
    ''' Byte register. When an event bit is set and the corresponding enable bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <value> The mask to use for enabling the events. </value>
    Public Property MeasurementEventEnableBitmask() As Integer?
        Get
            Return Me._MeasurementEventEnableBitmask
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.MeasurementEventEnableBitmask, value) Then
                Me._MeasurementEventEnableBitmask = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Programs and reads back the Measurement register events enable bit mask. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The bit mask or nothing if not known. </returns>
    Public Function ApplyMeasurementEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.WriteMeasurementEventEnableBitmask(value)
        Return Me.QueryMeasurementEventEnableBitmask()
    End Function

    ''' <summary> Gets the Measurement event enable query command. </summary>
    ''' <remarks> SCPI: ":STAT:OPER:ENAB?". </remarks>
    ''' <value> The Measurement event enable command format. </value>
    Protected Overridable ReadOnly Property MeasurementEventEnableQueryCommand As String = VI.Pith.Scpi.Syntax.MeasurementEventEnableQueryCommand

    ''' <summary> Queries the Measurement register event enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the Measurement events type that is specific to the
    ''' instrument The enable register gates the corresponding events for registration by the Status
    ''' Byte register. When an event bit is set and the corresponding enable bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <returns> The mask to use for enabling the events; nothing if unknown </returns>
    Public Function QueryMeasurementEventEnableBitmask() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.MeasurementEventEnableQueryCommand) Then
            Me.MeasurementEventEnableBitmask = Me.Session.Query(0I, Me.MeasurementEventEnableQueryCommand)
        End If
        Return Me.MeasurementEventEnableBitmask
    End Function

    ''' <summary> Gets or sets the Measurement event enable command format. </summary>
    ''' <remarks> SCPI: ":STAT:OPER:ENAB {0:D}".
    ''' <see cref="VI.Pith.Scpi.Syntax.MeasurementEventEnableCommandFormat"> </see> </remarks>
    ''' <value> The Measurement event enable command format. </value>
    Protected Overridable ReadOnly Property MeasurementEventEnableCommandFormat As String = VI.Pith.Scpi.Syntax.MeasurementEventEnableCommandFormat

    ''' <summary> Programs the Measurement register event enable bit mask. </summary>
    ''' <remarks> When an event bit is set and the corresponding enable bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <returns> The mask to use for enabling the events; nothing if unknown </returns>
    Public Overridable Function WriteMeasurementEventEnableBitmask(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.MeasurementEventEnableCommandFormat) Then
            Me.Session.WriteLine(Me.MeasurementEventEnableCommandFormat, value)
        End If
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
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets the measurement event condition query command. </summary>
    ''' <remarks> SCPI: ":STAT:MEAS:COND?".
    ''' <see cref="VI.Pith.Scpi.Syntax.MeasurementEventConditionQueryCommand"> </see> </remarks>
    ''' <value> The measurement event condition query command. </value>
    Protected Overridable ReadOnly Property MeasurementEventConditionQueryCommand As String = VI.Pith.Scpi.Syntax.MeasurementEventConditionQueryCommand

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
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Gets the measurement status query command. </summary>
    ''' <remarks> SCPI: ":STAT:MEAS:EVEN?".
    ''' <see cref="VI.Pith.Scpi.Syntax.MeasurementEventQueryCommand"> </see> </remarks>
    ''' <value> The measurement status query command. </value>
    Protected Overridable ReadOnly Property MeasurementStatusQueryCommand As String = VI.Pith.Scpi.Syntax.MeasurementEventQueryCommand

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

#Region " MAP EVENT NUMBERS "

    ''' <summary> Gets or sets the buffer 0% filled event number. </summary>
    ''' <value> The buffer empty event number. </value>
    Public Property BufferEmptyEventNumber As Integer = 4916

    ''' <summary> Gets or sets the buffer 100% full event number. </summary>
    ''' <value> The buffer full event number. </value>
    Public Property BufferFullEventNumber As Integer = 4917

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
                Me.SafePostPropertyChanged()
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
    Protected Overridable ReadOnly Property OperationEventEnableQueryCommand As String = VI.Pith.Scpi.Syntax.OperationEventEnableQueryCommand

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
    ''' <see cref="VI.Pith.Scpi.Syntax.OperationEventEnableCommandFormat"> </see> </remarks>
    ''' <value> The operation event enable command format. </value>
    Protected Overridable ReadOnly Property OperationEventEnableCommandFormat As String = VI.Pith.Scpi.Syntax.OperationEventEnableCommandFormat

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
                Me.SafePostPropertyChanged()
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
                Me.SafePostPropertyChanged()
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
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets the operation event enable query command. </summary>
    ''' <remarks> SCPI: ":STAT:OPER:ENAB?". </remarks>
    ''' <value> The operation event enable command format. </value>
    Protected Overridable ReadOnly Property OperationEventConditionQueryCommand As String = VI.Pith.Scpi.Syntax.OperationEventEnableQueryCommand


    ''' <summary> Queries the Operation event register condition from the instrument. </summary>
    ''' <remarks> This value reflects the real time condition of the instrument. The returned value
    ''' could be cast to the Operation events type that is specific to the instrument The condition
    ''' register is a real-time, read-only register that constantly updates to reflect the present
    ''' operating conditions of the instrument. </remarks>
    ''' <returns> The operation event condition. </returns>
    Public Overridable Function QueryOperationEventCondition() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.OperationEventConditionQueryCommand) Then
            Me.OperationEventCondition = Me.Session.Query(Me.OperationEventCondition.GetValueOrDefault(0), Me.OperationEventConditionQueryCommand)
        End If
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
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Gets or sets the operation event status query command. </summary>
    ''' <remarks> SCPI: ":STAT:OPER:EVEN?".
    ''' <see cref="VI.Pith.Scpi.Syntax.OperationEventQueryCommand"> </see> </remarks>
    ''' <value> The operation event status query command. </value>
    Protected Overridable ReadOnly Property OperationEventStatusQueryCommand As String = VI.Pith.Scpi.Syntax.OperationEventQueryCommand

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

#Region " MAP "

    ''' <summary> The operation event Map. </summary>
    Private _OperationEventMap As Dictionary(Of Integer, String)

    ''' <summary> Gets or sets the cached Map of the Operation Register Map events. </summary>
    Public Property OperationEventMap(ByVal bitNumber As Integer) As String
        Get
            If Me._OperationEventMap.ContainsKey(bitNumber) Then
                Return Me._OperationEventMap(bitNumber)
            Else
                Return ""
            End If
        End Get
        Protected Set(ByVal value As String)
            If Me._OperationEventMap.ContainsKey(bitNumber) Then
                Me._OperationEventMap(bitNumber) = value
            Else
                Me._OperationEventMap.Add(bitNumber, value)
            End If
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Programs and reads back the Operation register events Map. </summary>
    ''' <returns> The event map or nothing if not known. </returns>
    Public Function ApplyOperationEventMap(ByVal bitNumber As Integer, ByVal setEventNumber As Integer, ByVal clearEventNumber As Integer) As String
        Me.WriteOperationEventMap(bitNumber, setEventNumber, clearEventNumber)
        Return Me.QueryOperationEventMap(bitNumber)
    End Function


    ''' <summary> Gets or sets the operation event Map query command. </summary>
    ''' <remarks> SCPI: ":STAT:OPER:MAP? {0:D}".
    ''' <see cref="VI.Pith.Scpi.Syntax.OperationEventQueryCommand"> </see> </remarks>
    ''' <value> The operation event Map query command. </value>
    Protected Overridable ReadOnly Property OperationEventMapQueryCommand As String = VI.Pith.Scpi.Syntax.OperationEventMapQueryCommandFormat

    ''' <summary> Queries the Map of the Operation Register events. </summary>
    ''' <remarks> This query commands Queries the contents of the Map event register. This value
    ''' indicates which bits are set. An event register bit is set to 1 when its event occurs. The
    ''' bit remains latched to 1 until the register is reset. Reading an event register clears the
    ''' bits of that register. *CLS resets all four event registers. </remarks>
    ''' <returns> The event map or nothing if not known. </returns>
    Public Overridable Function QueryOperationEventMap(ByVal bitNumber As Integer) As String
        If Not String.IsNullOrWhiteSpace(Me.OperationEventMapQueryCommand) Then
            Me.OperationEventMap(bitNumber) = Me.Session.QueryTrimEnd(String.Format(Me.OperationEventMapQueryCommand, bitNumber))
        End If
        Return Me.OperationEventMap(bitNumber)
    End Function

    ''' <summary> Gets or sets the operation event Map command format. </summary>
    ''' <remarks> SCPI: ":STAT:OPER:MAP {0:D},{1:D},{2:D}".
    ''' <see cref="VI.Pith.Scpi.Syntax.OperationEventMapCommandFormat"> </see> </remarks>
    ''' <value> The operation event Map command format. </value>
    Protected Overridable ReadOnly Property OperationEventMapCommandFormat As String = VI.Pith.Scpi.Syntax.OperationEventMapCommandFormat

    ''' <summary> Programs the Operation register event Map bit mask. </summary>
    ''' <remarks> When an event bit is set and the corresponding Map bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <returns> The event map or nothing if not known. </returns>
    Public Overridable Function WriteOperationEventMap(ByVal bitNumber As Integer, ByVal setEventNumber As Integer, ByVal clearEventNumber As Integer) As String
        If Not String.IsNullOrWhiteSpace(Me.OperationEventMapCommandFormat) Then
            Me.Session.WriteLine(Me.OperationEventMapCommandFormat, bitNumber, setEventNumber, clearEventNumber)
        End If
        Me.OperationEventMap(bitNumber) = $"{setEventNumber},{clearEventNumber}"
        Return Me.OperationEventMap(bitNumber)
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
                Me.SafePostPropertyChanged()
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
                Me.SafePostPropertyChanged()
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
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the questionable status query command. </summary>
    ''' <remarks> SCPI: ":STAT:QUES:EVEN?"
    ''' <see cref="VI.Pith.Scpi.Syntax.QuestionableEventQueryCommand"> </see> </remarks>
    ''' <value> The questionable status query command. </value>
    Protected Overridable ReadOnly Property QuestionableStatusQueryCommand As String = VI.Pith.Scpi.Syntax.QuestionableEventQueryCommand

    ''' <summary> Reads the status of the Questionable register events. </summary>
    ''' <returns> System.Nullable{System.Int32}. </returns>
    Public Overridable Function QueryQuestionableEventStatus() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.QuestionableStatusQueryCommand) Then
            Me.QuestionableEventStatus = Me.Session.Query(0I, Me.QuestionableStatusQueryCommand)
        End If
        Return Me.QuestionableEventStatus
    End Function

#End Region

#Region " MAP "

    ''' <summary> The Questionable event Map. </summary>
    Private _QuestionableEventMap As New Dictionary(Of Integer, String)

    ''' <summary> Gets or sets the cached Map of the Questionable Register Map events. </summary>
    Public Property QuestionableEventMap(ByVal bitNumber As Integer) As String
        Get
            If Me._QuestionableEventMap.ContainsKey(bitNumber) Then
                Return Me._QuestionableEventMap(bitNumber)
            Else
                Return ""
            End If
        End Get
        Protected Set(ByVal value As String)
            If Me._QuestionableEventMap.ContainsKey(bitNumber) Then
                Me._QuestionableEventMap(bitNumber) = value
            Else
                Me._QuestionableEventMap.Add(bitNumber, value)
            End If
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Programs and reads back the Questionable register events Map. </summary>
    ''' <returns> The event map or nothing if not known. </returns>
    Public Function ApplyQuestionableEventMap(ByVal bitNumber As Integer, ByVal setEventNumber As Integer, ByVal clearEventNumber As Integer) As String
        Me.WriteQuestionableEventMap(bitNumber, setEventNumber, clearEventNumber)
        Return Me.QueryQuestionableEventMap(bitNumber)
    End Function


    ''' <summary> Gets or sets the Questionable event Map query command. </summary>
    ''' <remarks> SCPI: ":STAT:QUES:MAP? {0:D}".
    ''' <see cref="VI.Pith.Scpi.Syntax.QuestionableEventQueryCommand"> </see> </remarks>
    ''' <value> The Questionable event Map query command. </value>
    Protected Overridable ReadOnly Property QuestionableEventMapQueryCommand As String = VI.Pith.Scpi.Syntax.QuestionableEventMapQueryCommandFormat

    ''' <summary> Queries the Map of the Questionable Register events. </summary>
    ''' <remarks> This query commands Queries the contents of the Map event register. This value
    ''' indicates which bits are set. An event register bit is set to 1 when its event occurs. The
    ''' bit remains latched to 1 until the register is reset. Reading an event register clears the
    ''' bits of that register. *CLS resets all four event registers. </remarks>
    ''' <returns> The event map or nothing if not known. </returns>
    Public Overridable Function QueryQuestionableEventMap(ByVal bitNumber As Integer) As String
        If Not String.IsNullOrWhiteSpace(Me.QuestionableEventMapQueryCommand) Then
            Me.QuestionableEventMap(bitNumber) = Me.Session.QueryTrimEnd(String.Format(Me.QuestionableEventMapQueryCommand, bitNumber))
        End If
        Return Me.QuestionableEventMap(bitNumber)
    End Function

    ''' <summary> Gets or sets the Questionable event Map command format. </summary>
    ''' <remarks> SCPI: ":STAT:QUES:MAP {0:D},{1:D},{2:D}".
    ''' <see cref="VI.Pith.Scpi.Syntax.QuestionableEventMapCommandFormat"> </see> </remarks>
    ''' <value> The Questionable event Map command format. </value>
    Protected Overridable ReadOnly Property QuestionableEventMapCommandFormat As String = VI.Pith.Scpi.Syntax.QuestionableEventMapCommandFormat

    ''' <summary> Programs the Questionable register event Map bit mask. </summary>
    ''' <remarks> When an event bit is set and the corresponding Map bit is set, the output
    ''' (summary) of the register will set to 1, which in turn sets the summary bit of the Status
    ''' Byte Register. </remarks>
    ''' <returns> The event map or nothing if not known. </returns>
    Public Overridable Function WriteQuestionableEventMap(ByVal bitNumber As Integer, ByVal setEventNumber As Integer, ByVal clearEventNumber As Integer) As String
        If Not String.IsNullOrWhiteSpace(Me.QuestionableEventMapCommandFormat) Then
            Me.Session.WriteLine(Me.QuestionableEventMapCommandFormat, bitNumber, setEventNumber, clearEventNumber)
        End If
        Me.QuestionableEventMap(bitNumber) = $"{setEventNumber},{clearEventNumber}"
        Return Me.QuestionableEventMap(bitNumber)
    End Function

#End Region

#End Region

End Class
