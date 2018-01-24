Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.Tsp
''' <summary> Defines a Multimeter Subsystem for a Keithley 3700 instrument. </summary>
''' <license> (c) 2016 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2016" by="David" revision=""> Created. </history>
Public Class MultimeterSubsystem
    Inherits isr.VI.Tsp.MultimeterSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ChannelSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub
#End Region

#Region " I PRESETTABLE "

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.ApertureRange = New isr.Core.Pith.RangeR(0.00000833, 0.25)
        Me.FilterCountRange = New isr.Core.Pith.RangeI(1, 100)
        Me.FilterWindowRange = New isr.Core.Pith.RangeR(0, 0.1)
        Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 15)
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.PowerLineCycles = 1
        Me.AutoDelayMode = MultimeterAutoDelayMode.Once
        Me.AutoRangeEnabled = True
        Me.AutoZeroEnabled = True
        Me.FilterCount = 10
        Me.FilterEnabled = False
        Me.MovingAverageFilterEnabled = False
        Me.OpenDetectorEnabled = False
        Me.OpenDetectorKnownStates(MultimeterFunctionMode.ResistanceFourWire) = True
        Me.FilterWindow = 0.001
        For Each fm As MultimeterFunctionMode In [Enum].GetValues(GetType(MultimeterFunctionMode))
            Select Case fm
                Case MultimeterFunctionMode.CurrentAC, MultimeterFunctionMode.CurrentAC
                    Me.FunctionModeRanges(fm).SetRange(0, 3.1)
                Case MultimeterFunctionMode.VoltageAC, MultimeterFunctionMode.VoltageDC
                    Me.FunctionModeRanges(fm).SetRange(0, 303)
                Case MultimeterFunctionMode.ResistanceCommonWire, MultimeterFunctionMode.ResistanceTwoWire, MultimeterFunctionMode.ResistanceFourWire
                    Me.FunctionModeRanges(fm).SetRange(0, 120000000.0)
            End Select
        Next
        Me.FunctionMode = MultimeterFunctionMode.VoltageDC
        Me.Range = 303 'defaults volts range
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
    Protected Overrides ReadOnly Property ApertureQueryCommand As String = "_G.print(_G.dmm.aperture)"

    ''' <summary> Gets the Aperture command format. </summary>
    ''' <value> The Aperture command format. </value>
    Protected Overrides ReadOnly Property ApertureCommandFormat As String = "_G.dmm.aperture={0}"

#End Region

#Region " AUTO DELAY MODE "

    ''' <summary> Queries the Multimeter Auto Delay Mode. </summary>
    ''' <returns> The <see cref="MultimeterAutoDelayMode">Multimeter Auto Delay Mode</see> or none if unknown. </returns>
    Public Overrides Function QueryAutoDelayMode() As MultimeterAutoDelayMode?
        Dim mode As String = Me.AutoDelayMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryTrimEnd(Tsp.TspSyntax.PrintCommandStringIntegerFormat, "_G.dmm.autodelay")
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = "Failed fetching Multimeter Auto Delay Mode"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.AutoDelayMode = New MultimeterAutoDelayMode?
        Else
            Dim value As MultimeterAutoDelayMode
            If [Enum].TryParse(Of MultimeterAutoDelayMode)(mode, value) Then
                Me.AutoDelayMode = value
            Else
                Dim message As String = $"Failed parsing Multimeter Auto Delay Mode value of '{mode}'"
                Debug.Assert(Not Debugger.IsAttached, message)
                Me.AutoDelayMode = New MultimeterAutoDelayMode?
            End If
        End If
        Return Me.AutoDelayMode
    End Function

    ''' <summary> Writes the Multimeter Auto Delay Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Auto Delay Mode. </param>
    ''' <returns> The <see cref="MultimeterAutoDelayMode">Multimeter Auto Delay Mode</see> or none if unknown. </returns>
    Public Overrides Function WriteAutoDelayMode(ByVal value As MultimeterAutoDelayMode) As MultimeterAutoDelayMode?
        Me.Session.WriteLine("_G.dmm.autodelay={0}", CInt(value))
        Me.AutoDelayMode = value
        Return Me.AutoDelayMode
    End Function

#End Region

#Region " AUTO RANGE ENABLED "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = "_G.dmm.autorange={0:'1';'1';'0'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = "_G.print(_G.dmm.autorange==1)"

#End Region

#Region " AUTO ZERO ENABLED "

    ''' <summary> Gets the automatic Zero enabled command Format. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledCommandFormat As String = "_G.dmm.autozero={0:'1';'1';'0'}"

    ''' <summary> Gets the automatic Zero enabled query command. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledQueryCommand As String = "_G.print(_G.dmm.autozero==1)"

#End Region

#Region " CONNECT/DISCONNECT "

    Protected Overrides ReadOnly Property ConnectCommand As String = "if nil ~= dmm then dmm.connect = 7 end"

    Protected Overrides ReadOnly Property DisconnectCommand As String = "if nil ~= dmm then dmm.connect = 0 end"

#End Region

#Region " FILTER "

#Region " FILTER COUNT "

    ''' <summary> Gets the Filter Count query command. </summary>
    ''' <value> The FilterCount query command. </value>
    Protected Overrides ReadOnly Property FilterCountQueryCommand As String = "_G.print(_G.dmm.filter.count)"

    ''' <summary> Gets the Filter Count command format. </summary>
    ''' <value> The FilterCount command format. </value>
    Protected Overrides ReadOnly Property FilterCountCommandFormat As String = "_G.dmm.filter.count={0}"

#End Region

#Region " FILTER ENABLED "

    ''' <summary> Gets the Filter enabled command Format. </summary>
    ''' <value> The Filter enabled query command. </value>
    Protected Overrides ReadOnly Property FilterEnabledCommandFormat As String = "_G.dmm.filter.enable={0:'1';'1';'0'}"

    ''' <summary> Gets the Filter enabled query command. </summary>
    ''' <value> The Filter enabled query command. </value>
    Protected Overrides ReadOnly Property FilterEnabledQueryCommand As String = "_G.print(_G.dmm.filter.enable==1)"

#End Region

#Region " MOVING AVERAGE ENABLED "

    ''' <summary> Gets the moving average filter enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property MovingAverageFilterEnabledCommandFormat As String = "_G.dmm.filter.type={0:'0';'0';'1'}"

    ''' <summary> Gets the moving average filter enabled query command. </summary>
    ''' <value> The moving average filter enabled query command. </value>
    Protected Overrides ReadOnly Property MovingAverageFilterEnabledQueryCommand As String = "_G.print(_G.dmm.filter.type==0)"

#End Region

#Region " FILTER Window "

    ''' <summary> Gets the Filter Window query command. </summary>
    ''' <value> The FilterWindow query command. </value>
    Protected Overrides ReadOnly Property FilterWindowQueryCommand As String = "_G.print(_G.dmm.filter.window)"

    ''' <summary> Gets the Filter Window command format. </summary>
    ''' <value> The FilterWindow command format. </value>
    Protected Overrides ReadOnly Property FilterWindowCommandFormat As String = "_G.dmm.filter.window={0}"

#End Region

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Converts a function Mode to a decimal places. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an Integer. </returns>
    Public Overrides Function ToDecimalPlaces(functionMode As MultimeterFunctionMode) As Integer
        Dim result As Integer = MyBase.ToDecimalPlaces(functionMode)
        Select Case functionMode
            Case MultimeterFunctionMode.CurrentAC, MultimeterFunctionMode.CurrentDC
                result = 3
            Case MultimeterFunctionMode.VoltageAC, MultimeterFunctionMode.VoltageDC
                result = 3
            Case MultimeterFunctionMode.ResistanceCommonWire, MultimeterFunctionMode.ResistanceFourWire, MultimeterFunctionMode.ResistanceTwoWire
                result = 0
        End Select
        Return result
    End Function

    ''' <summary> Queries the Multimeter Function Mode. </summary>
    ''' <returns> The <see cref="MultimeterFunctionMode">Multimeter Function Mode</see> or none if unknown. </returns>
    Public Overrides Function QueryFunctionMode() As MultimeterFunctionMode?
        Dim mode As String = Me.FunctionMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryTrimEnd("_G.print(_G.dmm.func)")
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = "Failed fetching Multimeter Function Mode"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.FunctionMode = New MultimeterFunctionMode?
        Else
            Dim se As New StringEnumerator(Of MultimeterFunctionMode)
            Me.FunctionMode = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.FunctionMode
    End Function

    ''' <summary> Writes the Multimeter Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="MultimeterFunctionMode">Multimeter Function Mode</see> or none if unknown. </returns>
    Public Overrides Function WriteFunctionMode(ByVal value As MultimeterFunctionMode) As MultimeterFunctionMode?
        Me.Session.WriteLine("_G.dmm.func='{0}'", value.ExtractBetween())
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

#End Region

#Region " MEASURE "

    ''' <summary> Gets the Measure query command. </summary>
    ''' <value> The Aperture query command. </value>
    Protected Overrides ReadOnly Property MeasureQueryCommand As String = "_G.print(_G.dmm.measure())"

#End Region

#Region " OPEN DETECTOR ENABLED "

    ''' <summary> Gets the open detector enabled command Format. </summary>
    ''' <value> The open detector enabled query command. </value>
    Protected Overrides ReadOnly Property OpenDetectorEnabledCommandFormat As String = "_G.dmm.opendetector={0:'1';'1';'0'}"

    ''' <summary> Gets the open detector enabled query command. </summary>
    ''' <value> The open detector enabled query command. </value>
    Protected Overrides ReadOnly Property OpenDetectorEnabledQueryCommand As String = "_G.print(_G.dmm.opendetector==1)"

#End Region

#Region " POWER LINE CYCLES "

    ''' <summary> Gets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesCommandFormat As String = "_G.dmm.nplc={0}"

    ''' <summary> Gets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesQueryCommand As String = "_G.print(_G.dmm.nplc)"

#End Region

#Region " RANGE "

    ''' <summary> Gets the Range query command. </summary>
    ''' <value> The Range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = "_G.print(_G.dmm.range)"

    ''' <summary> Gets the Range command format. </summary>
    ''' <value> The Range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = "_G.dmm.range={0}"

#End Region

#End Region

End Class
