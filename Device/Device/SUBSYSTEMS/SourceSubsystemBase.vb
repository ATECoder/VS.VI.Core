Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ComboBoxExtensions
''' <summary> Defines the contract that must be implemented by a Source Subsystem. </summary>
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
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.AutoClearEnabled = False
        Me.AutoDelayEnabled = True
        Me.Delay = TimeSpan.Zero
        Me.FunctionMode = SourceFunctionModes.Voltage
        Me.SweepPoints = 2500
        Me.SupportedFunctionModes = SourceFunctionModes.Current Or SourceFunctionModes.Voltage
    End Sub

#End Region

#Region " AUTO CLEAR ENABLED "

    Private _AutoClearEnabled As Boolean?
    ''' <summary> Gets or sets the cached Auto Clear Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Clear Enabled is not known; <c>True</c> if output is on; otherwise,
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

    ''' <summary> Writes and reads back the Auto Clear Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoClearEnabled(value)
        Return Me.QueryAutoClearEnabled()
    End Function

    ''' <summary> Gets the automatic Clear enabled query command. </summary>
    ''' <value> The automatic Clear enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR:CLE:AUTO?" </remarks>
    Protected Overridable ReadOnly Property AutoClearEnabledQueryCommand As String

    ''' <summary> Queries the Auto Clear Enabled sentinel. Also sets the
    ''' <see cref="AutoClearEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoClearEnabled() As Boolean?
        Me.AutoClearEnabled = Me.Query(Me.AutoClearEnabled, Me.AutoClearEnabledQueryCommand)
        Return Me.AutoClearEnabled
    End Function

    ''' <summary> Gets the automatic Clear enabled command Format. </summary>
    ''' <value> The automatic Clear enabled query command. </value>
    ''' <remarks> SCPI: ":SOU:CLE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoClearEnabledCommandFormat As String

    ''' <summary> Writes the Auto Clear Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoClearEnabled = Me.Write(value, Me.AutoClearEnabledCommandFormat)
        Return Me.AutoClearEnabled
    End Function

#End Region

#Region " AUTO DELAY ENABLED "

    Private _AutoDelayEnabled As Boolean?
    ''' <summary> Gets or sets the cached Auto Delay Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Delay Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoDelayEnabled As Boolean?
        Get
            Return Me._AutoDelayEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoDelayEnabled, value) Then
                Me._AutoDelayEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Auto Delay Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoDelayEnabled(value)
        Return Me.QueryAutoDelayEnabled()
    End Function

    ''' <summary> Gets the automatic delay enabled query command. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR:DEL:AUTO?" </remarks>
    Protected Overridable ReadOnly Property AutoDelayEnabledQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="AutoDelayEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryAutoDelayEnabled() As Boolean?
        Me.AutoDelayEnabled = Me.Query(Me.AutoDelayEnabled, Me.AutoDelayEnabledQueryCommand)
        Return Me.AutoDelayEnabled
    End Function

    ''' <summary> Gets the automatic delay enabled command Format. </summary>
    ''' <value> The automatic delay enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR:DEL:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property AutoDelayEnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.AutoDelayEnabled = Me.Write(value, Me.AutoDelayEnabledCommandFormat)
        Return Me.AutoDelayEnabled
    End Function

#End Region

#Region " DELAY "

    ''' <summary> The delay. </summary>
    Private _Delay As TimeSpan?

    ''' <summary> Gets or sets the cached Source Delay. </summary>
    ''' <remarks> The delay is used to delay operation in the Source layer. After the programmed
    ''' Source event occurs, the instrument waits until the delay period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Source Delay or none if not set or unknown. </value>
    Public Overloads Property Delay As TimeSpan?
        Get
            Return Me._Delay
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Nullable.Equals(Me.Delay, value) Then
                Me._Delay = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Source Delay. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Source Delay or none if unknown. </returns>
    Public Function ApplyDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteDelay(value)
        Me.QueryDelay()
    End Function

    ''' <summary> Gets or sets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    ''' <remarks> SCPI: ":SOUR:DEL?" </remarks>
    Protected Overridable ReadOnly Property DelayQueryCommand As String

    ''' <summary> Gets or sets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    ''' <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property DelayFormat As String

    ''' <summary> Queries the Delay. </summary>
    ''' <returns> The Delay or none if unknown. </returns>
    Public Function QueryDelay() As TimeSpan?
        Me.Delay = Me.Query(Me.Delay, Me.DelayFormat, Me.DelayQueryCommand)
        Return Me.Delay
    End Function

    ''' <summary> Gets or sets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    ''' <remarks> SCPI: ":SOUR:DEL {0:s\.FFFFFFF}" </remarks>
    Protected Overridable ReadOnly Property DelayCommandFormat As String

    ''' <summary> Writes the Source Delay without reading back the value from the device. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Source Delay or none if unknown. </returns>
    Public Function WriteDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.Delay = Me.Write(value, Me.DelayCommandFormat)
        Return Me.Delay
    End Function

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

#Region " SWEEP POINTS "

    Private _SweepPoint As Integer?
    ''' <summary> Gets or sets the cached Sweep Points. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Sweep Points or none if not set or unknown. </value>
    Public Overloads Property SweepPoints As Integer?
        Get
            Return Me._SweepPoint
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.SweepPoints, value) Then
                Me._SweepPoint = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sweep Points. </summary>
    ''' <param name="value"> The current SweepPoints. </param>
    ''' <returns> The SweepPoints or none if unknown. </returns>
    Public Function ApplySweepPoints(ByVal value As Integer) As Integer?
        Me.WriteSweepPoints(value)
        Return Me.QuerySweepPoints()
    End Function

    ''' <summary> Gets Sweep Points query command. </summary>
    ''' <value> The Sweep Points query command. </value>
    ''' <remarks> SCPI: ":SOUR:SWE:POIN?" </remarks>
    Protected Overridable ReadOnly Property SweepPointsQueryCommand As String

    ''' <summary> Queries the current Sweep Points. </summary>
    ''' <returns> The Sweep Points or none if unknown. </returns>
    Public Function QuerySweepPoints() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.SweepPointsQueryCommand) Then
            Me.SweepPoints = Me.Session.Query(0I, Me.SweepPointsQueryCommand)
        End If
        Return Me.SweepPoints
    End Function

    ''' <summary> Gets Sweep Points command format. </summary>
    ''' <value> The Sweep Points command format. </value>
    ''' <remarks> SCPI: ":SOUR:SWE:POIN {0}" </remarks>
    Protected Overridable ReadOnly Property SweepPointsCommandFormat As String

    ''' <summary> Write the Trace PointsSweepPoints without reading back the value from the device. </summary>
    ''' <param name="value"> The current PointsSweepPoints. </param>
    ''' <returns> The PointsSweepPoints or none if unknown. </returns>
    Public Function WriteSweepPoints(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.SweepPointsCommandFormat) Then
            Me.Session.WriteLine(Me.SweepPointsCommandFormat, value)
        End If
        Me.SweepPoints = value
        Return Me.SweepPoints
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

#Region " UNUSED "
#If False Then
#Region " AUTO CLEAR "

    ''' <summary> The automatic clear enabled. </summary>
    Private _AutoClearEnabled As Boolean?

    ''' <summary> Gets or sets the cached source Auto Clear enabled state. </summary>
    ''' <value> <c>True</c> if the Auto Clear is enabled; <c>False</c> if not enabled, or none if
    ''' unknown or not set. </value>
    Public Overloads Property AutoClearEnabled As Boolean?
        Get
            Return Me._AutoClearEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoClearEnabled, value) Then
                Me._AutoClearEnabled = value
                Me.SafePostPropertyChanged(NameOf(Me.AutoClearEnabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the state of the source Auto Clear. </summary>
    ''' <param name="value"> Enable if set to <c>true</c>; otherwise, disable. </param>
    ''' <returns> <c>True</c> if the Auto Clear is enabled; <c>False</c> if not enabled, or none if
    ''' unknown. </returns>
    Public Function ApplyAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoClearEnabled(value)
        Return Me.QueryAutoClearEnabled()
    End Function

    ''' <summary> Queries the source AutoClear state. </summary>
    ''' <returns> <c>True</c> if the Auto Clear is enabled; <c>False</c> if not enabled, or none if
    ''' unknown. </returns>
    Public MustOverride Function QueryAutoClearEnabled() As Boolean?

    ''' <summary> Writes the state of the Source Auto Clear without reading back the value from the
    ''' device. </summary>
    ''' <param name="value"> Enable if set to <c>true</c>; otherwise, disable. </param>
    ''' <returns> <c>True</c> if the Auto Clear is enabled; <c>False</c> if not enabled, or none if
    ''' unknown. </returns>
    Public MustOverride Function WriteAutoClearEnabled(ByVal value As Boolean) As Boolean?

#End Region

#Region " AUTO DELAY "

    ''' <summary> The automatic delay enabled. </summary>
    Private _AutoDelayEnabled As Boolean?

    ''' <summary> Gets or sets the cached source Auto Delay enabled state. </summary>
    ''' <value> <c>True</c> if the Auto Delay is enabled; <c>False</c> if not enabled, or none if
    ''' unknown or not set. </value>
    Public Overloads Property AutoDelayEnabled As Boolean?
        Get
            Return Me._AutoDelayEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoDelayEnabled, value) Then
                Me._AutoDelayEnabled = value
                Me.SafePostPropertyChanged(NameOf(Me.AutoDelayEnabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the state of the Source Auto Delay. </summary>
    ''' <param name="value"> Enable if set to <c>true</c>; otherwise, disable. </param>
    ''' <returns> <c>True</c> if the Auto Delay is enabled; <c>False</c> if not enabled, or none if
    ''' unknown or not set. </returns>
    Public Function ApplyAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoDelayEnabled(value)
        Return Me.QueryAutoDelayEnabled()
    End Function

    ''' <summary> Queries the Source Auto Delay state. </summary>
    ''' <returns> <c>True</c> if the Auto Delay is enabled; <c>False</c> if not enabled, or none if
    ''' unknown or not set. </returns>
    Public MustOverride Function QueryAutoDelayEnabled() As Boolean?

    ''' <summary> Writes the state of the Source Auto Delay without reading back the value from the
    ''' device. </summary>
    ''' <param name="value"> Enable if set to <c>true</c>; otherwise, disable. </param>
    ''' <returns> <c>True</c> if the Auto Delay is enabled; <c>False</c> if not enabled, or none if
    ''' unknown or not set. </returns>
    Public MustOverride Function WriteAutoDelayEnabled(ByVal value As Boolean) As Boolean?

#End Region

#Region " DELAY "

    ''' <summary> The delay. </summary>
    Private _Delay As TimeSpan?

    ''' <summary> Gets or sets the cached Source Delay. </summary>
    ''' <remarks> The delay is used to delay operation in the Source layer. After the programmed
    ''' Source event occurs, the instrument waits until the delay period expires before performing
    ''' the Device Action. </remarks>
    ''' <value> The Source Delay or none if not set or unknown. </value>
    Public Overloads Property Delay As TimeSpan?
        Get
            Return Me._Delay
        End Get
        Protected Set(ByVal value As TimeSpan?)
            If Not Nullable.Equals(Me.Delay, value) Then
                Me._Delay = value
                Me.SafePostPropertyChanged(NameOf(Me.Delay))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Source Delay. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Source Delay or none if unknown. </returns>
    Public Function ApplyDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.WriteDelay(value)
        Me.QueryDelay()
    End Function

    ''' <summary> Gets or sets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    ''' <remarks> SCPI: ":SOUR:DEL?" </remarks>
    Protected Overridable ReadOnly Property DelayQueryCommand As String

    ''' <summary> Gets or sets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    ''' <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property DelayFormat As String

    ''' <summary> Queries the Delay. </summary>
    ''' <returns> The Delay or none if unknown. </returns>
    Public Function QueryDelay() As TimeSpan?
        Me.Delay = Me.Query(Me.Delay, Me.DelayFormat, Me.DelayQueryCommand)
        Return Me.Delay
    End Function

    ''' <summary> Gets or sets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    ''' <remarks> SCPI: ":SOUR:DEL {0:s\.FFFFFFF}" </remarks>
    Protected Overridable ReadOnly Property DelayCommandFormat As String

    ''' <summary> Writes the Source Delay without reading back the value from the device. </summary>
    ''' <param name="value"> The current Delay. </param>
    ''' <returns> The Source Delay or none if unknown. </returns>
    Public Function WriteDelay(ByVal value As TimeSpan) As TimeSpan?
        Me.Delay = Me.Write(value, Me.DelayCommandFormat)
        Return Me.Delay
    End Function

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
                Me.SafePostPropertyChanged(NameOf(Me.SupportedFunctionModes))
            End If
        End Set
    End Property

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
                Me.SafePostPropertyChanged(NameOf(Me.FunctionMode))
            End If
        End Set
    End Property

    ''' <summary> Queries the Source Function Mode. </summary>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public MustOverride Function QueryFunctionMode() As SourceFunctionModes?

    ''' <summary> Writes and reads back the source Function Mode. </summary>
    ''' <param name="value"> The  Source Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As SourceFunctionModes) As SourceFunctionModes?
        Me.WriteFunctionMode(value)
        Return Me.QueryFunctionMode()
    End Function

    ''' <summary> Writes the source Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public MustOverride Function WriteFunctionMode(ByVal value As SourceFunctionModes) As SourceFunctionModes?

#End Region

#Region " SWEEP POINTS "

    ''' <summary> The sweep points. </summary>
    Private _SweepPoints As Integer?

    ''' <summary> Gets or sets the cached source Sweep Points. </summary>
    ''' <value> The source Sweep Points or none if not set or unknown. </value>
    Public Overloads Property SweepPoints As Integer?
        Get
            Return Me._SweepPoints
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.SweepPoints, value) Then
                Me._SweepPoints = value
                Me.SafePostPropertyChanged(NameOf(Me.SweepPoints))
            End If
        End Set
    End Property

    ''' <summary> Queries the current Sweep Points. </summary>
    ''' <returns> The SweepPoints or none if unknown. </returns>
    Public MustOverride Function QuerySweepPoints() As Integer?

    ''' <summary> Writes and reads back the source Sweep Points. </summary>
    ''' <param name="value"> The current Sweep Points. </param>
    ''' <returns> The SweepPoints or none if unknown. </returns>
    Public Function ApplySweepPoints(ByVal value As Integer) As Integer?
        Me.WriteSweepPoints(value)
        Return Me.QuerySweepPoints()
    End Function

    ''' <summary> Sets back the source Sweep Points without reading back the value from the device. </summary>
    ''' <param name="value"> The current Sweep Points. </param>
    ''' <returns> The SweepPoints or none if unknown. </returns>
    Public MustOverride Function WriteSweepPoints(ByVal value As Integer) As Integer?

#End Region


#End If
#End Region