
''' <summary> Encapsulates the result of a resource parse. </summary>
''' <remarks> David, 11/23/2015. </remarks>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="11/23/2015" by="David" revision=""> Created. </history>
Public Class ResourceParseResult

    ''' <summary> Default constructor. </summary>
    ''' <remarks> David, 11/23/2015. </remarks>
    Public Sub New()
        MyBase.New()
        Me.ResourceName = ""
    End Sub

    ''' <summary> Default constructor. </summary>
    ''' <remarks> David, 11/23/2015. </remarks>
    Public Sub New(ByVal resourceName As String)
        MyBase.New()
        Me._Parse(resourceName)
    End Sub

    ''' <summary> Default constructor. </summary>
    ''' <remarks> David, 11/23/2015. </remarks>
    Public Sub New(ByVal resourceName As String, ByVal interfaceType As HardwareInterfaceType, ByVal interfaceNumber As Integer)
        MyBase.New()
        Me._ResourceName = resourceName
        Me._InterfaceType = interfaceType
        Me._InterfaceNumber = interfaceNumber
        Me._InterfaceBaseName = VI.ResourceNamesManager.InterfaceResourceBaseName(Me._InterfaceType)
        Me._InterfaceResourceName = VI.ResourceNamesManager.BuildInterfaceResourceName(Me._InterfaceBaseName, Me._InterfaceNumber)
        Me._ResourceType = VI.ResourceNamesManager.ParseResourceType(resourceName)
        Me._ResourceAddress = ""
        If Me.ResourceType = ResourceType.Instrument Then
            Me._ResourceAddress = VI.ResourceNamesManager.ParseAddress(resourceName)
        End If
        If Me._InterfaceType = HardwareInterfaceType.Gpib Then
            If Not Integer.TryParse(Me._ResourceAddress, Me._GpibAddress) Then
                Me._GpibAddress = 0
            End If
        End If
    End Sub

    ''' <summary> Parses. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    ''' <param name="resourceName"> The name of the resource. </param>
    Private Sub _Parse(ByVal resourceName As String)
        Me._ResourceName = resourceName
        Me._InterfaceType = VI.ResourceNamesManager.ParseHardwareInterfaceType(resourceName)
        Me._InterfaceNumber = VI.ResourceNamesManager.ParseInterfaceNumber(resourceName)
        Me._InterfaceBaseName = VI.ResourceNamesManager.InterfaceResourceBaseName(Me._InterfaceType)
        Me._InterfaceResourceName = VI.ResourceNamesManager.BuildInterfaceResourceName(Me._InterfaceBaseName, Me._InterfaceNumber)
        Me._ResourceType = VI.ResourceNamesManager.ParseResourceType(resourceName)
        Me._ResourceAddress = ""
        If Me.ResourceType = ResourceType.Instrument Then
            Me._ResourceAddress = VI.ResourceNamesManager.ParseAddress(resourceName)
        End If
        If Me._InterfaceType = HardwareInterfaceType.Gpib Then
            If Not Integer.TryParse(Me._ResourceAddress, Me._GpibAddress) Then
                Me._GpibAddress = 0
            End If
        End If
    End Sub

    ''' <summary> Parses resource name </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="resourceName"> The name of the resource. </param>
    Public Sub Parse(ByVal resourceName As String)
        Me._Parse(resourceName)
    End Sub

    ''' <summary> Gets or sets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Property ResourceName As String

    ''' <summary> Gets the type of the resource. </summary>
    ''' <value> The type of the resource. </value>
    Public Property ResourceType As ResourceType

    ''' <summary> Gets or sets the type of the interface. </summary>
    ''' <value> The type of the interface. </value>
    Public Property InterfaceType As HardwareInterfaceType

    ''' <summary> Gets or sets the interface number. </summary>
    ''' <value> The interface number. </value>
    Public Property InterfaceNumber As Integer

    ''' <summary> Gets the resource address. </summary>
    ''' <value> The resource address. </value>
    Public Property ResourceAddress As String

    ''' <summary> Gets the gpib address. </summary>
    ''' <value> The gpib address. </value>
    Public Property GpibAddress As Integer

    ''' <summary> Gets the is parsed. </summary>
    ''' <value> The is parsed. </value>
    Public ReadOnly Property IsParsed As Boolean
        Get
            Return Not String.IsNullOrWhiteSpace(Me.ResourceName)
        End Get
    End Property

    ''' <summary> Gets the name of the interface base. </summary>
    ''' <value> The name of the interface base. </value>
    Public ReadOnly Property InterfaceBaseName As String

    Private _InterfaceResourceName As String
    ''' <summary> Gets the name of the interface resource. </summary>
    ''' <returns> The name of the interface resource. </returns>
    Public Function InterfaceResourceName() As String
        If Me.InterfaceType = VI.HardwareInterfaceType.Gpib Then
            Return Me._InterfaceResourceName
        Else
            Throw New NotImplementedException("Interface resource name is available only for GPIB interfaces at this time")
        End If
    End Function

End Class
