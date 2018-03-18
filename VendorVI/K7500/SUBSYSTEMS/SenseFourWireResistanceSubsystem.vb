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
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        If Me.StatusSubsystem.LineFrequency.GetValueOrDefault(60) = 60 Then
            Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 15)
        Else
            Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 12)
        End If
        Me.FunctionRange = New isr.Core.Pith.RangeR(1, 1000000000.0)
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        If Me.StatusSubsystem.LineFrequency.GetValueOrDefault(60) = 60 Then
            Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 15)
        Else
            Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 12)
        End If
        Me.FunctionRange = New isr.Core.Pith.RangeR(1, 1000000000.0)
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

#Region " OPEN LEAD DETECTOR "

    ''' <summary> Gets the Open Lead Detector enabled command Format. </summary>
    ''' <value> The Open Lead Detector enabled query command. </value>
    Protected Overrides ReadOnly Property OpenLeadDetectorEnabledCommandFormat As String = ":SENS:FRES:ODET {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the Open Lead Detector enabled query command. </summary>
    ''' <value> The Open Lead Detector enabled query command. </value>
    Protected Overrides ReadOnly Property OpenLeadDetectorEnabledQueryCommand As String = ":SENS:FRES:ODET?"

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

#Region " RANGES: LEVEL, CURRENT "

    Private Shared _FourWireResistanceRanges As FourWireResistanceRangeCollection
    ''' <summary> Gets the FourWireResistance ranges. </summary>
    ''' <value> The FourWireResistance ranges. </value>
    Public Shared ReadOnly Property FourWireResistanceRanges As FourWireResistanceRangeCollection
        Get
            If SenseFourWireResistanceSubsystem._FourWireResistanceRanges Is Nothing Then
                SenseFourWireResistanceSubsystem._FourWireResistanceRanges = New FourWireResistanceRangeCollection
            End If
            Return SenseFourWireResistanceSubsystem._FourWireResistanceRanges
        End Get
    End Property

    ''' <summary> Gets the current. </summary>
    ''' <value> The current. </value>
    Public Overrides ReadOnly Property Current As Decimal
        Get
            Return CDec(SenseFourWireResistanceSubsystem.FourWireResistanceRanges.FindFourWireResistanceRange(Me.Range.GetValueOrDefault(100)).Current)
        End Get
    End Property

#End Region

End Class

''' <summary> A FourWireResistance range. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
<CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")>
Public Structure FourWireResistanceRange
    Public Sub New(ByVal mode As FourWireResistanceRangeMode, ByVal current As Double, ByVal range As Double)
        Me.Mode = mode
        Me.Current = current
        Me.Range = range
    End Sub
    Public Property Mode As FourWireResistanceRangeMode
    Public Property Current As Double
    Public Property Range As Double
    Public ReadOnly Property IsAutoRange As Boolean
        Get
            Return Me.Mode = FourWireResistanceRangeMode.R0
        End Get
    End Property
    Public Shared ReadOnly Property AutoRange As FourWireResistanceRange
        Get
            Return New FourWireResistanceRange(FourWireResistanceRangeMode.R0, 0, 0)
        End Get
    End Property
End Structure

''' <summary> Collection of FourWireResistance ranges. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="3/5/2018" by="David" revision=""> Created. </history>
Public Class FourWireResistanceRangeCollection
    Inherits Collections.ObjectModel.KeyedCollection(Of FourWireResistanceRangeMode, FourWireResistanceRange)

    Protected Overrides Function GetKeyForItem(item As FourWireResistanceRange) As FourWireResistanceRangeMode
        Throw New NotImplementedException()
    End Function

    Public Sub New()
        Me.Populate()
    End Sub

    Private Sub Populate()
        Me.Clear()
        Me.Add(FourWireResistanceRange.AutoRange)
        Me.Add(New FourWireResistanceRange(FourWireResistanceRangeMode.R20, 0.0072, 20))
        Me.Add(New FourWireResistanceRange(FourWireResistanceRangeMode.R200, 0.00096, 200))
        Me.Add(New FourWireResistanceRange(FourWireResistanceRangeMode.R2000, 0.00096, 2000))
        Me.Add(New FourWireResistanceRange(FourWireResistanceRangeMode.R20000, 0.000096, 20000))
        Me.Add(New FourWireResistanceRange(FourWireResistanceRangeMode.R200000, 0.0000096, 200000))
        Me.Add(New FourWireResistanceRange(FourWireResistanceRangeMode.R2000000, 0.0000019, 2000000))
    End Sub

    ''' <summary> Searches for the first FourWireResistance range. </summary>
    ''' <param name="range"> The range. </param>
    ''' <returns> The found FourWireResistance range. </returns>
    Public Function FindFourWireResistanceRange(ByVal range As Double) As FourWireResistanceRange
        Dim result As FourWireResistanceRange = FourWireResistanceRange.AutoRange
        For Each FourWireResistanceRange As FourWireResistanceRange In Me
            If range <= FourWireResistanceRange.Range Then
                result = FourWireResistanceRange
                Exit For
            End If
        Next
        Return result
    End Function

End Class

''' <summary> Values that represent the four-wire resistance range mode. </summary>
Public Enum FourWireResistanceRangeMode
    <ComponentModel.Description("Auto Range (R0)")> R0 = 0
    <ComponentModel.Description("20 ohm range @ 7.2 mA Test Current")> R20 = 20
    <ComponentModel.Description("200 ohm range @ 960 uA Test Current")> R200 = 200
    <ComponentModel.Description("2k ohm range @ 960 uA Test Current")> R2000 = 2000
    <ComponentModel.Description("20k ohm range @ 96 uA Test Current")> R20000 = 20000
    <ComponentModel.Description("200k ohm range @ 9.6 uA Test Current")> R200000 = 200000
    <ComponentModel.Description("2M ohm range @ 1.9 uA Test Current")> R2000000 = 2000000
End Enum

