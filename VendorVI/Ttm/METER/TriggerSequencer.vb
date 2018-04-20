Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
''' <summary> Triggered Measurement sequencer. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="3/14/2014" by="David" revision=""> Created. </history>
Public Class TriggerSequencer
    Inherits PropertyNotifyBase

#Region " CONSTRUCTION + CLEANUP "

    Public Sub New()
        MyBase.New()
        Me.signalQueueSyncLocker = New Object
        Me.LockedSignalQueue = New Queue(Of TriggerSequenceSignal)
    End Sub

    ''' <summary> Cleans up unmanaged or managed and unmanaged resources. </summary>
    ''' <param name="disposing"> <c>True</c> if this method releases both managed and unmanaged
    ''' resources;
    ''' False if this method releases only unmanaged resources. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        Try

            If Not Me.IsDisposed Then

                If disposing Then

                    ' Free managed resources when explicitly called
                    If Me._sequencerTimer IsNot Nothing Then
                        Me._sequencerTimer.Enabled = False
                        Me._sequencerTimer.Dispose()
                    End If

                End If

                ' Free shared unmanaged resources

            End If

        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception disposing device", "Exception {0}", ex.ToFullBlownString)
        Finally

            ' dispose the base class.
            MyBase.Dispose(disposing)

        End Try

    End Sub

#End Region

#Region " SEQUENCED MEASUREMENTS "

    ''' <summary> Gets or sets the assert requested. </summary>
    ''' <value> The assert requested. </value>
    Public Property AssertRequested As Boolean

    ''' <summary> Asserts a trigger to emulate triggering for timing measurements. </summary>
    Public Sub AssertTrigger()
        Me.AssertRequested = True
        Me.SafeSendPropertyChanged()
        Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary> Next status bar. </summary>
    ''' <param name="lastStatusBar"> The last status bar. </param>
    ''' <returns> The next status bar. </returns>
    Public Shared Function NextStatusBar(ByVal lastStatusBar As String) As String
        If String.IsNullOrEmpty(lastStatusBar) Then
            lastStatusBar = "|"
        ElseIf lastStatusBar = "|" Then
            lastStatusBar = "/"
        ElseIf lastStatusBar = "/" Then
            lastStatusBar = "-"
        ElseIf lastStatusBar = "-" Then
            lastStatusBar = "\"
        ElseIf lastStatusBar = "\" Then
            lastStatusBar = "|"
        Else
            lastStatusBar = "|"
        End If
        Return lastStatusBar
    End Function

    Dim lastTriggerTime As DateTime
    ''' <summary> Returns the percent progress. </summary>
    ''' <returns> The status message. </returns>
    Public Function ProgressMessage(ByVal lastStatusBar As String) As String
        Dim message As String = NextStatusBar(lastStatusBar)
        Select Case TriggerSequenceState
            Case TriggerSequenceState.Aborted
                message = "ABORTED"
            Case TriggerSequenceState.Failed
                message = "FAILED"
            Case TriggerSequenceState.Idle
                message = "Inactive"
            Case TriggerSequenceState.MeasurementCompleted
                message = "DATA AVAILABLE"
            Case TriggerSequenceState.None
            Case TriggerSequenceState.ReadingValues
                message = "READING..."
            Case TriggerSequenceState.Starting
                message = "PREPARING"
            Case TriggerSequenceState.Stopped
                message = "STOPPED"
            Case TriggerSequenceState.WaitingForTrigger
                message = String.Format("Waiting for trigger {0:0.0} s", DateTime.Now.Subtract(Me.lastTriggerTime).TotalSeconds)
        End Select
        Return message
    End Function

    Private _TriggerSequenceState As TriggerSequenceState
    ''' <summary> Gets or sets the state of the measurement. </summary>
    ''' <value> The measurement state. </value>
    Public Property TriggerSequenceState As TriggerSequenceState
        Get
            Return Me._TriggerSequenceState
        End Get
        Protected Set(value As TriggerSequenceState)
            If TriggerSequenceState.WaitingForTrigger = value OrElse Not value.Equals(Me.TriggerSequenceState) Then
                Me._TriggerSequenceState = value
                Me.SafeSendPropertyChanged()
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    Private signalQueueSyncLocker As Object

    ''' <summary> Gets or sets a queue of signals. </summary>
    ''' <value> A Queue of signals. </value>
    Private Property LockedSignalQueue As Queue(Of TriggerSequenceSignal)

    ''' <summary> Clears the signal queue. </summary>
    Public Sub ClearSignalQueue()
        SyncLock signalQueueSyncLocker
            Me.lockedSignalQueue.Clear()
        End SyncLock
    End Sub

    ''' <summary> Dequeues a  signal. </summary>
    ''' <param name="signal"> [in,out] The signal. </param>
    ''' <returns> <c>True</c> if a signal was dequeued. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function Dequeue(ByRef signal As TriggerSequenceSignal) As Boolean
        SyncLock signalQueueSyncLocker
            If Me.lockedSignalQueue.Any Then
                signal = Me.lockedSignalQueue.Dequeue()
                Return True
            Else
                Return False
            End If
        End SyncLock
    End Function

    ''' <summary> Enqueues a  signal. </summary>
    ''' <param name="signal"> The signal. </param>
    Public Sub Enqueue(ByVal signal As TriggerSequenceSignal)
        SyncLock signalQueueSyncLocker
            Me.lockedSignalQueue.Enqueue(signal)
        End SyncLock
    End Sub

    ''' <summary> Starts a measurement sequence. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges")>
    Public Sub StartMeasurementSequence()

        Me.RestartSignal = TriggerSequenceSignal.None
        If Me._sequencerTimer Is Nothing Then
            Me._sequencerTimer = New Timers.Timer()
        End If
        Me._sequencerTimer.Enabled = False
        Me._sequencerTimer.Interval = 100

        Me.ClearSignalQueue()
        Me.Enqueue(TriggerSequenceSignal.Step)
        Me._sequencerTimer.Enabled = True

    End Sub

    ''' <summary> Gets or sets the timer. </summary>
    Private WithEvents _SequencerTimer As Timers.Timer

    ''' <summary> Executes the state sequence. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SequencerTimer_Elapsed(ByVal sender As Object, ByVal e As System.EventArgs) Handles _sequencerTimer.Elapsed

        Try
            Me._sequencerTimer.Enabled = False
            Windows.Forms.Application.DoEvents()
            Me.TriggerSequenceState = Me.executeMeasurementSequence(Me.TriggerSequenceState)
        Catch ex As Exception
            Me.Enqueue(TriggerSequenceSignal.Failure)
        Finally
            Windows.Forms.Application.DoEvents()
            If Me.TriggerSequenceState = TriggerSequenceState.Idle OrElse
                Me.TriggerSequenceState = TriggerSequenceState.None Then
                Me._sequencerTimer.Enabled = False
            Else
                Me._sequencerTimer.Enabled = True
            End If
        End Try

    End Sub

    ''' <summary> Gets or sets the restart signal. </summary>
    ''' <value> The restart signal. </value>
    Public Property RestartSignal As TriggerSequenceSignal

    ''' <summary> Executes the measurement sequence returning the next state. </summary>
    ''' <param name="currentState"> The current measurement sequence state. </param>
    ''' <returns> The next state. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Function ExecuteMeasurementSequence(ByVal currentState As TriggerSequenceState) As TriggerSequenceState

        Dim signal As TriggerSequenceSignal
        Select Case currentState

            Case TriggerSequenceState.Idle

                ' Waiting for the step signal to start.
                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case TriggerSequenceSignal.Abort
                            currentState = TriggerSequenceState.Aborted
                        Case TriggerSequenceSignal.None
                        Case TriggerSequenceSignal.Stop
                        Case TriggerSequenceSignal.Step
                            currentState = TriggerSequenceState.Starting
                    End Select
                End If

            Case TriggerSequenceState.Aborted

                ' if failed, no action. The sequencer should stop here
                ' wait for the signal to move to the idle state
                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case TriggerSequenceSignal.Abort
                        Case TriggerSequenceSignal.Failure
                            currentState = TriggerSequenceState.Failed
                        Case TriggerSequenceSignal.None
                        Case TriggerSequenceSignal.Stop
                            ' clear the queue to start a fresh cycle.
                            Me.ClearSignalQueue()
                            currentState = TriggerSequenceState.Idle
                        Case TriggerSequenceSignal.Step
                            ' clear the queue to start a fresh cycle.
                            Me.ClearSignalQueue()
                            currentState = TriggerSequenceState.Idle
                    End Select
                End If

            Case TriggerSequenceState.Failed

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case TriggerSequenceSignal.Abort
                            currentState = TriggerSequenceState.Aborted
                        Case TriggerSequenceSignal.None
                        Case TriggerSequenceSignal.Stop
                            currentState = TriggerSequenceState.Aborted
                        Case TriggerSequenceSignal.Step
                            currentState = TriggerSequenceState.Aborted
                    End Select
                End If

            Case TriggerSequenceState.Stopped

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case TriggerSequenceSignal.Abort
                            currentState = TriggerSequenceState.Aborted
                        Case TriggerSequenceSignal.Failure
                            currentState = TriggerSequenceState.Failed
                        Case TriggerSequenceSignal.None
                        Case TriggerSequenceSignal.Stop
                            currentState = TriggerSequenceState.Idle
                        Case TriggerSequenceSignal.Step
                            currentState = TriggerSequenceState.Idle
                    End Select
                End If

            Case TriggerSequenceState.Starting

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case TriggerSequenceSignal.Abort
                            currentState = TriggerSequenceState.Aborted
                        Case TriggerSequenceSignal.Failure
                            currentState = TriggerSequenceState.Failed
                        Case TriggerSequenceSignal.None
                        Case TriggerSequenceSignal.Stop
                            currentState = TriggerSequenceState.Idle
                        Case TriggerSequenceSignal.Step
                            lastTriggerTime = DateTime.Now
                            currentState = TriggerSequenceState.WaitingForTrigger
                    End Select
                End If

            Case TriggerSequenceState.WaitingForTrigger

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case TriggerSequenceSignal.Abort, TriggerSequenceSignal.Failure, TriggerSequenceSignal.Stop
                            Me.RestartSignal = signal
                            ' request a trigger. 
                            Me.AssertRequested() = True
                        Case TriggerSequenceSignal.Step
                            currentState = TriggerSequenceState.MeasurementCompleted
                    End Select
                End If

            Case TriggerSequenceState.MeasurementCompleted

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case TriggerSequenceSignal.Abort
                            currentState = TriggerSequenceState.Aborted
                        Case TriggerSequenceSignal.Failure
                            currentState = TriggerSequenceState.Failed
                        Case TriggerSequenceSignal.None
                        Case TriggerSequenceSignal.Stop
                            currentState = TriggerSequenceState.Idle
                        Case TriggerSequenceSignal.Step
                            If Me.RestartSignal = TriggerSequenceSignal.None OrElse
                                Me.RestartSignal = TriggerSequenceSignal.Step Then
                                currentState = TriggerSequenceState.ReadingValues
                            Else
                                Select Case Me.RestartSignal
                                    Case TriggerSequenceSignal.Abort
                                        currentState = TriggerSequenceState.Aborted
                                    Case TriggerSequenceSignal.Failure
                                        currentState = TriggerSequenceState.Failed
                                    Case TriggerSequenceSignal.None
                                    Case TriggerSequenceSignal.Stop
                                        currentState = TriggerSequenceState.Idle
                                End Select
                                Me.RestartSignal = TriggerSequenceSignal.None
                            End If
                    End Select
                End If

            Case TriggerSequenceState.ReadingValues

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case TriggerSequenceSignal.Abort
                            currentState = TriggerSequenceState.Aborted
                        Case TriggerSequenceSignal.Failure
                            currentState = TriggerSequenceState.Failed
                        Case TriggerSequenceSignal.None
                        Case TriggerSequenceSignal.Stop
                            currentState = TriggerSequenceState.Idle
                        Case TriggerSequenceSignal.Step
                            If Me.RestartSignal = TriggerSequenceSignal.None OrElse
                                Me.RestartSignal = TriggerSequenceSignal.Step Then
                                currentState = TriggerSequenceState.Starting
                            Else
                                Select Case Me.RestartSignal
                                    Case TriggerSequenceSignal.Abort
                                        currentState = TriggerSequenceState.Aborted
                                    Case TriggerSequenceSignal.Failure
                                        currentState = TriggerSequenceState.Failed
                                    Case TriggerSequenceSignal.None
                                    Case TriggerSequenceSignal.Stop
                                        currentState = TriggerSequenceState.Idle
                                End Select
                                Me.RestartSignal = TriggerSequenceSignal.None
                            End If
                    End Select
                End If

            Case Else

                Debug.Assert(Not Debugger.IsAttached, "Unhandled state: " & currentState.ToString)
                Me.Enqueue(TriggerSequenceSignal.Abort)

        End Select
        Return currentState

    End Function

#End Region

End Class

''' <summary>Enumerates the measurement sequence.</summary>
Public Enum TriggerSequenceState
    <Description("Not Defined")> None = 0
    <Description("Idle")> Idle = 0
    <Description("Triggered Measurement Sequence Aborted")> Aborted
    <Description("Triggered Measurement Sequence Failed")> Failed
    <Description("Triggered Measurement Sequence Starting")> Starting
    <Description("Waiting for Trigger")> WaitingForTrigger
    <Description("Measurement Completed")> MeasurementCompleted
    <Description("Reading Values")> ReadingValues
    <Description("Triggered Measurement Sequence Stopped")> Stopped
End Enum

''' <summary>Enumerates the measurement signals.</summary>
Public Enum TriggerSequenceSignal
    <Description("Not Defined")> None = 0
    <Description("Step Measurement")> [Step]
    <Description("Abort Measurement")> [Abort]
    <Description("Stop Measurement")> [Stop]
    <Description("Report Failure")> [Failure]
End Enum


