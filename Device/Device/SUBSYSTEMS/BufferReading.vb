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
        Me._Reading = ""
        Me._TimestampReading = ""
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <param name="data"> The value. </param>
    Public Sub New(ByVal data As Queue(Of String))
        Me.New()
        Me._Parse(data)
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
        If data.Count >= Me.ElementCount Then
            Me._Reading = data.Dequeue
            Me._TimestampReading = data.Dequeue
        End If
    End Sub

    ''' <summary> Parses the given value. </summary>
    ''' <remarks> David, 7/23/2016. </remarks>
    ''' <param name="data"> The value. </param>
    Public Sub Parse(ByVal data As Queue(Of String))
        Me._Parse(data)
    End Sub

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
        Do While q.Count > r.ElementCount
            Me.Add(New BufferReading(q))
        Loop
    End Sub

End Class
