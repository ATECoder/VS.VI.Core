Imports isr.Core.Pith
''' <summary> Defines the SCPI Composite Limit subsystem. </summary>
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
Public MustInherit Class CompositeLimitBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
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
        Me.LimitMode = VI.LimitMode.Grading
        Me.BinningControl = VI.BinningControl.Immediate
        Me.FailureBits = 15
        Me.PassBits = 15
        Me.AutoClearEnabled = True
    End Sub

#End Region

#Region " COMMANDS "

    ''' <summary> Gets or sets the composite limits clear command. </summary>
    ''' <value> The composite limits clear command. </value>
    ''' <remarks> SCPI: ":CLAC2:CLIM:CLE" </remarks>
    Protected Overridable ReadOnly Property ClearCommand As String

    ''' <summary>
    ''' Clears composite limits.
    ''' Returns the instrument output to the TTL settings per SOURC2:TTL
    ''' </summary>
    Public Sub ClearLimits()
        Me.Session.Execute(Me.ClearCommand)
    End Sub

#End Region

#Region " AUTO CLEAR ENABLED "

    Private _AutoClearEnabled As Boolean?
    ''' <summary> Gets or sets the cached Composite Limits Auto Clear enabled sentinel. </summary>
    ''' <value> <c>null</c> if Composite Limits Auto Clear enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoClearEnabled As Boolean?
        Get
            Return Me._AutoClearEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoClearEnabled, value) Then
                Me._AutoClearEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Composite Limits Auto Clear enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if Enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoClearEnabled(value)
        Return Me.QueryAutoClearEnabled()
    End Function

    ''' <summary> Gets the Composite Limits Auto Clear enabled query command. </summary>
    ''' <value> The Composite Limits Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:CLE:AUTO?" </remarks>
    Protected Overridable ReadOnly Property AutoClearEnabledQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="AutoClearEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoClearEnabled() As Boolean?
        Me.AutoClearEnabled = Me.Query(Me.AutoClearEnabled, Me.AutoClearEnabledQueryCommand)
        Return Me.AutoClearEnabled
    End Function

    ''' <summary> Gets the Composite Limits Auto Clear enabled command Format. </summary>
    ''' <value> The Composite Limits Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:CLE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoClearEnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoClearEnabled = Me.Write(value, Me.AutoClearEnabledCommandFormat)
        Return Me.AutoClearEnabled
    End Function

#End Region

#Region " FAILURE BITS "

    Private _FailureBits As Integer?
    ''' <summary> Gets or sets the cached Composite Limits Failure Bits. </summary>
    ''' <value> The Composite Limits Failure Bits or none if not set or unknown. </value>
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

    ''' <summary> Writes and reads back the Composite Limits Failure Bits. </summary>
    ''' <param name="value"> The current Composite Limits Failure Bits. </param>
    ''' <returns> The Composite Limits Failure Bits or none if unknown. </returns>
    Public Function ApplyFailureBits(ByVal value As Integer) As Integer?
        Me.WriteFailureBits(value)
        Return Me.QueryFailureBits()
    End Function

    ''' <summary> Gets the Lower Limit failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:FAIL:SOUR2?" </remarks>
    Protected Overridable ReadOnly Property FailureBitsQueryCommand As String

    ''' <summary> Queries the current Lower Limit Failure Bits. </summary>
    ''' <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    Public Function QueryFailureBits() As Integer?
        Me.FailureBits = Me.Query(Me.FailureBits, Me.FailureBitsQueryCommand)
        Return Me.FailureBits
    End Function

    ''' <summary> Gets the Lower Limit Failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: "::CALC2:CLIM:FAIL:SOUR2 {0}" </remarks>
    Protected Overridable ReadOnly Property FailureBitsCommandFormat As String

    ''' <summary> Sets back the Lower Limit Failure Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Lower Limit Failure Bits. </param>
    ''' <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    Public Function WriteFailureBits(ByVal value As Integer) As Integer?
        Me.FailureBits = Me.Write(value, Me.FailureBitsCommandFormat)
        Return Me.FailureBits
    End Function

#End Region

#Region " LIMITS PASS BITS "

    Private _PassBits As Integer?
    ''' <summary> Gets or sets the cached Composite Limits Pass Bits. </summary>
    ''' <value> The Composite Limits Pass Bits or none if not set or unknown. </value>
    Public Overloads Property PassBits As Integer?
        Get
            Return Me._PassBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.PassBits, value) Then
                Me._PassBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Composite Limits Pass Bits. </summary>
    ''' <param name="value"> The current Composite Limits Pass Bits. </param>
    ''' <returns> The Composite Limits Pass Bits or none if unknown. </returns>
    Public Function ApplyPassBits(ByVal value As Integer) As Integer?
        Me.WritePassBits(value)
        Return Me.QueryPassBits()
    End Function

    ''' <summary> Gets the Lower Limit Pass Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:PASS:SOUR2?" </remarks>
    Protected Overridable ReadOnly Property PassBitsQueryCommand As String

    ''' <summary> Queries the current Lower Limit Pass Bits. </summary>
    ''' <returns> The Lower Limit Pass Bits or none if unknown. </returns>
    Public Function QueryPassBits() As Integer?
        Me.PassBits = Me.Query(Me.PassBits, Me.PassBitsQueryCommand)
        Return Me.PassBits
    End Function

    ''' <summary> Gets the Lower Limit Pass Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: "::CALC2:CLIM:PASS:SOUR2 {0}" </remarks>
    Protected Overridable ReadOnly Property PassBitsCommandFormat As String

    ''' <summary> Sets back the Lower Limit Pass Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Lower Limit Pass Bits. </param>
    ''' <returns> The Lower Limit Pass Bits or none if unknown. </returns>
    Public Function WritePassBits(ByVal value As Integer) As Integer?
        Me.PassBits = Me.Write(value, Me.PassBitsCommandFormat)
        Return Me.PassBits
    End Function

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
                Me.SafePostPropertyChanged()
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
                Me.SafePostPropertyChanged()
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
