''' <summary> A GPIB interface session. </summary>
''' <remarks> David, 11/21/2015. </remarks>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="11/21/2015" by="David" revision=""> Created. </history>
Public Class GpibInterfaceSession
    Inherits InterfaceSessionBase

#Region " CONSTRUCTOR "

    ''' <summary> Constructor. </summary>
    ''' <remarks> David, 11/21/2015. </remarks>
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
                    Me._CloseSession()
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

    ''' <summary> Gets or sets the sentinel indicating weather this is a dummy session. </summary>
    ''' <value> The dummy sentinel. </value>
    Public Overrides ReadOnly Property IsDummy As Boolean = True

    ''' <summary> Opens a session. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    Public Overrides Sub OpenSession(ByVal resourceName As String, ByVal timeout As TimeSpan)
        MyBase.OpenSession(resourceName, timeout)
        Me._OpenSession(resourceName, timeout)
    End Sub

    ''' <summary> Opens a session. </summary>
    ''' <remarks> David, 11/21/2015. </remarks>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <param name="timeout">      The timeout. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="resourceName")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="timeout")>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub _OpenSession(ByVal resourceName As String, ByVal timeout As TimeSpan)
        Me._IsOpen = True
    End Sub

    ''' <summary> Opens a session. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    ''' <param name="resourceName"> Name of the resource. </param>
    Public Overrides Sub OpenSession(ByVal resourceName As String)
        MyBase.OpenSession(resourceName)
        Me._OpenSession(resourceName)
    End Sub

    ''' <summary> Opens a session. </summary>
    ''' <remarks> David, 11/21/2015. </remarks>
    ''' <param name="resourceName"> Name of the resource. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="resourceName")>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Private Sub _OpenSession(ByVal resourceName As String)
        Me._IsOpen = True
    End Sub

    ''' <summary> Closes the session. </summary>
    ''' <remarks> David, 11/30/2015. </remarks>
    Public Overrides Sub CloseSession()
        Me._CloseSession()
    End Sub

    ''' <summary> Closes the session. </summary>
    ''' <remarks> David, 11/21/2015. </remarks>
    Private Sub _CloseSession()
        Me._IsOpen = False
    End Sub

    ''' <summary> Gets the is open. </summary>
    ''' <value> The is open. </value>
    Public Overrides ReadOnly Property IsOpen As Boolean

#End Region

#Region " GPIB INTERFACE "

    ''' <summary> Sends the interface clear. </summary>
    ''' <remarks> David, 11/21/2015. </remarks>
    Public Overrides Sub SendInterfaceClear()
    End Sub


    ''' <summary> Gets the type of the hardware interface. </summary>
    ''' <value> The type of the hardware interface. </value>
    Public Overrides ReadOnly Property HardwareInterfaceType As HardwareInterfaceType
        Get
            Return HardwareInterfaceType.Gpib
        End Get
    End Property

    ''' <summary> Gets the hardware interface number. </summary>
    ''' <value> The hardware interface number. </value>
    Public Overrides ReadOnly Property HardwareInterfaceNumber As Integer
        Get
            Return 0
        End Get
    End Property

    ''' <summary> Returns all instruments to some default state. </summary>
    Public Overrides Sub ClearDevices()
    End Sub

    ''' <summary> Clears the specified device. </summary>
    ''' <param name="gpibAddress"> The instrument address. </param>
    Public Overrides Sub SelectiveDeviceClear(ByVal gpibAddress As Integer)
    End Sub

    ''' <summary> Clears the specified device. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resourceName"> Name of the resource. </param>
    Public Overrides Sub SelectiveDeviceClear(ByVal resourceName As String)
    End Sub

    ''' <summary> Clears the interface. </summary>
    Public Overrides Sub ClearInterface()
    End Sub

#End Region

End Class

