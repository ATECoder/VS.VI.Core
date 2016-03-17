''' <summary> Defines a SCPI Measure Subsystem for a generic source measure instrument. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class MeasureSubsystem
    Inherits VI.Scpi.MeasureSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="MeasureSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary>
    ''' Sets subsystem values to their known execution clear state.
    ''' </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me._readings.Reset()
    End Sub

    ''' <summary>
    ''' Sets the subsystem values to their known execution reset state.
    ''' </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me._readings = New Readings
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

#Region " COMMAND SYNTAX "

#Region "  INIT, READ, FETCH "

    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    Protected Overrides ReadOnly Property InitiateCommand As String
        Get
            Return ":INIT"
        End Get
    End Property

    ''' <summary> Gets the fetch command. </summary>
    ''' <value> The fetch command. </value>
    Protected Overrides ReadOnly Property FetchCommand As String
        Get
            Return "FETCH?"
        End Get
    End Property

    ''' <summary> Gets the read command. </summary>
    ''' <value> The read command. </value>
    Protected Overrides ReadOnly Property ReadCommand As String
        Get
            Return "READ?"
        End Get
    End Property

#End Region

#End Region

#Region " PARSE READING "

    Private _readings As Readings

    ''' <summary> Returns the readings. </summary>
    ''' <returns> The readings. </returns>
    Public Function Readings() As Readings
        Return Me._readings
    End Function

    ''' <summary> Parses a new set of reading elements. </summary>
    ''' <param name="reading"> Specifies the measurement text to parse into the new reading. </param>
    Public Overrides Sub ParseReading(ByVal reading As String)
        If Me.Readings.TryParse(reading) Then

        End If
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
    ''' <summary> The level compliance bits. </summary>
    ''' <remarks> This is a custom value. </remarks>
    Public Overridable Property LevelComplianceBits As Long = CLng(2 ^ 32)

#Region " VOLTAGE "

    Private _voltage As Double?

    ''' <summary> Gets or sets the voltage. </summary>
    ''' <value> The voltage. </value>
    ''' <remarks> This element provides the voltage measurement or the programmed voltage source reading. 
    '''           If sourcing voltage and measuring voltage, this element will provide the voltage measurement 
    '''           (measure reading takes priority over source reading). 
    '''           If voltage is not sourced or measured, the  NAN (not a number) value of +9.91e37 is used. </remarks>
    Public Property Voltage As Double?
        Get
            Return Me._voltage
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Voltage, value) Then
                Me._voltage = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Voltage))
            End If
        End Set
    End Property

    Private _VoltageComplianceLevel As Double?

    ''' <summary> Gets or sets the voltage compliance level. </summary>
    ''' <value> The voltage compliance level. </value>
    Public Property VoltageComplianceLevel As Double?
        Get
            Return Me._VoltageComplianceLevel
        End Get
        Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.VoltageComplianceLevel, value) Then
                Me._VoltageComplianceLevel = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.VoltageComplianceLevel))
            End If
        End Set
    End Property


#End Region

#Region " CURRENT "

    Private _Current As Double?

    ''' <summary> Gets or sets the current. </summary>
    ''' <value> The current. </value>
    ''' <remarks> This element provides the current measurement or the programmed current source reading. 
    '''           If sourcing current and measuring current, this element will provide the current 
    '''           measurement (measure reading takes priority over source reading). If current is not 
    '''           sourced or measured, the NAN (not a number) value of +9.91e37 is used.
    '''          </remarks>
    Public Property Current As Double?
        Get
            Return Me._Current
        End Get
        Private Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Current, value) Then
                Me._Current = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Current))
            End If
        End Set
    End Property

    Private _CurrentComplianceLevel As Double?

    ''' <summary> Gets or sets the current compliance level. </summary>
    ''' <value> The current compliance level. </value>
    Public Property CurrentComplianceLevel As Double?
        Get
            Return Me._CurrentComplianceLevel
        End Get
        Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.CurrentComplianceLevel, value) Then
                Me._CurrentComplianceLevel = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.CurrentComplianceLevel))
            End If
        End Set
    End Property

#End Region

#Region " RESISTANCE "

    Private _Resistance As Double?

    ''' <summary> Gets or sets the resistance. </summary>
    ''' <value> The resistance. </value>
    Public Property Resistance As Double?
        Get
            Return Me._Resistance
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Resistance, value) Then
                Me._Resistance = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Resistance))
            End If
        End Set
    End Property

#End Region

#Region " TIMESTAMP "

    Private _Timestamp As Double?

    ''' <summary> Gets or sets the timestamp. </summary>
    ''' <remarks> A timestamp is available to reference each group of readings to a point in time. The
    ''' relative timestamp operates as a timer that starts at zero seconds when the instrument is
    ''' turned on or when the relative timestamp is reset (:SYSTem:TIME:RESet). The timestamp for
    ''' each reading sent over the bus is referenced, in seconds, to the start time.After 99,999.999
    ''' seconds, the timer resets to zero and starts over. </remarks>
    ''' <value> The timestamp. </value>
    Public Property Timestamp As Double?
        Get
            Return Me._Timestamp
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Timestamp, value) Then
                Me._Timestamp = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Timestamp))
            End If
        End Set
    End Property

#End Region

#Region " STATUS "

    ''' <summary> The compliance bits. </summary>
    ''' <remarks> See 2400 manual page 432 of 592</remarks>
    Public Overridable Property ComplianceBit As Integer = StatusWordBit.HitCompliance

    ''' <summary> The contact check bit pattern. </summary>
    Public Overridable Property FailedContactCheckBit As Integer = StatusWordBit.FailedContactCheck

    ''' <summary> The over-range bit pattern. </summary>
    Public Overridable Property OverRangeBit As Integer = StatusWordBit.OverRange

    ''' <summary> The range compliance bits. </summary>
    Public Overridable Property RangeComplianceBits As Integer = StatusWordBit.HitRangeCompliance

    ''' <summary> The voltage protection bits. </summary>
    Public Overridable Property VoltageProtectionBits As Integer = StatusWordBit.HitVoltageProtection

    Public Function IsStatusBit(ByVal bit As Integer) As Boolean
        Return Me.Status.HasValue AndAlso ((1 And (Me.Status.Value << bit)) <> 0)
    End Function

    Private _status As Long?

    ''' <summary> Gets or sets the status. </summary>
    ''' <remarks> A status word is available to provide status information concerning SourceMeter
    ''' operation. The 24-bit status word is sent in a decimal form and has to be converted by the
    ''' user to the binary equivalent to determine the state of each bit in the word. For example, if
    ''' the status value is 65, the binary equivalent is 0000000000001000001. Bits 0 and 6 are set.
    ''' This is extended beyond the integer status range to add custom values. </remarks>
    ''' <value> The status. </value>
    Public Property Status As Long?
        Get
            Return Me._status
        End Get
        Protected Set(ByVal value As Long?)
            If Not Me.Status.Equals(value) Then
                Me._status = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Status))
            End If
        End Set
    End Property

#Region " PARSE READING -- LEGACY "

    ''' <summary> Parses the reading into the data elements. </summary>
    ''' <remarks> This assumes that the instrument is set to output all reading elements. </remarks>
    Public Sub ParseReadingLegacy(ByVal reading As String)
        If String.IsNullOrWhiteSpace(reading) Then
            Me.Voltage = New Double?
            Me.Current = New Double?
            Me.Resistance = New Double?
            Me.Timestamp = New Double?
            Me.Status = New Long?
        Else
            Dim readings As String() = reading.Split(","c)
            If readings.Count >= 5 Then
                Me.Voltage = VI.SessionBase.Parse(1.0F, readings(0))
                Me.Current = VI.SessionBase.Parse(1.0F, readings(1))
                Me.Resistance = VI.SessionBase.Parse(1.0F, readings(2))
                Me.Timestamp = VI.SessionBase.Parse(1.0F, readings(3))
                Me.Status = CLng(VI.SessionBase.Parse(1.0F, readings(4)))

                If Me.Status.HasValue AndAlso Me.VoltageComplianceLevel.HasValue AndAlso
                       (Not (Me.Voltage.Value >= Me.VoltageComplianceLevel.Value) Xor (Me.VoltageComplianceLevel.Value > 0)) Then
                    Me.Status = Me.Status Or 1 << Measurand.LevelComplianceBit
                End If

                If Me.Status.HasValue AndAlso Me.CurrentComplianceLevel.HasValue AndAlso
                      (Not (Me.Current.Value >= Me.CurrentComplianceLevel.Value) Xor (Me.CurrentComplianceLevel.Value > 0)) Then
                    Me.Status = Me.Status Or 1 << Measurand.LevelComplianceBit
                End If

            End If
        End If
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary>
    ''' Sets subsystem values to their known execution clear state.
    ''' </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.Voltage = New Double?
        Me.Current = New Double?
        Me.Resistance = New Double?
        Me.Timestamp = New Double?
        Me.Status = New Long?
    End Sub

    ''' <summary>
    ''' Sets the subsystem values to their known execution reset state.
    ''' </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Voltage = New Double?
        Me.Current = New Double?
        Me.Resistance = New Double?
        Me.Timestamp = New Double?
        Me.Status = New Long?
    End Sub

#End Region
#End If
#End Region