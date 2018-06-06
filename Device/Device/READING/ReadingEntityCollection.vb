''' <summary> Defines the collection of <see cref="ReadingEntity">reading elements</see>. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="11/2/2013" by="David" revision=""> Created. </history>
Public Class ReadingEntityCollection
    Inherits System.Collections.ObjectModel.KeyedCollection(Of ReadingTypes, ReadingEntity)

    Protected Overrides Function GetKeyForItem(item As ReadingEntity) As ReadingTypes
        If item Is Nothing Then Throw New ArgumentNullException(NameOf(item))
        Return item.ReadingType
    End Function

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As ReadingEntityCollection)
        Me.New
        If model IsNot Nothing Then
            For Each entity As ReadingEntity In model
                Me.Add(entity)
            Next
        End If
    End Sub

    ''' <summary> Adds an item. </summary>
    ''' <param name="item"> The item. </param>
    Public Shadows Sub Add(ByVal item As ReadingEntity)
        If item Is Nothing Then Return
        MyBase.Add(item)
        ' add the length of the delimiter.
        If Me.ReadingsLength > 0 Then Me._ReadingsLength += 1
        Me._ReadingsLength += item.ReadingLength
    End Sub

    ''' <summary> Adds if reading entity type is included in the mask. </summary>
    ''' <param name="mask"> The mask. </param>
    ''' <param name="item"> The item. </param>
    Public Sub AddIf(ByVal mask As ReadingTypes, ByVal item As ReadingEntity)
        If item IsNot Nothing AndAlso (mask And item.ReadingType) <> 0 Then Me.Add(item)
    End Sub

    ''' <summary> Include unit suffix if. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub IncludeUnitSuffixIf(ByVal value As ReadingTypes)
        ' Units is a property of each element. If units are turned on, each element units is enabled.
        If (value And isr.VI.ReadingTypes.Units) <> 0 Then
            For Each e As ReadingEntity In Me
                e.IncludesUnitsSuffix = True
            Next
        End If
    End Sub

    ''' <summary> Returns the total length of the reading elements including delimiters. </summary>
    ''' <value> The length of the elements. </value>
    Public ReadOnly Property ReadingsLength() As Integer

    ''' <summary> Resets all values to null. </summary>
    Public Sub Reset()
        For Each r As ReadingEntity In Me
            r.Reset()
        Next
    End Sub

    ''' <summary> Gets the list of raw readings. </summary>
    ''' <returns> A list of raw readings. </returns>
    Public Function ToRawReadings() As String()
        Dim values As New List(Of String)
        For Each readingItem As ReadingAmount In Me
            values.Add(readingItem.ValueReading)
        Next
        Return values.ToArray
    End Function

    ''' <summary> Returns the meta status or new if does not exist. </summary>
    ''' <returns> The MetaStatus. </returns>
    Public Function MetaStatus(ByVal readingType As ReadingTypes) As MetaStatus
        Dim result As MetaStatus = New MetaStatus
        If Me.Contains(readingType) Then
            Dim amount As MeasuredAmount = TryCast(Me(readingType), MeasuredAmount)
            If amount IsNot Nothing Then result = amount.MetaStatus
        End If
        Return result
    End Function

    ''' <summary> Reading amount. </summary>
    ''' <param name="readingType"> Type of the reading. </param>
    ''' <returns> A ReadingAmount. </returns>
    Public Function ReadingAmount(ByVal readingType As ReadingTypes) As ReadingAmount
        Dim result As ReadingAmount = Nothing
        If Me.Contains(readingType) Then
            result = TryCast(Me(readingType), ReadingAmount)
        End If
        If result Is Nothing Then result = New ReadingAmount(readingType)
        Return result
    End Function

    ''' <summary> Reading status. </summary>
    ''' <param name="readingType"> Type of the reading. </param>
    ''' <returns> The ReadingStatus. </returns>
    Public Function ReadingStatus(ByVal readingType As ReadingTypes) As ReadingStatus
        Dim result As ReadingStatus = Nothing
        If Me.Contains(readingType) Then
            result = TryCast(Me(readingType), ReadingStatus)
        End If
        If result Is Nothing Then result = New ReadingStatus(readingType)
        Return result
    End Function

End Class