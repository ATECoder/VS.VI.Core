''' <summary> Defines the contract that must be implemented by a Sense Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific ReSenses, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SenseSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.FunctionRange = Me.DefaultFunctionRange ' isr.Core.Pith.RangeR.Full
        Me.FunctionUnit = Me.DefaultFunctionUnit ' Arebis.StandardUnits.ElectricUnits.Volt
        Me.FunctionRangeDecimalPlaces = Me.DefaultFunctionModeDecimalPlaces ' 3
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
    End Sub

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
    Protected MustOverride ReadOnly Property AutoRangeEnabledQueryCommand As String

    ''' <summary> Queries the Auto Range Enabled sentinel. Also sets the
    ''' <see cref="AutoRangeEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoRangeEnabled() As Boolean?
        Me.AutoRangeEnabled = MyBase.Query(Me.AutoRangeEnabled, Me.AutoRangeEnabledQueryCommand)
        Return Me.AutoRangeEnabled
    End Function

    ''' <summary> Gets or sets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    ''' <remarks> SCPI: "system:RANGE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected MustOverride ReadOnly Property AutoRangeEnabledCommandFormat As String

    ''' <summary> Writes the Auto Range Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoRangeEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoRangeEnabled = MyBase.Write(value, Me.AutoRangeEnabledCommandFormat)
        Return Me.AutoRangeEnabled
    End Function

#End Region

#Region " CONCURRENT SENSE ENABLED "

    ''' <summary> Auto Range enabled. </summary>
    Private _ConcurrentSenseEnabled As Boolean?

    ''' <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Range Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property ConcurrentSenseEnabled As Boolean?
        Get
            Return Me._ConcurrentSenseEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.ConcurrentSenseEnabled, value) Then
                Me._ConcurrentSenseEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Auto Range Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyConcurrentSenseEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteConcurrentSenseEnabled(value)
        Return Me.QueryConcurrentSenseEnabled()
    End Function

    ''' <summary> Gets or sets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    ''' <remarks> SCPI: "system:RANG:AUTO?" </remarks>
    Protected Overridable ReadOnly Property ConcurrentSenseEnabledQueryCommand As String

    ''' <summary> Queries the Auto Range Enabled sentinel. Also sets the
    ''' <see cref="ConcurrentSenseEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryConcurrentSenseEnabled() As Boolean?
        Me.ConcurrentSenseEnabled = MyBase.Query(Me.ConcurrentSenseEnabled, Me.ConcurrentSenseEnabledQueryCommand)
        Return Me.ConcurrentSenseEnabled
    End Function

    ''' <summary> Gets or sets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    ''' <remarks> SCPI: "system:RANGE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property ConcurrentSenseEnabledCommandFormat As String

    ''' <summary> Writes the Auto Range Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteConcurrentSenseEnabled(ByVal value As Boolean) As Boolean?
        Me.ConcurrentSenseEnabled = MyBase.Write(value, Me.ConcurrentSenseEnabledCommandFormat)
        Return Me.ConcurrentSenseEnabled
    End Function

#End Region

#Region " POWER LINE CYCLES (NPLC) "

    ''' <summary> The Power Line Cycles. </summary>
    Private _PowerLineCycles As Double?

    ''' <summary> Gets the integration period. </summary>
    ''' <value> The integration period. </value>
    Public ReadOnly Property IntegrationPeriod As TimeSpan?
        Get
            If Me.PowerLineCycles.HasValue Then
                Return StatusSubsystemBase.FromPowerLineCycles(Me.PowerLineCycles.Value)
            Else
                Return New TimeSpan?
            End If
        End Get
    End Property

    ''' <summary> Gets or sets the cached sense PowerLineCycles. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property PowerLineCycles As Double?
        Get
            Return Me._PowerLineCycles
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.PowerLineCycles, value) Then
                Me._PowerLineCycles = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the sense PowerLineCycles. </summary>
    ''' <param name="value"> The Power Line Cycles. </param>
    ''' <returns> The Power Line Cycles. </returns>
    Public Function ApplyPowerLineCycles(ByVal value As Double) As Double?
        Me.WritePowerLineCycles(value)
        Return Me.QueryPowerLineCycles
    End Function

    ''' <summary> Gets or sets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected MustOverride ReadOnly Property PowerLineCyclesQueryCommand As String

    ''' <summary> Queries The Power Line Cycles. </summary>
    ''' <returns> The Power Line Cycles or none if unknown. </returns>
    Public Function QueryPowerLineCycles() As Double?
        Me.PowerLineCycles = MyBase.Query(Me.PowerLineCycles, Me.PowerLineCyclesQueryCommand)
        Return Me.PowerLineCycles
    End Function

    ''' <summary> Gets or sets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected MustOverride ReadOnly Property PowerLineCyclesCommandFormat As String

    ''' <summary> Writes The Power Line Cycles without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Power Line Cycles. </remarks>
    ''' <param name="value"> The Power Line Cycles. </param>
    ''' <returns> The Power Line Cycles. </returns>
    Public Function WritePowerLineCycles(ByVal value As Double) As Double?
        Me.PowerLineCycles = MyBase.Write(value, Me.PowerLineCyclesCommandFormat)
        Return Me.PowerLineCycles
    End Function

#End Region

#Region " PROTECTION LEVEL "

    ''' <summary> The Current Limit. </summary>
    Private _ProtectionLevel As Double?

    ''' <summary> Gets or sets the cached source current Limit for a voltage source. Set to
    ''' <see cref="isr.VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="isr.VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property ProtectionLevel As Double?
        Get
            Return Me._ProtectionLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.ProtectionLevel, value) Then
                Me._ProtectionLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the protection level. </summary>
    ''' <param name="value"> the protection level. </param>
    ''' <returns> the protection level. </returns>
    Public Function ApplyProtectionLevel(ByVal value As Double) As Double?
        Me.WriteProtectionLevel(value)
        Return Me.QueryProtectionLevel
    End Function

    ''' <summary> Gets or sets the protection level query command. </summary>
    ''' <value> the protection level query command. </value>
    Protected MustOverride ReadOnly Property ProtectionLevelQueryCommand As String

    ''' <summary> Queries the protection level. </summary>
    ''' <returns> the protection level or none if unknown. </returns>
    Public Function QueryProtectionLevel() As Double?
        Me.ProtectionLevel = MyBase.Query(Me.ProtectionLevel, Me.ProtectionLevelQueryCommand)
        Return Me.ProtectionLevel
    End Function

    ''' <summary> Gets or sets the protection level command format. </summary>
    ''' <value> the protection level command format. </value>
    Protected MustOverride ReadOnly Property ProtectionLevelCommandFormat As String

    ''' <summary> Writes the protection level without reading back the value from the device. </summary>
    ''' <remarks> This command sets the protection level. </remarks>
    ''' <param name="value"> the protection level. </param>
    ''' <returns> the protection level. </returns>
    Public Function WriteProtectionLevel(ByVal value As Double) As Double?
        Me.ProtectionLevel = MyBase.Write(value, Me.ProtectionLevelCommandFormat)
        Return Me.ProtectionLevel
    End Function

#End Region

#Region " RANGE "

    ''' <summary> The Range. </summary>
    Private _Range As Double?

    ''' <summary> Gets or sets the cached sense Range. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <remarks>
    ''' You can assign any real number using this command. The instrument selects the closest fixed
    ''' range that Is large enough to measure the entered number. For example, for current
    ''' measurements, if you expect a reading Of approximately 9 mA, Set the range To 9 mA To Select
    ''' the 10 mA range. When you read this setting, you see the positive full-scale value Of the
    ''' measurement range that the instrument Is presently using. This command Is primarily intended
    ''' To eliminate the time that Is required by the instrument To automatically search For a range.
    ''' When a range Is fixed, any signal greater than the entered range generates an overrange
    ''' condition. When an over-range condition occurs, the front panel displays "Overflow" And the
    ''' remote interface returns 9.9e+37.
    ''' </remarks>
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

#Region " FETCH; DATA; READ "

    ''' <summary> Gets or sets the latest data query command. </summary>
    ''' <value> The latest data query command. </value>
    ''' <remarks> SCPI: ":SENSE:DATA:LAT?" </remarks>
    Protected Overridable ReadOnly Property LatestDataQueryCommand As String

    ''' <summary> Fetches the latest data and parses it. </summary>
    ''' <remarks> Issues the ':SENSE:DATA:LAT?' query, which reads data stored in the Sample Buffer.. </remarks>
    Public Overridable Function FetchLatestData() As Double?
        Return Me.Measure(Me.LatestDataQueryCommand)
    End Function

#End Region

End Class

#Region " UNUSED "
#If False Then

            Me.ParseReading(Me.LastReading)
            Me.MeasurementAvailable = True

    ''' <summary> <c>True</c> if Measurement available. </summary>
    Private _MeasurementAvailable As Boolean

    ''' <summary> Gets or sets a value indicating whether [Measurement available]. </summary>
    ''' <value> <c>True</c> if [Measurement available]; otherwise, <c>False</c>. </value>
    Public Property MeasurementAvailable As Boolean
        Get
            Return Me._MeasurementAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            Me._MeasurementAvailable = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property
#End If
#End Region