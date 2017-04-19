Imports isr.Core.Controls.DataGridViewExtensions
''' <summary> Defines a Trace Subsystem for a Keithley 2700 instrument. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class TraceSubsystem
    Inherits VI.Scpi.TraceSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="TraceSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.PointsCount = 10
        Me.FeedSource = VI.Scpi.FeedSource.None
        Me.FeedControl = VI.Scpi.FeedControl.Never
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

    ''' <summary> Gets or sets the Clear Buffer command. </summary>
    ''' <value> The ClearBuffer command. </value>
    Protected Overrides ReadOnly Property ClearBufferCommand As String = ":TRAC:CLE"

    ''' <summary> Gets the points count query command. </summary>
    ''' <value> The points count query command. </value>
    Protected Overrides ReadOnly Property PointsCountQueryCommand As String = ":TRAC:POIN:COUN?"

    ''' <summary> Gets the points count command format. </summary>
    ''' <value> The points count command format. </value>
    Protected Overrides ReadOnly Property PointsCountCommandFormat As String = ":TRAC:POIN:COUN {0}"

    ''' <summary> Gets the feed Control query command. </summary>
    ''' <value> The write feed Control query command. </value>
    Protected Overrides ReadOnly Property FeedControlQueryCommand As String = ":TRAC:FEED:CONTROL?"

    ''' <summary> Gets the feed Control command format. </summary>
    ''' <value> The write feed Control command format. </value>
    Protected Overrides ReadOnly Property FeedControlCommandFormat As String = ":TRAC:FEED:CONTROL {0}"

    ''' <summary> Gets the feed source query command. </summary>
    ''' <value> The write feed source query command. </value>
    Protected Overrides ReadOnly Property FeedSourceQueryCommand As String = ":TRAC:FEED?"

    ''' <summary> Gets the feed source command format. </summary>
    ''' <value> The write feed source command format. </value>
    Protected Overrides ReadOnly Property FeedSourceCommandFormat As String = ":TRAC:FEED {0}"

#End Region

#Region " TRACE READINGS "

    ''' <summary> Queries the current Data. </summary>
    ''' <returns> The Data or empty if none. </returns>
    Public Overloads Function QueryReadings(ByVal baseReading As Readings) As IEnumerable(Of Readings)
        Dim values As String = MyBase.QueryData()
        Return Readings.ParseMulti(baseReading, values)
    End Function

    ''' <summary> Displays the readings described by values. </summary>
    ''' <remarks> David, 12/1/2016. </remarks>
    ''' <param name="values"> The values. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Shared Sub DisplayReadings(ByVal grid As Windows.Forms.DataGridView, ByVal values As IEnumerable(Of Readings))
        If grid Is Nothing Then Throw New ArgumentNullException(NameOf(grid))
        With grid
            .DataSource = Nothing
            .Columns.Clear()
            .AutoGenerateColumns = False
            .AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGreen
            .AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader
            .BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            .ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            .AllowUserToResizeColumns = True
            .EnableHeadersVisualStyles = True
            .MultiSelect = False
            .RowHeadersBorderStyle = Windows.Forms.DataGridViewHeaderBorderStyle.Raised
            .DataSource = values
            Dim displayIndex As Integer = 0
            Dim column As New Windows.Forms.DataGridViewTextBoxColumn()
            With column
                .DataPropertyName = NameOf(Readings.RawReading)
                .Name = NameOf(Readings.RawReading)
                .Visible = True
                .DisplayIndex = displayIndex
                .HeaderText = "Reading"
            End With
            .Columns.Add(column)
            displayIndex += 1
            For Each c As Windows.Forms.DataGridViewColumn In .Columns
                c.AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
            Next
            .ParseHeaderText()
            .ScrollBars = Windows.Forms.ScrollBars.Both
        End With
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
' part of trigger subsystem.
' 

    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    Protected Overrides ReadOnly Property InitiateCommand As String = ":INIT"

    ''' <summary> Gets the Abort command. </summary>
    ''' <value> The Abort command. </value>
    Protected Overrides ReadOnly Property AbortCommand As String = ":ABOR"

#End If
#End Region