Namespace My

    ''' <summary> Provides assembly information for the class library. </summary>
    Public NotInheritable Class MyLibrary

        ''' <summary> Constructor that prevents a default instance of this class from being created. </summary>
        Private Sub New()
            MyBase.New()
        End Sub

        ''' <summary> Gets or sets the log. </summary>
        ''' <value> The log. </value>
        Public Shared ReadOnly Property MyLog As isr.Core.Pith.MyLog = New isr.Core.Pith.MyLog

        ''' <summary> Applies the given value. </summary>
        ''' <param name="value"> The value. </param>
        Public Shared Sub Apply(ByVal value As isr.Core.Pith.MyLog)
            MyLibrary._MyLog = value
        End Sub

        ''' <summary> Gets the identifier of the trace source. </summary>
        Public Const TraceEventId As Integer = ProjectTraceEventId.Device

        Public Const AssemblyTitle As String = "VI Device Library"
        Public Const AssemblyDescription As String = "Virtual Instrument Device Library"
        Public Const AssemblyProduct As String = "VI.Device.2017"

        ''' <summary> Gets the identify date. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <value> The identify date. </value>
        Public Shared Property IdentifyDate As Date

        ''' <summary> Identifies this talker. </summary>
        ''' <param name="talker"> The talker. </param>
        Public Shared Sub Identify(ByVal talker As isr.Core.Pith.ITraceMessageTalker)
            If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
            If DateTime.Now.Date > My.MyLibrary.IdentifyDate AndAlso talker.Listeners.ContainsListener(isr.Core.Pith.ListenerType.Logger) Then
                talker.Publish(TraceEventType.Information, MyLibrary.TraceEventId, My.MyLibrary.Identity)
                My.MyLibrary.IdentifyDate = DateTime.Now.Date
            End If
        End Sub

        ''' <summary> Gets the identity. </summary>
        ''' <value> The identity. </value>
        Public Shared ReadOnly Property Identity() As String
            Get
                Return $"{MyLibrary.AssemblyProduct} ID = {MyLibrary.TraceEventId:X}"
            End Get
        End Property

    End Class

End Namespace

