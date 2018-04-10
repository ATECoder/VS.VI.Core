Imports System.Drawing
Imports System.Windows.Forms
Imports isr.Core.Controls.DataGridViewExtensions
''' <summary> A bridge meter resistor. </summary>
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
Public Class BridgeMeterResistor
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
    Public Sub New(ByVal title As String, ByVal channelList As String)
        MyBase.New()
        Me.Title = title
        Me.ChannelList = channelList
    End Sub
    Public ReadOnly Property Title As String
    Public ReadOnly Property ChannelList As String
    Public Property Resistance As Double
End Class

''' <summary> Bridge meter Resistor Collection: an ordered collection of resistors. </summary>
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
Public Class BridgeMeterResistorCollection
    Inherits ObjectModel.KeyedCollection(Of String, BridgeMeterResistor)

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New
        Me.AddResistor("R1", My.Settings.R1ChannelList)
        Me.AddResistor("R2", My.Settings.R2ChannelList)
        Me.AddResistor("R3", My.Settings.R3ChannelList)
        Me.AddResistor("R4", My.Settings.R4ChannelList)
    End Sub

    ''' <summary>
    ''' When implemented in a derived class, extracts the key from the specified element.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="item"> The element from which to extract the key. </param>
    ''' <returns> The key for the specified element. </returns>
    Protected Overrides Function GetKeyForItem(item As BridgeMeterResistor) As String
        If item Is Nothing Then Throw New ArgumentNullException(NameOf(item))
        Return item.Title
    End Function

    ''' <summary> Adds a new resistor. </summary>
    ''' <param name="title">       The title. </param>
    ''' <param name="channelList"> List of channels. </param>
    Public Sub AddResistor(ByVal title As String, ByVal channelList As String)
        Me.Add(New BridgeMeterResistor(title, channelList))
    End Sub

    ''' <summary> Configure display values. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    ''' <returns> An Integer. </returns>
    Public Function ConfigureDisplayValues(ByVal grid As DataGridView) As Integer

        If grid Is Nothing Then Throw New ArgumentNullException("grid")

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
                .DataPropertyName = "Title"
                .Name = "Title"
                .Visible = True
                .DisplayIndex = displayIndex
            End With
            grid.Columns.Add(column)
            width += column.Width

            displayIndex += 1
            column = New DataGridViewTextBoxColumn()
            With column
                .DataPropertyName = "Resistance"
                .Name = "Resistance"
                .Visible = True
                .DisplayIndex = displayIndex
                .Width = grid.Width - width
                .DefaultCellStyle.Format = "G5"
            End With
        Catch
            If column IsNot Nothing Then column.Dispose()
            Throw
        End Try
        grid.Columns.Add(column)
        grid.ParseHeaderText()
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

        If grid Is Nothing Then Throw New ArgumentNullException("grid")

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

        If grid Is Nothing Then Throw New ArgumentNullException("grid")

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
        grid.ParseHeaderText()
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

        If grid Is Nothing Then Throw New ArgumentNullException("grid")

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

