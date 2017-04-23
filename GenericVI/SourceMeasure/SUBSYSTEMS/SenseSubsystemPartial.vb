Partial Public Class SenseSubsystem

#Region " CONCURRENT SENSE FUNCTION MODE "

    ''' <summary> Queries the Concurrent Function Mode Enabled sentinel. Also sets the 
    '''           <see cref="ConcurrentSenseEnabled">mode</see> sentinel. </summary>
    ''' <returns> <c>null</c> if mode is not known; <c>True</c> if concurrent; otherwise, <c>False</c>. </returns>
    Public Overrides Function QueryConcurrentSenseEnabled() As Boolean?
        Me.ConcurrentSenseEnabled = Me.Session.Query(Me.ConcurrentSenseEnabled.GetValueOrDefault(True), ":SENS:FUNC:CONC?")
        Return Me.ConcurrentSenseEnabled
    End Function

    ''' <summary> Writes the Concurrent Function Mode  Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is concurrent. </param>
    ''' <returns> <c>null</c> if mode is not known; <c>True</c> if concurrent; otherwise, <c>False</c>. </returns>
    Public Overrides Function WriteConcurrentSenseEnabled(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine(":SENS:FUNC:CONC {0:'ON';'ON';'OFF'}", CType(value, Integer))
        Me.ConcurrentSenseEnabled = value
        Return Me.ConcurrentSenseEnabled
    End Function

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Queries the Sense Function Modes. Also sets the <see cref="FunctionModes"></see> cached value. </summary>
    ''' <returns> The Sense Function Mode or null if unknown. </returns>
    Public Overrides Function QueryFunctionModes() As VI.Scpi.SenseFunctionModes?
        ' the instrument expects single quotes when writing the value but sends back items delimited with double quotes.
        Me.FunctionModes = SenseSubsystemBase.ParseSenseFunctionModes(Me.Session.QueryTrimEnd(":SENS:FUNC?").Replace(CChar(""""), "'"c))
        Return Me.FunctionModes
    End Function

    ''' <summary> Writes the Sense Function Mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The Sense Function Mode or null if unknown. </returns>
    Public Overrides Function WriteFunctionModes(ByVal value As VI.Scpi.SenseFunctionModes) As VI.Scpi.SenseFunctionModes?
        Me.Session.WriteLine(":SENS:FUNC {0}", SenseSubsystemBase.BuildRecord(value))
        Me.FunctionModes = value
        Return Me.FunctionModes
    End Function

#End Region

#Region " LATEST DATA "

    ''' <summary> Fetches the latest data and parses it. </summary>
    ''' <remarks> Issues the ':SENSE:DATA:LAT?' query, which reads data stored in the Sample Buffer.. </remarks>
    Public Overrides Sub FetchLatestData()
        Me.LastReading = Me.Session.QueryTrimEnd(":SENSE:DATA:LAT?")
        If Not String.IsNullOrWhiteSpace(Me.LastReading) Then
            ' the emulator will set the last reading. 
            Me.ParseReading(Me.LastReading)
            MyBase.MeasurementAvailable = True
        End If
    End Sub

#End Region

#Region " RANGE "

    ''' <summary> Queries the current Range. </summary>
    ''' <returns> The current Range or none if unknown. </returns>
    Public Overrides Function QueryRange() As Double?
        Me.Range = Me.Session.Query(Me.Range.GetValueOrDefault(0.105), ":SOUR:RANG?")
        Return Me.Range
    End Function

    ''' <summary> Writes the sense current Range without reading back the value from the device. </summary>
    ''' <remarks> This command sets the current Range. The value is in Amperes. 
    '''           At *RST, the range is auto and the value is not known. </remarks>
    ''' <param name="value"> The sense current Range. </param>
    ''' <returns> The sense Current Range. </returns>
    Public Overrides Function WriteRange(ByVal value As Double) As Double?
        If value >= (isr.VI.Scpi.Syntax.Infinity - 1) Then
            Me.Session.WriteLine(":SENS:RANG MAX")
            value = isr.VI.Scpi.Syntax.Infinity
        ElseIf value <= (isr.VI.Scpi.Syntax.NegativeInfinity + 1) Then
            Me.Session.WriteLine(":SENS:RANG MIN")
            value = isr.VI.Scpi.Syntax.NegativeInfinity
        Else
            Me.Session.WriteLine(":SENS:RANG {0}", value)
        End If
        Me.Range = value
        Return Me.Range
    End Function

#End Region

End Class
