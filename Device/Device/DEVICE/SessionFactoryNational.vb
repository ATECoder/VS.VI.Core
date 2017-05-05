Partial Public Class SessionFactory

    ''' <summary> Use national visa session factor. </summary>
    Public Sub UseNationalVisaSessionFactory()
        Me.Factory = New isr.VI.National.Visa.SessionFactory
    End Sub

End Class
