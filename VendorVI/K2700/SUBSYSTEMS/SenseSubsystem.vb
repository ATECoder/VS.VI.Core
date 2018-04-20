''' <summary> Defines a SCPI Sense Subsystem for a Keithley 2700 instrument. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class SenseSubsystem
    Inherits VI.Scpi.SenseSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.SupportedFunctionModes = VI.Scpi.SenseFunctionModes.CurrentDC Or
                                        VI.Scpi.SenseFunctionModes.VoltageDC Or
                                        VI.Scpi.SenseFunctionModes.FourWireResistance
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary>
    ''' Sets subsystem values to their known execution clear state.
    ''' </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.Readings?.Reset()
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        ' TO_DO: the readings are initialized when the format system is reset.
        Me.Readings = New Readings
        Me.FunctionMode = VI.Scpi.SenseFunctionModes.VoltageDC
        With Me.FunctionModeDecimalPlaces
            .Clear()
            For Each fmode As Scpi.SenseFunctionModes In [Enum].GetValues(GetType(Scpi.SenseFunctionModes))
                .Add(fmode, Me.DefaultFunctionModeDecimalPlaces)
            Next
        End With
        With Me.FunctionModeRanges
            .Clear()
            For Each fmode As Scpi.SenseFunctionModes In [Enum].GetValues(GetType(Scpi.SenseFunctionModes))
                .Add(fmode, Core.Pith.RangeR.Full)
            Next
        End With
        With Me.FunctionModeUnits
            .Clear()
            For Each fmode As Scpi.SenseFunctionModes In [Enum].GetValues(GetType(Scpi.SenseFunctionModes))
                .Add(fmode, Arebis.StandardUnits.UnitlessUnits.Ratio)
            Next
            .Item(Scpi.SenseFunctionModes.CurrentAC) = Arebis.StandardUnits.ElectricUnits.Ampere
            .Item(Scpi.SenseFunctionModes.CurrentDC) = Arebis.StandardUnits.ElectricUnits.Ampere
            .Item(Scpi.SenseFunctionModes.Resistance) = Arebis.StandardUnits.ElectricUnits.Ohm
            .Item(Scpi.SenseFunctionModes.Continuity) = Arebis.StandardUnits.ElectricUnits.Ohm
            .Item(Scpi.SenseFunctionModes.VoltageAC) = Arebis.StandardUnits.ElectricUnits.Volt
            .Item(Scpi.SenseFunctionModes.VoltageDC) = Arebis.StandardUnits.ElectricUnits.Volt
            .Item(Scpi.SenseFunctionModes.Diode) = Arebis.StandardUnits.ElectricUnits.Volt
            .Item(Scpi.SenseFunctionModes.Frequency) = Arebis.StandardUnits.FrequencyUnits.Hertz
            .Item(Scpi.SenseFunctionModes.Period) = Arebis.StandardUnits.TimeUnits.Second
            .Item(Scpi.SenseFunctionModes.Temperature) = Arebis.StandardUnits.TemperatureUnits.Kelvin
        End With

    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " AUTO RANGE "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = ":SENS:CURR:RANG:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = ":SENS:CURR:RANG:AUTO?"

#End Region

#Region " POWER LINE CYCLES "

    ''' <summary> Gets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesCommandFormat As String = ":SENS:CURR:NPLC {0}"

    ''' <summary> Gets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesQueryCommand As String = ":SENS:CURR:NPLC?"

#End Region

#Region " PROTECTION LEVEL "

    ''' <summary> Gets the protection level command format. </summary>
    ''' <value> the protection level command format. </value>
    Protected Overrides ReadOnly Property ProtectionLevelCommandFormat As String = ":SENS:CURR:PROT {0}"

    ''' <summary> Gets the protection level query command. </summary>
    ''' <value> the protection level query command. </value>
    Protected Overrides ReadOnly Property ProtectionLevelQueryCommand As String = ":SENS:CURR:PROT?"

#End Region


#Region " LATEST DATA "

    ''' <summary> Gets the latest data query command. </summary>
    ''' <value> The latest data query command. </value>
    Protected Overrides ReadOnly Property LatestDataQueryCommand As String = ":SENSE:DATA:LAT?"

#End Region

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

#Region " FUNCTION MODE "

    ''' <summary> Converts a functionMode to a range. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an isr.Core.Pith.RangeR. </returns>
    Public Overrides Function ToRange(ByVal functionMode As Integer) As isr.Core.Pith.RangeR
        Dim result As isr.Core.Pith.RangeR = isr.Core.Pith.RangeR.Full
        Select Case functionMode
            Case VI.Scpi.SenseFunctionModes.CurrentDC, VI.Scpi.SenseFunctionModes.Current, VI.Scpi.SenseFunctionModes.CurrentAC
                result = New Core.Pith.RangeR(0, 10)
            Case VI.Scpi.SenseFunctionModes.VoltageDC, VI.Scpi.SenseFunctionModes.Voltage, VI.Scpi.SenseFunctionModes.VoltageAC
                result = New Core.Pith.RangeR(0, 1000)
            Case VI.Scpi.SenseFunctionModes.FourWireResistance
                result = New Core.Pith.RangeR(0, 2000000)
            Case VI.Scpi.SenseFunctionModes.Resistance
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
            Case VI.Scpi.SenseFunctionModes.CurrentDC, VI.Scpi.SenseFunctionModes.Current, VI.Scpi.SenseFunctionModes.CurrentAC
                result = 3
            Case VI.Scpi.SenseFunctionModes.VoltageDC, VI.Scpi.SenseFunctionModes.Voltage, VI.Scpi.SenseFunctionModes.VoltageAC
                result = 3
            Case VI.Scpi.SenseFunctionModes.FourWireResistance
                result = 0
            Case VI.Scpi.SenseFunctionModes.Resistance
                result = 0
        End Select
        Return result
    End Function

#End Region

End Class
