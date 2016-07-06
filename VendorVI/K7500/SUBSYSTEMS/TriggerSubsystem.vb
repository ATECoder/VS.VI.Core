''' <summary> Defines a Trigger Subsystem for a Keithley 7500 Meter. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class TriggerSubsystem
    Inherits VI.Scpi.TriggerSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="TriggerSubsystem" /> class. </summary>
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

#Region " ABORT / INIT COMMANDS "

    ''' <summary> Gets the Abort command. </summary>
    ''' <value> The Abort command. </value>
    Protected Overrides ReadOnly Property AbortCommand As String = ":ABOR"

    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    Protected Overrides ReadOnly Property InitiateCommand As String = ":INIT"

#End Region

#End Region

#Region " GRAD Binning "

    ''' <summary> Loads grade binning. </summary>
    ''' <remarks> David, 6/27/2016. </remarks>
    ''' <param name="count">            Number of. </param>
    ''' <param name="startDelay">       The start delay. </param>
    ''' <param name="endDelay">         The end delay. </param>
    ''' <param name="highLimit">        The high limit. </param>
    ''' <param name="lowLimit">         The low limit. </param>
    ''' <param name="failedBitPattern"> A pattern specifying the failed bit. </param>
    ''' <param name="passBitPattern">   A pattern specifying the pass bit. </param>
    Public Sub LoadGradeBinning(ByVal count As Integer, ByVal startDelay As TimeSpan, ByVal endDelay As TimeSpan,
                                ByVal highLimit As Double, ByVal lowLimit As Double,
                                ByVal failedBitPattern As Integer, ByVal passBitPattern As Integer)
        Me.Write(":TRIG:LOAD 'GradeBinning',{0},7,{1},{2},{3},{4},{5},{6}",
                 count, startDelay.TotalSeconds, endDelay.TotalSeconds, highLimit, lowLimit, failedBitPattern, passBitPattern)
    End Sub

    ''' <summary> Loads grade binning. </summary>
    ''' <remarks> David, 6/27/2016. </remarks>
    ''' <param name="startDelay">       The start delay. </param>
    ''' <param name="endDelay">         The end delay. </param>
    ''' <param name="highLimit">        The high limit. </param>
    ''' <param name="lowLimit">         The low limit. </param>
    ''' <param name="failedBitPattern"> A pattern specifying the failed bit. </param>
    ''' <param name="passBitPattern">   A pattern specifying the pass bit. </param>
    Public Sub LoadGradeBinning(ByVal startDelay As TimeSpan, ByVal endDelay As TimeSpan,
                                ByVal highLimit As Double, ByVal lowLimit As Double,
                                ByVal failedBitPattern As Integer, ByVal passBitPattern As Integer)
        Me.Write(":TRIG:LOAD 'GradeBinning',{0},7,268000000, {1},{2},{3},{4},{5},{6}",
                 startDelay.TotalSeconds, endDelay.TotalSeconds, highLimit, lowLimit, failedBitPattern, passBitPattern)
    End Sub

#End Region




End Class
