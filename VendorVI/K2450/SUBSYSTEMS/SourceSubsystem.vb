''' <summary> Source subsystem. </summary>
''' <license> (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="01/03/2018" by="David" revision=""> Created. </history>
Public Class SourceSubsystem
    Inherits SourceSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Level = 0
        Me.Limit = 0.000105
        Me.Range = New Double?
        Me.AutoRangeEnabled = True
        Me.AutoDelayEnabled = True
        Me.FunctionModeRanges(SourceFunctionMode.CurrentDC).SetRange(-1.05, 1.05)
        Me.FunctionModeRanges(SourceFunctionMode.VoltageDC).SetRange(-210, 210)
        Me.FunctionMode = SourceFunctionMode.VoltageDC
        Me.Range = 0.02
        Me.LimitTripped = False
        Me.OutputEnabled = False
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

#Region " AUTO DELAY ENABLED "

    ''' <summary> Gets the automatic Delay enabled command Format. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    Protected Overrides ReadOnly Property AutoDelayEnabledCommandFormat As String = "_G.smu.source.autodelay={0:'smu.ON';'smu.ON';'smu.OFF'}"

    ''' <summary> Gets the automatic Delay enabled query command. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    Protected Overrides ReadOnly Property AutoDelayEnabledQueryCommand As String = "_G.print(_G.smu.source.autodelay==smu.ON)"

#End Region

#Region " AUTO RANGE STATE "

    ''' <summary> The Auto Range state command format. </summary>
    ''' <value> The automatic range state command format. </value>
    Protected Overrides ReadOnly Property AutoRangeStateCommandFormat As String = "_G.smu.source.autorange={0}"

    ''' <summary> Gets or sets the Auto Range state query command. </summary>
    ''' <value> The AutoRange state query command. </value>
    Protected Overrides ReadOnly Property AutoRangeStateQueryCommand As String = "_G.smu.source.autorange"

#End Region

#Region " AUTO RANGE ENABLED "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = "_G.smu.source.autorange={0:'smu.ON';'smu.ON';'smu.OFF'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = "_G.print(_G.smu.source.autorange==smu.ON)"

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command. </value>
    Protected Overrides ReadOnly Property FunctionModeQueryCommand As String = "_G.smu.source.func"

    ''' <summary> Gets or sets the function mode command format. </summary>
    ''' <value> The function mode command format. </value>
    Protected Overrides ReadOnly Property FunctionModeCommandFormat As String = "_G.smu.source.func={0}"

#End Region

#Region " LEVEL "

    ''' <summary> Gets the level query command. </summary>
    ''' <value> The level query command. </value>
    Protected Overrides ReadOnly Property LevelQueryCommand As String = "_G.smu.source.level"

    ''' <summary> Gets the level command format. </summary>
    ''' <value> The level command format. </value>
    Protected Overrides ReadOnly Property LevelCommandFormat As String = "_G.smu.source.level={0}"

#End Region

#Region " LIMIT "

    ''' <summary> Gets the limit query command. </summary>
    ''' <value> The limit query command. </value>
    Protected Overrides ReadOnly Property LimitQueryCommandFormat As String = "_G.smu.source.{0}limit.level"

    ''' <summary> Gets the limit command format. </summary>
    ''' <value> The limit command format. </value>
    Protected Overrides ReadOnly Property LimitCommandFormat As String = "_G.smu.source.{0}limit.level={1}"

#End Region

#Region " LIMIT TRIPPED "

    Protected Overrides ReadOnly Property LimitTrippedPrintCommandFormat As String = "_G.print(_G.smu.source.{0}limit.tripped==smu.ON)"

#End Region

#Region " OUTPUT ENABLED "

    ''' <summary> Gets or sets the Output enabled command Format. </summary>
    ''' <value> The Output enabled query command. </value>
    Protected Overrides ReadOnly Property OutputEnabledCommandFormat As String = "_G.smu.source.output={0:'smu.ON';'smu.ON';'smu.OFF'}"

    ''' <summary> Gets or sets the Output enabled query print command. </summary>
    ''' <value> The Output enabled query command. </value>
    Protected Overrides ReadOnly Property OutputEnabledQueryCommand As String = "_G.print(_G.smu.source.output==smu.ON)"

#End Region

#Region " RANGE "

    ''' <summary> Gets the Range query command. </summary>
    ''' <value> The Range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = "_G.print(_G.smu.source.range)"

    ''' <summary> Gets the Range command format. </summary>
    ''' <value> The Range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = "_G.smu.source.range={0}"

#End Region

#End Region


End Class
