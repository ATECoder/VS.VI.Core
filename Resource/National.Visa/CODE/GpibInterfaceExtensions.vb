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
Friend Module GpibInterfaceExtensions

#Region " GPIB INTERFACE "

    ''' <summary> Returns all instruments to some default state. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value"> The value. </param>
    <Extension()>
    Public Sub ClearDevices(ByVal value As NationalInstruments.Visa.GpibInterface)
        If value Is Nothing Then
            Throw New ArgumentNullException(NameOf(value))
        End If
        ' Transmit the DCL command to the interface.
        value.SendCommand(VI.Pith.Ieee488.Syntax.BuildDeviceClear.ToArray)
    End Sub

    ''' <summary> Clears the specified device. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">       The value. </param>
    ''' <param name="gpibAddress"> The instrument address. </param>
    <Extension()>
    Public Sub SelectiveDeviceClear(ByVal value As NationalInstruments.Visa.GpibInterface, ByVal gpibAddress As Integer)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        value.SendCommand(VI.Pith.Ieee488.Syntax.BuildSelectiveDeviceClear(CByte(gpibAddress)).ToArray)
    End Sub

    ''' <summary> Clears the specified device. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value">        The value. </param>
    ''' <param name="resourceName"> Name of the resource. </param>
    <Extension()>
    Public Sub SelectiveDeviceClear(ByVal value As NationalInstruments.Visa.GpibInterface, ByVal resourceName As String)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        If String.IsNullOrWhiteSpace(resourceName) Then Throw New ArgumentNullException(NameOf(resourceName))
        Using rc As ResourceManager = New ResourceManager()
            Dim parseResult As VI.Pith.ResourceNameParseInfo = rc.ParseResource(resourceName)
            If parseResult.GpibAddress > 0 Then value.SelectiveDeviceClear(parseResult.GpibAddress)
        End Using
    End Sub

    ''' <summary> Clears the interface. 
    '''           Resets interface if instruments are not connected.
    '''           </summary>
    <Extension()>
    Public Sub ClearInterface(ByVal value As NationalInstruments.Visa.GpibInterface)
        value.SendInterfaceClear()
        value.ClearDevices()
    End Sub

#End Region

End Module
