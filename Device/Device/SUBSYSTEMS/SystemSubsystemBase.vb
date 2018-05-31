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

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " INTERFACE "

    ''' <summary> Supports clear interface. </summary>
    ''' <returns> <c>True</c> if supports clearing the interface. </returns>
    <Obsolete("Use Status Subsystem Supports Clear Interface")>
    Public Function SupportsClearInterface() As Boolean
        Return Me.Session.SupportsClearInterface
    End Function

    ''' <summary> Clears the interface. </summary>
    <Obsolete("Use Status Subsystem Clear Interface")>
    Public Sub ClearInterface()
        If Me.SupportsClearInterface Then Me.Session.ClearInterface()
    End Sub

    ''' <summary> Clears the device (SDC). </summary>
    <Obsolete("Use Status Subsystem Clear Device or Status Subsystem Clear Active State")>
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

    ''' <summary> The Fan Level. </summary>
    Private _FanLevel As FanLevel?

    ''' <summary> Gets or sets the cached Fan Level. </summary>
    ''' <value> The Fan Level or null if unknown. </value>
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

    ''' <summary> Converts the specified value to string. </summary>
    ''' <param name="value"> The <see cref="FanLevel">Fan Level</see>. </param>
    ''' <returns> A String. </returns>
    Protected Overridable Function FromFanLevel(ByVal value As FanLevel) As String
        Return If(value = VI.FanLevel.Normal, "NORM", "QUIET")
    End Function

    ''' <summary> Converts a value to a fan level. </summary>
    ''' <param name="value"> The <see cref="FanLevel">Route Fan Level</see>. </param>
    ''' <returns> Value as a FanLevel. </returns>
    Private Function ToFanLevel(ByVal value As String) As FanLevel
        If value.StartsWith(Me.FromFanLevel(VI.FanLevel.Quiet), StringComparison.OrdinalIgnoreCase) Then
            Return VI.FanLevel.Quiet
        Else
            Return VI.FanLevel.Normal
        End If
    End Function

    ''' <summary> Writes and reads back the Fan Level. </summary>
    ''' <param name="value"> The <see cref="FanLevel">Fan Level</see>. </param>
    ''' <returns> The Fan Level or null if unknown. </returns>
    Public Function ApplyFanLevel(ByVal value As FanLevel) As FanLevel?
        Me.WriteFanLevel(value)
        Return Me.QueryFanLevel()
    End Function

    ''' <summary> Gets the Fan Level query command. </summary>
    ''' <value> The Fan Level command. </value>
    Protected Overridable ReadOnly Property FanLevelQueryCommand As String

    ''' <summary> Queries the Fan Level. Also sets the <see cref="FanLevel">output
    ''' on</see> sentinel. </summary>
    ''' <returns> The Fan Level or null if unknown. </returns>
    Public Function QueryFanLevel() As FanLevel?
        Return Me.ToFanLevel(Me.Query(Me.FromFanLevel(Me.FanLevel.GetValueOrDefault(VI.FanLevel.Normal)), Me.FanLevelQueryCommand))
    End Function

    ''' <summary> Gets the Fan Level command format. </summary>
    ''' <value> The Fan Level command format. </value>
    Protected Overridable ReadOnly Property FanLevelCommandFormat As String

    ''' <summary> Writes the Fan Level. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Fan Level. </param>
    ''' <returns> The Fan Level or null if unknown. </returns>
    Public Function WriteFanLevel(ByVal value As FanLevel) As FanLevel?
        Me.Write(Me.FanLevelCommandFormat, Me.FromFanLevel(value))
        Me.FanLevel = value
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
    Protected Overridable ReadOnly Property LanguageRevisionQueryCommand As String

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

End Class

Public Enum FanLevel
    <Description("Normal")> Normal
    <Description("Quiet")> Quiet
End Enum