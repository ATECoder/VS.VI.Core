Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a Format Subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created. </history>
Public MustInherit Class FormatSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="FormatSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " ELEMENTS "

    ''' <summary> Builds the elements record for the specified elements. </summary>
    ''' <param name="elements"> Reading Elements. </param>
    ''' <returns> The record. </returns>
    Public Shared Function BuildRecord(ByVal elements As ReadingElements) As String
        If elements = ReadingElements.None Then
            Return String.Empty
        Else
            Dim reply As New System.Text.StringBuilder
            For Each code As Integer In [Enum].GetValues(GetType(ReadingElements))
                If (elements And code) <> 0 Then
                    Dim value As String = CType(code, ReadingElements).ExtractBetween()
                    If Not String.IsNullOrWhiteSpace(value) Then
                        If reply.Length > 0 Then reply.Append(","c)
                        reply.Append(value)
                    End If
                End If
            Next
            Return reply.ToString
        End If
    End Function

    ''' <summary> Returns the <see cref="ReadingElements"></see> from the specified value. </summary>
    ''' <param name="value"> The Elements. </param>
    ''' <returns> The reading elements. </returns>
    Public Shared Function ParseReadingElement(ByVal value As String) As ReadingElements
        If String.IsNullOrWhiteSpace(value) Then
            Return ReadingElements.None
        Else
            Dim se As New StringEnumerator(Of ReadingElements)
            Return se.ParseContained(value.BuildDelimitedValue)
        End If
    End Function

    ''' <summary> Get the composite reading elements based on the message from the instrument. </summary>
    ''' <param name="record"> Specifies the comma delimited elements record. </param>
    ''' <returns> The reading elements. </returns>
    Public Shared Function ParseReadingElements(ByVal record As String) As ReadingElements
        Dim parsed As ReadingElements = ReadingElements.None
        If Not String.IsNullOrWhiteSpace(record) Then
            For Each elementValue As String In record.Split(","c)
                parsed = parsed Or ParseReadingElement(elementValue)
            Next
        End If
        Return parsed
    End Function

    ''' <summary> Reading Elements. </summary>
    Private _Elements As ReadingElements

    ''' <summary> Gets or sets the cached Elements. </summary>
    ''' <value> A List of scans. </value>
    Public Overloads Property Elements As ReadingElements
        Get
            Return Me._Elements
        End Get
        Protected Set(ByVal value As ReadingElements)
            If Not Me.Elements.Equals(value) Then
                Me._Elements = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Elements))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Elements. </summary>
    ''' <param name="value"> The Elements. </param>
    ''' <returns> A List of scans. </returns>
    Public Function ApplyElements(ByVal value As ReadingElements) As ReadingElements
        Me.WriteElements(value)
        Return Me.QueryElements()
    End Function

    ''' <summary> Gets the elements query command. </summary>
    ''' <value> The elements query command. </value>
    ''' <remarks> SCPI Base Command: ":FORM:ELEM". </remarks>
    Protected Overridable ReadOnly Property ElementsQueryCommand As String

    ''' <summary> Queries the Elements. Also sets the <see cref="Elements">Format on</see> sentinel. </summary>
    ''' <returns> A List of scans. </returns>
    Public Function QueryElements() As ReadingElements
        Dim record As String = FormatSubsystemBase.BuildRecord(Me.Elements)
        record = Me.Query(record, Me.ElementsQueryCommand)
        Me.Elements = FormatSubsystemBase.ParseReadingElements(record)
        Return Me.Elements
    End Function

    ''' <summary> Queries the Elements for a single element. Also sets the <see cref="Elements">Format on</see> sentinel. </summary>
    ''' <returns> A List of scans. </returns>
    Public Function QueryElement() As ReadingElements
        Dim record As String = FormatSubsystemBase.BuildRecord(Me.Elements)
        record = Me.Query(record, Me.ElementsQueryCommand)
        Me.Elements = FormatSubsystemBase.ParseReadingElement(record)
        Return Me.Elements
    End Function

    ''' <summary> Gets the elements command format. </summary>
    ''' <value> The elements command format. </value>
    ''' <remarks> SCPI Base Command: ":FORM:ELEM {0}". </remarks>
    Protected Overridable ReadOnly Property ElementsCommandFormat As String

    ''' <summary> Writes the Elements for a single element. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Elements. </param>
    ''' <returns> A List of scans. </returns>
    Public Function WriteElement(ByVal value As ReadingElements) As ReadingElements
        Dim record As String = FormatSubsystemBase.BuildRecord(value)
        record = Me.Write(Me.ElementsCommandFormat, record)
        Me.Elements = FormatSubsystemBase.ParseReadingElement(record)
        Return Me.Elements
    End Function

    ''' <summary> Writes the Elements. Does not read back from the instrument. </summary>
    ''' <param name="value"> The Elements. </param>
    ''' <returns> A List of scans. </returns>
    Public Function WriteElements(ByVal value As ReadingElements) As ReadingElements
        Dim record As String = FormatSubsystemBase.BuildRecord(value)
        record = Me.Write(Me.ElementsCommandFormat, record)
        Me.Elements = FormatSubsystemBase.ParseReadingElements(record)
        Return Me.Elements
    End Function

#End Region

End Class

