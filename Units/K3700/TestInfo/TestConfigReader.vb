Imports System.Configuration
Imports isr.VI.tsp.K3700

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

    ''' <summary> Gets all. </summary>
    ''' <value> all. </value>
    Public Shared ReadOnly Property All As Boolean
        Get
            Return My.MyAppSettingsReader.AppSettingBoolean()
        End Get
    End Property

#End Region

#Region " DEVICE INFORMATION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property ResourceModel As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared Property TestLocation As TestLocation

    Private Shared _resourceName As String
    Public Shared ReadOnly Property ResourceName As String
        Get
            If String.IsNullOrWhiteSpace(_resourceName) Then
                If isr.VI.ResourceNamesManager.Ping(IsrResourceName) Then
                    _resourceName = IsrResourceName
                    TestInfo.TestLocation = TestLocation.Isr
                Else
                    _resourceName = MicronResourceName
                    TestInfo.TestLocation = TestLocation.Micron
                End If
            End If
            Return _resourceName
        End Get
    End Property

    ''' <summary> Gets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Shared ReadOnly Property IsrResourceName As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property MicronResourceName As String
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

#Region " SOURCE MEASURE UNIT INFORMATION "

    ''' <summary> Gets the maximum output power of the instrument. </summary>
    ''' <value> The maximum output power . </value>
    Public Shared ReadOnly Property MaximumOutputPower As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    ''' <summary> Gets the line frequency. </summary>
    ''' <value> The line frequency. </value>
    Public Shared ReadOnly Property LineFrequency As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

#End Region

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

    ''' <summary> Gets the Initial Sense Function settings. </summary>
    ''' <value> The Sense Function settings. </value>
    Public Shared ReadOnly Property InitialSenseFunction As MultimeterFunctionMode
        Get
            Return CType(My.MyAppSettingsReader.AppSettingInt32(), MultimeterFunctionMode)
        End Get
    End Property

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
    Public Shared ReadOnly Property SenseFunction As MultimeterFunctionMode
        Get
            Return CType(My.MyAppSettingsReader.AppSettingInt32(), MultimeterFunctionMode)
        End Get
    End Property

    ''' <summary> Gets the power line cycles settings. </summary>
    ''' <value> The power line cycles settings. </value>
    Public Shared ReadOnly Property PowerLineCycles As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property ShortChannelList As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property ResistorChannelList As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property OpenChannelList As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property R1ChannelList As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property R2ChannelList As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property R3ChannelList As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property R4ChannelList As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property BridgeNumber As String
        Get
            Return If(TestInfo.TestLocation = TestLocation.Micron, TestInfo.MicronBridgeNumber, TestInfo.IsrBridgeNumber)
        End Get
    End Property

    Public Shared ReadOnly Property BridgeR1 As Double
        Get
            Return If(TestInfo.TestLocation = TestLocation.Micron, TestInfo.MicronBridgeR1, TestInfo.IsrBridgeR1)
        End Get
    End Property

    Public Shared ReadOnly Property BridgeR2 As Double
        Get
            Return If(TestInfo.TestLocation = TestLocation.Micron, TestInfo.MicronBridgeR2, TestInfo.IsrBridgeR2)
        End Get
    End Property

    Public Shared ReadOnly Property BridgeR3 As Double
        Get
            Return If(TestInfo.TestLocation = TestLocation.Micron, TestInfo.MicronBridgeR3, TestInfo.IsrBridgeR3)
        End Get
    End Property

    Public Shared ReadOnly Property BridgeR4 As Double
        Get
            Return If(TestInfo.TestLocation = TestLocation.Micron, TestInfo.MicronBridgeR4, TestInfo.IsrBridgeR4)
        End Get
    End Property

    Public Shared ReadOnly Property IsrBridgeNumber As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property IsrBridgeR1 As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property IsrBridgeR2 As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property
    Public Shared ReadOnly Property IsrBridgeR3 As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property
    Public Shared ReadOnly Property IsrBridgeR4 As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property MicronBridgeNumber As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared ReadOnly Property MicronBridgeR1 As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property MicronBridgeR2 As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property
    Public Shared ReadOnly Property MicronBridgeR3 As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property
    Public Shared ReadOnly Property MicronBridgeR4 As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property BridgeMeterEpsilon As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property


    Public Shared ReadOnly Property ExpectedResistance As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property ExpectedOpen As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property ExpectedResistanceEpsilon As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property ExpectedZeroResistanceEpsilon As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

    Public Shared ReadOnly Property ExpectedShort32ResistanceEpsilon As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
        End Get
    End Property

#End Region

End Class

Public Enum TestLocation
    Micron
    Isr
End Enum