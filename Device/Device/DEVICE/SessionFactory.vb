Imports isr.VI.ExceptionExtensions
''' <summary> A session factory. </summary>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="11/29/2015" by="David" revision=""> Created. </history>
Public Class SessionFactory
    Inherits isr.Core.Pith.PropertyNotifyBase

#Region " CONSTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New
        Me.PingFilterEnabled = True
        Me.UseNationalVisaSessionFactory()
    End Sub

    ''' <summary>
    ''' The locking object to enforce thread safety when creating the singleton instance.
    ''' </summary>
    Private Shared ReadOnly syncLocker As New Object

    ''' <summary>
    ''' The shared instance.
    ''' </summary>
    Private Shared _instance As SessionFactory

    ''' <summary> Returns a new or existing instance of this class. </summary>
    ''' <remarks> Use this property to get an instance of this class. Returns the default instance of
    ''' this form allowing to use this form as a singleton for. </remarks>
    ''' <value> <c>A</c> new or existing instance of the class. </value>
    Public Shared ReadOnly Property [Get]() As SessionFactory
        Get
            If SessionFactory._instance Is Nothing Then
                SyncLock SessionFactory.syncLocker
                    SessionFactory._instance = New SessionFactory
                    SessionFactory._instance.UseNationalVisaSessionFactory()
                End SyncLock
            End If
            Return SessionFactory._instance
        End Get
    End Property

    ''' <summary> Gets or sets the factory. </summary>
    ''' <value> The factory. </value>
    Public Property Factory As VI.Pith.SessionFactoryBase

#End Region

#Region " RESOURE MANAGER "

    Private _HasResources As Boolean
    ''' <summary> Gets or sets the has resources. </summary>
    ''' <value> The has resources. </value>
    Public Property HasResources As Boolean
        Get
            Return _HasResources
        End Get
        Set(ByVal value As Boolean)
            If Not Me.HasResources.Equals(value) Then
                Me._HasResources = value
                ' using sync notification is required when using unit tests; otherwise,
                ' actions invoked by the event may not occur.
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
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.ResourcesFilter, StringComparison.OrdinalIgnoreCase) Then
                Me._ResourcesFilter = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _PingFilterEnabled As Boolean
    ''' <summary>
    ''' Gets or sets the Ping Filter enabled sentinel. When enabled, Tcp/IP resources are added only
    ''' if they can be pinged.
    ''' </summary>
    ''' <value> The ping filter enabled. </value>
    Public Property PingFilterEnabled As Boolean
        Get
            Return Me._PingFilterEnabled
        End Get
        Set(value As Boolean)
            If value <> Me.PingFilterEnabled Then
                Me._PingFilterEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _EnumeratedResources As IEnumerable(Of String)

    ''' <summary> Enumerates enumerated resources in this collection. </summary>
    ''' <returns>
    ''' An enumerator that allows for each to be used to process enumerated resources in this
    ''' collection.
    ''' </returns>
    Public Function EnumeratedResources() As IEnumerable(Of String)
        Return Me._EnumeratedResources
    End Function

    ''' <summary> Enumerates the resources in this collection. </summary>
    ''' <returns>
    ''' An enumerator that allows for each to be used to process the resources in this collection.
    ''' </returns>
    Public Function EnumerateResources() As IEnumerable(Of String)
        Dim resources As IEnumerable(Of String) = New String() {}
        Using rm As VI.Pith.ResourcesManagerBase = Me.Factory.CreateResourcesManager()
            If String.IsNullOrWhiteSpace(Me.ResourcesFilter) Then
                resources = rm.FindResources()
            Else
                resources = rm.FindResources(Me.ResourcesFilter).ToArray
            End If
        End Using
        If Me.PingFilterEnabled Then resources = VI.Pith.ResourceNamesManager.PingFilter(resources)
        Me._EnumeratedResources = resources
        Me.HasResources = resources.Any
        Return resources
    End Function

    ''' <summary>
    ''' Displays the resource names based on the <see cref="ResourcesFilter">search pattern</see>.
    ''' </summary>
    ''' <param name="comboBox">      The combo box. </param>
    ''' <param name="resourceNames"> List of names of the resources. </param>
    Public Shared Sub DisplayResourceNames(ByVal comboBox As Windows.Forms.ComboBox, ByVal resourceNames As IEnumerable(Of String))
        If comboBox Is Nothing Then Throw New ArgumentNullException(NameOf(comboBox))
        If resourceNames.Any Then
            Dim resourceName As String = comboBox.Text
            comboBox.DataSource = Nothing
            comboBox.Items.Clear()
            comboBox.DataSource = resourceNames
            If resourceNames.Contains(resourceName) Then comboBox.Text = resourceName
        End If
    End Sub

    ''' <summary>
    ''' Displays the resource names based on the <see cref="ResourcesFilter">search pattern</see>.
    ''' </summary>
    ''' <param name="comboBox"> The combo box. </param>
    Public Sub DisplayResourceNames(ByVal comboBox As Windows.Forms.ComboBox)
        SessionFactory.DisplayResourceNames(comboBox, Me.EnumerateResources)
    End Sub

    ''' <summary> Enumerates the default resource name patterns in this collection. </summary>
    ''' <returns>
    ''' An enumerator that allows for each to be used to process the default resource name patters in
    ''' this collection.
    ''' </returns>
    Public Shared Function EnumerateDefaultResourceNamePatterns() As IEnumerable(Of String)
        Return New List(Of String) From {
            "GPIB[board]::number[::INSTR]",
            "GPIB[board]::INTFC",
            "TCPIP[board]::host address[::LAN device name][::INSTR]",
            "TCPIP[board]::host address::port::SOCKET"
        }
    End Function

    ''' <summary> Displays the default resource name patterns. </summary>
    ''' <param name="comboBox"> The combo box. </param>
    Public Shared Sub DisplayResourceNamePatterns(ByVal comboBox As Windows.Forms.ComboBox)
        If comboBox Is Nothing Then Throw New ArgumentNullException(NameOf(comboBox))
        comboBox.DataSource = Nothing
        comboBox.Items.Clear()
        comboBox.DataSource = SessionFactory.EnumerateDefaultResourceNamePatterns
    End Sub

#End Region

#Region " RESOURCE SELECTION "

    Private _EnteredResourceName As String
    ''' <summary> Gets the name of the entered resource. </summary>
    ''' <value> The name of the entered resource. </value>
    Public Property EnteredResourceName As String
        Get
            Return Me._EnteredResourceName
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            value = value.Trim
            If Not String.Equals(value, Me.EnteredResourceName, StringComparison.OrdinalIgnoreCase) Then
                Me._EnteredResourceName = value.Trim
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _SelectedResourceName As String
    ''' <summary> Returns the selected resource name. </summary>
    ''' <value> The name of the selected. </value>
    Public Property SelectedResourceName() As String
        Get
            Return Me._SelectedResourceName
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            value = value.Trim
            If Not String.Equals(value, Me.SelectedResourceName) Then
                Me._SelectedResourceName = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    Private _SelectedResourceExists As Boolean
    ''' <summary> Gets or sets the has resources. </summary>
    ''' <value> The has resources. </value>
    Public Property SelectedResourceExists As Boolean
        Get
            Return Me._SelectedResourceExists
        End Get
        Private Set(ByVal value As Boolean)
            ' not checking for changed value here because this needs to be refreshed if a new
            ' resource was selected.
            Me._SelectedResourceExists = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Queries if a given check resource exists. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Public Function QueryResourceExists(ByVal resourceName As String) As Boolean
        If String.IsNullOrWhiteSpace(resourceName) Then Throw New ArgumentNullException(NameOf(resourceName))
        Using rm As VI.Pith.ResourcesManagerBase = Me.Factory.CreateResourcesManager()
            Return rm.Exists(resourceName)
        End Using
    End Function

    Private _OpenResourceSessionEnabled As Boolean

    ''' <summary> Gets or sets the open resource session enabled. </summary>
    ''' <value> The open resource session enabled. </value>
    Public Property OpenResourceSessionEnabled As Boolean
        Get
            Return Me._OpenResourceSessionEnabled
        End Get
        Set(ByVal value As Boolean)
            ' not checking for changed value here because this needs to be refreshed if a new
            ' resource was selected.
            Me._OpenResourceSessionEnabled = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

    ''' <summary> Attempts to select resource from the given data. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <param name="e">            Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification:="OK")>
    Public Function TrySelectResource(ByVal resourceName As String, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If String.IsNullOrWhiteSpace(resourceName) Then resourceName = ""
        Dim activity As String = ""
        Try
            Me.EnteredResourceName = resourceName
            activity = "checking if need to select the resource"
            If String.IsNullOrWhiteSpace(resourceName) Then
                activity = "attempted to select an empty resource name"
                e.RegisterOutcomeEvent(TraceEventType.Information, activity)
            ElseIf Not String.Equals(resourceName, Me.SelectedResourceName, StringComparison.OrdinalIgnoreCase) OrElse
                   Not Me.SelectedResourceExists OrElse Not Me.OpenResourceSessionEnabled Then
                activity = $"checking if selected resource {resourceName} exists"
                If Me.QueryResourceExists(resourceName) Then
                    activity = $"setting selected resource to {resourceName}"
                    Me.SelectedResourceName = resourceName
                    Me.SelectedResourceExists = True
                Else
                    Me.SelectedResourceExists = False
                End If
            End If
        Catch ex As Exception
            e.RegisterCancellation($"Exception {activity};. {ex.ToFullBlownString}")
        Finally
        End Try
        Return Not e.Cancel
    End Function

#End Region

#Region " RESOURE LOOKUP "


#End Region

End Class
