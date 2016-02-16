Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a Trigger Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class TriggerSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="TriggerSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Count = 1
        Me.Delay = TimeSpan.Zero
    End Sub

#End Region

#Region " COMMANDS "

    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    ''' <remakrs> SCPI: ":INIT". </remakrs>
    Protected Overridable ReadOnly Property InitiateCommand As String

    ''' <summary> Initiates operations. </summary>
    ''' <remarks> This command is used to initiate source-measure operation by taking the SourceMeter
    ''' out of idle. The :READ? and :MEASure? commands also perform an initiation. Note that if auto
    ''' output-off is disabled (SOURce1:CLEar:AUTO OFF), the source output must first be turned on
    ''' before an initiation can be performed. The :MEASure? command automatically turns the output
    ''' source on before performing the initiation. </remarks>
    Public Sub Initiate()
        Me.Write(Me.InitiateCommand)
    End Sub

    ''' <summary> Gets the Abort command. </summary>
    ''' <value> The Abort command. </value>
    ''' <remakrs> SCPI: ":ABOR". </remakrs>
    Protected Overridable ReadOnly Property AbortCommand As String

    ''' <summary> Aborts operations. </summary>
    ''' <remarks> When this action command is sent, the SourceMeter aborts operation and returns to the
    ''' idle state. A faster way to return to idle is to use the DCL or SDC command. With auto output-
    ''' off enabled (:SOURce1:CLEar:AUTO ON), the output will remain on if operation is terminated
    ''' before the output has a chance to automatically turn off. </remarks>
    Public Sub Abort()
        If Not String.IsNullOrWhiteSpace(Me.AbortCommand) Then
            Me.Session.WriteLine(Me.AbortCommand)
        End If
    End Sub

#End Region

#Region " ARM SOURCE "

    ''' <summary> The Arm Source. </summary>
    Private _ArmSource As ArmSource?

    ''' <summary> Gets or sets the cached source ArmSource. </summary>
    ''' <value> The <see cref="ArmSource">source Arm Source</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property ArmSource As ArmSource?
        Get
            Return Me._ArmSource
        End Get
        Protected Set(ByVal value As ArmSource?)
            If Not Me.ArmSource.Equals(value) Then
                Me._ArmSource = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ArmSource))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Arm Source. </summary>
    ''' <param name="value"> The  Source Arm Source. </param>
    ''' <returns> The <see cref="ArmSource">source Arm Source</see> or none if unknown. </returns>
    Public Function ApplyArmSource(ByVal value As ArmSource) As ArmSource?
        Me.WriteArmSource(value)
        Return Me.QueryArmSource()
    End Function

    ''' <summary> Gets the Arm source query command. </summary>
    ''' <value> The Arm source query command. </value>
    ''' <remarks> SCPI: ":TRAC:ARM?" </remarks>
    Protected Overridable ReadOnly Property ArmSourceQueryCommand As String

    ''' <summary> Queries the Arm Source. </summary>
    ''' <returns> The <see cref="ArmSource">Arm Source</see> or none if unknown. </returns>
    Public Function QueryArmSource() As ArmSource?
        Dim currentValue As String = Me.ArmSource.ToString
        If String.IsNullOrEmpty(Me.Session.EmulatedReply) Then Me.Session.EmulatedReply = currentValue
        If Not String.IsNullOrWhiteSpace(Me.ArmSourceQueryCommand) Then
            currentValue = Me.Session.QueryTrimEnd(Me.ArmSourceQueryCommand)
        End If
        If String.IsNullOrWhiteSpace(currentValue) Then
            Me.ArmSource = New ArmSource?
        Else
            Dim se As New StringEnumerator(Of ArmSource)
            Me.ArmSource = se.ParseContained(currentValue.BuildDelimitedValue)
        End If
        Return Me.ArmSource
    End Function

    ''' <summary> Gets the Arm source command format. </summary>
    ''' <value> The write Arm source command format. </value>
    ''' <remarks> SCPI: ":TRAC:ARM {0}". </remarks>
    Protected Overridable ReadOnly Property ArmSourceCommandFormat As String

    ''' <summary> Writes the Arm Source without reading back the value from the device. </summary>
    ''' <param name="value"> The Arm Source. </param>
    ''' <returns> The <see cref="ArmSource">Arm Source</see> or none if unknown. </returns>
    Public Function WriteArmSource(ByVal value As ArmSource) As ArmSource?
        If Not String.IsNullOrWhiteSpace(Me.ArmSourceCommandFormat) Then
            Me.Session.WriteLine(Me.ArmSourceCommandFormat, value.ExtractBetween())
        End If
        Me.ArmSource = value
        Return Me.ArmSource
    End Function

#End Region

#Region " AUTO DELAY ENABLED "

    ''' <summary> Auto Delay enabled. </summary>
    Private _AutoDelayEnabled As Boolean?

    ''' <summary> Gets or sets the cached Auto Delay Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Delay Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoDelayEnabled As Boolean?
        Get
            Return Me._AutoDelayEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoDelayEnabled, value) Then
                Me._AutoDelayEnabled = value
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
    ''' <remarks> SCPI: ":TRIG:DEL:AUTO?" </remarks>
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
    ''' <remarks> SCPI: ":TRIG:DEL:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoDelayEnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoDelayEnabled = Me.Write(value, Me.AutoDelayEnabledCommandFormat)
        Return Me.AutoDelayEnabled
    End Function

#End Region

#Region " TRIGGER DIRECTION "

    ''' <summary> The Trigger Direction. </summary>
    Private _Direction As Direction?

    ''' <summary> Gets or sets the cached source Direction. </summary>
    ''' <value> The <see cref="Direction">source Trigger Direction</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property Direction As Direction?
        Get
            Return Me._Direction
        End Get
        Protected Set(ByVal value As Direction?)
            If Not Me.Direction.Equals(value) Then
                Me._Direction = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Direction))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Trigger Direction. </summary>
    ''' <param name="value"> The  Source Trigger Direction. </param>
    ''' <returns> The <see cref="Direction">source Trigger Direction</see> or none if unknown. </returns>
    Public Function ApplyDirection(ByVal value As Direction) As Direction?
        Me.WriteDirection(value)
        Return Me.QueryDirection()
    End Function

    ''' <summary> Gets the Trigger Direction query command. </summary>
    ''' <value> The Trigger Direction query command. </value>
    ''' <remarks> SCPI: ":TRIG:DIR" </remarks>
    Protected Overridable ReadOnly Property DirectionQueryCommand As String

    ''' <summary> Queries the Trigger Direction. </summary>
    ''' <returns> The <see cref="Direction">Trigger Direction</see> or none if unknown. </returns>
    Public Function QueryDirection() As Direction?
        Me.Direction = Me.Query(Of Direction)(Me.DirectionQueryCommand, Me.Direction)
        Return Me.Direction
    End Function

    ''' <summary> Gets the Trigger Direction command format. </summary>
    ''' <value> The Trigger Direction command format. </value>
    ''' <remarks> SCPI: ":TRIG:DIR {0}" </remarks>
    Protected Overridable ReadOnly Property DirectionCommandFormat As String

    ''' <summary> Writes the Trigger Direction without reading back the value from the device. </summary>
    ''' <param name="value"> The Trigger Direction. </param>
    ''' <returns> The <see cref="Direction">Trigger Direction</see> or none if unknown. </returns>
    Public Function WriteDirection(ByVal value As Direction) As Direction?
        Me.Direction = Me.Write(Of Direction)(Me.DirectionCommandFormat, value)
        Return Me.Direction
    End Function

#End Region

#Region " COUNT "

    Private _Count As Integer?
    ''' <summary> Gets or sets the cached Trigger Count. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Trigger Count or none if not set or unknown. </value>
    Public Overloads Property Count As Integer?
        Get
            Return Me._Count
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.Count, value) Then
                Me._Count = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Count))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Count. </summary>
    ''' <param name="value"> The current Count. </param>
    ''' <returns> The Count or none if unknown. </returns>
    Public Function ApplyCount(ByVal value As Integer) As Integer?
        Me.WriteCount(value)
        Return Me.QueryCount()
    End Function

    ''' <summary> Gets trigger count query command. </summary>
    ''' <value> The trigger count query command. </value>
    ''' <remarks> SCPI: ":TRIG:COUN?" </remarks>
    Protected Overridable ReadOnly Property CountQueryCommand As String

    ''' <summary> Queries the current PointsCount. </summary>
    ''' <returns> The PointsCount or none if unknown. </returns>
    Public Function QueryCount() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.CountQueryCommand) Then
            Me.Count = Me.Session.Query(0I, Me.CountQueryCommand)
        End If
        Return Me.Count
    End Function

    ''' <summary> Gets trigger count command format. </summary>
    ''' <value> The trigger count command format. </value>
    ''' <remarks> SCPI: ":TRIG:COUN {0}" </remarks>
    Protected Overridable ReadOnly Property CountCommandFormat As String

    ''' <summary> Write the Trace PointsCount without reading back the value from the device. </summary>
    ''' <param name="value"> The current PointsCount. </param>
    ''' <returns> The PointsCount or none if unknown. </returns>
    Public Function WriteCount(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.CountCommandFormat) Then
            Me.Session.WriteLine(Me.CountCommandFormat, value)
        End If
        Me.Count = value
        Return Me.Count
    End Function

#End Region

#Region " DELAY "

    ''' <summary> The delay. </summary>
    Private _Delay As TimeSpan?

    ''' <summary> Gets or sets the cached Trigger Delay. </summary>
    ''' <remarks> The delay is used to delay operation in the trigger layer. After the programmed
    ''' trigger event occurs, the instrument waits until the delay period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Trigger Delay or none if not set or unknown. </value>
    Public Overloads Property Delay As TimeSpan?
        Get
            Return Me._Delay
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Nullable.Equals(Me.Delay, value) Then
                Me._Delay = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Delay))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Delay. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Trigger Delay or none if unknown. </returns>
    Public Function ApplyDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteDelay(value)
        Return Me.QueryDelay()
    End Function

    ''' <summary> Gets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    ''' <remarks> SCPI: ":TRIG:DEL?" </remarks>
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
    ''' <remarks> SCPI: ":TRIG:DEL {0:s\.fff}" </remarks>
    Protected Overridable ReadOnly Property DelayCommandFormat As String

    ''' <summary> Writes the Trigger Delay without reading back the value from the device. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Trigger Delay or none if unknown. </returns>
    Public Function WriteDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.Delay = Me.Write(value, Me.DelayCommandFormat)
        Return Me.Delay
    End Function

#End Region

#Region " INPUT LINE NUMBER "

    Private _InputLineNumber As Integer?
    ''' <summary> Gets or sets the cached Trigger Input Line Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Trigger InputLineNumber or none if not set or unknown. </value>
    Public Overloads Property InputLineNumber As Integer?
        Get
            Return Me._InputLineNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.InputLineNumber, value) Then
                Me._InputLineNumber = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.InputLineNumber))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Input Line Number. </summary>
    ''' <param name="value"> The current Input Line Number. </param>
    ''' <returns> The Input Line Number or none if unknown. </returns>
    Public Function ApplyInputLineNumber(ByVal value As Integer) As Integer?
        Me.WriteInputLineNumber(value)
        Return Me.QueryInputLineNumber()
    End Function

    ''' <summary> Gets the Input Line Number query command. </summary>
    ''' <value> The Input Line Number query command. </value>
    ''' <remarks> SCPI: ":TRIG:ILIN?" </remarks>
    Protected Overridable ReadOnly Property InputLineNumberQueryCommand As String

    ''' <summary> Queries the InputLineNumber. </summary>
    ''' <returns> The Input Line Number or none if unknown. </returns>
    Public Function QueryInputLineNumber() As Integer?
        Me.InputLineNumber = Me.Query(Me.InputLineNumber, Me.InputLineNumberQueryCommand)
        Return Me.InputLineNumber
    End Function

    ''' <summary> Gets the Input Line Number command format. </summary>
    ''' <value> The Input Line Number command format. </value>
    ''' <remarks> SCPI: ":TRIG:ILIN {0}" </remarks>
    Protected Overridable ReadOnly Property InputLineNumberCommandFormat As String

    ''' <summary> Writes the Trigger Input Line Number without reading back the value from the device. </summary>
    ''' <param name="value"> The current InputLineNumber. </param>
    ''' <returns> The Trigger Input Line Number or none if unknown. </returns>
    Public Function WriteInputLineNumber(ByVal value As Integer) As Integer?
        Me.InputLineNumber = Me.Write(value, Me.InputLineNumberCommandFormat)
        Return Me.InputLineNumber
    End Function

#End Region

#Region " OUTPUT LINE NUMBER "

    Private _OutputLineNumber As Integer?
    ''' <summary> Gets or sets the cached Trigger Output Line Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Trigger OutputLineNumber or none if not set or unknown. </value>
    Public Overloads Property OutputLineNumber As Integer?
        Get
            Return Me._OutputLineNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.OutputLineNumber, value) Then
                Me._OutputLineNumber = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.OutputLineNumber))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Output Line Number. </summary>
    ''' <param name="value"> The current Output Line Number. </param>
    ''' <returns> The Output Line Number or none if unknown. </returns>
    Public Function ApplyOutputLineNumber(ByVal value As Integer) As Integer?
        Me.WriteOutputLineNumber(value)
        Return Me.QueryOutputLineNumber()
    End Function

    ''' <summary> Gets the Output Line Number query command. </summary>
    ''' <value> The Output Line Number query command. </value>
    ''' <remarks> SCPI: ":TRIG:OLIN?" </remarks>
    Protected Overridable ReadOnly Property OutputLineNumberQueryCommand As String

    ''' <summary> Queries the OutputLineNumber. </summary>
    ''' <returns> The Output Line Number or none if unknown. </returns>
    Public Function QueryOutputLineNumber() As Integer?
        Me.OutputLineNumber = Me.Query(Me.OutputLineNumber, Me.OutputLineNumberQueryCommand)
        Return Me.OutputLineNumber
    End Function

    ''' <summary> Gets the Output Line Number command format. </summary>
    ''' <value> The Output Line Number command format. </value>
    ''' <remarks> SCPI: ":TRIG:OLIN {0}" </remarks>
    Protected Overridable ReadOnly Property OutputLineNumberCommandFormat As String

    ''' <summary> Writes the Trigger Output Line Number without reading back the value from the device. </summary>
    ''' <param name="value"> The current OutputLineNumber. </param>
    ''' <returns> The Trigger Output Line Number or none if unknown. </returns>
    Public Function WriteOutputLineNumber(ByVal value As Integer) As Integer?
        Me.OutputLineNumber = Me.Write(value, Me.OutputLineNumberCommandFormat)
        Return Me.OutputLineNumber
    End Function

#End Region

#Region " TIMER TIME SPAN "

    ''' <summary> The Timer Interval. </summary>
    Private _TimerInterval As TimeSpan?

    ''' <summary> Gets or sets the cached Trigger Timer Interval. </summary>
    ''' <remarks> The Timer Interval is used to Timer Interval operation in the trigger layer. After the programmed
    ''' trigger event occurs, the instrument waits until the Timer Interval period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Trigger Timer Interval or none if not set or unknown. </value>
    Public Overloads Property TimerInterval As TimeSpan?
        Get
            Return Me._TimerInterval
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Me.TimerInterval.Equals(value) Then
                Me._TimerInterval = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.TimerInterval))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Timer Interval. </summary>
    ''' <param name="value"> The current TimerTimeSpan. </param>
    ''' <returns> The Trigger Timer Interval or none if unknown. </returns>
    Public Function ApplyTimerTimeSpan(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteTimerTimeSpan(value)
        Return Me.QueryTimerTimeSpan()
    End Function

    ''' <summary> Gets the Timer Interval query command. </summary>
    ''' <value> The Timer Interval query command. </value>
    ''' <remarks> SCPI: ":TRIG:TIM?" </remarks>
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
    ''' <remarks> SCPI: ":TRIG:TIM {0:s\.fff}" </remarks>
    Protected Overridable ReadOnly Property TimerIntervalCommandFormat As String

    ''' <summary> Writes the Trigger Timer Interval without reading back the value from the device. </summary>
    ''' <param name="value"> The current TimerTimeSpan. </param>
    ''' <returns> The Trigger Timer Interval or none if unknown. </returns>
    Public Function WriteTimerTimeSpan(ByVal value As TimeSpan) As TimeSpan?
        Me.TimerInterval = Me.Write(value, Me.TimerIntervalQueryCommand)
    End Function

#End Region

End Class

''' <summary> Enumerates the arm layer bypass mode. </summary>
Public Enum Direction
    <ComponentModel.Description("Not Defined ()")> None
    <ComponentModel.Description("Immediate (IMM)")> Immediate
    <ComponentModel.Description("End (END)")> [End]
End Enum

''' <summary> Enumerates the arm layer control sources. </summary>
Public Enum ArmSource
    <ComponentModel.Description("Not Defined ()")> None
    <ComponentModel.Description("Bus (BUS)")> Bus
    <ComponentModel.Description("External (EXT)")> External
    <ComponentModel.Description("Hold operation (HOLD)")> Hold
    <ComponentModel.Description("Immediate (IMM)")> Immediate
    <ComponentModel.Description("Manual (MAN)")> Manual
    <ComponentModel.Description("Timer (TIM)")> Timer

    ''' <summary> Event detection for the arm layer is satisfied when either a positive-going or 
    ''' a negative-going pulse (via the SOT line of the Digital I/O) is received. </summary>
    <ComponentModel.Description("Start Test Pulsed High or Low (BSTES)")> StartTestBoth

    ''' <summary> Event detection for the arm layer is satisfied when a positive-going pulse 
    ''' (via the SOT line of the Digital I/O) is received.  </summary>
    <ComponentModel.Description("Start Test Pulsed High (PSTES)")> StartTestHigh

    ''' <summary> Event detection for the arm layer is satisfied when a negative-going pulse 
    ''' (via the SOT line of the Digital I/O) is received. </summary>
    <ComponentModel.Description("Start Test Pulsed High (NSTES)")> StartTestLow
    <ComponentModel.Description("Trigger Link (TLIN)")> TriggerLink
End Enum

''' <summary> Enumerates the arm to trigger events. </summary>
Public Enum TriggerEvent
    <ComponentModel.Description("None (NONE)")> None
    <ComponentModel.Description("Source (SOUR)")> Source
    <ComponentModel.Description("Delay (DEL)")> Delay
    <ComponentModel.Description("Sense (SENS)")> Sense
End Enum

''' <summary> Enumerates the trigger layer control sources. </summary>
Public Enum TriggerSource
    <ComponentModel.Description("Not Defined ()")> None
    <ComponentModel.Description("Bus (BUS)")> Bus
    <ComponentModel.Description("External (Ext)")> External
    <ComponentModel.Description("Immediate (IMM)")> Immediate
    <ComponentModel.Description("Trigger Link (TLIN)")> TriggerLink
End Enum


