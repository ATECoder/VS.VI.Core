''' <summary>  Defines the contract that must be implemented by a SCPI Source Current Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SourceCurrentSubsystemBase
    Inherits VI.Scpi.SourceCurrentSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceCurrentSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        Me.ProtectionEnabled = True
        MyBase.ResetKnownState()
    End Sub

#End Region

#Region " PROTECTION ENABLED "

    ''' <summary> The protection enabled. </summary>
    Private _ProtectionEnabled As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether source current protection is enabled. </summary>
    ''' <remarks> :SOURCE:CURR:PROT:STAT The setter enables or disables the over-current protection
    ''' (OCP)
    ''' function. The enabled state is On (1); the disabled state is Off (0). If the over-current
    ''' protection function is enabled and the output goes into constant current operation, the
    ''' output is disabled and OCP is set in the Questionable Condition status register. The *RST
    ''' value = Off. </remarks>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property ProtectionEnabled As Boolean?
        Get
            Return Me._ProtectionEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.ProtectionEnabled, value) Then
                Me._ProtectionEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Protection Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyProtectionEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteProtectionEnabled(value)
        Return Me.QueryProtectionEnabled()
    End Function

    ''' <summary> Gets or sets the Protection enabled query command. </summary>
    ''' <value> The Protection enabled query command. </value>
    ''' <remarks> SCPI: "system:RANG:AUTO?" </remarks>
    Protected MustOverride ReadOnly Property ProtectionEnabledQueryCommand As String

    ''' <summary> Queries the Protection Enabled sentinel. Also sets the
    ''' <see cref="ProtectionEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryProtectionEnabled() As Boolean?
        Me.ProtectionEnabled = Me.Query(Me.ProtectionEnabled, Me.ProtectionEnabledQueryCommand)
        Return Me.ProtectionEnabled
    End Function

    ''' <summary> Gets or sets the Protection enabled command Format. </summary>
    ''' <value> The Protection enabled query command. </value>
    ''' <remarks> SCPI: "system:RANGE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected MustOverride ReadOnly Property ProtectionEnabledCommandFormat As String

    ''' <summary> Writes the Protection Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteProtectionEnabled(ByVal value As Boolean) As Boolean?
        Me.ProtectionEnabled = Me.Write(value, Me.ProtectionEnabledCommandFormat)
        Return Me.ProtectionEnabled
    End Function

#End Region

End Class
