''' <summary> Defines the contract that must be implemented by a SCPI Composite Limit Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class CompositeLimit
    Inherits VI.Scpi.CompositeLimitBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="CompositeLimit" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Public Sub New(ByVal statusSubsystem As isr.VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.AsyncNotifyPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " SYNTAX "

    ''' <summary> Gets or sets the composite limits clear command. </summary>
    ''' <value> The composite limits clear command. </value>
    ''' <remarks> SCPI: ":CLAC2:CLIM:CLE" </remarks>
    Protected Overrides ReadOnly Property ClearCommand As String = ":CLAC2:CLIM:CLE"

    ''' <summary> Gets the Composite Limits Auto Clear enabled query command. </summary>
    ''' <value> The Composite Limits Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:CLE:AUTO?" </remarks>
    Protected Overrides ReadOnly Property AutoClearEnabledQueryCommand As String = ":CALC2:CLIM:CLE:AUTO?"

    ''' <summary> Gets the Composite Limits Auto Clear enabled command Format. </summary>
    ''' <value> The Composite Limits Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:CLE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overrides ReadOnly Property AutoClearEnabledCommandFormat As String = ":CALC2:CLIM:CLE:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets or sets the Composite Limits failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property FailureBitsQueryCommand As String = ":CALC2:CLIM:FAIL:SOUR2?"

    ''' <summary> Gets or sets the Composite Limits Failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property FailureBitsCommandFormat As String = ":CALC2:CLIM:FAIL:SOUR2 {0}"

    ''' <summary> Gets or sets the Composite Limits Pass Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property PassBitsQueryCommand As String = ":CALC2:CLIM:PASS:SOUR2?"

    ''' <summary> Gets or sets the Composite Limits Pass Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property PassBitsCommandFormat As String = ":CALC2:CLIM:PASS:SOUR2 {0}"

    ''' <summary> Gets the Binning Control query command. </summary>
    ''' <value> The Binning Control query command. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:BCON?" </remarks>
    Protected Overrides ReadOnly Property BinningControlQueryCommand As String = ":CALC2:CLIM:BCON?"

    ''' <summary> Gets the Binning Control command format. </summary>
    ''' <value> The Binning Control query command format. </value>
    ''' <remarks> SCPI: ":CALC2:CLIM:BCON {0}" </remarks>
    Protected Overrides ReadOnly Property BinningControlCommandFormat As String = ":CALC2:CLIM:BCON {0}"

    ''' <summary> Gets the Limit Mode query command. </summary>
    ''' <value> The Limit Mode query command. </value>
    ''' <remarks> SCPI: "CALC2:CLIM:MODE?" </remarks>
    Protected Overrides ReadOnly Property LimitModeQueryCommand As String = "CALC2:CLIM:MODE?"

    ''' <summary> Gets the Limit Mode command format. </summary>
    ''' <value> The Limit Mode command format. </value>
    ''' <remarks> SCPI: "CALC2:CLIM:MODE {0}" </remarks>
    Protected Overrides ReadOnly Property LimitModeCommandFormat As String = "CALC2:CLIM:MODE {0}"

#End Region

End Class
