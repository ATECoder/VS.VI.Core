Imports System.Configuration

Partial Public NotInheritable Class TestInfo

#Region " CONFIGURATION INFORMATION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property Exists As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

#End Region

End Class
