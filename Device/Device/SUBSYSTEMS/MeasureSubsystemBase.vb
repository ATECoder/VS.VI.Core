Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a Measure Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class MeasureSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="MeasureSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.LastReading = ""
        Me._Amount = New MeasuredAmount(ReadingTypes.Reading)
        Me.MeasuredValue = New Double?
        Me._FunctionModeRanges = New RangeDictionary
        Me._FunctionModeDecimalPlaces = New IntegerDictionary
        Me._FunctionModeUnits = New UnitDictionary
        Me._OpenDetectorKnownStates = New BooleanDictionary
        Me.ApertureRange = Core.Pith.RangeR.FullNonnegative
        Me.FilterCountRange = Core.Pith.RangeI.FullNonnegative
        Me.FilterWindowRange = Core.Pith.RangeR.FullNonnegative
        Me.PowerLineCyclesRange = Core.Pith.RangeR.FullNonnegative
        Me.FunctionUnit = Me.DefaultFunctionUnit
        Me.FunctionRange = Me.DefaultFunctionRange
        Me.FunctionRangeDecimalPlaces = Me.DefaultFunctionModeDecimalPlaces
    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Clears last reading.
    '''           </para></remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.LastReading = ""
        Me._Amount = New MeasuredAmount(ReadingTypes.Reading)
        Me.MeasuredValue = New Double?
    End Sub



#End Region

#Region "  INIT, READ, FETCH, MEASURE "

    ''' <summary> Gets the fetch command. </summary>
    ''' <value> The fetch command. </value>
    ''' <remarks> SCPI: 'FETCh?' </remarks>
    Protected Overridable ReadOnly Property FetchCommand As String

    ''' <summary> Fetches the data. </summary>
    ''' <remarks> Issues the 'FETCH?' query, which reads data stored in the Sample Buffer. If, for example, there
    ''' are 20 data arrays stored in the Sample Buffer, then all 20 data arrays will be sent to the
    ''' computer when 'FETCh?' is executed. Note that FETCh? does not affect data in the Sample Buffer.
    ''' Thus, subsequent executions of FETCh? acquire the same data. </remarks>
    Public Overridable Function Fetch() As Double?
        Return Me.Measure(Me.FetchCommand)
    End Function

    ''' <summary> Gets the read command. </summary>
    ''' <value> The read command. </value>
    ''' <remarks> SCPI: 'READ' </remarks>
    Protected Overridable ReadOnly Property ReadCommand As String

    ''' <summary> Initiates an operation and then fetches the data. </summary>
    ''' <remarks> Issues the 'READ?' query, which performs a trigger initiation and then a
    ''' <see cref="Fetch">FETCh? </see>
    ''' The initiate triggers a new measurement cycle which puts new data in the Sample Buffer. Fetch
    ''' reads that new data. The <see cref="MeasureCurrentSubsystemBase.Measure">Measure</see> command
    ''' places the instrument in a “one-shot” mode and then performs a read. </remarks>
    Public Overridable Function Read() As Double?
        Return Me.Measure(Me.ReadCommand)
    End Function

    ''' <summary> Gets or sets The Measure query command. </summary>
    ''' <value> The Measure query command. </value>
    Protected Overridable ReadOnly Property MeasureQueryCommand As String

    ''' <summary> Queries The reading. </summary>
    ''' <returns> The reading or none if unknown. </returns>
    Public Overridable Function Measure() As Double?
        Return Me.Measure(Me.MeasureQueryCommand)
    End Function

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

    ''' <summary>
    ''' Gets or sets the cached sense Aperture. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum.
    ''' </summary>
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
                    Me.PowerLineCycles = Me.Aperture.Value * Me.StatusSubsystem.LineFrequency
                    ' Me.PowerLineCycles = Me.Aperture.Value * StatusSubsystemBase.StationLineFrequency
                Else
                    Me.PowerLineCycles = New Double?
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the sense Aperture. </summary>
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

    Protected Overridable ReadOnly Property AutoZeroOnceCommand As String

    ''' <summary> Request a single auto zero. </summary>
    Public Sub AutoZeroOnce()
        Me.Session.Execute(Me.AutoZeroOnceCommand)
    End Sub

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

    ''' <summary> Gets or sets the cached sense Filter Count. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
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

    ''' <summary> Writes and reads back the sense Filter Count. </summary>
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
    ''' <remarks> TSP: _G.print(dmm.filter.enable==1) </remarks>
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
    ''' <remarks> TSP: _G.print(dmm.filter.type=0) </remarks>
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

    ''' <summary> Gets or sets the cached sense Filter Window. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
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

    ''' <summary> Writes and reads back the sense Filter Window. </summary>
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

    ''' <summary> Gets or sets the front terminals selected query command. </summary>
    ''' <value> The front terminals selected query command. </value>
    Protected Overridable ReadOnly Property FrontTerminalsSelectedQueryCommand As String

    ''' <summary> Queries the Front Terminals Selected sentinel. Also sets the
    ''' <see cref="FrontTerminalsSelected">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryFrontTerminalsSelected() As Boolean?
        Me.FrontTerminalsSelected = Me.Query(Me.FrontTerminalsSelected, Me.FrontTerminalsSelectedQueryCommand)
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
    ''' <remarks> TSP: _G.print(_G.dmm.measure.limit1.autoclear==dmm.ON) </remarks>
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
    ''' <remarks> TSP: "_G.dmm.measure.limit1.autoclear={0:'dmm.ON';'dmm.ON';'dmm.OFF'}" </remarks>
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
    ''' <remarks> TSP: _G.print(_G.dmm.measure.limit1.autoclear==dmm.ON) </remarks>
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
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum.
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
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum.
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
    ''' <remarks> TSP: _G.print(_G.dmm.measure.limit1.autoclear==dmm.ON) </remarks>
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
    ''' <remarks> TSP: _G.print(_G.dmm.measure.limit2.autoclear==dmm.ON) </remarks>
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
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum.
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
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
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

#Region " MEASURE UNIT "

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

#End Region

#Region " OPEN DETECTOR ENABLED "

    ''' <summary> Gets or sets a list of states of the open detector knowns. </summary>
    ''' <value> The open detector known states. </value>
    Public ReadOnly Property OpenDetectorKnownStates As BooleanDictionary

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

    ''' <summary> Gets or sets the cached sense PowerLineCycles. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property PowerLineCycles As Double?
        Get
            Return Me._PowerLineCycles
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.PowerLineCycles, value) Then
                Me._PowerLineCycles = value
                Me.Aperture = StatusSubsystemBase.FromPowerLineCycles(Me._PowerLineCycles.Value).TotalSeconds
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
        value = If(value > FunctionRange.Max, FunctionRange.Max, If(value < FunctionRange.Min, FunctionRange.Min, value))
        Me.Range = Me.Write(value, Me.RangeCommandFormat)
        Return Me.Range
    End Function

#End Region

#Region " REMOTE SENSE SELECTED "

    ''' <summary> Remote Sense Selected. </summary>
    Private _RemoteSenseSelected As Boolean?

    ''' <summary> Gets or sets the cached Remote Sense Selected sentinel. </summary>
    ''' <value> <c>null</c> if Remote Sense Selected is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property RemoteSenseSelected As Boolean?
        Get
            Return Me._RemoteSenseSelected
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.RemoteSenseSelected, value) Then
                Me._RemoteSenseSelected = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Remote Sense Selected sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyRemoteSenseSelected(ByVal value As Boolean) As Boolean?
        Me.WriteRemoteSenseSelected(value)
        Return Me.QueryRemoteSenseSelected()
    End Function

    ''' <summary> Gets or sets the remote sense selected query command. </summary>
    ''' <value> The remote sense selected query command. </value>
    Protected Overridable ReadOnly Property RemoteSenseSelectedQueryCommand As String

    ''' <summary> Queries the Remote Sense Selected sentinel. Also sets the
    ''' <see cref="RemoteSenseSelected">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryRemoteSenseSelected() As Boolean?
        Me.RemoteSenseSelected = Me.Query(Me.RemoteSenseSelected, Me.RemoteSenseSelectedQueryCommand)
        Return Me.RemoteSenseSelected
    End Function

    ''' <summary> Gets or sets the remote sense selected command format. </summary>
    ''' <value> The remote sense selected command format. </value>
    Protected Overridable ReadOnly Property RemoteSenseSelectedCommandFormat As String

    ''' <summary> Writes the Remote Sense Selected sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteRemoteSenseSelected(ByVal value As Boolean) As Boolean?
        Me.RemoteSenseSelected = Me.Write(value, Me.RemoteSenseSelectedCommandFormat)
        Return Me.RemoteSenseSelected
    End Function

#End Region

End Class

