Partial Public Class SessionFactory

#If NI_VISA_NS Then
    ''' <summary> Use national legacy visa session factor. </summary>
    Public Sub UseNationalLegacyVisaSessionFactory()
        Me.Factory = New isr.VI.National.VisaNS.SessionFactory
    End Sub
#End If

End Class
