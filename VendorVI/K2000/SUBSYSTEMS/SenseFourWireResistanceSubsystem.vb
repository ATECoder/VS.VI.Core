''' <summary> Defines a SCPI Four Wire Sense Resistance Subsystem for a Keithley 2000 instrument. </summary>
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

#Region " AVERAGE ENABLED "

    ''' <summary> Gets the Average enabled command Format. </summary>
    ''' <value> The Average enabled query command. </value>
    Protected Overrides ReadOnly Property AverageEnabledCommandFormat As String = ":SENS:FRES:AVER:STAT {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the Average enabled query command. </summary>
    ''' <value> The Average enabled query command. </value>
    Protected Overrides ReadOnly Property AverageEnabledQueryCommand As String = ":SENS:FRES:AVER:STAT?"

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
        Dim i As Decimal = 0.0000014D
        Select Case range
            Case Is <= 20
                i = 0.0072D
            Case Is <= 200
                i = 0.00096D
            Case Is <= 2000
                i = 0.00096D
            Case Is <= 20000
                i = 0.000096D
            Case Is <= 200000
                i = 0.0000096D
            Case Is <= 2000000
                i = 0.0000019D
            Case Is <= 20000000.0
                i = 0.0000014D
            Case Is <= 200000000.0
                i = 0.0000014D
            Case Else
                i = 0.0000014D
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
