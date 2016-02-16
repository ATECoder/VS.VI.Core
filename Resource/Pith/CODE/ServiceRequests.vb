Imports System.ComponentModel
''' <summary> Gets or sets the status byte bits of the service request register. </summary>
''' <remarks> Enumerates the Status Byte Register Bits.
''' Use *STB? or status.request_event to read this register.
''' Use *SRE or status.request_enable to enable these services.
''' This attribute is used to read the status byte, which is returned as a
''' numeric value. The binary equivalent of the returned value indicates which
''' register bits are set.
''' </remarks>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
<System.Flags()> Public Enum ServiceRequests
    <Description("None")> None = 0
    ''' <summary>
    ''' Bit B0, Measurement Summary Bit (MSB). Set summary bit indicates
    ''' that an enabled measurement event has occurred.
    ''' </summary>
    <Description("Measurement Event (MSB)")> MeasurementEvent = &H1
    ''' <summary>
    ''' Bit B1, System Summary Bit (SSB). Set summary bit indicates
    ''' that an enabled system event has occurred.
    ''' </summary>
    <Description("System Event (SSB)")> SystemEvent = &H2
    ''' <summary>
    ''' Bit B2, Error Available (EAV). Set summary bit indicates that
    ''' an error or status message is present in the Error Queue.
    ''' </summary>
    <Description("Error Available (EAV)")> ErrorAvailable = &H4
    ''' <summary>
    ''' Bit B3, Questionable Summary Bit (QSB). Set summary bit indicates
    ''' that an enabled questionable event has occurred.
    ''' </summary>
    <Description("Questionable Event (QSB)")> QuestionableEvent = &H8
    ''' <summary>
    ''' Bit B4 (16), Message Available (MAV). Set summary bit indicates that
    ''' a response message is present in the Output Queue.
    ''' </summary>
    <Description("Message Available (MAV)")> MessageAvailable = &H10
    ''' <summary>Bit B5, Event Summary Bit (ESB). Set summary bit indicates 
    ''' that an enabled standard event has occurred.
    ''' </summary>
    <Description("Standard Event (ESB)")> StandardEvent = &H20 ' (32) ESB
    ''' <summary>
    ''' Bit B6 (64), Request Service (RQS)/Master Summary Status (MSS).
    ''' Set bit indicates that an enabled summary bit of the Status Byte Register
    ''' is set. Depending on how it is used, Bit B6 of the Status Byte Register
    ''' is either the Request for Service (RQS) bit or the Master Summary Status
    ''' (MSS) bit: When using the GPIB serial poll sequence of the unit to obtain
    ''' the status byte (serial poll byte), B6 is the RQS bit. When using
    ''' status.condition or the *STB? common command to read the status byte,
    ''' B6 is the MSS bit.
    ''' </summary>
    <Description("Request Service (RQS)/Master Summary Status (MSS)")> RequestingService = &H40
    ''' <summary>
    ''' Bit B7 (128), Operation Summary (OSB). Set summary bit indicates that
    ''' an enabled operation event has occurred.
    ''' </summary>
    <Description("Operation Event (OSB)")> OperationEvent = &H80
    ''' <summary>
    ''' Includes all bits.
    ''' </summary>
    <Description("All")> All = &HFF ' 255
    ''' <summary>
    ''' Unknown value due to, for example, error trying to get value from the device.
    ''' </summary>
    <Description("Unknown")> Unknown = &H100
End Enum

