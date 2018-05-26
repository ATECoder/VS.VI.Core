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
        Me.Direction = Scpi.Direction.Acceptor
        Me.InputLineNumber = 1
        Me.OutputLineNumber = 2
        Me.TriggerSource = Scpi.TriggerSources.Immediate
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
    <ComponentModel.Description("None (NONE)")> None
    <ComponentModel.Description("Source (SOUR)")> Source
    <ComponentModel.Description("Delay (DEL)")> Delay
    <ComponentModel.Description("Sense (SENS)")> Sense
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

