Imports isr.Core.Pith

Namespace My

    Partial Friend Class MyApplication

        Private _myApplicationInfo As MyAssemblyInfo

        ''' <summary> Gets an object that provides information about the application's assembly. </summary>
        ''' <value> The assembly information object. </value>
        Public Overloads ReadOnly Property Info As MyAssemblyInfo
            Get
                If Me._myApplicationInfo Is Nothing Then
                    Me._myApplicationInfo = New MyAssemblyInfo(MyBase.Info)
                End If
                Return Me._myApplicationInfo
            End Get
        End Property

        Private Shared _currentProcessName As String
        ''' <summary> Gets the current process name. </summary>
        Public Shared ReadOnly Property CurrentProcessName() As String
            Get
                If String.IsNullOrWhiteSpace(MyApplication._currentProcessName) Then
                    _currentProcessName = Process.GetCurrentProcess().ProcessName
                End If
                Return _currentProcessName
            End Get
        End Property

        ''' <summary> Gets or sets the log. </summary>
        ''' <value> The log. </value>
        Public ReadOnly Property MyLog As MyLog = New MyLog()

        ''' <summary> Applies the given value. </summary>
        ''' <remarks> David, 3/16/2016. </remarks>
        ''' <param name="value"> The value. </param>
        Public Sub Apply(ByVal value As MyLog)
            Me._MyLog = value
            Global.isr.VI.My.MyLibrary.apply(value)
        End Sub

        ''' <summary> Builds the default caption. </summary>
        ''' <returns> The caption. </returns>
        Friend Function BuildDefaultCaption() As String
            Dim suffix As New System.Text.StringBuilder
            suffix.Append(" ")
            Return isr.Core.Pith.ApplicationInfo.BuildDefaultCaption(suffix.ToString)
        End Function

        ''' <summary> Sets the visual styles, text display styles, and current principal for the main
        ''' application thread (if the application uses Windows authentication), and initializes the
        ''' splash screen, if defined. Replaces the default trace listener with the modified listener.
        ''' Updates the minimum splash screen display time. </summary>
        ''' <param name="commandLineArgs"> A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection" /> of String, 
        '''                                containing the command-line arguments as strings for the current application. </param>
        ''' <returns> A <see cref="T:System.Boolean" /> indicating if application startup should continue. </returns>
        Protected Overrides Function OnInitialize(ByVal commandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String)) As Boolean

            Me.Apply(New MyLog(My.MyApplication.AssemblyProduct))
            Dim listener As Microsoft.VisualBasic.Logging.FileLogTraceListener
            With Me.MyLog
                listener = .ReplaceDefaultTraceListener(UserLevel.AllUsers)
                If Not .LogFileExists Then
                    .TraceSource.TraceEventOverride(TraceEventType.Information, My.MyApplication.TraceEventId,
                                                    "{0} version {1} {2} {3}", My.Application.Info.ProductName,
                                                    My.Application.Info.Version.ToString(4), Date.Now.ToShortDateString(), Date.Now.ToLongTimeString)
                End If
                .TraceSource.TraceEventOverride(TraceEventType.Information, My.MyApplication.TraceEventId, "Process {0} initialized", MyApplication.CurrentProcessName)
            End With

            ' set the log for the application
            With My.Application.Log
                .TraceSource.Listeners.Remove(DefaultFileLogTraceListener.DefaultFileLogWriterName)
                .TraceSource.Listeners.Add(listener)
                .TraceSource.Switch.Level = SourceLevels.Verbose
            End With

            Me.ApplyTraceLevel(isr.VI.Ttm.My.MySettings.Default.TraceLevel)

        End Function

        ''' <summary> Applies the trace level described by value. </summary>
        ''' <param name="value"> The value. </param>
        Public Sub ApplyTraceLevel(ByVal value As TraceEventType)
            Me.MyLog.ApplyTraceLevel(value)
        End Sub

    End Class


End Namespace

