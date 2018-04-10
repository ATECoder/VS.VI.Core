''' <summary> Defines the contract that must be implemented by a Sense Resistance Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific ReSenses, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SenseResistanceSubsystemBase
    Inherits SenseFunctionSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseResistanceSubsystemBase" /> class. </summary>
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
        Me.Range = 210000
        Me.AutoRangeEnabled = True
        Me.ConfigurationMode = VI.ConfigurationMode.Auto
    End Sub

#End Region

#Region " CONFIGURATION MODE "

    ''' <summary> The Configuration Mode. </summary>
    Private _ConfigurationMode As ConfigurationMode?

    ''' <summary> Gets or sets the cached source ConfigurationMode. </summary>
    ''' <value> The <see cref="ConfigurationMode">source Configuration Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property ConfigurationMode As ConfigurationMode?
        Get
            Return Me._ConfigurationMode
        End Get
        Protected Set(ByVal value As ConfigurationMode?)
            If Not Me.ConfigurationMode.Equals(value) Then
                Me._ConfigurationMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Configuration Mode. </summary>
    ''' <param name="value"> The  Source Configuration Mode. </param>
    ''' <returns> The <see cref="ConfigurationMode">source Configuration Mode</see> or none if unknown. </returns>
    Public Function ApplyConfigurationMode(ByVal value As ConfigurationMode) As ConfigurationMode?
        Me.WriteConfigurationMode(value)
        Return Me.QueryConfigurationMode()
    End Function

    ''' <summary> Gets the Configuration Mode query command. </summary>
    ''' <value> The Configuration Mode query command. </value>
    ''' <remarks> SCPI: ":TRIG:DIR" </remarks>
    Protected Overridable ReadOnly Property ConfigurationModeQueryCommand As String

    ''' <summary> Queries the Configuration Mode. </summary>
    ''' <returns> The <see cref="ConfigurationMode">Configuration Mode</see> or none if unknown. </returns>
    Public Function QueryConfigurationMode() As ConfigurationMode?
        Me.ConfigurationMode = Me.Query(Of ConfigurationMode)(Me.ConfigurationModeQueryCommand, Me.ConfigurationMode)
        Return Me.ConfigurationMode
    End Function

    ''' <summary> Gets the Configuration Mode command format. </summary>
    ''' <value> The Configuration Mode command format. </value>
    ''' <remarks> SCPI: ":TRIG:DIR {0}" </remarks>
    Protected Overridable ReadOnly Property ConfigurationModeCommandFormat As String

    ''' <summary> Writes the Configuration Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Configuration Mode. </param>
    ''' <returns> The <see cref="ConfigurationMode">Configuration Mode</see> or none if unknown. </returns>
    Public Function WriteConfigurationMode(ByVal value As ConfigurationMode) As ConfigurationMode?
        Me.ConfigurationMode = Me.Write(Of ConfigurationMode)(Me.ConfigurationModeCommandFormat, value)
        Return Me.ConfigurationMode
    End Function

#End Region

End Class

''' <summary> Enumerates the configuration mode. </summary>
Public Enum ConfigurationMode
    <ComponentModel.Description("Not Defined ()")> None
    <ComponentModel.Description("Auto (AUTO)")> [Auto]
    <ComponentModel.Description("Manual (MAN)")> [Manual]
End Enum
