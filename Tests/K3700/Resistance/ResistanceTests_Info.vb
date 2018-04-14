Imports System.Configuration
Imports System.Data.Common
Namespace K3700.Tests

    Partial Friend NotInheritable Class Info

#Region " CONSTRUCTORS"

        Private Shared _ResistanceTestInfo As ResistanceTestInfo

        ''' <summary> Gets the Device Test Info. </summary>
        ''' <value> The Device Test Info. </value>
        Public Shared ReadOnly Property ResistanceTestInfo As ResistanceTestInfo
            Get
                If Info._ResistanceTestInfo Is Nothing Then
                    Info._ResistanceTestInfo = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New ResistanceTestInfo()), ResistanceTestInfo)
                End If
                Return Info._ResistanceTestInfo
            End Get
        End Property

#End Region

    End Class

    ''' <summary> A Measure Test Info. </summary>
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
    Friend Class ResistanceTestInfo
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
        Public ReadOnly Property SenseFunctionMode As VI.Tsp.MultimeterFunctionMode
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