Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Source Voltage Subsystem. </summary>
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
    Inherits VI.SourceVoltageSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceVoltageSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " AUTO RANGE "

    ''' <summary> Queries the Auto Range Enabled sentinel. Also sets the
    ''' <see cref="AutoRangeEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Overrides Function QueryAutoRangeEnabled() As Boolean?
        Me.AutoRangeEnabled = Session.Query(Me.AutoRangeEnabled.GetValueOrDefault(True), ":SOUR:VOLT:RANG:AUTO?")
        Return Me.AutoRangeEnabled
    End Function

    ''' <summary> Writes the Auto Range Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Overrides Function WriteAutoRangeEnabled(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine(":SOUR:VOLT:RANG:AUTO {0:'ON';'ON';'OFF'}", CType(value, Integer))
        Me.AutoRangeEnabled = value
        Return Me.AutoRangeEnabled
    End Function

#End Region

#Region " LEVEL "

    ''' <summary> Queries the level. </summary>
    ''' <returns> The level or none if unknown. </returns>
    Public Overrides Function QueryLevel() As Double?
        Me.Level = Me.Session.Query(Me.Level.GetValueOrDefault(0), ":SOURCE:VOLT?")
        Return Me.Level
    End Function

    ''' <summary> Writes and reads back the source Voltage level. </summary>
    ''' <remarks> These commands set the immediate output Voltage level. The values are programmed in
    ''' Volts. The immediate level is the output Voltage setting. At *RST, the Voltage values = 0.
    ''' The range of values that can be programmed for these commands is coupled with the voltage
    ''' protection and the voltage limit low settings. The maximum value for the immediate and
    ''' triggered voltage level is either the value in the following table, or the voltage protection
    ''' setting divided by 1.05; whichever is lower. The minimum value is either the value in the
    ''' table, or the low voltage setting divided by 0.95; whichever is higher.  </remarks>
    ''' <param name="value"> The Voltage level. </param>
    ''' <returns> <see cref="Level"/> </returns>
    Public Overrides Function WriteLevel(ByVal value As Double) As Double?
        Me.Session.WriteLine(":SOURCE:VOLT {0}", value)
        Me.Level = value
        Return Me.Level
    End Function

#End Region

#Region " SWEEP START LEVEL "

    ''' <summary> Queries the source voltage Sweep Start Level. </summary>
    ''' <returns> The source voltage Sweep Start Level or none if unknown. </returns>
    Public Overrides Function QuerySweepStartLevel() As Double?
        Me.SweepStartLevel = Me.Session.Query(Me.SweepStartLevel.GetValueOrDefault(0), ":SOUR:VOLT:STAR?")
        Return Me.SweepStartLevel
    End Function

    ''' <summary> Writes and reads back the source Voltage Sweep Start Level. </summary>
    ''' <param name="value"> The Voltage SweepStartLevel. </param>
    ''' <returns> <see cref="SweepStartLevel">Sweep Start Level</see>. </returns>
    Public Overrides Function WriteSweepStartLevel(ByVal value As Double) As Double?
        Me.Session.WriteLine(":SOUR:VOLT:STAR {0}", value)
        Me.SweepStartLevel = value
        Return Me.SweepStartLevel
    End Function

#End Region

#Region " SWEEP STOP LEVEL "

    ''' <summary> Queries the source voltage Sweep Stop Level. </summary>
    ''' <returns> The source voltage Sweep Stop Level or none if unknown. </returns>
    Public Overrides Function QuerySweepStopLevel() As Double?
        Me.SweepStopLevel = Me.Session.Query(Me.SweepStopLevel.GetValueOrDefault(1), ":SOUR:VOLT:STOP?")
        Return Me.SweepStopLevel
    End Function


    ''' <summary> Writes and reads back the source Voltage Sweep Stop Level. </summary>
    ''' <param name="value"> The Voltage SweepStopLevel. </param>
    ''' <returns> <see cref="SweepStopLevel">Sweep Stop Level</see>. </returns>
    Public Overrides Function WriteSweepStopLevel(ByVal value As Double) As Double?
        Me.Session.WriteLine(":SOUR:VOLT:STOP {0}", value)
        Me.SweepStopLevel = value
        Return Me.SweepStopLevel
    End Function

#End Region

#Region " SWEEP MODE "

    ''' <summary> Queries the sweep mode. </summary>
    ''' <returns> The sweep mode or none if unknown. </returns>
    Public Overrides Function QuerySweepMode() As SweepMode?
        Dim mode As String = Me.SweepMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryTrimEnd(":SOUR:VOLT:MODE?")
        If String.IsNullOrWhiteSpace(mode) Then
            Me.SweepMode = New SweepMode?
        Else
            Dim se As New StringEnumerator(Of SweepMode)
            Me.SweepMode = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.SweepMode
    End Function

    ''' <summary> Writes the sweep mode but does not read back from the device. </summary>
    ''' <param name="value"> The  sweep mode. </param>
    ''' <returns> The sweep mode or none if unknown. </returns>
    Public Overrides Function WriteSweepMode(ByVal value As SweepMode) As SweepMode?
        Me.Session.WriteLine(":SOUR:VOLT:MODE {0}", value.ExtractBetween())
        Me.SweepMode = value
        Return Me.SweepMode
    End Function

#End Region

End Class
