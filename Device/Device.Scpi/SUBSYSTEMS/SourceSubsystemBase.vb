Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ComboBoxExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Source Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SourceSubsystemBase
    Inherits VI.SourceSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FunctionMode = SourceFunctionModes.Voltage
        Me.SupportedFunctionModes = SourceFunctionModes.Current Or SourceFunctionModes.Voltage
    End Sub

#End Region


#Region " SYNTAX "

#Region " AUTO CLEAR ENABLED "

    ''' <summary> Gets the automatic Clear enabled query command. </summary>
    ''' <value> The automatic Clear enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR:CLE:AUTO?" </remarks>
    Protected Overrides ReadOnly Property AutoClearEnabledQueryCommand As String = ":SOUR:CLE:AUTO?"

    ''' <summary> Gets or sets the automatic Clear enabled command Format. </summary>
    ''' <value> The automatic Clear enabled query command. </value>
    Protected Overrides ReadOnly Property AutoClearEnabledCommandFormat As String = ":SOUR:CLE:AUTO {0:'ON';'ON';'OFF'}"

#End Region

#Region " AUTO DELAY ENABLED "

    ''' <summary> Gets the automatic Delay enabled query command. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR:CLE:AUTO?" </remarks>
    Protected Overrides ReadOnly Property AutoDelayEnabledQueryCommand As String = ":SOUR:DEL:AUTO?"

    ''' <summary> Gets or sets the automatic Delay enabled command Format. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    Protected Overrides ReadOnly Property AutoDelayEnabledCommandFormat As String = ":SOUR:DEL:AUTO {0:'ON';'ON';'OFF'}"

#End Region

#Region " FUNCTION MODE "
#If False Then
    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command, e.g., :SOUR:FUNC? </value>
    Protected Overrides ReadOnly Property FunctionModeQueryCommand As String = ":SOUR:FUNC?"

    ''' <summary> Gets or sets the function mode command. </summary>
    ''' <value> The function mode command, e.g., :SOUR:FUNC {0}. </value>
    Protected Overrides ReadOnly Property FunctionModeCommandFormat As String = ":SOUR:FUNC {0}"
#End If

#End Region

#Region " SWEEP POINTS "

    ''' <summary> Gets or sets Sweep Points query command. </summary>
    ''' <value> The Sweep Points query command. </value>
    Protected Overrides ReadOnly Property SweepPointsQueryCommand As String = ":SOUR:SWE:POIN?"

    ''' <summary> Gets or sets Sweep Points command format. </summary>
    ''' <value> The Sweep Points command format. </value>
    Protected Overrides ReadOnly Property SweepPointsCommandFormat As String = ":SOUR:SWE:POIN {0}"

#End Region

#End Region

#Region " FUNCTION MODE "

    Private _SupportedFunctionModes As SourceFunctionModes
    ''' <summary>
    ''' Gets or sets the supported Function Mode.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedFunctionModes() As SourceFunctionModes
        Get
            Return _SupportedFunctionModes
        End Get
        Set(ByVal value As SourceFunctionModes)
            If Not Me.SupportedFunctionModes.Equals(value) Then
                Me._SupportedFunctionModes = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Displays a supported function modes. </summary>
    ''' <param name="listControl"> The list control. </param>
    Public Sub DisplaySupportedFunctionModes(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        With listControl
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(SourceFunctionModes).ValueDescriptionPairs(Me.SupportedFunctionModes)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            If .Items.Count > 0 AndAlso Me.FunctionMode.HasValue Then
                .SelectedItem = Me.FunctionMode.Value.ValueDescriptionPair()
            End If
        End With
    End Sub

    ''' <summary> Returns the function mode selected by the list control. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    ''' <returns> The SenseFunctionModes. </returns>
    Public Shared Function SelectedFunctionMode(ByVal listControl As Windows.Forms.ComboBox) As SourceFunctionModes
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Return CType(CType(listControl.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, SourceFunctionModes)
    End Function

    ''' <summary> Safe select function mode. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    Public Sub SafeSelectFunctionMode(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        If Me.FunctionMode.HasValue Then
            listControl.SafeSelectItem(Me.FunctionMode.Value, Me.FunctionMode.Value.Description)
        End If
    End Sub

    ''' <summary> The function mode. </summary>
    Private _FunctionMode As SourceFunctionModes?

    ''' <summary> Gets or sets the cached source FunctionMode. </summary>
    ''' <value> The <see cref="SourceFunctionModes">source Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FunctionMode As SourceFunctionModes?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As SourceFunctionModes?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Function Mode. </summary>
    ''' <param name="value"> The  Source Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As SourceFunctionModes) As SourceFunctionModes?
        Me.WriteFunctionMode(value)
        Return Me.QueryFunctionMode()
    End Function

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command, e.g., :SOUR:FUNC? </value>
    Protected Overridable ReadOnly Property FunctionModeQueryCommand As String

    ''' <summary> Queries the Source Function Mode. </summary>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public Function QueryFunctionMode() As SourceFunctionModes?
        If String.IsNullOrWhiteSpace(Me.FunctionModeQueryCommand) Then
            Me.FunctionMode = New SourceFunctionModes?
        Else
            Dim mode As String = Me.FunctionMode.ToString
            Me.Session.MakeEmulatedReplyIfEmpty(mode)
            mode = Me.Session.QueryTrimEnd(Me.FunctionModeQueryCommand)
            If String.IsNullOrWhiteSpace(mode) Then
                Dim message As String = "Failed fetching source function mode"
                Debug.Assert(Not Debugger.IsAttached, message)
                Me.FunctionMode = New SourceFunctionModes?
            Else
                Dim se As New StringEnumerator(Of SourceFunctionModes)
                Me.FunctionMode = se.ParseContained(mode.BuildDelimitedValue)
            End If
        End If
        Return Me.FunctionMode
    End Function

    ''' <summary> Gets or sets the function mode command. </summary>
    ''' <value> The function mode command, e.g., :SOUR:FUNC {0}. </value>
    Protected Overridable ReadOnly Property FunctionModeCommandFormat As String

    ''' <summary> Writes the source Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public Function WriteFunctionMode(ByVal value As SourceFunctionModes) As SourceFunctionModes?
        If Not String.IsNullOrWhiteSpace(Me.FunctionModeCommandFormat) Then
            Me.Session.WriteLine(Me.FunctionModeCommandFormat, value.ExtractBetween())
        End If
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

#End Region

End Class

''' <summary>
''' Specifies the source function modes. Using flags permits using these values to define the
''' supported function modes.
''' </summary>
<Flags>
Public Enum SourceFunctionModes
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Voltage (VOLT)")> Voltage = 1
    <ComponentModel.Description("Current (CURR)")> Current = Voltage << 1
    <ComponentModel.Description("Memory (MEM)")> Memory = Current << 1
    <ComponentModel.Description("DC Voltage (VOLT:DC)")> VoltageDC = Memory << 1
    <ComponentModel.Description("DC Current (CURR:DC)")> CurrentDC = VoltageDC << 1
    <ComponentModel.Description("AC Voltage (VOLT:AC)")> VoltageAC = CurrentDC << 1
    <ComponentModel.Description("AC Current (CURR:AC)")> CurrentAC = VoltageAC << 1
End Enum


