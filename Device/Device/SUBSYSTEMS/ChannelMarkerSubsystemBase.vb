Imports isr.Core.Pith
''' <summary> Defines the Channel Marker subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public MustInherit Class ChannelMarkerSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class.
    ''' </summary>
    ''' <param name="markerNumber">    The Marker number. </param>
    ''' <param name="channelNumber">   The channel number. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal markerNumber As Integer, ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.MarkerNumber = markerNumber
        Me.ChannelNumber = channelNumber
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
    End Sub

#End Region

#Region " CHANNEL "

    ''' <summary> Gets or sets the channel number. </summary>
    ''' <value> The channel number. </value>
    Public ReadOnly Property ChannelNumber As Integer

#End Region

#Region " MARKER "

    ''' <summary> Gets or sets the Marker number. </summary>
    ''' <value> The Marker number. </value>
    Public ReadOnly Property MarkerNumber As Integer

#End Region

#Region " LATEST DATA "

    ''' <summary> Gets or sets the latest data query command. </summary>
    ''' <value> The latest data query command. </value>
    ''' <remarks> SCPI: ":SENSE:DATA:LAT?" </remarks>
    Protected Overridable ReadOnly Property LatestDataQueryCommand As String

    ''' <summary> Fetches the latest data and parses it. </summary>
    ''' <remarks> Issues the ':SENSE:DATA:LAT?' query, which reads data stored in the Sample Buffer.. </remarks>
    Public Function FetchLatestData() As Double?
        Return Me.Measure(Me.LatestDataQueryCommand)
    End Function

#End Region

#Region " ABSCISSA "

    Private _Abscissa As Double?
    ''' <summary> Gets or sets the cached Trigger Abscissa. </summary>
    ''' <remarks> The Abscissa is used to Abscissa operation in the trigger layer. After the programmed
    ''' trigger event occurs, the instrument waits until the Abscissa period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Trigger Abscissa or none if not set or unknown. </value>
    Public Overloads Property Abscissa As Double?
        Get
            Return Me._Abscissa
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Abscissa, value) Then
                Me._Abscissa = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Abscissa. </summary>
    ''' <param name="value"> The current Abscissa. </param>
    ''' <returns> The Trigger Abscissa or none if unknown. </returns>
    Public Function ApplyAbscissa(ByVal value As Double) As Double?
        Me.WriteAbscissa(value)
        Return Me.QueryAbscissa()
    End Function

    ''' <summary> Gets the Abscissa query command. </summary>
    ''' <value> The Abscissa query command. </value>
    ''' <remarks> SCPI: ":CALC&lt;ch#&gt;:MARK&lt;m#&gt;:X?" </remarks>
    Protected Overridable ReadOnly Property AbscissaQueryCommand As String

    ''' <summary> Queries the Abscissa. </summary>
    ''' <returns> The Abscissa or none if unknown. </returns>
    Public Function QueryAbscissa() As Double?
        Me.Abscissa = Me.Query(Me.Abscissa, Me.AbscissaQueryCommand)
        Return Me.Abscissa
    End Function

    ''' <summary> Gets the Abscissa command format. </summary>
    ''' <value> The Abscissa command format. </value>
    ''' <remarks> SCPI: ":CALC&lt;ch#&gt;:MARK&lt;m#&gt;:X {0}" </remarks>
    Protected Overridable ReadOnly Property AbscissaCommandFormat As String

    ''' <summary> Writes the Trigger Abscissa without reading back the value from the device. </summary>
    ''' <param name="value"> The current Abscissa. </param>
    ''' <returns> The Trigger Abscissa or none if unknown. </returns>
    Public Function WriteAbscissa(ByVal value As Double) As Double?
        Me.Abscissa = Me.Write(value, Me.AbscissaCommandFormat)
        Return Me.Abscissa
    End Function

#End Region

#Region " ENABLED "

    Private _Enabled As Boolean?
    ''' <summary> Gets or sets the cached Enabled sentinel. </summary>
    ''' <value> <c>null</c> if  Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Enabled As Boolean?
        Get
            Return Me._Enabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Enabled, value) Then
                Me._Enabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the  Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteEnabled(value)
        Return Me.QueryEnabled()
    End Function

    ''' <summary> Gets the marker enabled query command. </summary>
    ''' <value> The marker enabled query command. </value>
    ''' <remarks> SCPI: ":CALC&lt;ch#&gt;:MARK&lt;m#&gt;:STAT?" </remarks>
    Protected Overridable ReadOnly Property EnabledQueryCommand As String

    ''' <summary> Queries the  Enabled sentinel. Also sets the
    ''' <see cref="Enabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryEnabled() As Boolean?
        Me.Enabled = Me.Query(Me.Enabled, Me.EnabledQueryCommand)
        Return Me.Enabled
    End Function

    ''' <summary> Gets the marker enabled command Format. </summary>
    ''' <value> The marker enabled query command. </value>
    ''' <remarks> SCPI: "":CALC&lt;ch#&gt;:MARK&lt;m#&gt;:STAT {0:1;1;0}" </remarks>
    Protected Overridable ReadOnly Property EnabledCommandFormat As String

    ''' <summary> Writes the  Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteEnabled(ByVal value As Boolean) As Boolean?
        Me.Enabled = Me.Write(value, Me.EnabledCommandFormat)
        Return Me.Enabled
    End Function

#End Region

End Class

#Region " UNUSED "
#If False Then
    ''' <summary> <c>True</c> if Measurement available. </summary>
    Private _MeasurementAvailable As Boolean

    ''' <summary> Gets or sets a value indicating whether [Measurement available]. </summary>
    ''' <value> <c>True</c> if [Measurement available]; otherwise, <c>False</c>. </value>
    Public Property MeasurementAvailable As Boolean
        Get
            Return Me._MeasurementAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            Me._MeasurementAvailable = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property

#End If
#End Region