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

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
MOVED TO THE BASE CLASS
#Region " LANGUAGE VERSION "

    ''' <summary> The Language revision. </summary>
    Private _LanguageRevision As Double?
    ''' <summary> Gets the cached version level of the Language standard implemented by the device. </summary>
    ''' <value> The Language revision. </value>
    Public Property LanguageRevision As Double?
        Get
            Return Me._LanguageRevision
        End Get
        Protected Set(value As Double?)
            Me._LanguageRevision = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Gets or sets the Language revision query command. </summary>
    ''' <value> The Language revision query command. </value>
    ''' <remarks> ':SYST:VERS?'</remarks>
    Protected Overridable ReadOnly Property LanguageRevisionQueryCommand As String = Language.Syntax.LanguageRevisionQueryCommand

    ''' <summary> Queries the version level of the Language standard implemented by the device. </summary>
    ''' <returns> System.Nullable{System.Double}. </returns>
    ''' <remarks> Sends the ':SYST:VERS?' query. </remarks>
    Public Function QueryLanguageRevision() As Double?
        If Not String.IsNullOrWhiteSpace(Me.LanguageRevisionQueryCommand) Then
            Me._LanguageRevision = Me.Session.Query(0.0F, Me.LanguageRevisionQueryCommand)
        End If
        Return Me.LanguageRevision
    End Function

#End Region

#End If
#End Region
