''' <summary> Defines a SCPI Sense Channel Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public Class SenseChannelSubsystem
    Inherits VI.Scpi.SenseChannelSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceChannelSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(channelNumber, statusSubsystem)
        Me.SupportedAdapterTypes = VI.AdapterType.E4M1 Or VI.AdapterType.E4M2 Or VI.AdapterType.None
        Me.SupportedSweepTypes = VI.SweepType.Linear Or VI.SweepType.Logarithmic
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
        Me.ApertureRange = New isr.Core.Pith.RangeR(1, 5)
        Me.AdapterType = VI.AdapterType.None
        Me.SweepPoints = 201
        Me.SweepStart = 20
        Me.SweepStop = 10000000.0
        Me.SweepType = VI.SweepType.Linear
        Me.FrequencyPointsType = VI.FrequencyPointsType.Fixed
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " COMMANDS "

    ''' <summary> Gets the clear compensations command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:CLE". </remarks>
    ''' <value> The clear compensations command. </value>
    Protected Overrides ReadOnly Property ClearCompensationsCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:CLE"
        End Get
    End Property

#End Region


#Region " ADAPTER TYPE "

    ''' <summary> Gets or sets the Adapter Type query command. </summary>
    ''' <value> The Adapter Type query command, e.g., :SENSE:ADAPT? </value>
    Protected Overrides ReadOnly Property AdapterTypeQueryCommand As String
        Get
            Return ":SENS:ADAP?"
        End Get
    End Property

    ''' <summary> Gets or sets the Adapter Type command. </summary>
    ''' <value> The Adapter Type command, e.g., :SENSE:ADAPT {0}. </value>
    Protected Overrides ReadOnly Property AdapterTypeCommandFormat As String
        Get
            Return "SENS:ADAP {0}"
        End Get
    End Property

#End Region

#Region " Aperture "

    ''' <summary> Gets The Aperture command format. </summary>
    ''' <value> The Aperture command format. </value>
    Protected Overrides ReadOnly Property ApertureCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:APER {{0}}"
        End Get
    End Property


    ''' <summary> Gets The Aperture query command. </summary>
    ''' <value> The Aperture query command. </value>
    Protected Overrides ReadOnly Property ApertureQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:APER?"
        End Get
    End Property

#End Region

#Region " FREQUENCY POINTS TYPE "

    ''' <summary> Gets Frequency Points Type query command. </summary>
    ''' <value> The Frequency Points Type query command. </value>
    Protected Overrides ReadOnly Property FrequencyPointsTypeQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR:COLL:FPO?"
        End Get
    End Property

    ''' <summary> Gets Frequency Points Type command format. </summary>
    ''' <value> The Frequency Points Type command format. </value>
    Protected Overrides ReadOnly Property FrequencyPointsTypeCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR:COLL:FPO {{0}}"
        End Get
    End Property

#End Region

#Region " SWEEP "

    ''' <summary> Gets Sweep Points query command. </summary>
    ''' <value> The Sweep Points query command. </value>
    Protected Overrides ReadOnly Property SweepPointsQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:SWE:POIN?"
        End Get
    End Property

    ''' <summary> Gets Sweep Points command format. </summary>
    ''' <value> The Sweep Points command format. </value>
    Protected Overrides ReadOnly Property SweepPointsCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:SWE:POIN {{0}}"
        End Get
    End Property

    ''' <summary> Gets Sweep Start query command. </summary>
    ''' <value> The Sweep Start query command. </value>
    Protected Overrides ReadOnly Property SweepStartQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:FREQ:STAR?"
        End Get
    End Property

    ''' <summary> Gets Sweep Start command format. </summary>
    ''' <value> The Sweep Start command format. </value>
    Protected Overrides ReadOnly Property SweepStartCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:FREQ:STAR {{0}}"
        End Get
    End Property

    ''' <summary> Gets Sweep Stop query command. </summary>
    ''' <value> The Sweep Stop query command. </value>
    Protected Overrides ReadOnly Property SweepStopQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:FREQ:STOP?"
        End Get
    End Property

    ''' <summary> Gets Sweep Stop command format. </summary>
    ''' <value> The Sweep Stop command format. </value>
    Protected Overrides ReadOnly Property SweepStopCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:FREQ:STOP {{0}}"
        End Get
    End Property

    ''' <summary> Gets Sweep Type query command. </summary>
    ''' <value> The Sweep Type query command. </value>
    Protected Overrides ReadOnly Property SweepTypeQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:SWE:TYPE?"
        End Get
    End Property

    ''' <summary> Gets Sweep Type command format. </summary>
    ''' <value> The Sweep Type command format. </value>
    Protected Overrides ReadOnly Property SweepTypeCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:SWE:TYPE {{0}}"
        End Get
    End Property

#End Region

#End Region

End Class
