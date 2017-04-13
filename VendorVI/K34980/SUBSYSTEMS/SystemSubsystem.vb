''' <summary> Defines a System Subsystem for a Keysight 34980 Meter/Scanner. </summary>
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
    Inherits VI.Scpi.SystemSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.BeeperEnabled = True
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
                Me.SafePostPropertyChanged(NameOf(Me.BeeperEnabled))
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
        Me.Session.WriteLine(":SYST:BEEP:STAT {0:'ON';'ON';'OFF'}", CType(value, Integer))
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

End Class
