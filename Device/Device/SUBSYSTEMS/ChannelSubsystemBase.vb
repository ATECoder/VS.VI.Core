''' <summary> Defines a System Subsystem for a TSP System. </summary>
''' <license> (c) 2016 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/13/2016" by="David" revision=""> Created. </history>
Public MustInherit Class ChannelSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystemBase">status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._ClosedChannels = ""
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
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " CLOSED CHANNELS "

    Private _ClosedChannels As String

    ''' <summary> Gets or sets the closed channels. </summary>
    ''' <remarks> Nothing is not set. </remarks>
    ''' <value> The closed channels. </value>
    Public Overloads Property ClosedChannels As String
        Get
            Return Me._ClosedChannels
        End Get
        Protected Set(ByVal value As String)
            If Not String.Equals(value, Me.ClosedChannels) Then
                Me._ClosedChannels = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Applies the closed channels described by value. </summary>
    ''' <param name="value">   The scan list. </param>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function ApplyClosedChannels(ByVal value As String, ByVal timeout As TimeSpan) As String
        Me.WriteClosedChannels(value, timeout)
        Return Me.QueryClosedChannels()
    End Function

    ''' <summary> Gets the closed channels query command. </summary>
    ''' <value> The closed channels query command. </value>
    ''' <remarks> :ROUT:CLOS </remarks>
    Protected Overridable ReadOnly Property ClosedChannelsQueryCommand As String

    ''' <summary> Queries closed channels. </summary>
    ''' <returns> The closed channels. </returns>
    Public Function QueryClosedChannels() As String
        Dim value As String = Me.Query(Me.ClosedChannels, Me.ClosedChannelsQueryCommand)
        Me.ClosedChannels = If(VI.Pith.SessionBase.EqualsNil(value), "", value)
        Return Me.ClosedChannels
    End Function

    ''' <summary> Gets the closed channels command format. </summary>
    ''' <value> The closed channels command format. </value>
    ''' <remarks> :ROUT:CLOS {0} </remarks>
    Protected Overridable ReadOnly Property ClosedChannelsCommandFormat As String

    ''' <summary> Writes a closed channels. </summary>
    ''' <param name="value">   The scan list. </param>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function WriteClosedChannels(ByVal value As String, ByVal timeout As TimeSpan) As String
        Me.Write(Me.ClosedChannelsCommandFormat, value)
        If timeout > TimeSpan.Zero Then Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        Me.ClosedChannels = value
        Return Me.ClosedChannels
    End Function

    ''' <summary> Gets the open channels command format. </summary>
    ''' <value> The open channels command format. </value>
    ''' <remarks> SCPI: :ROUT:OPEN:ALL </remarks>
    Protected Overridable ReadOnly Property OpenChannelsCommandFormat As String

    ''' <summary> Open the specified channels in the list and read back the closed channels. </summary>
    ''' <param name="channelList"> List of channels. </param>
    ''' <param name="timeout">     The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function ApplyOpenChannels(ByVal channelList As String, ByVal timeout As TimeSpan) As String
        Me.WriteOpenChannels(channelList, timeout)
        Return Me.QueryClosedChannels
    End Function

    ''' <summary> Opens the specified channels in the list. </summary>
    ''' <param name="channelList"> List of channels. </param>
    ''' <param name="timeout">     The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function WriteOpenChannels(ByVal channelList As String, ByVal timeout As TimeSpan) As String
        Me.Session.Execute(String.Format(Me.OpenChannelsCommandFormat, channelList))
        If timeout > TimeSpan.Zero Then Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        ' set to nothing to indicate that the value is not known -- requires reading.
        Me.ClosedChannels = Nothing
        Return Me.ClosedChannels
    End Function

    ''' <summary> Gets the open channels command. </summary>
    ''' <value> The open channels command. </value>
    Protected Overridable ReadOnly Property OpenChannelsCommand As String

    ''' <summary> Opens all channels and reads back the closed channels. </summary>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function ApplyOpenAll(ByVal timeout As TimeSpan) As String
        Me.WriteOpenAll(timeout)
        Return Me.QueryClosedChannels
    End Function

    ''' <summary> Opens all channels. </summary>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function WriteOpenAll(ByVal timeout As TimeSpan) As String
        Me.Session.Execute(Me.OpenChannelsCommand)
        If timeout > TimeSpan.Zero Then Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        ' set to nothing to indicate that the value is not known -- requires reading.
        Me.ClosedChannels = Nothing
        Return Me.ClosedChannels
    End Function

#End Region

End Class
