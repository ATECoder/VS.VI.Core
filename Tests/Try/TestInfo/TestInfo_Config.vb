Imports System.Configuration

Partial Public NotInheritable Class TestInfo

#Region " TRACE EVENT IDENTITY FILE "

    ''' <summary> Gets the filename of the trace event project identities file. </summary>
    ''' <value> The filename of the trace event project identities file. </value>
    Public Shared ReadOnly Property TraceEventProjectIdentitiesFileName As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue
        End Get
    End Property

#End Region

End Class
