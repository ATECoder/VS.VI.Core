Public Class Console1
    Inherits isr.VI.Instrument.InstrumentPanelForm
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Me.AddInstrumentPanel("Switch/DMM", New VI.Tsp.K3700.K3700Panel)
        Me.AddListeners(My.Application.MyLog)
        MyBase.OnLoad(e)
    End Sub
End Class
