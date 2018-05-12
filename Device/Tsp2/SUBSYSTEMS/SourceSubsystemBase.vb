Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary>  Defines the contract that must be implemented by a Source Current Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class SourceSubsystemBase
    Inherits VI.SourceSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.Tsp2.StatusSubsystemBase">status subsystem</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
    End Sub

    ''' <summary> Sets values to their known execution reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Level = 0
        Me.Limit = 0.000105
        Me.Range = New Double?
        Me.AutoRangeEnabled = True
        Me.AutoDelayEnabled = True
        With Me.FunctionModeDecimalPlaces
            .Clear()
            For Each fmode As VI.Tsp2.SourceFunctionMode In [Enum].GetValues(GetType(VI.Tsp2.SourceFunctionMode))
                .Add(fmode, Me.DefaultFunctionModeDecimalPlaces)
            Next
        End With
        Me.SafePostPropertyChanged(NameOf(SourceSubsystemBase.FunctionModeDecimalPlaces))
        With Me.FunctionModeRanges
            .Clear()
            For Each fmode As VI.Tsp2.SourceFunctionMode In [Enum].GetValues(GetType(VI.Tsp2.SourceFunctionMode))
                .Add(fmode, Core.Pith.RangeR.Full)
            Next
            .Item(VI.Tsp2.SourceFunctionMode.CurrentDC).SetRange(-1.05, 1.05)
            .Item(VI.Tsp2.SourceFunctionMode.VoltageDC).SetRange(-210, 210)
        End With
        Me.SafePostPropertyChanged(NameOf(SourceSubsystemBase.FunctionModeRanges))
        With Me.FunctionModeUnits
            .Clear()
            For Each fmode As VI.Tsp2.SourceFunctionMode In [Enum].GetValues(GetType(VI.Tsp2.SourceFunctionMode))
                .Add(fmode, Arebis.StandardUnits.UnitlessUnits.Ratio)
            Next
            .Item(VI.Tsp2.SourceFunctionMode.CurrentDC) = Arebis.StandardUnits.ElectricUnits.Ampere
            .Item(VI.Tsp2.SourceFunctionMode.VoltageDC) = Arebis.StandardUnits.ElectricUnits.Volt
        End With
        Me.SafePostPropertyChanged(NameOf(SourceSubsystemBase.FunctionModeUnits))
        Me.FunctionMode = VI.Tsp2.SourceFunctionMode.VoltageDC
        Me.Range = 0.02
        Me.LimitTripped = False
        Me.OutputEnabled = False
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

#Region " AUTO RANGE STATE "

    ''' <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Range Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Overrides Property AutoRangeEnabled As Boolean?
        Get
            Return MyBase.AutoRangeEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoRangeEnabled, value) Then
                MyBase.AutoRangeEnabled = value
                If value.HasValue Then
                    Me.AutoRangeState = If(value.Value, OnOffState.On, OnOffState.Off)
                Else
                    Me.AutoRangeState = New OnOffState?
                End If
            End If
        End Set
    End Property

    Private _AutoRangeState As OnOffState?

    ''' <summary> Gets or sets the Auto Range. </summary>
    Public Property AutoRangeState() As OnOffState?
        Get
            Return Me._AutoRangeState
        End Get
        Protected Set(ByVal value As OnOffState?)
            If Not Nullable.Equals(value, Me.AutoRangeState) Then
                Me._AutoRangeState = value
                If value.HasValue Then
                    Me.AutoRangeEnabled = (value.Value = OnOffState.On)
                Else
                    Me.AutoRangeEnabled = New Boolean?
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the AutoRange state. </summary>
    Public Function ApplyAutoRangeState(ByVal value As OnOffState) As OnOffState?
        Me.WriteAutoRangeState(value)
        Return Me.QueryAutoRangeState()
    End Function

    ''' <summary> Gets or sets the Auto Range state query command. </summary>
    ''' <value> The Auto Range state query command. </value>
    Protected Overridable ReadOnly Property AutoRangeStateQueryCommand As String = "_G.smu.measure.autorange"

    ''' <summary> Queries automatic range state. </summary>
    ''' <returns> The automatic range state. </returns>
    Public Function QueryAutoRangeState() As OnOffState?
        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Reading {NameOf(AutoRangeState)};. ")
        Me.Session.LastNodeNumber = New Integer?
        Dim mode As String = Me.AutoRangeState.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.AutoRangeStateQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(AutoRangeState)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.AutoRangeState = New OnOffState?
        Else
            Dim se As New StringEnumerator(Of OnOffState)
            Me.AutoRangeState = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.AutoRangeState
    End Function

    ''' <summary> The Auto Range state command format. </summary>
    Protected Overridable ReadOnly Property AutoRangeStateCommandFormat As String = "_G.smu.measure.autorange={0}"

    ''' <summary> Writes an automatic range state. </summary>
    ''' <param name="value"> The Aperture. </param>
    ''' <returns> An OnOffState. </returns>
    Public Function WriteAutoRangeState(ByVal value As OnOffState) As OnOffState
        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Writing {NameOf(AutoRangeState)}={value};. ")
        Me.Session.LastNodeNumber = New Integer?
        Me.Session.WriteLine(Me.AutoRangeStateCommandFormat, value.ExtractBetween)
        Me.AutoRangeState = value
        Return value
    End Function

#End Region

#Region " FUNCTION MODE "

    ''' <summary> The function mode. </summary>
    Private _FunctionMode As SourceFunctionMode?

    ''' <summary> Writes and reads back the Source Function Mode. </summary>
    ''' <param name="value"> The  Source Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function Mode</see> or none if unknown. </returns>
    Public Function ApplyFunctionMode(ByVal value As SourceFunctionMode) As SourceFunctionMode?
        Me.WriteFunctionMode(value)
        ' changing the function mode changes range, auto delay mode and open detector enabled. 
        Me.QueryRange()
        Me.QueryLevel()
        Return FunctionMode
    End Function

    ''' <summary> Gets or sets the cached function mode. </summary>
    ''' <value> The <see cref="SourceFunctionMode">Source Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FunctionMode As SourceFunctionMode?
        Get
            Return Me._FunctionMode
        End Get
        Protected Set(ByVal value As SourceFunctionMode?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                Me._FunctionMode = value
                If value.HasValue Then
                    Me.FunctionUnit = Me.ToUnit(value.Value)
                    Me.FunctionRange = Me.ToRange(value.Value)
                Else
                    Me.FunctionRange = Me.DefaultFunctionRange
                    Me.FunctionRangeDecimalPlaces = Me.DefaultFunctionModeDecimalPlaces
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command. </value>
    Protected Overridable ReadOnly Property FunctionModeQueryCommand As String = "_G.smu.Source.func"

    ''' <summary> Queries the Source Function Mode. </summary>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function Mode</see> or none if unknown. </returns>
    Public Overridable Function QueryFunctionMode() As SourceFunctionMode?
        Dim mode As String = Me.FunctionMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(mode)
        mode = Me.Session.QueryPrintTrimEnd(Me.FunctionModeQueryCommand)
        If String.IsNullOrWhiteSpace(mode) Then
            Dim message As String = $"Failed fetching {NameOf(SourceSubsystemBase)}.{NameOf(FunctionMode)}"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.FunctionMode = New SourceFunctionMode?
        Else
            Dim se As New StringEnumerator(Of SourceFunctionMode)
            Me.FunctionMode = se.ParseContained(mode.BuildDelimitedValue)
        End If
        Return Me.FunctionMode
    End Function

    ''' <summary> Gets or sets the function mode command format. </summary>
    ''' <value> The function mode command format. </value>
    Protected Overridable ReadOnly Property FunctionModeCommandFormat As String = "_G.smu.Source.func={0}"

    ''' <summary> Writes the Source Function Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Function Mode. </param>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function Mode</see> or none if unknown. </returns>
    Public Overridable Function WriteFunctionMode(ByVal value As SourceFunctionMode) As SourceFunctionMode?
        Me.Session.WriteLine(Me.FunctionModeCommandFormat, value.ExtractBetween())
        Me.FunctionMode = value
        Return Me.FunctionMode
    End Function

#End Region

#Region " SYNTAX "

#Region " LIMIT "

    Private Const currentLimitFunction As String = "i"
    Private Const voltageLimitFunction As String = "v"

    ''' <summary> Limit function mode. </summary>
    ''' <returns> A String. </returns>
    Private Function LimitFunctionMode() As String
        Return If(Me.FunctionMode.Value = SourceFunctionMode.CurrentDC, SourceSubsystemBase.voltageLimitFunction, SourceSubsystemBase.currentLimitFunction)
    End Function

    ''' <summary> Gets the limit query command format. </summary>
    ''' <value> The limit query command format. </value>
    Protected Overridable ReadOnly Property LimitQueryCommandFormat As String

    ''' <summary> Gets the limit query command. </summary>
    ''' <value> The limit query command. </value>
    Protected Overrides ReadOnly Property ModalityLimitQueryCommandFormat As String
        Get
            Const printFormat As Decimal = 9.6D
            Dim tspCommand As String = String.Format(LimitQueryCommandFormat, Me.LimitFunctionMode)
            Return String.Format(LuaSyntax.PrintCommandStringFormat, printFormat, tspCommand)
        End Get
    End Property

    ''' <summary> Gets the limit command format. </summary>
    ''' <value> The limit command format. </value>
    Protected Overridable ReadOnly Property LimitCommandFormat As String

    ''' <summary> Gets the modality limit command format. </summary>
    ''' <value> The modality limit command format. </value>
    Protected Overrides ReadOnly Property ModalityLimitCommandFormat As String
        Get
            Return String.Format(Me.LimitCommandFormat, Me.LimitFunctionMode, "{0}")
        End Get
    End Property

#End Region

#Region " LIMIT TRIPPED "

    ''' <summary> Gets the limit tripped query command format. </summary>
    ''' <value> The limit tripped query command format. </value>
    Protected Overridable ReadOnly Property LimitTrippedPrintCommandFormat As String

    ''' <summary> Gets the limit tripped print command. </summary>
    ''' <value> The limit tripped print command. </value>
    Protected Overrides ReadOnly Property LimitTrippedQueryCommand As String
        Get
            Return String.Format(Me.LimitTrippedPrintCommandFormat, Me.LimitFunctionMode)
        End Get
    End Property

#End Region


#End Region

End Class

''' <summary> Specifies the function modes. </summary>
Public Enum SourceFunctionMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("DC Voltage (smu.FUNC_DC_VOLTAGE)")> VoltageDC
    <ComponentModel.Description("DC Current (smu.FUNC_DC_CURRENT)")> CurrentDC
End Enum

