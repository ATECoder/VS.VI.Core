Imports isr.Core.Pith
Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Builds the default caption. </summary>
        ''' <returns> The caption. </returns>
        Friend Function BuildDefaultCaption() As String
            Dim suffix As New System.Text.StringBuilder
            suffix.Append(" ")
            Return isr.Core.Pith.ApplicationInfo.BuildDefaultCaption(suffix.ToString)
        End Function

        ''' <summary> Destroys objects for this project. </summary>
        Friend Sub Destroy()
#If SPLASH Then
            MySplashScreen.Close()
            MySplashScreen.Dispose()
#End If
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
                Me.SplashTraceEvent(TraceEventType.Error, "Exception occurred initializing application known state;. Details: {0}", ex)
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
        Private Sub processShutDown()

            My.Application.SaveMySettingsOnExit = True
            If My.Application.SaveMySettingsOnExit Then
                ' Save library settings here
            End If
        End Sub

        ''' <summary> Processes the startup. Sets the event arguments
        ''' <see cref="Microsoft.VisualBasic.ApplicationServices.StartupEventArgs.Cancel">cancel</see>
        ''' value if failed. </summary>
        ''' <param name="e"> The <see cref="Microsoft.VisualBasic.ApplicationServices.StartupEventArgs" />
        ''' instance containing the event data. </param>
        Private Sub ProcessStartup(ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs)
            If Not e.Cancel Then
#If SPLASH Then
                MySplashScreen.CreateInstance(My.Application.SplashScreen)
                Me.SplashTraceEvent(TraceEventType.Verbose, "Using splash panel.")
#End If
                Me.SplashTraceEvent(TraceEventType.Information, "Parsing command line")
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

            With Me.Log
                .ReplaceDefaultTraceListener(UserLevel.AllUsers)
                If .LogFileExists Then
                    .TraceSource.TraceEventOverride(TraceEventType.Information, My.MyApplication.TraceEventId, "Application initialized")
                Else
                    .TraceSource.TraceEventOverride(TraceEventType.Information, My.MyApplication.TraceEventId,
                                                    "{0} version {1} {2} {3}", My.Application.Info.ProductName,
                                                    My.Application.Info.Version.ToString(4), Date.Now.ToShortDateString(), Date.Now.ToLongTimeString)
                End If
            End With
            Me.ApplyTraceLevel(My.Settings.TraceLevel)

#If SPLASH Then
            ' Set the display time to value from the settings class.
            Me.MinimumSplashScreenDisplayTime = My.Settings.MinimumSplashScreenDisplayMilliseconds
#End If

            Return MyBase.OnInitialize(commandLineArgs)

        End Function

        ''' <summary> Applies the trace level described by value. </summary>
        ''' <param name="value"> The value. </param>
        Public Sub ApplyTraceLevel(ByVal value As TraceEventType)
            Me.Log.ApplyTraceLevel(value)
        End Sub

    End Class


End Namespace

