''' <summary> Information about the interlock. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="2/16/2016" by="David" revision="4.0.5890"> Created. </history>
Public Class InterlockInfo

    ''' <summary> Constructor. </summary>
    ''' <param name="interlockNumber"> The interlock number. </param>
    Public Sub New(ByVal interlockNumber As Integer)
        MyBase.New
        Me._Number = interlockNumber
    End Sub

    ''' <summary> Gets the interlock number.  </summary>
    ''' <value> The number. </value>
    ReadOnly Property Number As Integer

    ''' <summary> Gets the state. </summary>
    ''' <value> The state. </value>
    Public Property State As InterlockState

    ''' <summary> Gets the is engaged. </summary>
    ''' <value> The is engaged. </value>
    Public ReadOnly Property IsEngaged As Boolean
        Get
            Return Me.State = InterlockState.Engaged
        End Get
    End Property
End Class

''' <summary> Collection of interlocks. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="2/16/2016" by="David" revision="4.0.5890"> Created. </history>
Public Class InterlockCollection
    Inherits ObjectModel.KeyedCollection(Of Integer, InterlockInfo)

    ''' <summary>
    ''' When implemented in a derived class, extracts the key from the specified element.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="item"> The element from which to extract the key. </param>
    ''' <returns> The key for the specified element. </returns>
    Protected Overrides Function GetKeyForItem(item As InterlockInfo) As Integer
        If item Is Nothing Then Throw New ArgumentNullException(NameOf(item))
        Return item.Number
    End Function

    ''' <summary> Adds interlockNumber. </summary>
    ''' <param name="interlockNumber"> The interlock number to add. </param>
    ''' <returns> An InterlockInfo. </returns>
    Public Overloads Function Add(ByVal interlockNumber As Integer) As InterlockInfo
        Dim value As New InterlockInfo(interlockNumber)
        Me.Add(value)
        Return value
    End Function

    ''' <summary> Updates the interlock state described by state. </summary>
    ''' <param name="state"> The state. </param>
    Public Sub UpdateInterlockState(ByVal state As Integer)
        For Each ilock As InterlockInfo In Me
            ilock.State = If((ilock.Number And state) = ilock.Number, InterlockState.Engaged, InterlockState.Open)
        Next
    End Sub

    ''' <summary> Gets the sentinel indicating if all interlocks are engaged. </summary>
    ''' <value> The are all interlocks engaged. </value>
    Public ReadOnly Property AreAllEngaged As Boolean
        Get
            Dim affirmative As Boolean = True
            For Each ilock As InterlockInfo In Me
                affirmative = affirmative AndAlso ilock.IsEngaged
            Next
            Return affirmative
        End Get
    End Property

    ''' <summary> Gets the open interlocks. </summary>
    ''' <value> The open interlocks. </value>
    Public ReadOnly Property OpenInterlocks As IEnumerable(Of Integer)
        Get
            Dim l As New List(Of Integer)
            For Each ilock As InterlockInfo In Me
                If Not ilock.IsEngaged Then
                    l.Add(ilock.Number)
                End If
            Next
            Return l
        End Get
    End Property

End Class

''' <summary> Values that represent interlock states. </summary>
Public Enum InterlockState
    <ComponentModel.Description("Open")> [Open]
    <ComponentModel.Description("Engaged")> Engaged
End Enum
