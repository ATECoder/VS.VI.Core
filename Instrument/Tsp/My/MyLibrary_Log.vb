Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
Namespace My

    Partial Public NotInheritable Class MyLibrary

        ''' <summary> Logs unpublished exception. </summary>
        ''' <param name="activity">  The activity. </param>
        ''' <param name="exception"> The exception. </param>
        Public Shared Sub LogUnpublishedException(ByVal activity As String, ByVal exception As Exception)
            MyLibrary.LogUnpublishedMessage(New TraceMessage(TraceEventType.Error, MyLibrary.TraceEventId, $"Exception {activity};. {exception.ToFullBlownString}"))
        End Sub

        ''' <summary> Applies the given value. </summary>
        ''' <param name="value"> The value. </param>
        Public Shared Sub Apply(ByVal value As isr.Core.Pith.MyLog)
            MyLibrary._MyLog = value
        End Sub

        ''' <summary> Applies the trace level described by value. </summary>
        ''' <param name="value"> The value. </param>
        Public Shared Sub ApplyTraceLogLevel(ByVal value As TraceEventType)
            MyLibrary.TraceLevel = value
            MyLibrary.MyLog.ApplyTraceLevel(value)
        End Sub

        ''' <summary> Applies the trace level described by value. </summary>
        Public Shared Sub ApplyTraceLogLevel()
            MyLibrary.ApplyTraceLogLevel(MyLibrary.TraceLevel)
        End Sub

    End Class

End Namespace
