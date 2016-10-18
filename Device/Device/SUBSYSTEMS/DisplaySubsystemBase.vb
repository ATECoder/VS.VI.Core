''' <summary> Defines a Scpi Display Subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created </history>
Public MustInherit Class DisplaySubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " CLEAR CAUTION MESSAGES (IMPEDANCE METER)"

    ''' <summary> Gets the clear caution messages command. </summary>
    ''' <value> The Abort command. </value>
    ''' <remarks> SCPI: ":DISP:CCL". </remarks>
    Protected Overridable ReadOnly Property ClearCautionMessagesCommand As String

    ''' <summary> Clears the caution messages. </summary>
    ''' <remarks> David, 7/6/2016. </remarks>
    Public Sub ClearCautionMessages()
        If Not String.IsNullOrWhiteSpace(Me.ClearCautionMessagesCommand) Then
            Me.Session.WriteLine(Me.ClearCautionMessagesCommand)
        End If
    End Sub

#End Region

#Region " CLEAR DISPLAY "

    ''' <summary> Gets the clear command. </summary>
    ''' <remarks> SCPI: ":DISP:CLE". </remarks>
    ''' <value> The clear command. </value>
    Protected Overridable ReadOnly Property ClearCommand As String

    ''' <summary> Clears the triggers. </summary>
    ''' <remarks> David, 3/10/2016. </remarks>
    Public Overridable Sub ClearDisplay()
        If Me.QueryExists.GetValueOrDefault(False) Then
            Me.Write(Me.ClearCommand)
        End If
    End Sub

    ''' <summary> Clears the display without raising exceptions. </summary>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryClearDisplay() As Boolean
        Dim affirmative As Boolean = True
        Try
            Me.ClearDisplay()
            affirmative = Me.TraceVisaDeviceOperationOkay(False, "clearing display;. ")
        Catch ex As NativeException
            Me.TraceVisaOperation(ex, "clearing display;. ")
            affirmative = False
        Catch ex As Exception
            Me.TraceOperation(ex, "clearing display;. ")
            affirmative = False
        End Try
        Return affirmative
    End Function

#End Region

#Region " DISPLAY LINE "

    ''' <summary> Gets the Display Line command. </summary>
    ''' <remarks> SCPI: ":DISP:USER{0}:TEXT ""{1}""". </remarks>
    ''' <value> The DisplayText command. </value>
    Protected Overridable ReadOnly Property DisplayLineCommandFormat As String

    ''' <summary> Displays a line of text. </summary>
    ''' <remarks> David, 10/15/2016. </remarks>
    ''' <param name="lineNumber"> The line number. </param>
    Public Overridable Sub DisplayLine(ByVal lineNumber As Integer, ByVal value As String)
        If Not String.IsNullOrWhiteSpace(Me.DisplayLineCommandFormat) Then
            Me.Write(String.Format(Me.DisplayLineCommandFormat, lineNumber, value))
        End If
    End Sub

    ''' <summary> Displays a line of text. </summary>
    ''' <remarks> David, 10/17/2016. </remarks>
    ''' <param name="lineNumber"> The line number. </param>
    ''' <param name="format">     Describes the format to use. </param>
    ''' <param name="args">       A variable-length parameters list containing arguments. </param>
    Public Overridable Sub DisplayLine(ByVal lineNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object)
        Me.DisplayLine(lineNumber, String.Format(format, args))
    End Sub


#End Region

#Region " ENABLED "

    ''' <summary> Enabled. </summary>
    Private _Enabled As Boolean?

    ''' <summary> Gets or sets the cached Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Enabled As Boolean?
        Get
            Return Me._Enabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Enabled, value) Then
                Me._Enabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Enabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteEnabled(value)
        Return Me.QueryEnabled()
    End Function

    ''' <summary> Gets or sets the display enabled query command. </summary>
    ''' <value> The display enabled query command. </value>
    Protected Overridable ReadOnly Property DisplayEnabledQueryCommand As String

    ''' <summary> Queries the Enabled sentinel. Also sets the
    '''           <see cref="Enabled">enabled</see> sentinel. </summary>
    ''' <returns> <c>null</c> display status is not known; <c>True</c> if enabled; otherwise, <c>False</c>. </returns>
    Public Function QueryEnabled() As Boolean?
        Me.Session.MakeEmulatedReplyIfEmpty(Me.Enabled.GetValueOrDefault(True))
        If String.IsNullOrWhiteSpace(Me.DisplayEnabledQueryCommand) Then
            Me.Enabled = Me.Session.Query(Me.Enabled.GetValueOrDefault(True), Me.DisplayEnabledQueryCommand)
        End If
        Return Me.Enabled
    End Function

    ''' <summary> Gets or sets the display enable command format. </summary>
    ''' <value> The display enable command format. </value>
    Protected Overridable ReadOnly Property DisplayEnableCommandFormat As String

    ''' <summary> Writes the Forced Digital Output Pattern  Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>null</c> if display status is not known; <c>True</c> if enabled; otherwise, <c>False</c>. </returns>
    Public Function WriteEnabled(ByVal value As Boolean) As Boolean?
        If Not String.IsNullOrWhiteSpace(Me.DisplayEnableCommandFormat) Then
            Me.Session.WriteLine(Me.DisplayEnableCommandFormat, CType(value, Integer))
        End If
        Me.Enabled = value
        Return Me.Enabled
    End Function

#End Region

#Region " EXISTS "

    Private _Exists As Boolean?

    ''' <summary> Gets or sets the cached Exists sentinel. </summary>
    ''' <value> <c>null</c> if display exists is not known; <c>True</c> if display exists is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Exists As Boolean?
        Get
            Return Me._Exists
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Exists, value) Then
                Me._Exists = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Exists))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the display exists query command. </summary>
    ''' <value> The display Installed query command. </value>
    Protected Overridable ReadOnly Property DisplayExistsQueryCommand As String

    ''' <summary> Queries the display existence sentinel. Also sets the 
    '''           <see cref="Exists">installed</see> sentinel. </summary>
    ''' <returns> <c>null</c> status is not known; <c>True</c> if display exists; otherwise, <c>False</c>. </returns>
    Public Overridable Function QueryExists() As Boolean?
        Me.Session.MakeEmulatedReplyIfEmpty(Me.Exists.GetValueOrDefault(True))
        If String.IsNullOrWhiteSpace(Me.DisplayExistsQueryCommand) Then
            Me.Exists = Me.Session.Query(Me.Exists.GetValueOrDefault(True), Me.DisplayExistsQueryCommand)
        End If
        Return Me.Exists
    End Function

#End Region

#Region " DISPLAY SCREEN  "

    ''' <summary> The Display Screen. </summary>
    Private _DisplayScreen As DisplayScreens?

    ''' <summary> Gets or sets the cached Display Screen. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property DisplayScreen As DisplayScreens?
        Get
            Return Me._DisplayScreen
        End Get
        Protected Set(ByVal value As DisplayScreens?)
            If Not Nullable.Equals(Me.DisplayScreen, value) Then
                Me._DisplayScreen = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.DisplayScreen))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Display Screen. </summary>
    ''' <param name="value"> The Display Screen. </param>
    ''' <returns> The Display Screen. </returns>
    Public Function ApplyDisplayScreen(ByVal value As DisplayScreens) As DisplayScreens?
        Me.WriteDisplayScreens(value)
        Return Me.QueryDisplayScreens
    End Function

    ''' <summary> Gets or sets the Display Screen  query command. </summary>
    ''' <value> The Display Screen  query command. </value>
    Protected Overridable ReadOnly Property DisplayScreenQueryCommand As String

    ''' <summary> Queries the Display Screen. </summary>
    ''' <returns> The Display Screen  or none if unknown. </returns>
    Public Function QueryDisplayScreens() As DisplayScreens?
        Me.DisplayScreen = Me.Query(Me.DisplayScreenQueryCommand, Me.DisplayScreen)
        Return Me.DisplayScreen
    End Function

    ''' <summary> Gets or sets the Display Screen  command format. </summary>
    ''' <value> The Display Screen  command format. </value>
    Protected Overridable ReadOnly Property DisplayScreenCommandFormat As String

    ''' <summary> Writes the Display Screen  without reading back the value from the device. </summary>
    ''' <remarks> This command sets the Display Screen. </remarks>
    ''' <param name="value"> The Display Screen. </param>
    ''' <returns> The Display Screen. </returns>
    Public Function WriteDisplayScreens(ByVal value As DisplayScreens) As DisplayScreens?
        Me.DisplayScreen = Me.Write(Me.DisplayScreenCommandFormat, value)
        Return Me.DisplayScreen
    End Function

#End Region

End Class

''' <summary> A bit-field of flags for specifying display screens. </summary>
''' <remarks> David, 10/15/2016. </remarks>
<Flags>
Public Enum DisplayScreens
    <ComponentModel.Description("Not specified")> None

    ''' <summary> 7500 SCREENS. </summary>
    <ComponentModel.Description("Home (HOME)")> Home
    <ComponentModel.Description("Home Large Reading (HOME_LARG)")> HomeLargeReading
    <ComponentModel.Description("Reading Table (READ)")> ReadingTable
    <ComponentModel.Description("Graph (GRAP)")> Graph
    <ComponentModel.Description("Histogram  (HIST)")> Histogram
    <ComponentModel.Description("Functions swipe screen (SWIPE_FUNC)")> FunctionsSwipe
    <ComponentModel.Description("Graph swipe screen (SWIPE_GRAP)")> GraphSwipe
    <ComponentModel.Description("Secondary swipe screen (SWIPE_SEC)")> SecondarySwipe
    <ComponentModel.Description("Settings swipe screen (SWIPE_SETT)")> SettingsSwipe
    <ComponentModel.Description("Statistics swipe screen (SWIPE_STAT)")> StatisticsSwipe
    <ComponentModel.Description("USER swipe screen (SWIPE_USER)")> UserSwipe

    ''' <summary> Special User Screens </summary>
    <System.ComponentModel.Description("Default screen")> [Default] = 1

    ''' <summary>Cleared user screen mode.</summary>
    <System.ComponentModel.Description("User screen")> User = 128

    ''' <summary>Last command displayed title.</summary>
    <System.ComponentModel.Description("Title is displayed")> Title = 256

    ''' <summary>Custom lines are displayed.</summary>
    <System.ComponentModel.Description("Special display")> Custom = 512

    ''' <summary>Measurement displayed.</summary>
    <System.ComponentModel.Description("Measurement is displayed")> Measurement = 1024

End Enum


