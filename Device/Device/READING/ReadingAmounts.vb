Imports isr.Core.Pith.EnumExtensions
''' <summary> Holds and processes an <see cref="ReadingAmount">base class</see> to a single set of
''' instrument readings. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/2/2013" by="David" revision=""> Created. </history>
Public MustInherit Class ReadingAmounts

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Constructs this class. </summary>
    ''' <remarks> Use this constructor to instantiate this class and set its properties. </remarks>
    Protected Sub New()
        MyBase.New()
        Me._Readings = New ReadingEntityCollection
        Me._RawReading = ReadingAmounts.Empty
    End Sub

    ''' <summary> Constructs this class. </summary>
    ''' <param name="model"> The value. </param>
    Protected Sub New(ByVal model As ReadingAmounts)
        Me.New()
        If model IsNot Nothing Then
            Me._Readings = New ReadingEntityCollection(model.Readings)
            Me._RawReading = model.RawReading
            Me._ActiveReadingType = model._ActiveReadingType
            Me._Elements = model.Elements
        End If
    End Sub

#End Region

#Region " PARSE "

    ''' <summary> Gets the default delimiter. </summary>
    Public Const DefaultDelimiter As String = ","

    ''' <summary> Applies the measured data. </summary>
    ''' <param name="values">    A record of one or more reading values or an empty string to clear
    '''                            the current readings. </param>
    ''' <param name="delimiter"> The delimiter. </param>
    ''' <returns> <c>True</c> if parsed; <c>False</c> otherwise. </returns>
    Public Overridable Function TryApplyReadings(ByVal values As String, ByVal delimiter As String) As Boolean

        If values Is Nothing Then
            Me._RawReading = ReadingAmounts.Empty
            Return False
        ElseIf values.Length = 0 Then
            ' indicate that we do not have a valid value
            Me.Reset()
            Me._RawReading = ReadingAmounts.Empty
            Return True
        Else
            Me._RawReading = values
            Return Me.TryApplyReadings(New Queue(Of String)(values.Split(CChar(delimiter))))
        End If

    End Function

    ''' <summary> Applies the measured data. </summary>
    ''' <param name="values"> A record of one or more readings. </param>
    ''' <returns> <c>True</c> if parsed; <c>False</c> otherwise. </returns>
    Public Overridable Function TryApplyReadings(ByVal values As String) As Boolean
        Return Me.TryApplyReadings(values, ReadingAmounts.DefaultDelimiter)
    End Function

    ''' <summary> Applies the measured data. </summary>
    ''' <param name="values"> The reading values. </param>
    ''' <returns> <c>True</c> if parsed; <c>False</c> otherwise. </returns>
    Public Overridable Function TryApplyReadings(ByVal values As String()) As Boolean
        Return Me.TryApplyReadings(New Queue(Of String)(values))
    End Function

    ''' <summary> Applies  the measured data. </summary>
    ''' <param name="values"> Specifies the values. </param>
    ''' <returns> <c>True</c> if applies; <c>False</c> otherwise. </returns>
    Public Overridable Function TryApplyReadings(ByVal values As Queue(Of String)) As Boolean

        Dim affirmative As Boolean = False
        If values Is Nothing Then
            Me._RawReading = ReadingAmounts.Empty
        ElseIf values.Count < Me._Readings.Count Then
            ' if the queue has fewer values than expected, reset to 
            ' indicate that the value is invalid
            Me.Reset()
            Me._RawReading = ReadingAmounts.Empty
        Else
            Me._RawReading = ""
            affirmative = True
            For Each readingItem As ReadingEntity In Me._Readings
                Dim valueReading As String = values.Dequeue
                Me._RawReading = $"{Me._RawReading}{ReadingAmounts.DefaultDelimiter}{valueReading}"
                If readingItem.IncludesUnitsSuffix Then
                    Dim unitsSuffix As String = ReadingAmounts.ParseUnitSuffix(valueReading)
                    valueReading = ReadingAmounts.TrimUnits(valueReading, unitsSuffix)
                    affirmative = affirmative And readingItem.TryApplyReading(valueReading, unitsSuffix)
                Else
                    affirmative = affirmative And readingItem.TryApplyReading(valueReading)
                End If
            Next
        End If
        Return affirmative

    End Function

    ''' <summary> Parses all <see cref="ReadingAmount">reading elements</see> in a 
    '''           <see cref="ReadingTypes">set of reading amounts</see>. 
    '''           Use for parsing reading elements that were set before limits were set.</summary>
    ''' <param name="values"> Specifies a reading element. </param>
    ''' <returns> <c>True</c> if parsed; <c>False</c> otherwise. </returns>
    Public Overridable Function TryApplyReadings(ByVal values As ReadingAmounts) As Boolean

        If values Is Nothing Then
            Me._RawReading = ReadingAmounts.Empty
            Return False
        End If

        ' clear all as we start from a fresh slate.
        Me.Reset()

        Return Me.TryApplyReadings(values.Readings.ToRawReadings)

    End Function

    ''' <summary> Attempts to evaluate using the applied reading and given status. </summary>
    ''' <param name="status"> The status. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Overridable Function TryEvaluate(ByVal status As Long) As Boolean
        Dim affirmative As Boolean = True
        For Each readingItem As ReadingEntity In Me._Readings
            affirmative = affirmative And readingItem.TryEvaluate(status)
        Next
        Return affirmative
    End Function

    ''' <summary> Resets the measured outcomes. </summary>
    Public Overridable Sub Reset()
        Me.Readings.Reset()
    End Sub

#End Region

#Region " UNITS "

    ''' <summary> Adds a unit to the units dictionary. </summary>
    ''' <param name="suffix"> The suffix. </param>
    ''' <param name="unit">   The unit. </param>
    Public Shared Sub AddUnit(ByVal suffix As String, ByVal unit As Arebis.TypedUnits.Unit)
        If String.IsNullOrWhiteSpace(suffix) Then Throw New ArgumentNullException(NameOf(suffix))
        If unit Is Nothing Then Throw New ArgumentNullException(NameOf(unit))
        If ReadingAmounts.UnitsDictionary.Keys.Contains(suffix) Then
            If Not unit.Equals(ReadingAmounts.UnitsDictionary(suffix)) Then
                Throw New InvalidOperationException($"Mismatch detected: Attempting to add {unit.Symbol} unit where existing {suffix} is {ReadingAmounts.UnitsDictionary(suffix).Symbol}")
            End If
        Else
            ReadingAmounts._UnitsDictionary.Add(suffix, unit)
        End If
    End Sub

    Private Shared _UnitsDictionary As Dictionary(Of String, Arebis.TypedUnits.Unit)
    ''' <summary> Returns the Unit Parser hash. </summary>
    ''' <returns> A Dictionary for translating SCPI unit names to <see cref="Arebis.StandardUnits">standard units</see>. </returns>
    Public Shared Function UnitsDictionary() As Dictionary(Of String, Arebis.TypedUnits.Unit)
        If ReadingAmounts._UnitsDictionary Is Nothing Then
            ReadingAmounts._UnitsDictionary = New Dictionary(Of String, Arebis.TypedUnits.Unit)
            Dim dix3 As Dictionary(Of String, Arebis.TypedUnits.Unit) = ReadingAmounts._UnitsDictionary
            dix3.Add("ADC", Arebis.StandardUnits.ElectricUnits.Ampere)
            dix3.Add("OHM", Arebis.StandardUnits.ElectricUnits.Ohm)
            dix3.Add("OHM4W", Arebis.StandardUnits.ElectricUnits.Ohm)
            dix3.Add("VDC", Arebis.StandardUnits.ElectricUnits.Volt)
            dix3.Add("SECS", Arebis.StandardUnits.TimeUnits.Second)
            dix3.Add("RDNG#", Arebis.StandardUnits.UnitlessUnits.Count)
            dix3 = Nothing
        End If
        Return ReadingAmounts._UnitsDictionary
    End Function

    ''' <summary> Tries to parse a unit from the unit suffix of the reading. </summary>
    ''' <param name="reading"> Specifies the reading text. </param>
    ''' <param name="unit">    [in,out] The unit. </param>
    ''' <returns> <c>True</c> if parsed. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function TryParseUnit(ByVal reading As String, ByRef unit As Arebis.TypedUnits.Unit) As Boolean
        Dim suffix As String = ReadingAmounts.ParseUnitSuffix(reading)
        If String.IsNullOrEmpty(suffix) OrElse Not ReadingAmounts.UnitsDictionary.Keys.Contains(suffix) Then
            unit = Nothing
        Else
            unit = ReadingAmounts.UnitsDictionary(suffix)
        End If
        Return unit IsNot Nothing
    End Function

    ''' <summary> Extracts the unit suffix from the reading. </summary>
    ''' <param name="reading"> The reading value that includes units as a suffix. </param>
    ''' <returns> The unit suffix </returns>
    Public Shared Function ParseUnitSuffix(ByVal reading As String) As String
        Dim suffix As String = ""
        If Not String.IsNullOrWhiteSpace(reading) AndAlso ReadingAmounts.UnitsDictionary.Keys.Any Then
            For Each suffix In ReadingAmounts.UnitsDictionary.Keys
                If reading.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) Then
                    Exit For
                End If
            Next
        End If
        Return suffix
    End Function

    ''' <summary> Trims unit suffixes. </summary>
    ''' <param name="reading">     The raw reading. </param>
    ''' <param name="unitsSuffix"> The units suffix. </param>
    ''' <returns> A string with the units suffix removed. </returns>
    Public Shared Function TrimUnits(ByVal reading As String, ByVal unitsSuffix As String) As String
        If Not (String.IsNullOrWhiteSpace(reading) OrElse String.IsNullOrWhiteSpace(unitsSuffix)) Then
            reading = reading.Substring(0, reading.Length - unitsSuffix.Length)
        End If
        Return reading
    End Function

    ''' <summary> Trims unit suffixes. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> A string with the units suffix removed. </returns>
    Public Shared Function TrimUnits(ByVal value As String) As String
        Return ReadingAmounts.TrimUnits(value, ReadingAmounts.ParseUnitSuffix(value))
    End Function

#End Region

#Region " READINGS "

    ''' <summary> The empty reading string. </summary>
    Public Const Empty As String = "nil"

    ''' <summary> Gets the raw reading. </summary>
    ''' <value> The raw reading. </value>
    Public ReadOnly Property RawReading As String

    ''' <summary> Gets the is empty. </summary>
    ''' <value> The is empty. </value>
    Public ReadOnly Property IsEmpty As Boolean
        Get
            Return String.IsNullOrWhiteSpace(Me.RawReading) OrElse String.Equals(Me.RawReading, ReadingAmounts.Empty)
        End Get
    End Property

    ''' <summary> Gets a collection of readings that implements the <see cref="ReadingAmount">reading
    ''' base</see>. </summary>
    ''' <value> The readings. </value>
    Public ReadOnly Property Readings() As ReadingEntityCollection

#End Region

#Region " ACTIVE READING "

    ''' <summary> Gets the reading elements. </summary>
    ''' <value> The elements. </value>
    Public ReadOnly Property Elements() As isr.VI.ReadingTypes

    ''' <summary> Initializes this object. </summary>
    ''' <remarks> Adds reading elements in the order they are returned by the instrument so as to
    ''' automate parsing of these data. </remarks>
    ''' <param name="value"> The value. </param>
    Public Overridable Sub Initialize(ByVal value As isr.VI.ReadingTypes)
        Me._Elements = value
        Me._Readings = New ReadingEntityCollection
    End Sub

    ''' <summary> Gets or sets the reading type of the active reading entity. </summary>
    ''' <value> The active element. </value>
    Public Property ActiveReadingType As isr.VI.ReadingTypes

    ''' <summary> Returns the meta status of the active reading. </summary>
    ''' <returns> The MetaStatus. </returns>
    Public Function ActiveMetaStatus() As MetaStatus
        Dim result As MetaStatus = New MetaStatus
        Dim amount As MeasuredAmount = TryCast(Me.Readings(Me.ActiveReadingType), MeasuredAmount)
        If amount IsNot Nothing Then result = amount.MetaStatus
        Return result
    End Function

    ''' <summary> Active reading amount. </summary>
    ''' <returns> A ReadingAmount. </returns>
    Public Function ActiveReadingAmount() As ReadingAmount
        Return TryCast(Me.Readings(Me.ActiveReadingType), ReadingAmount)
    End Function

    ''' <summary> Active reading unit symbol. </summary>
    ''' <returns> A String. </returns>
    Public Function ActiveReadingUnitSymbol() As String
        Dim result As String = ""
        Dim amount As ReadingAmount = Me.ActiveReadingAmount
        If amount IsNot Nothing Then
            result = amount.Unit.Symbol
        End If
        Return result
    End Function

    ''' <summary> Returns the caption value of the active reading. </summary>
    ''' <returns> A String. </returns>
    Public Function ActiveAmountCaption() As String
        Dim result As String = ""
        Dim amount As ReadingAmount = Me.ActiveReadingAmount
        If amount Is Nothing Then
            If Me.IsEmpty Then
                result = "0x------- "
            Else
                Dim value As ReadingStatus = TryCast(Me.Readings(Me.ActiveReadingType), ReadingStatus)
                If value Is Nothing Then
                    result = "-.------- :("
                Else
                    result = value.ToString()
                End If
            End If
        Else
            If Me.IsEmpty Then
                result = $"-.------- {amount.Unit.Symbol}"
            Else
                result = amount.Amount.ToString
            End If
        End If
        Return result
    End Function

    ''' <summary> List elements. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    ''' <param name="excluded">    The excluded. </param>
    Public Sub ListElements(ByVal listControl As Windows.Forms.ComboBox, ByVal excluded As ReadingTypes)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Dim selectedIndex As Integer = listControl.SelectedIndex
        With listControl
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(ReadingTypes).ValueDescriptionPairs(Me.Elements And Not excluded)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            If .Items.Count > 0 Then
                .SelectedIndex = Math.Max(selectedIndex, 0)
            End If
        End With
    End Sub

#End Region

End Class

