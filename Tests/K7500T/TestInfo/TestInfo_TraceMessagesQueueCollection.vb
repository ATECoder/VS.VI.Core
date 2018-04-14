
Partial Friend NotInheritable Class TestInfo

    Partial Public Class TraceMessageQueueCollection
        Inherits ObjectModel.Collection(Of isr.Core.Pith.TraceMessagesQueue)
        Public Sub New()
            MyBase.New
            Me.Add(TestInfo.TraceMessagesQueueListener)
            Me.Add(isr.Core.Pith.My.MyLibrary.UnpublishedTraceMessages)
            Me.Add(isr.VI.My.MyLibrary.UnpublishedTraceMessages)
            Me.Add(isr.VI.Tsp2.My.MyLibrary.UnpublishedTraceMessages)
            Me.Add(isr.VI.Tsp2.K7500.My.MyLibrary.UnpublishedTraceMessages)
        End Sub
    End Class

End Class
