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
    Inherits VI.Scpi.TriggerSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="TriggerSubsystem" /> class. </summary>
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

#Region " ABORT / INIT COMMANDS "

    ''' <summary> Gets the Abort command. </summary>
    ''' <value> The Abort command. </value>
    Protected Overrides ReadOnly Property AbortCommand As String = ":ABOR"

    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    Protected Overrides ReadOnly Property InitiateCommand As String = ":INIT"

#End Region

#End Region

#Region " SIMPLE LOOP "

    Private Const SimpleLoopModel As String = "SimpleLoop"

    ''' <summary> Loads simple loop. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <param name="count"> Number of. </param>
    ''' <param name="delay"> The delay. </param>
    Public Sub LoadSimpleLoop(ByVal count As Integer, ByVal delay As TimeSpan)
        Me.Write(":TRIG:LOAD '{0}',{1},{2}", SimpleLoopModel, count, delay.TotalSeconds)
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
        Me.Write(":TRIG:LOAD '{0}',{1},{2},{3},{4},{5},{6},{7},{8}",
                 GradeBinningModel, count, triggerOption, startDelay.TotalSeconds, endDelay.TotalSeconds, highLimit, lowLimit, failedBitPattern, passBitPattern)
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
    ''' <remarks> David, 12/30/2016. </remarks>
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
                cmd = $"DIG:LINE{i}:MODE DIG,OUT"
                Me.Write(cmd)
            End If
            bitPattern <<= 1
        Next

        ' clear the trigger model
        cmd = ":TRIG:LOAD 'EMPTY'" : Me.Write(cmd)

        ' clear any pending trigger
        cmd = ":TRIG:EXT:IN:CLE" : Me.Write(cmd)

        ' clear the default buffer
        cmd = ":TRAC:CLE" : Me.Write(cmd)

        Dim autonomous As Boolean = False

        ' Block 1: 
        ' -- if autonomous mode, clear the buffer
        ' -- if program control, clear the buffer
        block += 1 : cmd = If(autonomous, $":TRIG:BLOC:BUFF:CLE {block}", $":TRIG:BLOC:NOP {block}")
        Me.Write(cmd)

        ' Block 2: Wait for external trigger; this is the repeat block.
        Dim repeatBlock As Integer = block + 1
        block += 1 : cmd = $":TRIG:BLOC:WAIT {block}, EXT"
        Me.Write(cmd)

        ' Block 3: Pre-Measure Delay
        block += 1 : cmd = $":TRIG:BLOC:DEL:CONS {block}, {0.001 * startDelay.TotalMilliseconds}"
        Me.Write(cmd)

        ' Block 4: Measure
        block += 1 : cmd = $":TRIG:BLOC:MEAS {block}"
        Me.Write(cmd)

        ' Block 5: Limit test and branch; the pass block is 3 blocks ahead (8)
        Dim passBlock As Integer = block + 4
        block += 1 : cmd = $":TRIG:BLOC:BRAN:LIM:DYN {block},IN,1,{passBlock}"
        Me.Write(cmd)

        ' Block 6: Output failure bit pattern
        block += 1 : cmd = $":TRIG:BLOC:DIG:IO {block},{failedBitPattern},{mask}"
        Me.Write(cmd)

        ' Block 7: Skip the pass binning block
        block += 1 : cmd = $":TRIG:BLOCk:BRAN:ALW {block}, {block + 2}"
        Me.Write(cmd)

        ' Block 8: Output pass bit pattern
        block += 1 : cmd = $":TRIG:BLOC:DIG:IO {block},{passBitPattern},{mask}"
        Me.Write(cmd)

        Dim notificationId As Integer = 1

        ' Block 9: Notify measurement completed
        block += 1 : cmd = $":TRIG:BLOC:NOT {block},{notificationId}"
        Me.Write(cmd)

        ' set external output setting
        cmd = $":TRIG:EXT:OUT:STIM NOT{notificationId}"
        Me.Write(cmd)

        ' Block 10: Repeat Count times.
        block += 1 : cmd = If(count <= 0, $":TRIG:BLOCk:BRAN:ALW {block},{repeatBlock}", $":TRIG:BLOC:BRAN:COUN {block},{count},{repeatBlock}")
        Me.Write(cmd)

    End Sub

#End Region

End Class
