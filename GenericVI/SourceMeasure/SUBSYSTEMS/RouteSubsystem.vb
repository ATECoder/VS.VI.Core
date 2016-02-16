''' <summary> Defines a SCPI Route Subsystem for Source Measure SCPI devices. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class RouteSubsystem
    Inherits VI.Scpi.RouteSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="RouteSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
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
                Me.AsyncNotifyPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " CLOSED CHANNEL "

    ''' <summary> Gets the closed Channel query command. </summary>
    ''' <value> The closed Channel query command. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property ClosedChannelQueryCommand As String = ""

    ''' <summary> Gets the closed Channel command format. </summary>
    ''' <value> The closed Channel command format. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property ClosedChannelCommandFormat As String = ""

    ''' <summary> Gets the open channels command format. </summary>
    ''' <value> The open channels command format. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property OpenChannelCommandFormat As String = ""

#End Region

#Region " CLOSED CHANNELS "

    ''' <summary> Gets the closed channels query command. </summary>
    ''' <value> The closed channels query command. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property ClosedChannelsQueryCommand As String = ""

    ''' <summary> Gets the closed channels command format. </summary>
    ''' <value> The closed channels command format. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property ClosedChannelsCommandFormat As String = ""

    ''' <summary> Gets the open channels command format. </summary>
    ''' <value> The open channels command format. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property OpenChannelsCommandFormat As String = ""

#End Region

#Region " CHANNELS "

    ''' <summary> Gets the recall channel pattern command format. </summary>
    ''' <value> The recall channel pattern command format. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property RecallChannelPatternCommandFormat As String = ""

    ''' <summary> Gets the save channel pattern command format. </summary>
    ''' <value> The save channel pattern command format. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property SaveChannelPatternCommandFormat As String = ""

    ''' <summary> Gets the open channels command. </summary>
    ''' <value> The open channels command. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property OpenChannelsCommand As String = ""

#End Region

#Region " SCAN LIST "

    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property ScanListQueryCommand As String = ""

    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property ScanListCommandFormat As String = ""

#End Region

#Region " SLOT CARD TYPE "

    ''' <summary> Gets the slot card type command format. </summary>
    ''' <value> The slot card type command format. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property SlotCardTypeCommandFormat As String = ""

    ''' <summary> Gets the slot card type query command format. </summary>
    ''' <value> The slot card type query command format. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property SlotCardTypeQueryCommandFormat As String = ""

#End Region

#Region " SLOT CARD SETTLING TIME "

    ''' <summary> Gets the slot card settling time command format. </summary>
    ''' <value> The slot card settling time command format. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property SlotCardSettlingTimeCommandFormat As String = ""

    ''' <summary> Gets the slot card settling time query command format. </summary>
    ''' <value> The slot card settling time query command format. </value>
    <Obsolete("Not supported for the Source Meters.")>
    Protected Overrides ReadOnly Property SlotCardSettlingTimeQueryCommandFormat As String = ""

#End Region

#Region " TERMINAL MODE "

    ''' <summary> Gets the terminal mode query command. </summary>
    ''' <value> The terminal mode command. </value>
    Protected Overrides ReadOnly Property TerminalModeQueryCommand As String = ":ROUT:TERM?"

    ''' <summary> Gets the terminal mode command format. </summary>
    ''' <value> The terminal mode command format. </value>
    Protected Overrides ReadOnly Property TerminalModeCommandFormat As String = ":ROUT:TERM {0}"

#End Region

#End Region

End Class
