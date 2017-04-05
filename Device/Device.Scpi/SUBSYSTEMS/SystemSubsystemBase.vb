Imports System.ComponentModel
''' <summary> Defines the contract that must be implemented by SCPI System Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SystemSubsystemBase
    Inherits VI.SystemSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " SCPI VERSION "

    ''' <summary> The SCPI revision. </summary>
    Private _ScpiRevision As Double?
    ''' <summary> Gets the cached version level of the SCPI standard implemented by the device. </summary>
    ''' <value> The SCPI revision. </value>
    Public Property ScpiRevision As Double?
        Get
            Return Me._ScpiRevision
        End Get
        Protected Set(value As Double?)
            Me._ScpiRevision = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Gets or sets the scpi revision query command. </summary>
    ''' <value> The scpi revision query command. </value>
    ''' <remarks> ':SYST:VERS?'</remarks>
    Protected Overridable ReadOnly Property ScpiRevisionQueryCommand As String = Scpi.Syntax.ScpiRevisionQueryCommand

    ''' <summary> Queries the version level of the SCPI standard implemented by the device. </summary>
    ''' <returns> System.Nullable{System.Double}. </returns>
    ''' <remarks> Sends the ':SYST:VERS?' query. </remarks>
    Public Function QueryScpiRevision() As Double?
        If Not String.IsNullOrWhiteSpace(Me.ScpiRevisionQueryCommand) Then
            Me._ScpiRevision = Me.Session.Query(0.0F, Me.ScpiRevisionQueryCommand)
        End If
        Return Me.ScpiRevision
    End Function

#End Region

End Class
