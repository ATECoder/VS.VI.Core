#Region " TYPES "

''' <summary> Values that represent the resistance range mode. </summary>
Public Enum ResistanceRangeMode
    <ComponentModel.Description("Auto Range (R0)")> R0 = 0
    <ComponentModel.Description("20 ohm range @ 7.2 mA Test Current")> R20 = 20
    <ComponentModel.Description("200 ohm range @ 960 uA Test Current")> R200 = 200
    <ComponentModel.Description("2k ohm range @ 960 uA Test Current")> R2000 = 2000
    <ComponentModel.Description("20k ohm range @ 96 uA Test Current")> R20000 = 20000
    <ComponentModel.Description("200k ohm range @ 9.6 uA Test Current")> R200000 = 200000
    <ComponentModel.Description("2M ohm range @ 1.9 uA Test Current")> R2000000 = 2000000
    <ComponentModel.Description("20M ohm range @ 1.4 uA Test Current")> R20000000 = 20000000
    <ComponentModel.Description("200M ohm range @ 1.4 uA Test Current")> R200000000 = 200000000
    <ComponentModel.Description("1G ohm range @ 1.4 uA Test Current")> R1000000000 = 1000000000
End Enum

''' <summary> Values that represent the four-wire resistance range mode. </summary>
Public Enum FourWireResistanceRangeMode
    <ComponentModel.Description("Auto Range (R0)")> R0 = 0
    <ComponentModel.Description("20 ohm range @ 7.2 mA Test Current")> R20 = 20
    <ComponentModel.Description("200 ohm range @ 960 uA Test Current")> R200 = 200
    <ComponentModel.Description("2k ohm range @ 960 uA Test Current")> R2000 = 2000
    <ComponentModel.Description("20k ohm range @ 96 uA Test Current")> R20000 = 20000
    <ComponentModel.Description("200k ohm range @ 9.6 uA Test Current")> R200000 = 200000
    <ComponentModel.Description("2M ohm range @ 1.9 uA Test Current")> R2000000 = 2000000
End Enum

#End Region
