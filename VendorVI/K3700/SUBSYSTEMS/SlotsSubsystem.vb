Public Class SlotsSubsystem
    Inherits isr.VI.Tsp.SlotsSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ChannelSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.Tsp.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal maxSlotCount As Integer, ByVal statusSubsystem As VI.Tsp.StatusSubsystemBase)
        MyBase.New(maxSlotCount, statusSubsystem)
        Me._SlotList = New List(Of SlotSubsystem)
        For i As Integer = 1 To Me.MaximumSlotCount
            Dim s As New SlotSubsystem(i, statusSubsystem)
            Me._SlotList.Add(s)
        Next
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.AsyncNotifyPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " I PRESETTABLE "

    Private _SlotList As List(Of SlotSubsystem)

    Public Overrides Sub InitKnownState()
        For Each s As SlotSubsystemBase In Me._SlotList
            Me.Slots.Add(s)
        Next
        MyBase.InitKnownState()
    End Sub

#End Region


End Class
