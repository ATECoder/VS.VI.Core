Namespace My

    ''' <summary> Values that represent project trace event identifiers. </summary>
    Public Enum ProjectTraceEventId
        <System.ComponentModel.Description("Not specified")> None
        <System.ComponentModel.Description("Pith")> Pith = isr.Core.Pith.My.ProjectTraceEventId.VirtualInstruments
        <System.ComponentModel.Description("NI Visa")> NationalVisa = ProjectTraceEventId.Pith + &H1
        <System.ComponentModel.Description("NI NS Visa")> NationalVisaNonstandard = ProjectTraceEventId.Pith + &H3
        <System.ComponentModel.Description("Keysight Visa")> KeysightVisa = ProjectTraceEventId.Pith + &H4
        <System.ComponentModel.Description("Dummy Visa")> DummyVisa = ProjectTraceEventId.Pith + &H5
        <System.ComponentModel.Description("Device")> Device = ProjectTraceEventId.Pith + &H6
        <System.ComponentModel.Description("Tsp Device")> TspDevice = ProjectTraceEventId.Pith + &H7
        <System.ComponentModel.Description("R2D2 Device")> DeviceR2D2 = ProjectTraceEventId.Pith + &H8
        <System.ComponentModel.Description("SCPI Device")> DeviceScpi = ProjectTraceEventId.Pith + &H9
        <System.ComponentModel.Description("Tsp2 Device")> Tsp2Device = ProjectTraceEventId.Pith + &HA

        <System.ComponentModel.Description("Instrument")> Instrument = ProjectTraceEventId.Pith + &H10
        <System.ComponentModel.Description("Tsp Instrument")> InstrumentTsp = ProjectTraceEventId.Instrument + &H1
        <System.ComponentModel.Description("R2D2 Instrument")> InstrumentR2D2 = ProjectTraceEventId.Instrument + &H2
        <System.ComponentModel.Description("SCPI Instrument")> InstrumentScpi = ProjectTraceEventId.Instrument + &H3

        <System.ComponentModel.Description("Power Supply")> PowerSupply = ProjectTraceEventId.Pith + &H20
        <System.ComponentModel.Description("Source Measure")> SourceMeasure = ProjectTraceEventId.PowerSupply + &H1
        <System.ComponentModel.Description("Scanner")> Scanner = ProjectTraceEventId.PowerSupply + &H2
        <System.ComponentModel.Description("Multimeter")> Multimeter = ProjectTraceEventId.PowerSupply + &H3
        <System.ComponentModel.Description("EG2000 Prober")> EG2000Prober = ProjectTraceEventId.PowerSupply + &H4
        <System.ComponentModel.Description("Thermostream")> Thermostream = ProjectTraceEventId.PowerSupply + &H5
        <System.ComponentModel.Description("K2000 Meter")> K2000 = ProjectTraceEventId.PowerSupply + &H6
        <System.ComponentModel.Description("K2700 Meter/Scanner")> K2700 = ProjectTraceEventId.PowerSupply + &H7
        <System.ComponentModel.Description("Tegam Meter")> Tegam = ProjectTraceEventId.PowerSupply + &H8
        <System.ComponentModel.Description("K7000 Switch")> K7000 = ProjectTraceEventId.PowerSupply + &H9
        <System.ComponentModel.Description("K3700 Meter/Scanner")> K3700 = ProjectTraceEventId.PowerSupply + &HA
        <System.ComponentModel.Description("TTM Driver")> TtmDriver = ProjectTraceEventId.PowerSupply + &HB
        <System.ComponentModel.Description("K2600 Source Meter")> K2600 = ProjectTraceEventId.PowerSupply + &HC
        <System.ComponentModel.Description("E4990 Impedance Analyzer")> E4990 = ProjectTraceEventId.PowerSupply + &HD
        <System.ComponentModel.Description("K2400 Source Meter")> K2400 = ProjectTraceEventId.PowerSupply + &HE
        <System.ComponentModel.Description("K7500 Meter")> K7500 = ProjectTraceEventId.PowerSupply + &HF
        <System.ComponentModel.Description("K34980 Meter/Scanner")> K34980 = ProjectTraceEventId.PowerSupply + &H10
        <System.ComponentModel.Description("K3458 Meter")> K3458 = ProjectTraceEventId.PowerSupply + &H11
        <System.ComponentModel.Description("K2450 Source Meter")> K2450 = ProjectTraceEventId.PowerSupply + &H12
        <System.ComponentModel.Description("K7500 TSP2 Meter")> K7500Tsp = ProjectTraceEventId.PowerSupply + &H13

        <System.ComponentModel.Description("Device Console")> DeviceConsole = ProjectTraceEventId.Pith + &H40
        <System.ComponentModel.Description("Device Tester")> DeviceTester = ProjectTraceEventId.DeviceConsole + &H1
        <System.ComponentModel.Description("Multimeter Tester")> MultimeterTester = ProjectTraceEventId.DeviceConsole + &H2
        <System.ComponentModel.Description("Ohmni Tester")> OhmniTester = ProjectTraceEventId.DeviceConsole + &H3
        <System.ComponentModel.Description("Tsp Tester")> TspTester = ProjectTraceEventId.DeviceConsole + &H4
        <System.ComponentModel.Description("Ohm Prober Tester")> OhmProberTester = ProjectTraceEventId.DeviceConsole + &H5
        <System.ComponentModel.Description("TTM Console")> TtmConsole = ProjectTraceEventId.DeviceConsole + &H6
        <System.ComponentModel.Description("K3700 Console")> K3700Console = ProjectTraceEventId.DeviceConsole + &H7
        <System.ComponentModel.Description("Gauge Console")> GaugeConsole = ProjectTraceEventId.DeviceConsole + &H8

        <System.ComponentModel.Description("Device Tests")> DeviceTests = ProjectTraceEventId.Pith + &H50
        <System.ComponentModel.Description("Try Tests")> TryTests = ProjectTraceEventId.DeviceTests + &H1
        <System.ComponentModel.Description("EG2000 Tests")> EG2000ProberTests = ProjectTraceEventId.DeviceTests + &H2
        <System.ComponentModel.Description("Thermostream Tests")> ThermostreamTests = ProjectTraceEventId.DeviceTests + &H3
        <System.ComponentModel.Description("K2000 Tests")> K2000Tests = ProjectTraceEventId.DeviceTests + &H4
        <System.ComponentModel.Description("K2700 Tests")> K2700Tests = ProjectTraceEventId.DeviceTests + &H5
        <System.ComponentModel.Description("Tegam Tests")> TegamTests = ProjectTraceEventId.DeviceTests + &H6
        <System.ComponentModel.Description("K7000 Tests")> K7000Tests = ProjectTraceEventId.DeviceTests + &H7
        <System.ComponentModel.Description("K3700 Tests")> K3700Tests = ProjectTraceEventId.DeviceTests + &H8
        <System.ComponentModel.Description("TTM Driver Tests")> TtmDriverTests = ProjectTraceEventId.DeviceTests + &H9
        <System.ComponentModel.Description("K2600 Tests")> K2600Tests = ProjectTraceEventId.DeviceTests + &HA
        <System.ComponentModel.Description("E4990 Tests")> E4990Tests = ProjectTraceEventId.DeviceTests + &HB
        <System.ComponentModel.Description("K2400 Tests")> K2400Tests = ProjectTraceEventId.DeviceTests + &HD
        <System.ComponentModel.Description("K7500 Tests")> K7500Tests = ProjectTraceEventId.DeviceTests + &HD
        <System.ComponentModel.Description("K34980 Tests")> K34980Tests = ProjectTraceEventId.DeviceTests + &HE
        <System.ComponentModel.Description("K3458 Tests")> K3458Tests = ProjectTraceEventId.DeviceTests + &HF
        <System.ComponentModel.Description("K2450 Tests")> K2450Tests = ProjectTraceEventId.DeviceTests + &H10
        <System.ComponentModel.Description("K7500 TSP2 Tests")> K7500TspTests = ProjectTraceEventId.DeviceTests + &H11

    End Enum


End Namespace

