Imports System.Configuration

Partial Friend NotInheritable Class TestInfo

#Region " MEASURE SUBSYSTEM INFORMATION "

    ''' <summary> Gets the Initial auto Delay Enabled settings. </summary>
    ''' <value> The auto Delay settings. </value>
    Public Shared ReadOnly Property InitialAutoDelayEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the Initial auto Range enabled settings. </summary>
    ''' <value> The auto Range settings. </value>
    Public Shared ReadOnly Property InitialAutoRangeEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the Initial auto zero Enabled settings. </summary>
    ''' <value> The auto zero settings. </value>
    Public Shared ReadOnly Property InitialAutoZeroEnabled As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    Public Shared ReadOnly Property InitialFrontTerminalsSelected As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    Public Shared ReadOnly Property InitialRemoteSenseSelected As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    ''' <summary> Gets the Initial Sense Function settings. </summary>
    ''' <value> The Sense Function settings. </value>
    Public Shared ReadOnly Property InitialSenseFunction As MeasureFunctionMode
        Get
            Return CType(My.MyAppSettingsReader.AppSettingInt32(), MeasureFunctionMode)
        End Get
    End Property

    Public Shared ReadOnly Property InitialSourceFunction As SourceFunctionMode
        Get
            Return CType(My.MyAppSettingsReader.AppSettingInt32(), SourceFunctionMode)
        End Get
    End Property

    Public Shared ReadOnly Property InitialSourceLevel As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property
    Public Shared ReadOnly Property InitialSourceLimit As Double
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

    Public Shared ReadOnly Property FrontTerminalsSelected As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property


    ''' <summary> Gets the Sense Function settings. </summary>
    ''' <value> The Sense Function settings. </value>
    Public Shared ReadOnly Property SenseFunction As MeasureFunctionMode
        Get
            Return CType(My.MyAppSettingsReader.AppSettingInt32(), MeasureFunctionMode)
        End Get
    End Property

    Public Shared ReadOnly Property SourceFunction As SourceFunctionMode
        Get
            Return CType(My.MyAppSettingsReader.AppSettingInt32(), SourceFunctionMode)
        End Get
    End Property

    Public Shared ReadOnly Property SourceLevel As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    ''' <summary> Gets the power line cycles settings. </summary>
    ''' <value> The power line cycles settings. </value>
    Public Shared ReadOnly Property PowerLineCycles As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property RemoteSenseSelected As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

    Public Shared ReadOnly Property ExpectedResistance As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property ExpectedResistanceEpsilon As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

#End Region

End Class
