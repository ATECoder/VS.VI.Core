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
    Inherits SourceMeasureUnitBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.Tsp2.StatusSubsystemBase">status subsystem</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.Tsp2.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets values to their known execution reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Level = 0.105
        Me.VoltageLimit = 0.000105
        Me.Range = New Double?
        Me.AutoRangeEnabled = True
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

#End Region

#Region " AUTO RANGE ENABLED "

    ''' <summary> Auto Range enabled. </summary>
    Private _AutoRangeEnabled As Boolean?

    ''' <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Auto Range Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property AutoRangeEnabled As Boolean?
        Get
            Return Me._AutoRangeEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoRangeEnabled, value) Then
                Me._AutoRangeEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes the enabled state of the current Auto Range and reads back the value from the
    ''' device. </summary>
    ''' <remarks> This command enables or disables the over-current Auto Range (OCP)
    ''' function. The enabled state is On (1); the disabled state is Off (0). If the over-current
    ''' AutoRange function is enabled and the output goes into constant current operation, the output
    ''' is disabled and OCP is set in the Questionable Condition status register. The *RST value =
    ''' Off. </remarks>
    ''' <param name="value"> Enable if set to <c>true</c>; otherwise, disable. </param>
    ''' <returns> <c>True</c> if <see cref="AutoRangeEnabled">Auto Range Enabled</see>;
    '''           <c>False</c> otherwise. </returns>
    Public Function ApplyAutoRangeEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoRangeEnabled(value)
        Return Me.QueryAutoRangeEnabled()
    End Function

    ''' <summary> Queries the current AutoRange state. </summary>
    ''' <returns> True if the AutoRange is on; Otherwise, False. </returns>
    Public Function QueryAutoRangeEnabled() As Boolean?
        Me.AutoRangeEnabled = Me.Session.QueryPrint(Me.AutoRangeEnabled.GetValueOrDefault(True),
                                                    "{0}.source.rangei", Me.SourceMeasureUnitReference)
        Return Me.AutoRangeEnabled
    End Function

    ''' <summary> Writes the enabled state of the current Auto Range without reading back the value from
    ''' the device. </summary>
    ''' <remarks> This command enables or disables the over-current AutoRange (OCP)
    ''' function. The enabled state is On (1); the disabled state is Off (0). If the over-current
    ''' AutoRange function is enabled and the output goes into constant current operation, the output
    ''' is disabled and OCP is set in the Questionable Condition status register. The *RST value =
    ''' Off. </remarks>
    ''' <param name="value"> Enable if set to <c>true</c>; otherwise, disable. </param>
    ''' <returns> <c>True</c> if <see cref="AutoRangeEnabled">Auto Range Enabled</see>;
    '''           <c>False</c> otherwise. </returns>
    Public Function WriteAutoRangeEnabled(ByVal value As Boolean) As Boolean?
        Me.Session.WriteLine(String.Format(Globalization.CultureInfo.InvariantCulture,
                                           "{0}.source.rangei = {{0:'1';'1';'0'}} ", Me.SourceMeasureUnitReference),
                                       CType(value, Integer))
        Me.AutoRangeEnabled = value
        Return Me.AutoRangeEnabled
    End Function

#End Region

#Region " LEVEL "

    ''' <summary> The level. </summary>
    Private _Level As Double?

    ''' <summary> Gets or sets the cached Source Current Level. </summary>
    ''' <value> The Source Current Level. Actual current depends on the power supply mode. </value>
    Public Overloads Property Level As Double?
        Get
            Return Me._Level
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Level, value) Then
                Me._Level = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source current level. </summary>
    ''' <remarks> This command set the immediate output current level. The value is in Amperes. The
    ''' immediate level is the output current setting. At *RST, the current values = 0. </remarks>
    ''' <param name="value"> The current level. </param>
    ''' <returns> The Source Current Level. </returns>
    Public Function ApplyLevel(ByVal value As Double) As Double?
        Me.WriteLevel(value)
        Return Me.QueryLevel
    End Function

    ''' <summary> Queries the current level. </summary>
    ''' <returns> The current level or none if unknown. </returns>
    Public Function QueryLevel() As Double?
        Const printFormat As Decimal = 9.6D
        Me.Level = Me.Session.QueryPrint(Me.Level.GetValueOrDefault(0), printFormat, "{0}.source.leveli", Me.SourceMeasureUnitReference)
        Return Me.Level
    End Function

    ''' <summary> Writes the source current level without reading back the value from the device. </summary>
    ''' <remarks> This command sets the immediate output current level. The value is in Amperes. The
    ''' immediate level is the output current setting. At *RST, the current values = 0. </remarks>
    ''' <param name="value"> The current level. </param>
    ''' <returns> The Source Current Level. </returns>
    Public Function WriteLevel(ByVal value As Double) As Double?
        Me.Session.WriteLine("{0}.source.leveli={1}", Me.SourceMeasureUnitReference, value)
        Me.Level = value
        Return Me.Level
    End Function

#End Region

#Region " RANGE "

    ''' <summary> The Current Range. </summary>
    Private _Range As Double?

    ''' <summary> Gets or sets the cached Source current range. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Range As Double?
        Get
            Return Me._Range
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Range, value) Then
                Me._Range = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Source current Range. </summary>
    ''' <remarks> The value is in Amperes. At *RST, the range is set to Auto and the specific range is unknown. </remarks>
    ''' <param name="value"> The current Range. </param>
    ''' <returns> The Current Range. </returns>
    Public Function ApplyRange(ByVal value As Double) As Double?
        Me.WriteRange(value)
        Return Me.QueryRange
    End Function

    ''' <summary> Queries the current Range. </summary>
    ''' <returns> The current Range or none if unknown. </returns>
    Public Function QueryRange() As Double?
        Const printFormat As Decimal = 9.6D
        Me.Range = Me.Session.QueryPrint(Me.Range.GetValueOrDefault(0.99), printFormat, "{0}.source.rangei", Me.SourceMeasureUnitReference)
        Return Me.Range
    End Function

    ''' <summary> Writes the Source current Range without reading back the value from the device. </summary>
    ''' <remarks> This command sets the current Range. The value is in Amperes. 
    '''           At *RST, the range is auto and the value is not known. </remarks>
    ''' <param name="value"> The Source current Range. </param>
    ''' <returns> The Source Current Range. </returns>
    Public Function WriteRange(ByVal value As Double) As Double?
        If value >= (Scpi.Syntax.Infinity - 1) Then
            Me.Session.WriteLine("{0}.source.rangei={0}.source.rangei.max", Me.SourceMeasureUnitReference)
            value = Scpi.Syntax.Infinity
        ElseIf value <= (Scpi.Syntax.NegativeInfinity + 1) Then
            Me.Session.WriteLine("{0}.source.rangei={0}.source.rangei.min", Me.SourceMeasureUnitReference)
            value = Scpi.Syntax.NegativeInfinity
        Else
            Me.Session.WriteLine("{0}.source.rangei={1}", Me.SourceMeasureUnitReference, value)
        End If
        Me.Range = value
        Return Me.Range
    End Function

#End Region

#Region " VOLTAGE LIMIT "

    ''' <summary> The Voltage Limit. </summary>
    Private _VoltageLimit As Double?

    ''' <summary> Gets or sets the cached source Voltage Limit for a Current Source. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property VoltageLimit As Double?
        Get
            Return Me._VoltageLimit
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.VoltageLimit, value) Then
                Me._VoltageLimit = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Voltage Limit. </summary>
    ''' <remarks> This command set the immediate output Voltage Limit. The value is in Amperes. The
    ''' immediate Limit is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Voltage Limit. </param>
    ''' <returns> The Source Voltage Limit. </returns>
    Public Function ApplyVoltageLimit(ByVal value As Double) As Double?
        Me.WriteVoltageLimit(value)
        Return Me.QueryVoltageLimit()
    End Function

    ''' <summary> Queries the Voltage Limit. </summary>
    ''' <returns> The Voltage Limit or none if unknown. </returns>
    Public Function QueryVoltageLimit() As Double?
        Const printFormat As Decimal = 9.6D
        Me.VoltageLimit = Me.Session.QueryPrint(Me.VoltageLimit.GetValueOrDefault(0.099), printFormat, "{0}.source.limitv", Me.SourceMeasureUnitReference)
        Return Me.VoltageLimit
    End Function

    ''' <summary> Writes the source Voltage Limit without reading back the value from the device. </summary>
    ''' <remarks> This command set the immediate output Voltage Limit. The value is in Amperes. The
    ''' immediate Limit is the output Voltage setting. At *RST, the Voltage values = 0. </remarks>
    ''' <param name="value"> The Voltage Limit. </param>
    ''' <returns> The Source Voltage Limit. </returns>
    Public Function WriteVoltageLimit(ByVal value As Double) As Double?
        If value >= (Scpi.Syntax.Infinity - 1) Then
            Me.Session.WriteLine("{0}.source.limitv={0}.source.limitv.max", Me.SourceMeasureUnitReference)
            value = Scpi.Syntax.Infinity
        ElseIf value <= (Scpi.Syntax.NegativeInfinity + 1) Then
            Me.Session.WriteLine("{0}.source.limitv={0}.source.limitv.min", Me.SourceMeasureUnitReference)
            value = Scpi.Syntax.NegativeInfinity
        Else
            Me.Session.WriteLine("{0}.source.limitv={1}", Me.SourceMeasureUnitReference, value)
        End If
        Me.VoltageLimit = value
        Return Me.VoltageLimit
    End Function

#End Region

End Class

