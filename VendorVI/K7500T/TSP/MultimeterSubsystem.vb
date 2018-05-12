''' <summary> Measure subsystem. </summary>
''' <license> (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="01/03/2018" by="David" revision=""> Created. </history>
Public Class MultimeterSubsystem
    Inherits MultimeterSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="MultimeterSubsystem" /> class. </summary>
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

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        ' TO_DO: the readings are initialized when the format system is reset.
        Me.Readings = New Readings
        MyBase.InitKnownState()
        Dim lineFrequency As Double = Me.StatusSubsystem.LineFrequency.GetValueOrDefault(60)
        If lineFrequency = 50 Then
            Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 12)
        Else
            Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 15)
        End If
        Me.ApertureRange = New isr.Core.Pith.RangeR(Me.PowerLineCyclesRange.Min / lineFrequency, Me.PowerLineCyclesRange.Max / lineFrequency)
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FilterCountRange = New isr.Core.Pith.RangeI(1, 100)
        Me.FilterWindowRange = New isr.Core.Pith.RangeR(0, 0.1)
        Me.PowerLineCycles = 0.5
        Me.AutoZeroEnabled = True
        Me.FilterCount = 10
        Me.FilterEnabled = False
        Me.MovingAverageFilterEnabled = False
        Me.OpenDetectorEnabled = False
        Me.FilterWindow = 0.001
        For Each fmode As VI.Tsp2.MultimeterFunctionMode In [Enum].GetValues(GetType(VI.Tsp2.MultimeterFunctionMode))
            Select Case fmode
                Case VI.Tsp2.MultimeterFunctionMode.CurrentDC, VI.Tsp2.MultimeterFunctionMode.CurrentAC
                    Me.FunctionModeRanges(fmode).SetRange(0, 10)
                Case VI.Tsp2.MultimeterFunctionMode.VoltageDC
                    Me.FunctionModeRanges(fmode).SetRange(0, 1000)
                Case VI.Tsp2.MultimeterFunctionMode.VoltageAC
                    Me.FunctionModeRanges(fmode).SetRange(0, 700)
                Case VI.Tsp2.MultimeterFunctionMode.ResistanceTwoWire
                    Me.FunctionModeRanges(fmode).SetRange(0, 1000000000.0)
                Case VI.Tsp2.MultimeterFunctionMode.ResistanceFourWire
                    Me.FunctionModeRanges(fmode).SetRange(0, 1000000000.0)
                Case VI.Tsp2.MultimeterFunctionMode.Continuity
                    Me.FunctionModeRanges(fmode).SetRange(0, 1000)
                Case VI.Tsp2.MultimeterFunctionMode.Diode
                    Me.FunctionModeRanges(fmode).SetRange(0, 10)
                Case VI.Tsp2.MultimeterFunctionMode.Capacitance
                    Me.FunctionModeRanges(fmode).SetRange(0, 0.001)
                Case VI.Tsp2.MultimeterFunctionMode.Ratio
                    Me.FunctionModeRanges(fmode).SetRange(0, 1000)
            End Select
        Next
        Me.SafePostPropertyChanged(NameOf(MultimeterSubsystemBase.FunctionModeRanges))
        Me.FunctionMode = VI.Tsp2.MultimeterFunctionMode.VoltageDC
        Me.Range = 100
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

#Region " APERTURE "

    ''' <summary> Gets the Aperture query command. </summary>
    ''' <value> The Aperture query command. </value>
    Protected Overrides ReadOnly Property ApertureQueryCommand As String = "_G.print(_G.dmm.measure.aperture)"

    ''' <summary> Gets the Aperture command format. </summary>
    ''' <value> The Aperture command format. </value>
    Protected Overrides ReadOnly Property ApertureCommandFormat As String = "_G.dmm.measure.aperture={0}"

#End Region

#Region " AUTO DELAY ENABLED "

    ''' <summary> Gets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    Protected Overrides ReadOnly Property AutoDelayEnabledCommandFormat As String = "_G.dmm.measure.autodelay={0:'dmm.ON';'dmm.ON';'dmm.OFF'}"

    ''' <summary> Gets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    Protected Overrides ReadOnly Property AutoDelayEnabledQueryCommand As String = "_G.print(_G.dmm.measure.autodelay==dmm.ON)"

#End Region

#Region " AUTO RANGE ENABLED "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = "_G.dmm.measure.autorange={0:'dmm.ON';'dmm.ON';'dmm.OFF'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = "_G.print(_G.dmm.measure.autorange==dmm.ON)"

#End Region

#Region " AUTO ZERO ENABLED "

    ''' <summary> Gets the automatic Zero enabled command Format. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledCommandFormat As String = "_G.dmm.measure.autozero.enable={0:'dmm.ON';'dmm.ON';'dmm.OFF'}"

    ''' <summary> Gets the automatic Zero enabled query command. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledQueryCommand As String = "_G.print(_G.dmm.measure.autozero.enable==dmm.ON)"

#End Region

#Region " AUTO ZERO ONCE "

    ''' <summary> Gets or sets the automatic zero once command. </summary>
    ''' <value> The automatic zero once command. </value>
    Protected Overrides ReadOnly Property AutoZeroOnceCommand As String = "_G.dmm.measure.autozero.once()"

#End Region

#Region " BIAS "

#Region " BIAS ACTUAL "

    ''' <summary> Gets the Bias Actual query command. </summary>
    ''' <value> The BiasActual query command. </value>
    Protected Overrides ReadOnly Property BiasActualQueryCommand As String = "_G.print(_G.dmm.measure.bias.Actual)"

#End Region

#Region " BIAS LEVEL "

    ''' <summary> Gets the Bias Level query command. </summary>
    ''' <value> The BiasLevel query command. </value>
    Protected Overrides ReadOnly Property BiasLevelQueryCommand As String = "_G.dmm.measure.bias.Level={0}"

    ''' <summary> Gets the Bias Level command format. </summary>
    ''' <value> The BiasLevel command format. </value>
    Protected Overrides ReadOnly Property BiasLevelCommandFormat As String = "G_.print(_G.dmm.measure.bias.Level)"

#End Region

#End Region

#Region " FILTER "

#Region " FILTER COUNT "

    ''' <summary> Gets the Filter Count query command. </summary>
    ''' <value> The FilterCount query command. </value>
    Protected Overrides ReadOnly Property FilterCountQueryCommand As String = "_G.print(_G.dmm.measure.filter.count)"

    ''' <summary> Gets the Filter Count command format. </summary>
    ''' <value> The FilterCount command format. </value>
    Protected Overrides ReadOnly Property FilterCountCommandFormat As String = "_G.dmm.measure.filter.count={0}"

#End Region

#Region " FILTER ENABLED "

    ''' <summary> Gets the Filter enabled command Format. </summary>
    ''' <value> The Filter enabled query command. </value>
    Protected Overrides ReadOnly Property FilterEnabledCommandFormat As String = "_G.dmm.measure.filter.enable={0:'dmm.ON';'dmm.ON';'dmm.OFF'}"

    ''' <summary> Gets the Filter enabled query command. </summary>
    ''' <value> The Filter enabled query command. </value>
    Protected Overrides ReadOnly Property FilterEnabledQueryCommand As String = "_G.print(_G.dmm.measure.filter.enable==dmm.ON)"

#End Region

#Region " MOVING AVERAGE ENABLED "

    ''' <summary> Gets the moving average filter enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property MovingAverageFilterEnabledCommandFormat As String = "_G.dmm.measure.filter.type={0:'dmm.FILTER_MOVING_AVG';'dmm.FILTER_MOVING_AVG';'dmm.FILTER_REPEAT_AVG'}"

    ''' <summary> Gets the moving average filter enabled query command. </summary>
    ''' <value> The moving average filter enabled query command. </value>
    Protected Overrides ReadOnly Property MovingAverageFilterEnabledQueryCommand As String = "_G.print(_G.dmm.measure.filter.type==dmm.FILTER_MOVING_AVG)"

#End Region

#Region " FILTER Window "

    ''' <summary> Gets the Filter Window query command. </summary>
    ''' <value> The FilterWindow query command. </value>
    Protected Overrides ReadOnly Property FilterWindowQueryCommand As String = "_G.dmm.measure.filter.window={0}"

    ''' <summary> Gets the Filter Window command format. </summary>
    ''' <value> The FilterWindow command format. </value>
    Protected Overrides ReadOnly Property FilterWindowCommandFormat As String = "G_.print(_G.dmm.measure.filter.window)"

#End Region

#End Region

#Region " FRONT TERMINALS SELECTED "

    ''' <summary> Gets or sets the front terminals selected command format. </summary>
    ''' <value> The front terminals selected command format. </value>
    Protected Overrides ReadOnly Property FrontTerminalsSelectedCommandFormat As String = "_G.dmm.terminals={0:'dmm.TERMINALS_FRONT';'dmm.TERMINALS_FRONT';'dmm.TERMINALS_REAR'}"

    ''' <summary> Gets or sets the front terminals selected query command. </summary>
    ''' <value> The front terminals selected query command. </value>
    Protected Overrides ReadOnly Property FrontTerminalsSelectedQueryCommand As String = "_G.print(_G.dmm.terminals==dmm.TERMINALS_FRONT)"

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command. </value>
    Protected Overrides ReadOnly Property FunctionModeQueryCommand As String = "_G.dmm.measure.func"

    ''' <summary> Gets or sets the function mode command format. </summary>
    ''' <value> The function mode command format. </value>
    ''' <remarks> Query command uses <see cref="VI.Pith.SessionBase.QueryPrintTrimEnd"/></remarks>
    Protected Overrides ReadOnly Property FunctionModeCommandFormat As String = "_G.dmm.measure.func={0}"

#Region " UNIT "

    ''' <summary> Gets or sets the Measure Unit query command. </summary>
    ''' <remarks>
    ''' The query command uses the
    ''' <see cref="M:VI.Pith.SessionBase.QueryPrintTrimEnd(System.Int32,System.String)" />
    ''' </remarks>
    ''' <value> The Unit query command. </value>
    Protected Overrides ReadOnly Property MultimeterMeasurementUnitQueryCommand As String = "_G.smu.measure.unit"

    ''' <summary> Gets or sets the Measure Unit command format. </summary>
    ''' <value> The Unit command format. </value>
    Protected Overrides ReadOnly Property MultimeterMeasurementUnitCommandFormat As String = "_G.smu.measure.unit={0}"

#End Region

#End Region

#Region " LIMIT 1 "

#Region " LIMIT1 AUTO CLEAR "

    ''' <summary> Gets the Limit1 Auto Clear command Format. </summary>
    ''' <value> The Limit1 AutoClear query command. </value>
    Protected Overrides ReadOnly Property Limit1AutoClearCommandFormat As String = "_G.dmm.measure.limit1.autoclear={0:'dmm.ON';'dmm.ON';'dmm.OFF'}"

    ''' <summary> Gets the Limit1 Auto Clear query command. </summary>
    ''' <value> The Limit1 AutoClear query command. </value>
    Protected Overrides ReadOnly Property Limit1AutoClearQueryCommand As String = "G_.print(_G.dmm.measure.limit1.autoclear==dmm.ON)"

#End Region

#Region " LIMIT1 ENABLED "

    ''' <summary> Gets the Limit1 enabled command Format. </summary>
    ''' <value> The Limit1 enabled query command. </value>
    Protected Overrides ReadOnly Property Limit1EnabledCommandFormat As String = "_G.dmm.measure.limit1.enable={0:'dmm.ON';'dmm.ON';'dmm.OFF'}"

    ''' <summary> Gets the Limit1 enabled query command. </summary>
    ''' <value> The Limit1 enabled query command. </value>
    Protected Overrides ReadOnly Property Limit1EnabledQueryCommand As String = "G_.print(_G.dmm.measure.limit1.enable==dmm.ON)"

#End Region

#Region " LIMIT1 LOWER LEVEL "

    ''' <summary> Gets the Limit1 Lower Level command format. </summary>
    ''' <value> The Limit1LowerLevel command format. </value>
    Protected Overrides ReadOnly Property Limit1LowerLevelCommandFormat As String = "_G.dmm.measure.limit1.low.value={0}"

    ''' <summary> Gets the Limit1 Lower Level query command. </summary>
    ''' <value> The Limit1LowerLevel query command. </value>
    Protected Overrides ReadOnly Property Limit1LowerLevelQueryCommand As String = "G_.print(_G.dmm.measure.limit1.low.value)"

#End Region

#Region " Limit1 UPPER LEVEL "

    ''' <summary> Gets the Limit1 Upper Level command format. </summary>
    ''' <value> The Limit1UpperLevel command format. </value>
    Protected Overrides ReadOnly Property Limit1UpperLevelCommandFormat As String = "_G.dmm.measure.limit1.high.value={0}"

    ''' <summary> Gets the Limit1 Upper Level query command. </summary>
    ''' <value> The Limit1UpperLevel query command. </value>
    Protected Overrides ReadOnly Property Limit1UpperLevelQueryCommand As String = "G_.print(_G.dmm.measure.limit1.high.value)"

#End Region
#End Region

#Region " LIMIT 2 "

#Region " LIMIT2 AUTO CLEAR "

    ''' <summary> Gets the Limit2 Auto Clear command Format. </summary>
    ''' <value> The Limit2 AutoClear query command. </value>
    Protected Overrides ReadOnly Property Limit2AutoClearCommandFormat As String = "_G.dmm.measure.limit2.autoclear={0:'dmm.ON';'dmm.ON';'dmm.OFF'}"

    ''' <summary> Gets the Limit2 Auto Clear query command. </summary>
    ''' <value> The Limit2 AutoClear query command. </value>
    Protected Overrides ReadOnly Property Limit2AutoClearQueryCommand As String = "G_.print(_G.dmm.measure.limit2.autoclear==dmm.ON)"

#End Region

#Region " LIMIT2 ENABLED "

    ''' <summary> Gets the Limit2 enabled command Format. </summary>
    ''' <value> The Limit2 enabled query command. </value>
    Protected Overrides ReadOnly Property Limit2EnabledCommandFormat As String = "_G.dmm.measure.limit2.enable={0:'dmm.ON';'dmm.ON';'dmm.OFF'}"

    ''' <summary> Gets the Limit2 enabled query command. </summary>
    ''' <value> The Limit2 enabled query command. </value>
    Protected Overrides ReadOnly Property Limit2EnabledQueryCommand As String = "G_.print(_G.dmm.measure.limit2.enable==dmm.ON)"

#End Region

#Region " LIMIT2 LOWER LEVEL "

    ''' <summary> Gets the Limit2 Lower Level command format. </summary>
    ''' <value> The Limit2LowerLevel command format. </value>
    Protected Overrides ReadOnly Property Limit2LowerLevelCommandFormat As String = "_G.dmm.measure.limit2.low.value={0}"

    ''' <summary> Gets the Limit2 Lower Level query command. </summary>
    ''' <value> The Limit2LowerLevel query command. </value>
    Protected Overrides ReadOnly Property Limit2LowerLevelQueryCommand As String = "G_.print(_G.dmm.measure.limit2.low.value)"

#End Region

#Region " LIMIT2 UPPER LEVEL "

    ''' <summary> Gets the Limit2 Upper Level command format. </summary>
    ''' <value> The Limit2UpperLevel command format. </value>
    Protected Overrides ReadOnly Property Limit2UpperLevelCommandFormat As String = "_G.dmm.measure.limit1.high.value={0}"

    ''' <summary> Gets the Limit2 Upper Level query command. </summary>
    ''' <value> The Limit2UpperLevel query command. </value>
    Protected Overrides ReadOnly Property Limit2UpperLevelQueryCommand As String = "G_.print(_G.dmm.measure.limit2.high.value)"

#End Region
#End Region

#Region " OPEN DETECTOR ENABLED "

    ''' <summary> Gets the open detector enabled command Format. </summary>
    ''' <value> The open detector enabled query command. </value>
    Protected Overrides ReadOnly Property OpenDetectorEnabledCommandFormat As String = ""

    ''' <summary> Gets the open detector enabled query command. </summary>
    ''' <value> The open detector enabled query command. </value>
    Protected Overrides ReadOnly Property OpenDetectorEnabledQueryCommand As String = ""

#End Region

#Region " POWER LINE CYCLES "

    ''' <summary> Gets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesCommandFormat As String = "_G.dmm.measure.nplc={0}"

    ''' <summary> Gets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesQueryCommand As String = "_G.print(_G.dmm.measure.nplc)"

#End Region

#Region " RANGE "

    ''' <summary> Gets the Range query command. </summary>
    ''' <value> The Range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = "_G.print(_G.dmm.measure.range)"

    ''' <summary> Gets the Range command format. </summary>
    ''' <value> The Range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = "_G.dmm.measure.range={0}"

#End Region

#Region " READ "

    Protected Overrides ReadOnly Property ReadBufferQueryCommandFormat As String = "_G.print(_G.dmm.measure.read({0}))"

    ''' <summary> Gets the Measure query command. </summary>
    ''' <value> The Aperture query command. </value>
    Protected Overrides ReadOnly Property MeasureQueryCommand As String = "_G.print(_G.dmm.measure.read())"

#End Region

#End Region

#Region " MEASURE "

    Public Overrides Function Measure() As Double?
        Me.Session.MakeEmulatedReplyIfEmpty(Me.Readings.Reading.Generator.Value.ToString)
        Return MyBase.Measure()
    End Function

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
