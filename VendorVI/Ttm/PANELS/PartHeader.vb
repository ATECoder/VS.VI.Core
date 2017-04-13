Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Part header. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="4/12/2014" by="David" revision=""> Created. </history>
Public Class PartHeader
    Inherits MyUserControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> This constructor is public to allow using this as a startup form for the project. </summary>
    Public Sub New()
        MyBase.New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me._PartNumberTextBox.Text = ""
        Me._PartSerialNumberTextBox.Text = ""

    End Sub

    ''' <summary> Releases the resources. </summary>
    Private Sub releaseResources()
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
        End Set
    End Property

    ''' <summary> Executes the property changed action. </summary>
    ''' <remarks> David, 1/13/2016. </remarks>
    ''' <param name="sender">       The source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnPropertyChanged(ByVal sender As DeviceUnderTest, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(sender.PartNumber)
                If String.IsNullOrWhiteSpace(sender.PartNumber) Then
                    Me.Visible = False
                Else
                    Me.Visible = True
                    Me._PartNumberTextBox.Text = sender.PartNumber
                End If
            Case NameOf(sender.SerialNumber)
                Me._PartSerialNumberTextBox.Text = sender.SerialNumber.ToString
        End Select
    End Sub

    ''' <summary> Event handler. Called by _DeviceUnderTest for property changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Property changed event information. </param>
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

End Class
