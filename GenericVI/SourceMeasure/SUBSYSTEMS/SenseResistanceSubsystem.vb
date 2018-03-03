''' <summary> Defines a SCPI Sense Resistance Subsystem for a generic Source Measure instrument. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class SenseResistanceSubsystem
    Inherits VI.Scpi.SenseResistanceSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseResistanceSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.01, 10)
        Me.FunctionRange = New isr.Core.Pith.RangeR(0, 21000000.0)
    End Sub

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.01, 10)
        Dim model As String = Me.StatusSubsystem.VersionInfo.Model
        Select Case True
            Case model.StartsWith("2400", StringComparison.OrdinalIgnoreCase)
                Me.FunctionRange = New isr.Core.Pith.RangeR(0, 210000000.0)
            Case model.StartsWith("2410", StringComparison.OrdinalIgnoreCase)
                Me.FunctionRange = New isr.Core.Pith.RangeR(0, 210000000.0)
            Case model.StartsWith("242", StringComparison.OrdinalIgnoreCase)
                Me.FunctionRange = New isr.Core.Pith.RangeR(0, 21000000.0)
            Case model.StartsWith("243", StringComparison.OrdinalIgnoreCase)
                Me.FunctionRange = New isr.Core.Pith.RangeR(0, 21000000.0)
            Case model.StartsWith("244", StringComparison.OrdinalIgnoreCase)
                Me.FunctionRange = New isr.Core.Pith.RangeR(0, 21000000.0)
            Case Else
                Me.FunctionRange = New isr.Core.Pith.RangeR(0, 21000000.0)
        End Select
    End Sub

#End Region


#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " AUTO RANGE "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = ":SENS:RES:RANG:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = ":SENS:RES:RANG:AUTO?"

#End Region

#Region " POWER LINE CYCLES "

    ''' <summary> Gets The Power Line Cycles command format. </summary>
    ''' <value> The Power Line Cycles command format. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesCommandFormat As String = ":SENS:RES:NPLC {0}"

    ''' <summary> Gets The Power Line Cycles query command. </summary>
    ''' <value> The Power Line Cycles query command. </value>
    Protected Overrides ReadOnly Property PowerLineCyclesQueryCommand As String = ":SENS:RES:NPLC?"

#End Region

#Region " RANGE "

    ''' <summary> Gets the range command format. </summary>
    ''' <value> The range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = ":SENS:RES:RANG {0}"

    ''' <summary> Gets the range query command. </summary>
    ''' <value> The range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = ":SENS:RES:RANG?"

#End Region

#Region " CONFIGURATION MODE "

    ''' <summary> Gets the Configuration Mode query command. </summary>
    ''' <value> The Configuration Mode query command. </value>
    Protected Overrides ReadOnly Property ConfigurationModeQueryCommand As String = ":SENS:RES:MODE?"

    ''' <summary> Gets the Configuration Mode command format. </summary>
    ''' <value> The Configuration Mode command format. </value>
    Protected Overrides ReadOnly Property ConfigurationModeCommandFormat As String = ":SENS:RES:MODE {0}"

#End Region

#End Region

#Region " CURRENT "

    ''' <summary> Range current (based on 2400 and 2410). </summary>
    ''' <param name="range"> The range. </param>
    ''' <returns> The current for the specified range. </returns>
    Public Shared Function RangeCurrent(ByVal range As Double) As Decimal
        Dim i As Decimal = 0.0000014D
        Select Case range
            Case Is <= 20
                i = 0.1D
            Case Is <= 200
                i = 0.01D
            Case Is <= 2000
                i = 0.001D
            Case Is <= 20000
                i = 0.0001D
            Case Is <= 200000
                i = 0.00001D
            Case Is <= 2000000
                i = 0.000001D
            Case Is <= 20000000.0
                i = 0.000001D
            Case Is <= 200000000.0
                i = 0.00000014D
            Case Else
                i = 0.00000014D
        End Select
        Return i
    End Function

    ''' <summary> Gets the current. </summary>
    ''' <value> The current. </value>
    Public Overrides ReadOnly Property Current As Decimal
        Get
            Return SenseResistanceSubsystem.RangeCurrent(Me.Range.GetValueOrDefault(100))
        End Get
    End Property

#End Region

#Region " RANGE "

    ''' <summary> Find range match. The matched range is the first range with a range value
    ''' greater or equal to the specified range. </summary>
    ''' <param name="range">     The range. </param>
    ''' <param name="rangeMode"> [in,out] The range mode. </param>
    ''' <returns> <c>True</c> if a match was found; Otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function TryMatch(ByVal range As Double, ByRef rangeMode As ResistanceRangeMode) As Boolean
        Dim value As Double
        Dim affirmative As Boolean = False
        For Each e As ResistanceRangeMode In [Enum].GetValues(GetType(ResistanceRangeMode))
            If SenseResistanceSubsystem.TryConvert(e, value) AndAlso value >= range Then
                affirmative = True
                rangeMode = e
                Exit For
            End If
        Next
        Return affirmative
    End Function

    ''' <summary> Try convert. </summary>
    ''' <param name="range">     The range. </param>
    ''' <param name="rangeMode"> [in,out] The range mode. </param>
    ''' <returns> <c>True</c> if converted; Otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryConvert(ByVal range As Double, ByRef rangeMode As ResistanceRangeMode) As Boolean
        Dim value As Double
        Dim affirmative As Boolean = False
        For Each e As ResistanceRangeMode In [Enum].GetValues(GetType(ResistanceRangeMode))
            If SenseResistanceSubsystem.TryConvert(e, value) AndAlso Math.Abs(value - range) < 0.000000001 Then
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
    Public Shared Function TryConvert(ByVal rangeMode As ResistanceRangeMode, ByRef range As Double) As Boolean
        range = CDbl(rangeMode)
        Return True
    End Function

#End Region

End Class
