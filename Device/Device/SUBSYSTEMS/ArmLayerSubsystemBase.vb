''' <summary> Defines a SCPI arm layer base Subsystem. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/5/2013" by="David" revision=""> Created based on SCPI 5.1 library. </history>
Public MustInherit Class ArmLayerSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ArmLayerSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        Me.New(1, statusSubsystem)
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="ArmLayerSubsystemBase" /> class.
    ''' </summary>
    ''' <param name="layerNumber">     The arm layer number. </param>
    ''' <param name="statusSubsystem"> A reference to a
    '''                                <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal layerNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._LayerNumber = layerNumber
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
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.ArmCount = 1
        Me.Delay = TimeSpan.Zero
        Me.InputLineNumber = 2
        Me.OutputLineNumber = 1
        Me.TimerInterval = TimeSpan.FromSeconds(1)
    End Sub

#End Region

#Region " NUMERIC COMMAND BUILDER "

    ''' <summary> Gets or sets the arm layer number. </summary>
    ''' <value> The arm layer number. </value>
    Protected ReadOnly Property LayerNumber As Integer

#End Region

#Region " COMMANDS "

    ''' <summary> Gets the Immediate command. </summary>
    ''' <value> The Immediate command. </value>
    ''' <remarks> SCPI: ":ARM:LAYx:IMM". </remarks>
    Protected Overridable ReadOnly Property ImmediateCommand As String

    ''' <summary> Immediately move tot he next layer. </summary>
    Public Sub Immediate()
        Me.Session.Execute(Me.ImmediateCommand)
    End Sub

#End Region

#Region " ARM DIRECTION "

    ''' <summary> Direction getter. </summary>
    ''' <returns> A Nullable(Of T) </returns>
    Public MustOverride Function DirectionGetter() As Integer?

    ''' <summary> Direction setter. </summary>
    ''' <param name="value"> The current ArmCount. </param>
    Public MustOverride Sub DirectionSetter(ByVal value As Integer?)

    Public Overridable Function ApplyDirection(ByVal value As Integer) As Integer?
        Me.DirectionWriter(value)
        Return Me.DirectionReader()
    End Function

    ''' <summary> Gets the ARM Direction query command. </summary>
    ''' <value> The ARM Direction query command. </value>
    ''' <remarks> SCPI: ":ARM:LAYx:DIR?" </remarks>
    Protected Overridable ReadOnly Property DirectionQueryCommand As String

    ''' <summary> Direction reader. </summary>
    ''' <returns> An Integer? </returns>
    Public MustOverride Function DirectionReader() As Integer?

    ''' <summary> Gets the ARM Direction command format. </summary>
    ''' <value> The ARM Direction command format. </value>
    ''' <remarks> SCPI: ":ARM:LAYx:DIR {0}" </remarks>
    Protected Overridable ReadOnly Property DirectionCommandFormat As String

    ''' <summary> Direction writer. </summary>
    ''' <param name="value"> The current ArmCount. </param>
    ''' <returns> An Integer? </returns>
    Public MustOverride Function DirectionWriter(ByVal value As Integer) As Integer?

#End Region

#Region " ARM COUNT "

    Private _ArmCount As Integer?
    ''' <summary> Gets or sets the cached Arm ArmCount. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Arm model. </remarks>
    ''' <value> The Arm ArmCount or none if not set or unknown. </value>
    Public Overloads Property ArmCount As Integer?
        Get
            Return Me._ArmCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.ArmCount, value) Then
                Me._ArmCount = value
                Me.SafePostPropertyChanged()
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

#Region " DELAY "

    Private _Delay As TimeSpan?
    ''' <summary> Gets or sets the cached Arm Delay. </summary>
    ''' <remarks> The delay is used to delay operation in the Arm layer. After the programmed
    ''' Arm event occurs, the instrument waits until the delay period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Arm Delay or none if not set or unknown. </value>
    Public Overloads Property Delay As TimeSpan?
        Get
            Return Me._Delay
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Nullable.Equals(Me.Delay, value) Then
                Me._Delay = value
                Me.SafePostPropertyChanged()
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
    ''' <remarks> SCPI: ":ARM:LAYx:DEL?" </remarks>
    Protected Overridable ReadOnly Property DelayQueryCommand As String

    ''' <summary> Gets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    ''' <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property DelayFormat As String

    ''' <summary> Queries the Delay. </summary>
    ''' <returns> The Delay or none if unknown. </returns>
    Public Function QueryDelay() As TimeSpan?
        Me.Delay = Me.Query(Me.Delay, Me.DelayFormat, Me.DelayQueryCommand)
        Return Me.Delay
    End Function

    ''' <summary> Gets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    ''' <remarks> SCPI: ":ARM:LAYx:DEL {0:s\.FFFFFFF}" </remarks>
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

    Private _InputLineNumber As Integer?
    ''' <summary> Gets or sets the cached Arm Input Line Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Arm model. </remarks>
    ''' <value> The Arm InputLineNumber or none if not set or unknown. </value>
    Public Overloads Property InputLineNumber As Integer?
        Get
            Return Me._InputLineNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.InputLineNumber, value) Then
                Me._InputLineNumber = value
                Me.SafePostPropertyChanged()
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
    ''' <remarks> SCPI: ":ARM:LAYx:ILIN?" </remarks>
    Protected Overridable ReadOnly Property InputLineNumberQueryCommand As String

    ''' <summary> Queries the InputLineNumber. </summary>
    ''' <returns> The Input Line Number or none if unknown. </returns>
    Public Function QueryInputLineNumber() As Integer?
        Me.InputLineNumber = Me.Query(Me.InputLineNumber, Me.InputLineNumberQueryCommand)
        Return Me.InputLineNumber
    End Function

    ''' <summary> Gets the Input Line Number command format. </summary>
    ''' <value> The Input Line Number command format. </value>
    ''' <remarks> SCPI: ":ARM:LAYx:ILIN {1}" </remarks>
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

    Private _OutputLineNumber As Integer?
    ''' <summary> Gets or sets the cached Arm Output Line Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Arm model. </remarks>
    ''' <value> The Arm OutputLineNumber or none if not set or unknown. </value>
    Public Overloads Property OutputLineNumber As Integer?
        Get
            Return Me._OutputLineNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.OutputLineNumber, value) Then
                Me._OutputLineNumber = value
                Me.SafePostPropertyChanged()
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
    ''' <remarks> SCPI: ":ARM:LAYx:OLIN?" </remarks>
    Protected Overridable ReadOnly Property OutputLineNumberQueryCommand As String

    ''' <summary> Queries the OutputLineNumber. </summary>
    ''' <returns> The Output Line Number or none if unknown. </returns>
    Public Function QueryOutputLineNumber() As Integer?
        Me.OutputLineNumber = Me.Query(Me.OutputLineNumber, Me.OutputLineNumberQueryCommand)
        Return Me.OutputLineNumber
    End Function

    ''' <summary> Gets the Output Line Number command format. </summary>
    ''' <value> The Output Line Number command format. </value>
    ''' <remarks> SCPI: ":ARM:LAYx:OLIN {0}" </remarks>
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

    Private _TimerInterval As TimeSpan?
    ''' <summary> Gets or sets the cached Arm Timer Interval. </summary>
    ''' <remarks> The Timer Interval is used to Timer Interval operation in the Arm layer. After the programmed
    ''' Arm event occurs, the instrument waits until the Timer Interval period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Arm Timer Interval or none if not set or unknown. </value>
    Public Overloads Property TimerInterval As TimeSpan?
        Get
            Return Me._TimerInterval
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Me.TimerInterval.Equals(value) Then
                Me._TimerInterval = value
                Me.SafePostPropertyChanged()
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
    ''' <remarks> SCPI: ":ARM:LAYx:TIM?" </remarks>
    Protected Overridable ReadOnly Property TimerIntervalQueryCommand As String

    ''' <summary> Gets the Timer Interval format for converting the query to time span. </summary>
    ''' <value> The Timer Interval query command. </value>
    ''' <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property TimerIntervalFormat As String

    ''' <summary> Queries the Timer Interval. </summary>
    ''' <returns> The Timer Interval or none if unknown. </returns>
    Public Function QueryTimerTimeSpan() As TimeSpan?
        Me.TimerInterval = Me.Query(Me.TimerInterval, Me.TimerIntervalFormat, Me.TimerIntervalQueryCommand)
        Return Me.TimerInterval
    End Function

    ''' <summary> Gets the Timer Interval command format. </summary>
    ''' <value> The query command format. </value>
    ''' <remarks> SCPI: ":ARM:LAYx:TIM {0:s\.FFFFFFF}" </remarks>
    Protected Overridable ReadOnly Property TimerIntervalCommandFormat As String

    ''' <summary> Writes the Arm Timer Interval without reading back the value from the device. </summary>
    ''' <param name="value"> The current TimerTimeSpan. </param>
    ''' <returns> The Arm Timer Interval or none if unknown. </returns>
    Public Function WriteTimerTimeSpan(ByVal value As TimeSpan) As TimeSpan?
        Me.TimerInterval = Me.Write(value, Me.TimerIntervalQueryCommand)
        Return Me.TimerInterval
    End Function

#End Region

End Class

