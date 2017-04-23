Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines a Measure Subsystem for a Tegam 1750 Resistance Measuring System. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/7/2013" by="David" revision=""> Created. </history>
Public Class MeasureSubsystem
    Inherits VI.R2D2.MeasureSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="MeasureSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._supportedCommands = New String() {"D111x", "M63x", "R1x", "T0x", "U0x", "U1x", "U2x", "Y3x"}
        Me.randomResistance = New Random(CInt(DateTime.Now.Ticks Mod Integer.MaxValue))
        Me.MaximumDifference = 0.01
        Me.InitialDelay = TimeSpan.FromMilliseconds(150)
        Me.MeasurementDelay = TimeSpan.FromMilliseconds(150)
        Me.MaximumTrialsCount = 5
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary>
    ''' Sets subsystem values to their known execution clear state.
    ''' </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.LastReading = ""
        Me.Resistance = New Double?
        Me.OverRangeOpenWire = New Boolean?
    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Clears last reading.
    '''           </para></remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.LastReading = ""
        Me.Resistance = New Double?
        Me.OverRangeOpenWire = New Boolean?
    End Sub

    ''' <summary>
    ''' Sets the subsystem values to their known execution reset state.
    ''' </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.RangeMode = Tegam.RangeMode.R0
        Me.TriggerMode = Tegam.TriggerMode.T2
        Me.MaximumDifference = 0.01
        Me.InitialDelay = TimeSpan.FromMilliseconds(150)
        Me.MeasurementDelay = TimeSpan.FromMilliseconds(150)
        Me.MaximumTrialsCount = 5
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

#Region " COMMAND SYNTAX "

#Region "  INIT, READ, FETCH "

    ''' <summary> Gets the fetch command. </summary>
    ''' <value> The fetch command. </value>
    <Obsolete("Not supported")>
    Protected Overrides ReadOnly Property FetchCommand As String = ""

    ''' <summary> Gets the read command. </summary>
    ''' <value> The read command. </value>
    <Obsolete("Not supported")>
    Protected Overrides ReadOnly Property ReadCommand As String = ""

#End Region

#End Region

#Region " WRITE "

    Private _SupportedCommands As String()

    ''' <summary> Gets the supported commands. </summary>
    ''' <returns> A String </returns>
    Public Function SupportedCommands() As String()
        Return _supportedCommands
    End Function

#End Region

#Region " FETCH "

    ''' <summary> Fetches the data. </summary>
    ''' <remarks> the Tegam meter does not require issuing a read command. </remarks>
    Public Overrides Sub Fetch()
        Me.Read()
    End Sub

    ''' <summary> Fetches the data. </summary>
    ''' <remarks> the Tegam meter does not require issuing a read command. </remarks>
    Public Overrides Sub Read()
        Me.Read(False)
    End Sub

    ''' <summary> Fetches the data. </summary>
    ''' <param name="syncNotifyMeasurementAvailable"> The synchronization notify measurement available
    ''' to read. </param>
    Public Overloads Sub Read(ByVal syncNotifyMeasurementAvailable As Boolean)
        Me.LastReading = Me.Session.ReadLineTrimEnd
        If Not String.IsNullOrWhiteSpace(Me.LastReading) Then
            ' the emulator will set the last reading. 
            Me.ParseReading(Me.LastReading)
            If syncNotifyMeasurementAvailable Then
                Me.MeasurementAvailable = True
            Else
                Me.AsyncMeasurementAvailable(True)
            End If
        End If
    End Sub

#End Region

#Region " RANDOM READING "

    ''' <summary> Gets the random resistance. </summary>
    ''' <value> The random reading. </value>
    Private Property RandomResistance As Random

    ''' <summary> Creates a new reading. </summary>
    ''' <returns> null if it fails, else a list of. </returns>
    Public Function NewReading(ByVal meanValue As Double, ByVal range As Double) As String
        Dim value As Double = meanValue * (1 + range * (Me.randomResistance.NextDouble() - 0.5))
        Return String.Format(Globalization.CultureInfo.InvariantCulture, "{0} Ohm", value)
    End Function

#End Region

#Region " PARSE READING "

    Private _Resistance As Double?

    ''' <summary> Gets or sets the resistance. </summary>
    ''' <value> The resistance. </value>
    Public Property Resistance As Double?
        Get
            Return Me._resistance
        End Get
        Set(ByVal value As Double?)
            Me._resistance = value
            Me.SafePostPropertyChanged(NameOf(Me.Resistance))
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

    ''' <summary> Parse scale. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value"> The value. </param>
    ''' <returns> A Double. </returns>
    Public Shared Function ParseScale(ByVal value As String) As Double
        If String.IsNullOrWhiteSpace(value) Then Throw New ArgumentNullException(NameOf(value))
        value = value.Trim
        Select Case True
            Case value.StartsWith("mOhm", StringComparison.Ordinal)
                Return 0.001
            Case value.StartsWith("Ohm", StringComparison.Ordinal)
                Return 1
            Case value.StartsWith("KOhm", StringComparison.Ordinal)
                Return 1000
            Case value.StartsWith("MOhm", StringComparison.Ordinal)
                Return 1000000
            Case Else
                Return 0
        End Select
    End Function

    ''' <summary> Parses a new set of reading elements. </summary>
    ''' <param name="reading"> Specifies the measurement text to parse into the new reading. </param>
    Public Overrides Sub ParseReading(ByVal reading As String)

        Dim overRanges As String() = New String() {"2.9999", "29.999", "299.99", "2999.9", "29999"}
        Dim res As New Double?
        Dim overRange As New Boolean?
        If String.IsNullOrWhiteSpace(reading) Then
            overRange = New Boolean?
            res = New Double?
        Else
            reading = reading.Trim
            Dim readings As String() = reading.Split(" "c)
            If readings.Count >= 2 Then
                If overRanges.Contains(readings(0)) Then
                    overRange = True
                    res = New Double?
                Else
                    Dim r As Double = Double.Parse(readings(0))
                    Dim units As String = readings(readings.Count - 1).Trim
                    Dim s As Double = MeasureSubsystem.ParseScale(units)
                    If s > 0 Then
                        overRange = False
                        res = r * s
                    Else
                        overRange = New Boolean?
                        res = New Double?
                    End If
                End If
            Else
                overRange = New Boolean?
                res = New Double?
            End If
        End If
        ' update the resistance last as it is used when property changed.
        Me.OverRangeOpenWire = overRange
        Me.Resistance = res
        Windows.Forms.Application.DoEvents()
    End Sub

#End Region

#Region " OVER RANGE OPEN WIRE "

    Private Property _OverRangeOpenWireTimeout As TimeSpan

    ''' <summary> Gets or sets the over range open wire timeout. 
    '''           Measurements is allowed to repeat until the timeout expires. </summary>
    ''' <value> The over range open wire timeout. </value>
    Public Property OverRangeOpenWireTimeout As TimeSpan
        Get
            Return Me._OverRangeOpenWireTimeout
        End Get
        Set(value As TimeSpan)
            If Not value.Equals(Me.OverRangeOpenWireTimeout) Then
                Me._OverRangeOpenWireTimeout = value
                Me.SafePostPropertyChanged(NameOf(Me.OverRangeOpenWireTimeout))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    Private _OverRangeOpenWireTimeoutEndTime As DateTimeOffset

    ''' <summary> Gets or sets the over range open wire timeout end time. </summary>
    ''' <value> The over range open wire timeout end time. </value>
    Public Property OverRangeOpenWireTimeoutEndTime As DateTimeOffset
        Get
            Return Me._OverRangeOpenWireTimeoutEndTime
        End Get
        Set(value As DateTimeOffset)
            If Not value.Equals(Me._OverRangeOpenWireTimeoutEndTime) Then
                Me._OverRangeOpenWireTimeoutEndTime = value
                Me.SafePostPropertyChanged(NameOf(Me.OverRangeOpenWireTimeoutEndTime))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    ''' <summary> Resets the over range open wire timeout. </summary>
    Public Sub ResetOverRangeOpenWireTimeout()
        Me.OverRangeOpenWireTimeoutEndTime = DateTimeOffset.Now.Add(Me.OverRangeOpenWireTimeout)
    End Sub

    ''' <summary> Checks if time elapsed beyond the
    ''' <see cref="OverRangeOpenWireTimeout">over range open wire timeout</see>. </summary>
    ''' <returns> <c>True</c> if time elapsed beyond the
    ''' <see cref="OverRangeOpenWireTimeout">over range open wire timeout</see>; Otherwise,
    ''' <c>False</c>. </returns>
    Public Function IsOverRangeOpenWireTimeoutExpired() As Boolean
        Return DateTimeOffset.Now > Me.OverRangeOpenWireTimeoutEndTime
    End Function

    Private Property _OverRangeOpenWire As Boolean?

    ''' <summary> Gets or sets the over range open wire. </summary>
    ''' <value> The over range open wire. </value>
    Public Property OverRangeOpenWire As Boolean?
        Get
            Return Me._OverRangeOpenWire
        End Get
        Set(ByVal value As Boolean?)
            Me._OverRangeOpenWire = value
            Me.SafePostPropertyChanged(NameOf(Me.OverRangeOpenWire))
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " RANGE MODE PARSERS "

    ''' <summary> Try convert. </summary>
    ''' <param name="current">   The current. </param>
    ''' <param name="range">     The range. </param>
    ''' <param name="rangeMode"> [in,out] The range mode. </param>
    ''' <returns> <c>True</c> if converted; Otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryConvert(ByVal current As String, ByVal range As String, ByRef rangeMode As RangeMode) As Boolean
        rangeMode = Tegam.RangeMode.R0
        For Each e As RangeMode In [Enum].GetValues(GetType(RangeMode))
            Dim c As String = ""
            Dim r As String = ""
            If MeasureSubsystem.TryParse(e, c, r) Then
                If String.Equals(c, current, StringComparison.OrdinalIgnoreCase) AndAlso
                        String.Equals(r, range, StringComparison.OrdinalIgnoreCase) Then
                    rangeMode = e
                    Exit For
                End If
            Else
            End If
        Next
        Return Tegam.RangeMode.R0 <> rangeMode
    End Function

    ''' <summary> Find range match. The matched range is the first range where both current and range
    ''' are greater or equal to the specified values. If a current match is not found, a range is
    ''' selected based on the range value along. </summary>
    ''' <param name="current">   The current. </param>
    ''' <param name="range">     The range. </param>
    ''' <param name="rangeMode"> [in,out] The range mode. </param>
    ''' <returns> <c>True</c> if converted; Otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryMatch(ByVal current As Double, ByVal range As Double, ByRef rangeMode As RangeMode) As Boolean
        rangeMode = RangeMode.R0
        For Each e As RangeMode In [Enum].GetValues(GetType(RangeMode))
            Dim c As Double
            Dim r As Double
            If MeasureSubsystem.TryParse(e, c, r) Then
                If current >= c AndAlso range <= r Then
                    rangeMode = e
                    Exit For
                ElseIf range <= r Then
                    ' if has the last matched range so we use the smallest current; set it in case current does not match.
                    rangeMode = e
                End If
            Else
            End If
        Next
        Return RangeMode.R0 <> rangeMode
    End Function


    ''' <summary> Try convert. </summary>
    ''' <param name="current">   The current. </param>
    ''' <param name="range">     The range. </param>
    ''' <param name="rangeMode"> [in,out] The range mode. </param>
    ''' <returns> <c>True</c> if converted; Otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryConvert(ByVal current As Double, ByVal range As Double, ByRef rangeMode As RangeMode) As Boolean
        rangeMode = RangeMode.R0
        For Each e As RangeMode In [Enum].GetValues(GetType(RangeMode))
            Dim c As Double
            Dim r As Double
            If MeasureSubsystem.TryParse(e, c, r) Then
                If Math.Abs(c - current) < 0.0001 * current AndAlso Math.Abs(r - range) < 0.0001 * range Then
                    rangeMode = e
                    Exit For
                End If
            Else
            End If
        Next
        Return RangeMode.R0 <> rangeMode
    End Function

    ''' <summary> Tries parsing the range mode from the range description. </summary>
    ''' <remarks> Parses the description, such as <para>
    ''' <c>2m ohm range @ 1 Amp Test Current (R01)</c></para><para>
    ''' To current (e.g,, 1 Amp) and resistance (2m ohm) values.</para> </remarks>
    ''' <param name="rangeMode"> The range mode. </param>
    ''' <param name="current">   [in,out] The current. </param>
    ''' <param name="range">     [in,out] The range. </param>
    ''' <returns> <c>True</c> if parsed; Otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
            Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryParse(ByVal rangeMode As RangeMode, ByRef current As String, ByRef range As String) As Boolean
        Dim elements As String() = rangeMode.Description.Split(" "c)
        If elements.Count = 9 AndAlso elements.Contains("range") AndAlso elements.Contains("Test") Then
            Dim builder As New System.Text.StringBuilder(elements(0))
            builder.Append(" ")
            builder.Append(elements(1))
            range = builder.ToString
            builder = New System.Text.StringBuilder(elements(4))
            builder.Append(" ")
            builder.Append(elements(5))
            current = builder.ToString
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary> Tries parsing the range mode from the range description. </summary>
    ''' <param name="rangeMode"> The range mode. </param>
    ''' <param name="current">   [in,out] The current. </param>
    ''' <param name="range">     [in,out] The range. </param>
    ''' <returns> <c>True</c> if parsed; Otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#",
            Justification:="This is the normative implementation of this method.")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#",
            Justification:="This is the normative implementation of this method.")>
    Public Shared Function TryParse(ByVal rangeMode As RangeMode, ByRef current As Double, ByRef range As Double) As Boolean
        Dim c As String = ""
        Dim r As String = ""
        If MeasureSubsystem.TryParse(rangeMode, c, r) Then
            Dim cc As String() = c.Split(" "c)
            If cc.Count = 2 Then
                If Double.TryParse(cc(0), current) Then
                    Select Case cc(1)
                        Case "A"
                            current *= 1
                        Case "mA"
                            current *= 0.001
                        Case "nA"
                            current *= 0.000000001
                        Case "uA"
                            current *= 0.000001
                        Case Else
                            Return False
                    End Select
                Else
                    Return False
                End If
            Else
                Return False
            End If
            Dim rr As String() = r.Split(" "c)
            If rr.Count = 2 Then
                Dim value As String = rr(0)
                Dim units As String = value.Substring(value.Length - 1, 1)
                Dim scale As Double = 0
                Select Case units
                    Case "2"
                        scale = 1
                    Case "0"
                        scale = 1
                    Case "m"
                        scale = 0.001
                        value = value.Substring(0, value.Length - 1)
                    Case "K", "k"
                        scale = 1000
                        value = value.Substring(0, value.Length - 1)
                    Case "M"
                        scale = 1000000
                        value = value.Substring(0, value.Length - 1)
                End Select
                If Double.TryParse(value, range) Then
                    range *= scale
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
            Return True
        Else
            Return False
        End If

    End Function

#End Region

#Region " RANGE MODE "

    ''' <summary> The range settling time in milli-seconds. </summary>
    Public Const RangeSettlingTimeMilliseconds As Integer = 250

    ''' <summary> The range mode. </summary>
    Private _RangeMode As RangeMode?

    ''' <summary> Gets or sets the cached source RangeMode. </summary>
    ''' <value> The <see cref="RangeMode">Range Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property RangeMode As RangeMode?
        Get
            Return Me._RangeMode
        End Get
        Protected Set(ByVal value As RangeMode?)
            If Not Nullable.Equals(Me.RangeMode, value) Then
                Me._RangeMode = value
                Me.SafePostPropertyChanged(NameOf(Me.RangeMode))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Range Mode. </summary>
    ''' <param name="value"> The  Range Mode. </param>
    ''' <returns> The <see cref="RangeMode">Range Mode</see> or none if unknown. </returns>
    Public Function ApplyRangeMode(ByVal value As RangeMode) As RangeMode?
        Me.WriteRangeMode(value)
        Return Me.QueryRangeMode()
    End Function

    ''' <summary> Queries the Range Mode. </summary>
    ''' <returns> The <see cref="RangeMode">Range Mode</see> or none if unknown. </returns>
    Public Function QueryRangeMode() As RangeMode?
        Dim mode As String = Me.RangeMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryTrimEnd("U0x")
        If Not String.IsNullOrWhiteSpace(mode) Then
            ' 012345678901234567890123
            ' C0D111F0M63P0R05S0T0B0Y0
            mode = mode.Substring(13, 3)
        End If
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = "Failed fetching Range Mode"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.RangeMode = Tegam.RangeMode.R0
        Else
            If mode.Substring(1, 1) = "0" Then
                mode = mode.Substring(0, 1) & mode.Substring(2, 1)
            End If
            Me.RangeMode = CType([Enum].Parse(GetType(RangeMode), mode), RangeMode)
        End If
        Return Me.RangeMode
    End Function

    ''' <summary> Writes the Range Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The range mode. </param>
    ''' <returns> The <see cref="RangeMode">Range Mode</see> or none if unknown. </returns>
    Public Function WriteRangeMode(ByVal value As RangeMode) As RangeMode?
        Me.Session.WriteLine(value.ExtractBetween() & "x")
        Me.RangeMode = value
        Return Me.RangeMode
    End Function

#End Region

#Region " TRIGGER DELAY "

    ''' <summary> The TriggerDelay. </summary>
    Private _TriggerDelay As TimeSpan?

    ''' <summary> Gets or sets the cached Trigger Delay. </summary>
    ''' <remarks> The TriggerDelay is used to TriggerDelay operation in the trigger layer. After the programmed
    ''' trigger event occurs, the instrument waits until the TriggerDelay period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Trigger Delay or none if not set or unknown. </value>
    Public Overloads Property TriggerDelay As TimeSpan?
        Get
            Return Me._TriggerDelay
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Nullable.Equals(Me.TriggerDelay, value) Then
                Me._TriggerDelay = value
                Me.SafePostPropertyChanged(NameOf(Me.TriggerDelay))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Delay. </summary>
    ''' <param name="value"> The current TriggerDelay. </param>
    ''' <returns> The Trigger Delay or none if unknown. </returns>
    Public Function ApplyTriggerDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteTriggerDelay(value)
        Return Me.QueryTriggerDelay()
    End Function

    ''' <summary> Queries the TriggerDelay. </summary>
    ''' <returns> The TriggerDelay or none if unknown. </returns>
    Public Function QueryTriggerDelay() As TimeSpan?
        Dim reading As String = CInt(Me.TriggerDelay.GetValueOrDefault(TimeSpan.FromMilliseconds(111)).TotalMilliseconds).ToString
        Me.Session.MakeEmulatedReplyIfEmpty(reading)
        reading = Me.Session.QueryTrimEnd("U0x")
        If Not String.IsNullOrWhiteSpace(reading) Then
            ' 012345678901234567890123
            ' C0D111F0M63P0R05S0T0B0Y0
            reading = reading.Substring(2, 3)
        End If
        Dim value As Integer
        If Integer.TryParse(reading, value) Then
            Me.TriggerDelay = TimeSpan.FromMilliseconds(value)
        Else
            Me.TriggerDelay = New TimeSpan?
        End If
        Return Me.TriggerDelay
    End Function

    ''' <summary> Writes the Trigger Delay without reading back the value from the device. </summary>
    ''' <param name="value"> The current TriggerDelay. </param>
    ''' <returns> The Trigger Delay or none if unknown. </returns>
    Public Function WriteTriggerDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.Session.WriteLine("D{0:000}x", value.TotalMilliseconds)
        Me.TriggerDelay = value
        Return Me.TriggerDelay
    End Function

#End Region

#Region " TRIGGER MODE "

    ''' <summary> The trigger mode. </summary>
    Private _TriggerMode As TriggerMode?

    ''' <summary> Gets or sets the cached source TriggerMode. </summary>
    ''' <value> The <see cref="TriggerMode">Trigger Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property TriggerMode As TriggerMode?
        Get
            Return Me._TriggerMode
        End Get
        Protected Set(ByVal value As TriggerMode?)
            If Not Nullable.Equals(Me.TriggerMode, value) Then
                Me._TriggerMode = value
                Me.SafePostPropertyChanged(NameOf(Me.TriggerMode))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trigger Mode. </summary>
    ''' <param name="value"> The  Trigger Mode. </param>
    ''' <returns> The <see cref="TriggerMode">Trigger Mode</see> or none if unknown. </returns>
    Public Function ApplyTriggerMode(ByVal value As TriggerMode) As TriggerMode?
        Me.WriteTriggerMode(value)
        Return Me.QueryTriggerMode()
    End Function

    ''' <summary> Queries the Trigger Mode. </summary>
    ''' <returns> The <see cref="TriggerMode">Trigger Mode</see> or none if unknown. </returns>
    Public Function QueryTriggerMode() As TriggerMode?
        Dim mode As String = Me.TriggerMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryTrimEnd("U0x")
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = "Failed fetching Trigger Mode"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.TriggerMode = Tegam.TriggerMode.T2
        Else
            ' 0         1         2
            ' 012345678901234567890123
            ' C0D111F0M63P0R05S0T0B0Y0
            mode = mode.Substring(18, 2)
            Me.TriggerMode = CType([Enum].Parse(GetType(TriggerMode), mode), TriggerMode)
        End If
        Return Me.TriggerMode
    End Function

    ''' <summary> Writes the Trigger Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The trigger mode. </param>
    ''' <returns> The <see cref="TriggerMode">Trigger Mode</see> or none if unknown. </returns>
    Public Function WriteTriggerMode(ByVal value As TriggerMode) As TriggerMode?
        Me.Session.WriteLine(value.ExtractBetween() & "x")
        Me.TriggerMode = value
        Return Me.TriggerMode
    End Function

#End Region

#Region " MEASURE SEQUENCE "

    Private _InitialDelay As TimeSpan

    ''' <summary> Gets or sets the initial delay. </summary>
    ''' <value> The initial delay. </value>
    Public Property InitialDelay As TimeSpan
        Get
            Return Me._InitialDelay
        End Get
        Set(value As TimeSpan)
            If Not value.Equals(Me.InitialDelay) Then
                Me._InitialDelay = value
                Me.SafePostPropertyChanged(NameOf(Me.InitialDelay))
            End If
        End Set
    End Property

    Private _MeasurementDelay As TimeSpan

    ''' <summary> Gets or sets the delay between successive measurements. </summary>
    ''' <value> The Measurement delay. </value>
    Public Property MeasurementDelay As TimeSpan
        Get
            Return Me._MeasurementDelay
        End Get
        Set(value As TimeSpan)
            If Not value.Equals(Me.MeasurementDelay) Then
                Me._MeasurementDelay = value
                Me.SafePostPropertyChanged(NameOf(Me.MeasurementDelay))
            End If
        End Set
    End Property

    Private _MaximumTrialsCount As Integer

    ''' <summary> Gets or sets the maximum number trials. </summary>
    ''' <value> The number of Maximum measurements. </value>
    Public Property MaximumTrialsCount As Integer
        Get
            Return Me._MaximumTrialsCount
        End Get
        Set(value As Integer)
            If Not value.Equals(Me.MaximumTrialsCount) Then
                Me._MaximumTrialsCount = value
                Me.SafePostPropertyChanged(NameOf(Me.MaximumTrialsCount))
            End If
        End Set
    End Property

    Private _MaximumDifference As Double

    ''' <summary> Gets or sets the maximum allowed difference between successive measurements. </summary>
    ''' <value> The maximum difference. </value>
    Public Property MaximumDifference As Double
        Get
            Return Me._MaximumDifference
        End Get
        Set(value As Double)
            If Not value.Equals(Me._MaximumDifference) Then
                Me._MaximumDifference = value
                Me.SafePostPropertyChanged(NameOf(Me.MaximumDifference))
            End If
        End Set
    End Property

    ''' <summary> Are measurements done. </summary>
    ''' <param name="values">     The values. </param>
    ''' <param name="finalValue"> [in,out] The final value. </param>
    ''' <returns> <c>True</c> if measurement accepted or maximum count is done. </returns>
    Private Function AreMeasurementsDone(ByVal values As List(Of Double?), ByRef finalValue As Double?) As Boolean
        If values Is Nothing Then
            Return False
        ElseIf values.Count >= 2 AndAlso values(values.Count - 1).HasValue AndAlso values(values.Count - 2).HasValue Then
            If Math.Abs(values(values.Count - 1).Value - values(values.Count - 2).Value) <= Me.MaximumDifference Then
                finalValue = values(values.Count - 1)
                Return True
            ElseIf values.Count >= Me.MaximumTrialsCount Then
                finalValue = New Double?
                Return True
            Else
                Return False
            End If
        ElseIf values.Count >= Me.MaximumTrialsCount Then
            finalValue = New Double?
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary> Waits. </summary>
    ''' <param name="value">     The value. </param>
    ''' <param name="sleepTime"> The sleep time. </param>
    Private Shared Sub Wait(ByVal value As TimeSpan, ByVal sleepTime As TimeSpan)
        Dim finalTime As DateTime = DateTime.Now.Add(value)
        Do
            Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(sleepTime)
        Loop Until DateTime.Now > finalTime
    End Sub

    ''' <summary> Gets or sets the emulated resistance average. </summary>
    ''' <value> The emulated resistance average. </value>
    Public Property EmulatedResistanceAverage As Double = 1

    ''' <summary> Gets or sets the emulated resistance range. </summary>
    ''' <value> The emulated resistance range. </value>
    Public Property EmulatedResistanceRange As Double = 0.1

    ''' <summary> Measures a resistance using the measurement sequence. </summary>
    Public Sub Measure()

        Dim measurements As New List(Of Double?)
        Dim finalValue As Double? = New Double?
        Dim delayTime As TimeSpan = Me.InitialDelay
        Do
            Me.Session.EmulatedStatusByte = VI.ServiceRequests.MeasurementEvent
            Me.StatusSubsystem.ReadRegisters()
            Windows.Forms.Application.DoEvents()
            If Not Me.StatusSubsystem.ErrorAvailable Then
                MeasureSubsystem.wait(delayTime, TimeSpan.FromMilliseconds(10))
                delayTime = Me.MeasurementDelay
                Me.Session.MakeEmulatedReply(Me.NewReading(Me.EmulatedResistanceAverage, Me.EmulatedResistanceRange))
                Me.LastReading = Me.Session.ReadLineTrimEnd
                Windows.Forms.Application.DoEvents()
                If String.IsNullOrWhiteSpace(Me.LastReading) Then
                    Me.OverRangeOpenWire = False
                    Me.Resistance = New Double?
                    Windows.Forms.Application.DoEvents()
                Else
                    ' the emulator will set the last reading. 
                    Me.ParseReading(Me.LastReading)
                    Windows.Forms.Application.DoEvents()
                End If
                measurements.Add(Me.Resistance)
            End If
        Loop Until Me.StatusSubsystem.ErrorAvailable OrElse Me.AreMeasurementsDone(measurements, finalValue)
        Me.Resistance = finalValue
        Windows.Forms.Application.DoEvents()
        Me.MeasurementAvailable = True
        ' This caused cross-thread exception:
        ' me.AsyncMeasurementAvailable(True)
        Windows.Forms.Application.DoEvents()
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    Protected Overrides ReadOnly Property InitiateCommand As String = ""
#End If
#End Region
