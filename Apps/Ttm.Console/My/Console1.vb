Public Class Console1
    Inherits isr.VI.Ttm.Console
    Protected Overrides Sub OnLoad(e As EventArgs)
        Me.AddListener(My.Application.MyLog)
        MyBase.OnLoad(e)
    End Sub
End Class
