Imports System.ComponentModel
Imports System.Threading

<CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
Partial Public Class MovingWindowMeter

    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Private Sub InitializeWorker()
        Me.Worker = New System.ComponentModel.BackgroundWorker() With {
            .WorkerSupportsCancellation = True,
            .WorkerReportsProgress = True
        }
    End Sub

    Private Sub DisposeWorker()
        If Me._worker IsNot Nothing Then Me._worker.Dispose() : Me._worker = Nothing
    End Sub

#Region " BACKGROUND WORKER "

    ''' <summary> Gets the measurement rate. </summary>
    ''' <value> The measurement rate. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property MeasurementRate As Double = 25

    ''' <summary> Worker payload. </summary>
    Private Class WorkerPayLoad
        Public Property Device As K3700.Device
        Public Property MovingWindow As isr.Core.Engineering.MovingWindow
        Public Property EstimatedCountout As Integer
        Public Property DoEventCount As Integer = 10
        Public Sub ClearKnownState()
            Me.MovingWindow.ClearKnownState()
        End Sub
        Public Sub InitializeKnownState(ByVal measurementRate As Double)
            Me.EstimatedCountout = CInt(measurementRate * Me.MovingWindow.TimeoutInterval.TotalSeconds)
        End Sub
    End Class

    ''' <summary> A user state. </summary>
    Private Class UserState

        Public Sub New()
            MyBase.New
            Me.Result = New TaskResult
        End Sub
        Public Property Result As TaskResult

        Public Property MovingAverage As isr.Core.Engineering.MovingWindow
        Public Property EstimatedCountout As Integer
        Public ReadOnly Property PercentProgress As Integer
            Get
                Dim baseCount As Integer = 0
                Dim count As Integer = CType(Me.MovingAverage?.TotalReadingsCount, Integer?).GetValueOrDefault(0)
                If count < 2 * Me.MovingAverage.Length Then
                    baseCount = 2 * Me.MovingAverage.Length
                ElseIf Me.EstimatedCountout > 0 Then
                    baseCount = Me.EstimatedCountout
                End If
                If baseCount > 0 Then
                    Return CInt(100 * count / baseCount)
                ElseIf Me.MovingAverage.TimeoutInterval > TimeSpan.Zero Then
                    Return CInt(100 * MovingAverage.ElapsedMilliseconds / Me.MovingAverage.TimeoutInterval.TotalMilliseconds)
                Else
                    Return Me.LogPercentProgress
                End If
            End Get
        End Property
        Public ReadOnly Property LogPercentProgress As Integer
            Get
                Dim count As Integer = CType(Me.MovingAverage?.TotalReadingsCount, Integer?).GetValueOrDefault(0)
                If Me.EstimatedCountout > 0 Then
                    Return CInt(100 * Math.Log(count) / Math.Log(Me.EstimatedCountout))
                ElseIf Me.MovingAverage.TimeoutInterval > TimeSpan.Zero Then
                    Return CInt(100 * Math.Log(MovingAverage.ElapsedMilliseconds) / Math.Log(Me.MovingAverage.TimeoutInterval.TotalMilliseconds))
                Else
                    Return CInt(100 * count / Me.MovingAverage.Length)
                End If
            End Get
        End Property
    End Class

    Private WithEvents Worker As System.ComponentModel.BackgroundWorker

    ''' <summary> Worker do work. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Do work event information. </param>
    Private Sub Worker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles worker.DoWork

	  Me.ApplyCapturedSyncContext()
        Dim w As BackgroundWorker = TryCast(sender, BackgroundWorker)
        If w Is Nothing OrElse Me.IsDisposed OrElse e Is Nothing OrElse e.Cancel Then Return

        Dim userState As New UserState

        Dim payload As WorkerPayLoad = TryCast(e.Argument, WorkerPayLoad)
        If payload Is Nothing Then

            userState.Result.RegisterFailure("Payload Not assigned to worker")
            e.Result = userState.Result
            e.Cancel = True
            Return
        End If

        payload.MovingWindow.ClearKnownState()
        userState.MovingAverage = Me.MovingWindow
        userState.EstimatedCountout = payload.EstimatedCountout
        Do
            If payload.MovingWindow.ReadValue(Function() payload.Device.MultimeterSubsystem.Measure()) Then
                w.ReportProgress(userState.PercentProgress, userState)
            Else
                userState.Result.RegisterFailure("device returned a null value")
            End If
            e.Result = userState.Result
            e.Cancel = userState.Result.Failed
            If Not e.Cancel Then
                Dim eventCount As Integer = payload.DoEventCount
                Do While eventCount > 0
                    Windows.Forms.Application.DoEvents()
                    eventCount -= 1
                Loop
            End If
        Loop Until w.CancellationPending OrElse e.Cancel OrElse payload.MovingWindow.IsStopStatus

        Do Until e.Cancel OrElse payload.MovingWindow.IsStopStatus
            Dim eventCount As Integer = payload.DoEventCount
            Do While eventCount > 0
                Windows.Forms.Application.DoEvents()
                eventCount -= 1
            Loop
        Loop

    End Sub

    ''' <summary> Handles run worker completed event. </summary>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    Private Sub OnWorkerRunWorkerCompleted(ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
	  Me.ApplyCapturedSyncContext()
        If e Is Nothing Then Return
        Dim result As TaskResult = TryCast(e.Result, TaskResult)
        If result Is Nothing Then Return
        If e.Cancelled AndAlso Not result.Failed Then
            result.RegisterFailure("Worker canceled")
        End If

        If e.Error IsNot Nothing AndAlso result.Exception Is Nothing Then
            result.RegisterFailure(e.Error, "Worker exception")
        End If
        Me.ProcessCompletion(result)
    End Sub

    ''' <summary> Worker run worker completed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Run worker completed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031: DoNotCatchGeneralExceptionTypes")>
    Private Sub Worker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles worker.RunWorkerCompleted
        Me.OnWorkerRunWorkerCompleted(e)
    End Sub

    ''' <summary> Worker progress changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Progress changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub Worker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles worker.ProgressChanged
        Me.ApplyCapturedSyncContext()
        Dim us As UserState = TryCast(e.UserState, UserState)
        Dim ma As New isr.Core.Engineering.MovingWindow(TryCast(us.MovingAverage, isr.Core.Engineering.MovingWindow))
        Me.ReportProgressChanged(ma, us.PercentProgress, us.Result)
    End Sub

    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function StopMeasureAsyncIf(ByVal timeout As TimeSpan) As Boolean
        Dim stopped As Boolean = Me.worker Is Nothing OrElse Not Me.worker.IsBusy
        If Not stopped Then
            ' wait for previous operation to complete.
            Dim endTime As DateTime = DateTime.UtcNow.Add(timeout)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Waiting for worker to complete previous task")
            Do Until Me.IsDisposed OrElse Not Worker.IsBusy OrElse DateTime.UtcNow > endTime
                Windows.Forms.Application.DoEvents()
            Loop
            If worker.IsBusy Then
                worker.CancelAsync()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Waiting for worker to cancel previous task")
                endTime = DateTime.UtcNow.Add(timeout)
                Do Until Me.IsDisposed OrElse Not Worker.IsBusy OrElse DateTime.UtcNow > endTime
                    Windows.Forms.Application.DoEvents()
                Loop
            End If
            stopped = Not worker.IsBusy
            If Not stopped Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed when waiting for worker to complete previous task")
                Return False
            End If
        End If
        Return stopped
    End Function

    ''' <summary> Starts measure asynchronous. </summary>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function StartMeasureWork(ByVal syncContext As SynchronizationContext) As Boolean
        Me._MovingWindowTaskResult = New TaskResult
	  Me.CaptureSyncContext(syncContext)
        Dim stopped As Boolean = StopMeasureAsyncIf(TimeSpan.FromSeconds(1))
        If Not stopped Then Return False
        Me.InitializeWorker()
        Dim payload As New WorkerPayLoad With {
            .Device = Me.Device,
            .MovingWindow = Me.MovingWindow
        }

        payload.ClearKnownState()
        payload.InitializeKnownState(Me.MeasurementRate)

        If Not (Me.IsDisposed OrElse Me.worker.IsBusy) Then
            Me.worker.RunWorkerAsync(payload)
            Me.NotifyTaskStart()
            ' Me.MeasurementStarted = True
        Else
            Me.ClearTaskStart()
            ' Me.MeasurementStarted = False
        End If
        Return Me.TaskStart <> NotificationSemaphores.None '  Me.MeasurementStarted
    End Function

    ''' <summary> Starts a measure. </summary>
    ''' <param name="syncContext"> Context for the synchronization. </param>
    Public Sub StartMeasureWorkSync(ByVal syncContext As SynchronizationContext)
        If Me.StartMeasureWork(syncContext) Then
            ' wait for worker to get busy.
            Do While Not (Me.IsDisposed OrElse worker.IsBusy)
                Windows.Forms.Application.DoEvents()
            Loop
            ' wait till worker is done
            Do Until Me.IsDisposed OrElse Not worker.IsBusy
                Windows.Forms.Application.DoEvents()
            Loop
        End If
    End Sub

#End Region

End Class

