Public Class Console1
    Inherits isr.VI.Instrument.InstrumentPanelForm
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Dim enabled As Boolean = False
#If False Then
#ElseIf K2400 Then
        Me.AddInstrumentPanel("SourceMeter", New VI.K2400.K2400Panel)
        enabled = true
#ElseIf K3700 Then
        Me.AddInstrumentPanel("Switch/DMM", New VI.Tsp.K3700.K3700Panel)
        enabled = true
#ElseIf K7000 Then
        Me.AddInstrumentPanel("Switch", New VI.K7000.K7000Panel)
        enabled = True
#End If
        If enabled Then
            Me.AddListeners(My.Application.MyLog)
            MyBase.OnLoad(e)
        End If
    End Sub
End Class
