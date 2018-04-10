''' <summary> Defines the contract that must be implemented by Subsystems that report status. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created. </history>
Public MustInherit Class SubsystemPlusStatusBase
    Inherits SubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SubsystemPlusStatusBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId:="0")>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem.Session)
        Me._StatusSubsystem = statusSubsystem
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
                Me._StatusSubsystem = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " STATUS "

    Private _StatusSubsystem As StatusSubsystemBase

    ''' <summary> Gets the status subsystem. </summary>
    ''' <value> The status subsystem. </value>
    Protected ReadOnly Property StatusSubsystem As StatusSubsystemBase
        Get
            Return _StatusSubsystem
        End Get
    End Property

    ''' <summary> Reads the registers. </summary>
    Public Sub ReadRegisters()
        Me.StatusSubsystem.ReadEventRegisters()
    End Sub

#End Region

#Region " CHECK AND THROW "

    ''' <summary> Checks and throws an exception if device errors occurred.
    ''' Can only be used after receiving a full reply from the device. </summary>
    ''' <exception cref="VI.Pith.DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="flushReadFirst"> Flushes the read buffer before processing the error. </param>
    ''' <param name="format">         Describes the format to use. </param>
    ''' <param name="args">           A variable-length parameters list containing arguments. </param>
    Public Sub CheckThrowDeviceException(ByVal flushReadFirst As Boolean, ByVal format As String, ByVal ParamArray args() As Object)
        Me.StatusSubsystem.CheckThrowDeviceException(flushReadFirst, format, args)
    End Sub

#End Region

#Region " CHECK AND REPORT "

    ''' <summary> Check and reports visa or device error occurred. Can only be used after receiving a
    ''' full reply from the device. </summary>
    ''' <param name="nodeNumber"> Specifies the remote node number to validate. </param>
    ''' <param name="format">     Specifies the report format. </param>
    ''' <param name="args">       Specifies the report arguments. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Overridable Function TraceVisaDeviceOperationOkay(ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return Me.StatusSubsystem.TraceVisaDeviceOperationOkay(nodeNumber, format, args)
    End Function

    ''' <summary> Checks and reports if a visa or device error occurred. Can only be used after
    ''' receiving a full reply from the device. </summary>
    ''' <param name="nodeNumber">     Specifies the remote node number to validate. </param>
    ''' <param name="flushReadFirst"> Flushes the read buffer before processing the error. </param>
    ''' <param name="format">         Describes the format to use. </param>
    ''' <param name="args">           A variable-length parameters list containing arguments. </param>
    ''' <returns> <c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function TraceVisaDeviceOperationOkay(ByVal nodeNumber As Integer,
                                                  ByVal flushReadFirst As Boolean, ByVal format As String,
                                                  ByVal ParamArray args() As Object) As Boolean
        Return Me.StatusSubsystem.TraceVisaDeviceOperationOkay(nodeNumber, flushReadFirst, format, args)
    End Function

    ''' <summary> Checks and reports if a visa or device error occurred.
    ''' Can only be used after receiving a full reply from the device. </summary>
    ''' <param name="flushReadFirst"> Flushes the read buffer before processing the error. </param>
    ''' <param name="format">         Describes the format to use. </param>
    ''' <param name="args">           A variable-length parameters list containing arguments. </param>
    ''' <returns><c>True</c> if okay; otherwise, <c>False</c>. </returns>
    Public Function TraceVisaDeviceOperationOkay(ByVal flushReadFirst As Boolean, ByVal format As String,
                                                 ByVal ParamArray args() As Object) As Boolean
        Return Me.StatusSubsystem.TraceVisaDeviceOperationOkay(flushReadFirst, format, args)
    End Function

    ''' <summary> Reports if a visa error occurred.
    '''           Can be used with queries. </summary>
    ''' <param name="format">   Describes the format to use. </param>
    ''' <param name="args">     A variable-length parameters list containing arguments. </param>
    Public Sub TraceVisaOperation(ByVal format As String, ByVal ParamArray args() As Object)
        Me.StatusSubsystem.TraceVisaOperation(format, args)
    End Sub

    ''' <summary> Reports if a visa error occurred.
    '''           Can be used with queries. </summary>
    ''' <param name="format">   Describes the format to use. </param>
    ''' <param name="args">     A variable-length parameters list containing arguments. </param>
    Public Sub TraceVisaOperation(ByVal ex As VI.Pith.NativeException, ByVal format As String, ByVal ParamArray args() As Object)
        Me.StatusSubsystem.TraceVisaOperation(ex, format, args)
    End Sub

    ''' <summary> Reports if a operation error occurred.
    '''           Can be used with queries. </summary>
    ''' <param name="format">   Describes the format to use. </param>
    ''' <param name="args">     A variable-length parameters list containing arguments. </param>
    Public Sub TraceOperation(ByVal ex As Exception, ByVal format As String, ByVal ParamArray args() As Object)
        Me.StatusSubsystem.TraceOperation(ex, format, args)
    End Sub

    ''' <summary> Reports if a visa error occurred. Can be used with queries. </summary>
    ''' <param name="nodeNumber">   Specifies the remote node number to validate. </param>
    ''' <param name="format"> Describes the format to use. </param>
    ''' <param name="args">   A variable-length parameters list containing arguments. </param>
    Public Sub TraceVisaOperation(ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object)
        Me.StatusSubsystem.TraceVisaOperation(nodeNumber, format, args)
    End Sub

    ''' <summary> Reports if a visa error occurred. Can be used with queries. </summary>
    ''' <param name="nodeNumber">   Specifies the remote node number to validate. </param>
    ''' <param name="format"> Describes the format to use. </param>
    ''' <param name="args">   A variable-length parameters list containing arguments. </param>
    Public Sub TraceVisaOperation(ByVal ex As VI.Pith.NativeException, ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object)
        Me.StatusSubsystem.TraceVisaOperation(ex, nodeNumber, format, args)
    End Sub

    ''' <summary> Reports if an operation error occurred. Can be used with queries. </summary>
    ''' <param name="nodeNumber">   Specifies the remote node number to validate. </param>
    ''' <param name="format"> Describes the format to use. </param>
    ''' <param name="args">   A variable-length parameters list containing arguments. </param>
    Public Sub TraceOperation(ByVal ex As Exception, ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object)
        Me.StatusSubsystem.TraceOperation(ex, nodeNumber, format, args)
    End Sub

#End Region

End Class

