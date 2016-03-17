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

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Contracts this class setting the meta status to 0. </summary>
    Public Sub New()
        MyBase.New()
        Me.Reset()
    End Sub

    ''' <summary> Constructs a copy of an existing value. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As MetaStatus)
        MyBase.New()
        If model IsNot Nothing Then
            Me._Value = model._Value
        End If
    End Sub

#End Region

#Region " METHODS "

    ''' <summary> Clears the meta status. </summary>
    Public Sub Reset()
        Me._Value = 0
    End Sub

    ''' <summary> Presets the meta status. </summary>
    Public Sub Preset(ByVal value As Long)
        Me._Value = value
    End Sub

#End Region

#Region " PROPERTIES "

    ''' <summary> Returns a short description representing the measurand meta status. </summary>
    ''' <returns> The short meta status description. </returns>
    Public Function ToShortDescription(ByVal passedValue As String) As String
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
            result = "c"
        ElseIf Me.HitRangeCompliance Then
            result = "rc"
        ElseIf Me.HitLevelCompliance Then
            result = "lc"
        ElseIf Me.HitOverRange Then
            result = "or"
        ElseIf Me.HitVoltageProtection Then
            result = "vp"
        Else
            ' if we do not return a value, raise an exception
            Debug.Assert(Not Debugger.IsAttached, "Unhandled case in determining invalid short outcome.")
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
            result = "failed contact check"
        ElseIf Me.HitCompliance Then
            result = "Compliance"
        ElseIf Me.HitRangeCompliance Then
            result = "Range compliance"
        ElseIf Me.HitLevelCompliance Then
            result = "Level compliance"
        ElseIf Me.HitOverRange Then
            result = "Over range"
        ElseIf Me.HitVoltageProtection Then
            result = "Voltage protection"
        Else
            ' if we do not return a value, raise an exception
            Debug.Assert(Not Debugger.IsAttached, "Unhandled case in determining invalid short outcome.")
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

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that
    ''' hit Status compliance. Status compliance is reported by the instrument in the status word. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBit.HitStatusCompliance"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property HitStatusCompliance() As Boolean
        Get
            Return Me.IsBit(MetaStatusBit.HitStatusCompliance)
        End Get
        Set(ByVal value As Boolean)
            Me.ToggleBit(MetaStatusBit.HitStatusCompliance, value)
            Me.ToggleBit(MetaStatusBit.Valid, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that
    ''' Hit Range Compliance. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBit.HitRangeCompliance"/> was set; Otherwise, <c>False</c>. </returns>
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
        Return (value > 0) AndAlso (Me.Value > 0) AndAlso ((1 And (Me.Value >> value)) = 1)
    End Function

    ''' <summary> Toggles the bits specified in the <paramref name="value"/> based on the
    ''' <paramref name="switch"/>. </summary>
    ''' <param name="value">  The value. </param>
    ''' <param name="switch"> True to set; otherwise, clear. </param>
    Public Sub ToggleBit(ByVal value As MetaStatusBit, ByVal switch As Boolean)
        If switch Then
            Me._Value = Me._Value Or (1L << value)
        Else
            Me._Value = Me._Value And (Not (1L << value))
        End If
        If Not (value = MetaStatusBit.Pass OrElse value = MetaStatusBit.HasValue) Then
            Me.ToggleBit(MetaStatusBit.Pass, Me.Passed)
            Me.ToggleBit(MetaStatusBit.HasValue, True)
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

    ''' <summary> Returns True if passed. </summary>
    ''' <retuns> <c>True</c> if the status <see cref="IsValid"/> and 
    '''          not <see cref="FailedContactCheck"/> and
    '''          not <see cref="HitCompliance"/> and
    '''          not <see cref="HitRangeCompliance"/> and
    '''          not <see cref="HitVoltageProtection"/> and
    '''          not <see cref="HitOverRange"/> and
    '''          not <see cref="IsLow"/> and
    '''          not <see cref="IsHigh"/> and
    '''          </retuns>
    Public Function Passed() As Boolean
        Return Me.IsValid AndAlso
            (Not (Me.FailedContactCheck OrElse Me.HitCompliance OrElse Me.HitRangeCompliance OrElse
            Me.HitVoltageProtection OrElse Me.HitOverRange)) AndAlso
            (Not (Me.IsHigh OrElse Me.IsLow))
    End Function

    ''' <summary> Returns true if the value is valid, not out of range and not failed. </summary>
    ''' <returns> <c>True</c> if the value is valid, not out of range and not failed; Otherwise, <c>False</c> </returns>
    Public Function Valid() As Boolean
        Return Me.IsBit(MetaStatusBit.Valid) AndAlso Not (
               Me.IsBit(MetaStatusBit.NotANumber) OrElse
               Me.IsBit(MetaStatusBit.Infinity) OrElse
               Me.IsBit(MetaStatusBit.NegativeInfinity) OrElse
               Me.IsBit(MetaStatusBit.FailedContactCheck) OrElse
               Me.IsBit(MetaStatusBit.HitLevelCompliance) OrElse
               Me.IsBit(MetaStatusBit.HitRangeCompliance) OrElse
               Me.IsBit(MetaStatusBit.HitVoltageProtection) OrElse
               Me.IsBit(MetaStatusBit.HitOverRange))
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

