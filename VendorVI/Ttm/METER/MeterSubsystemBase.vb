Imports isr.Core.Pith.NumericExtensions
Imports isr.Core.Pith.StackTraceExtensions
Imports isr.Core.Pith.StopwatchExtensions
''' <summary> Defines the contract that must be implemented by Thermal Transient Meter Subsystems. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="12/12/2013" by="David" revision="3.0.5093"> Created. </history>
Public MustInherit Class MeterSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SubsystemPlusStatusBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId:="0")>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Clearing {Me.EntityName};. ")
        MyBase.ClearExecutionState()
        Me.LastReading = ""
        Me.LastOutcome = ""
        Me.LastMeasurementStatus = ""
        Me.MeasurementEventCondition = 0
        Me.MeasurementAvailable = False
        ' this is not required -- is done internally 
        ' Me.Session.WriteLine("{0}:clear()", Me.EntityName)
        Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    ''' <remarks> Reads instrument defaults, applies the values to the instrument and reads them back as 
    '''           configuration values. This ensures that the instrument state is mirrored in code. </remarks>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"Resetting {Me.EntityName};. ")
        System.Windows.Forms.Application.DoEvents()
        Me.ReadInstrumentDefaults()
        Me.ApplyInstrumentDefaults()
        ' this causes a problem requiring the make an initial resistance measurement before 
        ' the thermal transient.
        ' Me.Session.WriteLine("{0}:init()", Me.EntityName)
        Me.QueryConfiguration()
        Windows.Forms.Application.DoEvents()
    End Sub

#End Region

#Region " DEVICE UNDER TEST RESISTANCE "

    ''' <summary> Gets the <see cref="ResistanceMeasureBase">part resistance element</see>. </summary>
    ''' <value> The cold resistance. </value>
    MustOverride ReadOnly Property Resistance As ResistanceMeasureBase

#End Region

#Region " ENTITY "

    Public Const GlobalName As String = "_G"

    ''' <summary> Name of the thermal transient base entity. </summary>
    Public Const ThermalTransientBaseEntityName As String = "_G.ttm"

    ''' <summary> Name of the final resistance entity. </summary>
    Public Const FinalResistanceEntityName As String = "_G.ttm.fr"

    ''' <summary> Name of the initial resistance entity. </summary>
    Public Const InitialResistanceEntityName As String = "_G.ttm.ir"

    ''' <summary> Name of the thermal transient entity. </summary>
    Public Const ThermalTransientEntityName As String = "_G.ttm.tr"

    ''' <summary> Name of the thermal transient estimator entity. </summary>
    Public Const ThermalTransientEstimatorEntityName As String = "_G.ttm.est"

    Private _MeterEntity As ThermalTransientMeterEntity

    ''' <summary> Gets or sets the meter entity. </summary>
    ''' <value> The meter entity. </value>
    Public Property MeterEntity As ThermalTransientMeterEntity
        Get
            Return Me._MeterEntity
        End Get
        Set(ByVal value As ThermalTransientMeterEntity)
            If Not value.Equals(Me.MeterEntity) Then
                Me._MeterEntity = value
                Me.SafePostPropertyChanged()
                Select Case value
                    Case ThermalTransientMeterEntity.FinalResistance
                        Me.EntityName = MeterSubsystemBase.FinalResistanceEntityName
                        Me.BaseEntityName = MeterSubsystemBase.ThermalTransientBaseEntityName
                        Me.DefaultsName = "_G.ttm.coldResistance.Defaults"
                    Case ThermalTransientMeterEntity.InitialResistance
                        Me.EntityName = MeterSubsystemBase.InitialResistanceEntityName
                        Me.BaseEntityName = MeterSubsystemBase.ThermalTransientBaseEntityName
                        Me.DefaultsName = "_G.ttm.coldResistance.Defaults"
                    Case ThermalTransientMeterEntity.Transient
                        Me.EntityName = MeterSubsystemBase.ThermalTransientEntityName
                        Me.BaseEntityName = MeterSubsystemBase.ThermalTransientBaseEntityName
                        Me.DefaultsName = "_G.ttm.trace.Defaults"
                    Case ThermalTransientMeterEntity.Estimator
                        Me.EntityName = MeterSubsystemBase.ThermalTransientEstimatorEntityName
                        Me.BaseEntityName = MeterSubsystemBase.ThermalTransientBaseEntityName
                        Me.DefaultsName = "_G.ttm.estimator.Defaults"
                    Case ThermalTransientMeterEntity.Shunt
                        Me.EntityName = MeterSubsystemBase.GlobalName
                        Me.BaseEntityName = MeterSubsystemBase.GlobalName
                        Me.DefaultsName = ""
                    Case ThermalTransientMeterEntity.None
                        Me.EntityName = MeterSubsystemBase.ThermalTransientBaseEntityName
                        Me.BaseEntityName = MeterSubsystemBase.GlobalName
                        Me.DefaultsName = ""
                    Case Else
                        Debug.Assert(Not Debugger.IsAttached, "Unhandled case")
                End Select
            End If
        End Set
    End Property

    Private _DefaultsName As String

    ''' <summary> Gets or sets the Defaults name. </summary>
    ''' <returns> The Source Measure Unit. </returns>
    Public Property DefaultsName As String
        Get
            Return Me._DefaultsName
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.DefaultsName) Then
                Me._DefaultsName = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _EntityName As String

    ''' <summary> Gets or sets the entity name. </summary>
    ''' <returns> The Source Measure Unit. </returns>
    Public Property EntityName As String
        Get
            Return Me._EntityName
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.EntityName) Then
                Me._EntityName = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _BaseEntityName As String
    ''' <summary> Gets or sets the Base Entity name. </summary>
    ''' <returns> The Meter entity. </returns>
    Public Property BaseEntityName As String
        Get
            Return Me._BaseEntityName
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.BaseEntityName) Then
                Me._BaseEntityName = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " SOURCE MEASURE UNIT "

    ''' <summary> Queries if a given source measure unit exists. </summary>
    ''' <param name="sourceMeasureUnitName"> Name of the source measure unit, e.g., 'smua' or 'smub'. </param>
    ''' <returns> <c>True</c> if the source measure unit exists; otherwise <c>False</c> </returns>
    Public Function SourceMeasureUnitExists(ByVal sourceMeasureUnitName As String) As Boolean
        Me.Session.MakeTrueFalseReplyIfEmpty(String.Equals(sourceMeasureUnitName, "smua", StringComparison.OrdinalIgnoreCase))
        Return Not Me.Session.IsNil(String.Format("_G.localnode.{0}", sourceMeasureUnitName))
    End Function

    ''' <summary> Requires source measure unit. </summary>
    ''' <returns> <c>True</c> if this entity requires the assignment of the source measure unit. </returns>
    Public Function RequiresSourceMeasureUnit() As Boolean
        Return Me.MeterEntity = ThermalTransientMeterEntity.FinalResistance OrElse
            Me.MeterEntity = ThermalTransientMeterEntity.InitialResistance OrElse
            Me.MeterEntity = ThermalTransientMeterEntity.Transient
    End Function

    Private _SourceMeasureUnit As String

    ''' <summary> Gets or sets the cached Source Measure Unit. </summary>
    ''' <returns> The Source Measure Unit. </returns>
    Public Overridable Property SourceMeasureUnit As String
        Get
            Return Me._SourceMeasureUnit
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.SourceMeasureUnit) Then
                Me._SourceMeasureUnit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries the Source Measure Unit. Also sets the <see cref="SourceMeasureUnit">Source
    ''' Measure Unit</see>. </summary>
    ''' <exception cref="OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <returns> The Source Measure Unit. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="smuI")>
    Public Function QuerySourceMeasureUnit() As String
        If Me.RequiresSourceMeasureUnit Then
            Me.Session.MakeEmulatedReplyIfEmpty(True)
            If Me.Session.IsStatementTrue("{0}.smuI==_G.smua", Me.EntityName) Then
                Me.SourceMeasureUnit = "smua"
            ElseIf Me.Session.IsStatementTrue("{0}.smuI==_G.smub", Me.EntityName) Then
                Me.SourceMeasureUnit = "smub"
            Else
                Me.SourceMeasureUnit = "unknown"
                Throw New OperationFailedException($"Failed reading Source Measure Unit;. {Me.EntityName}.smuI is neither smua or smub.")
            End If
        End If
        Return Me.SourceMeasureUnit
    End Function

    ''' <summary> Programs the Source Measure Unit. Does not read back from the instrument. </summary>
    ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <param name="value"> the Source Measure Unit, e.g., 'smua' or 'smub'. </param>
    ''' <returns> The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    Public Function WriteSourceMeasureUnit(ByVal value As String) As String
        If Me.RequiresSourceMeasureUnit Then
            If Me.SourceMeasureUnitExists(value) Then
                Me.Session.WriteLine("{0}:currentSourceChannelSetter('{1}')", Me.EntityName, value)
            Else
                Throw New ArgumentException("This source measure unit name is invalid.", "value")
            End If
        End If
        Me.SourceMeasureUnit = value
        Return Me.SourceMeasureUnit
    End Function

    ''' <summary> Writes and reads back the Source Measure Unit. </summary>
    ''' <param name="value"> the Source Measure Unit, e.g., 'smua' or 'smub'. </param>
    ''' <returns> The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    Public Function ApplySourceMeasureUnit(ByVal value As String) As String
        Me.WriteSourceMeasureUnit(value)
        Return Me.QuerySourceMeasureUnit()
    End Function

#End Region

#Region " APERTURE "

    ''' <summary> The Aperture. </summary>
    Private _Aperture As Double?

    ''' <summary> Gets or sets the cached Source Aperture. </summary>
    ''' <value> The Source Aperture. </value>
    Public Overridable Property Aperture As Double?
        Get
            Return Me._Aperture
        End Get
        Protected Set(ByVal value As Double?)
            If value.HasValue Then
                Me.Resistance.Aperture = value.Value
            Else
                Me.Resistance.Aperture = 0
            End If
            If Me.Aperture.Differs(value, 0.000001) Then
                Me._Aperture = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Aperture. </summary>
    ''' <remarks> This command set the immediate output Aperture. The value is in Amperes. The
    ''' immediate Aperture is the output current setting. At *RST, the current values = 0. </remarks>
    ''' <param name="value"> The Aperture. </param>
    ''' <returns> The Source Aperture. </returns>
    Public Function ApplyAperture(ByVal value As Double) As Double?
        Me.WriteAperture(value)
        Return Me.QueryAperture
    End Function

    ''' <summary> Queries the Aperture. </summary>
    ''' <remarks> David, 1/6/2016. </remarks>
    ''' <returns> The Aperture or none if unknown. </returns>
    Public Function QueryAperture() As Double?
        Const printFormat As Decimal = 7.4D
        Me.Aperture = Me.Session.QueryPrint(Me.Aperture.GetValueOrDefault(1 / 60), printFormat, "{0}.aperture", Me.EntityName)
        Return Me.Aperture
    End Function

    ''' <summary> Writes the source Aperture without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output Aperture. The value is in Amperes. The
    ''' immediate Aperture is the output current setting. At *RST, the current values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Aperture. </param>
    ''' <returns> The Source Aperture. </returns>
    Public Overridable Function WriteAperture(ByVal value As Double) As Double?
        Dim details As String = ""
        If Me.MeterEntity = ThermalTransientMeterEntity.Transient Then
            If Not ThermalTransient.ValidateAperture(value, details) Then
                Throw New ArgumentOutOfRangeException("value", details)
            End If
        Else
            If Not ColdResistance.ValidateAperture(value, details) Then
                Throw New ArgumentOutOfRangeException("value", details)
            End If
        End If
        Me.Session.WriteLine("{0}:apertureSetter({1})", Me.EntityName, value)
        Me.Aperture = value
        Return Me.Aperture
    End Function

#End Region

#Region " CURRENT LEVEL "

    ''' <summary> The Current Level. </summary>
    Private _CurrentLevel As Double?

    ''' <summary> Gets or sets the cached Source Current Level. </summary>
    ''' <value> The Source Current Level. </value>
    Public Overridable Property CurrentLevel As Double?
        Get
            Return Me._CurrentLevel
        End Get
        Protected Set(ByVal value As Double?)
            If value.HasValue Then
                Me.Resistance.CurrentLevel = value.Value
            Else
                Me.Resistance.CurrentLevel = 0
            End If
            If Me.CurrentLevel.Differs(value, 0.000000001) Then
                Me._CurrentLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Current Level. </summary>
    ''' <remarks> This command set the immediate output Current Level. The value is in Amperes. The
    ''' immediate Current Level is the output current setting. At *RST, the current values = 0. </remarks>
    ''' <param name="value"> The Current Level. </param>
    ''' <returns> The Source Current Level. </returns>
    Public Function ApplyCurrentLevel(ByVal value As Double) As Double?
        Me.WriteCurrentLevel(value)
        Return Me.QueryCurrentLevel
    End Function

    ''' <summary> Queries the Current Level. </summary>
    ''' <returns> The Current Level or none if unknown. </returns>
    ''' <exception cref="DeviceException"> Thrown when a device error condition occurs. </exception>
    Public Function QueryCurrentLevel() As Double?
        Const printFormat As Decimal = 9.6D
        Me.CurrentLevel = Me.Session.QueryPrint(Me.CurrentLevel.GetValueOrDefault(0.27), printFormat, "{0}.level", Me.EntityName)
        Return Me.CurrentLevel
    End Function

    ''' <summary> Writes the source Current Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output Current Level. The value is in Amperes. The
    ''' immediate CurrentLevel is the output current setting. At *RST, the current values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Current Level. </param>
    ''' <returns> The Source Current Level. </returns>
    Public Function WriteCurrentLevel(ByVal value As Double) As Double?
        Dim details As String = ""
        If Me.MeterEntity = ThermalTransientMeterEntity.Transient Then
            If Not ThermalTransient.ValidateCurrentLevel(value, details) Then
                Throw New ArgumentOutOfRangeException("value", details)
            End If
        Else
            If Not ColdResistance.ValidateCurrentLevel(value, details) Then
                Throw New ArgumentOutOfRangeException("value", details)
            End If
        End If
        Me.Session.WriteLine("{0}:levelSetter({1})", Me.EntityName, value)
        Me.CurrentLevel = value
        Return Me.CurrentLevel
    End Function

#End Region

#Region " VOLTAGE LIMIT "

    ''' <summary> The Voltage Limit. </summary>
    Private _VoltageLimit As Double?

    ''' <summary> Gets or sets the cached Source Voltage Limit. </summary>
    ''' <value> The Source Voltage Limit. </value>
    Public Overridable Property VoltageLimit As Double?
        Get
            Return Me._VoltageLimit
        End Get
        Protected Set(ByVal value As Double?)
            If value.HasValue Then
                Me.Resistance.VoltageLimit = value.Value
            Else
                Me.Resistance.VoltageLimit = 0
            End If
            If Me.VoltageLimit.Differs(value, 0.000001) Then
                Me._VoltageLimit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Voltage Limit. </summary>
    ''' <remarks> This command set the immediate output Voltage Limit. The value is in Volts. The
    ''' immediate Voltage Limit is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Voltage Limit. </param>
    ''' <returns> The Source Voltage Limit. </returns>
    Public Function ApplyVoltageLimit(ByVal value As Double) As Double?
        Me.WriteVoltageLimit(value)
        Return Me.QueryVoltageLimit
    End Function

    ''' <summary> Queries the Voltage Limit. </summary>
    ''' <returns> The Voltage Limit or none if unknown. </returns>
    ''' <exception cref="DeviceException"> Thrown when a device error condition occurs. </exception>
    Public Function QueryVoltageLimit() As Double?
        Const printFormat As Decimal = 9.6D
        Me.VoltageLimit = Me.Session.QueryPrint(Me.VoltageLimit.GetValueOrDefault(0.1), printFormat, "{0}.limit", Me.EntityName)
        Return Me.VoltageLimit
    End Function

    ''' <summary> Writes the source Voltage Limit without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output Voltage Limit. The value is in Volts. The
    ''' immediate VoltageLimit is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Voltage Limit. </param>
    ''' <returns> The Source Voltage Limit. </returns>
    Public Function WriteVoltageLimit(ByVal value As Double) As Double?
        Dim details As String = ""
        If Me.MeterEntity = ThermalTransientMeterEntity.Transient Then
            If Not ThermalTransient.ValidateVoltageLimit(value, details) Then
                Throw New ArgumentOutOfRangeException("value", details)
            End If
        Else
            If Not ColdResistance.ValidateVoltageLimit(value, details) Then
                Throw New ArgumentOutOfRangeException("value", details)
            End If
        End If
        Me.Session.WriteLine("{0}:limitSetter({1})", Me.EntityName, value)
        Me.VoltageLimit = value
        Return Me.VoltageLimit
    End Function

#End Region

#Region " HIGH LIMIT "

    ''' <summary> The High Limit. </summary>
    Private _HighLimit As Double?

    ''' <summary> Gets or sets the cached High Limit. </summary>
    ''' <value> The High Limit. </value>
    Public Overridable Property HighLimit As Double?
        Get
            Return Me._HighLimit
        End Get
        Protected Set(ByVal value As Double?)
            If value.HasValue Then
                Me.Resistance.HighLimit = value.Value
            Else
                Me.Resistance.HighLimit = 0
            End If
            If Me.HighLimit.Differs(value, 0.000001) Then
                Me._HighLimit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the High Limit. </summary>
    ''' <remarks> This command set the immediate output High Limit. The value is in Ohms. The
    ''' immediate High Limit is the output High setting. At *RST, the High values = 0. </remarks>
    ''' <param name="value"> The High Limit. </param>
    ''' <returns> The High Limit. </returns>
    Public Function ApplyHighLimit(ByVal value As Double) As Double?
        Me.WriteHighLimit(value)
        Return Me.QueryHighLimit
    End Function

    ''' <summary> Queries the High Limit. </summary>
    ''' <returns> The High Limit or none if unknown. </returns>
    ''' <exception cref="DeviceException"> Thrown when a device error condition occurs. </exception>
    Public Function QueryHighLimit() As Double?
        Const printFormat As Decimal = 9.6D
        Me.HighLimit = Me.Session.QueryPrint(Me.HighLimit.GetValueOrDefault(0.1), printFormat, "{0}.highLimit", Me.EntityName)
        Return Me.HighLimit
    End Function

    ''' <summary> Writes the High Limit without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output High Limit. The value is in Ohms. The
    ''' immediate HighLimit is the output High setting. At *RST, the High values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The High Limit. </param>
    ''' <returns> The High Limit. </returns>
    Public Function WriteHighLimit(ByVal value As Double) As Double?
        Dim details As String = ""
        If Me.MeterEntity = ThermalTransientMeterEntity.Transient Then
            If Not ThermalTransient.ValidateLimit(value, details) Then
                Throw New ArgumentOutOfRangeException("value", details)
            End If
        Else
            If Not ColdResistance.ValidateLimit(value, details) Then
                Throw New ArgumentOutOfRangeException("value", details)
            End If
        End If
        Me.Session.WriteLine("{0}:highLimitSetter({1})", Me.EntityName, value)
        Me.HighLimit = value
        Return Me.HighLimit
    End Function

#End Region

#Region " LOW LIMIT "

    ''' <summary> The Low Limit. </summary>
    Private _LowLimit As Double?

    ''' <summary> Gets or sets the cached Low Limit. </summary>
    ''' <value> The Low Limit. </value>
    Public Overridable Property LowLimit As Double?
        Get
            Return Me._LowLimit
        End Get
        Protected Set(ByVal value As Double?)
            If value.HasValue Then
                Me.Resistance.LowLimit = value.Value
            Else
                Me.Resistance.LowLimit = 0
            End If
            If Me.LowLimit.Differs(value, 0.000001) Then
                Me._LowLimit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Low Limit. </summary>
    ''' <remarks> This command set the immediate output Low Limit. The value is in Ohms. The
    ''' immediate Low Limit is the output Low setting. At *RST, the Low values = 0. </remarks>
    ''' <param name="value"> The Low Limit. </param>
    ''' <returns> The Low Limit. </returns>
    Public Function ApplyLowLimit(ByVal value As Double) As Double?
        Me.WriteLowLimit(value)
        Return Me.QueryLowLimit
    End Function

    ''' <summary> Queries the Low Limit. </summary>
    ''' <returns> The Low Limit or none if unknown. </returns>
    Public Function QueryLowLimit() As Double?
        Const printFormat As Decimal = 9.6D
        Me.LowLimit = Me.Session.QueryPrint(Me.LowLimit.GetValueOrDefault(0.01), printFormat, "{0}.lowLimit", Me.EntityName)
        Return Me.LowLimit
    End Function

    ''' <summary> Writes the Low Limit without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output Low Limit. The value is in Ohms. The immediate
    ''' LowLimit is the output Low setting. At *RST, the Low values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Low Limit. </param>
    ''' <returns> The Low Limit. </returns>
    Public Function WriteLowLimit(ByVal value As Double) As Double?
        Dim details As String = ""
        If Me.MeterEntity = ThermalTransientMeterEntity.Transient Then
            If Not ThermalTransient.ValidateLimit(value, details) Then
                Throw New ArgumentOutOfRangeException("value", details)
            End If
        Else
            If Not ColdResistance.ValidateLimit(value, details) Then
                Throw New ArgumentOutOfRangeException("value", details)
            End If
        End If
        Me.Session.WriteLine("{0}:lowLimitSetter({1})", Me.EntityName, value)
        Me.LowLimit = value
        Return Me.LowLimit
    End Function

#End Region

#Region " CONFIGURE "

    ''' <summary> Applies the instrument defaults. </summary>
    ''' <remarks> This is required until the reset command gets implemented. </remarks>
    Public Overridable Sub ApplyInstrumentDefaults()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Applying {0} defaults;. ", Me.EntityName)
        Windows.Forms.Application.DoEvents()
        Me.StatusSubsystem.Write("{0}:currentSourceChannelSetter({1}.smuI)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.StatusSubsystem.Write("{0}:apertureSetter({1}.aperture)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.StatusSubsystem.Write("{0}:levelSetter({1}.level)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.StatusSubsystem.Write("{0}:limitSetter({1}.limit)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.StatusSubsystem.Write("{0}:lowLimitSetter({1}.lowLimit)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.StatusSubsystem.Write("{0}:highLimitSetter({1}.highLimit)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
    End Sub

    ''' <summary> Reads instrument defaults. </summary>
    Public Overridable Sub ReadInstrumentDefaults()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Reading {0} defaults;. ", Me.EntityName)
        Windows.Forms.Application.DoEvents()
        With My.MySettings.Default
            If Me.RequiresSourceMeasureUnit Then
                .SourceMeasureUnitDefault = Me.Session.QueryPrintTrimEnd("{0}.smuI", Me.DefaultsName)
                If String.IsNullOrWhiteSpace(.SourceMeasureUnitDefault) Then
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default source measure unit name;. Sent:'{0}; Received:'{1}'.",
                                               Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
                End If
            End If
        End With
        Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary> Applies changed meter configuration. </summary>
    ''' <param name="resistance"> The resistance. </param>
    Public Overridable Sub ConfigureChanged(ByVal resistance As ResistanceMeasureBase)
        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))

        If Not resistance.ConfigurationEquals(Me.Resistance) Then

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Configuring {0} resistance measurement;. ", Me.EntityName)
            Windows.Forms.Application.DoEvents()

            If Not String.Equals(Me.Resistance.SourceMeasureUnit, resistance.SourceMeasureUnit) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} source measure unit to {1};. ", Me.EntityName, resistance.SourceMeasureUnit)
                Me.ApplySourceMeasureUnit(resistance.SourceMeasureUnit)
            End If

            If Not Me.Resistance.Aperture.Equals(resistance.Aperture) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} aperture to {1};. ", Me.EntityName, resistance.Aperture)
                Me.ApplyAperture(resistance.Aperture)
            End If

            If Not Me.Resistance.CurrentLevel.Equals(resistance.CurrentLevel) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} Current Level to {1};. ", Me.EntityName, resistance.CurrentLevel)
                Me.ApplyCurrentLevel(resistance.CurrentLevel)
            End If

            If Not Me.Resistance.HighLimit.Equals(resistance.HighLimit) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} high limit to {1};. ", Me.EntityName, resistance.HighLimit)
                Me.ApplyHighLimit(resistance.HighLimit)
            End If

            If Not Me.Resistance.LowLimit.Equals(resistance.LowLimit) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} Low limit to {1};. ", Me.EntityName, resistance.LowLimit)
                Me.ApplyLowLimit(resistance.LowLimit)
            End If

            If Not Me.Resistance.VoltageLimit.Equals(resistance.VoltageLimit) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} Voltage Limit to {1};. ", Me.EntityName, resistance.VoltageLimit)
                Me.ApplyVoltageLimit(resistance.VoltageLimit)
            End If
            Windows.Forms.Application.DoEvents()

        End If

    End Sub

    ''' <summary> Configures the meter for making the resistance measurement. </summary>
    ''' <param name="resistance"> The resistance. </param>
    Public Overridable Sub Configure(ByVal resistance As ResistanceMeasureBase)
        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Configuring {0} resistance measurement;. ", Me.EntityName)
        Windows.Forms.Application.DoEvents()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} source measure unit to {1};. ", Me.EntityName, resistance.SourceMeasureUnit)
        Me.ApplySourceMeasureUnit(resistance.SourceMeasureUnit)
        Windows.Forms.Application.DoEvents()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} aperture to {1};. ", Me.EntityName, resistance.Aperture)
        Me.ApplyAperture(resistance.Aperture)
        Windows.Forms.Application.DoEvents()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} Current Level to {1};. ", Me.EntityName, resistance.CurrentLevel)
        Me.ApplyCurrentLevel(resistance.CurrentLevel)
        Windows.Forms.Application.DoEvents()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} high limit to {1};. ", Me.EntityName, resistance.HighLimit)
        Me.ApplyHighLimit(resistance.HighLimit)
        Windows.Forms.Application.DoEvents()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} Low limit to {1};. ", Me.EntityName, resistance.LowLimit)
        Me.ApplyLowLimit(resistance.LowLimit)
        Windows.Forms.Application.DoEvents()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} Voltage Limit to {1};. ", Me.EntityName, resistance.VoltageLimit)
        Me.ApplyVoltageLimit(resistance.VoltageLimit)
        Windows.Forms.Application.DoEvents()

    End Sub

    ''' <summary> Queries the configuration. </summary>
    Public Overridable Sub QueryConfiguration()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Reading {0} meter configuration;. ", Me.EntityName)
        Windows.Forms.Application.DoEvents()
        Me.QuerySourceMeasureUnit()
        Me.QueryAperture()
        Me.QueryCurrentLevel()
        Me.QueryHighLimit()
        Me.QueryLowLimit()
        Me.QueryVoltageLimit()
    End Sub

#End Region

#Region " MEASURE "

    ''' <summary> Measures the Final resistance and returns. </summary>
    ''' <exception cref="DeviceException"> Thrown when a device error condition occurs. </exception>
    Public Sub Measure()
        Me.ClearExecutionState()
        Me.Session.EnableWaitComplete()
        Select Case Me.MeterEntity
            Case ThermalTransientMeterEntity.FinalResistance
                Me.Session.WriteLine("_G.ttm.measureFinalResistance() waitcomplete()")
            Case ThermalTransientMeterEntity.InitialResistance
                Me.Session.WriteLine("_G.ttm.measureInitialResistance() waitcomplete()")
            Case ThermalTransientMeterEntity.Transient
                Me.Session.WriteLine("_G.ttm.measureThermalTransient() waitcomplete()")
            Case Else
        End Select
        Select Case Me.MeterEntity
            Case ThermalTransientMeterEntity.FinalResistance,
                ThermalTransientMeterEntity.InitialResistance,
                ThermalTransientMeterEntity.Transient
                Me.StatusSubsystem.QueryOperationCompleted()
                Me.StatusSubsystem.CheckThrowDeviceException(False, "Measuring Final Resistance;. ")
            Case Else
        End Select
    End Sub

    ''' <summary> Measures the Thermal Transient. </summary>
    ''' <param name="resistance"> The Thermal Transient element. </param>
    Public Sub Measure(ByVal resistance As ResistanceMeasureBase)
        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))
        resistance.ClearExecutionState()
        Me.Measure()
        Me.StatusSubsystem.CheckThrowDeviceException(False, "Measuring Thermal Transient;. last command: '{0}'", Me.Session.LastMessageSent)
    End Sub

#End Region

#Region " OKAY "

    ''' <summary> Queries the meter okay sentinel. </summary>
    ''' <returns> <c>True</c> if entity okay status is True; otherwise, <c>False</c>. </returns>
    Public Function QueryOkay() As Boolean
        Me.Session.MakeEmulatedReplyIfEmpty(True)
        Return Me.Session.IsStatementTrue("{0}:isOkay()", Me.EntityName)
    End Function

#End Region

#Region " OUTCOME "

    ''' <summary> Queries the meter outcome status. If nil, measurement was not made. </summary>
    ''' <returns> <c>True</c> if entity outcome is nil; otherwise, <c>False</c>. </returns>
    Public Function QueryOutcomeNil() As Boolean
        Me.Session.MakeEmulatedReplyIfEmpty(True)
        Return Me.Session.IsStatementTrue("{0}.outcome==nil", Me.EntityName)
    End Function

#End Region

#Region " READ "

    Private _LastReading As String

    ''' <summary> Gets or sets (protected) the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property LastReading() As String
        Get
            Return Me._lastReading
        End Get
        Protected Set(ByVal value As String)
            Me._lastReading = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    Private _LastOutcome As String

    ''' <summary> Gets or sets (protected) the last outcome. </summary>
    ''' <value> The last outcome. </value>
    Public Property LastOutcome() As String
        Get
            Return Me._lastOutcome
        End Get
        Protected Set(ByVal value As String)
            Me._lastOutcome = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    Private _LastMeasurementStatus As String
    ''' <summary> Gets or sets (protected) the last measurement status. </summary>
    Public Property LastMeasurementStatus() As String
        Get
            Return Me._lastMeasurementStatus
        End Get
        Protected Set(ByVal value As String)
            Me._lastMeasurementStatus = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    Private _MeasurementAvailable As Boolean

    ''' <summary> Gets or sets (protected) the measurement available. </summary>
    ''' <value> The measurement available. </value>
    Public Property MeasurementAvailable As Boolean
        Get
            Return Me._MeasurementAvailable
        End Get
        Set(ByVal value As Boolean)
            Me._MeasurementAvailable = value
            Me.SafeSendPropertyChanged()
        End Set
    End Property


    ''' <summary> The measurement event Condition. </summary>
    Private _MeasurementEventCondition As Integer

    ''' <summary> Gets or sets the cached Condition of the measurement register events. </summary>
    ''' <value> <c>null</c> if value is not known;. </value>
    Public Property MeasurementEventCondition() As Integer
        Get
            Return Me._MeasurementEventCondition
        End Get
        Protected Set(ByVal value As Integer)
            If Not Me.MeasurementEventCondition.Equals(value) Then
                Me._MeasurementEventCondition = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Reads the condition of the measurement register event. </summary>
    ''' <returns> System.Nullable{System.Int32}. </returns>
    Public Function TryQueryMeasurementEventCondition() As Boolean
        Return Me.Session.TryQueryPrint(1, Me.MeasurementEventCondition, "_G.status.measurement.condition")
    End Function

    ''' <summary> Parses the outcome. </summary>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> The parsed outcome. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function ParseOutcome(ByRef details As String) As MeasurementOutcomes

        Dim outcome As MeasurementOutcomes = MeasurementOutcomes.None
        Dim detailsBuilder As New System.Text.StringBuilder
        Dim measurementCondition As Integer

        Dim numericValue As Integer

        If String.IsNullOrWhiteSpace(Me.LastOutcome) Then
            outcome = outcome Or MeasurementOutcomes.MeasurementNotMade
            detailsBuilder.AppendFormat("Failed parsing outcome -- outcome is nothing indicating measurement not made;. ")
        Else
            If Integer.TryParse(Me.LastOutcome, Globalization.NumberStyles.Number, Globalization.CultureInfo.InvariantCulture, numericValue) Then
                If numericValue < 0 Then
                    If detailsBuilder.Length > 0 Then detailsBuilder.AppendLine()
                    detailsBuilder.AppendFormat("Failed parsing outcome '{0}';. ", Me.LastOutcome)
                    outcome = MeasurementOutcomes.MeasurementFailed
                    outcome = outcome Or MeasurementOutcomes.UnexpectedOutcomeFormat
                Else
                    ' interpret the outcome.
                    If numericValue <> 0 Then
                        outcome = MeasurementOutcomes.MeasurementFailed

                        If (numericValue And 1) = 1 Then
                            ' this is a bad status - could be compliance or other things.
                            If Not String.IsNullOrWhiteSpace(Me.LastMeasurementStatus) Then
                                If detailsBuilder.Length > 0 Then detailsBuilder.AppendLine()
                                detailsBuilder.AppendFormat("Outcome status={0};. ", Me.LastMeasurementStatus)
                            End If
                            If detailsBuilder.Length > 0 Then detailsBuilder.AppendLine()
                            If Me.TryQueryMeasurementEventCondition Then
                                measurementCondition = Me.MeasurementEventCondition
                                detailsBuilder.AppendFormat("Measurement condition=0x{0:X4};. ", measurementCondition)
                            Else
                                measurementCondition = &HFFFF
                                detailsBuilder.AppendFormat("Failed getting measurement condition. Set to 0x{0:X4};. ", measurementCondition)
                            End If
                            outcome = outcome Or MeasurementOutcomes.HitCompliance
                        End If

                        If (numericValue And 2) <> 0 Then
                            If detailsBuilder.Length > 0 Then detailsBuilder.AppendLine()
                            detailsBuilder.AppendFormat("Sampling returned bad time stamps;. Device reported outcome=0x{0:X4} and status measurement condition=0x{1:X4}",
                                                        numericValue, measurementCondition)
                            outcome = outcome Or MeasurementOutcomes.UnspecifiedStatusException
                        End If

                        If (numericValue And 4) <> 0 Then
                            If detailsBuilder.Length > 0 Then detailsBuilder.AppendLine()
                            detailsBuilder.AppendFormat("Configuration failed;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}",
                                                        numericValue, measurementCondition)
                            outcome = outcome Or MeasurementOutcomes.UnspecifiedStatusException
                        End If

                        If (numericValue And 8) <> 0 Then
                            If detailsBuilder.Length > 0 Then detailsBuilder.AppendLine()
                            detailsBuilder.AppendFormat("Initiation failed;. Device reported outcome=0x{0:X4} and status measurement condition=0x{1:X4}",
                                                        numericValue, measurementCondition)
                            outcome = outcome Or MeasurementOutcomes.UnspecifiedStatusException
                        End If

                        If (numericValue And 16) <> 0 Then
                            If detailsBuilder.Length > 0 Then detailsBuilder.AppendLine()
                            detailsBuilder.AppendFormat("Load failed;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}",
                                                        numericValue, measurementCondition)
                            outcome = outcome Or MeasurementOutcomes.UnspecifiedStatusException
                        End If

                        If (numericValue And (Not 31)) <> 0 Then
                            If detailsBuilder.Length > 0 Then detailsBuilder.AppendLine()
                            detailsBuilder.AppendFormat("Unknown failure;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}",
                                                        numericValue, measurementCondition)
                            outcome = outcome Or MeasurementOutcomes.UnspecifiedStatusException
                        End If
                    End If
                End If
            Else
                If detailsBuilder.Length > 0 Then detailsBuilder.AppendLine()
                detailsBuilder.AppendFormat("Failed parsing outcome '{0}';. {1}{2}.", Me.LastOutcome,
                                            Environment.NewLine, New StackFrame(True).UserCallStack())
                outcome = MeasurementOutcomes.MeasurementFailed Or MeasurementOutcomes.UnexpectedReadingFormat
            End If
        End If
        details = detailsBuilder.ToString
        Return outcome
    End Function

#End Region

End Class

