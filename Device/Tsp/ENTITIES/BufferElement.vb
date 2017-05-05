''' <summary> A buffer element. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/9/2016" by="David" revision=""> Created. </history>
Public Class BufferElement

    ''' <summary> Gets or sets the value. </summary>
    ''' <value> The value. </value>
    Property Value As Double

    ''' <summary> Gets or sets the timestamp. </summary>
    ''' <value> The timestamp. </value>
    Property Timestamp As DateTime

    ''' <summary> Gets or sets the relative time. </summary>
    ''' <value> The relative time. </value>
    Property RelativeTime As TimeSpan

    ''' <summary> Gets or sets the status. </summary>
    ''' <value> The status. </value>
    Property Status As BufferStatusBits

End Class

''' <summary>Enumerates the status bits as defined in reading buffers.</summary>
<Flags()> Public Enum BufferStatusBits
    <ComponentModel.Description("Not defined")> None = 0
    ''' <summary>0x02 over temperature condition.</summary>
    <ComponentModel.Description("Over Temp")> OverTemp = 2
    ''' <summary>0x04 measure range was auto ranged.</summary>
    <ComponentModel.Description("Auto Range Measure")> AutoRangeMeasure = 4
    ''' <summary>0x08 source range was auto ranged.</summary>
    <ComponentModel.Description("Auto Range Source")> AutoRangeSource = 8
    ''' <summary>0x10 4W (remote) sense mode.</summary>
    <ComponentModel.Description("Four Wire")> FourWire = 16
    ''' <summary>0x20 relative applied to reading.</summary>
    <ComponentModel.Description("Relative")> Relative = 32
    ''' <summary>0x40 source function in compliance.</summary>
    <ComponentModel.Description("Compliance")> Compliance = 64
    ''' <summary>0x80 reading was filtered.</summary>
    <ComponentModel.Description("Filtered")> Filtered = 128
End Enum

