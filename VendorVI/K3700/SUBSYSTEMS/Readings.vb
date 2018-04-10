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
            .ComplianceLimit = VI.Pith.Scpi.Syntax.Infinity
            .HighLimit = VI.Pith.Scpi.Syntax.Infinity
            .LowLimit = VI.Pith.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 15
        End With

        Me._Timestamp = New isr.VI.ReadingAmount(ReadingTypes.Timestamp)
        With Me._Timestamp
            .Unit = Arebis.StandardUnits.TimeUnits.Second
            .ReadingLength = 7
        End With

        Me._ReadingNumber = New isr.VI.ReadingValue(ReadingTypes.ReadingNumber)
        With Me._ReadingNumber
            .ReadingLength = 4
        End With

        Me._Channel = New isr.VI.ReadingValue(ReadingTypes.Channel)
        With Me._Channel
            .ReadingLength = 3
        End With

        Me._Limits = New isr.VI.ReadingValue(ReadingTypes.Limits)
        With Me._Limits
            .ReadingLength = 4
        End With
        ' Units is a property of each element. If units are turned on, each element units is enabled.
        ' Me._Units = New isr.VI.ReadingElement() : Me._Units.ReadingLength = 4

    End Sub

    ''' <summary> Create a copy of the model. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As Readings)
        MyBase.New(model)
        If model IsNot Nothing Then
            Me._Reading = New isr.VI.MeasuredAmount(model.Reading)
            Me._Timestamp = New isr.VI.ReadingAmount(model.Timestamp)
            Me._ReadingNumber = New isr.VI.ReadingValue(model.ReadingNumber)
            Me._Channel = New isr.VI.ReadingValue(model.Channel)
            Me._Limits = New isr.VI.ReadingValue(model.Limits)
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
    ''' <param name="status"> The status. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Overrides Function TryEvaluate(ByVal status As Long) As Boolean
        Dim metaStatus As New MetaStatus
        metaStatus.Preset(status)
        If status > 0 Then
            ' update the meta status based on the status reading.
            ' TO_DO: Get the status word from this instrument and use code from 2400 reading to set the bits.
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
    Public Shared Function ParseMulti(ByVal baseReading As Readings, ByVal readingRecords As String) As Readings()
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
    Public Overrides Sub Initialize(ByVal value As VI.ReadingTypes)
        MyBase.Initialize(value)
        Me.Readings.AddIf(value, Me.Reading)
        Me.Readings.AddIf(value, Me.Timestamp)
        Me.Readings.AddIf(value, Me.ReadingNumber)
        Me.Readings.AddIf(value, Me.Channel)
        Me.Readings.AddIf(value, Me.Limits)
        Me.Readings.IncludeUnitSuffixIf(value)
    End Sub

    ''' <summary>Gets or sets the <see cref="isr.VI.ReadingAmount">channel number</see>.</summary>
    Public Property Channel() As VI.ReadingValue

    ''' <summary>Gets or sets the <see cref="isr.VI.ReadingAmount">limits</see>.</summary>
    Public Property Limits() As VI.ReadingValue

    ''' <summary>Gets or sets the DMM <see cref="isr.VI.MeasuredAmount">reading</see>.</summary>
    Public Property Reading() As VI.MeasuredAmount

    ''' <summary>Gets or sets the <see cref="isr.VI.ReadingAmount">reading number</see>.</summary>
    Public Property ReadingNumber() As VI.ReadingValue

    ''' <summary>Gets or sets the timestamp <see cref="isr.VI.ReadingAmount">reading</see>.</summary>
    Public Property Timestamp() As VI.ReadingAmount

#End Region

End Class
