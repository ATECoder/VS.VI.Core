Imports System.Runtime.CompilerServices
''' <summary> Extensions for GPIB Interface. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/21/2005" by="David" revision="1.0.1847.x"> Created. </history>
Public Module GpibInterfaceExtensions

#Region " GPIB INTERFACE "

    ''' <summary> Returns all instruments to some default state. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value"> The value. </param>
    <Extension()>
    Public Sub ClearDevices(ByVal value As NationalInstruments.VisaNS.GpibInterface)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Try
            ' Transmit the DCL command to the interface.
            value.SendCommand(Ieee488.Syntax.BuildDeviceClear.ToArray)
        Catch ex As NationalInstruments.VisaNS.VisaException
            Dim nativeError As New NativeError(ex.ErrorCode, value.ResourceName, "@GPIB.DCL", "Clearing Devices")
            Throw New NativeException(nativeError, ex)
        End Try
    End Sub

    ''' <summary> Clears the specified device. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">       The value. </param>
    ''' <param name="gpibAddress"> The instrument address. </param>
    <Extension()>
    Public Sub SelectiveDeviceClear(ByVal value As NationalInstruments.VisaNS.GpibInterface, ByVal gpibAddress As Integer)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Try
            ' transmit the SDC command
            value.SendCommand(Ieee488.Syntax.BuildSelectiveDeviceClear(CByte(gpibAddress)).ToArray)
        Catch ex As NationalInstruments.VisaNS.VisaException
            Dim nativeError As New NativeError(ex.ErrorCode, value.ResourceName, "@GPIB.SDC", "Clearing Device")
            Throw New NativeException(nativeError, ex)
        End Try
    End Sub

    ''' <summary> Clears the specified device. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">        The value. </param>
    ''' <param name="resourceName"> Name of the resource. </param>
    <Extension()>
    Public Sub SelectiveDeviceClear(ByVal value As NationalInstruments.VisaNS.GpibInterface, ByVal resourceName As String)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        If String.IsNullOrWhiteSpace(resourceName) Then Throw New ArgumentNullException(NameOf(resourceName))
        Dim parseResult As VI.ResourceParseResult = NationalInstruments.VisaNS.ResourceManager.GetLocalManager.ParseResource(resourceName)
        If parseResult.GpibAddress > 0 Then value.SelectiveDeviceClear(parseResult.GpibAddress)
    End Sub

    ''' <summary> Clears the interface. 
    '''           Resets interface if instruments are not connected.
    '''           </summary>
    <Extension()>
    Public Sub ClearInterface(ByVal value As NationalInstruments.VisaNS.GpibInterface)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        value.SendInterfaceClear()
        value.ClearDevices()
    End Sub

#End Region
End Module
