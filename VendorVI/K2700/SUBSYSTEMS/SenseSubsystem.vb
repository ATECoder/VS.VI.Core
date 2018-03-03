''' <summary> Defines a SCPI Sense Subsystem for a Keithley 2700 instrument. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/22/2013" by="David" revision="3.0.5013"> Created. </history>
Public Class SenseSubsystem
    Inherits VI.Scpi.SenseSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.SupportedFunctionModes = VI.Scpi.SenseFunctionModes.CurrentDC Or
                                        VI.Scpi.SenseFunctionModes.VoltageDC Or
                                        VI.Scpi.SenseFunctionModes.FourWireResistance
        ' the readings are initialized when the format system is reset.
        Me._readings = New Readings
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary>
    ''' Sets subsystem values to their known execution clear state.
    ''' </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me._readings.Reset()
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FunctionMode = VI.Scpi.SenseFunctionModes.VoltageDC
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

#Region " LATEST DATA "

    ''' <summary> Gets the latest data query command. </summary>
    ''' <value> The latest data query command. </value>
    Protected Overrides ReadOnly Property LatestDataQueryCommand As String = ":SENSE:DATA:LAT?"

#End Region

#End Region

#Region " LATEST DATA "

    Private _Readings As Readings

    ''' <summary> Returns the readings. </summary>
    ''' <returns> The readings. </returns>
    Public Function Readings() As Readings
        Return Me._readings
    End Function

    ''' <summary> Parses a new set of reading elements. </summary>
    ''' <param name="reading"> Specifies the measurement text to parse into the new reading. </param>
    Public Overrides Sub ParseReading(ByVal reading As String)

        ' check if we have units suffixes.
        If (Me.Readings.Elements And isr.VI.ReadingTypes.Units) <> 0 Then
            reading = ReadingEntity.TrimUnits(reading)
        End If

        ' Take a reading and parse the results
        Me.Readings.TryParse(reading)

    End Sub

#End Region

#Region " FUNCTION MODE "

    ''' <summary> Converts a functionMode to a range. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an isr.Core.Pith.RangeR. </returns>
    Public Overrides Function ToRange(ByVal functionMode As Integer) As isr.Core.Pith.RangeR
        Dim result As isr.Core.Pith.RangeR = isr.Core.Pith.RangeR.Full
        Select Case functionMode
            Case VI.Scpi.SenseFunctionModes.CurrentDC, VI.Scpi.SenseFunctionModes.Current, VI.Scpi.SenseFunctionModes.CurrentAC
                result = New Core.Pith.RangeR(0, 10)
            Case VI.Scpi.SenseFunctionModes.VoltageDC, VI.Scpi.SenseFunctionModes.Voltage, VI.Scpi.SenseFunctionModes.VoltageAC
                result = New Core.Pith.RangeR(0, 1000)
            Case VI.Scpi.SenseFunctionModes.FourWireResistance
                result = New Core.Pith.RangeR(0, 2000000)
            Case VI.Scpi.SenseFunctionModes.Resistance
                result = New Core.Pith.RangeR(0, 1000000000D)
        End Select
        Return result
    End Function

    ''' <summary> Converts a functionMode to a decimal places. </summary>
    ''' <param name="functionMode"> The function mode. </param>
    ''' <returns> FunctionMode as an Integer. </returns>
    Public Overrides Function ToDecimalPlaces(ByVal functionMode As Integer) As Integer
        Dim result As Integer = 3
        Select Case functionMode
            Case VI.Scpi.SenseFunctionModes.CurrentDC, VI.Scpi.SenseFunctionModes.Current, VI.Scpi.SenseFunctionModes.CurrentAC
                result = 3
            Case VI.Scpi.SenseFunctionModes.VoltageDC, VI.Scpi.SenseFunctionModes.Voltage, VI.Scpi.SenseFunctionModes.VoltageAC
                result = 3
            Case VI.Scpi.SenseFunctionModes.FourWireResistance
                result = 0
            Case VI.Scpi.SenseFunctionModes.Resistance
                result = 0
        End Select
        Return result
    End Function

#End Region

End Class
