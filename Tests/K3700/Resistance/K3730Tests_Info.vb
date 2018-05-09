Imports System.Configuration
Imports System.Data.Common
Namespace K3700.Tests

    ''' <summary> A K3730 Test Info. </summary>
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
    Friend Class K3730TestInfo
        Inherits ApplicationSettingsBase

#Region " SINGLETON "

        Private Sub New()
            MyBase.New
        End Sub

        ''' <summary> Opens the settings editor. </summary>
        Public Shared Sub OpenSettingsEditor()
            Using f As Core.Pith.ConfigurationEditor = Core.Pith.ConfigurationEditor.Get
                f.Text = $"{GetType(K3730TestInfo)} Editor"
                f.ShowDialog(K3730TestInfo.Get)
            End Using
        End Sub

        ''' <summary> Gets the locking object to enforce thread safety when creating the singleton
        ''' instance. </summary>
        ''' <value> The sync locker. </value>
        Private Shared Property _SyncLocker As New Object

        ''' <summary> Gets the instance. </summary>
        ''' <value> The instance. </value>
        Private Shared Property _Instance As K3730TestInfo

        ''' <summary> Instantiates the class. </summary>
        ''' <remarks> Use this property to instantiate a single instance of this class. This class uses
        ''' lazy instantiation, meaning the instance isn't created until the first time it's retrieved. </remarks>
        ''' <returns> A new or existing instance of the class. </returns>
        Public Shared Function [Get]() As K3730TestInfo
            If _Instance Is Nothing Then
                SyncLock _SyncLocker
                    _Instance = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New K3730TestInfo()), K3730TestInfo)
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

#Region " MEASURE SUBSYSTEM INFORMATION "

        ''' <summary> Gets the auto zero Enabled settings. </summary>
        ''' <value> The auto zero settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property AutoZeroEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property AutoRangeEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

        ''' <summary> Gets the Sense Function settings. </summary>
        ''' <value> The Sense Function settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property SenseFunction As VI.Tsp.MultimeterFunctionMode
            Get
                Return Me.AppSettingEnum(Of VI.Tsp.MultimeterFunctionMode)
            End Get
        End Property

        ''' <summary> Gets the power line cycles settings. </summary>
        ''' <value> The power line cycles settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property PowerLineCycles As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ShortChannelList As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ResistorChannelList As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property OpenChannelList As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ExpectedResistance As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ExpectedOpen As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ExpectedResistanceEpsilon As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ExpectedZeroResistanceEpsilon As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ExpectedShort32ResistanceEpsilon As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

#End Region

    End Class

End Namespace