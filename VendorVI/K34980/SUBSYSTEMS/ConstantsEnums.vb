#Region " TYPES "

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

#End Region
