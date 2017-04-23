''' <summary> An inheritable exception for use by framework classes and applications. </summary>
''' <remarks> Inherits from System.Exception per design rule CA1958 which specifies that "Types do
''' not extend inheritance vulnerable types" and further explains that "This [Application
''' Exception] base exception type does not provide any additional value for framework classes.". </remarks>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/21/2014" by="David" revision="x.x.5134">   Based on legacy base exception. </history>
''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created. </history>
<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")>
<Serializable()>
Public MustInherit Class ExceptionBase
    Inherits System.Exception

#Region " CONSTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ExceptionBase" /> class. </summary>
    Protected Sub New()
        MyBase.New()
        obtainEnvironmentInformation()
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="ExceptionBase" /> class. </summary>
    ''' <param name="message"> The message. </param>
    Protected Sub New(ByVal message As String)
        MyBase.New(message)
        obtainEnvironmentInformation()
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="ExceptionBase" /> class. </summary>
    ''' <param name="message">        The message. </param>
    ''' <param name="innerException"> The inner exception. </param>
    Protected Sub New(ByVal message As String, ByVal innerException As System.Exception)
        MyBase.New(message, innerException)
        obtainEnvironmentInformation()
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="ExceptionBase" /> class. </summary>
    ''' <param name="format"> The format. </param>
    ''' <param name="args">   The arguments. </param>
    Protected Sub New(ByVal format As String, ByVal ParamArray args() As Object)
        MyBase.New(String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
        obtainEnvironmentInformation()
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="ExceptionBase" /> class. </summary>
    ''' <param name="innerException"> Specifies the InnerException. </param>
    ''' <param name="format">         Specifies the exception formatting. </param>
    ''' <param name="args">           Specifies the message arguments. </param>
    Protected Sub New(ByVal innerException As System.Exception, ByVal format As String, ByVal ParamArray args() As Object)
        MyBase.New(String.Format(Globalization.CultureInfo.CurrentCulture, format, args), innerException)
    End Sub

    ''' <summary> Initializes a new instance of the class with serialized data. </summary>
    ''' <param name="info">    The <see cref="T:System.Runtime.Serialization.SerializationInfo" />
    ''' that holds the serialized object data about the exception being thrown. </param>
    ''' <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext" />
    ''' that contains contextual information about the source or destination. </param>
    Protected Sub New(ByVal info As Runtime.Serialization.SerializationInfo, ByVal context As Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
        If info Is Nothing Then
            Return
        End If
        Me._machineName = info.GetString("machineName")
        Me._createdDateTime = info.GetDateTime("createdDateTime")
        Me._appDomainName = info.GetString("appDomainName")
        Me._threadIdentityName = info.GetString("threadIdentity")
        Me._windowsIdentityName = info.GetString("windowsIdentity")
        Me._OSVersion = info.GetString("OSVersion")
        Me._additionalInformation = CType(info.GetValue("additionalInformation",
                                                        GetType(System.Collections.Specialized.NameValueCollection)), 
                                                    System.Collections.Specialized.NameValueCollection)
    End Sub

    ''' <summary> Overrides the <see cref="GetObjectData" /> method to serialize custom values. </summary>
    ''' <param name="info">    The <see cref="Runtime.Serialization.SerializationInfo">serialization
    ''' information</see>. </param>
    ''' <param name="context"> The <see cref="Runtime.Serialization.StreamingContext">streaming
    ''' context</see> for the exception. </param>
    <Security.Permissions.SecurityPermission(Security.Permissions.SecurityAction.Demand, SerializationFormatter:=True)>
    Public Overrides Sub GetObjectData(ByVal info As Runtime.Serialization.SerializationInfo, ByVal context As Runtime.Serialization.StreamingContext)

        If info Is Nothing Then
            Return
        End If
        info.AddValue("machineName", Me.MachineName, GetType(String))
        info.AddValue("createdDateTime", Me.CreatedDateTime)
        info.AddValue("appDomainName", Me.AppDomainName, GetType(String))
        info.AddValue("threadIdentity", Me.ThreadIdentityName, GetType(String))
        info.AddValue("windowsIdentity", Me.WindowsIdentityName, GetType(String))
        info.AddValue("OSVersion", Me.OSVersion, GetType(String))
        info.AddValue("additionalInformation", Me.AdditionalInformation, GetType(System.Collections.Specialized.NameValueCollection))
        MyBase.GetObjectData(info, context)
    End Sub

#End Region

#Region " ADDITIONAL INFORMATION "

    ''' <summary> Information describing the additional. </summary>
    Private _AdditionalInformation As System.Collections.Specialized.NameValueCollection

    ''' <summary> Collection allowing additional information to be added to the exception. </summary>
    ''' <value> The additional information. </value>
    Public ReadOnly Property AdditionalInformation() As System.Collections.Specialized.NameValueCollection
        Get
            If _additionalInformation Is Nothing Then
                _additionalInformation = New System.Collections.Specialized.NameValueCollection
            End If
            Return Me._additionalInformation
        End Get
    End Property

    ''' <summary> Name of the application domain. </summary>
    Private _AppDomainName As String

    ''' <summary> AppDomain name where the Exception occurred. </summary>
    ''' <value> The name of the application domain. </value>
    Public ReadOnly Property AppDomainName() As String
        Get
            Return Me._appDomainName
        End Get
    End Property

    ''' <summary> Time of the created date. </summary>
    Private _CreatedDateTime As DateTime

    ''' <summary> Date and Time the exception was created. </summary>
    ''' <value> The created date time. </value>
    Public ReadOnly Property CreatedDateTime() As DateTime
        Get
            Return Me._createdDateTime
        End Get
    End Property

    ''' <summary> Name of the machine. </summary>
    Private _MachineName As String

    ''' <summary> Machine name where the Exception occurred. </summary>
    ''' <value> The name of the machine. </value>
    Public ReadOnly Property MachineName() As String
        Get
            Return Me._machineName
        End Get
    End Property

    ''' <summary> The operating system version. </summary>
    Private _OSVersion As String

    ''' <summary> Gets the OS version. </summary>
    ''' <value> The OS version. </value>
    Public ReadOnly Property OSVersion() As String
        Get
            Return Me._OSVersion
        End Get
    End Property

    ''' <summary> Name of the thread identity. </summary>
    Private _ThreadIdentityName As String

    ''' <summary> Identity of the executing thread on which the exception was created. </summary>
    ''' <value> The name of the thread identity. </value>
    Public ReadOnly Property ThreadIdentityName() As String
        Get
            Return Me._threadIdentityName
        End Get
    End Property

    ''' <summary> Name of the windows identity. </summary>
    Private _WindowsIdentityName As String

    ''' <summary> Windows identity under which the code was running. </summary>
    ''' <value> The name of the windows identity. </value>
    Public ReadOnly Property WindowsIdentityName() As String
        Get
            Return Me._windowsIdentityName
        End Get
    End Property

    ''' <summary> Gathers environment information safely. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub ObtainEnvironmentInformation()

        Const unknown As String = "N/A"
        Me._createdDateTime = DateTime.Now
        Me._createdDateTime = DateTime.Now

        Me._machineName = Environment.MachineName
        If Me._machineName.Length = 0 Then
            Me._machineName = unknown
        End If

        Me._threadIdentityName = System.Threading.Thread.CurrentPrincipal.Identity.Name
        If Me._threadIdentityName.Length = 0 Then
            Me._threadIdentityName = unknown
        End If

        Me._windowsIdentityName = System.Security.Principal.WindowsIdentity.GetCurrent().Name
        If Me._windowsIdentityName.Length = 0 Then
            Me._windowsIdentityName = unknown
        End If

        Me._appDomainName = AppDomain.CurrentDomain.FriendlyName
        If Me._appDomainName.Length = 0 Then
            Me._appDomainName = unknown
        End If

        Me._OSVersion = Environment.OSVersion.ToString
        If Me._OSVersion.Length = 0 Then
            Me._OSVersion = unknown
        End If

        Me._AdditionalInformation = New System.Collections.Specialized.NameValueCollection From {
            {AdditionalInfoItem.MachineName.ToString, My.Computer.Name},
            {AdditionalInfoItem.Timestamp.ToString, Date.Now.ToString(System.Globalization.CultureInfo.CurrentCulture)},
            {AdditionalInfoItem.FullName.ToString, Reflection.Assembly.GetExecutingAssembly().FullName},
            {AdditionalInfoItem.AppDomainName.ToString, AppDomain.CurrentDomain.FriendlyName},
            {AdditionalInfoItem.ThreadIdentity.ToString, My.User.Name} ' Thread.CurrentPrincipal.Identity.Name
            }

        Try
            ' WMI may not be installed on the computer
            Me._additionalInformation.Add(AdditionalInfoItem.WindowsIdentity.ToString, My.Computer.Info.OSFullName)
            Me._additionalInformation.Add(AdditionalInfoItem.OSVersion.ToString, My.Computer.Info.OSVersion)
        Catch
        End Try

    End Sub

#End Region

    ''' <summary> Specifies the contents of the additional information. </summary>
    Private Enum AdditionalInfoItem
        ''' <summary>
        ''' The none
        ''' </summary>
        None
        ''' <summary>
        ''' The machine name
        ''' </summary>
        MachineName
        ''' <summary>
        ''' The timestamp
        ''' </summary>
        Timestamp
        ''' <summary>
        ''' The full name
        ''' </summary>
        FullName
        ''' <summary>
        ''' The application domain name
        ''' </summary>
        AppDomainName
        ''' <summary>
        ''' The thread identity
        ''' </summary>
        ThreadIdentity
        ''' <summary>
        ''' The windows identity
        ''' </summary>
        WindowsIdentity
        ''' <summary>
        ''' The OS version
        ''' </summary>
        OSVersion
    End Enum

End Class
