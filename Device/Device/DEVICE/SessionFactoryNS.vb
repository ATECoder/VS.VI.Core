Partial Public Class SessionFactory

#If NI_VISA_NS Then
    ''' <summary> Use national legacy visa session factor. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    Public Sub UseNationalLegacyVisaSessionFactory()
        Me.Factory = New isr.VI.National.VisaNS.SessionFactory
    End Sub
#End If

End Class
