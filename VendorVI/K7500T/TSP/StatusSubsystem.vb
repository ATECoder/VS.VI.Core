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
    Inherits VI.Tsp2.StatusSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

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
    <ComponentModel.Description("Reading Overflow")> ReadingOverflow = 1
    <ComponentModel.Description("Low Limit 1 Failed")> LowLimit1Failed = 2
    <ComponentModel.Description("High Limit 1 Failed")> HighLimit1Failed = 4
    <ComponentModel.Description("Low Limit 2 Failed")> LowLimit2Failed = 8
    <ComponentModel.Description("High Limit 2 Failed")> HighLimit2Failed = 16
    <ComponentModel.Description("Reading Available")> ReadingAvailable = 32
    <ComponentModel.Description("Not Used 1")> NotUsed1 = 64
    <ComponentModel.Description("Buffer Available")> BufferAvailable = 128
    <ComponentModel.Description("Buffer Half Full")> BufferHalfFull = 256
    <ComponentModel.Description("Buffer Full")> BufferFull = 512
    <ComponentModel.Description("Buffer Overflow")> BufferOverflow = 1024
    <ComponentModel.Description("HardwareLimit Event")> HardwareLimitEvent = 2048
    <ComponentModel.Description("Buffer Quarter Full")> BufferQuarterFull = 4096
    <ComponentModel.Description("Buffer Three-Quarters Full")> BufferThreeQuartersFull = 8192
    <ComponentModel.Description("Master Limit")> MasterLimit = 16384
    <ComponentModel.Description("Not Used 2")> NotUsed2 = 32768
    <ComponentModel.Description("All")> All = 32767
End Enum

