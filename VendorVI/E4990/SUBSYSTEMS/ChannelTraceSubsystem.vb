''' <summary> Defines a SCPI Channel Trace Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public Class ChannelTraceSubsystem
    Inherits VI.Scpi.ChannelTraceSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="SourceChannelSubsystem" /> class.
    ''' </summary>
    ''' <remarks> David, 7/6/2016. </remarks>
    ''' <param name="traceNumber">     The Trace number. </param>
    ''' <param name="channelNumber">   A reference to a <see cref="VI.StatusSubsystemBase">message
    '''                                based session</see>. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Public Sub New(ByVal traceNumber As Integer, ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(traceNumber, channelNumber, statusSubsystem)
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

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.SupportedParameters = CType(-1 + TraceParameters.AbsoluteAdmittance << 1, TraceParameters)
        Me.Parameter = TraceParameters.AbsoluteImpedance
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " AUTO SCALE "

    Protected Overrides ReadOnly Property AutoScaleCommand As String
        Get
            Return $":DISP:WIND{Me.ChannelNumber}:TRAC{Me.TraceNumber}:Y:AUTO"
        End Get
    End Property


#End Region

#Region " PARAMETER "

    ''' <summary> Gets the trace parameter command format. </summary>
    ''' <value> The trace parameter command format. </value>
    Protected Overrides ReadOnly Property ParameterCommandFormat As String
        Get
            Return $":CALC{Me.ChannelNumber}:PAR{Me.TraceNumber}:DEF {0}"
        End Get
    End Property

    ''' <summary> Gets the trace parameter query command. </summary>
    ''' <value> The trace parameter query command. </value>
    Protected Overrides ReadOnly Property ParameterQueryCommand As String
        Get
            Return $":CALC{Me.ChannelNumber}:PAR{Me.TraceNumber}:DEF?"
        End Get
    End Property

#End Region

#Region " SELECT "

    Protected Overrides ReadOnly Property SelectCommand As String
        Get
            Return $":CALC{Me.ChannelNumber}:PAR{Me.TraceNumber}:SEL"
        End Get
    End Property


#End Region

#End Region

End Class
