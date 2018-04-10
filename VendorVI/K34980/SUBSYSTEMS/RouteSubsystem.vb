''' <summary> Defines a Route Subsystem for a Keysight 34980 Meter/Scanner. </summary>
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

#Region " CHANNEL "

    ''' <summary> Gets the closed Channel query command. </summary>
    ''' <value> The closed Channel query command. </value>
    <Obsolete("Not supported with 34980A")>
    Protected Overrides ReadOnly Property ClosedChannelQueryCommand As String = ""

    ''' <summary> Gets the closed Channel command format. </summary>
    ''' <value> The closed Channel command format. </value>
    <Obsolete("Use Channels commands with 34980A")>
    Protected Overrides ReadOnly Property ClosedChannelCommandFormat As String = ""

    ''' <summary> Gets the open Channel command format. </summary>
    ''' <value> The open Channel command format. </value>
    <Obsolete("Use Channels commands with 34980A")>
    Protected Overrides ReadOnly Property OpenChannelCommandFormat As String = ""

#End Region

#Region " CHANNELS CLOSED "

    ''' <summary> Gets the closed channels query command. </summary>
    ''' <value> The closed channels query command. </value>
    <Obsolete("Not supported with 34980A")>
    Protected Overrides ReadOnly Property ClosedChannelsQueryCommand As String = ""

    ''' <summary> Gets the closed channels command format. </summary>
    ''' <value> The closed channels command format. </value>
    Protected Overrides ReadOnly Property ClosedChannelsCommandFormat As String = ":ROUT:CLOS {0}"

#End Region

#Region " CHANNELS OPEN "

    ''' <summary> Gets the open channels command format. </summary>
    ''' <value> The open channels command format. </value>
    Protected Overrides ReadOnly Property OpenChannelsCommandFormat As String = ":ROUT:OPEN {0}"

    ''' <summary> Gets the open channels query command. </summary>
    ''' <value> The open channels query command. </value>
    <Obsolete("Not supported with 34980A")>
    Protected Overrides ReadOnly Property OpenChannelsQueryCommand As String = ""

    ''' <summary> Gets the open channels command. </summary>
    ''' <value> The open channels command. </value>
    Protected Overrides ReadOnly Property OpenChannelsCommand As String = ":ROUT:OPEN:ALL"

#End Region

#Region " CHANNELS "

    ''' <summary> Gets the recall channel pattern command format. </summary>
    ''' <value> The recall channel pattern command format. </value>
    <Obsolete("Not supported 34980A")>
    Protected Overrides ReadOnly Property RecallChannelPatternCommandFormat As String = ":ROUT:MEM:REC M{0}"

    ''' <summary> Gets the save channel pattern command format. </summary>
    ''' <value> The save channel pattern command format. </value>
    <Obsolete("Not supported 34980A")>
    Protected Overrides ReadOnly Property SaveChannelPatternCommandFormat As String = ":ROUT:MEM:SAVE M{0}"

#End Region

#Region " SCAN LIST "

    ''' <summary> Gets the scan list command query. </summary>
    ''' <value> The scan list query command. </value>
    Protected Overrides ReadOnly Property ScanListQueryCommand As String = ":ROUT:SCAN?"

    ''' <summary> Gets the scan list command format. </summary>
    ''' <value> The scan list command format. </value>
    Protected Overrides ReadOnly Property ScanListCommandFormat As String = ":ROUT:SCAN {0}"

#End Region

#Region " SLOT CARD TYPE "

    ''' <summary> Gets the slot card type query command format. </summary>
    ''' <value> The slot card type query command format. </value>
    Protected Overrides ReadOnly Property SlotCardTypeQueryCommandFormat As String = ":SYST:CTYPE?"

    ''' <summary> Gets the slot card type command format. </summary>
    ''' <value> The slot card type command format. </value>
    <Obsolete("Not supported 34980A")>
    Protected Overrides ReadOnly Property SlotCardTypeCommandFormat As String = ":ROUT:CONF:SLOT{0}:CTYPER {1}"

#End Region

#Region " SLOT CARD SETTLING TIME "

    ''' <summary> Gets the slot card settling time query command format. </summary>
    ''' <value> The slot card settling time query command format. </value>
    <Obsolete("Not supported 34980A")>
    Protected Overrides ReadOnly Property SlotCardSettlingTimeQueryCommandFormat As String = ""

    ''' <summary> Gets the slot card settling time command format. </summary>
    ''' <value> The slot card settling time command format. </value>
    <Obsolete("Not supported 34980A")>
    Protected Overrides ReadOnly Property SlotCardSettlingTimeCommandFormat As String = ""

#End Region

#Region " TERMINAL MODE "

    ''' <summary> Gets the terminals mode query command. </summary>
    ''' <value> The terminals mode command. </value>
    <Obsolete("Not supported 34980A")>
    Protected Overrides ReadOnly Property TerminalsModeQueryCommand As String = ""

    ''' <summary> Gets the terminals mode command format. </summary>
    ''' <value> The terminals mode command format. </value>
    <Obsolete("Not supported 34980A")>
    Protected Overrides ReadOnly Property TerminalsModeCommandFormat As String = ""

#End Region

#End Region

End Class
