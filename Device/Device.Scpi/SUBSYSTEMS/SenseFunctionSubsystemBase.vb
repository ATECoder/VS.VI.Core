
''' <summary> Defines the contract that must be implemented by a SCPI Sense Resistance Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific ReSenses, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SenseFunctionSubsystemBase
    Inherits VI.SenseFunctionSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseResistanceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FunctionModeRanges.Clear()
        For Each fmode As SenseFunctionModes In [Enum].GetValues(GetType(SenseFunctionModes))
            Me.FunctionModeRanges.Add(fmode, Core.Pith.RangeR.Full)
        Next
        Me.FunctionModeDecimalPlaces.Clear()
        For Each fmode As SenseFunctionModes In [Enum].GetValues(GetType(SenseFunctionModes))
            Me.FunctionModeDecimalPlaces.Add(fmode, Me.DefaultFunctionModeDecimalPlaces)
        Next
        Me.FunctionModeUnits.Clear()
        For Each fmode As SenseFunctionModes In [Enum].GetValues(GetType(SenseFunctionModes))
            Me.FunctionModeUnits.Add(fmode, Arebis.StandardUnits.UnitlessUnits.Ratio)
        Next
        Me.FunctionModeUnits(SenseFunctionModes.CurrentAC) = Arebis.StandardUnits.ElectricUnits.Ampere
        Me.FunctionModeUnits(SenseFunctionModes.CurrentDC) = Arebis.StandardUnits.ElectricUnits.Ampere
        Me.FunctionModeUnits(SenseFunctionModes.Resistance) = Arebis.StandardUnits.ElectricUnits.Ohm
        Me.FunctionModeUnits(SenseFunctionModes.Frequency) = Arebis.StandardUnits.FrequencyUnits.Hertz
        Me.FunctionModeUnits(SenseFunctionModes.Temperature) = Arebis.StandardUnits.TemperatureUnits.Kelvin
        Me.FunctionModeUnits(SenseFunctionModes.VoltageAC) = Arebis.StandardUnits.ElectricUnits.Volt
        Me.FunctionModeUnits(SenseFunctionModes.VoltageDC) = Arebis.StandardUnits.ElectricUnits.Volt
    End Sub

#End Region

#Region " AVERAGE FILTER TYPE "

    Private _AverageFilterType As AverageFilterType?
    ''' <summary> Gets or sets the cached Average Filter Type. </summary>
    ''' <value> The <see cref="AverageFilterType">Average Filter Type</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property AverageFilterType As AverageFilterType?
        Get
            Return Me._AverageFilterType
        End Get
        Protected Set(ByVal value As AverageFilterType?)
            If Not Me.AverageFilterType.Equals(value) Then
                Me._AverageFilterType = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Average Filter Type. </summary>
    ''' <param name="value"> The  Average Filter Type. </param>
    ''' <returns> The <see cref="AverageFilterType">Average Filter Type</see> or none if unknown. </returns>
    Public Function ApplyAverageFilterType(ByVal value As AverageFilterType) As AverageFilterType?
        Me.WriteAverageFilterType(value)
        Return Me.QueryAverageFilterType()
    End Function

    ''' <summary> Gets the Trigger AverageFilterType query command. </summary>
    ''' <value> The Trigger AverageFilterType query command. </value>
    ''' <remarks> SCPI: ":TRIG:DIR" </remarks>
    Protected Overridable ReadOnly Property AverageFilterTypeQueryCommand As String

    ''' <summary> Queries the Trigger AverageFilterType. </summary>
    ''' <returns> The <see cref="AverageFilterType">Trigger AverageFilterType</see> or none if unknown. </returns>
    Public Function QueryAverageFilterType() As AverageFilterType?
        Me.AverageFilterType = Me.Query(Of AverageFilterType)(Me.AverageFilterTypeQueryCommand, Me.AverageFilterType)
        Return Me.AverageFilterType
    End Function

    ''' <summary> Gets the Trigger AverageFilterType command format. </summary>
    ''' <value> The Trigger AverageFilterType command format. </value>
    ''' <remarks> SCPI: ":TRIG:DIR {0}" </remarks>
    Protected Overridable ReadOnly Property AverageFilterTypeCommandFormat As String

    ''' <summary> Writes the Trigger AverageFilterType without reading back the value from the device. </summary>
    ''' <param name="value"> The Trigger AverageFilterType. </param>
    ''' <returns> The <see cref="AverageFilterType">Trigger AverageFilterType</see> or none if unknown. </returns>
    Public Function WriteAverageFilterType(ByVal value As AverageFilterType) As AverageFilterType?
        Me.AverageFilterType = Me.Write(Of AverageFilterType)(Me.AverageFilterTypeCommandFormat, value)
        Return Me.AverageFilterType
    End Function

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

''' <summary> Values that represent average filter types. </summary>
Public Enum AverageFilterType
    <ComponentModel.Description("Repeat (REP)")> Repeat
    <ComponentModel.Description("Moving (MOV)")> Moving
End Enum

