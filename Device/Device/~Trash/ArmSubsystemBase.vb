Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines a SCPI ARM Subsystem. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/5/2013" by="David" revision=""> Created based on SCPI 5.1 library. </history>
Public MustInherit Class ArmSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ArmSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._ArmLayers = New PresettablePropertyPublisherCollection
        Me._AddLayer()
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' Free managed resources when explicitly called
                ' Unable to use null conditional because it is not visible to code analysis
                If Me._ActiveLayer IsNot Nothing Then Me._ActiveLayer.Dispose() : Me._ActiveLayer = Nothing
            End If
            If Not Me.IsDisposed Then
                Me.ArmLayers?.Clear() : Me._ArmLayers = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.ArmLayers.ClearExecutionState()
    End Sub

    ''' <summary> Sets the subsystem to its preset state. </summary>
    Public Overrides Sub PresetKnownState()
        MyBase.PresetKnownState()
        Me.ArmLayers.PresetKnownState()
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.ArmLayers.ResetKnownState()
    End Sub

#End Region

#Region " LAYERS "

    ''' <summary> Gets or sets reference to the <see cref="Armlayer">arm layer</see>.
    ''' </summary>
    Public Property ActiveLayer() As ArmLayer

    Private Sub _AddLayer()
        Me._ActiveLayer = New ArmLayer()
        Me._ArmLayers.Add(New ArmLayer())
    End Sub

    ''' <summary>
    ''' Adds an <see cref="Armlayer">arm layer</see> to the collection of arm layers.
    ''' Makes the layer the <see cref="Activelayer">active layer.</see>
    ''' </summary>
    Public Sub AddLayer()
        Me._AddLayer()
    End Sub

    Private _ArmLayers As PresettablePropertyPublisherCollection
    ''' <summary>
    ''' Gets reference to the collection of calculation layers
    ''' </summary>
    Public ReadOnly Property ArmLayers() As PresettablePropertyPublisherCollection
        Get
            Return Me._ArmLayers
        End Get
    End Property

#End Region

#Region " ACTIVE LAYER "

#Region " ARM COUNT "

    ''' <summary> Gets or sets the cached Arm ArmCount. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Arm model. </remarks>
    ''' <value> The Arm ArmCount or none if not set or unknown. </value>
    Public Overloads Property ArmCount As Integer?
        Get
            Return Me.ActiveLayer.Count
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.ArmCount, value) Then
                Me.ActiveLayer.Count = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ArmCount))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Arm ArmCount. </summary>
    ''' <param name="value"> The current ArmCount. </param>
    ''' <returns> The ArmCount or none if unknown. </returns>
    Public Function ApplyArmCount(ByVal value As Integer) As Integer?
        Me.WriteArmCount(value)
        Return Me.QueryArmCount()
    End Function

    ''' <summary> Gets Arm ArmCount query command. </summary>
    ''' <value> The Arm ArmCount query command. </value>
    ''' <remarks> SCPI: ":ARM:COUN?" </remarks>
    Protected Overridable ReadOnly Property ArmCountQueryCommand As String

    ''' <summary> Queries the current PointsArmCount. </summary>
    ''' <returns> The PointsArmCount or none if unknown. </returns>
    Public Function QueryArmCount() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.ArmCountQueryCommand) Then
            Me.ArmCount = Me.Session.Query(0I, Me.ArmCountQueryCommand)
        End If
        Return Me.ArmCount
    End Function

    ''' <summary> Gets Arm ArmCount command format. </summary>
    ''' <value> The Arm ArmCount command format. </value>
    ''' <remarks> SCPI: ":ARM:COUN {0}" </remarks>
    Protected Overridable ReadOnly Property ArmCountCommandFormat As String

    ''' <summary> Write the Trace PointsArmCount without reading back the value from the device. </summary>
    ''' <param name="value"> The current PointsArmCount. </param>
    ''' <returns> The PointsArmCount or none if unknown. </returns>
    Public Function WriteArmCount(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.ArmCountCommandFormat) Then
            Me.Session.WriteLine(Me.ArmCountCommandFormat, value)
        End If
        Me.ArmCount = value
        Return Me.ArmCount
    End Function

#End Region

#Region " ARM DIRECTION "

    ''' <summary> Gets or sets the cached source Direction. </summary>
    ''' <value> The <see cref="Direction">ARM Direction</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property Direction As Direction?
        Get
            Return Me.ActiveLayer.Direction
        End Get
        Protected Set(ByVal value As Direction?)
            If Not Me.Direction.Equals(value) Then
                Me.ActiveLayer.Direction = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Direction))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the ARM Direction. </summary>
    ''' <param name="value"> The ARM Direction. </param>
    ''' <returns> The <see cref="Direction">source  ARM Direction</see> or none if unknown. </returns>
    Public Function ApplyDirection(ByVal value As Direction) As Direction?
        Me.WriteDirection(value)
        Return Me.QueryDirection()
    End Function

    ''' <summary> Gets the ARM Direction query command. </summary>
    ''' <value> The ARM Direction query command. </value>
    ''' <remarks> SCPI: ":ARM:DIR" </remarks>
    Protected Overridable ReadOnly Property DirectionQueryCommand As String

    ''' <summary> Queries the ARM Direction. </summary>
    ''' <returns> The <see cref="Direction"> ARM Direction</see> or none if unknown. </returns>
    Public Function QueryDirection() As Direction?
        Me.Direction = Me.Query(Of Direction)(Me.DirectionQueryCommand, Me.Direction)
        Return Me.Direction
    End Function

    ''' <summary> Gets the ARM Direction command format. </summary>
    ''' <value> The ARM Direction command format. </value>
    ''' <remarks> SCPI: ":ARM:DIR {0}" </remarks>
    Protected Overridable ReadOnly Property DirectionCommandFormat As String

    ''' <summary> Writes the ARM Direction without reading back the value from the device. </summary>
    ''' <param name="value"> The ARM Direction. </param>
    ''' <returns> The <see cref="Direction"> ARM Direction</see> or none if unknown. </returns>
    Public Function WriteDirection(ByVal value As Direction) As Direction?
        Me.Direction = Me.Write(Of Direction)(Me.DirectionCommandFormat, value)
        Return Me.Direction
    End Function

#End Region

#Region " ARM SOURCE "

    Private _SupportedArmSources As ArmSources
    ''' <summary>
    ''' Gets or sets the supported Function Mode.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedArmSources() As ArmSources
        Get
            Return _SupportedArmSources
        End Get
        Set(ByVal value As ArmSources)
            If Not Me.SupportedArmSources.Equals(value) Then
                Me._SupportedArmSources = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SupportedArmSources))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the cached source ArmSource. </summary>
    ''' <value> The <see cref="ArmSource">source Arm Source</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property ArmSource As ArmSources?
        Get
            Return Me.ActiveLayer.ArmSource
        End Get
        Protected Set(ByVal value As ArmSources?)
            If Not Me.ArmSource.Equals(value) Then
                Me.ActiveLayer.ArmSource = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ArmSource))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Arm Source. </summary>
    ''' <param name="value"> The  Source Arm Source. </param>
    ''' <returns> The <see cref="ArmSource">source Arm Source</see> or none if unknown. </returns>
    Public Function ApplyArmSource(ByVal value As ArmSources) As ArmSources?
        Me.WriteArmSource(value)
        Return Me.QueryArmSource()
    End Function

    ''' <summary> Gets the Arm source query command. </summary>
    ''' <value> The Arm source query command. </value>
    ''' <remarks> SCPI: ":ARM:SOUR?" </remarks>
    Protected Overridable ReadOnly Property ArmSourceQueryCommand As String

    ''' <summary> Queries the Arm Source. </summary>
    ''' <returns> The <see cref="ArmSource">Arm Source</see> or none if unknown. </returns>
    Public Function QueryArmSource() As ArmSources?
        Dim currentValue As String = Me.ArmSource.ToString
        If String.IsNullOrEmpty(Me.Session.EmulatedReply) Then Me.Session.EmulatedReply = currentValue
        If Not String.IsNullOrWhiteSpace(Me.ArmSourceQueryCommand) Then
            currentValue = Me.Session.QueryTrimEnd(Me.ArmSourceQueryCommand)
        End If
        If String.IsNullOrWhiteSpace(currentValue) Then
            Me.ArmSource = New ArmSources?
        Else
            Dim se As New StringEnumerator(Of ArmSources)
            Me.ArmSource = se.ParseContained(currentValue.BuildDelimitedValue)
        End If
        Return Me.ArmSource
    End Function

    ''' <summary> Gets the Arm source command format. </summary>
    ''' <value> The write Arm source command format. </value>
    ''' <remarks> SCPI: ":ARM:SOUR {0}". </remarks>
    Protected Overridable ReadOnly Property ArmSourceCommandFormat As String

    ''' <summary> Writes the Arm Source without reading back the value from the device. </summary>
    ''' <param name="value"> The Arm Source. </param>
    ''' <returns> The <see cref="ArmSource">Arm Source</see> or none if unknown. </returns>
    Public Function WriteArmSource(ByVal value As ArmSources) As ArmSources?
        If Not String.IsNullOrWhiteSpace(Me.ArmSourceCommandFormat) Then
            Me.Session.WriteLine(Me.ArmSourceCommandFormat, value.ExtractBetween())
        End If
        Me.ArmSource = value
        Return Me.ArmSource
    End Function

#End Region

#Region " AUTO DELAY ENABLED "

    ''' <summary> Gets or sets the cached Auto Delay Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Delay Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoDelayEnabled As Boolean?
        Get
            Return Me.ActiveLayer.AutoDelayEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoDelayEnabled, value) Then
                Me.ActiveLayer.AutoDelayEnabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.AutoDelayEnabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Auto Delay Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoDelayEnabled(value)
        Return Me.QueryAutoDelayEnabled()
    End Function

    ''' <summary> Gets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":ARM:DEL:AUTO?" </remarks>
    Protected Overridable ReadOnly Property AutoDelayEnabledQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="AutoDelayEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoDelayEnabled() As Boolean?
        Me.AutoDelayEnabled = Me.Query(Me.AutoDelayEnabled, Me.AutoDelayEnabledQueryCommand)
        Return Me.AutoDelayEnabled
    End Function

    ''' <summary> Gets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":ARM:DEL:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoDelayEnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoDelayEnabled = Me.Write(value, Me.AutoDelayEnabledCommandFormat)
        Return Me.AutoDelayEnabled
    End Function

#End Region

#Region " DELAY "

    ''' <summary> Gets or sets the cached Arm Delay. </summary>
    ''' <remarks> The delay is used to delay operation in the Arm layer. After the programmed
    ''' Arm event occurs, the instrument waits until the delay period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Arm Delay or none if not set or unknown. </value>
    Public Overloads Property Delay As TimeSpan?
        Get
            Return Me.ActiveLayer.Delay
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Nullable.Equals(Me.Delay, value) Then
                Me.ActiveLayer.Delay = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Delay))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Arm Delay. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Arm Delay or none if unknown. </returns>
    Public Function ApplyDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteDelay(value)
        Return Me.QueryDelay()
    End Function

    ''' <summary> Gets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    ''' <remarks> SCPI: ":ARM:DEL?" </remarks>
    Protected Overridable ReadOnly Property DelayQueryCommand As String

    ''' <summary> Gets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    ''' <remarks> For example: "s\.fff" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property DelayFormat As String

    ''' <summary> Queries the Delay. </summary>
    ''' <returns> The Delay or none if unknown. </returns>
    Public Function QueryDelay() As TimeSpan?
        Me.Delay = Me.Query(Me.Delay, Me.DelayFormat, Me.DelayQueryCommand)
        Return Me.Delay
    End Function

    ''' <summary> Gets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    ''' <remarks> SCPI: ":ARM:DEL {0:s\.fff}" </remarks>
    Protected Overridable ReadOnly Property DelayCommandFormat As String

    ''' <summary> Writes the Arm Delay without reading back the value from the device. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Arm Delay or none if unknown. </returns>
    Public Function WriteDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.Delay = Me.Write(value, Me.DelayCommandFormat)
        Return Me.Delay
    End Function

#End Region

#Region " INPUT LINE NUMBER "

    ''' <summary> Gets or sets the cached Arm Input Line Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Arm model. </remarks>
    ''' <value> The Arm InputLineNumber or none if not set or unknown. </value>
    Public Overloads Property InputLineNumber As Integer?
        Get
            Return Me.ActiveLayer.InputLineNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.InputLineNumber, value) Then
                Me.ActiveLayer.InputLineNumber = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.InputLineNumber))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Arm Input Line Number. </summary>
    ''' <param name="value"> The current Input Line Number. </param>
    ''' <returns> The Input Line Number or none if unknown. </returns>
    Public Function ApplyInputLineNumber(ByVal value As Integer) As Integer?
        Me.WriteInputLineNumber(value)
        Return Me.QueryInputLineNumber()
    End Function

    ''' <summary> Gets the Input Line Number query command. </summary>
    ''' <value> The Input Line Number query command. </value>
    ''' <remarks> SCPI: ":ARM:ILIN?" </remarks>
    Protected Overridable ReadOnly Property InputLineNumberQueryCommand As String

    ''' <summary> Queries the InputLineNumber. </summary>
    ''' <returns> The Input Line Number or none if unknown. </returns>
    Public Function QueryInputLineNumber() As Integer?
        Me.InputLineNumber = Me.Query(Me.InputLineNumber, Me.InputLineNumberQueryCommand)
        Return Me.InputLineNumber
    End Function

    ''' <summary> Gets the Input Line Number command format. </summary>
    ''' <value> The Input Line Number command format. </value>
    ''' <remarks> SCPI: ":ARM:ILIN {0}" </remarks>
    Protected Overridable ReadOnly Property InputLineNumberCommandFormat As String

    ''' <summary> Writes the Arm Input Line Number without reading back the value from the device. </summary>
    ''' <param name="value"> The current InputLineNumber. </param>
    ''' <returns> The Arm Input Line Number or none if unknown. </returns>
    Public Function WriteInputLineNumber(ByVal value As Integer) As Integer?
        Me.InputLineNumber = Me.Write(value, Me.InputLineNumberCommandFormat)
        Return Me.InputLineNumber
    End Function

#End Region

#Region " OUTPUT LINE NUMBER "

    ''' <summary> Gets or sets the cached Arm Output Line Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Arm model. </remarks>
    ''' <value> The Arm OutputLineNumber or none if not set or unknown. </value>
    Public Overloads Property OutputLineNumber As Integer?
        Get
            Return Me.ActiveLayer.OutputLineNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.OutputLineNumber, value) Then
                Me.ActiveLayer.OutputLineNumber = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.OutputLineNumber))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Arm Output Line Number. </summary>
    ''' <param name="value"> The current Output Line Number. </param>
    ''' <returns> The Output Line Number or none if unknown. </returns>
    Public Function ApplyOutputLineNumber(ByVal value As Integer) As Integer?
        Me.WriteOutputLineNumber(value)
        Return Me.QueryOutputLineNumber()
    End Function

    ''' <summary> Gets the Output Line Number query command. </summary>
    ''' <value> The Output Line Number query command. </value>
    ''' <remarks> SCPI: ":ARM:OLIN?" </remarks>
    Protected Overridable ReadOnly Property OutputLineNumberQueryCommand As String

    ''' <summary> Queries the OutputLineNumber. </summary>
    ''' <returns> The Output Line Number or none if unknown. </returns>
    Public Function QueryOutputLineNumber() As Integer?
        Me.OutputLineNumber = Me.Query(Me.OutputLineNumber, Me.OutputLineNumberQueryCommand)
        Return Me.OutputLineNumber
    End Function

    ''' <summary> Gets the Output Line Number command format. </summary>
    ''' <value> The Output Line Number command format. </value>
    ''' <remarks> SCPI: ":ARM:OLIN {0}" </remarks>
    Protected Overridable ReadOnly Property OutputLineNumberCommandFormat As String

    ''' <summary> Writes the Arm Output Line Number without reading back the value from the device. </summary>
    ''' <param name="value"> The current OutputLineNumber. </param>
    ''' <returns> The Arm Output Line Number or none if unknown. </returns>
    Public Function WriteOutputLineNumber(ByVal value As Integer) As Integer?
        Me.OutputLineNumber = Me.Write(value, Me.OutputLineNumberCommandFormat)
        Return Me.OutputLineNumber
    End Function

#End Region

#Region " TIMER TIME SPAN "

    ''' <summary> Gets or sets the cached Arm Timer Interval. </summary>
    ''' <remarks> The Timer Interval is used to Timer Interval operation in the Arm layer. After the programmed
    ''' Arm event occurs, the instrument waits until the Timer Interval period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Arm Timer Interval or none if not set or unknown. </value>
    Public Overloads Property TimerInterval As TimeSpan?
        Get
            Return Me.ActiveLayer.TimerInterval
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Me.TimerInterval.Equals(value) Then
                Me.ActiveLayer.TimerInterval = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.TimerInterval))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Arm Timer Interval. </summary>
    ''' <param name="value"> The current TimerTimeSpan. </param>
    ''' <returns> The Arm Timer Interval or none if unknown. </returns>
    Public Function ApplyTimerTimeSpan(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteTimerTimeSpan(value)
        Return Me.QueryTimerTimeSpan()
    End Function

    ''' <summary> Gets the Timer Interval query command. </summary>
    ''' <value> The Timer Interval query command. </value>
    ''' <remarks> SCPI: ":ARM:TIM?" </remarks>
    Protected Overridable ReadOnly Property TimerIntervalQueryCommand As String

    ''' <summary> Gets the Timer Interval format for converting the query to time span. </summary>
    ''' <value> The Timer Interval query command. </value>
    ''' <remarks> For example: "s\.fff" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property TimerIntervalFormat As String

    ''' <summary> Queries the Timer Interval. </summary>
    ''' <returns> The Timer Interval or none if unknown. </returns>
    Public Function QueryTimerTimeSpan() As TimeSpan?
        Me.TimerInterval = Me.Query(Me.TimerInterval, Me.TimerIntervalFormat, Me.TimerIntervalQueryCommand)
        Return Me.TimerInterval
    End Function

    ''' <summary> Gets the Timer Interval command format. </summary>
    ''' <value> The query command format. </value>
    ''' <remarks> SCPI: ":ARM:TIM {0:s\.fff}" </remarks>
    Protected Overridable ReadOnly Property TimerIntervalCommandFormat As String

    ''' <summary> Writes the Arm Timer Interval without reading back the value from the device. </summary>
    ''' <param name="value"> The current TimerTimeSpan. </param>
    ''' <returns> The Arm Timer Interval or none if unknown. </returns>
    Public Function WriteTimerTimeSpan(ByVal value As TimeSpan) As TimeSpan?
        Me.TimerInterval = Me.Write(value, Me.TimerIntervalQueryCommand)
    End Function

#End Region

#End Region

End Class

