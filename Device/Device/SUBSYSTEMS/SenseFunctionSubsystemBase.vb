''' <summary> Defines the contract that must be implemented by a Sense function Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific ReSenses, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SenseFunctionSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SenseFunctionSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.DefaultFunctionRange = DeviceBase.DefaultFunctionRange
        Me.DefaultFunctionModeDecimalPlaces = 3
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.AutoZeroEnabled = True
        Me.AutoRangeEnabled = True
        Me.PowerLineCycles = 1
        Me.PowerLineCyclesRange = DeviceBase.DefaultPowerLineCyclesRange
        Me.FunctionRange = Me.DefaultFunctionRange
        Me.FunctionUnit = Me.DefaultFunctionUnit
        Me.FunctionRangeDecimalPlaces = Me.DefaultFunctionModeDecimalPlaces
    End Sub

#End Region

#Region " AVERAGE ENABLED "

    ''' <summary> Average enabled. </summary>
    Private _AverageEnabled As Boolean?

    ''' <summary> Gets or sets the cached Average Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Average Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AverageEnabled As Boolean?
        Get
            Return Me._AverageEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AverageEnabled, value) Then
                Me._AverageEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Average Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAverageEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAverageEnabled(value)
        Return Me.QueryAverageEnabled()
    End Function

    ''' <summary> Gets or sets the Average enabled query command. </summary>
    ''' <value> The Average enabled query command. </value>
    ''' <remarks> SCPI: "CURR:AVER:STAT?" </remarks>
    Protected Overridable ReadOnly Property AverageEnabledQueryCommand As String

    ''' <summary> Queries the Average Enabled sentinel. Also sets the
    ''' <see cref="AverageEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAverageEnabled() As Boolean?
        Me.AverageEnabled = Me.Query(Me.AverageEnabled, Me.AverageEnabledQueryCommand)
        Return Me.AverageEnabled
    End Function

    ''' <summary> Gets or sets the Average enabled command Format. </summary>
    ''' <value> The Average enabled query command. </value>
    ''' <remarks> SCPI: "CURR:AVER:STAT {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AverageEnabledCommandFormat As String

    ''' <summary> Writes the Average Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAverageEnabled(ByVal value As Boolean) As Boolean?
        Me.AverageEnabled = Me.Write(value, Me.AverageEnabledCommandFormat)
        Return Me.AverageEnabled
    End Function

#End Region

#Region " AVERAGE COUNT "

    ''' <summary> The average count. </summary>
    Private _AverageCount As Integer?

    ''' <summary> Gets or sets the cached average count. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property AverageCount As Integer?
        Get
            Return Me._AverageCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.AverageCount, value) Then
                Me._AverageCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the average count. </summary>
    ''' <param name="value"> The average count. </param>
    ''' <returns> The average count. </returns>
    Public Function ApplyAverageCount(ByVal value As Integer) As Integer?
        Me.WriteAverageCount(value)
        Return Me.QueryAverageCount
    End Function

    ''' <summary> Gets or sets The average count query command. </summary>
    ''' <value> The average count query command. </value>
    Protected Overridable ReadOnly Property AverageCountQueryCommand As String

    ''' <summary> Queries The average count. </summary>
    ''' <returns> The average count or none if unknown. </returns>
    Public Function QueryAverageCount() As Integer?
        Me.AverageCount = Me.Query(Me.AverageCount, Me.AverageCountQueryCommand)
        Return Me.AverageCount
    End Function

    ''' <summary> Gets or sets The average count command format. </summary>
    ''' <value> The average count command format. </value>
    Protected Overridable ReadOnly Property AverageCountCommandFormat As String

    ''' <summary> Writes The average count without reading back the value from the device. </summary>
    ''' <remarks> This command sets The average count. </remarks>
    ''' <param name="value"> The average count. </param>
    ''' <returns> The average count. </returns>
    Public Function WriteAverageCount(ByVal value As Integer) As Integer?
        Me.AverageCount = Me.Write(value, Me.AverageCountCommandFormat)
        Return Me.AverageCount
    End Function

#End Region

#Region " AVERAGE PERCENT WINDOW "

    ''' <summary> The Average Percent Window. </summary>
    Private _AveragePercentWindow As Double?

    ''' <summary> Gets or sets the cached Average Percent Window. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property AveragePercentWindow As Double?
        Get
            Return Me._AveragePercentWindow
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.AveragePercentWindow, value) Then
                Me._AveragePercentWindow = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Average Percent Window. </summary>
    ''' <param name="value"> The Average Percent Window. </param>
    ''' <returns> The Average Percent Window. </returns>
    Public Function ApplyAveragePercentWindow(ByVal value As Double) As Double?
        Me.WriteAveragePercentWindow(value)
        Return Me.QueryAveragePercentWindow
    End Function

    ''' <summary> Gets or sets The Average Percent Window query command. </summary>
    ''' <value> The Average Percent Window query command. </value>
    Protected Overridable ReadOnly Property AveragePercentWindowQueryCommand As String

    ''' <summary> Queries The Average Percent Window. </summary>
    ''' <returns> The Average Percent Window or none if unknown. </returns>
    Public Function QueryAveragePercentWindow() As Double?
        Me.AveragePercentWindow = Me.Query(Me.AveragePercentWindow, Me.AveragePercentWindowQueryCommand)
        Return Me.AveragePercentWindow
    End Function

    ''' <summary> Gets or sets The Average Percent Window command format. </summary>
    ''' <value> The Average Percent Window command format. </value>
    Protected Overridable ReadOnly Property AveragePercentWindowCommandFormat As String

    ''' <summary> Writes The Average Percent Window without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Average Percent Window. </remarks>
    ''' <param name="value"> The Average Percent Window. </param>
    ''' <returns> The Average Percent Window. </returns>
    Public Function WriteAveragePercentWindow(ByVal value As Double) As Double?
        Me.AveragePercentWindow = Me.Write(value, Me.AveragePercentWindowCommandFormat)
        Return Me.AveragePercentWindow
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
    ''' <remarks> SCPI: "CURR:RANG:AUTO?" </remarks>
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
    ''' <remarks> SCPI: "CURR:RANG:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoRangeEnabledCommandFormat As String

    ''' <summary> Writes the Auto Range Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoRangeEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoRangeEnabled = Me.Write(value, Me.AutoRangeEnabledCommandFormat)
        Return Me.AutoRangeEnabled
    End Function

#End Region

#Region " AUTO ZERO ENABLED "

    ''' <summary> Auto Zero enabled. </summary>
    Private _AutoZeroEnabled As Boolean?

    ''' <summary> Gets or sets the cached Auto Zero Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Zero Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoZeroEnabled As Boolean?
        Get
            Return Me._AutoZeroEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoZeroEnabled, value) Then
                Me._AutoZeroEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Auto Zero Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoZeroEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoZeroEnabled(value)
        Return Me.QueryAutoZeroEnabled()
    End Function

    ''' <summary> Gets or sets the automatic Zero enabled query command. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    ''' <remarks> SCPI: "CURR:AZER?" </remarks>
    Protected Overridable ReadOnly Property AutoZeroEnabledQueryCommand As String

    ''' <summary> Queries the Auto Zero Enabled sentinel. Also sets the
    ''' <see cref="AutoZeroEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoZeroEnabled() As Boolean?
        Me.AutoZeroEnabled = Me.Query(Me.AutoZeroEnabled, Me.AutoZeroEnabledQueryCommand)
        Return Me.AutoZeroEnabled
    End Function

    ''' <summary> Gets or sets the automatic Zero enabled command Format. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    ''' <remarks> SCPI: "CURR:AZER {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoZeroEnabledCommandFormat As String

    ''' <summary> Writes the Auto Zero Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoZeroEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoZeroEnabled = Me.Write(value, Me.AutoZeroEnabledCommandFormat)
        Return Me.AutoZeroEnabled
    End Function

#End Region

#Region " DELAY "

    Private _Delay As TimeSpan?
    ''' <summary> Gets or sets the cached Trigger Delay. </summary>
    ''' <remarks> The delay is used to delay operation in the trigger layer. After the programmed
    ''' trigger event occurs, the instrument waits until the delay period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Trigger Delay or none if not set or unknown. </value>
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

    ''' <summary> Writes and reads back the Trigger Delay. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Trigger Delay or none if unknown. </returns>
    Public Function ApplyDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteDelay(value)
        Return Me.QueryDelay()
    End Function

    ''' <summary> Gets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    ''' <remarks> SCPI: ":SENS:FRES:DEL?" </remarks>
    Protected Overridable ReadOnly Property DelayQueryCommand As String

    ''' <summary> Gets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    ''' <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property DelayFormat As String

    ''' <summary> Queries the Delay. </summary>
    ''' <returns> The Delay or none if unknown. </returns>
    Public Function QueryDelay() As TimeSpan?
        Me.Delay = Me.Query(Me.Delay, Me.DelayFormat, Me.DelayQueryCommand)
        Return Me.Delay
    End Function

    ''' <summary> Gets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    ''' <remarks> SCPI: ":SENS:FRES:DEL {0:s\.FFFFFFF}" </remarks>
    Protected Overridable ReadOnly Property DelayCommandFormat As String

    ''' <summary> Writes the Trigger Delay without reading back the value from the device. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Trigger Delay or none if unknown. </returns>
    Public Function WriteDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.Delay = Me.Write(value, Me.DelayCommandFormat)
        Return Me.Delay
    End Function

#End Region

#Region " OPEN LEAD DETECTOR ENABLED "

    ''' <summary> Open Lead Detector enabled. </summary>
    Private _OpenLeadDetectorEnabled As Boolean?

    ''' <summary> Gets or sets the cached Open Lead Detector Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Open Lead Detector Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property OpenLeadDetectorEnabled As Boolean?
        Get
            Return Me._OpenLeadDetectorEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.OpenLeadDetectorEnabled, value) Then
                Me._OpenLeadDetectorEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Open Lead Detector Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyOpenLeadDetectorEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteOpenLeadDetectorEnabled(value)
        Return Me.QueryOpenLeadDetectorEnabled()
    End Function

    ''' <summary> Gets or sets the Open Lead Detector enabled query command. </summary>
    ''' <value> The Open Lead Detector enabled query command. </value>
    ''' <remarks> SCPI: ":FRES:ODET?" </remarks>
    Protected Overridable ReadOnly Property OpenLeadDetectorEnabledQueryCommand As String

    ''' <summary> Queries the Open Lead Detector Enabled sentinel. Also sets the
    ''' <see cref="OpenLeadDetectorEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryOpenLeadDetectorEnabled() As Boolean?
        Me.OpenLeadDetectorEnabled = Me.Query(Me.OpenLeadDetectorEnabled, Me.OpenLeadDetectorEnabledQueryCommand)
        Return Me.OpenLeadDetectorEnabled
    End Function

    ''' <summary> Gets or sets the Open Lead Detector enabled command Format. </summary>
    ''' <value> The Open Lead Detector enabled query command. </value>
    ''' <remarks> SCPI: ":FRES:ODET {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property OpenLeadDetectorEnabledCommandFormat As String

    ''' <summary> Writes the Open Lead Detector Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteOpenLeadDetectorEnabled(ByVal value As Boolean) As Boolean?
        Me.OpenLeadDetectorEnabled = Me.Write(value, Me.OpenLeadDetectorEnabledCommandFormat)
        Return Me.OpenLeadDetectorEnabled
    End Function

#End Region

#Region " POWER LINE CYCLES (NPLC) "

    ''' <summary> Gets the power line cycles decimal places. </summary>
    ''' <value> The power line decimal places. </value>
    Public ReadOnly Property PowerLineCyclesDecimalPlaces As Integer
        Get
            Return CInt(Math.Max(0, 1 - Math.Log10(Me.PowerLineCyclesRange.Min)))
        End Get
    End Property

    Private _PowerLineCyclesRange As Core.Pith.RangeR
    ''' <summary> The Range of the power line cycles. </summary>
    Public Property PowerLineCyclesRange As Core.Pith.RangeR
        Get
            Return Me._PowerLineCyclesRange
        End Get
        Set(value As Core.Pith.RangeR)
            ' force a unit change as the value needs to be updated when the subsystem is switched.
            Me._PowerLineCyclesRange = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

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

    ''' <summary> Gets or sets the cached sense PowerLineCycles. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property PowerLineCycles As Double?
        Get
            Return Me._PowerLineCycles
        End Get
        Protected Set(ByVal value As Double?)
            ' force a unit change as the value needs to be updated when the subsystem is switched.
            Me._PowerLineCycles = value
            Me.SafePostPropertyChanged()
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
    Protected Overridable ReadOnly Property PowerLineCyclesQueryCommand As String

    ''' <summary> Queries The Power Line Cycles. </summary>
    ''' <returns> The Power Line Cycles or none if unknown. </returns>
    Public Function QueryPowerLineCycles() As Double?
        Me.PowerLineCycles = Me.Query(Me.PowerLineCycles, Me.PowerLineCyclesQueryCommand)
        Return Me.PowerLineCycles
    End Function

    ''' <summary> Gets or sets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected Overridable ReadOnly Property PowerLineCyclesCommandFormat As String

    ''' <summary> Writes The Power Line Cycles without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Power Line Cycles. </remarks>
    ''' <param name="value"> The Power Line Cycles. </param>
    ''' <returns> The Power Line Cycles. </returns>
    Public Function WritePowerLineCycles(ByVal value As Double) As Double?
        Me.PowerLineCycles = Me.Write(value, Me.PowerLineCyclesCommandFormat)
        Return Me.PowerLineCycles
    End Function

#End Region

#Region " RANGE "

    ''' <summary> The range. </summary>
    Private _Range As Double?

    ''' <summary> Gets or sets the cached range. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Range As Double?
        Get
            Return Me._Range
        End Get
        Protected Set(ByVal value As Double?)
            ' force a unit change as the value needs to be updated when the subsystem is switched.
            Me._Range = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Writes and reads back the range. </summary>
    ''' <param name="value"> The range. </param>
    ''' <returns> The range. </returns>
    Public Function ApplyRange(ByVal value As Double) As Double?
        Me.WriteRange(value)
        Return Me.QueryRange
    End Function

    ''' <summary> Gets or sets the range query command. </summary>
    ''' <value> The range query command. </value>
    Protected Overridable ReadOnly Property RangeQueryCommand As String

    ''' <summary> Queries the range. </summary>
    ''' <returns> The range or none if unknown. </returns>
    Public Function QueryRange() As Double?
        Me.Range = Me.Query(Me.Range, Me.RangeQueryCommand)
        Return Me.Range
    End Function

    ''' <summary> Gets or sets the range command format. </summary>
    ''' <value> The range command format. </value>
    Protected Overridable ReadOnly Property RangeCommandFormat As String

    ''' <summary> Writes the range without reading back the value from the device. </summary>
    ''' <remarks> This command sets the range. </remarks>
    ''' <param name="value"> The range. </param>
    ''' <returns> The range. </returns>
    Public Function WriteRange(ByVal value As Double) As Double?
        Me.Range = Me.Write(value, Me.RangeCommandFormat)
        Return Me.Range
    End Function

#End Region

#Region " PROTECTION LEVEL "

    ''' <summary> The Current Limit. </summary>
    Private _ProtectionLevel As Double?

    ''' <summary> Gets or sets the cached source current Limit for a voltage source. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
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
    Protected Overridable ReadOnly Property ProtectionLevelQueryCommand As String

    ''' <summary> Queries the protection level. </summary>
    ''' <returns> the protection level or none if unknown. </returns>
    Public Function QueryProtectionLevel() As Double?
        Me.ProtectionLevel = Me.Query(Me.ProtectionLevel, Me.ProtectionLevelQueryCommand)
        Return Me.ProtectionLevel
    End Function

    ''' <summary> Gets or sets the protection level command format. </summary>
    ''' <value> the protection level command format. </value>
    Protected Overridable ReadOnly Property ProtectionLevelCommandFormat As String

    ''' <summary> Writes the protection level without reading back the value from the device. </summary>
    ''' <remarks> This command sets the protection level. </remarks>
    ''' <param name="value"> the protection level. </param>
    ''' <returns> the protection level. </returns>
    Public Function WriteProtectionLevel(ByVal value As Double) As Double?
        Me.ProtectionLevel = Me.Write(value, Me.ProtectionLevelCommandFormat)
        Return Me.ProtectionLevel
    End Function

#End Region

#Region " PROTECTION ENABLED "

    ''' <summary> Protection enabled. </summary>
    Private _ProtectionEnabled As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether Sense Voltage protection is enabled. </summary>
    ''' <remarks> :SENSE:VOLT:PROT:STAT The setter enables or disables the over-Voltage protection (OCP)
    ''' function. The enabled state is On (1); the disabled state is Off (0). If the over-Voltage
    ''' protection function is enabled and the output goes into constant Voltage operation, the
    ''' output is disabled and OCP is set in the Questionable Condition status register. The *RST
    ''' value = Off. </remarks>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property ProtectionEnabled As Boolean?
        Get
            Return Me._ProtectionEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.ProtectionEnabled, value) Then
                Me._ProtectionEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Protection Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyProtectionEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteProtectionEnabled(value)
        Return Me.QueryProtectionEnabled()
    End Function

    ''' <summary> Gets the Protection enabled query command. </summary>
    ''' <value> The Protection enabled query command. </value>
    ''' <remarks> SCPI: "system:RANG:AUTO?" </remarks>
    Protected Overridable ReadOnly Property ProtectionEnabledQueryCommand As String

    ''' <summary> Queries the Protection Enabled sentinel. Also sets the
    ''' <see cref="ProtectionEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryProtectionEnabled() As Boolean?
        Me.ProtectionEnabled = Me.Query(Me.ProtectionEnabled, Me.ProtectionEnabledQueryCommand)
        Return Me.ProtectionEnabled
    End Function

    ''' <summary> Gets the Protection enabled command Format. </summary>
    ''' <value> The Protection enabled query command. </value>
    ''' <remarks> SCPI: "system:RANGE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property ProtectionEnabledCommandFormat As String

    ''' <summary> Writes the Protection Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteProtectionEnabled(ByVal value As Boolean) As Boolean?
        Me.ProtectionEnabled = Me.Write(value, Me.ProtectionEnabledCommandFormat)
        Return Me.ProtectionEnabled
    End Function

#End Region

End Class
