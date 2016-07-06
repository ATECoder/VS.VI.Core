''' <summary> Defines a Scpi Instrument Subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/1/2016" by="David" revision="4.0.6026"> Created. </history>
Public MustInherit Class InstrumentSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="InstrumentSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " DMM Installed "

    ''' <summary> DMM Installed. </summary>
    Private _DmmInstalled As Boolean?

    ''' <summary> Gets or sets the cached DMM Installed sentinel. </summary>
    ''' <value> <c>null</c> if DMM Installed is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property DmmInstalled As Boolean?
        Get
            Return Me._DmmInstalled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.DmmInstalled, value) Then
                Me._DmmInstalled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.DmmInstalled))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the DMM Installed query command. </summary>
    ''' <value> The DMM Installed query command. </value>
    Protected Overridable ReadOnly Property DmmInstalledQueryCommand As String

    ''' <summary> Queries the DMM Installed sentinel. Also sets the
    '''           <see cref="DmmInstalled">DMM Installed</see> sentinel. </summary>
    ''' <returns> <c>null</c> Instrument status is not known; <c>True</c> if DmmInstalled; otherwise, <c>False</c>. </returns>
    Public Function QueryDmmInstalled() As Boolean?
        Me.Session.MakeEmulatedReplyIfEmpty(Me.DmmInstalled.GetValueOrDefault(True))
        If Not String.IsNullOrWhiteSpace(Me.DmmInstalledQueryCommand) Then
            Me.DmmInstalled = Me.Session.Query(Me.DmmInstalled.GetValueOrDefault(True), Me.DmmInstalledQueryCommand)
        End If
        Return Me.DmmInstalled
    End Function

#End Region

End Class
