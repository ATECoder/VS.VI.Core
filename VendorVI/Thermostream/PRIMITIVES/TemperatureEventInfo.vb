Imports isr.Core.Pith.EnumExtensions
Imports System.ComponentModel
''' <summary> Information about the Temperature Event. </summary>
''' <license> (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="1/09/2015" by="David" revision=""> Created. </history>
Public Class TemperatureEventInfo

#Region " CONSTRUCTORS "

    ''' <summary> Constructor. </summary>
    Public Sub New()
        MyBase.New()
        Me._ClearKnownState()
    End Sub

    ''' <summary> Constructor. </summary>
    Public Sub New(ByVal temperatureEvent As TemperatureEvents)
        Me.New()
        Me._TemperatureEvent = temperatureEvent
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub New(ByVal value As TemperatureEventInfo)
        Me.New()
        If value IsNot Nothing Then
            Me._TemperatureEvent = value.TemperatureEvent
        End If
    End Sub

    ''' <summary> Clears to known (clear) state; Clears select values to their initial state. </summary>
    Private Sub _ClearKnownState()
        Me._TemperatureEvent = TemperatureEvents.None
    End Sub

    ''' <summary> Clears to known (clear) state; Clears select values to their initial state. </summary>
    Public Sub ClearKnownState()
        Me._ClearKnownState()
    End Sub

#End Region

#Region " VALUE "

    Private _TemperatureEvent As TemperatureEvents

    ''' <summary> Gets the temperature event. </summary>
    ''' <value> The temperature event. </value>
    Public ReadOnly Property TemperatureEvent As TemperatureEvents
        Get
            Return Me._TemperatureEvent
        End Get
    End Property

#End Region

#Region " BIT VALUES "

    ''' <summary> Query if the Bits of the <paramref name="bits">Temperature Events</paramref> are on. </summary>
    ''' <param name="value"> The value. </param>
    ''' <param name="bits">  The bits. </param>
    ''' <returns> <c>true</c> if Bin; otherwise <c>false</c> </returns>
    Public Shared Function IsBit(ByVal value As TemperatureEvents, ByVal bits As TemperatureEvents) As Boolean
        Return (value And bits) <> 0
    End Function

    ''' <summary> Query if the Bit of the <paramref name="bit">Temperature Event</paramref> is on. </summary>
    ''' <param name="bit"> The bit. </param>
    ''' <returns> <c>true</c> if Bin; otherwise <c>false</c> </returns>
    Public Function IsBit(ByVal bit As TemperatureEventBit) As Boolean
        Return Me.IsBit(CType(CInt(2 ^ CInt(bit)), TemperatureEvents))
    End Function

    ''' <summary> Query if the Bits of the <paramref name="bits">Temperature Events</paramref> are on. </summary>
    ''' <param name="bits"> The bits. </param>
    ''' <returns> <c>true</c> if Bin; otherwise <c>false</c> </returns>
    Public Function IsBit(ByVal bits As TemperatureEvents) As Boolean
        Return (Me.TemperatureEvent And bits) <> 0
    End Function

    ''' <summary> Query if this object is end of cycles. </summary>
    ''' <returns> <c>true</c> if end of cycles; otherwise <c>false</c> </returns>
    Public Function IsEndOfCycles() As Boolean
        Return Me.IsBit(TemperatureEvents.EndOfCycles)
    End Function

    ''' <summary> Query if this object is at temperature. </summary>
    ''' <returns> <c>true</c> if at temperature; otherwise <c>false</c> </returns>
    Public Function IsAtTemperature() As Boolean
        Return Me.IsBit(TemperatureEvents.AtTemperature)
    End Function

    ''' <summary> Query if this object is not at temperature. </summary>
    ''' <returns> <c>true</c> if not at temperature; otherwise <c>false</c> </returns>
    Public Function IsNotAtTemperature() As Boolean
        Return Me.IsBit(TemperatureEvents.NotAtTemperature)
    End Function

    ''' <summary> Query if this object is end of cycle. </summary>
    ''' <returns> <c>true</c> if end of cycle; otherwise <c>false</c> </returns>
    Public Function IsEndOfCycle() As Boolean
        Return Me.IsBit(TemperatureEvents.EndOfCycle)
    End Function

    ''' <summary> Query if this object is cycling stopped. </summary>
    ''' <returns> <c>true</c> if cycling stopped; otherwise <c>false</c> </returns>
    Public Function IsCyclingStopped() As Boolean
        Return Me.IsBit(TemperatureEvents.CyclingStopped)
    End Function

    ''' <summary> Query if this object is test time elapsed. </summary>
    ''' <returns> <c>true</c> if test time elapsed; otherwise <c>false</c> </returns>
    Public Function IsTestTimeElapsed() As Boolean
        Return Me.IsBit(TemperatureEvents.TestTimeElapsed)
    End Function

    ''' <summary> Gets the description. </summary>
    ''' <value> The description. </value>
    Public ReadOnly Property Description As String
        Get
            Return Me.TemperatureEvent.Description
        End Get
    End Property

    ''' <summary> Query if any <see cref="TemperatureEvent">bits</see> are. </summary>
    ''' <param name="included"> The included. </param>
    ''' <returns> <c>true</c> if included; otherwise <c>false</c> </returns>
    Public Function IsIncluded(ByVal included As TemperatureEvents) As Boolean
        Return TemperatureEventInfo.IsIncluded(Me.TemperatureEvent, included)
    End Function

#End Region

#Region " HELPERS "

    ''' <summary> Builds Temperature Events. </summary>
    ''' <param name="value">    The value. </param>
    ''' <param name="excluded"> The excluded. </param>
    ''' <param name="included"> The included. </param>
    ''' <returns> The TemperatureEvents. </returns>
    Public Shared Function BuildTemperatureEvents(ByVal value As TemperatureEvents, ByVal excluded As TemperatureEvents, ByVal included As TemperatureEvents) As TemperatureEvents
        Return (value Or included) And Not excluded
    End Function

    ''' <summary> Query if 'value' is included. </summary>
    ''' <param name="value">    The value. </param>
    ''' <param name="included"> The included. </param>
    ''' <returns> <c>true</c> if included; otherwise <c>false</c> </returns>
    Public Shared Function IsIncluded(ByVal value As TemperatureEvents, ByVal included As TemperatureEvents) As Boolean
        Return (value And included) <> TemperatureEvents.None
    End Function

#End Region

End Class

''' <summary> Values that represent the Temperature Event bit. </summary>
Public Enum TemperatureEventBit
    <Description("At Temperature")> AtTemperature = 0
    <Description("Not At Temperature")> NotAtTemperature = 1
    <Description("Test Time Elapsed")> TestTimeElapsed = 2
    <Description("End Of Cycle")> EndOfCycle = 3
    <Description("End Of Cycles")> EndOfCycles = 4
    <Description("Cycling Stopped")> CyclingStopped = 5
End Enum

''' <summary> A bit field of flags for specifying combination of Temperature Event values. </summary>
<Flags()>
Public Enum TemperatureEvents
    <Description("None")> None = 0
    <Description("At Temperature")> AtTemperature = CInt(2 ^ CInt(TemperatureEventBit.AtTemperature))
    <Description("Not At Temperature")> NotAtTemperature = CInt(2 ^ CInt(TemperatureEventBit.NotAtTemperature))
    <Description("Test Time Elapsed")> TestTimeElapsed = CInt(2 ^ CInt(TemperatureEventBit.TestTimeElapsed))
    <Description("End Of Cycle")> EndOfCycle = CInt(2 ^ CInt(TemperatureEventBit.EndOfCycle))
    <Description("End Of Cycles")> EndOfCycles = CInt(2 ^ CInt(TemperatureEventBit.EndOfCycles))
    <Description("Cycling Stopped")> CyclingStopped = CInt(2 ^ CInt(TemperatureEventBit.CyclingStopped))
End Enum
