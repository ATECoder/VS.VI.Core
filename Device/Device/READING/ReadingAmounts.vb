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
        Me._DefaultDelimiter = ","
        Me._Readings = New ReadingElementCollection
        Me._RawReading = ReadingAmounts.Empty
    End Sub

#End Region

#Region " PARSE "

    ''' <summary> Gets the default delimiter. </summary>
    ''' <value> The default delimiter. </value>
    Public Property DefaultDelimiter As String

    ''' <summary> Parses the measured data. </summary>
    ''' <param name="readings">          A record of one or more readings or an empty string to clear
    ''' the current readings. </param>
    ''' <param name="delimiter">         The delimiter. </param>
    ''' <param name="firstElementIndex"> Zero-based index of first element. </param>
    ''' <returns> <c>True</c> if parsed; <c>False</c> otherwise. </returns>
    Public Overridable Function TryParse(ByVal readings As String, ByVal delimiter As String, ByVal firstElementIndex As Integer) As Boolean

        If readings Is Nothing Then
            Me._RawReading = ReadingAmounts.Empty
            Return False
        ElseIf readings.Length = 0 Then
            ' indicate that we do not have a valid value
            Me.Reset()
            Me._RawReading = ReadingAmounts.Empty
            Return True
        Else
            Me._RawReading = readings
            Return Me.TryParse(readings.Split(CChar(delimiter)), firstElementIndex)
        End If

    End Function

    ''' <summary> Parses the measured data. </summary>
    ''' <param name="readings"> A record of one or more readings. </param>
    ''' <returns> <c>True</c> if parsed; <c>False</c> otherwise. </returns>
    Public Overridable Function TryParse(ByVal readings As String) As Boolean
        Return Me.TryParse(readings, Me.DefaultDelimiter, 0)
    End Function

    ''' <summary> Parses the measured data. </summary>
    ''' <param name="measuredData"> Information describing the measured. </param>
    ''' <returns> <c>True</c> if parsed; <c>False</c> otherwise. </returns>
    Public Overridable Function TryParse(ByVal measuredData As String()) As Boolean
        Return Me.TryParse(measuredData, 0)
    End Function

    ''' <summary> Parses the measured data. </summary>
    ''' <param name="readings">      A record of readings. </param>
    ''' <param name="firstElementIndex"> Zero-based index of first element. </param>
    ''' <returns> <c>True</c> if parsed; <c>False</c> otherwise. </returns>
    Public Overridable Function TryParse(ByVal readings As String(), ByVal firstElementIndex As Integer) As Boolean

        Dim affirmative As Boolean = False
        If readings Is Nothing Then
            Me._RawReading = ReadingAmounts.Empty
        ElseIf readings.Length < firstElementIndex + Me._Readings.Count Then
            ' indicate that we do not have a valid value
            Me.Reset()
            Me._RawReading = ReadingAmounts.Empty
        Else
            Me._RawReading = ""
            For Each readingItem As ReadingElement In Me._Readings
                Dim valueReading As String = readings(firstElementIndex)
                Me._RawReading = $"{Me._RawReading}{Me.DefaultDelimiter}{valueReading}"
                If readingItem.IncludesUnitsSuffix Then
                    Dim unitsSuffix As String = Me.ParseUnitSuffix(valueReading)
                    valueReading = ReadingAmounts.TrimUnits(valueReading, unitsSuffix)
                    affirmative = affirmative And readingItem.TryParse(valueReading, unitsSuffix)
                Else
                    affirmative = affirmative And readingItem.TryParse(valueReading)
                End If
                firstElementIndex += 1
            Next
        End If
        Return affirmative

    End Function

    ''' <summary> Parses all <see cref="ReadingAmount">reading elements</see> in a 
    '''           <see cref="ReadingElements">set of reading amounts</see>. 
    '''           Use for parsing reading elements that were set before limits were set.</summary>
    ''' <param name="values"> Specifies a reading element. </param>
    ''' <returns> <c>True</c> if parsed; <c>False</c> otherwise. </returns>
    Public Overridable Function TryParse(ByVal values As ReadingAmounts) As Boolean

        If values Is Nothing Then
            Me._RawReading = ReadingAmounts.Empty
            Return False
        End If

        ' clear all as we start from a fresh slate.
        Me.Reset()

        Return Me.TryParse(values.Readings.Readings, 0)

    End Function

    ''' <summary> Resets the measured outcomes. </summary>
    Public Overridable Sub Reset()
        Me.Readings.Reset()
    End Sub

#End Region

#Region " UNITS "

    ''' <summary> Returns the Unit Parser hash. </summary>
    ''' <returns> A Dictionary for translating SCPI unit names to <see cref="Arebis.TypedUnits">typed units</see>. </returns>
    Protected MustOverride Function UnitsDictionary() As Dictionary(Of String, Arebis.TypedUnits.Unit)

    ''' <summary> Parses the reading to create the corresponding unit. </summary>
    ''' <param name="reading"> Specifies the reading text. </param>
    ''' <param name="unit">    [in,out] The unit. </param>
    ''' <returns> <c>True</c> if parsed. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Function TryParse(ByVal reading As String, ByRef unit As Arebis.TypedUnits.Unit) As Boolean
        If Me.UnitsDictionary.ContainsKey(reading) Then
            unit = Me.UnitsDictionary(reading)
        Else
            unit = Nothing
        End If
        Return unit IsNot Nothing
    End Function


    ''' <summary> Unit suffixes. </summary>
    ''' <returns> A list of unit suffixes. </returns>
    Protected MustOverride Function UnitSuffixes() As ObjectModel.ReadOnlyCollection(Of String)

    ''' <summary> Extracts the unit suffix from the reading. </summary>
    ''' <param name="value"> The reading value that includes units as a suffix. </param>
    ''' <returns> The unit suffix </returns>
    Public Function ParseUnitSuffix(ByVal value As String) As String
        Dim suffix As String = ""
        If Not String.IsNullOrWhiteSpace(value) AndAlso Me.UnitSuffixes() IsNot Nothing Then
            For Each suffix In UnitSuffixes()
                If value.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) Then
                    Exit For
                End If
            Next
        End If
        Return suffix
    End Function

    ''' <summary> Trims unit suffixes. </summary>
    ''' <param name="value">       The value. </param>
    ''' <param name="unitsSuffix"> The units suffix. </param>
    ''' <returns> A string with the units suffix removed. </returns>
    Public Shared Function TrimUnits(ByVal value As String, ByVal unitsSuffix As String) As String
        If Not String.IsNullOrWhiteSpace(value) Then
            If Not String.IsNullOrWhiteSpace(unitsSuffix) Then
                value = value.Substring(0, value.Length - unitsSuffix.Length)
            End If
        End If
        Return value
    End Function

    ''' <summary> Trims unit suffixes. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> A string with the units suffix removed. </returns>
    Public Function TrimUnits(ByVal value As String) As String
        If Not String.IsNullOrWhiteSpace(value) AndAlso Me.UnitSuffixes() IsNot Nothing Then
            value = ReadingAmounts.TrimUnits(value, Me.ParseUnitSuffix(value))
        End If
        Return value
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

    ''' <summary> Gets or sets a collection of readings that implements the <see cref="ReadingAmount">reading
    ''' base</see>. </summary>
    ''' <value> The readings. </value>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")>
    Protected Property Readings() As ReadingElementCollection

    ''' <summary> Adds a reading to the collection and sets the measurement readings. </summary>
    ''' <param name="reading"> The reading. </param>
    Protected Sub AddReading(ByVal reading As ReadingElement)
        If Me._Readings Is Nothing Then
            Me._Readings = New ReadingElementCollection
        End If
        Me.Readings.Add(reading)
    End Sub

    ''' <summary> Returns the number of elements in the readings record. </summary>
    ''' <value> The number of elements. </value>
    Public ReadOnly Property ElementsCount() As Integer
        Get
            If Me.Readings Is Nothing Then
                Return 0
            Else
                Return Me.Readings.Count
            End If
        End Get
    End Property

    ''' <summary> Returns the total length of each readings record. </summary>
    ''' <value> The length of the elements. </value>
    Public ReadOnly Property ReadingsLength() As Integer
        Get
            If Me.Readings Is Nothing Then
                Return 0
            Else
                Return Me.Readings.ReadingsLength
            End If
        End Get
    End Property

#End Region

End Class

