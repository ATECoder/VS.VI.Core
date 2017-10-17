Imports System.ComponentModel
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.NumericUpDownExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Measure control -- defines the Four Wire Resistance Sense subsystem  settings. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="3/4/2014" by="David" revision=""> Created. </history>
Public Class K2000MeasureControl
    Inherits TalkerControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()

        ' This call is required by the designer.
        Me.InitializeComponent()

        ' populate the range mode selector
        Me.ListNonAutoRangeModes()

    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Me._SenseResistanceSubsystem = Nothing
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " MEASURE SUBSYSTEM "

    Private WithEvents _TriggerSubsystem As TriggerSubsystem

    ''' <summary> Gets or sets the trigger subsystem. </summary>
    ''' <value> The trigger subsystem. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property TriggerSubsystem As TriggerSubsystem
        Get
            Return Me._TriggerSubsystem
        End Get
        Set(value As TriggerSubsystem)
            Me._TriggerSubsystem = value
        End Set
    End Property

    ''' <summary> Gets or sets the resistance sense subsystem. </summary>
    ''' <value> The resistance sense subsystem. </value>
    Public Property ResistanceSenseSubsystem As SenseResistanceSubsystem

    ''' <summary> The four wire resistance sense subsystem. </summary>
    ''' <value> The four wire resistance sense subsystem. </value>
    Public Property FourWireResistanceSenseSubsystem As SenseFourWireResistanceSubsystem

    ''' <summary> Gets or sets the sense subsystem. </summary>
    ''' <value> The sense subsystem. </value>
    Public Property SenseSubsystem As SenseSubsystem

    Private WithEvents _SenseResistanceSubsystem As VI.Scpi.SenseResistanceSubsystemBase

    ''' <summary> Gets or sets the resistance sense subsystem . </summary>
    ''' <value> The Resistance Sense subsystem . </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SenseResistanceSubsystem As VI.Scpi.SenseResistanceSubsystemBase
        Get
            Return _SenseResistanceSubsystem
        End Get
        Set(value As VI.Scpi.SenseResistanceSubsystemBase)
            Me._SenseResistanceSubsystem = value
        End Set
    End Property

    ''' <summary> Handle the Trigger subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As TriggerSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.Delay)
                If subsystem.Delay.HasValue Then
                    Me.TriggerDelay = subsystem.Delay.Value
                End If
        End Select
    End Sub

    ''' <summary> Trigger subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub TriggerSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _TriggerSubsystem.PropertyChanged
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, TriggerSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling property '{e?.PropertyName}' change;. {ex.ToFullBlownString}")
        End Try
    End Sub


    ''' <summary> Handle the Four Wire Resistance Sense subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As SenseFourWireResistanceSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.Range)
                If subsystem.Range.HasValue Then
                    Me.MeterRange = CDec(subsystem.Range.Value)
                End If
        End Select
    End Sub

    ''' <summary> Wire Resistance Sense subsystem  property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SenseFourWireResistanceSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _SenseResistanceSubsystem.PropertyChanged
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, SenseFourWireResistanceSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"Exception handling property '{e?.PropertyName}' change;. {ex.ToFullBlownString}")
        End Try
    End Sub

    ''' <summary> Applies the meter range mode. </summary>
    Public Sub ApplyMeterRangeMode()
        If Me.SenseResistanceSubsystem IsNot Nothing AndAlso
                Not Nullable.Equals(Me.SenseResistanceSubsystem.Range, Me.MeterRange) Then
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Applying meter range settings;. range={0}; current={1}",
                               Me._MeterCurrentNumeric.Value, Me._MeterRangeNumeric.Value)
            Me.SenseResistanceSubsystem.ApplyRange(Me.MeterRange)
            Me.SenseResistanceSubsystem.ReadRegisters()
        End If
    End Sub

    ''' <summary> Applies the meter trigger delay. </summary>
    Public Sub ApplyTriggerDelay()
        If Me.SenseResistanceSubsystem IsNot Nothing AndAlso
                Not Nullable.Equals(Me.TriggerSubsystem.Delay, Me.TriggerDelay) Then
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Applying trigger delay {0};. ", Me.TriggerDelay)
            Me.TriggerSubsystem.ApplyDelay(Me.TriggerDelay)
            Me.SenseResistanceSubsystem.ReadRegisters()
        End If
    End Sub

    ''' <summary> Applies the settings. </summary>
    Public Sub ApplySettings()
        If Me.MeterRange <= 2000000.0 Then
            Me.SenseResistanceSubsystem = Me.FourWireResistanceSenseSubsystem
            Me.SenseSubsystem.ApplyFunctionMode(VI.Scpi.SenseFunctionModes.FourWireResistance)
        Else
            Me.SenseResistanceSubsystem = Me.ResistanceSenseSubsystem
            Me.SenseSubsystem.ApplyFunctionMode(VI.Scpi.SenseFunctionModes.Resistance)
        End If
        Me.ApplyMeterRangeMode()
        Me.ApplyTriggerDelay()
    End Sub

#End Region

#Region " RANGE "

    ''' <summary> List range modes. </summary>
    Public Sub ListRangeModes()
        ' populate the range mode selector
        With Me._MeterRangeComboBox
            .Enabled = False
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(K2000.ResistanceRangeMode).ValueDescriptionPairs
            .ValueMember = "Key"
            .DisplayMember = "Value"
            .Enabled = True
        End With
        Me.SelectMeterRange(Me.SelectedMeterRange)
    End Sub

    ''' <summary> List range modes. </summary>
    ''' <param name="values"> The values. </param>
    Public Sub ListRangeModes(ByVal values As ResistanceRangeMode())
        Dim keyValuePairs As ArrayList = New ArrayList()
        If values IsNot Nothing AndAlso values.Count > 0 Then
            For Each value As ResistanceRangeMode In values
                keyValuePairs.Add(value.ValueDescriptionPair())
            Next
        End If
        With Me._MeterRangeComboBox
            .Enabled = False
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = keyValuePairs
            .ValueMember = "Key"
            .DisplayMember = "Value"
            .Enabled = True
        End With
        Me.SelectMeterRange(Me.SelectedMeterRange)
    End Sub

    ''' <summary> List all range modes other than auto range. </summary>
    Public Sub ListNonAutoRangeModes()
        Dim keyValuePairs As ArrayList = New ArrayList()
        For Each value As ResistanceRangeMode In [Enum].GetValues(GetType(ResistanceRangeMode))
            If value <> ResistanceRangeMode.R0 Then
                keyValuePairs.Add(value.ValueDescriptionPair())
            End If
        Next
        With Me._MeterRangeComboBox
            .Enabled = False
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = keyValuePairs
            .ValueMember = "Key"
            .DisplayMember = "Value"
            .Enabled = True
        End With
        Me.SelectMeterRange(Me.SelectedMeterRange)
    End Sub

    ''' <summary> Gets the selected meter range. </summary>
    ''' <value> The selected meter range. </value>
    Public ReadOnly Property SelectedMeterRange As ResistanceRangeMode
        Get
            Return CType(CType(Me._MeterRangeComboBox.SelectedItem, KeyValuePair(Of System.Enum, String)).Key, ResistanceRangeMode)
        End Get
    End Property

    ''' <summary> Selects the meter range based on the range mode description. </summary>
    ''' <param name="description"> The description. </param>
    Public Sub SelectMeterRange(ByVal description As String)
        Me._MeterRangeComboBox.SafeSilentSelectItem(description)
        Me._MeterRangeComboBox.Refresh()
        Windows.Forms.Application.DoEvents()
        Me.SetMeterRange(Me.SelectedMeterRange)
        For i As Integer = 1 To 10 : Windows.Forms.Application.DoEvents() : Next
        Me.MeterCurrent = K2000.SenseResistanceSubsystem.RangeCurrent(Me.MeterRange)
    End Sub

    ''' <summary> Selects the meter range based on the range mode. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub SelectMeterRange(ByVal value As ResistanceRangeMode)
        Me._MeterRangeComboBox.SafeSilentSelectValue(value)
        Me._MeterRangeComboBox.Refresh()
        Windows.Forms.Application.DoEvents()
        Me.SetMeterRange(value)
        For i As Integer = 1 To 10 : Windows.Forms.Application.DoEvents() : Next
        Me.MeterCurrent = K2000.SenseResistanceSubsystem.RangeCurrent(Me.MeterRange)
    End Sub

    ''' <summary> Selects the meter range based on the range settings. </summary>
    ''' <param name="range">   The range. </param>
    Public Sub SelectMeterRange(ByVal range As Double)
        Dim resistanceRangeMode As ResistanceRangeMode = ResistanceRangeMode.R0
        If K2000.SenseResistanceSubsystem.TryMatch(range, resistanceRangeMode) Then
            Me.SelectMeterRange(resistanceRangeMode)
        End If
    End Sub

    ''' <summary> Gets or sets the meter current. </summary>
    ''' <value> The meter current. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property MeterCurrent As Decimal
        Get
            Return Me._MeterCurrentNumeric.Value
        End Get
        Set(value As Decimal)
            If Not Decimal.Equals(value, Me.MeterCurrent) Then
                Me._MeterCurrentNumeric.SafeSilentValueSetter(value)
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the meter range. </summary>
    ''' <value> The meter range. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property MeterRange As Decimal
        Get
            Return Me._MeterRangeNumeric.Value
        End Get
        Set(value As Decimal)
            If Not Decimal.Equals(value, Me.MeterRange) Then
                Me._MeterRangeNumeric.SafeSilentValueSetter(value)
                Me.SafePostPropertyChanged()
                'select a subsystem based on the range.
                If value <= 2000000.0 Then
                    Me.SenseResistanceSubsystem = Me.FourWireResistanceSenseSubsystem
                Else
                    Me.SenseResistanceSubsystem = Me.ResistanceSenseSubsystem
                End If
                If Me.SenseResistanceSubsystem IsNot Nothing Then
                    ' if both range and current change, binding causes the second bound value to restore to its previous value!
                    ' the code below allows the first binding event to complete before issuing the change on the second
                    ' binding event.
                    For i As Integer = 1 To 10 : Windows.Forms.Application.DoEvents() : Next
                    Me.MeterCurrent = Me.SenseResistanceSubsystem.Current
                Else
                    For i As Integer = 1 To 10 : Windows.Forms.Application.DoEvents() : Next
                    Me.MeterCurrent = K2000.SenseResistanceSubsystem.RangeCurrent(value)
                End If
            End If
        End Set
    End Property

    ''' <summary> Sets meter range. </summary>
    ''' <param name="value"> The value. </param>
    Private Sub SetMeterRange(ByVal value As ResistanceRangeMode)
        Dim r As Double
        If K2000.SenseResistanceSubsystem.TryConvert(value, r) Then
            Me.MeterRange = CDec(r)
        End If
    End Sub

    ''' <summary> Gets or sets the range selection as read only. </summary>
    ''' <value> The sentinel indicating if the meter range is read only. </value>
    Public Property RangeReadOnly As Boolean
        Get
            Return Me._MeterRangeComboBox.ReadOnly
        End Get
        Set(value As Boolean)
            Me._MeterRangeComboBox.ReadOnly = value
        End Set
    End Property

    ''' <summary> Event handler. Called by _MeterRangeComboBox for selected value changed events. </summary>
    ''' <param name="sender"> The source of the event. </param>
    ''' <param name="e">      Event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _MeterRangeComboBox_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MeterRangeComboBox.SelectedValueChanged
        If Me._MeterRangeComboBox.Enabled Then
            Dim value As ResistanceRangeMode = ResistanceRangeMode.R0
            value = CType([Enum].Parse(GetType(ResistanceRangeMode), Me._MeterRangeComboBox.SelectedValue.ToString), ResistanceRangeMode)
            Me.SetMeterRange(value)
        End If
    End Sub

#End Region

#Region " TRIGGER "

    Private _TriggerDelay As TimeSpan
    ''' <summary> Gets or sets the Trigger Delay. </summary>
    ''' <value> The Trigger Delay. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property TriggerDelay As TimeSpan
        Get
            Return _TriggerDelay
        End Get
        Set(value As TimeSpan)
            If Not Decimal.Equals(value, Me.TriggerDelay) Then
                Me._TriggerDelay = value
                Me._TriggerDelayNumeric.SafeSilentValueSetter(value.TotalMilliseconds)
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Event handler. Called by _TriggerDelayNumeric for value changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _TriggerDelayNumeric_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _TriggerDelayNumeric.ValueChanged
        If Me._TriggerDelayNumeric.Enabled Then
            Me.TriggerDelay = TimeSpan.FromMilliseconds(Me._TriggerDelayNumeric.Value)
        End If
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds a listener. </summary>
    ''' <param name="listener"> The listener. </param>
    Public Overrides Sub AddListener(ByVal listener As isr.Core.Pith.IMessageListener)
        MyBase.AddListener(listener)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overrides Sub AddListeners(ByVal listeners As IEnumerable(Of isr.Core.Pith.IMessageListener))
        MyBase.AddListeners(listeners)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

    ''' <summary> Adds the listeners. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Overrides Sub AddListeners(ByVal talker As ITraceMessageTalker)
        MyBase.AddListeners(talker)
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region

	
End Class
