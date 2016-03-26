Imports System.Windows.Forms
Imports System.ComponentModel
Imports isr.Core.Pith
''' <summary> Measurements header. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="2/25/2014" by="David" revision=""> Created. </history>
Public Class MeasurementsHeader
    Inherits MyUserControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> This constructor is public to allow using this as a startup form for the project. </summary>
    Public Sub New()
        MyBase.New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me._OutcomeTextBox.Text = ""

    End Sub

    ''' <summary> Releases the resources. </summary>
    Private Sub releaseResources()
        Me._InitialResistance = Nothing
        Me._FinalResistance = Nothing
        Me._ThermalTransient = Nothing
        Me._DeviceUnderTest = Nothing
    End Sub

#End Region

#Region " DUT "

    Private WithEvents _DeviceUnderTest As DeviceUnderTest

    ''' <summary> Gets or sets the device under test. </summary>
    ''' <value> The device under test. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property DeviceUnderTest As DeviceUnderTest
        Get
            Return Me._DeviceUnderTest
        End Get
        Set(value As DeviceUnderTest)
            Me._DeviceUnderTest = value
            If value Is Nothing Then
                Me._InitialResistance = Nothing
                Me._FinalResistance = Nothing
                Me._ThermalTransient = Nothing
            Else
                Me._InitialResistance = value.InitialResistance
                Me._FinalResistance = value.FinalResistance
                Me._ThermalTransient = value.ThermalTransient
            End If
        End Set
    End Property

    ''' <summary> Handles the device under test property changed event. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As DeviceUnderTest, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.Outcome)
                Me.OutcomeSetter(sender.Outcome)
        End Select
    End Sub

    ''' <summary> Event handler. Called by _DeviceUnderTest for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DeviceUnderTest_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _DeviceUnderTest.PropertyChanged
        Try
            Me.OnPropertyChanged(TryCast(sender, DeviceUnderTest), e?.PropertyName)
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling property '{0}'. Details: {1}.",
                         e.PropertyName, ex.Message)
        End Try
    End Sub

    Private _measurementMessage As String

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
                Me._OutcomeTextBox.Text = value
            End If
            Me._measurementMessage = value
        End Set
    End Property

    ''' <summary> Gets or sets the test <see cref="MeasurementOutcomes">outcome</see>. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub OutcomeSetter(ByVal value As MeasurementOutcomes)

        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of MeasurementOutcomes)(AddressOf OutcomeSetter), New Object() {value})
            Return
        End If
        If value = MeasurementOutcomes.None Then

            Me.MeasurementMessage = ""
            Me._OutcomePictureBox.Visible = False

        ElseIf (value And MeasurementOutcomes.MeasurementFailed) = 0 Then

            Me.MeasurementMessage = "OKAY"

        Else

            If (value And MeasurementOutcomes.FailedContactCheck) <> 0 Then
                Me.MeasurementMessage = "CONTACTS"
            ElseIf (value And MeasurementOutcomes.HitCompliance) <> 0 Then
                Me.MeasurementMessage = "COMPLIANCE"
            ElseIf (value And MeasurementOutcomes.UnexpectedReadingFormat) <> 0 Then
                Me.MeasurementMessage = "READING ?"
            ElseIf (value And MeasurementOutcomes.UnexpectedOutcomeFormat) <> 0 Then
                Me.MeasurementMessage = "OUTCOME ?"
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

            Me._OutcomePictureBox.Visible = True
            Me._OutcomePictureBox.Image = Global.isr.Vi.Ttm.My.Resources.Resources.Good

        ElseIf (value And MeasurementOutcomes.PartFailed) <> 0 Then

            Me._OutcomePictureBox.Visible = True
            Me._OutcomePictureBox.Image = Global.isr.Vi.Ttm.My.Resources.Resources.Bad

        End If

    End Sub

#End Region

#Region " DISPLAY ALERT "

    ''' <summary> Shows the alerts. </summary>
    ''' <param name="show"> true to show, false to hide. </param>
    Public Sub ShowAlerts(ByVal show As Boolean, ByVal isGood As Boolean)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of Boolean, Boolean)(AddressOf ShowAlerts), New Object() {show, isGood})
        Else
            If show Then
                If isGood Then
                    Me._AlertsPictureBox.Image = My.Resources.Good
                Else
                    Me._AlertsPictureBox.Image = My.Resources.Bad
                End If
            Else
                Me._AlertsPictureBox.Image = Nothing
            End If
        End If
    End Sub

    ''' <summary> Shows the outcome. </summary>
    ''' <param name="show">   true to show, false to hide. </param>
    ''' <param name="isGood"> true if this object is good. </param>
    Public Sub ShowOutcome(ByVal show As Boolean, ByVal isGood As Boolean)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of Boolean, Boolean)(AddressOf ShowOutcome), New Object() {show, isGood})
        Else
            If show Then
                If isGood Then
                    Me._OutcomePictureBox.Image = My.Resources.Good
                Else
                    Me._OutcomePictureBox.Image = My.Resources.Bad
                End If
            Else
                Me._OutcomePictureBox.Image = Nothing
            End If
        End If
    End Sub

#End Region

#Region " DISPLAY VALUE "

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Sub Clear()
        If Me.InvokeRequired Then
            Me.Invoke(New Action(AddressOf Clear))
        Else
            Me._ErrorProvider.Clear()
            Me.ShowOutcome(False, False)
            Me.ShowAlerts(False, False)
            Me._InitialResistanceTextBox.Text = ""
            Me._FinalResistanceTextBox.Text = ""
            Me._ThermalTransientVoltageTextBox.Text = ""
        End If
    End Sub

    ''' <summary> Displays the thermal transient. </summary>
    Private Sub setErrorProvider(ByVal textBox As TextBox, ByVal resistance As ResistanceMeasureBase)
        If (resistance.Outcome And MeasurementOutcomes.PartFailed) <> 0 Then
            Me._ErrorProvider.SetIconPadding(textBox, -Me._ErrorProvider.Icon.Width)
            Me._ErrorProvider.SetError(textBox, "Value out of range")
        ElseIf (resistance.Outcome And MeasurementOutcomes.MeasurementFailed) <> 0 Then
            Me._ErrorProvider.SetIconPadding(textBox, -Me._ErrorProvider.Icon.Width)
            Me._ErrorProvider.SetError(textBox, "Measurement failed")
        ElseIf (resistance.Outcome And MeasurementOutcomes.MeasurementNotMade) <> 0 Then
            Me._ErrorProvider.SetIconPadding(textBox, -Me._ErrorProvider.Icon.Width)
            Me._ErrorProvider.SetError(textBox, "Measurement not made")
        Else
            Me._ErrorProvider.SetError(textBox, "")
        End If
    End Sub

    ''' <summary> Displays the resistance. </summary>
    Private Sub clearResistance(ByVal textBox As TextBox)
        If textBox IsNot Nothing Then
            If textBox.InvokeRequired Then
                textBox.Invoke(New Action(Of TextBox)(AddressOf clearResistance), textBox)
            Else
                textBox.Text = ""
                Me._ErrorProvider.SetError(textBox, "")
            End If
        End If
    End Sub

    ''' <summary> Displays the resistance. </summary>
    Private Sub showResistance(ByVal textBox As TextBox, ByVal resistance As ResistanceMeasureBase)
        If textBox IsNot Nothing Then
            If textBox.InvokeRequired Then
                textBox.Invoke(New Action(Of TextBox, ResistanceMeasureBase)(AddressOf showResistance), New Object() {textBox, resistance})
            Else
                textBox.Text = resistance.ResistanceCaption
                Me.setErrorProvider(textBox, resistance)
            End If
        End If
    End Sub

    ''' <summary> Displays the thermal transient. </summary>
    Private Sub showThermalTransient(ByVal textBox As TextBox, ByVal resistance As ResistanceMeasureBase)
        If textBox IsNot Nothing Then
            If textBox.InvokeRequired Then
                textBox.Invoke(New Action(Of TextBox, ResistanceMeasureBase)(AddressOf showThermalTransient), New Object() {textBox, resistance})
            Else
                textBox.Text = resistance.VoltageCaption
                Me.setErrorProvider(textBox, resistance)
            End If
        End If
    End Sub

#End Region

#Region " PART: INITIAL RESISTANCE "

    ''' <summary> The Part Initial Resistance. </summary>
    Private WithEvents _InitialResistance As ColdResistance

    ''' <summary> Executes the initial resistance property changed action. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnInitialResistancePropertyChanged(ByVal sender As ColdResistance, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.MeasurementAvailable)
                If sender.MeasurementAvailable Then
                    Me.showResistance(Me._InitialResistanceTextBox, sender)
                End If
            Case NameOf(sender.LastReading)
                If String.IsNullOrWhiteSpace(sender.LastReading) Then
                    Me.clearResistance(Me._InitialResistanceTextBox)
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _InitialResistance for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _InitialResistance_PropertyChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InitialResistance.PropertyChanged
        Try
            Me.OnInitialResistancePropertyChanged(TryCast(sender, ColdResistance), e?.PropertyName)
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling property '{0}'. Details: {1}.",
                         e.PropertyName, ex.Message)
        End Try
    End Sub

#End Region

#Region " PART: FINAL RESISTANCE "

    ''' <summary> The Part Final Resistance. </summary>
    Private WithEvents _FinalResistance As ColdResistance

    ''' <summary> Executes the Final resistance property changed action. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnFinalResistancePropertyChanged(ByVal sender As ColdResistance, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.MeasurementAvailable)
                If sender.MeasurementAvailable Then
                    Me.showResistance(Me._FinalResistanceTextBox, sender)
                End If
            Case NameOf(sender.LastReading)
                If String.IsNullOrWhiteSpace(sender.LastReading) Then
                    Me.clearResistance(Me._FinalResistanceTextBox)
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _FinalResistance for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _FinalResistance_PropertyChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _FinalResistance.PropertyChanged
        Try
            Me.OnFinalResistancePropertyChanged(TryCast(sender, ColdResistance), e?.PropertyName)
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling property '{0}'. Details: {1}.",
                         e.PropertyName, ex.Message)
        End Try
    End Sub

#End Region

#Region " PART: THERMAL TRANSIENT "

    ''' <summary> The Part Thermal Transient. </summary>
    Private WithEvents _ThermalTransient As ThermalTransient

    ''' <summary> Executes the initial resistance property changed action. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnThermalTransientPropertyChanged(ByVal sender As ResistanceMeasureBase, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.MeasurementAvailable)
                If sender.MeasurementAvailable Then
                    Me.showThermalTransient(Me._ThermalTransientVoltageTextBox, sender)
                End If
            Case NameOf(sender.LastReading)
                If String.IsNullOrWhiteSpace(sender.LastReading) Then
                    Me.clearResistance(Me._ThermalTransientVoltageTextBox)
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _ThermalTransient for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ThermalTransient_PropertyChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ThermalTransient.PropertyChanged
        Try
            Me.OnThermalTransientPropertyChanged(TryCast(sender, ResistanceMeasureBase), e?.PropertyName)
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling property '{0}'. Details: {1}.",
                         e.PropertyName, ex.Message)
        End Try
    End Sub

#End Region

End Class
