Imports isr.Core.Pith.EnumExtensions
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
    Inherits VI.Scpi.SenseSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FunctionModes = VI.Scpi.SenseFunctionModes.VoltageDC
        Me.ConcurrentSenseEnabled = New Boolean?
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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.AutoRangeEnabled))
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

#Region " CONCURRENT FUNCTION MODE "

    ''' <summary> State of the output on. </summary>
    Private _ConcurrentSenseEnabled As Boolean?

    ''' <summary> Gets or sets the cached Concurrent Function Mode Enabled sentinel. </summary>
    ''' <value> <c>null</c> if mode is not known; <c>True</c> if concurrent; otherwise, <c>False</c>. </value>
    Public Property ConcurrentSenseEnabled As Boolean?
        Get
            Return Me._ConcurrentSenseEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.ConcurrentSenseEnabled, value) Then
                Me._ConcurrentSenseEnabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ConcurrentSenseEnabled))
            End If
        End Set
    End Property

    ''' <summary> Queries the Concurrent Function Mode Enabled sentinel. 
    '''           Also sets the <see cref="ConcurrentSenseEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>null</c> if mode is not known; <c>True</c> if concurrent; otherwise, <c>False</c>. </returns>
    Public MustOverride Function QueryConcurrentSenseEnabled() As Boolean?

    ''' <summary> Writes the Concurrent Function Mode Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is concurrent. </param>
    ''' <returns> <c>null</c> if mode is not known; <c>True</c> if concurrent; otherwise, <c>False</c>. </returns>
    Public MustOverride Function WriteConcurrentSenseEnabled(ByVal value As Boolean) As Boolean?

    ''' <summary> Writes and reads back the Concurrent Function Mode Enabled sentinel. </summary>
    ''' <param name="value"> if set to <c>True</c> is concurrent. </param>
    ''' <returns> <c>null</c> if mode is not known; <c>True</c> if concurrent; otherwise, <c>False</c>. </returns>
    Public Function ApplyConcurrentSenseEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteConcurrentSenseEnabled(value)
        Return Me.QueryConcurrentSenseEnabled()
    End Function

#End Region

#Region " POWER LINE CYCLES (NPLC) "

    ''' <summary> The Power Line Cycles. </summary>
    Private _PowerLineCycles As Double?

    ''' <summary> Gets or sets the cached sense PowerLineCycles. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property PowerLineCycles As Double?
        Get
            Return Me._PowerLineCycles
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.PowerLineCycles, value) Then
                Me._PowerLineCycles = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.PowerLineCycles))
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
    ''' <see cref="isr.VI.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="isr.VI.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property ProtectionLevel As Double?
        Get
            Return Me._ProtectionLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.ProtectionLevel, value) Then
                Me._ProtectionLevel = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ProtectionLevel))
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

    ''' <summary> The Current Range. </summary>
    Private _Range As Double?

    ''' <summary> Gets or sets the cached sense current range. Set to
    ''' <see cref="isr.VI.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="isr.VI.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Range As Double?
        Get
            Return Me._Range
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Range, value) Then
                Me._Range = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Range))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the sense current Range. </summary>
    ''' <remarks> The value is in Amperes. At *RST, the range is set to Auto and the specific range is unknown. </remarks>
    ''' <param name="value"> The current Range. </param>
    ''' <returns> The Current Range. </returns>
    Public Function ApplyRange(ByVal value As Double) As Double?
        Me.WriteRange(value)
        Return Me.QueryRange
    End Function

    ''' <summary> Queries the current Range. </summary>
    ''' <returns> The current Range or none if unknown. </returns>
    Public MustOverride Function QueryRange() As Double?

    ''' <summary> Writes the current Range without reading back the value from the device. </summary>
    ''' <remarks> This command sets the current Range. The value is in Amperes. 
    '''           At *RST, the range is auto and the values is not known. </remarks>
    ''' <param name="value"> The current Range. </param>
    ''' <returns> The Current Range. </returns>
    Public MustOverride Function WriteRange(ByVal value As Double) As Double?

#End Region

#Region " FUNCTION MODES "

    ''' <summary> Builds the Modes record for the specified Modes. </summary>
    ''' <param name="Modes"> Sense Function Modes. </param>
    ''' <returns> The record. </returns>
    Public Shared Function BuildRecord(ByVal modes As VI.Scpi.SenseFunctionModes) As String
        If modes = VI.Scpi.SenseFunctionModes.None Then
            Return String.Empty
        Else
            Dim reply As New System.Text.StringBuilder
            For Each code As Integer In [Enum].GetValues(GetType(VI.Scpi.SenseFunctionModes))
                If (modes And code) <> 0 Then
                    Dim value As String = CType(code, VI.Scpi.SenseFunctionModes).ExtractBetween()
                    If Not String.IsNullOrWhiteSpace(value) Then
                        If reply.Length > 0 Then
                            reply.Append(",")
                        End If
                        reply.Append(value)
                    End If
                End If
            Next
            Return reply.ToString
        End If
    End Function

    ''' <summary> Get the composite Sense Function Modes based on the message from the instrument. </summary>
    ''' <param name="record"> Specifies the comma delimited Modes record. </param>
    ''' <returns> The sense function modes. </returns>
    Public Shared Function ParseSenseFunctionModes(ByVal record As String) As VI.Scpi.SenseFunctionModes
        Dim parsed As VI.Scpi.SenseFunctionModes = VI.Scpi.SenseFunctionModes.None
        If Not String.IsNullOrWhiteSpace(record) Then
            For Each modeValue As String In record.Split(","c)
                parsed = parsed Or SenseSubsystemBase.ParseSenseFunctionMode(modeValue)
            Next
        End If
        Return parsed
    End Function

    ''' <summary> The Sense Function Modes. </summary>
    Private _FunctionModes As VI.Scpi.SenseFunctionModes?

    ''' <summary> Gets or sets the cached Sense Function Modes. </summary>
    ''' <value> The Function Modes or null if unknown. </value>
    Public Property FunctionModes As VI.Scpi.SenseFunctionModes?
        Get
            Return Me._FunctionModes
        End Get
        Protected Set(ByVal value As VI.Scpi.SenseFunctionModes?)
            If Not Nullable.Equals(Me.FunctionModes, value) Then
                Me._FunctionModes = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.FunctionModes))
            End If
        End Set
    End Property

    ''' <summary> Queries the Sense Function Modes. </summary>
    ''' <returns> The Function Modes or null if unknown. </returns>
    Public MustOverride Function QueryFunctionModes() As VI.Scpi.SenseFunctionModes?

    ''' <summary> Writes the Sense Function Modes. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Sense Function mode. </param>
    ''' <returns> The Function Modes or null if unknown. </returns>
    Public MustOverride Function WriteFunctionModes(ByVal value As VI.Scpi.SenseFunctionModes) As VI.Scpi.SenseFunctionModes?

    ''' <summary> Writes and reads back the Sense Function Modes. </summary>
    ''' <param name="value"> The <see cref="VI.Scpi.SenseFunctionModes">Function Modes</see>. </param>
    ''' <returns> The Sense Function Modes or null if unknown. </returns>
    Public Function ApplyFunctionModes(ByVal value As VI.Scpi.SenseFunctionModes) As VI.Scpi.SenseFunctionModes?
        Me.WriteFunctionModes(value)
        Return Me.QueryFunctionModes()
    End Function

    Private _supportsMultiFunctions As Boolean

    ''' <summary> Gets or sets the condition telling if the instrument supports multi-functions. For
    ''' example, the 2400 source-measure instrument support measuring voltage, current, and
    ''' resistance concurrently whereas the 2700 supports a single function at a time. </summary>
    ''' <value> The supports multi functions. </value>
    <CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Multi")>
    Public Property SupportsMultiFunctions() As Boolean
        Get
            Return Me._supportsMultiFunctions
        End Get
        Set(ByVal value As Boolean)
            If Not Me.SupportsMultiFunctions.Equals(value) Then
                Me._supportsMultiFunctions = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SupportsMultiFunctions))
            End If
        End Set
    End Property

#End Region

#Region " LATEST DATA "

    Private _readings As Readings

    ''' <summary> Returns the readings. </summary>
    ''' <returns> The readings. </returns>
    Public Function Readings() As Readings
        Return Me._readings
    End Function

    ''' <summary> Parses a new set of reading elements. </summary>
    ''' <param name="reading"> Specifies the measurement text to parse into the new reading. </param>
    Public Overrides Sub ParseReading(ByVal reading As String)

        ' check if we have units suffixes.
        If (Me._readings.Elements And ReadingElements.Units) <> 0 Then
            reading = ReadingElement.TrimUnits(reading)
        End If

        ' Take a reading and parse the results
        Me.Readings.TryParse(reading)

    End Sub

#End Region

End Class

