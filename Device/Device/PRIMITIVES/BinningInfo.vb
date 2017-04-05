Imports isr.Core.Pith
''' <summary> Defines the binning information . </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/5/2013" by="David" revision=""> Created based on SCPI 5.1 library. </history>
Public Class BinningInfo
    Inherits PropertyPublisherBase
    Implements IPresettablePropertyPublisher

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
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
    Private Sub _ResetKnownState()
        Me._LimitFailed = New Boolean?
        Me._Enabled = False
        Me._UpperLimit = 1
        Me._UpperLimitFailureBits = 15
        Me._LowerLimit = -1
        Me._LowerLimitFailureBits = 15
        Me._PassBits = 15
        Me._StrobePulseWidth = TimeSpan.FromTicks(CLng(0.01 * TimeSpan.TicksPerMillisecond))
        Me._InputLineNumber = 1
        Me._OutputLineNumber = 2
        Me._ArmCount = 0
        Me._ArmDirection = VI.Direction.Acceptor
        Me._ArmSource = VI.ArmSources.Immediate
        Me._TriggerDirection = VI.Direction.Acceptor
        Me._TriggerSource = VI.TriggerSources.Immediate
    End Sub

    ''' <summary> Sets values to their known execution reset state. </summary>
    Public Sub ResetKnownState() Implements IPresettable.ResetKnownState
        Me._ResetKnownState()
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafeSendPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " ARM SOURCE "

    ''' <summary> The arm source mode. </summary>
    Private _ArmSource As ArmSources

    ''' <summary> Gets or sets the arm Source. </summary>
    ''' <value> The <see cref="ArmSource">source Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Property ArmSource As ArmSources
        Get
            Return Me._ArmSource
        End Get
        Set(ByVal value As ArmSources)
            If Not Me.ArmSource.Equals(value) Then
                Me._ArmSource = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " COUNT "

    Private _ArmCount As Integer
    ''' <summary> Gets or sets the arm Count. </summary>
    ''' <value> The source Count or none if not set or unknown. </value>
    Public Property ArmCount As Integer
        Get
            Return Me._ArmCount
        End Get
        Set(ByVal value As Integer)
            If Not Nullable.Equals(Me.ArmCount, value) Then
                Me._ArmCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " ARM DIRECTION "

    ''' <summary> The ARM Direction. </summary>
    Private _ArmDirection As Direction

    ''' <summary> Gets or sets the arm Direction. </summary>
    ''' <value> The <see cref="ArmDirection">ARM Direction</see> or none if not set or
    ''' unknown. </value>
    Public Property ArmDirection As Direction
        Get
            Return Me._ArmDirection
        End Get
        Set(ByVal value As Direction)
            If Not Me.ArmDirection.Equals(value) Then
                Me._ArmDirection = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " ENABLED "

    ''' <summary> The on/off state. </summary>
    Private _Enabled As Boolean

    ''' <summary> Gets or sets the enabled state. </summary>
    ''' <value> <c>True</c> if the Enabled; <c>False</c> if not, or none if
    ''' unknown or not set. </value>
    Public Overloads Property Enabled As Boolean
        Get
            Return Me._Enabled
        End Get
        Set(ByVal value As Boolean)
            If Me.Enabled <> value Then
                Me._Enabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " FAILURE BITS "

    ''' <summary> The Failure Bits. </summary>
    Private _FailureBits As Integer

    ''' <summary>Gets or sets the Output fail bit pattern (15). </summary>
    ''' <value> The Failure Bits or none if not set or unknown. </value>
    Public Overloads Property FailureBits As Integer
        Get
            Return Me._FailureBits
        End Get
        Set(ByVal value As Integer)
            If Me.FailureBits <> value Then
                Me._FailureBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " INPUT LINE NUMBER "

    ''' <summary> The Input Line Number. </summary>
    Private _InputLineNumber As Integer

    ''' <summary> Gets or sets the arm Input Line Number. </summary>
    ''' <value> The source Input Line Number or none if not set or unknown. </value>
    Public Property InputLineNumber As Integer
        Get
            Return Me._InputLineNumber
        End Get
        Set(ByVal value As Integer)
            If Not Nullable.Equals(Me.InputLineNumber, value) Then
                Me._InputLineNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " LIMIT FAILED "

    ''' <summary> The Limit Failed. </summary>
    Private _LimitFailed As Boolean?

    ''' <summary> Gets or sets the fail condition (False) state. </summary>
    ''' <value> <c>True</c> if the Limit Failed; <c>False</c> if not, or none if
    ''' unknown or not set. </value>
    Public Overloads Property LimitFailed As Boolean?
        Get
            Return Me._LimitFailed
        End Get
        Set(ByVal value As Boolean?)
            If Not Boolean.Equals(Me.LimitFailed, value) Then
                Me._LimitFailed = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " LOWER LIMIT "

    ''' <summary> The Lower Limit. </summary>
    Private _LowerLimit As Double

    ''' <summary>Gets or sets the lower limit. </summary>
    ''' <value> The Lower Limit or none if not set or unknown. </value>
    Public Overloads Property LowerLimit As Double
        Get
            Return Me._LowerLimit
        End Get
        Set(ByVal value As Double)
            If Me.LowerLimit <> value Then
                Me._LowerLimit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " LOWER LIMIT FAILURE BITS "

    ''' <summary> The Lower Limit Failure Bits. </summary>
    Private _LowerLimitFailureBits As Integer

    ''' <summary>Gets or sets the lower limit failure bit pattern (15). </summary>
    ''' <value> The Lower Limit FailureBits or none if not set or unknown. </value>
    Public Overloads Property LowerLimitFailureBits As Integer
        Get
            Return Me._LowerLimitFailureBits
        End Get
        Set(ByVal value As Integer)
            If Me.LowerLimitFailureBits <> value Then
                Me._LowerLimitFailureBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " OUTPUT LINE NUMBER "

    ''' <summary> The Output Line Number. </summary>
    Private _OutputLineNumber As Integer

    ''' <summary> Gets or sets the Output Line Number. </summary>
    ''' <value> The source Output Line Number or none if not set or unknown. </value>
    Public Property OutputLineNumber As Integer
        Get
            Return Me._OutputLineNumber
        End Get
        Set(ByVal value As Integer)
            If Not Nullable.Equals(Me.OutputLineNumber, value) Then
                Me._OutputLineNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " PASS BITS "

    ''' <summary> The Pass Bits. </summary>
    Private _PassBits As Integer

    ''' <summary>Gets or sets the Output pass bit pattern (15). </summary>
    ''' <value> The Pass Bits or none if not set or unknown. </value>
    Public Overloads Property PassBits As Integer
        Get
            Return Me._PassBits
        End Get
        Set(ByVal value As Integer)
            If Me.PassBits <> value Then
                Me._PassBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " STROBE PULSE WIDTH "

    ''' <summary> The Strobe Pulse Width. </summary>
    Private _StrobePulseWidth As TimeSpan

    ''' <summary>Gets or sets the end of test strobe pulse width. </summary>
    ''' <value> The Strobe Pulse Width or none if not set or unknown. </value>
    Public Overloads Property StrobePulseWidth As TimeSpan
        Get
            Return Me._StrobePulseWidth
        End Get
        Set(ByVal value As TimeSpan)
            If Me.StrobePulseWidth <> value Then
                Me._StrobePulseWidth = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " TRIGGER SOURCE "

    ''' <summary> The Trigger source mode. </summary>
    Private _TriggerSource As TriggerSources

    ''' <summary> Gets or sets the Trigger Source. </summary>
    ''' <value> The <see cref="TriggerSource">source Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Property TriggerSource As TriggerSources
        Get
            Return Me._TriggerSource
        End Get
        Set(ByVal value As TriggerSources)
            If Not Me.TriggerSource.Equals(value) Then
                Me._TriggerSource = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " TRIGGER DIRECTION "

    ''' <summary> The Trigger Direction. </summary>
    Private _TriggerDirection As Direction

    ''' <summary> Gets or sets the trigger Direction. </summary>
    ''' <value> The <see cref="TriggerDirection">Trigger Direction</see> or none if not set or
    ''' unknown. </value>
    Public Property TriggerDirection As Direction
        Get
            Return Me._TriggerDirection
        End Get
        Set(ByVal value As Direction)
            If Not Me.TriggerDirection.Equals(value) Then
                Me._TriggerDirection = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " UPPER LIMIT "

    ''' <summary> The Upper Limit. </summary>
    Private _UpperLimit As Double

    ''' <summary>Gets or sets the Upper limit. </summary>
    ''' <value> The Upper Limit or none if not set or unknown. </value>
    Public Overloads Property UpperLimit As Double
        Get
            Return Me._UpperLimit
        End Get
        Set(ByVal value As Double)
            If Me.UpperLimit <> value Then
                Me._UpperLimit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " UPPER LIMIT FAILURE BITS "

    ''' <summary> The Upper Limit Failure Bits. </summary>
    Private _UpperLimitFailureBits As Integer

    ''' <summary>Gets or sets the Upper limit failure bit pattern (15). </summary>
    ''' <value> The Upper Limit FailureBits or none if not set or unknown. </value>
    Public Overloads Property UpperLimitFailureBits As Integer
        Get
            Return Me._UpperLimitFailureBits
        End Get
        Set(ByVal value As Integer)
            If Me.UpperLimitFailureBits <> value Then
                Me._UpperLimitFailureBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

End Class
