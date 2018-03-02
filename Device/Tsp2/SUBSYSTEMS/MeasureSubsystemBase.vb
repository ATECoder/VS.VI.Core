Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary>  Defines the contract that must be implemented by a Source Measure Unit Measure Subsystem. </summary>
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
    Inherits SourceMeasureUnitBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="MeasureSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.Tsp2.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.Tsp2.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._Amount = New MeasuredAmount(ReadingTypes.Reading)
        Me._FunctionModeRanges = New MeasureFunctionRangeDictionary
        For Each fm As MeasureFunctionMode In [Enum].GetValues(GetType(MeasureFunctionMode))
            Me._FunctionModeRanges.Add(fm, New Core.Pith.RangeR(0, 1))
        Next
        Me.ApertureRange = Core.Pith.RangeR.FullNonNegative
        Me.FilterCountRange = Core.Pith.RangeI.FullNonNegative
        Me.FilterWindowRange = Core.Pith.RangeR.FullNonNegative
        Me.PowerLineCyclesRange = Core.Pith.RangeR.FullNonNegative
        Me.FunctionUnit = Me.DefaultFunctionUnit
        Me.FunctionRange = Me.DefaultFunctionRange
        Me.FunctionRangeDecimalPlaces = Me.DefaultFunctionModeDecimalPlaces
        Me.DefaultMeasurementUnit = Arebis.StandardUnits.ElectricUnits.Volt
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.AutoRangeState = OnOffState.On
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

    ''' <summary> Gets or sets the cached sense Aperture. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
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

    ''' <summary> Gets or sets the automatic Zero enabled query print command. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overridable ReadOnly Property AutoZeroEnabledPrintCommand As String

    ''' <summary> Queries the Auto Zero Enabled sentinel. Also sets the
    ''' <see cref="AutoZeroEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoZeroEnabled() As Boolean?
        Me.AutoZeroEnabled = Me.Query(Me.AutoZeroEnabled, Me.AutoZeroEnabledPrintCommand)
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

    ''' <summary> Gets or sets the cached sense Filter Window. Set to
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
    Private _FunctionMode As MeasureFunctionMode?

    ''' <summary> Writes and reads back the Measure Function Mode. </summary>
    ''' <param name="value"> The  Measure Function Mode. </param>
    ''' <returns> The <see cref="MeasureFunctionMode">Measure Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As MeasureFunctionMode) As MeasureFunctionMode?
        Me.WriteFunctionMode(value)
        ' changing the function mode changes range, auto delay mode and open detector enabled. 
        Me.QueryRange()
        Me.QueryOpenDetectorEnabled()
        Return Me.QueryFunctionMode
    End Function

    ''' <summary> Gets or sets the cached function mode. </summary>
    ''' <value> The <see cref="MeasureFunctionMode">Measure Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FunctionMode As MeasureFunctionMode?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As MeasureFunctionMode?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                If value.HasValue Then
                    Me.FunctionUnit = Me.ToUnit(value.Value)
                    Me.FunctionRange = Me.ToRange(value.Value)
                    Me.FunctionRangeDecimalPlaces = Me.ToDecimalPlaces(value.Value)
                    Me._FunctionRange = Me.FunctionModeRanges(value.Value)
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command. </value>
    Protected Overridable ReadOnly Property FunctionModeQueryCommand As String = "_G.smu.measure.func"

    ''' <summary> Queries the Measure Function Mode. </summary>
    ''' <returns> The <see cref="MeasureFunctionMode">Measure Function Mode</see> or none if unknown. </returns>
    Public Overridable Function QueryFunctionMode() As MeasureFunctionMode?
        Dim mode As String = Me.FunctionMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.FunctionModeQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(MeasureSubsystemBase)}.{NameOf(FunctionMode)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.FunctionMode = New MeasureFunctionMode?
        Else
            Dim se As New StringEnumerator(Of MeasureFunctionMode)
            Me.FunctionMode = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.FunctionMode
    End Function

    ''' <summary> Gets or sets the function mode command format. </summary>
    ''' <value> The function mode command format. </value>
    Protected Overridable ReadOnly Property FunctionModeCommandFormat As String = "_G.smu.measure.func={0}"


    ''' <summary> Writes the Measure Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="MeasureFunctionMode">Measure Function Mode</see> or none if unknown. </returns>
    Public Overridable Function WriteFunctionMode(ByVal value As MeasureFunctionMode) As MeasureFunctionMode?
        Me.Session.WriteLine(Me.FunctionModeCommandFormat, value.ExtractBetween())
        ' check on the measure unit.
        Me.QueryMeasureUnit()
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

#End Region

#Region " FUNCTION MODE RANGE "

    ''' <summary> Gets or sets the function mode ranges. </summary>
    ''' <value> The function mode ranges. </value>
    Public ReadOnly Property FunctionModeRanges As MeasureFunctionRangeDictionary

    ''' <summary> Gets or sets the default function range. </summary>
    ''' <value> The default function range. </value>
    Public Property DefaultFunctionRange As isr.Core.Pith.RangeR = isr.Core.Pith.RangeR.Full

    ''' <summary> Converts a functionMode to a range. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an isr.Core.Pith.RangeR. </returns>
    Public Overridable Function ToRange(ByVal functionMode As MeasureFunctionMode) As isr.Core.Pith.RangeR
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
    Public Overridable Function ToDecimalPlaces(ByVal functionMode As MeasureFunctionMode) As Integer
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

    ''' <summary> Gets or sets the default unit. </summary>
    ''' <value> The default unit. </value>
    Public Property DefaultFunctionUnit As Arebis.TypedUnits.Unit = Arebis.StandardUnits.ElectricUnits.Volt

    ''' <summary> Parse units. </summary>
    ''' <param name="functionMode"> The  Multimeter Function Mode. </param>
    ''' <returns> An Arebis.TypedUnits.Unit. </returns>
    Public Overridable Function ToUnit(ByVal functionMode As MeasureFunctionMode) As Arebis.TypedUnits.Unit
        Dim result As Arebis.TypedUnits.Unit = Me.DefaultFunctionUnit
        Select Case functionMode
            Case MeasureFunctionMode.CurrentDC
                result = Arebis.StandardUnits.ElectricUnits.Ampere
            Case MeasureFunctionMode.VoltageDC
                result = Arebis.StandardUnits.ElectricUnits.Volt
            Case MeasureFunctionMode.Resistance
                result = Arebis.StandardUnits.ElectricUnits.Ohm
        End Select
        Return result
    End Function

    ''' <summary> Gets or sets the function mode unit. </summary>
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

#Region " MEASURE "

    Private _RawReading As String
    ''' <summary> Gets  or sets (protected) the reading.  When set, the value is converted to resistance. </summary>
    ''' <value> The reading. </value>
    Public Property RawReading() As String
        Get
            Return Me._RawReading
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.Reading) Then
                Me._RawReading = value
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the amount. </summary>
    ''' <value> The amount. </value>
    Public ReadOnly Property Amount As MeasuredAmount

    ''' <summary> The Reading. </summary>
    Private _Reading As Double?

    ''' <summary> Gets or sets the cached sense Reading. Set to
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

    ''' <summary> Gets or sets The Measure query command. </summary>
    ''' <value> The Measure query command. </value>
    Protected Overridable ReadOnly Property MeasureQueryCommand As String

    ''' <summary> Queries The reading. </summary>
    ''' <returns> The reading or none if unknown. </returns>
    Public Function Measure() As Double?
        Me.Reading = Me.Query(Me.Reading, Me.MeasureQueryCommand)
        Return Me.Reading
    End Function

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

    ''' <summary> Converts a a measure unit to a measurement unit. </summary>
    ''' <param name="value"> The Measure Unit. </param>
    ''' <returns> Value as an Arebis.TypedUnits.Unit. </returns>
    Public Function ToMeasurementUnit(ByVal value As Tsp2.MeasureUnit) As Arebis.TypedUnits.Unit
        Dim result As Arebis.TypedUnits.Unit = Me.DefaultFunctionUnit
        Select Case value
            Case Tsp2.MeasureUnit.Ampere
                result = Arebis.StandardUnits.ElectricUnits.Ampere
            Case Tsp2.MeasureUnit.Volt
                result = Arebis.StandardUnits.ElectricUnits.Volt
            Case Tsp2.MeasureUnit.Ohm
                result = Arebis.StandardUnits.ElectricUnits.Ohm
            Case Tsp2.MeasureUnit.Watt
                result = Arebis.StandardUnits.EnergyUnits.Watt
        End Select
        Return result
    End Function

    ''' <summary> Writes and reads back the Measure Unit. </summary>
    ''' <param name="value"> The  Measure Unit. </param>
    ''' <returns> The <see cref="MeasurementUnit">Measure Unit</see> or none if unknown. </returns>
    Public Function ApplyMeasureUnit(ByVal value As MeasureUnit) As MeasureUnit?
        Me.WriteMeasureUnit(value)
        Return Me.QueryMeasureUnit
    End Function

    ''' <summary> The Unit. </summary>
    Private _MeasureUnit As MeasureUnit?

    ''' <summary> Gets or sets the cached measure Unit.  This is the actual unit for measurement. </summary>
    ''' <value> The <see cref="MeasurementUnit">Measure Unit</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property MeasureUnit As MeasureUnit?
        Get
            Return Me._MeasureUnit
        End Get
        Protected Set(ByVal value As MeasureUnit?)
            If Not Nullable.Equals(Me.MeasureUnit, value) Then
                Me._MeasureUnit = value
                If value.HasValue Then
                    Me.MeasurementUnit = Me.ToMeasurementUnit(value.Value)
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Measure Unit query command. </summary>
    ''' <value> The Unit query command. </value>
    Protected Overridable ReadOnly Property MeasureUnitQueryCommand As String = "_G.smu.measure.unit"

    ''' <summary> Queries the Measure Unit. </summary>
    ''' <returns> The <see cref="MeasureUnit">Measure Unit</see> or none if unknown. </returns>
    Public Overridable Function QueryMeasureUnit() As MeasureUnit?
        Dim mode As String = Me.MeasureUnit.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.MeasureUnitQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(MeasureSubsystemBase)}.{NameOf(MeasureUnit)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.MeasureUnit = New MeasureUnit?
        Else
            Dim se As New StringEnumerator(Of MeasureUnit)
            Me.MeasureUnit = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.MeasureUnit
    End Function

    ''' <summary> Gets or sets the Measure Unit command format. </summary>
    ''' <value> The Unit command format. </value>
    Protected Overridable ReadOnly Property MeasureUnitCommandFormat As String = "_G.smu.measure.unit={0}"


    ''' <summary> Writes the Measure Unit without reading back the value from the device. </summary>
    ''' <param name="value"> The Unit. </param>
    ''' <returns> The <see cref="MeasureUnit">Measure Unit</see> or none if unknown. </returns>
    Public Overridable Function WriteMeasureUnit(ByVal value As MeasureUnit) As MeasureUnit?
        Me.Session.WriteLine(Me.MeasureUnitCommandFormat, value.ExtractBetween())
        Me.MeasureUnit = value
        Return Me.MeasureUnit
    End Function

#End Region

#Region " OPEN DETECTOR ENABLED "

    ''' <summary> Gets or sets a list of states of the open detector knowns. </summary>
    ''' <value> The open detector known states. </value>
    Public ReadOnly Property OpenDetectorKnownStates As MeasureFunctionEnabledDictionary

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
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
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

    ''' <summary> Gets or sets the remote sense selected print query command. </summary>
    ''' <value> The remote sense selected query command. </value>
    Protected Overridable ReadOnly Property RemoteSenseSelectedPrintCommand As String

    ''' <summary> Queries the Remote Sense Selected sentinel. Also sets the
    ''' <see cref="RemoteSenseSelected">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryRemoteSenseSelected() As Boolean?
        Me.RemoteSenseSelected = Me.Query(Me.RemoteSenseSelected, Me.RemoteSenseSelectedPrintCommand)
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

''' <summary> Specifies the units. </summary>
Public Enum MeasureUnit
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Volt (smu.UNIT_VOLT)")> Volt
    <ComponentModel.Description("Ohm (smu.UNIT_OHM)")> Ohm
    <ComponentModel.Description("Ampere (smu.UNIT_AMP)")> Ampere
    <ComponentModel.Description("Watt (smu.UNIT_WATT)")> Watt
End Enum


''' <summary> Specifies the function modes. </summary>
Public Enum MeasureFunctionMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("DC Voltage (smu.FUNC_DC_VOLTAGE)")> VoltageDC
    <ComponentModel.Description("DC Current (smu.FUNC_DC_CURRENT)")> CurrentDC
    <ComponentModel.Description("Resistance (smu.FUNC_RESISTANCE)")> Resistance
End Enum

''' <summary> Dictionary of measure function ranges. </summary>
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
Public Class MeasureFunctionRangeDictionary
    Inherits Collections.Generic.Dictionary(Of MeasureFunctionMode, Core.Pith.RangeR)
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
Public Class MeasureFunctionEnabledDictionary
    Inherits Collections.Generic.Dictionary(Of MeasureFunctionMode, Boolean)
End Class
