Imports isr.Core.Pith.NumericExtensions
''' <summary> Holds the measurand meta status and derived properties. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/1/2013" by="David" revision=""> Created. </history>
Public Class MetaStatus

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Contracts this class setting the meta status to 0. </summary>
    Public Sub New()
        MyBase.New()
        Me._ApplyDefaultFailStatusBitmask()
        Me._ApplyDefaultInvalidStatusBitmask()
        Me._Reset()
    End Sub

    ''' <summary> Constructs a copy of an existing value. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As MetaStatus)
        Me.New()
        If model IsNot Nothing Then
            Me._Value = model._Value
            Me._FailStatusBitmask = model.FailStatusBitmask
            Me._InvalidStatusBitmask = model.InvalidStatusBitmask
        End If
    End Sub

#End Region

#Region " METHODS "

    ''' <summary> Clears the meta status. </summary>
    Private Sub _Reset()
        Me._Value = 0
    End Sub

    ''' <summary> Clears the meta status. </summary>
    Public Sub Reset()
        Me._Reset()
    End Sub

    ''' <summary> Presets the meta status. </summary>
    Public Sub Preset(ByVal value As Long)
        Me._Value = value
    End Sub

    ''' <summary> Appends a value. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub Append(ByVal value As Long)
        Me._Value = Me._Value Or value
    End Sub

    ''' <summary> Appends a value. </summary>
    ''' <param name="status"> The status to append. </param>
    Public Sub Append(ByVal status As MetaStatus)
        If status IsNot Nothing AndAlso status.Value <> 0 Then
            Me._Value = Me._Value Or status.Value
        End If
    End Sub

#End Region

#Region " PROPERTIES "

    ''' <summary> Returns a two-character description representing the measurand meta status. </summary>
    ''' <returns> The short meta status description. </returns>
    Public Function TwoCharDescription(ByVal passedValue As String) As String
        Dim result As String = ""
        If Me.Passed Then
            result = passedValue
        ElseIf Not Me.IsSet Then
            ' if not set then return NO
            result = "no"
        ElseIf Not Me.HasValue Then
            ' if no value we are out of luck
            result = "nv"
        ElseIf Not Me.IsValid Then
            result = "iv"
        ElseIf Me.IsHigh Then
            result = "hi"
        ElseIf Me.IsLow Then
            result = "lo"
        ElseIf Me.FailedContactCheck Then
            result = "cc"
        ElseIf Me.HitCompliance Then
            result = "sc" ' status compliance
        ElseIf Me.HitRangeCompliance Then
            result = "rc"
        ElseIf Me.HitLevelCompliance Then
            result = "lc"
        ElseIf Me.HitOverRange Then
            result = "or"
        ElseIf Me.HitVoltageProtection Then
            result = "vp"
        ElseIf Me.Infinity Then
            result = Convert.ToChar(&H221E) ' 236)
        ElseIf Me.NegativeInfinity Then
            result = $"-{Convert.ToChar(&H221E)}"
        Else
            ' if we do not return a value, raise an exception
            Debug.Assert(Not Debugger.IsAttached, "Unhandled case in determining two-char code.")
            result = "na"
        End If
        Return result
    End Function

    ''' <summary> Returns a short description representing the measurand meta status. </summary>
    ''' <returns> The short meta status description. </returns>
    Public Function ToShortDescription(ByVal passedValue As String) As String
        Dim result As String = ""
        If Me.Passed Then
            result = passedValue
        ElseIf Not Me.IsSet Then
            ' if not set then return NO
            result = "~set"
        ElseIf Not Me.HasValue Then
            ' if no value we are out of luck
            result = "mpty"
        ElseIf Not Me.IsValid Then
            result = "~vld"
        ElseIf Me.IsHigh Then
            result = "high"
        ElseIf Me.IsLow Then
            result = "low"
        ElseIf Me.FailedContactCheck Then
            result = "open"
        ElseIf Me.HitCompliance Then
            result = "cmpl" ' status compliance
        ElseIf Me.HitRangeCompliance Then
            result = "rnge"
        ElseIf Me.HitLevelCompliance Then
            result = "levl"
        ElseIf Me.HitOverRange Then
            result = "over"
        ElseIf Me.HitVoltageProtection Then
            result = "volt"
        ElseIf Me.Infinity Then
            result = "inf"
        ElseIf Me.NegativeInfinity Then
            result = "-inf"
        Else
            ' if we do not return a value, raise an exception
            Debug.Assert(Not Debugger.IsAttached, "Unhandled case in determining code.")
            result = "na"
        End If
        Return result
    End Function

    ''' <summary> Returns a long description representing the measurand meta status. </summary>
    ''' <returns> The long  meta status description. </returns>
    Public Function ToLongDescription(ByVal passedValue As String) As String
        Dim result As String = ""
        If Me.Passed Then
            result = passedValue
        ElseIf Not Me.IsSet Then
            result = "Not Set"
        ElseIf Not Me.HasValue Then
            ' if no value we are out of luck
            result = "No Value"
        ElseIf Not Me.IsValid Then
            result = "Invalid"
        ElseIf Me.IsHigh Then
            result = "high"
        ElseIf Me.IsLow Then
            result = "low"
        ElseIf Me.FailedContactCheck Then
            result = "Failed contact check"
        ElseIf Me.HitCompliance Then
            result = "Status Compliance"
        ElseIf Me.HitRangeCompliance Then
            result = "Range compliance"
        ElseIf Me.HitLevelCompliance Then
            result = "Level compliance"
        ElseIf Me.HitOverRange Then
            result = "Over range"
        ElseIf Me.HitVoltageProtection Then
            result = "Voltage protection"
        ElseIf Me.Infinity Then
            result = "infinity"
        ElseIf Me.NegativeInfinity Then
            result = "-infinity"
        Else
            ' if we do not return a value, raise an exception
            Debug.Assert(Not Debugger.IsAttached, "Unhandled case in determining long outcome.")
            result = "not handled"
        End If
        Return result
    End Function

#End Region

#Region " CONDITIONS "

    ''' <summary> Gets or sets the condition determining if the outcome indicates a failed contact
    ''' check. </summary>
    ''' <value> The failed contact check. </value>
    Public Property FailedContactCheck() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.FailedContactCheck)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.FailedContactCheck, value)
            Me.ToggleBit(MetaStatusBit.Valid, Not value)
        End Set
    End Property

    ''' <summary> Returns true if the <see cref="MetaStatus"/> was set. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatus"/> was set; Otherwise, <c>False</c>. </returns>
    Public Function IsSet() As Boolean
        Return Me.Value - (1L << Measurand.MetaStatusBitBase) >= 0
    End Function

    ''' <summary> Gets or sets the condition determining if the outcome indicates a having a value,
    ''' namely if a value was set. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBit.HasValue"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property HasValue() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.HasValue)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.HasValue, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a high value. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBit.High"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property IsHigh() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.High)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.High, value)
        End Set
    End Property

    ''' <summary>
    ''' Gets the condition determining if the outcome indicates a measurement that Hit Compliance.
    ''' The instrument sensed an output hit the compliance limit before being able to make a
    ''' measurement.
    ''' </summary>
    ''' <value> <c>True</c> if hit compliance; Otherwise, <c>False</c>. </value>
    Public ReadOnly Property HitCompliance() As Boolean
        Get
            Return Me.HitLevelCompliance OrElse Me.HitStatusCompliance
        End Get
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that
    ''' hit level compliance. Level compliance is hit if the measured level absolute value is outside
    ''' the compliance limits.  This is a calculated value designed to address cases where the
    ''' compliance bit is in correct. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBit.HitLevelCompliance"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property HitLevelCompliance() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.HitLevelCompliance)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.HitLevelCompliance, value)
            Me.ToggleBit(MetaStatusBit.Valid, Not value)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the condition determining if the outcome indicates a measurement that hit Status
    ''' compliance.
    ''' </summary>
    ''' <remarks>
    ''' When in real compliance, the source clamps at the displayed compliance value. <para>
    ''' For example, if the compliance voltage Is set to 1V And the measurement range Is 2V, output
    ''' voltage will clamp at 1V. In this Case, the “CMPL” annunciator will flash. </para><para>
    ''' Status compliance is reported by the instrument in the status word.</para>
    ''' </remarks>
    ''' <value>
    ''' <c>True</c> if the <see cref="MetaStatusBit.HitStatusCompliance"/> was set; Otherwise,
    ''' <c>False</c>.
    ''' </value>
    Public Property HitStatusCompliance() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.HitStatusCompliance)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.HitStatusCompliance, value)
            Me.ToggleBit(MetaStatusBit.Valid, value)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the condition determining if the outcome indicates a measurement that Hit Range
    ''' Compliance.
    ''' </summary>
    ''' <remarks>
    ''' Range compliance is the maximum possible compliance value for the fixed measurement range.
    ''' Note that range compliance cannot occur if the AUTO measurement range Is selected. Thus, To
    ''' avoid range compliance, use AUTO range. <para>
    ''' When in range compliance, the source output clamps at the maximum compliance value
    '''    For the fixed measurement range (Not the compliance value). For example, if compliance Is
    ''' Set To 1V And the measurement range Is 200mV, output voltage will clamp at 210mV. In
    ''' this situation, the units In the compliance display field will flash. For example, With the
    ''' following display : Vcmpl: 10mA, the “mA” units indication will flash.</para>
    ''' </remarks>
    ''' <value>
    ''' <c>True</c> if the <see cref="MetaStatusBit.HitRangeCompliance"/> was set; Otherwise,
    ''' <c>False</c>.
    ''' </value>
    Public Property HitRangeCompliance() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.HitRangeCompliance)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.HitRangeCompliance, value)
            Me.ToggleBit(MetaStatusBit.Valid, Not value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that
    ''' Hit Voltage Protection. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBit.HitVoltageProtection"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property HitVoltageProtection() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.HitVoltageProtection)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.HitVoltageProtection, value)
            Me.ToggleBit(MetaStatusBit.Valid, Not value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that
    ''' was made while over range. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBit.HitOverRange"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property HitOverRange() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.HitOverRange)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.HitOverRange, value)
            Me.ToggleBit(MetaStatusBit.Valid, Not value)
        End Set
    End Property


    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that is
    ''' low. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBit.Low"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property IsLow() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.Low)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.Low, value)
        End Set
    End Property

    ''' <summary> Is affirmative. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> <c>True</c> if the <paramref name="value"/> is set in the
    ''' <see cref="MetaStatus">meta status</see>; Otherwise, <c>False</c>. </returns>
    Public Function IsBit(ByVal value As MetaStatusBit) As Boolean
        Return Me.Value.IsBit(value)
    End Function

    ''' <summary> Toggles the bits specified in the <paramref name="value"/> based on the
    ''' <paramref name="switch"/>. </summary>
    ''' <param name="value">  The value. </param>
    ''' <param name="switch"> True to set; otherwise, clear. </param>
    Public Sub ToggleBit(ByVal value As MetaStatusBit, ByVal switch As Boolean)
        Me._Value = Me.Value.Toggle(value, switch)
        If Not (value = MetaStatusBit.Pass OrElse value = MetaStatusBit.HasValue) Then
            Me._Value = Me.Value.Toggle(MetaStatusBit.Pass, Me.Passed)
            Me._Value = Me.Value.Toggle(MetaStatusBit.HasValue, True)
        End If
    End Sub

    ''' <summary> Gets or sets the condition determining if the outcome is Pass. </summary>
    ''' <value> <c>True</c> if the <see cref="MetaStatusBit.Pass"/> was set; Otherwise, <c>False</c>. </value>
    Public Property IsPass() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.Pass)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.Pass, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome is Valid. The measured value is
    ''' valid when its value or pass/fail outcomes or compliance or contact check conditions are set. </summary>
    ''' <value> <c>True</c> if the <see cref="MetaStatusBit.Valid"/> was set; Otherwise, <c>False</c>. </value>
    Public Property IsValid() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.Valid)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.Valid, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that is
    ''' infinity. </summary>
    ''' <value> <c>True</c> if the <see cref="MetaStatusBit.Infinity"/> was set; Otherwise, <c>False</c>. </value>
    Public Property Infinity() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.Infinity)
        End Get
        Set(ByVal value As Boolean)
            ToggleBit(MetaStatusBit.Infinity, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that is
    ''' not a number. </summary>
    ''' <value> <c>True</c> if the <see cref="MetaStatusBit.NotANumber"/> was set; Otherwise, <c>False</c>. </value>
    Public Property NotANumber() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.NotANumber)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.NotANumber, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that is
    ''' negative infinity. </summary>
    ''' <value> <c>True</c> if the <see cref="MetaStatusBit.NegativeInfinity"/> was set; Otherwise, <c>False</c>. </value>
    Public Property NegativeInfinity() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.NegativeInfinity)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.NegativeInfinity, value)
        End Set
    End Property

    ''' <summary> Gets the fail status bitmask. </summary>
    ''' <value> The fail status bitmask. </value>
    Public Property FailStatusBitmask As Long

    ''' <summary> Applies the default fail status bitmask. </summary>
    Private Sub _ApplyDefaultFailStatusBitmask()
        Me._FailStatusBitmask = 1L << MetaStatusBit.Low Or
                                1L << MetaStatusBit.High
    End Sub

    ''' <summary> Applies the default fail status bitmask. </summary>
    Public Sub ApplyDefaultFailStatusBitmask()
        Me._ApplyDefaultFailStatusBitmask()
    End Sub

    ''' <summary> Returns True if valid and passed. </summary>
    ''' <retuns> <c>True</c> if the status <see cref="IsValid"/> and 
    '''          not <see cref="IsLow"/> and
    '''          not <see cref="IsHigh"/> and
    '''          </retuns>
    Public Function Passed() As Boolean
        Return Me.Valid AndAlso (Me.Value And Me.FailStatusBitmask) = 0
    End Function

    ''' <summary> Gets the invalid status bitmask. </summary>
    ''' <value> The invalid status bitmask. </value>
    Public Property InvalidStatusBitmask As Long

    ''' <summary> Applies the default invalid status bitmask. </summary>
    Private Sub _ApplyDefaultInvalidStatusBitmask()
        Me._InvalidStatusBitmask = 1L << MetaStatusBit.NotANumber Or
                                   1L << MetaStatusBit.Infinity Or
                                   1L << MetaStatusBit.NegativeInfinity Or
                                   1L << MetaStatusBit.FailedContactCheck Or
                                   1L << MetaStatusBit.HitLevelCompliance Or
                                   1L << MetaStatusBit.HitRangeCompliance Or
                                   1L << MetaStatusBit.HitVoltageProtection Or
                                   1L << MetaStatusBit.HitOverRange
    End Sub

    ''' <summary> Applies the default invalid status bitmask. </summary>
    Public Sub ApplyDefaultInvalidStatusBitmask()
        Me._ApplyDefaultInvalidStatusBitmask()
    End Sub

    ''' <summary> Returns true if the value is valid, not out of range and not failed. </summary>
    ''' <returns> <c>True</c> if the value is valid, not out of range and not failed; Otherwise, <c>False</c> </returns>
    Public Function Valid() As Boolean
        Return Me.IsValid AndAlso (Me.Value And Me.InvalidStatusBitmask) = 0
    End Function

    Private _Value As Long

    ''' <summary> Holds the meta status value. </summary>
    ''' <value> The meta status value. </value>
    Public ReadOnly Property Value() As Long
        Get
            Return Me._Value
        End Get
    End Property

#End Region

End Class

