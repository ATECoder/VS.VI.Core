Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ComboBoxExtensions
''' <summary> Defines the contract that must be implemented by a Sense Subsystem. </summary>
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
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.Readings?.Reset()
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        ' TO_DO: the readings are initialized when the format system is reset.
        Me.Readings = New Readings
        Me.FunctionMode = SenseFunctionModes.VoltageDC
        With Me.FunctionModeDecimalPlaces
            .Clear()
            For Each fmode As SenseFunctionModes In [Enum].GetValues(GetType(SenseFunctionModes))
                .Add(fmode, Me.DefaultFunctionModeDecimalPlaces)
            Next
        End With
        With Me.FunctionModeRanges
            .Clear()
            For Each fmode As SenseFunctionModes In [Enum].GetValues(GetType(SenseFunctionModes))
                .Add(fmode, Core.Pith.RangeR.Full)
            Next
        End With
        With Me.FunctionModeUnits
            .Clear()
            For Each fmode As SenseFunctionModes In [Enum].GetValues(GetType(SenseFunctionModes))
                .Add(fmode, Arebis.StandardUnits.UnitlessUnits.Ratio)
            Next
            .Item(SenseFunctionModes.CurrentAC) = Arebis.StandardUnits.ElectricUnits.Ampere
            .Item(SenseFunctionModes.CurrentDC) = Arebis.StandardUnits.ElectricUnits.Ampere
            .Item(SenseFunctionModes.Resistance) = Arebis.StandardUnits.ElectricUnits.Ohm
            .Item(SenseFunctionModes.Continuity) = Arebis.StandardUnits.ElectricUnits.Ohm
            .Item(SenseFunctionModes.VoltageAC) = Arebis.StandardUnits.ElectricUnits.Volt
            .Item(SenseFunctionModes.VoltageDC) = Arebis.StandardUnits.ElectricUnits.Volt
            .Item(SenseFunctionModes.Diode) = Arebis.StandardUnits.ElectricUnits.Volt
            .Item(SenseFunctionModes.Frequency) = Arebis.StandardUnits.FrequencyUnits.Hertz
            .Item(SenseFunctionModes.Period) = Arebis.StandardUnits.TimeUnits.Second
            .Item(SenseFunctionModes.Temperature) = Arebis.StandardUnits.TemperatureUnits.Kelvin
        End With

    End Sub


#End Region


#Region " CONCURRENT SENSE FUNCTION MODE "

    ''' <summary> Gets or sets the automatic Range enabled query command. </summary>
    ''' <remarks> SCPI: "system:RANG:AUTO?". </remarks>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property ConcurrentSenseEnabledQueryCommand As String = ":SENS:FUNC:CONC?"

    ''' <summary> Gets or sets the automatic Range enabled command Format. </summary>
    ''' <remarks> SCPI: "system:RANGE:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property ConcurrentSenseEnabledCommandFormat As String = ":SENS:FUNC:CONC {0:'ON';'ON';'OFF'}"

#End Region

#Region " LATEST DATA "

    Private _Readings As Readings
    ''' <summary> Gets or sets the readings. </summary>
    ''' <value> The readings. </value>
    Public Property Readings As Readings
        Get
            Return Me._Readings
        End Get
        Private Set(value As Readings)
            Me._Readings = value
            MyBase.AssignReadingAmounts(value)
        End Set
    End Property

    ''' <summary> Parses a new set of reading elements. </summary>
    ''' <param name="reading"> Specifies the measurement text to parse into the new reading. </param>
    Public Overrides Function ParseReading(ByVal reading As String) As Double?
        Return MyBase.ParseReadingAmounts(reading)
    End Function

#End Region

#Region " LATEST DATA "

    ''' <summary> Gets or sets the latest data query command. </summary>
    ''' <remarks> SCPI: ":SENSE:DATA:LAT?". </remarks>
    ''' <value> The latest data query command. </value>
    Protected Overrides ReadOnly Property LatestDataQueryCommand As String = ":SENSE:DATA:LAT?"

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Converts a functionMode to a range. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an isr.Core.Pith.RangeR. </returns>
    Public Overrides Function ToRange(ByVal functionMode As Integer) As isr.Core.Pith.RangeR
        Dim result As isr.Core.Pith.RangeR = isr.Core.Pith.RangeR.Full
        Select Case functionMode
            Case VI.SourceMeasure.SenseFunctionModes.CurrentDC, VI.SourceMeasure.SenseFunctionModes.Current, VI.SourceMeasure.SenseFunctionModes.CurrentAC
                result = New Core.Pith.RangeR(0, 10)
            Case VI.SourceMeasure.SenseFunctionModes.VoltageDC, VI.SourceMeasure.SenseFunctionModes.Voltage, VI.SourceMeasure.SenseFunctionModes.VoltageAC
                result = New Core.Pith.RangeR(0, 1000)
            Case VI.SourceMeasure.SenseFunctionModes.FourWireResistance
                result = New Core.Pith.RangeR(0, 2000000)
            Case VI.SourceMeasure.SenseFunctionModes.Resistance
                result = New Core.Pith.RangeR(0, 1000000000D)
        End Select
        Return result
    End Function

    ''' <summary> Converts a functionMode to a decimal places. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an Integer. </returns>
    Public Overrides Function ToDecimalPlaces(ByVal functionMode As Integer) As Integer
        Dim result As Integer = 3
        Select Case functionMode
            Case VI.SourceMeasure.SenseFunctionModes.CurrentDC, VI.SourceMeasure.SenseFunctionModes.Current, VI.SourceMeasure.SenseFunctionModes.CurrentAC
                result = 3
            Case VI.SourceMeasure.SenseFunctionModes.VoltageDC, VI.SourceMeasure.SenseFunctionModes.Voltage, VI.SourceMeasure.SenseFunctionModes.VoltageAC
                result = 3
            Case VI.SourceMeasure.SenseFunctionModes.FourWireResistance
                result = 0
            Case VI.SourceMeasure.SenseFunctionModes.Resistance
                result = 0
        End Select
        Return result
    End Function

#End Region

#Region " FUNCTION MODES "

    ''' <summary> Builds the Modes record for the specified Modes. </summary>
    ''' <param name="Modes"> Sense Function Modes. </param>
    ''' <returns> The record. </returns>
    Public Shared Function BuildRecord(ByVal modes As VI.SourceMeasure.SenseFunctionModes) As String
        If modes = VI.SourceMeasure.SenseFunctionModes.None Then
            Return String.Empty
        Else
            Dim reply As New System.Text.StringBuilder
            For Each code As Integer In [Enum].GetValues(GetType(VI.SourceMeasure.SenseFunctionModes))
                If (modes And code) <> 0 Then
                    Dim value As String = CType(code, VI.SourceMeasure.SenseFunctionModes).ExtractBetween()
                    If Not String.IsNullOrWhiteSpace(value) Then
                        If reply.Length > 0 Then
                            reply.Append(",")
                        End If
                        reply.Append(value)
                    End If
                End If
            Next
            Return reply.ToString
        End If
    End Function

    ''' <summary> Get the composite Sense Function Modes based on the message from the instrument. </summary>
    ''' <param name="record"> Specifies the comma delimited Modes record. </param>
    ''' <returns> The sense function modes. </returns>
    Public Shared Function ParseSenseFunctionModes(ByVal record As String) As SenseFunctionModes
        Dim parsed As SenseFunctionModes = SenseFunctionModes.None
        If Not String.IsNullOrWhiteSpace(record) Then
            For Each modeValue As String In record.Split(","c)
                parsed = parsed Or SenseSubsystemBase.ParseSenseFunctionMode(modeValue)
            Next
        End If
        Return parsed
    End Function

    ''' <summary> The Sense Function Modes. </summary>
    Private _FunctionModes As VI.SourceMeasure.SenseFunctionModes?

    ''' <summary> Gets or sets the cached Sense Function Modes. </summary>
    ''' <value> The Function Modes or null if unknown. </value>
    Public Property FunctionModes As VI.SourceMeasure.SenseFunctionModes?
        Get
            Return Me._FunctionModes
        End Get
        Protected Set(ByVal value As VI.SourceMeasure.SenseFunctionModes?)
            If Not Nullable.Equals(Me.FunctionModes, value) Then
                Me._FunctionModes = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sense Function Modes. </summary>
    ''' <param name="value"> The <see cref="VI.SourceMeasure.SenseFunctionModes">Function Modes</see>. </param>
    ''' <returns> The Sense Function Modes or null if unknown. </returns>
    Public Function ApplyFunctionModes(ByVal value As VI.SourceMeasure.SenseFunctionModes) As VI.SourceMeasure.SenseFunctionModes?
        Me.WriteFunctionModes(value)
        Return Me.QueryFunctionModes()
    End Function

    Protected Overridable ReadOnly Property FunctionModeQueryCommand As String

    ''' <summary> Queries the Sense Function Modes. Also sets the <see cref="FunctionModes"></see> cached value. </summary>
    ''' <returns> The Sense Function Mode or null if unknown. </returns>
    Public Function QueryFunctionModes() As VI.SourceMeasure.SenseFunctionModes?
        ' the instrument expects single quotes when writing the value but sends back items delimited with double quotes.
        Me.FunctionModes = SenseSubsystemBase.ParseSenseFunctionModes(Me.Session.QueryTrimEnd(":SENS:FUNC?").Replace(CChar(""""), "'"c))
        Return Me.FunctionModes
    End Function

    Protected Overridable ReadOnly Property FunctionModeCommandFormat As String

    ''' <summary> Writes the Sense Function Mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The Sense Function Mode or null if unknown. </returns>
    Public Function WriteFunctionModes(ByVal value As VI.SourceMeasure.SenseFunctionModes) As VI.SourceMeasure.SenseFunctionModes?
        Me.Session.WriteLine(":SENS:FUNC {0}", SenseSubsystemBase.BuildRecord(value))
        Me.FunctionModes = value
        Return Me.FunctionModes
    End Function

    Private _SupportsMultiFunctions As Boolean

    ''' <summary> Gets or sets the condition telling if the instrument supports multi-functions. For
    ''' example, the 2400 source-measure instrument support measuring voltage, current, and
    ''' resistance concurrently whereas the 2700 supports a single function at a time. </summary>
    ''' <value> The supports multi functions. </value>
    <CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Multi")>
    Public Property SupportsMultiFunctions() As Boolean
        Get
            Return Me._SupportsMultiFunctions
        End Get
        Set(ByVal value As Boolean)
            If Not Me.SupportsMultiFunctions.Equals(value) Then
                Me._SupportsMultiFunctions = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Returns the <see cref="SenseFunctionModes"></see> from the specified value. </summary>
    ''' <param name="value"> The Modes. </param>
    ''' <returns> The sense function mode. </returns>
    Public Shared Function ParseSenseFunctionMode(ByVal value As String) As SenseFunctionModes
        If String.IsNullOrWhiteSpace(value) Then
            Return SenseFunctionModes.None
        Else
            Dim se As New isr.Core.Pith.StringEnumerator(Of SenseFunctionModes)
            Return se.ParseContained(value.BuildDelimitedValue)
        End If
    End Function

    ''' <summary> The Sense Function Mode. </summary>
    Private _FunctionMode As SenseFunctionModes?

    ''' <summary> Gets or sets the cached Sense Function Mode. </summary>
    ''' <value> The Function Mode or null if unknown. </value>
    Public Overridable Property FunctionMode As SenseFunctionModes?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As SenseFunctionModes?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                Me.SafePostPropertyChanged()
                If value.HasValue Then
                    Me.FunctionUnit = Me.ToUnit(value.Value)
                    Me.FunctionRange = Me.ToRange(value.Value)
                    Me.FunctionRangeDecimalPlaces = Me.ToDecimalPlaces(value.Value)
                End If
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
    ''' <returns> The Sense Function Mode or null if unknown. </returns>
    Public Function QueryFunctionMode() As SenseFunctionModes?
        ' the instrument expects single quotes when writing the value but sends back items delimited with double quotes.
        Me.FunctionMode = SenseSubsystemBase.ParseSenseFunctionMode(Me.Session.QueryTrimEnd(":SENS:FUNC?").Replace(CChar(""""), "'"c))
        Return Me.FunctionMode
    End Function

    ''' <summary> Writes the Sense Function Mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The Sense Function Mode or null if unknown. </returns>
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
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Displays a supported function modes. </summary>
    ''' <param name="listControl"> The list control. </param>
    Public Sub DisplaySupportedFunctionModes(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        With listControl
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(SenseFunctionModes).ValueDescriptionPairs(Me.SupportedFunctionModes)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            If .Items.Count > 0 AndAlso Me.FunctionMode.HasValue Then
                .SelectedItem = Me.FunctionMode.Value.ValueDescriptionPair()
            End If
        End With
    End Sub

    ''' <summary> Returns the function mode selected by the list control. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    ''' <returns> The SenseFunctionModes. </returns>
    Public Shared Function SelectedFunctionMode(ByVal listControl As Windows.Forms.ComboBox) As SenseFunctionModes
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Return CType(CType(listControl.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, SenseFunctionModes)
    End Function

    ''' <summary> Safe select function mode. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    Public Sub SafeSelectFunctionMode(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        If Me.FunctionMode.HasValue Then
            listControl.SafeSelectItem(Me.FunctionMode.Value, Me.FunctionMode.Value.Description)
        End If
    End Sub

#End Region

#Region " RANGE "

    ''' <summary> Gets or sets The Range query command. </summary>
    ''' <value> The Range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = ":SOUR:RANG?"

    ''' <summary> Gets or sets The Range command format. </summary>
    ''' <value> The Range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = ":SENS:RANG {0}"

#End Region

End Class

''' <summary>Specifies the sense function modes.</summary>
<System.Flags()> Public Enum SenseFunctionModes
    <ComponentModel.Description("Not specified")> None = 0
    <ComponentModel.Description("Voltage ('VOLT')")> Voltage = 1
    <ComponentModel.Description("Current ('CURR')")> Current = SenseFunctionModes.Voltage << 1
    <ComponentModel.Description("DC Voltage ('VOLT:DC')")> VoltageDC = SenseFunctionModes.Current << 1
    <ComponentModel.Description("DC Current ('CURR:DC')")> CurrentDC = SenseFunctionModes.VoltageDC << 1
    <ComponentModel.Description("AC Voltage ('VOLT:AC')")> VoltageAC = SenseFunctionModes.CurrentDC << 1
    <ComponentModel.Description("AC Current ('CURR:AC')")> CurrentAC = SenseFunctionModes.VoltageAC << 1
    <ComponentModel.Description("Resistance ('RES')")> Resistance = SenseFunctionModes.CurrentAC << 1
    <ComponentModel.Description("Four-Wire Resistance ('FRES')")> FourWireResistance = SenseFunctionModes.Resistance << 1
    <ComponentModel.Description("Temperature ('TEMP')")> Temperature = SenseFunctionModes.FourWireResistance << 1
    <ComponentModel.Description("Frequency ('FREQ')")> Frequency = SenseFunctionModes.Temperature << 1
    <ComponentModel.Description("Period ('PER')")> Period = SenseFunctionModes.Frequency << 1
    <ComponentModel.Description("Continuity ('CONT')")> Continuity = SenseFunctionModes.Period << 1
    <ComponentModel.Description("Timestamp element ('TIME')")> TimestampElement = SenseFunctionModes.Continuity << 1
    <ComponentModel.Description("Status Element ('STAT')")> StatusElement = SenseFunctionModes.TimestampElement << 1
    <ComponentModel.Description("Memory ('MEM')")> Memory = SenseFunctionModes.StatusElement << 1
    <ComponentModel.Description("Diode ('DIOD')")> Diode = SenseFunctionModes.Memory << 1
End Enum

