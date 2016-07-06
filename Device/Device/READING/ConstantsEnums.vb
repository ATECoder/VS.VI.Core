#Region " MEASURAND-RELATED TYPES "

Public Module Measurand
    ''' <summary> The level compliance bit. </summary>
    Public Const LevelComplianceBit As Integer = 32
    ''' <summary> The meta status bits base. </summary>
    Public Const MetaStatusBitBase As Integer = LevelComplianceBit + 1
End Module

''' <summary>
''' Holds the measurand status bits.
''' </summary>
''' <remarks> Based above 32 bits so that these can be added to the extended 64 bit status word which lower 32 bits
'''           hold the standard status word.
'''          </remarks>
Public Enum MetaStatusBit
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Valid")> Valid = Measurand.MetaStatusBitBase
    <ComponentModel.Description("Has Value")> HasValue = Measurand.MetaStatusBitBase + 1
    <ComponentModel.Description("Not a number")> NotANumber = Measurand.MetaStatusBitBase + 2
    <ComponentModel.Description("Infinity")> Infinity = Measurand.MetaStatusBitBase + 3
    <ComponentModel.Description("Negative Infinity")> NegativeInfinity = Measurand.MetaStatusBitBase + 4
    <ComponentModel.Description("Hit Status Compliance")> HitStatusCompliance = Measurand.MetaStatusBitBase + 5
    <ComponentModel.Description("Hit Level Compliance")> HitLevelCompliance = Measurand.MetaStatusBitBase + 6
    <ComponentModel.Description("Hit Range Compliance")> HitRangeCompliance = Measurand.MetaStatusBitBase + 7
    <ComponentModel.Description("Failed Contact Check")> FailedContactCheck = Measurand.MetaStatusBitBase + 8
    <ComponentModel.Description("Hit Voltage Protection")> HitVoltageProtection = Measurand.MetaStatusBitBase + 9
    <ComponentModel.Description("Measured while over range")> HitOverRange = Measurand.MetaStatusBitBase + 10
    <ComponentModel.Description("Pass")> Pass = Measurand.MetaStatusBitBase + 11
    <ComponentModel.Description("High")> High = Measurand.MetaStatusBitBase + 12
    <ComponentModel.Description("Low")> Low = Measurand.MetaStatusBitBase + 13
End Enum

''' <summary>
''' Enumerates reading types the instrument is capable of.
''' </summary>
<System.Flags()>
Public Enum ReadingTypes
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Reading (READ)")> Reading = 1
    <ComponentModel.Description("Time Stamp (TST)")> Timestamp = Reading << 1
    <ComponentModel.Description("Units (UNIT)")> Units = Timestamp << 1
    <ComponentModel.Description("Reading Number (RNUM)")> ReadingNumber = Units << 1
    <ComponentModel.Description("Source (SOUR)")> Source = ReadingNumber << 1
    <ComponentModel.Description("Compliance (COMP)")> Compliance = Source << 1
    <ComponentModel.Description("Average Voltage (AVOL)")> AverageVoltage = Compliance << 1
    <ComponentModel.Description("Voltage (VOLT)")> Voltage = AverageVoltage << 1
    <ComponentModel.Description("Current (CURR)")> Current = Voltage << 1
    <ComponentModel.Description("Resistance (RES)")> Resistance = Current << 1
    <ComponentModel.Description("Time (TIME)")> Time = Resistance << 1
    <ComponentModel.Description("Status (STAT)")> Status = Time << 1
    <ComponentModel.Description("Channel (CHAN)")> Channel = Status << 1
    <ComponentModel.Description("Limits (LIM)")> Limits = Channel << 1
    <ComponentModel.Description("Seconds (SEC)")> Seconds = Limits << 1
End Enum

#End Region
