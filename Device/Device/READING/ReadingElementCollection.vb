''' <summary> Defines the collection of <see cref="ReadingElement">reading elements</see>. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="11/2/2013" by="David" revision=""> Created. </history>
Public Class ReadingElementCollection
    Inherits System.Collections.ObjectModel.Collection(Of ReadingElement)

    ''' <summary> Adds an item. </summary>
    ''' <param name="item"> The item. </param>
    Public Shadows Sub Add(ByVal item As ReadingElement)
        MyBase.Add(item)
        If Me.ReadingsLength > 0 Then
            ' add the length of the delimiter.
            Me._readingsLength += 1
        End If
        If item IsNot Nothing Then
            Me._readingsLength += item.ReadingLength
        End If
    End Sub

    Private _readingsLength As Integer

    ''' <summary> Returns the total length of the reading elements including delimiters. </summary>
    ''' <value> The length of the elements. </value>
    Public ReadOnly Property ReadingsLength() As Integer
        Get
            Return Me._readingsLength
        End Get
    End Property

    ''' <summary> Resets all values to null. </summary>
    Public Sub Reset()
        For Each r As ReadingElement In Me
            r.Reset()
        Next
    End Sub

    ''' <summary> Gets the readings. </summary>
    ''' <returns> A list of readings. </returns>
    Public Function Readings() As String()

        Dim values As New List(Of String)
        For Each readingItem As ReadingAmount In Me
            values.Add(readingItem.ValueReading)
        Next
        Return values.ToArray

    End Function

End Class