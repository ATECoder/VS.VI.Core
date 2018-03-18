Imports System.Configuration

Partial Public NotInheritable Class TestInfo

#Region " CONFIGURATION INFORMTION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property Exists As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the verbose. </summary>
    ''' <value> The verbose. </value>
    Public Shared ReadOnly Property Verbose As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

#End Region

#Region " ASSEMBLY INFO "

    ''' <summary> Gets the executing assembly location. </summary>
    ''' <value> The executing assembly location. </value>
    Public Shared ReadOnly Property ExecutingAssemblyLocation As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue
        End Get
    End Property

    ''' <summary> Gets the filename of the trace event project identities file. </summary>
    ''' <value> The filename of the trace event project identities file. </value>
    Public Shared ReadOnly Property TraceEventProjectIdentitiesFileName As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue
        End Get
    End Property

#End Region

End Class
