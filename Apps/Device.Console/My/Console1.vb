Public Class Console1
#If TTM Then
    Inherits isr.VI.Ttm.Console
#Else
    Inherits isr.VI.Instrument.InstrumentPanelForm
#End If
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnLoad(e As EventArgs)
        Dim enabled As Boolean = False
#If False Then
#ElseIf E4990 Then
        Me.AddInstrumentPanel("SourceMeter", New VI.E4990.E4990Panel)
        enabled = true
#ElseIf K2000 Then
        Me.AddInstrumentPanel("Meter", New VI.K2000.K2000Panel, True)
        enabled = true
#ElseIf K2400 Then
        Me.AddInstrumentPanel("SourceMeter", New VI.K2400.K2400Panel, True)
        enabled = true
#ElseIf K3700 Then
        Me.AddInstrumentPanel("DMM/Scanner", New VI.Tsp.K3700.K3700Panel, True)
        enabled = True
#ElseIf K7000 Then
        Me.AddInstrumentPanel("Scanner", New VI.K7000.K7000Panel, True)
        enabled = True
#ElseIf K7500 Then
        Me.AddInstrumentPanel("Meter", New VI.K7500.K7500Panel, True)
        enabled = True
#ElseIf K34980 Then
        Me.AddInstrumentPanel("Scanner", New VI.K34980.K34980Panel, True)
        enabled = True
#ElseIf TTM Then
        enabled = true
#End If
        If enabled Then
            Me.AddListener(My.Application.MyLog)
            MyBase.OnLoad(e)
        End If
    End Sub
End Class
