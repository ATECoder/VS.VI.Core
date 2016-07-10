Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a Source Channel Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public MustInherit Class SourceChannelSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="SourceSubsystemBase" /> class.
    ''' </summary>
    ''' <remarks> David, 7/6/2016. </remarks>
    ''' <param name="channelNumber">   The channel number. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.ChannelNumber = channelNumber
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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.AutoClearEnabled))
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
    ''' <remarks> SCPI: ":SOUR&lt;ch&gt;:CLE:AUTO?" </remarks>
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
    ''' <remarks> SCPI: ":SOUR&lt;ch&gt;:CLE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.AutoDelayEnabled))
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
    ''' <remarks> SCPI: ":SOUR&lt;ch&gt;:DEL:AUTO?" </remarks>
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
    ''' <remarks> SCPI: ":SOUR&lt;ch&gt;:DEL:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoDelayEnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoDelayEnabled = Me.Write(value, Me.AutoDelayEnabledCommandFormat)
        Return Me.AutoDelayEnabled
    End Function

#End Region

#Region " CHANNEL "

    ''' <summary> Gets or sets the channel number. </summary>
    ''' <value> The channel number. </value>
    Public ReadOnly Property ChannelNumber As Integer

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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Delay))
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
    ''' <remarks> SCPI: ":SOUR&lt;ch&gt;:DEL?" </remarks>
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
    ''' <remarks> SCPI: ":SOUR&lt;ch&gt;:DEL {0:s\.FFFFFFF}" </remarks>
    Protected Overridable ReadOnly Property DelayCommandFormat As String

    ''' <summary> Writes the Source Delay without reading back the value from the device. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Source Delay or none if unknown. </returns>
    Public Function WriteDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.Delay = Me.Write(value, Me.DelayCommandFormat)
        Return Me.Delay
    End Function

#End Region

#Region " FUNCTION MODE "

    Private _SupportedFunctionModes As SourceFunctionModes
    ''' <summary>
    ''' Gets or sets the supported Function Mode.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedFunctionModes() As SourceFunctionModes
        Get
            Return _SupportedFunctionModes
        End Get
        Set(ByVal value As SourceFunctionModes)
            If Not Me.SupportedFunctionModes.Equals(value) Then
                Me._SupportedFunctionModes = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SupportedFunctionModes))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the function code. </summary>
    ''' <value> The function code. </value>
    Protected ReadOnly Property FunctionCode As String

    ''' <summary> The function mode. </summary>
    Private _FunctionMode As SourceFunctionModes?

    ''' <summary> Gets or sets the cached source FunctionMode. </summary>
    ''' <value> The <see cref="SourceFunctionModes">source Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overridable Property FunctionMode As SourceFunctionModes?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As SourceFunctionModes?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                If value.HasValue Then
                    Me._FunctionCode = value.Value.ExtractBetween
                Else
                    Me._FunctionCode = ""
                End If
                Me.AsyncNotifyPropertyChanged(NameOf(Me.FunctionMode))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Function Mode. </summary>
    ''' <param name="value"> The  Source Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As SourceFunctionModes) As SourceFunctionModes?
        Me.WriteFunctionMode(value)
        Return Me.QueryFunctionMode()
    End Function

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command, e.g., :SOUR&lt;ch&gt;:FUNC? </value>
    Protected Overridable ReadOnly Property FunctionModeQueryCommand As String

    ''' <summary> Queries the Source Function Mode. </summary>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public Function QueryFunctionMode() As SourceFunctionModes?
        If String.IsNullOrWhiteSpace(Me.FunctionModeQueryCommand) Then
            Me.FunctionMode = New SourceFunctionModes?
        Else
            Dim mode As String = Me.FunctionMode.ToString
            Me.Session.MakeEmulatedReplyIfEmpty(mode)
            mode = Me.Session.QueryTrimEnd(Me.FunctionModeQueryCommand)
            If String.IsNullOrWhiteSpace(mode) Then
                Dim message As String = "Failed fetching source function mode"
                Debug.Assert(Not Debugger.IsAttached, message)
                Me.FunctionMode = New SourceFunctionModes?
            Else
                Dim se As New StringEnumerator(Of SourceFunctionModes)
                Me.FunctionMode = se.ParseContained(mode.BuildDelimitedValue)
            End If
        End If
        Return Me.FunctionMode
    End Function

    ''' <summary> Gets or sets the function mode command. </summary>
    ''' <value> The function mode command, e.g., :SOUR&lt;ch&gt;:FUNC {0}. </value>
    Protected Overridable ReadOnly Property FunctionModeCommandFormat As String

    ''' <summary> Writes the source Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public Function WriteFunctionMode(ByVal value As SourceFunctionModes) As SourceFunctionModes?
        If Not String.IsNullOrWhiteSpace(Me.FunctionModeCommandFormat) Then
            Me.Session.WriteLine(Me.FunctionModeCommandFormat, value.ExtractBetween())
        End If
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

#End Region

#Region " LEVEL "

    ''' <summary> The Range of function values. </summary>
    Public Overridable Property LevelRange As Core.Pith.RangeR

    ''' <summary> The Level. </summary>
    Private _Level As Double?

    ''' <summary> Gets or sets the cached Level. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Level As Double?
        Get
            Return Me._Level
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Level, value) Then
                Me._Level = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Level))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Level. </summary>
    ''' <param name="value"> The Level. </param>
    ''' <returns> The Level. </returns>
    Public Function ApplyLevel(ByVal value As Double) As Double?
        Me.WriteLevel(value)
        Return Me.QueryLevel
    End Function

    ''' <summary> Gets or sets the Level query command. </summary>
    ''' <value> The Level query command. </value>
    Protected Overridable ReadOnly Property LevelQueryCommand As String

    ''' <summary> Queries the Level. </summary>
    ''' <returns> The Level or none if unknown. </returns>
    Public Function QueryLevel() As Double?
        Me.Level = Me.Query(Me.Level, Me.LevelQueryCommand)
        Return Me.Level
    End Function

    ''' <summary> Gets or sets the Level command format. </summary>
    ''' <value> The Level command format. </value>
    Protected Overridable ReadOnly Property LevelCommandFormat As String

    ''' <summary> Writes the Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Level. </remarks>
    ''' <param name="value"> The Level. </param>
    ''' <returns> The Level. </returns>
    Public Function WriteLevel(ByVal value As Double) As Double?
        Me.Level = Me.Write(value, Me.LevelCommandFormat)
        Return Me.Level
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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SweepPoints))
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
    ''' <remarks> SCPI: ":SOUR&lt;ch&gt;:SWE:POIN?" </remarks>
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
    ''' <remarks> SCPI: "SOUR&lt;ch&gt;:SWE:POIN {0}" </remarks>
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

