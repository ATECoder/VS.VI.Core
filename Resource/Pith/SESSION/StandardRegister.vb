Imports isr.Core.Pith.EnumExtensions
Partial Public Class SessionBase

#Region " STANDARD EVENT REGISTER "

    ''' <summary> Returns a detailed report of the event status register (ESR) byte. </summary>
    ''' <param name="value">     Specifies the value that was read from the status register. </param>
    ''' <param name="delimiter"> The delimiter. </param>
    ''' <returns> Returns a detailed report of the event status register (ESR) byte. </returns>
    Public Shared Function BuildReport(ByVal value As StandardEvents, ByVal delimiter As String) As String

        If String.IsNullOrWhiteSpace(delimiter) Then
            delimiter = "; "
        End If

        Dim builder As New System.Text.StringBuilder

        For Each eventValue As StandardEvents In [Enum].GetValues(GetType(StandardEvents))
            If eventValue <> StandardEvents.None AndAlso eventValue <> StandardEvents.All AndAlso (eventValue And value) <> 0 Then
                If builder.Length > 0 Then
                    builder.Append(delimiter)
                End If
                builder.Append(eventValue.Description)
            End If
        Next

        If builder.Length > 0 Then
            builder.Append(".")
            builder.Insert(0, String.Format(Globalization.CultureInfo.CurrentCulture,
                                            "The device standard status register reported: 0x{0:X2}{1}", value, Environment.NewLine))
        End If
        Return builder.ToString

    End Function

    ''' <summary> The standard event status. </summary>
    Private _StandardEventStatus As StandardEvents?

    ''' <summary> Gets or sets the cached Standard Event enable bit mask. </summary>
    ''' <value> <c>null</c> if value is not known; otherwise <see cref="StandardEvents">Standard
    ''' Events</see>. </value>
    Public Property StandardEventStatus() As StandardEvents?
        Get
            Return Me._StandardEventStatus
        End Get
        Set(ByVal value As StandardEvents?)
            Me._StandardEventStatus = value
            Me.AsyncNotifyPropertyChanged(NameOf(Me.StandardEventStatus))
        End Set
    End Property

    ''' <summary> Gets or sets the standard event query command. </summary>
    ''' <value> The standard event query command. </value>
    ''' <remarks> <see cref="Ieee488.Syntax.StandardEventQueryCommand"></see></remarks>
    Public Property StandardEventQueryCommand As String

    ''' <summary> Queries the Standard Event enable bit mask. </summary>
    ''' <returns> <c>null</c> if value is not known; otherwise <see cref="StandardEvents">Standard
    ''' Events</see>. </returns>
    Public Function QueryStandardEventStatus() As StandardEvents?
        If Not String.IsNullOrWhiteSpace(Me.StandardEventQueryCommand) Then
            Me.StandardEventStatus = CType(Me.Query(0I, Me.StandardEventQueryCommand), StandardEvents?)
        End If
        Return Me.StandardEventStatus
    End Function

#End Region

End Class

