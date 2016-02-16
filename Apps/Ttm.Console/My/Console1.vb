Public Class Console1
    Inherits isr.VI.Ttm.Console
    Protected Overrides Sub OnLoad(e As EventArgs)
        Me.AddListeners(My.Application.Log)
        MyBase.OnLoad(e)
    End Sub
End Class
