''' <summary>
''' A payload consisting of a single real value of <see cref="T:Double"/> type.
''' </summary>
''' <remarks></remarks>
Public Class RealValuePayload
    Inherits VI.Pith.PayloadBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary>
    ''' Constructor for this class.
    ''' </summary>
    Public Sub New()
        MyBase.New()
        Me.Range = isr.Core.Pith.RangeR.Full
        Me.PositiveOverflow = VI.Pith.Scpi.Syntax.Infinity
        Me.NegativeOverflow = VI.Pith.Scpi.Syntax.NegativeInfinity
        Me.Format = "0.00"
        Me.Unit = Arebis.StandardUnits.ElectricUnits.Volt
    End Sub

#End Region

#Region " SIMULATE "

    ''' <summary>
    ''' Returns an emulated value.
    ''' </summary>
    Public Shared Function SimulateValue(ByVal range As isr.Core.Pith.RangeR) As Double
        If range Is Nothing Then Throw New ArgumentNullException(NameOf(range))
        Dim rand As New Random(Now.Second)
        Return range.Min + range.Span * rand.NextDouble
    End Function

    ''' <summary> Gets the simulated value. </summary>
    ''' <value> The simulated value. </value>
    Public ReadOnly Property SimulatedValue As Double
        Get
            Return RealValuePayload.SimulateValue(Me.Range)
        End Get
    End Property

    ''' <summary> Gets the simulated payload. </summary>
    ''' <value> The simulated payload. </value>
    Public Overrides ReadOnly Property SimulatedPayload As String
        Get
            Return String.Format(Format, RealValuePayload.SimulateValue(Me.Range))
        End Get
    End Property

#End Region

#Region " RANGE "

    ''' <summary> Gets or sets the range. </summary>
    ''' <value> The range. </value>
    Public Property Range As isr.Core.Pith.RangeR

    ''' <summary> Gets or sets the positive overflow. </summary>
    ''' <value> The positive overflow. </value>
    Public Property PositiveOverflow As Double

    ''' <summary> Gets or sets the negative overflow. </summary>
    ''' <value> The negative overflow. </value>
    Public Property NegativeOverflow As Double

#End Region

#Region " QUERY "

    ''' <summary> Gets or sets the real value unit. </summary>
    ''' <value> The real value. </value>
    Public Property Unit As Arebis.TypedUnits.Unit

    ''' <summary> Gets or sets the analog input read. </summary>
    ''' <value> The analog input read. </value>
    Public Property Amount As Arebis.TypedUnits.Amount

    Private _RealValue As Double
    ''' <summary>
    ''' Gets or sets the value
    ''' </summary>
    Public Property RealValue As Double
        Get
            Return Me._RealValue
        End Get
        Set(value As Double)
            If value <> Me.RealValue Then
                Me._RealValue = value
                Me.Amount = New Arebis.TypedUnits.Amount(value, Unit)
            End If
        End Set
    End Property

    ''' <summary> Gets the real value caption. </summary>
    ''' <value> The real value caption. </value>
    Public ReadOnly Property RealValueCaption As String
        Get
            Return Me.Amount.ToString(Format)
        End Get
    End Property

    ''' <summary> Gets or sets the format to use. </summary>
    ''' <value> The format. </value>
    Public Property Format As String

    ''' <summary> Converts the reading to the specific payload such as a read number. </summary>
    ''' <param name="reading"> The reading. </param>
    Public Overrides Sub FromReading(reading As String)
        MyBase.FromReading(reading)
        If Double.TryParse(reading, Me.RealValue) Then
            Me.QueryStatus = Me.QueryStatus Or VI.Pith.PayloadStatus.QueryParsed And Not VI.Pith.PayloadStatus.QueryParseFailed
        Else
            Me.QueryStatus = Me.QueryStatus Or VI.Pith.PayloadStatus.QueryParseFailed And Not VI.Pith.PayloadStatus.QueryParsed And Not VI.Pith.PayloadStatus.Okay
        End If
    End Sub

#End Region

#Region " WRITE "

    ''' <summary>
    ''' Converts the specific payload value to a <see cref="P:isr.VI.PayloadBase.Writing">value</see>
    ''' to send to the session.
    ''' </summary>
    ''' <returns> A String. </returns>
    Public Overrides Function FromValue() As String
        Me.Writing = String.Format(Me.Format, Me.RealValue)
        Return Me.Writing
    End Function

#End Region

End Class

