Imports isr.Core.Pith
''' <summary> Defines the CALC2 SCPI subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/05/2013" by="David" revision="">            Created based on SCPI 5.1 library. </history>
''' <history date="03/25/2008" by="David" revision="5.0.3004.x">  Port to new SCPI library. </history>
Public MustInherit Class Calculate2SubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._activeLimit = New CalculateLimit()
        ' create a new instance of the arm layers collection
        Me._limits = New PresettablePropertyPublisherCollection
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me._ActiveLimit IsNot Nothing Then Me._ActiveLimit.Dispose() : Me._ActiveLimit = Nothing
                Me._limits?.Clear() : Me._limits = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.Limits.ClearExecutionState()
    End Sub

    ''' <summary> Sets the subsystem to its preset state. </summary>
    Public Overrides Sub PresetKnownState()
        MyBase.PresetKnownState()
        Me.Limits.PresetKnownState()
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Limits.ResetKnownState()
    End Sub

#End Region

#Region " LIMITS "

    ''' <summary> Gets or sets reference to the
    ''' <see cref="CalculateLimit">limit</see>. </summary>
    ''' <value> The active limit. </value>
    Public Property ActiveLimit() As CalculateLimit

    ''' <summary> Adds a <see cref="CalculateLimit">limit</see> to the collection of limits. Makes the
    ''' limits the <see cref="ActiveLimit">active limit.</see> </summary>
    Public Sub AddLimit()

        Me._ActiveLimit = New CalculateLimit()
        Me._limits.Add(Me._ActiveLimit)

    End Sub

    Private _limits As PresettablePropertyPublisherCollection
    ''' <summary> Gets reference to the collection of calculation limits </summary>
    Public ReadOnly Property Limits() As PresettablePropertyPublisherCollection
        Get
            Return Me._limits
        End Get
    End Property

#End Region

#Region " ACTIVE LIMIT "

#Region " COMPLIANCE FAILURE BITS "

    ''' <summary> Gets or sets the cached Compliance Failure Bits. </summary>
    ''' <value> The Compliance Failure Bits or none if not set or unknown. </value>
    Public Overloads Property ComplianceFailureBits As Integer?
        Get
            Return Me.ActiveLimit.ComplianceFailureBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.ComplianceFailureBits, value) Then
                Me.ActiveLimit.ComplianceFailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ComplianceFailureBits))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the ComplianceFailureBits. </summary>
    ''' <param name="value"> The current ComplianceFailureBits. </param>
    ''' <returns> The Compliance Failure Bits or none if unknown. </returns>
    Public Function ApplyComplianceFailureBits(ByVal value As Integer) As Integer?
        Me.WriteComplianceFailureBits(value)
        Return Me.QueryComplianceFailureBits()
    End Function

    ''' <summary> Queries the current ComplianceFailureBits. </summary>
    ''' <returns> The Compliance Failure Bits or none if unknown. </returns>
    Public MustOverride Function QueryComplianceFailureBits() As Integer?

    ''' <summary> Sets back the Compliance Failure Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current ComplianceFailureBits. </param>
    ''' <returns> The Compliance Failure Bits or none if unknown. </returns>
    Public MustOverride Function WriteComplianceFailureBits(ByVal value As Integer) As Integer?

#End Region

#Region " IN COMPLIANCE FAILURE CONDITION "

    ''' <summary> Gets or sets the cached In Compliance Condition sentinel. </summary>
    ''' <value> <c>null</c> if In Compliance Condition is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property IncomplianceCondition As Boolean?
        Get
            Return Me.ActiveLimit.IncomplianceCondition
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.IncomplianceCondition, value) Then
                Me.ActiveLimit.IncomplianceCondition = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IncomplianceCondition))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the In Compliance Condition sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if Condition; otherwise <c>False</c>. </returns>
    Public Function ApplyIncomplianceCondition(ByVal value As Boolean) As Boolean?
        Me.WriteIncomplianceCondition(value)
        Return Me.QueryIncomplianceCondition()
    End Function

    ''' <summary> Gets the In compliance Condition query command. </summary>
    ''' <value> The In-compliance Condition query command. </value>
    ''' <remarks> SCPI: "???" </remarks>
    Protected Overridable ReadOnly Property IncomplianceConditionQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="InComplianceCondition">Condition</see> sentinel. </summary>
    ''' <returns> <c>True</c> if in compliance; otherwise <c>False</c>. </returns>
    Public Function QueryIncomplianceCondition() As Boolean?
        Me.IncomplianceCondition = Me.Query(Me.IncomplianceCondition, Me.IncomplianceConditionQueryCommand)
        Return Me.IncomplianceCondition
    End Function

    ''' <summary> Gets the In-compliance Condition command Format.
    ''' <see cref="InComplianceCondition">Condition</see> sentinel. </summary>
    ''' <remarks> SCPI: ":??? {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property IncomplianceConditionCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if in compliance; otherwise <c>False</c>. </returns>
    Public Function WriteIncomplianceCondition(ByVal value As Boolean) As Boolean?
        Me.IncomplianceCondition = Me.Write(value, Me.IncomplianceConditionCommandFormat)
        Return Me.IncomplianceCondition
    End Function

#End Region

#Region " LIMIT FAILED "

    ''' <summary> Gets or sets the cached In Limit Failed Condition sentinel. </summary>
    ''' <value> <c>null</c> if In Limit Failed Condition is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property LimitFailed As Boolean?
        Get
            Return Me.ActiveLimit.LimitFailed
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.LimitFailed, value) Then
                Me.ActiveLimit.LimitFailed = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LimitFailed))
            End If
        End Set
    End Property

    ''' <summary> Gets the Limit Failed query command. </summary>
    ''' <value> The Limit Failed query command. </value>
    ''' <remarks> SCPI: ":???" </remarks>
    Protected Overridable ReadOnly Property LimitFailedQueryCommand As String

    ''' <summary> Queries the Auto Delay Failed sentinel. Also sets the
    ''' <see cref="LimitFailed">Failed</see> sentinel. </summary>
    ''' <returns> <c>True</c> if Failed; otherwise <c>False</c>. </returns>
    Public Function QueryLimitFailed() As Boolean?
        Me.LimitFailed = Me.Query(Me.LimitFailed, Me.LimitFailedQueryCommand)
        Return Me.LimitFailed
    End Function

#End Region

#Region " PASS BITS "

    ''' <summary> Gets the cached Pass Bits. </summary>
    ''' <value> The Pass Bits or none if not set or unknown. </value>
    Public Overloads Property PassBits As Integer?
        Get
            Return Me.ActiveLimit.PassBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.PassBits, value) Then
                Me.ActiveLimit.PassBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.PassBits))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Pass Bits. </summary>
    ''' <param name="value"> The current Pass Bits. </param>
    ''' <returns> The Pass Bits or none if unknown. </returns>
    Public Function ApplyPassBits(ByVal value As Integer) As Integer?
        Me.WritePassBits(value)
        Return Me.QueryPassBits()
    End Function

    ''' <summary> Queries the current Pass Bits. </summary>
    ''' <returns> The Pass Bits or none if unknown. </returns>
    Public MustOverride Function QueryPassBits() As Integer?

    ''' <summary> Sets back the Pass Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Pass Bits. </param>
    ''' <returns> The Pass Bits or none if unknown. </returns>
    Public MustOverride Function WritePassBits(ByVal value As Integer) As Integer?

#Region " PASS BITS: Finish coding "
#If TO_DO Then
    ''' <summary> Queries the current Pass Bits. </summary>
    ''' <returns> The Pass Bits or none if unknown. </returns>
    Public Overrides Function QueryPassBits() As Integer?
        If Me.IsSessionOpen Then
            Me.PassBits = Me.Session.QueryInteger(":CALC2:PASS:SOUR2?")
        End If
        Return Me.PassBits
    End Function

    ''' <summary> Write the Pass Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current PassBits. </param>
    ''' <returns> The Pass Bits or none if unknown. </returns>
    Public Overrides Function WritePassBits(ByVal value As Integer) As Integer?
        If Me.IsSessionOpen Then
            Me.Session.WriteLine(":CALC2:PASS:SOUR2 {0}", value)
        End If
        If Me.LastStatus < SessionStatusCode.Success Then
            Me.PassBits = New Integer?
        Else
            Me.PassBits = value
        End If
        Return Me.PassBits
    End Function
#End If
#End Region

#End Region

#Region " LIMIT ENABLED "

    ''' <summary> Gets the cached Limit Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Limit Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property LimitEnabled As Boolean?
        Get
            Return Me.ActiveLimit.Enabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.LimitEnabled, value) Then
                Me.ActiveLimit.Enabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LimitEnabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if Enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyLimitEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteLimitEnabled(value)
        Return Me.QueryLimitEnabled()
    End Function

    ''' <summary> Gets the Limit enabled query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: "CALC2:STAT?" </remarks>
    Protected Overridable ReadOnly Property LimitEnabledQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="LimitEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryLimitEnabled() As Boolean?
        Me.LimitEnabled = Me.Query(Me.LimitEnabled, Me.LimitEnabledQueryCommand)
        Return Me.LimitEnabled
    End Function

    ''' <summary> Gets the Limit enabled command Format. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: "CALC2:STAT {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property LimitEnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteLimitEnabled(ByVal value As Boolean) As Boolean?
        Me.LimitEnabled = Me.Write(value, Me.LimitEnabledCommandFormat)
        Return Me.LimitEnabled
    End Function

#End Region

#Region " LOWER LIMIT "

    ''' <summary> Gets or sets the cached Lower Limit. </summary>
    ''' <value> The Lower Limit or none if not set or unknown. </value>
    Public Overloads Property LowerLimit As Double?
        Get
            Return Me.ActiveLimit.LowerLimit
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.LowerLimit, value) Then
                Me.ActiveLimit.LowerLimit = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LowerLimit))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Lower Limit. </summary>
    ''' <param name="value"> The current Lower Limit. </param>
    ''' <returns> The Lower Limit or none if unknown. </returns>
    Public Function ApplyLowerLimit(ByVal value As Double) As Double?
        Me.WriteLowerLimit(value)
        Return Me.QueryLowerLimit()
    End Function

    ''' <summary> Gets the Lower Limit query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LOW?" </remarks>
    Protected Overridable ReadOnly Property LowerLimitQueryCommand As String

    ''' <summary> Queries the current Lower Limit. </summary>
    ''' <returns> The Lower Limit or none if unknown. </returns>
    Public Function QueryLowerLimit() As Double?
        Me.LowerLimit = Me.Query(Me.LowerLimit, Me.LowerLimitQueryCommand)
        Return Me.LowerLimit
    End Function

    ''' <summary> Gets the Lower Limit query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LOW {0}" </remarks>
    Protected Overridable ReadOnly Property LowerLimitCommandFormat As String

    ''' <summary> Sets the Lower Limit without reading back the value from the device. </summary>
    ''' <param name="value"> The current Lower Limit. </param>
    ''' <returns> The Lower Limit or none if unknown. </returns>
    Public Function WriteLowerLimit(ByVal value As Double) As Double?
        Me.LowerLimit = Me.Write(value, Me.LowerLimitCommandFormat)
        Return Me.LowerLimit
    End Function

#End Region

#Region " LOWER LIMIT FAILURE BITS "

    ''' <summary> Gets or sets the cached Lower Limit Failure Bits. </summary>
    ''' <value> The Lower Limit Failure Bits or none if not set or unknown. </value>
    Public Overloads Property LowerLimitFailureBits As Integer?
        Get
            Return Me.ActiveLimit.LowerLimitFailureBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.LowerLimitFailureBits, value) Then
                Me.ActiveLimit.LowerLimitFailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LowerLimitFailureBits))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Lower Limit Failure Bits. </summary>
    ''' <param name="value"> The current Lower Limit Failure Bits. </param>
    ''' <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    Public Function ApplyLowerLimitFailureBits(ByVal value As Integer) As Integer?
        Me.WriteLowerLimitFailureBits(value)
        Return Me.QueryLowerLimitFailureBits()
    End Function

    ''' <summary> Gets the Lower Limit failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LOW:SOUR2?" </remarks>
    Protected Overridable ReadOnly Property LowerLimitFailureBitsQueryCommand As String

    ''' <summary> Queries the current Lower Limit Failure Bits. </summary>
    ''' <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    Public Function QueryLowerLimitFailureBits() As Integer?
        Me.LowerLimitFailureBits = Me.Query(Me.LowerLimitFailureBits, Me.LowerLimitfailurebitsQueryCommand)
        Return Me.LowerLimitFailureBits
    End Function

    ''' <summary> Gets the Lower Limit Failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LOW:SOUR2 {0}" </remarks>
    Protected Overridable ReadOnly Property LowerLimitFailureBitsCommandFormat As String

    ''' <summary> Sets back the Lower Limit Failure Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Lower Limit Failure Bits. </param>
    ''' <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    Public Function WriteLowerLimitFailureBits(ByVal value As Integer) As Integer?
        Me.LowerLimitFailureBits = Me.Write(value, Me.LowerLimitFailureBitsCommandFormat)
        Return Me.LowerLimitFailureBits
    End Function

#End Region

#Region " UPPER LIMIT "

    ''' <summary> Gets or sets the cached Upper Limit. </summary>
    ''' <value> The Upper Limit or none if not set or unknown. </value>
    Public Overloads Property UpperLimit As Double?
        Get
            Return Me.ActiveLimit.UpperLimit
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.UpperLimit, value) Then
                Me.ActiveLimit.UpperLimit = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.UpperLimit))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Upper Limit. </summary>
    ''' <param name="value"> The current Upper Limit. </param>
    ''' <returns> The Upper Limit or none if unknown. </returns>
    Public Function ApplyUpperLimit(ByVal value As Double) As Double?
        Me.WriteUpperLimit(value)
        Return Me.QueryUpperLimit()
    End Function

    ''' <summary> Gets the Upper Limit query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:UPP?" </remarks>
    Protected Overridable ReadOnly Property UpperLimitQueryCommand As String

    ''' <summary> Queries the current Upper Limit. </summary>
    ''' <returns> The Upper Limit or none if unknown. </returns>
    Public Function QueryUpperLimit() As Double?
        Me.UpperLimit = Me.Query(Me.UpperLimit, Me.UpperLimitQueryCommand)
        Return Me.UpperLimit
    End Function

    ''' <summary> Gets the Upper Limit query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:UPP {0}" </remarks>
    Protected Overridable ReadOnly Property UpperLimitCommandFormat As String

    ''' <summary> Sets the Upper Limit without reading back the value from the device. </summary>
    ''' <param name="value"> The current Upper Limit. </param>
    ''' <returns> The Upper Limit or none if unknown. </returns>
    Public Function WriteUpperLimit(ByVal value As Double) As Double?
        Me.UpperLimit = Me.Write(value, Me.UpperLimitCommandFormat)
        Return Me.UpperLimit
    End Function

#End Region

#Region " UPPER LIMIT FAILURE BITS "

    ''' <summary> Gets or sets the cached Upper Limit Failure Bits. </summary>
    ''' <value> The Upper Limit Failure Bits or none if not set or unknown. </value>
    Public Overloads Property UpperLimitFailureBits As Integer?
        Get
            Return Me.ActiveLimit.UpperLimitFailureBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.UpperLimitFailureBits, value) Then
                Me.ActiveLimit.UpperLimitFailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.UpperLimitFailureBits))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Upper Limit Failure Bits. </summary>
    ''' <param name="value"> The current Upper Limit Failure Bits. </param>
    ''' <returns> The Upper Limit Failure Bits or none if unknown. </returns>
    Public Function ApplyUpperLimitFailureBits(ByVal value As Integer) As Integer?
        Me.WriteUpperLimitFailureBits(value)
        Return Me.QueryUpperLimitFailureBits()
    End Function

    ''' <summary> Gets the Upper Limit failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:UPP:SOUR2?" </remarks>
    Protected Overridable ReadOnly Property UpperLimitFailureBitsQueryCommand As String

    ''' <summary> Queries the current Upper Limit Failure Bits. </summary>
    ''' <returns> The Upper Limit Failure Bits or none if unknown. </returns>
    Public Function QueryUpperLimitFailureBits() As Integer?
        Me.UpperLimitFailureBits = Me.Query(Me.UpperLimitFailureBits, Me.UpperLimitFailureBitsQueryCommand)
        Return Me.UpperLimitFailureBits
    End Function

    ''' <summary> Gets the Upper Limit Failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:UPP:SOUR2 {0}" </remarks>
    Protected Overridable ReadOnly Property UpperLimitFailureBitsCommandFormat As String

    ''' <summary> Sets back the Upper Limit Failure Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Upper Limit Failure Bits. </param>
    ''' <returns> The Upper Limit Failure Bits or none if unknown. </returns>
    Public Function WriteUpperLimitFailureBits(ByVal value As Integer) As Integer?
        Me.UpperLimitFailureBits = Me.Write(value, Me.UpperLimitFailureBitsCommandFormat)
        Return Me.UpperLimitFailureBits
    End Function

#End Region

#End Region

#Region " CALC 2 "

    ''' <summary>
    ''' Return the average of the buffer contents.
    ''' </summary>
    Public Function CalculateBufferAverage() As Double

        ' select average
        Me.Session.WriteLine("CALC2:FORM:MEAN")

        ' turn status on.
        Me.Session.WriteLine("CALC2:STAT:ON")

        ' do the calculation.
        Me.Session.WriteLine("CALC2:IMM")

        ' get the result
        Return Me.Session.Query(0.0F, ":CALC2:DATA")

    End Function

#End Region

#Region " COMPOSITE LIMITS "

#Region " COMPOSITE LIMITS AUTO CLEAR ENABLED "

    Private _CompositeLimitsAutoClearEnabled As Boolean?
    ''' <summary> Gets or sets the cached Composite Limits Auto Clear enabled sentinel. </summary>
    ''' <value> <c>null</c> if Composite Limits Auto Clear enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property CompositeLimitsAutoClearEnabled As Boolean?
        Get
            Return Me._CompositeLimitsAutoClearEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.CompositeLimitsAutoClearEnabled, value) Then
                Me._CompositeLimitsAutoClearEnabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.CompositeLimitsAutoClearEnabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Composite Limits Auto Clear enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if Enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyCompositeLimitsAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteCompositeLimitsAutoClearEnabled(value)
        Return Me.QueryCompositeLimitsAutoClearEnabled()
    End Function

    ''' <summary> Gets the Composite Limits Auto Clear enabled query command. </summary>
    ''' <value> The Composite Limits Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:CLE:AUTO?" </remarks>
    Protected Overridable ReadOnly Property CompositeLimitsAutoClearEnabledQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="CompositeLimitsAutoClearEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryCompositeLimitsAutoClearEnabled() As Boolean?
        Me.CompositeLimitsAutoClearEnabled = Me.Query(Me.CompositeLimitsAutoClearEnabled, Me.CompositeLimitsAutoClearEnabledQueryCommand)
        Return Me.CompositeLimitsAutoClearEnabled
    End Function

    ''' <summary> Gets the Composite Limits Auto Clear enabled command Format. </summary>
    ''' <value> The Composite Limits Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:CLE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property CompositeLimitsAutoClearEnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteCompositeLimitsAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.CompositeLimitsAutoClearEnabled = Me.Write(value, Me.CompositeLimitsAutoClearEnabledCommandFormat)
        Return Me.CompositeLimitsAutoClearEnabled
    End Function

#End Region

#Region " COMPOSITE LIMITS FAILURE BITS "

    Private _compositeLimitsFailureBits As Integer?
    ''' <summary> Gets or sets the cached Composite Limits Failure Bits. </summary>
    ''' <value> The Composite Limits Failure Bits or none if not set or unknown. </value>
    Public Overloads Property CompositeLimitsFailureBits As Integer?
        Get
            Return Me._compositeLimitsFailureBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.CompositeLimitsFailureBits, value) Then
                Me._compositeLimitsFailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.CompositeLimitsFailureBits))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Composite Limits Failure Bits. </summary>
    ''' <param name="value"> The current Composite Limits Failure Bits. </param>
    ''' <returns> The Composite Limits Failure Bits or none if unknown. </returns>
    Public Function ApplyCompositeLimitsFailureBits(ByVal value As Integer) As Integer?
        Me.WriteCompositeLimitsFailureBits(value)
        Return Me.QueryCompositeLimitsFailureBits()
    End Function

    ''' <summary> Queries the current Composite Limits Failure Bits. </summary>
    ''' <returns> The Composite Limits Failure Bits or none if unknown. </returns>
    Public MustOverride Function QueryCompositeLimitsFailureBits() As Integer?

    ''' <summary> Sets back the Composite Limits Failure Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Composite Limits Failure Bits. </param>
    ''' <returns> The Composite Limits Failure Bits or none if unknown. </returns>
    Public MustOverride Function WriteCompositeLimitsFailureBits(ByVal value As Integer) As Integer?

#Region " COMPOSITE LIMITS FAILURE BITS "
#If TO_DO Then
    ''' <summary> Queries the current Composite Limits Failure Bits. </summary>
    ''' <returns> The Composite Limits Failure Bits or none if unknown. </returns>
    Public Overrides Function QueryCompositeLimitsFailureBits() As Integer?
        If Me.IsSessionOpen Then
            Me.CompositeLimitsFailureBits = Me.Session.QueryInteger(":CALC2:CLIM:FAIL:SOUR2?")
        End If
        Return Me.CompositeLimitsFailureBits
    End Function

    ''' <summary> Write the Composite Limits Failure Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Composite Limits Failure Bits. </param>
    ''' <returns> The Composite Limits Failure Bits or none if unknown. </returns>
    Public Overrides Function WriteCompositeLimitsFailureBits(ByVal value As Integer) As Integer?
        If Me.IsSessionOpen Then
            Me.Session.WriteLine(":CALC2:CLIM:FAIL:SOUR2 {0}", value)
        End If
        If Me.LastStatus < SessionStatusCode.Success Then
            Me.CompositeLimitsFailureBits = New Integer?
        Else
            Me.CompositeLimitsFailureBits = value
        End If
        Return Me.CompositeLimitsFailureBits
    End Function
#End If
#End Region
#End Region

#Region " BINNING CONTROL "

    ''' <summary> The Binning Control. </summary>
    Private _BinningControl As BinningControl?

    ''' <summary> Gets or sets the cached Binning Control. </summary>
    ''' <value> The <see cref="BinningControl">Binning Control</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property BinningControl As BinningControl?
        Get
            Return Me._BinningControl
        End Get
        Protected Set(ByVal value As BinningControl?)
            If Not Me.BinningControl.Equals(value) Then
                Me._BinningControl = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.BinningControl))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Binning Control. </summary>
    ''' <param name="value"> The Binning Control. </param>
    ''' <returns> The <see cref="BinningControl">source  Binning Control</see> or none if unknown. </returns>
    Public Function ApplyBinningControl(ByVal value As BinningControl) As BinningControl?
        Me.WriteBinningControl(value)
        Return Me.QueryBinningControl()
    End Function

    ''' <summary> Gets the Binning Control query command. </summary>
    ''' <value> The Binning Control query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:BCON" </remarks>
    Protected Overridable ReadOnly Property BinningControlQueryCommand As String

    ''' <summary> Queries the Binning Control. </summary>
    ''' <returns> The <see cref="BinningControl"> Binning Control</see> or none if unknown. </returns>
    Public Function QueryBinningControl() As BinningControl?
        Me.BinningControl = Me.Query(Of BinningControl)(Me.BinningControlQueryCommand, Me.BinningControl)
        Return Me.BinningControl
    End Function

    ''' <summary> Gets the Binning Control command format. </summary>
    ''' <value> The Binning Control query command format. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:BCON {0}" </remarks>
    Protected Overridable ReadOnly Property BinningControlCommandFormat As String

    ''' <summary> Writes the Binning Control without reading back the value from the device. </summary>
    ''' <param name="value"> The Binning Control. </param>
    ''' <returns> The <see cref="BinningControl"> Binning Control</see> or none if unknown. </returns>
    Public Function WriteBinningControl(ByVal value As BinningControl) As BinningControl?
        Me.BinningControl = Me.Write(Of BinningControl)(Me.BinningControlCommandFormat, value)
        Return Me.BinningControl
    End Function

#End Region

#Region " LIMIT MODE "

    ''' <summary> The Limit Mode. </summary>
    Private _LimitMode As LimitMode?

    ''' <summary> Gets or sets the cached Limit Mode. </summary>
    ''' <value> The <see cref="LimitMode">Limit Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property LimitMode As LimitMode?
        Get
            Return Me._LimitMode
        End Get
        Protected Set(ByVal value As LimitMode?)
            If Not Me.LimitMode.Equals(value) Then
                Me._LimitMode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LimitMode))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit Mode. </summary>
    ''' <param name="value"> The Limit Mode. </param>
    ''' <returns> The <see cref="LimitMode">source  Limit Mode</see> or none if unknown. </returns>
    Public Function ApplyLimitMode(ByVal value As LimitMode) As LimitMode?
        Me.WriteLimitMode(value)
        Return Me.QueryLimitMode()
    End Function

    ''' <summary> Gets the Limit Mode query command. </summary>
    ''' <value> The Limit Mode query command. </value>
    ''' <remarks> SCPI: "CALC2:CLIM:MODE" </remarks>
    Protected Overridable ReadOnly Property LimitModeQueryCommand As String

    ''' <summary> Queries the Limit Mode. </summary>
    ''' <returns> The <see cref="LimitMode"> Limit Mode</see> or none if unknown. </returns>
    Public Function QueryLimitMode() As LimitMode?
        Me.LimitMode = Me.Query(Of LimitMode)(Me.LimitModeQueryCommand, Me.LimitMode)
        Return Me.LimitMode
    End Function

    ''' <summary> Gets the Limit Mode command format. </summary>
    ''' <value> The Limit Mode command format. </value>
    ''' <remarks> SCPI: "CALC2:CLIM:MODE" </remarks>
    Protected Overridable ReadOnly Property LimitModeCommandFormat As String

    ''' <summary> Writes the Limit Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Limit Mode. </param>
    ''' <returns> The <see cref="LimitMode"> Limit Mode</see> or none if unknown. </returns>
    Public Function WriteLimitMode(ByVal value As LimitMode) As LimitMode?
        Me.LimitMode = Me.Write(Of LimitMode)(Me.LimitModeCommandFormat, value)
        Return Me.LimitMode
    End Function

#End Region

    ''' <summary>
    ''' Clears composite limits.
    ''' Returns the instrument output to the TTL settings per SOURC2:TTL
    ''' </summary>
    Public Sub ClearCompositeLimits()
        Me.Session.WriteLine(":CLAC2:CLIM:CLE")
    End Sub

#End Region

End Class

''' <summary> Enumerates the binning control mode. </summary>
Public Enum BinningControl
    <ComponentModel.Description("Not Defined ()")> None
    <ComponentModel.Description("Immediate (IMM)")> Immediate
    <ComponentModel.Description("End (END)")> [End]
End Enum

''' <summary> Enumerates the grading control mode. </summary>
Public Enum LimitMode
    <ComponentModel.Description("Not Defined ()")> None
    <ComponentModel.Description("Grading (GRAD)")> Grading
    <ComponentModel.Description("End (END)")> [End]
End Enum
