''' <summary> Defines a System Subsystem for a TSP System. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/7/2013" by="David" revision=""> Created. </history>
Public MustInherit Class DisplaySubsystemBase
    Inherits VI.DisplaySubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystemBase">status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.RestoreMainScreenWaitCompleteCommand = TspSyntax.Display.RestoreMainWaitCompleteCommand
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " DISPLAY SCREEN  "

    ''' <summary> Gets the display Screen command format. </summary>
    ''' <value> The display Screen command format. </value>
    Protected Overrides ReadOnly Property DisplayScreenCommandFormat As String = TspSyntax.Display.DisplayScreenCommandFormat

    ''' <summary> Gets the display Screen query command. </summary>
    ''' <value> The display Screen query command. </value>
    Protected Overrides ReadOnly Property DisplayScreenQueryCommand As String = ""

#End Region

#Region " ENABLED "

    ''' <summary> Gets the display enable command format. </summary>
    ''' <value> The display enable command format. </value>
    Protected Overrides ReadOnly Property DisplayEnableCommandFormat As String = ""

    ''' <summary> Gets the display enabled query command. </summary>
    ''' <value> The display enabled query command. </value>
    Protected Overrides ReadOnly Property DisplayEnabledQueryCommand As String = ""

#End Region

#End Region

#Region " EXISTS "

    ''' <summary> Reads the display existence indicator.
    '''           Some TSP instruments (e.g., 3706) may have no display. </summary>
    ''' <returns> <c>True</c> if the display exists; otherwise, <c>False</c>. </returns>
    Public Overrides Function QueryExists() As Boolean?
        ' detect the display
        Me.Session.MakeEmulatedReplyIfEmpty(Me.Exists.GetValueOrDefault(True))
        Me.Exists = Not Me.Session.IsNil(TspSyntax.Display.SubsystemName)
        Return Me.Exists
    End Function

#End Region

#Region " CLEAR "

    ''' <summary> Gets or sets the default display screen. </summary>
    ''' <value> The default display. </value>
    Public Property DefaultScreen As VI.DisplayScreens = VI.DisplayScreens.User

    Protected Overrides ReadOnly Property ClearCommand As String = TspSyntax.Display.ClearCommand

    ''' <summary> Clears the display. </summary>
    ''' <remarks> Sets the display to the user mode. </remarks>
    Public Overrides Sub ClearDisplay()
        Me.DisplayScreen = Me.DefaultScreen
        MyBase.ClearDisplay()
    End Sub

    ''' <summary> Clears the display if not in measurement mode and set measurement mode. </summary>
    ''' <remarks> Sets the display to user. </remarks>
    Public Sub TryClearDisplayMeasurement()
        If Me.Exists AndAlso ((Me.DisplayScreen And DisplayScreens.Measurement) = 0) Then
            Me.TryClearDisplay()
        End If
    End Sub

    ''' <summary> Gets or sets the measurement screen. </summary>
    ''' <value> The measurement screen. </value>
    Public Property MeasurementScreen As VI.DisplayScreens = VI.DisplayScreens.Measurement

    ''' <summary> Clears the display if not in measurement mode and set measurement mode. </summary>
    ''' <remarks> Sets the display to the measurement. </remarks>
    Public Sub ClearDisplayMeasurement()
        If Me.QueryExists.GetValueOrDefault(False) AndAlso ((Me.DisplayScreen And DisplayScreens.Measurement) = 0) Then
            Me.ClearDisplay()
        End If
        Me.DisplayScreen = Me.MeasurementScreen
    End Sub

#End Region

#Region " USER SCREEN TEXT "

    ''' <summary> Gets or sets the user screen. </summary>
    ''' <value> The user screen. </value>
    Public Property UserScreen As VI.DisplayScreens = VI.DisplayScreens.UserSwipe

    ''' <summary> Displays a message on the display. </summary>
    ''' <param name="lineNumber"> The line number. </param>
    ''' <param name="value">      The value. </param>
    Public Overrides Sub DisplayLine(ByVal lineNumber As Integer, ByVal value As String)
        If Me.QueryExists.GetValueOrDefault(False) AndAlso Not String.IsNullOrWhiteSpace(value) Then
            lineNumber = Math.Max(1, Math.Min(2, lineNumber))
            Dim length As Integer = If(lineNumber = 1, TspSyntax.Display.FirstLineLength, TspSyntax.Display.SecondLineLength)
            If value.Length < length Then value = value.PadRight(length)
            Me.Write(TspSyntax.Display.SetTextLineCommandFormat, lineNumber, value)
            Me.DisplayScreen = Me.UserScreen
        End If

    End Sub

    ''' <summary> Displays the program title. </summary>
    ''' <param name="title">    Top row data. </param>
    ''' <param name="subtitle"> Bottom row data. </param>
    Public Sub DisplayTitle(ByVal title As String, ByVal subtitle As String)
        Me.DisplayLine(1, title)
        Me.DisplayLine(2, subtitle)
    End Sub

#End Region

#Region " RESTORE "

    ''' <summary> Gets or sets the restore display command. </summary>
    ''' <value> The restore display command. </value>
    Public Property RestoreMainScreenWaitCompleteCommand As String

    ''' <summary> Restores the instrument display. </summary>
    ''' <param name="timeout"> The timeout. </param>
    Public Sub RestoreDisplay(ByVal timeout As TimeSpan)
        Me.DisplayScreen = VI.DisplayScreens.Default
        If Me.Exists.HasValue AndAlso Me.Exists.Value AndAlso Not String.IsNullOrWhiteSpace(Me.RestoreMainScreenWaitCompleteCommand) Then
            Me.StatusSubsystem.EnableWaitComplete()
            ' Documentation error: Display Main equals 1, not 0. This code should work on other instruments.
            Me.Session.WriteLine(Me.RestoreMainScreenWaitCompleteCommand)
            Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        End If
    End Sub

    ''' <summary> Restores the instrument display. </summary>
    Public Sub RestoreDisplay()
        Me.DisplayScreen = VI.DisplayScreens.Default
        If Me.Exists.HasValue AndAlso Me.Exists.Value AndAlso Not String.IsNullOrWhiteSpace(Me.RestoreMainScreenWaitCompleteCommand) Then
            ' Documentation error: Display Main equals 1, not 0. This code should work on other instruments.
            Me.Session.WriteLine(Me.RestoreMainScreenWaitCompleteCommand)
            Me.StatusSubsystem.QueryOperationCompleted()
        End If
    End Sub

#End Region

End Class

