Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
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
    Inherits MeasureSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="MeasureSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.PowerLineCycles = 1
        Me.AutoRangeState = OnOffState.On
        Me.AutoZeroEnabled = True
        Me.FilterCount = 10
        Me.FilterEnabled = False
        Me.MovingAverageFilterEnabled = False
        Me.OpenDetectorEnabled = False
        Me.FilterWindow = 0.001
        For Each fm As MeasureFunctionMode In [Enum].GetValues(GetType(MeasureFunctionMode))
            Select Case fm
                Case MeasureFunctionMode.CurrentDC
                    Me.FunctionModeRanges(fm).SetRange(0, 1)
                Case MeasureFunctionMode.VoltageDC
                    Me.FunctionModeRanges(fm).SetRange(0, 200)
                Case MeasureFunctionMode.Resistance
                    Me.FunctionModeRanges(fm).SetRange(0, 200000000.0)
            End Select
        Next
        Me.FunctionMode = MeasureFunctionMode.CurrentDC
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

    ''' <summary> The aperture range in seconds. </summary>
    ''' <value> The aperture range. </value>
    Public Overrides ReadOnly Property ApertureRange As Core.Pith.RangeR = New isr.Core.Pith.RangeR(0.000166667, 0.166667)

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
    Protected Overrides ReadOnly Property AutoRangeEnabledPrintCommand As String = "_G.print(_G.smu.measure.autorange==smu.ON)"

#End Region

#Region " AUTO ZERO ENABLED "

    ''' <summary> Gets the automatic Zero enabled command Format. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledCommandFormat As String = "_G.smu.measure.autozero.enable={0:'smu.ON';'smu.ON';'smu.OFF'}"

    ''' <summary> Gets the automatic Zero enabled query command. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledPrintCommand As String = "_G.print(_G.smu.measure.autozero.enable==smu.ON)"

#End Region

#Region " AUTO ZERO ONCE "

    ''' <summary> Gets or sets the automatic zero once command. </summary>
    ''' <value> The automatic zero once command. </value>
    Protected Overrides ReadOnly Property AutoZeroOnceCommand As String = "_G.smu.measure.autozero.once()"

#End Region

#Region " FILTER "

#Region " FILTER COUNT "

    ''' <summary> The Filter Count range in seconds. </summary>
    ''' <value> The FilterCount range. </value>
    Public Overrides ReadOnly Property FilterCountRange As Core.Pith.RangeI = New isr.Core.Pith.RangeI(1, 100)

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

    ''' <summary> The Filter Window range in seconds. </summary>
    ''' <value> The FilterWindow range. </value>
    Public Overrides ReadOnly Property FilterWindowRange As Core.Pith.RangeR = New isr.Core.Pith.RangeR(0, 0.1)

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
    Protected Overrides ReadOnly Property FrontTerminalsSelectedCommandFormat As String = "_G.smu.measure.terminals={0:'smu.TERMINALS_FRONT';'smu.TERMINALS_FRONT';'smu.TERMINALS_READ'}"

    ''' <summary> Gets or sets the front terminals selected query command. </summary>
    ''' <value> The front terminals selected query command. </value>
    Protected Overrides ReadOnly Property FrontTerminalsSelectedPrintCommand As String = "_G.print(_G.smu.measure.terminals==smu.TERMINALS_FRONT)"

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

    Public Overrides ReadOnly Property PowerLineCyclesRange As Core.Pith.RangeR = New isr.Core.Pith.RangeR(0.01, 10)

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
    Protected Overrides ReadOnly Property RangePrintCommand As String = "_G.print(_G.smu.measure.range)"

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
    Protected Overrides ReadOnly Property RemoteSenseSelectedPrintCommand As String = "_G.print(_G.smu.measure.sense==smu.SENSE_4WIRE)"

#End Region

#Region " UNIT "

    ''' <summary> Gets or sets the unit command. </summary>
    ''' <value> The unit query command. </value>
    Protected Overrides ReadOnly Property UnitQueryCommand As String = "_G.smu.measure.unit"

    ''' <summary> Gets or sets the unit command format. </summary>
    ''' <value> The unit command format. </value>
    Protected Overrides ReadOnly Property UnitCommandFormat As String = "_G.smu.measure.unit={0}"

#End Region

#End Region

End Class
