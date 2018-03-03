Imports isr.VI.ExceptionExtensions
''' <summary> Defines the contract that must be implemented by R2D2 Status Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class StatusSubsystemBase
    Inherits VI.StatusSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystemBase" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.SessionBase">message based
    ''' session</see>. </param>
    Protected Sub New(ByVal visaSession As VI.SessionBase)
        MyBase.New(visaSession, Scpi.Syntax.NoErrorCompoundMessage)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Clears Error Queue.
    '''           </para></remarks>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Dim action As String = "enabling wait completion"
        Try
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{action};. ")
            Me.EnableWaitComplete()
        Catch ex As Exception
            ex.Data.Add($"data{ex.Data.Count}.resource", Me.Session.ResourceName)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Exception {action};. {ex.ToFullBlownString}")
        End Try
        Try
            Action = "querying identity"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{action};. ")
            Me.QueryIdentity()
        Catch ex As Exception
            ex.Data.Add($"data{ex.Data.Count}.resource", Me.Session.ResourceName)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Exception {action};. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Sets the subsystem to its preset state. </summary>
    Public Overrides Sub PresetKnownState()
        MyBase.PresetKnownState()
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.StandardEventStatus = 0
        Me.StandardEventEnableBitmask = 0
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identifies the talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        Mybase.IdentifyTalkers()
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region


End Class
