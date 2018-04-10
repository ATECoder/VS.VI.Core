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

        ''' <summary> Applies the default trace level. </summary>
        Friend Sub ApplyTraceLevel()
            MyLog.ApplyTraceLevel(My.MySettings.Default.TraceLevel)
        End Sub

        ''' <summary> Instantiates the application to its known state. </summary>
        ''' <returns> <c>True</c> if success or <c>False</c> if failed. </returns>
        <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
        Private Function TryinitializeKnownState() As Boolean

            Dim affirmative As Boolean = True
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

            Catch ex As Exception

                ' Turn off the hourglass
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                Me.SplashTraceEvent(TraceEventType.Error, "Exception occurred initializing application known state;. {0}", ex.ToFullBlownString)
                affirmative = False

            Finally

                ' Turn off the hourglass
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

            End Try
            Return affirmative

        End Function

        ''' <summary> Processes the shut down. </summary>
        <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
        Private Sub ProcessShutDown()
            Try
                If My.Application.SaveMySettingsOnExit Then
                    Me.MyLog.TraceSource.TraceEventOverride(TraceEventType.Verbose, My.MyApplication.TraceEventId,
                                                            "Saving assembly settings")
                    ' Save library settings here
                End If
            Catch
            Finally
            End Try


        End Sub

        ''' <summary> Processes the startup. Sets the event arguments
        ''' <see cref="Microsoft.VisualBasic.ApplicationServices.StartupEventArgs.Cancel">cancel</see>
        ''' value if failed. </summary>
        ''' <param name="e"> The <see cref="Microsoft.VisualBasic.ApplicationServices.StartupEventArgs" />
        ''' instance containing the event data. </param>
        Private Sub ProcessStartup(ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs)
            If e IsNot Nothing Then
                Me.SplashTraceEvent(TraceEventType.Verbose, My.MyApplication.TraceEventId, "Parsing command line")
                e.Cancel = Not CommandLineInfo.TryParseCommandLine(e.CommandLine)
            End If
        End Sub

    End Class

End Namespace

