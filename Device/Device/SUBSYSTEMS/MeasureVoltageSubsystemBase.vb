''' <summary> Defines the contract that must be implemented by a Measure Voltage Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class MeasureVoltageSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="MeasureVoltageSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Level = 0
    End Sub

#End Region

#Region " MEASURE VOLTAGE "

    ''' <summary> Waits for the voltage to exceed a minimum voltage level. </summary>
    ''' <param name="limen"> The threshold level. </param>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> <c>True</c> if level was reached before the <paramref name="timeout">timeout</paramref> expired; <c>False</c> otherwise. </returns>
    Public Function AwaitMinimumLevel(ByVal limen As Double, timeout As TimeSpan) As Boolean
        Dim endTime As DateTime = DateTime.UtcNow.Add(timeout)
        Do
            Threading.Thread.Sleep(1)
            Me.Measure()
        Loop Until limen <= Me.Level OrElse DateTime.UtcNow > endTime
        Return Me.Level.HasValue AndAlso limen <= Me.Level.Value
    End Function

    ''' <summary> Waits for the voltage to attain a level. </summary>
    ''' <param name="targetLevel"> The target level. </param>
    ''' <param name="delta">   The delta. </param>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> <c>True</c> if level was reached before the <paramref name="timeout">timeout</paramref> expired; <c>False</c> otherwise. </returns>
    Public Function AwaitLevel(ByVal targetLevel As Double, ByVal delta As Double, ByVal timeout As TimeSpan) As Boolean
        Dim endTime As DateTime = DateTime.UtcNow.Add(timeout)
        Dim hasValue As Boolean = False
        Do
            Threading.Thread.Sleep(1)
            Me.Measure()
            hasValue = Me.Level.HasValue AndAlso Math.Abs(targetLevel - Me.Level.Value) <= delta
        Loop Until hasValue OrElse DateTime.UtcNow > endTime
        Return hasValue
    End Function

    ''' <summary> The level. </summary>
    Private _Level As Double?

    ''' <summary> Gets or sets the cached voltage level. </summary>
    ''' <value> The voltage. </value>
    Public Property Level As Double?
        Get
            Return Me._Level
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Level, value) Then
                Me._Level = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets The Measure query command. </summary>
    ''' <value> The Measure query command. </value>
    Protected Overridable ReadOnly Property MeasureQueryCommand As String

    ''' <summary> Reads a value and converts it to Double. </summary>
    ''' <returns> The measured value or none if unknown. </returns>
    Public Function MeasureValue() As Double?
        Return Me.MeasureValue(Me.MeasureQueryCommand)
    End Function

    ''' <summary> Queries The reading. </summary>
    ''' <returns> The reading or none if unknown. </returns>
    Public Function Measure() As Double?
        Return Me.Measure(Me.MeasureQueryCommand)
    End Function

#End Region

End Class
