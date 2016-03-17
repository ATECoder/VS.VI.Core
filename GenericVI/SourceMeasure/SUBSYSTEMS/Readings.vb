Imports isr.VI
''' <summary> Holds a single set of 27xx instrument reading elements. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2008" by="David" revision="2.0.2936.x"> Create based on the 24xx
''' system classes. </history>
Public Class Readings
    Inherits isr.VI.ReadingAmounts

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Constructs this class. </summary>
    Public Sub New()

        ' instantiate the base class
        MyBase.New()

        Me._VoltageReading = New isr.VI.MeasuredAmount()
        With Me._VoltageReading
            .Unit = Arebis.StandardUnits.ElectricUnits.Volt
            .ComplianceLimit = VI.Scpi.Syntax.Infinity
            .HighLimit = VI.Scpi.Syntax.Infinity
            .LowLimit = VI.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 13
        End With

        Me._CurrentReading = New isr.VI.MeasuredAmount()
        With Me._CurrentReading
            .Unit = Arebis.StandardUnits.ElectricUnits.Ampere
            .ComplianceLimit = VI.Scpi.Syntax.Infinity
            .HighLimit = VI.Scpi.Syntax.Infinity
            .LowLimit = VI.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 13
        End With

        Me._ResistanceReading = New isr.VI.MeasuredAmount()
        With Me._ResistanceReading
            .Unit = Arebis.StandardUnits.ElectricUnits.Ohm
            .ComplianceLimit = VI.Scpi.Syntax.Infinity
            .HighLimit = VI.Scpi.Syntax.Infinity
            .LowLimit = VI.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 13
        End With

        Me._Timestamp = New isr.VI.ReadingAmount()
        With Me._Timestamp
            .Unit = Arebis.StandardUnits.TimeUnits.Second
            .ReadingLength = 13
        End With

        Me._StatusReading = New isr.VI.ReadingStatus()
        With Me._StatusReading
            .ReadingLength = 13
        End With

    End Sub

    ''' <summary> Create a copy of the model. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As Readings)

        ' instantiate the base class
        MyBase.New()

        If model IsNot Nothing Then
            Me._CurrentReading = New isr.VI.MeasuredAmount(model.CurrentReading)
            Me._ResistanceReading = New isr.VI.MeasuredAmount(model.ResistanceReading)
            Me._VoltageReading = New isr.VI.MeasuredAmount(model.VoltageReading)
            Me._Timestamp = New isr.VI.ReadingAmount(model.Timestamp)
            Me._StatusReading = New isr.VI.ReadingStatus(model.StatusReading)
            Me.Elements = model.Elements
        End If

    End Sub

    ''' <summary> Clones this class. </summary>
    ''' <returns> A copy of this object. </returns>
    Public Function Clone() As Readings
        Return New Readings(Me)
    End Function

#End Region

#Region " PARSE "

    Public Overrides Function TryParse(ByVal readings As String(), ByVal firstElementIndex As Integer) As Boolean
        Dim affirmative As Boolean = MyBase.TryParse(readings, firstElementIndex)
        If affirmative Then
            Dim statusValue As Long = Me.StatusReading.StatusValue.GetValueOrDefault(0)
            ' add the status to each meta status value.
            Me.VoltageReading.MetaStatus.Preset(Me.VoltageReading.MetaStatus.Value Or statusValue)
            Me.CurrentReading.MetaStatus.Preset(Me.CurrentReading.MetaStatus.Value Or statusValue)
            Me.ResistanceReading.MetaStatus.Preset(Me.ResistanceReading.MetaStatus.Value Or statusValue)
            If statusValue > 0 Then
                ' update the meta status based on the status reading.
                If Me.StatusReading.IsBit(StatusWordBit.FailedContactCheck) Then
                    Me.VoltageReading.MetaStatus.FailedContactCheck = True
                    Me.CurrentReading.MetaStatus.FailedContactCheck = True
                    Me.ResistanceReading.MetaStatus.FailedContactCheck = True
                End If
                If Me.StatusReading.IsBit(StatusWordBit.HitCompliance) Then
                    Me.VoltageReading.MetaStatus.HitStatusCompliance = True
                    Me.CurrentReading.MetaStatus.HitStatusCompliance = True
                    Me.ResistanceReading.MetaStatus.HitStatusCompliance = True
                End If
                If Me.StatusReading.IsBit(StatusWordBit.HitRangeCompliance) Then
                    Me.VoltageReading.MetaStatus.HitRangeCompliance = True
                    Me.CurrentReading.MetaStatus.HitRangeCompliance = True
                    Me.ResistanceReading.MetaStatus.HitRangeCompliance = True
                End If
                If Me.StatusReading.IsBit(StatusWordBit.HitVoltageProtection) Then
                    Me.VoltageReading.MetaStatus.HitVoltageProtection = True
                    Me.CurrentReading.MetaStatus.HitVoltageProtection = True
                    Me.ResistanceReading.MetaStatus.HitVoltageProtection = True
                End If
                If Me.StatusReading.IsBit(StatusWordBit.OverRange) Then
                    Me.VoltageReading.MetaStatus.HitVoltageProtection = True
                    Me.CurrentReading.MetaStatus.HitVoltageProtection = True
                    Me.ResistanceReading.MetaStatus.HitVoltageProtection = True
                End If
            End If
        End If

        Return affirmative
    End Function

    ''' <summary> Parses reading data into a readings array. </summary>
    ''' <param name="baseReading">    Specifies the base reading which includes the limits for all
    ''' reading elements. </param>
    ''' <param name="readingRecords"> The reading records. </param>
    ''' <returns> Readings[][]. </returns>
    ''' <exception cref="System.ArgumentNullException"> readingRecords, baseReading. </exception>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification:="Object is returned")>
    <CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Multi")>
    Public Shared Function ParseMulti(ByVal baseReading As Readings, ByVal readingRecords As String) As Readings()

        If readingRecords Is Nothing Then
            Throw New ArgumentNullException(NameOf(readingRecords))
        ElseIf readingRecords.Length = 0 Then
            Dim r As Readings() = {}
            Return r
        ElseIf baseReading Is Nothing Then
        Throw New ArgumentNullException(NameOf(baseReading))
        End If

        Dim readings As String() = readingRecords.Split(","c)
        If readings.Length < baseReading.ElementsCount Then
            Dim r As Readings() = {}
            Return r
        End If

        Dim readingsArray(readings.Length \ baseReading.ElementsCount - 1) As Readings
        Dim j As Integer = 0
        For i As Integer = 0 To readings.Length - 1 Step baseReading.ElementsCount
            Dim reading As New Readings(baseReading)
            reading.TryParse(readings, i)
            readingsArray(j) = reading
            j += 1
        Next
        Return readingsArray

    End Function

#End Region

#Region " UNITS "

    Private _unitSuffixes As List(Of String)
    ''' <summary> Unit suffixes. </summary>
    ''' <returns> A list of unit suffixes. </returns>
    Protected Overrides Function UnitSuffixes() As ObjectModel.ReadOnlyCollection(Of String)
        If Me._unitSuffixes Is Nothing Then
            _unitSuffixes = New List(Of String)
            Dim l As List(Of String) = _unitSuffixes
            l.Add("ADC")
            l.Add("OHM")
            l.Add("OHM4W")
            l.Add("VDC")
            l.Add("SECS")
            l.Add("RDNG#")
            l = Nothing
        End If
        Return New ObjectModel.ReadOnlyCollection(Of String)(_unitSuffixes)
    End Function

    Private Shared _UnitsDictionary As Dictionary(Of String, Arebis.TypedUnits.Unit)
    ''' <summary> Returns the Unit Parser hash. </summary>
    ''' <returns> A Dictionary for translating SCPI unit names to <see cref="Arebis.StandardUnits">standard units</see>. </returns>
    Protected Overrides Function UnitsDictionary() As Dictionary(Of String, Arebis.TypedUnits.Unit)
        If _UnitsDictionary Is Nothing Then
            _UnitsDictionary = New Dictionary(Of String, Arebis.TypedUnits.Unit)
            Dim dix3 As Dictionary(Of String, Arebis.TypedUnits.Unit) = _UnitsDictionary
            dix3.Add("ADC", Arebis.StandardUnits.ElectricUnits.Ampere)
            dix3.Add("OHM", Arebis.StandardUnits.ElectricUnits.Ohm)
            dix3.Add("OHM4W", Arebis.StandardUnits.ElectricUnits.Ohm)
            dix3.Add("VDC", Arebis.StandardUnits.ElectricUnits.Volt)
            dix3.Add("SECS", Arebis.StandardUnits.TimeUnits.Second)
            dix3.Add("RDNG#", Arebis.StandardUnits.UnitlessUnits.Count)
            dix3 = Nothing
        End If
        Return _UnitsDictionary
    End Function

#End Region

#Region " READING ELEMENTS "

    Private _elements As isr.VI.ReadingElements

    ''' <summary> Gets or sets the reading elements. </summary>
    ''' <value> The elements. </value>
    ''' <remarks> Adds reading elements in the order they are returned by the instrument so as to
    ''' automate parsing of these data. </remarks>
    Public Property Elements() As isr.VI.ReadingElements
        Get
            Return Me._elements
        End Get
        Set(ByVal value As isr.VI.ReadingElements)
            If Not value.Equals(Me.Elements) Then
                Me._elements = value
                Me.Readings = New ReadingElementCollection
                If (value And isr.VI.ReadingElements.Voltage) <> 0 Then MyBase.AddReading(Me.VoltageReading)
                If (value And isr.VI.ReadingElements.Current) <> 0 Then MyBase.AddReading(Me.CurrentReading)
                If (value And isr.VI.ReadingElements.Resistance) <> 0 Then MyBase.AddReading(Me.ResistanceReading)
                If (value And isr.VI.ReadingElements.Time) <> 0 Then MyBase.AddReading(Me.Timestamp)
                If (value And isr.VI.ReadingElements.Status) <> 0 Then MyBase.AddReading(Me.StatusReading)
                ' Units is a property of each element. If units are turned on, each element units is enabled.
                If (value And isr.VI.ReadingElements.Units) <> 0 Then
                    For Each e As ReadingElement In Me.Readings
                        e.IncludesUnitsSuffix = True
                    Next
                End If
            End If
        End Set
    End Property

    ''' <summary>Gets or sets the source meter <see cref="isr.VI.MeasuredAmount">current reading</see>.</summary>
    Public Property CurrentReading() As isr.VI.MeasuredAmount

    ''' <summary>Gets or sets the source meter <see cref="isr.VI.MeasuredAmount">resistance reading</see>.</summary>
    Public Property ResistanceReading() As isr.VI.MeasuredAmount

    ''' <summary>Gets or sets the source meter <see cref="isr.VI.MeasuredAmount">voltage reading</see>.</summary>
    Public Property VoltageReading() As isr.VI.MeasuredAmount

    ''' <summary>Gets or sets the timestamp <see cref="isr.VI.ReadingAmount">reading</see>.</summary>
    Public Property Timestamp() As isr.VI.ReadingAmount

    ''' <summary>Gets or sets the source meter <see cref="isr.VI.MeasuredAmount">status reading</see>.</summary>
    Public Property StatusReading() As isr.VI.ReadingStatus

    ''' <summary> Select measured amount. </summary>
    ''' <remarks> David, 3/8/2016. </remarks>
    ''' <param name="element"> The element. </param>
    ''' <returns> A MeasuredAmount. </returns>
    Public Function SelectMeasuredAmount(ByVal element As ReadingElements) As MeasuredAmount
        Dim reading As MeasuredAmount = Nothing
        Select Case element
            Case ReadingElements.Current
                reading = Me.CurrentReading
            Case ReadingElements.Resistance
                reading = Me.ResistanceReading
            Case ReadingElements.Voltage
                reading = Me.VoltageReading
        End Select
        Return reading
    End Function

    ''' <summary> Convert this object into a string representation. </summary>
    ''' <param name="element"> The element. </param>
    ''' <returns> Returns the reading value for the selected element. </returns>
    Public Overloads Function ToString(ByVal element As ReadingElements) As String

        Dim value As String = ""
        Dim reading As MeasuredAmount = Me.SelectMeasuredAmount(element)
        If reading Is Nothing Then
            Select Case element
                Case ReadingElements.Time
                    If Me.Timestamp IsNot Nothing AndAlso Me.Timestamp.Amount IsNot Nothing Then
                        value = Timestamp.Amount.ToString()
                    End If
                Case ReadingElements.Status
                    value = Me.StatusReading?.ValueReading
            End Select
        ElseIf reading.Amount IsNot Nothing Then
            value = reading.Amount.ToString()
        End If
        Return value

    End Function

#End Region

End Class
