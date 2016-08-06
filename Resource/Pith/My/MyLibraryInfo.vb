Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    ''' <remarks> David, 11/26/2015. </remarks>
    Partial Friend NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        Public Const AssemblyTitle As String = "Visa Pith Library"
        Public Const AssemblyDescription As String = "Virtual Instrument Pith Library"
        Public Const AssemblyProduct As String = "VI.Pith.2016"

    End Class

    ''' <summary> Values that represent project trace event identifiers. </summary>
    ''' <remarks> David, 11/26/2015. </remarks>
    Public Enum ProjectTraceEventId
        <System.ComponentModel.Description("Not specified")> None
        <System.ComponentModel.Description("VI Base Trace Event IO")> VI = &H70
        <System.ComponentModel.Description("Device")> Device = ProjectTraceEventId.VI *
                                                               TraceEventConstants.BaseScaleFactor +
                                                               TraceEventConstants.LibraryDigitMask + &H1
        <System.ComponentModel.Description("Tsp Device")> TspDevice = ProjectTraceEventId.Device + &H1
        <System.ComponentModel.Description("R2D2 Device")> DeviceR2D2 = ProjectTraceEventId.Device + &H2
        <System.ComponentModel.Description("SCPI Device")> DeviceScpi = ProjectTraceEventId.Device + &H3

        <System.ComponentModel.Description("Instrument")> Instrument = ProjectTraceEventId.Device + &H10
        <System.ComponentModel.Description("Tsp Instrument")> InstrumentTsp = ProjectTraceEventId.Instrument + &H1
        <System.ComponentModel.Description("R2D2 Instrument")> InstrumentR2D2 = ProjectTraceEventId.Instrument + &H2
        <System.ComponentModel.Description("SCPI Instrument")> InstrumentScpi = ProjectTraceEventId.Instrument + &H3

        <System.ComponentModel.Description("Power Supply")> PowerSupply = ProjectTraceEventId.Instrument + &H10
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

        <System.ComponentModel.Description("Core Tester")> DeviceTester = ProjectTraceEventId.VI *
                                                                          TraceEventConstants.BaseScaleFactor +
                                                                          TraceEventConstants.FormApplicationDigitMask + &H1
        <System.ComponentModel.Description("Multimeter Tester")> MultimeterTester = ProjectTraceEventId.DeviceTester + &H1
        <System.ComponentModel.Description("Ohmni Tester")> OhmniTester = ProjectTraceEventId.DeviceTester + &H2
        <System.ComponentModel.Description("Tsp Tester")> TspTester = ProjectTraceEventId.DeviceTester + &H3
        <System.ComponentModel.Description("Ohm Prober Tester")> OhmProberTester = ProjectTraceEventId.DeviceTester + &H4
        <System.ComponentModel.Description("TTM Console")> TtmConsole = ProjectTraceEventId.DeviceTester + &H5
        <System.ComponentModel.Description("K3700 Console")> K3700Console = ProjectTraceEventId.DeviceTester + &H6
        <System.ComponentModel.Description("Gauge Console")> GaugeConsole = ProjectTraceEventId.DeviceTester + &H7

        <System.ComponentModel.Description("Code Units")> CoreUnits = ProjectTraceEventId.VI *
                                                                      TraceEventConstants.BaseScaleFactor +
                                                                      TraceEventConstants.UnitTestDigitMask + &H1

    End Enum

End Namespace

