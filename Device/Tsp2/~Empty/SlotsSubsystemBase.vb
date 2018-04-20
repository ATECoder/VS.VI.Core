''' <summary> A slots subsystem base. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="2/15/2016" by="David" revision=""> Created. </history>
Public MustInherit Class SlotsSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
    ''' </summary>
    ''' <param name="maxSlotCount">    Number of maximum slots. </param>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystemBase">status
    '''                                Subsystem</see>. </param>
    Protected Sub New(ByVal maxSlotCount As Integer, ByVal statusSubsystem As StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._MaximumSlotCount = maxSlotCount
        Me._Slots = New SlotCollection
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me._Slots IsNot Nothing Then
                    Dim l As New List(Of SlotSubsystemBase)(Me.Slots)
                    Do While l.Count > 0
                        Dim s As SlotSubsystemBase = l(0)
                        l.Remove(s)
                        Me._Slots.Remove(s.SlotNumber)
                        s.Dispose()
                        s = Nothing
                    Loop
                    Me._Slots = Nothing
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        For Each s As SlotSubsystemBase In Me.Slots
            s.QuerySlotExists()
            If s.IsSlotExists Then
                s.QuerySupportsInterlock()
                s.QueryInterlocksState()
            End If
        Next
    End Sub

#End Region

#Region " EXISTS "

    ''' <summary> The slots. </summary>
    Public ReadOnly Property Slots As SlotCollection

    ''' <summary> Gets or sets the number of maximum slots. </summary>
    ''' <value> The number of maximum slots. </value>
    Public ReadOnly Property MaximumSlotCount As Integer

#End Region

End Class

''' <summary> Collection of slots. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="2/15/2016" by="David" revision=""> Created. </history>
Public Class SlotCollection
    Inherits Collections.ObjectModel.KeyedCollection(Of Integer, SlotSubsystemBase)

    Protected Overrides Function GetKeyForItem(item As SlotSubsystemBase) As Integer
        If item IsNot Nothing Then
            Return item.SlotNumber
        End If
    End Function

End Class