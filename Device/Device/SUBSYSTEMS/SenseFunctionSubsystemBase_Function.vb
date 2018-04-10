Partial Public MustInherit Class SenseFunctionSubsystemBase

#Region " RANGE "

    ''' <summary> Gets or sets the function mode ranges. </summary>
    ''' <value> The function mode ranges. </value>
    Public ReadOnly Property FunctionModeRanges As RangeDictionary

    ''' <summary> Gets or sets the default function range. </summary>
    ''' <value> The default function range. </value>
    Public Property DefaultFunctionRange As isr.Core.Pith.RangeR = isr.Core.Pith.RangeR.Full

    ''' <summary> Converts a functionMode to a range. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an isr.Core.Pith.RangeR. </returns>
    Public Overridable Function ToRange(ByVal functionMode As Integer) As isr.Core.Pith.RangeR
        Return Me.FunctionModeRanges(functionMode)
    End Function

    Private _FunctionRange As Core.Pith.RangeR
    ''' <summary> The Range of the range. </summary>
    Public Property FunctionRange As Core.Pith.RangeR
        Get
            Return Me._FunctionRange
        End Get
        Set(value As Core.Pith.RangeR)
            If Me.FunctionRange <> value Then
                Me._FunctionRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " DECIMAL PLACES "

    ''' <summary> Gets or sets the default decimal places. </summary>
    ''' <value> The default decimal places. </value>
    Public Property DefaultFunctionModeDecimalPlaces As Integer = 3

    ''' <summary> Gets or sets the function mode decimal places. </summary>
    ''' <value> The function mode decimal places. </value>
    Public ReadOnly Property FunctionModeDecimalPlaces As IntegerDictionary

    ''' <summary> Converts a function Mode to a decimal places. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an Integer. </returns>
    Public Overridable Function ToDecimalPlaces(ByVal functionMode As Integer) As Integer
        Return Me.FunctionModeDecimalPlaces(functionMode)
    End Function

    Private _FunctionRangeDecimalPlaces As Integer

    ''' <summary> Gets or sets the function range decimal places. </summary>
    ''' <value> The function range decimal places. </value>
    Public Property FunctionRangeDecimalPlaces As Integer
        Get
            Return Me._FunctionRangeDecimalPlaces
        End Get
        Set(value As Integer)
            If Me.FunctionRangeDecimalPlaces <> value Then
                Me._FunctionRangeDecimalPlaces = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " UNIT "

    ''' <summary> Gets or sets the default unit. </summary>
    ''' <value> The default unit. </value>
    Public Property DefaultFunctionUnit As Arebis.TypedUnits.Unit = Arebis.StandardUnits.ElectricUnits.Volt

    ''' <summary> Gets or sets the function mode decimal places. </summary>
    ''' <value> The function mode decimal places. </value>
    Public ReadOnly Property FunctionModeUnits As UnitDictionary

    ''' <summary> Parse units. </summary>
    ''' <param name="functionMode"> The  Multimeter Function Mode. </param>
    ''' <returns> An Arebis.TypedUnits.Unit. </returns>
    Public Overridable Function ToUnit(ByVal functionMode As Integer) As Arebis.TypedUnits.Unit
        Return Me.FunctionModeUnits(functionMode)
    End Function

    ''' <summary> Gets or sets the function unit. </summary>
    ''' <value> The function unit. </value>
    Public Property FunctionUnit As Arebis.TypedUnits.Unit
        Get
            Return Me.Amount.Unit
        End Get
        Set(value As Arebis.TypedUnits.Unit)
            If Me.FunctionUnit <> value Then
                Me.Amount.Unit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

End Class

