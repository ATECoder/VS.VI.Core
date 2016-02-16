''' <summary> A Dummy message based session. </summary>
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
Public Class DummySession
    Inherits SessionBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the MessageBasedSession object from the specified
    ''' resource name. </summary>
    Public Sub New()
        MyBase.New()
        Me._Init()
    End Sub

    Private Sub _Init()
        Me._TerminationCharacter = 10
        Me._Timeout = TimeSpan.FromMilliseconds(3000)
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
    Public Overrides ReadOnly Property IsDummy As Boolean = True

    ''' <summary> Creates a session. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    ''' <param name="resourceName"> Name of the resource. </param>
    Protected Overrides Sub CreateSession(ByVal resourceName As String)
        Me._LastNativeError = DummyNativeError.Success
    End Sub

    ''' <summary> Initializes a dummy session. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <param name="timeout">      The open timeout. </param>
    Protected Overrides Sub CreateSession(ByVal resourceName As String, ByVal timeout As TimeSpan)
        Me._LastNativeError = DummyNativeError.Success
    End Sub

    ''' <summary> Discards session. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    Protected Overrides Sub DiscardSession()
        Me._LastNativeError = DummyNativeError.Success
        Me.DisableServiceRequest()
    End Sub

    ''' <summary> Searches for a listeners for the specified <see cref="ResourceName">reasource name</see>. </summary>
    ''' <remarks> David, 11/27/2015. Updates <see cref="ResourceFound ">Resource Exists</see>. </remarks>
    ''' <returns> <c>true</c> if it the resource exists; otherwise <c>false</c> </returns>
    Public Overrides Function FindResource() As Boolean
        Using rm As New DummyResourceManager
            Me.ResourceFound = rm.Exists(Me.ResourceName)
        End Using
        Return Me.ResourceFound
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

#End Region

#Region " ERROR / STATUS "

    Private _LastNativeError As DummyNativeError
    ''' <summary> Gets the last native error. </summary>
    ''' <value> The last native error. </value>
    Protected Overrides ReadOnly Property LastNativeError As NativeErrorBase
        Get
            Return Me._LastNativeError
        End Get
    End Property

#End Region

#Region " ATTRIBUTES "

    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
    Private Function Get_ReadBufferSize() As Integer
        Return 1024
    End Function

#End Region

#Region " READ/WRITE "

    ''' <summary> Gets or sets the termination character. </summary>
    ''' <value> The termination character. </value>
    Public Overrides Property TerminationCharacter As Byte

    ''' <summary> Gets or sets the timeout. </summary>
    ''' <value> The timeout. </value>
    Public Overrides Property Timeout As TimeSpan

    ''' <summary>
    ''' Synchronously reads ASCII-encoded string data. Reads up to the
    ''' <see cref="TerminationCharacter">termination character</see>.
    ''' </summary>
    ''' <remarks> David, 11/24/2015. </remarks>
    ''' <exception cref="NativeException"> Thrown when a Native error condition occurs. </exception>
    ''' <returns> The received message. </returns>
    Public Overrides Function ReadString() As String
        Me._LastNativeError = DummyNativeError.Success
        Me.LastMessageReceived = Me.EmulatedReply
        Return Me.LastMessageReceived
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
            Me._LastNativeError = DummyNativeError.Success
            Me.LastMessageSent = dataToWrite
        End If
    End Sub

#End Region

#Region " REGISTERS "

    ''' <summary> Reads status byte. </summary>
    ''' <remarks> David, 11/17/2015. </remarks>
    ''' <returns> The status byte. </returns>
    Public Overrides Function ReadStatusByte() As ServiceRequests
        Me._LastNativeError = DummyNativeError.Success
        Return Me.EmulatedStatusByte
    End Function

    ''' <summary> Clears the device. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public Overrides Sub Clear()
        Me._LastNativeError = DummyNativeError.Success
    End Sub

#End Region

#Region " EVENTS "

    Private Property EnabledEventType As Boolean

    ''' <summary> Gets the sentinel indication if a service request event was enabled. </summary>
    ''' <value> <c>True</c> if service request event is enabled; otherwise, <c>False</c>. </value>
    Public Overrides ReadOnly Property IsServiceRequestEventEnabled As Boolean
        Get
            Return Me.EnabledEventType
        End Get
    End Property

    ''' <summary> Enables the service request. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public Overrides Sub EnableServiceRequest()
        If Not IsServiceRequestEventEnabled Then
            Me._LastNativeError = DummyNativeError.Success
            Me.EnabledEventType = True
        End If
    End Sub

    ''' <summary> Disables the service request. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public Overrides Sub DisableServiceRequest()
        If IsServiceRequestEventEnabled Then
            Me._LastNativeError = DummyNativeError.Success
            Me.EnabledEventType = False
        End If
    End Sub

#End Region

#Region " TRIGGER "

    ''' <summary> Assert trigger. </summary>
    ''' <remarks> David, 11/20/2015. </remarks>
    Public Overrides Sub AssertTrigger()
        Me._LastNativeError = DummyNativeError.Success
    End Sub

#End Region

#Region " INTERFACE "

    ''' <summary> Supports clear interface. </summary>
    ''' <returns> <c>True</c> if supports clearing the interface. </returns>
    Public Overrides Function SupportsClearInterface() As Boolean
        Return Me.ResourceInfo.InterfaceType = VI.HardwareInterfaceType.Gpib
    End Function

    ''' <summary> Clears the interface. </summary>
    Public Overrides Sub ClearInterface()
        If Me.SupportsClearInterface AndAlso Me.IsSessionOpen Then
            Using gi As DummyGpibInterfaceSession = New DummyGpibInterfaceSession()
                gi.OpenSession(Me.ResourceInfo.InterfaceResourceName)
                If gi.IsOpen Then
                    gi.SelectiveDeviceClear(Me.ResourceName)
                Else
                    Throw New OperationFailedException($"Failed opening GPIB Interface Session {Me.ResourceInfo.InterfaceResourceName}")
                End If
            End Using
        End If
    End Sub

    ''' <summary> Clears the device (SDC). </summary>
    Public Overrides Sub ClearDevice()
        Me.Clear()
        If Me.SupportsClearInterface AndAlso Me.IsSessionOpen Then
            Using gi As DummyGpibInterfaceSession = New DummyGpibInterfaceSession()
                gi.OpenSession(Me.ResourceInfo.InterfaceResourceName)
                If gi.IsOpen Then
                    gi.SelectiveDeviceClear(Me.ResourceName)
                Else
                    Throw New OperationFailedException($"Failed opening GPIB Interface Session {Me.ResourceInfo.InterfaceResourceName}")
                End If
            End Using
        End If
    End Sub

#End Region

End Class
