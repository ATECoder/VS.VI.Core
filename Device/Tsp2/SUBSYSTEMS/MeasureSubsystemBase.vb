Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary>  Defines the contract that must be implemented by a Source Measure Unit Measure Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class MeasureSubsystemBase
    Inherits VI.MeasureSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MeasureSubsystemBase" /> class.
    ''' </summary>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.ApertureRange = Core.Pith.RangeR.FullNonnegative
        Me.FilterCountRange = Core.Pith.RangeI.FullNonnegative
        Me.FilterWindowRange = Core.Pith.RangeR.FullNonnegative
        Me.PowerLineCyclesRange = Core.Pith.RangeR.FullNonnegative
        Me.FunctionUnit = Me.DefaultFunctionUnit
        Me.FunctionRange = Me.DefaultFunctionRange
        Me.FunctionRangeDecimalPlaces = Me.DefaultFunctionModeDecimalPlaces
        Me.DefaultMeasurementUnit = Arebis.StandardUnits.ElectricUnits.Volt
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.AutoRangeState = OnOffState.On
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
            For Each fmode As MeasureFunctionMode In [Enum].GetValues(GetType(MeasureFunctionMode))
                .Add(fmode, Arebis.StandardUnits.UnitlessUnits.Ratio)
            Next
            .Item(MeasureFunctionMode.CurrentDC) = Arebis.StandardUnits.ElectricUnits.Ampere
            .Item(MeasureFunctionMode.Resistance) = Arebis.StandardUnits.ElectricUnits.Ohm
            .Item(MeasureFunctionMode.VoltageDC) = Arebis.StandardUnits.ElectricUnits.Volt
        End With
    End Sub


#End Region

#Region " AUTO RANGE STATE "

    ''' <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    ''' <remarks>
    ''' When this command is set to off, you must set the range. If you do not set the range, the
    ''' instrument remains at the range that was selected by auto range. When this command Is set to
    ''' on, the instrument automatically goes to the most sensitive range to perform the measurement.
    ''' If a range Is manually selected through the front panel Or a remote command, this command Is
    ''' automatically set to off. Auto range selects the best range In which To measure the signal
    ''' that Is applied To the input terminals of the instrument. When auto range Is enabled, the
    ''' range increases at 120 percent of range And decreases occurs When the reading Is less than 10
    ''' percent Of nominal range. For example, If you are On the 1 volt range And auto range Is
    ''' enabled, the instrument auto ranges up To the 10 volt range When the measurement exceeds 1.2
    ''' volts. It auto ranges down To the 100 mV range When the measurement falls below 1 volt.
    ''' </remarks>
    ''' <value>
    ''' <c>null</c> if Auto Range Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>.
    ''' </value>
    Public Overrides Property AutoRangeEnabled As Boolean?
        Get
            Return MyBase.AutoRangeEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoRangeEnabled, value) Then
                If value.HasValue Then
                    Me.AutoRangeState = If(value.Value, OnOffState.On, OnOffState.Off)
                Else
                    Me.AutoRangeState = New OnOffState?
                End If
                MyBase.AutoRangeEnabled = value
            End If
        End Set
    End Property

    Private _AutoRangeState As OnOffState?

    ''' <summary> Gets or sets the Auto Range. </summary>
    Public Property AutoRangeState() As OnOffState?
        Get
            Return Me._AutoRangeState
        End Get
        Protected Set(ByVal value As OnOffState?)
            If Not Nullable.Equals(value, Me.AutoRangeState) Then
                Me._AutoRangeState = value
                If value.HasValue Then
                    Me.AutoRangeEnabled = (value.Value = OnOffState.On)
                Else
                    Me.AutoRangeEnabled = New Boolean?
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the AutoRange state. </summary>
    Public Function ApplyAutoRangeState(ByVal value As OnOffState) As OnOffState?
        Me.WriteAutoRangeState(value)
        Return Me.QueryAutoRangeState()
    End Function

    ''' <summary> Gets or sets the Auto Range state query command. </summary>
    ''' <value> The Auto Range state query command. </value>
    Protected Overridable ReadOnly Property AutoRangeStateQueryCommand As String = "_G.smu.measure.autorange"

    ''' <summary> Queries automatic range state. </summary>
    ''' <returns> The automatic range state. </returns>
    Public Function QueryAutoRangeState() As OnOffState?
        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Reading {NameOf(AutoRangeState)};. ")
        Me.Session.LastNodeNumber = New Integer?
        Dim mode As String = Me.AutoRangeState.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.AutoRangeStateQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(AutoRangeState)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.AutoRangeState = New OnOffState?
        Else
            Dim se As New StringEnumerator(Of OnOffState)
            Me.AutoRangeState = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.AutoRangeState
    End Function

    ''' <summary> The Auto Range state command format. </summary>
    Protected Overridable ReadOnly Property AutoRangeStateCommandFormat As String = "_G.smu.measure.autorange={0}"

    ''' <summary> Writes an automatic range state. </summary>
    ''' <param name="value"> The Aperture. </param>
    ''' <returns> An OnOffState. </returns>
    Public Function WriteAutoRangeState(ByVal value As OnOffState) As OnOffState
        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Writing {NameOf(AutoRangeState)}={value};. ")
        Me.Session.LastNodeNumber = New Integer?
        Me.Session.WriteLine(Me.AutoRangeStateCommandFormat, value.ExtractBetween)
        Me.AutoRangeState = value
        Return value
    End Function

#End Region

#Region " FUNCTION MODE "

    ''' <summary> The function mode. </summary>
    Private _FunctionMode As MeasureFunctionMode?

    ''' <summary> Writes and reads back the Measure Function Mode. </summary>
    ''' <param name="value"> The  Measure Function Mode. </param>
    ''' <returns> The <see cref="MeasureFunctionMode">Measure Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As MeasureFunctionMode) As MeasureFunctionMode?
        Me.WriteFunctionMode(value)
        ' changing the function mode changes range, auto delay mode and open detector enabled. 
        Me.QueryRange()
        Me.QueryOpenDetectorEnabled()
        Return Me.QueryFunctionMode
    End Function

    ''' <summary> Gets or sets the cached function mode. </summary>
    ''' <value> The <see cref="MeasureFunctionMode">Measure Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FunctionMode As MeasureFunctionMode?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As MeasureFunctionMode?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                If value.HasValue Then
                    Me.FunctionUnit = Me.ToUnit(value.Value)
                    Me.FunctionRange = Me.ToRange(value.Value)
                    Me.FunctionRangeDecimalPlaces = Me.ToDecimalPlaces(value.Value)
                    Me.FunctionRange = Me.FunctionModeRanges(value.Value)
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command. </value>
    Protected Overridable ReadOnly Property FunctionModeQueryCommand As String

    ''' <summary> Queries the Measure Function Mode. </summary>
    ''' <returns> The <see cref="MeasureFunctionMode">Measure Function Mode</see> or none if unknown. </returns>
    Public Overridable Function QueryFunctionMode() As MeasureFunctionMode?
        Dim mode As String = Me.FunctionMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.FunctionModeQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(MeasureSubsystemBase)}.{NameOf(FunctionMode)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.FunctionMode = New MeasureFunctionMode?
        Else
            Dim se As New StringEnumerator(Of MeasureFunctionMode)
            Me.FunctionMode = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.FunctionMode
    End Function

    ''' <summary> Gets or sets the function mode command format. </summary>
    ''' <value> The function mode command format. </value>
    Protected Overridable ReadOnly Property FunctionModeCommandFormat As String


    ''' <summary> Writes the Measure Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="MeasureFunctionMode">Measure Function Mode</see> or none if unknown. </returns>
    Public Overridable Function WriteFunctionMode(ByVal value As MeasureFunctionMode) As MeasureFunctionMode?
        Me.Session.WriteLine(Me.FunctionModeCommandFormat, value.ExtractBetween())
        ' check on the measure unit.
        Me.QueryMeasureUnit()
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

#End Region

#Region " MEASURE UNIT "

    ''' <summary> Converts a a measure unit to a measurement unit. </summary>
    ''' <param name="value"> The Measure Unit. </param>
    ''' <returns> Value as an Arebis.TypedUnits.Unit. </returns>
    Public Function ToMeasurementUnit(ByVal value As Tsp2.MeasureUnit) As Arebis.TypedUnits.Unit
        Dim result As Arebis.TypedUnits.Unit = Me.DefaultFunctionUnit
        Select Case value
            Case Tsp2.MeasureUnit.Ampere
                result = Arebis.StandardUnits.ElectricUnits.Ampere
            Case Tsp2.MeasureUnit.Volt
                result = Arebis.StandardUnits.ElectricUnits.Volt
            Case Tsp2.MeasureUnit.Ohm
                result = Arebis.StandardUnits.ElectricUnits.Ohm
            Case Tsp2.MeasureUnit.Watt
                result = Arebis.StandardUnits.EnergyUnits.Watt
        End Select
        Return result
    End Function

    ''' <summary> Writes and reads back the Measure Unit. </summary>
    ''' <param name="value"> The  Measure Unit. </param>
    ''' <returns> The <see cref="MeasurementUnit">Measure Unit</see> or none if unknown. </returns>
    Public Function ApplyMeasureUnit(ByVal value As MeasureUnit) As MeasureUnit?
        Me.WriteMeasureUnit(value)
        Return Me.QueryMeasureUnit
    End Function

    ''' <summary> The Unit. </summary>
    Private _MeasureUnit As MeasureUnit?

    ''' <summary> Gets or sets the cached measure Unit.  This is the actual unit for measurement. </summary>
    ''' <value> The <see cref="MeasurementUnit">Measure Unit</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property MeasureUnit As MeasureUnit?
        Get
            Return Me._MeasureUnit
        End Get
        Protected Set(ByVal value As MeasureUnit?)
            If Not Nullable.Equals(Me.MeasureUnit, value) Then
                Me._MeasureUnit = value
                If value.HasValue Then
                    Me.MeasurementUnit = Me.ToMeasurementUnit(value.Value)
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Measure Unit query command. </summary>
    ''' <value> The Unit query command. </value>
    Protected Overridable ReadOnly Property MeasureUnitQueryCommand As String = "_G.smu.measure.unit"

    ''' <summary> Queries the Measure Unit. </summary>
    ''' <returns> The <see cref="MeasureUnit">Measure Unit</see> or none if unknown. </returns>
    Public Overridable Function QueryMeasureUnit() As MeasureUnit?
        Dim mode As String = Me.MeasureUnit.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.MeasureUnitQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(MeasureSubsystemBase)}.{NameOf(MeasureUnit)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.MeasureUnit = New MeasureUnit?
        Else
            Dim se As New StringEnumerator(Of MeasureUnit)
            Me.MeasureUnit = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.MeasureUnit
    End Function

    ''' <summary> Gets or sets the Measure Unit command format. </summary>
    ''' <value> The Unit command format. </value>
    Protected Overridable ReadOnly Property MeasureUnitCommandFormat As String = "_G.smu.measure.unit={0}"


    ''' <summary> Writes the Measure Unit without reading back the value from the device. </summary>
    ''' <param name="value"> The Unit. </param>
    ''' <returns> The <see cref="MeasureUnit">Measure Unit</see> or none if unknown. </returns>
    Public Overridable Function WriteMeasureUnit(ByVal value As MeasureUnit) As MeasureUnit?
        Me.Session.WriteLine(Me.MeasureUnitCommandFormat, value.ExtractBetween())
        Me.MeasureUnit = value
        Return Me.MeasureUnit
    End Function

#End Region

End Class

''' <summary> Specifies the units. </summary>
Public Enum MeasureUnit
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Volt (smu.UNIT_VOLT)")> Volt
    <ComponentModel.Description("Ohm (smu.UNIT_OHM)")> Ohm
    <ComponentModel.Description("Ampere (smu.UNIT_AMP)")> Ampere
    <ComponentModel.Description("Watt (smu.UNIT_WATT)")> Watt
End Enum


''' <summary> Specifies the function modes. </summary>
Public Enum MeasureFunctionMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("DC Voltage (smu.FUNC_DC_VOLTAGE)")> VoltageDC
    <ComponentModel.Description("DC Current (smu.FUNC_DC_CURRENT)")> CurrentDC
    <ComponentModel.Description("Resistance (smu.FUNC_RESISTANCE)")> Resistance
End Enum

