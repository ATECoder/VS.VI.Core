''' <summary> Defines a SCPI Channel marker Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public Class ChannelMarkerSubsystem
    Inherits VI.ChannelMarkerSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="SourceChannelSubsystem" /> class.
    ''' </summary>
    ''' <param name="markerNumber">    The marker number. </param>
    ''' <param name="channelNumber">   A reference to a <see cref="StatusSubsystemBase">message
    '''                                based session</see>. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Public Sub New(ByVal markerNumber As Integer, ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(markerNumber, channelNumber, statusSubsystem)
        Me.MarkerReadings = New MarkerReadings
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

    ''' <summary>
    ''' Sets subsystem values to their known execution clear state.
    ''' </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.MarkerReadings.Reset()
    End Sub

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.MarkerReadings.Initialize(ReadingTypes.Primary Or ReadingTypes.Secondary)
        Me.SafePostPropertyChanged(NameOf(E4990.ChannelMarkerSubsystem.MarkerReadings))
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " LATEST DATA "

    ''' <summary> Gets the latest data query command. </summary>
    ''' <value> The latest data query command. </value>
    ''' <remarks> Not exactly the same as reading the latest data.</remarks>
    Protected Overrides ReadOnly Property LatestDataQueryCommand As String
        Get
            Return $":CALC{Me.ChannelNumber}:MARK{Me.MarkerNumber}:Y?"
        End Get
    End Property

#End Region

#Region " ABSCISSA "

    ''' <summary> Gets the Abscissa command format. </summary>
    ''' <value> The Abscissa command format. </value>
    Protected Overrides ReadOnly Property AbscissaCommandFormat As String
        Get
            Return $":CALC{Me.ChannelNumber}:MARK{Me.MarkerNumber}:X {{0}}"
        End Get
    End Property

    ''' <summary> Gets the Abscissa query command. </summary>
    ''' <value> The Abscissa query command. </value>
    Protected Overrides ReadOnly Property AbscissaQueryCommand As String
        Get
            Return $":CALC{Me.ChannelNumber}:MARK{Me.MarkerNumber}:X?"
        End Get
    End Property

#End Region

#Region " ENABLED "

    ''' <summary> Gets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    Protected Overrides ReadOnly Property EnabledQueryCommand As String
        Get
            Return $":CALC{Me.ChannelNumber}:MARK{Me.MarkerNumber}:STAT?"
        End Get
    End Property

    ''' <summary> Gets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    Protected Overrides ReadOnly Property EnabledCommandFormat As String
        Get
            Return $":CALC{Me.ChannelNumber}:MARK{Me.MarkerNumber}:STAT {{0:1;1;0}}"
        End Get
    End Property

#End Region

#End Region

#Region " LATEST DATA "

    Private _MarkerReadings As MarkerReadings
    ''' <summary> Returns the readings. </summary>
    ''' <returns> The readings. </returns>
    Public Property MarkerReadings() As MarkerReadings
        Get
            Return Me._MarkerReadings
        End Get
        Set(value As MarkerReadings)
            Me._markerReadings = value
            MyBase.AssignReadingAmounts(value)
        End Set
    End Property


    ''' <summary> Parses a new set of reading elements. </summary>
    ''' <param name="reading"> Specifies the measurement text to parse into the new reading. </param>
    Public Overrides Function ParseReading(ByVal reading As String) As Double?
        Return MyBase.ParseReadingAmounts(reading)
    End Function

    ''' <summary> Initializes the marker average. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="device"> The device. </param>
    Public Sub InitializeMarkerAverage(ByVal device As Device)
        If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
        Me.MarkerReadings.Reset()
        device.TriggerSubsystem.ApplyTriggerSource(Scpi.TriggerSources.Bus)
        device.CalculateChannelSubsystem.ClearAverage()
        device.TriggerSubsystem.ApplyAveragingEnabled(True)
        device.TriggerSubsystem.Immediate()
    End Sub

    ''' <summary> Reads marker average. </summary>
    ''' <param name="device"> The device. </param>
    Public Sub ReadMarkerAverage(ByVal device As Device)
        If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
        Me.InitializeMarkerAverage(device)
        Me.StatusSubsystem.QueryOperationCompleted.GetValueOrDefault(False)
        Me.FetchLatestData()
    End Sub

#End Region

End Class
