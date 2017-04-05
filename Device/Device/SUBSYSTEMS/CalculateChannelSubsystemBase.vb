Imports isr.Core.Pith
''' <summary> Defines the Calculate Channel SCPI subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public MustInherit Class CalculateChannelSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.ChannelNumber = channelNumber
    End Sub

#End Region

#Region " CHANNEL "

    ''' <summary> Gets or sets the channel number. </summary>
    ''' <value> The channel number. </value>
    Public ReadOnly Property ChannelNumber As Integer

#End Region

#Region " TRACE COUNT "

    Private _TraceCount As Integer?
    ''' <summary> Gets or sets the cached Trace Count. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Trace model. </remarks>
    ''' <value> The Trace Count or none if not set or unknown. </value>
    Public Overloads Property TraceCount As Integer?
        Get
            Return Me._TraceCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.TraceCount, value) Then
                Me._TraceCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trace Count. </summary>
    ''' <param name="value"> The current TraceCount. </param>
    ''' <returns> The TraceCount or none if unknown. </returns>
    Public Function ApplyTraceCount(ByVal value As Integer) As Integer?
        Me.WriteTraceCount(value)
        Return Me.QueryTraceCount()
    End Function

    ''' <summary> Gets Trace Count query command. </summary>
    ''' <value> The Trace Count query command. </value>
    ''' <remarks> SCPI: ":CALC&lt;c#&gt;:PAR:COUN?" </remarks>
    Protected Overridable ReadOnly Property TraceCountQueryCommand As String

    ''' <summary> Queries the current Trace Count. </summary>
    ''' <returns> The Trace Count or none if unknown. </returns>
    Public Function QueryTraceCount() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.TraceCountQueryCommand) Then
            Me.TraceCount = Me.Session.Query(0I, Me.TraceCountQueryCommand)
        End If
        Return Me.TraceCount
    End Function

    ''' <summary> Gets Trace Count command format. </summary>
    ''' <value> The Trace Count command format. </value>
    ''' <remarks> SCPI: ":CALC&lt;c#&gt;:PAR:COUN {0}" </remarks>
    Protected Overridable ReadOnly Property TraceCountCommandFormat As String

    ''' <summary> Write the Trace PointsTraceCount without reading back the value from the device. </summary>
    ''' <param name="value"> The current PointsTraceCount. </param>
    ''' <returns> The PointsTraceCount or none if unknown. </returns>
    Public Function WriteTraceCount(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.TraceCountCommandFormat) Then
            Me.Session.WriteLine(Me.TraceCountCommandFormat, value)
        End If
        Me.TraceCount = value
        Return Me.TraceCount
    End Function

#End Region

#Region " AVERAGE "

    ''' <summary> Applies the average settings. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    ''' <param name="enabled"> true to enable, false to disable. </param>
    ''' <param name="count">   Number of. </param>
    Public Sub ApplyAverageSettings(ByVal enabled As Boolean, ByVal count As Integer)
        If Not Nullable.Equals(Me.AveragingEnabled, enabled) Then
            Me.ApplyAveragingEnabled(enabled)
        End If
        If Not Nullable.Equals(Me.AverageCount, count) Then
            Me.ApplyAverageCount(count)
        End If
    End Sub

#Region " AVERAGE CLEAR "

    ''' <summary> Gets the clear command. </summary>
    ''' <remarks> SCPI: ":CALC&lt;c#&gt;:AVER:CLE". </remarks>
    ''' <value> The clear command. </value>
    Protected Overridable ReadOnly Property AverageClearCommand As String

    ''' <summary> Clears the average. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    Public Sub ClearAverage()
        Me.Write(Me.AverageClearCommand)
    End Sub


#End Region

#Region " AVERAGE COUNT "

    Private _AverageCount As Integer?
    ''' <summary> Gets or sets the cached Average Count. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Average model. </remarks>
    ''' <value> The Average Count or none if not set or unknown. </value>
    Public Overloads Property AverageCount As Integer?
        Get
            Return Me._AverageCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.AverageCount, value) Then
                Me._AverageCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Average Count. </summary>
    ''' <param name="value"> The current AverageCount. </param>
    ''' <returns> The AverageCount or none if unknown. </returns>
    Public Function ApplyAverageCount(ByVal value As Integer) As Integer?
        Me.WriteAverageCount(value)
        Return Me.QueryAverageCount()
    End Function

    ''' <summary> Gets Average Count query command. </summary>
    ''' <value> The Average Count query command. </value>
    ''' <remarks> SCPI: ":CALC&lt;c#&gt;:AVER:COUN?" </remarks>
    Protected Overridable ReadOnly Property AverageCountQueryCommand As String

    ''' <summary> Queries the current Average Count. </summary>
    ''' <returns> The Average Count or none if unknown. </returns>
    Public Function QueryAverageCount() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.AverageCountQueryCommand) Then
            Me.AverageCount = Me.Session.Query(0I, Me.AverageCountQueryCommand)
        End If
        Return Me.AverageCount
    End Function

    ''' <summary> Gets Average Count command format. </summary>
    ''' <value> The Average Count command format. </value>
    ''' <remarks> SCPI: ":CALC&lt;c#&gt;:AVER:COUN {0}" </remarks>
    Protected Overridable ReadOnly Property AverageCountCommandFormat As String

    ''' <summary> Write the Average PointsAverageCount without reading back the value from the device. </summary>
    ''' <param name="value"> The current PointsAverageCount. </param>
    ''' <returns> The PointsAverageCount or none if unknown. </returns>
    Public Function WriteAverageCount(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.AverageCountCommandFormat) Then
            Me.Session.WriteLine(Me.AverageCountCommandFormat, value)
        End If
        Me.AverageCount = value
        Return Me.AverageCount
    End Function

#End Region

#Region " AVERAGING ENABLED "

    Private _AveragingEnabled As Boolean?
    ''' <summary> Gets or sets the cached Averaging Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Averaging Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AveragingEnabled As Boolean?
        Get
            Return Me._AveragingEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AveragingEnabled, value) Then
                Me._AveragingEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Averaging Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAveragingEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAveragingEnabled(value)
        Return Me.QueryAveragingEnabled()
    End Function

    ''' <summary> Gets the Averaging  enabled query command. </summary>
    ''' <value> The Averaging  enabled query command. </value>
    ''' <remarks> SCPI: ":CALC&lt;c#&gt;:AVER?" </remarks>
    Protected Overridable ReadOnly Property AveragingEnabledQueryCommand As String

    ''' <summary> Queries the Averaging Enabled sentinel. Also sets the
    ''' <see cref="AveragingEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAveragingEnabled() As Boolean?
        Me.AveragingEnabled = Me.Query(Me.AveragingEnabled, Me.AveragingEnabledQueryCommand)
        Return Me.AveragingEnabled
    End Function

    ''' <summary> Gets the Averaging enabled command Format. </summary>
    ''' <value> The Averaging enabled query command. </value>
    ''' <remarks> SCPI: ":CALC&lt;c#&gt;:AVER {0:1;1;0}" </remarks>
    Protected Overridable ReadOnly Property AveragingEnabledCommandFormat As String

    ''' <summary> Writes the Averaging Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAveragingEnabled(ByVal value As Boolean) As Boolean?
        Me.AveragingEnabled = Me.Write(value, Me.AveragingEnabledCommandFormat)
        Return Me.AveragingEnabled
    End Function

#End Region
#End Region

End Class

