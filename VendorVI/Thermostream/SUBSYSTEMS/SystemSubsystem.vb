''' <summary> Defines a System Subsystem for a InTest Thermo Stream instrument. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class SystemSubsystem
    Inherits VI.SystemSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.CoolingEnabled = New Boolean?
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

    ''' <summary> Gets the initialize memory command. </summary>
    ''' <value> The initialize memory command. </value>
    Protected Overrides ReadOnly Property InitializeMemoryCommand As String = ""

    ''' <summary> Gets the language revision query command. </summary>
    ''' <value> The language revision query command. </value>
    Protected Overrides ReadOnly Property LanguageRevisionQueryCommand As String = ""


#End Region

#Region " SCPI VERSION "

    Private _AutoTuningEnabled As Boolean?
    ''' <summary> Gets the cached version level of the SCPI standard implemented by the device. </summary>
    ''' <value> The Auto Tuning Enabled. </value>
    Public Property AutoTuningEnabled As Boolean?
        Get
            Return Me._AutoTuningEnabled
        End Get
        Protected Set(value As Boolean?)
            If Not Boolean?.Equals(Me.AutoTuningEnabled, value) Then
                Me._AutoTuningEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the AutoTuning enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if AutoTuning is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyAutoTuningEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoTuningEnabled(value)
        Return Me.QueryAutoTuningEnabled()
    End Function

    ''' <summary> Queries the AutoTuning enabled state. </summary>
    ''' <returns> <c>True</c> if AutoTuning is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function QueryAutoTuningEnabled() As Boolean?
        Me.AutoTuningEnabled = Me.Session.Query(Me.AutoTuningEnabled.GetValueOrDefault(True), "LRNM?")
        Return Me.AutoTuningEnabled
    End Function

    ''' <summary> Writes the AutoTuning enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if AutoTuning is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function WriteAutoTuningEnabled(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine("LRNM {0:'1';'1';'0'}", CType(value, Integer))
        Me.AutoTuningEnabled = value
        Return Me.AutoTuningEnabled
    End Function

#End Region

#Region " COOLING ENABLED "

    Private _CoolingEnabled As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether Cooling is enabled. </summary>
    ''' <value> <c>True</c> if Cooling is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </value>
    Public Property CoolingEnabled() As Boolean?
        Get
            Return Me._CoolingEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.CoolingEnabled, value) Then
                Me._CoolingEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Cooling enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if Cooling is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyCoolingEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteCoolingEnabled(value)
        Return Me.QueryCoolingEnabled()
    End Function

    ''' <summary> Queries the Cooling enabled state. </summary>
    ''' <returns> <c>True</c> if Cooling is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function QueryCoolingEnabled() As Boolean?
        Me.CoolingEnabled = Me.Session.Query(Me.CoolingEnabled.GetValueOrDefault(True), "COOL?")
        Return Me.CoolingEnabled
    End Function

    ''' <summary> Writes the Cooling enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if Cooling is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function WriteCoolingEnabled(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine("COOL {0:'1';'1';'0'}", CType(value, Integer))
        Me.CoolingEnabled = value
        Return Me.CoolingEnabled
    End Function

#End Region

#Region " DEVICE CONTROL ENABLED "

    Private _DeviceControlEnabled As Boolean?

    ''' <summary> Gets or sets a value indicating whether Device Control is enabled. </summary>
    ''' <value> <c>True</c> if Device Control mode is enabled; otherwise, <c>False</c>. </value>
    Public Property DeviceControlEnabled() As Boolean?
        Get
            Return Me._DeviceControlEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.DeviceControlEnabled, value) Then
                Me._DeviceControlEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Device Control enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if Device Control is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyDeviceControlEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteDeviceControlEnabled(value)
        Return Me.QueryDeviceControlEnabled()
    End Function

    ''' <summary> Queries the Device Control enabled state. </summary>
    ''' <returns> <c>True</c> if Device Control is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function QueryDeviceControlEnabled() As Boolean?
        Me.DeviceControlEnabled = Me.Session.Query(Me.DeviceControlEnabled.GetValueOrDefault(True), "DUTM?")
        Return Me.DeviceControlEnabled
    End Function

    ''' <summary> Writes the Device Control enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if Device Control is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function WriteDeviceControlEnabled(ByVal value As Boolean) As Boolean?
        Me.DeviceControlEnabled = New Boolean?
        Me.Session.WriteLine("DUTM {0:'1';'1';'0'}", CType(value, Integer))
        Me.DeviceControlEnabled = value
        Return Me.DeviceControlEnabled
    End Function

#End Region

End Class
