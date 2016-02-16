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
        Me.FunctionMode = SourceFunctionMode.Voltage
        Me.SweepPoints = 2500
    End Sub

#End Region

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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.AutoClearEnabled))
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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.AutoDelayEnabled))
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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Delay))
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
    ''' <remarks> For example: "s\.fff" will convert the result from seconds. </remarks>
    Protected Overridable ReadOnly Property DelayFormat As String

    ''' <summary> Queries the Delay. </summary>
    ''' <returns> The Delay or none if unknown. </returns>
    Public Function QueryDelay() As TimeSpan?
        Me.Delay = Me.Query(Me.Delay, Me.DelayFormat, Me.DelayQueryCommand)
        Return Me.Delay
    End Function

    ''' <summary> Gets or sets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    ''' <remarks> SCPI: ":SOUR:DEL {0:s\.fff}" </remarks>
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

    ''' <summary> The function mode. </summary>
    Private _FunctionMode As SourceFunctionMode?

    ''' <summary> Gets or sets the cached source FunctionMode. </summary>
    ''' <value> The <see cref="SourceFunctionMode">source Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FunctionMode As SourceFunctionMode?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As SourceFunctionMode?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.FunctionMode))
            End If
        End Set
    End Property

    ''' <summary> Queries the Source Function Mode. </summary>
    ''' <returns> The <see cref="SourceFunctionMode">source Function Mode</see> or none if unknown. </returns>
    Public MustOverride Function QueryFunctionMode() As SourceFunctionMode?

    ''' <summary> Writes and reads back the source Function Mode. </summary>
    ''' <param name="value"> The  Source Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionMode">source Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As SourceFunctionMode) As SourceFunctionMode?
        Me.WriteFunctionMode(value)
        Return Me.QueryFunctionMode()
    End Function

    ''' <summary> Writes the source Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionMode">source Function Mode</see> or none if unknown. </returns>
    Public MustOverride Function WriteFunctionMode(ByVal value As SourceFunctionMode) As SourceFunctionMode?

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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SweepPoints))
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

End Class

''' <summary> Specifies the source function modes. </summary>
Public Enum SourceFunctionMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Voltage (VOLT)")> Voltage
    <ComponentModel.Description("Current (CURR)")> Current
    <ComponentModel.Description("Memory (MEM)")> Memory
    <ComponentModel.Description("DC Voltage (VOLT:DC)")> VoltageDC
    <ComponentModel.Description("DC Current (CURR:DC)")> CurrentDC
    <ComponentModel.Description("AC Voltage (VOLT:AC)")> VoltageAC
    <ComponentModel.Description("AC Current (CURR:AC)")> CurrentAC
End Enum

