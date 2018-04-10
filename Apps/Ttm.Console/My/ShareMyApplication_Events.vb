Imports isr.Core.Pith
Imports System.Windows.Forms
Namespace My

    Partial Friend Class MyApplication

#Region " APPLICATION EXTENSIONS "

        Private _MyApplicationInfo As MyAssemblyInfo

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

        ''' <summary> Gets the number of current process threads. </summary>
        ''' <value> The number of current process threads. </value>
        Public Shared ReadOnly Property CurrentProcessThreadCount As Integer
            Get
                Return Process.GetCurrentProcess.Threads.Count
            End Get
        End Property

        ''' <summary> Gets a value indicating whether the application is running under the IDE in design
        ''' mode. </summary>
        ''' <value> <c>True</c> if the application is running under the IDE in design mode; otherwise,
        ''' <c>False</c>. </value>
        Public Shared ReadOnly Property InDesignMode() As Boolean
            Get
                Return Debugger.IsAttached
            End Get
        End Property

#End Region

#Region " SLASH TRACE EVENT "

        ''' <summary> Traces the event and displays on the splash screen if exists. </summary>
        ''' <param name="eventType"> Type of the event. </param>
        ''' <param name="format">    The details. </param>
        ''' <param name="args">      A variable-length parameters list containing arguments. </param>
        Private Sub SplashTraceEvent(ByVal eventType As TraceEventType, ByVal format As String, ByVal ParamArray args() As Object)
            Me.SplashTraceEvent(eventType, MyApplication.TraceEventId,
                                String.Format(Globalization.CultureInfo.CurrentCulture, format, args))
        End Sub

        ''' <summary> Traces the event and displays on the splash screen if exists. </summary>
        ''' <param name="eventType"> Type of the event. </param>
        ''' <param name="details">   The details. </param>
        Private Sub SplashTraceEvent(ByVal eventType As TraceEventType, ByVal details As String)
            Me.SplashTraceEvent(eventType, MyApplication.TraceEventId, details)
        End Sub

        ''' <summary> Traces the event and displays on the splash screen if exists. </summary>
        ''' <param name="eventType"> Type of the event. </param>
        ''' <param name="id">        The identifier. </param>
        ''' <param name="details">   The details. </param>
        Private Sub SplashTraceEvent(ByVal eventType As TraceEventType, ByVal id As Integer, ByVal details As String)
#If SPLASH Then
            MySplashScreen.SplashMessage(details)
#End If
            Me.MyLog.TraceSource.TraceEvent(eventType, id, details)
        End Sub

#End Region

#Region " APPLICATION EVENTS "

        ''' <summary> Occurs when the network connection is connected or disconnected. </summary>
        ''' <param name="sender"> The source of the event. </param>
        ''' <param name="e">      Network available event information. </param>
        Private Sub _NetworkAvailabilityChanged(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs) Handles Me.NetworkAvailabilityChanged
        End Sub

        ''' <summary> Handles the Shutdown event of the MyApplication control. Saves user settings for all
        ''' related libraries. </summary>
        ''' <remarks> This event is not raised if the application terminates abnormally. Application log is
        ''' set at verbose level to log shut down operations. </remarks>
        ''' <param name="sender"> The source of the event. </param>
        ''' <param name="e">      The <see cref="System.EventArgs" /> instance containing the event data. </param>
        <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
        Private Sub _Shutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shutdown

            Try
                Me.processShutDown()
            Catch
            Finally
                Me.MyLog.TraceSource.Flush()
            End Try
        End Sub

        ''' <summary> Occurs when the application starts, before the startup form is created. </summary>
        ''' <param name="sender"> The source of the event. </param>
        ''' <param name="e">      Startup event information. </param>
        Private Sub _Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup

            If e Is Nothing Then Return

            ' Turn on the screen hourglass
            Cursor.Current = Cursors.AppStarting
            Application.DoEvents()

            Try

                Cursor.Current = Cursors.AppStarting

                Trace.CorrelationManager.StartLogicalOperation(Reflection.MethodInfo.GetCurrentMethod.Name)

                Me.ProcessStartup(e)

                If e.Cancel Then

                    ' Show the exception message box with three custom buttons.
                    Cursor.Current = Cursors.Default
                    If isr.Core.MyMessageBox.ShowDialogIgnoreExit(Nothing, "Failed parsing command line.",
                                                         "Failed Starting Program", MessageBoxIcon.Stop) = DialogResult.OK Then
                        Me.SplashTraceEvent(TraceEventType.Error, My.MyApplication.TraceEventId,
                                            "Application aborted by the user because of failure to parse the command line.")
                        e.Cancel = True
                    Else
                        e.Cancel = False
                    End If
                    Cursor.Current = Cursors.AppStarting

                End If

                If Not e.Cancel Then
                    e.Cancel = Not Me.TryInitializeKnownState()
                    If e.Cancel Then
                        isr.Core.MyMessageBox.ShowDialogExit(Nothing, String.Format(
                                                    "Failed initializing application state. Check the program log at '{0}' for additional information.",
                                                     Me.MyLog?.FullLogFileName), "Failed Starting Program", MessageBoxIcon.Stop)
                    End If
                End If

                If e.Cancel Then
                    Me.SplashTraceEvent(TraceEventType.Error, My.MyApplication.TraceEventId, "Application failed to start up.")
                    Me.MyLog.TraceSource.Flush()

                    ' exit with an error code
                    Environment.Exit(-1)
                    Windows.Forms.Application.Exit()
#If Blue_Splash Then
                ElseIf MySplashScreen.IsCloseRequested Then
                    Me.SplashTraceEvent(TraceEventType.Error, My.MyApplication.TraceEventId, "User close requested.")
                    Me.MyLog.TraceSource.Flush()

                    ' exit with an error code
                    Environment.Exit(-1)
                    Windows.Forms.Application.Exit()
#End If
                Else
                    Me.SplashTraceEvent(TraceEventType.Verbose, My.MyApplication.TraceEventId, "Loading application window...")
                End If

            Catch ex As Exception

                Me.SplashTraceEvent(TraceEventType.Error, My.MyApplication.TraceEventId, "Exception occurred starting application.")
                Cursor.Current = Cursors.Default
                Dim owner As IWin32Window = Nothing
                If TypeOf (sender) Is IWin32Window Then
                    owner = TryCast(sender, IWin32Window)
                End If
                Me.MyLog.TraceSource.TraceEvent(ex, My.MyApplication.TraceEventId)
                ex.Data.Add("@isr", "Exception occurred starting this application")
                If DialogResult.Abort = isr.Core.MyMessageBox.ShowDialogAbortIgnore(owner, ex, MessageBoxIcon.Error) Then
                    ' exit with an error code
                    Environment.Exit(-1)
                    Windows.Forms.Application.Exit()
                End If

            Finally

                Cursor.Current = Cursors.Default
                Trace.CorrelationManager.StopLogicalOperation()

            End Try

        End Sub

        ''' <summary> Occurs when launching a single-instance application and the application is already
        ''' active. </summary>
        ''' <param name="sender"> The source of the event. </param>
        ''' <param name="e">      Startup next instance event information. </param>
        Private Sub _StartupNextInstance(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            Me.SplashTraceEvent(TraceEventType.Information, My.MyApplication.TraceEventId, "Application next instant starting.")
        End Sub

        ''' <summary> When overridden in a derived class, allows for code to run when an unhandled
        ''' exception occurs in the application. </summary>
        ''' <param name="e"> <see cref="T:Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs" />. </param>
        ''' <returns> A <see cref="T:System.Boolean" /> that indicates whether the
        ''' <see cref="E:Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase.UnhandledException" />
        ''' event was raised. </returns>
        Protected Overrides Function OnUnhandledException(e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) As Boolean

            Dim returnedValue As Boolean = True
            If e Is Nothing Then
                Debug.Assert(Not Debugger.IsAttached, "Unhandled exception event occurred with event arguments set to nothing.")
                Return MyBase.OnUnhandledException(e)
            End If

            Try
                Me.MyLog.DefaultFileLogWriter.Flush()
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, "Exception occurred flushing the log", "Exception occurred flushing the log: {0}", ex)
            End Try

            Try
                Dim owner As IWin32Window = Nothing
                e.Exception.Data.Add("@isr", "Unhandled Exception Occurred.")
                Me.MyLog.TraceSource.TraceEvent(e.Exception, My.MyApplication.TraceEventId)
                If DialogResult.Abort = isr.Core.MyMessageBox.ShowDialogAbortIgnore(owner, e.Exception, MessageBoxIcon.Error) Then
                    ' exit with an error code
                    Environment.Exit(-1)
                    Windows.Forms.Application.Exit()
                End If
            Catch
                If MessageBox.Show(e.Exception.ToString, "Unhandled Exception occurred.",
                                   MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error,
                                   MessageBoxDefaultButton.Button3, MessageBoxOptions.DefaultDesktopOnly) = Windows.Forms.DialogResult.Abort Then
                End If
            Finally
            End Try
            Return returnedValue

        End Function

        ''' <summary> Occurs when the application initializes. Replaces the default trace listener with the
        ''' modified listener. </summary>
        ''' <param name="commandLineArgs"> A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection" /> 
        '''                                of String, containing the command-line arguments as strings for the current
        ''' application. </param>
        ''' <returns> A <see cref="T:System.Boolean" /> indicating if application startup should continue. </returns>
        Protected Overrides Function OnInitialize(ByVal commandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String)) As Boolean
            Me.CreateLog()
            Return MyBase.OnInitialize(commandLineArgs)
        End Function

#End Region

    End Class

End Namespace
