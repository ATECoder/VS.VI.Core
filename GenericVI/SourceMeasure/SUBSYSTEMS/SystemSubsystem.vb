''' <summary>
''' Defines a SCPI System Subsystem for a generic Source Measure instrument such as the Keithley 2400.
''' </summary>
''' <license>
''' (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
Public Class SystemSubsystem
    Inherits SystemSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
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

#Region " COMMAND SYNTAX "

#Region " CONTACT CHECK RESISTANCE "

    ''' <summary> Gets or sets the ContactCheck Resistance command format. </summary>
    ''' <value> the ContactCheck Resistance command format. </value>
    Protected Overrides ReadOnly Property ContactCheckResistanceCommandFormat As String = "SYST:CCH:RES {0}"

    ''' <summary> Gets or sets the ContactCheck Resistance query command. </summary>
    ''' <value> the ContactCheck Resistance query command. </value>
    Protected Overrides ReadOnly Property ContactCheckResistanceQueryCommand As String = "SYST:CCH:RES?"

#End Region

#End Region

#Region " AUTO ZERO ENABLED "

    ''' <summary> Queries the auto zero enabled state. </summary>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Overrides Function QueryAutoZeroEnabled() As Boolean?
        Me.AutoZeroEnabled = Me.Session.Query(Me.AutoZeroEnabled.GetValueOrDefault(True), ":SYST:AZER?")
        Return Me.AutoZeroEnabled
    End Function

    ''' <summary> Writes the auto zero enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Overrides Function WriteAutoZeroEnabled(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine(":SYST:AZER {0:'ON';'ON';'OFF'}", CType(value, Integer))
        Me.AutoZeroEnabled = value
        Return Me.AutoZeroEnabled
    End Function

#End Region

#Region " CONTACT CHECK ENABLED "

    ''' <summary> Queries the Contact Check enabled state. </summary>
    ''' <returns> <c>True</c> if Contact Check is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Overrides Function QueryContactCheckEnabled() As Boolean?
        Me.ContactCheckEnabled = Me.Session.Query(Me.ContactCheckEnabled.GetValueOrDefault(True), ":SYST:CCH?")
        Return Me.ContactCheckEnabled
    End Function

    ''' <summary> Writes the Contact Check enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if Contact Check is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Overrides Function WriteContactCheckEnabled(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine(":SYST:CCH {0:'ON';'ON';'OFF'}", CType(value, Integer))
        Me.ContactCheckEnabled = value
        Return Me.ContactCheckEnabled
    End Function

#End Region

#Region " FOUR WIRE SENSE ENABLED "

    ''' <summary> Queries the Four Wire Sense enabled state. </summary>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Overrides Function QueryWireSenseEnabled() As Boolean?
        Me.FourWireSenseEnabled = Me.Session.Query(Me.FourWireSenseEnabled.GetValueOrDefault(True), ":SYST:RSEN?")
        Return Me.FourWireSenseEnabled
    End Function

    ''' <summary> Writes the Four Wire Sense enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Overrides Function WriteFourWireSenseEnabled(ByVal value As Boolean) As Boolean?
        Me.FourWireSenseEnabled = New Boolean?
        Me.Session.WriteLine(":SYST:RSEN {0:'ON';'ON';'OFF'}", CType(value, Integer))
        Me.FourWireSenseEnabled = value
        Return Me.FourWireSenseEnabled
    End Function

#End Region

#Region " FRONT SWITCHED "

    Private _FrontSwitched As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether Front is Switched. </summary>
    ''' <value> <c>True</c> if Front is Switched; <c>False</c> if not or none if not set or
    ''' unknown. </value>
    Public Property FrontSwitched() As Boolean?
        Get
            Return Me._FrontSwitched
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.FrontSwitched, value) Then
                Me._FrontSwitched = value
                Me.SafePostPropertyChanged(NameOf(Me.FrontSwitched))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the front switched query command. </summary>
    ''' <value> The front switched query command. </value>
    ''' <remarks> SCPI: :SYST:FRSW? </remarks>
    Public Overridable Property FrontSwitchedQueryCommand As String

    ''' <summary> Queries the Front Switched state. </summary>
    ''' <returns> <c>True</c> if Front is Switched; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function QueryFrontSwitched() As Boolean?
        Me.FrontSwitched = Me.Session.Query(Me.FrontSwitched.GetValueOrDefault(True), Me.FrontSwitchedQueryCommand)
        Return Me.FrontSwitched
    End Function

#End Region

#Region " SUPPORTS CONTACT CHECK "

    ''' <summary> The contact check option. </summary>
    Private Const ContactCheckOption As String = "CONTACT-CHECK"

    ''' <summary> Queries support for contact check. </summary>
    ''' <remarks> David, 3/5/2016. </remarks>
    ''' <returns> <c>True</c> if supports contact check; otherwise <c>False</c>. </returns>
    Public Overrides Function QuerySupportsContactCheck() As Boolean?
        If String.IsNullOrWhiteSpace(Me.Options) Then Me.QueryOptions()
        If String.IsNullOrWhiteSpace(Me.Options) Then
            Me.SupportsContactCheck = New Boolean?
        Else
            Me.SupportsContactCheck = Me.Options.Contains(SystemSubsystem.ContactCheckOption)
        End If
        Return Me.SupportsContactCheck
    End Function

#End Region

#Region " OPTIONS "

    ''' <summary> Gets or sets the option query command. </summary>
    ''' <value> The option query command. </value>
    Protected Overrides ReadOnly Property OptionQueryCommand As String = "*OPT?"

#End Region

End Class
