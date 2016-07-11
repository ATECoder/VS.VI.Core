Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ComboBoxExtensions
''' <summary> Defines the Channel Trace subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public MustInherit Class ChannelTraceSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class.
    ''' </summary>
    ''' <remarks> David, 7/6/2016. </remarks>
    ''' <param name="TraceNumber">    The Trace number. </param>
    ''' <param name="channelNumber">   The channel number. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal traceNumber As Integer, ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.TraceNumber = traceNumber
        Me.ChannelNumber = channelNumber
    End Sub

#End Region

#Region " CHANNEL "

    ''' <summary> Gets or sets the channel number. </summary>
    ''' <value> The channel number. </value>
    Public ReadOnly Property ChannelNumber As Integer

#End Region

#Region " TRACE "

    ''' <summary> Gets or sets the Trace number. </summary>
    ''' <value> The Trace number. </value>
    Public ReadOnly Property TraceNumber As Integer

#End Region

#Region " AUTO SCALE "

    ''' <summary> Gets the auto scale command. </summary>
    ''' <value> The auto scale command. </value>
    ''' <remarks> SCPI: ":DISP:WIND&lt;c#&gt;:TRAC&lt;t#&gt;:Y:AUTO". </remarks>
    Protected Overridable ReadOnly Property AutoScaleCommand As String

    ''' <summary> Automatic scale. </summary>
    ''' <remarks> David, 7/11/2016. </remarks>
    Public Sub AutoScale()
        If Not String.IsNullOrWhiteSpace(Me.SelectCommand) Then
            Me.Session.WriteLine(Me.SelectCommand)
        End If
    End Sub

#End Region

#Region " SELECT "

    ''' <summary> Gets the Select command. </summary>
    ''' <value> The Select command. </value>
    ''' <remarks> SCPI: ":CALC&lt;c#&gt;:PAR&lt;t#&gt;:SEL". </remarks>
    Protected Overridable ReadOnly Property SelectCommand As String

    ''' <summary> Selects this as active trace. </summary>
    Public Sub [Select]()
        If Not String.IsNullOrWhiteSpace(Me.SelectCommand) Then
            Me.Session.WriteLine(Me.SelectCommand)
        End If
    End Sub

#End Region

#Region " TRACE PARAMETER "

    ''' <summary> List adapters. </summary>
    ''' <remarks> David, 7/8/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    Public Sub ListParameters(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Dim selectedIndex As Integer = listControl.SelectedIndex
        With listControl
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(ReadingTypes).ValueDescriptionPairs(Me.SupportedParameters)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            If .Items.Count > 0 Then
                .SelectedIndex = Math.Max(selectedIndex, 0)
            End If
        End With
    End Sub

    ''' <summary> Returns the function mode selected by the list control. </summary>
    ''' <remarks> David, 7/11/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    ''' <returns> The SenseTraceParameters. </returns>
    Public Shared Function SelectedTraceParameters(ByVal listControl As Windows.Forms.ComboBox) As TraceParameters
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Return CType(CType(listControl.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, TraceParameters)
    End Function

    ''' <summary> Safe select function mode. </summary>
    ''' <remarks> David, 7/11/2016. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    Public Sub SafeSelectTraceParameters(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        If Me.Parameter.HasValue Then
            listControl.SafeSelectItem(Me.Parameter.Value, Me.Parameter.Value.Description)
        End If
    End Sub


    Private _SupportedParameters As TraceParameters
    ''' <summary>
    ''' Gets or sets the supported Trace Parameter.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedParameters() As TraceParameters
        Get
            Return _SupportedParameters
        End Get
        Set(ByVal value As TraceParameters)
            If Not Me.SupportedParameters.Equals(value) Then
                Me._SupportedParameters = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.SupportedParameters))
            End If
        End Set
    End Property

    ''' <summary> The Trace Parameter. </summary>
    Private _Parameter As TraceParameters?

    ''' <summary> Gets or sets the cached source Parameter. </summary>
    ''' <value> The <see cref="TraceParameters">Trace Parameter</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property Parameter As TraceParameters?
        Get
            Return Me._Parameter
        End Get
        Protected Set(ByVal value As TraceParameters?)
            If Not Nullable.Equals(Me.Parameter, value) Then
                Me._Parameter = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Parameter))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trace Parameter. </summary>
    ''' <param name="value"> The  Trace Parameter. </param>
    ''' <returns> The <see cref="TraceParameters">Trace Parameter</see> or none if unknown. </returns>
    Public Function ApplyParameter(ByVal value As TraceParameters) As TraceParameters?
        Me.WriteParameter(value)
        Return Me.QueryParameter()
    End Function

    ''' <summary> Gets or sets the Trace Parameter query command. </summary>
    ''' <value> The Trace Parameter query command, e.g., :CALC&lt;c#&gt;:PAR&lt;t#&gt;:DEF? </value>
    Protected Overridable ReadOnly Property ParameterQueryCommand As String

    ''' <summary> Queries the Trace Parameter. </summary>
    ''' <returns> The <see cref="TraceParameters">Trace Parameter</see> or none if unknown. </returns>
    Public Function QueryParameter() As TraceParameters?
        If String.IsNullOrWhiteSpace(Me.ParameterQueryCommand) Then
            Me.Parameter = New TraceParameters?
        Else
            Dim mode As String = Me.Parameter.ToString
            Me.Session.MakeEmulatedReplyIfEmpty(mode)
            mode = Me.Session.QueryTrimEnd(Me.ParameterQueryCommand)
            If String.IsNullOrWhiteSpace(mode) Then
                Dim message As String = "Failed fetching Trace Parameter"
                Debug.Assert(Not Debugger.IsAttached, message)
                Me.Parameter = New TraceParameters?
            Else
                Dim se As New StringEnumerator(Of TraceParameters)
                Me.Parameter = se.ParseContained(mode.BuildDelimitedValue)
            End If
        End If
        Return Me.Parameter
    End Function

    ''' <summary> Gets or sets the Trace Parameter command. </summary>
    ''' <value> The Trace Parameter command, e.g., :CALC&lt;c#&gt;:PAR&lt;t#&gt;:DEF {0}. </value>
    Protected Overridable ReadOnly Property ParameterCommandFormat As String

    ''' <summary> Writes the Trace Parameter without reading back the value from the device. </summary>
    ''' <param name="value"> The Trace Parameter. </param>
    ''' <returns> The <see cref="TraceParameters">Trace Parameter</see> or none if unknown. </returns>
    Public Function WriteParameter(ByVal value As TraceParameters) As TraceParameters?
        If Not String.IsNullOrWhiteSpace(Me.ParameterCommandFormat) Then
            Me.Session.WriteLine(Me.ParameterCommandFormat, value.ExtractBetween())
        End If
        Me.Parameter = value
        Return Me.Parameter
    End Function

#End Region

End Class

''' <summary> A bit-field of flags for specifying trace parameters. </summary>
''' <remarks> David, 7/6/2016. </remarks>
<Flags>
Public Enum TraceParameters
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Z: Absolute impedance value (Z)")> AbsoluteImpedance = 1
    <ComponentModel.Description("Y: Absolute admittance (Y)")> AbsoluteAdmittance = AbsoluteImpedance << 1
    <ComponentModel.Description("R: Equivalent series resistance (R)")> SeriesResistance = AbsoluteAdmittance << 1
    <ComponentModel.Description("X: Equivalent series reactance (X)")> SeriesReactance = SeriesResistance << 1
    <ComponentModel.Description("G: Equivalent parallel conductance (G)")> ParallelConductance = SeriesReactance << 1
    <ComponentModel.Description("B: Equivalent parallel susceptance (B)")> ParallelSusceptance = ParallelConductance << 1
    <ComponentModel.Description("LS: Equivalent series inductance (LS)")> SeriesInductance = ParallelSusceptance << 1
    <ComponentModel.Description("LP: Equivalent parallel inductance (LP)")> ParallelInductance = SeriesInductance << 1
    <ComponentModel.Description("CS: Equivalent series capacitance (CS)")> SeriesCapacitance = ParallelInductance << 1
    <ComponentModel.Description("CP: Equivalent parallel capacitance (CP)")> ParallelCapacitance = SeriesCapacitance << 1
    <ComponentModel.Description("RS: Equivalent series resistance (RS)")> SeriesResistance1 = ParallelCapacitance << 1
    <ComponentModel.Description("RP: Equivalent parallel resistance (RP)")> ParallelResistance = SeriesResistance1 << 1
    <ComponentModel.Description("Q: Q value (Q)")> QualityFactor = ParallelResistance << 1
    <ComponentModel.Description("D: Dissipation factor (D)")> DissipationFactor = QualityFactor << 1
    <ComponentModel.Description("TZ: Impedance phase (TZ)")> ImpedancePhase = DissipationFactor << 1
    <ComponentModel.Description("TY: Absolute phase (TY)")> AbsolutePhase = ImpedancePhase << 1
    <ComponentModel.Description("VAC: Oscillator Voltage (VAC)")> OscillatorVoltage = AbsolutePhase << 1
    <ComponentModel.Description("IAC: Oscillator Current (IAC)")> OscillatorCurrent = OscillatorVoltage << 1
    <ComponentModel.Description("VDC: DC Bias Voltage (VDC)")> BiasVoltage = OscillatorCurrent << 1
    <ComponentModel.Description("IDC: DC Bias Current (IDC)")> BiasCurrent = BiasVoltage << 1
    <ComponentModel.Description("IMP: Complex Impedance (IMP)")> ComplexImpedance = BiasCurrent << 1
    <ComponentModel.Description("ADM: Complex Admittance (ADM)")> ComplexAdmittance = ComplexImpedance << 1
End Enum
