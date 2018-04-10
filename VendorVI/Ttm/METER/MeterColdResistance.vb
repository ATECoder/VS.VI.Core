''' <summary> The Meter Cold Resistance. </summary>
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
Public Class MeterColdResistance
    Inherits MeterSubsystemBase

#Region " CONSTRUCTORS  AND  CLONES "

    ''' <summary> Default constructor. </summary>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    ''' <param name="resistance">      The cold resistance. </param>
    ''' <param name="meterEntity">     The meter entity type. </param>
    Public Sub New(ByVal statusSubsystem As StatusSubsystemBase, ByVal resistance As ColdResistance, ByVal meterEntity As ThermalTransientMeterEntity)
        MyBase.New(statusSubsystem)
        Me.MeterEntity = meterEntity
        Me.ColdResistance = resistance
    End Sub

    ''' <summary> Cleans up unmanaged or managed and unmanaged resources. </summary>
    ''' <param name="disposing"> True if this method releases both managed and unmanaged resources;
    ''' False if this method releases only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        Try

            If Not Me.IsDisposed Then

                If disposing Then
                    Me.ColdResistance = Nothing
                End If

                ' Free shared unmanaged resources

            End If

        Finally

            ' dispose the base class.
            MyBase.Dispose(disposing)

        End Try

    End Sub

#End Region

#Region " RESETTABLE "

    ''' <summary> Clears known state. </summary>
    Public Overrides Sub ClearExecutionState()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing {0} resistance execution state;. ", Me.EntityName)
        MyBase.ClearExecutionState()
        Me.ColdResistance.ClearExecutionState()
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
#If False Then
            Me.Session.WriteLine("{0}.level = nil", Me.EntityName)
            Dim b As Boolean = Me.Session.IsStatementTrue("{0}.level==nil", Me.EntityName)
            'Me.Session.WriteLine("{0}.level = 0.09", Me.EntityName)
            'B = Me.Session.IsStatementTrue("{0}.level==nil", Me.EntityName)
            Dim val As String = Me.Session.QueryPrintStringFormat("%9.6f", "_G.ttm.coldResistance.Defaults.level")
#End If
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Resetting {0} resistance known state;. ", Me.EntityName)
        Windows.Forms.Application.DoEvents()
        MyBase.ResetKnownState()
        Windows.Forms.Application.DoEvents()
        Me.ColdResistance.ResetKnownState()
        Windows.Forms.Application.DoEvents()
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

    ''' <summary> Gets the preset command. </summary>
    ''' <value> The preset command. </value>
    Protected Overrides ReadOnly Property PresetCommand As String
        Get
            ' no status preset command.
            Return ""
        End Get
    End Property

#End Region

#Region " DEVICE UNDER TEST: COLD RESISTANCE "

    ''' <summary> Gets or sets the <see cref="ColdResistance">cold resistance</see>. </summary>
    ''' <value> The cold resistance. </value>
    Public Property ColdResistance As ColdResistance

    ''' <summary> Gets the <see cref="ResistanceMeasureBase">part resistance element</see>. </summary>
    ''' <value> The cold resistance. </value>
    Public Overrides ReadOnly Property Resistance As ResistanceMeasureBase
        Get
            Return Me.ColdResistance
        End Get
    End Property

#End Region

#Region " CONFIGURE "

    ''' <summary> Reads instrument defaults. </summary>
    Public Overrides Sub ReadInstrumentDefaults()
        MyBase.ReadInstrumentDefaults()
        With My.MySettings.Default
            If Not Me.Session.TryQueryPrint(7.4D, .ColdResistanceApertureDefault, "{0}.aperture", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance aperture;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

            If Not Me.Session.TryQueryPrint(9.6D, .ColdResistanceCurrentLevelDefault, "{0}.level", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance current level;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

            If Not Me.Session.TryQueryPrint(9.6D, .ColdResistanceCurrentMinimum, "{0}.minCurrent", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance minimum current;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

            If Not Me.Session.TryQueryPrint(9.6D, .ColdResistanceCurrentMaximum, "{0}.maxCurrent", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance maximum current;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

            If Not Me.Session.TryQueryPrint(9.6D, .ColdResistanceVoltageLimitDefault, "{0}.limit", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance voltage limit;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

            If Not Me.Session.TryQueryPrint(9.6D, .ColdResistanceVoltageMinimum, "{0}.minVoltage", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance minimum voltage;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

            If Not Me.Session.TryQueryPrint(9.6D, .ColdResistanceVoltageMaximum, "{0}.maxVoltage", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance maximum voltage;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

            If Not Me.Session.TryQueryPrint(9.6D, .ColdResistanceLowLimitDefault, "{0}.lowLimit", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance low limit;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

            If Not Me.Session.TryQueryPrint(9.6D, .ColdResistanceHighLimitDefault, "{0}.highLimit", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance high limit;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

            If Not Me.Session.TryQueryPrint(9.6D, .ColdResistanceMinimum, "{0}.minResistance", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance minimum resistance;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

            If Not Me.Session.TryQueryPrint(9.6D, .ColdResistanceMaximum, "{0}.maxResistance", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default cold resistance maximum resistance;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
            Windows.Forms.Application.DoEvents()

        End With

    End Sub

    ''' <summary> Configures the meter for making the cold resistance measurement. </summary>
    ''' <param name="resistance"> The cold resistance. </param>
    Public Overrides Sub Configure(ByVal resistance As ResistanceMeasureBase)
        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))
        MyBase.Configure(resistance)
        Me.CheckThrowDeviceException(False, "configuring {0} measurement;. ", Me.EntityName)
        Me.ColdResistance.CheckThrowUnequalConfiguration(resistance)
    End Sub

    ''' <summary> Applies changed meter configuration. </summary>
    ''' <param name="resistance"> The cold resistance. </param>
    Public Overrides Sub ConfigureChanged(ByVal resistance As ResistanceMeasureBase)
        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))
        MyBase.ConfigureChanged(resistance)
        Me.StatusSubsystem.CheckThrowDeviceException(False, "configuring {0} measurement;. ", Me.EntityName)
        Me.ColdResistance.CheckThrowUnequalConfiguration(resistance)
    End Sub

    ''' <summary> Queries the configuration. </summary>
    Public Overrides Sub QueryConfiguration()
        MyBase.QueryConfiguration()
        Me.StatusSubsystem.CheckThrowDeviceException(False, "Reading {0} configuration;. ", Me.EntityName)
    End Sub

#End Region

#Region " MEASURE "

    ''' <summary> Measures the Final resistance. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resistance"> The resistance. </param>
    Public Overloads Sub Measure(ByVal resistance As ResistanceMeasureBase)
        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))
        Me.Session.MakeEmulatedReply(resistance.GenerateRandomReading)
        MyBase.Measure(resistance)
        Me.ReadResistance(resistance)
        ' Me.EmulateResistance(resistance)
    End Sub

#End Region

#Region " READ "

    ''' <summary>
    ''' Reads the resistance. Sets the last <see cref="LastReading">reading</see>,
    ''' <see cref="LastOutcome">outcome</see> <see cref="LastMeasurementStatus">status</see> and
    ''' <see cref="MeasurementAvailable">Measurement available sentinel</see>.
    ''' The outcome is left empty if measurements were not made.
    ''' </summary>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    Public Sub ReadResistance()

        If Me.QueryOutcomeNil() Then
            Me.LastReading = ""
            Me.LastOutcome = ""
            Me.LastMeasurementStatus = ""
            Throw New InvalidOperationException("Measurement not made.")
        Else
            If Me.QueryOkay() Then
                Me.LastReading = Me.Session.QueryPrintStringFormatTrimEnd(8.5D, "{0}.resistance", Me.EntityName)
                Me.StatusSubsystem.CheckThrowDeviceException(False, "reading resistance;. Sent: '{0}'; Received: '{1}'.",
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
        Me.ColdResistance.LastOutcome = Me.LastOutcome

        Me.MeasurementAvailable = True
    End Sub

    ''' <summary> Reads cold resistance. </summary>
    ''' <param name="resistance"> The cold resistance. </param>
    Public Sub ReadResistance(ByVal resistance As ResistanceMeasureBase)
        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))
        Me.ReadResistance()
        Me.StatusSubsystem.CheckThrowDeviceException(False, "Reading Resistance;. last command: '{0}'", Me.Session.LastMessageSent)
        Dim measurementOutcome As MeasurementOutcomes = MeasurementOutcomes.None
        If Me.LastOutcome <> "0" Then
            Dim details As String = ""
            measurementOutcome = Me.ParseOutcome(details)
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Measurement failed;. Details: {0}", details)
        End If
        Me.ColdResistance.ParseReading(Me.LastReading, measurementOutcome)
        resistance.ParseReading(Me.LastReading, measurementOutcome)
    End Sub

    ''' <summary> Emulates cold resistance. </summary>
    ''' <param name="resistance"> The cold resistance. </param>
    Public Sub EmulateResistance(ByVal resistance As ResistanceMeasureBase)
        If resistance Is Nothing Then Throw New ArgumentNullException(NameOf(resistance))
        Me.ColdResistance.EmulateReading()
        Me.LastReading = Me.ColdResistance.LastReading
        Me.LastOutcome = Me.ColdResistance.LastOutcome
        Me.LastMeasurementStatus = ""
        Dim measurementOutcome As MeasurementOutcomes = MeasurementOutcomes.None
        Me.ColdResistance.ParseReading(Me.LastReading, measurementOutcome)
        resistance.ParseReading(Me.LastReading, measurementOutcome)
    End Sub

#End Region

End Class
