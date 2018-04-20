Imports isr.Core.Pith.StackTraceExtensions
Imports isr.VI.ExceptionExtensions
''' <summary> Loads and runs TSP scripts. </summary>
''' <license> (c) 2007 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="03/12/2007" by="David" revision="1.15.2627.x"> Created. </history>
Public MustInherit Class ScriptManagerBase
    Inherits VI.SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="ScriptManagerBase" /> class. </summary>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._lastFetchedSavedScripts = ""
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                If Me._legacyScripts IsNot Nothing Then Me._legacyScripts.Dispose() : Me._legacyScripts = Nothing
                If Me._scripts IsNot Nothing Then Me._scripts.Dispose() : Me._scripts = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Clears Error Queue.
    '''           </para></remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me._newProgramRequired = ""
    End Sub

    ''' <summary> Reset known state of this instance. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me._FirmwareExists = New Boolean?
        Me._supportfirmwareExists = New Boolean?
        Me._lastFetchedSavedScripts = ""
        Me._lastFetchedSavedRemoteScripts = ""
        Me._lastFetchScriptSource = ""
    End Sub

#End Region

#Region " SESSION / STATUS SUBSYSTEM "

    ''' <summary> Gets or sets the display subsystem. </summary>
    Private DisplaySubsystem As DisplaySubsystemBase

    ''' <summary> Gets or sets the link subsystem. </summary>
    ''' <value> The link subsystem. </value>
    Public Property LinkSubsystem As LinkSubsystemBase

    ''' <summary> Gets the interactive subsystem. </summary>
    ''' <value> The interactive subsystem. </value>
    Public Property InteractiveSubsystem As LocalNodeSubsystemBase

#End Region

#Region " IDENTITY "

    Private _NewProgramRequired As String

    ''' <summary> Gets the message indicating that a new program is required for the instrument because
    ''' this instrument is not included in the instrument list. </summary>
    ''' <value> The new program required. </value>
    Public ReadOnly Property NewProgramRequired() As String
        Get
            If String.IsNullOrWhiteSpace(Me._newProgramRequired) Then
                Me._newProgramRequired = "A new version of the program is required;. for instrument {0}"
                Me._newProgramRequired = String.Format(Globalization.CultureInfo.CurrentCulture, Me._newProgramRequired,
                                                       Me.StatusSubsystem.Identity)
            End If
            Return Me._newProgramRequired
        End Get
    End Property

#End Region

#Region " NAME AND FILE PATH "

    ''' <summary>Gets or sets the script AutoLoadEnabled.
    ''' </summary>
    Private _AutoLoadEnabled As Boolean

    ''' <summary> Gets or sets the auto load enabled sentinel. </summary>
    ''' <value> The automatic load enabled. </value>
    Public Property AutoLoadEnabled() As Boolean
        Get
            Return Me._AutoLoadEnabled
        End Get
        Set(ByVal Value As Boolean)
            If Not Value.Equals(Me.AutoLoadEnabled) Then
                Me._AutoLoadEnabled = Value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary>Gets or sets the script name.
    ''' </summary>
    Private _Name As String

    ''' <summary> Gets or sets the script name. </summary>
    ''' <value> The name. </value>
    Public ReadOnly Property Name() As String
        Get
            Return Me._name
        End Get
    End Property


    ''' <summary> Script name setter. </summary>
    ''' <param name="value"> Specifies the script name. </param>
    Public Sub ScriptNameSetter(ByVal value As String)
        If String.IsNullOrWhiteSpace(value) Then value = ""
        If Not String.Equals(value, Me.Name, StringComparison.OrdinalIgnoreCase) Then
            If ScriptEntityBase.IsValidScriptName(value) Then
                Me._Name = value
                Me.SafePostPropertyChanged(NameOf(Script.ScriptManagerBase.Name))
            Else
                ' now report the error to the calling module
                Throw New System.IO.IOException(String.Format(Globalization.CultureInfo.CurrentCulture,
                                                              "Invalid file name'. value '{0}' must not include any of these characters '{1}'.",
                                                              value, TspSyntax.IllegalScriptNameCharacters))
            End If
        End If
    End Sub

    Private _FilePath As String

    ''' <summary> Gets or sets the script file name. </summary>
    ''' <value> The full pathname of the file. </value>
    Public Property FilePath() As String
        Get
            Return Me._FilePath
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.FilePath, StringComparison.OrdinalIgnoreCase) Then
                Me._FilePath = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " USER SCRIPTS "

    ''' <summary> Stores user script names. </summary>
    Private _UserScriptNames() As String

    ''' <summary> Returns the list of user script names. </summary>
    ''' <returns> the list of user script names. </returns>
    Public Function UserScriptNames() As String()
        Return Me._userScriptNames
    End Function

    ''' <summary> Gets all users scripts from the instrument. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <exception cref="TimeoutException">         Thrown when a Timeout error condition occurs. </exception>
    ''' <returns> The user script names. </returns>
    Private Function _FetchUserScriptNames() As Integer

        Dim activity As String = $"{Me.ResourceNameCaption} fetching scripts"
        ' load the function which to execute and get its name.
        Dim functionName As String
        functionName = Me.LoadPrintUserScriptNames()

        If String.IsNullOrWhiteSpace(functionName) Then

            ' now report the error to the calling module
            If (Me.StatusSubsystem.ReadServiceRequestStatus And Me.StatusSubsystem.ErrorAvailableBits) <> 0 Then
                ' this is called withing the device error query command. Me.StatusSubsystem.QueryStandardEventStatus()
                Dim e As New isr.Core.Pith.ActionEventArgs
                If Me.StatusSubsystem.TrySafeQueryDeviceErrors(e) Then
                    Throw New VI.Pith.OperationFailedException($"Failed {activity};. Device errors: {Me.StatusSubsystem.DeviceErrorsReport}")
                Else
                    Throw New VI.Pith.OperationFailedException($"Failed {activity};. Failed fetching device errors because {e.Details}")
                End If
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity};. no device errors")
            End If

        End If

        ' Disable automatic display of errors - leave error messages in queue and enable error Prompt.
        Me.InteractiveSubsystem.WriteShowErrors(False)

        ' Turn off prompts
        Me.InteractiveSubsystem.WriteShowPrompts(False)

        activity = $"{Me.ResourceNameCaption} executing {functionName} fetching user script names"
        ' run the function
        Dim delimiter As Char = ","c
        Dim callState As TspExecutionState
        Me.StatusSubsystem.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, activity)
        TspSyntax.CallFunction(Me.Session, functionName, "'" & delimiter & "'")

        ' wait till we get a reply from the instrument or timeout.
        Dim endTime As DateTime = DateTime.Now.Add(TimeSpan.FromMilliseconds(2000))
        Do
            Threading.Thread.Sleep(50)
        Loop Until endTime < DateTime.Now Or Me.StatusSubsystem.IsMessageAvailable(TimeSpan.FromMilliseconds(1), 3)
        If Not Me.StatusSubsystem.MessageAvailable Then
            Throw New TimeoutException($"Timeout waiting for {activity}")
        End If

        ' read the names
        Dim names As String = Me.Session.ReadLine()
        If String.IsNullOrWhiteSpace(names) Then

            ' check if return value is an error a prompt. This will happened if the call is bad.
        ElseIf names = TspSyntax.NilValue Then

            ' read the next value, which should be true
            Dim expectedValue As String = "1"
            Dim value As String = Me.Session.ReadLineTrimEnd()
            If String.IsNullOrWhiteSpace(value) OrElse Not value.Trim.StartsWith(expectedValue, StringComparison.OrdinalIgnoreCase) Then
                Debug.Assert(Not Debugger.IsAttached, "Expected True (1)", "Expected True {0} got {1}", expectedValue, value)
            End If

            ' read execution state explicitly, because session events are disabled.
            Me.InteractiveSubsystem.ReadExecutionState()

        Else

            ' parse execution state explicitly, because session events are disabled.
            callState = Me.InteractiveSubsystem.ParseExecutionState(names, TspExecutionState.Unknown)
            If TspExecutionState.IdleError = callState Then

                ' now report the error to the calling module
                If (Me.StatusSubsystem.ReadServiceRequestStatus And Me.StatusSubsystem.ErrorAvailableBits) <> 0 Then
                    ' down inside the device error query: Me.StatusSubsystem.QueryStandardEventStatus()
                    ' this is called withing the device error query command. Me.StatusSubsystem.QueryStandardEventStatus()
                    Dim e As New isr.Core.Pith.ActionEventArgs
                    If Me.StatusSubsystem.TrySafeQueryDeviceErrors(e) Then
                        Throw New VI.Pith.OperationFailedException($"Failed {activity};. Device errors: {Me.StatusSubsystem.DeviceErrorsReport}")
                    Else
                        Throw New VI.Pith.OperationFailedException($"Failed {activity};. Failed fetching device errors because {e.Details}")
                    End If
                Else
                    Throw New VI.Pith.OperationFailedException($"Failed {activity};. No device errors")
                End If

                ' check if return value is false.  This will happen if the function failed.
            ElseIf names.Substring(0, 4) = TspSyntax.FalseValue Then

                activity = $"{activity} w/ argument {names.Split(delimiter)(1)}"
                ' if failure, get the failure message
                If (Me.StatusSubsystem.ReadServiceRequestStatus And Me.StatusSubsystem.ErrorAvailableBits) <> 0 Then
                    ' done inside the query Me.StatusSubsystem.QueryStandardEventStatus()
                    Dim e As New isr.Core.Pith.ActionEventArgs
                    If Me.StatusSubsystem.TrySafeQueryDeviceErrors(e) Then
                        Throw New VI.Pith.OperationFailedException($"Failed {activity};. Device errors: {Me.StatusSubsystem.DeviceErrorsReport}")
                    Else
                        Throw New VI.Pith.OperationFailedException($"Failed {activity};. Failed fetching device errors because {e.Details}")
                    End If
                Else
                    Throw New VI.Pith.OperationFailedException($"Failed {activity};. No device errors")
                End If

            Else

                ' split the return values
                Me._userScriptNames = names.Split(delimiter)

                ' read the next value, which should be true
                Dim expectedValue As String = "1"
                Dim value As String = Me.Session.ReadLineTrimEnd()
                If String.IsNullOrWhiteSpace(value) OrElse Not value.Trim.StartsWith(expectedValue, StringComparison.OrdinalIgnoreCase) Then
                    Debug.Assert(Not Debugger.IsAttached, "Expected True (1)", "Expected True {0} got {1}", expectedValue, value)
                End If

                If Not Me.InteractiveSubsystem.ProcessExecutionStateEnabled Then
                    ' read execution state explicitly, because session events are disabled.
                    Me.InteractiveSubsystem.ReadExecutionState()
                End If

            End If
        End If

        ' return <c>True</c> if we got one or more.
        Return Me.UserScriptNames.Length

    End Function

    ''' <summary> Gets all users scripts from the instrument. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <exception cref="TimeoutException">         Thrown when a Timeout error condition occurs. </exception>
    ''' <returns> The number of user script names that were fetched. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Public Function FetchUserScriptNames() As Integer

        ' store state of prompts and errors.
        Me.InteractiveSubsystem.StoreStatus()
        Me.StatusSubsystem.Session.LastNodeNumber = New Integer?
        Dim processExecutionStateWasEnabled As Boolean = Me.InteractiveSubsystem.ProcessExecutionStateEnabled
        ' disable session events.
        Me.InteractiveSubsystem.ProcessExecutionStateEnabled = False

        Try
            Return Me._FetchUserScriptNames()

        Catch

            ' remove any remaining values.
            Me.Session.DiscardUnreadData(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(100))

            Throw

        Finally

            ' restore state of prompts and errors.
            Me.InteractiveSubsystem.RestoreStatus()

            ' add a wait to ensure the system returns the last status.
            Threading.Thread.Sleep(100)

            ' flush the buffer until empty to completely reading the status.
            Dim bits As VI.Pith.ServiceRequests = Me.Session.MeasurementAvailableBits
            Me.Session.MeasurementAvailableBits = Me.StatusSubsystem.MessageAvailableBits
            Me.Session.DiscardUnreadData()
            Me.Session.MeasurementAvailableBits = bits

            Me.InteractiveSubsystem.ProcessExecutionStateEnabled = processExecutionStateWasEnabled
        End Try

    End Function

    ''' <summary> Loads the 'printUserScriptNames' function and return the function name. The function
    ''' is loaded only if it does not exists already. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <returns> The function name. </returns>
    Public Function LoadPrintUserScriptNames() As String

        ' store state of prompts and errors.
        Me.InteractiveSubsystem.StoreStatus()
        Dim processExecutionStateWasEnabled As Boolean = Me.InteractiveSubsystem.ProcessExecutionStateEnabled
        Me.InteractiveSubsystem.ProcessExecutionStateEnabled = False

        Try

            Dim functionName As String
            functionName = "printUserScriptNames"

            Dim functionCode As String
            If Me.Session.IsGlobalExists(functionName) Then

                Return functionName

            Else

                functionCode = " function printUserScriptNames( delimiter ) local scripts = nil for i,v in _G.pairs(_G.script.user.scripts) do if scripts == nil then scripts = i else scripts = scripts .. delimiter .. i end end _G.print (scripts) _G.waitcomplete() end "

                ' load the function
                Me.Session.WriteLine(functionCode)
                Return functionName

            End If

        Catch ex As Exception

            ' remove any remaining values.
            Me.Session.DiscardUnreadData()

            Throw New VI.Pith.OperationFailedException(ex, "Failed loading print user script names functions. Discarded: {0}.", Me.Session.DiscardedData)

        Finally

            ' restore state of prompts and errors.
            Me.InteractiveSubsystem.RestoreStatus()

            Me.InteractiveSubsystem.ProcessExecutionStateEnabled = processExecutionStateWasEnabled

        End Try

    End Function

#End Region

#Region " BINARY SCRIPTS "

    ''' <summary> Loads the binary script function for creating binary scripts. </summary>
    ''' <remarks> Non-A instruments requires having a special function for creating the binary scripts. </remarks>
    ''' <exception cref="ArgumentNullException">  Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="Pith.NativeException">          Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when a Script Loading error condition occurs. </exception>
    ''' <param name="node"> Specifies the node. </param>
    Private Sub LoadBinaryScriptsFunction(ByVal node As NodeEntityBase)

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        Dim activity As String = $"{Me.ResourceNameCaption} loading Binary Scripts resource to node {node.Number}"
        Const scriptName As String = "CreateBinaries"
        Using script As New ScriptEntity(scriptName, Tsp.NodeEntity.ModelFamilyMask(Tsp.InstrumentModelFamily.K2600))
            script.Source = isr.Core.Pith.EmbeddedResourceManager.TryReadEmbeddedTextResource(System.Reflection.Assembly.GetExecutingAssembly,
                                                                                              "BinaryScripts.tsp")
            Me.LoadRunUserScript(script, node)
        End Using
        If (Me.StatusSubsystem.ReadServiceRequestStatus And Me.StatusSubsystem.ErrorAvailableBits) <> 0 Then
            ' done inside the query Me.StatusSubsystem.QueryStandardEventStatus()
            Dim e As New isr.Core.Pith.ActionEventArgs
            If Me.StatusSubsystem.TrySafeQueryDeviceErrors(e) Then
                Throw New VI.Pith.OperationFailedException($"Failed {activity};. Device errors: {Me.StatusSubsystem.DeviceErrorsReport}")
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity};. Failed fetching device errors because {e.Details}")
            End If
        End If

    End Sub

    ''' <summary> Checks if the script is Binary. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="name"> Specifies the name of the script. </param>
    ''' <param name="node"> Specifies the node entity. </param>
    ''' <returns> <c>True</c> if the script is a binary script; otherwise, <c>False&gt;</c>. </returns>
    Public Function IsBinaryScript(ByVal name As String, ByVal node As NodeEntityBase) As Boolean?
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If node.IsController Then
            Return Me.Session.IsStatementTrue("_G.isr.script.isBinary({0})", name)
        Else
            Return Me.Session.IsStatementTrue("_G.isr.script.isBinary({0},{1})", name, node.Number)
        End If
    End Function

    ''' <summary> Converts the script to binary format. </summary>
    ''' <remarks> Waits for operation completion. </remarks>
    ''' <exception cref="ArgumentNullException">  Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when a Script Loading error condition occurs. </exception>
    ''' <param name="name"> Specifies the script name. </param>
    ''' <param name="node"> Specifies the node entity. </param>
    Public Sub ConvertBinaryScript(ByVal name As String, ByVal node As NodeEntityBase)
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If String.IsNullOrWhiteSpace(name) Then
            Return
        ElseIf Me.IsBinaryScript(name, node) Then
            Return
        End If

        Me.Session.LastNodeNumber = node.Number
        If node.InstrumentModelFamily = InstrumentModelFamily.K2600A OrElse
            node.InstrumentModelFamily = InstrumentModelFamily.K3700 Then

            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Nilifying {0};. ", name)
            Dim message As String = Me.Session.ExecuteCommand(node.Number, "{0}.source = nil waitcomplete()", name)
            Me.CheckThrowDeviceException(False, message)

            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Waiting completion;. ")
            Me.LinkSubsystem.WaitComplete(node.Number, Me.SaveTimeout, False)
            Me.CheckThrowDeviceException(False, "clearing script '{0}' using '{1}';. ", name, message)
        Else

            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Creating binary script{0};. ", name)

            ' non-A instruments require having a special function for creating the binary scripts.
            Me.LoadBinaryScriptsFunction(node)

            Dim message As String = Me.Session.ExecuteCommand(node.Number, "CreateBinaryScript('{0}', {1}) waitcomplete()", name, name)
            Me.CheckThrowDeviceException(False, message)

            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Waiting completion;. ")
            Me.LinkSubsystem.WaitComplete(node.Number, Me.SaveTimeout, False)
            Me.CheckThrowDeviceException(False, "creating binary script '{0}' using '{1}';. ", name, message)

            Dim fullName As String = "script.user.scripts." & name
            If Me.Session.WaitNotNil(node.Number, fullName, Me._SaveTimeout) Then

                Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                           "assigning binary script name to '{0}';. ", name)

                message = Me.Session.ExecuteCommand(node.Number, "{0} = nil waitcomplete() {0} = {1} waitcomplete()", name, fullName)
                Me.LinkSubsystem.WaitComplete(node.Number, Me.SaveTimeout, False)
                Me.CheckThrowDeviceException(False, "assigning binary script name to '{0}' using '{1}';. ", name, message)

                If Me.Session.WaitNotNil(node.Number, name, Me._SaveTimeout) Then

                    If Not Me.IsBinaryScript(name, node) Then

                        Throw New VI.Pith.OperationFailedException("{0} failed creating binary script '{1}';. reference to script '{2}' on node {3}--new script not found on the remote node.{4}{5}",
                                                           Me.ResourceNameCaption, name, fullName, node.Number, Environment.NewLine,
                                                           New StackFrame(True).UserCallStack())
                    End If

                Else
                    Throw New VI.Pith.OperationFailedException("{0} failed creating script '{1}';. reference to script '{2}' on node {3}--new script not found on the remote node.{4}{5}",
                                                       Me.ResourceNameCaption, name, fullName, node.Number,
                                                       Environment.NewLine, New StackFrame(True).UserCallStack())

                End If

            Else
                Throw New VI.Pith.OperationFailedException("{0} failed creating script '{1}' using '{2}' on node {3}--new script not found on the remote node.{4}{5}",
                                                   Me.ResourceNameCaption, fullName, name, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
            End If

        End If

    End Sub

    ''' <summary> Saves the specifies script to non-volatile memory. </summary>
    ''' <remarks> Waits for operation completion. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="name">           Specifies the script name. </param>
    ''' <param name="node">           Specifies the node entity. </param>
    ''' <param name="isSaveAsBinary"> Specifies the condition requesting clearing the source before
    ''' saving. </param>
    ''' <param name="isBootScript">   Specifies the condition indicating if this is a boot script. </param>
    ''' <returns> <c>True</c> if the script was saved. </returns>
    Public Function SaveScript(ByVal name As String, ByVal node As NodeEntityBase, ByVal isSaveAsBinary As Boolean, ByVal isBootScript As Boolean) As Boolean

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))

        ' saving just the script name does not work if script was created and name assigned.
        Dim commandFormat As String = "script.user.scripts.{0}.save() waitcomplete()"

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                   "saving script '{0} on node {1};. ", name, node.Number)
        Me.Session.LastNodeNumber = node.Number

        If isSaveAsBinary Then
            Me.ConvertBinaryScript(name, node)
        End If

        If isBootScript Then
            commandFormat = "script.user.scripts.{0}.autorun = 'yes' " & commandFormat
        End If

        If node.IsController Then
            Me.StatusSubsystem.EnableWaitComplete()
            Me.Session.WriteLine(commandFormat, name)
            Me.StatusSubsystem.AwaitOperationCompleted(Me.SaveTimeout)
        Else
            Dim message As String = Me.Session.ExecuteCommand(node.Number, commandFormat, name)
            Me.CheckThrowDeviceException(False, message)

            Me.LinkSubsystem.WaitComplete(node.Number, Me.SaveTimeout, False)
            Me.CheckThrowDeviceException(False, "saving script '{0}' using '{1}';. ", name, message)
        End If

        Return Me.SavedScriptExists(name, node, True)

    End Function

#End Region

#Region " SCRIPT SOURCE "

    ''' <summary> Clears the <see cref="Name">specified</see> script source. </summary>
    Public Sub ClearScriptSource()
        Me.ClearScriptSource(Me._Name)
    End Sub

    ''' <summary> Clears the <paramref name="value">specified</paramref> script source. </summary>
    ''' <param name="value"> Specifies the script name. </param>
    Public Sub ClearScriptSource(ByVal value As String)
        If Not String.IsNullOrWhiteSpace(value) Then
            Me.Session.WriteLine("{0}.source = nil ", value)
        End If
    End Sub

    ''' <summary> Fetches the <paramref name="name">specified</paramref> script source. </summary>
    ''' <remarks>
    ''' Requires setting the correct bits for message available bits. No longer trimming spaces as
    ''' this caused failure to load script to the 2602.
    ''' </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="name"> Specifies the script name. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub FetchScriptSource(ByVal name As String)

        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))

        Me._LastFetchScriptSource = ""

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "fetching script source;. ")
        Me.Session.WriteLine("isr.script.list({0})", name)

        Threading.Thread.Sleep(500)
        Me._LastFetchScriptSource = Me.Session.ReadLines(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(400), True)

        ' trap non-empty buffer.
        Try
            If (Me.Session.ReadServiceRequestStatus And VI.Pith.ServiceRequests.MessageAvailable) <> 0 Then
                Debug.Assert(Not Debugger.IsAttached, "Buffer Not empty")
            End If
            NodeEntity.NodeExists(Me.Session, 1)
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Buffer Not empty")
        End Try

        Me.CheckThrowDeviceException(True, "reading source;. last sent: '{0}'; last read: '{1}'",
                                     Me.Session.LastMessageSent, Me.Session.LastMessageReceived)

    End Sub

    ''' <summary> Fetches the <paramref name="name">specified</paramref> script source. </summary>
    ''' <remarks> Requires setting the proper message available bits for the session. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="Pith.NativeException">         Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="nodeNumber"> Specifies the subsystem node. </param>
    ''' <param name="name">       Specifies the script name. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub FetchScriptSource(ByVal nodeNumber As Integer, ByVal name As String)

        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))

        Me._LastFetchScriptSource = ""

        ' clear data queue and report if not empty.
        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "clearing data queue")
        Me.LinkSubsystem.ClearDataQueue(nodeNumber)

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "listing script source")
        Me.Session.WriteLine("isr.script.list({0},{1})", name, nodeNumber)

        Threading.Thread.Sleep(500)
        Me._LastFetchScriptSource = Me.Session.ReadLines(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(400), True)

        ' trap non-empty buffer.
        Try
            If (Me.Session.ReadServiceRequestStatus And VI.Pith.ServiceRequests.MessageAvailable) <> 0 Then
                Debug.Assert(Not Debugger.IsAttached, "Buffer not empty")
            End If
            NodeEntity.NodeExists(Me.Session, 1)
        Catch
            Debug.Assert(Not Debugger.IsAttached, "Buffer not empty")
        End Try

        Me.CheckThrowDeviceException(True, "reading source;. last sent: '{0}'.", Me.Session.LastMessageSent)
    End Sub

    ''' <summary> Fetches the script source. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="script"> Specifies the script. </param>
    ''' <param name="node">   Specifies the node. </param>
    Public Sub FetchScriptSource(ByVal script As ScriptEntityBase, ByVal node As NodeEntityBase)

        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If node.IsController Then
            Me.FetchScriptSource(script.Name)
        Else
            Me.FetchScriptSource(node.Number, script.Name)
        End If

    End Sub

    ''' <summary> Lists the <paramref name="value">specified</paramref> script. </summary>
    ''' <param name="value">      Specifies the script name. </param>
    ''' <param name="trimSpaces"> Specifies a directive to trim leading and trailing spaces from each
    ''' line. </param>
    ''' <returns> The script source. </returns>
    Public Function FetchUserScriptSource(ByVal value As String, ByVal trimSpaces As Boolean) As String
        If Not String.IsNullOrWhiteSpace(value) Then
            Me.Session.WriteLine("print({0}.source)", value)
            Return Me.Session.ReadLines(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(400), trimSpaces, Not trimSpaces)
        Else
            Return ""
        End If
    End Function

    ''' <summary> Check if a user script was saved as a binary script. </summary>
    ''' <param name="name"> Gets or sets the script name. </param>
    ''' <returns> <c>True</c> if binary; otherwise, <c>False</c>. </returns>
    Public Function IsScriptSavedAsBinary(ByVal name As String) As Boolean
        Me.Session.MakeTrueFalseReplyIfEmpty(True)
        Return Me.Session.IsStatementTrue("string.sub({0}.source,1,24) == 'loadstring(table.concat('", name)
    End Function

    Private _LastFetchScriptSource As String

    ''' <summary> Gets the last source fetched from the instrument. </summary>
    ''' <value> The last fetch script source. </value>
    Public ReadOnly Property LastFetchScriptSource() As String
        Get
            Return Me._LastFetchScriptSource
        End Get
    End Property

    ''' <summary> Runs the named script. </summary>
    ''' <param name="timeout"> Specifies the time to wait for the instrument to return operation
    ''' completed. </param>
    ''' <exception cref="Pith.NativeException">            Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when a Script Call error condition
    ''' occurs. </exception>
    ''' <exception cref="TimeoutException">         Thrown when a Timeout error condition occurs. </exception>
    Public Sub RunScript(ByVal timeout As TimeSpan)

        ' store state of prompts and errors.
        Me.InteractiveSubsystem.StoreStatus()

        Try

            ' Disable automatic display of errors - leave error messages in queue and enable error Prompt.
            Me.InteractiveSubsystem.WriteShowErrors(False)

            ' Turn off prompts
            Me.InteractiveSubsystem.WriteShowPrompts(False)

            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "running script '{0}';. ", Name)
            Me.Session.LastNodeNumber = New Integer?
            Dim returnedValue As String = "1"
            Me.Session.WriteLine("{0}.run() waitcomplete() print('{1}') ", Me._Name, returnedValue)
            ' wait till we get a reply from the instrument or timeout.
            If Me.Session.IsMessageAvailable(TimeSpan.FromMilliseconds(40), timeout) Then
                Dim value As String = Me.Session.ReadLineTrimEnd
                If String.IsNullOrWhiteSpace(value) OrElse Not value.Trim.StartsWith(returnedValue, StringComparison.OrdinalIgnoreCase) Then
                    If String.IsNullOrWhiteSpace(value) Then
                        Throw New VI.Pith.OperationFailedException("Script '{0}' failed;. script returned no value.", Name)
                    Else
                        Throw New VI.Pith.OperationFailedException("Script '{0}' failed;. returned value '{1}' instead of the expected '{2}'.",
                                                               Name, value, returnedValue)
                    End If
                End If
            Else
                Throw New TimeoutException("Timeout waiting operation completion running the script '" & Me._Name & "'")
            End If

        Catch

            ' remove any remaining values.
            Me.Session.DiscardUnreadData()

            Throw

        Finally

            ' restore state of prompts and errors.
            Me.InteractiveSubsystem.RestoreStatus()

            ' add a wait to ensure the system returns the last status.
            Threading.Thread.Sleep(100)

            ' flush the buffer until empty to completely reading the status.
            Me.Session.DiscardUnreadData()

        End Try

    End Sub

#End Region

#Region " NILIFY SCRIPT "

    Private _NilifyTimeout As TimeSpan = TimeSpan.Zero

    ''' <summary> Gets or sets the time out for nilifying a script. </summary>
    ''' <value> The Nilify timeout. </value>
    Public Property NilifyTimeout() As TimeSpan
        Get
            Return Me._NilifyTimeout
        End Get
        Set(ByVal Value As TimeSpan)
            If Me._NilifyTimeout = Value Then
                Me._NilifyTimeout = Value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Makes a script nil. Return <c>True</c> if the script is nil. </summary>
    ''' <remarks> Assumes the script is known to exist. Waits for operation completion. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="Pith.NativeException">         Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when a Script Delete error condition occurs. </exception>
    ''' <param name="name"> Specifies the script name. </param>
    Private Sub _NilifyScript(ByVal name As String)

        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))

        Me.StatusSubsystem.EnableWaitComplete()
        Me.Session.WriteLine("{0} = nil waitcomplete()", name)
        Me.CheckThrowDeviceException(True, "nilifying script;. using the command '{0}'", Me.Session.LastMessageSent)

        Me.StatusSubsystem.AwaitOperationCompleted(Me.NilifyTimeout)
        If Not Me.Session.IsNil(name) Then
            Throw New VI.Pith.OperationFailedException("Instrument '{0}' script {1} still exists after nil.", Me.ResourceNameCaption, name)
        End If

    End Sub

    ''' <summary> Makes a script nil. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="name"> Specifies the script name. </param>
    Public Sub NilifyScript(ByVal name As String)
        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))
        If Not Me.Session.IsNil(name) Then
            Me._NilifyScript(name)
        End If
    End Sub

    ''' <summary> Makes a script nil and returns <c>True</c> if the script was nilified. Does not check
    ''' if the script exists. </summary>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <remarks> Assumes the script is known to exist. Waits till completion. </remarks>
    ''' <param name="nodeNumber"> Specifies the remote node number. </param>
    ''' <param name="name">       Specifies the script name. </param>
    ''' <returns> <c>True</c> if the script is nil; otherwise <c>False</c>. </returns>
    Private Function _NilifyScript(ByVal nodeNumber As Integer, ByVal name As String) As Boolean
        If String.IsNullOrWhiteSpace(name) Then
            Return False
        End If

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                   "nilifying script '{0} on node {1};. ", name, nodeNumber)
        Me.Session.LastNodeNumber = nodeNumber

        ' ignore errors as failure is handled below.
        Dim message As String = Me.Session.ExecuteCommand(nodeNumber, "{0} = nil", name)
        Me.CheckThrowDeviceException(False, message)

        Me.LinkSubsystem.WaitComplete(nodeNumber, Me.NilifyTimeout, False)
        Me.CheckThrowDeviceException(False, "nilifying script '{0}' using '{1}';. ", name, message)

        ' allow the next command to create an error if wait complete times out.
        Dim affirmative As Boolean = Me.Session.IsNil(nodeNumber, name)
        Me.Session.LastNodeNumber = New Integer?
        Return affirmative

    End Function

    ''' <summary> Makes a script nil and returns <c>True</c> if the script was nullified. </summary>
    ''' <param name="nodeNumber"> Specifies the remote node number. </param>
    ''' <param name="name">       Specifies the script name. </param>
    ''' <returns> <c>True</c> if the script is nil; otherwise <c>False</c>. </returns>
    Public Function NilifyScript(ByVal nodeNumber As Integer, ByVal name As String) As Boolean
        If String.IsNullOrWhiteSpace(name) Then
            Return False
        End If
        If Me.Session.IsNil(nodeNumber, name) Then
            Return True
        Else
            Return Me._NilifyScript(nodeNumber, name)
        End If
    End Function

#End Region

#Region " DELETE SCRIPTS "

    ''' <summary> Removes the script from the device. Updates the script list. </summary>
    ''' <param name="scriptName"> Name of the script. </param>
    Public Sub RemoveScript(ByVal scriptName As String)

        Me.DeleteScript(scriptName, True)
        If Me.StatusSubsystem.CollectGarbageWaitComplete(Me.DeleteTimeout, "deleting script '{0}';. ", scriptName) Then
            Me.FetchUserScriptNames()
        End If
    End Sub

    ''' <summary> Deletes the <paramref name="name">specified</paramref> script. Also nilifies the
    ''' script if delete command worked. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="name">                 Specifies the script name. </param>
    ''' <param name="refreshScriptCatalog"> True to refresh the list of saved scripts. </param>
    Public Sub DeleteScript(ByVal name As String, ByVal refreshScriptCatalog As Boolean)

        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))

        If Me.SavedScriptExists(name, refreshScriptCatalog) Then
            Me.DeleteSavedScript(name)
        Else
            Me.NilifyScript(name)
        End If
    End Sub

    ''' <summary> Deletes the <paramref name="name">specified</paramref> saved script. Also nilifies
    ''' the script if delete command worked. Returns <c>True</c> if the script was deleted. </summary>
    ''' <remarks> Assumes the script is known to exist. Waits for operation completion. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when a Script Delete error condition occurs. </exception>
    ''' <exception cref="Pith.NativeException">         Thrown when a Visa error condition occurs. </exception>
    ''' <param name="name"> Specifies the script name. </param>
    Public Sub DeleteSavedScript(ByVal name As String)

        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Enabling wait completion;. ")
        Me.StatusSubsystem.EnableWaitComplete()

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Deleting script;. ")
        Me.Session.WriteLine("script.delete('{0}') waitcomplete()", name)

        ' make script nil
        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Awaiting operation completion;. ")
        Me.StatusSubsystem.AwaitOperationCompleted(Me.DeleteTimeout)

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Deleting script;. ")
        Me.NilifyScript(name)

        ' make sure to re-check that script is gone.
        If Me.SavedScriptExists(name, True) Then
            Throw New VI.Pith.OperationFailedException("Instrument '{0}' script {1} still exists after nil.", Me.ResourceNameCaption, name)
        End If

    End Sub

    ''' <summary> Deletes the <paramref name="name">specified</paramref> saved script. Also nilifies
    ''' the script if delete command worked. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="name">                 Specifies the script name. </param>
    ''' <param name="refreshScriptCatalog"> True to refresh the list of saved scripts. </param>
    Public Sub DeleteSavedScript(ByVal name As String, ByVal refreshScriptCatalog As Boolean)
        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))
        If Me.SavedScriptExists(name, refreshScriptCatalog) Then
            Me.DeleteSavedScript(name)
        End If
    End Sub

    ''' <summary>
    ''' Deletes the <paramref name="name">specified</paramref> script.
    ''' Also nilifies the script if delete command worked.
    ''' </summary>
    ''' <param name="name">Specifies the script name</param>
    ''' <returns> <c>True</c> if the script is nil; otherwise <c>False</c>. </returns>
    Public Function DeleteScript(ByVal node As NodeEntityBase, ByVal name As String, ByVal refreshScriptCatalog As Boolean) As Boolean
        If String.IsNullOrWhiteSpace(name) Then Return False
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If node.IsController Then
            Me.DeleteScript(name, refreshScriptCatalog)
            Return True
        Else
            If Me.SavedScriptExists(node, name, refreshScriptCatalog) Then
                Return Me.DeleteSavedScript(node, name)
            Else
                Return Me.NilifyScript(node.Number, name)
            End If
        End If

    End Function

    ''' <summary>
    ''' Deletes the <paramref name="name">specified</paramref> saved script.
    ''' Also nilifies the script if delete command worked.
    ''' Then checks if the script was deleted and if so returns true. 
    ''' Otherwise, returns false. 
    ''' </summary>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="name">Specifies the script name</param>
    ''' <remarks>Presumes the saved script exists.
    ''' Waits for operation completion.
    ''' </remarks>
    Public Function DeleteSavedScript(ByVal node As NodeEntityBase, ByVal name As String) As Boolean
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If String.IsNullOrWhiteSpace(name) Then Return False

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Deleting script '{0}';. ", name)
        Me.Session.LastNodeNumber = node.Number
        ' failure is handled below.
        Dim message As String = Me.Session.ExecuteCommand(node.Number, "script.delete('{0}')", name)

        Me.CheckThrowDeviceException(False, message)

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Wait complete;. ")
        Me.LinkSubsystem.WaitComplete(node.Number, Me.DeleteTimeout, False)
        Me.CheckThrowDeviceException(False, "deleting script '{0}' using '{1}';. ", name, message)

        ' make script nil
        Dim affirmative As Boolean = True
        If Me.NilifyScript(node.Number, name) Then
            ' make sure to re-check that script is gone.
            If Me.SavedScriptExists(node, name, True) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                   "Instrument '{0}' saved script {1} still exists after nil on node {2};. {3}{4}",
                                   Me.ResourceNameCaption, name, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
                affirmative = False
            Else
                affirmative = True
            End If
        Else
            affirmative = False
        End If
        Me.Session.LastNodeNumber = New Integer?
        Return affirmative
    End Function

    ''' <summary> Deletes the <see cref="Name">specified</see> script. </summary>
    ''' <param name="refreshScriptCatalog"> True to refresh the list of saved scripts. </param>
    ''' <returns> <c>True</c> if the script is nil; otherwise <c>False</c>. </returns>
    Public Function DeleteSavedScript(ByVal refreshScriptCatalog As Boolean) As Boolean
        Me.DeleteSavedScript(Me._Name, refreshScriptCatalog)
        Return True
    End Function

    Private _DeleteTimeout As TimeSpan

    ''' <summary> Gets or sets the time out for deleting a script. </summary>
    ''' <value> The delete timeout. </value>
    Public Property DeleteTimeout() As TimeSpan
        Get
            Return Me._DeleteTimeout
        End Get
        Set(ByVal Value As TimeSpan)
            If Not Me.DeleteTimeout.Equals(Value) Then
                Me._DeleteTimeout = Value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " FETCH SCRIPTS "

    Private Const AuthodPrefix As String = "isr_"
    Private _LastFetchedAuthorScripts As List(Of String)

    ''' <summary> Last fetched author scripts. </summary>
    ''' <returns> A list of strings. </returns>
    Public Function LastFetchedAuthorScripts() As ObjectModel.ReadOnlyCollection(Of String)
        If _LastFetchedAuthorScripts Is Nothing Then
            Return New ObjectModel.ReadOnlyCollection(Of String)(New List(Of String))
        Else
            Return New ObjectModel.ReadOnlyCollection(Of String)(_LastFetchedAuthorScripts)
        End If
    End Function

    ''' <summary> Fetches the list of saved scripts and saves it in the
    ''' <see cref="LastFetchedSavedScripts"></see> </summary>
    ''' <exception cref="Pith.NativeException">       Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub FetchSavedScripts()
        Try
            Me.Session.StoreTimeout(Me.SaveTimeout)
            Me._LastFetchedSavedScripts = ""
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "fetching saved scripts;. ")
            Me.Session.LastNodeNumber = New Integer?
            Me.Session.WriteLine("do {0} print( names ) end ", TspSyntax.ScriptCatalogGetterCommand)
            Me._LastFetchedSavedScripts = Me.Session.ReadLineTrimEnd
            If String.IsNullOrWhiteSpace(Me._LastFetchedSavedScripts) Then
                Me._LastFetchedSavedScripts = ""
                Me.CheckThrowDeviceException(True, "fetching saved scripts;. last sent: '{0}'; last received: '{1}'.",
                                             Me.Session.LastMessageSent, Me.Session.LastMessageReceived)
            Else
                Me._LastFetchedAuthorScripts = New List(Of String)
                Dim scripts As String() = Me._LastFetchedSavedScripts.Split(","c)
                For Each s As String In scripts
                    If s.StartsWith(AuthodPrefix, StringComparison.OrdinalIgnoreCase) Then
                        Me._LastFetchedAuthorScripts.Add(s)
                    End If
                Next
            End If
        Catch
            Throw
        Finally
            Me.Session.RestoreTimeout()
        End Try
    End Sub

    ''' <summary> Fetches the list of saved scripts and saves it in the
    ''' <see cref="LastFetchedSavedScripts"></see> </summary>
    ''' <exception cref="Pith.NativeException">       Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="nodeNumber"> Specifies the subsystem node. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub FetchSavedScripts(ByVal nodeNumber As Integer)
        Try
            Me.Session.StoreTimeout(Me._SaveTimeout)
            Me._LastFetchedSavedRemoteScripts = ""
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "fetching catalog;. ")
            Me.Session.LastNodeNumber = nodeNumber
            Me.Session.WriteLine(TspSyntax.NodeValueGetterCommandFormat2, nodeNumber, TspSyntax.ScriptCatalogGetterCommand, "names")

            Me._LastFetchedSavedRemoteScripts = Me.Session.ReadLineTrimEnd

            Me.CheckThrowDeviceException(True, "fetching catalog;. last sent: '{0}'.", Me.Session.LastMessageSent)

        Catch
            Throw
        Finally
            Me.Session.RestoreTimeout()
            Me.Session.LastNodeNumber = New Integer?
        End Try
    End Sub

    ''' <summary> Fetches the list of saved scripts and saves it in the
    ''' <see cref="LastFetchedSavedScripts"></see> </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="Pith.NativeException">         Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="node"> Specifies the node. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub FetchSavedScripts(ByVal node As NodeEntityBase)
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If node.IsController Then
            FetchSavedScripts()
        Else
            Try
                Me.Session.StoreTimeout(Me._SaveTimeout)
                Me._LastFetchedSavedRemoteScripts = ""
                Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "fetching catalog;. ")
                Me.Session.LastNodeNumber = node.Number
                Me.Session.WriteLine(TspSyntax.NodeValueGetterCommandFormat2, node.Number, TspSyntax.ScriptCatalogGetterCommand, "names")
                Me.CheckThrowDeviceException(True, "fetching catalog using the command '{0}';. ", Me.Session.LastMessageSent)
                Me._LastFetchedSavedRemoteScripts = Me.Session.ReadLineTrimEnd
            Catch
                Throw
            Finally
                Me.Session.LastNodeNumber = New Integer?
                Me.Session.RestoreTimeout()
            End Try
        End If
    End Sub

    Private _LastFetchedSavedScripts As String

    ''' <summary> Gets a comma-separated and comma-terminated list of the saved scripts that was
    ''' fetched last.  A new script is fetched after save and delete. </summary>
    ''' <value> The last fetched saved scripts. </value>
    Public ReadOnly Property LastFetchedSavedScripts() As String
        Get
            Return Me._LastFetchedSavedScripts
        End Get
    End Property

    Private _LastFetchedSavedRemoteScripts As String

    ''' <summary> Gets a comma-separated and comma-terminated list of the saved scripts that was
    ''' fetched last from the remote node.  A new script is fetched after save and delete. </summary>
    ''' <value> The last fetched saved remote scripts. </value>
    Public ReadOnly Property LastFetchedSavedRemoteScripts() As String
        Get
            Return Me._LastFetchedSavedRemoteScripts
        End Get
    End Property

#End Region

#Region " RUN SCRIPT "

    ''' <summary> Runs the named script. </summary>
    ''' <remarks> Waits for operation completion. </remarks>
    ''' <param name="name">    Specifies the script name. </param>
    ''' <param name="timeout"> Specifies the time to wait for the instrument to return operation
    ''' completed. </param>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are
    ''' null. </exception>
    ''' <exception cref="Pith.NativeException">         Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="TimeoutException">      Thrown when a Timeout error condition occurs. </exception>
    Public Sub RunScript(ByVal name As String, ByVal timeout As TimeSpan)

        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))

        Dim returnedValue As String = "1"
        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "running script;. ")
        Me.Session.WriteLine("{0}.run() waitcomplete() print('{1}') ", name, returnedValue)

        ' wait till we get a reply from the instrument or timeout.
        If Me.Session.IsMessageAvailable(TimeSpan.FromMilliseconds(40), timeout) Then
            Dim value As String = Me.Session.ReadLineTrimEnd
            If String.IsNullOrWhiteSpace(value) OrElse Not value.Trim.StartsWith(returnedValue, StringComparison.OrdinalIgnoreCase) Then
                If String.IsNullOrWhiteSpace(value) Then
                    Throw New VI.Pith.OperationFailedException("Script '{0}' failed;. script returned no value.", name)
                Else
                    Throw New VI.Pith.OperationFailedException("Script '{0}' failed;. returned value {1} is not the same as expected '{2}'.",
                                                       name, value, returnedValue)
                End If
            End If
        Else
            Throw New TimeoutException("Timeout waiting operation completion running the script '" & name & "'")
        End If

    End Sub

    ''' <summary> Runs the named script. </summary>
    ''' <remarks> Waits for operation completion. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="nodeNumber"> Specifies the subsystem node. </param>
    ''' <param name="name">       Specifies the script name. </param>
    ''' <param name="timeout">    Specifies the time to wait for the instrument to return operation
    ''' completed. </param>
    Public Sub RunScript(ByVal nodeNumber As Integer, ByVal name As String, ByVal timeout As TimeSpan)

        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                   "Running script '{0}' on node {1};. ", name, nodeNumber)
        Me.Session.LastNodeNumber = nodeNumber

        Dim message As String = Me.Session.ExecuteCommand(nodeNumber, "{0}.run()", name)
        Me.CheckThrowDeviceException(False, message)

        Me.LinkSubsystem.WaitComplete(nodeNumber, timeout, False)
        Me.CheckThrowDeviceException(False, "running script '{0}' using '{1}';. ", name, message)

        Me.Session.LastNodeNumber = New Integer?

    End Sub

    ''' <summary> Runs the script. </summary>
    ''' <remarks> Waits for operation completion. </remarks>
    ''' <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    ''' null. </exception>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="script"> Specifies the script. </param>
    ''' <param name="node">   Specifies the subsystem node. </param>
    Public Sub RunScript(ByVal script As ScriptEntityBase, ByVal node As NodeEntityBase)

        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If String.IsNullOrWhiteSpace(script.Name) Then Throw New InvalidOperationException("script name is empty")

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                   "Running script '{0}' on node {1};. ", Name, node.Number)
        Me.Session.LastNodeNumber = node.Number

        Dim message As String = Me.Session.ExecuteCommand(node.Number,
                                                             "node[{1}].execute( '{0}.run()' ) waitcomplete({1})", script.Name, node.Number)
        Me.CheckThrowDeviceException(True, message)

        Me.LinkSubsystem.WaitComplete(node.Number, script.Timeout, True)
        Me.CheckThrowDeviceException(True, "running script '{0}' using '{1}';. ", Name, message)

        Me.Session.LastNodeNumber = New Integer?

    End Sub

#End Region

#Region " SAVE SCRIPT "

    Private _SaveTimeout As TimeSpan = TimeSpan.Zero

    ''' <summary> Gets or sets the time out for saving a script. </summary>
    ''' <value> The save timeout. </value>
    Public Property SaveTimeout() As TimeSpan
        Get
            Return Me._SaveTimeout
        End Get
        Set(ByVal Value As TimeSpan)
            If Not Me.SaveTimeout.Equals(Value) Then
                Me._SaveTimeout = Value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Saves the specifies script to non-volatile memory. </summary>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <param name="name"> Gets or sets the script name. </param>
    ''' <returns> <c>True</c> is script exist; otherwise, <c>False</c>. </returns>
    Public Function SaveScript(ByVal name As String) As Boolean
        If String.IsNullOrWhiteSpace(name) Then
            Return False
        End If

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                   "saving script '{0}';. ", name)
        Me.Session.LastNodeNumber = New Integer?
        Me.Session.WriteLine("{0}.save()", name)
        Return Me.SavedScriptExists(name, True)
    End Function

#End Region

#Region " FIND SCRIPT "

    ''' <summary> Checks if the specified script exists as a saved script. </summary>
    ''' <param name="name"> Specifies the script name. </param>
    ''' <returns> <c>True</c> if the specified script exists as a saved script; otherwise,
    ''' <c>False</c>. </returns>
    Public Function LastSavedScriptExists(ByVal name As String) As Boolean
        Return Me.SavedScriptExists(name, False)
    End Function

    ''' <summary> Checks <c>True</c> if the specified script exists as a saved script on the remote node. </summary>
    ''' <param name="name"> Specifies the script name. </param>
    ''' <returns> <c>True</c> if the specified script exists as a saved script on the remote node;
    ''' otherwise, <c>False</c>. </returns>
    Public Function LastSavedRemoteScriptExists(ByVal name As String) As Boolean
        If String.IsNullOrWhiteSpace(name) Then
            Return False
        End If
        Return Me._LastFetchedSavedRemoteScripts.IndexOf(name & ",", 0, StringComparison.OrdinalIgnoreCase) >= 0
    End Function

    ''' <summary>
    ''' Checks if the specified script exists as a saved script.
    ''' </summary>
    ''' <param name="name">Specifies the script name</param>
    ''' <param name="refreshScriptCatalog">True to refresh the list of saved scripts.</param>
    ''' <returns> <c>True</c> if the specified script exists as a saved script; otherwise, <c>False</c>. </returns>
    Public Function SavedScriptExists(ByVal name As String, ByVal refreshScriptCatalog As Boolean) As Boolean
        If refreshScriptCatalog Then
            Me.FetchSavedScripts()
        End If
        Return Me.LastFetchedSavedScripts.IndexOf(name & ",", 0, StringComparison.OrdinalIgnoreCase) >= 0
    End Function

    ''' <summary> Returns <c>True</c> if the specified script exists as a saved script. </summary>
    ''' <param name="node">                 Specifies the node to validate. </param>
    ''' <param name="name">                 Specifies the script name. </param>
    ''' <param name="refreshScriptCatalog"> True to refresh the list of saved scripts. </param>
    ''' <returns> <c>True</c> if the specified script exists as a saved script on the remote node;
    ''' otherwise, <c>False</c>. </returns>
    Public Function SavedScriptExists(ByVal node As NodeEntityBase, ByVal name As String, ByVal refreshScriptCatalog As Boolean) As Boolean
        If refreshScriptCatalog Then
            Me.FetchSavedScripts(node)
        End If
        Return Me.LastSavedRemoteScriptExists(name)
    End Function

    ''' <summary> Returns <c>True</c> if the specified script exists as a saved script. </summary>
    ''' <param name="nodeNumber">           Specifies the remote node number to validate. </param>
    ''' <param name="name">                 Specifies the script name. </param>
    ''' <param name="refreshScriptCatalog"> True to refresh the list of saved scripts. </param>
    ''' <returns> <c>True</c> if the specified script exists as a saved script on the remote node;
    ''' otherwise, <c>False</c>. </returns>
    Public Function SavedScriptExists(ByVal nodeNumber As Integer, ByVal name As String, ByVal refreshScriptCatalog As Boolean) As Boolean
        If refreshScriptCatalog Then
            Me.FetchSavedScripts(nodeNumber)
        End If
        Return Me.LastSavedRemoteScriptExists(name)
    End Function

    ''' <summary> Returns <c>True</c> if the specified script exists as a saved script. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="name">                 Specifies the script name. </param>
    ''' <param name="node">                 Specifies the node to validate. </param>
    ''' <param name="refreshScriptCatalog"> True to refresh the list of saved scripts. </param>
    ''' <returns> <c>True</c> if the specified script exists as a saved script; otherwise,
    ''' <c>False</c>. </returns>
    Public Function SavedScriptExists(ByVal name As String, ByVal node As NodeEntityBase, ByVal refreshScriptCatalog As Boolean) As Boolean

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If node.IsController Then
            Return SavedScriptExists(name, refreshScriptCatalog)
        Else
            Return SavedScriptExists(node.Number, name, refreshScriptCatalog)
        End If
    End Function

    ''' <summary> Checks if the script is not nil. </summary>
    ''' <param name="name"> The script name. </param>
    ''' <returns> <c>True</c> is script exist; otherwise, <c>False</c>. </returns>
    Public Function ScriptExists(ByVal name As String) As Boolean
        If String.IsNullOrWhiteSpace(name) Then
            Return False
        End If
        Return Not Me.Session.IsNil(name)
    End Function

#End Region

#Region " PARSE SCRIPT "

    ''' <summary> Parses the script. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="source">        specifies the source code for the script. </param>
    ''' <param name="retainOutline"> Specifies if the code outline is retained or trimmed. </param>
    ''' <returns> Parsed script. </returns>
    Public Shared Function ParseScript(ByVal source As String, ByVal retainOutline As Boolean) As String

        If source Is Nothing Then Throw New ArgumentNullException(NameOf(source))
        Dim sourceLines As String() = source.Split(Environment.NewLine.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
        Dim newSource As New System.Text.StringBuilder
        Dim isInCommentBlock As Boolean = False
        Dim wasInCommentBlock As Boolean
        Dim lineType As TspChunkLineContentType
        For Each chunkLine As String In sourceLines

            chunkLine = ScriptManagerBase.TrimTspChuckLine(chunkLine, retainOutline)
            wasInCommentBlock = isInCommentBlock
            lineType = ScriptManagerBase.ParseTspChuckLine(chunkLine, isInCommentBlock)

            If lineType = TspChunkLineContentType.None Then

                ' if no data, nothing to do.

            ElseIf wasInCommentBlock Then

                ' if was in a comment block exit the comment block if
                ' received a end of comment block
                If lineType = TspChunkLineContentType.EndCommentBlock Then
                    isInCommentBlock = False
                End If

            ElseIf lineType = TspChunkLineContentType.StartCommentBlock Then

                isInCommentBlock = True

            ElseIf lineType = TspChunkLineContentType.Comment Then

                ' if comment line do nothing

            ElseIf lineType = TspChunkLineContentType.Syntax OrElse lineType = TspChunkLineContentType.SyntaxStartCommentBlock Then

                If lineType = TspChunkLineContentType.SyntaxStartCommentBlock Then

                    chunkLine = chunkLine.Substring(0, chunkLine.IndexOf(TspSyntax.StartCommentChunk, StringComparison.OrdinalIgnoreCase))

                End If

                If Not String.IsNullOrWhiteSpace(chunkLine) Then
                    newSource.AppendLine(chunkLine)
                End If

                If lineType = TspChunkLineContentType.SyntaxStartCommentBlock Then

                    isInCommentBlock = True

                End If

            End If

        Next

        Return newSource.ToString

    End Function

    ''' <summary> Parses a TSP chuck line.  This assumes that the line was trimmed. </summary>
    ''' <param name="value">            Specifies the line. </param>
    ''' <param name="isInCommentBlock"> <c>True</c> if this object is in comment block. </param>
    ''' <returns> The parsed <see cref="TspChunkLineContentType">content type.</see> </returns>
    Public Shared Function ParseTspChuckLine(ByVal value As String, ByVal isInCommentBlock As Boolean) As TspChunkLineContentType

        If value Is Nothing Then value = ""

        value = value.Trim.Replace(Convert.ToChar(9, Globalization.CultureInfo.CurrentCulture), " ").Trim

        If String.IsNullOrWhiteSpace(value) Then

            Return TspChunkLineContentType.None

            ' check if start of comment block
        ElseIf value.StartsWith(TspSyntax.StartCommentChunk, StringComparison.OrdinalIgnoreCase) Then

            Return TspChunkLineContentType.StartCommentBlock

        ElseIf value.Contains(TspSyntax.StartCommentChunk) Then

            Return TspChunkLineContentType.SyntaxStartCommentBlock

            ' check if in a comment block
        ElseIf isInCommentBlock AndAlso value.Contains(TspSyntax.EndCommentChunk) Then

            ' check if end of comment block
            Return TspChunkLineContentType.EndCommentBlock

            ' skip comment lines.
        ElseIf value.StartsWith(TspSyntax.CommentChunk, StringComparison.OrdinalIgnoreCase) Then

            Return TspChunkLineContentType.Comment

        Else

            Return TspChunkLineContentType.Syntax

        End If

    End Function

    ''' <summary> Trims the TSP chuck line. </summary>
    ''' <param name="value">         Specifies the line. </param>
    ''' <param name="retainOutline"> Specifies if the code outline is retained or trimmed. </param>
    ''' <returns> The trimmed chunk line. </returns>
    Public Shared Function TrimTspChuckLine(ByVal value As String, ByVal retainOutline As Boolean) As String

        If String.IsNullOrWhiteSpace(value) Then

            Return ""

        Else

            If retainOutline Then

                ' remove leading and lagging spaces and horizontal tabs
                Return value.Replace(Convert.ToChar(9, Globalization.CultureInfo.CurrentCulture), "  ").TrimEnd

            Else

                ' remove leading and lagging spaces and horizontal tabs
                Return value.Replace(Convert.ToChar(9, Globalization.CultureInfo.CurrentCulture), " ").Trim

            End If

            If String.IsNullOrWhiteSpace(value) Then

                Return ""

                ' check if start of comment block
            ElseIf value.Contains(TspSyntax.StartCommentChunk) Then

                ' return the start of comment chunk
                Return TspSyntax.StartCommentChunk

                ' check if end of comment block
            ElseIf value.Contains(TspSyntax.EndCommentChunk) Then

                ' return the end of comment chunk
                Return TspSyntax.EndCommentChunk

                ' check if a comment line
            ElseIf value.Substring(0, 2) = TspSyntax.CommentChunk Then

                ' return the comment chunk
                Return TspSyntax.CommentChunk

            Else

                ' remove a trailing comment.  This cannot be easily done
                ' become of commands such as
                ' print( '----' )
                ' print( " --- -" )
                If value.Contains("""") Then
                    Return value
                ElseIf value.Contains("'") Then
                    Return value
                Else
                    ' if no text in the line, we can safely remove a trailing comment.
                    If value.Contains(TspSyntax.CommentChunk) Then
                        value = value.Substring(0, value.IndexOf(TspSyntax.CommentChunk, StringComparison.OrdinalIgnoreCase))
                    End If
                    Return value
                End If

            End If

        End If

    End Function

#End Region

#Region " OPEN FILE "

    ''' <summary> Opens a script file as a text reader. </summary>
    ''' <param name="filePath"> Specifies the script file path. </param>
    ''' <returns> A reference to an open
    ''' <see cref="System.IO.TextReader">Text Stream</see>. </returns>
    Public Shared Function OpenScriptFile(ByVal filePath As String) As System.IO.StreamReader

        ' Check name
        If String.IsNullOrWhiteSpace(filePath) OrElse Not System.IO.File.Exists(filePath) Then
            Return Nothing
            Exit Function
        End If
        Return New System.IO.StreamReader(filePath)

    End Function

    ''' <summary> Opens a script file as a text reader. </summary>
    ''' <returns> A reference to an open
    ''' <see cref="System.IO.TextReader">Text Stream</see>. </returns>
    Public Function OpenScriptFile() As System.IO.StreamReader

        ' Check name
        If String.IsNullOrWhiteSpace(Me._Name) OrElse String.IsNullOrWhiteSpace(Me._FilePath) OrElse Not System.IO.File.Exists(Me.FilePath) Then
            Return Nothing
            Exit Function
        End If

        Return New System.IO.StreamReader(Me.FilePath)

    End Function

#End Region

#Region " READ AND WRITE "

    ''' <summary> Reads the script from the script file. </summary>
    ''' <param name="filePath"> Specifies the script file path. </param>
    ''' <returns> The script. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Shared Function ReadScript(ByVal filePath As String) As String

        Using tspFile As System.IO.StreamReader = ScriptManagerBase.OpenScriptFile(filePath)
            Return tspFile.ReadToEnd()
        End Using

    End Function

    ''' <summary> Writes the script to file. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="source">   specifies the source code for the script. </param>
    ''' <param name="filePath"> Specifies the script file path. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Shared Sub WriteScript(ByVal source As String, ByVal filePath As String)

        If source Is Nothing Then Throw New ArgumentNullException(NameOf(source))
        Using tspFile As System.IO.StreamWriter = New System.IO.StreamWriter(filePath)
            tspFile.Write(source)
        End Using

    End Sub

#End Region

#Region " TSP SCRIPTS: PARSE, READ, WRITE "

    ''' <summary> Reads the scripts, parses them and saves them to file. </summary>
    ''' <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ''' null. </exception>
    ''' <exception cref="System.IO.FileNotFoundException"> Thrown when the requested file is not
    ''' present. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="filePath">      Specifies the folder where scripts are stored. </param>
    ''' <param name="retainOutline"> Specifies if the code outline is retained or trimmed. </param>
    Public Shared Sub ReadParseWriteScript(ByVal filePath As String, ByVal retainOutline As Boolean)

        If String.IsNullOrWhiteSpace(filePath) Then Throw New ArgumentNullException(NameOf(filePath))

        Dim scriptSource As String = ""
        ' check if file exists.
        If ScriptEntityBase.FileSize(filePath) <= 2 Then
            Throw New System.IO.FileNotFoundException("Script file not found", filePath)
        Else
            scriptSource = ScriptManagerBase.ReadScript(filePath)
            If String.IsNullOrWhiteSpace(scriptSource) Then
                Throw New VI.Pith.OperationFailedException("Failed reading script;. file '{0}' includes no source.", filePath)
            Else
                scriptSource = ScriptManagerBase.ParseScript(scriptSource, retainOutline)
                If String.IsNullOrWhiteSpace(scriptSource) Then
                    Throw New VI.Pith.OperationFailedException("Failed reading script;. parsed script from '{0}' is empty.", filePath)
                Else
                    filePath = filePath & ".debug"
                    ScriptManagerBase.WriteScript(scriptSource, filePath)
                End If

            End If
        End If

    End Sub

    ''' <summary> Reads the scripts, parses them and saves them to file. </summary>
    ''' <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    ''' null. </exception>
    ''' <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    ''' <param name="instrumentModelNumber"> The instrument model number. </param>
    ''' <param name="folderPath">           Specifies the folder where scripts are stored. </param>
    ''' <param name="scripts">              Specifies the collection of scripts. </param>
    ''' <param name="retainOutline">        Specifies if the code outline is retained or trimmed. </param>
    Public Shared Sub ReadParseWriteScripts(ByVal instrumentModelNumber As String, ByVal folderPath As String,
                                            ByVal scripts As ScriptEntityCollection, ByVal retainOutline As Boolean)
        If instrumentModelNumber Is Nothing Then Throw New ArgumentNullException(NameOf(instrumentModelNumber))
        If folderPath Is Nothing Then Throw New ArgumentNullException(NameOf(folderPath))
        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If scripts IsNot Nothing AndAlso scripts.Count > 0 Then
            For Each script As ScriptEntityBase In scripts
                If script.IsModelMatch(instrumentModelNumber) AndAlso script.RequiresReadParseWrite Then
                    If String.IsNullOrWhiteSpace(script.FileName) Then
                        Throw New InvalidOperationException(String.Format(Globalization.CultureInfo.CurrentCulture,
                                                                          "File name not specified for script '{0}'.",
                                                                          script.FileName))
                    Else
                        Dim filePath As String = System.IO.Path.Combine(folderPath, script.FileName)
                        ScriptManagerBase.ReadParseWriteScript(filePath, retainOutline)
                    End If
                End If
            Next
        End If

    End Sub

#End Region

#Region " TSP SCRIPTS: LOAD FROM FILE "

    ''' <summary> Loads a named script into the instrument. </summary>
    ''' <exception cref="System.IO.FileNotFoundException"> Thrown when the requested file is not present. </exception>
    ''' <exception cref="Pith.NativeException">         Thrown when a Visa error condition occurs. </exception>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="LoadScript")>
    Public Sub LoadScriptFileSimple()
        Me.LoadScriptFileSimple(Me.Name, Me.FilePath)
    End Sub

    ''' <summary> Loads a named script into the instrument allowing control over how errors and prompts
    ''' are handled. For loading a script that does not includes functions, turn off errors and turn
    ''' on the prompt. </summary>
    ''' <exception cref="System.IO.FileNotFoundException">  Thrown when the requested file is not present. </exception>
    ''' <exception cref="Pith.NativeException">          Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when a Script Loading error condition occurs. </exception>
    ''' <param name="showErrors">      Specifies the condition for turning off or on error checking
    ''' while the script is loaded. </param>
    ''' <param name="showPrompts">     Specifies the condition for turning off or on the TSP prompts
    ''' while the script is loaded. </param>
    ''' <param name="retainOutline">   Specifies if the code outline is retained or trimmed. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="LoadScript")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub LoadScriptFile(ByVal showErrors As Boolean, ByVal showPrompts As Boolean, ByVal retainOutline As Boolean)

        Dim activity As String = "loading script file"
        Dim actionDetails As String = ""

        ' store the status
        Me.InteractiveSubsystem.StoreStatus()

        Dim chunkLine As String

        Try

            Dim isInCommentBlock As Boolean
            isInCommentBlock = False

            Dim lineNumber As Integer
            lineNumber = 0

            ' flush the buffer until empty and update the TSP status.
            Me.Session.DiscardUnreadData()

            ' store state of prompts and errors.
            ' statusSubsystem.StoreStatus()

            ' Disable automatic display of errors - leave error messages in queue
            ' and enable error Prompt. or otherwise...
            Me.InteractiveSubsystem.WriteShowErrors(showErrors)

            ' Turn on prompts
            Me.InteractiveSubsystem.WriteShowPrompts(showPrompts)

            ' flush the buffer until empty and update the TSP status.
            Me.Session.DiscardUnreadData()

            Dim isFirstLine As Boolean
            isFirstLine = True
            Dim commandLine As String
            Dim wasInCommentBlock As Boolean
            Dim lineType As TspChunkLineContentType

            Using tspFile As System.IO.StreamReader = Me.OpenScriptFile()

                If tspFile Is Nothing Then
                    Throw New System.IO.FileNotFoundException("Failed opening TSP Script file", Me.FilePath)
                End If

                Using debugFile As System.IO.StreamWriter = New System.IO.StreamWriter(Me.FilePath & ".debug")

                    Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                               "sending a 'loadscript' for script '{0}' from file '{1}'",
                                                               Me._Name, Me._FilePath)
                    Me.Session.LastNodeNumber = New Integer?

                    Do While Not tspFile.EndOfStream

                        chunkLine = ScriptManagerBase.TrimTspChuckLine(tspFile.ReadLine, retainOutline)
                        lineNumber += 1
                        wasInCommentBlock = isInCommentBlock
                        lineType = ScriptManagerBase.ParseTspChuckLine(chunkLine, isInCommentBlock)

                        If lineType = TspChunkLineContentType.None Then

                            ' if no data, nothing to do.

                        ElseIf wasInCommentBlock Then

                            ' if was in a comment block exit the comment block if
                            ' received a end of comment block
                            If lineType = TspChunkLineContentType.EndCommentBlock Then
                                isInCommentBlock = False
                            End If

                        ElseIf lineType = TspChunkLineContentType.StartCommentBlock Then

                            isInCommentBlock = True

                        ElseIf lineType = TspChunkLineContentType.Comment Then

                            ' if comment line do nothing

                        ElseIf lineType = TspChunkLineContentType.Syntax OrElse lineType = TspChunkLineContentType.SyntaxStartCommentBlock Then

                            If lineType = TspChunkLineContentType.SyntaxStartCommentBlock Then

                                chunkLine = chunkLine.Substring(0, chunkLine.IndexOf(TspSyntax.StartCommentChunk, StringComparison.OrdinalIgnoreCase))

                            End If

                            ' end each line with a space
                            chunkLine &= " "

                            If isFirstLine Then
                                activity = $"{Me.ResourceNameCaption} sending a 'loadscript'"
                                actionDetails = $"sending a 'loadscript' for script '{Me.Name}' from file '{Me.FilePath}'"
                                ' issue a start of script command.  The command
                                ' 'loadscript' identifies the beginning of the named script.
                                commandLine = "loadscript " & Me._Name & " "
                                Me.Session.WriteLine(commandLine)
                                If Me.InteractiveSubsystem.ExecutionState <> TspExecutionState.IdleError Then
                                    isFirstLine = False
                                ElseIf (Me.StatusSubsystem.ReadServiceRequestStatus And Me.StatusSubsystem.ErrorAvailableBits) <> 0 Then
                                    ' done inside the query Me.StatusSubsystem.QueryStandardEventStatus()
                                    Dim e As New isr.Core.Pith.ActionEventArgs
                                    If Me.StatusSubsystem.TrySafeQueryDeviceErrors(e) Then
                                        Throw New VI.Pith.OperationFailedException($"{activity} failed;. {actionDetails}; Device errors: {Me.StatusSubsystem.DeviceErrorsReport}")
                                    Else
                                        Throw New VI.Pith.OperationFailedException($"{activity} failed;. {actionDetails}; Failed fetching device errors because {e.Details}")
                                    End If
                                Else
                                    Throw New VI.Pith.OperationFailedException($"{activity} failed;. {actionDetails}; No device errors")
                                End If
                            End If

                            Select Case Me.InteractiveSubsystem.ExecutionState
                                Case TspExecutionState.IdleContinuation
                                    ' Continuation prompt. TSP received script line successfully; waiting for next line.
                                Case TspExecutionState.IdleError
                                    System.Diagnostics.Debug.Assert(Not Debugger.IsAttached, "this should not happen :)")
                                Case TspExecutionState.IdleReady
                                    ' Ready prompt. TSP received script successfully; ready for next command.
                                    Exit Do
                                Case Else
                                    ' do nothing
                            End Select
                            activity = $"{Me.ResourceNameCaption} sending a syntax line"
                            actionDetails = $"sending a syntax line:
{chunkLine} 
for script {Me.Name} from file '{Me.FilePath}'"

                            Me.Session.WriteLine(chunkLine)
                            If Me.InteractiveSubsystem.ExecutionState = TspExecutionState.IdleError Then
                                ' now report the error to the calling module
                                If (Me.StatusSubsystem.ReadServiceRequestStatus And Me.StatusSubsystem.ErrorAvailableBits) <> 0 Then
                                    ' done inside the query Me.StatusSubsystem.QueryStandardEventStatus()
                                    Dim e As New isr.Core.Pith.ActionEventArgs
                                    If Me.StatusSubsystem.TrySafeQueryDeviceErrors(e) Then
                                        Throw New VI.Pith.OperationFailedException($"{activity} failed;. {actionDetails}; Device errors: {Me.StatusSubsystem.DeviceErrorsReport}")
                                    Else
                                        Throw New VI.Pith.OperationFailedException($"{activity} failed;. {actionDetails}; Failed fetching device errors because {e.Details}")
                                    End If
                                Else
                                    Throw New VI.Pith.OperationFailedException($"{activity} failed;. {actionDetails}; No device errors")
                                End If
                            Else
                                ' increment debug line number
                                debugFile.WriteLine(chunkLine)
                            End If

                            Select Case Me.InteractiveSubsystem.ExecutionState
                                Case TspExecutionState.IdleError
                                    System.Diagnostics.Debug.Assert(Not Debugger.IsAttached, "this should not happen :)")
                                Case TspExecutionState.IdleReady
                                    Exit Do
                                Case Else
                                    ' do nothing
                            End Select

                            If lineType = TspChunkLineContentType.SyntaxStartCommentBlock Then

                                isInCommentBlock = True

                            End If

                        End If

                    Loop

                End Using

            End Using


            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "ending loaded script '{0}';. ", Me.Name)
            Me.Session.LastNodeNumber = New Integer?

            activity = $"{Me.ResourceNameCaption} sending an 'endscript'"
            actionDetails = $"sending an 'endscript' for script '{Me.Name}' from file '{Me.FilePath}'"

            ' Tell TSP complete script has been downloaded.
            commandLine = "endscript waitcomplete() print('1') "
            Me.Session.WriteLine(commandLine)

            If Me.InteractiveSubsystem.ExecutionState = TspExecutionState.IdleError Then
                If (Me.StatusSubsystem.ReadServiceRequestStatus And Me.StatusSubsystem.ErrorAvailableBits) <> 0 Then
                    ' done inside the query Me.StatusSubsystem.QueryStandardEventStatus()
                    Dim e As New isr.Core.Pith.ActionEventArgs
                    If Me.StatusSubsystem.TrySafeQueryDeviceErrors(e) Then
                        Throw New VI.Pith.OperationFailedException($"{activity} failed;. {actionDetails}; Device errors: {Me.StatusSubsystem.DeviceErrorsReport}")
                    Else
                        Throw New VI.Pith.OperationFailedException($"{activity} failed;. {actionDetails}; Failed fetching device errors because {e.Details}")
                    End If
                Else
                    Throw New VI.Pith.OperationFailedException($"{activity} failed;. {actionDetails}; No device errors")
                End If
            End If

            ' wait till we get a reply from the instrument or timeout.

            ' The command above does not seem to work!  It looks like the print does not get executed!
            Dim endTime As DateTime = DateTime.Now.Add(TimeSpan.FromMilliseconds(3000))
            Dim value As String = ""
            Do
                Do
                    Threading.Thread.Sleep(50)
                Loop Until endTime < DateTime.Now Or Me.Session.IsMessageAvailable
                If Me.Session.IsMessageAvailable Then
                    value = Me.Session.ReadLine
                    If Not value.StartsWith("1", StringComparison.OrdinalIgnoreCase) Then
                        Me.InteractiveSubsystem.ParseExecutionState(value, TspExecutionState.IdleReady)
                    End If
                End If
            Loop Until endTime < DateTime.Now Or value.StartsWith("1", StringComparison.OrdinalIgnoreCase)
            If endTime < DateTime.Now Then
                'Throw New isr.Tsp.ScriptCallException("Timeout waiting operation completion loading the script '" & Me._name & "'")
            End If

            ' add a wait to ensure the system returns the last status.
            Threading.Thread.Sleep(100)

            ' flush the buffer until empty to completely reading the status.
            Me.Session.DiscardUnreadData()

            ' get the script state if showing prompts
            Select Case Me.InteractiveSubsystem.ExecutionState
                Case TspExecutionState.IdleError
                    System.Diagnostics.Debug.Assert(Not Debugger.IsAttached, "this should not happen :)")
                Case Else
                    ' do nothing
            End Select

        Catch

            ' remove any remaining values.
            Me.Session.DiscardUnreadData()

            Throw

        Finally

            ' restore state of prompts and errors.
            Me.InteractiveSubsystem.RestoreStatus()

        End Try

    End Sub


    ''' <summary> Loads a named script into the instrument. </summary>
    ''' <exception cref="system.IO.FileNotFoundException"> Thrown when the requested file is not present. </exception>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="name">     Specifies the script name. </param>
    ''' <param name="filePath"> The file path. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="LoadScript")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub LoadScriptFileSimple(ByVal name As String, ByVal filePath As String)

        Try

            Using tspFile As System.IO.StreamReader = ScriptManagerBase.OpenScriptFile(filePath)
                If tspFile Is Nothing Then
                    Throw New System.IO.FileNotFoundException("Failed opening script file", filePath)
                End If
                Dim line As String
                Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                           "load script {0} from {1};. ", name, filePath)
                Me.Session.LastNodeNumber = New Integer?
                Me.Session.WriteLine("loadscript {0}", name)
                Do Until tspFile.EndOfStream
                    line = tspFile.ReadLine.Trim
                    If Not String.IsNullOrWhiteSpace(line) Then
                        Me.Session.WriteLine(line)
                    End If
                Loop
                Me.Session.WriteLine("endscript")
                Me.CheckThrowDeviceException(True, "loading script '{0}';. last line was '{1}'", name, Me.Session.LastMessageSent)
            End Using

        Catch

            ' remove any remaining values.
            Me.Session.DiscardUnreadData()

            Throw

        Finally
        End Try

    End Sub

    ''' <summary> Loads a named script into the instrument allowing control over how errors and prompts
    ''' are handled. For loading a script that does not includes functions, turn off errors and turn
    ''' on the prompt. </summary>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when a Script Loading error condition occurs. </exception>
    ''' <param name="name">            Specifies the script name. </param>
    ''' <param name="filePath">        Specifies the script file name. </param>
    ''' <param name="retainOutline">   Specifies if the code outline is retained or trimmed. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="LoadScript")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub LoadScriptFileLegacy(ByVal name As String, ByVal filePath As String, ByVal retainOutline As Boolean)

        Dim chunkLine As String
        Dim commandLine As String
        Try

            Using tspFile As System.IO.StreamReader = ScriptManagerBase.OpenScriptFile(filePath)

                If tspFile Is Nothing Then

                    ' now report the error to the calling module
                    Throw New System.IO.IOException("Failed opening TSP Script File '" & filePath & "'.")

                End If

                Using debugFile As System.IO.StreamWriter = New System.IO.StreamWriter(filePath & ".debug")

                    Dim isInCommentBlock As Boolean
                    isInCommentBlock = False

                    Dim lineNumber As Integer
                    lineNumber = 0

                    Dim isFirstLine As Boolean
                    isFirstLine = True
                    Dim wasInCommentBlock As Boolean
                    Dim lineType As TspChunkLineContentType
                    Do While Not tspFile.EndOfStream

                        chunkLine = tspFile.ReadLine()
                        chunkLine = ScriptManagerBase.TrimTspChuckLine(chunkLine, retainOutline)
                        lineNumber = lineNumber + 1
                        wasInCommentBlock = isInCommentBlock
                        lineType = ScriptManagerBase.ParseTspChuckLine(chunkLine, isInCommentBlock)

                        If lineType = TspChunkLineContentType.None Then

                            ' if no data, nothing to do.

                        ElseIf wasInCommentBlock Then

                            ' if was in a comment block exit the comment block if
                            ' received a end of comment block
                            If lineType = TspChunkLineContentType.EndCommentBlock Then
                                isInCommentBlock = False
                            End If

                        ElseIf lineType = TspChunkLineContentType.StartCommentBlock Then

                            isInCommentBlock = True

                        ElseIf lineType = TspChunkLineContentType.Comment Then

                            ' if comment line do nothing

                        ElseIf lineType = TspChunkLineContentType.Syntax OrElse lineType = TspChunkLineContentType.SyntaxStartCommentBlock Then

                            If lineType = TspChunkLineContentType.SyntaxStartCommentBlock Then

                                chunkLine = chunkLine.Substring(0, chunkLine.IndexOf(TspSyntax.StartCommentChunk, StringComparison.OrdinalIgnoreCase))

                            End If

                            ' end each line with a space
                            chunkLine &= " "

                            If isFirstLine Then
                                ' issue a start of script command.  The command
                                ' 'loadscript' identifies the beginning of the named script.
                                commandLine = "loadscript " & name & " "
                                Me.Session.WriteLine(commandLine)
                                isFirstLine = False
                                Me.CheckThrowDeviceException(True,
                                                             "sending a 'loadscript' for script '{2}';. from file '{1}'",
                                                             name, filePath)
                            End If

                            Me.Session.WriteLine(chunkLine)

                            ' increment debug line number
                            debugFile.WriteLine(chunkLine)

                            If lineType = TspChunkLineContentType.SyntaxStartCommentBlock Then

                                isInCommentBlock = True

                            End If

                        End If

                    Loop

                End Using

            End Using

            ' Tell TSP complete script has been downloaded.
            commandLine = "endscript waitcomplete() print('1') "
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                       "sending an 'endscript' for script '{0}'; from file '{1}'", name, filePath)
            Me.Session.WriteLine(commandLine)

            ' wait till we get a reply from the instrument or timeout.
            Dim endTime As DateTime = DateTime.Now.Add(TimeSpan.FromMilliseconds(3000))
            Dim value As String = ""
            Do
                Do
                    Threading.Thread.Sleep(50)
                Loop Until endTime < DateTime.Now Or Me.Session.IsMessageAvailable
                If Me.Session.IsMessageAvailable Then
                    value = Me.Session.ReadLine
                End If
            Loop Until endTime < DateTime.Now Or value.StartsWith("1", StringComparison.OrdinalIgnoreCase)
            If endTime < DateTime.Now Then
                'Throw New isr.Tsp.ScriptCallException("Timeout waiting operation completion loading the script '" & Me._name & "'")
            End If

            ' add a wait to ensure the system returns the last status.
            Threading.Thread.Sleep(100)

            ' flush the receive buffer until empty.
            Me.Session.DiscardUnreadData()


        Catch

            ' flush the receive buffer until empty.
            Me.Session.DiscardUnreadData()

            Throw

        Finally
        End Try

    End Sub

    ''' <summary> Loads the specified TSP script from file. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="system.IO.FileNotFoundException"> Thrown when the requested file is not present. </exception>
    ''' <param name="folderPath">      Specifies the script folder path. </param>
    ''' <param name="script">          Specifies reference to a valid
    ''' <see cref="ScriptEntityBase">script</see> </param>
    Public Sub LoadUserScriptFile(ByVal folderPath As String, ByVal script As ScriptEntityBase)

        If folderPath Is Nothing Then Throw New ArgumentNullException(NameOf(folderPath))
        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))

        If Not Me.Session.IsNil(script.Name) Then
            ' script already exists
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                              "Instrument '{0}' script {1} already exists;. ", Me.ResourceNameCaption, script.Name)
            Return
        End If

        Dim filePath As String = System.IO.Path.Combine(folderPath, script.FileName)
        ' check if file exists.
        If ScriptEntityBase.FileSize(filePath) <= 2 Then
            Throw New System.IO.FileNotFoundException("Script file not found or is empty;. ", script.FileName)
        End If

        Me.DisplaySubsystem.DisplayLine(2, "Loading {0} from file", script.Name)
        Try
            Me.LoadScriptFileLegacy(script.Name, filePath, True)
        Catch
            Me.DisplaySubsystem.DisplayLine(2, "Failed loading {0} from file", script.Name)
            Throw
        End Try

        ' do a garbage collection
        If Not Me.StatusSubsystem.CollectGarbageWaitComplete(script.Timeout, "collecting garbage;. ") Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              "Ignoring instrument '{0}' error(s) collecting garbage after loading {1};. {2}{3}",
                              Me.ResourceNameCaption, script.Name, Environment.NewLine, New StackFrame(True).UserCallStack())
        End If

        Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Instrument '{0}' {1} script loaded;. ", Me.ResourceNameCaption, script.Name)

        Me.DisplaySubsystem.DisplayLine(2, "Done loading {0} from file", script.Name)

    End Sub

#End Region

#Region " TSP SCRIPTS: LOAD "

    ''' <summary> Load the code. Code could be embedded as a comma separated string table format, in
    ''' which case the script should be concatenated first. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value"> Includes the script as a long string. </param>
    Public Sub LoadString(ByVal value As String)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Dim scriptLines As String() = value.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
        Me.LoadString(scriptLines)
    End Sub

    ''' <summary> Load the code. Code could be embedded as a comma separated string table format, in
    ''' which case the script should be concatenated first. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scriptLines"> Includes the script in lines. </param>
    Public Sub LoadString(ByVal scriptLines() As String)

        If scriptLines Is Nothing Then Throw New ArgumentNullException(NameOf(scriptLines))

        If scriptLines IsNot Nothing Then
            For Each scriptLine As String In scriptLines
                scriptLine = scriptLine.Trim
                If Not String.IsNullOrWhiteSpace(scriptLine) Then
                    Me.Session.WriteLine(scriptLine)
                End If
            Next
        End If
    End Sub

    ''' <summary> Loads the script embedded in the string. </summary>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <param name="scriptLines"> Contains the script code line by line. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="LoadScript")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub LoadScript(ByVal scriptLines() As String)

        Try

            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                       "initiating load script for script '{0}';. ", Me.Name)
            Me.Session.LastNodeNumber = New Integer?

            Me.Session.WriteLine("loadscript " & Me.Name)

            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "loading script '{0}';. ", Me.Name)
            Me.LoadString(scriptLines)

            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "ending script '{0}';. ", Me.Name)
            Me.Session.WriteLine("endscript")

        Catch

            ' remove any remaining values.
            Me.Session.DiscardUnreadData()

            Throw

        End Try

    End Sub

    ''' <summary> Loads the script embedded in the string. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="Pith.NativeException">         Thrown when a Visa error condition occurs. </exception>
    ''' <param name="name">        Contains the script name. </param>
    ''' <param name="scriptLines"> Contains the script code line by line. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="LoadScript")>
    Public Sub LoadScript(ByVal name As String, ByVal scriptLines() As String)

        If scriptLines Is Nothing Then Throw New ArgumentNullException(NameOf(scriptLines))

        Dim firstLine As String = scriptLines(0)
        ' check if we already have the load/end constructs.
        Me.Session.LastNodeNumber = New Integer?
        If firstLine.Contains(name) OrElse firstLine.Contains("loadscript") Then
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "loading script '{0}';. ", name)
            Me.Session.LastNodeNumber = New Integer?
            Me.LoadString(scriptLines)
        Else
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                       "initiating load script for script '{0}';. ", name)
            Me.Session.WriteLine("loadscript " & name)

            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                       "loading script lines for script '{0}';. ", name)
            Me.LoadString(scriptLines)

            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                       "ending script for script '{0}';. ", name)
            Me.Session.WriteLine("endscript waitcomplete()")
        End If

    End Sub

    ''' <summary> Loads the script embedded in the string. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="name">   Contains the script name. </param>
    ''' <param name="source"> Contains the script code line by line. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="LoadScript")>
    Public Sub LoadScript(ByVal name As String, ByVal source As String)

        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException(NameOf(name))
        If String.IsNullOrWhiteSpace(source) Then Throw New ArgumentNullException(NameOf(source))
        If source.Substring(0, 50).Trim.StartsWith("{", True, Globalization.CultureInfo.CurrentCulture) Then
            source = "loadstring(table.concat(" & source & "))() "
        End If
        Dim scriptLines() As String = source.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
        Me.LoadScript(name, scriptLines)

    End Sub

    ''' <summary> Loads an anonymous script embedded in the string. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="source"> Contains the script code line by line. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="LoadScript")>
    Public Sub LoadScript(ByVal source As String)

        If source Is Nothing Then Throw New ArgumentNullException(NameOf(source))
        If source.Substring(0, 50).Trim.StartsWith("{", True, Globalization.CultureInfo.CurrentCulture) Then
            source = "loadstring(table.concat(" & source & "))() "
        End If
        Me.LoadString(source)

    End Sub

#End Region

#Region " TSP X: SCRIPT COLLECTION  - LEGACY "

    Private _LegacyScripts As ScriptEntityCollection

    ''' <summary> Gets the list of legacy scripts. </summary>
    ''' <returns> List of legacy scripts. </returns>
    Public Function LegacyScripts() As ScriptEntityCollection
        Return Me._LegacyScripts
    End Function

    ''' <summary> Adds a new script to the list of legacy scripts. </summary>
    ''' <param name="name"> Specifies the name of the script. </param>
    ''' <returns> The added script. </returns>
    Public Function AddLegacyScript(ByVal name As String) As ScriptEntityBase

        Dim script As New ScriptEntity(name, "")
        Me._LegacyScripts.Add(script)
        Return script

    End Function

    ''' <summary> Create a new instance of the legacy scripts. </summary>
    Public Sub NewLegacyScripts()
        Me._LegacyScripts = New ScriptEntityCollection
    End Sub

#End Region

#Region " TSP X: SCRIPT COLLECTION "

    Private _Scripts As ScriptEntityCollection

    ''' <summary> Gets the list of scripts. </summary>
    ''' <returns> List of scripts. </returns>
    Public Function Scripts() As ScriptEntityCollection
        Return Me._Scripts
    End Function

    ''' <summary> Adds a new script to the list of scripts. </summary>
    ''' <param name="name">      Specifies the name of the script. </param>
    ''' <param name="modelMask"> Specifies the family of instrument models for this script. </param>
    ''' <returns> The added script. </returns>
    Public Function AddScript(ByVal name As String, ByVal modelMask As String) As ScriptEntityBase

        Dim script As New ScriptEntity(name, modelMask)
        Me._Scripts.Add(script)
        Return script

    End Function

    ''' <summary> Create a new instance of the scripts. </summary>
    Public Sub NewScripts()
        Me._Scripts = New ScriptEntityCollection()
    End Sub

#End Region

#Region " TSP X: SCRIPTS "

    ''' <summary> Checks and returns <c>True</c> if all scripts are loaded on the specified node. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts"> Specifies the collection of scripts. </param>
    ''' <param name="node">    Specifies the node. </param>
    ''' <returns> <c>True</c> if all scripts are loaded on the specified node. </returns>
    Public Function AllUserScriptsExist(ByVal scripts As ScriptEntityCollection, ByVal node As NodeEntityBase) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        If scripts IsNot Nothing AndAlso scripts.Count > 0 Then

            For Each script As ScriptEntityBase In scripts

                ' return <c>False</c> if any script is not loaded.
                If script.IsModelMatch(node.ModelNumber) AndAlso
                    Not Me.Session.IsNil(node.IsController, node.Number, script.Name) Then
                    Return False
                End If

            Next

        Else

            Return False

        End If
        Return True

    End Function

    ''' <summary> Checks and returns <c>True</c> if all scripts were executed. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts"> Specifies the collection of scripts. </param>
    ''' <param name="node">    Specifies the node. </param>
    ''' <returns> <c>True</c> if all scripts were executed on the specified node. </returns>
    Public Function AllUserScriptsExecuted(ByVal scripts As ScriptEntityCollection, ByVal node As NodeEntityBase) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        If scripts IsNot Nothing AndAlso scripts.Count > 0 Then

            For Each script As ScriptEntityBase In scripts

                ' return <c>False</c> if any script is not loaded.
                If script.IsModelMatch(node.ModelNumber) AndAlso
                   script.Namespaces IsNot Nothing AndAlso script.Namespaces.Length > 0 AndAlso Me.Session.IsNil(node.IsController, node.Number, script.Namespaces) Then
                    Return False
                End If

            Next

        Else

            Return False

        End If
        Return True

    End Function

    ''' <summary> Checks and returns <c>True</c> if all scripts are loaded. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts"> Specifies the collection of scripts. </param>
    ''' <param name="node">    Specifies the node. </param>
    ''' <returns> <c>True</c> if all scripts were saved on the specified node. </returns>
    Public Function AllUserScriptsSaved(ByVal scripts As ScriptEntityCollection, ByVal node As NodeEntityBase) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If scripts IsNot Nothing AndAlso scripts.Count > 0 Then
            Me.FetchSavedScripts(node)

            For Each script As ScriptEntityBase In scripts
                ' return <c>False</c> if any script is not saved.
                If script.IsModelMatch(node.ModelNumber) AndAlso Not Me.SavedScriptExists(script.Name, node, False) Then
                    Return False
                End If
            Next
        Else
            Return False
        End If
        Return True

    End Function

    ''' <summary>
    ''' Returns <c>True</c> if any of the specified scripts exists.
    ''' </summary>
    ''' <param name="scripts">Specifies the collection of scripts.</param>
    ''' <param name="node">Specifies the node.</param>
    ''' <returns> <c>True</c> if any of the specified scripts exists. </returns>
    Public Function AnyUserScriptExists(ByVal scripts As ScriptEntityCollection, ByVal node As NodeEntityBase) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        If scripts IsNot Nothing AndAlso scripts.Count > 0 Then
            For Each script As ScriptEntityBase In scripts
                If Not String.IsNullOrWhiteSpace(script.Name) Then
                    If script.IsModelMatch(node.ModelNumber) AndAlso Not Me.Session.IsNil(node.IsController, node.Number, script.Name) Then
                        Return True
                    End If
                End If
            Next
        End If
        Return False

    End Function

#End Region

#Region " TSP X: SCRIPTS FIRMWARE "

    ''' <summary> Returns the released main firmware version. </summary>
    ''' <returns> The released main firmware version. </returns>
    Public Function FirmwareReleasedVersionGetter() As String
        If Me.LinkSubsystem.ControllerNode IsNot Nothing Then
            Return Me.FirmwareReleasedVersionGetter(Me.LinkSubsystem.ControllerNode)
        Else
            Return Me._Scripts.Item(0).ReleasedFirmwareVersion
        End If
    End Function

    ''' <summary> Returns the released main firmware version. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> The released main firmware version. </returns>
    Public Function FirmwareReleasedVersionGetter(ByVal node As NodeEntityBase) As String

        If Me.LinkSubsystem.ControllerNode Is Nothing Then
            Return Me._Scripts.Item(0).ReleasedFirmwareVersion
        Else
            If node Is Nothing Then
                Throw New ArgumentNullException(NameOf(node))
            End If
            Return Me._Scripts.SelectSerialNumberScript(node).ReleasedFirmwareVersion
        End If
    End Function

    ''' <summary> Returns the embedded main firmware version. </summary>
    ''' <returns> The actual embedded firmware version. </returns>
    Public Function FirmwareVersionGetter() As String
        If Me.LinkSubsystem.ControllerNode IsNot Nothing Then
            Return Me.FirmwareVersionGetter(Me.LinkSubsystem.ControllerNode)
        Else
            Return "<unknown>"
        End If
    End Function

    ''' <summary> Returns the embedded main firmware version. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> The released main firmware version. </returns>
    Public Function FirmwareVersionGetter(ByVal node As NodeEntityBase) As String
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        Return Me._Scripts.SelectSerialNumberScript(node).EmbeddedFirmwareVersion
    End Function

    ''' <summary> Returns the main firmware name from the controller node. </summary>
    ''' <returns> The firmware name. </returns>
    Public Function FirmwareNameGetter() As String
        Return Me.FirmwareNameGetter(Me.LinkSubsystem.ControllerNode)
    End Function

    ''' <summary> Returns the node firmware name. </summary>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> The node firmware name. </returns>
    Public Function FirmwareNameGetter(ByVal node As NodeEntityBase) As String
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        Return Me._Scripts.SelectSerialNumberScript(node).Name
    End Function

    Private _FirmwareExists As Boolean?

    ''' <summary> Gets or sets the firmware exists. </summary>
    ''' <value> The firmware exists. </value>
    Public Property FirmwareExists As Boolean?
        Get
            Return Me._FirmwareExists
        End Get
        Set(value As Boolean?)
            If Not Nullable.Equals(value, Me.FirmwareExists) Then
                Me._FirmwareExists = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Checks if firmware exists on the controller node. </summary>
    ''' <returns> <c>True</c> if the firmware exists; otherwise, <c>False</c>. </returns>
    Public Function FindFirmware() As Boolean
        Return Me.FindFirmware(Me.LinkSubsystem.ControllerNode)
    End Function

    ''' <summary> Checks if the main firmware exists . </summary>
    ''' <remarks> Value is cached in the <see cref="FirmwareExists">sentinel</see> </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> <c>True</c> if the firmware exists; otherwise, <c>False</c>. </returns>
    Public Function FindFirmware(ByVal node As NodeEntityBase) As Boolean
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        Me._FirmwareExists = Not Me.Session.IsNil(Me.FirmwareNameGetter(node))
        Return Me._FirmwareExists.Value
    End Function

    ''' <summary> Returns the Support firmware name. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> <c>True</c> if the support firmware exists; otherwise, <c>False</c>. </returns>
    Public Function SupportFirmwareNameGetter(ByVal node As NodeEntityBase) As String
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        Return Me._Scripts.SelectSupportScript(node).Name
    End Function

    Private _SupportfirmwareExists As Boolean?

    ''' <summary> Gets or sets the firmware exists. </summary>
    ''' <value> The firmware exists. </value>
    Public Property SupportFirmwareExists1 As Boolean?
        Get
            Return Me._SupportfirmwareExists
        End Get
        Set(value As Boolean?)
            If Not Nullable.Equals(value, Me.SupportFirmwareExists1) Then
                Me._SupportfirmwareExists = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Checks if the Support firmware exists on the controller node. </summary>
    ''' <returns> <c>True</c> if the support firmware exists; otherwise, <c>False</c>. </returns>
    Public Function FindSupportFirmware() As Boolean
        Return Me.FindSupportFirmware(Me.LinkSubsystem.ControllerNode)
    End Function

    ''' <summary> Checks if the Support firmware exists. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> <c>True</c> if the support firmware exists; otherwise, <c>False</c>. </returns>
    Public Function FindSupportFirmware(ByVal node As NodeEntityBase) As Boolean
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        Me._SupportfirmwareExists = Not Me.Session.IsNil(Me.SupportFirmwareNameGetter(node))
        Return Me._SupportfirmwareExists.Value
    End Function

    ''' <summary> Reads the firmware versions of the controller node. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function ReadFirmwareVersions() As Boolean
        Return Me._Scripts.ReadFirmwareVersions(Me.LinkSubsystem.ControllerNode, Me.Session)
    End Function

    ''' <summary> Reads the firmware versions of the controller node. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function ReadFirmwareVersions(ByVal node As NodeEntityBase) As Boolean
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        Return Me._Scripts.ReadFirmwareVersions(node, Me.Session)
    End Function

    ''' <summary> Checks if all scripts were saved. </summary>
    ''' <param name="node">                 Specifies the node. </param>
    ''' <param name="refreshScriptCatalog"> Specifies the condition for updating the catalog of
    ''' saved scripts before checking the status of these scripts. </param>
    ''' <returns> <c>True</c> if all scripts were saved; otherwise, <c>False</c>. </returns>
    Public Function AllScriptsSaved(ByVal refreshScriptCatalog As Boolean, ByVal node As NodeEntityBase) As Boolean
        If node IsNot Nothing Then
            Return Me._Scripts.FindSavedScripts(node, Me, refreshScriptCatalog)
        Else
            Return True
        End If
    End Function

    ''' <summary> Checks if all scripts exist. </summary>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> <c>True</c> if all scripts exist; otherwise, <c>False</c>. </returns>
    Public Function AllScriptsExist(ByVal node As NodeEntityBase) As Boolean
        If node IsNot Nothing Then
            Return Me._Scripts.FindScripts(node, Me.Session)
        Else
            Return True
        End If
    End Function

    ''' <summary> Checks if any script exists on the controller node. </summary>
    ''' <returns> <c>True</c> if any script exists; otherwise, <c>False</c>. </returns>
    Public Function AnyScriptExists() As Boolean
        Return Me.AnyScriptExists(Me.LinkSubsystem.ControllerNode)
    End Function

    ''' <summary> Checks if any script exists. </summary>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> <c>True</c> if any script exists; otherwise, <c>False</c>. </returns>
    Public Function AnyScriptExists(ByVal node As NodeEntityBase) As Boolean
        If node IsNot Nothing Then
            Return Me._Scripts.FindAnyScript(node, Me.Session)
        Else
            Return False
        End If
    End Function

    ''' <summary> Returns <c>True</c> if any legacy scripts exist on the controller node. </summary>
    ''' <returns> <c>True</c> if any legacy script exists; otherwise, <c>False</c>. </returns>
    Public Function AnyLegacyScriptExists() As Boolean
        Return AnyLegacyScriptExists(Me.LinkSubsystem.ControllerNode)
    End Function

    ''' <summary> Returns <c>True</c> if any legacy scripts exist. </summary>
    ''' <param name="node"> Specifies the node. </param>
    ''' <returns> <c>True</c> if any legacy script exists; otherwise, <c>False</c>. </returns>
    Public Function AnyLegacyScriptExists(ByVal node As NodeEntityBase) As Boolean
        If node IsNot Nothing Then
            Return Me._LegacyScripts.FindAnyScript(node, Me.Session)
        Else
            Return False
        End If
    End Function

#End Region

#Region " TSP X: DELETE SCRIPTS "

    ''' <summary> Check the script version and determines if the script needs to be deleted. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="script">                    Specifies the
    ''' <see cref="ScriptEntityBase">script</see>to delete. </param>
    ''' <param name="node">                      Specifies the system node. </param>
    ''' <param name="allowDeletingNewerScripts"> true to allow, false to deny deleting newer scripts. </param>
    ''' <returns> Returns <c>True</c> if script was deleted or did not exit. Returns <c>False</c> if a
    ''' VISA error was encountered. Assumes that the list of saved scripts is current. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Public Function IsDeleteUserScriptRequired(ByVal script As ScriptEntityBase,
                                              ByVal node As NodeEntityBase,
                                              ByVal allowDeletingNewerScripts As Boolean) As Boolean

        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        If Me.Session.IsNil(node.IsController, node.Number, script.Name) Then
            ' ignore error if nil
            If Not Me.TraceVisaDeviceOperationOkay(node.Number, "looking for script '{0}'. Ignoring error;. ", script.Name) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' had error(s) looking for script {1} on node {2};. nothing to do.",
                                  Me.ResourceNameCaption, script.Name, node.Number)
            End If
            ' return false, script does not exists.
            Return False
            ' check if can read version 
        ElseIf Me.Session.IsNil(node.IsController, node.Number, script.Namespaces) Then

            ' reading version requires an intact namespace. A missing name space may be missing, this might 
            ' indicate that referenced scripts were deleted or failed loading so this script should be deleted.
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                              "Instrument '{0}' reports that some namespaces '{1}' on node {2} are nil;. script '{3}' will be deleted.",
                              Me.ResourceNameCaption, script.Namespaces, node.Number, script.Name)
            Return True

        ElseIf Not script.FirmwareVersionQueryCommandExists(node, Me.Session) Then

            ' reading version requires a supported version function. Delete if a firmware version function is not
            ' defined.
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                              "Instrument '{0}' firmware version function not defined;. script '{1}' will be deleted.",
                              Me.ResourceNameCaption, script.Name)
            Return True

        End If

        ' read the script firmware version
        script.QueryFirmwareVersion(node, Me.Session)
        Dim validation As FirmwareVersionStatus = script.ValidateFirmware()

        Select Case validation

            Case FirmwareVersionStatus.None

                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' '{1}' script firmware version on node {2} is irrelevant;. script will be deleted.",
                                  Me.ResourceNameCaption, script.Name, node.Number)
                Return True

            Case FirmwareVersionStatus.Current

                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' script {1} on node {2} is up to date;. Nothing to do.",
                                   Me.ResourceNameCaption, script.Name, node.Number)
                Return False

            Case FirmwareVersionStatus.Missing

                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' custom firmware '{1}' version on node {2} is not known;. script version function is not defined. Script will be deleted.",
                                  Me.ResourceNameCaption, script.Name, node.Number)
                Return True

            Case FirmwareVersionStatus.Newer

                If allowDeletingNewerScripts Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                      "Instrument '{0}' existing custom firmware '{1}' on node {2} version '{3}' is newer than the specified version '{4}';. The scripts will be deleted to allow uploading the older script.",
                                      Me.ResourceNameCaption, script.Name, node.Number, script.EmbeddedFirmwareVersion, script.ReleasedFirmwareVersion)
                    Return True
                Else
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                      "Instrument '{0}' existing custom firmware '{1}' on node {2} version '{3}' is newer than the specified version '{4}';. A newer version of the program is required. Script will not be deleted.",
                                      Me.ResourceNameCaption, script.Name, node.Number, script.EmbeddedFirmwareVersion, script.ReleasedFirmwareVersion)
                    Return False
                End If

            Case FirmwareVersionStatus.Older

                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' existing custom firmware '{1}' on node {2} version '{3}' is older than the specified version '{4}';. Script will be deleted.",
                                  Me.ResourceNameCaption, script.Name, node.Number, script.EmbeddedFirmwareVersion, script.ReleasedFirmwareVersion)
                Return True

            Case FirmwareVersionStatus.ReferenceUnknown

                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                   "Instrument '{0}' custom firmware '{1}' released version not given;. Script will not be deleted.{2}{3}",
                                   Me.ResourceNameCaption, script.Name, Environment.NewLine, New StackFrame(True).UserCallStack())
                Return False

            Case FirmwareVersionStatus.Unknown

                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' firmware '{1}' on node {2} version was not read;. Script will be deleted.",
                                  Me.ResourceNameCaption, script.Name, node.Number)
                Return True

            Case Else

                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' encountered unhandled firmware version status {1} on node {2};. Nothing to do. Ignored.",
                                  Me.ResourceNameCaption, validation, node.Number)
                Return False

        End Select

        Return False

    End Function

    ''' <summary> Checks if delete is required on any user script. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts">               Specifies the list of the scripts to be deleted. </param>
    ''' <param name="node">                  Specifies the node. </param>
    ''' <param name="refreshScriptsCatalog"> Refresh catalog before checking if script exists. </param>
    ''' <returns> <c>True</c> if delete is required on any user script. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function IsDeleteUserScriptRequired(ByVal scripts As ScriptEntityCollection,
                                               ByVal node As NodeEntityBase,
                                               ByVal refreshScriptsCatalog As Boolean) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        ' <c>True</c> if any delete action was executed
        ' Dim scriptsDeleted As Boolean = False

        If scripts IsNot Nothing AndAlso scripts.Count > 0 Then

            If refreshScriptsCatalog Then
                Me.FetchSavedScripts(node)
            End If

            ' deletion of scripts must be done in reverse order.
            For i As Integer = scripts.Count - 1 To 0 Step -1

                Dim script As ScriptEntityBase = scripts.Item(i)

                If script.IsModelMatch(node.ModelNumber) Then

                    Try

                        If Not script.IsBootScript AndAlso Me.IsDeleteUserScriptRequired(script, node, scripts.AllowDeletingNewerScripts) Then

                            ' stop in design time to make sure delete is not incorrect.
                            Debug.Assert(Not Debugger.IsAttached, "ARE YOU SURE?")
                            Return Me.IsDeleteUserScriptRequired(script, node, scripts.AllowDeletingNewerScripts)

                        End If

                    Catch ex As Exception
                        Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                           "Exception occurred testing if delete is required for firmware {0} from node {1};. {2}",
                                           script.Name, node.Number, ex.ToFullBlownString)
                    End Try

                End If

            Next

        End If

        Return False

    End Function

    ''' <summary> Checks if delete is required on any node. </summary>
    ''' <param name="scripts">               Specifies the list of the scripts to be deleted. </param>
    ''' <param name="nodes">                 Specifies the list of nodes on which scripts are deleted. </param>
    ''' <param name="refreshScriptsCatalog"> Refresh catalog before checking if script exists. </param>
    ''' <returns> <c>True</c> if delete is required on any node. </returns>
    Public Function IsDeleteUserScriptRequired(ByVal scripts As ScriptEntityCollection,
                                               ByVal nodes As NodeEntityCollection,
                                               ByVal refreshScriptsCatalog As Boolean) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If nodes Is Nothing Then Throw New ArgumentNullException(NameOf(nodes))

        ' clear buffers before deleting.
        Me.Session.DiscardUnreadData()
        For Each node As NodeEntityBase In nodes

            If Me.IsDeleteUserScriptRequired(scripts, node, refreshScriptsCatalog) Then
                Return True
            End If

        Next
        Return False

    End Function

    ''' <summary> Deletes the user script. </summary>
    ''' <param name="script">                Specifies the <see cref="ScriptEntityBase">script</see>to
    ''' delete. </param>
    ''' <param name="node">                  Specifies the system node. </param>
    ''' <param name="refreshScriptsCatalog"> Refresh catalog before checking if script exists. </param>
    ''' <returns> Returns <c>True</c> if script was deleted or did not exit. Returns <c>False</c> if
    ''' deletion failed. </returns>
    Public Function DeleteUserScript(ByVal script As ScriptEntityBase,
                                     ByVal node As NodeEntityBase,
                                     ByVal refreshScriptsCatalog As Boolean) As Boolean

        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        If Me.Session.IsNil(node.IsController, node.Number, script.Name) Then
            ' ignore error if nil
            If Not Me.TraceVisaDeviceOperationOkay(node.Number, "looking for script '{0}'. Ignoring error;. ", script.Name) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' had error(s) looking for script {1} on node;. Nothing to do.",
                                  Me.ResourceNameCaption, script.Name, node.Number)
            End If
            script.IsDeleted = True
            Return True
        End If

        Me.DisplaySubsystem.DisplayLine(2, "Deleting {0}:{1}", node.Number, script.Name)
        If node.IsController Then
            Me.DeleteScript(script.Name, refreshScriptsCatalog)
            If Not Me.TraceVisaDeviceOperationOkay(False, "deleting {0};. ", script.Name) Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' had error(s) deleting script {1} on node {2};. ", Me.ResourceNameCaption, script.Name, node.Number)
                Return False
            End If

            ' do a garbage collection
            Me.StatusSubsystem.CollectGarbageWaitComplete(Me.DeleteTimeout, "collecting garbage--ignoring error;. ")

        Else

            If Me.DeleteScript(node, script.Name, refreshScriptsCatalog) Then

                If Not Me.TraceVisaDeviceOperationOkay(node.Number, "deleting script {0};. ", script.Name) Then
                    Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                      "Instrument '{0}' had error(s) deleting {1} on node {2};. ", Me.ResourceNameCaption, script.Name, node.Number)
                    Return False
                End If

            Else
                If Not Me.TraceVisaDeviceOperationOkay(node.Number, "deleting script {0};. ", script.Name, node.Number) Then
                    ' report failure if not an instrument or VISA error (handler returns Okay.)
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                      "Instrument '{0}' had error(s) deleting {1} on node {2};. {3}{4}",
                                      Me.ResourceNameCaption, script.Name, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
                End If
                Return False
            End If

            ' do a garbage collection
            Me.LinkSubsystem.CollectGarbageWaitComplete(node, Me.DeleteTimeout,
                                                        "collecting garbage on node {0}--ignoring error;. ", node.Number)
        End If

        script.IsDeleted = True
        Return True

    End Function

    ''' <summary> Deletes user scripts that are out-dated or where a deletion is set for the script. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts">               Specifies the list of the scripts to be deleted. </param>
    ''' <param name="node">                  Specifies the node. </param>
    ''' <param name="refreshScriptsCatalog"> Refresh catalog before checking if script exists. </param>
    ''' <param name="deleteOutdatedOnly">    if set to <c>True</c> deletes only if scripts is out of
    ''' date. </param>
    ''' <returns> <c>True</c> if success, <c>False</c> otherwise. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function DeleteUserScripts(ByVal scripts As ScriptEntityCollection,
                                      ByVal node As NodeEntityBase,
                                      ByVal refreshScriptsCatalog As Boolean,
                                      ByVal deleteOutdatedOnly As Boolean) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        Dim success As Boolean = True

        ' <c>True</c> if any delete action was executed
        Dim scriptsDeleted As Boolean = False

        If scripts IsNot Nothing AndAlso scripts.Count > 0 Then

            If refreshScriptsCatalog Then
                Me.FetchSavedScripts(node)
            End If

            ' deletion of scripts must be done in reverse order.
            For i As Integer = scripts.Count - 1 To 0 Step -1

                Dim script As ScriptEntityBase = scripts.Item(i)

                If script.IsModelMatch(node.ModelNumber) Then

                    Try

                        If Not script.IsDeleted AndAlso (script.RequiresDeletion OrElse
                                    (Not deleteOutdatedOnly OrElse
                                     Me.IsDeleteUserScriptRequired(script, node, scripts.AllowDeletingNewerScripts))) Then

                            If Me.DeleteUserScript(script, node, False) Then

                                ' mark that scripts were deleted, i.e., that any script was deleted 
                                ' or if a script that existed no longer exists.
                                scriptsDeleted = True
                            Else
                                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "failed deleting script '{1}' from node {2};. ",
                                                  Me.ResourceNameCaption, script.Name, node.Number)
                                success = False
                            End If

                        End If

                    Catch ex As Exception

                        Try
                            success = success AndAlso Me.Session.IsNil(script.Name)
                            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                              "Exception occurred deleting firmware {0} from node {1};. {2}", script.Name, node.Number, ex.ToFullBlownString)
                        Catch
                            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                              "Exception occurred checking existence after attempting deletion of firmware {0} from node {1};. {2}",
                                              script.Name, node.Number, ex.ToFullBlownString)
                            success = False
                        End Try

                    End Try

                End If

            Next

        End If

        If scriptsDeleted Then
            ' reset to refresh the instrument display.
            Me.LinkSubsystem.ResetNode(node)
        End If

        Return success

    End Function

    ''' <summary> Deletes user scripts from the remote instrument. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts">               Specifies the list of the scripts to be deleted. </param>
    ''' <param name="nodes">                 Specifies the list of nodes on which scripts are deleted. </param>
    ''' <param name="refreshScriptsCatalog"> Refresh catalog before checking if script exists. </param>
    ''' <param name="deleteOutdatedOnly">    if set to <c>True</c> deletes only if scripts is out of
    ''' date. </param>
    ''' <returns> <c>True</c> if success, <c>False</c> otherwise. </returns>
    Public Function DeleteUserScripts(ByVal scripts As ScriptEntityCollection,
                                      ByVal nodes As NodeEntityCollection,
                                      ByVal refreshScriptsCatalog As Boolean,
                                      ByVal deleteOutdatedOnly As Boolean) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If nodes Is Nothing Then Throw New ArgumentNullException(NameOf(nodes))

        ' clear buffers before deleting.
        Me.Session.DiscardUnreadData()

        Dim success As Boolean = True
        For Each node As NodeEntityBase In nodes

            success = success And Me.DeleteUserScripts(scripts, node, refreshScriptsCatalog, deleteOutdatedOnly)

        Next
        Return success

    End Function

#End Region

#Region " LOAD AND RUN SCRIPTS; Extended methods with error management"

    ''' <summary> Loads and executes the specified TSP script from file. </summary>
    ''' <exception cref="ArgumentNullException">  Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when a Script Loading error condition occurs. </exception>
    ''' <param name="script"> Specifies reference to a valid <see cref="ScriptEntity">script</see> </param>
    ''' <param name="node">   Specifies the node. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub LoadRunUserScript(ByVal script As ScriptEntityBase, ByVal node As NodeEntityBase)

        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        If Me.Session.IsNil(node.IsController, node.Number, script.Name) Then

            If String.IsNullOrWhiteSpace(script.Source) AndAlso script.Source.Length > 10 Then
                Me.DisplaySubsystem.DisplayLine(2, "Attempted loading empty script {0}:{1}", node.Number, script.Name)
                Throw New VI.Pith.OperationFailedException("Attempted loading empty script;. {0}:{1}", node.Number, script.Name)
            End If

            If node.IsController Then
                If Me.LoadUserScript(script) Then
                    If Not Me.RunUserScript(script) Then
                        Throw New VI.Pith.OperationFailedException("Failed running script;. {0}:{1}", node.Number, script.Name)
                    End If
                Else
                    Throw New VI.Pith.OperationFailedException("Failed loading script;. {0}:{1}", node.Number, script.Name)
                End If
            Else
                Me.LoadUserScript(node, script)
                If Not Me.RunUserScript(node, script) Then
                    Throw New VI.Pith.OperationFailedException("Failed running script;. {0}:{1}", node.Number, script.Name)
                End If
            End If

        End If

    End Sub

    ''' <summary> Loads and runs the user scripts on the controller instrument. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts"> Specifies the collection of scripts. </param>
    ''' <param name="node">    Specifies the node. </param>
    ''' <returns> The run user scripts. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function LoadRunUserScripts(ByVal scripts As ScriptEntityCollection,
                                       ByVal node As NodeEntityBase) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        ' reset all status values so as to force a read.
        Me.ResetKnownState()

        ' <c>True</c> if any load and run action was executed
        Dim resetRequired As Boolean = False

        If scripts IsNot Nothing AndAlso scripts.Count > 0 Then

            For Each script As ScriptEntityBase In scripts

                If script.IsModelMatch(node.ModelNumber) Then

                    ' reset if a new script will be loaded.
                    resetRequired = resetRequired OrElse Me.Session.IsNil(node.IsController, node.Number, script.Name)
                    Me.LoadRunUserScript(script, node)

                End If

            Next

        End If

        If resetRequired Then
            ' reset to refresh the instrument display.
            Me.LinkSubsystem.ResetNode(node)
        End If

        Return True

    End Function

    ''' <summary> Loads and runs the user scripts on the controller instrument for the specified node. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts"> Specifies the collection of scripts. </param>
    ''' <param name="nodes">   Specifies the nodes. </param>
    ''' <returns> The run user scripts. </returns>
    Public Function LoadRunUserScripts(ByVal scripts As ScriptEntityCollection,
                                       ByVal nodes As NodeEntityCollection) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If nodes Is Nothing Then Throw New ArgumentNullException(NameOf(nodes))

        ' clear buffers before deleting.
        Me.Session.DiscardUnreadData()

        For Each node As NodeEntityBase In nodes
            If Not LoadRunUserScripts(scripts, node) Then
                Return False
            End If
        Next
        Return True

    End Function

    ''' <summary> Copies a script source. </summary>
    ''' <remarks> For binary scripts, the controller and remote nodes must be binary compatible. </remarks>
    ''' <param name="sourceName">      The name of the source script. </param>
    ''' <param name="destinationName"> The name of the destination script. </param>
    Public Sub CopyScript(ByVal sourceName As String, ByVal destinationName As String)
        Me.Session.WriteLine("{1}=script.new( {0}.source , '{1}' ) waitcomplete()", sourceName, destinationName)
    End Sub

    ''' <summary> Copies a script from the controller node to a remote node. </summary>
    ''' <remarks> For binary scripts, the controller and remote nodes must be binary compatible. </remarks>
    ''' <param name="nodeNumber">      . </param>
    ''' <param name="sourceName">      The script name on the controller node. </param>
    ''' <param name="destinationName"> The script name on the remote node. </param>
    Public Sub CopyScript(ByVal nodeNumber As Integer, ByVal sourceName As String, ByVal destinationName As String)

        ' loads and runs the specified script.
        Dim commands As String = String.Format(Globalization.CultureInfo.CurrentCulture,
          "node[{0}].execute('waitcomplete() {2}=script.new({1}.source,[[{2}]])') waitcomplete({0}) waitcomplete()",
          nodeNumber, sourceName, destinationName)
        Me.LoadString(commands)

    End Sub

    ''' <summary> Copies script source from one script to another. </summary>
    ''' <remarks> For binary scripts, the controller and remote nodes must be binary compatible. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node">            Specifies a node on which to copy. </param>
    ''' <param name="sourceName">      The script name on the controller node. </param>
    ''' <param name="destinationName"> The script name on the remote node. </param>
    Public Sub CopyScript(ByVal node As NodeEntityBase, ByVal sourceName As String, ByVal destinationName As String)

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If node.IsController Then
            Me.CopyScript(sourceName, destinationName)
        Else
            Me.CopyScript(node.Number, sourceName, destinationName)
        End If

    End Sub

    ''' <summary> Loads and runs an anonymous script. </summary>
    ''' <param name="commands"> Specifies the script commands. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function LoadRunAnonymousScript(ByVal commands As String) As Boolean

        Dim prefix As String = "loadandrunscript"
        Dim suffix As String = "endscript waitcomplete()"
        Dim loadCommand As String = String.Format(Globalization.CultureInfo.CurrentCulture, "{1}{0}{2}{0}{3}",
                                                  Environment.NewLine, prefix, commands, suffix)
        Me.LoadString(loadCommand)
        Return True

    End Function

    ''' <summary> Builds a script for loading a script to the remote node. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="nodeNumber">        Specifies the remote node number. </param>
    ''' <param name="script">            Specifies the script. </param>
    ''' <param name="loadingScriptName"> Specifies the name of the loading script that will be deleted
    ''' after the process is done. </param>
    ''' <returns> The script with the load and end commands. </returns>
    Private Shared Function BuildScriptLoaderScript(ByVal nodeNumber As Integer,
                                                    ByVal script As ScriptEntityBase,
                                                    ByVal loadingScriptName As String) As System.Text.StringBuilder

        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))

        Dim prefix As String = "loadstring(table.concat("
        Dim suffix As String = "))()"
        Dim binaryDecorationRequire As Boolean = script.IsBinaryScript AndAlso Not script.Source.Contains(prefix)

        Dim loadCommands As New System.Text.StringBuilder(script.Source.Length + 512)
        loadCommands.AppendFormat("loadandrunscript {0}", loadingScriptName)
        loadCommands.AppendLine()
        loadCommands.AppendLine("do")
        loadCommands.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                  "node[{0}].dataqueue.add([[", nodeNumber)
        If binaryDecorationRequire Then
            loadCommands.Append(prefix)
        End If
        loadCommands.AppendLine(script.Source)
        If binaryDecorationRequire Then
            loadCommands.AppendLine(suffix)
        End If
        loadCommands.AppendFormat(Globalization.CultureInfo.CurrentCulture, "]]) waitcomplete()", nodeNumber)
        loadCommands.AppendLine()
        loadCommands.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                  "node[{0}].execute([[waitcomplete() {1}=script.new(dataqueue.next(),'{1}')]])",
                                  nodeNumber, script.Name)
        loadCommands.AppendLine()
        loadCommands.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                  "waitcomplete({0})", nodeNumber)
        loadCommands.AppendLine(" waitcomplete()")
        loadCommands.AppendLine("end")
        loadCommands.AppendLine("endscript")

        Return loadCommands

    End Function

    ''' <summary> Create a new script on the remote node. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="node">   The node. </param>
    ''' <param name="script"> . </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function UploadScript(ByVal node As NodeEntityBase, ByVal script As ScriptEntityBase) As Boolean

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))

        Me.Session.LastNodeNumber = node.Number
        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "loading script '{0}';. ", script.Name)
        Dim affirmative As Boolean = True
        If script.IsBinaryScript OrElse (Not Me.Session.IsNil(script.Name) AndAlso
                                         IsBinaryScript(script.Name, Me.LinkSubsystem.ControllerNode)) Then

            Dim tempName As String = "isr_temp"
            Try
                ' clear the data queue.
                Me.LinkSubsystem.ClearDataQueue(node.Number)
                Dim scriptLoaderScript As String = buildScriptLoaderScript(node.Number, script, tempName).ToString

                ' load and ran the temporary script. 
                Me.LoadString(scriptLoaderScript)

                ' enable wait completion (the wait complete command is included in the loader script
                Me.StatusSubsystem.EnableWaitComplete()
                Me.StatusSubsystem.AwaitOperationCompleted(script.Timeout)
                affirmative = Me.TraceVisaDeviceOperationOkay(node.Number, "loading script '{0}';. ", tempName)
            Catch ex As VI.Pith.NativeException
                Me.TraceVisaOperation(ex, node.Number, "loading script '{0}';. ", tempName)
                affirmative = False
            Catch ex As Exception
                Me.TraceOperation(ex, node.Number, "loading script '{0}';. ", tempName)
                affirmative = False
            End Try

            If affirmative Then
                Try
                    ' remove the temporary script if there.
                    If Not Me.Session.IsNil(tempName) Then
                        Me.DeleteScript(tempName, False)
                    End If
                    Me.TraceVisaDeviceOperationOkay(node.Number, "deleting script '{0}';. ", tempName)
                Catch ex As VI.Pith.NativeException
                    Me.TraceVisaOperation(ex, node.Number, "deleting script '{0}';. ", tempName)
                Catch ex As Exception
                    Me.TraceOperation(ex, node.Number, "deleting script '{0}';. ", tempName)
                End Try
            End If
        Else

            ' if scripts is already stored in the controller node in non-binary format, just
            ' copy it (upload) from the controller to the remote node.
            If Not Me.UploadScript(node, script.Name, script.Timeout) Then
                affirmative = False
            End If

        End If

        If Not affirmative Then Return affirmative

        ' verify that the script was loaded.
        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                   "Verifying script '{0}' loaded on node {1};. ", Name, node.Number)
        Me.Session.LastNodeNumber = node.Number

        ' check if the script short name exists.
        If Me.Session.WaitNotNil(node.Number, script.Name, script.Timeout) Then

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Instrument '{0}' loaded script '{1}' to node {2};. ",
                              Me.ResourceNameCaption, script.Name, node.Number)

            affirmative = True


        Else

            ' if script short name not found, check to see if the script long name is found.
            Dim fullName As String = "script.user.scripts." & script.Name
            If Me.Session.WaitNotNil(node.Number, fullName, script.Timeout) Then

                ' in design mode, assert this as a problem.
                ' 3783. this was asserted the first time after upgrading the 3706 to firmware 1.32.a
                Debug.Assert(Not Debugger.IsAttached, "Failed setting script short name")

                ' assign the new script name
                Dim message As String = Me.Session.ExecuteCommand(node.Number, "{0} = {1} waitcomplete() ", script.Name, fullName)
                Me.CheckThrowDeviceException(False, message)

                Me.LinkSubsystem.WaitComplete(node.Number, Me.SaveTimeout, False)
                Me.CheckThrowDeviceException(False, "uploading script '{0}' using '{1}';. ", Name, message)

                If Me.Session.WaitNotNil(node.Number, script.Name, script.Timeout) Then

                    affirmative = True

                Else

                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                      "failed referencing script '{1}' using '{2}' on node {3};. --new script not found on the remote node.{4}{5}",
                                      Me.ResourceNameCaption, script.Name, fullName, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
                    affirmative = False

                End If
            Else

                ' if both long and short names not found, report failure.
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                  "failed uploading script '{1}' to node {2} from script '{3}';. --new script not found on the remote node.{4}{5}",
                                  Me.ResourceNameCaption, fullName, node.Number, script.Name, Environment.NewLine, New StackFrame(True).UserCallStack())
                affirmative = False

            End If

        End If
        Me.Session.LastNodeNumber = New Integer?
        Return affirmative

    End Function

    ''' <summary> Uploads a script from the controller node to a remote node using the same name on the
    ''' remote node. Does not require having the ISR Support script on the controller node. </summary>
    ''' <remarks> For binary scripts, the controller and remote nodes must be binary compatible. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node">       The node. </param>
    ''' <param name="scriptName"> The script name on the controller node. </param>
    ''' <param name="timeout">    Specifies the time to wait for the instrument to return operation
    ''' completed. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function UploadScript(ByVal node As NodeEntityBase, ByVal scriptName As String,
                                 ByVal timeout As TimeSpan) As Boolean

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If String.IsNullOrWhiteSpace(scriptName) Then Throw New ArgumentNullException(NameOf(scriptName))

        ' NOTE: WAIT COMPLETE is required on the system before a wait complete is tested on the node
        ' otherwise getting error 1251.

        Dim commands As New System.Text.StringBuilder(1024)
        If Me.Session.IsNil(Scripts.SelectSupportScript(Me.LinkSubsystem.ControllerNode).Namespaces) Then

            ' loads and runs the specified script.
            commands.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                  "node[{0}].dataqueue.add({1}.source) waitcomplete()", node.Number, scriptName)
            commands.AppendLine()
            commands.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                  "node[{0}].execute('waitcomplete() {1}=script.new(dataqueue.next(),[[{1}]])')",
                                  node.Number, scriptName)
        Else
            commands.AppendFormat(Globalization.CultureInfo.CurrentCulture,
                                  "isr.script.uploadScript(node[{0}],{1})", node.Number, scriptName)
        End If

        commands.AppendLine()
        commands.AppendLine("waitcomplete(0)")
        Dim affirmative As Boolean = False
        Try
            Me.LoadString(commands.ToString)
            Me.LinkSubsystem.EnableWaitComplete(0)
            Me.StatusSubsystem.AwaitOperationCompleted(timeout)
            affirmative = Me.TraceVisaDeviceOperationOkay(node.Number, "timeout uploading script '{0}';. ", scriptName)
        Catch ex As VI.Pith.NativeException
            Me.TraceVisaOperation(ex, node.Number, "timeout uploading script '{0}';. ", scriptName)
        Catch ex As Exception
            Me.TraceOperation(ex, node.Number, "timeout uploading script '{0}';. ", scriptName)
        End Try
        Return affirmative

    End Function

    ''' <summary> Loads the specified script from the local to the remote node. </summary>
    ''' <remarks> Will not load a script list that includes the create script command. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node">           Specifies the node. </param>
    ''' <param name="script">         Specifies reference to a valid
    ''' <see cref="ScriptEntity">script</see> </param>
    Public Sub LoadUserScript(ByVal node As NodeEntityBase, ByVal script As ScriptEntityBase)

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))

        Me.Session.LastNodeNumber = node.Number

        If Not Me.Session.IsNil(node.Number, script.Name) Then
            Me.DisplaySubsystem.DisplayLine(2, "{0}:{1} exists--nothing to do", node.Number, script.Name)
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                              "Instrument '{0}' script {1} already exists on node {2};. Nothing to do.", Me.ResourceNameCaption, script.Name, node.Number)
            Return
        End If

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Uploading {0}:{1}", node.Number, script.Name)
        Me.DisplaySubsystem.DisplayLine(2, "Uploading {0}:{1}", node.Number, script.Name)
        If Not Me.UploadScript(node, script) Then
            Me.DisplaySubsystem.DisplayLine(2, "Failed uploading {0}:{1}", node.Number, script.Name)
            Return
        End If

        Me.DisplaySubsystem.DisplayLine(2, "Verifying {0}:{1}", node.Number, script.Name)
        If Not Me.Session.WaitNotNil(node.Number, script.Name, Me._saveTimeout) Then

            Me.DisplaySubsystem.DisplayLine(2, "{0}:{1} not found after loading", node.Number, script.Name)
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              "failed loading script '{1}' to node {2};. --new script not found on the remote node.{3}{4}",
                              Me.ResourceNameCaption, script.Name, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
            Return

        End If

        ' do a garbage collection
        Me.DisplaySubsystem.DisplayLine(2, "Cleaning local node")

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                   "Collecting garbage after uploading {0}:{1}", node.Number, script.Name)

        ' do a garbage collection
        Me.StatusSubsystem.CollectGarbageWaitComplete(script.Timeout, "Collecting garbage after uploading {0}:{1}", node.Number, script.Name)

        ' do a garbage collection on remote node
        Me.DisplaySubsystem.DisplayLine(2, "Cleaning node {0}", node.Number)
        Me.LinkSubsystem.CollectGarbageWaitComplete(node, script.Timeout, "Collecting garbage after uploading {0}:{1}", node.Number, script.Name)
        Me.DisplaySubsystem.DisplayLine(2, "{0}:{1} Loaded", node.Number, script.Name)

    End Sub

    ''' <summary> Loads the specified TSP script from code. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="script"> Specifies reference to a valid <see cref="ScriptEntity">script</see> </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function LoadUserScript(ByVal script As ScriptEntityBase) As Boolean

        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))

        If Not Me.Session.IsNil(script.Name) Then

            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Instrument '{0}' script {1} already exists;. ", Me.ResourceNameCaption, script.Name)
            Return True

        End If

        Dim affirmative As Boolean = True
        Try
            Me.DisplaySubsystem.DisplayLine(2, "Loading {0}", script.Name)
            affirmative = Me.TraceVisaDeviceOperationOkay(False, "Ready to load {0};. ", script.Name)
            If Not affirmative Then
                ' report failure if not an instrument or VISA error (handler returns Okay.)
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' had error(s) before loading {1};. {2}Problem ignored @{3}",
                                  Me.ResourceNameCaption, script.Name, Environment.NewLine, New StackFrame(True).UserCallStack())
            End If
            ' ignore device errors.
            affirmative = True
        Catch ex As VI.Pith.NativeException
            Me.TraceVisaOperation(ex, "Ready to load {0};. ", script.Name)
            affirmative = False
        Catch ex As Exception
            Me.TraceOperation(ex, "Ready to load {0};. ", script.Name)
            affirmative = False
        End Try

        Try
            If affirmative Then
                Me.LoadScript(script.Name, script.Source)
                affirmative = Me.TraceVisaDeviceOperationOkay(False, "loading {0};. ", script.Name)
                If Not affirmative Then
                    ' report failure if not an instrument or VISA error (handler returns Okay.)
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                      "Instrument '{0}' had error(s) loading {1};. {2}{3}",
                                      Me.ResourceNameCaption, script.Name, Environment.NewLine, New StackFrame(True).UserCallStack())
                End If
            End If
        Catch ex As VI.Pith.NativeException
            Me.TraceVisaOperation(ex, "loading {0};. ", script.Name)
            affirmative = False
        Catch ex As Exception
            Me.TraceOperation(ex, "loading {0};. ", script.Name)
            affirmative = False

        End Try

        If affirmative Then
            ' do a garbage collection
            If Not Me.StatusSubsystem.CollectGarbageWaitComplete(script.Timeout, "collecting garbage;. ") Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                  "Ignoring instrument '{0}' error(s) collecting garbage after loading {1};. {2}{3}",
                                  Me.ResourceNameCaption, script.Name, Environment.NewLine, New StackFrame(True).UserCallStack())
            End If

            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Instrument '{0}' {1} script loaded;. ", Me.ResourceNameCaption, script.Name)

            Me.DisplaySubsystem.DisplayLine(2, "{0} Loaded", script.Name)
        End If

        Return affirmative

    End Function

    ''' <summary> Executes the specified TSP script from file. </summary>
    ''' <param name="script"> Specifies reference to a valid <see cref="ScriptEntity">script</see> </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function RunUserScript(ByVal script As ScriptEntityBase) As Boolean
        Return Me.RunUserScript(script, True)
    End Function

    ''' <summary> Executes the specified TSP script from file. </summary>
    ''' <remarks>
    ''' David, 05/13/2009. Modified to run irrespective of the existence of the name spaces because a
    ''' new script can be loaded on top of existing old code and the new version number will not
    ''' materialize.
    ''' </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="script">    Specifies reference to a valid <see cref="ScriptEntity">script</see> </param>
    ''' <param name="runAlways"> true to run always. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function RunUserScript(ByVal script As ScriptEntityBase, ByVal runAlways As Boolean) As Boolean

        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))

        If Me.Session.IsNil(script.Name) Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              "failed running {1} because it does not exist;. {2}{3}",
                              Me.ResourceNameCaption, script.Name, Environment.NewLine, New StackFrame(True).UserCallStack())
            Return False
        End If

        ' taken out to run always.
        If Not runAlways AndAlso script.Namespaces IsNot Nothing AndAlso
            script.Namespaces.Length > 0 AndAlso Not Me.Session.IsNil(script.Namespaces) Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                              "Instrument '{0}' script {1} already run. Nothing to do;. ", Me.ResourceNameCaption, script.Name)
            Return True
        End If

        Dim affirmative As Boolean = True
        Try
            Me.DisplaySubsystem.DisplayLine(2, "Running {0}", script.Name)
            Me.RunScript(script.Name, script.Timeout)
            Me.DisplaySubsystem.DisplayLine(2, "Done running {0}", script.Name)
            affirmative = Me.TraceVisaDeviceOperationOkay(False, "running {0};. ", script.Name)
            If Not affirmative Then
                ' report failure if not an instrument or VISA error (handler returns Okay.)
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' had error(s) running {1};. {2}{3}",
                                  Me.ResourceNameCaption, script.Name, Environment.NewLine, New StackFrame(True).UserCallStack())
            End If
        Catch ex As VI.Pith.NativeException
            Me.TraceVisaOperation(ex, "running {0};. ", script.Name)
            affirmative = False
        Catch ex As Exception
            Me.TraceOperation(ex, "running {0};. ", script.Name)
            affirmative = False
        End Try

        If Not affirmative Then Return affirmative

        If Me.Session.IsNil(script.Name) Then

            If Not Me.TraceVisaDeviceOperationOkay(False, "script {0} not found after running;. ", script.Name) Then
                ' report failure if not an instrument or VISA error (handler returns Okay.)
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' had error(s) running script {1};. {2}{3}",
                                  Me.ResourceNameCaption, script.Name, Environment.NewLine, New StackFrame(True).UserCallStack())
            End If
            affirmative = False

        ElseIf script.Namespaces IsNot Nothing AndAlso script.Namespaces.Length > 0 AndAlso Me.Session.IsNil(script.Namespaces) Then

            If Me.TraceVisaDeviceOperationOkay(False, "some of the namespace(s) {0} are nil after running {1};. ", script.NamespaceList, script.Name) Then
                ' if not a visa error, report the specific namespaces.
                For Each value As String In script.Namespaces
                    If Me.Session.IsNil(value) Then
                        Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                          "Instrument '{0}' namespace {1} is nil;. {2}{3}",
                                          Me.ResourceNameCaption, value, Environment.NewLine, New StackFrame(True).UserCallStack())
                    End If
                Next
            End If
            affirmative = False

        Else

            Me.DisplaySubsystem.DisplayLine(2, "Done running {0}", script.Name)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "Instrument '{0}' {1} script run okay;. ", Me.ResourceNameCaption, script.Name)
            affirmative = True

        End If
        Return affirmative

    End Function

    ''' <summary> Executes the specified TSP script from file. </summary>
    ''' <param name="node">   The node. </param>
    ''' <param name="script"> Specifies reference to a valid <see cref="ScriptEntity">script</see> </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function RunUserScript(ByVal node As NodeEntityBase, ByVal script As ScriptEntityBase) As Boolean
        Return Me.RunUserScript(node, script, True)
    End Function

    ''' <summary> Executes a loaded script on the local node. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="node">   The node. </param>
    ''' <param name="script"> Specifies reference to a valid <see cref="ScriptEntity">script</see> </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    ''' <remarks>
    ''' David, 05/13/2009. Modified to run irrespective of the existence of the name spaces because a
    ''' new script can be loaded on top of existing old code and the new version number will not
    ''' materialize.
    ''' </remarks>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Public Function RunUserScript(ByVal node As NodeEntityBase, ByVal script As ScriptEntityBase, ByVal runAlways As Boolean) As Boolean

        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))

        If Me.Session.IsNil(node.Number, script.Name) Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              "failed running {1} because it does not exist on node {2};. {3}{4}",
                              Me.ResourceNameCaption, script.Name, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
            Return False
        End If

        If Not runAlways AndAlso script.Namespaces IsNot Nothing AndAlso
            script.Namespaces.Length > 0 AndAlso Not Me.Session.IsNil(node.Number, script.Namespaces) Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                              "Instrument '{0}' script {1} already run on node {2};. Nothing to do.",
                              Me.ResourceNameCaption, script.Name, node.Number)
            Return True
        End If

        Me.DisplaySubsystem.DisplayLine(2, "Running {0}:{1}", node.Number, script.Name)
        Dim message As String = Me.Session.ExecuteCommand(node.Number, "script.user.scripts.{0}()", script.Name)

        If Me.TraceVisaDeviceOperationOkay(node.Number, False, message) Then
            Me.LinkSubsystem.WaitComplete(node.Number, script.Timeout, False)
            If Not Me.TraceVisaDeviceOperationOkay(node.Number, False, "running user script '{0}' using '{1}';. ", Name, message) Then
                Me.DisplaySubsystem.DisplayLine(2, "Failed waiting {0}:{1}", node.Number, script.Name)
            End If
        Else
            Me.DisplaySubsystem.DisplayLine(2, "Failed running {0}:{1}", node.Number, script.Name)
        End If

        ' do a garbage collection
        Me.DisplaySubsystem.DisplayLine(2, "Waiting cleanup node {0}", node.Number)
        If Not Me.LinkSubsystem.CollectGarbageWaitComplete(node, script.Timeout,
                                                           "collecting garbage after running script {0};. ",
                                                           script.Name) Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              "Ignoring instrument '{0}' error(s) collecting garbage after loading script {1} to node {2};. {3}{4}",
                              Me.ResourceNameCaption, script.Name, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
        End If

        If Me.Session.IsNil(node.Number, script.Name) Then

            If Not Me.TraceVisaDeviceOperationOkay(node.Number, "script {0} not found after running;. ", script.Name) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' had error(s) loading script {1} to node {2};. {3}{4}",
                                  Me.ResourceNameCaption, script.Name, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
            End If
            Return False

        ElseIf script.Namespaces IsNot Nothing AndAlso script.Namespaces.Length > 0 AndAlso Me.Session.IsNil(node.Number, script.Namespaces) Then

            If Me.TraceVisaDeviceOperationOkay(node.Number, "some of the namespace(s) {0} are nil after running {1};. ",
                                                script.NamespaceList, script.Name) Then
                ' if not a visa error, report the specific namespaces.
                For Each value As String In script.Namespaces
                    If Me.Session.IsNil(value) Then
                        Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                          "Instrument '{0}' namespace {1} is nil on node {2};. {3}{4}",
                                          Me.ResourceNameCaption, value, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
                    End If
                Next
            End If
            Return False

        Else

            Me.DisplaySubsystem.DisplayLine(2, "Done running {0}:{1}", node.Number, script.Name)
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                              "Instrument '{0}' {1} script run on node {2};. ", Me.ResourceNameCaption, script.Name, node.Number)
            Return True

        End If
        Return True

    End Function

#End Region

#Region " TSP X: AUTO RUN SCRIPTS "

    ''' <summary> Builds the commands for the auto run script. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="commands"> Specifies the list of commands to add to the script. </param>
    ''' <param name="scripts">  Specifies the list of scripts which to include in the run. </param>
    ''' <returns> The auto run commands. </returns>
    Public Shared Function BuildAutoRunScript(ByVal commands As System.Text.StringBuilder,
                                       ByVal scripts As ScriptEntityCollection) As String

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        Dim script As New System.Text.StringBuilder(1024)
        Dim uniqueScripts As New Collections.Specialized.ListDictionary()
        ' add code to run all scripts other than the boot script
        If scripts IsNot Nothing Then
            For Each scriptEntity As ScriptEntityBase In scripts
                If Not String.IsNullOrWhiteSpace(scriptEntity.Name) Then
                    If Not scriptEntity.IsBootScript Then
                        If Not uniqueScripts.Contains(scriptEntity.Name) Then
                            uniqueScripts.Add(scriptEntity.Name, scriptEntity)
                            script.AppendFormat("{0}.run()", scriptEntity.Name)
                            script.AppendLine()
                        End If
                    End If
                End If
            Next
        End If

        ' add the custom commands.
        If commands IsNot Nothing AndAlso commands.Length > 1 Then
            script.AppendLine(commands.ToString)
        End If
        Return script.ToString

    End Function

#End Region

#Region " TSP X: SAVE SCRIPTS "

    ''' <summary> checks if save is required for the specified script. Presumes list of saved scripts
    ''' was retrieved. </summary>
    ''' <param name="scriptName">     Specifies the script to save. </param>
    ''' <param name="node">           Specifies the node. </param>
    ''' <param name="isSaveAsBinary"> Specifies the condition requesting saving the source as binary. </param>
    ''' <param name="isBootScript">   Specifies the condition indicating if this is a boot script. </param>
    ''' <returns> <c>True</c> if save required; otherwise, <c>False</c>. </returns>
    Public Function IsSaveRequired(ByVal scriptName As String, ByVal node As NodeEntityBase,
                                   ByVal isSaveAsBinary As Boolean, ByVal isBootScript As Boolean) As Boolean

        If String.IsNullOrWhiteSpace(scriptName) Then Throw New ArgumentNullException(NameOf(scriptName))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
        Return (isSaveAsBinary AndAlso Not Me.IsBinaryScript(scriptName, node).GetValueOrDefault(False)) OrElse
               Not Me.SavedScriptExists(scriptName, node, False) OrElse
               (isBootScript AndAlso node.BootScriptSaveRequired)
    End Function

    ''' <summary> Saves the user script in non-volatile memory. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scriptName">     Specifies the script to save. </param>
    ''' <param name="node">           Specifies the node. </param>
    ''' <param name="isSaveAsBinary"> Specifies the condition requesting saving the source as binary. </param>
    ''' <param name="isBootScript">   Specifies the condition indicating if this is a boot script. </param>
    ''' <param name="timeout">        The timeout. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function SaveUserScript(ByVal scriptName As String,
                                   ByVal node As NodeEntityBase,
                                   ByVal isSaveAsBinary As Boolean, ByVal isBootScript As Boolean,
                                   ByVal timeout As TimeSpan) As Boolean

        If String.IsNullOrWhiteSpace(scriptName) Then Throw New ArgumentNullException(NameOf(scriptName))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        If Me.Session.IsNil(node.IsController, node.Number, scriptName) Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                              "Instrument '{0}' custom firmware {1} not saved on node {2};. --it is not loaded. Error may be ignored.{3}{4}",
                              Me.ResourceNameCaption, scriptName, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
            Return False
        End If

        If IsSaveRequired(scriptName, node, isSaveAsBinary, isBootScript) Then

            If Not isBootScript Then
                ' if a script is saved, boot save is required.
                node.BootScriptSaveRequired = True
            End If

            ' do a garbage collection
            Me.DisplaySubsystem.DisplayLine(2, "Cleaning node {0}", node.Number)
            If Not Me.LinkSubsystem.CollectGarbageWaitComplete(node, timeout,
                                                               "collecting garbage before saving script {0} on node {1};. ",
                                                               scriptName, node.Number) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                  "Ignoring instrument '{0}' error(s) collecting garbage after loading {1} on node {2};. {3}{4}",
                                  Me.ResourceNameCaption, scriptName, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
            End If



            Me.DisplaySubsystem.DisplayLine(2, "Saving {0}:{1}", node.Number, scriptName)
            If Me.SaveScript(scriptName, node, isSaveAsBinary, isBootScript) Then

                ' if saved boot script, boot script save no longer required.
                If isBootScript Then
                    node.BootScriptSaveRequired = False
                End If

                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                  "Instrument '{0}' saved script {1} on node {2};. ", Me.ResourceNameCaption, scriptName, node.Number)

            Else

                If Not Me.TraceVisaDeviceOperationOkay(node.Number, "saving script {0};. ", scriptName) Then
                    ' report failure if not an instrument or VISA error (handler returns Okay.)
                    Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                      "Instrument '{0}' had error(s) saving script {1} on node {2};. {3}{4}",
                                      Me.ResourceNameCaption, scriptName, node.Number, Environment.NewLine, New StackFrame(True).UserCallStack())
                End If
                Return False

            End If

        Else
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Instrument '{0}' script {1} already saved on node {2};. ", Me.ResourceNameCaption, scriptName, node.Number)
        End If

        Return True

    End Function

    ''' <summary> Saves the user scripts. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts"> Specifies the list of scripts to save. </param>
    ''' <param name="node">    Specifies the node. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Overloads Function SaveUserScripts(ByVal scripts As ScriptEntityCollection,
                                              ByVal node As NodeEntityBase) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        Dim success As Boolean = True

        ' <c>True</c> if any save action was executed
        Dim resetRequired As Boolean = False

        Me.FetchSavedScripts(node)

        If scripts IsNot Nothing Then
            For Each script As ScriptEntityBase In scripts
                If script.IsModelMatch(node.ModelNumber) AndAlso Not String.IsNullOrWhiteSpace(script.Name) Then
                    If Not Me.Session.IsNil(node.IsController, node.Number, script.Name) Then
                        resetRequired = resetRequired OrElse IsSaveRequired(script.Name, node,
                                                                        ((script.FileFormat And ScriptFileFormats.Binary) <> 0),
                                                                        script.IsBootScript)
                        success = success And Me.SaveUserScript(script.Name, node,
                                                                ((script.FileFormat And ScriptFileFormats.Binary) <> 0),
                                                                script.IsBootScript, script.Timeout)
                    End If
                End If
            Next
        End If

        If resetRequired Then
            ' reset to refresh the instrument display.
            Me.LinkSubsystem.ResetNode(node)
        End If

        If success Then
            Me.DisplaySubsystem.DisplayLine(2, "All scripts saved on node {0}", node.Number)
        Else
            Me.DisplaySubsystem.DisplayLine(2, "Failed saving script on node {0}", node.Number)
        End If

        Return success

    End Function

    ''' <summary> Saves all users scripts. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts"> Specifies the list of the scripts to be deleted. </param>
    ''' <param name="nodes">   Specifies the list of nodes on which scripts are deleted. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Overloads Function SaveUserScripts(ByVal scripts As ScriptEntityCollection,
                                              ByVal nodes As NodeEntityCollection) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If nodes Is Nothing Then Throw New ArgumentNullException(NameOf(nodes))

        ' clear buffers before deleting.
        Me.Session.DiscardUnreadData()

        Dim success As Boolean = True
        For Each node As NodeEntityBase In nodes
            success = success And SaveUserScripts(scripts, node)
        Next
        Return success

    End Function

    ''' <summary> Updates users scripts. Deletes out-dated scripts and loads and runs new scripts as
    ''' required on all nodes. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts">      Specifies the list of the scripts to be deleted. </param>
    ''' <param name="node">         Specifies the node on which scripts are updated. </param>
    ''' <param name="isSecondPass"> Set true on the second pass through if first pass requires
    ''' loading new scripts. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Public Function UpdateUserScripts(ByVal scripts As ScriptEntityCollection,
                                      ByVal node As NodeEntityBase, ByVal isSecondPass As Boolean) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        Dim prefix As String = ""

        If node.IsController Then
            prefix = String.Format(Globalization.CultureInfo.CurrentCulture, "Instrument '{0}'", Me.ResourceNameCaption)
        Else
            prefix = String.Format(Globalization.CultureInfo.CurrentCulture, "Instrument '{0}' node #{1}", Me.ResourceNameCaption, node.Number)
        End If

        ' run scripts so that we can read their version numbers. Scripts will run only if not ran, namely 
        ' if there namespaces are not defined.
        If Not scripts.RunScripts(node, Me) Then

            ' report any failure.
            If isSecondPass Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                  "{0} failed running some firmware scripts because {1};. {2}{3}",
                                  prefix, scripts.OutcomeDetails, Environment.NewLine, New StackFrame(True).UserCallStack())
                Return False
            Else
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                  "{0} failed running some firmware scripts because {1}--problem ignored;. ", prefix, scripts.OutcomeDetails)
            End If
        End If

        ' read scripts versions.
        If Not scripts.ReadFirmwareVersions(node, Me.Session) Then

            If isSecondPass Then
                ' report any failure.
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                   " failed reading some firmware versions because {1};. {2}{3}",
                                   prefix, scripts.OutcomeDetails, Environment.NewLine, New StackFrame(True).UserCallStack())
                Return False
            Else
                ' report any failure.
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   " failed reading some firmware versions because {1}. Problem ignored;. ",
                                   prefix, scripts.OutcomeDetails)
            End If
        End If

        ' make sure program is up to date.
        If Not isSecondPass AndAlso scripts.IsProgramOutdated(node) Then
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                               "{0} program out of date because {1}. System initialization aborted;. {2}{3}",
                               prefix, scripts.OutcomeDetails, Environment.NewLine, New StackFrame(True).UserCallStack())
            Return False
        End If

        If scripts.VersionsUnspecified(node) Then
            If isSecondPass Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                   "{0} failed verifying firmware version because {1};. {2}{3}",
                                   prefix, scripts.OutcomeDetails, Environment.NewLine, New StackFrame(True).UserCallStack())
                Return False
            Else
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} failed verifying firmware version because {1}--problem ignored;. ",
                                   prefix, scripts.OutcomeDetails)
            End If
        End If

        If scripts.AllVersionsCurrent(node) Then

            Return True

        ElseIf isSecondPass Then

            If String.IsNullOrWhiteSpace(scripts.OutcomeDetails) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                   "{0} failed updating scripts;. Check log for details.{1}{2}",
                                   prefix, Environment.NewLine, New StackFrame(True).UserCallStack())
            Else
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                   "{0} failed updating scripts because {1};. {2}{3}",
                                   prefix, scripts.OutcomeDetails, Environment.NewLine, New StackFrame(True).UserCallStack())
            End If
            Return False

        Else

            isSecondPass = True

            ' delete scripts that are out-dated or slated for deletion.
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "{0} deleting out-dated scripts;. ", prefix)

            If Not Me.DeleteUserScripts(scripts, node, True, True) Then
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                   "{0} failed deleting out-dated scripts;. Check log for details. Problem ignored.", prefix)
            End If

            If LoadRunUserScripts(scripts, node) Then

                Return Me.UpdateUserScripts(scripts, node, True)

            Else

                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                   "{0} failed loading and/or running scripts;. Check log for details.{1}{2}",
                                   prefix, Environment.NewLine, New StackFrame(True).UserCallStack())
                Return False

            End If

        End If

    End Function

    ''' <summary> Updates users scripts deleting, as necessary, those that are out of date. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="scripts"> Specifies the list of the scripts to be deleted. </param>
    ''' <param name="nodes">   Specifies the list of nodes on which scripts are deleted. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function UpdateUserScripts(ByVal scripts As ScriptEntityCollection,
                                      ByVal nodes As NodeEntityCollection) As Boolean

        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If nodes Is Nothing Then Throw New ArgumentNullException(NameOf(nodes))

        ' clear buffers before deleting.
        Me.Session.DiscardUnreadData()

        For Each node As NodeEntityBase In nodes
            If Not UpdateUserScripts(scripts, node, False) Then
                Return False
            End If
        Next
        Return True

    End Function

#End Region

#Region " TSP X: WRITE "

    ''' <summary> Writes the script to file. </summary>
    ''' <param name="folderPath"> Specifies the script file folder. </param>
    ''' <param name="script">     Specifies the script. </param>
    ''' <param name="node">       Specifies the node. </param>
    ''' <param name="compress">   Specifies the compression condition. True to compress the source
    ''' before saving. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are
    ''' null. </exception>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function WriteScriptFile(ByVal folderPath As String, ByVal script As ScriptEntityBase,
                                    ByVal node As NodeEntityBase, ByVal compress As Boolean) As Boolean

        If folderPath Is Nothing Then Throw New ArgumentNullException(NameOf(folderPath))
        If script Is Nothing Then Throw New ArgumentNullException(NameOf(script))
        If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

        Dim filePath As String = String.Format(Globalization.CultureInfo.CurrentCulture, "{0}.{1}",
                                                   script.FileName, node.ModelNumber)
        filePath = System.IO.Path.Combine(folderPath, filePath)

        Try

            Using scriptFile As System.IO.StreamWriter = New System.IO.StreamWriter(filePath)

                If scriptFile Is Nothing Then

                    ' now report the error to the calling module
                    Throw New System.IO.IOException("Failed opening TSP Script File '" & filePath & "'.")

                End If
                Me.FetchScriptSource(script, node)
                If compress Then
                    scriptFile.WriteLine("{0}{1}{2}", ScriptEntity.CompressedPrefix,
                                         ScriptEntityBase.Compress(Me.LastFetchScriptSource),
                                         ScriptEntity.CompressedSuffix)
                Else
                    scriptFile.WriteLine(Me.LastFetchScriptSource)
                End If
                Return True

            End Using

        Catch

            ' clear receive buffer.
            Me.Session.DiscardUnreadData()

            Throw

        Finally
        End Try

    End Function

    ''' <summary> Writes the script to file. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="folderPath"> Specifies the script file folder. </param>
    ''' <param name="scripts">    Specifies the scripts. </param>
    ''' <param name="nodes">      Specifies the nodes. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function WriteScriptFiles(ByVal folderPath As String,
                                     ByVal scripts As ScriptEntityCollection,
                                     ByVal nodes As NodeEntityCollection) As Boolean

        If folderPath Is Nothing Then Throw New ArgumentNullException(NameOf(folderPath))
        If scripts Is Nothing Then Throw New ArgumentNullException(NameOf(scripts))
        If nodes Is Nothing Then Throw New ArgumentNullException(NameOf(nodes))

        ' clear receive buffer.
        Me.Session.DiscardUnreadData()

        Dim success As Boolean = True
        For Each node As NodeEntityBase In nodes
            For Each script As ScriptEntityBase In scripts
                ' do not save the script if is has no file name. Future upgrade might suggest adding a file name to the boot script.
                If script.IsModelMatch(node.ModelNumber) AndAlso Not String.IsNullOrWhiteSpace(script.FileName) AndAlso Not script.SavedToFile Then
                    Me.DisplaySubsystem.DisplayLine(2, "Writing {0}:{1}", node.ModelNumber, script.Name)
                    If Me.WriteScriptFile(folderPath, script, node, ((script.FileFormat And ScriptFileFormats.Compressed) <> 0)) Then
                        script.SavedToFile = True
                        success = success AndAlso True
                    Else
                        success = False
                    End If
                End If
            Next
        Next
        Return success

    End Function

#End Region

#Region " TSP X - FRAMEWORK SCRIPTS "

    ''' <summary> Define scripts. </summary>
    Public Overridable Sub DefineScripts()
    End Sub

    ''' <summary> Deletes the user scripts. </summary>
    ''' <param name="statusSubsystem">  A reference to a
    ''' <see cref="VI.Tsp.StatusSubsystemBase">status subsystem</see>. </param>
    ''' <param name="displaySubsystem"> A reference to a
    ''' <see cref="VI.Tsp.DisplaySubsystemBase">display subsystem</see>. </param>
    Public Overridable Sub DeleteUserScripts(ByVal statusSubsystem As VI.Tsp.StatusSubsystemBase,
                                             ByVal displaySubsystem As VI.Tsp.DisplaySubsystemBase)
    End Sub

    ''' <summary> Uploads the user scripts to the controller instrument. </summary>
    ''' <returns> <c>True</c> if uploaded; otherwise, <c>False</c>. </returns>
    Public Overridable Function UploadUserScripts(ByVal statusSubsystem As VI.Tsp.StatusSubsystemBase,
                                                  ByVal displaySubsystem As VI.Tsp.DisplaySubsystemBase,
                                                  ByVal accessSubsystem As VI.AccessSubsystemBase) As Boolean
        Return False
    End Function

    ''' <summary> Saves the user scripts applying the release value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="timeout">          The timeout. </param>
    ''' <param name="statusSubsystem">  A reference to a
    ''' <see cref="VI.Tsp.StatusSubsystemBase">status subsystem</see>. </param>
    ''' <param name="accessSubsystem">  The access subsystem. </param>
    ''' <param name="displaySubsystem"> A reference to a
    ''' <see cref="VI.Tsp.DisplaySubsystemBase">display subsystem</see>. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Overridable Overloads Function SaveUserScripts(ByVal timeout As TimeSpan,
                                                          ByVal statusSubsystem As VI.Tsp.StatusSubsystemBase,
                                                          ByVal displaySubsystem As VI.Tsp.DisplaySubsystemBase,
                                                          ByVal accessSubsystem As VI.AccessSubsystemBase) As Boolean
        Return False
    End Function

    ''' <summary> Try read parse user scripts. </summary>
    ''' <param name="statusSubsystem">  A reference to a
    ''' <see cref="VI.Tsp.StatusSubsystemBase">status subsystem</see>. </param>
    ''' <param name="displaySubsystem"> A reference to a
    ''' <see cref="VI.Tsp.DisplaySubsystemBase">display subsystem</see>. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Overridable Function TryReadParseUserScripts(ByVal statusSubsystem As VI.Tsp.StatusSubsystemBase,
                                                        ByVal displaySubsystem As VI.Tsp.DisplaySubsystemBase) As Boolean
        Return False
    End Function


#End Region

End Class

''' <summary>Enumerates the content type of a TSP chunk line. </summary>
Public Enum TspChunkLineContentType
    <ComponentModel.Description("Not Defined")> None
    <ComponentModel.Description("Start Comment Block")> StartCommentBlock
    <ComponentModel.Description("End Comment Block")> EndCommentBlock
    <ComponentModel.Description("Comment")> Comment
    <ComponentModel.Description("Syntax")> Syntax
    <ComponentModel.Description("Syntax and Start Comment Block")> SyntaxStartCommentBlock
    <ComponentModel.Description("Chunk Name Declaration")> ChunkNameDeclaration
    <ComponentModel.Description("Chunk Name Requirement")> ChunkNameRequire
    <ComponentModel.Description("Chunk Name Loaded")> ChunkNameLoaded
End Enum
