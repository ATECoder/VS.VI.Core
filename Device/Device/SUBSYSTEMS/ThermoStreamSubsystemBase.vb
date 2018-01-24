Imports isr.Core.Pith.StopwatchExtensions
''' <summary> Defines the contract that must be implemented by a Thermo Stream Subsystem. </summary>
''' <license> (c) 2015 Integrated Scientific ReSenses, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="03/17/2015" by="David" revision="3.0.5554"> Created. </history>
Public MustInherit Class ThermostreamSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ThermostreamSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.CycleCountRange = Core.Pith.RangeI.FullNonNegative
        Me.DeviceThermalConstantRange = Core.Pith.RangeI.FullNonNegative
        Me.LowRampRateRange = Core.Pith.RangeR.FullNonNegative
        Me.HighRampRateRange = Core.Pith.RangeR.FullNonNegative
        Me.SetpointRange = Core.Pith.RangeR.Full
        Me.SetpointNumberRange = Core.Pith.RangeI.FullNonNegative
        Me.SetpointWindowRange = Core.Pith.RangeR.FullNonNegative
        Me.SoakTimeRange = Core.Pith.RangeI.FullNonNegative
        Me.MaximumTestTimeRange = Core.Pith.RangeI.FullNonNegative
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.HeadDown = New Boolean?
        Me.MaximumTestTime = New Double?
        Me.RampRate = New Double?
        Me.Setpoint = New Double?
        Me.SetpointNumber = New Integer?
        Me.SetpointWindow = New Double?
        Me.SoakTime = New Double?
        Me.Temperature = New Double?
        Me.TemperatureEventStatus = New Integer?
        Me.RampRateUnit = Arebis.StandardUnits.TemperatureUnits.DegreesCelsiusPerMinute
    End Sub

#End Region

#Region " CYCLE COUNT "

    Private _CycleCountRange As Core.Pith.RangeI
    ''' <summary> The Range of the CycleCount. </summary>
    Public Property CycleCountRange As Core.Pith.RangeI
        Get
            Return Me._CycleCountRange
        End Get
        Set(value As Core.Pith.RangeI)
            If Me.CycleCountRange <> value Then
                Me._CycleCountRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> the Cycle Count. </summary>
    Private _CycleCount As Integer?

    ''' <summary> Gets or sets the cached CycleCount. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property CycleCount As Integer?
        Get
            Return Me._CycleCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.CycleCount, value) Then
                Me._CycleCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Cycle Count. </summary>
    ''' <param name="value"> the Cycle Count. </param>
    ''' <returns> the Cycle Count. </returns>
    Public Function ApplyCycleCount(ByVal value As Integer) As Integer?
        Me.WriteCycleCount(value)
        Return Me.QueryCycleCount
    End Function

    ''' <summary> Gets or sets the Cycle Count query command. </summary>
    ''' <value> the Cycle Count query command. </value>
    Protected Overridable ReadOnly Property CycleCountQueryCommand As String

    ''' <summary> Queries the Cycle Count. </summary>
    ''' <returns> the Cycle Count or none if unknown. </returns>
    Public Function QueryCycleCount() As Integer?
        Me.CycleCount = Me.Query(Me.CycleCount, Me.CycleCountQueryCommand)
        Return Me.CycleCount
    End Function

    ''' <summary> Gets or sets the Cycle Count command format. </summary>
    ''' <value> the Cycle Count command format. </value>
    Protected Overridable ReadOnly Property CycleCountCommandFormat As String

    ''' <summary> Writes the Cycle Count without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Cycle Count. </remarks>
    ''' <param name="value"> the Cycle Count. </param>
    ''' <returns> the Cycle Count. </returns>
    Public Function WriteCycleCount(ByVal value As Integer) As Integer?
        Me.CycleCount = Me.Write(value, Me.CycleCountCommandFormat)
        Return Me.CycleCount
    End Function

#End Region

#Region " CYCLE NUMBER "

    ''' <summary> the Cycle Number. </summary>
    Private _CycleNumber As Integer?

    ''' <summary> Gets or sets the cached CycleNumber. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property CycleNumber As Integer?
        Get
            Return Me._CycleNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.CycleNumber, value) Then
                Me._CycleNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Cycle Number query command. </summary>
    ''' <value> the Cycle Number query command. </value>
    Protected Overridable ReadOnly Property CycleNumberQueryCommand As String

    ''' <summary> Queries the Cycle Number. </summary>
    ''' <returns> the Cycle Number or none if unknown. </returns>
    Public Function QueryCycleNumber() As Integer?
        Me.CycleNumber = Me.Query(Me.CycleNumber, Me.CycleNumberQueryCommand)
        Return Me.CycleNumber
    End Function

#End Region

#Region " CYCLING: START / STOP "

    ''' <summary> Gets or sets the command execution refractory time span. </summary>
    ''' <value> The command execution refractory time span. </value>
    Public MustOverride ReadOnly Property CommandRefractoryTimeSpan As TimeSpan

    ''' <summary> Gets or sets the start cycling command. </summary>
    ''' <value> The start cycling command. </value>
    Protected Overridable ReadOnly Property StartCyclingCommand As String

    ''' <summary> Starts a cycling. </summary>
    Public Sub StartCycling()
        Me.Session.Execute(Me.StartCyclingCommand)
    End Sub

    ''' <summary> Gets or sets the stop cycling command. </summary>
    ''' <value> The stop cycling command. </value>
    Protected Overridable ReadOnly Property StopCyclingCommand As String

    ''' <summary> Stops a cycling. </summary>
    Public Sub StopCycling()
        Me.Session.Execute(Me.StopCyclingCommand)
    End Sub

#End Region

#Region " DEVICE ERROR "

    ''' <summary> The DeviceError. </summary>
    Private _DeviceError As String

    ''' <summary> Gets or sets the cached DeviceError. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property DeviceError As String
        Get
            Return Me._DeviceError
        End Get
        Protected Set(ByVal value As String)
            If Not Nullable.Equals(Me.DeviceError, value) Then
                Me._DeviceError = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the DeviceError query command. </summary>
    ''' <value> The DeviceError query command. </value>
    Protected Overridable ReadOnly Property DeviceErrorQueryCommand As String

    ''' <summary> Queries the DeviceError. </summary>
    ''' <returns> The DeviceError or none if unknown. </returns>
    Public Function QueryDeviceError() As String
        Me.DeviceError = Me.Query(Me.DeviceError, Me.DeviceErrorQueryCommand)
        Return Me.DeviceError
    End Function

#End Region

#Region " DEVICE CONTROL "

    ''' <summary> Device Control. </summary>
    Private _DeviceControl As Boolean?

    ''' <summary> Gets or sets the cached Device Control sentinel. </summary>
    ''' <value> <c>null</c> if Device Control is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property DeviceControl As Boolean?
        Get
            Return Me._DeviceControl
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.DeviceControl, value) Then
                Me._DeviceControl = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Device Control sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyDeviceControl(ByVal value As Boolean) As Boolean?
        Me.WriteDeviceControl(value)
        Return Me.QueryDeviceControl()
    End Function

    ''' <summary> Gets or sets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    ''' <remarks> SCPI: "system:RANG:AUTO?" </remarks>
    Protected Overridable ReadOnly Property DeviceControlQueryCommand As String

    ''' <summary> Queries the Device Control sentinel. Also sets the
    ''' <see cref="DeviceControl">Device Control</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryDeviceControl() As Boolean?
        Me.DeviceControl = Me.Query(Me.DeviceControl, Me.DeviceControlQueryCommand)
        Return Me.DeviceControl
    End Function

    ''' <summary> Gets or sets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    ''' <remarks> SCPI: "HEAD {0:'1';'1';'0'}" </remarks>
    Protected Overridable ReadOnly Property DeviceControlCommandFormat As String

    ''' <summary> Writes the Device Control sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteDeviceControl(ByVal value As Boolean) As Boolean?
        Me.DeviceControl = Me.Write(value, Me.DeviceControlCommandFormat)
        Return Me.DeviceControl
    End Function

#End Region

#Region " DEVICE SENSOR TYPE "

    ''' <summary> the Device Sensor Type. </summary>
    Private _DeviceSensorType As DeviceSensorType?

    ''' <summary> Gets or sets the cached DeviceSensorType. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property DeviceSensorType As DeviceSensorType?
        Get
            Return Me._DeviceSensorType
        End Get
        Protected Set(ByVal value As DeviceSensorType?)
            If Not Nullable.Equals(Me.DeviceSensorType, value) Then
                Me._DeviceSensorType = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Device Sensor Type. </summary>
    ''' <param name="value"> the Device Sensor Type. </param>
    ''' <returns> the Device Sensor Type. </returns>
    Public Function ApplyDeviceSensorType(ByVal value As DeviceSensorType) As DeviceSensorType?
        Me.WriteDeviceSensorType(value)
        Return Me.QueryDeviceSensorType
    End Function

    ''' <summary> Gets or sets the Device Sensor Type query command. </summary>
    ''' <value> the Device Sensor Type query command. </value>
    Protected Overridable ReadOnly Property DeviceSensorTypeQueryCommand As String

    ''' <summary> Queries the Device Sensor Type. </summary>
    ''' <returns> the Device Sensor Type or none if unknown. </returns>
    Public Function QueryDeviceSensorType() As DeviceSensorType?
        Me.DeviceSensorType = Me.QueryValue(Of DeviceSensorType)(Me.DeviceSensorTypeQueryCommand, Me.DeviceSensorType)
        Return Me.DeviceSensorType
    End Function

    ''' <summary> Gets or sets the Device Sensor Type command format. </summary>
    ''' <value> the Device Sensor Type command format. </value>
    Protected Overridable ReadOnly Property DeviceSensorTypeCommandFormat As String

    ''' <summary> Writes the Device Sensor Type without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Device Sensor Type. </remarks>
    ''' <param name="value"> the Device Sensor Type. </param>
    ''' <returns> the Device Sensor Type. </returns>
    Public Function WriteDeviceSensorType(ByVal value As DeviceSensorType) As DeviceSensorType?
        Me.DeviceSensorType = Me.WriteValue(Of DeviceSensorType)(Me.DeviceSensorTypeCommandFormat, value)
        Return Me.DeviceSensorType
    End Function

#End Region

#Region " DEVICE THERMAL CONSTANT "

    Private _DeviceThermalConstantRange As Core.Pith.RangeI
    ''' <summary> The Device Thermal Constant range. </summary>
    Public Property DeviceThermalConstantRange As Core.Pith.RangeI
        Get
            Return Me._DeviceThermalConstantRange
        End Get
        Set(value As Core.Pith.RangeI)
            If Me.DeviceThermalConstantRange <> value Then
                Me._DeviceThermalConstantRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> the Device Thermal Constant. </summary>
    Private _DeviceThermalConstant As Integer?

    ''' <summary> Gets or sets the cached DeviceThermalConstant. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property DeviceThermalConstant As Integer?
        Get
            Return Me._DeviceThermalConstant
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.DeviceThermalConstant, value) Then
                Me._DeviceThermalConstant = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Device Thermal Constant. </summary>
    ''' <param name="value"> the Device Thermal Constant. </param>
    ''' <returns> the Device Thermal Constant. </returns>
    Public Function ApplyDeviceThermalConstant(ByVal value As Integer) As Integer?
        Me.WriteDeviceThermalConstant(value)
        Return Me.QueryDeviceThermalConstant
    End Function

    ''' <summary> Gets or sets the Device Thermal Constant query command. </summary>
    ''' <value> the Device Thermal Constant query command. </value>
    Protected Overridable ReadOnly Property DeviceThermalConstantQueryCommand As String

    ''' <summary> Queries the Device Thermal Constant. </summary>
    ''' <returns> the Device Thermal Constant or none if unknown. </returns>
    Public Function QueryDeviceThermalConstant() As Integer?
        Me.DeviceThermalConstant = Me.Query(Me.DeviceThermalConstant, Me.DeviceThermalConstantQueryCommand)
        Return Me.DeviceThermalConstant
    End Function

    ''' <summary> Gets or sets the Device Thermal Constant command format. </summary>
    ''' <value> the Device Thermal Constant command format. </value>
    Protected Overridable ReadOnly Property DeviceThermalConstantCommandFormat As String

    ''' <summary> Writes the Device Thermal Constant without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Device Thermal Constant. </remarks>
    ''' <param name="value"> the Device Thermal Constant. </param>
    ''' <returns> the Device Thermal Constant. </returns>
    Public Function WriteDeviceThermalConstant(ByVal value As Integer) As Integer?
        Me.DeviceThermalConstant = Me.Write(value, Me.DeviceThermalConstantCommandFormat)
        Return Me.DeviceThermalConstant
    End Function

#End Region

#Region " HEAD DOWN "

    ''' <summary> Head Down. </summary>
    Private _HeadDown As Boolean?

    ''' <summary> Gets or sets the cached Head Down sentinel. </summary>
    ''' <value> <c>null</c> if Head Down is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property HeadDown As Boolean?
        Get
            Return Me._HeadDown
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.HeadDown, value) Then
                Me._HeadDown = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Head Down sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyHeadDown(ByVal value As Boolean) As Boolean?
        Me.WriteHeadDown(value)
        Return Me.QueryHeadDown()
    End Function

    ''' <summary> Gets or sets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    ''' <remarks> SCPI: "system:RANG:AUTO?" </remarks>
    Protected Overridable ReadOnly Property HeadDownQueryCommand As String

    ''' <summary> Queries the Head Down sentinel. Also sets the
    ''' <see cref="HeadDown">Head down</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryHeadDown() As Boolean?
        Me.HeadDown = Me.Query(Me.HeadDown, Me.HeadDownQueryCommand)
        Return Me.HeadDown
    End Function

    ''' <summary> Gets or sets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    ''' <remarks> SCPI: "HEAD {0:'1';'1';'0'}" </remarks>
    Protected Overridable ReadOnly Property HeadDownCommandFormat As String

    ''' <summary> Writes the Head Down sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteHeadDown(ByVal value As Boolean) As Boolean?
        Me.HeadDown = Me.Write(value, Me.HeadDownCommandFormat)
        Return Me.HeadDown
    End Function

#End Region

#Region " OPERATOR / CYCLE MODE "

    ''' <summary> Gets or sets the Reset Operator Screen command. </summary>
    ''' <value> The Reset Operator Screen command. </value>
    Protected Overridable ReadOnly Property ResetOperatorScreenCommand As String

    ''' <summary> Gets or sets the refractory time span for resetting to Operator Mode. </summary>
    ''' <value> The command execution refractory time span. </value>
    Public MustOverride ReadOnly Property ResetOperatorScreenRefractoryTimeSpan As TimeSpan

    ''' <summary> Go to Reset Operator Screen. </summary>
    Public Sub ResetOperatorScreen()
        Me.Session.Execute(Me.ResetOperatorScreenCommand)
    End Sub

    ''' <summary> Gets or sets the Reset Cycle Screen command. </summary>
    ''' <value> The Reset Cycle Screen command. </value>
    Protected Overridable ReadOnly Property ResetCycleScreenCommand As String

    ''' <summary> Gets or sets the refractory time span for resetting to Cycle (manual) mode. </summary>
    ''' <value> The command execution refractory time span. </value>
    Public MustOverride ReadOnly Property ResetCycleScreenRefractoryTimeSpan As TimeSpan

    ''' <summary> Go to Reset Cycle Screen. </summary>
    Public Sub ResetCycleScreen()
        Me.Session.Execute(Me.ResetCycleScreenCommand)
    End Sub

#End Region

#Region " RAMP RATE "

    ''' <summary> Gets or sets the ramp rate unit. </summary>
    ''' <value> The ramp rate unit. </value>
    Public Property RampRateUnit As Arebis.TypedUnits.Unit = Arebis.StandardUnits.TemperatureUnits.DegreesCelsiusPerMinute

    ''' <summary> The Ramp Rate. </summary>
    Private _RampRate As Double?

    ''' <summary> Gets or sets the cached Ramp Rate. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property RampRate As Double?
        Get
            Return Me._RampRate
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.RampRate, value) Then
                Me._RampRate = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Ramp Rate. </summary>
    ''' <param name="value"> the Ramp Rate. </param>
    ''' <returns> the Ramp Rate. </returns>
    Public Function ApplyRampRate(ByVal value As Double) As Double?
        Me.WriteRampRate(value)
        Return Me.QueryRampRate
    End Function

    ''' <summary> Gets or sets the Ramp Rate query command. </summary>
    ''' <value> the Ramp Rate query command. </value>
    Protected Overridable ReadOnly Property RampRateQueryCommand As String

    ''' <summary> Queries the Ramp Rate. </summary>
    ''' <returns> the Ramp Rate or none if unknown. </returns>
    ''' <remarks> Thermostream ramp rate query of rates lower than 100 gives incorrect values. 
    ''' ramp rate, Ramp?<para>
    ''' 700,  700\r\n</para><para>
    '''   1.1,  0.1\r\n</para><para>  
    '''  99.9,  9.9\r\n</para><para>
    '''  53.5,  5.3\r\n</para><para>
    '''   5.3, 0.5\r\n</para><para>  
    '''   1.0, 0.1\r\n</para><para>  
    ''' Sending RAMP 1.1, gives 1.1 ramp on the thermostream, reading using Ramp?\n returns 0.1\r\n</para>
    '''           </remarks>
    Public Function QueryRampRate() As Double?
        Dim value As Double? = Me.Query(Me.RampRate, Me.RampRateQueryCommand)
        If Not Me.RampRate.HasValue Then
            ' if this is the first reading, set value even if low range.
            Me.RampRate = value
        ElseIf Not value.HasValue Then
            ' if failed to read, set value to indicated failure to read.
            Me.RampRate = value
        ElseIf Me.HighRampRateRange.Contains(value.Value) Then
            ' if high ramp rate, than value is correct.
            Me.RampRate = value
        Else
            ' if read low ramp rate, value is x10 too low. Leave value as written -- open loop.
        End If
        Return Me.RampRate
    End Function

    ''' <summary> Writes the Ramp Rate without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Ramp Rate. </remarks>
    ''' <param name="value"> the Ramp Rate. </param>
    ''' <returns> the Ramp Rate. </returns>
    Public Function WriteRampRate(ByVal value As Double) As Double?
        If Me.LowRampRateRange.Contains(value) Then
            Me.RampRate = Me.Write(value, Me.LowRampRateCommandFormat)
        ElseIf Me.HighRampRateRange.Contains(value) Then
            Me.RampRate = Me.Write(value, Me.HighRampRateCommandFormat)
        Else
            Throw New InvalidOperationException($"Ramp range {value} is outside both the low {Me.LowRampRateRange.ToString} and high {Me.HighRampRateRange.ToString} ranges")
        End If
        Return Me.RampRate
    End Function

#Region " LOW RAMP RATE "

    Private _LowRampRateRange As Core.Pith.RangeR
    ''' <summary> The Low Ramp Rate range in degrees per minute. </summary>
    Public Property LowRampRateRange As Core.Pith.RangeR
        Get
            Return Me._LowRampRateRange
        End Get
        Set(value As Core.Pith.RangeR)
            If Me.LowRampRateRange <> value Then
                Me._LowRampRateRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Low Ramp Rate command format. </summary>
    ''' <value> the Low Ramp Rate command format. </value>
    Protected Overridable ReadOnly Property LowRampRateCommandFormat As String

#End Region

#Region " High RAMP RATE "

    Private _HighRampRateRange As Core.Pith.RangeR
    ''' <summary> The High Ramp Rate range in degrees per minute. </summary>
    Public Property HighRampRateRange As Core.Pith.RangeR
        Get
            Return Me._HighRampRateRange
        End Get
        Set(value As Core.Pith.RangeR)
            If Me.HighRampRateRange <> value Then
                Me._HighRampRateRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the High Ramp Rate command format. </summary>
    ''' <value> the High Ramp Rate command format. </value>
    Protected Overridable ReadOnly Property HighRampRateCommandFormat As String

#End Region

#End Region

#Region " SET POINT "

    Private _SetpointRange As Core.Pith.RangeR
    ''' <summary> The Setpoint range. </summary>
    Public Property SetpointRange As Core.Pith.RangeR
        Get
            Return Me._SetpointRange
        End Get
        Set(value As Core.Pith.RangeR)
            If Me.SetpointRange <> value Then
                Me._SetpointRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> The Set Point. </summary>
    Private _Setpoint As Double?

    ''' <summary> Gets or sets the cached sense Setpoint. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Setpoint As Double?
        Get
            Return Me._Setpoint
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Setpoint, value) Then
                Me._Setpoint = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the sense Setpoint. </summary>
    ''' <param name="value"> The Set Point. </param>
    ''' <returns> The Set Point. </returns>
    Public Function ApplySetpoint(ByVal value As Double) As Double?
        Me.WriteSetpoint(value)
        Return Me.QuerySetpoint
    End Function

    ''' <summary> Gets or sets The Set Point query command. </summary>
    ''' <value> The Set Point query command. </value>
    Protected Overridable ReadOnly Property SetpointQueryCommand As String

    ''' <summary> Queries The Set Point. </summary>
    ''' <returns> The Set Point or none if unknown. </returns>
    Public Function QuerySetpoint() As Double?
        Me.Setpoint = Me.Query(Me.Setpoint, Me.SetpointQueryCommand)
        Return Me.Setpoint
    End Function

    ''' <summary> Gets or sets The Set Point command format. </summary>
    ''' <value> The Set Point command format. </value>
    Protected Overridable ReadOnly Property SetpointCommandFormat As String

    ''' <summary> Writes The Set Point without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Set Point. </remarks>
    ''' <param name="value"> The Set Point. </param>
    ''' <returns> The Set Point. </returns>
    Public Function WriteSetpoint(ByVal value As Double) As Double?
        Me.Setpoint = Me.Write(value, Me.SetpointCommandFormat)
        Return Me.Setpoint
    End Function

#End Region

#Region " SET POINT NUMBER "

    Private _SetpointNumberRange As Core.Pith.RangeI
    ''' <summary> The Setpoint Number range. </summary>
    Public Property SetpointNumberRange As Core.Pith.RangeI
        Get
            Return Me._SetpointNumberRange
        End Get
        Set(value As Core.Pith.RangeI)
            If Me.SetpointNumberRange <> value Then
                Me._SetpointNumberRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property


    ''' <summary> The Set Point Number. </summary>
    Private _SetpointNumber As Integer?

    ''' <summary> Gets or sets the cached SetpointNumber. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Property SetpointNumber As Integer?
        Get
            Return Me._SetpointNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.SetpointNumber, value) Then
                Me._SetpointNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Set Point Number. </summary>
    ''' <param name="value"> the Set Point Number. </param>
    ''' <returns> the Set Point Number. </returns>
    Public Function ApplySetpointNumber(ByVal value As Integer) As Integer?
        Me.WriteSetpointNumber(value)
        Return Me.QuerySetpointNumber
    End Function

    ''' <summary> Gets or sets the Set Point Number query command. </summary>
    ''' <value> the Set Point Number query command. </value>
    Protected Overridable ReadOnly Property SetpointNumberQueryCommand As String

    ''' <summary> Queries the Set Point Number. </summary>
    ''' <returns> the Set Point Number or none if unknown. </returns>
    Public Function QuerySetpointNumber() As Integer?
        Me.SetpointNumber = Me.Query(Me.SetpointNumber, Me.SetpointNumberQueryCommand)
        Return Me.SetpointNumber
    End Function

    ''' <summary> Gets or sets the Set Point Number command format. </summary>
    ''' <value> the Set Point Number command format. </value>
    Protected Overridable ReadOnly Property SetpointNumberCommandFormat As String

    ''' <summary> Writes the Set Point Number without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Set Point Number. </remarks>
    ''' <param name="value"> the Set Point Number. </param>
    ''' <returns> the Set Point Number. </returns>
    Public Function WriteSetpointNumber(ByVal value As Integer) As Integer?
        Me.SetpointNumber = Me.Write(value, Me.SetpointNumberCommandFormat)
        Return Me.SetpointNumber
    End Function

#End Region

#Region " SET POINT: NEXT "

    ''' <summary> Gets or sets the next setpoint refractory time span. </summary>
    ''' <value> The next setpoint refractory time span. </value>
    Public MustOverride ReadOnly Property NextSetpointRefractoryTimeSpan As TimeSpan

    ''' <summary> Gets or sets the Next Set Point command. </summary>
    ''' <value> The Next Set Point command. </value>
    Protected Overridable ReadOnly Property NextSetpointCommand As String

    ''' <summary> Advances to the next setpoint. </summary>
    Public Sub NextSetpoint()
        Me.Session.Execute(Me.NextSetpointCommand)
    End Sub

#End Region

#Region " SET POINT WINDOW "

    Private _SetpointWindowRange As Core.Pith.RangeR
    ''' <summary> The Setpoint Window range in degrees centigrade. </summary>
    Public Property SetpointWindowRange As Core.Pith.RangeR
        Get
            Return Me._SetpointWindowRange
        End Get
        Set(value As Core.Pith.RangeR)
            If Me.SetpointWindowRange <> value Then
                Me._SetpointWindowRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> the Set Point Window. </summary>
    Private _SetpointWindow As Double?

    ''' <summary> Gets or sets the cached SetpointWindow. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property SetpointWindow As Double?
        Get
            Return Me._SetpointWindow
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.SetpointWindow, value) Then
                Me._SetpointWindow = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Set Point Window. </summary>
    ''' <param name="value"> the Set Point Window. </param>
    ''' <returns> the Set Point Window. </returns>
    Public Function ApplySetpointWindow(ByVal value As Double) As Double?
        Me.WriteSetpointWindow(value)
        Return Me.QuerySetpointWindow
    End Function

    ''' <summary> Gets or sets the Set Point Window query command. </summary>
    ''' <value> the Set Point Window query command. </value>
    Protected Overridable ReadOnly Property SetpointWindowQueryCommand As String

    ''' <summary> Queries the Set Point Window. </summary>
    ''' <returns> the Set Point Window or none if unknown. </returns>
    Public Function QuerySetpointWindow() As Double?
        Me.SetpointWindow = Me.Query(Me.SetpointWindow, Me.SetpointWindowQueryCommand)
        Return Me.SetpointWindow
    End Function

    ''' <summary> Gets or sets the Set Point Window command format. </summary>
    ''' <value> the Set Point Window command format. </value>
    Protected Overridable ReadOnly Property SetpointWindowCommandFormat As String

    ''' <summary> Writes the Set Point Window without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Set Point Window. </remarks>
    ''' <param name="value"> the Set Point Window. </param>
    ''' <returns> the Set Point Window. </returns>
    Public Function WriteSetpointWindow(ByVal value As Double) As Double?
        Me.SetpointWindow = Me.Write(value, Me.SetpointWindowCommandFormat)
        Return Me.SetpointWindow
    End Function

#End Region

#Region " SOAK TIME "

    Private _SoakTimeRange As Core.Pith.RangeI
    ''' <summary> The Soak Time range in seconds. </summary>
    Public Property SoakTimeRange As Core.Pith.RangeI
        Get
            Return Me._SoakTimeRange
        End Get
        Set(value As Core.Pith.RangeI)
            If Me.SoakTimeRange <> value Then
                Me._SoakTimeRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> The Soak Time. </summary>
    Private _SoakTime As Double?

    ''' <summary> Gets or sets the cached Soak Time. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property SoakTime As Double?
        Get
            Return Me._SoakTime
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.SoakTime, value) Then
                Me._SoakTime = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Soak Time. </summary>
    ''' <param name="value"> the Soak Time. </param>
    ''' <returns> the Soak Time. </returns>
    Public Function ApplySoakTime(ByVal value As Double) As Double?
        Me.WriteSoakTime(value)
        Return Me.QuerySoakTime
    End Function

    ''' <summary> Gets or sets the Soak Time query command. </summary>
    ''' <value> the Soak Time query command. </value>
    Protected Overridable ReadOnly Property SoakTimeQueryCommand As String

    ''' <summary> Queries the Soak Time. </summary>
    ''' <returns> the Soak Time or none if unknown. </returns>
    Public Function QuerySoakTime() As Double?
        Me.SoakTime = Me.Query(Me.SoakTime, Me.SoakTimeQueryCommand)
        Return Me.SoakTime
    End Function

    ''' <summary> Gets or sets the Soak Time command format. </summary>
    ''' <value> the Soak Time command format. </value>
    Protected Overridable ReadOnly Property SoakTimeCommandFormat As String

    ''' <summary> Writes the Soak Time without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Soak Time. </remarks>
    ''' <param name="value"> the Soak Time. </param>
    ''' <returns> the Soak Time. </returns>
    Public Function WriteSoakTime(ByVal value As Double) As Double?
        Me.SoakTime = Me.Write(value, Me.SoakTimeCommandFormat)
        Return Me.SoakTime
    End Function

#End Region

#Region " TEMPERATURE "

    ''' <summary> The Temperature. </summary>
    Private _Temperature As Double?

    ''' <summary> Gets or sets the cached Temperature. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Temperature As Double?
        Get
            Return Me._Temperature
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Temperature, value) Then
                Me._Temperature = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Temperature query command. </summary>
    ''' <value> The Temperature query command. </value>
    Protected Overridable ReadOnly Property TemperatureQueryCommand As String

    ''' <summary> Queries the Temperature. </summary>
    ''' <returns> The Temperature or none if unknown. </returns>
    Public Function QueryTemperature() As Double?
        Me.Temperature = Me.Query(Me.Temperature, Me.TemperatureQueryCommand)
        Return Me.Temperature
    End Function

#End Region

#Region " MAXIMUM TEST TIME "

    Private _MaximumTestTimeRange As Core.Pith.RangeI
    ''' <summary> The maximum Test Time range in seconds. </summary>
    Public Property MaximumTestTimeRange As Core.Pith.RangeI
        Get
            Return Me._MaximumTestTimeRange
        End Get
        Set(value As Core.Pith.RangeI)
            If Me.MaximumTestTimeRange <> value Then
                Me._MaximumTestTimeRange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property


    ''' <summary> The Maximum Test Time. </summary>
    Private _MaximumTestTime As Double?

    ''' <summary> Gets or sets the cached Maximum Test Time. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property MaximumTestTime As Double?
        Get
            Return Me._MaximumTestTime
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.MaximumTestTime, value) Then
                Me._MaximumTestTime = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Maximum Test Time. </summary>
    ''' <param name="value"> the Maximum Test Time. </param>
    ''' <returns> the Maximum Test Time. </returns>
    Public Function ApplyMaximumTestTime(ByVal value As Double) As Double?
        Me.WriteMaximumTestTime(value)
        Return Me.QueryMaximumTestTime
    End Function

    ''' <summary> Gets or sets the Maximum Test Time query command. </summary>
    ''' <value> the Maximum Test Time query command. </value>
    Protected Overridable ReadOnly Property MaximumTestTimeQueryCommand As String

    ''' <summary> Queries the Maximum Test Time. </summary>
    ''' <returns> the Maximum Test Time or none if unknown. </returns>
    Public Function QueryMaximumTestTime() As Double?
        Me.MaximumTestTime = Me.Query(Me.MaximumTestTime, Me.MaximumTestTimeQueryCommand)
        Return Me.MaximumTestTime
    End Function

    ''' <summary> Gets or sets the Maximum Test Time command format. </summary>
    ''' <value> the Maximum Test Time command format. </value>
    Protected Overridable ReadOnly Property MaximumTestTimeCommandFormat As String

    ''' <summary> Writes the Maximum Test Time without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Maximum Test Time. </remarks>
    ''' <param name="value"> the Maximum Test Time. </param>
    ''' <returns> the Maximum Test Time. </returns>
    Public Function WriteMaximumTestTime(ByVal value As Double) As Double?
        Me.MaximumTestTime = Me.Write(value, Me.MaximumTestTimeCommandFormat)
        Return Me.MaximumTestTime
    End Function

#End Region

#Region " SYSTEM SCREEN "

    ''' <summary> The System Screen. </summary>
    Private _SystemScreen As Integer?

    ''' <summary> Gets or sets the cached System Screen. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property SystemScreen As Integer?
        Get
            Return Me._SystemScreen
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.SystemScreen, value) Then
                Me._SystemScreen = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the System Screen query command. </summary>
    ''' <value> The System Screen query command. </value>
    Protected Overridable ReadOnly Property SystemScreenQueryCommand As String

    ''' <summary> Queries the System Screen. </summary>
    ''' <returns> The System Screen or none if unknown. </returns>
    Public Overridable Function QuerySystemScreen() As Integer?
        Me.SystemScreen = Me.Query(Me.SystemScreen, Me.SystemScreenQueryCommand)
        Return Me.SystemScreen
    End Function

#End Region

#Region " REGISTERS "

#Region " TEMPERATURE EVENT REGISTER "

#Region " BIT MASK"

    ''' <summary> The Temperature event enable bitmask. </summary>
    Private _TemperatureEventEnableBitmask As Integer?

    ''' <summary> Gets or sets the cached value of the Temperature register event enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the Temperature events type that is specific to
    ''' the instrument The enable register gates the corresponding events for registration by the
    ''' Status Byte register. When an event bit is set and the corresponding enable bit is set, the
    ''' output (summary) of the register will set to 1, which in turn sets the summary bit of the
    ''' Status Byte Register. </remarks>
    ''' <value> The mask to use for enabling the events. </value>
    Public Property TemperatureEventEnableBitmask() As Integer?
        Get
            Return Me._TemperatureEventEnableBitmask
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.TemperatureEventEnableBitmask, value) Then
                Me._TemperatureEventEnableBitmask = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Programs and reads back the Temperature register events enable bit mask. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The bit mask or nothing if not known. </returns>
    Public Function ApplyTemperatureEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.WriteTemperatureEventEnableBitmask(value)
        Return Me.QueryTemperatureEventEnableBitmask()
    End Function

    ''' <summary> Gets or sets the temperature event enable mask query command. </summary>
    ''' <value> The temperature event enable mask query command. </value>
    ''' <remarks> 'TESE?' </remarks>
    Protected Overridable ReadOnly Property TemperatureEventEnableMaskQueryCommand As String

    ''' <summary> Reads back the Temperature register event enable bit mask. </summary>
    ''' <remarks> The returned value could be cast to the Temperature events type that is specific to
    ''' the instrument The enable register gates the corresponding events for registration by the
    ''' Status Byte register. When an event bit is set and the corresponding enable bit is set, the
    ''' output (summary) of the register will set to 1, which in turn sets the summary bit of the
    ''' Status Byte Register. </remarks>
    ''' <returns>  The mask used for enabling the events. </returns>
    Public Function QueryTemperatureEventEnableBitmask() As Integer?
        Me.TemperatureEventEnableBitmask = Me.Session.Query(Me.TemperatureEventEnableBitmask.GetValueOrDefault(0), Me.TemperatureEventEnableMaskQueryCommand)
        Return Me.TemperatureEventEnableBitmask
    End Function

    ''' <summary> Gets or sets the temperature event enable mask command format. </summary>
    ''' <value> The temperature event enable mask command format. </value>
    ''' <remarks> 'TESE {0:D}' </remarks>
    Protected Overridable ReadOnly Property TemperatureEventEnableMaskCommandFormat As String

    ''' <summary> Programs the Temperature register events enable bit mask without updating the value from
    ''' the device. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The bit mask or nothing if not known. </returns>
    Public Function WriteTemperatureEventEnableBitmask(ByVal value As Integer) As Integer?
        Me.Session.WriteLine(TemperatureEventEnableMaskCommandFormat, value)
        Me.TemperatureEventEnableBitmask = value
        Return Me.TemperatureEventEnableBitmask
    End Function

#End Region

#Region " CONDITION "

    ''' <summary> The Temperature event Condition. </summary>
    Private _TemperatureEventCondition As Integer?

    ''' <summary> Gets or sets the cached Condition of the Temperature register events. </summary>
    ''' <value> <c>null</c> if value is not known;. </value>
    Public Property TemperatureEventCondition() As Integer?
        Get
            Return Me._TemperatureEventCondition
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.TemperatureEventCondition, value) Then
                Me._TemperatureEventCondition = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the Temperature event condition query command. </summary>
    ''' <value> The Temperature event condition query command. </value>
    Protected Overridable ReadOnly Property TemperatureEventConditionQueryCommand As String

    ''' <summary> Reads the condition of the Temperature register event. </summary>
    ''' <returns> System.Nullable{System.Int32}. </returns>
    Public Overridable Function QueryTemperatureEventCondition() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.TemperatureEventConditionQueryCommand) Then
            Me.TemperatureEventCondition = Me.Session.Query(0I, Me.TemperatureEventConditionQueryCommand)
        End If
        Return Me.TemperatureEventCondition
    End Function

#End Region

#Region " STATUS "

    ''' <summary> The Temperature event status. </summary>
    Private _TemperatureEventStatus As Integer?

    ''' <summary> Gets or sets the cached status of the Temperature register events. </summary>
    ''' <value> <c>null</c> if value is not known;. </value>
    Public Overridable Property TemperatureEventStatus() As Integer?
        Get
            Return Me._TemperatureEventStatus
        End Get
        Protected Set(ByVal value As Integer?)
            Me._TemperatureEventStatus = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Gets or sets the Temperature Event status query command. </summary>
    ''' <remarks> 'TESR?' </remarks>
    ''' <value> The Temperature status query command. </value>
    Protected Overridable ReadOnly Property TemperatureEventStatusQueryCommand As String

    ''' <summary> Reads the status of the Temperature register events. </summary>
    ''' <returns> System.Nullable{System.Int32}. </returns>
    Public Overridable Function QueryTemperatureEventStatus() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.TemperatureEventStatusQueryCommand) Then
            Me.TemperatureEventStatus = Me.Session.Query(Me.TemperatureEventStatus.GetValueOrDefault(0), Me.TemperatureEventStatusQueryCommand)
        End If
        Return Me.TemperatureEventStatus
    End Function
#End Region

#End Region

#Region " AUXILIARY REGISTER "

#Region " STATUS "

    ''' <summary> The Auxiliary event status. </summary>
    Private _AuxiliaryEventStatus As Integer?

    ''' <summary> Gets or sets the cached status of the Auxiliary register events. </summary>
    ''' <value> <c>null</c> if value is not known;. </value>
    Public Overridable Property AuxiliaryEventStatus() As Integer?
        Get
            Return Me._AuxiliaryEventStatus
        End Get
        Protected Set(ByVal value As Integer?)
            Me._AuxiliaryEventStatus = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Gets or sets the Auxiliary Event status query command. </summary>
    ''' <remarks> 'AUXC?' </remarks>
    ''' <value> The Auxiliary status query command. </value>
    Protected Overridable ReadOnly Property AuxiliaryEventStatusQueryCommand As String

    ''' <summary> Reads the status of the Auxiliary register events. </summary>
    ''' <returns> System.Nullable{System.Int32}. </returns>
    Public Overridable Function QueryAuxiliaryEventStatus() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.AuxiliaryEventStatusQueryCommand) Then
            Me.AuxiliaryEventStatus = Me.Session.Query(0I, Me.AuxiliaryEventStatusQueryCommand)
        End If
        Return Me.AuxiliaryEventStatus
    End Function
#End Region

#End Region

#End Region

End Class

''' <summary> Values that represent device sensor types. </summary>
Public Enum DeviceSensorType
    <ComponentModel.Description("Air Control")> None = 0
    <ComponentModel.Description("T Thermocouple")> TThermocouple = 1
    <ComponentModel.Description("K Thermocouple")> KThermocouple = 2
End Enum
