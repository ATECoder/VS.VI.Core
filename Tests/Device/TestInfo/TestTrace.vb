Partial Public NotInheritable Class TestInfo

#Region " TRACE "

    ''' <summary> Initializes the trace listener. </summary>
    Public Shared Sub InitializeTraceListener()
        TestInfo.ReplaceTraceListener()
        Console.Out.WriteLine(My.Application.Log.DefaultFileLogWriter.FullLogFileName)
    End Sub

    ''' <summary> Replace trace listener. </summary>
    Public Shared Sub ReplaceTraceListener()
        With My.Application.Log
            .TraceSource.Listeners.Remove(isr.Core.Pith.DefaultFileLogTraceListener.DefaultFileLogWriterName)
            .TraceSource.Listeners.Add(isr.Core.Pith.DefaultFileLogTraceListener.CreateListener(Core.Pith.UserLevel.CurrentUser))
            .TraceSource.Switch.Level = SourceLevels.Verbose
        End With
    End Sub

    ''' <summary> Trace message. </summary>
    ''' <param name="format"> Describes the format to use. </param>
    ''' <param name="args">   A variable-length parameters list containing arguments. </param>
    Public Shared Sub TraceMessage(ByVal format As String, ByVal ParamArray args() As Object)
        TestInfo.TraceMessage(String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
    End Sub

    ''' <summary> Trace message. </summary>
    ''' <param name="message"> The message. </param>
    Private Shared Sub TraceMessage(ByVal message As String)
        My.Application.Log.WriteEntry(message)
        'System.Diagnostics.Debug.WriteLine(message)
        Console.Out.WriteLine(message)
    End Sub

#End Region

End Class
