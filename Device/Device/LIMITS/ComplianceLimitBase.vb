Imports isr.Core.Pith
''' <summary> Defines the SCPI Compliance Limit subsystem. </summary>
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
Public MustInherit Class ComplianceLimitBase
    Inherits NumericLimitBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="ComplianceLimitBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(1, statusSubsystem)
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <param name="limitNumber">     The limit number. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal limitNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(limitNumber, statusSubsystem)
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
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Enabled = False
        Me.IncomplianceCondition = True
        Me.FailureBits = 15
    End Sub

#End Region

#Region " FAILURE BITS "

    Private _FailureBits As Integer?
    ''' <summary> Gets or sets the cached Failure Bits. </summary>
    ''' <value> The Failure Bits or none if not set or unknown. </value>
    Public Overloads Property FailureBits As Integer?
        Get
            Return Me._FailureBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.FailureBits, value) Then
                Me._FailureBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Failure Bits. </summary>
    ''' <param name="value"> The current Failure Bits. </param>
    ''' <returns> The Failure Bits or none if unknown. </returns>
    Public Function ApplyFailureBits(ByVal value As Integer) As Integer?
        Me.WriteFailureBits(value)
        Return Me.QueryFailureBits()
    End Function

    ''' <summary> Gets Failure Bits query command. </summary>
    ''' <value> The Failure Bits query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM:COMP:SOUR2?" </remarks>
    Protected Overridable ReadOnly Property FailureBitsQueryCommand As String

    ''' <summary> Queries the current Failure Bits. </summary>
    ''' <returns> The Failure Bits or none if unknown. </returns>
    Public Function QueryFailureBits() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.FailureBitsQueryCommand) Then
            Me.FailureBits = Me.Session.Query(0I, Me.BuildCommand(Me.FailureBitsQueryCommand))
        End If
        Return Me.FailureBits
    End Function

    ''' <summary> Gets Failure Bits command format. </summary>
    ''' <value> The Failure Bits command format. </value>
    ''' <remarks> SCPI: ":CALC2:LIM:COMP:SOUR2 {0}" </remarks>
    Protected Overridable ReadOnly Property FailureBitsCommandFormat As String

    ''' <summary> Write the Failure Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Failure Bits. </param>
    ''' <returns> The Failure Bits or none if unknown. </returns>
    Public Function WriteFailureBits(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.FailureBitsCommandFormat) Then
            Me.Session.WriteLine(Me.BuildCommand(Me.FailureBitsCommandFormat), value)
        End If
        Me.FailureBits = value
        Return Me.FailureBits
    End Function

#End Region

#Region " IN COMPLIANCE FAILURE CONDITION "

    Private _IncomplianceCondition As Boolean?
    ''' <summary> Gets or sets the cached In Compliance Condition sentinel. </summary>
    ''' <value> <c>null</c> if In Compliance Condition is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property IncomplianceCondition As Boolean?
        Get
            Return Me._IncomplianceCondition
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.IncomplianceCondition, value) Then
                Me._IncomplianceCondition = value
                Me.SafePostPropertyChanged()
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
    ''' <remarks> SCPI: ":CALC2:LIM:COMP:FAIL?" </remarks>
    Protected Overridable ReadOnly Property IncomplianceConditionQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="InComplianceCondition">Condition</see> sentinel. </summary>
    ''' <returns> <c>True</c> if in compliance; otherwise <c>False</c>. </returns>
    Public Function QueryIncomplianceCondition() As Boolean?
        Me.IncomplianceCondition = Me.Query(Me.IncomplianceCondition, Me.BuildCommand(Me.IncomplianceConditionQueryCommand))
        Return Me.IncomplianceCondition
    End Function

    ''' <summary> Gets the In-compliance Condition command Format.
    ''' <see cref="InComplianceCondition">Condition</see> sentinel. </summary>
    ''' <remarks> SCPI: ":CALC2:LIM:COMP:FAIL {0:'IN';'IN';'OUT'}" </remarks>
    Protected Overridable ReadOnly Property IncomplianceConditionCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if in compliance; otherwise <c>False</c>. </returns>
    Public Function WriteIncomplianceCondition(ByVal value As Boolean) As Boolean?
        Me.IncomplianceCondition = Me.Write(value, Me.BuildCommand(Me.IncomplianceConditionCommandFormat))
        Return Me.IncomplianceCondition
    End Function

#End Region

End Class
