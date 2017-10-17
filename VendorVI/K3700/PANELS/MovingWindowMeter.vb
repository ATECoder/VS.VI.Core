Imports System.ComponentModel
Imports System.Threading
Imports System.Windows.Forms
Imports isr.Core.Pith
Imports isr.Core.Controls.ProgressBarExtensions
''' <summary> A moving window averaging meter. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/30/2016" by="David" revision=""> Created. </history>
Public Class MovingWindowMeter
    Inherits TalkerControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    Private _InitializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()

        Me._initializingComponents = True
        ' This call is required by the designer.
        Me.InitializeComponent()
        Me._initializingComponents = False

        Me._MeasurementFormatString = "G8"
        Me._MovingWindow = New isr.Core.Engineering.MovingWindow

        Me._ExpectedStopTimeout = TimeSpan.FromMilliseconds(100)

        ' set very long in case operator starts browsing the application
        Me._TaskCompleteNotificationTimeout = TimeSpan.FromSeconds(10)

        ' set very long in case operator starts browsing the application
        Me._TaskStartNotificationTimeout = TimeSpan.FromSeconds(10)

        Me._TaskStopTimeout = TimeSpan.FromSeconds(1)
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed Then
                If disposing Then
                    If Me._MovingWindow IsNot Nothing Then
                        Me.MovingWindow.ClearKnownState()
                        Me._MovingWindow = Nothing
                    End If
                    If Me._task IsNot Nothing Then Me._task.Dispose() : Me._task = Nothing
                    ' release the device.
                    If Me._Device IsNot Nothing Then Me._Device = Nothing
                    If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " FORM EVENTS  "

    Private Sub MovingWindowMeter_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me._StartMovingAverageButton.Enabled = False
    End Sub

#End Region

#Region " MOVING AVERAGE "

    ''' <summary> Device open changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _Device_OpenChanged(sender As Object, e As EventArgs) Handles _Device.Opened, _Device.Closed
        Me._StartMovingAverageButton.Enabled = Me._Device.IsDeviceOpen
    End Sub

    Private WithEvents _Device As K3700.Device
    ''' <summary> Gets or sets the device. </summary>
    ''' <value> The device. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property Device As K3700.Device
        Get
            Return Me._Device
        End Get
        Set(value As K3700.Device)
            Me._Device = value
        End Set
    End Property

    ''' <summary> Gets or sets the moving average. </summary>
    ''' <value> The moving average. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property MovingWindow As isr.Core.Engineering.MovingWindow

    Public Property Length As Integer
        Get
            Return Me.MovingWindow.Length
        End Get
        Set(value As Integer)
            Me.MovingWindow.Length = value
            Me._LengthTextBox.Text = value.ToString
        End Set
    End Property

    ''' <summary> Gets the timeout interval. </summary>
    ''' <value> The timeout interval. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property TimeoutInterval As TimeSpan
        Get
            Return Me.MovingWindow.TimeoutInterval
        End Get
        Set(value As TimeSpan)
            Me.MovingWindow.TimeoutInterval = value
            Me._TimeoutTextBox.Text = value.TotalSeconds.ToString
        End Set
    End Property

    ''' <summary> Gets or sets the update rule. </summary>
    ''' <value> The update rule. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property UpdateRule As isr.Core.Engineering.MovingWindowUpdateRule
        Get
            Return Me.MovingWindow.UpdateRule
        End Get
        Set(value As isr.Core.Engineering.MovingWindowUpdateRule)
            Me.MovingWindow.UpdateRule = value
        End Set
    End Property

    ''' <summary> Gets or sets the window. </summary>
    ''' <value> The window. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property Window As Double
        Get
            Return Me.MovingWindow.Window
        End Get
        Set(value As Double)
            If value <> Me.Window Then
                Me.MovingWindow.Window = value
                Me._WindowTextBox.Text = $"{(100 * value):0.####}"
            End If
        End Set
    End Property

    Private _ReadingTimespan As TimeSpan
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ReadingTimespan As TimeSpan
        Get
            Return Me._ReadingTimespan
        End Get
        Set(value As TimeSpan)
            If value <> Me.ReadingTimespan Then
                Me._ReadingTimespan = value
                Me.UpdateReadingTimespanCaption(value)
            End If
        End Set
    End Property

    ''' <summary> Updates the reading timespan caption described by value. </summary>
    ''' <param name="value"> The value. </param>
    Private Sub UpdateReadingTimespanCaption(ByVal value As TimeSpan)
        Dim caption As String = $"{value.TotalMilliseconds:0}"
        If Not String.Equals(caption, Me._ReadingTimeSpanLabel.Text) Then Me._ReadingTimeSpanLabel.Text = caption
    End Sub

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property HasMovingAverageTaskResult As Boolean
        Get
            Return Me.MovingAverageTaskResult IsNot Nothing
        End Get
    End Property

    ''' <summary> Gets or sets the measurement Failed. </summary>
    ''' <value> The measurement Failed. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property MeasurementFailed As Boolean
        Get
            Return Me.HasMovingAverageTaskResult AndAlso Me.MovingAverageTaskResult.Failed
        End Get
    End Property

    ''' <summary> Gets the failure details. </summary>
    ''' <value> The failure details. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property FailureDetails As String
        Get
            If Me.MeasurementFailed Then
                If Me.MovingAverageTaskResult.Exception IsNot Nothing Then
                    Return Me.MovingAverageTaskResult.Exception.ToString
                Else
                    Return Me.MovingAverageTaskResult.Details
                End If
            Else
                Return ""
            End If
        End Get
    End Property

    ''' <summary> Gets the failure exception. </summary>
    ''' <value> The failure exception. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property FailureException As Exception
        Get
            Return Me.MovingAverageTaskResult.Exception
        End Get
    End Property

#Region " TASK COMPLETE "

    ''' <summary> Gets or sets the task complete notification timeout. </summary>
    ''' <value> The task complete notification timeout. </value>
    ''' <remarks> Defaults to 10 seconds to allow for user interface browsing while collecting data. </remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property TaskCompleteNotificationTimeout As TimeSpan


    ''' <summary> Clears the task complete semaphore. </summary>
    Private Sub ClearTaskComplete()
        Me._TaskComplete = NotificationSemaphores.None
        Me.SafePostPropertyChanged(NameOf(Me.TaskComplete))
    End Sub

    ''' <summary> Set the task complete semaphore. </summary>
    Private Sub NotifyTaskComplete()
        Me._TaskComplete = NotificationSemaphores.Sent
        Me.SafePostPropertyChanged(NameOf(Me.TaskComplete))
    End Sub

    Private Function NotifyTaskComplete(timeout As TimeSpan) As Boolean
        Dim endTime As DateTime = DateTime.UtcNow.Add(timeout)
        Me.NotifyTaskComplete()
        Do
            System.Windows.Forms.Application.DoEvents()
        Loop Until ((Me.TaskComplete And NotificationSemaphores.Acknowledged) <> 0) OrElse DateTime.UtcNow > endTime
        Return (Me.TaskComplete And NotificationSemaphores.Acknowledged) <> 0
    End Function

    ''' <summary> Acknowledge task complete semaphore. </summary>
    Public Sub AcknowledgeTaskComplete()
        Me._TaskComplete = Me.TaskComplete Or NotificationSemaphores.Acknowledged
    End Sub

    ''' <summary> Gets or sets the task complete semaphore. </summary>
    ''' <value> The measurement Completed. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property TaskComplete As NotificationSemaphores

#End Region

#Region " TASK Start "

    ''' <summary> Gets or sets the task start notification timeout. </summary>
    ''' <value> The task start notification timeout. </value>
    ''' <remarks> Defaults to 10 seconds to allow for user interface browsing while collecting data. </remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property TaskStartNotificationTimeout As TimeSpan

    ''' <summary> Clears the task Start semaphore. </summary>
    Private Sub ClearTaskStart()
        Me._TaskStart = NotificationSemaphores.None
        Me.SafePostPropertyChanged(NameOf(Me.TaskStart))
    End Sub

    ''' <summary> Set the task Start semaphore. </summary>
    Private Sub NotifyTaskStart()
        Me._TaskStart = NotificationSemaphores.Sent
        Me.SafePostPropertyChanged(NameOf(Me.TaskStart))
    End Sub

    Private Function NotifyTaskStart(timeout As TimeSpan) As Boolean
        Dim endTime As DateTime = DateTime.UtcNow.Add(timeout)
        Me.NotifyTaskStart()
        Do
            System.Windows.Forms.Application.DoEvents()
        Loop Until ((Me.TaskStart And NotificationSemaphores.Acknowledged) <> 0) OrElse DateTime.UtcNow > endTime
        Return (Me.TaskStart And NotificationSemaphores.Acknowledged) <> 0
    End Function

    ''' <summary> Acknowledge task Start semaphore. </summary>
    Public Sub AcknowledgeTaskStart()
        Me._TaskStart = Me.TaskStart Or NotificationSemaphores.Acknowledged
    End Sub

    ''' <summary> Gets or sets the task Start semaphore. </summary>
    ''' <value> The measurement Started. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public ReadOnly Property TaskStart As NotificationSemaphores

#End Region

    Private _MeasurementFormatString As String

    ''' <summary> Gets or sets the measurement Format String. </summary>
    ''' <value> The measurement FormatString. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property MeasurementFormatString As String
        Get
            Return Me._MeasurementFormatString
        End Get
        Protected Set(value As String)
            Me._MeasurementFormatString = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

#End Region

#Region " PROGRESS REPORTING "

    Private _Count As Integer

    ''' <summary> Gets or sets the number of.  </summary>
    ''' <value> The count. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property Count As Integer
        Get
            Return Me._Count
        End Get
        Protected Set(value As Integer)
            If Me.Count <> value Then
                Me._Count = value
                Me._CountLabel.Text = value.ToString
            End If
        End Set
    End Property

    Private _ReadingsCount As Integer

    ''' <summary> Gets or sets the number of readings. </summary>
    ''' <value> The number of readings. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ReadingsCount As Integer
        Get
            Return Me._ReadingsCount
        End Get
        Protected Set(value As Integer)
            If Me.ReadingsCount <> value Then
                Me._ReadingsCount = value
                Me._ReadingsCountLabel.Text = value.ToString
            End If
        End Set
    End Property

    Private _PercentProgress As Integer

    ''' <summary> Gets or sets the percent progress. </summary>
    ''' <value> The percent progress. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property PercentProgress As Integer
        Get
            Return Me._PercentProgress
        End Get
        Protected Set(value As Integer)
            If Me.PercentProgress <> value Then
                Me._PercentProgress = value
                Me._AverageProgressBar.ValueSetter(value)
            End If
        End Set
    End Property

    Private _ElapsedTime As TimeSpan

    ''' <summary> Gets or sets the elapsed time. </summary>
    ''' <value> The elapsed time. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ElapsedTime As TimeSpan
        Get
            Return Me._ElapsedTime
        End Get
        Protected Set(value As TimeSpan)
            If Me.ElapsedTime <> value Then
                Me._ElapsedTime = value
                Me._ElapsedTimeLabel.Text = value.ToString("mm\:ss\.ff", Globalization.CultureInfo.CurrentCulture)
            End If
        End Set
    End Property

    Private _MaximumReading As Double

    ''' <summary> Gets or sets the maximum reading. </summary>
    ''' <value> The maximum reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property MaximumReading As Double
        Get
            Return Me._MaximumReading
        End Get
        Protected Set(value As Double)
            If Me.MaximumReading <> value Then
                Me._MaximumReading = value
                Me._MaximumLabel.Text = value.ToString(Me.MeasurementFormatString)
            End If
        End Set
    End Property

    Private _MinimumReading As Double

    ''' <summary> Gets or sets the minimum reading. </summary>
    ''' <value> The minimum reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property MinimumReading As Double
        Get
            Return Me._MinimumReading
        End Get
        Protected Set(value As Double)
            If Me.MinimumReading <> value Then
                Me._MinimumReading = value
                Me._MinimumLabel.Text = value.ToString(Me.MeasurementFormatString)
            End If
        End Set
    End Property

    Private _Reading As Double

    ''' <summary> Gets or sets the reading. </summary>
    ''' <value> The reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property Reading As Double
        Get
            Return Me._Reading
        End Get
        Protected Set(value As Double)
            If value <> Me.Reading Then
                Me._Reading = value
                Me._AverageLabel.Text = value.ToString(Me.MeasurementFormatString)
            End If
        End Set
    End Property

    Private _LastReading As Double?

    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property LastReading As Double?
        Get
            Return Me._LastReading
        End Get
        Protected Set(value As Double?)
            If Not Nullable.Equals(value, Me.LastReading) Then
                Me._LastReading = value
                If value.HasValue Then Me._ReadingLabel.Text = value.Value.ToString(Me.MeasurementFormatString)
            End If
        End Set
    End Property

    Private _ReadingStatus As Core.Engineering.MovingWindowStatus

    ''' <summary> Gets or sets the reading status. </summary>
    ''' <value> The reading status. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property ReadingStatus As Core.Engineering.MovingWindowStatus
        Get
            Return Me._ReadingStatus
        End Get
        Protected Set(value As Core.Engineering.MovingWindowStatus)
            If Me.ReadingStatus <> value Then
                Me._ReadingStatus = value
                Me._StatusLabel.Text = Core.Engineering.MovingWindow.StatusAnnunciationCaption(value)
            End If
        End Set
    End Property

    ''' <summary> Reports progress changed. </summary>
    ''' <param name="movingWindow">    The moving window. </param>
    ''' <param name="percentProgress"> The percent progress. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ReportProgressChanged(ByVal movingWindow As isr.Core.Engineering.MovingWindow, ByVal percentProgress As Integer, ByVal result As TaskResult)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of isr.Core.Engineering.MovingWindow, Integer, TaskResult)(AddressOf ReportProgressChanged), New Object() {movingWindow, percentProgress, result})
        Else
            Dim ma As isr.Core.Engineering.MovingWindow = movingWindow
            If ma IsNot Nothing AndAlso ma.ReadingsCount > 0 Then
                Try
                    Dim value As Double = 0
                    Dim hasReading As Boolean = Core.Engineering.MovingWindow.HasReading(ma.Status)
                    If hasReading Then value = ma.Mean
                    Me.Window = ma.Window
                    Me.ReadingTimespan = ma.ReadingTimespan
                    Me.PercentProgress = percentProgress
                    Me.ElapsedTime = ma.ElapsedTime
                    Me.Count = ma.Count
                    Me.ReadingsCount = ma.ReadingsCount
                    Me.MaximumReading = ma.Maximum
                    Me.MinimumReading = ma.Minimum
                    Me.LastReading = ma.LastReading
                    Me.ReadingStatus = ma.Status
                    If hasReading Then Me.Reading = value
                    ' this helps flash out exceptions:
                    Application.DoEvents()
                Catch ex As Exception
                    result.RegisterFailure(ex, "exception reporting progress")
                End Try
            End If
        End If
    End Sub

    ''' <summary> Reports progress changed. </summary>
    ''' <param name="movingWindow"> The moving window. </param>
    Private Sub ReportProgressChanged(ByVal movingWindow As isr.Core.Engineering.MovingWindow)
        Me.ReportProgressChanged(movingWindow, movingWindow.PercentProgress, Me.MovingAverageTaskResult)
    End Sub

    ''' <summary> Process the completion described by result. </summary>
    ''' <param name="result"> The result. </param>
    Private Sub ProcessCompletion(ByVal result As TaskResult)
        If result Is Nothing Then
            Me._MovingAverageTaskResult = New TaskResult
            Me.MovingAverageTaskResult.RegisterFailure("Unexpected null task result when completing the task;. Contact the developer")
        Else
            Me._MovingAverageTaskResult = result
        End If
        If Not Me.NotifyTaskComplete(Me.TaskCompleteNotificationTimeout) Then
            If Me.MovingAverageTaskResult.Failed Then
                Me.MovingAverageTaskResult.RegisterFailure($"{Me.MovingAverageTaskResult.Details}; also, timeout receiving completion acknowledgment")
            Else
                Me.MovingAverageTaskResult.RegisterFailure("Timeout receiving completion acknowledgment")
            End If
        End If
    End Sub

#End Region

#Region " MEASURE "

    Private ReadOnly Property MovingAverageTaskResult As New TaskResult

    ''' <summary> Measure moving average. </summary>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <param name="progress"> The progress reporter. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureMovingAverage(progress As IProgress(Of isr.Core.Engineering.MovingWindow))
        Me._MovingAverageTaskResult = New TaskResult
        Try
		Me.ApplyCapturedSyncContext()
            Me.MovingWindow.ClearKnownState()
            If Me.NotifyTaskStart(Me.TaskStartNotificationTimeout) Then
                Do
                    ' measure and time
                    If Me.MovingWindow.ReadValue(Function() Me.Device.MultimeterSubsystem.Measure()) Then

                        progress.Report(New isr.Core.Engineering.MovingWindow(Me.MovingWindow))
                    Else
                        Me.MovingAverageTaskResult.RegisterFailure("device returned a null value")
                    End If
                Loop Until Me.IsCancellationRequested OrElse Me.MovingAverageTaskResult.Failed OrElse Me.MovingWindow.IsCompleted OrElse Me.MovingWindow.IsTimeout
            Else
                Me.MovingAverageTaskResult.RegisterFailure("Timeout receiving start acknowledgment")
            End If
        Catch ex As Exception
            Me.MovingAverageTaskResult.RegisterFailure(ex)
        Finally
            Me.ProcessCompletion(Me.MovingAverageTaskResult)
        End Try
    End Sub

#End Region

#Region " ASYNC TASK "

    ''' <summary> The cancellation token source. </summary>
    Private Property CancellationTokenSource As CancellationTokenSource

    ''' <summary> The cancellation token. </summary>
    Private Property CancellationToken As CancellationToken

    ''' <summary>
    ''' Requests the state machine to immediately stop operation.
    ''' </summary>
    Public Overridable Sub RequestCancellation()
        If Not Me.IsCancellationRequested Then
            ' stops the wait.
            Me.CancellationTokenSource.Cancel()
            Me.SafePostPropertyChanged(NameOf(Me.IsCancellationRequested))
        End If
    End Sub

    ''' <summary> Query if cancellation requested. </summary>
    ''' <value> <c>true</c> if cancellation requested; otherwise <c>false</c> </value>
    Public ReadOnly Property IsCancellationRequested() As Boolean
        Get
            Return Me.CancellationToken.IsCancellationRequested
        End Get
    End Property

    ''' <summary> Gets the sentinel indicating if the meter is running. </summary>
    ''' <value> The sentinel indicating if the meter is running. </value>
    Public ReadOnly Property IsRunning As Boolean
        Get
            Return Me.Task IsNot Nothing AndAlso Me.Task.Status = TaskStatus.Running
        End Get
    End Property

    ''' <summary> Gets the is stopped. </summary>
    ''' <value> The is stopped. </value>
    Public ReadOnly Property IsStopped As Boolean
        Get
            Return Me.Task Is Nothing OrElse Me.Task.Status <> TaskStatus.Running
        End Get
    End Property

    ''' <summary> Gets the expected stop timeout. </summary>
    ''' <value> The expected stop timeout. </value>
    Public Property ExpectedStopTimeout As TimeSpan

    ''' <summary> Stops measure asynchronous if. </summary>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c>
    ''' </returns>
    Public Function StopAsyncTask() As Boolean
        Return Me.StopAsyncTask(Me.ExpectedStopTimeout)
    End Function

    ''' <summary> Stops measure asynchronous if. </summary>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Private Function StopAsyncTask(ByVal timeout As TimeSpan) As Boolean
        If Not Me.IsStopped AndAlso Not Me.IsCancellationRequested Then
            ' wait for previous operation to complete.
            Dim endTime As DateTime = DateTime.UtcNow.Add(timeout)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Waiting for previous task to complete")
            Do Until Me.IsStopped OrElse DateTime.UtcNow > endTime
                Windows.Forms.Application.DoEvents()
            Loop
            If Not Me.IsStopped AndAlso Not Me.IsCancellationRequested Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Requesting cancellation of previous tasks")
                Me.RequestCancellation()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Waiting for previous task to stop")
                endTime = DateTime.UtcNow.Add(timeout)
                Do Until Me.IsStopped OrElse DateTime.UtcNow > endTime
                    Windows.Forms.Application.DoEvents()
                Loop
            End If
        End If
        If Not Me.IsStopped Then Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Attempt to stop previous task failed")
        Return Me.IsStopped
    End Function

    ''' <summary> The task. </summary>
    Private _Task As Task

    ''' <summary> Gets the task. </summary>
    ''' <value> The task. </value>
    Protected ReadOnly Property Task As Task
        Get
            Return Me._task
        End Get
    End Property

    ''' <summary> Gets or sets the task stopping timeout. </summary>
    ''' <value> The task stopping timeout. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property TaskStopTimeout As TimeSpan

    ''' <summary> Activates the machine asynchronous task. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="runSynchronously"> True to run synchronously. </param>
    ''' <param name="syncContext">      Context for the synchronization. </param>
    Public Sub StartMeasureTask(ByVal runSynchronously As Boolean, ByVal syncContext As SynchronizationContext)
        Me.CaptureSyncContext(syncContext)
        Me.StopAsyncTask(Me.TaskStopTimeout)
        ' proceed even if not stopped
        Me._TaskStart = NotificationSemaphores.None
        Me.CancellationTokenSource = New CancellationTokenSource
        Me.CancellationToken = CancellationTokenSource.Token

        Dim progress As New Progress(Of Core.Engineering.MovingWindow)(AddressOf ReportProgressChanged)
        Me._task = New Task(Sub() Me.MeasureMovingAverage(progress))
        If runSynchronously Then
            Me.Task.RunSynchronously(TaskScheduler.FromCurrentSynchronizationContext)
        Else
            Me.Task.Start()
        End If
    End Sub

    ''' <summary> Starts measure asynchronous. </summary>
    ''' <param name="syncContext"> Context for the synchronization. </param>
    Public Sub StartMeasureAsync(ByVal syncContext As SynchronizationContext)
        Me.StartMeasureTask(False, syncContext)
    End Sub

    ''' <summary> Activates the machine asynchronous task. </summary>
    ''' <remarks> This was suspected to cause issues. </remarks>
    Public Sub StartMeasureTaskStopRequired(ByVal runSynchronously As Boolean, ByVal syncContext As SynchronizationContext)
        Me.CaptureSyncContext(syncContext)
        If Me.StopAsyncTask(Me.TaskStopTimeout) Then
            Me._TaskStart = NotificationSemaphores.None
            Me.CancellationTokenSource = New CancellationTokenSource
            Me.CancellationToken = CancellationTokenSource.Token
            Dim progress As New Progress(Of Core.Engineering.MovingWindow)(AddressOf ReportProgressChanged)
            Me._task = New Task(Sub() Me.MeasureMovingAverage(progress))
            If runSynchronously Then

                Me.Task.RunSynchronously(TaskScheduler.FromCurrentSynchronizationContext)
            Else
                Me.Task.Start()
            End If
        Else
            Me._TaskStart = NotificationSemaphores.None
            'Me.MeasurementStarted = False
        End If
    End Sub

    Public Sub StartMeasureTaskStopRequiredAsync(ByVal syncContext As SynchronizationContext)
        Me.StartMeasureTaskStopRequired(False, syncContext)
    End Sub

    Public Async Function AsyncTask() As Task
        Dim progress As New Progress(Of Core.Engineering.MovingWindow)(AddressOf ReportProgressChanged)
        Await Task.Run(Sub() Me.MeasureMovingAverage(progress))
    End Function

#End Region

#Region " START STOP "

    ''' <summary> Starts moving average button check state changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _StartMovingAverageButton_CheckStateChanged(sender As Object, e As EventArgs) Handles _StartMovingAverageButton.CheckStateChanged
        If Me._initializingComponents Then Return
        Dim button As ToolStripButton = TryCast(sender, ToolStripButton)
        If button Is Nothing Then Return
        Try
            If button.Checked Then
                Me.MovingWindow.Length = CInt(Me._LengthTextBox.Text)
                Me.MovingWindow.Window = 0.01 * CDbl(Me._WindowTextBox.Text)
                Me.MovingWindow.UpdateRule = Core.Engineering.MovingWindowUpdateRule.StopOnWithinWindow
                Me.MovingWindow.TimeoutInterval = TimeSpan.FromSeconds(CDbl(Me._TimeoutTextBox.Text))
                Me.StartMeasureAsync(SynchronizationContext.Current)
                If Not Me.IsRunning Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed starting the moving window task")
                End If
            Else
                Me.StopAsyncTask(Me.TaskStopTimeout)
                If Not Me.IsStopped Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed stopping the moving window task")
                End If
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception starting or stopping;. {0}", ex.ToFullBlownString)
        Finally
            button.Text = $"{button.Checked.GetHashCode:'Stop';'Stop';'Start'}"
        End Try
    End Sub

#End Region

#Region " TASK RESULT "

    ''' <summary> Encapsulates the result of a task. </summary>
    ''' <license>
    ''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
    ''' Licensed under The MIT License.</para><para>
    ''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
    ''' </license>
    ''' <history date="9/24/2016" by="David" revision=""> Created. </history>
    Private Class TaskResult

        ''' <summary> Registers the failure described by exception. </summary>
        ''' <param name="details"> The details. </param>
        Public Sub RegisterFailure(ByVal details As String)
            Me._Failed = True
            Me._Details = details
        End Sub

        ''' <summary> Registers the failure described by exception. </summary>
        ''' <param name="exception"> The exception. </param>
        Public Sub RegisterFailure(ByVal exception As Exception)
            Me.RegisterFailure(exception, "Exception occurred")
        End Sub

        Public Sub RegisterFailure(ByVal exception As Exception, ByVal details As String)
            Me._Failed = True
            Me._Details = details
            Me._Exception = exception
        End Sub

        ''' <summary> Gets the failed sentinel. </summary>
        ''' <value> The failed sentinel. </value>
        Public ReadOnly Property Failed As Boolean

        ''' <summary> Gets the failure details. </summary>
        ''' <value> The details. </value>
        Public ReadOnly Property Details As String

        ''' <summary> Gets the exception. </summary>
        ''' <value> The exception. </value>
        Public ReadOnly Property Exception As Exception

    End Class

#End Region

#Region " TALKER "

    ''' <summary> Adds a listener. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overrides Sub AddListener(ByVal listener As isr.Core.Pith.IMessageListener)
        MyBase.AddListener(listener)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of isr.Core.Pith.IMessageListener))
        MyBase.AddListeners(listeners)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AddListeners(ByVal talker As ITraceMessageTalker)
        MyBase.AddListeners(talker)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region

	
End Class

''' <summary> Values that represent notification semaphores. </summary>
<Flags> Public Enum NotificationSemaphores
    <Description("Not set")> None = 0
    <Description("Notification Sent")> Sent = 1
    <Description("Notification Acknowledged")> Acknowledged = 2
End Enum

