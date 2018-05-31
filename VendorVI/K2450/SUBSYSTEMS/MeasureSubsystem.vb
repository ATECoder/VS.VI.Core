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
Public Class MeasureSubsystem
    Inherits VI.Tsp2.MeasureSubsystemBase

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
        Me.Readings.Reset()
        Me.SafePostPropertyChanged(NameOf(MeasureSubsystem.Readings))
    End Sub

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.Readings.Initialize(ReadingTypes.Current)
        Me.ApertureRange = New isr.Core.Pith.RangeR(0.000166667, 0.166667)
        Me.FilterCountRange = New isr.Core.Pith.RangeI(1, 100)
        Me.FilterWindowRange = New isr.Core.Pith.RangeR(0, 0.1)
        Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.01, 10)
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Readings = New Readings
        Me.PowerLineCycles = 1
        Me.AutoRangeState = OnOffState.On
        Me.AutoZeroEnabled = True
        Me.FilterCount = 10
        Me.FilterEnabled = False
        Me.MovingAverageFilterEnabled = False
        Me.OpenDetectorEnabled = False
        Me.FilterWindow = 0.001
        Me.FunctionModeRanges(VI.Tsp2.MeasureFunctionMode.CurrentDC).SetRange(0, 1)
        Me.FunctionModeRanges(VI.Tsp2.MeasureFunctionMode.VoltageDC).SetRange(0, 200)
        Me.FunctionModeRanges(VI.Tsp2.MeasureFunctionMode.Resistance).SetRange(0, 200000000.0)
        Me.FunctionModeDecimalPlaces(VI.Tsp2.MeasureFunctionMode.Resistance) = 0
        Me.FunctionMode = VI.Tsp2.MeasureFunctionMode.CurrentDC
        Me.Range = 0.000001
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
    Protected Overrides ReadOnly Property ApertureQueryCommand As String = ""

    ''' <summary> Gets the Aperture command format. </summary>
    ''' <value> The Aperture command format. </value>
    Protected Overrides ReadOnly Property ApertureCommandFormat As String = ""

#End Region

#Region " AUTO RANGE STATE "

    ''' <summary> The Auto Range state command format. </summary>
    ''' <value> The automatic range state command format. </value>
    Protected Overrides ReadOnly Property AutoRangeStateCommandFormat As String = "_G.smu.measure.autorange={0}"

    ''' <summary> Gets or sets the Auto Range state query command. </summary>
    ''' <value> The AutoRange state query command. </value>
    Protected Overrides ReadOnly Property AutoRangeStateQueryCommand As String = "_G.smu.measure.autorange"

#End Region

#Region " AUTO RANGE ENABLED "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = "_G.smu.measure.autorange={0:'smu.ON';'smu.ON';'smu.OFF'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = "_G.print(_G.smu.measure.autorange==smu.ON)"

#End Region

#Region " AUTO ZERO ENABLED "

    ''' <summary> Gets the automatic Zero enabled command Format. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledCommandFormat As String = "_G.smu.measure.autozero.enable={0:'smu.ON';'smu.ON';'smu.OFF'}"

    ''' <summary> Gets the automatic Zero enabled query command. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledQueryCommand As String = "_G.print(_G.smu.measure.autozero.enable==smu.ON)"

#End Region

#Region " AUTO ZERO ONCE "

    ''' <summary> Gets or sets the automatic zero once command. </summary>
    ''' <value> The automatic zero once command. </value>
    Protected Overrides ReadOnly Property AutoZeroOnceCommand As String = "_G.smu.measure.autozero.once()"

#End Region

#Region " FILTER "

#Region " FILTER COUNT "

    ''' <summary> Gets the Filter Count query command. </summary>
    ''' <value> The FilterCount query command. </value>
    Protected Overrides ReadOnly Property FilterCountQueryCommand As String = "_G.print(_G.smu.measure.filter.count)"

    ''' <summary> Gets the Filter Count command format. </summary>
    ''' <value> The FilterCount command format. </value>
    Protected Overrides ReadOnly Property FilterCountCommandFormat As String = "_G.smu.measure.filter.count={0}"

#End Region

#Region " FILTER ENABLED "

    ''' <summary> Gets the Filter enabled command Format. </summary>
    ''' <value> The Filter enabled query command. </value>
    Protected Overrides ReadOnly Property FilterEnabledCommandFormat As String = "_G.smu.measure.filter.enable={0:'smu.ON';'smu.ON';'smu.OFF'}"

    ''' <summary> Gets the Filter enabled query command. </summary>
    ''' <value> The Filter enabled query command. </value>
    Protected Overrides ReadOnly Property FilterEnabledQueryCommand As String = "_G.print(_G.smu.measure.filter.enable==smu.ON)"

#End Region

#Region " MOVING AVERAGE ENABLED "

    ''' <summary> Gets the moving average filter enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property MovingAverageFilterEnabledCommandFormat As String = "_G.smu.measure.filter.type={0:'smu.FILTER_MOVING_AVG';'smu.FILTER_MOVING_AVG';'smu.FILTER_REPEAT_AVG'}"

    ''' <summary> Gets the moving average filter enabled query command. </summary>
    ''' <value> The moving average filter enabled query command. </value>
    Protected Overrides ReadOnly Property MovingAverageFilterEnabledQueryCommand As String = "_G.print(_G.smu.measure.filter.type==smu.FILTER_MOVING_AVG)"

#End Region

#Region " FILTER Window "

    ''' <summary> Gets the Filter Window query command. </summary>
    ''' <value> The FilterWindow query command. </value>
    Protected Overrides ReadOnly Property FilterWindowQueryCommand As String = ""

    ''' <summary> Gets the Filter Window command format. </summary>
    ''' <value> The FilterWindow command format. </value>
    Protected Overrides ReadOnly Property FilterWindowCommandFormat As String = ""

#End Region

#End Region

#Region " FRONT TERMINALS SELECTED "

    ''' <summary> Gets or sets the front terminals selected command format. </summary>
    ''' <value> The front terminals selected command format. </value>
    Protected Overrides ReadOnly Property FrontTerminalsSelectedCommandFormat As String = "_G.smu.measure.terminals={0:'smu.TERMINALS_FRONT';'smu.TERMINALS_FRONT';'smu.TERMINALS_REAR'}"

    ''' <summary> Gets or sets the front terminals selected query command. </summary>
    ''' <value> The front terminals selected query command. </value>
    Protected Overrides ReadOnly Property FrontTerminalsSelectedQueryCommand As String = "_G.print(_G.smu.measure.terminals==smu.TERMINALS_FRONT)"

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command. </value>
    Protected Overrides ReadOnly Property FunctionModeQueryCommand As String = "_G.smu.measure.func"

    ''' <summary> Gets or sets the function mode command format. </summary>
    ''' <value> The function mode command format. </value>
    Protected Overrides ReadOnly Property FunctionModeCommandFormat As String = "_G.smu.measure.func={0}"

#End Region

#Region " MEASURE "

    ''' <summary> Gets the Measure query command. </summary>
    ''' <value> The Aperture query command. </value>
    Protected Overrides ReadOnly Property MeasureQueryCommand As String = "_G.print(_G.smu.measure.read())"

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
    Protected Overrides ReadOnly Property PowerLineCyclesCommandFormat As String = "_G.smu.measure.nplc={0}"

    ''' <summary> Gets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesQueryCommand As String = "_G.print(_G.smu.measure.nplc)"

#End Region

#Region " RANGE "

    ''' <summary> Gets the Range query command. </summary>
    ''' <value> The Range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = "_G.print(_G.smu.measure.range)"

    ''' <summary> Gets the Range command format. </summary>
    ''' <value> The Range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = "_G.smu.measure.range={0}"

#End Region

#Region " REMOTE SENSE SELECTED "

    ''' <summary> Gets or sets the remote sense selected command format. </summary>
    ''' <value> The remote sense selected command format. </value>
    Protected Overrides ReadOnly Property RemoteSenseSelectedCommandFormat As String = "_G.smu.measure.sense={0:'smu.SENSE_4WIRE';'smu.SENSE_4WIRE';'smu.SENSE_2WIRE'}"

    ''' <summary> Gets or sets the remote sense selected query command. </summary>
    ''' <value> The remote sense selected query command. </value>
    Protected Overrides ReadOnly Property RemoteSenseSelectedQueryCommand As String = "_G.print(_G.smu.measure.sense==smu.SENSE_4WIRE)"

#End Region

#Region " UNIT "

    ''' <summary> Gets or sets the unit command. </summary>
    ''' <value> The unit query command. </value>
    Protected Overrides ReadOnly Property MeasureUnitQueryCommand As String = "_G.smu.measure.unit"

    ''' <summary> Gets or sets the unit command format. </summary>
    ''' <value> The unit command format. </value>
    Protected Overrides ReadOnly Property MeasureUnitCommandFormat As String = "_G.smu.measure.unit={0}"

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
