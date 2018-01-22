Imports isr.Core.Pith
Imports isr.Core.Pith.NumericExtensions

Public Class ResistanceMeasureCollection
    Inherits ObjectModel.Collection(Of ResistanceMeasureBase)
    Implements IPresettablePublisher, ITalker

#Region " CONSTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Clears the queues and resets all registers to zero. Sets the subsystem properties to
    ''' the following CLS default values:<para>
    ''' </para> </summary>
    Public Sub ClearExecutionState() Implements IPresettable.ClearExecutionState
        For Each element As IPresettable In Me.Items
            element.ClearExecutionState()
        Next
    End Sub

    ''' <summary> Performs a reset and additional custom setting for the subsystem:<para>
    ''' </para> </summary>
    Public Sub InitKnownState() Implements IPresettable.InitKnownState
        For Each element As IPresettable In Me.Items
            element.InitKnownState()
        Next
    End Sub

    ''' <summary> Gets subsystem to the following default system preset values:<para>
    ''' </para> </summary>
    Public Sub PresetKnownState() Implements IPresettable.PresetKnownState
        For Each element As IPresettable In Me.Items
            element.PresetKnownState()
        Next
    End Sub

    ''' <summary> Restore member properties to the following RST or System Preset values:<para>
    ''' </para> </summary>
    Public Sub ResetKnownState() Implements IPresettable.ResetKnownState
        For Each element As IPresettable In Me.Items
            element.ResetKnownState()
        Next
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Gets or sets the publishable sentinel. </summary>
    ''' <value> The publishable. </value>
    Public Property Publishable As Boolean Implements IPublisher.Publishable

    ''' <summary> Publishes all values. </summary>
    Public Sub Publish() Implements IPublisher.Publish
        For Each element As IPublisher In Me.Items
            element.Publish()
        Next
    End Sub

    ''' <summary> Resume property events. </summary>
    Public Sub ResumePublishing() Implements IPublisher.ResumePublishing
        For Each element As IPublisher In Me.Items
            element.ResumePublishing()
            Me.Publishable = element.Publishable
        Next
    End Sub

    ''' <summary> Suspend publishing. </summary>
    Public Sub SuspendPublishing() Implements IPublisher.SuspendPublishing
        For Each element As IPublisher In Me.Items
            element.SuspendPublishing()
            Me.Publishable = element.Publishable
        Next
    End Sub

#End Region

#Region " CLEAR/DISPOSE "

    ''' <summary> Dispose items. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub DisposeItems()
        Me.RemoveListeners()
        For Each element As IDisposable In Me.Items
            Try
                element.Dispose()
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToString)
            End Try
        Next
        Me.Clear()
    End Sub

#End Region

End Class
