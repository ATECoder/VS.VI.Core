Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
Partial Public Class SubsystemBase
    Implements ITalker

    ''' <summary> Constructor-Safe talker setter. </summary>
    ''' <param name="talker"> The talker. </param>
    Private Sub ConstructorSafeTalkerSetter(ByVal talker As ITraceMessageTalker)
        Me._Talker = talker
        If Me._Talker IsNot Nothing Then
            AddHandler Me._Talker.DateChanged, AddressOf Me.HandleTalkerDateChange
        End If
    End Sub

    Private _Talker As ITraceMessageTalker
    ''' <summary> Gets the trace message talker. </summary>
    ''' <value> The trace message talker. </value>
    Public Property Talker As ITraceMessageTalker
        Get
            Return Me._Talker
        End Get
        Private Set(value As ITraceMessageTalker)
            If Me._Talker IsNot Nothing Then
                RemoveHandler Me._Talker.DateChanged, AddressOf Me.HandleTalkerDateChange
                Me.RemoveListeners()
            End If
            Me.ConstructorSafeTalkerSetter(value)
        End Set
    End Property

    Private IsAssignedTalker As Boolean
    ''' <summary> Assigns a talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overridable Sub AssignTalker(ByVal talker As ITraceMessageTalker)
        Me.IsAssignedTalker = talker IsNot Nothing
        Me.Talker = talker
        If talker IsNot Nothing Then Me.IdentifyTalkers()
    End Sub

    ''' <summary> Handles the talker date change. </summary>
    ''' <param name="log">          The log. </param>
    ''' <param name="assemblyInfo"> Information describing my assembly. </param>
    Protected Overridable Sub HandleTalkerDateChange(ByVal log As MyLog, ByVal assemblyInfo As MyAssemblyInfo)
        If log Is Nothing Then Throw New ArgumentNullException(NameOf(log))
        If assemblyInfo Is Nothing Then Throw New ArgumentNullException(NameOf(assemblyInfo))
        Me.Talker.PublishOverride(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Log at;. {log.FullLogFileName}")
        Me.Talker.PublishOverride(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Local time {assemblyInfo.LocalUtcCaption}")
    End Sub

    ''' <summary> Handles the talker date change. </summary>
    Protected Overridable Sub HandleTalkerDateChange()
        Me.IdentifyTalkers()
    End Sub

    ''' <summary> Identifies talkers. </summary>
    Protected Overridable Sub IdentifyTalkers()
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Handles the talker date change. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub HandleTalkerDateChange(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.HandleTalkerDateChange()
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
        End Try
    End Sub

    ''' <summary>
    ''' Removes the private listeners. Removes all listeners if the talker was not assigned.
    ''' </summary>
    Public Overridable Sub RemoveListeners() Implements ITalker.RemoveListeners
        Me.RemovePrivateListeners()
        If Not Me.IsAssignedTalker Then Me.Talker.RemoveListeners()
    End Sub

    Private _PrivateListeners As List(Of IMessageListener)
    ''' <summary> Gets the private listeners. </summary>
    ''' <value> The private listeners. </value>
    Public Overridable ReadOnly Property PrivateListeners As IEnumerable(Of IMessageListener) Implements ITalker.PrivateListeners
        Get
            If Me._PrivateListeners Is Nothing Then _PrivateListeners = New List(Of IMessageListener)
            Return Me._PrivateListeners
        End Get
    End Property

    ''' <summary> Adds a private listener. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overridable Sub AddPrivateListener(ByVal listener As IMessageListener) Implements ITalker.AddPrivateListener
        If Me.PrivateListeners IsNot Nothing Then
            Me._PrivateListeners.Add(listener)
        End If
        Me.AddListener(listener)
    End Sub

    ''' <summary> Adds private listeners. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overridable Sub AddPrivateListeners(ByVal listeners As IEnumerable(Of IMessageListener)) Implements ITalker.AddPrivateListeners
        If listeners Is Nothing Then Throw New ArgumentNullException(NameOf(listeners))
        If Me.PrivateListeners IsNot Nothing Then
            Me._PrivateListeners.AddRange(listeners)
        End If
        Me.AddListeners(listeners)
    End Sub

    ''' <summary> Adds private listeners. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="talker"> The talker. </param>
    Public Overridable Sub AddPrivateListeners(ByVal talker As ITraceMessageTalker) Implements ITalker.AddPrivateListeners
        If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
        Me.AddPrivateListeners(talker.Listeners)
    End Sub

    ''' <summary> Removes the private listener described by listener. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overridable Sub RemovePrivateListener(ByVal listener As IMessageListener) Implements ITalker.RemovePrivateListener
        Me.RemoveListener(listener)
        If Me.PrivateListeners IsNot Nothing Then
            Me._PrivateListeners.Remove(listener)
        End If
    End Sub

    ''' <summary> Removes the private listeners. </summary>
    Public Overridable Sub RemovePrivateListeners() Implements ITalker.RemovePrivateListeners
        For Each listener As IMessageListener In Me.PrivateListeners
            Me.RemoveListener(listener)
        Next
        Me._PrivateListeners.Clear()
    End Sub

    ''' <summary> Removes the listener described by listener. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overridable Sub RemoveListener(ByVal listener As IMessageListener) Implements ITalker.RemoveListener
        Me.Talker.RemoveListener(listener)
    End Sub

    ''' <summary> Removes the specified listeners. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overridable Sub RemoveListeners(ByVal listeners As IEnumerable(Of IMessageListener)) Implements ITalker.RemoveListeners

        If listeners Is Nothing Then Throw New ArgumentNullException(NameOf(listeners))
        For Each listener As IMessageListener In listeners
            Me.RemoveListener(listener)
        Next
    End Sub

    ''' <param name="listener"> The listener. </param>
    Public Overridable Sub AddListener(ByVal listener As IMessageListener) Implements ITalker.AddListener
        Me.Talker.AddListener(listener)
        Me.IdentifyTalkers()
    End Sub

    ''' <summary> Adds the listeners. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overridable Sub AddListeners(ByVal listeners As IEnumerable(Of IMessageListener)) Implements ITalker.AddListeners
        If listeners Is Nothing Then Throw New ArgumentNullException(NameOf(listeners))
        For Each listener As IMessageListener In listeners
            Me.AddListener(listener)
        Next
    End Sub

    ''' <summary> Adds the listeners. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overridable Sub AddListeners(ByVal talker As ITraceMessageTalker) Implements ITalker.AddListeners
        If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
        Me.AddListeners(talker.Listeners)
    End Sub

    ''' <summary> Applies the trace level to all listeners to the specified type. </summary>
    ''' <param name="listenerType"> Type of the listener. </param>
    ''' <param name="value">        The value. </param>
    Public Overridable Sub ApplyListenerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType) Implements ITalker.ApplyListenerTraceLevel
        ' this should apply only to the listeners associated with this form
        ' Not this: Me.Talker.ApplyListenerTraceLevel(listenerType, value)
        Me.IdentifyTalkers()
    End Sub

    ''' <summary> Applies the trace level type to all talkers. </summary>
    ''' <param name="listenerType"> Type of the trace level. </param>
    ''' <param name="value">        The value. </param>
    Public Overridable Sub ApplyTalkerTraceLevel(ByVal listenerType As ListenerType, ByVal value As TraceEventType) Implements ITalker.ApplyTalkerTraceLevel
        Me.Talker.ApplyTalkerTraceLevel(listenerType, value)
    End Sub

    ''' <summary> Applies the talker trace levels described by talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overridable Sub ApplyTalkerTraceLevels(ByVal talker As ITraceMessageTalker) Implements ITalker.ApplyTalkerTraceLevels
        Me.Talker.ApplyTalkerTraceLevels(talker)
    End Sub

    ''' <summary> Applies the talker listeners trace levels described by talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overridable Sub ApplyListenerTraceLevels(ByVal talker As ITraceMessageTalker) Implements ITalker.ApplyListenerTraceLevels
        Me.Talker.ApplyListenerTraceLevels(talker)
    End Sub

End Class

