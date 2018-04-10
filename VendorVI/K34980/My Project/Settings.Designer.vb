﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Namespace My

    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0"),
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>
    Partial Public NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase

        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()), MySettings)

#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(sender As Global.System.Object, e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region

        Public Shared ReadOnly Property [Default]() As MySettings
            Get

#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("10")>
        Public Property LoadResistance() As Decimal
            Get
                Return CType(Me("LoadResistance"), Decimal)
            End Get
            Set
                Me("LoadResistance") = Value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("5")>
        Public Property YardstickResistance() As Decimal
            Get
                Return CType(Me("YardstickResistance"), Decimal)
            End Get
            Set
                Me("YardstickResistance") = Value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("0.01")>
        Public Property YardstickResistanceTolerance() As Decimal
            Get
                Return CType(Me("YardstickResistanceTolerance"), Decimal)
            End Get
            Set
                Me("YardstickResistanceTolerance") = Value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("0.000001")>
        Public Property YardstickInductanceLimit() As Decimal
            Get
                Return CType(Me("YardstickInductanceLimit"), Decimal)
            End Get
            Set
                Me("YardstickInductanceLimit") = Value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public Property OpenCompensationReading() As String
            Get
                Return CType(Me("OpenCompensationReading"), String)
            End Get
            Set
                Me("OpenCompensationReading") = Value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public Property ShortCompensationReading() As String
            Get
                Return CType(Me("ShortCompensationReading"), String)
            End Get
            Set
                Me("ShortCompensationReading") = Value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public Property LoadCompensationReading() As String
            Get
                Return CType(Me("LoadCompensationReading"), String)
            End Get
            Set
                Me("LoadCompensationReading") = Value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public Property FrequencyArrayReading() As String
            Get
                Return CType(Me("FrequencyArrayReading"), String)
            End Get
            Set
                Me("FrequencyArrayReading") = Value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("")>
        Public Property AdapterType() As String
            Get
                Return CType(Me("AdapterType"), String)
            End Get
            Set
                Me("AdapterType") = Value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("Warning")>
        Public Property TraceLogLevel() As Global.System.Diagnostics.TraceEventType
            Get
                Return CType(Me("TraceLogLevel"), Global.System.Diagnostics.TraceEventType)
            End Get
            Set
                Me("TraceLogLevel") = Value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("Warning")>
        Public Property TraceShowLevel() As Global.System.Diagnostics.TraceEventType
            Get
                Return CType(Me("TraceShowLevel"), Global.System.Diagnostics.TraceEventType)
            End Get
            Set
                Me("TraceShowLevel") = Value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("00:00:10")>  _
        Public Property InitializeTimeout() As Global.System.TimeSpan
            Get
                Return CType(Me("InitializeTimeout"),Global.System.TimeSpan)
            End Get
            Set
                Me("InitializeTimeout") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("00:00:01.0500000")>  _
        Public Property DeviceClearRefractoryPeriod() As Global.System.TimeSpan
            Get
                Return CType(Me("DeviceClearRefractoryPeriod"),Global.System.TimeSpan)
            End Get
            Set
                Me("DeviceClearRefractoryPeriod") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("00:00:00.2000000")>  _
        Public Property ResetRefractoryPeriod() As Global.System.TimeSpan
            Get
                Return CType(Me("ResetRefractoryPeriod"),Global.System.TimeSpan)
            End Get
            Set
                Me("ResetRefractoryPeriod") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("00:00:00.1000000")>  _
        Public Property InitRefractoryPeriod() As Global.System.TimeSpan
            Get
                Return CType(Me("InitRefractoryPeriod"),Global.System.TimeSpan)
            End Get
            Set
                Me("InitRefractoryPeriod") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("00:00:00.1000000")>  _
        Public Property ClearRefractoryPeriod() As Global.System.TimeSpan
            Get
                Return CType(Me("ClearRefractoryPeriod"),Global.System.TimeSpan)
            End Get
            Set
                Me("ClearRefractoryPeriod") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(),
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
         Global.System.Configuration.DefaultSettingValueAttribute("0")>
        Public Property SessionMessageNotificationLevel() As Integer
            Get
                Return CType(Me("SessionMessageNotificationLevel"), Integer)
            End Get
            Set
                Me("SessionMessageNotificationLevel") = Value
            End Set
        End Property
    End Class
End Namespace

Namespace My

    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>
    Friend Module MySettingsProperty
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>
        Friend ReadOnly Property Settings() As Global.isr.VI.K34980.My.MySettings
            Get
                Return Global.isr.VI.K34980.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
