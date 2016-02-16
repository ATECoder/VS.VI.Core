''' <summary> Information about the version. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="9/21/2013" by="David" revision=""> Created. </history>
Public Class VersionInfo
    Inherits isr.VI.VersionInfo

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Overrides Sub Clear()
        MyBase.Clear()
        Me.ParseFirmwareRevision("")
    End Sub

    ''' <summary> Parses the instrument identity string. </summary>
    ''' <param name="value"> Specifies the instrument identity string, which includes at a minimum the
    ''' following information: e.g., <para> <c>2001X.CD.249799-011</c>.</para><para>
    ''' CD is revision letters,</para><para>
    ''' 249799 is software part number</para><para>
    ''' 001 is EG standard PROM-base software</para><para>
    ''' </para>
    ''' <see cref="ManufacturerName">manufacturer</see>, <see cref="Model">model</see>,
    ''' <see cref="SerialNumber">serial number</see>,  </param>
    Public Overrides Sub Parse(value As String)

        ' clear
        MyBase.Parse("")

        If String.IsNullOrWhiteSpace(value) Then

            Me.Identity = value

        Else

            ' save the identity.
            Me.Identity = value

            Me.ManufacturerName = "Electroglass"

            ' Parse the id to get the revision number
            Dim idItems() As String = value.Split("."c)

            ' model: 2001X
            Me.Model = idItems(0)

            ' firmware: CD
            Me.FirmwareRevision = idItems(1)

            ' Serial Number: 249799-011
            Me.SerialNumber = idItems(2)

            ' parse thee firmware revision
            Me.ParseFirmwareRevision("")

        End If

    End Sub

End Class
