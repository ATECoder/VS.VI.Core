Partial Public Class SessionFactory

    ''' <summary> Use national visa session factor. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    Public Sub UseNationalVisaSessionFactory()
        Me.Factory = New isr.VI.National.Visa.SessionFactory
    End Sub

End Class
