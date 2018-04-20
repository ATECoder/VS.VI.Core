Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
''' <summary> Panel for editing the parts. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="4/12/2014" by="David" revision=""> Created. </history>
Public Class PartsPanel
    Inherits TalkerControlBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> This constructor is public to allow using this as a startup form for the project. </summary>
    Public Sub New()
        MyBase.New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.OnPartOutcomeChanged()
        Me._Parts = New DeviceUnderTestCollection
        Me.ConfigurePartsDisplay(Me._PartsDataGridView)
        Me.OnPartsChanged(Me._Parts, System.EventArgs.Empty)

#If designMode Then
        Debug.Assert(False, "Illegal call; reset design mode")
#End If
    End Sub

    ''' <summary> Release resources. </summary>
    Private Sub ReleaseResources()

        Me._Part = Nothing
        If Me.Parts IsNot Nothing Then
            Me._Parts.Clear()
            Me._Parts = Nothing
        End If

    End Sub

#End Region

#Region " DEVICE UNDER TEST "

    ''' <summary> The Part. </summary>
    Private WithEvents _Part As DeviceUnderTest

    ''' <summary> Gets or sets the part. </summary>
    ''' <value> The part. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Part As DeviceUnderTest
        Get
            Return Me._Part
        End Get
        Set(ByVal value As DeviceUnderTest)
            Me._Part = value
        End Set
    End Property

    ''' <summary> Update outcome display and enable adding part. </summary>
    Private Sub OnPartOutcomeChanged()
        If Me.Part Is Nothing Then
            Me.OutcomeSetter(MeasurementOutcomes.None)
            Me._ClearMeasurementsToolStripMenuItem.Enabled = False
            Me._AddPartToolStripButton.Enabled = False
        Else
            Me.OutcomeSetter(Me.Part.Outcome)
            Me._AddPartToolStripButton.Enabled = Me.Part.AllMeasurementsAvailable AndAlso
                                        ((Me.Part.Outcome And MeasurementOutcomes.FailedContactCheck) = 0) AndAlso
                                        ((Me.Part.Outcome And MeasurementOutcomes.MeasurementFailed) = 0) AndAlso
                                        ((Me.Part.Outcome And MeasurementOutcomes.MeasurementNotMade) = 0)
            Me._ClearMeasurementsToolStripMenuItem.Enabled = Me.Part.AnyMeasurementsAvailable
        End If
    End Sub

    ''' <summary> Executes the part property changed action. </summary>
    ''' <param name="sender">       Specifies the object where the call originated. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPartPropertyChanged(ByVal sender As DeviceUnderTest, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(Ttm.DeviceUnderTest.AnyMeasurementsAvailable)
                Me.OnPartOutcomeChanged()
            Case NameOf(Ttm.DeviceUnderTest.AllMeasurementsAvailable)
                Me.OnPartOutcomeChanged()
            Case NameOf(Ttm.DeviceUnderTest.LotId)
                Me._LotToolStripTextBox.Text = Me.Part.LotId
            Case NameOf(Ttm.DeviceUnderTest.Outcome)
                Me.OnPartOutcomeChanged()
            Case NameOf(Ttm.DeviceUnderTest.PartNumber)
                Me._PartNumberToolStripTextBox.Text = Part.PartNumber.ToString
            Case NameOf(Ttm.DeviceUnderTest.OperatorId)
                Me._OperatorToolStripTextBox.Text = Me.Part.OperatorId
            Case NameOf(Ttm.DeviceUnderTest.SerialNumber)
                Me._SerialNumberToolStripTextBox.Text = Me.Part.SerialNumber.ToString
        End Select
    End Sub

    ''' <summary> Event handler. Called by _Part for property changed events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Part_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _Part.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf _Part_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPartPropertyChanged(TryCast(sender, DeviceUnderTest), e?.PropertyName)
            End If
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                              "Exception handling property '{0}'. {1}.", e.PropertyName, ex.ToFullBlownString)

        End Try
    End Sub

    Private _MeasurementMessage As String


    ''' <summary> Gets or sets a message describing the measurement. </summary>
    ''' <value> A message describing the measurement. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property MeasurementMessage() As String
        Get
            Return Me._measurementMessage
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then
                value = ""
            ElseIf value <> Me.MeasurementMessage Then
                Me._OutcomeToolStripLabel.Text = value
            End If
            Me._measurementMessage = value
        End Set
    End Property

    ''' <summary> Gets or sets the test <see cref="MeasurementOutcomes">outcome</see>. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub OutcomeSetter(ByVal value As MeasurementOutcomes)

        If value = MeasurementOutcomes.None Then

            Me.MeasurementMessage = ""
            Me._PassFailToolStripButton.Visible = False

        ElseIf (value And MeasurementOutcomes.MeasurementFailed) = 0 Then

            Me.MeasurementMessage = "OKAY"

        Else

            If (value And MeasurementOutcomes.FailedContactCheck) <> 0 Then
                Me.MeasurementMessage = "CONTACTS"
            ElseIf (value And MeasurementOutcomes.HitCompliance) <> 0 Then
                Me.MeasurementMessage = "COMPLIANCE"
            ElseIf (value And MeasurementOutcomes.UnexpectedReadingFormat) <> 0 Then
                Me.MeasurementMessage = "READING FORMAT"
            ElseIf (value And MeasurementOutcomes.UnexpectedOutcomeFormat) <> 0 Then
                Me.MeasurementMessage = "OUTCOME FORMAT"
            ElseIf (value And MeasurementOutcomes.UnspecifiedStatusException) <> 0 Then
                Me.MeasurementMessage = "DEVICE"
            ElseIf (value And MeasurementOutcomes.UnspecifiedProgramFailure) <> 0 Then
                Me.MeasurementMessage = "PROGRAM"
            ElseIf (value And MeasurementOutcomes.MeasurementNotMade) <> 0 Then
                Me.MeasurementMessage = "PARTIAL"
            Else
                Me.MeasurementMessage = "FAILED"
                If (value And MeasurementOutcomes.UnknownOutcome) <> 0 Then
                    Debug.Assert(Not Debugger.IsAttached, "Unknown outcome")
                End If
            End If

        End If

        If (value And MeasurementOutcomes.PartPassed) <> 0 Then

            Me._PassFailToolStripButton.Visible = True
            Me._PassFailToolStripButton.Image = Global.isr.Vi.Ttm.My.Resources.Resources.Good

        ElseIf (value And MeasurementOutcomes.PartFailed) <> 0 Then

            Me._PassFailToolStripButton.Visible = True
            Me._PassFailToolStripButton.Image = Global.isr.Vi.Ttm.My.Resources.Resources.Bad

        End If

    End Sub

#Region " SETTINGS AND BINDING "

    ''' <summary> Copy panel values to the settings store. </summary>
    Public Sub CopySettings()

        My.Settings.AutomaticallyAddPart = Me._AutoAddToolStripMenuItem.Checked

        Dim value As String = Me._PartNumberToolStripTextBox.Text.Trim
        If Not String.IsNullOrWhiteSpace(value) Then
            My.Settings.PartNumber = value
        End If
        value = Me._LotToolStripTextBox.Text.Trim
        If Not String.IsNullOrWhiteSpace(value) Then
            My.Settings.LotNumber = value
        End If

        value = Me._OperatorToolStripTextBox.Text.Trim
        If Not String.IsNullOrWhiteSpace(value) Then
            My.Settings.OperatorId = value
        End If

    End Sub

    ''' <summary> Applies settings to the controls. </summary>
    Public Sub ApplySettings()

        Me._AutoAddToolStripMenuItem.Checked = My.Settings.AutomaticallyAddPart

        Me.Part.PartNumber = My.Settings.PartNumber
        With Me._PartNumberToolStripTextBox
            .Text = My.Settings.PartNumber
        End With

        Me.Part.LotId = My.Settings.LotNumber
        With Me._LotToolStripTextBox
            .Text = My.Settings.LotNumber
        End With

        Me.Part.OperatorId = My.Settings.OperatorId
        With Me._OperatorToolStripTextBox
            .Text = My.Settings.OperatorId
        End With

        With Me._SerialNumberToolStripTextBox
            .Text = Me.Part.SerialNumber.ToString
        End With

    End Sub

#End Region

#End Region

#Region " PARTS LIST MANAGEMENT "

    Private WithEvents _Parts As DeviceUnderTestCollection
    ''' <summary> Gets or sets the parts. </summary>
    Public Function Parts() As DeviceUnderTestCollection
        Return _Parts
    End Function

    ''' <summary> Event handler. Called by  for  events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information to send to registered event handlers. </param>
    Protected Sub OnPartsChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Parts.CollectionChanged
        ' Me.ConfigurePartsDisplay(Me._PartsDataGridView)
        Me._PartsBindingSource.ResetBindings(False)
        Me._AddPartToolStripButton.Enabled = Me._AddPartToolStripButton.Enabled AndAlso Me.Parts IsNot Nothing AndAlso Me.Parts.Any
        Me._ClearPartListToolStripButton.Enabled = Me.Parts IsNot Nothing AndAlso Me.Parts.Any
        Me._SavePartsToolStripButton.Enabled = Me.Parts IsNot Nothing AndAlso Me.Parts.Any
    End Sub

    ''' <summary> Clears the list of measurements. </summary>
    Public Sub ClearParts()

        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Clearing parts;. ")
        If Me.Parts Is Nothing Then
            Me._Parts = New DeviceUnderTestCollection
        End If
        Me.Parts.Clear()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Part list cleared;. ")

    End Sub

    ''' <summary> Gets the automatic add enabled. </summary>
    ''' <value> The automatic add enabled. </value>
    Public ReadOnly Property AutoAddEnabled As Boolean
        Get
            Return Me._AddPartToolStripButton.Enabled AndAlso
                   Me._AutoAddToolStripMenuItem.Checked
        End Get
    End Property

    ''' <summary> Adds the current part to the list of measurements. </summary>
    Public Sub AddPart()

        If Me.Part.AllMeasurementsMade Then

            If String.IsNullOrEmpty(Me._PartNumberToolStripTextBox.Text.Trim) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                           "Enter part number;. ")
            ElseIf String.IsNullOrEmpty(Me._LotToolStripTextBox.Text.Trim) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                           "Enter lot number;. ")
            ElseIf String.IsNullOrEmpty(Me._OperatorToolStripTextBox.Text.Trim) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                           "Enter operator id;. ")
            Else

                Me.CopySettings()

                Me.Part.PartNumber = Me._PartNumberToolStripTextBox.Text.Trim
                Me.Part.LotId = Me._LotToolStripTextBox.Text.Trim
                Me.Part.OperatorId = Me._OperatorToolStripTextBox.Text.Trim
                If Not Integer.TryParse(Me._SerialNumberToolStripTextBox.Text.Trim, Me.Part.SerialNumber) Then
                    Me.Part.SerialNumber += 1
                End If

                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Setting part sample number to '{0}';. ", Me.Parts.Count + 1)
                Me.Part.SampleNumber = Me.Parts.Count + 1

                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Adding part '{0}';. ", Me.Part.UniqueKey)
                Me.Parts.Add(New DeviceUnderTest(Me.Part))

                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Part '{0}' added;. ", Me.Part.UniqueKey)

            End If

        Else

            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Part has incomplete data;. Outcome: {0)", Me.Part.Outcome.ToString)

        End If

    End Sub

    ''' <summary> Sets the default file path in case the data folder does not exist. </summary>
    ''' <param name="filePath"> The file path. </param>
    ''' <returns> System.String. </returns>
    Private Shared Function UpdateDefaultFilePath(ByVal filePath As String) As String

        ' check validity of data folder.
        Dim dataFolder As String = System.IO.Path.GetDirectoryName(filePath)

        If Not System.IO.Directory.Exists(dataFolder) Then

            If Debugger.IsAttached Then

                ' in design mode use the application folder.
                dataFolder = My.Application.Info.DirectoryPath

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

    ''' <summary> Gets the save enabled. </summary>
    ''' <value> The save enabled. </value>
    Public ReadOnly Property SaveEnabled As Boolean
        Get
            Return Me._SavePartsToolStripButton.Enabled
        End Get
    End Property

    ''' <summary> Saves data for all parts. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub SaveParts()

        If Me.Parts IsNot Nothing AndAlso Me.Parts.Any Then

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Browsing for file;. ")
            Dim filePath As String = ""
            filePath = PartsPanel.browseForFile(My.Settings.PartsFilePath)
            If String.IsNullOrWhiteSpace(filePath) Then
                Return
            End If
            My.Settings.PartsFilePath = filePath

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Saving...;. data to '{0}'", filePath)
            Try

                Using writer As New System.IO.StreamWriter(filePath, False)
                    For Each part As DeviceUnderTest In Me.Parts
                        If part.SampleNumber = 1 Then
                            writer.WriteLine("""TTM measurements""")
                            writer.WriteLine("""Part number"",""{0}""", part.PartNumber)
                            writer.WriteLine("""Lot number"",""{0}""", part.LotId)
                            writer.WriteLine("""Operator Id"",""{0}""", part.OperatorId)
                            writer.WriteLine(DeviceUnderTest.DataHeader)
                        End If
                        writer.WriteLine(part.DataRecord)
                    Next
                    writer.Flush()
                End Using
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Done saving part number {0};. ", Part.PartNumber)
                Me._SavePartsToolStripButton.Enabled = False
            Catch ex As Exception
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, "Exception occurred saving;. data to {0}. {1}", filePath, ex.ToFullBlownString)
                Me._ErrorProvider.SetError(Me._PartsListToolStrip, "Exception occurred saving")
            Finally

            End Try

        End If

    End Sub

    ''' <summary> Event handler. Called by _AddPartToolStripButton for click events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _AddPartToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _AddPartToolStripButton.Click
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "User action @{0};. ", System.Reflection.MethodInfo.GetCurrentMethod.Name)
        Me._ErrorProvider.SetError(Me._PartsListToolStrip, "")
        Me.AddPart()
    End Sub

    ''' <summary> Event handler. Called by _CLearPartListToolStripButton for click events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _CLearPartListToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ClearPartListToolStripButton.Click
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "User action @{0};. ", System.Reflection.MethodInfo.GetCurrentMethod.Name)
        Me._ErrorProvider.SetError(Me._PartsListToolStrip, "")
        Me.ClearParts()
    End Sub

    ''' <summary> Event handler. Called by _SavePartsToolStripButton for click events.3 </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _SavePartsToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SavePartsToolStripButton.Click
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "User action @{0};. ", System.Reflection.MethodInfo.GetCurrentMethod.Name)
        Me._ErrorProvider.SetError(Me._PartsListToolStrip, "")
        Me.SaveParts()
    End Sub

#End Region

#Region " PARTS DISPLAY "

    ''' <summary> Event handler. Called by _PartsDataGridView for data error events. </summary>
    ''' <param name="sender"> Specifies the object where the call originated. </param>
    ''' <param name="e">      Data grid view data error event information. </param>
    Private Sub _PartsDataGridView_DataError(sender As Object, e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles _PartsDataGridView.DataError
        If e IsNot Nothing Then
            e.Cancel = False
        End If
    End Sub

    ''' <summary> The binder. </summary>
    Private _PartsBindingSource As BindingSource

    ''' <summary> Configures parts display. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="grid"> The grid. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub ConfigurePartsDisplay(ByVal grid As DataGridView)

        If grid Is Nothing Then Throw New ArgumentNullException(NameOf(grid))

        Me._PartsBindingSource = New BindingSource() With {.DataSource = Me.Parts}

        grid.Enabled = False
        grid.Columns.Clear()
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSalmon
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        grid.AllowUserToAddRows = False
        grid.RowHeadersVisible = False
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        grid.AutoGenerateColumns = False
        grid.ReadOnly = True
        grid.DataSource = Me._PartsBindingSource
        grid.Refresh()
        Dim displayIndex As Integer = 0
        Dim column As DataGridViewTextBoxColumn
        column = New DataGridViewTextBoxColumn()
        grid.Columns.Add(column)
        With column
            .DataPropertyName = "SampleNumber"
            .Name = "SampleNumber"
            .Visible = True
            .HeaderText = " # "
            .DisplayIndex = displayIndex
            displayIndex += 1
            .Width = 30
        End With
        column = New DataGridViewTextBoxColumn()
        grid.Columns.Add(column)
        With column
            .DataPropertyName = "SerialNumber"
            .Name = "SerialNumber"
            .Visible = True
            .HeaderText = " S/N "
            .DisplayIndex = displayIndex
            displayIndex += 1
        End With
        column = New DataGridViewTextBoxColumn()
        grid.Columns.Add(column)
        With column
            .DataPropertyName = "Timestamp"
            .Name = "Timestamp"
            .Visible = True
            .HeaderText = " Time "
            .DisplayIndex = displayIndex
            displayIndex += 1
        End With
        column = New DataGridViewTextBoxColumn()
        grid.Columns.Add(column)
        With column
            .DataPropertyName = "InitialResistanceCaption"
            .Name = "InitialResistanceCaption"
            .Visible = True
            .HeaderText = " Initial "
            .DisplayIndex = displayIndex
            displayIndex += 1
        End With
        column = New DataGridViewTextBoxColumn()
        grid.Columns.Add(column)
        With column
            .DataPropertyName = "FinalResistanceCaption"
            .Name = "FinalResistanceCaption"
            .Visible = True
            .HeaderText = " Final "
            .DisplayIndex = displayIndex
            displayIndex += 1
        End With
        column = New DataGridViewTextBoxColumn()
        grid.Columns.Add(column)
        With column
            .DataPropertyName = "ThermalTransientVoltageCaption"
            .Name = "ThermalTransientVoltageCaption"
            .Visible = True
            .HeaderText = " Transient "
            .DisplayIndex = displayIndex
            displayIndex += 1
        End With
        grid.Enabled = True

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
