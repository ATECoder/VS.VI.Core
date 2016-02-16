Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Pith

''' <summary> A moving average meter. </summary>
''' <remarks> David, 1/30/2016. </remarks>
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

    Private _initializingComponents As Boolean
    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()

        Me._initializingComponents = True
        ' This call is required by the designer.
        Me.InitializeComponent()
        Me._initializingComponents = False

        Me._MovingAverage = New isr.Core.Engineering.MovingWindow
        Me.worker = New System.ComponentModel.BackgroundWorker()
        Me.worker.WorkerSupportsCancellation = True
        Me.worker.WorkerReportsProgress = True

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
            If Not Me.IsDisposed AndAlso disposing Then
                If Me._MovingAverage IsNot Nothing Then
                    Me.MovingAverage.ClearKnownState()
                    Me._MovingAverage = Nothing
                End If
                If Me._worker IsNot Nothing Then
                    Me._worker.Dispose()
                    Me._worker = Nothing
                End If
                ' release the device.
                If Me._Device IsNot Nothing Then Me._Device = Nothing
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
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
    Public ReadOnly Property MovingAverage As isr.Core.Engineering.MovingWindow

    Public Property Length As Integer
        Get
            Return Me.MovingAverage.Length
        End Get
        Set(value As Integer)
            Me.MovingAverage.Length = value
            Me._LengthTextBox.Text = value.ToString
        End Set
    End Property

    ''' <summary> Gets the timeout interval. </summary>
    ''' <value> The timeout interval. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property TimeoutInterval As TimeSpan
        Get
            Return Me.MovingAverage.TimeoutInterval
        End Get
        Set(value As TimeSpan)
            Me.MovingAverage.TimeoutInterval = value
            Me._TimeoutTextBox.Text = value.TotalSeconds.ToString
        End Set
    End Property

    ''' <summary> Gets or sets the update rule. </summary>
    ''' <value> The update rule. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property UpdateRule As isr.Core.Engineering.MovingWindowUpdateRule
        Get
            Return Me.MovingAverage.UpdateRule
        End Get
        Set(value As isr.Core.Engineering.MovingWindowUpdateRule)
            Me.MovingAverage.UpdateRule = value
        End Set
    End Property

    ''' <summary> Gets or sets the window. </summary>
    ''' <value> The window. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property Window As Double
        Get
            Return Me.MovingAverage.Window
        End Get
        Set(value As Double)
            Me.MovingAverage.Window = value
            Me._WindowLabel.Text = CStr(100 * value)
        End Set
    End Property

    Private _MeasurementAvailable As Boolean

    ''' <summary> Gets or sets the measurement available. </summary>
    ''' <value> The measurement available. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Property MeasurementAvailable As Boolean
        Get
            Return Me._MeasurementAvailable
        End Get
        Protected Set(value As Boolean)
            Me._MeasurementAvailable = value
            Me.AsyncNotifyPropertyChanged()
        End Set
    End Property

#End Region

#Region " BACKGROUND WORKER "

    ''' <summary> Gets the measurement rate. </summary>
    ''' <value> The measurement rate. </value>
    Public Property MeasurementRate As Double = 25

    ''' <summary> Worker payload. </summary>
    Private Class WorkerPayLoad
        Public Property Device As K3700.Device
        Public Property MovingAverage As isr.Core.Engineering.MovingWindow
        Public Property EstimatedCountout As Integer
        Public Property DoEventCount As Integer = 10
        Public Sub ClearKnownState()
            Me.MovingAverage.ClearKnownState()
        End Sub
        Public Sub InitializeKnownState(ByVal measurementRate As Double)
            Me.EstimatedCountout = CInt(measurementRate * Me.MovingAverage.TimeoutInterval.TotalSeconds)
        End Sub
    End Class

    ''' <summary> A user state. </summary>
    Private Class UserState
        Public Property MovingAverage As isr.Core.Engineering.MovingWindow
        Public Property EstimatedCountout As Integer
        Public ReadOnly Property PercentProgress As Integer
            Get
                Dim baseCount As Integer = 0
                If Me.MovingAverage.ReadingsQueue.Count < 2 * Me.MovingAverage.Length Then
                    baseCount = 2 * Me.MovingAverage.Length
                ElseIf Me.EstimatedCountout > 0 Then
                    baseCount = Me.EstimatedCountout
                End If
                If baseCount > 0 Then
                    Return CInt(100 * MovingAverage.ReadingsQueue.Count / baseCount)
                ElseIf Me.MovingAverage.TimeoutInterval > TimeSpan.Zero Then
                    Return CInt(100 * MovingAverage.ElapsedMilliseconds / Me.MovingAverage.TimeoutInterval.TotalMilliseconds)
                Else
                    Return Me.LogPercentProgress
                End If
            End Get
        End Property
        Public ReadOnly Property LogPercentProgress As Integer
            Get
                If Me.EstimatedCountout > 0 Then
                    Return CInt(100 * Math.Log(MovingAverage.ReadingsQueue.Count) / Math.Log(Me.EstimatedCountout))
                ElseIf Me.MovingAverage.TimeoutInterval > TimeSpan.Zero Then
                    Return CInt(100 * Math.Log(MovingAverage.ElapsedMilliseconds) / Math.Log(Me.MovingAverage.TimeoutInterval.TotalMilliseconds))
                Else
                    Return CInt(100 * MovingAverage.ReadingsQueue.Count / Me.MovingAverage.Length)
                End If
            End Get
        End Property
    End Class

    ''' <summary> Encapsulates the result of a work. </summary>
    Private Class WorkResult
        Public Property Cancelled As Boolean
        Public Property Details As String
    End Class

    Private WithEvents worker As System.ComponentModel.BackgroundWorker

    ''' <summary> Worker do work. </summary>
    ''' <remarks> David, 1/30/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Do work event information. </param>
    Private Sub worker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles worker.DoWork

        Dim w As BackgroundWorker = TryCast(sender, BackgroundWorker)
        If w Is Nothing OrElse Me.IsDisposed OrElse e Is Nothing OrElse e.Cancel Then Return

        Dim result As New WorkResult

        Dim payload As WorkerPayLoad = TryCast(e.Argument, WorkerPayLoad)
        If payload Is Nothing Then
            result.Cancelled = True
            result.Details = "Payload not assigned to worker"
            e.Result = result
            e.Cancel = True
            Return
        End If

        payload.MovingAverage.ClearKnownState()
        Dim userState As New UserState
        userState.MovingAverage = Me.MovingAverage
        userState.EstimatedCountout = payload.EstimatedCountout
        Do
            Dim value As Double? = payload.Device.MultimeterSubsystem.Measure()
            If value.HasValue Then
                payload.MovingAverage.AddValue(value.Value)
                w.ReportProgress(userState.PercentProgress, userState)
            Else
                result.Cancelled = True
                result.Details = "device returned a null value"
                e.Result = result
                e.Cancel = True
            End If
            Dim eventCount As Integer = payload.DoEventCount
            Do While eventCount > 0
                Windows.Forms.Application.DoEvents()
                eventCount -= 1
            Loop
        Loop Until w.CancellationPending OrElse e.Cancel OrElse payload.MovingAverage.IsCompleted OrElse payload.MovingAverage.IsTimeout

        Do Until e.Cancel OrElse payload.MovingAverage.IsCompleted OrElse payload.MovingAverage.IsTimeout
            Dim eventCount As Integer = payload.DoEventCount
            Do While eventCount > 0
                Windows.Forms.Application.DoEvents()
                eventCount -= 1
            Loop
        Loop

    End Sub

    ''' <summary> Handles run worker completed event. </summary>
    ''' <remarks> David, 1/30/2016. </remarks>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    Private Sub OnWorkerRunWorkerCompleted(ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        Dim result As WorkResult = TryCast(e.Result, WorkResult)
        If result?.Cancelled Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Work canceled;. Details: {0}", result.Details)
        ElseIf e?.Cancelled Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Work canceled;. ")
        ElseIf e?.Error IsNot Nothing Then
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred doing work;. Details: {0}", e?.Error)
        Else
            Me.MeasurementAvailable = True
        End If
    End Sub

    ''' <summary> Worker run worker completed. </summary>
    ''' <remarks> David, 1/30/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Run worker completed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub worker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles worker.RunWorkerCompleted
        Me.OnWorkerRunWorkerCompleted(e)
    End Sub

    ''' <summary> Executes the worker progress changed action. </summary>
    ''' <remarks> David, 1/30/2016. </remarks>
    ''' <param name="userState"> State of the user. </param>
    Private Sub OnWorkerProgressChanged(ByVal userState As UserState)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of UserState)(AddressOf OnWorkerProgressChanged), New Object() {userState})
        Else
            If userState IsNot Nothing Then
                Me._AverageProgressBar.Value = Math.Min(Me._AverageProgressBar.Maximum, userState.PercentProgress)
                Me._ElapsedTimeLabel.Text = userState.MovingAverage.ElapsedTime.ToString("mm\:ss\.ff", Globalization.CultureInfo.CurrentCulture)
                Me._CountLabel.Text = userState.MovingAverage.Count.ToString
                Me._ReadingsCountLabel.Text = userState.MovingAverage.ReadingsQueue.Count.ToString
                Me._MaximumLabel.Text = userState.MovingAverage.Maximum.ToString
                Me._MinimumLabel.Text = userState.MovingAverage.Minimum.ToString
                With userState.MovingAverage
                    If .Status = Core.Engineering.MovingWindowStatus.AboveWindow Then
                        Me._StatusLabel.Text = "high"
                        Me._AverageLabel.Text = .Mean.ToString
                    ElseIf .Status = Core.Engineering.MovingWindowStatus.BelowWindow Then
                        Me._StatusLabel.Text = "low"
                        Me._AverageLabel.Text = .Mean.ToString
                    ElseIf .Status = Core.Engineering.MovingWindowStatus.Filling Then
                        Me._StatusLabel.Text = "filling"
                    ElseIf .Status = Core.Engineering.MovingWindowStatus.None Then
                        Me._StatusLabel.Text = "n/a"
                    Else
                        Me._StatusLabel.Text = "within"
                        Me._AverageLabel.Text = .Mean.ToString
                    End If
                    If userState.MovingAverage.LastReading.HasValue Then
                        Me._ReadingLabel.Text = userState.MovingAverage.LastReading.Value.ToString
                    End If
                End With
                System.Windows.Forms.Application.DoEvents()
            End If
        End If
    End Sub

    ''' <summary> Worker progress changed. </summary>
    ''' <remarks> David, 1/30/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Progress changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub worker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles worker.ProgressChanged
        Me.OnWorkerProgressChanged(TryCast(e.UserState, UserState))
    End Sub

    ''' <summary> Stops measure asynchronous if. </summary>
    ''' <remarks> David, 1/30/2016. </remarks>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function StopMeasureAsyncIf(ByVal timeout As TimeSpan) As Boolean
        Dim stopped As Boolean = worker Is Nothing OrElse Not worker.IsBusy
        If Not stopped Then
            ' wait for previous operation to complete.
            Dim endTime As DateTime = DateTime.Now.Add(timeout)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Waiting for worker to complete previous task")
            Do Until Me.IsDisposed OrElse Not worker.IsBusy OrElse DateTime.Now > endTime
                Windows.Forms.Application.DoEvents()
            Loop
            If worker.IsBusy Then
                worker.CancelAsync()
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Waiting for worker to cancel previous task")
                endTime = DateTime.Now.Add(timeout)
                Do Until Me.IsDisposed OrElse Not worker.IsBusy OrElse DateTime.Now > endTime
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
    ''' <remarks> David, 1/30/2016. </remarks>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function StartMeasureAsync() As Boolean

        Dim stopped As Boolean = StopMeasureAsyncIf(TimeSpan.FromSeconds(1))
        If Not stopped Then Return False

        Dim payload As New WorkerPayLoad
        Me.MovingAverage.Length = CInt(Me._LengthTextBox.Text)
        Me.MovingAverage.Window = 0.01 * CDbl(Me._WindowTextBox.Text)
        Me.MovingAverage.UpdateRule = Core.Engineering.MovingWindowUpdateRule.StopOnWithinWindow
        Me.MovingAverage.TimeoutInterval = TimeSpan.FromSeconds(CDbl(Me._TimeoutTextBox.Text))

        Me._WindowLabel.Text = Me.MovingAverage.Window.ToString

        payload.Device = Me.Device
        payload.MovingAverage = Me.MovingAverage
        payload.ClearKnownState()
        payload.InitializeKnownState(Me.MeasurementRate)

        If Not (Me.IsDisposed OrElse Me.worker.IsBusy) Then
            Me.worker.RunWorkerAsync(payload)
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary> Clears the send sentinels. </summary>
    Public Sub StartMeasure()
        If Me.StartMeasureAsync() Then
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

#Region " START STOP "

    ''' <summary> Starts moving average button check state changed. </summary>
    ''' <remarks> David, 1/30/2016. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _StartMovingAverageButton_CheckStateChanged(sender As Object, e As EventArgs) Handles _StartMovingAverageButton.CheckStateChanged
        If Me._initializingComponents Then Return
        Dim button As ToolStripButton = TryCast(sender, ToolStripButton)
        If button Is Nothing Then Return
        Try
            If button.Checked Then
                Dim started As Boolean = Me.StartMeasureAsync()
                If Not started Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed starting the moving average worker")
                End If
            Else
                Dim stopped As Boolean = StopMeasureAsyncIf(TimeSpan.FromSeconds(1))
                If Not stopped Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Failed stopping the moving average worker")
                End If
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception starting or stopping;. Details: {0}", ex)
        Finally
            button.Text = $"{button.Checked.GetHashCode:'Stop';'Stop';'Start'}"
        End Try
    End Sub

#End Region

End Class
