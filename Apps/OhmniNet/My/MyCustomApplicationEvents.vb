Imports isr.Core.Pith
Imports isr.VI.ExceptionExtensions
Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Builds the default caption. </summary>
        ''' <returns> The caption. </returns>
        Friend Function BuildDefaultCaption() As String
            Dim suffix As New System.Text.StringBuilder
            suffix.Append(" ")
            Return isr.Core.Pith.ApplicationInfo.BuildApplicationTitleCaption(suffix.ToString)
        End Function

        ''' <summary> Destroys objects for this project. </summary>
        Friend Sub Destroy()
            MySplashScreen.Close()
            MySplashScreen.Dispose()
            Me.SplashScreen = Nothing
        End Sub

        ''' <summary> Instantiates the application to its known state. </summary>
        ''' <returns> <c>True</c> if success or <c>False</c> if failed. </returns>
        <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
        Private Function TryinitializeKnownState() As Boolean

            Try

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.AppStarting

                ' show status
                If My.MyApplication.InDesignMode Then
                    Me.SplashTraceEvent(TraceEventType.Verbose, "Application is initializing. Design Mode.")
                Else
                    Me.SplashTraceEvent(TraceEventType.Verbose, "Application is initializing. Runtime Mode.")
                End If

                ' Apply command line results.
                If CommandLineInfo.DevicesEnabled.HasValue Then
                    Me.SplashTraceEvent(TraceEventType.Information, "{0} use of devices from command line",
                                  IIf(CommandLineInfo.DevicesEnabled.Value, "Enabled", "Disabling"))
                    My.Settings.DevicesEnabled = CommandLineInfo.DevicesEnabled.Value
                End If

                Return True

            Catch ex As Exception

                ' Turn off the hourglass
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                Me.SplashTraceEvent(TraceEventType.Error, "Exception occurred initializing application known state;. {0}", ex.ToFullBlownString)
                Try
                    Me.Destroy()
                Finally
                End Try
                Return False
            Finally

                ' Turn off the hourglass
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

            End Try

        End Function

        ''' <summary> Processes the shut down. </summary>
        Private Sub ProcessShutDown()
            My.Application.SaveMySettingsOnExit = True
            If My.Application.SaveMySettingsOnExit Then
                ' Save library settings here
            End If
        End Sub

        ''' <summary> Gets or sets the log. </summary>
        ''' <value> The log. </value>
        Public ReadOnly Property MyLog As MyLog = New MyLog()

        ''' <summary> Applies the given value. </summary>
        ''' <param name="value"> The value. </param>
        Public Sub Apply(ByVal value As MyLog)
            Me._MyLog = value
            Global.isr.VI.My.MyLibrary.Apply(value)
        End Sub

        ''' <summary> Processes the startup. Sets the event arguments
        ''' <see cref="Microsoft.VisualBasic.ApplicationServices.StartupEventArgs.Cancel">cancel</see>
        ''' value if failed. </summary>
        ''' <param name="e"> The <see cref="Microsoft.VisualBasic.ApplicationServices.StartupEventArgs" />
        ''' instance containing the event data. </param>
        Private Sub ProcessStartup(ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs)
            If Not e.Cancel Then
                MySplashScreen.CreateInstance(Me.SplashScreen)
                Me.SplashTraceEvent(TraceEventType.Verbose, My.MyApplication.TraceEventId, "Allowing library use of splash screen")
                Me.SplashTraceEvent(TraceEventType.Verbose, My.MyApplication.TraceEventId, "Parsing command line")
                e.Cancel = Not CommandLineInfo.TryParseCommandLine(e.CommandLine)
            End If
        End Sub

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

            Me.ApplyTraceLevel(My.Settings.TraceLevel)

            Me.MinimumSplashScreenDisplayTime = My.Settings.MinimumSplashScreenDisplayMilliseconds
            Return MyBase.OnInitialize(commandLineArgs)

        End Function

        ''' <summary> Applies the trace level described by value. </summary>
        ''' <param name="value"> The value. </param>
        Public Sub ApplyTraceLevel(ByVal value As TraceEventType)
            My.Application.MyLog.TraceSource.ApplyTraceLevel(value)
        End Sub

    End Class


End Namespace

