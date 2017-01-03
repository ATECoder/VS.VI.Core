''' <summary> Defines a SCPI Four Wire Sense Resistance Subsystem for a Keithley 7500 Meter. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="03/01/2014" by="David" revision="3.0.5173"> Created. </history>
Public Class SenseFourWireResistanceSubsystem
    Inherits VI.Scpi.SenseResistanceSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    ''' <remarks> David, 6/27/2016. </remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        If Me.StatusSubsystem.LineFrequency.GetValueOrDefault(60) = 60 Then
            Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 15)
        Else
            Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 12)
        End If
        Me.ValueRange1 = New isr.Core.Pith.RangeR(1, 1000000000.0)
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.AsyncNotifyPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " AVERAGE "

    ''' <summary> Gets the average enabled command Format. </summary>
    ''' <value> The average enabled query command. </value>
    Protected Overrides ReadOnly Property AverageEnabledCommandFormat As String = ":SENS:FRES:AVER {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the average enabled query command. </summary>
    ''' <value> The average enabled query command. </value>
    Protected Overrides ReadOnly Property AverageEnabledQueryCommand As String = ":SENS:FRES:AVER?"

#End Region

#Region " AVERAGE COUNT "

    ''' <summary> Gets the Average Count command format. </summary>
    ''' <value> The AverageCount command format. </value>
    Protected Overrides ReadOnly Property AverageCountCommandFormat As String = ":SENS:FRES:AVER:COUNT {0}"

    ''' <summary> Gets the Average Count query command. </summary>
    ''' <value> The AverageCount query command. </value>
    Protected Overrides ReadOnly Property AverageCountQueryCommand As String = ":SENS:FRES:AVER:COUNT?"

#End Region

#Region " AVERAGE FILTER TYPE "

    ''' <summary> Gets the Average FilterType command format. </summary>
    ''' <value> The AverageFilterType command format. </value>
    Protected Overrides ReadOnly Property AverageFilterTypeCommandFormat As String = ":SENS:FRES:AVER:TCON {0}"

    ''' <summary> Gets the Average FilterType query command. </summary>
    ''' <value> The AverageFilterType query command. </value>
    Protected Overrides ReadOnly Property AverageFilterTypeQueryCommand As String = ":SENS:FRES:AVER:TCON?"

#End Region

#Region " AVERAGE PERCENT WINDOW "

    ''' <summary> Gets the Average Percent Window command format. </summary>
    ''' <value> The AveragePercentWindow command format. </value>
    Protected Overrides ReadOnly Property AveragePercentWindowCommandFormat As String = ":SENS:FRES:AVER:WIND {0}"

    ''' <summary> Gets the Average Percent Window query command. </summary>
    ''' <value> The AveragePercentWindow query command. </value>
    Protected Overrides ReadOnly Property AveragePercentWindowQueryCommand As String = ":SENS:FRES:AVER:WIND?"

#End Region

#Region " AUTO ZERO "

    ''' <summary> Gets the automatic Zero enabled command Format. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledCommandFormat As String = ":SENS:FRES:AZER {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the automatic Zero enabled query command. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledQueryCommand As String = ":SENS:FRES:AZER?"

#End Region

#Region " AUTO RANGE "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = ":SENS:FRES:RANG:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = ":SENS:FRES:RANG:AUTO?"

#End Region

#Region " POWER LINE CYCLES "

    ''' <summary> Gets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesCommandFormat As String = ":SENS:FRES:NPLC {0}"

    ''' <summary> Gets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesQueryCommand As String = ":SENS:FRES:NPLC?"

#End Region

#Region " RANGE "

    ''' <summary> The maximum Kelvin test resistance. </summary>
    Public Const MaximumKelvinTestResistance As Double = 2100000.0

    ''' <summary> Determine if we can use Kelvin (4-wire) measurements. </summary>
    ''' <param name="resistance"> The resistance. </param>
    ''' <returns> <c>true</c> if we can use Kelvin; otherwise <c>false</c> </returns>
    Public Shared Function CanUseKelvin(ByVal resistance As Double) As Boolean
        Return resistance <= MaximumKelvinTestResistance
    End Function


    ''' <summary> Gets the range command format. </summary>
    ''' <value> The range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = ":SENS:FRES:RANG {0}"

    ''' <summary> Gets the range query command. </summary>
    ''' <value> The range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = ":SENS:FRES:RANG?"

#End Region

#End Region

#Region " CURRENT "

    ''' <summary> Range current. </summary>
    ''' <param name="range"> The range. </param>
    ''' <returns> The current for the specified range. </returns>
    Public Shared Function RangeCurrent(ByVal range As Double) As Decimal
        Dim i As Decimal = 0.001D
        Select Case range
            Case Is <= 1
                i = 0.01D
            Case Is <= 10
                i = 0.01D
            Case Is <= 100
                i = 0.001D
            Case Is <= 1000
                i = 0.001D
            Case Is <= 10000
                i = 0.0001D
            Case Is <= 100000
                i = 0.00001D
            Case Is <= 1000000
                i = 0.00001D
            Case Is <= 10000000.0
                i = 0.00000069D
            Case Is <= 100000000.0
                i = 0.00000069D
            Case Else
                i = 0.00000069D
        End Select
        Return i
    End Function

    ''' <summary> Gets the current. </summary>
    ''' <value> The current. </value>
    Public Overrides ReadOnly Property Current As Decimal
        Get
            Return SenseFourWireResistanceSubsystem.RangeCurrent(Me.Range.GetValueOrDefault(100))
        End Get
    End Property

#End Region

#Region " RANGE "

    ''' <summary> Try convert. </summary>
    ''' <param name="range">     The range. </param>
    ''' <param name="rangeMode"> [in,out] The range mode. </param>
    ''' <returns> <c>True</c> if converted; Otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryConvert(ByVal range As Double, ByRef rangeMode As FourWireResistanceRangeMode) As Boolean
        Dim value As Double
        Dim affirmative As Boolean = False
        For Each e As FourWireResistanceRangeMode In [Enum].GetValues(GetType(FourWireResistanceRangeMode))
            If SenseFourWireResistanceSubsystem.TryConvert(e, value) AndAlso Math.Abs(value - range) < 0.000000001 Then
                affirmative = True
                rangeMode = e
                Exit For
            End If
        Next
        Return affirmative
    End Function

    ''' <summary> Try convert range mode to range. </summary>
    ''' <param name="rangeMode"> The function mode. </param>
    ''' <param name="range">     [in,out] The range. </param>
    ''' <returns> <c>True</c> if converted; Otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function TryConvert(ByVal rangeMode As FourWireResistanceRangeMode, ByRef range As Double) As Boolean
        range = CLng(rangeMode)
        Return True
    End Function

    ''' <summary> Try convert. </summary>
    ''' <param name="rangeMode"> The range mode. </param>
    ''' <param name="range">     [in,out] The range. </param>
    ''' <returns> <c>True</c> if converted; Otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function TryConvert(ByVal rangeMode As ResistanceRangeMode, ByRef range As FourWireResistanceRangeMode) As Boolean
        Dim r As Double = 0
        Return SenseResistanceSubsystem.TryConvert(rangeMode, r) AndAlso SenseFourWireResistanceSubsystem.TryConvert(r, range)
    End Function

#End Region

End Class
