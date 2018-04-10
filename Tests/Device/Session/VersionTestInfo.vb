Imports System.Configuration

Partial Public NotInheritable Class TestInfo

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
