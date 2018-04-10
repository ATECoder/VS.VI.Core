Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Logs unpublished exception. </summary>
        ''' <param name="activity">  The activity. </param>
        ''' <param name="exception"> The exception. </param>
        Public Sub LogUnpublishedException(ByVal activity As String, ByVal exception As Exception)
            Me.LogUnpublishedMessage(New TraceMessage(TraceEventType.Error, MyApplication.TraceEventId, $"Exception {activity};. {exception.ToFullBlownString}"))
        End Sub

        ''' <summary> Applies the given value. </summary>
        ''' <param name="value"> The value. </param>
        Public Sub Apply(ByVal value As MyLog)
            Me._MyLog = value
        End Sub

        ''' <summary> Applies the trace level described by value. </summary>
        ''' <param name="value"> The value. </param>
        Public Sub ApplyTraceLogLevel(ByVal value As TraceEventType)
            Me.TraceLevel = value
            Me.MyLog.ApplyTraceLevel(value)
        End Sub

        ''' <summary> Applies the trace level described by value. </summary>
        Public Sub ApplyTraceLogLevel()
            Me.ApplyTraceLogLevel(My.MySettings.Default.TraceLevel)
        End Sub

    End Class

End Namespace
