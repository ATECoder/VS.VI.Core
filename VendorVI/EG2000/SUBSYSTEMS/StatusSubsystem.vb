Imports isr.Core.Pith.EscapeSequencesExtensions
''' <summary> Defines a Status Subsystem for a EG2000 Prober. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/01/2013" by="David" revision="3.0.5022"> Created. </history>
Public Class StatusSubsystem
    Inherits VI.R2D2.StatusSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystem" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.Pith.SessionBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal visaSession As VI.Pith.SessionBase)
        MyBase.New(visaSession)

        Me._VersionInfo = New VersionInfo

        ' operation completion not support -- set to Completed.
        Me.OperationCompleted = True

    End Sub

    ''' <summary> Creates a new StatusSubsystem. </summary>
    ''' <returns> A StatusSubsystem. </returns>
    Public Shared Function Create() As StatusSubsystem
        Dim subsystem As StatusSubsystem = Nothing
        Try
            subsystem = New StatusSubsystem(isr.VI.SessionFactory.Get.Factory.CreateSession())
        Catch
            If subsystem IsNot Nothing Then
                subsystem.Dispose()
                subsystem = Nothing
            End If
            Throw
        End Try
        Return subsystem
    End Function

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.OperationCompleted = True
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " STANDARD STATUS  "

    ''' <summary> Gets the clear execution state command. </summary>
    ''' <value> The clear execution state command. </value>
    ''' <remarks> reset and clear not supported </remarks>
    Protected Overrides ReadOnly Property ClearExecutionStateCommand As String = ""

    ''' <summary> Gets the reset known state command. </summary>
    ''' <value> The reset known state command. </value>
    ''' <remarks> reset and clear not supported </remarks>
    Protected Overrides ReadOnly Property ResetKnownStateCommand As String = ""

    ''' <summary> Gets the bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Public Overrides ReadOnly Property ErrorAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.ErrorAvailable

    ''' <summary> Gets the bits that would be set for detecting if an Measurement is available. </summary>
    ''' <value> The Measurement available bits. </value>
    Public Overrides ReadOnly Property MeasurementAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.MeasurementEvent

    ''' <summary> Gets the bits that would be set for detecting if an Message is available. </summary>
    ''' <value> The Message available bits. </value>
    Public Overrides ReadOnly Property MessageAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.RequestingService

    ''' <summary> Gets the operation completed query command. </summary>
    ''' <value> The operation completed query command. </value>
    Protected Overrides ReadOnly Property OperationCompletedQueryCommand As String = ""

    ''' <summary> Gets the bits that would be set for detecting if a Standard Event is available. </summary>
    ''' <value> The Standard Event available bits. </value>
    Public Overrides ReadOnly Property StandardEventAvailableBits As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.StandardEvent

    ''' <summary> Gets the standard event status query command. </summary>
    ''' <value> The standard event status query command. </value>
    Protected Overrides ReadOnly Property StandardEventStatusQueryCommand As String = ""

    ''' <summary> Gets the standard event enable query command. </summary>
    ''' <value> The standard event enable query command. </value>
    Protected Overrides ReadOnly Property StandardEventEnableQueryCommand As String = ""

    ''' <summary> Gets the standard service enable command format. </summary>
    ''' <value> The standard service enable command format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCommandFormat As String = ""

    ''' <summary> Gets the standard service enable and complete command format. </summary>
    ''' <value> The standard service enable command and complete format. </value>
    Protected Overrides ReadOnly Property StandardServiceEnableCompleteCommandFormat As String = ""

    ''' <summary> Gets the service request enable command format. </summary>
    ''' <value> The service request enable command format. </value>
    Protected Overrides ReadOnly Property ServiceRequestEnableCommandFormat As String = ""

#End Region

#End Region

#Region " IDENTITY "

    ''' <summary> Gets the identity query command. </summary>
    ''' <value> The identity query command. </value>
    Protected Overrides ReadOnly Property IdentityQueryCommand As String = "ID"

    ''' <summary> Queries the Identity. </summary>
    ''' <returns> System.String. </returns>
    Public Overrides Function QueryIdentity() As String
        If Not String.IsNullOrWhiteSpace(Me.IdentityQueryCommand) Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Requesting identity;. ")
            System.Windows.Forms.Application.DoEvents()
            Me.WriteIdentityQueryCommand()
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Trying to read identity;. ")
            System.Windows.Forms.Application.DoEvents()
            Dim value As String = ""
            If Me.TryRead(value) Then
                Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Identity {0};. ", value)
                Me.Identity = value.ReplaceCommonEscapeSequences
            Else
                Me.Identity = "2001X.CD.999999-011"
            End If
        End If
        Return Me.Identity
    End Function

    ''' <summary> Parse version information. </summary>
    ''' <param name="value"> The value. </param>
    Protected Overrides Sub ParseVersionInfo(value As String)
        Me._VersionInfo = New VersionInfo
        Me.VersionInfo.Parse(value)
        MyBase.ParseVersionInfo(Me.VersionInfo.BuildIdentity)
    End Sub

    ''' <summary> Gets or sets the information describing the version. </summary>
    ''' <value> Information describing the version. </value>
    Public Overloads ReadOnly Property VersionInfo As VersionInfo

#End Region

#Region " DEVICE ERRORS "

    ''' <summary> Gets the clear error queue command. </summary>
    ''' <value> The clear error queue command. </value>
    Protected Overrides ReadOnly Property ClearErrorQueueCommand As String = ""

    ''' <summary> Gets the error queue query command. </summary>
    ''' <value> The error queue query command. </value>
    Protected Overrides ReadOnly Property NextDeviceErrorQueryCommand As String = ""

    ''' <summary> Gets the last error query command. </summary>
    ''' <value> The last error query command. </value>
    Protected Overrides ReadOnly Property DeviceErrorQueryCommand As String = "?E"

    ''' <summary> Queues device error. </summary>
    ''' <param name="compoundErrorMessage"> Message describing the compound error. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Protected Overrides Function EnqueueDeviceError(ByVal compoundErrorMessage As String) As VI.DeviceError
        Dim de As DeviceError = New DeviceError()
        de.Parse(compoundErrorMessage)
        If de.IsError Then Me.DeviceErrorQueue.Enqueue(de)
        Return de
    End Function

#End Region

#Region " READ "

    ''' <summary> Attempts to read from the given data. </summary>
    ''' <param name="value"> [in,out] The value. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryRead(ByRef value As String) As Boolean
        Dim bits As VI.Pith.ServiceRequests = Me.MessageAvailableBits
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Awaiting SQR bits {0:X2};. ", CInt(bits))
        If Me.TryAwaitServiceRequest(bits, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(10)) Then
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Fetching;. ")
            value = Me.Session.ReadLineTrimEnd
        Else
            Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, "Timeout awaiting SQR bits {0:X2};. ", CInt(bits))
        End If
        Return Not String.IsNullOrEmpty(value)
    End Function

#End Region

#Region " TALKER "

    ''' <summary> Identifies talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

#End Region


End Class
