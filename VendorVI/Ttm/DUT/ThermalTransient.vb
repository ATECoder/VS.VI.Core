Imports System.ComponentModel
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.NumericExtensions
''' <summary> Thermal transient. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="12/23/2013" by="David" revision=""> Created. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance")>
Public Class ThermalTransient
    Inherits ThermalTransientBase

    Implements System.ICloneable

#Region " CONSTRUCTORS  AND  CLONES "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Clones an existing measurement. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub New(ByVal value As ThermalTransient)
        MyBase.New(value)
        If value IsNot Nothing Then
        End If
    End Sub

    ''' <summary> Creates a new object that is a copy of the current instance. </summary>
    ''' <returns> A new object that is a copy of this instance. </returns>
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return New ThermalTransient(Me)
    End Function

#End Region

#Region " PRESET "

    ''' <summary> Clears execution state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.Asymptote = New Double?
        Me.TimeConstant = New Double?
        Me.EstimatedVoltage = New Double?
        Me.CorrelationCoefficient = New Double?
        Me.StandardError = New Double?
        Me.Iterations = New Integer?
        Me.OptimizationOutcome = New OptimizationOutcome?
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " MODEL VALUES "

    Private _timeConstant As Double?

    ''' <summary> Gets or sets the time constant. </summary>
    ''' <value> The time constant. </value>
    Public Property TimeConstant As Double?
        Get
            Return Me._timeConstant
        End Get
        Set(value As Double?)
            If Me.TimeConstant.Differs(value, 0.000000001) Then
                Me._timeConstant = value
                Me.SafePostPropertyChanged(NameOf(Me.TimeConstant))
            End If
        End Set
    End Property

    ''' <summary> Gets the time constant caption. </summary>
    ''' <value> The time constant caption. </value>
    Public ReadOnly Property TimeConstantCaption As String
        Get
            If Me.TimeConstant.HasValue Then
                Return (1000 * Me.TimeConstant.Value).ToString("G4")
            Else
                Return ""
            End If
        End Get
    End Property

    Private _Asymptote As Double?

    ''' <summary> Gets or sets the asymptote. </summary>
    ''' <value> The asymptote. </value>
    Public Property Asymptote As Double?
        Get
            Return Me._Asymptote
        End Get
        Set(value As Double?)
            If Me.Asymptote.Differs(value, 0.000000001) Then
                Me._Asymptote = value
                Me.SafePostPropertyChanged(NameOf(Me.Asymptote))
            End If
        End Set
    End Property

    ''' <summary> Gets the asymptote caption. </summary>
    ''' <value> The asymptote caption. </value>
    Public ReadOnly Property AsymptoteCaption As String
        Get
            If Me.Asymptote.HasValue Then
                Return (1000 * Me.Asymptote.Value).ToString("G4")
            Else
                Return ""
            End If
        End Get
    End Property

    Private _EstimatedVoltage As Double?

    ''' <summary> Gets or sets the estimated voltage. </summary>
    ''' <value> The estimated voltage. </value>
    Public Property EstimatedVoltage As Double?
        Get
            Return Me._EstimatedVoltage
        End Get
        Set(value As Double?)
            If Me.EstimatedVoltage.Differs(value, 0.000000001) Then
                Me._EstimatedVoltage = value
                Me.SafePostPropertyChanged(NameOf(Me.EstimatedVoltage))
            End If
        End Set
    End Property

    ''' <summary> Gets the estimated voltage caption. </summary>
    ''' <value> The estimated voltage caption. </value>
    Public ReadOnly Property EstimatedVoltageCaption As String
        Get
            If Me.EstimatedVoltage.HasValue Then
                Return (1000 * Me.EstimatedVoltage.Value).ToString("G4")
            Else
                Return ""
            End If
        End Get
    End Property

    Private _CorrelationCoefficient As Double?

    ''' <summary> Gets or sets the correlation coefficient. </summary>
    ''' <value> The correlation coefficient. </value>
    Public Property CorrelationCoefficient As Double?
        Get
            Return Me._CorrelationCoefficient
        End Get
        Set(value As Double?)
            If Me.CorrelationCoefficient.Differs(value, 0.000001) Then
                Me._CorrelationCoefficient = value
                Me.SafePostPropertyChanged(NameOf(Me.CorrelationCoefficient))
            End If
        End Set
    End Property

    ''' <summary> Gets the correlation coefficient caption. </summary>
    ''' <value> The correlation coefficient caption. </value>
    Public ReadOnly Property CorrelationCoefficientCaption As String
        Get
            If Me.CorrelationCoefficient.HasValue Then
                Return Me.CorrelationCoefficient.Value.ToString("G4")
            Else
                Return ""
            End If
        End Get
    End Property

    Private _StandardError As Double?

    ''' <summary> Gets or sets the standard error. </summary>
    ''' <value> The standard error. </value>
    Public Property StandardError As Double?
        Get
            Return Me._StandardError
        End Get
        Set(value As Double?)
            If Me.StandardError.Differs(value, 0.000001) Then
                Me._StandardError = value
                Me.SafePostPropertyChanged(NameOf(Me.StandardError))
            End If
        End Set
    End Property

    ''' <summary> Gets the standard error caption. </summary>
    ''' <value> The standard error caption. </value>
    Public ReadOnly Property StandardErrorCaption As String
        Get
            If Me.StandardError.HasValue Then
                Return (1000 * Me.StandardError.Value).ToString("G4")
            Else
                Return ""
            End If
        End Get
    End Property

    Private _Iterations As Integer?

    ''' <summary> Gets or sets the iterations. </summary>
    ''' <value> The iterations. </value>
    Public Property Iterations As Integer?
        Get
            Return Me._Iterations
        End Get
        Set(value As Integer?)
            If Not Nullable.Equals(value, Me.Iterations) Then
                Me._Iterations = value
                Me.SafePostPropertyChanged(NameOf(Me.Iterations))
            End If
        End Set
    End Property

    ''' <summary> Gets the iterations caption. </summary>
    ''' <value> The iterations caption. </value>
    Public ReadOnly Property IterationsCaption As String
        Get
            If Me.Iterations.HasValue Then
                Return Me.Iterations.Value.ToString()
            Else
                Return ""
            End If
        End Get
    End Property

    Private _OptimizationOutcome As OptimizationOutcome?

    ''' <summary> Gets or sets the optimization outcome. </summary>
    ''' <value> The optimization outcome. </value>
    Public Property OptimizationOutcome As OptimizationOutcome?
        Get
            Return Me._OptimizationOutcome
        End Get
        Set(value As OptimizationOutcome?)
            If Not Nullable.Equals(value, Me.OptimizationOutcome) Then
                Me._OptimizationOutcome = value
                Me.SafePostPropertyChanged(NameOf(Me.OptimizationOutcome))
            End If
        End Set
    End Property

    ''' <summary> Gets the optimization outcome caption. </summary>
    ''' <value> The optimization outcome caption. </value>
    Public ReadOnly Property OptimizationOutcomeCaption As String
        Get
            If Me.OptimizationOutcome.HasValue Then
                Return Me.OptimizationOutcome.Value.ToString
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property OptimizationOutcomeDescription As String
        Get
            If Me.OptimizationOutcome.HasValue Then
                Return Me.OptimizationOutcome.Value.Description
            Else
                Return ""
            End If
        End Get
    End Property

#End Region

End Class

''' <summary> Values that represent Optimization Outcome. </summary>
Public Enum OptimizationOutcome
    <Description("Not Specified")> None
    <Description("Count Out--reached maximum number of iterations")> Exhausted
    <Description("Optimized--within the objective function precision range")> Optimized
    <Description("Converged--within the argument values convergence range")> Converged
End Enum