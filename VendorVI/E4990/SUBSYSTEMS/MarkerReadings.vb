''' <summary> Holds a single set of marker reading elements. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2008" by="David" revision="2.0.2936.x"> Create based on the 24xx
''' system classes. </history>
Public Class MarkerReadings
    Inherits isr.VI.ReadingAmounts

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Constructs this class. </summary>
    Public Sub New()

        ' instantiate the base class
        MyBase.New()

        Me._PrimaryReading = New isr.VI.MeasuredAmount(ReadingTypes.Primary)
        With Me._PrimaryReading
            .Unit = Arebis.StandardUnits.ElectricUnits.Ohm
            .ComplianceLimit = VI.Pith.Scpi.Syntax.Infinity
            .HighLimit = VI.Pith.Scpi.Syntax.Infinity
            .LowLimit = VI.Pith.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 15
        End With

        Me._SecondaryReading = New isr.VI.MeasuredAmount(ReadingTypes.Secondary)
        With Me._SecondaryReading
            .Unit = Arebis.StandardUnits.ElectricUnits.Henry
            .ComplianceLimit = VI.Pith.Scpi.Syntax.Infinity
            .HighLimit = VI.Pith.Scpi.Syntax.Infinity
            .LowLimit = VI.Pith.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 15
        End With

    End Sub

    ''' <summary> Create a copy of the model. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As MarkerReadings)
        MyBase.New(model)
        If model IsNot Nothing Then
            Me._PrimaryReading = New isr.VI.MeasuredAmount(model.PrimaryReading)
            Me._SecondaryReading = New isr.VI.MeasuredAmount(model.SecondaryReading)
        End If

    End Sub

    ''' <summary> Clones this class. </summary>
    ''' <returns> A copy of this object. </returns>
    Public Function Clone() As MarkerReadings
        Return New MarkerReadings(Me)
    End Function

#End Region

#Region " PARSE "

    ''' <summary> Attempts to evaluate using the applied reading and given status. </summary>
    ''' <param name="status"> The status. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Overrides Function TryEvaluate(ByVal status As Long) As Boolean
        Dim metaStatus As New MetaStatus
        metaStatus.Preset(status)
        If status > 0 Then
            ' update the meta status based on the status reading.
        End If
        Return MyBase.TryEvaluate(metaStatus.Value)
    End Function

    ''' <summary> Parses reading data into a readings array. </summary>
    ''' <param name="baseReading">    Specifies the base reading which includes the limits for all
    ''' reading elements. </param>
    ''' <param name="readingRecords"> The reading records. </param>
    ''' <returns> Readings[][]. </returns>
    ''' <exception cref="System.ArgumentNullException"> readingRecords, baseReading. </exception>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification:="Object is returned")>
    <CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Multi")>
    Public Shared Function ParseMulti(ByVal baseReading As MarkerReadings, ByVal readingRecords As String) As MarkerReadings()
        Dim readingsArray As New List(Of MarkerReadings)
        If readingRecords Is Nothing Then
            Throw New ArgumentNullException(NameOf(readingRecords))
        ElseIf baseReading Is Nothing Then
            Throw New ArgumentNullException(NameOf(baseReading))
        ElseIf baseReading.Readings Is Nothing Then
            Throw New InvalidOperationException("Base reading readings not defined")
        ElseIf baseReading.Readings.Count = 0 Then
            Throw New InvalidOperationException("Base reading has not readings")
        ElseIf readingRecords.Length > 0 Then
            Dim values As New Queue(Of String)(readingRecords.Split(ReadingAmounts.DefaultDelimiter.ToCharArray))
            If values.Count >= baseReading.Readings.Count Then
                For j As Integer = 0 To values.Count \ baseReading.Readings.Count - 1
                    Dim reading As New MarkerReadings(baseReading)
                    reading.TryParse(values)
                    readingsArray.Add(reading)
                Next
            End If
        End If
        Return readingsArray.ToArray

    End Function

#End Region

#Region " READING ELEMENTS "

    ''' <summary> Initializes this object. </summary>
    ''' <remarks> Adds reading elements in the order they are returned by the instrument so as to
    ''' automate parsing of these data. 
    ''' This could be exp[anded for this instrument to read the status and buffer name. </remarks>
    ''' <param name="value"> The value. </param>
    Public Overrides Sub Initialize(ByVal value As VI.ReadingTypes)
        MyBase.Initialize(value)
        Me.Readings.AddIf(value, Me.PrimaryReading)
        Me.Readings.AddIf(value, Me.SecondaryReading)
        Me.Readings.IncludeUnitSuffixIf(value)
    End Sub

    ''' <summary>Gets or sets the <see cref="isr.VI.MeasuredAmount">primary reading</see>.</summary>
    Public Property PrimaryReading() As VI.MeasuredAmount

    ''' <summary>Gets or sets the <see cref="isr.VI.MeasuredAmount">Secondary reading</see>.</summary>
    Public Property SecondaryReading() As VI.MeasuredAmount

#End Region

End Class
