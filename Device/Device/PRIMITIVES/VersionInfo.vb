''' <summary> Parses and holds the instrument version information.</summary>
''' <license> (c) 2008 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2008" by="David" revision="2.0.2936.x">
''' Derived from previous Scpi Instrument implementation. </history>
Public Class VersionInfo

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Specialized default constructor for use only by derived classes. </summary>
    Public Sub New()
        MyBase.New()
        Me._Clear()
    End Sub

#End Region

#Region " INFORMATION PROPERTIES "

    ''' <summary> Gets or sets the identity information that was used to parse the version information. </summary>
    ''' <value> The identity. </value>
    Public Property Identity() As String

    ''' <summary> Gets or sets the extended version information. </summary>
    Public Property FirmwareRevision() As String

    ''' <summary>Gets or sets the instrument manufacturer name .</summary>
    Public Property ManufacturerName() As String

    ''' <summary> Gets or sets the instrument model number. </summary>
    ''' <value> A string property that may include additional precursors such as 'Model' to the
    ''' relevant information. </value>
    Public Property Model() As String

    ''' <summary> Gets or sets the serial number. </summary>
    ''' <value> The serial number. </value>
    Public Property SerialNumber() As String

    ''' <summary>Gets or sets the list of revision elements.</summary>
    Public Property FirmwareRevisionElements() As System.Collections.Specialized.StringDictionary

#End Region

#Region " PARSE "

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Private Sub _Clear()
        Me._FirmwareRevisionElements = New System.Collections.Specialized.StringDictionary
        Me._Identity = ""
        Me._FirmwareRevision = ""
        Me._ManufacturerName = ""
        Me._Model = ""
        Me._SerialNumber = ""
    End Sub

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Overridable Sub Clear()
        Me._Clear()
    End Sub

    ''' <summary> Parses the instrument firmware revision. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="revision"> Specifies the instrument firmware revision.. </param>
    Protected Overridable Sub ParseFirmwareRevision(ByVal revision As String)
        Me._FirmwareRevisionElements = New System.Collections.Specialized.StringDictionary
    End Sub

    ''' <summary> Builds the identity. </summary>
    ''' <returns> A String. </returns>
    Public Function BuildIdentity() As String
        Dim builder As New System.Text.StringBuilder
        If String.IsNullOrWhiteSpace(Me.ManufacturerName) Then
            builder.Append("Manufacturer,")
        Else
            builder.Append($"{Me.ManufacturerName},")
        End If
        If String.IsNullOrWhiteSpace(Me.Model) Then
            builder.Append("Model,")
        Else
            builder.Append($"{Me.Model},")
        End If
        If String.IsNullOrWhiteSpace(Me.SerialNumber) Then
            builder.Append("1,")
        Else
            builder.Append($"{Me.SerialNumber},")
        End If
        If String.IsNullOrWhiteSpace(Me.FirmwareRevision) Then
            builder.Append("Oct 10 1997")
        Else
            builder.Append($"{Me.FirmwareRevision}")
        End If
        Return builder.ToString
    End Function

    ''' <summary> Parses the instrument identity string. </summary>
    ''' <remarks> The firmware revision can be further interpreted by the child instruments. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="value"> Specifies the instrument identity string, which includes at a minimum the
    ''' following information:
    ''' <see cref="ManufacturerName">manufacturer</see>, <see cref="Model">model</see>,
    ''' <see cref="SerialNumber">serial number</see>, e.g., <para>
    ''' <c>KEITHLEY INSTRUMENTS INC.,MODEL 2420,0669977,C11 Oct 10 1997 09:51:36/A02
    ''' /D/B/E.</c>.</para> </param>
    Public Overridable Sub Parse(ByVal value As String)

        If String.IsNullOrEmpty(value) Then
            Me.Clear()
        Else

            ' save the identity.
            Me._Identity = value

            ' Parse the id to get the revision number
            Dim idItems As Queue(Of String) = New Queue(Of String)(value.Split(","c))

            ' company, e.g., KEITHLEY INSTRUMENTS,
            Me._ManufacturerName = idItems.Dequeue

            ' 24xx: MODEL 2420
            ' 7510: MODEL DMM7510
            Me._Model = idItems.Dequeue
            Dim stripCandidate As String = "MODEL "
            If Me._Model.StartsWith(stripCandidate, StringComparison.OrdinalIgnoreCase) Then
                Me._Model = Me.Model.Substring(stripCandidate.Length)
            End If

            ' Serial Number: 0669977
            Me._SerialNumber = idItems.Dequeue

            ' 2002: C11 Oct 10 1997 09:51:36/A02 /D/B/E
            ' 7510: 1.0.0i
            Me._FirmwareRevision = idItems.Dequeue

            ' parse thee firmware revision
            Me.ParseFirmwareRevision(Me._FirmwareRevision)
        End If

    End Sub

#End Region

End Class

''' <summary> Enumerates the instrument board types as defined by the instrument identity. </summary>
Public Enum FirmwareRevisionElement
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Analog")> Analog
    <ComponentModel.Description("Digital")> Digital
    <ComponentModel.Description("Display")> Display
    <ComponentModel.Description("Contact Check")> ContactCheck
    <ComponentModel.Description("LED Display")> LedDisplay
    <ComponentModel.Description("Primary")> Primary
    <ComponentModel.Description("Secondary")> Secondary
    <ComponentModel.Description("Ternary")> Ternary
    <ComponentModel.Description("Quaternary")> Quaternary
    <ComponentModel.Description("Mainframe")> Mainframe
    <ComponentModel.Description("Boot code")> BootCode
    <ComponentModel.Description("Front panel")> FrontPanel
    <ComponentModel.Description("Internal Meter")> InternalMeter
End Enum
