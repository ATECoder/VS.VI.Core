
Partial Public NotInheritable Class TestInfo

#Region " DEVICE INFORMATION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property ResourceModel As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    ''' <summary> Gets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Shared ReadOnly Property ResourceName As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    ''' <summary> Gets the Title of the resource. </summary>
    ''' <value> The Title of the resource. </value>
    Public Shared ReadOnly Property ResourceTitle As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    ''' <summary> Gets the language. </summary>
    ''' <value> The language. </value>
    Public Shared ReadOnly Property Language As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    ''' <summary> Gets the keep alive query command. </summary>
    ''' <value> The keep alive query command. </value>
    Public Shared ReadOnly Property KeepAliveQueryCommand As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    ''' <summary> Gets the keep alive command. </summary>
    ''' <value> The keep alive command. </value>
    Public Shared ReadOnly Property KeepAliveCommand As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    ''' <summary> Gets the read termination enabled. </summary>
    ''' <value> The read termination enabled. </value>
    Public Shared ReadOnly Property ReadTerminationEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the termination character. </summary>
    ''' <value> The termination character. </value>
    Public Shared ReadOnly Property TerminationCharacter As Integer
        Get
            Return My.MyAppSettingsReader.AppSettingInt32()
        End Get
    End Property

#End Region

#Region " LOCAL NODE "

    ''' <summary> Gets the line frequency. </summary>
    ''' <value> The line frequency. </value>
    Public Shared ReadOnly Property LineFrequency As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

#End Region

#Region " MULTIMETER SUBSYSTEM INFORMATION "

    ''' <summary> Gets the Initial power line cycles settings. </summary>
    ''' <value> The power line cycles settings. </value>
    Public Shared ReadOnly Property InitialPowerLineCycles As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    ''' <summary> Gets the auto zero Enabled settings. </summary>
    ''' <value> The auto zero settings. </value>
    Public Shared ReadOnly Property AutoZeroEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    Public Shared ReadOnly Property AutoRangeEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the Sense Function settings. </summary>
    ''' <value> The Sense Function settings. </value>
    Public Shared ReadOnly Property SenseFunction As Integer
        Get
            Return My.MyAppSettingsReader.AppSettingInt32()
        End Get
    End Property

    ''' <summary> Gets the power line cycles settings. </summary>
    ''' <value> The power line cycles settings. </value>
    Public Shared ReadOnly Property PowerLineCycles As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

#End Region

End Class

