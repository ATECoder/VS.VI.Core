''' <summary> Information about the version of a Keithley 2700 instrument. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class VersionInfo
    Inherits isr.VI.VersionInfo

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me._boardRevisions = New System.Collections.Specialized.StringDictionary
    End Sub

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Overrides Sub Clear()
        MyBase.Clear()
        Me._boardRevisions = New System.Collections.Specialized.StringDictionary
        Me.ParseFirmwareRevision("")
    End Sub

    Private _boardRevisions As System.Collections.Specialized.StringDictionary
    ''' <summary>Returns the list of board revisions.</summary>
    Public ReadOnly Property BoardRevisions() As System.Collections.Specialized.StringDictionary
        Get
            Return Me._boardRevisions
        End Get
    End Property

    ''' <summary>Parses the instrument firmware revision.</summary>
    ''' <param name="revision">Specifies the instrument <see cref="BoardRevisions">board revisions</see>
    '''   e.g., <c>yyyyy/zzz</c> for the digital and display boards.
    '''   </param>
    ''' <exception cref="ArgumentNullException" guarantee="strong"></exception>
    Protected Overrides Sub ParseFirmwareRevision(ByVal revision As String)

        If revision Is Nothing Then
            Throw New ArgumentNullException("revision")
        ElseIf String.IsNullOrWhiteSpace(revision) Then
            Me._boardRevisions = New System.Collections.Specialized.StringDictionary
        Else
            ' get the revision sections
            Dim revisionSections() As String = revision.Split("/"c)

            ' set board revisions collection
            Me._boardRevisions = New System.Collections.Specialized.StringDictionary

            ' Rev: yyyyy/ZZZ
            If revisionSections.Length > 0 Then
                Me._boardRevisions.Add(BoardType.Digital.ToString, revisionSections(0).Trim)
                If revisionSections.Length > 1 Then
                    Me._boardRevisions.Add(BoardType.Display.ToString, revisionSections(1).Trim)
                End If
            End If

        End If

    End Sub

End Class

''' <summary>Boards included in the instrument.</summary>
Public Enum BoardType
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Digital")> Digital = 1
    <ComponentModel.Description("Display")> Display = 2
End Enum
