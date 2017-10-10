Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines a Measure Subsystem for a Keysight 1750 Resistance Measuring System. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="10/7/2013" by="David" revision=""> Created. </history>
Public Class MeasureSubsystem
    Inherits VI.R2D2.MeasureSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="MeasureSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary>
    ''' Sets subsystem values to their known execution clear state.
    ''' </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.LastReading = ""
    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Clears last reading.
    '''           </para></remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.LastReading = ""
    End Sub

    ''' <summary>
    ''' Sets the subsystem values to their known execution reset state.
    ''' </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
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

#Region "  INIT, READ, FETCH "

    ''' <summary> Gets the fetch command. </summary>
    ''' <value> The fetch command. </value>
    <Obsolete("Not supported")>
    Protected Overrides ReadOnly Property FetchCommand As String = ""

    ''' <summary> Gets the read command. </summary>
    ''' <value> The read command. </value>
    <Obsolete("Not supported")>
    Protected Overrides ReadOnly Property ReadCommand As String = ""

#End Region

#End Region

#Region " FETCH "

    ''' <summary> Fetches the data. </summary>
    ''' <remarks> the K3458 meter does not require issuing a read command. </remarks>
    Public Overrides Sub Fetch()
        Me.Read()
    End Sub

    ''' <summary> Fetches the data. </summary>
    ''' <remarks> the K3458 meter does not require issuing a read command. </remarks>
    Public Overrides Sub Read()
        Me.Read(False)
    End Sub

    ''' <summary> Fetches the data. </summary>
    ''' <param name="syncNotifyMeasurementAvailable"> The synchronization notify measurement available
    ''' to read. </param>
    Public Overloads Sub Read(ByVal syncNotifyMeasurementAvailable As Boolean)
        Me.LastReading = Me.Session.ReadLineTrimEnd
        If Not String.IsNullOrWhiteSpace(Me.LastReading) Then
            ' the emulator will set the last reading. 
            Me.ParseReading(Me.LastReading)
            If syncNotifyMeasurementAvailable Then
                Me.MeasurementAvailable = True
            Else
                Me.AsyncMeasurementAvailable(True)
            End If
        End If
    End Sub

    Public Overrides Sub ParseReading(reading As String)
        Throw New NotImplementedException()
    End Sub

#End Region

End Class
