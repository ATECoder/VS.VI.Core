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
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.5.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
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
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Verbose")>  _
        Public Property TraceLogLevel() As Global.System.Diagnostics.TraceEventType
            Get
                Return CType(Me("TraceLogLevel"),Global.System.Diagnostics.TraceEventType)
            End Get
            Set
                Me("TraceLogLevel") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Verbose")>  _
        Public Property TraceShowLevel() As Global.System.Diagnostics.TraceEventType
            Get
                Return CType(Me("TraceShowLevel"),Global.System.Diagnostics.TraceEventType)
            End Get
            Set
                Me("TraceShowLevel") = value
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
         Global.System.Configuration.DefaultSettingValueAttribute("00:00:00.5000000")>  _
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
         Global.System.Configuration.DefaultSettingValueAttribute("00:00:00.5000000")>  _
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
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.isr.VI.Tsp2.K2450.My.MySettings
            Get
                Return Global.isr.VI.Tsp2.K2450.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
