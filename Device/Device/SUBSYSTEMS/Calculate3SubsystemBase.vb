''' <summary> Defines a Scpi Calculate3 Subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created </history>
Public MustInherit Class Calculate3SubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="Calculate3SubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " DIGITAL I/O "

#Region " FORCED DIGITAL OUTPUT PATTERN ENABLED "

    Private _ForcedDigitalOutputPatternEnabled As Boolean?
    ''' <summary> Gets or sets the cached Forced Digital Output Pattern Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Forced Digital Output Pattern Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property ForcedDigitalOutputPatternEnabled As Boolean?
        Get
            Return Me._ForcedDigitalOutputPatternEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.ForcedDigitalOutputPatternEnabled, value) Then
                Me._ForcedDigitalOutputPatternEnabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ForcedDigitalOutputPatternEnabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Forced Digital Output Pattern Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyForcedDigitalOutputPatternEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteForcedDigitalOutputPatternEnabled(value)
        Return Me.QueryForcedDigitalOutputPatternEnabled()
    End Function

    ''' <summary> Gets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":CALC3:FORC:STAT?" </remarks>
    Protected Overridable ReadOnly Property ForcedDigitalOutputPatternEnabledQueryCommand As String

    ''' <summary> Queries the Forced Digital Output Pattern Enabled sentinel. Also sets the
    ''' <see cref="ForcedDigitalOutputPatternEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryForcedDigitalOutputPatternEnabled() As Boolean?
        Me.ForcedDigitalOutputPatternEnabled = Me.Query(Me.ForcedDigitalOutputPatternEnabled, Me.ForcedDigitalOutputPatternEnabledQueryCommand)
        Return Me.ForcedDigitalOutputPatternEnabled
    End Function

    ''' <summary> Gets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":CALC3:FORC:STAT {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property ForcedDigitalOutputPatternEnabledCommandFormat As String

    ''' <summary> Writes the Forced Digital Output Pattern Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteForcedDigitalOutputPatternEnabled(ByVal value As Boolean) As Boolean?
        Me.ForcedDigitalOutputPatternEnabled = Me.Write(value, Me.ForcedDigitalOutputPatternEnabledCommandFormat)
        Return Me.ForcedDigitalOutputPatternEnabled
    End Function

#End Region

#Region " FORCED DIGITAL OUTPUT PATTERN "

    Private _ForcedDigitalOutputPattern As Integer?
    ''' <summary> Gets or sets the cached Forced Digital Output Pattern. </summary>
    ''' <value> The Forced Digital Output Pattern or none if not set or unknown. </value>
    Public Overloads Property ForcedDigitalOutputPattern As Integer?
        Get
            Return Me._ForcedDigitalOutputPattern
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.ForcedDigitalOutputPattern, value) Then
                Me._ForcedDigitalOutputPattern = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ForcedDigitalOutputPattern))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Forced Digital Output Pattern. </summary>
    ''' <param name="value"> The current Forced Digital Output Pattern. </param>
    ''' <returns> The Forced Digital Output Pattern or none if unknown. </returns>
    Public Function ApplyForcedDigitalOutputPattern(ByVal value As Integer) As Integer?
        Me.WriteForcedDigitalOutputPattern(value)
        Return Me.QueryForcedDigitalOutputPattern()
    End Function

    ''' <summary> Gets the forced digital output Pattern query command. </summary>
    ''' <value> The forced digital output Pattern query command. </value>
    ''' <remarks> SCPI ":CALC3:FORC:PATT". </remarks>
    Protected Overridable ReadOnly Property ForcedDigitalOutputPatternQueryCommand As String

    ''' <summary> Queries the current Forced Digital Output Pattern. </summary>
    ''' <returns> The Forced Digital Output Pattern or none if unknown. </returns>
    Public Function QueryForcedDigitalOutputPattern() As Integer?
        Me.ForcedDigitalOutputPattern = Me.Query(Me.ForcedDigitalOutputPattern, Me.ForcedDigitalOutputPatternQueryCommand)
        Return Me.ForcedDigitalOutputPattern

    End Function

    ''' <summary> Gets the forced digital output Pattern command format. </summary>
    ''' <value> The forced digital output Pattern command format. </value>
    ''' <remarks> SCPI ":CALC3:FORC:PATT". </remarks>
    Protected Overridable ReadOnly Property ForcedDigitalOutputPatternCommandFormat As String

    ''' <summary> Sets back the Forced Digital Output Pattern without reading back the value from the device. </summary>
    ''' <param name="value"> The current Forced Digital Output Pattern. </param>
    ''' <returns> The Forced Digital Output Pattern or none if unknown. </returns>
    Public Function WriteForcedDigitalOutputPattern(ByVal value As Integer) As Integer?
        Me.ForcedDigitalOutputPattern = Me.Write(value, Me.ForcedDigitalOutputPatternCommandFormat)
        Return Me.ForcedDigitalOutputPattern
    End Function

#End Region

#End Region

End Class


