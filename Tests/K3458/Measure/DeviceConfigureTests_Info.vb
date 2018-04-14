Imports System.Configuration

Partial Friend NotInheritable Class TestInfo

#Region " DEVICE INFORMATION "

    ''' <summary> Gets the auto zero settings. </summary>
    ''' <value> The auto zero settings. </value>
    Public Shared ReadOnly Property AutoZero As Integer
        Get
            Return Convert.ToInt32(ConfigurationManager.AppSettings(NameOf(TestInfo.AutoZero)))
        End Get
    End Property

    ''' <summary> Gets the Sense Function settings. </summary>
    ''' <value> The Sense Function settings. </value>
    Public Shared ReadOnly Property SenseFunction As Integer
        Get
            Return Convert.ToInt32(ConfigurationManager.AppSettings(NameOf(TestInfo.SenseFunction)))
        End Get
    End Property

    ''' <summary> Gets the power line cycles settings. </summary>
    ''' <value> The power line cycles settings. </value>
    Public Shared ReadOnly Property PowerLineCycles As Double
        Get
            Return Convert.ToInt32(ConfigurationManager.AppSettings(NameOf(TestInfo.PowerLineCycles)))
        End Get
    End Property

    ''' <summary> Gets the math mode settings. </summary>
    ''' <value> The math mode settings. </value>
    Public Shared ReadOnly Property MathMode As Integer
        Get
            Return Convert.ToInt32(ConfigurationManager.AppSettings(NameOf(TestInfo.MathMode)))
        End Get
    End Property

    ''' <summary> Gets the store mathematics register. </summary>
    ''' <value> The store  mathematics register. </value>
    Public Shared ReadOnly Property StoreMathRegister As Integer
        Get
            Return Convert.ToInt32(ConfigurationManager.AppSettings(NameOf(TestInfo.StoreMathRegister)))
        End Get
    End Property

    ''' <summary> Gets the store math value settings. </summary>
    ''' <value> The store math value settings. </value>
    Public Shared ReadOnly Property StoreMathValue As Double
        Get
            Return Convert.ToInt32(ConfigurationManager.AppSettings(NameOf(TestInfo.StoreMathValue)))
        End Get
    End Property


#End Region

End Class
