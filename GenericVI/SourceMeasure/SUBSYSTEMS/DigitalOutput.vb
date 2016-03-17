''' <summary> Defines the contract that must be implemented by a SCPI Digital Output Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class DigitalOutput
    Inherits VI.Scpi.DigitalOutputBase

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

    ''' <summary> Gets or sets the digital output clear command. </summary>
    ''' <value> The digital output clear command. </value>
    ''' <remarks> SCPI: ":SOUR2:CLE" </remarks>
    Protected Overrides ReadOnly Property ClearCommand As String = ":SOUR2:CLE"

    ''' <summary> Gets the digital output Auto Clear enabled query command. </summary>
    ''' <value> The digital output Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR2:CLE:AUTO?" </remarks>
    Protected Overrides ReadOnly Property AutoClearEnabledQueryCommand As String = ":SOUR2:CLE:AUTO?"

    ''' <summary> Gets the digital output Auto Clear enabled command Format. </summary>
    ''' <value> The digital output Auto Clear enabled query command. </value>
    ''' <remarks> SCPI: ":SOUR2:CLE:AUTO {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overrides ReadOnly Property AutoClearEnabledCommandFormat As String = ":SOUR2:CLE:AUTO {0:'ON';'ON';'OFF'}"

    ''' <summary> Gets or sets the delay (pulse width) range. </summary>
    ''' <value> The delay range. </value>
    Public Overrides ReadOnly Property DelayRange As isr.Core.Pith.Range(Of TimeSpan) = New isr.Core.Pith.Range(Of TimeSpan)(TimeSpan.Zero, TimeSpan.FromSeconds(60))

    ''' <summary> Gets or sets the digital output Delay query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property DelayQueryCommand As String = ":SOUR2:CLE:AUTO:DEL?"

    ''' <summary> Gets or sets the digital output Delay query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property DelayCommandFormat As String = ":SOUR2:CLE:AUTO:DEL {0}"

    ''' <summary> Gets or sets the digital output Bit Size query command. </summary>
    Protected Overrides ReadOnly Property BitSizeQueryCommand As String = ":SOUR2:BSIZ?"

    ''' <summary> Gets or sets the digital output Bit Size query command. </summary>
    Protected Overrides ReadOnly Property BitSizeCommandFormat As String = ":SOUR2:BSIZ {0}"

    ''' <summary> Gets or sets the digital output Level query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property LevelQueryCommand As String = ":SOUR2:TTL:ACT?"

    ''' <summary> Gets or sets the digital output Level query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    Protected Overrides ReadOnly Property LevelCommandFormat As String = ":SOUR2:TTL:LEV {0}"

    ''' <summary> Gets the Output Mode query command. </summary>
    ''' <value> The Output Mode query command. </value>
    ''' <remarks> SCPI: ":SOUR2:TTL4:MODE?" </remarks>
    Protected Overrides ReadOnly Property OutputModeQueryCommand As String = ":SOUR2:TTL4:MODE?"

    ''' <summary> Gets the Output Mode command format. </summary>
    ''' <value> The Output Mode query command format. </value>
    ''' <remarks> SCPI: ":SOUR2:TTL4:MODE {0}" </remarks>
    Protected Overrides ReadOnly Property OutputModeCommandFormat As String = ":SOUR2:TTL4:MODE {0}"

    ''' <summary> Gets the Output Polarity query command. </summary>
    ''' <value> The Output Polarity query command. </value>
    ''' <remarks> SCPI: "SOUR2:TTL4:BST?" </remarks>
    Protected Overrides ReadOnly Property OutputSignalPolarityQueryCommand As String = "SOUR2:TTL4:BST?"

    ''' <summary> Gets the Output Polarity command format. </summary>
    ''' <value> The Output Polarity command format. </value>
    ''' <remarks> SCPI: "SOUR2:TTL4:BST {0}" </remarks>
    Protected Overrides ReadOnly Property OutputSignalPolarityCommandFormat As String = "SOUR2:TTL4:BST {0}"

#End Region

End Class
