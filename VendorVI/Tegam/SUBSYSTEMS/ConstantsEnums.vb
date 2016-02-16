#Region " TYPES "

''' <summary> Values that represent Trigger Mode. </summary>
Public Enum TriggerMode
    <ComponentModel.Description("Fast Continuous (T0)")> T0 = 0
    <ComponentModel.Description("Fast One Shot (T1)")> T1 = 1
    <ComponentModel.Description("Delay Continuous (T2)")> T2 = 2
    <ComponentModel.Description("Delay One Shot (T3)")> T3 = 3
End Enum

''' <summary> Values that represent Range Mode. </summary>
Public Enum RangeMode
    <ComponentModel.Description("Auto Range (R0)")> R0 = 0
    <ComponentModel.Description("2m ohm range @ 1 A Test Current (R1)")> R1
    <ComponentModel.Description("20m ohm range @ 1 A Test Current (R2)")> R2
    <ComponentModel.Description("20m ohm range @ 100 mA Test Current (R3)")> R3
    <ComponentModel.Description("200m ohm range @ 1 A Test Current (R4)")> R4
    <ComponentModel.Description("200m ohm range @ 100 mA Test Current (R5)")> R5
    <ComponentModel.Description("2 ohm range @ 100 mA Test Current (R6)")> R6
    <ComponentModel.Description("2 ohm range @ 10 mA Test Current (R7)")> R7
    <ComponentModel.Description("20 ohm range @ 10 mA Test Current (R8)")> R8
    <ComponentModel.Description("20 ohm range @ 1 mA Test Current (R9)")> R9
    <ComponentModel.Description("200 ohm range @ 10 mA Test Current (R10)")> R10
    <ComponentModel.Description("200 ohm range @ 1 mA Test Current (R11)")> R11
    <ComponentModel.Description("200 ohm range @ 100 uA Test Current (R12)")> R12
    <ComponentModel.Description("2k ohm range @ 1 mA Test Current (R13)")> R13
    <ComponentModel.Description("2k ohm range @ 100 uA Test Current (R14)")> R14
    <ComponentModel.Description("20k ohm range @ 100 uA Test Current (R15)")> R15
    <ComponentModel.Description("20k ohm range @ 10 uA Test Current (R16)")> R16
    <ComponentModel.Description("200k ohm range @ 10 uA Test Current (R17)")> R17
    <ComponentModel.Description("2M ohm range @ 1 uA Test Current (R18)")> R18
    <ComponentModel.Description("20M ohm range @ 100 nA Test Current (R19)")> R19
End Enum

''' <summary> Values that represent Error Codes. </summary>
<Flags()>
Public Enum ErrorCodes
    <ComponentModel.Description("No Errors")> None = 0
    <ComponentModel.Description("self test fail")> SafeTestFailed = 8
    <ComponentModel.Description("illegal command")> IllegalCommand = 16
    <ComponentModel.Description("conflict")> Conflict = 32
    <ComponentModel.Description("illegal command option")> IllegalCommandOption = 64
End Enum

#End Region
