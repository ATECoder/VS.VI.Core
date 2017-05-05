Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Channel Marker Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public MustInherit Class ChannelMarkerSubsystemBase
    Inherits VI.ChannelMarkerSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class.
    ''' </summary>
    ''' <param name="markerNumber">    The marker number. </param>
    ''' <param name="channelNumber">   A reference to a <see cref="VI.StatusSubsystemBase">status
    '''                                subsystem</see>. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal markerNumber As Integer, ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(markerNumber, channelNumber, statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.MeasurementAvailable = False
    End Sub

#End Region

#Region " LATEST DATA "

    ''' <summary> Gets or sets the latest data query command. </summary>
    ''' <value> The latest data query command. </value>
    ''' <remarks> SCPI: ":SENSE:DATA:LAT?" </remarks>
    Protected Overridable ReadOnly Property LatestDataQueryCommand As String

    ''' <summary> Fetches the latest data and parses it. </summary>
    ''' <remarks> Issues the ':SENSE:DATA:LAT?' query, which reads data stored in the Sample Buffer.. </remarks>
    Public Overrides Sub FetchLatestData()
        If Not String.IsNullOrWhiteSpace(Me.LatestDataQueryCommand) Then
            Me.LastReading = Me.Session.QueryTrimEnd(Me.LatestDataQueryCommand)
        End If
        If Not String.IsNullOrWhiteSpace(Me.LastReading) Then
            ' the emulator will set the last reading. 
            Me.ParseReading(Me.LastReading)
            Me.MeasurementAvailable = True
        End If
    End Sub

#End Region

End Class
