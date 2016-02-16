Imports isr.Core.Pith.EnumExtensions
Imports System.ComponentModel
''' <summary> Information about the Auxiliary Status. </summary>
''' <license> (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="1/09/2015" by="David" revision=""> Created. </history>
Public Class AuxiliaryStatusInfo

#Region " CONSTRUCTORS "

    ''' <summary> Constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me._ClearKnownState()
    End Sub

    ''' <summary> Constructor. </summary>
    Public Sub New(ByVal auxiliaryStatus As AuxiliaryStatuses)
        Me.New()
        Me._AuxiliaryStatus = auxiliaryStatus
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub New(ByVal value As AuxiliaryStatusInfo)
        Me.New()
        If value IsNot Nothing Then
            Me._AuxiliaryStatus = value.AuxiliaryStatus
        End If
    End Sub

    ''' <summary> Clears to known (clear) state; Clears select values to their initial state. </summary>
    Private Sub _ClearKnownState()
        Me._AuxiliaryStatus = AuxiliaryStatuses.None
    End Sub

    ''' <summary> Clears to known (clear) state; Clears select values to their initial state. </summary>
    Public Sub ClearKnownState()
        Me._ClearKnownState()
    End Sub

#End Region

#Region " VALUE "

    Private _AuxiliaryStatus As AuxiliaryStatuses

    ''' <summary> Gets the auxiliary Status. </summary>
    ''' <value> The auxiliary Status. </value>
    Public ReadOnly Property AuxiliaryStatus As AuxiliaryStatuses
        Get
            Return Me._AuxiliaryStatus
        End Get
    End Property

#End Region

#Region " BIT VALUES "

    ''' <summary> Query if the Bits of the <paramref name="bits">Auxiliary Statuses</paramref> are on. </summary>
    ''' <param name="value"> The value. </param>
    ''' <param name="bits">  The bits. </param>
    ''' <returns> <c>true</c> if Bin; otherwise <c>false</c> </returns>
    Public Shared Function IsBit(ByVal value As AuxiliaryStatuses, ByVal bits As AuxiliaryStatuses) As Boolean
        Return (value And bits) <> 0
    End Function

    ''' <summary> Query if the Bit of the <paramref name="bit">Auxiliary Status</paramref> is on. </summary>
    ''' <param name="bit"> The bit. </param>
    ''' <returns> <c>true</c> if Bin; otherwise <c>false</c> </returns>
    Public Function IsBit(ByVal bit As AuxiliaryStatusBit) As Boolean
        Return Me.IsBit(CType(CInt(2 ^ CInt(bit)), AuxiliaryStatuses))
    End Function

    ''' <summary> Query if the Bits of the <paramref name="bits">Auxiliary Statuses</paramref> are on. </summary>
    ''' <param name="bits"> The bits. </param>
    ''' <returns> <c>true</c> if Bin; otherwise <c>false</c> </returns>
    Public Function IsBit(ByVal bits As AuxiliaryStatuses) As Boolean
        Return (Me.AuxiliaryStatus And bits) <> 0
    End Function

    Public Function IsDutControl() As Boolean
        Return Me.IsBit(AuxiliaryStatuses.DutControlMode)
    End Function

    Public Function IsHeatOnlyMode() As Boolean
        Return Me.IsBit(AuxiliaryStatuses.HeatOnlyMode)
    End Function

    Public Function IsReady() As Boolean
        Return Me.IsBit(AuxiliaryStatuses.ReadyStartup)
    End Function

    Public Function IsManualMode() As Boolean
        Return Me.IsBit(AuxiliaryStatuses.ManualProgram)
    End Function

    Public Function IsRampMode() As Boolean
        Return Me.IsBit(AuxiliaryStatuses.RampMode)
    End Function

    Public Function IsFlowOn() As Boolean
        Return Me.IsBit(AuxiliaryStatuses.FlowOnOff)
    End Function

    Public Function IsHeadUp() As Boolean
        Return Me.IsBit(AuxiliaryStatuses.HeadUpDown)
    End Function

    ''' <summary> Gets the description. </summary>
    ''' <value> The description. </value>
    Public ReadOnly Property Description As String
        Get
            Return Me.AuxiliaryStatus.Description
        End Get
    End Property

    ''' <summary> Query if any <see cref="AuxiliaryStatus">bits</see> are. </summary>
    ''' <param name="included"> The included. </param>
    ''' <returns> <c>true</c> if included; otherwise <c>false</c> </returns>
    Public Function IsIncluded(ByVal included As AuxiliaryStatuses) As Boolean
        Return AuxiliaryStatusInfo.IsIncluded(Me.AuxiliaryStatus, included)
    End Function

#End Region

#Region " HELPERS "

    ''' <summary> Builds Auxiliary Statuses. </summary>
    ''' <param name="value">    The value. </param>
    ''' <param name="excluded"> The excluded. </param>
    ''' <param name="included"> The included. </param>
    ''' <returns> The AuxiliaryStatuses. </returns>
    Public Shared Function BuildAuxiliaryStatuses(ByVal value As AuxiliaryStatuses, ByVal excluded As AuxiliaryStatuses, ByVal included As AuxiliaryStatuses) As AuxiliaryStatuses
        Return (value Or included) And Not excluded
    End Function

    ''' <summary> Query if 'value' is included. </summary>
    ''' <param name="value">    The value. </param>
    ''' <param name="included"> The included. </param>
    ''' <returns> <c>true</c> if included; otherwise <c>false</c> </returns>
    Public Shared Function IsIncluded(ByVal value As AuxiliaryStatuses, ByVal included As AuxiliaryStatuses) As Boolean
        Return (value And included) <> AuxiliaryStatuses.None
    End Function

#End Region

End Class

''' <summary> Values that represent the auxiliary Status bit. </summary>
Public Enum AuxiliaryStatusBit
    <Description("Not defined")> None = 0
    <Description("Head Up (1) Down (0)")> HeadUpDown = 2
    <Description("Heat Only (1) Compressor on (0)")> HeatOnlyMode = 3
    <Description("Dut-control (1) or air-control (0) Mode")> DutControlMode = 4
    <Description("Flow on (1) or off (0)")> FlowOnOff = 5
    <Description("Ready (1) Startup (0)")> ReadyStartup = 6
    <Description("Short Failure")> ManualProgram = 8
    <Description("Ramp Mode")> RampMode = 9
End Enum

''' <summary> A bit field of flags for specifying combination of Auxiliary Statuses. </summary>
<Flags()>
Public Enum AuxiliaryStatuses
    <Description("None")> None = 0
    <Description("Head Up (1) Down (0)")> HeadUpDown = CInt(2 ^ CInt(AuxiliaryStatusBit.HeadUpDown))
    <Description("Heat Only (1) Compressor on (0)")> HeatOnlyMode = CInt(2 ^ CInt(AuxiliaryStatusBit.HeatOnlyMode))
    <Description("Dut-control (1) or air-control (0) Mode")> DutControlMode = CInt(2 ^ CInt(AuxiliaryStatusBit.DutControlMode))
    <Description("Flow on (1) or off (0)")> FlowOnOff = CInt(2 ^ CInt(AuxiliaryStatusBit.FlowOnOff))
    <Description("Ready (1) Startup (0)")> ReadyStartup = CInt(2 ^ CInt(AuxiliaryStatusBit.ReadyStartup))
    <Description("Manual (1) Program (0)")> ManualProgram = CInt(2 ^ CInt(AuxiliaryStatusBit.ManualProgram))
    <Description("Ramp Mode")> RampMode = CInt(2 ^ CInt(AuxiliaryStatusBit.RampMode))
End Enum
