Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ComboBoxExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Trigger Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class TriggerSubsystemBase
    Inherits VI.TriggerSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="TriggerSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.AutoDelayEnabled = False
        Me.TriggerCount = 1
        Me.Delay = TimeSpan.Zero
        Me.Direction = Tsp2.Direction.Acceptor
        Me.InputLineNumber = 1
        Me.OutputLineNumber = 2
        Me.TriggerSource = TriggerSources.Immediate
        Me.TimerInterval = TimeSpan.FromSeconds(0.1)
        Me.SupportedTriggerSources = TriggerSources.Bus Or TriggerSources.External Or TriggerSources.Immediate
        Me.ContinuousEnabled = False
        Me.TriggerState = VI.TriggerState.None
    End Sub

#End Region

#Region " TRIGGER DIRECTION "

    Private _Direction As Direction?
    ''' <summary> Gets or sets the cached source Direction. </summary>
    ''' <value> The <see cref="Direction">source Trigger Direction</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property Direction As Direction?
        Get
            Return Me._Direction
        End Get
        Protected Set(ByVal value As Direction?)
            If Not Me.Direction.Equals(value) Then
                Me._Direction = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Trigger Direction. </summary>
    ''' <param name="value"> The  Source Trigger Direction. </param>
    ''' <returns> The <see cref="Direction">source Trigger Direction</see> or none if unknown. </returns>
    Public Function ApplyDirection(ByVal value As Direction) As Direction?
        Me.WriteDirection(value)
        Return Me.QueryDirection()
    End Function

    ''' <summary> Gets the Trigger Direction query command. </summary>
    ''' <value> The Trigger Direction query command. </value>
    ''' <remarks> SCPI: ":TRIG:DIR" </remarks>
    Protected Overridable ReadOnly Property DirectionQueryCommand As String

    ''' <summary> Queries the Trigger Direction. </summary>
    ''' <returns> The <see cref="Direction">Trigger Direction</see> or none if unknown. </returns>
    Public Function QueryDirection() As Direction?
        Me.Direction = Me.Query(Of Direction)(Me.DirectionQueryCommand, Me.Direction)
        Return Me.Direction
    End Function

    ''' <summary> Gets the Trigger Direction command format. </summary>
    ''' <value> The Trigger Direction command format. </value>
    ''' <remarks> SCPI: ":TRIG:DIR {0}" </remarks>
    Protected Overridable ReadOnly Property DirectionCommandFormat As String

    ''' <summary> Writes the Trigger Direction without reading back the value from the device. </summary>
    ''' <param name="value"> The Trigger Direction. </param>
    ''' <returns> The <see cref="Direction">Trigger Direction</see> or none if unknown. </returns>
    Public Function WriteDirection(ByVal value As Direction) As Direction?
        Me.Direction = Me.Write(Of Direction)(Me.DirectionCommandFormat, value)
        Return Me.Direction
    End Function

#End Region

#Region " TRIGGER EVENTS "

    ''' <summary> List Trigger Event. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    Public Sub ListTriggerEvent(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Dim selectedIndex As Integer = listControl.SelectedIndex
        With listControl
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(TriggerEvent).ValueDescriptionPairs(Me.SupportedTriggerEvent)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            If .Items.Count > 0 Then
                .SelectedIndex = Math.Max(selectedIndex, 0)
            End If
        End With
    End Sub

    ''' <summary> Returns the function mode selected by the list control. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    ''' <returns> The SenseTriggerEvent. </returns>
    Public Shared Function SelectedTriggerEvent(ByVal listControl As Windows.Forms.ComboBox) As TriggerEvent
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Return CType(CType(listControl.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, TriggerEvent)
    End Function

    ''' <summary> Safe select function mode. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    Public Sub SafeSelectTriggerEvent(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        If Me.TriggerEvent.HasValue Then
            listControl.SafeSelectItem(Me.TriggerEvent.Value, Me.TriggerEvent.Value.Description)
        End If
    End Sub

    Private _SupportedTriggerEvent As TriggerEvent
    ''' <summary>
    ''' Gets or sets the supported Function Mode.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedTriggerEvent() As TriggerEvent
        Get
            Return _SupportedTriggerEvent
        End Get
        Set(ByVal value As TriggerEvent)
            If Not Me.SupportedTriggerEvent.Equals(value) Then
                Me._SupportedTriggerEvent = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> List supported trigger Event. </summary>
    ''' <param name="comboBox"> The combo box. </param>
    Public Sub ListSupportedTriggerEvent(ByVal comboBox As System.Windows.Forms.ComboBox)
        If comboBox Is Nothing Then Throw New ArgumentNullException(NameOf(comboBox))
        With comboBox
            .DataSource = Nothing
            .Invalidate() : Windows.Forms.Application.DoEvents()
            ' unit tests caused an exception in internal Check No Data Event method even though, clearly, the data Event was set to nothing 
            If .DataSource Is Nothing Then .Items.Clear()
            .DataSource = GetType(TriggerEvent).ValueNamePairs(Me.SupportedTriggerEvent)
            .DisplayMember = "Value"
            .ValueMember = "Key"
        End With
    End Sub

    Private _TriggerEvent As TriggerEvent?
    ''' <summary> Gets or sets the cached Event TriggerEvent. </summary>
    ''' <value> The <see cref="TriggerEvent">Event Trigger Event</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property TriggerEvent As TriggerEvent?
        Get
            Return Me._TriggerEvent
        End Get
        Protected Set(ByVal value As TriggerEvent?)
            If Not Me.TriggerEvent.Equals(value) Then
                Me._TriggerEvent = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Event Trigger Event. </summary>
    ''' <param name="value"> The  Event Trigger Event. </param>
    ''' <returns> The <see cref="TriggerEvent">Event Trigger Event</see> or none if unknown. </returns>
    Public Function ApplyTriggerEvent(ByVal value As TriggerEvent) As TriggerEvent?
        Me.WriteTriggerEvent(value)
        Return Me.QueryTriggerEvent()
    End Function

    ''' <summary> Gets the Trigger Event query command. </summary>
    ''' <value> The Trigger Event query command. </value>
    ''' <remarks> SCPI: ":TRIG:SOUR?" </remarks>
    Protected Overridable ReadOnly Property TriggerEventQueryCommand As String

    ''' <summary> Queries the Trigger Event. </summary>
    ''' <returns> The <see cref="TriggerEvent">Trigger Event</see> or none if unknown. </returns>
    Public Function QueryTriggerEvent() As TriggerEvent?
        Dim currentValue As String = Me.TriggerEvent.ToString
        If String.IsNullOrEmpty(Me.Session.EmulatedReply) Then Me.Session.EmulatedReply = currentValue
        If Not String.IsNullOrWhiteSpace(Me.TriggerEventQueryCommand) Then
            currentValue = Me.Session.QueryTrimEnd(Me.TriggerEventQueryCommand)
        End If
        If String.IsNullOrWhiteSpace(currentValue) Then
            Me.TriggerEvent = New TriggerEvent?
        Else
            Dim se As New StringEnumerator(Of TriggerEvent)
            Me.TriggerEvent = se.ParseContained(currentValue.BuildDelimitedValue)
        End If
        Return Me.TriggerEvent
    End Function

    ''' <summary> Gets the Trigger Event command format. </summary>
    ''' <value> The write Trigger Event command format. </value>
    ''' <remarks> SCPI: ":TRIG:SOUR {0}". </remarks>
    Protected Overridable ReadOnly Property TriggerEventCommandFormat As String

    ''' <summary> Writes the Trigger Event without reading back the value from the device. </summary>
    ''' <param name="value"> The Trigger Event. </param>
    ''' <returns> The <see cref="TriggerEvent">Trigger Event</see> or none if unknown. </returns>
    Public Function WriteTriggerEvent(ByVal value As TriggerEvent) As TriggerEvent?
        Me.Write(Me.TriggerEventCommandFormat, value.ExtractBetween())
        Me.TriggerEvent = value
        Return Me.TriggerEvent
    End Function

#End Region

#Region " TRIGGER SOURCE "

    ''' <summary> List Trigger Sources. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    Public Sub ListTriggerSources(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Dim selectedIndex As Integer = listControl.SelectedIndex
        With listControl
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(TriggerSources).ValueDescriptionPairs(Me.SupportedTriggerSources)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            If .Items.Count > 0 Then
                .SelectedIndex = Math.Max(selectedIndex, 0)
            End If
        End With
    End Sub

    ''' <summary> Returns the function mode selected by the list control. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    ''' <returns> The SenseTriggerSource. </returns>
    Public Shared Function SelectedTriggerSource(ByVal listControl As Windows.Forms.ComboBox) As TriggerSources
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Return CType(CType(listControl.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, TriggerSources)
    End Function

    ''' <summary> Safe select function mode. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    Public Sub SafeSelectTriggerSource(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        If Me.TriggerSource.HasValue Then
            listControl.SafeSelectItem(Me.TriggerSource.Value, Me.TriggerSource.Value.Description)
        End If
    End Sub

    Private _SupportedTriggerSources As TriggerSources
    ''' <summary>
    ''' Gets or sets the supported Function Mode.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedTriggerSources() As TriggerSources
        Get
            Return _SupportedTriggerSources
        End Get
        Set(ByVal value As TriggerSources)
            If Not Me.SupportedTriggerSources.Equals(value) Then
                Me._SupportedTriggerSources = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> List supported trigger sources. </summary>
    ''' <param name="comboBox"> The combo box. </param>
    Public Sub ListSupportedTriggerSources(ByVal comboBox As System.Windows.Forms.ComboBox)
        If comboBox Is Nothing Then Throw New ArgumentNullException(NameOf(comboBox))
        With comboBox
            .DataSource = Nothing
            .Invalidate() : Windows.Forms.Application.DoEvents()
            ' unit tests caused an exception in internal Check No Data Source method even though, clearly, the data source was set to nothing 
            If .DataSource Is Nothing Then .Items.Clear()
            .DataSource = GetType(TriggerSources).ValueNamePairs(Me.SupportedTriggerSources)
            .DisplayMember = "Value"
            .ValueMember = "Key"
        End With
    End Sub


    Private _TriggerSource As TriggerSources?
    ''' <summary> Gets or sets the cached source TriggerSource. </summary>
    ''' <value> The <see cref="TriggerSource">source Trigger Source</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property TriggerSource As TriggerSources?
        Get
            Return Me._TriggerSource
        End Get
        Protected Set(ByVal value As TriggerSources?)
            If Not Me.TriggerSource.Equals(value) Then
                Me._TriggerSource = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Trigger Source. </summary>
    ''' <param name="value"> The  Source Trigger Source. </param>
    ''' <returns> The <see cref="TriggerSource">source Trigger Source</see> or none if unknown. </returns>
    Public Function ApplyTriggerSource(ByVal value As TriggerSources) As TriggerSources?
        Me.WriteTriggerSource(value)
        Return Me.QueryTriggerSource()
    End Function

    ''' <summary> Gets the Trigger source query command. </summary>
    ''' <value> The Trigger source query command. </value>
    ''' <remarks> SCPI: ":TRIG:SOUR?" </remarks>
    Protected Overridable ReadOnly Property TriggerSourceQueryCommand As String

    ''' <summary> Queries the Trigger Source. </summary>
    ''' <returns> The <see cref="TriggerSource">Trigger Source</see> or none if unknown. </returns>
    Public Function QueryTriggerSource() As TriggerSources?
        Dim currentValue As String = Me.TriggerSource.ToString
        If String.IsNullOrEmpty(Me.Session.EmulatedReply) Then Me.Session.EmulatedReply = currentValue
        If Not String.IsNullOrWhiteSpace(Me.TriggerSourceQueryCommand) Then
            currentValue = Me.Session.QueryTrimEnd(Me.TriggerSourceQueryCommand)
        End If
        If String.IsNullOrWhiteSpace(currentValue) Then
            Me.TriggerSource = New TriggerSources?
        Else
            Dim se As New StringEnumerator(Of TriggerSources)
            Me.TriggerSource = se.ParseContained(currentValue.BuildDelimitedValue)
        End If
        Return Me.TriggerSource
    End Function

    ''' <summary> Gets the Trigger source command format. </summary>
    ''' <value> The write Trigger source command format. </value>
    ''' <remarks> SCPI: ":TRIG:SOUR {0}". </remarks>
    Protected Overridable ReadOnly Property TriggerSourceCommandFormat As String

    ''' <summary> Writes the Trigger Source without reading back the value from the device. </summary>
    ''' <param name="value"> The Trigger Source. </param>
    ''' <returns> The <see cref="TriggerSource">Trigger Source</see> or none if unknown. </returns>
    Public Function WriteTriggerSource(ByVal value As TriggerSources) As TriggerSources?
        If Not String.IsNullOrWhiteSpace(Me.TriggerSourceCommandFormat) Then
            Me.Session.WriteLine(Me.TriggerSourceCommandFormat, value.ExtractBetween())
        End If
        Me.TriggerSource = value
        Return Me.TriggerSource
    End Function

#End Region

#Region " BLENDER CLEAR "

    Protected Overridable ReadOnly Property TriggerBlenderClearCommandFormat As String = "trigger.blender[{0}].clear()"

    ''' <summary> Trigger blender clear. </summary>
    ''' <param name="blenderNumber"> The blender number. </param>
    Public Sub TriggerBlenderClear(ByVal blenderNumber As Integer)
        Me.Execute(Me.TriggerBlenderClearCommandFormat, blenderNumber)
    End Sub

#End Region

#Region " BLENDER OR ENABLED  "

    Public Function ApplyTriggerBlenderOrEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoDelayEnabled(value)
        Return Me.QueryAutoDelayEnabled()
    End Function

    Protected Overridable ReadOnly Property TriggerBlenderOrEnableQueryCommandFormat As String = "trigger.blender[{0}].orenable=true"

    Public Function QueryTriggerBlenderOrEnabled(ByVal blenderNumber As Integer) As Boolean?
        Me.AutoDelayEnabled = Me.Query(False, String.Format(Me.TriggerBlenderOrEnableQueryCommandFormat, blenderNumber))
        Return Me.AutoDelayEnabled
    End Function

    Protected Overridable ReadOnly Property TriggerBlenderOrEnableCommandFormat As String = "trigger.blender[{0}].orenable={1}"

    Public Sub WriteTriggerBlenderOrEnables(ByVal blenderNumber As Integer, ByVal value As Boolean)
        Me.Execute(Me.TriggerBlenderOrEnableCommandFormat, blenderNumber, value)
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " ABORT / INIT COMMANDS "

    ''' <summary> Gets the Abort command. </summary>
    ''' <value> The Abort command. </value>
    Protected Overrides ReadOnly Property AbortCommand As String = "trigger.model.abort()"

    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    Protected Overrides ReadOnly Property InitiateCommand As String = "trigger.model.initiate()"

    ''' <summary> Gets or sets the clear command. </summary>
    ''' <value> The clear command. </value>
    Protected Overrides ReadOnly Property ClearCommand As String = "trigger.extin.clear()"

    ''' <summary> Gets or sets the clear  trigger model command. </summary>
    ''' <remarks> SCPI: ":TRIG:LOAD 'EMPTY'". </remarks>
    ''' <value> The clear command. </value>
    Protected Overrides ReadOnly Property ClearTriggerModelCommand As String = "trigger.model.clear()"


#End Region

#Region " TRIGGER STATE "

    Protected Overrides ReadOnly Property TriggerStateQueryCommand As String = "trigger.model.state()"

#End Region

#Region " TRIGGER COUNT "

    ''' <summary> Gets trigger count query command. </summary>
    ''' <value> The trigger count query command. </value>
    Protected Overrides ReadOnly Property TriggerCountQueryCommand As String = "" '  ":TRIG:COUN?"

    ''' <summary> Gets trigger count command format. </summary>
    ''' <value> The trigger count command format. </value>
    Protected Overrides ReadOnly Property TriggerCountCommandFormat As String = "" '  ":TRIG:COUN {0}"

#End Region

#Region " DELAY "

    ''' <summary> Gets the delay command format. </summary>
    ''' <value> The delay command format. </value>
    Protected Overrides ReadOnly Property DelayCommandFormat As String = "" '  ":TRIG:DEL {0:s\.FFFFFFF}"

    ''' <summary> Gets the Delay format for converting the query to time span. </summary>
    ''' <value> The Delay query command. </value>
    Protected Overrides ReadOnly Property DelayFormat As String = "" ' "s\.FFFFFFF"

    ''' <summary> Gets the delay query command. </summary>
    ''' <value> The delay query command. </value>
    Protected Overrides ReadOnly Property DelayQueryCommand As String = "" ' ":TRIG:DEL?"

#End Region
#End Region

End Class

''' <summary> Enumerates the trigger or arm layer bypass mode. </summary>
Public Enum Direction
    <ComponentModel.Description("Not Defined ()")> None
    ''' <summary> An enum constant representing the acceptor (bypass) option. </summary>
    <ComponentModel.Description("Acceptor (ACC")> Acceptor
    <ComponentModel.Description("Source (SOUR)")> Source
End Enum

''' <summary> Enumerates the arm to trigger events. </summary>
Public Enum TriggerEvent
    <ComponentModel.Description("None (trigger.EVENT_NONE)")> None
    <ComponentModel.Description("Display (trigger.EVENT_DISPLAY)")> Display
    ''' <summary> An enum constant representing the notify 1 option. generates a trigger event when
    '''    the trigger model executes it </summary>
    <ComponentModel.Description("Notify trigger block 1 (trigger.EVENT_NOTIFY1)")> Notify1
    <ComponentModel.Description("Notify trigger block 2 (trigger.EVENT_NOTIFY2)")> Notify2
    <ComponentModel.Description("Notify trigger block 3 (trigger.EVENT_NOTIFY3)")> Notify3
    <ComponentModel.Description("Notify trigger block 4 (trigger.EVENT_NOTIFY4)")> Notify4
    <ComponentModel.Description("Notify trigger block 5 (trigger.EVENT_NOTIFY5)")> Notify5
    <ComponentModel.Description("Notify trigger block 6 (trigger.EVENT_NOTIFY6)")> Notify6
    <ComponentModel.Description("Notify trigger block 7 (trigger.EVENT_NOTIFY7)")> Notify7
    <ComponentModel.Description("Notify trigger block 8 (trigger.EVENT_NOTIFY8)")> Notify8
    ''' <summary> An enum constant representing the bus option. A command interface trigger (bus trigger): 
    '''           Any remote interface: *TRG or GPIB GET bus command  or VXI-11 command device_trigger  </summary>
    <ComponentModel.Description("Bus command (trigger.EVENT_COMMAND)")> Bus

    ''' <summary> An enum constant representing the line edge option. 
    '''           Line edge (either rising, falling, or either based on the 
    '''           configuration of the line) detected on digital input line N (1 to 6)  </summary>
    <ComponentModel.Description("Line Edge 1 (trigger.EVENT_DIGIO1)")> LineEdge1
    <ComponentModel.Description("Line Edge 2 (trigger.EVENT_DIGIO2)")> LineEdge2
    <ComponentModel.Description("Line Edge 3 (trigger.EVENT_DIGIO3)")> LineEdge3
    <ComponentModel.Description("Line Edge 4 (trigger.EVENT_DIGIO4)")> LineEdge4
    <ComponentModel.Description("Line Edge 5 (trigger.EVENT_DIGIO5)")> LineEdge5
    <ComponentModel.Description("Line Edge 6 (trigger.EVENT_DIGIO6)")> LineEdge6

    ''' <summary> An enum constant representing the tsp link 1 option. 
    '''           Line edge detected on TSP-Link synchronization line N (1 to 3) </summary>
    <ComponentModel.Description("Tsp-Link Line Edge 1 (trigger.EVENT_TSPLINK1)")> TspLink1
    <ComponentModel.Description("Tsp-Link Line Edge 2 (trigger.EVENT_TSPLINK2)")> TspLink2
    <ComponentModel.Description("Tsp-Link Line Edge 3 (trigger.EVENT_TSPLINK3)")> TspLink3
    ''' <summary> An enum constant representing the Tangent 1 option. 
    '''           Appropriate LXI trigger packet is received on LAN trigger object N (1 to 8) </summary>
    <ComponentModel.Description("Lan Trigger 1 (trigger.EVENT_LAN1)")> Lan1
    <ComponentModel.Description("Lan Trigger 2 (trigger.EVENT_LAN2)")> Lan2
    <ComponentModel.Description("Lan Trigger 3 (trigger.EVENT_LAN3)")> Lan3
    <ComponentModel.Description("Lan Trigger 4 (trigger.EVENT_LAN4)")> Lan4
    <ComponentModel.Description("Lan Trigger 5 (trigger.EVENT_LAN5)")> Lan5
    <ComponentModel.Description("Lan Trigger 6 (trigger.EVENT_LAN6)")> Lan6
    <ComponentModel.Description("Lan Trigger 7 (trigger.EVENT_LAN7)")> Lan7
    <ComponentModel.Description("Lan Trigger 8 (trigger.EVENT_LAN8)")> Lan8
    <ComponentModel.Description("Blender 1 (trigger.EVENT_BLENDER1)")> Blender1
    <ComponentModel.Description("Blender 2 (trigger.EVENT_BLENDER2)")> Blender2
    <ComponentModel.Description("Timer Expired 2 (trigger.EVENT_TIMER1)")> Timer1
    <ComponentModel.Description("Timer Expired 2 (trigger.EVENT_TIMER2)")> Timer2
    <ComponentModel.Description("Timer Expired 3 (trigger.EVENT_TIMER3)")> Timer3
    <ComponentModel.Description("Timer Expired 4 (trigger.EVENT_TIMER4)")> Timer4
    <ComponentModel.Description("Analog Trigger (trigger.EVENT_ANALOGTRIGGER)")> Analog
    <ComponentModel.Description("External in trigger (trigger.EVENT_EXTERNAL)")> External
End Enum

''' <summary> Enumerates the trigger layer control sources. </summary>
<Flags>
Public Enum TriggerSources
    <ComponentModel.Description("Not Defined ()")> None
    <ComponentModel.Description("Bus (BUS)")> Bus = 1
    <ComponentModel.Description("External (EXT)")> External = Bus << 1
    <ComponentModel.Description("Immediate (IMM)")> Immediate = External << 1
    <ComponentModel.Description("Trigger Link (TLIN)")> TriggerLink = Immediate << 1
    <ComponentModel.Description("Internal (INT)")> Internal = TriggerLink << 1
    <ComponentModel.Description("Manual (MAN)")> Manual = Internal << 1
    <ComponentModel.Description("Hold (HOLD)")> Hold = Manual << 1
    <ComponentModel.Description("Timer (TIM)")> Timer = Hold << 1
    <ComponentModel.Description("LXI (LAN)")> Lan = Timer << 1
    <ComponentModel.Description("Analog (ATR)")> Analog = Lan << 1
    <ComponentModel.Description("Blender (BLEND)")> Blender = Analog << 1
    <ComponentModel.Description("Digital I/O (DIG)")> Digital = Blender << 1
End Enum

