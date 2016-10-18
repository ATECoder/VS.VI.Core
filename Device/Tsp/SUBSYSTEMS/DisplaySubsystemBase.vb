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

    Protected Overrides ReadOnly Property ClearCommand As String = TspSyntax.Display.ClearCommand

    ''' <summary> Clears the display. </summary>
    ''' <remarks> Sets the display to the user mode. </remarks>
    Public Overrides Sub ClearDisplay()
        Me.DisplayScreen = VI.DisplayScreens.User
        If Me.QueryExists.GetValueOrDefault(False) Then
            Me.Session.WriteLine(TspSyntax.Display.ClearCommand)
        End If
    End Sub

    ''' <summary> Clears the display if not in measurement mode and set measurement mode. </summary>
    ''' <remarks> Sets the display to user. </remarks>
    Public Sub TryClearDisplayMeasurement()
        If Me.Exists AndAlso ((Me.DisplayScreen And DisplayScreens.Measurement) = 0) Then
            Me.TryClearDisplay()
        End If
    End Sub

    ''' <summary> Clears the display if not in measurement mode and set measurement mode. </summary>
    ''' <remarks> Sets the display to the measurement. </remarks>
    Public Sub ClearDisplayMeasurement()
        If Me.QueryExists.GetValueOrDefault(False) AndAlso ((Me.DisplayScreen And DisplayScreens.Measurement) = 0) Then
            Me.ClearDisplay()
        End If
        Me.DisplayScreen = DisplayScreens.Measurement
    End Sub

#End Region

#Region " DISPLAY CHARACTER "

    ''' <summary> Displays the character. </summary>
    ''' <param name="lineNumber">      The line number. </param>
    ''' <param name="position">        The position. </param>
    ''' <param name="characterNumber"> The character number. </param>
    Public Sub DisplayCharacter(ByVal lineNumber As Integer, ByVal position As Integer,
                                ByVal characterNumber As Integer)
        Me.DisplayScreen = DisplayScreens.User Or DisplayScreens.Custom
        If Not Me.QueryExists.GetValueOrDefault(False) Then
            Return
        End If
        ' ignore empty character.
        If characterNumber <= 0 OrElse characterNumber > TspSyntax.Display.MaximumCharacterNumber Then
            Return
        End If
        Me.Session.WriteLine(TspSyntax.Display.SetCursorCommandFormat, lineNumber, position)
        Me.Session.WriteLine(TspSyntax.Display.SetCharacterCommandFormat, characterNumber)
    End Sub

#End Region

#Region " DISPLAY LINE "

    ''' <summary> Displays a message on the display. </summary>
    ''' <param name="lineNumber"> The line number. </param>
    ''' <param name="value">      The value. </param>
    Public Overrides Sub DisplayLine(ByVal lineNumber As Integer, ByVal value As String)

        Me.DisplayScreen = DisplayScreens.User Or DisplayScreens.Custom
        If Not Me.QueryExists.GetValueOrDefault(False) Then
            Return
        End If

        ' ignore empty strings.
        If String.IsNullOrWhiteSpace(value) Then
            Return
        End If

        Dim length As Integer = TspSyntax.Display.FirstLineLength

        If lineNumber < 1 Then
            lineNumber = 1
        ElseIf lineNumber > 2 Then
            lineNumber = 2
        End If
        If lineNumber = 2 Then
            length = TspSyntax.Display.SecondLineLength
        End If

        Me.Session.WriteLine(TspSyntax.Display.SetCursorLineCommandFormat, lineNumber)
        If value.Length < length Then value = value.PadRight(length)
        Me.Session.WriteLine(TspSyntax.Display.SetTextCommandFormat, value)

    End Sub

    ''' <summary> Displays the program title. </summary>
    ''' <param name="title">    Top row data. </param>
    ''' <param name="subtitle"> Bottom row data. </param>
    Public Sub DisplayTitle(ByVal title As String, ByVal subtitle As String)
        Me.DisplayLine(0, title)
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
        Me.DisplayScreen = DisplayScreens.Default
        If Me.Exists.HasValue AndAlso Me.Exists.Value AndAlso Not String.IsNullOrWhiteSpace(Me.RestoreMainScreenWaitCompleteCommand) Then
            Me.StatusSubsystem.EnableWaitComplete()
            ' Documentation error: Display Main equals 1, not 0. This code should work on other instruments.
            Me.Session.WriteLine(Me.RestoreMainScreenWaitCompleteCommand)
            Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        End If
    End Sub

    ''' <summary> Restores the instrument display. </summary>
    Public Sub RestoreDisplay()
        Me.DisplayScreen = DisplayScreens.Default
        If Me.Exists.HasValue AndAlso Me.Exists.Value AndAlso Not String.IsNullOrWhiteSpace(Me.RestoreMainScreenWaitCompleteCommand) Then
            ' Documentation error: Display Main equals 1, not 0. This code should work on other instruments.
            Me.Session.WriteLine(Me.RestoreMainScreenWaitCompleteCommand)
            Me.StatusSubsystem.QueryOperationCompleted()
        End If
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then
    Private _isDisplayExists As Boolean?

    ''' <summary> Gets or sets (Protected) the display existence indicator.
    '''           Some TSP instruments (e.g., 3706) may have no display. </summary>
    ''' <value> The is display exists. </value>
    Public Property IsDisplayExists() As Boolean?
        Get
            Return Me._isDisplayExists
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(value, Me.IsDisplayExists) Then
                Me._isDisplayExists = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.IsDisplayExists))
            End If
        End Set
    End Property


#End If
#End Region