''' <summary> Information about the version of a Keithley 2000 instrument. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class VersionInfo
    Inherits isr.VI.VersionInfo

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Overrides Sub Clear()
        MyBase.Clear()
        Me._FirmwareLevelMajorRevision = ""
        Me._FirmwareLevelMinorRevision = ""
        Me._FirmwareLevelDate = Date.MinValue
        Me._SupportsContactCheck = False
    End Sub

    ''' <summary>Returns the instrument firmware level major revision name .</summary>
    Public ReadOnly Property FirmwareLevelMajorRevision() As String

    ''' <summary> Gets or sets the firmware level minor revision. </summary>
    ''' <value> The firmware level minor revision. </value>
    Public ReadOnly Property FirmwareLevelMinorRevision() As String

    ''' <summary>Returns the instrument firmware level date.</summary>
    Public ReadOnly Property FirmwareLevelDate() As Date

    ''' <summary>Returns True if the instrument supports contact check.</summary>
    Public ReadOnly Property SupportsContactCheck() As Boolean

    ''' <summary> Parses the instrument firmware revision. </summary>
    ''' <remarks>
    ''' Identity: <para>
    ''' <c>KEITHLEY INSTRUMENTS INC.,MODEL 2420,0669977,C11 Oct 10 1997 09:51:36/A02
    ''' /D/B/E.</c>.</para>
    ''' </remarks>
    ''' <exception cref="ArgumentNullException" guarantee="strong"> . </exception>
    ''' <param name="revision"> Specifies the instrument firmware revision with the following items:
    '''                         <see cref="FirmwareLevelMajorRevision">firmware level major
    '''                         revision</see>
    '''                         <see cref="FirmwareLevelDate">date</see>
    '''                         <see cref="FirmwareLevelMinorRevision">firmware level minor
    '''                         revision</see>
    '''                         <see cref="BoardRevisions">board revisions</see>
    '''                         e.g., <c>C11 Oct 10 1997 09:51:36/A02 /D/B/E.</c>.  
    '''                         The last three letters separated by "/" indicate the board revisions
    '''                         (i.e. /Analog/Digital/Contact Check). Contact Check board revisions
    '''                         have the following features:<p>
    '''                         Revisions A through C have only one resistance range.</p><p>
    '''                         Revisions D and above have selectable resistance ranges.</p> </param>
    Protected Overrides Sub ParseFirmwareRevision(ByVal revision As String)

        If revision Is Nothing Then
            Throw New ArgumentNullException(NameOf(revision))
        ElseIf String.IsNullOrWhiteSpace(revision) Then
            MyBase.ParseFirmwareRevision(revision)
        Else
            MyBase.ParseFirmwareRevision(revision)

            ' get the revision sections
            Dim revSections As Queue(Of String) = New Queue(Of String)(revision.Split("/"c))

            ' Rev: C11; revision: Oct 10 1997 09:51:36
            Dim firmwareLevel As String = revSections.Dequeue.Trim
            Me._FirmwareLevelMajorRevision = firmwareLevel.Split(" "c)(0)

            ' date string: Oct 10 1997 09:51:36
            If Not Date.TryParse(firmwareLevel.Substring(Me._FirmwareLevelMajorRevision.Length).Trim,
                             Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.AssumeLocal,
                             Me._FirmwareLevelDate) Then
                Me._FirmwareLevelDate = DateTime.MinValue
            End If

            ' Minor revision: A02
            Me._FirmwareLevelMinorRevision = revSections.Dequeue.Trim

            If revSections.Count > 0 Then Me.BoardRevisions.Add(BoardType.Analog.ToString, revSections.Dequeue.Trim)
            If revSections.Count > 0 Then Me.BoardRevisions.Add(BoardType.Digital.ToString, revSections.Dequeue.Trim)
            If revSections.Count > 0 Then Me.BoardRevisions.Add(BoardType.ContactCheck.ToString, revSections.Dequeue.Trim)
            Me._SupportsContactCheck = Me.BoardRevisions.ContainsKey(BoardType.ContactCheck.ToString)

        End If

    End Sub

End Class

