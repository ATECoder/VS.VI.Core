Imports isr.Core.Pith

Namespace My

    Partial Friend Class MyApplication

        ''' <summary> Gets the log. </summary>
        ''' <value> The log. </value>
        Public Overloads ReadOnly Property Log As MyLog = New MyLog

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
            Me.ApplyTraceLevel(isr.VI.Ttm.My.MySettings.Default.TraceLevel)

        End Function

        ''' <summary> Applies the trace level described by value. </summary>
        ''' <param name="value"> The value. </param>
        Public Sub ApplyTraceLevel(ByVal value As TraceEventType)
            Me.Log.ApplyTraceLevel(value)
        End Sub

    End Class


End Namespace

