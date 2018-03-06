Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines a Multimeter Subsystem for a TSP System. </summary>
''' <license> (c) 2016 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2016" by="David" revision=""> Created. </history>
Public MustInherit Class MultimeterSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystemBase">status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._Amount = New MeasuredAmount(ReadingTypes.Reading)
        Me._FunctionModeRanges = New FunctionRangeDictionary
        For Each fm As MultimeterFunctionMode In [Enum].GetValues(GetType(MultimeterFunctionMode))
            Me._FunctionModeRanges.Add(fm, New Core.Pith.RangeR(0, 1))
        Next
        Me._OpenDetectorKnownStates = New MultimeterFunctionEnabledDictionary
        For Each fm As MultimeterFunctionMode In [Enum].GetValues(GetType(MultimeterFunctionMode))
            Me._OpenDetectorKnownStates.Add(fm, False)
        Next
        Me.ApertureRange = Core.Pith.RangeR.FullNonnegative
        Me.FilterCountRange = Core.Pith.RangeI.FullNonnegative
        Me.FilterWindowRange = Core.Pith.RangeR.FullNonnegative
        Me.FunctionUnit = Me.DefaultFunctionUnit
        Me.PowerLineCyclesRange = Core.Pith.RangeR.FullNonnegative
        Me.FunctionRange = Me.DefaultFunctionRange
        Me.FunctionRangeDecimalPlaces = Me.DefaultFunctionModeDecimalPlaces
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " APERTURE "

    Private _ApertureRange As Core.Pith.RangeR
    ''' <summary> The aperture range in seconds. </summary>
    Public Property ApertureRange As Core.Pith.RangeR
        Get
            Return Me._ApertureRange
        End Get
        Set(value As Core.Pith.RangeR)
            If Me.ApertureRange <> value Then
                Me._ApertureRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> The Aperture. </summary>
    Private _Aperture As Double?

    ''' <summary> Gets or sets the cached Multimeter Aperture. Set to. </summary>
    ''' <remarks>
    ''' The aperture sets the amount of time the ADC takes when making a measurement, which is the
    ''' integration period For the selected measurement Function. The integration period Is specified
    ''' In seconds. In general, a short integration period provides a fast reading rate, while a long
    ''' integration period provides better accuracy. The selected integration period Is a compromise
    ''' between speed And accuracy. During the integration period, If an external trigger With a
    ''' count Of 1 Is sent, the trigger Is ignored. If the count Is Set To more than 1, the first
    ''' reading Is initialized by this trigger. Subsequent readings occur as rapidly as the
    ''' instrument can make them. If a trigger occurs during the group measurement, the trigger Is
    ''' latched And another group Of measurements With the same count will be triggered after the
    ''' current group completes. You can also Set the integration rate by setting the number Of power
    ''' line cycles (NPLCs). Changing the NPLC value changes the aperture time And changing the
    ''' aperture time changes the NPLC value.
    ''' </remarks>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Aperture As Double?
        Get
            Return Me._Aperture
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Aperture, value) Then
                Me._Aperture = value
                If value.HasValue Then
                    Me.PowerLineCycles = StatusSubsystemBase.ToPowerLineCycles(TimeSpan.FromTicks(CLng(TimeSpan.TicksPerSecond * Me._Aperture.Value)))
                Else
                    Me.PowerLineCycles = New Double?
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Multimeter Aperture. </summary>
    ''' <param name="value"> The Aperture. </param>
    ''' <returns> The Aperture. </returns>
    Public Function ApplyAperture(ByVal value As Double) As Double?
        Me.WriteAperture(value)
        Return Me.QueryAperture
    End Function

    ''' <summary> Gets or sets The Aperture query command. </summary>
    ''' <value> The Aperture query command. </value>
    Protected Overridable ReadOnly Property ApertureQueryCommand As String

    ''' <summary> Queries The Aperture. </summary>
    ''' <returns> The Aperture or none if unknown. </returns>
    Public Function QueryAperture() As Double?
        Me.Aperture = Me.Query(Me.Aperture, Me.ApertureQueryCommand)
        Return Me.Aperture
    End Function

    ''' <summary> Gets or sets The Aperture command format. </summary>
    ''' <value> The Aperture command format. </value>
    Protected Overridable ReadOnly Property ApertureCommandFormat As String

    ''' <summary> Writes The Aperture without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Aperture. </remarks>
    ''' <param name="value"> The Aperture. </param>
    ''' <returns> The Aperture. </returns>
    Public Function WriteAperture(ByVal value As Double) As Double?
        value = If(value > ApertureRange.Max, ApertureRange.Max, If(value < ApertureRange.Min, ApertureRange.Min, value))
        Me.Aperture = Me.Write(value, Me.ApertureCommandFormat)
        Return Me.Aperture
    End Function

#End Region

#Region " AUTO DELAY ENABLED "

    ''' <summary> Auto Delay enabled. </summary>
    Private _AutoDelayEnabled As Boolean?

    ''' <summary> Gets or sets the cached Auto Delay Enabled sentinel. </summary>
    ''' <remarks> When this is enabled, a delay is added before each measurement. </remarks>
    ''' <value>
    ''' <c>null</c> if Auto Delay Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>.
    ''' </value>
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

    ''' <summary> Gets or sets the automatic Delay enabled query command. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    Protected Overridable ReadOnly Property AutoDelayEnabledQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="AutoDelayEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoDelayEnabled() As Boolean?
        Me.AutoDelayEnabled = Me.Query(Me.AutoDelayEnabled, Me.AutoDelayEnabledQueryCommand)
        Return Me.AutoDelayEnabled
    End Function

    ''' <summary> Gets or sets the automatic Delay enabled command Format. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
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
    ''' <remarks>
    ''' When this command is set to off, you must set the range. If you do not set the range, the
    ''' instrument remains at the range that was selected by auto range. When this command Is set to
    ''' on, the instrument automatically goes to the most sensitive range to perform the measurement.
    ''' If a range Is manually selected through the front panel Or a remote command, this command Is
    ''' automatically set to off. Auto range selects the best range In which To measure the signal
    ''' that Is applied To the input terminals of the instrument. When auto range Is enabled, the
    ''' range increases at 120 percent of range And decreases occurs When the reading Is less than 10
    ''' percent Of nominal range. For example, If you are On the 1 volt range And auto range Is
    ''' enabled, the instrument auto ranges up To the 10 volt range When the measurement exceeds 1.2
    ''' volts. It auto ranges down To the 100 mV range When the measurement falls below 1 volt.
    ''' </remarks>
    ''' <value>
    ''' <c>null</c> if Auto Range Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>.
    ''' </value>
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
    ''' <remarks>
    ''' To ensure the accuracy of readings, the instrument must periodically get new measurements of
    ''' its internal ground And voltage reference. The time interval between updates To these
    ''' reference measurements Is determined by the integration aperture that Is being used for
    ''' measurements. The Model DMM7510 uses separate reference And zero measurements For Each
    ''' aperture. By Default, the instrument automatically checks these reference measurements
    ''' whenever a signal measurement Is made. The time To make the reference measurements Is In
    ''' addition To the normal measurement time. If timing Is critical, you can disable auto zero to
    ''' avoid this time penalty. When auto zero Is set to off, the instrument may gradually drift out
    ''' of specification. To minimize the drift, you can send the once command to make a reference
    ''' And zero measurement immediately before a test sequence. For AC voltage And AC current
    ''' measurements where the detector bandwidth Is set To 3 Hz Or 30 Hz, auto zero Is set on And
    ''' cannot be changed.
    ''' </remarks>
    ''' <value>
    ''' <c>null</c> if Auto Zero Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>.
    ''' </value>
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
    ''' <remarks> SCPI: "system:RANG:AUTO?" </remarks>
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
    ''' <remarks> SCPI: "system:Zero:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoZeroEnabledCommandFormat As String

    ''' <summary> Writes the Auto Zero Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoZeroEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoZeroEnabled = Me.Write(value, Me.AutoZeroEnabledCommandFormat)
        Return Me.AutoZeroEnabled
    End Function

#End Region

#Region " AUTO ZERO ONCE "

    ''' <summary> Gets or sets the automatic zero once command. </summary>
    ''' <value> The automatic zero once command. </value>
    Protected Overridable ReadOnly Property AutoZeroOnceCommand As String

    ''' <summary> Request a single auto zero. </summary>
    Public Sub AutoZeroOnce()
        Me.Execute(Me.AutoZeroOnceCommand)
    End Sub

#End Region

#Region " BIAS "

#Region " BIAS ACTUAL "

    ''' <summary> The BiasActual. </summary>
    Private _BiasActual As Double?

    ''' <summary> Gets or sets the cached Multimeter Bias Actual. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property BiasActual As Double?
        Get
            Return Me._BiasActual
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.BiasActual, value) Then
                Me._BiasActual = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets The Bias Actual query command. </summary>
    ''' <value> The BiasActual query command. </value>
    Protected Overridable ReadOnly Property BiasActualQueryCommand As String

    ''' <summary> Queries The Bias Actual. </summary>
    ''' <returns> The Bias Actual or none if unknown. </returns>
    Public Function QueryBiasActual() As Double?
        Me.BiasActual = Me.Query(Me.BiasActual, Me.BiasActualQueryCommand)
        Return Me.BiasActual
    End Function

#End Region

#Region " BIAS LEVEL "

    ''' <summary> The BiasLevel. </summary>
    Private _BiasLevel As Double?

    ''' <summary> Gets or sets the cached Multimeter Bias Level. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    ''' <remarks> Selects the amount of current that is sourced by the instrument to make measurements. Applies to diode measurements. </remarks>
    Public Overloads Property BiasLevel As Double?
        Get
            Return Me._BiasLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.BiasLevel, value) Then
                Me._BiasLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Multimeter Bias Level. </summary>
    ''' <param name="value"> The Bias Level. </param>
    ''' <returns> The Bias Level. </returns>
    Public Function ApplyBiasLevel(ByVal value As Double) As Double?
        Me.WriteBiasLevel(value)
        Return Me.QueryBiasLevel
    End Function

    ''' <summary> Gets or sets The Bias Level query command. </summary>
    ''' <value> The BiasLevel query command. </value>
    Protected Overridable ReadOnly Property BiasLevelQueryCommand As String

    ''' <summary> Queries The Bias Level. </summary>
    ''' <returns> The Bias Level or none if unknown. </returns>
    Public Function QueryBiasLevel() As Double?
        Dim value As Double? = Me.Query(Me.BiasLevel, Me.BiasLevelQueryCommand)
        If value.HasValue Then Me.BiasLevel = value.Value Else Me.BiasLevel = New Double?
        Return Me.BiasLevel
    End Function

    ''' <summary> Gets or sets The Bias Level command format. </summary>
    ''' <value> The BiasLevel command format. </value>
    Protected Overridable ReadOnly Property BiasLevelCommandFormat As String

    ''' <summary> Writes The Bias Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Bias Level. </remarks>
    ''' <param name="value"> The Bias Level. </param>
    ''' <returns> The Bias Level. </returns>
    Public Function WriteBiasLevel(ByVal value As Double) As Double?
        Me.BiasLevel = Me.Write(value, Me.BiasLevelCommandFormat)
        Return Me.BiasLevel
    End Function

#End Region

#End Region

#Region " FILTER "

#Region " FILTER COUNT "

    Private _FilterCountRange As Core.Pith.RangeI
    ''' <summary> The Filter Count range in seconds. </summary>
    Public Property FilterCountRange As Core.Pith.RangeI
        Get
            Return Me._FilterCountRange
        End Get
        Set(value As Core.Pith.RangeI)
            If Me.FilterCountRange <> value Then
                Me._FilterCountRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> The FilterCount. </summary>
    Private _FilterCount As Integer?

    ''' <summary> Gets or sets the cached Multimeter Filter Count. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property FilterCount As Integer?
        Get
            Return Me._FilterCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.FilterCount, value) Then
                Me._FilterCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Multimeter Filter Count. </summary>
    ''' <param name="value"> The Filter Count. </param>
    ''' <returns> The Filter Count. </returns>
    Public Function ApplyFilterCount(ByVal value As Integer) As Integer?
        Me.WriteFilterCount(value)
        Return Me.QueryFilterCount
    End Function

    ''' <summary> Gets or sets The Filter Count query command. </summary>
    ''' <value> The FilterCount query command. </value>
    Protected Overridable ReadOnly Property FilterCountQueryCommand As String

    ''' <summary> Queries The Filter Count. </summary>
    ''' <returns> The Filter Count or none if unknown. </returns>
    Public Function QueryFilterCount() As Integer?
        Me.FilterCount = Me.Query(Me.FilterCount, Me.FilterCountQueryCommand)
        Return Me.FilterCount
    End Function

    ''' <summary> Gets or sets The Filter Count command format. </summary>
    ''' <value> The FilterCount command format. </value>
    Protected Overridable ReadOnly Property FilterCountCommandFormat As String

    ''' <summary> Writes The Filter Count without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Filter Count. </remarks>
    ''' <param name="value"> The Filter Count. </param>
    ''' <returns> The Filter Count. </returns>
    Public Function WriteFilterCount(ByVal value As Integer) As Integer?
        value = If(value > FilterCountRange.Max, FilterCountRange.Max, If(value < FilterCountRange.Min, FilterCountRange.Min, value))
        Me.FilterCount = Me.Write(value, Me.FilterCountCommandFormat)
        Return Me.FilterCount
    End Function

#End Region

#Region " FILTER ENABLED "

    ''' <summary> Filter enabled. </summary>
    Private _FilterEnabled As Boolean?

    ''' <summary> Gets or sets the cached Filter Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Filter Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property FilterEnabled As Boolean?
        Get
            Return Me._FilterEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.FilterEnabled, value) Then
                Me._FilterEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Filter Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyFilterEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteFilterEnabled(value)
        Return Me.QueryFilterEnabled()
    End Function

    ''' <summary> Gets or sets the Filter enabled query command. </summary>
    ''' <value> The Filter enabled query command. </value>
    ''' <remarks> TSP: "print(dmm.filter.enable==1)" </remarks>
    Protected Overridable ReadOnly Property FilterEnabledQueryCommand As String

    ''' <summary> Queries the Filter Enabled sentinel. Also sets the
    ''' <see cref="FilterEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryFilterEnabled() As Boolean?
        Me.FilterEnabled = Me.Query(Me.FilterEnabled, Me.FilterEnabledQueryCommand)
        Return Me.FilterEnabled
    End Function

    ''' <summary> Gets or sets the Filter enabled command Format. </summary>
    ''' <value> The Filter enabled query command. </value>
    ''' <remarks> TSP "dmm.filter.enable={0:'1';'1';'0'}" </remarks>
    Protected Overridable ReadOnly Property FilterEnabledCommandFormat As String

    ''' <summary> Writes the Filter Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteFilterEnabled(ByVal value As Boolean) As Boolean?
        Me.FilterEnabled = Me.Write(value, Me.FilterEnabledCommandFormat)
        Return Me.FilterEnabled
    End Function

#End Region

#Region " MOVING AVERAGE FILTER ENABLED "

    ''' <summary> Moving Average Filter enabled. </summary>
    Private _MovingAverageFilterEnabled As Boolean?

    ''' <summary> Gets or sets the cached Moving Average Filter Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Moving Average Filter Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property MovingAverageFilterEnabled As Boolean?
        Get
            Return Me._MovingAverageFilterEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.MovingAverageFilterEnabled, value) Then
                Me._MovingAverageFilterEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Moving Average Filter Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyMovingAverageFilterEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteMovingAverageFilterEnabled(value)
        Return Me.QueryMovingAverageFilterEnabled()
    End Function

    ''' <summary> Gets or sets the Moving Average Filter enabled query command. </summary>
    ''' <value> The Moving Average Filter enabled query command. </value>
    ''' <remarks> TSP: "print(dmm.filter.type=0)" </remarks>
    Protected Overridable ReadOnly Property MovingAverageFilterEnabledQueryCommand As String

    ''' <summary> Queries the Moving Average Filter Enabled sentinel. Also sets the
    ''' <see cref="MovingAverageFilterEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryMovingAverageFilterEnabled() As Boolean?
        Me.MovingAverageFilterEnabled = Me.Query(Me.MovingAverageFilterEnabled, Me.MovingAverageFilterEnabledQueryCommand)
        Return Me.MovingAverageFilterEnabled
    End Function

    ''' <summary> Gets or sets the Moving Average Filter enabled command Format. </summary>
    ''' <value> The Moving Average Filter enabled query command. </value>
    ''' <remarks> TSP: "dmm.filter.type={0:'0';'0';'1'}" </remarks>
    Protected Overridable ReadOnly Property MovingAverageFilterEnabledCommandFormat As String

    ''' <summary> Writes the Moving Average Filter Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteMovingAverageFilterEnabled(ByVal value As Boolean) As Boolean?
        Me.MovingAverageFilterEnabled = Me.Write(value, Me.MovingAverageFilterEnabledCommandFormat)
        Return Me.MovingAverageFilterEnabled
    End Function

#End Region

#Region " FILTER WINDOW "

    Private _FilterWindowRange As Core.Pith.RangeR
    ''' <summary> The Filter Window range. </summary>
    Public Property FilterWindowRange As Core.Pith.RangeR
        Get
            Return Me._FilterWindowRange
        End Get
        Set(value As Core.Pith.RangeR)
            If Me.FilterWindowRange <> value Then
                Me._FilterWindowRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> The FilterWindow. </summary>
    Private _FilterWindow As Double?

    ''' <summary> Gets or sets the cached Multimeter Filter Window. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property FilterWindow As Double?
        Get
            Return Me._FilterWindow
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.FilterWindow, value) Then
                Me._FilterWindow = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Multimeter Filter Window. </summary>
    ''' <param name="value"> The Filter Window. </param>
    ''' <returns> The Filter Window. </returns>
    Public Function ApplyFilterWindow(ByVal value As Double) As Double?
        Me.WriteFilterWindow(value)
        Return Me.QueryFilterWindow
    End Function

    ''' <summary> Gets or sets The Filter Window query command. </summary>
    ''' <value> The FilterWindow query command. </value>
    Protected Overridable ReadOnly Property FilterWindowQueryCommand As String

    ''' <summary> Queries The Filter Window. </summary>
    ''' <returns> The Filter Window or none if unknown. </returns>
    Public Function QueryFilterWindow() As Double?
        Dim value As Double? = Me.Query(Me.FilterWindow, Me.FilterWindowQueryCommand)
        If value.HasValue Then Me.FilterWindow = 100 * value.Value Else Me.FilterWindow = New Double?
        Return Me.FilterWindow
    End Function

    ''' <summary> Gets or sets The Filter Window command format. </summary>
    ''' <value> The FilterWindow command format. </value>
    Protected Overridable ReadOnly Property FilterWindowCommandFormat As String

    ''' <summary> Writes The Filter Window without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Filter Window. </remarks>
    ''' <param name="value"> The Filter Window. </param>
    ''' <returns> The Filter Window. </returns>
    Public Function WriteFilterWindow(ByVal value As Double) As Double?
        value = If(value > FilterWindowRange.Max, FilterWindowRange.Max, If(value < FilterWindowRange.Min, FilterWindowRange.Min, value))
        Me.FilterWindow = Me.Write(100 * value, Me.FilterWindowCommandFormat)
        Return Me.FilterWindow
    End Function

#End Region

#End Region

#Region " FRONT TERMINALS SELECTED "

    ''' <summary> Front Terminals Selected. </summary>
    Private _FrontTerminalsSelected As Boolean?

    ''' <summary> Gets or sets the cached Front Terminals Selected sentinel. </summary>
    ''' <value> <c>null</c> if Front Terminals Selected is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property FrontTerminalsSelected As Boolean?
        Get
            Return Me._FrontTerminalsSelected
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.FrontTerminalsSelected, value) Then
                Me._FrontTerminalsSelected = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Front Terminals Selected sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyFrontTerminalsSelected(ByVal value As Boolean) As Boolean?
        Me.WriteFrontTerminalsSelected(value)
        Return Me.QueryFrontTerminalsSelected()
    End Function

    ''' <summary> Gets or sets the front terminals selected print query command. </summary>
    ''' <value> The front terminals selected query command. </value>
    Protected Overridable ReadOnly Property FrontTerminalsSelectedPrintCommand As String

    ''' <summary> Queries the Front Terminals Selected sentinel. Also sets the
    ''' <see cref="FrontTerminalsSelected">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryFrontTerminalsSelected() As Boolean?
        Me.FrontTerminalsSelected = Me.Query(Me.FrontTerminalsSelected, Me.FrontTerminalsSelectedPrintCommand)
        Return Me.FrontTerminalsSelected
    End Function

    ''' <summary> Gets or sets the front terminals selected command format. </summary>
    ''' <value> The front terminals selected command format. </value>
    Protected Overridable ReadOnly Property FrontTerminalsSelectedCommandFormat As String

    ''' <summary> Writes the Front Terminals Selected sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteFrontTerminalsSelected(ByVal value As Boolean) As Boolean?
        Me.FrontTerminalsSelected = Me.Write(value, Me.FrontTerminalsSelectedCommandFormat)
        Return Me.FrontTerminalsSelected
    End Function

#End Region

#Region " FUNCTION MODE "

    ''' <summary> The function mode. </summary>
    Private _FunctionMode As MultimeterFunctionMode?

    ''' <summary> Gets or sets the cached function mode. </summary>
    ''' <value> The <see cref="MultimeterFunctionMode">Multimeter Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FunctionMode As MultimeterFunctionMode?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As MultimeterFunctionMode?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                If value.HasValue Then
                    Me.FunctionUnit = Me.ToUnit(value.Value)
                    Me.FunctionRange = Me.ToRange(value.Value)
                    Me.FunctionRangeDecimalPlaces = Me.ToDecimalPlaces(value.Value)
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Multimeter Function Mode. </summary>
    ''' <param name="value"> The  Multimeter Function Mode. </param>
    ''' <returns> The <see cref="MultimeterFunctionMode">Multimeter Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As MultimeterFunctionMode) As MultimeterFunctionMode?
        Me.WriteFunctionMode(value)
        Me.QueryFunctionMode()
        ' changing the function mode changes range, auto delay mode and open detector enabled. 
        Me.QueryAperture()
        Me.QueryRange()
        Me.QueryAutoDelayEnabled()
        Me.QueryOpenDetectorEnabled()
        Return FunctionMode
    End Function

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command. </value>
    Protected Overridable ReadOnly Property FunctionModeQueryCommand As String

    ''' <summary> Queries the Measure Function Mode. </summary>
    ''' <returns> The <see cref="MultimeterFunctionMode">Measure Function Mode</see> or none if unknown. </returns>
    Public Overridable Function QueryFunctionMode() As MultimeterFunctionMode?
        Dim mode As String = Me.FunctionMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.FunctionModeQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(MeasureSubsystemBase)}.{NameOf(FunctionMode)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.FunctionMode = New MultimeterFunctionMode?
        Else
            Dim se As New StringEnumerator(Of MultimeterFunctionMode)
            Me.FunctionMode = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.FunctionMode
    End Function

    ''' <summary> Gets or sets the function mode command format. </summary>
    ''' <value> The function mode command format. </value>
    Protected Overridable ReadOnly Property FunctionModeCommandFormat As String

    ''' <summary> Writes the Measure Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="MultimeterFunctionMode">Measure Function Mode</see> or none if unknown. </returns>
    Public Overridable Function WriteFunctionMode(ByVal value As MultimeterFunctionMode) As MultimeterFunctionMode?
        Me.Session.WriteLine(Me.FunctionModeCommandFormat, value.ExtractBetween())
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

#End Region

#Region " FUNCTION MODE UNIT "

    ''' <summary> Gets or sets the default measurement unit. </summary>
    ''' <value> The default measure unit. </value>
    Public Property DefaultMeasurementUnit As Arebis.TypedUnits.Unit

    ''' <summary> Gets or sets the function unit. </summary>
    ''' <value> The function unit. </value>
    Public Property MeasurementUnit As Arebis.TypedUnits.Unit
        Get
            Return Me.Amount.Unit
        End Get
        Set(value As Arebis.TypedUnits.Unit)
            If Me.MeasurementUnit <> value Then
                Me.Amount.Unit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Converts a a measure unit to a measurement unit. </summary>
    ''' <param name="value"> The Measure Unit. </param>
    ''' <returns> Value as an Arebis.TypedUnits.Unit. </returns>
    Public Function ToMeasurementUnit(ByVal value As Tsp2.MultimeterMeasurementUnit) As Arebis.TypedUnits.Unit
        Dim result As Arebis.TypedUnits.Unit = Me.DefaultFunctionUnit
        Select Case value
            Case Tsp2.MultimeterMeasurementUnit.Celsius
                result = Arebis.StandardUnits.TemperatureUnits.DegreeCelsius
            Case Tsp2.MultimeterMeasurementUnit.Decibel
                result = Arebis.StandardUnits.UnitlessUnits.Decibel
            Case Tsp2.MultimeterMeasurementUnit.Fahrenheit
                result = Arebis.StandardUnits.TemperatureUnits.DegreeFahrenheit
            Case Tsp2.MultimeterMeasurementUnit.Kelvin
                result = Arebis.StandardUnits.TemperatureUnits.Kelvin
            Case Tsp2.MultimeterMeasurementUnit.Volt
                result = Arebis.StandardUnits.ElectricUnits.Volt
            Case Else
                result = Me.MeasurementUnit
        End Select
        Return result
    End Function

    ''' <summary> Writes and reads back the Measure Unit. </summary>
    ''' <param name="value"> The  Measure Unit. </param>
    ''' <returns> The <see cref="MeasurementUnit">Measure Unit</see> or none if unknown. </returns>
    Public Function ApplyMultimeterMeasurementUnit(ByVal value As MultimeterMeasurementUnit) As MultimeterMeasurementUnit?
        Me.WriteMultimeterMeasurementUnit(value)
        Return Me.QueryMultimeterMeasurementUnit
    End Function

    ''' <summary> The Unit. </summary>
    Private _MultimeterMeasurementUnit As MultimeterMeasurementUnit?

    ''' <summary> Gets or sets the cached measure Unit.  This is the actual unit for measurement. </summary>
    ''' <value> The <see cref="MeasurementUnit">Measure Unit</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property MultimeterMeasurementUnit As MultimeterMeasurementUnit?
        Get
            Return Me._MultimeterMeasurementUnit
        End Get
        Protected Set(ByVal value As MultimeterMeasurementUnit?)
            If Not Nullable.Equals(Me.MultimeterMeasurementUnit, value) Then
                Me._MultimeterMeasurementUnit = value
                If value.HasValue Then
                    Me.MeasurementUnit = Me.ToMeasurementUnit(value.Value)
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Measure Unit query command. </summary>
    ''' <remarks>
    ''' The query command uses the <see cref="SessionBase.QueryPrintTrimEnd(Integer, String)"/>
    ''' </remarks>
    ''' <value> The Unit query command. </value>
    Protected Overridable ReadOnly Property MultimeterMeasurementUnitQueryCommand As String

    ''' <summary> Queries the Measure Unit. </summary>
    ''' <returns> The <see cref="MultimeterMeasurementUnit">Measure Unit</see> or none if unknown. </returns>
    Public Overridable Function QueryMultimeterMeasurementUnit() As MultimeterMeasurementUnit?
        Dim mode As String = Me.MultimeterMeasurementUnit.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.MultimeterMeasurementUnitQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(MeasureSubsystemBase)}.{NameOf(MultimeterMeasurementUnit)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.MultimeterMeasurementUnit = New MultimeterMeasurementUnit?
        Else
            Dim se As New StringEnumerator(Of MultimeterMeasurementUnit)
            Me.MultimeterMeasurementUnit = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.MultimeterMeasurementUnit
    End Function

    ''' <summary> Gets or sets the Measure Unit command format. </summary>
    ''' <value> The Unit command format. </value>
    Protected Overridable ReadOnly Property MultimeterMeasurementUnitCommandFormat As String

    ''' <summary> Writes the Measure Unit without reading back the value from the device. </summary>
    ''' <param name="value"> The Unit. </param>
    ''' <returns> The <see cref="MultimeterMeasurementUnit">Measure Unit</see> or none if unknown. </returns>
    Public Overridable Function WriteMultimeterMeasurementUnit(ByVal value As MultimeterMeasurementUnit) As MultimeterMeasurementUnit?
        Me.Session.WriteLine(Me.MultimeterMeasurementUnitCommandFormat, value.ExtractBetween())
        Me.MultimeterMeasurementUnit = value
        Return Me.MultimeterMeasurementUnit
    End Function

#End Region

#Region " FUNCTION MODE RANGE "

    ''' <summary> Gets or sets the function mode ranges. </summary>
    ''' <value> The function mode ranges. </value>
    Public ReadOnly Property FunctionModeRanges As FunctionRangeDictionary

    ''' <summary> Gets or sets the default function range. </summary>
    ''' <value> The default function range. </value>
    Public Property DefaultFunctionRange As isr.Core.Pith.RangeR = isr.Core.Pith.RangeR.Full

    ''' <summary> Converts a functionMode to a range. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an isr.Core.Pith.RangeR. </returns>
    Public Overridable Function ToRange(ByVal functionMode As MultimeterFunctionMode) As isr.Core.Pith.RangeR
        Return Me.FunctionModeRanges(functionMode)
    End Function

    Private _FunctionRange As Core.Pith.RangeR
    ''' <summary> The Range of the range. </summary>
    Public Property FunctionRange As Core.Pith.RangeR
        Get
            Return Me._FunctionRange
        End Get
        Set(value As Core.Pith.RangeR)
            If Me.FunctionRange <> value Then
                Me._FunctionRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the default decimal places. </summary>
    ''' <value> The default decimal places. </value>
    Public Property DefaultFunctionModeDecimalPlaces As Integer = 3

    ''' <summary> Converts a function Mode to a decimal places. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an Integer. </returns>
    Public Overridable Function ToDecimalPlaces(ByVal functionMode As MultimeterFunctionMode) As Integer
        Return Me.DefaultFunctionModeDecimalPlaces
    End Function

    Private _FunctionRangeDecimalPlaces As Integer

    ''' <summary> Gets or sets the function range decimal places. </summary>
    ''' <value> The function range decimal places. </value>
    Public Property FunctionRangeDecimalPlaces As Integer
        Get
            Return Me._FunctionRangeDecimalPlaces
        End Get
        Set(value As Integer)
            If Me.FunctionRangeDecimalPlaces <> value Then
                Me._FunctionRangeDecimalPlaces = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " FUNCTION MODE RANGE "

    ''' <summary> Gets or sets the default unit. </summary>
    ''' <value> The default unit. </value>
    Public Property DefaultFunctionUnit As Arebis.TypedUnits.Unit = Arebis.StandardUnits.ElectricUnits.Volt

    ''' <summary> Parse units. </summary>
    ''' <param name="functionMode"> The  Multimeter Function Mode. </param>
    ''' <returns> An Arebis.TypedUnits.Unit. </returns>
    Public Overridable Function ToUnit(ByVal functionMode As MultimeterFunctionMode) As Arebis.TypedUnits.Unit
        Dim result As Arebis.TypedUnits.Unit = Me.DefaultFunctionUnit
        Select Case functionMode
            Case MultimeterFunctionMode.CurrentAC, MultimeterFunctionMode.CurrentDC
                result = Arebis.StandardUnits.ElectricUnits.Ampere
            Case MultimeterFunctionMode.VoltageAC, MultimeterFunctionMode.VoltageDC
                result = Arebis.StandardUnits.ElectricUnits.Volt
            Case MultimeterFunctionMode.ResistanceFourWire, MultimeterFunctionMode.ResistanceTwoWire
                result = Arebis.StandardUnits.ElectricUnits.Ohm
            Case MultimeterFunctionMode.Temperature
                result = Arebis.StandardUnits.TemperatureUnits.DegreeCelsius
            Case MultimeterFunctionMode.Period
                result = Arebis.StandardUnits.TimeUnits.Second
            Case MultimeterFunctionMode.Frequency
                result = Arebis.StandardUnits.FrequencyUnits.Hertz
            Case MultimeterFunctionMode.Capacitance
                result = Arebis.StandardUnits.ElectricUnits.Farad
            Case MultimeterFunctionMode.Diode
                result = Arebis.StandardUnits.ElectricUnits.Volt
            Case MultimeterFunctionMode.Ratio
                result = Arebis.StandardUnits.UnitlessUnits.Ratio
        End Select
        Return result
    End Function

    ''' <summary> Gets or sets the function unit. </summary>
    ''' <value> The function unit. </value>
    Public Property FunctionUnit As Arebis.TypedUnits.Unit
        Get
            Return Me.Amount.Unit
        End Get
        Set(value As Arebis.TypedUnits.Unit)
            If Me.FunctionUnit <> value Then
                Me.Amount.Unit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " LIMIT 1 "

#Region " LIMIT1 AUTO CLEAR "

    ''' <summary> Limit1 Auto Clear. </summary>
    Private _Limit1AutoClear As Boolean?

    ''' <summary> Gets or sets the cached Limit1 Auto Clear sentinel. </summary>
    ''' <remarks>
    ''' When auto clear is set to on for a measure function, limit conditions are cleared
    ''' automatically after each measurement. If you are making a series of measurements, the
    ''' instrument shows the limit test result of the last measurement for the pass Or fail
    ''' indication for the limit. If you want To know If any Of a series Of measurements failed the
    ''' limit, Set the auto clear setting To off. When this set to off, a failed indication Is Not
    ''' cleared automatically. It remains set until it Is cleared With the clear command. The auto
    ''' clear setting affects both the high And low limits.
    ''' </remarks>
    ''' <value>
    ''' <c>null</c> if Limit1 Auto Clear is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>.
    ''' </value>
    Public Property Limit1AutoClear As Boolean?
        Get
            Return Me._Limit1AutoClear
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Limit1AutoClear, value) Then
                Me._Limit1AutoClear = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Auto Clear sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function ApplyLimit1AutoClear(ByVal value As Boolean) As Boolean?
        Me.WriteLimit1AutoClear(value)
        Return Me.QueryLimit1AutoClear()
    End Function

    ''' <summary> Gets or sets the Limit1 Auto Clear query command. </summary>
    ''' <value> The Limit1 Auto Clear query command. </value>
    ''' <remarks> TSP: G_.print(_G.dmm.measure.limit1.autoclear==dmm.ON) </remarks>
    Protected Overridable ReadOnly Property Limit1AutoClearQueryCommand As String

    ''' <summary> Queries the Limit1 Auto Clear sentinel. Also sets the
    ''' <see cref="Limit1AutoClear">AutoClear</see> sentinel. </summary>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function QueryLimit1AutoClear() As Boolean?
        Me.Limit1AutoClear = Me.Query(Me.Limit1AutoClear, Me.Limit1AutoClearQueryCommand)
        Return Me.Limit1AutoClear
    End Function

    ''' <summary> Gets or sets the Limit1 Auto Clear command Format. </summary>
    ''' <value> The Limit1 Auto Clear query command. </value>
    ''' <remarks> TSP: _G.dmm.measure.limit1.autoclear={0:'dmm.ON';'dmm.ON';'dmm.OFF'}" </remarks>
    Protected Overridable ReadOnly Property Limit1AutoClearCommandFormat As String

    ''' <summary> Writes the Limit1 Auto Clear sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is Auto Clear. </param>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function WriteLimit1AutoClear(ByVal value As Boolean) As Boolean?
        Me.Limit1AutoClear = Me.Write(value, Me.Limit1AutoClearCommandFormat)
        Return Me.Limit1AutoClear
    End Function

#End Region

#Region " LIMIT1 ENABLED "

    ''' <summary> Limit1 enabled. </summary>
    Private _Limit1Enabled As Boolean?

    ''' <summary> Gets or sets the cached Limit1 Enabled sentinel. </summary>
    ''' <remarks> This command enables or disables a limit test for the selected measurement function. When this
    ''' attribute Is enabled, the limit 1 testing occurs on each measurement made by the instrument. Limit 1
    ''' testing compares the measurements To the high And low limit values. If a measurement falls outside
    ''' these limits, the test fails. </remarks>
    ''' <value> <c>null</c> if Limit1 Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Limit1Enabled As Boolean?
        Get
            Return Me._Limit1Enabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Limit1Enabled, value) Then
                Me._Limit1Enabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyLimit1Enabled(ByVal value As Boolean) As Boolean?
        Me.WriteLimit1Enabled(value)
        Return Me.QueryLimit1Enabled()
    End Function

    ''' <summary> Gets or sets the Limit1 enabled query command. </summary>
    ''' <value> The Limit1 enabled query command. </value>
    ''' <remarks> TSP G_.print(_G.dmm.measure.limit1.autoclear==dmm.ON)" </remarks>
    Protected Overridable ReadOnly Property Limit1EnabledQueryCommand As String

    ''' <summary> Queries the Limit1 Enabled sentinel. Also sets the
    ''' <see cref="Limit1Enabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryLimit1Enabled() As Boolean?
        Me.Limit1Enabled = Me.Query(Me.Limit1Enabled, Me.Limit1EnabledQueryCommand)
        Return Me.Limit1Enabled
    End Function

    ''' <summary> Gets or sets the Limit1 enabled command Format. </summary>
    ''' <value> The Limit1 enabled query command. </value>
    ''' <remarks> TSP _G.dmm.measure.limit1.enable={0:'dmm.ON';'dmm.ON';'dmm.OFF'} </remarks>
    Protected Overridable ReadOnly Property Limit1EnabledCommandFormat As String

    ''' <summary> Writes the Limit1 Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteLimit1Enabled(ByVal value As Boolean) As Boolean?
        Me.Limit1Enabled = Me.Write(value, Me.Limit1EnabledCommandFormat)
        Return Me.Limit1Enabled
    End Function

#End Region

#Region " LIMIT1 LOWER LEVEL "

    ''' <summary> The Limit1 Lower Level. </summary>
    Private _Limit1LowerLevel As Double?

    ''' <summary>
    ''' Gets or sets the cached Limit1 Lower Level. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum.
    ''' </summary>
    ''' <remarks>
    ''' This command sets the lower limit for the limit 1 test for the selected measure function.
    ''' When limit 1 testing Is enabled, this causes a fail indication to occur when the measurement
    ''' value Is less than this value.  Default Is 0.3 For limit 1 When the diode Function Is
    ''' selected. The Default For limit 2 For the diode Function is() –1.
    ''' </remarks>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Limit1LowerLevel As Double?
        Get
            Return Me._Limit1LowerLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit1LowerLevel, value) Then
                Me._Limit1LowerLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Lower Level. </summary>
    ''' <param name="value"> The Limit1 Lower Level. </param>
    ''' <returns> The Limit1 Lower Level. </returns>
    Public Function ApplyLimit1LowerLevel(ByVal value As Double) As Double?
        Me.WriteLimit1LowerLevel(value)
        Return Me.QueryLimit1LowerLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Lower Level query command. </summary>
    ''' <value> The Limit1 Lower Level query command. </value>
    Protected Overridable ReadOnly Property Limit1LowerLevelQueryCommand As String

    ''' <summary> Queries The Limit1 Lower Level. </summary>
    ''' <returns> The Limit1 Lower Level or none if unknown. </returns>
    Public Function QueryLimit1LowerLevel() As Double?
        Me.Limit1LowerLevel = Me.Query(Me.Limit1LowerLevel, Me.Limit1LowerLevelQueryCommand)
        Return Me.Limit1LowerLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Lower Level command format. </summary>
    ''' <value> The Limit1 Lower Level command format. </value>
    Protected Overridable ReadOnly Property Limit1LowerLevelCommandFormat As String

    ''' <summary> Writes The Limit1 Lower Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Limit1 Lower Level. </remarks>
    ''' <param name="value"> The Limit1 Lower Level. </param>
    ''' <returns> The Limit1 Lower Level. </returns>
    Public Function WriteLimit1LowerLevel(ByVal value As Double) As Double?
        Me.Limit1LowerLevel = Me.Write(value, Me.Limit1LowerLevelCommandFormat)
        Return Me.Limit1LowerLevel
    End Function

#End Region

#Region " LIMIT1 UPPER LEVEL "

    ''' <summary> The Limit1 Upper Level. </summary>
    Private _Limit1UpperLevel As Double?

    ''' <summary>
    ''' Gets or sets the cached Limit1 Upper Level. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum.
    ''' </summary>
    ''' <remarks>
    ''' This command sets the high limit for the limit 2 test for the selected measurement function.
    ''' When limit 2 testing Is enabled, the instrument generates a fail indication When the
    ''' measurement value Is more than this value. Default Is 0.8 For limit 1 When the diode Function
    ''' Is selected; 10 When the continuity Function Is selected. The default for limit 2 for the
    ''' diode And continuity functions Is 1.
    ''' </remarks>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Limit1UpperLevel As Double?
        Get
            Return Me._Limit1UpperLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit1UpperLevel, value) Then
                Me._Limit1UpperLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Upper Level. </summary>
    ''' <param name="value"> The Limit1 Upper Level. </param>
    ''' <returns> The Limit1 Upper Level. </returns>
    Public Function ApplyLimit1UpperLevel(ByVal value As Double) As Double?
        Me.WriteLimit1UpperLevel(value)
        Return Me.QueryLimit1UpperLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Upper Level query command. </summary>
    ''' <value> The Limit1 Upper Level query command. </value>
    Protected Overridable ReadOnly Property Limit1UpperLevelQueryCommand As String

    ''' <summary> Queries The Limit1 Upper Level. </summary>
    ''' <returns> The Limit1 Upper Level or none if unknown. </returns>
    Public Function QueryLimit1UpperLevel() As Double?
        Me.Limit1UpperLevel = Me.Query(Me.Limit1UpperLevel, Me.Limit1UpperLevelQueryCommand)
        Return Me.Limit1UpperLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Upper Level command format. </summary>
    ''' <value> The Limit1 Upper Level command format. </value>
    Protected Overridable ReadOnly Property Limit1UpperLevelCommandFormat As String

    ''' <summary> Writes The Limit1 Upper Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Limit1 Upper Level. </remarks>
    ''' <param name="value"> The Limit1 Upper Level. </param>
    ''' <returns> The Limit1 Upper Level. </returns>
    Public Function WriteLimit1UpperLevel(ByVal value As Double) As Double?
        Me.Limit1UpperLevel = Me.Write(value, Me.Limit1UpperLevelCommandFormat)
        Return Me.Limit1UpperLevel
    End Function

#End Region

#End Region

#Region " LIMIT 2 "

#Region " LIMIT2 AUTO CLEAR "

    ''' <summary> Limit2 Auto Clear. </summary>
    Private _Limit2AutoClear As Boolean?

    ''' <summary> Gets or sets the cached Limit2 Auto Clear sentinel. </summary>
    ''' <remarks>
    ''' When auto clear is set to on for a measure function, limit conditions are cleared
    ''' automatically after each measurement. If you are making a series of measurements, the
    ''' instrument shows the limit test result of the last measurement for the pass Or fail
    ''' indication for the limit. If you want To know If any Of a series Of measurements failed the
    ''' limit, Set the auto clear setting To off. When this set to off, a failed indication Is Not
    ''' cleared automatically. It remains set until it Is cleared With the clear command. The auto
    ''' clear setting affects both the high And low limits.
    ''' </remarks>
    ''' <value> <c>null</c> if Limit2 Auto Clear is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Limit2AutoClear As Boolean?
        Get
            Return Me._Limit2AutoClear
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Limit2AutoClear, value) Then
                Me._Limit2AutoClear = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit2 Auto Clear sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function ApplyLimit2AutoClear(ByVal value As Boolean) As Boolean?
        Me.WriteLimit2AutoClear(value)
        Return Me.QueryLimit2AutoClear()
    End Function

    ''' <summary> Gets or sets the Limit2 Auto Clear query command. </summary>
    ''' <value> The Limit2 Auto Clear query command. </value>
    ''' <remarks> TSP: G_.print(_G.dmm.measure.limit1.autoclear==dmm.ON) </remarks>
    Protected Overridable ReadOnly Property Limit2AutoClearQueryCommand As String

    ''' <summary> Queries the Limit2 Auto Clear sentinel. Also sets the
    ''' <see cref="Limit2AutoClear">AutoClear</see> sentinel. </summary>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function QueryLimit2AutoClear() As Boolean?
        Me.Limit2AutoClear = Me.Query(Me.Limit2AutoClear, Me.Limit2AutoClearQueryCommand)
        Return Me.Limit2AutoClear
    End Function

    ''' <summary> Gets or sets the Limit2 Auto Clear command Format. </summary>
    ''' <value> The Limit2 Auto Clear query command. </value>
    ''' <remarks> TSP: "_G.dmm.measure.limit1.autoclear={0:'dmm.ON';'dmm.ON';'dmm.OFF'}" </remarks>
    Protected Overridable ReadOnly Property Limit2AutoClearCommandFormat As String

    ''' <summary> Writes the Limit2 Auto Clear sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is Auto Clear. </param>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function WriteLimit2AutoClear(ByVal value As Boolean) As Boolean?
        Me.Limit2AutoClear = Me.Write(value, Me.Limit2AutoClearCommandFormat)
        Return Me.Limit2AutoClear
    End Function

#End Region

#Region " LIMIT2 ENABLED "

    ''' <summary> Limit2 enabled. </summary>
    Private _Limit2Enabled As Boolean?

    ''' <summary> Gets or sets the cached Limit2 Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Limit2 Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    ''' <remarks> This command enables or disables a limit test for the selected measurement function. When this
    ''' attribute Is enabled, the limit 2 testing occurs on each measurement made by the instrument. Limit 2
    ''' testing compares the measurements To the high And low limit values. If a measurement falls outside
    ''' these limits, the test fails. </remarks>
    Public Property Limit2Enabled As Boolean?
        Get
            Return Me._Limit2Enabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Limit2Enabled, value) Then
                Me._Limit2Enabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit2 Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyLimit2Enabled(ByVal value As Boolean) As Boolean?
        Me.WriteLimit2Enabled(value)
        Return Me.QueryLimit2Enabled()
    End Function

    ''' <summary> Gets or sets the Limit2 enabled query command. </summary>
    ''' <value> The Limit2 enabled query command. </value>
    ''' <remarks> TSP G_.print(_G.dmm.measure.limit2.autoclear==dmm.ON)" </remarks>
    Protected Overridable ReadOnly Property Limit2EnabledQueryCommand As String

    ''' <summary> Queries the Limit2 Enabled sentinel. Also sets the
    ''' <see cref="Limit2Enabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryLimit2Enabled() As Boolean?
        Me.Limit2Enabled = Me.Query(Me.Limit2Enabled, Me.Limit2EnabledQueryCommand)
        Return Me.Limit2Enabled
    End Function

    ''' <summary> Gets or sets the Limit2 enabled command Format. </summary>
    ''' <value> The Limit2 enabled query command. </value>
    ''' <remarks> TSP: _G.dmm.measure.limit2.enable={0:'dmm.ON';'dmm.ON';'dmm.OFF'} </remarks>
    Protected Overridable ReadOnly Property Limit2EnabledCommandFormat As String

    ''' <summary> Writes the Limit2 Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteLimit2Enabled(ByVal value As Boolean) As Boolean?
        Me.Limit2Enabled = Me.Write(value, Me.Limit2EnabledCommandFormat)
        Return Me.Limit2Enabled
    End Function

#End Region

#Region " LIMIT2 LOWER LEVEL "

    ''' <summary> The Limit2 Lower Level. </summary>
    Private _Limit2LowerLevel As Double?

    ''' <summary>
    ''' Gets or sets the cached Limit2 Lower Level. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum.
    ''' </summary>
    ''' <remarks>
    ''' This command sets the lower limit for the limit 1 test for the selected measure function.
    ''' When limit 1 testing Is enabled, this causes a fail indication to occur when the measurement
    ''' value Is less than this value.  Default Is 0.3 For limit 1 When the diode Function Is
    ''' selected. The Default For limit 2 For the diode Function is() –1.
    ''' </remarks>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Limit2LowerLevel As Double?
        Get
            Return Me._Limit2LowerLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit2LowerLevel, value) Then
                Me._Limit2LowerLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit2 Lower Level. </summary>
    ''' <param name="value"> The Limit2 Lower Level. </param>
    ''' <returns> The Limit2 Lower Level. </returns>
    Public Function ApplyLimit2LowerLevel(ByVal value As Double) As Double?
        Me.WriteLimit2LowerLevel(value)
        Return Me.QueryLimit2LowerLevel
    End Function

    ''' <summary> Gets or sets The Limit2 Lower Level query command. </summary>
    ''' <value> The Limit2 Lower Level query command. </value>
    Protected Overridable ReadOnly Property Limit2LowerLevelQueryCommand As String

    ''' <summary> Queries The Limit2 Lower Level. </summary>
    ''' <returns> The Limit2 Lower Level or none if unknown. </returns>
    Public Function QueryLimit2LowerLevel() As Double?
        Me.Limit2LowerLevel = Me.Query(Me.Limit2LowerLevel, Me.Limit2LowerLevelQueryCommand)
        Return Me.Limit2LowerLevel
    End Function

    ''' <summary> Gets or sets The Limit2 Lower Level command format. </summary>
    ''' <value> The Limit2 Lower Level command format. </value>
    Protected Overridable ReadOnly Property Limit2LowerLevelCommandFormat As String

    ''' <summary> Writes The Limit2 Lower Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Limit2 Lower Level. </remarks>
    ''' <param name="value"> The Limit2 Lower Level. </param>
    ''' <returns> The Limit2 Lower Level. </returns>
    Public Function WriteLimit2LowerLevel(ByVal value As Double) As Double?
        Me.Limit2LowerLevel = Me.Write(value, Me.Limit2LowerLevelCommandFormat)
        Return Me.Limit2LowerLevel
    End Function

#End Region

#Region " LIMIT2 UPPER LEVEL "

    ''' <summary> The Limit2 Upper Level. </summary>
    Private _Limit2UpperLevel As Double?

    ''' <summary> Gets or sets the cached Limit2 Upper Level. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    ''' <remarks> This command sets the high limit for the limit 2 test for the selected measurement function. When limit
    ''' 2 testing Is enabled, the instrument generates a fail indication When the measurement value Is more
    ''' than this value.
    ''' Default Is 0.8 For limit 1 When the diode Function Is selected; 10 When the continuity Function Is
    ''' selected. The default for limit 2 for the diode And continuity functions Is 1</remarks>
    Public Overloads Property Limit2UpperLevel As Double?
        Get
            Return Me._Limit2UpperLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit2UpperLevel, value) Then
                Me._Limit2UpperLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit2 Upper Level. </summary>
    ''' <param name="value"> The Limit2 Upper Level. </param>
    ''' <returns> The Limit2 Upper Level. </returns>
    Public Function ApplyLimit2UpperLevel(ByVal value As Double) As Double?
        Me.WriteLimit2UpperLevel(value)
        Return Me.QueryLimit2UpperLevel
    End Function

    ''' <summary> Gets or sets The Limit2 Upper Level query command. </summary>
    ''' <value> The Limit2 Upper Level query command. </value>
    Protected Overridable ReadOnly Property Limit2UpperLevelQueryCommand As String

    ''' <summary> Queries The Limit2 Upper Level. </summary>
    ''' <returns> The Limit2 Upper Level or none if unknown. </returns>
    Public Function QueryLimit2UpperLevel() As Double?
        Me.Limit2UpperLevel = Me.Query(Me.Limit2UpperLevel, Me.Limit2UpperLevelQueryCommand)
        Return Me.Limit2UpperLevel
    End Function

    ''' <summary> Gets or sets The Limit2 Upper Level command format. </summary>
    ''' <value> The Limit2 Upper Level command format. </value>
    Protected Overridable ReadOnly Property Limit2UpperLevelCommandFormat As String

    ''' <summary> Writes The Limit2 Upper Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Limit2 Upper Level. </remarks>
    ''' <param name="value"> The Limit2 Upper Level. </param>
    ''' <returns> The Limit2 Upper Level. </returns>
    Public Function WriteLimit2UpperLevel(ByVal value As Double) As Double?
        Me.Limit2UpperLevel = Me.Write(value, Me.Limit2UpperLevelCommandFormat)
        Return Me.Limit2UpperLevel
    End Function

#End Region

#End Region

#Region " OPEN DETECTOR ENABLED "

    ''' <summary> Gets or sets a list of states of the open detector knowns. </summary>
    ''' <value> The open detector known states. </value>
    Public ReadOnly Property OpenDetectorKnownStates As MultimeterFunctionEnabledDictionary

    ''' <summary> Open Detector enabled. </summary>
    Private _OpenDetectorEnabled As Boolean?

    ''' <summary> Gets or sets the cached Open Detector Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Open Detector Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property OpenDetectorEnabled As Boolean?
        Get
            Return Me._OpenDetectorEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.OpenDetectorEnabled, value) Then
                Me._OpenDetectorEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Open Detector Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyOpenDetectorEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteOpenDetectorEnabled(value)
        Return Me.QueryOpenDetectorEnabled()
    End Function

    ''' <summary> Gets or sets the automatic Zero enabled query command. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    ''' <remarks> TSP: _G.print(_G.dmm.opendetector==1) </remarks>
    Protected Overridable ReadOnly Property OpenDetectorEnabledQueryCommand As String

    ''' <summary> Queries the Open Detector Enabled sentinel. Also sets the
    ''' <see cref="OpenDetectorEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryOpenDetectorEnabled() As Boolean?
        Me.OpenDetectorEnabled = Me.Query(Me.OpenDetectorEnabled, Me.OpenDetectorEnabledQueryCommand)
        Return Me.OpenDetectorEnabled
    End Function

    ''' <summary> Gets or sets the automatic Zero enabled command Format. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    ''' <remarks> TSP: _G.opendetector={0:'1';'1';'0'}" </remarks>
    Protected Overridable ReadOnly Property OpenDetectorEnabledCommandFormat As String

    ''' <summary> Writes the Open Detector Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteOpenDetectorEnabled(ByVal value As Boolean) As Boolean?
        Me.OpenDetectorEnabled = Me.Write(value, Me.OpenDetectorEnabledCommandFormat)
        Return Me.OpenDetectorEnabled
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
    ''' <summary> The power line cycles range in units. </summary>
    Public Property PowerLineCyclesRange As Core.Pith.RangeR
        Get
            Return Me._PowerLineCyclesRange
        End Get
        Set(value As Core.Pith.RangeR)
            If Me.PowerLineCyclesRange <> value Then
                Me._PowerLineCyclesRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> The Power Line Cycles. </summary>
    Private _PowerLineCycles As Double?

    ''' <summary> Gets or sets the cached Multimeter PowerLineCycles. </summary>
    ''' <remarks>
    ''' This command sets the amount of time that the input signal is measured. The amount of time is
    ''' specified as the number of power line cycles (NPLCs). Each PLC for 60 Hz Is 16.67 ms (1/60)
    ''' And each PLC for 50 Hz Is 20 ms (1/50). For 60 Hz, if you set the NPLC to 0.1, the time Is
    ''' 1.667 ms. The shortest amount Of time results In the fastest reading rate, but increases the
    ''' reading noise And decreases the number Of usable digits. The longest amount Of time provides
    ''' the lowest reading noise And more usable digits, but has the slowest reading rate. Settings
    ''' between the fastest And slowest number Of PLCs are a compromise between speed And noise. If
    ''' you change the PLCs, you may want To adjust the displayed digits To reflect the change In
    ''' usable digits.
    ''' </remarks>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property PowerLineCycles As Double?
        Get
            Return Me._PowerLineCycles
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.PowerLineCycles, value) Then
                Me._PowerLineCycles = value
                Me._Aperture = StatusSubsystemBase.FromPowerLineCycles(Me._PowerLineCycles.Value).TotalSeconds
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Multimeter PowerLineCycles. </summary>
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
        value = If(value > PowerLineCyclesRange.Max, PowerLineCyclesRange.Max, If(value < PowerLineCyclesRange.Min, PowerLineCyclesRange.Min, value))
        Me.PowerLineCycles = Me.Write(value, Me.PowerLineCyclesCommandFormat)
        Return Me.PowerLineCycles
    End Function

#End Region

#Region " RANGE "

    ''' <summary> The Range. </summary>
    Private _Range As Double?

    ''' <summary>
    ''' Gets or sets the cached Multimeter Range. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum.
    ''' </summary>
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

    ''' <summary> Writes and reads back the Multimeter Range. </summary>
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
        value = If(value > FunctionRange.Max, FunctionRange.Max, If(value < FunctionRange.Min, FunctionRange.Min, value))
        Me.Range = Me.Write(value, Me.RangeCommandFormat)
        Return Me.Range
    End Function

#End Region

#Region " READ "

    ''' <summary> Gets or sets the amount. </summary>
    ''' <value> The amount. </value>
    Public ReadOnly Property Amount As MeasuredAmount

    ''' <summary> The Reading. </summary>
    Private _Reading As Double?

    ''' <summary> Gets or sets the cached Multimeter Reading. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Reading As Double?
        Get
            Return Me._Reading
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Reading, value) Then
                Me._Reading = value
                If value.HasValue Then
                    Me.Amount.Value = value
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the read buffer query command format. </summary>
    ''' <value> The read buffer query command format. </value>
    Protected Overridable ReadOnly Property ReadBufferQueryCommandFormat As String

    ''' <summary> Gets or sets the read query command. </summary>
    ''' <value> The read query command. </value>
    Protected Overridable ReadOnly Property ReadQueryCommand As String

    ''' <summary> Queries The reading. </summary>
    ''' <remarks>
    ''' This command initiates measurements using the present function setting, stores the readings
    ''' in a reading buffer, and returns the last reading. The dmm.measure.count attribute determines
    ''' how many measurements are performed. When you use a reading buffer with a command Or action
    ''' that makes multiple readings, all readings are available In the reading buffer. However, only
    ''' the last reading Is returned As a reading With the command. If you define a specific reading
    ''' buffer, the reading buffer must exist before you make the measurement.
    ''' </remarks>
    ''' <returns> The reading or none if unknown. </returns>
    Public Function Measure() As Double?
        Me.Reading = Me.Query(Me.Reading, Me.ReadQueryCommand)
        Return Me.Reading
    End Function

    ''' <summary> Queries The reading. </summary>
    ''' <remarks>
    ''' This command initiates measurements using the present function setting, stores the readings
    ''' in a reading buffer, and returns the last reading. The dmm.measure.count attribute determines
    ''' how many measurements are performed. When you use a reading buffer with a command Or action
    ''' that makes multiple readings, all readings are available In the reading buffer. However, only
    ''' the last reading Is returned As a reading With the command. If you define a specific reading
    ''' buffer, the reading buffer must exist before you make the measurement.
    ''' </remarks>
    ''' <param name="bufferName"> Name of the buffer. </param>
    ''' <returns> The reading or none if unknown. </returns>
    Public Function Measure(ByVal bufferName As String) As Double?
        Me.Reading = Me.Query(Me.Reading, String.Format(Me.ReadBufferQueryCommandFormat, bufferName))
        Return Me.Reading
    End Function

#End Region


End Class

''' <summary> Specifies the function modes. </summary>
Public Enum MultimeterFunctionMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("DC Voltage (dmm.FUNC_DC_VOLTAGE)")> VoltageDC
    <ComponentModel.Description("AC Voltage (FUNC_AC_VOLTAGE)")> VoltageAC
    <ComponentModel.Description("DC Current (dmm.FUNC_DC_CURRENT)")> CurrentDC
    <ComponentModel.Description("AC Current (dmm.FUNC_AC_CURRENT)")> CurrentAC
    <ComponentModel.Description("Temperature (dmm.FUNC_TEMPERATURE)")> Temperature
    <ComponentModel.Description("Resistance 2-Wire (dmm.FUNC_RESISTANCE)")> ResistanceTwoWire
    <ComponentModel.Description("Resistance 4-Wire (dmm.FUNC_4W_RESISTANCE)")> ResistanceFourWire
    <ComponentModel.Description("Temperature (dmm.FUNC_DIODE)")> Diode
    <ComponentModel.Description("Temperature (dmm.FUNC_CAPACITANCE)")> Capacitance
    <ComponentModel.Description("Temperature (dmm.FUNC_CONTINUITY)")> Continuity
    <ComponentModel.Description("Temperature (dmm.FUNC_ACV_FREQUENCY)")> Frequency
    <ComponentModel.Description("Temperature (dmm.FUNC_ACV_PERIOD)")> Period
    <ComponentModel.Description("Temperature (dmm.FUNC_DCV_RATIO)")> Ratio
End Enum

''' <summary> Specifies the units. </summary>
Public Enum MultimeterMeasurementUnit
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Volt (dmm.UNIT_VOLT)")> Volt
    <ComponentModel.Description("Decibel (dmm.UNIT_DB)")> Decibel
    <ComponentModel.Description("Celsius (dmm.UNIT_CELCIUS)")> Celsius
    <ComponentModel.Description("Kelvin (dmm.UNIT_KELVIN)")> Kelvin
    <ComponentModel.Description("Fahrenheit (dmm.UNIT_FAHRENHEIT)")> Fahrenheit
End Enum


''' <summary> Dictionary of function ranges. </summary>
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
Public Class FunctionRangeDictionary
    Inherits Collections.Generic.Dictionary(Of MultimeterFunctionMode, Core.Pith.RangeR)
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
Public Class MultimeterFunctionEnabledDictionary
    Inherits Collections.Generic.Dictionary(Of MultimeterFunctionMode, Boolean)
End Class


#Region " UNUSED "
#If False Then
#Region " CONNECT/DISCONNECT "

    Protected Overridable ReadOnly Property ConnectCommand As String

    ''' <summary> Connects this object. </summary>
    Public Sub Connect()
        Me.Execute(Me.ConnectCommand)
    End Sub

    Protected Overridable ReadOnly Property DisconnectCommand As String

    ''' <summary> Disconnects this object. </summary>
    Public Sub Disconnect()
        Me.Execute(DisconnectCommand)
    End Sub

#End Region


#End If
#End Region