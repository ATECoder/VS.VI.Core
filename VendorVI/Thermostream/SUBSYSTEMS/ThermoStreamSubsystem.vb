Imports System.Windows.Forms
Imports isr.Core.Pith.StopwatchExtensions
''' <summary> Defines a SCPI Sense Current Subsystem for a InTest Thermo Stream instrument. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="03/01/2014" by="David" revision="3.0.5173"> Created. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="Thermostream")>
Public Class ThermostreamSubsystem
    Inherits VI.Scpi.ThermostreamSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ThermoStreamSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._TemperatureEventInfo = New TemperatureEventInfo(TemperatureEvents.None)
        Me._AuxiliaryStatusInfo = New AuxiliaryStatusInfo(AuxiliaryStatuses.None)
        Me._thermalProfile = New ThermalProfileCollection
        Me._AmbientTemperature = 25
    End Sub
#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.QuerySystemScreen()
        If Not Me.OperatorScreen.GetValueOrDefault(False) Then
            Me.ApplyOperatorScreen()
        End If
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.AsyncNotifyPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " READ TEMPERATURE "

    ''' <summary> Reads current set point values. </summary>
    Public Sub ReadCurrentSetpointValues()
        Me.QuerySetpointNumber()
        Me.QuerySetpointWindow()
        Me.QuerySoakTime()
        Me.QuerySetpoint()
        Me.QueryRampRate()
    End Sub

    ''' <summary> Searches for the zero-based last setpoint number. </summary>
    ''' <returns> The found setpoint number. </returns>
    Public Function FindLastSetpointNumber() As Integer
        Dim currentSetpoint As Integer = Me.SetpointNumber.GetValueOrDefault(-1)
        Dim setPointNumber As Integer = -1
        For i As Integer = Me.SetpointNumberRange.Min To Me.SetpointNumberRange.Max
            Me.ApplySetpointNumber(i)
            If Me.Session.IsSessionOpen Then Diagnostics.Stopwatch.StartNew.Wait(Me.NextSetpointRefractoryTimeSpan)
            Me.QueryRampRate()
            If Me.RampRate.HasValue AndAlso Math.Abs(Me.RampRate.Value) > 0.01 Then
                setPointNumber = i
            Else
                Exit For
            End If
        Next
        If currentSetpoint >= 0 Then Me.ApplySetpointNumber(currentSetpoint)
        Return setPointNumber
    End Function

#End Region

#Region " DEVICE ERROR "

    ''' <summary> Gets the DeviceError query command. </summary>
    ''' <value> The DeviceError query command. </value>
    Protected Overrides ReadOnly Property DeviceErrorQueryCommand As String = "EROR?"

#End Region

#Region " CYCLE COUNT "

    ''' <summary> The Cycle Count range. </summary>
    Public Overrides ReadOnly Property CycleCountRange As isr.Core.Pith.RangeI = New isr.Core.Pith.RangeI(1, 9999)

    ''' <summary> Gets the Cycle Count command format. </summary>
    ''' <value> the Cycle Count command format. </value>
    Protected Overrides ReadOnly Property CycleCountCommandFormat As String = "CYCC {0}"

    ''' <summary> Gets the Cycle Count query command. </summary>
    ''' <value> the Cycle Count query command. </value>
    Protected Overrides ReadOnly Property CycleCountQueryCommand As String = "CYCC?"

#End Region

#Region " CYCLE NUMBER "

    ''' <summary> Gets or sets the Cycle Number query command. </summary>
    ''' <value> the Cycle Number query command. </value>
    Protected Overrides ReadOnly Property CycleNumberQueryCommand As String = "CYCL?"

#End Region

#Region " CYCLING: START / STOP "

    ''' <summary> Gets or sets the command execution refractory time span. </summary>
    ''' <value> The command execution refractory time span. </value>
    Public Overrides ReadOnly Property CommandRefractoryTimeSpan As TimeSpan = TimeSpan.FromMilliseconds(5)

    ''' <summary> Gets or sets the start cycling command. </summary>
    ''' <value> The start cycling command. </value>
    Protected Overrides ReadOnly Property StartCyclingCommand As String = "CYCL 1"


    ''' <summary> Gets or sets the stop cycling command. </summary>
    ''' <value> The stop cycling command. </value>
    Protected Overrides ReadOnly Property StopCyclingCommand As String = "CYCL 0"

    ''' <summary> true if use cycles completed. </summary>
    Public Const UseCyclesCompleted As Boolean = False

    ''' <summary> Start cycling and wait till confirmed. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="countdown"> The countdown. </param>
    ''' <param name="e">         Event information to send to registered event handlers. </param>
    Public Overloads Sub StartCycling(ByVal countdown As Integer, ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))

        If Not e.Cancel Then
            ' send the start command
            Me.StartCycling()
            Dim cycleNumber As Integer = -1
            Dim cycleCompleted As Boolean = True
            Dim setpointNumber As Integer = -1
            Do
                countdown -= 1
                Application.DoEvents()
                Diagnostics.Stopwatch.StartNew.Wait(Me.CommandRefractoryTimeSpan)
                Me.QueryCycleNumber()
                cycleNumber = Me.CycleNumber.GetValueOrDefault(-1)
                If cycleNumber > 0 Then
                    Me.QueryTemperatureEventStatus()
                    cycleCompleted = Me.IsCycleCompleted OrElse (ThermostreamSubsystem.UseCyclesCompleted AndAlso Me.IsCyclesCompleted)
                    Me.QuerySetpointNumber()
                    setpointNumber = Me.SetpointNumber.GetValueOrDefault(-1)
                End If
            Loop Until countdown = 0 OrElse (cycleNumber > 0 AndAlso Not cycleCompleted AndAlso setpointNumber = 0)
            If Me.IsCyclingStopped Then
                e.RegisterCancellation("Thermo-Stream cycle stopped on error. Testing will stop.")
            ElseIf e.Cancel Then
            ElseIf Not cycleCompleted Then
                e.RegisterCancellation("Thermo-Stream failed starting - cycle is tagged as completed after sending start. Testing will stop.")
            ElseIf cycleNumber <= 0 Then
                e.RegisterCancellation("Thermo-Stream failed starting - cycle number = {0}. Testing will stop.", cycleNumber)
            ElseIf setpointNumber > 0 Then
                e.RegisterCancellation("Thermo-Stream failed starting - set point = {0}. Testing will stop.", setpointNumber)
            Else
                ' got a start
            End If
        End If

    End Sub

    ''' <summary> Stop cycling and wait till confirmed. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="countdown"> The countdown. </param>
    ''' <param name="e">         Event information to send to registered event handlers. </param>
    Public Overloads Sub StopCycling(ByVal countdown As Integer, ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))

        If Not e.Cancel Then
            ' send the start command
            Me.StopCycling()
            Dim cycleNumber As Integer = -1
            Dim headUp As Boolean = False
            Do
                countdown -= 1
                Application.DoEvents()
                Diagnostics.Stopwatch.StartNew.Wait(Me.CommandRefractoryTimeSpan)
                Me.QueryCycleNumber()
                cycleNumber = Me.CycleNumber.GetValueOrDefault(-1)
                If cycleNumber > 0 Then
                    Me.QueryAuxiliaryEventStatus()
                    headUp = Me.IsHeadUp
                End If
            Loop Until countdown = 0 OrElse cycleNumber = 0 AndAlso headUp
            If e.Cancel Then
            ElseIf Not headUp Then
                e.RegisterCancellation("Thermo-Stream failed stopping--head is not up. Testing will stop.")
            ElseIf cycleNumber > 0 Then
                e.RegisterCancellation("Thermo-Stream failed starting--cycle number = {0}. Testing will stop.", cycleNumber)
            Else
                ' got a stop
            End If
        End If

    End Sub

#End Region

#Region " DEVICE CONTROL "

    ''' <summary> Gets the Device Control command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property DeviceControlCommandFormat As String = "DUTM {0:        '1';'1';'0'}"

    ''' <summary> Gets the Device Control query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property DeviceControlQueryCommand As String = "DUTM?"

#End Region

#Region " DEVICE SENSOR TYPE "

    ''' <summary> Gets the Device Sensor Type command format. </summary>
    ''' <value> the Device Sensor Type command format. </value>
    Protected Overrides ReadOnly Property DeviceSensorTypeCommandFormat As String = "DSNS {0}"

    ''' <summary> Gets the Device Sensor Type query command. </summary>
    ''' <value> the Device Sensor Type query command. </value>
    Protected Overrides ReadOnly Property DeviceSensorTypeQueryCommand As String = "DSNS?"

#End Region

#Region " DEVICE THERMAL CONSTANT "

    ''' <summary> The Device Thermal Constant range. </summary>
    Public Overrides ReadOnly Property DeviceThermalConstantRange As isr.Core.Pith.RangeI = New isr.Core.Pith.RangeI(20, 500)

    ''' <summary> Gets the Device Thermal Constant command format. </summary>
    ''' <value> the Device Thermal Constant command format. </value>
    Protected Overrides ReadOnly Property DeviceThermalConstantCommandFormat As String = "DUTC {0}"

    ''' <summary> Gets the Device Thermal Constant query command. </summary>
    ''' <value> the Device Thermal Constant query command. </value>
    Protected Overrides ReadOnly Property DeviceThermalConstantQueryCommand As String = "DUTC?"

#End Region

#Region " HEAD STATUS "

    ''' <summary> Gets the head down command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property HeadDownCommandFormat As String = "HEAD {0:'1';'1';'0'}"

    ''' <summary> Gets the head down query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property HeadDownQueryCommand As String = "HEAD?"

#End Region

#Region " RESET OPERATOR / CYCLE MODE "

    ''' <summary>
    ''' Gets or sets the refractory time span for resetting to Cycle (manual) mode.
    ''' </summary>
    ''' <value> The command execution refractory time span. </value>
    Public Overrides ReadOnly Property ResetCycleScreenRefractoryTimeSpan As TimeSpan = TimeSpan.FromMilliseconds(4000)

    ''' <summary> Gets the Reset Cycle Screen command. </summary>
    ''' <value> The Reset Cycle Screen command. </value>
    Protected Overrides ReadOnly Property ResetCycleScreenCommand As String = "*RST"

    ''' <summary> Cycle Screen. </summary>
    Private _CycleScreen As Boolean?

    ''' <summary> Gets or sets the cached Cycle Screen sentinel. </summary>
    ''' <value> <c>null</c> if Cycle Screen is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property CycleScreen As Boolean?
        Get
            Return Me._CycleScreen
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.CycleScreen, value) Then
                Me._CycleScreen = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.CycleScreen))
            End If
        End Set
    End Property

    ''' <summary> Gets the Cycle screen value. </summary>
    ''' <value> The Cycle screen value. </value>
    Public Property CycleScreenValue As Integer = 10

    ''' <summary> Queries the Operator Screen sentinel. Also sets the
    ''' <see cref="OperatorScreen">Operator Screen</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Overrides Function QuerySystemScreen() As Integer?
        MyBase.QuerySystemScreen()
        If Me.SystemScreen.HasValue Then
            Me.OperatorScreen = Me.SystemScreen.Value = Me.OperatorScreenValue
            Me.CycleScreen = Me.SystemScreen.Value = Me.CycleScreenValue
        Else
            Me.OperatorScreen = New Boolean?
            Me.CycleScreen = New Boolean?
        End If
    End Function

    ''' <summary> Queries the Cycle Screen sentinel. Also sets the
    ''' <see cref="CycleScreen">Cycle Screen</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryCycleScreen() As Boolean?
        Me.QuerySystemScreen()
        Return Me.CycleScreen
    End Function

    ''' <summary> Applies the operator screen. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    Public Sub ApplyCyclesScreen()
        Me.ResetCycleScreen()
        Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Awaiting reset to Cycles (manual mode) screen;. ")
        Diagnostics.Stopwatch.StartNew.Wait(Me.ResetCycleScreenRefractoryTimeSpan)
        Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Querying system screen;. ")
        Me.QuerySystemScreen()
    End Sub

    ''' <summary> Gets or sets the refractory time span for resetting to Operator Mode. </summary>
    ''' <value> The command execution refractory time span. </value>
    Public Overrides ReadOnly Property ResetOperatorScreenRefractoryTimeSpan As TimeSpan = TimeSpan.FromMilliseconds(1000)

    ''' <summary> Gets the Reset Operator Screen command. </summary>
    ''' <value> The Reset Operator Screen command. </value>
    Protected Overrides ReadOnly Property ResetOperatorScreenCommand As String = "RSTO"

    ''' <summary> Operator Screen. </summary>
    Private _OperatorScreen As Boolean?

    ''' <summary> Gets or sets the cached Operator Screen sentinel. </summary>
    ''' <value> <c>null</c> if Operator Screen is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property OperatorScreen As Boolean?
        Get
            Return Me._OperatorScreen
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.OperatorScreen, value) Then
                Me._OperatorScreen = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.OperatorScreen))
            End If
        End Set
    End Property

    ''' <summary> Gets the operator screen value. </summary>
    ''' <value> The operator screen value. </value>
    Public Property OperatorScreenValue As Integer = 10

    ''' <summary> Queries the Operator Screen sentinel. Also sets the
    ''' <see cref="OperatorScreen">Operator Screen</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryOperatorScreen() As Boolean?
        Me.QuerySystemScreen()
        Return Me.OperatorScreen
    End Function

    ''' <summary> Applies the operator screen. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    Public Sub ApplyOperatorScreen()
        Me.ResetOperatorScreen()
        Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Awaiting reset to Operator screen;. ")
        Diagnostics.Stopwatch.StartNew.Wait(Me.ResetOperatorScreenRefractoryTimeSpan)
        Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Querying system screen;. ")
        Me.QuerySystemScreen()
    End Sub

#End Region

#Region " RAMP RATE "

    ''' <summary> The Low Ramp Rate range in degrees per minute. </summary>
    Public Overrides ReadOnly Property LowRampRateRange As isr.Core.Pith.RangeR = New isr.Core.Pith.RangeR(0, 99.99)

    ''' <summary> The High Ramp Rate range in degrees per minute. </summary>
    Public Overrides ReadOnly Property HighRampRateRange As isr.Core.Pith.RangeR = New isr.Core.Pith.RangeR(100, 9999)

    ''' <summary> Gets Low the Ramp Rate command format. </summary>
    ''' <value> the Low Ramp Rate command format. </value>
    Protected Overrides ReadOnly Property LowRampRateCommandFormat As String = "RAMP {0:0.0}"

    ''' <summary> Gets High the Ramp Rate command format. </summary>
    ''' <value> the High Ramp Rate command format. </value>
    Protected Overrides ReadOnly Property HighRampRateCommandFormat As String = "RAMP {0:0}"

    ''' <summary> Gets the Ramp Rate query command. </summary>
    ''' <value> the Ramp Rate query command. </value>
    Protected Overrides ReadOnly Property RampRateQueryCommand As String = "RAMP?"

#End Region

#Region " SET POINT "

    ''' <summary> The Setpoint range. </summary>
    Public Overrides ReadOnly Property SetpointRange As isr.Core.Pith.RangeR = New isr.Core.Pith.RangeR(-99.9, 225)

    ''' <summary> Gets The Set Point command format. </summary>
    ''' <value> The Set Point command format. </value>
    Protected Overrides ReadOnly Property SetpointCommandFormat As String = "SETP {0}"

    ''' <summary> Gets The Set Point query command. </summary>
    ''' <value> The Set Point query command. </value>
    Protected Overrides ReadOnly Property SetpointQueryCommand As String = "SETP?"

#End Region

#Region " SET POINT: NEXT "

    ''' <summary> Gets or sets the next setpoint refractory time span. </summary>
    ''' <value> The next setpoint refractory time span. </value>
    Public Overrides ReadOnly Property NextSetpointRefractoryTimeSpan As TimeSpan = TimeSpan.FromMilliseconds(100)

    ''' <summary> Gets or sets the Next Set Point command. </summary>
    ''' <value> The Next Set Point command. </value>
    Protected Overrides ReadOnly Property NextSetpointCommand As String = "NEXT"

    Private _cycleCompletedCache As Boolean

    ''' <summary> Gets a value indicating whether the cycle completed cache. </summary>
    ''' <value> <c>true</c> if cycle completed cache; otherwise <c>false</c> </value>
    Public ReadOnly Property CycleCompletedCache As Boolean
        Get
            Return Me._cycleCompletedCache
        End Get
    End Property

    ''' <summary> Query if this object is cycle ended. </summary>
    ''' <returns> <c>true</c> if cycle ended; otherwise <c>false</c> </returns>
    Public Function QueryCycleCompleted(ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim affirmative As Boolean = False
        Me.QueryTemperatureEventStatus()
        affirmative = Me.IsCycleCompleted OrElse (ThermostreamSubsystem.UseCyclesCompleted AndAlso Me.IsCyclesCompleted)
        If Not affirmative Then
            Me.QueryAuxiliaryEventStatus()
            affirmative = Me.IsHeadUp
            If Not affirmative Then
                Me.QueryRampRate()
                affirmative = Me.RampRate.GetValueOrDefault(0) < 0.1
            End If
        End If
        Return affirmative
    End Function

    ''' <summary> Move next setpoint. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="countdown"> The countdown. </param>
    ''' <param name="e">         Cancel event information. </param>
    Public Sub MoveNextSetpoint(ByVal countdown As Integer, ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        ' otherwise, save the current setpoint number
        Me.QuerySetpointNumber()
        If Me.SetpointNumber.HasValue Then
            Dim setpointNumber As Integer = Me.SetpointNumber.Value
            ' move next.
            Me.NextSetpoint()
            Me._cycleCompletedCache = False
            Do
                countdown -= 1
                Application.DoEvents()
                Diagnostics.Stopwatch.StartNew.Wait(Me.NextSetpointRefractoryTimeSpan)
                Me._cycleCompletedCache = Me.QueryCycleCompleted(e)
                If Not (e.Cancel OrElse Me._cycleCompletedCache) Then
                    Me.QuerySetpointNumber()
                End If
            Loop Until countdown = 0 OrElse Me._cycleCompletedCache OrElse e.Cancel OrElse
                    Me.SetpointNumber.GetValueOrDefault(setpointNumber) <> setpointNumber
            If Me.IsCyclingStopped Then
                e.RegisterCancellation("Thermo-Stream cycle stopped on error. Testing will stop.")
            ElseIf e.Cancel Then
            ElseIf Me._cycleCompletedCache Then
                ' done with this cycle, no need to check the new setpoint number
            ElseIf Me.SetpointNumber.GetValueOrDefault(setpointNumber) = setpointNumber Then
                e.RegisterCancellation("Thermo-Stream failed advancing the setpoint number from {0}. Testing will stop.", setpointNumber)
            Else
                ' got a new setpoint number
            End If
        Else
            e.RegisterCancellation("Thermo-Stream failed reading the setpoint number--value is empty. Testing will stop.")
            Application.DoEvents()
        End If
    End Sub

    ''' <summary> Queries if cycles stopped or terminated; Otherwise, steps to the next temperature. </summary>
    ''' <remarks> This is the same as indicating that the start of test signal was received. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="countdown"> The countdown. </param>
    ''' <param name="e">         Cancel event information. </param>
    Public Sub ConditionalCycleMoveNext(ByVal countdown As Integer, ByVal additionalInfo As String,
                                        ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If String.IsNullOrEmpty(additionalInfo) Then additionalInfo = "waiting for At-Temp"
        Me.QueryTemperatureEventStatus()
        If Me.IsCycleCompleted Then
            e.RegisterCancellation("Thermo-Stream cycle completed while {0}", additionalInfo)
        ElseIf ThermostreamSubsystem.UseCyclesCompleted AndAlso Me.IsCyclesCompleted Then
            e.RegisterCancellation("Thermo-Stream completed all cycles while  {0}", additionalInfo)
        ElseIf Me.IsCyclingStopped Then
            e.RegisterCancellation("Thermo-Stream cycle was stopped while  {0}", additionalInfo)
        Else
            Me.ValidateHandlerHeadDown(e)
            If e.Cancel Then
                e.RegisterCancellation("Unable to proceed {0};. Details: {1}", additionalInfo, e.Details)
            Else
                Me.MoveNextSetpoint(countdown, e)
            End If
        End If
    End Sub

#End Region

#Region " SET POINT NUMBER "

    ''' <summary> The Setpoint Number range. </summary>
    Public Overrides ReadOnly Property SetpointNumberRange As isr.Core.Pith.RangeI = New isr.Core.Pith.RangeI(0, 11)

    ''' <summary> Gets the Set Point Number command format. </summary>
    ''' <value> the Set Point Number command format. </value>
    Protected Overrides ReadOnly Property SetpointNumberCommandFormat As String = "SETN {0}"

    ''' <summary> Gets the Set Point Number query command. </summary>
    ''' <value> the Set Point Number query command. </value>
    Protected Overrides ReadOnly Property SetpointNumberQueryCommand As String = "SETN?"

#End Region

#Region " SET POINT WINDOW "

    ''' <summary> The Setpoint Window range in degrees centigrade. </summary>
    Public Overrides ReadOnly Property SetpointWindowRange As isr.Core.Pith.RangeR = New isr.Core.Pith.RangeR(0.1, 9.9)

    ''' <summary> Gets the Set Point Window command format. </summary>
    ''' <value> the Set Point Window command format. </value>
    Protected Overrides ReadOnly Property SetpointWindowCommandFormat As String = "WNDW {0}"

    ''' <summary> Gets the Set Point Window query command. </summary>
    ''' <value> the Set Point Window query command. </value>
    Protected Overrides ReadOnly Property SetpointWindowQueryCommand As String = "WNDW?"

#End Region

#Region " SOAK TIME "

    ''' <summary> The Soak Time range in seconds. </summary>
    Public Overrides ReadOnly Property SoakTimeRange As isr.Core.Pith.RangeR = New isr.Core.Pith.RangeR(0, 9999)

    ''' <summary> Gets the Soak Time command format. </summary>
    ''' <value> the Soak Time command format. </value>
    Protected Overrides ReadOnly Property SoakTimeCommandFormat As String = "SOAK {0}"

    ''' <summary> Gets the Soak Time query command. </summary>
    ''' <value> the Soak Time query command. </value>
    Protected Overrides ReadOnly Property SoakTimeQueryCommand As String = "SOAK?"

#End Region

#Region " TEMPERATURE "

    ''' <summary> Gets the Temperature query command. </summary>
    ''' <value> The Temperature query command. </value>
    Protected Overrides ReadOnly Property TemperatureQueryCommand As String = "TEMP?"

#End Region

#Region " MAXIMUM TEST TIME "

    ''' <summary> The maximum Test Time range in seconds. </summary>
    Public Overrides ReadOnly Property MaximumTestTimeRange As isr.Core.Pith.RangeR = New isr.Core.Pith.RangeR(0, 9999)

    ''' <summary> Gets the Maximum Test Time command format. </summary>
    ''' <value> the Maximum Test Time command format. </value>
    Protected Overrides ReadOnly Property MaximumTestTimeCommandFormat As String = "TTIM {0}"

    ''' <summary> Gets the Maximum Test Time query command. </summary>
    ''' <value> the Maximum Test Time query command. </value>
    Protected Overrides ReadOnly Property MaximumTestTimeQueryCommand As String = "TTIM?"

#End Region

#Region " SYSTEM SCREEN "

    ''' <summary> Gets the System Screen query command. </summary>
    ''' <value> The System Screen query command. </value>
    Protected Overrides ReadOnly Property SystemScreenQueryCommand As String = "WHAT?"

#End Region

#End Region

#Region " PROFILE "

    Private _thermalProfile As ThermalProfileCollection

    ''' <summary> Thermal profile. </summary>
    ''' <returns> A ThermalProfileCollection. </returns>
    Public Function ThermalProfile() As ThermalProfileCollection
        Return Me._thermalProfile
    End Function

    ''' <summary> Creates new profile. </summary>
    Public Sub CreateNewProfile()
        Me._thermalProfile = New ThermalProfileCollection
    End Sub

    Private _ActiveThermalSetpoint As ThermalSetpoint

    ''' <summary> Gets the active thermal profile setpoint. </summary>
    ''' <value> The active thermal setpoint. </value>
    Public ReadOnly Property ActiveThermalSetpoint As ThermalSetpoint
        Get
            Return Me._ActiveThermalSetpoint
        End Get
    End Property

    ''' <summary> Adds a new setpoint. </summary>
    ''' <param name="number"> Number of. </param>
    ''' <returns> A ThermalSetpoint. </returns>
    Public Function AddNewSetpoint(ByVal number As Integer) As ThermalSetpoint
        Return Me._thermalProfile.AddNewSetpoint(number)
    End Function

    ''' <summary> Gets the ambient temperature. </summary>
    ''' <value> The ambient temperature. </value>
    Public Property AmbientTemperature As Integer

    Public Property HotSetpointNumber As Integer = 0
    Public Property AmbientSetpointNumber As Integer = 1
    Public Property ColdSetpointNumber As Integer = 2

    ''' <summary> Parse setpoint number. </summary>
    ''' <param name="temperature"> The temperature. </param>
    ''' <returns> An Integer. </returns>
    Public Function ParseSetpointNumber(ByVal temperature As Double) As Integer
        If temperature < Me.AmbientTemperature Then
            Return Me.ColdSetpointNumber
        ElseIf temperature > Me.AmbientTemperature Then
            Return Me.HotSetpointNumber
        Else
            Return Me.AmbientSetpointNumber
        End If
    End Function

    ''' <summary> Builds a command. </summary>
    ''' <param name="setpoint"> The setpoint. </param>
    ''' <returns> A String. </returns>
    Public Function BuildCommand(ByVal setpoint As ThermalSetpoint) As String
        If setpoint Is Nothing Then Throw New ArgumentNullException(NameOf(setpoint))
        Dim builder As New Text.StringBuilder()
        Dim delimiter As String = "; "
        builder.AppendFormat(Me.SetpointNumberCommandFormat, Me.ParseSetpointNumber(setpoint.Temperature))
        builder.Append(delimiter)
        builder.AppendFormat(Me.SetpointCommandFormat, setpoint.Temperature)
        builder.Append(delimiter)
        Dim value As Double = 60 * setpoint.RampRate
        If Me.LowRampRateRange.Contains(value) Then
            builder.AppendFormat(Me.LowRampRateCommandFormat, value)
        ElseIf Me.HighRampRateRange.Contains(value) Then
            builder.AppendFormat(Me.HighRampRateCommandFormat, value)
        Else
            Throw New InvalidOperationException($"Ramp range {value} is outside both the low {Me.LowRampRateRange.ToString} and high {Me.HighRampRateRange.ToString} ranges")
        End If
        builder.Append(delimiter)
        builder.AppendFormat(Me.SoakTimeCommandFormat, Math.Max(Me.SoakTimeRange.Min, Math.Min(Me.SoakTimeRange.Max, setpoint.SoakSeconds)))
        builder.Append(delimiter)
        builder.AppendFormat(Me.SetpointWindowCommandFormat, Math.Max(Me.SetpointWindowRange.Min, Math.Min(Me.SetpointWindowRange.Max, setpoint.Window)))
        Return builder.ToString
    End Function

    ''' <summary> Applies the active thermal setpoint described by setpoint. </summary>
    Public Sub ApplyActiveThermalSetpoint()
        Me.Session.Execute(Me.BuildCommand(Me.ActiveThermalSetpoint))
        Application.DoEvents()
    End Sub

    ''' <summary> Select active thermal setpoint. </summary>
    ''' <param name="thermalProfileSetpointNumber"> The thermal profile setpoint number. </param>
    ''' <param name="e">                            Cancel event information. </param>
    Public Sub SelectActiveThermalSetpoint(ByVal thermalProfileSetpointNumber As Integer, ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Me.ThermalProfile Is Nothing Then
            e.RegisterCancellation("Thermo-Stream thermal profile not set")
        ElseIf Me.ThermalProfile.Count = 0 Then
            e.RegisterCancellation("Thermo-Stream thermal profile is empty")
        ElseIf thermalProfileSetpointNumber < 1 Then
            e.RegisterCancellation("Thermo-Stream thermal profile setpoint number {0} must be positive", thermalProfileSetpointNumber)
        ElseIf thermalProfileSetpointNumber > Me.ThermalProfile.Count Then
            e.RegisterCancellation("Thermo-Stream thermal profile setpoint number {0} is out of range [1,{0}]", thermalProfileSetpointNumber, Me.ThermalProfile.Count)
        Else
            Me._ActiveThermalSetpoint = Me.ThermalProfile.Item(thermalProfileSetpointNumber)
        End If
    End Sub

    ''' <summary> Validates the thermal setpoint. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="setpoint"> The setpoint. </param>
    ''' <param name="e">        Cancel event information. </param>
    Public Sub ValidateThermalSetpoint(ByVal setpoint As ThermalSetpoint, ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If setpoint Is Nothing Then Throw New ArgumentNullException(NameOf(setpoint))
        e.RegisterCancellation("")
        Me.QuerySetpointNumber()
        If Me.SetpointNumber.HasValue Then
            Me.QuerySetpoint()
            If Me.Setpoint.HasValue Then
                Me.QueryRampRate()
                If Me.RampRate.HasValue Then
                    Me.QuerySoakTime()
                    If Me.SoakTime.HasValue Then
                        Me.QuerySetpointWindow()
                        If Me.SetpointWindow.HasValue Then
                            If Me.SetpointNumber.Value = Me.ParseSetpointNumber(setpoint.Temperature) Then
                                If Math.Abs(Me.Setpoint.Value - setpoint.Temperature) < 0.1F Then
                                    If Math.Abs(Me.RampRate.Value - 60 * setpoint.RampRate) < 1.1F Then
                                        If Math.Abs(Me.SoakTime.Value - setpoint.SoakSeconds) < 1.1F Then
                                            If Math.Abs(Me.SetpointWindow.Value - setpoint.Window) < 0.1F Then
                                            Else
                                                e.RegisterCancellation("Thermo-Stream failed setting the setpoint window {0} to {1} {2}.",
                                                                       Me.SetpointWindow.Value, setpoint.Window,
                                                                       Arebis.StandardUnits.TemperatureUnits.DegreeCelsius.Symbol)
                                            End If
                                        Else
                                            e.RegisterCancellation("Thermo-Stream failed setting the soak time {0} to {1} s.",
                                                                   Me.SoakTime.Value, setpoint.SoakSeconds)
                                        End If
                                    Else
                                        e.RegisterCancellation("Thermo-Stream failed setting the ramp rate {0} to {1} {2}.",
                                                               Me.RampRate.Value, 60 * setpoint.RampRate, Me.RampRateUnit.symbol)
                                    End If
                                Else
                                    e.RegisterCancellation("Thermo-Stream failed setting the setpoint {0} to {1} {2}.",
                                                           Me.Setpoint.Value, setpoint.Temperature,
                                                           Arebis.StandardUnits.TemperatureUnits.DegreeCelsius.Symbol)
                                End If
                            Else
                                e.RegisterCancellation("Thermo-Stream failed setting the setpoint number {0} to {1}.",
                                                    Me.SetpointNumber.Value, Me.ParseSetpointNumber(setpoint.Temperature))
                            End If
                        Else
                            e.RegisterCancellation("Thermo-Stream failed reading the setpoint window--value is empty.")
                        End If
                    Else
                        e.RegisterCancellation("Thermo-Stream failed reading the soak time--value is empty.")
                    End If
                Else
                    e.RegisterCancellation("Thermo-Stream failed reading the ramp rate--value is empty.")
                End If
            Else
                e.RegisterCancellation("Thermo-Stream failed reading the setpoint--value is empty.")
            End If
        Else
            e.RegisterCancellation("Thermo-Stream failed reading the setpoint number--value is empty.")
        End If
        Application.DoEvents()
    End Sub

    ''' <summary> Conditional apply setpoint. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="thermalProfileSetpointNumber"> The thermal profile setpoint number. </param>
    ''' <param name="countdown">                    The countdown. </param>
    ''' <param name="e">                            Cancel event information. </param>
    Public Sub ConditionalApplySetpoint(ByVal thermalProfileSetpointNumber As Integer, ByVal countdown As Integer,
                                        ByVal additionalInfo As String, ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If Not e.Cancel Then
            If String.IsNullOrWhiteSpace(additionalInfo) Then additionalInfo = $"applying setpoint {thermalProfileSetpointNumber};"
            Me.SelectActiveThermalSetpoint(thermalProfileSetpointNumber, e)
            If Not e.Cancel Then
                Me.ApplyActiveThermalSetpoint()
                Do
                    Diagnostics.Stopwatch.StartNew.Wait(Me.CommandRefractoryTimeSpan)
                    Me.ValidateThermalSetpoint(Me.ThermalProfile.Item(thermalProfileSetpointNumber), e)
                    countdown -= 1
                Loop Until Not e.Cancel OrElse countdown = 0
            End If
            If e.Cancel Then
                e.RegisterCancellation("Thermo-Stream failed {0};. Details: {1}", additionalInfo, e.Details)
            End If
        End If
    End Sub

    ''' <summary> Conditional move first. This assumes the head is still up. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="thermalProfileSetpointNumber"> The thermal profile setpoint number. </param>
    ''' <param name="countdown">                    The countdown. </param>
    ''' <param name="e">                            Cancel event information. </param>
    Public Sub ConditionalMoveFirst1(ByVal thermalProfileSetpointNumber As Integer, ByVal countdown As Integer,
                                    ByVal additionalInfo As String, ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If String.IsNullOrWhiteSpace(additionalInfo) Then additionalInfo = "selecting first profile @ head up; clear to start"
        Me.ValidateHandlerHeadUp(e)
        If e.Cancel Then
            e.RegisterCancellation("Thermo-Stream is unable to start while {0};. Details: {1}", additionalInfo, e.Details)
        Else
            Me.ConditionalApplySetpoint(thermalProfileSetpointNumber, countdown, additionalInfo, e)
        End If
    End Sub

    ''' <summary> Conditional move next. Expects the head to be down. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="thermalProfileSetpointNumber"> The thermal profile setpoint number. </param>
    ''' <param name="countdown">                    The countdown. </param>
    ''' <param name="e">                            Cancel event information. </param>
    Public Sub ConditionalMoveNext1(ByVal thermalProfileSetpointNumber As Integer, ByVal countdown As Integer,
                                   ByVal additionalInfo As String,
                                   ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If String.IsNullOrWhiteSpace(additionalInfo) Then additionalInfo = "moving to next profile"
        Me.ValidateHandlerHeadDown(e)
        If e.Cancel Then
            e.RegisterCancellation("Thermo-Stream is unable to start while {0};. Details: {1}", additionalInfo, e.Details)
        Else
            Me.ConditionalApplySetpoint(thermalProfileSetpointNumber, countdown, additionalInfo, e)
        End If
    End Sub

    ''' <summary> Query if 'setpointNumber' exceeds the profile count. </summary>
    ''' <param name="setpointNumber"> The setpoint number. </param>
    ''' <returns> <c>true</c> if profile completed; otherwise <c>false</c> </returns>
    Public Function IsProfileCompleted(ByVal setpointNumber As Integer) As Boolean
        Dim affirmative As Boolean = False
        If Me.ThermalProfile Is Nothing Then
        ElseIf Me.ThermalProfile.Count = 0 Then
        ElseIf setpointNumber < 1 Then
        ElseIf setpointNumber > Me.ThermalProfile.Count Then
            affirmative = True
        End If
        Return affirmative
    End Function

    ''' <summary> Query if 'setpointNumber' exceeds the profile count. </summary>
    ''' <param name="setpointNumber"> The setpoint number. </param>
    ''' <param name="e">              Cancel event information. </param>
    ''' <returns> <c>true</c> if profile completed; otherwise <c>false</c> </returns>
    Public Function IsProfileCompleted(ByVal setpointNumber As Integer, ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim affirmative As Boolean = False
        If Me.ThermalProfile Is Nothing Then
            e.RegisterCancellation("Thermo-Stream thermal profile not set")
        ElseIf Me.ThermalProfile.Count = 0 Then
            e.RegisterCancellation("Thermo-Stream thermal profile is empty")
        ElseIf setpointNumber < 1 Then
            e.RegisterCancellation("Thermo-Stream thermal profile setpoint number {0} must be positive", setpointNumber)
        ElseIf setpointNumber > Me.ThermalProfile.Count Then
            affirmative = True
        End If
        Return affirmative
    End Function
#End Region

#Region " REGISTERS "

#Region " TEMPERATURE EVENT "

    ''' <summary> Gets the Temperature event condition query command. </summary>
    ''' <value> The Temperature event condition query command. </value>
    Protected Overrides ReadOnly Property TemperatureEventConditionQueryCommand As String = "TECR?"

    ''' <summary> Gets the temperature event enable mask command format. </summary>
    ''' <value> The temperature event enable mask command format. </value>
    Protected Overrides ReadOnly Property TemperatureEventEnableMaskCommandFormat As String = "TESE {0:D}"

    ''' <summary> Gets the temperature event enable mask query command. </summary>
    ''' <value> The temperature event enable mask query command. </value>
    Protected Overrides ReadOnly Property TemperatureEventEnableMaskQueryCommand As String = "TESE?"

    ''' <summary> Gets the Temperature Event status query command. </summary>
    ''' <value> The Temperature status query command. </value>
    Protected Overrides ReadOnly Property TemperatureEventStatusQueryCommand As String = "TESR?"

    ''' <summary> Gets or sets the cached Temperature Event sentinel. </summary>
    ''' <value> <c>null</c> if Temperature Event is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Overrides Property TemperatureEventStatus As Integer?
        Get
            Return MyBase.TemperatureEventStatus
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Integer?.Equals(Me.TemperatureEventStatus, value) Then
                MyBase.TemperatureEventStatus = value
                If value.GetValueOrDefault(0) > 0 Then
                    ' update the event only if having a new value -- this way the event info gets latched.
                    Me.TemperatureEventInfo = New TemperatureEventInfo(CType(value.Value, TemperatureEvents))
                End If
            End If
        End Set
    End Property

    Private _TemperatureEventInfo As TemperatureEventInfo

    ''' <summary> Gets or sets information describing the temperature event. </summary>
    ''' <value> Information describing the temperature event. </value>
    Public Property TemperatureEventInfo As TemperatureEventInfo
        Get
            Return Me._TemperatureEventInfo
        End Get
        Protected Set(value As TemperatureEventInfo)
            If value IsNot Nothing AndAlso Me.TemperatureEventInfo.TemperatureEvent <> value.TemperatureEvent Then
                Me._TemperatureEventInfo = value
                Me.IsAtTemperature = value.IsAtTemperature
                Me.IsNotAtTemperature = value.IsNotAtTemperature
                Me.IsCycleCompleted = value.IsEndOfCycle
                Me.IsCyclesCompleted = value.IsEndOfCycles
                Me.IsCyclingStopped = value.IsCyclingStopped
                Me.IsTestTimeElapsed = value.IsTestTimeElapsed
            End If
        End Set
    End Property

    Private _IsCycleCompleted As Boolean

    ''' <summary> Gets or sets a value indicating whether one cycle completed. </summary>
    ''' <value> <c>true</c> if one cycle completed; otherwise <c>false</c> </value>
    Public Property IsCycleCompleted As Boolean
        Get
            Return Me._IsCycleCompleted
        End Get
        Set(value As Boolean)
            If value <> Me.IsCycleCompleted Then
                Me._IsCycleCompleted = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsCycleCompleted))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    Private _IsCyclesCompleted As Boolean

    ''' <summary> Gets or sets a value indicating whether all Cycles completed. </summary>
    ''' <value> <c>true</c> if all Cycles completed; otherwise <c>false</c> </value>
    Public Property IsCyclesCompleted As Boolean
        Get
            Return Me._IsCyclesCompleted
        End Get
        Set(value As Boolean)
            If value <> Me.IsCyclesCompleted Then
                Me._IsCyclesCompleted = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsCyclesCompleted))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    Private _IsTestTimeElapsed As Boolean

    ''' <summary> Gets or sets a value indicating whether test time elapsed. </summary>
    ''' <value> <c>true</c> if this cycling Stopped; otherwise <c>false</c> </value>
    Public Property IsTestTimeElapsed As Boolean
        Get
            Return Me._IsTestTimeElapsed
        End Get
        Set(value As Boolean)
            If value <> Me.IsTestTimeElapsed Then
                Me._IsTestTimeElapsed = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsTestTimeElapsed))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    Private _IsCyclingStopped As Boolean

    ''' <summary> Gets or sets a value indicating whether cycling stopped. </summary>
    ''' <value> <c>true</c> if this cycling Stopped; otherwise <c>false</c> </value>
    Public Property IsCyclingStopped As Boolean
        Get
            Return Me._IsCyclingStopped
        End Get
        Set(value As Boolean)
            If value <> Me.IsCyclingStopped Then
                Me._IsCyclingStopped = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsCyclingStopped))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    Private _IsAtTemperature As Boolean

    ''' <summary> Gets or sets a value indicating whether this object is at temperature. </summary>
    ''' <remarks> Use <see cref="IsNotAtTemperature">not at temp</see> as this is on when the
    ''' instrument turns on. </remarks>
    ''' <value> <c>true</c> if this object is at temperature; otherwise <c>false</c> </value>
    Public Property IsAtTemperature As Boolean
        Get
            Return Me._IsAtTemperature
        End Get
        Set(value As Boolean)
            If value <> Me.IsAtTemperature Then
                Me._IsAtTemperature = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsAtTemperature))
            End If
        End Set
    End Property

    Private _IsNotAtTemperature As Boolean

    ''' <summary> Gets or sets a value indicating whether this object is not at temperature. </summary>
    ''' <value> <c>true</c> if this object is not at temperature; otherwise <c>false</c> </value>
    Public Property IsNotAtTemperature As Boolean
        Get
            Return Me._IsNotAtTemperature
        End Get
        Set(value As Boolean)
            If value <> Me.IsNotAtTemperature Then
                Me._IsNotAtTemperature = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsNotAtTemperature))
            End If
        End Set
    End Property

    ''' <summary> Query if cycles stopped or terminated; Otherwise, queries if reached setpoint. </summary>
    ''' <remarks> This is the same as indicating that the start of test signal was received. </remarks>
    ''' <returns> <c>true</c> if at temperature; otherwise <c>false</c> </returns>
    Public Function QueryCyclingAtTemperature(ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim affirmative As Boolean = False
        Me.QueryTemperatureEventStatus()
        If Me.IsCycleCompleted Then
            e.RegisterCancellation("Thermo-Stream cycle completed while waiting for At-Temp")
        ElseIf ThermostreamSubsystem.UseCyclesCompleted AndAlso Me.IsCyclesCompleted Then
            e.RegisterCancellation("Thermo-Stream completed all cycles while waiting for At-Temp")
        ElseIf Me.IsCyclingStopped Then
            e.RegisterCancellation("Thermo-Stream cycle was stopped while waiting for At-Temp")
        Else
            Me.ValidateHandlerHeadDown(e)
            If e.Cancel Then
                e.RegisterCancellation("Thermo-Stream is unable to wait for At-Temp;. Details: {0}", e.Details)
            Else
                ' problem is: at temp may falsely show at temp.
                affirmative = Me.IsAtTemperature AndAlso Not Me.IsNotAtTemperature
            End If
        End If
        Return affirmative
    End Function

    ''' <summary> Query if cycles stopped or terminated; Otherwise, queries if reached setpoint. </summary>
    ''' <remarks> This is the same as indicating that the start of test signal was received. </remarks>
    ''' <returns> <c>true</c> if at temperature; otherwise <c>false</c> </returns>
    Public Function QueryAtTemperature(ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim affirmative As Boolean = False
        Me.ValidateHandlerHeadDown(e)
        If e.Cancel Then
            e.RegisterCancellation("Thermo-Stream is unable to wait for At-Temp;. Details: {0}", e.Details)
        Else
            Me.QueryTemperatureEventStatus()
            ' problem is: at temp may falsely show at temp.
            affirmative = Me.IsAtTemperature AndAlso Not Me.IsNotAtTemperature
        End If
        Return affirmative
    End Function

    ''' <summary> Queries head down. </summary>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Overloads Function QueryCyclingHeadDown(ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim affirmative As Boolean = False
        Me.QueryTemperatureEventStatus()
        If Me.IsCycleCompleted Then
            e.RegisterCancellation("Thermo-Stream cycle completed while waiting for head down")
        ElseIf ThermostreamSubsystem.UseCyclesCompleted AndAlso Me.IsCyclesCompleted Then
            e.RegisterCancellation("Thermo-Stream completed all cycles while waiting for head down")
        ElseIf Me.IsCyclingStopped Then
            e.RegisterCancellation("Thermo-Stream cycle was stopped while waiting for head down")
        Else
            affirmative = Me.QueryHeadDown(e)
        End If
        Return affirmative
    End Function

    ''' <summary> Queries head down. </summary>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Overloads Function QueryHeadDown(ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim affirmative As Boolean = False
        Me.QueryAuxiliaryEventStatus()
        If Not Me.IsReady Then
            e.RegisterCancellation("Thermo-Stream is no longer ready while waiting for head down;. Ready={0}.and.{1}",
                                CInt(AuxiliaryStatuses.ReadyStartup), CInt(Me.AuxiliaryStatusInfo.AuxiliaryStatus))
        Else
            affirmative = Not Me.IsHeadUp
        End If
        Return affirmative
    End Function

    ''' <summary> Queries head down. </summary>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Overloads Function QueryHeadUp(ByVal e As isr.Core.Pith.CancelDetailsEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim affirmative As Boolean = False
        Me.QueryAuxiliaryEventStatus()
        If Not Me.IsReady Then
            e.RegisterCancellation("Thermo-Stream is no longer ready while waiting for head up;. Ready={0}.and.{1}",
                                CInt(AuxiliaryStatuses.ReadyStartup), CInt(Me.AuxiliaryStatusInfo.AuxiliaryStatus))
        Else
            affirmative = Me.IsHeadUp
        End If
        Return affirmative
    End Function

    ''' <summary> Check if the handler is ready and cycle not completed and head is down. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Public Sub ValidateCyclingHandlerReady(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        ' check if handler is ready and head is up
        Me.ValidateHandlerHeadUp(e)
        If Not e.Cancel Then
            Me.QueryTemperatureEventStatus()
            If Not Me.IsCycleCompleted Then
                e.RegisterCancellation("Thermo-Stream seems busy--cycle not completed--stop and try again")
            ElseIf ThermostreamSubsystem.UseCyclesCompleted AndAlso Not Me.IsCyclesCompleted Then
                e.RegisterCancellation("Thermo-Stream seems busy--cycles not all completed--stop and try again")
            End If
        End If
    End Sub

    ''' <summary> Check if the handler is ready and cycle not completed and head is down. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Public Sub ValidateHandlerHeadUp(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        ' check if handler is ready
        Me.QueryAuxiliaryEventStatus()
        If Not Me.IsReady Then
            e.RegisterCancellation("Thermo-Stream is not ready--wait for startup operation to complete and try again;. Ready={0}.and.{1}",
                                CInt(AuxiliaryStatuses.ReadyStartup), CInt(Me.AuxiliaryStatusInfo.AuxiliaryStatus))
            ' check if head is up.
        ElseIf Not Me.IsHeadUp Then
            e.RegisterCancellation("Thermo-Stream head is down;. Up={0}.and.{1}",
                                CInt(AuxiliaryStatuses.HeadUpDown), CInt(Me.AuxiliaryStatusInfo.AuxiliaryStatus))
        End If
    End Sub

    ''' <summary> Check if the handler is ready and cycle not completed and head is down. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Public Sub ValidateHandlerHeadDown(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        ' check if handler is ready
        Me.QueryAuxiliaryEventStatus()
        If Not Me.IsReady Then
            e.RegisterCancellation("Thermo-Stream is not ready--wait for startup operation to complete and try again;. Ready={0}.and.{1}",
                                CInt(AuxiliaryStatuses.ReadyStartup), CInt(Me.AuxiliaryStatusInfo.AuxiliaryStatus))
        ElseIf Me.IsHeadUp Then
            e.RegisterCancellation("Thermo-Stream head is up;. Up={0}.and.{1}",
                                CInt(AuxiliaryStatuses.HeadUpDown), CInt(Me.AuxiliaryStatusInfo.AuxiliaryStatus))
        End If
    End Sub

    ''' <summary> Conditional reset operator mode. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="repeatCount"> Number of repeats. </param>
    ''' <param name="e">           Cancel event information. </param>
    Public Sub ConditionalResetOperatorMode(ByVal repeatCount As Integer, ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Me.QuerySystemScreen()
        If Not Me.OperatorScreen.GetValueOrDefault(False) Then
            Me.ResetOperatorMode(repeatCount, e)
        End If
    End Sub

    ''' <summary> Check if the handler is ready and head is up and switch to operator screen mode then wait tile mode established. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Public Sub ResetOperatorMode(ByVal repeatCount As Integer, ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Me.ValidateHandlerHeadUp(e)
        If e.Cancel Then
            e.RegisterCancellation("Unable to reset operator mode;. Details: {0}", e.Details)
        Else
            Me.ResetOperatorScreen()
            Diagnostics.Stopwatch.StartNew.Wait(Me.CommandRefractoryTimeSpan)
            Dim opMode As Boolean = False
            Do
                Me.QuerySystemScreen()
                opMode = Me.OperatorScreen.GetValueOrDefault(False)
                If Not opMode Then
                    Diagnostics.Stopwatch.StartNew.Wait(Me.CommandRefractoryTimeSpan)
                    repeatCount -= 1
                End If
            Loop Until opMode OrElse repeatCount = 0
            If Not opMode Then
                e.RegisterCancellation("Thermo-Stream seems failed switching to operator mode--stop and try again")
            End If
        End If
    End Sub

#End Region

#Region " AUXILIARY EVENT "

    ''' <summary> Gets the Auxiliary Event status query command. </summary>
    ''' <value> The Auxiliary status query command. </value>
    Protected Overrides ReadOnly Property AuxiliaryEventStatusQueryCommand As String = "AUXC?"

    ''' <summary> Gets or sets the cached Auxiliary Event sentinel. </summary>
    ''' <value> <c>null</c> if Auxiliary Event is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Overrides Property AuxiliaryEventStatus As Integer?
        Get
            Return MyBase.AuxiliaryEventStatus
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Integer?.Equals(Me.AuxiliaryEventStatus, value) Then
                MyBase.AuxiliaryEventStatus = value
                Me.AuxiliaryStatusInfo = New AuxiliaryStatusInfo(CType(value.Value, AuxiliaryStatuses))
                If value.GetValueOrDefault(0) > 0 Then
                    ' update the event only if having a new value -- this way the event info gets latched.
                End If
            End If
        End Set
    End Property

    Private _AuxiliaryStatusInfo As AuxiliaryStatusInfo

    ''' <summary> Gets or sets information describing the Auxiliary event. </summary>
    ''' <value> Information describing the Auxiliary event. </value>
    Public Property AuxiliaryStatusInfo As AuxiliaryStatusInfo
        Get
            Return Me._AuxiliaryStatusInfo
        End Get
        Protected Set(value As AuxiliaryStatusInfo)
            If value IsNot Nothing AndAlso
                    Me.AuxiliaryStatusInfo.AuxiliaryStatus <> value.AuxiliaryStatus Then
                Me._AuxiliaryStatusInfo = value
                Me.IsDutControl = value.IsDutControl
                Me.IsFlowOn = value.IsFlowOn
                Me.IsHeadUp = value.IsHeadUp
                Me.IsCyclingStopped = value.IsHeatOnlyMode
                Me.IsManualMode = value.IsManualMode
                Me.IsRampMode = value.IsRampMode
                Me.IsReady = value.IsReady
            End If
        End Set
    End Property

    Private _IsRampMode As Boolean

    ''' <summary> Gets or sets a value indicating whether the instrument is in ramp mode. </summary>
    ''' <value> <c>true</c> if in ramp mode; otherwise <c>false</c> </value>
    Public Property IsRampMode As Boolean
        Get
            Return Me._IsRampMode
        End Get
        Set(value As Boolean)
            If value <> Me.IsRampMode Then
                Me._IsRampMode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsRampMode))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    Private _IsReady As Boolean

    ''' <summary> Gets or sets a value indicating whether the instrument is ready for operation or in startup. </summary>
    ''' <value> <c>true</c> if instrument is ready for operation or in startup; otherwise <c>false</c> </value>
    Public Property IsReady As Boolean
        Get
            Return Me._IsReady
        End Get
        Set(value As Boolean)
            If value <> Me.IsReady Then
                Me._IsReady = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsReady))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property


    Private _IsHeadUp As Boolean

    ''' <summary> Gets or sets a value indicating whether head is up. </summary>
    ''' <value> <c>true</c> if head is up; otherwise <c>false</c> </value>
    Public Property IsHeadUp As Boolean
        Get
            Return Me._IsHeadUp
        End Get
        Set(value As Boolean)
            If value <> Me.IsHeadUp Then
                Me._IsHeadUp = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsHeadUp))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    Private _IsManualMode As Boolean

    ''' <summary> Gets or sets a value indicating whether manual mode is on. </summary>
    ''' <value> <c>true</c> if manual mode is on; otherwise <c>false</c> </value>
    Public Property IsManualMode As Boolean
        Get
            Return Me._IsManualMode
        End Get
        Set(value As Boolean)
            If value <> Me.IsManualMode Then
                Me._IsManualMode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsManualMode))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    Private _IsHeatOnlyMode As Boolean

    ''' <summary> Gets or sets a value indicating whether heat-only mode. </summary>
    ''' <value> <c>true</c> if this heat-only mode; otherwise <c>false</c> </value>
    Public Property IsHeatOnlyMode As Boolean
        Get
            Return Me._IsHeatOnlyMode
        End Get
        Set(value As Boolean)
            If value <> Me.IsHeatOnlyMode Then
                Me._IsHeatOnlyMode = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsHeatOnlyMode))
                Windows.Forms.Application.DoEvents()
            End If
        End Set
    End Property

    Private _IsDutControl As Boolean

    ''' <summary> Gets or sets a value indicating whether dut control is used. </summary>
    ''' <value> <c>true</c> if dut control is used; otherwise <c>false</c> </value>
    Public Property IsDutControl As Boolean
        Get
            Return Me._IsDutControl
        End Get
        Set(value As Boolean)
            If value <> Me.IsDutControl Then
                Me._IsDutControl = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsDutControl))
            End If
        End Set
    End Property

    Private _IsFlowOn As Boolean

    ''' <summary> Gets or sets a value indicating whether flow is on. </summary>
    ''' <value> <c>true</c> if flow is on; otherwise <c>false</c> </value>
    Public Property IsFlowOn As Boolean
        Get
            Return Me._IsFlowOn
        End Get
        Set(value As Boolean)
            If value <> Me.IsFlowOn Then
                Me._IsFlowOn = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsFlowOn))
            End If
        End Set
    End Property

#End Region

#End Region

End Class
