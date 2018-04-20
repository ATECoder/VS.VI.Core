''' <summary> Defines a SCPI Measure Subsystem for a generic source measure instrument. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
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
        Me.Readings?.Reset()
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        ' TO_DO: the readings are initialized when the format system is reset.
        Me.Readings = New Readings
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

#Region " READ, FETCH "

    ''' <summary> Gets the fetch command. </summary>
    ''' <value> The fetch command. </value>
    Protected Overrides ReadOnly Property FetchCommand As String = ":FETCH?"

    ''' <summary> Gets the read command. </summary>
    ''' <value> The read command. </value>
    Protected Overrides ReadOnly Property ReadCommand As String = ":READ?"

#End Region

#End Region

#Region " PARSE READING "

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

End Class

