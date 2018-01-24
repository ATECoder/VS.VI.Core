''' <summary> Defines the contract that must be implemented by a Sense Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific ReSenses, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SenseSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.FunctionRange = Me.DefaultFunctionRange ' isr.Core.Pith.RangeR.Full
        Me.FunctionUnit = Me.DefaultFunctionUnit ' Arebis.StandardUnits.ElectricUnits.Volt
        Me.FunctionRangeDecimalPlaces = Me.DefaultFunctionRangeDecimalPlaces ' 3
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.MeasurementAvailable = False
    End Sub

#End Region

#Region " FETCH; DATA; READ "

    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property LastReading As String

    ''' <summary> Parses the reading into the data elements. </summary>
    Public MustOverride Sub ParseReading(ByVal reading As String)

    ''' <summary> Fetches the latest data </summary>
    Public MustOverride Sub FetchLatestData()

    ''' <summary> <c>True</c> if Measurement available. </summary>
    Private _MeasurementAvailable As Boolean

    ''' <summary> Gets or sets a value indicating whether [Measurement available]. </summary>
    ''' <value> <c>True</c> if [Measurement available]; otherwise, <c>False</c>. </value>
    Public Property MeasurementAvailable As Boolean
        Get
            Return Me._MeasurementAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            Me._MeasurementAvailable = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property

#End Region

#Region " FUNCTION RANGE AND UNIT "

    ''' <summary> Gets or sets the default function unit. </summary>
    ''' <value> The default function unit. </value>
    Public Property DefaultFunctionUnit As Arebis.TypedUnits.Unit = Arebis.StandardUnits.ElectricUnits.Volt

    ''' <summary> Converts a Function Mode to a unit. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an Arebis.TypedUnits.Unit. </returns>
    MustOverride Function ToUnit(ByVal functionMode As Integer) As Arebis.TypedUnits.Unit

    ''' <summary> Gets or sets the default function range. </summary>
    ''' <value> The default function range. </value>
    Public Property DefaultFunctionRange As isr.Core.Pith.RangeR = isr.Core.Pith.RangeR.Full

    ''' <summary> Converts a function Mode to a range. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an isr.Core.Pith.RangeR. </returns>
    MustOverride Function ToRange(ByVal functionMode As Integer) As isr.Core.Pith.RangeR

    Public Property DefaultFunctionRangeDecimalPlaces As Integer = 3

    ''' <summary> Converts a functionMode to a decimal places. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an Integer. </returns>
    MustOverride Function ToDecimalPlaces(ByVal functionMode As Integer) As Integer

    Private _FunctionRange As Core.Pith.RangeR
    ''' <summary> The range of the function range. </summary>
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

    Private _FunctionUnit As Arebis.TypedUnits.Unit

    ''' <summary> Gets or sets the function unit. </summary>
    ''' <value> The function unit. </value>
    Public Property FunctionUnit As Arebis.TypedUnits.Unit
        Get
            Return Me._FunctionUnit
        End Get
        Set(value As Arebis.TypedUnits.Unit)
            If Me.FunctionUnit <> value Then
                Me._FunctionUnit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

End Class

