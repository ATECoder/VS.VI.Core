Imports System.Drawing
Imports System.Windows.Forms
''' <summary> A channel source measure elements. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/13/2018" by="David" revision=""> Created. </history>
Public Class ChannelSourceMeasure

    ''' <summary> Constructor. </summary>
    ''' <param name="title">       The title. </param>
    ''' <param name="channelList"> List of channels. </param>
    Public Sub New(ByVal title As String, ByVal channelList As String)
        MyBase.New()
        Me.Title = title
        Me.ChannelList = Me.ToSortedList(channelList)
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="channelSourceMeasure"> The channel source measure. </param>
    Public Sub New(ByVal channelSourceMeasure As ChannelSourceMeasure)
        Me.New(ChannelSourceMeasure.Validated(channelSourceMeasure).Title, channelSourceMeasure.ChannelList)
        Me.Current = channelSourceMeasure.Current
        Me.Voltage = channelSourceMeasure.Voltage
    End Sub

    ''' <summary> Validated the given channel source measure. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="channelSourceMeasure"> The channel source measure. </param>
    ''' <returns> A ChannelSourceMeasure. </returns>
    Public Shared Function Validated(ByVal channelSourceMeasure As ChannelSourceMeasure) As ChannelSourceMeasure
        If channelSourceMeasure Is Nothing Then Throw New ArgumentNullException(NameOf(channelSourceMeasure))
        Return channelSourceMeasure
    End Function

    Private Function ToSortedList(ByVal list As String) As String
        Return Me.ToSortedList(list, Me.ChannelListDelimiter(list))
    End Function

    Private Function ChannelListDelimiter(ByVal list As String) As Char
        Dim result As Char = ";"c
        If Not list.Contains(result) Then
            result = ","c
        End If
        Return result
    End Function

    ''' <summary> Converts this object to a sorted list. </summary>
    ''' <param name="list">      The list. </param>
    ''' <param name="delimiter"> The delimiter. </param>
    ''' <returns> The given data converted to a String. </returns>
    Private Function ToSortedList(ByVal list As String, ByVal delimiter As Char) As String
        Dim result As New System.Text.StringBuilder
        Dim l As New List(Of String)(list.Split(delimiter))
        l.Sort()
        For Each s As String In l
            If result.Length > 0 Then result.Append(delimiter)
            result.Append(s)
        Next
        Return result.ToString
    End Function

    ''' <summary> Gets the title. </summary>
    ''' <value> The title. </value>
    Public ReadOnly Property Title As String

    ''' <summary> Gets a list of channels. </summary>
    ''' <value> A List of channels. </value>
    Public ReadOnly Property ChannelList As String

    ''' <summary> Gets the sentinel indicating if the measure has a non zero current value. </summary>
    ''' <value> The sentinel indicating if the measure has a non zero current value. </value>
    Public ReadOnly Property HasValue As Boolean
        Get
            Return Me.Current <> 0
        End Get
    End Property

    ''' <summary> Gets the resistance. </summary>
    ''' <value> The sheet resistance or <see cref="Double.NaN"/> if not <see cref="HasValue"/>. </value>
    Public ReadOnly Property Resistance As Double
        Get
            If Me.HasValue Then
                Return Me.Voltage / Me.Current
            Else
                Return Double.NaN
            End If
        End Get
    End Property

    ''' <summary> Gets or sets the voltage. </summary>
    ''' <value> The voltage. </value>
    Public Property Voltage As Double

    ''' <summary> Gets or sets the current. </summary>
    ''' <value> The current. </value>
    Public Property Current As Double
End Class

''' <summary> Channel source measure Collection: an ordered collection of source measures. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/13/2018" by="David" revision=""> Created. </history>
Public Class ChannelSourceMeasureCollection
    Inherits ObjectModel.KeyedCollection(Of String, ChannelSourceMeasure)

    ''' <summary>
    ''' When implemented in a derived class, extracts the key from the specified element.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="item"> The element from which to extract the key. </param>
    ''' <returns> The key for the specified element. </returns>
    Protected Overrides Function GetKeyForItem(item As ChannelSourceMeasure) As String
        If item Is Nothing Then Throw New ArgumentNullException(NameOf(item))
        Return item.Title
    End Function

    ''' <summary> Adds a new source measure. </summary>
    ''' <param name="title">       The title. </param>
    ''' <param name="channelList"> List of channels. </param>
    Public Sub AddSourceMeasure(ByVal title As String, ByVal channelList As String)
        Me.Add(New ChannelSourceMeasure(title, channelList))
    End Sub

    ''' <summary> Configure display values. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Function ConfigureDisplayValues(ByVal grid As DataGridView) As Integer

        If grid Is Nothing Then Throw New ArgumentNullException(NameOf(grid))

        Dim wasEnabled As Boolean = grid.Enabled
        grid.Enabled = False
        grid.Enabled = wasEnabled

        grid.Enabled = False
        grid.DataSource = Nothing
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSalmon
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        grid.AutoGenerateColumns = False
        grid.RowHeadersVisible = False
        grid.ReadOnly = True
        grid.DataSource = Me.ToArray

        grid.Columns.Clear()
        grid.Refresh()
        Dim displayIndex As Integer = 0
        Dim width As Integer = 30
        Dim column As DataGridViewTextBoxColumn = Nothing
        Try
            column = New DataGridViewTextBoxColumn()
            With column
                .DataPropertyName = NameOf(ChannelSourceMeasure.Title)
                .Name = NameOf(ChannelSourceMeasure.Title)
                .Visible = True
                .DisplayIndex = displayIndex
            End With
            grid.Columns.Add(column)
            width += column.Width
        Catch
            If column IsNot Nothing Then column.Dispose()
            Throw
        End Try

        column = Nothing
        Try
            displayIndex += 1
            column = New DataGridViewTextBoxColumn()
            With column
                .DataPropertyName = NameOf(ChannelSourceMeasure.Voltage)
                .Name = "Volt"
                .Visible = True
                .DisplayIndex = displayIndex
                .Width = grid.Width - width - grid.Columns.Count
                .DefaultCellStyle.Format = "G5"
            End With
        Catch
            If column IsNot Nothing Then column.Dispose()
            Throw
        End Try

        column = Nothing
        Try
            displayIndex += 1
            column = New DataGridViewTextBoxColumn()
            With column
                .DataPropertyName = NameOf(ChannelSourceMeasure.Current)
                .Name = "Ampere"
                .Visible = True
                .DisplayIndex = displayIndex
                .Width = grid.Width - width - grid.Columns.Count
                .DefaultCellStyle.Format = "G5"
            End With
        Catch
            If column IsNot Nothing Then column.Dispose()
            Throw
        End Try

        column = Nothing
        Try
            displayIndex += 1
            column = New DataGridViewTextBoxColumn()
            With column
                .DataPropertyName = NameOf(ChannelSourceMeasure.Resistance)
                .Name = "Ohm"
                .Visible = True
                .DisplayIndex = displayIndex
                .Width = grid.Width - width - grid.Columns.Count
                .DefaultCellStyle.Format = "G5"
            End With
            grid.Columns.Add(column)
        Catch
            If column IsNot Nothing Then column.Dispose()
            Throw
        End Try

        column = Nothing
        grid.Enabled = True
        If grid.Columns IsNot Nothing AndAlso grid.Columns.Count > 0 Then
            Return grid.Columns.Count
        Else
            Return 0
        End If

    End Function

    ''' <summary> Displays the values described by grid. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    Public Function DisplayValues(ByVal grid As DataGridView) As Integer

        If grid Is Nothing Then Throw New ArgumentNullException(NameOf(grid))

        Dim wasEnabled As Boolean = grid.Enabled
        grid.Enabled = False
        grid.Enabled = wasEnabled

        If grid.DataSource Is Nothing Then
            Me.ConfigureDisplayValues(grid)
            Application.DoEvents()
        End If

        grid.DataSource = Me
        Application.DoEvents()
        Return grid.Columns.Count

    End Function

    ''' <summary> Configure display. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    Public Function ConfigureDisplay(ByVal grid As DataGridView) As Integer

        If grid Is Nothing Then Throw New ArgumentNullException(NameOf(grid))

        Dim wasEnabled As Boolean = grid.Enabled
        grid.Enabled = False
        grid.Enabled = wasEnabled

        grid.Enabled = False
        grid.DataSource = Nothing
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSalmon
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        grid.AutoGenerateColumns = True
        grid.RowHeadersVisible = False
        grid.ReadOnly = True
        grid.DataSource = Me.ToArray
        grid.Enabled = True
        If grid.Columns IsNot Nothing AndAlso grid.Columns.Count > 0 Then
            Return grid.Columns.Count
        Else
            Return 0
        End If

    End Function

    ''' <summary> Displays the given grid. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    Public Function Display(ByVal grid As DataGridView) As Integer

        If grid Is Nothing Then Throw New ArgumentNullException(NameOf(grid))

        Dim wasEnabled As Boolean = grid.Enabled
        grid.Enabled = False
        grid.Enabled = wasEnabled

        If grid.DataSource Is Nothing Then
            Me.ConfigureDisplay(grid)
            Application.DoEvents()
        End If

        grid.DataSource = Me
        Application.DoEvents()
        Return grid.Columns.Count

    End Function
End Class

