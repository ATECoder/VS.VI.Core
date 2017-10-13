Imports System.Configuration
''' <summary> Information about the test. </summary>
''' <license>
''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="10/13/2017" by="David" revision=""> Created. </history>
Public NotInheritable Class TestInfo

#Region " CONSTRUCTOR "

    Private Sub New()
        MyBase.New
    End Sub

#End Region

#Region " CONFIGURATION INFORMTION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property Exists As Boolean
        Get
            Return String.Equals("True", ConfigurationManager.AppSettings(NameOf(TestInfo.Exists)), StringComparison.OrdinalIgnoreCase)
        End Get
    End Property

#End Region

#Region " DEVICE INFORMATION "

    ''' <summary> Gets the Model of the resource. </summary>
    ''' <value> The Model of the resource. </value>
    Public Shared ReadOnly Property ResourceModel As String
        Get
            Return ConfigurationManager.AppSettings(NameOf(TestInfo.ResourceModel))
        End Get
    End Property

    ''' <summary> Gets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Shared ReadOnly Property ResourceName As String
        Get
            Return ConfigurationManager.AppSettings(NameOf(TestInfo.ResourceName))
        End Get
    End Property

    ''' <summary> Gets the Title of the resource. </summary>
    ''' <value> The Title of the resource. </value>
    Public Shared ReadOnly Property ResourceTitle As String
        Get
            Return ConfigurationManager.AppSettings(NameOf(TestInfo.ResourceTitle))
        End Get
    End Property

#End Region

End Class
