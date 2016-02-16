Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Sense Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific ReSenses, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SenseSubsystemBase
    Inherits VI.SenseSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.MeasurementAvailable = False
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FunctionMode = SenseFunctionModes.VoltageDC
    End Sub

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Returns the <see cref="SenseFunctionModes"></see> from the specified value. </summary>
    ''' <param name="value"> The Modes. </param>
    ''' <returns> The sense function mode. </returns>
    Public Shared Function ParseSenseFunctionMode(ByVal value As String) As SenseFunctionModes
        If String.IsNullOrWhiteSpace(value) Then
            Return SenseFunctionModes.None
        Else
            Dim se As New StringEnumerator(Of SenseFunctionModes)
            Return se.ParseContained(value.BuildDelimitedValue)
        End If
    End Function

    ''' <summary> The Sense Function Mode. </summary>
    Private _FunctionMode As SenseFunctionModes?

    ''' <summary> Gets or sets the cached Sense Function Mode. </summary>
    ''' <value> The Function Mode or null if unknown. </value>
    Public Property FunctionMode As SenseFunctionModes?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As SenseFunctionModes?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.FunctionMode))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sense Function Mode. </summary>
    ''' <param name="value"> The <see cref="SenseFunctionModes">Function Mode</see>. </param>
    ''' <returns> The Sense Function Mode or null if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As SenseFunctionModes) As SenseFunctionModes?
        Me.WriteFunctionMode(value)
        Return Me.QueryFunctionMode()
    End Function

    ''' <summary> Queries the Sense Function Mode. Also sets the <see cref="FunctionMode"></see> cached value. </summary>
    ''' <returns> The Sense Function mode or null if unknown. </returns>
    Public Function QueryFunctionMode() As SenseFunctionModes?
        ' the instrument expects single quotes when writing the value but sends back items delimited with double quotes.
        Me.FunctionMode = SenseSubsystemBase.ParseSenseFunctionMode(Me.Session.QueryTrimEnd(":SENS:FUNC?").Replace(CChar(""""), "'"c))
        Return Me.FunctionMode
    End Function

    ''' <summary> Writes the Sense Function mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Function mode. </param>
    ''' <returns> The Sense Function mode or null if unknown. </returns>
    Public Function WriteFunctionMode(ByVal value As SenseFunctionModes) As SenseFunctionModes?
        Me.Session.WriteLine(":SENS:FUNC {0}", value.ExtractBetween())
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

    Private _SupportedFunctionModes As SenseFunctionModes
    ''' <summary>
    ''' Gets or sets the supported Function Mode.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedFunctionModes() As SenseFunctionModes
        Get
            Return _SupportedFunctionModes
        End Get
        Set(ByVal value As SenseFunctionModes)
            If Not Me.SupportedFunctionModes.Equals(value) Then
                Me._SupportedFunctionModes = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SupportedFunctionModes))
            End If
        End Set
    End Property

#End Region

#Region " LATEST DATA "

    ''' <summary> Gets or sets the latest data query command. </summary>
    ''' <value> The latest data query command. </value>
    ''' <remarks> SCPI: ":SENSE:DATA:LAT?" </remarks>
    Protected Overridable ReadOnly Property LatestDataQueryCommand As String

    ''' <summary> Fetches the latest data and parses it. </summary>
    ''' <remarks> Issues the ':SENSE:DATA:LAT?' query, which reads data stored in the Sample Buffer.. </remarks>
    Public Overrides Sub FetchLatestData()
        If Not String.IsNullOrWhiteSpace(Me.LatestDataQueryCommand) Then
            Me.LastReading = Me.Session.QueryTrimEnd(LatestDataQueryCommand)
        End If
        If Not String.IsNullOrWhiteSpace(Me.LastReading) Then
            ' the emulator will set the last reading. 
            Me.ParseReading(Me.LastReading)
            Me.MeasurementAvailable = True
        End If
    End Sub

#End Region

#Region " UNITS "

    Private Shared _UnitParserHash As Dictionary(Of SenseFunctionModes, Arebis.TypedUnits.Unit)
    ''' <summary> Builds Unit Parser hash. </summary>
    ''' <returns> A Dictionary for translating SCPI unit names to <see cref="Arebis.StandardUnits">standard units</see>. </returns>
    Public Shared Function BuildUnitParserHash() As Dictionary(Of SenseFunctionModes, Arebis.TypedUnits.Unit)
        Dim dix2 As New Dictionary(Of SenseFunctionModes, Arebis.TypedUnits.Unit)
        Dim dix3 As Dictionary(Of SenseFunctionModes, Arebis.TypedUnits.Unit) = dix2
        dix3.Add(SenseFunctionModes.Current, Arebis.StandardUnits.ElectricUnits.Ampere)
        dix3.Add(SenseFunctionModes.CurrentAC, Arebis.StandardUnits.ElectricUnits.Ampere)
        dix3.Add(SenseFunctionModes.CurrentDC, Arebis.StandardUnits.ElectricUnits.Ampere)
        dix3.Add(SenseFunctionModes.FourWireResistance, Arebis.StandardUnits.ElectricUnits.Ohm)
        dix3.Add(SenseFunctionModes.Frequency, Arebis.StandardUnits.FrequencyUnits.Hertz)
        dix3.Add(SenseFunctionModes.Period, Arebis.StandardUnits.TimeUnits.Second)
        dix3.Add(SenseFunctionModes.Resistance, Arebis.StandardUnits.ElectricUnits.Ohm)
        dix3.Add(SenseFunctionModes.Temperature, Arebis.StandardUnits.TemperatureUnits.DegreeCelsius)
        dix3.Add(SenseFunctionModes.Voltage, Arebis.StandardUnits.ElectricUnits.Volt)
        dix3.Add(SenseFunctionModes.VoltageAC, Arebis.StandardUnits.ElectricUnits.Volt)
        dix3.Add(SenseFunctionModes.VoltageDC, Arebis.StandardUnits.ElectricUnits.Volt)
        dix3 = Nothing
        Return dix2
    End Function

    ''' <summary> Parses the Function Mode to create the corresponding units. 
    '''           Assume a single function mode is defined. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <param name="unit">         [in,out] The unit. </param>
    ''' <returns> <c>True</c> if parsed. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Overloads Shared Function TryParse(ByVal functionMode As SenseFunctionModes, ByRef unit As Arebis.TypedUnits.Unit) As Boolean
        If SenseSubsystemBase._UnitParserHash Is Nothing Then
            SenseSubsystemBase._UnitParserHash = SenseSubsystemBase.BuildUnitParserHash
        End If
        If SenseSubsystemBase._UnitParserHash.ContainsKey(functionMode) Then
            unit = SenseSubsystemBase._UnitParserHash(functionMode)
        Else
            unit = Nothing
        End If
        Return unit IsNot Nothing
    End Function

#End Region

End Class

''' <summary>Specifies the sense function modes.</summary>
<System.Flags()> Public Enum SenseFunctionModes
    <ComponentModel.Description("Not specified")> None = 0
    <ComponentModel.Description("Voltage ('VOLT')")> Voltage = 1
    <ComponentModel.Description("Current ('CURR')")> Current = 2 * SenseFunctionModes.Voltage
    <ComponentModel.Description("DC Voltage ('VOLT:DC')")> VoltageDC = 2 * SenseFunctionModes.Current
    <ComponentModel.Description("DC Current ('CURR:DC')")> CurrentDC = 2 * SenseFunctionModes.VoltageDC
    <ComponentModel.Description("AC Voltage ('VOLT:AC')")> VoltageAC = 2 * SenseFunctionModes.CurrentDC
    <ComponentModel.Description("AC Current ('CURR:AC')")> CurrentAC = 2 * SenseFunctionModes.VoltageAC
    <ComponentModel.Description("Resistance ('RES')")> Resistance = 2 * SenseFunctionModes.CurrentAC
    <ComponentModel.Description("Four-Wire Resistance ('FRES')")> FourWireResistance = 2 * SenseFunctionModes.Resistance
    <ComponentModel.Description("Temperature ('TEMP')")> Temperature = 2 * SenseFunctionModes.FourWireResistance
    <ComponentModel.Description("Frequency ('FREQ')")> Frequency = 2 * SenseFunctionModes.Temperature
    <ComponentModel.Description("Period ('PER')")> Period = 2 * SenseFunctionModes.Frequency
    <ComponentModel.Description("Continuity ('CONT')")> Continuity = 2 * SenseFunctionModes.Period
    <ComponentModel.Description("Timestamp element ('TIME')")> TimestampElement = 2 * SenseFunctionModes.Continuity
    <ComponentModel.Description("Status Element ('STAT')")> StatusElement = 2 * SenseFunctionModes.TimestampElement
    <ComponentModel.Description("Memory ('MEM')")> Memory = 2 * SenseFunctionModes.StatusElement
    <ComponentModel.Description("Diode ('DIOD')")> Diode = 2 * SenseFunctionModes.Memory
End Enum

