Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines a Multimeter Subsystem for a TSP System. </summary>
''' <license> (c) 2016 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2016" by="David" revision=""> Created. </history>
Public MustInherit Class MultimeterSubsystemBase
    Inherits VI.MultimeterSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystemBase">status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        With Me.FunctionModeDecimalPlaces
            .Clear()
            For Each fmode As MultimeterFunctionMode In [Enum].GetValues(GetType(MultimeterFunctionMode))
                .Add(fmode, Me.DefaultFunctionModeDecimalPlaces)
            Next
        End With
        With Me.FunctionModeRanges
            .Clear()
            For Each fmode As MultimeterFunctionMode In [Enum].GetValues(GetType(MultimeterFunctionMode))
                .Add(fmode, Core.Pith.RangeR.Full)
            Next
        End With
        With Me.OpenDetectorKnownStates
            .Clear()
            For Each fmode As MultimeterFunctionMode In [Enum].GetValues(GetType(MultimeterFunctionMode))
                .Add(fmode, False)
            Next
        End With
        With Me.FunctionModeUnits
            .Clear()
            For Each fmode As MultimeterFunctionMode In [Enum].GetValues(GetType(MultimeterFunctionMode))
                .Add(fmode, Arebis.StandardUnits.UnitlessUnits.Ratio)
            Next
            .Item(MultimeterFunctionMode.CurrentAC) = Arebis.StandardUnits.ElectricUnits.Ampere
            .Item(MultimeterFunctionMode.CurrentDC) = Arebis.StandardUnits.ElectricUnits.Ampere
            .Item(MultimeterFunctionMode.ResistanceTwoWire) = Arebis.StandardUnits.ElectricUnits.Ohm
            .Item(MultimeterFunctionMode.ResistanceFourWire) = Arebis.StandardUnits.ElectricUnits.Ohm
            .Item(MultimeterFunctionMode.Continuity) = Arebis.StandardUnits.ElectricUnits.Ohm
            .Item(MultimeterFunctionMode.VoltageAC) = Arebis.StandardUnits.ElectricUnits.Volt
            .Item(MultimeterFunctionMode.VoltageDC) = Arebis.StandardUnits.ElectricUnits.Volt

            .Item(MultimeterFunctionMode.Capacitance) = Arebis.StandardUnits.ElectricUnits.Farad
            .Item(MultimeterFunctionMode.Diode) = Arebis.StandardUnits.ElectricUnits.Volt
            .Item(MultimeterFunctionMode.Frequency) = Arebis.StandardUnits.FrequencyUnits.Hertz
            .Item(MultimeterFunctionMode.Period) = Arebis.StandardUnits.TimeUnits.Second
            .Item(MultimeterFunctionMode.Ratio) = Arebis.StandardUnits.UnitlessUnits.Ratio
            .Item(MultimeterFunctionMode.Temperature) = Arebis.StandardUnits.TemperatureUnits.DegreeCelsius
        End With
    End Sub

#End Region

#Region " FUNCTION MODE "

    ''' <summary> The function mode. </summary>
    Private _FunctionMode As MultimeterFunctionMode?

    ''' <summary> Gets or sets the cached function mode. </summary>
    ''' <value> The <see cref="MultimeterFunctionMode">Multimeter Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FunctionMode As MultimeterFunctionMode?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As MultimeterFunctionMode?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                If value.HasValue Then
                    Me.FunctionUnit = Me.ToUnit(value.Value)
                    Me.FunctionRange = Me.ToRange(value.Value)
                    Me.FunctionRangeDecimalPlaces = Me.ToDecimalPlaces(value.Value)
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Multimeter Function Mode. </summary>
    ''' <param name="value"> The  Multimeter Function Mode. </param>
    ''' <returns> The <see cref="MultimeterFunctionMode">Multimeter Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As MultimeterFunctionMode) As MultimeterFunctionMode?
        Me.WriteFunctionMode(value)
        Me.QueryFunctionMode()
        ' changing the function mode changes range, auto delay mode and open detector enabled. 
        Me.QueryAperture()
        Me.QueryRange()
        Me.QueryAutoDelayEnabled()
        Me.QueryOpenDetectorEnabled()
        Return FunctionMode
    End Function

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command. </value>
    Protected Overridable ReadOnly Property FunctionModeQueryCommand As String

    ''' <summary> Queries the Measure Function Mode. </summary>
    ''' <returns> The <see cref="MultimeterFunctionMode">Measure Function Mode</see> or none if unknown. </returns>
    Public Overridable Function QueryFunctionMode() As MultimeterFunctionMode?
        Dim mode As String = Me.FunctionMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.FunctionModeQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(MeasureSubsystemBase)}.{NameOf(FunctionMode)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.FunctionMode = New MultimeterFunctionMode?
        Else
            Dim se As New StringEnumerator(Of MultimeterFunctionMode)
            Me.FunctionMode = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.FunctionMode
    End Function

    ''' <summary> Gets or sets the function mode command format. </summary>
    ''' <value> The function mode command format. </value>
    Protected Overridable ReadOnly Property FunctionModeCommandFormat As String

    ''' <summary> Writes the Measure Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="MultimeterFunctionMode">Measure Function Mode</see> or none if unknown. </returns>
    Public Overridable Function WriteFunctionMode(ByVal value As MultimeterFunctionMode) As MultimeterFunctionMode?
        Me.Session.WriteLine(Me.FunctionModeCommandFormat, value.ExtractBetween())
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

#End Region

#Region " FUNCTION MODE UNIT "

    ''' <summary> Converts a a measure unit to a measurement unit. </summary>
    ''' <param name="value"> The Measure Unit. </param>
    ''' <returns> Value as an Arebis.TypedUnits.Unit. </returns>
    Public Function ToMeasurementUnit(ByVal value As Tsp2.MultimeterMeasurementUnit) As Arebis.TypedUnits.Unit
        Dim result As Arebis.TypedUnits.Unit = Me.DefaultFunctionUnit
        Select Case value
            Case Tsp2.MultimeterMeasurementUnit.Celsius
                result = Arebis.StandardUnits.TemperatureUnits.DegreeCelsius
            Case Tsp2.MultimeterMeasurementUnit.Decibel
                result = Arebis.StandardUnits.UnitlessUnits.Decibel
            Case Tsp2.MultimeterMeasurementUnit.Fahrenheit
                result = Arebis.StandardUnits.TemperatureUnits.DegreeFahrenheit
            Case Tsp2.MultimeterMeasurementUnit.Kelvin
                result = Arebis.StandardUnits.TemperatureUnits.Kelvin
            Case Tsp2.MultimeterMeasurementUnit.Volt
                result = Arebis.StandardUnits.ElectricUnits.Volt
            Case Else
                result = Me.FunctionUnit
        End Select
        Return result
    End Function

    ''' <summary> Writes and reads back the Measure Unit. </summary>
    ''' <param name="value"> The  Measure Unit. </param>
    ''' <returns> The <see cref="FunctionUnit">Measure Unit</see> or none if unknown. </returns>
    Public Function ApplyMultimeterMeasurementUnit(ByVal value As MultimeterMeasurementUnit) As MultimeterMeasurementUnit?
        Me.WriteMultimeterMeasurementUnit(value)
        Return Me.QueryMultimeterMeasurementUnit
    End Function

    ''' <summary> The Unit. </summary>
    Private _MultimeterMeasurementUnit As MultimeterMeasurementUnit?

    ''' <summary> Gets or sets the cached measure Unit.  This is the actual unit for measurement. </summary>
    ''' <value> The <see cref="FunctionUnit">Measure Unit</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property MultimeterMeasurementUnit As MultimeterMeasurementUnit?
        Get
            Return Me._MultimeterMeasurementUnit
        End Get
        Protected Set(ByVal value As MultimeterMeasurementUnit?)
            If Not Nullable.Equals(Me.MultimeterMeasurementUnit, value) Then
                Me._MultimeterMeasurementUnit = value
                If value.HasValue Then
                    Me.FunctionUnit = Me.ToMeasurementUnit(value.Value)
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Measure Unit query command. </summary>
    ''' <remarks>
    ''' The query command uses the <see cref="VI.Pith.SessionBase.QueryPrintTrimEnd(Integer, String)"/>
    ''' </remarks>
    ''' <value> The Unit query command. </value>
    Protected Overridable ReadOnly Property MultimeterMeasurementUnitQueryCommand As String

    ''' <summary> Queries the Measure Unit. </summary>
    ''' <returns> The <see cref="MultimeterMeasurementUnit">Measure Unit</see> or none if unknown. </returns>
    Public Overridable Function QueryMultimeterMeasurementUnit() As MultimeterMeasurementUnit?
        Dim mode As String = Me.MultimeterMeasurementUnit.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.MultimeterMeasurementUnitQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(MeasureSubsystemBase)}.{NameOf(MultimeterMeasurementUnit)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.MultimeterMeasurementUnit = New MultimeterMeasurementUnit?
        Else
            Dim se As New StringEnumerator(Of MultimeterMeasurementUnit)
            Me.MultimeterMeasurementUnit = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.MultimeterMeasurementUnit
    End Function

    ''' <summary> Gets or sets the Measure Unit command format. </summary>
    ''' <value> The Unit command format. </value>
    Protected Overridable ReadOnly Property MultimeterMeasurementUnitCommandFormat As String

    ''' <summary> Writes the Measure Unit without reading back the value from the device. </summary>
    ''' <param name="value"> The Unit. </param>
    ''' <returns> The <see cref="MultimeterMeasurementUnit">Measure Unit</see> or none if unknown. </returns>
    Public Overridable Function WriteMultimeterMeasurementUnit(ByVal value As MultimeterMeasurementUnit) As MultimeterMeasurementUnit?
        Me.Session.WriteLine(Me.MultimeterMeasurementUnitCommandFormat, value.ExtractBetween())
        Me.MultimeterMeasurementUnit = value
        Return Me.MultimeterMeasurementUnit
    End Function

#End Region

#Region " READ "

    ''' <summary> Gets or sets the read buffer query command format. </summary>
    ''' <value> The read buffer query command format. </value>
    Protected Overridable ReadOnly Property ReadBufferQueryCommandFormat As String

    ''' <summary> Queries buffer reading. </summary>
    ''' <remarks>
    ''' This command initiates measurements using the present function setting, stores the readings
    ''' in a reading buffer, and returns the last reading. The dmm.measure.count attribute determines
    ''' how many measurements are performed. When you use a reading buffer with a command Or action
    ''' that makes multiple readings, all readings are available In the reading buffer. However, only
    ''' the last reading Is returned As a reading With the command. If you define a specific reading
    ''' buffer, the reading buffer must exist before you make the measurement.
    ''' </remarks>
    ''' <param name="bufferName"> Name of the buffer. </param>
    ''' <returns> The reading or none if unknown. </returns>
    Public Overloads Function Measure(ByVal bufferName As String) As Double?
        Dim value As String = Me.Query(Me.Session.EmulatedReply, String.Format(Me.ReadBufferQueryCommandFormat, bufferName))
        ' the emulator will set the last reading. 
        Return Me.ParseReading(value)
    End Function

#End Region

End Class

''' <summary> Specifies the function modes. </summary>
Public Enum MultimeterFunctionMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("DC Voltage (dmm.FUNC_DC_VOLTAGE)")> VoltageDC
    <ComponentModel.Description("AC Voltage (FUNC_AC_VOLTAGE)")> VoltageAC
    <ComponentModel.Description("DC Current (dmm.FUNC_DC_CURRENT)")> CurrentDC
    <ComponentModel.Description("AC Current (dmm.FUNC_AC_CURRENT)")> CurrentAC
    <ComponentModel.Description("Temperature (dmm.FUNC_TEMPERATURE)")> Temperature
    <ComponentModel.Description("Resistance 2-Wire (dmm.FUNC_RESISTANCE)")> ResistanceTwoWire
    <ComponentModel.Description("Resistance 4-Wire (dmm.FUNC_4W_RESISTANCE)")> ResistanceFourWire
    <ComponentModel.Description("Temperature (dmm.FUNC_DIODE)")> Diode
    <ComponentModel.Description("Temperature (dmm.FUNC_CAPACITANCE)")> Capacitance
    <ComponentModel.Description("Temperature (dmm.FUNC_CONTINUITY)")> Continuity
    <ComponentModel.Description("Temperature (dmm.FUNC_ACV_FREQUENCY)")> Frequency
    <ComponentModel.Description("Temperature (dmm.FUNC_ACV_PERIOD)")> Period
    <ComponentModel.Description("Temperature (dmm.FUNC_DCV_RATIO)")> Ratio
End Enum

''' <summary> Specifies the units. </summary>
Public Enum MultimeterMeasurementUnit
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Volt (dmm.UNIT_VOLT)")> Volt
    <ComponentModel.Description("Decibel (dmm.UNIT_DB)")> Decibel
    <ComponentModel.Description("Celsius (dmm.UNIT_CELCIUS)")> Celsius
    <ComponentModel.Description("Kelvin (dmm.UNIT_KELVIN)")> Kelvin
    <ComponentModel.Description("Fahrenheit (dmm.UNIT_FAHRENHEIT)")> Fahrenheit
End Enum

#Region " UNUSED "
#If False Then
''' <summary> Gets or sets the default measurement unit. </summary>
    ''' <value> The default measure unit. </value>
    Public Property DefaultMeasurementUnit As Arebis.TypedUnits.Unit

    ''' <summary> Gets or sets the function unit. </summary>
    ''' <value> The function unit. </value>
    Public Property MeasurementUnit As Arebis.TypedUnits.Unit
        Get
            Return Me.Amount.Unit
        End Get
        Set(value As Arebis.TypedUnits.Unit)
            If Me.MeasurementUnit <> value Then
                Me.Amount.Unit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    
#End If
#End Region