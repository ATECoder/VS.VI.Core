Imports System.ComponentModel
Imports isr.Core.Controls.ComboBoxExtensions
Imports isr.Core.Controls.NumericUpDownExtensions
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Measure control -- defines the measure subsystem settings. </summary>
''' <license> (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="3/4/2014" by="David" revision=""> Created. </history>
Public Class T1750MeasureControl
    Inherits TalkerControlBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()

        ' This call is required by the designer.
        Me.InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' populate the range mode selector
        Me.ListNonAutoRangeModes()
        ' populate the emulated reply combo.
        Me.ListOneShotTriggerModes()
        Me.SelectMeterTrigger(TriggerMode.T3)

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
                Me._MeasureSubsystem = Nothing
                If Me.components IsNot Nothing Then Me.components.Dispose() : Me.components = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " MEASURE SUBSYSTEM "

    Private WithEvents _MeasureSubsystem As MeasureSubsystem

    ''' <summary> Gets or sets the measure subsystem. </summary>
    ''' <value> The measure subsystem. </value>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property MeasureSubsystem As MeasureSubsystem
        Get
            Return _MeasureSubsystem
        End Get
        Set(value As MeasureSubsystem)
            Me._MeasureSubsystem = value
        End Set
    End Property

    ''' <summary> Handle the Measure subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As MeasureSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(subsystem.RangeMode)
                If subsystem.RangeMode.HasValue Then
                    Me.SelectMeterRange(subsystem.RangeMode.Value.Description)
                End If
            Case NameOf(subsystem.TriggerMode)
                If subsystem.TriggerMode.HasValue Then
                    Me.SelectMeterTrigger(subsystem.TriggerMode.Value.Description)
                End If
            Case NameOf(subsystem.TriggerDelay)
                If subsystem.TriggerDelay.HasValue Then
                    Me.TriggerDelay = subsystem.TriggerDelay.Value
                End If
            Case NameOf(subsystem.InitialDelay)
                Me.InitialDelay = subsystem.InitialDelay
            Case NameOf(subsystem.MeasurementDelay)
                Me.MeasurementDelay = subsystem.MeasurementDelay
            Case NameOf(subsystem.MaximumTrialsCount)
                Me.MaximumTrialsCount = subsystem.MaximumTrialsCount
            Case NameOf(subsystem.MaximumDifference)
                Me.MaximumDifference = CDec(subsystem.MaximumDifference)
        End Select
    End Sub

    ''' <summary> Measure subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub MeasureSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _MeasureSubsystem.PropertyChanged
        Try
            Me.OnSubsystemPropertyChanged(TryCast(sender, MeasureSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

    ''' <summary> Applies the meter trigger mode. </summary>
    Public Sub ApplyMeterTriggerMode()
        If Me.MeasureSubsystem IsNot Nothing AndAlso
                Not Nullable.Equals(Me.MeasureSubsystem.TriggerMode, Me.SelectedMeterTrigger) Then
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Applying meter trigger {0};. ", Me.SelectedMeterTrigger)
            Me.MeasureSubsystem.ApplyTriggerMode(Me.SelectedMeterTrigger)
            Me.MeasureSubsystem.ReadRegisters()
            ' a delay is required between the two settings
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Waiting {0}ms for meter trigger to settle;. ",
                               MeasureSubsystem.RangeSettlingTimeMilliseconds)
            Threading.Thread.Sleep(MeasureSubsystem.RangeSettlingTimeMilliseconds)
        End If

    End Sub

    ''' <summary> Applies the meter range mode. </summary>
    Public Sub ApplyMeterRangeMode()
        If Me.MeasureSubsystem IsNot Nothing AndAlso
                Not Nullable.Equals(Me.MeasureSubsystem.RangeMode, Me.SelectedMeterRange) Then
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Applying meter range settings;. range={0}; current={1}, mode='{2}'",
                               Me._MeterCurrentNumeric.Value, Me._MeterRangeNumeric.Value, Me.SelectedMeterRange)
            Me.MeasureSubsystem.ApplyRangeMode(Me.SelectedMeterRange)
            Me.MeasureSubsystem.ReadRegisters()
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Waiting {0}ms for meter range to settle;. ", MeasureSubsystem.RangeSettlingTimeMilliseconds)
            ' at this point, 1/31/2014, the first reading comes in too quickly. Trying to detect operation completion using bit 16 of the
            ' service register does not work. So we are resorting to a brute force delay.
            Threading.Thread.Sleep(MeasureSubsystem.RangeSettlingTimeMilliseconds)
        End If
    End Sub

    ''' <summary> Applies the meter trigger delay. </summary>
    Public Sub ApplyTriggerDelay()
        If Me.MeasureSubsystem IsNot Nothing AndAlso
                Not Nullable.Equals(Me.MeasureSubsystem.TriggerDelay, Me.TriggerDelay) Then
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Applying trigger delay {0} ms;. ", Me.TriggerDelay.TotalMilliseconds)
            Me.MeasureSubsystem.ApplyTriggerDelay(Me.TriggerDelay)
            Me.MeasureSubsystem.ReadRegisters()
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Waiting {0}ms for meter range to settle;. ", MeasureSubsystem.RangeSettlingTimeMilliseconds)
            ' at this point, 1/31/2014, the first reading comes in too quickly. Trying to detect operation completion using bit 16 of the
            ' service register does not work. So we are resorting to a brute force delay.
            Threading.Thread.Sleep(MeasureSubsystem.RangeSettlingTimeMilliseconds)
        End If
    End Sub

    ''' <summary> Applies the settings. </summary>
    Public Sub ApplySettings()
        Me.ApplyMeterTriggerMode()
        Me.ApplyMeterRangeMode()
        Me.ApplyTriggerDelay()
        If Me.MeasureSubsystem IsNot Nothing Then
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Setting initial delay to {0}ms;. ", Me.InitialDelay.TotalMilliseconds)
            Me.MeasureSubsystem.InitialDelay = Me.InitialDelay
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Setting measurement delay to {0}ms;. ", Me.MeasurementDelay.TotalMilliseconds)
            Me.MeasureSubsystem.MeasurementDelay = Me.MeasurementDelay
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Setting maximum trial count to {0};. ", Me.MaximumTrialsCount)
            Me.MeasureSubsystem.MaximumTrialsCount = Me.MaximumTrialsCount
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId,
                               "Setting maximum difference to {0}%;. ", 100 * Me.MaximumDifference)
            Me.MeasureSubsystem.MaximumDifference = Me.MaximumDifference
        End If
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
            .DataSource = GetType(RangeMode).ValueDescriptionPairs
            .ValueMember = "Key"
            .DisplayMember = "Value"
            .Enabled = True
        End With
    End Sub

    ''' <summary> List range modes. </summary>
    ''' <param name="values"> The values. </param>
    Public Sub ListRangeModes(ByVal values As RangeMode())
        Dim keyValuePairs As ArrayList = New ArrayList()
        If values IsNot Nothing AndAlso values.Count > 0 Then
            For Each value As RangeMode In values
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
    End Sub

    ''' <summary> List all range modes other than auto range. </summary>
    Public Sub ListNonAutoRangeModes()
        Dim keyValuePairs As ArrayList = New ArrayList()
        For Each value As RangeMode In [Enum].GetValues(GetType(RangeMode))
            If value <> RangeMode.R0 Then
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
    End Sub

    ''' <summary> Gets the selected meter range. </summary>
    ''' <value> The selected meter range. </value>
    Public ReadOnly Property SelectedMeterRange As RangeMode
        Get
            Return CType(CType(Me._MeterRangeComboBox.SelectedItem, KeyValuePair(Of System.Enum, String)).Key, RangeMode)
        End Get
    End Property

    ''' <summary> Selects the meter range based on the range mode description. </summary>
    ''' <param name="description"> The description. </param>
    Public Sub SelectMeterRange(ByVal description As String)
        Me._MeterRangeComboBox.SafeSilentSelectItem(description)
        Me._MeterRangeComboBox.Refresh()
        Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary> Selects the meter range based on the range mode. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub SelectMeterRange(ByVal value As RangeMode)
        Me._MeterRangeComboBox.SafeSilentSelectValue(value)
        Me._MeterRangeComboBox.Refresh()
        Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary> Selects the meter range based on the current and range settings. </summary>
    ''' <param name="current"> The current. </param>
    ''' <param name="range">   The range. </param>
    Public Sub SelectMeterRange(ByVal current As Double, ByVal range As Double)
        Dim rangeMode As RangeMode = RangeMode.R0
        If MeasureSubsystem.TryMatch(current, range, rangeMode) Then
            Me._MeterRangeComboBox.SafeSilentSelectValue(rangeMode)
            Me._MeterRangeComboBox.Refresh()
            Windows.Forms.Application.DoEvents()
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
                Me.SafePostPropertyChanged(NameOf(Me.MeterCurrent))
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
                Me.SafePostPropertyChanged(NameOf(Me.MeterRange))
            End If
        End Set
    End Property

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
            Dim range As RangeMode = RangeMode.R0
            range = CType([Enum].Parse(GetType(RangeMode), Me._MeterRangeComboBox.SelectedValue.ToString), RangeMode)
            Dim c, r As Double
            If MeasureSubsystem.TryParse(range, c, r) Then
                Me.MeterCurrent = CDec(c)
                ' if both range and current change, binding cause the range to restore to its previous value!
                ' the code below allows the first binding event to complete before issuing the change on the second
                ' binding event.
                For i As Integer = 1 To 10 : Windows.Forms.Application.DoEvents() : Next
                Me.MeterRange = CDec(r)
            End If
        End If
    End Sub

#End Region

#Region " TRIGGER "

    ''' <summary> List trigger modes. </summary>
    Public Sub ListTriggerModes()
        ' populate the emulated reply combo. 
        With Me._TriggerCombo
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(TriggerMode).ValueDescriptionPairs
            .SelectedIndex = 0
            .ValueMember = "Key"
            .DisplayMember = "Value"
        End With
    End Sub

    ''' <summary> List one shot trigger modes. </summary>
    Public Sub ListOneShotTriggerModes()
        Me.ListTriggerModes(New TriggerMode() {TriggerMode.T1, TriggerMode.T3})
    End Sub

    ''' <summary> List trigger modes. </summary>
    ''' <param name="values"> The values. </param>
    Public Sub ListTriggerModes(ByVal values As TriggerMode())

        Dim keyValuePairs As ArrayList = New ArrayList()
        If values IsNot Nothing AndAlso values.Count > 0 Then
            For Each value As TriggerMode In values
                keyValuePairs.Add(value.ValueDescriptionPair())
            Next
        End If

        ' populate the emulated reply combo.
        With Me._TriggerCombo
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = keyValuePairs
            .SelectedIndex = 0
            .ValueMember = "Key"
            .DisplayMember = "Value"
        End With

    End Sub

    ''' <summary> Gets the selected meter Trigger Mode. </summary>
    ''' <value> The selected meter Trigger. </value>
    Public ReadOnly Property SelectedMeterTrigger As TriggerMode
        Get
            Return CType(CType(Me._TriggerCombo.SelectedItem, KeyValuePair(Of System.Enum, String)).Key, TriggerMode)
        End Get
    End Property

    ''' <summary> Selects the meter Trigger mode based on the Trigger mode description. </summary>
    ''' <param name="description"> The description. </param>
    Public Sub SelectMeterTrigger(ByVal description As String)
        Me._TriggerCombo.SafeSilentSelectItem(description)
        Me._TriggerCombo.Refresh()
        Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary> Selects the meter Trigger mode based on the Trigger mode. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub SelectMeterTrigger(ByVal value As TriggerMode)
        Me._TriggerCombo.SafeSilentSelectValue(value)
        Me._TriggerCombo.Refresh()
        Windows.Forms.Application.DoEvents()
    End Sub

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
                Me.SafePostPropertyChanged(NameOf(Me.TriggerDelay))
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

#Region " VALUES "

    Private _InitialDelay As TimeSpan
    ''' <summary> Gets or sets the initial delay. </summary>
    ''' <value> The initial delay. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property InitialDelay As TimeSpan
        Get
            Return _InitialDelay
        End Get
        Set(value As TimeSpan)
            If Not TimeSpan.Equals(value, Me.InitialDelay) Then
                Me._InitialDelay = value
                With Me._InitialDelayNumeric
                    .Value = CDec(.SafeSilentValueSetter(value.TotalMilliseconds))
                End With
                If Me.MeasureSubsystem IsNot Nothing Then
                    Me.MeasureSubsystem.InitialDelay = value
                End If
                Me.SafeSendPropertyChanged(NameOf(Me.InitialDelay))
                ' For i As Integer = 1 To 10 : Windows.Forms.Application.DoEvents() : Next
            End If
        End Set
    End Property

    ''' <summary> Event handler. Called by _InitialDelayNumeric for value changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _InitialDelayNumeric_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _InitialDelayNumeric.ValueChanged
        If Me._InitialDelayNumeric.Enabled Then
            Me.InitialDelay = TimeSpan.FromMilliseconds(Me._InitialDelayNumeric.Value)
        End If
    End Sub

    Private _MeasurementDelay As TimeSpan
    ''' <summary> Gets or sets the Measurement delay. </summary>
    ''' <value> The Measurement delay. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property MeasurementDelay As TimeSpan
        Get
            Return Me._MeasurementDelay
        End Get
        Set(value As TimeSpan)
            If Not TimeSpan.Equals(value, Me.MeasurementDelay) Then
                Me._MeasurementDelay = value
                With Me._MeasurementDelayNumeric
                    .Value = CDec(.SafeSilentValueSetter(value.TotalMilliseconds))
                End With
                If Me.MeasureSubsystem IsNot Nothing Then
                    Me.MeasureSubsystem.MeasurementDelay = value
                End If
                Me.SafeSendPropertyChanged(NameOf(Me.MeasurementDelay))
                ' For i As Integer = 1 To 10 : Windows.Forms.Application.DoEvents() : Next
            End If
        End Set
    End Property

    ''' <summary> Event handler. Called by _MeasurementDelayNumeric for value changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _MeasurementDelayNumeric_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MeasurementDelayNumeric.ValueChanged
        If Me._MeasurementDelayNumeric.Enabled Then
            Me.MeasurementDelay = TimeSpan.FromMilliseconds(Me._MeasurementDelayNumeric.Value)
        End If
    End Sub

    Private _MaximumTrialsCount As Integer
    ''' <summary> Gets or sets the maximum trial count. </summary>
    ''' <value> The maximum number of trials before giving up. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property MaximumTrialsCount As Integer
        Get
            Return Me._MaximumTrialsCount
        End Get
        Set(value As Integer)
            If Not Integer.Equals(value, Me.MaximumTrialsCount) Then
                Me._MaximumTrialsCount = value
                With Me._MaximumTrialsCountNumeric
                    .Value = CDec(.SafeSilentValueSetter(value))
                End With
                If Me.MeasureSubsystem IsNot Nothing Then
                    Me.MeasureSubsystem.MaximumTrialsCount = value
                End If
                Me.SafeSendPropertyChanged(NameOf(Me.MaximumTrialsCount))
                ' For i As Integer = 1 To 10 : Windows.Forms.Application.DoEvents() : Next
            End If
        End Set
    End Property

    ''' <summary> Event handler. Called by _MaximumTrialsCountNumeric for value changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _MaximumTrialsCountNumeric_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MaximumTrialsCountNumeric.ValueChanged
        If Me._MaximumTrialsCountNumeric.Enabled Then
            Me.MaximumTrialsCount = CInt(Me._MaximumTrialsCountNumeric.Value)
        End If
    End Sub

    Private _MaximumDifference As Decimal
    ''' <summary> Gets or sets the maximum difference between consecutive measurements. </summary>
    ''' <value> The maximum difference between consecutive measurements. </value>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property MaximumDifference As Decimal
        Get
            Return Me._MaximumDifference
        End Get
        Set(value As Decimal)
            If Not Decimal.Equals(value, Me.MaximumDifference) Then
                Me._MaximumDifference = value
                Me._MaximumDifferenceNumeric.SafeSilentValueSetter(100 * value)
                If Me.MeasureSubsystem IsNot Nothing Then
                    Me.MeasureSubsystem.MaximumDifference = Me.MaximumDifference
                End If
                Me.SafeSendPropertyChanged(NameOf(Me.MaximumDifference))
                ' this is required to ensure binding takes place.
                ' For i As Integer = 1 To 10 : Windows.Forms.Application.DoEvents() : Next
            End If
        End Set
    End Property

    ''' <summary> Event handler. Called by _MaximumDifferenceNumeric for value changed events. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Event information. </param>
    Private Sub _MaximumDifferenceNumeric_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MaximumDifferenceNumeric.ValueChanged
        If Me._MaximumDifferenceNumeric.Enabled Then
            Me.MaximumDifference = CDec(0.01 * Me._MaximumDifferenceNumeric.Value)
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
