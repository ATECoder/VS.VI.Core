Imports System.Configuration
Imports isr.VI.tsp.K3700

Partial Public NotInheritable Class TestInfo

#Region " BRIDGE METER INFORMATION "

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

#End Region

End Class

