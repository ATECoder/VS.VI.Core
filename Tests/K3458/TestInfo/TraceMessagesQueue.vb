
Partial Public NotInheritable Class TestInfo

#Region " TRACE MESSAGE QUEUE "

    Private Shared _TraceMessagesQueueListener As isr.Core.Pith.TraceMessagesQueueListener

    ''' <summary> Gets the trace message queue listener. </summary>
    ''' <value> The trace message queue listener. </value>
    Public Shared ReadOnly Property TraceMessagesQueueListener As isr.Core.Pith.TraceMessagesQueueListener
        Get
            If TestInfo._TraceMessagesQueueListener Is Nothing Then
                TestInfo._TraceMessagesQueueListener = New isr.Core.Pith.TraceMessagesQueueListener
                TestInfo._TraceMessagesQueueListener.ApplyTraceLevel(TraceEventType.Warning)
            End If
            Return TestInfo._TraceMessagesQueueListener
        End Get
    End Property

    ''' <summary> Assert message. </summary>
    ''' <param name="traceMessage"> Message describing the trace. </param>
    Private Shared Sub AssertMessage(ByVal traceMessage As isr.Core.Pith.TraceMessage)
        If traceMessage Is Nothing Then
        ElseIf traceMessage.EventType = TraceEventType.Warning Then
            TestInfo.TraceMessage($"Warning published: {traceMessage.ToString}")
        ElseIf traceMessage.EventType = TraceEventType.Error Then
            Assert.Fail($"Error published: {traceMessage.ToString}")
        End If
    End Sub

    ''' <summary> Assert message queue. </summary>
    ''' <param name="queue"> The queue listener. </param>
    Private Shared Sub AssertMessageQueue(ByVal queue As isr.Core.Pith.TraceMessagesQueue)
        Do While Not queue.IsEmpty
            TestInfo.AssertMessage(queue.TryDequeue)
        Loop
    End Sub

#End Region

#Region " TRACE MESSAGE QUEUE COLLECTION "

    Private Shared _TraceMessagesQueues As TraceMessageQueueCollection
    Private Shared ReadOnly Property TraceMessagesQueues As TraceMessageQueueCollection
        Get
            If TestInfo._TraceMessagesQueues Is Nothing Then
                TestInfo._TraceMessagesQueues = New TraceMessageQueueCollection
            End If
            Return TestInfo._TraceMessagesQueues
        End Get
    End Property

    ''' <summary> Assert message queue. </summary>
    Public Shared Sub AssertMessageQueue()
        TestInfo.TraceMessagesQueues.AssertMessageQueue()
    End Sub

    ''' <summary> Clears the message queue. </summary>
    ''' <returns> A String. </returns>
    Public Shared Function ClearMessageQueue() As String
        Return TestInfo.TraceMessagesQueues.ClearMessageQueue()
    End Function

    Private Class TraceMessageQueueCollection
        Inherits ObjectModel.Collection(Of isr.Core.Pith.TraceMessagesQueue)
        Public Sub New()
            MyBase.New
            Me.Add(TestInfo.TraceMessagesQueueListener)
            Me.Add(isr.Core.Pith.My.MyLibrary.UnpublishedTraceMessages)
            Me.Add(isr.VI.My.MyLibrary.UnpublishedTraceMessages)
            Me.Add(isr.VI.R2D2.My.MyLibrary.UnpublishedTraceMessages)
            Me.Add(isr.VI.K3458.My.MyLibrary.UnpublishedTraceMessages)
        End Sub
        Public Sub AssertMessageQueue()
            For Each traceMessageQueue As isr.Core.Pith.TraceMessagesQueue In Me
                TestInfo.AssertMessageQueue(traceMessageQueue)
            Next
        End Sub
        ''' <summary> Appends a line. </summary>
        ''' <param name="builder"> The builder. </param>
        ''' <param name="value">   The value. </param>
        Private Shared Sub AppendLine(ByVal builder As System.Text.StringBuilder, ByVal value As String)
            If Not String.IsNullOrWhiteSpace(value) Then builder.AppendLine(value)
        End Sub

        Public Function ClearMessageQueue() As String
            Dim builder As New System.Text.StringBuilder
            For Each traceMessageQueue As isr.Core.Pith.TraceMessagesQueue In Me
                TraceMessageQueueCollection.AppendLine(builder, traceMessageQueue.FetchContent())
            Next
            Return builder.ToString
        End Function
    End Class

#End Region

End Class
