Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.Core.Pith.ErrorProviderExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Measurement panel base. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="4/12/2014" by="David" revision=""> Created. </history>
''' <history date="3/15/2014" by="David">             > Created. </history>
Public Class MeasurementPanelBase
    Inherits isr.Core.Pith.TalkerControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> A private constructor for this class making it not publicly creatable. This ensure
    ''' using the class as a singleton. </summary>
    Protected Sub New()
        MyBase.New()
        Me.InitializeComponent()
        Me._TtmMeasureControlsToolStrip.Enabled = False
    End Sub

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                Me.ReleaseResources()
                If components IsNot Nothing Then
                    components.Dispose()
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " ON CONTROL EVENTS "

    ''' <summary> Releases the resources. </summary>
    Protected Overridable Sub ReleaseResources()
        Me._TriggerSequencer = Nothing
        Me._MeasureSequencer = Nothing
        Me._LastTimeSeries = Nothing
    End Sub

    ''' <summary> Gets or sets the is device open. </summary>
    ''' <value> The is device open. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overridable ReadOnly Property IsDeviceOpen As Boolean

    Private _IsTraceAvailable As Boolean
    ''' <summary> Updates the availability of the controls. </summary>
    Protected Sub OnStateChanged()

        Dim measurementSequenceState As MeasurementSequenceState = MeasurementSequenceState.None
        If Me.MeasureSequencer IsNot Nothing Then
            measurementSequenceState = Me.MeasureSequencer.MeasurementSequenceState
        End If

        Dim triggerSequenceState As TriggerSequenceState = TriggerSequenceState.None
        If Me.TriggerSequencer IsNot Nothing Then
            triggerSequenceState = Me.TriggerSequencer.TriggerSequenceState
        End If

        Me._TtmMeasureControlsToolStrip.Enabled = Me.IsDeviceOpen

        Me._TriggerToolStripDropDownButton.Enabled = Me.IsDeviceOpen AndAlso MeasurementSequenceState.Idle = measurementSequenceState
        If Me._TriggerToolStripDropDownButton.Enabled Then
            Me._WaitForTriggerToolStripMenuItem.Enabled = Me.IsDeviceOpen AndAlso
                                                         (MeasurementSequenceState.Idle = measurementSequenceState OrElse
                                                          TriggerSequenceState.WaitingForTrigger = triggerSequenceState)
            Me._AssertTriggerToolStripMenuItem.Enabled = TriggerSequenceState.WaitingForTrigger = triggerSequenceState
        End If

        Me._MeasureToolStripDropDownButton.Enabled = Me.IsDeviceOpen AndAlso TriggerSequenceState.Idle = triggerSequenceState
        If Me._TtmMeasureControlsToolStrip.Enabled Then
            Me._InitialResistanceToolStripMenuItem.Enabled = MeasurementSequenceState.Idle = measurementSequenceState
            Me._ThermalTransientToolStripMenuItem.Enabled = MeasurementSequenceState.Idle = measurementSequenceState
            Me._FinalResistanceToolStripMenuItem.Enabled = MeasurementSequenceState.Idle = measurementSequenceState
            Me._MeasureAllToolStripMenuItem.Enabled = MeasurementSequenceState.Idle = measurementSequenceState
            Me._AbortSequenceToolStripMenuItem.Enabled = MeasurementSequenceState.Idle <> measurementSequenceState
        End If

        Me._TraceToolStripDropDownButton.Enabled = Me.IsDeviceOpen AndAlso
                                             TriggerSequenceState.Idle = triggerSequenceState AndAlso
                                             MeasurementSequenceState.Idle = measurementSequenceState
        If Me._TraceToolStripDropDownButton.Enabled Then
            Me._ReadTraceToolStripMenuItem.Enabled = Me._isTraceAvailable
            Dim enabled As Boolean = Me._LastTimeSeries IsNot Nothing AndAlso Me._LastTimeSeries.Any
            Me._SaveTraceToolStripMenuItem.Enabled = enabled
            Me._ClearTraceToolStripMenuItem.Enabled = enabled
            Me._ModelTraceToolStripMenuItem.Enabled = enabled
        End If
    End Sub

#End Region

#Region " TRACE TOOL STRIP "

#Region " TRACE LIST VIEW "

    ''' <summary> Displays a part described by grid. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    Private Shared Sub DisplayTrace(ByVal grid As DataGridView, ByVal values As ObjectModel.ReadOnlyCollection(Of System.Drawing.PointF))

        If grid Is Nothing Then Throw New ArgumentNullException(NameOf(grid))

        grid.Enabled = False
        grid.Columns.Clear()
        grid.DataSource = New List(Of System.Drawing.PointF)
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSteelBlue
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        grid.RowHeadersVisible = False
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        If values IsNot Nothing Then
            grid.DataSource = values
        End If
        grid.Refresh()
        If grid.Columns IsNot Nothing AndAlso grid.Columns.Count > 0 Then
            For Each column As DataGridViewColumn In grid.Columns
                column.Visible = False
            Next
            Dim displayIndex As Integer = 0
            With grid.Columns("X")
                .Visible = True
                .HeaderText = " Time, ms "
                .DisplayIndex = displayIndex
                displayIndex += 1
            End With
            With grid.Columns("Y")
                .Visible = True
                .HeaderText = " Voltage, mV "
                .DisplayIndex = displayIndex
                displayIndex += 1
            End With
            grid.Enabled = True
        End If
    End Sub

#End Region

    Private _LastTimeSeries As ObjectModel.ReadOnlyCollection(Of System.Drawing.PointF)

    ''' <summary> Displays the thermal transient trace. </summary>
    ''' <param name="chart"> The chart. </param>
    ''' <returns> The trace values </returns>
    Protected Overridable Function DisplayThermalTransientTrace(ByVal chart As DataVisualization.Charting.Chart) As ObjectModel.ReadOnlyCollection(Of System.Drawing.PointF)
        Return New ObjectModel.ReadOnlyCollection(Of System.Drawing.PointF)(New System.Drawing.PointF() {})
    End Function

    ''' <summary> Event handler. Called by _ReadTraceToolStripMenuItem for click events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ReadTraceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles _ReadTraceToolStripMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "")
            Me._ErrorProvider.SetIconPadding(Me._TtmMeasureControlsToolStrip, -15)
            ' MeterThermalTransient.EmulateThermalTransientTrace(Me._Chart)
            Me._LastTimeSeries = Me.DisplayThermalTransientTrace(Me._Chart)
            ' list the trace values
            MeasurementPanelBase.displayTrace(Me._TraceDataGridView, Me._LastTimeSeries)
            Dim enabled As Boolean = Me._LastTimeSeries IsNot Nothing AndAlso Me._LastTimeSeries.Any
            Me._SaveTraceToolStripMenuItem.Enabled = enabled
            Me._ClearTraceToolStripMenuItem.Enabled = enabled
            Me._ModelTraceToolStripMenuItem.Enabled = enabled

        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "Failed reading and displaying the thermal transient trace.")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                       "Exception occurred reading and displaying the trace;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Clears the trace list. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ClearTraceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ClearTraceToolStripMenuItem.Click
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "User action @{0};. ",
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name)
        MeasurementPanelBase.displayTrace(Me._TraceDataGridView, Nothing)
    End Sub

    ''' <summary> Determines whether the specified folder path is writable. </summary>
    ''' <remarks> Uses a temporary random file name to test if the file can be created. The file is
    ''' deleted thereafter. </remarks>
    ''' <param name="path"> The path. </param>
    ''' <returns> <c>True</c> if the specified path is writable; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Shared Function IsFolderWritable(ByVal path As String) As Boolean
        Dim filePath As String = ""
        Dim affirmative As Boolean = False
        Try
            filePath = System.IO.Path.Combine(path, System.IO.Path.GetRandomFileName())
            Using s As System.IO.FileStream = System.IO.File.Open(filePath, System.IO.FileMode.OpenOrCreate)
            End Using
            affirmative = True
        Catch
        Finally
            ' SS reported an exception from this test possibly indicating that Windows allowed writing the file 
            ' by failed report deletion. Or else, Windows raised another exception type.
            Try
                If System.IO.File.Exists(filePath) Then
                    System.IO.File.Delete(filePath)
                End If
            Catch
            End Try
        End Try
        Return affirmative
    End Function

    ''' <summary> Sets the default file path in case the data folder does not exist. </summary>
    ''' <param name="filePath"> The file path. </param>
    ''' <returns> System.String. </returns>
    Private Shared Function UpdateDefaultFilePath(ByVal filePath As String) As String

        ' check validity of data folder.
        Dim dataFolder As String = System.IO.Path.GetDirectoryName(filePath)

        If Not System.IO.Directory.Exists(dataFolder) Then

            dataFolder = My.Application.Info.DirectoryPath

            If MeasurementPanelBase.IsFolderWritable(dataFolder) Then

                dataFolder = System.IO.Path.Combine(dataFolder, "..\Measurements")
                If System.IO.Directory.Exists(dataFolder) Then
                    dataFolder = System.IO.Path.GetDirectoryName(dataFolder)
                Else
                    dataFolder = System.IO.Directory.CreateDirectory(dataFolder).FullName
                End If

            Else

                ' in run time use the documents folder.
                dataFolder = My.Computer.FileSystem.SpecialDirectories.MyDocuments

                dataFolder = System.IO.Path.Combine(dataFolder, "TTM")
                If Not System.IO.Directory.Exists(dataFolder) Then
                    System.IO.Directory.CreateDirectory(dataFolder)
                End If

                dataFolder = System.IO.Path.Combine(dataFolder, "Measurements")
                If Not System.IO.Directory.Exists(dataFolder) Then
                    System.IO.Directory.CreateDirectory(dataFolder)
                End If

            End If

        End If

        Return System.IO.Path.Combine(dataFolder, System.IO.Path.GetFileName(filePath))

    End Function

    ''' <summary> Gets a new file for storing the data. </summary>
    ''' <param name="filePath"> The file path. </param>
    ''' <returns> A file name or empty if error. </returns>
    Private Shared Function BrowseForFile(ByVal filePath As String) As String

        ' make sure the default data file name is valid.
        filePath = updateDefaultFilePath(filePath)

        Using dialog As New SaveFileDialog()
            dialog.CheckFileExists = False
            dialog.CheckPathExists = True
            dialog.DefaultExt = ".xls"
            dialog.FileName = filePath
            dialog.Filter = "Comma Separated Values (*.csv)|*.csv|All Files (*.*)|*.*"
            dialog.FilterIndex = 0
            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(filePath)
            dialog.RestoreDirectory = True
            dialog.Title = "Select a Comma-Separated File"

            ' Open the Open dialog
            If dialog.ShowDialog = DialogResult.OK Then

                ' if file selected,
                Return dialog.FileName

            Else

                ' if some error, just ignore
                Return ""

            End If
        End Using

    End Function

    ''' <summary> Saves the trace to file. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _SaveTraceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SaveTraceToolStripMenuItem.Click

        Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "")
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "User action @{0};. ", System.Reflection.MethodInfo.GetCurrentMethod.Name)

        If Me._LastTimeSeries IsNot Nothing AndAlso Me._LastTimeSeries.Any Then

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Browsing for file;. ")
            Dim filePath As String = ""
            filePath = MeasurementPanelBase.browseForFile(isr.VI.Ttm.My.MySettings.Default.TraceFilePath)
            If String.IsNullOrWhiteSpace(filePath) Then
                Return
            End If
            My.MySettings.Default.TraceFilePath = filePath

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Saving trace;. to '{0}'", filePath)
            Try

                Using writer As New System.IO.StreamWriter(filePath, False)
                    Dim record As String = ""
                    For Each point As Drawing.PointF In Me._LastTimeSeries
                        If String.IsNullOrWhiteSpace(record) Then
                            record = "Trace time Series"
                            writer.WriteLine(record)
                            record = "Time,Voltage"
                            writer.WriteLine(record)
                            record = "ms,mV"
                            writer.WriteLine(record)
                        End If
                        record = String.Format(Globalization.CultureInfo.CurrentCulture, "{0},{1}", point.X, point.Y)
                        writer.WriteLine(record)
                    Next
                    writer.Flush()
                End Using
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Done saving;. ")

            Catch ex As Exception
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                           "Exception occurred saving trace;. to {0}. {1}", filePath, ex.ToFullBlownString)
                Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "Exception occurred saving trace")
            Finally
            End Try
        End If


    End Sub

    ''' <summary> Creates a model of the thermal transient trace. </summary>
    ''' <param name="chart"> The chart. </param>
    Protected Overridable Sub ModelThermalTransientTrace(ByVal chart As DataVisualization.Charting.Chart)
    End Sub

    ''' <summary> Event handler. Called by _ModelTraceToolStripMenuItem for click events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ModelTraceToolStripMenuItem_Click(sender As Object, e As System.EventArgs) Handles _ModelTraceToolStripMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "")
            Me._ErrorProvider.SetIconPadding(Me._TtmMeasureControlsToolStrip, -15)
            ' MeterThermalTransient.EmulateThermalTransientTrace(Me._Chart)
            Me.ModelThermalTransientTrace(Me._Chart)
        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "Failed modeling the thermal transient trace.")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                       "Exception occurred modeling the trace;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " TTM MEASURE TOOL STRIP "

    ''' <summary> Event handler. Called by  for  events. </summary>
    Protected Overridable Sub ClearPartMeasurements()
    End Sub

    ''' <summary> Event handler. Called by _ClearToolStripMenuItem for click events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _ClearToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles _ClearToolStripMenuItem.Click
        Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "")
        Me.ClearPartMeasurements()
    End Sub

    ''' <summary> Event handler. Called by  for  events. </summary>
    Protected Overridable Function MeasureInitialResistance() As MeasurementOutcomes
        Return MeasurementOutcomes.None
    End Function

    ''' <summary> Measures initial resistance. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitialResistanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _InitialResistanceToolStripMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "")
            Me._ErrorProvider.SetIconPadding(Me._TtmMeasureControlsToolStrip, -15)
            Me.MeasureInitialResistance()
            Dim outcome As MeasurementOutcomes = Me.MeasureInitialResistance
            If MeasurementOutcomes.None = outcome OrElse outcome = MeasurementOutcomes.PartPassed Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Initial Resistance measured;. ")
            Else
                If (outcome And MeasurementOutcomes.FailedContactCheck) <> 0 Then
                    Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "Failed contact check.")
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Contact check failed;. ")
                Else
                    Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "Failed initial resistance.")
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Initial Resistance failed with outcome = {0};. ", CInt(outcome))
                End If
            End If
        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "Failed Measuring Initial Resistance")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                       "Failed Measuring Initial Resistance;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by  for  events. </summary>
    Protected Overridable Function MeasureThermalTransient() As MeasurementOutcomes
        Return MeasurementOutcomes.None
    End Function

    ''' <summary> Measures the thermal transient voltage. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ThermalTransientToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ThermalTransientToolStripMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "")
            Me._ErrorProvider.SetIconPadding(Me._TtmMeasureControlsToolStrip, -15)
            Dim outcome As MeasurementOutcomes = Me.MeasureThermalTransient
            If MeasurementOutcomes.None = outcome OrElse outcome = MeasurementOutcomes.PartPassed Then
                Me._isTraceAvailable = True
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Thermal Transient measured;. ")
            Else
                Me._isTraceAvailable = False
                Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "Thermal transient failed.")
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Thermal transient failed with outcome = {0};. ", CInt(outcome))
            End If
            Me.OnStateChanged()
        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "Failed Measuring Thermal Transient")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                       "Failed Measuring Thermal Transient;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary> Event handler. Called by  for  events. </summary>
    Protected Overridable Function MeasureFinalResistance() As MeasurementOutcomes
        Return MeasurementOutcomes.None
    End Function

    ''' <summary> Measures the final resistance. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _FinalResistanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _FinalResistanceToolStripMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "")
            Me._ErrorProvider.SetIconPadding(Me._TtmMeasureControlsToolStrip, -15)
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Measuring Final Resistance...;. ")
            Dim outcome As MeasurementOutcomes = Me.MeasureFinalResistance
            If MeasurementOutcomes.None = outcome OrElse outcome = MeasurementOutcomes.PartPassed Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Final Resistance measured;. ")
            Else
                Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "Failed final resistance.")
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Final Resistance failed with outcome = {0};. ", CInt(outcome))
            End If
        Catch ex As Exception
            Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "Failed Measuring Final Resistance")
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                       "Failed Measuring Final Resistance;. {0}", ex.ToFullBlownString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " SEQUENCED MEASUREMENTS "

    Private WithEvents _MeasureSequencer As MeasureSequencer
    ''' <summary> Gets or sets the sequencer. </summary>
    ''' <value> The sequencer. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property MeasureSequencer As MeasureSequencer
        Get
            Return Me._MeasureSequencer
        End Get
        Set(value As MeasureSequencer)
            Me._MeasureSequencer = value
            Me._MeasureSequencer?.CaptureSyncContext(Threading.SynchronizationContext.Current)
        End Set
    End Property

    ''' <summary> Handles the measure sequencer property changed event. </summary>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As MeasureSequencer, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Ttm.MeasureSequencer.MeasurementSequenceState)
                Me.OnMeasurementSequenceStateChanged(sender.MeasurementSequenceState)
        End Select
    End Sub

    ''' <summary> Event handler. Called by _MeasureSequencer for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MeasureSequencer_PropertyChanged(ByVal sender As System.Object, ByVal e As PropertyChangedEventArgs) Handles _MeasureSequencer.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._MeasureSequencer_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, MeasureSequencer), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)

        End Try
    End Sub

    ''' <summary> Updates the progress bar. </summary>
    ''' <param name="state"> The state. </param>
    Private Sub UpdateProgressbar(ByVal state As MeasurementSequenceState)

        ' unhide the progress bar.
        Me._TtmToolStripProgressBar.Visible = True

        If state = MeasurementSequenceState.Failed OrElse
            state = MeasurementSequenceState.Starting OrElse
            state = MeasurementSequenceState.Idle Then
            Me._TtmToolStripProgressBar.Value = Me._TtmToolStripProgressBar.Minimum
            Me._TtmToolStripProgressBar.ToolTipText = "Failed"
        ElseIf state = MeasurementSequenceState.Completed Then
            Me._TtmToolStripProgressBar.Value = Me._TtmToolStripProgressBar.Maximum
            Me._TtmToolStripProgressBar.ToolTipText = "Completed"
        Else
            Me._TtmToolStripProgressBar.Value = Me.MeasureSequencer.PercentProgress
        End If

    End Sub

    ''' <summary> Handles the change in measurement state. </summary>
    ''' <param name="state"> The state. </param>
    Private Sub OnMeasurementSequenceStateChanged(ByVal state As MeasurementSequenceState)
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "Processing the {0} state;. ", state.Description)
        Me.updateProgressbar(state)
        Select Case state
            Case MeasurementSequenceState.Aborted
            Case MeasurementSequenceState.Completed
            Case MeasurementSequenceState.Failed
            Case MeasurementSequenceState.MeasureInitialResistance
            Case MeasurementSequenceState.MeasureThermalTransient
            Case MeasurementSequenceState.Idle
                Me.OnStateChanged()
            Case MeasurementSequenceState.None
            Case MeasurementSequenceState.PostTransientPause
                Me._isTraceAvailable = True
            Case MeasurementSequenceState.MeasureFinalResistance
            Case MeasurementSequenceState.Starting
                Me._isTraceAvailable = False
                Me.OnStateChanged()
            Case Else
                Debug.Assert(Not Debugger.IsAttached, "Unhandled state: " & state.ToString)
        End Select
    End Sub

    ''' <summary> Aborts the measurement sequence. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _AbortSequenceToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _AbortSequenceToolStripMenuItem.Click
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "User action @{0};. ", System.Reflection.MethodInfo.GetCurrentMethod.Name)
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Abort requested;. ")
        Me.MeasureSequencer.Enqueue(MeasurementSequenceSignal.Abort)
    End Sub

    ''' <summary> Starts measurement sequence. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges")>
    Private Sub _MeasureAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MeasureAllToolStripMenuItem.Click
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "User action @{0};. ", System.Reflection.MethodInfo.GetCurrentMethod.Name)
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Start requested;. ")
        Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "")
        Me._ErrorProvider.SetIconPadding(Me._TtmMeasureControlsToolStrip, -15)
        Me.MeasureSequencer.StartMeasurementSequence()
    End Sub

#End Region

#Region " TRIGGERED MEASUREMENTS "

    ''' <summary> Abort triggered measurement. </summary>
    Protected Overridable Sub AbortTriggerSequenceIf()
    End Sub

    ''' <summary> Event handler. Called by _AbortToolStripMenuItem for click events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _AbortToolStripMenuItem_Click(sender As Object, e As System.EventArgs) Handles _AbortToolStripMenuItem.Click
        Me._ErrorProvider.SetError(Me._TtmMeasureControlsToolStrip, "")
        Me.AbortTriggerSequenceIf()
    End Sub

    Private WithEvents _TriggerSequencer As TriggerSequencer
    ''' <summary> Gets or sets the trigger sequencer. </summary>
    ''' <value> The sequencer. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property TriggerSequencer As TriggerSequencer
        Get
            Return Me._TriggerSequencer
        End Get
        Set(value As TriggerSequencer)
            Me._TriggerSequencer = value
            Me._TriggerSequencer?.CaptureSyncContext(Threading.SynchronizationContext.Current)
        End Set
    End Property

    ''' <summary> Handles the trigger sequencer property changed event. </summary>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As TriggerSequencer, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Ttm.TriggerSequencer.TriggerSequenceState)
                Me.OnTriggerSequenceStateChanged(sender.TriggerSequenceState)
        End Select
    End Sub

    ''' <summary> Event handler. Called by _TriggerSequencer for property changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _TriggerSequencer_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _TriggerSequencer.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._TriggerSequencer_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, TriggerSequencer), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

    Dim lastStatusBar As String
    ''' <summary> Handles the change in measurement state. </summary>
    ''' <param name="state"> The state. </param>
    Private Sub OnTriggerSequenceStateChanged(ByVal state As TriggerSequenceState)

        Me._TriggerActionToolStripLabel.Text = Me.TriggerSequencer.ProgressMessage(lastStatusBar)
        Select Case state
            Case TriggerSequenceState.Aborted
                Me.OnStateChanged()
            Case TriggerSequenceState.Stopped
            Case TriggerSequenceState.Failed
            Case TriggerSequenceState.WaitingForTrigger
            Case TriggerSequenceState.MeasurementCompleted
                Me._isTraceAvailable = True
            Case TriggerSequenceState.ReadingValues
            Case TriggerSequenceState.Idle
                Me.OnStateChanged()
                Me._WaitForTriggerToolStripMenuItem.Image = Global.isr.Vi.Ttm.My.Resources.Resources.ViewRefresh7
            Case TriggerSequenceState.None
            Case TriggerSequenceState.Starting
                Me._isTraceAvailable = False
                Me.OnStateChanged()
                Me._AssertTriggerToolStripMenuItem.Enabled = True
                Me._WaitForTriggerToolStripMenuItem.Image = Global.isr.Vi.Ttm.My.Resources.Resources.MediaPlaybackStop2
            Case Else
                Debug.Assert(Not Debugger.IsAttached, "Unhandled state: " & state.ToString)
        End Select
    End Sub

    ''' <summary> Event handler. Called by  for  events. </summary>
    Private Sub AssertTrigger()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Assert requested;. ")
        Me.TriggerSequencer.AssertTrigger()
    End Sub

    ''' <summary> Asserts a trigger to emulate triggering for timing measurements. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _AssertTriggerToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _AssertTriggerToolStripMenuItem.Click
        Me.AssertTrigger()
    End Sub

    ''' <summary> Event handler. Called by _WaitForTriggerToolStripMenuItem for click events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _WaitForTriggerToolStripMenuItem_CheckChanged(sender As Object, e As System.EventArgs) Handles _WaitForTriggerToolStripMenuItem.CheckedChanged
        If _WaitForTriggerToolStripMenuItem.Enabled Then
            If Me._TriggerSequencer.TriggerSequenceState = TriggerSequenceState.Idle Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Starting triggered measurements;. ")
                Me._TriggerSequencer.StartMeasurementSequence()
            ElseIf Me._TriggerSequencer.TriggerSequenceState = TriggerSequenceState.WaitingForTrigger Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Stopping triggered measurements;. ")
                Me._TriggerSequencer.Enqueue(TriggerSequenceSignal.Stop)
            End If
        End If
    End Sub

#End Region

#Region " TRACE LEVEL "

    Private _MasterDevice As VI.Tsp.K2600.Device
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property MasterDevice As VI.Tsp.K2600.Device
        Get
            Return Me._MasterDevice
        End Get
        Set(value As VI.Tsp.K2600.Device)
            Me._MasterDevice = value
            If value Is Nothing Then
                Me.DisableTraceLevelControls()
            Else
                Me.EnableTraceLevelControls()
            End If
        End Set
    End Property

    ''' <summary> Disables the trace level controls. </summary>
    Private Sub DisableTraceLevelControls()
        RemoveHandler Me._LogTraceLevelComboBox.ComboBox.SelectedValueChanged, AddressOf Me._LogTraceLevelComboBox_SelectedValueChanged
        RemoveHandler Me._DisplayTraceLevelComboBox.ComboBox.SelectedValueChanged, AddressOf Me._DisplayTraceLevelComboBox_SelectedValueChanged
    End Sub


    ''' <summary> Enables the trace level controls. </summary>
    Private Sub EnableTraceLevelControls()

        TalkerControlBase.ListTraceEventLevels(Me._LogTraceLevelComboBox.ComboBox)
        AddHandler Me._LogTraceLevelComboBox.ComboBox.SelectedValueChanged, AddressOf Me._LogTraceLevelComboBox_SelectedValueChanged

        TalkerControlBase.ListTraceEventLevels(Me._DisplayTraceLevelComboBox.ComboBox)
        AddHandler Me._DisplayTraceLevelComboBox.ComboBox.SelectedValueChanged, AddressOf Me._DisplayTraceLevelComboBox_SelectedValueChanged

        TalkerControlBase.SelectItem(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel)
        TalkerControlBase.SelectItem(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel)

    End Sub

    ''' <summary>
    ''' Event handler. Called by _LogTraceLevelComboBox for selected value changed events.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _LogTraceLevelComboBox_SelectedValueChanged(sender As Object, e As EventArgs)
        Dim activity As String = "selecting log trace level on this instrument only"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            Me.MasterDevice.ApplyTalkerTraceLevel(ListenerType.Logger,
                                            TalkerControlBase.SelectedValue(Me._LogTraceLevelComboBox, My.Settings.TraceLogLevel))
        Catch ex As Exception
            Me._ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.MasterDevice.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Event handler. Called by _DisplayTraceLevelComboBox for selected value changed events.
    ''' </summary>
    ''' <param name="sender"> <see cref="System.Object"/> instance of this
    '''                       <see cref="System.Windows.Forms.Control"/> </param>
    ''' <param name="e">      Event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DisplayTraceLevelComboBox_SelectedValueChanged(sender As Object, e As EventArgs)
        Dim activity As String = "selecting Display trace level on this instrument only"
        Try
            Me.Cursor = Cursors.WaitCursor
            Me._ErrorProvider.Clear()
            Me.MasterDevice.ApplyTalkerTraceLevel(ListenerType.Display,
                                            TalkerControlBase.SelectedValue(Me._DisplayTraceLevelComboBox, My.Settings.TraceShowLevel))
        Catch ex As Exception
            Me._ErrorProvider.Annunciate(sender, ex.Message)
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"{Me.MasterDevice.ResourceTitle} exception {activity};. {ex.ToFullBlownString}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

#End Region

End Class
