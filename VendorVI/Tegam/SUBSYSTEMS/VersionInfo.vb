''' <summary> Information about the version of a Tegam 1750 Resistance Measuring System. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/7/2013" by="David" revision=""> Created. </history>
Public Class VersionInfo
    Inherits isr.VI.VersionInfo

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Overrides Sub Clear()
        MyBase.Clear()
    End Sub

    ''' <summary> Parses the instrument firmware revision. </summary>
    ''' <remarks> <c>Tegam 1750 D02.20  Apr 26,2000.</c>. </remarks>
    ''' <exception cref="ArgumentNullException" guarantee="strong"> . </exception>
    ''' <param name="revision"> Specifies the instrument <see cref="FirmwareRevisionElements">board
    '''                         revisions</see>
    '''                         e.g., <c>D02.20</c> for the digital and display boards. </param>
    Protected Overrides Sub ParseFirmwareRevision(ByVal revision As String)

        If revision Is Nothing Then
            Throw New ArgumentNullException(NameOf(revision))
        ElseIf String.IsNullOrWhiteSpace(revision) Then
            MyBase.ParseFirmwareRevision(revision)
        Else
            MyBase.ParseFirmwareRevision(revision)

            ' get the revision sections
            Dim revSections As Queue(Of String) = New Queue(Of String)(revision.Split("."c))

            ' Rev: D02.20
            If revSections.Count > 0 Then Me.FirmwareRevisionElements.Add(FirmwareRevisionElement.Digital.ToString, revSections.Dequeue.Trim)
            If revSections.Count > 0 Then Me.FirmwareRevisionElements.Add(FirmwareRevisionElement.Display.ToString, revSections.Dequeue.Trim)

        End If

    End Sub

    ''' <summary> Parses the instrument identity string. </summary>
    ''' <param name="value"> Specifies the instrument identity string, which includes at a minimum the
    ''' following information:
    ''' <see cref="ManufacturerName">manufacturer</see>, <see cref="Model">model</see>,
    ''' <see cref="SerialNumber">serial number</see>, e.g., <para> 
    '''                          <c>Tegam 1750 D02.20  Apr 26,2000.</c>.</para> </param>
    Public Overrides Sub Parse(value As String)
        Me.Identity = value
        Me.ManufacturerName = "Tegam"
        Me.Model = "1750"
        Me.SerialNumber = "1"
        Me.ParseFirmwareRevision("D02.20")
    End Sub
End Class

