''' <summary> Defines the contract that must be implemented by a Source Function Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SourceFunctionSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceFunctionSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " LEVEL "

    ''' <summary> The Range of function values. </summary>
    Public Overridable ReadOnly Property LevelRange As Core.Pith.RangeR

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
                Me.SafePostPropertyChanged()
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

#Region " RANGE "

    ''' <summary> The range. </summary>
    Private _Range As Double?

    ''' <summary> Gets or sets the cached range. Set to
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

#Region " SWEEP START LEVEL "

    ''' <summary> The Sweep Start Level. </summary>
    Private _SweepStartLevel As Double?

    ''' <summary> Gets or sets the cached Sweep Start Level. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property SweepStartLevel As Double?
        Get
            Return Me._SweepStartLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.SweepStartLevel, value) Then
                Me._SweepStartLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sweep Start Level. </summary>
    ''' <param name="value"> The Sweep Start Level. </param>
    ''' <returns> The Sweep Start Level. </returns>
    Public Function ApplySweepStartLevel(ByVal value As Double) As Double?
        Me.WriteSweepStartLevel(value)
        Return Me.QuerySweepStartLevel
    End Function

    ''' <summary> Gets or sets the Sweep Start Level query command. </summary>
    ''' <value> The Sweep Start Level query command. </value>
    Protected Overridable ReadOnly Property SweepStartLevelQueryCommand As String

    ''' <summary> Queries the Sweep Start Level. </summary>
    ''' <returns> The Sweep Start Level or none if unknown. </returns>
    Public Function QuerySweepStartLevel() As Double?
        Me.SweepStartLevel = Me.Query(Me.SweepStartLevel, Me.SweepStartLevelQueryCommand)
        Return Me.SweepStartLevel
    End Function

    ''' <summary> Gets or sets the Sweep Start Level command format. </summary>
    ''' <value> The Sweep Start Level command format. </value>
    Protected Overridable ReadOnly Property SweepStartLevelCommandFormat As String

    ''' <summary> Writes the Sweep Start Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Sweep Start Level. </remarks>
    ''' <param name="value"> The Sweep Start Level. </param>
    ''' <returns> The Sweep Start Level. </returns>
    Public Function WriteSweepStartLevel(ByVal value As Double) As Double?
        Me.SweepStartLevel = Me.Write(value, Me.SweepStartLevelCommandFormat)
        Return Me.SweepStartLevel
    End Function

#End Region

#Region " SWEEP STOP LEVEL "

    ''' <summary> The Sweep Stop Level. </summary>
    Private _SweepStopLevel As Double?

    ''' <summary> Gets or sets the cached Sweep Stop Level. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property SweepStopLevel As Double?
        Get
            Return Me._SweepStopLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.SweepStopLevel, value) Then
                Me._SweepStopLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sweep Stop Level. </summary>
    ''' <param name="value"> The Sweep Stop Level. </param>
    ''' <returns> The Sweep Stop Level. </returns>
    Public Function ApplySweepStopLevel(ByVal value As Double) As Double?
        Me.WriteSweepStopLevel(value)
        Return Me.QuerySweepStopLevel
    End Function

    ''' <summary> Gets or sets the Sweep Stop Level query command. </summary>
    ''' <value> The Sweep Stop Level query command. </value>
    Protected Overridable ReadOnly Property SweepStopLevelQueryCommand As String

    ''' <summary> Queries the Sweep Stop Level. </summary>
    ''' <returns> The Sweep Stop Level or none if unknown. </returns>
    Public Function QuerySweepStopLevel() As Double?
        Me.SweepStopLevel = Me.Query(Me.SweepStopLevel, Me.SweepStopLevelQueryCommand)
        Return Me.SweepStopLevel
    End Function

    ''' <summary> Gets or sets the Sweep Stop Level command format. </summary>
    ''' <value> The Sweep Stop Level command format. </value>
    Protected Overridable ReadOnly Property SweepStopLevelCommandFormat As String

    ''' <summary> Writes the Sweep Stop Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Sweep Stop Level. </remarks>
    ''' <param name="value"> The Sweep Stop Level. </param>
    ''' <returns> The Sweep Stop Level. </returns>
    Public Function WriteSweepStopLevel(ByVal value As Double) As Double?
        Me.SweepStopLevel = Me.Write(value, Me.SweepStopLevelCommandFormat)
        Return Me.SweepStopLevel
    End Function

#End Region

#Region " SWEEP MODE  "

    ''' <summary> The Sweep Mode. </summary>
    Private _SweepMode As SweepMode?

    ''' <summary> Gets or sets the cached Sweep Mode. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property SweepMode As SweepMode?
        Get
            Return Me._SweepMode
        End Get
        Protected Set(ByVal value As SweepMode?)
            If Not Nullable.Equals(Me.SweepMode, value) Then
                Me._SweepMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sweep Mode. </summary>
    ''' <param name="value"> The Sweep Mode. </param>
    ''' <returns> The Sweep Mode. </returns>
    Public Function ApplySweepMode(ByVal value As SweepMode) As SweepMode?
        Me.WriteSweepMode(value)
        Return Me.QuerySweepMode
    End Function

    ''' <summary> Gets or sets the Sweep Mode  query command. </summary>
    ''' <value> The Sweep Mode  query command. </value>
    Protected Overridable ReadOnly Property SweepModeQueryCommand As String

    ''' <summary> Queries the Sweep Mode. </summary>
    ''' <returns> The Sweep Mode  or none if unknown. </returns>
    Public Function QuerySweepMode() As SweepMode?
        Me.SweepMode = Me.Query(Me.SweepModeQueryCommand, Me.SweepMode)
        Return Me.SweepMode
    End Function

    ''' <summary> Gets or sets the Sweep Mode  command format. </summary>
    ''' <value> The Sweep Mode  command format. </value>
    Protected Overridable ReadOnly Property SweepModeCommandFormat As String

    ''' <summary> Writes the Sweep Mode  without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Sweep Mode. </remarks>
    ''' <param name="value"> The Sweep Mode. </param>
    ''' <returns> The Sweep Mode. </returns>
    Public Function WriteSweepMode(ByVal value As SweepMode) As SweepMode?
        Me.SweepMode = Me.Write(Me.SweepModeCommandFormat, value)
        Return Me.SweepMode
    End Function

#End Region

End Class

