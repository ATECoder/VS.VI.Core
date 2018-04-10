''' <summary> Defines a System Subsystem for a Keithley 2000 instrument. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class SystemSubsystem
    Inherits VI.SystemSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.AutoZeroEnabled = True
        Me.BeeperEnabled = True
        Me.FourWireSenseEnabled = False
        Me.QueryFrontSwitched()
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

    ''' <summary> Gets the initialize memory command. </summary>
    ''' <value> The initialize memory command. </value>
    Protected Overrides ReadOnly Property InitializeMemoryCommand As String = VI.Pith.Scpi.Syntax.InitializeMemoryCommand

    ''' <summary> Gets the preset command. </summary>
    ''' <value> The preset command. </value>
    Protected Overrides ReadOnly Property PresetCommand As String = VI.Pith.Scpi.Syntax.SystemPresetCommand

    ''' <summary> Gets the language revision query command. </summary>
    ''' <value> The language revision query command. </value>
    Protected Overrides ReadOnly Property LanguageRevisionQueryCommand As String = VI.Pith.Scpi.Syntax.LanguageRevisionQueryCommand

#End Region

#Region " AUTO ZERO ENABLED "

    Private _AutoZeroEnabled As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether auto zero is enabled. </summary>
    ''' <value> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </value>
    Public Property AutoZeroEnabled() As Boolean?
        Get
            Return Me._AutoZeroEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoZeroEnabled, value) Then
                Me._AutoZeroEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the auto zero enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyAutoZeroEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoZeroEnabled(value)
        Return Me.QueryAutoZeroEnabled()
    End Function

    ''' <summary> Gets or sets the automatic zero enabled query command. </summary>
    ''' <value> The automatic zero enabled query command. </value>
    Protected ReadOnly Property AutoZeroEnabledQueryCommand As String = ":SYST:AZER:STAT?"

    ''' <summary> Queries the auto zero enabled state. </summary>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function QueryAutoZeroEnabled() As Boolean?
        Me.AutoZeroEnabled = Me.Query(Me.AutoZeroEnabled, Me.AutoZeroEnabledQueryCommand)
        Return Me.AutoZeroEnabled
    End Function

    ''' <summary> Gets or sets the automatic zero enabled command format. </summary>
    ''' <value> The automatic zero enabled command format. </value>
    Protected ReadOnly Property AutoZeroEnabledCommandFormat As String = ":SYST:AZER:STAT {0:'ON';'ON';'OFF'}"

    ''' <summary> Writes the auto zero enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function WriteAutoZeroEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoZeroEnabled = Me.Write(value, Me.AutoZeroEnabledCommandFormat)
        Return Me.AutoZeroEnabled
    End Function

#End Region

#Region " BEEPER ENABLED "

    Private _BeeperEnabled As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether Beeper is enabled. </summary>
    ''' <value> <c>True</c> if Beeper is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </value>
    Public Property BeeperEnabled() As Boolean?
        Get
            Return Me._BeeperEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.BeeperEnabled, value) Then
                Me._BeeperEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Beeper enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if Beeper is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyBeeperEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteBeeperEnabled(value)
        Return Me.QueryBeeperEnabled()
    End Function

    ''' <summary> Queries the Beeper enabled state. </summary>
    ''' <returns> <c>True</c> if Beeper is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function QueryBeeperEnabled() As Boolean?
        Me.BeeperEnabled = Me.Session.Query(Me.BeeperEnabled.GetValueOrDefault(True), ":SYST:BEEP:STAT?")
        Return Me.BeeperEnabled
    End Function

    ''' <summary> Writes the Beeper enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if Beeper is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function WriteBeeperEnabled(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine(":SYST:BEEP:STAT {0:'1';'1';'0'}", CType(value, Integer))
        Me.BeeperEnabled = value
        Return Me.BeeperEnabled
    End Function

    ''' <summary> Commands the instrument to issue a Beep on the instrument. </summary>
    ''' <param name="frequency"> Specifies the frequency of the beep. </param>
    ''' <param name="duration">  Specifies the duration of the beep. </param>
    Public Sub BeepImmediately(ByVal frequency As Integer, ByVal duration As Single)
        Me.Session.WriteLine(":SYST:BEEP:IMM {0}, {1}", frequency, duration)
    End Sub

#End Region

#Region " FOUR WIRE SENSE ENABLED "

    Private _FourWireSenseEnabled As Boolean?

    ''' <summary> Gets or sets a value indicating whether four wire sense is enabled. </summary>
    ''' <value> <c>True</c> if four wire sense mode is enabled; otherwise, <c>False</c>. </value>
    Public Property FourWireSenseEnabled() As Boolean?
        Get
            Return Me._FourWireSenseEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.FourWireSenseEnabled, value) Then
                Me._FourWireSenseEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Four Wire Sense enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyFourWireSenseEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteFourWireSenseEnabled(value)
        Return Me.QueryWireSenseEnabled()
    End Function

    ''' <summary> Queries the Four Wire Sense enabled state. </summary>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function QueryWireSenseEnabled() As Boolean?
        Me.FourWireSenseEnabled = Me.Session.Query(Me.FourWireSenseEnabled.GetValueOrDefault(True), ":SYST:RSEN?")
        Return Me.FourWireSenseEnabled
    End Function

    ''' <summary> Writes the Four Wire Sense enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function WriteFourWireSenseEnabled(ByVal value As Boolean) As Boolean?
        Me.FourWireSenseEnabled = New Boolean?
        Me.Session.WriteLine(":SYST:RSEN {0:'ON';'ON';'OFF'}", CType(value, Integer))
        Me.FourWireSenseEnabled = value
        Return Me.FourWireSenseEnabled
    End Function

#End Region

#Region " Front Switched "

    Private _FrontSwitched As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether Front is Switched. </summary>
    ''' <value> <c>True</c> if Front is Switched; <c>False</c> if not or none if not set or
    ''' unknown. </value>
    Public Property FrontSwitched() As Boolean?
        Get
            Return Me._FrontSwitched
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.FrontSwitched, value) Then
                Me._FrontSwitched = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries the Front Switched state. </summary>
    ''' <returns> <c>True</c> if Front is Switched; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function QueryFrontSwitched() As Boolean?
        Me.FrontSwitched = Me.Session.Query(Me.FrontSwitched.GetValueOrDefault(True), ":SYST:FRSW?")
        Return Me.FrontSwitched
    End Function

#End Region

End Class
