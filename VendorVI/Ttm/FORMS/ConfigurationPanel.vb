Imports System.ComponentModel
''' <summary> Panel for editing the TTM configuration. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="2/25/2014" by="David" revision=""> Created. </history>
Public Class ConfigurationPanel
    Inherits ConfigurationPanelBase

#Region " CONFIGURE "

    Protected Overrides Sub ReleaseResources()
        MyBase.ReleaseResources()
        Me._meter = Nothing
    End Sub

    ''' <summary> The with events. </summary>
    Private _Meter As Meter

    ''' <summary> Gets or sets reference to the thermal transient meter device. </summary>
    ''' <value> The meter. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Meter() As Meter
        Get
            Return Me._meter
        End Get
        Set(ByVal value As Meter)
            Me._meter = value
            If value Is Nothing Then
                Me.DeviceUnderTest = Nothing
            Else
                Me.DeviceUnderTest = Me.Meter.ConfigInfo
            End If
        End Set
    End Property

    ''' <summary> Configure changed. </summary>
    ''' <param name="part"> The part. </param>
    Protected Overrides Sub ConfigureChanged(part As DeviceUnderTest)
        If Me.Meter IsNot Nothing Then
            Me.Meter.ConfigureChanged(part)
        End If
    End Sub

    ''' <summary> Configures the given part. </summary>
    ''' <param name="part"> The part. </param>
    Protected Overrides Sub Configure(part As DeviceUnderTest)
        If Me.Meter IsNot Nothing Then
            Me.Meter.Configure(Me.Part)
        End If
    End Sub

#End Region

End Class
