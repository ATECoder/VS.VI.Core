Imports System.ComponentModel
''' <summary> Defines the contract that must be implemented by System Subsystem. </summary>
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
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " INTERFACE "

    ''' <summary> Supports clear interface. </summary>
    ''' <returns> <c>True</c> if supports clearing the interface. </returns>
    Public Function SupportsClearInterface() As Boolean
        Return Me.Session.SupportsClearInterface
    End Function

    ''' <summary> Clears the interface. </summary>
    Public Sub ClearInterface()
        If Me.SupportsClearInterface Then Me.Session.ClearInterface()
    End Sub

    ''' <summary> Clears the device (SDC). </summary>
    Public Sub ClearDevice()
        Me.Session.ClearDevice()
    End Sub

#End Region

#Region " MEMORY MANAGEMENT "

    ''' <summary> Gets or sets the initialize memory command. </summary>
    ''' <value> The initialize memory command. </value>
    Protected Overridable ReadOnly Property InitializeMemoryCommand As String

    ''' <summary> Initializes battery backed RAM. This initializes trace, source list, user-defined
    ''' math, source-memory locations, standard save setups, and all call math expressions. </summary>
    ''' <remarks> Sends the <see cref="InitializeMemoryCommand"/> message. </remarks>
    Public Sub InitializeMemory()
        If Not String.IsNullOrWhiteSpace(Me.InitializeMemoryCommand) Then
            Me.Session.WriteLine(Me.InitializeMemoryCommand)
        End If
    End Sub

#End Region

#Region " FAN LEVEL "

    ''' <summary> The Route Fan Level. </summary>
    Private _FanLevel As FanLevel?

    ''' <summary> Gets or sets the cached Route Fan Level. </summary>
    ''' <value> The Route Fan Level or null if unknown. </value>
    Public Property FanLevel As FanLevel?
        Get
            Return Me._FanLevel
        End Get
        Protected Set(ByVal value As FanLevel?)
            If Not Nullable.Equals(Me.FanLevel, value) Then
                Me._FanLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Route Fan Level. </summary>
    ''' <param name="value"> The <see cref="FanLevel">Route Fan Level</see>. </param>
    ''' <returns> The Route Fan Level or null if unknown. </returns>
    Public Function ApplyFanLevel(ByVal value As FanLevel) As FanLevel?
        Me.WriteFanLevel(value)
        Return Me.QueryFanLevel()
    End Function

    ''' <summary> Gets the Fan Level query command. </summary>
    ''' <value> The Fan Level command. </value>
    Protected Overridable ReadOnly Property FanLevelQueryCommand As String

    ''' <summary> Queries the Route Fan Level. Also sets the <see cref="FanLevel">output
    ''' on</see> sentinel. </summary>
    ''' <returns> The Route Fan Level or null if unknown. </returns>
    Public Function QueryFanLevel() As FanLevel?
        Me.FanLevel = Me.Query(Of FanLevel)(Me.FanLevelQueryCommand, Me.FanLevel)
        Return Me.FanLevel
    End Function

    ''' <summary> Gets the Fan Level command format. </summary>
    ''' <value> The Fan Level command format. </value>
    Protected Overridable ReadOnly Property FanLevelCommandFormat As String

    ''' <summary> Writes the Route Fan Level. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Fan Level. </param>
    ''' <returns> The Route Fan Level or null if unknown. </returns>
    Public Function WriteFanLevel(ByVal value As FanLevel) As FanLevel?
        Me.FanLevel = Me.Write(Of FanLevel)(Me.FanLevelCommandFormat, value)
        Return Me.FanLevel
    End Function

#End Region

#Region " FIRMWARE VERSION "

    ''' <summary> The Firmware Version. </summary>
    Private _FirmwareVersion As Double?
    ''' <summary> Gets the cached version the Firmware. </summary>
    ''' <value> The Firmware Version. </value>
    Public Property FirmwareVersion As Double?
        Get
            Return Me._FirmwareVersion
        End Get
        Protected Set(value As Double?)
            Me._FirmwareVersion = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Gets or sets the Firmware Version query command. </summary>
    ''' <value> The Firmware Version query command. </value>
    Protected Overridable ReadOnly Property FirmwareVersionQueryCommand As String

    ''' <summary> Queries the Firmware version. </summary>
    ''' <returns> System.Nullable{System.Double}. </returns>
    Public Function QueryFirmwareVersion() As Double?
        If Not String.IsNullOrWhiteSpace(Me.FirmwareVersionQueryCommand) Then
            Me._FirmwareVersion = Me.Session.Query(0.0F, Me.FirmwareVersionQueryCommand)
        End If
        Return Me.FirmwareVersion
    End Function

#End Region

End Class

Public Enum FanLevel
    <Description("Not Specified")> NotSpecified
    <Description("Normal")> Normal
    <Description("Quiet")> Quiet
End Enum