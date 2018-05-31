''' <summary> System subsystem. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="12/14/2013" by="David" revision=""> Created. </history>
Public Class SystemSubsystem
    Inherits VI.Tsp2.SystemSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystem" /> class. </summary>
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

#Region " BEEPER IMMEDIATE "

    ''' <summary> Commands the instrument to issue a Beep on the instrument. </summary>
    ''' <param name="frequency"> Specifies the frequency of the beep. </param>
    ''' <param name="duration">  Specifies the duration of the beep. </param>
    Public Sub BeepImmediately(ByVal frequency As Integer, ByVal duration As Single)
        Me.Write("beeper.beep({0},{1})", frequency, duration)
    End Sub

#End Region

#Region " FAN LEVEL "

    ''' <summary> Converts the specified value to string. </summary>
    ''' <param name="value"> The <see cref="P:isr.VI.SystemSubsystemBase.FanLevel">Fan Level</see>. </param>
    ''' <returns> A String. </returns>
    Protected Overrides Function FromFanLevel(ByVal value As FanLevel) As String
        Return If(value = VI.FanLevel.Normal, "fan.LEVEL_NORMAL", "fan.LEVEL_QUIET")
    End Function

    ''' <summary> Gets the Fan Level query command. </summary>
    ''' <value> The Fan Level command. </value>
    Protected Overrides ReadOnly Property FanLevelQueryCommand As String = "_G.print(_G.fan.level)"

    ''' <summary> Gets the Fan Level command format. </summary>
    ''' <value> The Fan Level command format. </value>
    Protected Overrides ReadOnly Property FanLevelCommandFormat As String = "_G.fan.level={0}"

#End Region

End Class
