Imports System.ComponentModel
''' <summary> Panel for editing the measurement. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="3/17/2014" by="David" revision=""> Created. </history>
Public Class MeasurementPanel
    Inherits MeasurementPanelBase

    ''' <summary> Releases the resources. </summary>
    Protected Overrides Sub ReleaseResources()
        Me.Meter = Nothing
        MyBase.ReleaseResources()
    End Sub

    ''' <summary> Gets the is device open. </summary>
    ''' <value> The is device open. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
    Public Overrides ReadOnly Property IsDeviceOpen As Boolean
        Get
            If Me.Meter Is Nothing Then
                Return False
            Else
                Return Me.Meter.IsDeviceOpen
            End If
        End Get
    End Property

    Private _Meter As Meter

    ''' <summary> Gets or sets the meter. </summary>
    ''' <value> The meter. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Meter As Meter
        Get
            Return Me._Meter
        End Get
        Set(value As Meter)
            Me._Meter = value
            If value IsNot Nothing Then
                Me.MasterDevice = Meter.MasterDevice
                Me.TriggerSequencer = value.TriggerSequencer
                Me.MeasureSequencer = value.MeasureSequencer
                MyBase.OnStateChanged()
            End If
        End Set
    End Property

    ''' <summary> Abort Trigger Sequence if started. </summary>
    Protected Overrides Sub AbortTriggerSequenceIf()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Aborting measurements;. ")
        Me.Meter.AbortTriggerSequenceIf()
    End Sub

    ''' <summary> Clears the part measurements. </summary>
    Protected Overrides Sub ClearPartMeasurements()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing part measurements;. ")
        Me.Meter.Part.ClearMeasurements()
    End Sub

    ''' <summary> Measure initial resistance. </summary>
    ''' <returns> The measurement <see cref="MeasurementOutcomes">outcome</see>. </returns>
    Protected Overrides Function MeasureInitialResistance() As MeasurementOutcomes
        Me.Meter.MeasureInitialResistance(Me.Meter.Part.InitialResistance)
        Return Me.Meter.Part.Outcome
    End Function

    ''' <summary> Measure thermal transient. </summary>
    ''' <returns> The measurement <see cref="MeasurementOutcomes">outcome</see>. </returns>
    Protected Overrides Function MeasureThermalTransient() As MeasurementOutcomes
        Me.Meter.MeasureThermalTransient(Me.Meter.Part.ThermalTransient)
        Return Me.Meter.Part.Outcome
    End Function

    ''' <summary> Measure final resistance. </summary>
    ''' <returns> The measurement <see cref="MeasurementOutcomes">outcome</see>. </returns>
    Protected Overrides Function MeasureFinalResistance() As MeasurementOutcomes
        Me.Meter.MeasureFinalResistance(Me.Meter.Part.FinalResistance)
        Return Me.Meter.Part.Outcome
    End Function

    ''' <summary> Displays a thermal transient trace described by chart. </summary>
    ''' <param name="chart"> The chart. </param>
    ''' <returns> A list of trace values. </returns>
    Protected Overrides Function DisplayThermalTransientTrace(chart As System.Windows.Forms.DataVisualization.Charting.Chart) As System.Collections.ObjectModel.ReadOnlyCollection(Of System.Drawing.PointF)
        Me.Meter.ThermalTransient.DisplayThermalTransientTrace(chart)
        Return Me.Meter.ThermalTransient.LastTimeSeries
    End Function

    ''' <summary> Models the thermal transient trace. </summary>
    ''' <param name="chart"> The chart. </param>
    Protected Overrides Sub ModelThermalTransientTrace(chart As System.Windows.Forms.DataVisualization.Charting.Chart)
        Me.Meter.ThermalTransient.ModelTransientResponse(Me.Meter.Part.ThermalTransient)
        Me.Meter.ThermalTransient.DisplayModel(chart)
    End Sub

End Class
