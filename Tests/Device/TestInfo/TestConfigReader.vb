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

#End Region

#Region " VERSION INFO "

    Public Shared ReadOnly Property NationalInstrumentVisaVersion As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property FoundationVisaFileVersion32 As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property FoundationVisaFileVersion64 As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

#End Region

End Class
