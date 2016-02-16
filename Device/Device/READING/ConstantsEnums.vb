#Region " MEASURAND-RELATED TYPES "

Public Module Measurand
    ''' <summary> The meta status bits base. </summary>
    Public Const MetaStatusBitsBase As Long = CLng(2 ^ 32)
End Module

''' <summary>
''' Holds the measurand status bits.
''' </summary>
''' <remarks> Based above 32 bits so that these can be added to the extended 64 bit status word which lower 32 bits
'''           hold the standard status word.
'''          </remarks>
<System.Flags()>
Public Enum MetaStatusBits As Long
    <ComponentModel.Description("None")> None = CLng(0)
    <ComponentModel.Description("Valid")> Valid = CLng(2 ^ 0) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("Has Value")> HasValue = CLng(2 ^ 1) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("Not a number")> NotANumber = CLng(2 ^ 2) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("Infinity")> Infinity = CLng(2 ^ 3) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("Negative Infinity")> NegativeInfinity = CLng(2 ^ 4) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("Hit Status Compliance")> HitStatusCompliance = CLng(2 ^ 5) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("Hit Level Compliance")> HitLevelCompliance = CLng(2 ^ 6) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("Hit Range Compliance")> HitRangeCompliance = CLng(2 ^ 7) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("Failed Contact Check")> FailedContactCheck = CLng(2 ^ 8) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("Pass")> Pass = CLng(2 ^ 9) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("High")> High = CLng(2 ^ 10) * Measurand.MetaStatusBitsBase
    <ComponentModel.Description("Low")> Low = CLng(2 ^ 11) * Measurand.MetaStatusBitsBase
End Enum

''' <summary>
''' Enumerates reading elements the instrument is capable of.
''' </summary>
<System.Flags()>
Public Enum ReadingElements
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Reading (READ)")> Reading = 1
    <ComponentModel.Description("Time Stamp (TST)")> Timestamp = 2 * Reading
    <ComponentModel.Description("Units (UNIT)")> Units = 2 * Timestamp
    <ComponentModel.Description("Reading Number (RNUM)")> ReadingNumber = 2 * Units
    <ComponentModel.Description("Source (SOUR)")> Source = 2 * ReadingNumber
    <ComponentModel.Description("Compliance (COMP)")> Compliance = 2 * Source
    <ComponentModel.Description("Average Voltage (AVOL)")> AverageVoltage = 2 * Compliance
    <ComponentModel.Description("Voltage (VOLT)")> Voltage = 2 * AverageVoltage
    <ComponentModel.Description("Current (CURR)")> Current = 2 * Voltage
    <ComponentModel.Description("Resistance (RES)")> Resistance = 2 * Current
    <ComponentModel.Description("Time (TIME)")> Time = 2 * Resistance
    <ComponentModel.Description("Status (STAT)")> Status = 2 * Time
    <ComponentModel.Description("Channel (CHAN)")> Channel = 2 * Status
    <ComponentModel.Description("Limits (LIM)")> Limits = 2 * Channel
End Enum

#End Region
