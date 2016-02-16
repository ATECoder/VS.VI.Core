''' <summary> Defines the contract that must be implemented by a Source Voltage Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SourceVoltageSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceVoltageSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.AutoRangeEnabled = True
        Me.Level = 0
        Me.ProtectionLevel = Scpi.Syntax.Infinity
        Me.Range = 21
        Me.SweepStartLevel = 0
        Me.SweepStopLevel = 0
        Me.SweepMode = VI.SweepMode.Fixed
    End Sub

#End Region

#Region " AUTO RANGE "

    ''' <summary> The automatic range enabled. </summary>
    Private _AutoRangeEnabled As Boolean?

    ''' <summary> Gets or sets a value indicating whether Auto Range is enabled. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if enabled; otherwise, <c>False</c>. </value>
    Public Overloads Property AutoRangeEnabled As Boolean?
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
    ''' <param name="value">       Set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoRangeEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoRangeEnabled(value)
        Return Me.QueryAutoRangeEnabled()
    End Function

    ''' <summary> Queries the Auto Range Enabled sentinel. Also sets the
    ''' <see cref="AutoRangeEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public MustOverride Function QueryAutoRangeEnabled() As Boolean?

    ''' <summary> Writes the Auto Range Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public MustOverride Function WriteAutoRangeEnabled(ByVal value As Boolean) As Boolean?

#End Region

#Region " LEVEL "

    ''' <summary> The level. </summary>
    Private _Level As Double?

    ''' <summary> Gets or sets the cached Source Voltage Level. </summary>
    ''' <value> The Source Voltage Level. Actual Voltage depends on the power supply mode. </value>
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

    ''' <summary> Queries the level. </summary>
    ''' <returns> The level or none if unknown. </returns>
    Public MustOverride Function QueryLevel() As Double?

    ''' <summary> Writes and reads back the source Voltage level. </summary>
    ''' <remarks> These commands set the immediate output Voltage level. The values are programmed in
    ''' Volts. The immediate level is the output Voltage setting. At *RST, the Voltage values = 0.
    ''' The range of values that can be programmed for these commands is coupled with the voltage
    ''' protection and the voltage limit low settings. The maximum value for the immediate and
    ''' triggered voltage level is either the value in the following table, or the voltage protection
    ''' setting divided by 1.05; whichever is lower. The minimum value is either the value in the
    ''' table, or the low voltage setting divided by 0.95; whichever is higher.  </remarks>
    ''' <param name="value"> The Voltage level. </param>
    ''' <returns> The level or none if unknown. </returns>
    Public Function ApplyLevel(ByVal value As Double) As Double?
        Me.WriteLevel(value)
        Return Me.QueryLevel()
    End Function

    ''' <summary> Write the source Voltage level without reading back from the instrument. </summary>
    ''' <remarks> These commands set the immediate output Voltage level. The values are programmed in
    ''' Volts. The immediate level is the output Voltage setting. At *RST, the Voltage values = 0.
    ''' The range of values that can be programmed for these commands is coupled with the voltage
    ''' protection and the voltage limit low settings. The maximum value for the immediate and
    ''' triggered voltage level is either the value in the following table, or the voltage protection
    ''' setting divided by 1.05; whichever is lower. The minimum value is either the value in the
    ''' table, or the low voltage setting divided by 0.95; whichever is higher.  </remarks>
    ''' <param name="value"> The Voltage level. </param>
    ''' <returns> <see cref="Level">Level</see>. </returns>
    Public MustOverride Function WriteLevel(ByVal value As Double) As Double?

#End Region

#Region " PROTECTION LEVEL "

    ''' <summary> The protection level. </summary>
    Private _ProtectionLevel As Double?

    ''' <summary> Gets or sets the cached Over Voltage Protection Level. </summary>
    ''' <remarks> This command sets the over-voltage protection (OVP) level of the output. The values
    ''' are programmed in volts. If the output voltage exceeds the OVP level, the output is disabled
    ''' and OVP is set in the Questionable Condition status register. The*RST value = Max. </remarks>
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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Range))
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

#Region " SWEEP START LEVEL "

    ''' <summary> The sweep start level. </summary>
    Private _SweepStartLevel As Double?

    ''' <summary> Gets or sets the cached Source Voltage Sweep Start Level. </summary>
    ''' <value> The Source Voltage Sweep Start Level. </value>
    Public Overloads Property SweepStartLevel As Double?
        Get
            Return Me._SweepStartLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.SweepStartLevel, value) Then
                Me._SweepStartLevel = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SweepStartLevel))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Voltage Sweep Start Level. </summary>
    ''' <param name="value"> The Voltage SweepStartLevel. </param>
    ''' <returns> <see cref="SweepStartLevel">Sweep Start Level</see>. </returns>
    Public Function ApplySweepStartLevel(ByVal value As Double) As Double?
        Me.WriteSweepStartLevel(value)
        Return Me.QuerySweepStartLevel()
    End Function

    ''' <summary> Queries the source voltage Sweep Start Level. </summary>
    ''' <returns> The source voltage Sweep Start Level or none if unknown. </returns>
    Public MustOverride Function QuerySweepStartLevel() As Double?

    ''' <summary> Writes the source Voltage Sweep Start Level without reading back from the instrument. </summary>
    ''' <param name="value"> The Voltage SweepStartLevel. </param>
    ''' <returns> <see cref="SweepStartLevel">Sweep Start Level</see>. </returns>
    Public MustOverride Function WriteSweepStartLevel(ByVal value As Double) As Double?

#End Region

#Region " SWEEP STOP LEVEL "

    ''' <summary> The sweep stop level. </summary>
    Private _SweepStopLevel As Double?

    ''' <summary> Gets or sets the cached Source Voltage Sweep Stop Level. </summary>
    ''' <value> The Source Voltage Sweep Stop Level. </value>
    Public Overloads Property SweepStopLevel As Double?
        Get
            Return Me._SweepStopLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.SweepStopLevel, value) Then
                Me._SweepStopLevel = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SweepStopLevel))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Voltage Sweep Stop Level. </summary>
    ''' <param name="value"> The Voltage SweepStopLevel. </param>
    ''' <returns> <see cref="SweepStopLevel">Sweep Stop Level</see>. </returns>
    Public Function ApplySweepStopLevel(ByVal value As Double) As Double?
        Me.WriteSweepStopLevel(value)
        Return Me.QuerySweepStopLevel()
    End Function

    ''' <summary> Queries the source voltage Sweep Stop Level. </summary>
    ''' <returns> The source voltage Sweep Stop Level or none if unknown. </returns>
    Public MustOverride Function QuerySweepStopLevel() As Double?

    ''' <summary> Writes the source Voltage Sweep Stop Level without reading back from the instrument. </summary>
    ''' <param name="value"> The Voltage SweepStopLevel. </param>
    ''' <returns> <see cref="SweepStopLevel">Sweep Stop Level</see>. </returns>
    Public MustOverride Function WriteSweepStopLevel(ByVal value As Double) As Double?

#End Region

#Region " SWEEP MODE "

    ''' <summary> The sweep mode. </summary>
    Private _SweepMode As SweepMode?

    ''' <summary> Gets or sets the cached sweep mode. </summary>
    ''' <value> The sweep mode or none if not set or unknown. </value>
    Public Overloads Property SweepMode As SweepMode?
        Get
            Return Me._SweepMode
        End Get
        Protected Set(ByVal value As SweepMode?)
            If Not Nullable.Equals(Me.SweepMode, value) Then
                Me._SweepMode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SweepMode))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the sweep mode. </summary>
    ''' <param name="value"> The  sweep mode. </param>
    ''' <returns> The sweep mode or none if unknown. </returns>
    Public Function ApplySweepMode(ByVal value As SweepMode) As SweepMode?
        Me.WriteSweepMode(value)
        Return Me.QuerySweepMode()
    End Function

    ''' <summary> Queries the sweep mode. </summary>
    ''' <returns> The sweep mode or none if unknown. </returns>
    Public MustOverride Function QuerySweepMode() As SweepMode?

    ''' <summary> Writes the sweep mode but does not read back the from the device. </summary>
    ''' <param name="value"> The  sweep mode. </param>
    ''' <returns> The sweep mode or none if unknown. </returns>
    Public MustOverride Function WriteSweepMode(ByVal value As SweepMode) As SweepMode?

#End Region

End Class

''' <summary>Specifies the source sweep modes.</summary>
Public Enum SweepMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Fixed (FIX)")> Fixed
    <ComponentModel.Description("Sweep (SWE)")> Sweep
    <ComponentModel.Description("List (LIST)")> List
End Enum
