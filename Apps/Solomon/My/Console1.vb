Public Class Console1
    Inherits isr.VI.Tsp.K3700.MovingWindowForm
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Me.AddListeners(My.Application.Log)
        MyBase.OnLoad(e)
    End Sub
End Class
