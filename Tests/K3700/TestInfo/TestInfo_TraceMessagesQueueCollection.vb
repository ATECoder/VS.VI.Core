
Partial Friend NotInheritable Class TestInfo

    Partial Public Class TraceMessageQueueCollection
        Inherits ObjectModel.Collection(Of isr.Core.Pith.TraceMessagesQueue)
        Public Sub New()
            MyBase.New
            Me.Add(TestInfo.TraceMessagesQueueListener)
            Me.Add(isr.Core.Pith.My.MyLibrary.UnpublishedTraceMessages)
            Me.Add(isr.VI.My.MyLibrary.UnpublishedTraceMessages)
            Me.Add(isr.VI.Tsp.My.MyLibrary.UnpublishedTraceMessages)
            Me.Add(isr.VI.Tsp.K3700.My.MyLibrary.UnpublishedTraceMessages)
        End Sub
    End Class

End Class
