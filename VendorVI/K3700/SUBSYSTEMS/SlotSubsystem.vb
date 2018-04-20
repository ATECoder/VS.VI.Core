Public Class SlotSubsystem
    Inherits isr.VI.Tsp.SlotSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="ChannelSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem"> A reference to a <see cref="VI.Tsp.StatusSubsystemBase">message
    '''                                based session</see>. </param>
    ''' <param name="slotNumber">      The slot number. </param>
    Public Sub New(ByVal slotNumber As Integer, ByVal statusSubsystem As VI.Tsp.StatusSubsystemBase)
        MyBase.New(slotNumber, statusSubsystem)
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

End Class
