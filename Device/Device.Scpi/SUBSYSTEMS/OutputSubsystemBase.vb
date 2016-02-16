Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Output Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class OutputSubsystemBase
    Inherits VI.OutputSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="OutputSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " OFF MODE "

    ''' <summary> Queries the output Off Mode. Also sets the <see cref="OffMode"></see> cached value. </summary>
    ''' <returns> The output off mode or null if unknown. </returns>
    Public Overrides Function QueryOffMode() As OutputOffMode?
        Dim mode As String = Me.OffMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryTrimEnd(":OUTP:SMOD?")
        If String.IsNullOrWhiteSpace(mode) Then
            Me.OffMode = New OutputOffMode?
        Else
            Dim se As New StringEnumerator(Of OutputOffMode)
            Me.OffMode = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.OffMode
    End Function

    ''' <summary> Writes the output off mode. Does not read back from the instrument. </summary>
    ''' <param name="value"> The off mode. </param>
    ''' <returns> The output off mode or null if unknown. </returns>
    Public Overrides Function WriteOffMode(ByVal value As OutputOffMode) As OutputOffMode?
        Me.Session.WriteLine(":OUTP:SMOD {0}", value.ExtractBetween())
        Me.OffMode = value
        Return Me.OffMode
    End Function

#End Region

#Region " ON/OFF STATE "

    ''' <summary> Queries the output on/off state. Also sets the <see cref="OutputOnState">output
    ''' on</see> sentinel. </summary>
    ''' <returns> <c>True</c> if on; otherwise <c>False</c>. </returns>
    Public Overrides Function QueryOutputOnState() As Boolean?
        Me.OutputOnState = Me.Session.Query(Me.OutputOnState.GetValueOrDefault(True), ":OUTP:STAT?")
        Return Me.OutputOnState
    End Function

    ''' <summary> Writes the output on/off state. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> [is on]. </param>
    ''' <returns> <c>True</c> if on; otherwise <c>False</c>. </returns>
    Public Overrides Function WriteOutputOnState(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine(":OUTP:STAT {0:'1';'1';'0'}", CType(value, Integer))
        Me.OutputOnState = value
        Return Me.OutputOnState
    End Function

#End Region

End Class
