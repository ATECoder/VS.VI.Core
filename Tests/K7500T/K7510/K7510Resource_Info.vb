Namespace K7500.Tests

    ''' <summary> The 2450 Resource Info. </summary>
    ''' <license>
    ''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
    ''' Licensed under The MIT License.</para><para>
    ''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
    ''' </license>
    ''' <history date="2/12/2018" by="David" revision=""> Created. </history>
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.5.0.0"),
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>
    Friend Class K7510ResourceInfo
        Inherits ApplicationSettingsBase

#Region " SINGLETON "

        ''' <summary>
        ''' Constructor that prevents a default instance of this class from being created.
        ''' </summary>
        Private Sub New()
            MyBase.New
        End Sub

        ''' <summary> Opens the settings editor. </summary>
        Public Shared Sub OpenSettingsEditor()
            Using f As Core.Pith.ConfigurationEditor = Core.Pith.ConfigurationEditor.Get
                f.Text = $"{GetType(K7510ResourceInfo)} Editor"
                f.ShowDialog(K7510ResourceInfo.Get)
            End Using
        End Sub

        ''' <summary> Gets the locking object to enforce thread safety when creating the singleton
        ''' instance. </summary>
        ''' <value> The sync locker. </value>
        Private Shared Property _SyncLocker As New Object

        ''' <summary> Gets the instance. </summary>
        ''' <value> The instance. </value>
        Private Shared Property _Instance As K7510ResourceInfo

        ''' <summary> Instantiates the class. </summary>
        ''' <remarks> Use this property to instantiate a single instance of this class. This class uses
        ''' lazy instantiation, meaning the instance isn't created until the first time it's retrieved. </remarks>
        ''' <returns> A new or existing instance of the class. </returns>
        Public Shared Function [Get]() As K7510ResourceInfo
            If _Instance Is Nothing Then
                SyncLock _SyncLocker
                    _Instance = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New K7510ResourceInfo()), K7510ResourceInfo)
                End SyncLock
            End If
            Return _Instance
        End Function

        ''' <summary> Returns true if an instance of the class was created and not disposed. </summary>
        ''' <value> <c>True</c> if instantiated; otherwise, <c>False</c>. </value>
        Public Shared ReadOnly Property Instantiated() As Boolean
            Get
                SyncLock _SyncLocker
                    Return _Instance IsNot Nothing
                End SyncLock
            End Get
        End Property

#End Region

#Region " CONFIGURATION INFORMATION "

        ''' <summary> Gets the Model of the resource. </summary>
        ''' <value> The Model of the resource. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("True")>
        Public ReadOnly Property Exists As Boolean
            Get
                Return Me.AppSettingBoolean()
            End Get
        End Property

        ''' <summary> Gets the verbose. </summary>
        ''' <value> The verbose. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("False")>
        Public ReadOnly Property Verbose As Boolean
            Get
                Return Me.AppSettingBoolean()
            End Get
        End Property

#End Region

#Region " DEVICE RESOURCE INFORMATION "

        ''' <summary> Gets the Model of the resource. </summary>
        ''' <value> The Model of the resource. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ResourceModel As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        Private _ResourcePinged As Boolean?

        ''' <summary> Gets the resource pinged. </summary>
        ''' <value> The resource pinged. </value>
        Public ReadOnly Property ResourcePinged As Boolean
            Get
                If Not Me._ResourcePinged.HasValue Then
                    Me._ResourcePinged = Not String.IsNullOrWhiteSpace(Me.ResourceName)
                End If
                Return Me._ResourcePinged.Value
            End Get
        End Property

        ''' <summary> Name of the resource. </summary>
        Private _resourceName As String
        Public ReadOnly Property ResourceName As String
            Get
                If String.IsNullOrWhiteSpace(Me._resourceName) Then
                    If VI.Pith.ResourceNamesManager.Ping(Me.FirstResourceName) Then
                        Me._resourceName = Me.FirstResourceName
                    ElseIf VI.Pith.ResourceNamesManager.Ping(Me.SecondResourceName) Then
                        Me._resourceName = Me.SecondResourceName
                    ElseIf VI.Pith.ResourceNamesManager.Ping(Me.ThirdResourceName) Then
                        Me._resourceName = Me.ThirdResourceName
                    ElseIf VI.Pith.ResourceNamesManager.Ping(Me.FourthResourceName) Then
                        Me._resourceName = Me.FourthResourceName
                    Else
                        Me._resourceName = ""
                    End If
                End If
                Return Me._resourceName
            End Get
        End Property

        ''' <summary> Gets the name of the resource. </summary>
        ''' <value> The name of the resource. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property FirstResourceName As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the name of the second resource. </summary>
        ''' <value> The name of the second resource. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property SecondResourceName As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the name of the third resource. </summary>
        ''' <value> The name of the third resource. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ThirdResourceName As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the name of the fourth resource. </summary>
        ''' <value> The name of the fourth resource. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property FourthResourceName As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the Title of the resource. </summary>
        ''' <value> The Title of the resource. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ResourceTitle As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the language. </summary>
        ''' <value> The language. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property Language As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the firmware revision. </summary>
        ''' <value> The firmware revision. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property FirmwareRevision As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

#End Region

    End Class

End Namespace