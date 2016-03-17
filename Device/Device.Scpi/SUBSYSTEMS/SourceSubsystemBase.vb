Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Source Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SourceSubsystemBase
    Inherits VI.SourceSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " AUTO CLEAR "

    ''' <summary> Queries the source AutoClear state. </summary>
    ''' <returns> <c>True</c> if the Auto Clear is enabled; <c>False</c> if not enabled, or none if
    ''' unknown. </returns>
    Public Overrides Function QueryAutoClearEnabled() As Boolean?
        Me.AutoClearEnabled = Me.Session.Query(Me.AutoClearEnabled.GetValueOrDefault(True), ":SOUR:CLE:AUTO?")
        Return Me.AutoClearEnabled
    End Function

    ''' <summary> Writes the state of the Source Auto Clear without reading back the value from the
    ''' device. </summary>
    ''' <param name="value"> Enable if <c>true</c>; otherwise, disable. </param>
    ''' <returns> <c>True</c> if the Auto Clear is enabled; <c>False</c> if not enabled, or none if
    ''' unknown. </returns>
    Public Overrides Function WriteAutoClearEnabled(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine(":SOUR:CLE:AUTO {0:'ON';'ON';'OFF'}", CType(value, Integer))
        Me.AutoClearEnabled = value
        Return Me.AutoClearEnabled
    End Function

#End Region

#Region " AUTO DELAY "

    ''' <summary> Queries the Source Auto Delay state. </summary>
    ''' <returns> <c>True</c> if the Auto Delay is enabled; <c>False</c> if not enabled, or none if
    ''' unknown or not set. </returns>
    Public Overrides Function QueryAutoDelayEnabled() As Boolean?
        Me.AutoDelayEnabled = Me.Session.Query(Me.AutoDelayEnabled.GetValueOrDefault(True), ":SOUR:DEL:AUTO?")
        Return Me.AutoDelayEnabled
    End Function

    ''' <summary> Writes the state of the Source Auto Delay without reading back the value from the
    ''' device. </summary>
    ''' <param name="value"> Enable if <c>true</c>; otherwise, disable. </param>
    ''' <returns> <c>True</c> if the Auto Delay is enabled; <c>False</c> if not enabled, or none if
    ''' unknown or not set. </returns>
    Public Overrides Function WriteAutoDelayEnabled(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine(":SOUR:DEL:AUTO {0:'ON';'ON';'OFF'}", CType(value, Integer))
        Me.AutoDelayEnabled = value
        Return Me.AutoDelayEnabled
    End Function

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Queries the Source Function Mode. </summary>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public Overrides Function QueryFunctionMode() As SourceFunctionModes?
        Dim mode As String = Me.FunctionMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryTrimEnd(":SOUR:FUNC?")
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = "Failed fetching source function mode"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.FunctionMode = New SourceFunctionModes?
        Else
            Dim se As New StringEnumerator(Of SourceFunctionModes)
            Me.FunctionMode = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.FunctionMode
    End Function

    ''' <summary> Writes the source Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionModes">source Function Mode</see> or none if unknown. </returns>
    Public Overrides Function WriteFunctionMode(ByVal value As SourceFunctionModes) As SourceFunctionModes?
        Me.Session.WriteLine(":SOUR:FUNC {0}", value.ExtractBetween())
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

#End Region

#Region " SWEEP POINTS "

    ''' <summary> Queries the current Sweep Points. </summary>
    ''' <returns> The SweepPoints or none if unknown. </returns>
    Public Overrides Function QuerySweepPoints() As Integer?
        Me.SweepPoints = Me.Session.Query(Me.SweepPoints.GetValueOrDefault(0), ":SOUR:SWE:POIN??")
        Return Me.SweepPoints
    End Function

    ''' <summary> Sets back the source Sweep Points without reading back the value from the device. </summary>
    ''' <param name="value"> The current Sweep Points. </param>
    ''' <returns> The SweepPoints or none if unknown. </returns>
    Public Overrides Function WriteSweepPoints(ByVal value As Integer) As Integer?
        Me.Session.WriteLine(":SOUR:SWE:POIN? {0}", value)
        Me.SweepPoints = value
        Return Me.SweepPoints
    End Function

#End Region

End Class
