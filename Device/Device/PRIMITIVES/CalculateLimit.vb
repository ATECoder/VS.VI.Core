Imports isr.Core.Pith
''' <summary> Defines a limit for the SCPI Calculate SCPI subsystem. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/5/2013" by="David" revision=""> Created based on SCPI 5.1 library. </history>
Public Class CalculateLimit
    Inherits PropertyPublisherBase
    Implements IPresettablePropertyPublisher

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me.ComplianceFailureBits = 15
        Me.IncomplianceCondition = True
        Me.FailureBits = 15
        Me.LowerLimit = New Double?
        Me.LowerLimitFailureBits = 15
        Me.PassBits = 15
        Me.Enabled = False
        Me.UpperLimit = New Double?
        Me.UpperLimitFailureBits = 15
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets values to their known execution clear state. </summary>
    Public Sub ClearExecutionState() Implements IPresettable.ClearExecutionState
    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Sub InitKnownState() Implements IPresettable.InitKnownState
    End Sub

    ''' <summary> Sets values to their known execution preset state. </summary>
    Public Sub PresetKnownState() Implements IPresettable.PresetKnownState
    End Sub

    ''' <summary> Sets values to their known execution reset state. </summary>
    Public Sub ResetKnownState() Implements IPresettable.ResetKnownState
        Me.Enabled = False
        Me.IncomplianceCondition = True
        Me.LowerLimit = New Double?
        Me.Enabled = False
        Me.UpperLimit = New Double?
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.AsyncNotifyPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMPLIANCE BITS "

    ''' <summary> The Compliance Failure Bit patterns. </summary>
    Private _ComplianceFailureBits As Integer?

    ''' <summary>Gets or sets the fail bit pattern for compliance (15). </summary>
    ''' <value> The Compliance Failure Bit pattern or none if not set or unknown. </value>
    Public Overloads Property ComplianceFailureBits As Integer?
        Get
            Return Me._ComplianceFailureBits
        End Get
        Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.ComplianceFailureBits, value) Then
                Me._ComplianceFailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ComplianceFailureBits))
            End If
        End Set
    End Property

#End Region

#Region " IN COMPLIANCE CONDITION "

    ''' <summary> The In Compliance Condition. </summary>
    Private _IncomplianceCondition As Boolean?

    ''' <summary> Gets or sets the cached the Compliance condition (In/Out) (in) state. </summary>
    ''' <value> <c>True</c> if the In Compliance Condition is In; <c>False</c> if out, or none if
    ''' unknown or not set. </value>
    Public Overloads Property IncomplianceCondition As Boolean?
        Get
            Return Me._IncomplianceCondition
        End Get
        Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.IncomplianceCondition, value) Then
                Me._IncomplianceCondition = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IncomplianceCondition))
            End If
        End Set
    End Property

#End Region

#Region " ENABLED "

    ''' <summary> The on/off state. </summary>
    Private _Enabled As Boolean?

    ''' <summary> Gets or sets the cached on/off (off = False) state. </summary>
    ''' <value> <c>True</c> if the Enabled; <c>False</c> if not, or none if
    ''' unknown or not set. </value>
    Public Overloads Property Enabled As Boolean?
        Get
            Return Me._Enabled
        End Get
        Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Enabled, value) Then
                Me._Enabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Enabled))
            End If
        End Set
    End Property

#End Region

#Region " FAILURE BITS "

    ''' <summary> The Failure Bits. </summary>
    Private _FailureBits As Integer?

    ''' <summary>Gets or sets the Output fail bit pattern (15). </summary>
    ''' <value> The Failure Bits or none if not set or unknown. </value>
    Public Overloads Property FailureBits As Integer?
        Get
            Return Me._FailureBits
        End Get
        Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.FailureBits, value) Then
                Me._FailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.FailureBits))
            End If
        End Set
    End Property

#End Region

#Region " LIMIT FAILED "

    ''' <summary> The Limit Failed. </summary>
    Private _LimitFailed As Boolean?

    ''' <summary> Gets or sets the cached the fail condition (False) state. </summary>
    ''' <value> <c>True</c> if the Limit Failed; <c>False</c> if not, or none if
    ''' unknown or not set. </value>
    Public Overloads Property LimitFailed As Boolean?
        Get
            Return Me._LimitFailed
        End Get
        Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.LimitFailed, value) Then
                Me._LimitFailed = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LimitFailed))
            End If
        End Set
    End Property

#End Region

#Region " LOWER LIMIT "

    ''' <summary> The Lower Limit. </summary>
    Private _LowerLimit As Double?

    ''' <summary>Gets or sets the lower limit. </summary>
    ''' <value> The Lower Limit or none if not set or unknown. </value>
    Public Overloads Property LowerLimit As Double?
        Get
            Return Me._LowerLimit
        End Get
        Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.LowerLimit, value) Then
                Me._LowerLimit = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LowerLimit))
            End If
        End Set
    End Property

#End Region

#Region " LOWER LIMIT FAILURE BITS "

    ''' <summary> The Lower Limit Failure Bits. </summary>
    Private _LowerLimitFailureBits As Integer?

    ''' <summary>Gets or sets the lower limit failure bit pattern (15). </summary>
    ''' <value> The Lower Limit FailureBits or none if not set or unknown. </value>
    Public Overloads Property LowerLimitFailureBits As Integer?
        Get
            Return Me._LowerLimitFailureBits
        End Get
        Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.LowerLimitFailureBits, value) Then
                Me._LowerLimitFailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LowerLimitFailureBits))
            End If
        End Set
    End Property

#End Region

#Region " PASS BITS "

    ''' <summary> The Pass Bits. </summary>
    Private _PassBits As Integer?

    ''' <summary>Gets or sets the Output pass bit pattern (15). </summary>
    ''' <value> The Pass Bits or none if not set or unknown. </value>
    Public Overloads Property PassBits As Integer?
        Get
            Return Me._PassBits
        End Get
        Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.PassBits, value) Then
                Me._PassBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.PassBits))
            End If
        End Set
    End Property

#End Region

#Region " UPPER LIMIT "

    ''' <summary> The Upper Limit. </summary>
    Private _UpperLimit As Double?

    ''' <summary>Gets or sets the Upper limit. </summary>
    ''' <value> The Upper Limit or none if not set or unknown. </value>
    Public Overloads Property UpperLimit As Double?
        Get
            Return Me._UpperLimit
        End Get
        Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.UpperLimit, value) Then
                Me._UpperLimit = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.UpperLimit))
            End If
        End Set
    End Property

#End Region

#Region " UPPER LIMIT FAILURE BITS "

    ''' <summary> The Upper Limit Failure Bits. </summary>
    Private _UpperLimitFailureBits As Integer?

    ''' <summary>Gets or sets the Upper limit failure bit pattern (15). </summary>
    ''' <value> The Upper Limit FailureBits or none if not set or unknown. </value>
    Public Overloads Property UpperLimitFailureBits As Integer?
        Get
            Return Me._UpperLimitFailureBits
        End Get
        Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.UpperLimitFailureBits, value) Then
                Me._UpperLimitFailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.UpperLimitFailureBits))
            End If
        End Set
    End Property

#End Region

End Class
