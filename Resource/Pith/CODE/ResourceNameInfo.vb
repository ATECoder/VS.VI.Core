''' <summary> Information about the resource name. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="4/4/2018" by="David" revision=""> Created. </history>
Public Class ResourceNameInfo
    Inherits isr.Core.Pith.PropertyPublisherBase

#Region " CONSTRUCTOR "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New
        Me.DefaultResourceNameClosedCaption = My.Settings.DefaultClosedResourceCaption
        Me.ResourceNameClosedCaption = Me.DefaultResourceNameClosedCaption
        Me.DefaultResourceTitle = My.Settings.DefaultResourceTitle
        Me.ResourceTitle = Me.ResourceTitle
        Me.DefaultResourcesFilter = ResourceNameInfo.BuildMinimalResourcesFilter
        Me.ResourcesFilter = Me.DefaultResourcesFilter
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

#Region " FILTER "

    ''' <summary> Builds minimal resources filter. </summary>
    ''' <returns> A String. </returns>
    Public Shared Function BuildMinimalResourcesFilter() As String
        Return ResourceNamesManager.BuildInstrumentFilter(HardwareInterfaceType.Gpib,
                                                             HardwareInterfaceType.Tcpip,
                                                             HardwareInterfaceType.Usb)
    End Function

    Private _DefaultResourcesFilter As String

    ''' <summary> Gets or sets the default resource filter. </summary>
    ''' <value> The default resource filter. </value>
    Public Property DefaultResourcesFilter As String
        Get
            Return Me._DefaultResourcesFilter
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.DefaultResourcesFilter) Then
                Me._DefaultResourcesFilter = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    Private _ResourcesFilter As String
    ''' <summary> Gets or sets the resources search pattern. </summary>
    ''' <value> The resources search pattern. </value>
    Public Property ResourcesFilter As String
        Get
            Return Me._ResourcesFilter
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.ResourcesFilter) Then
                Me._ResourcesFilter = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Checks if the resource name exists. Use for checking if the instrument is turned on.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="manager"> The manager. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function Find(ByVal manager As VI.Pith.ResourcesManagerBase) As Boolean
        If manager Is Nothing Then Throw New ArgumentNullException(NameOf(manager))
        Dim result As Boolean
        If String.IsNullOrWhiteSpace(Me.ResourcesFilter) Then
            result = manager.FindResources().ToArray.Contains(Me.ResourceName)
        Else
            result = manager.FindResources(ResourcesFilter).ToArray.Contains(Me.ResourceName)
        End If
        If result Then
            Me.FoundResourceName = Me.ResourceName
        Else
            Me.FoundResourceName = String.Empty
        End If
        Return result
    End Function

    Private _FoundResourceName As String
    ''' <summary> Gets the name of the found resource. </summary>
    ''' <value> The name of the found resource. </value>
    Public Property FoundResourceName As String
        Get
            Return Me._FoundResourceName
        End Get
        Set(value As String)
            Me._FoundResourceName = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Gets the resource located. </summary>
    ''' <value> The resource located. </value>
    Public ReadOnly Property ResourceLocated As Boolean
        Get
            Return Not String.IsNullOrWhiteSpace(Me.FoundResourceName) AndAlso
                   String.Equals(Me.ResourceName, Me.FoundResourceName, StringComparison.OrdinalIgnoreCase)
        End Get
    End Property

#End Region

#Region " NAME  AND CAPTION"

    ''' <summary> Executes the session open actions. </summary>
    ''' <param name="resourceName">  The name of the resource. </param>
    ''' <param name="resourceTitle"> The short title of the device. </param>
    Public Sub HandleSessionOpen(ByVal resourceName As String, ByVal resourceTitle As String)
        Me._ResourceName = resourceName
        Me._ResourceTitle = resourceTitle
        Me.IsDeviceOpen = True
        Me.SafePostPropertyChanged(NameOf(ResourceNameInfo.ResourceName))
        Me.SafePostPropertyChanged(NameOf(ResourceNameInfo.ResourceTitle))
    End Sub

    Private _IsDeviceOpen As Boolean

    ''' <summary> Gets or sets the is device open. </summary>
    ''' <value> The is device open. </value>
    Public Property IsDeviceOpen As Boolean
        Get
            Return Me._IsDeviceOpen
        End Get
        Set(value As Boolean)
            Me._IsDeviceOpen = value
            Me.SafeSendPropertyChanged()
            Me.SafeSendPropertyChanged(NameOf(ResourceNameInfo.ResourceNameCaption))
        End Set
    End Property

    Private _DefaultResourceNameClosedCaption As String
    ''' <summary> Gets or sets the default resource name closed caption. </summary>
    Public Property DefaultResourceNameClosedCaption As String
        Get
            Return Me._DefaultResourceNameClosedCaption
        End Get
        Set(ByVal value As String)
            If Not String.Equals(Me.DefaultResourceNameClosedCaption, value) Then
                Me._DefaultResourceNameClosedCaption = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    Private _ResourceNameClosedCaption As String

    ''' <summary> Gets or sets the resource name closed caption. </summary>
    ''' <value> The resource name closed caption. </value>
    Public Property ResourceNameClosedCaption As String
        Get
            Return Me._ResourceNameClosedCaption
        End Get
        Set(ByVal value As String)
            If Not String.Equals(Me.ResourceNameClosedCaption, value) Then
                Me._ResourceNameClosedCaption = value
                Me.SafeSendPropertyChanged()
                Me.SafeSendPropertyChanged(NameOf(ResourceNameInfo.ResourceNameCaption))
            End If
        End Set
    End Property

    ''' <summary> Gets the resource name caption. </summary>
    ''' <value>
    ''' The <see cref="ResourceName"/> resource <see cref="ResourceNameClosedCaption">closed
    ''' caption</see> if not open.
    ''' </value>
    Public ReadOnly Property ResourceNameCaption As String
        Get
            If Me.IsDeviceOpen Then
                Return Me.ResourceName
            Else
                Return Me.ResourceNameClosedCaption
            End If
        End Get
    End Property

    Private _ResourceName As String
    ''' <summary> Gets the name of the resource. </summary>
    Public Property ResourceName As String
        Get
            Return Me._ResourceName
        End Get
        Set(ByVal value As String)
            If Not String.Equals(Me.ResourceName, value) Then
                Me._ResourceName = value
                Me.SafeSendPropertyChanged()
                Me.SafeSendPropertyChanged(NameOf(ResourceNameInfo.ResourceNameCaption))
            End If
        End Set
    End Property

#End Region

#Region " TITLE "

    Private _DefaultResourceTitle As String
    ''' <summary> Gets or sets the default resource title. </summary>
    Public Property DefaultResourceTitle As String
        Get
            Return Me._DefaultResourceTitle
        End Get
        Set(value As String)
            If value Is Nothing Then value = String.Empty
            If Not String.Equals(value, Me.DefaultResourceTitle) Then
                Me._DefaultResourceTitle = value
                Me.SafeSendPropertyChanged()
                Me.SafeSendPropertyChanged(NameOf(ResourceNameInfo.ResourceTitle))
            End If
        End Set
    End Property

    Private _ResourceTitle As String
    ''' <summary> Gets or sets a short title for the device. </summary>
    ''' <value> The short title of the device. </value>
    Public Property ResourceTitle As String
        Get
            If Me.IsDeviceOpen Then
                Return Me._ResourceTitle
            Else
                Return Me.DefaultResourceTitle
            End If
        End Get
        Set(value As String)
            If value Is Nothing Then value = String.Empty
            If Not String.Equals(Me.ResourceTitle, value) Then
                Me._ResourceTitle = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

End Class
