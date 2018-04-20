Imports isr.Core.Pith.NumericExtensions
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

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Constructs this class. </summary>
    Public Sub New()

        ' instantiate the base class
        MyBase.New()

        Me._VoltageReading = New isr.VI.MeasuredAmount(ReadingTypes.Voltage)
        With Me._VoltageReading
            .Unit = Arebis.StandardUnits.ElectricUnits.Volt
            .ComplianceLimit = VI.Pith.Scpi.Syntax.Infinity
            .HighLimit = VI.Pith.Scpi.Syntax.Infinity
            .LowLimit = VI.Pith.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 13
        End With

        Me._CurrentReading = New isr.VI.MeasuredAmount(ReadingTypes.Current)
        With Me._CurrentReading
            .Unit = Arebis.StandardUnits.ElectricUnits.Ampere
            .ComplianceLimit = VI.Pith.Scpi.Syntax.Infinity
            .HighLimit = VI.Pith.Scpi.Syntax.Infinity
            .LowLimit = VI.Pith.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 13
        End With

        Me._ResistanceReading = New isr.VI.MeasuredAmount(ReadingTypes.Resistance)
        With Me._ResistanceReading
            .Unit = Arebis.StandardUnits.ElectricUnits.Ohm
            .ComplianceLimit = VI.Pith.Scpi.Syntax.Infinity
            .HighLimit = VI.Pith.Scpi.Syntax.Infinity
            .LowLimit = VI.Pith.Scpi.Syntax.NegativeInfinity
            .ReadingLength = 13
        End With

        Me._Timestamp = New isr.VI.ReadingAmount(ReadingTypes.Time)
        With Me._Timestamp
            .Unit = Arebis.StandardUnits.TimeUnits.Second
            .ReadingLength = 13
        End With

        Me._StatusReading = New isr.VI.ReadingStatus(ReadingTypes.Status)
        With Me._StatusReading
            .ReadingLength = 13
        End With

    End Sub

    ''' <summary> Create a copy of the model. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As Readings)
        MyBase.New(model)
        If model IsNot Nothing Then
            Me._CurrentReading = New isr.VI.MeasuredAmount(model.CurrentReading)
            Me._ResistanceReading = New isr.VI.MeasuredAmount(model.ResistanceReading)
            Me._VoltageReading = New isr.VI.MeasuredAmount(model.VoltageReading)
            Me._Timestamp = New isr.VI.ReadingAmount(model.Timestamp)
            Me._StatusReading = New isr.VI.ReadingStatus(model.StatusReading)
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
        Me.Readings.AddIf(value, Me.VoltageReading)
        Me.Readings.AddIf(value, Me.CurrentReading)
        Me.Readings.AddIf(value, Me.ResistanceReading)
        Me.Readings.AddIf(value, Me.Timestamp)
        Me.Readings.AddIf(value, Me.StatusReading)
        Me.Readings.IncludeUnitSuffixIf(value)
    End Sub

    ''' <summary>Gets or sets the source meter <see cref="isr.VI.MeasuredAmount">current reading</see>.</summary>
    Public Property CurrentReading() As VI.MeasuredAmount

    ''' <summary>Gets or sets the source meter <see cref="isr.VI.MeasuredAmount">resistance reading</see>.</summary>
    Public Property ResistanceReading() As VI.MeasuredAmount

    ''' <summary>Gets or sets the source meter <see cref="isr.VI.MeasuredAmount">voltage reading</see>.</summary>
    Public Property VoltageReading() As VI.MeasuredAmount

    ''' <summary>Gets or sets the timestamp <see cref="isr.VI.ReadingAmount">reading</see>.</summary>
    Public Property Timestamp() As VI.ReadingAmount

    ''' <summary>Gets or sets the source meter <see cref="isr.VI.MeasuredAmount">status reading</see>.</summary>
    Public Property StatusReading() As VI.ReadingStatus

#End Region

End Class

