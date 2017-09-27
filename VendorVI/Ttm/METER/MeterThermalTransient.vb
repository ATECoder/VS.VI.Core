Imports System.Windows.Forms
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
Public Class MeterThermalTransient
    Inherits MeterSubsystemBase

#Region " CONSTRUCTORS  AND  CLONES "

    ''' <summary> Default constructor. </summary>
    ''' <param name="thermalTransient"> The thermal transient element. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase, ByVal thermalTransient As ThermalTransient)
        MyBase.New(statusSubsystem)
        Me.MeterEntity = ThermalTransientMeterEntity.Transient
        Me.ThermalTransient = thermalTransient
    End Sub

    ''' <summary> Cleans up unmanaged or managed and unmanaged resources. </summary>
    ''' <param name="disposing"> True if this method releases both managed and unmanaged resources;
    ''' False if this method releases only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Me.ThermalTransient = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " RESETTABLE "

    ''' <summary> Clears known state. </summary>
    Public Overrides Sub ClearExecutionState()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing {0} thermal transient execution state;. ", Me.EntityName)
        MyBase.ClearExecutionState()
        Me.ThermalTransient.ClearExecutionState()
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Resetting {0} thermal transient to known state;. ", Me.EntityName)
        Windows.Forms.Application.DoEvents()
        MyBase.ResetKnownState()

        Windows.Forms.Application.DoEvents()
        Me.ThermalTransient.ResetKnownState()
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

#Region " DEVICE UNDER TEST: THERMAL TRANSIENT "

    ''' <summary> Gets or sets the <see cref="ThermalTransient">part Thermal Transient</see>. </summary>
    ''' <value> The cold resistance. </value>
    Public Property ThermalTransient As ThermalTransient

    ''' <summary> Gets the <see cref="ResistanceMeasureBase">part resistance element</see>. </summary>
    ''' <value> The cold resistance. </value>
    Public Overrides ReadOnly Property Resistance As ResistanceMeasureBase
        Get
            Return Me.ThermalTransient
        End Get
    End Property

#End Region

#Region " PULSE DURATION "

    ''' <summary> Gets the duration of the pulse. </summary>
    ''' <value> The pulse duration. </value>
    Public ReadOnly Property PulseDuration As TimeSpan
        Get
            Return TimeSpan.FromTicks(CLng(TimeSpan.TicksPerSecond * Me.TracePoints.GetValueOrDefault(0) * Me.SamplingInterval.GetValueOrDefault(0)))
        End Get
    End Property
#End Region

#Region " APERTURE "


    ''' <summary> Writes the Aperture without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output Aperture. The value is in Volts. The immediate
    ''' Aperture is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Aperture. </param>
    ''' <returns> The Aperture. </returns>
    Public Overrides Function WriteAperture(ByVal value As Double) As Double?
        MyBase.WriteAperture(value)
        ' update the maximum sampling rage.
        Me.Session.WriteLine("{0}:maxRateSetter()", Me.EntityName)
        ' and read the current period.
        Me.QuerySamplingInterval()
        Return Me.Aperture
    End Function

#End Region

#Region " MEDIAN FILTER SIZE "

    ''' <summary> The Median Filter Size. </summary>
    Private _MedianFilterSize As Integer?

    ''' <summary> Gets or sets the cached Median Filter Size. </summary>
    ''' <value> The Median Filter Size. </value>
    Public Property MedianFilterSize As Integer?
        Get
            Return Me._MedianFilterSize
        End Get
        Protected Set(ByVal value As Integer?)
            If value.HasValue Then
                Me.ThermalTransient.MedianFilterSize = value.Value
            Else
                Me.ThermalTransient.MedianFilterSize = 0
            End If
            If Not Nullable.Equals(Me.MedianFilterSize, value) Then
                Me._MedianFilterSize = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Median Filter Size. </summary>
    ''' <remarks> This command set the immediate output Median Filter Size. The value is in Volts. The
    ''' immediate Median Filter Size is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Median Filter Size. </param>
    ''' <returns> The Median Filter Size. </returns>
    Public Function ApplyMedianFilterSize(ByVal value As Integer) As Integer?
        Me.WriteMedianFilterSize(value)
        Return Me.QueryMedianFilterSize
    End Function

    ''' <summary> Queries the Median Filter Size. </summary>
    ''' <returns> The Median Filter Size or none if unknown. </returns>
    ''' <exception cref="DeviceException"> Thrown when a device error condition occurs. </exception>
    Public Function QueryMedianFilterSize() As Integer?
        Const printFormat As Integer = 1
        Me.MedianFilterSize = Me.Session.QueryPrint(Me.MedianFilterSize.GetValueOrDefault(5), printFormat, "{0}.medianFilterSize", Me.EntityName)
        Return Me.MedianFilterSize
    End Function

    ''' <summary> Writes the Median Filter Size without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output Median Filter Size. The value is in Volts. The
    ''' immediate MedianFilterSize is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Median Filter Size. </param>
    ''' <returns> The Median Filter Size. </returns>
    Public Function WriteMedianFilterSize(ByVal value As Integer) As Integer?
        Dim details As String = ""
        If Not ThermalTransient.ValidateMedianFilterSize(value, details) Then
            Throw New ArgumentOutOfRangeException("value", details)
        End If
        Me.Session.WriteLine("{0}:medianFilterSizeSetter({1})", Me.EntityName, value)
        Me.MedianFilterSize = value
        Return Me.MedianFilterSize
    End Function

#End Region

#Region " POST TRANSIENT DELAY "

    ''' <summary> The Post Transient Delay. </summary>
    Private _PostTransientDelay As Double?

    ''' <summary> Gets or sets the cached Source Post Transient Delay. </summary>
    ''' <value> The Source Post Transient Delay. Actual Voltage depends on the power supply mode. </value>
    Public Property PostTransientDelay As Double?
        Get
            Return Me._PostTransientDelay
        End Get
        Protected Set(ByVal value As Double?)
            If value.HasValue Then
                Me.ThermalTransient.PostTransientDelay = value.Value
            Else
                Me.ThermalTransient.PostTransientDelay = 0
            End If
            If Me.PostTransientDelay.Differs(value, 0.001) Then
                Me._PostTransientDelay = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Post Transient Delay. </summary>
    ''' <remarks> This command set the immediate output Post Transient Delay. The value is in Amperes. The
    ''' immediate Post Transient Delay is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Post Transient Delay. </param>
    ''' <returns> The Source Post Transient Delay. </returns>
    Public Function ApplyPostTransientDelay(ByVal value As Double) As Double?
        Me.WritePostTransientDelay(value)
        Return Me.QueryPostTransientDelay
    End Function

    ''' <summary> Queries the Post Transient Delay. </summary>
    ''' <returns> The Post Transient Delay or none if unknown. </returns>
    Public Function QueryPostTransientDelay() As Double?
        Const printFormat As Decimal = 9.3D
        Me.PostTransientDelay = Me.Session.QueryPrint(Me.PostTransientDelay.GetValueOrDefault(0.5), printFormat, "{0}.postTransientDelayGetter()", Me.BaseEntityName)
        Return Me.PostTransientDelay
    End Function

    ''' <summary> Writes the source Post Transient Delay without reading back the value from the
    ''' device. </summary>
    ''' <remarks> This command sets the immediate output Post Transient Delay. The value is in Amperes.
    ''' The immediate PostTransientDelay is the output Voltage setting. At *RST, the Voltage values =
    ''' 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Post Transient Delay. </param>
    ''' <returns> The Source Post Transient Delay. </returns>
    Public Function WritePostTransientDelay(ByVal value As Double) As Double?
        Dim details As String = ""
        If Not ThermalTransient.ValidatePostTransientDelay(value, details) Then
            Throw New ArgumentOutOfRangeException("value", details)
        End If
        Me.Session.WriteLine("{0}.postTransientDelaySetter({1})", Me.BaseEntityName, value)
        Me.PostTransientDelay = value
        Return Me.PostTransientDelay
    End Function

#End Region

#Region " SAMPLING INTERVAL "

    ''' <summary> The Sampling Interval. </summary>
    Private _SamplingInterval As Double?

    ''' <summary> Gets or sets the cached Sampling Interval. </summary>
    ''' <value> The Sampling Interval. </value>
    Public Property SamplingInterval As Double?
        Get
            Return Me._SamplingInterval
        End Get
        Protected Set(ByVal value As Double?)
            If value.HasValue Then
                Me.ThermalTransient.SamplingInterval = value.Value
            Else
                Me.ThermalTransient.SamplingInterval = 0
            End If
            If Me.SamplingInterval.Differs(value, 0.000001) Then
                Me._SamplingInterval = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sampling Interval. </summary>
    ''' <remarks> This command set the immediate output Sampling Interval. The value is in Ohms. The
    ''' immediate Sampling Interval is the output Low setting. At *RST, the Low values = 0. </remarks>
    ''' <param name="value"> The Sampling Interval. </param>
    ''' <returns> The Sampling Interval. </returns>
    Public Function ApplySamplingInterval(ByVal value As Double) As Double?
        Me.WriteSamplingInterval(value)
        Return Me.QuerySamplingInterval
    End Function

    ''' <summary> Queries the Sampling Interval. </summary>
    ''' <returns> The Sampling Interval or none if unknown. </returns>
    Public Function QuerySamplingInterval() As Double?
        Const printFormat As Decimal = 9.6D
        Me.SamplingInterval = Me.Session.QueryPrint(Me.SamplingInterval.GetValueOrDefault(0.0001), printFormat, "{0}.period", Me.EntityName)
        Return Me.SamplingInterval
    End Function

    ''' <summary> Writes the Sampling Interval without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output Sampling Interval. The value is in Ohms. The immediate
    ''' SamplingInterval is the output Low setting. At *RST, the Low values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Sampling Interval. </param>
    ''' <returns> The Sampling Interval. </returns>
    Public Function WriteSamplingInterval(ByVal value As Double) As Double?
        Dim details As String = ""
        If Not ThermalTransient.ValidateLimit(value, details) OrElse
            (Me.TracePoints.HasValue AndAlso Not ThermalTransient.ValidatePulseWidth(value, Me.TracePoints.Value, details)) Then
            Throw New ArgumentOutOfRangeException("value", details)
        End If
        Me.Session.WriteLine("{0}:periodSetter({1})", Me.EntityName, value)
        Me.SamplingInterval = value
        Return Me.SamplingInterval
    End Function

#End Region

#Region " TRACE POINTS "

    ''' <summary> The Trace Points. </summary>
    Private _TracePoints As Integer?

    ''' <summary> Gets or sets the cached Trace Points. </summary>
    ''' <value> The Trace Points. </value>
    Public Property TracePoints As Integer?
        Get
            Return Me._TracePoints
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.TracePoints, value) Then
                Me._TracePoints = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trace Points. </summary>
    ''' <remarks> This command set the immediate output Trace Points. The value is in Volts. The
    ''' immediate Trace Points is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Trace Points. </param>
    ''' <returns> The Trace Points. </returns>
    Public Function ApplyTracePoints(ByVal value As Integer) As Integer?
        Me.WriteTracePoints(value)
        Return Me.QueryTracePoints
    End Function

    ''' <summary> Queries the Trace Points. </summary>
    ''' <returns> The Trace Points or none if unknown. </returns>
    Public Function QueryTracePoints() As Integer?
        Const printFormat As Integer = 1
        Me.TracePoints = Me.Session.QueryPrint(Me.TracePoints.GetValueOrDefault(100), printFormat, "{0}.points", Me.EntityName)
        Return Me.TracePoints
    End Function

    ''' <summary> Writes the Trace Points without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output Trace Points. The value is in Volts. The
    ''' immediate TracePoints is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Trace Points. </param>
    ''' <returns> The Trace Points. </returns>
    Public Function WriteTracePoints(ByVal value As Integer) As Integer?
        Dim details As String = ""
        If Not ThermalTransient.ValidateTracePoints(value, details) OrElse
            (Me.SamplingInterval.HasValue AndAlso Not ThermalTransient.ValidatePulseWidth(Me.SamplingInterval.Value, value, details)) Then
            Throw New ArgumentOutOfRangeException("value", details)
        End If
        Me.Session.WriteLine("{0}:pointsSetter({1})", Me.EntityName, value)
        Me.TracePoints = value
        Return Me.TracePoints
    End Function

#End Region

#Region " TIME SERIES "

    Dim _LastTrace As String

    ''' <summary> Gets or sets (protected) the last trace. </summary>
    ''' <value> The last trace. </value>
    Public Property LastTrace() As String
        Get
            Return Me._lastTrace
        End Get
        Protected Set(ByVal value As String)
            Me._lastTrace = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    Dim _LastTimeSeries As List(Of System.Drawing.PointF)

    ''' <summary> Gets or sets the thermal transient time series. </summary>
    ''' <value> The last time series. </value>
    Public ReadOnly Property LastTimeSeries() As ObjectModel.ReadOnlyCollection(Of System.Drawing.PointF)
        Get
            Return New ObjectModel.ReadOnlyCollection(Of System.Drawing.PointF)(Me._lastTimeSeries)
        End Get
    End Property

#End Region

#Region " CHARTING "

    ''' <summary> Configure trace chart. </summary>
    ''' <param name="chart"> The <see cref="DataVisualization.Charting.Chart">Chart</see>. </param>
    Private Shared Sub ConfigureTraceChart(ByVal chart As DataVisualization.Charting.Chart)
        If chart.ChartAreas.Count = 0 Then
            Dim chartArea As New DataVisualization.Charting.ChartArea("Area")
            chart.ChartAreas.Add(chartArea)
            With (chartArea)
                .AxisY.Title = "Voltage, mV"
                .AxisY.Minimum = 0
                .AxisY.TitleFont = New Drawing.Font(chart.Parent.Font, Drawing.FontStyle.Bold)
                .AxisX.Title = "Time, ms"
                .AxisX.Minimum = 0
                .AxisX.TitleFont = New Drawing.Font(chart.Parent.Font, Drawing.FontStyle.Bold)
                .AxisX.IsLabelAutoFit = True
                With .AxisX.LabelStyle
                    .Format = "#.0"
                    .Enabled = True
                    .IsEndLabelVisible = True
                End With
            End With

            chart.Location = New System.Drawing.Point(82, 16)
            chart.Series.Add(New System.Windows.Forms.DataVisualization.Charting.Series("Trace"))
            With chart.Series("Trace")
                .SmartLabelStyle.Enabled = True
                .ChartArea = "Area"
                ' .Legend = "Legend"
                .ChartType = DataVisualization.Charting.SeriesChartType.Line
                .XAxisType = DataVisualization.Charting.AxisType.Primary
                .YAxisType = DataVisualization.Charting.AxisType.Primary
                .XValueType = DataVisualization.Charting.ChartValueType.Double
                .YValueType = DataVisualization.Charting.ChartValueType.Double
            End With
            chart.Text = "Thermal Transient Trace"
            With chart.Titles.Add("Thermal Transient Trace")
                .Font = New Drawing.Font(chart.Parent.Font.FontFamily, 11, Drawing.FontStyle.Bold)
            End With

        End If
    End Sub

    ''' <summary> Gets a simulate time series. </summary>
    ''' <param name="asymptote">    The asymptote. </param>
    ''' <param name="timeConstant"> The time constant. </param>
    ''' <returns> null if it fails, else a list of. </returns>
    Private Shared Function Simulate(ByVal asymptote As Double, ByVal timeConstant As Double) As ObjectModel.ReadOnlyCollection(Of System.Drawing.PointF)
        Dim l As New List(Of Drawing.PointF)
        Dim vc As Double = 0
        Dim deltaT As Double = 0.1
        For i As Integer = 0 To 99
            Dim deltaV As Double = deltaT * (asymptote - Vc) / timeConstant
            l.Add(New Drawing.PointF(CSng(deltaT * (i + 1)), CSng(Vc)))
            Vc += deltaV
        Next
        Return New ObjectModel.ReadOnlyCollection(Of System.Drawing.PointF)(l)
    End Function

    ''' <summary> Displays a trace described by timeSeries. </summary>
    ''' <param name="chart"> The <see cref="DataVisualization.Charting.Chart">Chart</see>. </param>
    ''' <param name="timeSeries"> The time series in millivolts and milliseconds. </param>
    Private Shared Sub DisplayTrace(ByVal chart As DataVisualization.Charting.Chart,
                             ByVal timeSeries As ObjectModel.ReadOnlyCollection(Of System.Drawing.PointF))
        With chart.Series(0)
            .Points.SuspendUpdates()
            .Points.Clear()
            .Points.AddXY(0, 0)
            Dim deltaT As Single = 0
            If timeSeries.Count > 1 Then
                deltaT = timeSeries.Item(1).X - timeSeries.Item(0).X
            End If
            For Each v As Drawing.PointF In timeSeries
                .Points.AddXY(v.X, v.Y)
            Next
            ' Add one more point
            .Points.AddXY(timeSeries.Item(timeSeries.Count - 1).X + deltaT,
                          2 * timeSeries(timeSeries.Count - 1).Y - timeSeries(timeSeries.Count - 2).Y)
            .Points.ResumeUpdates()
            .Points.Invalidate()
        End With
    End Sub

    ''' <summary> Displays a thermal transient trace. </summary>
    ''' <param name="chart"> The <see cref="DataVisualization.Charting.Chart">Chart</see>. </param>
    Public Shared Sub EmulateThermalTransientTrace(ByVal chart As DataVisualization.Charting.Chart)
        MeterThermalTransient.ConfigureTraceChart(chart)
        MeterThermalTransient.DisplayTrace(chart, Simulate(100, 5))
    End Sub

    ''' <summary> Displays a thermal transient trace. </summary>
    ''' <param name="chart"> The <see cref="DataVisualization.Charting.Chart">Chart</see>. </param>
    Public Sub DisplayThermalTransientTrace(ByVal chart As DataVisualization.Charting.Chart)
        If Me.NewTraceReadyToRead Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Reading trace;. ")
            Dim st As Diagnostics.Stopwatch = Diagnostics.Stopwatch.StartNew
            Me.ReadThermalTransientTrace()
            Dim t As TimeSpan = st.Elapsed
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Read trace in {0} ms;. ", t.TotalMilliseconds)
        Else
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Displaying thermal transient trace;. ")
        End If
        MeterThermalTransient.ConfigureTraceChart(chart)
        MeterThermalTransient.DisplayTrace(chart, Me.LastTimeSeries)
    End Sub


#End Region

#Region " VOLTAGE CHANGE "

    ''' <summary> The Voltage Change. </summary>
    Private _VoltageChange As Double?

    ''' <summary> Gets or sets the cached Voltage Change. </summary>
    ''' <value> The Voltage Change. </value>
    Public Property VoltageChange As Double?
        Get
            Return Me._VoltageChange
        End Get
        Protected Set(ByVal value As Double?)
            If value.HasValue Then
                Me.ThermalTransient.AllowedVoltageChange = value.Value
            Else
                Me.ThermalTransient.AllowedVoltageChange = 0
            End If
            If Me.VoltageChange.Differs(value, 0.000001) Then
                Me._VoltageChange = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Voltage Change. </summary>
    ''' <remarks> This command set the immediate output Voltage Change. The value is in Volts. The
    ''' immediate Voltage Change is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Voltage Change. </param>
    ''' <returns> The Voltage Change. </returns>
    Public Function ApplyVoltageChange(ByVal value As Double) As Double?
        Me.WriteVoltageChange(value)
        Return Me.QueryVoltageChange
    End Function

    ''' <summary> Queries the Voltage Change. </summary>
    ''' <returns> The Voltage Change or none if unknown. </returns>
    Public Function QueryVoltageChange() As Double?
        Const printFormat As Decimal = 9.6D
        Me.VoltageChange = Me.Session.QueryPrint(Me.VoltageChange.GetValueOrDefault(0.099), printFormat, "{0}.maxVoltageChange", Me.EntityName)
        Return Me.VoltageChange
    End Function

    ''' <summary> Writes the Voltage Change without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output Voltage Change. The value is in Volts. The
    ''' immediate VoltageChange is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ''' the required range. </exception>
    ''' <param name="value"> The Voltage Change. </param>
    ''' <returns> The Voltage Change. </returns>
    Public Function WriteVoltageChange(ByVal value As Double) As Double?
        Dim details As String = ""
        If Not ThermalTransient.ValidateVoltageChange(value, details) Then
            Throw New ArgumentOutOfRangeException("value", details)
        End If
        Me.Session.WriteLine("{0}:maxVoltageChangeSetter({1})", Me.EntityName, value)
        Me.VoltageChange = value
        Return Me.VoltageChange
    End Function

#End Region

#Region " CONFIGURE "

    ''' <summary> Applies the instrument defaults. </summary>
    ''' <remarks> This is required until the reset command gets implemented. </remarks>
    Public Overrides Sub ApplyInstrumentDefaults()
        MyBase.ApplyInstrumentDefaults()
        Me.StatusSubsystem.Write("{0}:maxRateSetter()", Me.EntityName)
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.StatusSubsystem.Write("{0}:maxVoltageChangeSetter({1}.level)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.StatusSubsystem.Write("{0}:latencySetter({1}.latency)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.StatusSubsystem.Write("{0}:medianFilterSizeSetter({1}.medianFilterSize)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
        Me.StatusSubsystem.Write("{0}:pointsSetter({1}.points)", Me.EntityName, Me.DefaultsName)
        Me.StatusSubsystem.QueryOperationCompleted()
    End Sub

    ''' <summary> Reads instrument defaults. </summary>
    Public Overrides Sub ReadInstrumentDefaults()
        MyBase.ReadInstrumentDefaults()
        With My.MySettings.Default

            If Not Me.Session.TryQueryPrint(7.4D, .ThermalTransientApertureDefault, "{0}.aperture", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient aperture;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientApertureMinimum, "{0}.minAperture", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient minimum aperture;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientApertureMaximum, "{0}.maxAperture", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient maximum aperture;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientCurrentLevelDefault, "{0}.level", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient current level;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientCurrentMinimum, "{0}.minCurrent", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient minimum current level;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientCurrentMaximum, "{0}.maxCurrent", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient maximum current level;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientVoltageLimitDefault, "{0}.limit", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient voltage limit;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientVoltageMinimum, "{0}.minVoltage", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient minimum voltage;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientVoltageMaximum, "{0}.maxVoltage", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient maximum voltage;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientHighLimitDefault, "{0}.highLimit", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient high limit;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientLowLimitDefault, "{0}.lowLimit", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient low limit;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientVoltageChangeDefault, "{0}.maxVoltageChange", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient maximum voltage change;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(1, .ThermalTransientMedianFilterLengthDefault, "{0}.medianFilterSize", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient median filter size;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientSamplingIntervalDefault, "{0}.period", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient period;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientSamplingIntervalMinimum, "{0}.minPeriod", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient minimum period;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(9.6D, .ThermalTransientSamplingIntervalMaximum, "{0}.maxPeriod", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient maximum period;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(1, .ThermalTransientTracePointsDefault, "{0}.points", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient trace points;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(1, .ThermalTransientTracePointsMinimum, "{0}.minPoints", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient minimum trace points;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If

            If Not Me.Session.TryQueryPrint(1, .ThermalTransientTracePointsMaximum, "{0}.maxPoints", Me.DefaultsName) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "failed reading default thermal transient maximum trace points;. Sent:'{0}; Received:'{1}'.",
                                           Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            End If
        End With

    End Sub

    ''' <summary> Configures the meter for making the thermal transient measurement. </summary>
    ''' <param name="thermalTransient"> The Thermal Transient element. </param>
    Public Overloads Sub Configure(ByVal thermalTransient As ThermalTransientBase)

        If thermalTransient Is Nothing Then Throw New ArgumentNullException(NameOf(thermalTransient))
        MyBase.Configure(thermalTransient)

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} post transient delay to {1};. ", Me.BaseEntityName, thermalTransient.PostTransientDelay)
        Me.ApplyPostTransientDelay(thermalTransient.PostTransientDelay)
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "{0} post transient delay set to {1};. ", Me.BaseEntityName, thermalTransient.PostTransientDelay)

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} Allowed Voltage Change to {1};. ", Me.EntityName, thermalTransient.AllowedVoltageChange)
        Me.ApplyVoltageChange(thermalTransient.AllowedVoltageChange)
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "{0} Allowed Voltage Change set to {1};. ", Me.EntityName, thermalTransient.AllowedVoltageChange)

        Me.StatusSubsystem.CheckThrowDeviceException(False, "configuring Thermal Transient measurement;. ")
        Me.ThermalTransient.CheckThrowUnequalConfiguration(thermalTransient)

    End Sub

    ''' <summary> Applies changed meter configuration. </summary>
    ''' <param name="thermalTransient"> The Thermal Transient element. </param>
    Public Overloads Sub ConfigureChanged(ByVal thermalTransient As ThermalTransientBase)

        If thermalTransient Is Nothing Then Throw New ArgumentNullException(NameOf(thermalTransient))
        MyBase.ConfigureChanged(thermalTransient)
        If Not Me.ThermalTransient.ConfigurationEquals(thermalTransient) Then

            If Not Me.ThermalTransient.PostTransientDelay.Equals(thermalTransient.PostTransientDelay) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} post transient delay to {1};. ", Me.BaseEntityName, thermalTransient.PostTransientDelay)
                Me.ApplyPostTransientDelay(thermalTransient.PostTransientDelay)
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "{0} post transient delay set to {1};. ", Me.BaseEntityName, thermalTransient.PostTransientDelay)
            End If

            If Not Me.ThermalTransient.AllowedVoltageChange.Equals(thermalTransient.AllowedVoltageChange) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting {0} Allowed Voltage Change to {1};. ", Me.EntityName, thermalTransient.AllowedVoltageChange)
                Me.ApplyVoltageChange(thermalTransient.AllowedVoltageChange)
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "{0} Allowed Voltage Change set to {1};. ", Me.EntityName, thermalTransient.AllowedVoltageChange)
            End If

        End If
        Me.StatusSubsystem.CheckThrowDeviceException(False, "configuring Thermal Transient measurement;. ")
        Me.ThermalTransient.CheckThrowUnequalConfiguration(thermalTransient)

    End Sub

    ''' <summary> Queries the configuration. </summary>
    Public Overrides Sub QueryConfiguration()
        MyBase.QueryConfiguration()
        Me.QueryMedianFilterSize()
        Me.QueryPostTransientDelay()
        Me.QuerySamplingInterval()
        Me.QueryTracePoints()
        Me.QueryVoltageChange()
        Me.StatusSubsystem.CheckThrowDeviceException(False, "Reading {0} configuration;. ", Me.EntityName)
    End Sub

#End Region

#Region " MEASURE "

    ''' <summary> Measures the Thermal Transient. </summary>
    ''' <param name="thermalTransient"> The Thermal Transient element. </param>
    Public Overloads Sub Measure(ByVal thermalTransient As ResistanceMeasureBase)
        If thermalTransient Is Nothing Then Throw New ArgumentNullException(NameOf(thermalTransient))
        If Me.Session.IsSessionOpen Then
            MyBase.Measure(thermalTransient)
            Me.ReadThermalTransient(thermalTransient)
        Else
            Me.EmulateThermalTransient(thermalTransient)
        End If
    End Sub

#End Region

#Region " READ "

    ''' <summary> Gets or sets the trace ready to read sentinel. </summary>
    ''' <value> The trace ready to read. </value>
    Public Property NewTraceReadyToRead As Boolean

    ''' <summary>
    ''' Reads the thermal transient. Sets the last <see cref="LastReading">reading</see>,
    ''' <see cref="LastOutcome">outcome</see> <see cref="LastMeasurementStatus">status</see> and
    ''' <see cref="MeasurementAvailable">Measurement available sentinel</see>.
    ''' The outcome is left empty if measurements were not made.
    ''' </summary>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    Public Sub ReadThermalTransient()

        Me.NewTraceReadyToRead = False
        If Me.QueryOutcomeNil() Then
            Me.LastReading = ""
            Me.LastOutcome = ""
            Me.LastMeasurementStatus = ""
            Throw New InvalidOperationException("Measurement Not made.")
        Else
            If Me.QueryOkay() Then
                Me.LastReading = Me.Session.QueryPrintStringFormatTrimEnd(9.6D, "{0}.voltageChange", MeterSubsystemBase.ThermalTransientEstimatorEntityName)
                Me.StatusSubsystem.CheckThrowDeviceException(False, "reading voltage change;. Sent: '{0}'; Received: '{1}'.",
                                                             Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
                Me.LastOutcome = "0"
                Me.LastMeasurementStatus = ""
                Me.NewTraceReadyToRead = True
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

    ''' <summary> Reads the Thermal Transient. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="thermalTransient"> The Thermal Transient element. </param>
    Public Sub ReadThermalTransient(ByVal thermalTransient As ResistanceMeasureBase)
        If thermalTransient Is Nothing Then Throw New ArgumentNullException(NameOf(thermalTransient))
        Me.ReadThermalTransient()
        Me.StatusSubsystem.CheckThrowDeviceException(False, "Reading Thermal Transient;. last command: '{0}'", Me.Session.LastMessageSent)
        Dim measurementOutcome As MeasurementOutcomes = MeasurementOutcomes.None
        If Me.LastOutcome <> "0" Then
            Dim details As String = ""
            measurementOutcome = Me.ParseOutcome(details)
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Measurement failed;. Details: {0}", details)
        End If
        Me.ThermalTransient.ParseReading(Me.LastReading, thermalTransient.CurrentLevel, measurementOutcome)
        thermalTransient.ParseReading(Me.LastReading, thermalTransient.CurrentLevel, measurementOutcome)
    End Sub

    ''' <summary> Emulates a Thermal Transient. </summary>
    ''' <param name="thermalTransient"> The Thermal Transient element. </param>
    Public Sub EmulateThermalTransient(ByVal thermalTransient As ResistanceMeasureBase)
        If thermalTransient Is Nothing Then Throw New ArgumentNullException(NameOf(thermalTransient))
        Me.ThermalTransient.EmulateReading()
        Me.LastReading = Me.ThermalTransient.LastReading
        Me.LastOutcome = Me.ThermalTransient.LastOutcome
        Me.LastMeasurementStatus = ""
        Dim measurementOutcome As MeasurementOutcomes = MeasurementOutcomes.None
        Me.ThermalTransient.ParseReading(Me.LastReading, thermalTransient.CurrentLevel, measurementOutcome)
        thermalTransient.ParseReading(Me.LastReading, thermalTransient.CurrentLevel, measurementOutcome)
    End Sub


    ''' <summary> Reads the thermal transient trace from the instrument into a
    ''' <see cref="LastTrace">comma-separated array of times in milliseconds and values in
    ''' milli volts.</see> and
    ''' <see cref="LastTimeSeries">collection of times in milliseconds and values in millivolts.</see>
    ''' Also updates the outcome and status. </summary>
    Public Sub ReadThermalTransientTrace()

        ' clear the new trace sentinel
        Me.NewTraceReadyToRead = False
        Dim delimiter As String = ","
        Me._lastTimeSeries = New List(Of Drawing.PointF)
        If Me.LastOutcome <> "0" Then
            Me.LastTrace = ""
            Throw New InvalidOperationException("Measurement failed.")
        Else
            Dim builder As New System.Text.StringBuilder
            Me.Session.WriteLine("{0}:printTimeSeries(4)", Me.EntityName)
            Me.StatusSubsystem.TraceVisaOperation("sent query '{0}';. ", Me.Session.LastMessageSent)
            Do While Me.Session.IsMessageAvailable(TimeSpan.FromMilliseconds(1), 3)
                Dim reading As String = Me.Session.ReadLine
                If builder.Length > 0 Then
                    builder.Append(delimiter)
                End If
                builder.Append(reading)
                Dim xy As String() = reading.Split(","c)
                Dim x, y As Single
                If Not Single.TryParse(xy(0).Trim, Globalization.NumberStyles.Number, Globalization.CultureInfo.CurrentCulture, x) Then
                    x = -1
                End If
                If Not Single.TryParse(xy(1).Trim, Globalization.NumberStyles.Number, Globalization.CultureInfo.CurrentCulture, y) Then
                    y = -1
                End If
                Dim point As New Drawing.PointF(x, y)
                Me._lastTimeSeries.Add(point)
            Loop
            Me.LastTrace = builder.ToString
        End If
        Me.SafePostPropertyChanged()

    End Sub

#End Region

#Region " ESTIMATE "

    Dim _Model As isr.Algorithms.Optima.PulseResponseFunction

    ''' <summary> Gets the model. </summary>
    ''' <value> The model. </value>
    Public ReadOnly Property Model As isr.Algorithms.Optima.PulseResponseFunction
        Get
            Return Me._model
        End Get
    End Property

    Dim _Simplex As isr.Algorithms.Optima.Simplex

    ''' <summary> Model transient response. </summary>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <param name="thermalTransient"> The thermal transient element. </param>
    Public Sub ModelTransientResponse(ByVal thermalTransient As ThermalTransient)

        If thermalTransient Is Nothing Then
            Throw New ArgumentNullException(NameOf(thermalTransient))
        ElseIf Me.LastTimeSeries Is Nothing Then
            Throw New InvalidOperationException("Last time series not read")
        ElseIf Me.LastTimeSeries.Count = 0 Then
            Throw New InvalidOperationException("Time series has no values")
        ElseIf Me.LastTimeSeries.Count <= 10 Then
            Throw New InvalidOperationException("Time series has less than 10 values")
        End If
        Dim voltageScale As Double = 1000 ' scale to millivolts
        Dim timeScale As Double = 1000 ' scale to milliseconds
        Dim flatnessPrecisionFactor As Double = 0.0001
        Dim pulseDuration As Double = timeScale * Me.PulseDuration.TotalSeconds
        Dim estimatedTau As Double = timeScale * 0.5 * Me.PulseDuration.TotalSeconds
        Dim estimatedAsymptote As Double = voltageScale * Me.ThermalTransient.HighLimit
        Dim voltageRange As Double() = New Double() {0.5 * estimatedAsymptote, 2 * estimatedAsymptote}
        Dim voltagePresision As Double = flatnessPrecisionFactor * estimatedAsymptote
        Dim negativeInverseTauRange As Double() = New Double() {-1 / (0.5 * estimatedTau), -1 / (2 * estimatedTau)}
        Dim inverseTauPrecision As Double = flatnessPrecisionFactor / estimatedTau
        Dim dimension As Integer = 2
        Dim maximumIterations As Integer = 100
        Dim expectedDeviation As Double = 0.05 * estimatedAsymptote
        Dim expectedMaximumSSQ As Double = Me.ThermalTransient.TracePoints * expectedDeviation * expectedDeviation
        Dim objectivePrecisionFactor As Double = 0.0001
        Dim objectivePrecision As Double = objectivePrecisionFactor * expectedMaximumSSQ

        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Instantiating Simplex")
        Me._simplex = New isr.Algorithms.Optima.Simplex(dimension,
                                                        New Double() {voltageRange(0), negativeInverseTauRange(0)},
                                                        New Double() {voltageRange(1), negativeInverseTauRange(1)}, maximumIterations,
                                                        New Double() {voltagePresision, inverseTauPrecision},
                                                        objectivePrecision)

        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Instantiating Thermal Transient model")
        Me._model = New isr.Algorithms.Optima.PulseResponseFunction(Me.LastTimeSeries)

        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Initializing simplex")
        Me._simplex.Initialize(Me._model)
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Initial simplex is {0}", Me._simplex.ToString)
        Me._simplex.Solve()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Finished in {0} Iterations", Me._simplex.IterationNumber)
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Final simplex is {0}", Me._simplex.ToString)
        Me.Model.EvaluateFunctionValues(Me._simplex.BestSolution.Values)

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                   "Estimated Asymptote = {0:G4} mV", Me._simplex.BestSolution.Values(0))
        thermalTransient.Asymptote = 0.001 * Me._simplex.BestSolution.Values(0)

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                   "Estimated Time Constant = {0:G4} ms", -1 / Me._simplex.BestSolution.Values(1))
        thermalTransient.TimeConstant = -0.001 / Me._simplex.BestSolution.Values(1)

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                   "Estimated Thermal Transient Voltage = {0:G4} mV", _model.FunctionValue(Me._simplex.BestSolution.Values,
                                                                                                        New Double() {pulseDuration, 0}))
        thermalTransient.EstimatedVoltage = 0.001 * _model.FunctionValue(Me._simplex.BestSolution.Values, New Double() {pulseDuration, 0})

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                   "Correlation Coefficient = {0:G4}", _model.EvaluateCorrelationCoefficient)
        thermalTransient.CorrelationCoefficient = _model.EvaluateCorrelationCoefficient

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                   "Standard Error = {0:G4} mV", _model.EvaluateStandardError(Me._simplex.BestSolution.Objective))
        thermalTransient.StandardError = 0.001 * _model.EvaluateStandardError(Me._simplex.BestSolution.Objective)

        thermalTransient.Iterations = Me._simplex.IterationNumber

        If Me._simplex.Converged Then
            thermalTransient.OptimizationOutcome = OptimizationOutcome.Converged
        ElseIf Me._simplex.Optimized Then
            thermalTransient.OptimizationOutcome = OptimizationOutcome.Optimized
        ElseIf Me._simplex.IterationNumber >= maximumIterations Then
            thermalTransient.OptimizationOutcome = OptimizationOutcome.Exhausted
        Else
            thermalTransient.OptimizationOutcome = OptimizationOutcome.None
        End If

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Converged = {0}", Me._simplex.Converged)
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Optimized = {0}", Me._simplex.Optimized)

    End Sub

    ''' <summary> Displays a trace described by timeSeries. </summary>
    ''' <param name="chart">          The <see cref="DataVisualization.Charting.Chart">Chart</see>. </param>
    Public Sub DisplayModel(ByVal chart As DataVisualization.Charting.Chart)
        If chart Is Nothing Then Throw New ArgumentNullException(NameOf(chart))
        If chart.Series.Count = 1 Then
            chart.Series.Add("Model")
            With chart.Series(1)
                .ChartType = DataVisualization.Charting.SeriesChartType.Line
            End With
        End If
        With chart.Series(1)
            .Points.SuspendUpdates()
            .Points.Clear()
            .Color = Drawing.Color.Chocolate
            For i As Integer = 0 To Me.Model.FunctionValues.Count - 1
                Dim x As Double = Me.LastTimeSeries(i).X
                Dim y As Double = Me.Model.FunctionValues(i)
                If x > 0 AndAlso y > 0 Then
                    .Points.AddXY(x, y)
                End If
            Next
            .Points.ResumeUpdates()
            .Points.Invalidate()
        End With
    End Sub

#End Region


End Class


