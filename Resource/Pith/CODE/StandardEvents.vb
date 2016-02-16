Imports System.ComponentModel
''' <summary> Enumerates the status byte flags of the standard event register. </summary>
''' <remarks> Enumerates the Standard Event Status Register Bits.
''' Read this information using ESR? or status.standard.event.
''' Use *ESE or status.standard.enable or event status enable 
''' to enable this register.''' These values are used when reading or writing to the
''' standard event registers. Reading a status register returns a value.
''' The binary equivalent of the returned value indicates which register bits
''' are set. The least significant bit of the binary number is bit 0, and
''' the most significant bit is bit 15. For example, assume value 9 is
''' returned for the enable register. The binary equivalent is
''' 0000000000001001. This value indicates that bit 0 (OPC) and bit 3 (DDE)
''' are set.
''' </remarks>
<System.Flags()> Public Enum StandardEvents
    <Description("None")> None = 0
    ''' <summary>
    ''' Bit B0, Operation Complete (OPC). Set bit indicates that all
    ''' pending selected device operations are completed and the unit is ready to
    ''' accept new commands. The bit is set in response to an *OPC command.
    ''' The ICL function OPC() can be used in place of the *OPC command.
    ''' </summary>
    <Description("Operation Complete (OPC)")> OperationComplete = 1
    ''' <summary>
    ''' Bit B1, Request Control (RQC). Set bit indicates that....
    ''' </summary>
    <Description("Request Control (RQC)")> RequestControl = &H2
    ''' <summary>
    ''' Bit B2, Query Error (QYE). Set bit indicates that you attempted
    ''' to read data from an empty Output Queue.
    ''' </summary>
    <Description("Query Error (QYE)")> QueryError = &H4
    ''' <summary>
    ''' Bit B3, Device-Dependent Error (DDE). Set bit indicates that a
    ''' device operation did not execute properly due to some internal
    ''' condition.
    ''' </summary>
    <Description("Device Dependent Error (DDE)")> DeviceDependentError = &H8
    ''' <summary>
    ''' Bit B4 (16), Execution Error (EXE). Set bit indicates that the unit
    ''' detected an error while trying to execute a command. 
    ''' This is used by QUATECH to report No Contact.
    ''' </summary>
    <Description("Execution Error (EXE)")> ExecutionError = &H10
    ''' <summary>
    ''' Bit B5 (32), Command Error (CME). Set bit indicates that a
    ''' command error has occurred. Command errors include:<p>
    ''' IEEE-488.2 syntax error — unit received a message that does not follow
    ''' the defined syntax of the IEEE-488.2 standard.  </p><p>
    ''' Semantic error — unit received a command that was misspelled or received
    ''' an optional IEEE-488.2 command that is not implemented.  </p><p>
    ''' The device received a Group Execute Trigger (GET) inside a program
    ''' message.  </p>
    ''' </summary>
    <Description("Command Error (CME)")> CommandError = &H20
    ''' <summary>
    ''' Bit B6 (64), User Request (URQ). Set bit indicates that the LOCAL
    ''' key on the SourceMeter front panel was pressed.
    ''' </summary>
    <Description("User Request (URQ)")> UserRequest = &H40
    ''' <summary>
    ''' Bit B7 (128), Power ON (PON). Set bit indicates that the device
    ''' has been turned off and turned back on since the last time this register
    ''' has been read.
    ''' </summary>
    <Description("Power Toggled (PON)")> PowerToggled = &H80
    ''' <summary>
    ''' Unknown value due to, for example, error trying to get value from the device.
    ''' </summary>
    <Description("Unknown")> Unknown = &H100
    ''' <summary>Includes all bits.
    ''' </summary>
    <Description("All")> All = &HFF ' 255
End Enum
