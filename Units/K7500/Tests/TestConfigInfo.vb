Imports System.Configuration

Partial Public NotInheritable Class TestInfo

#Region " CONSTRUCTOR "

    Private Sub New()
        MyBase.New
    End Sub

#End Region

#Region " CONFIGURATION INFORMTION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property Exists As Boolean
        Get
            Return String.Equals("True", ConfigurationManager.AppSettings(NameOf(TestInfo.Exists)), StringComparison.OrdinalIgnoreCase)
        End Get
    End Property

#End Region

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

#End Region

End Class
