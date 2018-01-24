''' <summary> Defines a SCPI Source Current Subsystem for a generic Source Measure instrument . </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class SourceCurrentSubsystem
    Inherits VI.Scpi.SourceCurrentSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceCurrentSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overrides Sub InitKnownState()
        Dim action As String = ""
        MyBase.InitKnownState()
        Try
            action = $"Setting {NameOf(SourceCurrentSubsystem)} level range"
            Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, $"{action};. ")
            Me.LevelRangeSetter(StatusSubsystem.VersionInfo.Model)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Exception {action}; Ignored;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " AUTO RANGE "

    ''' <summary> Gets the automatic Range enabled command Format. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledCommandFormat As String = ":SOUR:CURR:RANG:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets the automatic Range enabled query command. </summary>
    ''' <value> The automatic Range enabled query command. </value>
    Protected Overrides ReadOnly Property AutoRangeEnabledQueryCommand As String = ":SOUR:CURR:RANG:AUTO?"

#End Region

#Region " RANGE "

    ''' <summary> Gets the range command format. </summary>
    ''' <value> The range command format. </value>
    Protected Overrides ReadOnly Property RangeCommandFormat As String = ":SOUR:CURR:RANG {0}"

    ''' <summary> Gets the range query command. </summary>
    ''' <value> The range query command. </value>
    Protected Overrides ReadOnly Property RangeQueryCommand As String = ":SOUR:CURR:RANG?"

    ''' <summary> Sets the level range for the instrument model. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub LevelRangeSetter(ByVal model As String)
        Select Case True
            Case model.StartsWith("2400", StringComparison.OrdinalIgnoreCase)
                Me.LevelRange = New Core.Pith.RangeR(-1.05, +1.05)
            Case model.StartsWith("2410", StringComparison.OrdinalIgnoreCase)
                Me.LevelRange = New Core.Pith.RangeR(-1.05, +1.05)
            Case model.StartsWith("242", StringComparison.OrdinalIgnoreCase)
                Me.LevelRange = New Core.Pith.RangeR(-3.15, +3.05)
            Case model.StartsWith("243", StringComparison.OrdinalIgnoreCase)
                Me.LevelRange = New Core.Pith.RangeR(-3.15, +3.15)
            Case model.StartsWith("244", StringComparison.OrdinalIgnoreCase)
                Me.LevelRange = New Core.Pith.RangeR(-5.25, +5.25)
            Case Else
                Me.LevelRange = New Core.Pith.RangeR(-1.05, +1.05)
        End Select
    End Sub

#End Region

#End Region

End Class
