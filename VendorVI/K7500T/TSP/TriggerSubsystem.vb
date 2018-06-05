''' <summary> Defines a Trigger Subsystem for a Keithley 7500 Meter. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class TriggerSubsystem
    Inherits VI.Tsp2.TriggerSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="TriggerSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.SupportedTriggerSources = Tsp2.TriggerSources.Bus Or Tsp2.TriggerSources.External Or
                                     Tsp2.TriggerSources.Hold Or Tsp2.TriggerSources.Immediate Or
                                     Tsp2.TriggerSources.Manual Or Tsp2.TriggerSources.Timer Or
                                     Tsp2.TriggerSources.TriggerLink
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

#Region " SIMPLE LOOP "

    Private Const SimpleLoopModel As String = "SimpleLoop"

    ''' <summary> Loads simple loop. </summary>
    ''' <param name="count"> Number of. </param>
    ''' <param name="delay"> The delay. </param>
    Public Sub LoadSimpleLoop(ByVal count As Integer, ByVal delay As TimeSpan)
        Me.Write("trigger.mode.load('{0}',{1},{2})", SimpleLoopModel, count, delay.TotalSeconds)
    End Sub

#End Region

#Region " GRAD BINNING "

    Private Const GradeBinningModel As String = "GradeBinning"

    ''' <summary> Loads grade binning. </summary>
    ''' <param name="count">            Number of. </param>
    ''' <param name="triggerOption">    The trigger option (use 7 for external line). </param>
    ''' <param name="startDelay">       The start delay. </param>
    ''' <param name="endDelay">         The end delay. </param>
    ''' <param name="highLimit">        The high limit. </param>
    ''' <param name="lowLimit">         The low limit. </param>
    ''' <param name="failedBitPattern"> A pattern specifying the failed bit. </param>
    ''' <param name="passBitPattern">   A pattern specifying the pass bit. </param>
    Public Sub LoadGradeBinning(ByVal count As Integer, ByVal triggerOption As Integer,
                                ByVal startDelay As TimeSpan, ByVal endDelay As TimeSpan,
                                ByVal highLimit As Double, ByVal lowLimit As Double,
                                ByVal failedBitPattern As Integer, ByVal passBitPattern As Integer)
        Me.Write("trigger.mode.load('{0}',{1},{2},{3},{4},{5},{6},{7},{8})",
                 GradeBinningModel, count, triggerOption, startDelay.TotalSeconds,
                 endDelay.TotalSeconds, highLimit, lowLimit, failedBitPattern, passBitPattern)
    End Sub

    ''' <summary> Loads grade binning. </summary>
    ''' <remarks> Uses external trigger (start line = 7). </remarks>
    ''' <param name="count">            Number of. </param>
    ''' <param name="startDelay">       The start delay. </param>
    ''' <param name="endDelay">         The end delay. </param>
    ''' <param name="highLimit">        The high limit. </param>
    ''' <param name="lowLimit">         The low limit. </param>
    ''' <param name="failedBitPattern"> A pattern specifying the failed bit. </param>
    ''' <param name="passBitPattern">   A pattern specifying the pass bit. </param>
    Public Sub LoadGradeBinning(ByVal count As Integer, ByVal startDelay As TimeSpan, ByVal endDelay As TimeSpan,
                                ByVal highLimit As Double, ByVal lowLimit As Double,
                                ByVal failedBitPattern As Integer, ByVal passBitPattern As Integer)
        Me.LoadGradeBinning(count, 7, startDelay, endDelay, highLimit, lowLimit, failedBitPattern, passBitPattern)
    End Sub

    ''' <summary> Loads grade binning. </summary>
    ''' <remarks> Uses external trigger (start line = 7) and maximum count (268000000). </remarks>
    ''' <param name="startDelay">       The start delay. </param>
    ''' <param name="endDelay">         The end delay. </param>
    ''' <param name="highLimit">        The high limit. </param>
    ''' <param name="lowLimit">         The low limit. </param>
    ''' <param name="failedBitPattern"> A pattern specifying the failed bit. </param>
    ''' <param name="passBitPattern">   A pattern specifying the pass bit. </param>
    Public Sub LoadGradeBinning(ByVal startDelay As TimeSpan, ByVal endDelay As TimeSpan,
                                ByVal highLimit As Double, ByVal lowLimit As Double,
                                ByVal failedBitPattern As Integer, ByVal passBitPattern As Integer)
        Me.LoadGradeBinning(268000000, 7, startDelay, endDelay, highLimit, lowLimit, failedBitPattern, passBitPattern)
    End Sub

#End Region

#Region " CUSTOM BINNING "

    ''' <summary> Applies the grade binning. </summary>
    ''' <param name="count">            Number of. </param>
    ''' <param name="startDelay">       The start delay. </param>
    ''' <param name="failedBitPattern"> A pattern specifying the failed bit. </param>
    ''' <param name="passBitPattern">   A pattern specifying the pass bit. </param>
    Public Sub ApplyGradeBinning(ByVal count As Integer, ByVal startDelay As TimeSpan,
                                 ByVal failedBitPattern As Integer, ByVal passBitPattern As Integer)

        Dim block As Integer = 0
        Dim cmd As String = ""

        ' use the mask to assign digital outputs.
        Dim mask As Integer = failedBitPattern Or passBitPattern
        Dim bitPattern As Integer = 1
        For i As Integer = 1 To 6
            If (mask And bitPattern) <> 0 Then
                cmd = $"digio.line[{i}].mode=digio.MODE_DIGIIAL_OUT"
                Me.Write(cmd)
            End If
            bitPattern <<= 1
        Next

        ' clear the trigger model
        cmd = "trigger.model.load('Empty')" : Me.Write(cmd)

        ' clear any pending trigger
        cmd = "trigger.extin.clear()" : Me.Write(cmd)

        ' clear the default buffer
        cmd = "defbuffer1.clear()" : Me.Write(cmd)

        Dim autonomous As Boolean = count = 1

        ' Block 1: 
        ' -- if autonomous mode, clear the buffer
        ' -- if program control, clear the buffer
        block += 1 : cmd = If(autonomous, $"trigger.model.setblock({block},trigger.BLOCK_BUFFER_CLEAR)", $"trigger.model.setblock({block},trigger.BLOCK_NOP)")
        Me.Write(cmd)

        ' Block 2: Wait for external trigger; this is the repeat block.
        Dim repeatBlock As Integer = block + 1
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_EXTERNAL)"
        Me.Write(cmd)

        ' Block 3: Pre-Measure Delay
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DELAY_CONSTANT, {0.001 * startDelay.TotalMilliseconds})"
        Me.Write(cmd)

        ' Block 4: Measure and save to the default buffer.
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_MEASURE)"
        Me.Write(cmd)

        ' Block 5: Limit test and branch; the pass block is 3 blocks ahead (8)
        Dim passBlock As Integer = block + 4
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_LIMIT_DYNAMIC, trigger.LIMIT_INSIDE, 1, {passBlock})"
        Me.Write(cmd)

        ' Block 6: Output failure bit pattern
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DIGITAL_IO, {failedBitPattern},{mask})"
        Me.Write(cmd)

        ' Block 7: Skip the pass binning block
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_ALWAYS, {block + 2})"
        Me.Write(cmd)

        ' Block 8: Output pass bit pattern
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DIGITAL_IO, {passBitPattern},{mask})"
        Me.Write(cmd)

        Dim notificationId As Integer = 1

        ' Block 9: Notify measurement completed
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_NOTIFY, trigger.EVENT_NOTIFY{notificationId})"
        Me.Write(cmd)

        ' set external output setting
        cmd = $"trigger.extout.stimulus=trigger.EVENT_NOTIFY{notificationId}"
        Me.Write(cmd)

        ' Block 10: Repeat Count times.
        block += 1 : cmd = If(count <= 0, $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_ALWAYS, {repeatBlock})",
                                          $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_COUNTER, {count},{repeatBlock})")
        Me.Write(cmd)

    End Sub

    Public Sub ApplyGradeBinning(ByVal count As Integer, ByVal startDelay As TimeSpan,
                                 ByVal failedBitPattern As Integer, ByVal passBitPattern As Integer,
                                 ByVal openContactBitPattern As Integer, ByVal triggerSource As Scpi.TriggerSources)

        Dim block As Integer = 0
        Dim cmd As String = ""

        ' use the mask to assign digital outputs.
        Dim mask As Integer = failedBitPattern Or passBitPattern Or openContactBitPattern
        Dim bitPattern As Integer = 1
        For i As Integer = 1 To 6
            If (mask And bitPattern) <> 0 Then
                cmd = $"digio.line[{i}].mode=digio.MODE_DIGIIAL_OUT"
                Me.Write(cmd)
            End If
            bitPattern <<= 1
        Next

        ' clear the trigger model
        Me.ClearTriggerModel()

        ' clear any pending trigger
        Me.ClearTriggers()

        Dim autonomous As Boolean = count = 1

        ' Block 1: 
        ' -- if autonomous mode, clear the buffer
        ' -- if program control, clear the buffer
        block += 1 : cmd = If(autonomous, $"trigger.model.setblock({block},trigger.BLOCK_BUFFER_CLEAR)", $"trigger.model.setblock({block},trigger.BLOCK_NOP)")
        Me.Write(cmd)

        ' Block 2: Wait for external trigger; this is the repeat block.
        Dim repeatBlock As Integer = block + 1
        block += 1
        cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_EXTERNAL)"
        If 0 <> (triggerSource And Tsp2.TriggerSources.Bus) Then
            cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_COMMAND)"
        ElseIf 0 <> (triggerSource And Tsp2.TriggerSources.Manual) Then
            cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_DISPLAY)"
        ElseIf 0 <> (triggerSource And Tsp2.TriggerSources.TriggerLink) Then
            cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_TSPLINK1)"
        ElseIf 0 <> (triggerSource And Tsp2.TriggerSources.Timer) Then
            cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_TIMER1)"
        ElseIf 0 <> (triggerSource And Tsp2.TriggerSources.Digital) Then
            cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_DIGIO1)"
        End If
        Me.Write(cmd)

        ' Block 3: Pre-Measure Delay
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DELAY_CONSTANT, {0.001 * startDelay.TotalMilliseconds})"
        Me.Write(cmd)

        ' Block 4: Measure and save to the default buffer.
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_MEASURE)"
        Me.Write(cmd)

        ' Block 5: Limit 2 (open contact) test and branch if INside to the limit block; the limit 1 block is 3 blocks ahead (8)
        Dim limitBlock As Integer = block + 4
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_LIMIT_DYNAMIC, trigger.LIMIT_INSIDE, 2, {limitBlock})"
        Me.Write(cmd)

        ' Block 6: Output open bit pattern
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DIGITAL_IO, {openContactBitPattern},{mask})"
        Me.Write(cmd)

        ' Block 7: Jump to the notification block number
        Dim notificationBlock As Integer = block + 6 ' = 12
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_ALWAYS, {notificationBlock})"
        Me.Write(cmd)

        ' Block 8: Limit 1 test and branch if INside to the pass block; the pass block is 3 blocks ahead
        Dim passBlock As Integer = block + 4 ' = 11
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_LIMIT_DYNAMIC, trigger.LIMIT_INSIDE, 1, {passBlock})"
        Me.Write(cmd)

        ' Block 9: Output failure bit pattern
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DIGITAL_IO, {failedBitPattern},{mask})"
        Me.Write(cmd)

        ' Block 10: Jump to the notification block
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_ALWAYS, {notificationBlock})"
        Me.Write(cmd)

        ' Block 11: Output pass bit pattern
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DIGITAL_IO, {passBitPattern},{mask})"
        Me.Write(cmd)

        Dim notificationId As Integer = 1

        ' Block 12: Notify measurement completed
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_NOTIFY, trigger.EVENT_NOTIFY{notificationId})"
        Me.Write(cmd)

        ' set external output setting
        cmd = $"trigger.extout.stimulus=trigger.EVENT_NOTIFY{notificationId}"
        Me.Write(cmd)

        ' Block 13: Repeat Count times.
        block += 1 : cmd = If(count <= 0, $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_ALWAYS, {repeatBlock})",
                                          $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_COUNTER, {count},{repeatBlock})")
        Me.Write(cmd)

    End Sub

    Public Sub ApplyMeterCompleteFirstGradeBinning(ByVal count As Integer, ByVal startDelay As TimeSpan,
                                                   ByVal failedBitPattern As Integer, ByVal passBitPattern As Integer,
                                                   ByVal openContactBitPattern As Integer, ByVal triggerSource As Scpi.TriggerSources)

        Dim block As Integer = 0
        Dim cmd As String = ""

        ' use the mask to assign digital outputs.
        Dim mask As Integer = failedBitPattern Or passBitPattern Or openContactBitPattern
        Dim bitPattern As Integer = 1
        For i As Integer = 1 To 6
            If (mask And bitPattern) <> 0 Then
                cmd = $"digio.line[{i}].mode=digio.MODE_DIGIIAL_OUT"
                Me.Write(cmd)
            End If
            bitPattern <<= 1
        Next

        ' clear the trigger model
        Me.ClearTriggerModel()

        ' clear any pending trigger
        Me.ClearTriggers()

        Dim autonomous As Boolean = count = 1

        ' Block 1: 
        ' -- if autonomous mode, clear the buffer
        ' -- if program control, clear the buffer
        block += 1 : cmd = If(autonomous, $"trigger.model.setblock({block},trigger.BLOCK_BUFFER_CLEAR)", $"trigger.model.setblock({block},trigger.BLOCK_NOP)")
        Me.Write(cmd)

        Dim notificationId As Integer = 1

        ' Block 2: Notify measurement completed; this is the loop start block.
        Dim loopStartBlock As Integer = block + 1
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_NOTIFY, trigger.EVENT_NOTIFY{notificationId})"
        Me.Write(cmd)

        ' set external output setting
        cmd = $"trigger.extout.stimulus=trigger.EVENT_NOTIFY{notificationId}"
        Me.Write(cmd)

        ' Block 3: Wait for external trigger
        block += 1
        cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_EXTERNAL)"
        If 0 <> (triggerSource And Tsp2.TriggerSources.Bus) Then
            cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_COMMAND)"
        ElseIf 0 <> (triggerSource And Tsp2.TriggerSources.Manual) Then
            cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_DISPLAY)"
        ElseIf 0 <> (triggerSource And Tsp2.TriggerSources.TriggerLink) Then
            cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_TSPLINK1)"
        ElseIf 0 <> (triggerSource And Tsp2.TriggerSources.Timer) Then
            cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_TIMER1)"
        ElseIf 0 <> (triggerSource And Tsp2.TriggerSources.Digital) Then
            cmd = $"trigger.model.setblock({block}, trigger.BLOCK_WAIT, trigger.EVENT_DIGIO1)"
        End If
        Me.Write(cmd)

        ' Block 4: Pre-Measure Delay
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DELAY_CONSTANT, {0.001 * startDelay.TotalMilliseconds})"
        Me.Write(cmd)

        ' Block 5: Measure and save to the default buffer.
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_MEASURE)"
        Me.Write(cmd)

        ' Block 6: Limit 2 (open contact) test and branch if INside to the limit block; the limit 1 block is 3 blocks ahead (8)
        Dim limitBlock As Integer = block + 4
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_LIMIT_DYNAMIC, trigger.LIMIT_INSIDE, 2, {limitBlock})"
        Me.Write(cmd)

        ' Block 7: Output open bit pattern
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DIGITAL_IO, {openContactBitPattern},{mask})"
        Me.Write(cmd)

        ' Block 8: Jump to the loop back block number
        Dim loopBackBlock As Integer = block + 6 ' = 12
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_ALWAYS, {loopBackBlock})"
        Me.Write(cmd)

        ' Block 8: Limit 1 test and branch if INside to the pass block; the pass block is 3 blocks ahead
        Dim passBlock As Integer = block + 4 ' = 11
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_LIMIT_DYNAMIC, trigger.LIMIT_INSIDE, 1, {passBlock})"
        Me.Write(cmd)

        ' Block 9: Output failure bit pattern
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DIGITAL_IO, {failedBitPattern},{mask})"
        Me.Write(cmd)

        ' Block 10: Jump to the loop back block
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_ALWAYS, {loopBackBlock})"
        Me.Write(cmd)

        ' Block 11: Output pass bit pattern
        block += 1 : cmd = $"trigger.model.setblock({block}, trigger.BLOCK_DIGITAL_IO, {passBitPattern},{mask})"
        Me.Write(cmd)

        ' Block 13: Repeat Count times; Jump to the loop start block
        block += 1 : cmd = If(count <= 0, $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_ALWAYS, {loopStartBlock})",
                                          $"trigger.model.setblock({block}, trigger.BLOCK_BRANCH_COUNTER, {count},{loopStartBlock})")
        Me.Write(cmd)

    End Sub

#End Region

End Class
