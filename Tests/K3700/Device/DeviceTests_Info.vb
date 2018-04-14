Namespace K3700.Tests

    Partial Friend NotInheritable Class Info

#Region " CONSTRUCTORS"

        Private Shared _DeviceTestInfo As DeviceTestInfo

        ''' <summary> Gets the Device Test Info. </summary>
        ''' <value> The Device Test Info. </value>
        Public Shared ReadOnly Property DeviceTestInfo As DeviceTestInfo
            Get
                If Info._DeviceTestInfo Is Nothing Then
                    Info._DeviceTestInfo = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New DeviceTestInfo()), DeviceTestInfo)
                End If
                Return Info._DeviceTestInfo
            End Get
        End Property

#End Region

    End Class

    ''' <summary> A Device Test Info. </summary>
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
    Friend Class DeviceTestInfo
        Inherits ApplicationSettingsBase

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
                    If VI.Pith.ResourceNamesManager.Ping(Me.LocalResourceName) Then
                        Me._resourceName = Me.LocalResourceName
                    ElseIf VI.Pith.ResourceNamesManager.Ping(Me.RemoteResourceName) Then
                        Me._resourceName = Me.RemoteResourceName
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
        Public ReadOnly Property LocalResourceName As String
            Get
                Return Me.AppSettingValue
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property RemoteResourceName As String
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

#Region " SOURCE MEASURE UNIT INFORMATION "

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

#Region " MEASURE SUBSYSTEM INFORMATION "

        ''' <summary> Gets the Initial auto Delay Enabled settings. </summary>
        ''' <value> The auto Delay settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialAutoDelayEnabled As Boolean
            Get
                Return Me.AppSettingBoolean
            End Get
        End Property

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

        Public ReadOnly Property InitialSenseFunctionMode As VI.Tsp.MultimeterFunctionMode
            Get
                Return CType(Me.InitialSenseFunction, VI.Tsp.MultimeterFunctionMode)
            End Get
        End Property

        ''' <summary> Gets the Initial Sense Function settings. </summary>
        ''' <value> The Sense Function settings. </value>
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public ReadOnly Property InitialSenseFunction As Integer
            Get
                Return Me.AppSettingInt32
            End Get
        End Property

#End Region

    End Class

End Namespace