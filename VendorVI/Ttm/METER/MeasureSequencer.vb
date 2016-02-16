Imports System.ComponentModel
Imports isr.Core.Pith
''' <summary> Measure sequencer. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="3/14/2014" by="David" revision=""> Created. </history>
Public Class MeasureSequencer
    Inherits PropertyNotifyBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Public Sub New()
        MyBase.New()
        Me.signalQueueSyncLocker = New Object
        Me.LockedSignalQueue = New Queue(Of MeasurementSequenceSignal)
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
            Debug.Assert(Not Debugger.IsAttached, "Exception disposing device", "Exception details: {0}", ex)
        Finally

            ' dispose the base class.
            MyBase.Dispose(disposing)

        End Try

    End Sub

#End Region

#Region " SEQUENCED MEASUREMENTS "

    ''' <summary> Returns the percent progress. </summary>
    ''' <param name="state"> The state. </param>
    ''' <returns> A value between 0 and 100 representing the progress. </returns>
    Private Shared Function PercentProgress(ByVal state As MeasurementSequenceState) As Integer
        Return CInt(100 * Math.Min(1, Math.Max(0, (state - MeasurementSequenceState.Starting) /
                                               (MeasurementSequenceState.Completed - MeasurementSequenceState.Starting))))
    End Function

    ''' <summary> Returns the percent progress. </summary>
    ''' <returns> A value between 0 and 100 representing the progress. </returns>
    Public Function PercentProgress() As Integer
        Return MeasureSequencer.PercentProgress(Me.MeasurementSequenceState)
    End Function

    Private _MeasurementSequenceState As MeasurementSequenceState
    ''' <summary> Gets or sets the state of the measurement. </summary>
    ''' <value> The measurement state. </value>
    Public Property MeasurementSequenceState As MeasurementSequenceState
        Get
            Return Me._MeasurementSequenceState
        End Get
        Protected Set(value As MeasurementSequenceState)
            If Not value.Equals(Me.MeasurementSequenceState) Then
                Me._MeasurementSequenceState = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    Private signalQueueSyncLocker As Object

    ''' <summary> Gets or sets a queue of signals. </summary>
    ''' <value> A Queue of signals. </value>
    Private Property lockedSignalQueue As Queue(Of MeasurementSequenceSignal)

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
    Public Function Dequeue(ByRef signal As MeasurementSequenceSignal) As Boolean
        SyncLock signalQueueSyncLocker
            If Me.lockedSignalQueue.Count > 0 Then
                signal = Me.lockedSignalQueue.Dequeue()
                Return True
            Else
                Return False
            End If
        End SyncLock
    End Function

    ''' <summary> Enqueues a  signal. </summary>
    ''' <param name="signal"> The signal. </param>
    Public Sub Enqueue(ByVal signal As MeasurementSequenceSignal)
        SyncLock signalQueueSyncLocker
            Me.lockedSignalQueue.Enqueue(signal)
        End SyncLock
    End Sub

    ''' <summary> Starts a measurement sequence. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges")>
    Public Sub StartMeasurementSequence()

        If Me._sequencerTimer Is Nothing Then
            Me._sequencerTimer = New Timers.Timer()
        End If
        Me._sequencerTimer.Enabled = False
        Me._sequencerTimer.Interval = 300

        Me.ClearSignalQueue()
        Me.Enqueue(MeasurementSequenceSignal.Step)
        Me._sequencerTimer.Enabled = True

    End Sub

    ''' <summary> Gets or sets the timer. </summary>
    Private WithEvents _sequencerTimer As Timers.Timer

    ''' <summary> Executes the state sequence. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _sequencerTimer_Elapsed(ByVal sender As Object, ByVal e As System.EventArgs) Handles _sequencerTimer.Elapsed

        Try
            Me._sequencerTimer.Enabled = False
            Me.MeasurementSequenceState = Me.executeMeasurementSequence(Me.MeasurementSequenceState)
        Catch ex As Exception
            Me.Enqueue(MeasurementSequenceSignal.Failure)
        Finally
            If Me.MeasurementSequenceState = MeasurementSequenceState.Idle OrElse
                Me.MeasurementSequenceState = MeasurementSequenceState.None Then
                Me._sequencerTimer.Enabled = False
            Else
                Me._sequencerTimer.Enabled = True
            End If
        End Try

    End Sub

    ''' <summary> Gets or sets the start final resistance time. </summary>
    ''' <value> The start final resistance time. </value>
    Public Property StartFinalResistanceTime As DateTimeOffset

    ''' <summary> Waits for end of post transient delay time. </summary>
    ''' <returns> true when delay is done. </returns>
    Private Function donePostTransientPause() As Boolean
        Return DateTimeOffset.Now > Me.StartFinalResistanceTime
    End Function

    ''' <summary> Executes the measurement sequence returning the next state. </summary>
    ''' <param name="currentState"> The current measurement sequence state. </param>
    ''' <returns> The next state. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Private Function executeMeasurementSequence(ByVal currentState As MeasurementSequenceState) As MeasurementSequenceState

        Dim signal As MeasurementSequenceSignal
        Select Case currentState

            Case MeasurementSequenceState.Idle

                ' Waiting for the step signal to start.
                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case MeasurementSequenceSignal.Abort
                            currentState = MeasurementSequenceState.Aborted
                        Case MeasurementSequenceSignal.None
                        Case MeasurementSequenceSignal.Step
                            currentState = MeasurementSequenceState.Starting
                    End Select
                End If

            Case MeasurementSequenceState.Aborted

                ' if failed, no action. The sequencer should stop here
                ' wait for the signal to move to the idle state
                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case MeasurementSequenceSignal.Abort
                        Case MeasurementSequenceSignal.Failure
                            currentState = MeasurementSequenceState.Failed
                        Case MeasurementSequenceSignal.None
                        Case MeasurementSequenceSignal.Step
                            ' clear the queue to start a fresh cycle.
                            Me.ClearSignalQueue()
                            currentState = MeasurementSequenceState.Idle
                    End Select
                End If

            Case MeasurementSequenceState.Failed

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case MeasurementSequenceSignal.Abort
                            currentState = MeasurementSequenceState.Aborted
                        Case MeasurementSequenceSignal.None
                        Case MeasurementSequenceSignal.Step
                            currentState = MeasurementSequenceState.Aborted
                    End Select
                End If

            Case MeasurementSequenceState.Completed

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case MeasurementSequenceSignal.Abort
                            currentState = MeasurementSequenceState.Aborted
                        Case MeasurementSequenceSignal.Failure
                            currentState = MeasurementSequenceState.Failed
                        Case MeasurementSequenceSignal.None
                        Case MeasurementSequenceSignal.Step
                            currentState = MeasurementSequenceState.Idle
                    End Select
                End If

            Case MeasurementSequenceState.Starting

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case MeasurementSequenceSignal.Abort
                            currentState = MeasurementSequenceState.Aborted
                        Case MeasurementSequenceSignal.Failure
                            currentState = MeasurementSequenceState.Failed
                        Case MeasurementSequenceSignal.None
                        Case MeasurementSequenceSignal.Step
                            currentState = MeasurementSequenceState.MeasureInitialResistance
                    End Select
                End If

            Case MeasurementSequenceState.MeasureInitialResistance

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case MeasurementSequenceSignal.Abort
                            currentState = MeasurementSequenceState.Aborted
                        Case MeasurementSequenceSignal.Failure
                            currentState = MeasurementSequenceState.Failed
                        Case MeasurementSequenceSignal.None
                        Case MeasurementSequenceSignal.Step
                            currentState = MeasurementSequenceState.MeasureThermalTransient
                    End Select
                End If

            Case MeasurementSequenceState.MeasureThermalTransient

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case MeasurementSequenceSignal.Abort
                            currentState = MeasurementSequenceState.Aborted
                        Case MeasurementSequenceSignal.Failure
                            currentState = MeasurementSequenceState.Failed
                        Case MeasurementSequenceSignal.None
                        Case MeasurementSequenceSignal.Step
                            currentState = MeasurementSequenceState.PostTransientPause
                    End Select
                End If

            Case MeasurementSequenceState.PostTransientPause

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case MeasurementSequenceSignal.Abort
                            currentState = MeasurementSequenceState.Aborted
                        Case MeasurementSequenceSignal.Failure
                            currentState = MeasurementSequenceState.Failed
                        Case MeasurementSequenceSignal.None
                        Case MeasurementSequenceSignal.Step
                            currentState = MeasurementSequenceState.MeasureFinalResistance
                    End Select
                ElseIf Me.donePostTransientPause Then
                    currentState = MeasurementSequenceState.MeasureFinalResistance
                End If

            Case MeasurementSequenceState.MeasureFinalResistance

                If Me.Dequeue(signal) Then
                    Select Case signal
                        Case MeasurementSequenceSignal.Abort
                            currentState = MeasurementSequenceState.Aborted
                        Case MeasurementSequenceSignal.Failure
                            currentState = MeasurementSequenceState.Failed
                        Case MeasurementSequenceSignal.None
                        Case MeasurementSequenceSignal.Step
                            currentState = MeasurementSequenceState.Completed
                    End Select
                End If

            Case Else

                Debug.Assert(Not Debugger.IsAttached, "Unhandled state: " & currentState.ToString)
                Me.Enqueue(MeasurementSequenceSignal.Abort)

        End Select
        Return currentState

    End Function

#End Region

End Class

''' <summary>Enumerates the measurement sequence.</summary>
Public Enum MeasurementSequenceState
    <Description("Not Defined")> None = 0
    <Description("Idle")> Idle = 0
    <Description("Measurement Sequence Aborted")> Aborted
    <Description("Measurement Sequence Failed")> Failed
    <Description("Measurement Sequence Starting")> Starting
    <Description("Measure Initial Resistance")> MeasureInitialResistance
    <Description("Fetched Initial Resistance")> MeasureThermalTransient
    <Description("Post Transient Pause")> PostTransientPause
    <Description("Measure Final Resistance")> MeasureFinalResistance
    <Description("Measurement Sequence Completed")> Completed
End Enum

''' <summary>Enumerates the measurement signals.</summary>
Public Enum MeasurementSequenceSignal
    <Description("Not Defined")> None = 0
    <Description("Step Measurement")> [Step]
    <Description("Abort Measurement")> [Abort]
    <Description("Report Failure")> [Failure]
End Enum


