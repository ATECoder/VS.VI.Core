''' <summary> Defines a SCPI Calculate Channel Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public Class CalculateChannelSubsystem
    Inherits VI.CalculateChannelSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="SourceChannelSubsystem" /> class.
    ''' </summary>
    ''' <param name="channelNumber">   A reference to a <see cref="StatusSubsystemBase">message
    '''                                based session</see>. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Public Sub New(ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(channelNumber, statusSubsystem)
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

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.TraceCount = 2
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " AVERAGE "

    Protected Overrides ReadOnly Property AverageClearCommand As String
        Get
            Return $":CALC{Me.ChannelNumber}:AVER:CLE"
        End Get
    End Property

    Protected Overrides ReadOnly Property AverageCountCommandFormat As String
        Get
            Return $":CALC{Me.ChannelNumber}:AVER:COUN {{0}}"
        End Get
    End Property

    Protected Overrides ReadOnly Property AverageCountQueryCommand As String
        Get
            Return $":CALC{Me.ChannelNumber}:AVER:COUN?"
        End Get
    End Property

#Region " AVERAGING ENABLED "

    ''' <summary> Gets or sets the averaging enabled query command. </summary>
    ''' <value> The averaging enabled query command. </value>
    Protected Overrides ReadOnly Property AveragingEnabledQueryCommand As String
        Get
            Return $":CALC{Me.ChannelNumber}:AVER?"
        End Get
    End Property

    ''' <summary> Gets or sets the averaging enabled command Format. </summary>
    ''' <value> The averaging enabled query command. </value>
    Protected Overrides ReadOnly Property AveragingEnabledCommandFormat As String
        Get
            Return $":CALC{Me.ChannelNumber}:AVER {{0:1;1;0}}"
        End Get
    End Property


#End Region

#End Region

#Region " PARAMETER "

    ''' <summary> Gets Trace Count command format. </summary>
    ''' <value> The Trace Count command format. </value>
    Protected Overrides ReadOnly Property TraceCountCommandFormat As String
        Get
            Return $":CALC{Me.ChannelNumber}:PAR:COUN {{0}}"
        End Get
    End Property

    ''' <summary> Gets Trace Count query command. </summary>
    ''' <value> The Trace Count query command. </value>
    Protected Overrides ReadOnly Property TraceCountQueryCommand As String
        Get
            Return $":CALC{Me.ChannelNumber}:PAR:COUN?"
        End Get
    End Property

#End Region

#End Region

End Class
