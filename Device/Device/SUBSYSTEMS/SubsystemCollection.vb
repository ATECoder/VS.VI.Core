Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
''' <summary> Collection of subsystems. </summary>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="12/21/2015" by="David" revision=""> Created. </history>
Public Class SubsystemCollection
    Inherits Collections.ObjectModel.Collection(Of SubsystemBase)
    Implements IPresettablePublisher, ITalker

#Region " CONSTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.ConstructorSafeSetter(New TraceMessageTalker)
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

#Region " ADD "

    ''' <summary>
    ''' Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="item"> The object to add to the
    '''                     <see cref="T:System.Collections.Generic.ICollection`1" />. </param>
    Public Overloads Sub Add(ByVal item As SubsystemBase)
        If item Is Nothing Then Throw New ArgumentNullException(NameOf(item))
        MyBase.Add(item)
        If Me.Talker IsNot Nothing Then item.AssignTalker(Me.Talker)
    End Sub

#End Region

#Region " CLEAR/DISPOSE "

    ''' <summary> Dispose items. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub DisposeItems()
        For Each element As IDisposable In Me.Items
            Try
                element.Dispose()
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
        Me.Clear()
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

    ''' <summary> Capture synchronization context. </summary>
    ''' <param name="syncContext"> Context for the synchronization. </param>
    Public Sub CaptureSyncContext(ByVal syncContext As Threading.SynchronizationContext)
        For Each ss As SubsystemBase In Me
            ss.CaptureSyncContext(syncContext)
        Next
    End Sub

#End Region

End Class

