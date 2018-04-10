Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ComboBoxExtensions
''' <summary> Defines the Channel Trace subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public MustInherit Class ChannelTraceSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class.
    ''' </summary>
    ''' <param name="TraceNumber">    The Trace number. </param>
    ''' <param name="channelNumber">   The channel number. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal traceNumber As Integer, ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.TraceNumber = traceNumber
        Me.ChannelNumber = channelNumber
    End Sub

#End Region

#Region " CHANNEL "

    ''' <summary> Gets or sets the channel number. </summary>
    ''' <value> The channel number. </value>
    Public ReadOnly Property ChannelNumber As Integer

#End Region

#Region " TRACE "

    ''' <summary> Gets or sets the Trace number. </summary>
    ''' <value> The Trace number. </value>
    Public ReadOnly Property TraceNumber As Integer

#End Region

#Region " AUTO SCALE "

    ''' <summary> Gets the auto scale command. </summary>
    ''' <value> The auto scale command. </value>
    ''' <remarks> SCPI: ":DISP:WIND&lt;c#&gt;:TRAC&lt;t#&gt;:Y:AUTO". </remarks>
    Protected Overridable ReadOnly Property AutoScaleCommand As String

    ''' <summary> Automatic scale. </summary>
    Public Sub AutoScale()
        If Not String.IsNullOrWhiteSpace(Me.SelectCommand) Then
            Me.Session.WriteLine(Me.SelectCommand)
        End If
    End Sub

#End Region

#Region " SELECT "

    ''' <summary> Gets the Select command. </summary>
    ''' <value> The Select command. </value>
    ''' <remarks> SCPI: ":CALC&lt;c#&gt;:PAR&lt;t#&gt;:SEL". </remarks>
    Protected Overridable ReadOnly Property SelectCommand As String

    ''' <summary> Selects this as active trace. </summary>
    Public Sub [Select]()
        If Not String.IsNullOrWhiteSpace(Me.SelectCommand) Then
            Me.Session.WriteLine(Me.SelectCommand)
        End If
    End Sub

#End Region

End Class

