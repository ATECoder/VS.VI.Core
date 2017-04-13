Imports isr.Core.Pith.NumericExtensions
''' <summary> Thermal transient. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="12/23/2013" by="David" revision=""> Created. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance")>
Public Class ThermalTransientEstimator
    Inherits MeterSubsystemBase

#Region " CONSTRUCTORS  AND  CLONES "

    ''' <summary> Default constructor. </summary>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.MeterEntity = ThermalTransientMeterEntity.Estimator
    End Sub

#End Region

#Region " RESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        Me.ReadInstrumentDefaults()
        Me.ApplyInstrumentDefaults()
        MyBase.ResetKnownState()
        Me.QueryConfiguration()
    End Sub

#End Region

#Region " PART "

    ''' <summary> Gets the <see cref="ResistanceMeasureBase">part resistance element</see>. </summary>
    ''' <value> The cold resistance. </value>
    Public Overrides ReadOnly Property Resistance As ResistanceMeasureBase
        Get
            Return Nothing
        End Get
    End Property

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

    ''' <summary> Gets the preset command. </summary>
    ''' <value> The preset command. </value>
    Protected Overrides ReadOnly Property PresetCommand As String
        Get
            ' no status preset command.
            Return ""
        End Get
    End Property

#End Region

#Region " THERMAL COEFFICIENT "

    ''' <summary> The Thermal Coefficient. </summary>
    Private _ThermalCoefficient As Double?

    ''' <summary> Gets or sets the cached Thermal Coefficient. </summary>
    ''' <value> The Thermal Coefficient. </value>
    Public Overloads Property ThermalCoefficient As Double?
        Get
            Return Me._ThermalCoefficient
        End Get
        Protected Set(ByVal value As Double?)
            If Me.ThermalCoefficient.Differs(value, 0.000000001) Then
                Me._ThermalCoefficient = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Thermal Coefficient. </summary>
    ''' <remarks> This command set the immediate output Thermal Coefficient. The value is in Volts. The
    ''' immediate ThermalCoefficient is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Thermal Coefficient. </param>
    ''' <returns> The Thermal Coefficient. </returns>
    Public Function ApplyThermalCoefficient(ByVal value As Double) As Double?
        Me.WriteThermalCoefficient(value)
        Return Me.QueryThermalCoefficient
    End Function

    ''' <summary> Queries the Thermal Coefficient. </summary>
    ''' <remarks> David, 1/6/2016. </remarks>
    ''' <returns> The ThermalCoefficient or none if unknown. </returns>
    Public Function QueryThermalCoefficient() As Double?
        Const printFormat As Decimal = 9.6D
        Me.ThermalCoefficient = Me.Session.QueryPrint(Me.ThermalCoefficient.GetValueOrDefault(0), printFormat, "{0}.thermalCoefficient", Me.EntityName)
        Return Me.ThermalCoefficient
    End Function

    ''' <summary> Writes the ThermalCoefficient without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output Thermal Coefficient. The value is in Volts. The immediate
    ''' ThermalCoefficient is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Thermal Coefficient. </param>
    ''' <returns> The Thermal Coefficient. </returns>
    Public Function WriteThermalCoefficient(ByVal value As Double) As Double?
        Dim details As String = ""
        If Not ThermalTransient.ValidateThermalCoefficient(value, details) Then
            Throw New ArgumentOutOfRangeException("value", details)
        End If
        Me.Session.WriteLine("{0}:thermalCoefficientSetter({1})", Me.EntityName, value)
        Me.ThermalCoefficient = value
        Return Me.ThermalCoefficient
    End Function

#End Region

#Region " CONFIGURE "

    ''' <summary> Applies the instrument defaults. </summary>
    ''' <remarks> This is required until the reset command gets implemented. </remarks>
    Public Overrides Sub ApplyInstrumentDefaults()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Applying {0} defaults.", Me.EntityName)
        Me.StatusSubsystem.Write("{0}:thermalCoefficientSetter({1}.thermalCoefficient)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
    End Sub

    ''' <summary> Reads instrument defaults. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId:="System.Decimal.TryParse(System.String,System.Decimal@)")>
    Public Overrides Sub ReadInstrumentDefaults()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Reading {0} defaults.", Me.EntityName)
        With My.MySettings.Default
            If Not Me.Session.TryQueryPrint(9.6D, .ThermalCoefficientDefault, "{0}.thermalCoefficient", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                           "failed reading default thermal transient estimator thermal coefficient;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
        End With
    End Sub

    ''' <summary> Configures the meter for estimation. </summary>
    ''' <param name="resistance"> The resistance. </param>
    Public Overrides Sub Configure(resistance As ResistanceMeasureBase)
    End Sub

    ''' <summary> Queries the configuration. </summary>
    Public Overrides Sub QueryConfiguration()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Reading {0} configuration.", Me.EntityName)
        Me.QueryThermalCoefficient()
        Me.StatusSubsystem.CheckThrowDeviceException(False, "Reading {0} configuration;. ", Me.EntityName)
    End Sub

#End Region

#Region " READ "

    ''' <summary>
    ''' Reads the thermal transient. Sets the last <see cref="LastReading">reading</see>,
    ''' <see cref="LastOutcome">outcome</see> <see cref="LastMeasurementStatus">status</see> and
    ''' <see cref="MeasurementAvailable">Measurement available sentinel</see>.
    ''' The outcome is left empty if measurements were not made.
    ''' </summary>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    Public Sub ReadThermalTransient()

        If Me.QueryOutcomeNil() Then
            Me.LastReading = ""
            Me.LastOutcome = ""
            Me.LastMeasurementStatus = ""
            Throw New InvalidOperationException("Measurement not made.")
        Else
            If Me.QueryOkay() Then
                Me.LastReading = Me.Session.QueryPrintStringFormatTrimEnd(9.6D, "{0}.voltageChange", Me.EntityName)
                Me.StatusSubsystem.CheckThrowDeviceException(False, "reading voltage change;. Sent: '{0}'; Received: '{1}'.",
                                                             Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
                Me.LastOutcome = "0"
                Me.LastMeasurementStatus = ""
            Else
                ' if outcome failed, read and parse the outcome and status.
                Me.LastOutcome = Me.Session.QueryPrintStringFormatTrimEnd(1, "{0}.outcome", Me.EntityName)
                Me.StatusSubsystem.CheckThrowDeviceException(False, "reading outcome;. Sent: '{0}'; Received: '{1}'.",
                                                             Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
                Me.LastMeasurementStatus = Me.Session.QueryPrintStringFormatTrimEnd(1, "{0}.status", Me.EntityName)
                Me.StatusSubsystem.CheckThrowDeviceException(False, "reading status;. Sent: '{0}'; Received: '{1}'.",
                                                             Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
                Me.LastReading = ""
            End If
        End If
        Me.MeasurementAvailable = True
    End Sub

#End Region

End Class
