Imports System.Configuration

Partial Public NotInheritable Class TestInfo

#Region " DEVICE INFORMATION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property ResourceModel As String
        Get
            Return ConfigurationManager.AppSettings(NameOf(TestInfo.ResourceModel))
        End Get
    End Property

    ''' <summary> Gets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Shared ReadOnly Property ResourceName As String
        Get
            Return ConfigurationManager.AppSettings(NameOf(TestInfo.ResourceName))
        End Get
    End Property

    ''' <summary> Gets the Title of the resource. </summary>
    ''' <value> The Title of the resource. </value>
    Public Shared ReadOnly Property ResourceTitle As String
        Get
            Return ConfigurationManager.AppSettings(NameOf(TestInfo.ResourceTitle))
        End Get
    End Property

    ''' <summary> Gets the auto zero settings. </summary>
    ''' <value> The auto zero settings. </value>
    Public Shared ReadOnly Property AutoZero As Integer
        Get
            Return Convert.ToInt32(ConfigurationManager.AppSettings(NameOf(TestInfo.AutoZero)))
        End Get
    End Property

    ''' <summary> Gets the power line cycles settings. </summary>
    ''' <value> The power line cycles settings. </value>
    Public Shared ReadOnly Property PowerLineCycles As Double
        Get
            Return Convert.ToInt32(ConfigurationManager.AppSettings(NameOf(TestInfo.PowerLineCycles)))
        End Get
    End Property

#End Region

End Class
