''' <summary> Defines a Multimeter Subsystem for a TSP System. </summary>
''' <license> (c) 2016 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2016" by="David" revision=""> Created. </history>
Public MustInherit Class MultimeterSubsystemBase
    Inherits VI.MultimeterSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystemBase">status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FunctionModeRanges.Clear()
        For Each fmode As VI.Tsp.MultimeterFunctionMode In [Enum].GetValues(GetType(VI.Tsp.MultimeterFunctionMode))
            Me.FunctionModeRanges.Add(fmode, New Core.Pith.RangeR(Me.DefaultFunctionRange))
        Next
        Me.SafePostPropertyChanged(NameOf(MultimeterSubsystemBase.FunctionModeRanges))
        Me.OpenDetectorKnownStates.Clear()
        For Each fmode As VI.Tsp.MultimeterFunctionMode In [Enum].GetValues(GetType(VI.Tsp.MultimeterFunctionMode))
            Me.OpenDetectorKnownStates.Add(fmode, False)
        Next
        Me.SafePostPropertyChanged(NameOf(MultimeterSubsystemBase.OpenDetectorKnownStates))
        Me.FunctionModeDecimalPlaces.Clear()
        For Each fmode As VI.Tsp.MultimeterFunctionMode In [Enum].GetValues(GetType(VI.Tsp.MultimeterFunctionMode))
            Me.FunctionModeDecimalPlaces.Add(fmode, Me.DefaultFunctionModeDecimalPlaces)
        Next
        Me.SafePostPropertyChanged(NameOf(MultimeterSubsystemBase.FunctionModeDecimalPlaces))
        Me.FunctionModeUnits.Clear()
        For Each fmode As VI.Tsp.MultimeterFunctionMode In [Enum].GetValues(GetType(VI.Tsp.MultimeterFunctionMode))
            Me.FunctionModeUnits.Add(fmode, Arebis.StandardUnits.UnitlessUnits.Ratio)
        Next
        Me.FunctionModeUnits(VI.Tsp.MultimeterFunctionMode.CurrentAC) = Arebis.StandardUnits.ElectricUnits.Ampere
        Me.FunctionModeUnits(VI.Tsp.MultimeterFunctionMode.CurrentDC) = Arebis.StandardUnits.ElectricUnits.Ampere
        Me.FunctionModeUnits(VI.Tsp.MultimeterFunctionMode.ResistanceCommonWire) = Arebis.StandardUnits.ElectricUnits.Ohm
        Me.FunctionModeUnits(VI.Tsp.MultimeterFunctionMode.ResistanceTwoWire) = Arebis.StandardUnits.ElectricUnits.Ohm
        Me.FunctionModeUnits(VI.Tsp.MultimeterFunctionMode.ResistanceFourWire) = Arebis.StandardUnits.ElectricUnits.Ohm
        Me.FunctionModeUnits(VI.Tsp.MultimeterFunctionMode.VoltageAC) = Arebis.StandardUnits.ElectricUnits.Volt
        Me.FunctionModeUnits(VI.Tsp.MultimeterFunctionMode.VoltageDC) = Arebis.StandardUnits.ElectricUnits.Volt
        Me.SafePostPropertyChanged(NameOf(MultimeterSubsystemBase.FunctionModeUnits))
    End Sub

#End Region

#Region " AUTO DELAY MODE "

    ''' <summary> The Auto Delay Mode. </summary>
    Private _AutoDelayMode As MultimeterAutoDelayMode?

    ''' <summary> Gets or sets the cached Auto Delay Mode. </summary>
    ''' <value> The <see cref="MultimeterAutoDelayMode">Multimeter Auto Delay Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property AutoDelayMode As MultimeterAutoDelayMode?
        Get
            Return Me._AutoDelayMode
        End Get
        Protected Set(ByVal value As MultimeterAutoDelayMode?)
            If Not Nullable.Equals(Me.AutoDelayMode, value) Then
                Me._AutoDelayMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries the Multimeter Auto Delay Mode. </summary>
    ''' <returns> The <see cref="MultimeterAutoDelayMode">Multimeter Auto Delay Mode</see> or none if unknown. </returns>
    Public MustOverride Function QueryAutoDelayMode() As MultimeterAutoDelayMode?

    ''' <summary> Writes and reads back the Multimeter Auto Delay Mode. </summary>
    ''' <param name="value"> The  Multimeter Auto Delay Mode. </param>
    ''' <returns> The <see cref="MultimeterAutoDelayMode">Multimeter Auto Delay Mode</see> or none if unknown. </returns>
    Public Function ApplyAutoDelayMode(ByVal value As MultimeterAutoDelayMode) As MultimeterAutoDelayMode?
        Me.WriteAutoDelayMode(value)
        Return Me.QueryAutoDelayMode()
    End Function

    ''' <summary> Writes the Multimeter Auto Delay Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Auto Delay Mode. </param>
    ''' <returns> The <see cref="MultimeterAutoDelayMode">Multimeter Auto Delay Mode</see> or none if unknown. </returns>
    Public MustOverride Function WriteAutoDelayMode(ByVal value As MultimeterAutoDelayMode) As MultimeterAutoDelayMode?

#End Region

#Region " FUNCTION MODE "

    ''' <summary> The function mode. </summary>
    Private _FunctionMode As MultimeterFunctionMode?

    ''' <summary> Gets or sets the cached function mode. </summary>
    ''' <value> The <see cref="MultimeterFunctionMode">Multimeter Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FunctionMode As MultimeterFunctionMode?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As MultimeterFunctionMode?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                If value.HasValue Then
                    Me.FunctionUnit = Me.ToUnit(value.Value)
                    Me.FunctionRange = Me.ToRange(value.Value)
                    Me.FunctionRangeDecimalPlaces = Me.ToDecimalPlaces(value.Value)
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries the Multimeter Function Mode. </summary>
    ''' <returns> The <see cref="MultimeterFunctionMode">Multimeter Function Mode</see> or none if unknown. </returns>
    Public MustOverride Function QueryFunctionMode() As MultimeterFunctionMode?

    ''' <summary> Writes and reads back the Multimeter Function Mode. </summary>
    ''' <param name="value"> The  Multimeter Function Mode. </param>
    ''' <returns> The <see cref="MultimeterFunctionMode">Multimeter Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As MultimeterFunctionMode) As MultimeterFunctionMode?
        Me.WriteFunctionMode(value)
        Me.FunctionMode = Me.QueryFunctionMode()
        ' changing the function mode changes range, auto delay mode and open detector enabled. 
        Me.QueryRange()
        Me.QueryAutoDelayMode()
        Me.QueryOpenDetectorEnabled()
        Return Me.FunctionMode
    End Function

    ''' <summary> Writes the Multimeter Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="MultimeterFunctionMode">Multimeter Function Mode</see> or none if unknown. </returns>
    Public MustOverride Function WriteFunctionMode(ByVal value As MultimeterFunctionMode) As MultimeterFunctionMode?

#End Region

End Class

''' <summary> Specifies the Auto Delay modes. </summary>
Public Enum MultimeterAutoDelayMode
    <ComponentModel.Description("Off (_G.dmm.OFF)")> [Off] = 0
    <ComponentModel.Description("On (_G.dmm.OFF)")> [On] = 1
    <ComponentModel.Description("Once (_G.dmm.AUTODELAY_ONCE)")> [Once] = 2
End Enum


''' <summary> Specifies the function modes. </summary>
Public Enum MultimeterFunctionMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Resistance Common Side (commonsideohms)")> ResistanceCommonWire
    <ComponentModel.Description("Resistance 2-Wire (twowireohms)")> ResistanceTwoWire
    <ComponentModel.Description("Resistance 4-Wire (fourwireohms)")> ResistanceFourWire
    <ComponentModel.Description("DC Voltage (dcvolts)")> VoltageDC
    <ComponentModel.Description("DC Current (dccurrent)")> CurrentDC
    <ComponentModel.Description("AC Voltage (acvolts)")> VoltageAC
    <ComponentModel.Description("AC Current (accurrent)")> CurrentAC
End Enum

