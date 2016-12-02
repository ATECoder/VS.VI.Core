''' <summary> Holds a single set of 27xx instrument reading elements. </summary>
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
Public Class Readings
    Inherits isr.VI.ReadingAmounts

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Constructs this class. </summary>
    Public Sub New()

        ' instantiate the base class
        MyBase.New()

        Me._Reading = New isr.VI.MeasuredAmount(ReadingTypes.Reading)
        With Me._Reading
            .Unit = Arebis.StandardUnits.ElectricUnits.Ampere
            .ComplianceLimit = VI.Scpi.Syntax.Infinity
            .HighLimit = VI.Scpi.Syntax.Infinity
            .LowLimit = VI.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 15
        End With

        Me._Channel = New isr.VI.ReadingValue(ReadingTypes.Channel)
        With Me._Channel
            .ReadingLength = 3
        End With

    End Sub

    ''' <summary> Create a copy of the model. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As Readings)
        MyBase.New(model)
        If model IsNot Nothing Then
            Me._Reading = New isr.VI.MeasuredAmount(model.Reading)
            Me._Channel = New isr.VI.ReadingValue(model.Channel)
        End If

    End Sub

    ''' <summary> Clones this class. </summary>
    ''' <returns> A copy of this object. </returns>
    Public Function Clone() As Readings
        Return New Readings(Me)
    End Function

#End Region

#Region " PARSE "

    ''' <summary> Attempts to evaluate using the applied reading and given status. </summary>
    ''' <remarks> David, 3/17/2016. </remarks>
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

    ''' <summary> Attempts to parse from the given data. </summary>
    ''' <remarks>
    ''' Parsing takes two steps. First all values are assigned. Then the status is used to evaluate
    ''' the measured amounts.
    ''' </remarks>
    ''' <param name="readings"> The readings. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function TryParse(ByVal readings As String) As Boolean
        Dim affirmative As Boolean = True
        If MyBase.TryApplyReadings(readings) Then
            Dim statusValue As Long = 0
            affirmative = Me.TryEvaluate(statusValue)
        Else
            affirmative = False
        End If
        Return affirmative
    End Function

    ''' <summary> Attempts to parse from the given data. </summary>
    ''' <remarks> David, 3/17/2016. </remarks>
    ''' <param name="values">  A queue of reading values. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function TryParse(ByVal values As Queue(Of String)) As Boolean
        Dim affirmative As Boolean = True
        If MyBase.TryApplyReadings(values) Then
            Dim statusValue As Long = 0
            affirmative = Me.TryEvaluate(statusValue)
        Else
            affirmative = False
        End If
        Return affirmative
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
    Public Shared Function ParseMulti(ByVal baseReading As Readings, ByVal readingRecords As String) As IEnumerable(Of Readings)
        Dim readingsArray As New List(Of Readings)
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
                    Dim reading As New Readings(baseReading)
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
    ''' automate parsing of these data. </remarks>
    ''' <param name="value"> The value. </param>
    Public Overrides Sub Initialize(ByVal value As isr.VI.ReadingTypes)
        MyBase.Initialize(value)
        Me.Readings.AddIf(value, Me.Reading)
        Me.Readings.AddIf(value, Me.Channel)
        Me.Readings.IncludeUnitSuffixIf(value)
    End Sub

    ''' <summary>Gets or sets the <see cref="isr.VI.ReadingAmount">channel number</see>.</summary>
    Public Property Channel() As isr.VI.ReadingValue

    ''' <summary>Gets or sets the DMM <see cref="isr.VI.MeasuredAmount">reading</see>.</summary>
    Public Property Reading() As isr.VI.MeasuredAmount

#End Region

End Class
