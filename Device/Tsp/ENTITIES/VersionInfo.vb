Imports System.Linq
''' <summary> Information about the version of a TSP instrument. </summary>
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
        Me._Clear()
    End Sub

    ''' <summary> Clears this object to its blank/initial state. </summary>
    ''' <remarks> David, 3/12/2016. </remarks>
    Private Sub _Clear()
        Me._FirmwareVersion = New System.Version
    End Sub

    ''' <summary> Clears this object to its blank/initial state. </summary>
    Public Overrides Sub Clear()
        MyBase.Clear()
        Me._Clear()
    End Sub

    ''' <summary> Gets or sets the firmware version. </summary>
    ''' <value> The firmware version. </value>
    Public Property FirmwareVersion As System.Version

    Private Shared Function GetNumbers(input As String) As String
        Return New String(input.Where(Function(c) Char.IsDigit(c)).ToArray())
    End Function

    Private Shared Function GetNotNumbers(input As String) As String
        Return New String(input.Where(Function(c) Not Char.IsDigit(c)).ToArray())
    End Function

    ''' <summary> Parses the instrument firmware revision. </summary>
    ''' <exception cref="ArgumentNullException" guarantee="strong"> . </exception>
    ''' <param name="revision"> Specifies the instrument revision
    ''' e.g., <c>2.1.6</c>. The source meter identity includes no board specs. </param>
    Protected Overrides Sub ParseFirmwareRevision(ByVal revision As String)
        If revision Is Nothing Then
            Throw New ArgumentNullException(NameOf(revision))
        ElseIf String.IsNullOrWhiteSpace(revision) Then
            MyBase.ParseFirmwareRevision(revision)
            Me.FirmwareVersion = New System.Version
        Else
            MyBase.ParseFirmwareRevision(revision)
            Dim rev As New System.Version
            If Not System.Version.TryParse(revision, rev) Then
                Dim tempRev As New System.Version
                ' 3700 revision is 1.53c.
                Dim values As String() = revision.Split("."c)
                Dim builder As New System.Text.StringBuilder
                For Each v As String In values
                    Dim iv As Integer = 0
                    Dim vv As String = GetNumbers(v)
                    If Integer.TryParse(vv, iv) Then
                        If builder.Length > 0 Then builder.Append(".")
                        builder.Append(vv)
                    End If
                    If System.Version.TryParse(builder.ToString, tempRev) Then
                        rev = System.Version.Parse(builder.ToString)
                    End If

                    vv = GetNotNumbers(v)
                    If Not String.IsNullOrWhiteSpace(vv) Then
                        Dim value As Integer = 0
                        For Each c As Char In vv.ToUpperInvariant.ToCharArray
                            value = Convert.ToInt16(c) - Convert.ToInt16("A"c) + 1
                            If builder.Length > 0 Then builder.Append(".")
                            builder.Append(CStr(value))
                            If System.Version.TryParse(builder.ToString, tempRev) Then
                                rev = System.Version.Parse(builder.ToString)
                            End If
                        Next
                    End If
                Next
            End If
            Me.FirmwareVersion = rev
        End If
    End Sub

End Class

