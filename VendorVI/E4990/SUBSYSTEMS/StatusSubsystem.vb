''' <summary> Defines a SCPI Status Subsystem for a generic Switch. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/10/2013" by="David" revision="3.0.5001"> Created. </history>
Public Class StatusSubsystem
    Inherits VI.Scpi.StatusSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystem" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.SessionBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal visaSession As VI.SessionBase)
        MyBase.New(visaSession)
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

#Region " SYNTAX "

#Region " EXECUTION STATE "

    ''' <summary> Gets the preset command. </summary>
    ''' <value> The preset command. </value>
    Protected Overrides ReadOnly Property PresetCommand As String = VI.Scpi.Syntax.ScpiSyntax.StatusPresetCommand

#End Region

#Region " ERROR CLEAR = *CLS "

    ''' <summary> Gets the clear error queue command. </summary>
    ''' <value> The clear error queue command. </value>
    ''' <remarks> K7001 does not support this commend == *CLS clears the queue. </remarks>
    Protected Overrides ReadOnly Property ClearErrorQueueCommand As String = Ieee488.Syntax.ClearExecutionStateCommand

#End Region

#Region " LINE FREQUENCY -- NOT SUPPORTED "

    ''' <summary> Gets line frequency query command. </summary>
    ''' <value> The line frequency query command. </value>
    ''' <remarks>  K7001 does not support this command.</remarks>
    Protected Overrides ReadOnly Property LineFrequencyQueryCommand As String = ""

#End Region

#End Region

#Region " IDENTITY "

    ''' <summary> Parse version information. </summary>
    ''' <param name="value"> The value. </param>
    Protected Overrides Sub ParseVersionInfo(value As String)
        MyBase.ParseVersionInfo(value)
        Me._VersionInfo = New VersionInfo
        Me.VersionInfo.Parse(value)
    End Sub

    ''' <summary> Gets or sets the information describing the version. </summary>
    ''' <value> Information describing the version. </value>
    Public Overloads ReadOnly Property VersionInfo As VersionInfo

#End Region

#Region " MEASUREMENET AVAILABLE "

    ''' <summary> Enables the measurement available. </summary>
    Public Sub EnableMeasurementAvailable()

        ' Sets the positive transition filter to 0 and the negative transition filter to 1 
        ' so that the operation status event register at bit 4 is set to 1 only when the
        ' operation status condition register at bit 4 is changed from 1 to 0.
        ' Me.Device.Session.Write(":STAT:OPER:PTR 0")
        Me.ApplyOperationPositiveTransitionEventEnableBitmask(0)
        ' Me.Device.Session.Write(":STAT:OPER:NTR 16")
        Me.ApplyOperationNegativeTransitionEventEnableBitmask(16)

        ' Enables bit 4 in the operation status event register and bit 8 in the status byte register.
        ' Me.Device.Session.Write(":STAT:OPER:ENAB 16")
        Me.ApplyOperationEventEnableBitmask(16)
        ' Me.Device.Session.Write("*SRE 128")
        Me.EnableServiceRequest(ServiceRequests.OperationEvent) ' 128

    End Sub

#End Region

End Class
