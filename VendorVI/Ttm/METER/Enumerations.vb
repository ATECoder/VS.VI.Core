Imports System.ComponentModel
''' <summary> Enumerates the measurement outcomes. </summary>
<System.Flags()> Public Enum MeasurementOutcomes
    <Description("Not Defined")> None
    <Description("Measured value is in range")> PartPassed = 1
    ''' <summary> Out of range. </summary>
    <Description("Measured values is out of range")> PartFailed = 2
    ''' <summary> Measurement Failed. </summary>
    <Description("Measurement Failed")> MeasurementFailed = 4
    ''' <summary> Measurement Not made. </summary>
    <Description("Measurement Not Made")> MeasurementNotMade = 8
    ''' <summary> Instrument failed to measure because it hit the voltage limit. </summary>
    <Description("Compliance")> HitCompliance = 16
    ''' <summary> The message received from the instrument did not parse. </summary>
    <Description("Unexpected Reading format")> UnexpectedReadingFormat = 32
    ''' <summary> The message received from the instrument did not parse. </summary>
    <Description("Unexpected Outcome format")> UnexpectedOutcomeFormat = 64
    ''' <summary> One of a number of failure occurred at the device level. </summary>
    <Description("Unspecified Status Exception")> UnspecifiedStatusException = 128
    ''' <summary> One of a number of failure occurred at the program level. </summary>
    <Description("Unspecified Program Failure")> UnspecifiedProgramFailure = 256
    ''' <summary> Device Error. </summary>
    <Description("Device Error")> DeviceError = 512
    <Description("Failed Contact Check")> FailedContactCheck = 1024
    <Description("Undefined4")> Undefined4 = 2048
    <Description("Undefined5")> Undefined5 = 4096
    <Description("Undefined6")> Undefined6 = 8192
    ''' <summary> Unknown outcome - assert. </summary>
    <Description("Unknown outcome - assert")> UnknownOutcome = 2048 + 4096 + 8192
End Enum

''' <summary>Enumerates the display screens and special status.</summary>
<Flags()> Public Enum DisplayScreens

    ''' <summary> Not defined. </summary>
    <Description("Not defined")> None = 0

    ''' <summary> Custom lines are displayed. </summary>
    <Description("Default screen")> [Default] = 1

    ''' <summary> Cleared user screen mode. </summary>
    <Description("User screen")> User = 128

    ''' <summary> Last command displayed title. </summary>
    <Description("Title is displayed")> Title = 256

    ''' <summary> Custom lines are displayed. </summary>
    <Description("Special display")> Custom = 512

    ''' <summary> Measurement displayed. </summary>
    <Description("Measurement is displayed")> Measurement = 1024

End Enum

''' <summary> Specifies the contact check speed modes. </summary>
Public Enum ContactCheckSpeedMode
    <Description("None")> None
    <Description("Fast (CONTACT_FAST)")> Fast
    <Description("Medium (CONTACT_MEDIUM)")> Medium
    <Description("Slow (CONTACT_SLOW)")> Slow
End Enum

''' <summary> Values that represent ThermalTransientMeterEntity. </summary>
Public Enum ThermalTransientMeterEntity
    <Description("None")> None = 0
    ''' <summary> Initial resistance entity. </summary>
    <Description("Initial Resistance (_G.ttm.ir)")> InitialResistance
    ''' <summary> Final resistance entity. </summary>
    <Description("Initial Resistance (_G.ttm.fr)")> FinalResistance
    ''' <summary> Thermal Transient entity. </summary>
    <Description("Thermal Transient (_G.ttm.tr)")> Transient
    ''' <summary> Thermal Transient Estimator. </summary>
    <Description("Thermal Transient Estimator (_G.ttm.est)")> Estimator
    ''' <summary> Shunt resistance entity. </summary>
    <Description("Shunt Resistance (_G)")> Shunt
End Enum

