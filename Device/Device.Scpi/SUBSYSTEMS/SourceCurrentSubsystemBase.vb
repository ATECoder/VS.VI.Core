
''' <summary> Defines the contract that must be implemented by a SCPI Source Current Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SourceCurrentSubsystemBase
    Inherits VI.SourceCurrentSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceCurrentSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " LEVEL "

    ''' <summary> Queries the current level. </summary>
    ''' <returns> The current level or none if unknown. </returns>
    Public Overrides Function QueryLevel() As Double?
        Me.Level = Me.Session.Query(Me.Level.GetValueOrDefault(0), ":SOURCE:CURR?")
        Return Me.Level
    End Function

    ''' <summary> Writes the source current level without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output current level. The value is in Amperes. The
    ''' immediate level is the output current setting. At *RST, the current values = 0. </remarks>
    ''' <param name="value"> The current level. </param>
    ''' <returns> The Source Current Level. </returns>
    Public Overrides Function WriteLevel(ByVal value As Double) As Double?
        Me.Session.WriteLine(":SOURCE:CURR {0}", value)
        Me.Level = value
        Return Me.Level
    End Function

#End Region

End Class
