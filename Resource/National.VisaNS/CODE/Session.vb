''' <summary> A National Instrument message based session. </summary>
''' <remarks> David, 11/20/2015. </remarks>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="11/20/2015" by="David" revision=""> Created. </history>
Public Class Session
    Inherits SessionBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the MessageBasedSession object from the specified
    ''' resource name. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

#Region " Disposable Support"

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
                Try
                    Me.DiscardSession()
                    Me._LastNativeError = Nothing
                Catch ex As Exception
                    Debug.Assert(Not Debugger.IsAttached, "Failed discarding enabled events.",
                                         "Failed discarding enabled events. Details: {0}", ex)
                End Try
            End If
        Finally
            Me.IsDisposed = True
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " SESSION "

    ''' <summary> Gets the sentinel indicating weather this is a dummy session. </summary>
    ''' <exception cref="NativeException"> Thrown when a Native error condition occurs. </exception>
    ''' <value> The dummy sentinel. </value>
    Public Overrides ReadOnly Property IsDummy As Boolean = False

    Public ReadOnly Property IsVisaSessionDisposed As Boolean

    ''' <summary> The visa session. </summary>
    Private _VisaSession As NationalInstruments.VisaNS.MessageBasedSession

    ''' <summary> The visa session. </summary>
    ''' <remarks> Must be defined without events; Otherwise, setting the timeout causes a memory exception. </remarks>
    Private Property VisaSession As NationalInstruments.VisaNS.MessageBasedSession
        Get
            Return Me._VisaSession
        End Get
        Set(value As NationalInstruments.VisaNS.MessageBasedSession)
            Me._VisaSession = value
            If Me._VisaSession.HardwareInterfaceType = NationalInstruments.VisaNS.HardwareInterfaceType.Tcpip Then
                Me._TcpIpSession = TryCast(Me.VisaSession, NationalInstruments.VisaNS.TcpipSession)
            End If
        End Set
    End Property

    Private _TcpIpSession As NationalInstruments.VisaNS.TcpipSession

    ''' <summary>
    ''' Gets the session open sentinel. When open, the session is capable of addressing the hardware.
    ''' See also <see cref="P:isr.VI.SessionBase.IsDeviceOpen" />.
    ''' </summary>
    ''' <value> The is session open. </value>
    Public Overrides ReadOnly Property IsSessionOpen As Boolean
        Get
            Return Me.VisaSession IsNot Nothing AndAlso Not Me.IsVisaSessionDisposed
        End Get
    End Property

    ''' <summary>
    ''' Initializes a new instance of the <see cref="NationalInstruments.VisaNS.Session" /> class.
    ''' </summary>
    ''' <remarks>
    ''' This method does not lock the resource. Rev 4.1 and 5.0 of VISA did not support this call and
    ''' could not verify the resource.
    ''' </remarks>
    ''' <exception cref="NativeException"> Thrown when a Native error condition occurs. </exception>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <param name="timeout">      The open timeout. </param>
    Protected Overrides Sub CreateSession(ByVal resourceName As String, ByVal timeout As TimeSpan)
        Try
            Me._LastNativeError = NativeError.Success
            If Me.Enabled Then
                Select Case ResourceManager.ParseResource(resourceName).InterfaceType
                    Case HardwareInterfaceType.Gpib
                        Me.VisaSession = New NationalInstruments.VisaNS.GpibSession(resourceName, NationalInstruments.VisaNS.AccessModes.NoLock,
                                                                                    CInt(timeout.TotalMilliseconds), True)
                    Case HardwareInterfaceType.Tcpip
                        Me.VisaSession = New NationalInstruments.VisaNS.TcpipSession(resourceName, NationalInstruments.VisaNS.AccessModes.NoLock,
                                                                                     CInt(timeout.TotalMilliseconds), True)
                    Case VI.HardwareInterfaceType.Usb
                        Me.VisaSession = New NationalInstruments.VisaNS.UsbSession(resourceName, NationalInstruments.VisaNS.AccessModes.NoLock,
                                                                                   CInt(timeout.TotalMilliseconds), True)
                    Case Else
                        Me.VisaSession = New NationalInstruments.VisaNS.MessageBasedSession(resourceName, NationalInstruments.VisaNS.AccessModes.NoLock,
                                                                                            CInt(timeout.TotalMilliseconds))
                End Select
                Me._IsVisaSessionDisposed = Not Me.VisaSession Is Nothing
            End If
            If Not Me.KeepAlive OrElse Not Me.IsAlive Then Throw New OperationFailedException($"Resource not found;. {resourceName}")
        Catch ex As NationalInstruments.VisaNS.VisaException
            Me.DisposeSession()
            Me._LastNativeError = New NativeError(ex.ErrorCode, resourceName, "@opening", "opening session")
            Throw New NativeException(Me._LastNativeError, ex)
        Catch
            Me.DisposeSession()
            Throw
        End Try
    End Sub

    ''' <summary> Dispose session. </summary>
    ''' <remarks> David, 3/14/2016. </remarks>
    Private Sub DisposeSession()
        Me._IsVisaSessionDisposed = True
        If Me.VisaSession IsNot Nothing Then
            Me._VisaSession.Dispose()
            Me._VisaSession = Nothing
        End If
    End Sub

    ''' <summary> Discards the session. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    ''' <exception cref="NativeException"> Thrown when a Native error condition occurs. </exception>
    Protected Overrides Sub DiscardSession()
        If Me.IsSessionOpen Then
            Try
                Me._LastNativeError = NativeError.Success
                Me.VisaSession.DiscardEvent(NationalInstruments.VisaNS.MessageBasedSessionEventType.AllEnabledEvents)
                Me.DisableServiceRequest()
            Catch ex As NationalInstruments.VisaNS.VisaException
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, "@discarding", "discarding session")
                Throw New NativeException(Me._LastNativeError, ex)
            Finally
                Me.DisposeSession()
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Searches for a listeners for the specified <see cref="ResourceName">reasource name</see>.
    ''' </summary>
    ''' <remarks>
    ''' David, 11/27/2015. Updates <see cref="ResourceFound">Resource Exists</see>
    ''' This could return a false positive because the resource manager does not always rely on
    ''' physical check of existence. The NI VISA Manager returns affirmative if the resource is
    ''' listed in the configuration file.
    ''' </remarks>
    ''' <returns> <c>true</c> if it the resource exists; otherwise <c>false</c> </returns>
    Public Overrides Function FindResource() As Boolean
        Dim result As Boolean = True
        If Me.Enabled Then
            result = ResourceManager.Exists(Me.ResourceName)
        End If
        Me.ResourceFound = result
        Return result
    End Function

    ''' <summary>
    ''' Gets or sets the sentinel indicating if call backs are performed in a specific
    ''' synchronization context.
    ''' </summary>
    ''' <remarks>
    ''' For .NET Framework 2.0, use SynchronizeCallbacks to specify that the object marshals
    ''' callbacks across threads appropriately.<para>
    ''' DH: 3339 Setting true prevents display.
    ''' </para><para>
    ''' Note that setting to false also breaks display updates.
    ''' </para>
    ''' </remarks>
    ''' <value>
    ''' The sentinel indicating if call backs are performed in a specific synchronization context.
    ''' </value>
    Public Overrides Property SynchronizeCallBacks As Boolean
        Get
            If Me.IsSessionOpen Then
                MyBase.SynchronizeCallbacks = Me.VisaSession.SynchronizeCallbacks
            End If
            Return MyBase.SynchronizeCallbacks
        End Get
        Set(value As Boolean)
            MyBase.SynchronizeCallbacks = value
            If Me.IsSessionOpen Then
                Me.VisaSession.SynchronizeCallbacks = value
            End If
        End Set
    End Property

#End Region

#Region " ERROR / STATUS "

    Private _LastNativeError As NativeError
    ''' <summary> Gets the last native error. </summary>
    ''' <value> The last native error. </value>
    Protected Overrides ReadOnly Property LastNativeError As NativeErrorBase
        Get
            Return Me._LastNativeError
        End Get
    End Property

#End Region

#Region " READ/WRITE "

    ''' <summary> Gets or sets the termination character. </summary>
    ''' <value> The termination character. </value>
    Public Overrides Property TerminationCharacter As Byte
        Get
            If Me.IsSessionOpen Then
                MyBase.TerminationCharacter = Me.VisaSession.TerminationCharacter
            End If
            Return MyBase.TerminationCharacter
        End Get
        Set(value As Byte)
            If value <> Me.TerminationCharacter Then
                MyBase.TerminationCharacter = value
                If Me.IsSessionOpen Then
                    Me.VisaSession.TerminationCharacter = value
                End If
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the timeout. </summary>
    ''' <value> The timeout. </value>
    Public Overrides Property Timeout As TimeSpan
        Get
            If Me.IsSessionOpen Then
                MyBase.Timeout = TimeSpan.FromMilliseconds(Me.VisaSession.Timeout)
            End If
            Return MyBase.Timeout
        End Get
        Set(value As TimeSpan)
            If value <> Me.Timeout Then
                MyBase.Timeout = value
                If Me.IsSessionOpen Then
                    Me.VisaSession.Timeout = CInt(value.TotalMilliseconds)
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Synchronously reads ASCII-encoded string data. </summary>
    ''' <remarks> David, 11/24/2015. </remarks>
    ''' <exception cref="NativeException"> Thrown when a Native error condition occurs. </exception>
    ''' <returns> The received message. </returns>
    Public Overrides Function ReadFreeLine() As String
        Try
            Me._LastNativeError = NativeError.Success
            If Me.IsSessionOpen Then
                Me.LastMessageReceived = Me.VisaSession.ReadString()
            Else
                Me.LastMessageReceived = Me.EmulatedReply
            End If
            Return Me.LastMessageReceived
        Catch ex As NationalInstruments.VisaNS.VisaException
            If Me.LastNodeNumber.HasValue Then
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, Me.LastNodeNumber.Value, Me.LastMessageSent, Me.LastAction)
            Else
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, Me.LastMessageSent, Me.LastAction)
            End If
            Throw New NativeException(Me._LastNativeError, ex)
        Finally
            Me.LastInputOutputTime = DateTime.Now
        End Try
    End Function

    ''' <summary>
    ''' Synchronously reads ASCII-encoded string data. Reads up to the
    ''' <see cref="TerminationCharacter">termination character</see>.
    ''' </summary>
    ''' <remarks> David, 11/24/2015. </remarks>
    ''' <exception cref="NativeException"> Thrown when a Native error condition occurs. </exception>
    ''' <returns> The received message. </returns>
    Public Overrides Function ReadFiniteLine() As String
        Try
            Me._LastNativeError = NativeError.Success
            If Me.IsSessionOpen Then
                Me.LastMessageReceived = Me.VisaSession.ReadString()
            Else
                Me.LastMessageReceived = Me.EmulatedReply
            End If
            Return Me.LastMessageReceived
        Catch ex As NationalInstruments.VisaNS.VisaException
            If Me.LastNodeNumber.HasValue Then
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, Me.LastNodeNumber.Value, Me.LastMessageSent, Me.LastAction)
            Else
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, Me.LastMessageSent, Me.LastAction)
            End If
            Throw New NativeException(Me._LastNativeError, ex)
        Finally
            Me.LastInputOutputTime = DateTime.Now
        End Try
    End Function

    ''' <summary>
    ''' Synchronously writes ASCII-encoded string data to the device or interface. Terminates the
    ''' data with the <see cref="TerminationCharacter">termination character</see>.
    ''' </summary>
    ''' <remarks> David, 11/24/2015. </remarks>
    ''' <exception cref="NativeException"> Thrown when a Native error condition occurs. </exception>
    ''' <param name="dataToWrite"> The data to write. </param>
    Public Overrides Sub Write(ByVal dataToWrite As String)
        If Not String.IsNullOrWhiteSpace(dataToWrite) Then
            Try
                Me._LastNativeError = NativeError.Success
                If Me.IsSessionOpen Then
                    Me.VisaSession.Write(dataToWrite)
                End If
                Me.LastMessageSent = dataToWrite
            Catch ex As NationalInstruments.VisaNS.VisaException
                If Me.LastNodeNumber.HasValue Then
                    Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, Me.LastNodeNumber.Value, dataToWrite, Me.LastAction)
                Else
                    Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, dataToWrite, Me.LastAction)
                End If
                Throw New NativeException(Me._LastNativeError, ex)
            Finally
                Me.LastInputOutputTime = DateTime.Now
            End Try
        End If
    End Sub

    ''' <summary> Sends a TCP/IP message to keep the socket connected. </summary>
    ''' <remarks> David, 3/14/2016. </remarks>
    ''' <returns> <c>true</c> if success; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overrides Function KeepAlive() As Boolean
        Dim affirmative As Boolean = True
        If Me.IsSessionOpen AndAlso Not Me._TcpIpSession Is Nothing Then
            Try
                Me._TcpIpSession.UnlockResource()
            Catch
                affirmative = False
            End Try
        End If
        Return affirmative
    End Function

    Private _InputBufferSize As Integer
    ''' <summary> Gets the size of the input buffer. </summary>
    ''' <value> The size of the input buffer. </value>
    Public Overrides ReadOnly Property InputBufferSize As Integer
        Get
            If Me.IsSessionOpen Then
                If Me._InputBufferSize = 0 Then Me._InputBufferSize = Me.VisaSession.DefaultBufferSize
                Return Me._InputBufferSize
            Else
                Return 4096
            End If
        End Get
    End Property

#End Region

#Region " REGISTERS "

    ''' <summary> Reads status byte. </summary>
    ''' <remarks> David, 11/17/2015. </remarks>
    ''' <returns> The status byte. </returns>
    Public Overrides Function ReadStatusByte() As ServiceRequests
        Try
            Me._LastNativeError = NativeError.Success
            Dim value As ServiceRequests = Me.EmulatedStatusByte
            Me.EmulatedStatusByte = 0
            If Me.IsSessionOpen Then
                value = CType(Me.VisaSession.ReadStatusByte, ServiceRequests)
            End If
            Return value
        Catch ex As NationalInstruments.VisaNS.VisaException
            If Me.LastNodeNumber.HasValue Then
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, Me.LastNodeNumber.Value, "@STB", Me.LastAction)
            Else
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, "@STB", Me.LastAction)
            End If
            Throw New NativeException(Me._LastNativeError, ex)
        Finally
            Me.LastInputOutputTime = DateTime.Now
        End Try
    End Function

    ''' <summary> Clears the device. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public Overrides Sub Clear()
        Try
            Me._LastNativeError = NativeError.Success
            If Me.IsSessionOpen Then Me.VisaSession.Clear()
        Catch ex As NationalInstruments.VisaNS.VisaException
            If Me.LastNodeNumber.HasValue Then
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, Me.LastNodeNumber.Value, "@DCL", Me.LastAction)
            Else
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, "@DCL", Me.LastAction)
            End If
            Throw New NativeException(Me._LastNativeError, ex)
        Finally
            Me.LastInputOutputTime = DateTime.Now
        End Try
    End Sub

#End Region

#Region " EVENTS "

    ''' <summary> Visa session service request. </summary>
    ''' <remarks> David, 11/21/2015. </remarks>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Message based session event information. </param>
    Private Overloads Sub OnServiceRequested(sender As Object, e As NationalInstruments.VisaNS.MessageBasedSessionEventArgs)
        If sender IsNot Nothing AndAlso e IsNot Nothing AndAlso
            NationalInstruments.VisaNS.MessageBasedSessionEventType.ServiceRequest = e.EventType Then
            Me.OnServiceRequested()
        End If
    End Sub

    ''' <summary> Gets the type of the enabled event. </summary>
    ''' <exception cref="NativeException"> Thrown when a Native error condition occurs. </exception>
    ''' <value> The type of the enabled event. </value>
    Private Property EnabledEventType As NationalInstruments.VisaNS.MessageBasedSessionEventType = NationalInstruments.VisaNS.MessageBasedSessionEventType.Custom

    ''' <summary> Gets the sentinel indication if a service request event was enabled. </summary>
    ''' <value> <c>True</c> if service request event is enabled; otherwise, <c>False</c>. </value>
    Public Overrides ReadOnly Property IsServiceRequestEventEnabled As Boolean
        Get
            Return NationalInstruments.VisaNS.MessageBasedSessionEventType.ServiceRequest = Me.EnabledEventType
        End Get
    End Property

    ''' <summary> Enables the service request. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public Overrides Sub EnableServiceRequest()
        Try
            If Not Me.IsServiceRequestEventEnabled Then
                Me._LastNativeError = NativeError.Success
                If Me.IsSessionOpen Then
                    ' must define the handler before enabling the events.
                    AddHandler Me.VisaSession.ServiceRequest, AddressOf OnServiceRequested
                    Me.VisaSession.EnableEvent(NationalInstruments.VisaNS.MessageBasedSessionEventType.ServiceRequest,
                                               NationalInstruments.VisaNS.EventMechanism.Handler)
                End If
                Me.EnabledEventType = NationalInstruments.VisaNS.MessageBasedSessionEventType.ServiceRequest
            End If
        Catch ex As NationalInstruments.VisaNS.VisaException
            If Me.LastNodeNumber.HasValue Then
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, Me.LastNodeNumber.Value, "@Enable SRQ", Me.LastAction)
            Else
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, "@Enable SRQ", Me.LastAction)
            End If
            Throw New NativeException(Me._LastNativeError, ex)
        End Try
    End Sub

    ''' <summary> Disables the service request. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public Overrides Sub DisableServiceRequest()
        Try
            If Me.IsServiceRequestEventEnabled Then
                Me._LastNativeError = NativeError.Success
                If Me.IsSessionOpen Then
                    ' must disable before removing the handler.
                    Me.VisaSession.DiscardEvent(NationalInstruments.VisaNS.MessageBasedSessionEventType.ServiceRequest)
                    Me.VisaSession.DisableEvent(NationalInstruments.VisaNS.MessageBasedSessionEventType.ServiceRequest,
                                                NationalInstruments.VisaNS.EventMechanism.Handler)
                    RemoveHandler Me.VisaSession.ServiceRequest, AddressOf OnServiceRequested
                End If
                Me.EnabledEventType = NationalInstruments.VisaNS.MessageBasedSessionEventType.Custom
            End If
        Catch ex As NationalInstruments.VisaNS.VisaException
            If Me.LastNodeNumber.HasValue Then
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, Me.LastNodeNumber.Value, "@Disable SRQ", Me.LastAction)
            Else
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, "@Disable SRQ", Me.LastAction)
            End If
            Throw New NativeException(Me._LastNativeError, ex)
        End Try
    End Sub

#End Region

#Region " TRIGGER "

    ''' <summary> Assert trigger. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public Overrides Sub AssertTrigger()
        Try
            Me._LastNativeError = NativeError.Success
            If Me.IsSessionOpen Then Me.VisaSession.AssertTrigger()
        Catch ex As NationalInstruments.VisaNS.VisaException
            If Me.LastNodeNumber.HasValue Then
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, Me.LastNodeNumber.Value, "@TRG", Me.LastAction)
            Else
                Me._LastNativeError = New NativeError(ex.ErrorCode, Me.ResourceName, "@TRG", Me.LastAction)
            End If
            Throw New NativeException(Me._LastNativeError, ex)
        Finally
            Me.LastInputOutputTime = DateTime.Now
        End Try
    End Sub


#End Region

#Region " INTERFACE "

    ''' <summary> Clears the interface. </summary>
    Public Overrides Sub ClearInterface()
        If Me.SupportsClearInterface AndAlso Me.Enabled Then
            Using gi As GpibInterfaceSession = New GpibInterfaceSession()
                gi.OpenSession(Me.ResourceInfo.InterfaceResourceName)
                If gi.IsOpen Then
                    gi.SendInterfaceClear()
                Else
                    Throw New OperationFailedException($"Failed opening GPIB Interface Session {Me.ResourceInfo.InterfaceResourceName}")
                End If
            End Using
        End If
    End Sub

    ''' <summary> Clears the device (SDC). </summary>
    Public Overrides Sub ClearDevice()
        Me.Clear()
        If Me.SupportsClearInterface AndAlso Me.Enabled Then
            Using gi As GpibInterfaceSession = New GpibInterfaceSession()
                gi.OpenSession(Me.ResourceInfo.InterfaceResourceName)
                If gi.IsOpen Then
                    gi.SelectiveDeviceClear(Me.VisaSession.ResourceName)
                Else
                    Throw New OperationFailedException($"Failed opening GPIB Interface Session {Me.ResourceInfo.InterfaceResourceName}")
                End If
            End Using
        End If
    End Sub

#End Region

End Class

