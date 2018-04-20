''' <summary> Defines a Channel Subsystem for a Keithley 3700 instrument. </summary>
''' <license> (c) 2016 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/13/2016" by="David" revision=""> Created. </history>
Public Class ChannelSubsystem
    Inherits isr.VI.ChannelSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="ChannelSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub
#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " CHANNELS "

    ''' <summary> Gets the closed channels query command. </summary>
    ''' <value> The closed channels query command. </value>
    Protected Overrides ReadOnly Property ClosedChannelsQueryCommand As String = "_G.print(_G.channel.getclose('allslots'))"

    ''' <summary> Gets the closed channels command format. </summary>
    ''' <value> The closed channels command format. </value>
    Protected Overrides ReadOnly Property ClosedChannelsCommandFormat As String = "_G.channel.close('{0}')"

    ''' <summary> Gets the open channels command format. </summary>
    ''' <value> The open channels command format. </value>
    Protected Overrides ReadOnly Property OpenChannelsCommandFormat As String = "_G.channel.open('{0}')"

    ''' <summary> Gets the open channels command. </summary>
    ''' <value> The open channels command. </value>
    Protected Overrides ReadOnly Property OpenChannelsCommand As String = "_G.channel.open('allslots')"

#End Region

#End Region

End Class
