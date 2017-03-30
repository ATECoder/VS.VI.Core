Imports System.ComponentModel
Imports System.Windows.Forms
Imports isr.Core.Pith.SubstringExtensions

''' <summary> A buffer reading. </summary>
''' <remarks> David, 7/23/2016. </remarks>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="7/23/2016" by="David" revision=""> Created. </history>
Public Class BufferReading

#Region " CONSTRUCTORS "

    ''' <summary> Constructor. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    Public Sub New()
        MyBase.New()
        Me._Clear()
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <param name="data"> The value. </param>
    Public Sub New(ByVal data As Queue(Of String))
        Me.New()
        Me._Parse(data)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <remarks> David, 8/11/2016. </remarks>
    ''' <param name="data">         The value. </param>
    ''' <param name="firstReading"> The first buffer reading. </param>
    Public Sub New(ByVal data As Queue(Of String), ByVal firstReading As BufferReading)
        Me.New()
        Me._Parse(data, firstReading)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <remarks> David, 2/23/2017. </remarks>
    ''' <param name="reading">      The reading. </param>
    ''' <param name="firstReading"> The first buffer reading. </param>
    Public Sub New(ByVal reading As BufferReading, ByVal firstReading As BufferReading)
        Me.New()
        If reading IsNot Nothing Then
            Me._LocalTime = reading.LocalTime
            Me._Reading = reading.Reading
            Me.ParseStatus(reading.StatusReading)
            Me.ParseTimestamp(reading.TimestampReading)
            Me._adjustRelativeTimespan(firstReading)
            Me.ParseUnit(reading.UnitReading)
        End If
    End Sub

    ''' <summary> Clears this object to its blank/initial state. </summary>
    ''' <remarks> David, 8/11/2016. </remarks>
    Private Sub _Clear()
        Me._Reading = ""
        Me._TimestampReading = ""
        Me._StatusReading = ""
        Me._Timestamp = DateTime.MinValue
        Me._LocalTime = DateTime.MinValue
        Me._FractionalSecond = 0
        Me._FractionalTimestamp = TimeSpan.Zero
        Me.RelativeTimespan = TimeSpan.Zero
        Me._UnitReading = ""
        Me._Amount = Nothing
    End Sub

    ''' <summary> Gets or sets the reading. </summary>
    ''' <value> The reading. </value>
    Public ReadOnly Property Reading As String

    ''' <summary> Gets or sets the timestamp reading. </summary>
    ''' <value> The timestamp reading. </value>
    Public ReadOnly Property TimestampReading As String

    ''' <summary> Gets or sets the status reading. </summary>
    ''' <value> The status reading. </value>
    Public ReadOnly Property StatusReading As String


    ''' <summary> Parses the given value. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <param name="data"> The value. </param>
    Private Sub _Parse(ByVal data As Queue(Of String))
        If data Is Nothing Then Throw New ArgumentNullException(NameOf(data))
        Me._Clear()
        Me._LocalTime = DateTime.Now
        If data.Any Then
            Me._Reading = data.Dequeue
        End If
        If data.Any Then
            Me.ParseTimestamp(data.Dequeue)
        End If
        If data.Any Then
            Me.ParseStatus(data.Dequeue)
        End If
        If data.Any Then
            Me.ParseUnit(data.Dequeue)
        End If
    End Sub

    ''' <summary> Parses the given value. </summary>
    ''' <remarks> David, 8/11/2016. </remarks>
    ''' <param name="data">         The value. </param>
    ''' <param name="firstReading"> The first buffer reading. </param>
    Private Sub _Parse(ByVal data As Queue(Of String), ByVal firstReading As BufferReading)
        Me._Parse(data)
        If Not String.IsNullOrWhiteSpace(Me._TimestampReading) Then
            Me._adjustRelativeTimespan(firstReading)
        End If
    End Sub

#End Region

#Region " STATUS "

    ''' <summary> Gets or sets the status word. </summary>
    ''' <value> The status word. </value>
    Public ReadOnly Property StatusWord As Integer

    ''' <summary> Parse status. </summary>
    ''' <remarks> David, 2/24/2017. </remarks>
    ''' <param name="reading"> The reading. </param>
    Private Sub ParseStatus(ByVal reading As String)
        Me._StatusReading = reading
        Dim value As Double = Double.Parse(reading, Globalization.NumberStyles.AllowDecimalPoint Or Globalization.NumberStyles.AllowExponent)
        Me._StatusWord = CInt(value)
    End Sub

#End Region

#Region " AMOUNT "

    ''' <summary> Gets or sets the unit reading. </summary>
    ''' <value> The unit reading. </value>
    Public ReadOnly Property UnitReading As String

    ''' <summary> The amount. </summary>
    ''' <value> The amount. </value>
    ''' <remarks>Value is valid id the status word is not <see cref="BufferStatusBits.Questionable"/> </remarks>
    Public ReadOnly Property Amount As Arebis.TypedUnits.Amount

    ''' <summary> Parse unit. </summary>
    ''' <remarks> David, 2/25/2017. </remarks>
    ''' <param name="unit"> The reading. </param>
    Private Sub ParseUnit(ByVal unit As String)
        Me._UnitReading = unit
        Dim value As Double = 0
        If Not Double.TryParse(Me.Reading, value) Then
            value = 0
            ' if failed to parse value, tag as questionable.
            Me._StatusWord = Me._StatusWord Or BufferStatusBits.Questionable
        End If
        Select Case True
            Case String.Equals(unit, "OHM", StringComparison.OrdinalIgnoreCase)
                Me._Amount = New Arebis.TypedUnits.Amount(value, Arebis.StandardUnits.ElectricUnits.Ohm)
            Case String.Equals(unit.SafeSubstring(0, 4), "VOLT", StringComparison.OrdinalIgnoreCase)
                Me._Amount = New Arebis.TypedUnits.Amount(value, Arebis.StandardUnits.ElectricUnits.Volt)
            Case String.Equals(unit.SafeSubstring(0, 3), "AMP", StringComparison.OrdinalIgnoreCase)
                Me._Amount = New Arebis.TypedUnits.Amount(value, Arebis.StandardUnits.ElectricUnits.Ampere)
            Case String.Equals(unit.SafeSubstring(0, 4), "KELV", StringComparison.OrdinalIgnoreCase)
                Me._Amount = New Arebis.TypedUnits.Amount(value, Arebis.StandardUnits.TemperatureUnits.Kelvin)
            Case String.Equals(unit.SafeSubstring(0, 4), "CELS", StringComparison.OrdinalIgnoreCase)
                Me._Amount = New Arebis.TypedUnits.Amount(value, Arebis.StandardUnits.TemperatureUnits.DegreeCelsius)
            Case String.Equals(unit.SafeSubstring(0, 4), "FAHR", StringComparison.OrdinalIgnoreCase)
                Me._Amount = New Arebis.TypedUnits.Amount(value, Arebis.StandardUnits.TemperatureUnits.DegreeFahrenheit)
            Case String.Equals(unit, "DB", StringComparison.OrdinalIgnoreCase)
                Me._Amount = New Arebis.TypedUnits.Amount(value, Arebis.StandardUnits.UnitlessUnits.Decibel)
        End Select

    End Sub

#End Region

#Region " TIMESTAMP "

    ''' <summary> Parse timestamp. </summary>
    ''' <remarks> David, 8/11/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="timestamp"> The time stamp rounded down to the second. </param>
    Private Sub ParseTimestamp(ByVal timestamp As String)
        If String.IsNullOrWhiteSpace(timestamp) Then Throw New ArgumentNullException(NameOf(timestamp))
        Me._TimestampReading = timestamp
        Dim q As New Queue(Of String)(timestamp.Split("."c))
        Me._Timestamp = DateTime.Parse(q.Dequeue)
        Me._FractionalSecond = Double.Parse($".{q.Dequeue}")
        Me._FractionalTimestamp = TimeSpan.FromTicks(CLng(TimeSpan.TicksPerSecond * Me._FractionalSecond))
        Me._MeterTime = Me._Timestamp.Add(Me.FractionalTimestamp)
    End Sub

    ''' <summary> Gets the local time. </summary>
    ''' <value> The local time. </value>
    Public ReadOnly Property LocalTime As DateTime

    ''' <summary> Gets or sets the meter time. </summary>
    ''' <value> The meter time. </value>
    Public ReadOnly Property MeterTime As DateTime

    ''' <summary> Gets the time stamp rounded down to the second. </summary>
    ''' <value> The time stamp rounded down to the second. </value>
    ''' <remakrs> The actual time is the sum of <see cref="Timestamp"/> and <see cref="FractionalTimestamp"/>  </remakrs>
    Public ReadOnly Property Timestamp As DateTime

    ''' <summary> Gets or sets the fractional second. </summary>
    ''' <value> The fractional second. </value>
    Public ReadOnly Property FractionalSecond As Double

    ''' <summary> Gets the fractional timestamp. </summary>
    ''' <value> The fractional timestamp. </value>
    ''' <remarks> Converted from the fractional second of the instrument timestamp f</remarks>
    Public ReadOnly Property FractionalTimestamp As TimeSpan

    ''' <summary> Gets or sets the timespan relative to the first reading. </summary>
    ''' <value> The relative timespan. </value>
    Public Property RelativeTimespan As TimeSpan

    ''' <summary> Parses the given value. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <param name="data">         The value. </param>
    ''' <param name="firstReading"> The first buffer reading. </param>
    Public Sub Parse(ByVal data As Queue(Of String), ByVal firstReading As BufferReading)
        Me._Parse(data)
        Me._adjustRelativeTimespan(firstReading)
    End Sub

    ''' <summary> Adjust relative timespan. </summary>
    ''' <remarks> David, 8/11/2016. </remarks>
    ''' <param name="firstReading"> The first buffer reading. </param>
    Private Sub _adjustRelativeTimespan(ByVal firstReading As BufferReading)
        If firstReading Is Nothing Then
            Me.RelativeTimespan = TimeSpan.Zero
        Else
            Me.RelativeTimespan = Me.Timestamp.Subtract(firstReading.Timestamp).Add(Me.FractionalTimestamp).Subtract(firstReading.FractionalTimestamp)
        End If
    End Sub

    ''' <summary> Adjust relative timespan. </summary>
    ''' <remarks> David, 8/11/2016. </remarks>
    ''' <param name="firstReading"> The first buffer reading. </param>
    Public Sub AdjustRelativeTimespan(ByVal firstReading As BufferReading)
        Me._adjustRelativeTimespan(firstReading)
    End Sub

#End Region

End Class

''' <summary> A buffer readings collection. </summary>
''' <remarks> David, 7/23/2016. </remarks>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="7/23/2016" by="David" revision=""> Created. </history>
Public Class BufferReadingCollection
    Inherits ObjectModel.ObservableCollection(Of BufferReading)

#Region " CONSTRUCTORS "

    Public Sub New()
        MyBase.New
        Me._FirstReading = New BufferReading()
        Me._LastReading = New BufferReading()
    End Sub

#End Region

#Region " ADD "

    ''' <summary> Parses the reading and adds values to the collection. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="data"> The data. </param>
    Public Overloads Sub Add(ByVal data As String)
        If String.IsNullOrWhiteSpace(data) Then Throw New ArgumentNullException(NameOf(data))
        Dim q As New Queue(Of String)(data.Split(","c))
        Do While q.Any
            Me.Add(q)
        Loop
    End Sub

    ''' <summary> Parses the reading and adds values to the collection. </summary>
    ''' <remarks> David, 2/23/2017. </remarks>
    ''' <param name="readingTimestampQueue"> The reading plus timestamp pair of values to add. </param>
    Public Overloads Sub Add(ByVal readingTimestampQueue As Queue(Of String))
        If readingTimestampQueue Is Nothing Then Throw New ArgumentNullException(NameOf(readingTimestampQueue))
        Me.Add(New BufferReading(readingTimestampQueue, Me.FirstReading))
    End Sub

    ''' <summary>
    ''' Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />
    ''' .
    ''' </summary>
    ''' <remarks> David, 2/23/2017. </remarks>
    ''' <param name="value"> The object to add to the
    '''                      <see cref="T:System.Collections.Generic.ICollection`1" />
    '''                      . </param>
    Public Overloads Sub Add(ByVal value As BufferReading)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        If Not Me.Any Then Me._FirstReading = value
        Me._LastReading = value
        ' does not help getting the grid refreshed: 
        'value.NotifyValueChanged()
        MyBase.Add(value)
        ' does not help getting the grid refreshed: 
        'Me.OnCollectionChanged(New Specialized.NotifyCollectionChangedEventArgs(Specialized.NotifyCollectionChangedAction.Add))
    End Sub

    ''' <summary> Parses the reading and adds values to the collection. </summary>
    ''' <remarks> David, 2/23/2017. </remarks>
    ''' <param name="values"> The values to add. </param>
    Public Overloads Sub Add(ByVal values As IEnumerable(Of BufferReading))
        If values Is Nothing Then Throw New ArgumentNullException(NameOf(values))
        For Each br As BufferReading In values
            Me.Add(New BufferReading(br, Me.FirstReading))
        Next
    End Sub

    Public Overloads Sub Clear()
        MyBase.Clear()
        Me._FirstReading = New BufferReading()
        Me._LastReading = New BufferReading()
    End Sub

#End Region

#Region " FIRST AND LAST VALUES "

    ''' <summary> Gets or sets the first reading. </summary>
    ''' <value> The first reading. </value>
    Public ReadOnly Property FirstReading As BufferReading

    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public ReadOnly Property LastReading As BufferReading

#End Region

#Region " DISPLAY "

    ''' <summary> Configure buffer display. </summary>
    ''' <remarks> David, 2/24/2017. </remarks>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Function ConfigureBufferDisplay(ByVal grid As Windows.Forms.DataGridView) As Integer
        If grid Is Nothing Then Throw New ArgumentNullException(NameOf(grid))
        Dim wasEnabled As Boolean = grid.Enabled

        With grid
            .Enabled = False
            .AllowUserToAddRows = False
            .AutoGenerateColumns = False
            .AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGreen
            .AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
            .BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            .ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            .EnableHeadersVisualStyles = True
            .MultiSelect = True
            .RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .ScrollBars = Windows.Forms.ScrollBars.Both
        End With

        grid.Columns.Clear()
        grid.DataSource = Me

        Dim displayIndex As Integer = 0
        Dim column As New DataGridViewTextBoxColumn()
        With column
            .DataPropertyName = NameOf(BufferReading.Reading)
            .Name = NameOf(BufferReading.Reading)
            .Visible = True
            .DisplayIndex = displayIndex
            .HeaderText = "Value"
        End With
        grid.Columns.Add(column)

        displayIndex += 1
        column = New DataGridViewTextBoxColumn()
        With column
            .DataPropertyName = NameOf(BufferReading.UnitReading)
            .Name = NameOf(BufferReading.UnitReading)
            .Visible = True
            .DisplayIndex = displayIndex
            .HeaderText = "Unit"
        End With
        grid.Columns.Add(column)

        displayIndex += 1
        column = New DataGridViewTextBoxColumn()
        With column
            .DataPropertyName = NameOf(BufferReading.TimestampReading)
            .Name = NameOf(BufferReading.TimestampReading)
            .Visible = True
            .DisplayIndex = displayIndex
            .HeaderText = "Time"
        End With
        grid.Columns.Add(column)

        displayIndex += 1
        column = New DataGridViewTextBoxColumn()
        With column
            .DataPropertyName = NameOf(BufferReading.StatusReading)
            .Name = NameOf(BufferReading.StatusReading)
            .Visible = True
            .DisplayIndex = displayIndex
            .HeaderText = "Status"
        End With
        grid.Columns.Add(column)

        grid.Enabled = wasEnabled
        If grid.Columns IsNot Nothing Then
            Return grid.Columns.Count
        Else
            Return 0
        End If

    End Function

    ''' <summary> Displays the readings described by grid. </summary>
    ''' <remarks> David, 2/24/2017. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    Public Function DisplayReadings(ByVal grid As DataGridView, ByVal reconfigure As Boolean) As Integer
        If grid Is Nothing Then Throw New ArgumentNullException(NameOf(grid))
        Dim wasEnabled As Boolean = grid.Enabled
        Try
            grid.Enabled = False
            ' 2/24/2017: had to configure each time otherwise, the grid would not display
            ' when called from the thread.
            If grid.DataSource Is Nothing OrElse reconfigure Then
                Me.ConfigureBufferDisplay(grid)
                ' grid.DataSource = Me
            End If
            For Each c As DataGridViewColumn In grid.Columns
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Next
            grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
            grid.Invalidate()
            Return grid.Columns.Count
        Catch
            Throw
        Finally
            grid.Enabled = wasEnabled
        End Try
    End Function

#End Region

End Class

''' <summary> Values that represent Buffer status bits for sense measurements. </summary>
''' <remarks> David, 2/24/2017. </remarks>
<Flags>
Public Enum BufferStatusBits
    <Description("Not specified")> None

    ''' <summary> Measure status questionable. </summary>
    <Description("Measure status questionable")> Questionable = &H1

#If False Then
    ''' <summary> A/D converter from which reading originated; for the
    '''    Model DMM7510, this will always be 0 (Main) Or 4 (2 if right shift 1).
    ''' (digitizer). </summary>
    <Description("A/D converter")> Origin = &H6
#End If

    ''' <summary> Digitized A/D converter is used. Otherwise, Main. </summary>
    <Description("Digitized converter")> Digitizer = &H4

#If False Then
    ''' <summary> Measure terminal, front is 1, rear is 0. </summary>
    <Description("Measure terminal")> Terminal = &H8
#End If

    ''' <summary> Front terminal. </summary>
    <Description("Front terminal")> FrontTerminal = &H8

    ''' <summary> Measure status limit 2 low. </summary>
    <Description("")> Limit2Low = &H10

    ''' <summary> Measure status limit 2 high. </summary>
    <Description("")> Limit2High = &H20

    ''' <summary> Measure status limit 1 low. </summary>
    <Description("")> Limit1Low = &H40

    ''' <summary> Measure status limit 1 high. </summary>
    <Description("")> Limit1High = &H80

    ''' <summary> First reading in a group. </summary>
    <Description("First reading in a group ")> StartGroup = &H100

End Enum

