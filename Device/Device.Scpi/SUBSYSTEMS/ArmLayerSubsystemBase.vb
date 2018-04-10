Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Arm Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class ArmLayerSubsystemBase
    Inherits VI.ArmLayerSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ArmLayerSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

    Protected Sub New(ByVal layerNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(layerNumber, statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.ArmSource = VI.Scpi.ArmSources.Immediate
        Me.Direction = VI.Scpi.Direction.Acceptor
    End Sub

#End Region

#Region " ARM DIRECTION "

    Private _Direction As Direction?
    ''' <summary> Gets or sets the cached source Direction. </summary>
    ''' <value> The <see cref="Direction">ARM Direction</see> or none if not set or
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

    ''' <summary> Writes and reads back the ARM Direction. </summary>
    ''' <param name="value"> The ARM Direction. </param>
    ''' <returns> The <see cref="Direction">source  ARM Direction</see> or none if unknown. </returns>
    Public Overloads Function ApplyDirection(ByVal value As Direction) As Direction?
        Me.DirectionWriter(value)
        Return Me.QueryDirection()
    End Function

    ''' <summary> Queries the ARM Direction. </summary>
    ''' <returns> The <see cref="Direction"> ARM Direction</see> or none if unknown. </returns>
    Public Function QueryDirection() As Direction?
        Me.Direction = Me.Query(Of Direction)(Me.DirectionQueryCommand, Me.Direction)
        Return Me.Direction
    End Function

    ''' <summary> Writes the ARM Direction without reading back the value from the device. </summary>
    ''' <param name="value"> The ARM Direction. </param>
    ''' <returns> The <see cref="Direction"> ARM Directio`n</see> or none if unknown. </returns>
    Public Overloads Function WriteDirection(ByVal value As Direction) As Direction?
        Me.Direction = Me.Write(Of Direction)(Me.DirectionCommandFormat, value)
        Return Me.Direction
    End Function

#Region " OVERRIDES "
    ''' <summary> Direction getter. </summary>
    ''' <returns> A Nullable(Of T) </returns>
    Public Overrides Function DirectionGetter() As Integer?
        Return Me.Direction
    End Function

    ''' <summary> Direction setter. </summary>
    ''' <param name="value"> The current ArmCount. </param>
    Public Overrides Sub DirectionSetter(ByVal value As Integer?)
        Me.Direction = CType(value, Direction)
    End Sub

    ''' <summary> Direction reader. </summary>
    ''' <returns> An Integer? </returns>
    Public Overrides Function DirectionReader() As Integer?
        Return Me.QueryDirection()
    End Function

    ''' <summary> Direction writer. </summary>
    ''' <param name="value"> The ARM Direction. </param>
    ''' <returns> An Integer? </returns>
    Public Overrides Function DirectionWriter(ByVal value As Integer) As Integer?
        Return Me.WriteDirection(CType(value, Direction))
    End Function
#End Region
#End Region

#Region " ARM SOURCE "

    Private _SupportedArmSources As ArmSources
    ''' <summary>
    ''' Gets or sets the supported Function Mode.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedArmSources() As ArmSources
        Get
            Return Me._SupportedArmSources
        End Get
        Set(ByVal value As ArmSources)
            If Not Me.SupportedArmSources.Equals(value) Then
                Me._SupportedArmSources = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> List supported Arm sources. </summary>
    ''' <param name="comboBox"> The combo box. </param>
    Public Sub ListSupportedArmSources(ByVal comboBox As System.Windows.Forms.ComboBox)
        If comboBox Is Nothing Then Throw New ArgumentNullException(NameOf(comboBox))
        With comboBox
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(ArmSources).ValueNamePairs(Me.SupportedArmSources)
            .DisplayMember = "Value"
            .ValueMember = "Key"
        End With
    End Sub

    Private _ArmSource As ArmSources?
    ''' <summary> Gets or sets the cached source ArmSource. </summary>
    ''' <value> The <see cref="ArmSource">source Arm Source</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property ArmSource As ArmSources?
        Get
            Return Me._ArmSource
        End Get
        Protected Set(ByVal value As ArmSources?)
            If Not Me.ArmSource.Equals(value) Then
                Me._ArmSource = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Arm Source. </summary>
    ''' <param name="value"> The  Source Arm Source. </param>
    ''' <returns> The <see cref="ArmSource">source Arm Source</see> or none if unknown. </returns>
    Public Function ApplyArmSource(ByVal value As ArmSources) As ArmSources?
        Me.WriteArmSource(value)
        Return Me.QueryArmSource()
    End Function

    ''' <summary> Gets the Arm source query command. </summary>
    ''' <value> The Arm source query command. </value>
    ''' <remarks> SCPI: ":ARM:LAYx:SOUR?" </remarks>
    Protected Overridable ReadOnly Property ArmSourceQueryCommand As String

    ''' <summary> Queries the Arm Source. </summary>
    ''' <returns> The <see cref="ArmSource">Arm Source</see> or none if unknown. </returns>
    Public Function QueryArmSource() As ArmSources?
        Dim currentValue As String = Me.ArmSource.ToString
        If String.IsNullOrEmpty(Me.Session.EmulatedReply) Then Me.Session.EmulatedReply = currentValue
        If Not String.IsNullOrWhiteSpace(Me.ArmSourceQueryCommand) Then
            currentValue = Me.Session.QueryTrimEnd(Me.ArmSourceQueryCommand)
        End If
        If String.IsNullOrWhiteSpace(currentValue) Then
            Me.ArmSource = New ArmSources?
        Else
            Dim se As New StringEnumerator(Of ArmSources)
            Me.ArmSource = se.ParseContained(currentValue.BuildDelimitedValue)
        End If
        Return Me.ArmSource
    End Function

    ''' <summary> Gets the Arm source command format. </summary>
    ''' <value> The write Arm source command format. </value>
    ''' <remarks> SCPI: ":ARM:LAYx:SOUR {0}". </remarks>
    Protected Overridable ReadOnly Property ArmSourceCommandFormat As String

    ''' <summary> Writes the Arm Source without reading back the value from the device. </summary>
    ''' <param name="value"> The Arm Source. </param>
    ''' <returns> The <see cref="ArmSource">Arm Source</see> or none if unknown. </returns>
    Public Function WriteArmSource(ByVal value As ArmSources) As ArmSources?
        If Not String.IsNullOrWhiteSpace(Me.ArmSourceCommandFormat) Then
            Me.Session.WriteLine(Me.ArmSourceCommandFormat, value.ExtractBetween())
        End If
        Me.ArmSource = value
        Return Me.ArmSource
    End Function


#End Region

End Class

''' <summary> Enumerates the arm layer control sources. </summary>
<Flags>
Public Enum ArmSources

    <ComponentModel.Description("Not Defined ()")> None
    <ComponentModel.Description("Bus (BUS)")> Bus
    <ComponentModel.Description("External (EXT)")> External
    <ComponentModel.Description("Hold operation (HOLD)")> Hold
    <ComponentModel.Description("Immediate (IMM)")> Immediate
    <ComponentModel.Description("Manual (MAN)")> Manual
    <ComponentModel.Description("Timer (TIM)")> Timer

    ''' <summary> Event detection for the arm layer is satisfied when either a positive-going or 
    ''' a negative-going pulse (via the SOT line of the Digital I/O) is received. </summary>
    <ComponentModel.Description("SOT Pulsed High or Low (BSTES)")> StartTestBoth

    ''' <summary> Event detection for the arm layer is satisfied when a positive-going pulse 
    ''' (via the SOT line of the Digital I/O) is received.  </summary>
    <ComponentModel.Description("SOT Pulsed High (PSTES)")> StartTestHigh

    ''' <summary> Event detection for the arm layer is satisfied when a negative-going pulse 
    ''' (via the SOT line of the Digital I/O) is received. </summary>
    <ComponentModel.Description("SOT Pulsed High (NSTES)")> StartTestLow

    ''' <summary> Event detection occurs when an input trigger via the Trigger Link input line is
    ''' received. See “Trigger link,” 2400 manual page 11-19, For more information. With TLINk selected, you
    ''' can Loop around the Arm Event Detector by setting the Event detector bypass. </summary>
    <ComponentModel.Description("Trigger Link (TLIN)")> TriggerLink

    <ComponentModel.Description("All")> All = ArmSources.TriggerLink - 1

End Enum

