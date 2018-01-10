Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary>  Defines the contract that must be implemented by a Source Current Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class SourceSubsystemBase
    Inherits SourceMeasureUnitBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.Tsp2.StatusSubsystemBase">status subsystem</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.Tsp2.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.NewAmount()
        Me._FunctionModeRanges = New SourceFunctionRangeDictionary
        For Each fm As SourceFunctionMode In [Enum].GetValues(GetType(SourceFunctionMode))
            Me._FunctionModeRanges.Add(fm, New Core.Pith.RangeR(0, 1))
        Next
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets values to their known execution reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Level = 0
        Me.Limit = 0.000105
        Me.Range = New Double?
        Me.AutoRangeEnabled = True
        Me.AutoDelayEnabled = True
        For Each fm As SourceFunctionMode In [Enum].GetValues(GetType(SourceFunctionMode))
            Select Case fm
                Case SourceFunctionMode.CurrentDC
                    Me.FunctionModeRanges(fm).SetRange(-1.05, 1.05)
                Case SourceFunctionMode.VoltageDC
                    Me.FunctionModeRanges(fm).SetRange(-210, 210)
            End Select
        Next
        Me.FunctionMode = SourceFunctionMode.VoltageDC
        Me.Range = 0.02
        Me.LimitTripped = False
        Me.OutputEnabled = False
        Me.NewAmount()
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

#Region " AUTO DELAY ENABLED "

    ''' <summary> Auto Delay enabled. </summary>
    Private _AutoDelayEnabled As Boolean?

    ''' <summary> Gets or sets the cached Auto Delay Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Delay Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoDelayEnabled As Boolean?
        Get
            Return Me._AutoDelayEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoDelayEnabled, value) Then
                Me._AutoDelayEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Auto Delay Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoDelayEnabled(value)
        Return Me.QueryAutoDelayEnabled()
    End Function

    ''' <summary> Gets or sets the automatic Delay enabled query print command. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    Protected Overridable ReadOnly Property AutoDelayEnabledPrintCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="AutoDelayEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoDelayEnabled() As Boolean?
        Me.AutoDelayEnabled = Me.Query(Me.AutoDelayEnabled, Me.AutoDelayEnabledPrintCommand)
        Return Me.AutoDelayEnabled
    End Function

    ''' <summary> Gets or sets the automatic Delay enabled command Format. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    ''' <remarks> SCPI: "system:Delay:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoDelayEnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoDelayEnabled = Me.Write(value, Me.AutoDelayEnabledCommandFormat)
        Return Me.AutoDelayEnabled
    End Function

#End Region

#Region " AUTO RANGE ENABLED "

    ''' <summary> Auto Range enabled. </summary>
    Private _AutoRangeEnabled As Boolean?

    ''' <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Range Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoRangeEnabled As Boolean?
        Get
            Return Me._AutoRangeEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoRangeEnabled, value) Then
                Me._AutoRangeEnabled = value
                If value.HasValue Then
                    Me.AutoRangeState = If(value.Value, OnOffState.On, OnOffState.Off)
                Else
                    Me.AutoRangeState = New OnOffState?
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Auto Range Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoRangeEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoRangeEnabled(value)
        Return Me.QueryAutoRangeEnabled()
    End Function

    ''' <summary> Gets or sets the automatic Range enabled print query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    ''' <remarks> SCPI: "system:RANG:AUTO?" </remarks>
    Protected Overridable ReadOnly Property AutoRangeEnabledPrintCommand As String

    ''' <summary> Queries the Auto Range Enabled sentinel. Also sets the
    ''' <see cref="AutoRangeEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoRangeEnabled() As Boolean?
        Me.AutoRangeEnabled = Me.Query(Me.AutoRangeEnabled, Me.AutoRangeEnabledPrintCommand)
        Return Me.AutoRangeEnabled
    End Function

    ''' <summary> Gets or sets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    ''' <remarks> SCPI: "system:RANGE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoRangeEnabledCommandFormat As String

    ''' <summary> Writes the Auto Range Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoRangeEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoRangeEnabled = Me.Write(value, Me.AutoRangeEnabledCommandFormat)
        Return Me.AutoRangeEnabled
    End Function

#End Region

#Region " AUTO RANGE STATE "

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
    Private _FunctionMode As SourceFunctionMode?

    ''' <summary> Parse units. </summary>
    ''' <param name="value"> The  Source Function Mode. </param>
    ''' <returns> An Arebis.TypedUnits.Unit. </returns>
    Public Shared Function ParseUnits(ByVal value As SourceFunctionMode) As Arebis.TypedUnits.Unit
        Select Case value
            Case SourceFunctionMode.CurrentDC
                Return Arebis.StandardUnits.ElectricUnits.Ampere
            Case SourceFunctionMode.VoltageDC
                Return Arebis.StandardUnits.ElectricUnits.Volt
            Case Else
                Return Arebis.StandardUnits.ElectricUnits.Volt
        End Select
    End Function

    ''' <summary> Writes and reads back the Source Function Mode. </summary>
    ''' <param name="value"> The  Source Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As SourceFunctionMode) As SourceFunctionMode?
        Me.WriteFunctionMode(value)
        ' changing the function mode changes range, auto delay mode and open detector enabled. 
        Me.QueryRange()
        Me.QueryLevel()
        Return FunctionMode
    End Function

    ''' <summary> Gets or sets the cached function mode. </summary>
    ''' <value> The <see cref="SourceFunctionMode">Source Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FunctionMode As SourceFunctionMode?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As SourceFunctionMode?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                Me.NewAmount()
                If value.HasValue Then
                    Me._RangeRange = Me.FunctionModeRanges(value.Value)
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command. </value>
    Protected Overridable ReadOnly Property FunctionModeQueryCommand As String = "_G.smu.Source.func"

    ''' <summary> Queries the Source Function Mode. </summary>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function Mode</see> or none if unknown. </returns>
    Public Overridable Function QueryFunctionMode() As SourceFunctionMode?
        Dim mode As String = Me.FunctionMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.FunctionModeQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(SourceSubsystemBase)}.{NameOf(FunctionMode)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.FunctionMode = New SourceFunctionMode?
        Else
            Dim se As New StringEnumerator(Of SourceFunctionMode)
            Me.FunctionMode = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.FunctionMode
    End Function

    ''' <summary> Gets or sets the function mode command format. </summary>
    ''' <value> The function mode command format. </value>
    Protected Overridable ReadOnly Property FunctionModeCommandFormat As String = "_G.smu.Source.func={0}"

    ''' <summary> Writes the Source Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function Mode</see> or none if unknown. </returns>
    Public Overridable Function WriteFunctionMode(ByVal value As SourceFunctionMode) As SourceFunctionMode?
        Me.Session.WriteLine(Me.FunctionModeCommandFormat, value.ExtractBetween())
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

#End Region

#Region " LEVEL "

    ''' <summary> Gets or sets the amount. </summary>
    ''' <value> The amount. </value>
    Public ReadOnly Property Amount As Arebis.TypedUnits.Amount

    ''' <summary> Creates a new amount. </summary>
    Private Sub NewAmount()
        If Me.FunctionMode.HasValue AndAlso Me.Level.HasValue Then
            Me._Amount = New Arebis.TypedUnits.Amount(Me.Level.Value, SourceSubsystemBase.ParseUnits(Me.FunctionMode.Value))
        Else
            Me._Amount = New Arebis.TypedUnits.Amount(0, Arebis.StandardUnits.ElectricUnits.Volt)
        End If
    End Sub

    ''' <summary> The level. </summary>
    Private _Level As Double?

    ''' <summary> Gets or sets the cached Source Current Level. </summary>
    ''' <value> The Source Current Level. Actual current depends on the power supply mode. </value>
    Public Overloads Property Level As Double?
        Get
            Return Me._Level
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Level, value) Then
                Me._Level = value
                Me.NewAmount()
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source current level. </summary>
    ''' <remarks> This command set the immediate output current level. The value is in Amperes. The
    ''' immediate level is the output current setting. At *RST, the current values = 0. </remarks>
    ''' <param name="value"> The current level. </param>
    ''' <returns> The Source Current Level. </returns>
    Public Function ApplyLevel(ByVal value As Double) As Double?
        Me.WriteLevel(value)
        Return Me.QueryLevel
    End Function

    ''' <summary> Gets or sets The Level query print command. </summary>
    ''' <value> The Level query command. </value>
    Protected Overridable ReadOnly Property LevelQueryCommand As String

    ''' <summary> Queries the current level. </summary>
    ''' <returns> The current level or none if unknown. </returns>
    Public Function QueryLevel() As Double?
        Const printFormat As Decimal = 9.6D
        Me.Level = Me.Session.QueryPrint(Me.Level.GetValueOrDefault(0), printFormat, Me.LevelQueryCommand)
        Return Me.Level
    End Function

    ''' <summary> Gets or sets The Level command format. </summary>
    ''' <value> The Level command format. </value>
    Protected Overridable ReadOnly Property LevelCommandFormat As String

    ''' <summary> Writes the source current level without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output current level. The value is in Amperes. The
    ''' immediate level is the output current setting. At *RST, the current values = 0. </remarks>
    ''' <param name="value"> The current level. </param>
    ''' <returns> The Source Current Level. </returns>
    Public Function WriteLevel(ByVal value As Double) As Double?
        Me.Session.WriteLine(LevelCommandFormat, value)
        Me.Level = value
        Return Me.Level
    End Function

#End Region

#Region " LIMIT "

    ''' <summary> The Limit. </summary>
    Private _Limit As Double?

    ''' <summary> Gets or sets the cached source Limit for a Current Source. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Limit As Double?
        Get
            Return Me._Limit
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit, value) Then
                Me._Limit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Limit. </summary>
    ''' <remarks> This command set the immediate output Limit. The value is in Amperes. The
    ''' immediate Limit is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Limit. </param>
    ''' <returns> The Source Limit. </returns>
    Public Function ApplyLimit(ByVal value As Double) As Double?
        Me.WriteLimit(value)
        Return Me.QueryLimit()
    End Function

    ''' <summary> Gets or sets the limit query command. </summary>
    ''' <value> The limit query command. </value>
    Protected Overridable ReadOnly Property LimitQueryCommandFormat As String

    Private Const currentLimitFunction As String = "i"
    Private Const voltageLimitFunction As String = "v"

    ''' <summary> Limit function mode. </summary>
    ''' <returns> A String. </returns>
    Private Function LimitFunctionMode() As String
        Return If(Me.FunctionMode.Value = SourceFunctionMode.CurrentDC, SourceSubsystemBase.voltageLimitFunction, SourceSubsystemBase.currentLimitFunction)
    End Function

    ''' <summary> Queries the Limit. </summary>
    ''' <returns> The Limit or none if unknown. </returns>
    Public Function QueryLimit() As Double?
        Const printFormat As Decimal = 9.6D
        Me.Limit = Me.Session.QueryPrint(Me.Limit.GetValueOrDefault(0.099), printFormat, Me.LimitQueryCommandFormat, Me.LimitFunctionMode)
        Return Me.Limit
    End Function

    Protected Overridable ReadOnly Property LimitCommandFormat As String

    ''' <summary> Writes the source Limit without reading back the value from the device. </summary>
    ''' <remarks> This command set the immediate output Limit. The value is in Amperes. The
    ''' immediate Limit is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Limit. </param>
    ''' <returns> The Source Limit. </returns>
    Public Function WriteLimit(ByVal value As Double) As Double?
        Me.Session.WriteLine(Me.LimitCommandFormat, Me.LimitFunctionMode, Me.SourceMeasureUnitReference, value)
        Me.Limit = value
        Return Me.Limit
    End Function

#End Region

#Region " LIMIT TRIPPED "

    ''' <summary> Limit Tripped. </summary>
    Private _LimitTripped As Boolean?

    ''' <summary> Gets or sets the cached Limit Tripped sentinel. </summary>
    ''' <value> <c>null</c> if Limit Tripped is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property LimitTripped As Boolean?
        Get
            Return Me._LimitTripped
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.LimitTripped, value) Then
                Me._LimitTripped = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets the limit tripped print command format. </summary>
    ''' <value> The limit tripped print command format. </value>
    Protected Overridable ReadOnly Property LimitTrippedPrintCommandFormat As String

    ''' <summary> Gets the limit tripped print command. </summary>
    ''' <value> The limit tripped print command. </value>
    Protected Overridable ReadOnly Property LimitTrippedPrintCommand As String
        Get
            Return String.Format(Me.LimitTrippedPrintCommandFormat, Me.LimitFunctionMode)
        End Get
    End Property

    ''' <summary> Queries the Limit Tripped sentinel. Also sets the
    ''' <see cref="LimitTripped">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryLimitTripped() As Boolean?
        Me.LimitTripped = Me.Query(Me.LimitTripped, Me.LimitTrippedPrintCommand)
        Return Me.LimitTripped
    End Function

#End Region

#Region " OUTPUT ENABLED "

    ''' <summary> Output enabled. </summary>
    Private _OutputEnabled As Boolean?

    ''' <summary> Gets or sets the cached Output Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Output Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property OutputEnabled As Boolean?
        Get
            Return Me._OutputEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.OutputEnabled, value) Then
                Me._OutputEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Output Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyOutputEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteOutputEnabled(value)
        Return Me.QueryOutputEnabled()
    End Function

    ''' <summary> Gets or sets the Output enabled query print command. </summary>
    ''' <value> The Output enabled query command. </value>
    Protected Overridable ReadOnly Property OutputEnabledPrintCommand As String

    ''' <summary> Queries the Output Enabled sentinel. Also sets the
    ''' <see cref="OutputEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryOutputEnabled() As Boolean?
        Me.OutputEnabled = Me.Query(Me.OutputEnabled, Me.OutputEnabledPrintCommand)
        Return Me.OutputEnabled
    End Function

    ''' <summary> Gets or sets the Output enabled command Format. </summary>
    ''' <value> The Output enabled query command. </value>
    Protected Overridable ReadOnly Property OutputEnabledCommandFormat As String

    ''' <summary> Writes the Output Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteOutputEnabled(ByVal value As Boolean) As Boolean?
        Me.OutputEnabled = Me.Write(value, Me.OutputEnabledCommandFormat)
        Return Me.OutputEnabled
    End Function

#End Region

#Region " RANGE "

    ''' <summary> Gets or sets the function mode ranges. </summary>
    ''' <value> The function mode ranges. </value>
    Public ReadOnly Property FunctionModeRanges As SourceFunctionRangeDictionary

    ''' <summary> The Range of the range. </summary>
    Public ReadOnly Property RangeRange As Core.Pith.RangeR

    ''' <summary> The Range. </summary>
    Private _Range As Double?

    ''' <summary> Gets or sets the cached sense Range. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Range As Double?
        Get
            Return Me._Range
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Range, value) Then
                Me._Range = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the sense Range. </summary>
    ''' <param name="value"> The Range. </param>
    ''' <returns> The Range. </returns>
    Public Function ApplyRange(ByVal value As Double) As Double?
        Me.WriteRange(value)
        Return Me.QueryRange
    End Function

    ''' <summary> Gets or sets The Range query print command. </summary>
    ''' <value> The Range query command. </value>
    Protected Overridable ReadOnly Property RangePrintCommand As String

    ''' <summary> Queries The Range. </summary>
    ''' <returns> The Range or none if unknown. </returns>
    Public Function QueryRange() As Double?
        Me.Range = Me.Query(Me.Range, Me.RangePrintCommand)
        Return Me.Range
    End Function

    ''' <summary> Gets or sets The Range command format. </summary>
    ''' <value> The Range command format. </value>
    Protected Overridable ReadOnly Property RangeCommandFormat As String

    ''' <summary> Writes The Range without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Range. </remarks>
    ''' <param name="value"> The Range. </param>
    ''' <returns> The Range. </returns>
    Public Function WriteRange(ByVal value As Double) As Double?
        value = If(value > RangeRange.Max, RangeRange.Max, If(value < RangeRange.Min, RangeRange.Min, value))
        Me.Range = Me.Write(value, Me.RangeCommandFormat)
        Return Me.Range
    End Function

#End Region

End Class

''' <summary> Specifies the function modes. </summary>
Public Enum SourceFunctionMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("DC Voltage (smu.FUNC_DC_VOLTAGE)")> VoltageDC
    <ComponentModel.Description("DC Current (smu.FUNC_DC_CURRENT)")> CurrentDC
End Enum

''' <summary> Dictionary of Source function ranges. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/16/2016" by="David" revision=""> Created. </history>
<CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")>
Public Class SourceFunctionRangeDictionary
    Inherits Collections.Generic.Dictionary(Of SourceFunctionMode, Core.Pith.RangeR)
End Class

''' <summary> Dictionary of function-related enabled functionality. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="2/8/2016" by="David" revision=""> Created. </history>
<CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")>
Public Class SourceFunctionEnabledDictionary
    Inherits Collections.Generic.Dictionary(Of SourceFunctionMode, Boolean)
End Class
