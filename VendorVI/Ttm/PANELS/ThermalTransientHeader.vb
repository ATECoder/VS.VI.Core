Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Thermal Transient Model header. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="2/25/2014" by="David" revision=""> Created. </history>
Public Class ThermalTransientHeader
    Inherits MyUserControlBase

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
                Me._ThermalTransient = Nothing
            Else
                Me._ThermalTransient = value.ThermalTransient
            End If
        End Set
    End Property

    ''' <summary> Releases the resources. </summary>
    Private Sub ReleaseResources()
        Me._ThermalTransient = Nothing
        Me._DeviceUnderTest = Nothing
    End Sub

    ''' <summary> Executes the device under test property changed action. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As DeviceUnderTest, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.Outcome)
                If sender.Outcome = MeasurementOutcomes.None Then
                    Me.Clear()
                End If
        End Select
    End Sub

    ''' <summary> Event handler. Called by _DeviceUnderTest for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _DeviceUnderTest_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _DeviceUnderTest.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._DeviceUnderTest_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, DeviceUnderTest), e?.PropertyName)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}.", e.PropertyName, ex.ToFullBlownString)

        End Try
    End Sub

#End Region

#Region " DISPLAY VALUE "

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Sub Clear()
        Me._ErrorProvider.Clear()
        Me._AsymptoteTextBox.Text = ""
        Me._EstimatedVoltageTextBox.Text = ""
        Me._IterationsCountTextBox.Text = ""
        Me._CorrelationCoefficientTextBox.Text = ""
        Me._StandardErrorTextBox.Text = ""
        Me._TimeConstantTextBox.Text = ""
        Me._OutcomeTextBox.Text = ""
    End Sub

#End Region

#Region " PART: THERMAL TRANSIENT "

    ''' <summary> The Part Thermal Transient. </summary>
    Private WithEvents _ThermalTransient As ThermalTransient

    ''' <summary> Executes the device under test property changed action. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As ThermalTransient, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.TimeConstant)
                Me._TimeConstantTextBox.Text = sender.TimeConstantCaption
            Case NameOf(sender.Asymptote)
                Me._AsymptoteTextBox.Text = sender.AsymptoteCaption
            Case NameOf(sender.EstimatedVoltage)
                Me._EstimatedVoltageTextBox.Text = sender.EstimatedVoltageCaption
            Case NameOf(sender.CorrelationCoefficient)
                Me._CorrelationCoefficientTextBox.Text = sender.CorrelationCoefficientCaption
            Case NameOf(sender.StandardError)
                Me._StandardErrorTextBox.Text = sender.StandardErrorCaption
            Case NameOf(sender.Iterations)
                Me._IterationsCountTextBox.Text = sender.IterationsCaption
            Case NameOf(sender.OptimizationOutcome)
                Me._OutcomeTextBox.Text = sender.OptimizationOutcomeCaption
                Me._ToolTip.SetToolTip(Me._OutcomeTextBox, sender.OptimizationOutcomeDescription)
        End Select
    End Sub

    ''' <summary> Event handler. Called by _ThermalTransient for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _ThermalTransient_PropertyChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ThermalTransient.PropertyChanged
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of Object, PropertyChangedEventArgs)(AddressOf Me._ThermalTransient_PropertyChanged), New Object() {sender, e})
            Else
                Me.OnPropertyChanged(TryCast(sender, ThermalTransient), e?.PropertyName)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}.", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

End Class
