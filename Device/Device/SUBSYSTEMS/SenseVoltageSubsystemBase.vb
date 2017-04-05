''' <summary> Defines the contract that must be implemented by a Sense Voltage Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific ReSenses, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SenseVoltageSubsystemBase
    Inherits SenseFunctionSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseVoltageSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.PowerLineCycles = 1
        Me.ProtectionLevel = 21
        Me.ProtectionEnabled = True
        Me.Range = 21
        Me.AutoRangeEnabled = True
    End Sub

#End Region

#Region " PROTECTION LEVEL "

    ''' <summary> The Voltage Limit. </summary>
    Private _ProtectionLevel As Double?

    ''' <summary> Gets or sets the cached source Voltage Limit for a current source. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property ProtectionLevel As Double?
        Get
            Return Me._ProtectionLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.ProtectionLevel, value) Then
                Me._ProtectionLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the protection level. </summary>
    ''' <param name="value"> the protection level. </param>
    ''' <returns> the protection level. </returns>
    Public Function ApplyProtectionLevel(ByVal value As Double) As Double?
        Me.WriteProtectionLevel(value)
        Return Me.QueryProtectionLevel
    End Function

    ''' <summary> Gets the protection level query command. </summary>
    ''' <value> the protection level query command. </value>
    Protected Overridable ReadOnly Property ProtectionLevelQueryCommand As String

    ''' <summary> Queries the protection level. </summary>
    ''' <returns> the protection level or none if unknown. </returns>
    Public Function QueryProtectionLevel() As Double?
        Me.ProtectionLevel = Me.Query(Me.ProtectionLevel, Me.ProtectionLevelQueryCommand)
        Return Me.ProtectionLevel
    End Function

    ''' <summary> Gets the protection level command format. </summary>
    ''' <value> the protection level command format. </value>
    Protected Overridable ReadOnly Property ProtectionLevelCommandFormat As String

    ''' <summary> Writes the protection level without reading back the value from the device. </summary>
    ''' <remarks> This command sets the protection level. </remarks>
    ''' <param name="value"> the protection level. </param>
    ''' <returns> the protection level. </returns>
    Public Function WriteProtectionLevel(ByVal value As Double) As Double?
        Me.ProtectionLevel = Me.Write(value, Me.ProtectionLevelCommandFormat)
        Return Me.ProtectionLevel
    End Function

#End Region

#Region " PROTECTION ENABLED "

    ''' <summary> Protection enabled. </summary>
    Private _ProtectionEnabled As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether Sense Voltage protection is enabled. </summary>
    ''' <remarks> :SENSE:VOLT:PROT:STAT The setter enables or disables the over-Voltage protection (OCP)
    ''' function. The enabled state is On (1); the disabled state is Off (0). If the over-Voltage
    ''' protection function is enabled and the output goes into constant Voltage operation, the
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

    ''' <summary> Gets the Protection enabled query command. </summary>
    ''' <value> The Protection enabled query command. </value>
    ''' <remarks> SCPI: "system:RANG:AUTO?" </remarks>
    Protected Overridable ReadOnly Property ProtectionEnabledQueryCommand As String

    ''' <summary> Queries the Protection Enabled sentinel. Also sets the
    ''' <see cref="ProtectionEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryProtectionEnabled() As Boolean?
        Me.ProtectionEnabled = Me.Query(Me.ProtectionEnabled, Me.ProtectionEnabledQueryCommand)
        Return Me.ProtectionEnabled
    End Function

    ''' <summary> Gets the Protection enabled command Format. </summary>
    ''' <value> The Protection enabled query command. </value>
    ''' <remarks> SCPI: "system:RANGE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property ProtectionEnabledCommandFormat As String

    ''' <summary> Writes the Protection Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteProtectionEnabled(ByVal value As Boolean) As Boolean?
        Me.ProtectionEnabled = Me.Write(value, Me.ProtectionEnabledCommandFormat)
        Return Me.ProtectionEnabled
    End Function

#End Region

End Class

