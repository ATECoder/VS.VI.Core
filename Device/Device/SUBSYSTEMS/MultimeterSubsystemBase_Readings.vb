
Partial Public Class MultimeterSubsystemBase

#Region " MEASURED AMOUNT "

    ''' <summary> Gets or sets the amount. </summary>
    ''' <value> The amount. </value>
    Public ReadOnly Property Amount As MeasuredAmount

    Private _LastReading As String
    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property LastReading As String
        Get
            Return Me._LastReading
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.LastReading, StringComparison.OrdinalIgnoreCase) Then
                Me._LastReading = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _ReadingCaption As String

    ''' <summary> Gets or sets the reading caption. </summary>
    ''' <value> The reading caption. </value>
    Public Property ReadingCaption As String
        Get
            Return Me._ReadingCaption
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.ReadingCaption, StringComparison.OrdinalIgnoreCase) Then
                Me._ReadingCaption = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _FailureShortDescription As String

    ''' <summary> Gets or sets the failure Short Description. </summary>
    ''' <value> The failure Short Description. </value>
    Public Property FailureShortDescription As String
        Get
            Return Me._FailureShortDescription
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.FailureShortDescription, StringComparison.OrdinalIgnoreCase) Then
                Me._FailureShortDescription = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _FailureCode As String

    ''' <summary> Gets or sets the failure Code. </summary>
    ''' <value> The failure Code. </value>
    Public Property FailureCode As String
        Get
            Return Me._FailureCode
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.FailureCode, StringComparison.OrdinalIgnoreCase) Then
                Me._FailureCode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _FailureLongDescription As String

    ''' <summary> Gets or sets the failure long description. </summary>
    ''' <value> The failure long description. </value>
    Public Property FailureLongDescription As String
        Get
            Return Me._FailureLongDescription
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.FailureLongDescription, StringComparison.OrdinalIgnoreCase) Then
                Me._FailureLongDescription = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Safe post readings. </summary>
    Public Sub SafePostReadings()
        Me.SafePostPropertyChanged(NameOf(ChannelMarkerSubsystemBase.ReadingCaption))
        Me.SafePostPropertyChanged(NameOf(ChannelMarkerSubsystemBase.FailureLongDescription))
        Me.SafePostPropertyChanged(NameOf(ChannelMarkerSubsystemBase.FailureShortDescription))
        Me.SafePostPropertyChanged(NameOf(ChannelMarkerSubsystemBase.FailureCode))
        Me.SafePostPropertyChanged(NameOf(ChannelMarkerSubsystemBase.LastReading))
    End Sub

    ''' <summary> Safe send readings. </summary>
    Public Sub SafeSendReadings()
        Me.SafeSendPropertyChanged(NameOf(ChannelMarkerSubsystemBase.ReadingCaption))
        Me.SafeSendPropertyChanged(NameOf(ChannelMarkerSubsystemBase.FailureLongDescription))
        Me.SafeSendPropertyChanged(NameOf(ChannelMarkerSubsystemBase.FailureShortDescription))
        Me.SafeSendPropertyChanged(NameOf(ChannelMarkerSubsystemBase.FailureCode))
        Me.SafeSendPropertyChanged(NameOf(ChannelMarkerSubsystemBase.LastReading))
    End Sub

#End Region

#Region " PARSE META STATUS "

    ''' <summary> Parse meta status. </summary>
    ''' <param name="metaStatus"> The meta status. </param>
    Private Sub ParseMetaStatus(ByVal metaStatus As MetaStatus)
        Const clear As String = ""
        Dim failureCode As String = clear
        Dim failureShortDescription As String = clear
        Dim failureLongDescription As String = clear
        If metaStatus.HasValue Then
            failureCode = $"{metaStatus.TwoCharDescription(""),2}"
            failureShortDescription = $"{metaStatus.ToShortDescription(""),4}"
            failureLongDescription = metaStatus.ToLongDescription("")
        End If
        Me.FailureLongDescription = failureLongDescription
        Me.FailureShortDescription = failureShortDescription
        Me.FailureCode = failureCode
    End Sub

#End Region

#Region " PARSE VALUE "

    ''' <summary> Parse measured value. </summary>
    Private Sub ParseMeasuredValue(ByVal reading As String)
        Const clear As String = ""
        Dim caption As String = clear
        If String.IsNullOrWhiteSpace(reading) Then
            caption = ($"-.---- {Me.Amount.Unit.ToString}")
        Else
            caption = $"{Me.Amount.ToString} {Me.Amount.Unit.ToString}"
        End If
        Me.ReadingCaption = caption
        Me.FailureCode = clear
        Me.FailureShortDescription = clear
        Me.FailureLongDescription = clear
        Me.LastReading = reading
    End Sub

    ''' <summary> Parse measured amount. </summary>
    Private Sub ParseMeasuredAmount(ByVal reading As String)
        Const clear As String = ""
        Dim caption As String = clear
        If String.IsNullOrWhiteSpace(reading) Then
            caption = ($"-.---- {Me.Amount.Unit.ToString}")
        Else
            caption = $"{Me.Amount.ToString} {Me.Amount.Unit.ToString}"
            Me.ParseMetaStatus(Me.Amount.MetaStatus)
        End If
        If String.IsNullOrEmpty(Me.FailureLongDescription) Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.StatusSubsystem.ResourceTitle}={caption}")
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, FailureLongDescription)
        End If
        Me.ReadingCaption = caption
        Me.LastReading = reading
    End Sub

#End Region

#Region " READING AMOUNTS "

    ''' <summary> Parse the active reading. </summary>
    ''' <param name="readingAmounts"> The readings. </param>
    ''' <returns> A Double? </returns>
    Protected Overridable Function ParseActiveReading(ByVal readingAmounts As ReadingAmounts) As Double?
        If readingAmounts Is Nothing Then Throw New ArgumentNullException(NameOf(readingAmounts))
        Const clear As String = ""
        Dim reading As String = clear
        Dim caption As String = clear
        Dim value As New Double?
        If readingAmounts Is Nothing OrElse readingAmounts.ActiveReadingType = ReadingTypes.None Then
            caption = $"-.---- {Me.Amount.Unit.ToString}"
        ElseIf readingAmounts.IsEmpty Then
            caption = readingAmounts.ActiveAmountCaption
        Else
            value = readingAmounts.ActiveReadingAmount.Value
            caption = readingAmounts.ActiveAmountCaption
            reading = readingAmounts.ActiveReadingAmount.ValueReading
            Me.ParseMetaStatus(readingAmounts.ActiveMetaStatus)
        End If
        If String.IsNullOrEmpty(Me.FailureLongDescription) Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{Me.StatusSubsystem.ResourceTitle}={caption}")
        Else
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, FailureLongDescription)
        End If
        Me.ReadingCaption = caption
        Me.LastReading = reading
        Return value
    End Function

    ''' <summary> Gets or sets the reading amounts. </summary>
    ''' <value> The reading amounts. </value>
    Public ReadOnly Property ReadingAmounts As ReadingAmounts

    ''' <summary> Assign reading amounts. </summary>
    ''' <param name="readingAmounts"> The reading amounts. </param>
    Protected Sub AssignReadingAmounts(ByVal readingAmounts As ReadingAmounts)
        Me._ReadingAmounts = readingAmounts
    End Sub

    ''' <summary> Select active reading. </summary>
    ''' <param name="readingType"> Type of the reading. </param>
    Public Sub SelectActiveReading(ByVal readingType As ReadingTypes)
        Me.ReadingAmounts.ActiveReadingType = readingType
        Me.ParseActiveReading(Me.ReadingAmounts)
    End Sub

    ''' <summary> Parses a new set of reading elements. </summary>
    ''' <param name="reading"> Specifies the measurement text to parse into the new reading. </param>
    Protected Function ParseReadingAmounts(ByVal reading As String) As Double?
        Dim result As New Double?
        ' check if we have units suffixes.
        If (Me.ReadingAmounts.Elements And isr.VI.ReadingTypes.Units) <> 0 Then reading = ReadingEntity.TrimUnits(reading)
        If Me.ReadingAmounts.TryParse(reading) Then
            result = Me.ParseActiveReading(Me.ReadingAmounts)
        End If
        Return result
    End Function

#End Region

#Region " PARSE READING "

    ''' <summary> The Measured Value. </summary>
    Private _MeasuredValue As Double?

    ''' <summary> Gets or sets the cached Measured Value. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property MeasuredValue As Double?
        Get
            Return Me._MeasuredValue
        End Get
        Protected Set(ByVal value As Double?)
            Me._MeasuredValue = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Parses the reading into the data elements. </summary>
    Public Overridable Function ParseReading(ByVal reading As String) As Double?
        If Me.Amount.TryApplyReading(reading) Then
            Me.MeasuredValue = Me.Amount.Amount.Value
            Me.SafePostPropertyChanged(NameOf(MeasureSubsystemBase.Amount))
            Me.ParseMeasuredAmount(reading)
        Else
            Dim result As Double = 0
            If Double.TryParse(reading, result) Then
                Me.MeasuredValue = result
                Me.Amount.Value = result
            Else
                Me.MeasuredValue = New Double?
                Me.Amount.Value = New Double?
            End If
            Me.ParseMeasuredValue(reading)
        End If
        Return Me.MeasuredValue
    End Function

#End Region

#Region " MEASURE "

    ''' <summary> Reads a value and converts it to Double. </summary>
    ''' <returns> The measured value or none if unknown. </returns>
    Public Overridable Function MeasureValue(ByVal queryCommand As String) As Double?
        Dim value As String = Me.Query(Me.Session.EmulatedReply, queryCommand)
        Dim result As Double = 0
        If Double.TryParse(value, result) Then
            Me.MeasuredValue = result
            Me.Amount.Value = result
        Else
            Me.MeasuredValue = New Double?
            Me.Amount.Value = New Double?
        End If
        Return Me.MeasuredValue
    End Function

    ''' <summary> Queries The reading. </summary>
    ''' <returns> The reading or none if unknown. </returns>
    Public Overridable Function Measure(ByVal queryCommand As String) As Double?
        Dim value As String = Me.Query(Me.Session.EmulatedReply, queryCommand)
        ' the emulator will set the last reading. 
        Return Me.ParseReading(value)
    End Function

#End Region


End Class

