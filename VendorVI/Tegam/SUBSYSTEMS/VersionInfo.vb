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
    '''   e.g., <c>D02.20</c> for the digital and display boards.
    '''   </param>
    ''' <exception cref="ArgumentNullException" guarantee="strong"></exception>
    Protected Overrides Sub ParseFirmwareRevision(ByVal revision As String)

        If revision Is Nothing Then
            Throw New ArgumentNullException("revision")
        ElseIf String.IsNullOrWhiteSpace(revision) Then
            Me._boardRevisions = New System.Collections.Specialized.StringDictionary
        Else
            ' get the revision sections
            Dim revisionSections() As String = revision.Split("."c)

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

''' <summary>Boards included in the instrument.</summary>
Public Enum BoardType
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Digital")> Digital = 1
    <ComponentModel.Description("Display")> Display = 2
End Enum
