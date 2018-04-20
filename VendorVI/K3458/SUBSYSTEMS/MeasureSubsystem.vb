Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines a Measure Subsystem for a Keysight 1750 Resistance Measuring System. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/7/2013" by="David" revision=""> Created. </history>
Public Class MeasureSubsystem
    Inherits VI.MeasureSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="MeasureSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary>
    ''' Sets subsystem values to their known execution clear state.
    ''' </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.LastReading = ""
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.LastReading = ""
        MyBase.InitKnownState()
        Me.Readings = New Readings
        With Me.FunctionModeDecimalPlaces
            .Clear()
            For Each fmode As SenseFunctionMode In [Enum].GetValues(GetType(SenseFunctionMode))
                .Add(fmode, Me.DefaultFunctionModeDecimalPlaces)
            Next
        End With
        With Me.FunctionModeRanges
            .Clear()
            For Each fmode As SenseFunctionMode In [Enum].GetValues(GetType(SenseFunctionMode))
                .Add(fmode, Core.Pith.RangeR.Full)
            Next
        End With
        With Me.FunctionModeUnits
            .Clear()
            For Each fmode As SenseFunctionMode In [Enum].GetValues(GetType(SenseFunctionMode))
                .Add(fmode, Arebis.StandardUnits.UnitlessUnits.Ratio)
            Next
            .Item(K3458.SenseFunctionMode.CurrentDirect) = Arebis.StandardUnits.ElectricUnits.Ampere
            .Item(K3458.SenseFunctionMode.CurrentDirectAlternating) = Arebis.StandardUnits.ElectricUnits.Ampere
            .Item(K3458.SenseFunctionMode.Resistance) = Arebis.StandardUnits.ElectricUnits.Ohm
            .Item(K3458.SenseFunctionMode.ResistanceFourWire) = Arebis.StandardUnits.ElectricUnits.Ohm
            .Item(K3458.SenseFunctionMode.VoltageAlternating) = Arebis.StandardUnits.ElectricUnits.Volt
            .Item(K3458.SenseFunctionMode.VoltageDirect) = Arebis.StandardUnits.ElectricUnits.Volt
            .Item(K3458.SenseFunctionMode.VoltageDirectAlternating) = Arebis.StandardUnits.ElectricUnits.Volt
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

#Region "  INIT, READ, FETCH "

    ''' <summary> Gets the fetch command. </summary>
    ''' <value> The fetch command. </value>
    <Obsolete("Not supported")>
    Protected Overrides ReadOnly Property FetchCommand As String = ""

    ''' <summary> Gets the read command. </summary>
    ''' <value> The read command. </value>
    <Obsolete("Not supported")>
    Protected Overrides ReadOnly Property ReadCommand As String = ""

#End Region

#Region " AVERAGE ENABLED "

    ''' <summary> Gets the Average enabled command Format. </summary>
    ''' <value> The Average enabled query command. </value>
    Protected Overridable ReadOnly Property AverageEnabledCommandFormat As String = ":SENS:RES:AVER:STAT {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the Average enabled query command. </summary>
    ''' <value> The Average enabled query command. </value>
    Protected Overridable ReadOnly Property AverageEnabledQueryCommand As String = ":SENS:RES:AVER:STAT?"

#End Region

#Region " AUTO ZERO MODE "

    ''' <summary> Gets the auto zero command Format. </summary>
    ''' <value> The auto zero enabled query command. </value>
    Protected Overridable ReadOnly Property AutoZeroModeCommandFormat As String = "AZERO {0}"

    ''' <summary> Gets the auto zero query command. </summary>
    ''' <value> The auto Zero enabled query command. </value>
    Protected Overridable ReadOnly Property AutoZeroModeQueryCommand As String = "AZERO?"

    Private _AutoZeroMode As AutoZeroMode?
    ''' <summary> Gets or sets the cached Auto Zero Mode. </summary>
    ''' <value> The <see cref="AutoZeroMode">Auto Zero Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property AutoZeroMode As AutoZeroMode?
        Get
            Return Me._AutoZeroMode
        End Get
        Protected Set(ByVal value As AutoZeroMode?)
            If Not Me.AutoZeroMode.Equals(value) Then
                Me._AutoZeroMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Auto Zero Mode. </summary>
    ''' <param name="value"> The  Auto Zero Mode. </param>
    ''' <returns> The <see cref="AutoZeroMode">Auto Zero Mode</see> or none if unknown. </returns>
    Public Function ApplyAutoZeroMode(ByVal value As AutoZeroMode) As AutoZeroMode?
        Me.WriteAutoZeroMode(value)
        Return Me.QueryAutoZeroMode()
    End Function

    ''' <summary> Queries the auto Zero Mode. </summary>
    ''' <returns> The <see cref="AutoZeroMode">auto Zero Mode</see> or none if unknown. </returns>
    Public Function QueryAutoZeroMode() As AutoZeroMode?
        Me.AutoZeroMode = Me.QueryValue(Of AutoZeroMode)(Me.AutoZeroModeQueryCommand, Me.AutoZeroMode)
        Return Me.AutoZeroMode
    End Function

    ''' <summary> Writes the auto Zero Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The autoZeroMode. </param>
    ''' <returns> The <see cref="AutoZeroMode">autoZeroMode</see> or none if unknown. </returns>
    Public Function WriteAutoZeroMode(ByVal value As AutoZeroMode) As AutoZeroMode?
        Me.AutoZeroMode = Me.WriteValue(Of AutoZeroMode)(Me.AutoZeroModeCommandFormat, value)
        Return Me.AutoZeroMode
    End Function

#End Region

#Region " FETCH "

    ''' <summary> Fetches the data. </summary>
    ''' <remarks> the K3458 meter does not require issuing a read command. </remarks>
    Public Overrides Function Fetch() As Double?
        Return Me.Read()
    End Function

    ''' <summary> Fetches the data. </summary>
    ''' <remarks> the K3458 meter does not require issuing a read command. </remarks>
    Public Overrides Function Read() As Double?
        Dim value As String = Me.Session.ReadLineTrimEnd
        If Not String.IsNullOrWhiteSpace(Me.LastReading) Then
            ' the emulator will set the last reading. 
            Me.ParseReading(value)
        End If
        Return Me.MeasuredValue
    End Function

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

#Region " MATH MODE "

    ''' <summary> Gets the Math command Format. </summary>
    ''' <value> The Math enabled query command. </value>
    Protected Overridable ReadOnly Property MathModeCommandFormat As String = "MATH {0}"

    ''' <summary> Gets the Math query command. </summary>
    ''' <value> The Math enabled query command. </value>
    Protected Overridable ReadOnly Property MathModeQueryCommand As String = "MATH?"

    Private _MathMode As MathMode?
    ''' <summary> Gets or sets the cached Math Mode. </summary>
    ''' <value> The <see cref="MathMode">Math Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property MathMode As MathMode?
        Get
            Return Me._MathMode
        End Get
        Protected Set(ByVal value As MathMode?)
            If Not Me.MathMode.Equals(value) Then
                Me._MathMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Math Mode. </summary>
    ''' <param name="value"> The  Math Mode. </param>
    ''' <returns> The <see cref="MathMode">Math Mode</see> or none if unknown. </returns>
    Public Function ApplyMathMode(ByVal value As MathMode) As MathMode?
        Me.WriteMathMode(value)
        Return Me.QueryMathMode()
    End Function

    ''' <summary> Queries the Math Mode. </summary>
    ''' <returns> The <see cref="MathMode">Math Mode</see> or none if unknown. </returns>
    Public Function QueryMathMode() As MathMode?
        Me.MathMode = Me.QueryFirstValue(Of MathMode)(Me.MathModeQueryCommand, Me.MathMode)
        Return Me.MathMode
    End Function

    ''' <summary> Writes the Math Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The MathMode. </param>
    ''' <returns> The <see cref="MathMode">MathMode</see> or none if unknown. </returns>
    Public Function WriteMathMode(ByVal value As MathMode) As MathMode?
        Me.MathMode = Me.WriteValue(Of MathMode)(Me.MathModeCommandFormat, value)
        Return Me.MathMode
    End Function

#End Region

#Region " SENSE FUNCTION MODE "

    ''' <summary> Gets the Sense function command Format. </summary>
    ''' <value> The auto Range enabled query command. </value>
    Protected Overridable ReadOnly Property SenseFunctionModeCommandFormat As String = "FUNC {0}"

    ''' <summary> Gets the Sense function query command. </summary>
    ''' <value> The auto Range enabled query command. </value>
    Protected Overridable ReadOnly Property SenseFunctionModeQueryCommand As String = "FUNC?"

    Private _SenseFunctionMode As SenseFunctionMode?
    ''' <summary> Gets or sets the cached Sense function Mode. </summary>
    ''' <value> The <see cref="SenseFunctionMode">Sense function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property SenseFunctionMode As SenseFunctionMode?
        Get
            Return Me._SenseFunctionMode
        End Get
        Protected Set(ByVal value As SenseFunctionMode?)
            If Not Me.SenseFunctionMode.Equals(value) Then
                Me._SenseFunctionMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sense function Mode. </summary>
    ''' <param name="value"> The  Sense function Mode. </param>
    ''' <returns> The <see cref="SenseFunctionMode">Sense function Mode</see> or none if unknown. </returns>
    Public Function ApplySenseFunctionMode(ByVal value As SenseFunctionMode) As SenseFunctionMode?
        Me.WriteSenseFunctionMode(value)
        Return Me.QuerySenseFunctionMode()
    End Function

    ''' <summary> Queries the Sense Function Mode. </summary>
    ''' <returns> The <see cref="SenseFunctionMode">Sense Function Mode</see> or none if unknown. </returns>
    Public Function QuerySenseFunctionMode() As SenseFunctionMode?
        Me.SenseFunctionMode = Me.QueryFirstValue(Of SenseFunctionMode)(Me.SenseFunctionModeQueryCommand, Me.SenseFunctionMode)
        Return Me.SenseFunctionMode
    End Function

    ''' <summary> Writes the Sense Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Sense Function Mode. </param>
    ''' <returns> The <see cref="SenseFunctionMode">Sense Function Mode</see> or none if unknown. </returns>
    Public Function WriteSenseFunctionMode(ByVal value As SenseFunctionMode) As SenseFunctionMode?
        Me.SenseFunctionMode = Me.WriteValue(Of SenseFunctionMode)(Me.SenseFunctionModeCommandFormat, value)
        Return Me.SenseFunctionMode
    End Function

#End Region

#Region " AUTO RANGE ENABLED "

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = "ARANG?"

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = "ARANGE {0:'ON';'ON';'OFF'}"

#End Region

#Region " POWER LINE CYCLES "

    ''' <summary> Gets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesQueryCommand As String = "NPLC?"

    ''' <summary> Gets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesCommandFormat As String = "NPLC {0}"

#End Region

#Region " STORE MATH "

    Private _StoreMathValue As Double?
    ''' <summary> Gets or sets the cached Store Math Value. </summary>
    ''' <value> The cached Store Math Value or none if not set or unknown. </value>
    Public Overloads Property StoreMathValue As Double?
        Get
            Return Me._StoreMathValue
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.StoreMathValue, value) Then
                Me._StoreMathValue = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _StoreMathRegister As StoreMathRegister?
    ''' <summary> Gets or sets the cached Store Math Register. </summary>
    ''' <value> The <see cref="StoreMathRegister">Store Math Register</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property StoreMathRegister As StoreMathRegister?
        Get
            Return Me._StoreMathRegister
        End Get
        Protected Set(ByVal value As StoreMathRegister?)
            If Not Me.StoreMathRegister.Equals(value) Then
                Me._StoreMathRegister = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Store Math Register. </summary>
    ''' <returns> The <see cref="StoreMathRegister">Store Math Register</see> or none if unknown. </returns>
    <Obsolete("Apply does not work with store math because reading back returns the measured value and the register cannot be read")>
    Public Function ApplyStoreMath(ByVal register As StoreMathRegister, ByVal value As Double) As StoreMathRegister?
        Me.WriteStoreMath(register, value)
        Me.QueryStoreMathValue()
        Return Me.StoreMathRegister
    End Function

    ''' <summary> Gets the auto zero query command. </summary>
    ''' <value> The auto Zero enabled query command. </value>
    Protected Overridable ReadOnly Property StoreMathQueryCommand As String = "SMATH?"

    ''' <summary> Queries the Store Math Register. </summary>
    ''' <returns> The <see cref="StoreMathRegister">Store Math Register</see> or none if unknown. </returns>
    <Obsolete("Reading the register is not available")>
    Public Function QueryStoreMathRegister() As StoreMathRegister?
        Me.StoreMathRegister = Me.QueryFirstValue(Of StoreMathRegister)(Me.StoreMathQueryCommand, Me.StoreMathRegister)
        Return Me.StoreMathRegister
    End Function

    ''' <summary> Queries store mathematics value. </summary>
    ''' <returns> The store mathematics value. </returns>
    <Obsolete("Reading back from the stored math reads the measured value")>
    Public Function QueryStoreMathValue() As Double?
        Me.StoreMathValue = Me.Query(Me.StoreMathValue, Me.StoreMathQueryCommand)
        Return Me.StoreMathValue
    End Function

    ''' <summary> Gets or sets the store mathematics command format. </summary>
    ''' <value> The store mathematics command format. </value>
    Protected Overridable ReadOnly Property StoreMathCommandFormat As String = "SMATH {0},{1}"

    ''' <summary> Writes the Store Math Register and value without reading back the value from the device. </summary>
    ''' <returns> The <see cref="StoreMathRegister">StoreMathRegister</see> or none if unknown. </returns>
    Public Function WriteStoreMath(ByVal register As StoreMathRegister, ByVal value As Double) As StoreMathRegister?
        Me.StoreMathValue = value
        Me.StoreMathRegister = register
        Me.Session.Execute(String.Format(Me.StoreMathCommandFormat, register.ExtractBetween(), value))
        Return Me.StoreMathRegister
    End Function

#End Region

End Class

''' <summary> Enumerates the auto zero mode. </summary>
Public Enum AutoZeroMode
    ''' <summary> Zero measurement Is updated once, Then only after a
    ''' Function, range, aperture, NPLC, Or resolution change. </summary>
    <ComponentModel.Description("Off (OFF)")> [Off] = 0
    ''' <summary> Zero measurement Is updated after every measurement. </summary>
    <ComponentModel.Description("On (ON)")> [On] = 1
    ''' <summary> Zero measurement is updated once, then only after a
    ''' Function, range, aperture, NPLC, Or resolution change. </summary>
    <ComponentModel.Description("Once (ONCE)")> [Once] = 2
End Enum

''' <summary> Values that represent sense function modes. </summary>
Public Enum SenseFunctionMode
    <ComponentModel.Description("Not selected`")> None = 0
    ''' <summary> DCV 1 Selects DC voltage measurements. </summary>
    <ComponentModel.Description("DC Volts (DCV)")> VoltageDirect = 1
    ''' <summary> Selects AC voltage measurements (the mode is set by the SETACV command). </summary>
    <ComponentModel.Description("AC Volts (ACV)")> VoltageAlternating = 2
    ''' <summary> ACDCV 3 Selects AC+DC voltage measurements (the mode is set by the SETACV command). </summary>
    <ComponentModel.Description("Once (ACDCV)")> VoltageDirectAlternating = 3
    ''' <summary> OHM 4 Selects 2-wire ohms measurements. </summary>
    <ComponentModel.Description("Ohms (OHM)")> Resistance = 4
    ''' <summary> OHMF 5 Selects 4-wire ohms measurements. </summary>
    <ComponentModel.Description("Four Wire Ohms (OHMF)")> ResistanceFourWire = 5
    ''' <summary> Selects DC current measurements. </summary>
    <ComponentModel.Description("DC Current (DCI)")> CurrentDirect = 6
    ''' <summary> ACI 7 Selects AC current measurements. </summary>
    <ComponentModel.Description("AC Current (ACI)")> CurrentAlternating = 7
    ''' <summary> ACDCI 8 Selects AC+DC current measurements. </summary>
    <ComponentModel.Description("AC DC Current (ACDCI)")> CurrentDirectAlternating = 8
End Enum

Public Enum MathMode
    <ComponentModel.Description("OFF (OFF)")> None = 0
    <ComponentModel.Description("Percent (PERC)")> Percent = 10
End Enum

Public Enum StoreMathRegister
    <ComponentModel.Description("Not specified")> None = 0
    <ComponentModel.Description("Offset (OFFSET)")> Offset = 7
    <ComponentModel.Description("Percent (PERC)")> Percent = 8
End Enum
