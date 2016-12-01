Imports isr.VI

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

    ''' <summary> Clears this object to its blank/initial state. </summary>
    ''' <remarks> David, 8/11/2016. </remarks>
    Private Sub _Clear()
        Me._Reading = ""
        Me._TimestampReading = ""
        Me._Timestamp = DateTime.MinValue
        Me._FractionalSecond = 0
        Me._FractionalTimestamp = TimeSpan.Zero
        Me.RelativeTimespan = TimeSpan.Zero
    End Sub

    ''' <summary> Gets or sets the reading. </summary>
    ''' <value> The reading. </value>
    Public ReadOnly Property Reading As String

    Public ReadOnly Property TimestampReading As String

    ''' <summary> Gets the number of elements. </summary>
    ''' <value> The number of elements. </value>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Public ReadOnly Property ElementCount As Integer
        Get
            Return 2
        End Get
    End Property

    ''' <summary> Parses the given value. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <param name="data"> The value. </param>
    Private Sub _Parse(ByVal data As Queue(Of String))
        If data Is Nothing Then Throw New ArgumentNullException(NameOf(data))
        Me._Clear()
        If data.Count >= Me.ElementCount Then
            Me._Reading = data.Dequeue
            Me._TimestampReading = data.Dequeue
            Me.ParseTimestamp(Me._TimestampReading)
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

#Region " TIMESTAMP "

    ''' <summary> Parse timestamp. </summary>
    ''' <remarks> David, 8/11/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="timestamp"> The time stamp rounded down to the second. </param>
    Private Sub ParseTimestamp(ByVal timestamp As String)
        If String.IsNullOrWhiteSpace(timestamp) Then Throw New ArgumentNullException(NameOf(timestamp))
        Dim q As New Queue(Of String)(timestamp.Split("."c))
        Me._Timestamp = DateTime.Parse(q.Dequeue)
        Me._FractionalSecond = Double.Parse($".{q.Dequeue}")
        Me._FractionalTimestamp = TimeSpan.FromTicks(CLng(TimeSpan.TicksPerSecond * Me._FractionalSecond))
    End Sub

    ''' <summary> Gets the time stamp. </summary>
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

    ''' <summary> Gets or sets the timespan relative tot eh first reading. </summary>
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
    Inherits List(Of BufferReading)

    ''' <summary> Parses. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="data"> The data. </param>
    Public Sub Parse(ByVal data As String)
        If data Is Nothing Then Throw New ArgumentNullException(NameOf(data))
        Me.Clear()
        Dim q As New Queue(Of String)(data.Split(","c))
        Dim r As BufferReading = New BufferReading
        Dim firstItem As BufferReading = Nothing
        Do While q.Count >= r.ElementCount
            Me.Add(New BufferReading(q, firstItem))
            If Me.Count = 1 Then firstItem = Me.Item(0)
        Loop
    End Sub

    ''' <summary> Gets or sets the timestamp. </summary>
    ''' <value> The timestamp of the first sample rounded down to the second. </value>
    Public ReadOnly Property Timestamp As DateTime

    ''' <summary> Gets or sets the relative timestamp. </summary>
    ''' <value>
    ''' The relative timespan of the first sample to provide accurate fractional seconds form the
    ''' initial time stamp.
    ''' </value>
    Public ReadOnly Property RelativeTimespan As TimeSpan

End Class
