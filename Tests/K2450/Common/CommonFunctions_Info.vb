
Partial Public NotInheritable Class TestInfo

#Region " DEVICE INFORMATION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property ResourceModel As String
        Get
            Return My.MyAppSettingsReader.AppSettingValue()
        End Get
    End Property

    Public Shared Property TestLocation As TestLocation

    Private Shared _ResourceLocated As Boolean?

    ''' <summary> Gets the sentinel indicating if a resource was located. </summary>
    ''' <value> The resource located sentinel. </value>
    Public Shared ReadOnly Property ResourceLocated As Boolean
        Get
            If Not TestInfo._ResourceLocated.HasValue Then
                TestInfo._ResourceLocated = Not String.IsNullOrWhiteSpace(TestInfo.ResourceName)
            End If
            Return TestInfo._ResourceLocated.Value
        End Get
    End Property

    Private Shared _resourceName As String
    Public Shared ReadOnly Property ResourceName As String
        Get
            If String.IsNullOrWhiteSpace(TestInfo._resourceName) Then
                If isr.VI.Pith.ResourceNamesManager.Ping(TestInfo.IsrResourceName) Then
                    TestInfo._resourceName = TestInfo.IsrResourceName
                    TestInfo.TestLocation = TestLocation.Isr
                ElseIf isr.VI.Pith.ResourceNamesManager.Ping(TestInfo.MicronResourceName) Then
                    TestInfo._resourceName = TestInfo.MicronResourceName
                    TestInfo.TestLocation = TestLocation.Micron
                Else
                    TestInfo._resourceName = ""
                    TestInfo.TestLocation = TestLocation.None
                End If
            End If
            Return TestInfo._resourceName
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

#Region " STATUS SUBSYSTEM INFORMATION "

    ''' <summary> Gets the Initial power line cycles settings. </summary>
    ''' <value> The power line cycles settings. </value>
    Public Shared ReadOnly Property InitialPowerLineCycles As Double
        Get
            Return My.MyAppSettingsReader.AppSettingDouble()
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

End Class

Public Enum TestLocation
    None
    Micron
    Isr
End Enum