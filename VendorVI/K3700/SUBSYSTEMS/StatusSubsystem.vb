Imports isr.VI
Imports isr.VI.Tsp
''' <summary> Status subsystem. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="12/14/2013" by="David" revision=""> Created. </history>
Public Class StatusSubsystem
    Inherits isr.VI.Tsp.StatusSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="StatusSubsystem" /> class. </summary>
    ''' <param name="session"> The session. </param>
    Public Sub New(ByVal session As VI.Pith.SessionBase)
        MyBase.New(session)
    End Sub

    ''' <summary> Creates a new StatusSubsystem. </summary>
    ''' <returns> A StatusSubsystem. </returns>
    Public Shared Function Create() As StatusSubsystem
        Dim subsystem As StatusSubsystem = Nothing
        Try
            subsystem = New StatusSubsystem(isr.VI.SessionFactory.Get.Factory.CreateSession())
        Catch
            If subsystem IsNot Nothing Then
                subsystem.Dispose()
                subsystem = Nothing
            End If
            Throw
        End Try
        Return subsystem
    End Function

#End Region

#Region " I PRESETTABLE "

#If False Then
    ''' <summary> Clears the active state. Issues selective device clear. </summary>
    Public Overrides Sub ClearActiveState()
        ' A delay is required before issuing a device clear.
        ' First a 1ms delay was added. Without the delay, the instrument had error -286 TSP Runtime Error and User Abort error 
        ' if the clear command was issued after turning off the status _G.status.request_enable=0
        ' Thereafter, the instrument has a resource not found error when trying to connect after failing to connect 
        ' because instruments were off. Stopping here in debug mode seems to have alleviated this issue.  So,
        ' the delay was increased to 10 ms.
        Threading.Thread.Sleep(10)
        MyBase.ClearActiveState()
        Me.QueryOperationCompleted()
    End Sub
#End If

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

End Class

''' <summary>Gets or sets the status byte flags of the measurement event register.</summary>
<System.Flags()>
Public Enum MeasurementEvents
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Lower Limit 1 Failed")> LowerLimit1Failed = 1
    <ComponentModel.Description("Upper Limit 1 Failed")> UpperLimit1Failed = 2
    <ComponentModel.Description("Lower Limit 2 Failed")> LowerLimit2Failed = 4
    <ComponentModel.Description("Upper Limit 2 Failed")> UpperLimit2Failed = 8
    <ComponentModel.Description("Bit4: Unused")> Bit4 = 16
    <ComponentModel.Description("Bit5: Unused")> Bit5 = 32
    <ComponentModel.Description("Bit6: Unused")> Bit6 = 64
    <ComponentModel.Description("Reading Overflow")> ReadingOverflow = 128
    <ComponentModel.Description("Buffer Available")> BufferAvailable = 256
    <ComponentModel.Description("All")> All = 512 - Bit4 - Bit5 - Bit6
End Enum
