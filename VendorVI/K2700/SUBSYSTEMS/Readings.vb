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

        Me._Reading = New isr.VI.MeasuredAmount()
        With Me._Reading
            .Unit = Arebis.StandardUnits.ElectricUnits.Ampere
            .ComplianceLimit = VI.Scpi.Syntax.Infinity
            .HighLimit = VI.Scpi.Syntax.Infinity
            .LowLimit = VI.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 15
        End With

        Me._Timestamp = New isr.VI.ReadingAmount()
        With Me._Timestamp
            .Unit = Arebis.StandardUnits.TimeUnits.Second
            .ReadingLength = 7
        End With

        Me._ReadingNumber = New isr.VI.ReadingValue()
        With Me._ReadingNumber
            .ReadingLength = 4
        End With

        Me._Channel = New isr.VI.ReadingValue()
        With Me._Channel
            .ReadingLength = 3
        End With

        Me._Limits = New isr.VI.ReadingValue()
        With Me._Limits
            .ReadingLength = 4
        End With
        ' Units is a property of each element. If units are turned on, each element units is enabled.
        ' Me._Units = New isr.VI.ReadingElement() : Me._Units.ReadingLength = 4

    End Sub

    ''' <summary> Create a copy of the model. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As Readings)

        ' instantiate the base class
        MyBase.New()

        If model IsNot Nothing Then
            Me._Reading = New isr.VI.MeasuredAmount(model.Reading)
            Me._Timestamp = New isr.VI.ReadingAmount(model.Timestamp)
            Me._ReadingNumber = New isr.VI.ReadingValue(model.ReadingNumber)
            Me._Channel = New isr.VI.ReadingValue(model.Channel)
            Me._Limits = New isr.VI.ReadingValue(model.Limits)
            ' Units is a property of each element. If units are turned on, each element units is enabled.
            ' Me._Units = New isr.VI.ReadingElement(model.Units)
            Me.Elements = model._elements
        End If

    End Sub

    ''' <summary> Clones this class. </summary>
    ''' <returns> A copy of this object. </returns>
    Public Function Clone() As Readings
        Return New Readings(Me)
    End Function

#End Region

#Region " PARSE "

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
            Throw New ArgumentNullException("readingRecords")
        ElseIf readingRecords.Length = 0 Then
            Dim r As Readings() = {}
            Return r
        ElseIf baseReading Is Nothing Then
            Throw New ArgumentNullException("baseReading")
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
                If (value And isr.VI.ReadingElements.Reading) <> 0 Then
                    Me.AddReading(Me.Reading)
                End If
                If (value And isr.VI.ReadingElements.Timestamp) <> 0 Then
                    Me.AddReading(Me.Timestamp)
                End If
                If (value And isr.VI.ReadingElements.ReadingNumber) <> 0 Then
                    Me.AddReading(Me.ReadingNumber)
                End If
                If (value And isr.VI.ReadingElements.Channel) <> 0 Then
                    Me.AddReading(Me.Channel)
                End If
                If (value And isr.VI.ReadingElements.Limits) <> 0 Then
                    Me.AddReading(Me.Limits)
                End If
                ' Units is a property of each element. If units are turned on, each element units is enabled.
                If (value And isr.VI.ReadingElements.Units) <> 0 Then
                    For Each e As ReadingElement In Me.Readings
                        e.IncludesUnitsSuffix = True
                    Next
                End If
            End If
        End Set
    End Property

    ''' <summary>Gets or sets the <see cref="isr.VI.ReadingAmount">channel number</see>.</summary>
    Public Property Channel() As isr.VI.ReadingValue

    ''' <summary>Gets or sets the <see cref="isr.VI.ReadingAmount">limits</see>.</summary>
    Public Property Limits() As isr.VI.ReadingValue

    ''' <summary>Gets or sets the DMM <see cref="isr.VI.MeasuredAmount">reading</see>.</summary>
    Public Property Reading() As isr.VI.MeasuredAmount

    ''' <summary>Gets or sets the <see cref="isr.VI.ReadingAmount">reading number</see>.</summary>
    Public Property ReadingNumber() As isr.VI.ReadingValue

    ''' <summary>Gets or sets the timestamp <see cref="isr.VI.ReadingAmount">reading</see>.</summary>
    Public Property Timestamp() As isr.VI.ReadingAmount

    ''' <summary> Convert this object into a string representation. </summary>
    ''' <param name="element"> The element. </param>
    ''' <returns> Returns the reading value for the selected element. </returns>
    Public Overloads Function ToString(ByVal element As ReadingElements) As String

        Dim value As String = ""
        If Me.Reading IsNot Nothing Then
            Select Case element
                Case VI.ReadingElements.Reading
                    If Me.Reading IsNot Nothing AndAlso Me.Reading.Amount IsNot Nothing Then
                        value = Me.Reading.Amount.ToString()
                    End If
                Case VI.ReadingElements.Timestamp
                    If Me.Timestamp IsNot Nothing AndAlso Me.Reading.Amount IsNot Nothing Then
                        value = Me.Timestamp.Amount.ToString()
                    End If
                Case VI.ReadingElements.ReadingNumber
                    If Me.ReadingNumber IsNot Nothing AndAlso Me.Reading.Amount IsNot Nothing Then
                        value = Me.ReadingNumber.ToString()
                    End If
                Case VI.ReadingElements.Channel
                    If Me.Channel IsNot Nothing AndAlso Me.Reading.Amount IsNot Nothing Then
                        value = Me.Channel.ToString()
                    End If
                Case VI.ReadingElements.Limits
                    If Me.Limits IsNot Nothing AndAlso Me.Reading.Amount IsNot Nothing Then
                        value = Me.Limits.ToString()
                    End If
                    ' Units is a property of each element. If units are turned on, each element units is enabled.
                    ' Case VI.ReadingElements.Units
                    '     If Me.Units IsNot Nothing AndAlso Me.Reading.Amount IsNot Nothing Then
                    '         value = Me.Units.ToString()
                    '     End If
            End Select
        End If
        Return value

    End Function

#End Region

End Class
