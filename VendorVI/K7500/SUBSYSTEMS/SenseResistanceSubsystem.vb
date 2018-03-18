Imports isr.VI.K7500
''' <summary> Defines a SCPI Sense Resistance Subsystem for a Keithley 7500 Meter. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="03/01/2014" by="David" revision="3.0.5173"> Created. </history>
Public Class SenseResistanceSubsystem
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
        Me.FunctionRange = New isr.Core.Pith.RangeR(10, 1000000000.0)
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        If Me.StatusSubsystem.LineFrequency.GetValueOrDefault(60) = 60 Then
            Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 15)
        Else
            Me.PowerLineCyclesRange = New isr.Core.Pith.RangeR(0.0005, 12)
        End If
        Me.FunctionRange = New isr.Core.Pith.RangeR(10, 1000000000.0)
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

#Region " AUTO ZERO "

    ''' <summary> Gets the automatic Zero enabled command Format. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledCommandFormat As String = ":SENS:RES:AZER {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the automatic Zero enabled query command. </summary>
    ''' <value> The automatic Zero enabled query command. </value>
    Protected Overrides ReadOnly Property AutoZeroEnabledQueryCommand As String = ":SENS:RES:AZER?"

#End Region

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

#End Region

#Region " RANGES: LEVEL, CURRENT "

    Private Shared _ResistanceRanges As ResistanceRangeCollection
    ''' <summary> Gets the resistance ranges. </summary>
    ''' <value> The resistance ranges. </value>
    Public Shared ReadOnly Property ResistanceRanges As ResistanceRangeCollection
        Get
            If SenseResistanceSubsystem._ResistanceRanges Is Nothing Then
                SenseResistanceSubsystem._ResistanceRanges = New ResistanceRangeCollection
            End If
            Return SenseResistanceSubsystem._ResistanceRanges
        End Get
    End Property

    ''' <summary> Gets the current. </summary>
    ''' <value> The current. </value>
    Public Overrides ReadOnly Property Current As Decimal
        Get
            Return CDec(SenseResistanceSubsystem.ResistanceRanges.FindResistanceRange(Me.Range.GetValueOrDefault(100)).Current)
        End Get
    End Property

#End Region

End Class

''' <summary> A resistance range. </summary>
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
Public Structure ResistanceRange
    Public Sub New(ByVal mode As ResistanceRangeMode, ByVal current As Double, ByVal range As Double)
        Me.Mode = mode
        Me.Current = current
        Me.Range = range
    End Sub
    Public Property Mode As ResistanceRangeMode
    Public Property Current As Double
    Public Property Range As Double
    Public ReadOnly Property IsAutoRange As Boolean
        Get
            Return Me.Mode = ResistanceRangeMode.R0
        End Get
    End Property
    Public Shared ReadOnly Property AutoRange As ResistanceRange
        Get
            Return New ResistanceRange(ResistanceRangeMode.R0, 0, 0)
        End Get
    End Property
End Structure

''' <summary> Collection of resistance ranges. </summary>
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
Public Class ResistanceRangeCollection
    Inherits Collections.ObjectModel.KeyedCollection(Of ResistanceRangeMode, ResistanceRange)

    Protected Overrides Function GetKeyForItem(item As ResistanceRange) As ResistanceRangeMode
        Return item.Mode
    End Function

    Public Sub New()
        Me.Populate()
    End Sub

    Private Sub Populate()
        Me.Clear()
        Me.Add(ResistanceRange.AutoRange)
        Me.Add(New ResistanceRange(ResistanceRangeMode.R20, 0.0072, 20))
        Me.Add(New ResistanceRange(ResistanceRangeMode.R200, 0.00096, 200))
        Me.Add(New ResistanceRange(ResistanceRangeMode.R2000, 0.00096, 2000))
        Me.Add(New ResistanceRange(ResistanceRangeMode.R20000, 0.000096, 20000))
        Me.Add(New ResistanceRange(ResistanceRangeMode.R200000, 0.0000096, 200000))
        Me.Add(New ResistanceRange(ResistanceRangeMode.R2000000, 0.0000019, 2000000))
        Me.Add(New ResistanceRange(ResistanceRangeMode.R20000000, 0.0000014, 20000000))
        Me.Add(New ResistanceRange(ResistanceRangeMode.R200000000, 0.0000014, 200000000))
        Me.Add(New ResistanceRange(ResistanceRangeMode.R1000000000, 0.0000014, 1000000000))
    End Sub

    ''' <summary> Searches for the first resistance range. </summary>
    ''' <param name="range"> The range. </param>
    ''' <returns> The found resistance range. </returns>
    Public Function FindResistanceRange(ByVal range As Double) As ResistanceRange
        Dim result As ResistanceRange = ResistanceRange.AutoRange
        For Each resistanceRange As ResistanceRange In Me
            If range <= resistanceRange.Range Then
                result = resistanceRange
                Exit For
            End If
        Next
        Return result
    End Function

End Class

''' <summary> Values that represent the resistance range mode. </summary>
Public Enum ResistanceRangeMode
    <ComponentModel.Description("Auto Range (R0)")> R0 = 0
    <ComponentModel.Description("20 ohm range @ 7.2 mA Test Current")> R20 = 20
    <ComponentModel.Description("200 ohm range @ 960 uA Test Current")> R200 = 200
    <ComponentModel.Description("2k ohm range @ 960 uA Test Current")> R2000 = 2000
    <ComponentModel.Description("20k ohm range @ 96 uA Test Current")> R20000 = 20000
    <ComponentModel.Description("200k ohm range @ 9.6 uA Test Current")> R200000 = 200000
    <ComponentModel.Description("2M ohm range @ 1.9 uA Test Current")> R2000000 = 2000000
    <ComponentModel.Description("20M ohm range @ 1.4 uA Test Current")> R20000000 = 20000000
    <ComponentModel.Description("200M ohm range @ 1.4 uA Test Current")> R200000000 = 200000000
    <ComponentModel.Description("1G ohm range @ 1.4 uA Test Current")> R1000000000 = 1000000000
End Enum

