Namespace K7500.Tests

    ''' <summary> The Subsystems Test Information. </summary>
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
    Friend Class K7510SubsystemsInfo
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
                f.Text = $"{GetType(K7510SubsystemsInfo)} Editor"
                f.ShowDialog(K7510SubsystemsInfo.Get)
            End Using
        End Sub

        ''' <summary> Gets the locking object to enforce thread safety when creating the singleton
        ''' instance. </summary>
        ''' <value> The sync locker. </value>
        Private Shared Property _SyncLocker As New Object

        ''' <summary> Gets the instance. </summary>
        ''' <value> The instance. </value>
        Private Shared Property _Instance As K7510SubsystemsInfo

        ''' <summary> Instantiates the class. </summary>
        ''' <remarks> Use this property to instantiate a single instance of this class. This class uses
        ''' lazy instantiation, meaning the instance isn't created until the first time it's retrieved. </remarks>
        ''' <returns> A new or existing instance of the class. </returns>
        Public Shared Function [Get]() As K7510SubsystemsInfo
            If _Instance Is Nothing Then
                SyncLock _SyncLocker
                    _Instance = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New K7510SubsystemsInfo()), K7510SubsystemsInfo)
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

#Region " DEVICE SESSION INFORMATION "

        ''' <summary> Gets the keep alive query command. </summary>
        ''' <value> The keep alive query command. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property KeepAliveQueryCommand As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the keep alive command. </summary>
        ''' <value> The keep alive command. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property KeepAliveCommand As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the read termination enabled. </summary>
        ''' <value> The read termination enabled. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ReadTerminationEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

        ''' <summary> Gets the termination character. </summary>
        ''' <value> The termination character. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property TerminationCharacter As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

#End Region

#Region " STATUS SUBSYSTEM INFORMATION "

        ''' <summary> Gets the Initial power line cycles settings. </summary>
        ''' <value> The power line cycles settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialPowerLineCycles As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

#End Region

#Region " DEVICE ERRORS "

        ''' <summary> Gets the erroneous command. </summary>
        ''' <value> The erroneous command. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ErroneousCommand As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets the error available milliseconds delay. </summary>
        ''' <value> The error available milliseconds delay. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ErrorAvailableMillisecondsDelay As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ExpectedCompoundErrorMessage As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        ''' <summary> Gets a message describing the expected error. </summary>
        ''' <value> A message describing the expected error. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ExpectedErrorMessage As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ExpectedErrorNumber As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property ExpectedErrorLevel As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

#End Region

#Region " LOCAL NODE INFORMATION "

        ''' <summary> Gets the maximum output power of the instrument. </summary>
        ''' <value> The maximum output power . </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property MaximumOutputPower As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

        ''' <summary> Gets the line frequency. </summary>
        ''' <value> The line frequency. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property LineFrequency As Double
            Get
                Return Me.AppSettingDouble
            End Get
        End Property

#End Region

#Region " MULTIMETER SUBSYSTEM INFORMATION "

        ''' <summary> Gets the Initial auto Range enabled settings. </summary>
        ''' <value> The auto Range settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialAutoRangeEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

        ''' <summary> Gets the Initial auto zero Enabled settings. </summary>
        ''' <value> The auto zero settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialAutoZeroEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

        ''' <summary> Gets the initial sense function mode. </summary>
        ''' <value> The initial sense function mode. </value>
        Public ReadOnly Property InitialSenseFunctionMode As Tsp2.MultimeterFunctionMode
            Get
                Return CType(Me.InitialMultimeterFunction, Tsp2.MultimeterFunctionMode)
            End Get
        End Property

        ''' <summary> Gets the Initial Sense Function settings. </summary>
        ''' <value> The Sense Function settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialMultimeterFunction As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

#End Region

#Region " BUFFER SUBSYSTEM INFORMATION "

        ''' <summary> Gets the buffer capacity. </summary>
        ''' <value> The buffer capacity. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property BufferCapacity As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

        ''' <summary> Gets the buffer first point number. </summary>
        ''' <value> The buffer first point number. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property BufferFirstPointNumber As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

        ''' <summary> Gets the buffer last point number. </summary>
        ''' <value> The buffer last point number. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property BufferLastPointNumber As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

        ''' <summary> Gets the buffer fill once enabled. </summary>
        ''' <value> The buffer fill once enabled. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property BufferFillOnceEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

#End Region

    End Class

End Namespace