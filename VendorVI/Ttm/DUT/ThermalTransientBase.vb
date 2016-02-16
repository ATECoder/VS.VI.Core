Imports isr.Core.Pith.NumericExtensions
''' <summary> Defines a measured thermal transient resistance and voltage. </summary>
''' <license> (c) 2009 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="8/5/2013" by="David" revision="">                  Created. </history>
''' <history date="02/02/2009" by="David" revision="2.1.3320.x"> Created. </history>
Public MustInherit Class ThermalTransientBase
    Inherits ResistanceMeasureBase
    Implements System.IEquatable(Of ThermalTransientBase)

#Region " CONSTRUCTORS  AND  CLONES "

    ''' <summary> Default constructor. </summary>
    Protected Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Clones an existing measurement. </summary>
    ''' <param name="value"> The value. </param>
    Protected Sub New(ByVal value As ThermalTransientBase)
        MyBase.New(value)
        If value IsNot Nothing Then
            ' Configuration
            Me._AllowedVoltageChange = value.AllowedVoltageChange
            Me._MedianFilterSize = value.MedianFilterSize
            Me._PostTransientDelay = value.PostTransientDelay
            Me._SamplingInterval = value.SamplingInterval
            Me._TracePoints = value.TracePoints
        End If
    End Sub


    ''' <summary> Copies the configuration described by value. </summary>
    ''' <param name="value"> The value. </param>
    Public Overloads Sub CopyConfiguration(value As ThermalTransientBase)
        MyBase.CopyConfiguration(value)
        If value IsNot Nothing Then
            Me._AllowedVoltageChange = value.AllowedVoltageChange
            Me._MedianFilterSize = value.MedianFilterSize
            Me._PostTransientDelay = value.PostTransientDelay
            Me._SamplingInterval = value.SamplingInterval
            Me._TracePoints = value.TracePoints
        End If
    End Sub

#End Region

#Region " PRESET "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        With My.MySettings.Default
            Me.Aperture = .ThermalTransientApertureDefault
            Me.CurrentLevel = .ThermalTransientCurrentLevelDefault
            Me.LowLimit = .ThermalTransientLowLimitDefault
            Me.HighLimit = .ThermalTransientHighLimitDefault
            Me.VoltageLimit = .ThermalTransientVoltageLimitDefault

            Me.AllowedVoltageChange = .ThermalTransientVoltageChangeDefault
            Me.MedianFilterSize = .ThermalTransientMedianFilterLengthDefault
            Me.PostTransientDelay = .PostTransientDelayDefault
            Me.SamplingInterval = .ThermalTransientSamplingIntervalDefault
            Me.TracePoints = .ThermalTransientTracePointsDefault
        End With
    End Sub

#End Region

#Region " EQUALS "

    ''' <summary> Indicates whether the current <see cref="T:ThermalTransientBase"></see> value is equal to a
    ''' specified object. </summary>
    ''' <param name="obj"> An object. </param>
    ''' <returns> <c>True</c> if <paramref name="obj" /> and this instance are the same type and represent the
    ''' same value; otherwise, <c>False</c>. </returns>
    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        Return obj IsNot Nothing AndAlso (Object.ReferenceEquals(Me, obj) OrElse ThermalTransientBase.Equals(Me, TryCast(obj, ThermalTransientBase)))
    End Function

    ''' <summary> Indicates whether the current <see cref="T:ThermalTransientBase"></see> value is
    ''' equal to a specified object. </summary>
    ''' <param name="other"> The cold resistance to compare to this object. </param>
    ''' <returns> <c>True</c> if the <paramref name="other" /> parameter and this instance are the same type and represent the
    ''' same value; otherwise, <c>False</c>. </returns>
    Public Overloads Function Equals(ByVal other As ThermalTransientBase) As Boolean Implements System.IEquatable(Of ThermalTransientBase).Equals
        Return other IsNot Nothing AndAlso
            Me.Reading.Equals(other.Reading) AndAlso
            Me.ConfigurationEquals(other) AndAlso
            True
    End Function

    ''' <summary> Check throw unequal configuration. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="other"> The thermal transient configuration to compare to this object. </param>
    Public Overloads Sub CheckThrowUnequalConfiguration(ByVal other As ThermalTransientBase)
        If other IsNot Nothing Then
            MyBase.CheckThrowUnequalConfiguration(other)
            If Not Me.ConfigurationEquals(other) Then
                Dim format As String = "Unequal configuring--instrument {0}={1}.NE.{2}"
                If Not Me.AllowedVoltageChange.Approximates(other.AllowedVoltageChange, 0.0001) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "Allowed Voltage Change", Me.AllowedVoltageChange, other.AllowedVoltageChange))
                ElseIf Not Me.MedianFilterSize.Equals(other.MedianFilterSize) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "Median Filter Size", Me.MedianFilterSize, other.MedianFilterSize))
                ElseIf Not Me.PostTransientDelay.Approximates(other.PostTransientDelay, 0.001) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "Post Transient Delay", Me.PostTransientDelay, other.PostTransientDelay))
                ElseIf Not Me.SamplingInterval.Approximates(other.SamplingInterval, 0.000001) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "Sampling Interval", Me.SamplingInterval, other.SamplingInterval))
                ElseIf Not Me.TracePoints.Equals(other.TracePoints) Then
                    Throw New OperationFailedException(String.Format(Globalization.CultureInfo.CurrentCulture, format, "Trace Points", Me.TracePoints, other.TracePoints))
                Else
                    Debug.Assert(Not Debugger.IsAttached, "Failed logic")
                End If
            End If
        End If
    End Sub


    ''' <summary> Indicates whether the current <see cref="T:ThermalTransientBase"></see> configuration values are
    ''' equal to a specified object. </summary>
    ''' <param name="other"> The cold resistance to compare to this object. </param>
    ''' <returns> <c>True</c> if the other parameter is equal to the current
    ''' <see cref="T:ThermalTransientBase"></see> value;
    ''' otherwise, <c>False</c>. </returns>
    Public Overloads Function ConfigurationEquals(ByVal other As ThermalTransientBase) As Boolean
        Return other IsNot Nothing AndAlso MyBase.ConfigurationEquals(other) AndAlso
            Me.AllowedVoltageChange.Approximates(other.AllowedVoltageChange, 0.001) AndAlso
            Me.MedianFilterSize.Equals(other.MedianFilterSize) AndAlso
            Me.PostTransientDelay.Approximates(other.PostTransientDelay, 0.001) AndAlso
            Me.SamplingInterval.Approximates(other.SamplingInterval, 0.000001) AndAlso
            Me.TracePoints.Equals(other.TracePoints) AndAlso
            True
    End Function

    ''' <summary> Returns a hash code for this instance. </summary>
    ''' <returns> A hash code for this object. </returns>
    Public Overloads Overrides Function GetHashCode() As Int32
        Return MyBase.GetHashCode
    End Function

    ''' <summary> Implements the operator =. </summary>
    ''' <param name="left">  The left. </param>
    ''' <param name="right"> The right. </param>
    ''' <returns> The result of the operation. </returns>
    Public Shared Operator =(ByVal left As ThermalTransientBase, ByVal right As ThermalTransientBase) As Boolean
        Return ((left Is Nothing) AndAlso (right Is Nothing)) OrElse (left IsNot Nothing) AndAlso left.Equals(right)
    End Operator

    ''' <summary> Implements the operator &lt;&gt;. </summary>
    ''' <param name="left">  The left. </param>
    ''' <param name="right"> The right. </param>
    ''' <returns> The result of the operation. </returns>
    Public Shared Operator <>(ByVal left As ThermalTransientBase, ByVal right As ThermalTransientBase) As Boolean
        Return ((left Is Nothing) AndAlso (right IsNot Nothing)) OrElse Not ((left IsNot Nothing) AndAlso left.Equals(right))
    End Operator

#End Region

#Region " CONFIGURATION PROPERTIES "

    Private _AllowedVoltageChange As Double
    ''' <summary> Gets or sets the maximum expected transient voltage. </summary>
    ''' <value> The allowed voltage change. </value>
    Public Property AllowedVoltageChange() As Double
        Get
            Return Me._AllowedVoltageChange
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.AllowedVoltageChange) Then
                Me._AllowedVoltageChange = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> The Median Filter Size. </summary>
    Private _MedianFilterSize As Integer

    ''' <summary> Gets or sets the cached Median Filter Size. </summary>
    ''' <value> The Median Filter Size. </value>
    Public Overloads Property MedianFilterSize As Integer
        Get
            Return Me._MedianFilterSize
        End Get
        Set(ByVal value As Integer)
            If Not value.Equals(Me.MedianFilterSize) Then
                Me._MedianFilterSize = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    Private _PostTransientDelay As Double
    ''' <summary> Gets or sets the delay time in seconds between the end of the thermal transient and
    ''' the start of the final cold resistance measurement. </summary>
    ''' <value> The post transient delay. </value>
    Public Property PostTransientDelay() As Double
        Get
            Return Me._PostTransientDelay
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.PostTransientDelay) Then
                Me._PostTransientDelay = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    Private _SamplingInterval As Double
    ''' <summary> Gets or sets the sampling interval. </summary>
    ''' <value> The sampling interval. </value>
    Public Property SamplingInterval() As Double
        Get
            Return Me._SamplingInterval
        End Get
        Set(ByVal value As Double)
            If Not value.Equals(Me.SamplingInterval) Then
                Me._SamplingInterval = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    Private _TracePoints As Integer
    ''' <summary> Gets or sets the number of trace points to measure. </summary>
    ''' <value> The trace points. </value>
    Public Property TracePoints() As Integer
        Get
            Return Me._TracePoints
        End Get
        Set(ByVal value As Integer)
            If Not value.Equals(Me.TracePoints) Then
                Me._TracePoints = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

#End Region

End Class
