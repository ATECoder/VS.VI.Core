Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ComboBoxExtensions
''' <summary> Defines the contract that must be implemented by a Source Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SourceSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._FunctionModeRanges = New RangeDictionary
        Me._FunctionModeDecimalPlaces = New IntegerDictionary
        Me._FunctionModeUnits = New UnitDictionary
        Me.FunctionRange = Me.DefaultFunctionRange
        Me.FunctionRangeDecimalPlaces = Me.DefaultFunctionModeDecimalPlaces
        Me.Amount = New Arebis.TypedUnits.Amount(0, Arebis.StandardUnits.ElectricUnits.Volt)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.AutoClearEnabled = False
        Me.AutoDelayEnabled = True
        Me.Delay = TimeSpan.Zero
        Me.SweepPoints = 2500
    End Sub

#End Region

#Region " AUTO CLEAR ENABLED "

    Private _AutoClearEnabled As Boolean?
    ''' <summary> Gets or sets the cached Auto Clear Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Clear Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoClearEnabled As Boolean?
        Get
            Return Me._AutoClearEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoClearEnabled, value) Then
                Me._AutoClearEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Auto Clear Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoClearEnabled(value)
        Return Me.QueryAutoClearEnabled()
    End Function

    ''' <summary> Gets the automatic Clear enabled query command. </summary>
    ''' <value> The automatic Clear enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR:CLE:AUTO?" </remarks>
    Protected Overridable ReadOnly Property AutoClearEnabledQueryCommand As String

    ''' <summary> Queries the Auto Clear Enabled sentinel. Also sets the
    ''' <see cref="AutoClearEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoClearEnabled() As Boolean?
        Me.AutoClearEnabled = Me.Query(Me.AutoClearEnabled, Me.AutoClearEnabledQueryCommand)
        Return Me.AutoClearEnabled
    End Function

    ''' <summary> Gets the automatic Clear enabled command Format. </summary>
    ''' <value> The automatic Clear enabled query command. </value>
    ''' <remarks> SCPI: ":SOU:CLE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoClearEnabledCommandFormat As String

    ''' <summary> Writes the Auto Clear Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoClearEnabled = Me.Write(value, Me.AutoClearEnabledCommandFormat)
        Return Me.AutoClearEnabled
    End Function

#End Region

#Region " AUTO DELAY ENABLED "

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

    ''' <summary> Gets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR:DEL:AUTO?" </remarks>
    Protected Overridable ReadOnly Property AutoDelayEnabledQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="AutoDelayEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoDelayEnabled() As Boolean?
        Me.AutoDelayEnabled = Me.Query(Me.AutoDelayEnabled, Me.AutoDelayEnabledQueryCommand)
        Return Me.AutoDelayEnabled
    End Function

    ''' <summary> Gets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR:DEL:AUTO {0:'ON';'ON';'OFF'}" </remarks>
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
    Public Overridable Property AutoRangeEnabled As Boolean?
        Get
            Return Me._AutoRangeEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoRangeEnabled, value) Then
                Me._AutoRangeEnabled = value
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

    ''' <summary> Gets or sets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    ''' <remarks> SCPI: "system:RANG:AUTO?" </remarks>
    Protected Overridable ReadOnly Property AutoRangeEnabledQueryCommand As String

    ''' <summary> Queries the Auto Range Enabled sentinel. Also sets the
    ''' <see cref="AutoRangeEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoRangeEnabled() As Boolean?
        Me.AutoRangeEnabled = Me.Query(Me.AutoRangeEnabled, Me.AutoRangeEnabledQueryCommand)
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

#Region " DELAY "

    ''' <summary> The delay. </summary>
    Private _Delay As TimeSpan?

    ''' <summary> Gets or sets the cached Source Delay. </summary>
    ''' <remarks> The delay is used to delay operation in the Source layer. After the programmed
    ''' Source event occurs, the instrument waits until the delay period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Source Delay or none if not set or unknown. </value>
    Public Overloads Property Delay As TimeSpan?
        Get
            Return Me._Delay
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Nullable.Equals(Me.Delay, value) Then
                Me._Delay = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Source Delay. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Source Delay or none if unknown. </returns>
    Public Function ApplyDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteDelay(value)
        Me.QueryDelay()
    End Function

    ''' <summary> Gets or sets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    ''' <remarks> SCPI: ":SOUR:DEL?" </remarks>
    Protected Overridable ReadOnly Property DelayQueryCommand As String

    ''' <summary> Gets or sets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    ''' <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property DelayFormat As String

    ''' <summary> Queries the Delay. </summary>
    ''' <returns> The Delay or none if unknown. </returns>
    Public Function QueryDelay() As TimeSpan?
        Me.Delay = Me.Query(Me.Delay, Me.DelayFormat, Me.DelayQueryCommand)
        Return Me.Delay
    End Function

    ''' <summary> Gets or sets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    ''' <remarks> SCPI: ":SOUR:DEL {0:s\.FFFFFFF}" </remarks>
    Protected Overridable ReadOnly Property DelayCommandFormat As String

    ''' <summary> Writes the Source Delay without reading back the value from the device. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Source Delay or none if unknown. </returns>
    Public Function WriteDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.Delay = Me.Write(value, Me.DelayCommandFormat)
        Return Me.Delay
    End Function

#End Region

#Region " LEVEL "

    Private _Amount As Arebis.TypedUnits.Amount
    ''' <summary> Gets or sets the amount. </summary>
    ''' <value> The amount. </value>
    Public Property Amount As Arebis.TypedUnits.Amount
        Get
            Return Me._Amount
        End Get
        Set(value As Arebis.TypedUnits.Amount)
            Me._Amount = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Creates a new amount. </summary>
    ''' <param name="unit"> The unit. </param>
    Private Sub NewAmount(ByVal unit As Arebis.TypedUnits.Unit)
        If Me.Level.HasValue Then
            Me._Amount = New Arebis.TypedUnits.Amount(Me.Level.Value, unit)
        Else
            Me._Amount = New Arebis.TypedUnits.Amount(0, unit)
        End If
        Me.SafePostPropertyChanged(NameOf(SourceSubsystemBase.Amount))
        Me.SafePostPropertyChanged(NameOf(SourceSubsystemBase.FunctionUnit))
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
                Me.NewAmount(Me.FunctionUnit)
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

    ''' <summary> Gets or sets The Level query command. </summary>
    ''' <value> The Level query command. </value>
    Protected Overridable ReadOnly Property LevelQueryCommand As String

    ''' <summary> Queries the current level. </summary>
    ''' <returns> The current level or none if unknown. </returns>
    Public Function QueryLevel() As Double?
        Me.Level = Me.Query(Me.Level.GetValueOrDefault(0), Me.LevelQueryCommand)
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
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
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
    Protected Overridable ReadOnly Property ModalityLimitQueryCommandFormat As String

    ''' <summary> Queries the Limit. </summary>
    ''' <returns> The Limit or none if unknown. </returns>
    Public Overridable Function QueryLimit() As Double?
        Me.Limit = Me.Query(Me.Limit, Me.ModalityLimitQueryCommandFormat)
        Return Me.Limit
    End Function

    Protected Overridable ReadOnly Property ModalityLimitCommandFormat As String

    ''' <summary> Writes the source Limit without reading back the value from the device. </summary>
    ''' <remarks> This command set the immediate output Limit. The value is in Amperes. The
    ''' immediate Limit is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Limit. </param>
    ''' <returns> The Source Limit. </returns>
    Public Overridable Function WriteLimit(ByVal value As Double) As Double?
        Me.Write(Me.ModalityLimitCommandFormat, value)
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

    ''' <summary> Gets the limit tripped command format. </summary>
    ''' <value> The limit tripped command format. </value>
    Protected Overridable ReadOnly Property LimitTrippedQueryCommand As String

    ''' <summary> Queries the Limit Tripped sentinel. Also sets the
    ''' <see cref="LimitTripped">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryLimitTripped() As Boolean?
        Me.LimitTripped = Me.Query(Me.LimitTripped, Me.LimitTrippedQueryCommand)
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

    ''' <summary> Gets or sets the Output enabled query command. </summary>
    ''' <value> The Output enabled query command. </value>
    Protected Overridable ReadOnly Property OutputEnabledQueryCommand As String

    ''' <summary> Queries the Output Enabled sentinel. Also sets the
    ''' <see cref="OutputEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryOutputEnabled() As Boolean?
        Me.OutputEnabled = Me.Query(Me.OutputEnabled, Me.OutputEnabledQueryCommand)
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

    ''' <summary> The Range. </summary>
    Private _Range As Double?

    ''' <summary> Gets or sets the cached sense Range. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
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

    ''' <summary> Gets or sets The Range query command. </summary>
    ''' <value> The Range query command. </value>
    Protected Overridable ReadOnly Property RangeQueryCommand As String

    ''' <summary> Queries The Range. </summary>
    ''' <returns> The Range or none if unknown. </returns>
    Public Function QueryRange() As Double?
        Me.Range = Me.Query(Me.Range, Me.RangeQueryCommand)
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
        Me.Range = Me.Write(value, Me.RangeCommandFormat)
        Return Me.Range
    End Function

#End Region

#Region " SWEEP POINTS "

    Private _SweepPoint As Integer?
    ''' <summary> Gets or sets the cached Sweep Points. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Sweep Points or none if not set or unknown. </value>
    Public Overloads Property SweepPoints As Integer?
        Get
            Return Me._SweepPoint
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.SweepPoints, value) Then
                Me._SweepPoint = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sweep Points. </summary>
    ''' <param name="value"> The current SweepPoints. </param>
    ''' <returns> The SweepPoints or none if unknown. </returns>
    Public Function ApplySweepPoints(ByVal value As Integer) As Integer?
        Me.WriteSweepPoints(value)
        Return Me.QuerySweepPoints()
    End Function

    ''' <summary> Gets Sweep Points query command. </summary>
    ''' <value> The Sweep Points query command. </value>
    ''' <remarks> SCPI: ":SOUR:SWE:POIN?" </remarks>
    Protected Overridable ReadOnly Property SweepPointsQueryCommand As String

    ''' <summary> Queries the current Sweep Points. </summary>
    ''' <returns> The Sweep Points or none if unknown. </returns>
    Public Function QuerySweepPoints() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.SweepPointsQueryCommand) Then
            Me.SweepPoints = Me.Session.Query(0I, Me.SweepPointsQueryCommand)
        End If
        Return Me.SweepPoints
    End Function

    ''' <summary> Gets Sweep Points command format. </summary>
    ''' <value> The Sweep Points command format. </value>
    ''' <remarks> SCPI: ":SOUR:SWE:POIN {0}" </remarks>
    Protected Overridable ReadOnly Property SweepPointsCommandFormat As String

    ''' <summary> Write the Trace PointsSweepPoints without reading back the value from the device. </summary>
    ''' <param name="value"> The current PointsSweepPoints. </param>
    ''' <returns> The PointsSweepPoints or none if unknown. </returns>
    Public Function WriteSweepPoints(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.SweepPointsCommandFormat) Then
            Me.Session.WriteLine(Me.SweepPointsCommandFormat, value)
        End If
        Me.SweepPoints = value
        Return Me.SweepPoints
    End Function

#End Region

End Class
